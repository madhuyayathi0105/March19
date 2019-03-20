using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;
using InsproDataAccess;
using System.Configuration;

public partial class DFCR_ReportSchool : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    ReuasableMethods reuse = new ReuasableMethods();
    DataSet ds = new DataSet();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static byte roll = 0;
    ArrayList colord = new ArrayList();
    bool usBasedRights = false;
    static int rightscode = 0;
    InsproDirectAccess inspro = new InsproDirectAccess();
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
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                setLabelText();
                loadcollege();
                if (ddlcollege.Items.Count > 0)
                    collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
                loadcollege();
                loadtype();
                bindBtch();
                binddeg();
                binddept();
                bindsem();
                loadpaid();
                //  loadfinanceyear();
                cbdate_Changed(sender, e);
                ddlacctype_change(sender, e);
                treeledger.Attributes.Add("onclick", "OnCheckBoxCheckChanged(event)");
                loadStudenttype();
                LoadIncludeSetting();
                loadFnlYrSchl();
            }
            divcolorder.Attributes.Add("Style", "display:none;");
            if (ddlcollege.Items.Count > 0)
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
        }
        catch (Exception ex) { }
    }

    #region college
    public void loadcollege()
    {
        ddlcollege.Items.Clear();
        reuse.bindCollegeToDropDown(usercode, ddlcollege);
    }
    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlcollege.Items.Count > 0)
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            loadtype();
            bindBtch();
            binddeg();
            binddept();
            bindsem();
            loadpaid();
            //loadfinanceyear();
            loadFnlYrSchl();
        }
        catch
        { }
    }
    #endregion

    #region type or stream
    public void loadtype()
    {
        try
        {
            ddltype.Items.Clear();
            string strquery = "select distinct type from course where college_code='" + collegecode + "' and type is not null and type<>''";
            ds = d2.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddltype.DataSource = ds;
                ddltype.DataTextField = "type";
                ddltype.DataBind();
                ddltype.Enabled = true;
                ddltype.Items.Insert(0, "Both");
            }
            else
                ddltype.Enabled = false;
        }
        catch
        { }
    }

    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindBtch();
        binddeg();
        binddept();
        bindsem();
        ddlacctype_change(sender, e);
    }
    #endregion

    #region batch
    public void bindBtch()
    {
        try
        {
            cbl_batch.Items.Clear();
            cb_batch.Checked = false;
            txt_batch.Text = "---Select---";
            ds.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_batch.DataSource = ds;
                cbl_batch.DataTextField = "batch_year";
                cbl_batch.DataValueField = "batch_year";
                cbl_batch.DataBind();
                if (cbl_batch.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_batch.Items.Count; i++)
                    {
                        cbl_batch.Items[i].Selected = true;
                    }
                    txt_batch.Text = lblbatch.Text + "(" + cbl_batch.Items.Count + ")";
                    cb_batch.Checked = true;
                }
            }
        }
        catch { }
    }
    protected void cb_batch_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_batch, cbl_batch, txt_batch, lblbatch.Text, "--Select--");
        binddeg();
        binddept();
    }
    protected void cbl_batch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_batch, cbl_batch, txt_batch, lblbatch.Text, "--Select--");
        binddeg();
        binddept();
    }
    #endregion

    #region degree


    public void binddeg()
    {
        try
        {
            cbl_degree.Items.Clear();
            cb_degree.Checked = false;
            txt_degree.Text = "---Select---";
            string stream = "";
            if (ddltype.Items.Count > 0)
            {
                if (ddltype.SelectedItem.ToString() != "Both" && ddltype.SelectedItem.ToString() != "")
                    stream = ddltype.SelectedItem.Text.ToString();
            }
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
                codevalues = "and user_code='" + usercode + "'";
            cbl_degree.Items.Clear();
            ds.Clear();
            string selqry = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where  course.course_id=degree.course_id and course.college_code = degree.college_code and  degree.college_code='" + collegecode + "' and deptprivilages.Degree_code=degree.Degree_code " + codevalues + "";
            if (stream != "")
                selqry = selqry + " and type  in('" + stream + "')";
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_id";
                cbl_degree.DataBind();
                if (cbl_degree.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_degree.Items.Count; i++)
                    {
                        cbl_degree.Items[i].Selected = true;
                    }
                    txt_degree.Text = lbldeg.Text + "(" + cbl_degree.Items.Count + ")";
                    cb_degree.Checked = true;
                }
            }

        }
        catch { }
    }
    protected void cb_degree_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_degree, cbl_degree, txt_degree, lbldeg.Text, "--Select--");
        binddept();

    }
    protected void cbl_degree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_degree, cbl_degree, txt_degree, lbldeg.Text, "--Select--");
        binddept();

    }
    #endregion

    #region dept
    public void binddept()
    {
        try
        {
            cbl_dept.Items.Clear();
            cb_dept.Checked = false;
            txt_dept.Text = "---Select---";
            string batch = "";
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    if (batch == "")
                        batch = Convert.ToString(cbl_batch.Items[i].Text);
                    else
                        batch += "','" + Convert.ToString(cbl_batch.Items[i].Text);
                }
            }
            string degree = "";
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    if (degree == "")
                        degree = Convert.ToString(cbl_degree.Items[i].Value);
                    else
                        degree += "," + Convert.ToString(cbl_degree.Items[i].Value);
                }
            }
            if (batch != "" && degree != "")
            {
                ds.Clear();
                ds = d2.BindBranchMultiple(singleuser, group_user, degree, collegecode, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_dept.DataSource = ds;
                    cbl_dept.DataTextField = "dept_name";
                    cbl_dept.DataValueField = "degree_code";
                    cbl_dept.DataBind();
                    if (cbl_dept.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_dept.Items.Count; i++)
                        {
                            cbl_dept.Items[i].Selected = true;
                        }
                        txt_dept.Text = lbldept.Text + "(" + cbl_dept.Items.Count + ")";
                        cb_dept.Checked = true;
                    }
                }
            }

        }
        catch { }
    }
    protected void cb_dept_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_dept, cbl_dept, txt_dept, lbldept.Text, "--Select--");
        bindsem();
    }
    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_dept, cbl_dept, txt_dept, "Department", "--Select--");
        bindsem();
    }
    #endregion

    #region sem
    protected void cb_sem_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_sem, cbl_sem, txt_sem, lblsem.Text, "--Select--");
    }
    protected void cbl_sem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_sem, cbl_sem, txt_sem, lblsem.Text, "--Select--");
    }

    protected void bindsem()
    {
        try
        {
            string featDegcode = Convert.ToString(getCblSelectedValue(cbl_dept));
            cbl_sem.Items.Clear();
            cb_sem.Checked = false;
            txt_sem.Text = "--Select--";
            ds.Clear();
            string linkName = string.Empty;
            string cbltext = string.Empty;
            d2.featDegreeCode = featDegcode;
            ds = d2.loadFeecategory(Convert.ToString(ddlcollege.SelectedItem.Value), usercode, ref linkName);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cbl_sem.DataSource = ds;
                cbl_sem.DataTextField = "TextVal";
                cbl_sem.DataValueField = "TextCode";
                cbl_sem.DataBind();

                if (cbl_sem.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_sem.Items.Count; i++)
                    {
                        cbl_sem.Items[i].Selected = true;
                        cbltext = Convert.ToString(cbl_sem.Items[i].Text);
                    }
                    if (cbl_sem.Items.Count == 1)
                        txt_sem.Text = "" + linkName + "(" + cbltext + ")";
                    else
                        txt_sem.Text = "" + linkName + "(" + cbl_sem.Items.Count + ")";
                    cb_sem.Checked = true;
                }
            }
        }
        catch { }
    }


    #endregion

    #region financial year
    public void loadFnlYrSchl()
    {
        try
        {
            string fnalyr = "";
            string getfinanceyear = "select distinct convert(nvarchar(15),FinYearStart,103) sdate,convert(nvarchar(15),FinYearEnd,103) edate,FinYearPK from FM_FinYearMaster where CollegeCode='" + ddlcollege.SelectedItem.Value + "'  order by FinYearPK desc";
            ds.Dispose();
            ds.Reset();
            ddlfinlyear.Items.Clear();
            ds = d2.select_method_wo_parameter(getfinanceyear, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string fdatye = ds.Tables[0].Rows[i]["sdate"].ToString() + '-' + ds.Tables[0].Rows[i]["edate"].ToString();
                    string actid = ds.Tables[0].Rows[i]["FinYearPK"].ToString();
                    ddlfinlyear.Items.Insert(0, new System.Web.UI.WebControls.ListItem(fdatye, actid));
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void loadfinanceyear()
    {
        try
        {
            string fnalyr = "";
            string getfinanceyear = "select distinct convert(nvarchar(15),FinYearStart,103) sdate,convert(nvarchar(15),FinYearEnd,103) edate,FinYearPK from FM_FinYearMaster where CollegeCode='" + collegecode + "'  order by FinYearPK desc";
            ds.Dispose();
            ds.Reset();
            chkfyear.Checked = false;
            chklsfyear.Items.Clear();
            ds = d2.select_method_wo_parameter(getfinanceyear, "text");
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
                    fnalyr = Convert.ToString(chklsfyear.Items[i].Text);
                }
                if (chklsfyear.Items.Count == 1)
                {
                    txtfyear.Text = "" + fnalyr + "";
                }
                else
                {
                    txtfyear.Text = "Finance Year(" + (chklsfyear.Items.Count) + ")";
                }
                // txtfyear.Text = "Finance Year (" + chklsfyear.Items.Count + ")";
                chkfyear.Checked = true;
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void chklsfyear_selected(object sender, EventArgs e)
    {
        CallCheckboxListChange(chkfyear, chklsfyear, txtfyear, "Finance Year", "--Select--");

    }
    protected void chkfyear_changed(object sender, EventArgs e)
    {
        CallCheckboxChange(chkfyear, chklsfyear, txtfyear, "Finance Year", "--Select--");
    }
    #endregion

    #region paymentmode
    public void loadpaid()
    {
        try
        {
            chkl_paid.Items.Clear();
            d2.BindPaymodeToCheckboxList(chkl_paid, usercode, collegecode);
            if (chkl_paid.Items.Count > 0)
            {
                for (int i = 0; i < chkl_paid.Items.Count; i++)
                {
                    chkl_paid.Items[i].Selected = true;
                }
                txt_paid.Text = "Paid(" + chkl_paid.Items.Count + ")";
                chk_paid.Checked = true;
            }
        }
        catch
        {

        }

    }
    public void chk_paid_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(chk_paid, chkl_paid, txt_paid, "Paid", "--Select--");

    }
    public void chkl_paid_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(chk_paid, chkl_paid, txt_paid, "Paid", "--Select--");

    }
    #endregion

    #region group, header,ledger
    protected void ddlacctype_change(object sender, EventArgs e)
    {
        try
        {
            treeledger.Visible = false;
            treeledger.Nodes.Clear();
            if (ddlacctype.SelectedItem.Text == "Group Header")
            {
                Label1.Text = "G/P Header";
                loadGroupheaders();
                txtheader.Enabled = true;
                ddlfeetype.Enabled = true;
                cbledgacr.Enabled = false;
            }
            else if (ddlacctype.SelectedItem.Text == "Header")
            {
                Label1.Text = "Header";
                loadheaderorLedger();
                txtheader.Enabled = true;
                ddlfeetype.Enabled = true;
                cbledgacr.Enabled = false;
            }
            else
            {
                treeledger.Visible = true;
                loadheaderorLedger();
                Label1.Text = "Header";
                txtheader.Enabled = true;
                ddlfeetype.Enabled = true;
                cbledgacr.Enabled = true;
            }
        }
        catch
        {

        }
    }
    public void loadGroupheaders()
    {
        try
        {
            cbheader.Checked = false;
            txtheader.Text = "---Select---";
            cblheader.Items.Clear();
            string strheadid = Convert.ToString(getCblSelectedValue(chklsfyear));
            if (!string.IsNullOrEmpty(strheadid))
            {
                string straccheadquery = "select distinct ChlGroupHeader  from FS_ChlGroupHeaderSettings ";
                if (ddltype.Items.Count > 0 && ddltype.SelectedItem.Text != "Both")
                    straccheadquery = straccheadquery + " where Stream='" + ddltype.SelectedItem.Text.ToString() + "'";
                ds = d2.select_method_wo_parameter(straccheadquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cblheader.DataSource = ds;
                    cblheader.DataTextField = "ChlGroupHeader";
                    cblheader.DataValueField = "ChlGroupHeader";
                    cblheader.DataBind();

                    for (int i = 0; i < cblheader.Items.Count; i++)
                    {
                        cblheader.Items[i].Selected = true;
                    }
                    cbheader.Checked = true;
                    txtheader.Text = "Group Header (" + cblheader.Items.Count + ")";
                }
            }
        }
        catch { }
    }
    public void loadheaderorLedger()
    {
        try
        {
            treeledger.Nodes.Clear();
            txtheader.Text = "---Select---";
            cbheader.Checked = false;
            cblheader.Items.Clear();
            string type = string.Empty;
            string hedgId = string.Empty;
            if (ddltype.Enabled == true && ddltype.Items.Count > 0)
            {
                if (ddltype.SelectedItem.ToString() != "Both" && ddltype.SelectedItem.ToString() != "")
                    type = "and c.Stream='" + ddltype.SelectedItem.ToString() + "'";
            }
            string straccheadquery = "SELECT HeaderPK as header_id,HeaderName as header_name,hd_priority FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = '" + usercode + "' AND H.CollegeCode = '" + collegecode + "' order by len(isnull(hd_priority,10000)),hd_priority asc ";
            ds = d2.select_method_wo_parameter(straccheadquery, "Text");
            if (ddlacctype.SelectedItem.Text == "Ledger")
            {
                txtheader.Text = " Header (" + ds.Tables[0].Rows.Count + ")";
                cbheader.Checked = true;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        TreeNode node = new TreeNode(ds.Tables[0].Rows[i]["header_name"].ToString(), ds.Tables[0].Rows[i]["header_id"].ToString());
                        string strled = "SELECT LedgerPK as fee_code,LedgerName as fee_type FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode   AND P. UserCode = '" + Session["usercode"].ToString() + "' AND L.CollegeCode = '" + collegecode + "'  and L.HeaderFK in('" + ds.Tables[0].Rows[i]["header_id"].ToString() + "')and LedgerMode='0'  order by isnull(l.priority,1000), l.ledgerName asc ";
                        DataSet ds1 = d2.select_method_wo_parameter(strled, "Text");
                        for (int ledge = 0; ledge < ds1.Tables[0].Rows.Count; ledge++)
                        {
                            TreeNode subchildnode = new TreeNode(ds1.Tables[0].Rows[ledge]["fee_type"].ToString(), ds1.Tables[0].Rows[ledge]["fee_code"].ToString());
                            subchildnode.ShowCheckBox = true;
                            node.ChildNodes.Add(subchildnode);
                        }
                        node.ShowCheckBox = true;
                        treeledger.Nodes.Add(node);
                        if (hedgId == "")
                            hedgId = Convert.ToString(ds.Tables[0].Rows[i]["header_id"]);
                        else
                            hedgId = hedgId + "','" + Convert.ToString(ds.Tables[0].Rows[i]["header_id"]);
                    }
                    if (cbheader.Checked == true)
                    {
                        for (int remv = 0; remv < treeledger.Nodes.Count; remv++)
                        {
                            treeledger.Nodes[remv].Checked = true;
                            txtheader.Text = "Header(" + (treeledger.Nodes.Count) + ")";
                            if (treeledger.Nodes[remv].ChildNodes.Count > 0)
                            {
                                for (int child = 0; child < treeledger.Nodes[remv].ChildNodes.Count; child++)
                                    treeledger.Nodes[remv].ChildNodes[child].Checked = true;
                            }
                        }
                    }
                    //ViewState["hedgId"] = hedgId;
                }
            }
            else
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cblheader.DataSource = ds;
                    cblheader.DataTextField = "header_name";
                    cblheader.DataValueField = "header_id";
                    cblheader.DataBind();
                    for (int i = 0; i < cblheader.Items.Count; i++)
                    {
                        cblheader.Items[i].Selected = true;
                        if (hedgId == "")
                            hedgId = cblheader.Items[i].Value;
                        else
                            hedgId = hedgId + "," + cblheader.Items[i].Value;
                    }
                    txtheader.Text = " Header (" + cblheader.Items.Count + ")";
                    cbheader.Checked = true;
                    ledgerload();
                }
            }
        }
        catch
        { }
    }
    protected void cblheader_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string clg = "";
            int commcount = 0;
            cbheader.Checked = false;
            for (int i = 0; i < cblheader.Items.Count; i++)
            {
                if (cblheader.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    if (clg == "")
                        clg = cblheader.Items[i].Value.ToString();
                    else
                        clg = clg + "," + cblheader.Items[i].Value;
                }
            }
            string set = "Header";
            if (ddlacctype.SelectedItem.Text == "Group Header")
                set = "Group Header";
            if (commcount == cblheader.Items.Count)
            {
                txtheader.Text = "" + set + "(" + commcount.ToString() + ")";
                cbheader.Checked = true;
            }
            else if (commcount == 0)
                txtheader.Text = "--Select--";
            else
                txtheader.Text = "" + set + "(" + commcount.ToString() + ")";
            ledgerload();
        }
        catch
        { }
    }
    protected void cbheader_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlacctype.SelectedItem.Text == "Ledger")
            {
                if (cbheader.Checked == true)
                {
                    for (int remv = 0; remv < treeledger.Nodes.Count; remv++)
                    {
                        treeledger.Nodes[remv].Checked = true;
                        txtheader.Text = "Header(" + (treeledger.Nodes.Count) + ")";
                        if (treeledger.Nodes[remv].ChildNodes.Count > 0)
                        {
                            for (int child = 0; child < treeledger.Nodes[remv].ChildNodes.Count; child++)
                            {
                                treeledger.Nodes[remv].ChildNodes[child].Checked = true;
                            }
                        }
                    }
                }
                else
                {
                    for (int remv = 0; remv < treeledger.Nodes.Count; remv++)
                    {
                        treeledger.Nodes[remv].Checked = false;
                        txtheader.Text = "---Select---";
                        if (treeledger.Nodes[remv].ChildNodes.Count > 0)
                        {
                            for (int child = 0; child < treeledger.Nodes[remv].ChildNodes.Count; child++)
                            {
                                treeledger.Nodes[remv].ChildNodes[child].Checked = false;
                            }
                        }
                    }
                }
            }
            else
            {
                if (cbheader.Checked == true)
                {
                    for (int i = 0; i < cblheader.Items.Count; i++)
                    {
                        cblheader.Items[i].Selected = true;
                    }
                    if (ddlacctype.SelectedItem.Text == "Group Header")
                        txtheader.Text = "Group Header (" + cblheader.Items.Count + ")";
                    else
                        txtheader.Text = "Header (" + cblheader.Items.Count + ")";
                }
                else
                {
                    for (int i = 0; i < cblheader.Items.Count; i++)
                    {
                        cblheader.Items[i].Selected = false;
                    }
                    txtheader.Text = "---Select---";
                }
                ledgerload();
            }
        }
        catch
        {
        }
    }

    public void ledgerload()
    {
        try
        {
            string clgvalue = Convert.ToString(ddlcollege.SelectedItem.Value);
            chkl_studled.Items.Clear();
            txt_studled.Text = "--Select--";
            chk_studled.Checked = false; 
            string hed = Convert.ToString(getCblSelectedValue(cblheader));
            string query1 = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='0' and L.CollegeCode = " + clgvalue + "  and L.HeaderFK in('" + hed + "')  order by isnull(l.priority,1000), l.ledgerName asc ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query1, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chkl_studled.DataSource = ds;
                chkl_studled.DataTextField = "LedgerName";
                chkl_studled.DataValueField = "LedgerPK";
                chkl_studled.DataBind();
                for (int i = 0; i < chkl_studled.Items.Count; i++)
                {
                    chkl_studled.Items[i].Selected = true;
                }
                txt_studled.Text = "Ledger(" + chkl_studled.Items.Count + ")";
                chk_studled.Checked = true; 
            }            
        }
        catch
        {
        }
    }
    public void chk_studled_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(chk_studled, chkl_studled, txt_studled, "Ledger", "--Select--");
    }
    public void chkl_studled_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(chk_studled, chkl_studled, txt_studled, "Ledger", "--Select--");
    }
    #endregion

    //added by sudhagar 01.06.2017
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
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("Cancel", "4"));
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
        CallCheckboxChange(cbinclude, cblinclude, txtinclude, "Include Setting", "--Select--");
    }
    protected void cblinclude_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbinclude, cblinclude, txtinclude, "Include Setting", "--Select--");

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
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblbank.DataSource = ds;
                cblbank.DataTextField = "TextVal";
                cblbank.DataValueField = "TextCode";
                cblbank.DataBind();
                if (cblbank.Items.Count > 0)
                {
                    for (int i = 0; i < cblbank.Items.Count; i++)
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
        CallCheckboxChange(cbbank, cblbank, txtbankname, "Include Bank", "--Select--");
    }
    protected void cblbank_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbbank, cblbank, txtbankname, "Include Bank", "--Select--");
    }


    #endregion

    protected void ddlfeetype_change(object sender, EventArgs e)
    {
        if (ddlfeetype.SelectedItem.Text == "Paid")
        {
            txt_paid.Enabled = true;
            cbRcpt.Checked = false;
            cbRcpt.Enabled = true;
        }
        else
        {
            txt_paid.Enabled = false;
            cbRcpt.Checked = false;
            cbRcpt.Enabled = false;
        }
    }

    protected void ddlstudtype_change(object sender, EventArgs e)
    {
        if (ddlstudtype.SelectedItem.Text.Trim() == "Before Admission")
            cbbfadm.Enabled = false;
        else
            cbbfadm.Enabled = true;
    }

    protected void cbdate_Changed(object sender, EventArgs e)
    {
        if (cbdate.Checked)
        {
            txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_fromdate.Attributes.Add("readonly", "readonly");
            txt_todate.Attributes.Add("readonly", "readonly");
            txt_fromdate.Enabled = true;
            txt_todate.Enabled = true;
            cbdegwisetotal.Enabled = true;
            cbdgreename.Enabled = false;
        }
        else
        {
            txt_fromdate.Enabled = false;
            txt_todate.Enabled = false;
            cbdegwisetotal.Enabled = false;
            cbdgreename.Enabled = true;
        }
    }

    protected string ledgervalue()
    {
        string header = string.Empty;
        string ledger = string.Empty;
        for (int remv = 0; remv < treeledger.Nodes.Count; remv++)
        {
            if (header == "")
                header = treeledger.Nodes[remv].Value.ToString();
            else
                header = header + "','" + treeledger.Nodes[remv].Value.ToString();
            if (treeledger.Nodes[remv].ChildNodes.Count > 0)
            {
                for (int child = 0; child < treeledger.Nodes[remv].ChildNodes.Count; child++)
                {
                    if (treeledger.Nodes[remv].ChildNodes[child].Checked)
                    {
                        if (ledger == "")
                            ledger = treeledger.Nodes[remv].ChildNodes[child].Value.ToString();
                        else
                            ledger = ledger + "','" + treeledger.Nodes[remv].ChildNodes[child].Value.ToString();
                    }
                }
            }
        }
        return ledger;
    }

    protected DataSet getDetails(ref DateTime fromdt, ref DateTime todt)
    {
        DataSet dsval = new DataSet();
        try
        {
            DataSet lgacr = new DataSet();
            string studType = string.Empty;
            string stream = string.Empty;
            string fromdate = string.Empty;
            string todate = string.Empty;
            string bankcode = string.Empty;
            string delflg = string.Empty;
            if (ddltype.Enabled == true)
            {
                if (ddltype.SelectedItem.Text != "All" && ddltype.SelectedItem.Text != "")
                    stream = Convert.ToString(ddltype.SelectedItem.Text);
            }
            string batch = Convert.ToString(getCblSelectedValue(cbl_batch));
            string degree = Convert.ToString(getCblSelectedValue(cbl_dept));
            if (ddlstudtype.SelectedItem.Text != "All" && ddlstudtype.SelectedItem.Text != "")
                studType = Convert.ToString(ddlstudtype.SelectedItem.Value);
            string feesType = string.Empty;
            feesType = Convert.ToString(ddlfeetype.SelectedItem.Text);
            string feecat = Convert.ToString(getCblSelectedValue(cbl_sem));
            string payMode = Convert.ToString(getCblSelectedValue(chkl_paid));
            string fnlYear = Convert.ToString(getCblSelectedValue(chklsfyear));
            string headerValue = string.Empty;
            string ledgerValue = string.Empty;
            if (ddlacctype.SelectedIndex == 0 || ddlacctype.SelectedIndex == 1)
                headerValue = Convert.ToString(getCblSelectedValue(cblheader));
            else
                ledgerValue = ledgervalue();

            string studMode = Convert.ToString(getCblSelectedValue(cbl_type));
            #region include
            if (cbdate.Checked)
            {
                fromdate = txt_fromdate.Text;
                todate = txt_todate.Text;
                string[] frdate = fromdate.Split('/');
                if (frdate.Length == 3)
                    fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
                string[] tdate = todate.Split('/');
                if (tdate.Length == 3)
                    todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
                fromdt = Convert.ToDateTime(fromdate);
                todt = Convert.ToDateTime(todate);
            }
            if (cbbankcheck.Checked)
            {
                for (int i = 0; i < chkl_paid.Items.Count; i++)
                {
                    if (chkl_paid.Items[i].Selected == true && chkl_paid.Items[i].Text == "Online Pay")
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
            }
            // if (checkdicon.Checked)
            //  {
            #region student category
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
                            cc = " r.cc=1";
                        if (cblinclude.Items[i].Value == "2")
                            debar = " r.Exam_Flag like 'debar'";

                        if (cblinclude.Items[i].Value == "3")
                            disc = " r.DelFlag=1";
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
            #endregion
            // }
            #endregion
            if (cbledgacr.Checked == true)
            {
                string strled = "SELECT LedgerPK ,LedgerAcr FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode   AND P. UserCode = '" + usercode + "' AND L.CollegeCode = '" + collegecode + "'  and LedgerMode='0'  order by isnull(l.priority,1000), l.ledgerName asc ";
                lgacr = d2.select_method_wo_parameter(strled, "Text");
            }
            bool boolpaymode = false;
            if (ddlfeetype.SelectedItem.Text.Trim() == "Paid")
            {
                if (!string.IsNullOrEmpty(payMode))
                    boolpaymode = true;
            }
            else
                boolpaymode = true;
            if (!string.IsNullOrEmpty(batch) && !string.IsNullOrEmpty(degree) && !string.IsNullOrEmpty(batch) && !string.IsNullOrEmpty(feesType) && !string.IsNullOrEmpty(feecat) && !string.IsNullOrEmpty(fnlYear) && !string.IsNullOrEmpty(headerValue) || !string.IsNullOrEmpty(ledgerValue) && boolpaymode)
            {
                #region query
                string Query = string.Empty;
                string strorder = " order by r.Batch_Year,r.degree_code,r.Current_Semester,r.Stud_Name";
                string hdStr = string.Empty;
                if (ddlacctype.SelectedItem.Text.Trim() == "Header")
                    hdStr = ",headerfk";
                else if (ddlacctype.SelectedItem.Text.Trim() == "Ledger")
                    hdStr = ",ledgerfk";
                if (!cbdate.Checked)
                {
                    if (ddlacctype.SelectedItem.Text.Trim() == "Group Header")
                    {
                        #region gp header
                        if (ddlfeetype.SelectedItem.Text.Trim() == "Paid")
                        {
                            if (ddlstudtype.SelectedItem.Text.Trim() != "Before Admission")
                            {
                                #region general query
                                Query = " select distinct f.App_No ,r.degree_code,r.Batch_Year,r.current_semester  from FT_FeeAllot f,Registration r ,Degree d,Department dt,Course c ,FS_ChlGroupHeaderSettings G where G.HeaderFK =f.HeaderFK and f.App_No =r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code ='" + collegecode + "'";
                                if (delflg != "")
                                    Query = Query + delflg;
                                if (stream.Trim() != "Both" && stream.Trim() != "")
                                    Query = Query + " and type ='" + stream + "'";
                                if (batch.Trim() != "")
                                    Query = Query + " and r.batch_year  in ('" + batch + "')";
                                if (degree.Trim() != "")
                                    Query = Query + " and r.degree_code  in ('" + degree + "')";
                                if (feecat.Trim() != "")
                                    Query = Query + " and f.FeeCategory  in ('" + feecat + "')";
                                if (headerValue.Trim() != "")
                                    Query = Query + " and ChlGroupHeader   in ('" + headerValue + "')";
                                if (fnlYear.Trim() != "")
                                    Query = Query + " and f.FinYearFK   in ('" + fnlYear + "')";
                                if (chkfeeroll.Checked == true)
                                    Query = Query + " and r.Roll_No not in(select s.roll_no from stucon s where s.roll_no=r.roll_no and s.semester=r.Current_Semester and s.ack_fee_of_roll=1)";
                                if (!string.IsNullOrEmpty(studMode))
                                    Query += " and r.mode in('" + studMode + "')";
                                if (cbbfadm.Checked == true)
                                {
                                    #region before admission
                                    Query = Query + " union select distinct f.App_No ,r.degree_code,r.Batch_Year,r.current_semester  from FT_FeeAllot f,applyn r ,Degree d,Department dt,Course c ,FS_ChlGroupHeaderSettings G where G.HeaderFK =f.HeaderFK and f.App_No =r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code ='" + collegecode + "'  and r.isconfirm = 1 and r.admission_status = '1' and isnull(r.is_enroll,'0')<>'2'";
                                    if (stream.Trim() != "Both" && stream.Trim() != "")
                                        Query = Query + " and g.Stream ='" + stream + "'";
                                    if (batch.Trim() != "")
                                        Query = Query + " and r.batch_year  in ('" + batch + "')";
                                    if (degree.Trim() != "")
                                        Query = Query + " and r.degree_code  in ('" + degree + "')";
                                    if (feecat.Trim() != "")
                                        Query = Query + " and f.FeeCategory  in ('" + feecat + "')";
                                    if (headerValue.Trim() != "")
                                        Query = Query + " and ChlGroupHeader   in ('" + headerValue + "')";
                                    if (fnlYear.Trim() != "")
                                        Query = Query + " and f.FinYearFK   in ('" + fnlYear + "')";
                                    if (!string.IsNullOrEmpty(studMode))
                                        Query += " and r.mode in('" + studMode + "')";
                                    #endregion
                                }

                                Query += "   select distinct f.App_No,r.Roll_No,r.roll_admit,r.Reg_No,r.Stud_Name,r.degree_code,f.FeeCategory,r.current_semester from FT_FinDailyTransaction f,Registration r,FS_ChlGroupHeaderSettings G  where f.App_No =r.App_No and g.HeaderFK =f.HeaderFK  and r.college_code ='" + collegecode + "'";
                                if (usBasedRights == true)
                                    Query += " and f.EntryUserCode in('" + usercode + "')";
                                if (delflg != "")
                                    Query = Query + delflg;
                                if (stream.Trim() != "Both" && stream.Trim() != "")
                                    Query = Query + " and g.Stream ='" + stream + "'";
                                if (batch.Trim() != "")
                                    Query = Query + " and r.batch_year  in ('" + batch + "')";
                                if (degree.Trim() != "")
                                    Query = Query + " and r.degree_code  in ('" + degree + "')";
                                if (feecat.Trim() != "")
                                    Query = Query + " and f.FeeCategory  in ('" + feecat + "')";
                                if (fnlYear.Trim() != "")
                                    Query = Query + " and f.FinYearFK   in ('" + fnlYear + "')";
                                if (headerValue.Trim() != "")
                                    Query = Query + " and ChlGroupHeader   in ('" + headerValue + "')";
                                if (bankcode != "")
                                    Query += " and f.DDBankCode in ('" + bankcode + "')";
                                if (payMode.Trim() != "")
                                    Query += " and f.paymode in ('" + payMode + "')";
                                if (!string.IsNullOrEmpty(studMode))
                                    Query += " and r.mode in('" + studMode + "')";
                                if (cbbfadm.Checked == true)
                                {
                                    #region before admission
                                    Query += " union  select distinct d.App_No,r.app_formno as Roll_No,r.app_formno as Reg_No,r.app_formno as roll_admit,r.Stud_Name,r.degree_code,d.FeeCategory,r.current_semester from applyn r,ft_findailytransaction d,fs_chlgroupheadersettings f where  f.headerfk=d.Headerfk and isconfirm = 1 and admission_status = '1' and d.app_no = r.app_no and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'and r.college_code ='" + collegecode + "' and isnull(r.is_enroll,'0')<>'2' and r.college_code ='" + collegecode + "'";
                                    // and r.app_no not in (select app_no from Registration  where degree_code in(" + degree + ")   and Batch_year in(" + batch + ") )
                                    if (usBasedRights == true)
                                        Query += " and d.EntryUserCode in('" + usercode + "')";

                                    if (stream.Trim() != "Both" && stream.Trim() != "")
                                        Query = Query + " and f.Stream ='" + stream + "'";
                                    if (batch.Trim() != "")
                                        Query = Query + " and r.batch_year  in ('" + batch + "')";
                                    if (degree.Trim() != "")
                                        Query = Query + " and r.degree_code  in ('" + degree + "')";
                                    if (feecat.Trim() != "")
                                        Query = Query + " and d.FeeCategory  in ('" + feecat + "')";
                                    if (fnlYear.Trim() != "")
                                        Query = Query + " and d.FinYearFK   in ('" + fnlYear + "')";
                                    if (headerValue.Trim() != "")
                                        Query = Query + " and ChlGroupHeader   in ('" + headerValue + "')";
                                    if (bankcode != "")
                                        Query += " and d.DDBankCode in ('" + bankcode + "')";
                                    if (payMode.Trim() != "")
                                        Query += " and d.paymode in ('" + payMode + "')";
                                    if (!string.IsNullOrEmpty(studMode))
                                        Query += " and r.mode in('" + studMode + "')";
                                    //Query += " order by r.app_formno";
                                    #endregion
                                }
                                // Query += " order by r.Roll_No";

                                Query += " Select SUM(Debit) as Paid, f.App_No,f.FeeCategory,r.degree_code,r.Batch_Year,r.Stud_Name,r.Roll_No,r.roll_admit,r.Reg_No,g.ChlGroupHeader,f.TransCode,DDNo,f.PayMode,r.Current_Semester from FT_FinDailyTransaction f,Registration r ,FS_ChlGroupHeaderSettings G where f.App_No =r.App_No  and G.HeaderFK =F.HeaderFK   and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'   and r.college_code ='" + collegecode + "' ";
                                if (usBasedRights == true)
                                    Query += " and f.EntryUserCode in('" + usercode + "')";
                                if (delflg != "")
                                    Query = Query + delflg;
                                if (!string.IsNullOrEmpty(stream))
                                    Query = Query + " and Stream ='" + stream + "'";
                                if (batch.Trim() != "")
                                    Query = Query + " and r.batch_year  in ('" + batch + "')";
                                if (degree.Trim() != "")
                                    Query = Query + " and r.degree_code  in ('" + degree + "')";
                                if (feecat.Trim() != "")
                                    Query = Query + " and f.FeeCategory  in ('" + feecat + "')";
                                if (fnlYear.Trim() != "")
                                    Query = Query + " and f.FinYearFK   in ('" + fnlYear + "')";
                                if (headerValue.Trim() != "")
                                    Query = Query + " and ChlGroupHeader   in ('" + headerValue + "')";
                                if (bankcode != "")
                                    Query += " and f.DDBankCode in ('" + bankcode + "')";
                                if (payMode.Trim() != "")
                                    Query += " and f.paymode in ('" + payMode + "')";
                                if (!string.IsNullOrEmpty(studMode))
                                    Query += " and r.mode in('" + studMode + "')";
                                Query = Query + " group by f.App_No,f.FeeCategory,r.degree_code,r.Batch_Year,r.Stud_Name,r.Roll_No,r.roll_admit,r.Reg_No,g.ChlGroupHeader,f.TransCode ,f.PayMode,DDNo,r.Current_Semester ";
                                if (cbbfadm.Checked == true)
                                {
                                    #region befor admission

                                    Query += " union  select distinct SUM(Debit) as Paid, d.App_No,d.FeeCategory,r.degree_code,r.Batch_Year,r.Stud_Name,r.app_formno as Roll_No,r.app_formno as  Reg_No,r.app_formno as roll_admit,f.ChlGroupHeader,d.TransCode,DDNo,d.PayMode,r.Current_Semester from ft_findailytransaction d,applyn r,fs_chlgroupheadersettings f where d.app_no = r.app_no  and f.headerfk=d.headerfk  and credit=0 and transtype=1 and d.debit>0  and r.isconfirm = 1 and r.admission_status = '1' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(r.is_enroll,'0')<>'2'  and r.college_code ='" + collegecode + "' ";
                                    // and r.app_no not in (select app_no from Registration  where degree_code in(" + degree + ") and f.stream ='" + typevalue + "'  and Batch_year in(" + batch + "))
                                    if (usBasedRights == true)
                                        Query += " and d.EntryUserCode in('" + usercode + "')";

                                    if (stream.Trim() != "Both" && stream.Trim() != "")
                                        Query = Query + " and f.Stream ='" + stream + "'";
                                    if (batch.Trim() != "")
                                        Query = Query + " and r.batch_year  in ('" + batch + "')";
                                    if (degree.Trim() != "")
                                        Query = Query + " and r.degree_code  in ('" + degree + "')";
                                    if (feecat.Trim() != "")
                                        Query = Query + " and d.FeeCategory  in ('" + feecat + "')";
                                    if (fnlYear.Trim() != "")
                                        Query = Query + " and d.FinYearFK   in ('" + fnlYear + "')";
                                    if (headerValue.Trim() != "")
                                        Query = Query + " and f.ChlGroupHeader   in ('" + headerValue + "')";
                                    if (bankcode != "")
                                        Query += " and d.DDBankCode in ('" + bankcode + "')";
                                    if (payMode.Trim() != "")
                                        Query += " and d.paymode in ('" + payMode + "')";
                                    if (!string.IsNullOrEmpty(studMode))
                                        Query += " and r.mode in('" + studMode + "')";
                                    Query += "  group by d.App_No,d.FeeCategory,r.degree_code,r.Batch_Year,r.Stud_Name,r.app_formno,f.ChlGroupHeader,d.TransCode,DDNo,d.PayMode,r.Current_Semester";
                                    #endregion
                                }
                                Query += " order by Roll_No";
                                #endregion
                            }
                            else
                            {
                                #region arrear list
                                //paid query

                                //Student list
                                Query = " select distinct f.App_No ,r.degree_code,r.Batch_Year,r.current_semester  from FT_FeeAllot f,applyn r ,Degree d,Department dt,Course c ,FS_ChlGroupHeaderSettings G where G.HeaderFK =f.HeaderFK and f.App_No =r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code ='" + collegecode + "'  and r.isconfirm = 1 and r.admission_status = '1'  and r.app_no not in (select app_no from Registration  where degree_code in(" + degree + ") and g.stream ='DAY'  and Batch_year in(" + batch + "))";
                                if (stream.Trim() != "Both" && stream.Trim() != "")
                                    Query = Query + " and g.Stream ='" + stream + "'";
                                if (batch.Trim() != "")
                                    Query = Query + " and r.batch_year  in ('" + batch + "')";
                                if (degree.Trim() != "")
                                    Query = Query + " and r.degree_code  in ('" + degree + "')";
                                if (feecat.Trim() != "")
                                    Query = Query + " and f.FeeCategory  in ('" + feecat + "')";
                                if (headerValue.Trim() != "")
                                    Query = Query + " and ChlGroupHeader   in ('" + headerValue + "')";
                                if (fnlYear.Trim() != "")
                                    Query = Query + " and f.FinYearFK   in ('" + fnlYear + "')";
                                if (!string.IsNullOrEmpty(studMode))
                                    Query += " and r.mode in('" + studMode + "')";

                                Query += "   select distinct d.App_No,r.app_formno as Roll_No,r.app_formno as Reg_No,r.app_formno as roll_admit,r.Stud_Name,r.degree_code,d.FeeCategory,r.current_semester from applyn r,ft_findailytransaction d,fs_chlgroupheadersettings f where  f.headerfk=d.Headerfk and isconfirm = 1 and admission_status = '1' and d.app_no = r.app_no and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'and r.college_code ='13' and r.app_no not in (select app_no from Registration  where degree_code in(" + degree + ")   and Batch_year in(" + batch + ") ) and r.college_code ='" + collegecode + "'";
                                if (usBasedRights == true)
                                    Query += " and d.EntryUserCode in('" + usercode + "')";
                                if (stream.Trim() != "Both" && stream.Trim() != "")
                                    Query = Query + " and f.Stream ='" + stream + "'";
                                if (batch.Trim() != "")
                                    Query = Query + " and r.batch_year  in ('" + batch + "')";
                                if (degree.Trim() != "")
                                    Query = Query + " and r.degree_code  in ('" + degree + "')";
                                if (feecat.Trim() != "")
                                    Query = Query + " and d.FeeCategory  in ('" + feecat + "')";
                                if (fnlYear.Trim() != "")
                                    Query = Query + " and d.FinYearFK   in ('" + fnlYear + "')";
                                if (headerValue.Trim() != "")
                                    Query = Query + " and ChlGroupHeader   in ('" + headerValue + "')";
                                if (bankcode != "")
                                    Query += " and d.DDBankCode in ('" + bankcode + "')";
                                if (payMode.Trim() != "")
                                    Query += " and d.paymode in ('" + payMode + "')";
                                if (!string.IsNullOrEmpty(studMode))
                                    Query += " and r.mode in('" + studMode + "')";
                                Query += " order by r.app_formno";

                                Query += "   select distinct SUM(Debit) as Paid, d.App_No,d.FeeCategory,r.degree_code,r.Batch_Year,r.Stud_Name,r.app_formno as Roll_No,r.app_formno as  Reg_No,r.app_formno as  roll_admit,f.ChlGroupHeader,d.TransCode,DDNo,d.PayMode,r.Current_Semester from ft_findailytransaction d,applyn r,fs_chlgroupheadersettings f where d.app_no = r.app_no  and f.headerfk=d.headerfk  and credit=0 and transtype=1 and d.debit>0  and r.isconfirm = 1 and r.admission_status = '1' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'  and r.app_no not in (select app_no from Registration  where degree_code in(" + degree + ") and f.stream ='" + stream + "'  and Batch_year in(" + batch + ")) and r.college_code ='" + collegecode + "' ";
                                if (usBasedRights == true)
                                    Query += " and d.EntryUserCode in('" + usercode + "')";
                                if (stream.Trim() != "Both" && stream.Trim() != "")
                                    Query = Query + " and f.Stream ='" + stream + "'";
                                if (batch.Trim() != "")
                                    Query = Query + " and r.batch_year  in ('" + batch + "')";
                                if (degree.Trim() != "")
                                    Query = Query + " and r.degree_code  in ('" + degree + "')";
                                if (feecat.Trim() != "")
                                    Query = Query + " and d.FeeCategory  in ('" + feecat + "')";
                                if (fnlYear.Trim() != "")
                                    Query = Query + " and d.FinYearFK   in ('" + fnlYear + "')";
                                if (headerValue.Trim() != "")
                                    Query = Query + " and f.ChlGroupHeader   in ('" + headerValue + "')";
                                if (bankcode != "")
                                    Query += " and d.DDBankCode in ('" + bankcode + "')";
                                if (payMode.Trim() != "")
                                    Query += " and d.paymode in ('" + payMode + "')";
                                if (!string.IsNullOrEmpty(studMode))
                                    Query += " and r.mode in('" + studMode + "')";
                                Query += "  group by d.App_No,d.FeeCategory,r.degree_code,r.Batch_Year,r.Stud_Name,r.app_formno,f.ChlGroupHeader,d.TransCode,DDNo,d.PayMode,r.Current_Semester";
                                Query += " order by R.app_formno";

                                #endregion
                            }
                        }
                        else if (ddlfeetype.SelectedItem.Text == "Yet To Be Paid")
                        {
                            #region Query
                            Query = " select distinct f.App_No ,r.degree_code,r.Batch_Year  from FT_FeeAllot f,Registration r ,Degree d,Department dt,Course c ,FS_ChlGroupHeaderSettings G where G.HeaderFK =f.HeaderFK and f.App_No =r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and ISNULL ( BalAmount ,TotalAmount)<>0  and d.college_code ='" + collegecode + "' ";
                            if (delflg != "")
                                Query = Query + delflg;
                            if (stream.Trim() != "Both" && stream.Trim() != "")
                                Query = Query + " and type ='" + stream + "'";
                            if (batch.Trim() != "")
                                Query = Query + " and r.batch_year  in ('" + batch + "')";
                            if (degree.Trim() != "")
                                Query = Query + " and r.degree_code  in ('" + degree + "')";
                            if (feecat.Trim() != "")
                                Query = Query + " and f.FeeCategory  in ('" + feecat + "')";
                            if (headerValue.Trim() != "")
                                Query = Query + " and ChlGroupHeader   in ('" + headerValue + "')";
                            if (fnlYear.Trim() != "")
                                Query = Query + " and f.FinYearFK   in ('" + fnlYear + "')";
                            if (!string.IsNullOrEmpty(studMode))
                                Query += " and r.mode in('" + studMode + "')";
                            if (chkfeeroll.Checked == true)
                                Query = Query + " and r.Roll_No not in(select s.roll_no from stucon s where s.roll_no=r.roll_no and s.semester=r.Current_Semester and s.ack_fee_of_roll=1)";

                            Query += "   select distinct f.App_No,r.Roll_No,r.roll_admit,r.Reg_No,r.Stud_Name,FeeCategory,r.degree_code from FT_FeeAllot f,Registration r,FS_ChlGroupHeaderSettings G  where f.App_No =r.App_No and g.HeaderFK =f.HeaderFK and ISNULL ( BalAmount ,TotalAmount)<>0  and r.college_code ='" + collegecode + "'";
                            if (delflg != "")
                                Query = Query + delflg;
                            if (stream.Trim() != "Both" && stream.Trim() != "")
                                Query = Query + " and g.Stream ='" + stream + "'";
                            if (batch.Trim() != "")
                                Query = Query + " and r.batch_year  in ('" + batch + "')";
                            if (degree.Trim() != "")
                                Query = Query + " and r.degree_code  in ('" + degree + "')";
                            if (feecat.Trim() != "")
                                Query = Query + " and f.FeeCategory  in ('" + feecat + "')";
                            if (fnlYear.Trim() != "")
                                Query = Query + " and f.FinYearFK   in ('" + fnlYear + "')";
                            if (!string.IsNullOrEmpty(studMode))
                                Query += " and r.mode in('" + studMode + "')";
                            if (headerValue.Trim() != "")
                                Query = Query + " and ChlGroupHeader   in ('" + headerValue + "')";

                            Query += " select  f.App_No,sum (TotalAmount),SUM( PaidAmount),sum(BalAmount)as Balance,FeeCategory,ChlGroupHeader from FT_FeeAllot f,Registration r ,Degree d,Department dt,Course c,FS_ChlGroupHeaderSettings G where f.App_No =r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and G.HeaderFK =F.HeaderFK and Stream =c.type and ISNULL (BalAmount ,TotalAmount)<>0  and d.college_code ='" + collegecode + "' ";
                            if (delflg != "")
                                Query = Query + delflg;
                            if (stream.Trim() != "Both" && stream.Trim() != "")
                                Query = Query + " and type ='" + stream + "'";
                            if (batch.Trim() != "")
                                Query = Query + " and r.batch_year  in ('" + batch + "')";
                            if (degree.Trim() != "")
                                Query = Query + " and r.degree_code  in ('" + degree + "')";
                            if (feecat.Trim() != "")
                                Query = Query + " and f.FeeCategory  in ('" + feecat + "')";
                            if (fnlYear.Trim() != "")
                                Query = Query + " and f.FinYearFK   in ('" + fnlYear + "')";
                            if (!string.IsNullOrEmpty(studMode))
                                Query += " and r.mode in('" + studMode + "')";
                            if (headerValue.Trim() != "")
                                Query = Query + " and ChlGroupHeader   in ('" + headerValue + "')";
                            Query = Query + " group by ChlGroupHeader,f.App_No,FeeCategory Order by f.app_no";
                            #endregion
                        }
                        else if (ddlfeetype.SelectedItem.Text == "Both")
                        {
                            #region query
                            Query = " select distinct f.App_No ,r.degree_code,r.Batch_Year  from FT_FeeAllot f,Registration r ,Degree d,Department dt,Course c ,FS_ChlGroupHeaderSettings G where G.HeaderFK =f.HeaderFK and f.App_No =r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code ='" + collegecode + "'";
                            if (delflg != "")
                                Query = Query + delflg;
                            if (stream.Trim() != "Both" && stream.Trim() != "")
                                Query = Query + " and type ='" + stream + "'";
                            if (batch.Trim() != "")
                                Query = Query + " and r.batch_year  in ('" + batch + "')";
                            if (degree.Trim() != "")
                                Query = Query + " and r.degree_code  in ('" + degree + "')";
                            if (feecat.Trim() != "")
                                Query = Query + " and f.FeeCategory  in ('" + feecat + "')";
                            if (headerValue.Trim() != "")
                                Query = Query + " and ChlGroupHeader   in ('" + headerValue + "')";
                            if (fnlYear.Trim() != "")
                                Query = Query + " and f.FinYearFK   in ('" + fnlYear + "')";
                            if (!string.IsNullOrEmpty(studMode))
                                Query += " and r.mode in('" + studMode + "')";
                            if (chkfeeroll.Checked == true)
                                Query = Query + " and r.Roll_No not in(select s.roll_no from stucon s where s.roll_no=r.roll_no and s.semester=r.Current_Semester and s.ack_fee_of_roll=1)";

                            Query += "   select distinct f.App_No,r.Roll_No,r.roll_admit,r.Reg_No,r.Stud_Name,FeeCategory,r.degree_code from FT_FeeAllot f,Registration r,FS_ChlGroupHeaderSettings G  where f.App_No =r.App_No and g.HeaderFK =f.HeaderFK and r.college_code ='" + collegecode + "'";
                            if (delflg != "")
                                Query = Query + delflg;
                            if (stream.Trim() != "Both" && stream.Trim() != "")
                                Query = Query + " and g.Stream ='" + stream + "'";
                            if (batch.Trim() != "")
                                Query = Query + " and r.batch_year  in ('" + batch + "')";
                            if (degree.Trim() != "")
                                Query = Query + " and r.degree_code  in ('" + degree + "')";
                            if (feecat.Trim() != "")
                                Query = Query + " and f.FeeCategory  in ('" + feecat + "')";
                            if (fnlYear.Trim() != "")
                                Query = Query + " and f.FinYearFK   in ('" + fnlYear + "')";
                            if (!string.IsNullOrEmpty(studMode))
                                Query += " and r.mode in('" + studMode + "')";
                            if (headerValue.Trim() != "")
                                Query = Query + " and ChlGroupHeader   in ('" + headerValue + "')";

                            Query += " select  f.App_No,sum (TotalAmount) as Total,SUM( isnull(PaidAmount,'0')) as Paid,sum(BalAmount)as Balance,FeeCategory,ChlGroupHeader from FT_FeeAllot f,Registration r ,Degree d,Department dt,Course c,FS_ChlGroupHeaderSettings G where f.App_No =r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and G.HeaderFK =F.HeaderFK and Stream =c.type  and d.college_code ='" + collegecode + "' ";
                            if (delflg != "")
                                Query = Query + delflg;
                            if (stream.Trim() != "Both" && stream.Trim() != "")
                                Query = Query + " and type ='" + stream + "'";
                            if (batch.Trim() != "")
                                Query = Query + " and r.batch_year  in ('" + batch + "')";
                            if (degree.Trim() != "")
                                Query = Query + " and r.degree_code  in ('" + degree + "')";
                            if (feecat.Trim() != "")
                                Query = Query + " and f.FeeCategory  in ('" + feecat + "')";
                            if (fnlYear.Trim() != "")
                                Query = Query + " and f.FinYearFK   in ('" + fnlYear + "')";
                            if (!string.IsNullOrEmpty(studMode))
                                Query += " and r.mode in('" + studMode + "')";
                            if (headerValue.Trim() != "")
                                Query = Query + " and ChlGroupHeader   in ('" + headerValue + "')";
                            Query = Query + " group by ChlGroupHeader,f.App_No,FeeCategory Order by f.app_no";

                            Query = Query + " select  f.App_No,sum (Debit) as TotalPaid,FeeCategory,ChlGroupHeader from FT_FinDailyTransaction f,Registration r ,Degree d,Department dt,Course c,FS_ChlGroupHeaderSettings G where f.App_No =r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and G.HeaderFK =F.HeaderFK and Stream =c.type  and d.college_code ='" + collegecode + "'  and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' ";
                            if (usBasedRights == true)
                                Query += " and f.EntryUserCode in('" + usercode + "')";
                            if (delflg != "")
                                Query = Query + delflg;
                            if (stream.Trim() != "Both" && stream.Trim() != "")
                                Query = Query + " and type ='" + stream + "'";
                            if (batch.Trim() != "")
                                Query = Query + " and r.batch_year  in ('" + batch + "')";
                            if (degree.Trim() != "")
                                Query = Query + " and r.degree_code  in ('" + degree + "')";
                            if (feecat.Trim() != "")
                                Query = Query + " and f.FeeCategory  in ('" + feecat + "')";
                            if (fnlYear.Trim() != "")
                                Query = Query + " and f.FinYearFK   in ('" + fnlYear + "')";
                            if (!string.IsNullOrEmpty(studMode))
                                Query += " and r.mode in('" + studMode + "')";
                            if (headerValue.Trim() != "")
                                Query = Query + " and ChlGroupHeader   in ('" + headerValue + "')";
                            Query = Query + " group by ChlGroupHeader,f.App_No,FeeCategory Order by f.app_no";
                            #endregion
                        }
                        Query = Query + " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode + "'";
                        Query = Query + " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code ='" + collegecode + "'";
                        #endregion
                    }
                    else if (ddlacctype.SelectedItem.Text.Trim() == "Header" || ddlacctype.SelectedItem.Text.Trim() == "Ledger")
                    {
                        #region

                        if (ddlfeetype.SelectedItem.Text.Trim() == "Paid")
                        {
                            #region paid
                            string strstufillerfee = " (select f.app_no from ft_feeallot f,fm_ledgermaster fi where fi.ledgerpk=f.ledgerfk and  f.headerfk=fi.headerfk  and totalamount>0 order by isnull(fi.priority,1000), fi.ledgerName asc";
                            strstufillerfee += " and f.feecategory in(" + feecat + ")";
                            string regrolladmit = strstufillerfee + " and f.app_no=r.app_no";
                            #region student
                            if (ddlstudtype.SelectedItem.Text == "Regular" || ddlstudtype.SelectedItem.Text == "Lateral" || ddlstudtype.SelectedItem.Text == "Transfer")
                            {
                                Query = " select distinct r.stud_name as Name,r.Current_Semester,r.Reg_No,r.Roll_No,r.degree_code,r.roll_admit,r.batch_year from registration r, ft_feeallot a where r.cc=0 and r.delflag=0 and r.app_no = a.app_no and totalamount>0 and r.college_code ='" + collegecode + "'  and r.exam_flag<>'debar' and r.mode='" + ddlstudtype.SelectedValue.ToString() + "' ";
                                if (delflg != "")
                                    Query = Query + delflg;
                                if (batch.Trim() != "")
                                    Query = Query + " and r.batch_year  in ('" + batch + "')";
                                if (degree.Trim() != "")
                                    Query = Query + " and r.degree_code  in ('" + degree + "')";
                                if (feecat.Trim() != "")
                                    Query = Query + " and f.FeeCategory  in ('" + feecat + "')";
                                if (headerValue.Trim() != "")
                                    Query = Query + " and headerfk   in ('" + headerValue + "')";
                                if (fnlYear.Trim() != "")
                                    Query = Query + " and f.FinYearFK   in ('" + fnlYear + "')";
                                if (!string.IsNullOrEmpty(studMode))
                                    Query += " and r.mode in('" + studMode + "')";
                                Query += strorder;
                            }
                            else if (ddlstudtype.SelectedItem.Text == "Re-admit")
                            {
                                Query += " select distinct r.stud_name as Name,r.Current_Semester,r.Reg_No,r.Roll_No,r.degree_code,roll_admit,r.batch_year from Readmission a,Registration r where a.App_no=r.App_No  and r.cc=0  and r.delflag=0 and r.exam_flag<>'debar' and r.college_code ='" + collegecode + "'  and a.newbatch_year in('" + batch + "') " + regrolladmit + "  " + strorder + "";//Modify By M.SakthiPriya 04/02/2015
                                if (delflg != "")
                                    Query = Query + delflg;
                                if (fnlYear.Trim() != "")
                                    Query = Query + " and f.FinYearFK   in ('" + fnlYear + "')";
                                if (!string.IsNullOrEmpty(studMode))
                                    Query += " and r.mode in('" + studMode + "')";
                                Query += strorder;
                            }
                            else if (ddlstudtype.SelectedItem.Text == "Re-join")
                            {
                                Query += " select distinct r.stud_name as Name,r.Current_Semester,r.Reg_No,r.Roll_No,r.degree_code,roll_admit,r.batch_year from rejoin_list a,Registration r where a.roll_no=r.Roll_No  and r.cc=0  and r.delflag=0 and r.exam_flag<>'debar' and r.college_code ='" + collegecode + "' and a.newBatchYear in('" + batch + "') " + regrolladmit + "  " + strorder + "";//" + finyearval + "
                                if (!string.IsNullOrEmpty(studMode))
                                    Query += " and r.mode in('" + studMode + "')";
                            }
                            else if (ddlstudtype.SelectedItem.Text == "EnRoll")
                            {
                                Query += " select distinct r.stud_name as Name,r.Current_Semester,'' as Reg_No,r.app_formno as Roll_No,r.degree_code,'' as roll_admit,r.batch_year from applyn r,ft_feeallot a where r.app_no = a.app_no and r.isconfirm=1 and isnull(admission_status,0) = 0 and totalamount>0 and r.is_enroll=1 and r.isconfirm = 1 and r.admission_status = 0 and r.college_code ='" + collegecode + "'";
                                if (delflg != "")
                                    Query = Query + delflg;
                                if (batch.Trim() != "")
                                    Query = Query + " and r.batch_year  in ('" + batch + "')";
                                if (degree.Trim() != "")
                                    Query = Query + " and r.degree_code  in ('" + degree + "')";
                                if (feecat.Trim() != "")
                                    Query = Query + " and a.FeeCategory  in ('" + feecat + "')";
                                if (headerValue.Trim() != "")
                                    Query = Query + " and headerfk   in ('" + headerValue + "')";
                                if (fnlYear.Trim() != "")
                                    Query = Query + " and a.FinYearFK   in ('" + fnlYear + "')";
                                if (!string.IsNullOrEmpty(studMode))
                                    Query += " and r.mode in('" + studMode + "')";
                                Query += strorder;
                            }
                            else
                            {
                                //header yetbe paid
                                if (ddlstudtype.SelectedItem.Text.Trim() != "Before Admission")
                                {
                                    Query += " select distinct r.stud_name as Name,r.Current_Semester,r.Reg_No,r.Roll_No,r.degree_code,r.roll_admit,r.batch_year from ft_feeallot a,registration r where  a.app_no=r.app_no and a.totalamount>0    and r.exam_flag<>'debar' and r.college_code ='" + collegecode + "'";
                                    if (delflg != "")
                                        Query = Query + delflg;
                                    if (batch.Trim() != "")
                                        Query = Query + " and r.batch_year  in ('" + batch + "')";
                                    if (degree.Trim() != "")
                                        Query = Query + " and r.degree_code  in ('" + degree + "')";
                                    if (feecat.Trim() != "")
                                        Query = Query + " and a.FeeCategory  in ('" + feecat + "')";
                                    if (headerValue.Trim() != "")
                                        Query = Query + " and headerfk   in ('" + headerValue + "')";
                                    if (fnlYear.Trim() != "")
                                        Query = Query + " and a.FinYearFK   in ('" + fnlYear + "')";
                                    if (!string.IsNullOrEmpty(studMode))
                                        Query += " and r.mode in('" + studMode + "')";
                                    if (cbbfadm.Checked == true)
                                    {
                                        Query += " union all select distinct r.stud_name as Name,r.Current_Semester,'' Reg_No,'' Roll_No,r.degree_code,r.app_formno as roll_admit,r.batch_year from ft_feeallot a,applyn r where a.app_no=r.app_no and a.totalamount>0  and r.isconfirm = 1 and r.admission_status = '1' and isnull(r.is_enroll,'0')<>'2'   and r.college_code ='" + collegecode + "'";
                                        if (batch.Trim() != "")
                                            Query = Query + " and r.batch_year  in ('" + batch + "')";
                                        if (degree.Trim() != "")
                                            Query = Query + " and r.degree_code  in ('" + degree + "')";
                                        if (feecat.Trim() != "")
                                            Query = Query + " and a.FeeCategory  in ('" + feecat + "')";
                                        if (headerValue.Trim() != "")
                                            Query = Query + " and headerfk   in ('" + headerValue + "')";
                                        if (fnlYear.Trim() != "")
                                            Query = Query + " and a.FinYearFK   in ('" + fnlYear + "')";
                                        if (!string.IsNullOrEmpty(studMode))
                                            Query += " and r.mode in('" + studMode + "')";
                                        Query += strorder;
                                        //and r.app_no not in (select app_no from Registration where degree_code in(" + course_all + ")  and Batch_year in(" + batch_all + ") )
                                        strorder = "";
                                    }
                                    Query += strorder;
                                }
                                else
                                {
                                    Query += " select distinct r.stud_name as Name,r.Current_Semester,r.app_formno as Reg_No,r.app_formno as Roll_No,r.degree_code,r.app_formno as roll_admit,r.batch_year from ft_feeallot a,applyn r where a.app_no=r.app_no and a.totalamount>0  and r.isconfirm = 1 and r.admission_status = '1'  and r.app_no not in (select app_no from Registration where  r.college_code ='" + collegecode + "' ";
                                    if (delflg != "")
                                        Query = Query + delflg;
                                    if (batch.Trim() != "")
                                        Query = Query + " and r.batch_year  in ('" + batch + "')";
                                    if (degree.Trim() != "")
                                        Query = Query + " and r.degree_code  in ('" + degree + "')";
                                    if (feecat.Trim() != "")
                                        Query = Query + " and a.FeeCategory  in ('" + feecat + "')";
                                    if (headerValue.Trim() != "")
                                        Query = Query + " and headerfk   in ('" + headerValue + "')";
                                    if (fnlYear.Trim() != "")
                                        Query = Query + " and a.FinYearFK   in ('" + fnlYear + "')";
                                    if (!string.IsNullOrEmpty(studMode))
                                        Query += " and r.mode in('" + studMode + "')";
                                    Query += strorder;
                                }
                            }
                            #endregion
                            //paid query
                            if (ddlstudtype.SelectedItem.Text.Trim() != "Before Admission")
                            {
                                Query += " select distinct r.stud_name as Name,r.Current_Semester, r.Roll_No,r.roll_admit,r.Reg_No,sum(a.debit) as paid,r.degree_code ,a.paymode as mode " + hdStr + " ,feecategory,a.TransCode as ReceiptNo,a.ddno as challan_no,(select textval from textvaltable where textcode = feecategory)+' ('+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=A.FinYearFK)+')' as FinYear,ActualFinYearFk   from ft_findailytransaction a,registration r where  a.app_no = r.app_no  and credit=0 and transtype=1 and a.debit>0  and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' ";
                                if (delflg != "")
                                    Query += delflg;
                                if (batch.Trim() != "")
                                    Query += " and r.batch_year  in ('" + batch + "')";
                                if (degree.Trim() != "")
                                    Query += " and r.degree_code  in ('" + degree + "')";
                                if (feecat.Trim() != "")
                                    Query += " and a.FeeCategory  in ('" + feecat + "')";
                                if (headerValue.Trim() != "")
                                    Query += " and headerfk   in ('" + headerValue + "')";
                                if (ledgerValue.Trim() != "")
                                    Query += " and ledgerfk   in ('" + ledgerValue + "')";
                                if (fnlYear.Trim() != "")
                                    Query += " and a.ActualFinYearFk   in ('" + fnlYear + "')";
                                if (usBasedRights == true)
                                    Query += " and a.EntryUserCode in('" + usercode + "')";
                                if (bankcode != "")
                                    Query += " and a.DDBankCode in('" + bankcode + "')";
                                if (payMode.Trim() != "")
                                    Query += " and a.paymode in ('" + payMode + "')";
                                if (!string.IsNullOrEmpty(studMode))
                                    Query += " and r.mode in('" + studMode + "')";

                                Query += " and r.college_code ='" + collegecode + "' group by r.roll_no,r.Reg_No,r.roll_admit,r.stud_name,r.degree_code,r.Current_Semester,a.paymode" + hdStr + ",feecategory,a.TransCode,a.ddno,a.feecategory,a.FInYearfk,ActualFinYearFk";
                                if (cbbfadm.Checked == true)
                                {
                                    Query += " union select distinct r.stud_name as Name,r.Current_Semester,r.app_formno as Roll_No,r.app_formno as Reg_No,r.app_formno as roll_admit,sum(a.debit) as paid,r.degree_code ,a.paymode as mode" + hdStr + " ,feecategory,a.TransCode as ReceiptNo,a.ddno as challan_no,(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=A.FinYearFK) as FinYear,ActualFinYearFk  from ft_findailytransaction a,applyn r  where a.app_no = r.app_no and credit=0 and transtype=1 and a.debit>0  and r.isconfirm = 1 and r.admission_status = '1' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(r.is_enroll,'0')<>'2'  ";
                                    //and r.app_no not in (select app_no from Registration where degree_code in(" + course_all + ")  and Batch_year in(" + batch_all + ") )                           
                                    if (batch.Trim() != "")
                                        Query += " and r.batch_year  in ('" + batch + "')";
                                    if (degree.Trim() != "")
                                        Query += " and r.degree_code  in ('" + degree + "')";
                                    if (feecat.Trim() != "")
                                        Query += " and a.FeeCategory  in ('" + feecat + "')";
                                    if (headerValue.Trim() != "")
                                        Query += " and headerfk   in ('" + headerValue + "')";
                                    if (ledgerValue.Trim() != "")
                                        Query += " and ledgerfk   in ('" + ledgerValue + "')";
                                    if (fnlYear.Trim() != "")
                                        Query += " and a.ActualFinYearFk   in ('" + fnlYear + "')";
                                    if (payMode.Trim() != "")
                                        Query += " and a.paymode in ('" + payMode + "')";
                                    if (usBasedRights == true)
                                        Query += " and a.EntryUserCode in('" + usercode + "')";
                                    if (bankcode != "")
                                        Query += " and a.DDBankCode in('" + bankcode + "')";
                                    if (!string.IsNullOrEmpty(studMode))
                                        Query += " and r.mode in('" + studMode + "')";
                                    Query += " and r.college_code ='" + collegecode + "' group by r.app_no,r.app_formno,r.stud_name,r.degree_code,r.Current_Semester,a.paymode" + hdStr + ",feecategory,a.TransCode,a.ddno,a.feecategory,a.FinYearFK,ActualFinYearFk";
                                }
                            }
                            else
                            {
                                //arrear list
                                Query += " select distinct r.stud_name as Name,r.Current_Semester,r.app_formno as Roll_No,r.app_formno as Reg_No,r.app_formno as roll_admit,sum(a.debit) as paid,r.degree_code ,a.paymode as mode" + hdStr + ",feecategory,a.TransCode as ReceiptNo,a.ddno as challan_no,(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=A.FinYearFK) as FinYear,ActualFinYearFk  from ft_findailytransaction a,applyn r  where a.app_no = r.app_no and credit=0 and transtype=1 and a.debit>0  and r.isconfirm = 1 and r.admission_status = '1' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'  and r.app_no not in (select app_no from Registration where degree_code in(" + degree + ")  and Batch_year in(" + batch + ") )";
                                if (delflg != "")
                                    Query += delflg;
                                if (batch.Trim() != "")
                                    Query += " and r.batch_year  in ('" + batch + "')";
                                if (degree.Trim() != "")
                                    Query += " and r.degree_code  in ('" + degree + "')";
                                if (feecat.Trim() != "")
                                    Query += " and a.FeeCategory  in ('" + feecat + "')";
                                if (headerValue.Trim() != "")
                                    Query += " and headerfk   in ('" + headerValue + "')";
                                if (ledgerValue.Trim() != "")
                                    Query += " and ledgerfk   in ('" + ledgerValue + "')";
                                if (fnlYear.Trim() != "")
                                    Query += " and a.ActualFinYearFk   in ('" + fnlYear + "')";
                                if (usBasedRights == true)
                                    Query += " and a.EntryUserCode in('" + usercode + "')";
                                if (payMode.Trim() != "")
                                    Query += " and a.paymode in ('" + payMode + "')";
                                if (bankcode != "")
                                    Query += " and a.DDBankCode in('" + bankcode + "')";
                                if (!string.IsNullOrEmpty(studMode))
                                    Query += " and r.mode in('" + studMode + "')";
                                Query += " and r.college_code ='" + collegecode + "' group by r.app_no,r.app_formno,r.stud_name,r.degree_code,r.Current_Semester,a.paymode " + hdStr + ",feecategory,a.TransCode,a.ddno,a.feecategory,a.FInYearFk,ActualFinYearFk";
                            }
                            Query += " select c.course_id,c.course_name,r.degree_code,r.acronym,e.Dept_Name from course c,degree r,Department e where c.course_id=r.course_id and r.Dept_Code=e.Dept_Code and r.degree_code  in ('" + degree + "') and r.college_code ='" + collegecode + "'";
                            #endregion
                        }
                        else if (ddlfeetype.SelectedItem.Text == "Yet To Be Paid")
                        {
                            #region Query
                            Query = " select distinct f.App_No ,r.degree_code,r.Batch_Year  from FT_FeeAllot f,Registration r ,Degree d,Department dt,Course c where  f.App_No =r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and ISNULL ( BalAmount ,TotalAmount)<>0   and d.college_code ='" + collegecode + "'";
                            if (delflg != "")
                                Query += delflg;
                            if (batch.Trim() != "")
                                Query += " and r.batch_year  in ('" + batch + "')";
                            if (degree.Trim() != "")
                                Query += " and r.degree_code  in ('" + degree + "')";
                            if (feecat.Trim() != "")
                                Query += " and f.FeeCategory  in ('" + feecat + "')";
                            if (fnlYear.Trim() != "")
                                Query += " and f.FinYearFK   in ('" + fnlYear + "')";
                            if (headerValue.Trim() != "")
                                Query += " and headerfk   in ('" + headerValue + "')";
                            if (ledgerValue.Trim() != "")
                                Query += " and ledgerfk   in ('" + ledgerValue + "')";
                            if (!string.IsNullOrEmpty(studMode))
                                Query += " and r.mode in('" + studMode + "')";
                            if (chkfeeroll.Checked == true)
                            {
                                Query = Query + " and r.Roll_No not in(select s.roll_no from stucon s where s.roll_no=r.roll_no and s.semester=r.Current_Semester and s.ack_fee_of_roll=1) order by r.Batch_Year,r.degree_code,f.App_No";
                            }
                            Query = Query + " select distinct f.App_No,r.Roll_No,roll_admit,r.Reg_No,r.Stud_Name,(C.Course_Name +'-'+dt.Dept_Name) as Degree,FeeCategory,r.degree_code,T.TextVal from FT_FeeAllot f,Registration r ,Degree d,Department dt,Course c , TextValTable T  where T.TextCode =F.FeeCategory  and f.App_No =r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and ISNULL ( BalAmount ,TotalAmount)<>0   and d.college_code ='" + collegecode + "'";
                            if (delflg != "")
                                Query += delflg;
                            if (batch.Trim() != "")
                                Query += " and r.batch_year  in ('" + batch + "')";
                            if (degree.Trim() != "")
                                Query += " and r.degree_code  in ('" + degree + "')";
                            if (feecat.Trim() != "")
                                Query += " and f.FeeCategory  in ('" + feecat + "')";
                            if (fnlYear.Trim() != "")
                                Query += " and f.FinYearFK   in ('" + fnlYear + "')";
                            if (headerValue.Trim() != "")
                                Query += " and headerfk   in ('" + headerValue + "')";
                            if (ledgerValue.Trim() != "")
                                Query += " and ledgerfk   in ('" + ledgerValue + "')";
                            if (!string.IsNullOrEmpty(studMode))
                                Query += " and r.mode in('" + studMode + "')";
                            Query += " order by r.degree_code";

                            Query += " select  f.App_No,sum (TotalAmount),SUM( PaidAmount),sum(BalAmount)as Balance,FeeCategory,(select textval from textvaltable where textcode = feecategory)+' ('+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=f.FinYearFK)+')' as FinYear  " + hdStr + " from FT_FeeAllot f,Registration r ,Degree d,Department dt,Course c where f.App_No =r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and ISNULL (BalAmount ,TotalAmount)<>0  and d.college_code ='" + collegecode + "'";
                            if (delflg != "")
                                Query += delflg;
                            if (batch.Trim() != "")
                                Query += " and r.batch_year  in ('" + batch + "')";
                            if (degree.Trim() != "")
                                Query += " and r.degree_code  in ('" + degree + "')";
                            if (feecat.Trim() != "")
                                Query += " and f.FeeCategory  in ('" + feecat + "')";
                            if (fnlYear.Trim() != "")
                                Query += " and f.FinYearFK   in ('" + fnlYear + "')";
                            if (headerValue.Trim() != "")
                                Query += " and headerfk   in ('" + headerValue + "')";
                            if (ledgerValue.Trim() != "")
                                Query += " and ledgerfk   in ('" + ledgerValue + "')";
                            if (!string.IsNullOrEmpty(studMode))
                                Query += " and r.mode in('" + studMode + "')";
                            Query = Query + " group by f.App_No,FeeCategory" + hdStr + ",f.finyearfk Order by f.app_no";

                            Query = Query + " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode + "'";
                            Query = Query + " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code ='" + collegecode + "'";
                            #endregion
                        }
                        else if (ddlfeetype.SelectedItem.Text == "Both")
                        {
                            #region Query
                            Query = Query + " select distinct f.App_No ,r.degree_code,r.Batch_Year  from FT_FeeAllot f,Registration r ,Degree d,Department dt,Course c where  f.App_No =r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and d.college_code ='" + collegecode + "'";

                            if (delflg != "")
                                Query += delflg;
                            if (batch.Trim() != "")
                                Query += " and r.batch_year  in ('" + batch + "')";
                            if (degree.Trim() != "")
                                Query += " and r.degree_code  in ('" + degree + "')";
                            if (feecat.Trim() != "")
                                Query += " and f.FeeCategory  in ('" + feecat + "')";
                            if (fnlYear.Trim() != "")
                                Query += " and f.FinYearFK   in ('" + fnlYear + "')";
                            if (headerValue.Trim() != "")
                                Query += " and headerfk   in ('" + headerValue + "')";
                            if (ledgerValue.Trim() != "")
                                Query += " and ledgerfk   in ('" + ledgerValue + "')";
                            if (!string.IsNullOrEmpty(studMode))
                                Query += " and r.mode in('" + studMode + "')";
                            if (chkfeeroll.Checked == true)
                            {
                                Query = Query + " and r.Roll_No not in(select s.roll_no from stucon s where s.roll_no=r.roll_no and s.semester=r.Current_Semester and s.ack_fee_of_roll=1) order by r.Batch_Year,r.degree_code";
                            }
                            Query = Query + " select distinct f.App_No,r.Roll_No,r.roll_admit,r.Reg_No,r.Stud_Name,(C.Course_Name +'-'+dt.Dept_Name) as Degree,FeeCategory,r.degree_code,T.TextVal from FT_FeeAllot f,Registration r ,Degree d,Department dt,Course c , TextValTable T  where T.TextCode =F.FeeCategory  and f.App_No =r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code ='" + collegecode + "'";

                            if (delflg != "")
                                Query += delflg;
                            if (batch.Trim() != "")
                                Query += " and r.batch_year  in ('" + batch + "')";
                            if (degree.Trim() != "")
                                Query += " and r.degree_code  in ('" + degree + "')";
                            if (feecat.Trim() != "")
                                Query += " and f.FeeCategory  in ('" + feecat + "')";
                            if (fnlYear.Trim() != "")
                                Query += " and f.FinYearFK   in ('" + fnlYear + "')";
                            if (headerValue.Trim() != "")
                                Query += " and headerfk   in ('" + headerValue + "')";
                            if (ledgerValue.Trim() != "")
                                Query += " and ledgerfk   in ('" + ledgerValue + "')";
                            if (!string.IsNullOrEmpty(studMode))
                                Query += " and r.mode in('" + studMode + "')";
                            Query += " select  f.App_No,sum (TotalAmount) as Total,SUM( PaidAmount) as TotalPaid,sum(BalAmount)as Balance,FeeCategory,FeeCategory,(select textval from textvaltable where textcode = feecategory)+' ('+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=f.FinYearFK)+')' as FinYear " + hdStr + " from FT_FeeAllot f,Registration r ,Degree d,Department dt,Course c where f.App_No =r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and d.college_code ='" + collegecode + "'";
                            if (delflg != "")
                                Query += delflg;
                            if (batch.Trim() != "")
                                Query += " and r.batch_year  in ('" + batch + "')";
                            if (degree.Trim() != "")
                                Query += " and r.degree_code  in ('" + degree + "')";
                            if (feecat.Trim() != "")
                                Query += " and f.FeeCategory  in ('" + feecat + "')";
                            if (fnlYear.Trim() != "")
                                Query += " and f.FinYearFK   in ('" + fnlYear + "')";
                            if (headerValue.Trim() != "")
                                Query += " and headerfk   in ('" + headerValue + "')";
                            if (ledgerValue.Trim() != "")
                                Query += " and ledgerfk   in ('" + ledgerValue + "')";
                            if (!string.IsNullOrEmpty(studMode))
                                Query += " and r.mode in('" + studMode + "')";
                            Query = Query + " group by f.App_No,FeeCategory,F.FinyearFk" + hdStr + " Order by f.app_no";
                            //daily transaction
                            Query = Query + " select  f.App_No,sum (Debit) as TotalPaid,FeeCategory,(select textval from textvaltable where textcode = feecategory)+' ('+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=f.FinYearFK)+')' as FinYear ,FeeCategory,ActualFinYearFk" + hdStr + " from FT_FinDailyTransaction f,Registration r ,Degree d,Department dt,Course c where f.App_No =r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code ='" + collegecode + "' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'";
                            //and Stream =c.type 
                            if (delflg != "")
                                Query += delflg;
                            if (batch.Trim() != "")
                                Query += " and r.batch_year  in ('" + batch + "')";
                            if (degree.Trim() != "")
                                Query += " and r.degree_code  in ('" + degree + "')";
                            if (feecat.Trim() != "")
                                Query += " and f.FeeCategory  in ('" + feecat + "')";
                            if (fnlYear.Trim() != "")
                                Query += " and f.ActualFinYearFk   in ('" + fnlYear + "')";
                            if (headerValue.Trim() != "")
                                Query += " and headerfk   in ('" + headerValue + "')";
                            if (ledgerValue.Trim() != "")
                                Query += " and ledgerfk   in ('" + ledgerValue + "')";
                            if (!string.IsNullOrEmpty(studMode))
                                Query += " and r.mode in('" + studMode + "')";
                            Query = Query + " group by f.App_No,f.finyearfk,FeeCategory,ActualFinYearFk" + hdStr + " Order by f.app_no";

                            Query = Query + " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode + "'";
                            Query = Query + " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code ='" + collegecode + "'";
                            #endregion
                        }
                        #endregion
                    }
                }
                else
                {
                    if (ddlacctype.SelectedItem.Text == "Group Header")//Group Header
                    {
                        if (ddlfeetype.SelectedItem.Text == "Paid")
                        {
                            if (ddlstudtype.SelectedItem.Text.Trim() != "Before Admission")
                            {
                                #region query
                                //paid details
                                //date wise student list
                                Query = " select distinct r.stud_name as name,r.Current_Semester,r.Reg_No,r.Roll_No,r.degree_code,r.roll_admit,r.batch_year,d.transdate cal_date,d.TransCode as ReceiptNo,d.ddno as challan_no ,d.feecategory,d.App_no,(select textval from textvaltable where textcode = feecategory)+' ('+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=d.FinYearFK)+')' as FinYear   from registration r,ft_findailytransaction d,FS_ChlGroupHeaderSettings g where r.App_No=d.App_No and d.HeaderFK=g.HeaderFK and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and credit=0 and transtype =1 and r.college_code ='" + collegecode + "'";
                                if (usBasedRights == true)
                                    Query += " and d.EntryUserCode in('" + usercode + "')";
                                if (delflg != "")
                                    Query += delflg;
                                if (stream.Trim() != "Both" && stream.Trim() != "")
                                    Query = Query + " and Stream ='" + stream + "'";
                                if (batch.Trim() != "")
                                    Query = Query + " and r.batch_year  in ('" + batch + "')";
                                if (degree.Trim() != "")
                                    Query = Query + " and r.degree_code  in ('" + degree + "')";
                                if (feecat.Trim() != "")
                                    Query = Query + " and d.FeeCategory  in ('" + feecat + "')";
                                if (fnlYear.Trim() != "")
                                    Query = Query + " and d.ActualFinYearFk   in ('" + fnlYear + "')";
                                if (headerValue.Trim() != "")
                                    Query = Query + " and ChlGroupHeader   in ('" + headerValue + "')";
                                if (bankcode != "")
                                    Query += " and d.DDBankCode in ('" + bankcode + "')";
                                if (payMode.Trim() != "")
                                    Query += " and d.paymode in ('" + payMode + "')";
                                if (!string.IsNullOrEmpty(studMode))
                                    Query += " and r.mode in('" + studMode + "')";
                                Query += " and d.TransDate between '" + fromdate + "' and '" + todate + "'";
                                if (cbbfadm.Checked == true)
                                {
                                    #region before admission

                                    Query += " union select distinct r.stud_name as name,r.Current_Semester,r.app_formno as Reg_No,r.app_formno as Roll_No,r.degree_code,r.app_formno as roll_admit,r.batch_year,d.transdate cal_date,d.TransCode as ReceiptNo,d.ddno as challan_no ,d.feecategory,d.App_no,(select textval from textvaltable where textcode = feecategory)+' ('+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=d.FinYearFK)+')' as FinYear   from applyn r,ft_findailytransaction d,FS_ChlGroupHeaderSettings g where r.App_No=d.App_No and d.HeaderFK=g.HeaderFK and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and credit=0 and transtype =1 and  r.isconfirm = 1 and r.admission_status = '1' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(r.is_enroll,'0')<>'2'   and r.college_code ='" + collegecode + "'";
                                    //and r.app_no not in (select app_no from Registration  where degree_code in(" + course_all + ") and g.stream ='" + typevalue + "'  and Batch_year in(" + batch_all + "))
                                    if (usBasedRights == true)
                                        Query += " and d.EntryUserCode in('" + usercode + "')";
                                    if (stream.Trim() != "Both" && stream.Trim() != "")
                                        Query = Query + " and Stream ='" + stream + "'";
                                    if (batch.Trim() != "")
                                        Query = Query + " and r.batch_year  in ('" + batch + "')";
                                    if (degree.Trim() != "")
                                        Query = Query + " and r.degree_code  in ('" + degree + "')";
                                    if (feecat.Trim() != "")
                                        Query = Query + " and d.FeeCategory  in ('" + feecat + "')";
                                    if (fnlYear.Trim() != "")
                                        Query = Query + " and d.ActualFinYearFk   in ('" + fnlYear + "')";
                                    if (headerValue.Trim() != "")
                                        Query = Query + " and ChlGroupHeader   in ('" + headerValue + "')";
                                    if (bankcode != "")
                                        Query += " and d.DDBankCode in ('" + bankcode + "')";
                                    if (payMode.Trim() != "")
                                        Query += " and d.paymode in ('" + payMode + "')";
                                    if (!string.IsNullOrEmpty(studMode))
                                        Query += " and r.mode in('" + studMode + "')";
                                    Query += " and d.TransDate between '" + fromdate + "' and '" + todate + "'";
                                    #endregion
                                }

                                Query += " select distinct r.stud_name as Name,d.app_no,r.Current_Semester, r.Roll_No,r.roll_admit,r.Reg_No,sum(d.debit) as paid,r.degree_code ,d.paymode as mode ,g.ChlGroupHeader ,feecategory,d.TransCode as ReceiptNo,d.ddno as challan_no ,d.TransDate cal_date,d.feecategory,(select textval from textvaltable where textcode = feecategory)+' ('+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=d.FinYearFK)+')' as FinYear  from ft_findailytransaction d,registration r,FS_ChlGroupHeaderSettings g where  d.app_no = r.app_no and d.HeaderFK=g.HeaderFK and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'   and r.college_code ='" + collegecode + "' and d.debit>0 ";
                                if (usBasedRights == true)
                                    Query += " and d.EntryUserCode in('" + usercode + "')";
                                if (delflg != "")
                                    Query += delflg;
                                if (stream.Trim() != "Both" && stream.Trim() != "")
                                    Query = Query + " and Stream ='" + stream + "'";
                                if (batch.Trim() != "")
                                    Query = Query + " and r.batch_year  in ('" + batch + "')";
                                if (degree.Trim() != "")
                                    Query = Query + " and r.degree_code  in ('" + degree + "')";
                                if (feecat.Trim() != "")
                                    Query = Query + " and d.FeeCategory  in ('" + feecat + "')";
                                if (fnlYear.Trim() != "")
                                    Query = Query + " and d.ActualFinYearFk   in ('" + fnlYear + "')";
                                if (headerValue.Trim() != "")
                                    Query = Query + " and ChlGroupHeader   in ('" + headerValue + "')";
                                if (bankcode != "")
                                    Query += " and d.DDBankCode in ('" + bankcode + "')";
                                if (!string.IsNullOrEmpty(studMode))
                                    Query += " and r.mode in('" + studMode + "')";
                                if (payMode.Trim() != "")
                                    Query += " and d.paymode in ('" + payMode + "')";
                                Query += " and d.TransDate between '" + fromdate + "' and '" + todate + "'";
                                Query += " group by r.roll_no,d.app_no,r.Reg_No,r.roll_admit,r.stud_name,r.degree_code,r.Current_Semester,d.paymode  ,g.ChlGroupHeader,feecategory,d.TransCode,d.ddno,d.TransDate,d.FinYearFk  ";
                                if (cbbfadm.Checked == true)
                                {
                                    #region before admission

                                    Query += " union select distinct r.stud_name as Name,d.app_no,r.Current_Semester, r.app_formno as Roll_No,r.app_formno as roll_admit,r.app_formno as Reg_No,sum(d.debit) as paid,r.degree_code ,d.paymode as mode ,g.ChlGroupHeader ,feecategory,d.TransCode as ReceiptNo,d.ddno as challan_no ,d.TransDate cal_date,d.feecategory from ft_findailytransaction d,applyn r,FS_ChlGroupHeaderSettings g where  d.app_no = r.app_no and d.HeaderFK=g.HeaderFK and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and r.isconfirm = 1 and r.admission_status = '1' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(r.is_enroll,'0')<>'2'  and r.college_code ='" + collegecode + "' and d.debit>0 ";
                                    // and r.app_no not in (select app_no from Registration  where degree_code in(" + course_all + ") and g.stream ='" + typevalue + "' and Batch_year in(" + batch_all + "))
                                    if (usBasedRights == true)
                                        Query += " and d.EntryUserCode in('" + usercode + "')";
                                    if (stream.Trim() != "Both" && stream.Trim() != "")
                                        Query = Query + " and Stream ='" + stream + "'";
                                    if (batch.Trim() != "")
                                        Query = Query + " and r.batch_year  in ('" + batch + "')";
                                    if (degree.Trim() != "")
                                        Query = Query + " and r.degree_code  in ('" + degree + "')";
                                    if (feecat.Trim() != "")
                                        Query = Query + " and d.FeeCategory  in ('" + feecat + "')";
                                    if (fnlYear.Trim() != "")
                                        Query = Query + " and d.ActualFinYearFk   in ('" + fnlYear + "')";
                                    if (headerValue.Trim() != "")
                                        Query = Query + " and ChlGroupHeader   in ('" + headerValue + "')";
                                    if (bankcode != "")
                                        Query += " and d.DDBankCode in ('" + bankcode + "')";
                                    if (payMode.Trim() != "")
                                        Query += " and d.paymode in ('" + payMode + "')";
                                    if (!string.IsNullOrEmpty(studMode))
                                        Query += " and r.mode in('" + studMode + "')";
                                    Query += " and d.TransDate between '" + fromdate + "' and '" + todate + "'";
                                    Query += " group by r.app_formno,d.app_no,r.stud_name,r.degree_code,r.Current_Semester,d.paymode ,g.ChlGroupHeader,feecategory,d.TransCode,d.ddno,d.TransDate ";
                                    #endregion
                                }

                                #endregion
                            }
                            else
                            {
                                #region arrear list
                                //date wise student list
                                Query += "select distinct r.stud_name as name,r.Current_Semester,r.app_formno as Reg_No,r.app_formno as Roll_No,r.degree_code,r.app_formno as roll_admit,r.batch_year,d.transdate cal_date,d.TransCode as ReceiptNo,d.ddno as challan_no ,d.feecategory,d.App_no,(select textval from textvaltable where textcode = feecategory)+' ('+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=d.FinYearFK)+')' as FinYear   from applyn r,ft_findailytransaction d,FS_ChlGroupHeaderSettings g where r.App_No=d.App_No and d.HeaderFK=g.HeaderFK and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and credit=0 and transtype =1 and  r.isconfirm = 1 and r.admission_status = '1' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'  and r.app_no not in (select app_no from Registration  where degree_code in(" + degree + ") and g.stream ='" + stream + "'  and Batch_year in(" + batch + ")) and r.college_code ='" + collegecode + "'";
                                if (usBasedRights == true)
                                    Query += " and d.EntryUserCode in('" + usercode + "')";
                                if (delflg != "")
                                    Query += delflg;
                                if (stream.Trim() != "Both" && stream.Trim() != "")
                                    Query = Query + " and Stream ='" + stream + "'";
                                if (batch.Trim() != "")
                                    Query = Query + " and r.batch_year  in ('" + batch + "')";
                                if (degree.Trim() != "")
                                    Query = Query + " and r.degree_code  in ('" + degree + "')";
                                if (feecat.Trim() != "")
                                    Query = Query + " and d.FeeCategory  in ('" + feecat + "')";
                                if (fnlYear.Trim() != "")
                                    Query = Query + " and d.ActualFinYearFk   in ('" + fnlYear + "')";
                                if (headerValue.Trim() != "")
                                    Query = Query + " and ChlGroupHeader   in ('" + headerValue + "')";
                                if (bankcode != "")
                                    Query += " and d.DDBankCode in ('" + bankcode + "')";
                                if (payMode.Trim() != "")
                                    Query += " and d.paymode in ('" + payMode + "')";
                                if (!string.IsNullOrEmpty(studMode))
                                    Query += " and r.mode in('" + studMode + "')";
                                Query += " and d.TransDate between '" + fromdate + "' and '" + todate + "'";

                                Query += " select distinct r.stud_name as Name,d.app_no,r.Current_Semester, r.app_formno as Roll_No,r.app_formno as Reg_No,r.app_formno as roll_admit,sum(d.debit) as paid,r.degree_code ,d.paymode as mode ,g.ChlGroupHeader ,feecategory,d.TransCode as ReceiptNo,d.ddno as challan_no ,d.TransDate cal_date,d.feecategory,(select textval from textvaltable where textcode = feecategory)+' ('+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=d.FinYearFK)+')' as FinYear  from ft_findailytransaction d,applyn r,FS_ChlGroupHeaderSettings g where  d.app_no = r.app_no and d.HeaderFK=g.HeaderFK and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and r.isconfirm = 1 and r.admission_status = '1' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'  and r.app_no not in (select app_no from Registration  where degree_code in(" + degree + ") and g.stream ='" + stream + "' and Batch_year in(" + batch + "))   and r.college_code ='" + collegecode + "' and d.debit>0 ";
                                if (usBasedRights == true)
                                    Query += " and d.EntryUserCode in('" + usercode + "')";
                                if (delflg != "")
                                    Query += delflg;
                                if (stream.Trim() != "Both" && stream.Trim() != "")
                                    Query = Query + " and Stream ='" + stream + "'";
                                if (batch.Trim() != "")
                                    Query = Query + " and r.batch_year  in ('" + batch + "')";
                                if (degree.Trim() != "")
                                    Query = Query + " and r.degree_code  in ('" + degree + "')";
                                if (feecat.Trim() != "")
                                    Query = Query + " and d.FeeCategory  in ('" + feecat + "')";
                                if (fnlYear.Trim() != "")
                                    Query = Query + " and d.ActualFinYearFk   in ('" + fnlYear + "')";
                                if (headerValue.Trim() != "")
                                    Query = Query + " and ChlGroupHeader   in ('" + headerValue + "')";
                                if (bankcode != "")
                                    Query += " and d.DDBankCode in ('" + bankcode + "')";
                                if (payMode.Trim() != "")
                                    Query += " and d.paymode in ('" + payMode + "')";
                                if (!string.IsNullOrEmpty(studMode))
                                    Query += " and r.mode in('" + studMode + "')";
                                Query += " and d.TransDate between '" + fromdate + "' and '" + todate + "'";
                                Query += " group by r.app_formno,d.app_no,r.stud_name,r.degree_code,r.Current_Semester,d.paymode ,g.ChlGroupHeader,feecategory,d.TransCode,d.ddno,d.TransDate,d.FinYearFk ";

                                #endregion
                            }
                        }
                        else if (ddlfeetype.SelectedItem.Text == "Yet To Be Paid")
                        {
                            #region query
                            // if (ddltype.SelectedItem.Text != "Both")
                            // stream = " and fs.stream in('" + ddltype.SelectedItem.Text + "')";
                            //demand amount
                            Query = " select SUM(TotalAmount) as Demand, f.App_No,r.degree_code,r.Batch_Year  from FT_FeeAllot f,Registration r,FS_ChlGroupHeaderSettings fs where f.App_No =r.App_No and f.HeaderFK =fs.HeaderFK";
                            if (usBasedRights == true)
                                Query += " and f.EntryUserCode in('" + usercode + "')";
                            if (delflg != "")
                                Query += delflg;
                            if (stream.Trim() != "Both" && stream.Trim() != "")
                                Query = Query + " and Stream ='" + stream + "'";
                            if (batch.Trim() != "")
                                Query = Query + " and r.batch_year  in ('" + batch + "')";
                            if (degree.Trim() != "")
                                Query = Query + " and r.degree_code  in ('" + degree + "')";
                            if (feecat.Trim() != "")
                                Query = Query + " and f.FeeCategory  in ('" + feecat + "')";
                            if (fnlYear.Trim() != "")
                                Query = Query + " and f.FinYearFK   in ('" + fnlYear + "')";
                            if (headerValue.Trim() != "")
                                Query = Query + " and ChlGroupHeader   in ('" + headerValue + "')";
                            if (!string.IsNullOrEmpty(studMode))
                                Query += " and r.mode in('" + studMode + "')";
                            Query += "  group by f.App_No,r.degree_code,r.Batch_Year";

                            //paid amount
                            Query += " select SUM(TotalAmount) as Demand, f.App_No,r.degree_code,r.Batch_Year,r.Stud_Name,r.Roll_No,r.roll_admit,r.Reg_No,f.FeeCategory,(select textval from textvaltable where textcode = feecategory)+' ('+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=f.FinYearFK)+')' as FinYear  from FT_FeeAllot f,Registration r,FS_ChlGroupHeaderSettings fs where f.App_No =r.App_No and f.HeaderFK =fs.HeaderFK ";
                            if (usBasedRights == true)
                                Query += " and f.EntryUserCode in('" + usercode + "')";
                            if (delflg != "")
                                Query += delflg;
                            if (stream.Trim() != "Both" && stream.Trim() != "")
                                Query = Query + " and Stream ='" + stream + "'";
                            if (batch.Trim() != "")
                                Query = Query + " and r.batch_year  in ('" + batch + "')";
                            if (degree.Trim() != "")
                                Query = Query + " and r.degree_code  in ('" + degree + "')";
                            if (feecat.Trim() != "")
                                Query = Query + " and f.FeeCategory  in ('" + feecat + "')";
                            if (fnlYear.Trim() != "")
                                Query = Query + " and f.FinYearFK   in ('" + fnlYear + "')";
                            if (headerValue.Trim() != "")
                                Query = Query + " and ChlGroupHeader   in ('" + headerValue + "')";
                            if (!string.IsNullOrEmpty(studMode))
                                Query += " and r.mode in('" + studMode + "')";
                            Query += "  group by f.App_No,r.degree_code,r.Batch_Year,r.Stud_Name,r.Roll_No,r.roll_admit,r.Reg_No,f.FeeCategory,F.Finyearfk order by f.App_No,f.FeeCategory asc";

                            //header and ledger
                            Query += " select SUM(TotalAmount) as Demand, f.App_No,f.FeeCategory,r.degree_code,r.Batch_Year,fs.ChlGroupHeader,(select textval from textvaltable where textcode = feecategory)+' ('+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=f.FinYearFK)+')' as FinYear  from FT_FeeAllot f,Registration r,FS_ChlGroupHeaderSettings fs where f.App_No =r.App_No and f.HeaderFK =fs.HeaderFK ";
                            if (usBasedRights == true)
                                Query += " and f.EntryUserCode in('" + usercode + "')";
                            if (delflg != "")
                                Query += delflg;
                            if (stream.Trim() != "Both" && stream.Trim() != "")
                                Query = Query + " and Stream ='" + stream + "'";
                            if (batch.Trim() != "")
                                Query = Query + " and r.batch_year  in ('" + batch + "')";
                            if (degree.Trim() != "")
                                Query = Query + " and r.degree_code  in ('" + degree + "')";
                            if (feecat.Trim() != "")
                                Query = Query + " and f.FeeCategory  in ('" + feecat + "')";
                            if (fnlYear.Trim() != "")
                                Query = Query + " and f.FinYearFK   in ('" + fnlYear + "')";
                            if (headerValue.Trim() != "")
                                Query = Query + " and ChlGroupHeader   in ('" + headerValue + "')";
                            if (!string.IsNullOrEmpty(studMode))
                                Query += " and r.mode in('" + studMode + "')";
                            Query += "  group by f.App_No,f.FeeCategory,r.degree_code,r.Batch_Year,fs.ChlGroupHeader,f.finyearfk";


                            Query += " select SUM(Debit) as Paid, f.App_No,f.FeeCategory,r.degree_code,r.Batch_Year,r.Stud_Name,r.Roll_No,r.roll_admit,r.Reg_No,fs.ChlGroupHeader,(select textval from textvaltable where textcode = feecategory)+' ('+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=f.FinYearFK)+')' as FinYear  from FT_FinDailyTransaction f,Registration r,FS_ChlGroupHeaderSettings fs where f.App_No =r.App_No and f.HeaderFK =fs.HeaderFK ";
                            if (usBasedRights == true)
                                Query += " and f.EntryUserCode in('" + usercode + "')";
                            if (delflg != "")
                                Query += delflg;
                            if (stream.Trim() != "Both" && stream.Trim() != "")
                                Query = Query + " and Stream ='" + stream + "'";
                            if (batch.Trim() != "")
                                Query = Query + " and r.batch_year  in ('" + batch + "')";
                            if (degree.Trim() != "")
                                Query = Query + " and r.degree_code  in ('" + degree + "')";
                            if (feecat.Trim() != "")
                                Query = Query + " and f.FeeCategory  in ('" + feecat + "')";
                            if (fnlYear.Trim() != "")
                                Query = Query + " and f.ActualFinYearFk   in ('" + fnlYear + "')";
                            if (headerValue.Trim() != "")
                                Query = Query + " and ChlGroupHeader   in ('" + headerValue + "')";
                            if (!string.IsNullOrEmpty(studMode))
                                Query += " and r.mode in('" + studMode + "')";
                            Query += " and TransDate <='" + todate + "'";
                            Query += " group by f.App_No,f.FeeCategory,r.degree_code,r.Batch_Year,r.Stud_Name,r.Roll_No,r.roll_admit,r.Reg_No,fs.ChlGroupHeader,f.finyearfk";

                            #endregion
                        }
                        Query = Query + " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode + "'";
                        Query = Query + " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code ='" + collegecode + "'";
                    }
                    else if (ddlacctype.SelectedItem.Text == "Header" || ddlacctype.SelectedItem.Text == "Ledger")
                    {
                        if (ddlfeetype.SelectedItem.Text == "Paid")
                        {
                            #region student
                            if (ddlstudtype.SelectedItem.Text == "Regular" || ddlstudtype.SelectedItem.Text == "Lateral" || ddlstudtype.SelectedItem.Text == "Transfer")
                            {
                                //and credit=0 and transtype =1 and d.debit>0
                                Query = "select distinct r.stud_name as name,r.Current_Semester,r.Reg_No,r.Roll_No,r.app_no,r.roll_admit,r.degree_code,r.roll_admit,r.batch_year,d.transdate cal_date,d.ddno as challan_no,feecategory,(select textval from textvaltable where textcode = feecategory)+' ('+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=d.FinYearFK)+')' as FinYear  from registration r,ft_findailytransaction d,fm_ledgermaster f where f.ledgerpk=d.ledgerfk  and f.headerfk=d.Headerfk  " + delflg + " and r.mode='" + ddlstudtype.SelectedValue + "'  and d.app_no = r.app_no and credit=0 and transtype =1 and d.debit>0 ";
                                if (delflg != "")
                                    Query += delflg;
                                if (batch.Trim() != "")
                                    Query += " and r.batch_year  in ('" + batch + "')";
                                if (degree.Trim() != "")
                                    Query += " and r.degree_code  in ('" + degree + "')";
                                if (feecat.Trim() != "")
                                    Query += " and d.FeeCategory  in ('" + feecat + "')";
                                if (headerValue.Trim() != "")
                                    Query += " and headerfk   in ('" + headerValue + "')";
                                if (ledgerValue.Trim() != "")
                                    Query += " and ledgerfk   in ('" + ledgerValue + "')";
                                if (fnlYear.Trim() != "")
                                    Query += " and d.FinYearFK   in ('" + fnlYear + "')";
                                if (usBasedRights == true)
                                    Query += " and d.EntryUserCode in('" + usercode + "')";
                                if (bankcode != "")
                                    Query += " and d.DDBankCode in('" + bankcode + "')";
                                if (payMode.Trim() != "")
                                    Query += " and d.paymode in ('" + payMode + "')";
                                if (!string.IsNullOrEmpty(studMode))
                                    Query += " and r.mode in('" + studMode + "')";
                                Query += " and d.TransDate between '" + fromdate + "' and '" + todate + "'";
                                Query += " and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and r.college_code ='" + collegecode + "'";
                                // Query += strorder;
                            }
                            else if (ddlstudtype.SelectedItem.Text == "Re-admit")
                            {
                                Query += " select distinct r.stud_name as Name,r.Current_Semester,r.Reg_No,r.Roll_No,r.app_no,r.degree_code,roll_admit,r.batch_year from Readmission a,Registration r where a.App_no=r.App_No  and r.cc=0  and r.delflag=0 and r.exam_flag<>'debar' and r.college_code ='" + collegecode + "'  and a.newbatch_year in('" + batch + "')   " + strorder + "";//Modify By M.SakthiPriya 04/02/2015
                                if (delflg != "")
                                    Query = Query + delflg;
                                if (fnlYear.Trim() != "")
                                    Query = Query + " and f.FinYearFK   in ('" + fnlYear + "')";
                                if (!string.IsNullOrEmpty(studMode))
                                    Query += " and r.mode in('" + studMode + "')";
                                //  Query += strorder;" + regrolladmit + "
                            }
                            else if (ddlstudtype.SelectedItem.Text == "Re-join")
                            {
                                Query += " select distinct r.stud_name as Name,r.Current_Semester,r.Reg_No,r.app_no,r.Roll_No,r.degree_code,roll_admit,r.batch_year from rejoin_list a,Registration r where a.roll_no=r.Roll_No  and r.cc=0  and r.delflag=0 and r.exam_flag<>'debar' and r.college_code ='" + collegecode + "' and a.newBatchYear in('" + batch + "')   " + strorder + "";//" + finyearval + "" + regrolladmit + "
                                if (!string.IsNullOrEmpty(studMode))
                                    Query += " and r.mode in('" + studMode + "')";
                            }
                            else if (ddlstudtype.SelectedItem.Text == "EnRoll")
                            {
                                Query = "select distinct r.stud_name as Name,r.Current_Semester,'' as Reg_No,r.app_formno as Roll_No,r.app_formno as roll_admit,r.degree_code,'' as roll_admit,r.batch_year,d.transdate cal_date,d.ddno as challan_no,d.TransCode as ReceiptNo  ,feecategory,r.app_no,(select textval from textvaltable where textcode = feecategory)+' ('+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=d.FinYearFK)+')' as FinYear  from applyn r,ft_findailytransaction D where  r.app_no = D.app_no and r.isconfirm=1 and isnull(admission_status,0) = '1' and r.is_enroll=1 and r.isconfirm = 1";
                                if (batch.Trim() != "")
                                    Query += " and r.batch_year  in ('" + batch + "')";
                                if (degree.Trim() != "")
                                    Query += " and r.degree_code  in ('" + degree + "')";
                                if (feecat.Trim() != "")
                                    Query += " and d.FeeCategory  in ('" + feecat + "')";
                                if (headerValue.Trim() != "")
                                    Query += " and headerfk   in ('" + headerValue + "')";
                                if (ledgerValue.Trim() != "")
                                    Query += " and ledgerfk   in ('" + ledgerValue + "')";
                                if (fnlYear.Trim() != "")
                                    Query += " and d.FinYearFK   in ('" + fnlYear + "')";
                                if (usBasedRights == true)
                                    Query += " and d.EntryUserCode in('" + usercode + "')";
                                if (payMode.Trim() != "")
                                    Query += " and d.paymode in ('" + payMode + "')";
                                if (!string.IsNullOrEmpty(studMode))
                                    Query += " and r.mode in('" + studMode + "')";
                                if (bankcode != "")
                                    Query += " and d.DDBankCode in('" + bankcode + "')";
                                Query += " and d.TransDate between '" + fromdate + "' and '" + todate + "'";
                                Query += " and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and r.admission_status = 0 and r.college_code ='" + collegecode + "'";
                                // Query += strorder;
                            }
                            else
                            {
                                //old query
                                if (ddlstudtype.SelectedItem.Text.Trim() != "Before Admission")
                                {
                                    Query += "select distinct r.stud_name as name,r.Current_Semester,r.Reg_No,r.Roll_No,r.degree_code,r.roll_admit,r.app_no,r.batch_year,d.transdate cal_date,d.TransCode as ReceiptNo,d.ddno as challan_no ,feecategory,(select textval from textvaltable where textcode = feecategory)+' ('+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=d.FinYearFK)+')' as FinYear   from registration r,ft_findailytransaction d where d.app_no = r.app_no " + delflg + " and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and r.college_code ='" + collegecode + "'";
                                    if (delflg != "")
                                        Query += delflg;
                                    if (batch.Trim() != "")
                                        Query += " and r.batch_year  in ('" + batch + "')";
                                    if (degree.Trim() != "")
                                        Query += " and r.degree_code  in ('" + degree + "')";
                                    if (feecat.Trim() != "")
                                        Query += " and d.FeeCategory  in ('" + feecat + "')";
                                    if (headerValue.Trim() != "")
                                        Query += " and headerfk   in ('" + headerValue + "')";
                                    if (ledgerValue.Trim() != "")
                                        Query += " and ledgerfk   in ('" + ledgerValue + "')";
                                    if (fnlYear.Trim() != "")
                                        Query += " and d.ActualFinYearFk   in ('" + fnlYear + "')";
                                    if (usBasedRights == true)
                                        Query += " and d.EntryUserCode in('" + usercode + "')";
                                    if (payMode.Trim() != "")
                                        Query += " and d.paymode in ('" + payMode + "')";
                                    if (!string.IsNullOrEmpty(studMode))
                                        Query += " and r.mode in('" + studMode + "')";
                                    if (bankcode != "")
                                        Query += " and d.DDBankCode in('" + bankcode + "')";
                                    Query += " and d.TransDate between '" + fromdate + "' and '" + todate + "'";
                                }
                                else
                                {
                                    //arrear list
                                    Query = " select distinct r.stud_name as name,r.Current_Semester,'' as Reg_No,r.app_formno as Roll_No,r.degree_code,'' as roll_admit,r.batch_year,d.transdate cal_date,d.TransCode as ReceiptNo,d.ddno as challan_no ,feecategory,r.app_no,(select textval from textvaltable where textcode = feecategory)+' ('+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=d.FinYearFK)+')' as FinYear  from applyn r,ft_findailytransaction d where  isconfirm = 1 and admission_status = '1' and d.app_no = r.app_no and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'and r.college_code ='" + collegecode + "'";
                                    if (batch.Trim() != "")
                                        Query += " and r.batch_year  in ('" + batch + "')";
                                    if (degree.Trim() != "")
                                        Query += " and r.degree_code  in ('" + degree + "')";
                                    if (feecat.Trim() != "")
                                        Query += " and d.FeeCategory  in ('" + feecat + "')";
                                    if (headerValue.Trim() != "")
                                        Query += " and headerfk   in ('" + headerValue + "')";
                                    if (ledgerValue.Trim() != "")
                                        Query += " and ledgerfk   in ('" + ledgerValue + "')";
                                    if (fnlYear.Trim() != "")
                                        Query += " and d.ActualFinYearFk   in ('" + fnlYear + "')";
                                    if (payMode.Trim() != "")
                                        Query += " and d.paymode in ('" + payMode + "')";
                                    if (usBasedRights == true)
                                        Query += " and d.EntryUserCode in('" + usercode + "')";
                                    if (!string.IsNullOrEmpty(studMode))
                                        Query += " and r.mode in('" + studMode + "')";
                                    if (bankcode != "")
                                        Query += " and d.DDBankCode in('" + bankcode + "')";
                                    Query += " and d.TransDate between '" + fromdate + "' and '" + todate + "'";
                                    Query += "  and isnull(is_enroll,'0')<>'2' ";
                                    //and r.app_no not in (select app_no from Registration  where " + degrqry + "  and Batch_year in(" + batch_all + ") )
                                }
                            }
                            if (cbbfadm.Checked == true)
                            {
                                Query += " union all   select distinct r.stud_name as name,r.Current_Semester,'' as Reg_No,r.app_formno as Roll_No,r.degree_code,'' as roll_admit,r.app_no,r.batch_year,d.transdate cal_date,d.TransCode as ReceiptNo,d.ddno as challan_no,feecategory,(select textval from textvaltable where textcode = feecategory)+' ('+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=d.FinYearFK)+')' as FinYear  from applyn r,ft_findailytransaction d where isconfirm = 1 and admission_status = '1' and d.app_no = r.app_no and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'and r.college_code ='" + collegecode + "'";
                                if (batch.Trim() != "")
                                    Query += " and r.batch_year  in ('" + batch + "')";
                                if (degree.Trim() != "")
                                    Query += " and r.degree_code  in ('" + degree + "')";
                                if (feecat.Trim() != "")
                                    Query += " and d.FeeCategory  in ('" + feecat + "')";
                                if (headerValue.Trim() != "")
                                    Query += " and headerfk   in ('" + headerValue + "')";
                                if (ledgerValue.Trim() != "")
                                    Query += " and ledgerfk   in ('" + ledgerValue + "')";
                                if (payMode.Trim() != "")
                                    Query += " and d.paymode in ('" + payMode + "')";
                                if (fnlYear.Trim() != "")
                                    Query += " and d.ActualFinYearFk   in ('" + fnlYear + "')";
                                if (usBasedRights == true)
                                    Query += " and d.EntryUserCode in('" + usercode + "')";
                                if (!string.IsNullOrEmpty(studMode))
                                    Query += " and r.mode in('" + studMode + "')";
                                if (bankcode != "")
                                    Query += " and d.DDBankCode in('" + bankcode + "')";
                                Query += " and d.TransDate between '" + fromdate + "' and '" + todate + "'";
                                Query += "  and isnull(is_enroll,'0')<>'2'  ";
                                //and r.app_no not in (select app_no from Registration  where " + degrqry + "  and Batch_year in(" + batch_all + ") )
                            }
                            Query += strorder;
                            #endregion
                            #region paid query
                            if (ddlstudtype.SelectedItem.Text.Trim() != "Before Admission")
                            {
                                Query += " select distinct r.stud_name as Name,r.Current_Semester, r.Roll_No,r.roll_admit,r.Reg_No,sum(d.debit) as paid,r.degree_code ,d.paymode as mode " + hdStr + " ,feecategory,d.TransCode as ReceiptNo,d.ddno as challan_no ,d.TransDate cal_date,d.app_no,(select textval from textvaltable where textcode = feecategory)+' ('+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=d.FinYearFK)+')' as FinYear  from ft_findailytransaction d,registration r where  d.app_no = r.app_no  and credit=0 and transtype=1 and d.debit>0  and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and r.college_code ='" + collegecode + "'";
                                if (delflg != "")
                                    Query += delflg;
                                if (batch.Trim() != "")
                                    Query += " and r.batch_year  in ('" + batch + "')";
                                if (degree.Trim() != "")
                                    Query += " and r.degree_code  in ('" + degree + "')";
                                if (feecat.Trim() != "")
                                    Query += " and d.FeeCategory  in ('" + feecat + "')";
                                if (headerValue.Trim() != "")
                                    Query += " and headerfk   in ('" + headerValue + "')";
                                if (ledgerValue.Trim() != "")
                                    Query += " and ledgerfk   in ('" + ledgerValue + "')";
                                if (fnlYear.Trim() != "")
                                    Query += " and d.ActualFinYearFk   in ('" + fnlYear + "')";
                                if (usBasedRights == true)
                                    Query += " and d.EntryUserCode in('" + usercode + "')";
                                if (payMode.Trim() != "")
                                    Query += " and d.paymode in ('" + payMode + "')";
                                if (bankcode != "")
                                    Query += " and d.DDBankCode in('" + bankcode + "')";
                                if (!string.IsNullOrEmpty(studMode))
                                    Query += " and r.mode in('" + studMode + "')";
                                Query += " and d.TransDate between '" + fromdate + "' and '" + todate + "'";
                                Query += " group by r.roll_no,r.roll_admit,r.Reg_No,r.stud_name,r.degree_code,r.Current_Semester,d.paymode " + hdStr + ",feecategory,d.TransCode,d.ddno,d.TransDate,d.app_no,d.finyearfk ";
                                if (cbbfadm.Checked == true)
                                {
                                    Query += " union select distinct r.stud_name as Name,r.Current_Semester,r.app_formno as Roll_No,r.app_formno as Reg_No,r.app_formno as roll_admit,sum(d.debit) as paid,r.degree_code ,d.paymode as mode " + hdStr + " ,feecategory,d.TransCode as ReceiptNo,d.ddno as challan_no ,d.TransDate cal_date,d.app_no,(select textval from textvaltable where textcode = feecategory)+' ('+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=d.FinYearFK)+')' as FinYear  from ft_findailytransaction d,applyn r  where d.app_no = r.app_no  and credit=0 and transtype=1 and d.debit>0  and r.isconfirm = 1 and r.admission_status = '1' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and isnull(is_enroll,'0')<>'2'   and r.college_code ='" + collegecode + "'";
                                    //and r.app_no not in (select app_no from Registration  where " + degrqry + "  and Batch_year in(" + batch_all + ") )                          
                                    if (batch.Trim() != "")
                                        Query += " and r.batch_year  in ('" + batch + "')";
                                    if (degree.Trim() != "")
                                        Query += " and r.degree_code  in ('" + degree + "')";
                                    if (feecat.Trim() != "")
                                        Query += " and d.FeeCategory  in ('" + feecat + "')";
                                    if (headerValue.Trim() != "")
                                        Query += " and headerfk   in ('" + headerValue + "')";
                                    if (ledgerValue.Trim() != "")
                                        Query += " and ledgerfk   in ('" + ledgerValue + "')";
                                    if (fnlYear.Trim() != "")
                                        Query += " and d.ActualFinYearFk   in ('" + fnlYear + "')";
                                    if (payMode.Trim() != "")
                                        Query += " and d.paymode in ('" + payMode + "')";
                                    if (usBasedRights == true)
                                        Query += " and d.EntryUserCode in('" + usercode + "')";
                                    if (bankcode != "")
                                        Query += " and d.DDBankCode in('" + bankcode + "')";
                                    if (!string.IsNullOrEmpty(studMode))
                                        Query += " and r.mode in('" + studMode + "')";
                                    Query += " and d.TransDate between '" + fromdate + "' and '" + todate + "'";
                                    Query += "  group by r.app_no,r.app_formno,r.stud_name,r.degree_code,r.Current_Semester,d.paymode" + hdStr + ",feecategory,d.TransCode,d.ddno,d.app_no,d.TransDate,d.finyearfk ";
                                }
                            }
                            else
                            {
                                Query += " select distinct r.stud_name as Name,r.Current_Semester,r.app_formno as Roll_No,r.app_formno as Reg_No,r.app_formno as roll_admit,sum(d.debit) as paid,r.degree_code ,d.paymode as mode " + hdStr + ",feecategory,d.TransCode as ReceiptNo,d.ddno as challan_no ,d.TransDate cal_date,d.app_no,(select textval from textvaltable where textcode = feecategory)+' ('+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=d.FinYearFK)+')' as FinYear  from ft_findailytransaction d,applyn r  where d.app_no = r.app_no and credit=0 and transtype=1 and d.debit>0  and r.isconfirm = 1 and r.admission_status = '1' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'  and r.app_no not in (select app_no from Registration  where degree_code  in ('" + degree + "')  and Batch_year in(" + batch + ") ) and r.college_code ='" + collegecode + "'";
                                if (batch.Trim() != "")
                                    Query += " and r.batch_year  in ('" + batch + "')";
                                if (degree.Trim() != "")
                                    Query += " and r.degree_code  in ('" + degree + "')";
                                if (feecat.Trim() != "")
                                    Query += " and d.FeeCategory  in ('" + feecat + "')";
                                if (headerValue.Trim() != "")
                                    Query += " and headerfk   in ('" + headerValue + "')";
                                if (ledgerValue.Trim() != "")
                                    Query += " and ledgerfk   in ('" + ledgerValue + "')";
                                if (fnlYear.Trim() != "")
                                    Query += " and d.ActualFinYearFk   in ('" + fnlYear + "')";
                                if (payMode.Trim() != "")
                                    Query += " and d.paymode in ('" + payMode + "')";
                                if (usBasedRights == true)
                                    Query += " and d.EntryUserCode in('" + usercode + "')";
                                if (!string.IsNullOrEmpty(studMode))
                                    Query += " and r.mode in('" + studMode + "')";
                                if (bankcode != "")
                                    Query += " and d.DDBankCode in('" + bankcode + "')";
                                Query += " and d.TransDate between '" + fromdate + "' and '" + todate + "'";
                                Query += "  group by r.app_no,r.app_formno,r.stud_name,r.degree_code,r.Current_Semester,d.paymode" + hdStr + ",feecategory,d.TransCode,d.ddno,d.app_no,d.TransDate,d.finyearfk ";
                            }
                            #endregion
                        }
                        else if (ddlfeetype.SelectedItem.Text == "Yet To Be Paid")
                        {
                            #region query
                            //demand amount
                            Query = " select SUM(TotalAmount) as Demand, f.App_No,r.degree_code,r.Batch_Year,(select textval from textvaltable where textcode = feecategory)+' ('+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=f.FinYearFK)+')' as FinYear   from FT_FeeAllot f,Registration r where f.App_No =r.App_No ";
                            if (delflg != "")
                                Query += delflg;
                            if (batch.Trim() != "")
                                Query += " and r.batch_year  in ('" + batch + "')";
                            if (degree.Trim() != "")
                                Query += " and r.degree_code  in ('" + degree + "')";
                            if (feecat.Trim() != "")
                                Query += " and f.FeeCategory  in ('" + feecat + "')";
                            if (headerValue.Trim() != "")
                                Query += " and headerfk   in ('" + headerValue + "')";
                            if (ledgerValue.Trim() != "")
                                Query += " and ledgerfk   in ('" + ledgerValue + "')";
                            if (fnlYear.Trim() != "")
                                Query += " and f.FinYearFK   in ('" + fnlYear + "')";
                            if (!string.IsNullOrEmpty(studMode))
                                Query += " and r.mode in('" + studMode + "')";
                            Query += "  group by f.App_No,r.degree_code,r.Batch_Year,f.finyearfk,feecategory";

                            //paid amount
                            Query += " select SUM(TotalAmount) as Demand, f.App_No,r.degree_code,r.Batch_Year,r.Stud_Name,r.Roll_No,r.roll_admit,r.Reg_No,f.FeeCategory,(select textval from textvaltable where textcode = feecategory)+' ('+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=f.FinYearFK)+')' as FinYear  from FT_FeeAllot f,Registration r where f.App_No =r.App_No ";
                            if (delflg != "")
                                Query += delflg;
                            if (batch.Trim() != "")
                                Query += " and r.batch_year  in ('" + batch + "')";
                            if (degree.Trim() != "")
                                Query += " and r.degree_code  in ('" + degree + "')";
                            if (feecat.Trim() != "")
                                Query += " and f.FeeCategory  in ('" + feecat + "')";
                            if (headerValue.Trim() != "")
                                Query += " and headerfk   in ('" + headerValue + "')";
                            if (ledgerValue.Trim() != "")
                                Query += " and ledgerfk   in ('" + ledgerValue + "')";
                            if (fnlYear.Trim() != "")
                                Query += " and f.FinYearFK   in ('" + fnlYear + "')";
                            if (!string.IsNullOrEmpty(studMode))
                                Query += " and r.mode in('" + studMode + "')";
                            Query += "  group by f.App_No,r.degree_code,r.Batch_Year,r.Stud_Name,r.Roll_No,r.roll_admit,r.Reg_No,f.FeeCategory,f.finyearfk order by f.App_No,f.FeeCategory asc";


                            //header and ledger
                            Query += " select SUM(TotalAmount) as Demand, f.App_No,f.FeeCategory,r.degree_code,r.Batch_Year,(select textval from textvaltable where textcode = feecategory)+' ('+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=f.FinYearFK)+')' as FinYear " + hdStr + "  from FT_FeeAllot f,Registration r where f.App_No =r.App_No ";
                            if (delflg != "")
                                Query += delflg;
                            if (batch.Trim() != "")
                                Query += " and r.batch_year  in ('" + batch + "')";
                            if (degree.Trim() != "")
                                Query += " and r.degree_code  in ('" + degree + "')";
                            if (feecat.Trim() != "")
                                Query += " and f.FeeCategory  in ('" + feecat + "')";
                            if (headerValue.Trim() != "")
                                Query += " and headerfk   in ('" + headerValue + "')";
                            if (ledgerValue.Trim() != "")
                                Query += " and ledgerfk   in ('" + ledgerValue + "')";
                            if (fnlYear.Trim() != "")
                                Query += " and f.FinYearFK   in ('" + fnlYear + "')";
                            if (!string.IsNullOrEmpty(studMode))
                                Query += " and r.mode in('" + studMode + "')";
                            Query += "  group by f.App_No,f.FeeCategory,r.degree_code,r.Batch_Year,f.finyearfk" + hdStr + "";


                            //paid amt with headerfk or ledgerfk
                            Query += " select SUM(Debit) as Paid, f.App_No,f.FeeCategory,r.degree_code,r.Batch_Year,r.Stud_Name,r.Roll_No,r.roll_admit,r.Reg_No,(select textval from textvaltable where textcode = feecategory)+' ('+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=f.FinYearFK)+')' as FinYear " + hdStr + "  from FT_FinDailyTransaction f,Registration r where f.App_No =r.App_No";
                            if (delflg != "")
                                Query += delflg;
                            if (batch.Trim() != "")
                                Query += " and r.batch_year  in ('" + batch + "')";
                            if (degree.Trim() != "")
                                Query += " and r.degree_code  in ('" + degree + "')";
                            if (feecat.Trim() != "")
                                Query += " and f.FeeCategory  in ('" + feecat + "')";
                            if (headerValue.Trim() != "")
                                Query += " and headerfk   in ('" + headerValue + "')";
                            if (ledgerValue.Trim() != "")
                                Query += " and ledgerfk   in ('" + ledgerValue + "')";
                            if (fnlYear.Trim() != "")
                                Query += " and f.FinYearFK   in ('" + fnlYear + "')";
                            if (usBasedRights == true)
                                Query += " and f.EntryUserCode in('" + usercode + "')";
                            if (bankcode != "")
                                Query += " and f.DDBankCode in('" + bankcode + "')";
                            if (!string.IsNullOrEmpty(studMode))
                                Query += " and r.mode in('" + studMode + "')";
                            if (payMode.Trim() != "")
                                //  Query += " and f.paymode in ('" + payMode + "')";
                                Query += " and TransDate <='" + todate + "' and ISNULL(IsCanceled,'0')='0'";
                            Query += " group by f.App_No,f.FeeCategory,r.degree_code,r.Batch_Year,r.Stud_Name,r.Roll_No,r.roll_admit,r.Reg_No,f.finyearfk" + hdStr + "";

                            //Query = Query + " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code ='" + collegecode + "'";
                            //Query = Query + " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode + "'";
                            #endregion
                        }
                        Query = Query + " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode + "'";
                        Query = Query + " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code ='" + collegecode + "'";
                    }
                }
                dsval.Clear();
                dsval = d2.select_method_wo_parameter(Query, "Text");
                #endregion
            }
            //else
            //{
            //    lbl_alert.Text = "Please Select Values";
            //    alertDiv.Visible = true;
            //}

        }
        catch { }
        return dsval;
    }

    protected void loadspreadDetails(ref Hashtable htColCnt)
    {
        try
        {
            #region design
            //roll no settins
            RollAndRegSettings();
            loadcolumns();
            spreadDet.Sheets[0].RowCount = 0;
            spreadDet.Sheets[0].ColumnCount = 0;
            spreadDet.CommandBar.Visible = false;
            spreadDet.Sheets[0].AutoPostBack = true;
            spreadDet.Sheets[0].RowHeader.Visible = false;
            spreadDet.Sheets[0].ColumnCount = 10;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            spreadDet.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Student Name";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            spreadDet.Sheets[0].Columns[1].Width = 350;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            spreadDet.Sheets[0].Columns[2].Visible = true;
            if (!colord.Contains("2"))
                spreadDet.Sheets[0].Columns[2].Visible = false;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg No";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            spreadDet.Sheets[0].Columns[3].Visible = true;
            if (!colord.Contains("3"))
                spreadDet.Sheets[0].Columns[3].Visible = false;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Admission No";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
            spreadDet.Sheets[0].Columns[4].Visible = true;
            if (!colord.Contains("4"))
                spreadDet.Sheets[0].Columns[4].Visible = false;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].Text = lbldeg.Text;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
            spreadDet.Sheets[0].Columns[5].Visible = true;
            if (!colord.Contains("5"))
                spreadDet.Sheets[0].Columns[5].Visible = false;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 6].Text = lblsem.Text;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Left;
            spreadDet.Sheets[0].Columns[6].Visible = true;
            if (!colord.Contains("6"))
                spreadDet.Sheets[0].Columns[6].Visible = false;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Receipt No";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 7].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Left;
            spreadDet.Sheets[0].Columns[7].Visible = true;
            if (!colord.Contains("7"))
                spreadDet.Sheets[0].Columns[7].Visible = false;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Challan No";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 8].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Left;
            spreadDet.Sheets[0].Columns[8].Visible = true;
            if (!colord.Contains("8"))
                spreadDet.Sheets[0].Columns[8].Visible = false;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Fee Type";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 9].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Left;
            spreadDet.Sheets[0].Columns[9].Visible = true;
            if (!colord.Contains("9"))
                spreadDet.Sheets[0].Columns[9].Visible = false;

            int checkva = 0;
            // Hashtable htColCnt = new Hashtable();
            int pCnt = 0;
            bool boolfnltot = true;
            string strTotal = string.Empty;
            if (ddlfeetype.SelectedItem.Text == "Yet To Be Paid")
                strTotal = "Total Balance";
            else
                strTotal = "Total Paid";
            if (ddlacctype.SelectedIndex == 0 || ddlacctype.SelectedIndex == 1)
            {
                #region group,header
                spreadDet.Sheets[0].ColumnHeader.RowCount = 1;
                pCnt = spreadDet.Sheets[0].ColumnCount++;
                bool checkbool = false;
                for (int s = 0; s < cblheader.Items.Count; s++)
                {
                    int tempcnt = 0;
                    if (cblheader.Items[s].Selected == true)
                    {
                        checkva++;
                        if (checkva > 1)
                            tempcnt = spreadDet.Sheets[0].ColumnCount++;
                        if (pCnt == 0)
                            pCnt = tempcnt;
                        htColCnt.Add(Convert.ToString(cblheader.Items[s].Value), spreadDet.Sheets[0].ColumnCount - 1);
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(cblheader.Items[s].Text);
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(cblheader.Items[s].Value);
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                        if (ddlfeetype.SelectedItem.Text == "Both")
                        {
                            #region
                            boolfnltot = false;
                            spreadDet.Sheets[0].ColumnHeader.RowCount = 2;
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Text = "Allot";
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                            spreadDet.Sheets[0].ColumnCount++;
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Text = "Paid";
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                            spreadDet.Sheets[0].ColumnCount++;
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Text = "Balance";
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, pCnt, 1, 3);
                            pCnt = 0;
                            checkbool = true;
                            #endregion
                        }
                    }
                }
                if (checkbool)
                {
                    #region
                    spreadDet.Sheets[0].ColumnCount++;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Text = "Total Allot";
                    htColCnt.Add("Total Allot", spreadDet.Sheets[0].ColumnCount - 1);
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, spreadDet.Sheets[0].ColumnCount - 1, 2, 1);
                    spreadDet.Sheets[0].ColumnCount++;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Text = "Total Paid";
                    htColCnt.Add("Total Paid", spreadDet.Sheets[0].ColumnCount - 1);
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, spreadDet.Sheets[0].ColumnCount - 1, 2, 1);
                    spreadDet.Sheets[0].ColumnCount++;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Text = "Total Balance";
                    htColCnt.Add("Total Balance", spreadDet.Sheets[0].ColumnCount - 1);
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, spreadDet.Sheets[0].ColumnCount - 1, 2, 1);
                    spreadDet.Sheets[0].ColumnCount++;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Text = "Excess Amount";
                    htColCnt.Add("Excess Amount", spreadDet.Sheets[0].ColumnCount - 1);
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, spreadDet.Sheets[0].ColumnCount - 1, 2, 1);
                    spreadDet.Sheets[0].ColumnCount++;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Text = "Status";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, spreadDet.Sheets[0].ColumnCount - 1, 2, 1);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 2, 1);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 2, 1);
                    #endregion
                }
                // spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, pCnt, 1, checkva);
                #endregion
            }
            else
            {
                #region ledger
                DataSet lgacr = new DataSet();
                if (cbledgacr.Checked == true)
                {
                    string strled = "SELECT LedgerPK ,LedgerAcr FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode   AND P. UserCode = '" + usercode + "' AND L.CollegeCode = '" + collegecode + "'  and LedgerMode='0'  order by isnull(l.priority,1000), l.ledgerName asc ";
                    lgacr = d2.select_method_wo_parameter(strled, "Text");
                }
                spreadDet.Sheets[0].ColumnHeader.RowCount = 2;
                pCnt = spreadDet.Sheets[0].ColumnCount++;
                checkva = 0;
                int cnt = 0;
                bool checkbool = false;
                string ledgerStrHd = string.Empty;
                for (int hd = 0; hd < treeledger.Nodes.Count; hd++)
                {
                    if (treeledger.Nodes[hd].ChildNodes.Count > 0)
                    {
                        int tempcnt = 0;
                        int tempcolumn = 0;
                        for (int child = 0; child < treeledger.Nodes[hd].ChildNodes.Count; child++)
                        {
                            if (treeledger.Nodes[hd].ChildNodes[child].Checked == true)
                            {
                                checkva++;
                                tempcnt++;
                                if (checkva > 1)
                                    cnt = spreadDet.Sheets[0].ColumnCount++;
                                if (pCnt == 0)
                                {
                                    pCnt = cnt;
                                    tempcolumn = cnt;
                                }
                                else if (tempcolumn == 0 && pCnt != 0 && cnt == 0)
                                    tempcolumn = pCnt;
                                else
                                    tempcolumn = cnt;
                                string ledgerfk = Convert.ToString(treeledger.Nodes[hd].ChildNodes[child].Value);
                                htColCnt.Add(ledgerfk, spreadDet.Sheets[0].ColumnCount - 1);
                                if (cbledgacr.Checked == true)
                                {
                                    #region ledger acronym
                                    DataView dv = new DataView();
                                    if (lgacr.Tables.Count > 0)
                                    {
                                        lgacr.Tables[0].DefaultView.RowFilter = "ledgerPk='" + ledgerfk + "'";
                                        dv = lgacr.Tables[0].DefaultView;
                                        if (dv.Count > 0)
                                        {
                                            string name = Convert.ToString(dv[0]["LedgerAcr"]);
                                            spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Text = name;
                                            //ledgerStrHd = name;
                                            ledgerStrHd = ledgerfk;
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(treeledger.Nodes[hd].ChildNodes[child].Text);
                                    //ledgerStrHd = Convert.ToString(treeledger.Nodes[hd].ChildNodes[child].Text);
                                    ledgerStrHd = Convert.ToString(treeledger.Nodes[hd].ChildNodes[child].Value);
                                }

                                spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(treeledger.Nodes[hd].ChildNodes[child].Value);
                                spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                if (ddlfeetype.SelectedItem.Text == "Both")
                                {
                                    #region
                                    boolfnltot = false;
                                    spreadDet.Sheets[0].ColumnHeader.RowCount = 3;
                                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Text = "Allot";
                                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                    spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                    spreadDet.Sheets[0].ColumnCount++;
                                    tempcnt++;
                                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Text = "Paid";
                                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                    spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                    spreadDet.Sheets[0].ColumnCount++;
                                    tempcnt++;
                                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Text = "Balance";
                                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                    spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(1, tempcolumn, 1, 3);
                                    tempcolumn = 0;
                                    checkbool = true;
                                    #endregion
                                }
                            }
                        }
                        if (tempcnt > 0)
                        {
                            if (ddlfeetype.SelectedItem.Text != "Both")
                            {
                                //every ledger total amount
                                spreadDet.Sheets[0].ColumnCount++;
                                spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Text = strTotal;
                                spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                htColCnt.Add(ledgerStrHd + " TotalAmount", spreadDet.Sheets[0].ColumnCount - 1);
                                tempcnt++;
                            }
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, pCnt].Text = Convert.ToString(treeledger.Nodes[hd].Text);
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, pCnt].ForeColor = ColorTranslator.FromHtml("#000000");
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, pCnt].Font.Bold = true;
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, pCnt].Font.Name = "Book Antiqua";
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, pCnt].Font.Size = FontUnit.Medium;
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, pCnt].HorizontalAlign = HorizontalAlign.Center;
                            //spreadDet.Sheets[0].Columns[pCnt].HorizontalAlign = HorizontalAlign.Right;
                            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, pCnt, 1, tempcnt);
                            pCnt = 0;
                        }
                    }
                }
                spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
                spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);
                spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 2, 1);
                spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 2, 1);
                if (checkbool)
                {
                    #region
                    spreadDet.Sheets[0].ColumnCount++;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Text = "Total Allot";
                    htColCnt.Add("Total Allot", spreadDet.Sheets[0].ColumnCount - 1);
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, spreadDet.Sheets[0].ColumnCount - 1, 3, 1);
                    spreadDet.Sheets[0].ColumnCount++;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Text = "Total Paid";
                    htColCnt.Add("Total Paid", spreadDet.Sheets[0].ColumnCount - 1);
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, spreadDet.Sheets[0].ColumnCount - 1, 3, 1);
                    spreadDet.Sheets[0].ColumnCount++;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Text = "Total Balance";
                    htColCnt.Add("Total Balance", spreadDet.Sheets[0].ColumnCount - 1);
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, spreadDet.Sheets[0].ColumnCount - 1, 3, 1);
                    spreadDet.Sheets[0].ColumnCount++;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Text = "Excess Amount";
                    htColCnt.Add("Excess Amount", spreadDet.Sheets[0].ColumnCount - 1);
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, spreadDet.Sheets[0].ColumnCount - 1, 3, 1);
                    spreadDet.Sheets[0].ColumnCount++;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Text = "Status";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, spreadDet.Sheets[0].ColumnCount - 1, 3, 1);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 3, 1);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 3, 1);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 3, 1);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 3, 1);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 3, 1);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 3, 1);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 3, 1);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 3, 1);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 3, 1);
                    #endregion
                }
                #endregion
            }
            if (boolfnltot)
            {
                spreadDet.Sheets[0].ColumnCount++;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Text = strTotal;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, spreadDet.Sheets[0].ColumnCount - 1, 2, 1);
            }
            #endregion

            // spreadDet.SaveChanges();
        }
        catch { }
    }

    protected void btnGos_Click(object sender, EventArgs e)
    {
        divcolorder.Attributes.Add("Style", "display:none;");
        txtexcelname.Text = string.Empty;
        lblvalidation1.Text = string.Empty;
        DateTime fromdt = new DateTime();
        DateTime todt = new DateTime();
        ds.Clear();
        ds = getDetails(ref fromdt, ref todt);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            #region value
            if (!cbdate.Checked)
            {
                if (ddlacctype.SelectedItem.Text.Trim() == "Group Header")
                {
                    if (ddlfeetype.SelectedItem.Text.Trim() == "Paid")
                        groupHdPaidBalance(ds);
                    else if (ddlfeetype.SelectedItem.Text.Trim() == "Yet To Be Paid")
                        groupHdPaidBalance(ds);
                    else if (ddlfeetype.SelectedItem.Text.Trim() == "Both")
                        groupHdBoth(ds);
                }
                else if (ddlacctype.SelectedItem.Text.Trim() == "Header")
                {
                    if (ddlfeetype.SelectedItem.Text.Trim() == "Paid")
                        headerPaid(ds);
                    else if (ddlfeetype.SelectedItem.Text.Trim() == "Yet To Be Paid")
                        groupHdPaidBalance(ds);
                    else if (ddlfeetype.SelectedItem.Text.Trim() == "Both")
                        groupHdBoth(ds);
                }
                else
                {
                    if (ddlfeetype.SelectedItem.Text.Trim() == "Paid")
                        headerPaid(ds);
                    else if (ddlfeetype.SelectedItem.Text.Trim() == "Yet To Be Paid")
                        groupHdPaidBalance(ds);
                    else if (ddlfeetype.SelectedItem.Text.Trim() == "Both")
                        groupHdBoth(ds);
                }
            }
            else
            {
                if (ddlacctype.SelectedItem.Text.Trim() == "Group Header")
                {
                    if (ddlfeetype.SelectedItem.Text.Trim() == "Paid")
                        DatwiseGroupHdPaid(ds, fromdt, todt);
                    else if (ddlfeetype.SelectedItem.Text.Trim() == "Yet To Be Paid")
                        headerLedgerYetToBePaid(ds);
                    else if (ddlfeetype.SelectedItem.Text.Trim() == "Both")
                        groupHdPaidBalance(ds);
                }
                else if (ddlacctype.SelectedItem.Text.Trim() == "Header")
                {
                    if (ddlfeetype.SelectedItem.Text.Trim() == "Paid")
                        DatwiseGroupHdPaid(ds, fromdt, todt);
                    else if (ddlfeetype.SelectedItem.Text.Trim() == "Yet To Be Paid")
                        headerLedgerYetToBePaid(ds);
                    //else if (ddlfeetype.SelectedItem.Text.Trim() == "Both")
                    //    groupHdPaidBalance(ds);
                }
                else
                {
                    if (ddlfeetype.SelectedItem.Text.Trim() == "Paid")
                        DatwiseGroupHdPaid(ds, fromdt, todt);
                    else if (ddlfeetype.SelectedItem.Text.Trim() == "Yet To Be Paid")
                        headerLedgerYetToBePaid(ds);
                    //else if (ddlfeetype.SelectedItem.Text.Trim() == "Both")
                    //    groupHdPaidBalance(ds);
                }
            }
            #endregion
        }
        else
        {
            spreadDet.Visible = false;
            print.Visible = false;
            txtexcelname.Text = string.Empty;
            lblvalidation1.Text = string.Empty;
            divlabl.Visible = false;
            lbl_alert.Text = "No Record Found";
            alertDiv.Visible = true;
        }
        // loadspreadDetails();
    }

    protected void groupHdPaidBalance(DataSet ds)
    {
        try
        {
            #region value
            challanAndReceiptNoRights();
            Hashtable htColCnt = new Hashtable();
            RollAndRegSettings();
            loadspreadDetails(ref  htColCnt);
            spreadColumnVisible();
            string Linkvalue = ArrearLinkValue();
            int sno = 0;
            int height = 0;
            bool checkStatus = false;
            bool booldegree = false;
            Hashtable GrandTotal = new Hashtable();
            Dictionary<string, Double> diccurarrfee = new Dictionary<string, double>();
            Hashtable dtPayMode = new Hashtable();
            FarPoint.Web.Spread.TextCellType txtroll = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.TextCellType txtreg = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.TextCellType txtadd = new FarPoint.Web.Spread.TextCellType();
            for (int batch = 0; batch < cbl_batch.Items.Count; batch++)
            {
                if (cbl_batch.Items[batch].Selected == true)
                {
                    for (int Bnch = 0; Bnch < cbl_dept.Items.Count; Bnch++)
                    {
                        if (cbl_dept.Items[Bnch].Selected == true)
                        {
                            ds.Tables[0].DefaultView.RowFilter = "degree_code='" + cbl_dept.Items[Bnch].Value + "' and Batch_Year='" + Convert.ToString(cbl_batch.Items[batch].Text) + "'";
                            DataView dv2 = ds.Tables[0].DefaultView;
                            if (dv2.Count > 0)
                            {
                                booldegree = true;
                                //spreadDet.Sheets[0].RowCount++;
                                //spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(cbl_dept.Items[Bnch].Text) + " - " + Convert.ToString(cbl_batch.Items[batch].Text);
                                //spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                //spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                //spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].BackColor = Color.LightGreen;
                                //spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, spreadDet.Sheets[0].ColumnCount - 1);
                                for (int row = 0; row < dv2.Count; row++)
                                {
                                    string app_no = Convert.ToString(dv2[row]["App_No"]);
                                    ds.Tables[1].DefaultView.RowFilter = "App_No=" + Convert.ToString(app_no) + " and degree_code='" + cbl_dept.Items[Bnch].Value + "'";
                                    DataView dv = ds.Tables[1].DefaultView;
                                    if (dv.Count > 0)
                                    {
                                        for (int drow = 0; drow < dv.Count; drow++)
                                        {
                                            sno++;
                                            double totalAmount = 0;
                                            string Feecategory = Convert.ToString(dv[drow]["FeeCategory"]);
                                            spreadDet.Sheets[0].RowCount++;
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dv[drow]["Stud_Name"]);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dv[drow]["Roll_No"]);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv[drow]["Reg_No"]);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dv[drow]["roll_admit"]);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].CellType = txtroll;
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].CellType = txtreg;
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].CellType = txtadd;
                                            DataView Dview = new DataView();
                                            string Degreename = "";
                                            if (ds.Tables[3].Rows.Count > 0)
                                            {
                                                ds.Tables[3].DefaultView.RowFilter = "Degree_code='" + Convert.ToString(dv[drow]["Degree_code"]) + "'";
                                                Dview = ds.Tables[3].DefaultView;
                                                if (Dview.Count > 0)
                                                    Degreename = Convert.ToString(Dview[0]["degreename"]);
                                            }
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 5].Text = Degreename;
                                            string TextName = "";
                                            if (ds.Tables[4].Rows.Count > 0)
                                            {
                                                ds.Tables[4].DefaultView.RowFilter = "TextCode='" + Feecategory + "'";
                                                Dview = ds.Tables[4].DefaultView;
                                                if (Dview.Count > 0)
                                                    TextName = Convert.ToString(Dview[0]["TextVal"]);
                                            }
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 6].Text = TextName;
                                            if (ddlfeetype.SelectedItem.Text == "Paid")
                                            {
                                                string curSem = Convert.ToString(dv[drow]["current_semester"]);
                                                //color change due list 
                                                string feechec = TextName.Split(' ')[0];
                                                if (Convert.ToInt32(curSem) > Convert.ToInt32(feechec))
                                                {
                                                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].BackColor = Color.YellowGreen;
                                                }
                                            }
                                            if (ddlacctype.SelectedItem.Text != "Ledger")
                                            {
                                                for (int gphd = 0; gphd < cblheader.Items.Count; gphd++)
                                                {
                                                    if (cblheader.Items[gphd].Selected == true)
                                                    {
                                                        if (ddlfeetype.SelectedItem.Text == "Paid")
                                                        {
                                                            #region paid
                                                            divlabl.Visible = true;
                                                            spreadDet.Sheets[0].Columns[6].Visible = true;
                                                            spreadDet.Sheets[0].Columns[7].Visible = true;
                                                            ds.Tables[2].DefaultView.RowFilter = "App_no='" + app_no + "' and FeeCategory='" + Feecategory + "' and ChlGroupHeader='" + Convert.ToString(cblheader.Items[gphd].Text) + "'";
                                                            DataView dvpaid = ds.Tables[2].DefaultView;
                                                            string hdName = Convert.ToString(cblheader.Items[gphd].Text);
                                                            int colCnt = 0;
                                                            int.TryParse(Convert.ToString(htColCnt[hdName]), out colCnt);
                                                            if (dvpaid.Count > 0)
                                                            {
                                                                if (booldegree && cbdeptName.Checked)
                                                                {
                                                                    spreadDet.Sheets[0].RowCount++;
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(cbl_dept.Items[Bnch].Text) + " - " + Convert.ToString(cbl_batch.Items[batch].Text);
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                                                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].BackColor = Color.LightGreen;
                                                                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, spreadDet.Sheets[0].ColumnCount - 1);
                                                                    booldegree = false;
                                                                }
                                                                for (int k = 0; k < dvpaid.Count; k++)
                                                                {
                                                                    #region receipt and challan no
                                                                    string receiptno = Convert.ToString(dvpaid[k]["Transcode"]);
                                                                    string challanno = Convert.ToString(dvpaid[k]["DDno"]);
                                                                    if (rightscode == 1)
                                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 7].Text = receiptno;
                                                                    if (rightscode == 2 || rightscode == 0)
                                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 8].Text = challanno;   // if (rightscode == 3)
                                                                    if (rightscode == 3)
                                                                    {
                                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 7].Text = receiptno;
                                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 8].Text = challanno;   // if (rightscode == 3)
                                                                    }
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(dvpaid[k]["FinYear"]);
                                                                    #endregion
                                                                    double paidAmount = 0;
                                                                    double.TryParse(Convert.ToString(dvpaid[k]["Paid"]), out paidAmount);
                                                                    totalAmount += paidAmount;
                                                                    height += 10;
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = Convert.ToString(paidAmount);
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Right;
                                                                    if (!GrandTotal.ContainsKey(colCnt))
                                                                        GrandTotal.Add(colCnt, paidAmount);
                                                                    else
                                                                    {
                                                                        double total = 0;
                                                                        double.TryParse(Convert.ToString(GrandTotal[colCnt]), out total);
                                                                        total += paidAmount;
                                                                        GrandTotal.Remove(colCnt);
                                                                        GrandTotal.Add(colCnt, total);
                                                                    }
                                                                    #region Paymode value set
                                                                    string cursem = Convert.ToString(dvpaid[k]["Current_Semester"]);
                                                                    string paymode = Convert.ToString(dvpaid[k]["paymode"]);
                                                                    if (paidAmount != 0)
                                                                    {
                                                                        if (Linkvalue == "0")
                                                                        {
                                                                            if (diccurarrfee.ContainsKey(Feecategory + "$" + cursem))
                                                                            {
                                                                                Double getval = paidAmount + diccurarrfee[Feecategory + "$" + cursem];
                                                                                diccurarrfee[Feecategory + "$" + cursem] = getval;
                                                                            }
                                                                            else
                                                                                diccurarrfee.Add(Feecategory + "$" + cursem, paidAmount);
                                                                        }
                                                                        else
                                                                        {
                                                                            string valuenew = returnYearforSem(cursem);
                                                                            if (diccurarrfee.ContainsKey(Feecategory + "$" + valuenew.ToString()))
                                                                            {
                                                                                Double getval = paidAmount + diccurarrfee[Feecategory + "$" + valuenew.ToString()];
                                                                                diccurarrfee[Feecategory + "$" + valuenew.ToString()] = getval;
                                                                            }
                                                                            else
                                                                                diccurarrfee.Add(Feecategory + "$" + valuenew.ToString(), paidAmount);
                                                                        }
                                                                        if (paymode == "1")
                                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].BackColor = Color.LightCoral;
                                                                        else if (paymode == "2")
                                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].BackColor = Color.LightGray;
                                                                        else if (paymode == "3")
                                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].BackColor = Color.Orange;
                                                                        else if (paymode == "4")
                                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].BackColor = Color.LightGreen;
                                                                        else if (paymode == "5")
                                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].BackColor = Color.LightGoldenrodYellow;

                                                                        if (!dtPayMode.ContainsKey(paymode))
                                                                            dtPayMode.Add(paymode, paidAmount);
                                                                        else
                                                                        {
                                                                            double total = 0;
                                                                            double.TryParse(Convert.ToString(dtPayMode[paymode]), out total);
                                                                            total += paidAmount;
                                                                            dtPayMode.Remove(paymode);
                                                                            dtPayMode.Add(paymode, total);
                                                                        }
                                                                        checkStatus = true;
                                                                    }

                                                                    #endregion
                                                                }
                                                            }
                                                            else
                                                            {
                                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = "-";
                                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Center;
                                                            }
                                                            #endregion
                                                        }
                                                        else
                                                        {
                                                            #region yet to be paid
                                                            string strhdName = string.Empty;
                                                            string hdName = string.Empty;
                                                            if (ddlacctype.SelectedItem.Text == "Group Header")
                                                            {
                                                                strhdName = "and ChlGroupHeader='" + Convert.ToString(cblheader.Items[gphd].Text) + "'";
                                                                hdName = Convert.ToString(cblheader.Items[gphd].Text);
                                                            }
                                                            else
                                                            {
                                                                strhdName = "and headerfk='" + Convert.ToString(cblheader.Items[gphd].Value) + "'";
                                                                hdName = Convert.ToString(cblheader.Items[gphd].Value);
                                                            }



                                                            ds.Tables[2].DefaultView.RowFilter = "App_no='" + app_no + "' and FeeCategory='" + Feecategory + "' " + strhdName + "";
                                                            DataView dvpaid = ds.Tables[2].DefaultView;

                                                            int colCnt = 0;
                                                            int.TryParse(Convert.ToString(htColCnt[hdName]), out colCnt);
                                                            if (dvpaid.Count > 0)
                                                            {
                                                                if (booldegree && cbdgreename.Checked)
                                                                {
                                                                    spreadDet.Sheets[0].RowCount++;
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(cbl_dept.Items[Bnch].Text) + " - " + Convert.ToString(cbl_batch.Items[batch].Text);
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                                                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].BackColor = Color.LightGreen;
                                                                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, spreadDet.Sheets[0].ColumnCount - 1);
                                                                    booldegree = false;
                                                                }
                                                                for (int k = 0; k < dvpaid.Count; k++)
                                                                {

                                                                    //column 6 visible false
                                                                    spreadDet.Sheets[0].Columns[7].Visible = false;
                                                                    spreadDet.Sheets[0].Columns[8].Visible = false;
                                                                    double paidAmount = 0;
                                                                    double.TryParse(Convert.ToString(dvpaid[k]["Balance"]), out paidAmount);
                                                                    totalAmount += paidAmount;
                                                                    height += 10;
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = Convert.ToString(paidAmount);
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(dvpaid[k]["FinYear"]);
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Right;


                                                                    if (k < (dvpaid.Count - 1))
                                                                    {
                                                                        sno++;
                                                                        spreadDet.Sheets[0].RowCount++;
                                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dv[drow]["Stud_Name"]);
                                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dv[drow]["Roll_No"]);
                                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv[drow]["Reg_No"]);
                                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dv[drow]["roll_admit"]);
                                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].CellType = txtroll;
                                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].CellType = txtreg;
                                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].CellType = txtadd;
                                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 5].Text = Degreename;
                                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 6].Text = TextName;

                                                                        if (!GrandTotal.ContainsKey(colCnt))
                                                                            GrandTotal.Add(colCnt, paidAmount);
                                                                        else
                                                                        {
                                                                            double total = 0;
                                                                            double.TryParse(Convert.ToString(GrandTotal[colCnt]), out total);
                                                                            total += paidAmount;
                                                                            GrandTotal.Remove(colCnt);
                                                                            GrandTotal.Add(colCnt, total);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (!GrandTotal.ContainsKey(colCnt))
                                                                            GrandTotal.Add(colCnt, paidAmount);
                                                                        else
                                                                        {
                                                                            double total = 0;
                                                                            double.TryParse(Convert.ToString(GrandTotal[colCnt]), out total);
                                                                            total += paidAmount;
                                                                            GrandTotal.Remove(colCnt);
                                                                            GrandTotal.Add(colCnt, total);
                                                                        }
                                                                        checkStatus = true;
                                                                    }

                                                                }
                                                            }
                                                            else
                                                            {
                                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = "-";
                                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Center;
                                                            }
                                                            #endregion
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                for (int remv = 0; remv < treeledger.Nodes.Count; remv++)
                                                {
                                                    if (treeledger.Nodes[remv].ChildNodes.Count > 0)
                                                    {
                                                        double tempTotalAmount = 0;
                                                        string ledgerStrHd = string.Empty;
                                                        for (int j = 0; j < treeledger.Nodes[remv].ChildNodes.Count; j++)
                                                        {
                                                            if (treeledger.Nodes[remv].ChildNodes[j].Checked == true)
                                                            {
                                                                #region ledger yet to be paid
                                                                string strhdName = string.Empty;
                                                                string hdName = string.Empty;
                                                                strhdName = " and ledgerfk='" + Convert.ToString(treeledger.Nodes[remv].ChildNodes[j].Value) + "'";
                                                                //ledgerStrHd = treeledger.Nodes[remv].ChildNodes[j].Text;
                                                                ledgerStrHd = treeledger.Nodes[remv].ChildNodes[j].Value;
                                                                hdName = Convert.ToString(treeledger.Nodes[remv].ChildNodes[j].Value);

                                                                ds.Tables[2].DefaultView.RowFilter = "App_no='" + app_no + "' and FeeCategory='" + Feecategory + "' " + strhdName + "";
                                                                DataView dvpaid = ds.Tables[2].DefaultView;

                                                                int colCnt = 0;
                                                                int.TryParse(Convert.ToString(htColCnt[hdName]), out colCnt);
                                                                if (dvpaid.Count > 0)
                                                                {
                                                                    if (booldegree && cbdgreename.Checked)
                                                                    {
                                                                        spreadDet.Sheets[0].RowCount++;
                                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(cbl_dept.Items[Bnch].Text) + " - " + Convert.ToString(cbl_batch.Items[batch].Text);
                                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                                                        spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].BackColor = Color.LightGreen;
                                                                        spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, spreadDet.Sheets[0].ColumnCount - 1);
                                                                        booldegree = false;
                                                                    }


                                                                    for (int k = 0; k < dvpaid.Count; k++)
                                                                    {
                                                                        //column 6 visible false
                                                                        spreadDet.Sheets[0].Columns[7].Visible = false;
                                                                        spreadDet.Sheets[0].Columns[8].Visible = false;
                                                                        double paidAmount = 0;
                                                                        double.TryParse(Convert.ToString(dvpaid[k]["Balance"]), out paidAmount);
                                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(dvpaid[k]["FinYear"]);
                                                                        totalAmount += paidAmount;
                                                                        tempTotalAmount += paidAmount;
                                                                        height += 10;
                                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = Convert.ToString(paidAmount);
                                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Right;
                                                                        //if (!GrandTotal.ContainsKey(colCnt))
                                                                        //    GrandTotal.Add(colCnt, paidAmount);
                                                                        //else
                                                                        //{
                                                                        //    double total = 0;
                                                                        //    double.TryParse(Convert.ToString(GrandTotal[colCnt]), out total);
                                                                        //    total += paidAmount;
                                                                        //    GrandTotal.Remove(colCnt);
                                                                        //    GrandTotal.Add(colCnt, total);
                                                                        //}
                                                                        //checkStatus = true;

                                                                        if (k < (dvpaid.Count - 1))
                                                                        {
                                                                            sno++;
                                                                            spreadDet.Sheets[0].RowCount++;
                                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dv[drow]["Stud_Name"]);
                                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dv[drow]["Roll_No"]);
                                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv[drow]["Reg_No"]);
                                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dv[drow]["roll_admit"]);
                                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].CellType = txtroll;
                                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].CellType = txtreg;
                                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].CellType = txtadd;
                                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 5].Text = Degreename;
                                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 6].Text = TextName;

                                                                            if (!GrandTotal.ContainsKey(colCnt))
                                                                                GrandTotal.Add(colCnt, paidAmount);
                                                                            else
                                                                            {
                                                                                double total = 0;
                                                                                double.TryParse(Convert.ToString(GrandTotal[colCnt]), out total);
                                                                                total += paidAmount;
                                                                                GrandTotal.Remove(colCnt);
                                                                                GrandTotal.Add(colCnt, total);
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            if (!GrandTotal.ContainsKey(colCnt))
                                                                                GrandTotal.Add(colCnt, paidAmount);
                                                                            else
                                                                            {
                                                                                double total = 0;
                                                                                double.TryParse(Convert.ToString(GrandTotal[colCnt]), out total);
                                                                                total += paidAmount;
                                                                                GrandTotal.Remove(colCnt);
                                                                                GrandTotal.Add(colCnt, total);
                                                                            }
                                                                            checkStatus = true;
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = "-";
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Center;
                                                                }
                                                                #endregion
                                                            }
                                                        }
                                                        //every ledger totalamount
                                                        if (tempTotalAmount != 0)
                                                        {
                                                            #region
                                                            int colCnt = 0;
                                                            int.TryParse(Convert.ToString(htColCnt[ledgerStrHd + " TotalAmount"]), out colCnt);

                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = Convert.ToString(tempTotalAmount);
                                                            //Total += Convert.ToDouble(dv1[0]["Balance"]);
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Right;
                                                            if (!GrandTotal.ContainsKey(colCnt))
                                                                GrandTotal.Add(colCnt, Convert.ToString(tempTotalAmount));
                                                            else
                                                            {
                                                                double total = 0;
                                                                double.TryParse(Convert.ToString(GrandTotal[colCnt]), out total);
                                                                total += tempTotalAmount;
                                                                GrandTotal.Remove(colCnt);
                                                                GrandTotal.Add(colCnt, Convert.ToString(total));
                                                            }
                                                            #endregion
                                                        }
                                                    }
                                                }
                                            }
                                            if (totalAmount != 0)
                                            {
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(totalAmount);
                                                //Total += Convert.ToDouble(dv1[0]["Balance"]);
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                                if (!GrandTotal.ContainsKey(spreadDet.Sheets[0].ColumnCount - 1))
                                                    GrandTotal.Add(spreadDet.Sheets[0].ColumnCount - 1, Convert.ToString(totalAmount));
                                                else
                                                {
                                                    double total = 0;
                                                    double.TryParse(Convert.ToString(GrandTotal[spreadDet.Sheets[0].ColumnCount - 1]), out total);
                                                    total += totalAmount;
                                                    GrandTotal.Remove(spreadDet.Sheets[0].ColumnCount - 1);
                                                    GrandTotal.Add(spreadDet.Sheets[0].ColumnCount - 1, Convert.ToString(total));
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            int colcnt = 0;
            if (checkStatus)
            {
                //  spreadDet.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
                spreadDet.Sheets[0].RowCount++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString("Grand Total");
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].BackColor = Color.LightSeaGreen;
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].Font.Bold = true;
                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, 6);
                for (int j = 9; j < spreadDet.Sheets[0].ColumnCount; j++)
                {
                    if (colcnt == 0)
                        colcnt = j;
                    double grandvalue = 0;
                    double.TryParse(Convert.ToString(GrandTotal[j]), out grandvalue);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalue);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, j].Font.Bold = true;
                }
                if (cbabstract.Checked)
                {
                    #region abstract

                    //abstract
                    double totalPaid = 0;
                    spreadDet.Sheets[0].RowCount++;// = FpSpread1.Sheets[0].RowCount + 2;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = "ABSTRACT";
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].BackColor = Color.Brown;
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].ForeColor = Color.White;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, 7);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    for (int ps = 0; ps < chkl_paid.Items.Count; ps++)
                    {
                        if (chkl_paid.Items[ps].Selected == true)
                        {
                            string strptype = chkl_paid.Items[ps].Text.ToString();
                            spreadDet.Sheets[0].RowCount++;
                            spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, 7);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = strptype;
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                            double grandPaid = 0;
                            //  if (dtPayMode.ContainsKey(Convert.ToString(dtPayMode[chkl_paid.Items[ps].Value])))
                            double.TryParse(Convert.ToString(dtPayMode[chkl_paid.Items[ps].Value]), out grandPaid);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, colcnt].Text = Convert.ToString(grandPaid); totalPaid += grandPaid;
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcnt].HorizontalAlign = HorizontalAlign.Right;
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcnt].Font.Bold = true;
                        }
                    }
                    spreadDet.Sheets[0].RowCount++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = "Total";
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, 7);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcnt].Text = Convert.ToString(totalPaid);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcnt].HorizontalAlign = HorizontalAlign.Right;
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
                            for (int rcs = 0; rcs < cbl_sem.Items.Count; rcs++)
                            {
                                if (cbl_sem.Items[rcs].Value.ToString() == spt[0].ToString())
                                {
                                    string feeval = cbl_sem.Items[rcs].Text.ToString();
                                    string[] stc = feeval.Split(' ');
                                    if (stc.GetUpperBound(0) >= 0)
                                    {
                                        if (stc[0].ToString().Trim() == spt[1].ToString().Trim())
                                            currfee = currfee + Convert.ToDouble(getfeeamount);
                                        else
                                            arrfee = arrfee + Convert.ToDouble(getfeeamount);
                                    }
                                    else
                                    {
                                        if (cbl_sem.Items[rcs].Text.Contains(spt[1].ToString()))
                                            currfee = currfee + Convert.ToDouble(getfeeamount);
                                        else
                                            arrfee = arrfee + Convert.ToDouble(getfeeamount);
                                    }
                                    rcs = cbl_sem.Items.Count;
                                }
                            }
                        }
                    }
                    spreadDet.Sheets[0].RowCount++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = "REGULAR";
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, 7);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcnt].Text = currfee.ToString();
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcnt].HorizontalAlign = HorizontalAlign.Right;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcnt].Font.Bold = true;
                    spreadDet.Sheets[0].RowCount++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = "ARREAR";
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].BackColor = Color.YellowGreen;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, 7);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcnt].Text = arrfee.ToString();
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcnt].HorizontalAlign = HorizontalAlign.Right;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcnt].Font.Bold = true;
                    #endregion
                }
            }
            spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
            spreadDet.SaveChanges();
            spreadDet.Height = height;
            spreadDet.Visible = true;
            print.Visible = true;
            //divlabl.Visible = true;
            #endregion
        }
        catch { }
    }
    protected void groupHdBoth(DataSet ds)
    {
        try
        {
            #region value
            challanAndReceiptNoRights();
            Hashtable htColCnt = new Hashtable();
            RollAndRegSettings();
            loadspreadDetails(ref  htColCnt);
            spreadColumnVisible();
            string Linkvalue = ArrearLinkValue();
            int sno = 0;
            int height = 0;
            bool checkStatus = false;
            Hashtable GrandTotal = new Hashtable();
            Hashtable htGrandTotal = new Hashtable();
            Dictionary<string, Double> diccurarrfee = new Dictionary<string, double>();
            Hashtable dtPayMode = new Hashtable();
            FarPoint.Web.Spread.TextCellType txtroll = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.TextCellType txtreg = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.TextCellType txtadd = new FarPoint.Web.Spread.TextCellType();
            for (int batch = 0; batch < cbl_batch.Items.Count; batch++)
            {
                if (cbl_batch.Items[batch].Selected == true)
                {
                    for (int Bnch = 0; Bnch < cbl_dept.Items.Count; Bnch++)
                    {
                        if (cbl_dept.Items[Bnch].Selected == true)
                        {
                            ds.Tables[0].DefaultView.RowFilter = "degree_code='" + cbl_dept.Items[Bnch].Value + "' and Batch_Year='" + Convert.ToString(cbl_batch.Items[batch].Text) + "'";
                            DataView dv2 = ds.Tables[0].DefaultView;
                            if (dv2.Count > 0)
                            {
                                if (cbdgreename.Checked)
                                {
                                    spreadDet.Sheets[0].RowCount++;
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(cbl_dept.Items[Bnch].Text) + " - " + Convert.ToString(cbl_batch.Items[batch].Text);
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].BackColor = Color.LightGreen;
                                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, spreadDet.Sheets[0].ColumnCount - 1);
                                }
                                for (int row = 0; row < dv2.Count; row++)
                                {
                                    string app_no = Convert.ToString(dv2[row]["App_No"]);
                                    ds.Tables[1].DefaultView.RowFilter = "App_No=" + Convert.ToString(app_no) + " and degree_code='" + cbl_dept.Items[Bnch].Value + "'";
                                    DataView dv = ds.Tables[1].DefaultView;
                                    if (dv.Count > 0)
                                    {
                                        for (int drow = 0; drow < dv.Count; drow++)
                                        {
                                            sno++;
                                            double totalAmount = 0;
                                            double totalPaidAmt = 0;
                                            double totalBalAmt = 0;
                                            double totalExcAmt = 0;
                                            string Feecategory = Convert.ToString(dv[drow]["FeeCategory"]);
                                            spreadDet.Sheets[0].RowCount++;
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dv[drow]["Stud_Name"]);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dv[drow]["Roll_No"]);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv[drow]["Reg_No"]);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dv[drow]["roll_admit"]);
                                            if ("152ch124" == Convert.ToString(dv[drow]["Reg_No"]))
                                            {
                                            }
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].CellType = txtroll;
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].CellType = txtreg;
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].CellType = txtadd;
                                            DataView Dview = new DataView();
                                            string Degreename = "";
                                            if (ds.Tables[4].Rows.Count > 0)
                                            {
                                                ds.Tables[4].DefaultView.RowFilter = "Degree_code='" + Convert.ToString(dv[drow]["Degree_code"]) + "'";
                                                Dview = ds.Tables[4].DefaultView;
                                                if (Dview.Count > 0)
                                                    Degreename = Convert.ToString(Dview[0]["degreename"]);
                                            }
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 5].Text = Degreename;
                                            string TextName = "";
                                            if (ds.Tables[5].Rows.Count > 0)
                                            {
                                                ds.Tables[5].DefaultView.RowFilter = "TextCode='" + Feecategory + "'";
                                                Dview = ds.Tables[5].DefaultView;
                                                if (Dview.Count > 0)
                                                    TextName = Convert.ToString(Dview[0]["TextVal"]);
                                            }
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 6].Text = TextName;
                                            if (ddlfeetype.SelectedItem.Text == "Paid")
                                            {
                                                string curSem = Convert.ToString(dv[drow]["current_semester"]);
                                                //color change due list 
                                                string feechec = TextName.Split(' ')[0];
                                                if (Convert.ToInt32(curSem) > Convert.ToInt32(feechec))
                                                {
                                                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].BackColor = Color.YellowGreen;
                                                }
                                            }
                                            if (ddlacctype.SelectedItem.Text != "Ledger")
                                            {
                                                for (int gphd = 0; gphd < cblheader.Items.Count; gphd++)
                                                {
                                                    if (cblheader.Items[gphd].Selected == true)
                                                    {
                                                        #region paid
                                                        string strhdName = string.Empty;
                                                        string hdName = string.Empty;
                                                        if (ddlacctype.SelectedItem.Text == "Group Header")
                                                        {
                                                            strhdName = "and ChlGroupHeader='" + Convert.ToString(cblheader.Items[gphd].Text) + "'";
                                                            hdName = Convert.ToString(cblheader.Items[gphd].Text);
                                                        }
                                                        else
                                                        {
                                                            strhdName = "and headerfk='" + Convert.ToString(cblheader.Items[gphd].Value) + "'";
                                                            hdName = Convert.ToString(cblheader.Items[gphd].Value);
                                                        }

                                                        spreadDet.Sheets[0].Columns[7].Visible = false;
                                                        spreadDet.Sheets[0].Columns[8].Visible = false;
                                                        ds.Tables[2].DefaultView.RowFilter = "App_no='" + app_no + "' and FeeCategory='" + Feecategory + "' " + strhdName + "";
                                                        DataView dvpaid = ds.Tables[2].DefaultView;
                                                        int colCnt = 0;
                                                        double totalamount = 0;
                                                        double paidAmount = 0;
                                                        double balanceamt = 0;
                                                        int.TryParse(Convert.ToString(htColCnt[hdName]), out colCnt);
                                                        if (dvpaid.Count > 0)
                                                        {
                                                            double.TryParse(Convert.ToString(dvpaid[0]["Total"]), out totalamount);
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(dvpaid[0]["FinYear"]);
                                                            ds.Tables[3].DefaultView.RowFilter = "App_no='" + app_no + "' and FeeCategory='" + Feecategory + "' " + strhdName + " and FinYear='" + Convert.ToString(dvpaid[0]["FinYear"]) + "'";
                                                            DataView dvdailpaid = ds.Tables[3].DefaultView;
                                                            if (dvdailpaid.Count > 0)
                                                                double.TryParse(Convert.ToString(dvdailpaid[0]["TotalPaid"]), out paidAmount);
                                                            // double.TryParse(Convert.ToString(dvpaid[0]["Paid"]), out paidAmount);
                                                            balanceamt = totalamount - paidAmount;
                                                            totalAmount += totalamount;
                                                            totalPaidAmt += paidAmount;
                                                            totalBalAmt += balanceamt;
                                                            height += 10;
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = Convert.ToString(totalamount);

                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Right;
                                                            if (!GrandTotal.ContainsKey(colCnt))
                                                                GrandTotal.Add(colCnt, totalamount);
                                                            else
                                                            {
                                                                double total = 0;
                                                                double.TryParse(Convert.ToString(GrandTotal[colCnt]), out total);
                                                                total += totalamount;
                                                                GrandTotal.Remove(colCnt);
                                                                GrandTotal.Add(colCnt, total);
                                                            }
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, ++colCnt].Text = Convert.ToString(paidAmount);
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Right;
                                                            if (!GrandTotal.ContainsKey(colCnt))
                                                                GrandTotal.Add(colCnt, paidAmount);
                                                            else
                                                            {
                                                                double total = 0;
                                                                double.TryParse(Convert.ToString(GrandTotal[colCnt]), out total);
                                                                total += paidAmount;
                                                                GrandTotal.Remove(colCnt);
                                                                GrandTotal.Add(colCnt, total);
                                                            }
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, ++colCnt].Text = Convert.ToString(balanceamt);
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Right;
                                                            if (!GrandTotal.ContainsKey(colCnt))
                                                                GrandTotal.Add(colCnt, balanceamt);
                                                            else
                                                            {
                                                                double total = 0;
                                                                double.TryParse(Convert.ToString(GrandTotal[colCnt]), out total);
                                                                total += balanceamt;
                                                                GrandTotal.Remove(colCnt);
                                                                GrandTotal.Add(colCnt, total);
                                                            }
                                                            checkStatus = true;

                                                        }
                                                        else
                                                        {
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = "-";
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Center;
                                                        }
                                                        #endregion
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                for (int remv = 0; remv < treeledger.Nodes.Count; remv++)
                                                {
                                                    if (treeledger.Nodes[remv].ChildNodes.Count > 0)
                                                    {
                                                        for (int j = 0; j < treeledger.Nodes[remv].ChildNodes.Count; j++)
                                                        {
                                                            if (treeledger.Nodes[remv].ChildNodes[j].Checked == true)
                                                            {
                                                                #region both
                                                                string strhdName = string.Empty;
                                                                string hdName = string.Empty;
                                                                strhdName = "and ledgerfk='" + Convert.ToString(treeledger.Nodes[remv].ChildNodes[j].Value) + "'";
                                                                hdName = Convert.ToString(treeledger.Nodes[remv].ChildNodes[j].Value);

                                                                spreadDet.Sheets[0].Columns[7].Visible = false;
                                                                spreadDet.Sheets[0].Columns[8].Visible = false;
                                                                ds.Tables[2].DefaultView.RowFilter = "App_no='" + app_no + "' and FeeCategory='" + Feecategory + "' " + strhdName + "";
                                                                DataView dvpaid = ds.Tables[2].DefaultView;
                                                                int colCnt = 0;
                                                                double totalamount = 0;
                                                                double paidAmount = 0;
                                                                double balanceamt = 0;
                                                                int.TryParse(Convert.ToString(htColCnt[hdName]), out colCnt);
                                                                if (dvpaid.Count > 0)
                                                                {
                                                                    double.TryParse(Convert.ToString(dvpaid[0]["Total"]), out totalamount);
                                                                    ds.Tables[3].DefaultView.RowFilter = "App_no='" + app_no + "' and FeeCategory='" + Feecategory + "' " + strhdName + "";
                                                                    DataView dvdailpaid = ds.Tables[3].DefaultView;
                                                                    if (dvdailpaid.Count > 0)
                                                                        double.TryParse(Convert.ToString(dvdailpaid[0]["TotalPaid"]), out paidAmount);
                                                                    // double.TryParse(Convert.ToString(dvpaid[0]["Paid"]), out paidAmount);
                                                                    balanceamt = totalamount - paidAmount;
                                                                    totalAmount += totalamount;
                                                                    totalPaidAmt += paidAmount;
                                                                    totalBalAmt += balanceamt;
                                                                    height += 10;
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(dvpaid[0]["FinYear"]);
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = Convert.ToString(totalamount);
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Right;
                                                                    if (!GrandTotal.ContainsKey(colCnt))
                                                                        GrandTotal.Add(colCnt, totalamount);
                                                                    else
                                                                    {
                                                                        double total = 0;
                                                                        double.TryParse(Convert.ToString(GrandTotal[colCnt]), out total);
                                                                        total += totalamount;
                                                                        GrandTotal.Remove(colCnt);
                                                                        GrandTotal.Add(colCnt, total);
                                                                    }
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, ++colCnt].Text = Convert.ToString(paidAmount);
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Right;
                                                                    if (!GrandTotal.ContainsKey(colCnt))
                                                                        GrandTotal.Add(colCnt, paidAmount);
                                                                    else
                                                                    {
                                                                        double total = 0;
                                                                        double.TryParse(Convert.ToString(GrandTotal[colCnt]), out total);
                                                                        total += paidAmount;
                                                                        GrandTotal.Remove(colCnt);
                                                                        GrandTotal.Add(colCnt, total);
                                                                    }
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, ++colCnt].Text = Convert.ToString(balanceamt);
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Right;
                                                                    if (!GrandTotal.ContainsKey(colCnt))
                                                                        GrandTotal.Add(colCnt, balanceamt);
                                                                    else
                                                                    {
                                                                        double total = 0;
                                                                        double.TryParse(Convert.ToString(GrandTotal[colCnt]), out total);
                                                                        total += balanceamt;
                                                                        GrandTotal.Remove(colCnt);
                                                                        GrandTotal.Add(colCnt, total);
                                                                    }
                                                                    checkStatus = true;

                                                                }
                                                                else
                                                                {
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = "-";
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Center;
                                                                }
                                                                #endregion
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            if (totalAmount != 0)
                                            {
                                                #region toal allot,paid balance
                                                double excessAmount = 0;
                                                int colCnt = 0;
                                                int.TryParse(Convert.ToString(htColCnt["Total Allot"]), out colCnt);
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = Convert.ToString(totalAmount);
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Right;
                                                if (!GrandTotal.ContainsKey(colCnt))
                                                    GrandTotal.Add(colCnt, Convert.ToString(totalAmount));
                                                else
                                                {
                                                    double total = 0;
                                                    double.TryParse(Convert.ToString(GrandTotal[colCnt]), out total);
                                                    total += totalAmount;
                                                    GrandTotal.Remove(colCnt);
                                                    GrandTotal.Add(colCnt, Convert.ToString(total));
                                                }
                                                int.TryParse(Convert.ToString(htColCnt["Total Paid"]), out colCnt);
                                                if (totalPaidAmt > totalAmount)
                                                {
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = Convert.ToString(totalPaidAmt);
                                                    excessAmount = totalPaidAmt - totalAmount;
                                                }
                                                else
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = Convert.ToString(totalPaidAmt);
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Right;
                                                if (!GrandTotal.ContainsKey(colCnt))
                                                    GrandTotal.Add(colCnt, Convert.ToString(totalPaidAmt));
                                                else
                                                {
                                                    double total = 0;
                                                    double.TryParse(Convert.ToString(GrandTotal[colCnt]), out total);
                                                    total += totalPaidAmt;
                                                    GrandTotal.Remove(colCnt);
                                                    GrandTotal.Add(colCnt, Convert.ToString(total));
                                                }

                                                int.TryParse(Convert.ToString(htColCnt["Total Balance"]), out colCnt);
                                                if (totalBalAmt <= 0)
                                                {
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = Convert.ToString(0);
                                                    totalBalAmt = 0;
                                                }
                                                else
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = Convert.ToString(totalBalAmt);
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Right;
                                                if (!GrandTotal.ContainsKey(colCnt))
                                                    GrandTotal.Add(colCnt, Convert.ToString(totalBalAmt));
                                                else
                                                {
                                                    double total = 0;
                                                    double.TryParse(Convert.ToString(GrandTotal[colCnt]), out total);
                                                    total += totalBalAmt;
                                                    GrandTotal.Remove(colCnt);
                                                    GrandTotal.Add(colCnt, Convert.ToString(total));
                                                }
                                                int.TryParse(Convert.ToString(htColCnt["Excess Amount"]), out colCnt);
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text
                                                    = Convert.ToString(excessAmount);
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Right;
                                                if (!GrandTotal.ContainsKey(colCnt))
                                                    GrandTotal.Add(colCnt, Convert.ToString(excessAmount));
                                                else
                                                {
                                                    double total = 0;
                                                    double.TryParse(Convert.ToString(GrandTotal[colCnt]), out total);
                                                    total += excessAmount;
                                                    GrandTotal.Remove(colCnt);
                                                    GrandTotal.Add(colCnt, Convert.ToString(total));
                                                }
                                                double status = totalAmount - totalPaidAmt;
                                                if (status == 0 || excessAmount != 0)
                                                {
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].Text = "Paid";
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Left;
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].BackColor = ColorTranslator.FromHtml("#99ebff");
                                                }
                                                else
                                                {
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].Text = "UnPaid";
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Left;
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].BackColor = ColorTranslator.FromHtml("#ff6666");
                                                }
                                                #endregion
                                            }
                                        }
                                    }
                                }
                                #region total individual dept
                                if (cbdgreename.Checked)
                                {
                                    spreadDet.Sheets[0].RowCount++;
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString("Total");
                                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = ColorTranslator.FromHtml("#4870BE");
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                }
                                double getAmount = 0;
                                for (int j = 9; j < spreadDet.Sheets[0].ColumnCount; j++)
                                {
                                    getAmount = 0;
                                    double.TryParse(Convert.ToString(GrandTotal[j]), out getAmount);
                                    if (cbdgreename.Checked)
                                    {
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, j].Text = Convert.ToString(getAmount);
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, j].HorizontalAlign = HorizontalAlign.Right;
                                    }
                                    if (!htGrandTotal.ContainsKey(j))
                                        htGrandTotal.Add(j, getAmount);
                                    else
                                    {
                                        double total = 0;
                                        double.TryParse(Convert.ToString(htGrandTotal[j]), out total);
                                        total += getAmount;
                                        htGrandTotal.Remove(j);
                                        htGrandTotal.Add(j, Convert.ToString(total));
                                    }
                                }
                                GrandTotal.Clear();
                                #endregion
                            }
                        }
                    }
                }
            }
            int colcnt = 0;
            int tempcolCnt = 0;
            if (checkStatus)
            {
                double grandvalue = 0;
                spreadDet.Sheets[0].RowCount++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString("Grand Total");
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#FFBF00");
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, 6);
                for (int j = 9; j < spreadDet.Sheets[0].ColumnCount; j++)
                {
                    grandvalue = 0;
                    double.TryParse(Convert.ToString(htGrandTotal[j]), out grandvalue);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalue);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, j].Font.Bold = true;
                }
            }
            spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
            spreadDet.SaveChanges();
            //   spreadDet.Height = height;
            spreadDet.Visible = true;
            print.Visible = true;
            // divlabl.Visible = true;
            #endregion
        }
        catch { }
    }
    protected void headerPaid(DataSet ds)
    {
        try
        {
            challanAndReceiptNoRights();
            string deg_acr = string.Empty;
            string deg_dept = string.Empty;
            int height = 0;
            int sno = 0;
            int sclValue = 0;
            bool checkStatus = false;
            Hashtable htColCnt = new Hashtable();
            RollAndRegSettings();
            loadspreadDetails(ref  htColCnt);
            spreadColumnVisible();
            int.TryParse(Convert.ToString(sclSett()), out sclValue);
            Hashtable GrandTotal = new Hashtable();
            Hashtable htfnlTotal = new Hashtable();
            Hashtable dtPayMode = new Hashtable();
            string Linkvalue = ArrearLinkValue();
            Dictionary<string, Double> diccurarrfee = new Dictionary<string, double>();
            ArrayList ardegree = new ArrayList();
            string getdegreedetails = string.Empty;
            FarPoint.Web.Spread.TextCellType txtroll = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.TextCellType txtreg = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.TextCellType txtadd = new FarPoint.Web.Spread.TextCellType();
            for (int st = 0; st < ds.Tables[0].Rows.Count; st++)
            {
                ArrayList arfeecat = new ArrayList();
                for (int sem = 0; sem < cbl_sem.Items.Count; sem++)
                {
                    if (cbl_sem.Items[sem].Selected)
                    {
                        string feecat = " and feecategory='" + cbl_sem.Items[sem].Value + "'";
                        string feeVal = Convert.ToString(cbl_sem.Items[sem].Value);
                        string tempdegreedeatisl = ds.Tables[0].Rows[st]["batch_year"].ToString() + '-' + ds.Tables[0].Rows[st]["degree_code"].ToString() + '-' + ds.Tables[0].Rows[st]["Current_Semester"].ToString();
                        string courseid = ds.Tables[0].Rows[st]["degree_code"].ToString();
                        string batcyear = ds.Tables[0].Rows[st]["batch_year"].ToString();
                        string year = ds.Tables[0].Rows[st]["Current_Semester"].ToString();
                        ds.Tables[2].DefaultView.RowFilter = "degree_code='" + courseid + "'";
                        DataView dv_get_course = ds.Tables[2].DefaultView;
                        if (dv_get_course.Count > 0)
                        {
                            deg_acr = dv_get_course[0]["course_name"] + "-" + dv_get_course[0]["acronym"];
                            deg_dept = dv_get_course[0]["course_name"] + "-" + dv_get_course[0]["Dept_Name"];
                        }
                        string str = courseid + "/" + year;
                        if (!ardegree.Contains(str))
                        {
                            #region every degreewise total
                            if (GrandTotal.Count > 0)
                            {
                                if (cbdgreename.Checked)
                                {
                                    spreadDet.Sheets[0].RowCount++;
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = "Degree Wise Total";
                                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, 3);
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].BackColor = Color.YellowGreen;
                                }
                                for (int d = 9; d < spreadDet.Sheets[0].Columns.Count; d++)
                                {
                                    Double dict = 0;
                                    if (GrandTotal.ContainsKey(d))
                                        double.TryParse(Convert.ToString(GrandTotal[d]), out dict);
                                    if (cbdgreename.Checked)
                                    {
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, d].Text = Convert.ToString(dict);
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, d].Font.Bold = true;
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, d].Font.Size = FontUnit.Medium;
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, d].Font.Name = "Book Antiqua";
                                    }
                                    if (!htfnlTotal.ContainsKey(d))
                                        htfnlTotal.Add(d, dict);
                                    else
                                    {
                                        double total = 0;
                                        double.TryParse(Convert.ToString(htfnlTotal[d]), out total);
                                        total += dict;
                                        htfnlTotal.Remove(d);
                                        htfnlTotal.Add(d, total);
                                    }
                                }
                                GrandTotal.Clear();
                            }
                            ardegree.Add(str);
                            #endregion
                        }

                        //if (tempdegreedeatisl != getdegreedetails)
                        //{
                        //    booldegree = true;
                        //    spreadDet.Sheets[0].RowCount++;
                        //    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = batcyear + "-" + deg_dept + "/Sem-" + year;
                        //    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                        //    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, spreadDet.Sheets[0].Columns.Count);
                        //    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        //    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].BackColor = Color.LightSkyBlue;
                        //    //arfeecat.Add(feeVal);
                        //    getdegreedetails = tempdegreedeatisl;
                        //}

                        string name = Convert.ToString(ds.Tables[0].Rows[st]["Name"]);
                        string name_roll = Convert.ToString(ds.Tables[0].Rows[st]["Roll_No"] + "-" + ds.Tables[0].Rows[st]["Name"]);
                        string rollno = Convert.ToString(ds.Tables[0].Rows[st]["Roll_No"]);
                        string regno = Convert.ToString(ds.Tables[0].Rows[st]["Reg_No"]);
                        string rolladmit = Convert.ToString(ds.Tables[0].Rows[st]["roll_admit"]);
                        string FinYear = string.Empty;
                        string receiptno = string.Empty;
                        string challanno = string.Empty;
                        if (rollno.Trim() == "")
                            rollno = rolladmit;

                        double totalAmount = 0;
                        bool addRow = false;
                        //for (int fin = 0; fin < chklsfyear.Items.Count; fin++)
                        //{
                        //    if (chklsfyear.Items[fin].Selected)
                        //    {
                        if (ddlacctype.SelectedItem.Text == "Header")
                        {
                            for (int hd = 0; hd < cblheader.Items.Count; hd++)
                            {
                                if (cblheader.Items[hd].Selected)
                                {
                                    #region header
                                    divlabl.Visible = true;
                                    ds.Tables[1].DefaultView.RowFilter = "roll_no='" + rollno + "' " + feecat + " and headerfk='" + cblheader.Items[hd].Value + "' ";//and actualfinyearfk='"+chklsfyear.Items[fin].Value+"'
                                    DataView dvpaid = ds.Tables[1].DefaultView;
                                    int colCnt = 0;
                                    int.TryParse(Convert.ToString(htColCnt[cblheader.Items[hd].Value]), out colCnt);
                                    if (dvpaid.Count > 0)
                                    {
                                        if (tempdegreedeatisl != getdegreedetails && cbdeptName.Checked)
                                        {
                                            spreadDet.Sheets[0].RowCount++;
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = batcyear + "-" + deg_dept + "/Sem-" + year;
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                            spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, spreadDet.Sheets[0].Columns.Count);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].BackColor = Color.LightSkyBlue;
                                            //arfeecat.Add(feeVal);
                                            getdegreedetails = tempdegreedeatisl;
                                        }
                                        for (int pd = 0; pd < dvpaid.Count; pd++)
                                        {
                                            if (!addRow)
                                            {
                                                sno++;
                                                spreadDet.Sheets[0].RowCount++;
                                            }
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = name;
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = rollno;
                                            // if (sclflag == false)
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].Text = regno;
                                            // else
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].Text = rolladmit;
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].CellType = txtroll;
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].CellType = txtreg;
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].CellType = txtadd;
                                            // spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].CellType = txtreg;
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 5].Text = deg_acr;
                                            // if (feecateflag == true)
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 6].Text = cbl_sem.Items[sem].Text;
                                            // else
                                            // spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 6].Text = year;
                                            receiptno = Convert.ToString(dvpaid[pd]["ReceiptNo"]);
                                            challanno = Convert.ToString(dvpaid[pd]["challan_no"]);
                                            //  feecat = Convert.ToString(dvpaid[pd]["FeeCategory"]);
                                            string cursem = Convert.ToString(dvpaid[pd]["current_semester"]);
                                            string paymode = Convert.ToString(dvpaid[pd]["mode"]);
                                            string feechec = string.Empty;
                                            FinYear = Convert.ToString(dvpaid[pd]["FinYear"]);
                                            //due color change
                                            if (sclValue == 0)
                                                feechec = cbl_sem.Items[sem].Text.Split(' ')[1];
                                            else
                                                feechec = cbl_sem.Items[sem].Text.Split(' ')[0];
                                            if (Convert.ToInt32(cursem) > Convert.ToInt32(feechec))
                                                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].BackColor = Color.YellowGreen;
                                            if (rightscode == 1)
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 7].Text = receiptno;
                                            if (rightscode == 2 || rightscode == 0)
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 8].Text = challanno;
                                            if (rightscode == 3)
                                            {
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 7].Text = receiptno;
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 8].Text = challanno;
                                            }

                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 9].Text = FinYear;

                                            if (sclValue == 0)
                                                spreadDet.Sheets[0].Columns[6].Visible = false;
                                            else
                                                spreadDet.Sheets[0].Columns[6].Visible = true;
                                            double paidAmount = 0;
                                            double.TryParse(Convert.ToString(dvpaid[pd]["paid"]), out paidAmount);
                                            totalAmount += paidAmount;
                                            height += 10;
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = Convert.ToString(paidAmount);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Right;
                                            if (!GrandTotal.ContainsKey(colCnt))
                                                GrandTotal.Add(colCnt, paidAmount);
                                            else
                                            {
                                                double total = 0;
                                                double.TryParse(Convert.ToString(GrandTotal[colCnt]), out total);
                                                total += paidAmount;
                                                GrandTotal.Remove(colCnt);
                                                GrandTotal.Add(colCnt, total);
                                            }
                                            #region Paymode value set

                                            if (paidAmount != 0)
                                            {
                                                if (Linkvalue == "0")
                                                {
                                                    if (diccurarrfee.ContainsKey(feeVal + "$" + cursem))
                                                    {
                                                        Double getval = paidAmount + diccurarrfee[feeVal + "$" + cursem];
                                                        diccurarrfee[feeVal + "$" + cursem] = getval;
                                                    }
                                                    else
                                                        diccurarrfee.Add(feeVal + "$" + cursem, paidAmount);
                                                }
                                                else
                                                {
                                                    string valuenew = returnYearforSem(cursem);
                                                    if (diccurarrfee.ContainsKey(feeVal + "$" + valuenew.ToString()))
                                                    {
                                                        Double getval = paidAmount + diccurarrfee[feeVal + "$" + valuenew.ToString()];
                                                        diccurarrfee[feeVal + "$" + valuenew.ToString()] = getval;
                                                    }
                                                    else
                                                        diccurarrfee.Add(feeVal + "$" + valuenew.ToString(), paidAmount);
                                                }
                                                if (paymode == "1")
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].BackColor = Color.LightCoral;
                                                else if (paymode == "2")
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].BackColor = Color.LightGray;
                                                else if (paymode == "3")
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].BackColor = Color.Orange;
                                                else if (paymode == "4")
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].BackColor = Color.LightGreen;
                                                else if (paymode == "5")
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].BackColor = Color.LightGoldenrodYellow;

                                                if (!dtPayMode.ContainsKey(paymode))
                                                    dtPayMode.Add(paymode, paidAmount);
                                                else
                                                {
                                                    double total = 0;
                                                    double.TryParse(Convert.ToString(dtPayMode[paymode]), out total);
                                                    total += paidAmount;
                                                    dtPayMode.Remove(paymode);
                                                    dtPayMode.Add(paymode, total);
                                                }
                                                checkStatus = true;
                                            }
                                            addRow = true;
                                            #endregion
                                        }
                                    }
                                    else
                                    {
                                        //  spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = "-";
                                        // spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    #endregion
                                }
                            }
                        }
                        else
                        {
                            for (int remv = 0; remv < treeledger.Nodes.Count; remv++)
                            {
                                if (treeledger.Nodes[remv].ChildNodes.Count > 0)
                                {
                                    double tempTotalAmount = 0;
                                    string ledgerStrHd = string.Empty;
                                    for (int j = 0; j < treeledger.Nodes[remv].ChildNodes.Count; j++)
                                    {
                                        if (treeledger.Nodes[remv].ChildNodes[j].Checked == true)
                                        {
                                            #region ledger
                                            ds.Tables[1].DefaultView.RowFilter = "roll_no='" + rollno + "' " + feecat + " and ledgerfk='" + treeledger.Nodes[remv].ChildNodes[j].Value + "' ";
                                            //and actualfinyearfk='" + chklsfyear.Items[fin].Value + "'
                                            DataView dvpaid = ds.Tables[1].DefaultView;
                                            int colCnt = 0;
                                            int.TryParse(Convert.ToString(htColCnt[treeledger.Nodes[remv].ChildNodes[j].Value]), out colCnt);
                                            // ledgerStrHd = treeledger.Nodes[remv].ChildNodes[j].Text;
                                            ledgerStrHd = treeledger.Nodes[remv].ChildNodes[j].Value;
                                            if (dvpaid.Count > 0)
                                            {
                                                if (tempdegreedeatisl != getdegreedetails && cbdeptName.Checked)
                                                {
                                                    spreadDet.Sheets[0].RowCount++;
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = batcyear + "-" + deg_dept + "/Sem-" + year;
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, spreadDet.Sheets[0].Columns.Count);
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].BackColor = Color.LightSkyBlue;
                                                    //arfeecat.Add(feeVal);
                                                    getdegreedetails = tempdegreedeatisl;
                                                }
                                                for (int pd = 0; pd < dvpaid.Count; pd++)
                                                {
                                                    if (!addRow)
                                                    {
                                                        sno++;
                                                        spreadDet.Sheets[0].RowCount++;
                                                    }
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = name;
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = rollno;
                                                    // if (sclflag == false)
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].Text = regno;
                                                    // else
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].Text = rolladmit;
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].CellType = txtroll;
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].CellType = txtreg;
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].CellType = txtadd;
                                                    // spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].CellType = txtreg;
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 5].Text = deg_acr;
                                                    // if (feecateflag == true)
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 6].Text = cbl_sem.Items[sem].Text;
                                                    // else
                                                    // spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 6].Text = year;
                                                    receiptno = Convert.ToString(dvpaid[pd]["ReceiptNo"]);
                                                    challanno = Convert.ToString(dvpaid[pd]["challan_no"]);
                                                    //  feecat = Convert.ToString(dvpaid[pd]["FeeCategory"]);
                                                    string cursem = Convert.ToString(dvpaid[pd]["current_semester"]);
                                                    string paymode = Convert.ToString(dvpaid[pd]["mode"]);
                                                    string feechec = string.Empty;
                                                    //due color change
                                                    if (sclValue == 0)
                                                        feechec = cbl_sem.Items[sem].Text.Split(' ')[1];
                                                    else
                                                        feechec = cbl_sem.Items[sem].Text.Split(' ')[0];
                                                    if (Convert.ToInt32(cursem) > Convert.ToInt32(feechec))
                                                        spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].BackColor = Color.YellowGreen;
                                                    if (rightscode == 1)
                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 7].Text = receiptno;
                                                    if (rightscode == 2 || rightscode == 0)
                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 8].Text = challanno;
                                                    if (rightscode == 3)
                                                    {
                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 7].Text = receiptno;
                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 8].Text = challanno;
                                                    }

                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(dvpaid[pd]["FinYear"]);

                                                    if (sclValue == 0)
                                                        spreadDet.Sheets[0].Columns[6].Visible = false;
                                                    else
                                                        spreadDet.Sheets[0].Columns[6].Visible = true;
                                                    double paidAmount = 0;
                                                    double.TryParse(Convert.ToString(dvpaid[pd]["paid"]), out paidAmount);
                                                    totalAmount += paidAmount;
                                                    tempTotalAmount += paidAmount;
                                                    height += 10;
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = Convert.ToString(paidAmount);
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Right;
                                                    if (!GrandTotal.ContainsKey(colCnt))
                                                        GrandTotal.Add(colCnt, paidAmount);
                                                    else
                                                    {
                                                        double total = 0;
                                                        double.TryParse(Convert.ToString(GrandTotal[colCnt]), out total);
                                                        total += paidAmount;
                                                        GrandTotal.Remove(colCnt);
                                                        GrandTotal.Add(colCnt, total);
                                                    }
                                                    #region Paymode value set

                                                    if (paidAmount != 0)
                                                    {
                                                        if (Linkvalue == "0")
                                                        {
                                                            if (diccurarrfee.ContainsKey(feeVal + "$" + cursem))
                                                            {
                                                                Double getval = paidAmount + diccurarrfee[feeVal + "$" + cursem];
                                                                diccurarrfee[feeVal + "$" + cursem] = getval;
                                                            }
                                                            else
                                                                diccurarrfee.Add(feeVal + "$" + cursem, paidAmount);
                                                        }
                                                        else
                                                        {
                                                            string valuenew = returnYearforSem(cursem);
                                                            if (diccurarrfee.ContainsKey(feeVal + "$" + valuenew.ToString()))
                                                            {
                                                                Double getval = paidAmount + diccurarrfee[feeVal + "$" + valuenew.ToString()];
                                                                diccurarrfee[feeVal + "$" + valuenew.ToString()] = getval;
                                                            }
                                                            else
                                                                diccurarrfee.Add(feeVal + "$" + valuenew.ToString(), paidAmount);
                                                        }
                                                        if (paymode == "1")
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].BackColor = Color.LightCoral;
                                                        else if (paymode == "2")
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].BackColor = Color.LightGray;
                                                        else if (paymode == "3")
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].BackColor = Color.Orange;
                                                        else if (paymode == "4")
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].BackColor = Color.LightGreen;
                                                        else if (paymode == "5")
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].BackColor = Color.LightGoldenrodYellow;

                                                        if (!dtPayMode.ContainsKey(paymode))
                                                            dtPayMode.Add(paymode, paidAmount);
                                                        else
                                                        {
                                                            double total = 0;
                                                            double.TryParse(Convert.ToString(dtPayMode[paymode]), out total);
                                                            total += paidAmount;
                                                            dtPayMode.Remove(paymode);
                                                            dtPayMode.Add(paymode, total);
                                                        }
                                                        checkStatus = true;
                                                    }
                                                    addRow = true;
                                                    #endregion
                                                }
                                            }
                                            else
                                            {
                                                //  spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = "-";
                                                // spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Center;
                                            }
                                            #endregion
                                        }
                                    }
                                    //every ledger totalamount
                                    if (tempTotalAmount != 0)
                                    {
                                        #region
                                        int colCnt = 0;
                                        int.TryParse(Convert.ToString(htColCnt[ledgerStrHd + " TotalAmount"]), out colCnt);

                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = Convert.ToString(tempTotalAmount);
                                        //Total += Convert.ToDouble(dv1[0]["Balance"]);
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Right;
                                        if (!GrandTotal.ContainsKey(colCnt))
                                            GrandTotal.Add(colCnt, Convert.ToString(tempTotalAmount));
                                        else
                                        {
                                            double total = 0;
                                            double.TryParse(Convert.ToString(GrandTotal[colCnt]), out total);
                                            total += tempTotalAmount;
                                            GrandTotal.Remove(colCnt);
                                            GrandTotal.Add(colCnt, Convert.ToString(total));
                                        }
                                        #endregion
                                    }
                                }
                            }
                        }
                        //    }
                        //}



                        if (totalAmount != 0)
                        {
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(totalAmount);
                            //Total += Convert.ToDouble(dv1[0]["Balance"]);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                            if (!GrandTotal.ContainsKey(spreadDet.Sheets[0].ColumnCount - 1))
                                GrandTotal.Add(spreadDet.Sheets[0].ColumnCount - 1, Convert.ToString(totalAmount));
                            else
                            {
                                double total = 0;
                                double.TryParse(Convert.ToString(GrandTotal[spreadDet.Sheets[0].ColumnCount - 1]), out total);
                                total += totalAmount;
                                GrandTotal.Remove(spreadDet.Sheets[0].ColumnCount - 1);
                                GrandTotal.Add(spreadDet.Sheets[0].ColumnCount - 1, Convert.ToString(total));
                            }
                        }
                    }
                }
            }
            #region every degreewise total
            if (GrandTotal.Count > 0)
            {
                if (cbdgreename.Checked)
                {
                    spreadDet.Sheets[0].RowCount++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = "Degree Wise Total";
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, 3);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].BackColor = Color.YellowGreen;
                }
                for (int d = 9; d < spreadDet.Sheets[0].Columns.Count; d++)
                {
                    Double dict = 0;
                    if (GrandTotal.ContainsKey(d))
                        double.TryParse(Convert.ToString(GrandTotal[d]), out dict);
                    if (cbdgreename.Checked)
                    {
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, d].Text = Convert.ToString(dict);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, d].Font.Bold = true;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, d].Font.Size = FontUnit.Medium;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, d].Font.Name = "Book Antiqua";
                    }
                    if (!htfnlTotal.ContainsKey(d))
                        htfnlTotal.Add(d, dict);
                    else
                    {
                        double total = 0;
                        double.TryParse(Convert.ToString(htfnlTotal[d]), out total);
                        total += dict;
                        htfnlTotal.Remove(d);
                        htfnlTotal.Add(d, total);
                    }
                }
                GrandTotal.Clear();
            }
            #endregion

            int colcnt = 0;
            if (checkStatus)
            {
                //  spreadDet.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
                spreadDet.Sheets[0].RowCount++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString("Grand Total");
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].BackColor = Color.LightSeaGreen;
                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, 6);
                for (int j = 9; j < spreadDet.Sheets[0].ColumnCount; j++)
                {
                    if (colcnt == 0)
                        colcnt = j;
                    double grandvalue = 0;
                    double.TryParse(Convert.ToString(htfnlTotal[j]), out grandvalue);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalue);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, j].Font.Bold = true;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, j].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, j].Font.Name = "Book Antiqua";
                }
                //string GetNewValue1 = Convert.ToString(GrandTotal[spreadDet.Sheets[0].ColumnCount - 1]);
                //spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(GetNewValue1);
                //spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                if (cbabstract.Checked)
                {
                    #region abstract

                    //abstract
                    double totalPaid = 0;
                    spreadDet.Sheets[0].RowCount++;// = FpSpread1.Sheets[0].RowCount + 2;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = "ABSTRACT";
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].BackColor = Color.Brown;
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].ForeColor = Color.White;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, 7);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    for (int ps = 0; ps < chkl_paid.Items.Count; ps++)
                    {
                        if (chkl_paid.Items[ps].Selected == true)
                        {
                            string strptype = chkl_paid.Items[ps].Text.ToString();
                            spreadDet.Sheets[0].RowCount++;
                            spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, 7);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = strptype;
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;

                            double grandPaid = 0;
                            //  if (dtPayMode.ContainsKey(Convert.ToString(dtPayMode[chkl_paid.Items[ps].Value])))
                            double.TryParse(Convert.ToString(dtPayMode[chkl_paid.Items[ps].Value]), out grandPaid);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, colcnt].Text = Convert.ToString(grandPaid); totalPaid += grandPaid;
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcnt].Font.Bold = true;
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcnt].HorizontalAlign = HorizontalAlign.Right;
                        }
                    }
                    spreadDet.Sheets[0].RowCount++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = "Total";
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, 7);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcnt].Text = Convert.ToString(totalPaid);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcnt].Font.Bold = true;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcnt].HorizontalAlign = HorizontalAlign.Right;
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
                            for (int rcs = 0; rcs < cbl_sem.Items.Count; rcs++)
                            {
                                if (cbl_sem.Items[rcs].Value.ToString() == spt[0].ToString())
                                {
                                    string feeval = cbl_sem.Items[rcs].Text.ToString();
                                    string[] stc = feeval.Split(' ');
                                    if (stc.GetUpperBound(0) >= 0)
                                    {
                                        if (stc[0].ToString().Trim() == spt[1].ToString().Trim())
                                            currfee = currfee + Convert.ToDouble(getfeeamount);
                                        else
                                            arrfee = arrfee + Convert.ToDouble(getfeeamount);
                                    }
                                    else
                                    {
                                        if (cbl_sem.Items[rcs].Text.Contains(spt[1].ToString()))
                                            currfee = currfee + Convert.ToDouble(getfeeamount);
                                        else
                                            arrfee = arrfee + Convert.ToDouble(getfeeamount);
                                    }
                                    rcs = cbl_sem.Items.Count;
                                }
                            }
                        }
                    }
                    spreadDet.Sheets[0].RowCount++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = "REGULAR";
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, 7);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcnt].Text = currfee.ToString();
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcnt].HorizontalAlign = HorizontalAlign.Right;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcnt].Font.Bold = true;
                    spreadDet.Sheets[0].RowCount++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = "ARREAR";
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].BackColor = Color.YellowGreen;
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, 7);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcnt].Text = arrfee.ToString();
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcnt].Font.Bold = true;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcnt].HorizontalAlign = HorizontalAlign.Right;
                    #endregion
                }
            }
            spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
            spreadDet.SaveChanges();
            //  spreadDet.Height = height;
            spreadDet.Visible = true;
            print.Visible = true;
            divlabl.Visible = true;
        }
        catch { }
    }

    protected void DatwiseGroupHdPaid(DataSet ds, DateTime fromdt, DateTime todt)
    {
        try
        {
            #region value
            challanAndReceiptNoRights();
            Hashtable htColCnt = new Hashtable();
            RollAndRegSettings();
            loadspreadDetails(ref  htColCnt);
            spreadColumnVisible();
            int sclValue = 0;
            int.TryParse(Convert.ToString(sclSett()), out sclValue);
            string Linkvalue = ArrearLinkValue();
            ArrayList datearray = new ArrayList();
            ArrayList arroll = new ArrayList();
            Dictionary<string, Double> diccurarrfee = new Dictionary<string, double>();
            Hashtable GrandTotal = new Hashtable();
            Hashtable fnltotal = new Hashtable();
            Hashtable dtPayMode = new Hashtable();
            Hashtable htfnlTotal = new Hashtable();
            ArrayList ardegree = new ArrayList();
            bool checkStatus = false;
            int height = 0;
            string tempdegreedeatis = string.Empty;
            string getdegreedetails = string.Empty;
            string from = fromdt.ToString("yyyy-MM-dd");
            string to = todt.ToString("yyyy-MM-dd");
            int sno = 0;
            FarPoint.Web.Spread.TextCellType txtroll = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.TextCellType txtreg = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.TextCellType txtadd = new FarPoint.Web.Spread.TextCellType();
            for (DateTime dts = Convert.ToDateTime(from); dts <= Convert.ToDateTime(to); dts = dts.AddDays(1))
            {
                bool dtbool = false;
                for (int sem = 0; sem < cbl_sem.Items.Count; sem++)
                {
                    if (cbl_sem.Items[sem].Selected)
                    {
                        ds.Tables[0].DefaultView.RowFilter = " cal_date='" + dts + "' and FeeCategory='" + Convert.ToString(cbl_sem.Items[sem].Value) + "'";
                        DataView dvstud = ds.Tables[0].DefaultView;
                        if (dvstud.Count > 0)
                        {
                            for (int std = 0; std < dvstud.Count; std++)
                            {
                                string dcode = Convert.ToString(dvstud[std]["Degree_code"]);

                                if (cbdegwisetotal.Checked)
                                {
                                    if (!ardegree.Contains(dts.ToString("dd/MM/yyyy")))
                                    {
                                        #region every degreewise total
                                        if (GrandTotal.Count > 0)
                                        {
                                            spreadDet.Sheets[0].RowCount++;
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = "Date Wise Total";
                                            spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, 3);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#48D1CC");
                                            // spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].BackColor = Color.YellowGreen;

                                            for (int d = 9; d < spreadDet.Sheets[0].Columns.Count; d++)
                                            {
                                                Double dict = 0;
                                                if (GrandTotal.ContainsKey(d))
                                                    double.TryParse(Convert.ToString(GrandTotal[d]), out dict);
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, d].Text = Convert.ToString(dict);
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, d].Font.Bold = true;
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, d].Font.Size = FontUnit.Medium;
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, d].Font.Name = "Book Antiqua";
                                                if (!htfnlTotal.ContainsKey(d))
                                                    htfnlTotal.Add(d, dict);
                                                else
                                                {
                                                    double total = 0;
                                                    double.TryParse(Convert.ToString(htfnlTotal[d]), out total);
                                                    total += dict;
                                                    htfnlTotal.Remove(d);
                                                    htfnlTotal.Add(d, total);
                                                }
                                            }
                                            GrandTotal.Clear();
                                        }
                                        ardegree.Add(dts.ToString("dd/MM/yyyy"));
                                        #endregion
                                    }
                                }
                                if (!dtbool)
                                {
                                    spreadDet.Sheets[0].RowCount++;
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = dts.ToString("dd/MM/yyyy");
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].BackColor = Color.PowderBlue;
                                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, spreadDet.Sheets[0].ColumnCount - 1);
                                    dtbool = true;
                                }
                                string Degreenme = "";
                                if (ds.Tables[2].Rows.Count > 0)
                                {
                                    ds.Tables[2].DefaultView.RowFilter = "Degree_code='" + dcode + "'";
                                    DataView Dview = ds.Tables[2].DefaultView;
                                    if (Dview.Count > 0)
                                        Degreenme = Convert.ToString(Dview[0]["degreename"]);
                                }
                                tempdegreedeatis = Degreenme + "-" + Convert.ToString(dvstud[std]["current_semester"]);
                                if ((cbdegwisetotal.Checked && cbdeptName.Checked) || cbdeptName.Checked)
                                {
                                    if (tempdegreedeatis != getdegreedetails)
                                    {
                                        spreadDet.Sheets[0].RowCount++;
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Degreenme + "-" + Convert.ToString(dvstud[std]["current_semester"]);
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                        spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, spreadDet.Sheets[0].Columns.Count);
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].BackColor = Color.LightSkyBlue;
                                        //arfeecat.Add(feeVal);
                                        getdegreedetails = tempdegreedeatis;
                                    }
                                    //if (!datearray.Contains(dts.ToString("dd/MM/yyyy")))
                                    //{
                                    //    datearray.Add(dts.ToString("dd/MM/yyyy"));
                                    //    spreadDet.Sheets[0].RowCount++;
                                    //    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Degreenme + "-" + Convert.ToString(dvstud[std]["current_semester"]);
                                    //    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    //    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].BackColor = Color.LightSkyBlue;
                                    //    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, spreadDet.Sheets[0].ColumnCount - 1);
                                    //}
                                }
                                string dvapp_no = Convert.ToString(dvstud[std]["App_No"]);
                                string Feecategory = Convert.ToString(dvstud[std]["FeeCategory"]);
                                spreadDet.Sheets[0].RowCount++;
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dvstud[std]["name"]);
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dvstud[std]["Roll_No"]);
                                //   if (sclflag == false)
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dvstud[std]["Reg_No"]);
                                // else
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dvstud[std]["roll_admit"]);
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].CellType = txtroll;
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].CellType = txtreg;
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].CellType = txtadd;
                                // spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].CellType = txtreg;
                                if (!arroll.Contains(Convert.ToString(dvstud[std]["Roll_No"])))
                                {
                                    arroll.Add(Convert.ToString(dvstud[std]["Roll_No"]));
                                    // sno++;
                                }
                                sno++;
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 5].Text = Degreenme;
                                string TextName = "";
                                if (ds.Tables[3].Rows.Count > 0)
                                {
                                    ds.Tables[3].DefaultView.RowFilter = "TextCode='" + Feecategory + "'";
                                    DataView Dview = ds.Tables[3].DefaultView;
                                    if (Dview.Count > 0)
                                        TextName = Convert.ToString(Dview[0]["TextVal"]);
                                }
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 6].Text = TextName;
                                string curSem = Convert.ToString(dvstud[std]["current_semester"]);
                                string feechec = string.Empty;
                                if (sclValue == 1)//school or college checking to split feecategory
                                    feechec = TextName.Split(' ')[0];
                                else
                                    feechec = TextName.Split(' ')[1];
                                if (Convert.ToInt32(curSem) > Convert.ToInt32(feechec))
                                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].BackColor = Color.YellowGreen;
                                string reptno = Convert.ToString(dvstud[std]["ReceiptNo"]);
                                string chalno = Convert.ToString(dvstud[std]["challan_no"]);
                                double totalAmount = 0;
                                height += 10;
                                if (ddlacctype.SelectedItem.Text != "Ledger")
                                {
                                    for (int j = 0; j < cblheader.Items.Count; j++)
                                    {
                                        if (cblheader.Items[j].Selected == true)
                                        {
                                            #region header and group header
                                            string strhdName = string.Empty;
                                            string hdName = string.Empty;
                                            if (ddlacctype.SelectedItem.Text == "Group Header")
                                            {
                                                strhdName = "and ChlGroupHeader='" + Convert.ToString(cblheader.Items[j].Text) + "'";
                                                hdName = Convert.ToString(cblheader.Items[j].Text);
                                            }
                                            else
                                            {
                                                strhdName = "and headerfk='" + Convert.ToString(cblheader.Items[j].Value) + "'";
                                                hdName = Convert.ToString(cblheader.Items[j].Value);
                                            }
                                            string detail = "App_no='" + dvapp_no + "' and FeeCategory='" + Feecategory + "' " + strhdName + " and cal_date='" + dts + "'";
                                            if (chalno != "")
                                                detail += "and challan_no='" + chalno + "'";
                                            if (reptno != "")
                                                detail += "and ReceiptNo='" + reptno + "'";
                                            ds.Tables[1].DefaultView.RowFilter = detail;
                                            DataView dvpaid = ds.Tables[1].DefaultView;
                                            int colCnt = 0;
                                            int.TryParse(Convert.ToString(htColCnt[hdName]), out colCnt);
                                            if (dvpaid.Count > 0)
                                            {
                                                for (int k = 0; k < dvpaid.Count; k++)
                                                {
                                                    //  if (colval == false)
                                                    // colcnt = col;
                                                    //  colval = true;
                                                    #region receipt and challan no

                                                    string receiptno = "";
                                                    receiptno = Convert.ToString(dvpaid[k]["ReceiptNo"]);
                                                    string challanno = Convert.ToString(dvpaid[k]["challan_no"]);
                                                    if (rightscode == 1)
                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 7].Text = receiptno;
                                                    if (rightscode == 2 || rightscode == 0)
                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 8].Text = challanno;
                                                    if (rightscode == 3)
                                                    {
                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 7].Text = receiptno;
                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 8].Text = challanno;
                                                    }
                                                    #endregion
                                                    double paidAmount = 0;
                                                    double.TryParse(Convert.ToString(dvpaid[k]["paid"]), out paidAmount);
                                                    totalAmount += paidAmount;
                                                    // dateval = true;
                                                    // FpSpread1.Sheets[0].RowCount++;
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = Convert.ToString(paidAmount);
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(dvpaid[k]["Finyear"]);
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Right;
                                                    if (!GrandTotal.ContainsKey(colCnt))
                                                        GrandTotal.Add(colCnt, Convert.ToString(paidAmount));
                                                    else
                                                    {
                                                        double total = 0;
                                                        double.TryParse(Convert.ToString(GrandTotal[colCnt]), out total);
                                                        total += paidAmount;
                                                        GrandTotal.Remove(colCnt);
                                                        GrandTotal.Add(colCnt, Convert.ToString(total));
                                                    }
                                                    #region Paymode value set
                                                    string cursem = Convert.ToString(dvpaid[k]["Current_Semester"]);
                                                    string paymode = Convert.ToString(dvpaid[k]["mode"]);
                                                    if (paidAmount != 0)
                                                    {
                                                        if (Linkvalue == "0")
                                                        {
                                                            if (diccurarrfee.ContainsKey(Feecategory + "$" + cursem))
                                                            {
                                                                Double getval = paidAmount + diccurarrfee[Feecategory + "$" + cursem];
                                                                diccurarrfee[Feecategory + "$" + cursem] = getval;
                                                            }
                                                            else
                                                                diccurarrfee.Add(Feecategory + "$" + cursem, paidAmount);
                                                        }
                                                        else
                                                        {
                                                            string valuenew = returnYearforSem(cursem);
                                                            if (diccurarrfee.ContainsKey(Feecategory + "$" + valuenew.ToString()))
                                                            {
                                                                Double getval = paidAmount + diccurarrfee[Feecategory + "$" + valuenew.ToString()];
                                                                diccurarrfee[Feecategory + "$" + valuenew.ToString()] = getval;
                                                            }
                                                            else
                                                                diccurarrfee.Add(Feecategory + "$" + valuenew.ToString(), paidAmount);
                                                        }
                                                        if (paymode == "1")
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].BackColor = Color.LightCoral;
                                                        else if (paymode == "2")
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].BackColor = Color.LightGray;
                                                        else if (paymode == "3")
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].BackColor = Color.Orange;
                                                        else if (paymode == "4")
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].BackColor = Color.LightGreen;
                                                        else if (paymode == "5")
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].BackColor = Color.LightGoldenrodYellow;

                                                        if (!dtPayMode.ContainsKey(paymode))
                                                            dtPayMode.Add(paymode, paidAmount);
                                                        else
                                                        {
                                                            double total = 0;
                                                            double.TryParse(Convert.ToString(dtPayMode[paymode]), out total);
                                                            total += paidAmount;
                                                            dtPayMode.Remove(paymode);
                                                            dtPayMode.Add(paymode, total);
                                                        }
                                                        checkStatus = true;
                                                    }
                                                    // addRow = true;
                                                    #endregion
                                                }
                                            }
                                            else
                                            {
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = "-";
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Center;
                                            }
                                            #endregion
                                        }
                                    }
                                }
                                else
                                {
                                    for (int remv = 0; remv < treeledger.Nodes.Count; remv++)
                                    {
                                        if (treeledger.Nodes[remv].ChildNodes.Count > 0)
                                        {
                                            double tempTotalAmount = 0;
                                            string ledgerStrHd = string.Empty;
                                            for (int j = 0; j < treeledger.Nodes[remv].ChildNodes.Count; j++)
                                            {
                                                if (treeledger.Nodes[remv].ChildNodes[j].Checked == true)
                                                {
                                                    #region ledger
                                                    string strhdName = string.Empty;
                                                    string hdName = string.Empty;
                                                    strhdName = " and ledgerfk='" + Convert.ToString(treeledger.Nodes[remv].ChildNodes[j].Value) + "'";
                                                    //ledgerStrHd = treeledger.Nodes[remv].ChildNodes[j].Text;
                                                    ledgerStrHd = treeledger.Nodes[remv].ChildNodes[j].Value;
                                                    hdName = Convert.ToString(treeledger.Nodes[remv].ChildNodes[j].Value);
                                                    string detail = "App_no='" + dvapp_no + "' and FeeCategory='" + Feecategory + "' " + strhdName + " and cal_date='" + dts + "'";
                                                    if (chalno != "")
                                                        detail += "and challan_no='" + chalno + "'";
                                                    if (reptno != "")
                                                        detail += "and ReceiptNo='" + reptno + "'";
                                                    ds.Tables[1].DefaultView.RowFilter = detail;
                                                    DataView dvpaid = ds.Tables[1].DefaultView;
                                                    int colCnt = 0;
                                                    int.TryParse(Convert.ToString(htColCnt[hdName]), out colCnt);
                                                    if (dvpaid.Count > 0)
                                                    {
                                                        for (int k = 0; k < dvpaid.Count; k++)
                                                        {
                                                            //  if (colval == false)
                                                            // colcnt = col;
                                                            //  colval = true;
                                                            #region receipt and challan no

                                                            string receiptno = "";
                                                            receiptno = Convert.ToString(dvpaid[k]["ReceiptNo"]);
                                                            string challanno = Convert.ToString(dvpaid[k]["challan_no"]);
                                                            if (rightscode == 1)
                                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 7].Text = receiptno;
                                                            if (rightscode == 2 || rightscode == 0)
                                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 8].Text = challanno;
                                                            if (rightscode == 3)
                                                            {
                                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 7].Text = receiptno;
                                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 8].Text = challanno;
                                                            }
                                                            #endregion
                                                            double paidAmount = 0;
                                                            double.TryParse(Convert.ToString(dvpaid[k]["paid"]), out paidAmount);
                                                            totalAmount += paidAmount;
                                                            tempTotalAmount += paidAmount;
                                                            // dateval = true;
                                                            // FpSpread1.Sheets[0].RowCount++;
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = Convert.ToString(paidAmount);
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Right;
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(dvpaid[k]["Finyear"]);
                                                            if (!GrandTotal.ContainsKey(colCnt))
                                                                GrandTotal.Add(colCnt, Convert.ToString(paidAmount));
                                                            else
                                                            {
                                                                double total = 0;
                                                                double.TryParse(Convert.ToString(GrandTotal[colCnt]), out total);
                                                                total += paidAmount;
                                                                GrandTotal.Remove(colCnt);
                                                                GrandTotal.Add(colCnt, Convert.ToString(total));
                                                            }
                                                            #region Paymode value set
                                                            string cursem = Convert.ToString(dvpaid[k]["Current_Semester"]);
                                                            string paymode = Convert.ToString(dvpaid[k]["mode"]);
                                                            if (paidAmount != 0)
                                                            {
                                                                if (Linkvalue == "0")
                                                                {
                                                                    if (diccurarrfee.ContainsKey(Feecategory + "$" + cursem))
                                                                    {
                                                                        Double getval = paidAmount + diccurarrfee[Feecategory + "$" + cursem];
                                                                        diccurarrfee[Feecategory + "$" + cursem] = getval;
                                                                    }
                                                                    else
                                                                        diccurarrfee.Add(Feecategory + "$" + cursem, paidAmount);
                                                                }
                                                                else
                                                                {
                                                                    string valuenew = returnYearforSem(cursem);
                                                                    if (diccurarrfee.ContainsKey(Feecategory + "$" + valuenew.ToString()))
                                                                    {
                                                                        Double getval = paidAmount + diccurarrfee[Feecategory + "$" + valuenew.ToString()];
                                                                        diccurarrfee[Feecategory + "$" + valuenew.ToString()] = getval;
                                                                    }
                                                                    else
                                                                        diccurarrfee.Add(Feecategory + "$" + valuenew.ToString(), paidAmount);
                                                                }
                                                                if (paymode == "1")
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].BackColor = Color.LightCoral;
                                                                else if (paymode == "2")
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].BackColor = Color.LightGray;
                                                                else if (paymode == "3")
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].BackColor = Color.Orange;
                                                                else if (paymode == "4")
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].BackColor = Color.LightGreen;
                                                                else if (paymode == "5")
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].BackColor = Color.LightGoldenrodYellow;

                                                                if (!dtPayMode.ContainsKey(paymode))
                                                                    dtPayMode.Add(paymode, paidAmount);
                                                                else
                                                                {
                                                                    double total = 0;
                                                                    double.TryParse(Convert.ToString(dtPayMode[paymode]), out total);
                                                                    total += paidAmount;
                                                                    dtPayMode.Remove(paymode);
                                                                    dtPayMode.Add(paymode, total);
                                                                }
                                                                checkStatus = true;
                                                            }
                                                            // addRow = true;
                                                            #endregion
                                                        }
                                                    }
                                                    else
                                                    {
                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = "-";
                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Center;
                                                    }
                                                    #endregion
                                                }
                                            }
                                            //every ledger totalamount
                                            if (tempTotalAmount != 0)
                                            {
                                                #region
                                                int colCnt = 0;
                                                int.TryParse(Convert.ToString(htColCnt[ledgerStrHd + " TotalAmount"]), out colCnt);

                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = Convert.ToString(tempTotalAmount);
                                                //Total += Convert.ToDouble(dv1[0]["Balance"]);
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Right;
                                                if (!GrandTotal.ContainsKey(colCnt))
                                                    GrandTotal.Add(colCnt, Convert.ToString(tempTotalAmount));
                                                else
                                                {
                                                    double total = 0;
                                                    double.TryParse(Convert.ToString(GrandTotal[colCnt]), out total);
                                                    total += tempTotalAmount;
                                                    GrandTotal.Remove(colCnt);
                                                    GrandTotal.Add(colCnt, Convert.ToString(total));
                                                }
                                                #endregion
                                            }
                                        }
                                    }
                                }
                                if (totalAmount != 0)
                                {
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(totalAmount);
                                    //Total += Convert.ToDouble(dv1[0]["Balance"]);
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                    if (!GrandTotal.ContainsKey(spreadDet.Sheets[0].ColumnCount - 1))
                                        GrandTotal.Add(spreadDet.Sheets[0].ColumnCount - 1, Convert.ToString(totalAmount));
                                    else
                                    {
                                        double total = 0;
                                        double.TryParse(Convert.ToString(GrandTotal[spreadDet.Sheets[0].ColumnCount - 1]), out total);
                                        total += totalAmount;
                                        GrandTotal.Remove(spreadDet.Sheets[0].ColumnCount - 1);
                                        GrandTotal.Add(spreadDet.Sheets[0].ColumnCount - 1, Convert.ToString(total));
                                    }
                                }
                            }
                            spreadDet.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            spreadDet.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            spreadDet.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        }
                    }
                }
            }
            if (cbdegwisetotal.Checked)
            {
                #region every degreewise total
                if (GrandTotal.Count > 0)
                {
                    spreadDet.Sheets[0].RowCount++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = "Date Wise Total";
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, 3);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#48D1CC");
                    for (int d = 9; d < spreadDet.Sheets[0].Columns.Count; d++)
                    {
                        Double dict = 0;
                        if (GrandTotal.ContainsKey(d))
                            double.TryParse(Convert.ToString(GrandTotal[d]), out dict);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, d].Text = Convert.ToString(dict);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, d].Font.Bold = true;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, d].Font.Size = FontUnit.Medium;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, d].Font.Name = "Book Antiqua";
                        if (!htfnlTotal.ContainsKey(d))
                            htfnlTotal.Add(d, dict);
                        else
                        {
                            double total = 0;
                            double.TryParse(Convert.ToString(htfnlTotal[d]), out total);
                            total += dict;
                            htfnlTotal.Remove(d);
                            htfnlTotal.Add(d, total);
                        }
                    }
                    GrandTotal.Clear();
                }
                #endregion
            }
            int colcnt = 0;
            if (checkStatus)
            {
                //  spreadDet.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
                spreadDet.Sheets[0].RowCount++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString("Grand Total");
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].BackColor = Color.LightSeaGreen;
                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, 6);
                for (int j = 9; j < spreadDet.Sheets[0].ColumnCount; j++)
                {
                    if (colcnt == 0)
                        colcnt = j;
                    double grandvalue = 0;
                    if (cbdegwisetotal.Checked)
                        double.TryParse(Convert.ToString(htfnlTotal[j]), out grandvalue);
                    else
                        double.TryParse(Convert.ToString(GrandTotal[j]), out grandvalue);

                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalue);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, j].Font.Bold = true;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, j].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, j].Font.Name = "Book Antiqua";
                }
                //string GetNewValue1 = Convert.ToString(GrandTotal[spreadDet.Sheets[0].ColumnCount - 1]);
                //spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(GetNewValue1);
                //spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                if (cbabstract.Checked)
                {
                    #region abstract

                    //abstract
                    double totalPaid = 0;
                    spreadDet.Sheets[0].RowCount++;// = FpSpread1.Sheets[0].RowCount + 2;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = "ABSTRACT";
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].BackColor = Color.Brown;
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].ForeColor = Color.White;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, 7);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    for (int ps = 0; ps < chkl_paid.Items.Count; ps++)
                    {
                        if (chkl_paid.Items[ps].Selected == true)
                        {
                            string strptype = chkl_paid.Items[ps].Text.ToString();
                            spreadDet.Sheets[0].RowCount++;
                            spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, 7);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = strptype;
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                            double grandPaid = 0;
                            //  if (dtPayMode.ContainsKey(Convert.ToString(dtPayMode[chkl_paid.Items[ps].Value])))
                            double.TryParse(Convert.ToString(dtPayMode[chkl_paid.Items[ps].Value]), out grandPaid);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, colcnt].Text = Convert.ToString(grandPaid); totalPaid += grandPaid;
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcnt].Font.Bold = true;
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcnt].HorizontalAlign = HorizontalAlign.Right;
                        }
                    }
                    spreadDet.Sheets[0].RowCount++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = "Total";
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, 7);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcnt].Text = Convert.ToString(totalPaid);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcnt].Font.Bold = true;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcnt].HorizontalAlign = HorizontalAlign.Right;
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
                            for (int rcs = 0; rcs < cbl_sem.Items.Count; rcs++)
                            {
                                if (cbl_sem.Items[rcs].Value.ToString() == spt[0].ToString())
                                {
                                    string feeval = cbl_sem.Items[rcs].Text.ToString();
                                    string[] stc = feeval.Split(' ');
                                    if (stc.GetUpperBound(0) >= 0)
                                    {
                                        if (stc[0].ToString().Trim() == spt[1].ToString().Trim())
                                            currfee = currfee + Convert.ToDouble(getfeeamount);
                                        else
                                            arrfee = arrfee + Convert.ToDouble(getfeeamount);
                                    }
                                    else
                                    {
                                        if (cbl_sem.Items[rcs].Text.Contains(spt[1].ToString()))
                                            currfee = currfee + Convert.ToDouble(getfeeamount);
                                        else
                                            arrfee = arrfee + Convert.ToDouble(getfeeamount);
                                    }
                                    rcs = cbl_sem.Items.Count;
                                }
                            }
                        }
                    }
                    spreadDet.Sheets[0].RowCount++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = "REGULAR";
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, 7);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcnt].Text = currfee.ToString();
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcnt].Font.Bold = true;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcnt].HorizontalAlign = HorizontalAlign.Right;
                    spreadDet.Sheets[0].RowCount++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = "ARREAR";
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].BackColor = Color.YellowGreen;
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, 7);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcnt].Text = arrfee.ToString();
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcnt].Font.Bold = true;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colcnt].HorizontalAlign = HorizontalAlign.Right;
                    #endregion
                }
            }
            spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
            spreadDet.SaveChanges();
            spreadDet.Height = height;
            spreadDet.Visible = true;
            print.Visible = true;
            divlabl.Visible = true;
            #endregion
        }
        catch { }
    }

    protected void headerLedgerYetToBePaid(DataSet ds)
    {
        try
        {
            #region value
            challanAndReceiptNoRights();
            Hashtable htColCnt = new Hashtable();
            RollAndRegSettings();
            loadspreadDetails(ref  htColCnt);
            spreadColumnVisible();
            string Linkvalue = ArrearLinkValue();
            int sno = 0;
            int height = 0;
            bool checkStatus = false;
            Hashtable GrandTotal = new Hashtable();
            Dictionary<string, Double> diccurarrfee = new Dictionary<string, double>();
            Hashtable dtPayMode = new Hashtable();
            spreadDet.Sheets[0].Columns[7].Visible = false;
            spreadDet.Sheets[0].Columns[8].Visible = false;
            FarPoint.Web.Spread.TextCellType txtroll = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.TextCellType txtreg = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.TextCellType txtadd = new FarPoint.Web.Spread.TextCellType();
            for (int batch = 0; batch < cbl_batch.Items.Count; batch++)
            {
                if (cbl_batch.Items[batch].Selected == true)
                {
                    for (int Bnch = 0; Bnch < cbl_dept.Items.Count; Bnch++)
                    {
                        if (cbl_dept.Items[Bnch].Selected == true)
                        {
                            ds.Tables[0].DefaultView.RowFilter = "degree_code='" + cbl_dept.Items[Bnch].Value + "' and Batch_Year='" + Convert.ToString(cbl_batch.Items[batch].Text) + "'";
                            DataView dv2 = ds.Tables[0].DefaultView;
                            if (dv2.Count > 0)
                            {
                                if (cbdgreename.Checked)
                                {
                                    spreadDet.Sheets[0].RowCount++;
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(cbl_dept.Items[Bnch].Text) + " - " + Convert.ToString(cbl_batch.Items[batch].Text);
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].BackColor = Color.LightGreen;
                                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, spreadDet.Sheets[0].ColumnCount - 1);
                                }
                                for (int row = 0; row < dv2.Count; row++)
                                {
                                    string app_no = Convert.ToString(dv2[row]["App_No"]);
                                    ds.Tables[1].DefaultView.RowFilter = "App_No=" + Convert.ToString(app_no) + " and degree_code='" + cbl_dept.Items[Bnch].Value + "'";
                                    DataView dv = ds.Tables[1].DefaultView;
                                    if (dv.Count > 0)
                                    {
                                        for (int drow = 0; drow < dv.Count; drow++)
                                        {
                                            string dvapp_no = Convert.ToString(dv[drow]["App_No"]);
                                            string Feecategory = Convert.ToString(dv[drow]["FeeCategory"]);
                                            spreadDet.Sheets[0].RowCount++;
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dv[drow]["Stud_Name"]);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dv[drow]["Roll_No"]);
                                            // if (sclflag == false)
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv[drow]["Reg_No"]);
                                            // else
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dv[drow]["roll_admit"]);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].CellType = txtroll;
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].CellType = txtreg;
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].CellType = txtadd;
                                            //spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].CellType = txtreg;
                                            string Degreename = "";
                                            if (ds.Tables[4].Rows.Count > 0)
                                            {
                                                ds.Tables[4].DefaultView.RowFilter = "Degree_code='" + Convert.ToString(dv[drow]["Degree_code"]) + "'";
                                                DataView Dview = ds.Tables[4].DefaultView;
                                                if (Dview.Count > 0)
                                                {
                                                    Degreename = Convert.ToString(Dview[0]["degreename"]);
                                                }
                                            }
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 5].Text = Degreename;
                                            string TextName = "";
                                            if (ds.Tables[5].Rows.Count > 0)
                                            {
                                                ds.Tables[5].DefaultView.RowFilter = "TextCode='" + Convert.ToString(dv[drow]["FeeCategory"]) + "'";
                                                DataView Dview = ds.Tables[5].DefaultView;
                                                if (Dview.Count > 0)
                                                    TextName = Convert.ToString(Dview[0]["TextVal"]);
                                            }
                                            // if (feecateflag == true)
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 6].Text = TextName;
                                            // else
                                            //  spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 5].Text = TextName;
                                            double totalAmount = 0;
                                            int HeaderCount = 0;
                                            if (ddlacctype.SelectedItem.Text == "Header" || ddlacctype.SelectedItem.Text == "Group Header")
                                            {
                                                #region header
                                                int colcnt = 0;
                                                bool check = false;
                                                for (int j = 0; j < cblheader.Items.Count; j++)
                                                {
                                                    double demandAmt = 0;
                                                    double PaidAmt = 0;
                                                    double BalAmt = 0;
                                                    if (cblheader.Items[j].Selected == true)
                                                    {
                                                        //allot totalamount
                                                        string strhdName = string.Empty;
                                                        string hdName = string.Empty;
                                                        if (ddlacctype.SelectedItem.Text == "Group Header")
                                                        {
                                                            strhdName = "and ChlGroupHeader='" + Convert.ToString(cblheader.Items[j].Text) + "'";
                                                            hdName = Convert.ToString(cblheader.Items[j].Text);
                                                        }
                                                        else
                                                        {
                                                            strhdName = "and headerfk='" + Convert.ToString(cblheader.Items[j].Value) + "'";
                                                            hdName = Convert.ToString(cblheader.Items[j].Value);
                                                        }

                                                        spreadDet.Sheets[0].Columns[7].Visible = false;
                                                        spreadDet.Sheets[0].Columns[8].Visible = false;
                                                        check = true;
                                                        HeaderCount++;
                                                        ds.Tables[2].DefaultView.RowFilter = "App_no='" + dvapp_no + "' and FeeCategory='" + Feecategory + "' " + strhdName + "";
                                                        DataView dvallot = ds.Tables[2].DefaultView;
                                                        if (dvallot.Count > 0)
                                                            double.TryParse(Convert.ToString(Convert.ToString(dvallot[0]["Demand"])), out demandAmt);
                                                        //paid
                                                        ds.Tables[3].DefaultView.RowFilter = "App_no='" + dvapp_no + "' and FeeCategory='" + Feecategory + "' " + strhdName + "";
                                                        DataView dvpaid = ds.Tables[3].DefaultView;
                                                        //  col++;
                                                        // string strhdName = "and headerfk='" + Convert.ToString(cblheader.Items[j].Value) + "'";
                                                        //  string hdName = Convert.ToString(cblheader.Items[j].Value);
                                                        int colCnt = 0;
                                                        int.TryParse(Convert.ToString(htColCnt[hdName]), out colCnt);

                                                        if (dvpaid.Count > 0)
                                                        {
                                                            double.TryParse(Convert.ToString(Convert.ToString(dvpaid[0]["Paid"])), out PaidAmt);

                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(dvpaid[0]["Finyear"]);
                                                        }
                                                        if (demandAmt == PaidAmt)
                                                        {
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = "-";
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Center;
                                                            colcnt++;
                                                        }
                                                        else
                                                        {
                                                            BalAmt = demandAmt - PaidAmt;
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = Convert.ToString(BalAmt);
                                                            totalAmount += BalAmt;
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Right;
                                                        }
                                                        if (!GrandTotal.ContainsKey(colCnt))
                                                            GrandTotal.Add(colCnt, Convert.ToString(BalAmt));
                                                        else
                                                        {
                                                            double total = 0;
                                                            double.TryParse(Convert.ToString(GrandTotal[colCnt]), out total);
                                                            total += BalAmt;
                                                            GrandTotal.Remove(colCnt);
                                                            GrandTotal.Add(colCnt, Convert.ToString(total));
                                                            BalAmt = 0;
                                                        }
                                                        checkStatus = true;
                                                    }
                                                }
                                                if (check == true)
                                                {
                                                    if (HeaderCount == colcnt)
                                                    {
                                                        spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].Visible = false;
                                                    }
                                                    else
                                                    {
                                                        sno++;
                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                        height += 15;
                                                    }
                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                #region ledger
                                                bool boolvisb = false;
                                                for (int remv = 0; remv < treeledger.Nodes.Count; remv++)
                                                {
                                                    int colcnt = 0;
                                                    if (treeledger.Nodes[remv].ChildNodes.Count > 0)
                                                    {
                                                        double tempTotalAmount = 0;
                                                        string ledgerStrHd = string.Empty;
                                                        bool check = false;
                                                        for (int j = 0; j < treeledger.Nodes[remv].ChildNodes.Count; j++)
                                                        {
                                                            double demandAmt = 0;
                                                            double PaidAmt = 0;
                                                            double BalAmt = 0;
                                                            if (treeledger.Nodes[remv].ChildNodes[j].Checked == true)
                                                            {
                                                                #region ledger
                                                                string strhdName = string.Empty;
                                                                string hdName = string.Empty;
                                                                strhdName = " and ledgerfk='" + Convert.ToString(treeledger.Nodes[remv].ChildNodes[j].Value) + "'";
                                                                //ledgerStrHd = treeledger.Nodes[remv].ChildNodes[j].Text;
                                                                ledgerStrHd = treeledger.Nodes[remv].ChildNodes[j].Value;
                                                                hdName = Convert.ToString(treeledger.Nodes[remv].ChildNodes[j].Value);

                                                                //allot totalamount
                                                                check = true;
                                                                HeaderCount++;
                                                                ds.Tables[2].DefaultView.RowFilter = "App_no='" + dvapp_no + "' and FeeCategory='" + Feecategory + "' " + strhdName + "";
                                                                DataView dvallot = ds.Tables[2].DefaultView;
                                                                if (dvallot.Count > 0)
                                                                    double.TryParse(Convert.ToString(Convert.ToString(dvallot[0]["Demand"])), out demandAmt);
                                                                //paid
                                                                ds.Tables[3].DefaultView.RowFilter = "App_no='" + dvapp_no + "' and FeeCategory='" + Feecategory + "' " + strhdName + "";
                                                                DataView dvpaid = ds.Tables[3].DefaultView;
                                                                int colCnt = 0;
                                                                int.TryParse(Convert.ToString(htColCnt[hdName]), out colCnt);
                                                                if (dvpaid.Count > 0)
                                                                {
                                                                    double.TryParse(Convert.ToString(Convert.ToString(dvpaid[0]["Paid"])), out PaidAmt);

                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(dvpaid[0]["Finyear"]);
                                                                }
                                                                if (demandAmt == PaidAmt)
                                                                {
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = "-";
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Center;
                                                                    colcnt++;
                                                                }
                                                                else
                                                                {
                                                                    BalAmt = demandAmt - PaidAmt;
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = Convert.ToString(BalAmt);
                                                                    totalAmount += BalAmt;
                                                                    tempTotalAmount += BalAmt;
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Right;
                                                                }
                                                                if (!GrandTotal.ContainsKey(colCnt))
                                                                    GrandTotal.Add(colCnt, Convert.ToString(BalAmt));
                                                                else
                                                                {
                                                                    double total = 0;
                                                                    double.TryParse(Convert.ToString(GrandTotal[colCnt]), out total);
                                                                    total += BalAmt;
                                                                    GrandTotal.Remove(colCnt);
                                                                    GrandTotal.Add(colCnt, Convert.ToString(total));
                                                                    BalAmt = 0;
                                                                }
                                                                checkStatus = true;
                                                                #endregion
                                                            }
                                                        }
                                                        if (check == true)
                                                        {
                                                            if (HeaderCount == colcnt)
                                                            {
                                                                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].Visible = false;
                                                            }
                                                            else
                                                            {
                                                                sno++;
                                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].Visible = true;
                                                                height += 15;
                                                                boolvisb = true;
                                                            }
                                                        }
                                                        //every ledger totalamount
                                                        if (tempTotalAmount != 0)
                                                        {
                                                            #region
                                                            int colCnt = 0;
                                                            int.TryParse(Convert.ToString(htColCnt[ledgerStrHd + " TotalAmount"]), out colCnt);

                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = Convert.ToString(tempTotalAmount);
                                                            //Total += Convert.ToDouble(dv1[0]["Balance"]);
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Right;
                                                            if (!GrandTotal.ContainsKey(colCnt))
                                                                GrandTotal.Add(colCnt, Convert.ToString(tempTotalAmount));
                                                            else
                                                            {
                                                                double total = 0;
                                                                double.TryParse(Convert.ToString(GrandTotal[colCnt]), out total);
                                                                total += tempTotalAmount;
                                                                GrandTotal.Remove(colCnt);
                                                                GrandTotal.Add(colCnt, Convert.ToString(total));
                                                            }
                                                            #endregion
                                                        }
                                                    }
                                                }
                                                //header ed ,if any ledger have due amount to visible this row
                                                if (boolvisb)
                                                {
                                                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].Visible = true;
                                                }
                                                #endregion
                                            }
                                            if (totalAmount != 0)
                                            {
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(totalAmount);
                                                //Total += Convert.ToDouble(dv1[0]["Balance"]);
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                                if (!GrandTotal.ContainsKey(spreadDet.Sheets[0].ColumnCount - 1))
                                                    GrandTotal.Add(spreadDet.Sheets[0].ColumnCount - 1, Convert.ToString(totalAmount));
                                                else
                                                {
                                                    double total = 0;
                                                    double.TryParse(Convert.ToString(GrandTotal[spreadDet.Sheets[0].ColumnCount - 1]), out total);
                                                    total += totalAmount;
                                                    GrandTotal.Remove(spreadDet.Sheets[0].ColumnCount - 1);
                                                    GrandTotal.Add(spreadDet.Sheets[0].ColumnCount - 1, Convert.ToString(total));
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //int colcnt = 0;
            if (checkStatus)
            {
                //spreadDet.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
                spreadDet.Sheets[0].RowCount++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString("Grand Total");
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].RowCount - 1].BackColor = Color.LightSeaGreen;
                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, 6);
                for (int j = 9; j < spreadDet.Sheets[0].ColumnCount; j++)
                {
                    //if (colcnt == 0)
                    //colcnt = j;
                    double grandvalue = 0;
                    double.TryParse(Convert.ToString(GrandTotal[j]), out grandvalue);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalue);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, j].Font.Bold = true;
                }
            }
            spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
            spreadDet.SaveChanges();
            spreadDet.Height = height;
            spreadDet.Visible = true;
            print.Visible = true;
            #endregion
        }
        catch { }
    }

    #region colorder
    protected void lnkcolorder_Click(object sender, EventArgs e)
    {
        loadcolumnorder();
        loadcolumns();
        divcolorder.Attributes.Add("Style", "height: 100%; display:block; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;left: 0px;");
        // divcolorder.Visible = true;
    }
    public void loadcolumnorder()
    {
        cblcolumnorder.Items.Clear();
        cblcolumnorder.Items.Add(new ListItem("Student Name", "1"));
        cblcolumnorder.Items.Add(new ListItem("Roll No", "2"));
        cblcolumnorder.Items.Add(new ListItem("Reg No", "3"));
        cblcolumnorder.Items.Add(new ListItem("Admission No", "4"));
        cblcolumnorder.Items.Add(new ListItem(lbldeg.Text, "5"));
        //   cblcolumnorder.Items.Add(new ListItem("Semester", "6"));
        cblcolumnorder.Items.Add(new ListItem("Receipt No", "6"));
        cblcolumnorder.Items.Add(new ListItem("Receipt Date", "7"));
        // cblcolumnorder.Items.Add(new ListItem("Fee Type", "8"));

    }

    protected void btncolorderOK_Click(object sender, EventArgs e)
    {
        divcolorder.Visible = false;
        loadcolumns();
    }

    public bool columncount()
    {
        bool colorder = false;
        try
        {
            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
            {
                if (cblcolumnorder.Items[i].Selected == true)
                {
                    colorder = true;
                }
            }
        }
        catch { }
        return colorder;
    }
    public void loadcolumns()
    {
        try
        {
            string linkname = "DFCR column order settings";
            string columnvalue = "";
            int clsupdate = 0;
            DataSet dscol = new DataSet();
            string selcol = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "' and college_code='" + collegecode + "' ";
            dscol.Clear();
            dscol = d2.select_method_wo_parameter(selcol, "Text");
            if (columncount() == true)
            {
                if (cblcolumnorder.Items.Count > 0)
                {
                    colord.Clear();
                    for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                    {
                        if (cblcolumnorder.Items[i].Selected == true)
                        {
                            colord.Add(Convert.ToString(cblcolumnorder.Items[i].Value));
                            if (columnvalue == "")
                                columnvalue = Convert.ToString(cblcolumnorder.Items[i].Value);
                            else
                                columnvalue = columnvalue + ',' + Convert.ToString(cblcolumnorder.Items[i].Value);
                        }
                    }
                }
            }
            else if (dscol.Tables.Count > 0)
            {
                if (dscol.Tables[0].Rows.Count > 0)
                {
                    colord.Clear();
                    for (int col = 0; col < dscol.Tables[0].Rows.Count; col++)
                    {
                        string value = Convert.ToString(dscol.Tables[0].Rows[col]["LinkValue"]);
                        string[] valuesplit = value.Split(',');
                        if (valuesplit.Length > 0)
                        {
                            for (int k = 0; k < valuesplit.Length; k++)
                            {
                                colord.Add(Convert.ToString(valuesplit[k]));
                                if (columnvalue == "")
                                    columnvalue = Convert.ToString(valuesplit[k]);
                                else
                                    columnvalue = columnvalue + ',' + Convert.ToString(valuesplit[k]);
                            }
                        }
                    }
                }

            }
            else
            {
                colord.Clear();
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = true;
                    colord.Add(Convert.ToString(cblcolumnorder.Items[i].Value));
                    if (columnvalue == "")
                        columnvalue = Convert.ToString(cblcolumnorder.Items[i].Value);
                    else
                        columnvalue = columnvalue + ',' + Convert.ToString(cblcolumnorder.Items[i].Value);
                }
            }
            if (columnvalue != "" && columnvalue != null)
            {
                string clsinsert = " if exists(select * from New_InsSettings where LinkName='" + linkname + "') update New_InsSettings set LinkValue='" + columnvalue + "' where LinkName='" + linkname + "' and user_code='" + usercode + "' and college_code='" + collegecode + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('" + linkname + "','" + columnvalue + "','" + usercode + "','" + collegecode + "')";
                clsupdate = d2.update_method_wo_parameter(clsinsert, "Text");
            }
            if (clsupdate == 1)
            {
                string sel = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "' and college_code='" + collegecode + "' ";
                DataSet dscolor = new DataSet();
                dscolor.Clear();
                dscolor = d2.select_method_wo_parameter(sel, "Text");
                if (dscolor.Tables.Count > 0)
                {
                    int count = 0;
                    if (dscolor.Tables[0].Rows.Count > 0)
                    {
                        string value = Convert.ToString(dscolor.Tables[0].Rows[0]["LinkValue"]);
                        string[] value1 = value.Split(',');
                        if (value1.Length > 0)
                        {
                            for (int i = 0; i < value1.Length; i++)
                            {
                                string val = value1[i].ToString();
                                for (int k = 0; k < cblcolumnorder.Items.Count; k++)
                                {
                                    if (val == cblcolumnorder.Items[k].Value)
                                    {
                                        cblcolumnorder.Items[k].Selected = true;
                                        count++;
                                    }
                                    if (count == cblcolumnorder.Items.Count)
                                        cb_column.Checked = true;
                                    else
                                        cb_column.Checked = false;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch { }
    }
    #endregion

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        alertDiv.Visible = false;
    }

    #region Print
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(spreadDet, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your  Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        { }
    }

    public void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            string degreedetails;
            string pagename;
            degreedetails = "Daily Fees Structure Report" + '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            pagename = "DailyFeesCollectionReport.aspx";
            Printcontrolhed.loadspreaddetails(spreadDet, pagename, degreedetails);
            Printcontrolhed.Visible = true;
        }
        catch { }
    }
    #endregion

    #region roll,reg,admission setting
    private void RollAndRegSettings()
    {
        try
        {
            DataSet dsl = new DataSet();
            string Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
            dsl = d2.select_method_wo_parameter(Master1, "text");
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

    protected void spreadColumnVisible()
    {
        try
        {
            #region student wise
            if (roll == 0)
            {
                spreadDet.Columns[2].Visible = true;
                spreadDet.Columns[3].Visible = true;
                spreadDet.Columns[4].Visible = true;
            }
            else if (roll == 1)
            {
                spreadDet.Columns[2].Visible = true;
                spreadDet.Columns[3].Visible = true;
                spreadDet.Columns[4].Visible = true;
            }
            else if (roll == 2)
            {
                spreadDet.Columns[2].Visible = true;
                spreadDet.Columns[3].Visible = false;
                spreadDet.Columns[4].Visible = false;

            }
            else if (roll == 3)
            {
                spreadDet.Columns[2].Visible = false;
                spreadDet.Columns[3].Visible = true;
                spreadDet.Columns[4].Visible = false;
            }
            else if (roll == 4)
            {
                spreadDet.Columns[2].Visible = false;
                spreadDet.Columns[3].Visible = false;
                spreadDet.Columns[4].Visible = true;
            }
            else if (roll == 5)
            {
                spreadDet.Columns[2].Visible = true;
                spreadDet.Columns[3].Visible = true;
                spreadDet.Columns[4].Visible = false;
            }
            else if (roll == 6)
            {
                spreadDet.Columns[2].Visible = false;
                spreadDet.Columns[3].Visible = true;
                spreadDet.Columns[4].Visible = true;
            }
            else if (roll == 7)
            {
                spreadDet.Columns[2].Visible = true;
                spreadDet.Columns[3].Visible = false;
                spreadDet.Columns[4].Visible = true;
            }
            #endregion
        }
        catch { }
    }

    #endregion

    //user based reports
    protected void UserbasedRights()
    {
        string userrht = d2.GetFunction("select value from Master_Settings where settings='Finance Include User Based Report Settings'  and usercode='" + usercode + "'");
        if (userrht == "1")
            usBasedRights = true;
        else
            usBasedRights = false;

    }
    protected string ArrearLinkValue()
    {
        string Linkvalue = string.Empty;
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();

            Linkvalue = d2.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + group_user + "' and college_code ='" + collegecode + "'");
        }
        else
        {
            Linkvalue = d2.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'");
        }
        return Linkvalue;
    }
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
    protected string sclSett()
    {
        string sclType = string.Empty;
        sclType = d2.GetFunction("select value from Master_Settings where settings='schoolorcollege' and usercode='" + usercode + "'");
        return sclType;
    }
    public void challanAndReceiptNoRights()
    {
        try
        {
            string query = "";
            string Master1 = "";
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
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = Convert.ToString(ds.Tables[0].Rows[i]["value"]);
                    if (val == "1")
                        rightscode = 1;
                    else if (val == "2")
                        rightscode = 2;
                    else if (val == "3")
                        rightscode = 3;
                    else
                        rightscode = 0;
                }
            }
        }
        catch
        { }
    }

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

        lbl.Add(lblclg);
        lbl.Add(lbltype);
        lbl.Add(lbldeg);
        lbl.Add(lbldept);
        lbl.Add(lblsem);
        fields.Add(0);
        fields.Add(1);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);

        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }

    #region student type
    protected void cb_type_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_type, cbl_type, txt_type, "Type", "--Select--");
    }
    protected void cbl_type_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_type, cbl_type, txt_type, "Type", "--Select--");
    }
    public void loadStudenttypes()
    {

        try
        {

            cbl_type.Items.Clear();

            string type = "";
            string deptquery = "select distinct case when mode =1 then 'Regular' when mode =3 then 'Lateral'  when mode =2 then 'Transfer' end as Modename,mode from Registration r,Degree g where r.degree_code = g.Degree_Code and g.college_code in('" + collegecode + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_type.DataSource = ds;
                cbl_type.DataTextField = "Modename";
                cbl_type.DataValueField = "mode";
                cbl_type.DataBind();

                if (cbl_type.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_type.Items.Count; i++)
                    {
                        cbl_type.Items[i].Selected = true;
                        type = Convert.ToString(cbl_type.Items[i].Text);
                    }
                    if (cbl_type.Items.Count == 1)
                    {
                        txt_type.Text = "Type(" + type + ")";
                    }
                    else
                    {
                        txt_type.Text = "Type(" + cbl_type.Items.Count + ")";
                    }
                    cb_type.Checked = true;
                }
            }
            else
            {
                txt_type.Text = "--Select--";

            }
        }
        catch
        {
        }

    }

    protected void loadStudenttype()
    {
        try
        {
            cbl_type.Items.Clear();
            if (checkSchoolSetting() == 0)
            {
                cbl_type.Items.Add(new ListItem("Old Studnent", "1"));
                cbl_type.Items.Add(new ListItem("New    ", "3"));
                // cbl_type.Items.Add(new ListItem("Transfer", "2"));
            }
            else
            {
                cbl_type.Items.Add(new ListItem("Regular", "1"));
                cbl_type.Items.Add(new ListItem("Lateral", "3"));
                cbl_type.Items.Add(new ListItem("Transfer", "2"));
                cbl_type.Items.Add(new ListItem("IrRegular", "4"));
            }
            if (cbl_type.Items.Count > 0)
            {
                for (int i = 0; i < cbl_type.Items.Count; i++)
                {
                    cbl_type.Items[i].Selected = true;
                }
                cb_type.Checked = true;
                txt_type.Text = "Type(" + cbl_type.Items.Count + ")";
            }
        }
        catch { }
    }

    protected Dictionary<string, string> getstudMode()
    {
        Dictionary<string, string> studMode = new Dictionary<string, string>();
        for (int i = 0; i < cbl_type.Items.Count; i++)
        {
            studMode.Add(cbl_type.Items[i].Text, cbl_type.Items[i].Value);
        }
        return studMode;
    }
    #endregion

    //added by sudhagar 08.02.2017
    private double checkSchoolSetting()
    {
        double getVal = 0;
        double.TryParse(Convert.ToString(d2.GetFunction("select  value from Master_Settings where settings='schoolorcollege' and usercode='" + usercode + "'")), out getVal);
        return getVal;
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //added by sudhagar  09.06.2017
    protected DataSet getDateset()
    {
        DataSet dsload = new DataSet();
        try
        {
            #region get value
            DateTime fromdt;
            DateTime todt;
            DataSet lgacr = new DataSet();
            string studType = string.Empty;
            string stream = string.Empty;
            string fromdate = string.Empty;
            string todate = string.Empty;
            string bankcode = string.Empty;
            string delflg = string.Empty;
            string collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            if (ddltype.Enabled == true)
            {
                if (ddltype.SelectedItem.Text != "All" && ddltype.SelectedItem.Text != "")
                    stream = Convert.ToString(ddltype.SelectedItem.Text);
            }
            string batch = Convert.ToString(getCblSelectedValue(cbl_batch));
            string degree = Convert.ToString(getCblSelectedValue(cbl_dept));
            if (ddlstudtype.SelectedItem.Text != "All" && ddlstudtype.SelectedItem.Text != "")
                studType = Convert.ToString(ddlstudtype.SelectedItem.Value);
            string feesType = string.Empty;
            feesType = Convert.ToString(ddlfeetype.SelectedItem.Text);
            string feecat = Convert.ToString(getCblSelectedValue(cbl_sem));
            string payMode = Convert.ToString(getCblSelectedValue(chkl_paid));
            //string fnlYear = Convert.ToString(getCblSelectedValue(chklsfyear));
            string fnlYear = Convert.ToString(ddlfinlyear.SelectedItem.Value);
            string headerValue = string.Empty;
            string ledgerValue = string.Empty;
            string strtype = string.Empty;
            string ledgerFK = string.Empty;
            if (ddlacctype.SelectedIndex == 0 || ddlacctype.SelectedIndex == 1)
            {
                headerValue = Convert.ToString(getCblSelectedValue(cblheader));
                strtype = ",headerfk";
                ledgerFK = Convert.ToString(getCblSelectedValue(chkl_studled));
            }
            else
            {
                headerValue = ledgervalue();
                strtype = ",ledgerfk";
            }

            string studMode = Convert.ToString(getCblSelectedValue(cbl_type));


            #region include

            if (cbdate.Checked)
            {
                fromdate = txt_fromdate.Text;
                todate = txt_todate.Text;
                string[] frdate = fromdate.Split('/');
                if (frdate.Length == 3)
                    fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
                string[] tdate = todate.Split('/');
                if (tdate.Length == 3)
                    todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
                fromdt = Convert.ToDateTime(fromdate);
                todt = Convert.ToDateTime(todate);
            }
            if (cbbankcheck.Checked)
            {
                for (int i = 0; i < chkl_paid.Items.Count; i++)
                {
                    if (chkl_paid.Items[i].Selected == true && chkl_paid.Items[i].Text == "Online Pay")
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
            }
            #endregion
            string strInclude = getStudCategory();
            string applynStr = " AND r.IsConfirm = 1  and isnull(is_enroll,'0')<>'2'";
            //  dtPaidHeader(strtype, batch, degree, feecat, collegecode, studMode, strInclude, headerValue, fnlYear, applynStr, fromdate, todate, payMode, headerValue);
            #endregion
            string selQ = "";
            if (ddlacctype.SelectedItem.Text.Trim() == "Header")//header
            {
                if (ddlfeetype.SelectedItem.Text.Trim() == "Paid")//paid
                {
                    if (!cbRcpt.Checked)//without receipt no
                    {
                        #region without receipt no
                        //allot total student strength
                        selQ = "  select distinct app_no, (stud_name+'-'+(case when mode='1' then '(O)' when mode='2' then '(T)'when mode='3' then '(N)' end)) as stud_name,Current_Semester,Reg_No,Roll_No,degree_code,roll_admit,batch_year from (";
                        selQ += "  select  r.app_no, r.stud_name,r.mode,r.Current_Semester,r.Reg_No,r.Roll_No,r.degree_code,r.roll_admit,r.batch_year from ft_feeallot a,registration r where  a.app_no=r.app_no and isnull(a.totalamount,'0')>0  and isnull(a.Paidamount,'0')>0   and r.college_code ='" + collegecode + "' " + strInclude + " and r.batch_year  in ('" + batch + "') and r.degree_code  in ('" + degree + "') and a.FeeCategory  in ('" + feecat + "') and headerfk   in ('" + headerValue + "') and a.ledgerfk in('" + ledgerFK + "') and a.FinYearFK   in ('" + fnlYear + "') and r.mode in('" + studMode + "')  ";
                        if (cbBeforAdm.Checked)
                        {
                            selQ += " union all select  r.app_no, r.stud_name,r.mode,r.Current_Semester,''Reg_No,''Roll_No,r.degree_code,''roll_admit,r.batch_year from ft_feeallot a,applyn r where  a.app_no=r.app_no and isnull(a.totalamount,'0')>0  and isnull(a.Paidamount,'0')>0   and r.college_code ='" + collegecode + "'  and r.batch_year  in ('" + batch + "') and r.degree_code  in ('" + degree + "') and a.FeeCategory  in ('" + feecat + "') and headerfk   in ('" + headerValue + "') and a.ledgerfk in('" + ledgerFK + "') and a.FinYearFK   in ('" + fnlYear + "') and r.mode in('" + studMode + "') " + applynStr + " ";
                        }
                        selQ += " ) tbl order by Batch_Year,degree_code,Current_Semester,Stud_Name ";

                        //stud paid list
                        selQ += "  select distinct sum(debit) as paid, app_no,headerfk,feecategory,ActualFinYearFk from(";
                        selQ += "  select debit, a.app_no,headerfk,feecategory,ActualFinYearFk  from ft_findailytransaction a,registration r where  a.app_no = r.app_no and isnull(debit,'0')>0 and isnull(actualfinyearfk,'0')<>'0'  and isnull(IsCanceled,'0')='0' and isnull(isdeposited,'0')='1' and  ISNULL(IsCollected,0)='1' and isnull(transcode,'')<>''  " + strInclude + " and r.batch_year  in ('" + batch + "') and r.degree_code  in ('" + degree + "') and a.FeeCategory  in ('" + feecat + "') and headerfk   in ('" + headerValue + "') and a.ledgerfk in('" + ledgerFK + "') and a.ActualFinYearFk  in ('" + fnlYear + "') and r.mode in('" + studMode + "') and a.paymode in ('" + payMode + "')  and r.college_code ='" + collegecode + "'";
                        if (cbdate.Checked)
                            selQ += " and Transdate between '" + fromdate + "' and '" + todate + "'";

                        if (cbBeforAdm.Checked)
                        {
                            selQ += " union all  select  debit, a.app_no,headerfk,feecategory,ActualFinYearFk  from ft_findailytransaction a,applyn r where  a.app_no = r.app_no and isnull(debit,'0')>0 and isnull(actualfinyearfk,'0')<>'0'  and isnull(IsCanceled,'0')='0' and isnull(isdeposited,'0')='1' and  ISNULL(IsCollected,0)='1' and isnull(transcode,'')<>''  " + strInclude + " and r.batch_year  in ('" + batch + "') and r.degree_code  in ('" + degree + "') and a.FeeCategory  in ('" + feecat + "') and headerfk   in ('" + headerValue + "') and a.ledgerfk in('" + ledgerFK + "') and a.ActualFinYearFk  in ('" + fnlYear + "') and r.mode in('" + studMode + "') and a.paymode in ('" + payMode + "')  and r.college_code ='" + collegecode + "' " + strInclude + "";
                            if (cbdate.Checked)
                                selQ += " and Transdate between '" + fromdate + "' and '" + todate + "'";
                        }
                        selQ += ")tbl group by app_no,headerfk,feecategory,ActualFinYearFk ";
                        #endregion



                        //selQ = "  select distinct r.app_no, (r.stud_name+'-'+(case when mode='1' then '(O)' when mode='2' then '(T)'when mode='3' then '(N)' end)) as stud_name,r.Current_Semester,r.Reg_No,r.Roll_No,r.degree_code,r.roll_admit,r.batch_year from ft_feeallot a,registration r where  a.app_no=r.app_no and isnull(a.totalamount,'0')>0  and isnull(a.Paidamount,'0')>0   and r.college_code ='" + collegecode + "' " + strInclude + " and r.batch_year  in ('" + batch + "') and r.degree_code  in ('" + degree + "') and a.FeeCategory  in ('" + feecat + "') and headerfk   in ('" + headerValue + "') and a.FinYearFK   in ('" + fnlYear + "') and r.mode in('" + studMode + "') order by r.Batch_Year,r.degree_code,r.Current_Semester,r.Stud_Name ";

                        ////stud paid list
                        //selQ += "  select distinct sum(debit) as paid, a.app_no,headerfk ,ActualFinYearFk,feecategory     from ft_findailytransaction a,registration r where  a.app_no = r.app_no and isnull(debit,'0')>0 and isnull(actualfinyearfk,'0')<>'0'  and isnull(IsCanceled,'0')='0' and isnull(isdeposited,'0')='1' and  ISNULL(IsCollected,0)='1' and isnull(transcode,'')<>''  " + strInclude + " and r.batch_year  in ('" + batch + "') and r.degree_code  in ('" + degree + "') and a.FeeCategory  in ('" + feecat + "') and headerfk   in ('" + headerValue + "') and a.ActualFinYearFk  in ('" + fnlYear + "') and r.mode in('" + studMode + "') and a.paymode in ('" + payMode + "')  and r.college_code ='" + collegecode + "' " + strInclude + "";
                        //if (cbdate.Checked)
                        //    selQ += " and Transdate between '" + fromdate + "' and '" + todate + "'";
                        //selQ += " group by a.app_no,headerfk ,ActualFinYearFk,feecategory ";
                    }
                    else
                    {

                        #region without receipt no
                        //allot total student strength
                        selQ = "  select distinct app_no, (stud_name+'-'+(case when mode='1' then '(O)' when mode='2' then '(T)'when mode='3' then '(N)' end)) as stud_name,Current_Semester,Reg_No,Roll_No,degree_code,roll_admit,batch_year from (";
                        selQ += "  select  r.app_no, r.stud_name,r.mode,r.Current_Semester,r.Reg_No,r.Roll_No,r.degree_code,r.roll_admit,r.batch_year from ft_feeallot a,registration r where  a.app_no=r.app_no and isnull(a.totalamount,'0')>0  and isnull(a.Paidamount,'0')>0   and r.college_code ='" + collegecode + "' " + strInclude + " and r.batch_year  in ('" + batch + "') and r.degree_code  in ('" + degree + "') and a.FeeCategory  in ('" + feecat + "') and headerfk   in ('" + headerValue + "') and a.ledgerfk in('" + ledgerFK + "') and a.FinYearFK   in ('" + fnlYear + "') and r.mode in('" + studMode + "')  ";
                        if (cbBeforAdm.Checked)
                        {
                            selQ += " union all select  r.app_no, r.stud_name,r.mode,r.Current_Semester,''Reg_No,''Roll_No,r.degree_code,''roll_admit,r.batch_year from ft_feeallot a,applyn r where  a.app_no=r.app_no and isnull(a.totalamount,'0')>0  and isnull(a.Paidamount,'0')>0   and r.college_code ='" + collegecode + "'  and r.batch_year  in ('" + batch + "') and r.degree_code  in ('" + degree + "') and a.FeeCategory  in ('" + feecat + "') and headerfk   in ('" + headerValue + "') and a.ledgerfk in('" + ledgerFK + "') and a.FinYearFK   in ('" + fnlYear + "') and r.mode in('" + studMode + "') " + applynStr + " ";
                        }
                        selQ += " ) tbl order by Batch_Year,degree_code,Current_Semester,Stud_Name ";

                        //stud paid list
                        selQ += "  select distinct sum(debit) as paid, app_no,headerfk,feecategory,ActualFinYearFk,Transcode,convert(varchar(10),transdate,103) as  transdate from(";
                        selQ += "  select debit, a.app_no,headerfk,feecategory,ActualFinYearFk,Transcode,convert(varchar(10),transdate,103) as  transdate  from ft_findailytransaction a,registration r where  a.app_no = r.app_no and isnull(debit,'0')>0 and isnull(actualfinyearfk,'0')<>'0'  and isnull(IsCanceled,'0')='0' and isnull(isdeposited,'0')='1' and  ISNULL(IsCollected,0)='1' and isnull(transcode,'')<>''  " + strInclude + " and r.batch_year  in ('" + batch + "') and r.degree_code  in ('" + degree + "') and a.FeeCategory  in ('" + feecat + "') and headerfk   in ('" + headerValue + "') and a.ledgerfk in('" + ledgerFK + "') and a.ActualFinYearFk  in ('" + fnlYear + "') and r.mode in('" + studMode + "') and a.paymode in ('" + payMode + "')  and r.college_code ='" + collegecode + "'";
                        if (cbdate.Checked)
                            selQ += " and Transdate between '" + fromdate + "' and '" + todate + "'";

                        if (cbBeforAdm.Checked)
                        {
                            selQ += " union all  select  debit, a.app_no,headerfk,feecategory,ActualFinYearFk,Transcode,convert(varchar(10),transdate,103) as  transdate  from ft_findailytransaction a,applyn r where  a.app_no = r.app_no and isnull(debit,'0')>0 and isnull(actualfinyearfk,'0')<>'0'  and isnull(IsCanceled,'0')='0' and isnull(isdeposited,'0')='1' and  ISNULL(IsCollected,0)='1' and isnull(transcode,'')<>''  " + strInclude + " and r.batch_year  in ('" + batch + "') and r.degree_code  in ('" + degree + "') and a.FeeCategory  in ('" + feecat + "') and headerfk   in ('" + headerValue + "') and a.ledgerfk in('" + ledgerFK + "') and a.ActualFinYearFk  in ('" + fnlYear + "') and r.mode in('" + studMode + "') and a.paymode in ('" + payMode + "')  and r.college_code ='" + collegecode + "' " + strInclude + "";
                            if (cbdate.Checked)
                                selQ += " and Transdate between '" + fromdate + "' and '" + todate + "'";
                        }
                        selQ += ")tbl group by app_no,headerfk,feecategory,ActualFinYearFk,Transcode,transdate ";
                        #endregion


                        //selQ = "  select distinct r.app_no, (r.stud_name+'-'+(case when mode='1' then '(O)' when mode='2' then '(T)'when mode='3' then '(N)' end)) as stud_name,r.Current_Semester,r.Reg_No,r.Roll_No,r.degree_code,r.roll_admit,r.batch_year from ft_feeallot a,registration r where  a.app_no=r.app_no and isnull(a.totalamount,'0')>0  and isnull(a.Paidamount,'0')>0   and r.college_code ='" + collegecode + "' " + strInclude + " and r.batch_year  in ('" + batch + "') and r.degree_code  in ('" + degree + "') and a.FeeCategory  in ('" + feecat + "') and headerfk   in ('" + headerValue + "') and a.FinYearFK   in ('" + fnlYear + "') and r.mode in('" + studMode + "') order by r.Batch_Year,r.degree_code,r.Current_Semester,r.Stud_Name ";

                        ////stud paid list
                        //selQ += "  select distinct sum(debit) as paid, a.app_no,headerfk ,ActualFinYearFk,feecategory,Transcode,convert(varchar(10),transdate,103) as  transdate   from ft_findailytransaction a,registration r where  a.app_no = r.app_no and isnull(debit,'0')>0 and isnull(actualfinyearfk,'0')<>'0'  and isnull(IsCanceled,'0')='0' and isnull(isdeposited,'0')='1' and  ISNULL(IsCollected,0)='1' and isnull(transcode,'')<>''  " + strInclude + " and r.batch_year  in ('" + batch + "') and r.degree_code  in ('" + degree + "') and a.FeeCategory  in ('" + feecat + "') and headerfk   in ('" + headerValue + "') and a.ActualFinYearFk  in ('" + fnlYear + "') and r.mode in('" + studMode + "') and a.paymode in ('" + payMode + "')  and r.college_code ='" + collegecode + "' " + strInclude + "";
                        //if (cbdate.Checked)
                        //    selQ += " and Transdate between '" + fromdate + "' and '" + todate + "'";
                        //selQ += " group by a.app_no,headerfk ,ActualFinYearFk,feecategory,Transcode,transdate  ";
                    }

                }
                else if (ddlfeetype.SelectedItem.Text.Trim() == "Yet To Be Paid")//yet to be paid
                {

                    #region yet to be paid
                    selQ = "select distinct app_no, (stud_name+'-'+(case when mode='1' then '(O)' when mode='2' then '(T)'when mode='3' then '(N)' end)) as stud_name,Current_Semester,Reg_No,Roll_No,degree_code,roll_admit,batch_year from (";
                    selQ += "  select  r.app_no, r.stud_name,r.mode,r.Current_Semester,r.Reg_No,r.Roll_No,r.degree_code,r.roll_admit,r.batch_year from ft_feeallot a,registration r where  a.app_no=r.app_no and isnull(a.totalamount,'0')>0  and isnull(a.balamount,'0')>0  and r.college_code ='" + collegecode + "' " + strInclude + " and r.batch_year  in ('" + batch + "') and r.degree_code  in ('" + degree + "') and a.FeeCategory  in ('" + feecat + "') and headerfk   in ('" + headerValue + "') and a.ledgerfk in('" + ledgerFK + "') and a.FinYearFK   in ('" + fnlYear + "') and r.mode in('" + studMode + "') " + strInclude + " ";
                    if (cbBeforAdm.Checked)
                    {
                        selQ += " union all select  r.app_no, r.stud_name,r.mode,r.Current_Semester,''Reg_No,''Roll_No,r.degree_code,''roll_admit,r.batch_year from ft_feeallot a,applyn r where  a.app_no=r.app_no and isnull(a.totalamount,'0')>0  and isnull(a.balamount,'0')>0  and r.college_code ='" + collegecode + "' " + strInclude + " and r.batch_year  in ('" + batch + "') and r.degree_code  in ('" + degree + "') and a.FeeCategory  in ('" + feecat + "') and headerfk   in ('" + headerValue + "') and a.ledgerfk in('" + ledgerFK + "') and a.FinYearFK   in ('" + fnlYear + "') and r.mode in('" + studMode + "') " + applynStr + " ";
                    }
                    selQ += " ) tbl order by Batch_Year,degree_code,Current_Semester,Stud_Name ";

                    //allot get student total amoun
                    selQ += " select distinct sum(totalamount) as total,sum(paidamount) as paid, app_no,headerfk,feecategory,FinYearFK from (";
                    selQ += "  select totalamount,paidamount, a.app_no,headerfk ,feecategory,FinYearFK from ft_feeallot a,registration r where  a.app_no=r.app_no and isnull(a.totalamount,'0')>0  and isnull(a.balamount,'0')>0   and r.college_code ='" + collegecode + "' " + strInclude + " and r.batch_year  in ('" + batch + "') and r.degree_code  in ('" + degree + "') and a.FeeCategory  in ('" + feecat + "') and headerfk   in ('" + headerValue + "') and a.ledgerfk in('" + ledgerFK + "') and a.FinYearFK   in ('" + fnlYear + "') and r.mode in('" + studMode + "') " + strInclude + "  ";
                    if (cbBeforAdm.Checked)
                    {
                        //allot get student total amoun
                        selQ += " union all  select totalamount,paidamount, a.app_no,headerfk ,feecategory,FinYearFK from ft_feeallot a,applyn r where  a.app_no=r.app_no and isnull(a.totalamount,'0')>0  and isnull(a.balamount,'0')>0   and r.college_code ='" + collegecode + "' " + strInclude + " and r.batch_year  in ('" + batch + "') and r.degree_code  in ('" + degree + "') and a.FeeCategory  in ('" + feecat + "') and headerfk   in ('" + headerValue + "') and a.ledgerfk in('" + ledgerFK + "') and a.FinYearFK   in ('" + fnlYear + "') and r.mode in('" + studMode + "') " + applynStr + "";
                    }
                    selQ += " ) tbl group by app_no,headerfk ,feecategory,FinYearFK";

                    //stud paid list
                    selQ += " select distinct sum(debit) as paid,app_no,headerfk,feecategory,ActualFinYearFk from  (";
                    selQ += "  select debit, a.app_no,headerfk,feecategory ,ActualFinYearFk  from ft_findailytransaction a,registration r where  a.app_no = r.app_no and isnull(debit,'0')>0 and isnull(actualfinyearfk,'0')<>'0'  and isnull(IsCanceled,'0')='0' and isnull(isdeposited,'0')='1' and  ISNULL(IsCollected,0)='1' and isnull(transcode,'')<>''  " + strInclude + " and r.batch_year  in ('" + batch + "') and r.degree_code  in ('" + degree + "') and a.FeeCategory  in ('" + feecat + "') and headerfk   in ('" + headerValue + "') and a.ledgerfk in('" + ledgerFK + "') and a.ActualFinYearFk  in ('" + fnlYear + "') and r.mode in('" + studMode + "') and a.paymode in ('" + payMode + "')  and r.college_code ='" + collegecode + "' " + strInclude + " ";
                    if (cbBeforAdm.Checked)
                    {
                        //stud paid list
                        selQ += " union all  select  debit, a.app_no,headerfk,feecategory,ActualFinYearFk  from ft_findailytransaction a,applyn r where  a.app_no = r.app_no and isnull(debit,'0')>0 and isnull(actualfinyearfk,'0')<>'0'  and isnull(IsCanceled,'0')='0' and isnull(isdeposited,'0')='1' and  ISNULL(IsCollected,0)='1' and isnull(transcode,'')<>''  " + strInclude + " and r.batch_year  in ('" + batch + "') and r.degree_code  in ('" + degree + "') and a.FeeCategory  in ('" + feecat + "') and headerfk   in ('" + headerValue + "') and a.ledgerfk in('" + ledgerFK + "') and a.ActualFinYearFk  in ('" + fnlYear + "') and r.mode in('" + studMode + "') and a.paymode in ('" + payMode + "')  and r.college_code ='" + collegecode + "' " + applynStr + " ";
                    }
                    selQ += ") tbl group by app_no,headerfk,feecategory,ActualFinYearFk ";
                    #endregion


                    //selQ = "  select distinct r.app_no, (r.stud_name+'-'+(case when mode='1' then '(O)' when mode='2' then '(T)'when mode='3' then '(N)' end)) as stud_name,r.Current_Semester,r.Reg_No,r.Roll_No,r.degree_code,r.roll_admit,r.batch_year from ft_feeallot a,registration r where  a.app_no=r.app_no and isnull(a.totalamount,'0')>0  and isnull(a.balamount,'0')>0  and r.college_code ='" + collegecode + "' " + strInclude + " and r.batch_year  in ('" + batch + "') and r.degree_code  in ('" + degree + "') and a.FeeCategory  in ('" + feecat + "') and headerfk   in ('" + headerValue + "') and a.FinYearFK   in ('" + fnlYear + "') and r.mode in('" + studMode + "') " + strInclude + " order by r.Batch_Year,r.degree_code,r.Current_Semester,r.Stud_Name ";

                    ////allot get student total amoun
                    //selQ += "  select distinct sum(totalamount) as total,sum(paidamount) as paid, a.app_no,headerfk ,FinYearFK,feecategory from ft_feeallot a,registration r where  a.app_no=r.app_no and isnull(a.totalamount,'0')>0  and isnull(a.balamount,'0')>0   and r.college_code ='" + collegecode + "' " + strInclude + " and r.batch_year  in ('" + batch + "') and r.degree_code  in ('" + degree + "') and a.FeeCategory  in ('" + feecat + "') and headerfk   in ('" + headerValue + "') and a.FinYearFK   in ('" + fnlYear + "') and r.mode in('" + studMode + "') " + strInclude + " group by a.app_no,headerfk ,FinYearFK,feecategory ";

                    ////stud paid list
                    //selQ += "  select distinct sum(debit) as paid, a.app_no,headerfk ,ActualFinYearFk,feecategory     from ft_findailytransaction a,registration r where  a.app_no = r.app_no and isnull(debit,'0')>0 and isnull(actualfinyearfk,'0')<>'0'  and isnull(IsCanceled,'0')='0' and isnull(isdeposited,'0')='1' and  ISNULL(IsCollected,0)='1' and isnull(transcode,'')<>''  " + strInclude + " and r.batch_year  in ('" + batch + "') and r.degree_code  in ('" + degree + "') and a.FeeCategory  in ('" + feecat + "') and headerfk   in ('" + headerValue + "') and a.ActualFinYearFk  in ('" + fnlYear + "') and r.mode in('" + studMode + "') and a.paymode in ('" + payMode + "')  and r.college_code ='" + collegecode + "' " + strInclude + " group by a.app_no,headerfk ,ActualFinYearFk,feecategory ";
                }
            }
            else//ledger
            {
                if (ddlfeetype.SelectedItem.Text.Trim() == "Paid")//paid
                {

                }
                else//yet to be paid
                {
                }
            }
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(selQ, "Text");
        }
        catch { }
        return dsload;
    }

    protected void loadSchoolspreadDetails(ref Hashtable htColCnt, ref Dictionary<string, string> dtSemCol)
    {
        try
        {
            #region design
            //roll no settins
            RollAndRegSettings();
            loadcolumns();
            spreadDet.Sheets[0].RowCount = 0;
            spreadDet.Sheets[0].ColumnCount = 0;
            spreadDet.CommandBar.Visible = false;
            spreadDet.Sheets[0].AutoPostBack = true;
            spreadDet.Sheets[0].RowHeader.Visible = false;
            spreadDet.Sheets[0].ColumnCount = 8;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            spreadDet.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Student Name";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            spreadDet.Sheets[0].Columns[1].Width = 350;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            spreadDet.Sheets[0].Columns[2].Visible = true;
            if (!colord.Contains("2"))
                spreadDet.Sheets[0].Columns[2].Visible = false;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg No";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            spreadDet.Sheets[0].Columns[3].Visible = true;
            if (!colord.Contains("3"))
                spreadDet.Sheets[0].Columns[3].Visible = false;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Admission No";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
            spreadDet.Sheets[0].Columns[4].Visible = true;
            if (!colord.Contains("4"))
                spreadDet.Sheets[0].Columns[4].Visible = false;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].Text = lbldeg.Text;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
            spreadDet.Sheets[0].Columns[5].Visible = true;
            if (!colord.Contains("5"))
                spreadDet.Sheets[0].Columns[5].Visible = false;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Receipt No";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Left;
            spreadDet.Sheets[0].Columns[6].Visible = true;
            if (!colord.Contains("6"))
                spreadDet.Sheets[0].Columns[6].Visible = false;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Receipt Date";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 7].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Left;
            spreadDet.Sheets[0].Columns[7].Visible = true;
            if (!colord.Contains("7"))
                spreadDet.Sheets[0].Columns[7].Visible = false;

            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);
            spreadSclColumnVisible();
            int pCnt = 0;
            bool boolfnltot = true;
            string strTotal = string.Empty;
            if (ddlfeetype.SelectedItem.Text == "Yet To Be Paid")
                strTotal = "Total Balance";
            else
                strTotal = "Total Paid";
            if (ddlacctype.SelectedIndex == 0)//header || ddlacctype.SelectedIndex == 1
            {
                #region group,header
                spreadDet.Sheets[0].ColumnHeader.RowCount = 2;
                pCnt = spreadDet.Sheets[0].ColumnCount++;
                bool checkbool = false;
                DataTable dtpaid = dtPaidHeader();
                Hashtable htHDName = getHeaderFK();
                for (int Trm = 0; Trm < cbl_sem.Items.Count; Trm++)
                {
                    int tempcnt = 0;
                    int checkva = 0;
                    bool boolCheck = false;
                    if (cbl_sem.Items[Trm].Selected)
                    {
                        string feecatVal = Convert.ToString(cbl_sem.Items[Trm].Value);
                        dtpaid.DefaultView.RowFilter = "feecategory='" + feecatVal + "'";
                        DataTable dtPaidHd = dtpaid.DefaultView.ToTable();
                        if (dtPaidHd.Rows.Count > 0)
                        {
                            for (int hd = 0; hd < dtPaidHd.Rows.Count; hd++)
                            {
                                checkva++;
                                if (checkva > 1)
                                    tempcnt = spreadDet.Sheets[0].ColumnCount++;
                                if (!boolCheck && pCnt == 0)
                                    tempcnt = spreadDet.Sheets[0].ColumnCount++;
                                if (pCnt == 0)
                                    pCnt = tempcnt;

                                boolCheck = true;
                                string hdName = Convert.ToString(htHDName[Convert.ToString(dtPaidHd.Rows[hd]["headerfk"]).Trim()]);
                                htColCnt.Add(feecatVal + "-" + Convert.ToString(dtPaidHd.Rows[hd]["headerfk"]), spreadDet.Sheets[0].ColumnCount - 1);
                                dtSemCol.Add(feecatVal + "-" + Convert.ToString(dtPaidHd.Rows[hd]["headerfk"]), Convert.ToString(spreadDet.Sheets[0].ColumnCount - 1));
                                spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Text = hdName;
                                spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(dtPaidHd.Rows[hd]["headerfk"]);
                                spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                checkbool = true;
                            }
                            if (checkbool)
                            {
                                spreadDet.Sheets[0].ColumnHeader.Cells[0, pCnt].Text = Convert.ToString(cbl_sem.Items[Trm].Text);
                                spreadDet.Sheets[0].ColumnHeader.Cells[0, pCnt].ForeColor = ColorTranslator.FromHtml("#000000");
                                spreadDet.Sheets[0].ColumnHeader.Cells[0, pCnt].Font.Bold = true;
                                spreadDet.Sheets[0].ColumnHeader.Cells[0, pCnt].Font.Name = "Book Antiqua";
                                spreadDet.Sheets[0].ColumnHeader.Cells[0, pCnt].Font.Size = FontUnit.Medium;
                                spreadDet.Sheets[0].ColumnHeader.Cells[0, pCnt].HorizontalAlign = HorizontalAlign.Center;
                                spreadDet.Sheets[0].Columns[pCnt].HorizontalAlign = HorizontalAlign.Right;
                                spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, pCnt, 1, checkva);
                                pCnt = 0;
                            }
                        }
                    }
                }
                #endregion
                if (boolfnltot)
                {
                    spreadDet.Sheets[0].ColumnCount++;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Text = strTotal;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, spreadDet.Sheets[0].ColumnCount - 1, 2, 1);
                }
            }
            #endregion
        }
        catch { }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        divcolorder.Attributes.Add("Style", "display:none;");
        txtexcelname.Text = string.Empty;
        lblvalidation1.Text = string.Empty;
        DateTime fromdt = new DateTime();
        DateTime todt = new DateTime();
        Hashtable htColCnt = new Hashtable();
        Dictionary<string, string> dtSemCol = new Dictionary<string, string>();
        loadSchoolspreadDetails(ref  htColCnt, ref dtSemCol);
        spreadDet.Visible = true;
        ds.Clear();
        if (checkSchoolSetting() == 0)
            ds = getDateset();
        else
            ds = getDatesetCollege();
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            //if (!cbdate.Checked)//without date
            //{
            if (ddlfeetype.SelectedItem.Text.Trim() == "Paid")
            {
                if (ddlacctype.SelectedItem.Text.Trim() == "Header")
                {
                    if (checkSchoolSetting() == 0)
                    {
                        if (!cbRcpt.Checked)
                            getPaidDet(ds, ref  htColCnt, ref dtSemCol);
                        else
                            getPaidDetRcptNo(ds, ref  htColCnt, ref dtSemCol);
                    }
                    else
                    {
                        if (!cbRcpt.Checked)
                            getPaidDetCollege(ds, ref  htColCnt, ref dtSemCol);
                        else
                            getPaidDetRcptNoCollege(ds, ref  htColCnt, ref dtSemCol);
                    }
                }
                else//ledger
                {
                }
            }
            else if (ddlfeetype.SelectedItem.Text.Trim() == "Yet To Be Paid")
            {
                if (checkSchoolSetting() == 0)
                {
                    if (ddlacctype.SelectedItem.Text.Trim() == "Header")
                        getYetToBePaid(ds, ref  htColCnt, ref dtSemCol);
                }
                else
                {
                    if (ddlacctype.SelectedItem.Text.Trim() == "Header")
                        getYetToBePaidCollege(ds, ref  htColCnt, ref dtSemCol);
                }

            }
            // }
        }
        else
        {
            spreadDet.Visible = false;
            print.Visible = false;
            txtexcelname.Text = string.Empty;
            lblvalidation1.Text = string.Empty;
            divlabl.Visible = false;
            lbl_alert.Text = "No Record Found";
            alertDiv.Visible = true;
        }
        #region old
        //ds.Clear();
        //ds = getDetails(ref fromdt, ref todt);
        //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //{
        //    #region value
        //    if (!cbdate.Checked)
        //    {
        //        if (ddlacctype.SelectedItem.Text.Trim() == "Group Header")
        //        {
        //            if (ddlfeetype.SelectedItem.Text.Trim() == "Paid")
        //                groupHdPaidBalance(ds);
        //            else if (ddlfeetype.SelectedItem.Text.Trim() == "Yet To Be Paid")
        //                groupHdPaidBalance(ds);
        //            else if (ddlfeetype.SelectedItem.Text.Trim() == "Both")
        //                groupHdBoth(ds);
        //        }
        //        else if (ddlacctype.SelectedItem.Text.Trim() == "Header")
        //        {
        //            if (ddlfeetype.SelectedItem.Text.Trim() == "Paid")
        //                headerPaid(ds);
        //            else if (ddlfeetype.SelectedItem.Text.Trim() == "Yet To Be Paid")
        //                groupHdPaidBalance(ds);
        //            else if (ddlfeetype.SelectedItem.Text.Trim() == "Both")
        //                groupHdBoth(ds);
        //        }
        //        else
        //        {
        //            if (ddlfeetype.SelectedItem.Text.Trim() == "Paid")
        //                headerPaid(ds);
        //            else if (ddlfeetype.SelectedItem.Text.Trim() == "Yet To Be Paid")
        //                groupHdPaidBalance(ds);
        //            else if (ddlfeetype.SelectedItem.Text.Trim() == "Both")
        //                groupHdBoth(ds);
        //        }
        //    }
        //    else
        //    {
        //        if (ddlacctype.SelectedItem.Text.Trim() == "Group Header")
        //        {
        //            if (ddlfeetype.SelectedItem.Text.Trim() == "Paid")
        //                DatwiseGroupHdPaid(ds, fromdt, todt);
        //            else if (ddlfeetype.SelectedItem.Text.Trim() == "Yet To Be Paid")
        //                headerLedgerYetToBePaid(ds);
        //            else if (ddlfeetype.SelectedItem.Text.Trim() == "Both")
        //                groupHdPaidBalance(ds);
        //        }
        //        else if (ddlacctype.SelectedItem.Text.Trim() == "Header")
        //        {
        //            if (ddlfeetype.SelectedItem.Text.Trim() == "Paid")
        //                DatwiseGroupHdPaid(ds, fromdt, todt);
        //            else if (ddlfeetype.SelectedItem.Text.Trim() == "Yet To Be Paid")
        //                headerLedgerYetToBePaid(ds);
        //            //else if (ddlfeetype.SelectedItem.Text.Trim() == "Both")
        //            //    groupHdPaidBalance(ds);
        //        }
        //        else
        //        {
        //            if (ddlfeetype.SelectedItem.Text.Trim() == "Paid")
        //                DatwiseGroupHdPaid(ds, fromdt, todt);
        //            else if (ddlfeetype.SelectedItem.Text.Trim() == "Yet To Be Paid")
        //                headerLedgerYetToBePaid(ds);
        //            //else if (ddlfeetype.SelectedItem.Text.Trim() == "Both")
        //            //    groupHdPaidBalance(ds);
        //        }
        //    }
        //    #endregion
        //}
        //else
        //{
        //    spreadDet.Visible = false;
        //    print.Visible = false;
        //    txtexcelname.Text = string.Empty;
        //    lblvalidation1.Text = string.Empty;
        //    divlabl.Visible = false;
        //    lbl_alert.Text = "No Record Found";
        //    alertDiv.Visible = true;
        //}
        //// loadspreadDetails();
        #endregion
    }

    #region  School

    protected void getPaidDet(DataSet ds, ref Hashtable htColCnt, ref Dictionary<string, string> dtSemCol)
    {
        try
        {

            Hashtable htTempFnl = new Hashtable();
            Hashtable htFnlFnlYR = new Hashtable();
            Hashtable htDeptName = getDeptName();//deptment name
            DataTable dtpaidName = dtPaidHeader();//spread column header
            Hashtable finYear = getFinyear();//financial year
            Dictionary<string, string> htSelFnlYR = new Dictionary<string, string>();
            if (!htSelFnlYR.ContainsKey(Convert.ToString(ddlfinlyear.SelectedValue)))
                htSelFnlYR.Add(Convert.ToString(ddlfinlyear.SelectedValue), Convert.ToString(ddlfinlyear.SelectedItem.Text));
            int rowCnt = 0;
            int height = 0;
            FarPoint.Web.Spread.TextCellType txtroll = new FarPoint.Web.Spread.TextCellType();
            //for (int fnlYr = 0; fnlYr < chklsfyear.Items.Count; fnlYr++)
            //{
            foreach (KeyValuePair<string, string> dtFnlYr in htSelFnlYR)
            {
                bool boolFinlYr = false;
                //if (chklsfyear.Items[fnlYr].Selected)
                //{
                // string actFinYearFK = Convert.ToString(chklsfyear.Items[fnlYr].Value);
                string actFinYearFK = Convert.ToString(dtFnlYr.Key);
                DataTable dtPaid = ds.Tables[0].Copy();
                for (int dtRow = 0; dtRow < dtPaid.Rows.Count; dtRow++)
                {
                    if (!boolFinlYr)
                    {
                        spreadDet.Sheets[0].RowCount++;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(finYear[actFinYearFK]);
                        spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, spreadDet.Sheets[0].ColumnCount - 1);
                        spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
                        boolFinlYr = true;
                    }
                    string appNo = Convert.ToString(dtPaid.Rows[dtRow]["app_no"]);
                    bool studPaidStatus = true;
                    double totalAmount = 0;

                    #region header Amount
                    //semester and headerfk with column count added in dictionary while header binding
                    foreach (KeyValuePair<string, string> dtSemeHd in dtSemCol)
                    {
                        string feeCatg = Convert.ToString(dtSemeHd.Key).Split('-')[0];
                        string hdFK = Convert.ToString(dtSemeHd.Key).Split('-')[1];
                        // string hdFK = Convert.ToString(dtSemeHd.Key).Split('-')[1];
                        ds.Tables[1].DefaultView.RowFilter = "actualfinyearfk='" + actFinYearFK + "' and app_no='" + appNo + "' and feecategory='" + feeCatg + "' and headerfk='" + hdFK + "'";
                        DataTable dtHeader = ds.Tables[1].DefaultView.ToTable();
                        if (dtHeader.Rows.Count > 0)
                        {
                            for (int dtHd = 0; dtHd < dtHeader.Rows.Count; dtHd++)
                            {
                                int colCnt = 0;
                                int.TryParse(Convert.ToString(htColCnt[feeCatg + "-" + dtHeader.Rows[dtHd]["headerfk"]]), out colCnt);
                                double paidAmount = 0;
                                double.TryParse(Convert.ToString(dtHeader.Rows[dtHd]["paid"]), out paidAmount);
                                totalAmount += paidAmount;
                                if (studPaidStatus)//add new row when get paid amount only
                                    spreadDet.Sheets[0].RowCount++;
                                studPaidStatus = false;
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = Convert.ToString(paidAmount);
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Right;
                                if (!htTempFnl.ContainsKey(colCnt))
                                    htTempFnl.Add(colCnt, paidAmount);
                                else
                                {
                                    double total = 0;
                                    double.TryParse(Convert.ToString(htTempFnl[colCnt]), out total);
                                    total += paidAmount;
                                    htTempFnl.Remove(colCnt);
                                    htTempFnl.Add(colCnt, total);
                                }
                            }
                        }
                    }
                    if (totalAmount != 0)
                    {
                        height += 10;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(totalAmount);//end of the column total
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                        int colCnt = spreadDet.Sheets[0].ColumnCount - 1;
                        if (!htTempFnl.ContainsKey(colCnt))
                            htTempFnl.Add(colCnt, totalAmount);
                        else
                        {
                            double total = 0;
                            double.TryParse(Convert.ToString(htTempFnl[colCnt]), out total);
                            total += totalAmount;
                            htTempFnl.Remove(colCnt);
                            htTempFnl.Add(colCnt, total);
                        }

                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(++rowCnt);

                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dtPaid.Rows[dtRow]["stud_name"]);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dtPaid.Rows[dtRow]["roll_no"]);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dtPaid.Rows[dtRow]["Reg_No"]);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dtPaid.Rows[dtRow]["roll_admit"]);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].CellType = txtroll;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].CellType = txtroll;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].CellType = txtroll;
                        string deptName = Convert.ToString(htDeptName[Convert.ToString(dtPaid.Rows[dtRow]["degree_code"])]);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 5].Text = deptName;
                        spreadDet.Sheets[0].Columns[6].Visible = false;
                        spreadDet.Sheets[0].Columns[7].Visible = false;
                    }
                    #endregion
                }
                // }
                if (htTempFnl.Count > 0)//every financial year total
                {
                    #region Every Total
                    spreadDet.Sheets[0].Rows.Count++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Total";
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                    double grandAmt = 0;
                    for (int col = 8; col < spreadDet.Sheets[0].ColumnCount; col++)
                    {
                        double.TryParse(Convert.ToString(htTempFnl[col]), out grandAmt);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, col].Text = Convert.ToString(grandAmt);
                        if (!htFnlFnlYR.ContainsKey(col))//final total
                            htFnlFnlYR.Add(col, grandAmt);
                        else
                        {
                            double total = 0;
                            double.TryParse(Convert.ToString(htFnlFnlYR[col]), out total);
                            total += grandAmt;
                            htFnlFnlYR.Remove(col);
                            htFnlFnlYR.Add(col, total);
                        }
                    }
                    htTempFnl.Clear();
                    #endregion
                }
            }
            // }
            if (htFnlFnlYR.Count > 0)//final financial year total
            {
                #region Every Total
                spreadDet.Sheets[0].Rows.Count++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Gold;
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                double grandAmt = 0;
                for (int col = 8; col < spreadDet.Sheets[0].ColumnCount; col++)
                {
                    double.TryParse(Convert.ToString(htFnlFnlYR[col]), out grandAmt);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, col].Text = Convert.ToString(grandAmt);
                }
                #endregion
            }
            spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
            spreadDet.SaveChanges();
            spreadDet.Height = height;
            spreadDet.Visible = true;
            print.Visible = true;
            divlabl.Visible = true;
        }
        catch { }
    }

    protected void getPaidDetRcptNo(DataSet ds, ref Hashtable htColCnt, ref Dictionary<string, string> dtSemCol)
    {
        try
        {

            Hashtable htTempFnl = new Hashtable();
            Hashtable htFnlFnlYR = new Hashtable();
            Hashtable htDeptName = getDeptName();//deptment name
            DataTable dtpaidName = dtPaidHeader();//spread column header
            Hashtable finYear = getFinyear();//financial year
            Dictionary<string, string> htSelFnlYR = new Dictionary<string, string>();
            if (!htSelFnlYR.ContainsKey(Convert.ToString(ddlfinlyear.SelectedValue)))
                htSelFnlYR.Add(Convert.ToString(ddlfinlyear.SelectedValue), Convert.ToString(ddlfinlyear.SelectedItem.Text));
            int rowCnt = 0;
            int height = 0;
            FarPoint.Web.Spread.TextCellType txtroll = new FarPoint.Web.Spread.TextCellType();
            //for (int fnlYr = 0; fnlYr < chklsfyear.Items.Count; fnlYr++)
            //{
            foreach (KeyValuePair<string, string> dtFnlYr in htSelFnlYR)
            {
                bool boolFinlYr = false;
                //if (chklsfyear.Items[fnlYr].Selected)
                //{
                // string actFinYearFK = Convert.ToString(chklsfyear.Items[fnlYr].Value);
                string actFinYearFK = Convert.ToString(dtFnlYr.Key);
                DataTable dtPaid = ds.Tables[0].Copy();
                for (int dtRow = 0; dtRow < dtPaid.Rows.Count; dtRow++)
                {
                    if (!boolFinlYr)
                    {
                        spreadDet.Sheets[0].RowCount++;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(finYear[actFinYearFK]);
                        spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, spreadDet.Sheets[0].ColumnCount - 1);
                        spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
                        boolFinlYr = true;
                    }
                    string appNo = Convert.ToString(dtPaid.Rows[dtRow]["app_no"]);
                    bool studPaidStatus = true;
                    double totalAmount = 0;

                    #region header Amount
                    //semester and headerfk with column count added in dictionary while header binding
                    foreach (KeyValuePair<string, string> dtSemeHd in dtSemCol)
                    {
                        string feeCatg = Convert.ToString(dtSemeHd.Key).Split('-')[0];
                        string hdFK = Convert.ToString(dtSemeHd.Key).Split('-')[1];
                        // string hdFK = Convert.ToString(dtSemeHd.Key).Split('-')[1];
                        ds.Tables[1].DefaultView.RowFilter = "actualfinyearfk='" + actFinYearFK + "' and app_no='" + appNo + "' and feecategory='" + feeCatg + "' and headerfk='" + hdFK + "'";
                        DataTable dtHeader = ds.Tables[1].DefaultView.ToTable();
                        if (dtHeader.Rows.Count > 0)
                        {
                            for (int dtHd = 0; dtHd < dtHeader.Rows.Count; dtHd++)
                            {
                                int colCnt = 0;
                                int.TryParse(Convert.ToString(htColCnt[feeCatg + "-" + dtHeader.Rows[dtHd]["headerfk"]]), out colCnt);
                                double paidAmount = 0;
                                double.TryParse(Convert.ToString(dtHeader.Rows[dtHd]["paid"]), out paidAmount);
                                totalAmount += paidAmount;
                                if (studPaidStatus)//add new row when get paid amount only
                                    spreadDet.Sheets[0].RowCount++;
                                studPaidStatus = false;
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = Convert.ToString(paidAmount);
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dtHeader.Rows[dtHd]["Transcode"]);
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(dtHeader.Rows[dtHd]["transdate"]);
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Right;
                                if (!htTempFnl.ContainsKey(colCnt))
                                    htTempFnl.Add(colCnt, paidAmount);
                                else
                                {
                                    double total = 0;
                                    double.TryParse(Convert.ToString(htTempFnl[colCnt]), out total);
                                    total += paidAmount;
                                    htTempFnl.Remove(colCnt);
                                    htTempFnl.Add(colCnt, total);
                                }
                            }
                        }
                    }
                    if (totalAmount != 0)
                    {
                        height += 10;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(totalAmount);//end of the column total
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                        int colCnt = spreadDet.Sheets[0].ColumnCount - 1;
                        if (!htTempFnl.ContainsKey(colCnt))
                            htTempFnl.Add(colCnt, totalAmount);
                        else
                        {
                            double total = 0;
                            double.TryParse(Convert.ToString(htTempFnl[colCnt]), out total);
                            total += totalAmount;
                            htTempFnl.Remove(colCnt);
                            htTempFnl.Add(colCnt, total);
                        }

                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(++rowCnt);

                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dtPaid.Rows[dtRow]["stud_name"]);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dtPaid.Rows[dtRow]["roll_no"]);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dtPaid.Rows[dtRow]["Reg_No"]);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dtPaid.Rows[dtRow]["roll_admit"]);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].CellType = txtroll;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].CellType = txtroll;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].CellType = txtroll;
                        string deptName = Convert.ToString(htDeptName[Convert.ToString(dtPaid.Rows[dtRow]["degree_code"])]);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 5].Text = deptName;
                    }
                    #endregion
                }
                // }
                if (htTempFnl.Count > 0)//every financial year total
                {
                    #region Every Total
                    spreadDet.Sheets[0].Rows.Count++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Total";
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                    double grandAmt = 0;
                    for (int col = 8; col < spreadDet.Sheets[0].ColumnCount; col++)
                    {
                        double.TryParse(Convert.ToString(htTempFnl[col]), out grandAmt);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, col].Text = Convert.ToString(grandAmt);
                        if (!htFnlFnlYR.ContainsKey(col))//final total
                            htFnlFnlYR.Add(col, grandAmt);
                        else
                        {
                            double total = 0;
                            double.TryParse(Convert.ToString(htFnlFnlYR[col]), out total);
                            total += grandAmt;
                            htFnlFnlYR.Remove(col);
                            htFnlFnlYR.Add(col, total);
                        }
                    }
                    htTempFnl.Clear();
                    #endregion
                }
            }
            // }
            if (htFnlFnlYR.Count > 0)//final financial year total
            {
                #region Every Total
                spreadDet.Sheets[0].Rows.Count++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Gold;
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                double grandAmt = 0;
                for (int col = 8; col < spreadDet.Sheets[0].ColumnCount; col++)
                {
                    double.TryParse(Convert.ToString(htFnlFnlYR[col]), out grandAmt);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, col].Text = Convert.ToString(grandAmt);
                }
                #endregion
            }
            spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
            spreadDet.SaveChanges();
            spreadDet.Height = height;
            spreadDet.Visible = true;
            print.Visible = true;
            divlabl.Visible = true;
        }
        catch { }
    }

    protected void getYetToBePaid(DataSet ds, ref Hashtable htColCnt, ref Dictionary<string, string> dtSemCol)
    {
        try
        {
            Hashtable htTempFnl = new Hashtable();
            Hashtable htFnlFnlYR = new Hashtable();
            Hashtable htDeptName = getDeptName();//deptment name
            DataTable dtpaidName = dtPaidHeader();//spread column header
            Hashtable finYear = getFinyear();//financial year
            Dictionary<string, string> htSelFnlYR = new Dictionary<string, string>();
            if (!htSelFnlYR.ContainsKey(Convert.ToString(ddlfinlyear.SelectedValue)))
                htSelFnlYR.Add(Convert.ToString(ddlfinlyear.SelectedValue), Convert.ToString(ddlfinlyear.SelectedItem.Text));
            int rowCnt = 0;
            int height = 0;
            FarPoint.Web.Spread.TextCellType txtroll = new FarPoint.Web.Spread.TextCellType();
            //for (int fnlYr = 0; fnlYr < chklsfyear.Items.Count; fnlYr++)
            //{
            foreach (KeyValuePair<string, string> dtFnlYr in htSelFnlYR)
            {
                bool boolFinlYr = false;
                //if (chklsfyear.Items[fnlYr].Selected)
                //{
                // string actFinYearFK = Convert.ToString(chklsfyear.Items[fnlYr].Value);
                string actFinYearFK = Convert.ToString(dtFnlYr.Key);
                DataTable dtPaid = ds.Tables[0].Copy();
                for (int dtRow = 0; dtRow < dtPaid.Rows.Count; dtRow++)
                {
                    if (!boolFinlYr)
                    {
                        spreadDet.Sheets[0].RowCount++;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(finYear[actFinYearFK]);
                        spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, spreadDet.Sheets[0].ColumnCount - 1);
                        spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
                        boolFinlYr = true;
                    }
                    string appNo = Convert.ToString(dtPaid.Rows[dtRow]["app_no"]);
                    bool studPaidStatus = true;
                    double totalAmount = 0;

                    #region header Amount
                    //semester and headerfk with column count added in dictionary while header binding
                    foreach (KeyValuePair<string, string> dtSemeHd in dtSemCol)
                    {
                        string feeCatg = Convert.ToString(dtSemeHd.Key).Split('-')[0];
                        string hdFK = Convert.ToString(dtSemeHd.Key).Split('-')[1];
                        // string hdFK = Convert.ToString(dtSemeHd.Key).Split('-')[1];
                        ds.Tables[1].DefaultView.RowFilter = "finyearfk='" + actFinYearFK + "' and app_no='" + appNo + "' and feecategory='" + feeCatg + "' and headerfk='" + hdFK + "'";
                        DataTable dtHeader = ds.Tables[1].DefaultView.ToTable();
                        if (dtHeader.Rows.Count > 0)
                        {
                            for (int dtHd = 0; dtHd < dtHeader.Rows.Count; dtHd++)
                            {
                                int colCnt = 0;
                                int.TryParse(Convert.ToString(htColCnt[feeCatg + "-" + dtHeader.Rows[dtHd]["headerfk"]]), out colCnt);
                                double totAmount = 0;
                                double.TryParse(Convert.ToString(dtHeader.Rows[dtHd]["total"]), out totAmount);
                                //paidamount get dailytransaction table 
                                double paidAmount = 0;
                                ds.Tables[2].DefaultView.RowFilter = "actualfinyearfk='" + actFinYearFK + "' and app_no='" + appNo + "' and feecategory='" + feeCatg + "' and headerfk='" + hdFK + "'";
                                DataTable dtPaidDet = ds.Tables[2].DefaultView.ToTable();
                                if (dtPaidDet.Rows.Count > 0)
                                    double.TryParse(Convert.ToString(dtPaidDet.Rows[dtHd]["paid"]), out paidAmount);
                                double balAmount = totAmount - paidAmount;

                                totalAmount += balAmount;
                                if (studPaidStatus)//add new row when get paid amount only
                                    spreadDet.Sheets[0].RowCount++;
                                studPaidStatus = false;
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = Convert.ToString(balAmount);
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Right;
                                if (!htTempFnl.ContainsKey(colCnt))
                                    htTempFnl.Add(colCnt, balAmount);
                                else
                                {
                                    double total = 0;
                                    double.TryParse(Convert.ToString(htTempFnl[colCnt]), out total);
                                    total += balAmount;
                                    htTempFnl.Remove(colCnt);
                                    htTempFnl.Add(colCnt, total);
                                }
                            }
                        }
                    }
                    if (totalAmount != 0)
                    {
                        height += 10;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(totalAmount);//end of the column total
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                        int colCnt = spreadDet.Sheets[0].ColumnCount - 1;
                        if (!htTempFnl.ContainsKey(colCnt))
                            htTempFnl.Add(colCnt, totalAmount);
                        else
                        {
                            double total = 0;
                            double.TryParse(Convert.ToString(htTempFnl[colCnt]), out total);
                            total += totalAmount;
                            htTempFnl.Remove(colCnt);
                            htTempFnl.Add(colCnt, total);
                        }

                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(++rowCnt);

                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dtPaid.Rows[dtRow]["stud_name"]);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dtPaid.Rows[dtRow]["roll_no"]);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dtPaid.Rows[dtRow]["Reg_No"]);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dtPaid.Rows[dtRow]["roll_admit"]);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].CellType = txtroll;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].CellType = txtroll;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].CellType = txtroll;
                        string deptName = Convert.ToString(htDeptName[Convert.ToString(dtPaid.Rows[dtRow]["degree_code"])]);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 5].Text = deptName;
                        spreadDet.Sheets[0].Columns[6].Visible = false;
                        spreadDet.Sheets[0].Columns[7].Visible = false;
                    }
                    #endregion
                }
                // }
                if (htTempFnl.Count > 0)//every financial year total
                {
                    #region Every Total
                    spreadDet.Sheets[0].Rows.Count++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Total";
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                    double grandAmt = 0;
                    for (int col = 8; col < spreadDet.Sheets[0].ColumnCount; col++)
                    {
                        double.TryParse(Convert.ToString(htTempFnl[col]), out grandAmt);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, col].Text = Convert.ToString(grandAmt);
                        if (!htFnlFnlYR.ContainsKey(col))//final total
                            htFnlFnlYR.Add(col, grandAmt);
                        else
                        {
                            double total = 0;
                            double.TryParse(Convert.ToString(htFnlFnlYR[col]), out total);
                            total += grandAmt;
                            htFnlFnlYR.Remove(col);
                            htFnlFnlYR.Add(col, total);
                        }
                    }
                    htTempFnl.Clear();
                    #endregion
                }
            }
            // }
            if (htFnlFnlYR.Count > 0)//final financial year total
            {
                #region Every Total
                spreadDet.Sheets[0].Rows.Count++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Gold;
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                double grandAmt = 0;
                for (int col = 8; col < spreadDet.Sheets[0].ColumnCount; col++)
                {
                    double.TryParse(Convert.ToString(htFnlFnlYR[col]), out grandAmt);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, col].Text = Convert.ToString(grandAmt);
                }
                #endregion
            }
            spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
            spreadDet.SaveChanges();
            spreadDet.Height = height;
            spreadDet.Visible = true;
            print.Visible = true;
            divlabl.Visible = true;
        }
        catch { }
    }

    #endregion

    #region college
    protected DataSet getDatesetCollege()
    {
        DataSet dsload = new DataSet();
        try
        {
            #region get value
            DateTime fromdt;
            DateTime todt;
            DataSet lgacr = new DataSet();
            string studType = string.Empty;
            string stream = string.Empty;
            string fromdate = string.Empty;
            string todate = string.Empty;
            string bankcode = string.Empty;
            string delflg = string.Empty;
            string collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            if (ddltype.Enabled == true)
            {
                if (ddltype.SelectedItem.Text != "All" && ddltype.SelectedItem.Text != "")
                    stream = Convert.ToString(ddltype.SelectedItem.Text);
            }
            string batch = Convert.ToString(getCblSelectedValue(cbl_batch));
            string degree = Convert.ToString(getCblSelectedValue(cbl_dept));
            if (ddlstudtype.SelectedItem.Text != "All" && ddlstudtype.SelectedItem.Text != "")
                studType = Convert.ToString(ddlstudtype.SelectedItem.Value);
            string feesType = string.Empty;
            feesType = Convert.ToString(ddlfeetype.SelectedItem.Text);
            string feecat = Convert.ToString(getCblSelectedValue(cbl_sem));
            string payMode = Convert.ToString(getCblSelectedValue(chkl_paid));
            //string fnlYear = Convert.ToString(getCblSelectedValue(chklsfyear));
            string fnlYear = Convert.ToString(ddlfinlyear.SelectedItem.Value);
            string headerValue = string.Empty;
            string ledgerValue = string.Empty;
            string strtype = string.Empty;
            if (ddlacctype.SelectedIndex == 0 || ddlacctype.SelectedIndex == 1)
            {
                headerValue = Convert.ToString(getCblSelectedValue(cblheader));
                strtype = ",headerfk";
            }
            else
            {
                headerValue = ledgervalue();
                strtype = ",ledgerfk";
            }

            string studMode = Convert.ToString(getCblSelectedValue(cbl_type));


            #region include

            if (cbdate.Checked)
            {
                fromdate = txt_fromdate.Text;
                todate = txt_todate.Text;
                string[] frdate = fromdate.Split('/');
                if (frdate.Length == 3)
                    fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
                string[] tdate = todate.Split('/');
                if (tdate.Length == 3)
                    todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
                fromdt = Convert.ToDateTime(fromdate);
                todt = Convert.ToDateTime(todate);
            }
            if (cbbankcheck.Checked)
            {
                for (int i = 0; i < chkl_paid.Items.Count; i++)
                {
                    if (chkl_paid.Items[i].Selected == true && chkl_paid.Items[i].Text == "Online Pay")
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
            }
            #endregion
            string strInclude = getStudCategory();
            string applynStr = " AND r.IsConfirm = 1  and isnull(is_enroll,'0')<>'2'";
            //  dtPaidHeader(strtype, batch, degree, feecat, collegecode, studMode, strInclude, headerValue, fnlYear, applynStr, fromdate, todate, payMode, headerValue);
            #endregion
            string selQ = "";
            if (ddlacctype.SelectedItem.Text.Trim() == "Header")//header
            {
                if (ddlfeetype.SelectedItem.Text.Trim() == "Paid")//paid
                {
                    if (!cbRcpt.Checked)//without receipt no
                    {
                        #region without receipt no
                        //allot total student strength
                        selQ = "  select distinct app_no, stud_name,Current_Semester,Reg_No,Roll_No,degree_code,roll_admit,batch_year from (";
                        selQ += "  select  r.app_no, r.stud_name,r.Current_Semester,r.Reg_No,r.Roll_No,r.degree_code,r.roll_admit,r.batch_year from ft_feeallot a,registration r where  a.app_no=r.app_no and isnull(a.totalamount,'0')>0  and isnull(a.Paidamount,'0')>0   and r.college_code ='" + collegecode + "' " + strInclude + " and r.batch_year  in ('" + batch + "') and r.degree_code  in ('" + degree + "') and a.FeeCategory  in ('" + feecat + "') and headerfk   in ('" + headerValue + "') and a.FinYearFK   in ('" + fnlYear + "') and r.mode in('" + studMode + "')  ";
                        if (cbBeforAdm.Checked)
                        {
                            selQ += " union all select  r.app_no, r.stud_name,r.Current_Semester,''Reg_No,''Roll_No,r.degree_code,''roll_admit,r.batch_year from ft_feeallot a,applyn r where  a.app_no=r.app_no and isnull(a.totalamount,'0')>0  and isnull(a.Paidamount,'0')>0   and r.college_code ='" + collegecode + "'  and r.batch_year  in ('" + batch + "') and r.degree_code  in ('" + degree + "') and a.FeeCategory  in ('" + feecat + "') and headerfk   in ('" + headerValue + "') and a.FinYearFK   in ('" + fnlYear + "') and r.mode in('" + studMode + "') " + applynStr + " ";
                        }
                        selQ += " ) tbl order by Batch_Year,degree_code,Current_Semester,Stud_Name ";

                        //stud paid list
                        selQ += "  select distinct sum(debit) as paid, app_no,headerfk,feecategory from(";
                        selQ += "  select debit, a.app_no,headerfk,feecategory  from ft_findailytransaction a,registration r where  a.app_no = r.app_no and isnull(debit,'0')>0 and isnull(actualfinyearfk,'0')<>'0'  and isnull(IsCanceled,'0')='0' and isnull(isdeposited,'0')='1' and  ISNULL(IsCollected,0)='1' and isnull(transcode,'')<>''  " + strInclude + " and r.batch_year  in ('" + batch + "') and r.degree_code  in ('" + degree + "') and a.FeeCategory  in ('" + feecat + "') and headerfk   in ('" + headerValue + "') and a.ActualFinYearFk  in ('" + fnlYear + "') and r.mode in('" + studMode + "') and a.paymode in ('" + payMode + "')  and r.college_code ='" + collegecode + "'";
                        if (cbdate.Checked)
                            selQ += " and Transdate between '" + fromdate + "' and '" + todate + "'";

                        if (cbBeforAdm.Checked)
                        {
                            selQ += " union all  select  debit, a.app_no,headerfk,feecategory  from ft_findailytransaction a,applyn r where  a.app_no = r.app_no and isnull(debit,'0')>0 and isnull(actualfinyearfk,'0')<>'0'  and isnull(IsCanceled,'0')='0' and isnull(isdeposited,'0')='1' and  ISNULL(IsCollected,0)='1' and isnull(transcode,'')<>''  " + strInclude + " and r.batch_year  in ('" + batch + "') and r.degree_code  in ('" + degree + "') and a.FeeCategory  in ('" + feecat + "') and headerfk   in ('" + headerValue + "') and a.ActualFinYearFk  in ('" + fnlYear + "') and r.mode in('" + studMode + "') and a.paymode in ('" + payMode + "')  and r.college_code ='" + collegecode + "' " + strInclude + "";
                            if (cbdate.Checked)
                                selQ += " and Transdate between '" + fromdate + "' and '" + todate + "'";
                        }
                        selQ += ")tbl group by app_no,headerfk,feecategory ";
                        #endregion
                    }
                    else
                    {
                        #region without receipt no
                        //allot total student strength
                        selQ = "  select distinct app_no, stud_name,Current_Semester,Reg_No,Roll_No,degree_code,roll_admit,batch_year from (";
                        selQ += "  select  r.app_no, r.stud_name,r.Current_Semester,r.Reg_No,r.Roll_No,r.degree_code,r.roll_admit,r.batch_year from ft_feeallot a,registration r where  a.app_no=r.app_no and isnull(a.totalamount,'0')>0  and isnull(a.Paidamount,'0')>0   and r.college_code ='" + collegecode + "' " + strInclude + " and r.batch_year  in ('" + batch + "') and r.degree_code  in ('" + degree + "') and a.FeeCategory  in ('" + feecat + "') and headerfk   in ('" + headerValue + "') and a.FinYearFK   in ('" + fnlYear + "') and r.mode in('" + studMode + "')  ";
                        if (cbBeforAdm.Checked)
                        {
                            selQ += " union all select  r.app_no, r.stud_name,r.Current_Semester,''Reg_No,''Roll_No,r.degree_code,''roll_admit,r.batch_year from ft_feeallot a,applyn r where  a.app_no=r.app_no and isnull(a.totalamount,'0')>0  and isnull(a.Paidamount,'0')>0   and r.college_code ='" + collegecode + "'  and r.batch_year  in ('" + batch + "') and r.degree_code  in ('" + degree + "') and a.FeeCategory  in ('" + feecat + "') and headerfk   in ('" + headerValue + "') and a.FinYearFK   in ('" + fnlYear + "') and r.mode in('" + studMode + "') " + applynStr + " ";
                        }
                        selQ += " ) tbl order by Batch_Year,degree_code,Current_Semester,Stud_Name ";

                        //stud paid list
                        selQ += "  select distinct sum(debit) as paid, app_no,headerfk,feecategory,Transcode,transdate from(";
                        selQ += "  select debit, a.app_no,headerfk,feecategory,Transcode,convert(varchar(10),transdate,103) as  transdate  from ft_findailytransaction a,registration r where  a.app_no = r.app_no and isnull(debit,'0')>0 and isnull(actualfinyearfk,'0')<>'0'  and isnull(IsCanceled,'0')='0' and isnull(isdeposited,'0')='1' and  ISNULL(IsCollected,0)='1' and isnull(transcode,'')<>''  " + strInclude + " and r.batch_year  in ('" + batch + "') and r.degree_code  in ('" + degree + "') and a.FeeCategory  in ('" + feecat + "') and headerfk   in ('" + headerValue + "') and a.ActualFinYearFk  in ('" + fnlYear + "') and r.mode in('" + studMode + "') and a.paymode in ('" + payMode + "')  and r.college_code ='" + collegecode + "'";
                        if (cbdate.Checked)
                            selQ += " and Transdate between '" + fromdate + "' and '" + todate + "'";

                        if (cbBeforAdm.Checked)
                        {
                            selQ += " union all  select  debit, a.app_no,headerfk,feecategory,Transcode,convert(varchar(10),transdate,103) as  transdate  from ft_findailytransaction a,applyn r where  a.app_no = r.app_no and isnull(debit,'0')>0 and isnull(actualfinyearfk,'0')<>'0'  and isnull(IsCanceled,'0')='0' and isnull(isdeposited,'0')='1' and  ISNULL(IsCollected,0)='1' and isnull(transcode,'')<>''  " + strInclude + " and r.batch_year  in ('" + batch + "') and r.degree_code  in ('" + degree + "') and a.FeeCategory  in ('" + feecat + "') and headerfk   in ('" + headerValue + "') and a.ActualFinYearFk  in ('" + fnlYear + "') and r.mode in('" + studMode + "') and a.paymode in ('" + payMode + "')  and r.college_code ='" + collegecode + "' " + strInclude + "";
                            if (cbdate.Checked)
                                selQ += " and Transdate between '" + fromdate + "' and '" + todate + "'";
                        }
                        selQ += ")tbl group by app_no,headerfk,feecategory,Transcode, transdate ";
                        #endregion
                    }

                }
                else if (ddlfeetype.SelectedItem.Text.Trim() == "Yet To Be Paid")//yet to be paid
                {
                    #region yet to be paid
                    selQ = "select distinct app_no, stud_name,Current_Semester,Reg_No,Roll_No,degree_code,roll_admit,batch_year from (";
                    selQ += "  select  r.app_no, r.stud_name,r.Current_Semester,r.Reg_No,r.Roll_No,r.degree_code,r.roll_admit,r.batch_year from ft_feeallot a,registration r where  a.app_no=r.app_no and isnull(a.totalamount,'0')>0  and isnull(a.balamount,'0')>0  and r.college_code ='" + collegecode + "' " + strInclude + " and r.batch_year  in ('" + batch + "') and r.degree_code  in ('" + degree + "') and a.FeeCategory  in ('" + feecat + "') and headerfk   in ('" + headerValue + "') and a.FinYearFK   in ('" + fnlYear + "') and r.mode in('" + studMode + "') " + strInclude + " ";
                    if (cbBeforAdm.Checked)
                    {
                        selQ += " union all select  r.app_no, r.stud_name,r.Current_Semester,''Reg_No,''Roll_No,r.degree_code,''roll_admit,r.batch_year from ft_feeallot a,applyn r where  a.app_no=r.app_no and isnull(a.totalamount,'0')>0  and isnull(a.balamount,'0')>0  and r.college_code ='" + collegecode + "' " + strInclude + " and r.batch_year  in ('" + batch + "') and r.degree_code  in ('" + degree + "') and a.FeeCategory  in ('" + feecat + "') and headerfk   in ('" + headerValue + "') and a.FinYearFK   in ('" + fnlYear + "') and r.mode in('" + studMode + "') " + applynStr + " ";
                    }
                    selQ += " ) tbl order by Batch_Year,degree_code,Current_Semester,Stud_Name ";

                    //allot get student total amoun
                    selQ += " select distinct sum(totalamount) as total,sum(paidamount) as paid, app_no,headerfk,feecategory from (";
                    selQ += "  select totalamount,paidamount, a.app_no,headerfk ,feecategory from ft_feeallot a,registration r where  a.app_no=r.app_no and isnull(a.totalamount,'0')>0  and isnull(a.balamount,'0')>0   and r.college_code ='" + collegecode + "' " + strInclude + " and r.batch_year  in ('" + batch + "') and r.degree_code  in ('" + degree + "') and a.FeeCategory  in ('" + feecat + "') and headerfk   in ('" + headerValue + "') and a.FinYearFK   in ('" + fnlYear + "') and r.mode in('" + studMode + "') " + strInclude + "  ";
                    if (cbBeforAdm.Checked)
                    {
                         //allot get student total amoun
                        selQ += " union all  select totalamount,paidamount, a.app_no,headerfk ,feecategory from ft_feeallot a,applyn r where  a.app_no=r.app_no and isnull(a.totalamount,'0')>0  and isnull(a.balamount,'0')>0   and r.college_code ='" + collegecode + "' " + strInclude + " and r.batch_year  in ('" + batch + "') and r.degree_code  in ('" + degree + "') and a.FeeCategory  in ('" + feecat + "') and headerfk   in ('" + headerValue + "') and a.FinYearFK   in ('" + fnlYear + "') and r.mode in('" + studMode + "') " + applynStr + "";
                    }
                    selQ += " ) tbl group by app_no,headerfk ,feecategory";

                    //stud paid list
                    selQ += " select distinct sum(debit) as paid,app_no,headerfk,feecategory from  (";
                    selQ += "  select debit, a.app_no,headerfk,feecategory     from ft_findailytransaction a,registration r where  a.app_no = r.app_no and isnull(debit,'0')>0 and isnull(actualfinyearfk,'0')<>'0'  and isnull(IsCanceled,'0')='0' and isnull(isdeposited,'0')='1' and  ISNULL(IsCollected,0)='1' and isnull(transcode,'')<>''  " + strInclude + " and r.batch_year  in ('" + batch + "') and r.degree_code  in ('" + degree + "') and a.FeeCategory  in ('" + feecat + "') and headerfk   in ('" + headerValue + "') and a.ActualFinYearFk  in ('" + fnlYear + "') and r.mode in('" + studMode + "') and a.paymode in ('" + payMode + "')  and r.college_code ='" + collegecode + "' " + strInclude + " ";
                    if (cbBeforAdm.Checked)
                    {
                         //stud paid list
                    selQ += " union all  select  debit, a.app_no,headerfk,feecategory  from ft_findailytransaction a,applyn r where  a.app_no = r.app_no and isnull(debit,'0')>0 and isnull(actualfinyearfk,'0')<>'0'  and isnull(IsCanceled,'0')='0' and isnull(isdeposited,'0')='1' and  ISNULL(IsCollected,0)='1' and isnull(transcode,'')<>''  " + strInclude + " and r.batch_year  in ('" + batch + "') and r.degree_code  in ('" + degree + "') and a.FeeCategory  in ('" + feecat + "') and headerfk   in ('" + headerValue + "') and a.ActualFinYearFk  in ('" + fnlYear + "') and r.mode in('" + studMode + "') and a.paymode in ('" + payMode + "')  and r.college_code ='" + collegecode + "' " + applynStr + " ";
                    }
                    selQ += ") tbl group by app_no,headerfk,feecategory ";
                    #endregion
                }
            }
            else//ledger
            {
                if (ddlfeetype.SelectedItem.Text.Trim() == "Paid")//paid
                {

                }
                else//yet to be paid
                {
                }
            }
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(selQ, "Text");
        }
        catch { }
        return dsload;
    }

    protected void getPaidDetCollege(DataSet ds, ref Hashtable htColCnt, ref Dictionary<string, string> dtSemCol)
    {
        try
        {

            Hashtable htTempFnl = new Hashtable();
            Hashtable htFnlFnlYR = new Hashtable();
            Hashtable htDeptName = getDeptName();//deptment name
            DataTable dtpaidName = dtPaidHeader();//spread column header
            Hashtable finYear = getFinyear();//financial year
            //Dictionary<string, string> htSelFnlYR = new Dictionary<string, string>();
            //if (!htSelFnlYR.ContainsKey(Convert.ToString(ddlfinlyear.SelectedValue)))
            //    htSelFnlYR.Add(Convert.ToString(ddlfinlyear.SelectedValue), Convert.ToString(ddlfinlyear.SelectedItem.Text));
            int rowCnt = 0;
            int height = 0;
            FarPoint.Web.Spread.TextCellType txtroll = new FarPoint.Web.Spread.TextCellType();
            //for (int fnlYr = 0; fnlYr < chklsfyear.Items.Count; fnlYr++)
            //{
            //foreach (KeyValuePair<string, string> dtFnlYr in htSelFnlYR)
            //{
            bool boolFinlYr = false;
            //if (chklsfyear.Items[fnlYr].Selected)
            //{
            // string actFinYearFK = Convert.ToString(chklsfyear.Items[fnlYr].Value);
            //string actFinYearFK = Convert.ToString(dtFnlYr.Key);
            DataTable dtPaid = ds.Tables[0].Copy();
            for (int dtRow = 0; dtRow < dtPaid.Rows.Count; dtRow++)
            {
                //if (!boolFinlYr)
                //{
                //    spreadDet.Sheets[0].RowCount++;
                //    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(finYear[actFinYearFK]);
                //    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, spreadDet.Sheets[0].ColumnCount - 1);
                //    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
                //    boolFinlYr = true;
                //}
                string appNo = Convert.ToString(dtPaid.Rows[dtRow]["app_no"]);
                bool studPaidStatus = true;
                double totalAmount = 0;

                #region header Amount
                //semester and headerfk with column count added in dictionary while header binding
                foreach (KeyValuePair<string, string> dtSemeHd in dtSemCol)
                {
                    string feeCatg = Convert.ToString(dtSemeHd.Key).Split('-')[0];
                    string hdFK = Convert.ToString(dtSemeHd.Key).Split('-')[1];
                    // string hdFK = Convert.ToString(dtSemeHd.Key).Split('-')[1];
                    ds.Tables[1].DefaultView.RowFilter = " app_no='" + appNo + "' and feecategory='" + feeCatg + "' and headerfk='" + hdFK + "'";//actualfinyearfk='" + actFinYearFK + "' and
                    DataTable dtHeader = ds.Tables[1].DefaultView.ToTable();
                    if (dtHeader.Rows.Count > 0)
                    {
                        for (int dtHd = 0; dtHd < dtHeader.Rows.Count; dtHd++)
                        {
                            int colCnt = 0;
                            int.TryParse(Convert.ToString(htColCnt[feeCatg + "-" + dtHeader.Rows[dtHd]["headerfk"]]), out colCnt);
                            double paidAmount = 0;
                            double.TryParse(Convert.ToString(dtHeader.Rows[dtHd]["paid"]), out paidAmount);
                            totalAmount += paidAmount;
                            if (studPaidStatus)//add new row when get paid amount only
                                spreadDet.Sheets[0].RowCount++;
                            studPaidStatus = false;
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = Convert.ToString(paidAmount);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Right;
                            if (!htTempFnl.ContainsKey(colCnt))
                                htTempFnl.Add(colCnt, paidAmount);
                            else
                            {
                                double total = 0;
                                double.TryParse(Convert.ToString(htTempFnl[colCnt]), out total);
                                total += paidAmount;
                                htTempFnl.Remove(colCnt);
                                htTempFnl.Add(colCnt, total);
                            }
                        }
                    }
                }
                if (totalAmount != 0)
                {
                    height += 10;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(totalAmount);//end of the column total
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                    int colCnt = spreadDet.Sheets[0].ColumnCount - 1;
                    if (!htTempFnl.ContainsKey(colCnt))
                        htTempFnl.Add(colCnt, totalAmount);
                    else
                    {
                        double total = 0;
                        double.TryParse(Convert.ToString(htTempFnl[colCnt]), out total);
                        total += totalAmount;
                        htTempFnl.Remove(colCnt);
                        htTempFnl.Add(colCnt, total);
                    }

                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(++rowCnt);

                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dtPaid.Rows[dtRow]["stud_name"]);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dtPaid.Rows[dtRow]["roll_no"]);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dtPaid.Rows[dtRow]["Reg_No"]);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dtPaid.Rows[dtRow]["roll_admit"]);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].CellType = txtroll;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].CellType = txtroll;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].CellType = txtroll;
                    string deptName = Convert.ToString(htDeptName[Convert.ToString(dtPaid.Rows[dtRow]["degree_code"])]);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 5].Text = deptName;
                    spreadDet.Sheets[0].Columns[6].Visible = false;
                    spreadDet.Sheets[0].Columns[7].Visible = false;
                }
                #endregion
            }
            // }
            if (htTempFnl.Count > 0)//every financial year total
            {
                #region Every Total
                spreadDet.Sheets[0].Rows.Count++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Total";
                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                double grandAmt = 0;
                for (int col = 8; col < spreadDet.Sheets[0].ColumnCount; col++)
                {
                    double.TryParse(Convert.ToString(htTempFnl[col]), out grandAmt);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, col].Text = Convert.ToString(grandAmt);
                    if (!htFnlFnlYR.ContainsKey(col))//final total
                        htFnlFnlYR.Add(col, grandAmt);
                    else
                    {
                        double total = 0;
                        double.TryParse(Convert.ToString(htFnlFnlYR[col]), out total);
                        total += grandAmt;
                        htFnlFnlYR.Remove(col);
                        htFnlFnlYR.Add(col, total);
                    }
                }
                htTempFnl.Clear();
                #endregion
            }
            //  }
            // }
            if (htFnlFnlYR.Count > 0)//final financial year total
            {
                #region Every Total
                spreadDet.Sheets[0].Rows.Count++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Gold;
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                double grandAmt = 0;
                for (int col = 8; col < spreadDet.Sheets[0].ColumnCount; col++)
                {
                    double.TryParse(Convert.ToString(htFnlFnlYR[col]), out grandAmt);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, col].Text = Convert.ToString(grandAmt);
                }
                #endregion
            }
            spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
            spreadDet.SaveChanges();
            spreadDet.Height = height;
            spreadDet.Visible = true;
            print.Visible = true;
            divlabl.Visible = true;
        }
        catch { }
    }

    protected void getPaidDetRcptNoCollege(DataSet ds, ref Hashtable htColCnt, ref Dictionary<string, string> dtSemCol)
    {
        try
        {

            Hashtable htTempFnl = new Hashtable();
            Hashtable htFnlFnlYR = new Hashtable();
            Hashtable htDeptName = getDeptName();//deptment name
            DataTable dtpaidName = dtPaidHeader();//spread column header
            Hashtable finYear = getFinyear();//financial year
            //Dictionary<string, string> htSelFnlYR = new Dictionary<string, string>();
            //if (!htSelFnlYR.ContainsKey(Convert.ToString(ddlfinlyear.SelectedValue)))
            //    htSelFnlYR.Add(Convert.ToString(ddlfinlyear.SelectedValue), Convert.ToString(ddlfinlyear.SelectedItem.Text));
            int rowCnt = 0;
            int height = 0;
            FarPoint.Web.Spread.TextCellType txtroll = new FarPoint.Web.Spread.TextCellType();
            //for (int fnlYr = 0; fnlYr < chklsfyear.Items.Count; fnlYr++)
            //{
            //foreach (KeyValuePair<string, string> dtFnlYr in htSelFnlYR)
            //{
            bool boolFinlYr = false;
            //if (chklsfyear.Items[fnlYr].Selected)
            //{
            // string actFinYearFK = Convert.ToString(chklsfyear.Items[fnlYr].Value);
            //  string actFinYearFK = Convert.ToString(dtFnlYr.Key);
            DataTable dtPaid = ds.Tables[0].Copy();
            for (int dtRow = 0; dtRow < dtPaid.Rows.Count; dtRow++)
            {
                //if (!boolFinlYr)
                //{
                //    spreadDet.Sheets[0].RowCount++;
                //    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(finYear[actFinYearFK]);
                //    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, spreadDet.Sheets[0].ColumnCount - 1);
                //    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
                //    boolFinlYr = true;
                //}
                string appNo = Convert.ToString(dtPaid.Rows[dtRow]["app_no"]);
                bool studPaidStatus = true;
                double totalAmount = 0;

                #region header Amount
                //semester and headerfk with column count added in dictionary while header binding
                foreach (KeyValuePair<string, string> dtSemeHd in dtSemCol)
                {
                    string feeCatg = Convert.ToString(dtSemeHd.Key).Split('-')[0];
                    string hdFK = Convert.ToString(dtSemeHd.Key).Split('-')[1];
                    // string hdFK = Convert.ToString(dtSemeHd.Key).Split('-')[1];
                    ds.Tables[1].DefaultView.RowFilter = " app_no='" + appNo + "' and feecategory='" + feeCatg + "' and headerfk='" + hdFK + "'";//actualfinyearfk='" + actFinYearFK + "' and
                    DataTable dtHeader = ds.Tables[1].DefaultView.ToTable();
                    if (dtHeader.Rows.Count > 0)
                    {
                        for (int dtHd = 0; dtHd < dtHeader.Rows.Count; dtHd++)
                        {
                            int colCnt = 0;
                            int.TryParse(Convert.ToString(htColCnt[feeCatg + "-" + dtHeader.Rows[dtHd]["headerfk"]]), out colCnt);
                            double paidAmount = 0;
                            double.TryParse(Convert.ToString(dtHeader.Rows[dtHd]["paid"]), out paidAmount);
                            totalAmount += paidAmount;
                            if (studPaidStatus)//add new row when get paid amount only
                                spreadDet.Sheets[0].RowCount++;
                            studPaidStatus = false;
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = Convert.ToString(paidAmount);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dtHeader.Rows[dtHd]["Transcode"]);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(dtHeader.Rows[dtHd]["transdate"]);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Right;
                            if (!htTempFnl.ContainsKey(colCnt))
                                htTempFnl.Add(colCnt, paidAmount);
                            else
                            {
                                double total = 0;
                                double.TryParse(Convert.ToString(htTempFnl[colCnt]), out total);
                                total += paidAmount;
                                htTempFnl.Remove(colCnt);
                                htTempFnl.Add(colCnt, total);
                            }
                        }
                    }
                }
                if (totalAmount != 0)
                {
                    height += 10;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(totalAmount);//end of the column total
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                    int colCnt = spreadDet.Sheets[0].ColumnCount - 1;
                    if (!htTempFnl.ContainsKey(colCnt))
                        htTempFnl.Add(colCnt, totalAmount);
                    else
                    {
                        double total = 0;
                        double.TryParse(Convert.ToString(htTempFnl[colCnt]), out total);
                        total += totalAmount;
                        htTempFnl.Remove(colCnt);
                        htTempFnl.Add(colCnt, total);
                    }

                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(++rowCnt);

                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dtPaid.Rows[dtRow]["stud_name"]);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dtPaid.Rows[dtRow]["roll_no"]);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dtPaid.Rows[dtRow]["Reg_No"]);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dtPaid.Rows[dtRow]["roll_admit"]);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].CellType = txtroll;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].CellType = txtroll;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].CellType = txtroll;
                    string deptName = Convert.ToString(htDeptName[Convert.ToString(dtPaid.Rows[dtRow]["degree_code"])]);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 5].Text = deptName;
                }
                #endregion
            }
            // }
            if (htTempFnl.Count > 0)//every financial year total
            {
                #region Every Total
                spreadDet.Sheets[0].Rows.Count++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Total";
                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                double grandAmt = 0;
                for (int col = 8; col < spreadDet.Sheets[0].ColumnCount; col++)
                {
                    double.TryParse(Convert.ToString(htTempFnl[col]), out grandAmt);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, col].Text = Convert.ToString(grandAmt);
                    if (!htFnlFnlYR.ContainsKey(col))//final total
                        htFnlFnlYR.Add(col, grandAmt);
                    else
                    {
                        double total = 0;
                        double.TryParse(Convert.ToString(htFnlFnlYR[col]), out total);
                        total += grandAmt;
                        htFnlFnlYR.Remove(col);
                        htFnlFnlYR.Add(col, total);
                    }
                }
                htTempFnl.Clear();
                #endregion
            }
            // }
            // }
            if (htFnlFnlYR.Count > 0)//final financial year total
            {
                #region Every Total
                spreadDet.Sheets[0].Rows.Count++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Gold;
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                double grandAmt = 0;
                for (int col = 8; col < spreadDet.Sheets[0].ColumnCount; col++)
                {
                    double.TryParse(Convert.ToString(htFnlFnlYR[col]), out grandAmt);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, col].Text = Convert.ToString(grandAmt);
                }
                #endregion
            }
            spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
            spreadDet.SaveChanges();
            spreadDet.Height = height;
            spreadDet.Visible = true;
            print.Visible = true;
            divlabl.Visible = true;
        }
        catch { }
    }

    protected void getYetToBePaidCollege(DataSet ds, ref Hashtable htColCnt, ref Dictionary<string, string> dtSemCol)
    {
        try
        {
            Hashtable htTempFnl = new Hashtable();
            Hashtable htFnlFnlYR = new Hashtable();
            Hashtable htDeptName = getDeptName();//deptment name
            DataTable dtpaidName = dtPaidHeader();//spread column header
            Hashtable finYear = getFinyear();//financial year
            Dictionary<string, string> htSelFnlYR = new Dictionary<string, string>();
            if (!htSelFnlYR.ContainsKey(Convert.ToString(ddlfinlyear.SelectedValue)))
                htSelFnlYR.Add(Convert.ToString(ddlfinlyear.SelectedValue), Convert.ToString(ddlfinlyear.SelectedItem.Text));
            int rowCnt = 0;
            int height = 0;
            FarPoint.Web.Spread.TextCellType txtroll = new FarPoint.Web.Spread.TextCellType();
            //for (int fnlYr = 0; fnlYr < chklsfyear.Items.Count; fnlYr++)
            //{
            //foreach (KeyValuePair<string, string> dtFnlYr in htSelFnlYR)
            //{
            bool boolFinlYr = false;
            //if (chklsfyear.Items[fnlYr].Selected)
            //{
            // string actFinYearFK = Convert.ToString(chklsfyear.Items[fnlYr].Value);
            //   string actFinYearFK = Convert.ToString(dtFnlYr.Key);
            DataTable dtPaid = ds.Tables[0].Copy();
            for (int dtRow = 0; dtRow < dtPaid.Rows.Count; dtRow++)
            {
                //if (!boolFinlYr)
                //{
                //    spreadDet.Sheets[0].RowCount++;
                //    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(finYear[actFinYearFK]);
                //    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].RowCount - 1, 0, 1, spreadDet.Sheets[0].ColumnCount - 1);
                //    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
                //    boolFinlYr = true;
                //}
                string appNo = Convert.ToString(dtPaid.Rows[dtRow]["app_no"]);
                bool studPaidStatus = true;
                double totalAmount = 0;

                #region header Amount
                //semester and headerfk with column count added in dictionary while header binding
                foreach (KeyValuePair<string, string> dtSemeHd in dtSemCol)
                {
                    string feeCatg = Convert.ToString(dtSemeHd.Key).Split('-')[0];
                    string hdFK = Convert.ToString(dtSemeHd.Key).Split('-')[1];
                    // string hdFK = Convert.ToString(dtSemeHd.Key).Split('-')[1];
                    ds.Tables[1].DefaultView.RowFilter = "app_no='" + appNo + "' and feecategory='" + feeCatg + "' and headerfk='" + hdFK + "'";//finyearfk='" + actFinYearFK + "' and 
                    DataTable dtHeader = ds.Tables[1].DefaultView.ToTable();
                    if (dtHeader.Rows.Count > 0)
                    {
                        for (int dtHd = 0; dtHd < dtHeader.Rows.Count; dtHd++)
                        {
                            int colCnt = 0;
                            int.TryParse(Convert.ToString(htColCnt[feeCatg + "-" + dtHeader.Rows[dtHd]["headerfk"]]), out colCnt);
                            double totAmount = 0;
                            double.TryParse(Convert.ToString(dtHeader.Rows[dtHd]["total"]), out totAmount);
                            //paidamount get dailytransaction table 
                            double paidAmount = 0;
                            ds.Tables[2].DefaultView.RowFilter = "app_no='" + appNo + "' and feecategory='" + feeCatg + "' and headerfk='" + hdFK + "'";//actualfinyearfk='" + actFinYearFK + "' and 
                            DataTable dtPaidDet = ds.Tables[2].DefaultView.ToTable();
                            if (dtPaidDet.Rows.Count > 0)
                                double.TryParse(Convert.ToString(dtPaidDet.Rows[dtHd]["paid"]), out paidAmount);
                            double balAmount = totAmount - paidAmount;

                            totalAmount += balAmount;
                            if (studPaidStatus)//add new row when get paid amount only
                                spreadDet.Sheets[0].RowCount++;
                            studPaidStatus = false;
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].Text = Convert.ToString(balAmount);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, colCnt].HorizontalAlign = HorizontalAlign.Right;
                            if (!htTempFnl.ContainsKey(colCnt))
                                htTempFnl.Add(colCnt, balAmount);
                            else
                            {
                                double total = 0;
                                double.TryParse(Convert.ToString(htTempFnl[colCnt]), out total);
                                total += balAmount;
                                htTempFnl.Remove(colCnt);
                                htTempFnl.Add(colCnt, total);
                            }
                        }
                    }
                }
                if (totalAmount != 0)
                {
                    height += 10;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(totalAmount);//end of the column total
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                    int colCnt = spreadDet.Sheets[0].ColumnCount - 1;
                    if (!htTempFnl.ContainsKey(colCnt))
                        htTempFnl.Add(colCnt, totalAmount);
                    else
                    {
                        double total = 0;
                        double.TryParse(Convert.ToString(htTempFnl[colCnt]), out total);
                        total += totalAmount;
                        htTempFnl.Remove(colCnt);
                        htTempFnl.Add(colCnt, total);
                    }

                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(++rowCnt);

                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dtPaid.Rows[dtRow]["stud_name"]);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dtPaid.Rows[dtRow]["roll_no"]);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dtPaid.Rows[dtRow]["Reg_No"]);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dtPaid.Rows[dtRow]["roll_admit"]);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].CellType = txtroll;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].CellType = txtroll;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].CellType = txtroll;
                    string deptName = Convert.ToString(htDeptName[Convert.ToString(dtPaid.Rows[dtRow]["degree_code"])]);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 5].Text = deptName;
                    spreadDet.Sheets[0].Columns[6].Visible = false;
                    spreadDet.Sheets[0].Columns[7].Visible = false;
                }
                #endregion
            }
            // }
            if (htTempFnl.Count > 0)//every financial year total
            {
                #region Every Total
                spreadDet.Sheets[0].Rows.Count++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Total";
                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                double grandAmt = 0;
                for (int col = 8; col < spreadDet.Sheets[0].ColumnCount; col++)
                {
                    double.TryParse(Convert.ToString(htTempFnl[col]), out grandAmt);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, col].Text = Convert.ToString(grandAmt);
                    if (!htFnlFnlYR.ContainsKey(col))//final total
                        htFnlFnlYR.Add(col, grandAmt);
                    else
                    {
                        double total = 0;
                        double.TryParse(Convert.ToString(htFnlFnlYR[col]), out total);
                        total += grandAmt;
                        htFnlFnlYR.Remove(col);
                        htFnlFnlYR.Add(col, total);
                    }
                }
                htTempFnl.Clear();
                #endregion
            }
            // }
            // }
            if (htFnlFnlYR.Count > 0)//final financial year total
            {
                #region Every Total
                spreadDet.Sheets[0].Rows.Count++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Gold;
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                double grandAmt = 0;
                for (int col = 8; col < spreadDet.Sheets[0].ColumnCount; col++)
                {
                    double.TryParse(Convert.ToString(htFnlFnlYR[col]), out grandAmt);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, col].Text = Convert.ToString(grandAmt);
                }
                #endregion
            }
            spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
            spreadDet.SaveChanges();
            spreadDet.Height = height;
            spreadDet.Visible = true;
            print.Visible = true;
            divlabl.Visible = true;
        }
        catch { }
    }
    #endregion

    protected DataTable dtPaidHeader()
    {
        // protected DataTable dtPaidHeader(string strtype, string batch, string deptdegcode, string sem, string collegecode, string studMode, string strInclude, string headervalue, string Finyearvalue, string applynStr, string fromdate, string todate, string paid, string ledgervalue)

        DataTable dtHdName = new DataTable();
        try
        {
            #region get value
            DataSet lgacr = new DataSet();
            string studType = string.Empty;
            string stream = string.Empty;
            string fromdate = string.Empty;
            string todate = string.Empty;
            string bankcode = string.Empty;
            string delflg = string.Empty;
            collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            if (ddltype.Enabled == true)
            {
                if (ddltype.SelectedItem.Text != "All" && ddltype.SelectedItem.Text != "")
                    stream = Convert.ToString(ddltype.SelectedItem.Text);
            }
            string batch = Convert.ToString(getCblSelectedValue(cbl_batch));
            string deptdegcode = Convert.ToString(getCblSelectedValue(cbl_dept));
            if (ddlstudtype.SelectedItem.Text != "All" && ddlstudtype.SelectedItem.Text != "")
                studType = Convert.ToString(ddlstudtype.SelectedItem.Value);
            string feesType = string.Empty;
            feesType = Convert.ToString(ddlfeetype.SelectedItem.Text);
            string sem = Convert.ToString(getCblSelectedValue(cbl_sem));
            string paid = Convert.ToString(getCblSelectedValue(chkl_paid));
            //string Finyearvalue = Convert.ToString(getCblSelectedValue(chklsfyear));
            string Finyearvalue = Convert.ToString(ddlfinlyear.SelectedItem.Value);
            string headervalue = string.Empty;
            string ledgerValue = string.Empty;
            string strtype = string.Empty;
            if (ddlacctype.SelectedIndex == 0)//|| ddlacctype.SelectedIndex == 1
            {
                headervalue = Convert.ToString(getCblSelectedValue(cblheader));
                strtype = ",headerfk";
            }
            else
            {
                // headerValue = ledgervalue();
                strtype = ",ledgerfk";
            }

            string studMode = Convert.ToString(getCblSelectedValue(cbl_type));


            #region include
            DateTime fromdt;
            DateTime todt;
            if (cbdate.Checked)
            {
                fromdate = txt_fromdate.Text;
                todate = txt_todate.Text;
                string[] frdate = fromdate.Split('/');
                if (frdate.Length == 3)
                    fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
                string[] tdate = todate.Split('/');
                if (tdate.Length == 3)
                    todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
                fromdt = Convert.ToDateTime(fromdate);
                todt = Convert.ToDateTime(todate);
            }
            if (cbbankcheck.Checked)
            {
                for (int i = 0; i < chkl_paid.Items.Count; i++)
                {
                    if (chkl_paid.Items[i].Selected == true && chkl_paid.Items[i].Text == "Online Pay")
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
            }
            #endregion
            string strInclude = getStudCategory();
            string applynStr = " AND r.IsConfirm = 1  and isnull(is_enroll,'0')<>'2'";
            // dtPaidHeader(strtype, batch, degree, feecat, collegecode, studMode, strInclude, headerValue, fnlYear, applynStr, fromdate, todate, payMode, headerValue);
            #endregion

            string SelQ = string.Empty;
            if (ddlfeetype.SelectedItem.Text.Trim() == "Paid")
            {
                SelQ = " select distinct feecategory" + strtype + " from (";
                SelQ += " select  f.feecategory" + strtype + " from ft_findailytransaction f,registration r where f.app_no=r.app_no  and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "')  and f.headerfk in('" + headervalue + "')  and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "'   and isnull(debit,'0')>0 and isnull(actualfinyearfk,'0')<>'0'  and isnull(IsCanceled,'0')='0' and isnull(isdeposited,'0')='1' and  ISNULL(IsCollected,0)='1' and isnull(transcode,'')<>'' and r.mode in('" + studMode + "')    " + strInclude + "";//and f.Transdate between '" + fromdate + "' and '" + todate + "'
                if (cbbfadm.Checked)
                {
                    SelQ += " union all select f.feecategory" + strtype + " from ft_findailytransaction f,applyn r where f.app_no=r.app_no and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "')  and f.headerfk in('" + headervalue + "')  and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + applynStr + "  and isnull(debit,'0')>0 and isnull(actualfinyearfk,'0')<>'0'  and isnull(IsCanceled,'0')='0' and isnull(isdeposited,'0')='1' and  ISNULL(IsCollected,0)='1' and isnull(transcode,'')<>'' and r.mode in('" + studMode + "')  ";//and f.Transdate between '" + fromdate + "' and '" + todate + "'
                }
                SelQ += ") tbl ";
            }
            else if (ddlfeetype.SelectedItem.Text.Trim() == "Yet To Be Paid")//yet to be paid
            {
                SelQ = " select distinct feecategory" + strtype + " from (";
                SelQ += " select  f.feecategory" + strtype + " from ft_feeallot f,registration r where f.app_no=r.app_no  and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "')  and f.headerfk in('" + headervalue + "')   and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "'   and isnull(balamount,'0')>0  and r.mode in('" + studMode + "')    " + strInclude + "";//and f.Transdate between '" + fromdate + "' and '" + todate + "'
                if (cbbfadm.Checked)
                {
                    SelQ += " union all select f.feecategory" + strtype + " from ft_findailytransaction f,applyn r where f.app_no=r.app_no and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "')  and f.headerfk in('" + headervalue + "')  and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + applynStr + "  and isnull(balamount,'0')>0  and r.mode in('" + studMode + "')  ";//and f.Transdate between '" + fromdate + "' and '" + todate + "'
                }
                SelQ += ") tbl ";
            }
            dtHdName = inspro.selectDataTable(SelQ);
        }
        catch { }
        return dtHdName;
    }

    //discontinue,delflag
    protected string getStudCategory()
    {
        string strInclude = string.Empty;
        try
        {
            #region includem

            string cc = "";
            string debar = "";
            string disc = "";
            string cancel = "";
            if (cblinclude.Items.Count > 0)
            {
                for (int i = 0; i < cblinclude.Items.Count; i++)
                {
                    if (cblinclude.Items[i].Selected == true)
                    {
                        if (cblinclude.Items[i].Value == "1")
                            cc = " r.cc=1";
                        if (cblinclude.Items[i].Value == "2")
                            debar = " r.Exam_Flag like '%debar'";
                        if (cblinclude.Items[i].Value == "3")
                            disc = "  r.DelFlag=1";
                        if (cblinclude.Items[i].Value == "4")
                            cancel = "  r.DelFlag=2";
                    }
                }
            }
            if (!checkdicon.Checked)
            {
                if (cc != "" && debar == "" && disc == "" && cancel == "")
                    strInclude = " and (" + cc + " or r.cc=0)  and r.Exam_Flag<>'debar' and r.DelFlag=0";
                if (cc == "" && debar != "" && disc == "" && cancel == "")
                    strInclude = " and r.cc=0  and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";
                if (cc == "" && debar == "" && disc != "" && cancel == "")
                    strInclude = " and r.cc=0  and r.Exam_Flag<>'debar' and (" + disc + " or r.DelFlag=0)";
                if (cc == "" && debar == "" && disc == "" && cancel != "")
                    strInclude = " and r.cc=0  and r.Exam_Flag<>'debar' and (" + cancel + " or r.DelFlag=0)";
                //2
                if (cc != "" && debar != "" && disc == "" && cancel == "")
                    strInclude = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";
                if (cc != "" && debar == "" && disc != "" && cancel == "")
                    strInclude = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar' and (" + disc + " or r.DelFlag=0)";
                if (cc != "" && debar == "" && disc == "" && cancel != "")
                    strInclude = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar' and (" + cancel + " or r.DelFlag=0)";
                //
                if (cc == "" && debar != "" && disc != "" && cancel == "")
                    strInclude = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar') and (" + disc + " or r.DelFlag=0)";
                if (cc == "" && debar != "" && disc == "" && cancel != "")
                    strInclude = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar') and (" + cancel + " or r.DelFlag=0)";
                //
                if (cc == "" && debar == "" && disc != "" && cancel != "")
                    strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and (" + disc + " or " + cancel + " or r.DelFlag=0)";
                //3
                if (cc != "" && debar != "" && disc != "" && cancel == "")
                    strInclude = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and (" + disc + " or r.DelFlag=0)";
                if (cc != "" && debar == "" && disc != "" && cancel != "")
                    strInclude = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar' and (" + disc + " or " + cancel + " or r.DelFlag=0)";
                if (cc != "" && debar != "" && disc == "" && cancel != "")
                    strInclude = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and (" + cancel + " or r.DelFlag=0)";
                if (cc == "" && debar != "" && disc != "" && cancel != "")
                    strInclude = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar') and (" + disc + " or " + cancel + " or r.DelFlag=0)";
                if (cc == "" && debar == "" && disc == "" && cancel == "")
                    strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and  r.DelFlag=0";
                if (cc != "" && debar != "" && disc != "" && cancel != "")
                    strInclude = "";
            }
            else
            {
                if (cc != "" && debar == "" && disc == "" && cancel == "")
                    strInclude = " and " + cc + "";
                if (cc == "" && debar != "" && disc == "" && cancel == "")
                    strInclude = " and " + debar + "";
                if (cc == "" && debar == "" && disc != "" && cancel == "")
                    strInclude = " and " + disc + "";
                if (cc == "" && debar == "" && disc == "" && cancel != "")
                    strInclude = " and " + cancel + "";
                //2
                if (cc != "" && debar != "" && disc == "" && cancel == "")
                    strInclude = " and( " + cc + " or " + debar + ")";
                if (cc != "" && debar == "" && disc != "" && cancel == "")
                    strInclude = " and (" + cc + " or " + disc + ")";
                if (cc != "" && debar == "" && disc == "" && cancel != "")
                    strInclude = " and (" + cc + " or " + cancel + ")";
                //
                if (cc == "" && debar != "" && disc != "" && cancel == "")
                    strInclude = " and (" + debar + " or " + disc + ")";
                if (cc == "" && debar != "" && disc == "" && cancel != "")
                    strInclude = " and (" + debar + " or " + cancel + ")";
                //
                if (cc == "" && debar == "" && disc != "" && cancel != "")
                    strInclude = " and (" + disc + " or " + cancel + ")";
                //3
                if (cc != "" && debar != "" && disc != "" && cancel == "")
                    strInclude = " and (" + cc + " or " + debar + " or " + disc + ")";
                if (cc != "" && debar == "" && disc != "" && cancel != "")
                    strInclude = " and (" + cc + " or " + disc + " or " + cancel + ")";
                if (cc != "" && debar != "" && disc == "" && cancel != "")
                    strInclude = " and (" + cc + " or " + debar + " or " + cancel + ")";
                if (cc == "" && debar != "" && disc != "" && cancel != "")
                    strInclude = " and (" + debar + " or " + disc + " or " + cancel + ")";
                if (cc == "" && debar == "" && disc == "" && cancel == "")
                    strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and  r.DelFlag=0";
                if (cc != "" && debar != "" && disc != "" && cancel != "")
                    strInclude = " and (" + cc + " or " + debar + " or " + disc + " or " + cancel + ")";
            }

            #endregion
        }
        catch { }
        return strInclude;
    }

    protected Hashtable getDeptName()
    {
        Hashtable htdtName = new Hashtable();
        try
        {
            collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            string SelQ = " select distinct d.degree_code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename,(c.Course_Name +'-'+dt.dept_acronym)as dept_acronym from degree d,department dt,course c where c.course_id=d.course_id and d.dept_code=dt.dept_code and d.college_code in('" + collegecode + "')";
            DataSet dsdeg = d2.select_method_wo_parameter(SelQ, "Text");
            if (dsdeg.Tables.Count > 0 && dsdeg.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsdeg.Tables[0].Rows.Count; row++)
                {
                    if (!htdtName.ContainsKey(Convert.ToString(dsdeg.Tables[0].Rows[row]["degree_code"])))
                        htdtName.Add(Convert.ToString(dsdeg.Tables[0].Rows[row]["degree_code"]), Convert.ToString(dsdeg.Tables[0].Rows[row]["dept_acronym"]));
                }
            }
        }
        catch { }
        return htdtName;
    }
    protected Hashtable getHeaderFK()
    {
        Hashtable hthdName = new Hashtable();
        try
        {
            string selQFK = string.Empty;
            if (ddlacctype.SelectedItem.Text.Trim() == "Header")
                selQFK = "  select distinct headerpk as pk,headername as name from fm_headermaster where collegecode in('" + ddlcollege.SelectedValue + "') ";
            else
                selQFK = "   select distinct ledgername as name,ledgerpk as pk from fm_headermaster h,fm_ledgermaster l where h.headerpk=l.headerfk and h.collegecode in('" + ddlcollege.SelectedValue + "') ";
            DataSet dsval = d2.select_method_wo_parameter(selQFK, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsval.Tables[0].Rows.Count; row++)
                {
                    if (!hthdName.ContainsKey(Convert.ToString(dsval.Tables[0].Rows[row]["pk"])))
                        hthdName.Add(Convert.ToString(dsval.Tables[0].Rows[row]["pk"]), Convert.ToString(dsval.Tables[0].Rows[row]["name"]));
                }
            }
        }
        catch { hthdName.Clear(); }
        return hthdName;
    }
    protected Hashtable getFinyear()
    {
        Hashtable htfin = new Hashtable();
        try
        {
            string SelQ = "  select (convert(varchar(10),datepart(year,finyearstart))+'-'+convert(varchar(10),datepart(year,finyearend)))as finyear,finyearpk,collegecode from fm_finyearmaster where collegecode='" + ddlcollege.SelectedValue + "'";
            DataSet dsval = d2.select_method_wo_parameter(SelQ, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsval.Tables[0].Rows.Count; row++)
                {
                    if (!htfin.ContainsKey(Convert.ToString(dsval.Tables[0].Rows[row]["finyearpk"])))
                        htfin.Add(Convert.ToString(dsval.Tables[0].Rows[row]["finyearpk"]), Convert.ToString(dsval.Tables[0].Rows[row]["finyear"]));
                }
            }
        }
        catch { htfin.Clear(); }
        return htfin;
    }

    //roll,reg,admission no setting based
    protected void spreadSclColumnVisible()
    {
        try
        {
            #region
            if (roll == 0)
            {
                spreadDet.Columns[2].Visible = true;
                spreadDet.Columns[3].Visible = true;
                spreadDet.Columns[4].Visible = true;
            }
            else if (roll == 1)
            {
                spreadDet.Columns[2].Visible = true;
                spreadDet.Columns[3].Visible = true;
                spreadDet.Columns[4].Visible = true;
            }
            else if (roll == 2)
            {
                spreadDet.Columns[2].Visible = true;
                spreadDet.Columns[3].Visible = false;
                spreadDet.Columns[4].Visible = false;

            }
            else if (roll == 3)
            {
                spreadDet.Columns[2].Visible = false;
                spreadDet.Columns[3].Visible = true;
                spreadDet.Columns[4].Visible = false;
            }
            else if (roll == 4)
            {
                spreadDet.Columns[2].Visible = false;
                spreadDet.Columns[3].Visible = false;
                spreadDet.Columns[4].Visible = true;
            }
            else if (roll == 5)
            {
                spreadDet.Columns[2].Visible = true;
                spreadDet.Columns[3].Visible = true;
                spreadDet.Columns[4].Visible = false;
            }
            else if (roll == 6)
            {
                spreadDet.Columns[2].Visible = false;
                spreadDet.Columns[3].Visible = true;
                spreadDet.Columns[4].Visible = true;
            }
            else if (roll == 7)
            {
                spreadDet.Columns[2].Visible = true;
                spreadDet.Columns[3].Visible = false;
                spreadDet.Columns[4].Visible = true;
            }
            #endregion
        }
        catch { }
    }
}