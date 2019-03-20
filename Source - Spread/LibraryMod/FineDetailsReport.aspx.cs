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
using InsproDataAccess;
using System.Data.SqlClient;
using System.Configuration;

public partial class LibraryMod_FineDetailsReport : System.Web.UI.Page
{

    # region fielddeclaration
    DAccess2 da = new DAccess2();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    DataTable dtCommon = new DataTable();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    ReuasableMethods rs = new ReuasableMethods();
    Hashtable columnhash = new Hashtable();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    string collegeCode = string.Empty;
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string qryUserOrGroupCode = string.Empty;
    string library = string.Empty;
    string inward = string.Empty;
    string dept = string.Empty;
    string Sql = string.Empty;
    static string booktypevar = string.Empty;
    DataSet fine = new DataSet();
    static string fieldnameacc = string.Empty;
    static string strCond = string.Empty;
    static string colname = "";
    DataTable finecollection = new DataTable();
    string lib = string.Empty;
    DataTable dtfine1 = new DataTable();
    DataRow drfinere;

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
                report();
                Bindcollege();
                getLibPrivil();
                select();
                Binddept();
                status();
                sem();
                book();
                BindBatchYear();
                popcumlative.Visible = false;
                //Fpload1.Visible = false;
                //Fpload2.Visible = false;
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate2.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate2.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
        }
        catch
        { }
    }

    public void report()
    {
        try
        {
            rblreport.Items.Add("Detailed");
            rblreport.Items.Add("Cummlative");
            rblreport.Items.FindByText("Detailed").Selected = true;
            Rblreporttype.Items.Add("Date Wise");
            Rblreporttype.Items.Add("Month Wise");
            Rblreporttype.Items.Add("Year Wise");
            Rblreporttype.Items.FindByText("Date Wise").Selected = true;
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "FineReport"); }
    }

    protected void BindBatchYear()
    {
        string qry = " select distinct Batch_Year from Registration order by batch_year desc";
        DataTable dtbatchyr = dirAcc.selectDataTable(qry);
        ddlBatch.Items.Clear();
        if (dtbatchyr.Rows.Count > 0)
        {
            ddlBatch.DataSource = dtbatchyr;
            ddlBatch.DataTextField = "Batch_Year";
            ddlBatch.DataValueField = "Batch_Year";
            ddlBatch.DataBind();
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
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "FineReport"); }
    }

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
            //libcodecollection = Left(libcodecollection, Len(libcodecollection) - 1);
            libcodecollection = "WHERE lib_code IN ('" + libcodecollection + "')";
        aa:
            LibCollection = libcodecollection;
            BindLibrary(LibCollection);
            BindLibrary1(LibCollection);
            //LibNameDefault = LibCollection;
        }
        catch (Exception ex)
        {
        }
    }

    public void BindLibrary(string Libcode)
    {
        try
        {
            ddlLibrary.Items.Clear();
            ds.Clear();
            string strquery = "SELECT Lib_Code,Lib_Name,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) FROM Library " + Libcode + " and College_Code ='" + userCollegeCode + "' ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
            ds.Clear();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlLibrary.DataSource = ds;
                ddlLibrary.DataTextField = "Lib_Name";
                ddlLibrary.DataValueField = "Lib_Code";
                ddlLibrary.DataBind();
                ddlLibrary.Items.Insert(0, "All");
            }
        }

        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "FineReport"); }
    }

    public void BindLibrary1(string LibCodeCol)
    {
        try
        {
            ddllibrary1.Items.Clear();
            ds.Clear();
            string strquery = "SELECT Lib_Code,Lib_Name,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) FROM Library " + LibCodeCol + " and College_Code ='" + userCollegeCode + "' ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
            ds.Clear();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddllibrary1.DataSource = ds;
                ddllibrary1.DataTextField = "Lib_Name";
                ddllibrary1.DataValueField = "Lib_Code";
                ddllibrary1.DataBind();
                ddllibrary1.Items.Insert(0, "All");
            }
        }

        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "FineReport"); }
    }

    #endregion

    #region dept

    public void Binddept()
    {
        try
        {
            ds.Clear();
            if (ddlselect.SelectedItem.Text == "Student")
            {
                string collcode = Convert.ToString(ddlCollege.SelectedValue);
                string sql = "select distinct degree.degree_code,department.dept_name,department.dept_code,degree.dept_priority,len(isnull(degree.dept_priority,1000)) from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code  and degree.college_code='" + collcode + "'  and deptprivilages.Degree_code=degree.Degree_code and user_code='" + userCode + "' order by len(isnull(degree.dept_priority,1000)),degree.dept_priority asc";
                ds.Clear();
                ds = d2.select_method_wo_parameter(sql, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddldept.DataSource = ds;
                    ddldept.DataTextField = "dept_name";
                    ddldept.DataValueField = "degree_code";
                    ddldept.DataBind();
                }
                ddldept.Items.Insert(0, "All");
            }
            else
            {
                string strqur = "SELECT DISTINCT (ISNULL(Dept_Code,'')) Dept_Code FROM BookDetails WHERE  Dept_Code <> '' ";
                strqur = strqur + " UNION ";
                strqur = strqur + "SELECT DISTINCT (ISNULL(Dept_Name,'')) Dept_Code FROM Journal WHERE  Dept_Name <> '' ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(strqur, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddldept.DataSource = ds;
                    ddldept.DataTextField = "Dept_Code";
                    ddldept.DataValueField = "Dept_Code";
                    ddldept.DataBind();
                }
                ddldept.Items.Insert(0, "All");
            }

        }

        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "FineReport"); }
    }

    #endregion

    #region select
    public void select()
    {
        try
        {
            ddlselect.Items.Add("All");
            ddlselect.Items.Add("Staff");
            ddlselect.Items.Add("Student");
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "FineReport"); }
    }
    #endregion

    #region status
    public void status()
    {
        try
        {
            ddlstatus.Items.Add("All");
            ddlstatus.Items.Add("Cancel");
            ddlstatus.Items.Add("Edited");
            ddlstatus.Items.Add("Paid");
            ddlstatus.Items.Add("Unpaid");
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "FineReport"); }
    }
    #endregion

    #region book
    public void book()
    {
        try
        {
            ddlbook.Items.Add("All");
            ddlbook.Items.Add("Book");
            ddlbook.Items.Add("Reference Book");
            ddlbook.Items.Add("Project Book");
            ddlbook.Items.Add("Question bank");
            ddlbook.Items.Add("Non Book Material");
            ddlbook.Items.Add("Periodical");
            ddlbook.Items.Add("Back Volume");
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "FineReport"); }
    }
    #endregion

    #region sem

    public void sem()
    {
        try
        {
            ds.Clear();
            string strsem = "SELECT * FROM TEXTVALTABLE WHERE textcriteria='FEECA' and college_code='" + userCollegeCode + "' and textval <>'Hostel'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(strsem, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsem.DataSource = ds;
                ddlsem.DataTextField = "textval";
                ddlsem.DataValueField = "textval";
                ddlsem.DataBind();
                ddlsem.Items.Insert(0, "All");
            }

        }

        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "FineReport"); }
    }

    #endregion

    public void Bindcollege1()
    {
        try
        {
            ddlcollege1.Items.Clear();
            dtCommon.Clear();
            ddlcollege1.Enabled = false;
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
                ddlcollege1.DataSource = dtCommon;
                ddlcollege1.DataTextField = "collname";
                ddlcollege1.DataValueField = "college_code";
                ddlcollege1.DataBind();
                ddlcollege1.SelectedIndex = 0;
                ddlcollege1.Enabled = true;
            }
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "FineReport"); }
    }

    #region sem1

    public void sem1()
    {
        try
        {

            ds.Clear();

            string strsem = "SELECT * FROM TEXTVALTABLE WHERE textcriteria='FEECA' and college_code='" + userCollegeCode + "' and textval <>'Hostel'";

            ds.Clear();
            ds = d2.select_method_wo_parameter(strsem, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsem1.DataSource = ds;
                ddlsem1.DataTextField = "textval";
                ddlsem1.DataValueField = "textval";
                ddlsem1.DataBind();
                ddlsem1.Items.Insert(0, "All");
            }

        }

        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "FineReport"); }
    }

    #endregion

    protected void rblreport_Selected(object sender, EventArgs e)
    {
        try
        {
            if (rblreport.SelectedIndex == 0)
            {
                divDetail.Visible = true;
                divCum.Visible = false;
                popdetails.Visible = true;
                popcumlative.Visible = false;
                grid_Details.Visible = false;
            }
            if (rblreport.SelectedIndex == 1)
            {
                divDetail.Visible = false;
                divCum.Visible = true;
                popdetails.Visible = false;
                popcumlative.Visible = true;
                Bindcollege1();
                getLibPrivil();
                sem1();
                grid_Details.Visible = false;
                colour.Visible = false;
            }

        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "FineReport"); }

    }

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {


        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "FineReport"); }


    }

    protected void ddllibrary_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {


        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "FineReport"); }

    }

    protected void ddldept_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {


        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "FineReport"); }
    }

    protected void cbdate_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbdate.Checked)
            {
                txt_fromdate.Enabled = true;
                txt_todate.Enabled = true;

            }
            else
            {
                txt_fromdate.Enabled = false;
                txt_todate.Enabled = false;
            }

        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "FineReport"); }
    }

    protected void ddlselect_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlselect.SelectedIndex == 0)
            {
                ddlsem.Enabled = false;
                tdbatch.Visible = false;
                Binddept();
            }
            if (ddlselect.SelectedIndex == 1)
            {
                tdbatch.Visible = false;
                ddlsem.Enabled = false;
                Binddept();
            }
            if (ddlselect.SelectedIndex == 2)
            {
                ddlsem.Enabled = true;
                tdbatch.Visible = true;
                Binddept();
            }

        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "FineReport"); }
    }

    protected void ddlstatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {


        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "FineReport"); }
    }

    protected void ddlbook_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string bookType = Convert.ToString(ddlbook.SelectedItem.Text);
            if (bookType == "Book")
            {
                booktypevar = "BOK";
                colname = "bookdetails";
                fieldnameacc = "acc_no";
            }
            else if (bookType == "Reference Book")
            {
                booktypevar = "REF";
                colname = "bookdetails";
                fieldnameacc = "acc_no";
            }
            else if (bookType == "Project Book")
            {
                booktypevar = "PRO";
                colname = "project_book";
                fieldnameacc = "probook_accno";
            }
            else if (bookType == "Question Bank")
            {
                booktypevar = "QBA";
                colname = "university_question";
                fieldnameacc = "access_code";
            }
            else if (bookType == "Non Book Material")
            {
                booktypevar = "NBM";
                colname = "nonbookmat";
                fieldnameacc = "nonbookmat_no";
            }
            else if (bookType == "Periodical")
            {
                booktypevar = "PER";
                colname = "journal";
                fieldnameacc = "access_code";
            }
            else if (bookType == "Periodical")
            {
                booktypevar = "BVO";
                colname = "back_volume";
                fieldnameacc = "access_code";
            }
            else if (bookType == "Back Volume")
            {
                booktypevar = "BVO";
                colname = "back_volume";
                fieldnameacc = "access_code";
            }
            else if (bookType == "Back Volume")
            {
                booktypevar = "%";
                colname = "";
            }



        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "FineReport"); }
    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {


        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "FineReport"); }
    }

    protected void ddlcollege1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {


        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "FineReport"); }


    }

    protected void ddllibrary1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {


        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "FineReport"); }

    }

    protected void ddlsem1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {


        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "FineReport"); }
    }

    protected void Rblreporttype_Rblreporttype(object sender, EventArgs e)
    {
        try
        {
            if (Rblreporttype.SelectedIndex == 0)
            {
                GrdCumulative.Visible = false;
                Div1.Visible = false;
            }
            if (Rblreporttype.SelectedIndex == 1)
            {
                GrdCumulative.Visible = false;
                Div1.Visible = false;
            }
            if (Rblreporttype.SelectedIndex == 1)
            {
                GrdCumulative.Visible = false;
                Div1.Visible = false;
            }

        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "FineReport"); }

    }

    #region detailed

    protected void grid_Details_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }

    //protected void grdManualExit_OnPageIndexChanged(object sender, GridViewPageEventArgs e)
    //{
    //    grid_Details.PageIndex = e.NewPageIndex;
    //    btngoClick(sender, e);
    //}

    private void cmd_rpt_spd_Click()
    {
        try
        {

            string Var_Status = "";
            string Var_FineAmt = "";
            int det = 0;
            double Var_PaidAmt = 0.0;
            double Var_CancelAmt = 0.0;
            double Var_UnPaidAmt = 0.0;
            Var_PaidAmt = 0;
            Var_UnPaidAmt = 0;
            Var_CancelAmt = 0;
            int i = 0;
            for (i = 0; i < grid_Details.Rows.Count; i++)
            {
                if (Convert.ToString(ddlselect.SelectedItem) == "All")
                {
                    Var_Status = grid_Details.Rows[i].Cells[14].Text;
                    Var_FineAmt = grid_Details.Rows[i].Cells[13].Text;

                }
                else if (Convert.ToString(ddlselect.SelectedItem) == "Student")
                {
                    Var_Status = grid_Details.Rows[i].Cells[14].Text;
                    Var_FineAmt = grid_Details.Rows[i].Cells[13].Text;

                }
                else if (Convert.ToString(ddlselect.SelectedItem) == "Staff")
                {
                    Var_Status = grid_Details.Rows[i].Cells[14].Text;
                    Var_FineAmt = grid_Details.Rows[i].Cells[13].Text;

                }
                if (Var_Status == "Paid")
                {
                    Var_PaidAmt = Var_PaidAmt + Convert.ToDouble(Var_FineAmt);
                }
                else if (Var_Status == "UnPaid/Returned")
                {
                    Var_UnPaidAmt = Var_UnPaidAmt + Convert.ToDouble(Var_FineAmt);
                }
                else if (Var_Status == "Cancel")
                {
                    Var_CancelAmt = Var_CancelAmt + Convert.ToDouble(Var_FineAmt);
                }

                if (Var_Status == "Paid")
                {
                    grid_Details.Rows[i].BackColor = System.Drawing.Color.GreenYellow;
                }
                else if (Var_Status == "UnPaid/Returned")
                {
                    grid_Details.Rows[i].BackColor = System.Drawing.Color.SkyBlue;
                }
                else if (Var_Status == "Cancel")
                {
                    grid_Details.Rows[i].BackColor = System.Drawing.Color.Orange;
                }
            }
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "FineReport"); }


    }

    protected void btngoClick(object sender, EventArgs e)
    {
        try
        {
            string status = string.Empty;
            string selectfor = string.Empty;
            string book = string.Empty;
            string Sql = string.Empty;
            string fineroll = string.Empty;
            string sem = string.Empty;
            string infromdate = string.Empty;
            string intodate = string.Empty;
            string qrylibraryFilter = string.Empty;
            string strDate = string.Empty;
            string strStatus = string.Empty;
            string qrydeptFilter = string.Empty;
            string qrysemFilter = string.Empty;
            string access = string.Empty;
            string title = string.Empty;
            string recipt = string.Empty;
            string roll = string.Empty;
            string category = string.Empty;
            string student = string.Empty;
            string issue = string.Empty;
            string due = string.Empty;
            string Return = string.Empty;
            string paiddate = "";
            string booktype = string.Empty;
            string description = string.Empty;
            string overduedays = string.Empty;
            string fineamount = string.Empty;

            string reason = string.Empty;
            string Actual = string.Empty;
            string staff = string.Empty;
            string libraryname = string.Empty;
            if (ddlCollege.Items.Count > 0)
                collegeCode = Convert.ToString(ddlCollege.SelectedValue);
            if (ddlLibrary.Items.Count > 0)
                lib = Convert.ToString(ddlLibrary.SelectedValue);
            if (ddldept.Items.Count > 0)
                dept = Convert.ToString(ddldept.SelectedValue);
            if (ddlselect.Items.Count > 0)
                selectfor = Convert.ToString(ddlselect.SelectedValue);
            if (ddlstatus.Items.Count > 0)
                status = Convert.ToString(ddlstatus.SelectedValue);
            if (ddlbook.Items.Count > 0)
                book = Convert.ToString(ddlbook.SelectedValue);
            if (ddlsem.Items.Count > 0)
                sem = Convert.ToString(ddlsem.SelectedValue);
            fineroll = txtroll.Text;
            string typ = string.Empty;
            string str1 = string.Empty;

            if (ddlsem.Items.Count > 0)
            {
                for (int i = 0; i < ddlsem.Items.Count - 1; i++)
                {
                    if (Convert.ToString(ddlsem.SelectedItem) == "All")
                    {
                        if (typ == "")
                        {
                            typ = "All";
                        }
                    }
                    else
                    {
                        if (typ == "")
                        {
                            typ = "" + ddlsem.Items[i + 1].Value + "";
                            string[] spl = typ.Split();
                            typ = spl[0];
                        }
                        else
                        {
                            string ty = ddlsem.Items[i + 1].Value;
                            string[] spl = ty.Split();
                            typ = typ + "'" + "," + "'" + spl[0] + "";
                        }
                    }
                }
            }
            string typ1 = string.Empty;
            if (ddlLibrary.Items.Count > 0)
            {
                for (int i = 0; i < ddlLibrary.Items.Count - 1; i++)
                {
                    if (Convert.ToString(ddlLibrary.SelectedItem) == "All")
                    {
                        if (typ1 == "")
                        {
                            typ1 = "" + ddlLibrary.Items[i + 1].Value + "";
                        }
                        else
                        {
                            typ1 = typ1 + "'" + "," + "'" + ddlLibrary.Items[i + 1].Value + "";
                        }
                    }
                    else
                        typ1 = ddlLibrary.SelectedValue;
                }
            }
            if (Convert.ToString(ddlbook.SelectedItem) == "Book")
            {
                str1 = "  and fine_details.booktype='BOK'";
            }
            else if (Convert.ToString(ddlbook.SelectedItem) == "Reference Book")
            {
                str1 = " and fine_details.booktype='REF'";
            }
            else if (Convert.ToString(ddlbook.SelectedItem) == "Project Book")
            {
                str1 = " and fine_details.booktype='PRO'";
            }
            else if (Convert.ToString(ddlbook.SelectedItem) == "Project Book")
            {
                str1 = " and fine_details.booktype='PER'";
            }
            else if (Convert.ToString(ddlbook.SelectedItem) == "Periodical")
            {
                str1 = " and fine_details.booktype='PER'";
            }
            else if (Convert.ToString(ddlbook.SelectedItem) == "Back Volume")
            {
                str1 = " and fine_details.booktype='BVO'";
            }
            else if (Convert.ToString(ddlbook.SelectedItem) == "Question Bank")
            {
                str1 = " and fine_details.booktype='QBA'";
            }
            else if (Convert.ToString(ddlbook.SelectedItem) == "Non Book Material")
            {
                str1 = " and fine_details.booktype='NBM'";
            }
            else
            {
                str1 = "";
            }
            if (status == "Paid")
            {
                strStatus = " and fine_details.fineamt - fine_details.paidamt  = 0 and isnull(fine_details.Is_Cancel,0) = 0";
            }
            else if (status == "Unpaid")
            {
                strStatus = " and fine_details.fineamt - fine_details.paidamt  <> 0 and isnull(fine_details.Is_Cancel,0) = 0";
            }
            else if (status == "Cancel")
            {
                strStatus = " and fine_details.Is_Cancel = 1";
            }
            else if (status == "Edited")
            {
                strStatus = " and fine_details.fineamt <> fine_details.actfineamt and isnull(fine_details.Is_Cancel,0) = 0";
            }
            else if (status == "All")
            {
                strStatus = " ";
            }
            else if (status == "")
            {
                strStatus = " ";
            }
            if (fineroll != "")
            {
                strStatus = strStatus + " and Fine_Details.roll_no like '" + fineroll + "%'";
            }
            else
            {
                strStatus = strStatus + " and Fine_Details.roll_no like '%'";
            }
            if (cbdate.Checked)
            {
                string fromDate = txt_fromdate.Text;
                string toDate = txt_todate.Text;
                string[] fromdate = fromDate.Split('/');
                string[] todate = toDate.Split('/');
                if (fromdate.Length == 3)
                    infromdate = fromdate[2].ToString() + "/" + fromdate[1].ToString() + "/" + fromdate[0].ToString();
                if (todate.Length == 3)
                    intodate = todate[2].ToString() + "/" + todate[1].ToString() + "/" + todate[0].ToString();
                strDate = " and cal_date between '" + infromdate + "' and '" + intodate + "'";
            }
            Sql = "Select distinct fine_details.receipt_no as 'Receipt No',fine_details.roll_no as 'Roll No',stud_name as 'Name',fine_details.acc_no as 'Access No', fine_details.title as 'Title',convert(varchar,fine_details.due_date,103) as 'Due Date',library.lib_name as Library,convert(varchar,iss_date,103) as 'Issue Date',convert(varchar,cal_date,103) as 'Paid Date',fine_details.booktype as 'Book Type',fine_details.description as 'Description',case when datediff(dd,fine_details.due_date,cal_date)< 0 then '' else datediff(dd,fine_details.due_date,cal_date) end as 'No of OverDue Days',convert(varchar,fine_details.fineamt) as 'Fine Amount',case when ((fineamt - paidamt) =0 )then 'Paid' else 'UnPaid/Returned' end as 'Status', library.lib_name as 'Library Name'  ";

            if (Convert.ToString(ddlselect.SelectedItem) == "Student")
            {
                Sql = "   select distinct fine_details.receipt_no as 'Receipt No',fine_details.roll_no as 'Roll No',registration.stud_name as 'Student Name', fine_details.acc_no as 'Access No', isnull(fine_details.title,'') as 'Title', convert(varchar,iss_date,103) as 'Issue Date',convert(varchar,due_date,103) as 'Due Date',convert(varchar,cal_date,103) as 'Paid Date',library.lib_name as Library,fine_details.booktype as 'Book Type',fine_details.description as 'Description',case when datediff(dd,fine_details.due_date,cal_date)< 0  then '' else datediff(dd,fine_details.due_date,cal_date) end as 'No of OverDue Days' ,convert(varchar,fine_details.fineamt) as 'Fine Amount',case when Is_Cancel =1 then 'Cancel' When ((fineamt - paidamt) =0 )then 'Paid' else 'UnPaid/Returned' end as 'Status',CASE WHEN Is_Cancel = 1 THEN fine_Details.Reason ELSE '' END Reason,isnull(paidamt,0) as 'Actual Fine'  From fine_details, library, registration, Degree, department,TEXTVALTABLE where   library.lib_code=fine_details.lib_code " + strStatus + " and (registration.roll_no=Fine_Details.roll_no or registration.lib_id = fine_details.roll_no)  " + strDate + " and  registration.degree_code=degree.degree_code  and  degree.dept_code=department.dept_code and is_staff=0 and fine_details.lib_code in( '" + typ1 + "')";

                if (typ != "All")
                {
                    Sql = Sql + "AND Registration.Current_Semester in('" + typ + "')";
                }
                if (Convert.ToString(ddlBatch.SelectedItem) != "")
                {
                    Sql = Sql + "AND Registration.Batch_Year in('" + ddlBatch.SelectedValue + "')";
                }
                if (Convert.ToString(ddldept.SelectedItem) != "All")
                {
                    Sql = Sql + "AND registration.degree_code in('" + ddldept.SelectedValue + "')";
                }
              
            }

            else if (Convert.ToString(ddlselect.SelectedItem) == "Staff")
            {
                Sql = "select distinct fine_details.receipt_no as 'Receipt No',fine_details.roll_no as 'Roll No', staffmaster.staff_name as 'Staff Name',fine_details.acc_no as 'Access No', isnull(fine_details.title,'') as 'Title',library.lib_name as Library,convert(varchar,iss_date,103) as 'Issue Date',convert(varchar,due_date,103) as 'Due Date', convert(varchar,cal_date,103) as 'Paid Date',fine_details.booktype  as 'Book Type',fine_details.description as 'Description',case when datediff(dd,fine_details.due_date,cal_date)< 0  then ''  else datediff(dd,fine_details.due_date,cal_date) end as 'No of OverDue Days' ,convert(varchar,fine_details.fineamt) as 'Fine Amount',case when Is_Cancel =1 then 'Cancel' When ((fineamt - paidamt) =0 )then  'Paid' else 'UnPaid/Returned' end as 'Status',CASE WHEN Is_Cancel = 1 THEN fine_Details.Reason ELSE '' END Reason,isnull(paidamt,0) as 'Actual Fine' From fine_details, library,staffmaster where library.lib_code=fine_details.lib_code and  (staffmaster.staff_code=fine_details.roll_no or staffmaster.lib_id = fine_details.roll_no) " + strStatus + strDate + "  and fine_details.is_staff=1  and fine_details.lib_code in( '" + typ1 + "')";
                if (typ != "All")
                {
                    Sql = Sql + "AND Semester in('" + typ + "')";
                }
            }
            else if (Convert.ToString(ddlselect.SelectedItem) == "All")
            {
                Sql = " select distinct fine_details.receipt_no as 'Receipt No',fine_details.roll_no as 'Roll No',case when fine_details.is_staff=0 then 'Student' when fine_details.is_staff=1 then 'Staff' end as 'Category',registration.stud_name as Name, fine_details.acc_no as 'Access No', isnull(fine_details.title,'') as 'Title', convert(varchar,iss_date,103) as 'Issue Date',convert(varchar,due_date,103) as 'Due Date',convert(varchar,cal_date,103) as 'Paid Date',library.lib_name as Library,fine_details.booktype as 'Book Type',fine_details.description as 'Description',case when datediff(dd,fine_details.due_date,cal_date)< 0  then '' else datediff(dd,fine_details.due_date,cal_date) end as 'No of OverDue Days' ,convert(varchar,fine_details.fineamt) as 'Fine Amount',case when Is_Cancel =1 then 'Cancel' When ((fineamt - paidamt) =0 )then 'Paid' else 'UnPaid/Returned' end as 'Status',CASE WHEN Is_Cancel = 1 THEN fine_Details.Reason ELSE '' END Reason,isnull(paidamt,0) as 'Actual Fine'  From fine_details, library, registration, Degree, department,TEXTVALTABLE where   library.lib_code=fine_details.lib_code " + strStatus + " and (registration.roll_no=Fine_Details.roll_no or registration.lib_id = fine_details.roll_no)  " + strDate + " and  registration.degree_code=degree.degree_code  and  degree.dept_code=department.dept_code and is_staff=0 and fine_details.lib_code in( '" + typ1 + "')";

                Sql = Sql + "Union All select distinct fine_details.receipt_no as 'Receipt No',fine_details.roll_no as 'Roll No',case when fine_details.is_staff=0 then 'Student' when fine_details.is_staff=1 then 'Staff' end as 'Category', staffmaster.staff_name as Name,fine_details.acc_no as 'Access No', isnull(fine_details.title,'') as 'Title',library.lib_name as Library,  convert(varchar,iss_date,103) as 'Issue Date',convert(varchar,due_date,103) as 'Due Date', convert(varchar,cal_date,103) as'Paid Date', fine_details.booktype  as 'Book Type',fine_details.description as 'Description',case when datediff(dd,fine_details.due_date,cal_date)< 0  then '' else datediff(dd,fine_details.due_date,cal_date) end as 'No of OverDue Days',convert(varchar,fine_details.fineamt)  as 'Fine Amount',case when Is_Cancel =1 then 'Cancel' When ((fineamt - paidamt) =0 )then  'Paid' else 'UnPaid/Returned' end as 'Status',CASE WHEN Is_Cancel = 1 THEN fine_Details.Reason ELSE '' END Reason,isnull(paidamt,0) as 'Actual Fine' From fine_details, library,staffmaster where library.lib_code = fine_details.lib_code And (staffmaster.staff_code = fine_details.roll_no or staffmaster.lib_id = fine_details.roll_no) " + strStatus + strDate + " and fine_details.lib_code in( '" + typ1 + "')";
                if (typ != "All")
                {
                    Sql = Sql + "AND Semester in('" + typ + "')";
                }
            }

            fine.Clear();
            fine = d2.select_method_wo_parameter(Sql, "Text");
            int sno = 0;
            double TotfineAmt = 0;
            double TotPaidAmt = 0;
            if (fine.Tables.Count > 0 && fine.Tables[0].Rows.Count > 0)
            {
                dtfine1.Columns.Add("SNo", typeof(string));
                dtfine1.Columns.Add("Roll No", typeof(string));
                dtfine1.Columns.Add("Name", typeof(string));
                dtfine1.Columns.Add("Access No", typeof(string));
                dtfine1.Columns.Add("Title", typeof(string));
                dtfine1.Columns.Add("Issue Date", typeof(string));
                dtfine1.Columns.Add("Due Date", typeof(string));
                dtfine1.Columns.Add("Return Date", typeof(string));
                dtfine1.Columns.Add("Description", typeof(string));
                dtfine1.Columns.Add("NoofOverDue Days", typeof(string));
                dtfine1.Columns.Add("Fine Amount", typeof(string));
                dtfine1.Columns.Add("Receipt No", typeof(string));
                dtfine1.Columns.Add("Paid Date", typeof(string));
                dtfine1.Columns.Add("Paid Amount", typeof(string));
                dtfine1.Columns.Add("Status", typeof(string));
                dtfine1.Columns.Add("Reason", typeof(string));
                dtfine1.Columns.Add("Category", typeof(string));
                dtfine1.Columns.Add("Library Name", typeof(string));

                drfinere = dtfine1.NewRow();
                drfinere["SNo"] = "SNo";
                drfinere["Roll No"] = "Roll No";
                drfinere["Name"] = "Name";
                drfinere["Access No"] = "Access No";
                drfinere["Title"] = "Title";
                drfinere["Issue Date"] = "Issue Date";
                drfinere["Due Date"] = "Due Date";
                drfinere["Return Date"] = "Return Date";
                drfinere["Description"] = "Description";
                drfinere["NoofOverDue Days"] = "NoofOverDue Days";
                drfinere["Fine Amount"] = "Fine Amount";
                drfinere["Receipt No"] = "Receipt No";
                drfinere["Paid Date"] = "Paid Date";
                drfinere["Paid Amount"] = "Paid Amount";
                drfinere["Status"] = "Status";
                drfinere["Reason"] = "Reason";
                drfinere["Category"] = "Category";
                drfinere["Library Name"] = "Library Name";

                dtfine1.Rows.Add(drfinere);
                if (fine.Tables.Count > 0 && fine.Tables[0].Rows.Count > 0)
                {
                    for (int row = 0; row < fine.Tables[0].Rows.Count; row++)
                    {
                        sno++;
                        drfinere = dtfine1.NewRow();
                        recipt = Convert.ToString(fine.Tables[0].Rows[row]["Receipt No"]).Trim();
                        roll = Convert.ToString(fine.Tables[0].Rows[row]["Roll No"]).Trim();
                        if (Convert.ToString(ddlselect.SelectedItem) == "All")
                        {
                            category = Convert.ToString(fine.Tables[0].Rows[row]["Category"]).Trim();
                            student = Convert.ToString(fine.Tables[0].Rows[row]["Name"]).Trim();
                        }
                        else if (Convert.ToString(ddlselect.SelectedItem) == "Student")
                        {
                            student = Convert.ToString(fine.Tables[0].Rows[row]["Student Name"]).Trim();
                        }
                        else if (Convert.ToString(ddlselect.SelectedItem) == "Staff")
                        {
                            staff = Convert.ToString(fine.Tables[0].Rows[row]["staff name"]).Trim();
                        }
                        access = Convert.ToString(fine.Tables[0].Rows[row]["Access No"]).Trim();
                        title = Convert.ToString(fine.Tables[0].Rows[row]["Title"]).Trim();
                        library = Convert.ToString(fine.Tables[0].Rows[row]["Library"]).Trim();
                        issue = Convert.ToString(fine.Tables[0].Rows[row]["Issue Date"]).Trim();
                        due = Convert.ToString(fine.Tables[0].Rows[row]["Due Date"]).Trim();
                        paiddate = Convert.ToString(fine.Tables[0].Rows[row]["Paid Date"]).Trim();
                        booktype = Convert.ToString(fine.Tables[0].Rows[row]["Book Type"]).Trim();
                        description = Convert.ToString(fine.Tables[0].Rows[row]["Description"]).Trim();
                        string[] borrowdate=issue.Split('/');
                        issue=borrowdate[1]+"/"+borrowdate[0]+"/"+borrowdate[2];
                        string[] duedate = due.Split('/');
                        due = duedate[1] + "/" + duedate[0] + "/" + duedate[2];

                        string sqlFine = "select convert(varchar,return_date,103) as 'Return Date',case when datediff(dd,due_date,return_date)< 0  then '' else datediff(dd,due_date,return_date) end as 'No of OverDue Days' from borrow where roll_no='" + roll + "' and acc_no='" + access + "' and borrow_date='" + issue + "' and due_date='" + due + "'";
                        DataSet dsfinerpt = new DataSet();
                        dsfinerpt = d2.select_method_wo_parameter(sqlFine, "text");
                        if (dsfinerpt.Tables[0].Rows.Count > 0)
                        {
                            overduedays = Convert.ToString(dsfinerpt.Tables[0].Rows[0]["No of OverDue Days"]).Trim();
                            Return = Convert.ToString(dsfinerpt.Tables[0].Rows[0]["Return Date"]).Trim();
                        }
                        
                        fineamount = Convert.ToString(fine.Tables[0].Rows[row]["Fine Amount"]).Trim();
                        status = Convert.ToString(fine.Tables[0].Rows[row]["Status"]).Trim();
                        reason = Convert.ToString(fine.Tables[0].Rows[row]["Reason"]).Trim();
                        Actual = Convert.ToString(fine.Tables[0].Rows[row]["Actual Fine"]).Trim();
                        libraryname = Convert.ToString(fine.Tables[0].Rows[row]["Library"]).Trim();

                        drfinere["SNo"] = sno;
                        drfinere["Roll No"] = roll;
                        if (Convert.ToString(ddlselect.SelectedItem) == "All")
                        {
                            drfinere["Category"] = category;
                            drfinere["Name"] = student;
                        }
                        else if (Convert.ToString(ddlselect.SelectedItem) == "Student")
                        {
                            drfinere["Name"] = student;
                        }
                        else if (Convert.ToString(ddlselect.SelectedItem) == "Staff")
                        {
                            drfinere["Name"] = staff;
                        }
                        drfinere["Access No"] = access;
                        drfinere["Title"] = title;
                        drfinere["Issue Date"] = issue;
                        drfinere["Due Date"] = due;
                        drfinere["Return Date"] = Return;
                        drfinere["Description"] = description;
                        drfinere["NoofOverDue Days"] = overduedays;
                        drfinere["Fine Amount"] = fineamount;
                        drfinere["Receipt No"] = recipt;
                        TotfineAmt = TotfineAmt + Convert.ToDouble(fineamount);
                        drfinere["Status"] = status;
                        drfinere["Reason"] = reason;
                        drfinere["Paid Date"] = paiddate;
                        drfinere["Paid Amount"] = Actual;
                        drfinere["Library Name"] = libraryname;
                        TotPaidAmt = TotPaidAmt + Convert.ToDouble(Actual);
                        dtfine1.Rows.Add(drfinere);
                    }
                    drfinere = dtfine1.NewRow();
                    drfinere["SNo"] = "Total";
                    drfinere["Fine Amount"] = TotfineAmt;
                    drfinere["Paid Amount"] = TotPaidAmt;
                    dtfine1.Rows.Add(drfinere);

                    grid_Details.DataSource = dtfine1;
                    grid_Details.DataBind();
                    grid_Details.Visible = true;
                    colour.Visible = true;
                    print2.Visible = true;
                    btnExcel2.Visible = true;
                    btnprintmasterhed2.Visible = true;
                    cmd_rpt_spd_Click();
                    RowHead(grid_Details);
                    double fineamountTotal = 0;
                    fineamountTotal = TotfineAmt - TotPaidAmt;
                    lblpaid1.Text = Convert.ToString(TotPaidAmt + ".00");
                    lblunpaid1.Text = Convert.ToString(fineamountTotal + ".00");
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Record Found!";
            }
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "FineReport"); }
    }

    protected void grid_Details_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex == 0)
        {
        }
        else
        {
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[12].HorizontalAlign = HorizontalAlign.Right;
        }
    }

    protected void RowHead(GridView grid_Details)
    {
        for (int head = 0; head < 1; head++)
        {
            grid_Details.Rows[head].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            grid_Details.Rows[head].Font.Bold = true;
            grid_Details.Rows[head].HorizontalAlign = HorizontalAlign.Center;
        }
    }

    #region Print
    protected void btnExcel_Click2(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname2.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(grid_Details, reportname);

            }
            else
            {
                txtexcelname2.Focus();
            }
        }
        catch (Exception ex)
        {
        }

    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    public void btnprintmaster_Click2(object sender, EventArgs e)
    {
        try
        {
            lblvalidation3.Text = "";
            txtexcelname2.Text = "";
            string degreedetails;
            string pagename;
            degreedetails = "Fine Report";
            //+'@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            pagename = "FineDetailsReport.aspx";
            string ss = Session["usercode"].ToString();
            Printcontrolhed2.loadspreaddetails(grid_Details, pagename, degreedetails, 0, ss);

            Printcontrolhed2.Visible = true;
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "FineReport"); }
    }

    protected void getPrintSettings2()
    {
        try
        {

            #region Excel print settings
            string usertype = "";
            if (userCode.Trim() != "")
                usertype = " and usercode='" + userCode + "'";
            else if (groupUserCode.Trim() != "")
                usertype = " and group_code='" + groupUserCode + "'";
            string printset = d2.GetFunction("select value from Master_Settings where settings='Excel and Pdf Print Settings' " + usertype + " ");
            if (printset != "")
            {
                if (printset.Contains("E"))
                {
                    lblrptname2.Visible = true;
                    txtexcelname2.Visible = true;
                    btnExcel2.Visible = true;
                }
                if (printset.Contains("P"))
                {
                    btnprintmasterhed2.Visible = true;
                }
                if (printset == "0")
                {
                    lblrptname2.Visible = true;
                    txtexcelname2.Visible = true;
                    btnExcel2.Visible = true;
                    btnprintmasterhed2.Visible = true;

                }
            }
            #endregion
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "FineReport"); }
    }

    #endregion

    #endregion

    #region cummaltive

    protected void btngo1Click(object sender, EventArgs e)
    {
        try
        {
            DataTable cumreport = new DataTable();
            cumreport = collection();
            if (cumreport.Rows.Count > 0)
            {
                loadspreadcum(cumreport);
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Record Found!";
            }
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "FineReport"); }
    }

    protected void GrdCumulative_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }

    //protected void GridView1_OnPageIndexChanged(object sender, GridViewPageEventArgs e)
    //{
    //    grid_Details.PageIndex = e.NewPageIndex;
    //    btngo1Click(sender, e);
    //}

    private DataTable collection()
    {

        string collegeCode1 = string.Empty;
        string library1 = string.Empty;
        string sem1 = string.Empty;

        string fromdate1 = string.Empty;
        string todate1 = string.Empty;
        string fromdate2 = string.Empty;
        string todate2 = string.Empty;
        try
        {
            if (ddlcollege1.Items.Count > 0)
                collegeCode1 = Convert.ToString(ddlcollege1.SelectedValue);
            if (ddllibrary1.Items.Count > 0)
                library1 = Convert.ToString(ddllibrary1.SelectedValue);
            if (ddlsem1.Items.Count > 0)
                sem1 = Convert.ToString(ddlsem1.SelectedValue);

            string typ = string.Empty;
            if (ddlsem1.Items.Count > 0)
            {
                for (int i = 0; i < ddlsem1.Items.Count - 1; i++)
                {
                    if (Convert.ToString(ddlsem1.SelectedItem) == "All")
                    {
                        if (typ == "")
                        {
                            typ = "All";
                        }
                    }
                    else
                        typ = ddlsem1.SelectedValue;
                }
            }
            string typ1 = string.Empty;
            if (ddllibrary1.Items.Count > 0)
            {
                for (int i = 0; i < ddllibrary1.Items.Count - 1; i++)
                {
                    if (Convert.ToString(ddllibrary1.SelectedItem) == "All")
                    {
                        if (typ1 == "")
                        {
                            typ1 = "" + ddllibrary1.Items[i + 1].Value + "";
                        }
                        else
                        {
                            typ1 = typ1 + "'" + "," + "'" + ddllibrary1.Items[i + 1].Value + "";
                        }
                    }
                    else
                        typ1 = ddllibrary1.SelectedValue;
                }
            }

            string fromDate = txt_fromdate2.Text;
            string toDate = txt_todate2.Text;
            string[] fromdate = fromDate.Split('/');
            string[] todate = toDate.Split('/');
            if (fromdate.Length == 3)
                fromdate2 = fromdate[1].ToString() + "/" + fromdate[0].ToString() + "/" + fromdate[2].ToString();

            if (todate.Length == 3)
                todate2 = todate[1].ToString() + "/" + todate[0].ToString() + "/" + todate[2].ToString();
            if (Rblreporttype.SelectedIndex == 0)
            {
                if (library1 == "All")
                {
                    Sql = "SELECT cal_date,SUM(CASE WHEN paidamt>0 THEN paidamt ELSE 0 END) paidAmt,SUM(CASE WHEN paidamt = 0 THEN fineamt ELSE 0 END) UnpaidAmt FROM fine_details where cal_date between '" + fromdate2 + "' and '" + todate2 + "' and isnull(is_cancel,'0')=0 ";
                    if (typ != "All")
                    {
                        Sql = Sql + "AND Semester in('" + typ + "')";
                    }
                }
                else
                {
                    Sql = "SELECT cal_date,SUM(CASE WHEN paidamt>0 THEN paidamt ELSE 0 END) paidAmt,SUM(CASE WHEN paidamt = 0 THEN fineamt ELSE 0 END) UnpaidAmt FROM fine_details where cal_date between '" + fromdate2 + "' and '" + todate2 + "' and lib_code in ('" + typ1 + "') and isnull(is_cancel,'0')=0 ";
                    if (sem1 != "All")
                    {
                        Sql = Sql + "AND Semester in('" + typ + "')";
                    }
                }
                Sql = Sql + "group by cal_date order by cal_date";
            }
            if (Rblreporttype.SelectedIndex == 1)
            {
                if (library1 == "All")
                {
                    Sql = "select datename(mm,cal_date) montname,month(cal_date) montno,datename(yyyy,cal_date ) year,sum(paidamt) paidamt from fine_details where paidamt > 0 and cal_date between '" + fromdate2 + "' and '" + todate2 + "' and isnull(is_cancel,'0')=0 ";
                    if (typ != "All")
                    {
                        Sql = Sql + "AND Semester in('" + typ + "')";
                    }
                }
                else
                {
                    Sql = "select datename(mm,cal_date) montname,month(cal_date) montno,datename(yyyy,cal_date ) year,sum(paidamt) from fine_details where paidamt > 0 and cal_date between '" + fromdate2 + "' and '" + todate2 + "' and lib_code in ('" + typ1 + "') and isnull(is_cancel,'0')=0 ";
                    if (sem1 != "All")
                    {
                        Sql = Sql + "AND Semester in('" + typ + "')";
                    }
                }
                Sql += " GROUP BY datename(mm,cal_date ), month(cal_date),datename(yyyy,cal_date ) order by datename(yyyy,cal_date ) ,month(cal_date) ";
            }
            if (Rblreporttype.SelectedIndex == 2)
            {
                if (library1 == "All")
                {
                    Sql = "select datename(yyyy,cal_date) Year,sum(paidamt) paidamt from fine_details where paidamt > 0 and cal_date between '" + fromdate2 + "' and '" + todate2 + "' and isnull(is_cancel,'0')=0 ";
                    if (typ != "All")
                    {
                        Sql = Sql + "AND Semester in('" + typ + "')";
                    }
                }
                else
                {
                    Sql = "select datename(yyyy,cal_date) Year,sum(paidamt) from fine_details where paidamt > 0 and cal_date between '" + fromdate2 + "' and '" + todate2 + "' and lib_code in ('" + typ1 + "') and isnull(is_cancel,'0')=0 ";
                    if (sem1 != "All")
                    {
                        Sql = Sql + "AND Semester in('" + typ + "')";
                    }
                }
                Sql += " GROUP BY datename(yyyy,cal_date ) order by datename(yyyy,cal_date) ";
            }
            finecollection.Clear();
            finecollection = dirAcc.selectDataTable(Sql);
        }

        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "FineReport"); }
        return finecollection;

    }

    private void loadspreadcum(DataTable ds)
    {
        try
        {
            string date = string.Empty;
            string amount = string.Empty;
            int aa = 0;
            int grandprev = 0;
            DataTable dt = new DataTable();
            DataRow dr;
            int rowcount = 0;
            dt.Columns.Add("SNo");
            if (Rblreporttype.SelectedIndex == 0)
            {
                dt.Columns.Add("Date");
            }
            if (Rblreporttype.SelectedIndex == 1)
            {
                dt.Columns.Add("Month");
            }
            if (Rblreporttype.SelectedIndex == 2)
            {
                dt.Columns.Add("Year");
            }
            dt.Columns.Add("Paid Amount");
            dt.Columns.Add("Un PaidAmount");
            dr = dt.NewRow();
            dr["SNo"] = "SNo";
            if (Rblreporttype.SelectedIndex == 0)
            {
                dr["Date"] = "Date";
            }
            if (Rblreporttype.SelectedIndex == 1)
            {
                dr["Month"] = "Month";
            }
            if (Rblreporttype.SelectedIndex == 2)
            {
                dr["Year"] = "Year";
            }
            dr["Paid Amount"] = "Paid Amount";
            dr["Un PaidAmount"] = "Un PaidAmount";
            dt.Rows.Add(dr);
            double dblunpaid = 0;
            string fromdate2 = string.Empty;
            string todate2 = string.Empty;
            string fromDate = txt_fromdate2.Text;
            string toDate = txt_todate2.Text;
            string[] fromdate = fromDate.Split('/');
            string[] todate = toDate.Split('/');
            string monthName = "";
            string CalDate = "";
            string Year = "";
            if (fromdate.Length == 3)
                fromdate2 = fromdate[1].ToString() + "/" + fromdate[0].ToString() + "/" + fromdate[2].ToString();

            if (todate.Length == 3)
                todate2 = todate[1].ToString() + "/" + todate[0].ToString() + "/" + todate[2].ToString();
            for (int i = 0; i < ds.Rows.Count; i++)
            {
                dr = dt.NewRow();
                dr["SNo"] = Convert.ToString(++rowcount);

                //========================Unpaid Amount=========================//
                string library1 = "";
                string sem1 = "";
                if (ddllibrary1.Items.Count > 0)
                    library1 = Convert.ToString(ddllibrary1.SelectedValue);
                if (ddlsem1.Items.Count > 0)
                    sem1 = Convert.ToString(ddlsem1.SelectedValue);

                string typ = string.Empty;
                if (ddlsem1.Items.Count > 0)
                {
                    for (int ij = 0; ij < ddlsem1.Items.Count - 1; ij++)
                    {
                        if (Convert.ToString(ddlsem1.SelectedItem) == "All")
                        {
                            if (typ == "")
                            {
                                typ = "All";
                            }
                        }
                        else
                            typ = ddlsem1.SelectedValue;
                    }
                }
                string typ1 = string.Empty;
                if (ddllibrary1.Items.Count > 0)
                {
                    for (int ik = 0; ik < ddllibrary1.Items.Count - 1; ik++)
                    {
                        if (Convert.ToString(ddllibrary1.SelectedItem) == "All")
                        {
                            if (typ1 == "")
                            {
                                typ1 = "" + ddllibrary1.Items[ik + 1].Value + "";
                            }
                            else
                            {
                                typ1 = typ1 + "'" + "," + "'" + ddllibrary1.Items[ik + 1].Value + "";
                            }
                        }
                        else
                            typ1 = ddllibrary1.SelectedValue;
                    }
                }
                if (Rblreporttype.SelectedIndex == 0)
                {
                    CalDate = Convert.ToString(ds.Rows[i]["cal_date"]);
                    string[] dat = CalDate.Split('/');
                    if (dat.Length == 3)
                        CalDate = dat[1] + '/' + dat[0] + '/' + dat[2];
                    dr["Date"] = CalDate.Split(' ')[0];
                }
                if (Rblreporttype.SelectedIndex == 1)
                {
                    monthName = Convert.ToString(ds.Rows[i]["montname"]);
                    Year = Convert.ToString(ds.Rows[i]["Year"]);
                    dr["Month"] = monthName;
                }
                if (Rblreporttype.SelectedIndex == 2)
                {
                    Year = Convert.ToString(ds.Rows[i]["Year"]);
                    dr["Year"] = Year;
                }
                //if (Rblreporttype.SelectedIndex == 0)
                //{
                //    CalDate = Convert.ToString(ds.Rows[i]["cal_date"]);
                //    if (library1 == "All")
                //    {
                //        Sql = "select cal_date,sum(fineamt) as unpaid from fine_details where paidamt = 0 and cal_date between '" + fromdate2 + "' and '" + todate2 + "' and isnull(is_cancel,'0')=0 ";
                //        if (typ != "All")
                //        {
                //            Sql = Sql + "AND Semester in('" + typ + "')";
                //        }
                //    }
                //    else
                //    {
                //        Sql = "select cal_date,sum(fineamt) as unpaid from fine_details where paidamt = 0 and lib_code in ('" + typ1 + "') and isnull(is_cancel,'0')=0 ";
                //        if (sem1 != "All")
                //        {
                //            Sql = Sql + "AND Semester in('" + typ + "')";
                //        }
                //    }
                //    Sql = Sql + "group by cal_date order by cal_date";
                //}
                if (Rblreporttype.SelectedIndex == 1)
                {
                    if (library1 == "All")
                    {
                        Sql = "select datename(mm,cal_date) montname,month(cal_date) montno,datename(yyyy,cal_date ) year,sum(fineamt) as unpaid from fine_details where paidamt = 0 and datename(mm,cal_date)='" + monthName + "' and datename(yyyy,cal_date )='" + Year + "' and isnull(is_cancel,'0')=0 ";
                        if (typ != "All")
                        {
                            Sql = Sql + "AND Semester in('" + typ + "')";
                        }
                    }
                    else
                    {
                        Sql = "select datename(mm,cal_date) montname,month(cal_date) montno,datename(yyyy,cal_date ) year,sum(fineamt) as unpaid from fine_details where paidamt = 0 and datename(mm,cal_date)='" + monthName + "' and datename(yyyy,cal_date )='" + Year + "' and lib_code in ('" + typ1 + "') and isnull(is_cancel,'0')=0 ";
                        if (sem1 != "All")
                        {
                            Sql = Sql + "AND Semester in('" + typ + "')";
                        }
                    }
                    Sql = Sql + "group by datename(mm,cal_date) ,month(cal_date),datename(yyyy,cal_date ) ";
                }
                if (Rblreporttype.SelectedIndex == 2)
                {
                    if (library1 == "All")
                    {
                        Sql = "select datename(yyyy,cal_date) Year,sum(fineamt) as unpaid from fine_details where paidamt = 0 and cal_date between '" + fromdate2 + "' and '" + todate2 + "' and datename(yyyy,cal_date )='" + Year + "' and isnull(is_cancel,'0')=0 ";
                        if (typ != "All")
                        {
                            Sql = Sql + "AND Semester in('" + typ + "')";
                        }
                    }
                    else
                    {
                        Sql = "select datename(yyyy,cal_date) Year,sum(fineamt) as unpaid from fine_details where paidamt = 0 and cal_date between '" + fromdate2 + "' and '" + todate2 + "' and datename(yyyy,cal_date )='" + Year + "' and lib_code in ('" + typ1 + "') and isnull(is_cancel,'0')=0 ";
                        if (sem1 != "All")
                        {
                            Sql = Sql + "AND Semester in('" + typ + "')";
                        }
                    }
                    Sql = Sql + "group by datename(yyyy,cal_date) ";
                }
                DataSet dsUnpaid = new DataSet();
                dsUnpaid = d2.select_method_wo_parameter(Sql, "text");
                int unpaid = 0;
                string dtdate = "";
                if (dsUnpaid.Tables[0].Rows.Count > 0)
                {
                    if (Rblreporttype.SelectedIndex != 0)
                    {
                        unpaid = Convert.ToInt32(dsUnpaid.Tables[0].Rows[0]["unpaid"]);
                        if (unpaid > 0)
                        {
                            dr["Un PaidAmount"] = unpaid;
                            dblunpaid = dblunpaid + unpaid;
                        }
                        else
                        {
                            dr["Un PaidAmount"] = "0";
                        }
                    }
                }
                //====================================================//
                if (Rblreporttype.SelectedIndex == 0)
                {
                    dr["Paid Amount"] = Convert.ToString(ds.Rows[i]["paidamt"]);
                    dt.Rows.Add(dr);
                    int m = Convert.ToInt32(ds.Rows[i]["paidamt"]);
                    grandprev = grandprev + m;
                    dr["Un PaidAmount"] = Convert.ToString(ds.Rows[i]["UnpaidAmt"]);
                    dblunpaid = dblunpaid + Convert.ToDouble(ds.Rows[i]["UnpaidAmt"]);
                }
                else
                {
                    dr["Paid Amount"] = Convert.ToString(ds.Rows[i]["paidamt"]);
                    dt.Rows.Add(dr);
                    int m = Convert.ToInt32(ds.Rows[i]["paidamt"]);
                    grandprev = grandprev + m;
                }

            }
            dr = dt.NewRow();
            if (Rblreporttype.SelectedIndex == 0)
            {
                dr["Date"] = "Total";
            }
            if (Rblreporttype.SelectedIndex == 1)
            {
                dr["Month"] = "Total";
            }
            if (Rblreporttype.SelectedIndex == 2)
            {
                dr["Year"] = "Total";
            }
            dr["Paid Amount"] = Convert.ToString(grandprev);
            dr["Un PaidAmount"] = Convert.ToString(dblunpaid);
            dt.Rows.Add(dr);

            GrdCumulative.DataSource = dt;
            GrdCumulative.DataBind();
            GrdCumulative.Visible = true;
            Div1.Visible = true;
            ImageButton2.Visible = true;
            ImageButton1.Visible = true;
            RowHead1(GrdCumulative);
        }
        catch (Exception ex)
        { //d2.sendErrorMail(ex, userCollegeCode, "FineReport"); 
        }
    }

    protected void RowHead1(GridView GrdCumulative)
    {
        for (int head = 0; head < 1; head++)
        {
            GrdCumulative.Rows[head].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            GrdCumulative.Rows[head].Font.Bold = true;
            GrdCumulative.Rows[head].HorizontalAlign = HorizontalAlign.Center;

        }
    }

    #region Print
    protected void btnExcel_Click3(object sender, EventArgs e)
    {
        try
        {
            string reportname = TextBox1.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(GrdCumulative, reportname);

            }
            else
            {
                TextBox1.Focus();
            }
        }
        catch (Exception ex)
        {
        }

    }



    public void btnprintmaster_Click3(object sender, EventArgs e)
    {
        try
        {
            lblvalidation3.Text = "";
            TextBox1.Text = "";
            string degreedetails;
            string pagename;
            degreedetails = "Fine Report";
            //+'@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            pagename = "FineDetailsReport.aspx";
            string ss = Session["usercode"].ToString();
            NEWPrintMater1.loadspreaddetails(GrdCumulative, pagename, degreedetails, 0, ss);

            NEWPrintMater1.Visible = true;
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "FineReport"); }
    }

    protected void getPrintSettings3()
    {
        try
        {

            #region Excel print settings
            string usertype = "";
            if (userCode.Trim() != "")
                usertype = " and usercode='" + userCode + "'";
            else if (groupUserCode.Trim() != "")
                usertype = " and group_code='" + groupUserCode + "'";
            string printset = d2.GetFunction("select value from Master_Settings where settings='Excel and Pdf Print Settings' " + usertype + " ");
            if (printset != "")
            {
                if (printset.Contains("E"))
                {
                    Label4.Visible = true;
                    TextBox1.Visible = true;
                    ImageButton1.Visible = true;
                }
                if (printset.Contains("P"))
                {
                    ImageButton2.Visible = true;
                }
                if (printset == "0")
                {
                    Label4.Visible = true;
                    TextBox1.Visible = true;
                    ImageButton1.Visible = true;
                    ImageButton2.Visible = true;

                }
            }
            #endregion
        }
        catch (Exception ex)
        { d2.sendErrorMail(ex, userCollegeCode, "FineReport"); }
    }

    #endregion

    #endregion

    #region alertclose
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        try
        {
            lblalerterr.Text = string.Empty;
            alertpopwindow.Visible = false;

        }
        catch (Exception ex) { }
        {
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {

        // Div1.Visible = false;
        //Div4.Visible = false;
    }
    #endregion

}