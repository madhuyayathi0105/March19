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
using System.Text;
using System.Configuration;

public partial class LibraryMod_PeriodicalEntry : System.Web.UI.Page
{
    DAccess2 da = new DAccess2();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    DataTable dtCommon = new DataTable();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    ReuasableMethods rs = new ReuasableMethods();
    Hashtable columnhash = new Hashtable();
    Hashtable ht = new Hashtable();
    DataRow dr;
    DataRow drmonth;
    DataRow drjournal;
    DataSet dsmonth = new DataSet();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    string collegeCode = string.Empty;
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string qryUserOrGroupCode = string.Empty;
    string qryCollege = string.Empty;
    string accesscode = string.Empty;
    string journalcode = string.Empty;
    string title = string.Empty;
    string periodicity = string.Empty;
    string subsyear = string.Empty;
    string subsfrom = string.Empty;
    string substo = string.Empty;
    string journalmy = string.Empty;
    string date = string.Empty;
    string volumeno = string.Empty;
    string issuedno = string.Empty;
    string attachement = string.Empty;
    string select = string.Empty;
    string status = string.Empty;
    string collgcode = string.Empty;
    bool flag_true = false;
    bool cellflag = false;
    string collegecode = string.Empty;
    Boolean pageno = false;
    int ivalue = 0;
    int curpage = 0;
    double pageSize1 = 0.0;
    int pagecnt = 0;
    int pgsize = 0;
    static int searchby = 0;
    static string searchlibcode = string.Empty;
    DataTable perioden = new DataTable();
    DataTable periodmonth = new DataTable();
    DataTable periodjour = new DataTable();
    int selectedCellIndex = 0;

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
                collegecode = Session["Collegecode"].ToString();
          
