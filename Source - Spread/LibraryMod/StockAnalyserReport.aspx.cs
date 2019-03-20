using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class LibraryMod_StockAnalyserReport : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet dsload = new DataSet();
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string selectQuery = string.Empty;
    string Sql = string.Empty;
    DataTable dtreport = new DataTable();
    DataRow drow = null;
    ArrayList arrColHdrNames = new ArrayList();
    int SNo = 0;
    Dictionary<int, string> dicColor = new Dictionary<int, string>();

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
          
            if (!Page.IsPostBack)
            {
                txtfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                Bindcollege();
                getLibPrivil();
                int count = 0;
                for (int i = 1950; i <= DateTime.Now.AddYears(7).Year; i++)
                {
                    ddlYear.Items.Add(i.ToString());
                    count++;
                }
                ddlYear.Items.Insert(count, "All");
                string year = Convert.ToString(DateTime.Now.ToString("yyyy"));
                ddlYear.Items.FindByText(year).Selected = true;
            }
        }
        catch (Exception ex) { }
    }

    #region Collge

    public void Bindcollege()
    {
        try
        {
            string strUser = d2.getUserCode(Convert.ToString(Session["group_code"]), Convert.ToString(Session["usercode"]), 1);
            ds.Clear();
            ddlCollege.Items.Clear();
            selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where " + strUser + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(selectQuery, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlCollege.DataSource = ds;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
            }
        }
        catch (Exception ex) { }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            getLibPrivil();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "StockAnalyserReport");
        }
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
            loadlibrary(LibCollection);
        }
        catch (Exception ex)
        {
        }
    }

    public void loadlibrary(string LibCollection)
    {
        try
        {
            string collegeCode = Convert.ToString(ddlCollege.SelectedValue);
            selectQuery = "select *,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + LibCollection + " and college_code in('" + collegeCode + "') ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
            ds = d2.select_method_wo_parameter(selectQuery, "Text");
            ddllibrary.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddllibrary.DataSource = ds;
                ddllibrary.DataTextField = "lib_name";
                ddllibrary.DataValueField = "lib_code";
                ddllibrary.DataBind();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "Library_Master");
        }
    }

    #endregion

    protected void rbBefore_OnCheckedChanged(object sender, EventArgs e)
    {
        LblYr.Visible = false;
        ddlYear.Visible = false;
        LblStatus.Visible = false;
        ddlStatus.Visible = false;
        chkredate.Enabled = true;
    }

    protected void rbAfter_OnCheckedChanged(object sender, EventArgs e)
    {
        LblYr.Visible = true;
        ddlYear.Visible = true;
        LblStatus.Visible = true;
        ddlStatus.Visible = true;
        chkredate.Enabled = false;
    }

    protected void chkredate_CheckedChanged(object sender, EventArgs e)
    {
        if (chkredate.Checked == true)
        {
            txtfromdate.Enabled = true;
            txttodate.Enabled = true;
        }
        if (chkredate.Checked == false)
        {
            txtfromdate.Enabled = false;
            txttodate.Enabled = false;
        }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            string lib_code = Convert.ToString(ddllibrary.SelectedValue);
            string Year = Convert.ToString(ddlYear.SelectedItem.Text);
            string strDate = string.Empty;
            if (rbBefore.Checked == true)
            {
                #region Before Scanning

                string firstdate = Convert.ToString(txtfromdate.Text);
                string seconddate = Convert.ToString(txttodate.Text);
                string dt = string.Empty;
                string dt1 = string.Empty;

                string[] split = firstdate.Split('/');
                dt = split[1] + "/" + split[0] + "/" + split[2];

                split = seconddate.Split('/');
                dt1 = split[1] + "/" + split[0] + "/" + split[2];
                if (chkredate.Checked == true)
                {

                    strDate = "  and scandate between '" + dt + "' and '" + dt1 + "'";
                }
                else
                {
                    strDate = "";
                }

                if (ddltype.SelectedItem.Text == "Books")
                {
                    Sql = "select  distinct ltrim(rtrim(acc_no)) as Acc_No,title,author,price,bookdolist.lib_code from bookdetails,bookdolist where bookdetails.acc_no=bookdolist.acc_no_sys and acc_no_phy<>'' and bookdetails.lib_code=bookdolist.lib_code and bookdolist.lib_code='" + lib_code + "'" + strDate + " and booktype='BOK'";
                }
                if (ddltype.SelectedItem.Text == "Project Books")
                {
                    Sql = "select distinct ltrim(rtrim(probook_accno)) as Acc_No,title,roll_no,name,bookdolist.lib_code from project_book,bookdolist where project_book.probook_accno=bookdolist.acc_no_sys and project_book.lib_code=bookdolist.lib_code and bookdolist.lib_code='" + lib_code + "' " + strDate + " and booktype='PRO'  and acc_no_phy<>''";
                }
                if (ddltype.SelectedItem.Text == "Non Book Materials")
                {
                    Sql = "select distinct ltrim(rtrim(nonbookmat_no)) as Acc_No,title,author,attachment,bookdolist.lib_code from nonbookmat,bookdolist where nonbookmat.nonbookmat_no=bookdolist.acc_no_sys and acc_no_phy<>'' and nonbookmat.lib_code=bookdolist.lib_code and bookdolist.lib_code='" + lib_code + "' " + strDate + " and booktype='NBM'";
                }
                if (ddltype.SelectedItem.Text == "Back Volume")
                {
                    Sql = "select distinct ltrim(rtrim(access_code)) as Acc_No,title,publisher,monthpub,bookdolist.lib_code from back_volume,bookdolist where back_volume.access_code=bookdolist.acc_no_sys and acc_no_phy<>'' and back_volume.lib_code=bookdolist.lib_code and bookdolist.lib_code='" + lib_code + "' " + strDate + " and booktype='BVO'";
                }
                ds = d2.select_method_wo_parameter(Sql, "text");
                int SNo = 0;
                double PriceTot = 0;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ddltype.SelectedItem.Text == "Books")
                    {
                        arrColHdrNames.Add("S.No");
                        dtreport.Columns.Add("S.No");
                        arrColHdrNames.Add("Access No");
                        dtreport.Columns.Add("Access No");
                        arrColHdrNames.Add("Title");
                        dtreport.Columns.Add("Title");
                        arrColHdrNames.Add("Author");
                        dtreport.Columns.Add("Author");
                        arrColHdrNames.Add("Price");
                        dtreport.Columns.Add("Price");
                    }
                    if (ddltype.SelectedItem.Text == "Project Books")
                    {
                        arrColHdrNames.Add("S.No");
                        dtreport.Columns.Add("S.No");
                        arrColHdrNames.Add("Access No");
                        dtreport.Columns.Add("Access No");
                        arrColHdrNames.Add("Title");
                        dtreport.Columns.Add("Title");
                        arrColHdrNames.Add("Roll No");
                        dtreport.Columns.Add("Roll No");
                        arrColHdrNames.Add("Name");
                        dtreport.Columns.Add("Name");
                    }
                    if (ddltype.SelectedItem.Text == "Non Book Materials")
                    {
                        arrColHdrNames.Add("S.No");
                        dtreport.Columns.Add("S.No");
                        arrColHdrNames.Add("Access No");
                        dtreport.Columns.Add("Access No");
                        arrColHdrNames.Add("Title");
                        dtreport.Columns.Add("Title");
                        arrColHdrNames.Add("Author");
                        dtreport.Columns.Add("Author");
                        arrColHdrNames.Add("Attachment");
                        dtreport.Columns.Add("Attachment");
                    }
                    if (ddltype.SelectedItem.Text == "Back Volume")
                    {
                        arrColHdrNames.Add("S.No");
                        dtreport.Columns.Add("S.No");
                        arrColHdrNames.Add("Access No");
                        dtreport.Columns.Add("Access No");
                        arrColHdrNames.Add("Title");
                        dtreport.Columns.Add("Title");
                        arrColHdrNames.Add("Publisher");
                        dtreport.Columns.Add("Publisher");
                        arrColHdrNames.Add("Month.Pub");
                        dtreport.Columns.Add("Month.Pub");
                    }

                    DataRow drHdr1 = dtreport.NewRow();
                    for (int grCol = 0; grCol < dtreport.Columns.Count; grCol++)
                    {
                        drHdr1[grCol] = arrColHdrNames[grCol];
                    }
                    dtreport.Rows.Add(drHdr1);
                    double PriceVal = 0;

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        SNo++;
                        drow = dtreport.NewRow();
                        drow[0] = SNo;
                        if (ddltype.SelectedItem.Text == "Books")
                        {
                            drow[1] = Convert.ToString(ds.Tables[0].Rows[i]["Acc_No"]);
                            drow[2] = Convert.ToString(ds.Tables[0].Rows[i]["Title"]);
                            drow[3] = Convert.ToString(ds.Tables[0].Rows[i]["Author"]);
                            drow[4] = Convert.ToString(ds.Tables[0].Rows[i]["Price"]);
                            string price = Convert.ToString(ds.Tables[0].Rows[i]["Price"]);
                            double.TryParse(price, out PriceVal);
                            PriceTot = PriceTot + PriceVal;
                        }
                        if (ddltype.SelectedItem.Text == "Project Books")
                        {
                            drow[1] = Convert.ToString(ds.Tables[0].Rows[i]["Acc_No"]);
                            drow[2] = Convert.ToString(ds.Tables[0].Rows[i]["Title"]);
                            drow[3] = Convert.ToString(ds.Tables[0].Rows[i]["roll_no"]);
                            drow[4] = Convert.ToString(ds.Tables[0].Rows[i]["name"]);
                        }
                        if (ddltype.SelectedItem.Text == "Non Book Materials")
                        {
                            drow[1] = Convert.ToString(ds.Tables[0].Rows[i]["Acc_No"]);
                            drow[2] = Convert.ToString(ds.Tables[0].Rows[i]["Title"]);
                            drow[3] = Convert.ToString(ds.Tables[0].Rows[i]["Author"]);
                            drow[4] = Convert.ToString(ds.Tables[0].Rows[i]["attachment"]);
                        }
                        if (ddltype.SelectedItem.Text == "Back Volume")
                        {
                            drow[1] = Convert.ToString(ds.Tables[0].Rows[i]["Acc_No"]);
                            drow[2] = Convert.ToString(ds.Tables[0].Rows[i]["Title"]);
                            drow[3] = Convert.ToString(ds.Tables[0].Rows[i]["publisher"]);
                            drow[4] = Convert.ToString(ds.Tables[0].Rows[i]["monthpub"]);
                        }
                        dtreport.Rows.Add(drow);
                    }
                    if (ddltype.SelectedItem.Text == "Books")
                    {
                        drow = dtreport.NewRow();
                        drow[0] = "Total";
                        drow[4] = PriceTot;
                        dtreport.Rows.Add(drow);
                        dicColor.Add(dtreport.Rows.Count - 1, "Total");
                    }
                    divReport.Visible = true;
                    grdReport.DataSource = dtreport;
                    grdReport.DataBind();
                    grdReport.Visible = true;
                    grdReport.Rows[0].BackColor = Color.FromArgb(12, 166, 202);
                    grdReport.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                    grdReport.Rows[0].Font.Bold = true;
                    rptprint1.Visible = true;
                    foreach (KeyValuePair<int, string> dr in dicColor)
                    {
                        int rowcnt = dr.Key;
                        string DicVal = dr.Value.ToString();

                        if (DicVal == "Total")
                        {
                            grdReport.Rows[rowcnt].BackColor = Color.Green;
                            grdReport.Rows[rowcnt].Font.Bold = true;
                        }
                    }
                    divLabVal.Visible = true;
                    LblGrdTot.Text = Convert.ToString(PriceTot);
                    LblTotAvail.Text = "Total " + ddlStatus.SelectedItem.Text + " Books :" + ds.Tables[0].Rows.Count;
                }
                #endregion
            }
            if (rbAfter.Checked == true)
            {
                #region AfterScaning

                if (ddlStatus.SelectedIndex == 0)//Available
                {
                    if (ddltype.SelectedItem.Text == "Books")
                    {
                        Sql = "select  distinct ltrim(rtrim(acc_no)) as acc_no,title,author,price,lib_code from bookdetails where book_status <> 'Lost' and lib_code='" + lib_code + "'";
                    }
                    if (ddltype.SelectedItem.Text == "Project Books")
                    {
                        Sql = "select distinct ltrim(rtrim(probook_accno)) as acc_no,title,roll_no,name,lib_code from project_book where issue_flag <> 'Lost' and lib_code='" + lib_code + "'";
                    }
                    if (ddltype.SelectedItem.Text == "Non Book Materials")
                    {
                        Sql = "select distinct ltrim(rtrim(nonbookmat_no)) as acc_no,title,author,attachment,lib_code from nonbookmat  where  issue_flag <> 'Lost' and lib_code='" + lib_code + "'";
                    }
                    if (ddltype.SelectedItem.Text == "Back Volume")
                    {
                        Sql = "select distinct ltrim(rtrim(access_code)) as acc_no,title,publisher,monthpub,lib_code from back_volume  where issue_flag <> 'Lost' and lib_code='" + lib_code + "'";
                    }
                }
                if (ddlStatus.SelectedIndex == 1)//Lost(All)
                {
                    if (ddltype.SelectedItem.Text == "Books")
                    {
                        if (Year == "All")
                        {
                            Sql = "select  distinct ltrim(rtrim(acc_no)) as acc_no,title,author,price,lib_code from bookdetails where book_status = 'Lost' and lib_code='" + lib_code + "'";
                        }
                        else
                        {
                            Sql = "select distinct ltrim(rtrim(bookstatus.acc_no)) as acc_no,title,author,price,bookdetails.lib_code from bookdetails,bookstatus where  bookdetails.lib_code='" + lib_code + "' and bookdetails.acc_no= bookstatus.acc_no and y_lost = '" + Year + "'and book_type='BOK'and bookstatus.lib_code='" + lib_code + "'";
                        }
                    }
                    if (ddltype.SelectedItem.Text == "Project Books")
                    {
                        if (Year == "All")
                        {
                            Sql = "select distinct ltrim(rtrim(probook_accno)) as acc_no,title,roll_no,name,lib_code from project_book where issue_flag = 'Lost' and lib_code='" + lib_code + "'";
                        }
                        else
                        {
                            Sql = "select distinct ltrim(rtrim(probook_accno)) as acc_no,title,roll_no,name,project_book.lib_code from project_book,bookstatus where project_book.lib_code='" + lib_code + "' and probook_accno =acc_no and y_lost = '" + Year + "'and book_type='PRO'and bookstatus.lib_code='" + lib_code + "'";
                        }
                    }
                    if (ddltype.SelectedItem.Text == "Non Book Materials")
                    {
                        if (Year == "All")
                        {
                            Sql = "select distinct ltrim(rtrim(nonbookmat_no)) as acc_no,title,author,attachment,lib_code from nonbookmat  where  issue_flag = 'Lost' and lib_code='" + lib_code + "'";
                        }
                        else
                        {
                            Sql = "select distinct ltrim(rtrim(nonbookmat_no)) as acc_no,title,author,attachment,nonbookmat.lib_code from nonbookmat,bookstatus where  nonbookmat.lib_code='" + lib_code + "' and nonbookmat_no = bookstatus.acc_no and y_lost = '" + Year + "' and book_type='NBM'and bookstatus.lib_code='" + lib_code + "'";
                        }
                    }
                    if (ddltype.SelectedItem.Text == "Back Volume")
                    {
                        if (Year == "All")
                        {
                            Sql = "select distinct ltrim(rtrim(access_code)) as acc_no,title,publisher,monthpub,lib_code from back_volume  where issue_flag = 'Lost' and lib_code='" + lib_code + "'";
                        }
                        else
                        {
                            Sql = "select distinct ltrim(rtrim(access_code))as acc_no,title,publisher,monthpub,back_volume.lib_code from back_volume,bookstatus  where  back_volume.lib_code='" + lib_code + "' and access_code = acc_no and y_lost = '" + Year + "' and book_type='BVO'and bookstatus.lib_code='" + lib_code + "'";
                        }
                    }
                }
                if (ddlStatus.SelectedIndex == 2)//Lost(Library)
                {
                    if (ddltype.SelectedItem.Text == "Books")
                    {
                        if (Year == "All")
                        {
                            Sql = "select distinct  ltrim(rtrim(acc_no)) as acc_no,title,author,price,lib_code from bookdetails where acc_no not in (select distinct acc_no from fine_details where ltrim(rtrim(description)) in ('Lost Fine','Lost and Overdue Fine') and booktype ='BOK')and  book_status='Lost' and lib_code='" + lib_code + "'";
                        }
                        else
                        {
                            Sql = "select distinct ltrim(rtrim(bookdetails.acc_no)) as acc_no,title,author,price,bookdetails.lib_code from bookdetails,bookstatus where bookdetails.acc_no not in (select distinct fine_details.acc_no from fine_details where ltrim(rtrim(description)) in ('Lost Fine','Lost and Overdue Fine')  and booktype ='BOK') and   bookdetails.lib_code='" + lib_code + "' and bookdetails.acc_no  = bookstatus.acc_no and y_lost = '" + Year + "' and bookstatus.book_type='BOK'and bookstatus.lib_code='" + lib_code + "'";
                        }
                    }
                    if (ddltype.SelectedItem.Text == "Project Books")
                    {
                        if (Year == "All")
                        {
                            Sql = "select distinct ltrim(rtrim(probook_accno)) as acc_no,title,roll_no,name,lib_code from project_book where  probook_accno not in (select distinct acc_no from fine_details where ltrim(rtrim(description)) in('Lost Fine','Lost and Overdue Fine')  and booktype ='PRO')  and issue_flag='Lost' and lib_code='" + lib_code + "'";
                        }
                        else
                        {
                            Sql = "select distinct ltrim(rtrim(probook_accno)) as acc_no,title,roll_no,name,project_book.lib_code from project_book,bookstatus  where  probook_accno not in (select distinct acc_no from fine_details where ltrim(rtrim(description))in ('Lost Fine','Lost and Overdue Fine') and booktype ='PRO') and project_book.lib_code='" + lib_code + "' and probook_accno = acc_no and y_lost = '" + Year + "' and book_type='PRO'and bookstatus.lib_code='" + lib_code + "'";
                        }
                    }
                    if (ddltype.SelectedItem.Text == "Non Book Materials")
                    {
                        if (Year == "All")
                        {
                            Sql = "select distinct ltrim(rtrim(nonbookmat_no)) as acc_no,title,author,attachment,lib_code from nonbookmat  where  nonbookmat_no not in (select distinct acc_no from fine_details where ltrim(rtrim(description)) in ('Lost Fine','Lost and Overdue Fine')  and booktype ='NBM')  and issue_flag='Lost' and lib_code='" + lib_code + "'";
                        }
                        else
                        {
                            Sql = "select distinct ltrim(rtrim(nonbookmat_no)) as acc_no,title,author,attachment,nonbookmat.lib_code from nonbookmat,bookstatus where nonbookmat_no not in (select distinct acc_no from fine_details where ltrim(rtrim(description)) in ('Lost Fine','Lost and Overdue Fine')  and booktype ='NBM')  and nonbookmat.lib_code='" + lib_code + "' and nonbookmat_no =bookstatus.acc_no and y_lost = '" + Year + "' and book_type='NBM'and bookstatus.lib_code='" + lib_code + "'";
                        }
                    }
                    if (ddltype.SelectedItem.Text == "Back Volume")
                    {
                        if (Year == "All")
                        {
                            Sql = "select distinct ltrim(rtrim(access_code)) as acc_no,title,publisher,monthpub,lib_code from back_volume  where  access_code not in (select distinct acc_no from fine_details where ltrim(rtrim(description)) in ('Lost Fine','Lost and Overdue Fine') and booktype ='PER') and issue_flag='Lost' and  lib_code='" + lib_code + "'";
                        }
                        else
                        {
                            Sql = "select distinct ltrim(rtrim(access_code)) as acc_no,title,publisher,monthpub,back_volume.lib_code from back_volume,bookstatus where  access_code not in (select distinct acc_no from fine_details where ltrim(rtrim(description)) in ('Lost Fine','Lost and Overdue Fine') and booktype ='PER')  and back_volume.lib_code='" + lib_code + "' and access_code = acc_no  and y_lost = '" + Year + "' and book_type='BVO'and bookstatus.lib_code='" + lib_code + "'";
                        }
                    }
                }
                if (ddlStatus.SelectedIndex == 3)//Lost(Fine Collected)
                {
                    if (ddltype.SelectedItem.Text == "Books")
                    {
                        if (Year == "All")
                        {
                            Sql = "select distinct ltrim(rtrim(acc_no)) as acc_no,title,author,price,lib_code from bookdetails where acc_no in (select distinct acc_no from fine_details where ltrim(rtrim(description)) in ('Lost Fine','Lost and Overdue Fine')and booktype ='BOK') and  book_status='Lost' and lib_code='" + lib_code + "'";
                        }
                        else
                        {
                            Sql = "select distinct ltrim(rtrim(bookdetails.acc_no)) as acc_no,title,author,price,bookdetails.lib_code from bookdetails,bookstatus where  bookdetails.acc_no in (select distinct acc_no from fine_details where ltrim(rtrim(description)) in ('Lost Fine','Lost and Overdue Fine') and booktype ='BOK') and   bookdetails.lib_code='" + lib_code + "' and bookdetails.acc_no = bookstatus.acc_no and y_lost = '" + Year + "' and book_type='BOK'and bookstatus.lib_code='" + lib_code + "'";
                        }
                    }
                    if (ddltype.SelectedItem.Text == "Project Books")
                    {
                        if (Year == "All")
                        {
                            Sql = "select distinct ltrim(rtrim(probook_accno)) as acc_no,title,roll_no,name,lib_code from project_book where  probook_accno in (select distinct acc_no from fine_details where ltrim(rtrim(description)) in ('Lost Fine','Lost and Overdue Fine') and booktype ='PRO') and issue_flag='Lost'  and lib_code='" + lib_code + "'";
                        }
                        else
                        {
                            Sql = "select distinct ltrim(rtrim(probook_accno)) as acc_no,title,roll_no,name,project_book.lib_code from project_book,bookstatus where  probook_accno in (select distinct acc_no from fine_details where ltrim(rtrim(description)) in ('Lost Fine','Lost and Overdue Fine') and booktype ='PRO') and project_book.lib_code='" + lib_code + "' and probook_accno = acc_no and y_lost = '" + Year + "' and book_type='PRO' and bookstatus.lib_code='" + lib_code + "'";
                        }
                    }
                    if (ddltype.SelectedItem.Text == "Non Book Materials")
                    {
                        if (Year == "All")
                        {
                            Sql = "select distinct ltrim(rtrim(nonbookmat_no)) as acc_no,title,author,attachment,lib_code from nonbookmat  where  nonbookmat_no in (select distinct acc_no from fine_details where ltrim(rtrim(description)) in ('Lost Fine','Lost and Overdue Fine') and booktype ='NBM')  and  issue_flag='Lost' and lib_code='" + lib_code + "'";
                        }
                        else
                        {
                            Sql = "select distinct ltrim(rtrim(nonbookmat_no)) as acc_no,title,author,attachment,nonbookmat.lib_code from nonbookmat,bookstatus  where   nonbookmat_no in (select distinct acc_no from fine_details where ltrim(rtrim(description)) in ('Lost Fine','Lost and Overdue Fine') and booktype ='NBM')  and nonbookmat.lib_code='" + lib_code + "' and nonbookmat_no = bookstatus.acc_no and y_lost = '" + Year + "'and book_type='NBM' and bookstatus.lib_code='" + lib_code + "'";
                        }
                    }
                    if (ddltype.SelectedItem.Text == "Back Volume")
                    {
                        if (Year == "All")
                        {
                            Sql = "select distinct ltrim(rtrim(access_code)) as acc_no,title,publisher,monthpub,lib_code from back_volume  where  access_code in (select distinct acc_no from fine_details where ltrim(rtrim(description)) in ('Lost Fine','Lost and Overdue Fine') and booktype ='PER') and  issue_flag='Lost'  and lib_code='" + lib_code + "'";
                        }
                        else
                        {
                            Sql = "select distinct ltrim(rtrim(access_code)) as acc_no,title,publisher,monthpub,back_volume.lib_code from back_volume,bookstatus where   access_code in (select distinct acc_no from fine_details where ltrim(rtrim(description)) in ('Lost Fine','Lost and Overdue Fine') and booktype ='PER')  and back_volume.lib_code='" + lib_code + "' and access_code = acc_no and y_lost = '" + Year + "'and book_type='BVO'and bookstatus.lib_code='" + lib_code + "'";
                        }
                    }
                }
                if (ddlStatus.SelectedIndex == 4)//Condemn
                {
                    if (ddltype.SelectedItem.Text == "Books")
                    {
                        if (Year == "All")
                        {
                            Sql = "select distinct ltrim(rtrim(acc_no)) as acc_no,title,author,price,lib_code from bookdetails where book_status = 'condemn' and lib_code='" + lib_code + "'";
                        }
                        else
                        {
                            Sql = "select distinct ltrim(rtrim(bookdetails.acc_no)) as acc_no,title,author,price,bookdetails.lib_code from bookdetails,book_condemn where bookdetails.lib_code='" + lib_code + "' and bookdetails.acc_no =book_condemn.acc_no and y_condemn = '" + Year + "'and book_type='BOK'and book_condemn.lib_code='" + lib_code + "'";
                        }
                    }
                    if (ddltype.SelectedItem.Text == "Project Books")
                    {
                        if (Year == "All")
                        {
                            Sql = "select distinct ltrim(rtrim(probook_accno)) as acc_no,title,roll_no,name,lib_code from project_book where issue_flag = 'condemn' and lib_code='" + lib_code + "'";
                        }
                        else
                        {
                            Sql = "select distinct ltrim(rtrim(probook_accno)) as acc_no,title,roll_no,name,project_book.lib_code from project_book,book_condemn where project_book.lib_code='" + lib_code + "' and issue_flag='condemn'and probook_accno = acc_no and y_condemn = '" + Year + "'and book_type='PRO'and book_condemn.lib_code='" + lib_code + "'";
                        }
                    }
                    if (ddltype.SelectedItem.Text == "Non Book Materials")
                    {
                        if (Year == "All")
                        {
                            Sql = "select distinct  ltrim(rtrim(nonbookmat_no)) as acc_no,title,author,attachment,lib_code from nonbookmat  where issue_flag = 'condemn' and lib_code='" + lib_code + "'";
                        }
                        else
                        {
                            Sql = "select distinct ltrim(rtrim(nonbookmat_no)) as acc_no,title,author,attachment,nonbookmat.lib_code from nonbookmat,book_condemn  where  nonbookmat.lib_code='" + lib_code + "' and nonbookmat_no =book_condemn.acc_no and y_condemn = '" + Year + "' and book_type='NBM'and book_condemn.lib_code='" + lib_code + "'";
                        }
                    }
                    if (ddltype.SelectedItem.Text == "Back Volume")
                    {
                        if (Year == "All")
                        {
                            Sql = "select distinct ltrim(rtrim(access_code)) as acc_no,title,publisher,monthpub,lib_code from back_volume  where  lib_code='" + lib_code + "'";
                        }
                        else
                        {
                            Sql = "select distinct ltrim(rtrim(access_code)) as acc_no,title,publisher,monthpub,back_volume.lib_code from back_volume,book_condemn where issue_flag = 'condemn' and back_volume.lib_code='" + lib_code + "' and access_code = acc_no and y_condemn = '" + Year + "' and book_type='BVO'and book_condemn.lib_code='" + lib_code + "'";
                        }
                    }
                }
                ds = d2.select_method_wo_parameter(Sql, "text");
                int SNo = 0;
                double PriceTot = 0;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ddltype.SelectedItem.Text == "Books")
                    {
                        arrColHdrNames.Add("S.No");
                        dtreport.Columns.Add("S.No");
                        arrColHdrNames.Add("Access No");
                        dtreport.Columns.Add("Access No");
                        arrColHdrNames.Add("Title");
                        dtreport.Columns.Add("Title");
                        arrColHdrNames.Add("Author");
                        dtreport.Columns.Add("Author");
                        arrColHdrNames.Add("Price");
                        dtreport.Columns.Add("Price");
                    }
                    if (ddltype.SelectedItem.Text == "Project Books")
                    {
                        arrColHdrNames.Add("S.No");
                        dtreport.Columns.Add("S.No");
                        arrColHdrNames.Add("Access No");
                        dtreport.Columns.Add("Access No");
                        arrColHdrNames.Add("Title");
                        dtreport.Columns.Add("Title");
                        arrColHdrNames.Add("Roll No");
                        dtreport.Columns.Add("Roll No");
                        arrColHdrNames.Add("Name");
                        dtreport.Columns.Add("Name");
                    }
                    if (ddltype.SelectedItem.Text == "Non Book Materials")
                    {
                        arrColHdrNames.Add("S.No");
                        dtreport.Columns.Add("S.No");
                        arrColHdrNames.Add("Access No");
                        dtreport.Columns.Add("Access No");
                        arrColHdrNames.Add("Title");
                        dtreport.Columns.Add("Title");
                        arrColHdrNames.Add("Author");
                        dtreport.Columns.Add("Author");
                        arrColHdrNames.Add("Attachment");
                        dtreport.Columns.Add("Attachment");
                    }
                    if (ddltype.SelectedItem.Text == "Back Volume")
                    {
                        arrColHdrNames.Add("S.No");
                        dtreport.Columns.Add("S.No");
                        arrColHdrNames.Add("Access No");
                        dtreport.Columns.Add("Access No");
                        arrColHdrNames.Add("Title");
                        dtreport.Columns.Add("Title");
                        arrColHdrNames.Add("Publisher");
                        dtreport.Columns.Add("Publisher");
                        arrColHdrNames.Add("Month.Pub");
                        dtreport.Columns.Add("Month.Pub");
                    }

                    DataRow drHdr1 = dtreport.NewRow();
                    for (int grCol = 0; grCol < dtreport.Columns.Count; grCol++)
                    {
                        drHdr1[grCol] = arrColHdrNames[grCol];
                    }
                    dtreport.Rows.Add(drHdr1);
                    double PriceVal = 0;

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        SNo++;
                        drow = dtreport.NewRow();
                        drow[0] = SNo;
                        if (ddltype.SelectedItem.Text == "Books")
                        {
                            drow[1] = Convert.ToString(ds.Tables[0].Rows[i]["Acc_No"]);
                            drow[2] = Convert.ToString(ds.Tables[0].Rows[i]["Title"]);
                            drow[3] = Convert.ToString(ds.Tables[0].Rows[i]["Author"]);
                            drow[4] = Convert.ToString(ds.Tables[0].Rows[i]["Price"]);
                            string price = Convert.ToString(ds.Tables[0].Rows[i]["Price"]);
                            double.TryParse(price, out PriceVal);
                            PriceTot = PriceTot + PriceVal;
                        }
                        if (ddltype.SelectedItem.Text == "Project Books")
                        {
                            drow[1] = Convert.ToString(ds.Tables[0].Rows[i]["Acc_No"]);
                            drow[2] = Convert.ToString(ds.Tables[0].Rows[i]["Title"]);
                            drow[3] = Convert.ToString(ds.Tables[0].Rows[i]["roll_no"]);
                            drow[4] = Convert.ToString(ds.Tables[0].Rows[i]["name"]);
                        }
                        if (ddltype.SelectedItem.Text == "Non Book Materials")
                        {
                            drow[1] = Convert.ToString(ds.Tables[0].Rows[i]["Acc_No"]);
                            drow[2] = Convert.ToString(ds.Tables[0].Rows[i]["Title"]);
                            drow[3] = Convert.ToString(ds.Tables[0].Rows[i]["Author"]);
                            drow[4] = Convert.ToString(ds.Tables[0].Rows[i]["attachment"]);
                        }
                        if (ddltype.SelectedItem.Text == "Back Volume")
                        {
                            drow[1] = Convert.ToString(ds.Tables[0].Rows[i]["Acc_No"]);
                            drow[2] = Convert.ToString(ds.Tables[0].Rows[i]["Title"]);
                            drow[3] = Convert.ToString(ds.Tables[0].Rows[i]["publisher"]);
                            drow[4] = Convert.ToString(ds.Tables[0].Rows[i]["monthpub"]);
                        }
                        dtreport.Rows.Add(drow);
                    }
                    if (ddltype.SelectedItem.Text == "Books")
                    {
                        drow = dtreport.NewRow();
                        drow[0] = "Total";
                        drow[4] = PriceTot;
                        dtreport.Rows.Add(drow);
                        dicColor.Add(dtreport.Rows.Count - 1, "Total");
                    }
                    divReport.Visible = true;
                    grdReport.DataSource = dtreport;
                    grdReport.DataBind();
                    grdReport.Visible = true;
                    grdReport.Rows[0].BackColor = Color.FromArgb(12, 166, 202);
                    grdReport.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                    grdReport.Rows[0].Font.Bold = true;
                    rptprint1.Visible = true;
                    foreach (KeyValuePair<int, string> dr in dicColor)
                    {
                        int rowcnt = dr.Key;
                        string DicVal = dr.Value.ToString();

                        if (DicVal == "Total")
                        {
                            grdReport.Rows[rowcnt].BackColor = Color.Green;
                            grdReport.Rows[rowcnt].Font.Bold = true;
                        }
                    }
                    divLabVal.Visible = true;
                    LblGrdTot.Text = Convert.ToString(PriceTot);
                    LblTotAvail.Text = "Total " + ddlStatus.SelectedItem.Text + " Books :" + ds.Tables[0].Rows.Count;
                }

                #endregion
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void grdReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
        e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
    }

    #region Print

    protected void btnprintmaster1_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "StockAnalyserReport";
            string pagename = "StockAnalyserReport.aspx";
            string ss = Session["usercode"].ToString();
            Printcontrolhed2.loadspreaddetails(grdReport, pagename, degreedetails, 0, ss);
            Printcontrolhed2.Visible = true;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "StockAnalyserReport"); }


    }

    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname1.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(grdReport, reportname);
                lblvalidation2.Visible = false;
            }
            else
            {
                lblvalidation2.Text = "Please Enter Your Report Name";
                lblvalidation2.Visible = true;
                txtexcelname1.Focus();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "StockAnalyserReport"); }


    }
    #endregion
}