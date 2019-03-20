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
using System.Globalization;


public partial class LibraryMod_periodicalmaster : System.Web.UI.Page
{
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    DAccess2 da = new DAccess2();
    DAccess2 d1 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet searchb = new DataSet();
    DataSet sp = new DataSet();
    DataTable dtCommon = new DataTable();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    Hashtable ht = new Hashtable();
    string usercollegecode = string.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string groupusercode = string.Empty;
    string collegecode = string.Empty;
    string qrycollegecode = string.Empty;
    string qryUserOrGroupCode = string.Empty;
    string libr_name = string.Empty;
    string qrylibname = string.Empty;
    string l_code, is_natio, i_active, iss_by, i_type, language1;
    string addedtype = string.Empty;
    string li_code = string.Empty;
    bool cellflag = false;
    static string searchclgcode = string.Empty;
    static string searchlibcode = string.Empty;

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
          
                usercollegecode = Convert.ToString(Session["collegecode"]).Trim();
                usercode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
                singleuser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
                groupusercode = (Session["group_user"] != null) ? Convert.ToString(Session["group_user"]).Trim() : "";
                collegecode = Session["Collegecode"].ToString();
          
            if (!IsPostBack)
            {
                bindclg();
                getLibPrivil();
                bindtype();
                bindjournaltype();
                chkSubscribe.Checked = false;
                bindyear();
                list();
                publis();
                dept();
                binjournalty();
                periodi();
                deliver();
                supp();
                subj();
                addtbl.Visible = false;
                if (chkSubscribe.Checked == false)
                {
                    ddlYear.Enabled = false;
                }
                else
                {
                    ddlYear.Enabled = true;
                }
                ddlcurrencytype.Attributes.Add("onfocus", "frelig1()");
                ddldepartment.Attributes.Add("onfocus", "frelig2()");
                ddlpublish.Attributes.Add("onfocus", "frelig3()");
                ddljour.Attributes.Add("onfocus", "frelig4()");
                ddlsubject.Attributes.Add("onfocus", "frelig5()");
                ddllanguage.Attributes.Add("onfocus", "frelig6()");
                ddldevt.Attributes.Add("onfocus", "frelig7()");
                ddlpri.Attributes.Add("onfocus", "frelig8()");
                ddlccountry.DataSource = countrylist();
                ddlccountry.DataBind();
            }
        }
        catch
        {
        }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getsearchtitle(string prefixText)
    {
        string query = "";
        WebService ws = new WebService();
        List<string> values = new List<string>();





        if (searchlibcode != "All")
            query = "SELECT Journal_name FROM Journal_Master where Journal_name Like '" + prefixText + "%' AND Lib_Code ='" + searchlibcode + "'";
        else
            query = "SELECT Journal_name FROM Journal_Master where Journal_name Like '" + prefixText + "%' ";

        values = ws.Getname(query);
        return values;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getsearchjournalcode(string prefixText)
    {
        string query = "";
        WebService ws = new WebService();
        List<string> values = new List<string>();





        if (searchlibcode != "All")
            query = "SELECT Journal_Code FROM Journal_Master where Journal_Code Like '" + prefixText + "%' AND Lib_Code ='" + searchlibcode + "'";
        else
            query = "SELECT Journal_Code FROM Journal_Master where Journal_Code Like '" + prefixText + "%' ";

        values = ws.Getname(query);
        return values;
    }

    #region BindHeaders

    public void bindclg()
    {
        try
        {

            ddlclg.Items.Clear();
            dtCommon.Clear();

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
                ddlclg.DataSource = dtCommon;
                ddlclg.DataTextField = "collname";
                ddlclg.DataValueField = "college_code";
                ddlclg.DataBind();
                ddlclg.SelectedIndex = 0;
                ddlclg.Enabled = true;

                searchclgcode = Convert.ToString(ddlclg.SelectedValue);
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
            ds.Clear();

            if (ddlclg.Items.Count > 0)
            {
                collegecode = string.Empty;
                foreach (ListItem li in ddlclg.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collegecode))
                        {
                            collegecode = "'" + li.Value + "'";
                        }
                        else
                        {
                            collegecode = ",'" + li.Value + "'";
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(collegecode))
            {
                string lib_name = "select lib_code,lib_name,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + LibCollection + " AND college_code=" + collegecode + " ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
                ds = da.select_method_wo_parameter(lib_name, "text");
                libr_name = ds.Tables[0].Rows[0]["lib_name"].ToString();
            }
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

    public void bindissueBy()
    {
        ddlissueby.Items.Clear();
        ds.Clear();
        string selectedval = string.Empty;
    }

    public void bindtype()
    {
        try
        {
        }
        catch
        {
        }
    }

    public void searchby()
    {
        ddlsearchby.Items.Clear();
    }

    public void list()
    {
        ddlis.Items.Clear();
        ddlis.Items.Add("Daily");
        ddlis.Items.Add("Weekly");
        ddlis.Items.Add("Monthly");
        ddlis.Items.Add("Yearly");
        ddlis.Items.Add("Others");
    }

    public void rbradioclick()
    {

    }

    public void bindyear()
    {
        //ddlYear.Items.Clear();
        //ds.Clear();
        //if (!string.IsNullOrEmpty(collegecode))
        //{
        //    string yer = "select distinct s.Subscription_Year from subscription s,library l where l.college_code=" + collegecode + " and l.lib_code=s.lib_code";
        //    ds = da.select_method_wo_parameter(yer, "text");
        //}
        //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //{
        //    ddlYear.DataSource = ds;
        //    ddlYear.DataTextField = "Subscription_Year";
        //    ddlYear.DataValueField = "Subscription_Year";
        //    ddlYear.DataBind();
        //    ddlYear.SelectedIndex = 0;
        //}
        for (int intYear = DateTime.Now.Year - 20; intYear <= DateTime.Now.Year + 20; intYear++)
        {
            ddlYear.Items.Add(intYear.ToString());
        }

        //Make the current year selected item in the list
        ddlYear.Items.FindByValue(DateTime.Now.Year.ToString()).Selected = true;

    }

    public void bindjournaltype()
    {
        try
        {

            ds.Clear();
            if (ddlclg.Items.Count > 0)
            {
                collegecode = string.Empty;
                foreach (ListItem li in ddlclg.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collegecode))
                        {
                            collegecode = "'" + li.Value + "'";
                        }
                        else
                        {
                            collegecode = ",'" + li.Value + "'";
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(collegecode))
            {
                string journ_type = "select distinct ISNULL(Journal_Type,'') Journal_Type FROM Journal_Master J,Library L WHERE J.Lib_Code = L.Lib_Code AND L.College_Code = " + collegecode + " ORDER BY Journal_Type";
                ds = da.select_method_wo_parameter(journ_type, "text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddljournaltype.DataSource = ds;
                ddljournaltype.DataTextField = "Journal_Type";
                ddljournaltype.DataValueField = "Journal_Type";
                ddljournaltype.DataBind();

            }
            ddljournaltype.Items.Insert(0, "All");
        }
        catch
        {
        }


    }

    public void languagesearch()
    {

        try
        {

            ds.Clear();
            if (ddlclg.Items.Count > 0)
            {
                collegecode = string.Empty;
                foreach (ListItem li in ddlclg.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collegecode))
                        {
                            collegecode = "'" + li.Value + "'";
                        }
                        else
                        {
                            collegecode = ",'" + li.Value + "'";
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(collegecode))
            {
                string lang = "SELECT DISTINCT ISNULL(Lang,'') Lang FROM Journal_Master J,Library L  WHERE J.Lib_Code= L.Lib_Code AND L.College_Code =" + collegecode + "";
                ds = da.select_method_wo_parameter(lang, "text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlsearch.DataSource = ds;
                ddlsearch.DataTextField = "Lang";
                ddlsearch.DataValueField = "Lang";
                ddlsearch.DataBind();

            }
        }
        catch
        {
        }

    }

    #endregion

    protected void ddlclg_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            getLibPrivil();
            searchclgcode = Convert.ToString(ddlclg.SelectedValue);
        }
        catch
        {
        }

    }

    protected void ddllibrary_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            searchlibcode = Convert.ToString(ddllibrary.SelectedValue);
        }
        catch
        {
        }
    }

    protected void ddlissueby_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch
        {
        }
    }

    protected void ddlpubby_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void ddlsearchby_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            if (ddlsearchby.SelectedIndex == 0)
            {
                Txtsearchby.Visible = true;

                ddlsearch.Visible = false;
            }
            else if (ddlsearchby.SelectedIndex == 1)
            {
                Txtsearchby.Visible = false;

                ddlsearch.Visible = true;
                languagesearch();
            }
            else if (ddlsearchby.SelectedIndex == 2)
            {
                ddlsearch.Items.Insert(0, "English");
                ddlsearch.Items.Insert(1, "Tamil");
            }

            else if (ddlsearchby.SelectedIndex == 3)
            {
                Txtsearchby.Visible = false;
                ddlsearch.Visible = false;

            }
        }
        catch
        {
        }

    }

    protected void ddljournaltype_SelectedIndexChanged(object sender, EventArgs e)
    {


    }

    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch
        {
        }
    }

    protected void chkSub_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSubscribe.Checked == true)
        {
            ddlYear.Enabled = true;
            lblsearchby.Visible = false;
            ddlsearchby.Visible = false;
            Txtsearchby.Visible = false;
        }
        else
        {
            ddlYear.Enabled = false;
        }
    }

    protected void ddlYear_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }

    #region ButtonClick

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            collegecode = string.Empty;
            DataSet lang1 = new DataSet();
            qrycollegecode = string.Empty;
            libr_name = string.Empty;
            qrylibname = string.Empty;
            string issueby = string.Empty;
            string publtype = string.Empty;
            string sql = string.Empty;
            string j_code = string.Empty;
            string journa_lang = string.Empty;
            string search_lang = string.Empty;
            string activ = string.Empty;

            sql = "SELECT Journal_Code,Journal_Name,Periodicity,CASE WHEN ISNULL(IssueBy,0) = 1 THEN 'Daily' WHEN ISNULL(IssueBy,0) = 2 THEN 'Weekly' WHEN ISNULL(IssueBy,0) = 3 THEN 'Monthly' WHEN ISNULL(IssueBy,0) = 4 THEN 'Yearly' ELSE 'Others' END IssueBy,ISNULL(TotalNoIssues,0)TotalNoIssues ,ISNULL(Department,'') Department,ISNULL(Subject,'') Subject,CASE WHEN ISNULL(Is_National,1) = 1 THEN 'National' ELSE 'International' END PubType,ISNULL(TamilJrnlName,'') TamilJrnlName,ISNULL(TitleLanguage,0) TitleLanguage,year FROM Journal_Master WHERE 1=1";

            if (ddllibrary.SelectedIndex != 0)
            {
                sql = sql + " AND Lib_Code ='" + Convert.ToString(ddllibrary.SelectedValue) + "' ";
            }
            if (ddlissueby.SelectedIndex != 4)
            {
                sql = sql + "  AND IssueBY ='" + Convert.ToString(ddlissueby.SelectedValue) + "'";
            }
            if (ddlpubtype.SelectedIndex == 0)
            {
                sql = sql + " AND ISNULL(Is_National,1) = 'True'";
            }
            else if (ddlpubtype.SelectedIndex == 1)
            {
                sql = sql + " AND ISNULL(Is_National,1) = 'False'";
            }
            if (txttitle.Text != "")
            {
                sql = sql + " AND Journal_Name LIKE '%" + txttitle.Text + "%'";
            }
            if (ddljournaltype.SelectedIndex != 0)
            {
                sql = sql + " AND ISNULL(Journal_Type,'') ='" + ddljournaltype.SelectedItem.ToString() + "' ";
            }
            if (ddlsearchby.SelectedIndex == 1)
            {
                sql = sql + " AND Lang LIKE '" + ddlsearch.SelectedItem.ToString() + "%'";
            }
            else if (ddlsearchby.SelectedIndex == 2)
            {
                sql = sql + " AND TitleLanguage ='" + ddlsearch.SelectedIndex.ToString() + "'";
            }
            else if (ddlsearchby.SelectedIndex == 0)
            {
                sql = sql + " AND Journal_Code ='" + Txtsearchby.Text + "' ";
            }
            if (rblStatus.SelectedIndex == 0)
            {
                sql = sql + " AND ISNULL(IsActive,0) = 'True'";
            }
            else if (rblStatus.SelectedIndex == 1)
            {
                sql = sql + " AND ISNULL(IsActive,0) = 'False'";
            }
            if (chkSubscribe.Checked == true)
            {
                sql = sql + " AND Journal_Code IN (SELECT Journal_Code FROM Subscription WHERE Subscription_Year ='" + ddlYear.SelectedItem.ToString() + "')";
            }
            if (ddlType.SelectedIndex == 1)
            {
                sql = sql + " AND ISNULL(PeriodicalType,1) = 1";
            }
            else if (ddlType.SelectedIndex == 0)
            {
                sql = sql + " AND ISNULL(PeriodicalType,1) = 2";
            }

            sql = sql + " ORDER BY LEN(Journal_Code),Journal_Code";
            sp = d1.select_method_wo_parameter(sql, "text");
            DataTable dtPeriodical = new DataTable();
            DataRow drow;
            int sno = 0;
            if (sp.Tables.Count > 0 && sp.Tables[0].Rows.Count > 0)
            {
                dtPeriodical.Columns.Add("SNo", typeof(string));
                dtPeriodical.Columns.Add("Journal Code", typeof(string));
                dtPeriodical.Columns.Add("Title", typeof(string));
                dtPeriodical.Columns.Add("Prioridicity", typeof(string));
                dtPeriodical.Columns.Add("Issue Type", typeof(string));
                dtPeriodical.Columns.Add("Total Issues", typeof(string));
                dtPeriodical.Columns.Add("Year", typeof(string));
                dtPeriodical.Columns.Add("Department", typeof(string));
                dtPeriodical.Columns.Add("Subject", typeof(string));
                dtPeriodical.Columns.Add("Publish Type", typeof(string));

                //drow = dtPeriodical.NewRow();
                //drow["SNo"] = "SNo";
                //drow["Journal Code"] = "Access No";
                //drow["Title"] = "Title";
                //drow["Prioridicity"] = "Prioridicity";
                //drow["Issue Type"] = "Issue Type";
                //drow["Total Issues"] = "Year";
                //drow["Year"] = "Borrow Date";
                //drow["Department"] = "Department";
                //drow["Subject"] = "Subject";
                //drow["Publish Type"] = "Publish Type";
                //dtPeriodical.Rows.Add(drow);

                for (int i = 0; i < sp.Tables[0].Rows.Count; i++)
                {
                    sno++;
                    string journtype = Convert.ToString(sp.Tables[0].Rows[i]["journal_code"]).Trim();
                    string jname = Convert.ToString(sp.Tables[0].Rows[i]["Journal_name"]).Trim();
                    string periodicity = Convert.ToString(sp.Tables[0].Rows[i]["Periodicity"]).Trim();
                    string issby = Convert.ToString(sp.Tables[0].Rows[i]["Issueby"]).Trim();
                    string totissue = Convert.ToString(sp.Tables[0].Rows[i]["TotalNoIssues"]).Trim();
                    string yr = Convert.ToString(sp.Tables[0].Rows[i]["year"]).Trim();
                    string dept = Convert.ToString(sp.Tables[0].Rows[i]["Department"]).Trim();
                    string sub = Convert.ToString(sp.Tables[0].Rows[i]["Subject"]).Trim();
                    string publishtype = Convert.ToString(sp.Tables[0].Rows[i]["PubType"]).Trim();

                    drow = dtPeriodical.NewRow();
                    drow["SNo"] = Convert.ToString(sno);
                    drow["Journal Code"] = journtype;
                    drow["Title"] = jname;
                    drow["Prioridicity"] = periodicity;
                    drow["Issue Type"] = issby;
                    drow["Total Issues"] = totissue;
                    drow["Year"] = yr;
                    drow["Department"] = dept;
                    drow["Subject"] = sub;
                    drow["Publish Type"] = publishtype;
                    dtPeriodical.Rows.Add(drow);
                }
                grdperiodical.DataSource = dtPeriodical;
                grdperiodical.DataBind();
                grdperiodical.Visible = true;
                divtable.Visible = true;
                btn_printmaster.Visible = true;
                btn_Excel.Visible = true;
                txt_excelname.Visible = true;
                div_report.Visible = true;
                lbl_reportname.Visible = true;
            }
            else
            {
                divPopupAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No Records Found";
                grdperiodical.Visible = false;
                divtable.Visible = false;
                btn_printmaster.Visible = false;
                btn_Excel.Visible = false;
                txt_excelname.Visible = false;
                div_report.Visible = false;
                lbl_reportname.Visible = false;
            }

        }
        catch (Exception ex) { da.sendErrorMail(ex, collegecode, "periodicalmaster.aspx"); }
    }



    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            lblAlertMsg.Visible = false;
            divPopupAlert.Visible = false;
            lblAlertMsg.Text = string.Empty;
        }
        catch (Exception ex) { da.sendErrorMail(ex, collegecode, "periodicalmaster.aspx"); }
    }

    protected void btnadd_Click(object sender, EventArgs e)
    {
        try
        {
            addtbl.Visible = true;
            divsaventry.Visible = true;
            getLibPrivil();
            publis();
            dept();
            binjournalty();
            periodi();
            deliver();
            supp();
            subj();
            language3();
            Currencytype();
            txtsubsam.Text = "0.00";
            list();
            btndelete.Visible = false;
            clear();
        }
        catch (Exception ex) { da.sendErrorMail(ex, collegecode, "periodicalmaster.aspx"); }
    }

    protected void Btnpersave_Click(object sender, EventArgs e)
    {
        try
        {
            string curda = string.Empty;
            string curti = string.Empty;
            double dd_amt = 0.00;
            string tydate = string.Empty;
            curda = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            curti = DateTime.Now.ToString("HH:mm:ss");
            tydate = DateTime.Now.ToString("MMM;/dd");

            string licode = "select lib_code from library where lib_name='" + ddllibararyname.SelectedItem.Text + "' and college_code=" + collegecode + "";
            sp.Clear();
            sp = d1.select_method_wo_parameter(licode, "text");
            if (sp.Tables.Count > 0 && sp.Tables[0].Rows.Count > 0)
            {
                l_code = sp.Tables[0].Rows[0]["lib_code"].ToString();
            }
            if (rblpubty.SelectedValue == "National Periodical")
            {
                is_natio = "1";
            }
            else
            {
                is_natio = "2";
            }
            if (rbljrweb.SelectedValue == "Active")
            {
                i_active = "1";
            }
            else
            {
                i_active = "0";
            }
            if (ddlis.SelectedValue == "Daily")
            {
                iss_by = "1";
            }
            if (ddlis.SelectedValue == "Weekly")
            {
                iss_by = "2";
            }
            if (ddlis.SelectedValue == "Monthly")
            {
                iss_by = "3";
            }
            if (ddlis.SelectedValue == "Yearly")
            {
                iss_by = "4";

            }
            if (rblisty.SelectedValue == "Datewise")
            {
                i_type = "1";
            }
            if (rblisty.SelectedValue == "Daywise")
            {
                i_type = "2";
            }
            if (rblisty.SelectedValue == "Monthwise")
            {
                i_type = "3";
            }
            if (rblisty.SelectedValue == "For Every")
            {
                i_type = "4";
            }
            if (ddlengtam.SelectedValue == "English")
            {
                language1 = "0";
            }
            else
            {
                language1 = "1";
            }

            string countries = Convert.ToString(ddlccountry.SelectedItem);

            string qry1 = "INSERT INTO Journal_Master(access_date,access_time,journal_code,journal_name,remarks,used_flag,Journal_type,lang,lib_code,Periodicity,Periodicity_value,journal_price,department,rackno,row_no,publisher,is_national,currency_type,currency_value,subject,F_Issue,L_Issue,A_Status,G_Period,DD_No,DD_Date,In_Favour,DD_Amount,Active,ISSNNo,DeliveryType,Pos_No,Pos_Place,IssueBy,PerIssueNo,TotalNoIssues,IssueType,IssueTypeVAl,Journal_Website,TamilJrnlName,IssueByDays,Supplier,SubsAmount,TitleLanguage,IsActive,PeriodicalType,country,year) values('" + curda + "','" + curti + "','" + txtpercode.Text + "','" + txtpertitle.Text + "','" + txtremark.Text + "','" + "" + "','" + Convert.ToString(ddljour.SelectedItem) + "','" + Convert.ToString(ddllanguage.SelectedItem) + "','" + l_code + "','" + Convert.ToString(ddlpri.SelectedItem) + "','" + " " + "','" + Txtindaianprice.Text + "','" + Convert.ToString(ddldepartment.SelectedItem) + "','" + "" + "','" + "" + "','" + Convert.ToString(ddlpublish.SelectedItem) + "','" + Convert.ToInt32(is_natio) + "','" + Convert.ToString(ddlcurrencytype.SelectedItem) + "','" + txtcurrencyvalue.Text + "','" + Convert.ToString(ddlsubject.SelectedItem) + "','" + curda + "','" + curda + "','" + "" + "','" + "" + "','" + "" + "','" + curda + "','" + "" + "','" + dd_amt + "','" + i_active + "','" + txtissn.Text + "','" + Convert.ToString(ddldevt.SelectedItem) + "','" + "" + "','" + "" + "','" + Convert.ToInt32(iss_by) + "','" + txtperis.Text + "','" + txttotnois.Text + "','" + i_type + "','" + txtdays.Text + "','" + txtjoweb.Text + "','" + "" + "','" + txtdays.Text + "','" + Convert.ToString(ddlsupp.SelectedItem) + "','" + txtsubsam.Text + "','" + language1 + "','" + Convert.ToInt32(i_active) + "','" + Convert.ToInt32(is_natio) + "','" + countries + "','" + txtyear.Text + "')";

            int insertqry = d1.update_method_wo_parameter(qry1, "text");
            if (insertqry == 0)
            {
                divPopupAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Records Not Saved";
                grdperiodical.Visible = false;
                addtbl.Visible = false;
                div_report.Visible = false;
                clear();
            }
            else
            {
                divPopupAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Records Saved Successfully";
                grdperiodical.Visible = false;
                addtbl.Visible = false;
                div_report.Visible = false;
                clear();
            }
        }
        catch (Exception ex) { da.sendErrorMail(ex, collegecode, "periodicalmaster.aspx"); }
    }

    public void clear()
    {
        //txtpercode.Text = "";
        txttitle.Text = "";
        txtcurrencyvalue.Text = "";
        Txtindaianprice.Text = "";
        txtissn.Text = "";
        txtperis.Text = "";
        //txtdays.Text = "";
        txttotnois.Text = "";
        txtremark.Text = "";
        txtjoweb.Text = "";
        txtpertitle.Text = "";
    }

    protected void btnperclose_Click(object sender, EventArgs e)
    {
        addtbl.Visible = false;
        divsaventry.Visible = false;
        txtpercode.Text = "";
        clear();
    }

    protected void btnaddlanguage_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = true;
        panel_addgroup.Visible = true;
        lbl_addgroup.Text = "Language";
        txt_addgroup.Attributes.Add("placeholder", "");
        txt_addgroup.Attributes.Add("maxlength", "150");
        lblerror.Visible = false;
    }

    protected void btnsublanguage_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddllanguage.Items.Count > 0)
                addedtype = Convert.ToString(ddllanguage.Text);
            if (ddllibararyname.Items.Count > 0)
                li_code = Convert.ToString(ddllibararyname.SelectedValue);
            if (addedtype != "")
            {
                string get = da.GetFunction("select count(Lang ) Lang  from Journal_Master where Lang ='" + addedtype + "' and  Lib_Code ='" + li_code + "'");
                int getcnt = Convert.ToInt32(get);
                if (getcnt > 0)
                {
                    divAlertContent.Visible = true;
                    divPopupAlert.Visible = true;
                    btnPopAlertClose.Visible = true;
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "Language is Available in Journal master.So it Cannot be deleted.";
                    return;
                }
                else
                {
                    language3();
                }
            }
            else
            {
                language3();
            }
        }
        catch (Exception ex) { da.sendErrorMail(ex, collegecode, "periodicalmaster.aspx"); }
    }

    protected void btnaddsubject_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = true;
        panel_addgroup.Visible = true;
        lbl_addgroup.Text = "Subject";
        txt_addgroup.Attributes.Add("placeholder", "");
        txt_addgroup.Attributes.Add("maxlength", "150");
        lblerror.Visible = false;
    }

    protected void Btnsubsubject_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlsubject.Items.Count > 0)
                addedtype = Convert.ToString(ddlsubject.Text);
            if (ddllibararyname.Items.Count > 0)
                li_code = Convert.ToString(ddllibararyname.SelectedValue);
            if (addedtype != "")
            {
                string get = da.GetFunction("select count(Subject ) Subject  from Journal_Master where Subject ='" + addedtype + "' and  Lib_Code ='" + li_code + "'");
                int getcnt = Convert.ToInt32(get);
                if (getcnt > 0)
                {
                    divAlertContent.Visible = true;
                    divPopupAlert.Visible = true;
                    btnPopAlertClose.Visible = true;
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "Subject is Available in Journal master.So it Cannot be deleted.";
                    return;

                }
                else
                {
                    subj();
                }
            }
            else
            {
                subj();
            }
        }
        catch (Exception ex) { da.sendErrorMail(ex, collegecode, "periodicalmaster.aspx"); }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string report = txt_excelname.Text;
            if (report.ToString().Trim() != "")
            {
                da.printexcelreportgrid(grdperiodical, report);
                lbl_norec.Visible = false;
            }
            else
            {
                lbl_norec.Text = "Please Enter Your Report Name";
                lbl_norec.Visible = true;
            }
            btn_Excel.Focus();
        }
        catch (Exception ex) { da.sendErrorMail(ex, collegecode, "periodicalmaster.aspx"); }
    }

    protected void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string attendance = "Periodical Master";
            string pagename = "periodicalmaster.aspx";
            string ss = Session["usercode"].ToString();
            Printcontrolhed2.loadspreaddetails(grdperiodical, pagename, attendance, 0, ss, "1");
            Printcontrolhed2.Visible = true;
        }
        catch { }
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

    protected void btnaddtypecurrency_Click(object sender, EventArgs e)
    {

        try
        {
            plusdiv.Visible = true;
            panel_addgroup.Visible = true;
            lbl_addgroup.Text = "Currency";
            txt_addgroup.Attributes.Add("placeholder", "");
            txt_addgroup.Attributes.Add("maxlength", "150");
            lblerror.Visible = false;

        }
        catch
        {

        }

    }

    protected void btnsubcurrencytype_Click(object sender, EventArgs e)
    {

        try
        {
            if (ddlcurrencytype.Items.Count > 0)
                addedtype = Convert.ToString(ddlcurrencytype.Text);
            if (ddllibararyname.Items.Count > 0)
                li_code = Convert.ToString(ddllibararyname.SelectedValue);
            if (addedtype != "")
            {
                string get = da.GetFunction("select count(Currency_Type ) Currency_Type  from Journal_Master where Currency_Type ='" + addedtype + "' and  Lib_Code ='" + li_code + "'");
                int getcnt = Convert.ToInt32(get);
                if (getcnt > 0)
                {
                    divAlertContent.Visible = true;
                    divPopupAlert.Visible = true;
                    btnPopAlertClose.Visible = true;
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "Currency_Type is Available in Journal master.So it Cannot be deleted.";
                    return;

                }
                else
                {
                    Currencytype();
                }
            }
            else
            {
                Currencytype();
            }




        }


        catch
        {
        }

    }

    protected void btnadddepartment_Click(object sender, EventArgs e)
    {

        try
        {
            plusdiv.Visible = true;
            panel_addgroup.Visible = true;
            lbl_addgroup.Text = "Department";
            txt_addgroup.Attributes.Add("placeholder", "");
            txt_addgroup.Attributes.Add("maxlength", "150");
            lblerror.Visible = false;

        }
        catch
        {
        }

    }

    protected void btnsubdepartment_Click(object sender, EventArgs e)
    {

        try
        {
            if (ddldepartment.Items.Count > 0)
                addedtype = Convert.ToString(ddldepartment.Text);
            if (ddllibararyname.Items.Count > 0)
                li_code = Convert.ToString(ddllibararyname.SelectedValue);
            if (addedtype != "")
            {
                string get = da.GetFunction("select count(Dept_Name ) Dept_Name  from Journal_Dept where Dept_Name ='" + addedtype + "' and  Lib_Code ='" + li_code + "'");
                int getcnt = Convert.ToInt32(get);
                if (getcnt > 0)
                {
                    divAlertContent.Visible = true;
                    divPopupAlert.Visible = true;
                    btnPopAlertClose.Visible = true;
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "Department is Available in Journal Department.So it Cannot be deleted.";
                    return;

                }
                else
                {
                    dept();
                }
            }
            else
            {
                dept();
            }




        }


        catch
        {
        }

    }

    protected void btnadddpub_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = true;
        panel_addgroup.Visible = true;
        lbl_addgroup.Text = "Publisher";
        txt_addgroup.Attributes.Add("placeholder", "");
        txt_addgroup.Attributes.Add("maxlength", "150");
        lblerror.Visible = false;

    }

    protected void btnsubpub_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlpublish.Items.Count > 0)
                addedtype = Convert.ToString(ddlpublish.Text);
            if (ddllibararyname.Items.Count > 0)
                li_code = Convert.ToString(ddllibararyname.SelectedValue);
            if (addedtype != "")
            {
                string get = da.GetFunction("select count(Publisher ) Publisher  from Journal_Master where Publisher ='" + addedtype + "' and  Lib_Code ='" + li_code + "'");
                int getcnt = Convert.ToInt32(get);
                if (getcnt > 0)
                {
                    divAlertContent.Visible = true;
                    divPopupAlert.Visible = true;
                    btnPopAlertClose.Visible = true;
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "Publisher is Available in Journal master.So it Cannot be deleted.";
                    return;

                }
                else
                {
                    publis();
                }
            }
            else
            {
                publis();
            }


        }


        catch
        {
        }

    }

    protected void btnaddjourtype_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = true;
        panel_addgroup.Visible = true;
        lbl_addgroup.Text = "Journal Type";
        txt_addgroup.Attributes.Add("placeholder", "");
        txt_addgroup.Attributes.Add("maxlength", "150");
        lblerror.Visible = false;

    }

    protected void btnsubjourtype_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddljour.Items.Count > 0)
                addedtype = Convert.ToString(ddljour.Text);
            if (ddllibararyname.Items.Count > 0)
                li_code = Convert.ToString(ddllibararyname.SelectedValue);
            if (addedtype != "")
            {
                string get = da.GetFunction("select count(Journal_Type ) Journal_Type  from Journal_Master where Journal_Type ='" + addedtype + "' and  Lib_Code ='" + li_code + "'");
                int getcnt = Convert.ToInt32(get);
                if (getcnt > 0)
                {
                    divAlertContent.Visible = true;
                    divPopupAlert.Visible = true;
                    btnPopAlertClose.Visible = true;
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "Journal_Type is Available in Journal master.So it Cannot be deleted.";
                    return;

                }
                else
                {
                    binjournalty();
                }
            }
            else
            {
                binjournalty();
            }


        }


        catch
        {
        }
    }

    protected void btnsubdel_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddldevt.Items.Count > 0)
                addedtype = Convert.ToString(ddldevt.Text);
            if (ddllibararyname.Items.Count > 0)
                li_code = Convert.ToString(ddllibararyname.SelectedValue);
            if (addedtype != "")
            {
                string get = da.GetFunction("select count(DeliveryType ) DeliveryType  from Journal_Master where DeliveryType ='" + addedtype + "' and  Lib_Code ='" + li_code + "'");
                int getcnt = Convert.ToInt32(get);
                if (getcnt > 0)
                {
                    divAlertContent.Visible = true;
                    divPopupAlert.Visible = true;
                    btnPopAlertClose.Visible = true;
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "DeliveryType is Available in Journal master.So it Cannot be deleted.";
                    return;

                }
                else
                {
                    deliver();
                }
            }
            else
            {
                deliver();
            }


        }


        catch
        {
        }
    }

    protected void btnadddeliv_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = true;
        panel_addgroup.Visible = true;
        lbl_addgroup.Text = "Delivery Type";
        txt_addgroup.Attributes.Add("placeholder", "");
        txt_addgroup.Attributes.Add("maxlength", "150");
        lblerror.Visible = false;
    }

    protected void btnsubperiod_Click(object sender, EventArgs e)
    {

        try
        {
            if (ddlpri.Items.Count > 0)
                addedtype = Convert.ToString(ddlpri.Text);
            if (ddllibararyname.Items.Count > 0)
                li_code = Convert.ToString(ddllibararyname.SelectedValue);
            if (addedtype != "")
            {
                string get = da.GetFunction("select count(Periodicity ) Periodicity  from Journal_Master where Periodicity ='" + addedtype + "' and  Lib_Code ='" + li_code + "'");
                int getcnt = Convert.ToInt32(get);
                if (getcnt > 0)
                {
                    divAlertContent.Visible = true;
                    divPopupAlert.Visible = true;
                    btnPopAlertClose.Visible = true;
                    lblAlertMsg.Visible = true;
                    lblAlertMsg.Text = "Periodicity is Available in Journal master.So it Cannot be deleted.";
                    return;

                }
                else
                {
                    periodi();
                }
            }
            else
            {
                periodi();
            }


        }


        catch
        {
        }
    }

    protected void btnaddperiod_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = true;
        panel_addgroup.Visible = true;
        lbl_addgroup.Text = "Prioridicity";
        txt_addgroup.Attributes.Add("placeholder", "");
        txt_addgroup.Attributes.Add("maxlength", "150");
        lblerror.Visible = false;
    }

    protected void btn_addgroup_Click(object sender, EventArgs e)
    {
        string group = Convert.ToString(txt_addgroup.Text);
        if (group != "")
        {
            if (lbl_addgroup.Text.Trim() == "Currency")
            // ddlcurrencytype.Items[0].Text = group;
            {
                int b = ddlcurrencytype.Items.Count;

                ddlcurrencytype.Items.Insert(b, group);
            }
            else if (lbl_addgroup.Text.Trim() == "Department")
            //ddldepartment.Items[0].Text = group;
            {
                int i = ddldepartment.Items.Count;

                ddldepartment.Items.Insert(i, group);
            }
            else if (lbl_addgroup.Text.Trim() == "Publisher")
            //ddlpublish.Items[0].Text = group;
            {

                int j = ddlpublish.Items.Count;

                ddlpublish.Items.Insert(j, group);

            }
            else if (lbl_addgroup.Text.Trim() == "Journal Type")
            {

                int k = ddljour.Items.Count;

                ddljour.Items.Insert(k, group);
                ddljour.Items[0].Text = group;

            }

            else if (lbl_addgroup.Text.Trim() == "Subject")
            //ddlsubject.Items[0].Text = group;
            {

                int m = ddlsubject.Items.Count;

                ddlsubject.Items.Insert(m, group);

            }
            else if (lbl_addgroup.Text.Trim() == "Language")
            //  ddllanguage.Items[0].Text = group;
            {

                int n = ddllanguage.Items.Count;

                ddllanguage.Items.Insert(n, group);

            }
            else if (lbl_addgroup.Text.Trim() == "Delivery Type")
            //  ddldevt.Items[0].Text = group;
            {

                int s = ddldevt.Items.Count;

                ddldevt.Items.Insert(s, group);

            }
            else if (lbl_addgroup.Text.Trim() == "Prioridicity")
            // ddlpri.Items[0].Text = group;
            {
                int t = ddlpri.Items.Count;

                ddlpri.Items.Insert(t, group);
            }

            plusdiv.Visible = false;
        }
        else
        {
            plusdiv.Visible = true;
            lblerror.Visible = true;
            lblerror.Text = "Please Enter the " + lbl_addgroup.Text + "";
        }
        txt_addgroup.Text = string.Empty;

    }

    protected void btn_exitaddgroup_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = false;
        panel_addgroup.Visible = false;
        txt_addgroup.Text = "";
    }

    protected void Btnperupdate_Click(object sender, EventArgs e)
    {
        try
        {
            string curda = string.Empty;
            string curti = string.Empty;

            double dd_amt = 0.00;
            string tydate = string.Empty;
            curda = DateTime.Now.ToString("yyyy-MM-dd");
            curti = DateTime.Now.ToString("HH:mm:ss");
            tydate = DateTime.Now.ToString("MMM;/dd");

            if (txtpercode.Text == "")
            {
                lblAlertMsg.Text = "Enter The Periodical Code";
                lblAlertMsg.Visible = true;
                divPopupAlert.Visible = true;
                divAlertContent.Visible = true;
                btnPopAlertClose.Visible = true;
                return;
            }
            if (txtpertitle.Text == "")
            {
                lblAlertMsg.Text = "Enter The Title";
                lblAlertMsg.Visible = true;
                divPopupAlert.Visible = true;
                divAlertContent.Visible = true;
                btnPopAlertClose.Visible = true;
                return;
            }
            //if (txtperis.Text == "")
            //{
            //    lblAlertMsg.Text = "Enter The Per Issue";
            //    lblAlertMsg.Visible = true;
            //    divPopupAlert.Visible = true;
            //    divAlertContent.Visible = true;
            //    btnPopAlertClose.Visible = true;
            //    return;
            //}
            //if (txttotnois.Text == "")
            //{
            //    lblAlertMsg.Text = "Enter The Total Issue Number";
            //    lblAlertMsg.Visible = true;
            //    divPopupAlert.Visible = true;
            //    divAlertContent.Visible = true;
            //    btnPopAlertClose.Visible = true;
            //    return;
            //}


            double subsamount = 0;
            if (!string.IsNullOrEmpty(txtsubsam.Text))
                subsamount = Convert.ToDouble(txtsubsam.Text);

            string licode = "select lib_code from library where lib_name='" + ddllibararyname.SelectedItem.ToString() + "' and college_code=" + collegecode + "";
            sp.Clear();
            sp = d1.select_method_wo_parameter(licode, "text");
            if (sp.Tables.Count > 0 && sp.Tables[0].Rows.Count > 0)
            {
                l_code = sp.Tables[0].Rows[0]["lib_code"].ToString();
            }
            if (rblpubty.SelectedValue == "National Periodical")
            {
                is_natio = "1";

            }
            else
            {
                is_natio = "2";
            }
            if (rbljrweb.SelectedValue == "Active")
            {
                i_active = "1";

            }
            else
            {
                i_active = "0";
            }
            if (ddlis.SelectedValue == "Daily")
            {
                iss_by = "1";
            }
            if (ddlis.SelectedValue == "Weekly")
            {
                iss_by = "2";
            }
            if (ddlis.SelectedValue == "Monthly")
            {
                iss_by = "3";
            }
            if (ddlis.SelectedValue == "Yearly")
            {
                iss_by = "4";

            }
            if (rblisty.SelectedValue == "Datewise")
            {
                i_type = "1";
            }
            if (rblisty.SelectedValue == "Daywise")
            {
                i_type = "2";
            }
            if (rblisty.SelectedValue == "Monthwise")
            {
                i_type = "3";
            }
            if (rblisty.SelectedValue == "For Every")
            {
                i_type = "4";
            }
            if (ddlengtam.SelectedValue == "English")
            {
                language1 = "0";
            }
            else
            {
                language1 = "1";
            }
            string countries = Convert.ToString(ddlccountry.SelectedItem);
            string qry = "UPDATE Journal_Master SET access_date='" + curda + "',access_time='" + curti + "',journal_name='" + txtpertitle.Text + "',remarks ='" + txtremark.Text + "',used_flag='',Journal_type='" + Convert.ToString(ddljour.SelectedItem) + "',lang='" + Convert.ToString(ddllanguage.SelectedItem) + "',Periodicity='" + Convert.ToString(ddlpri.SelectedItem) + "',Periodicity_value='',journal_price='" + Txtindaianprice.Text + "',department='" + Convert.ToString(ddldepartment.SelectedItem) + "',publisher=' " + Convert.ToString(ddlpublish.SelectedItem) + "',is_national='" + Convert.ToInt32(is_natio) + "',currency_type='" + Convert.ToString(ddlcurrencytype.SelectedItem) + "',currency_value='" + txtcurrencyvalue.Text + "',subject='" + Convert.ToString(ddlsubject.SelectedItem) + "',F_Issue='" + curda + "',L_Issue='" + curda + "',A_Status='',G_Period='',DD_No='',DD_Date='" + curda + "',In_Favour='',DD_Amount=0,Active=1,ISSNNo='" + txtissn.Text + "',DeliveryType='" + Convert.ToString(ddldevt.SelectedItem) + "',IssueBy='" + Convert.ToInt32(iss_by) + "',PerIssueNo='" + txtperis.Text + "',TotalNoIssues='" + txttotnois.Text + "',IssueType='" + i_type + "',IssueTypeVAl='',Journal_Website='" + txtjoweb.Text + "',TamilJrnlName='',IssueByDays='" + txtdays.Text + "',Supplier ='" + Convert.ToString(ddlsupp.SelectedItem) + "',SubsAmount ='" + subsamount + "',TitleLanguage ='" + language1 + "',ISActive ='" + Convert.ToInt32(i_active) + "',PeriodicalType='" + Convert.ToInt32(is_natio) + "',country='" + countries + "',year='" + txtyear.Text + "' WHERE Journal_Code ='" + txtpercode.Text + "' AND Lib_Code ='" + l_code + "' ";

            int insert = da.update_method_wo_parameter(qry, "text");

            string depatname = da.GetFunction("SELECT COUNT(*) FROM Journal_Dept WHERE Dept_Name ='" + ddldepartment.SelectedItem.ToString() + "' AND College_Code ='" + collegecode + "'");
            if (depatname == "0")
            {
                string qry1 = "INSERT INTO Journal_Dept(dept_name,dept_Acr,College_Code,Lib_Code) VALUES('" + ddldepartment.SelectedItem.ToString() + "','','" + collegecode + "','" + l_code + "')";
                int insert1 = da.update_method_wo_parameter(qry1, "text");
            }
            if (insert == 0)
            {
                divPopupAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Journal master entry not updated ";
                grdperiodical.Visible = false;
                addtbl.Visible = false;
                div_report.Visible = false;
                clear();
            }
            else
            {
                divPopupAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Journal master entry updated sucessfully";
                grdperiodical.Visible = false;
                addtbl.Visible = false;
                div_report.Visible = false;
                clear();
            }
        }
        catch
        {
        }
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {
        try
        {
            div1.Visible = true;
            div2.Visible = true;
            lbldeletealter.Visible = true;
            lbldeletealter.Text = "Are you sure to delete?";
            btnyes.Visible = true;
            btnNo.Visible = true;
        }
        catch
        {
        }
    }

    protected void btnPopAlertyes_Click(object sender, EventArgs e)
    {
        try
        {
            string qry3 = da.GetFunction("SELECT COUNT(*) FROM Subscription WHERE Journal_Code ='" + txtpercode.Text + "' WHERE Lib_Code ='" + ddllibararyname.SelectedValue.ToString() + "' ");
            int qry3count = Convert.ToInt32(qry3);
            if (qry3count > 0)
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "This journal is used, Can't delete";
                btnPopAlertClose.Visible = true;
                divPopupAlert.Visible = true;
                divAlertContent.Visible = true;
            }
            string qry4 = da.GetFunction("SELECT COUNT(*) FROM Journal WHERE Journal_Code ='" + txtpercode.Text + "' WHERE Lib_Code ='" + ddllibararyname.SelectedValue.ToString() + "'");
            int qry4count = Convert.ToInt32(qry3);
            if (qry4count > 0)
            {
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "This journal is used, Can't delete";
                btnPopAlertClose.Visible = true;
                divPopupAlert.Visible = true;
                divAlertContent.Visible = true;
            }
            string delqry = "DELETE FROM Journal_Master WHERE Journal_Code ='" + txtpercode.Text + "' AND Lib_Code ='" + ddllibararyname.SelectedValue.ToString() + "'";
            int del = da.update_method_wo_parameter(delqry, "text");
            if (del == 0)
            {
                divPopupAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Journal not deleted ";
                grdperiodical.Visible = false;
                addtbl.Visible = false;
                div_report.Visible = false;
                clear();
                div2.Visible = false;
                div1.Visible = false;
            }
            else
            {
                divPopupAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "Journal deleted sucessfully";
                grdperiodical.Visible = false;
                addtbl.Visible = false;
                div_report.Visible = false;
                clear();
                div2.Visible = false;
                div1.Visible = false;
            }
        }
        catch
        {
        }

    }

    protected void btnPopAlertNo_Click(object sender, EventArgs e)
    {
        addtbl.Visible = true;
        divsaventry.Visible = true;
        div1.Visible = false;
    }

    protected void grdperiodical_OnPageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        grdperiodical.PageIndex = e.NewPageIndex;
        btngo_Click(sender, e);
    }

    #endregion

    public void libararyname(string LibCollection)
    {
        try
        {
            ddllibararyname.Items.Clear();
            ds.Clear();
            if (!string.IsNullOrEmpty(collegecode))
            {
                string lib_name = "SELECT DISTINCT Lib_Code,Lib_Name FROM Library " + LibCollection + " AND college_code=" + collegecode + "  ORDER BY Lib_Name ";
                ds = da.select_method_wo_parameter(lib_name, "text");
                // lib_name = ds.Tables[0].Rows[0]["lib_name"].ToString();
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddllibararyname.DataSource = ds;
                ddllibararyname.DataTextField = "lib_name";
                ddllibararyname.DataValueField = "Lib_Code";
                ddllibararyname.DataBind();
                ddllibararyname.SelectedIndex = 0;
            }
        }
        catch (Exception ex) { da.sendErrorMail(ex, collegecode, "periodicalmaster.aspx"); }
    }

    public void dept()
    {
        try
        {
            ddldepartment.Items.Clear();
            ds.Clear();
            if (!string.IsNullOrEmpty(collegecode))
            {
                string dept = "SELECT DISTINCT ISNULL(Dept_Name,'') Dept_Name FROM Journal_Dept WHERE College_Code =" + collegecode + " AND ISNULL(Dept_Name,'') <> '' ORDER BY Dept_Name";
                ds = da.select_method_wo_parameter(dept, "text");

            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddldepartment.DataSource = ds;
                ddldepartment.DataTextField = "Dept_Name";
                ddldepartment.DataValueField = "Dept_Name";
                ddldepartment.DataBind();
                ddldepartment.Items.Insert(0, "");
            }
        }
        catch (Exception ex) { da.sendErrorMail(ex, collegecode, "periodicalmaster.aspx"); }
    }

    public List<string> countrylist()
    {
        List<string> Countrylistnew = new List<string>();
        CultureInfo[] get = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
        foreach (CultureInfo getcul in get)
        {
            RegionInfo getregion = new RegionInfo(getcul.LCID);
            if (!(Countrylistnew.Contains(getregion.EnglishName)))
            {
                Countrylistnew.Add(getregion.EnglishName);
            }
        }
        if (Countrylistnew.Contains("India"))
        {
            Countrylistnew[0] = "India";
        }
        // Countrylistnew.Sort();
        return Countrylistnew;//delsi2702
    }

    public void publis()
    {
        try
        {
            ddlpublish.Items.Clear();
            ds.Clear();
            if (!string.IsNullOrEmpty(collegecode))
            {
                string publi = "SELECT DISTINCT ISNULL(Publisher,'') Publisher FROM Journal_Master J,Library L WHERE J.Lib_Code = L.Lib_Code AND College_Code =" + collegecode + " AND ISNULL(Publisher,'') <> ''  ORDER BY Publisher";
                ds = da.select_method_wo_parameter(publi, "text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlpublish.DataSource = ds;
                ddlpublish.DataTextField = "Publisher";
                ddlpublish.DataValueField = "Publisher";
                ddlpublish.DataBind();
                ddlpublish.Items.Insert(0, "");
            }
        }
        catch (Exception ex) { da.sendErrorMail(ex, collegecode, "periodicalmaster.aspx"); }
    }

    public void binjournalty()
    {
        try
        {
            ds.Clear();

            if (!string.IsNullOrEmpty(collegecode))
            {
                string journ_ty = "SELECT DISTINCT ISNULL(Journal_Type,'') Journal_Type FROM Journal_Master M,Library L WHERE M.Lib_Code = L.Lib_Code AND College_Code =" + collegecode + " AND ISNULL(Journal_Type,'') <> '' ORDER BY Journal_Type";
                ds = da.select_method_wo_parameter(journ_ty, "text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddljour.DataSource = ds;
                ddljour.DataTextField = "Journal_Type";
                ddljour.DataValueField = "Journal_Type";
                ddljour.DataBind();
                ddljour.Items.Insert(0, "");
            }
        }
        catch (Exception ex) { da.sendErrorMail(ex, collegecode, "periodicalmaster.aspx"); }
    }

    public void subj()
    {
        try
        {
            ddlsubject.Items.Clear();
            ds.Clear();
            if (!string.IsNullOrEmpty(collegecode))
            {
                string subje = "SELECT DISTINCT ISNULL(Subject,'') Subject FROM Journal_Master M,Library L WHERE M.Lib_Code = L.Lib_Code AND College_Code =" + collegecode + " AND ISNULL(Subject,'') <> '' ORDER BY Subject  ";
                ds = da.select_method_wo_parameter(subje, "text");

            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlsubject.DataSource = ds;
                ddlsubject.DataTextField = "Subject";
                ddlsubject.DataValueField = "Subject";
                ddlsubject.DataBind();
                ddlsubject.Items.Insert(0, "");
            }
        }
        catch (Exception ex) { da.sendErrorMail(ex, collegecode, "periodicalmaster.aspx"); }
    }

    public void supp()
    {
        try
        {
            ddlsupp.Items.Clear();
            ds.Clear();
            string sup = "SELECT DISTINCT ISNULL(VendorCompName,'') as Supplier_Name FROM CO_VendorMaster S WHERE LibraryFlag='1' and ISNULL(VendorCompName,'') <> ''  ORDER BY Supplier_Name ";
            ds = da.select_method_wo_parameter(sup, "text");
            // libr_name = ds.Tables[0].Rows[0]["subject"].ToString();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlsupp.DataSource = ds;
                ddlsupp.DataTextField = "Supplier_Name";
                ddlsupp.DataValueField = "Supplier_Name";
                ddlsupp.DataBind();
                ddlsupp.Items.Insert(0, "");
            }
        }
        catch (Exception ex) { da.sendErrorMail(ex, collegecode, "periodicalmaster.aspx"); }
    }

    public void language3()
    {
        try
        {
            ddllanguage.Items.Clear();
            ds.Clear();
            if (!string.IsNullOrEmpty(collegecode))
            {
                string lang = "SELECT DISTINCT ISNULL(Lang,'') Lang FROM Journal_Master M,Library L WHERE M.Lib_Code = L.Lib_Code AND College_Code =" + collegecode + " AND ISNULL(Lang,'') <> '' ORDER BY Lang ";
                ds = da.select_method_wo_parameter(lang, "text");
                // libr_name = ds.Tables[0].Rows[0]["subject"].ToString();
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddllanguage.DataSource = ds;
                ddllanguage.DataTextField = "Lang";
                ddllanguage.DataValueField = "Lang";
                ddllanguage.DataBind();
                ddllanguage.Items.Insert(0, "");
            }
        }
        catch (Exception ex) { da.sendErrorMail(ex, collegecode, "periodicalmaster.aspx"); }
    }

    public void Currencytype()
    {
        try
        {
            ddlcurrencytype.Items.Clear();
            ds.Clear();
            if (!string.IsNullOrEmpty(collegecode))
            {
                string currtype = "SELECT DISTINCT ISNULL(Currency_Type,'') Currency_Type FROM Journal_Master M,Library L WHERE M.Lib_Code = L.Lib_Code AND College_Code =" + collegecode + " AND ISNULL(Currency_Type,'') <> ''  ";
                ds = da.select_method_wo_parameter(currtype, "text");
                // libr_name = ds.Tables[0].Rows[0]["subject"].ToString();
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlcurrencytype.DataSource = ds;
                ddlcurrencytype.DataTextField = "Currency_Type";
                ddlcurrencytype.DataValueField = "Currency_Type";
                ddlcurrencytype.DataBind();
                ddlcurrencytype.Items.Insert(0, "");
            }
        }
        catch (Exception ex) { da.sendErrorMail(ex, collegecode, "periodicalmaster.aspx"); }
    }

    public void deliver()
    {
        try
        {
            ddldevt.Items.Clear();
            ds.Clear();
            if (!string.IsNullOrEmpty(collegecode))
            {
                string del = "SELECT DISTINCT ISNULL(DeliveryType,'') DeliveryType FROM Journal_Master M,Library L WHERE M.Lib_Code = L.Lib_Code AND College_Code =" + collegecode + " AND ISNULL(DeliveryType,'') <> ''";
                ds = da.select_method_wo_parameter(del, "text");
                // libr_name = ds.Tables[0].Rows[0]["subject"].ToString();
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddldevt.DataSource = ds;
                ddldevt.DataTextField = "DeliveryType";
                ddldevt.DataValueField = "DeliveryType";
                ddldevt.DataBind();
                ddldevt.Items.Insert(0, "");
            }
        }
        catch (Exception ex) { da.sendErrorMail(ex, collegecode, "periodicalmaster.aspx"); }
    }

    public void periodi()
    {
        try
        {
            ddlpri.Items.Clear();
            ds.Clear();
            if (!string.IsNullOrEmpty(collegecode))
            {
                string prio = "SELECT DISTINCT ISNULL(Periodicity,'') Periodicity FROM Journal_Master M,Library L WHERE M.Lib_Code = L.Lib_Code AND College_Code =" + collegecode + " AND ISNULL(Periodicity,'') <> '' ";
                ds = da.select_method_wo_parameter(prio, "text");
                // libr_name = ds.Tables[0].Rows[0]["subject"].ToString();
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlpri.DataSource = ds;
                ddlpri.DataTextField = "Periodicity";
                ddlpri.DataValueField = "Periodicity";
                ddlpri.DataBind();
                ddlpri.Items.Insert(0, "");
            }
        }
        catch (Exception ex) { da.sendErrorMail(ex, collegecode, "periodicalmaster.aspx"); }
    }

    public void accessautocode()
    {
        try
        {
            string codeno = string.Empty;
            string codeno1 = string.Empty;
            string libcodeval = Convert.ToString(ddllibararyname.SelectedValue);
            DataSet dsAutoAccess = new DataSet();
            DataSet dsBack = new DataSet();
            string sql = "SELECT ISNULL(JournalAutono,0) JournalAutono,ISNULL(pm_acr,'') pm_acr,ISNULL(pm_stno,1) pm_stno FROM Library Where Lib_Code ='" + libcodeval + "'";
            dsAutoAccess = da.select_method_wo_parameter(sql, "text");
            if (dsAutoAccess.Tables[0].Rows.Count > 0)
            {
                string book = Convert.ToString(dsAutoAccess.Tables[0].Rows[0]["JournalAutono"]);
                if (book.ToLower() == "true")
                {
                    sql = "select max(substring(journal_code,4,4)) as journal_code from Journal_Master where  lib_code ='" + libcodeval + "'";
                    dsBack.Clear();
                    dsBack = da.select_method_wo_parameter(sql, "text");
                    if (dsBack.Tables[0].Rows.Count > 0)
                    {
                        codeno = Convert.ToString(dsBack.Tables[0].Rows[dsBack.Tables[0].Rows.Count - 1]["journal_code"]);
                        string str = "";
                        for (int k = 0; k < codeno.Length; k++)
                        {
                            string a = Convert.ToString(codeno.ElementAt<char>(k));
                            if (a.All(char.IsNumber))
                            {
                                str = str + a;
                            }
                        }
                        int jj = Convert.ToInt32(str) + 1;
                        codeno1 = Convert.ToString(dsAutoAccess.Tables[0].Rows[0]["pm_acr"]) + jj;
                        txtpercode.Text = codeno1;
                        txtpercode.Enabled = false;
                    }
                    else
                    {
                        codeno1 = Convert.ToString(ds.Tables[0].Rows[0]["pm_acr"]) + Convert.ToString(ds.Tables[0].Rows[0]["pm_stno"]);
                        txtpercode.Text = codeno1;
                        txtpercode.Enabled = false;
                    }
                }
                else
                {
                    txtpercode.Text = "";
                    txtpercode.Enabled = true;
                }
            }
            else
            {
                txtpercode.Text = "";
                txtpercode.Enabled = true;
            }
        }
        catch (Exception ex) { }
    }

    protected void ddllibararyname_SelectedIndexChanged(object sender, EventArgs e)
    {
        accessautocode();
    }

    protected void ddllanguage_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            btnlanuage.Visible = true;
            Btnsublanguage.Visible = true;
        }
        catch (Exception ex) { da.sendErrorMail(ex, collegecode, "periodicalmaster.aspx"); }
    }

    protected void ddlsubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        Btnaddsubj.Visible = true;
        Btnsubsubject.Visible = true;
    }

    protected void ddlsearch_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void ddlperiodical_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //Btnaddbtype.Visible = true;
            //    Btnsubtype.Visible=true;


        }
        catch
        {
        }


    }

    protected void ddlperiodicalcity_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Btncity.Visible = true;
        //    btnsubcity.Visible=true;

    }

    protected void ddlcurrencytype_SelectedIndexChanged(object sender, EventArgs e)
    {
        Btnaddcurrency.Visible = true;
        Btnsubcurrency.Visible = true;

    }

    protected void ddldepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnadddept.Visible = true;
        Btnsubdept.Visible = true;


    }

    protected void ddlpublisher_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Btnaddpub.Visible = true;

        //    Btnsubsupplier.Visible=true;


    }

    protected void ddlis_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txttotnois.Text = "";
            if (ddlis.SelectedIndex == 0)
            {
                rblisty.Items[0].Enabled = false;
                rblisty.Items[1].Enabled = true;
                rblisty.Items[2].Enabled = false;
                rblisty.Items[3].Enabled = false;
                rblisty.Items[0].Selected = false;
                rblisty.Items[1].Selected = true;
                rblisty.Items[2].Selected = false;
                rblisty.Items[3].Selected = false;
                txtdays.Enabled = false;
                Lnkbutton.Visible = false;
                txtyear.Visible = false;
                Lblyear.Visible = false;
            }
            else if (ddlis.SelectedIndex == 1)
            {

                rblisty.Items[0].Enabled = false;
                rblisty.Items[1].Enabled = true;
                rblisty.Items[2].Enabled = false;
                rblisty.Items[3].Enabled = false;
                rblisty.Items[0].Selected = false;
                rblisty.Items[1].Selected = true;
                rblisty.Items[2].Selected = false;
                rblisty.Items[3].Selected = false;
                txtdays.Enabled = false;
                Lnkbutton.Visible = true;
                txtyear.Visible = false;
                Lblyear.Visible = false;
            }
            else if (ddlis.SelectedIndex == 2)
            {
                rblisty.Items[0].Enabled = true;
                rblisty.Items[1].Enabled = false;
                rblisty.Items[2].Enabled = false;
                rblisty.Items[3].Enabled = false;
                rblisty.Items[0].Selected = true;
                rblisty.Items[1].Selected = false;
                rblisty.Items[2].Selected = false;
                rblisty.Items[3].Selected = false;
                txtdays.Enabled = true;
                Lnkbutton.Visible = true;
                txtyear.Visible = false;
                Lblyear.Visible = false;
            }

            else if (ddlis.SelectedIndex == 3)
            {
                rblisty.Items[0].Enabled = false;
                rblisty.Items[1].Enabled = false;
                rblisty.Items[2].Enabled = true;
                rblisty.Items[3].Enabled = false;
                rblisty.Items[0].Selected = false;
                rblisty.Items[1].Selected = false;
                rblisty.Items[2].Selected = true;
                rblisty.Items[3].Selected = false;
                txtdays.Enabled = true;
                Lnkbutton.Visible = true;
                txtyear.Visible = true;
                Lblyear.Visible = true;
            }
            else if (ddlis.SelectedIndex == 4)
            {
                rblisty.Items[0].Enabled = false;
                rblisty.Items[1].Enabled = false;
                rblisty.Items[2].Enabled = false;
                rblisty.Items[3].Enabled = true;
                rblisty.Items[0].Selected = false;
                rblisty.Items[1].Selected = false;
                rblisty.Items[2].Selected = false;
                rblisty.Items[3].Selected = true;
                Lnkbutton.Visible = true;
                Lnkbutton.Visible = false;
                txtdays.Enabled = true;
                txtyear.Visible = false;
                Lblyear.Visible = false;
            }
        }
        catch
        {


        }
    }

    protected void ddlpri_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void ddldevt_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void ddljour_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void ddlpublish_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void ddlengtam_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void Rdlperoidicl_Selected(object sender, EventArgs e)
    {
    }

    protected void ddldelivertype_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    //Added by SD
    protected void Lnkbutton_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlis.SelectedIndex == 1)
            {
                popupselect.Visible = true;
                FpSpreadissue.Sheets[0].RowCount = 7;
                FpSpreadissue.Sheets[0].ColumnCount = 2;
                FpSpreadissue.CommandBar.Visible = false;
                FpSpreadissue.Sheets[0].AutoPostBack = false;
                FpSpreadissue.Sheets[0].ColumnHeader.RowCount = 1;
                FpSpreadissue.Sheets[0].RowHeader.Visible = false;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                FpSpreadissue.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Type";
                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                FpSpreadissue.Sheets[0].Columns[0].Locked = true;
                FpSpreadissue.Columns[0].Width = 150;


                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 1].Text = "NoOIssue";
                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpreadissue.Columns[1].Width = 160;
                FpSpreadissue.Columns[1].Visible = true;



                FarPoint.Web.Spread.TextCellType txtCell = new FarPoint.Web.Spread.TextCellType();



                FpSpreadissue.Sheets[0].Cells[0, 0].CellType = txtCell;
                FpSpreadissue.Sheets[0].Cells[1, 0].CellType = txtCell;
                FpSpreadissue.Sheets[0].Cells[2, 0].CellType = txtCell;
                FpSpreadissue.Sheets[0].Cells[3, 0].CellType = txtCell;
                FpSpreadissue.Sheets[0].Cells[4, 0].CellType = txtCell;
                FpSpreadissue.Sheets[0].Cells[5, 0].CellType = txtCell;
                FpSpreadissue.Sheets[0].Cells[6, 0].CellType = txtCell;
                FpSpreadissue.Sheets[0].Cells[0, 1].CellType = txtCell;
                FpSpreadissue.Sheets[0].Cells[1, 1].CellType = txtCell;
                FpSpreadissue.Sheets[0].Cells[2, 1].CellType = txtCell;
                FpSpreadissue.Sheets[0].Cells[3, 1].CellType = txtCell;
                FpSpreadissue.Sheets[0].Cells[4, 1].CellType = txtCell;
                FpSpreadissue.Sheets[0].Cells[5, 1].CellType = txtCell;
                FpSpreadissue.Sheets[0].Cells[6, 1].CellType = txtCell;

                FpSpreadissue.Sheets[0].Cells[0, 0].Text = "Sunday";
                FpSpreadissue.Sheets[0].Cells[1, 0].Text = "Monday";
                FpSpreadissue.Sheets[0].Cells[2, 0].Text = "Tuesday";
                FpSpreadissue.Sheets[0].Cells[3, 0].Text = "Wednessday";
                FpSpreadissue.Sheets[0].Cells[4, 0].Text = "Thursday";
                FpSpreadissue.Sheets[0].Cells[5, 0].Text = "Friday";
                FpSpreadissue.Sheets[0].Cells[6, 0].Text = "Saturday";

                FpSpreadissue.Sheets[0].Cells[0, 1].Locked = false;
                FpSpreadissue.Sheets[0].Cells[1, 1].Locked = false;
                FpSpreadissue.Sheets[0].Cells[2, 1].Locked = false;
                FpSpreadissue.Sheets[0].Cells[3, 1].Locked = false;
                FpSpreadissue.Sheets[0].Cells[4, 1].Locked = false;
                FpSpreadissue.Sheets[0].Cells[5, 1].Locked = false;
                FpSpreadissue.Sheets[0].Cells[6, 1].Locked = false;

                FpSpreadissue.SaveChanges();
                FpSpreadissue.Sheets[0].PageSize = FpSpreadissue.Sheets[0].RowCount;

                FpSpreadissue.Height = 375;
                FpSpreadissue.Width = 300;

                FpSpreadissue.Visible = true;
                btn_ok.Visible = true;
                btn_exit1.Visible = true;
            }

            else if (ddlis.SelectedIndex == 2)
            {

                popupselect.Visible = true;
                FpSpreadissue.Sheets[0].RowCount = 31;
                FpSpreadissue.Sheets[0].ColumnCount = 2;
                FpSpreadissue.CommandBar.Visible = false;
                FpSpreadissue.Sheets[0].AutoPostBack = false;
                FpSpreadissue.Sheets[0].ColumnHeader.RowCount = 1;
                FpSpreadissue.Sheets[0].RowHeader.Visible = false;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                FpSpreadissue.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Type";
                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                FpSpreadissue.Sheets[0].Columns[0].Locked = true;
                FpSpreadissue.Columns[0].Width = 150;


                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 1].Text = "NoOIssue";
                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpreadissue.Columns[1].Width = 150;
                FpSpreadissue.Columns[1].Visible = true;

                FarPoint.Web.Spread.TextCellType txtCell = new FarPoint.Web.Spread.TextCellType();
                for (int j = 0; j < FpSpreadissue.Rows.Count; j++)
                {
                    //FpSpreadissue.Rows.Count++;
                    int k = j + 1;

                    FpSpreadissue.Sheets[0].Cells[j, 0].CellType = txtCell;
                    FpSpreadissue.Sheets[0].Cells[j, 1].CellType = txtCell;

                    FpSpreadissue.Sheets[0].Cells[j, 0].Text = Convert.ToString(k);

                    FpSpreadissue.Sheets[0].Cells[j, 0].Locked = true;

                    FpSpreadissue.Sheets[0].Cells[j, 1].Locked = false;


                }
                FpSpreadissue.SaveChanges();
                FpSpreadissue.Sheets[0].PageSize = FpSpreadissue.Sheets[0].RowCount;

                FpSpreadissue.Height = 375;
                FpSpreadissue.Width = 300;

                FpSpreadissue.Visible = true;
                btn_ok.Visible = true;
                btn_exit1.Visible = true;

            }
            else
            {
                popupselect.Visible = true;
                FpSpreadissue.Sheets[0].RowCount = 12;
                FpSpreadissue.Sheets[0].ColumnCount = 4;
                FpSpreadissue.CommandBar.Visible = false;
                FpSpreadissue.Sheets[0].AutoPostBack = false;
                FpSpreadissue.Sheets[0].ColumnHeader.RowCount = 1;
                FpSpreadissue.Sheets[0].RowHeader.Visible = false;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                FpSpreadissue.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Type";
                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                FpSpreadissue.Sheets[0].Columns[0].Locked = true;
                FpSpreadissue.Columns[0].Width = 100;


                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 1].Text = "NoOIssue";
                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpreadissue.Columns[1].Width = 100;
                FpSpreadissue.Columns[1].Visible = true;

                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Days";
                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                FpSpreadissue.Columns[2].Width = 100;
                FpSpreadissue.Columns[2].Visible = true;

                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Year";
                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                FpSpreadissue.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpreadissue.Columns[3].Width = 100;
                FpSpreadissue.Columns[3].Visible = true;

                FarPoint.Web.Spread.TextCellType txtCell = new FarPoint.Web.Spread.TextCellType();


                for (int j = 0; j < FpSpreadissue.Rows.Count; j++)
                {

                    FpSpreadissue.Sheets[0].Cells[j, 0].CellType = txtCell;
                    FpSpreadissue.Sheets[0].Cells[j, 1].CellType = txtCell;

                    FpSpreadissue.Sheets[0].Cells[j, 0].Locked = true;

                    FpSpreadissue.Sheets[0].Cells[j, 1].Locked = false;
                    FpSpreadissue.Sheets[0].Cells[j, 2].Locked = false;

                }
                FpSpreadissue.Sheets[0].Cells[0, 0].Text = "January";
                FpSpreadissue.Sheets[0].Cells[1, 0].Text = "February";
                FpSpreadissue.Sheets[0].Cells[2, 0].Text = "March";
                FpSpreadissue.Sheets[0].Cells[3, 0].Text = "April";
                FpSpreadissue.Sheets[0].Cells[4, 0].Text = "May";
                FpSpreadissue.Sheets[0].Cells[5, 0].Text = "June";
                FpSpreadissue.Sheets[0].Cells[6, 0].Text = "July";
                FpSpreadissue.Sheets[0].Cells[7, 0].Text = "August";
                FpSpreadissue.Sheets[0].Cells[8, 0].Text = "September";
                FpSpreadissue.Sheets[0].Cells[9, 0].Text = "October";
                FpSpreadissue.Sheets[0].Cells[10, 0].Text = "November";
                FpSpreadissue.Sheets[0].Cells[11, 0].Text = "December";

                FpSpreadissue.SaveChanges();
                FpSpreadissue.Sheets[0].PageSize = FpSpreadissue.Sheets[0].RowCount;

                FpSpreadissue.Height = 375;
                FpSpreadissue.Width = 400;

                FpSpreadissue.Visible = true;
                btn_ok.Visible = true;
                btn_exit1.Visible = true;
            }
        }
        catch (Exception ex) { da.sendErrorMail(ex, collegecode, "periodicalmaster.aspx"); }
    }

    protected void btn_ok_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlis.SelectedIndex == 1)
            {
                FpSpreadissue.SaveChanges();
                if (FpSpreadissue.Rows.Count > 0)
                {
                    int checkval1 = 0;
                    FpSpreadissue.SaveChanges();
                    for (int row = 0; row < FpSpreadissue.Sheets[0].RowCount; row++)
                    {
                        string value = Convert.ToString(FpSpreadissue.Sheets[0].Cells[row, 1].Text);
                        if (value != "")
                            checkval1 += Convert.ToInt32(value);

                    }
                    if (Convert.ToString(checkval1) != "")
                    {
                        popupselect.Visible = false;
                        txttotnois.Text = Convert.ToString(checkval1);
                    }
                }
            }
            else if (ddlis.SelectedIndex == 2)
            {
                FpSpreadissue.SaveChanges();
                if (FpSpreadissue.Rows.Count > 0)
                {
                    int checkval1 = 0;
                    FpSpreadissue.SaveChanges();
                    for (int row = 0; row < FpSpreadissue.Sheets[0].RowCount; row++)
                    {
                        string value = Convert.ToString(FpSpreadissue.Sheets[0].Cells[row, 1].Text);
                        if (value != "")
                            checkval1 += Convert.ToInt32(value);

                    }
                    if (Convert.ToString(checkval1) != "")
                    {
                        popupselect.Visible = false;
                        txttotnois.Text = Convert.ToString(checkval1);
                    }
                }
            }
            else if (ddlis.SelectedIndex == 3)
            {
                FpSpreadissue.SaveChanges();
                if (FpSpreadissue.Rows.Count > 0)
                {
                    string year = "";
                    int checkval1 = 0;
                    string academicYear = "";
                    FpSpreadissue.SaveChanges();
                    for (int row = 0; row < FpSpreadissue.Sheets[0].RowCount; row++)
                    {
                        string value = Convert.ToString(FpSpreadissue.Sheets[0].Cells[row, 1].Text);
                        string yr = Convert.ToString(FpSpreadissue.Sheets[0].Cells[row, 3].Text);

                        if (yr != "")
                        {
                            if (!year.Contains(yr))
                            {
                                if (year == "")
                                    year = yr;
                                else
                                    year = yr + "-" + year;
                            }
                        }

                        //if (!year.Contains(yr))
                        //{
                        //    if (year == "")
                        //        year = yr;
                        //    else
                        //        year = year + "'-'" + yr;
                        //}
                        if (value != "")
                            checkval1 += Convert.ToInt32(value);

                    }
                    popupselect.Visible = false;
                    txttotnois.Text = Convert.ToString(checkval1);
                    txtyear.Text = Convert.ToString(year);
                    if (Convert.ToString(checkval1) != "")
                    {
                        popupselect.Visible = false;
                        txttotnois.Text = Convert.ToString(checkval1);
                    }
                }
            }
        }
        catch (Exception ex) { da.sendErrorMail(ex, collegecode, "periodicalmaster.aspx"); }
    }

    protected void btn_exit1_Click(object sender, EventArgs e)
    {
        popupselect.Visible = false;
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
            libararyname(LibCollection);
        }
        catch (Exception ex)
        {
        }
    }

    protected void grdperiodical_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 1; i < e.Row.Cells.Count; i++)
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

    protected void grdperiodical_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            if (ddlclg.Items.Count > 0)
            {
                collegecode = string.Empty;
                foreach (ListItem li in ddlclg.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collegecode))
                        {
                            collegecode = "'" + li.Value + "'";
                        }
                        else
                        {
                            collegecode = ",'" + li.Value + "'";
                        }
                    }
                }
            }
            if (Convert.ToString(rowIndex) != "-1")
            {
                addtbl.Visible = true;
                divsaventry.Visible = true;
                //getLibPrivil();
                //publis();
                //dept();
                //binjournalty();
                //periodi();
                //deliver();
                //supp();
                //subj();
                //language3();
                //Currencytype();

                btnupdate.Visible = true;
                btndelete.Visible = true;
                Btnpersave.Visible = false;
                txtpercode.Text = grdperiodical.Rows[rowIndex].Cells[1].Text;
                // ddllibararyname.Enabled = false;
                string qry = "SELECT Journal_Code,Journal_Name,ISNULL(Department,'') Department,ISNULL(Publisher,'') Publisher,ISNULL(Subject,'') Subject,ISNULL(Journal_Type,'') Journal_Type,ISNULL(Lang,'') Lang,ISNULL(Currency_Type,'') Currency_Type,ISNULL(Currency_Value,'') Currency_Value,ISNULL(SubsAmount,0) SubsAmount,ISNULL(Supplier,'') Supplier,ISNULL(Journal_Price,0) Journal_Price,ISNULL(Is_National,1) Is_National,ISNULL(ISSNNo,'') ISSNNo,ISNULL(DeliveryType,'') DeliveryType,ISNULL(Periodicity,'') Periodicity,ISNULL(IssueBy,'') IssueBy,ISNULL(PerIssueNo,0) PerIssueNo,ISNULL(TotalNoIssues,0) TotalNoIssues,ISNULL(IssueType,0) IssueType,ISNULL(Remarks,'') Remarks,ISNULL(IssueTypeVAl,'') IssueTypeVAl,ISNULL(Remarks,'') Remarks,Lib_Name,ISNULL(Journal_Website,'') Journal_Website,ISNULL(TamilJrnlName,'') TamilJrnlName,ISNULL(IssueByDays,0) IssueByDays,ISNULL(TitleLanguage,0) TitleLanguage,ISNULL(IsActive,0) IsActive,ISNULL(PeriodicalType,1) PeriodicalType,country FROM Journal_Master J,Library L WHERE J.Lib_Code = L.Lib_Code AND Journal_Code ='" + txtpercode.Text + "' AND College_Code =" + collegecode + "";
                ds.Clear();
                ds = da.select_method_wo_parameter(qry, "text");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtpertitle.Text = Convert.ToString(ds.Tables[0].Rows[0]["Journal_Name"]);
                    string dept = Convert.ToString(ds.Tables[0].Rows[0]["Department"]);
                    if (!string.IsNullOrEmpty(dept))
                    {
                        ddldepartment.ClearSelection();
                        ddldepartment.Items.FindByText(dept).Selected = true;
                    }
                    string Publish = Convert.ToString(ds.Tables[0].Rows[0]["Publisher"]);
                    if (!string.IsNullOrEmpty(Publish))
                    {
                        ddlpublish.ClearSelection();
                        ddlpublish.Items.FindByText(Publish).Selected = true;
                    }
                    string subject = Convert.ToString(ds.Tables[0].Rows[0]["Subject"]);
                    if (!string.IsNullOrEmpty(subject))
                    {
                        ddlsubject.ClearSelection();
                        ddlsubject.Items.FindByText(subject).Selected = true;
                    }
                    string curtype = Convert.ToString(ds.Tables[0].Rows[0]["Currency_Type"]);
                    if (!string.IsNullOrEmpty(curtype))
                    {
                        ddlcurrencytype.ClearSelection();
                        ddlcurrencytype.Items.FindByText(curtype).Selected = true;
                    }
                    string subAmount = Convert.ToString(ds.Tables[0].Rows[0]["SubsAmount"]);
                    if (!string.IsNullOrEmpty(subAmount))
                        txtsubsam.Text = subAmount;
                    string country = Convert.ToString(ds.Tables[0].Rows[0]["country"]);
                    if (!string.IsNullOrEmpty(country))
                    {
                        ddlccountry.ClearSelection();
                        ddlccountry.Items.FindByText(country).Selected = true;
                    }
                    string delv = Convert.ToString(ds.Tables[0].Rows[0]["DeliveryType"]);
                    if (!string.IsNullOrEmpty(delv))
                    {
                        ddldevt.ClearSelection();
                        ddldevt.Items.FindByText(delv).Selected = true;
                    }
                    string periodicity = Convert.ToString(ds.Tables[0].Rows[0]["Periodicity"]);
                    if (!string.IsNullOrEmpty(periodicity))
                    {
                        ddlpri.ClearSelection();
                        ddlpri.Items.FindByText(periodicity).Selected = true;
                    }
                    string issueby = Convert.ToString(ds.Tables[0].Rows[0]["IssueBy"]);
                    if (!string.IsNullOrEmpty(issueby))
                    {
                        ddlis.ClearSelection();
                        if (issueby == "1")
                            ddlis.Items.FindByText("Daily").Selected = true;
                        if (issueby == "2")
                            ddlis.Items.FindByText("Weekly").Selected = true;
                        if (issueby == "3")
                            ddlis.Items.FindByText("Monthly").Selected = true;
                        if (issueby == "4")
                            ddlis.Items.FindByText("Yearly").Selected = true;
                    }

                    string issuetype = Convert.ToString(ds.Tables[0].Rows[0]["IssueType"]);
                    if (!string.IsNullOrEmpty(issuetype))
                    {
                        if (issuetype == "1")
                            rblisty.SelectedIndex = 1;
                        if (issuetype == "2")
                            rblisty.SelectedIndex = 2;
                        if (issuetype == "3")
                            rblisty.SelectedIndex = 3;
                        if (issuetype == "4")
                            rblisty.SelectedIndex = 4;
                    }
                    string remarks = Convert.ToString(ds.Tables[0].Rows[0]["Remarks"]);
                    if (!string.IsNullOrEmpty(remarks))
                        txtremark.Text = remarks;
                    string web = Convert.ToString(ds.Tables[0].Rows[0]["Journal_Website"]);
                    if (!string.IsNullOrEmpty(web))
                        txtjoweb.Text = web;

                    string pertype = Convert.ToString(ds.Tables[0].Rows[0]["PeriodicalType"]);
                    if (!string.IsNullOrEmpty(pertype))
                    {
                        if (pertype == "1")
                            rblpubty.SelectedIndex = 1;
                        if (pertype == "2")
                            rblpubty.SelectedIndex = 2;
                    }
                    string isact = Convert.ToString(ds.Tables[0].Rows[0]["IsActive"]);
                    if (!string.IsNullOrEmpty(isact))
                    {
                        if (isact.ToLower() == "true")
                            rbljrweb.SelectedIndex = 1;
                        else
                            rbljrweb.SelectedIndex = 0;
                    }

                    string Jortype = Convert.ToString(ds.Tables[0].Rows[0]["Journal_Type"]);
                    if (!string.IsNullOrEmpty(Jortype))
                    {
                        ddljour.ClearSelection();
                        ddljour.Items.FindByText(Jortype).Selected = true;
                    }
                    string Lang = Convert.ToString(ds.Tables[0].Rows[0]["Lang"]);
                    if (!string.IsNullOrEmpty(Lang))
                    {
                        ddllanguage.ClearSelection();
                        ddllanguage.Items.FindByText(Lang).Selected = true;
                    }
                    string curval = Convert.ToString(ds.Tables[0].Rows[0]["currency_value"]);
                    if (!string.IsNullOrEmpty(curval))
                        txtcurrencyvalue.Text = curval;
                    string sup = Convert.ToString(ds.Tables[0].Rows[0]["Supplier"]);
                    if (!string.IsNullOrEmpty(sup))
                    {
                        ddlsupp.ClearSelection();
                        ddlsupp.Items.FindByText(sup).Selected = true;
                    }
                    string issn = Convert.ToString(ds.Tables[0].Rows[0]["ISSNNo"]);
                    if (!string.IsNullOrEmpty(issn))
                        txtissn.Text = issn;

                    string titleLang = Convert.ToString(ds.Tables[0].Rows[0]["TitleLanguage"]);
                    if (!string.IsNullOrEmpty(titleLang))
                    {
                        ddlengtam.ClearSelection();
                        if (titleLang == "1")
                            ddlengtam.Items.FindByText("Tamil").Selected = true;
                        if (titleLang == "0")
                            ddlengtam.Items.FindByText("English").Selected = true;
                    }
                    string periss = Convert.ToString(ds.Tables[0].Rows[0]["PerIssueNo"]);
                    if (!string.IsNullOrEmpty(periss))
                        txtperis.Text = periss;
                    string totiss = Convert.ToString(ds.Tables[0].Rows[0]["TotalNoIssues"]);
                    if (!string.IsNullOrEmpty(totiss))
                        txttotnois.Text = totiss;
                    //string isstypeval = Convert.ToString(ds.Tables[0].Rows[0]["IssueTypeVAl"]);
                    //if (!string.IsNullOrEmpty(isstypeval))
                    //    txtdays.Text = isstypeval;
                    ddllibararyname.ClearSelection();
                    ddllibararyname.Items.FindByText(Convert.ToString(ds.Tables[0].Rows[0]["Lib_Name"])).Selected = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    protected void txtpercode_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            string libname = Convert.ToString(ddllibararyname.SelectedValue);
            string qry = "SELECT Journal_Code,Journal_Name,ISNULL(Department,'') Department,ISNULL(Publisher,'') Publisher,ISNULL(Subject,'') Subject,ISNULL(Journal_Type,'') Journal_Type,ISNULL(Lang,'') Lang,ISNULL(Currency_Type,'') Currency_Type,ISNULL(Currency_Value,'') Currency_Value,ISNULL(SubsAmount,0) SubsAmount,ISNULL(Supplier,'') Supplier,ISNULL(Journal_Price,0) Journal_Price,ISNULL(Is_National,1) Is_National,ISNULL(ISSNNo,'') ISSNNo,ISNULL(DeliveryType,'') DeliveryType,ISNULL(Periodicity,'') Periodicity,ISNULL(IssueBy,'') IssueBy,ISNULL(PerIssueNo,0) PerIssueNo,ISNULL(TotalNoIssues,0) TotalNoIssues,ISNULL(IssueType,0) IssueType,ISNULL(Remarks,'') Remarks,ISNULL(IssueTypeVAl,'') IssueTypeVAl,ISNULL(Remarks,'') Remarks,Lib_Name,ISNULL(Journal_Website,'') Journal_Website,ISNULL(TamilJrnlName,'') TamilJrnlName,ISNULL(IssueByDays,0) IssueByDays,ISNULL(TitleLanguage,0) TitleLanguage,ISNULL(IsActive,0) IsActive,ISNULL(PeriodicalType,1) PeriodicalType,country FROM Journal_Master J,Library L WHERE J.Lib_Code = L.Lib_Code AND Journal_Code ='" + txtpercode.Text + "' AND College_Code =" + collegecode + " and j.lib_code='" + libname + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(qry, "text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                addtbl.Visible = true;
                divsaventry.Visible = true;               
                btnupdate.Visible = true;
                btndelete.Visible = true;
                Btnpersave.Visible = false;
                txtpertitle.Text = Convert.ToString(ds.Tables[0].Rows[0]["Journal_Name"]);
                string dept = Convert.ToString(ds.Tables[0].Rows[0]["Department"]);
                if (!string.IsNullOrEmpty(dept))
                {
                    ddldepartment.ClearSelection();
                    ddldepartment.Items.FindByText(dept).Selected = true;
                }
                string Publish = Convert.ToString(ds.Tables[0].Rows[0]["Publisher"]);
                if (!string.IsNullOrEmpty(Publish))
                {
                    ddlpublish.ClearSelection();
                    ddlpublish.Items.FindByText(Publish).Selected = true;
                }
                string subject = Convert.ToString(ds.Tables[0].Rows[0]["Subject"]);
                if (!string.IsNullOrEmpty(subject))
                {
                    ddlsubject.ClearSelection();
                    ddlsubject.Items.FindByText(subject).Selected = true;
                }
                string curtype = Convert.ToString(ds.Tables[0].Rows[0]["Currency_Type"]);
                if (!string.IsNullOrEmpty(curtype))
                {
                    ddlcurrencytype.ClearSelection();
                    ddlcurrencytype.Items.FindByText(curtype).Selected = true;
                }
                string subAmount = Convert.ToString(ds.Tables[0].Rows[0]["SubsAmount"]);
                if (!string.IsNullOrEmpty(subAmount))
                    txtsubsam.Text = subAmount;
                string country = Convert.ToString(ds.Tables[0].Rows[0]["country"]);
                if (!string.IsNullOrEmpty(country))
                {
                    ddlccountry.ClearSelection();
                    ddlccountry.Items.FindByText(country).Selected = true;
                }
                string delv = Convert.ToString(ds.Tables[0].Rows[0]["DeliveryType"]);
                if (!string.IsNullOrEmpty(delv))
                {
                    ddldevt.ClearSelection();
                    ddldevt.Items.FindByText(delv).Selected = true;
                }
                string periodicity = Convert.ToString(ds.Tables[0].Rows[0]["Periodicity"]);
                if (!string.IsNullOrEmpty(periodicity))
                {
                    ddlpri.ClearSelection();
                    ddlpri.Items.FindByText(periodicity).Selected = true;
                }
                string issueby = Convert.ToString(ds.Tables[0].Rows[0]["IssueBy"]);
                if (!string.IsNullOrEmpty(issueby))
                {
                    ddlis.ClearSelection();
                    if (issueby == "1")
                        ddlis.Items.FindByText("Daily").Selected = true;
                    if (issueby == "2")
                        ddlis.Items.FindByText("Weekly").Selected = true;
                    if (issueby == "3")
                        ddlis.Items.FindByText("Monthly").Selected = true;
                    if (issueby == "4")
                        ddlis.Items.FindByText("Yearly").Selected = true;
                }
                string issuetype = Convert.ToString(ds.Tables[0].Rows[0]["IssueType"]);
                if (!string.IsNullOrEmpty(issuetype))
                {
                    if (issuetype == "1")
                        rblisty.SelectedIndex = 1;
                    if (issuetype == "2")
                        rblisty.SelectedIndex = 2;
                    if (issuetype == "3")
                        rblisty.SelectedIndex = 3;
                    if (issuetype == "4")
                        rblisty.SelectedIndex = 4;
                }
                string remarks = Convert.ToString(ds.Tables[0].Rows[0]["Remarks"]);
                if (!string.IsNullOrEmpty(remarks))
                    txtremark.Text = remarks;
                string web = Convert.ToString(ds.Tables[0].Rows[0]["Journal_Website"]);
                if (!string.IsNullOrEmpty(web))
                    txtjoweb.Text = web;

                string pertype = Convert.ToString(ds.Tables[0].Rows[0]["PeriodicalType"]);
                if (!string.IsNullOrEmpty(pertype))
                {
                    if (pertype == "1")
                        rblpubty.SelectedIndex = 1;
                    if (pertype == "2")
                        rblpubty.SelectedIndex = 2;
                }
                string isact = Convert.ToString(ds.Tables[0].Rows[0]["IsActive"]);
                if (!string.IsNullOrEmpty(isact))
                {
                    if (isact.ToLower() == "true")
                        rbljrweb.SelectedIndex = 1;
                    else
                        rbljrweb.SelectedIndex = 0;
                }

                string Jortype = Convert.ToString(ds.Tables[0].Rows[0]["Journal_Type"]);
                if (!string.IsNullOrEmpty(Jortype))
                {
                    ddljour.ClearSelection();
                    ddljour.Items.FindByText(Jortype).Selected = true;
                }
                string Lang = Convert.ToString(ds.Tables[0].Rows[0]["Lang"]);
                if (!string.IsNullOrEmpty(Lang))
                {
                    ddllanguage.ClearSelection();
                    ddllanguage.Items.FindByText(Lang).Selected = true;
                }
                string curval = Convert.ToString(ds.Tables[0].Rows[0]["currency_value"]);
                if (!string.IsNullOrEmpty(curval))
                    txtcurrencyvalue.Text = curval;
                string sup = Convert.ToString(ds.Tables[0].Rows[0]["Supplier"]);
                if (!string.IsNullOrEmpty(sup))
                {
                    ddlsupp.ClearSelection();
                    ddlsupp.Items.FindByText(sup).Selected = true;
                }
                string issn = Convert.ToString(ds.Tables[0].Rows[0]["ISSNNo"]);
                if (!string.IsNullOrEmpty(issn))
                    txtissn.Text = issn;

                string titleLang = Convert.ToString(ds.Tables[0].Rows[0]["TitleLanguage"]);
                if (!string.IsNullOrEmpty(titleLang))
                {
                    ddlengtam.ClearSelection();
                    if (titleLang == "1")
                        ddlengtam.Items.FindByValue(titleLang).Selected = true;
                    if (titleLang == "0")
                        ddlengtam.Items.FindByValue(titleLang).Selected = true;
                }
                string periss = Convert.ToString(ds.Tables[0].Rows[0]["PerIssueNo"]);
                if (!string.IsNullOrEmpty(periss))
                    txtperis.Text = periss;
                string totiss = Convert.ToString(ds.Tables[0].Rows[0]["TotalNoIssues"]);
                if (!string.IsNullOrEmpty(totiss))
                    txttotnois.Text = totiss;
                //string isstypeval = Convert.ToString(ds.Tables[0].Rows[0]["IssueTypeVAl"]);
                //if (!string.IsNullOrEmpty(isstypeval))
                //    txtdays.Text = isstypeval;
                ddllibararyname.ClearSelection();
                ddllibararyname.Items.FindByText(Convert.ToString(ds.Tables[0].Rows[0]["Lib_Name"])).Selected = true;
            }
        }
        catch (Exception ex) { }
    }

}







