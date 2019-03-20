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
using System.Configuration;

public partial class LibraryMod_librarynorm : System.Web.UI.Page
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
    static bool isSaveBtnClick = false;
    bool check = false;
    DataTable boklib = new DataTable();
    DataTable periodlib = new DataTable();
    DataRow drbok;
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
                inwardentry();
                Binddept();
                btn_save.Enabled = false;
            }
        }
        catch
        { }
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
        catch (Exception ex) { }
        {
        }
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

    public void BindLibrary(string Libcode)
    {
        try
        {
            ddlLibrary.Items.Clear();
            ds.Clear();
            string College = ddlCollege.SelectedValue.ToString();
            string strquery = "select lib_code,lib_name,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + Libcode + " AND college_code=" + College + " ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
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

        catch (Exception ex) { }
    }

    #endregion

    #region inwardentry

    public void inwardentry()
    {
        try
        {
            ddlinwardentry.Items.Add("Books");
            ddlinwardentry.Items.Add("Periodicals");
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "Book Statistic"); }
    }

    #endregion

    #region dept

    public void Binddept()
    {
        try
        {
            ds.Clear();
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

        catch (Exception ex) { }
    }

    #endregion

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            showreport2.Visible = false;
            ////print2.Visible = false;
        }
        catch (Exception ex) { }
    }

    protected void ddllibrary_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            showreport2.Visible = false;
            //print2.Visible = false;
        }
        catch (Exception ex) { }
    }

    protected void ddlinwardentry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            showreport2.Visible = false;
            Binddept();
            //print2.Visible = false;
        }
        catch (Exception ex) { }
    }

    protected void ddldept_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            showreport2.Visible = false;
            //print2.Visible = false;
        }
        catch (Exception ex) { }
    }

    #region save

    protected void btnsave_Click(object sender, EventArgs e)
    {
        isSaveBtnClick = true;
        try
        {
            int query = 0;
            string typ1 = string.Empty;
            int Insert = 0;
            if (ddlinwardentry.SelectedIndex == 0)
            {
                foreach (GridViewRow gvrow in grdLibNorms.Rows)
                {
                    int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    string deps = "";
                    string normss = "";
                    string nortits = "";
                    Label dept = (Label)grdLibNorms.Rows[RowCnt].FindControl("lbl_dep");
                    if (dept.Text.Trim() != "")
                    {
                        deps = dept.Text.Trim();
                    }
                    TextBox asPerVol = (TextBox)grdLibNorms.Rows[RowCnt].FindControl("lbl_aspervol");
                    if (asPerVol.Text.Trim() != "")
                    {
                        normss = asPerVol.Text.Trim();
                    }
                    TextBox asPerTilte = (TextBox)grdLibNorms.Rows[RowCnt].FindControl("lbl_aspertit");
                    if (asPerTilte.Text.Trim() != "")
                    {
                        nortits = asPerTilte.Text.Trim();
                    }
                    library = Convert.ToString(ddlLibrary.SelectedValue);

                    if (!string.IsNullOrEmpty(deps) && !string.IsNullOrEmpty(normss) && !string.IsNullOrEmpty(nortits))
                    {
                        string sqld = "DELETE FROM Lib_Norms WHERE Dept ='" + deps + "' AND Lib_Code='" + library + "' ";
                        query = d2.update_method_wo_parameter(sqld, "TEXT");

                        string sqli = "INSERT INTO Lib_Norms(Lib_Code,Dept,Norms,National_Norms,International_Norms,Norms_Title) VALUES ('" + library + "','" + deps + "','" + normss + "','',''," + nortits + ")";
                        query = d2.update_method_wo_parameter(sqli, "Text");
                        Insert++;
                    }
                }
            }
            else if (ddlinwardentry.SelectedIndex == 1)
            {
                foreach (GridViewRow gvrow in grdLibNormsPer.Rows)
                {
                    int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                    string deps = "";
                    string NatNorms = "";
                    string IntNorms = "";
                    Label dept = (Label)grdLibNormsPer.Rows[RowCnt].FindControl("lbl_dep");
                    if (dept.Text.Trim() != "")
                    {
                        deps = dept.Text.Trim();
                    }
                    TextBox asPerVol = (TextBox)grdLibNormsPer.Rows[RowCnt].FindControl("lbl_aspernationl");
                    if (asPerVol.Text.Trim() != "")
                    {
                        NatNorms = asPerVol.Text.Trim();
                    }
                    TextBox asPerTilte = (TextBox)grdLibNormsPer.Rows[RowCnt].FindControl("lbl_asperinter");
                    if (asPerTilte.Text.Trim() != "")
                    {
                        IntNorms = asPerTilte.Text.Trim();
                    }
                    library = Convert.ToString(ddlLibrary.SelectedValue);

                    if (!string.IsNullOrEmpty(deps) && !string.IsNullOrEmpty(NatNorms) && !string.IsNullOrEmpty(IntNorms))
                    {
                        string sqld = "DELETE FROM Lib_Norms WHERE Dept ='" + deps + "' AND Lib_Code='" + library + "'";
                        query = d2.update_method_wo_parameter(sqld, "Text");
                        string sqli = "INSERT INTO Lib_Norms(Lib_Code,Dept,Norms,National_Norms,International_Norms) VALUES ('" + library + "','" + deps + "','','" + NatNorms + "','" + IntNorms + "')";
                        query = d2.update_method_wo_parameter(sqli, "Text");
                        Insert++;
                    }
                }
            }
            if (Insert > 0)
            {
                alertpopwindow.Visible = true;
                pnl2.Visible = true;
                lblalerterr.Text = "Saved Successfully";
                btngoClick(sender, e);
            }
        }
        catch { }
    }

    #endregion

    protected void grdLibNorms_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.Header)
        //{
        //    int tempt = Convert.ToInt32(ViewState["temp_table"]);
        //    GridView HeaderGrid = (GridView)sender;
        //    GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
        //    TableCell headerCell = new TableCell();

        //    Table table = (Table)grdLibNorms.Controls[0];
        //    TableRow headerRow = table.Rows[0];
        //    int numberOfHeaderCellsToMove = headerRow.Cells.Count - 1;

        //    for (int i = 0; i < 2; i++)
        //    {
        //        headerCell = headerRow.Cells[0];
        //        HeaderGridRow.Cells.Add(headerCell);
        //        headerCell.RowSpan = 2;
        //    }
        //    grdLibNorms.Controls[0].Controls.AddAt(0, HeaderGridRow);
        //    TableHeaderCell HeaderCell = new TableHeaderCell();

        //    HeaderCell = new TableHeaderCell();
        //    HeaderCell.Text = "As Per Norms";
        //    HeaderCell.ColumnSpan = 2;
        //    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
        //    HeaderGridRow.Cells.Add(HeaderCell);
        //    grdLibNorms.Controls[0].Controls.AddAt(0, HeaderGridRow);

        //    HeaderCell = new TableHeaderCell();
        //    HeaderCell.Text = "Available";
        //    HeaderCell.ColumnSpan = 2;
        //    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
        //    HeaderGridRow.Cells.Add(HeaderCell);
        //    grdLibNorms.Controls[0].Controls.AddAt(0, HeaderGridRow);

        //    HeaderCell = new TableHeaderCell();
        //    HeaderCell.Text = "Needed";
        //    HeaderCell.ColumnSpan = 2;
        //    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
        //    HeaderGridRow.Cells.Add(HeaderCell);
        //    grdLibNorms.Controls[0].Controls.AddAt(0, HeaderGridRow);

        //    HeaderCell = new TableHeaderCell();
        //    HeaderCell.Text = "Extra";
        //    HeaderCell.ColumnSpan = 2;
        //    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
        //    HeaderGridRow.Cells.Add(HeaderCell);
        //    grdLibNorms.Controls[0].Controls.AddAt(0, HeaderGridRow);
        //    grdLibNorms.Controls[0].Controls.AddAt(0, HeaderGridRow);
        //}
    }

    protected void grdLibNorms_onpageindexchanged(object sender, GridViewPageEventArgs e)
    {
        grdLibNorms.PageIndex = e.NewPageIndex;
        btngoClick(sender, e);
    }

    protected void btngoClick(object sender, EventArgs e)
    {
        isSaveBtnClick = false;
        DataSet rsdept = new DataSet();
        DataSet rsNorms = new DataSet();
        DataSet rsVol = new DataSet();
        btn_save.Enabled = true;
        showreport2.Visible = true;
        grdLibNorms.Visible = true;
        string sqry = string.Empty;
        string Dept_Code = string.Empty;
        string LngNorm = string.Empty;
        string LngNormTit = string.Empty;
        string LngNo = string.Empty;
        string Lngtitle = string.Empty;
        string LngNatNorms = string.Empty;
        string LngIntNorms = string.Empty;
        string sql1 = string.Empty;
        string norms1 = string.Empty;
        string normstitle1 = string.Empty;
        int LngIntBal = 0;
        DataTable libbok = new DataTable();
        DataRow drbok;
        int LngNatBal = 0;
        int sno = 0;
        int norlng = 0;
        int normlongtit = 0;
        try
        {
            if (ddlLibrary.Items.Count > 0)
                library = Convert.ToString(ddlLibrary.SelectedValue);
            if (ddlinwardentry.Items.Count > 0)
                inward = Convert.ToString(ddlinwardentry.SelectedItem);
            if (ddldept.Items.Count > 0)
                dept = Convert.ToString(ddldept.SelectedValue);

            if (ddlinwardentry.SelectedIndex == 0)
            {
                string sql = "SELECT DISTINCT (ISNULL(Dept_Code,'')) Dept_Code FROM BookDetails WHERE Dept_Code <> '' ";
                if (library != "All")
                {
                    sql += " AND Lib_Code='" + library + "'";
                }
                if (dept != "All")
                {
                    sql += "AND Dept_Code ='" + dept + "'";
                }
                sql = sql + " UNION ";
                sql = sql + "SELECT DISTINCT (ISNULL(Dept_Name,'')) Dept_Code FROM Journal WHERE Dept_Name <> ''";

                if (library != "All")
                {
                    sql += " AND Lib_Code='" + library + "'";
                }
                if (dept != "All")
                {
                    sql += "AND Dept_Name ='" + dept + "'";
                }
                rsdept.Clear();
                rsdept = d2.select_method_wo_parameter(sql, "Text");
                if (rsdept.Tables.Count > 0 && rsdept.Tables[0].Rows.Count > 0)
                {
                    libbok.Columns.Add("Department");
                    libbok.Columns.Add("asperVolumes");
                    libbok.Columns.Add("asperTitle");
                    libbok.Columns.Add("AvaVolume");
                    libbok.Columns.Add("AvaTitle");
                    libbok.Columns.Add("needVolume");
                    libbok.Columns.Add("needtit");
                    libbok.Columns.Add("extVolume");
                    libbok.Columns.Add("exttit");
                    for (int row = 0; row < rsdept.Tables[0].Rows.Count; row++)
                    {
                        int lngbal = 0;
                        int LngBalTit = 0;
                        sno++;
                        drbok = libbok.NewRow();
                        string dep = Convert.ToString(rsdept.Tables[0].Rows[row]["Dept_Code"]).Trim();
                        drbok["Department"] = dep;

                        if (ddlLibrary.SelectedIndex != 0)
                        {
                            sqry = "SELECT ISNULL(Norms,'') Norms,ISNULL(Norms_Title,'') Norms_Title FROM Lib_Norms WHERE Dept ='" + dep + "' AND Lib_Code ='" + library + "' ";
                        }
                        else
                        {
                            sqry = "SELECT SUM(CAST(Norms as int)) Norms,SUM(cast(Norms_Title as int)) Norms_Title FROM Lib_Norms WHERE Dept ='" + dep + "' ";
                        }
                        rsNorms.Clear();
                        rsNorms = d2.select_method_wo_parameter(sqry, "Text");

                        if (rsNorms.Tables.Count != 0)
                        {
                            if (rsNorms.Tables.Count > 0 && rsNorms.Tables[0].Rows.Count > 0)
                            {
                                if (Convert.ToString(rsNorms.Tables[0].Rows[0]["Norms"]).Trim() != "")
                                {
                                    string norms = Convert.ToString(rsNorms.Tables[0].Rows[0]["Norms"]).Trim();
                                    LngNorm = norms;
                                    drbok["asperVolumes"] = Convert.ToString(LngNorm);
                                }
                                else
                                {
                                    LngNorm = "0";
                                }
                            }
                            else
                            {
                                LngNorm = "0";
                            }
                        }

                        if (rsNorms.Tables.Count != 0)
                        {
                            if (rsNorms.Tables.Count > 0 && rsNorms.Tables[0].Rows.Count > 0)
                            {
                                string normstitle = Convert.ToString(rsNorms.Tables[0].Rows[0]["Norms_Title"]).Trim();
                                LngNormTit = normstitle;
                                drbok["asperTitle"] = Convert.ToString(LngNormTit);
                            }
                            else
                            {
                                LngNormTit = "0";
                            }
                        }
                        else
                        {
                            drbok["asperTitle"] = "";
                            drbok["asperVolumes"] = "";
                            LngNorm = "0";
                            LngNormTit = "0";
                        }

                        sql1 = "SELECT Dept_Code,COUNT(*) TotVal,COUNT(DISTINCT Title) TotTit FROM BookDetails WHERE Dept_Code ='" + dep + "'";
                        if (library != "All")
                        {
                            sql1 += " AND Lib_Code ='" + library + "' ";
                        }
                        sql1 = sql1 + " GROUP BY Dept_Code ";
                        sql1 = sql1 + " ORDER BY Dept_Code ";

                        rsVol.Clear();
                        rsVol = d2.select_method_wo_parameter(sql1, "Text");
                        if (rsVol.Tables[0].Rows.Count > 0)
                        {
                            if (Convert.ToString(rsVol.Tables[0].Rows[0]["TotVal"]).Trim() != "")
                            {
                                norms1 = Convert.ToString(rsVol.Tables[0].Rows[0]["TotVal"]).Trim();
                                drbok["AvaVolume"] = Convert.ToString(norms1);
                                lngbal = Convert.ToInt32(LngNorm) - Convert.ToInt32(norms1);
                                //if (Convert.ToInt32(LngNorm) > 0)
                                //{
                                //    if (Convert.ToInt32(LngNorm) > Convert.ToInt32(norms1))
                                //        lngbal = Convert.ToInt32(LngNorm) - Convert.ToInt32(norms1);
                                //    else
                                //    {
                                //        lngbal = Convert.ToInt32(norms1) - Convert.ToInt32(LngNorm);
                                //        norms1 = Convert.ToString(lngbal);                                        
                                //    }
                                //}
                            }
                            if (Convert.ToString(rsVol.Tables[0].Rows[0]["TotTit"]).Trim() != "")
                            {
                                normstitle1 = Convert.ToString(rsVol.Tables[0].Rows[0]["TotTit"]).Trim();
                                drbok["AvaTitle"] = Convert.ToString(normstitle1);

                                int dd = 0;
                                int.TryParse(LngNormTit, out dd);
                                LngBalTit = Convert.ToInt32(dd) - Convert.ToInt32(normstitle1);
                                //if (dd > 0)
                                //{
                                //    if (Convert.ToInt32(LngNormTit) > Convert.ToInt32(normstitle1))
                                //        LngBalTit = Convert.ToInt32(LngNormTit) - Convert.ToInt32(normstitle1);
                                //    else
                                //    {
                                //        LngBalTit = Convert.ToInt32(normstitle1) - Convert.ToInt32(LngNormTit);
                                //        normstitle1 = Convert.ToString(LngBalTit);
                                //    }
                                //}
                            }
                        }

                        if (lngbal > 0)
                        {
                            drbok["needVolume"] = Convert.ToString(lngbal);
                        }
                        else
                        {
                            drbok["extVolume"] = Convert.ToString(norms1);
                        }
                        if (LngBalTit > 0)
                        {
                            drbok["needtit"] = Convert.ToString(LngBalTit);
                        }
                        else
                        {
                            drbok["exttit"] = Convert.ToString(normstitle1);
                        }

                        libbok.Rows.Add(drbok);
                    }
                }
                grdLibNorms.DataSource = libbok;
                grdLibNorms.DataBind();
                grdLibNorms.Visible = true;
                grdLibNormsPer.Visible = false;
                for (int l = 0; l < grdLibNorms.Rows.Count; l++)
                {
                    foreach (GridViewRow row in grdLibNorms.Rows)
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            grdLibNorms.Rows[l].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            grdLibNorms.Rows[l].Cells[4].HorizontalAlign = HorizontalAlign.Right;
                            grdLibNorms.Rows[l].Cells[5].HorizontalAlign = HorizontalAlign.Right;
                            grdLibNorms.Rows[l].Cells[6].HorizontalAlign = HorizontalAlign.Right;
                            grdLibNorms.Rows[l].Cells[7].HorizontalAlign = HorizontalAlign.Right;
                            grdLibNorms.Rows[l].Cells[8].HorizontalAlign = HorizontalAlign.Right;
                            grdLibNorms.Rows[l].Cells[9].HorizontalAlign = HorizontalAlign.Right;
                        }
                    }
                }
            }
            else if (ddlinwardentry.SelectedIndex == 1)
            {
                string sqlp = " SELECT DISTINCT (ISNULL(Dept_Code,'')) Dept_Code FROM BookDetails WHERE Dept_Code <> ''";
                if (dept != "All")
                {
                    sqlp = sqlp + "AND Dept_Code ='" + dept + "' ";
                }
                sqlp = sqlp + " UNION ";
                sqlp = sqlp + "   SELECT DISTINCT (ISNULL(Department,'')) Dept_Code FROM Journal_Master WHERE Department <> '' ";
                if (dept != "All")
                {
                    sqlp = sqlp + "AND Department ='" + dept + "' ";
                }
                rsdept.Clear();
                rsdept = d2.select_method_wo_parameter(sqlp, "Text");
                // loadspreadHeaderperiod(ds);
                if (rsdept.Tables.Count > 0 && rsdept.Tables[0].Rows.Count > 0)
                {
                    libbok.Columns.Add("Department");
                    libbok.Columns.Add("aspernation");
                    libbok.Columns.Add("asperinter");
                    libbok.Columns.Add("Avanational");
                    libbok.Columns.Add("Avainter");
                    libbok.Columns.Add("neednationl");
                    libbok.Columns.Add("needinter");
                    libbok.Columns.Add("extnationale");
                    libbok.Columns.Add("extinter");
                    for (int row = 0; row < rsdept.Tables[0].Rows.Count; row++)
                    {
                        sno++;
                        drbok = libbok.NewRow();
                        string dep = Convert.ToString(rsdept.Tables[0].Rows[row]["Dept_Code"]).Trim();
                        drbok["Department"] = dep;
                        if (ddlLibrary.SelectedIndex != 0)
                        {
                            sqlp = "SELECT ISNULL(National_Norms,'') NatNorms,ISNULL(International_Norms,'') IntNatNorms FROM Lib_Norms WHERE Dept ='" + dep + "' AND Lib_Code ='" + library + "' ";
                        }
                        else
                        {
                            sqlp = "SELECT  SUM( CONVERT(int,National_Norms) ) NatNorms,SUM( CONVERT(int,International_Norms) ) IntNatNorms FROM Lib_Norms WHERE Dept ='" + dep + "' ";
                        }
                        rsNorms.Clear();
                        rsNorms = d2.select_method_wo_parameter(sqlp, "Text");
                        if (rsNorms.Tables[0].Rows.Count > 0)
                        {
                            if (Convert.ToString(rsNorms.Tables[0].Rows[0]["NatNorms"]).Trim() != "0" && Convert.ToString(rsNorms.Tables[0].Rows[0]["NatNorms"]).Trim() != "")
                            {
                                string natnorms = Convert.ToString(rsNorms.Tables[0].Rows[0]["NatNorms"]).Trim();
                                drbok["aspernation"] = Convert.ToString(natnorms);
                                LngNatNorms = natnorms;
                            }
                            else
                            {
                                LngNatNorms = "0";
                            }
                            if (Convert.ToString(rsNorms.Tables[0].Rows[0]["IntNatNorms"]).Trim() != "0" && Convert.ToString(rsNorms.Tables[0].Rows[0]["IntNatNorms"]).Trim() != "")
                            {
                                string normstitle = Convert.ToString(rsNorms.Tables[0].Rows[0]["IntNatNorms"]).Trim();
                                drbok["asperinter"] = Convert.ToString(normstitle);
                                LngIntNorms = normstitle;
                            }
                            else
                            {
                                LngIntNorms = "0";
                            }
                        }
                        else
                        {
                            drbok["aspernation"] = "";
                            drbok["asperinter"] = "";
                            LngNatNorms = "0";
                            LngIntNorms = "0";
                        }
                        sql1 = "SELECT Department,COUNT(*) TotVal,COUNT(DISTINCT Journal_Name) TotTit FROM Journal_Master WHERE Department ='" + dep + "' AND Is_National = 1";
                        if (library != "All")
                        {
                            sql1 += " AND Lib_Code ='" + library + "' ";
                        }
                        sql1 = sql1 + " GROUP BY Department ";
                        sql1 = sql1 + " ORDER BY Department ";
                        rsVol.Clear();
                        rsVol = d2.select_method_wo_parameter(sql1, "Text");
                        if (rsVol.Tables[0].Rows.Count > 0)
                        {
                            norms1 = Convert.ToString(rsVol.Tables[0].Rows[0]["TotVal"]).Trim();
                            drbok["Avanational"] = Convert.ToString(norms1);
                            int value = Convert.ToInt32(rsVol.Tables[0].Rows[0]["TotVal"]);
                            LngNatBal = Convert.ToInt32(LngNatNorms) - value;
                            //if (LngNatBal > 0)
                            //{
                            //    if (Convert.ToInt32(LngNormTit) > Convert.ToInt32(normstitle1))
                            //        LngNatBal = Convert.ToInt32(LngNormTit) - Convert.ToInt32(normstitle1);
                            //    else
                            //        LngNatBal = Convert.ToInt32(normstitle1) - Convert.ToInt32(LngNormTit);
                            //}
                        }
                        else
                        {
                            drbok["Avanational"] = "";
                            LngNatBal = Convert.ToInt32(LngNatNorms) - 0;
                        }
                        if (library != "")
                        {
                            sql1 = "SELECT Department,COUNT(*) TotVal,COUNT(DISTINCT Journal_Name) TotTit FROM Journal_Master WHERE Department ='" + dep + "' AND Is_National = 0";
                            if (library != "All")
                            {
                                sql1 += " AND Lib_Code ='" + library + "' ";
                            }
                            sql1 = sql1 + " GROUP BY Department ";
                            sql1 = sql1 + " ORDER BY Department ";
                            rsVol.Clear();
                            rsVol = d2.select_method_wo_parameter(sql1, "Text");
                        }
                        if (rsVol.Tables[0].Rows.Count > 0)
                        {
                            norms1 = Convert.ToString(rsVol.Tables[0].Rows[0]["TotVal"]).Trim();
                            LngIntBal = Convert.ToInt32(LngIntNorms) - Convert.ToInt32(norms1);
                            drbok["Avainter"] = Convert.ToString(norms1);
                        }
                        else
                        {
                            drbok["Avainter"] = "";
                            int bb = 0;
                            int.TryParse(LngIntNorms, out bb);
                            LngIntBal = bb - 0;
                        }
                        if (LngNatBal > 0)
                        {
                            drbok["neednationl"] = Convert.ToString(LngNatBal);
                        }
                        else
                        {
                            string extraNat = Convert.ToString(LngNatBal);
                            if (extraNat.Length > 0)
                            {
                                string exNat = extraNat[0].ToString();
                                if (exNat == "-")
                                {
                                    extraNat = extraNat.Remove(0, 1);
                                }
                                LngNatBal = Convert.ToInt32(extraNat);
                            }

                            drbok["extnationale"] = Convert.ToString(LngNatBal);
                        }

                        if (LngIntBal > 0)
                        {
                            drbok["needinter"] = Convert.ToString(LngIntBal);
                        }
                        else
                        {
                            string extraNat = Convert.ToString(LngIntBal);
                            if (extraNat.Length > 0)
                            {
                                string exNat = extraNat[0].ToString();
                                if (exNat == "-")
                                {
                                    exNat = extraNat.Remove(0, 1);
                                }
                                LngIntBal = Convert.ToInt32(exNat);
                            }
                            drbok["extinter"] = Convert.ToString(LngIntBal);
                        }
                        libbok.Rows.Add(drbok);
                    }
                }
                grdLibNormsPer.DataSource = libbok;
                grdLibNormsPer.DataBind();
                grdLibNormsPer.Visible = true;
                grdLibNorms.Visible = false;
                print2.Visible = true;
                for (int l = 0; l < grdLibNormsPer.Rows.Count; l++)
                {
                    foreach (GridViewRow row in grdLibNormsPer.Rows)
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            grdLibNormsPer.Rows[l].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            grdLibNormsPer.Rows[l].Cells[2].HorizontalAlign = HorizontalAlign.Right;
                            grdLibNormsPer.Rows[l].Cells[3].HorizontalAlign = HorizontalAlign.Right;
                            grdLibNormsPer.Rows[l].Cells[4].HorizontalAlign = HorizontalAlign.Right;
                            grdLibNormsPer.Rows[l].Cells[5].HorizontalAlign = HorizontalAlign.Right;
                            grdLibNormsPer.Rows[l].Cells[6].HorizontalAlign = HorizontalAlign.Right;
                            grdLibNormsPer.Rows[l].Cells[7].HorizontalAlign = HorizontalAlign.Right;
                            grdLibNormsPer.Rows[l].Cells[8].HorizontalAlign = HorizontalAlign.Right;
                            grdLibNormsPer.Rows[l].Cells[9].HorizontalAlign = HorizontalAlign.Right;
                        }
                    }
                }
            }
        }

        catch { }

    }

    protected void grdLibNormsPer_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.Header)
        //{
        //    int tempt = Convert.ToInt32(ViewState["temp_table"]);
        //    GridView HeaderGrid = (GridView)sender;
        //    GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
        //    TableCell headerCell = new TableCell();
        //    Table table = (Table)grdLibNormsPer.Controls[0];
        //    TableRow headerRow = table.Rows[0];
        //    int numberOfHeaderCellsToMove = headerRow.Cells.Count - 1;

        //    for (int i = 0; i < 2; i++)
        //    {
        //        headerCell = headerRow.Cells[0];
        //        HeaderGridRow.Cells.Add(headerCell);
        //        headerCell.RowSpan = 2;
        //    }
        //    grdLibNormsPer.Controls[0].Controls.AddAt(0, HeaderGridRow);
        //    TableHeaderCell HeaderCell = new TableHeaderCell();

        //    HeaderCell = new TableHeaderCell();
        //    HeaderCell.Text = "As Per Norms";
        //    HeaderCell.ColumnSpan = 2;
        //    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
        //    HeaderGridRow.Cells.Add(HeaderCell);
        //    grdLibNormsPer.Controls[0].Controls.AddAt(0, HeaderGridRow);

        //    HeaderCell = new TableHeaderCell();
        //    HeaderCell.Text = "Available";
        //    HeaderCell.ColumnSpan = 2;
        //    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
        //    HeaderGridRow.Cells.Add(HeaderCell);
        //    grdLibNormsPer.Controls[0].Controls.AddAt(0, HeaderGridRow);

        //    HeaderCell = new TableHeaderCell();
        //    HeaderCell.Text = "Needed";
        //    HeaderCell.ColumnSpan = 2;
        //    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
        //    HeaderGridRow.Cells.Add(HeaderCell);
        //    grdLibNormsPer.Controls[0].Controls.AddAt(0, HeaderGridRow);

        //    HeaderCell = new TableHeaderCell();
        //    HeaderCell.Text = "Extra";
        //    HeaderCell.ColumnSpan = 2;
        //    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
        //    HeaderGridRow.Cells.Add(HeaderCell);
        //    grdLibNormsPer.Controls[0].Controls.AddAt(0, HeaderGridRow);
        //    grdLibNormsPer.Controls[0].Controls.AddAt(0, HeaderGridRow);
        //}
    }

    #region alertclose

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        try
        {
            lblalerterr.Text = string.Empty;
            alertpopwindow.Visible = false;
            btngoClick(sender, e);
        }
        catch (Exception ex) { }
        {
        }
    }

    #endregion

    #region Print

    protected void btnprintmaster_Click2(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "LibraryNorms";
            string pagename = "librarynorm.aspx";
            string ss = Session["usercode"].ToString();
            if (ddlinwardentry.SelectedIndex == 0)
            {
                Printcontrolhed2.loadspreaddetails(grdLibNorms, pagename, degreedetails, 0, ss, "1");
                Printcontrolhed2.Visible = true;
            }
            if (ddlinwardentry.SelectedIndex == 1)
            {
                Printcontrolhed2.loadspreaddetails(grdLibNormsPer, pagename, degreedetails, 0, ss, "1");
                Printcontrolhed2.Visible = true;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "LibraryNorms"); }


    }

    protected void btnExcel_Click2(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname2.Text;
            if (reportname.ToString().Trim() != "")
            {
                if (ddlinwardentry.SelectedIndex == 0)
                {
                    d2.printexcelreportgrid(grdLibNorms, reportname);
                }
                if (ddlinwardentry.SelectedIndex == 1)
                {
                    d2.printexcelreportgrid(grdLibNormsPer, reportname);
                }
                lblvalidation3.Visible = false;
            }
            else
            {
                lblvalidation3.Text = "Please Enter Your Report Name";
                lblvalidation3.Visible = true;
                txtexcelname2.Focus();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, userCollegeCode, "LibraryNorms"); }


    }
    #endregion

}