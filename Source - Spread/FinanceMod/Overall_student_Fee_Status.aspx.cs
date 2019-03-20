using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using FarPoint.Web.Spread;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

public partial class Overall_student_Fee_Status : System.Web.UI.Page
{
    DAccess2 da = new DAccess2();
    DataSet ds1 = new DataSet();
    DataSet ds = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds2 = new DataSet();
    Boolean finalflag = false;
    Boolean deptflag = false;
    static byte roll = 0;
    string usercode = "", singleuser = "", group_user = "";
    string course_id = string.Empty;
    static string collegecode = "";
    int i = 0;
    string headid = "";
    static TreeNode node;
    TreeNode subchildnode;
    static int rightscode = 0;
    bool usBasedRights = false;
    static bool sclflag = false;
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
            // collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            errmsg.Visible = false;
            if (!IsPostBack)
            {

                //chklstpaymode.Items.Add(new ListItem("Cash","1"));
                //chklstpaymode.Items.Add(new ListItem("Cheque","2"));
                // chklstpaymode.Items.Add(new ListItem("dd","3"));
                //chklstpaymode.Items.Add(new ListItem("Challan","4"));
                //chklstpaymode.Items.Add(new ListItem("Online Pay","5"));
                setLabelText();
                UserbasedRights();
                lblc1.Visible = false;
                lblc2.Visible = false;
                lblc3.Visible = false;
                lblc4.Visible = false;
                lblc5.Visible = false;
                lblcard.Visible = false;
                txtdate.Enabled = false;
                txtto.Enabled = false;
                txtpaymode.Enabled = false;
                txtdate.Attributes.Add("readonly", "readonly");
                txtto.Attributes.Add("readonly", "readonly");

                DataSet dsfindate = da.select_method_wo_parameter("select distinct convert(nvarchar(15),FinYearStart,103) sdate,convert(nvarchar(15),FinYearEnd,103) edate,FinYearPK from FM_FinYearMaster where collegecode='" + collegecode + "' order by sdate desc", "Text");
                if (dsfindate.Tables[0].Rows.Count > 0)
                {
                    txtdate.Text = dsfindate.Tables[0].Rows[0]["sdate"].ToString();
                    txtto.Text = dsfindate.Tables[0].Rows[0]["edate"].ToString();
                }
                else
                {
                    txtdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtto.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                treeview_spreadfields.Attributes.Add("onclick", "OnCheckBoxCheckChanged(event)");
                FpSpread1.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                loadcollege();
                collegecode = ddl_collegename.SelectedItem.Value.ToString();
                loadfinanceyear();
                loadtype();
                BindBatch();
                BindDegree();
                loadpaymode();
                BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);

                for (int chk = 0; chk < chklstpaymode.Items.Count; chk++)
                {
                    chklstpaymode.Items[chk].Selected = true;
                }
                chkpaymode.Checked = true;
                txtpaymode.Text = "Mode (" + chklstpaymode.Items.Count + ")";
                bindsem();
                challanAndReceiptNoRights();
                RollAndRegSettings();
                chkdate_CheckedChanged(sender, e);
            }
        }
        catch (Exception ex) { }
    }

    public void loadpaymode()
    {
        try
        {
            chklstpaymode.Items.Clear();
            //cbltypedep.Items.Add(new ListItem("Cash", "1"));
            //cbltypedep.Items.Add(new ListItem("Cheque", "2"));
            //cbltypedep.Items.Add(new ListItem("DD", "3"));
            //cbltypedep.Items.Add(new ListItem("Challan", "4"));
            //cbltypedep.Items.Add(new ListItem("Online", "5"));
            da.BindPaymodeToCheckboxList(chklstpaymode, usercode, Convert.ToString(ddl_collegename.SelectedItem.Value));
            if (chklstpaymode.Items.Count > 0)
            {
                for (int i = 0; i < chklstpaymode.Items.Count; i++)
                {
                    chklstpaymode.Items[i].Selected = true;
                }
                txtpaymode.Text = "Paid(" + chklstpaymode.Items.Count + ")";
                chkpaymode.Checked = true;
            }
        }
        catch
        {

        }
    }
    public void challanAndReceiptNoRights()
    {
        try
        {
            string query = "";
            string Master1 = "";
            string stud = "";
            string values = "";
            string sms = "";
            string sms1 = "";
            string sms2 = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {

                string group = Session["group_code"].ToString();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    Master1 = group_semi[0].ToString();
                }
                query = "select * from Master_Settings where settings ='Reciept/Challan No Rights' and group_code ='" + Master1 + "'";
            }
            else
            {
                Master1 = Session["usercode"].ToString();
                query = "select * from Master_Settings where settings ='Reciept/Challan No Rights' and usercode ='" + Master1 + "'";
            }
            ds = da.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = Convert.ToString(ds.Tables[0].Rows[i]["value"]);
                    if (val == "1")
                    {
                        rightscode = 1;
                    }
                    else if (val == "2")
                    {
                        rightscode = 2;
                    }
                    else if (val == "3")
                    {
                        rightscode = 3;
                    }
                    else
                    {
                        rightscode = 0;
                    }
                }
            }
        }
        catch
        { }
    }

    #region college Name

    public void loadcollege()
    {
        try
        {
            ds.Clear();
            ddl_collegename.Items.Clear();
            string Query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = da.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_collegename.DataSource = ds;
                ddl_collegename.DataTextField = "collname";
                ddl_collegename.DataValueField = "college_code";
                ddl_collegename.DataBind();
            }
        }
        catch
        { }
    }
    protected void ddl_collegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            collegecode = ddl_collegename.SelectedItem.Value.ToString();
            loadtype();
            BindBatch();
            BindDegree();
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            bindsem();
            loadfinanceyear();
            loadheader();
            bindsem();
        }
        catch
        {
        }
    }

    #endregion

    public void loadheader()
    {
        try
        {
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            treeview_spreadfields.Nodes.Clear();
            txtaccheader.Text = "---Select---";
            chkaccheader.Checked = false;
            chklstaccheader.Items.Clear();
            string type = "";
            if (ddltype.Enabled == true)
            {
                if (ddltype.Items.Count > 0)
                {
                    if (ddltype.SelectedItem.ToString() != "Both" && ddltype.SelectedItem.ToString() != "")
                    {
                        type = "and c.Stream='" + ddltype.SelectedItem.ToString() + "'";
                    }
                }
            }
            string strheadid = "";
            for (int t = 0; t < chklsfyear.Items.Count; t++)
            {
                if (chklsfyear.Items[t].Selected == true)
                {
                    if (strheadid == "")
                    {
                        strheadid = "'" + chklsfyear.Items[t].Value.ToString() + "'";
                    }
                    else
                    {
                        strheadid = strheadid + ",'" + chklsfyear.Items[t].Value.ToString() + "'";
                    }
                }
            }
            if (strheadid.Trim() != "")
            {
                strheadid = " and a.acct_id in (" + strheadid + ")";
            }
            //if (strheadid.Trim() != "") hided by sridharan 25.01.2016
            //{
            ds.Reset();
            ds.Dispose();
            string hedgId = "";
            // string straccheadquery = "select distinct a.header_id,a.header_name from chlheadersettings c,Acctheader a where c.Header_ID=a.header_id and a.header_name not in ('arrear') " + type + " "+strheadid+"";hided by sridharan 25.01.2016
            string straccheadquery = "SELECT HeaderPK as header_id,HeaderName as header_name FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = '" + Session["usercode"].ToString() + "' AND H.CollegeCode = '" + collegecode + "' ";
            ds = da.select_method_wo_parameter(straccheadquery, "Text");
            if (ddlacctype.SelectedItem.Text == "Ledger")
            {
                txtaccheader.Text = " Header (" + ds.Tables[0].Rows.Count + ")";
                chkaccheader.Checked = true;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        node = new TreeNode(ds.Tables[0].Rows[i]["header_name"].ToString(), ds.Tables[0].Rows[i]["header_id"].ToString());
                        //string strled = "select fee_type,fee_code from fee_info where fee_type not in ('Cash','Income & Expenditure','Misc') and fee_type not in (select bankname from bank_master1) and header_id = (" + ds.Tables[0].Rows[i]["header_id"].ToString() + ") order by fee_code"; //hided by sridharan 25.01.2016
                        string strled = "SELECT LedgerPK as fee_code,LedgerName as fee_type FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode   AND P. UserCode = '" + Session["usercode"].ToString() + "' AND L.CollegeCode = '" + collegecode + "'  and L.HeaderFK in('" + ds.Tables[0].Rows[i]["header_id"].ToString() + "')and LedgerMode='0'  order by isnull(l.priority,1000), l.ledgerName asc ";
                        ds1 = da.select_method_wo_parameter(strled, "Text");
                        for (int ledge = 0; ledge < ds1.Tables[0].Rows.Count; ledge++)
                        {
                            subchildnode = new TreeNode(ds1.Tables[0].Rows[ledge]["fee_type"].ToString(), ds1.Tables[0].Rows[ledge]["fee_code"].ToString());
                            subchildnode.ShowCheckBox = true;
                            node.ChildNodes.Add(subchildnode);

                        }
                        node.ShowCheckBox = true;
                        treeview_spreadfields.Nodes.Add(node);
                        if (hedgId == "")
                            hedgId = Convert.ToString(ds.Tables[0].Rows[i]["header_id"]);
                        else
                            hedgId = hedgId + "','" + Convert.ToString(ds.Tables[0].Rows[i]["header_id"]);
                    }
                    if (chkaccheader.Checked == true)
                    {

                        for (int remv = 0; remv < treeview_spreadfields.Nodes.Count; remv++)
                        {
                            treeview_spreadfields.Nodes[remv].Checked = true;
                            txtaccheader.Text = "Header(" + (treeview_spreadfields.Nodes.Count) + ")";
                            if (treeview_spreadfields.Nodes[remv].ChildNodes.Count > 0)
                            {
                                for (int child = 0; child < treeview_spreadfields.Nodes[remv].ChildNodes.Count; child++)
                                {
                                    treeview_spreadfields.Nodes[remv].ChildNodes[child].Checked = true;
                                }
                            }
                        }
                    }
                    ViewState["hedgId"] = hedgId;
                }
            }
            else
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chklstaccheader.DataSource = ds;
                    chklstaccheader.DataTextField = "header_name";
                    chklstaccheader.DataValueField = "header_id";
                    chklstaccheader.DataBind();

                    for (int i = 0; i < chklstaccheader.Items.Count; i++)
                    {
                        chklstaccheader.Items[i].Selected = true;
                        if (headid == "")
                        {
                            headid = chklstaccheader.Items[i].Value;
                        }
                        else
                        {
                            headid = headid + "," + chklstaccheader.Items[i].Value;
                        }
                    }
                    txtaccheader.Text = " Header (" + chklstaccheader.Items.Count + ")";
                    chkaccheader.Checked = true;
                }
            }
            //}

        }
        catch
        {

        }
    }
    public void loadfinanceyear()
    {
        try
        {
            string getfinanceyear = "select distinct convert(nvarchar(15),FinYearStart,103) sdate,convert(nvarchar(15),FinYearEnd,103) edate,FinYearPK from FM_FinYearMaster where CollegeCode='" + collegecode + "' order by FinYearPK desc";
            // string getfinanceyear = d2.getCurrentFinanceYear(usercode,Session["collegecode"].ToString() );  
            ds.Dispose();
            ds.Reset();
            chkfyear.Checked = false;
            chklsfyear.Items.Clear();
            ds = da.select_method_wo_parameter(getfinanceyear, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string fdatye = ds.Tables[0].Rows[i]["sdate"].ToString() + '-' + ds.Tables[0].Rows[i]["edate"].ToString();
                    string actid = ds.Tables[0].Rows[i]["FinYearPK"].ToString();
                    chklsfyear.Items.Insert(0, new System.Web.UI.WebControls.ListItem(fdatye, actid));
                }

                for (int i = 0; i < chklsfyear.Items.Count; i++)
                {
                    chklsfyear.Items[i].Selected = true;
                }
                txtfyear.Text = "Finance Year (" + chklsfyear.Items.Count + ")";
                chkfyear.Checked = true;
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void chkfyear_changed(object sender, EventArgs e)
    {
        try
        {

            if (chkfyear.Checked == true)
            {
                for (int i = 0; i < chklsfyear.Items.Count; i++)
                {
                    chklsfyear.Items[i].Selected = true;
                }
                txtfyear.Text = "Finance Year (" + (chklsfyear.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklsfyear.Items.Count; i++)
                {
                    chklsfyear.Items[i].Selected = false;
                }
                txtfyear.Text = "--Select--";
            }
            // loadheader();
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void chklsfyear_selected(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            chkfyear.Checked = false;
            txtfyear.Text = "--Select--";
            for (int i = 0; i < chklsfyear.Items.Count; i++)
            {
                if (chklsfyear.Items[i].Selected == true)
                {
                    count++;
                }
            }
            if (count > 0)
            {
                txtfyear.Text = "Finance Year (" + count + ")";
                if (count == chklsfyear.Items.Count)
                {
                    chkfyear.Checked = true;
                }
            }
            //loadheader();
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        lbldegree.Visible = true;
        FpSpread1.Visible = false;
        btnprintmaster.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnxl.Visible = false;
        errmsg.Visible = false;
    }

    public void fill_Studheader()
    {
        try
        {
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;

            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].DefaultStyle.Font.Bold = true;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].SheetCorner.RowCount = 1;


            FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
            style.Font.Size = 10;
            style.Font.Bold = true;
            FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
            FpSpread1.Sheets[0].AllowTableCorner = true;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            string strbatch = "", strcourse = "", strbranch = "", finalstr = "";

            if ((strcourse.ToString() != "") && (strbranch.ToString() != ""))
            {
                finalstr = strbatch + "-" + strcourse + "[" + strbranch + "]";
            }
            if (strbranch.ToString() == "")
            {
                finalstr = strbatch + "-" + strcourse;
            }
        }
        catch
        {
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            try
            {
                UserbasedRights();
                RollAndRegSettings();
                challanAndReceiptNoRights();
                string feecategorycolumngroup = "";
                txtexcelname.Text = "";
                FpSpread1.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                lblc1.Visible = false;
                lblc2.Visible = false;
                lblc3.Visible = false;
                lblc4.Visible = false;
                lblc5.Visible = false;
                lblcard.Visible = false;
                DateTime dat = DateTime.ParseExact(txtdate.Text.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime dat1 = DateTime.ParseExact(txtto.Text.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                if (chkdate.Checked == true)
                {
                    if (dat > dat1)
                    {
                        errmsg.Visible = true;
                        errmsg.Text = "To Date Must Be Greater Than or Equal To From Date";
                        return;
                    }
                }
                if (ddl_collegename.Items.Count > 0)
                {
                    collegecode = Convert.ToString(ddl_collegename.SelectedItem.Value);
                }
                string batchquery = "";
                string degreequery = "";
                string setbatchanddegree = "";
                string getdegreedetails = "";
                string delflg = "";
                string demquery = "";
                string batch_all = string.Empty;
                string course_all = string.Empty;
                string header_all = string.Empty;
                string category = string.Empty;
                string feecode = "";
                string tot_category = string.Empty;
                string strorder = "";
                string paidquery = "";
                string str = "";
                string degquery = "";
                string modestatus = "";
                int sno = 0;
                string courseid = "";
                string batcyear = "";
                string year = "";
                string deg_acr = string.Empty;
                string deg_dept = string.Empty;
                int degreerow = 0;
                string stefeesem = "";
                string feesemquery = "";
                string feecategorycolumn = " ";
                string strfeelist = "";
                string strfeeval = "";
                string approlladmit = "";
                string head = "";
                string starttear = dat.ToString("yyyy");
                string endyear = dat1.ToString("yyyy");
                // string strstufillerfee = " (select f.Roll_Admit from fee_allot f,fee_info fi where fi.fee_code=f.fee_code and f.header_id=fi.header_id  and total>0 ";
                string strstufillerfee = " (select f.app_no from ft_feeallot f,fm_ledgermaster fi where fi.ledgerpk=f.ledgerfk and  f.headerfk=fi.headerfk  and totalamount>0 order by isnull(fi.priority,1000), fi.ledgerName asc";
                string strstufillerfeeapplyn = " (select f.app_formno from fee_allot f,fee_info fi where fi.fee_code=f.fee_code and f.header_id=fi.header_id and total>0 ";
                string regrolladmit = " and r.app_no in ";
                string y1 = dat.ToString("yyyy-MM-dd");
                string y2 = dat1.ToString("yyyy-MM-dd");
                string getfeeval = "";
                Boolean degflag = false;
                Boolean feecateflag = false;

                FarPoint.Web.Spread.TextCellType txtcel = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtchal = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtreg = new FarPoint.Web.Spread.TextCellType();


                if (ddlacctype.Text == "---Select---")
                {
                    finalflag = true;
                    errmsg.Visible = true;
                    errmsg.Text = "Please Select Account Type";
                    FpSpread1.Visible = false;
                    btnprintmaster.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnxl.Visible = false;
                    return;
                }
                else if (Convert.ToDouble(starttear) > Convert.ToDouble(endyear))
                {
                    finalflag = true;
                    errmsg.Visible = true;
                    errmsg.Text = "End Year must be greater Than The Start Year ! ";
                    FpSpread1.Visible = false;
                    btnprintmaster.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnxl.Visible = false;
                }
                else
                {
                    if (chkdate.Checked == true && ddlfeetype.SelectedItem.Text != "Both" && chkyettobepaid.Checked == false && ddlacctype.SelectedItem.Text != "Group Header")
                    {
                        if (ddlfeetype.SelectedItem.Text == "Paid" && ddlacctype.SelectedItem.Text != "Group Header")
                        {
                            loadpaid();
                        }
                        else if (ddlfeetype.SelectedItem.Text == "Yet To Be Paid")
                        {
                            // loadyettobepaid();
                        }
                    }
                    else
                    {
                        if (ddlfeetype.Text == "---Select---")
                        {
                            errmsg.Visible = true;
                            errmsg.Text = "Please Select Fee Type";
                            FpSpread1.Visible = false;
                            btnprintmaster.Visible = false;
                            lblrptname.Visible = false;
                            txtexcelname.Visible = false;
                            btnxl.Visible = false;
                            return;
                        }
                        fill_Studheader();

                        for (int b = 0; b < chklstbatch.Items.Count; b++)
                        {
                            if (chklstbatch.Items[b].Selected == true)
                            {
                                if (batch_all == "")
                                {
                                    batch_all = chklstbatch.Items[b].Value.ToString();
                                }
                                else
                                {

                                    batch_all = batch_all + "," + chklstbatch.Items[b].Value.ToString();
                                }
                            }
                        }
                        if (batch_all.Trim() != "")
                        {
                            batchquery = " and r.Batch_year in(" + batch_all + ")";
                            setbatchanddegree = " Batch_year in(" + batch_all + ")";
                        }

                        for (int c = 0; c < chklstbranch.Items.Count; c++)
                        {
                            if (chklstbranch.Items[c].Selected == true)
                            {
                                if (course_all == "")
                                {
                                    course_all = "'" + chklstbranch.Items[c].Value.ToString() + "'";
                                }
                                else
                                {
                                    course_all = course_all + ",'" + chklstbranch.Items[c].Value.ToString() + "'";
                                }
                            }
                        }

                        if (course_all.Trim() != "")
                        {
                            degreequery = " and r.degree_code in(" + course_all + ")";
                            if (setbatchanddegree.Trim() != "")
                            {
                                setbatchanddegree = setbatchanddegree + " and degree_code in(" + course_all + ")";
                            }
                            else
                            {
                                setbatchanddegree = " and degree_code in(" + course_all + ")";
                            }
                        }

                        for (int a = 0; a < chklstaccheader.Items.Count; a++)
                        {
                            if (chklstaccheader.Items[a].Selected == true)
                            {
                                if (header_all == "")
                                {
                                    header_all = chklstaccheader.Items[a].Value.ToString();
                                }
                                else
                                {
                                    header_all = header_all + "','" + chklstaccheader.Items[a].Value.ToString();
                                }
                            }
                        }

                        for (int remv = 0; remv < treeview_spreadfields.Nodes.Count; remv++)
                        {
                            if (header_all == "")
                            {
                                header_all = treeview_spreadfields.Nodes[remv].Value.ToString();
                            }
                            else
                            {
                                header_all = header_all + "','" + treeview_spreadfields.Nodes[remv].Value.ToString();
                            }
                            if (treeview_spreadfields.Nodes[remv].ChildNodes.Count > 0)
                            {
                                for (int child = 0; child < treeview_spreadfields.Nodes[remv].ChildNodes.Count; child++)
                                {
                                    if (category == "")
                                    {
                                        category = treeview_spreadfields.Nodes[remv].ChildNodes[child].Value.ToString();
                                    }
                                    else
                                    {
                                        category = category + "," + treeview_spreadfields.Nodes[remv].ChildNodes[child].Value.ToString();
                                    }
                                }
                            }
                        }
                        ArrayList arrayfeecate = new ArrayList();
                        bool semflag = false;
                        for (int fe = 0; fe < chklsfeesem.Items.Count; fe++)
                        {
                            if (chklsfeesem.Items[fe].Selected == true)
                            {
                                arrayfeecate.Add(chklsfeesem.Items[fe].Text.ToString() + '^' + chklsfeesem.Items[fe].Value.ToString());
                                semflag = true;
                                feecateflag = true;
                                if (stefeesem == "")
                                {
                                    stefeesem = "'" + chklsfeesem.Items[fe].Value.ToString() + "'";
                                    strfeelist = chklsfeesem.Items[fe].Value.ToString();
                                    strfeeval = chklsfeesem.Items[fe].Text.ToString();
                                }
                                else
                                {
                                    stefeesem = stefeesem + ",'" + chklsfeesem.Items[fe].Value.ToString() + "'";
                                    strfeelist = strfeelist + "/" + chklsfeesem.Items[fe].Value.ToString();
                                    strfeeval = strfeeval + "/" + chklsfeesem.Items[fe].Text.ToString();
                                }
                            }
                        }
                        if (stefeesem.Trim() != "" && stefeesem != null)
                        {
                            strstufillerfee = strstufillerfee + " and f.feecategory in(" + stefeesem + ")";
                            strstufillerfeeapplyn = strstufillerfeeapplyn + " and f.feecategory in(" + stefeesem + ")";
                            feesemquery = " and feecategory in(" + stefeesem + ")";
                            feecategorycolumn = ",feecategory";
                            feecategorycolumngroup = feecategorycolumn;
                        }
                        else
                        {
                            errmsg.Visible = true;
                            errmsg.Text = "Please Select Any One Semester";
                            return;
                        }

                        if (checkdicon.Checked == true)
                        {
                            delflg = "";
                        }
                        else
                        {
                            delflg = "and r.delflag=0";
                        }

                        strorder = " order by r.Batch_Year,r.degree_code,r.Current_Semester,r.Stud_Name";
                        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.BackColor = Color.FromArgb(214, 235, 255);
                        // settingValueRollAndReg();
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 3;
                        FpSpread1.Sheets[0].ColumnHeader.Columns.Count = 7;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Student Name";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
                        if (checkSchoolSetting() == 0)
                        {
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Admission No";
                            sclflag = true;
                        }
                        else
                        {
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg No";
                            sclflag = false;
                        }

                        if (roll == 0)
                        {
                            FpSpread1.Sheets[0].Columns[2].Visible = true;
                            FpSpread1.Sheets[0].Columns[3].Visible = true;
                        }
                        else if (roll == 1)
                        {
                            FpSpread1.Sheets[0].Columns[2].Visible = true;
                            FpSpread1.Sheets[0].Columns[3].Visible = true;
                        }
                        else if (roll == 2)
                        {
                            FpSpread1.Sheets[0].Columns[2].Visible = true;
                            FpSpread1.Sheets[0].Columns[3].Visible = false;
                        }
                        else if (roll == 3)
                        {
                            FpSpread1.Sheets[0].Columns[2].Visible = false;
                            FpSpread1.Sheets[0].Columns[3].Visible = true;
                        }
                        else if (roll == 4)
                        {
                            FpSpread1.Sheets[0].Columns[2].Visible = false;
                            FpSpread1.Sheets[0].Columns[3].Visible = true;
                        }
                        else if (roll == 5)
                        {
                            FpSpread1.Sheets[0].Columns[2].Visible = true;
                            FpSpread1.Sheets[0].Columns[3].Visible = true;
                        }
                        else if (roll == 6)
                        {
                            FpSpread1.Sheets[0].Columns[2].Visible = false;
                            FpSpread1.Sheets[0].Columns[3].Visible = true;
                        }
                        else if (roll == 7)
                        {
                            FpSpread1.Sheets[0].Columns[2].Visible = true;
                            FpSpread1.Sheets[0].Columns[3].Visible = true;
                        }
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = lbldegree.Text;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = lblfeesem.Text;
                        if (feecateflag == true)
                        {
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = lblfeesem.Text;
                            FpSpread1.Sheets[0].Columns[5].Width = 100;
                            FpSpread1.Sheets[0].Columns[5].Visible = true;
                        }
                        else
                        {
                            FpSpread1.Sheets[0].Columns[5].Visible = false;
                        }
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Receipt No/Challan No";
                        if (ddlfeetype.Text == "Yet To Be Paid" || ddlfeetype.Text == "Both")
                        {
                            FpSpread1.Sheets[0].Columns[6].Visible = false;
                            FpSpread1.Sheets[0].Columns[6].CellType = txtchal;
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 3, 1);
                        }
                        else
                        {
                            FpSpread1.Sheets[0].Columns[6].Visible = true;
                            FpSpread1.Sheets[0].Columns[6].CellType = txtchal;
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 3, 1);
                        }
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 3, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 3, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 3, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 3, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 3, 1);


                        FpSpread1.Sheets[0].Columns[2].CellType = txtcel;
                        FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;


                        FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;

                        DataView dvhead = new DataView();
                        DataView dv_get_course = new DataView();
                        DataView dv_paid_data = new DataView();
                        DataView dv_demand_amount = new DataView();

                        Dictionary<int, Double> dictotal = new Dictionary<int, double>();
                        Dictionary<int, Double> dicgrandtotal = new Dictionary<int, double>();
                        Dictionary<string, string> dicstu = new Dictionary<string, string>();
                        Dictionary<string, Double> dicyetpaid = new Dictionary<string, double>();
                        Dictionary<string, int> groupheadg = new Dictionary<string, int>();
                        string finyearval = "";
                        string Newfinayear = "";
                        for (int i = 0; i < chklsfyear.Items.Count; i++)
                        {
                            if (chklsfyear.Items[i].Selected == true)
                            {
                                if (finyearval == "")
                                {
                                    finyearval = "'" + chklsfyear.Items[i].Value.ToString() + "'";
                                }
                                else
                                {
                                    finyearval = finyearval + ",'" + chklsfyear.Items[i].Value.ToString() + "'";
                                }
                            }
                        }
                        if (finyearval.Trim() != "")
                        {
                            Newfinayear = finyearval;
                            finyearval = " and a.FinYearFK in(" + finyearval + ")";
                        }

                        string bankcode = "";
                        for (int i = 0; i < chklstpaymode.Items.Count; i++)
                        {
                            if (chklstpaymode.Items[i].Selected == true)
                            {
                                if (chklstpaymode.Items[i].Text == "Online Pay")
                                {
                                    for (int bk = 0; bk < cblbank.Items.Count; bk++)
                                    {
                                        if (cblbank.Items[bk].Selected == true)
                                        {
                                            if (bankcode == "")
                                                bankcode = "" + cblbank.Items[bk].Value.ToString() + "";
                                            else
                                                bankcode = bankcode + "','" + cblbank.Items[bk].Value.ToString() + "";
                                        }
                                    }
                                }
                                else
                                    bankcode = "";
                            }
                            else
                                bankcode = "";
                        }


                        //paymode
                        for (int mode = 0; mode < chklstpaymode.Items.Count; mode++)
                        {
                            if (chklstpaymode.Items[mode].Selected == true)
                            {
                                if (modestatus == "")
                                {
                                    modestatus = Convert.ToString(chklstpaymode.Items[mode].Value);
                                }
                                else
                                {
                                    modestatus = modestatus + "','" + Convert.ToString(chklstpaymode.Items[mode].Value);
                                }
                            }
                        }


                        string demandquery = "";
                        string headervalue = "";
                        string oldjedvalue = "";
                        if (ddlacctype.SelectedItem.Text == "Ledger")//Ledger
                        {
                            DataSet lgacr = new DataSet();
                            #region Without date Ledger
                            //ViewState["hedgId"] = hedgId;
                            if (cbledgacr.Checked == true)
                            {
                                if (ViewState["hedgId"] != null)
                                {
                                    string hedgID = Convert.ToString(ViewState["hedgId"]);
                                    string strled = "SELECT LedgerPK ,LedgerAcr FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode   AND P. UserCode = '" + Session["usercode"].ToString() + "' AND L.CollegeCode = '" + collegecode + "'  and L.HeaderFK in('" + hedgID + "')and LedgerMode='0'  order by isnull(l.priority,1000), l.ledgerName asc ";
                                    lgacr.Clear();
                                    lgacr = da.select_method_wo_parameter(strled, "Text");
                                }
                            }


                            for (int remv = 0; remv < treeview_spreadfields.Nodes.Count; remv++)
                            {
                                Boolean headflag = false;
                                int ccount = 0;
                                int hstartc = 0;
                                int ledcount = 0;
                                Boolean headervieflag = false;
                                if (treeview_spreadfields.Nodes[remv].ChildNodes.Count > 0)
                                {
                                    for (int child = 0; child < treeview_spreadfields.Nodes[remv].ChildNodes.Count; child++)
                                    {
                                        if (treeview_spreadfields.Nodes[remv].ChildNodes[child].Checked == true)
                                        {
                                            headervieflag = true;
                                            //   headervalue = Convert.ToString(treeview_spreadfields.Nodes[remv].Value);
                                            if (headervalue == "")
                                            {
                                                headervalue = Convert.ToString(treeview_spreadfields.Nodes[remv].Value);
                                                oldjedvalue = Convert.ToString(treeview_spreadfields.Nodes[remv].Value);
                                            }
                                            else
                                            {
                                                if (oldjedvalue != Convert.ToString(treeview_spreadfields.Nodes[remv].Value))
                                                {
                                                    oldjedvalue = Convert.ToString(treeview_spreadfields.Nodes[remv].Value);
                                                    headervalue = headervalue + "','" + Convert.ToString(treeview_spreadfields.Nodes[remv].Value);
                                                }
                                            }



                                            if (getfeeval == "")
                                            {
                                                getfeeval = treeview_spreadfields.Nodes[remv].ChildNodes[child].Value;
                                            }
                                            else
                                            {
                                                getfeeval = getfeeval + "','" + treeview_spreadfields.Nodes[remv].ChildNodes[child].Value;
                                            }
                                            if (ddlfeetype.SelectedItem.Text == "Paid")
                                            {
                                                FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 1;
                                                if (headflag == false)
                                                {
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = treeview_spreadfields.Nodes[remv].Text.ToString();
                                                    headflag = true;
                                                    hstartc = FpSpread1.Sheets[0].ColumnCount - 1;

                                                }
                                                ledcount++;
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Note = oldjedvalue;
                                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1, 1, 1);
                                                feecode = treeview_spreadfields.Nodes[remv].ChildNodes[child].Value.ToString();
                                                if (cbledgacr.Checked == true)
                                                {
                                                    DataView dv = new DataView();
                                                    if (lgacr.Tables.Count > 0)
                                                    {
                                                        lgacr.Tables[0].DefaultView.RowFilter = "ledgerPk='" + feecode + "'";
                                                        dv = lgacr.Tables[0].DefaultView;
                                                        if (dv.Count > 0)
                                                        {
                                                            string name = Convert.ToString(dv[0]["LedgerAcr"]);
                                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = name;
                                                        }
                                                    }
                                                }
                                                else
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = treeview_spreadfields.Nodes[remv].ChildNodes[child].Text.ToString();

                                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Paid";
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Note = feecode;
                                                ccount += 1;
                                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - ccount, 1, ccount);
                                            }

                                            else if (ddlfeetype.SelectedItem.Text == "Yet To Be Paid")
                                            {
                                                FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 1;
                                                if (headflag == false)
                                                {
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = treeview_spreadfields.Nodes[remv].Text.ToString();
                                                    headflag = true;
                                                    hstartc = FpSpread1.Sheets[0].ColumnCount - 1;
                                                }
                                                ledcount++;
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Note = headervalue;
                                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1, 1, 1);
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = treeview_spreadfields.Nodes[remv].ChildNodes[child].Text.ToString();
                                                feecode = treeview_spreadfields.Nodes[remv].ChildNodes[child].Value.ToString();
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Balance";
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Note = feecode;
                                                ccount += 1;
                                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - ccount, 1, ccount);
                                            }
                                            else
                                            {

                                                FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 3;
                                                if (headflag == false)
                                                {
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 3].Text = treeview_spreadfields.Nodes[remv].Text.ToString();
                                                    headflag = true;
                                                    hstartc = FpSpread1.Sheets[0].ColumnCount - 3;
                                                }
                                                ledcount++;
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 3].Note = headervalue;
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 2].Note = headervalue;
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Note = headervalue;
                                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3, 1, 3);
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 3].Text = treeview_spreadfields.Nodes[remv].ChildNodes[child].Text.ToString();
                                                feecode = treeview_spreadfields.Nodes[remv].ChildNodes[child].Value.ToString();
                                                //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3].Text = "Receivable";
                                                //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3].Note = feecode;
                                                //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Text = "Received";
                                                //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Note = feecode;
                                                //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Dues";
                                                //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Note = feecode;

                                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3, 1, 3);
                                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3, 1, 3);

                                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3].Text = "Alloted";
                                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3, 3, 1);
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Text = "Paid";
                                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2, 3, 1);
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Balance";
                                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1, 3, 1);
                                                ccount += 3;
                                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - ccount, 1, ccount);
                                            }

                                        }
                                    }
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 1;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = treeview_spreadfields.Nodes[remv].Text.ToString();
                                }
                                if (headervieflag == true)
                                {
                                    if (ddlfeetype.Text == "Paid")//ledgerwise total header binding.......................
                                    {
                                        FpSpread1.Sheets[0].ColumnCount++;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = treeview_spreadfields.Nodes[remv].Text.ToString() + " Total Paid";
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].BackColor = Color.MistyRose;
                                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1, 3, 1);
                                    }
                                    else if (ddlfeetype.Text == "Yet To Be Paid")
                                    {
                                        FpSpread1.Sheets[0].ColumnCount++;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = treeview_spreadfields.Nodes[remv].Text.ToString() + " Total Balance";
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].BackColor = Color.MistyRose;
                                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1, 3, 1);
                                    }
                                    //else
                                    //{
                                    //    FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 3;
                                    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3].Text = treeview_spreadfields.Nodes[remv].Text.ToString() + " Total Alloted";
                                    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3].BackColor = Color.MistyRose;
                                    //    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3, 3, 1);
                                    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Text = treeview_spreadfields.Nodes[remv].Text.ToString() + " Total Paid";
                                    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].BackColor = Color.MistyRose;
                                    //    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2, 3, 1);
                                    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = treeview_spreadfields.Nodes[remv].Text.ToString() + " Total Balance";
                                    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].BackColor = Color.MistyRose;
                                    //    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1, 3, 1);

                                    //}
                                }
                            }
                            if (ddlfeetype.Text == "Paid")
                            {
                                FpSpread1.Sheets[0].ColumnCount++;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Total Paid";
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1, 3, 1);
                            }
                            else if (ddlfeetype.Text == "Yet To Be Paid")
                            {
                                FpSpread1.Sheets[0].ColumnCount++;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Total Balance";
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1, 3, 1);
                            }
                            else
                            {
                                FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 5;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 5].Text = "Total Alloted";
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 5, 3, 1);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 4].Text = "Total Paid";
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 4, 3, 1);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3].Text = "Total Balance";
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3, 3, 1);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Text = "Excess Amount";
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2, 3, 1);

                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Status";
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1, 3, 1);
                            }

                            if (category.Trim() != "" && category != null)
                            {
                                strstufillerfee = strstufillerfee + " and a.LedgerFK in(" + getfeeval + ")";
                                strstufillerfeeapplyn = strstufillerfeeapplyn + " and a.LedgerFK in(" + getfeeval + ")";
                                demandquery = " and a.LedgerFK in('" + getfeeval + "')";
                            }

                            feecategorycolumn = feecategorycolumn + ",a.LedgerFK as fee_code";
                            feecategorycolumngroup = feecategorycolumngroup + ",a.LedgerFK";
                            #endregion
                        }
                        else if (ddlacctype.SelectedItem.Text == "Group Header")//Group Header
                        {
                            #region Without Date Group Header

                            #region design

                            //#region groupnoneed

                            ////Modified by srinath 10/6/2015
                            ////string acchead = "select distinct header_name,ChlHeaderName,header_id from acctheader";
                            string acchead = "select distinct ChlGroupHeader from FS_ChlGroupHeaderSettings ";
                            ds = da.select_method_wo_parameter(acchead, "text");
                            for (int j = 0; j < chklstaccheader.Items.Count; j++)
                            {
                                if (chklstaccheader.Items[j].Selected == true)
                                {
                                    string grhead = chklstaccheader.Items[j].Text.ToString();
                                    ds.Tables[0].DefaultView.RowFilter = " ChlGroupHeader='" + grhead + "'";
                                    dvhead = ds.Tables[0].DefaultView;
                                    int ccount = 0;
                                    int hstartc = 0;

                                    Boolean headflag = false;
                                    for (int a = 0; a < dvhead.Count; a++)
                                    {
                                        if (ddlfeetype.SelectedItem.Text == "Paid")
                                        {
                                            FpSpread1.Sheets[0].ColumnCount++;
                                            if (headflag == false)
                                            {
                                                // FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = chklstaccheader.Items[j].Text;
                                                headflag = true;
                                                hstartc = FpSpread1.Sheets[0].ColumnCount - 1;
                                            }
                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                // feecode = dvhead[a]["header_id"].ToString();

                                                FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Width = 200;
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = dvhead[a]["ChlGroupHeader"].ToString();
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Paid";
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Note = feecode;
                                                if (head == "")
                                                {
                                                    // head = dvhead[a]["header_id"].ToString();
                                                }
                                                else
                                                {
                                                    // head = head + "," + dvhead[a]["header_id"].ToString();
                                                }

                                            }
                                            ccount += 1;
                                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - ccount, 1, ccount);

                                        }
                                        else if (ddlfeetype.SelectedItem.Text == "Yet To Be Paid")
                                        {
                                            FpSpread1.Sheets[0].ColumnCount++;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = chklstaccheader.Items[j].Text;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Balance";
                                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                                        }
                                        else
                                        {
                                            #region old
                                            //FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 3;
                                            //if (headflag == false)
                                            //{
                                            //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 3].Text = chklstaccheader.Items[j].Text;
                                            //    headflag = true;
                                            //    hstartc = FpSpread1.Sheets[0].ColumnCount - 3;
                                            //}
                                            //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3, 1, 3);
                                            //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 3].Text = dvhead[a]["ChlGroupheader"].ToString();
                                            //if (dvhead.Count > 0)
                                            //{
                                            //    feecode = dvhead[a]["ChlGroupheader"].ToString();
                                            //    FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3].Text = "Receivable";
                                            //    FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3].Note = feecode;
                                            //    FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Text = "Received";
                                            //    FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Note = feecode;
                                            //    FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Dues";
                                            //    FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Note = feecode;
                                            //    if (head == "")
                                            //    {
                                            //        head = dvhead[a]["ChlGroupheader"].ToString();
                                            //    }
                                            //    else
                                            //    {
                                            //        head = head + "," + dvhead[a]["ChlGroupheader"].ToString();
                                            //    }
                                            //}
                                            //ccount += 3;
                                            //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - ccount, 1, ccount);

                                            #endregion
                                            #region new
                                            FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 3;
                                            if (headflag == false)
                                            {
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 3].Text = chklstaccheader.Items[j].Text;
                                                headflag = true;
                                            }
                                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3, 1, 3);
                                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3, 1, 3);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3].Text = "Alloted";
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3].Note = chklstaccheader.Items[a].Value;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Text = "Paid";
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Note = chklstaccheader.Items[a].Value;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Balance";
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Note = chklstaccheader.Items[a].Value;
                                            #endregion

                                        }
                                    }
                                }
                            }
                            if (ddlfeetype.Text == "Paid")
                            {
                                FpSpread1.Sheets[0].ColumnCount++;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Total Paid";
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1, 3, 1);
                            }
                            else if (ddlfeetype.Text == "Yet To Be Paid")
                            {
                                FpSpread1.Sheets[0].ColumnCount++;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Total Balance";
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1, 3, 1);
                            }
                            else
                            {
                                FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 4;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 4].Text = "Total Alloted";
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 4, 3, 1);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3].Text = "Total Paid";
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3, 3, 1);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Text = "Total Balance";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Status";
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2, 3, 1);
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1, 3, 1);
                            }
                            #endregion

                            #region

                            string cc = "";
                            string debar = "";
                            string disc = "";
                            string commondist = "";

                            if (cblinclude.Items.Count > 0)
                            {
                                for (int i = 0; i < cblinclude.Items.Count; i++)
                                {
                                    if (cblinclude.Items[i].Selected == true)
                                    {
                                        if (cblinclude.Items[i].Value == "1")
                                        {
                                            cc = "r.cc=1";
                                        }
                                        if (cblinclude.Items[i].Value == "2")
                                        {
                                            debar = "r.Exam_Flag like '%debar'";
                                        }
                                        if (cblinclude.Items[i].Value == "3")
                                        {
                                            disc = "r.DelFlag=1";
                                        }
                                    }
                                }
                            }

                            if (cc != "" && debar == "" && disc == "")
                                commondist = " and (" + cc + " or r.cc=0)  and r.Exam_Flag<>'debar' and r.DelFlag=0";

                            if (cc == "" && debar != "" && disc == "")
                                commondist = " and r.cc=0  and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";

                            if (cc == "" && debar == "" && disc != "")
                                commondist = " and r.cc=0  and r.Exam_Flag<>'debar' and (" + disc + " or r.DelFlag=0)";

                            if (cc != "" && debar != "" && disc == "")
                                commondist = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";

                            if (cc == "" && debar != "" && disc != "")
                                commondist = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar')  and (" + disc + " or r.DelFlag=0)";

                            if (cc != "" && debar == "" && disc != "")
                                commondist = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar'  and (" + disc + " or r.DelFlag=0)";

                            else if (cc == "" && debar == "" && disc == "")
                                commondist = " and r.cc=0  and r.Exam_Flag<>'debar' and r.DelFlag=0";

                            if (cc != "" && debar != "" && disc != "")
                                commondist = "";

                            #endregion


                            if (chkyettobepaid.Checked == false)
                            {
                                if (ddlfeetype.SelectedItem.Text == "Yet To Be Paid")
                                {
                                    #region
                                    // Jairam New Updation
                                    string typevalue = "";
                                    if (ddltype.Enabled == true)
                                    {
                                        typevalue = Convert.ToString(ddltype.SelectedItem.Text);
                                    }
                                    #region Query


                                    string Query = " select  f.App_No,sum (TotalAmount),SUM( PaidAmount),sum(BalAmount)as Balance,FeeCategory,ChlGroupHeader from FT_FeeAllot f,Registration r ,Degree d,Department dt,Course c,FS_ChlGroupHeaderSettings G where f.App_No =r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and G.HeaderFK =F.HeaderFK and Stream =c.type and ISNULL (BalAmount ,TotalAmount)<>0  and d.college_code ='" + collegecode + "' ";
                                    if (commondist != "")
                                        Query = Query + commondist;

                                    if (typevalue.Trim() != "Both" && typevalue.Trim() != "")
                                    {
                                        Query = Query + " and type ='" + typevalue + "'";
                                    }
                                    if (batch_all.Trim() != "")
                                    {
                                        Query = Query + " and r.batch_year  in (" + batch_all + ")";
                                    }
                                    if (course_all.Trim() != "")
                                    {
                                        Query = Query + " and r.degree_code  in (" + course_all + ")";
                                    }
                                    if (stefeesem.Trim() != "")
                                    {
                                        Query = Query + " and f.FeeCategory  in (" + stefeesem + ")";
                                    }
                                    if (finyearval.Trim() != "")
                                    {
                                        Query = Query + " and f.FinYearFK   in (" + Newfinayear + ")";
                                    }
                                    if (header_all.Trim() != "")
                                    {
                                        Query = Query + " and ChlGroupHeader   in ('" + header_all + "')";
                                    }
                                    Query = Query + " group by ChlGroupHeader,f.App_No,FeeCategory Order by f.app_no";
                                    Query = Query + " select distinct f.App_No ,r.degree_code,r.Batch_Year  from FT_FeeAllot f,Registration r ,Degree d,Department dt,Course c ,FS_ChlGroupHeaderSettings G where G.HeaderFK =f.HeaderFK and f.App_No =r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and ISNULL ( BalAmount ,TotalAmount)<>0  and d.college_code ='" + collegecode + "' ";

                                    if (commondist != "")
                                        Query = Query + commondist;
                                    if (typevalue.Trim() != "Both" && typevalue.Trim() != "")
                                    {
                                        Query = Query + " and type ='" + typevalue + "'";
                                    }
                                    if (batch_all.Trim() != "")
                                    {
                                        Query = Query + " and r.batch_year  in (" + batch_all + ")";
                                    }
                                    if (course_all.Trim() != "")
                                    {
                                        Query = Query + " and r.degree_code  in (" + course_all + ")";
                                    }
                                    if (stefeesem.Trim() != "")
                                    {
                                        Query = Query + " and f.FeeCategory  in (" + stefeesem + ")";
                                    }
                                    if (header_all.Trim() != "")
                                    {
                                        Query = Query + " and ChlGroupHeader   in ('" + header_all + "')";
                                    }
                                    if (finyearval.Trim() != "")
                                    {
                                        Query = Query + " and f.FinYearFK   in (" + Newfinayear + ")";
                                    }
                                    if (chkfeeroll.Checked == true)
                                    {
                                        Query = Query + " and r.Roll_No not in(select s.roll_no from stucon s where s.roll_no=r.roll_no and s.semester=r.Current_Semester and s.ack_fee_of_roll=1)";
                                    }
                                    Query += "   select distinct f.App_No,r.Roll_No,r.roll_admit,r.Reg_No,r.Stud_Name,FeeCategory,r.degree_code from FT_FeeAllot f,Registration r,FS_ChlGroupHeaderSettings G  where f.App_No =r.App_No and g.HeaderFK =f.HeaderFK and ISNULL ( BalAmount ,TotalAmount)<>0  and r.college_code ='" + collegecode + "'";
                                    if (commondist != "")
                                        Query = Query + commondist;
                                    if (typevalue.Trim() != "Both" && typevalue.Trim() != "")
                                    {
                                        Query = Query + " and g.Stream ='" + typevalue + "'";
                                    }
                                    if (batch_all.Trim() != "")
                                    {
                                        Query = Query + " and r.batch_year  in (" + batch_all + ")";
                                    }
                                    if (course_all.Trim() != "")
                                    {
                                        Query = Query + " and r.degree_code  in (" + course_all + ")";
                                    }
                                    if (stefeesem.Trim() != "")
                                    {
                                        Query = Query + " and f.FeeCategory  in (" + stefeesem + ")";
                                    }
                                    if (finyearval.Trim() != "")
                                    {
                                        Query = Query + " and f.FinYearFK   in (" + Newfinayear + ")";
                                    }
                                    if (header_all.Trim() != "")
                                    {
                                        Query = Query + " and ChlGroupHeader   in ('" + header_all + "')";
                                    }
                                    //Query = Query + " select distinct f.App_No,r.Roll_No,r.Reg_No,r.Stud_Name,(C.Course_Name +'-'+dt.Dept_Name) as Degree,FeeCategory,r.degree_code,T.TextVal from FT_FeeAllot f,Registration r ,Degree d,Department dt,Course c ,FS_ChlGroupHeaderSettings G, TextValTable T where T.TextCode =F.FeeCategory and G.HeaderFK =F.HeaderFK and f.App_No =r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and ISNULL ( BalAmount ,TotalAmount)<>0  and d.college_code ='" + collegecode + "'";
                                    ////if (checkdicon.Checked == true)
                                    ////{
                                    ////    Query = Query + " and DelFlag =1";
                                    ////}
                                    ////else
                                    ////{
                                    ////    Query = Query + " and DelFlag =0";
                                    ////}
                                    //if (cc != "")
                                    //    Query = Query + cc;
                                    //else
                                    //    Query = Query + " and r.cc=0";

                                    //if (debar != "")
                                    //    Query = Query + debar;
                                    //else
                                    //    Query = Query + " and r.Exam_Flag<>'debar'";

                                    //if (disc != "")
                                    //    Query = Query + disc;
                                    //else
                                    //    Query = Query + " and r.DelFlag=0";
                                    ////if (checkdicon.Checked == false)
                                    ////{
                                    ////    Query = Query + " and DelFlag =0";
                                    ////}
                                    //if (typevalue.Trim() != "Both" && typevalue.Trim() != "")
                                    //{
                                    //    Query = Query + " and type ='" + typevalue + "'";
                                    //}
                                    //if (batch_all.Trim() != "")
                                    //{
                                    //    Query = Query + " and r.batch_year  in (" + batch_all + ")";
                                    //}
                                    //if (course_all.Trim() != "")
                                    //{
                                    //    Query = Query + " and r.degree_code  in (" + course_all + ")";
                                    //}
                                    //if (stefeesem.Trim() != "")
                                    //{
                                    //    Query = Query + " and f.FeeCategory  in (" + stefeesem + ")";
                                    //}
                                    //if (finyearval.Trim() != "")
                                    //{
                                    //    Query = Query + " and f.FinYearFK   in (" + Newfinayear + ")";
                                    //}
                                    //if (header_all.Trim() != "")
                                    //{
                                    //    Query = Query + " and ChlGroupHeader   in ('" + header_all + "')";
                                    //}

                                    Query = Query + " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode + "'";
                                    Query = Query + " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code ='" + collegecode + "'";
                                    #endregion

                                    ds.Clear();
                                    ds = da.select_method_wo_parameter(Query, "Text");
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        #region value

                                        DataView dv = new DataView();
                                        DataView dv1 = new DataView();
                                        DataView dv2 = new DataView();
                                        finalflag = true;
                                        Hashtable GrandTotal = new Hashtable();
                                        if (chklstbranch.Items.Count > 0)
                                        {
                                            for (int batch = 0; batch < chklstbatch.Items.Count; batch++)
                                            {
                                                if (chklstbatch.Items[batch].Selected == true)
                                                {
                                                    for (int Bnch = 0; Bnch < chklstbranch.Items.Count; Bnch++)
                                                    {
                                                        if (chklstbranch.Items[Bnch].Selected == true)
                                                        {
                                                            //for (int batch = 0; batch < chklstbatch.Items.Count; batch++)
                                                            //{
                                                            //    if (chklstbatch.Items[batch].Selected == true)
                                                            //    {
                                                            ds.Tables[1].DefaultView.RowFilter = "degree_code='" + chklstbranch.Items[Bnch].Value + "' and Batch_Year='" + Convert.ToString(chklstbatch.Items[batch].Text) + "'";
                                                            dv2 = ds.Tables[1].DefaultView;
                                                            if (dv2.Count > 0)
                                                            {
                                                                FpSpread1.Sheets[0].RowCount++;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(chklstbranch.Items[Bnch].Text) + " - " + Convert.ToString(chklstbatch.Items[batch].Text);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.LightGreen;
                                                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, FpSpread1.Sheets[0].ColumnCount - 1);
                                                                for (int row = 0; row < dv2.Count; row++)
                                                                {
                                                                    string app_no = Convert.ToString(dv2[row]["App_No"]);
                                                                    ds.Tables[2].DefaultView.RowFilter = "App_No=" + Convert.ToString(app_no) + " and degree_code='" + chklstbranch.Items[Bnch].Value + "'";
                                                                    dv = ds.Tables[2].DefaultView;
                                                                    for (int drow = 0; drow < dv.Count; drow++)
                                                                    {
                                                                        sno++;
                                                                        string dvapp_no = Convert.ToString(dv[drow]["App_No"]);
                                                                        string Feecategory = Convert.ToString(dv[drow]["FeeCategory"]);
                                                                        FpSpread1.Sheets[0].RowCount++;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dv[drow]["Stud_Name"]);
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dv[drow]["Roll_No"]);
                                                                        if (sclflag == false)
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv[drow]["Reg_No"]);
                                                                        else
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv[drow]["roll_admit"]);
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = txtreg;

                                                                        DataView Dview = new DataView();
                                                                        string Degreename = "";
                                                                        if (ds.Tables[3].Rows.Count > 0)
                                                                        {
                                                                            ds.Tables[3].DefaultView.RowFilter = "Degree_code='" + Convert.ToString(dv[drow]["Degree_code"]) + "'";
                                                                            Dview = ds.Tables[3].DefaultView;
                                                                            if (Dview.Count > 0)
                                                                            {
                                                                                Degreename = Convert.ToString(Dview[0]["degreename"]);
                                                                            }
                                                                        }
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Degreename;
                                                                        string TextName = "";
                                                                        if (ds.Tables[4].Rows.Count > 0)
                                                                        {
                                                                            ds.Tables[4].DefaultView.RowFilter = "TextCode='" + Convert.ToString(dv[drow]["FeeCategory"]) + "'";
                                                                            Dview = ds.Tables[4].DefaultView;
                                                                            if (Dview.Count > 0)
                                                                            {
                                                                                TextName = Convert.ToString(Dview[0]["TextVal"]);
                                                                            }
                                                                        }
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = TextName;





                                                                        // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dv[drow]["Degree"]);
                                                                        //if (feecateflag == true)
                                                                        //{
                                                                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dv[drow]["TextVal"]);
                                                                        //}
                                                                        //else
                                                                        //{
                                                                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dv[drow]["TextVal"]);
                                                                        //}
                                                                        int col = 6;
                                                                        double Total = 0;
                                                                        double balance = 0;
                                                                        for (int j = 0; j < chklstaccheader.Items.Count; j++)
                                                                        {
                                                                            if (chklstaccheader.Items[j].Selected == true)
                                                                            {
                                                                                ds.Tables[0].DefaultView.RowFilter = "App_no='" + dvapp_no + "' and FeeCategory='" + Feecategory + "' and ChlGroupHeader='" + Convert.ToString(chklstaccheader.Items[j].Text) + "'";
                                                                                dv1 = ds.Tables[0].DefaultView;
                                                                                col++;
                                                                                if (dv1.Count > 0)
                                                                                {
                                                                                    double.TryParse(Convert.ToString(dv1[0]["Balance"]), out balance);
                                                                                    Total += balance;
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(balance);
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                                                                    if (!GrandTotal.ContainsKey(Convert.ToString(chklstaccheader.Items[j].Text)))
                                                                                        GrandTotal.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(balance));
                                                                                    else
                                                                                    {
                                                                                        double total = 0;
                                                                                        double.TryParse(Convert.ToString(GrandTotal[Convert.ToString(chklstaccheader.Items[j].Text)]), out balance);
                                                                                        total += balance;
                                                                                        GrandTotal.Remove(Convert.ToString(chklstaccheader.Items[j].Text));
                                                                                        GrandTotal.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(total));
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = "-";
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                                                                }
                                                                            }
                                                                        }
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(Total);
                                                                        //Total += Convert.ToDouble(dv1[0]["Balance"]);
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                                                        if (!GrandTotal.ContainsKey(Convert.ToString("Total")))
                                                                            GrandTotal.Add(Convert.ToString("Total"), Convert.ToString(Total));
                                                                        else
                                                                        {
                                                                            double total = 0;
                                                                            double.TryParse(Convert.ToString(GrandTotal[Convert.ToString("Total")]), out total);
                                                                            total += Total;
                                                                            GrandTotal.Remove(Convert.ToString("Total"));
                                                                            GrandTotal.Add(Convert.ToString("Total"), Convert.ToString(total));
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            FpSpread1.Sheets[0].RowCount++;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString("Grand Total");
                                            int col1 = 6;
                                            for (int j = 0; j < chklstaccheader.Items.Count; j++)
                                            {
                                                if (chklstaccheader.Items[j].Selected == true)
                                                {
                                                    col1++;
                                                    string GetNewValue = Convert.ToString(GrandTotal[Convert.ToString(chklstaccheader.Items[j].Text)]);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].Text = Convert.ToString(GetNewValue);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].HorizontalAlign = HorizontalAlign.Right;
                                                }
                                            }
                                            col1++;
                                            string GetNewValue1 = Convert.ToString(GrandTotal[Convert.ToString("Total")]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].Text = Convert.ToString(GetNewValue1);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].HorizontalAlign = HorizontalAlign.Right;
                                        }
                                        if (FpSpread1.Sheets[0].RowCount > 0)
                                        {
                                            FpSpread1.Visible = true;
                                            btnprintmaster.Visible = true;
                                            lblrptname.Visible = true;
                                            txtexcelname.Visible = true;
                                            btnxl.Visible = true;
                                        }
                                        else
                                        {
                                            FpSpread1.Visible = false;
                                            btnprintmaster.Visible = false;
                                            lblrptname.Visible = false;
                                            txtexcelname.Visible = false;
                                            btnxl.Visible = false;
                                        }
                                        #endregion
                                    }
                                    #endregion
                                }
                                else if (ddlfeetype.SelectedItem.Text == "Both")
                                {
                                    #region
                                    // Jairam New Updation
                                    string typevalue = "";
                                    if (ddltype.Enabled == true)
                                    {
                                        typevalue = Convert.ToString(ddltype.SelectedItem.Text);
                                    }

                                    #region query

                                    string Query = " select  f.App_No,sum (TotalAmount) as Total,SUM( isnull(PaidAmount,'0')) as Paid,sum(BalAmount)as Balance,FeeCategory,ChlGroupHeader from FT_FeeAllot f,Registration r ,Degree d,Department dt,Course c,FS_ChlGroupHeaderSettings G where f.App_No =r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and G.HeaderFK =F.HeaderFK and Stream =c.type  and d.college_code ='" + collegecode + "' ";

                                    if (commondist != "")
                                        Query = Query + commondist;

                                    if (typevalue.Trim() != "Both" && typevalue.Trim() != "")
                                    {
                                        Query = Query + " and type ='" + typevalue + "'";
                                    }
                                    if (batch_all.Trim() != "")
                                    {
                                        Query = Query + " and r.batch_year  in (" + batch_all + ")";
                                    }
                                    if (course_all.Trim() != "")
                                    {
                                        Query = Query + " and r.degree_code  in (" + course_all + ")";
                                    }
                                    if (stefeesem.Trim() != "")
                                    {
                                        Query = Query + " and f.FeeCategory  in (" + stefeesem + ")";
                                    }
                                    if (finyearval.Trim() != "")
                                    {
                                        Query = Query + " and f.FinYearFK   in (" + Newfinayear + ")";
                                    }
                                    if (header_all.Trim() != "")
                                    {
                                        Query = Query + " and ChlGroupHeader   in ('" + header_all + "')";
                                    }
                                    Query = Query + " group by ChlGroupHeader,f.App_No,FeeCategory Order by f.app_no";

                                    Query = Query + " select  f.App_No,sum (Debit) as TotalPaid,FeeCategory,ChlGroupHeader from FT_FinDailyTransaction f,Registration r ,Degree d,Department dt,Course c,FS_ChlGroupHeaderSettings G where f.App_No =r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and G.HeaderFK =F.HeaderFK and Stream =c.type  and d.college_code ='" + collegecode + "'  and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' ";
                                    if (usBasedRights == true)
                                        Query += " and f.EntryUserCode in('" + usercode + "')";

                                    if (commondist != "")
                                        Query = Query + commondist;
                                    if (typevalue.Trim() != "Both" && typevalue.Trim() != "")
                                    {
                                        Query = Query + " and type ='" + typevalue + "'";
                                    }
                                    if (batch_all.Trim() != "")
                                    {
                                        Query = Query + " and r.batch_year  in (" + batch_all + ")";
                                    }
                                    if (course_all.Trim() != "")
                                    {
                                        Query = Query + " and r.degree_code  in (" + course_all + ")";
                                    }
                                    if (stefeesem.Trim() != "")
                                    {
                                        Query = Query + " and f.FeeCategory  in (" + stefeesem + ")";
                                    }
                                    if (finyearval.Trim() != "")
                                    {
                                        Query = Query + " and f.FinYearFK   in (" + Newfinayear + ")";
                                    }
                                    if (header_all.Trim() != "")
                                    {
                                        Query = Query + " and ChlGroupHeader   in ('" + header_all + "')";
                                    }
                                    Query = Query + " group by ChlGroupHeader,f.App_No,FeeCategory Order by f.app_no";
                                    Query = Query + " select distinct f.App_No ,r.degree_code,r.Batch_Year  from FT_FeeAllot f,Registration r ,Degree d,Department dt,Course c ,FS_ChlGroupHeaderSettings G where G.HeaderFK =f.HeaderFK and f.App_No =r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code ='" + collegecode + "'";
                                    if (commondist != "")
                                        Query = Query + commondist;

                                    if (typevalue.Trim() != "Both" && typevalue.Trim() != "")
                                    {
                                        Query = Query + " and type ='" + typevalue + "'";
                                    }
                                    if (batch_all.Trim() != "")
                                    {
                                        Query = Query + " and r.batch_year  in (" + batch_all + ")";
                                    }
                                    if (course_all.Trim() != "")
                                    {
                                        Query = Query + " and r.degree_code  in (" + course_all + ")";
                                    }
                                    if (stefeesem.Trim() != "")
                                    {
                                        Query = Query + " and f.FeeCategory  in (" + stefeesem + ")";
                                    }
                                    if (header_all.Trim() != "")
                                    {
                                        Query = Query + " and ChlGroupHeader   in ('" + header_all + "')";
                                    }
                                    if (finyearval.Trim() != "")
                                    {
                                        Query = Query + " and f.FinYearFK   in (" + Newfinayear + ")";
                                    }
                                    if (chkfeeroll.Checked == true)
                                    {
                                        Query = Query + " and r.Roll_No not in(select s.roll_no from stucon s where s.roll_no=r.roll_no and s.semester=r.Current_Semester and s.ack_fee_of_roll=1)";
                                    }

                                    Query += "   select distinct f.App_No,r.Roll_No,r.roll_admit,r.Reg_No,r.Stud_Name,FeeCategory,r.degree_code from FT_FeeAllot f,Registration r,FS_ChlGroupHeaderSettings G  where f.App_No =r.App_No and g.HeaderFK =f.HeaderFK and r.college_code ='" + collegecode + "'";
                                    if (commondist != "")
                                        Query = Query + commondist;
                                    if (typevalue.Trim() != "Both" && typevalue.Trim() != "")
                                    {
                                        Query = Query + " and g.Stream ='" + typevalue + "'";
                                    }
                                    if (batch_all.Trim() != "")
                                    {
                                        Query = Query + " and r.batch_year  in (" + batch_all + ")";
                                    }
                                    if (course_all.Trim() != "")
                                    {
                                        Query = Query + " and r.degree_code  in (" + course_all + ")";
                                    }
                                    if (stefeesem.Trim() != "")
                                    {
                                        Query = Query + " and f.FeeCategory  in (" + stefeesem + ")";
                                    }
                                    if (finyearval.Trim() != "")
                                    {
                                        Query = Query + " and f.FinYearFK   in (" + Newfinayear + ")";
                                    }
                                    if (header_all.Trim() != "")
                                    {
                                        Query = Query + " and ChlGroupHeader   in ('" + header_all + "')";
                                    }
                                    //Query = Query + " select distinct f.App_No,r.Roll_No,r.Reg_No,r.Stud_Name,(C.Course_Name +'-'+dt.Dept_Name) as Degree,FeeCategory,r.degree_code,T.TextVal from FT_FeeAllot f,Registration r ,Degree d,Department dt,Course c ,FS_ChlGroupHeaderSettings G, TextValTable T where T.TextCode =F.FeeCategory and G.HeaderFK =F.HeaderFK and f.App_No =r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id ";
                                    //if (commondist != "")
                                    //    Query = Query + commondist;

                                    //if (typevalue.Trim() != "Both" && typevalue.Trim() != "")
                                    //{
                                    //    Query = Query + " and type ='" + typevalue + "'";
                                    //}
                                    //if (batch_all.Trim() != "")
                                    //{
                                    //    Query = Query + " and r.batch_year  in (" + batch_all + ")";
                                    //}
                                    //if (course_all.Trim() != "")
                                    //{
                                    //    Query = Query + " and r.degree_code  in (" + course_all + ")";
                                    //}
                                    //if (stefeesem.Trim() != "")
                                    //{
                                    //    Query = Query + " and f.FeeCategory  in (" + stefeesem + ")";
                                    //}
                                    //if (finyearval.Trim() != "")
                                    //{
                                    //    Query = Query + " and f.FinYearFK   in (" + Newfinayear + ")";
                                    //}
                                    //if (header_all.Trim() != "")
                                    //{
                                    //    Query = Query + " and ChlGroupHeader   in ('" + header_all + "')";
                                    //}

                                    Query = Query + " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode + "'";
                                    Query = Query + " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code ='" + collegecode + "'";
                                    #endregion

                                    ds.Clear();
                                    ds = da.select_method_wo_parameter(Query, "Text");
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        #region value

                                        string gettotval1 = "";
                                        double getpaidval1 = 0;
                                        string GetNewValue1 = "";
                                        int col1 = 0;
                                        DataView dv = new DataView();
                                        DataView dv1 = new DataView();
                                        DataView dv2 = new DataView();
                                        finalflag = true;
                                        Hashtable GrandTotal = new Hashtable();
                                        Hashtable totval = new Hashtable();
                                        Hashtable paidval = new Hashtable();

                                        Hashtable gdallot = new Hashtable();
                                        Hashtable gdpaid = new Hashtable();
                                        Hashtable gdbal = new Hashtable();
                                        if (chklstbatch.Items.Count > 0)
                                        {

                                            for (int batch = 0; batch < chklstbatch.Items.Count; batch++)
                                            {
                                                if (chklstbatch.Items[batch].Selected == true)
                                                {
                                                    if (chklstbranch.Items.Count > 0)
                                                    {
                                                        for (int Bnch = 0; Bnch < chklstbranch.Items.Count; Bnch++)
                                                        {
                                                            if (chklstbranch.Items[Bnch].Selected == true)
                                                            {
                                                                ds.Tables[2].DefaultView.RowFilter = "degree_code='" + chklstbranch.Items[Bnch].Value + "' and Batch_Year='" + Convert.ToString(chklstbatch.Items[batch].Text) + "'";
                                                                dv2 = ds.Tables[2].DefaultView;
                                                                if (dv2.Count > 0)
                                                                {
                                                                    FpSpread1.Sheets[0].RowCount++;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(chklstbranch.Items[Bnch].Text) + " - " + Convert.ToString(chklstbatch.Items[batch].Text);
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.LightGreen;
                                                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, FpSpread1.Sheets[0].ColumnCount - 1);
                                                                    for (int row = 0; row < dv2.Count; row++)
                                                                    {
                                                                        string app_no = Convert.ToString(dv2[row]["App_No"]);
                                                                        ds.Tables[3].DefaultView.RowFilter = "App_No=" + Convert.ToString(app_no) + " and degree_code='" + chklstbranch.Items[Bnch].Value + "'";
                                                                        dv = ds.Tables[3].DefaultView;
                                                                        for (int drow = 0; drow < dv.Count; drow++)
                                                                        {
                                                                            sno++;
                                                                            int col = 6;
                                                                            string dvapp_no = Convert.ToString(dv[drow]["App_No"]);
                                                                            string Feecategory = Convert.ToString(dv[drow]["FeeCategory"]);
                                                                            FpSpread1.Sheets[0].RowCount++;
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dv[drow]["Stud_Name"]);
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dv[drow]["Roll_No"]);
                                                                            if (sclflag == false)
                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv[drow]["Reg_No"]);
                                                                            else
                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv[drow]["roll_admit"]);
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = txtreg;


                                                                            DataView Dview = new DataView();
                                                                            string Degreename = "";
                                                                            if (ds.Tables[4].Rows.Count > 0)
                                                                            {
                                                                                ds.Tables[4].DefaultView.RowFilter = "Degree_code='" + Convert.ToString(dv[drow]["Degree_code"]) + "'";
                                                                                Dview = ds.Tables[4].DefaultView;
                                                                                if (Dview.Count > 0)
                                                                                {
                                                                                    Degreename = Convert.ToString(Dview[0]["degreename"]);
                                                                                }
                                                                            }
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Degreename;
                                                                            string TextName = "";
                                                                            if (ds.Tables[5].Rows.Count > 0)
                                                                            {
                                                                                ds.Tables[5].DefaultView.RowFilter = "TextCode='" + Convert.ToString(dv[drow]["FeeCategory"]) + "'";
                                                                                Dview = ds.Tables[5].DefaultView;
                                                                                if (Dview.Count > 0)
                                                                                {
                                                                                    TextName = Convert.ToString(Dview[0]["TextVal"]);
                                                                                }
                                                                            }
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = TextName;

                                                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dv[drow]["Degree"]);
                                                                            //if (feecateflag == true)
                                                                            //{
                                                                            //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dv[drow]["TextVal"]);
                                                                            //}
                                                                            //else
                                                                            //{
                                                                            //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dv[drow]["TextVal"]);
                                                                            //}

                                                                            #region allot,paid,total
                                                                            double Total = 0;
                                                                            double totpaid = 0;
                                                                            double totamt = 0;
                                                                            for (int j = 0; j < chklstaccheader.Items.Count; j++)
                                                                            {
                                                                                if (chklstaccheader.Items[j].Selected == true)
                                                                                {
                                                                                    ds.Tables[0].DefaultView.RowFilter = "App_no='" + dvapp_no + "' and FeeCategory='" + Feecategory + "' and ChlGroupHeader='" + Convert.ToString(chklstaccheader.Items[j].Text) + "'";
                                                                                    dv1 = ds.Tables[0].DefaultView;
                                                                                    //col++;                                                                                  
                                                                                    double totalamount = 0;
                                                                                    double paidamount = 0;
                                                                                    double balanceamt = 0;
                                                                                    if (dv1.Count > 0)
                                                                                    {
                                                                                        double.TryParse(Convert.ToString(dv1[0]["Total"]), out totalamount);
                                                                                        totamt += totalamount;
                                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ++col].Text = Convert.ToString(totalamount);
                                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                                                                        DataView dvpaid = new DataView();
                                                                                        ds.Tables[1].DefaultView.RowFilter = "App_no='" + dvapp_no + "' and FeeCategory='" + Feecategory + "' and ChlGroupHeader='" + Convert.ToString(chklstaccheader.Items[j].Text) + "'";
                                                                                        dvpaid = ds.Tables[1].DefaultView;
                                                                                        if (dvpaid.Count > 0)
                                                                                        {
                                                                                            double.TryParse(Convert.ToString(dvpaid[0]["TotalPaid"]), out paidamount);
                                                                                            totpaid += paidamount;
                                                                                        }
                                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ++col].Text = Convert.ToString(paidamount);
                                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                                                                        balanceamt = totalamount - paidamount;
                                                                                        Total += balanceamt;
                                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ++col].Text = Convert.ToString(balanceamt);
                                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;

                                                                                        if (!GrandTotal.ContainsKey(Convert.ToString(chklstaccheader.Items[j].Text)))
                                                                                            GrandTotal.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(balanceamt));
                                                                                        else
                                                                                        {
                                                                                            double total = 0;
                                                                                            double.TryParse(Convert.ToString(GrandTotal[Convert.ToString(chklstaccheader.Items[j].Text)]), out total);
                                                                                            total += balanceamt;
                                                                                            GrandTotal.Remove(Convert.ToString(chklstaccheader.Items[j].Text));
                                                                                            GrandTotal.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(total));
                                                                                        }

                                                                                        if (!totval.ContainsKey(Convert.ToString(chklstaccheader.Items[j].Text)))
                                                                                            totval.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(totalamount));
                                                                                        else
                                                                                        {
                                                                                            double totvalue = 0;
                                                                                            double.TryParse(Convert.ToString(totval[Convert.ToString(chklstaccheader.Items[j].Text)]), out totvalue);
                                                                                            totvalue += totalamount;
                                                                                            totval.Remove(Convert.ToString(chklstaccheader.Items[j].Text));
                                                                                            totval.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(totvalue));
                                                                                        }

                                                                                        if (!paidval.ContainsKey(Convert.ToString(chklstaccheader.Items[j].Text)))
                                                                                            paidval.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(paidamount));
                                                                                        else
                                                                                        {
                                                                                            double paidvalue = 0;
                                                                                            double.TryParse(Convert.ToString(paidval[Convert.ToString(chklstaccheader.Items[j].Text)]), out paidvalue);
                                                                                            paidvalue += paidamount;
                                                                                            paidval.Remove(Convert.ToString(chklstaccheader.Items[j].Text));
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        col += 3;
                                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = "-";
                                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                                                                    }
                                                                                }
                                                                            }
                                                                            #endregion

                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 4].Text = Convert.ToString(totamt);
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 4].HorizontalAlign = HorizontalAlign.Right;
                                                                            if (!totval.ContainsKey(Convert.ToString("totamt")))
                                                                                totval.Add(Convert.ToString("totamt"), Convert.ToString(totamt));
                                                                            else
                                                                            {
                                                                                double total = 0;
                                                                                double.TryParse(Convert.ToString(totval[Convert.ToString("totamt")]), out total);
                                                                                total += Convert.ToDouble(totamt);
                                                                                totval.Remove(Convert.ToString("totamt"));
                                                                                totval.Add(Convert.ToString("totamt"), Convert.ToString(total));
                                                                            }
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 3].Text = Convert.ToString(totpaid);
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Right;
                                                                            if (!paidval.ContainsKey(Convert.ToString("totpaid")))
                                                                                paidval.Add(Convert.ToString("totpaid"), Convert.ToString(totpaid));
                                                                            else
                                                                            {
                                                                                double total = 0;
                                                                                double.TryParse(Convert.ToString(paidval[Convert.ToString("totpaid")]), out total);
                                                                                total += Convert.ToDouble(totpaid);
                                                                                paidval.Remove(Convert.ToString("totpaid"));
                                                                            }
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Text = Convert.ToString(Total);

                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Right;
                                                                            if (!GrandTotal.ContainsKey(Convert.ToString("Total")))
                                                                                GrandTotal.Add(Convert.ToString("Total"), Convert.ToString(Total));
                                                                            else
                                                                            {
                                                                                double total = 0;
                                                                                double.TryParse(Convert.ToString(GrandTotal[Convert.ToString("Total")]), out total);
                                                                                total += Convert.ToDouble(Total);
                                                                                GrandTotal.Remove(Convert.ToString("Total"));
                                                                                GrandTotal.Add(Convert.ToString("Total"), Convert.ToString(total));
                                                                            }
                                                                            if (Total == 0)
                                                                            {
                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Paid";
                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Left;
                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].BackColor = ColorTranslator.FromHtml("#99ebff");
                                                                            }
                                                                            else
                                                                            {
                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "UnPaid";
                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Left;
                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].BackColor = ColorTranslator.FromHtml("#ff6666");
                                                                            }
                                                                        }
                                                                    }
                                                                    ///toal
                                                                    #region total individual dept

                                                                    FpSpread1.Sheets[0].RowCount++;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString("Total");
                                                                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].BackColor = ColorTranslator.FromHtml("#4870BE");
                                                                    col1 = 6;
                                                                    for (int j = 0; j < chklstaccheader.Items.Count; j++)
                                                                    {
                                                                        if (chklstaccheader.Items[j].Selected == true)
                                                                        {
                                                                            //col1++;
                                                                            string gettotval = Convert.ToString(totval[Convert.ToString(chklstaccheader.Items[j].Text)]);
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ++col1].Text = Convert.ToString(gettotval);
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].HorizontalAlign = HorizontalAlign.Right;
                                                                            if (!gdallot.ContainsKey(Convert.ToString(chklstaccheader.Items[j].Text)))
                                                                                gdallot.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(gettotval));
                                                                            else
                                                                            {
                                                                                double total = 0;
                                                                                double.TryParse(Convert.ToString(gdallot[Convert.ToString(chklstaccheader.Items[j].Text)]), out total);
                                                                                total += Convert.ToDouble(gettotval);
                                                                                gdallot.Remove(Convert.ToString(chklstaccheader.Items[j].Text));
                                                                                gdallot.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(total));
                                                                            }
                                                                            ///
                                                                            double getpaidval = 0;
                                                                            double.TryParse(Convert.ToString(paidval[Convert.ToString(chklstaccheader.Items[j].Text)]), out getpaidval);
                                                                            // string getpaidval = Convert.ToString(paidval[Convert.ToString(chklstaccheader.Items[j].Text)]);
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ++col1].Text = Convert.ToString(getpaidval);
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].HorizontalAlign = HorizontalAlign.Right;
                                                                            if (!gdpaid.ContainsKey(Convert.ToString(chklstaccheader.Items[j].Text)))
                                                                                gdpaid.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(getpaidval));
                                                                            else
                                                                            {
                                                                                double total = 0;
                                                                                double.TryParse(Convert.ToString(gdpaid[Convert.ToString(chklstaccheader.Items[j].Text)]), out total);
                                                                                total += Convert.ToDouble(getpaidval);
                                                                                gdpaid.Remove(Convert.ToString(chklstaccheader.Items[j].Text));
                                                                                gdpaid.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(total));
                                                                            }

                                                                            string GetNewValue = Convert.ToString(GrandTotal[Convert.ToString(chklstaccheader.Items[j].Text)]);
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ++col1].Text = Convert.ToString(GetNewValue);
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].HorizontalAlign = HorizontalAlign.Right;
                                                                            if (!gdbal.ContainsKey(Convert.ToString(chklstaccheader.Items[j].Text)))
                                                                                gdbal.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(GetNewValue));
                                                                            else
                                                                            {
                                                                                double total = 0;
                                                                                double.TryParse(Convert.ToString(gdbal[Convert.ToString(chklstaccheader.Items[j].Text)]), out total);
                                                                                total += Convert.ToDouble(GetNewValue);
                                                                                gdbal.Remove(Convert.ToString(chklstaccheader.Items[j].Text));
                                                                                gdbal.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(total));
                                                                            }
                                                                        }
                                                                    }

                                                                    gettotval1 = Convert.ToString(totval[Convert.ToString("totamt")]);
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 4].Text = Convert.ToString(gettotval1);
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 4].HorizontalAlign = HorizontalAlign.Right;
                                                                    //grand total allot
                                                                    if (!gdallot.ContainsKey(Convert.ToString("totamt")))
                                                                        gdallot.Add(Convert.ToString("totamt"), Convert.ToString(gettotval1));
                                                                    else
                                                                    {
                                                                        double total = 0;
                                                                        double.TryParse(Convert.ToString(gdallot[Convert.ToString("totamt")]), out total);
                                                                        total += Convert.ToDouble(gettotval1);
                                                                        gdallot.Remove(Convert.ToString("totamt"));
                                                                        gdallot.Add(Convert.ToString("totamt"), Convert.ToString(total));
                                                                    }

                                                                    double.TryParse(Convert.ToString(paidval[Convert.ToString("totpaid")]), out getpaidval1);
                                                                    // getpaidval1 = Convert.ToString(paidval[Convert.ToString("totpaid")]);
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 3].Text = Convert.ToString(getpaidval1);
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Right;
                                                                    if (!gdpaid.ContainsKey(Convert.ToString("totpaid")))
                                                                        gdpaid.Add(Convert.ToString("totpaid"), Convert.ToString(getpaidval1));
                                                                    else
                                                                    {
                                                                        double total = 0;
                                                                        double.TryParse(Convert.ToString(gdpaid[Convert.ToString("totpaid")]), out total);
                                                                        total += Convert.ToDouble(getpaidval1);
                                                                        gdpaid.Remove(Convert.ToString("totpaid"));
                                                                        gdpaid.Add(Convert.ToString("totpaid"), Convert.ToString(total));
                                                                    }

                                                                    GetNewValue1 = Convert.ToString(GrandTotal[Convert.ToString("Total")]);
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Text = Convert.ToString(GetNewValue1);
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Right;
                                                                    if (!gdbal.ContainsKey(Convert.ToString("Total")))
                                                                        gdbal.Add(Convert.ToString("Total"), Convert.ToString(GetNewValue1));
                                                                    else
                                                                    {
                                                                        double total = 0;
                                                                        double.TryParse(Convert.ToString(gdbal[Convert.ToString("Total")]), out total);
                                                                        total += Convert.ToDouble(GetNewValue1);
                                                                        gdbal.Remove(Convert.ToString("Total"));
                                                                    }
                                                                    totval.Clear();
                                                                    paidval.Clear();
                                                                    GrandTotal.Clear();
                                                                    #endregion
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            #region  grand total
                                            FpSpread1.Sheets[0].RowCount++;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString("Grand Total");
                                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].BackColor = ColorTranslator.FromHtml("#ffbf00");
                                            col1 = 6;
                                            for (int j = 0; j < chklstaccheader.Items.Count; j++)
                                            {
                                                if (chklstaccheader.Items[j].Selected == true)
                                                {
                                                    //col1++;
                                                    string gettotval = Convert.ToString(gdallot[Convert.ToString(chklstaccheader.Items[j].Text)]);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ++col1].Text = Convert.ToString(gettotval);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].HorizontalAlign = HorizontalAlign.Right;

                                                    string getpaidval = Convert.ToString(gdpaid[Convert.ToString(chklstaccheader.Items[j].Text)]);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ++col1].Text = Convert.ToString(getpaidval);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].HorizontalAlign = HorizontalAlign.Right;

                                                    string GetNewValue = Convert.ToString(gdbal[Convert.ToString(chklstaccheader.Items[j].Text)]);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ++col1].Text = Convert.ToString(GetNewValue);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].HorizontalAlign = HorizontalAlign.Right;
                                                }
                                            }

                                            gettotval1 = Convert.ToString(gdallot[Convert.ToString("totamt")]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 4].Text = Convert.ToString(gettotval1);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 4].HorizontalAlign = HorizontalAlign.Right;

                                            double.TryParse(Convert.ToString(gdpaid[Convert.ToString("totpaid")]), out getpaidval1);
                                            //getpaidval1 = Convert.ToString(gdpaid[Convert.ToString("totpaid")]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 3].Text = Convert.ToString(getpaidval1);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Right;


                                            GetNewValue1 = Convert.ToString(gdbal[Convert.ToString("Total")]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Text = Convert.ToString(GetNewValue1);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Right;

                                            #endregion
                                        }

                                        if (FpSpread1.Sheets[0].RowCount > 0)
                                        {
                                            FpSpread1.Visible = true;
                                            btnprintmaster.Visible = true;
                                            lblrptname.Visible = true;
                                            txtexcelname.Visible = true;
                                            btnxl.Visible = true;
                                        }
                                        else
                                        {
                                            FpSpread1.Visible = false;
                                            btnprintmaster.Visible = false;
                                            lblrptname.Visible = false;
                                            txtexcelname.Visible = false;
                                            btnxl.Visible = false;
                                        }
                                        #endregion
                                    }
                                    #endregion
                                }
                                else if (ddlfeetype.SelectedItem.Text == "Paid")
                                {
                                    if (chkdate.Checked == false)
                                    {
                                        #region
                                        // Jairam New Updation
                                        string typevalue = "";
                                        if (ddltype.Enabled == true)
                                        {
                                            typevalue = Convert.ToString(ddltype.SelectedItem.Text);
                                        }

                                        #region query
                                        string Query = "";
                                        if (ddlstudtype.SelectedItem.Text.Trim() != "Before Admission")
                                        {
                                            #region general query

                                            Query = "Select SUM(Debit) as Paid, f.App_No,f.FeeCategory,r.degree_code,r.Batch_Year,r.Stud_Name,r.Roll_No,r.roll_admit,r.Reg_No,g.ChlGroupHeader,f.TransCode,DDNo,f.PayMode,r.Current_Semester from FT_FinDailyTransaction f,Registration r ,FS_ChlGroupHeaderSettings G where f.App_No =r.App_No  and G.HeaderFK =F.HeaderFK   and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'   and r.college_code ='" + collegecode + "' ";
                                            if (usBasedRights == true)
                                                Query += " and f.EntryUserCode in('" + usercode + "')";
                                            if (commondist != "")
                                                Query = Query + commondist;
                                            if (typevalue.Trim() != "Both" && typevalue.Trim() != "")
                                                Query = Query + " and Stream ='" + typevalue + "'";

                                            if (batch_all.Trim() != "")
                                                Query = Query + " and r.batch_year  in (" + batch_all + ")";

                                            if (course_all.Trim() != "")
                                                Query = Query + " and r.degree_code  in (" + course_all + ")";

                                            if (stefeesem.Trim() != "")
                                                Query = Query + " and f.FeeCategory  in (" + stefeesem + ")";

                                            if (finyearval.Trim() != "")
                                                Query = Query + " and f.FinYearFK   in (" + Newfinayear + ")";

                                            if (header_all.Trim() != "")
                                                Query = Query + " and ChlGroupHeader   in ('" + header_all + "')";

                                            if (bankcode != "")
                                                Query += " and f.DDBankCode in ('" + bankcode + "')";
                                            if (modestatus.Trim() != "")
                                                Query += " and f.paymode in ('" + modestatus + "')";

                                            Query = Query + " group by f.App_No,f.FeeCategory,r.degree_code,r.Batch_Year,r.Stud_Name,r.Roll_No,r.roll_admit,r.Reg_No,g.ChlGroupHeader,f.TransCode ,f.PayMode,DDNo,r.Current_Semester ";

                                            if (cbbfadm.Checked == true)
                                            {
                                                #region befor admission

                                                Query += " union  select distinct SUM(Debit) as Paid, d.App_No,d.FeeCategory,r.degree_code,r.Batch_Year,r.Stud_Name,r.app_formno as Roll_No,r.app_formno as  Reg_No,r.app_formno as roll_admit,f.ChlGroupHeader,d.TransCode,DDNo,d.PayMode,r.Current_Semester from ft_findailytransaction d,applyn r,fs_chlgroupheadersettings f where d.app_no = r.app_no  and f.headerfk=d.headerfk  and credit=0 and transtype=1 and d.debit>0  and r.isconfirm = 1 and r.admission_status = '1' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(r.is_enroll,'0')<>'2'  and r.college_code ='" + collegecode + "' ";
                                                // and r.app_no not in (select app_no from Registration  where degree_code in(" + course_all + ") and f.stream ='" + typevalue + "'  and Batch_year in(" + batch_all + "))
                                                if (usBasedRights == true)
                                                    Query += " and d.EntryUserCode in('" + usercode + "')";

                                                if (typevalue.Trim() != "Both" && typevalue.Trim() != "")
                                                    Query = Query + " and f.Stream ='" + typevalue + "'";

                                                if (batch_all.Trim() != "")
                                                    Query = Query + " and r.batch_year  in (" + batch_all + ")";

                                                if (course_all.Trim() != "")
                                                    Query = Query + " and r.degree_code  in (" + course_all + ")";

                                                if (stefeesem.Trim() != "")
                                                    Query = Query + " and d.FeeCategory  in (" + stefeesem + ")";

                                                if (finyearval.Trim() != "")
                                                    Query = Query + " and d.FinYearFK   in (" + Newfinayear + ")";

                                                if (header_all.Trim() != "")
                                                    Query = Query + " and f.ChlGroupHeader   in ('" + header_all + "')";

                                                if (bankcode != "")
                                                    Query += " and d.DDBankCode in ('" + bankcode + "')";
                                                if (modestatus.Trim() != "")
                                                    Query += " and d.paymode in ('" + modestatus + "')";

                                                Query += "  group by d.App_No,d.FeeCategory,r.degree_code,r.Batch_Year,r.Stud_Name,r.app_formno,f.ChlGroupHeader,d.TransCode,DDNo,d.PayMode,r.Current_Semester";
                                                // Query += " order by R.app_formno";
                                                #endregion
                                            }
                                            Query += " order by Roll_No";

                                            Query = Query + " select distinct f.App_No ,r.degree_code,r.Batch_Year,r.current_semester  from FT_FeeAllot f,Registration r ,Degree d,Department dt,Course c ,FS_ChlGroupHeaderSettings G where G.HeaderFK =f.HeaderFK and f.App_No =r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code ='" + collegecode + "'";
                                            if (commondist != "")
                                                Query = Query + commondist;

                                            if (typevalue.Trim() != "Both" && typevalue.Trim() != "")
                                                Query = Query + " and type ='" + typevalue + "'";

                                            if (batch_all.Trim() != "")
                                                Query = Query + " and r.batch_year  in (" + batch_all + ")";

                                            if (course_all.Trim() != "")
                                                Query = Query + " and r.degree_code  in (" + course_all + ")";

                                            if (stefeesem.Trim() != "")
                                                Query = Query + " and f.FeeCategory  in (" + stefeesem + ")";

                                            if (header_all.Trim() != "")
                                                Query = Query + " and ChlGroupHeader   in ('" + header_all + "')";

                                            if (finyearval.Trim() != "")
                                                Query = Query + " and f.FinYearFK   in (" + Newfinayear + ")";

                                            if (chkfeeroll.Checked == true)
                                                Query = Query + " and r.Roll_No not in(select s.roll_no from stucon s where s.roll_no=r.roll_no and s.semester=r.Current_Semester and s.ack_fee_of_roll=1)";
                                            if (cbbfadm.Checked == true)
                                            {
                                                #region before admission

                                                Query = Query + " union select distinct f.App_No ,r.degree_code,r.Batch_Year,r.current_semester  from FT_FeeAllot f,applyn r ,Degree d,Department dt,Course c ,FS_ChlGroupHeaderSettings G where G.HeaderFK =f.HeaderFK and f.App_No =r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code ='" + collegecode + "'  and r.isconfirm = 1 and r.admission_status = '1' and isnull(r.is_enroll,'0')<>'2'";
                                                // and r.app_no not in (select app_no from Registration  where degree_code in(" + course_all + ") and g.stream ='DAY'  and Batch_year in(" + batch_all + "))

                                                if (typevalue.Trim() != "Both" && typevalue.Trim() != "")
                                                    Query = Query + " and g.Stream ='" + typevalue + "'";

                                                if (batch_all.Trim() != "")
                                                    Query = Query + " and r.batch_year  in (" + batch_all + ")";

                                                if (course_all.Trim() != "")
                                                    Query = Query + " and r.degree_code  in (" + course_all + ")";

                                                if (stefeesem.Trim() != "")
                                                    Query = Query + " and f.FeeCategory  in (" + stefeesem + ")";

                                                if (header_all.Trim() != "")
                                                    Query = Query + " and ChlGroupHeader   in ('" + header_all + "')";

                                                if (finyearval.Trim() != "")
                                                    Query = Query + " and f.FinYearFK   in (" + Newfinayear + ")";
                                                #endregion
                                            }

                                            Query += "   select distinct f.App_No,r.Roll_No,r.roll_admit,r.Reg_No,r.Stud_Name,r.degree_code,f.FeeCategory,r.current_semester from FT_FinDailyTransaction f,Registration r,FS_ChlGroupHeaderSettings G  where f.App_No =r.App_No and g.HeaderFK =f.HeaderFK  and r.college_code ='" + collegecode + "'";
                                            if (usBasedRights == true)
                                                Query += " and f.EntryUserCode in('" + usercode + "')";
                                            if (commondist != "")
                                                Query = Query + commondist;
                                            if (typevalue.Trim() != "Both" && typevalue.Trim() != "")
                                                Query = Query + " and g.Stream ='" + typevalue + "'";

                                            if (batch_all.Trim() != "")
                                                Query = Query + " and r.batch_year  in (" + batch_all + ")";

                                            if (course_all.Trim() != "")
                                                Query = Query + " and r.degree_code  in (" + course_all + ")";

                                            if (stefeesem.Trim() != "")
                                                Query = Query + " and f.FeeCategory  in (" + stefeesem + ")";

                                            if (finyearval.Trim() != "")
                                                Query = Query + " and f.FinYearFK   in (" + Newfinayear + ")";

                                            if (header_all.Trim() != "")
                                                Query = Query + " and ChlGroupHeader   in ('" + header_all + "')";

                                            if (bankcode != "")
                                                Query += " and f.DDBankCode in ('" + bankcode + "')";
                                            if (modestatus.Trim() != "")
                                                Query += " and f.paymode in ('" + modestatus + "')";

                                            if (cbbfadm.Checked == true)
                                            {
                                                #region before admission

                                                Query += " union  select distinct d.App_No,r.app_formno as Roll_No,r.app_formno as Reg_No,r.app_fromno as roll_admit,r.Stud_Name,r.degree_code,d.FeeCategory,r.current_semester from applyn r,ft_findailytransaction d,fs_chlgroupheadersettings f where  f.headerfk=d.Headerfk and isconfirm = 1 and admission_status = '1' and d.app_no = r.app_no and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'and r.college_code ='" + collegecode + "' and isnull(r.is_enroll,'0')<>'2' and r.college_code ='" + collegecode + "'";
                                                // and r.app_no not in (select app_no from Registration  where degree_code in(" + course_all + ")   and Batch_year in(" + batch_all + ") )
                                                if (usBasedRights == true)
                                                    Query += " and d.EntryUserCode in('" + usercode + "')";

                                                if (typevalue.Trim() != "Both" && typevalue.Trim() != "")
                                                    Query = Query + " and f.Stream ='" + typevalue + "'";

                                                if (batch_all.Trim() != "")
                                                    Query = Query + " and r.batch_year  in (" + batch_all + ")";

                                                if (course_all.Trim() != "")
                                                    Query = Query + " and r.degree_code  in (" + course_all + ")";

                                                if (stefeesem.Trim() != "")
                                                    Query = Query + " and d.FeeCategory  in (" + stefeesem + ")";

                                                if (finyearval.Trim() != "")
                                                    Query = Query + " and d.FinYearFK   in (" + Newfinayear + ")";

                                                if (header_all.Trim() != "")
                                                    Query = Query + " and ChlGroupHeader   in ('" + header_all + "')";

                                                if (bankcode != "")
                                                    Query += " and d.DDBankCode in ('" + bankcode + "')";
                                                if (modestatus.Trim() != "")
                                                    Query += " and d.paymode in ('" + modestatus + "')";

                                                //Query += " order by r.app_formno";
                                                #endregion
                                            }
                                            Query += " order by r.Roll_No";
                                            #endregion
                                        }
                                        else
                                        {
                                            #region arrear list
                                            //paid query
                                            Query = "   select distinct SUM(Debit) as Paid, d.App_No,d.FeeCategory,r.degree_code,r.Batch_Year,r.Stud_Name,r.app_formno as Roll_No,r.app_formno as  Reg_No,r.app_formno as  roll_admit,f.ChlGroupHeader,d.TransCode,DDNo,d.PayMode,r.Current_Semester from ft_findailytransaction d,applyn r,fs_chlgroupheadersettings f where d.app_no = r.app_no  and f.headerfk=d.headerfk  and credit=0 and transtype=1 and d.debit>0  and r.isconfirm = 1 and r.admission_status = '1' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'  and r.app_no not in (select app_no from Registration  where degree_code in(" + course_all + ") and f.stream ='" + typevalue + "'  and Batch_year in(" + batch_all + ")) and r.college_code ='" + collegecode + "' ";
                                            if (usBasedRights == true)
                                                Query += " and d.EntryUserCode in('" + usercode + "')";

                                            if (typevalue.Trim() != "Both" && typevalue.Trim() != "")
                                                Query = Query + " and f.Stream ='" + typevalue + "'";

                                            if (batch_all.Trim() != "")
                                                Query = Query + " and r.batch_year  in (" + batch_all + ")";

                                            if (course_all.Trim() != "")
                                                Query = Query + " and r.degree_code  in (" + course_all + ")";

                                            if (stefeesem.Trim() != "")
                                                Query = Query + " and d.FeeCategory  in (" + stefeesem + ")";

                                            if (finyearval.Trim() != "")
                                                Query = Query + " and d.FinYearFK   in (" + Newfinayear + ")";

                                            if (header_all.Trim() != "")
                                                Query = Query + " and f.ChlGroupHeader   in ('" + header_all + "')";

                                            if (bankcode != "")
                                                Query += " and d.DDBankCode in ('" + bankcode + "')";
                                            if (modestatus.Trim() != "")
                                                Query += " and d.paymode in ('" + modestatus + "')";

                                            Query += "  group by d.App_No,d.FeeCategory,r.degree_code,r.Batch_Year,r.Stud_Name,r.app_formno,f.ChlGroupHeader,d.TransCode,DDNo,d.PayMode,r.Current_Semester";
                                            Query += " order by R.app_formno";

                                            //Student list
                                            Query = Query + " select distinct f.App_No ,r.degree_code,r.Batch_Year,r.current_semester  from FT_FeeAllot f,applyn r ,Degree d,Department dt,Course c ,FS_ChlGroupHeaderSettings G where G.HeaderFK =f.HeaderFK and f.App_No =r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code ='" + collegecode + "'  and r.isconfirm = 1 and r.admission_status = '1'  and r.app_no not in (select app_no from Registration  where degree_code in(" + course_all + ") and g.stream ='DAY'  and Batch_year in(" + batch_all + "))";


                                            if (typevalue.Trim() != "Both" && typevalue.Trim() != "")
                                                Query = Query + " and g.Stream ='" + typevalue + "'";

                                            if (batch_all.Trim() != "")
                                                Query = Query + " and r.batch_year  in (" + batch_all + ")";

                                            if (course_all.Trim() != "")
                                                Query = Query + " and r.degree_code  in (" + course_all + ")";

                                            if (stefeesem.Trim() != "")
                                                Query = Query + " and f.FeeCategory  in (" + stefeesem + ")";

                                            if (header_all.Trim() != "")
                                                Query = Query + " and ChlGroupHeader   in ('" + header_all + "')";

                                            if (finyearval.Trim() != "")
                                                Query = Query + " and f.FinYearFK   in (" + Newfinayear + ")";

                                            // if (chkfeeroll.Checked == true)
                                            // Query = Query + " and r.Roll_No not in(select s.roll_no from stucon s where s.roll_no=r.roll_no and s.semester=r.Current_Semester and s.ack_fee_of_roll=1)";

                                            Query += "   select distinct d.App_No,r.app_formno as Roll_No,r.app_formno as Reg_No,r.app_formno as roll_admit,r.Stud_Name,r.degree_code,d.FeeCategory,r.current_semester from applyn r,ft_findailytransaction d,fs_chlgroupheadersettings f where  f.headerfk=d.Headerfk and isconfirm = 1 and admission_status = '1' and d.app_no = r.app_no and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'and r.college_code ='13' and r.app_no not in (select app_no from Registration  where degree_code in(" + course_all + ")   and Batch_year in(" + batch_all + ") ) and r.college_code ='" + collegecode + "'";
                                            if (usBasedRights == true)
                                                Query += " and d.EntryUserCode in('" + usercode + "')";

                                            if (typevalue.Trim() != "Both" && typevalue.Trim() != "")
                                                Query = Query + " and f.Stream ='" + typevalue + "'";

                                            if (batch_all.Trim() != "")
                                                Query = Query + " and r.batch_year  in (" + batch_all + ")";

                                            if (course_all.Trim() != "")
                                                Query = Query + " and r.degree_code  in (" + course_all + ")";

                                            if (stefeesem.Trim() != "")
                                                Query = Query + " and d.FeeCategory  in (" + stefeesem + ")";

                                            if (finyearval.Trim() != "")
                                                Query = Query + " and d.FinYearFK   in (" + Newfinayear + ")";

                                            if (header_all.Trim() != "")
                                                Query = Query + " and ChlGroupHeader   in ('" + header_all + "')";

                                            if (bankcode != "")
                                                Query += " and d.DDBankCode in ('" + bankcode + "')";
                                            if (modestatus.Trim() != "")
                                                Query += " and d.paymode in ('" + modestatus + "')";

                                            Query += " order by r.app_formno";

                                            #endregion
                                        }

                                        Query = Query + " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode + "'";
                                        Query = Query + " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code ='" + collegecode + "'";
                                        #endregion

                                        ds.Clear();
                                        ds = da.select_method_wo_parameter(Query, "Text");
                                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                        {
                                            #region value

                                            DataView dv = new DataView();
                                            DataView dv1 = new DataView();
                                            DataView dv2 = new DataView();
                                            finalflag = true;
                                            double Cashtotal = 0;
                                            double checktotal = 0;
                                            double ddtotal = 0;
                                            double Challantotal = 0;
                                            double onlinetotal = 0;
                                            int colcnt = 0;
                                            bool colval = false;
                                            ArrayList arroll = new ArrayList();
                                            Dictionary<string, Double> diccurarrfee = new Dictionary<string, double>();
                                            Hashtable GrandTotal = new Hashtable();
                                            if (chklstbranch.Items.Count > 0)
                                            {
                                                for (int batch = 0; batch < chklstbatch.Items.Count; batch++)
                                                {
                                                    if (chklstbatch.Items[batch].Selected == true)
                                                    {
                                                        for (int Bnch = 0; Bnch < chklstbranch.Items.Count; Bnch++)
                                                        {
                                                            if (chklstbranch.Items[Bnch].Selected == true)
                                                            {
                                                                ds.Tables[1].DefaultView.RowFilter = "degree_code='" + chklstbranch.Items[Bnch].Value + "' and Batch_Year='" + Convert.ToString(chklstbatch.Items[batch].Text) + "'";
                                                                dv2 = ds.Tables[1].DefaultView;
                                                                if (dv2.Count > 0)
                                                                {
                                                                    FpSpread1.Sheets[0].RowCount++;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(chklstbranch.Items[Bnch].Text) + " - " + Convert.ToString(chklstbatch.Items[batch].Text);
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.LightGreen;
                                                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, FpSpread1.Sheets[0].ColumnCount - 1);

                                                                    for (int row = 0; row < dv2.Count; row++)
                                                                    {
                                                                        string app_no = Convert.ToString(dv2[row]["App_No"]);
                                                                        ds.Tables[2].DefaultView.RowFilter = "App_No=" + Convert.ToString(app_no) + " and degree_code='" + chklstbranch.Items[Bnch].Value + "'";
                                                                        dv = ds.Tables[2].DefaultView;
                                                                        for (int drow = 0; drow < dv.Count; drow++)
                                                                        {
                                                                            sno++;
                                                                            string dvapp_no = Convert.ToString(dv[drow]["App_No"]);
                                                                            string Feecategory = Convert.ToString(dv[drow]["FeeCategory"]);
                                                                            FpSpread1.Sheets[0].RowCount++;

                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dv[drow]["Stud_Name"]);
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dv[drow]["Roll_No"]);
                                                                            if (sclflag == false)
                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv[drow]["Reg_No"]);
                                                                            else
                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv[drow]["roll_admit"]);
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = txtreg;
                                                                            //if (!arroll.Contains(Convert.ToString(dv[drow]["Reg_No"])))
                                                                            //{
                                                                            //    arroll.Add(Convert.ToString(dv[drow]["Reg_No"]));
                                                                            //    sno++;
                                                                            //}
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                                                            DataView Dview = new DataView();
                                                                            string Degreename = "";
                                                                            if (ds.Tables[3].Rows.Count > 0)
                                                                            {
                                                                                ds.Tables[3].DefaultView.RowFilter = "Degree_code='" + Convert.ToString(dv[drow]["Degree_code"]) + "'";
                                                                                Dview = ds.Tables[3].DefaultView;
                                                                                if (Dview.Count > 0)
                                                                                {
                                                                                    Degreename = Convert.ToString(Dview[0]["degreename"]);
                                                                                }
                                                                            }
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Degreename;
                                                                            string TextName = "";
                                                                            if (ds.Tables[4].Rows.Count > 0)
                                                                            {
                                                                                ds.Tables[4].DefaultView.RowFilter = "TextCode='" + Feecategory + "'";
                                                                                Dview = ds.Tables[4].DefaultView;
                                                                                if (Dview.Count > 0)
                                                                                {
                                                                                    TextName = Convert.ToString(Dview[0]["TextVal"]);
                                                                                }
                                                                            }
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = TextName;

                                                                            string curSem = Convert.ToString(dv[drow]["current_semester"]);
                                                                            string feechec = TextName.Split(' ')[0];
                                                                            if (Convert.ToInt32(curSem) > Convert.ToInt32(feechec))
                                                                            {
                                                                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.YellowGreen;
                                                                            }

                                                                            int col = 6;
                                                                            double Total = 0;
                                                                            double balance = 0;
                                                                            bool valu = false;
                                                                            for (int j = 0; j < chklstaccheader.Items.Count; j++)
                                                                            {
                                                                                if (chklstaccheader.Items[j].Selected == true)
                                                                                {
                                                                                    ds.Tables[0].DefaultView.RowFilter = "App_no='" + dvapp_no + "' and FeeCategory='" + Feecategory + "' and ChlGroupHeader='" + Convert.ToString(chklstaccheader.Items[j].Text) + "'";
                                                                                    dv1 = ds.Tables[0].DefaultView;
                                                                                    col++;
                                                                                    if (dv1.Count > 0)
                                                                                    {
                                                                                        for (int k = 0; k < dv1.Count; k++)
                                                                                        {
                                                                                            if (colval == false)
                                                                                                colcnt = col;
                                                                                            colval = true;
                                                                                            #region receipt and challan no

                                                                                            string receiptno = "";
                                                                                            receiptno = Convert.ToString(dv1[k]["Transcode"]);
                                                                                            string challanno = Convert.ToString(dv1[k]["DDno"]);
                                                                                            string chaandrpt = "";
                                                                                            if (rightscode == 3 || rightscode == 1 || rightscode == 2)
                                                                                            {
                                                                                                if (receiptno != "" && challanno == "")
                                                                                                {
                                                                                                    chaandrpt = receiptno;
                                                                                                }
                                                                                                if (receiptno == "" && challanno != "")
                                                                                                {
                                                                                                    chaandrpt = receiptno;
                                                                                                }
                                                                                                if (receiptno != "" && challanno != "")
                                                                                                {
                                                                                                    chaandrpt = challanno + "/" + receiptno;
                                                                                                }
                                                                                                if (receiptno == "" && challanno == "")
                                                                                                {
                                                                                                    chaandrpt = challanno;
                                                                                                }

                                                                                            }
                                                                                            if (rightscode == 1)
                                                                                            {
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = receiptno;
                                                                                            }
                                                                                            if (rightscode == 2 || rightscode == 0)
                                                                                            {
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = challanno;
                                                                                            }
                                                                                            if (rightscode == 3)
                                                                                            {
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = chaandrpt;
                                                                                            }
                                                                                            #endregion

                                                                                            double.TryParse(Convert.ToString(dv1[k]["Paid"]), out balance);
                                                                                            Total += balance;
                                                                                            if (balance != 0)
                                                                                                valu = true;

                                                                                            // FpSpread1.Sheets[0].RowCount++;
                                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(balance);
                                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                                                                            if (!GrandTotal.ContainsKey(Convert.ToString(chklstaccheader.Items[j].Text)))
                                                                                                GrandTotal.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(balance));
                                                                                            else
                                                                                            {
                                                                                                double total = 0;
                                                                                                double.TryParse(Convert.ToString(GrandTotal[Convert.ToString(chklstaccheader.Items[j].Text)]), out total);
                                                                                                total += balance;
                                                                                                GrandTotal.Remove(Convert.ToString(chklstaccheader.Items[j].Text));
                                                                                                GrandTotal.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(total));
                                                                                            }

                                                                                            #region Paymode value set
                                                                                            string cursem = Convert.ToString(dv1[k]["Current_Semester"]);
                                                                                            string paymode = Convert.ToString(dv1[k]["paymode"]);
                                                                                            if (balance != 0)
                                                                                            {
                                                                                                string Linkvalue = "";
                                                                                                if (group_user.Contains(';'))
                                                                                                {
                                                                                                    string[] group_semi = group_user.Split(';');
                                                                                                    group_user = group_semi[0].ToString();

                                                                                                    Linkvalue = da.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + group_user + "' and college_code ='" + collegecode + "'");
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    Linkvalue = da.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'");
                                                                                                }

                                                                                                if (Linkvalue == "0")
                                                                                                {
                                                                                                    if (diccurarrfee.ContainsKey(Feecategory + "$" + cursem))
                                                                                                    {
                                                                                                        Double getval = balance + diccurarrfee[Feecategory + "$" + cursem];
                                                                                                        diccurarrfee[Feecategory + "$" + cursem] = getval;
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        diccurarrfee.Add(Feecategory + "$" + cursem, balance);
                                                                                                    }
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    string sem = "";
                                                                                                    double amt = 0;
                                                                                                    string valuenew = returnYearforSem(cursem);
                                                                                                    if (diccurarrfee.ContainsKey(Feecategory + "$" + valuenew.ToString()))
                                                                                                    {
                                                                                                        Double getval = Convert.ToDouble(balance) + diccurarrfee[Feecategory + "$" + valuenew.ToString()];
                                                                                                        diccurarrfee[Feecategory + "$" + valuenew.ToString()] = getval;
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        diccurarrfee.Add(Feecategory + "$" + valuenew.ToString(), Convert.ToDouble(balance));
                                                                                                    }

                                                                                                }
                                                                                                finalflag = true;
                                                                                                // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].CellType = dobcell;
                                                                                                // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Text = balance;
                                                                                                // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].HorizontalAlign = HorizontalAlign.Right;
                                                                                                if (paymode == "1")
                                                                                                {
                                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].BackColor = Color.LightCoral;
                                                                                                    Cashtotal = Cashtotal + balance;
                                                                                                }
                                                                                                else if (paymode == "2")
                                                                                                {
                                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].BackColor = Color.LightGray;
                                                                                                    checktotal = checktotal + balance;
                                                                                                }
                                                                                                else if (paymode == "3")
                                                                                                {
                                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].BackColor = Color.Orange;
                                                                                                    // Challantotal = Challantotal + Convert.ToDouble(paid_amt);
                                                                                                    ddtotal = ddtotal + balance;

                                                                                                }
                                                                                                else if (paymode == "4")
                                                                                                {
                                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].BackColor = Color.LightGreen;
                                                                                                    Challantotal = Challantotal + balance;
                                                                                                }
                                                                                                else if (paymode == "5")
                                                                                                {
                                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].BackColor = Color.LightGoldenrodYellow;
                                                                                                    onlinetotal = onlinetotal + balance;
                                                                                                }
                                                                                            }

                                                                                            #endregion
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = "-";
                                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                                                                    }
                                                                                }
                                                                            }

                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(Total);
                                                                            //Total += Convert.ToDouble(dv1[0]["Balance"]);
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                                                            if (!GrandTotal.ContainsKey(Convert.ToString("Total")))
                                                                                GrandTotal.Add(Convert.ToString("Total"), Convert.ToString(Total));
                                                                            else
                                                                            {
                                                                                double total = 0;
                                                                                double.TryParse(Convert.ToString(GrandTotal[Convert.ToString("Total")]), out total);
                                                                                total += Total;
                                                                                GrandTotal.Remove(Convert.ToString("Total"));
                                                                                GrandTotal.Add(Convert.ToString("Total"), Convert.ToString(total));
                                                                            }

                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                //spaning
                                                //  FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                                //FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                                //FpSpread1.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                                //FpSpread1.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);

                                                FpSpread1.Sheets[0].RowCount++;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString("Grand Total");
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.LightSeaGreen;
                                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                                                int col1 = 6;
                                                for (int j = 0; j < chklstaccheader.Items.Count; j++)
                                                {
                                                    if (chklstaccheader.Items[j].Selected == true)
                                                    {
                                                        col1++;
                                                        string GetNewValue = Convert.ToString(GrandTotal[Convert.ToString(chklstaccheader.Items[j].Text)]);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].Text = Convert.ToString(GetNewValue);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].HorizontalAlign = HorizontalAlign.Right;
                                                    }
                                                }
                                                col1++;
                                                string GetNewValue1 = Convert.ToString(GrandTotal[Convert.ToString("Total")]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].Text = Convert.ToString(GetNewValue1);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].HorizontalAlign = HorizontalAlign.Right;

                                                #region abstract

                                                //abstract
                                                FpSpread1.Sheets[0].RowCount++;// = FpSpread1.Sheets[0].RowCount + 2;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "ABSTRACT";
                                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.Brown;
                                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].ForeColor = Color.White;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 7);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                for (int ps = 0; ps < chklstpaymode.Items.Count; ps++)
                                                {
                                                    if (chklstpaymode.Items[ps].Selected == true)
                                                    {
                                                        string strptype = chklstpaymode.Items[ps].Text.ToString();
                                                        FpSpread1.Sheets[0].RowCount++;
                                                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 7);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = strptype;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                                                        if (strptype == "Cash")
                                                        {
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].Text = Cashtotal.ToString();
                                                        }
                                                        else if (strptype == "Cheque")
                                                        {
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].Text = checktotal.ToString();
                                                        }
                                                        else if (strptype == "DD")
                                                        {
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].Text = ddtotal.ToString();
                                                        }
                                                        else if (strptype == "Challan")
                                                        {
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].Text = Challantotal.ToString();
                                                        }
                                                        else if (strptype == "Online Pay")
                                                        {
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].Text = onlinetotal.ToString();
                                                        }
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].HorizontalAlign = HorizontalAlign.Right;
                                                    }
                                                }
                                                FpSpread1.Sheets[0].RowCount++;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
                                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 7);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                                                Double dotvd = Cashtotal + checktotal + ddtotal + Challantotal + onlinetotal;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].Text = dotvd.ToString();
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].HorizontalAlign = HorizontalAlign.Right;
                                                #endregion

                                                #region collected and arrear

                                                Double currfee = 0;
                                                Double arrfee = 0;
                                                foreach (var parameter in diccurarrfee)
                                                {
                                                    string getsplallow = parameter.Key.ToString();
                                                    string getfeeamount = parameter.Value.ToString();
                                                    string[] spt = getsplallow.Split('$');
                                                    if (spt.GetUpperBound(0) == 1)
                                                    {
                                                        for (int rcs = 0; rcs < chklsfeesem.Items.Count; rcs++)
                                                        {
                                                            if (chklsfeesem.Items[rcs].Value.ToString() == spt[0].ToString())
                                                            {
                                                                string feeval = chklsfeesem.Items[rcs].Text.ToString();
                                                                string[] stc = feeval.Split(' ');
                                                                if (stc.GetUpperBound(0) >= 0)
                                                                {
                                                                    if (stc[0].ToString().Trim() == spt[1].ToString().Trim())
                                                                    {
                                                                        currfee = currfee + Convert.ToDouble(getfeeamount);
                                                                    }
                                                                    else
                                                                    {
                                                                        arrfee = arrfee + Convert.ToDouble(getfeeamount);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (chklsfeesem.Items[rcs].Text.Contains(spt[1].ToString()))
                                                                    {
                                                                        currfee = currfee + Convert.ToDouble(getfeeamount);
                                                                    }
                                                                    else
                                                                    {
                                                                        arrfee = arrfee + Convert.ToDouble(getfeeamount);
                                                                    }
                                                                }
                                                                rcs = chklsfeesem.Items.Count;
                                                            }
                                                        }
                                                    }
                                                }
                                                FpSpread1.Sheets[0].RowCount++;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "REGULAR";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 7);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].Text = currfee.ToString();
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].HorizontalAlign = HorizontalAlign.Right;
                                                FpSpread1.Sheets[0].RowCount++;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "ARREAR";
                                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.YellowGreen;
                                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 7);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].Text = arrfee.ToString();
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].HorizontalAlign = HorizontalAlign.Right;
                                                #endregion
                                            }
                                            if (FpSpread1.Sheets[0].RowCount > 0)
                                            {
                                                FpSpread1.Visible = true;
                                                btnprintmaster.Visible = true;
                                                lblrptname.Visible = true;
                                                txtexcelname.Visible = true;
                                                btnxl.Visible = true;
                                                lblc1.Visible = true;
                                                lblc2.Visible = true;
                                                lblc3.Visible = true;
                                                lblc4.Visible = true;
                                                lblc5.Visible = true;
                                                lblcard.Visible = true;
                                            }
                                            else
                                            {
                                                FpSpread1.Visible = false;
                                                btnprintmaster.Visible = false;
                                                lblrptname.Visible = false;
                                                txtexcelname.Visible = false;
                                                btnxl.Visible = false;
                                                lblc1.Visible = false;
                                                lblc2.Visible = false;
                                                lblc3.Visible = false;
                                                lblc4.Visible = false;
                                                lblc5.Visible = false;
                                                lblcard.Visible = false;
                                            }
                                            #endregion
                                        }
                                        #endregion
                                    }
                                }
                            }
                            #endregion

                            //date wise paid group header only
                            if (ddlfeetype.SelectedItem.Text == "Paid" && chkdate.Checked == true)
                            {
                                #region with date paid for group header
                                // Jairam New Updation
                                string typevalue = "";
                                string headerid = "";
                                string header = "";
                                string commonid = "";
                                if (ddlacctype.SelectedItem.Text == "Group Header")
                                {
                                    headerid = ",fs.ChlGroupHeader";
                                    header = " and fs.ChlGroupHeader in('" + header_all + "')";
                                    commonid = "fs.ChlGroupHeader";
                                }
                                if (ddltype.Enabled == true)
                                {
                                    typevalue = Convert.ToString(ddltype.SelectedItem.Text);
                                }
                                string Query = "";
                                if (ddlstudtype.SelectedItem.Text.Trim() != "Before Admission")
                                {
                                    #region query
                                    //paid details

                                    Query = "select distinct r.stud_name as Name,d.app_no,r.Current_Semester, r.Roll_No,r.roll_admit,r.Reg_No,sum(d.debit) as paid,r.degree_code ,d.paymode as mode ,g.ChlGroupHeader ,feecategory,d.TransCode as ReceiptNo,d.ddno as challan_no ,d.TransDate cal_date,d.feecategory from ft_findailytransaction d,registration r,FS_ChlGroupHeaderSettings g where  d.app_no = r.app_no and d.HeaderFK=g.HeaderFK and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'   and r.college_code ='" + collegecode + "' and d.debit>0 ";
                                    if (usBasedRights == true)
                                        Query += " and d.EntryUserCode in('" + usercode + "')";
                                    if (commondist != "")
                                        Query = Query + commondist;
                                    if (typevalue.Trim() != "Both" && typevalue.Trim() != "")
                                    {
                                        Query = Query + " and Stream ='" + typevalue + "'";
                                    }
                                    if (batch_all.Trim() != "")
                                    {
                                        Query = Query + " and r.batch_year  in (" + batch_all + ")";
                                    }
                                    if (course_all.Trim() != "")
                                    {
                                        Query = Query + " and r.degree_code  in (" + course_all + ")";
                                    }
                                    if (stefeesem.Trim() != "")
                                    {
                                        Query = Query + " and d.FeeCategory  in (" + stefeesem + ")";
                                    }
                                    if (finyearval.Trim() != "")
                                    {
                                        Query = Query + " and d.FinYearFK   in (" + Newfinayear + ")";
                                    }
                                    if (header_all.Trim() != "")
                                    {
                                        Query = Query + " and ChlGroupHeader   in ('" + header_all + "')";
                                    }
                                    if (bankcode != "")
                                        Query += " and d.DDBankCode in ('" + bankcode + "')";
                                    if (modestatus.Trim() != "")
                                    {
                                        Query += " and d.paymode in ('" + modestatus + "')";
                                    }
                                    Query += " and d.TransDate between '" + dat.ToString("MM/dd/yyyy") + "' and '" + dat1.ToString("MM/dd/yyyy") + "'";
                                    Query += " group by r.roll_no,d.app_no,r.Reg_No,r.roll_admit,r.stud_name,r.degree_code,r.Current_Semester,d.paymode  ,g.ChlGroupHeader,feecategory,d.TransCode,d.ddno,d.TransDate  ";
                                    if (cbbfadm.Checked == true)
                                    {
                                        #region before admission

                                        Query += " union select distinct r.stud_name as Name,d.app_no,r.Current_Semester, r.app_formno as Roll_No,r.app_formno as roll_admit,r.app_formno as Reg_No,sum(d.debit) as paid,r.degree_code ,d.paymode as mode ,g.ChlGroupHeader ,feecategory,d.TransCode as ReceiptNo,d.ddno as challan_no ,d.TransDate cal_date,d.feecategory from ft_findailytransaction d,applyn r,FS_ChlGroupHeaderSettings g where  d.app_no = r.app_no and d.HeaderFK=g.HeaderFK and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and r.isconfirm = 1 and r.admission_status = '1' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(r.is_enroll,'0')<>'2'  and r.college_code ='" + collegecode + "' and d.debit>0 ";
                                        // and r.app_no not in (select app_no from Registration  where degree_code in(" + course_all + ") and g.stream ='" + typevalue + "' and Batch_year in(" + batch_all + "))
                                        if (usBasedRights == true)
                                            Query += " and d.EntryUserCode in('" + usercode + "')";

                                        if (typevalue.Trim() != "Both" && typevalue.Trim() != "")
                                        {
                                            Query = Query + " and g.Stream ='" + typevalue + "'";
                                        }
                                        if (batch_all.Trim() != "")
                                        {
                                            Query = Query + " and r.batch_year  in (" + batch_all + ")";
                                        }
                                        if (course_all.Trim() != "")
                                        {
                                            Query = Query + " and r.degree_code  in (" + course_all + ")";
                                        }
                                        if (stefeesem.Trim() != "")
                                        {
                                            Query = Query + " and d.FeeCategory  in (" + stefeesem + ")";
                                        }
                                        if (finyearval.Trim() != "")
                                        {
                                            Query = Query + " and d.FinYearFK   in (" + Newfinayear + ")";
                                        }
                                        if (header_all.Trim() != "")
                                        {
                                            Query = Query + " and ChlGroupHeader   in ('" + header_all + "')";
                                        }
                                        if (bankcode != "")
                                            Query += " and d.DDBankCode in ('" + bankcode + "')";
                                        if (modestatus.Trim() != "")
                                        {
                                            Query += " and d.paymode in ('" + modestatus + "')";
                                        }
                                        Query += " and d.TransDate between '" + dat.ToString("MM/dd/yyyy") + "' and '" + dat1.ToString("MM/dd/yyyy") + "'";
                                        Query += " group by r.app_formno,d.app_no,r.stud_name,r.degree_code,r.Current_Semester,d.paymode ,g.ChlGroupHeader,feecategory,d.TransCode,d.ddno,d.TransDate ";
                                        #endregion
                                    }
                                    //date wise student list
                                    Query += "select distinct r.stud_name as name,r.Current_Semester,r.Reg_No,r.Roll_No,r.degree_code,r.roll_admit,r.batch_year,d.transdate cal_date,d.TransCode as ReceiptNo,d.ddno as challan_no ,d.feecategory,d.App_no  from registration r,ft_findailytransaction d,FS_ChlGroupHeaderSettings g where r.App_No=d.App_No and d.HeaderFK=g.HeaderFK and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and credit=0 and transtype =1 and r.college_code ='" + collegecode + "'";
                                    if (usBasedRights == true)
                                        Query += " and d.EntryUserCode in('" + usercode + "')";
                                    if (commondist != "")
                                        Query = Query + commondist;
                                    if (typevalue.Trim() != "Both" && typevalue.Trim() != "")
                                    {
                                        Query = Query + " and Stream ='" + typevalue + "'";
                                    }
                                    if (batch_all.Trim() != "")
                                    {
                                        Query = Query + " and r.batch_year  in (" + batch_all + ")";
                                    }
                                    if (course_all.Trim() != "")
                                    {
                                        Query = Query + " and r.degree_code  in (" + course_all + ")";
                                    }
                                    if (stefeesem.Trim() != "")
                                    {
                                        Query = Query + " and d.FeeCategory  in (" + stefeesem + ")";
                                    }
                                    if (finyearval.Trim() != "")
                                    {
                                        Query = Query + " and d.FinYearFK   in (" + Newfinayear + ")";
                                    }
                                    if (header_all.Trim() != "")
                                    {
                                        Query = Query + " and ChlGroupHeader   in ('" + header_all + "')";
                                    }
                                    if (bankcode != "")
                                        Query += " and d.DDBankCode in ('" + bankcode + "')";
                                    if (modestatus.Trim() != "")
                                    {
                                        Query += " and d.paymode in ('" + modestatus + "')";
                                    }
                                    Query += " and d.TransDate between '" + dat.ToString("MM/dd/yyyy") + "' and '" + dat1.ToString("MM/dd/yyyy") + "'";
                                    if (cbbfadm.Checked == true)
                                    {
                                        #region before admission

                                        Query += " union select distinct r.stud_name as name,r.Current_Semester,r.app_formno as Reg_No,r.app_formno as Roll_No,r.degree_code,r.app_formno as roll_admit,r.batch_year,d.transdate cal_date,d.TransCode as ReceiptNo,d.ddno as challan_no ,d.feecategory,d.App_no  from applyn r,ft_findailytransaction d,FS_ChlGroupHeaderSettings g where r.App_No=d.App_No and d.HeaderFK=g.HeaderFK and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and credit=0 and transtype =1 and  r.isconfirm = 1 and r.admission_status = '1' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(r.is_enroll,'0')<>'2'   and r.college_code ='" + collegecode + "'";
                                        //and r.app_no not in (select app_no from Registration  where degree_code in(" + course_all + ") and g.stream ='" + typevalue + "'  and Batch_year in(" + batch_all + "))
                                        if (usBasedRights == true)
                                            Query += " and d.EntryUserCode in('" + usercode + "')";

                                        if (typevalue.Trim() != "Both" && typevalue.Trim() != "")
                                        {
                                            Query = Query + " and Stream ='" + typevalue + "'";
                                        }
                                        if (batch_all.Trim() != "")
                                        {
                                            Query = Query + " and r.batch_year  in (" + batch_all + ")";
                                        }
                                        if (course_all.Trim() != "")
                                        {
                                            Query = Query + " and r.degree_code  in (" + course_all + ")";
                                        }
                                        if (stefeesem.Trim() != "")
                                        {
                                            Query = Query + " and d.FeeCategory  in (" + stefeesem + ")";
                                        }
                                        if (finyearval.Trim() != "")
                                        {
                                            Query = Query + " and d.FinYearFK   in (" + Newfinayear + ")";
                                        }
                                        if (header_all.Trim() != "")
                                        {
                                            Query = Query + " and ChlGroupHeader   in ('" + header_all + "')";
                                        }
                                        if (bankcode != "")
                                            Query += " and d.DDBankCode in ('" + bankcode + "')";
                                        if (modestatus.Trim() != "")
                                        {
                                            Query += " and d.paymode in ('" + modestatus + "')";
                                        }
                                        Query += " and d.TransDate between '" + dat.ToString("MM/dd/yyyy") + "' and '" + dat1.ToString("MM/dd/yyyy") + "'";
                                        #endregion
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region arrear list
                                    Query = "select distinct r.stud_name as Name,d.app_no,r.Current_Semester, r.app_formno as Roll_No,r.app_formno as Reg_No,r.app_formno as roll_admit,sum(d.debit) as paid,r.degree_code ,d.paymode as mode ,g.ChlGroupHeader ,feecategory,d.TransCode as ReceiptNo,d.ddno as challan_no ,d.TransDate cal_date,d.feecategory from ft_findailytransaction d,applyn r,FS_ChlGroupHeaderSettings g where  d.app_no = r.app_no and d.HeaderFK=g.HeaderFK and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and r.isconfirm = 1 and r.admission_status = '1' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'  and r.app_no not in (select app_no from Registration  where degree_code in(" + course_all + ") and g.stream ='" + typevalue + "' and Batch_year in(" + batch_all + "))   and r.college_code ='" + collegecode + "' and d.debit>0 ";
                                    if (usBasedRights == true)
                                        Query += " and d.EntryUserCode in('" + usercode + "')";

                                    if (typevalue.Trim() != "Both" && typevalue.Trim() != "")
                                    {
                                        Query = Query + " and g.Stream ='" + typevalue + "'";
                                    }
                                    if (batch_all.Trim() != "")
                                    {
                                        Query = Query + " and r.batch_year  in (" + batch_all + ")";
                                    }
                                    if (course_all.Trim() != "")
                                    {
                                        Query = Query + " and r.degree_code  in (" + course_all + ")";
                                    }
                                    if (stefeesem.Trim() != "")
                                    {
                                        Query = Query + " and d.FeeCategory  in (" + stefeesem + ")";
                                    }
                                    if (finyearval.Trim() != "")
                                    {
                                        Query = Query + " and d.FinYearFK   in (" + Newfinayear + ")";
                                    }
                                    if (header_all.Trim() != "")
                                    {
                                        Query = Query + " and ChlGroupHeader   in ('" + header_all + "')";
                                    }
                                    if (bankcode != "")
                                        Query += " and d.DDBankCode in ('" + bankcode + "')";
                                    if (modestatus.Trim() != "")
                                    {
                                        Query += " and d.paymode in ('" + modestatus + "')";
                                    }
                                    Query += " and d.TransDate between '" + dat.ToString("MM/dd/yyyy") + "' and '" + dat1.ToString("MM/dd/yyyy") + "'";
                                    Query += " group by r.app_formno,d.app_no,r.stud_name,r.degree_code,r.Current_Semester,d.paymode ,g.ChlGroupHeader,feecategory,d.TransCode,d.ddno,d.TransDate ";
                                    //date wise student list
                                    Query += "select distinct r.stud_name as name,r.Current_Semester,r.app_formno as Reg_No,r.app_formno as Roll_No,r.degree_code,r.app_formno as roll_admit,r.batch_year,d.transdate cal_date,d.TransCode as ReceiptNo,d.ddno as challan_no ,d.feecategory,d.App_no  from applyn r,ft_findailytransaction d,FS_ChlGroupHeaderSettings g where r.App_No=d.App_No and d.HeaderFK=g.HeaderFK and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and credit=0 and transtype =1 and  r.isconfirm = 1 and r.admission_status = '1' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'  and r.app_no not in (select app_no from Registration  where degree_code in(" + course_all + ") and g.stream ='" + typevalue + "'  and Batch_year in(" + batch_all + ")) and r.college_code ='" + collegecode + "'";
                                    if (usBasedRights == true)
                                        Query += " and d.EntryUserCode in('" + usercode + "')";

                                    if (typevalue.Trim() != "Both" && typevalue.Trim() != "")
                                    {
                                        Query = Query + " and Stream ='" + typevalue + "'";
                                    }
                                    if (batch_all.Trim() != "")
                                    {
                                        Query = Query + " and r.batch_year  in (" + batch_all + ")";
                                    }
                                    if (course_all.Trim() != "")
                                    {
                                        Query = Query + " and r.degree_code  in (" + course_all + ")";
                                    }
                                    if (stefeesem.Trim() != "")
                                    {
                                        Query = Query + " and d.FeeCategory  in (" + stefeesem + ")";
                                    }
                                    if (finyearval.Trim() != "")
                                    {
                                        Query = Query + " and d.FinYearFK   in (" + Newfinayear + ")";
                                    }
                                    if (header_all.Trim() != "")
                                    {
                                        Query = Query + " and ChlGroupHeader   in ('" + header_all + "')";
                                    }
                                    if (bankcode != "")
                                        Query += " and d.DDBankCode in ('" + bankcode + "')";
                                    if (modestatus.Trim() != "")
                                    {
                                        Query += " and d.paymode in ('" + modestatus + "')";
                                    }
                                    Query += " and d.TransDate between '" + dat.ToString("MM/dd/yyyy") + "' and '" + dat1.ToString("MM/dd/yyyy") + "'";
                                    #endregion
                                }
                                Query = Query + " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode + "'";
                                Query = Query + " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code ='" + collegecode + "'";
                                ds.Clear();
                                ds = da.select_method_wo_parameter(Query, "Text");
                                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                {
                                    #region value

                                    DataView dv = new DataView();
                                    DataView dv1 = new DataView();
                                    DataView dv2 = new DataView();
                                    DataView Dview = new DataView();
                                    finalflag = true;
                                    double Cashtotal = 0;
                                    double checktotal = 0;
                                    double ddtotal = 0;
                                    double Challantotal = 0;
                                    double onlinetotal = 0;
                                    int colcnt = 0;
                                    bool colval = false;
                                    ArrayList arroll = new ArrayList();
                                    ArrayList datearray = new ArrayList();
                                    Dictionary<string, Double> diccurarrfee = new Dictionary<string, double>();
                                    Hashtable GrandTotal = new Hashtable();
                                    Hashtable fnltotal = new Hashtable();

                                    for (DateTime dtdatec = Convert.ToDateTime(y1); dtdatec <= Convert.ToDateTime(y2); dtdatec = dtdatec.AddDays(1))
                                    {
                                        bool dateval = false;
                                        for (int sem = 0; sem < chklsfeesem.Items.Count; sem++)
                                        {
                                            if (chklsfeesem.Items[sem].Selected == true)
                                            {
                                                ds.Tables[1].DefaultView.RowFilter = " cal_date='" + dtdatec + "' and FeeCategory='" + Convert.ToString(chklsfeesem.Items[sem].Value) + "'";
                                                dv2 = ds.Tables[1].DefaultView;
                                                if (dv2.Count > 0)
                                                {
                                                    //date print to spread
                                                    FpSpread1.Sheets[0].RowCount++;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = dtdatec.ToString("dd/MM/yyyy");
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.PowderBlue;
                                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, FpSpread1.Sheets[0].ColumnCount - 1);
                                                    for (int std = 0; std < dv2.Count; std++)
                                                    {
                                                        if (!datearray.Contains(dtdatec.ToString("dd/MM/yyyy")))
                                                        {
                                                            datearray.Add(dtdatec.ToString("dd/MM/yyyy"));

                                                            string dcode = Convert.ToString(dv2[std]["Degree_code"]);
                                                            string Degreenme = "";
                                                            if (ds.Tables[2].Rows.Count > 0)
                                                            {
                                                                ds.Tables[2].DefaultView.RowFilter = "Degree_code='" + dcode + "'";
                                                                Dview = ds.Tables[2].DefaultView;
                                                                if (Dview.Count > 0)
                                                                {
                                                                    Degreenme = Convert.ToString(Dview[0]["degreename"]);
                                                                }
                                                            }
                                                            FpSpread1.Sheets[0].RowCount++;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Degreenme + "-" + Convert.ToString(dv2[std]["current_semester"]);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.LightSkyBlue;
                                                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, FpSpread1.Sheets[0].ColumnCount - 1);
                                                        }
                                                        string dvapp_no = Convert.ToString(dv2[std]["App_No"]);
                                                        string Feecategory = Convert.ToString(dv2[std]["FeeCategory"]);
                                                        FpSpread1.Sheets[0].RowCount++;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dv2[std]["name"]);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dv2[std]["Roll_No"]);
                                                        if (sclflag == false)
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv2[std]["Reg_No"]);
                                                        else
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv2[std]["roll_admit"]);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = txtreg;
                                                        if (!arroll.Contains(Convert.ToString(dv2[std]["Roll_No"])))
                                                        {
                                                            arroll.Add(Convert.ToString(dv2[std]["Roll_No"]));
                                                            sno++;
                                                        }
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();


                                                        // DataView Dview = new DataView();
                                                        //   string dcode = Convert.ToString(dv2[std]["Degree_code"]);
                                                        string Degreename = "";
                                                        if (ds.Tables[2].Rows.Count > 0)
                                                        {
                                                            ds.Tables[2].DefaultView.RowFilter = "Degree_code='" + Convert.ToString(dv2[std]["Degree_code"]) + "'";
                                                            Dview = ds.Tables[2].DefaultView;
                                                            if (Dview.Count > 0)
                                                            {
                                                                Degreename = Convert.ToString(Dview[0]["degreename"]);
                                                            }
                                                        }
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Degreename;

                                                        string TextName = "";
                                                        if (ds.Tables[3].Rows.Count > 0)
                                                        {
                                                            ds.Tables[3].DefaultView.RowFilter = "TextCode='" + Feecategory + "'";
                                                            Dview = ds.Tables[3].DefaultView;
                                                            if (Dview.Count > 0)
                                                            {
                                                                TextName = Convert.ToString(Dview[0]["TextVal"]);
                                                            }
                                                        }
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = TextName;

                                                        string curSem = Convert.ToString(dv2[std]["current_semester"]);
                                                        string feechec = TextName.Split(' ')[0];
                                                        if (Convert.ToInt32(curSem) > Convert.ToInt32(feechec))
                                                        {
                                                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.YellowGreen;
                                                        }


                                                        string reptno = Convert.ToString(dv2[std]["ReceiptNo"]);
                                                        string chalno = Convert.ToString(dv2[std]["challan_no"]);
                                                        int col = 6;
                                                        double Total = 0;
                                                        double balance = 0;
                                                        bool valu = false;
                                                        for (int j = 0; j < chklstaccheader.Items.Count; j++)
                                                        {
                                                            if (chklstaccheader.Items[j].Selected == true)
                                                            {
                                                                string detail = "App_no='" + dvapp_no + "' and FeeCategory='" + Feecategory + "' and ChlGroupHeader='" + Convert.ToString(chklstaccheader.Items[j].Text) + "' and cal_date='" + dtdatec + "'";
                                                                if (chalno != "")
                                                                    detail += "and challan_no='" + chalno + "'";
                                                                if (reptno != "")
                                                                    detail += "and ReceiptNo='" + reptno + "'";

                                                                ds.Tables[0].DefaultView.RowFilter = detail;
                                                                dv1 = ds.Tables[0].DefaultView;
                                                                col++;
                                                                if (dv1.Count > 0)
                                                                {
                                                                    for (int k = 0; k < dv1.Count; k++)
                                                                    {
                                                                        if (colval == false)
                                                                            colcnt = col;
                                                                        colval = true;
                                                                        #region receipt and challan no

                                                                        string receiptno = "";
                                                                        receiptno = Convert.ToString(dv1[k]["ReceiptNo"]);
                                                                        string challanno = Convert.ToString(dv1[k]["challan_no"]);
                                                                        string chaandrpt = "";
                                                                        if (rightscode == 3 || rightscode == 1 || rightscode == 2)
                                                                        {
                                                                            if (receiptno != "" && challanno == "")
                                                                            {
                                                                                chaandrpt = receiptno;
                                                                            }
                                                                            if (receiptno == "" && challanno != "")
                                                                            {
                                                                                chaandrpt = receiptno;
                                                                            }
                                                                            if (receiptno != "" && challanno != "")
                                                                            {
                                                                                chaandrpt = challanno + "/" + receiptno;
                                                                            }
                                                                            if (receiptno == "" && challanno == "")
                                                                            {
                                                                                chaandrpt = challanno;
                                                                            }

                                                                        }
                                                                        if (rightscode == 1)
                                                                        {
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = receiptno;
                                                                        }
                                                                        if (rightscode == 2 || rightscode == 0)
                                                                        {
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = challanno;
                                                                        }
                                                                        if (rightscode == 3)
                                                                        {
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = chaandrpt;
                                                                        }
                                                                        #endregion

                                                                        double.TryParse(Convert.ToString(dv1[k]["paid"]), out balance);
                                                                        Total += balance;

                                                                        dateval = true;
                                                                        // FpSpread1.Sheets[0].RowCount++;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(balance);
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                                                        if (!GrandTotal.ContainsKey(Convert.ToString(chklstaccheader.Items[j].Text)))
                                                                            GrandTotal.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(balance));
                                                                        else
                                                                        {
                                                                            double total = 0;
                                                                            double.TryParse(Convert.ToString(GrandTotal[Convert.ToString(chklstaccheader.Items[j].Text)]), out total);
                                                                            total += balance;
                                                                            GrandTotal.Remove(Convert.ToString(chklstaccheader.Items[j].Text));
                                                                            GrandTotal.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(total));
                                                                        }

                                                                        #region Paymode value set
                                                                        string cursem = Convert.ToString(dv1[k]["Current_Semester"]);
                                                                        string paymode = Convert.ToString(dv1[k]["mode"]);
                                                                        if (balance != 0)
                                                                        {
                                                                            string Linkvalue = "";
                                                                            if (group_user.Contains(';'))
                                                                            {
                                                                                string[] group_semi = group_user.Split(';');
                                                                                group_user = group_semi[0].ToString();

                                                                                Linkvalue = da.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + group_user + "' and college_code ='" + collegecode + "'");
                                                                            }
                                                                            else
                                                                            {
                                                                                Linkvalue = da.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'");
                                                                            }

                                                                            if (Linkvalue == "0")
                                                                            {
                                                                                if (diccurarrfee.ContainsKey(Feecategory + "$" + cursem))
                                                                                {
                                                                                    Double getval = balance + diccurarrfee[Feecategory + "$" + cursem];
                                                                                    diccurarrfee[Feecategory + "$" + cursem] = getval;
                                                                                }
                                                                                else
                                                                                {
                                                                                    diccurarrfee.Add(Feecategory + "$" + cursem, balance);
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                // string sem = "";
                                                                                double amt = 0;
                                                                                string valuenew = returnYearforSem(cursem);
                                                                                if (diccurarrfee.ContainsKey(Feecategory + "$" + valuenew.ToString()))
                                                                                {
                                                                                    Double getval = Convert.ToDouble(balance) + diccurarrfee[Feecategory + "$" + valuenew.ToString()];
                                                                                    diccurarrfee[Feecategory + "$" + valuenew.ToString()] = getval;
                                                                                }
                                                                                else
                                                                                {
                                                                                    diccurarrfee.Add(Feecategory + "$" + valuenew.ToString(), Convert.ToDouble(balance));
                                                                                }

                                                                            }
                                                                            finalflag = true;
                                                                            // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].CellType = dobcell;
                                                                            // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Text = balance;
                                                                            // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].HorizontalAlign = HorizontalAlign.Right;
                                                                            if (paymode == "1")
                                                                            {
                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].BackColor = Color.LightCoral;
                                                                                Cashtotal = Cashtotal + balance;
                                                                            }
                                                                            else if (paymode == "2")
                                                                            {
                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].BackColor = Color.LightGray;
                                                                                checktotal = checktotal + balance;
                                                                            }
                                                                            else if (paymode == "3")
                                                                            {
                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].BackColor = Color.Orange;
                                                                                // Challantotal = Challantotal + Convert.ToDouble(paid_amt);
                                                                                ddtotal = ddtotal + balance;

                                                                            }
                                                                            else if (paymode == "4")
                                                                            {
                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].BackColor = Color.LightGreen;
                                                                                Challantotal = Challantotal + balance;
                                                                            }
                                                                            else if (paymode == "5")
                                                                            {
                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].BackColor = Color.LightGoldenrodYellow;
                                                                                onlinetotal = onlinetotal + balance;
                                                                            }
                                                                        }

                                                                        #endregion
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = "-";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                                                }
                                                            }
                                                        }
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(Total);
                                                        //Total += Convert.ToDouble(dv1[0]["Balance"]);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                                        if (!GrandTotal.ContainsKey(Convert.ToString("Total")))
                                                            GrandTotal.Add(Convert.ToString("Total"), Convert.ToString(Total));
                                                        else
                                                        {
                                                            double total = 0;
                                                            double.TryParse(Convert.ToString(GrandTotal[Convert.ToString("Total")]), out total);
                                                            total += Total;
                                                            GrandTotal.Remove(Convert.ToString("Total"));
                                                            GrandTotal.Add(Convert.ToString("Total"), Convert.ToString(total));
                                                        }
                                                    }
                                                    FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                                    FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                                    FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                                }
                                            }
                                        }

                                        //date wise total
                                        if (dateval == true)
                                        {
                                            #region degreewise total

                                            FpSpread1.Sheets[0].RowCount++;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString("Date Wise Total");
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.MediumTurquoise;
                                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                                            int col1 = 6;
                                            for (int j = 0; j < chklstaccheader.Items.Count; j++)
                                            {
                                                if (chklstaccheader.Items[j].Selected == true)
                                                {
                                                    col1++;
                                                    double GetNewValue = 0;
                                                    double.TryParse(Convert.ToString(GrandTotal[Convert.ToString(chklstaccheader.Items[j].Text)]), out GetNewValue);
                                                    // string GetNewValue = Convert.ToString(GrandTotal[Convert.ToString(chklstaccheader.Items[j].Text)]);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].Text = Convert.ToString(GetNewValue);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].HorizontalAlign = HorizontalAlign.Right;

                                                    //grand total
                                                    if (!fnltotal.ContainsKey(Convert.ToString(chklstaccheader.Items[j].Text)))
                                                        fnltotal.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(GetNewValue));
                                                    else
                                                    {
                                                        double total = 0;
                                                        double.TryParse(Convert.ToString(fnltotal[Convert.ToString(chklstaccheader.Items[j].Text)]), out total);
                                                        total += GetNewValue;
                                                        fnltotal.Remove(Convert.ToString(chklstaccheader.Items[j].Text));
                                                        fnltotal.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(total));
                                                    }
                                                }
                                            }
                                            col1++;
                                            double GetNewValue1 = 0;
                                            double.TryParse(Convert.ToString(GrandTotal[Convert.ToString("Total")]), out GetNewValue1);
                                            // string GetNewValue1 = Convert.ToString(GrandTotal[Convert.ToString("Total")]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].Text = Convert.ToString(GetNewValue1);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].HorizontalAlign = HorizontalAlign.Right;
                                            GrandTotal.Clear();
                                            //last column total
                                            if (!fnltotal.ContainsKey(Convert.ToString("Total")))
                                                fnltotal.Add(Convert.ToString("Total"), Convert.ToString(GetNewValue1));
                                            else
                                            {
                                                double total = 0;
                                                double.TryParse(Convert.ToString(fnltotal[Convert.ToString("Total")]), out total);
                                                total += GetNewValue1;
                                                fnltotal.Remove(Convert.ToString("Total"));
                                                fnltotal.Add(Convert.ToString("Total"), Convert.ToString(total));
                                            }
                                            #endregion
                                        }

                                    }

                                    //final total
                                    #region final total

                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString("Grand Total");
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.LightSeaGreen;
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);

                                    int cols = 6;
                                    for (int j = 0; j < chklstaccheader.Items.Count; j++)
                                    {
                                        if (chklstaccheader.Items[j].Selected == true)
                                        {
                                            cols++;
                                            double GetNewValue = 0;
                                            double.TryParse(Convert.ToString(fnltotal[Convert.ToString(chklstaccheader.Items[j].Text)]), out GetNewValue);
                                            //  string GetNewValue = Convert.ToString(fnltotal[Convert.ToString(chklstaccheader.Items[j].Text)]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cols].Text = Convert.ToString(GetNewValue);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cols].HorizontalAlign = HorizontalAlign.Right;
                                        }
                                    }
                                    cols++;
                                    string GetNewValues1 = Convert.ToString(fnltotal[Convert.ToString("Total")]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cols].Text = Convert.ToString(GetNewValues1);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cols].HorizontalAlign = HorizontalAlign.Right;

                                    #endregion

                                    #region abstract

                                    //abstract
                                    FpSpread1.Sheets[0].RowCount++;// = FpSpread1.Sheets[0].RowCount + 2;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "ABSTRACT";
                                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.Brown;
                                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].ForeColor = Color.White;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 7);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    for (int ps = 0; ps < chklstpaymode.Items.Count; ps++)
                                    {
                                        if (chklstpaymode.Items[ps].Selected == true)
                                        {
                                            string strptype = chklstpaymode.Items[ps].Text.ToString();
                                            FpSpread1.Sheets[0].RowCount++;
                                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 7);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = strptype;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                                            if (strptype == "Cash")
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].Text = Cashtotal.ToString();
                                            }
                                            else if (strptype == "Cheque")
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].Text = checktotal.ToString();
                                            }
                                            else if (strptype == "DD")
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].Text = ddtotal.ToString();
                                            }
                                            else if (strptype == "Challan")
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].Text = Challantotal.ToString();
                                            }
                                            else if (strptype == "Online Pay")
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].Text = onlinetotal.ToString();
                                            }
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].HorizontalAlign = HorizontalAlign.Right;
                                        }
                                    }
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 7);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                                    Double dotvd = Cashtotal + checktotal + ddtotal + Challantotal + onlinetotal;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].Text = dotvd.ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].HorizontalAlign = HorizontalAlign.Right;
                                    #endregion

                                    #region collected and arrear

                                    Double currfee = 0;
                                    Double arrfee = 0;
                                    foreach (var parameter in diccurarrfee)
                                    {
                                        string getsplallow = parameter.Key.ToString();
                                        string getfeeamount = parameter.Value.ToString();
                                        string[] spt = getsplallow.Split('$');
                                        if (spt.GetUpperBound(0) == 1)
                                        {
                                            for (int rcs = 0; rcs < chklsfeesem.Items.Count; rcs++)
                                            {
                                                if (chklsfeesem.Items[rcs].Value.ToString() == spt[0].ToString())
                                                {
                                                    string feeval = chklsfeesem.Items[rcs].Text.ToString();
                                                    string[] stc = feeval.Split(' ');
                                                    if (stc.GetUpperBound(0) >= 0)
                                                    {
                                                        if (stc[0].ToString().Trim() == spt[1].ToString().Trim())
                                                        {
                                                            currfee = currfee + Convert.ToDouble(getfeeamount);
                                                        }
                                                        else
                                                        {
                                                            arrfee = arrfee + Convert.ToDouble(getfeeamount);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (chklsfeesem.Items[rcs].Text.Contains(spt[1].ToString()))
                                                        {
                                                            currfee = currfee + Convert.ToDouble(getfeeamount);
                                                        }
                                                        else
                                                        {
                                                            arrfee = arrfee + Convert.ToDouble(getfeeamount);
                                                        }
                                                    }
                                                    rcs = chklsfeesem.Items.Count;
                                                }
                                            }
                                        }
                                    }
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "REGULAR";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 7);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].Text = currfee.ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "ARREAR";
                                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.YellowGreen;
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 7);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].Text = arrfee.ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].HorizontalAlign = HorizontalAlign.Right;
                                    #endregion



                                    if (FpSpread1.Sheets[0].RowCount > 0)
                                    {
                                        FpSpread1.Visible = true;
                                        btnprintmaster.Visible = true;
                                        lblrptname.Visible = true;
                                        txtexcelname.Visible = true;
                                        btnxl.Visible = true;
                                        lblc1.Visible = true;
                                        lblc2.Visible = true;
                                        lblc3.Visible = true;
                                        lblc4.Visible = true;
                                        lblc5.Visible = true;
                                        lblcard.Visible = true;
                                    }
                                    else
                                    {
                                        FpSpread1.Visible = false;
                                        btnprintmaster.Visible = false;
                                        lblrptname.Visible = false;
                                        txtexcelname.Visible = false;
                                        btnxl.Visible = false;
                                        lblc1.Visible = false;
                                        lblc2.Visible = false;
                                        lblc3.Visible = false;
                                        lblc4.Visible = false;
                                        lblc5.Visible = false;
                                        lblcard.Visible = false;
                                    }
                                    #endregion
                                }
                                #endregion
                            }
                        }
                        else//Header..............
                        {
                            #region without date Header

                            if (header_all.Trim() != "" && header_all != null)
                            {
                                strstufillerfee = strstufillerfee + " and fi.headerfk in('" + header_all + "')";
                                demandquery = " and a.headerfk in('" + header_all + "') ";
                            }

                            // feecategorycolumn = feecategorycolumn + ",a.headerfk as header_id";

                            for (int a = 0; a < chklstaccheader.Items.Count; a++)
                            {
                                if (chklstaccheader.Items[a].Selected == true)
                                {
                                    if (ddlfeetype.Text == "Paid")
                                    {
                                        FpSpread1.Sheets[0].ColumnCount++;
                                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Width = 200;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = chklstaccheader.Items[a].Text;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Paid";
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Note = chklstaccheader.Items[a].Value;

                                    }
                                    else if (ddlfeetype.Text == "Yet To Be Paid")
                                    {
                                        FpSpread1.Sheets[0].ColumnCount++;
                                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Width = 200;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = chklstaccheader.Items[a].Text;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Balance";
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Note = chklstaccheader.Items[a].Value;
                                    }
                                    else
                                    {

                                        FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 3;
                                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3].Width = 200;
                                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Width = 200;
                                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Width = 200;
                                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3, 1, 3);
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 3].Text = chklstaccheader.Items[a].Text;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3].Text = "Alloted";
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3].Note = chklstaccheader.Items[a].Value;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Text = "Paid";
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Note = chklstaccheader.Items[a].Value;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Balance";
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Note = chklstaccheader.Items[a].Value;
                                    }

                                }
                            }
                            if (ddlfeetype.Text == "Paid")
                            {
                                FpSpread1.Sheets[0].ColumnCount++;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Total Paid";
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1, 2, 1);
                            }
                            else if (ddlfeetype.Text == "Yet To Be Paid")
                            {
                                FpSpread1.Sheets[0].ColumnCount++;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Total Balance";
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1, 2, 1);
                            }
                            else
                            {
                                FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 5;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 5].Text = "Total Alloted";
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 5, 2, 1);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 4].Text = "Total Paid";
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 4, 2, 1);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3].Text = "Total Balance";
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3, 2, 1);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Text = "Excess Amount";
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2, 2, 1);

                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Status";
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1, 3, 1);
                            }

                            #endregion
                        }

                        #region without date Paid Header,Ledger and Group header

                        if (ddlfeetype.SelectedItem.Text == "Paid")
                        {
                            string cc = "";
                            string debar = "";
                            string disc = "";
                            string commondist = "";
                            if (cblinclude.Items.Count > 0)
                            {
                                for (int i = 0; i < cblinclude.Items.Count; i++)
                                {
                                    if (cblinclude.Items[i].Selected == true)
                                    {
                                        if (cblinclude.Items[i].Value == "1")
                                        {
                                            cc = " r.cc=1";
                                        }
                                        if (cblinclude.Items[i].Value == "2")
                                        {
                                            debar = "  r.Exam_Flag like '%debar'";
                                        }
                                        if (cblinclude.Items[i].Value == "3")
                                        {
                                            disc = "  r.DelFlag=1";
                                        }
                                    }
                                }
                            }

                            if (cc != "" && debar == "" && disc == "")
                                commondist = " and (" + cc + " or r.cc=0)  and r.Exam_Flag<>'debar' and r.DelFlag=0";

                            if (cc == "" && debar != "" && disc == "")
                                commondist = " and r.cc=0  and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";

                            if (cc == "" && debar == "" && disc != "")
                                commondist = " and r.cc=0  and r.Exam_Flag<>'debar' and (" + disc + " or r.DelFlag=0)";

                            if (cc != "" && debar != "" && disc == "")
                                commondist = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";

                            if (cc == "" && debar != "" && disc != "")
                                commondist = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar')  and (" + disc + " or r.DelFlag=0)";

                            if (cc != "" && debar == "" && disc != "")
                                commondist = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar'  and (" + disc + " or r.DelFlag=0)";

                            else if (cc == "" && debar == "" && disc == "")
                                commondist = " and r.cc=0  and r.Exam_Flag<>'debar' and r.DelFlag=0";

                            if (cc != "" && debar != "" && disc != "")
                                commondist = "";

                            #region old
                            if (ddlacctype.SelectedItem.Text != "Group Header")
                            {
                                regrolladmit = regrolladmit + strstufillerfee + " and f.app_no=r.app_no  )";

                                //-------------Modify By M.SakthiPriya  04/02/2015
                                if (ddlstudtype.SelectedItem.Text != "EnRoll")
                                {
                                    approlladmit = " and r.app_formno in " + strstufillerfeeapplyn + " and f.app_formno=r.app_formno)";
                                }
                                else
                                {
                                    approlladmit = " and applyn.app_formno in " + strstufillerfeeapplyn + " and f.app_formno=applyn.app_formno)";
                                }
                                //------------------End------------------                             

                                //header  paid
                                if (ddlstudtype.SelectedItem.Text.Trim() != "Before Admission")
                                {
                                    demquery = "select distinct r.stud_name as Name,r.Current_Semester,r.Roll_No,r.roll_admit,r.degree_code,sum(totalamount) as demand ,a.headerfk as header_id" + feecategorycolumn + " from ft_feeallot a,registration r where a.app_no=r.app_no  ";
                                    if (commondist != "")
                                        demquery = demquery + commondist;


                                    demquery = demquery + " and a.totalamount>0   " + demandquery + " " + degreequery + " " + batchquery + " " + feesemquery + " " + finyearval + " and r.college_code ='" + collegecode + "' group by a.app_no,r.roll_admit,r.roll_no,r.stud_name,r.degree_code,r.Current_Semester ,a.headerfk" + feecategorycolumngroup + " ";
                                    if (cbbfadm.Checked == true)
                                    {
                                        demquery = demquery + "   union all select distinct r.stud_name as Name,r.Current_Semester,r.app_formno,app_formno as roll_admit,r.degree_code,sum(totalamount) as demand ,a.headerfk as header_id " + feecategorycolumn + " from ft_feeallot a,applyn r where  a.app_no=r.app_no ";
                                        if (checkdicon.Checked == false)
                                        {
                                            //  demquery = demquery + " and r.DelFlag =0";
                                        }
                                        demquery = demquery + " and a.totalamount>0 and r.isconfirm = 1 and r.admission_status = '1' " + demandquery + "" + degreequery + " " + batchquery + " " + feesemquery + " " + finyearval + " and r.college_code ='" + collegecode + "' and isnull(r.is_enroll,'0')<>'2' group by r.app_no,r.app_formno ,r.stud_name,r.degree_code,r.Current_Semester,a.headerfk" + feecategorycolumngroup + "";
                                        //  and r.app_no not in (select app_no from Registration where degree_code in(" + course_all + ")  and Batch_year in(" + batch_all + ") )
                                    }
                                }
                                else
                                {
                                    //arrear List 
                                    demquery = " select distinct r.stud_name as Name,r.Current_Semester,r.app_formno,app_formno as roll_admit,r.degree_code,sum(totalamount) as demand ,a.headerfk as header_id " + feecategorycolumn + " from ft_feeallot a,applyn r where  a.app_no=r.app_no ";
                                    demquery = demquery + " and a.totalamount>0 and r.isconfirm = 1 and r.admission_status = '1' " + demandquery + "" + degreequery + " " + batchquery + " " + feesemquery + " " + finyearval + " and r.college_code ='" + collegecode + "'   and r.app_no not in (select app_no from Registration where degree_code in(" + course_all + ")  and Batch_year in(" + batch_all + ") ) group by r.app_no,r.app_formno ,r.stud_name,r.degree_code,r.Current_Semester,a.headerfk" + feecategorycolumngroup + "";
                                }

                                string daterange = "";
                                if (chkdate.Checked == true)
                                {
                                    daterange = " and a.transdate between '" + dat.ToString("MM/dd/yyyy") + "' and '" + dat1.ToString("MM/dd/yyyy") + "'";
                                }
                                if (chkyettobepaid.Checked == true && chkdate.Checked == true)
                                {
                                    daterange = " and a.transdate<='" + dat1.ToString("MM/dd/yyyy") + "'";
                                }
                                if (ddlfeetype.SelectedItem.Text == "Paid")
                                {
                                    for (int mode = 0; mode < chklstpaymode.Items.Count; mode++)
                                    {
                                        if (chklstpaymode.Items[mode].Selected == true)
                                        {
                                            if (chklstpaymode.Items[mode].Text == "Cash")
                                            {
                                                if (modestatus == "")
                                                {
                                                    modestatus = "1";
                                                }
                                                else
                                                {
                                                    modestatus = modestatus + "','" + "1";
                                                }
                                            }
                                            if (chklstpaymode.Items[mode].Text == "Cheque")
                                            {
                                                if (modestatus == "")
                                                {
                                                    modestatus = "2";
                                                }
                                                else
                                                {
                                                    modestatus = modestatus + "','" + "2";
                                                }
                                            }
                                            if (chklstpaymode.Items[mode].Text == "DD")
                                            {
                                                if (modestatus == "")
                                                {
                                                    modestatus = "3";
                                                }
                                                else
                                                {
                                                    modestatus = modestatus + "','" + "3";
                                                }
                                            }
                                            if (chklstpaymode.Items[mode].Text == "Challan")
                                            {
                                                if (modestatus == "")
                                                {
                                                    modestatus = "4";
                                                }
                                                else
                                                {
                                                    modestatus = modestatus + "','" + "4";
                                                }
                                            }
                                            if (chklstpaymode.Items[mode].Text == "Online Pay")
                                            {
                                                if (modestatus == "")
                                                {
                                                    modestatus = "5";
                                                }
                                                else
                                                {
                                                    modestatus = modestatus + "','" + "5";
                                                }
                                            }

                                        }

                                    }
                                    if (modestatus.Trim() != "")
                                    {
                                        modestatus = " and a.paymode in ('" + modestatus + "')";
                                    }
                                    if (ddlstudtype.SelectedItem.Text.Trim() != "Before Admission")
                                    {
                                        paidquery = "select distinct r.stud_name as Name,r.Current_Semester, r.Roll_No,r.roll_admit,r.Reg_No,sum(a.debit) as paid,r.degree_code ,a.paymode as mode ,a.headerfk as header_id" + feecategorycolumn + ",a.TransCode as ReceiptNo,a.ddno as challan_no ,a.feecategory from ft_findailytransaction a,registration r where  a.app_no = r.app_no   " + daterange + " " + demandquery + " " + degreequery + " " + batchquery + " " + feesemquery + " " + modestatus + " " + finyearval + " and credit=0 and transtype=1 and a.debit>0  and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' ";
                                        if (commondist != "")
                                            paidquery = paidquery + commondist;
                                        if (usBasedRights == true)
                                            paidquery += " and a.EntryUserCode in('" + usercode + "')";

                                        if (bankcode != "")
                                            paidquery += " and a.DDBankCode in('" + bankcode + "')";

                                        paidquery = paidquery + " and r.college_code ='" + collegecode + "' group by r.roll_no,r.Reg_No,r.roll_admit,r.stud_name,r.degree_code,r.Current_Semester,a.paymode  ,a.headerfk" + feecategorycolumngroup + ",a.TransCode,a.ddno,a.feecategory";
                                        if (cbbfadm.Checked == true)
                                        {
                                            paidquery = paidquery + " union select distinct r.stud_name as Name,r.Current_Semester,r.app_formno as Roll_No,r.app_formno as Reg_No,r.app_formno as roll_admit,sum(a.debit) as paid,r.degree_code ,a.paymode as mode ,a.headerfk as header_id" + feecategorycolumn + ",a.TransCode as ReceiptNo,a.ddno as challan_no ,a.feecategory from ft_findailytransaction a,applyn r  where a.app_no = r.app_no  " + daterange + " " + demandquery + " " + degreequery + " " + batchquery + " " + feesemquery + "  " + modestatus + "  " + finyearval + " and credit=0 and transtype=1 and a.debit>0  and r.isconfirm = 1 and r.admission_status = '1' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(r.is_enroll,'0')<>'2'  ";
                                            //and r.app_no not in (select app_no from Registration where degree_code in(" + course_all + ")  and Batch_year in(" + batch_all + ") )
                                            if (bankcode != "")
                                                paidquery += " and a.DDBankCode in ('" + bankcode + "')";
                                            if (usBasedRights == true)
                                                paidquery += " and a.EntryUserCode in('" + usercode + "')";
                                            paidquery = paidquery + " and r.college_code ='" + collegecode + "' group by r.app_no,r.app_formno,r.stud_name,r.degree_code,r.Current_Semester,a.paymode ,a.headerfk" + feecategorycolumngroup + ",a.TransCode,a.ddno,a.feecategory";
                                        }
                                    }
                                    else
                                    {
                                        //arrear list
                                        paidquery = " select distinct r.stud_name as Name,r.Current_Semester,r.app_formno as Roll_No,r.app_formno as Reg_No,r.app_formno as roll_admit,sum(a.debit) as paid,r.degree_code ,a.paymode as mode ,a.headerfk as header_id" + feecategorycolumn + ",a.TransCode as ReceiptNo,a.ddno as challan_no ,a.feecategory from ft_findailytransaction a,applyn r  where a.app_no = r.app_no  " + daterange + " " + demandquery + " " + degreequery + " " + batchquery + " " + feesemquery + "  " + modestatus + "  " + finyearval + " and credit=0 and transtype=1 and a.debit>0  and r.isconfirm = 1 and r.admission_status = '1' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'  and r.app_no not in (select app_no from Registration where degree_code in(" + course_all + ")  and Batch_year in(" + batch_all + ") )";
                                        if (bankcode != "")
                                            paidquery += " and a.DDBankCode in ('" + bankcode + "')";
                                        if (usBasedRights == true)
                                            paidquery += " and a.EntryUserCode in('" + usercode + "')";
                                        paidquery = paidquery + " and r.college_code ='" + collegecode + "' group by r.app_no,r.app_formno,r.stud_name,r.degree_code,r.Current_Semester,a.paymode ,a.headerfk" + feecategorycolumngroup + ",a.TransCode,a.ddno,a.feecategory";
                                    }

                                }
                                else
                                {
                                    //header yet be paid
                                    paidquery = "select distinct r.stud_name as Name,r.Current_Semester, r.Roll_No,r.roll_admit,r.Reg_No,sum(a.debit) as paid,r.degree_code  ,a.headerfk as header_id " + feecategorycolumn + ",a.TransCode as ReceiptNo,a.ddno as challan_no ,a.feecategory from ft_findailytransaction a,registration r where a.app_no = r.app_no   " + daterange + "  " + demandquery + " " + degreequery + " " + batchquery + " " + feesemquery + "  " + finyearval + " " + finyearval + " and credit=0 and transtype=1 and a.debit>0 and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'";

                                    if (commondist != "")
                                        paidquery = paidquery + commondist;
                                    if (usBasedRights == true)
                                        paidquery += " and a.EntryUserCode in('" + usercode + "')";

                                    if (bankcode != "")
                                        paidquery += " and a.DDBankCode in('" + bankcode + "')";

                                    paidquery = paidquery + " and r.college_code ='" + collegecode + "' group by r.roll_no,r.roll_admit,r.Reg_No,r.stud_name,r.degree_code,r.Current_Semester ,a.headerfk" + feecategorycolumngroup + ",a.TransCode,a.ddno,a.feecategory";
                                    if (cbbfadm.Checked == true)
                                    {

                                        paidquery = paidquery + "  union select distinct r.stud_name as Name,r.Current_Semester,r.app_formno as Roll_No,r.app_formno as  Reg_No,r.app_formno as roll_admit,sum(a.debit) as paid,r.degree_code ,a.headerfk as header_id " + feecategorycolumn + ",a.TransCode as ReceiptNo,a.ddno as challan_no ,a.feecategory from ft_findailytransaction a,applyn r  where a.app_no = r.app_no " + daterange + "  " + demandquery + " " + degreequery + " " + batchquery + " " + feesemquery + " " + finyearval + " and credit=0 and transtype=1 and a.debit>0  and r.isconfirm = 1 and r.admission_status = 0 and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'  and r.cc=0 and r.Exam_Flag<>'debar' ";
                                        if (bankcode != "")
                                            paidquery += " and a.DDBankCode in ('" + bankcode + "')";
                                        if (usBasedRights == true)
                                            paidquery += " and a.EntryUserCode in('" + usercode + "')";
                                        paidquery = paidquery + "" + finyearval + " and r.college_code ='" + collegecode + "' group by r.app_no,r.app_formno,r.stud_name,r.degree_code,r.Current_Semester,a.headerfk" + feecategorycolumngroup + ",a.TransCode,a.ddno,a.feecategory";
                                    }

                                }

                                if (ddlstudtype.SelectedItem.Text == "Regular" || ddlstudtype.SelectedItem.Text == "Lateral" || ddlstudtype.SelectedItem.Text == "Transfer")
                                {
                                    str = "select distinct r.stud_name as Name,r.Current_Semester,r.Reg_No,r.Roll_No,r.degree_code,r.roll_admit,r.batch_year from registration r, ft_feeallot a where r.cc=0 and r.delflag=0 and r.app_no = a.app_no and totalamount>0 and r.college_code ='" + collegecode + "'  and r.exam_flag<>'debar' and r.mode='" + ddlstudtype.SelectedValue.ToString() + "' " + degreequery + " " + batchquery + " " + delflg + " " + demandquery + " " + feesemquery + " " + finyearval + " " + strorder + "";
                                }
                                else if (ddlstudtype.SelectedItem.Text == "Re-admit")
                                {
                                    str = " select distinct r.stud_name as Name,r.Current_Semester,r.Reg_No,r.Roll_No,r.degree_code,roll_admit,r.batch_year from Readmission a,Registration r where a.App_no=r.App_No  and r.cc=0  and r.delflag=0 and r.exam_flag<>'debar' and r.college_code ='" + collegecode + "'  and a.newbatch_year in(" + batch_all + ") " + regrolladmit + " " + finyearval + " " + strorder + "";//Modify By M.SakthiPriya 04/02/2015
                                }
                                else if (ddlstudtype.SelectedItem.Text == "Re-join")
                                {
                                    str = "select distinct r.stud_name as Name,r.Current_Semester,r.Reg_No,r.Roll_No,r.degree_code,roll_admit,r.batch_year from rejoin_list a,Registration r where a.roll_no=r.Roll_No  and r.cc=0  and r.delflag=0 and r.exam_flag<>'debar' and r.college_code ='" + collegecode + "' and a.newBatchYear in(" + batch_all + ") " + regrolladmit + " " + finyearval + " " + strorder + "";//Modify By M.SakthiPriya 04/02/2015
                                }
                                else if (ddlstudtype.SelectedItem.Text == "EnRoll")
                                {
                                    str = "select distinct r.stud_name as Name,r.Current_Semester,'' as Reg_No,r.app_formno as Roll_No,r.degree_code,'' as roll_admit,r.batch_year from applyn r,ft_feeallot a where r.app_no = a.app_no and r.isconfirm=1 and isnull(admission_status,0) = 0 and totalamount>0 and r.is_enroll=1 and r.isconfirm = 1 and r.admission_status = 0 and r.college_code ='" + collegecode + "'   " + degreequery + " " + batchquery + " " + demandquery + " " + feesemquery + " " + finyearval + " " + strorder + "";
                                }
                                else
                                {
                                    //header yetbe paid
                                    if (ddlstudtype.SelectedItem.Text.Trim() != "Before Admission")
                                    {
                                        str = "select distinct r.stud_name as Name,r.Current_Semester,r.Reg_No,r.Roll_No,r.degree_code,r.roll_admit,r.batch_year from ft_feeallot a,registration r where  a.app_no=r.app_no and a.totalamount>0    and r.exam_flag<>'debar' and r.college_code ='" + collegecode + "' " + degreequery + " " + batchquery + " " + delflg + " " + demandquery + " " + feesemquery + " " + finyearval + " ";
                                        if (cbbfadm.Checked == true)
                                        {
                                            str = str + " union all select distinct r.stud_name as Name,r.Current_Semester,'' Reg_No,'' Roll_No,r.degree_code,r.app_formno as roll_admit,r.batch_year from ft_feeallot a,applyn r where a.app_no=r.app_no and a.totalamount>0  and r.isconfirm = 1 and r.admission_status = '1' and isnull(r.is_enroll,'0')<>'2'   and r.college_code ='" + collegecode + "'" + degreequery + " " + batchquery + " " + demandquery + " " + feesemquery + " " + finyearval + " " + strorder + " ";
                                            //and r.app_no not in (select app_no from Registration where degree_code in(" + course_all + ")  and Batch_year in(" + batch_all + ") )
                                            strorder = "";
                                        }
                                        str += " " + strorder + "";
                                    }
                                    else
                                    {
                                        str = " select distinct r.stud_name as Name,r.Current_Semester,r.app_formno as Reg_No,r.app_formno as Roll_No,r.degree_code,r.app_formno as roll_admit,r.batch_year from ft_feeallot a,applyn r where a.app_no=r.app_no and a.totalamount>0  and r.isconfirm = 1 and r.admission_status = '1'  and r.app_no not in (select app_no from Registration where degree_code in(" + course_all + ")  and Batch_year in(" + batch_all + ") ) and r.college_code ='" + collegecode + "'" + degreequery + " " + batchquery + " " + demandquery + " " + feesemquery + " " + finyearval + " " + strorder + " ";
                                    }
                                }
                                ds.Reset();
                                ds.Dispose();
                                ds = da.select_method_wo_parameter(demquery, "Text");

                                ds1.Reset();
                                ds1.Dispose();
                                ds1 = da.select_method_wo_parameter(paidquery, "Text");

                                degquery = "select c.course_id,c.course_name,r.degree_code,r.acronym,e.Dept_Name from course c,degree r,Department e where c.course_id=r.course_id and r.Dept_Code=e.Dept_Code " + degreequery + " and r.college_code ='" + collegecode + "'";
                                ds2 = da.select_method_wo_parameter(degquery, "Text");

                                ds3.Reset();
                                ds3.Dispose();
                                ds3 = da.select_method_wo_parameter(str, "text");
                            #endregion
                                #region old
                                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                {
                                    int sclValue = 0;
                                    int.TryParse(Convert.ToString(sclSett()), out sclValue);
                                    Dictionary<string, Double> diccurarrfee = new Dictionary<string, double>();
                                    FpSpread1.Visible = true;
                                    btnprintmaster.Visible = true;
                                    lblrptname.Visible = true;
                                    txtexcelname.Visible = true;
                                    btnxl.Visible = true;
                                    errmsg.Visible = false;
                                    lblc1.Visible = false;
                                    lblc2.Visible = false;
                                    lblc3.Visible = false;
                                    lblc4.Visible = false;
                                    lblc5.Visible = false;
                                    lblcard.Visible = false;
                                    double Cashtotal = 0;
                                    double checktotal = 0;
                                    double ddtotal = 0;
                                    double Challantotal = 0;
                                    double onlinetotal = 0;
                                    double cardtotal = 0;
                                    int col = 0;
                                    bool colval = false;
                                    string feecater = "";
                                    int fl = 0;
                                    double tot_ledge = 0;
                                    string[] gtfeecat = strfeelist.Split('/');
                                    string[] getfeetext = strfeeval.Split('/');
                                    if (ds3.Tables[0].Rows.Count > 0)
                                    {
                                        for (int i = 0; i < ds3.Tables[0].Rows.Count; i++)
                                        {
                                            for (int fec = 0; fec <= gtfeecat.GetUpperBound(0); fec++)
                                            {
                                                string getfee = gtfeecat[fec].ToString();
                                                string feetx = getfeetext[fec].ToString();
                                                string filfeecate = "";
                                                if (getfee.Trim() != "" && getfee != null)
                                                {
                                                    filfeecate = " and feecategory='" + getfee + "'";
                                                }

                                                string tempdegreedeatisl = ds3.Tables[0].Rows[i]["batch_year"].ToString() + '-' + ds3.Tables[0].Rows[i]["degree_code"].ToString() + '-' + ds3.Tables[0].Rows[i]["Current_Semester"].ToString();
                                                if (tempdegreedeatisl != getdegreedetails)
                                                {
                                                    courseid = ds3.Tables[0].Rows[i]["degree_code"].ToString();
                                                    batcyear = ds3.Tables[0].Rows[i]["batch_year"].ToString();
                                                    year = ds3.Tables[0].Rows[i]["Current_Semester"].ToString();

                                                    ds2.Tables[0].DefaultView.RowFilter = "degree_code='" + courseid + "'";
                                                    dv_get_course = ds2.Tables[0].DefaultView;
                                                    if (dv_get_course.Count > 0)
                                                    {
                                                        deg_acr = dv_get_course[0]["course_name"].ToString() + "-" + dv_get_course[0]["acronym"].ToString();
                                                        deg_dept = dv_get_course[0]["course_name"].ToString() + "-" + dv_get_course[0]["Dept_Name"].ToString();
                                                    }
                                                    if (getdegreedetails != "")
                                                    {
                                                        if (degflag == true)
                                                        {
                                                            FpSpread1.Sheets[0].RowCount++;
                                                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.LightCyan;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Degree Wise Total";
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                                                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 5);

                                                            for (int d = 7; d < FpSpread1.Sheets[0].Columns.Count; d = d + 1)
                                                            {
                                                                Double setva = 0;
                                                                if (dictotal.ContainsKey(d))
                                                                {
                                                                    setva = dictotal[d];
                                                                    dictotal[d] = 0;
                                                                }
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Text = setva.ToString();
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].HorizontalAlign = HorizontalAlign.Right;
                                                                if (dicgrandtotal.ContainsKey(d))
                                                                {
                                                                    Double val = dicgrandtotal[d];
                                                                    val = val + setva;
                                                                    dicgrandtotal[d] = val;
                                                                }
                                                                else
                                                                    dicgrandtotal.Add(d, setva);
                                                            }
                                                            dictotal.Clear();
                                                        }
                                                        else
                                                            FpSpread1.Sheets[0].Rows[degreerow].Visible = false;
                                                    }
                                                    degflag = false;
                                                    FpSpread1.Sheets[0].RowCount++;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = batcyear + "-" + deg_dept + "/Sem-" + year;
                                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, FpSpread1.Sheets[0].Columns.Count);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = Color.LightSkyBlue;
                                                    degreerow = FpSpread1.Sheets[0].RowCount - 1;
                                                    getdegreedetails = tempdegreedeatisl;
                                                }

                                                string name = ds3.Tables[0].Rows[i]["Name"].ToString();
                                                string name_roll = ds3.Tables[0].Rows[i]["Roll_No"].ToString() + "-" + ds3.Tables[0].Rows[i]["Name"].ToString();
                                                string rollno = ds3.Tables[0].Rows[i]["Roll_No"].ToString();
                                                string regno = ds3.Tables[0].Rows[i]["Reg_No"].ToString();
                                                string rolladmit = ds3.Tables[0].Rows[i]["roll_admit"].ToString();
                                                string receiptno = "";
                                                string challanno = "";
                                                if (rollno.Trim() == "")
                                                    rollno = ds3.Tables[0].Rows[i]["roll_admit"].ToString();

                                                FpSpread1.Sheets[0].RowCount++;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = name;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = rollno;
                                                if (sclflag == false)
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = regno;
                                                else
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = rolladmit;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = txtreg;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = deg_acr;
                                                if (feecateflag == true)
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = feetx;
                                                else
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = year;

                                                int r = FpSpread1.Sheets[0].RowCount - 1;
                                                double total_paid = 0, total_due = 0, ledge_tot = 0;
                                                dicstu.Clear();
                                                dicyetpaid.Clear();
                                                challanAndReceiptNoRights();
                                                ds1.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "' " + filfeecate + "";
                                                dv_paid_data = ds1.Tables[0].DefaultView;
                                                string feecat = "";
                                                string cursem = "";

                                                for (int so = 0; so < dv_paid_data.Count; so++)
                                                {
                                                    receiptno = dv_paid_data[so]["ReceiptNo"].ToString();
                                                    challanno = dv_paid_data[so]["challan_no"].ToString();
                                                    feecat = dv_paid_data[so]["FeeCategory"].ToString();
                                                    cursem = dv_paid_data[so]["current_semester"].ToString();
                                                    string feechec = string.Empty;
                                                    if (sclValue == 0)
                                                        feechec = feetx.Split(' ')[1];
                                                    else
                                                        feechec = feetx.Split(' ')[0];
                                                    // string feechec = feetx.Split(' ')[0];
                                                    if (Convert.ToInt32(cursem) > Convert.ToInt32(feechec))
                                                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.YellowGreen;
                                                    //regular or arrer checked
                                                    string chaandrpt = "";
                                                    if (rightscode == 3 || rightscode == 1 || rightscode == 2)
                                                    {
                                                        if (receiptno != "" && challanno == "")
                                                            chaandrpt = receiptno;

                                                        if (receiptno == "" && challanno != "")
                                                            chaandrpt = challanno;

                                                        if (receiptno != "" && challanno != "")
                                                            chaandrpt = challanno + "/" + receiptno;


                                                        if (receiptno == "" && challanno == "")
                                                            chaandrpt = challanno;
                                                    }
                                                    if (rightscode == 1)
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = receiptno;
                                                    if (rightscode == 2 || rightscode == 0)
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = challanno;
                                                    if (rightscode == 3)
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = chaandrpt;
                                                    if (sclValue == 0)
                                                        FpSpread1.Sheets[0].Columns[6].Visible = false;
                                                    else
                                                        FpSpread1.Sheets[0].Columns[6].Visible = true;

                                                    string fecode = "";
                                                    if (ddlacctype.SelectedItem.Text == "Ledger")
                                                        fecode = dv_paid_data[so]["header_id"].ToString() + '-' + dv_paid_data[so]["fee_code"].ToString();
                                                    else
                                                        fecode = dv_paid_data[so]["header_id"].ToString();
                                                    string paid = dv_paid_data[so]["paid"].ToString();
                                                    if (ddlfeetype.Text == "Paid")
                                                    {
                                                        string pmode = dv_paid_data[so]["mode"].ToString();
                                                        if (!dicstu.ContainsKey(fecode))
                                                            dicstu.Add(fecode, paid + '/' + pmode);
                                                        else
                                                        {
                                                            Double amountsf = Convert.ToDouble(paid);
                                                            string strgetval = dicstu[fecode];
                                                            string[] spval = strgetval.Split('/');
                                                            if (spval.GetUpperBound(0) > 0)
                                                            {
                                                                string val = spval[0].ToString();
                                                                if (val.Trim() != "")
                                                                    amountsf = amountsf + Convert.ToDouble(val);
                                                            }
                                                            dicstu[fecode] = amountsf.ToString() + '/' + pmode;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (!dicstu.ContainsKey(fecode))
                                                            dicstu.Add(fecode, paid);
                                                        else
                                                        {
                                                            Double amountsf = Convert.ToDouble(dicstu[fecode]);
                                                            amountsf = amountsf + Convert.ToDouble(paid);
                                                            dicstu[fecode] = amountsf.ToString();
                                                        }
                                                    }
                                                }

                                                if (ddlfeetype.Text == "Paid")
                                                {
                                                    lblc1.Visible = true;
                                                    lblc2.Visible = true;
                                                    lblc3.Visible = true;
                                                    lblc4.Visible = true;
                                                    lblc5.Visible = true;
                                                    lblcard.Visible = true;
                                                }
                                                int inco = 1;
                                                int fincol = 1;
                                                string headval = "";
                                                int ferow = 1;
                                                if (ddlacctype.SelectedItem.Text == "Ledger")
                                                    ferow = 2;
                                                else if (ddlacctype.SelectedItem.Text == "Group Header")
                                                    ferow = 2;
                                                headval = "Paid";
                                                for (int d = 7; d < FpSpread1.Sheets[0].Columns.Count - fincol; d = d + inco)
                                                {
                                                    if (FpSpread1.Sheets[0].ColumnHeader.Cells[ferow, d].Text == headval)
                                                    {
                                                        if (colval == false)
                                                            col = d;
                                                        colval = true;
                                                        string code = FpSpread1.Sheets[0].ColumnHeader.Cells[ferow, d].Note.ToString();
                                                        if (ddlacctype.SelectedItem.Text == "Ledger")
                                                        {
                                                            code = FpSpread1.Sheets[0].ColumnHeader.Cells[0, d].Note.ToString() + '-' + FpSpread1.Sheets[0].ColumnHeader.Cells[ferow, d].Note.ToString();
                                                        }

                                                        string paid_amt = "";
                                                        string mode = "";
                                                        string demand_amt = "";
                                                        if (dicyetpaid.ContainsKey(code))
                                                            demand_amt = dicyetpaid[code].ToString();
                                                        if (demand_amt.Trim() == "" || demand_amt == null)
                                                            demand_amt = "0";
                                                        if (dicstu.ContainsKey(code))
                                                        {
                                                            string[] setval = dicstu[code].Split('/');
                                                            if (setval.GetUpperBound(0) == 1)
                                                                mode = setval[1].ToString();
                                                            paid_amt = setval[0].ToString();
                                                        }
                                                        if (paid_amt.Trim() == "" || paid_amt == null)
                                                            paid_amt = "0";
                                                        if (ddlfeetype.Text == "Paid")
                                                        {
                                                            #region old

                                                            //if (paid_amt == "0" || paid_amt == "")
                                                            //{
                                                            //    finalflag = true;
                                                            //    FpSpread1.Sheets[0].Cells[r, d].Text = "-";
                                                            //    FpSpread1.Sheets[0].Cells[r, d].HorizontalAlign = HorizontalAlign.Center;
                                                            //}
                                                            //else
                                                            //{
                                                            //    finalflag = true;
                                                            //    FpSpread1.Sheets[0].Cells[r, d].Text = paid_amt;
                                                            //    if (!dictotal.ContainsKey(d))
                                                            //    {
                                                            //        dictotal.Add(d, Convert.ToDouble(paid_amt));
                                                            //    }
                                                            //    else
                                                            //    {
                                                            //        Double val = dictotal[d];
                                                            //        val = val + Convert.ToDouble(paid_amt);
                                                            //        dictotal[d] = val;
                                                            //    }
                                                            //    FpSpread1.Sheets[0].Cells[r, d].HorizontalAlign = HorizontalAlign.Right;
                                                            //    if (mode == "1")
                                                            //    {
                                                            //        FpSpread1.Sheets[0].Cells[r, d].BackColor = Color.LightCoral;
                                                            //        Cashtotal += Convert.ToDouble(paid_amt);
                                                            //    }

                                                            //    else if (mode == "2")
                                                            //    {
                                                            //        FpSpread1.Sheets[0].Cells[r, d].BackColor = Color.LightGray;
                                                            //        checktotal += Convert.ToDouble(paid_amt);
                                                            //    }

                                                            //    else if (mode == "3")
                                                            //    {
                                                            //        FpSpread1.Sheets[0].Cells[r, d].BackColor = Color.Orange;
                                                            //        ddtotal += Convert.ToDouble(paid_amt);
                                                            //    }

                                                            //    else if (mode == "4")
                                                            //    {
                                                            //        FpSpread1.Sheets[0].Cells[r, d].BackColor = Color.LightGreen;
                                                            //        Challantotal += Convert.ToDouble(paid_amt);
                                                            //    }

                                                            //    else if (mode == "5")
                                                            //    {
                                                            //        FpSpread1.Sheets[0].Cells[r, d].BackColor = Color.LightGoldenrodYellow;
                                                            //        onlinetotal += Convert.ToDouble(paid_amt);
                                                            //    }

                                                            //    if (paid_amt.Trim() == "")
                                                            //        paid_amt = "0";

                                                            //    total_paid = total_paid + Convert.ToDouble(paid_amt);
                                                            //    ledge_tot = ledge_tot + Convert.ToDouble(paid_amt);
                                                            //}
                                                            #endregion

                                                            //added by sudhagar 02/08/2015
                                                            if (paid_amt == "0" || paid_amt.Trim() == "")
                                                            {
                                                                finalflag = true;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Text = "-";
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].HorizontalAlign = HorizontalAlign.Center;
                                                            }
                                                            else
                                                            {
                                                                finalflag = true;
                                                                FpSpread1.Sheets[0].Cells[r, d].Text = paid_amt;
                                                                if (!dictotal.ContainsKey(d))
                                                                {
                                                                    dictotal.Add(d, Convert.ToDouble(paid_amt));
                                                                }
                                                                else
                                                                {
                                                                    Double val = dictotal[d];
                                                                    val = val + Convert.ToDouble(paid_amt);
                                                                    dictotal[d] = val;
                                                                }

                                                                string Linkvalue = "";
                                                                if (group_user.Contains(';'))
                                                                {
                                                                    string[] group_semi = group_user.Split(';');
                                                                    group_user = group_semi[0].ToString();

                                                                    Linkvalue = da.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + group_user + "' and college_code ='" + collegecode + "'");
                                                                }
                                                                else
                                                                {
                                                                    Linkvalue = da.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'");
                                                                }

                                                                if (Linkvalue == "0")
                                                                {
                                                                    if (diccurarrfee.ContainsKey(feecat + "$" + cursem))
                                                                    {
                                                                        Double getval = Convert.ToDouble(paid_amt) + Convert.ToDouble(diccurarrfee[feecat + "$" + cursem]);
                                                                        diccurarrfee[feecat + "$" + cursem] = getval;
                                                                    }
                                                                    else
                                                                    {
                                                                        diccurarrfee.Add(feecat + "$" + cursem, Convert.ToDouble(paid_amt));
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    string sem = "";
                                                                    double amt = 0;
                                                                    // sem = dvdatestud[fe]["Current_Semester"].ToString();
                                                                    string valuenew = returnYearforSem(sem);
                                                                    if (diccurarrfee.ContainsKey(feecater + "$" + valuenew.ToString()))
                                                                    {
                                                                        Double getval = Convert.ToDouble(paid_amt) + Convert.ToDouble(diccurarrfee[feecater + "$" + valuenew.ToString()]);
                                                                        diccurarrfee[feecater + "$" + valuenew.ToString()] = getval;
                                                                    }
                                                                    else
                                                                    {
                                                                        diccurarrfee.Add(feecater + "$" + valuenew.ToString(), Convert.ToDouble(paid_amt));
                                                                    }

                                                                }
                                                                //   if(>cursem)
                                                                finalflag = true;
                                                                // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].CellType = dobcell;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Text = paid_amt;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].HorizontalAlign = HorizontalAlign.Right;
                                                                if (mode == "1")
                                                                {
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].BackColor = Color.LightCoral;
                                                                    Cashtotal = Cashtotal + Convert.ToDouble(paid_amt);
                                                                }
                                                                else if (mode == "2")
                                                                {
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].BackColor = Color.LightGray;
                                                                    checktotal = checktotal + Convert.ToDouble(paid_amt);
                                                                }
                                                                else if (mode == "3")
                                                                {
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].BackColor = Color.Orange;
                                                                    // Challantotal = Challantotal + Convert.ToDouble(paid_amt);
                                                                    ddtotal = ddtotal + Convert.ToDouble(paid_amt);

                                                                }
                                                                else if (mode == "4")
                                                                {
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].BackColor = Color.LightGreen;
                                                                    Challantotal = Challantotal + Convert.ToDouble(paid_amt);
                                                                }
                                                                else if (mode == "5")
                                                                {
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].BackColor = Color.LightGoldenrodYellow;
                                                                    onlinetotal = onlinetotal + Convert.ToDouble(paid_amt);
                                                                }
                                                                else if (mode == "6")
                                                                {
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].BackColor = Color.White;
                                                                    cardtotal = cardtotal + Convert.ToDouble(paid_amt);
                                                                }
                                                                if (paid_amt.Trim() == "")
                                                                    paid_amt = "0";

                                                                total_paid = total_paid + Convert.ToDouble(paid_amt);
                                                                ledge_tot = ledge_tot + Convert.ToDouble(paid_amt);
                                                            }
                                                        }
                                                    }
                                                }
                                                if (ddlfeetype.Text == "Paid")
                                                {
                                                    FpSpread1.Sheets[0].Cells[r, FpSpread1.Sheets[0].Columns.Count - 1].Text = total_paid.ToString();
                                                    if (!dictotal.ContainsKey(FpSpread1.Sheets[0].Columns.Count - 1))
                                                    {
                                                        dictotal.Add(FpSpread1.Sheets[0].Columns.Count - 1, Convert.ToDouble(total_paid));
                                                    }
                                                    else
                                                    {
                                                        Double val = dictotal[FpSpread1.Sheets[0].Columns.Count - 1];
                                                        val = val + Convert.ToDouble(total_paid);
                                                        dictotal[FpSpread1.Sheets[0].Columns.Count - 1] = val;
                                                    }
                                                }

                                                FpSpread1.Sheets[0].Cells[r, FpSpread1.Sheets[0].Columns.Count - 1].HorizontalAlign = HorizontalAlign.Right;
                                                if (total_paid > 0 || total_due > 0)
                                                {
                                                    fl = 1;
                                                    sno++;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                                    degflag = true;
                                                }
                                                else
                                                {
                                                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Visible = false;
                                                }
                                            }
                                            if (i == ds3.Tables[0].Rows.Count - 1)
                                            {

                                                FpSpread1.Sheets[0].RowCount++;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Degree Wise Total";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 5);
                                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.LightCyan;

                                                if (degflag == false)
                                                {
                                                    FpSpread1.Sheets[0].Rows[degreerow].Visible = false;
                                                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Visible = false;
                                                }

                                                FpSpread1.Sheets[0].RowCount++;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 5);
                                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.LightSeaGreen;

                                                for (int d = 7; d < FpSpread1.Sheets[0].Columns.Count; d = d + 1)
                                                {
                                                    Double setva = 0;
                                                    if (dictotal.ContainsKey(d))
                                                    {
                                                        setva = dictotal[d];
                                                        dictotal[d] = 0;
                                                    }
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, d].Text = setva.ToString();

                                                    if (dicgrandtotal.ContainsKey(d))
                                                    {
                                                        Double val = dicgrandtotal[d];
                                                        val = val + setva;
                                                        dicgrandtotal[d] = val;
                                                    }
                                                    else
                                                    {
                                                        dicgrandtotal.Add(d, setva);
                                                    }

                                                    setva = 0;
                                                    if (dicgrandtotal.ContainsKey(d))
                                                    {
                                                        setva = dicgrandtotal[d];
                                                    }
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Text = setva.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, d].HorizontalAlign = HorizontalAlign.Right;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].HorizontalAlign = HorizontalAlign.Right;
                                                }
                                                dictotal.Clear();
                                            }
                                        }
                                        if (sclValue != 0)
                                        {
                                            FpSpread1.Sheets[0].RowCount++;// = FpSpread1.Sheets[0].RowCount + 2;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "ABSTRACT";
                                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.Brown;
                                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].ForeColor = Color.White;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                            // FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, FpSpread1.Sheets[0].ColumnCount);
                                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 7);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                                            for (int ps = 0; ps < chklstpaymode.Items.Count; ps++)
                                            {
                                                if (chklstpaymode.Items[ps].Selected == true)
                                                {
                                                    string strptype = chklstpaymode.Items[ps].Text.ToString();
                                                    FpSpread1.Sheets[0].RowCount++;
                                                    //   FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 7);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = strptype;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                                                    if (strptype == "Cash")
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Cashtotal.ToString();
                                                    }
                                                    else if (strptype == "Cheque")
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = checktotal.ToString();
                                                    }
                                                    else if (strptype == "DD")
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = ddtotal.ToString();
                                                    }
                                                    else if (strptype == "Challan")
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Challantotal.ToString();
                                                    }
                                                    else if (strptype == "Online Pay")
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = onlinetotal.ToString();
                                                    }
                                                    else if (strptype == "Card")
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = cardtotal.ToString();
                                                    }
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                                    // FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, col, 1, FpSpread1.Sheets[0].ColumnCount-1);
                                                }
                                            }
                                            FpSpread1.Sheets[0].RowCount++;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
                                            //   FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 7);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                                            Double dotvd = Cashtotal + checktotal + ddtotal + Challantotal + onlinetotal;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = dotvd.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                            //   FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, col, 1, FpSpread1.Sheets[0].ColumnCount - 1);
                                            Double currfee = 0;
                                            Double arrfee = 0;
                                            foreach (var parameter in diccurarrfee)
                                            {
                                                string getsplallow = parameter.Key.ToString();
                                                string getfeeamount = parameter.Value.ToString();
                                                string[] spt = getsplallow.Split('$');
                                                if (spt.GetUpperBound(0) == 1)
                                                {
                                                    for (int rcs = 0; rcs < chklsfeesem.Items.Count; rcs++)
                                                    {
                                                        if (chklsfeesem.Items[rcs].Value.ToString() == spt[0].ToString())
                                                        {
                                                            string feeval = chklsfeesem.Items[rcs].Text.ToString();
                                                            string[] stc = feeval.Split(' ');
                                                            if (stc.GetUpperBound(0) >= 0)
                                                            {
                                                                if (stc[0].ToString().Trim() == spt[1].ToString().Trim())
                                                                {
                                                                    currfee = currfee + Convert.ToDouble(getfeeamount);
                                                                }
                                                                else
                                                                {
                                                                    arrfee = arrfee + Convert.ToDouble(getfeeamount);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (chklsfeesem.Items[rcs].Text.Contains(spt[1].ToString()))
                                                                {
                                                                    currfee = currfee + Convert.ToDouble(getfeeamount);
                                                                }
                                                                else
                                                                {
                                                                    arrfee = arrfee + Convert.ToDouble(getfeeamount);
                                                                }
                                                            }
                                                            rcs = chklsfeesem.Items.Count;
                                                        }
                                                    }
                                                }
                                            }
                                            FpSpread1.Sheets[0].RowCount++;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "REGULAR";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                                            // FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 7);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = currfee.ToString();
                                            //  FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, col, 1, FpSpread1.Sheets[0].ColumnCount - 1);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                            FpSpread1.Sheets[0].RowCount++;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "ARREAR";
                                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.YellowGreen;
                                            //  FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 7);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = arrfee.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                            //  FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, col, 1, FpSpread1.Sheets[0].ColumnCount - 1); 
                                        }
                                        if (fl == 0)
                                        {
                                            errmsg.Visible = true;
                                            errmsg.Text = "No Records Found";
                                            FpSpread1.Visible = false;
                                            btnprintmaster.Visible = false;
                                            lblrptname.Visible = false;
                                            txtexcelname.Visible = false;
                                            btnxl.Visible = false;
                                            lblc1.Visible = false;
                                            lblc2.Visible = false;
                                            lblc3.Visible = false;
                                            lblc4.Visible = false;
                                            lblc5.Visible = false;
                                            lblcard.Visible = false;
                                        }
                                    }
                                    else
                                    {
                                        errmsg.Visible = true;
                                        errmsg.Text = "No Records Found";
                                        FpSpread1.Visible = false;
                                        btnprintmaster.Visible = false;
                                        lblrptname.Visible = false;
                                        txtexcelname.Visible = false;
                                        btnxl.Visible = false;
                                        lblc1.Visible = false;
                                        lblc2.Visible = false;
                                        lblc3.Visible = false;
                                        lblc4.Visible = false;
                                        lblc5.Visible = false;
                                        lblcard.Visible = false;
                                    }
                                }
                                #endregion

                            }
                        }

                        #endregion


                        #region Without date && with date Yet to be paid Header And Ledger query and value

                        if (ddlfeetype.SelectedItem.Text == "Yet To Be Paid")
                        {
                            #region include distcont
                            string cc = "";
                            string debar = "";
                            string disc = "";
                            string commondist = "";
                            if (cblinclude.Items.Count > 0)
                            {
                                for (int i = 0; i < cblinclude.Items.Count; i++)
                                {
                                    if (cblinclude.Items[i].Selected == true)
                                    {
                                        if (cblinclude.Items[i].Value == "1")
                                        {
                                            cc = " r.cc=1";
                                        }
                                        if (cblinclude.Items[i].Value == "2")
                                        {
                                            debar = "  r.Exam_Flag like '%debar'";
                                        }
                                        if (cblinclude.Items[i].Value == "3")
                                        {
                                            disc = " r.DelFlag=1";
                                        }
                                    }
                                }
                            }
                            if (cc != "" && debar == "" && disc == "")
                                commondist = " and (" + cc + " or r.cc=0)  and r.Exam_Flag<>'debar' and r.DelFlag=0";

                            if (cc == "" && debar != "" && disc == "")
                                commondist = " and r.cc=0  and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";

                            if (cc == "" && debar == "" && disc != "")
                                commondist = " and r.cc=0  and r.Exam_Flag<>'debar' and (" + disc + " or r.DelFlag=0)";

                            if (cc != "" && debar != "" && disc == "")
                                commondist = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";

                            if (cc == "" && debar != "" && disc != "")
                                commondist = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar')  and (" + disc + " or r.DelFlag=0)";

                            if (cc != "" && debar == "" && disc != "")
                                commondist = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar'  and (" + disc + " or r.DelFlag=0)";

                            else if (cc == "" && debar == "" && disc == "")
                                commondist = " and r.cc=0  and r.Exam_Flag<>'debar' and r.DelFlag=0";

                            if (cc != "" && debar != "" && disc != "")
                                commondist = "";

                            #endregion
                            if (ddlacctype.SelectedItem.Text != "Group Header")//Header
                            {
                                //header and ledger
                                if (chkyettobepaid.Checked == false)
                                {
                                    #region without date

                                    string commonid = "";
                                    string headerid = "";
                                    string header = "";
                                    string ledger = "";
                                    string ledgerid = "";
                                    string hedgeandledge = "";
                                    string trdate = "";

                                    if (ddlacctype.SelectedItem.Text == "Header")
                                    {
                                        headerid = ",f.HeaderFK";
                                        header = " and f.HeaderFK in('" + header_all + "')";
                                        commonid = "HeaderFK";
                                    }
                                    else if (ddlacctype.SelectedItem.Text == "Ledger")
                                    {
                                        ledgerid = ",f.LedgerFK";
                                        ledger = " and f.ledgerFK in('" + getfeeval + "')";
                                        hedgeandledge = " and f.HeaderFK in('" + headervalue + "')";
                                        commonid = "LedgerFK";
                                    }
                                    #region new
                                    string typevalue = "";
                                    if (ddltype.Enabled == true)
                                    {
                                        typevalue = Convert.ToString(ddltype.SelectedItem.Text);
                                    }

                                    #region Query

                                    string Query = " select  f.App_No,sum (TotalAmount),SUM( PaidAmount),sum(BalAmount)as Balance,FeeCategory" + headerid + "" + ledgerid + " from FT_FeeAllot f,Registration r ,Degree d,Department dt,Course c where f.App_No =r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and ISNULL (BalAmount ,TotalAmount)<>0  and d.college_code ='" + collegecode + "'";

                                    if (commondist != "")
                                        Query = Query + commondist;

                                    if (typevalue.Trim() != "Both" && typevalue.Trim() != "")
                                    {
                                        Query = Query + " and type ='" + typevalue + "'";
                                    }
                                    if (batch_all.Trim() != "")
                                    {
                                        Query = Query + " and r.batch_year  in (" + batch_all + ")";
                                    }
                                    if (course_all.Trim() != "")
                                    {
                                        Query = Query + " and r.degree_code  in (" + course_all + ")";
                                    }
                                    if (stefeesem.Trim() != "")
                                    {
                                        Query = Query + " and f.FeeCategory  in (" + stefeesem + ")";
                                    }
                                    if (finyearval.Trim() != "")
                                    {
                                        Query = Query + " and f.FinYearFK   in (" + Newfinayear + ")";
                                    }
                                    if (header.Trim() != "")
                                    {
                                        Query = Query + header;
                                    }
                                    if (hedgeandledge.Trim() != "")
                                    {
                                        Query = Query + hedgeandledge;
                                    }
                                    if (ledger.Trim() != "")
                                    {
                                        Query = Query + ledger;
                                    }
                                    if (trdate != "")
                                    {
                                        Query += trdate;
                                    }
                                    Query = Query + " group by f.App_No,FeeCategory" + headerid + " " + ledgerid + " Order by f.app_no";
                                    Query = Query + " select distinct f.App_No ,r.degree_code,r.Batch_Year  from FT_FeeAllot f,Registration r ,Degree d,Department dt,Course c where  f.App_No =r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and ISNULL ( BalAmount ,TotalAmount)<>0   and d.college_code ='" + collegecode + "'";
                                    if (commondist != "")
                                        Query = Query + commondist;

                                    if (typevalue.Trim() != "Both" && typevalue.Trim() != "")
                                    {
                                        Query = Query + " and type ='" + typevalue + "'";
                                    }
                                    if (batch_all.Trim() != "")
                                    {
                                        Query = Query + " and r.batch_year  in (" + batch_all + ")";
                                    }
                                    if (course_all.Trim() != "")
                                    {
                                        Query = Query + " and r.degree_code  in (" + course_all + ")";
                                    }
                                    if (stefeesem.Trim() != "")
                                    {
                                        Query = Query + " and f.FeeCategory  in (" + stefeesem + ")";
                                    }
                                    if (header.Trim() != "")
                                    {
                                        Query = Query + header;
                                    }
                                    if (hedgeandledge.Trim() != "")
                                    {
                                        Query = Query + hedgeandledge;
                                    }
                                    if (ledger.Trim() != "")
                                    {
                                        Query = Query + ledger;
                                    }
                                    if (finyearval.Trim() != "")
                                    {
                                        Query = Query + " and f.FinYearFK   in (" + Newfinayear + ")";
                                    }
                                    if (trdate != "")
                                    {
                                        Query += trdate;
                                    }
                                    if (chkfeeroll.Checked == true)
                                    {
                                        Query = Query + " and r.Roll_No not in(select s.roll_no from stucon s where s.roll_no=r.roll_no and s.semester=r.Current_Semester and s.ack_fee_of_roll=1) order by r.Batch_Year,r.degree_code,f.App_No";
                                    }
                                    Query = Query + " select distinct f.App_No,r.Roll_No,roll_admit,r.Reg_No,r.Stud_Name,(C.Course_Name +'-'+dt.Dept_Name) as Degree,FeeCategory,r.degree_code,T.TextVal from FT_FeeAllot f,Registration r ,Degree d,Department dt,Course c , TextValTable T  where T.TextCode =F.FeeCategory  and f.App_No =r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and ISNULL ( BalAmount ,TotalAmount)<>0   and d.college_code ='" + collegecode + "'";
                                    if (commondist != "")
                                        Query = Query + commondist;

                                    if (typevalue.Trim() != "Both" && typevalue.Trim() != "")
                                    {
                                        Query = Query + " and type ='" + typevalue + "'";
                                    }
                                    if (batch_all.Trim() != "")
                                    {
                                        Query = Query + " and r.batch_year  in (" + batch_all + ")";
                                    }
                                    if (course_all.Trim() != "")
                                    {
                                        Query = Query + " and r.degree_code  in (" + course_all + ")";
                                    }
                                    if (stefeesem.Trim() != "")
                                    {
                                        Query = Query + " and f.FeeCategory  in (" + stefeesem + ")";
                                    }
                                    if (finyearval.Trim() != "")
                                    {
                                        Query = Query + " and f.FinYearFK   in (" + Newfinayear + ")";
                                    }
                                    if (header.Trim() != "")
                                    {
                                        Query = Query + header;
                                    }
                                    if (hedgeandledge.Trim() != "")
                                    {
                                        Query = Query + hedgeandledge;
                                    }
                                    if (trdate != "")
                                    {
                                        Query += trdate;
                                    }
                                    if (ledger.Trim() != "")
                                    {
                                        Query = Query + ledger;
                                    }
                                    Query += " order by r.degree_code";
                                    #endregion

                                    #region value bind
                                    ds.Clear();
                                    ds = da.select_method_wo_parameter(Query, "Text");
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        DataView dv = new DataView();
                                        DataView dv1 = new DataView();
                                        DataView dv2 = new DataView();
                                        finalflag = true;
                                        Hashtable GrandTotal = new Hashtable();
                                        if (chklstbatch.Items.Count > 0)
                                        {
                                            for (int batch = 0; batch < chklstbatch.Items.Count; batch++)
                                            {
                                                if (chklstbatch.Items[batch].Selected == true)
                                                {
                                                    for (int Bnch = 0; Bnch < chklstbranch.Items.Count; Bnch++)
                                                    {
                                                        if (chklstbranch.Items[Bnch].Selected == true)
                                                        {
                                                            //for (int batch = 0; batch < chklstbatch.Items.Count; batch++)
                                                            //{
                                                            //    if (chklstbatch.Items[batch].Selected == true)
                                                            //    {
                                                            ds.Tables[1].DefaultView.RowFilter = "degree_code='" + chklstbranch.Items[Bnch].Value + "' and Batch_Year='" + Convert.ToString(chklstbatch.Items[batch].Text) + "'";
                                                            dv2 = ds.Tables[1].DefaultView;
                                                            if (dv2.Count > 0)
                                                            {
                                                                FpSpread1.Sheets[0].RowCount++;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(chklstbranch.Items[Bnch].Text) + " - " + Convert.ToString(chklstbatch.Items[batch].Text);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.LightGreen;
                                                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, FpSpread1.Sheets[0].ColumnCount - 1);
                                                                for (int row = 0; row < dv2.Count; row++)
                                                                {
                                                                    string app_no = Convert.ToString(dv2[row]["App_No"]);
                                                                    ds.Tables[2].DefaultView.RowFilter = "App_No=" + Convert.ToString(app_no) + " and degree_code='" + chklstbranch.Items[Bnch].Value + "'";
                                                                    dv = ds.Tables[2].DefaultView;
                                                                    for (int drow = 0; drow < dv.Count; drow++)
                                                                    {
                                                                        sno++;
                                                                        string dvapp_no = Convert.ToString(dv[drow]["App_No"]);
                                                                        string Feecategory = Convert.ToString(dv[drow]["FeeCategory"]);
                                                                        FpSpread1.Sheets[0].RowCount++;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dv[drow]["Stud_Name"]);
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dv[drow]["Roll_No"]);
                                                                        if (sclflag == false)
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv[drow]["Reg_No"]);
                                                                        else
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv[drow]["roll_admit"]);
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = txtreg;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dv[drow]["Degree"]);
                                                                        if (feecateflag == true)
                                                                        {
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dv[drow]["TextVal"]);
                                                                        }
                                                                        else
                                                                        {
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dv[drow]["TextVal"]);
                                                                        }
                                                                        int col = 6;
                                                                        double Total = 0;
                                                                        double balance = 0;
                                                                        double Allot = 0;
                                                                        double paid = 0;
                                                                        #region Header
                                                                        if (ddlacctype.SelectedItem.Text == "Header")
                                                                        {
                                                                            for (int j = 0; j < chklstaccheader.Items.Count; j++)
                                                                            {
                                                                                if (chklstaccheader.Items[j].Selected == true)
                                                                                {
                                                                                    ds.Tables[0].DefaultView.RowFilter = "App_no='" + dvapp_no + "' and FeeCategory='" + Feecategory + "' and HeaderFK='" + Convert.ToString(chklstaccheader.Items[j].Value) + "'";
                                                                                    dv1 = ds.Tables[0].DefaultView;
                                                                                    col++;
                                                                                    if (dv1.Count > 0)
                                                                                    {
                                                                                        double.TryParse(Convert.ToString(dv1[0]["Balance"]), out balance);
                                                                                        Total += balance;
                                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(balance);
                                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                                                                        if (!GrandTotal.ContainsKey(Convert.ToString(chklstaccheader.Items[j].Text)))
                                                                                            GrandTotal.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(balance));
                                                                                        else
                                                                                        {
                                                                                            double total = 0;
                                                                                            double.TryParse(Convert.ToString(GrandTotal[Convert.ToString(chklstaccheader.Items[j].Text)]), out total);
                                                                                            total += balance;
                                                                                            GrandTotal.Remove(Convert.ToString(chklstaccheader.Items[j].Text));
                                                                                            GrandTotal.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(total));
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = "-";
                                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                                                                    }

                                                                                }
                                                                            }
                                                                        }
                                                                        #endregion

                                                                        #region Ledger
                                                                        if (ddlacctype.SelectedItem.Text == "Ledger")
                                                                        {
                                                                            for (int remv = 0; remv < treeview_spreadfields.Nodes.Count; remv++)
                                                                            {
                                                                                if (treeview_spreadfields.Nodes[remv].ChildNodes.Count > 0)
                                                                                {
                                                                                    for (int j = 0; j < treeview_spreadfields.Nodes[remv].ChildNodes.Count; j++)
                                                                                    {
                                                                                        if (treeview_spreadfields.Nodes[remv].ChildNodes[j].Checked == true)
                                                                                        {
                                                                                            ds.Tables[0].DefaultView.RowFilter = "App_no='" + dvapp_no + "' and FeeCategory='" + Feecategory + "' and LedgerFK='" + Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Value) + "'";
                                                                                            dv1 = ds.Tables[0].DefaultView;
                                                                                            col++;
                                                                                            if (dv1.Count > 0)
                                                                                            {
                                                                                                double.TryParse(Convert.ToString(dv1[0]["Balance"]), out balance);
                                                                                                double.TryParse(Convert.ToString(dv1[0][1]), out Allot);
                                                                                                double.TryParse(Convert.ToString(dv1[0][2]), out paid);

                                                                                                if (balance != 0)
                                                                                                {
                                                                                                    Total += balance;
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    balance = Allot - paid;
                                                                                                    Total += balance;
                                                                                                }
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(balance);
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                                                                                if (!GrandTotal.ContainsKey(Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text)))
                                                                                                    GrandTotal.Add(Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text), Convert.ToString(balance));
                                                                                                else
                                                                                                {
                                                                                                    double total = 0;
                                                                                                    double.TryParse(Convert.ToString(GrandTotal[Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text)]), out total);
                                                                                                    total += balance;
                                                                                                    GrandTotal.Remove(Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text));
                                                                                                    GrandTotal.Add(Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text), Convert.ToString(total));
                                                                                                }
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = "-";
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                                                                            }

                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                        #endregion

                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(Total);
                                                                        //Total += Convert.ToDouble(dv1[0]["Balance"]);
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                                                        if (!GrandTotal.ContainsKey(Convert.ToString("Total")))
                                                                            GrandTotal.Add(Convert.ToString("Total"), Convert.ToString(Total));
                                                                        else
                                                                        {
                                                                            double total = 0;
                                                                            double.TryParse(Convert.ToString(GrandTotal[Convert.ToString("Total")]), out total);
                                                                            total += Total;
                                                                            GrandTotal.Remove(Convert.ToString("Total"));
                                                                            GrandTotal.Add(Convert.ToString("Total"), Convert.ToString(total));
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            FpSpread1.Sheets[0].RowCount++;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString("Grand Total");
                                            int col1 = 6;
                                            if (ddlacctype.SelectedItem.Text == "Header")
                                            {
                                                for (int j = 0; j < chklstaccheader.Items.Count; j++)
                                                {
                                                    if (chklstaccheader.Items[j].Selected == true)
                                                    {
                                                        col1++;
                                                        string GetNewValue = Convert.ToString(GrandTotal[Convert.ToString(chklstaccheader.Items[j].Text)]);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].Text = Convert.ToString(GetNewValue);
                                                        //Total += Convert.ToDouble(dv1[0]["Balance"]);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].HorizontalAlign = HorizontalAlign.Right;
                                                    }
                                                }
                                                col1++;
                                                string GetNewValue1 = Convert.ToString(GrandTotal[Convert.ToString("Total")]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].Text = Convert.ToString(GetNewValue1);
                                                //Total += Convert.ToDouble(dv1[0]["Balance"]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].HorizontalAlign = HorizontalAlign.Right;
                                            }
                                            else
                                            {
                                                for (int remv = 0; remv < treeview_spreadfields.Nodes.Count; remv++)
                                                {
                                                    if (treeview_spreadfields.Nodes[remv].ChildNodes.Count > 0)
                                                    {
                                                        for (int j = 0; j < treeview_spreadfields.Nodes[remv].ChildNodes.Count; j++)
                                                        {
                                                            if (treeview_spreadfields.Nodes[remv].ChildNodes[j].Checked == true)
                                                            {
                                                                col1++;
                                                                string GetNewValue = Convert.ToString(GrandTotal[Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text)]);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].Text = Convert.ToString(GetNewValue);
                                                                //Total += Convert.ToDouble(dv1[0]["Balance"]);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].HorizontalAlign = HorizontalAlign.Right;
                                                            }
                                                        }
                                                    }
                                                }
                                                string GetNewValue1 = Convert.ToString(GrandTotal[Convert.ToString("Total")]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(GetNewValue1);
                                                //Total += Convert.ToDouble(dv1[0]["Balance"]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;

                                            }
                                        }
                                        if (FpSpread1.Sheets[0].RowCount > 0)
                                        {
                                            FpSpread1.Visible = true;
                                            btnprintmaster.Visible = true;
                                            lblrptname.Visible = true;
                                            txtexcelname.Visible = true;
                                            btnxl.Visible = true;
                                        }
                                        else
                                        {
                                            FpSpread1.Visible = false;
                                            btnprintmaster.Visible = false;
                                            lblrptname.Visible = false;
                                            txtexcelname.Visible = false;
                                            btnxl.Visible = false;
                                        }
                                    }
                                    #endregion

                                    #endregion

                                    #endregion
                                }
                                else
                                {
                                    #region with date

                                    #region query

                                    string commonid = "";
                                    string headerid = "";
                                    string header = "";
                                    string ledger = "";
                                    string ledgerid = "";
                                    string hedgeandledge = "";

                                    if (ddlacctype.SelectedItem.Text == "Header")
                                    {
                                        headerid = ",f.HeaderFK";
                                        header = " and f.HeaderFK in('" + header_all + "')";
                                        commonid = "HeaderFK";
                                    }
                                    else if (ddlacctype.SelectedItem.Text == "Ledger")
                                    {
                                        ledgerid = ",f.LedgerFK";
                                        ledger = " and f.ledgerFK in('" + getfeeval + "')";
                                        hedgeandledge = " and f.HeaderFK in('" + headervalue + "')";
                                        commonid = "LedgerFK";
                                    }


                                    //demand amount
                                    string Query = " select SUM(TotalAmount) as Demand, f.App_No,r.degree_code,r.Batch_Year  from FT_FeeAllot f,Registration r where f.App_No =r.App_No ";
                                    if (commondist != "")
                                        Query = Query + commondist;

                                    if (batch_all.Trim() != "")
                                        Query = Query + " and r.batch_year  in (" + batch_all + ")";

                                    if (course_all.Trim() != "")
                                        Query = Query + " and r.degree_code  in (" + course_all + ")";

                                    if (stefeesem.Trim() != "")
                                        Query = Query + " and f.FeeCategory  in (" + stefeesem + ")";

                                    if (finyearval.Trim() != "")
                                        Query = Query + " and f.FinYearFK   in (" + Newfinayear + ")";

                                    if (header.Trim() != "")
                                        Query = Query + header;

                                    if (hedgeandledge.Trim() != "")
                                        Query = Query + hedgeandledge;

                                    if (ledger.Trim() != "")
                                        Query = Query + ledger;

                                    Query += "  group by f.App_No,r.degree_code,r.Batch_Year";

                                    //header and ledger
                                    Query += " select SUM(TotalAmount) as Demand, f.App_No,f.FeeCategory,r.degree_code,r.Batch_Year" + headerid + "" + ledgerid + "  from FT_FeeAllot f,Registration r where f.App_No =r.App_No ";
                                    if (commondist != "")
                                        Query = Query + commondist;

                                    if (batch_all.Trim() != "")
                                        Query = Query + " and r.batch_year  in (" + batch_all + ")";

                                    if (course_all.Trim() != "")
                                        Query = Query + " and r.degree_code  in (" + course_all + ")";

                                    if (stefeesem.Trim() != "")
                                        Query = Query + " and f.FeeCategory  in (" + stefeesem + ")";

                                    if (finyearval.Trim() != "")
                                        Query = Query + " and f.FinYearFK   in (" + Newfinayear + ")";

                                    if (header.Trim() != "")
                                        Query = Query + header;

                                    if (hedgeandledge.Trim() != "")
                                        Query = Query + hedgeandledge;

                                    if (ledger.Trim() != "")
                                        Query = Query + ledger;

                                    Query += "  group by f.App_No,f.FeeCategory,r.degree_code,r.Batch_Year" + headerid + "" + ledgerid + "";

                                    //paid amount
                                    Query += " select SUM(TotalAmount) as Demand, f.App_No,r.degree_code,r.Batch_Year,r.Stud_Name,r.Roll_No,r.roll_admit,r.Reg_No,f.FeeCategory from FT_FeeAllot f,Registration r where f.App_No =r.App_No ";
                                    if (commondist != "")
                                        Query = Query + commondist;

                                    if (batch_all.Trim() != "")
                                        Query = Query + " and r.batch_year  in (" + batch_all + ")";

                                    if (course_all.Trim() != "")
                                        Query = Query + " and r.degree_code  in (" + course_all + ")";

                                    if (stefeesem.Trim() != "")
                                        Query = Query + " and f.FeeCategory  in (" + stefeesem + ")";

                                    if (finyearval.Trim() != "")
                                        Query = Query + " and f.FinYearFK   in (" + Newfinayear + ")";

                                    if (header.Trim() != "")
                                        Query = Query + header;

                                    if (hedgeandledge.Trim() != "")
                                        Query = Query + hedgeandledge;

                                    if (ledger.Trim() != "")
                                        Query = Query + ledger;

                                    Query += "  group by f.App_No,r.degree_code,r.Batch_Year,r.Stud_Name,r.Roll_No,r.roll_admit,r.Reg_No,f.FeeCategory order by f.App_No,f.FeeCategory asc";

                                    //Query += " select SUM(Debit) as Paid, f.App_No,f.FeeCategory,r.degree_code,r.Batch_Year,r.Stud_Name,r.Roll_No,r.Reg_No from FT_FinDailyTransaction f,Registration r where f.App_No =r.App_No";
                                    //if (commondist != "")
                                    //    Query = Query + commondist;

                                    //if (batch_all.Trim() != "")
                                    //    Query = Query + " and r.batch_year  in (" + batch_all + ")";

                                    //if (course_all.Trim() != "")
                                    //    Query = Query + " and r.degree_code  in (" + course_all + ")";

                                    //if (stefeesem.Trim() != "")
                                    //    Query = Query + " and f.FeeCategory  in (" + stefeesem + ")";

                                    //if (finyearval.Trim() != "")
                                    //    Query = Query + " and f.FinYearFK   in (" + Newfinayear + ")";

                                    //if (header.Trim() != "")
                                    //    Query = Query + header;

                                    //if (hedgeandledge.Trim() != "")
                                    //    Query = Query + hedgeandledge;

                                    //if (ledger.Trim() != "")
                                    //    Query = Query + ledger;
                                    //Query += " and TransDate <='" + dat1.ToString("MM/dd/yyyy") + "'";
                                    //Query += " group by f.App_No,f.FeeCategory,r.degree_code,r.Batch_Year,r.Stud_Name,r.Roll_No,r.Reg_No order by f.App_No,f.FeeCategory asc";

                                    //paid amt with headerfk or ledgerfk
                                    Query += " select SUM(Debit) as Paid, f.App_No,f.FeeCategory,r.degree_code,r.Batch_Year,r.Stud_Name,r.Roll_No,r.roll_admit,r.Reg_No" + headerid + "" + ledgerid + "  from FT_FinDailyTransaction f,Registration r where f.App_No =r.App_No";
                                    if (usBasedRights == true)
                                        Query += " and f.EntryUserCode in('" + usercode + "')";
                                    if (commondist != "")
                                        Query = Query + commondist;

                                    if (batch_all.Trim() != "")
                                        Query = Query + " and r.batch_year  in (" + batch_all + ")";

                                    if (course_all.Trim() != "")
                                        Query = Query + " and r.degree_code  in (" + course_all + ")";

                                    if (stefeesem.Trim() != "")
                                        Query = Query + " and f.FeeCategory  in (" + stefeesem + ")";

                                    if (finyearval.Trim() != "")
                                        Query = Query + " and f.FinYearFK   in (" + Newfinayear + ")";

                                    if (header.Trim() != "")
                                        Query = Query + header;

                                    if (hedgeandledge.Trim() != "")
                                        Query = Query + hedgeandledge;

                                    if (ledger.Trim() != "")
                                        Query = Query + ledger;
                                    Query += " and TransDate <='" + dat1.ToString("MM/dd/yyyy") + "' and ISNULL(IsCanceled,'0')='0'";
                                    Query += " group by f.App_No,f.FeeCategory,r.degree_code,r.Batch_Year,r.Stud_Name,r.Roll_No,r.roll_admit,r.Reg_No" + headerid + "" + ledgerid + "";

                                    Query = Query + " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code ='" + collegecode + "'";
                                    Query = Query + " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode + "'";
                                    #region old
                                    //degree dept
                                    //Query += " select r.degree_code,r.Batch_Year, f.App_No  from FT_FeeAllot f,Registration r where f.App_No =r.App_No ";
                                    //if (commondist != "")
                                    //    Query = Query + commondist;

                                    //if (batch_all.Trim() != "")
                                    //    Query = Query + " and r.batch_year  in (" + batch_all + ")";

                                    //if (course_all.Trim() != "")
                                    //    Query = Query + " and r.degree_code  in (" + course_all + ")";

                                    //if (stefeesem.Trim() != "")
                                    //    Query = Query + " and f.FeeCategory  in (" + stefeesem + ")";

                                    //if (finyearval.Trim() != "")
                                    //    Query = Query + " and f.FinYearFK   in (" + Newfinayear + ")";

                                    //if (header.Trim() != "")
                                    //    Query = Query + header;

                                    //if (hedgeandledge.Trim() != "")
                                    //    Query = Query + hedgeandledge;

                                    //if (ledger.Trim() != "")
                                    //    Query = Query + ledger;

                                    //Query += "  group by f.App_No,f.FeeCategory";
                                    #endregion

                                    #endregion

                                    ds.Clear();
                                    ds = da.select_method_wo_parameter(Query, "Text");
                                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                    {
                                        #region value

                                        DataView dvstddt = new DataView();
                                        DataView paiddt = new DataView();
                                        DataView dvpaid = new DataView();
                                        DataView Dview = new DataView();
                                        DataView dvallot = new DataView();
                                        Hashtable GrandTotal = new Hashtable();
                                        if (chklstbatch.Items.Count > 0)
                                        {
                                            for (int batch = 0; batch < chklstbatch.Items.Count; batch++)
                                            {
                                                if (chklstbatch.Items[batch].Selected == true)
                                                {
                                                    for (int Bnch = 0; Bnch < chklstbranch.Items.Count; Bnch++)
                                                    {
                                                        if (chklstbranch.Items[Bnch].Selected == true)
                                                        {
                                                            //for (int batch = 0; batch < chklstbatch.Items.Count; batch++)
                                                            //{
                                                            //    if (chklstbatch.Items[batch].Selected == true)
                                                            //    {
                                                            ds.Tables[0].DefaultView.RowFilter = "degree_code='" + chklstbranch.Items[Bnch].Value + "' and Batch_Year='" + Convert.ToString(chklstbatch.Items[batch].Text) + "'";
                                                            dvstddt = ds.Tables[0].DefaultView;
                                                            if (dvstddt.Count > 0)
                                                            {
                                                                FpSpread1.Sheets[0].RowCount++;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(chklstbranch.Items[Bnch].Text) + " - " + Convert.ToString(chklstbatch.Items[batch].Text);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.LightGreen;
                                                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, FpSpread1.Sheets[0].ColumnCount - 1);

                                                                for (int row = 0; row < dvstddt.Count; row++)
                                                                {

                                                                    string Appno = Convert.ToString(dvstddt[row]["App_No"]);
                                                                    string degCode = Convert.ToString(dvstddt[row]["degree_code"]);
                                                                    //  double.TryParse(Convert.ToString(dvstddt[row]["Demand"]), out demandAmt);
                                                                    ds.Tables[2].DefaultView.RowFilter = "App_No=" + Appno + " and degree_code='" + degCode + "'";
                                                                    paiddt = ds.Tables[2].DefaultView;
                                                                    if (paiddt.Count > 0)
                                                                    {
                                                                        for (int drow = 0; drow < paiddt.Count; drow++)
                                                                        {

                                                                            string dvapp_no = Convert.ToString(paiddt[drow]["App_No"]);
                                                                            string Feecategory = Convert.ToString(paiddt[drow]["FeeCategory"]);
                                                                            FpSpread1.Sheets[0].RowCount++;

                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(paiddt[drow]["Stud_Name"]);
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(paiddt[drow]["Roll_No"]);
                                                                            if (sclflag == false)
                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(paiddt[drow]["Reg_No"]);
                                                                            else
                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(paiddt[drow]["roll_admit"]);
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = txtreg;
                                                                            string Degreename = "";
                                                                            if (ds.Tables[5].Rows.Count > 0)
                                                                            {
                                                                                ds.Tables[5].DefaultView.RowFilter = "Degree_code='" + Convert.ToString(paiddt[drow]["Degree_code"]) + "'";
                                                                                Dview = ds.Tables[5].DefaultView;
                                                                                if (Dview.Count > 0)
                                                                                {
                                                                                    Degreename = Convert.ToString(Dview[0]["degreename"]);
                                                                                }
                                                                            }
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Degreename;
                                                                            string TextName = "";
                                                                            if (ds.Tables[4].Rows.Count > 0)
                                                                            {
                                                                                ds.Tables[4].DefaultView.RowFilter = "TextCode='" + Convert.ToString(paiddt[drow]["FeeCategory"]) + "'";
                                                                                Dview = ds.Tables[4].DefaultView;
                                                                                if (Dview.Count > 0)
                                                                                    TextName = Convert.ToString(Dview[0]["TextVal"]);
                                                                            }

                                                                            if (feecateflag == true)
                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = TextName;
                                                                            else
                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = TextName;

                                                                            int col = 6;
                                                                            double Total = 0;
                                                                            double BalAmt = 0;
                                                                            if (dvapp_no == "108528")
                                                                            {
                                                                            }

                                                                            #region Header
                                                                            int HeaderCount = 0;
                                                                            if (ddlacctype.SelectedItem.Text == "Header")
                                                                            {
                                                                                int colcnt = 0;
                                                                                bool check = false;
                                                                                for (int j = 0; j < chklstaccheader.Items.Count; j++)
                                                                                {
                                                                                    double demandAmt = 0;
                                                                                    double PaidAmt = 0;
                                                                                    if (chklstaccheader.Items[j].Selected == true)
                                                                                    {
                                                                                        //allot totalamount
                                                                                        check = true;
                                                                                        HeaderCount++;
                                                                                        ds.Tables[1].DefaultView.RowFilter = "App_no='" + dvapp_no + "' and FeeCategory='" + Feecategory + "' and HeaderFK='" + Convert.ToString(chklstaccheader.Items[j].Value) + "'";
                                                                                        dvallot = ds.Tables[1].DefaultView;
                                                                                        if (dvallot.Count > 0)
                                                                                            double.TryParse(Convert.ToString(Convert.ToString(dvallot[0]["Demand"])), out demandAmt);
                                                                                        //paid
                                                                                        ds.Tables[3].DefaultView.RowFilter = "App_no='" + dvapp_no + "' and FeeCategory='" + Feecategory + "' and HeaderFK='" + Convert.ToString(chklstaccheader.Items[j].Value) + "'";
                                                                                        dvpaid = ds.Tables[3].DefaultView;
                                                                                        col++;
                                                                                        if (dvpaid.Count > 0)
                                                                                            double.TryParse(Convert.ToString(Convert.ToString(dvpaid[0]["Paid"])), out PaidAmt);

                                                                                        if (demandAmt == PaidAmt)
                                                                                        {
                                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = "-";
                                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                                                                            colcnt++;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            BalAmt = demandAmt - PaidAmt;
                                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(BalAmt);
                                                                                            Total += BalAmt;
                                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                                                                        }

                                                                                        if (!GrandTotal.ContainsKey(Convert.ToString(chklstaccheader.Items[j].Text)))
                                                                                            GrandTotal.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(BalAmt));
                                                                                        else
                                                                                        {
                                                                                            double total = 0;
                                                                                            double.TryParse(Convert.ToString(GrandTotal[Convert.ToString(chklstaccheader.Items[j].Text)]), out total);
                                                                                            total += BalAmt;
                                                                                            GrandTotal.Remove(Convert.ToString(chklstaccheader.Items[j].Text));
                                                                                            GrandTotal.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(total));
                                                                                            BalAmt = 0;
                                                                                        }
                                                                                    }
                                                                                }
                                                                                if (check == true)
                                                                                {
                                                                                    if (HeaderCount == colcnt)
                                                                                    {
                                                                                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Visible = false;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        sno++;
                                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                                                    }
                                                                                }
                                                                            }
                                                                            #endregion

                                                                            #region Ledger
                                                                            int Ledgercount = 0;
                                                                            if (ddlacctype.SelectedItem.Text == "Ledger")
                                                                            {
                                                                                for (int remv = 0; remv < treeview_spreadfields.Nodes.Count; remv++)
                                                                                {
                                                                                    int hedcnt = 0;
                                                                                    if (treeview_spreadfields.Nodes[remv].ChildNodes.Count > 0)
                                                                                    {
                                                                                        bool check = false;
                                                                                        for (int j = 0; j < treeview_spreadfields.Nodes[remv].ChildNodes.Count; j++)
                                                                                        {
                                                                                            double PaidAmt = 0;
                                                                                            double demandAmt = 0;

                                                                                            if (treeview_spreadfields.Nodes[remv].ChildNodes[j].Checked == true)
                                                                                            {
                                                                                                //allot totalamount
                                                                                                check = true;
                                                                                                Ledgercount++;
                                                                                                ds.Tables[1].DefaultView.RowFilter = "App_no='" + dvapp_no + "' and FeeCategory='" + Feecategory + "'  and LedgerFK='" + Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Value) + "'";
                                                                                                dvallot = ds.Tables[1].DefaultView;
                                                                                                if (dvallot.Count > 0)
                                                                                                    double.TryParse(Convert.ToString(Convert.ToString(dvallot[0]["Demand"])), out demandAmt);
                                                                                                //paid
                                                                                                ds.Tables[3].DefaultView.RowFilter = "App_no='" + dvapp_no + "' and FeeCategory='" + Feecategory + "' and LedgerFK='" + Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Value) + "'";
                                                                                                dvpaid = ds.Tables[3].DefaultView;
                                                                                                col++;
                                                                                                if (dvpaid.Count > 0)
                                                                                                    double.TryParse(Convert.ToString(Convert.ToString(dvpaid[0]["Paid"])), out PaidAmt);

                                                                                                if (demandAmt == PaidAmt)
                                                                                                {
                                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = "-";
                                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                                                                                    hedcnt++;
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    BalAmt = demandAmt - PaidAmt;
                                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(BalAmt);
                                                                                                    Total += BalAmt;
                                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;

                                                                                                }

                                                                                                if (!GrandTotal.ContainsKey(Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text)))
                                                                                                    GrandTotal.Add(Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text), Convert.ToString(BalAmt));
                                                                                                else
                                                                                                {
                                                                                                    double total = 0;
                                                                                                    double.TryParse(Convert.ToString(GrandTotal[Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text)]), out total);
                                                                                                    total += BalAmt;
                                                                                                    GrandTotal.Remove(Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text));
                                                                                                    GrandTotal.Add(Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text), Convert.ToString(total));
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                        /////
                                                                                        if (check == true)
                                                                                        {
                                                                                            if (Ledgercount == hedcnt)
                                                                                            {
                                                                                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Visible = false;
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                sno++;
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                                // if (chklstaccheader.Items.Count == colcnt)
                                                                                // FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Visible = false;
                                                                            }
                                                                            #endregion

                                                                            //last column total
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(Total);
                                                                            //Total += Convert.ToDouble(dv1[0]["Balance"]);
                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                                                            if (!GrandTotal.ContainsKey(Convert.ToString("Total")))
                                                                                GrandTotal.Add(Convert.ToString("Total"), Convert.ToString(Total));
                                                                            else
                                                                            {
                                                                                double total = 0;
                                                                                double.TryParse(Convert.ToString(GrandTotal[Convert.ToString("Total")]), out total);
                                                                                total += Total;
                                                                                GrandTotal.Remove(Convert.ToString("Total"));
                                                                                GrandTotal.Add(Convert.ToString("Total"), Convert.ToString(total));
                                                                            }
                                                                            Total = 0;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            //Grand total
                                            FpSpread1.Sheets[0].RowCount++;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString("Grand Total");
                                            int col1 = 6;
                                            if (ddlacctype.SelectedItem.Text == "Header")
                                            {
                                                #region Grand total

                                                for (int j = 0; j < chklstaccheader.Items.Count; j++)
                                                {
                                                    if (chklstaccheader.Items[j].Selected == true)
                                                    {
                                                        col1++;
                                                        string GetNewValue = Convert.ToString(GrandTotal[Convert.ToString(chklstaccheader.Items[j].Text)]);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].Text = Convert.ToString(GetNewValue);
                                                        //Total += Convert.ToDouble(dv1[0]["Balance"]);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].HorizontalAlign = HorizontalAlign.Right;
                                                    }
                                                }
                                                col1++;
                                                string GetNewValue1 = Convert.ToString(GrandTotal[Convert.ToString("Total")]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].Text = Convert.ToString(GetNewValue1);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].HorizontalAlign = HorizontalAlign.Right;
                                                #endregion
                                            }
                                            else
                                            {
                                                #region ledger

                                                for (int remv = 0; remv < treeview_spreadfields.Nodes.Count; remv++)
                                                {
                                                    if (treeview_spreadfields.Nodes[remv].ChildNodes.Count > 0)
                                                    {
                                                        for (int j = 0; j < treeview_spreadfields.Nodes[remv].ChildNodes.Count; j++)
                                                        {
                                                            if (treeview_spreadfields.Nodes[remv].ChildNodes[j].Checked == true)
                                                            {
                                                                col1++;
                                                                string GetNewValue = Convert.ToString(GrandTotal[Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text)]);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].Text = Convert.ToString(GetNewValue);
                                                                //Total += Convert.ToDouble(dv1[0]["Balance"]);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].HorizontalAlign = HorizontalAlign.Right;
                                                            }
                                                        }
                                                    }
                                                }
                                                string GetNewValue1 = Convert.ToString(GrandTotal[Convert.ToString("Total")]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(GetNewValue1);
                                                //Total += Convert.ToDouble(dv1[0]["Balance"]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;

                                                #endregion
                                            }
                                        }
                                        if (FpSpread1.Sheets[0].RowCount > 0)
                                        {
                                            FpSpread1.Visible = true;
                                            btnprintmaster.Visible = true;
                                            lblrptname.Visible = true;
                                            txtexcelname.Visible = true;
                                            btnxl.Visible = true;
                                            finalflag = true;
                                        }
                                        else
                                        {
                                            FpSpread1.Visible = false;
                                            btnprintmaster.Visible = false;
                                            lblrptname.Visible = false;
                                            txtexcelname.Visible = false;
                                            btnxl.Visible = false;
                                            finalflag = false;
                                        }

                                        #endregion
                                    }


                                    #endregion
                                }
                            }
                            else
                            {          //group header yet to be paid date wise                     
                                if (chkyettobepaid.Checked == true)
                                {
                                    if (ddlfeetype.SelectedItem.Text == "Yet To Be Paid")
                                    {
                                        #region with date

                                        #region query

                                        string commonid = "";
                                        string headerid = "";
                                        string header = "";
                                        string ledger = "";
                                        string ledgerid = "";
                                        string hedgeandledge = "";
                                        string stream = "";
                                        if (ddlacctype.SelectedItem.Text == "Group Header")
                                        {
                                            headerid = ",fs.ChlGroupHeader";
                                            header = " and fs.ChlGroupHeader in('" + header_all + "')";
                                            commonid = "fs.ChlGroupHeader";
                                            if (ddltype.SelectedItem.Text != "Both")
                                                stream = " and fs.stream in('" + ddltype.SelectedItem.Text + "')";
                                        }
                                        //demand amount
                                        string Query = " select SUM(TotalAmount) as Demand, f.App_No,r.degree_code,r.Batch_Year  from FT_FeeAllot f,Registration r,FS_ChlGroupHeaderSettings fs where f.App_No =r.App_No and f.HeaderFK =fs.HeaderFK" + stream + " ";
                                        if (commondist != "")
                                            Query = Query + commondist;

                                        if (batch_all.Trim() != "")
                                            Query = Query + " and r.batch_year  in (" + batch_all + ")";

                                        if (course_all.Trim() != "")
                                            Query = Query + " and r.degree_code  in (" + course_all + ")";

                                        if (stefeesem.Trim() != "")
                                            Query = Query + " and f.FeeCategory  in (" + stefeesem + ")";

                                        if (finyearval.Trim() != "")
                                            Query = Query + " and f.FinYearFK   in (" + Newfinayear + ")";

                                        if (header.Trim() != "")
                                            Query = Query + header;

                                        if (hedgeandledge.Trim() != "")
                                            Query = Query + hedgeandledge;

                                        if (ledger.Trim() != "")
                                            Query = Query + ledger;

                                        Query += "  group by f.App_No,r.degree_code,r.Batch_Year";

                                        //header and ledger
                                        Query += " select SUM(TotalAmount) as Demand, f.App_No,f.FeeCategory,r.degree_code,r.Batch_Year" + headerid + " from FT_FeeAllot f,Registration r,FS_ChlGroupHeaderSettings fs where f.App_No =r.App_No and f.HeaderFK =fs.HeaderFK " + stream + "";
                                        if (commondist != "")
                                            Query = Query + commondist;

                                        if (batch_all.Trim() != "")
                                            Query = Query + " and r.batch_year  in (" + batch_all + ")";

                                        if (course_all.Trim() != "")
                                            Query = Query + " and r.degree_code  in (" + course_all + ")";

                                        if (stefeesem.Trim() != "")
                                            Query = Query + " and f.FeeCategory  in (" + stefeesem + ")";

                                        if (finyearval.Trim() != "")
                                            Query = Query + " and f.FinYearFK   in (" + Newfinayear + ")";

                                        if (header.Trim() != "")
                                            Query = Query + header;

                                        if (hedgeandledge.Trim() != "")
                                            Query = Query + hedgeandledge;

                                        if (ledger.Trim() != "")
                                            Query = Query + ledger;

                                        Query += "  group by f.App_No,f.FeeCategory,r.degree_code,r.Batch_Year" + headerid + "" + ledgerid + "";

                                        //paid amount
                                        Query += " select SUM(TotalAmount) as Demand, f.App_No,r.degree_code,r.Batch_Year,r.Stud_Name,r.Roll_No,r.roll_admit,r.Reg_No,f.FeeCategory from FT_FeeAllot f,Registration r,FS_ChlGroupHeaderSettings fs where f.App_No =r.App_No and f.HeaderFK =fs.HeaderFK " + stream + "";
                                        if (commondist != "")
                                            Query = Query + commondist;

                                        if (batch_all.Trim() != "")
                                            Query = Query + " and r.batch_year  in (" + batch_all + ")";

                                        if (course_all.Trim() != "")
                                            Query = Query + " and r.degree_code  in (" + course_all + ")";

                                        if (stefeesem.Trim() != "")
                                            Query = Query + " and f.FeeCategory  in (" + stefeesem + ")";

                                        if (finyearval.Trim() != "")
                                            Query = Query + " and f.FinYearFK   in (" + Newfinayear + ")";

                                        if (header.Trim() != "")
                                            Query = Query + header;

                                        if (hedgeandledge.Trim() != "")
                                            Query = Query + hedgeandledge;

                                        if (ledger.Trim() != "")
                                            Query = Query + ledger;

                                        Query += "  group by f.App_No,r.degree_code,r.Batch_Year,r.Stud_Name,r.Roll_No,r.roll_admit,r.Reg_No,f.FeeCategory order by f.App_No,f.FeeCategory asc";
                                        Query += " select SUM(Debit) as Paid, f.App_No,f.FeeCategory,r.degree_code,r.Batch_Year,r.Stud_Name,r.Roll_No,r.roll_admit,r.Reg_No" + headerid + " from FT_FinDailyTransaction f,Registration r,FS_ChlGroupHeaderSettings fs where f.App_No =r.App_No and f.HeaderFK =fs.HeaderFK " + stream + "";
                                        if (usBasedRights == true)
                                            Query += " and f.EntryUserCode in('" + usercode + "')";
                                        if (commondist != "")
                                            Query = Query + commondist;

                                        if (batch_all.Trim() != "")
                                            Query = Query + " and r.batch_year  in (" + batch_all + ")";

                                        if (course_all.Trim() != "")
                                            Query = Query + " and r.degree_code  in (" + course_all + ")";

                                        if (stefeesem.Trim() != "")
                                            Query = Query + " and f.FeeCategory  in (" + stefeesem + ")";

                                        if (finyearval.Trim() != "")
                                            Query = Query + " and f.FinYearFK   in (" + Newfinayear + ")";

                                        if (header.Trim() != "")
                                            Query = Query + header;

                                        if (hedgeandledge.Trim() != "")
                                            Query = Query + hedgeandledge;

                                        if (ledger.Trim() != "")
                                            Query = Query + ledger;
                                        Query += " and TransDate <='" + dat1.ToString("MM/dd/yyyy") + "'";
                                        Query += " group by f.App_No,f.FeeCategory,r.degree_code,r.Batch_Year,r.Stud_Name,r.Roll_No,r.roll_admit,r.Reg_No" + headerid + "" + ledgerid + "";

                                        Query = Query + " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code ='" + collegecode + "'";
                                        Query = Query + " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode + "'";

                                        #endregion

                                        ds.Clear();
                                        ds = da.select_method_wo_parameter(Query, "Text");

                                        #region value

                                        DataView dvstddt = new DataView();
                                        DataView paiddt = new DataView();
                                        DataView dvpaid = new DataView();
                                        DataView Dview = new DataView();
                                        DataView dvallot = new DataView();
                                        Hashtable GrandTotal = new Hashtable();
                                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                        {
                                            if (chklstbatch.Items.Count > 0)
                                            {
                                                for (int batch = 0; batch < chklstbatch.Items.Count; batch++)
                                                {
                                                    if (chklstbatch.Items[batch].Selected == true)
                                                    {
                                                        for (int Bnch = 0; Bnch < chklstbranch.Items.Count; Bnch++)
                                                        {
                                                            if (chklstbranch.Items[Bnch].Selected == true)
                                                            {
                                                                //for (int batch = 0; batch < chklstbatch.Items.Count; batch++)
                                                                //{
                                                                //    if (chklstbatch.Items[batch].Selected == true)
                                                                //    {
                                                                ds.Tables[0].DefaultView.RowFilter = "degree_code='" + chklstbranch.Items[Bnch].Value + "' and Batch_Year='" + Convert.ToString(chklstbatch.Items[batch].Text) + "'";
                                                                dvstddt = ds.Tables[0].DefaultView;
                                                                if (dvstddt.Count > 0)
                                                                {
                                                                    FpSpread1.Sheets[0].RowCount++;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(chklstbranch.Items[Bnch].Text) + " - " + Convert.ToString(chklstbatch.Items[batch].Text);
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.LightGreen;
                                                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, FpSpread1.Sheets[0].ColumnCount - 1);

                                                                    for (int row = 0; row < dvstddt.Count; row++)
                                                                    {

                                                                        string Appno = Convert.ToString(dvstddt[row]["App_No"]);
                                                                        string degCode = Convert.ToString(dvstddt[row]["degree_code"]);
                                                                        //  double.TryParse(Convert.ToString(dvstddt[row]["Demand"]), out demandAmt);
                                                                        ds.Tables[2].DefaultView.RowFilter = "App_No=" + Appno + " and degree_code='" + degCode + "'";
                                                                        paiddt = ds.Tables[2].DefaultView;
                                                                        if (paiddt.Count > 0)
                                                                        {
                                                                            for (int drow = 0; drow < paiddt.Count; drow++)
                                                                            {

                                                                                string dvapp_no = Convert.ToString(paiddt[drow]["App_No"]);
                                                                                string Feecategory = Convert.ToString(paiddt[drow]["FeeCategory"]);
                                                                                FpSpread1.Sheets[0].RowCount++;

                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(paiddt[drow]["Stud_Name"]);
                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(paiddt[drow]["Roll_No"]);
                                                                                if (sclflag == false)
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(paiddt[drow]["Reg_No"]);
                                                                                else
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(paiddt[drow]["roll_admit"]);
                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = txtreg;
                                                                                string Degreename = "";
                                                                                if (ds.Tables[5].Rows.Count > 0)
                                                                                {
                                                                                    ds.Tables[5].DefaultView.RowFilter = "Degree_code='" + Convert.ToString(paiddt[drow]["Degree_code"]) + "'";
                                                                                    Dview = ds.Tables[5].DefaultView;
                                                                                    if (Dview.Count > 0)
                                                                                    {
                                                                                        Degreename = Convert.ToString(Dview[0]["degreename"]);
                                                                                    }
                                                                                }
                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Degreename;
                                                                                string TextName = "";
                                                                                if (ds.Tables[4].Rows.Count > 0)
                                                                                {
                                                                                    ds.Tables[4].DefaultView.RowFilter = "TextCode='" + Convert.ToString(paiddt[drow]["FeeCategory"]) + "'";
                                                                                    Dview = ds.Tables[4].DefaultView;
                                                                                    if (Dview.Count > 0)
                                                                                        TextName = Convert.ToString(Dview[0]["TextVal"]);
                                                                                }

                                                                                if (feecateflag == true)
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = TextName;
                                                                                else
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = TextName;

                                                                                int col = 6;
                                                                                double Total = 0;
                                                                                double BalAmt = 0;

                                                                                #region Header
                                                                                int HeaderCount = 0;
                                                                                if (ddlacctype.SelectedItem.Text == "Group Header")
                                                                                {
                                                                                    int colcnt = 0;
                                                                                    for (int j = 0; j < chklstaccheader.Items.Count; j++)
                                                                                    {
                                                                                        double demandAmt = 0;
                                                                                        double PaidAmt = 0;
                                                                                        if (chklstaccheader.Items[j].Selected == true)
                                                                                        {
                                                                                            //allot totalamount
                                                                                            HeaderCount++;
                                                                                            ds.Tables[1].DefaultView.RowFilter = "App_no='" + dvapp_no + "' and FeeCategory='" + Feecategory + "' and ChlGroupHeader='" + Convert.ToString(chklstaccheader.Items[j].Value) + "'";
                                                                                            dvallot = ds.Tables[1].DefaultView;
                                                                                            if (dvallot.Count > 0)
                                                                                                double.TryParse(Convert.ToString(Convert.ToString(dvallot[0]["Demand"])), out demandAmt);
                                                                                            //paid
                                                                                            ds.Tables[3].DefaultView.RowFilter = "App_no='" + dvapp_no + "' and FeeCategory='" + Feecategory + "' and ChlGroupHeader='" + Convert.ToString(chklstaccheader.Items[j].Value) + "'";
                                                                                            dvpaid = ds.Tables[3].DefaultView;
                                                                                            col++;
                                                                                            if (dvpaid.Count > 0)
                                                                                                double.TryParse(Convert.ToString(Convert.ToString(dvpaid[0]["Paid"])), out PaidAmt);

                                                                                            if (demandAmt == PaidAmt)
                                                                                            {
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = "-";
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                                                                                colcnt++;
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                BalAmt = demandAmt - PaidAmt;
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(BalAmt);
                                                                                                Total += BalAmt;
                                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                                                                            }

                                                                                            if (!GrandTotal.ContainsKey(Convert.ToString(chklstaccheader.Items[j].Text)))
                                                                                                GrandTotal.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(BalAmt));
                                                                                            else
                                                                                            {
                                                                                                double total = 0;
                                                                                                double.TryParse(Convert.ToString(GrandTotal[Convert.ToString(chklstaccheader.Items[j].Text)]), out total);
                                                                                                total += BalAmt;
                                                                                                GrandTotal.Remove(Convert.ToString(chklstaccheader.Items[j].Text));
                                                                                                GrandTotal.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(total));
                                                                                                BalAmt = 0;
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                    if (HeaderCount == colcnt)
                                                                                    {
                                                                                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Visible = false;
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        sno++;
                                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                                                    }
                                                                                }
                                                                                #endregion



                                                                                //last column total
                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(Total);
                                                                                //Total += Convert.ToDouble(dv1[0]["Balance"]);
                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                                                                if (!GrandTotal.ContainsKey(Convert.ToString("Total")))
                                                                                    GrandTotal.Add(Convert.ToString("Total"), Convert.ToString(Total));
                                                                                else
                                                                                {
                                                                                    double total = 0;
                                                                                    double.TryParse(Convert.ToString(GrandTotal[Convert.ToString("Total")]), out total);
                                                                                    total += Total;
                                                                                    GrandTotal.Remove(Convert.ToString("Total"));
                                                                                    GrandTotal.Add(Convert.ToString("Total"), Convert.ToString(total));
                                                                                }
                                                                                Total = 0;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                //Grand total
                                                FpSpread1.Sheets[0].RowCount++;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString("Grand Total");
                                                int col1 = 6;
                                                if (ddlacctype.SelectedItem.Text == "Group Header")
                                                {
                                                    #region Grand total

                                                    for (int j = 0; j < chklstaccheader.Items.Count; j++)
                                                    {
                                                        if (chklstaccheader.Items[j].Selected == true)
                                                        {
                                                            col1++;
                                                            string GetNewValue = Convert.ToString(GrandTotal[Convert.ToString(chklstaccheader.Items[j].Text)]);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].Text = Convert.ToString(GetNewValue);
                                                            //Total += Convert.ToDouble(dv1[0]["Balance"]);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].HorizontalAlign = HorizontalAlign.Right;
                                                        }
                                                    }
                                                    col1++;
                                                    string GetNewValue1 = Convert.ToString(GrandTotal[Convert.ToString("Total")]);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].Text = Convert.ToString(GetNewValue1);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].HorizontalAlign = HorizontalAlign.Right;
                                                    #endregion
                                                }
                                            }
                                        }
                                        if (FpSpread1.Sheets[0].RowCount > 0)
                                        {
                                            FpSpread1.Visible = true;
                                            btnprintmaster.Visible = true;
                                            lblrptname.Visible = true;
                                            txtexcelname.Visible = true;
                                            btnxl.Visible = true;
                                            finalflag = true;
                                        }
                                        else
                                        {
                                            FpSpread1.Visible = false;
                                            btnprintmaster.Visible = false;
                                            lblrptname.Visible = false;
                                            txtexcelname.Visible = false;
                                            btnxl.Visible = false;
                                            finalflag = false;
                                        }

                                        #endregion


                                        #endregion
                                    }
                                }
                            }

                        }


                        #endregion




                        #region withoudate Both header and Ledger

                        if (ddlacctype.SelectedItem.Text != "Group Header")
                        {
                            if (ddlfeetype.SelectedItem.Text == "Both")
                            {
                                #region
                                // Jairam New Updation
                                string commonid = "";
                                string headerid = "";
                                string header = "";
                                string ledger = "";
                                string ledgerid = "";
                                string hedgeandledge = "";

                                if (ddlacctype.SelectedItem.Text == "Header")
                                {
                                    headerid = ",f.HeaderFK";
                                    header = " and f.HeaderFK in('" + header_all + "')";
                                    commonid = "HeaderFK";
                                }
                                else if (ddlacctype.SelectedItem.Text == "Ledger")
                                {
                                    ledgerid = ",f.LedgerFK";
                                    ledger = " and f.ledgerFK in('" + getfeeval + "')";
                                    hedgeandledge = " and f.HeaderFK in('" + headervalue + "')";
                                    commonid = "LedgerFK";
                                }

                                string typevalue = "";
                                if (ddltype.Enabled == true)
                                {
                                    typevalue = Convert.ToString(ddltype.SelectedItem.Text);
                                }
                                //  string headerid = " and f.HeaderFK in('" + header_all + "') ";
                                string feeroll = "";
                                if (chkfeeroll.Checked == true)
                                    feeroll = "and r.Roll_No not in(select s.roll_no from stucon s where s.roll_no=r.roll_no and s.semester=r.Current_Semester and s.ack_fee_of_roll=1) ";

                                string cc = "";
                                string debar = "";
                                string disc = "";
                                string commondist = "";
                                if (cblinclude.Items.Count > 0)
                                {
                                    for (int i = 0; i < cblinclude.Items.Count; i++)
                                    {
                                        if (cblinclude.Items[i].Selected == true)
                                        {
                                            if (cblinclude.Items[i].Value == "1")
                                            {
                                                cc = " r.cc=1";
                                            }
                                            if (cblinclude.Items[i].Value == "2")
                                            {
                                                debar = " r.Exam_Flag like '%debar'";
                                            }
                                            if (cblinclude.Items[i].Value == "3")
                                            {
                                                disc = "  r.DelFlag=1";
                                            }
                                        }
                                    }
                                }
                                if (cc != "" && debar == "" && disc == "")
                                    commondist = " and (" + cc + " or r.cc=0)  and r.Exam_Flag<>'debar' and r.DelFlag=0";

                                if (cc == "" && debar != "" && disc == "")
                                    commondist = " and r.cc=0  and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";

                                if (cc == "" && debar == "" && disc != "")
                                    commondist = " and r.cc=0  and r.Exam_Flag<>'debar' and (" + disc + " or r.DelFlag=0)";

                                if (cc != "" && debar != "" && disc == "")
                                    commondist = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";

                                if (cc == "" && debar != "" && disc != "")
                                    commondist = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar')  and (" + disc + " or r.DelFlag=0)";

                                if (cc != "" && debar == "" && disc != "")
                                    commondist = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar'  and (" + disc + " or r.DelFlag=0)";

                                else if (cc == "" && debar == "" && disc == "")
                                    commondist = " and r.cc=0  and r.Exam_Flag<>'debar' and r.DelFlag=0";

                                if (cc != "" && debar != "" && disc != "")
                                    commondist = "";

                                #region Query
                                string Query = " select  f.App_No,sum (TotalAmount) as Total,SUM( PaidAmount) as TotalPaid,sum(BalAmount)as Balance,FeeCategory" + headerid + "" + ledgerid + " from FT_FeeAllot f,Registration r ,Degree d,Department dt,Course c where f.App_No =r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and d.college_code ='" + collegecode + "'";

                                if (commondist != "")
                                    Query = Query + commondist;


                                if (typevalue.Trim() != "Both" && typevalue.Trim() != "")
                                {
                                    Query = Query + " and type ='" + typevalue + "'";
                                }
                                if (batch_all.Trim() != "")
                                {
                                    Query = Query + " and r.batch_year  in (" + batch_all + ")";
                                }
                                if (course_all.Trim() != "")
                                {
                                    Query = Query + " and r.degree_code  in (" + course_all + ")";
                                }
                                if (stefeesem.Trim() != "")
                                {
                                    Query = Query + " and f.FeeCategory  in (" + stefeesem + ")";
                                }
                                if (finyearval.Trim() != "")
                                {
                                    Query = Query + " and f.FinYearFK   in (" + Newfinayear + ")";
                                }
                                if (header.Trim() != "")
                                {
                                    Query = Query + header;
                                }
                                if (hedgeandledge.Trim() != "")
                                {
                                    Query = Query + hedgeandledge;
                                }
                                if (ledger.Trim() != "")
                                {
                                    Query = Query + ledger;
                                }
                                Query = Query + " group by f.App_No,FeeCategory" + headerid + " " + ledgerid + " Order by f.app_no";
                                //daily transaction
                                Query = Query + " select  f.App_No,sum (Debit) as TotalPaid,FeeCategory" + headerid + "" + ledgerid + " from FT_FinDailyTransaction f,Registration r ,Degree d,Department dt,Course c where f.App_No =r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code ='" + collegecode + "' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'";
                                //and Stream =c.type 
                                if (usBasedRights == true)
                                    Query += " and f.EntryUserCode in('" + usercode + "')";
                                if (commondist != "")
                                    Query = Query + commondist;

                                if (typevalue.Trim() != "Both" && typevalue.Trim() != "")
                                {
                                    Query = Query + " and type ='" + typevalue + "'";
                                }
                                if (batch_all.Trim() != "")
                                {
                                    Query = Query + " and r.batch_year  in (" + batch_all + ")";
                                }
                                if (course_all.Trim() != "")
                                {
                                    Query = Query + " and r.degree_code  in (" + course_all + ")";
                                }
                                if (stefeesem.Trim() != "")
                                {
                                    Query = Query + " and f.FeeCategory  in (" + stefeesem + ")";
                                }
                                if (finyearval.Trim() != "")
                                {
                                    Query = Query + " and f.FinYearFK   in (" + Newfinayear + ")";
                                }
                                if (header.Trim() != "")
                                {
                                    Query = Query + header;
                                }
                                if (hedgeandledge.Trim() != "")
                                {
                                    Query = Query + hedgeandledge;
                                }
                                if (ledger.Trim() != "")
                                {
                                    Query = Query + ledger;
                                }
                                Query = Query + " group by f.App_No,FeeCategory" + headerid + " " + ledgerid + " Order by f.app_no";
                                Query = Query + " select distinct f.App_No ,r.degree_code,r.Batch_Year  from FT_FeeAllot f,Registration r ,Degree d,Department dt,Course c where  f.App_No =r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and d.college_code ='" + collegecode + "'";

                                if (commondist != "")
                                    Query = Query + commondist;

                                if (typevalue.Trim() != "Both" && typevalue.Trim() != "")
                                {
                                    Query = Query + " and type ='" + typevalue + "'";
                                }
                                if (batch_all.Trim() != "")
                                {
                                    Query = Query + " and r.batch_year  in (" + batch_all + ")";
                                }
                                if (course_all.Trim() != "")
                                {
                                    Query = Query + " and r.degree_code  in (" + course_all + ")";
                                }
                                if (stefeesem.Trim() != "")
                                {
                                    Query = Query + " and f.FeeCategory  in (" + stefeesem + ")";
                                }
                                if (header.Trim() != "")
                                {
                                    Query = Query + header;
                                }
                                if (hedgeandledge.Trim() != "")
                                {
                                    Query = Query + hedgeandledge;
                                }
                                if (ledger.Trim() != "")
                                {
                                    Query = Query + ledger;
                                }
                                if (finyearval.Trim() != "")
                                {
                                    Query = Query + " and f.FinYearFK   in (" + Newfinayear + ")";
                                }
                                if (chkfeeroll.Checked == true)
                                {
                                    Query = Query + " and r.Roll_No not in(select s.roll_no from stucon s where s.roll_no=r.roll_no and s.semester=r.Current_Semester and s.ack_fee_of_roll=1) order by r.Batch_Year,r.degree_code";
                                }
                                Query = Query + " select distinct f.App_No,r.Roll_No,r.roll_admit,r.Reg_No,r.Stud_Name,(C.Course_Name +'-'+dt.Dept_Name) as Degree,FeeCategory,r.degree_code,T.TextVal from FT_FeeAllot f,Registration r ,Degree d,Department dt,Course c , TextValTable T  where T.TextCode =F.FeeCategory  and f.App_No =r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code ='" + collegecode + "'";

                                if (commondist != "")
                                    Query = Query + commondist;

                                if (typevalue.Trim() != "Both" && typevalue.Trim() != "")
                                {
                                    Query = Query + " and type ='" + typevalue + "'";
                                }
                                if (batch_all.Trim() != "")
                                {
                                    Query = Query + " and r.batch_year  in (" + batch_all + ")";
                                }
                                if (course_all.Trim() != "")
                                {
                                    Query = Query + " and r.degree_code  in (" + course_all + ")";
                                }
                                if (stefeesem.Trim() != "")
                                {
                                    Query = Query + " and f.FeeCategory  in (" + stefeesem + ")";
                                }
                                if (finyearval.Trim() != "")
                                {
                                    Query = Query + " and f.FinYearFK   in (" + Newfinayear + ")";
                                }
                                if (header.Trim() != "")
                                {
                                    Query = Query + header;
                                }
                                if (hedgeandledge.Trim() != "")
                                {
                                    Query = Query + hedgeandledge;
                                }
                                if (ledger.Trim() != "")
                                {
                                    Query = Query + ledger;
                                }



                                #endregion

                                ds.Clear();
                                ds = da.select_method_wo_parameter(Query, "Text");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    string gettotval1 = "";
                                    string getpaidval1 = "";
                                    string GetNewValue1 = "";
                                    int col1 = 0;
                                    DataView dv = new DataView();
                                    DataView dv1 = new DataView();
                                    DataView dv2 = new DataView();
                                    finalflag = true;
                                    Hashtable GrandTotal = new Hashtable();
                                    Hashtable totval = new Hashtable();
                                    Hashtable paidval = new Hashtable();
                                    Hashtable exhash = new Hashtable();

                                    Hashtable gdallot = new Hashtable();
                                    Hashtable gdpaid = new Hashtable();
                                    Hashtable gdbal = new Hashtable();
                                    Hashtable exsh = new Hashtable();

                                    #region value bind
                                    if (chklstbatch.Items.Count > 0)
                                    {
                                        for (int batch = 0; batch < chklstbatch.Items.Count; batch++)
                                        {
                                            if (chklstbatch.Items[batch].Selected == true)
                                            {
                                                for (int Bnch = 0; Bnch < chklstbranch.Items.Count; Bnch++)
                                                {
                                                    if (chklstbranch.Items[Bnch].Selected == true)
                                                    {
                                                        //for (int batch = 0; batch < chklstbatch.Items.Count; batch++)
                                                        //{
                                                        //    if (chklstbatch.Items[batch].Selected == true)
                                                        //    {
                                                        ds.Tables[2].DefaultView.RowFilter = "degree_code='" + chklstbranch.Items[Bnch].Value + "' and Batch_Year='" + Convert.ToString(chklstbatch.Items[batch].Text) + "'";
                                                        dv2 = ds.Tables[2].DefaultView;
                                                        if (dv2.Count > 0)
                                                        {
                                                            FpSpread1.Sheets[0].RowCount++;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(chklstbranch.Items[Bnch].Text) + " - " + Convert.ToString(chklstbatch.Items[batch].Text);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.LightGreen;
                                                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, FpSpread1.Sheets[0].ColumnCount - 1);
                                                            for (int row = 0; row < dv2.Count; row++)
                                                            {
                                                                string app_no = Convert.ToString(dv2[row]["App_No"]);
                                                                ds.Tables[3].DefaultView.RowFilter = "App_No=" + Convert.ToString(app_no) + " and degree_code='" + chklstbranch.Items[Bnch].Value + "'";
                                                                dv = ds.Tables[3].DefaultView;
                                                                for (int drow = 0; drow < dv.Count; drow++)
                                                                {
                                                                    sno++;
                                                                    int col = 6;
                                                                    string dvapp_no = Convert.ToString(dv[drow]["App_No"]);
                                                                    string Feecategory = Convert.ToString(dv[drow]["FeeCategory"]);
                                                                    FpSpread1.Sheets[0].RowCount++;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dv[drow]["Stud_Name"]);
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dv[drow]["Roll_No"]);
                                                                    if (sclflag == false)
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv[drow]["Reg_No"]);
                                                                    else
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv[drow]["roll_admit"]);
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = txtreg;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dv[drow]["Degree"]);
                                                                    if (feecateflag == true)
                                                                    {
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dv[drow]["TextVal"]);
                                                                    }
                                                                    else
                                                                    {
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dv[drow]["TextVal"]);
                                                                    }

                                                                    #region Header allot,paid,balance
                                                                    double Total = 0;
                                                                    double totpaid = 0;
                                                                    double totamt = 0;
                                                                    if (ddlacctype.SelectedItem.Text == "Header")
                                                                    {
                                                                        for (int j = 0; j < chklstaccheader.Items.Count; j++)
                                                                        {
                                                                            if (chklstaccheader.Items[j].Selected == true)
                                                                            {
                                                                                ds.Tables[0].DefaultView.RowFilter = "App_no='" + dvapp_no + "' and FeeCategory='" + Feecategory + "' and HeaderFK='" + Convert.ToString(chklstaccheader.Items[j].Value) + "'";
                                                                                dv1 = ds.Tables[0].DefaultView;
                                                                                //col++;   
                                                                                //  string TotAmt = "";
                                                                                //  string PaidAmt = "";
                                                                                double totalamount = 0;
                                                                                double paidamount = 0;
                                                                                double balanceamt = 0;
                                                                                if (dv1.Count > 0)
                                                                                {
                                                                                    double.TryParse(Convert.ToString(dv1[0]["Total"]), out totalamount);
                                                                                    totamt += totalamount;
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ++col].Text = Convert.ToString(totalamount);
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                                                                    DataView dvpaid = new DataView();
                                                                                    ds.Tables[1].DefaultView.RowFilter = "App_no='" + dvapp_no + "' and FeeCategory='" + Feecategory + "' and HeaderFK='" + Convert.ToString(chklstaccheader.Items[j].Value) + "'";
                                                                                    dvpaid = ds.Tables[1].DefaultView;
                                                                                    if (dvpaid.Count > 0)
                                                                                    {
                                                                                        double.TryParse(Convert.ToString(dvpaid[0]["TotalPaid"]), out paidamount);
                                                                                        totpaid += paidamount;
                                                                                    }
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ++col].Text = Convert.ToString(paidamount);
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                                                                    balanceamt = totalamount - paidamount;
                                                                                    Total += balanceamt;
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ++col].Text = Convert.ToString(balanceamt);
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;


                                                                                    //balance
                                                                                    if (!GrandTotal.ContainsKey(Convert.ToString(chklstaccheader.Items[j].Text)))
                                                                                        GrandTotal.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(balanceamt));
                                                                                    else
                                                                                    {
                                                                                        double total = 0;
                                                                                        double.TryParse(Convert.ToString(GrandTotal[Convert.ToString(chklstaccheader.Items[j].Text)]), out total);
                                                                                        total += balanceamt;
                                                                                        GrandTotal.Remove(Convert.ToString(chklstaccheader.Items[j].Text));
                                                                                        GrandTotal.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(total));
                                                                                    }
                                                                                    //total
                                                                                    if (!totval.ContainsKey(Convert.ToString(chklstaccheader.Items[j].Text)))
                                                                                        totval.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(totalamount));
                                                                                    else
                                                                                    {
                                                                                        double totvalue = 0;
                                                                                        double.TryParse(Convert.ToString(totval[Convert.ToString(chklstaccheader.Items[j].Text)]), out totvalue);
                                                                                        totvalue += totalamount;
                                                                                        totval.Remove(Convert.ToString(chklstaccheader.Items[j].Text));
                                                                                        totval.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(totvalue));
                                                                                    }
                                                                                    //paid
                                                                                    if (!paidval.ContainsKey(Convert.ToString(chklstaccheader.Items[j].Text)))
                                                                                        paidval.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(paidamount));
                                                                                    else
                                                                                    {
                                                                                        double paidvalue = 0;
                                                                                        double.TryParse(Convert.ToString(paidval[Convert.ToString(chklstaccheader.Items[j].Text)]), out paidvalue);
                                                                                        paidvalue += paidamount;
                                                                                        paidval.Remove(Convert.ToString(chklstaccheader.Items[j].Text));
                                                                                        paidval.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(paidvalue));
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    col += 3;
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = "-";
                                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                                                                }

                                                                            }
                                                                        }
                                                                    }
                                                                    #endregion

                                                                    #region Ledger allot,paid,balance

                                                                    if (ddlacctype.SelectedItem.Text == "Ledger")
                                                                    {
                                                                        for (int remv = 0; remv < treeview_spreadfields.Nodes.Count; remv++)
                                                                        {
                                                                            if (treeview_spreadfields.Nodes[remv].ChildNodes.Count > 0)
                                                                            {
                                                                                for (int j = 0; j < treeview_spreadfields.Nodes[remv].ChildNodes.Count; j++)
                                                                                {
                                                                                    if (treeview_spreadfields.Nodes[remv].ChildNodes[j].Checked == true)
                                                                                    {
                                                                                        ds.Tables[0].DefaultView.RowFilter = "App_no='" + dvapp_no + "' and FeeCategory='" + Feecategory + "' and LedgerFK='" + Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Value) + "'";
                                                                                        dv1 = ds.Tables[0].DefaultView;
                                                                                        // col++;
                                                                                        #region old
                                                                                        //if (dv1.Count > 0)
                                                                                        //{
                                                                                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(dv1[0]["Balance"]);
                                                                                        //    string value = Convert.ToString(dv1[0]["Balance"]);
                                                                                        //    if (value.Trim() != "")
                                                                                        //    {
                                                                                        //        Total += Convert.ToDouble(value);
                                                                                        //    }
                                                                                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                                                                        //    if (!GrandTotal.ContainsKey(Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text)))
                                                                                        //    {
                                                                                        //        GrandTotal.Add(Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text), Convert.ToString(dv1[0]["Balance"]));
                                                                                        //    }
                                                                                        //    else
                                                                                        //    {
                                                                                        //        double total = 0;
                                                                                        //        string getvalue = Convert.ToString(GrandTotal[Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text)]);
                                                                                        //        if (getvalue.Trim() != "")
                                                                                        //        {
                                                                                        //            if (Convert.ToString(dv1[0]["Balance"]).Trim() != "")
                                                                                        //            {
                                                                                        //                total = Convert.ToDouble(getvalue) + Convert.ToDouble(dv1[0]["Balance"]);
                                                                                        //            }
                                                                                        //            else
                                                                                        //            {
                                                                                        //                total = Convert.ToDouble(getvalue);
                                                                                        //            }
                                                                                        //            GrandTotal.Remove(Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text));
                                                                                        //            GrandTotal.Add(Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text), Convert.ToString(total));
                                                                                        //        }

                                                                                        //    }
                                                                                        //}
                                                                                        //else
                                                                                        //{
                                                                                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = "-";
                                                                                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                                                                        //}
                                                                                        #endregion
                                                                                        double totalamount = 0;
                                                                                        double paidamount = 0;
                                                                                        double balanceamt = 0;
                                                                                        if (dv1.Count > 0)
                                                                                        {
                                                                                            double.TryParse(Convert.ToString(dv1[0]["Total"]), out totalamount);
                                                                                            totamt += totalamount;
                                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ++col].Text = Convert.ToString(totalamount);
                                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                                                                            DataView dvpaid = new DataView();
                                                                                            ds.Tables[1].DefaultView.RowFilter = "App_no='" + dvapp_no + "' and FeeCategory='" + Feecategory + "' and LedgerFK='" + Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Value) + "'";
                                                                                            dvpaid = ds.Tables[1].DefaultView;
                                                                                            if (dvpaid.Count > 0)
                                                                                            {
                                                                                                double.TryParse(Convert.ToString(dvpaid[0]["TotalPaid"]), out paidamount);
                                                                                                totpaid += paidamount;
                                                                                            }
                                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ++col].Text = Convert.ToString(paidamount);
                                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                                                                            balanceamt = totalamount - paidamount;
                                                                                            Total += balanceamt;
                                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ++col].Text = Convert.ToString(balanceamt);
                                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                                                                                            if (!GrandTotal.ContainsKey(Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text)))
                                                                                                GrandTotal.Add(Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text), Convert.ToString(balanceamt));
                                                                                            else
                                                                                            {
                                                                                                double total = 0;
                                                                                                double.TryParse(Convert.ToString(GrandTotal[Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text)]), out total);
                                                                                                total += balanceamt;
                                                                                                GrandTotal.Remove(Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text));
                                                                                                GrandTotal.Add(Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text), Convert.ToString(total));
                                                                                            }
                                                                                            if (!totval.ContainsKey(Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text)))
                                                                                                totval.Add(Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text), Convert.ToString(totalamount));
                                                                                            else
                                                                                            {
                                                                                                double totvalue = 0;
                                                                                                double.TryParse(Convert.ToString(totval[Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text)]), out totvalue);
                                                                                                totvalue += totalamount;
                                                                                                totval.Remove(Convert.ToString(Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text)));
                                                                                                totval.Add(Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text), Convert.ToString(totvalue));
                                                                                            }
                                                                                            if (!paidval.ContainsKey(Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text)))
                                                                                                paidval.Add(Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text), Convert.ToString(paidamount));
                                                                                            else
                                                                                            {
                                                                                                double paidvalue = 0;
                                                                                                double.TryParse(Convert.ToString(paidval[Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text)]), out paidvalue);
                                                                                                paidvalue += paidamount;
                                                                                                paidval.Remove(Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text));
                                                                                                paidval.Add(Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text), Convert.ToString(paidvalue));
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            col += 3;
                                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = "-";
                                                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                                                                        }

                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }

                                                                    #endregion
                                                                    double excessvalue = 0;
                                                                    #region Total allot
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 5].Text = Convert.ToString(totamt);
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 5].HorizontalAlign = HorizontalAlign.Right;
                                                                    if (!totval.ContainsKey(Convert.ToString("totamt")))
                                                                        totval.Add(Convert.ToString("totamt"), Convert.ToString(totamt));
                                                                    else
                                                                    {
                                                                        double total = 0;
                                                                        double.TryParse(Convert.ToString(totval[Convert.ToString("totamt")]), out total);
                                                                        total += totamt;
                                                                        totval.Remove(Convert.ToString("totamt"));
                                                                        totval.Add(Convert.ToString("totamt"), Convert.ToString(total));
                                                                    }
                                                                    if (totpaid > totamt)
                                                                    {
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 4].Text = Convert.ToString(totpaid);
                                                                        excessvalue = totpaid - totamt;
                                                                    }
                                                                    else
                                                                    {
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 4].Text = Convert.ToString(totpaid);
                                                                    }
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 4].HorizontalAlign = HorizontalAlign.Right;
                                                                    if (!paidval.ContainsKey(Convert.ToString("totpaid")))
                                                                        paidval.Add(Convert.ToString("totpaid"), Convert.ToString(totpaid));
                                                                    else
                                                                    {
                                                                        double total = 0;
                                                                        double.TryParse(Convert.ToString(paidval[Convert.ToString("totpaid")]), out total);
                                                                        total += totpaid;
                                                                        paidval.Remove(Convert.ToString("totpaid"));
                                                                        paidval.Add(Convert.ToString("totpaid"), Convert.ToString(total));
                                                                    }
                                                                    if (Total <= 0)
                                                                    {
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 3].Text = Convert.ToString(0);
                                                                        Total = 0;
                                                                    }
                                                                    else
                                                                    {
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 3].Text = Convert.ToString(Total);

                                                                    }
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Right;

                                                                    if (!GrandTotal.ContainsKey(Convert.ToString("Total")))
                                                                        GrandTotal.Add(Convert.ToString("Total"), Convert.ToString(Total));
                                                                    else
                                                                    {
                                                                        double total = 0;
                                                                        double.TryParse(Convert.ToString(GrandTotal[Convert.ToString("Total")]), out total);
                                                                        total += Total;
                                                                        GrandTotal.Remove(Convert.ToString("Total"));
                                                                        GrandTotal.Add(Convert.ToString("Total"), Convert.ToString(total));
                                                                    }
                                                                    if (excessvalue != 0)
                                                                    {
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Text
                                                                            = Convert.ToString(excessvalue);
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Right;
                                                                        if (!exhash.ContainsKey(Convert.ToString("extot")))
                                                                            exhash.Add(Convert.ToString("extot"), Convert.ToString(excessvalue));
                                                                        else
                                                                        {
                                                                            double total = 0;
                                                                            double.TryParse(Convert.ToString(exhash[Convert.ToString("extot")]), out total);
                                                                            total += excessvalue;
                                                                            exhash.Remove(Convert.ToString("extot"));
                                                                            exhash.Add(Convert.ToString("extot"), Convert.ToString(total));
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Text
                                                                           = Convert.ToString(0);
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Right;
                                                                    }

                                                                    if (Total == 0 || excessvalue != 0)
                                                                    {
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Paid";
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Left;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].BackColor = ColorTranslator.FromHtml("#99ebff");
                                                                    }
                                                                    else
                                                                    {
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "UnPaid";
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Left;
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].BackColor = ColorTranslator.FromHtml("#ff6666");
                                                                    }
                                                                    #endregion
                                                                }
                                                            }

                                                            ///toal individual dept
                                                            if (ddlacctype.SelectedItem.Text == "Header")
                                                            {
                                                                #region total individual dept

                                                                FpSpread1.Sheets[0].RowCount++;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString("Total");
                                                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].BackColor = ColorTranslator.FromHtml("#4870BE");
                                                                col1 = 6;
                                                                for (int j = 0; j < chklstaccheader.Items.Count; j++)
                                                                {
                                                                    if (chklstaccheader.Items[j].Selected == true)
                                                                    {
                                                                        //col1++;
                                                                        string gettotval = Convert.ToString(totval[Convert.ToString(chklstaccheader.Items[j].Text)]);
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ++col1].Text = Convert.ToString(gettotval);
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].HorizontalAlign = HorizontalAlign.Right;
                                                                        if (!gdallot.ContainsKey(Convert.ToString(chklstaccheader.Items[j].Text)))
                                                                            gdallot.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(gettotval));
                                                                        else
                                                                        {
                                                                            double total = 0;
                                                                            double.TryParse(Convert.ToString(gdallot[Convert.ToString(chklstaccheader.Items[j].Text)]), out total);
                                                                            if (Convert.ToString(gettotval).Trim() != "")
                                                                            {
                                                                                total += Convert.ToDouble(gettotval);
                                                                            }
                                                                            gdallot.Remove(Convert.ToString(chklstaccheader.Items[j].Text));
                                                                            gdallot.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(total));
                                                                            total = 0;
                                                                        }

                                                                        ///
                                                                        string getpaidval = Convert.ToString(paidval[Convert.ToString(chklstaccheader.Items[j].Text)]);
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ++col1].Text = Convert.ToString(getpaidval);
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].HorizontalAlign = HorizontalAlign.Right;
                                                                        if (!gdpaid.ContainsKey(Convert.ToString(chklstaccheader.Items[j].Text)))
                                                                            gdpaid.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(getpaidval));
                                                                        else
                                                                        {
                                                                            double total = 0;
                                                                            double.TryParse(Convert.ToString(gdpaid[Convert.ToString(chklstaccheader.Items[j].Text)]), out total);
                                                                            if (getpaidval.Trim() != "")
                                                                            {
                                                                                total += Convert.ToDouble(getpaidval);
                                                                            }
                                                                            gdpaid.Remove(Convert.ToString(chklstaccheader.Items[j].Text));
                                                                            gdpaid.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(total));
                                                                            total = 0;
                                                                        }

                                                                        string GetNewValue = Convert.ToString(GrandTotal[Convert.ToString(chklstaccheader.Items[j].Text)]);
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ++col1].Text = Convert.ToString(GetNewValue);
                                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].HorizontalAlign = HorizontalAlign.Right;
                                                                        if (!gdbal.ContainsKey(Convert.ToString(chklstaccheader.Items[j].Text)))
                                                                            gdbal.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(GetNewValue));
                                                                        else
                                                                        {
                                                                            double total = 0;
                                                                            double.TryParse(Convert.ToString(gdbal[Convert.ToString(chklstaccheader.Items[j].Text)]), out total);
                                                                            if (GetNewValue.Trim() != "")
                                                                            {
                                                                                total += Convert.ToDouble(GetNewValue);
                                                                            }
                                                                            gdbal.Remove(Convert.ToString(chklstaccheader.Items[j].Text));
                                                                            gdbal.Add(Convert.ToString(chklstaccheader.Items[j].Text), Convert.ToString(total));
                                                                            total = 0;
                                                                        }
                                                                    }
                                                                }

                                                                gettotval1 = Convert.ToString(totval[Convert.ToString("totamt")]);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 5].Text = Convert.ToString(gettotval1);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 5].HorizontalAlign = HorizontalAlign.Right;
                                                                //grand total allot
                                                                if (!gdallot.ContainsKey(Convert.ToString("totamt")))
                                                                    gdallot.Add(Convert.ToString("totamt"), Convert.ToString(gettotval1));
                                                                else
                                                                {
                                                                    double total = 0;
                                                                    double.TryParse(Convert.ToString(gdallot[Convert.ToString("totamt")]), out total);
                                                                    total += Convert.ToDouble(gettotval1);
                                                                    gdallot.Remove(Convert.ToString("totamt"));
                                                                    gdallot.Add(Convert.ToString("totamt"), Convert.ToString(total));
                                                                }

                                                                getpaidval1 = Convert.ToString(paidval[Convert.ToString("totpaid")]);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 4].Text = Convert.ToString(getpaidval1);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 4].HorizontalAlign = HorizontalAlign.Right;
                                                                if (!gdpaid.ContainsKey(Convert.ToString("totpaid")))
                                                                    gdpaid.Add(Convert.ToString("totpaid"), Convert.ToString(getpaidval1));
                                                                else
                                                                {
                                                                    double total = 0;
                                                                    double.TryParse(Convert.ToString(gdpaid[Convert.ToString("totpaid")]), out total);
                                                                    total += Convert.ToDouble(getpaidval1);
                                                                    gdpaid.Remove(Convert.ToString("totpaid"));
                                                                    gdpaid.Add(Convert.ToString("totpaid"), Convert.ToString(total));
                                                                }
                                                                GetNewValue1 = Convert.ToString(GrandTotal[Convert.ToString("Total")]);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 3].Text = Convert.ToString(GetNewValue1);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Right;
                                                                if (!gdbal.ContainsKey(Convert.ToString("Total")))
                                                                    gdbal.Add(Convert.ToString("Total"), Convert.ToString(GetNewValue1));
                                                                else
                                                                {
                                                                    double total = 0;
                                                                    double.TryParse(Convert.ToString(gdbal[Convert.ToString("Total")]), out total);
                                                                    total += Convert.ToDouble(GetNewValue1);
                                                                    gdbal.Remove(Convert.ToString("Total"));
                                                                    gdbal.Add(Convert.ToString("Total"), Convert.ToString(total));
                                                                }
                                                                //excess
                                                                string excessval = Convert.ToString(exhash[Convert.ToString("extot")]);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Text = Convert.ToString(excessval);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Right;
                                                                if (!exsh.ContainsKey(Convert.ToString("extot")))
                                                                    exsh.Add(Convert.ToString("extot"), Convert.ToString(excessval));
                                                                else
                                                                {
                                                                    double total = 0;
                                                                    double.TryParse(Convert.ToString(exsh[Convert.ToString("extot")]), out total);
                                                                    if (excessval.Trim() != "")
                                                                    {
                                                                        total += Convert.ToDouble(excessval);
                                                                    }
                                                                    exsh.Remove(Convert.ToString("extot"));
                                                                    exsh.Add(Convert.ToString("extot"), Convert.ToString(total));
                                                                }
                                                                totval.Clear();
                                                                paidval.Clear();
                                                                GrandTotal.Clear();
                                                                exhash.Clear();
                                                                #endregion
                                                            }
                                                            else
                                                            {
                                                                #region total individual dept
                                                                FpSpread1.Sheets[0].RowCount++;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString("Total");
                                                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].BackColor = ColorTranslator.FromHtml("#4870BE");
                                                                col1 = 6;
                                                                for (int remv = 0; remv < treeview_spreadfields.Nodes.Count; remv++)
                                                                {
                                                                    if (treeview_spreadfields.Nodes[remv].ChildNodes.Count > 0)
                                                                    {
                                                                        for (int j = 0; j < treeview_spreadfields.Nodes[remv].ChildNodes.Count; j++)
                                                                        {
                                                                            if (treeview_spreadfields.Nodes[remv].ChildNodes[j].Checked == true)
                                                                            {
                                                                                col1++;
                                                                                string gettotval = Convert.ToString(totval[Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text)]);
                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].Text = Convert.ToString(gettotval);
                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].HorizontalAlign = HorizontalAlign.Right;
                                                                                if (!gdallot.ContainsKey(Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text)))
                                                                                    gdallot.Add(Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text), Convert.ToString(gettotval));
                                                                                else
                                                                                {
                                                                                    double total = 0;
                                                                                    double.TryParse(Convert.ToString(gdallot[Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text)]), out total);
                                                                                    if (gettotval.Trim() != "")
                                                                                    {
                                                                                        total += Convert.ToDouble(gettotval);
                                                                                    }
                                                                                    gdallot.Remove(Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text));
                                                                                    gdallot.Add(Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text), Convert.ToString(total));
                                                                                }

                                                                                col1++;
                                                                                string getpaidval = Convert.ToString(paidval[Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text)]);
                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].Text = Convert.ToString(getpaidval);
                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].HorizontalAlign = HorizontalAlign.Right;
                                                                                if (!gdpaid.ContainsKey(Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text)))
                                                                                    gdpaid.Add(Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text), Convert.ToString(getpaidval));
                                                                                else
                                                                                {
                                                                                    double total = 0;
                                                                                    double.TryParse(Convert.ToString(gdpaid[Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text)]), out total);
                                                                                    if (getpaidval.Trim() != "")
                                                                                    {
                                                                                        total += Convert.ToDouble(getpaidval);
                                                                                    }
                                                                                    gdpaid.Remove(Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text));
                                                                                    gdpaid.Add(Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text), Convert.ToString(total));
                                                                                    total = 0;
                                                                                }
                                                                                col1++;
                                                                                string GetNewValue = Convert.ToString(GrandTotal[Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text)]);
                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].Text = Convert.ToString(GetNewValue);
                                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].HorizontalAlign = HorizontalAlign.Right;
                                                                                if (!gdbal.ContainsKey(Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text)))
                                                                                    gdbal.Add(Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text), Convert.ToString(GetNewValue));
                                                                                else
                                                                                {
                                                                                    double total = 0;
                                                                                    double.TryParse(Convert.ToString(gdbal[Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text)]), out total);
                                                                                    if (GetNewValue.Trim() != "")
                                                                                    {
                                                                                        total += Convert.ToDouble(GetNewValue);
                                                                                    }
                                                                                    gdbal.Remove(Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text));
                                                                                    gdbal.Add(Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text), Convert.ToString(total));
                                                                                    total = 0;
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }

                                                                gettotval1 = Convert.ToString(totval[Convert.ToString("totamt")]);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 5].Text = Convert.ToString(gettotval1);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 5].HorizontalAlign = HorizontalAlign.Right;
                                                                //grand total allot
                                                                if (!gdallot.ContainsKey(Convert.ToString("totamt")))
                                                                    gdallot.Add(Convert.ToString("totamt"), Convert.ToString(gettotval1));
                                                                else
                                                                {
                                                                    double total = 0;
                                                                    double.TryParse(Convert.ToString(gdallot[Convert.ToString("totamt")]), out total);
                                                                    if (gettotval1.Trim() != "")
                                                                    {
                                                                        total += Convert.ToDouble(gettotval1);
                                                                    }
                                                                    gdallot.Remove(Convert.ToString("totamt"));
                                                                    gdallot.Add(Convert.ToString("totamt"), Convert.ToString(total));
                                                                }

                                                                getpaidval1 = Convert.ToString(paidval[Convert.ToString("totpaid")]);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 4].Text = Convert.ToString(getpaidval1);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 4].HorizontalAlign = HorizontalAlign.Right;
                                                                if (!gdpaid.ContainsKey(Convert.ToString("totpaid")))
                                                                    gdpaid.Add(Convert.ToString("totpaid"), Convert.ToString(getpaidval1));
                                                                else
                                                                {
                                                                    double total = 0;
                                                                    double.TryParse(Convert.ToString(gdpaid[Convert.ToString("totpaid")]), out total);
                                                                    if (getpaidval1.Trim() != "")
                                                                    {
                                                                        total += Convert.ToDouble(getpaidval1);
                                                                    }
                                                                    gdpaid.Remove(Convert.ToString("totpaid"));
                                                                    gdpaid.Add(Convert.ToString("totpaid"), Convert.ToString(total));
                                                                }

                                                                GetNewValue1 = Convert.ToString(GrandTotal[Convert.ToString("Total")]);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 3].Text = Convert.ToString(GetNewValue1);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Right;
                                                                if (!gdbal.ContainsKey(Convert.ToString("Total")))
                                                                    gdbal.Add(Convert.ToString("Total"), Convert.ToString(GetNewValue1));
                                                                else
                                                                {
                                                                    double total = 0;
                                                                    double.TryParse(Convert.ToString(gdbal[Convert.ToString("Total")]), out total);
                                                                    if (GetNewValue1.Trim() != "")
                                                                    {
                                                                        total += Convert.ToDouble(GetNewValue1);
                                                                    }
                                                                    gdbal.Remove(Convert.ToString("Total"));
                                                                    gdbal.Add(Convert.ToString("Total"), Convert.ToString(total));
                                                                }
                                                                string excessval = Convert.ToString(exhash[Convert.ToString("extot")]);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Text = Convert.ToString(excessval);
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Right;
                                                                if (!exsh.ContainsKey(Convert.ToString("extot")))
                                                                    exsh.Add(Convert.ToString("extot"), Convert.ToString(excessval));
                                                                else
                                                                {
                                                                    double total = 0;
                                                                    double.TryParse(Convert.ToString(exsh[Convert.ToString("extot")]), out total);
                                                                    if (excessval.Trim() != "")
                                                                    {
                                                                        total += Convert.ToDouble(excessval);
                                                                    }
                                                                    exsh.Remove(Convert.ToString("extot"));
                                                                    exsh.Add(Convert.ToString("extot"), Convert.ToString(total));
                                                                }
                                                                totval.Clear();
                                                                paidval.Clear();
                                                                GrandTotal.Clear();
                                                                exhash.Clear();
                                                                #endregion
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #region  grand total
                                        if (ddlacctype.SelectedItem.Text == "Header")
                                        {
                                            #region header
                                            string val = "";
                                            FpSpread1.Sheets[0].RowCount++;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString("Grand Total");
                                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].BackColor = ColorTranslator.FromHtml("#ffbf00");
                                            col1 = 6;
                                            for (int j = 0; j < chklstaccheader.Items.Count; j++)
                                            {
                                                if (chklstaccheader.Items[j].Selected == true)
                                                {
                                                    //col1++;
                                                    string gettotval = Convert.ToString(gdallot[Convert.ToString(chklstaccheader.Items[j].Text)]);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ++col1].Text = Convert.ToString(gettotval);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].HorizontalAlign = HorizontalAlign.Right;

                                                    string getpaidval = Convert.ToString(gdpaid[Convert.ToString(chklstaccheader.Items[j].Text)]);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ++col1].Text = Convert.ToString(getpaidval);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].HorizontalAlign = HorizontalAlign.Right;

                                                    string GetNewValue = Convert.ToString(gdbal[Convert.ToString(chklstaccheader.Items[j].Text)]);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ++col1].Text = Convert.ToString(GetNewValue);
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].HorizontalAlign = HorizontalAlign.Right;

                                                    //  val = Convert.ToString(exsh[Convert.ToString(chklstaccheader.Items[j].Text)]);
                                                    // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ++col1].Text = Convert.ToString(val);
                                                    // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].HorizontalAlign = HorizontalAlign.Right;
                                                }
                                            }
                                            gettotval1 = Convert.ToString(gdallot[Convert.ToString("totamt")]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 5].Text = Convert.ToString(gettotval1);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 5].HorizontalAlign = HorizontalAlign.Right;

                                            getpaidval1 = Convert.ToString(gdpaid[Convert.ToString("totpaid")]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 4].Text = Convert.ToString(getpaidval1);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 4].HorizontalAlign = HorizontalAlign.Right;


                                            GetNewValue1 = Convert.ToString(gdbal[Convert.ToString("Total")]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 3].Text = Convert.ToString(GetNewValue1);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Right;

                                            string val1 = Convert.ToString(exsh[Convert.ToString("extot")]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Text = Convert.ToString(val1);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Right;
                                            #endregion
                                        }

                                        else
                                        {
                                            col1 = 6;
                                            FpSpread1.Sheets[0].RowCount++;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString("Grand Total");
                                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].BackColor = ColorTranslator.FromHtml("#ffbf00");
                                            for (int remv = 0; remv < treeview_spreadfields.Nodes.Count; remv++)
                                            {
                                                if (treeview_spreadfields.Nodes[remv].ChildNodes.Count > 0)
                                                {
                                                    for (int j = 0; j < treeview_spreadfields.Nodes[remv].ChildNodes.Count; j++)
                                                    {
                                                        if (treeview_spreadfields.Nodes[remv].ChildNodes[j].Checked == true)
                                                        {
                                                            col1++;
                                                            string gettotval = Convert.ToString(gdallot[Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text)]);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].Text = Convert.ToString(gettotval);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].HorizontalAlign = HorizontalAlign.Right;
                                                            col1++;
                                                            string getpaidval = Convert.ToString(gdpaid[Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text)]);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].Text = Convert.ToString(getpaidval);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].HorizontalAlign = HorizontalAlign.Right;
                                                            col1++;
                                                            string GetNewValue = Convert.ToString(gdbal[Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text)]);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].Text = Convert.ToString(GetNewValue);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col1].HorizontalAlign = HorizontalAlign.Right;
                                                        }
                                                    }
                                                }
                                            }
                                            gettotval1 = Convert.ToString(gdallot[Convert.ToString("totamt")]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 5].Text = Convert.ToString(gettotval1);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 5].HorizontalAlign = HorizontalAlign.Right;

                                            getpaidval1 = Convert.ToString(gdpaid[Convert.ToString("totpaid")]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 4].Text = Convert.ToString(getpaidval1);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 4].HorizontalAlign = HorizontalAlign.Right;


                                            GetNewValue1 = Convert.ToString(gdbal[Convert.ToString("Total")]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 3].Text = Convert.ToString(GetNewValue1);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 3].HorizontalAlign = HorizontalAlign.Right;


                                            string val = Convert.ToString(exsh[Convert.ToString("extot")]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Text = Convert.ToString(val);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Right;
                                        }
                                        #endregion
                                    }

                                    #endregion

                                    if (FpSpread1.Sheets[0].RowCount > 0)
                                    {
                                        FpSpread1.Visible = true;
                                        btnprintmaster.Visible = true;
                                        lblrptname.Visible = true;
                                        txtexcelname.Visible = true;
                                        btnxl.Visible = true;
                                    }
                                    else
                                    {
                                        FpSpread1.Visible = false;
                                        btnprintmaster.Visible = false;
                                        lblrptname.Visible = false;
                                        txtexcelname.Visible = false;
                                        btnxl.Visible = false;
                                    }
                                }
                                #endregion
                            }
                        }


                        #endregion

                        batchquery = null;
                        degreequery = null;
                        setbatchanddegree = null;
                        getdegreedetails = null;
                        delflg = null;
                        demquery = null;
                        y1 = null;
                        y2 = null;
                        starttear = null;
                        endyear = null;
                        batch_all = null;
                        course_all = null;
                        header_all = null;
                        category = null;
                        feecode = null;
                        tot_category = null;
                        strorder = null;
                        paidquery = null;
                        str = null;
                        degquery = null;
                        courseid = null;
                        batcyear = null;
                        year = null;
                        deg_acr = null;
                        deg_dept = null;

                        ds.Dispose();
                        ds1.Dispose();
                        ds2.Dispose();
                        ds3.Dispose();
                        dvhead.Dispose();
                        dv_get_course.Dispose();
                        dv_paid_data.Dispose();
                        dv_demand_amount.Dispose();

                        ds.Clear();
                        ds1.Clear();
                        ds2.Clear();
                        ds3.Clear();

                        ds = null;
                        ds1 = null;
                        ds2 = null;
                        ds3 = null;
                        dvhead = null;
                        dv_get_course = null;
                        dv_paid_data = null;
                        dv_demand_amount = null;

                        dicstu.Clear();
                        dictotal.Clear();
                        dicgrandtotal.Clear();
                        dicyetpaid.Clear();

                        dicstu = null;
                        dictotal = null;
                        dicgrandtotal = null;
                        dicyetpaid = null;
                    }
                    if (FpSpread1.Sheets[0].ColumnHeader.RowCount > 2)
                    {
                        if (ddlacctype.SelectedItem.Text == "Header")
                        {
                            FpSpread1.Sheets[0].ColumnHeader.Rows[2].Visible = false;
                        }
                        else
                        {
                            FpSpread1.Sheets[0].ColumnHeader.Rows[2].Visible = true;
                        }
                    }

                }

                if (finalflag == false)
                {
                    errmsg.Visible = true;
                    errmsg.Text = "No Records Found";
                    FpSpread1.Visible = false;
                    btnprintmaster.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    btnxl.Visible = false;
                    lblc1.Visible = false;
                    lblc2.Visible = false;
                    lblc3.Visible = false;
                    lblc4.Visible = false;
                    lblc5.Visible = false;
                    lblcard.Visible = false;
                }
                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                // FpSpread1.Sheets[0].AutoPostBack = false;
                MyClass ms = new MyClass();
                ms.Dispose();
                GC.SuppressFinalize(this);
                GC.Collect();
                GC.WaitForFullGCComplete();

            }
            catch (System.OutOfMemoryException ex)
            {
                //errmsg.Text = ex.ToString();
                //errmsg.Visible = true;
                //FpSpread1.Visible = true;
                //btnprintmaster.Visible = true;
                //lblrptname.Visible = true;
                //txtexcelname.Visible = true;
                //btnxl.Visible = true;
                //lblc1.Visible = true;
                //lblc2.Visible = true;
                //lblc3.Visible = true;
                //lblc4.Visible = true;
                //return;
            }
        }
        catch (Exception ex)
        {
            //  da.sendErrorMail(ex, ddl_collegename.SelectedItem.Value, "Overall_student_Fee_Status.aspx");
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    public class MyClass : IDisposable
    {
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // called via myClass.Dispose(). 
                    // OK to use any private object references
                }

                disposed = true;
            }
            disposed = true;
        }

        public void Dispose() // Implement IDisposable
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~MyClass() // the finalizer
        {
            Dispose(false);
        }
    }

    protected void chklstaccheader_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string clg = "";
            int commcount = 0;
            chkaccheader.Checked = false;
            for (int i = 0; i < chklstaccheader.Items.Count; i++)
            {
                if (chklstaccheader.Items[i].Selected == true)
                {
                    commcount = commcount + 1;

                    if (clg == "")
                    {
                        clg = chklstaccheader.Items[i].Value.ToString();
                    }
                    else
                    {
                        clg = clg + "," + chklstaccheader.Items[i].Value;
                    }
                }
            }
            string set = "Header";
            if (ddlacctype.SelectedItem.Text == "Group Header")
            {
                set = "Group Header";
            }
            if (commcount == chklstaccheader.Items.Count)
            {
                txtaccheader.Text = "" + set + "(" + commcount.ToString() + ")";
                chkaccheader.Checked = true;
            }
            else if (commcount == 0)
            {
                txtaccheader.Text = "--Select--";
            }
            else
            {
                txtaccheader.Text = "" + set + "(" + commcount.ToString() + ")";
            }
        }
        catch
        {

        }
    }
    protected void chkaccheader_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlacctype.SelectedItem.Text == "Ledger")
            {
                if (chkaccheader.Checked == true)
                {
                    for (int remv = 0; remv < treeview_spreadfields.Nodes.Count; remv++)
                    {
                        treeview_spreadfields.Nodes[remv].Checked = true;
                        txtaccheader.Text = "Header(" + (treeview_spreadfields.Nodes.Count) + ")";
                        if (treeview_spreadfields.Nodes[remv].ChildNodes.Count > 0)
                        {
                            for (int child = 0; child < treeview_spreadfields.Nodes[remv].ChildNodes.Count; child++)
                            {
                                treeview_spreadfields.Nodes[remv].ChildNodes[child].Checked = true;
                            }
                        }
                    }
                }
                else
                {
                    for (int remv = 0; remv < treeview_spreadfields.Nodes.Count; remv++)
                    {
                        treeview_spreadfields.Nodes[remv].Checked = false;
                        txtaccheader.Text = "---Select---";
                        if (treeview_spreadfields.Nodes[remv].ChildNodes.Count > 0)
                        {
                            for (int child = 0; child < treeview_spreadfields.Nodes[remv].ChildNodes.Count; child++)
                            {
                                treeview_spreadfields.Nodes[remv].ChildNodes[child].Checked = false;
                            }
                        }
                    }
                }
            }
            else
            {
                if (chkaccheader.Checked == true)
                {
                    for (int i = 0; i < chklstaccheader.Items.Count; i++)
                    {
                        chklstaccheader.Items[i].Selected = true;
                    }
                    if (ddlacctype.SelectedItem.Text == "Group Header")
                    {
                        txtaccheader.Text = "Group Header (" + chklstaccheader.Items.Count + ")";
                    }
                    else
                    {
                        txtaccheader.Text = "Header (" + chklstaccheader.Items.Count + ")";
                    }
                }
                else
                {
                    for (int i = 0; i < chklstaccheader.Items.Count; i++)
                    {
                        chklstaccheader.Items[i].Selected = false;
                    }
                    txtaccheader.Text = "---Select---";
                }
            }
        }
        catch
        {
        }
    }
    public void BindBatch()
    {
        try
        {
            txtbatch.Text = "---Select---";
            chkbatch.Checked = false;
            ds2.Dispose();
            ds2.Reset();
            if (ddlstudtype.SelectedItem.Text == "EnRoll")
            {
                string strsql = "select max(batch_year) as Batch_year from applyn where batch_year<>'-1' and batch_year<>''";
                ds2 = da.select_method_wo_parameter(strsql, "Text");
            }
            else
            {
                ds2 = da.BindBatch();
            }
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklstbatch.DataSource = ds2;
                chklstbatch.DataTextField = "Batch_year";
                chklstbatch.DataValueField = "Batch_year";
                chklstbatch.DataBind();
                chklstbatch.SelectedIndex = 0;
                txtbatch.Text = "Batch (1)";
            }
        }
        catch
        {

        }
    }
    public void BindBranchMultiple(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            txtbranch.Text = "---Select---";
            chkbranch.Checked = false;
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    if (course_id == "")
                    {
                        course_id = "" + chklstdegree.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        course_id = course_id + "," + "" + chklstdegree.Items[i].Value.ToString() + "";
                    }
                }
            }
            chklstbranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            ds2 = da.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklstbranch.DataSource = ds2;
                chklstbranch.DataTextField = "dept_name";
                chklstbranch.DataValueField = "degree_code";
                chklstbranch.DataBind();
                chklstbranch.Items[0].Selected = true;
                txtbranch.Text = lblbranch.Text + " (1)";
            }
        }
        catch
        {

        }
    }
    protected void chkbranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkbranch.Checked == true)
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = true;
                    txtbranch.Text = lblbranch.Text + "(" + (chklstbranch.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = false;
                    txtbranch.Text = "---Select---";
                }
            }
        }
        catch
        {
        }
    }
    protected void chklstbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            pbranch.Focus();
            int branchcount = 0;
            chkbranch.Checked = false;
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    branchcount = branchcount + 1;
                }
            }
            if (branchcount == chklstbranch.Items.Count)
            {
                txtbranch.Text = lblbranch.Text + " (" + branchcount.ToString() + ")";
                chkbranch.Checked = true;
            }
            else if (branchcount == 0)
            {
                txtbranch.Text = "--Select--";
            }
            else
            {
                txtbranch.Text = lblbranch.Text + " (" + branchcount.ToString() + ")";
            }
        }
        catch
        {

        }
    }
    protected void chkdegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            if (chkdegree.Checked == true)
            {
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = true;
                    txtdegree.Text = lbldegree.Text + "(" + (chklstdegree.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = false;
                    txtdegree.Text = "---Select---";
                }
            }

            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        }
        catch
        {

        }
    }
    protected void chklstdegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            pdegree1.Focus();
            int degreecount = 0;
            chkdegree.Checked = false;
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    degreecount = degreecount + 1;
                }
            }
            if (degreecount == chklstdegree.Items.Count)
            {
                txtdegree.Text = lbldegree.Text + "(" + degreecount.ToString() + ")";
                chkdegree.Checked = true;
            }
            else if (degreecount == 0)
            {
                txtdegree.Text = "--Select--";
            }
            else
            {
                txtdegree.Text = lbldegree.Text + "(" + degreecount.ToString() + ")";
            }
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        }
        catch
        {
        }
    }
    protected void chkbatch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            if (chkbatch.Checked == true)
            {
                for (int i = 0; i < chklstbatch.Items.Count; i++)
                {
                    chklstbatch.Items[i].Selected = true;
                    txtbatch.Text = "Batch (" + (chklstbatch.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklstbatch.Items.Count; i++)
                {
                    chklstbatch.Items[i].Selected = false;
                    txtbatch.Text = "---Select---";
                }
            }
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        }
        catch
        {
        }
    }
    protected void chklstbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            pbatch.Focus();
            int batchcount = 0;
            chkbatch.Checked = false;
            for (int i = 0; i < chklstbatch.Items.Count; i++)
            {
                if (chklstbatch.Items[i].Selected == true)
                {
                    batchcount = batchcount + 1;
                }
            }
            if (batchcount == chklstbatch.Items.Count)
            {
                txtbatch.Text = "Batch (" + batchcount.ToString() + ")";
                chkbatch.Checked = true;
            }
            else if (batchcount == 0)
            {
                txtbatch.Text = "--Select--";
            }
            else
            {
                txtbatch.Text = "Batch (" + batchcount.ToString() + ")";
            }
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string from = txtdate.Text;
        string to = txtto.Text;
        string[] frdate = from.Split('/');
        if (frdate.Length == 3)
        {
            from = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
        }
        string[] tdate = to.Split('/');
        if (tdate.Length == 3)
        {
            to = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
        }
        string dt = "";
        if (chkdate.Checked == true)
            dt = "@ Date: " + from + " to " + to + " ";
        else
            dt = "";
        Printcontrol.Visible = true;
        string degreedetails = string.Empty;
        string hdnm = string.Empty;
        if (ddlfeetype.SelectedItem.Text == "Both")
        {
            hdnm = "Fee Detailed";
        }
        else
        {
            hdnm = ddlfeetype.SelectedItem.Text.ToString();
        }

        degreedetails = " Course Wise Student's Fee Report : " + hdnm + " List " + dt + "";
        string pagename = "Overall_student_Fee_Status.aspx";
        Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
    }
    protected void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("default.aspx", false);

    }

    protected void ddlstudtype_change(object sender, EventArgs e)
    {
        try
        {
            // BindBatch();
            if (ddlstudtype.SelectedItem.Text.Trim() == "Before Admission")
                cbbfadm.Enabled = false;
            else
                cbbfadm.Enabled = true;
        }
        catch
        {
        }

    }

    protected void ddlacctype_change(object sender, EventArgs e)
    {
        try
        {
            treeview_spreadfields.Visible = false;
            treeview_spreadfields.Nodes.Clear();
            if (ddlacctype.SelectedItem.Text == "Group Header")
            {
                Label1.Text = "G/p Header";
                groupheader();
                txtaccheader.Enabled = true;
                ddlfeetype.Enabled = true;
            }
            else if (ddlacctype.SelectedItem.Text == "Header")
            {
                Label1.Text = "A/c Header";
                loadheader();
                txtaccheader.Enabled = true;
                ddlfeetype.Enabled = true;
            }
            else
            {
                treeview_spreadfields.Visible = true;
                loadheader();
                Label1.Text = "A/c Header";
                txtaccheader.Enabled = true;
                ddlfeetype.Enabled = true;
            }
        }
        catch
        {

        }
    }
    public void groupheader()
    {
        try
        {
            chkaccheader.Checked = false;
            txtaccheader.Text = "---Select---";
            chklstaccheader.Items.Clear();
            //Modified by srinath 10/6/2015
            ds.Reset();
            ds.Dispose();
            string strheadid = "";
            //for (int t = 0; t < chklsfyear.Items.Count; t++)
            //{
            //    if (chklsfyear.Items[t].Selected == true)
            //    {
            //        if (strheadid == "")
            //        {
            //            strheadid = "'" + chklsfyear.Items[t].Value.ToString() + "'";
            //        }
            //        else
            //        {
            //            strheadid = strheadid + ",'" + chklsfyear.Items[t].Value.ToString() + "'";
            //        }
            //    }
            //}
            //string fnyr = "";
            strheadid = Convert.ToString(getCheckboxListValue(chklsfyear));
            //if (strheadid.Trim() != "")
            //{
            //    strheadid = " and a.acct_id in (" + strheadid + ")";
            //}
            if (strheadid.Trim() != "")
            {
                string straccheadquery = "select distinct ChlGroupHeader  from FS_ChlGroupHeaderSettings ";

                if (ddltype.Items.Count > 0 && ddltype.SelectedItem.Text != "Both")
                {
                    straccheadquery = straccheadquery + " where Stream='" + ddltype.SelectedItem.Text.ToString() + "'";
                    //string straccheadquery = "select distinct ChlHeaderName from Acctheader where ChlHeaderName IS NOT NULL and ChlHeaderName!='' ";
                    //ds = da.select_method_wo_parameter(straccheadquery, "Text");
                }
                //else
                //{
                //    //string straccheadquery = "select distinct ChlHeaderName from Acctheader where ChlHeaderName IS NOT NULL and ChlHeaderName!='' and HeaderType='" + ddltype.SelectedItem.Text.ToString() + "' ";
                //    //ds = da.select_method_wo_parameter(straccheadquery, "Text");
                //}
                ds = da.select_method_wo_parameter(straccheadquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chklstaccheader.DataSource = ds;
                    chklstaccheader.DataTextField = "ChlGroupHeader";
                    chklstaccheader.DataValueField = "ChlGroupHeader";
                    chklstaccheader.DataBind();

                    for (int i = 0; i < chklstaccheader.Items.Count; i++)
                    {
                        chklstaccheader.Items[i].Selected = true;
                    }
                    chkaccheader.Checked = true;
                    txtaccheader.Text = "Group Header (" + chklstaccheader.Items.Count + ")";
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void ddlfeetype_change(object sender, EventArgs e)
    {
        if (ddlfeetype.SelectedItem.Text == "Paid")
        {
            txtpaymode.Enabled = true;
        }
        else
        {
            txtpaymode.Enabled = false;
        }
        chkdate_CheckedChanged(sender, e);
    }
    protected void chkpaymode_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkpaymode.Checked == true)
            {
                for (int i = 0; i < chklstpaymode.Items.Count; i++)
                {
                    chklstpaymode.Items[i].Selected = true;
                    txtpaymode.Text = "Mode (" + (chklstpaymode.Items.Count) + ")";
                    //cbbankcheck.Enabled = true;
                }
            }
            else
            {
                for (int i = 0; i < chklstpaymode.Items.Count; i++)
                {
                    chklstpaymode.Items[i].Selected = false;
                    txtpaymode.Text = "---Select---";
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }

    }
    protected void chklstpaymode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int categorycount = 0;
            chkpaymode.Checked = false;
            for (int i = 0; i < chklstpaymode.Items.Count; i++)
            {
                if (chklstpaymode.Items[i].Selected == true)
                {
                    categorycount = categorycount + 1;
                    //if (chklstpaymode.Items[i].Text == "Online Pay")
                    //    cbbankcheck.Enabled = true;
                    //else
                    //    cbbankcheck.Enabled = false;
                }
            }
            if (categorycount == chklstpaymode.Items.Count)
            {

                txtpaymode.Text = "Mode (" + categorycount.ToString() + ")";
                chkpaymode.Checked = true;
            }
            else if (categorycount == 0)
            {
                txtpaymode.Text = "--Select--";
            }
            else
            {
                txtpaymode.Text = "Mode (" + categorycount.ToString() + ")";
            }
        }
        catch
        {
        }
    }

    public void loadtype()
    {
        try
        {
            // collegecode = Session["collegecode"].ToString();
            string clgvalue = ddl_collegename.SelectedItem.Value.ToString();
            ddltype.Items.Clear();
            string strquery = "select distinct type from course where college_code='" + clgvalue + "' and type is not null and type<>''";
            ds = da.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddltype.DataSource = ds;
                ddltype.DataTextField = "type";
                ddltype.DataBind();
                ddltype.Enabled = true;
                ddltype.Items.Insert(0, "Both");
            }
            else
            {
                ddltype.Enabled = false;
            }
        }
        catch
        {
        }
    }
    public void BindDegree()
    {
        try
        {
            string usercode = Session["usercode"].ToString();
            // collegecode = Session["collegecode"].ToString();
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            txtdegree.Text = "---Select---";
            chkdegree.Checked = false;
            string type = "";
            if (ddltype.Enabled == true)
            {
                if (ddltype.Items.Count > 0)
                {
                    if (ddltype.SelectedItem.ToString() != "Both" && ddltype.SelectedItem.ToString() != "")
                    {
                        type = " and course.type='" + ddltype.SelectedItem.ToString() + "'";
                    }
                }
            }

            chklstdegree.Items.Clear();
            string codevalues = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                codevalues = "and group_code='" + group_user + "'";
            }
            else
            {
                codevalues = "and user_code='" + usercode + "'";
            }
            ds2.Dispose();
            ds2.Reset();
            string strquery = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where  course.course_id=degree.course_id and course.college_code = degree.college_code and  degree.college_code='" + collegecode + "' and deptprivilages.Degree_code=degree.Degree_code " + codevalues + " " + type + " ";
            ds2 = da.select_method_wo_parameter(strquery, "Text");
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklstdegree.DataSource = ds2;
                chklstdegree.DataTextField = "course_name";
                chklstdegree.DataValueField = "course_id";
                chklstdegree.DataBind();
                chklstdegree.Items[0].Selected = true;
                loadheader();
                txtdegree.Text = lbldegree.Text + "(1)";
            }
        }
        catch
        {
        }
    }
    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_collegename.Items.Count > 0)
        {
            collegecode = Convert.ToString(ddl_collegename.SelectedItem.Value);
        }
        BindBatch();
        BindDegree();
        BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        treeview_spreadfields.Visible = false;
        treeview_spreadfields.Nodes.Clear();
        if (ddlacctype.SelectedItem.Text == "Group Header")
        {
            Label1.Text = "G/p Header";
            groupheader();
            txtaccheader.Enabled = true;
            ddlfeetype.Enabled = true;

        }
        else if (ddlacctype.SelectedItem.Text == "Header")
        {
            Label1.Text = "A/c Header";
            loadheader();
            txtaccheader.Enabled = true;
            ddlfeetype.Enabled = true;
        }
        else
        {
            treeview_spreadfields.Visible = true;
            loadheader();
            Label1.Text = "A/c Header";
            txtaccheader.Enabled = true;
            ddlfeetype.Enabled = true;
        }

    }
    public void loadpaid()
    {
        try
        {
            Dictionary<int, Double> dicdegree = new Dictionary<int, Double>();
            Dictionary<int, Double> dicdate = new Dictionary<int, Double>();
            Dictionary<int, Double> dicgrandtotal = new Dictionary<int, Double>();
            Dictionary<string, string> diccourse = new Dictionary<string, string>();
            Dictionary<string, string> dicstupaidfee = new Dictionary<string, string>();

            DataView dvhead = new DataView();
            DataView dvdatestud = new DataView();
            DataView dv_paid_data = new DataView();

            ArrayList arrayfeecate = new ArrayList();

            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.DoubleCellType dobcell = new DoubleCellType();

            DateTime dat = DateTime.ParseExact(txtdate.Text.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime dat1 = DateTime.ParseExact(txtto.Text.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

            if (ddl_collegename.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            string y1 = dat.ToString("yyyy-MM-dd");
            string y2 = dat1.ToString("yyyy-MM-dd");
            fill_Studheader();
            string batch_all = string.Empty;
            string course_all = string.Empty;
            string header_all = string.Empty;
            string category = string.Empty;
            string feecode = "";
            string tot_category = string.Empty;
            int daterowstart = 0;
            string paidquery = "";
            string str = "";
            string semestercate = "";
            string regsemcater = "";
            string degreequery = "";
            string degrqry = "";
            string batchquery = "";
            string feesquery = "";

            Double Cashtotal = 0;
            Double checktotal = 0;
            Double Challantotal = 0;
            Double ddtotal = 0;
            Double onlinetotal = 0;
            Double cardtotal = 0;
            for (int b = 0; b < chklstbatch.Items.Count; b++)
            {
                if (chklstbatch.Items[b].Selected == true)
                {
                    if (batch_all == "")
                    {
                        batch_all = chklstbatch.Items[b].Value.ToString();
                    }
                    else
                    {

                        batch_all = batch_all + "," + chklstbatch.Items[b].Value.ToString();
                    }
                }
            }
            if (batch_all.Trim() != "" && batch_all != null)
            {
                batchquery = " and r.batch_year in(" + batch_all + ")";
            }
            for (int c = 0; c < chklstbranch.Items.Count; c++)
            {
                if (chklstbranch.Items[c].Selected == true)
                {
                    if (course_all == "")
                    {
                        course_all = "'" + chklstbranch.Items[c].Value.ToString() + "'";
                    }
                    else
                    {
                        course_all = course_all + ",'" + chklstbranch.Items[c].Value.ToString() + "'";
                    }
                }
            }
            if (course_all.Trim() != "" && course_all != null)
            {
                degreequery = " and r.degree_code in(" + course_all + ") ";
                degrqry = "degree_code in(" + course_all + ") ";
            }

            for (int a = 0; a < chklstaccheader.Items.Count; a++)
            {
                if (chklstaccheader.Items[a].Selected == true)
                {
                    if (header_all == "")
                    {
                        header_all = chklstaccheader.Items[a].Value.ToString();
                    }
                    else
                    {
                        header_all = header_all + "','" + chklstaccheader.Items[a].Value.ToString();
                    }
                }
            }

            int ledgercount = 0;


            for (int remv = 0; remv < treeview_spreadfields.Nodes.Count; remv++)
            {
                if (header_all == "")
                {
                    header_all = treeview_spreadfields.Nodes[remv].Value.ToString();
                }
                else
                {
                    header_all = header_all + "','" + treeview_spreadfields.Nodes[remv].Value.ToString();
                }
                if (treeview_spreadfields.Nodes[remv].ChildNodes.Count > 0)
                {
                    for (int child = 0; child < treeview_spreadfields.Nodes[remv].ChildNodes.Count; child++)
                    {
                        if (category == "")
                        {
                            ledgercount++;
                            category = treeview_spreadfields.Nodes[remv].ChildNodes[child].Value.ToString();
                        }
                        else
                        {
                            ledgercount = ledgercount + 1;
                            category = category + "," + treeview_spreadfields.Nodes[remv].ChildNodes[child].Value.ToString();
                        }
                    }
                }
            }
            regsemcater = ",d.feecategory";
            Boolean semflag = false;
            string semester1 = "";

            for (int fs = 0; fs < chklsfeesem.Items.Count; fs++)
            {
                if (chklsfeesem.Items[fs].Selected == true)
                {
                    arrayfeecate.Add(chklsfeesem.Items[fs].Text.ToString() + '^' + chklsfeesem.Items[fs].Value.ToString());
                    semflag = true;
                    if (semester1 == "")
                    {
                        semester1 = chklsfeesem.Items[fs].Value.ToString();
                    }
                    else
                    {
                        semester1 = semester1 + "," + chklsfeesem.Items[fs].Value.ToString();
                    }
                }
            }


            if (semester1 != "")
            {
                semester1 = " and d.feecategory in (" + semester1 + ")";
                semestercate = ",d.feecategory";

            }
            else
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select Any One Semester";
                return;
            }

            string strorder = "";

            string delflg = "";
            if (checkdicon.Checked == true)
            {
                // delflg = "";
            }
            else
            {
                //  delflg = "and r.delflag=0";
            }
            string cc = "";
            string debar = "";
            string disc = "";
            if (cblinclude.Items.Count > 0)
            {
                for (int i = 0; i < cblinclude.Items.Count; i++)
                {
                    if (cblinclude.Items[i].Selected == true)
                    {
                        if (cblinclude.Items[i].Value == "1")
                        {
                            cc = " r.cc=1";
                        }
                        if (cblinclude.Items[i].Value == "2")
                        {
                            debar = " r.Exam_Flag like 'debar'";
                        }
                        if (cblinclude.Items[i].Value == "3")
                        {
                            disc = " r.DelFlag=1";
                        }
                    }
                }
            }


            if (cc != "" && debar == "" && disc == "")
                delflg = " and (" + cc + " or r.cc=0)  and r.Exam_Flag<>'debar' and r.DelFlag=0";

            if (cc == "" && debar != "" && disc == "")
                delflg = " and r.cc=0  and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";

            if (cc == "" && debar == "" && disc != "")
                delflg = " and r.cc=0  and r.Exam_Flag<>'debar' and (" + disc + " or r.DelFlag=0)";

            if (cc != "" && debar != "" && disc == "")
                delflg = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";

            if (cc == "" && debar != "" && disc != "")
                delflg = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar')  and (" + disc + " or r.DelFlag=0)";

            if (cc != "" && debar == "" && disc != "")
                delflg = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar'  and (" + disc + " or r.DelFlag=0)";

            else if (cc == "" && debar == "" && disc == "")
                delflg = " and r.cc=0  and r.Exam_Flag<>'debar' and r.DelFlag=0";

            if (cc != "" && debar != "" && disc != "")
                delflg = "";

            string bankcode = "";
            string bkcode = "";
            for (int i = 0; i < chklstpaymode.Items.Count; i++)
            {
                if (chklstpaymode.Items[i].Selected == true)
                {
                    if (chklstpaymode.Items[i].Text == "Online Pay")
                    {
                        for (int bk = 0; bk < cblbank.Items.Count; bk++)
                        {
                            if (cblbank.Items[bk].Selected == true)
                            {
                                if (bankcode == "")
                                    bankcode = "" + cblbank.Items[bk].Value.ToString() + "";
                                else
                                    bankcode = bankcode + "','" + cblbank.Items[bk].Value.ToString() + "";
                            }
                        }
                    }
                    else
                        bankcode = "";
                }
                else
                    bankcode = "";
            }
            if (bankcode != "")
                bkcode = " and d.DDBankCode in ('" + bankcode + "')";
            else
                bkcode = "";



            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.BackColor = Color.FromArgb(214, 235, 255);
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 3;
            FpSpread1.Sheets[0].ColumnHeader.Columns.Count = 7;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Student Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";

            if (checkSchoolSetting() == 0)
            {
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Admission No";
                sclflag = true;
            }
            else
            {
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg No";
                sclflag = false;
            }

            if (roll == 0)
            {
                FpSpread1.Sheets[0].Columns[2].Visible = true;
                FpSpread1.Sheets[0].Columns[3].Visible = true;
            }
            else if (roll == 1)
            {
                FpSpread1.Sheets[0].Columns[2].Visible = true;
                FpSpread1.Sheets[0].Columns[3].Visible = true;
            }
            else if (roll == 2)
            {
                FpSpread1.Sheets[0].Columns[2].Visible = true;
                FpSpread1.Sheets[0].Columns[3].Visible = false;
            }
            else if (roll == 3)
            {
                FpSpread1.Sheets[0].Columns[2].Visible = false;
                FpSpread1.Sheets[0].Columns[3].Visible = true;
            }
            else if (roll == 4)
            {
                FpSpread1.Sheets[0].Columns[2].Visible = false;
                FpSpread1.Sheets[0].Columns[3].Visible = true;
            }
            else if (roll == 5)
            {
                FpSpread1.Sheets[0].Columns[2].Visible = true;
                FpSpread1.Sheets[0].Columns[3].Visible = true;
            }
            else if (roll == 6)
            {
                FpSpread1.Sheets[0].Columns[2].Visible = false;
                FpSpread1.Sheets[0].Columns[3].Visible = true;
            }
            else if (roll == 7)
            {
                FpSpread1.Sheets[0].Columns[2].Visible = true;
                FpSpread1.Sheets[0].Columns[3].Visible = true;
            }


            // FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = lbldegree.Text;
            if (semflag == true)
            {
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = lblfeesem.Text;
                FpSpread1.Sheets[0].Columns[5].Width = 100;
            }
            else
            {
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = lblfeesem.Text;
            }
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Challan/Reciept No";
            FpSpread1.Sheets[0].Columns[6].Visible = true;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 3, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 3, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 3, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 3, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 3, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 3, 1);
            FpSpread1.Sheets[0].Columns[2].CellType = txt;

            FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            string feecodeall = "";

            if (ddlacctype.SelectedItem.Text == "Ledger")
            {
                semestercate = ",d.headerfk,d.ledgerfk";

                for (int remv = 0; remv < treeview_spreadfields.Nodes.Count; remv++)
                {
                    Boolean headflag = false;
                    int ccount = 0;
                    int hstartc = 0;
                    int ledcount = 0;
                    Boolean headerviewflag = false;
                    if (treeview_spreadfields.Nodes[remv].ChildNodes.Count > 0)
                    {
                        for (int child = 0; child < treeview_spreadfields.Nodes[remv].ChildNodes.Count; child++)
                        {
                            if (treeview_spreadfields.Nodes[remv].ChildNodes[child].Checked == true)
                            {
                                headerviewflag = true;
                                string headervalue = treeview_spreadfields.Nodes[remv].Value.ToString();

                                FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 1;
                                if (headflag == false)
                                {
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = treeview_spreadfields.Nodes[remv].Text.ToString();

                                    headflag = true;
                                    hstartc = FpSpread1.Sheets[0].ColumnCount - 1;

                                }
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Note = headervalue;
                                ledcount++;
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1, 1, 1);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = treeview_spreadfields.Nodes[remv].ChildNodes[child].Text.ToString();
                                feecode = treeview_spreadfields.Nodes[remv].ChildNodes[child].Value.ToString();
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Paid";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Note = feecode;

                                ccount += 1;
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - ccount, 1, ccount);

                                if (feecodeall == "")
                                {
                                    feecodeall = feecode;
                                }
                                else
                                {
                                    feecodeall = feecodeall + ',' + feecode;
                                }
                            }
                        }
                    }
                    if (headerviewflag == true)
                    {
                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = treeview_spreadfields.Nodes[remv].Text.ToString() + " Total Paid";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].BackColor = Color.MistyRose;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Note = treeview_spreadfields.Nodes[remv].Text.ToString();
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1, 3, 1);
                    }
                }
                FpSpread1.Sheets[0].ColumnCount++;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Total Paid";
                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Note = "Total Paid";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1, 3, 1);
                if (feecodeall.Trim() != "" && feecodeall != null)
                {
                    feesquery = " and d.ledgerfk in(" + feecodeall + ")";
                }
            }
            else if (ddlacctype.SelectedItem.Text == "Group Header")
            {
                semestercate = semestercate + ",f.header_id";
                string acchead = "select distinct a.header_name,c.ChlHeaderName,a.header_id from chlheadersettings c,Acctheader a where c.Header_ID=a.header_id and a.header_name not in ('arrear')";
                ds.Dispose();
                ds = da.select_method_wo_parameter(acchead, "text");

                for (int j = 0; j < chklstaccheader.Items.Count; j++)
                {
                    if (chklstaccheader.Items[j].Selected == true)
                    {
                        string grhead = chklstaccheader.Items[j].Text.ToString();
                        ds.Tables[0].DefaultView.RowFilter = " ChlHeaderName='" + grhead + "'";
                        dvhead = ds.Tables[0].DefaultView;
                        int ccount = 0;
                        int hstartc = 0;

                        Boolean headflag = false;
                        for (int a = 0; a < dvhead.Count; a++)
                        {
                            FpSpread1.Sheets[0].ColumnCount++;
                            if (headflag == false)
                            {
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = chklstaccheader.Items[j].Text;
                                headflag = true;
                                hstartc = FpSpread1.Sheets[0].ColumnCount - 1;
                            }
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                feecode = dvhead[a]["header_id"].ToString();

                                FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Width = 200;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = dvhead[a]["header_name"].ToString();
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Paid";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Note = feecode;
                                if (feecodeall == "")
                                {
                                    feecodeall = feecode;
                                }
                                else
                                {
                                    feecodeall = feecodeall + "," + feecode;
                                }
                            }
                            ccount += 1;
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - ccount, 1, ccount);

                            //...........................End............
                        }
                    }
                }

                if (ddlfeetype.Text == "Paid")//total header binding.......................
                {
                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Total Paid";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Note = "Total Paid";
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1, 3, 1);
                }
                if (feecodeall.Trim() != "" && feecodeall != null)
                {
                    feesquery = " and f.header_id in(" + feecodeall + ")";
                }
            }
            else//Header..............
            {
                semestercate = ",d.headerfk";

                for (int a = 0; a < chklstaccheader.Items.Count; a++)
                {
                    if (chklstaccheader.Items[a].Selected == true)
                    {
                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Width = 200;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = chklstaccheader.Items[a].Text;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Paid";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Note = chklstaccheader.Items[a].Value;
                        feecode = chklstaccheader.Items[a].Value;
                        if (feecodeall == "")
                        {
                            feecodeall = chklstaccheader.Items[a].Value;
                        }
                        else
                        {
                            feecodeall = feecodeall + "," + chklstaccheader.Items[a].Value;
                        }

                    }
                }
                FpSpread1.Sheets[0].ColumnCount++;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Total Paid";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Note = "Total Paid";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1, 2, 1);

                if (feecodeall.Trim() != "" && feecodeall != null)
                {
                    feesquery = " and d.headerfk in(" + feecodeall + ")";
                }
            }

            string modestatus = "";
            #region old
            //for (int mode = 0; mode < chklstpaymode.Items.Count; mode++)
            //{
            //    if (chklstpaymode.Items[mode].Selected == true)
            //    {
            //        if (chklstpaymode.Items[mode].Text == "Cash")
            //        {
            //            if (modestatus == "")
            //            {
            //                modestatus = "1";
            //            }
            //            else
            //            {
            //                modestatus = modestatus + "','" + "1";
            //            }
            //        }
            //        if (chklstpaymode.Items[mode].Text == "Cheque")
            //        {
            //            if (modestatus == "")
            //            {
            //                modestatus = "2";
            //            }
            //            else
            //            {
            //                modestatus = modestatus + "','" + "2";
            //            }
            //        }
            //        if (chklstpaymode.Items[mode].Text == "DD")
            //        {
            //            if (modestatus == "")
            //            {
            //                modestatus = "3";
            //            }
            //            else
            //            {
            //                modestatus = modestatus + "','" + "3";
            //            }
            //        }
            //        if (chklstpaymode.Items[mode].Text == "Challan")
            //        {
            //            if (modestatus == "")
            //            {
            //                modestatus = "4";
            //            }
            //            else
            //            {
            //                modestatus = modestatus + "','" + "4";
            //            }
            //        }
            //        if (chklstpaymode.Items[mode].Text == "Online Pay")
            //        {
            //            if (modestatus == "")
            //            {
            //                modestatus = "5";
            //            }
            //            else
            //            {
            //                modestatus = modestatus + "','" + "5";
            //            }
            //        }
            //    }
            //}
            #endregion
            for (int mode = 0; mode < chklstpaymode.Items.Count; mode++)
            {
                if (chklstpaymode.Items[mode].Selected == true)
                {
                    if (modestatus == "")
                    {
                        modestatus = Convert.ToString(chklstpaymode.Items[mode].Value);
                    }
                    else
                    {
                        modestatus = modestatus + "','" + Convert.ToString(chklstpaymode.Items[mode].Value);
                    }
                }
            }
            if (modestatus.Trim() != "")
            {
                modestatus = " and d.paymode in ('" + modestatus + "')";
            }

            string finyearval = "";

            for (int i = 0; i < chklsfyear.Items.Count; i++)
            {
                if (chklsfyear.Items[i].Selected == true)
                {
                    if (finyearval == "")
                    {
                        finyearval = "'" + chklsfyear.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        finyearval = finyearval + ",'" + chklsfyear.Items[i].Value.ToString() + "'";
                    }
                }
            }
            if (finyearval.Trim() != "")
            {
                finyearval = " and d.FinYearFK in(" + finyearval + ")";
            }
            //old

            //  paidquery = " Select stud_name as  Name,r.Roll_No,r.Reg_No,sum(d.debit) as paid,d.paymode mode,d.TransDate cal_date,d.headerfk,isnull(d.ddno,'') as challan_no,d.TransCode as ReceiptNo,f.headerfk" + semestercate + " from ft_findailytransaction d,fm_ledgermaster f,registration r where d.ledgerfk=f.ledgerpk  and f.headerfk=d.Headerfk  and d.app_no = r.app_no  and f.ledgername <>'Excess Amount' and d.TransDate between '" + dat.ToString("MM/dd/yyyy") + "' and '" + dat1.ToString("MM/dd/yyyy") + "' " + batchquery + " " + degreequery + " " + modestatus + " " + feesquery + " " + semester1 + " " + finyearval + " and credit = 0 and transtype =1 and d.debit>0 and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and r.college_code ='" + collegecode + "' group by r.stud_name,d.paymode,d.TransDate,d.headerfk,d.ddno,f.headerfk,d.TransCode,r.Roll_No,r.Reg_No" + semestercate + "";

            //new 
            if (ddlstudtype.SelectedItem.Text.Trim() != "Before Admission")
            {
                paidquery = " select distinct r.stud_name as Name,r.Current_Semester, r.Roll_No,r.roll_admit,r.Reg_No,sum(d.debit) as paid,r.degree_code ,d.paymode as mode " + semestercate + " ,feecategory,d.TransCode as ReceiptNo,d.ddno as challan_no ,d.TransDate cal_date,d.feecategory from ft_findailytransaction d,registration r where  d.app_no = r.app_no     " + feesquery + "   " + degreequery + "  and r.Batch_year in(" + batch_all + ")  " + semester1 + "  " + modestatus + "  " + finyearval + " and d.TransDate between '" + dat.ToString("MM/dd/yyyy") + "' and '" + dat1.ToString("MM/dd/yyyy") + "' " + bkcode + " and credit=0 and transtype=1 and d.debit>0  and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'  " + delflg + " and r.college_code ='" + collegecode + "'";
                if (usBasedRights == true)
                    paidquery += " and d.EntryUserCode in('" + usercode + "')";
                paidquery += " group by r.roll_no,r.roll_admit,r.Reg_No,r.stud_name,r.degree_code,r.Current_Semester,d.paymode  " + semestercate + ",feecategory,d.TransCode,d.ddno,d.TransDate ,d.feecategory ";
                if (cbbfadm.Checked == true)
                {
                    paidquery = paidquery + " union select distinct r.stud_name as Name,r.Current_Semester,r.app_formno as Roll_No,r.app_formno as Reg_No,r.app_formno as roll_admit,sum(d.debit) as paid,r.degree_code ,d.paymode as mode " + semestercate + " ,feecategory,d.TransCode as ReceiptNo,d.ddno as challan_no ,d.TransDate cal_date,d.feecategory from ft_findailytransaction d,applyn r  where d.app_no = r.app_no     " + feesquery + "   " + degreequery + "  and r.Batch_year in(" + batch_all + ")  " + semester1 + "  " + modestatus + "  " + finyearval + " and d.TransDate between '" + dat.ToString("MM/dd/yyyy") + "' and '" + dat1.ToString("MM/dd/yyyy") + "' " + bkcode + " and credit=0 and transtype=1 and d.debit>0  and r.isconfirm = 1 and r.admission_status = '1' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(is_enroll,'0')<>'2'   and r.college_code ='" + collegecode + "'";
                    //and r.app_no not in (select app_no from Registration  where " + degrqry + "  and Batch_year in(" + batch_all + ") )
                    if (usBasedRights == true)
                        paidquery += " and d.EntryUserCode in('" + usercode + "')";
                    paidquery += "  group by r.app_no,r.app_formno,r.stud_name,r.degree_code,r.Current_Semester,d.paymode " + semestercate + ",feecategory,d.TransCode,d.ddno,d.feecategory,d.TransDate ";
                }
            }
            else
            {
                paidquery = " select distinct r.stud_name as Name,r.Current_Semester,r.app_formno as Roll_No,r.app_formno as Reg_No,r.app_formno as roll_admit,sum(d.debit) as paid,r.degree_code ,d.paymode as mode " + semestercate + " ,feecategory,d.TransCode as ReceiptNo,d.ddno as challan_no ,d.TransDate cal_date,d.feecategory from ft_findailytransaction d,applyn r  where d.app_no = r.app_no     " + feesquery + "   " + degreequery + "  and r.Batch_year in(" + batch_all + ")  " + semester1 + "  " + modestatus + "  " + finyearval + " and d.TransDate between '" + dat.ToString("MM/dd/yyyy") + "' and '" + dat1.ToString("MM/dd/yyyy") + "' " + bkcode + " and credit=0 and transtype=1 and d.debit>0  and r.isconfirm = 1 and r.admission_status = '1' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'  and r.app_no not in (select app_no from Registration  where " + degrqry + "  and Batch_year in(" + batch_all + ") ) and r.college_code ='" + collegecode + "'";
                if (usBasedRights == true)
                    paidquery += " and d.EntryUserCode in('" + usercode + "')";
                paidquery += "  group by r.app_no,r.app_formno,r.stud_name,r.degree_code,r.Current_Semester,d.paymode " + semestercate + ",feecategory,d.TransCode,d.ddno,d.feecategory,d.TransDate ";
            }
            if (ddlacctype.SelectedItem.Text == "Ledger")
            {
                paidquery = paidquery + ",d.ledgerfk ";
            }
            // paidquery = " Select Substring(Name, 1,Charindex('-', Name)-1) as roll_no,Substring(Name, Charindex('-', Name)+1, LEN(Name)) as  Name,sum(d.credit) as paid,d.mode,d.cal_date,d.header_id,isnull(d.challan_no,'') as challan_no" + semestercate + " from dailytransaction d,fee_info f where d.fee_code=f.fee_code  and f.header_id=d.Header_ID and f.fee_type<>'Excess Amount' and d.cal_date between '" + y1 + "' and '" + y2 + "' " + modestatus + " " + feesquery + " " + semester1 + " and debit=0 and vouchertype=1 and d.credit>0 group by d.name,d.mode,d.cal_date,d.header_id,d.challan_no" + semestercate + "";
            feesquery = feesquery + "  " + finyearval + "  and credit=0 and transtype =1 and d.debit>0  " + modestatus + " ";
            //and f.ledgername<>'Excess Amount'
            //order by isnull(l.priority,1000), l.ledgerName asc
            strorder = " ORDER BY cal_date,Batch_Year,degree_code,Current_Semester,Roll_No,name,d.ddno";
            //,isnull(f.priority,1000), f.ledgerName asc
            if (ddlstudtype.SelectedItem.Text == "Regular" || ddlstudtype.SelectedItem.Text == "Lateral" || ddlstudtype.SelectedItem.Text == "Transfer")
            {
                str = "select distinct r.stud_name as name,r.Current_Semester,r.Reg_No,r.Roll_No,r.roll_admit,r.degree_code,r.roll_admit,r.batch_year,d.transdate cal_date,d.ddno as challan_no " + regsemcater + " from registration r,ft_findailytransaction d,fm_ledgermaster f where f.ledgerpk=d.ledgerfk  and f.headerfk=d.Headerfk  " + delflg + " and r.mode='" + ddlstudtype.SelectedValue + "'  and d.app_no = r.app_no and d.transdate between '" + dat.ToString("MM/dd/yyyy") + "' and '" + dat1.ToString("MM/dd/yyyy") + "'";
                if (usBasedRights == true)
                    str += " and d.EntryUserCode in('" + usercode + "')";
                str += " and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and r.college_code ='" + collegecode + "'  " + batchquery + "  " + degreequery + " " + feesquery + "";
            }
            else if (ddlstudtype.SelectedItem.Text == "Re-admit")
            {
                str = " select distinct r.stud_name as Name,r.Current_Semester,r.Reg_No,r.Roll_No,r.degree_code,roll_admit,r.batch_year,d.transdate cal_date,d.ddno as challan_no " + regsemcater + " from Readmission a,Registration r where a.App_no=r.App_No  and r.cc=0  and r.delflag=0 and r.college_code ='" + collegecode + "' and r.exam_flag<>'debar'  and a.newbatch_year in(" + batch_all + ") " + degreequery + " " + feesquery + "";//Modify By M.SakthiPriya 04/02/2015
            }
            else if (ddlstudtype.SelectedItem.Text == "Re-join")
            {
                str = "select distinct r.stud_name as Name,r.Current_Semester,r.Reg_No,r.Roll_No,r.degree_code,roll_admit,r.batch_year,d.transdate cal_date,d.ddno as challan_no " + regsemcater + " from rejoin_list a,Registration r where a.roll_no=r.Roll_No   and r.delflag=0 and r.college_code ='" + collegecode + "' and r.exam_flag<>'debar' and a.newBatchYear in(" + batch_all + ") " + degreequery + " " + feesquery + "";//Modify By M.SakthiPriya 04/02/2015
            }
            else if (ddlstudtype.SelectedItem.Text == "EnRoll")
            {
                str = "select distinct r.stud_name as Name,r.Current_Semester,'' as Reg_No,r.app_formno as Roll_No,r.app_formno as roll_admit,r.degree_code,'' as roll_admit,r.batch_year,d.transdate cal_date,d.ddno as challan_no,d.TransCode as ReceiptNo  " + regsemcater + " from applyn r,ft_findailytransaction D where  r.app_no = D.app_no and r.isconfirm=1 and isnull(admission_status,0) = '1' and r.is_enroll=1 and r.isconfirm = 1";
                if (usBasedRights == true)
                    str += " and d.EntryUserCode in('" + usercode + "')";
                str += " and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and r.admission_status = 0 and r.college_code ='" + collegecode + "'  " + degreequery + " " + batchquery + "  " + feesquery + "";
            }
            else
            {
                //old query
                if (ddlstudtype.SelectedItem.Text.Trim() != "Before Admission")
                {
                    str = "select distinct r.stud_name as name,r.Current_Semester,r.Reg_No,r.Roll_No,r.degree_code,r.roll_admit,r.batch_year,d.transdate cal_date,d.TransCode as ReceiptNo,d.ddno as challan_no " + regsemcater + "  from registration r,ft_findailytransaction d where d.app_no = r.app_no " + delflg + " and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and r.college_code ='" + collegecode + "'";
                    if (usBasedRights == true)
                        str += " and d.EntryUserCode in('" + usercode + "')";
                    str += "  and d.transdate between '" + dat.ToString("MM/dd/yyyy") + "' and '" + dat1.ToString("MM/dd/yyyy") + "'  " + batchquery + "  " + degreequery + " " + feesquery + "";
                }
                else
                {
                    //arrear list
                    str = " select distinct r.stud_name as name,r.Current_Semester,'' as Reg_No,r.app_formno as Roll_No,r.degree_code,'' as roll_admit,r.batch_year,d.transdate cal_date,d.TransCode as ReceiptNo,d.ddno as challan_no" + regsemcater + " from applyn r,ft_findailytransaction d where  isconfirm = 1 and admission_status = '1' and d.app_no = r.app_no and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'and r.college_code ='" + collegecode + "'";
                    if (usBasedRights == true)
                        str += " and d.EntryUserCode in('" + usercode + "')";
                    str += "  and isnull(is_enroll,'0')<>'2'  and d.transdate between '" + dat.ToString("MM/dd/yyyy") + "' and '" + dat1.ToString("MM/dd/yyyy") + "'  " + batchquery + "  " + degreequery + " " + feesquery + "";
                    //and r.app_no not in (select app_no from Registration  where " + degrqry + "  and Batch_year in(" + batch_all + ") )
                }
            }
            if (cbbfadm.Checked == true)
            {
                str = str + " union all   select distinct r.stud_name as name,r.Current_Semester,'' as Reg_No,r.app_formno as Roll_No,r.degree_code,'' as roll_admit,r.batch_year,d.transdate cal_date,d.TransCode as ReceiptNo,d.ddno as challan_no" + regsemcater + " from applyn r,ft_findailytransaction d where isconfirm = 1 and admission_status = '1' and d.app_no = r.app_no and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'and r.college_code ='" + collegecode + "'";
                if (usBasedRights == true)
                    str += " and d.EntryUserCode in('" + usercode + "')";
                str += "  and isnull(is_enroll,'0')<>'2'  and d.transdate between '" + dat.ToString("MM/dd/yyyy") + "' and '" + dat1.ToString("MM/dd/yyyy") + "'  " + batchquery + "  " + degreequery + " " + feesquery + "";
                //and r.app_no not in (select app_no from Registration  where " + degrqry + "  and Batch_year in(" + batch_all + ") )
            }
            if (bkcode != "")
                str += bkcode;
            if (str != "")
                str += strorder;
            Dictionary<string, Double> diccurarrfee = new Dictionary<string, double>();

            // string strr = " select distinct r.stud_name as name,r.Current_Semester,'' as Reg_No,r.app_formno as Roll_No,r.degree_code,'' as roll_admit,r.batch_year,d.transdate cal_date,d.TransCode as ReceiptNo,d.ddno as challan_no,d.feecategory from applyn r,ft_findailytransaction d,fm_ledgermaster f where f.ledgerpk=d.ledgerfk  and f.headerfk=d.Headerfk and isconfirm = 1 and admission_status = '1' and d.app_no = r.app_no and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'and r.college_code ='13' and d.transdate between '02/01/2016' and '06/16/2016'   and r.batch_year in(2016)   and r.degree_code in('82')   and f.headerfk in(9,11,12,13,14,15,16,17)   and d.FinYearFK in('2','3') and f.ledgername<>'Excess Amount' and credit=0 and transtype =1 and d.debit>0   and d.paymode in ('1','2','3','4','5')   ORDER BY cal_date,Batch_Year,degree_code,Current_Semester,Roll_No,name,d.ddno";
            ds1.Reset();
            ds1.Dispose();
            ds1 = da.select_method_wo_parameter(paidquery, "Text");

            ds3.Reset();
            ds3.Dispose();
            ds3 = da.select_method_wo_parameter(str, "text");
            // ds3 = da.select_method_wo_parameter(strr, "text");

            ArrayList alsno = new ArrayList();
            int sno = 0;
            int col = 0;
            bool colval = false;
            if (ds1.Tables[0].Rows.Count > 0)
            {
                string degquery = "select c.course_id,c.course_name,r.degree_code,r.acronym,e.Dept_Name from course c,degree r,Department e where c.course_id=r.course_id and r.Dept_Code=e.Dept_Code " + degreequery + " ";
                ds2.Reset();
                ds2.Dispose();
                ds2 = da.select_method_wo_parameter(degquery, "Text");
                for (int di = 0; di < ds2.Tables[0].Rows.Count; di++)
                {
                    diccourse.Add(ds2.Tables[0].Rows[di]["degree_code"].ToString(), ds2.Tables[0].Rows[di]["course_name"].ToString() + "-" + ds2.Tables[0].Rows[di]["Dept_Name"].ToString());
                }

                for (DateTime dtdatec = Convert.ToDateTime(y1); dtdatec <= Convert.ToDateTime(y2); dtdatec = dtdatec.AddDays(1))
                {
                    string semfiletr = "";
                    int totsem = 0;
                    if (semflag == true)
                    {
                        totsem = arrayfeecate.Count - 1;
                    }
                    Boolean rowflag = false;
                    string feesem = "";
                    Boolean dataset = false;
                    string tempdegreee = "";
                    for (int add = 0; add <= totsem; add++)
                    {
                        if (semflag == true)
                        {
                            string getfeecate = arrayfeecate[add].ToString();
                            string[] spvfee = getfeecate.Split('^');
                            if (spvfee.GetUpperBound(0) >= 0)
                            {
                                semfiletr = " and feecategory=" + spvfee[1].ToString() + "";
                                feesem = spvfee[0].ToString();
                            }
                        }
                        ds3.Tables[0].DefaultView.RowFilter = " cal_date='" + dtdatec + "' " + semfiletr + "";
                        dvdatestud = ds3.Tables[0].DefaultView;
                        int decostar = 0;
                        if (dvdatestud.Count > 0)
                        {
                            daterowstart = FpSpread1.Sheets[0].RowCount + 1;

                            if (dataset == false)
                            {
                                FpSpread1.Sheets[0].RowCount++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = dtdatec.ToString("dd/MM/yyyy");
                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, FpSpread1.Sheets[0].Columns.Count);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = Color.PowderBlue;
                            }
                            dataset = true;
                            string deg_dept = string.Empty;
                            for (int fe = 0; fe < dvdatestud.Count; fe++)
                            {
                                string courseid = dvdatestud[fe]["degree_code"].ToString();
                                string batcyear = dvdatestud[fe]["batch_year"].ToString();
                                string year = dvdatestud[fe]["Current_Semester"].ToString();
                                string currentdegree = batcyear + "-" + courseid + "-" + year + "";

                                if (semflag == true)
                                {
                                    currentdegree = batcyear + "-" + courseid + "-" + year + " " + add + "";
                                }
                                if (tempdegreee != currentdegree)
                                {
                                    if (diccourse.ContainsKey(courseid))
                                    {
                                        deg_dept = diccourse[courseid];
                                    }
                                    if (tempdegreee != "")
                                    {
                                        if (checkcoursetot.Checked == true)
                                        {
                                            FpSpread1.Sheets[0].RowCount++;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Degree Wise Total";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.LightCyan;
                                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                                        }
                                        for (int d = 7; d < FpSpread1.Sheets[0].Columns.Count; d = d + 1)
                                        {
                                            Double dco = 0;
                                            if (dicdegree.ContainsKey(d))
                                            {
                                                dco = dicdegree[d];
                                            }
                                            if (checkcoursetot.Checked == true)
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].CellType = dobcell;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Text = dco.ToString();
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].HorizontalAlign = HorizontalAlign.Right;
                                            }
                                            if (dco > 0)
                                            {
                                                if (dicdate.ContainsKey(d))
                                                {
                                                    dco = dicdate[d] + dco;
                                                    dicdate[d] = dco;
                                                }
                                                else
                                                {
                                                    dicdate.Add(d, dco);
                                                }
                                            }
                                        }
                                        dicdegree.Clear();
                                    }
                                    tempdegreee = currentdegree;
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = batcyear + " - " + deg_dept + " / Sem - " + year;
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, FpSpread1.Sheets[0].Columns.Count);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = Color.LightSkyBlue;
                                    decostar = FpSpread1.Sheets[0].RowCount;

                                }
                                //sno++;
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                string name = dvdatestud[fe]["Name"].ToString();
                                string name_roll = dvdatestud[fe]["Roll_No"].ToString() + "-" + dvdatestud[fe]["Name"].ToString();
                                string rollno = dvdatestud[fe]["Roll_No"].ToString();
                                string regno = dvdatestud[fe]["Reg_No"].ToString();
                                string rolladmit = dvdatestud[fe]["roll_admit"].ToString();
                                string feecater = dvdatestud[fe]["feecategory"].ToString();
                                string receiptno = "";
                                receiptno = Convert.ToString(dvdatestud[fe]["ReceiptNo"]);
                                string challanno = Convert.ToString(dvdatestud[fe]["challan_no"]);
                                string chaandrpt = "";
                                if (rightscode == 3 || rightscode == 1 || rightscode == 2)
                                {
                                    if (receiptno != "" && challanno == "")
                                        chaandrpt = receiptno;
                                    if (receiptno == "" && challanno != "")
                                        chaandrpt = receiptno;
                                    if (receiptno != "" && challanno != "")
                                        chaandrpt = challanno + "/" + receiptno;
                                    if (receiptno == "" && challanno == "")
                                        chaandrpt = challanno;
                                }
                                string strView = "roll_no='" + rollno + "' and cal_date='" + dtdatec + "'";
                                if (!string.IsNullOrEmpty(challanno))
                                    strView += " and challan_no='" + challanno + "'";
                                if (!string.IsNullOrEmpty(receiptno))
                                    strView += " and ReceiptNo='" + receiptno + "'";
                                strView += " and paid>0 " + semfiletr + "";
                                ds1.Tables[0].DefaultView.RowFilter = strView;
                               // ds1.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "' and cal_date='" + dtdatec + "' and challan_no='" + challanno + "' and ReceiptNo='" + receiptno + "' and paid>0 " + semfiletr + "";
                                dv_paid_data = ds1.Tables[0].DefaultView;
                                if (dv_paid_data.Count > 0)
                                {
                                    FpSpread1.Sheets[0].RowCount++;
                                    if (!alsno.Contains(rollno))
                                    {
                                        alsno.Add(rollno);
                                        sno++;
                                    }

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = name;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = rollno;
                                    if (sclflag == false)
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = regno;
                                    else
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = rolladmit;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = deg_dept;

                                    if (rightscode == 1)
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = receiptno;
                                    }
                                    if (rightscode == 2 || rightscode == 0)
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = challanno;
                                    }
                                    if (rightscode == 3)
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = chaandrpt;
                                    }
                                    if (semflag == true)
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = feesem;
                                    }
                                    else
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = year;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    string feechec = string.Empty;
                                    if (sclSett() == "0")
                                        feechec = feesem.Split(' ')[1];
                                    else
                                        feechec = feesem.Split(' ')[0];

                                    if (Convert.ToInt32(year) > Convert.ToInt32(feechec))
                                    {
                                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.YellowGreen;
                                    }

                                    Double tot_ledge = 0, total_paid = 0;
                                    string mode = "";
                                    rowflag = true;
                                    //===========Reduce Maximum Excecution Time=====================Start=================================================
                                    dicstupaidfee.Clear();
                                    for (int spf = 0; spf < dv_paid_data.Count; spf++)
                                    {
                                        string amount = dv_paid_data[spf]["paid"].ToString();
                                        string feemode = dv_paid_data[spf]["mode"].ToString();
                                        string spfeec = "";
                                        if (ddlacctype.SelectedItem.Text == "Ledger")
                                        {
                                            string nnn = dv_paid_data[spf]["ledgerfk"].ToString();
                                            spfeec = dv_paid_data[spf]["headerfk"].ToString() + '-' + dv_paid_data[spf]["ledgerfk"].ToString();
                                        }
                                        else if (ddlacctype.SelectedItem.Text == "Group Header")
                                        {
                                            spfeec = dv_paid_data[spf]["headerfk"].ToString();
                                        }
                                        else
                                        {
                                            spfeec = dv_paid_data[spf]["headerfk"].ToString();
                                        }

                                        if (amount.Trim() != "" && amount.Trim() != "0")
                                        {
                                            if (!dicstupaidfee.ContainsKey(spfeec))
                                            {
                                                dicstupaidfee.Add(spfeec, amount + "/" + feemode);
                                            }
                                            else
                                            {
                                                string getv = dicstupaidfee[spfeec];
                                                string[] spiltfee = getv.Split('/');
                                                if (spiltfee.GetUpperBound(0) > 0)
                                                {
                                                    Double dtoapl = Convert.ToDouble(spiltfee[0].ToString()) + Convert.ToDouble(amount);
                                                    string setval = dtoapl + "/" + feemode;
                                                    dicstupaidfee[spfeec] = setval;
                                                }
                                            }
                                        }
                                    }
                                    //===========Reduce Maximum Excecution Time============================End================================================
                                    for (int d = 7; d < FpSpread1.Sheets[0].Columns.Count; d = d + 1)
                                    {
                                        if (colval == false)
                                            col = d;

                                        colval = true;
                                        string text = "";
                                        if (ddlacctype.SelectedItem.Text == "Ledger")
                                        {
                                            text = FpSpread1.Sheets[0].ColumnHeader.Cells[2, d].Text;
                                        }
                                        else if (ddlacctype.SelectedItem.Text == "Group Header")
                                        {
                                            text = FpSpread1.Sheets[0].ColumnHeader.Cells[2, d].Text;
                                        }
                                        else
                                        {
                                            text = FpSpread1.Sheets[0].ColumnHeader.Cells[1, d].Text;
                                        }
                                        if (text == "Paid")
                                        {
                                            string code = "";
                                            if (ddlacctype.SelectedItem.Text == "Ledger")
                                            {
                                                code = FpSpread1.Sheets[0].ColumnHeader.Cells[0, d].Note + '-' + FpSpread1.Sheets[0].ColumnHeader.Cells[2, d].Note.ToString();
                                            }
                                            else if (ddlacctype.SelectedItem.Text == "Group Header")
                                            {
                                                code = FpSpread1.Sheets[0].ColumnHeader.Cells[2, d].Note.ToString();
                                            }
                                            else
                                            {
                                                code = FpSpread1.Sheets[0].ColumnHeader.Cells[1, d].Note.ToString();
                                            }
                                            string paid_amt = "";
                                            if (dicstupaidfee.ContainsKey(code))
                                            {
                                                string getavl = dicstupaidfee[code];
                                                string[] spste = getavl.Split('/');
                                                if (spste.GetUpperBound(0) > 0)
                                                {
                                                    mode = spste[1].ToString();
                                                    paid_amt = spste[0].ToString();
                                                }
                                            }
                                            else
                                            {
                                                paid_amt = "0";
                                            }
                                            if (paid_amt == "0" || paid_amt.Trim() == "")
                                            {
                                                finalflag = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Text = "-";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].HorizontalAlign = HorizontalAlign.Center;
                                            }
                                            else
                                            {

                                                string Linkvalue = "";
                                                if (group_user.Contains(';'))
                                                {
                                                    string[] group_semi = group_user.Split(';');
                                                    group_user = group_semi[0].ToString();

                                                    Linkvalue = da.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + group_user + "' and college_code ='" + collegecode + "'");
                                                }
                                                else
                                                {
                                                    Linkvalue = da.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'");
                                                }

                                                if (Linkvalue == "0")
                                                {
                                                    if (diccurarrfee.ContainsKey(feecater + "$" + dvdatestud[fe]["Current_Semester"].ToString()))
                                                    {
                                                        Double getval = Convert.ToDouble(paid_amt) + diccurarrfee[feecater + "$" + dvdatestud[fe]["Current_Semester"].ToString()];
                                                        diccurarrfee[feecater + "$" + dvdatestud[fe]["Current_Semester"].ToString()] = getval;
                                                    }
                                                    else
                                                    {
                                                        diccurarrfee.Add(feecater + "$" + dvdatestud[fe]["Current_Semester"].ToString(), Convert.ToDouble(paid_amt));
                                                    }
                                                }
                                                else
                                                {
                                                    string sem = "";
                                                    double amt = 0;
                                                    sem = dvdatestud[fe]["Current_Semester"].ToString();
                                                    string valuenew = returnYearforSem(sem);
                                                    if (diccurarrfee.ContainsKey(feecater + "$" + valuenew.ToString()))
                                                    {
                                                        Double getval = Convert.ToDouble(paid_amt) + diccurarrfee[feecater + "$" + valuenew.ToString()];
                                                        diccurarrfee[feecater + "$" + valuenew.ToString()] = getval;
                                                    }
                                                    else
                                                    {
                                                        diccurarrfee.Add(feecater + "$" + valuenew.ToString(), Convert.ToDouble(paid_amt));
                                                    }

                                                }
                                                finalflag = true;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].CellType = dobcell;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Text = paid_amt;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].HorizontalAlign = HorizontalAlign.Right;
                                                if (mode == "1")
                                                {
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].BackColor = Color.LightCoral;
                                                    Cashtotal = Cashtotal + Convert.ToDouble(paid_amt);
                                                }
                                                else if (mode == "2")
                                                {
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].BackColor = Color.LightGray;
                                                    checktotal = checktotal + Convert.ToDouble(paid_amt);
                                                }
                                                else if (mode == "3")
                                                {
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].BackColor = Color.Orange;
                                                    // Challantotal = Challantotal + Convert.ToDouble(paid_amt);
                                                    ddtotal = ddtotal + Convert.ToDouble(paid_amt);

                                                }
                                                else if (mode == "4")
                                                {
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].BackColor = Color.LightGreen;
                                                    Challantotal = Challantotal + Convert.ToDouble(paid_amt);
                                                }
                                                else if (mode == "5")
                                                {
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].BackColor = Color.LightGoldenrodYellow;
                                                    onlinetotal = onlinetotal + Convert.ToDouble(paid_amt);
                                                }
                                                else if (mode == "6")
                                                {
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].BackColor = Color.White;
                                                    cardtotal = cardtotal + Convert.ToDouble(paid_amt);
                                                }
                                                tot_ledge = tot_ledge + Convert.ToDouble(paid_amt);
                                                if (tot_ledge > 0)
                                                {
                                                    if (dicdegree.ContainsKey(d))
                                                    {
                                                        Double val = dicdegree[d] + Convert.ToDouble(paid_amt);
                                                        dicdegree[d] = val;
                                                    }
                                                    else
                                                    {
                                                        dicdegree.Add(d, Convert.ToDouble(paid_amt));
                                                    }
                                                }
                                            }
                                        }
                                        if (text != "Paid")
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].CellType = dobcell;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Text = tot_ledge.ToString();
                                            total_paid = total_paid + tot_ledge;
                                            if (ddlacctype.SelectedItem.Text != "Ledger")
                                            {
                                                tot_ledge = 0;
                                            }
                                            if (total_paid > 0)
                                            {
                                                if (dicdegree.ContainsKey(d))
                                                {
                                                    Double val = dicdegree[d] + Convert.ToDouble(tot_ledge);
                                                    dicdegree[d] = val;
                                                }
                                                else
                                                {
                                                    dicdegree.Add(d, Convert.ToDouble(tot_ledge));
                                                }
                                            }
                                            tot_ledge = 0;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].HorizontalAlign = HorizontalAlign.Right;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].BackColor = Color.MistyRose;
                                        }
                                    }
                                    dicstupaidfee.Clear();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].Columns.Count - 1].CellType = dobcell;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].Columns.Count - 1].Text = total_paid.ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].Columns.Count - 1].HorizontalAlign = HorizontalAlign.Right;
                                    if (dicdegree.ContainsKey(FpSpread1.Sheets[0].Columns.Count - 1))
                                    {
                                        Double val = dicdegree[FpSpread1.Sheets[0].Columns.Count - 1] + Convert.ToDouble(total_paid);
                                        dicdegree[FpSpread1.Sheets[0].Columns.Count - 1] = val;
                                    }
                                    else
                                    {
                                        dicdegree.Add(FpSpread1.Sheets[0].Columns.Count - 1, Convert.ToDouble(total_paid));
                                    }
                                    string codet = "";
                                    if (ddlacctype.SelectedItem.Text == "Ledger")
                                    {
                                        codet = FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].Columns.Count - 1].Note.ToString();
                                    }
                                    else if (ddlacctype.SelectedItem.Text == "Group Header")
                                    {
                                        codet = FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].Columns.Count - 1].Note.ToString();
                                    }
                                    else
                                    {
                                        codet = FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].Columns.Count - 1].Note.ToString();
                                    }

                                    FpSpread1.Visible = true;
                                    btnprintmaster.Visible = true;
                                    lblrptname.Visible = true;
                                    txtexcelname.Visible = true;
                                    btnxl.Visible = true;
                                    errmsg.Visible = false;
                                    lblc1.Visible = true;
                                    lblc2.Visible = true;
                                    lblc3.Visible = true;
                                    lblc4.Visible = true;
                                    lblc5.Visible = true;
                                    lblcard.Visible = true;
                                }
                            }
                            alsno.Clear();
                        }
                    }
                    if (rowflag == true || dtdatec == Convert.ToDateTime(y2))
                    {
                        if (rowflag == true)
                        {
                            if (checkcoursetot.Checked == true)
                            {
                                FpSpread1.Sheets[0].RowCount++;

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Degree Wise Total";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.LightCyan;
                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                            }
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Date Wise Total";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.MediumTurquoise;
                        }
                        int datrow = FpSpread1.Sheets[0].RowCount - 1;

                        if (dtdatec == Convert.ToDateTime(y2))
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.LightSeaGreen;
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                        }

                        for (int d = 7; d < FpSpread1.Sheets[0].Columns.Count; d = d + 1)
                        {
                            string code = "";
                            if (ddlacctype.SelectedItem.Text == "Ledger")
                            {
                                code = FpSpread1.Sheets[0].ColumnHeader.Cells[2, d].Note.ToString();
                            }
                            else if (ddlacctype.SelectedItem.Text == "Group Header")
                            {
                                code = FpSpread1.Sheets[0].ColumnHeader.Cells[2, d].Note.ToString();
                                if (code == "")
                                {
                                    code = FpSpread1.Sheets[0].ColumnHeader.Cells[0, d].Note.ToString();
                                }
                            }
                            else
                            {
                                code = FpSpread1.Sheets[0].ColumnHeader.Cells[1, d].Note.ToString();
                                if (code == "")
                                {
                                    code = FpSpread1.Sheets[0].ColumnHeader.Cells[0, d].Note.ToString();
                                }
                            }
                            if (rowflag == true)
                            {
                                Double dco = 0;
                                if (dicdegree.ContainsKey(d))
                                {
                                    dco = dicdegree[d];
                                }
                                if (checkcoursetot.Checked == true)
                                {
                                    FpSpread1.Sheets[0].Cells[datrow - 1, d].CellType = dobcell;
                                    FpSpread1.Sheets[0].Cells[datrow - 1, d].Text = dco.ToString();
                                    FpSpread1.Sheets[0].Cells[datrow - 1, d].HorizontalAlign = HorizontalAlign.Right;
                                }
                                if (dicdate.ContainsKey(d))
                                {
                                    dco = dicdate[d] + dco;
                                    dicdate[d] = dco;
                                    FpSpread1.Sheets[0].Cells[datrow, d].CellType = dobcell;
                                    FpSpread1.Sheets[0].Cells[datrow, d].Text = dco.ToString();
                                    FpSpread1.Sheets[0].Cells[datrow, d].HorizontalAlign = HorizontalAlign.Right;
                                    if (dicgrandtotal.ContainsKey(d))
                                    {
                                        Double grto = dicgrandtotal[d] + dco;
                                        dicgrandtotal[d] = grto;
                                    }
                                    else
                                    {
                                        dicgrandtotal.Add(d, dco);
                                    }
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Cells[datrow, d].CellType = dobcell;
                                    FpSpread1.Sheets[0].Cells[datrow, d].Text = dco.ToString();
                                    FpSpread1.Sheets[0].Cells[datrow, d].HorizontalAlign = HorizontalAlign.Right;
                                    if (dicgrandtotal.ContainsKey(d))
                                    {
                                        Double grto = dicgrandtotal[d] + dco;
                                        dicgrandtotal[d] = grto;
                                    }
                                    else
                                    {
                                        dicgrandtotal.Add(d, dco);
                                    }
                                }
                            }
                            if (dtdatec == Convert.ToDateTime(y2))
                            {
                                if (dicgrandtotal.ContainsKey(d))
                                {
                                    Double val = dicgrandtotal[d];
                                    dicgrandtotal.Remove(d);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].CellType = dobcell;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Text = val.ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].HorizontalAlign = HorizontalAlign.Right;
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].CellType = dobcell;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Text = "0";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].HorizontalAlign = HorizontalAlign.Right;
                                }
                            }
                        }
                        dicdegree.Clear();
                        dicdate.Clear();
                    }
                }
                FpSpread1.Sheets[0].RowCount++;// = FpSpread1.Sheets[0].RowCount + 2;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "ABSTRACT";
                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.Brown;
                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].ForeColor = Color.White;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 7);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                for (int ps = 0; ps < chklstpaymode.Items.Count; ps++)
                {
                    if (chklstpaymode.Items[ps].Selected == true)
                    {
                        string strptype = chklstpaymode.Items[ps].Text.ToString();
                        FpSpread1.Sheets[0].RowCount++;
                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 7);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = strptype;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                        if (strptype == "Cash")
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Cashtotal.ToString();
                        }
                        else if (strptype == "Cheque")
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = checktotal.ToString();
                        }
                        else if (strptype == "DD")
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = ddtotal.ToString();
                        }
                        else if (strptype == "Challan")
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Challantotal.ToString();
                        }
                        else if (strptype == "Online Pay")
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = onlinetotal.ToString();
                        }
                        else if (strptype == "Card")
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = cardtotal.ToString();
                        }
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                    }
                }
                FpSpread1.Sheets[0].RowCount++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 7);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                Double dotvd = Cashtotal + checktotal + ddtotal + Challantotal + onlinetotal;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = dotvd.ToString();
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;

                Double currfee = 0;
                Double arrfee = 0;
                foreach (var parameter in diccurarrfee)
                {
                    string getsplallow = parameter.Key.ToString();
                    string getfeeamount = parameter.Value.ToString();
                    string[] spt = getsplallow.Split('$');
                    if (spt.GetUpperBound(0) == 1)
                    {
                        for (int rcs = 0; rcs < chklsfeesem.Items.Count; rcs++)
                        {
                            if (chklsfeesem.Items[rcs].Value.ToString() == spt[0].ToString())
                            {
                                string feeval = chklsfeesem.Items[rcs].Text.ToString();
                                string[] stc = feeval.Split(' ');
                                if (stc.GetUpperBound(0) >= 0)
                                {
                                    if (stc[0].ToString().Trim() == spt[1].ToString().Trim())
                                    {
                                        currfee = currfee + Convert.ToDouble(getfeeamount);
                                    }
                                    else
                                    {
                                        arrfee = arrfee + Convert.ToDouble(getfeeamount);
                                    }
                                }
                                else
                                {
                                    if (chklsfeesem.Items[rcs].Text.Contains(spt[1].ToString()))
                                    {
                                        currfee = currfee + Convert.ToDouble(getfeeamount);
                                    }
                                    else
                                    {
                                        arrfee = arrfee + Convert.ToDouble(getfeeamount);
                                    }
                                }
                                rcs = chklsfeesem.Items.Count;
                            }
                        }
                    }
                }
                FpSpread1.Sheets[0].RowCount++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "REGULAR";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 7);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = currfee.ToString();
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
                FpSpread1.Sheets[0].RowCount++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "ARREAR";
                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.YellowGreen;
                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 7);

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = arrfee.ToString();
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;
            }
            else
            {
                errmsg.Visible = true;
                errmsg.Text = "No Records Found";
                FpSpread1.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                lblc1.Visible = false;
                lblc2.Visible = false;
                lblc3.Visible = false;
                lblc4.Visible = false;
                lblc5.Visible = false;
                lblcard.Visible = false;
            }
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;

            ds3.Dispose();
            ds.Dispose();
            ds1.Dispose();
            ds2.Dispose();
            ds3.Dispose();
            dvdatestud.Dispose();
            dv_paid_data.Dispose();
            dvhead.Dispose();

            dicgrandtotal.Clear();
            diccourse.Clear();
            dicdate.Clear();
            dicdegree.Clear();
            dicstupaidfee.Clear();

            ds3 = null;
            ds = null;
            ds1 = null;
            ds2 = null;
            ds3 = null;
            dvdatestud = null;
            dv_paid_data = null;
            dvhead = null;
            dicgrandtotal = null;
            // dicacro = null;
            diccourse = null;
            dicdate = null;
            dicdegree = null;
            dicstupaidfee = null;

            arrayfeecate.Clear();
            arrayfeecate = null;

            GC.Collect();
            GC.WaitForFullGCComplete();
        }
        catch (Exception exception)
        {
            da.sendErrorMail(exception, ddl_collegename.SelectedItem.Value, "Overall_student_Fee_Status.aspx");
            errmsg.Visible = true;
            errmsg.Text = exception.ToString();
            finalflag = true;
        }
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                da.printexcelreport(FpSpread1, reportname);
            }
            else
            {
                errmsg.Text = "Please Enter Your Report Name";
                errmsg.Visible = true;
            }
        }
        catch
        {
        }
    }
    public void loadyettobepaid()
    {
        try
        {
            txtexcelname.Text = "";
            DateTime dat = DateTime.ParseExact(txtdate.Text.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string starttear = dat.ToString("yyyy");
            DateTime dat1 = DateTime.ParseExact(txtto.Text.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string endyear = dat1.ToString("yyyy");
            DataSet dstemp = new DataSet();
            string y1 = dat.ToString("yyyy-MM-dd");
            string y2 = dat1.ToString("yyyy-MM-dd");
            int startrow = 0;
            int datestartrow = 0;
            if (ddlacctype.Text == "---Select---")
            {
                finalflag = true;
                errmsg.Visible = true;
                errmsg.Text = "Please Select Account Type";
                FpSpread1.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
            }
            else if (Convert.ToDouble(starttear) > Convert.ToDouble(endyear))
            {
                finalflag = true;
                errmsg.Visible = true;
                errmsg.Text = "End Year must be greater Than The Start Year ! ";
                FpSpread1.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
            }
            else
            {
                #region get value

                fill_Studheader();
                string batch_all = string.Empty;
                string course_all = string.Empty;
                string degree_all = string.Empty;
                string header_all = string.Empty;
                string tot_course = string.Empty;
                string category = string.Empty;
                string feecode = "";
                string tot_category = string.Empty;
                string finyearval = string.Empty;

                for (int i = 0; i < chklsfyear.Items.Count; i++)
                {
                    if (chklsfyear.Items[i].Selected == true)
                    {
                        if (finyearval == "")
                        {
                            finyearval = "'" + chklsfyear.Items[i].Value.ToString() + "'";
                        }
                        else
                        {
                            finyearval = finyearval + ",'" + chklsfyear.Items[i].Value.ToString() + "'";
                        }
                    }
                }
                //  finyearval = Convert.ToString(getCheckboxListValue(chklsfyear));
                string sem = "";
                // sem = Convert.ToString(getCheckboxListValue(chklsfeesem));
                for (int i = 0; i < chklsfeesem.Items.Count; i++)
                {
                    if (chklsfeesem.Items[i].Selected == true)
                    {
                        if (sem == "")
                        {
                            sem = "'" + chklsfeesem.Items[i].Value.ToString() + "'";
                        }
                        else
                        {
                            sem = sem + ",'" + chklsfeesem.Items[i].Value.ToString() + "'";
                        }
                    }
                }
                for (int b = 0; b < chklstbatch.Items.Count; b++)
                {
                    if (chklstbatch.Items[b].Selected == true)
                    {
                        if (batch_all == "")
                        {
                            batch_all = chklstbatch.Items[b].Value.ToString();
                        }
                        else
                        {

                            batch_all = batch_all + "," + chklstbatch.Items[b].Value.ToString();
                        }
                    }
                }
                //  batch_all = Convert.ToString(getCheckboxListValue(chklstbatch));
                for (int d = 0; d < chklstdegree.Items.Count; d++)
                {
                    if (chklstdegree.Items[d].Selected == true)
                    {
                        if (degree_all == "")
                        {
                            degree_all = chklstdegree.Items[d].Value.ToString();


                        }
                        else
                        {
                            degree_all = degree_all + "','" + chklstdegree.Items[d].Value.ToString();
                        }
                    }
                }
                //  degree_all = Convert.ToString(getCheckboxListValue(chklstdegree));
                for (int c = 0; c < chklstbranch.Items.Count; c++)
                {
                    if (chklstbranch.Items[c].Selected == true)
                    {
                        if (course_all == "")
                        {
                            course_all = chklstbranch.Items[c].Value.ToString();
                            tot_course = chklstbranch.Items[c].Value.ToString();
                        }
                        else
                        {
                            course_all = course_all + "','" + chklstbranch.Items[c].Value.ToString();
                            tot_course = tot_course + "/" + chklstbranch.Items[c].Value.ToString();
                        }
                    }
                }

                for (int a = 0; a < chklstaccheader.Items.Count; a++)
                {
                    if (chklstaccheader.Items[a].Selected == true)
                    {
                        if (header_all == "")
                        {
                            header_all = chklstaccheader.Items[a].Value.ToString();
                        }
                        else
                        {
                            header_all = header_all + "','" + chklstaccheader.Items[a].Value.ToString();
                        }
                    }
                }


                for (int remv = 0; remv < treeview_spreadfields.Nodes.Count; remv++)
                {
                    if (header_all == "")
                    {
                        header_all = treeview_spreadfields.Nodes[remv].Value.ToString();
                    }
                    else
                    {
                        header_all = header_all + "','" + treeview_spreadfields.Nodes[remv].Value.ToString();
                    }
                    if (treeview_spreadfields.Nodes[remv].ChildNodes.Count > 0)
                    {
                        for (int child = 0; child < treeview_spreadfields.Nodes[remv].ChildNodes.Count; child++)
                        {
                            if (category == "")
                            {
                                category = treeview_spreadfields.Nodes[remv].ChildNodes[child].Value.ToString();
                            }
                            else
                            {
                                category = category + "," + treeview_spreadfields.Nodes[remv].ChildNodes[child].Value.ToString();
                            }
                        }
                    }
                }

                #endregion

                string cc = "";
                string debar = "";
                string disc = "";
                string delflg = "";
                if (cblinclude.Items.Count > 0)
                {
                    for (int i = 0; i < cblinclude.Items.Count; i++)
                    {
                        if (cblinclude.Items[i].Selected == true)
                        {
                            if (cblinclude.Items[i].Value == "1")
                            {
                                cc = " r.cc=1";
                            }
                            if (cblinclude.Items[i].Value == "2")
                            {
                                debar = " r.Exam_Flag like 'debar'";
                            }
                            if (cblinclude.Items[i].Value == "3")
                            {
                                disc = " r.DelFlag=1";
                            }
                        }
                    }
                }


                if (cc != "" && debar == "" && disc == "")
                    delflg = " and (" + cc + " or r.cc=0)  and r.Exam_Flag<>'debar' and r.DelFlag=0";

                if (cc == "" && debar != "" && disc == "")
                    delflg = " and r.cc=0  and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";

                if (cc == "" && debar == "" && disc != "")
                    delflg = " and r.cc=0  and r.Exam_Flag<>'debar' and (" + disc + " or r.DelFlag=0)";

                if (cc != "" && debar != "" && disc == "")
                    delflg = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";

                if (cc == "" && debar != "" && disc != "")
                    delflg = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar')  and (" + disc + " or r.DelFlag=0)";

                if (cc != "" && debar == "" && disc != "")
                    delflg = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar'  and (" + disc + " or r.DelFlag=0)";

                else if (cc == "" && debar == "" && disc == "")
                    delflg = " and r.cc=0  and r.Exam_Flag<>'debar' and r.DelFlag=0";

                if (cc != "" && debar != "" && disc != "")
                    delflg = "";
                string strorder = "";
                strorder = "ORDER BY r.Roll_No";
                string serialno = da.GetFunction("select LinkValue from inssettings where college_code=" + collegecode + " and linkname='Student Attendance'");
                if (serialno.Trim() == "1")
                {
                    strorder = "ORDER BY r.serialno";
                }
                else
                {
                    string orderby_Setting = da.GetFunction("select value from master_Settings where settings='order_by'");
                    if (orderby_Setting == "0")
                    {
                        strorder = "ORDER BY r.Roll_No";
                    }
                    else if (orderby_Setting == "1")
                    {
                        strorder = "ORDER BY r.Reg_No";
                    }
                    else if (orderby_Setting == "2")
                    {
                        strorder = "ORDER BY r.Stud_Name";
                    }
                    else if (orderby_Setting == "0,1,2")
                    {
                        strorder = "ORDER BY r.Roll_No,r.Reg_No,r.Stud_Name";
                    }
                    else if (orderby_Setting == "0,1")
                    {
                        strorder = "ORDER BY r.Roll_No,r.Reg_No";
                    }
                    else if (orderby_Setting == "1,2")
                    {
                        strorder = "ORDER BY r.Reg_No,r.Stud_Name";
                    }
                    else if (orderby_Setting == "0,2")
                    {
                        strorder = "ORDER BY r.Roll_No,r.Stud_Name";
                    }
                }
                if (ddlacctype.SelectedItem.Text == "Ledger")
                {
                    #region Ledger

                    if (ddlfeetype.Text == "---Select---")
                    {
                        errmsg.Visible = true;
                        errmsg.Text = "Please Select Fee Type";
                        FpSpread1.Visible = false;
                        btnprintmaster.Visible = false;
                        lblrptname.Visible = false;
                        txtexcelname.Visible = false;
                        btnxl.Visible = false;
                    }
                    else
                    {
                        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.BackColor = Color.FromArgb(214, 235, 255);
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 3;
                        FpSpread1.Sheets[0].ColumnHeader.Columns.Count = 6;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Student Name";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Course";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Sem";
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 3, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 3, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 3, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 3, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 3, 1);
                        //string s = "select fee_type,fee_code,header_id from fee_info order by fee_code ;";
                        //s = s + " select fee_code,fee_type,header_id from fee_info ";
                        //ds1 = da.select_method_wo_parameter(s, "Text");
                        //DataView dvhead = new DataView();
                        //for (int a = 0; a < chklstaccheader.Items.Count; a++)
                        //{
                        //    if (chklstaccheader.Items[a].Selected == true)
                        //    {
                        //        ds1.Tables[0].DefaultView.RowFilter = " header_id ='" + chklstaccheader.Items[a].Value.ToString() + "' ";
                        //        dvhead = ds1.Tables[0].DefaultView;
                        //        string head = chklstaccheader.Items[a].Value.ToString();
                        //        Hashtable hat = new Hashtable();
                        //        for (int i = 0; i < dvhead.Count; i++)
                        //        {
                        //            string fe = dvhead[i]["fee_code"].ToString();
                        //            string type = dvhead[i]["fee_type"].ToString();
                        //            hat.Add(fe, type);
                        //        }
                        //    }
                        //}


                        string getfeeval = "";
                        string demandquery = "";
                        string strstufillerfee = "";
                        string strstufillerfeeapplyn = "";
                        string feecategorycolumn = "";
                        string feecategorycolumngroup = "";
                        if (ddlacctype.SelectedItem.Text == "Ledger")
                        {
                            for (int remv = 0; remv < treeview_spreadfields.Nodes.Count; remv++)
                            {
                                Boolean headflag = false;
                                int ccount = 0;
                                int hstartc = 0;
                                int ledcount = 0;
                                Boolean headervieflag = false;
                                if (treeview_spreadfields.Nodes[remv].ChildNodes.Count > 0)
                                {
                                    for (int child = 0; child < treeview_spreadfields.Nodes[remv].ChildNodes.Count; child++)
                                    {
                                        if (treeview_spreadfields.Nodes[remv].ChildNodes[child].Checked == true)
                                        {
                                            headervieflag = true;
                                            string headervalue = treeview_spreadfields.Nodes[remv].Value.ToString();

                                            if (getfeeval == "")
                                            {
                                                getfeeval = treeview_spreadfields.Nodes[remv].ChildNodes[child].Value;
                                            }
                                            else
                                            {
                                                getfeeval = getfeeval + ',' + treeview_spreadfields.Nodes[remv].ChildNodes[child].Value;
                                            }
                                            if (ddlfeetype.SelectedItem.Text == "Paid")
                                            {
                                                FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 1;
                                                if (headflag == false)
                                                {
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = treeview_spreadfields.Nodes[remv].Text.ToString();
                                                    headflag = true;
                                                    hstartc = FpSpread1.Sheets[0].ColumnCount - 1;

                                                }
                                                ledcount++;
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Note = headervalue;
                                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1, 1, 1);
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = treeview_spreadfields.Nodes[remv].ChildNodes[child].Text.ToString();
                                                feecode = treeview_spreadfields.Nodes[remv].ChildNodes[child].Value.ToString();
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Paid";
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Note = feecode;
                                                ccount += 1;
                                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - ccount, 1, ccount);
                                            }

                                            else if (ddlfeetype.SelectedItem.Text == "Yet To Be Paid")
                                            {
                                                FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 1;
                                                if (headflag == false)
                                                {
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = treeview_spreadfields.Nodes[remv].Text.ToString();
                                                    headflag = true;
                                                    hstartc = FpSpread1.Sheets[0].ColumnCount - 1;
                                                }
                                                ledcount++;
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Note = headervalue;
                                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1, 1, 1);
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = treeview_spreadfields.Nodes[remv].ChildNodes[child].Text.ToString();
                                                feecode = treeview_spreadfields.Nodes[remv].ChildNodes[child].Value.ToString();
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Balance";
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Note = feecode;
                                                ccount += 1;
                                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - ccount, 1, ccount);
                                            }
                                            else
                                            {

                                                FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 3;
                                                if (headflag == false)
                                                {
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 3].Text = treeview_spreadfields.Nodes[remv].Text.ToString();
                                                    headflag = true;
                                                    hstartc = FpSpread1.Sheets[0].ColumnCount - 3;
                                                }
                                                ledcount++;
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 3].Note = headervalue;
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 2].Note = headervalue;
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Note = headervalue;
                                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3, 1, 3);
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 3].Text = treeview_spreadfields.Nodes[remv].ChildNodes[child].Text.ToString();
                                                feecode = treeview_spreadfields.Nodes[remv].ChildNodes[child].Value.ToString();
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3].Text = "Receivable";
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3].Note = feecode;
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Text = "Received";
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Note = feecode;
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Dues";
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Note = feecode;

                                                ccount += 3;
                                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - ccount, 1, ccount);
                                            }

                                        }
                                    }
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 1;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = treeview_spreadfields.Nodes[remv].Text.ToString();
                                }
                                if (headervieflag == true)
                                {
                                    if (ddlfeetype.Text == "Paid")//ledgerwise total header binding.......................
                                    {
                                        FpSpread1.Sheets[0].ColumnCount++;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = treeview_spreadfields.Nodes[remv].Text.ToString() + " Total Paid";
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].BackColor = Color.MistyRose;
                                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1, 3, 1);
                                    }
                                    else if (ddlfeetype.Text == "Yet To Be Paid")
                                    {
                                        FpSpread1.Sheets[0].ColumnCount++;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = treeview_spreadfields.Nodes[remv].Text.ToString() + " Total Balance";
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].BackColor = Color.MistyRose;
                                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1, 3, 1);
                                    }
                                    else
                                    {
                                        FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 3;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3].Text = treeview_spreadfields.Nodes[remv].Text.ToString() + " Total Alloted";
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3].BackColor = Color.MistyRose;
                                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3, 3, 1);
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Text = treeview_spreadfields.Nodes[remv].Text.ToString() + " Total Paid";
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].BackColor = Color.MistyRose;
                                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2, 3, 1);
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = treeview_spreadfields.Nodes[remv].Text.ToString() + " Total Balance";
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].BackColor = Color.MistyRose;
                                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1, 3, 1);

                                    }
                                }
                            }
                            if (ddlfeetype.Text == "Paid")
                            {
                                FpSpread1.Sheets[0].ColumnCount++;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Total Paid";
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1, 3, 1);
                            }
                            else if (ddlfeetype.Text == "Yet To Be Paid")
                            {
                                FpSpread1.Sheets[0].ColumnCount++;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Total Balance";
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1, 3, 1);
                            }
                            else
                            {
                                FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 4;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 4].Text = "Total Alloted";
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 4, 3, 1);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3].Text = "Total Paid";
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3, 3, 1);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Text = "Total Balance";
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2, 3, 1);

                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Status";
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1, 3, 1);
                            }

                            if (category.Trim() != "" && category != null)
                            {
                                strstufillerfee = strstufillerfee + " and a.LedgerFK in(" + getfeeval + ")";
                                strstufillerfeeapplyn = strstufillerfeeapplyn + " and a.LedgerFK in(" + getfeeval + ")";
                                demandquery = " and a.LedgerFK in(" + getfeeval + ")";
                            }

                            feecategorycolumn = feecategorycolumn + ",a.LedgerFK as fee_code";
                            feecategorycolumngroup = feecategorycolumngroup + ",a.LedgerFK";
                        }


                        if (ddlfeetype.Text == "Yet To Be Paid")
                        {
                            FpSpread1.Sheets[0].ColumnCount++;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Total Balance";
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1, 3, 1);
                        }

                        //feecategorycolumn = ",a.Ledgerfk";
                        //feecategorycolumngroup = feecategorycolumn + ",a.Ledgerfk";
                        string demquery = "select distinct r.stud_name as Name,r.Current_Semester,batch_year,r.Roll_No,r.Reg_No,r.roll_admit,r.degree_code,CONVERT(varchar(10), a.paystartdate,103) as DueDate,CONVERT(varchar(10), a.allotdate,103) as allotdate,sum(totalamount) as demand ,a.Ledgerfk as header_id" + feecategorycolumn + " from ft_feeallot a,registration r where  a.app_no=r.app_no and a.totalamount>0 and ISNULL (BalAmount ,TotalAmount)<>0 " + delflg + "  and a.Ledgerfk in(" + category + ") and r.degree_code in(" + course_all + ") and r.Batch_year in(" + batch_all + ") and a.feecategory in(" + sem + ")  and a.paystartdate between '" + y1 + "' and '" + y2 + "' group by a.app_no,batch_year,r.roll_admit,r.roll_no,r.Reg_No,r.stud_name,r.degree_code,r.Current_Semester,CONVERT(varchar(10), a.paystartdate,103),CONVERT(varchar(10), a.allotdate,103),a.Ledgerfk ";
                        if (cbbfadm.Checked == true)
                        {
                            demquery += "   union all select distinct r.stud_name as Name,r.Current_Semester,'' Roll_No ,'' Reg_no,''roll_admit,batch_year,r.degree_code, CONVERT(varchar(10), a.paystartdate,103) as DueDate,CONVERT(varchar(10), a.allotdate,103) as allotdate,sum(totalamount) as demand ,a.Ledgerfk as header_id" + feecategorycolumn + " from ft_feeallot a,applyn r where a.app_no=r.app_no and a.totalamount>0 and r.isconfirm = 1 and r.admission_status = 0 and a.Ledgerfk in(" + category + ") and r.degree_code in(" + course_all + ") and r.Batch_year in(" + batch_all + ") and a.feecategory in(" + sem + ")  and a.paystartdate between '" + y1 + "' and '" + y2 + "'  group by r.app_no,r.app_formno ,r.stud_name,r.degree_code,batch_year,r.Current_Semester,CONVERT(varchar(10), a.paystartdate,103),CONVERT(varchar(10), a.allotdate,103),a.Ledgerfk";
                        }

                        ds = da.select_method_wo_parameter(demquery, "Text");
                        DataTable dttemp = ds.Tables[0];
                        DataView dv_demand_data = new DataView();

                        string degquery = "select c.course_id,c.course_name,d.degree_code,d.acronym from course c,degree d where c.course_id=d.course_id ";
                        ds2 = da.select_method_wo_parameter(degquery, "Text");

                        //Added by Venkat 6/9/2014=======================Start
                        if (ddlstudtype.SelectedItem.Text == "Regular" || ddlstudtype.SelectedItem.Text == "Lateral" || ddlstudtype.SelectedItem.Text == "Transfer")
                        {
                            string str = "select distinct r.stud_name as Name,r.Current_Semester,r.Reg_No,r.Roll_No,r.degree_code,roll_admit,r.batch_year from registration r where r.batch_year in(" + batch_all + ")  and r.mode='" + ddlstudtype.SelectedValue + "' " + strorder + "";
                            ds3 = da.select_method_wo_parameter(str, "text");
                        }
                        else if (ddlstudtype.SelectedItem.Text == "Re-admit")
                        {
                            string str = "select distinct r.stud_name as Name,r.Current_Semester,r.Reg_No,r.Roll_No,r.degree_code,roll_admit,r.batch_year from Readmission a,Registration r where a.App_no=r.App_No and a.newBatchYear in(" + batch_all + ") and  r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' " + strorder + "";
                            ds3 = da.select_method_wo_parameter(str, "text");
                        }
                        else if (ddlstudtype.SelectedItem.Text == "Re-join")
                        {
                            string str = "select distinct r.stud_name as Name,r.Current_Semester,r.Reg_No,r.Roll_No,r.degree_code,roll_admit,r.batch_year from rejoin_list a,Registration r where a.roll_no=r.Roll_No and a.newBatchYear in(" + batch_all + ") and  r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' " + strorder + "";
                            ds3 = da.select_method_wo_parameter(str, "text");
                        }
                        else
                        {
                            string str = "select distinct r.stud_name as Name,r.Current_Semester,r.Reg_No,r.Roll_No,r.degree_code,roll_admit,r.batch_year from registration r where CC=0 and delflag=0 and exam_flag<>'debar' ";
                            if (cbbfadm.Checked == true)
                            {
                                str += " union select distinct r.stud_name as Name,r.Current_Semester,'' as Reg_No,r.app_formno as Roll_No,r.degree_code,'' as roll_admit,r.batch_year from applyn r where  isnull(r.is_enroll,'0')<>'2' " + strorder + "";
                                //  r.app_no not in (select app_no from Registration ) " + strorder + "";
                            }
                            ds3 = da.select_method_wo_parameter(str, "text");
                        }
                        //=====================================End
                        DataView dvstudent = new DataView();
                        string date = "";
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            int sno = 0;
                            for (DateTime dtcal = Convert.ToDateTime(y1); dtcal <= Convert.ToDateTime(y2); dtcal = dtcal.AddDays(1))
                            {
                                dttemp.DefaultView.RowFilter = " duedate='" + dtcal.ToString("dd/MM/yyyy").Split(' ')[0] + "'";
                                date = dtcal.ToString("dd/MM/yyyy").Split(' ')[0];
                                dv_demand_data = dttemp.DefaultView;
                                if (dv_demand_data.Count > 0)
                                {
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = dtcal.ToString("dd/MM/yyyy");
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, FpSpread1.Sheets[0].Columns.Count);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = Color.LightCyan;

                                    datestartrow = FpSpread1.Sheets[0].RowCount;
                                    string[] spl_course = tot_course.Split('/');
                                    string[] spl_batch = batch_all.Split(',');
                                    for (int splbat = 0; spl_batch.GetUpperBound(0) >= splbat; splbat++)
                                    {
                                        string batcyear = spl_batch[splbat].ToString();
                                        for (int spl = 0; spl_course.GetUpperBound(0) >= spl; spl++)
                                        {
                                            startrow = FpSpread1.Sheets[0].RowCount;
                                            deptflag = false; ;
                                            string courseid = spl_course[spl].ToString();
                                            dttemp.DefaultView.RowFilter = " batch_year='" + batcyear + "' and degree_code='" + courseid + "' and  duedate='" + date + "'";
                                            dv_demand_data = dttemp.DefaultView;
                                            int count4 = dv_demand_data.Count;

                                            DataView dv_get_course = new DataView();
                                            ds2.Tables[0].DefaultView.RowFilter = "degree_code='" + courseid + "'";
                                            dv_get_course = ds2.Tables[0].DefaultView;

                                            ds3.Tables[0].DefaultView.RowFilter = " batch_year='" + batcyear + "' and degree_code='" + courseid + "'";
                                            dvstudent = ds3.Tables[0].DefaultView;


                                            if (dvstudent.Count > 0)
                                            {
                                                for (int i = 0; i < dvstudent.Count; i++)
                                                {
                                                    string name = dvstudent[i]["Name"].ToString();
                                                    string name_roll = dvstudent[i]["Roll_No"].ToString() + "-" + dvstudent[i]["Name"].ToString();
                                                    string year = dvstudent[i]["Current_Semester"].ToString();
                                                    string rollno = dvstudent[i]["Roll_No"].ToString();
                                                    string deg_dept = string.Empty;
                                                    if (dv_get_course.Count > 0)
                                                    {
                                                        deg_dept = dv_get_course[0]["course_name"].ToString() + "-" + dv_get_course[0]["acronym"].ToString();
                                                    }
                                                    if (deptflag == false)
                                                    {
                                                        FpSpread1.Sheets[0].RowCount++;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = batcyear + "-" + deg_dept + "/Sem-" + year;

                                                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, FpSpread1.Sheets[0].Columns.Count);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = Color.LightCyan;
                                                        deptflag = true;
                                                    }
                                                    FpSpread1.Sheets[0].RowCount++;
                                                    sno++;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = name;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dvstudent[i]["Roll_No"].ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = dvstudent[i]["Reg_No"].ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = deg_dept;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = year;
                                                    FpSpread1.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                                    FpSpread1.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                                    FpSpread1.Visible = true;
                                                    btnprintmaster.Visible = true;
                                                    lblrptname.Visible = true;
                                                    txtexcelname.Visible = true;
                                                    btnxl.Visible = true;
                                                    errmsg.Visible = false;
                                                    lblc1.Visible = false;
                                                    lblc2.Visible = false;
                                                    lblc3.Visible = false;
                                                    lblc4.Visible = false;
                                                    lblc5.Visible = false;
                                                    lblcard.Visible = false;
                                                }
                                            }


                                            if (dv_demand_data.Count > 0)
                                            {
                                                for (int r = startrow; r < FpSpread1.Sheets[0].RowCount; r++)
                                                {
                                                    if (FpSpread1.Sheets[0].Cells[r, 2].Text.ToString() != "")
                                                    {
                                                        double total_due = 0;
                                                        string name = FpSpread1.Sheets[0].Cells[r, 1].Text.ToString();
                                                        string roll = FpSpread1.Sheets[0].Cells[r, 2].Text.ToString();
                                                        string name_roll = roll + "-" + name;
                                                        string altdat = "";
                                                        string duedat = "";
                                                        for (int d = 5; d < FpSpread1.Sheets[0].Columns.Count - 1; d = d + 1)
                                                        {
                                                            string demand_amt = "";
                                                            string code = FpSpread1.Sheets[0].ColumnHeader.Cells[2, d].Note.ToString();
                                                            if (code != "")
                                                            {
                                                                DataView dv_demand_amount = new DataView();
                                                                if (ds.Tables[0].Rows.Count > 0)
                                                                {
                                                                    ds.Tables[0].DefaultView.RowFilter = "Roll_No='" + roll + "' and fee_code='" + code + "' and duedate='" + date + "'";
                                                                    dv_demand_amount = ds.Tables[0].DefaultView;
                                                                }
                                                                if (dv_demand_amount.Count > 0)
                                                                {
                                                                    demand_amt = dv_demand_amount[0]["demand"].ToString();
                                                                    altdat = dv_demand_amount[0]["allotdate"].ToString();
                                                                    duedat = dv_demand_amount[0]["DueDate"].ToString();
                                                                }
                                                                else
                                                                {
                                                                    demand_amt = "0";
                                                                }

                                                                string paidquery = "select distinct r.stud_name as Name,r.Current_Semester, r.Roll_No,sum(a.debit) as paid,r.degree_code  ,a.ledgerfk as header_id ,feecategory from ft_findailytransaction a,registration r where  a.app_no = r.app_no and a.TransDate between '" + altdat + "' and '" + duedat + "'    and a.Ledgerfk in(" + category + ")  and r.degree_code in('" + course_all + "') and r.Batch_year in(" + batch_all + ")  and feecategory in(" + sem + ")   and credit=0 and transtype=1 and a.debit>0 and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'";
                                                                if (usBasedRights == true)
                                                                    paidquery = " and a.EntryUserCode in('" + usercode + "')";
                                                                paidquery += " group by r.roll_no,r.stud_name,r.degree_code,r.Current_Semester ,a.ledgerfk,feecategory ";

                                                                if (cbbfadm.Checked == true)
                                                                {
                                                                    paidquery += "   union select distinct r.stud_name as Name,r.Current_Semester,r.app_formno as Roll_No,sum(a.debit) as paid,r.degree_code ,a.ledgerfk as header_id ,feecategory from ft_findailytransaction a,applyn r  where  a.app_no = r.app_no and a.TransDate between '" + altdat + "' and '" + duedat + "'  and a.Ledgerfk in(" + category + ")   and r.degree_code in('" + course_all + "') and r.Batch_year in(" + batch_all + ")  and feecategory in(" + sem + ") and credit=0 and transtype=1 and a.debit>0  and r.isconfirm = 1 and r.admission_status = 0 and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' ";
                                                                    if (usBasedRights == true)
                                                                        paidquery = " and a.EntryUserCode in('" + usercode + "')";
                                                                    paidquery += " group by r.app_no,r.app_formno,r.stud_name,r.degree_code,r.Current_Semester,a.ledgerfk,feecategory";
                                                                }
                                                                ds1 = da.select_method_wo_parameter(paidquery, "Text");

                                                                DataView dv_paid_data = new DataView();
                                                                if (ds1.Tables[0].Rows.Count > 0)
                                                                {
                                                                    ds1.Tables[0].DefaultView.RowFilter = "Roll_No='" + roll + "' and fee_code='" + code + "'";
                                                                    dv_paid_data = ds1.Tables[0].DefaultView;
                                                                }
                                                                string paid_amt = string.Empty;
                                                                if (dv_paid_data.Count > 0)
                                                                {
                                                                    paid_amt = dv_paid_data[0]["paid"].ToString();
                                                                }
                                                                else
                                                                {
                                                                    paid_amt = "0";
                                                                }
                                                                if (demand_amt == "0" && paid_amt == "0")
                                                                {
                                                                    finalflag = true;
                                                                    FpSpread1.Sheets[0].Cells[r, d].Text = "-";
                                                                    FpSpread1.Sheets[0].Cells[r, d].HorizontalAlign = HorizontalAlign.Center;
                                                                }
                                                                else
                                                                {
                                                                    finalflag = true;
                                                                    double due_amt = Convert.ToDouble(Convert.ToDouble(demand_amt) - Convert.ToDouble(paid_amt));
                                                                    if (due_amt < 0)
                                                                    {
                                                                        due_amt = 0;
                                                                    }
                                                                    FpSpread1.Sheets[0].Cells[r, d].Text = due_amt.ToString();
                                                                    FpSpread1.Sheets[0].Cells[r, d].HorizontalAlign = HorizontalAlign.Right;
                                                                    total_due = total_due + Convert.ToDouble(due_amt);
                                                                }
                                                            }
                                                        }
                                                        if (total_due == 0)
                                                        {
                                                            FpSpread1.Sheets[0].Rows[r].Visible = false;
                                                        }
                                                        else
                                                        {
                                                            i = i + 1;
                                                            FpSpread1.Sheets[0].Cells[r, 0].Text = i.ToString();
                                                            FpSpread1.Sheets[0].Cells[r, FpSpread1.Sheets[0].Columns.Count - 1].Text = total_due.ToString();
                                                            FpSpread1.Sheets[0].Cells[r, FpSpread1.Sheets[0].Columns.Count - 1].HorizontalAlign = HorizontalAlign.Right;
                                                        }
                                                        if (FpSpread1.Sheets[0].Cells[r, 0].Text == "Total")
                                                        {
                                                            FpSpread1.Sheets[0].Rows[r].Visible = true;
                                                        }
                                                    }
                                                }
                                                FpSpread1.Sheets[0].RowCount++;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Degree Wise Total";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 5);

                                                for (int d = 7; d < FpSpread1.Sheets[0].Columns.Count; d = d + 1)
                                                {
                                                    double totaldue = 0;
                                                    if (FpSpread1.Sheets[0].RowCount > 0)
                                                    {
                                                        for (int r = startrow; r < FpSpread1.Sheets[0].RowCount; r++)
                                                        {
                                                            if (FpSpread1.Sheets[0].Cells[r, 0].ColumnSpan <= 1)
                                                            {
                                                                double val = 0;
                                                                string amount = FpSpread1.Sheets[0].Cells[r, d].Text;
                                                                if (amount == "-")
                                                                {
                                                                    val = 0;
                                                                }
                                                                else
                                                                {
                                                                    if (amount == "")
                                                                    {
                                                                        val = 0;
                                                                    }
                                                                    else
                                                                    {
                                                                        val = Convert.ToDouble(amount);
                                                                    }
                                                                }
                                                                totaldue = totaldue + val;
                                                            }
                                                        }
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Text = totaldue.ToString();
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].HorizontalAlign = HorizontalAlign.Right;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Date Wise Total";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                                    for (int d = 7; d < FpSpread1.Sheets[0].Columns.Count; d = d + 1)
                                    {
                                        double totaldue = 0;
                                        if (FpSpread1.Sheets[0].RowCount > 0)
                                        {
                                            for (int r = datestartrow; r < FpSpread1.Sheets[0].RowCount; r++)
                                            {
                                                if (FpSpread1.Sheets[0].Cells[r, 0].ColumnSpan <= 1)
                                                {
                                                    double val = 0;
                                                    string amount = FpSpread1.Sheets[0].Cells[r, d].Text;
                                                    if (amount == "-")
                                                    {
                                                        val = 0;
                                                    }
                                                    else
                                                    {
                                                        if (amount == "")
                                                        {
                                                            val = 0;
                                                        }
                                                        else
                                                        {
                                                            val = Convert.ToDouble(amount);
                                                        }
                                                    }
                                                    totaldue = totaldue + val;
                                                }
                                            }
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Text = totaldue.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].HorizontalAlign = HorizontalAlign.Right;
                                        }
                                    }
                                }
                            }
                            if (FpSpread1.Sheets[0].RowCount > 2)
                            {
                                FpSpread1.Sheets[0].RowCount++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                                for (int d = 7; d < FpSpread1.Sheets[0].Columns.Count; d = d + 1)
                                {
                                    double totaldue = 0;
                                    if (FpSpread1.Sheets[0].RowCount > 0)
                                    {
                                        for (int r = 0; r < FpSpread1.Sheets[0].RowCount; r++)
                                        {
                                            if (FpSpread1.Sheets[0].Cells[r, 0].ColumnSpan <= 1)
                                            {
                                                double val = 0;
                                                string amount = FpSpread1.Sheets[0].Cells[r, d].Text;
                                                if (amount == "-")
                                                {
                                                    val = 0;
                                                }
                                                else
                                                {
                                                    if (amount == "")
                                                    {
                                                        val = 0;
                                                    }
                                                    else
                                                    {
                                                        val = Convert.ToDouble(amount);
                                                    }
                                                }
                                                totaldue = totaldue + val;
                                            }
                                        }
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Text = totaldue.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].HorizontalAlign = HorizontalAlign.Right;
                                    }
                                }
                            }
                        }
                        else
                        {
                            errmsg.Visible = true;
                            errmsg.Text = "No Records Found";
                            FpSpread1.Visible = false;
                            btnprintmaster.Visible = false;
                            lblrptname.Visible = false;
                            txtexcelname.Visible = false;
                            btnxl.Visible = false;
                            lblc1.Visible = false;
                            lblc2.Visible = false;
                            lblc3.Visible = false;
                            lblc4.Visible = false;
                            lblc5.Visible = false;
                            lblcard.Visible = false;
                        }
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    }

                    #endregion
                }

                else if (ddlacctype.SelectedItem.Text == "Group Header")
                {
                    #region groupnoneed
                    if (ddlfeetype.Text == "---Select---")
                    {
                        errmsg.Visible = true;
                        errmsg.Text = "Please Select Fee Type";
                        FpSpread1.Visible = false;
                        btnprintmaster.Visible = false;
                        lblrptname.Visible = false;
                        txtexcelname.Visible = false;
                        btnxl.Visible = false;
                    }
                    else
                    {
                        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.BackColor = Color.FromArgb(214, 235, 255);
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 3;
                        FpSpread1.Sheets[0].ColumnHeader.Columns.Count = 5;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Student Name";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No/Application No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Course";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Sem";
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 3, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 3, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 3, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 3, 1);
                        string head = "";

                        string acchead = "select distinct header_name,ChlHeaderName,header_id from acctheader";
                        ds = da.select_method_wo_parameter(acchead, "text");
                        DataView dvhead = new DataView();
                        for (int j = 0; j < chklstaccheader.Items.Count; j++)
                        {
                            if (chklstaccheader.Items[j].Selected == true)
                            {
                                string grhead = chklstaccheader.Items[j].Text.ToString();
                                ds.Tables[0].DefaultView.RowFilter = " ChlHeaderName='" + grhead + "'";
                                dvhead = ds.Tables[0].DefaultView;
                                int ccount = 0;
                                int hstartc = 0;

                                Boolean headflag = false;
                                for (int a = 0; a < dvhead.Count; a++)
                                {
                                    if (ddlfeetype.SelectedItem.Text == "Yet To Be Paid")
                                    {
                                        FpSpread1.Sheets[0].ColumnCount++;
                                        if (headflag == false)
                                        {
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = chklstaccheader.Items[j].Text;
                                            headflag = true;
                                            hstartc = FpSpread1.Sheets[0].ColumnCount - 1;
                                        }
                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            feecode = dvhead[a]["header_id"].ToString();
                                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Width = 200;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = dvhead[a]["header_name"].ToString();
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Balance";
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Note = feecode;
                                            if (head == "")
                                            {
                                                head = ds.Tables[0].Rows[a]["header_id"].ToString();
                                            }
                                            else
                                            {
                                                head = head + "," + dvhead[a]["header_id"].ToString();
                                            }
                                        }
                                        ccount += 1;
                                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - ccount, 1, ccount);
                                    }
                                }
                            }
                        }
                        if (ddlfeetype.Text == "Yet To Be Paid")
                        {
                            FpSpread1.Sheets[0].ColumnCount++;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Total Balance";
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1, 3, 1);
                        }
                        string demquery = "select distinct r.stud_name as Name,r.Current_Semester,r.Roll_No,f.header_id,a.roll_admit,r.degree_code,sum(total) as demand,a.duedate,a.allotdate from fee_allot a,fee_info f,registration r where a.fee_code=f.fee_code and f.header_id=a.header_id and a.roll_admit=r.roll_admit and f.header_id in (" + head + ") and r.batch_year in(" + batch_all + ") and r.degree_code in('" + course_all + "') and a.DueDate between '" + y1 + "' and '" + y2 + "' group by a.roll_admit,r.roll_no,r.stud_name,r.degree_code,r.Current_Semester,f.header_id,a.duedate,a.allotdate ";

                        if (cbbfadm.Checked == true)
                        {
                            demquery += "   union select distinct r.stud_name as Name,r.Current_Semester,r.app_formno,f.header_id,'' as roll_admit,r.degree_code,sum(total) as demand,a.duedate,a.allotdate from fee_allot a,fee_info f,applyn r where a.fee_code=f.fee_code and f.header_id=a.header_id and a.app_formno=r.app_formno and f.header_id in (" + head + ") and r.batch_year in(" + batch_all + ") and r.degree_code in('" + course_all + "')  and isnull(r.is_enroll,'0')<>'2' and a.DueDate between '" + y1 + "' and '" + y2 + "' group by r.app_formno,r.stud_name,r.degree_code,r.Current_Semester,f.header_id,a.duedate,a.allotdate";
                            // and r.app_no not in (select app_no from Registration where batch_year in ( " + batch_all + "))

                        }
                        ds = da.select_method_wo_parameter(demquery, "Text");
                        DataTable dttemp = ds.Tables[0];

                        string degquery = "select c.course_id,c.course_name,d.degree_code,d.acronym from course c,degree d where c.course_id=d.course_id";
                        ds2 = da.select_method_wo_parameter(degquery, "Text");

                        //Added by Venkat 6/9/2014============================Start
                        if (ddlstudtype.SelectedItem.Text == "Regular" || ddlstudtype.SelectedItem.Text == "Lateral" || ddlstudtype.SelectedItem.Text == "Transfer")
                        {
                            string str = "select distinct r.stud_name as Name,r.Current_Semester,r.Reg_No,r.Roll_No,r.degree_code,roll_admit,r.batch_year from registration r where r.batch_year in(" + batch_all + ")  and r.mode='" + ddlstudtype.SelectedValue + "' " + strorder + "";
                            ds3 = da.select_method_wo_parameter(str, "text");
                        }
                        else if (ddlstudtype.SelectedItem.Text == "Re-admit")
                        {
                            string str = "select distinct r.stud_name as Name,r.Current_Semester,r.Reg_No,r.Roll_No,r.degree_code,roll_admit,r.batch_year from Readmission a,Registration r where a.App_no=r.App_No and a.newBatchYear in(" + batch_all + ") and  r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' " + strorder + "";
                            ds3 = da.select_method_wo_parameter(str, "text");
                        }
                        else if (ddlstudtype.SelectedItem.Text == "Re-join")
                        {
                            string str = "select distinct r.stud_name as Name,r.Current_Semester,r.Reg_No,r.Roll_No,r.degree_code,roll_admit,r.batch_year from rejoin_list a,Registration r where a.roll_no=r.Roll_No and a.newBatchYear in(" + batch_all + ") and  r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' " + strorder + "";
                            ds3 = da.select_method_wo_parameter(str, "text");
                        }
                        else
                        {
                            string str = "select distinct r.stud_name as Name,r.Current_Semester,r.Reg_No,r.Roll_No,r.degree_code,roll_admit,r.batch_year from registration r where r.cc=0 and delflag=0 and exam_flag<>'debar' ";
                            if (cbbfadm.Checked == true)
                            {
                                str += "  union select distinct r.stud_name as Name,r.Current_Semester,'' as Reg_No,r.app_formno as Roll_No,r.degree_code,'' as roll_admit,r.degree_code,r.batch_year from applyn r where   isnull(r.is_enroll,'0')<>'2'  " + strorder + "";
                                //r.app_no not in (select app_no from Registration where batch_year in (" + batch_all + "))
                            }
                            ds3 = da.select_method_wo_parameter(str, "text");
                        }
                        //==========================End
                        DataView dvstucoun = new DataView();
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            int sno = 0;
                            for (DateTime dtdatec = Convert.ToDateTime(y1); dtdatec <= Convert.ToDateTime(y2); dtdatec = dtdatec.AddDays(1))
                            {
                                dttemp.DefaultView.RowFilter = " duedate='" + dtdatec + "'";
                                DataView dvdatefinance = dttemp.DefaultView;
                                Boolean dateflag = false;
                                if (dvdatefinance.Count > 0)
                                {
                                    dateflag = true;
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = dtdatec.ToString("dd/MM/yyyy");
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, FpSpread1.Sheets[0].Columns.Count);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = Color.LightCyan;
                                    datestartrow = FpSpread1.Sheets[0].RowCount + 1;
                                    string[] spl_course = tot_course.Split('/');
                                    string[] spl_batch = batch_all.Split(',');
                                    for (int splbat = 0; spl_batch.GetUpperBound(0) >= splbat; splbat++)
                                    {
                                        string batcyear = spl_batch[splbat].ToString();
                                        for (int spl = 0; spl_course.GetUpperBound(0) >= spl; spl++)
                                        {
                                            string courseid = spl_course[spl].ToString();
                                            dttemp.DefaultView.RowFilter = " duedate='" + dtdatec + "' and degree_code='" + courseid + "' and duedate='" + dtdatec + "' ";
                                            DataView dtdegfee = dttemp.DefaultView;

                                            if (dtdegfee.Count > 0)
                                            {
                                                deptflag = false;
                                                startrow = FpSpread1.Sheets[0].RowCount;
                                                DataView dv_demand_data = new DataView();
                                                ds.Tables[0].DefaultView.RowFilter = "degree_code='" + courseid + "'";
                                                dv_demand_data = ds.Tables[0].DefaultView;
                                                int count4 = 0;
                                                count4 = dv_demand_data.Count;

                                                DataView dv_get_course = new DataView();
                                                ds2.Tables[0].DefaultView.RowFilter = "degree_code='" + courseid + "'";
                                                dv_get_course = ds2.Tables[0].DefaultView;

                                                ds3.Tables[0].DefaultView.RowFilter = " degree_code='" + courseid + "' and batch_year='" + batcyear + "'";
                                                dvstucoun = ds3.Tables[0].DefaultView;

                                                for (int i = 0; i < dv_demand_data.Count; i++)
                                                {
                                                    string name = dv_demand_data[i]["Name"].ToString();
                                                    string name_roll = dv_demand_data[i]["Roll_No"].ToString() + "-" + dv_demand_data[i]["Name"].ToString();
                                                    string year = dv_demand_data[i]["Current_Semester"].ToString();
                                                    string rollno = dv_demand_data[i]["Roll_No"].ToString();

                                                    string deg_dept = string.Empty;
                                                    if (dv_get_course.Count > 0)
                                                    {
                                                        deg_dept = dv_get_course[0]["course_name"].ToString() + "-" + dv_get_course[0]["acronym"].ToString();
                                                    }
                                                    if (deptflag == false)
                                                    {
                                                        FpSpread1.Sheets[0].RowCount++;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = batcyear + "-" + deg_dept + "/Sem-" + year;

                                                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, FpSpread1.Sheets[0].Columns.Count);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = Color.LightCyan;
                                                        deptflag = true;
                                                    }

                                                    FpSpread1.Sheets[0].RowCount++;
                                                    sno++;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = name;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dv_demand_data[i]["Roll_No"].ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = deg_dept;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = year;
                                                    FpSpread1.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                                    FpSpread1.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                                    FpSpread1.Visible = true;
                                                    btnprintmaster.Visible = true;
                                                    lblrptname.Visible = true;
                                                    txtexcelname.Visible = true;
                                                    btnxl.Visible = true;
                                                    errmsg.Visible = false;
                                                    lblc1.Visible = false;
                                                    lblc2.Visible = false;
                                                    lblc3.Visible = false;
                                                    lblc4.Visible = false;
                                                    lblc5.Visible = false;
                                                    lblcard.Visible = false;
                                                }

                                                if (deptflag == true)
                                                {
                                                    for (int r = startrow; r < FpSpread1.Sheets[0].RowCount; r++)
                                                    {
                                                        if (FpSpread1.Sheets[0].Cells[r, 2].Text.ToString() != "")
                                                        {
                                                            double total_due = 0;
                                                            string name = FpSpread1.Sheets[0].Cells[r, 1].Text.ToString();
                                                            string roll = FpSpread1.Sheets[0].Cells[r, 2].Text.ToString();
                                                            string name_roll = roll + "-" + name;
                                                            string altdat = "";
                                                            string duedat = "";
                                                            if (ddlfeetype.Text == "Yet To Be Paid")
                                                            {
                                                                for (int d = 5; d < FpSpread1.Sheets[0].Columns.Count - 1; d = d + 1)
                                                                {

                                                                    string demand_amt = "";
                                                                    string code = FpSpread1.Sheets[0].ColumnHeader.Cells[2, d].Note.ToString();

                                                                    DataView dv_demand_amount = new DataView();
                                                                    if (ds.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        ds.Tables[0].DefaultView.RowFilter = "roll_no='" + roll + "' and header_id='" + code + "' and duedate='" + dtdatec + "'";
                                                                        dv_demand_amount = ds.Tables[0].DefaultView;
                                                                    }

                                                                    if (dv_demand_amount.Count > 0)
                                                                    {
                                                                        demand_amt = dv_demand_amount[0]["demand"].ToString();
                                                                        altdat = dv_demand_amount[0]["allotdate"].ToString();
                                                                        duedat = dv_demand_amount[0]["DueDate"].ToString();
                                                                    }
                                                                    else
                                                                    {
                                                                        demand_amt = "0";
                                                                    }
                                                                    DataView dv_paid_data = new DataView();

                                                                    string paidquery = "select distinct r.stud_name as Name,r.Current_Semester, r.Roll_No, f.header_id, sum(d.credit) as paid,r.degree_code from dailytransaction d,fee_info f,registration r where d.fee_code=f.fee_code and f.fee_type<>'Excess Amount' and d.name = r.Roll_No+'-'+r.stud_name  and f.header_id in (" + head + ") and r.batch_year in(" + batch_all + ") and r.degree_code in('" + course_all + "') and d.cal_date between '" + altdat + "' and '" + duedat + "' and debit=0 and vouchertype=1 group by r.roll_no,r.stud_name,r.degree_code,r.Current_Semester,f.header_id ";
                                                                    if (cbbfadm.Checked == true)
                                                                    {
                                                                        paidquery += "  union select distinct r.stud_name as Name,r.Current_Semester, r.app_formno as Roll_No, f.header_id, sum(d.credit) as paid,r.degree_code  from dailytransaction d,fee_info f,applyn r  where d.fee_code=f.fee_code and f.fee_type<>'Excess Amount' and d.name = r.app_formno+'-'+r.stud_name  and f.header_id in (" + head + ") and r.batch_year in(" + batch_all + ") and r.degree_code in('" + course_all + "') and d.cal_date between '" + altdat + "' and '" + duedat + "' and debit=0 and vouchertype=1 and isnull(r.is_enroll,'0')<>'2'  group by r.app_formno,r.stud_name,r.degree_code,r.Current_Semester,f.header_id";
                                                                        //and r.app_no not in (select app_no from Registration where batch_year in ( " + batch_all + "))
                                                                    }
                                                                    ds1 = da.select_method_wo_parameter(paidquery, "Text");


                                                                    if (ds1.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        ds1.Tables[0].DefaultView.RowFilter = "roll_no='" + roll + "' and header_id='" + code + "' ";
                                                                        dv_paid_data = ds1.Tables[0].DefaultView;
                                                                    }
                                                                    string paid_amt = string.Empty;
                                                                    if (dv_paid_data.Count > 0)
                                                                    {
                                                                        paid_amt = dv_paid_data[0]["paid"].ToString();
                                                                    }
                                                                    else
                                                                    {
                                                                        paid_amt = "0";
                                                                    }
                                                                    if (demand_amt == "0" && paid_amt == "0")
                                                                    {
                                                                        finalflag = true;
                                                                        FpSpread1.Sheets[0].Cells[r, d].Text = "-";
                                                                        FpSpread1.Sheets[0].Cells[r, d].HorizontalAlign = HorizontalAlign.Center;
                                                                    }
                                                                    else
                                                                    {
                                                                        finalflag = true;
                                                                        double due_amt = Convert.ToDouble(Convert.ToDouble(demand_amt) - Convert.ToDouble(paid_amt));
                                                                        if (due_amt < 0)
                                                                        {
                                                                            due_amt = 0;
                                                                        }
                                                                        FpSpread1.Sheets[0].Cells[r, d].Text = due_amt.ToString();
                                                                        FpSpread1.Sheets[0].Cells[r, d].HorizontalAlign = HorizontalAlign.Right;
                                                                        total_due = total_due + Convert.ToDouble(due_amt);
                                                                    }
                                                                }
                                                                if (total_due == 0)
                                                                {
                                                                    FpSpread1.Sheets[0].Rows[r].Visible = false;
                                                                }
                                                                else
                                                                {
                                                                    i = i + 1;
                                                                    FpSpread1.Sheets[0].Cells[r, 0].Text = i.ToString();
                                                                    FpSpread1.Sheets[0].Cells[r, FpSpread1.Sheets[0].Columns.Count - 1].Text = total_due.ToString();
                                                                    FpSpread1.Sheets[0].Cells[r, FpSpread1.Sheets[0].Columns.Count - 1].HorizontalAlign = HorizontalAlign.Right;
                                                                }
                                                                if (FpSpread1.Sheets[0].Cells[r, 0].Text == "Total")
                                                                {
                                                                    FpSpread1.Sheets[0].Rows[r].Visible = true;
                                                                }
                                                            }
                                                        }
                                                    }

                                                    FpSpread1.Sheets[0].RowCount++;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Degree Wise Total";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 5);
                                                    if (ddlfeetype.Text == "Yet To Be Paid")
                                                    {
                                                        for (int d = 5; d < FpSpread1.Sheets[0].Columns.Count; d = d + 1)
                                                        {
                                                            double totaldue = 0;
                                                            if (FpSpread1.Sheets[0].RowCount > 0)
                                                            {
                                                                for (int r = startrow; r < FpSpread1.Sheets[0].RowCount; r++)
                                                                {
                                                                    if (FpSpread1.Sheets[0].Cells[r, 0].ColumnSpan >= 1)
                                                                    {
                                                                        double val = 0;
                                                                        string amount = FpSpread1.Sheets[0].Cells[r, d].Text;
                                                                        if (amount == "-")
                                                                        {
                                                                            val = 0;
                                                                        }
                                                                        else
                                                                        {
                                                                            if (amount == "")
                                                                            {
                                                                                val = 0;
                                                                            }
                                                                            else
                                                                            {
                                                                                val = Convert.ToDouble(amount);
                                                                            }
                                                                        }
                                                                        totaldue = totaldue + val;
                                                                    }
                                                                }
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Text = totaldue.ToString();
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].HorizontalAlign = HorizontalAlign.Right;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Date Wise Total";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 5);
                                    if (ddlfeetype.Text == "Yet To Be Paid")
                                    {
                                        for (int d = 5; d < FpSpread1.Sheets[0].Columns.Count; d = d + 1)
                                        {
                                            double totaldue = 0;
                                            if (FpSpread1.Sheets[0].RowCount > 0)
                                            {
                                                for (int r = datestartrow; r < FpSpread1.Sheets[0].RowCount; r++)
                                                {
                                                    if (FpSpread1.Sheets[0].Cells[r, 0].ColumnSpan <= 1)
                                                    {
                                                        double val = 0;
                                                        string amount = FpSpread1.Sheets[0].Cells[r, d].Text;
                                                        if (amount == "-")
                                                        {
                                                            val = 0;
                                                        }
                                                        else
                                                        {
                                                            if (amount == "")
                                                            {
                                                                val = 0;
                                                            }
                                                            else
                                                            {
                                                                val = Convert.ToDouble(amount);
                                                            }
                                                        }
                                                        totaldue = totaldue + val;
                                                    }
                                                }
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Text = totaldue.ToString();
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].HorizontalAlign = HorizontalAlign.Right;
                                            }
                                        }
                                    }
                                }
                            }
                            if (FpSpread1.Sheets[0].RowCount > 2)
                            {
                                FpSpread1.Sheets[0].RowCount++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 5);
                                if (ddlfeetype.Text == "Yet To Be Paid")
                                {
                                    for (int d = 5; d < FpSpread1.Sheets[0].Columns.Count; d = d + 1)
                                    {
                                        double totaldue = 0;
                                        if (FpSpread1.Sheets[0].RowCount > 0)
                                        {
                                            for (int r = 0; r < FpSpread1.Sheets[0].RowCount; r++)
                                            {
                                                if (FpSpread1.Sheets[0].Cells[r, 0].ColumnSpan <= 1)
                                                {
                                                    double val = 0;
                                                    string amount = FpSpread1.Sheets[0].Cells[r, d].Text;
                                                    if (amount == "-")
                                                    {
                                                        val = 0;
                                                    }
                                                    else
                                                    {
                                                        if (amount == "")
                                                        {
                                                            val = 0;
                                                        }
                                                        else
                                                        {
                                                            val = Convert.ToDouble(amount);
                                                        }
                                                    }

                                                    totaldue = totaldue + val;
                                                }
                                            }
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Text = totaldue.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].HorizontalAlign = HorizontalAlign.Right;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            errmsg.Visible = true;
                            errmsg.Text = "No Records Found";
                            FpSpread1.Visible = false;
                            btnprintmaster.Visible = false;
                            lblrptname.Visible = false;
                            txtexcelname.Visible = false;
                            btnxl.Visible = false;
                            lblc1.Visible = false;
                            lblc2.Visible = false;
                            lblc3.Visible = false;
                            lblc4.Visible = false;
                            lblc5.Visible = false;
                            lblcard.Visible = false;
                        }
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        FpSpread1.Visible = true;
                        btnprintmaster.Visible = true;
                        lblrptname.Visible = true;
                        txtexcelname.Visible = true;
                        btnxl.Visible = true;
                        errmsg.Visible = false;
                    }
                    #endregion
                }
                else//Header..............
                {
                    #region header

                    if (ddlfeetype.Text == "---Select---")
                    {
                        errmsg.Visible = true;
                        errmsg.Text = "Please Select Fee Type";
                        FpSpread1.Visible = false;
                        btnprintmaster.Visible = false;
                        lblrptname.Visible = false;
                        txtexcelname.Visible = false;
                        btnxl.Visible = false;
                    }
                    else
                    {                  //Spread binding Start...................                       

                        string feecategorycolumn = ",f.headerfk";
                        string feecategorycolumngroup = feecategorycolumn + ",f.headerfk";
                        string demquery = "select distinct r.stud_name as Name,r.Current_Semester,batch_year,r.Roll_No,r.roll_admit,r.degree_code,CONVERT(varchar(10), a.paystartdate,103) as DueDate,CONVERT(varchar(10), a.allotdate,103) as allotdate,sum(totalamount) as demand ,a.headerfk as header_id from ft_feeallot a,registration r where  a.app_no=r.app_no and a.totalamount>0 and ISNULL (BalAmount ,TotalAmount)<>0 " + delflg + "  and a.headerfk in('" + header_all + "') and r.degree_code in('" + course_all + "') and r.Batch_year in(" + batch_all + ") and a.feecategory in(" + sem + ")  and a.paystartdate between '" + y1 + "' and '" + y2 + "' group by a.app_no,batch_year,r.roll_admit,r.roll_no,r.stud_name,r.degree_code,r.Current_Semester,CONVERT(varchar(10), a.paystartdate,103),CONVERT(varchar(10), a.allotdate,103),a.headerfk";
                        if (cbbfadm.Checked == true)
                        {
                            demquery += " union all select distinct r.stud_name as Name,r.Current_Semester,'' Roll_No ,''roll_admit,batch_year,r.degree_code, CONVERT(varchar(10), a.paystartdate,103) as DueDate,CONVERT(varchar(10), a.allotdate,103) as allotdate,sum(totalamount) as demand ,a.headerfk as header_id from ft_feeallot a,applyn r where  a.app_no=r.app_no and a.totalamount>0 and r.isconfirm = 1 and r.admission_status = 0 and a.headerfk in('" + header_all + "') and r.degree_code in(" + course_all + ") and r.Batch_year in(" + batch_all + ") and a.feecategory in(" + sem + ")  and a.paystartdate between '" + y1 + "' and '" + y2 + "'  group by r.app_no,r.app_formno ,r.stud_name,r.degree_code,batch_year,r.Current_Semester,CONVERT(varchar(10), a.paystartdate,103),CONVERT(varchar(10), a.allotdate,103) ,a.headerfk";
                        }

                        ds = da.select_method_wo_parameter(demquery, "Text");
                        DataTable dttemp = ds.Tables[0];

                        string degquery = "select c.course_id,c.course_name,d.degree_code,d.acronym from course c,degree d where c.course_id=d.course_id";
                        ds2 = da.select_method_wo_parameter(degquery, "Text");

                        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.BackColor = Color.FromArgb(214, 235, 255);
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                        FpSpread1.Sheets[0].RowHeader.Width = 300;
                        FpSpread1.Sheets[0].ColumnHeader.Columns.Count = 5;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Student Name";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No/Application No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Course";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Sem";
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);

                        for (int a = 0; a < chklstaccheader.Items.Count; a++)
                        {
                            if (chklstaccheader.Items[a].Selected == true)
                            {

                                if (ddlfeetype.Text == "Yet To Be Paid")
                                {
                                    FpSpread1.Sheets[0].ColumnCount++;
                                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Width = 200;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = chklstaccheader.Items[a].Text;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Balance";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Note = chklstaccheader.Items[a].Value;
                                }
                            }
                        }
                        if (ddlfeetype.Text == "Yet To Be Paid")
                        {
                            FpSpread1.Sheets[0].ColumnCount++;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Total Balance";
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1, 2, 1);
                        }

                        //Added by Venkat 6/9/2014============================Start
                        if (ddlstudtype.SelectedItem.Text == "Regular" || ddlstudtype.SelectedItem.Text == "Lateral" || ddlstudtype.SelectedItem.Text == "Transfer")
                        {
                            string str = "select distinct r.stud_name as Name,r.Current_Semester,r.Reg_No,r.Roll_No,r.degree_code,roll_admit,r.batch_year from registration r where r.batch_year in(" + batch_all + ")  and r.mode='" + ddlstudtype.SelectedValue + "' " + strorder + "";
                            ds3 = da.select_method_wo_parameter(str, "text");
                        }
                        else if (ddlstudtype.SelectedItem.Text == "Re-admit")
                        {
                            string str = "select distinct r.stud_name as Name,r.Current_Semester,r.Reg_No,r.Roll_No,r.degree_code,roll_admit,r.batch_year from Readmission a,Registration r where a.App_no=r.App_No and a.newBatchYear in(" + batch_all + ") and  r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' " + strorder + "";
                            ds3 = da.select_method_wo_parameter(str, "text");
                        }
                        else if (ddlstudtype.SelectedItem.Text == "Re-join")
                        {
                            string str = "select distinct r.stud_name as Name,r.Current_Semester,r.Reg_No,r.Roll_No,r.degree_code,roll_admit,r.batch_year from rejoin_list a,Registration r where a.roll_no=r.Roll_No and a.newBatchYear in(" + batch_all + ") and  r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' " + strorder + "";
                            ds3 = da.select_method_wo_parameter(str, "text");
                        }
                        else
                        {
                            string str = "select distinct r.stud_name as Name,r.Current_Semester,r.Reg_No,r.Roll_No,r.degree_code,roll_admit,r.degree_code,r.batch_year from registration r where r.cc=0 and delflag=0 and exam_flag<>'debar'";

                            if (cbbfadm.Checked == true)
                            {
                                str += " union select distinct r.stud_name as Name,r.Current_Semester,'' as Reg_No,r.app_formno as Roll_No,r.degree_code,'' as roll_admit,r.degree_code,r.batch_year from applyn r where isnull(r.is_enroll,'0')<>'2'   " + strorder + "";
                                //r.app_no not in (select app_no from Registration where batch_year in (" + batch_all + "))
                            }
                            ds3 = da.select_method_wo_parameter(str, "text");
                        }

                        //==========================End
                        DataView dvstucoun = new DataView();
                        string date = "";
                        string fnldate = "";
                        DateTime dtdatec;
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            int sno = 0;
                            for (dtdatec = Convert.ToDateTime(y1); dtdatec <= Convert.ToDateTime(y2); dtdatec = dtdatec.AddDays(1))
                            {
                                dttemp.DefaultView.RowFilter = " DueDate='" + dtdatec.ToString("dd/MM/yyyy").Split(' ')[0] + "'";
                                date = dtdatec.ToString("dd/MM/yyyy").Split(' ')[0];

                                DataView dvdatefinance = dttemp.DefaultView;
                                if (dvdatefinance.Count > 0)
                                {
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = dtdatec.ToString("dd/MM/yyyy");
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, FpSpread1.Sheets[0].Columns.Count);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = Color.LightCyan;
                                    datestartrow = FpSpread1.Sheets[0].RowCount;

                                    string[] spl_course = tot_course.Split('/');
                                    string[] spl_batch = batch_all.Split(',');
                                    for (int splbat = 0; spl_batch.GetUpperBound(0) >= splbat; splbat++)
                                    {
                                        string batcyear = spl_batch[splbat].ToString();

                                        for (int spl = 0; spl_course.GetUpperBound(0) >= spl; spl++)
                                        {
                                            deptflag = false;
                                            string courseid = spl_course[spl].ToString();
                                            DataView dv_demand_data = new DataView();

                                            dttemp.DefaultView.RowFilter = "degree_code='" + courseid + "' and batch_year='" + batcyear + "' and duedate='" + date + "'";
                                            dv_demand_data = dttemp.DefaultView;
                                            int count4 = dv_demand_data.Count;
                                            if (dv_demand_data.Count > 0)
                                            {
                                                DataView dv_get_course = new DataView();
                                                ds2.Tables[0].DefaultView.RowFilter = "degree_code='" + courseid + "' ";
                                                dv_get_course = ds2.Tables[0].DefaultView;

                                                ds3.Tables[0].DefaultView.RowFilter = " degree_code='" + courseid + "' and batch_year='" + batcyear + "'";
                                                dvstucoun = ds3.Tables[0].DefaultView;
                                                startrow = FpSpread1.Sheets[0].RowCount;
                                                if (dvstucoun.Count > 0)
                                                {
                                                    for (int i = 0; i < dvstucoun.Count; i++)
                                                    {
                                                        string name = dvstucoun[i]["Name"].ToString();
                                                        string name_roll = dvstucoun[i]["Roll_No"].ToString();
                                                        //+ "-" + dv_demand_data[i]["Name"].ToString();
                                                        string year = dvstucoun[i]["Current_Semester"].ToString();
                                                        string rollno = dvstucoun[i]["Roll_No"].ToString();
                                                        string deg_dept = string.Empty;
                                                        if (dv_get_course.Count > 0)
                                                        {
                                                            deg_dept = dv_get_course[0]["course_name"].ToString() + "-" + dv_get_course[0]["acronym"].ToString();
                                                        }
                                                        if (deptflag == false)
                                                        {
                                                            FpSpread1.Sheets[0].RowCount++;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = batcyear + "-" + deg_dept + "/Sem-" + year;
                                                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, FpSpread1.Sheets[0].Columns.Count);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = Color.LightCyan;
                                                            deptflag = true;
                                                        }

                                                        FpSpread1.Sheets[0].RowCount++;
                                                        sno++;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = name;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dvstucoun[i]["Roll_No"].ToString();
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = deg_dept;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = year;
                                                        FpSpread1.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                                        FpSpread1.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                                        FpSpread1.Visible = true;
                                                        btnprintmaster.Visible = true;
                                                        lblrptname.Visible = true;
                                                        txtexcelname.Visible = true;
                                                        btnxl.Visible = true;
                                                        errmsg.Visible = false;
                                                        lblc1.Visible = false;
                                                        lblc2.Visible = false;
                                                        lblc3.Visible = false;
                                                        lblc4.Visible = false;
                                                        lblc5.Visible = false;
                                                        lblcard.Visible = false;
                                                    }
                                                }
                                            }

                                            if (dv_demand_data.Count > 0)
                                            {
                                                for (int r = startrow; r < FpSpread1.Sheets[0].RowCount; r++)
                                                {
                                                    if (FpSpread1.Sheets[0].Cells[r, 2].Text.ToString() != "")
                                                    {
                                                        double total_due = 0;
                                                        string name = FpSpread1.Sheets[0].Cells[r, 1].Text.ToString();
                                                        string roll = FpSpread1.Sheets[0].Cells[r, 2].Text.ToString();
                                                        string name_roll = roll + "-" + name;
                                                        string altdat = "";
                                                        string duedat = "";
                                                        for (int d = 5; d < FpSpread1.Sheets[0].Columns.Count - 1; d = d + 1)
                                                        {
                                                            string demand_amt = "";
                                                            string code = FpSpread1.Sheets[0].ColumnHeader.Cells[1, d].Note.ToString();

                                                            DataView dv_demand_amount = new DataView();
                                                            if (ds.Tables[0].Rows.Count > 0)
                                                            {
                                                                ds.Tables[0].DefaultView.RowFilter = "roll_no='" + roll + "' and header_id='" + code + "' and duedate='" + date + "'";
                                                                dv_demand_amount = ds.Tables[0].DefaultView;
                                                            }

                                                            if (dv_demand_amount.Count > 0)
                                                            {
                                                                demand_amt = dv_demand_amount[0]["demand"].ToString();
                                                                altdat = dv_demand_amount[0]["allotdate"].ToString();
                                                                duedat = dv_demand_amount[0]["DueDate"].ToString();
                                                            }
                                                            else
                                                            {
                                                                demand_amt = "0";
                                                            }

                                                            string paidquery = "select distinct r.stud_name as Name,r.Current_Semester, r.Roll_No,sum(a.debit) as paid,r.degree_code  ,a.headerfk as header_id ,f.headerfk from ft_findailytransaction a,fm_ledgermaster f,registration r where a.ledgerfk=f.ledgerpk and f.ledgername <>'Excess Amount' and a.app_no = r.app_no   and a.TransDate between '" + altdat + "' and '" + duedat + "'  and a.headerfk in('" + header_all + "') and r.degree_code in('" + course_all + "') and r.Batch_year in(" + batch_all + ")  and credit=0 and transtype=1 and a.debit>0 and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'";
                                                            if (usBasedRights == true)
                                                                paidquery = " and a.EntryUserCode in('" + usercode + "')";
                                                            paidquery += " group by r.roll_no,r.stud_name,r.degree_code,r.Current_Semester ,a.headerfk" + feecategorycolumngroup + "  ";
                                                            //order by isnull(f.priority,1000), f.ledgerName asc
                                                            if (cbbfadm.Checked == true)
                                                            {
                                                                paidquery += " union select distinct r.stud_name as Name,r.Current_Semester,r.app_formno as Roll_No,sum(a.debit) as paid,r.degree_code ,a.headerfk as header_id " + feecategorycolumn + " from ft_findailytransaction a,applyn r  where  a.app_no = r.app_no and a.TransDate between '" + altdat + "' and '" + duedat + "'  and a.headerfk in('" + header_all + "') and r.degree_code in(" + course_all + ") and r.Batch_year in(" + batch_all + ") and credit=0 and transtype=1 and a.debit>0  and r.isconfirm = 1 and r.admission_status = 0 and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' ";
                                                                if (usBasedRights == true)
                                                                    paidquery = " and a.EntryUserCode in('" + usercode + "')";
                                                                paidquery += " group by r.app_no,r.app_formno,r.stud_name,r.degree_code,r.Current_Semester,a.headerfk ";
                                                            }
                                                            ds1 = da.select_method_wo_parameter(paidquery, "Text");


                                                            DataView dv_paid_data = new DataView();
                                                            if (ds1.Tables[0].Rows.Count > 0)
                                                            {
                                                                ds1.Tables[0].DefaultView.RowFilter = "roll_no='" + roll + "' and header_id='" + code + "'";
                                                                dv_paid_data = ds1.Tables[0].DefaultView;
                                                            }
                                                            string paid_amt = string.Empty;
                                                            if (dv_paid_data.Count > 0)
                                                            {
                                                                paid_amt = dv_paid_data[0]["paid"].ToString();
                                                            }
                                                            else
                                                            {
                                                                paid_amt = "0";
                                                            }
                                                            if (demand_amt == "0" && paid_amt == "0")
                                                            {
                                                                finalflag = true;
                                                                FpSpread1.Sheets[0].Cells[r, d].Text = "-";
                                                                FpSpread1.Sheets[0].Cells[r, d].HorizontalAlign = HorizontalAlign.Center;
                                                            }
                                                            else
                                                            {
                                                                finalflag = true;
                                                                double due_amt = Convert.ToDouble(Convert.ToDouble(demand_amt) - Convert.ToDouble(paid_amt));
                                                                if (due_amt < 0)
                                                                {
                                                                    due_amt = 0;
                                                                }
                                                                FpSpread1.Sheets[0].Cells[r, d].Text = due_amt.ToString();
                                                                FpSpread1.Sheets[0].Cells[r, d].HorizontalAlign = HorizontalAlign.Right;
                                                                total_due = total_due + Convert.ToDouble(due_amt);
                                                            }
                                                        }
                                                        if (total_due == 0)
                                                        {
                                                            FpSpread1.Sheets[0].Rows[r].Visible = false;
                                                        }
                                                        else
                                                        {
                                                            i = i + 1;
                                                            FpSpread1.Sheets[0].Cells[r, 0].Text = i.ToString();
                                                            FpSpread1.Sheets[0].Cells[r, FpSpread1.Sheets[0].Columns.Count - 1].Text = total_due.ToString();
                                                            FpSpread1.Sheets[0].Cells[r, FpSpread1.Sheets[0].Columns.Count - 1].HorizontalAlign = HorizontalAlign.Right;
                                                        }
                                                        if (FpSpread1.Sheets[0].Cells[r, 0].Text == "Total")
                                                        {
                                                            FpSpread1.Sheets[0].Rows[r].Visible = true;
                                                        }
                                                    }
                                                }
                                                FpSpread1.Sheets[0].RowCount++;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Degree Wise Total";
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 5);

                                                for (int d = 5; d < FpSpread1.Sheets[0].Columns.Count; d = d + 1)
                                                {
                                                    double totaldue = 0;
                                                    if (FpSpread1.Sheets[0].RowCount > 0)
                                                    {
                                                        for (int r = startrow; r < FpSpread1.Sheets[0].RowCount; r++)
                                                        {
                                                            if (FpSpread1.Sheets[0].Cells[r, 0].ColumnSpan <= 1)
                                                            {
                                                                double val = 0;
                                                                string amount = FpSpread1.Sheets[0].Cells[r, d].Text;

                                                                if (amount == "-")
                                                                {
                                                                    val = 0;
                                                                }
                                                                else
                                                                {
                                                                    if (amount == "")
                                                                    {
                                                                        val = 0;
                                                                    }
                                                                    else
                                                                    {
                                                                        val = Convert.ToDouble(amount);
                                                                    }
                                                                }

                                                                totaldue = totaldue + val;
                                                            }
                                                        }
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Text = totaldue.ToString();
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].HorizontalAlign = HorizontalAlign.Right;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Date Wise Total";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 5);
                                    for (int d = 5; d < FpSpread1.Sheets[0].Columns.Count; d = d + 1)
                                    {
                                        double totaldue = 0;
                                        if (FpSpread1.Sheets[0].RowCount > 0)
                                        {
                                            for (int r = datestartrow; r < FpSpread1.Sheets[0].RowCount; r++)
                                            {
                                                if (FpSpread1.Sheets[0].Cells[r, 0].ColumnSpan <= 1)
                                                {
                                                    double val = 0;
                                                    string amount = FpSpread1.Sheets[0].Cells[r, d].Text;

                                                    if (amount == "-")
                                                    {
                                                        val = 0;
                                                    }
                                                    else
                                                    {
                                                        if (amount == "")
                                                        {
                                                            val = 0;
                                                        }
                                                        else
                                                        {
                                                            val = Convert.ToDouble(amount);
                                                        }
                                                    }
                                                    totaldue = totaldue + val;
                                                }
                                            }
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Text = totaldue.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].HorizontalAlign = HorizontalAlign.Right;
                                        }
                                    }
                                }
                            }
                            if (FpSpread1.Sheets[0].RowCount > 0)
                            {
                                FpSpread1.Sheets[0].RowCount++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 5);
                                for (int d = 5; d < FpSpread1.Sheets[0].Columns.Count; d = d + 1)
                                {
                                    double totaldue = 0;
                                    if (FpSpread1.Sheets[0].RowCount > 0)
                                    {
                                        for (int r = 0; r < FpSpread1.Sheets[0].RowCount; r++)
                                        {
                                            if (FpSpread1.Sheets[0].Cells[r, 0].ColumnSpan <= 1)
                                            {
                                                double val = 0;
                                                string amount = FpSpread1.Sheets[0].Cells[r, d].Text;

                                                if (amount == "-")
                                                {
                                                    val = 0;
                                                }
                                                else
                                                {
                                                    if (amount == "")
                                                    {
                                                        val = 0;
                                                    }
                                                    else
                                                    {
                                                        val = Convert.ToDouble(amount);
                                                    }
                                                }

                                                totaldue = totaldue + val;
                                            }
                                        }
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Text = totaldue.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].HorizontalAlign = HorizontalAlign.Right;
                                    }
                                }
                            }
                        }
                        else
                        {
                            errmsg.Visible = true;
                            errmsg.Text = "No Records Found";
                            FpSpread1.Visible = false;
                            btnprintmaster.Visible = false;
                            lblrptname.Visible = false;
                            txtexcelname.Visible = false;
                            btnxl.Visible = false;
                            lblc1.Visible = false;
                            lblc2.Visible = false;
                            lblc3.Visible = false;
                            lblc4.Visible = false;
                            lblc5.Visible = false;
                            lblcard.Visible = false;
                        }
                    }
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    #endregion
                }
            }
            if (finalflag == false)
            {
                errmsg.Visible = true;
                errmsg.Text = "No Records Found";
                FpSpread1.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                lblc1.Visible = false;
                lblc2.Visible = false;
                lblc3.Visible = false;
                lblc4.Visible = false;
                lblc5.Visible = false;
                lblcard.Visible = false;
            }
        }
        catch
        {
        }
    }
    protected void chkdate_CheckedChanged(object sender, EventArgs e)
    {
        checkcoursetot.Visible = false;
        if (chkdate.Checked == true)
        {
            txtdate.Enabled = true;
            txtto.Enabled = true;
            if (ddlfeetype.SelectedItem.Text == "Paid" || ddlfeetype.SelectedItem.Text == "Yet To Be Paid")
            {
                checkcoursetot.Visible = true;
            }
        }
        else
        {
            txtdate.Enabled = false;
            txtto.Enabled = false;
        }
    }
    protected void chkfeesem_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txtfeesem.Text = "--Select--";
            string sem = "";
            if (chkfeesem.Checked == true)
            {
                for (int i = 0; i < chklsfeesem.Items.Count; i++)
                {
                    chklsfeesem.Items[i].Selected = true;
                    sem = Convert.ToString(chklsfeesem.Items[i].Text);
                }
                if (chklsfeesem.Items.Count == 1)
                {
                    txtfeesem.Text = " (" + sem + ")";
                }
                else
                {
                    txtfeesem.Text = "Semester (" + (chklsfeesem.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklsfeesem.Items.Count; i++)
                {
                    chklsfeesem.Items[i].Selected = false;
                    txtfeesem.Text = "--Select--";
                }
            }
        }
        catch
        {
        }
    }

    protected void chklsfeesem_SelectedIndexChanged(object sender, EventArgs e)
    {
        int ddlcount = 0;
        try
        {
            txtfeesem.Text = "--Select--";
            string sem = "";
            chkfeesem.Checked = false;
            for (int i = 0; i < chklsfeesem.Items.Count; i++)
            {
                if (chklsfeesem.Items[i].Selected == true)
                {
                    ddlcount = ddlcount + 1;
                    sem = Convert.ToString(chklsfeesem.Items[i].Text);
                }
            }
            if (ddlcount == 0)
            {
                txtfeesem.Text = "---Select---";
            }
            else
            {
                if (ddlcount == 1)
                {
                    txtfeesem.Text = " (" + sem + ")";
                }
                else
                {
                    txtfeesem.Text = "Semester (" + ddlcount.ToString() + ")";
                }
                if (ddlcount == chklsfeesem.Items.Count)
                {
                    chkfeesem.Checked = true;
                }
            }
        }
        catch
        {
        }
    }

    protected void bindsem()
    {
        try
        {
            chklsfeesem.Items.Clear();
            chkfeesem.Checked = false;
            txtfeesem.Text = "--Select--";
            ds.Clear();
            string linkName = string.Empty;
            string cbltext = string.Empty;
            ds = da.loadFeecategory(Convert.ToString(ddl_collegename.SelectedItem.Value), usercode, ref linkName);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                chklsfeesem.DataSource = ds;
                chklsfeesem.DataTextField = "TextVal";
                chklsfeesem.DataValueField = "TextCode";
                chklsfeesem.DataBind();
            }
        }
        catch { }
    }


    //protected void bindsem()
    //{
    //    try
    //    {
    //        string sem = "";
    //        // string clgvalue = ddl_collegename.SelectedItem.Value.ToString();
    //        string semyear = "select * from New_InsSettings where linkname = 'SemesterandYear' and user_code ='" + usercode + "'and college_code ='" + collegecode + "'";
    //        DataSet dsset = new DataSet();
    //        dsset.Clear();
    //        dsset = da.select_method_wo_parameter(semyear, "Text");
    //        if (dsset.Tables.Count > 0 && dsset.Tables[0].Rows.Count > 0)
    //        {
    //            string value = Convert.ToString(dsset.Tables[0].Rows[0]["LinkValue"]);
    //            if (value == "1")
    //            {
    //                string SelectQ = "select * from textvaltable where TextCriteria = 'FEECA'and (textval like '%Semester' or textval like '%Year') and textval not like '-1%'and college_code ='" + collegecode + "' order by len(textval),textval asc";
    //                ds.Clear();
    //                ds = da.select_method_wo_parameter(SelectQ, "Text");
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    chklsfeesem.DataSource = ds;
    //                    chklsfeesem.DataTextField = "textval";
    //                    chklsfeesem.DataValueField = "TextCode";
    //                    chklsfeesem.DataBind();
    //                }

    //            }
    //            else
    //            {
    //                chklsfeesem.Items.Clear();
    //                string settingquery = "select * from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'";
    //                ds.Clear();
    //                ds = da.select_method_wo_parameter(settingquery, "Text");
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    string linkvalue = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
    //                    if (linkvalue == "0")
    //                    {
    //                        string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Semester' and textval not like '-1%' and college_code ='" + collegecode + "' order by len(textval),textval asc";
    //                        ds.Clear();
    //                        ds = da.select_method_wo_parameter(semesterquery, "Text");
    //                        if (ds.Tables[0].Rows.Count > 0)
    //                        {
    //                            chklsfeesem.DataSource = ds;
    //                            chklsfeesem.DataTextField = "textval";
    //                            chklsfeesem.DataValueField = "TextCode";
    //                            chklsfeesem.DataBind();
    //                        }
    //                    }
    //                    else
    //                    {
    //                        string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Year' and textval not like '-1%' and college_code ='" + collegecode + "' order by len(textval),textval asc";
    //                        ds.Clear();
    //                        ds = da.select_method_wo_parameter(semesterquery, "Text");
    //                        if (ds.Tables[0].Rows.Count > 0)
    //                        {
    //                            chklsfeesem.DataSource = ds;
    //                            chklsfeesem.DataTextField = "textval";
    //                            chklsfeesem.DataValueField = "TextCode";
    //                            chklsfeesem.DataBind();
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch { }
    //}

    public string returnYearforSem(string cursem)
    {
        switch (cursem)
        {
            case "1":
            case "2":
                cursem = "1";
                break;
            case "3":
            case "4":
                cursem = "2";
                break;
            case "5":
            case "6":
                cursem = "3";
                break;
            case "7":
            case "8":
                cursem = "4";
                break;
            case "9":
            case "10":
                cursem = "5";
                break;
        }
        return cursem;
    }

    private string getCheckboxListValue(CheckBoxList cbl)
    {
        System.Text.StringBuilder cblvalue = new System.Text.StringBuilder();
        int sel = 0;
        try
        {
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    if (cblvalue.Length == 0)
                    {
                        cblvalue.Append(Convert.ToString(cbl.Items[sel].Value));
                    }
                    else
                    {
                        cblvalue.Append("','" + Convert.ToString(cbl.Items[sel].Value));
                    }
                }
            }
        }
        catch { cbl.Items.Clear(); }
        return cblvalue.ToString();
    }

    private string getCheckboxListText(CheckBoxList cbl)
    {
        System.Text.StringBuilder cbltext = new System.Text.StringBuilder();
        int sel = 0;
        try
        {
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    if (cbltext.Length == 0)
                    {
                        cbltext.Append(Convert.ToString(cbl.Items[sel].Text));
                    }
                    else
                    {
                        cbltext.Append("','" + Convert.ToString(cbl.Items[sel].Text));
                    }
                }
            }
        }
        catch { cbl.Items.Clear(); }
        return cbltext.ToString();
    }

    #region Include setting
    protected void checkdicon_Changed(object sender, EventArgs e)
    {
        try
        {
            if (checkdicon.Checked == true)
            {
                txtinclude.Enabled = true;
                LoadIncludeSetting();
            }
            else
            {
                txtinclude.Enabled = false;
                cblinclude.Items.Clear();
                // LoadIncludeSetting();
            }
        }
        catch { }
    }

    private void LoadIncludeSetting()
    {
        try
        {
            cblinclude.Items.Clear();
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("Course Completed", "1"));
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("Debar", "2"));
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("Discontinue", "3"));
            if (cblinclude.Items.Count > 0)
            {
                for (int i = 0; i < cblinclude.Items.Count; i++)
                {
                    cblinclude.Items[i].Selected = true;
                }
                cbinclude.Checked = true;
                txtinclude.Text = "Include Settings(" + cblinclude.Items.Count + ")";
            }
        }
        catch { }
    }


    protected void cbinclude_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cbinclude, cblinclude, txtinclude, "Include Setting", "--Select--");
        }
        catch { }
    }
    protected void cblinclude_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cbinclude, cblinclude, txtinclude, "Include Setting", "--Select--");
        }
        catch { }
    }


    #endregion


    #region Include bank
    protected void cbbankcheck_Changed(object sender, EventArgs e)
    {
        try
        {
            if (cbbankcheck.Checked == true)
            {
                txtbankname.Enabled = true;
                loadBank();
            }
            else
            {
                txtbankname.Enabled = false;
                cblbank.Items.Clear();
                // LoadIncludeSetting();
            }
        }
        catch { }
    }

    private void loadBank()
    {
        try
        {
            string selqry = "select TextCode,TextVal  from textvaltable where TextCriteria = 'BName'";
            ds = da.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblbank.DataSource = ds;
                cblbank.DataTextField = "TextVal";
                cblbank.DataValueField = "TextCode";
                cblbank.DataBind();
                if (cblbank.Items.Count > 0)
                {
                    for (i = 0; i < cblbank.Items.Count; i++)
                    {
                        cblbank.Items[i].Selected = true;
                    }
                    txtbankname.Text = "Bank(" + cblbank.Items.Count + ")";
                    cbbank.Checked = true;
                }
            }
        }
        catch { }
    }


    protected void cbbank_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cbbank, cblbank, txtbankname, "Include Bank", "--Select--");
        }
        catch { }
    }
    protected void cblbank_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cbbank, cblbank, txtbankname, "Include Bank", "--Select--");
        }
        catch { }
    }


    #endregion

    #region Common Checkbox and Checkboxlist Event

    private string getCblSelectedValue(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedvalue = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedvalue.Length == 0)
                    {
                        selectedvalue.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        selectedvalue.Append("','" + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedvalue.ToString();
    }
    private string getCblSelectedText(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedText = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append(Convert.ToString(cblSelected.Items[sel].Text));
                    }
                    else
                    {
                        selectedText.Append("','" + Convert.ToString(cblSelected.Items[sel].Text));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedText.ToString();
    }
    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = "";
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
                if (cbl.Items.Count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
                }
            }
            else
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = false;
                }
                txt.Text = deft;
            }
        }
        catch { }
    }
    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst, string deft)
    {
        try
        {
            int sel = 0;
            int count = 0;
            string name = "";
            cb.Checked = false;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dipst + "(" + count + ")";
                }
                if (cbl.Items.Count == count)
                {
                    cb.Checked = true;
                }
            }
        }
        catch { }
    }

    #endregion

    protected void UserbasedRights()
    {
        string userrht = da.GetFunction("select value from Master_Settings where settings='Finance Include User Based Report Settings'  and usercode='" + usercode + "'");
        if (userrht == "1")
            usBasedRights = true;
        else
            usBasedRights = false;

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

        lbl.Add(lbl_collegename);
        lbl.Add(lbltype);
        lbl.Add(lbldegree);
        lbl.Add(lblbranch);
        lbl.Add(lblfeesem);
        fields.Add(0);
        fields.Add(1);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }

    protected string sclSett()
    {
        string sclType = string.Empty;
        try
        {
            sclType = da.GetFunction("select value from Master_Settings where settings='schoolorcollege' and usercode='" + usercode + "'");

        }
        catch { }
        return sclType;
    }

    private double checkSchoolSetting()
    {
        double getVal = 0;
        try
        {
            double.TryParse(Convert.ToString(da.GetFunction("select  value from Master_Settings where settings='schoolorcollege' and usercode='" + usercode + "'")), out getVal);

        }
        catch { }
        return getVal;
    }

    #region roll,reg,admission setting
    private void RollAndRegSettings()
    {
        try
        {
            DataSet dsl = new DataSet();
            string Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
            dsl = da.select_method_wo_parameter(Master1, "text");
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Admission"] = "0";
            if (dsl.Tables[0].Rows.Count > 0)
            {
                for (int hf = 0; hf < dsl.Tables[0].Rows.Count; hf++)
                {
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Roll No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Register No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Admission No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Admission"] = "1";
                    }
                }
                settingValueRollAndReg(Convert.ToString(Session["Rollflag"]), Convert.ToString(Session["Regflag"]), Convert.ToString(Session["Admission"]));
            }
        }
        catch { }
    }
    private void settingValueRollAndReg(string rollvalue, string regvalue, string addmis)
    {
        // Tuple<byte, byte>
        string rollval = rollvalue;
        string regval = regvalue;
        string addVal = addmis;
        try
        {
            if (rollval != "" && regval != "")
            {
                if (rollval == "0" && regval == "0" && addVal == "0")
                    roll = 0;
                else if (rollval == "1" && regval == "1" && addVal == "1")
                    roll = 1;
                else if (rollval == "1" && regval == "0" && addVal == "0")
                    roll = 2;
                else if (rollval == "0" && regval == "1" && addVal == "0")
                    roll = 3;
                else if (rollval == "0" && regval == "0" && addVal == "1")
                    roll = 4;
                else if (rollval == "1" && regval == "1" && addVal == "0")
                    roll = 5;
                else if (rollval == "0" && regval == "1" && addVal == "1")
                    roll = 6;
                else if (rollval == "1" && regval == "0" && addVal == "1")
                    roll = 7;
            }
        }
        catch { }
        // return new Tuple<byte, byte>(roll,reg);

    }

    //protected void spreadColumnVisible()
    //{
    //    try
    //    {
    //        if (roll == 0)
    //        {
    //            FpSpreadstud.Columns[2].Visible = true;
    //            FpSpreadstud.Columns[3].Visible = true;
    //            FpSpreadstud.Columns[4].Visible = true;
    //        }
    //        else if (roll == 1)
    //        {
    //            FpSpreadstud.Columns[2].Visible = true;
    //            FpSpreadstud.Columns[3].Visible = true;
    //            FpSpreadstud.Columns[4].Visible = true;
    //        }
    //        else if (roll == 2)
    //        {
    //            FpSpreadstud.Columns[2].Visible = true;
    //            FpSpreadstud.Columns[3].Visible = false;
    //            FpSpreadstud.Columns[4].Visible = false;

    //        }
    //        else if (roll == 3)
    //        {
    //            FpSpreadstud.Columns[2].Visible = false;
    //            FpSpreadstud.Columns[3].Visible = true;
    //            FpSpreadstud.Columns[4].Visible = false;
    //        }
    //        else if (roll == 4)
    //        {
    //            FpSpreadstud.Columns[2].Visible = false;
    //            FpSpreadstud.Columns[3].Visible = false;
    //            FpSpreadstud.Columns[4].Visible = true;
    //        }
    //        else if (roll == 5)
    //        {
    //            FpSpreadstud.Columns[2].Visible = true;
    //            FpSpreadstud.Columns[3].Visible = true;
    //            FpSpreadstud.Columns[4].Visible = false;
    //        }
    //        else if (roll == 6)
    //        {
    //            FpSpreadstud.Columns[2].Visible = false;
    //            FpSpreadstud.Columns[3].Visible = true;
    //            FpSpreadstud.Columns[4].Visible = true;
    //        }
    //        else if (roll == 7)
    //        {
    //            FpSpreadstud.Columns[2].Visible = true;
    //            FpSpreadstud.Columns[3].Visible = false;
    //            FpSpreadstud.Columns[4].Visible = true;
    //        }
    //    }
    //    catch { }
    //}

    #endregion

    // last modified 28.11.2016 sudhagar
}