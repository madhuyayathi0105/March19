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
using System.Drawing;

public partial class BudgetAllocation : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    static ArrayList ItemList1 = new ArrayList();
    static ArrayList Itemindex1 = new ArrayList();

    static ArrayList ItemList2 = new ArrayList();
    static ArrayList Itemindex2 = new ArrayList();

    static string colldept = string.Empty;
    static string collhead = string.Empty;
    static string colpop = string.Empty;

    string selectQuery = "";
    DAccess2 d2 = new DAccess2();
    int i;
    Boolean Cellclick = false;
    Boolean Cellclick1 = false;
    Boolean Cellclick2 = false;
    int cout;
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();

    DAccess2 queryObject = new DAccess2();
    DAccess2 da = new DAccess2();
    int count = 0;
    Hashtable hat = new Hashtable();
    Hashtable hat1 = new Hashtable();
    Hashtable hat2 = new Hashtable();
    string dept = "";
    string pur1 = "";
    int commcount;
    int row;
    string valcode = "";
    string querynew = "";
    decimal amount;
    decimal qty;
    int purpose;
    string allowchk = "";
    int header;
    int itemcode;
    string itemhrdcode = "";
    int deptcode;
    string pur = "";
    string additinal = "";


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
            collegecode1 = Session["collegecode"].ToString();

            if (!IsPostBack)
            {
                setLabelText();
                setLabelTextlookup();
                txt_frmdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_frmdate.Attributes.Add("readonly", "readonly");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Attributes.Add("readonly", "readonly");
                //txt_popfdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_popfdate.Attributes.Add("readonly", "readonly");
                //txt_poptdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_poptdate.Attributes.Add("readonly", "readonly");

                loadcoldept();
                loadcolhead();
                loadpopcol();
                if (ddlcolload.Items.Count > 0)
                {
                    colldept = ddlcolload.SelectedItem.Value.ToString();
                }
                if (ddlheadcol.Items.Count > 0)
                {
                    collhead = ddlheadcol.SelectedItem.Value.ToString();
                }
                if (ddlpopcol.Items.Count > 0)
                {
                    colpop = ddlpopcol.SelectedItem.Value.ToString();
                }
                rb_dept.Checked = true;
                rb_dept_CheckedChanged(sender, e);
                binddepartment();
                loadheader();
                loaditem();
                txt_searchhd1.Visible = true;
                txt_search1.Visible = true;
                headerbind();
                ledgerbind();
                ledgerbind1();
                loaddesc();
                lbl_err.Visible = false;
                Fpspread1.Visible = false;
                pheaderfilter.Visible = false;
                pcolumnorder.Visible = false;
                pheaderfilter1.Visible = false;
                pcolumnorder1.Visible = false;
                bindheader();
                pheaderfilter2.Visible = false;
                pcolumnorder2.Visible = false;
                cb_ldgr.Checked = true;
                ViewState["Valcode"] = null;
            }
            if (ddlcolload.Items.Count > 0)
            {
                colldept = ddlcolload.SelectedItem.Value.ToString();
            }
            if (ddlheadcol.Items.Count > 0)
            {
                collhead = ddlheadcol.SelectedItem.Value.ToString();
            }
            if (ddlpopcol.Items.Count > 0)
            {
                colpop = ddlpopcol.SelectedItem.Value.ToString();
            }
        }
        catch (Exception ex) { }
    }
    protected void lb3_Click(object sender, EventArgs e)
    {

    }
    protected void txt_date_TextChanged(object sender, EventArgs e)
    {

    }
    protected void txt_todate_TextChanged(object sender, EventArgs e)
    {

    }

    public void txt_popfdate_TextChanged(object sender, EventArgs e)
    {
    }
    public void txt_poptdate_TextChanged(object sender, EventArgs e)
    {
    }

    protected void rb_dept_CheckedChanged(object sender, EventArgs e)
    {
        if (rb_dept.Checked == true)
        {
            cb_dept.Enabled = true;
            cb_depitem.Enabled = true;
            cb_dept.Checked = true;

            cb_dept_CheckedChanged(sender, e);
            cb_depitem_CheckedChanged(sender, e);
            div_dept.Visible = true;
            div_header.Visible = false;
            pheaderfilter.Visible = false;
            pcolumnorder.Visible = false;
            lblerrmsg.Visible = false;
            pheaderfilter1.Visible = false;
            pcolumnorder1.Visible = false;
            pheaderfilter2.Visible = false;
            pcolumnorder2.Visible = false;
            div1.Visible = false;
            div2.Visible = false;
        }
    }
    protected void rb_header_CheckedChanged(object sender, EventArgs e)
    {
        if (rb_header.Checked == true)
        {
            cb_dept.Checked = false;
            cb_depitem.Checked = false;
            cb_dept.Enabled = false;
            cb_depitem.Enabled = false;

            loadcolhead();
            headerbind();
            ledgerbind();
            ledgerbind1();

            div_dept.Visible = false;
            div_header.Visible = true;
            pheaderfilter.Visible = false;
            pcolumnorder.Visible = false;
            lblerrmsg.Visible = false;
            pheaderfilter1.Visible = false;
            pcolumnorder1.Visible = false;
            pheaderfilter2.Visible = false;
            pcolumnorder2.Visible = false;
            div1.Visible = false;
            div2.Visible = false;
        }
    }
    protected void cb_dept_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_dept.Checked == true)
        {
            cb_depitem.Checked = false;
            lbl_itemhdr.Enabled = false;
            txt_itemhdr.Enabled = false;
            lbl_item.Enabled = false;
            txt_item.Enabled = false;
            txt_searchhd1.Enabled = false;
            lbl_itmsearch.Enabled = false;
            ddl_itmsearch.Enabled = false;
            txt_search.Enabled = false;
            pheaderfilter.Visible = false;
            pcolumnorder.Visible = false;
            lblerrmsg.Visible = false;
            pheaderfilter1.Visible = false;
            pcolumnorder1.Visible = false;
            pheaderfilter2.Visible = false;
            pcolumnorder2.Visible = false;
            div1.Visible = false;
            div2.Visible = false;
        }
        if (cb_dept.Checked == false)
        {
            cb_depitem.Checked = true;
            lbl_itemhdr.Enabled = true;
            txt_itemhdr.Enabled = true;
            lbl_item.Enabled = true;
            txt_item.Enabled = true;
            lbl_itmsearch.Enabled = true;
            ddl_itmsearch.Enabled = true;
            txt_searchhd1.Enabled = true;
            pheaderfilter.Visible = false;
            pcolumnorder.Visible = false;
            lblerrmsg.Visible = false;
            pheaderfilter1.Visible = false;
            pcolumnorder1.Visible = false;
            pheaderfilter2.Visible = false;
            pcolumnorder2.Visible = false;
            div1.Visible = false;
            div2.Visible = false;
        }
    }
    protected void cb_depitem_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_depitem.Checked == true)
        {
            cb_dept.Checked = false;
            lbl_itemhdr.Enabled = true;
            txt_itemhdr.Enabled = true;
            lbl_item.Enabled = true;
            txt_item.Enabled = true;
            lbl_itmsearch.Enabled = true;
            ddl_itmsearch.Enabled = true;
            txt_searchhd1.Enabled = true;
            txt_search.Enabled = true;
            pheaderfilter.Visible = false;
            pcolumnorder.Visible = false;
            lblerrmsg.Visible = false;
            pheaderfilter1.Visible = false;
            pcolumnorder1.Visible = false;
            pheaderfilter2.Visible = false;
            pcolumnorder2.Visible = false;
            div1.Visible = false;
            div2.Visible = false;
        }
        if (cb_depitem.Checked == false)
        {
            cb_dept.Checked = true;
            lbl_itemhdr.Enabled = false;
            txt_itemhdr.Enabled = false;
            txt_searchhd1.Enabled = false;
            lbl_item.Enabled = false;
            txt_item.Enabled = false;
            lbl_itmsearch.Enabled = false;
            ddl_itmsearch.Enabled = false;
            txt_search.Enabled = false;
            pheaderfilter.Visible = false;
            pcolumnorder.Visible = false;
            lblerrmsg.Visible = false;
            pheaderfilter1.Visible = false;
            pcolumnorder1.Visible = false;
            pheaderfilter2.Visible = false;
            pcolumnorder2.Visible = false;
            div1.Visible = false;
            div2.Visible = false;
        }
        loaditem();
    }

    protected void btn_exit_Click(object sender, EventArgs e)
    {
        popup_addnew.Visible = false;
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            int type = 0;
            int gvcount1 = 0;
            int gvcount2 = 0;
            int gvcount3 = 0;
            int inscount = 0;
            string firstdate = Convert.ToString(txt_popfdate.Text);
            string todate = Convert.ToString(txt_poptdate.Text);
            DateTime dt = new DateTime();
            DateTime dt1 = new DateTime();
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);

            string[] split1 = todate.Split('/');
            dt1 = Convert.ToDateTime(split1[1] + "/" + split1[0] + "/" + split1[2]);
            if (rb_dep.Checked == true)
            {
                type = 1;
            }
            else if (rb_depitem.Checked == true)
            {
                type = 2;
            }
            else if (rb_hdr.Checked == true)
            {
                type = 3;
            }

            string query = "insert into FinBudget(FinBudgetType,FromDate,ToDate,CollegeCode)values('" + type + "','" + dt.ToString("MM/dd/yyyy") + "','" + dt1.ToString("MM/dd/yyyy") + "','" + colpop + "')";
            d2.update_method_wo_parameter(query, "Text");

            string getcode = d2.GetFunction("select FinBudget from FinBudget where CollegeCode='" + colpop + "' order by FinBudget desc");
            if (getcode != "" && getcode.Trim() != "0")
            {
                if (rb_dep.Checked == true)
                {
                    if (gridView2.Rows.Count > 0)
                    {
                        for (int i = 0; i < gridView2.Rows.Count; i++)
                        {
                            TextBox txtamt = (TextBox)gridView2.Rows[i].FindControl("txt_amt");
                            TextBox txtqty = (TextBox)gridView2.Rows[i].FindControl("txt_bqty");
                            if (txtamt.Text.Trim() == "" && txtqty.Text.Trim() == "")
                            {
                                gvcount1++;
                            }
                        }
                        if (gvcount1 == gridView2.Rows.Count)
                        {
                            imgdiv2.Visible = true;
                            lbl_erroralert.Visible = true;
                            lbl_erroralert.Text = "Please Enter the Budget Amount & Budget Qty!";
                        }
                        else
                        {
                            for (int i = 0; i < gridView2.Rows.Count; i++)
                            {
                                TextBox txtamt = (TextBox)gridView2.Rows[i].FindControl("txt_amt");
                                TextBox txtqty = (TextBox)gridView2.Rows[i].FindControl("txt_bqty");
                                if (txtamt.Text.Trim() != "" || txtqty.Text.Trim() != "")
                                {
                                    amount = Convert.ToDecimal(txtamt.Text);
                                    qty = Convert.ToDecimal(txtqty.Text);
                                    DropDownList ddlpur = (DropDownList)gridView2.Rows[i].FindControl("ddl_pur2");
                                    if (ddlpur.SelectedItem.Text != "Select")
                                    {
                                        purpose = Convert.ToInt32(ddlpur.SelectedItem.Value);
                                    }
                                    else
                                    {
                                        purpose = 0;
                                    }
                                    Label dept = (Label)gridView2.Rows[i].FindControl("lbl_dept");
                                    string deptcode1 = d2.GetFunction("select Dept_Code from Department where Dept_Name='" + dept.Text + "' and college_code='" + colpop + "'");
                                    deptcode = Convert.ToInt16(deptcode1);

                                    CheckBox chkItemHeader = (CheckBox)gridView2.Rows[i].FindControl("cb_allo");
                                    if (chkItemHeader.Checked == true)
                                    {
                                        allowchk = "1";

                                    }
                                    else
                                    {
                                        allowchk = "0";
                                    }
                                    string query1 = "insert into FinBudgetDet(FinBudget,Dept_Code,BudgetQty,BudgetAmt,Purpose,Allow_Aditional)values('" + getcode + "','" + deptcode + "','" + qty + "','" + amount + "','" + purpose + "','" + allowchk + "')";
                                    inscount = d2.update_method_wo_parameter(query1, "Text");
                                }
                            }
                        }
                    }
                }

                if (rb_depitem.Checked == true)
                {
                    if (gridView1.Rows.Count > 0)
                    {
                        for (int i = 0; i < gridView1.Rows.Count; i++)
                        {
                            TextBox txtamt = (TextBox)gridView1.Rows[i].FindControl("txt_amt");
                            TextBox txtqty = (TextBox)gridView1.Rows[i].FindControl("txt_bqty");
                            if (txtamt.Text == "" && txtqty.Text == "")
                            {
                                gvcount2++;
                            }
                        }
                        if (gvcount2 == gridView1.Rows.Count)
                        {
                            imgdiv2.Visible = true;
                            lbl_erroralert.Visible = true;
                            lbl_erroralert.Text = "Please Enter the Budget Amount & Budget Qty!";
                        }
                        else
                        {
                            for (int i = 0; i < gridView1.Rows.Count; i++)
                            {
                                TextBox txtamt = (TextBox)gridView1.Rows[i].FindControl("txt_amt");
                                TextBox txtqty = (TextBox)gridView1.Rows[i].FindControl("txt_bqty");
                                if (txtamt.Text.Trim() != "" || txtqty.Text.Trim() != "")
                                {
                                    amount = Convert.ToDecimal(txtamt.Text);
                                    qty = Convert.ToDecimal(txtqty.Text);

                                    DropDownList ddlpur = (DropDownList)gridView1.Rows[i].FindControl("ddl_pur1");
                                    if (ddlpur.SelectedItem.Text != "Select")
                                    {
                                        purpose = Convert.ToInt32(ddlpur.SelectedItem.Value);
                                    }
                                    else
                                    {
                                        purpose = 0;
                                    }
                                    Label txtfeecode = (Label)gridView1.Rows[i].FindControl("txt_icode");
                                    itemhrdcode = Convert.ToString(txtfeecode.Text);


                                    Label dept = (Label)gridView1.Rows[i].FindControl("txt_dept");


                                    string deptcode1 = d2.GetFunction("select Dept_Code from Department where Dept_Name='" + dept.Text + "' and college_code='" + colpop + "'");
                                    deptcode = Convert.ToInt16(deptcode1);

                                    CheckBox chkItemHeader = (CheckBox)gridView1.Rows[i].FindControl("cb_allo");
                                    if (chkItemHeader.Checked == true)
                                    {
                                        allowchk = "1";
                                    }
                                    else
                                    {
                                        allowchk = "0";
                                    }
                                    string query1 = "insert into FinBudgetDet(FinBudget,Dept_Code,Item_Code,BudgetQty,BudgetAmt,Purpose,Allow_Aditional)values('" + getcode + "','" + deptcode + "','" + itemhrdcode + "','" + qty + "','" + amount + "','" + purpose + "','" + allowchk + "')";
                                    inscount = d2.update_method_wo_parameter(query1, "Text");
                                }
                            }
                        }
                    }
                }

                if (rb_hdr.Checked == true)
                {
                    if (gridView3.Rows.Count > 0)
                    {
                        for (int i = 0; i < gridView3.Rows.Count; i++)
                        {
                            TextBox txtamt = (TextBox)gridView3.Rows[i].FindControl("txt_amt");
                            TextBox txtqty = (TextBox)gridView3.Rows[i].FindControl("txt_bqty");
                            if (txtamt.Text.Trim() == "" && txtqty.Text.Trim() == "")
                            {
                                gvcount3++;
                            }
                        }
                        if (gvcount3 == gridView3.Rows.Count)
                        {
                            imgdiv2.Visible = true;
                            lbl_erroralert.Visible = true;
                            lbl_erroralert.Text = "Please Enter the Budget Amount & Budget Qty!";
                        }
                        else
                        {
                            for (int i = 0; i < gridView3.Rows.Count; i++)
                            {
                                TextBox txtamt = (TextBox)gridView3.Rows[i].FindControl("txt_amt");
                                TextBox txtqty = (TextBox)gridView3.Rows[i].FindControl("txt_bqty");
                                if (txtamt.Text.Trim() != "" || txtqty.Text.Trim() != "")
                                {
                                    amount = Convert.ToDecimal(txtamt.Text);
                                    qty = Convert.ToDecimal(txtqty.Text);
                                    DropDownList ddlpur = (DropDownList)gridView3.Rows[i].FindControl("ddl_pur");
                                    if (ddlpur.SelectedItem.Text != "Select")
                                    {
                                        purpose = Convert.ToInt32(ddlpur.SelectedItem.Value);
                                    }
                                    else
                                    {
                                        purpose = 0;
                                    }
                                    //purpose = Convert.ToInt16(ddlpur.SelectedItem.Value);

                                    CheckBox chkItemHeader = (CheckBox)gridView3.Rows[i].FindControl("cb_allo");

                                    Label txtheader = (Label)gridView3.Rows[i].FindControl("txt_hdr_code");
                                    header = Convert.ToInt16(txtheader.Text);

                                    Label txtfeecode = (Label)gridView3.Rows[i].FindControl("txt_ldgcode");
                                    itemcode = Convert.ToInt16(txtfeecode.Text);

                                    if (chkItemHeader.Checked == true)
                                    {
                                        allowchk = "1";

                                    }
                                    else
                                    {
                                        allowchk = "0";
                                    }
                                    string query1 = "insert into FinBudgetDet(FinBudget,Header_Id,Fee_Code,BudgetQty,BudgetAmt,Purpose,Allow_Aditional)values('" + getcode + "','" + header + "','" + itemcode + "','" + qty + "','" + amount + "','" + purpose + "','" + allowchk + "')";
                                    inscount = d2.update_method_wo_parameter(query1, "Text");

                                }
                            }
                        }
                    }
                }
            }
            if (inscount > 0)
            {
                imgdiv2.Visible = true;
                lbl_erroralert.Text = "Saved Successfully";
            }
        }
        catch
        {
        }
    }
    protected void cblheader_Change(object sender, EventArgs e)
    {
        try
        {
            binditem();
        }
        catch
        {

        }
    }

    protected void btn_addnew_Click(object sender, EventArgs e)
    {
        loadpopcol();
        popup_addnew.Visible = true;
        btn_update.Visible = false;
        btn_del.Visible = false;
        btn_exit1.Visible = false;
        gridView1.Visible = false;
        gridView2.Visible = false;
        gridView3.Visible = false;
        btn_save.Visible = true;

        string linkval = d2.getCurrentFinanceYear(usercode, colpop);
        string seldate = "select Convert(varchar(10),FinYearStart,103) as FinYearStart,Convert(varchar(10),FinYearEnd,103) as FinYearEnd from FM_FinYearMaster where FinYearPK='" + linkval + "' and CollegeCode='" + colpop + "'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(seldate, "Text");
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                txt_popfdate.Text = Convert.ToString(ds.Tables[0].Rows[0]["FinYearStart"]);
                txt_poptdate.Text = Convert.ToString(ds.Tables[0].Rows[0]["FinYearEnd"]);
            }
            else
            {
                txt_popfdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_poptdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }
        bindheader();
        binditem();
        bindpopdept();
        //bindpopdept();
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            int type;
            string typevalue = "";
            string department = "";
            string pur_code = "";
            string itemname = "";
            string build = "";
            string buildvalue = "";
            string build1 = "";
            string buildvalue1 = "";
            string build2 = "";
            string buildvalue2 = "";

            div2.Visible = true;
            Fpspread2.Sheets[0].Visible = true;

            pheaderfilter1.Visible = false;
            pcolumnorder1.Visible = false;
            pheaderfilter.Visible = false;
            pcolumnorder.Visible = false;
            pheaderfilter2.Visible = false;
            pcolumnorder2.Visible = false;

            Fpspread2.Sheets[0].RowHeader.Visible = false;
            Fpspread2.CommandBar.Visible = false;

            Fpspread2.Sheets[0].AutoPostBack = true;
            Fpspread2.Sheets[0].RowCount = 0;

            Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
            Fpspread2.Sheets[0].ColumnCount = 1;

            FarPoint.Web.Spread.StyleInfo darkstyle2 = new FarPoint.Web.Spread.StyleInfo();
            darkstyle2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle2.ForeColor = Color.Black;
            darkstyle2.HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle2;


            if (rb_dept.Checked == true)
            {
                if (cb_depitem.Checked == true)
                {
                    type = 2;
                }
                else
                {
                    type = 1;
                }

            }
            else
            {
                type = 3;
            }

            string[] ay = txt_frmdate.Text.Split('/');
            string[] ay1 = txt_todate.Text.Split('/');

            DateTime dt = new DateTime();
            DateTime dt1 = new DateTime();
            dt = Convert.ToDateTime(ay[1] + "/" + ay[0] + "/" + ay[2]);
            dt1 = Convert.ToDateTime(ay1[1] + "/" + ay1[0] + "/" + ay1[2]);
            //************ DEPT, HEADER , ITEM ***********

            for (int i = 0; i < cbl_deprt.Items.Count; i++)
            {
                if (cbl_deprt.Items[i].Selected == true)
                {
                    build = cbl_deprt.Items[i].Value.ToString();
                    if (buildvalue == "")
                    {
                        buildvalue = build;
                    }
                    else
                    {
                        buildvalue = buildvalue + "'" + "," + "'" + build;

                    }
                }
            }

            for (int i = 0; i < cbl_itemnheader1.Items.Count; i++)
            {

                if (cbl_itemnheader1.Items[i].Selected == true)
                {
                    build2 = cbl_itemnheader1.Items[i].Value.ToString();
                    if (buildvalue == "")
                    {
                        buildvalue2 = build2;
                    }
                    else
                    {
                        buildvalue2 = buildvalue2 + "'" + "," + "'" + build2;

                    }
                }
            }



            for (int i = 0; i < cbl_itemname1.Items.Count; i++)
            {

                if (cbl_itemname1.Items[i].Selected == true)
                {
                    build1 = cbl_itemname1.Items[i].Value.ToString();
                    if (buildvalue1 == "")
                    {
                        buildvalue1 = build1;
                    }
                    else
                    {
                        buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;

                    }
                }
            }

            //**************DEPT END ********
            querynew = "select distinct f.FinBudget,Dept_Code,Item_Code,Header_Id,Fee_Code,BudgetQty,BudgetAmt,Purpose,Allow_Aditional,f.CollegeCode from FinBudgetDet b,FinBudget f where f.FinBudget=b.FinBudget and f.FinBudgetType='" + type + "' and f.FromDate='" + dt.ToString("MM/dd/yyyy") + "' and f.ToDate='" + dt1.ToString("MM/dd/yyyy") + "' and f.CollegeCode='" + colldept + "'";

            if (txt_searchby.Text != "")
            {
                string dept = gedeptcode(txt_searchby.Text);
                querynew = "select distinct f.FinBudget,Dept_Code,Item_Code,Header_Id,Fee_Code,BudgetQty,BudgetAmt,Purpose,Allow_Aditional,f.CollegeCode from FinBudgetDet b,FinBudget f where f.FinBudget=b.FinBudget and f.FinBudgetType='" + type + "' and Dept_Code='" + dept + "' and f.CollegeCode='" + colldept + "'";
            }
            else if (txt_searchhd1.Text != "")
            {
                string hdrcode = getitemnameheadcode(txt_searchhd1.Text);
                querynew = "select distinct f.FinBudget,Dept_Code,Item_Code,Header_Id,Fee_Code,BudgetQty,BudgetAmt,Purpose,Allow_Aditional,f.CollegeCode from FinBudgetDet b,FinBudget f where f.FinBudget=b.FinBudget and f.FinBudgetType='" + type + "' and Item_Code='" + hdrcode + "' and f.CollegeCode='" + colldept + "'";
            }
            else if (txt_search.Text != "")
            {
                string item = getitemcode(txt_search.Text);
                querynew = "select distinct f.FinBudget,Dept_Code,Item_Code,Header_Id,Fee_Code,BudgetQty,BudgetAmt,Purpose,Allow_Aditional,f.CollegeCode from FinBudgetDet b,FinBudget f where f.FinBudget=b.FinBudget and f.FinBudgetType='" + type + "' and Item_Code='" + item + "' and f.CollegeCode='" + colldept + "'";
            }
            else if (txt_searchby.Text == "" && txt_search.Text == "" && buildvalue == "")
            {
                querynew = "select distinct f.FinBudget,Dept_Code,Item_Code,Header_Id,Fee_Code,BudgetQty,BudgetAmt,Purpose,Allow_Aditional,f.CollegeCode from FinBudgetDet b,FinBudget f where f.FinBudget=b.FinBudget and f.FinBudgetType='" + type + "' and f.FromDate='" + dt.ToString("MM/dd/yyyy") + "' and f.ToDate='" + dt1.ToString("MM/dd/yyyy") + "' and f.CollegeCode='" + colldept + "'";
            }
            else if (buildvalue != "")
            {
                querynew = querynew + "AND Dept_Code in ('" + buildvalue + "')";
            }

            else if (buildvalue1 != "")
            {
                querynew = querynew + "AND Item_Code in ('" + buildvalue1 + "')";
            }
            else
            {
                querynew = "";
            }

            if (cb_depitem.Checked == true)
            {
                if (querynew == "")
                {
                    Fpspread2.Visible = false;
                    div2.Visible = false;
                    lblerrmsg.Visible = true;
                    lblerrmsg.Text = "Kindly Select All List ";
                    pheaderfilter2.Visible = false;
                    pcolumnorder2.Visible = false;
                }
                else
                {
                    if (querynew != "")
                    {
                        ds = d2.select_method(querynew, hat2, "Text");
                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            Fpspread2.Visible = false;
                            lblerrmsg.Visible = true;
                            lblerrmsg.Text = "No Records Found";
                            pheaderfilter2.Visible = false;
                            pcolumnorder2.Visible = false;
                            div2.Visible = false;
                        }
                        else
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                lblerrmsg.Visible = false;
                                for (int i = 0; i < cblcolumnorder2.Items.Count; i++)
                                {
                                    if (cblcolumnorder2.Items[i].Selected == true)
                                    {
                                        hat2.Add(cblcolumnorder2.Items[i].Text, cblcolumnorder2.Items[i].Value);

                                        string colvalue = cblcolumnorder2.Items[i].Text;
                                        if (ItemList2.Contains(colvalue) == false)
                                        {
                                            ItemList2.Add(cblcolumnorder2.Items[i].Text);

                                        }
                                        tborder2.Text = "";
                                        for (int j = 0; j < ItemList2.Count; j++)
                                        {
                                            tborder2.Text = tborder2.Text + ItemList2[j].ToString();

                                            tborder2.Text = tborder2.Text + "(" + (j + 1).ToString() + ")  ";

                                        }
                                    }
                                    cblcolumnorder2.Items[0].Enabled = false;
                                }


                                if (ItemList2.Count == 0)
                                {
                                    for (int i = 0; i < 3; i++)
                                    {
                                        cblcolumnorder2.Items[i].Selected = true;
                                        hat2.Add(cblcolumnorder2.Items[i].Text, cblcolumnorder2.Items[i].Value);
                                        string colvalue = cblcolumnorder2.Items[i].Text;
                                        if (ItemList2.Contains(colvalue) == false)
                                        {
                                            ItemList2.Add(cblcolumnorder2.Items[i].Text);

                                        }
                                        tborder2.Text = "";
                                        for (int j = 0; j < ItemList2.Count; j++)
                                        {
                                            tborder2.Text = tborder2.Text + ItemList2[j].ToString();

                                            tborder2.Text = tborder2.Text + "(" + (j + 1).ToString() + ")  ";

                                        }
                                    }
                                }
                            }



                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                            Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                            for (int i = 0; i < ItemList2.Count; i++)
                            {
                                string value1 = ItemList2[i].ToString();
                                int a = value1.Length;
                                Fpspread2.Sheets[0].ColumnCount++;

                                Fpspread2.Sheets[0].ColumnHeader.Cells[Fpspread2.Sheets[0].ColumnHeader.RowCount - 1, Fpspread2.Sheets[0].ColumnCount - 1].Text = ItemList2[i].ToString();
                                Fpspread2.Sheets[0].ColumnHeader.Cells[Fpspread2.Sheets[0].ColumnHeader.RowCount - 1, Fpspread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].ColumnHeader.Cells[Fpspread2.Sheets[0].ColumnHeader.RowCount - 1, Fpspread2.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                Fpspread2.Sheets[0].ColumnHeader.Cells[Fpspread2.Sheets[0].ColumnHeader.RowCount - 1, Fpspread2.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;

                            }
                            Fpspread2.Sheets[0].RowCount = 0;
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                Fpspread2.Sheets[0].RowCount++;
                                count++;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[i]["CollegeCode"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;

                                int c = 0;
                                if (ItemList2.Count > 0 && hat2.Count > 0)
                                {
                                    for (int j = 0; j < ItemList2.Count; j++)
                                    {
                                        string k = Convert.ToString(ItemList2[j]);
                                        string names = Convert.ToString(hat2[k]);
                                        c++;
                                        if (names.Trim() != "")
                                        {
                                            string val = ds.Tables[0].Rows[i][names].ToString();

                                            if (names == "Dept_Code")
                                            {
                                                if (val.Trim() == "0" || val.Trim() == "")
                                                {
                                                    department = "";
                                                }
                                                else
                                                {
                                                    department = getdepartment(val);
                                                }
                                            }

                                            if (k == "Item Name")
                                            {
                                                if (names == "Item_Code")
                                                {
                                                    if (val.Trim() == "0" || val.Trim() == "")
                                                    {
                                                        itemname = "";
                                                    }
                                                    else
                                                    {
                                                        itemname = getitemname(val);
                                                    }
                                                }
                                            }

                                            if (names == "Purpose")
                                            {
                                                if (val.Trim() == "0" || val.Trim() == "")
                                                {
                                                    pur_code = "";
                                                }
                                                else
                                                {
                                                    pur_code = gettext(val);
                                                }
                                            }

                                            if (names == "Allow_Aditional")
                                            {
                                                if (val.Trim() == "False")
                                                {
                                                    typevalue = "0";
                                                }
                                                else
                                                {
                                                    typevalue = "1";
                                                }
                                            }
                                        }

                                        FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();
                                        Fpspread2.Sheets[0].Columns[2].CellType = textcel_type;

                                        if (names == "Dept_Code")
                                        {
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].Text = department;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].Column.Width = 250;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].Tag = Convert.ToString(ds.Tables[0].Rows[i]["FinBudget"]);
                                        }
                                        else if (k == "Item Name")
                                        {
                                            if (names == "Item_Code")
                                            {
                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].Text = itemname;
                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].Column.Width = 200;
                                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].Tag = Convert.ToString(ds.Tables[0].Rows[i]["FinBudget"]);
                                            }
                                        }

                                        else if (names == "Purpose")
                                        {
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].Text = pur_code;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].Column.Width = 200;
                                        }
                                        else if (names == "Allow_Aditional")
                                        {
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].Text = typevalue;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].Column.Width = 50;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        else
                                        {
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].Text = ds.Tables[0].Rows[i][names].ToString();
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].Column.Locked = true;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].Column.Width = 150;
                                        }
                                    }
                                }

                            }
                            if (cb_depitem.Checked == true)
                            {
                                Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                                Fpspread2.Visible = true;
                                div2.Visible = true;
                                pheaderfilter2.Visible = true;
                                pcolumnorder2.Visible = true;
                                pheaderfilter.Visible = false;
                                pcolumnorder.Visible = false;
                                pheaderfilter1.Visible = false;
                                pcolumnorder1.Visible = false;
                            }
                        }
                    }
                }
            }


        //**************************************************
            // if (cb_dept.Checked == true && cb_depitem.Checked==false)
            else
            {
                if (querynew == "")
                {
                    Fpspread2.Visible = false;
                    div2.Visible = false;
                    lblerrmsg.Visible = true;
                    lblerrmsg.Text = "Kindly Select All List ";
                    pheaderfilter1.Visible = false;
                    pcolumnorder1.Visible = false;
                    // div_report.Visible = false;
                }
                else
                {
                    if (querynew != "")
                    {
                        ds = d2.select_method(querynew, hat1, "Text");
                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            Fpspread2.Visible = false;
                            lblerrmsg.Visible = true;
                            lblerrmsg.Text = "No Records Found";
                            pheaderfilter1.Visible = false;
                            pcolumnorder1.Visible = false;
                            div2.Visible = false;
                            // div_report.Visible = false;
                        }
                        else
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                lblerrmsg.Visible = false;
                                for (int i = 0; i < cblcolumnorder1.Items.Count; i++)
                                {
                                    if (cblcolumnorder1.Items[i].Selected == true)
                                    {
                                        hat1.Add(cblcolumnorder1.Items[i].Text, cblcolumnorder1.Items[i].Value);

                                        string colvalue = cblcolumnorder1.Items[i].Text;
                                        if (ItemList1.Contains(colvalue) == false)
                                        {
                                            ItemList1.Add(cblcolumnorder1.Items[i].Text);

                                        }
                                        tborder1.Text = "";
                                        for (int j = 0; j < ItemList1.Count; j++)
                                        {
                                            tborder1.Text = tborder1.Text + ItemList1[j].ToString();

                                            tborder1.Text = tborder1.Text + "(" + (j + 1).ToString() + ")  ";

                                        }
                                    }
                                    cblcolumnorder1.Items[0].Enabled = false;
                                }


                                if (ItemList1.Count == 0)
                                {
                                    for (int i = 0; i < 3; i++)
                                    {
                                        cblcolumnorder1.Items[i].Selected = true;
                                        hat1.Add(cblcolumnorder1.Items[i].Text, cblcolumnorder1.Items[i].Value);
                                        string colvalue = cblcolumnorder1.Items[i].Text;
                                        if (ItemList1.Contains(colvalue) == false)
                                        {
                                            ItemList1.Add(cblcolumnorder1.Items[i].Text);

                                        }
                                        tborder1.Text = "";
                                        for (int j = 0; j < ItemList1.Count; j++)
                                        {
                                            tborder1.Text = tborder1.Text + ItemList1[j].ToString();

                                            tborder1.Text = tborder1.Text + "(" + (j + 1).ToString() + ")  ";

                                        }
                                    }
                                }
                            }
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                            Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                            for (int i = 0; i < ItemList1.Count; i++)
                            {
                                string value1 = ItemList1[i].ToString();
                                int a = value1.Length;
                                Fpspread2.Sheets[0].ColumnCount++;

                                Fpspread2.Sheets[0].ColumnHeader.Cells[Fpspread2.Sheets[0].ColumnHeader.RowCount - 1, Fpspread2.Sheets[0].ColumnCount - 1].Text = ItemList1[i].ToString();
                                Fpspread2.Sheets[0].ColumnHeader.Cells[Fpspread2.Sheets[0].ColumnHeader.RowCount - 1, Fpspread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].ColumnHeader.Cells[Fpspread2.Sheets[0].ColumnHeader.RowCount - 1, Fpspread2.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                Fpspread2.Sheets[0].ColumnHeader.Cells[Fpspread2.Sheets[0].ColumnHeader.RowCount - 1, Fpspread2.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;

                            }
                            Fpspread2.Sheets[0].RowCount = 0;
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                Fpspread2.Sheets[0].RowCount++;
                                count++;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[i]["CollegeCode"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;

                                int c = 0;
                                if (ItemList1.Count > 0 && hat1.Count > 0)
                                {
                                    for (int j = 0; j < ItemList1.Count; j++)
                                    {
                                        string k = Convert.ToString(ItemList1[j]);
                                        string names = Convert.ToString(hat1[k]);
                                        c++;
                                        if (names.Trim() != "")
                                        {
                                            string val = ds.Tables[0].Rows[i][names].ToString();
                                            if (names == "Dept_Code")
                                            {
                                                if (val.Trim() == "0" || val.Trim() == "")
                                                {
                                                    department = "";
                                                }
                                                else
                                                {
                                                    department = getdepartment(val);
                                                }
                                            }
                                            if (names == "Purpose")
                                            {
                                                if (val.Trim() == "0" || val.Trim() == "")
                                                {
                                                    pur_code = "";
                                                }
                                                else
                                                {
                                                    pur_code = gettext(val);
                                                }
                                            }

                                            if (names == "Allow_Aditional")
                                            {
                                                if (val.Trim() == "False")
                                                {
                                                    typevalue = "0";
                                                }
                                                else
                                                {
                                                    typevalue = "1";
                                                }
                                            }
                                        }

                                        FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();
                                        Fpspread2.Sheets[0].Columns[2].CellType = textcel_type;

                                        if (names == "Dept_Code")
                                        {
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].Text = department;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].Column.Width = 250;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].Tag = Convert.ToString(ds.Tables[0].Rows[i]["FinBudget"]);
                                        }

                                        else if (names == "Purpose")
                                        {
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].Text = pur_code;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].Column.Width = 200;
                                        }
                                        else if (names == "Allow_Aditional")
                                        {
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].Text = typevalue;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].Column.Width = 50;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        else
                                        {
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].Text = ds.Tables[0].Rows[i][names].ToString();
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].Tag = Convert.ToString(ds.Tables[0].Rows[i]["FinBudget"]);
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].Column.Locked = true;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].Column.Width = 150;
                                        }
                                    }
                                }

                            }

                            if (cb_dept.Checked == true)
                            {
                                Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                                Fpspread2.Visible = true;
                                div2.Visible = true;
                                pheaderfilter1.Visible = true;
                                pcolumnorder1.Visible = true;
                                pheaderfilter2.Visible = false;
                                pcolumnorder2.Visible = false;
                                pheaderfilter.Visible = false;
                                pcolumnorder.Visible = false;
                            }
                            if (rb_header.Checked == true)
                            {
                                Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                                Fpspread2.Visible = true;
                                div2.Visible = true;
                                pheaderfilter.Visible = true;
                                pcolumnorder.Visible = true;
                                pheaderfilter2.Visible = false;
                                pcolumnorder2.Visible = false;
                                pheaderfilter1.Visible = false;
                                pcolumnorder1.Visible = false;
                            }
                            // div_report.Visible = true;
                        }
                    }
                }
            }
            txt_search1.Text = "";
            txt_searchby.Text = "";
            txt_searchhd1.Text = "";
            txt_search.Text = "";
            //********************* department with item ************************
            //  else if ((cb_dept.Checked == true && cb_depitem.Checked==true) || (cb_dept.Checked==false && cb_depitem.Checked==true))

        }
        catch
        {
        }
    }
    protected void btn_addnew1_Click(object sender, EventArgs e)
    {
        loadpopcol();
        popup_addnew.Visible = true;
        if (rb_header.Checked == true)
        {
            rb_hdr.Checked = true;
            div_hdrwithledgr.Visible = true;
            div_depwithitem.Visible = false;
        }
        //bindheader();
        //binditem();

        string linkval = d2.getCurrentFinanceYear(usercode, colpop);
        string seldate = "select Convert(varchar(10),FinYearStart,103) as FinYearStart,Convert(varchar(10),FinYearEnd,103) as FinYearEnd from FM_FinYearMaster where FinYearPK='" + linkval + "' and CollegeCode='" + colpop + "'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(seldate, "Text");
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                txt_popfdate.Text = Convert.ToString(ds.Tables[0].Rows[0]["FinYearStart"]);
                txt_poptdate.Text = Convert.ToString(ds.Tables[0].Rows[0]["FinYearEnd"]);
            }
            else
            {
                txt_popfdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_poptdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }
        bindpopdept();
        headerpopbind();
        ledgerpopbind();

        btn_save.Visible = true;
        btn_exit.Visible = true;
        btn_exit1.Visible = false;
        btn_update.Visible = false;
        btn_del.Visible = false;
        gridView1.Visible = false;
        gridView2.Visible = false;
        gridView3.Visible = false;
    }
    protected void btn_go1_Click(object sender, EventArgs e)
    {
        try
        {
            string build = "";
            string buildvalue = "";
            string build1 = "";
            string buildvalue1 = "";

            int type;
            string typevalue = "";

            string pur_code = "";
            string headername = "";
            string ledgername = "";
            div1.Visible = true;
            Fpspread2.Visible = false;

            Fpspread1.Sheets[0].Visible = true;
            pheaderfilter.Visible = true;
            pcolumnorder.Visible = true;
            Fpspread1.Sheets[0].RowHeader.Visible = false;
            Fpspread1.CommandBar.Visible = false;
            pheaderfilter2.Visible = false;
            pcolumnorder2.Visible = false;
            Fpspread1.Sheets[0].AutoPostBack = true;
            Fpspread1.Sheets[0].RowCount = 0;

            Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
            Fpspread1.Sheets[0].ColumnCount = 1;

            FarPoint.Web.Spread.StyleInfo darkstyle2 = new FarPoint.Web.Spread.StyleInfo();
            darkstyle2.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle2.ForeColor = Color.Black;
            darkstyle2.HorizontalAlign = HorizontalAlign.Center;
            Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle2;


            if (rb_dept.Checked == true)
            {
                if (cb_dept.Checked == true && cb_depitem.Checked == true)
                {
                    type = 2;
                }
                else
                {
                    type = 1;
                }
            }

            else
            {
                type = 3;
            }


            string[] ay = txt_frmdate.Text.Split('/');
            string[] ay1 = txt_todate.Text.Split('/');

            DateTime dt = new DateTime();
            DateTime dt1 = new DateTime();
            dt = Convert.ToDateTime(ay[1] + "/" + ay[0] + "/" + ay[2]);
            dt1 = Convert.ToDateTime(ay1[1] + "/" + ay1[0] + "/" + ay1[2]);


            for (int i = 0; i < cbl_header.Items.Count; i++)
            {

                if (cbl_header.Items[i].Selected == true)
                {
                    build = cbl_header.Items[i].Value.ToString();
                    if (buildvalue == "")
                    {
                        buildvalue = build;
                    }
                    else
                    {
                        buildvalue = buildvalue + "'" + "," + "'" + build;

                    }
                }
            }

            for (int i = 0; i < cbl_ledger1.Items.Count; i++)
            {

                if (cbl_ledger1.Items[i].Selected == true)
                {
                    build1 = cbl_ledger1.Items[i].Value.ToString();
                    if (buildvalue1 == "")
                    {
                        buildvalue1 = build1;
                    }
                    else
                    {
                        buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;

                    }
                }
            }


            querynew = "select distinct f.FinBudget,Dept_Code,Item_Code,Header_Id,Fee_Code,BudgetQty,BudgetAmt,Purpose,Allow_Aditional,f.CollegeCode from FinBudgetDet b,FinBudget f where f.FinBudget=b.FinBudget and f.FinBudgetType='" + type + "' and f.FromDate='" + dt.ToString("MM/dd/yyyy") + "' and f.ToDate='" + dt1.ToString("MM/dd/yyyy") + "' and f.CollegeCode='" + collhead + "'";

            if (txt_search1.Text != "")
            {
                string valueheader = getheadercode(txt_search1.Text);
                querynew = "select distinct f.FinBudget,Dept_Code,Item_Code,Header_Id,Fee_Code,BudgetQty,BudgetAmt,Purpose,Allow_Aditional,f.CollegeCode from FinBudgetDet b,FinBudget f where f.FinBudget=b.FinBudget and f.FinBudgetType='" + type + "' and Header_Id='" + valueheader + "' and f.CollegeCode='" + collhead + "'";

            }
            else if (txt_ldg_search.Text != "")
            {
                string valueitem = getledgercode(txt_ldg_search.Text);
                querynew = "select distinct f.FinBudget,Dept_Code,Item_Code,Header_Id,Fee_Code,BudgetQty,BudgetAmt,Purpose,Allow_Aditional,f.CollegeCode from FinBudgetDet b,FinBudget f where f.FinBudget=b.FinBudget and f.FinBudgetType='" + type + "' and Fee_Code='" + valueitem + "' and f.CollegeCode='" + collhead + "'";

            }

            else if (buildvalue != "")
            {
                querynew = querynew + "AND Header_Id in ('" + buildvalue + "')";
            }

            else if (buildvalue1 != "")
            {
                querynew = querynew + "AND Fee_Code in ('" + buildvalue1 + "')";
            }

            else if (txt_search1.Text == "" && txt_ldg_search.Text == "" && buildvalue == "" && buildvalue1 == "")
            {
                querynew = "select distinct f.FinBudget,Dept_Code,Item_Code,Header_Id,Fee_Code,BudgetQty,BudgetAmt,Purpose,Allow_Aditional,f.CollegeCode from FinBudgetDet b,FinBudget f where f.FinBudget=b.FinBudget and f.FinBudgetType='" + type + "' and f.FromDate='" + dt.ToString("MM/dd/yyyy") + "' and f.ToDate='" + dt1.ToString("MM/dd/yyyy") + "' and f.CollegeCode='" + collhead + "'";

            }

            else
            {
                querynew = "";
            }
            if (querynew == "")
            {
                Fpspread1.Visible = false;
                div1.Visible = false;
                lblerrmsg.Visible = true;
                lblerrmsg.Text = "Kindly Select All List ";
                pheaderfilter.Visible = false;
                pcolumnorder.Visible = false;
                // div_report.Visible = false;
            }
            else
            {
                if (querynew != "")
                {
                    ds = d2.select_method(querynew, hat, "Text");
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        div1.Visible = false;
                        Fpspread1.Visible = false;
                        lblerrmsg.Visible = true;
                        lblerrmsg.Text = "No Records Found";
                        pheaderfilter.Visible = false;
                        pcolumnorder.Visible = false;
                        // div_report.Visible = false;
                    }
                    else
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            lblerrmsg.Visible = false;
                            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                            {
                                if (cblcolumnorder.Items[i].Selected == true)
                                {
                                    hat.Add(cblcolumnorder.Items[i].Text, cblcolumnorder.Items[i].Value);

                                    string colvalue = cblcolumnorder.Items[i].Text;
                                    if (ItemList.Contains(colvalue) == false)
                                    {
                                        ItemList.Add(cblcolumnorder.Items[i].Text);

                                    }
                                    tborder.Text = "";
                                    for (int j = 0; j < ItemList.Count; j++)
                                    {
                                        tborder.Text = tborder.Text + ItemList[j].ToString();

                                        tborder.Text = tborder.Text + "(" + (j + 1).ToString() + ")  ";

                                    }
                                }
                                cblcolumnorder.Items[0].Enabled = false;
                            }


                            if (ItemList.Count == 0)
                            {
                                for (int i = 0; i < 3; i++)
                                {
                                    cblcolumnorder.Items[i].Selected = true;
                                    hat.Add(cblcolumnorder.Items[i].Text, cblcolumnorder.Items[i].Value);
                                    string colvalue = cblcolumnorder.Items[i].Text;
                                    if (ItemList.Contains(colvalue) == false)
                                    {
                                        ItemList.Add(cblcolumnorder.Items[i].Text);

                                    }
                                    tborder.Text = "";
                                    for (int j = 0; j < ItemList.Count; j++)
                                    {
                                        tborder.Text = tborder.Text + ItemList[j].ToString();

                                        tborder.Text = tborder.Text + "(" + (j + 1).ToString() + ")  ";

                                    }
                                }
                            }
                        }



                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                        for (int i = 0; i < ItemList.Count; i++)
                        {
                            string value1 = ItemList[i].ToString();
                            int a = value1.Length;
                            Fpspread1.Sheets[0].ColumnCount++;

                            Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Text = ItemList[i].ToString();
                            Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;

                        }
                        Fpspread1.Sheets[0].RowCount = 0;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Fpspread1.Sheets[0].RowCount++;
                            count++;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[i]["CollegeCode"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;

                            int c = 0;
                            if (ItemList.Count > 0 && hat.Count > 0)
                            {
                                for (int j = 0; j < ItemList.Count; j++)
                                {
                                    string k = Convert.ToString(ItemList[j].ToString());
                                    string names = Convert.ToString(hat[k].ToString());
                                    c++;
                                    if (names.Trim() != "")
                                    {
                                        string val = ds.Tables[0].Rows[i][names].ToString();

                                        if (names == "Header_Id")
                                        {
                                            if (val.Trim() == "0" || val.Trim() == "")
                                            {
                                                headername = "";
                                            }
                                            else
                                            {
                                                headername = getheadername(val);
                                            }
                                        }

                                        if (names == "Fee_Code")
                                        {
                                            if (val.Trim() == "0" || val.Trim() == "")
                                            {
                                                ledgername = "";
                                            }
                                            else
                                            {
                                                ledgername = getledgername(val);
                                            }
                                        }

                                        if (names == "Purpose")
                                        {
                                            if (val.Trim() == "0" || val.Trim() == "")
                                            {
                                                pur_code = "";
                                            }
                                            else
                                            {
                                                pur_code = gettext(val);
                                            }
                                        }

                                        if (names == "Allow_Aditional")
                                        {
                                            if (val.Trim() == "False")
                                            {
                                                typevalue = "0";
                                            }
                                            else
                                            {
                                                typevalue = "1";
                                            }
                                        }
                                    }

                                    FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();
                                    Fpspread1.Sheets[0].Columns[2].CellType = textcel_type;

                                    if (names == "Header_Id")
                                    {
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, c].Text = headername;
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, c].Tag = Convert.ToString(ds.Tables[0].Rows[i]["FinBudget"]);
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, c].Column.Width = 250;
                                    }
                                    else if (names == "Fee_Code")
                                    {
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, c].Text = ledgername;
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, c].Tag = Convert.ToString(ds.Tables[0].Rows[i]["FinBudget"]);
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, c].Column.Width = 250;
                                    }
                                    else if (names == "Purpose")
                                    {
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, c].Text = pur_code;
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, c].Column.Width = 200;
                                    }
                                    else if (names == "Allow_Aditional")
                                    {
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, c].Text = typevalue;
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, c].Column.Width = 50;
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else
                                    {
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, c].Text = ds.Tables[0].Rows[i][names].ToString();

                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, c].Column.Locked = true;
                                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, c].Column.Width = 150;
                                    }
                                }
                            }

                        }

                        Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                        Fpspread1.Visible = true;
                        // div_report.Visible = true;
                    }
                }
            }
            txt_search1.Text = "";
            txt_ldg_search.Text = "";
        }
        catch
        {

        }

    }
    protected void imagebtnpopclose3_Click(object sender, EventArgs e)
    {
        popup_addnew.Visible = false;
    }
    protected void rb_hdr_CheckedChanged(object sender, EventArgs e)
    {
        div_depwithitem.Visible = false;
        div_hdrwithledgr.Visible = true;
        div_rdo1.Visible = false;
        headerpopbind();
        ledgerpopbind();
        gridView3.Visible = false;
        gridView1.Visible = false;
        gridView2.Visible = false;
    }
    protected void rb_depitem_CheckedChanged(object sender, EventArgs e)
    {
        div_rdo2.Visible = true;
        div_hdrwithledgr.Visible = false;
        div_depwithitem.Visible = true;
        div_rdo1.Visible = true;
        bindpopdept();
        loaddesc();
        cbdepartemt.Checked = false;
        gridView2.Visible = false;
        gridView3.Visible = false;
        gridView1.Visible = false;
    }
    protected void rb_dep_CheckedChanged(object sender, EventArgs e)
    {
        div_rdo1.Visible = true;
        div_rdo2.Visible = false;
        div_hdrwithledgr.Visible = false;
        div_depwithitem.Visible = true;
        bindpopdept();
        loaddesc();
        cbdepartemt.Checked = false;
        bindheader();
        gridView2.Visible = false;
        gridView1.Visible = false;
        gridView3.Visible = false;
    }


    protected void btnplus_Click(object sender, EventArgs e)
    {
        //imgdiv3.Visible = true;
        plusdiv.Visible = true;
        panel_description.Visible = true;
    }
    protected void btnminus_Click(object sender, EventArgs e)
    {
        if (ddl_purpose.SelectedIndex == -1)
        {
            imgdiv2.Visible = true;
            lbl_erroralert.Text = "No records found";
        }
        else if (ddl_purpose.SelectedIndex == 0)
        {
            imgdiv2.Visible = true;
            lbl_erroralert.Text = "Select any record";
        }
        else if (ddl_purpose.SelectedIndex != 0)
        {
            string sql = "delete from textvaltable where TextCode='" + ddl_purpose.SelectedItem.Value.ToString() + "' and TextCriteria='FiBud' and college_code='" + collegecode1 + "' ";
            int delete = d2.update_method_wo_parameter(sql, "TEXT");
            if (delete != 0)
            {
                imgdiv2.Visible = true;
                lbl_erroralert.Text = "Deleted Sucessfully";
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_erroralert.Text = "No records found";
            }
            loaddesc();
        }

        else
        {
            imgdiv2.Visible = true;
            lbl_erroralert.Text = "No records found";
        }
    }
    public void loaddesc()
    {
        ddl_purpose.Items.Clear();
        ds.Tables.Clear();

        string sql = "select TextCode,TextVal from TextValTable where TextCriteria ='FiBud' and college_code ='" + colpop + "'";
        ds = d2.select_method_wo_parameter(sql, "TEXT");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_purpose.DataSource = ds;
            ddl_purpose.DataTextField = "TextVal";
            ddl_purpose.DataValueField = "TextCode";
            ddl_purpose.DataBind();
            ddl_purpose.Items.Insert(0, new ListItem("Select", "0"));
        }
        else
        {
            ddl_purpose.Items.Insert(0, new ListItem("Select", "0"));
        }

    }
    protected void btndescpopadd_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_description11.Text != "")
            {
                string sql = "if exists ( select * from TextValTable where TextVal ='" + txt_description11.Text + "' and TextCriteria ='FiBud' and college_code ='" + collegecode1 + "') update TextValTable set TextVal ='" + txt_description11.Text + "' where TextVal ='" + txt_description11.Text + "' and TextCriteria ='FiBud' and college_code ='" + collegecode1 + "' else insert into TextValTable (TextVal,TextCriteria,college_code) values ('" + txt_description11.Text + "','FiBud','" + collegecode1 + "')";
                int insert = d2.update_method_wo_parameter(sql, "TEXT");
                if (insert != 0)
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Saved sucessfully";
                    txt_description11.Text = "";
                    //imgdiv3.Visible = false;
                    plusdiv.Visible = false;
                    panel_description.Visible = false;
                }
                loaddesc();
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_erroralert.Text = "Enter the description";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btndescpopexit_Click(object sender, EventArgs e)
    {
        //imgdiv3.Visible = false;
        plusdiv.Visible = false;
        panel_description.Visible = false;
    }
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    public void bindheader()
    {
        try
        {

            //string group_code = Session["group_code"].ToString();
            //string columnfield = "";
            //if (group_code.Contains(';'))
            //{
            //    string[] group_semi = group_code.Split(';');
            //    group_code = group_semi[0].ToString();
            //}
            //if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            //{
            //    columnfield = " and group_code='" + group_code + "'";
            //}
            //else
            //{
            //    columnfield = " and usercode='" + Session["usercode"] + "'";
            //}
            //string maninvalue = "";
            //string selectnewquery = d2.GetFunction("select value  from Master_Settings where settings='ItemHeaderRights' " + columnfield + "");
            //if (selectnewquery.Trim() != "" && selectnewquery.Trim() != "0")
            //{
            //    string[] splitnew = selectnewquery.Split(',');
            //    if (splitnew.Length > 0)
            //    {
            //        for (int row = 0; row <= splitnew.GetUpperBound(0); row++)
            //        {
            //            if (maninvalue == "")
            //            {
            //                maninvalue = Convert.ToString(splitnew[row]);
            //            }
            //            else
            //            {
            //                maninvalue = maninvalue + "'" + "," + "'" + Convert.ToString(splitnew[row]);
            //            }
            //        }
            //    }
            //}
            //string headerquery = "";
            //if (maninvalue.Trim() != "")
            //{
            //    headerquery = "select distinct itemheader_code ,itemheader_name  from item_master where itemheader_code in ('" + maninvalue + "')";
            //}
            //else
            //{
            //    headerquery = "select distinct itemheader_code ,itemheader_name  from item_master";
            //}

            cblheader.Items.Clear();
            ds.Clear();
            cblheader.Items.Clear();
            //string statequery = "select distinct itemheader_code ,itemheader_name  from item_master where Is_Hostel ='0'";
            //ds = da.select_method_wo_parameter(headerquery, "Text");
            ds = da.BindItemHeaderWithOutRights_inv();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblheader.DataSource = ds;
                cblheader.DataTextField = "ItemHeaderName";
                cblheader.DataValueField = "ItemHeaderCode";
                cblheader.DataBind();

                //cbl_itemnheader1.DataSource = ds;
                //cbl_itemnheader1.DataTextField = "itemheader_name";
                //cbl_itemnheader1.DataValueField = "itemheader_code";
                //cbl_itemnheader1.DataBind();

            }

        }
        catch
        {

        }
    }
    public void binditem()
    {
        try
        {
            string itemheadercode = "";
            for (int i = 0; i < cblheader.Items.Count; i++)
            {
                if (cblheader.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + cblheader.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "'" + "," + "'" + cblheader.Items[i].Value.ToString() + "";
                    }
                }
            }
            ds.Clear();
            cblitem.Items.Clear();
            //string statequery = "select item_code,(item_name +'-'+ model_name )as item_name  from item_master where itemheader_code in ('" + itemheadercode + "') and itemheader_code<>'' and Is_Hostel ='0'";
            //ds = da.select_method_wo_parameter(statequery, "Text");
            ds = da.BindItemCode_inv(itemheadercode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblitem.DataSource = ds;
                cblitem.DataTextField = "itemname";
                cblitem.DataValueField = "itemcode";
                cblitem.DataBind();

            }
        }
        catch
        {

        }
    }

    public void binddepartment()
    {
        try
        {
            // string deptquery = "select Dept_Code as DeptCode ,Dept_Name as DeptName from Department where college_code ='" + colldept + "' order by Dept_Code ";
            string strquery = "select distinct degree.degree_code,department.dept_name,department.dept_code,degree.dept_priority,len(isnull(degree.dept_priority,1000)) from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code  and degree.college_code=" + colldept + "  and deptprivilages.Degree_code=degree.Degree_code  order by len(isnull(degree.dept_priority,1000)),degree.dept_priority asc ";
            //and degree.course_id in(" + course_id + ") and user_code=" + usercode + "
            ds.Clear();
            ds = da.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbldepartment.DataSource = ds;
                cbldepartment.DataTextField = "Dept_Name";
                cbldepartment.DataValueField = "dept_code";
                cbldepartment.DataBind();

                cbl_deprt.DataSource = ds;
                cbl_deprt.DataTextField = "Dept_Name";
                cbl_deprt.DataValueField = "dept_code";
                cbl_deprt.DataBind();

                cb_deprt.Checked = true;
                if (cbl_deprt.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_deprt.Items.Count; i++)
                    {
                        cbl_deprt.Items[i].Selected = true;
                    }
                    txt_deprt.Text = lbl_deprt.Text + "(" + cbl_deprt.Items.Count + ")";
                }

                cbdepartemt.Checked = true;
                if (cbldepartment.Items.Count > 0)
                {
                    for (int i = 0; i < cbldepartment.Items.Count; i++)
                    {
                        cbldepartment.Items[i].Selected = true;
                    }

                }

            }
        }
        catch
        {

        }
    }

    public void bindpopdept()
    {
        try
        {
            string deptquery = "select Dept_Code as DeptCode ,Dept_Name as DeptName from Department where college_code ='" + colpop + "' order by Dept_Code ";
            ds.Clear();
            ds = da.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbldepartment.DataSource = ds;
                cbldepartment.DataTextField = "DeptName";
                cbldepartment.DataValueField = "DeptCode";
                cbldepartment.DataBind();

                cbl_deprt.DataSource = ds;
                cbl_deprt.DataTextField = "DeptName";
                cbl_deprt.DataValueField = "DeptCode";
                cbl_deprt.DataBind();

                cb_deprt.Checked = true;
                if (cbl_deprt.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_deprt.Items.Count; i++)
                    {
                        cbl_deprt.Items[i].Selected = true;
                    }
                    txt_deprt.Text = lbl_deprt.Text + "(" + cbl_deprt.Items.Count + ")";
                }

                cbdepartemt.Checked = true;
                if (cbldepartment.Items.Count > 0)
                {
                    for (int i = 0; i < cbldepartment.Items.Count; i++)
                    {
                        cbldepartment.Items[i].Selected = true;
                    }

                }

            }
        }
        catch
        {

        }
    }

    protected void chckconitm_change(object sender, EventArgs e)
    {
        try
        {
            if (cblitem.Items.Count > 0)
            {
                if (chckconitm.Checked == true)
                {
                    for (int i = 0; i < cblitem.Items.Count; i++)
                    {
                        cblitem.Items[i].Selected = true;
                    }
                }
                else
                {
                    for (int i = 0; i < cblitem.Items.Count; i++)
                    {
                        cblitem.Items[i].Selected = false;
                    }
                }
            }
        }
        catch
        {

        }
    }

    public void cbhdr_CheckedChanged(object Sender, EventArgs e)
    {
        try
        {
            if (cblheader.Items.Count > 0)
            {
                if (cbhdr.Checked == true)
                {
                    for (int i = 0; i < cblheader.Items.Count; i++)
                    {
                        cblheader.Items[i].Selected = true;
                    }
                }
                else
                {
                    for (int i = 0; i < cblheader.Items.Count; i++)
                    {
                        cblheader.Items[i].Selected = false;
                    }
                    cbhdr.Checked = false;
                }
            }
            binditem();
        }
        catch
        {

        }
    }
    protected void cbdepartment_Change(object sender, EventArgs e)
    {
        //try
        //{
        //    if (cbldepartment.Items.Count > 0)
        //    {
        //        if (cbdepartemt.Checked == true)
        //        {
        //            for (int i = 0; i < cbldepartment.Items.Count; i++)
        //            {
        //                cbldepartment.Items[i].Selected = true;
        //            }
        //        }
        //        else
        //        {
        //            for (int i = 0; i < cbldepartment.Items.Count; i++)
        //            {
        //                cbldepartment.Items[i].Selected = false;
        //            }
        //        }
        //    }
        //}
        //catch
        //{

        //}


        if (cbdepartemt.Checked == true)
        {
            for (int i = 0; i < cbldepartment.Items.Count; i++)
            {
                cbldepartment.Items[i].Selected = true;
            }

        }
        else
        {
            for (int i = 0; i < cbldepartment.Items.Count; i++)
            {
                cbldepartment.Items[i].Selected = false;
            }

        }
    }

    protected void cbldepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {


            cbdepartemt.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbldepartment.Items.Count; i++)
            {
                if (cbldepartment.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbldepartment.Items.Count)
                {
                    cbdepartemt.Checked = true;
                }
            }

        }
        catch (Exception ex)
        {

        }
    }

    protected void cb_pop1header_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbl_pop1hrdr.Items.Count > 0)
            {
                if (cb_pop1header.Checked == true)
                {
                    for (int i = 0; i < cbl_pop1hrdr.Items.Count; i++)
                    {
                        cbl_pop1hrdr.Items[i].Selected = true;
                    }
                }
                else
                {
                    for (int i = 0; i < cbl_pop1hrdr.Items.Count; i++)
                    {
                        cbl_pop1hrdr.Items[i].Selected = false;
                    }
                }
            }
            ledgerbind();
        }
        catch
        {

        }
    }

    protected void cb_ldgr_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbl_ldgr.Items.Count > 0)
            {
                if (cb_ldgr.Checked == true)
                {
                    for (int i = 0; i < cbl_ldgr.Items.Count; i++)
                    {
                        cbl_ldgr.Items[i].Selected = true;
                    }
                }
                else
                {
                    for (int i = 0; i < cbl_ldgr.Items.Count; i++)
                    {
                        cbl_ldgr.Items[i].Selected = false;
                    }
                }
            }
        }
        catch
        {

        }

    }

    public void cbl_ldgr_selectedchanged(object sender, EventArgs e)
    {
        try
        {
            cb_ldgr.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_ldgr.Items.Count; i++)
            {
                if (cbl_ldgr.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_ldgr.Items.Count)
                {
                    cb_ldgr.Checked = true;
                }
            }
        }
        catch
        {
        }
    }

    protected void cbl_pop1hrdr_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ledgerbind();
        }
        catch
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

        string query = "select distinct itemname from IM_ItemMaster WHERE itemname like '" + prefixText + "%' ";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables.Count > 0)
        {
            if (dw.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
                {
                    name.Add(dw.Tables[0].Rows[i]["itemname"].ToString());
                }
            }
        }
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getdept(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();

        string query = "select Dept_Name from Department WHERE Dept_Name like '" + prefixText + "%' and college_code='" + colldept + "' ";
        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getitemheader(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct ItemHeaderName from IM_ItemMaster WHERE ItemHeaderName like '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;
    }

    protected void cb_deprt_Change(object sender, EventArgs e)
    {
        try
        {
            string deptname = "";
            // cb_deprt.Checked = true;
            if (cb_deprt.Checked == true)
            {
                for (int i = 0; i < cbl_deprt.Items.Count; i++)
                {
                    cbl_deprt.Items[i].Selected = true;
                    deptname = Convert.ToString(cbl_deprt.Items[i].Text);
                }
                if (cbl_deprt.Items.Count == 1)
                {
                    txt_deprt.Text = "" + deptname + "";
                }
                else
                {
                    txt_deprt.Text = lbl_deprt.Text + "(" + (cbl_deprt.Items.Count) + ")";
                }
                // txt_deprt.Text = "Department(" + (cbl_deprt.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_deprt.Items.Count; i++)
                {
                    cbl_deprt.Items[i].Selected = false;
                }
                txt_deprt.Text = "--Select--";
            }

        }
        catch (Exception ex)
        {

        }

    }
    protected void cbl_deprt_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string deptname = "";
            txt_deprt.Text = "--Select--";
            cb_deprt.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_deprt.Items.Count; i++)
            {
                if (cbl_deprt.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    deptname = Convert.ToString(cbl_deprt.Items[i].Text);
                }
            }
            if (commcount > 0)
            {
                // txt_deprt.Text = "Department(" + commcount.ToString() + ")";
                if (commcount == cbl_deprt.Items.Count)
                {
                    cb_deprt.Checked = true;
                }
                if (commcount == 1)
                {
                    txt_deprt.Text = "" + deptname + "";
                }
                else
                {
                    txt_deprt.Text = lbl_deprt.Text + "(" + commcount.ToString() + ")";
                }
            }

        }
        catch (Exception ex)
        {

        }
    }

    public void chk_header_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string headername = "";
            if (chk_header1.Checked == true)
            {
                for (int i = 0; i < cbl_header.Items.Count; i++)
                {
                    cbl_header.Items[i].Selected = true;
                    headername = Convert.ToString(cbl_header.Items[i].Text);
                }
                if (cbl_header.Items.Count == 1)
                {
                    txt_header.Text = "" + headername + "";
                }
                else
                {
                    txt_header.Text = "Header Name(" + (cbl_header.Items.Count) + ")";
                }
                //txt_header.Text = "Header Name(" + (cbl_header.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_header.Items.Count; i++)
                {
                    cbl_header.Items[i].Selected = false;
                }
                txt_header.Text = "--Select--";
            }
            ledgerbind1();
        }
        catch (Exception ex)
        {

        }
    }
    public void cbl_header_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string headername = "";
            txt_header.Text = "--Select--";
            chk_header1.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_header.Items.Count; i++)
            {
                if (cbl_header.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    headername = Convert.ToString(cbl_header.Items[i].Text);
                }
            }
            if (commcount > 0)
            {
                //  txt_header.Text = "Header Name(" + commcount.ToString() + ")";
                if (commcount == cbl_header.Items.Count)
                {
                    chk_header1.Checked = true;
                }
                if (commcount == 1)
                {
                    txt_header.Text = "" + headername + "";
                }
                else
                {
                    txt_header.Text = "Header Name(" + commcount.ToString() + ")";
                }
            }
            ledgerbind1();
        }
        catch (Exception ex)
        {

        }
    }

    public void cb_itenheader_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_itenheader1.Checked == true)
            {
                for (int i = 0; i < cbl_itemnheader1.Items.Count; i++)
                {
                    cbl_itemnheader1.Items[i].Selected = true;
                }
                txt_itemhdr.Text = "Header Name(" + (cbl_itemnheader1.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_itemnheader1.Items.Count; i++)
                {
                    cbl_itemnheader1.Items[i].Selected = false;
                }
                txt_itemhdr.Text = "--Select--";
            }
            loaditem();
        }
        catch (Exception ex)
        {

        }
    }
    public void cbl_itemnheader1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            txt_itemhdr.Text = "--Select--";
            cb_itenheader1.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_itemnheader1.Items.Count; i++)
            {
                if (cbl_itemnheader1.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_itemhdr.Text = "Header Name(" + commcount.ToString() + ")";
                if (commcount == cbl_itemnheader1.Items.Count)
                {
                    cb_itenheader1.Checked = true;
                }
            }
            loaditem();
        }
        catch (Exception ex)
        {

        }
    }

    public void cb_item1_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_item1.Checked == true)
            {
                for (int i = 0; i < cbl_itemname1.Items.Count; i++)
                {
                    cbl_itemname1.Items[i].Selected = true;
                }
                txt_item.Text = "Item Name(" + (cbl_itemname1.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_itemname1.Items.Count; i++)
                {
                    cbl_itemname1.Items[i].Selected = false;
                }
                txt_item.Text = "--Select--";
            }
            //  loaditem();
        }
        catch (Exception ex)
        {

        }
    }
    public void cbl_item1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            txt_item.Text = "--Select--";
            cb_item1.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_itemname1.Items.Count; i++)
            {
                if (cbl_itemname1.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_itemhdr.Text = "Item Name(" + commcount.ToString() + ")";
                if (commcount == cbl_itemname1.Items.Count)
                {
                    cb_item1.Checked = true;
                }
            }
            // loaditem();
        }
        catch (Exception ex)
        {

        }
    }

    public void loadheader()
    {

        try
        {

            //string group_code = Session["group_code"].ToString();
            //string columnfield = "";
            //if (group_code.Contains(';'))
            //{
            //    string[] group_semi = group_code.Split(';');
            //    group_code = group_semi[0].ToString();
            //}
            //if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            //{
            //    columnfield = " and group_code='" + group_code + "'";
            //}
            //else
            //{
            //    columnfield = " and usercode='" + Session["usercode"] + "'";
            //}
            //string maninvalue = "";
            //string selectnewquery = d2.GetFunction("select value  from Master_Settings where settings='ItemHeaderRights' " + columnfield + "");
            //if (selectnewquery.Trim() != "" && selectnewquery.Trim() != "0")
            //{
            //    string[] splitnew = selectnewquery.Split(',');
            //    if (splitnew.Length > 0)
            //    {
            //        for (int row = 0; row <= splitnew.GetUpperBound(0); row++)
            //        {
            //            if (maninvalue == "")
            //            {
            //                maninvalue = Convert.ToString(splitnew[row]);
            //            }
            //            else
            //            {
            //                maninvalue = maninvalue + "'" + "," + "'" + Convert.ToString(splitnew[row]);
            //            }
            //        }
            //    }
            //}
            //string headerquery = "";
            //if (maninvalue.Trim() != "")
            //{
            //    headerquery = "select distinct itemheader_code ,itemheader_name  from item_master where itemheader_code in ('" + maninvalue + "')";
            //}
            //else
            //{
            //    headerquery = "select distinct itemheader_code ,itemheader_name  from item_master";
            //}
            cbl_itemnheader1.Items.Clear();
            ds.Clear();
            ds = d2.BindItemHeaderWithRights_inv();
            //ds = d2.select_method_wo_parameter(headerquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_itemnheader1.DataSource = ds;
                cbl_itemnheader1.DataTextField = "ItemHeaderName";
                cbl_itemnheader1.DataValueField = "ItemHeaderCode";
                cbl_itemnheader1.DataBind();

                //cbl_header.DataSource = ds;
                //cbl_header.DataTextField = "itemheader_name";
                //cbl_header.DataValueField = "itemheader_code";
                //cbl_header.DataBind();

                cb_itenheader1.Checked = true;
                if (cbl_itemnheader1.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_itemnheader1.Items.Count; i++)
                    {
                        cbl_itemnheader1.Items[i].Selected = true;
                    }
                    txt_itemhdr.Text = "Header Name(" + cbl_itemnheader1.Items.Count + ")";
                }
            }

        }
        catch
        {

        }

    }

    public void loaditem()
    {
        try
        {

            // cbl_itemname1.Items.Clear();
            txt_item.Text = "--Select--";
            string itemheadercode = "";
            for (int i = 0; i < cbl_itemnheader1.Items.Count; i++)
            {
                if (cbl_itemnheader1.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + cbl_itemnheader1.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "'" + "," + "'" + cbl_itemnheader1.Items[i].Value.ToString() + "";
                    }
                }
            }
            ds.Clear();
            cbl_itemname1.Items.Clear();
            //string statequery = "select item_code,(item_name +'-'+ model_name )as item_name  from item_master where itemheader_code in ('" + itemheadercode + "') and itemheader_code<>'' and Is_Hostel ='0'";
            //ds = da.select_method_wo_parameter(statequery, "Text");
            ds = da.BindItemCode_inv(itemheadercode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_itemname1.DataSource = ds;
                cbl_itemname1.DataTextField = "itemname";
                cbl_itemname1.DataValueField = "itemcode";
                cbl_itemname1.DataBind();
                // cbl_itemname1.Items.Clear();

                if (cbl_itemname1.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_itemname1.Items.Count; i++)
                    {
                        cbl_itemname1.Items[i].Selected = true;
                    }
                    txt_item.Text = "Item Name(" + cbl_itemname1.Items.Count + ")";
                }
            }
        }
        catch
        {

        }
    }

    protected void ddl_itmsearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_itmsearch.SelectedValue == "0")
        {
            txt_searchhd1.Visible = true;
            txt_search.Visible = false;
            txt_search.Text = "";
        }
        else if (ddl_itmsearch.SelectedValue == "1")
        {
            txt_searchhd1.Visible = false;
            txt_search.Visible = true;
            txt_searchhd1.Text = "";
        }
    }

    public void btn_pop1go_Click(object sender, EventArgs e)
    {
        if (rb_dep.Checked == true)
        {
            gridView3.Visible = false;
            gridView2.Visible = true;
            bindgrid2();
            gridView1.Visible = false;
            lbl_err.Visible = false;
        }
        if (rb_depitem.Checked == true)
        {
            gridView2.Visible = false;
            gridView3.Visible = false;
            gridView1.Visible = true;

            bindgrid();
            lbl_err.Visible = false;
        }
        if (rb_hdr.Checked == true)
        {
            gridView2.Visible = false;
            gridView1.Visible = false;
            gridView3.Visible = true;
            bindgrid3();
            lbl_err.Visible = false;
        }
    }

    public void bindgrid()
    {
        try
        {
            int i;
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add("department");
            dt.Columns.Add("Header");

            dt.Columns.Add("ItemCode");
            dt.Columns.Add("Itemname");
            dt.Columns.Add("BudQty");
            dt.Columns.Add("BudAmt");


            string itemheadercode = "";
            string itemcode = "";
            string deptcode = "";

            for (int j = 0; j < cbldepartment.Items.Count; j++)
            {
                if (cbldepartment.Items[j].Selected == true)
                {
                    if (deptcode == "")
                    {
                        deptcode = "" + cbldepartment.Items[j].Value.ToString() + "";
                    }
                    else
                    {
                        deptcode = deptcode + "'" + "," + "'" + cbldepartment.Items[j].Value.ToString() + "";
                    }

                }

            }


            for (int j = 0; j < cblheader.Items.Count; j++)
            {
                if (cblheader.Items[j].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + cblheader.Items[j].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "'" + "," + "'" + cblheader.Items[j].Value.ToString() + "";
                    }

                }

            }

            for (int j = 0; j < cblitem.Items.Count; j++)
            {
                if (cblitem.Items[j].Selected == true)
                {
                    if (itemcode == "")
                    {
                        itemcode = "" + cblitem.Items[j].Value.ToString() + "";
                    }
                    else
                    {
                        itemcode = itemcode + "'" + "," + "'" + cblitem.Items[j].Value.ToString() + "";
                    }

                }

            }

            string query = "select itemname,itemcode,ItemHeaderCode,ItemHeaderName,d.Dept_Name,d.Dept_Code from IM_ItemMaster,Department d where ItemHeaderCode	IN('" + itemheadercode + "') and itemcode IN ('" + itemcode + "') and d.Dept_Code IN ('" + deptcode + "')";
            ds = da.select_method_wo_parameter(query, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    dr = dt.NewRow();
                    string hearde1 = Convert.ToString(ds.Tables[0].Rows[i]["ItemHeaderCode"]);
                    string header = d2.GetFunction("select distinct ItemHeaderName from IM_ItemMaster where ItemHeaderCode='" + hearde1 + "'");

                    dr[0] = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                    dr[1] = Convert.ToString(header);

                    dr[2] = Convert.ToString(ds.Tables[0].Rows[i]["itemcode"]);
                    dr[3] = Convert.ToString(ds.Tables[0].Rows[i]["itemname"]);

                    dt.Rows.Add(dr);
                }

                if (dt.Rows.Count > 0)
                {
                    gridView1.DataSource = dt;
                    gridView1.DataBind();
                }

            }


        }
        catch
        {
        }
    }

    public void bindgrid2()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("department");
        dt.Columns.Add("BudQty");
        dt.Columns.Add("BudAmt");
        // dt.Columns.Add("code");

        DataRow dr;

        for (int i = 0; i < cbldepartment.Items.Count; i++)
        {
            if (cbldepartment.Items[i].Selected == true)
            {
                dr = dt.NewRow();
                dr[0] = cbldepartment.Items[i].Text;
                // dr[0] = cbldepartment.Items[i].Value;
                dt.Rows.Add(dr);
            }
        }

        if (dt.Rows.Count > 0)
        {
            gridView2.DataSource = dt;
            gridView2.DataBind();
        }
    }

    public void bindgrid3()
    {
        try
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Header");
            dt.Columns.Add("Header_Code");
            dt.Columns.Add("Ledger");
            dt.Columns.Add("Ledger_code");
            dt.Columns.Add("BudQty");
            dt.Columns.Add("BudAmt");

            DataRow dr = null;

            string itemheadercode = "";
            string ledgercode = "";


            for (int i = 0; i < cbl_pop1hrdr.Items.Count; i++)
            {
                if (cbl_pop1hrdr.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + cbl_pop1hrdr.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "'" + "," + "'" + cbl_pop1hrdr.Items[i].Value.ToString() + "";
                    }
                }
            }


            for (int i = 0; i < cbl_ldgr.Items.Count; i++)
            {
                if (cbl_ldgr.Items[i].Selected == true)
                {
                    if (ledgercode == "")
                    {
                        ledgercode = "" + cbl_ldgr.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        ledgercode = ledgercode + "'" + "," + "'" + cbl_ldgr.Items[i].Value.ToString() + "";
                    }
                }
            }


            string query = "SELECT LedgerPK,LedgerName,i.HeaderFK,HeaderName FROM FM_LedgerMaster I,FM_HeaderMaster H where i.HeaderFK = h.HeaderPK and I.HeaderFK IN ('" + itemheadercode + "') and i.LedgerPK IN('" + ledgercode + "')  order by isnull(i.priority,1000), i.ledgerName asc ";
            ds = da.select_method_wo_parameter(query, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    dr = dt.NewRow();
                    dr[0] = Convert.ToString(ds.Tables[0].Rows[i]["HeaderName"]);
                    dr[1] = Convert.ToString(ds.Tables[0].Rows[i]["HeaderFK"]);
                    dr[2] = Convert.ToString(ds.Tables[0].Rows[i]["LedgerName"]);
                    dr[3] = Convert.ToString(ds.Tables[0].Rows[i]["LedgerPK"]);

                    dt.Rows.Add(dr);
                }

                if (dt.Rows.Count > 0)
                {
                    gridView3.DataSource = dt;
                    gridView3.DataBind();
                }

            }

        }
        catch
        {
        }
    }


    public void headerbind()
    {
        try
        {
            cbl_pop1hrdr.Items.Clear();

            //string query = "SELECT DISTINCT H.Header_ID,Header_Name FROM AcctHeader H,AcctInfo I WHERE I.Acct_ID = H.Acct_ID AND I.College_Code =13 AND H.acct_id = 78 ORDER BY header_name ";
            string query = "SELECT distinct HeaderPK,HeaderName FROM FM_HeaderMaster L,FS_HeaderPrivilage P WHERE L.HeaderPK = P.HeaderFK AND P.CollegeCode = L.CollegeCode AND P.UserCode = " + usercode + " AND L.CollegeCode='" + collhead + "' order by HeaderName";
            ds = da.select_method_wo_parameter(query, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_pop1hrdr.DataSource = ds;
                cbl_pop1hrdr.DataTextField = "HeaderName";
                cbl_pop1hrdr.DataValueField = "HeaderPK";
                cbl_pop1hrdr.DataBind();

                cbl_header.DataSource = ds;
                cbl_header.DataTextField = "HeaderName";
                cbl_header.DataValueField = "HeaderPK";
                cbl_header.DataBind();

                chk_header1.Checked = true;
                if (cbl_header.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_header.Items.Count; i++)
                    {
                        cbl_header.Items[i].Selected = true;
                    }
                    txt_header.Text = "Header Name(" + cbl_header.Items.Count + ")";
                }

                cb_pop1header.Checked = true;
                if (cbl_pop1hrdr.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_pop1hrdr.Items.Count; i++)
                    {
                        cbl_pop1hrdr.Items[i].Selected = true;
                    }
                }
            }
            else
            {
                cbl_header.Items.Clear();
                txt_header.Text = "--Select--";
            }
        }

        catch
        {
        }
    }

    public void ledgerbind()
    {
        try
        {
            string itemheadercode = "";
            for (int i = 0; i < cbl_pop1hrdr.Items.Count; i++)
            {
                if (cbl_pop1hrdr.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + cbl_pop1hrdr.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "'" + "," + "'" + cbl_pop1hrdr.Items[i].Value.ToString() + "";
                    }
                }
            }

            ds.Clear();
            cbl_ldgr.Items.Clear();

            //string query = "SELECT Fee_Code,Fee_Type FROM fee_info I,acctheader H WHERE I.header_id = H.header_id AND I.header_id IN ('" + itemheadercode + "') and  Fee_Type NOT IN ('Cash','Income & Expenditure','Misc','Excess Amount','Fine') AND Fee_Type NOT IN (SELECT BankName FROM Bank_Master1) ORDER BY Fee_Type";

            string query = "select distinct l.LedgerPK,l.LedgerAcr,l.LedgerName,isnull(l.priority,1000),l.FinGroupFK,case when l.LedgerType ='1' then 'Term Fee' else 'Other Fee' end as LedgerType,l.HeaderFK,case when l.LedgerMode='1' then 'Cr' else 'Dr' end as LedgerMode ,l.Purpose,h.HeaderName,h.HeaderPK,fg.GroupName  from FM_LedgerMaster l,FM_HeaderMaster h,FM_FinGroupMaster fg where l.HeaderFK =h.HeaderPK and fg.FinGroupPK =l.FinGroupFK and l.CollegeCode='" + collhead + "' and HeaderFK in('" + itemheadercode + "')  order by isnull(l.priority,1000), l.ledgerName asc ";

            ds = da.select_method_wo_parameter(query, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_ldgr.DataSource = ds;
                cbl_ldgr.DataTextField = "LedgerName";
                cbl_ldgr.DataValueField = "LedgerPK";
                cbl_ldgr.DataBind();


                if (cbl_ldgr.Items.Count > 0)
                {
                    if (cb_ldgr.Checked == true)
                    {
                        for (int i = 0; i < cbl_ldgr.Items.Count; i++)
                        {
                            cbl_ldgr.Items[i].Selected = true;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < cbl_ldgr.Items.Count; i++)
                        {
                            cbl_ldgr.Items[i].Selected = false;
                        }
                    }

                }

            }



        }

        catch
        {
        }
    }

    public void headerpopbind()
    {
        try
        {
            cbl_pop1hrdr.Items.Clear();

            //string query = "SELECT DISTINCT H.Header_ID,Header_Name FROM AcctHeader H,AcctInfo I WHERE I.Acct_ID = H.Acct_ID AND I.College_Code =13 AND H.acct_id = 78 ORDER BY header_name ";
            string query = "SELECT distinct HeaderPK,HeaderName FROM FM_HeaderMaster L,FS_HeaderPrivilage P WHERE L.HeaderPK = P.HeaderFK AND P.CollegeCode = L.CollegeCode AND P.UserCode = " + usercode + " AND L.CollegeCode='" + colpop + "' order by HeaderName";
            ds = da.select_method_wo_parameter(query, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_pop1hrdr.DataSource = ds;
                cbl_pop1hrdr.DataTextField = "HeaderName";
                cbl_pop1hrdr.DataValueField = "HeaderPK";
                cbl_pop1hrdr.DataBind();

                cbl_header.DataSource = ds;
                cbl_header.DataTextField = "HeaderName";
                cbl_header.DataValueField = "HeaderPK";
                cbl_header.DataBind();

                chk_header1.Checked = true;
                if (cbl_header.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_header.Items.Count; i++)
                    {
                        cbl_header.Items[i].Selected = true;
                    }
                    txt_header.Text = "Header Name(" + cbl_header.Items.Count + ")";
                }

                cb_pop1header.Checked = true;
                if (cbl_pop1hrdr.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_pop1hrdr.Items.Count; i++)
                    {
                        cbl_pop1hrdr.Items[i].Selected = true;
                    }
                }
            }
            else
            {
                cbl_header.Items.Clear();
                txt_header.Text = "--Select--";
            }
        }

        catch
        {
        }
    }

    public void ledgerpopbind()
    {
        try
        {
            string itemheadercode = "";
            for (int i = 0; i < cbl_pop1hrdr.Items.Count; i++)
            {
                if (cbl_pop1hrdr.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + cbl_pop1hrdr.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "'" + "," + "'" + cbl_pop1hrdr.Items[i].Value.ToString() + "";
                    }
                }
            }

            ds.Clear();
            cbl_ldgr.Items.Clear();

            //string query = "SELECT Fee_Code,Fee_Type FROM fee_info I,acctheader H WHERE I.header_id = H.header_id AND I.header_id IN ('" + itemheadercode + "') and  Fee_Type NOT IN ('Cash','Income & Expenditure','Misc','Excess Amount','Fine') AND Fee_Type NOT IN (SELECT BankName FROM Bank_Master1) ORDER BY Fee_Type";

            string query = "select distinct l.LedgerPK,l.LedgerAcr,l.LedgerName,isnull(l.priority,1000),l.FinGroupFK,case when l.LedgerType ='1' then 'Term Fee' else 'Other Fee' end as LedgerType,l.HeaderFK,case when l.LedgerMode='1' then 'Cr' else 'Dr' end as LedgerMode ,l.Purpose,h.HeaderName,h.HeaderPK,fg.GroupName  from FM_LedgerMaster l,FM_HeaderMaster h,FM_FinGroupMaster fg where l.HeaderFK =h.HeaderPK and fg.FinGroupPK =l.FinGroupFK and l.CollegeCode='" + colpop + "' and HeaderFK in('" + itemheadercode + "') order by isnull(l.priority,1000), l.ledgerName asc";

            ds = da.select_method_wo_parameter(query, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_ldgr.DataSource = ds;
                cbl_ldgr.DataTextField = "LedgerName";
                cbl_ldgr.DataValueField = "LedgerPK";
                cbl_ldgr.DataBind();


                if (cbl_ldgr.Items.Count > 0)
                {
                    if (cb_ldgr.Checked == true)
                    {
                        for (int i = 0; i < cbl_ldgr.Items.Count; i++)
                        {
                            cbl_ldgr.Items[i].Selected = true;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < cbl_ldgr.Items.Count; i++)
                        {
                            cbl_ldgr.Items[i].Selected = false;
                        }
                    }
                }
            }
        }

        catch
        {

        }
    }

    public void ledgerbind1()
    {
        try
        {
            txt_ledger.Text = "--Select--";
            string itemheadercode = "";
            for (int i = 0; i < cbl_header.Items.Count; i++)
            {
                if (cbl_header.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + cbl_header.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "'" + "," + "'" + cbl_header.Items[i].Value.ToString() + "";
                    }
                }
            }


            cbl_ledger1.Items.Clear();

            string query = "select distinct l.LedgerPK,l.LedgerAcr,l.LedgerName,isnull(l.priority,1000),l.FinGroupFK,case when l.LedgerType ='1' then 'Term Fee' else 'Other Fee' end as LedgerType,l.HeaderFK,case when l.LedgerMode='1' then 'Cr' else 'Dr' end as LedgerMode ,l.Purpose,h.HeaderName,h.HeaderPK,fg.GroupName  from FM_LedgerMaster l,FM_HeaderMaster h,FM_FinGroupMaster fg where l.HeaderFK =h.HeaderPK and fg.FinGroupPK =l.FinGroupFK and l.CollegeCode='" + collhead + "' and HeaderFK in('" + itemheadercode + "') order by isnull(l.priority,1000), l.ledgerName asc";
            ds = da.select_method_wo_parameter(query, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_ledger1.DataSource = ds;
                cbl_ledger1.DataTextField = "LedgerName";
                cbl_ledger1.DataValueField = "LedgerPK";
                cbl_ledger1.DataBind();

                cb_ledgr.Checked = true;
                if (cbl_ledger1.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_ledger1.Items.Count; i++)
                    {
                        cbl_ledger1.Items[i].Selected = true;
                    }
                    txt_ledger.Text = "Ledger Name(" + cbl_ledger1.Items.Count + ")";
                }


            }



        }

        catch
        {
        }
    }

    public void loadcoldept()
    {
        try
        {
            ds.Clear();
            ddlcolload.Items.Clear();
            string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcolload.DataSource = ds;
                ddlcolload.DataTextField = "collname";
                ddlcolload.DataValueField = "college_code";
                ddlcolload.DataBind();
            }
        }
        catch
        {
        }
    }

    public void loadcolhead()
    {
        try
        {
            ds.Clear();
            ddlheadcol.Items.Clear();
            string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlheadcol.DataSource = ds;
                ddlheadcol.DataTextField = "collname";
                ddlheadcol.DataValueField = "college_code";
                ddlheadcol.DataBind();
            }
        }
        catch
        {
        }
    }

    public void loadpopcol()
    {
        try
        {
            ds.Clear();
            ddlpopcol.Items.Clear();
            string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlpopcol.DataSource = ds;
                ddlpopcol.DataTextField = "collname";
                ddlpopcol.DataValueField = "college_code";
                ddlpopcol.DataBind();
            }
        }
        catch
        {
        }
    }

    public void cb_ledgr_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_ledgr.Checked == true)
            {
                for (int i = 0; i < cbl_ledger1.Items.Count; i++)
                {
                    cbl_ledger1.Items[i].Selected = true;
                }
                txt_ledger.Text = "Ledger Name(" + (cbl_ledger1.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_ledger1.Items.Count; i++)
                {
                    cbl_ledger1.Items[i].Selected = false;
                }
                txt_ledger.Text = "--Select--";
            }


        }
        catch (Exception ex)
        {

        }
    }
    public void cbl_ledger_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            txt_ledger.Text = "--Select--";
            cb_ledgr.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_ledger1.Items.Count; i++)
            {
                if (cbl_ledger1.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_ledger.Text = "Ledger Name(" + commcount.ToString() + ")";
                if (commcount == cbl_ledger1.Items.Count)
                {
                    cb_ledgr.Checked = true;
                }
            }

        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlcolload_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            binddepartment();
            btn_go_Click(sender, e);
        }
        catch
        {

        }
    }

    protected void ddlheadcol_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            headerbind();
            ledgerbind();
            ledgerbind1();
            btn_go1_Click(sender, e);
        }
        catch
        {

        }
    }

    protected void ddlpopcol_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //btn_addnew_Click(sender, e);

            string linkval = d2.getCurrentFinanceYear(usercode, colpop);
            string seldate = "select Convert(varchar(10),FinYearStart,103) as FinYearStart,Convert(varchar(10),FinYearEnd,103) as FinYearEnd from FM_FinYearMaster where FinYearPK='" + linkval + "' and CollegeCode='" + colpop + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(seldate, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txt_popfdate.Text = Convert.ToString(ds.Tables[0].Rows[0]["FinYearStart"]);
                    txt_poptdate.Text = Convert.ToString(ds.Tables[0].Rows[0]["FinYearEnd"]);
                }
                else
                {
                    txt_popfdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txt_poptdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                bindheader();
                binditem();
                bindpopdept();
                loaddesc();
                headerpopbind();
                ledgerpopbind();
                gridView2.Visible = false;
                gridView1.Visible = false;
                gridView3.Visible = false;
            }
        }
        catch
        {

        }
    }

    protected void ddl_hdr_ldg_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_hdr_ldg.SelectedValue == "0")
        {
            txt_search1.Visible = true;
            txt_ldg_search.Visible = false;
            txt_ldg_search.Text = "";
        }
        else if (ddl_hdr_ldg.SelectedValue == "1")
        {
            txt_search1.Visible = false;
            txt_ldg_search.Visible = true;
            txt_search1.Text = "";
        }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getheader(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "SELECT DISTINCT HeaderName FROM FM_HeaderMaster WHERE HeaderName like '" + prefixText + "%' and CollegeCode='" + collhead + "'";
        // string query = "SELECT DISTINCT Header_Name FROM AcctHeader H,AcctInfo I WHERE I.Acct_ID = H.Acct_ID AND I.College_Code =13 AND H.acct_id = 78 ORDER BY header_name like '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getledger(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "SELECT LedgerName FROM FM_LedgerMaster where  LedgerName like '" + prefixText + "%' and CollegeCode='" + collhead + "' order by isnull(l.priority,1000), l.ledgerName asc";

        name = ws.Getname(query);
        return name;
    }

    protected void uggirdrowcommand(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddl = (DropDownList)e.Row.Cells[1].FindControl("ddl_pur");
                string subjquery = "select TextCode,textval from textvaltable where TextCriteria='FiBud' and textval<>'' and college_code ='" + colpop + "' order by textval";
                ds1.Clear();
                ds1 = d2.select_method_wo_parameter(subjquery, "Text");
                if (ds1.Tables[0].Rows.Count > 0)
                {

                    ddl.DataSource = ds1;
                    ddl.DataTextField = "textval";
                    ddl.DataValueField = "TextCode";
                    ddl.DataBind();
                    ddl.Items.Insert(0, "Select");
                }
                else
                {
                    ddl.Items.Insert(0, "Select");
                }
            }
        }
        catch
        {

        }
    }

    protected void uggirdrowcommand2(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddl = (DropDownList)e.Row.Cells[1].FindControl("ddl_pur2");
                string subjquery = "select TextCode,textval from textvaltable where TextCriteria='FiBud' and textval<>'' and college_code ='" + colpop + "' order by textval";
                ds.Clear();
                ds = d2.select_method_wo_parameter(subjquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl.DataSource = ds;
                    ddl.DataTextField = "textval";
                    ddl.DataValueField = "TextCode";
                    ddl.DataBind();
                    ddl.Items.Insert(0, "Select");
                }
                else
                {
                    ddl.Items.Insert(0, "Select");
                }
            }
        }
        catch
        {

        }
    }

    protected void uggirdrowcommand1(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddl = (DropDownList)e.Row.Cells[1].FindControl("ddl_pur1");
                string subjquery = "select TextCode,textval from textvaltable where TextCriteria='FiBud' and textval<>'' and college_code ='" + colpop + "' order by textval";
                ds.Clear();
                ds = d2.select_method_wo_parameter(subjquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {

                    ddl.DataSource = ds;
                    ddl.DataTextField = "textval";
                    ddl.DataValueField = "TextCode";
                    ddl.DataBind();

                    ddl.Items.Insert(0, "Select");
                }
                else
                {
                    ddl.Items.Insert(0, "Select");
                }
            }
        }
        catch
        {

        }
    }

    public string gettext(string text)
    {
        string purpose_val = d2.GetFunction("select TextVal  from textvaltable where TextCriteria='FiBud' and TextCode ='" + text + "'");
        return purpose_val;
    }


    public string getdepartment(string dept)
    {
        string appno = d2.GetFunction("select Dept_Name  from Department where Dept_Code ='" + dept + "'");
        return appno;
    }

    public string getitemname(string itemname)
    {
        string item = d2.GetFunction("select itemname from IM_ItemMaster where itemcode='" + itemname + "'");

        return item;
    }

    public string getitemnameheadcode(string itemheadname)
    {
        string item = d2.GetFunction("select ItemCode from IM_ItemMaster where ItemHeaderName='" + itemheadname + "'");

        return item;
    }
    public string getitemcode(string itemcode)
    {
        string item = d2.GetFunction("select itemcode from IM_ItemMaster where itemname='" + itemcode + "'");
        return item;
    }
    public string getitemnameheadname(string itemheadcode)
    {
        string item = d2.GetFunction("select ItemHeaderName from IM_ItemMaster where ItemHeaderCode='" + itemheadcode + "'");
        return item;
    }
    public string getheadername(string headername)
    {
        string name = d2.GetFunction("SELECT DISTINCT HeaderName from FM_HeaderMaster where HeaderPK='" + headername + "' and CollegeCode='" + collegecode1 + "'");
        return name;
    }

    public string getledgername(string ledgername)
    {
        string name = d2.GetFunction("Select Distinct LedgerName from FM_LedgerMaster where LedgerPK='" + ledgername + "' and CollegeCode='" + collegecode1 + "'");
        return name;
    }

    public string getheadercode(string headername)
    {
        string name = d2.GetFunction("SELECT HeaderPK FROM FM_HeaderMaster where HeaderName='" + headername + "' AND CollegeCode='" + collegecode1 + "'");
        return name;
    }

    public string gedeptcode(string dept)
    {

        string appno = d2.GetFunction("select Dept_Code  from Department where Dept_Name ='" + dept + "'");
        return appno;

    }
    public string getledgercode(string ledgername)
    {
        string name = d2.GetFunction(" SELECT LedgerPK FROM FM_LedgerMaster WHERE LedgerName='" + ledgername + "' AND CollegeCode='" + collegecode1 + "'");
        return name;
    }
    public void CheckBox_column_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (CheckBox_column.Checked == true)
            {
                ItemList.Clear();
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    string si = Convert.ToString(i);
                    cblcolumnorder.Items[i].Selected = true;
                    // //LinkButton1.Visible = true;
                    ItemList.Add(cblcolumnorder.Items[i].Text.ToString());
                    Itemindex.Add(si);
                }
                // //LinkButton1.Visible = true;
                tborder.Visible = true;
                tborder.Text = "";
                int j = 0;
                for (int i = 0; i < ItemList.Count; i++)
                {
                    j = j + 1;
                    tborder.Text = tborder.Text + ItemList[i].ToString();

                    tborder.Text = tborder.Text + "(" + (j).ToString() + ")  ";

                }

            }
            else
            {
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = false;
                    //LinkButton1.Visible = false;
                    ItemList.Clear();
                    Itemindex.Clear();
                    cblcolumnorder.Items[0].Enabled = false;
                }

                tborder.Text = "";
                tborder.Visible = false;

            }

        }
        catch (Exception ex)
        {

        }
    }
    public void cblcolumnorder_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox_column.Checked = false;
            string value = "";
            int index;
            cblcolumnorder.Items[0].Selected = true;
            cblcolumnorder.Items[0].Enabled = false;
            value = string.Empty;
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index);
            if (cblcolumnorder.Items[index].Selected)
            {
                if (!Itemindex.Contains(sindex))
                {
                    if (tborder.Text == "")
                    {
                        ItemList.Add("Roll No");
                    }

                    ItemList.Add(cblcolumnorder.Items[index].Text.ToString());
                    Itemindex.Add(sindex);
                }
            }
            else
            {
                ItemList.Remove(cblcolumnorder.Items[index].Text.ToString());
                Itemindex.Remove(sindex);
            }
            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
            {

                if (cblcolumnorder.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList.Remove(cblcolumnorder.Items[i].Text.ToString());
                    Itemindex.Remove(sindex);

                }
            }

            // //LinkButton1.Visible = true;
            tborder.Visible = true;
            tborder.Text = "";
            for (int i = 0; i < ItemList.Count; i++)
            {
                tborder.Text = tborder.Text + ItemList[i].ToString();

                tborder.Text = tborder.Text + "(" + (i + 1).ToString() + ")  ";

            }
            if (ItemList.Count == 22)
            {
                CheckBox_column.Checked = true;
            }
            if (ItemList.Count == 0)
            {
                tborder.Visible = false;
                //LinkButton1.Visible = false;
            }

            //  Button2.Focus();
        }
        catch (Exception ex)
        {

        }
    }

    public void LinkButtonsremove1_Click(object sender, EventArgs e)
    {
        try
        {
            cblcolumnorder.ClearSelection();
            CheckBox_column.Checked = false;
            //LinkButton1.Visible = false;
            //cblcolumnorder.Items[0].Selected = true;
            ItemList.Clear();
            Itemindex.Clear();
            tborder.Text = "";
            tborder.Visible = false;
            //Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }

    public void CheckBox_column1_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (CheckBox_column1.Checked == true)
            {
                ItemList1.Clear();
                for (int i = 0; i < cblcolumnorder1.Items.Count; i++)
                {
                    string si = Convert.ToString(i);
                    cblcolumnorder1.Items[i].Selected = true;
                    LinkButton3.Visible = true;
                    ItemList1.Add(cblcolumnorder1.Items[i].Text.ToString());
                    Itemindex1.Add(si);
                }
                LinkButton3.Visible = true;
                tborder1.Visible = true;
                tborder1.Text = "";
                int j = 0;
                for (int i = 0; i < ItemList1.Count; i++)
                {
                    j = j + 1;
                    tborder1.Text = tborder1.Text + ItemList1[i].ToString();

                    tborder1.Text = tborder1.Text + "(" + (j).ToString() + ")  ";

                }

            }
            else
            {
                for (int i = 0; i < cblcolumnorder1.Items.Count; i++)
                {
                    cblcolumnorder1.Items[i].Selected = false;
                    LinkButton3.Visible = false;
                    ItemList1.Clear();
                    Itemindex1.Clear();
                    cblcolumnorder1.Items[0].Enabled = false;
                }

                tborder1.Text = "";
                tborder1.Visible = false;

            }

        }
        catch (Exception ex)
        {

        }
    }
    public void cblcolumnorder1_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            CheckBox_column1.Checked = false;
            string value = "";
            int index;
            cblcolumnorder1.Items[0].Selected = true;
            cblcolumnorder1.Items[0].Enabled = false;
            value = string.Empty;
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index);
            if (cblcolumnorder1.Items[index].Selected)
            {
                if (!Itemindex1.Contains(sindex))
                {
                    if (tborder1.Text == "")
                    {
                        ItemList1.Add("Roll No");
                    }

                    ItemList1.Add(cblcolumnorder1.Items[index].Text.ToString());
                    Itemindex1.Add(sindex);
                }
            }
            else
            {
                ItemList1.Remove(cblcolumnorder1.Items[index].Text.ToString());
                Itemindex1.Remove(sindex);
            }
            for (int i = 0; i < cblcolumnorder1.Items.Count; i++)
            {

                if (cblcolumnorder1.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList1.Remove(cblcolumnorder1.Items[i].Text.ToString());
                    Itemindex1.Remove(sindex);

                }
            }

            LinkButton3.Visible = true;
            tborder1.Visible = true;
            tborder1.Text = "";
            for (int i = 0; i < ItemList1.Count; i++)
            {
                tborder1.Text = tborder1.Text + ItemList1[i].ToString();

                tborder1.Text = tborder1.Text + "(" + (i + 1).ToString() + ")  ";

            }
            if (ItemList1.Count == 22)
            {
                CheckBox_column1.Checked = true;
            }
            if (ItemList1.Count == 0)
            {
                tborder1.Visible = false;
                LinkButton3.Visible = false;
            }


        }
        catch (Exception ex)
        {

        }
    }
    public void LinkButtonsremove2_Click(object sender, EventArgs e)
    {
        try
        {
            cblcolumnorder1.ClearSelection();
            CheckBox_column1.Checked = false;
            LinkButton3.Visible = false;

            ItemList1.Clear();
            Itemindex1.Clear();
            tborder1.Text = "";
            tborder1.Visible = false;
        }
        catch (Exception ex)
        {
        }

    }

    public void CheckBox_column2_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (CheckBox_column2.Checked == true)
            {
                ItemList2.Clear();
                for (int i = 0; i < cblcolumnorder2.Items.Count; i++)
                {
                    string si = Convert.ToString(i);
                    cblcolumnorder2.Items[i].Selected = true;
                    LinkButton4.Visible = true;
                    ItemList2.Add(cblcolumnorder2.Items[i].Text.ToString());
                    Itemindex2.Add(si);
                }
                LinkButton4.Visible = true;
                tborder2.Visible = true;
                tborder2.Text = "";
                int j = 0;
                for (int i = 0; i < ItemList2.Count; i++)
                {
                    j = j + 1;
                    tborder2.Text = tborder2.Text + ItemList2[i].ToString();

                    tborder2.Text = tborder2.Text + "(" + (j).ToString() + ")  ";

                }

            }
            else
            {
                for (int i = 0; i < cblcolumnorder2.Items.Count; i++)
                {
                    cblcolumnorder2.Items[i].Selected = false;
                    LinkButton4.Visible = false;
                    ItemList2.Clear();
                    Itemindex2.Clear();
                    cblcolumnorder2.Items[0].Enabled = false;
                }

                tborder2.Text = "";
                tborder2.Visible = false;
            }
        }
        catch
        {
        }
    }
    public void cblcolumnorder2_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            CheckBox_column2.Checked = false;
            string value = "";
            int index;
            cblcolumnorder2.Items[0].Selected = true;
            cblcolumnorder2.Items[0].Enabled = false;
            value = string.Empty;
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index);
            if (cblcolumnorder2.Items[index].Selected)
            {
                if (!Itemindex2.Contains(sindex))
                {
                    if (tborder2.Text == "")
                    {
                        ItemList2.Add("Roll No");
                    }

                    ItemList2.Add(cblcolumnorder2.Items[index].Text.ToString());
                    Itemindex2.Add(sindex);
                }
            }
            else
            {
                ItemList2.Remove(cblcolumnorder2.Items[index].Text.ToString());
                Itemindex2.Remove(sindex);
            }
            for (int i = 0; i < cblcolumnorder2.Items.Count; i++)
            {

                if (cblcolumnorder2.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList2.Remove(cblcolumnorder2.Items[i].Text.ToString());
                    Itemindex2.Remove(sindex);

                }
            }

            LinkButton4.Visible = true;
            tborder2.Visible = true;
            tborder2.Text = "";
            for (int i = 0; i < ItemList2.Count; i++)
            {
                tborder2.Text = tborder2.Text + ItemList2[i].ToString();

                tborder2.Text = tborder2.Text + "(" + (i + 1).ToString() + ")  ";

            }
            if (ItemList2.Count == 22)
            {
                CheckBox_column2.Checked = true;
            }
            if (ItemList2.Count == 0)
            {
                tborder2.Visible = false;
                LinkButton4.Visible = false;
            }


        }
        catch (Exception ex)
        {

        }
    }
    public void LinkButtonsremove3_Click(object sendre, EventArgs e)
    {
        try
        {
            cblcolumnorder2.ClearSelection();
            CheckBox_column2.Checked = false;
            LinkButton4.Visible = false;

            ItemList2.Clear();
            Itemindex2.Clear();
            tborder2.Text = "";
            tborder2.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    public void FpSpread1_CellClick(object sender, EventArgs e)
    {
        try
        {
            Cellclick = true;
            popup_addnew.Visible = true;
            btn_save.Visible = false;
            btn_exit.Visible = false;
            gridView3.Visible = true;
        }
        catch
        {
        }
    }

    public void FpSpread1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cellclick == true)
        {
            try
            {
                rb_hdr.Checked = true;
                rb_dep.Checked = false;
                rb_depitem.Checked = false;
                div_hdrwithledgr.Visible = true;
                div_depwithitem.Visible = false;
                div_depwithitem.Visible = false;
                div_rdo1.Visible = false;

                gridView3.Visible = true;
                gridView2.Visible = false;
                gridView1.Visible = false;
                btn_save.Visible = false;
                btn_exit.Visible = false;
                btn_update.Visible = true;
                btn_del.Visible = true;
                btn_exit1.Visible = true;
                txt_popfdate.Text = txt_frmdate.Text;
                txt_poptdate.Text = txt_todate.Text;
                ddlpopcol.Enabled = false;
                txt_popfdate.Enabled = false;
                txt_poptdate.Enabled = false;
                Panel6.Enabled = false;
                Panel7.Enabled = false;
                rb_hdr.Enabled = true;
                rb_depitem.Enabled = false;
                rb_dep.Enabled = false;
                btn_pop1go.Enabled = false;

                string activerow = "";
                activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
                string val = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                valcode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                string val1 = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
                string val1code = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                string colcode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
                ddlpopcol.SelectedItem.Value = colcode;
                ViewState["Valcode"] = Convert.ToString(valcode);
                string header_code = getheadercode(val);
                string feecode = getledgercode(val1);
                string fee = d2.GetFunction("select LedgerPK from FM_LedgerMaster i,FM_HeaderMaster h where i.HeaderFK='" + header_code + "' and LedgerName='" + val1 + "' and i.CollegeCode='" + colcode + "'");

                string query_select = "SELECT i.LedgerPK,LedgerName,i.HeaderFK,HeaderName,fd.BudgetAmt,fd.BudgetQty,fd.Purpose,fd.Allow_Aditional  FROM FM_LedgerMaster I,FM_HeaderMaster H ,FinBudget f,FinBudgetDet fd where i.HeaderFK = h.HeaderPK and f.FinBudget =fd.FinBudget and fd.Header_Id =h.HeaderPK and i.LedgerPK =fd.Fee_Code and f.FinBudget ='" + valcode + "' and f.CollegeCode='" + colcode + "' order by isnull(i.priority,1000), i.ledgerName asc";

                // string query_select = " select Header_Id,Fee_Code,BudgetQty,BudgetAmt,Purpose,Allow_Aditional from FinBudgetDet where Header_Id='" + header_code + "' and Fee_Code='" + feecode + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(query_select, "Text");
                ds1.Clear();
                ds1 = d2.select_method_wo_parameter(query_select, "Text");
                ds2.Clear();
                ds2 = d2.select_method_wo_parameter(query_select, "Text");

                DataTable dt = new DataTable();
                DataRow dr;
                ArrayList addnew = new ArrayList();
                ArrayList addnew1 = new ArrayList();
                dt.Columns.Add("Header");
                dt.Columns.Add("Header_Code");
                dt.Columns.Add("Ledger");
                dt.Columns.Add("Ledger_code");
                dt.Columns.Add("BudQty");
                dt.Columns.Add("BudAmt");
                dt.Columns.Add("Purpose");
                dt.Columns.Add("Allow Additional");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string fee_code = Convert.ToString(ds.Tables[0].Rows[i]["LedgerPK"]);
                        string head_code = Convert.ToString(ds.Tables[0].Rows[i]["HeaderFK"]);
                        addnew1.Add(head_code);
                        addnew.Add(fee_code);

                        dr = dt.NewRow();
                        dr[0] = Convert.ToString(ds.Tables[0].Rows[i]["HeaderName"]);
                        dr[1] = Convert.ToString(ds.Tables[0].Rows[i]["HeaderFK"]);
                        dr[2] = Convert.ToString(ds.Tables[0].Rows[i]["LedgerName"]);
                        dr[3] = Convert.ToString(ds.Tables[0].Rows[i]["LedgerPK"]);
                        dr[4] = Convert.ToString(ds.Tables[0].Rows[i]["BudgetQty"]);
                        dr[5] = Convert.ToString(ds.Tables[0].Rows[i]["BudgetAmt"]);
                        dr[6] = Convert.ToString(ds.Tables[0].Rows[i]["Purpose"]);
                        dr[7] = Convert.ToString(ds.Tables[0].Rows[i]["Allow_Aditional"]);

                        dt.Rows.Add(dr);
                    }

                    if (dt.Rows.Count > 0)
                    {
                        gridView3.DataSource = dt;
                        gridView3.DataBind();
                    }

                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string head_name = Convert.ToString(ds.Tables[0].Rows[i]["HeaderName"]);
                        string led_name = Convert.ToString(ds.Tables[0].Rows[i]["LedgerName"]);
                        for (int k = 0; k < cbl_pop1hrdr.Items.Count; k++)
                        {
                            if (addnew1.Contains(cbl_pop1hrdr.Items[k].Value))
                            {
                                cbl_pop1hrdr.Items[k].Selected = true;
                            }
                            else
                            {
                                cbl_pop1hrdr.Items[k].Selected = false;

                            }
                        }
                        ledgerbind();

                        for (int k = 0; k < cbl_ldgr.Items.Count; k++)
                        {
                            if (addnew.Contains(cbl_ldgr.Items[k].Value))
                            {
                                cbl_ldgr.Items[k].Selected = true;
                            }
                            else
                            {
                                cbl_ldgr.Items[k].Selected = false;

                            }
                        }
                    }
                }

                if (ds1.Tables[0].Rows.Count > 0)
                {
                    DropDownList ddlpur = new DropDownList();
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        for (int j = 0; j < gridView3.Rows.Count; j++)
                        {
                            if (i == j)
                            {
                                string ddlval = Convert.ToString(ds1.Tables[0].Rows[i]["Purpose"]);
                                ddlpur = (DropDownList)gridView3.Rows[j].Cells[5].FindControl("ddl_pur");
                                if (ddlval.Trim() != "0")
                                {
                                    ddlpur.SelectedValue = ddlval;
                                }
                            }
                        }
                    }
                }

                if (ds2.Tables[0].Rows.Count > 0)
                {
                    CheckBox chkadd = new CheckBox();
                    for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                    {
                        for (int j = 0; j < gridView3.Rows.Count; j++)
                        {
                            if (i == j)
                            {
                                string chkval = Convert.ToString(ds2.Tables[0].Rows[i]["Allow_Aditional"]);
                                chkadd = (CheckBox)gridView3.Rows[j].Cells[6].FindControl("cb_allo");
                                if (chkval.Trim() != "False")
                                {
                                    chkadd.Checked = true;
                                }
                            }
                        }
                    }
                }

            }

            catch
            {

            }
        }
    }

    public void btn_update_Click(object sender, EventArgs e)
    {
        try
        {
            int type = 0;
            string firstdate = Convert.ToString(txt_popfdate.Text);
            string todate = Convert.ToString(txt_poptdate.Text);
            DateTime dt = new DateTime();
            DateTime dt1 = new DateTime();
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);

            string[] split1 = todate.Split('/');
            dt1 = Convert.ToDateTime(split1[1] + "/" + split1[0] + "/" + split1[2]);
            if (rb_dep.Checked == true)
            {
                type = 1;
            }
            else if (rb_depitem.Checked == true)
            {
                type = 2;
            }
            else if (rb_hdr.Checked == true)
            {
                type = 3;

            }
            if (rb_hdr.Checked == true)
            {
                string activerow = "";
                activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
                string val = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                valcode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                string colcode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
                if (ViewState["Valcode"] != null)
                {
                    valcode = Convert.ToString(ViewState["Valcode"]);
                }
                string deletequery = "delete FinBudgetDet where FinBudget='" + valcode + "'";
                int up = d2.update_method_wo_parameter(deletequery, "Text");

                for (int i = 0; i < gridView3.Rows.Count; i++)
                {
                    TextBox txtamt = (TextBox)gridView3.Rows[i].FindControl("txt_amt");
                    amount = Convert.ToDecimal(txtamt.Text);


                    TextBox txtqty = (TextBox)gridView3.Rows[i].FindControl("txt_bqty");
                    qty = Convert.ToDecimal(txtqty.Text);

                    DropDownList ddlpur = (DropDownList)gridView3.Rows[i].FindControl("ddl_pur");
                    if (ddlpur.SelectedItem.Text != "Select")
                    {
                        purpose = Convert.ToInt32(ddlpur.SelectedItem.Value);
                    }
                    else
                    {
                        purpose = 0;
                    }


                    CheckBox chkItemHeader = (CheckBox)gridView3.Rows[i].FindControl("cb_allo");

                    Label txtheader = (Label)gridView3.Rows[i].FindControl("txt_hdr_code");
                    header = Convert.ToInt16(txtheader.Text);

                    Label txtfeecode = (Label)gridView3.Rows[i].FindControl("txt_ldgcode");
                    itemcode = Convert.ToInt16(txtfeecode.Text);



                    if (chkItemHeader.Checked == true)
                    {
                        allowchk = "1";

                    }
                    else
                    {
                        allowchk = "0";
                    }

                    string query1 = "insert into FinBudgetDet(FinBudget,Header_Id,Fee_Code,BudgetQty,BudgetAmt,Purpose,Allow_Aditional)values('" + valcode + "','" + header + "','" + itemcode + "','" + qty + "','" + amount + "','" + purpose + "','" + allowchk + "')";
                    d2.update_method_wo_parameter(query1, "Text");

                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Updated Successfully";
                }
            }
            else if (rb_dep.Checked == true)
            {
                string department = "";
                string amt = "";
                string q = "";
                string dep = "";
                string activerow = "";
                activerow = Fpspread2.ActiveSheetView.ActiveRow.ToString();
                string val = Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                valcode = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);

                if (ViewState["Valcode"] != null)
                {
                    valcode = Convert.ToString(ViewState["Valcode"]);
                }

                string deletequery = "delete FinBudgetDet where FinBudget='" + valcode + "'";
                int up = d2.update_method_wo_parameter(deletequery, "Text");

                for (int i = 0; i < gridView2.Rows.Count; i++)
                {
                    Label dept = (Label)gridView2.Rows[i].FindControl("lbl_dept");
                    department = Convert.ToString(dept.Text);

                    dep = d2.GetFunction("select Dept_Code from Department where Dept_Name='" + department + "'");

                    TextBox amount = (TextBox)gridView2.Rows[i].FindControl("txt_amt");
                    amt = Convert.ToString(amount.Text);

                    TextBox qty = (TextBox)gridView2.Rows[i].FindControl("txt_bqty");
                    q = Convert.ToString(qty.Text);

                    DropDownList ddlpur = (DropDownList)gridView2.Rows[i].FindControl("ddl_pur2");
                    if (ddlpur.SelectedItem.Text != "Select")
                    {
                        purpose = Convert.ToInt32(ddlpur.SelectedItem.Value);
                    }
                    else
                    {
                        purpose = 0;
                    }

                    CheckBox chkItemHeader = (CheckBox)gridView2.Rows[i].FindControl("cb_allo");

                    if (chkItemHeader.Checked == true)
                    {
                        allowchk = "1";

                    }
                    else
                    {
                        allowchk = "0";
                    }

                    //string query = "insert into FinBudget(FinBudgetType,FromDate,ToDate)values('" + type + "','" + dt.ToString("MM/dd/yyyy") + "','" + dt1.ToString("MM/dd/yyyy") + "')";
                    //d2.update_method_wo_parameter(query, "Text");

                    //string getcode = d2.GetFunction("select FinBudget from FinBudget order by FinBudget desc");

                    string query1 = "insert into FinBudgetDet(FinBudget,Dept_Code,BudgetQty,BudgetAmt,Purpose,Allow_Aditional)values('" + valcode + "','" + dep + "','" + q + "','" + amt + "','" + purpose + "','" + allowchk + "')";
                    d2.update_method_wo_parameter(query1, "Text");

                }


                imgdiv2.Visible = true;
                lbl_erroralert.Text = "Updated Successfully";
            }
            else if (rb_depitem.Checked == true)
            {
                string department = "";
                string amt = "";
                string q = "";
                string item = "";

                string activerow = "";
                activerow = Fpspread2.ActiveSheetView.ActiveRow.ToString();
                string val = Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                valcode = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);

                if (ViewState["Valcode"] != null)
                {
                    valcode = Convert.ToString(ViewState["Valcode"]);
                }

                string deletequery = "delete FinBudgetDet where FinBudget='" + valcode + "'";
                int up = d2.update_method_wo_parameter(deletequery, "Text");

                for (int i = 0; i < gridView1.Rows.Count; i++)
                {

                    Label dept = (Label)gridView1.Rows[i].FindControl("txt_dept");
                    department = Convert.ToString(dept.Text);

                    string dep = d2.GetFunction("select Dept_Code from Department where Dept_Name='" + department + "'");

                    TextBox amount = (TextBox)gridView1.Rows[i].FindControl("txt_amt");
                    amt = Convert.ToString(amount.Text);

                    TextBox qty = (TextBox)gridView1.Rows[i].FindControl("txt_bqty");
                    q = Convert.ToString(qty.Text);

                    Label code_item = (Label)gridView1.Rows[i].FindControl("txt_icode");
                    item = Convert.ToString(code_item.Text);

                    DropDownList ddlpur = (DropDownList)gridView1.Rows[i].FindControl("ddl_pur1");
                    if (ddlpur.SelectedItem.Text != "Select")
                    {
                        purpose = Convert.ToInt32(ddlpur.SelectedItem.Value);
                    }
                    else
                    {
                        purpose = 0;
                    }


                    CheckBox chkItemHeader = (CheckBox)gridView1.Rows[i].FindControl("cb_allo");

                    if (chkItemHeader.Checked == true)
                    {
                        allowchk = "1";

                    }
                    else
                    {
                        allowchk = "0";
                    }

                    string query1 = "insert into FinBudgetDet(FinBudget,Dept_Code,Item_Code,BudgetQty,BudgetAmt,Purpose,Allow_Aditional)values('" + valcode + "','" + dep + "','" + item + "','" + q + "','" + amt + "','" + purpose + "','" + allowchk + "')";
                    d2.update_method_wo_parameter(query1, "Text");

                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Updated Successfully";
                }
            }


        }
        catch
        {
        }
    }
    public void btn_del_Click(object sender, EventArgs e)
    {
        try
        {
            string department = "";
            string header = "";
            if (rb_dep.Checked == true)
            {
                string activerow = "";
                activerow = Fpspread2.ActiveSheetView.ActiveRow.ToString();
                string val = Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                valcode = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);

                for (int i = 0; i < gridView2.Rows.Count; i++)
                {
                    Label dept = (Label)gridView2.Rows[i].FindControl("lbl_dept");
                    department = Convert.ToString(dept.Text);
                    // string dep = d2.GetFunction("select Dept_Code from Department where Dept_Name='" + department + "'");
                    string del1 = "delete FinBudget where FinBudget='" + valcode + "'";
                    int s1 = d2.update_method_wo_parameter(del1, "Text");
                    string del = "delete FinBudgetDet where FinBudget='" + valcode + "'";
                    int s = d2.update_method_wo_parameter(del, "Text");

                }
                imgdiv2.Visible = true;
                lbl_erroralert.Text = "Deleted Successfully";
            }
            else if (rb_depitem.Checked == true)
            {
                int s = 0;
                string activerow = "";
                activerow = Fpspread2.ActiveSheetView.ActiveRow.ToString();
                string val = Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                valcode = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                for (int i = 0; i < gridView1.Rows.Count; i++)
                {
                    Label dept = (Label)gridView1.Rows[i].FindControl("txt_dept");
                    department = Convert.ToString(dept.Text);
                    // string dep = d2.GetFunction("select Dept_Code from Department where Dept_Name='" + department + "'");
                    string del1 = "delete FinBudget where FinBudget='" + valcode + "'";
                    int s1 = d2.update_method_wo_parameter(del1, "Text");
                    string del = "delete FinBudgetDet where FinBudget='" + valcode + "'";
                    s = d2.update_method_wo_parameter(del, "Text");
                }
                if (s != 0)
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Deleted Successfully";
                }
            }
            else if (rb_hdr.Checked == true)
            {
                int s = 0;
                string activerow = "";
                activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
                string val = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                valcode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);

                for (int i = 0; i < gridView3.Rows.Count; i++)
                {
                    Label head = (Label)gridView3.Rows[i].FindControl("txt_header");
                    header = Convert.ToString(head.Text);
                    string hd = getheadercode(header);
                    string del1 = "delete FinBudget where FinBudget='" + valcode + "'";
                    int s1 = d2.update_method_wo_parameter(del1, "Text");

                    string del = "delete FinBudgetDet where FinBudget='" + valcode + "'";
                    s = d2.update_method_wo_parameter(del, "Text");
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Deleted Successfully";
                }

            }




        }
        catch
        {

        }

    }
    public void btn_exit1_Click(object sender, EventArgs e)
    {
        popup_addnew.Visible = false;
    }
    public void FpSpread2_CellClick(object sender, EventArgs e)
    {
        try
        {
            if (cb_depitem.Checked == true)
            {
                Cellclick1 = true;
                gridView1.Visible = true;
            }
            else if (cb_dept.Checked == true)
            {
                Cellclick2 = true;
                gridView2.Visible = true;
            }
            popup_addnew.Visible = true;
            btn_save.Visible = false;
            btn_exit.Visible = false;

        }
        catch
        {
        }
    }
    public void FpSpread2_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cellclick1 == true)
        {
            try
            {
                gridView2.Visible = false;
                gridView3.Visible = false;
                rb_depitem.Checked = true;
                rb_dep.Checked = false;
                rb_hdr.Checked = false;
                div_depwithitem.Visible = true;
                div_hdrwithledgr.Visible = false;
                div_rdo2.Visible = true;

                gridView1.Visible = true;
                btn_save.Visible = false;
                btn_exit.Visible = false;
                btn_update.Visible = true;
                btn_del.Visible = true;
                btn_exit1.Visible = true;
                txt_popfdate.Text = txt_frmdate.Text;
                txt_poptdate.Text = txt_todate.Text;
                ddlpopcol.Enabled = false;
                txt_popfdate.Enabled = false;
                txt_poptdate.Enabled = false;
                Panel5.Enabled = false;
                Panel3.Enabled = false;
                Panel4.Enabled = false;
                rb_depitem.Enabled = true;
                rb_dep.Enabled = false;
                rb_hdr.Enabled = false;
                btn_pop1go.Enabled = false;

                string activerow = "";
                activerow = Fpspread2.ActiveSheetView.ActiveRow.ToString();
                string val = Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                valcode = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                ViewState["Valcode"] = Convert.ToString(valcode);
                string val1 = Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
                string val1code = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                string colcode = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
                ddlpopcol.SelectedItem.Value = colcode;

                string code_dept = d2.GetFunction("select Dept_Code from Department where Dept_Name='" + val + "' and college_code='" + colcode + "'");
                //string query_select = "select d.FinBudget,BudgetAmt,item_code,BudgetQty,Dept_Code,Purpose,Allow_Aditional from FinBudgetDet d,FinBudget f where f.FinBudget=d.FinBudget and d.FinBudget='" + valcode + "' and Dept_Code='" + code_dept + "'";


                string query_select = "select d.FinBudget,BudgetAmt,item_code,BudgetQty,Dept_Code,Purpose,Allow_Aditional from FinBudgetDet d,FinBudget f where f.FinBudget=d.FinBudget and d.FinBudget='" + valcode + "' and f.CollegeCode='" + colcode + "'";

                ds.Clear();
                ds = d2.select_method_wo_parameter(query_select, "Text");
                ds1.Clear();
                ds1 = d2.select_method_wo_parameter(query_select, "Text");
                ds2.Clear();
                ds2 = d2.select_method_wo_parameter(query_select, "Text");
                ds3.Clear();
                ds3 = d2.select_method_wo_parameter(query_select, "Text");


                DataTable dt = new DataTable();
                DataRow dr;
                ArrayList addnew = new ArrayList();
                ArrayList addnew1 = new ArrayList();
                ArrayList addnew2 = new ArrayList();
                dt.Columns.Add("department");
                dt.Columns.Add("Header");
                dt.Columns.Add("ItemCode");
                dt.Columns.Add("Itemname");
                dt.Columns.Add("BudQty");
                dt.Columns.Add("BudAmt");
                dt.Columns.Add("Purpose");
                dt.Columns.Add("Allow Additional");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string dept = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Code"]);
                        string deptcode = getdepartment(dept);
                        string item = Convert.ToString(ds.Tables[0].Rows[i]["item_code"]);
                        string code = getitemname(item);
                        string header = d2.GetFunction("select distinct ItemHeaderName from IM_ItemMaster where itemcode='" + item + "'");
                        addnew.Add(dept);
                        addnew1.Add(header);
                        addnew2.Add(item);

                        dr = dt.NewRow();

                        dr[0] = Convert.ToString(deptcode);
                        dr[1] = Convert.ToString(header);
                        dr[2] = Convert.ToString(ds.Tables[0].Rows[i]["item_code"]);
                        dr[3] = Convert.ToString(code);
                        dr[4] = Convert.ToString(ds.Tables[0].Rows[i]["BudgetQty"]);
                        dr[5] = Convert.ToString(ds.Tables[0].Rows[i]["BudgetAmt"]);
                        dr[6] = Convert.ToString(ds.Tables[0].Rows[i]["Purpose"]);
                        dr[7] = Convert.ToString(ds.Tables[0].Rows[i]["Allow_Aditional"]);

                        dt.Rows.Add(dr);

                    }

                    if (dt.Rows.Count > 0)
                    {
                        gridView1.DataSource = dt;
                        gridView1.DataBind();
                    }

                }

                if (ds1.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        string dept_name1 = Convert.ToString(ds1.Tables[0].Rows[i]["Dept_Code"]);

                        string dept_name = getdepartment(dept_name1);

                        string item = Convert.ToString(ds1.Tables[0].Rows[i]["item_code"]);
                        string item_name = getitemname(item);
                        string header = d2.GetFunction("select distinct ItemHeaderName from IM_ItemMaster where itemcode='" + item + "'");


                        for (int k = 0; k < cbldepartment.Items.Count; k++)
                        {
                            if (addnew.Contains(cbldepartment.Items[k].Value))
                            {
                                cbldepartment.Items[k].Selected = true;
                            }
                            else
                            {
                                cbldepartment.Items[k].Selected = false;

                            }
                        }
                        for (int k = 0; k < cblheader.Items.Count; k++)
                        {
                            if (cblheader.Items[k].Text != header)
                            {
                                cblheader.Items[k].Selected = false;
                            }
                            else
                            {
                                cblheader.Items[k].Selected = true;

                            }

                        }

                        binditem();

                        for (int k = 0; k < cblitem.Items.Count; k++)
                        {
                            if (addnew2.Contains(cblitem.Items[k].Value))
                            {
                                cblitem.Items[k].Selected = true;
                            }
                            else
                            {
                                cblitem.Items[k].Selected = false;

                            }
                        }
                    }
                }

                if (ds2.Tables[0].Rows.Count > 0)
                {
                    DropDownList ddlpur = new DropDownList();
                    for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                    {
                        for (int j = 0; j < gridView1.Rows.Count; j++)
                        {
                            if (i == j)
                            {
                                string ddlval = Convert.ToString(ds2.Tables[0].Rows[i]["Purpose"]);
                                ddlpur = (DropDownList)gridView1.Rows[j].Cells[9].FindControl("ddl_pur1");
                                if (ddlval.Trim() != "0")
                                {
                                    ddlpur.SelectedValue = ddlval;
                                }
                            }
                        }
                    }
                }

                if (ds3.Tables[0].Rows.Count > 0)
                {
                    CheckBox chkadd = new CheckBox();
                    for (int i = 0; i < ds3.Tables[0].Rows.Count; i++)
                    {
                        for (int j = 0; j < gridView1.Rows.Count; j++)
                        {
                            if (i == j)
                            {
                                string chkval = Convert.ToString(ds3.Tables[0].Rows[i]["Allow_Aditional"]);
                                chkadd = (CheckBox)gridView1.Rows[j].Cells[10].FindControl("cb_allo");
                                if (chkval.Trim() != "False")
                                {
                                    chkadd.Checked = true;
                                }
                            }
                        }
                    }
                }
            }

            catch
            {

            }
        }

        if (Cellclick2 == true)
        {
            try
            {
                rb_dep.Checked = true;
                rb_depitem.Checked = false;
                rb_hdr.Checked = false;
                div_depwithitem.Visible = true;
                div_hdrwithledgr.Visible = false;
                div_rdo2.Visible = false;

                gridView2.Visible = true;
                gridView1.Visible = false;
                gridView3.Visible = false;
                btn_save.Visible = false;
                btn_exit.Visible = false;
                btn_update.Visible = true;
                btn_del.Visible = true;
                btn_exit1.Visible = true;
                txt_popfdate.Text = txt_frmdate.Text;
                txt_poptdate.Text = txt_todate.Text;
                ddlpopcol.Enabled = false;
                txt_popfdate.Enabled = false;
                txt_poptdate.Enabled = false;
                Panel5.Enabled = false;
                rb_dep.Enabled = true;
                rb_depitem.Enabled = false;
                rb_hdr.Enabled = false;
                btn_pop1go.Enabled = false;

                string activerow = "";
                activerow = Fpspread2.ActiveSheetView.ActiveRow.ToString();
                string val = Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                valcode = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                string colcode = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
                ddlpopcol.SelectedItem.Value = colcode;
                ViewState["Valcode"] = Convert.ToString(valcode);
                string code = d2.GetFunction("select Dept_Code from Department where Dept_Name='" + val + "' and college_code='" + colcode + "'");
                string query_select = "select d.FinBudget,BudgetAmt,BudgetQty,Dept_Code,Purpose,Allow_Aditional from FinBudgetDet d, FinBudget f where f.FinBudget=d.FinBudget and d.FinBudget='" + valcode + "' and f.CollegeCode='" + colcode + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(query_select, "Text");
                ds1.Clear();
                ds1 = d2.select_method_wo_parameter(query_select, "Text");
                ds2.Clear();
                ds2 = d2.select_method_wo_parameter(query_select, "Text");
                ds3.Clear();
                ds3 = d2.select_method_wo_parameter(query_select, "Text");

                DataTable dt = new DataTable();
                DataRow dr;
                ArrayList addnew = new ArrayList();
                dt.Columns.Add("department");
                dt.Columns.Add("BudQty");
                dt.Columns.Add("BudAmt");
                dt.Columns.Add("Purpose");
                dt.Columns.Add("Allow Additional");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string dept = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Code"]);
                        string deptcode = getdepartment(dept);
                        addnew.Add(dept);
                        dr = dt.NewRow();
                        dr[0] = Convert.ToString(deptcode);
                        dr[1] = Convert.ToString(ds.Tables[0].Rows[i]["BudgetQty"]);
                        dr[2] = Convert.ToString(ds.Tables[0].Rows[i]["BudgetAmt"]);
                        dr[3] = Convert.ToString(ds.Tables[0].Rows[i]["Purpose"]);
                        dr[4] = Convert.ToString(ds.Tables[0].Rows[i]["Allow_Aditional"]);

                        dt.Rows.Add(dr);
                    }

                    if (dt.Rows.Count > 0)
                    {
                        gridView2.DataSource = dt;
                        gridView2.DataBind();
                    }
                }

                if (ds1.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        string dept_name1 = Convert.ToString(ds1.Tables[0].Rows[i]["Dept_Code"]);

                        string dept_name = getdepartment(dept_name1);
                        for (int k = 0; k < cbldepartment.Items.Count; k++)
                        {
                            if (addnew.Contains(cbldepartment.Items[k].Value))
                            {
                                cbldepartment.Items[k].Selected = true;
                            }
                            else
                            {
                                cbldepartment.Items[k].Selected = false;
                            }
                        }
                    }
                }

                if (ds2.Tables[0].Rows.Count > 0)
                {
                    DropDownList ddlpur = new DropDownList();
                    for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                    {
                        for (int j = 0; j < gridView2.Rows.Count; j++)
                        {
                            if (i == j)
                            {
                                string ddlval = Convert.ToString(ds2.Tables[0].Rows[i]["Purpose"]);
                                ddlpur = (DropDownList)gridView2.Rows[j].Cells[4].FindControl("ddl_pur2");
                                if (ddlval.Trim() != "0")
                                {
                                    ddlpur.SelectedValue = ddlval;
                                }
                            }
                        }
                    }
                }

                if (ds3.Tables[0].Rows.Count > 0)
                {
                    CheckBox chkadd = new CheckBox();
                    for (int i = 0; i < ds3.Tables[0].Rows.Count; i++)
                    {
                        for (int j = 0; j < gridView2.Rows.Count; j++)
                        {
                            if (i == j)
                            {
                                string chkval = Convert.ToString(ds3.Tables[0].Rows[i]["Allow_Aditional"]);
                                chkadd = (CheckBox)gridView2.Rows[j].Cells[5].FindControl("cb_allo");
                                if (chkval.Trim() != "False")
                                {
                                    chkadd.Checked = true;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {

            }
        }
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
        lbl.Add(lbl_deprt);
        fields.Add(0);
        fields.Add(3);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }

    private void setLabelTextlookup()
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

        lbl.Add(lblpopcol);
        lbl.Add(Label7);
        fields.Add(0);
        fields.Add(3);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }

    // last modified 04-10-2016 sudhagar

}