            if (!IsPostBack)
            {
                txt_fromdate1.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate1.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                Bindcollege();
                getLibPrivil();
                suppliers();
                search();
                issued();
                type();
                language();
                subyear();
                bindlib();
                //subsyear1();
                // recieveddate();
                //attachment();
                //status1();

                cbyear.Checked = false;
                if (cbyear.Checked == false)
                {
                    ddlyear.Enabled = false;
                }
                else
                {
                    ddlyear.Enabled = true;
                }
                btnprint.Visible = false;
            }
        }
        catch
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
            query = "SELECT DISTINCT  TOP  100 journal_code FROM journal where journal_code Like '" + prefixText + "%' AND lib_code='" + searchlibcode + "'  order by journal_code";
        }
        else if (searchby == 2)
        {
            query = "SELECT DISTINCT  TOP  100 title FROM journal where title Like '" + prefixText + "%'  AND lib_code='" + searchlibcode + "'  order by title";
        }
        values = ws.Getname(query);
        return values;
    }

    #region BindHeaders

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
        }




    }

    public void BindLibrary(string LibCollection)
    {
        try
        {

            ddllibrary.Items.Clear();
            ds.Clear();
            if (ddlCollege.Items.Count > 0)
            {
                foreach (ListItem li in ddlCollege.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collegeCode))
                        {
                            collegeCode = "'" + li.Value + "'";
                        }
                        else
                        {
                            collegeCode = ",'" + li.Value + "'";
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(collegeCode))
            {
                string lib = "select lib_name,lib_code,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + LibCollection + " AND college_code=" + collegeCode + " ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
                ds = da.select_method_wo_parameter(lib, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddllibrary.DataSource = ds;
                    ddllibrary.DataValueField = "lib_code";
                    ddllibrary.DataTextField = "lib_name";
                    ddllibrary.DataBind();


                    ddllibname2.DataSource = ds;
                    ddllibname2.DataValueField = "lib_code";
                    ddllibname2.DataTextField = "lib_name";
                    ddllibname2.DataBind();

                    searchlibcode = Convert.ToString(ddllibrary.SelectedValue);
                }
            }



        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book Statistic"); }
    }
    
    public void suppliers()
    {
        try
        {

            ddlsup.Items.Clear();
            ds.Clear();
            if (ddlCollege.Items.Count > 0)
            {
                foreach (ListItem li in ddlCollege.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collgcode))
                        {
                            collgcode = "'" + li.Value + "'";
                        }
                        else
                        {
                            collgcode = ",'" + li.Value + "'";
                        }
                    }
                }
            }
            string supplier = ddlsup.SelectedValue.ToString();
            if (!string.IsNullOrEmpty(collgcode))
            {

                string suppl = "SELECT DISTINCT S.Supplier_Code,Supplier_Name FROM Subscription S,Supplier_Details D  where S.Supplier_Code = D.Supplier_Code";
                ds = da.select_method_wo_parameter(suppl, "text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlsup.DataSource = ds;
                ddlsup.DataTextField = "Supplier_Name";
                ddlsup.DataValueField = "Supplier_Name";
                ddlsup.DataBind();
                ddlsup.Items.Insert(0, "All");
            }
        }
        catch (Exception ex) { }
    }
    
    public void search()
    {
        try
        {


        }
        catch (Exception ex) { }
    }
    
    public void issued()
    {
        try
        {

        }
        catch (Exception ex) { }
    }
    
    public void language()
    {
        try
        {
            if (ddllang.SelectedIndex == 0)
            {
            }
            else
            {
            }


        }
        catch (Exception ex) { }
    }
    
    public void subyear()
    {
        try
        {
            ddlyear.Items.Clear();
            ds.Clear();
            string year = ddlyear.SelectedValue.ToString();

            if (!string.IsNullOrEmpty(collgcode))
            {

                string yer = "select distinct s.Subscription_Year from subscription s,library l where l.college_code=" + collgcode + " and l.lib_code=s.lib_code";
                ds = da.select_method_wo_parameter(yer, "text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlyear.DataSource = ds;
                ddlyear.DataTextField = "Subscription_Year";
                ddlyear.DataValueField = "Subscription_Year";
                ddlyear.DataBind();
                ddlyear.SelectedIndex = 0;
            }




        }
        catch
        {
        }
    }
        
    public void type()
    {
        try
        {


        }
        catch (Exception ex) { }
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

        BindLibrary(LibCollection);
        }
        catch (Exception ex)
        {
        }
    }
    
    #endregion

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            getLibPrivil();
        }
        catch (Exception ex) { }


    }

    protected void ddllibrary_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            searchlibcode = Convert.ToString(ddllibrary.SelectedValue);

        }
        catch (Exception ex) { }

    }

    protected void cbyear_OnCheckedChanged(object sender, EventArgs e)
    {

        try
        {
            if (cbyear.Checked == false)
            {
                ddlyear.Enabled = false;
            }
            else
            {
                ddlyear.Enabled = true;
            }

        }
        catch (Exception ex) { }
        {
        }

    }

    protected void ddlsup_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {


        }
        catch (Exception ex) { }

    }

    protected void ddlsearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlsearch.SelectedIndex == 0)
            {
                ddlsearch1.Visible = false;
                txtsearch.Visible = false;
                ddllang.Visible = false;
            }
            if (ddlsearch.SelectedIndex == 1)
            {
                txtsearch.Visible = true;
                ddlsearch1.Visible = false;
                lbllanguage.Visible = false;
                ddllang.Visible = false;

                searchby = 1;
            }
            if (ddlsearch.SelectedIndex == 2)
            {
                txtsearch.Visible = true;
                lbllanguage.Visible = true;
                ddllang.Visible = true;
                ddlsearch1.Visible = false;

                searchby = 2;
            }
            if (ddlsearch.SelectedIndex == 3)
            {
                ddlsearch1.Visible = true;
                txtsearch.Visible = false;
                ddllang.Visible = false;
                lbllanguage.Visible = false;
                ddlsearch1.Items.Add("Issued");
                ddlsearch1.Items.Add("Available");
                ddlsearch1.Items.Add("Lost");
                ddlsearch1.Items.Add("Binding");

            }

        }
        catch (Exception ex) { }

    }

    protected void ddlsearch1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex) { }

    }

    protected void ddllang_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {


        }
        catch (Exception ex) { }

    }

    protected void ddlissued_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {


        }
        catch (Exception ex) { }

    }

    protected void ddlyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {


        }
        catch (Exception ex) { }

    }

    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {


        }
        catch (Exception ex) { }

    }

    protected void cbdate1_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbdate1.Checked)
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
        catch (Exception ex) { }
        {
        }

    }

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        lblAlertMsg.Text = string.Empty;
        lblAlertMsg.Visible = false;
        divPopupAlert.Visible = false;
        lblAlertMsg.Text = string.Empty;
    }

    #region ButtonClick

    protected void grdPerEntry_onpageindexchanged(object sender, GridViewPageEventArgs e)
    {
        grdPerEntry.PageIndex = e.NewPageIndex;
        btn_go_Click(sender, e);
    }

    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            string issuby = string.Empty;
            string issueno = string.Empty;
            string qry = string.Empty;
            string typ = string.Empty;
            string typeno = string.Empty;
            string langno = string.Empty;
            DataSet entrydisp = new DataSet();

            divtable.Visible = true;
            grdPerEntry.Visible = true;
            issuby = Convert.ToString(ddlissued.SelectedItem.Value);
            string libCode = Convert.ToString(ddllibrary.SelectedValue);

            qry = "SELECT Access_Code,D.Journal_Code,Title,Periodicity,ISNULL(Subscription_Year,'') Subscription_Year,ISNULL(FromDate,'') FromDate,ISNULL(ToDate,'') ToDate,Volume_No,isnull(ActIssueNo,'') ActIssueNo,Attachement,Issue_Flag,Receive_Date,ISNULL(TamilJrnlName,'') TamilJrnlName,ISNULL(TitleLanguage,0) TitleLanguage,Issue_No,Issue_Month FROM Journal D INNER JOIN Journal_Master M ON M.Journal_Code = D.Journal_Code AND M.Lib_Code = D.Lib_Code INNER JOIN Subscription S ON S.Journal_Code = D.Journal_Code AND S.Subscription_Year = D.Subs_Year AND S.Lib_Code = D.Lib_Code INNER JOIN Supplier_Details U ON U.Supplier_Code = S.Supplier_Code ";
            if (libCode != "")
                qry = qry + "AND D.Lib_Code ='" + libCode + "' ";
            if (ddltype.SelectedIndex == 2)
            {
                qry = qry + " and isnull(periodicaltype,'1')=2";
            }
            else if (ddltype.SelectedIndex == 1)
            {
                qry = qry + " and  isnull(periodicaltype,'1')=1";
            }
            if (ddlsearch.SelectedIndex == 1)
            {
                qry = qry + " and d.journal_code='" + txtsearch.Text + "'";
            }
            else if (ddlsearch.SelectedIndex == 2)
            {
                if (ddllang.SelectedIndex == 0)
                {
                    langno = "0";

                    qry = qry + " and title='" + txtsearch.Text + "' and TitleLanguage='" + langno + "' ";
                }
                else
                {
                    langno = "1";
                    qry = qry + " and title='" + txtsearch.Text + "' and TitleLanguage='" + langno + "' ";
                }
            }
            else if (ddlsearch.SelectedIndex == 3)
            {
                qry = qry + " and Issue_Flag='" + ddlsearch1.SelectedItem.Text + "'";
            }
            if (ddlsup.SelectedIndex > 0)
            {
                qry = qry + " and m.Supplier='" + ddlsup.SelectedItem.Text + "'";
            }
            if (cbyear.Checked == true)
            {
                qry = qry + "  and Subscription_Year='" + ddlyear.SelectedItem.Text + "'";
            }
            if (cbdate1.Checked == true)
            {
                qry = qry + " and fromdate='" + txt_fromdate1.Text + "' and todate='" + txt_todate1.Text + "'";
            }
            if (issuby != "All")
                qry = qry + "AND IssueBY ='" + issuby + "'";

            qry = qry + " order by Title,D.Journal_Code,Subscription_Year,LEN(Issue_No) DESC,Issue_No DESC ";
            entrydisp = d2.select_method_wo_parameter(qry, "text");
            if (entrydisp.Tables[0].Rows.Count > 0 && entrydisp.Tables.Count > 0)
            {
                perioden.Columns.Add("SNo", typeof(string));
                perioden.Columns.Add("Access No", typeof(string));
                perioden.Columns.Add("Journal Code", typeof(string));
                perioden.Columns.Add("Title", typeof(string));
                perioden.Columns.Add("Periodicity", typeof(string));
                perioden.Columns.Add("Subs Year", typeof(string));
                perioden.Columns.Add("Subs Period From", typeof(string));
                perioden.Columns.Add("Subs Period To", typeof(string));
                perioden.Columns.Add("Journal Month & Year", typeof(string));
                perioden.Columns.Add("Recieved Date", typeof(string));
                perioden.Columns.Add("Volume No", typeof(string));
                perioden.Columns.Add("Issue No", typeof(string));
                perioden.Columns.Add("Attachment", typeof(string));
                perioden.Columns.Add("Status", typeof(string));
                int sno = 0;
                int row = 0;

             

                for (row = 0; row < entrydisp.Tables[0].Rows.Count; row++)
                {
                    sno++;
                    dr = perioden.NewRow();
                    accesscode = Convert.ToString(entrydisp.Tables[0].Rows[row]["Access_Code"]).Trim();
                    journalcode = Convert.ToString(entrydisp.Tables[0].Rows[row]["Journal_Code"]).Trim();
                    title = Convert.ToString(entrydisp.Tables[0].Rows[row]["Title"]).Trim();
                    periodicity = Convert.ToString(entrydisp.Tables[0].Rows[row]["Periodicity"]).Trim();
                    subsyear = Convert.ToString(entrydisp.Tables[0].Rows[row]["Subscription_Year"]).Trim();
                    subsfrom = Convert.ToString(entrydisp.Tables[0].Rows[row]["FromDate"]).Trim();
                    substo = Convert.ToString(entrydisp.Tables[0].Rows[row]["ToDate"]).Trim();
                    journalmy = Convert.ToString(entrydisp.Tables[0].Rows[row]["Issue_Month"]).Trim();
                    date = Convert.ToString(entrydisp.Tables[0].Rows[row]["Receive_Date"]).Trim();
                    volumeno = Convert.ToString(entrydisp.Tables[0].Rows[row]["Volume_No"]).Trim();
                    issuedno = Convert.ToString(entrydisp.Tables[0].Rows[row]["Issue_No"]).Trim();
                    attachement = Convert.ToString(entrydisp.Tables[0].Rows[row]["Attachement"]).Trim();
                    status = Convert.ToString(entrydisp.Tables[0].Rows[row]["Issue_Flag"]).Trim();
                    dr["SNo"] = Convert.ToString(sno);
                    dr["Access No"] = accesscode;
                    dr["Journal Code"] = journalcode;
                    dr["Title"] = title;
                    dr["Periodicity"] = periodicity;
                    dr["Subs Year"] = subsyear;
                    string[] dtsubsfrom = subsfrom.Split('/');
                    if (dtsubsfrom.Length == 3)
                        subsfrom = dtsubsfrom[1].ToString() + "/" + dtsubsfrom[0].ToString() + "/" + dtsubsfrom[2].ToString();
                    dr["Subs Period From"] = subsfrom.Split(' ')[0];

                    string[] dtsubsto = substo.Split('/');
                    if (dtsubsto.Length == 3)
                        substo = dtsubsto[1].ToString() + "/" + dtsubsto[0].ToString() + "/" + dtsubsto[2].ToString();
                    dr["Subs Period To"] = substo.Split(' ')[0];

                    string[] dtjournalmy = journalmy.Split('/');
                    if (dtjournalmy.Length == 3)
                        journalmy = dtjournalmy[1].ToString() + "/" + dtjournalmy[0].ToString() + "/" + dtjournalmy[2].ToString();
                    dr["Journal Month & Year"] = journalmy;

                    string[] dtdate = date.Split('/');
                    if (dtdate.Length == 3)
                        date = dtdate[1].ToString() + "/" + dtdate[0].ToString() + "/" + dtdate[2].ToString();
                    dr["Recieved Date"] = date.Split(' ')[0];
                    dr["Volume No"] = volumeno;
                    dr["Issue No"] = issuedno;
                    dr["Attachment"] = attachement;
                    dr["Status"] = status;
                    perioden.Rows.Add(dr);
                }
                chkGridSelectAll.Visible = true;
                grdPerEntry.DataSource = perioden;
                grdPerEntry.DataBind();
                grdPerEntry.Visible = true;
              
                div_report.Visible = true;
                for (int l = 0; l < grdPerEntry.Rows.Count; l++)
                {
                    foreach (GridViewRow rowss in grdPerEntry.Rows)
                    {
                        foreach (TableCell cell in rowss.Cells)
                        {
                            grdPerEntry.Rows[l].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            grdPerEntry.Rows[l].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                            grdPerEntry.Rows[l].Cells[6].HorizontalAlign = HorizontalAlign.Right;
                            grdPerEntry.Rows[l].Cells[11].HorizontalAlign = HorizontalAlign.Right;
                            grdPerEntry.Rows[l].Cells[12].HorizontalAlign = HorizontalAlign.Right;
                        }
                    }
                }

               
            }
            else
            {
                grdPerEntry.Visible = false;
                divPopupAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No Records Found";
                //div_report.Visible = false;
            }


           

        }

        catch
        {
        }


    }

  
    protected void grdPerEntry_OnRowCreated(object sender, GridViewRowEventArgs e)
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

    protected void grdPerEntry_RowDataBound(object sender, GridViewRowEventArgs e)
    {
       
    }

    protected void grdPerEntry_onselectedindexchanged(object sender, EventArgs e)
    {
        try
        {
            divsaveDetails.Visible = true;
            divsaventry.Visible = true;
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            if (ddlCollege.Items.Count > 0)
            {
                collegecode = string.Empty;
                foreach (ListItem li in ddlCollege.Items)
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
                //libararyname();
                //publis();
                //dept();
                //binjournalty();
                //periodi();
                //deliver();
                //supp();
                //subj();
                //language3();
                bindlib();
                subsyear1();
                // recieveddate();
                attachment();
                status1();
                // btngo.ImageUrl = "~/LibImages/GoWhite.jpg";
                btnsave.ImageUrl = "~/LibImages/update (2).jpg";

                txtaccess.Text = Convert.ToString(grdPerEntry.Rows[0].Cells[2].Text);
                txtjour.Text = Convert.ToString(grdPerEntry.Rows[0].Cells[3].Text);
                string qry = "select * from journal where access_code='" + txtaccess.Text + "' and journal_code='" + txtjour.Text + "'";
                ds.Clear();
                ds = da.select_method_wo_parameter(qry, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txttit.Text = Convert.ToString(ds.Tables[0].Rows[0]["title"]).Trim();
                    ddlsubsyr.SelectedItem.Text = Convert.ToString(ds.Tables[0].Rows[0]["Subs_Year"]).Trim();
                    txtmonth.Items.Add(Convert.ToString(ds.Tables[0].Rows[0]["Issue_Month"]).Trim());
                    txtprice.Text = Convert.ToString(ds.Tables[0].Rows[0]["Price"]).Trim();
                    txtpgto.Text = Convert.ToString(ds.Tables[0].Rows[0]["Pages"]).Trim();
                    txtvol.Text = Convert.ToString(ds.Tables[0].Rows[0]["volume_no"]).Trim();
                    txtissueno.Text = Convert.ToString(ds.Tables[0].Rows[0]["issue_no"]).Trim();
                    txtISSN.Text = Convert.ToString(ds.Tables[0].Rows[0]["issn"]).Trim();
                    ddlattach.SelectedItem.Text = Convert.ToString(ds.Tables[0].Rows[0]["attachement"]).Trim();
                    txtremark.Text = Convert.ToString(ds.Tables[0].Rows[0]["remarks"]).Trim();
                    ddlstatus.SelectedItem.Text = Convert.ToString(ds.Tables[0].Rows[0]["issue_flag"]).Trim();

                }


            }


        }

        catch
        {
        }
    }

    protected void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string attendance = "Periodical Entry";
            string pagename = "PeriodicalEntry.aspx";
            string ss = Session["usercode"].ToString();
            Printcontrolhed2.loadspreaddetails(grdPerEntry, pagename, attendance, 0, ss,"1");
            Printcontrolhed2.Visible = true;
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
                da.printexcelreportgrid(grdPerEntry, report);
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

    public override void VerifyRenderingInServerForm(Control control)
    { }

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

    protected void btn_go1_Click(object sender, EventArgs e)
    {
        try
        {
            divjourcode.Visible = true;
            divjourcod.Visible = true;
            string jcod = string.Empty;
            string journlcod = string.Empty;
            string jname = string.Empty;
            string dep = string.Empty;
            string code = "";


            if (ddlCollege.Items.Count > 0)
            {
                foreach (ListItem li in ddlCollege.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collgcode))
                        {
                            collgcode = "'" + li.Value + "'";
                        }
                        else
                        {
                            collgcode = ",'" + li.Value + "'";
                        }
                    }
                }
            }
            if (txtcode.Text != "")
                code = "and journal_code='" + txtcode.Text + "'";
            if (txttit1.Text != "")
                title = "and journal_name ='" + txttit1.Text + "' ";

            jcod = "select distinct jm.journal_code,jm.journal_name,jm.department,jm.lib_code from journal_master jm,library l where l.college_code=" + collgcode + " and l.lib_code=jm.lib_code  " + code + title + " order by jm.journal_name ";
            DataSet dsjour = new DataSet();
            dsjour = d2.select_method_wo_parameter(jcod, "text");
            if (dsjour.Tables.Count > 0 && dsjour.Tables[0].Rows.Count > 0)
            {
                periodjour.Columns.Add("Journal Code", typeof(string));
                periodjour.Columns.Add("Journal Title", typeof(string));
                periodjour.Columns.Add("Department", typeof(string));


                int sno = 0;
                for (int i = 0; i < dsjour.Tables[0].Rows.Count; i++)
                {
                    sno++;
                    drjournal = periodjour.NewRow();
                    journalcode = Convert.ToString(dsjour.Tables[0].Rows[i]["journal_code"]);
                    jname = Convert.ToString(dsjour.Tables[0].Rows[i]["journal_name"]);
                    dep = Convert.ToString(dsjour.Tables[0].Rows[i]["department"]);
                    string lib_code = Convert.ToString(dsjour.Tables[0].Rows[i]["lib_code"]);


                    drjournal["Journal Code"] = journalcode;
                    drjournal["Journal Title"] = jname;
                    drjournal["Department"] = dep;

                    periodjour.Rows.Add(drjournal);
                }
                grdJournalCode.DataSource = periodjour;
                grdJournalCode.DataBind();
                grdJournalCode.Visible = true;
                div1jour.Visible = true;
                btnex.Visible = true;
            }
            else
            {
                divPopupAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No Records Found";
            }
        }
        catch
        {
        }
    }

    protected void grdJournalCode_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , HiddenField1.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }

    protected void grdJournalCode_onselectedindexchanged(object sender, EventArgs e)
    {
        try
        {
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            string libcode = "";

            DataSet dsjour = new DataSet();
            if (ddllibname.Items.Count > 0)
                libcode = Convert.ToString(ddllibname.SelectedValue);

            if (div1.Visible == false && div2.Visible == false)
            {
                string journalco = grdJournalCode.Rows[rowIndex].Cells[1].Text;
                string journalt = grdJournalCode.Rows[rowIndex].Cells[2].Text;
                txtjour.Text = journalco;
                txttit.Text = journalt;
                if (libcode != "" && journalco != "")
                {
                    string Sql = "select ISNULL(Periodicity,'') Periodicity,ISNULL(IssueBy,0) IssueBy,ISNULL(PerIssueNo,0) PerIssueNo,ISNULL(TotalNoIssues,0) TotalNoIssues,ISNULL(IssueType,0) IssueType,ISNULL(IssueTypeVAl,'') IssueTypeVAl,ISNULL(TamilJrnlName ,'') TamilJrnlName,isnull(TitleLanguage,0) TitleLanguage,journal_price  from journal_master where journal_code = '" + journalco + "' ";
                    dsjour.Clear();
                    dsjour = d2.select_method_wo_parameter(Sql, "Text");
                    if (dsjour.Tables[0].Rows.Count > 0)
                    {
                        txtperiod.Text = Convert.ToString(dsjour.Tables[0].Rows[0]["Periodicity"]);
                    }
                }
            }
            else
            {
                string journalt = grdJournalCode.Rows[rowIndex].Cells[2].Text;
                string jcode1 = grdJournalCode.Rows[rowIndex].Cells[1].Text;
                txtjourname1.Text = journalt;
                Label_jc.Text = jcode1;
                divjourcod.Visible = false;
                divjourcode.Visible = false;
            }
            ddlsubsyear_SelectedIndexChanged(sender, e);
            txtmonth_OnSelectedIndexChanged(sender, e);
            divjourcod.Visible = false;
            divjourcode.Visible = false;
        }
        catch
        {
        }
    }

    protected void btn_ex_Click(object sender, EventArgs e)
    {
        div1jour.Visible = false;
        divjourcod.Visible = false;
        divjourcode.Visible = false;

    }

    protected void btn_add_Click(object sender, EventArgs e)
    {
        divsaveDetails.Visible = true;
        divsaventry.Visible = true;
        //btnsave.Text = "Save";
        btnsave.ImageUrl = "~/LibImages/save.jpg";
        bindlib();
        subsyear1();
        // recieveddate();
        attachment();
        status1();

        TextBox1.Attributes.Add("readonly", "readonly");
        TextBox1.Text = DateTime.Now.ToString("dd-MMM-yyyy");

        TextBox3.Attributes.Add("readonly", "readonly");
        TextBox3.Text = DateTime.Now.ToString("dd-MMM-yyyy");

        TextBox4.Attributes.Add("readonly", "readonly");
        TextBox4.Text = DateTime.Now.ToString("dd-MMM-yyyy");


    }

    protected void btn_print_Click(object sender, EventArgs e)
    {
    }

    protected void btn_Save_Click(object sender, EventArgs e)
    {
        try
        {
            int insertqry1 = 0;
            string insertqry2 = "";

            if (txtaccess.Text != "")
            {
                if (btnsave.ImageUrl == "~/LibImages/save.jpg")
                    insertqry2 = "if not exists (select * from journal where access_code='" + txtaccess.Text + "' and journal_code='" + txtjour.Text + "' and title='" + txttit.Text + "' and  Subs_Year='" + Convert.ToString(ddlsubsyr.SelectedItem.Text) + "' and Issue_Month='" + txtmonth.Items[0].Text + "' and Price='" + txtprice.Text + "' and Pages='" + txtpgto.Text + "' and volume_no='" + txtvol.Text + "' and issue_no='" + txtissueno.Text + "' and issn='" + txtISSN.Text + "' and attachement='" + Convert.ToString(ddlattach.SelectedItem.Text) + "' and remarks='" + txtremark.Text + "' and issue_flag='" + Convert.ToString(ddlstatus.SelectedItem.Text) + "') insert into journal(access_code,journal_code,title,Subs_Year,Issue_Month,Price,Pages,volume_no,issue_no,issn,attachement,remarks,issue_flag)values('" + txtaccess.Text + "','" + txtjour.Text + "','" + txttit.Text + "','" + Convert.ToString(ddlsubsyr.SelectedItem.Text) + "','" + txtmonth.Items[0].Text + "','" + txtprice.Text + "','" + txtpgto.Text + "','" + txtvol.Text + "','" + txtissueno.Text + "','" + txtISSN.Text + "','" + Convert.ToString(ddlattach.SelectedItem.Text) + "','" + txtremark.Text + "','" + Convert.ToString(ddlstatus.SelectedItem.Text) + "') else update journal set  journal_code='" + txtjour.Text + "' , title='" + txttit.Text + "' ,  Subs_Year='" + Convert.ToString(ddlsubsyr.SelectedItem.Text) + "' , Issue_Month='" + txtmonth.Items[0].Text + "' , Price='" + txtprice.Text + "' , Pages='" + txtpgto.Text + "' , volume_no='" + txtvol.Text + "' , issue_no='" + txtissueno.Text + "' , issn='" + txtISSN.Text + "' , attachement='" + Convert.ToString(ddlattach.SelectedItem.Text) + "' , remarks='" + txtremark.Text + "' , issue_flag='" + Convert.ToString(ddlstatus.SelectedItem.Text) + "' where access_code='" + txtaccess.Text + "'";
                else
                    insertqry2 = "update journal set  journal_code='" + txtjour.Text + "' , title='" + txttit.Text + "' ,  Subs_Year='" + Convert.ToString(ddlsubsyr.SelectedItem.Text) + "' , Issue_Month='" + txtmonth.Items[0].Text + "' , Price='" + txtprice.Text + "' , Pages='" + txtpgto.Text + "' , volume_no='" + txtvol.Text + "' , issue_no='" + txtissueno.Text + "' , issn='" + txtISSN.Text + "' , attachement='" + Convert.ToString(ddlattach.SelectedItem.Text) + "' , remarks='" + txtremark.Text + "' , issue_flag='" + Convert.ToString(ddlstatus.SelectedItem.Text) + "' where access_code='" + txtaccess.Text + "'";
                insertqry1 = da.update_method_wo_parameter(insertqry2, "text");
            }
            if (insertqry1 == 0)
            {
                divPopupAlert.Visible = true;
                lblAlertMsg.Visible = true;
                if (btnsave.ImageUrl == "~/LibImages/save.jpg")
                    lblAlertMsg.Text = "Not Saved";
                else
                    lblAlertMsg.Text = "Not Updated";
                grdPerEntry.Visible = false;
                divsaveDetails.Visible = false;
                divsaventry.Visible = false;
                cleartxt();

            }
            else
            {
                divPopupAlert.Visible = true;
                lblAlertMsg.Visible = true;
                if (btnsave.ImageUrl == "~/LibImages/save.jpg")
                    lblAlertMsg.Text = "Saved Successfully";
                else
                    lblAlertMsg.Text = "Updated Successfully";
                grdPerEntry.Visible = false;
                divsaveDetails.Visible = false;
                divsaventry.Visible = false;
                cleartxt();
            }


        }
        catch
        {
        }
    }

    protected void btn_exit_Click(object sender, EventArgs e)
    {
        divsaventry.Visible = false;
        divsaveDetails.Visible = false;
        cleartxt();
    }

    protected void btn_jour_Click(object sender, EventArgs e)
    {
        try
        {
            divjourcode.Visible = true;
            divjourcod.Visible = true;

        }
        catch
        {
        }

    }
    #endregion

    public void cleartxt()
    {
        txtaccess.Text = string.Empty;
        txtjour.Text = string.Empty;
        txttit.Text = string.Empty;
        TextBox3.Text = string.Empty;
        TextBox4.Text = string.Empty;
        txtprice.Text = string.Empty;
        //txtmon.Text = string.Empty;
        txtpgto.Text = string.Empty;
        // txtperiod.Text=string.Empty;
        txtvol.Text = string.Empty;
        txtissueno.Text = string.Empty;
        txtISSN.Text = string.Empty;
        txtremark.Text = string.Empty;
        btnsave.ImageUrl = "~/LibImages/save.jpg";
    }

    public void bindlib()
    {
        try
        {
            ddllibname.Items.Clear();
            ds1.Clear();
            string College = ddlCollege.SelectedValue.ToString();
            string SelectQ = string.Empty;
            if (!string.IsNullOrEmpty(College))
            {
                dicQueryParameter.Clear();
                dicQueryParameter.Add("CollegeCode", Convert.ToString(College));
                ds1 = storeAcc.selectDataSet("[GetLibrary]", dicQueryParameter);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    ddllibname.DataSource = ds1;
                    ddllibname.DataTextField = "lib_name";
                    ddllibname.DataValueField = "lib_code";
                    ddllibname.DataBind();
                    ddllibname.Items.Insert(0, "All");
                }
            }
        }
        catch (Exception ex) { }
    }

    public void subsyear1()
    {
        try
        {
            ddlsubsyr.Items.Clear();
            ds1.Clear();
            string year = ddlyear.SelectedValue.ToString();
            if (ddlCollege.Items.Count > 0)
            {
                foreach (ListItem li in ddlCollege.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collgcode))
                        {
                            collgcode = "'" + li.Value + "'";
                        }
                        else
                        {
                            collgcode = ",'" + li.Value + "'";
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(collgcode))
            {

                string yer = "select distinct s.Subscription_Year from subscription s,library l where l.college_code=" + collgcode + " and l.lib_code=s.lib_code";
                ds1 = da.select_method_wo_parameter(yer, "text");
            }
            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                ddlsubsyr.DataSource = ds1;
                ddlsubsyr.DataTextField = "Subscription_Year";
                ddlsubsyr.DataValueField = "Subscription_Year";
                ddlsubsyr.DataBind();
                ddlsubsyr.SelectedIndex = 0;

                ddlissueyr.DataSource = ds1;
                ddlissueyr.DataTextField = "Subscription_Year";
                ddlissueyr.DataValueField = "Subscription_Year";
                ddlissueyr.DataBind();
                ddlissueyr.SelectedIndex = 0;


            }




        }
        catch
        {
        }
    }

    public void attachment()
    {
        try
        {
            string attach1 = string.Empty;
            if (!string.IsNullOrEmpty(collgcode))
            {
                attach1 = "select distinct attachement FROM Journal d ,Journal_Master m,Subscription S,Supplier_Details U where M.Journal_Code = D.Journal_Code AND M.Lib_Code = D.Lib_Code and S.Journal_Code = D.Journal_Code AND S.Subscription_Year = D.Subs_Year AND S.Lib_Code = D.Lib_Code and U.Supplier_Code = S.Supplier_Code";
                ds1.Clear();
                ds1 = d2.select_method_wo_parameter(attach1, "text");
            }
            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                ddlattach.DataSource = ds1;
                ddlattach.DataTextField = "Attachement";
                ddlattach.DataValueField = "Attachement";
                ddlattach.DataBind();

            }
        }
        catch
        {
        }
    }

    public void status1()
    {
        try
        {
            ddlstatus.Items.Add("Issued");
            ddlstatus.Items.Add("Available");
            ddlstatus.Items.Add("Lost");
            ddlstatus.Items.Add("Binding");
        }
        catch
        {
        }
    }

    protected void ddllibname_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void cbautocode_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txtaccess.Text = "";
            if (chkautocode.Checked == true)
            {
                if (ddlCollege.Items.Count > 0)
                {
                    foreach (ListItem li in ddlCollege.Items)
                    {
                        if (li.Selected)
                        {
                            if (string.IsNullOrEmpty(collgcode))
                            {
                                collgcode = "'" + li.Value + "'";
                            }
                            else
                            {
                                collgcode = ",'" + li.Value + "'";
                            }
                        }
                    }
                }
                string qry3 = string.Empty;
                qry3 = "select access_code from journal j,library l where l.lib_code=j.lib_code and l.college_code=" + collgcode + "";
                ds1 = d2.select_method_wo_parameter(qry3, "text");
                if (ds1.Tables[0].Rows.Count > 0 && ds1.Tables.Count > 0)
                {

                    string acc_code = Convert.ToString(ds1.Tables[0].Rows[0]["access_code"]);
                    string acode = acc_code.Remove(0, 3);
                    int code = Convert.ToInt32(acode) + 1;
                    txtaccess.Text = Convert.ToString(code);

                }

                else
                {
                    txtaccess.Text = "PER1";
                }

            }

            else
            {
            }
        }
        catch
        {
        }

    }

    protected void ddlsubsyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            string year = Convert.ToString(ddlsubsyr.SelectedValue);
            txtmonth.Items.Clear();
            if (!string.IsNullOrEmpty(year) && txtjour.Text != "" && Labellibcode.Text != "")
            {
                string sql1 = "SELECT DISTINCT CONVERT(varchar(20),I.IssueDate,103) IssueDate ,CASE WHEN Issue_Status = 1 THEN 'Received' WHEN Issue_Status = 0 AND ISNULL(IssueDate,'') <= getdate() THEN 'Pending' ELSE '' END Status, CASE WHEN Issue_Status = 1 THEN Receive_Date ELSE '' END Received_Date FROM Journal_Issues I  LEFT JOIN Journal J ON I.Journal_Code = J.Journal_Code AND I.Subs_Year = J.Subs_Year AND I.Lib_Code = J.Lib_Code AND I.IssueNo = J.Issue_No INNER JOIN Journal_Master M ON I.Journal_Code = M.Journal_Code AND I.Lib_Code = M.Lib_Code AND I.Subs_Year='" + year + "' AND I.Lib_Code='" + Labellibcode.Text + "' AND I.Journal_Code='" + txtjour.Text + "' ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(sql1, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtmonth.DataSource = ds;
                    txtmonth.DataTextField = "IssueDate";
                    txtmonth.DataValueField = "IssueDate";
                    txtmonth.DataBind();


                }
                if (txtmonth.Items.Count > 0)
                {
                    TextBox3.Text = Convert.ToString(txtmonth.Items[0]);
                    TextBox4.Text = Convert.ToString(txtmonth.Items[txtmonth.Items.Count - 1]);
                    ddlissueyr.Text = year;
                }
            }
        }
        catch
        {


        }

    }

    protected void txtmonth_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string month = Convert.ToString(txtmonth.SelectedValue);
            string year = Convert.ToString(ddlsubsyr.SelectedValue);

            string issueno = d2.GetFunction("SELECT I.IssueNo FROM Journal_Issues I LEFT JOIN Journal J ON I.Journal_Code = J.Journal_Code AND I.Subs_Year = J.Subs_Year AND I.Lib_Code = J.Lib_Code AND I.IssueNo = J.Issue_No INNER JOIN Journal_Master M ON I.Journal_Code = M.Journal_Code AND I.Lib_Code = M.Lib_Code AND I.Subs_Year='" + year + "' AND I.Lib_Code='" + Labellibcode.Text + "' AND I.Journal_Code='" + txtjour.Text + "' and CONVERT(varchar(20),I.IssueDate,103)='" + month + "' ");
            if (issueno != "")
                txtissueno.Text = issueno;
        }
        catch
        {

        }

    }

    protected void ddlissueyear_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void cbdate_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chkdat.Checked == true)
        {
            TextBox3.Enabled = true;
            TextBox4.Enabled = true;
        }
        else
        {
            TextBox3.Enabled = false;
            TextBox4.Enabled = false;
        }
    }

    protected void ddlrecdate_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void ddlattach_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void ddlstatus_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void ddllan1_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void btngopendinglist_Click(object sender, EventArgs e)
    {
        try
        {
            string StrMonth = "";
            string month = "";
            string yar = "";
            string lcode = "";
            string jcode = "";
            string sqlqry = "";

            if (cblmonth.Items.Count > 0)
                month = Convert.ToString(rs.getCblSelectedValue(cblmonth));
            if (ddlsubyr3.Items.Count > 0)
                yar = Convert.ToString(ddlsubyr3.SelectedValue);
            if (ddllibname2.Items.Count > 0)
                lcode = Convert.ToString(ddllibname2.SelectedValue);
            if (Label_jc.Text != "")
                jcode = "  AND I.Journal_Code='" + Label_jc.Text + "'";
            if (txtjour.Text != "")
                jcode = "  AND I.Journal_Code='" + txtjour.Text + "'";
            string currdate = DateTime.Now.ToString("yyyy/MM/dd");

            sqlqry = "SELECT DISTINCT IssueCode,Journal_Name,IssueMonth,CONVERT(varchar(20),I.IssueDate,103)IssueDate,i.IssueDay,I.IssueNo,I.MonthIssue_No,CASE WHEN Issue_Status = 1 THEN 'Received' WHEN Issue_Status = 0 AND ISNULL(IssueDate,'') <= getdate() THEN 'Pending' ELSE '' END Status,CASE WHEN Issue_Status = 1 THEN CONVERT(varchar(20),Receive_Date,103) END Received_Date FROM Journal_Issues I LEFT JOIN Journal J ON I.Journal_Code = J.Journal_Code AND I.Subs_Year = J.Subs_Year AND I.Lib_Code = J.Lib_Code AND I.IssueNo = J.Issue_No INNER JOIN Journal_Master M ON I.Journal_Code = M.Journal_Code AND I.Lib_Code = M.Lib_Code AND I.Subs_Year ='" + yar + "' AND I.Lib_Code='" + lcode + "' " + jcode + " ";


            if (txtisnumber.Text != "")
                sqlqry = sqlqry + " AND I.IssueNo ='" + txtisnumber.Text + "'";
            if (txtmonissno.Text != "")
                sqlqry = sqlqry + " AND I.MonthIssue_No ='" + txtmonissno.Text + "'";
            if (rblStatus.SelectedIndex == 0)
                sqlqry = sqlqry + " AND Issue_Status =1";
            if (rblStatus.SelectedIndex == 1)
                sqlqry = sqlqry + "  AND Issue_Status =0 AND ISNULL(IssueDate,'') <='" + currdate + "' ";
            dsmonth.Clear();
            dsmonth = d2.select_method_wo_parameter(sqlqry, "Text");

            if (dsmonth.Tables[0].Rows.Count > 0)
            {
                periodmonth.Columns.Add("Issue Month", typeof(string));
                periodmonth.Columns.Add("Issue Date", typeof(string));
                periodmonth.Columns.Add("Issue Day", typeof(string));
                periodmonth.Columns.Add("Issue No", typeof(string));
                periodmonth.Columns.Add("Status", typeof(string));
                periodmonth.Columns.Add("ReceivedDate", typeof(string));



                int sno = 0;
                for (int i = 0; i < dsmonth.Tables[0].Rows.Count; i++)
                {
                    sno++;
                    drmonth = periodmonth.NewRow();
                    drmonth["Issue Month"] = Convert.ToString(dsmonth.Tables[0].Rows[i]["IssueMonth"]);
                    drmonth["Issue Date"] = Convert.ToString(dsmonth.Tables[0].Rows[i]["IssueDate"]);
                    drmonth["Issue Day"] = Convert.ToString(dsmonth.Tables[0].Rows[i]["IssueDay"]);
                    drmonth["Issue No"] = Convert.ToString(dsmonth.Tables[0].Rows[i]["IssueNo"]);
                    drmonth["Status"] = Convert.ToString(dsmonth.Tables[0].Rows[i]["Status"]);
                    drmonth["ReceivedDate"] = Convert.ToString(dsmonth.Tables[0].Rows[i]["Received_Date"]);
                    periodmonth.Rows.Add(drmonth);

                }
                grdJournalPending.DataSource = periodmonth;
                grdJournalPending.DataBind();
                grdJournalPending.Visible = true;
                div3.Visible = true;
                btnk.Visible = true;

            }
            else
            {
                grdJournalPending.Visible = false;
                divPopupAlert.Visible = true;
                lblAlertMsg.Visible = true;
                lblAlertMsg.Text = "No Records Found";
            }

        }
        catch
        {


        }

    }

    protected void chkmonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        int i = 0;
        chkmonth.Checked = false;
        int commcount = 0;
        // Txtfromyear.Text = Txtyear.Text;

        txtmonth.Text = "--Select--";
        for (i = 0; i < cblmonth.Items.Count; i++)
        {
            if (cblmonth.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cblmonth.Items.Count)
            {
                chkmonth.Checked = true;
            }
            txtmonth12.Text = "Month(" + commcount.ToString() + ")";
        }

    }

    protected void chkmonth_CheckedChanged(object sender, EventArgs e)
    {
        txtmonth.Text = "--Select--";

        if (chkmonth.Checked == true)
        {

            for (int i = 0; i < cblmonth.Items.Count; i++)
            {
                cblmonth.Items[i].Selected = true;
            }
            txtmonth12.Text = "Month(" + (cblmonth.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cblmonth.Items.Count; i++)
            {
                cblmonth.Items[i].Selected = false;
            }
            txtmonth12.Text = "--Select--";
        }
    }

    protected void ddlsubyr3_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void btnjoucode_Click(object sender, EventArgs e)
    {
        divjourcode.Visible = true;
        divjourcod.Visible = true;
    }

    protected void ddllibname2_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void btn_month_Click(object sender, EventArgs e)
    {
        if (txttit.Text != "")
        {
            txtjourname1.Text = txttit.Text;

        }
        div1.Visible = true;
        div2.Visible = true;
        getLibPrivil();
    }

    protected void btnexb_Click(object sender, EventArgs e)
    {
        div1.Visible = false;
        div2.Visible = false;
    }

    protected void grdJournalPending_OnRowCreated(object sender, GridViewRowEventArgs e)
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

    protected void grdJournalPending_onselectedindexchanged(object sender, EventArgs e)
    {
        try
        {
            divsaveDetails.Visible = true;
            divsaventry.Visible = true;
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);

        }

        catch
        {
        }
    }

    protected void btnk_okk1_Click(object sender, EventArgs e)
    {
        try
        {
            string month = grdJournalPending.Rows[selectedCellIndex].Cells[2].Text;
            txtmonth.Items.Insert(0, month);

            div1.Visible = false;
            div2.Visible = false;
        }
        catch
        {

        }
    }   

}