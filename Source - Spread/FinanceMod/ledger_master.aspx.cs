using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Web.Services;
using System.Drawing;

//Alter Add LedgerAcr at 16th Nov
public partial class ledger_master : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static string collegestat = string.Empty;
    static string collegestat0 = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Boolean flag_true = false;
    bool check = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("FinanceIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/FinanceMod/FinanceIndex.aspx");
                    return;
                }
            }
            //****************************************************//
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            lblvalidation1.Visible = false;
            if (!IsPostBack)
            {
                setLabelText();
                bindcollege();
                bindacccollege();
                bindledgername();
                bindaccheader();
                loadacctype();
                txt_searchby.Visible = true;

                ddlactype();
                Fpspread1.Sheets[0].RowCount = 0;
                Fpspread1.Sheets[0].ColumnCount = 0;
                Fpspread1.Visible = false;
                rdb_tufee.Visible = false;
                rdb_otfee.Visible = false;
                lbl_feetype.Visible = false;
                if (ddlcol.Items.Count > 0)
                {
                    collegestat0 = ddlcol.SelectedItem.Value.ToString();
                }
                if (ddl_college.Items.Count > 0)
                {
                    collegestat = ddl_college.SelectedItem.Value.ToString();
                }
                btn_go_Click(sender, e);
            }
            if (ddlcol.Items.Count > 0)
            {
                collegestat0 = ddlcol.SelectedItem.Value.ToString();
            }
            if (ddl_college.Items.Count > 0)
            {
                collegestat = ddl_college.SelectedItem.Value.ToString();
            }
        }
        catch (Exception ex) { }
    }

    public class HierarchyTree : List<HierarchyTree.HGroup>
    {
        public class HGroup
        {
            private int m_group_code;
            private int m_parent_code;
            private string m_group_name;

            public int group_code
            {
                get { return m_group_code; }
                set { m_group_code = value; }
            }

            public int parent_code
            {
                get { return m_parent_code; }
                set { m_parent_code = value; }
            }
            public string group_name
            {
                get { return m_group_name; }
                set { m_group_name = value; }
            }

        }
    }

    public void RecursiveChild(TreeNode tn, string searchValue, HierarchyTree.HGroup hTree)
    {
        try
        {
            if (tn.Value == searchValue)
            {
                tn.ChildNodes.Add(new TreeNode(hTree.group_name.ToString(), hTree.group_code.ToString()));
            }
            if (tn.ChildNodes.Count > 0)
            {
                foreach (TreeNode ctn in tn.ChildNodes)
                {
                    RecursiveChild(ctn, searchValue, hTree);
                }
            }
        }
        catch (Exception ex)
        {

        }
    }


    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        string query = "select distinct LedgerName from FM_LedgerMaster WHERE CollegeCode=" + collegestat0 + " and  LedgerName like '" + prefixText + "%' ";
        dw.Clear();
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["LedgerName"].ToString());
            }
        }
        return name;

    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getheader(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        string query = "select distinct HeaderName from FM_HeaderMaster WHERE CollegeCode=" + collegestat0 + " and  HeaderName like '" + prefixText + "%' ";
        dw.Clear();
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["HeaderName"].ToString());
            }
        }
        return name;

    }

    [WebMethod]
    public static string checkLedgeName(string ledgername)
    {
        string returnValue = "1";
        try
        {
            DAccess2 dd = new DAccess2();
            string led_name = ledgername;
            if (led_name.Trim() != "" && led_name != null)
            {
                string queryledname = dd.GetFunction("select distinct LedgerName,LedgerPK from FM_LedgerMaster where CollegeCode=" + collegestat + " and LedgerName='" + led_name + "'");
                if (queryledname.Trim() == "" || queryledname == null || queryledname == "0" || queryledname == "-1")
                {
                    returnValue = "0";
                }

            }
            else
            {
                returnValue = "2";
            }
        }
        catch (SqlException ex)
        {
            returnValue = "error" + ex.ToString();
        }
        return returnValue;
    }

    [WebMethod]
    public static string checkLedgeacr(string ledgeracr)
    {
        string returnValue = "1";
        try
        {
            DAccess2 dd = new DAccess2();
            string led_acr = ledgeracr;
            if (led_acr.Trim() != "" && led_acr != null)
            {
                string queryledacr = dd.GetFunction("select distinct LedgerAcr,LedgerPK from FM_LedgerMaster where CollegeCode=" + collegestat + " and LedgerAcr='" + led_acr + "'");
                if (queryledacr.Trim() == "" || queryledacr == null || queryledacr == "0" || queryledacr == "-1")
                {
                    returnValue = "0";
                }
            }
            else
            {
                returnValue = "2";
            }
        }
        catch (SqlException ex)
        {
            returnValue = "error" + ex.ToString();
        }
        return returnValue;
    }

    protected void btnexitgrp_click(object sender, EventArgs e)
    {
        poppergroup.Visible = false;
    }
    protected void ddlcol_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindcollege();
            bindledgername();
            bindaccheader();
            //txt_searchby.Visible = true;

            ddlactype();
            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnCount = 0;
            Fpspread1.Visible = false;
            rdb_tufee.Visible = false;
            rdb_otfee.Visible = false;
            lbl_feetype.Visible = false;
            ddl_type_OnSelectedIndexChanged(sender, e);
            if (ddlcol.Items.Count > 0)
            {
                collegestat0 = ddlcol.SelectedItem.Value.ToString();
            }
            btn_go_Click(sender, e);
        }
        catch { }
    }
    protected void ddl_college_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindaccheader();

        }
        catch { }
    }
    protected void chkPriority_OnCheckedChanged(object sender, EventArgs e)
    {
        btn_go_Click(sender, e);
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        divPriorityBtns.Visible = false;
        try
        {
            string ledgercode = "";
            string acctypcode = "";
            for (int i = 0; i < cbl_ledgername.Items.Count; i++)
            {
                if (cbl_ledgername.Items[i].Selected == true)
                {
                    if (ledgercode == "")
                    {
                        ledgercode = "" + cbl_ledgername.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        ledgercode = ledgercode + "'" + "," + "'" + cbl_ledgername.Items[i].Value.ToString() + "";
                    }
                }
            }
            for (int i = 0; i < cblacctyp.Items.Count; i++)
            {
                if (cblacctyp.Items[i].Selected == true)
                {
                    if (acctypcode == "")
                    {
                        acctypcode = "" + cblacctyp.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        acctypcode = acctypcode + "'" + "," + "'" + cblacctyp.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (acctypcode.Trim() == "")
            {
                div1.Visible = false;
                txt_searchby.Text = "";
                txt_header.Text = "";
                Fpspread1.Visible = false;
                rptprint.Visible = false;
                lbl_error.Visible = true;
                lbl_error.Text = "Acc Type Not Found";
            }
            else if (ledgercode != "" || acctypcode != "")
            {
                ds.Clear();
                string selectquery = "";
                if (txt_searchby.Text.Trim() != "")
                {
                    selectquery = "select l.LedgerPK,l.LedgerName,l.LedgerAcr,l.collegecode,l.FinGroupFK,case when l.LedgerType ='1' then 'Term Fee' else 'Other Fee' end as LedgerType,l.HeaderFK,case when l.LedgerMode='0' then 'Cr' when l.LedgerMode='1' then 'Dr' else 'Both' end as LedgerMode ,l.Purpose,h.HeaderName,h.HeaderPK,fg.GroupName, Priority,OpeningCredit  from FM_LedgerMaster l,FM_HeaderMaster h,FM_FinGroupMaster fg where l.HeaderFK =h.HeaderPK and fg.FinGroupPK =l.FinGroupFK and l.LedgerName ='" + Convert.ToString(txt_searchby.Text) + "' and l.LedgerPK in('" + ledgercode + "') and LedgerMode in('" + acctypcode + "') and l.CollegeCode='" + collegestat0 + "' order by case when priority is null then 1 else 0 end, priority ";
                }
                else if (txt_header.Text.Trim() != "")
                {
                    selectquery = "select l.LedgerPK,l.LedgerName,l.LedgerAcr,l.collegecode,l.FinGroupFK,case when l.LedgerType ='1' then 'Term Fee' else 'Other Fee' end as LedgerType,l.HeaderFK,case when l.LedgerMode='0' then 'Cr' when l.LedgerMode='1' then 'Dr' else 'Both' end as LedgerMode ,l.Purpose,h.HeaderName,h.HeaderPK,fg.GroupName, Priority,OpeningCredit  from FM_LedgerMaster l,FM_HeaderMaster h,FM_FinGroupMaster fg where l.HeaderFK =h.HeaderPK and fg.FinGroupPK =l.FinGroupFK and h.HeaderName ='" + Convert.ToString(txt_header.Text) + "' and l.LedgerPK in('" + ledgercode + "') and LedgerMode in('" + acctypcode + "') and l.CollegeCode='" + collegestat0 + "' order by case when priority is null then 1 else 0 end, priority ";
                }
                else
                {
                    selectquery = "select l.LedgerPK,l.LedgerName,l.LedgerAcr,l.collegecode,l.FinGroupFK,case when l.LedgerType ='1' then 'Term Fee' else 'Other Fee' end as LedgerType,l.HeaderFK,case when l.LedgerMode='0' then 'Cr' when l.LedgerMode='1' then 'Dr' else 'Both' end as LedgerMode ,l.Purpose,h.HeaderName,h.HeaderPK,fg.GroupName, Priority,hd_priority,OpeningCredit  from FM_LedgerMaster l,FM_HeaderMaster h,FM_FinGroupMaster fg where l.HeaderFK =h.HeaderPK and fg.FinGroupPK =l.FinGroupFK and l.LedgerPK in ('" + ledgercode + "') and LedgerMode in('" + acctypcode + "') and l.CollegeCode='" + collegestat0 + "' order by len(isnull(hd_priority,10000)),hd_priority asc ";//case when priority is null then 1 else 0 end, priority
                }
                ds = d2.select_method_wo_parameter(selectquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Fpspread1.Sheets[0].RowCount = 0;
                    Fpspread1.Sheets[0].ColumnCount = 0;
                    Fpspread1.CommandBar.Visible = false;
                    if (chkPriority.Checked)
                    {
                        Fpspread1.Sheets[0].AutoPostBack = false;
                    }
                    else
                    {
                        Fpspread1.Sheets[0].AutoPostBack = true;
                    }

                    Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                    Fpspread1.Sheets[0].RowHeader.Visible = false;
                    Fpspread1.Sheets[0].ColumnCount = 9;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.Black;
                    Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Columns[0].Width = 50;


                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Header Name";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    // Fpspread1.Columns[2].Width = 200;
                    Fpspread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Group Name";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Columns[2].Width = 50;
                    Fpspread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Ledger Name";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Account Type";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;

                    //Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Opening Balance";
                    //Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    //Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    //Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    //Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Fee Type";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Columns[5].Visible = false;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Purpose";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Set Priority";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Priority";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;


            

                    Fpspread1.Sheets[0].Columns[8].Locked = true;

                    FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
                    chk.AutoPostBack = true;

                    if (chkPriority.Checked)
                    {
                        Fpspread1.Sheets[0].Columns[0].Locked = true;
                        Fpspread1.Sheets[0].Columns[1].Locked = true;
                        Fpspread1.Sheets[0].Columns[2].Locked = true;
                        Fpspread1.Sheets[0].Columns[3].Locked = true;
                        Fpspread1.Sheets[0].Columns[4].Locked = true;
                        Fpspread1.Sheets[0].Columns[5].Locked = true;
                        Fpspread1.Sheets[0].Columns[6].Locked = true;

                        Fpspread1.Sheets[0].Columns[7].Visible = true;
                        Fpspread1.Sheets[0].Columns[8].Visible = true;
                    }
                    else
                    {
                        Fpspread1.Sheets[0].Columns[0].Locked = false;
                        Fpspread1.Sheets[0].Columns[1].Locked = false;
                        Fpspread1.Sheets[0].Columns[2].Locked = false;
                        Fpspread1.Sheets[0].Columns[3].Locked = false;
                        Fpspread1.Sheets[0].Columns[4].Locked = false;
                        Fpspread1.Sheets[0].Columns[5].Locked = false;
                        Fpspread1.Sheets[0].Columns[6].Locked = false;

                        Fpspread1.Sheets[0].Columns[7].Visible = false;
                        Fpspread1.Sheets[0].Columns[8].Visible = false;
                    }

                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        Fpspread1.Sheets[0].RowCount++;

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[row]["collegecode"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(ds.Tables[0].Rows[row]["OpeningCredit"]);
                        
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["HeaderName"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["HeaderPK"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["GroupName"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[row]["FinGroupFK"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["LedgerName"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[row]["LedgerPK"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["LedgerMode"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[row]["LedgerAcr"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                        //if (ds.Tables[0].Rows[row]["fee"].ToString() == "Cr")
                        //{
                        //    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["credit"]);
                        //}
                        //if (ds.Tables[0].Rows[row]["fee"].ToString() == "Dr")
                        //{
                        //    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["debit"]);
                        //}
                        //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                        //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["LedgerType"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[row]["Purpose"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";


                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].CellType = chk;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                        string pr = Convert.ToString(ds.Tables[0].Rows[row]["Priority"]).Trim();
                        if (pr != "")
                        {
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Locked = true;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Value = 1;
                        }
                        else
                        {
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Locked = false;
                        }

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Text = pr;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                    }
                    Fpspread1.Visible = true;
                    rptprint.Visible = true;
                    div1.Visible = true;
                    txt_searchby.Text = "";
                    txt_header.Text = "";
                    lbl_error.Visible = false;
                    Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;

                    if (chkPriority.Checked)
                    {
                        divPriorityBtns.Visible = true;
                        // Fpspread1.Sheets[0].AutoPostBack = true;
                        Fpspread1.Width = 750;
                        Fpspread1.Height = 500;
                    }
                    else
                    {
                        divPriorityBtns.Visible = false;
                    }
                    Fpspread1.SaveChanges();
                }
                else
                {
                    div1.Visible = false;
                    Fpspread1.Visible = false;
                    rptprint.Visible = false;
                    txt_searchby.Text = "";
                    txt_header.Text = "";
                    lbl_error.Visible = true;
                    lbl_error.Text = "No Record Found";
                }
            }
            else
            {
                div1.Visible = false;
                txt_searchby.Text = "";
                txt_header.Text = "";
                Fpspread1.Visible = false;
                rptprint.Visible = false;
                lbl_error.Visible = true;
                lbl_error.Text = "No Ledgers Found";
            }
        }
        catch
        {

        }
    }
    protected void btn_addnew_Click(object sender, EventArgs e)
    {
        btn_delete.Visible = false;
        btn_update.Visible = false;
        btn_save.Visible = true;

        bindcollege();
        bindaccheader();
        txt_ledgername1.Text = "";
        txt_ledgeracr.Text = "";
        ddlactype();
        txt_group.Text = "";
        txt_desc.Text = "";
        txt_openbal.Text = "";
        cbheader.Checked = false;
        for (int i = 0; i < cblheader.Items.Count; i++)
        {
            cblheader.Items[i].Selected = false;
        }
        txtheader.Text = "--Select--";
        rdb_tufee.Checked = true;
        rdb_otfee.Checked = false;
        poperrjs.Visible = true;


    }
    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    protected void btn_exit_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread1, reportname);
                //d2.printxml(Fpspread1, reportname);
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
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Ledger Master";
            string pagename = "ledger_master.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {

        }

    }
    protected void bindcollege()
    {
        try
        {
            string strUser = d2.getUserCode(Convert.ToString(Session["group_code"]), Convert.ToString(Session["usercode"]), 1);
            ds.Clear();
            ddl_college.Items.Clear();
            string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where " + strUser + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_college.DataSource = ds;
                ddl_college.DataTextField = "collname";
                ddl_college.DataValueField = "college_code";
                ddl_college.DataBind();
            }
        }
        catch
        {
        }
    }
    protected void bindacccollege()
    {
        try
        {
            string strUser = d2.getUserCode(Convert.ToString(Session["group_code"]), Convert.ToString(Session["usercode"]), 1);
            ds.Clear();
            ddlcol.Items.Clear();
            string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where " + strUser + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcol.DataSource = ds;
                ddlcol.DataTextField = "collname";
                ddlcol.DataValueField = "college_code";
                ddlcol.DataBind();
            }
        }
        catch
        {
        }
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        if (txt_group.Text.Trim() != "")
        {
            savedetails();
            bindledgername();
            btn_go_Click(sender, e);
            bindcollege();
        }
        else
        {
            imgdiv2.Visible = true;
            lbl_alert.Visible = true;
            lbl_alert.Text = "Please Select Group Name";
        }
    }
    protected void savedetails()
    {
        try
        {
            bool count = false;
            string check = "";
            string acccheck = "";
            string clgcode = Convert.ToString(ddl_college.SelectedItem.Value);
            string feename = Convert.ToString(txt_ledgername1.Text);
            string ledgeacr = Convert.ToString(txt_ledgeracr.Text);
            string actype = Convert.ToString(ddl_actype.SelectedItem.Value);
            string desc = Convert.ToString(txt_desc.Text);
            //double openbal = Convert.ToDouble (txt_openbal.Text);
            string group = ViewState["currcode"].ToString();
            //string yearstart = "";
            //string yearend = "";
            double openbal;
            feename = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(feename);
            desc = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(desc);

            if (rdb_tufee.Checked == true)
            {
                check = "1";
            }
            if (rdb_otfee.Checked == true)
            {
                check = "2";
            }


            if (actype == "Cr")
                acccheck = "0";
            else if (actype == "Dr")
                acccheck = "1";
            else if (actype == "Both")
                acccheck = "2";
            if (string.IsNullOrEmpty(txt_openbal.Text))
            {
                openbal = 0; // entry is null
            }
            else
            {
                openbal = Convert.ToDouble(txt_openbal.Text);
            }

            string chk = "select LedgerName from FM_LedgerMaster where LedgerName='" + feename + "' and collegecode='" + ddl_college.SelectedItem.Value + "'";
            chk = chk + "select LedgerAcr from FM_LedgerMaster where LedgerAcr='" + ledgeacr + "' and collegecode='" + ddl_college.SelectedItem.Value + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(chk, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Ledger Name already exist!";
            }
            else if (ds.Tables[1].Rows.Count > 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Ledger Acronym already exist!";
            }
            else
            {
                for (int i = 0; i < cblheader.Items.Count; i++)
                {
                    if (cblheader.Items[i].Selected == true)
                    {
                        string query = "Insert Into FM_LedgerMaster(LedgerName,LedgerAcr,HeaderFK,LedgerMode,LedgerType,Purpose,FinGroupFK,CollegeCode,OpeningCredit) values('" + feename + "','" + ledgeacr.ToUpper() + "','" + cblheader.Items[i].Value + "','" + acccheck + "','" + check + "','" + desc + "','" + group + "','" + ddl_college.SelectedItem.Value + "','" + openbal + "')";
                        int iv = d2.update_method_wo_parameter(query, "Text");
                        if (iv != 0)
                        {
                            count = true;
                        }
                    }
                }

                if (count == true)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Saved Successfully";
                    clear();
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Please Select Account Header";
                }
            }


        }
        catch (Exception ex)
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Please Try Later";
        }
    }
    public void clear()
    {
        btn_delete.Visible = false;
        btn_update.Visible = false;
        btn_save.Visible = true;
        bindcollege();
        txt_ledgername1.Text = "";
        txt_ledgeracr.Text = "";

        ddl_actype.SelectedIndex = 0;
        txt_group.Text = "";
        txt_desc.Text = "";
        txt_openbal.Text = "";
        txtheader.Text = "--Select--";
        for (int i = 0; i < cblheader.Items.Count; i++)
        {
            cblheader.Items[i].Selected = false;
        }
        rdb_tufee.Checked = true;
        rdb_otfee.Checked = false;
        poperrjs.Visible = true;


    }
    protected void cb_ledgername_CheckedChanged(object sender, EventArgs e)
    {
        int cout = 0;
        string ledgername = "";
        txt_ledgername.Text = "--Select--";
        if (cb_ledgername.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_ledgername.Items.Count; i++)
            {
                cbl_ledgername.Items[i].Selected = true;
                ledgername = Convert.ToString(cbl_ledgername.Items[i].Text);
            }
            if (cbl_ledgername.Items.Count == 1)
            {
                txt_ledgername.Text = "" + ledgername + "";
            }
            else
            {
                txt_ledgername.Text = "Ledger(" + (cbl_ledgername.Items.Count) + ")";
            }
            // txt_ledgername.Text = "Ledger(" + (cbl_ledgername.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_ledgername.Items.Count; i++)
            {
                cbl_ledgername.Items[i].Selected = false;
            }
            txt_ledgername.Text = "--Select--";
        }


    }
    protected void cbl_ledgername_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string ledgername = "";
            int i = 0;
            cb_ledgername.Checked = false;
            int commcount = 0;

            txt_ledgername.Text = "--Select--";
            for (i = 0; i < cbl_ledgername.Items.Count; i++)
            {
                if (cbl_ledgername.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    ledgername = Convert.ToString(cbl_ledgername.Items[i].Text);
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_ledgername.Items.Count)
                {

                    cb_ledgername.Checked = true;
                }
                if (commcount == 1)
                {
                    txt_ledgername.Text = "" + ledgername + "";
                }
                else
                {
                    txt_ledgername.Text = "Ledger(" + commcount.ToString() + ")";
                }
                // txt_ledgername.Text = "Ledger(" + commcount.ToString() + ")";

            }
        }
        catch (Exception ex)
        {

        }


    }

    protected void cbacctyp_CheckedChanged(object sender, EventArgs e)
    {
        int cout = 0;
        string acctype = "";
        txtacctyp.Text = "--Select--";
        if (cbacctyp.Checked == true)
        {
            cout++;
            for (int i = 0; i < cblacctyp.Items.Count; i++)
            {
                cblacctyp.Items[i].Selected = true;
                acctype = Convert.ToString(cblacctyp.Items[i].Text);
            }
            if (cblacctyp.Items.Count == 1)
            {
                txtacctyp.Text = "" + acctype + "";
            }
            else
            {
                txtacctyp.Text = "Acc Type(" + (cblacctyp.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < cblacctyp.Items.Count; i++)
            {
                cblacctyp.Items[i].Selected = false;
            }
            txtacctyp.Text = "--Select--";
        }
    }

    protected void cblacctyp_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            cbacctyp.Checked = false;
            int commcount = 0;
            string acctype = "";

            txtacctyp.Text = "--Select--";
            for (i = 0; i < cblacctyp.Items.Count; i++)
            {
                if (cblacctyp.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    acctype = Convert.ToString(cblacctyp.Items[i].Text);
                }
            }
            if (commcount > 0)
            {
                if (commcount == cblacctyp.Items.Count)
                {
                    cbacctyp.Checked = true;
                }
                if (commcount == 1)
                {
                    txtacctyp.Text = "" + acctype + "";
                }
                else
                {
                    txtacctyp.Text = "Acc Type(" + commcount.ToString() + ")";
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void cbheader_CheckedChanged(object sender, EventArgs e)
    {
        int cout = 0;

        txtheader.Text = "--Select--";
        if (cbheader.Checked == true)
        {
            cout++;
            for (int i = 0; i < cblheader.Items.Count; i++)
            {
                cblheader.Items[i].Selected = true;
            }
            txtheader.Text = "Ledger Name(" + (cblheader.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cblheader.Items.Count; i++)
            {
                cblheader.Items[i].Selected = false;
            }
            txtheader.Text = "--Select--";
        }
    }

    protected void cblheader_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            int i = 0;
            cbheader.Checked = false;
            int commcount = 0;

            txtheader.Text = "--Select--";
            for (i = 0; i < cblheader.Items.Count; i++)
            {
                if (cblheader.Items[i].Selected == true)
                {
                    commcount = commcount + 1;

                }
            }
            if (commcount > 0)
            {
                if (commcount == cblheader.Items.Count)
                {
                    cbheader.Checked = true;
                }
                txtheader.Text = "Header Name(" + commcount.ToString() + ")";

            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddl_type_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_type.SelectedValue == "0")
        {
            txt_searchby.Visible = true;
            txt_header.Visible = false;

            txt_header.Text = "";

        }
        else if (ddl_type.SelectedValue == "1")
        {
            txt_searchby.Visible = false;
            txt_header.Visible = true;

            txt_searchby.Text = "";
        }
    }

    protected void btn_group_click(object sender, EventArgs e)
    {
        poppergroup.Visible = true;
        panel3.Visible = true;
        lblerr4.Visible = false;
        if (txt_ledgername1.Text.Trim() == "")
        {
            poppergroup.Visible = false;
            imgdiv2.Visible = true;
            lbl_alert.Visible = true;
            lbl_alert.Text = "Please Enter Ledger Name";
        }
        //else if (txt_group.Text.Trim() == "")
        //{
        //    poppergroup.Visible = true;
        //    panel3.Visible = false;
        //    lblerr4.Visible = true;
        //    lblerr4.Text = "Please Select Group Name";
        //}
        //else if (txtheader.Text.Trim() == "--Select--")
        //{
        //    poppergroup.Visible = true;
        //    panel3.Visible = false;
        //    lblerr4.Visible = true;
        //    lblerr4.Text = "Please Select the Account Header";
        //}
        else
        {
            lblerr4.Visible = false;
            poppergroup.Visible = true;
            panel3.Visible = true;
            TreeView1.SelectedNodeStyle.ForeColor = System.Drawing.Color.Red;
            bindtreeview();
        }
    }

    protected void TreeView1_DataBound(object sender, EventArgs e)
    {

    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        try
        {
            poperrjs.Visible = true;
            poppergroup.Visible = false;
            panel3.Visible = false;
            TreeNode currnode = TreeView1.SelectedNode;
            TreeView1.SelectedNodeStyle.ForeColor = System.Drawing.Color.Red;
            string currnodevalue = currnode.Text;
            string currnodecode = currnode.Value;
            ViewState["currcode"] = currnodecode;
            string desc = "";

            string selquery = "select GroupDesc from FM_FinGroupMaster where FinGroupPK='" + currnodecode + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                desc = ds.Tables[0].Rows[0]["GroupDesc"].ToString();
            }
            txt_group.Text = currnodevalue;
        }
        catch
        {

        }
    }

    protected void imagebtnpopclose5_Click(object sender, EventArgs e)
    {
        poppergroup.Visible = false;
    }

    public void bindledgername()
    {

        try
        {
            cbl_ledgername.Items.Clear();
            string query = "select distinct l.LedgerPK,l.LedgerAcr,l.LedgerName,l.FinGroupFK,case when l.LedgerType ='1' then 'Term Fee' else 'Other Fee' end as LedgerType,l.HeaderFK,case when l.LedgerMode='0' then 'Cr' when l.LedgerMode='1' then 'Dr' else 'Both' end as LedgerMode ,l.Purpose,h.HeaderName,h.HeaderPK,fg.GroupName  from FM_LedgerMaster l,FM_HeaderMaster h,FM_FinGroupMaster fg where l.HeaderFK =h.HeaderPK and fg.FinGroupPK =l.FinGroupFK and l.CollegeCode='" + ddlcol.SelectedItem.Value + "'";


            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_ledgername.DataSource = ds;
                cbl_ledgername.DataTextField = "LedgerName";
                cbl_ledgername.DataValueField = "LedgerPK";
                cbl_ledgername.DataBind();

                if (cbl_ledgername.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_ledgername.Items.Count; i++)
                    {
                        cbl_ledgername.Items[i].Selected = true;
                    }
                    cb_ledgername.Checked = true;
                    txt_ledgername.Text = "Ledger(" + cbl_ledgername.Items.Count + ")";
                }
            }
            else
            {
                txt_ledgername.Text = "--Select--";
            }
        }
        catch
        {

        }

    }

    protected void bindaccheader()
    {
        try
        {
            cblheader.Items.Clear();
            string query = "SELECT distinct HeaderPK,HeaderName FROM FM_HeaderMaster L,FS_HeaderPrivilage P WHERE L.HeaderPK = P.HeaderFK AND P.CollegeCode = L.CollegeCode AND P.UserCode = " + usercode + " AND L.CollegeCode='" + ddl_college.SelectedItem.Value + "'";

            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblheader.DataSource = ds;
                cblheader.DataTextField = "HeaderName";
                cblheader.DataValueField = "HeaderPK";
                cblheader.DataBind();

                if (cblheader.Items.Count > 0)
                {
                    for (int i = 0; i < cblheader.Items.Count; i++)
                    {
                        cblheader.Items[i].Selected = true;
                    }
                    cbheader.Checked = true;
                    txtheader.Text = "Header Name(" + cblheader.Items.Count + ")";
                }
            }
            else
            {
                txtheader.Text = "--Select--";
            }
        }
        catch
        {

        }
    }

    public void bindheader()
    {
        try
        {
            cblheader.Items.Clear();
            string currrow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
            string currcol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();

            string feetype = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(currrow), 3].Text);
            string selquery = "select l.HeaderFK,a.HeaderName,a.HeaderPK from FM_LedgerMaster l,FM_HeaderMaster a where l.HeaderFK=a.HeaderPK and LedgerName='" + feetype + "' and CollegeCode='" + ddl_college.SelectedItem.Value + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblheader.DataSource = ds;
                cblheader.DataTextField = "HeaderName";
                cblheader.DataValueField = "HeaderPK";
                cblheader.DataBind();
            }
        }
        catch
        {

        }
    }

    public void loadacctype()
    {
        try
        {
            string acctype = "";
            cblacctyp.Items.Clear();
            cblacctyp.Items.Add(new ListItem("Cr", "0"));
            cblacctyp.Items.Add(new ListItem("Dr", "1"));
            cblacctyp.Items.Add(new ListItem("Both", "2"));
            for (int i = 0; i < cblacctyp.Items.Count; i++)
            {
                cblacctyp.Items[i].Selected = true;
                acctype = Convert.ToString(cblacctyp.Items[i].Text);
            }
            if (cblacctyp.Items.Count == 1)
            {
                txtacctyp.Text = "Acc Type(" + acctype + ")";
            }
            else
            {
                txtacctyp.Text = "Acc Type(" + cblacctyp.Items.Count + ")";
            }
            cbacctyp.Checked = true;
        }
        catch
        {

        }
    }

    protected void ddlactype()
    {
        try
        {
            ddl_actype.Items.Clear();

            ddl_actype.Items.Insert(0, "Cr");
            ddl_actype.Items.Insert(1, "Dr");
            ddl_actype.Items.Insert(2, "Both");
        }
        catch
        {

        }
    }
    protected void Cell_Click(object sender, EventArgs e)
    {
        try
        {
            check = true;
        }
        catch
        {

        }
    }

    protected void Fpspread1_render(object sender, EventArgs e)
    {
        if (flag_true == true)
        {
            Fpspread1.SaveChanges();
            string activrow = "";

            activrow = Fpspread1.Sheets[0].ActiveRow.ToString();
            string activecol = Fpspread1.Sheets[0].ActiveColumn.ToString();
            int actcol = Convert.ToInt16(activecol);
            int hy_order = 0;
            for (int i = 0; i <= Convert.ToInt16(Fpspread1.Sheets[0].RowCount) - 1; i++)
            {
                int isval = Convert.ToInt32(Fpspread1.Sheets[0].Cells[i, actcol].Value);
                if (isval == 1)
                {

                    hy_order++;
                    Fpspread1.Sheets[0].Cells[Convert.ToInt32(activrow), actcol].Locked = true;
                }
            }
            Fpspread1.Sheets[0].Cells[Convert.ToInt32(activrow), actcol + 1].Text = hy_order.ToString();
        }
        else
        {
            try
            {
                if (check == true)
                {
                    poperrjs.Visible = true;
                    btn_delete.Visible = true;
                    btn_update.Visible = true;
                    btn_save.Visible = false;
                    string activerow = "";
                    string activecol = "";
                    string acccheck = "";
                    string openbal = "";
                    int count = 0;
                    bindaccheader();
                    activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
                    activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
                    collegecode = "";
                    if (activerow.Trim() != "")
                    {

                        string ledgername = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text);
                        string ledgeracr = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Tag);
                        string ledgercode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Tag);
                        string headerid = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                        ViewState["ledgercode"] = Convert.ToString(ledgercode);
                        string college = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
                        ddl_college.SelectedIndex = ddl_college.Items.IndexOf(ddl_college.Items.FindByValue(college));
                        string actype = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text);

                        string amount = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Note);
                        //if (actype == "Cr")
                        //{
                        //    openbal = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text);
                        //}
                        //if (actype == "Dr")
                        //{
                        //    openbal = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text);
                        //}

                        string feetype = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text);
                        if (feetype == "Term Fee")
                        {
                            rdb_tufee.Checked = true;
                            rdb_otfee.Checked = false;
                        }
                        if (feetype == "Other Fee")
                        {
                            rdb_tufee.Checked = false;
                            rdb_otfee.Checked = true;
                        }
                        txt_openbal.Text = openbal;

                        string desc = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Text);
                        txt_group.Text = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                        txt_ledgername1.Text = Convert.ToString(ledgername);
                        txt_ledgeracr.Text = Convert.ToString(ledgeracr);
                        txt_openbal.Text = Convert.ToString(amount);
                        ViewState["currcode"] = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);


                        bindaccheader();
                        cbheader.Checked = false;
                        cblheader.ClearSelection();
                        for (int i = 0; i < cblheader.Items.Count; i++)
                        {
                            if (Convert.ToString(cblheader.Items[i].Value) == Convert.ToString(headerid))
                            {
                                cblheader.Items[i].Selected = true;
                                count = count + 1;
                            }
                        }
                        txtheader.Text = "Header Name(" + count + ")";
                        ddlactype();
                        ddl_actype.SelectedIndex = ddl_actype.Items.IndexOf(ddl_actype.Items.FindByValue(actype));
                        txt_desc.Text = Convert.ToString(desc);
                    }
                }
            }
            catch
            {

            }
        }



    }
    protected void btn_update_Click(object sender, EventArgs e)
    {

        try
        {
            string activerow = "";
            string activecol = "";
            //string openbal = Convert.ToString(txt_openbal.Text);
            bool acccheck = false;
            string acfee = "";
            string creditcheck = "";
            double openbal = 0;
            activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
            activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
            string colcode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
            string clgcode = Convert.ToString(ddl_college.SelectedItem.Value);
           
            string ledgername = Convert.ToString(txt_ledgername1.Text);
            ledgername = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(ledgername);
            string ledgeracr = Convert.ToString(txt_ledgeracr.Text);
            string actype = Convert.ToString(ddl_actype.SelectedItem.Value);

            string desc = Convert.ToString(txt_desc.Text);
            desc = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(desc);
            string groupname = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
            string ledgerid = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Tag);
            string headerid = cblheader.SelectedValue;
            string typeoffee = "";
            if (rdb_tufee.Checked)
            {
                typeoffee = "1";
            }
            if (rdb_otfee.Checked)
            {
                typeoffee = "2";
            }

            if (actype == "Cr")
                acfee = "0";
            else if (actype == "Dr")
                acfee = "1";
            else if (actype == "Both")
                acfee = "2";
            if (string.IsNullOrEmpty (txt_openbal.Text))
            {
                 openbal = 0; // entry is null
            }
            else
            {
                 openbal = Convert.ToDouble(txt_openbal.Text);
            }

            string groupcode = ViewState["currcode"].ToString();
            string selq = "select LedgerName from FM_LedgerMaster where LedgerName='" + ledgername + "' and LedgerPK not in('" + ledgerid + "') and HeaderFK in('" + headerid + "') and CollegeCode='" + collegestat + "'";
            selq = selq + " select LedgerAcr from FM_LedgerMaster where LedgerAcr='" + ledgeracr + "' and LedgerPK not in('" + ledgerid + "') and HeaderFK in('" + headerid + "') and CollegeCode='" + collegestat + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Visible = true;
                    lbl_alert.Text = "Ledger Name already Exist!";
                }
                else if (ds.Tables[1].Rows.Count > 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Visible = true;
                    lbl_alert.Text = "Ledger Acronym already Exist!";
                }
                else
                {
                    acccheck = false;
                    for (int i = 0; i < cblheader.Items.Count; i++)
                    {
                        if (cblheader.Items[i].Selected == true)
                        {
                            string query = "if exists (select * from FM_LedgerMaster where HeaderFK ='" + cblheader.Items[i].Value + "' and LedgerPK ='" + Convert.ToString(ViewState["ledgercode"]) + "' and CollegeCode='" + collegestat + "') update FM_LedgerMaster set LedgerName='" + ledgername + "',LedgerAcr='" + ledgeracr.ToUpper() + "',LedgerMode='" + acfee + "',FinGroupFK='" + groupcode + "',Purpose='" + desc + "',LedgerType='" + typeoffee + "',OpeningCredit='" + openbal + "' where LedgerPK='" + Convert.ToString(ViewState["ledgercode"]) + "' and HeaderFK='" + cblheader.Items[i].Value + "' and CollegeCode='" + clgcode + "'   else Insert Into FM_LedgerMaster(LedgerName,LedgerAcr,HeaderFK,LedgerMode,LedgerType,Purpose,FinGroupFK,CollegeCode,OpeningCredit) values('" + ledgername + "','" + ledgeracr.ToUpper() + "','" + cblheader.Items[i].Value + "','" + acfee + "','" + typeoffee + "','" + desc + "','" + groupcode + "','" + collegestat + "','" + openbal + "')";

                            int iv = d2.update_method_wo_parameter(query, "Text");
                            if (iv != 0)
                            {
                                acccheck = true;
                            }
                        }

                    }
                    if (acccheck == true)
                    {
                        imgdiv2.Visible = true;
                        btn_go_Click(sender, e);
                        bindledgername();
                        bindcollege();
                        ddlactype();
                        lbl_alert.Text = "Updated Successfully";
                        poperrjs.Visible = false;
                        btn_go_Click(sender, e);
                    }
                    if (acccheck == false)
                    {
                        imgdiv2.Visible = true;
                        lbl_alert.Visible = true;
                        lbl_alert.Text = "Please select account Header!";
                    }
                }
            }
        }
        catch
        {

        }
    }
    protected void btn_delete_Click(object sender, EventArgs e)
    {
        imgdiv1.Visible = true;
        lblalert.Visible = true;
        lblalert.Text = "Do you want to delete this record?";
    }

    protected void btnyes_Click(object sender, EventArgs e)
    {
        try
        {
            string clgcode = Convert.ToString(ddl_college.SelectedItem.Value);
            string selq = "Select * from FT_FeeAllot where LedgerFK='" + Convert.ToString(ViewState["ledgercode"]) + "'";
            selq = selq + " Select * from FT_FeeAllotDegree where LedgerFK='" + Convert.ToString(ViewState["ledgercode"]) + "'";
            selq = selq + " select * from FT_FinDailyTransaction where LedgerFK='" + Convert.ToString(ViewState["ledgercode"]) + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0 || ds.Tables[1].Rows.Count > 0 || ds.Tables[2].Rows.Count > 0)
                {
                    imgdiv1.Visible = false;
                    imgdiv2.Visible = true;
                    lbl_alert.Visible = true;
                    lbl_alert.Text = "you can't delete this record!";
                }
                else if (txtheader.Text == "--Select--")
                {
                    imgdiv1.Visible = false;
                    imgdiv2.Visible = true;
                    lbl_alert.Visible = true;
                    lbl_alert.Text = "Please select account Header!";
                }
                else
                {
                    string query2 = "delete from FM_LedgerMaster where LedgerPK='" + Convert.ToString(ViewState["ledgercode"]) + "' and CollegeCode='" + collegestat + "'";
                    int iv = d2.update_method_wo_parameter(query2, "Text");
                    if (iv != 0)
                    {
                        imgdiv1.Visible = false;
                        imgdiv2.Visible = true;
                        btn_go_Click(sender, e);
                        bindledgername();
                        bindcollege();

                        ddlactype();
                        lbl_alert.Text = "Deleted Successfully";
                        poperrjs.Visible = false;
                    }
                }
            }
        }
        catch
        {

        }
    }

    protected void btnno_Click(object sender, EventArgs e)
    {
        imgdiv1.Visible = false;
    }

    protected void bindtreeview()
    {
        try
        {
            string accheadcode = "";
            string dt_groupcode = "";
            string dt_parentcode = "";

            //for (int i = 0; i < cblheader.Items.Count; i++)
            //{
            //    if (cblheader.Items[i].Selected == true)
            //    {
            //        if (accheadcode == "")
            //        {
            //            accheadcode = "" + cblheader.Items[i].Value.ToString() + "";
            //        }
            //        else
            //        {
            //            accheadcode = accheadcode + "'" + "," + "'" + cblheader.Items[i].Value.ToString() + "";
            //        }
            //    }

            //}

            //if (accheadcode.Trim() != "")
            //{
            this.TreeView1.Nodes.Clear();
            HierarchyTree hierarchy = new HierarchyTree();
            HierarchyTree.HGroup objhtree = null;

            string selgroup = "select distinct FinGroupPK,GroupName,ParentCode from FM_FinGroupMaster where CollegeCode=" + ddl_college.SelectedItem.Value + "";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selgroup, "Text");
            this.TreeView1.Nodes.Clear();
            hierarchy.Clear();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                objhtree = new HierarchyTree.HGroup();
                objhtree.group_code = int.Parse(ds.Tables[0].Rows[i]["FinGroupPK"].ToString());
                objhtree.parent_code = int.Parse(ds.Tables[0].Rows[i]["ParentCode"].ToString());
                objhtree.group_name = ds.Tables[0].Rows[i]["GroupName"].ToString();
                hierarchy.Add(objhtree);
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                string get_topic_no = "";
                string get_topic_no1 = "";
                string get_topic_no2 = "";

                for (int dt_row_cnt = 0; dt_row_cnt < ds.Tables[0].Rows.Count; dt_row_cnt++)
                {
                    dt_groupcode = ds.Tables[0].Rows[dt_row_cnt][0].ToString();
                    string[] split_topics2 = dt_groupcode.Split('/');
                    for (int i = 0; split_topics2.GetUpperBound(0) >= i; i++)
                    {
                        if (get_topic_no == "")
                        {
                            get_topic_no = "'" + split_topics2[i] + "'";
                        }
                        else
                        {
                            get_topic_no = get_topic_no + ',' + "'" + split_topics2[i] + "'";
                        }
                    }
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int dt_dailyentdet1_row_cnt = 0; dt_dailyentdet1_row_cnt < ds.Tables[0].Rows.Count; dt_dailyentdet1_row_cnt++)
                    {
                        dt_parentcode = ds.Tables[0].Rows[dt_dailyentdet1_row_cnt][1].ToString();
                        string[] split_topics3 = dt_parentcode.Split('/');
                        for (int i = 0; split_topics3.GetUpperBound(0) >= i; i++)
                        {
                            if (get_topic_no1 == "")
                            {
                                get_topic_no1 = "'" + split_topics3[i] + "'";
                            }
                            else
                            {
                                get_topic_no1 = get_topic_no1 + ',' + "'" + split_topics3[i] + "'";
                            }
                        }
                    }

                }
                if (get_topic_no1 != "")
                {
                    get_topic_no2 = get_topic_no + "," + get_topic_no1;
                }
                else
                {
                    get_topic_no2 = get_topic_no;
                }

                selgroup = "select FinGroupPK,ParentCode,GroupName from FM_FinGroupMaster where convert(varchar,FinGroupPK) in(" + get_topic_no2 + ") and CollegeCode=" + ddl_college.SelectedItem.Value + " order by ParentCode,FinGroupPK";
                DataSet dsloadtopic = d2.select_method_wo_parameter(selgroup, "Text");
                if (dsloadtopic.Tables[0].Rows.Count > 0)
                {
                    hierarchy.Clear();

                    for (int at = 0; at < dsloadtopic.Tables[0].Rows.Count; at++)
                    {
                        string sqlquery = "select isnull(count(*),0) as ischild from FM_FinGroupMaster where CollegeCode=" + ddl_college.SelectedItem.Value + " and ParentCode=" + dsloadtopic.Tables[0].Rows[at]["FinGroupPK"].ToString() + "";
                        string ischild = d2.GetFunction(sqlquery);
                        string sqlquery1 = "select isnull(count(*),0) as isavailable from FM_FinGroupMaster where CollegeCode=" + ddl_college.SelectedItem.Value + " and convert(varchar,FinGroupPK) in(" + get_topic_no2 + ") and ParentCode=" + dsloadtopic.Tables[0].Rows[at]["FinGroupPK"].ToString() + "";
                        string isavailable = d2.GetFunction(sqlquery1);

                        if (Convert.ToInt16(ischild) == 0)
                        {
                            objhtree = new HierarchyTree.HGroup();
                            objhtree.group_code = int.Parse(dsloadtopic.Tables[0].Rows[at]["FinGroupPK"].ToString());
                            objhtree.parent_code = int.Parse(dsloadtopic.Tables[0].Rows[at]["ParentCode"].ToString());
                            objhtree.group_name = dsloadtopic.Tables[0].Rows[at]["GroupName"].ToString();
                            hierarchy.Add(objhtree);
                        }
                        else if (Convert.ToInt16(ischild) > 0 && Convert.ToInt16(isavailable) > 0)
                        {
                            objhtree = new HierarchyTree.HGroup();
                            objhtree.group_code = int.Parse(dsloadtopic.Tables[0].Rows[at]["FinGroupPK"].ToString());
                            objhtree.parent_code = int.Parse(dsloadtopic.Tables[0].Rows[at]["ParentCode"].ToString());
                            objhtree.group_name = dsloadtopic.Tables[0].Rows[at]["GroupName"].ToString();
                            hierarchy.Add(objhtree);
                        }

                    }
                }

                panel3.Visible = true;
            }
            else
            {
                lblerr4.Visible = true;
                lblerr4.Text = "Group Not Found For This Header";
            }

            foreach (HierarchyTree.HGroup hTree in hierarchy)
            {
                HierarchyTree.HGroup parentNode = hierarchy.Find(delegate(HierarchyTree.HGroup emp) { return emp.group_code == hTree.parent_code; });
                if (parentNode != null)
                {
                    foreach (TreeNode tn in TreeView1.Nodes)
                    {
                        if (tn.Value == parentNode.group_code.ToString())
                        {
                            tn.ChildNodes.Add(new TreeNode(hTree.group_name.ToString(), hTree.group_code.ToString()));
                        }
                        if (tn.ChildNodes.Count > 0)
                        {
                            foreach (TreeNode ctn in tn.ChildNodes)
                            {
                                RecursiveChild(ctn, parentNode.group_code.ToString(), hTree);
                            }
                        }

                    }
                }
                else
                {
                    TreeView1.Nodes.Add(new TreeNode(hTree.group_name, hTree.group_code.ToString()));
                }

                TreeView1.ExpandAll();
            }
        }
        //}
        catch
        {

        }
    }
    //Code added by Idhris 01-02-2016
    protected void FpSpread1_ButtonCommand(object sender, EventArgs e)
    {
        Fpspread1.SaveChanges();

        string activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
        string activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
        if (activecol == "7")
        {

            int act1 = Convert.ToInt32(activerow);
            int act2 = Convert.ToInt16(activecol);

            if (Fpspread1.Sheets[0].Cells[act1, act2].Value.ToString() == "1")
            {
                flag_true = true;
                Fpspread1.Sheets[0].Cells[act1, act2 + 1].Text = "";
            }
            else
            {
                flag_true = false;
            }
        }
        Fpspread1.SaveChanges();

    }
    protected void btnSetPriority_Click(object sender, EventArgs e)
    {
        try
        {
            imgdiv2.Visible = true;
            int insQ2 = d2.update_method_wo_parameter("update FM_LedgerMaster set Priority=null where  collegecode=" + collegestat0 + "", "Text");
            if (Fpspread1.Sheets[0].Rows.Count > 0 && chkPriority.Checked)
            {
                for (int i = 0; i < Fpspread1.Sheets[0].Rows.Count; i++)
                {
                    string priority = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 8].Text.Trim());
                    string ledgerPk = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 3].Tag);
                    if (priority.Trim() != "" && priority.Trim() != "0")
                    {
                        int insQ = d2.update_method_wo_parameter("update FM_LedgerMaster set Priority=" + priority + " where LedgerPk=" + ledgerPk + "  and collegecode=" + collegestat0 + "", "Text");
                    }
                }
                lbl_alert.Text = "Priority Assigned";
            }
            else
            {

                lbl_alert.Text = "Priority Not Assigned";
            }
        }
        catch { lbl_alert.Text = "Priority Not Assigned"; }
    }
    protected void btnResetPriority_Click(object sender, EventArgs e)
    {
        try
        {
            if (Fpspread1.Sheets[0].Rows.Count > 0 && chkPriority.Checked)
            {
                for (int i = 0; i < Fpspread1.Sheets[0].Rows.Count; i++)
                {
                    Fpspread1.Sheets[0].Cells[i, 7].Locked = false;
                    Fpspread1.Sheets[0].Cells[i, 7].Value = 0;
                    Fpspread1.Sheets[0].Cells[i, 8].Text = "";
                }

            }
            Fpspread1.SaveChanges();
        }
        catch { }
    }

    private void setLabelText()
    {
        string grouporusercode = string.Empty;
        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
        }
        else if (Session["usercode"] != null)
        {
            grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
        }
        List<Label> lbl = new List<Label>();
        List<byte> fields = new List<byte>();

        lbl.Add(lblcol);
        fields.Add(0);
        lbl.Add(lbl_collegename);
        fields.Add(0);

        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }
    // last modified 04-10-2016 sudhagar
}