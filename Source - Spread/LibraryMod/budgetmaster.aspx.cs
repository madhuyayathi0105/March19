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
using InsproDataAccess;

public partial class LibraryMod_budgetmaster : System.Web.UI.Page
{
    DAccess2 da = new DAccess2();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DAccess2 obi_access = new DAccess2();
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
    Boolean Cellclick = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        ddlbudgethead.Attributes.Add("onfocus", "frelig()");
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
                Bindhead();
                getLibPrivil();
                rptprint.Visible = false;
                Binddeopt();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "budgetmaster");
        }

    }

    public void Bindcollege()
    {
        try
        {
            ddlcollege.Items.Clear();
            dtCommon.Clear();
            ddlcollege.Enabled = false;
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
                ddlcollege.DataSource = dtCommon;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
                ddlcollege.SelectedIndex = 0;
                ddlcollege.Enabled = true;
                ddlcollege.Items.Insert(0, "All");
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "budgetmaster");
        }
    }

    public void Bindhead()
    {
        try
        {
            string typ = string.Empty;
            if (ddlcollege.Items.Count > 0)
            {
                for (int i = 0; i < ddlcollege.Items.Count - 1; i++)
                {
                    if (Convert.ToString(ddlcollege.SelectedItem) == "All")
                    {
                        if (typ == "")
                        {
                            typ = "" + ddlcollege.Items[i + 1].Value + "";
                        }
                        else
                        {
                            typ = typ + "'" + "," + "'" + ddlcollege.Items[i + 1].Value + "";
                        }
                    }
                    else
                        typ = ddlcollege.SelectedValue;
                }
            }
            string hed = "Select TextVal,textcode from textvaltable where college_code=" + typ + "and TextCriteria='LBHed' ";
            DataSet ds2 = d2.select_method_wo_parameter(hed, "Text");
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                ddlhead.DataSource = ds2;
                ddlhead.DataTextField = "TextVal";
                ddlhead.DataValueField = "textcode";
                ddlhead.DataBind();
                ddlhead.Items.Insert(0, "All");
                ddlbudgethead.DataSource = ds2;
                ddlbudgethead.DataTextField = "TextVal";
                ddlbudgethead.DataValueField = "textcode";
                ddlbudgethead.DataBind();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "budgetmaster");
        }
    }

    public void Binddeopt()
    {
        try
        {
            string typ = string.Empty;
            if (ddlcollege.Items.Count > 0)
            {
                for (int i = 0; i < ddlcollege.Items.Count - 1; i++)
                {
                    if (Convert.ToString(ddlcollege.SelectedItem) == "All")
                    {
                        if (typ == "")
                        {
                            typ = "" + ddlcollege.Items[i + 1].Value + "";
                        }
                        else
                        {
                            typ = typ + "'" + "," + "'" + ddlcollege.Items[i + 1].Value + "";
                        }
                    }
                    else
                        typ = ddlcollege.SelectedValue;
                }
            }
            string hed = "SELECT Distinct Dept_Name FROM Journal_Dept WHERE college_code=" + typ + " AND Lib_Code ='" + Convert.ToString(ddllib.SelectedValue) + "' ORDER BY Dept_Name ";
            DataSet ds2 = d2.select_method_wo_parameter(hed, "Text");
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                ddldepartment.DataSource = ds2;
                ddldepartment.DataTextField = "Dept_Name";
                ddldepartment.DataValueField = "Dept_Name";
                ddldepartment.DataBind();
                ddldepartment.Items.Insert(0, "All");
                ddlbudgetdept.DataSource = ds2;
                ddlbudgetdept.DataTextField = "Dept_Name";
                ddlbudgetdept.DataValueField = "Dept_Name";
                ddlbudgetdept.DataBind();
                ddlbudgetdept.Items.Insert(0, "");
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "budgetmaster");
        }
    }

    public void getLibPrivil()
    {
        try
        {
            string libcodecollection = "";
            string coll_Code = Convert.ToString(ddlcollege.SelectedValue);
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
            Bindlib(LibCollection);
        }
        catch (Exception ex)
        {
        }
    }

    public void Bindlib(string libcode)
    {
        try
        {
            string typ = string.Empty;
            string college = Convert.ToString(ddlcollege.SelectedValue);
            string lib_name = "select lib_code,lib_name,CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1) from library " + libcode + " AND college_code=" + college + " ORDER BY CAST(RIGHT(lib_code, LEN(lib_code) - PATINDEX('%[0-9]%', lib_code)+1) AS INT), LEFT(lib_code, PATINDEX('%[0-9]%', lib_code)-1)";
            ds = da.select_method_wo_parameter(lib_name, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddllib.DataSource = ds;
                ddllib.DataTextField = "Lib_Name";
                ddllib.DataValueField = "Lib_Code";
                ddllib.DataBind();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "budgetmaster");
        }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        getLibPrivil();
        Binddeopt();
        Bindhead();
    }

    protected void ddldepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void ddlhead_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void ddllib_SelectedIndexChanged(object sender, EventArgs e)
    {
        Binddeopt();
    }

    #region Go Events

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            string sql = string.Empty;
            sql = " SELECT BudgetCode,Budget_From,Budget_To,TextVal,Dept_Name,TotBudAmt,TotSpendAmt,TotBalAmt,BBudAmt,BSpendAmt,BBalAmt,JBudAmt,JSpendAmt,JBalAmt,NBudAmt,NSpendAmt,NBalAmt,Remarks";
            sql = sql + " FROM LibBudgetMaster B";
            sql = sql + " INNER JOIN TextValTable T ON T.TextCode = B.Head_Code";
            sql = sql + " Where 1 = 1 ";
            if (Chkaccno.Checked == true)
            {
                string Budj_fromDt = txt_fromdate.Text;
                string Budj_toDt = txt_todate.Text;
                string[] fromdate = Budj_fromDt.Split('/');
                string[] todate = Budj_toDt.Split('/');
                if (fromdate.Length == 3)
                    Budj_fromDt = fromdate[1].ToString() + "/" + fromdate[0].ToString() + "/" + fromdate[2].ToString();
                if (todate.Length == 3)
                    Budj_toDt = todate[1].ToString() + "/" + todate[0].ToString() + "/" + todate[2].ToString();
                sql = sql + " AND (Budget_From Between '" + Budj_fromDt + "' AND '" + Budj_toDt + "'";
                sql = sql + " OR Budget_To Between '" + Budj_fromDt + "' AND '" + Budj_toDt + "')";
            }
            string typ = string.Empty;
            if (ddldepartment.Items.Count > 0)
            {
                for (int i = 0; i < ddldepartment.Items.Count - 1; i++)
                {
                    if (Convert.ToString(ddldepartment.SelectedItem) == "All")
                    {
                        if (typ == "")
                        {
                            typ = "" + ddldepartment.Items[i + 1].Value + "";
                        }
                        else
                        {
                            typ = typ + "'" + "," + "'" + ddldepartment.Items[i + 1].Value + "";
                        }
                    }
                    else
                        typ = ddldepartment.SelectedValue;
                }
            }
            sql = sql + " AND Dept_Name in('" + typ + "')";

            string typs = string.Empty;
            if (ddlhead.Items.Count > 0)
            {
                for (int i = 0; i < ddlhead.Items.Count - 1; i++)
                {
                    if (Convert.ToString(ddlhead.SelectedItem) == "All")
                    {
                        if (typs == "")
                        {
                            typs = "" + ddlhead.Items[i + 1].Text + "";
                        }
                        else
                        {
                            typs = typs + "'" + "," + "'" + ddlhead.Items[i + 1].Text + "";
                        }
                    }
                    else
                        typs = ddlhead.SelectedItem.Text;
                }
            }
            sql = sql + " AND TextVal in('" + typs + "')";
            sql = sql + "Order By Dept_Name";
            DataSet bookallo = d2.select_method_wo_parameter(sql, "Text");
            if (bookallo.Tables.Count > 0 && bookallo.Tables[0].Rows.Count > 0)
            {
                DataTable dtBudjMaster = new DataTable();
                DataRow drow;

                dtBudjMaster.Columns.Add("BudgetCode", typeof(string));
                dtBudjMaster.Columns.Add("From Date", typeof(string));
                dtBudjMaster.Columns.Add("To Date", typeof(string));
                dtBudjMaster.Columns.Add("Head", typeof(string));
                dtBudjMaster.Columns.Add("Department", typeof(string));
                dtBudjMaster.Columns.Add("Budget Amt", typeof(string));
                dtBudjMaster.Columns.Add("Spend Amt", typeof(string));
                dtBudjMaster.Columns.Add("Balance Amt", typeof(string));
                dtBudjMaster.Columns.Add("Book Budget", typeof(string));
                dtBudjMaster.Columns.Add("Book Spend", typeof(string));
                dtBudjMaster.Columns.Add("Book Balance", typeof(string));
                dtBudjMaster.Columns.Add("Journal Budget", typeof(string));
                dtBudjMaster.Columns.Add("Journal Spend", typeof(string));
                dtBudjMaster.Columns.Add("Journal Balance", typeof(string));
                dtBudjMaster.Columns.Add("NonBook Budget", typeof(string));
                dtBudjMaster.Columns.Add("NonBook Spend", typeof(string));
                dtBudjMaster.Columns.Add("NonBook Balance", typeof(string));
                dtBudjMaster.Columns.Add("Remark", typeof(string));

                for (int i = 0; i < bookallo.Tables[0].Rows.Count; i++)
                {
                    string Budj_fromDt = Convert.ToString(bookallo.Tables[0].Rows[i]["Budget_From"]);
                    string Budj_toDt = Convert.ToString(bookallo.Tables[0].Rows[i]["Budget_To"]);
                    string[] fromdate = Budj_fromDt.Split('/');
                    string[] todate = Budj_toDt.Split('/');
                    if (fromdate.Length == 3)
                        Budj_fromDt = fromdate[1].ToString() + "/" + fromdate[0].ToString() + "/" + fromdate[2].ToString();
                    if (todate.Length == 3)
                        Budj_toDt = todate[1].ToString() + "/" + todate[0].ToString() + "/" + todate[2].ToString();

                    drow = dtBudjMaster.NewRow();
                    drow["BudgetCode"] = Convert.ToString(bookallo.Tables[0].Rows[i]["BudgetCode"]);
                    drow["From Date"] = Budj_fromDt.Split(' ')[0];
                    drow["To Date"] = Budj_toDt.Split(' ')[0];
                    drow["Head"] = Convert.ToString(bookallo.Tables[0].Rows[i]["TextVal"]);
                    drow["Department"] = Convert.ToString(bookallo.Tables[0].Rows[i]["Dept_Name"]);
                    drow["Budget Amt"] = Convert.ToString(bookallo.Tables[0].Rows[i]["TotBudAmt"]);
                    drow["Spend Amt"] = Convert.ToString(bookallo.Tables[0].Rows[i]["TotSpendAmt"]);
                    drow["Balance Amt"] = Convert.ToString(bookallo.Tables[0].Rows[i]["TotBalAmt"]);
                    drow["Book Budget"] = Convert.ToString(bookallo.Tables[0].Rows[i]["BBudAmt"]);
                    drow["Book Spend"] = Convert.ToString(bookallo.Tables[0].Rows[i]["BSpendAmt"]);
                    drow["Book Balance"] = Convert.ToString(bookallo.Tables[0].Rows[i]["BBalAmt"]);
                    drow["Journal Budget"] = Convert.ToString(bookallo.Tables[0].Rows[i]["JBudAmt"]);
                    drow["Journal Spend"] = Convert.ToString(bookallo.Tables[0].Rows[i]["JSpendAmt"]);
                    drow["Journal Balance"] = Convert.ToString(bookallo.Tables[0].Rows[i]["JBalAmt"]);
                    drow["NonBook Budget"] = Convert.ToString(bookallo.Tables[0].Rows[i]["NBudAmt"]);
                    drow["NonBook Spend"] = Convert.ToString(bookallo.Tables[0].Rows[i]["NSpendAmt"]);
                    drow["NonBook Balance"] = Convert.ToString(bookallo.Tables[0].Rows[i]["NBalAmt"]);
                    drow["Remark"] = Convert.ToString(bookallo.Tables[0].Rows[i]["Remarks"]);
                    dtBudjMaster.Rows.Add(drow);
                }
                Grdbudget.DataSource = dtBudjMaster;
                Grdbudget.DataBind();
                Grdbudget.Visible = true;
                rptprint.Visible = true;
            }
            else
            {
                Grdbudget.Visible = false;
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No record found";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "budgetmaster");
        }
    }

    protected void Grdbudget_OnPageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        Grdbudget.PageIndex = e.NewPageIndex;
        btngo_Click(sender, e);
    }

    protected void Grdbudget_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[1].Visible = false;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[1].Visible = false;
        }
    }

    protected void Grdbudget_OnRowCreated(object sender, GridViewRowEventArgs e)
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

    protected void Grdbudget_SelectedIndexChanged(Object sender, EventArgs e)
    {
        var grid = (GridView)sender;
        GridViewRow selectedRow = grid.SelectedRow;
        int rowIndex = grid.SelectedIndex;
        int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);


        string sql = string.Empty;
        if (Convert.ToString(rowIndex) != "" && Convert.ToString(selectedCellIndex) != "1")
        {
            string bud_code = Convert.ToString(Grdbudget.Rows[rowIndex].Cells[1].Text);//Convert.ToString(spreadbudget.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
            sql = " SELECT BudgetCode,Budget_From,Budget_To,TextVal,Dept_Name,TotBudAmt,TotSpendAmt,TotBalAmt,BBudAmt,BSpendAmt,BBalAmt,JBudAmt,JSpendAmt,JBalAmt,NBudAmt,NSpendAmt,NBalAmt,Remarks";
            sql = sql + " FROM LibBudgetMaster B";
            sql = sql + " INNER JOIN TextValTable T ON T.TextCode = B.Head_Code";
            sql = sql + " Where 1 = 1 ";
            sql = sql + " AND BudgetCode ='" + bud_code + "'";
            DataSet bookallo = d2.select_method_wo_parameter(sql, "Text");
            if (bookallo.Tables.Count > 0 && bookallo.Tables[0].Rows.Count > 0)
            {
                Binddeopt();
                Bindhead();
                for (int i = 0; i < bookallo.Tables[0].Rows.Count; i++)
                {
                    txtbudget.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["BudgetCode"]);
                    ddlbudgethead.Items.Insert(0, Convert.ToString(bookallo.Tables[0].Rows[0]["TextVal"]));
                    ddlbudgetdept.Items.Insert(0, Convert.ToString(bookallo.Tables[0].Rows[0]["Dept_Name"]));
                    Txtbudgetamt.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["TotBudAmt"]);
                    Txtamt.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["TotSpendAmt"]);
                    Txtbal.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["TotBalAmt"]);
                    Txtbooks1.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["BBudAmt"]);
                    Txtbook2.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["BSpendAmt"]);
                    Txtbook3.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["BBalAmt"]);
                    Txtjou1.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["JBudAmt"]);
                    Txtjou2.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["JSpendAmt"]);
                    Txtjou3.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["JBalAmt"]);
                    Txtnobok1.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["NBudAmt"]);
                    Txtnobok2.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["NSpendAmt"]);
                    Txtnobok3.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["NBalAmt"]);
                    Txtremarks.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["Remarks"]);
                    ddlbudgetdept.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["Dept_Name"]);
                    ddlbudgethead.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["TextVal"]);
                    txt_fromdatebudget.Text = Convert.ToDateTime(bookallo.Tables[0].Rows[0]["Budget_From"]).ToString("dd/MM/yyyy");
                    txt_todatebudget.Text = Convert.ToDateTime(bookallo.Tables[0].Rows[0]["Budget_To"]).ToString("dd/MM/yyyy");

                    ddlbudgetdept.Enabled = false;
                    ddlbudgethead.Enabled = false;
                    txtbudget.Enabled = true;
                    txt_fromdatebudget.Enabled = true;
                    txt_todatebudget.Enabled = true;
                    Txtbudgetamt.Enabled = true;
                    Txtamt.Enabled = true;
                    Txtbal.Enabled = true;
                    Txtbooks1.Enabled = true;
                    Txtbook2.Enabled = true;
                    Txtbook3.Enabled = true;
                    Txtjou1.Enabled = true;
                    Txtjou2.Enabled = true;
                    Txtjou3.Enabled = true;
                    Txtnobok1.Enabled = true;
                    Txtnobok2.Enabled = true;
                    Txtnobok3.Enabled = true;
                    txt_fromdatebudget.Enabled = false;
                    txt_todatebudget.Enabled = false;
                    btnsavebud.Visible = false;
                    Btnup.Visible = true;
                    Btndele.Visible = true;
                }
            }
            divPopAlertbudget.Visible = true;
            //btnadd_Click(sender, e);
            string a = Txtnobok1.Text;
        }
    }

    #endregion

    protected void btnadd_Click(object sender, EventArgs e)
    {
        try
        {
            if (Cellclick != true)
            {
                Addclear();
                Binddeopt();
                txt_fromdatebudget.Enabled = true;
                txt_todatebudget.Enabled = true;
                ddlbudgetdept.Enabled = true;
                ddlbudgethead.Enabled = true;
                btnsavebud.Visible = true;
                Btnup.Visible = false;
                Btndele.Visible = false;
            }
            else
            {
                btnsavebud.Visible = false;
                Btnup.Visible = true;
                Btndele.Visible = true;
            }
            divPopAlertbudget.Visible = true;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "budgetmaster");
        }

    }

    protected void Addclear()
    {
        try
        {
            int code = 0;
            string sqll2 = "SELECT MAX(cast(BudgetCode AS numeric)) As value FROM LibBudgetMaster";
            DataSet dss2 = new DataSet();
            dss2 = obi_access.select_method_wo_parameter(sqll2, "Text");
            if (dss2.Tables[0].Rows.Count > 0)
            {
                string Strcode = Convert.ToString(dss2.Tables[0].Rows[0][0]);
                if (!string.IsNullOrEmpty(Strcode))
                    code = Convert.ToInt32(Strcode) + 1;
                else
                    code = 1;
            }
            else
                code = 1;
            txtbudget.Text = Convert.ToString(code);
            txt_fromdatebudget.Text = "";
            txt_todatebudget.Text = "";
            Txtbudgetamt.Text = "";
            Txtamt.Text = "";
            Txtbal.Text = "";
            Txtbooks1.Text = "";
            Txtbook2.Text = "";
            Txtbook3.Text = "";
            Txtjou1.Text = "";
            Txtjou2.Text = "";
            Txtjou3.Text = "";
            Txtnobok1.Text = "";
            Txtnobok2.Text = "";
            Txtnobok3.Text = "";
            Txtremarks.Text = "";
            //txtbudget.Text = "";
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "budgetmaster");
        }
    }

    protected void ddlbudgethead_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            btnaddbudget.Visible = true;
            btnsubbudget.Visible = true;
        }
        catch
        {
        }

    }

    protected void Chkaccno_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (Chkaccno.Checked == true)
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
        {
            d2.sendErrorMail(ex, userCollegeCode, "budgetmaster");
        }
    }

    protected void btnaddbudget_Click(object sender, EventArgs e)
    {
        try
        {
            Div1.Visible = true;
            Div2.Visible = true;
            txt_infra.Text = "";
            txt_infra.Focus();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "budgetmaster");
        }

    }

    protected void btnsubbudget_Click(object sender, EventArgs e)
    {
        try
        {
            string txt = Convert.ToString(ddlbudgethead.SelectedItem);
            string sql = string.Empty;
            DataSet iss = new DataSet();
            if (txt != "")
            {
                sql = "Select * from textvaltable where college_code=" + Convert.ToString(ddlcollege.SelectedValue) + "and TextCriteria='LBHed' and TextVal='" + txt + "'";
                iss = d2.select_method_wo_parameter(sql, "text");
                if (iss.Tables.Count > 0 && iss.Tables[0].Rows.Count > 0)
                {
                    sql = "delete from textvaltable where TextVal='" + txt + "' and college_code=" + Convert.ToString(ddlcollege.SelectedValue) + "and TextCriteria='LBHed'";
                    int up = d2.update_method_wo_parameter(sql, "Text");
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Enter Reason";
            }
            Bindhead();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "budgetmaster");
        }
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            string txt = txt_infra.Text;
            string sql = string.Empty;
            if (txt != "")
            {
                sql = "Select * from textvaltable where college_code='" + Convert.ToString(ddlcollege.SelectedValue) + "' and TextCriteria='LBHed' and TextVal='" + txt + "'";
                DataSet iss = d2.select_method_wo_parameter(sql, "text");
                if (iss.Tables.Count > 0 && iss.Tables[0].Rows.Count <= 0)
                {
                    //sql = "update textvaltable set TextVal='" + txt + "' where college_code=" + Convert.ToString(ddlcollege.SelectedValue) + "and TextCriteria='LBHed'";
                    sql = "insert into textvaltable(TextVal,TextCriteria,college_code) values('" + txt + "','LBHed'," + ddlcollege.SelectedValue + ")";
                    int up = d2.update_method_wo_parameter(sql, "Text");
                    btnexit_Click(sender, e);
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Enter Reason";
            }
            Bindhead();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "budgetmaster");
        }

    }

    protected void btnexit_Click(object sender, EventArgs e)
    {
        try
        {
            Div1.Visible = false;
            Div2.Visible = false;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "budgetmaster");
        }

    }

    protected void ddlbudgetdept_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void btnsavebud_Click(object sender, EventArgs e)
    {
        try
        {
            string sql = string.Empty;
            double txt = 0.00;
            Txtbudgetamt.Text = Hidden1.Value;
            Txtbal.Text = Hidden2.Value;
            Txtbook3.Text = Hidden3.Value;
            Txtjou3.Text = Hidden4.Value;
            Txtnobok3.Text = Hidden5.Value;
            if (Txtbudgetamt.Text == "")
                Txtbudgetamt.Text = Convert.ToString(txt);
            if (Txtamt.Text == "")
                Txtamt.Text = Convert.ToString(txt);
            if (Txtbal.Text == "")
                Txtbal.Text = Convert.ToString(txt);
            if (Txtbooks1.Text == "")
                Txtbooks1.Text = Convert.ToString(txt);
            if (Txtbook2.Text == "")
                Txtbook2.Text = Convert.ToString(txt);
            if (Txtbook3.Text == "")
                Txtbook3.Text = Convert.ToString(txt);

            if (Txtjou1.Text == "")
                Txtjou1.Text = Convert.ToString(txt);
            if (Txtjou2.Text == "")
                Txtjou2.Text = Convert.ToString(txt);
            if (Txtjou3.Text == "")
                Txtjou3.Text = Convert.ToString(txt);
            if (Txtnobok1.Text == "")
                Txtnobok1.Text = Convert.ToString(txt);
            if (Txtnobok2.Text == "")
                Txtnobok2.Text = Convert.ToString(txt);
            if (Txtnobok3.Text == "")
                Txtnobok3.Text = Convert.ToString(txt);

            string firstdate = Convert.ToString(txt_fromdatebudget.Text);
            string[] split = firstdate.Split('/');
            firstdate = Convert.ToString(split[1] + "/" + split[0] + "/" + split[2]);

            string lastdate = Convert.ToString(txt_todatebudget.Text);
            string[] split1 = lastdate.Split('/');
            lastdate = Convert.ToString(split1[1] + "/" + split1[0] + "/" + split1[2]);

            sql = "INSERT INTO LibBudgetMaster(Budget_From,Budget_To,Head_Code,Head,Dept_Name,TotBudAmt,TotSpendAmt,TotBalAmt,BBudAmt,BSpendAmt,BBalAmt,JBudAmt,JSpendAmt,JBalAmt,NBudAmt,NSpendAmt,NBalAmt,Remarks,College_Code)";
            sql = sql + " VALUES('" + firstdate + "','" + lastdate + "',";
            sql = sql + " '" + Convert.ToString(ddlbudgethead.SelectedValue) + "','" + Convert.ToString(ddlbudgethead.SelectedItem) + "', '" + Convert.ToString(ddlbudgetdept.SelectedItem) + "',";
            sql = sql + "'" + Txtbudgetamt.Text + "','" + Txtamt.Text + "','" + Txtbal.Text + "',";
            sql = sql + "'" + Txtbooks1.Text + "','" + Txtbook2.Text + "','" + Txtbook3.Text + "',";
            sql = sql + " '" + Txtjou1.Text + "','" + Txtjou2.Text + "','" + Txtjou3.Text + "',";
            sql = sql + "'" + Txtnobok1.Text + "','" + Txtnobok2.Text + "','" + Txtnobok3.Text + "',";
            sql = sql + "'" + Txtremarks.Text + "','" + Convert.ToString(ddlcollege.SelectedValue) + "')";
            int up = d2.update_method_wo_parameter(sql, "Text");
            if (up != 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Budget Details has been Saved Successfully";
                Binddeopt();
                Bindhead();
                txtbudget.Enabled = true;
                Txtbudgetamt.Enabled = false;
                Txtamt.Enabled = false;
                Txtbal.Enabled = false;
                txtbudget.Text = "";
                Txtbudgetamt.Text = "";
                Txtamt.Text = "";
                Txtbal.Text = "";
                Txtbooks1.Text = "";
                Txtbook2.Text = "";
                Txtbook3.Text = "";
                Txtjou1.Text = "";
                Txtjou2.Text = "";
                Txtjou3.Text = "";
                Txtnobok1.Text = "";
                Txtnobok2.Text = "";
                Txtnobok3.Text = "";
                Txtremarks.Text = "";
                txt_fromdatebudget.Text = "";
                txt_todatebudget.Text = "";
                txt_fromdatebudget.Enabled = true;
                txt_todatebudget.Enabled = true;
                ddlbudgetdept.Enabled = true;
                ddlbudgethead.Enabled = true;
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Record Found";
            }
            btngo_Click(sender, e);
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "budgetmaster");
        }
    }

    protected void Btnup_Click(object sender, EventArgs e)
    {
        try
        {
            string sql = string.Empty;
            double txt = 0.00;
            Txtbudgetamt.Text = Hidden1.Value;
            Txtbal.Text = Hidden2.Value;
            Txtbook3.Text = Hidden3.Value;
            Txtjou3.Text = Hidden4.Value;
            Txtnobok3.Text = Hidden5.Value;
            if (Txtbudgetamt.Text == "")
                Txtbudgetamt.Text = Convert.ToString(txt);
            if (Txtamt.Text == "")
                Txtamt.Text = Convert.ToString(txt);
            if (Txtbal.Text == "")
                Txtbal.Text = Convert.ToString(txt);
            if (Txtbooks1.Text == "")
                Txtbooks1.Text = Convert.ToString(txt);
            if (Txtbook2.Text == "")
                Txtbook2.Text = Convert.ToString(txt);
            if (Txtbook3.Text == "")
                Txtbook3.Text = Convert.ToString(txt);

            if (Txtjou1.Text == "")
                Txtjou1.Text = Convert.ToString(txt);
            if (Txtjou2.Text == "")
                Txtjou2.Text = Convert.ToString(txt);
            if (Txtjou3.Text == "")
                Txtjou3.Text = Convert.ToString(txt);
            if (Txtnobok1.Text == "")
                Txtnobok1.Text = Convert.ToString(txt);
            if (Txtnobok2.Text == "")
                Txtnobok2.Text = Convert.ToString(txt);
            if (Txtnobok3.Text == "")
                Txtnobok3.Text = Convert.ToString(txt);
            sql = "UPDATE LibBudgetMaster SET TotBudAmt ='" + Txtbudgetamt.Text + "',TotSpendAmt ='" + Txtamt.Text + "',TotBalAmt='" + Txtbal.Text + "',";
            sql = sql + " BBudAmt='" + Txtbooks1.Text + "',BSpendAmt ='" + Txtbook2.Text + "',BBalAmt='" + Txtbook3.Text + "',";
            sql = sql + " JBudAmt='" + Txtjou1.Text + "',JSpendAmt='" + Txtjou2.Text + "' ,JBalAmt='" + Txtjou3.Text + "',";
            sql = sql + " NBudAmt='" + Txtnobok1.Text + "',NSpendAmt='" + Txtnobok2.Text + "',NBalAmt='" + Txtnobok3.Text + "',";
            sql = sql + " Remarks='" + Txtremarks.Text + "'";
            sql = sql + " WHERE BudgetCode ='" + txtbudget.Text + "'";
            sql = sql + " AND College_Code ='" + Convert.ToString(ddlcollege.SelectedValue) + "'";
            int up = d2.update_method_wo_parameter(sql, "Text");
            if (up != 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Budget Details has been Updated Successfully";
                Binddeopt();
                Bindhead();
                txtbudget.Enabled = true;
                Txtbudgetamt.Enabled = false;
                Txtamt.Enabled = false;
                Txtbal.Enabled = false;
                txtbudget.Text = "";
                Txtbudgetamt.Text = "";
                Txtamt.Text = "";
                Txtbal.Text = "";
                Txtbooks1.Text = "";
                Txtbook2.Text = "";
                Txtbook3.Text = "";
                Txtjou1.Text = "";
                Txtjou2.Text = "";
                Txtjou3.Text = "";
                Txtnobok1.Text = "";
                Txtnobok2.Text = "";
                Txtnobok3.Text = "";
                Txtremarks.Text = "";
                txt_fromdatebudget.Text = "";
                txt_todatebudget.Text = "";
                txt_fromdatebudget.Enabled = true;
                txt_todatebudget.Enabled = true;
                ddlbudgetdept.Enabled = true;
                ddlbudgethead.Enabled = true;
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Record Found";
            }
            btngo_Click(sender, e);
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "budgetmaster");
        }
    }

    protected void Btndele_Click(object sender, EventArgs e)
    {
        try
        {
            string sql = string.Empty;
            sql = "Delete from LibBudgetMaster Where BudgetCode ='" + txtbudget.Text + "'";
            int up = d2.update_method_wo_parameter(sql, "Text");
            if (up != 0)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Budget Details has been deleted Successfully";
                Binddeopt();
                Bindhead();
                txtbudget.Enabled = true;
                Txtbudgetamt.Enabled = false;
                Txtamt.Enabled = false;
                Txtbal.Enabled = false;
                txtbudget.Text = "";
                Txtbudgetamt.Text = "";
                Txtamt.Text = "";
                Txtbal.Text = "";
                Txtbooks1.Text = "";
                Txtbook2.Text = "";
                Txtbook3.Text = "";
                Txtjou1.Text = "";
                Txtjou2.Text = "";
                Txtjou3.Text = "";
                Txtnobok1.Text = "";
                Txtnobok2.Text = "";
                Txtnobok3.Text = "";
                Txtremarks.Text = "";
                txt_fromdatebudget.Text = "";
                txt_todatebudget.Text = "";
                txt_fromdatebudget.Enabled = true;
                txt_todatebudget.Enabled = true;
                ddlbudgetdept.Enabled = true;
                ddlbudgethead.Enabled = true;
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Record Found";
            }
            btngo_Click(sender, e);
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "budgetmaster");
        }
    }

    protected void btnclosebud_Click(object sender, EventArgs e)
    {

        divPopAlertbudget.Visible = false;
    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }

    //protected void spreadbudget_CellClick(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        Cellclick = true;
    //    }
    //    catch (Exception ex)
    //    {
    //        d2.sendErrorMail(ex, userCollegeCode, "budgetmaster");
    //    }
    //}

    //protected void spreadbudget_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (Cellclick == true)
    //        {
    //            btnsavebud.Enabled = false;
    //            string sql = string.Empty;
    //            string activerow = "";//spreadbudget.ActiveSheetView.ActiveRow.ToString();
    //            string activecol = "";// spreadbudget.ActiveSheetView.ActiveColumn.ToString();
    //            if (activerow != "-1" && activerow != "")
    //            {
    //                //string bud_code = Convert.ToString(spreadbudget.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
    //                sql = " SELECT BudgetCode,Budget_From,Budget_To,TextVal,Dept_Name,TotBudAmt,TotSpendAmt,TotBalAmt,BBudAmt,BSpendAmt,BBalAmt,JBudAmt,JSpendAmt,JBalAmt,NBudAmt,NSpendAmt,NBalAmt,Remarks";
    //                sql = sql + " FROM LibBudgetMaster B";
    //                sql = sql + " INNER JOIN TextValTable T ON T.TextCode = B.Head_Code";
    //                sql = sql + " Where 1 = 1 ";
    //                //sql = sql + " AND BudgetCode ='" + bud_code + "'";
    //                DataSet bookallo = d2.select_method_wo_parameter(sql, "Text");
    //                if (bookallo.Tables.Count > 0 && bookallo.Tables[0].Rows.Count > 0)
    //                {
    //                    Binddeopt();
    //                    Bindhead();

    //                    for (int i = 0; i < bookallo.Tables[0].Rows.Count; i++)
    //                    {
    //                        txtbudget.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["BudgetCode"]);
    //                        ddlbudgethead.Items.Insert(0, Convert.ToString(bookallo.Tables[0].Rows[0]["TextVal"]));
    //                        ddlbudgetdept.Items.Insert(0, Convert.ToString(bookallo.Tables[0].Rows[0]["Dept_Name"]));
    //                        Txtbudgetamt.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["TotBudAmt"]);
    //                        Txtamt.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["TotSpendAmt"]);
    //                        Txtbal.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["TotBalAmt"]);
    //                        Txtbooks1.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["BBudAmt"]);
    //                        Txtbook2.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["BSpendAmt"]);
    //                        Txtbook3.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["BBalAmt"]);
    //                        Txtjou1.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["JBudAmt"]);
    //                        Txtjou2.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["JSpendAmt"]);
    //                        Txtjou3.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["JBalAmt"]);
    //                        Txtnobok1.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["NBudAmt"]);
    //                        Txtnobok2.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["NSpendAmt"]);
    //                        Txtnobok3.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["NBalAmt"]);
    //                        Txtremarks.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["Remarks"]);

    //                        ddlbudgetdept.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["Dept_Name"]);
    //                        ddlbudgethead.Text = Convert.ToString(bookallo.Tables[0].Rows[0]["TextVal"]);

    //                        //string firstdate = Convert.ToString(bookallo.Tables[0].Rows[0]["Budget_From"]);
    //                        //string[] split = firstdate.Split('/');
    //                        //firstdate = Convert.ToString(split[1] + "/" + split[0] + "/" + split[2]);

    //                        //string lastdate = Convert.ToString(bookallo.Tables[0].Rows[0]["Budget_To"]);
    //                        //string[] split1 = lastdate.Split('/');
    //                        //lastdate = Convert.ToString(split1[1] + "/" + split1[0] + "/" + split1[2]);
    //                        //lastdate = Convert.ToString(Convert.ToDateTime(lastdate).ToString("dd/MM/yyyy");
    //                        txt_fromdatebudget.Text = Convert.ToDateTime(bookallo.Tables[0].Rows[0]["Budget_From"]).ToString("dd/MM/yyyy");

    //                        txt_todatebudget.Text = Convert.ToDateTime(bookallo.Tables[0].Rows[0]["Budget_To"]).ToString("dd/MM/yyyy");

    //                        //double g = 0.00;
    //                        // double.TryParse(Convert.ToString(bookallo.Tables[0].Rows[0]["JBudAmt"]),out g);
    //                        //Txtjou1.Text = g;                            
    //                    }
    //                }
    //                btnadd_Click(sender, e);

    //                string a = Txtnobok1.Text;
    //                Cellclick = false;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        d2.sendErrorMail(ex, userCollegeCode, "budgetmaster");
    //    }
    //}

    #region Print

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Budget Master";
            string pagename = "budgetmaster.aspx";
            string ss = "";
            Printcontrolhed2.loadspreaddetails(Grdbudget, pagename, degreedetails, 0, ss, "1");
            Printcontrolhed2.Visible = true;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, userCollegeCode, "budgetmaster");
        }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(Grdbudget, reportname);
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
        {
            d2.sendErrorMail(ex, userCollegeCode, "budgetmaster");
        }
    }
    #endregion

    protected void btn_popclose_Click(object sender, EventArgs e)
    {
        divPopAlertbudget.Visible = false;
    }
}
