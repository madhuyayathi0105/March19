using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using InsproDataAccess;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Configuration;

public partial class LibraryMod_Cumulative_Books_Journal : System.Web.UI.Page
{
    #region Filed_Declaration
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
    DataTable dtcumboks = new DataTable();
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
                Department();
                //FpSpread1.Visible = false;
                //rptprint.Visible = false;
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
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Library_Books_And_Journal_Details"); }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        getLibPrivil();
        //FpSpread1.Visible = false;
        // rptprint.Visible = false;
    }

    #endregion

    #region Library

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
            Library(LibCollection);
        }
        catch (Exception ex)
        {
        }
    }

    public void Library(string libcode)
    {
        try
        {
            ddllibrary.Items.Clear();
            ds.Clear();
            string College = ddlCollege.SelectedValue.ToString();
            string SelectQ = string.Empty;
            if (!string.IsNullOrEmpty(College))
            {
                string lib_name = "select lib_code,lib_name,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + libcode + " AND college_code=" + College + " ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
                ds = da.select_method_wo_parameter(lib_name, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddllibrary.DataSource = ds;
                    ddllibrary.DataTextField = "lib_name";
                    ddllibrary.DataValueField = "lib_code";
                    ddllibrary.DataBind();
                    ddllibrary.Items.Insert(0, "All");
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Library_Books_And_Journal_Details"); }


    }

    protected void ddllibrary_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //FpSpread1.Visible = false;
            //rptprint.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Library_Books_And_Journal_Details"); }

    }

    #endregion

    #region Department
    public void Department()
    {
        try
        {

            string College = ddlCollege.SelectedValue.ToString();
            if (!string.IsNullOrEmpty(College))
            {
                //hat.Add("collegecode", College);
                //ds.Clear();
                //ds = da.select_method("LoadJournalDepartment", hat, "sp");
                string loaddept = "Select distinct ISNULL(dept_name,'') dept_name  from journal_dept order by dept_name ";
                ds.Clear();
                ds = da.select_method_wo_parameter(loaddept, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_dept.DataSource = ds;
                    ddl_dept.DataTextField = "Dept_Name";
                    ddl_dept.DataValueField = "Dept_Name";
                    ddl_dept.DataBind();
                    ddl_dept.Items.Insert(0, "All");
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Library_Books_And_Journal_Details"); }
    }

    protected void ddl_dept_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //FpSpread1.Visible = false;
            //rptprint.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Library_Books_And_Journal_Details"); }

    }


    #endregion

    protected void chkredate_CheckedChanged(object sender, EventArgs e)
    {
        //FpSpread1.Visible = false;
        // rptprint.Visible = false;
    }

    protected void gridview1_onselectedindexchanged(object sender, EventArgs e)
    {
    }

    protected void gridview1_onpageindexchanged(object sender, GridViewPageEventArgs e)
    {
        gridview1.PageIndex = e.NewPageIndex;
        btngo_Click(sender, e);
    }

    #region Go

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            #region get Value

            DataSet dsgo = new DataSet();
            DataSet dstol = new DataSet();
            string dept = "";
            if (ddlCollege.Items.Count > 0)
                collcode = Convert.ToString(ddlCollege.SelectedValue);
            if (ddllibrary.Items.Count > 0)
                libcode = Convert.ToString(ddllibrary.SelectedValue);
            if (ddl_dept.Items.Count > 0)
                dept = Convert.ToString(ddl_dept.SelectedValue);
            string getrecord = "";
            string qrylib = "";
            string qrylib1 = "";
            string qrydept = "";
            //if (libcode != "All")
            //    qrylib = "AND Lib_Code='" + libcode + "'";
            if (dept != "All")
                qrydept = "AND Dept_Name='" + dept + "'";
            string typ1 = string.Empty;
            if (ddllibrary.Items.Count > 0)
            {
                for (int i = 0; i < ddllibrary.Items.Count - 1; i++)
                {
                    if (Convert.ToString(ddllibrary.SelectedItem) == "All")
                    {
                        if (typ1 == "")
                        {
                            typ1 = "" + ddllibrary.Items[i + 1].Value + "";
                        }
                        else
                        {
                            typ1 = typ1 + "'" + "," + "'" + ddllibrary.Items[i + 1].Value + "";
                        }
                    }
                    else
                        typ1 = ddllibrary.SelectedValue;
                }
            }

            int sno = 0;
            string totitle = "";
            string totvol = "";
            int LngTotTitle = 0;
            int LngTotVol = 0;
            int LngTotNatJrnl = 0;
            int LngTotINatJrnl = 0;
            int rowcount = 0;
            DataRow dr;


            if (chkredate.Checked == false)
            {
                getrecord = "SELECT DISTINCT isnull(Dept_Name,'') Dept_Name FROM Journal_Dept WHERE 1=1  AND Lib_Code in ('" + typ1 + "') " + qrydept + " group by dept_name order by dept_name";
                dsgo.Clear();
                dsgo = d2.select_method_wo_parameter(getrecord, "Text");


                if (dsgo.Tables.Count > 0 && dsgo.Tables[0].Rows.Count > 0)
                {
                    dtcumboks.Columns.Add("SNo", typeof(string));
                    dtcumboks.Columns.Add("Department", typeof(string));
                    dtcumboks.Columns.Add("Title", typeof(string));
                    dtcumboks.Columns.Add("Volumes", typeof(string));
                    dtcumboks.Columns.Add("National Journals", typeof(string));
                    dtcumboks.Columns.Add("International Journals", typeof(string));
                    dtcumboks.Columns.Add("Remarks", typeof(string));


                    for (int row = 0; row < dsgo.Tables[0].Rows.Count; row++)
                    {

                        dr = dtcumboks.NewRow();
                        sno++;
                        dr["SNo"] = Convert.ToString(sno);
                        string dept_name = Convert.ToString(dsgo.Tables[0].Rows[row]["Dept_Name"]);
                        dr["Department"] = Convert.ToString(dsgo.Tables[0].Rows[row]["Dept_Name"]);
                        string Sql = "SELECT COUNT(DISTINCT Title) TotTitle,COUNT(*) TotVol FROM BookDetails WHERE Dept_Code ='" + dept_name + "' AND Lib_Code in ('" + typ1 + "')";
                        dstol.Clear();
                        dstol = d2.select_method_wo_parameter(Sql, "Text");
                        if (dstol.Tables[0].Rows.Count > 0)
                        {

                            totitle = Convert.ToString(dstol.Tables[0].Rows[0]["TotTitle"]);
                            totvol = Convert.ToString(dstol.Tables[0].Rows[0]["TotVol"]);
                        }
                        else
                        {
                            totitle = "0";
                            totvol = "0";
                        }
                        dr["Title"] = totitle;
                        dr["Volumes"] = totvol;
                        LngTotTitle = LngTotTitle + Convert.ToInt32(totitle);
                        LngTotVol = LngTotVol + Convert.ToInt32(totvol);

                        string SqlTotNat = d2.GetFunction("SELECT COUNT(*) TotNat FROM Journal_Master WHERE Is_National = 1 AND Department ='" + dept_name + "' AND Lib_Code in ('" + typ1 + "')");
                        string SqlTotInNat = d2.GetFunction("SELECT COUNT(*) TotInNat FROM Journal_Master WHERE Is_National = 0 AND Department ='" + dept_name + "' AND Lib_Code in ('" + typ1 + "')");
                        LngTotNatJrnl = LngTotNatJrnl + Convert.ToInt32(SqlTotNat);
                        LngTotINatJrnl = LngTotINatJrnl + Convert.ToInt32(SqlTotInNat);
                        dr["National Journals"] = SqlTotNat;
                        dr["International Journals"] = SqlTotInNat;


                        dtcumboks.Rows.Add(dr);

                    }
                    gridview1.DataSource = dtcumboks;
                    gridview1.DataBind();
                    gridview1.Visible = true;
                    rptprint.Visible = true;

                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "No Record Found!";

                }
            }
            else
            {

                DataRow dr1;
                dtcumboks.Columns.Add("SNo", typeof(string));
                dtcumboks.Columns.Add("Title", typeof(string));
                dtcumboks.Columns.Add("Volumes", typeof(string));
                dtcumboks.Columns.Add("National Journals", typeof(string));
                dtcumboks.Columns.Add("International Journals", typeof(string));
                dtcumboks.Columns.Add("Remarks", typeof(string));

                sno++;
                dr1 = dtcumboks.NewRow();

                string Sql = "SELECT COUNT(DISTINCT Title) TotTitle,COUNT(*) TotVol FROM BookDetails where Lib_Code in ('" + typ1 + "')";
                dstol.Clear();
                dstol = d2.select_method_wo_parameter(Sql, "Text");

                if (dstol.Tables[0].Rows.Count > 0)
                {
                    totitle = Convert.ToString(dstol.Tables[0].Rows[0]["TotTitle"]);
                    totvol = Convert.ToString(dstol.Tables[0].Rows[0]["TotVol"]);
                }
                else
                {
                    totitle = "0";
                    totvol = "0";
                }

                LngTotTitle = LngTotTitle + Convert.ToInt32(totitle);
                LngTotVol = LngTotVol + Convert.ToInt32(totvol);
                dr1["SNo"] = Convert.ToString(sno);
                dr1["Title"] = LngTotTitle;
                dr1["Volumes"] = LngTotVol;
                string SqlTotNat1 = d2.GetFunction("SELECT COUNT(*) TotNat FROM Journal_Master WHERE Is_National = 1  AND Lib_Code in ('" + typ1 + "')");
                string SqlTotInNat1 = d2.GetFunction("SELECT COUNT(*) TotInNat FROM Journal_Master WHERE Is_National = 0 AND Lib_Code in ('" + typ1 + "')");
                LngTotNatJrnl = LngTotNatJrnl + Convert.ToInt32(SqlTotNat1);
                LngTotINatJrnl = LngTotINatJrnl + Convert.ToInt32(SqlTotInNat1);
                dr1["National Journals"] = LngTotNatJrnl;
                dr1["International Journals"] = LngTotINatJrnl;

                dtcumboks.Rows.Add(dr1);

                gridview1.DataSource = dtcumboks;
                gridview1.DataBind();
                gridview1.Visible = true;

                rptprint.Visible = true;

            }
            #endregion
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Library_Books_And_Journal_Details"); }
    }

    #endregion


    #region Print

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Cumulative Book And Journal";
            string pagename = "Cumulative_Books_Journal.aspx";
            string ss = Session["usercode"].ToString();
            Printcontrol.loadspreaddetails(gridview1, pagename, degreedetails, 0, ss, "1");
            Printcontrol.Visible = true;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Library_Books_And_Journal_Details"); }
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
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Library_Books_And_Journal_Details"); }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }
    #endregion

    #region Close
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }
    #endregion

}