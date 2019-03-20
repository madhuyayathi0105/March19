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
using System.Text;

public partial class Finance_MIS_Budget_Report : System.Web.UI.Page
{
    DAccess2 da = new DAccess2();
    DataSet ds1 = new DataSet();
    DataSet ds = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds2 = new DataSet();
    Boolean finalflag = false;
    Boolean deptflag = false;
    string usercode = "", collegecode = "", singleuser = "", group_user = "";
    string course_id = string.Empty;
    int i = 0;
    string headid = "";
    static TreeNode node;
    TreeNode subchildnode;
    Hashtable htfeecat = new Hashtable();
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
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            errmsg.Visible = false;
            if (!IsPostBack)
            {
                setLabelText();
                clear();
                loadfinanceyear();
                loadtype();
                BindBatch();
                BindDegree();
                BindBranch();
                bindsem();
                ddlacctype_change(sender, e);
                treeview_spreadfields.Attributes.Add("onclick", "OnCheckBoxCheckChanged(event)");
                chk_datewise_OnCheckedChanged(sender, e);
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_todate.Attributes.Add("readonly", "readonly");
                loadseat();
            }
        }
        catch (Exception ex) { }
    }
    public void loadtype()
    {
        try
        {
            ddltype.Items.Clear();
            collegecode = Session["collegecode"].ToString();
            string strquery = "select distinct type from course where college_code='" + collegecode + "' and type is not null and type<>''";
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
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
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
            ds2 = da.BindBatch();
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
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    public void BindDegree()
    {
        try
        {
            string usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
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
                txtdegree.Text = "Degree (1)";
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    public void BindBranch()
    {
        try
        {
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
                txtbranch.Text = "Branch (1)";
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    public void loadheader()
    {
        try
        {
            chklstaccheader.Items.Clear();
            txtaccheader.Text = "---Select---";
            chkaccheader.Checked = false;
            string type = "";
            if (ddltype.Enabled == true)
            {
                if (ddltype.Items.Count > 0)
                {
                    if (ddltype.SelectedItem.ToString() != "Both" && ddltype.SelectedItem.ToString() != "")
                    {
                        //type = " HeaderType='" + ddltype.SelectedItem.ToString() + "' and ";
                        type = " and Stream='" + ddltype.SelectedItem.ToString() + "'";
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
                strheadid = " and HeaderPK in (" + strheadid + ")";
            }
            ds.Reset();
            ds.Dispose();
            if (strheadid.Trim() != "")
            {
                //string straccheadquery = "select header_name,header_id from Acctheader where " + type + "  header_name not in ('arrear')";
                // string straccheadquery = "select distinct HeaderPK,HeaderName from FM_HeaderMaster a where CollegeCode='" + collegecode + "'";
                string straccheadquery = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + collegecode + "  ";
                ds = da.select_method_wo_parameter(straccheadquery, "Text");
                if (ddlacctype.SelectedItem.Text == "Ledger")
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            node = new TreeNode(ds.Tables[0].Rows[i]["HeaderName"].ToString(), ds.Tables[0].Rows[i]["HeaderPK"].ToString());
                            // string strled = "select LedgerName,LedgerPK from FM_LedgerMaster where LedgerName not in ('Cash','Income & Expenditure','Misc') and HeaderFK = (" + ds.Tables[0].Rows[i]["HeaderPK"].ToString() + ") order by LedgerPK";
                            string strled = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode and LedgerName not in ('Cash','Income & Expenditure','Misc') AND P. UserCode = " + usercode + "  and L.CollegeCode = " + collegecode + "  and L.HeaderFK in(" + ds.Tables[0].Rows[i]["HeaderPK"].ToString() + ")   order by isnull(l.priority,1000), l.ledgerName asc ";
                            ds1 = da.select_method_wo_parameter(strled, "Text");
                            for (int ledge = 0; ledge < ds1.Tables[0].Rows.Count; ledge++)
                            {
                                subchildnode = new TreeNode(ds1.Tables[0].Rows[ledge]["LedgerName"].ToString(), ds1.Tables[0].Rows[ledge]["LedgerPK"].ToString());
                                subchildnode.ShowCheckBox = true;
                                node.ChildNodes.Add(subchildnode);

                            }
                            node.ShowCheckBox = true;
                            treeview_spreadfields.Nodes.Add(node);
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
                    }
                }
                else
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        chklstaccheader.DataSource = ds;
                        chklstaccheader.DataTextField = "HeaderName";
                        chklstaccheader.DataValueField = "HeaderPK";
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
                    else
                    {
                        chkaccheader.Checked = false;
                        txtaccheader.Text = "--Select--";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    public void groupheader()
    {
        try
        {
            chklstaccheader.Items.Clear();
            txtaccheader.Text = "---Select---";
            ds.Reset();
            ds.Dispose();

            string stream = "";
            string selstream = "select distinct type from course where college_code='" + collegecode + "' and type is not null and type<>''";
            ds.Clear();
            ds = da.select_method_wo_parameter(selstream, "Text");
            if (ddltype.Enabled == true)
            {
                if (ddltype.SelectedItem.Text == "Both")
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int st = 0; st < ds.Tables[0].Rows.Count; st++)
                            {
                                if (stream.Trim() == "")
                                {
                                    stream = "" + Convert.ToString(ds.Tables[0].Rows[st]["type"]) + "";
                                }
                                else
                                {
                                    stream = stream + "'" + "," + "'" + Convert.ToString(ds.Tables[0].Rows[st]["type"]);
                                }
                            }
                        }
                    }
                }
                else
                {
                    stream = Convert.ToString(ddltype.SelectedItem.Value);
                    //string straccheadquery = "select distinct ChlGroupHeader from FS_ChlGroupHeaderSettings where ChlGroupHeader IS NOT NULL and ChlGroupHeader!='' and Stream='" + ddltype.SelectedItem.Text.ToString() + "' ";
                    //ds = da.select_method_wo_parameter(straccheadquery, "Text");
                }
            }

            string straccheadquery = "select distinct ChlGroupHeader from FS_ChlGroupHeaderSettings where ChlGroupHeader IS NOT NULL and ChlGroupHeader!='' and Stream in('" + stream + "')";
            ds.Clear();
            ds = da.select_method_wo_parameter(straccheadquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklstaccheader.DataSource = ds;
                chklstaccheader.DataTextField = "ChlGroupHeader";
                chklstaccheader.DataValueField = "ChlGroupHeader";
                chklstaccheader.DataBind();
            }
            for (int i = 0; i < chklstaccheader.Items.Count; i++)
            {
                chklstaccheader.Items[i].Selected = true;
            }
            chkaccheader.Checked = true;
            txtaccheader.Text = "Group Header (" + chklstaccheader.Items.Count + ")";
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    public void loadfinanceyear()
    {
        try
        {
            string getfinanceyear = "select convert(nvarchar(15),FinYearStart,103) sdate,convert(nvarchar(15),FinYearEnd,103) edate,FinYearPK as acct_id from FM_FinYearMaster where CollegeCode='" + collegecode + "' order by FinYearPK";
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
                    string actid = ds.Tables[0].Rows[i]["acct_id"].ToString();
                    //chklsfyear.Items.Insert(fdatye, ds.Tables[0].Rows[i]["acct_id"].ToString());
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
            clear();
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
            //loadheader();
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
            clear();
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
            // loadheader();
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("default.aspx", false);

    }

    protected void chkbatch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (chkbatch.Checked == true)
            {
                for (int i = 0; i < chklstbatch.Items.Count; i++)
                {
                    chklstbatch.Items[i].Selected = true;
                }
                txtbatch.Text = "Batch (" + (chklstbatch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstbatch.Items.Count; i++)
                {
                    chklstbatch.Items[i].Selected = false;
                }
                txtbatch.Text = "---Select---";
            }
            BindBranch();
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void chklstbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            int batchcount = 0;
            chkbatch.Checked = false;
            txtbatch.Text = "--Select--";
            for (int i = 0; i < chklstbatch.Items.Count; i++)
            {
                if (chklstbatch.Items[i].Selected == true)
                {
                    batchcount = batchcount + 1;
                }
            }

            if (batchcount > 0)
            {
                txtbatch.Text = "Batch (" + batchcount.ToString() + ")";
                if (batchcount == chklstbatch.Items.Count)
                {
                    chkbatch.Checked = true;
                }
            }
            BindBranch();
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void chkdegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (chkdegree.Checked == true)
            {
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = true;
                }
                txtdegree.Text = lbldeg.Text + " (" + (chklstdegree.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = false;
                }
                txtdegree.Text = "---Select---";
            }

            BindBranch();
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void chklstdegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            int degreecount = 0;
            txtdegree.Text = "--Select--";
            chkdegree.Checked = false;
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    degreecount = degreecount + 1;
                }
            }
            if (degreecount > 0)
            {
                txtdegree.Text = lbldeg.Text + " (" + degreecount.ToString() + ")";
                if (degreecount == chklstdegree.Items.Count)
                {
                    chkdegree.Checked = true;
                }
            }
            BindBranch();
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void chkbranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (chkbranch.Checked == true)
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = true;
                }
                txtbranch.Text = lbldept.Text + "(" + (chklstbranch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = false;
                }
                txtbranch.Text = "---Select---";
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void chklstbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            int branchcount = 0;
            chkbranch.Checked = false;
            txtbranch.Text = "--Select--";
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    branchcount = branchcount + 1;
                }
            }
            if (branchcount > 0)
            {
                txtbranch.Text = lbldept.Text + " (" + branchcount.ToString() + ")";
                if (branchcount == chklstbranch.Items.Count)
                {
                    chkbranch.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void ddlacctype_change(object sender, EventArgs e)
    {
        try
        {
            clear();
            treeview_spreadfields.Visible = false;
            treeview_spreadfields.Nodes.Clear();
            if (ddlacctype.SelectedItem.Text == "Group Header")
            {
                Label1.Text = "Group Header";
                groupheader();
                txtaccheader.Enabled = true;
            }
            else if (ddlacctype.SelectedItem.Text == "Header")
            {
                Label1.Text = "Account Header";
                loadheader();
                txtaccheader.Enabled = true;
            }
            else
            {
                treeview_spreadfields.Visible = true;
                loadheader();
                Label1.Text = "Account Header";
                txtaccheader.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    public void loadseat()
    {
        try
        {
            ddlseattype.Items.Clear();

            string seat = "";
            string deptquery = "select distinct TextCode,TextVal from TextValTable  where TextCriteria='seat' and college_code='" + collegecode + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlseattype.DataSource = ds;
                ddlseattype.DataTextField = "TextVal";
                ddlseattype.DataValueField = "TextCode";
                ddlseattype.DataBind();
                ddlseattype.Items.Insert(0, "Select");
            }

        }
        catch
        {
        }

    }
    //protected void chkaccheader_CheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        clear();
    //        if (ddlacctype.SelectedItem.Text == "Ledger")
    //        {
    //            if (chkaccheader.Checked == true)
    //            {
    //                for (int remv = 0; remv < treeview_spreadfields.Nodes.Count; remv++)
    //                {
    //                    treeview_spreadfields.Nodes[remv].Checked = true;
    //                    txtaccheader.Text = "Header(" + (treeview_spreadfields.Nodes.Count) + ")";
    //                    if (treeview_spreadfields.Nodes[remv].ChildNodes.Count > 0)
    //                    {
    //                        for (int child = 0; child < treeview_spreadfields.Nodes[remv].ChildNodes.Count; child++)
    //                        {
    //                            treeview_spreadfields.Nodes[remv].ChildNodes[child].Checked = true;
    //                        }
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                for (int remv = 0; remv < treeview_spreadfields.Nodes.Count; remv++)
    //                {
    //                    treeview_spreadfields.Nodes[remv].Checked = false;
    //                    txtaccheader.Text = "---Select---";
    //                    if (treeview_spreadfields.Nodes[remv].ChildNodes.Count > 0)
    //                    {
    //                        for (int child = 0; child < treeview_spreadfields.Nodes[remv].ChildNodes.Count; child++)
    //                        {
    //                            treeview_spreadfields.Nodes[remv].ChildNodes[child].Checked = false;
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        else
    //        {
    //            if (chkaccheader.Checked == true)
    //            {
    //                for (int i = 0; i < chklstaccheader.Items.Count; i++)
    //                {
    //                    chklstaccheader.Items[i].Selected = true;
    //                }
    //                if (ddlacctype.SelectedItem.Text == "Group Header")
    //                {
    //                    txtaccheader.Text = "Group Header (" + chklstaccheader.Items.Count + ")";
    //                }
    //                else
    //                {
    //                    txtaccheader.Text = "Header (" + chklstaccheader.Items.Count + ")";
    //                }
    //            }
    //            else
    //            {
    //                for (int i = 0; i < chklstaccheader.Items.Count; i++)
    //                {
    //                    chklstaccheader.Items[i].Selected = false;
    //                }
    //                txtaccheader.Text = "---Select---";
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        errmsg.Visible = true;
    //        errmsg.Text = ex.ToString();
    //    }
    //}
    //protected void chklstaccheader_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        clear();
    //        string clg = "";
    //        int commcount = 0;
    //        chkaccheader.Checked = false;
    //        txtaccheader.Text = "--Select--";
    //        for (int i = 0; i < chklstaccheader.Items.Count; i++)
    //        {
    //            if (chklstaccheader.Items[i].Selected == true)
    //            {
    //                commcount = commcount + 1;

    //                if (clg == "")
    //                {
    //                    clg = chklstaccheader.Items[i].Value.ToString();
    //                }
    //                else
    //                {
    //                    clg = clg + "," + chklstaccheader.Items[i].Value;
    //                }
    //            }
    //        }
    //        string set = "Header";
    //        if (ddlacctype.SelectedItem.Text == "Group Header")
    //        {
    //            set = "Group Header";
    //        }
    //        if (commcount > 0)
    //        {
    //            txtaccheader.Text = "" + set + "(" + commcount.ToString() + ")";
    //            if (commcount == chklstaccheader.Items.Count)
    //            {
    //                chkaccheader.Checked = true;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        errmsg.Visible = true;
    //        errmsg.Text = ex.ToString();
    //    }
    //}
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
    protected void ddlstudtype_change(object sender, EventArgs e)
    {
        try
        {
            clear();
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        BindBatch();
        BindDegree();
        BindBranch();
        treeview_spreadfields.Visible = false;
        treeview_spreadfields.Nodes.Clear();
        if (ddlacctype.SelectedItem.Text == "Group Header")
        {
            Label1.Text = "Group Header";
            groupheader();
            txtaccheader.Enabled = true;

        }
        else if (ddlacctype.SelectedItem.Text == "Header")
        {
            Label1.Text = "Account Header";
            loadheader();
            txtaccheader.Enabled = true;
        }
        else
        {
            treeview_spreadfields.Visible = true;
            loadheader();
            Label1.Text = "Account Header";
            txtaccheader.Enabled = true;
        }
    }
    public void clear()
    {
        FpSpread1.Visible = false;
        txtexcelname.Text = "";
        FpSpread1.Visible = false;
        btnprintmaster.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnxl.Visible = false;
        Printcontrol.Visible = false;
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {

        if (!cbdue.Checked)
        {
            withoutdue();
        }
        else
        {
            withdue();
        }
    }
    protected string getCurrentSemester(string batch, string degree, ref ArrayList arFeecat, string commondist, ref Dictionary<string, string> CurSemDeg)
    {
        string curSem = string.Empty;
        try
        {
            if (cbCurSem.Checked)
            {
                string type = string.Empty;
                string strtype = da.GetFunction("select LinkValue from New_InsSettings where college_code='" + collegecode + "' and LinkName='Fee Yearwise' And user_code = '" + usercode + "'");
                if (strtype == "0")
                {
                    type = "Semester";
                }
                else if (strtype == "1")
                {
                    type = "Yearly";
                }
                else if (strtype == "2")
                {
                    type = "Term";
                }
                StringBuilder sbCurSem = new StringBuilder();
                string selQ = " select distinct current_semester,(cast(batch_year as nvarchar(10))+'-'+cast(degree_code as nvarchar(10))+'-'+cast(r.current_semester as nvarchar(10))) as batch,degree_code,batch_year from registration r where batch_year in(" + batch + ") and degree_code in(" + degree + ")";
                if (!string.IsNullOrEmpty(commondist))
                    selQ += commondist;
                selQ += " order by degree_code,batch_year desc";
                DataSet dsVal = da.select_method_wo_parameter(selQ, "Text");
                if (dsVal.Tables.Count > 0 && dsVal.Tables[0].Rows.Count > 0)
                {
                    for (int row = 0; row < dsVal.Tables[0].Rows.Count; row++)
                    {
                        string batDeg = Convert.ToString(dsVal.Tables[0].Rows[row]["batch"]);
                        sbCurSem.Append(Convert.ToString(dsVal.Tables[0].Rows[row]["current_semester"]) + "','");
                        string typeStr = string.Empty;
                        if (type == "Term")
                            typeStr = type + " " + Convert.ToString(dsVal.Tables[0].Rows[row]["current_semester"]);
                        else
                            typeStr = Convert.ToString(dsVal.Tables[0].Rows[row]["current_semester"]) + " " + type;
                        string feecode = Convert.ToString(da.GetFunction("select textcode from textvaltable where textcriteria='FEECA' and textval like '" + typeStr + "%' and college_code='" + collegecode + "'"));
                        if (!arFeecat.Contains(feecode))
                        {
                            arFeecat.Add(feecode);
                        }
                        if (!CurSemDeg.ContainsKey(batDeg))
                        {
                            CurSemDeg.Add(batDeg, feecode);
                        }
                    }
                    if (sbCurSem.Length > 0)
                    {
                        sbCurSem.Remove(sbCurSem.Length - 3, 3);
                        curSem = Convert.ToString(sbCurSem);
                    }
                }
            }
        }
        catch
        {
            curSem = string.Empty;
            arFeecat.Clear();
        }
        return curSem;

    }
    protected string getCurrentSemester(string batch, string degree, ref ArrayList arFeecat, string commondist)
    {
        string curSem = string.Empty;

        try
        {
            if (cbCurSem.Checked)
            {
                string type = string.Empty;
                string strtype = da.GetFunction("select LinkValue from New_InsSettings where college_code='" + collegecode + "' and LinkName='Fee Yearwise' And user_code = '" + usercode + "'");
                if (strtype == "0")
                {
                    type = "Semester";
                }
                else if (strtype == "1")
                {
                    type = "Yearly";
                }
                else if (strtype == "2")
                {
                    type = "Term";
                }
                StringBuilder sbCurSem = new StringBuilder();
                string selQ = " select distinct current_semester from registration r where batch_year in(" + batch + ") and degree_code in(" + degree + ")";
                if (!string.IsNullOrEmpty(commondist))
                    selQ += commondist;
                DataSet dsVal = da.select_method_wo_parameter(selQ, "Text");
                if (dsVal.Tables.Count > 0 && dsVal.Tables[0].Rows.Count > 0)
                {
                    for (int row = 0; row < dsVal.Tables[0].Rows.Count; row++)
                    {
                        sbCurSem.Append(Convert.ToString(dsVal.Tables[0].Rows[row]["current_semester"]) + "','");
                        string typeStr = string.Empty;
                        if (type == "Term")
                            typeStr = type + " " + Convert.ToString(dsVal.Tables[0].Rows[row]["current_semester"]);
                        else
                            typeStr = Convert.ToString(dsVal.Tables[0].Rows[row]["current_semester"]) + " " + type;
                        string feecode = Convert.ToString(da.GetFunction("select textcode from textvaltable where textcriteria='FEECA' and textval like '" + typeStr + "%' and college_code='" + collegecode + "'"));
                        if (!arFeecat.Contains(feecode))
                        {

                            arFeecat.Add(feecode);
                        }
                    }
                    if (sbCurSem.Length > 0)
                    {
                        sbCurSem.Remove(sbCurSem.Length - 3, 3);
                        curSem = Convert.ToString(sbCurSem);
                    }
                }
            }
        }
        catch
        {
            curSem = string.Empty;
            arFeecat.Clear();
        }
        return curSem;

    }

    protected void withoutdue()
    {
        try
        {
            clear();
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = Color.Black;
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(darkstyle);



            #region get value
            string seattext = "";
            string batchquery = "";
            string degreequery = "";
            string setbatchanddegree = "";
            string getdegreedetails = "";
            string demquery = "";
            string batch_all = string.Empty;
            string course_all = string.Empty;
            string header_all = string.Empty;
            string category = string.Empty;
            string feecode = "";
            string tot_category = string.Empty;
            string paidquery = "";
            string demandfee = "";
            int sno = 0;
            string deg_acr = string.Empty;
            string deg_dept = string.Empty;

            string getfeeval = "";

            FarPoint.Web.Spread.TextCellType txtcel = new FarPoint.Web.Spread.TextCellType();

            if (ddlseattype.Items.Count > 0)
                seattext = Convert.ToString(ddlseattype.SelectedItem.Text);

            if (ddlacctype.Text == "---Select---")
            {
                finalflag = true;
                errmsg.Visible = true;
                errmsg.Text = "Please Select Account Type";

                return;
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
            if (batch_all.Trim() != "")
            {
                batchquery = " and r.Batch_year in(" + batch_all + ")";
                setbatchanddegree = " r.Batch_year in(" + batch_all + ")";
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
                    setbatchanddegree = setbatchanddegree + " and r.degree_code in(" + course_all + ")";
                }
                else
                {
                    setbatchanddegree = " and r.degree_code in(" + course_all + ")";
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

            string fnlYr = "";
            for (int t = 0; t < chklsfyear.Items.Count; t++)
            {
                if (chklsfyear.Items[t].Selected == true)
                {
                    if (fnlYr == "")
                    {
                        fnlYr = "'" + chklsfyear.Items[t].Value.ToString() + "'";
                    }
                    else
                    {
                        fnlYr = fnlYr + ",'" + chklsfyear.Items[t].Value.ToString() + "'";
                    }
                }
            }
            if (fnlYr.Trim() != "")
            {
                fnlYr = " and finyearfk in (" + fnlYr + ")";
            }
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
            //semester
            ArrayList arFeecat = new ArrayList();
            Dictionary<string, string> CurSemDeg = new Dictionary<string, string>();
            string curSem = getCurrentSemester(batch_all, course_all, ref arFeecat, commondist, ref CurSemDeg);//current semester only
            feeCatVal(curSem, arFeecat);
            Dictionary<string, string> htfeecat = new Dictionary<string, string>();
            htfeecat = (Dictionary<string, string>)ViewState["feecat"];
            string feecatval = "";
            string feecatg = "";
            if (htfeecat.Count > 0)
            {
                foreach (KeyValuePair<string, string> sem in htfeecat)
                {
                    if (feecatval == "")
                        feecatval = sem.Value.ToString();
                    else
                        feecatval += "'" + "," + "'" + sem.Value.ToString();
                }
                if (feecatval != "")
                    feecatg = " and a.FeeCategory in('" + feecatval + "')";
            }

            string fromdate = "";
            string todate = "";
            string date = "";
            if (chk_datewise.Checked == true)
            {
                fromdate = txt_fromdate.Text;
                todate = txt_todate.Text;
                if (fromdate != "" && todate != "")
                {
                    string[] frdate = fromdate.Split('/');
                    if (frdate.Length == 3)
                        fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();

                    string[] tdate = todate.Split('/');
                    if (tdate.Length == 3)
                        todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();

                    date = " AND TransDate between '" + fromdate + "' and '" + todate + "'";
                }
            }

            #endregion

            #region design

            //FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.BackColor = Color.FromArgb(214, 235, 255);
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 3;
            FpSpread1.Sheets[0].ColumnHeader.Columns.Count = 5;

            FpSpread1.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Rows[0].Font.Size = FontUnit.Large;
            FpSpread1.Sheets[0].ColumnHeader.Rows[0].Font.Name = "Book Antiqua";

            FpSpread1.Sheets[0].ColumnHeader.Rows[1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Rows[1].Font.Size = FontUnit.Large;
            FpSpread1.Sheets[0].ColumnHeader.Rows[1].Font.Name = "Book Antiqua";

            FpSpread1.Sheets[0].ColumnHeader.Rows[2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Rows[2].Font.Size = FontUnit.Large;
            FpSpread1.Sheets[0].ColumnHeader.Rows[2].Font.Name = "Book Antiqua";

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Batch Year";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = lbldept.Text;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = lblsem.Text;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Strength";


            FpSpread1.Sheets[0].Columns[1].Width = 200;

            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 3, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 3, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 3, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 3, 1);

            FpSpread1.Sheets[0].Columns[2].CellType = txtcel;

            DataView dvhead = new DataView();

            FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;

            #endregion
            string StudentFees = "";
            string strledgre = "";
            string strledger = "";
            bool boolColFnl = false;
            Dictionary<string, int> htcolCnt = new Dictionary<string, int>();
            if (ddlacctype.SelectedItem.Text == "Ledger")
            {
                #region ledger
                strledgre = ",a.LedgerFK as Fee_Code";
                strledger = ",a.LedgerFK";
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

                                FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 6;
                                if (headflag == false)
                                {
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 6].Text = treeview_spreadfields.Nodes[remv].Text.ToString();
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 6].HorizontalAlign = HorizontalAlign.Center;
                                    headflag = true;
                                    hstartc = FpSpread1.Sheets[0].ColumnCount - 6;
                                }
                                ledcount++;

                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6, 1, 6);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 6].Text = treeview_spreadfields.Nodes[remv].ChildNodes[child].Text.ToString();
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 6].HorizontalAlign = HorizontalAlign.Center;
                                feecode = treeview_spreadfields.Nodes[remv].ChildNodes[child].Value.ToString();
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6].Text = "Rate";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6].Note = feecode;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 5].Text = "Amount";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 5].Note = feecode;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 4].Text = "Collected";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 4].Note = feecode;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3].Text = "Arrear";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3].Note = feecode;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Text = "Total";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Note = feecode;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Concession";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Note = feecode;
                                boolColFnl = true;
                                ccount += 6;
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - ccount, 1, ccount - 1);
                                if (demandfee == "")
                                {
                                    demandfee = feecode;
                                    StudentFees = feecode;
                                }
                                else
                                {
                                    StudentFees = StudentFees + ',' + feecode;
                                    demandfee = demandfee + ',' + feecode;
                                }

                            }
                        }
                    }
                    else
                    {
                        FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 1;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = treeview_spreadfields.Nodes[remv].Text.ToString();
                    }
                }
                if (demandfee.Trim() != "")
                {
                    demandfee = " and a.LedgerFK in(" + demandfee + ")";
                    StudentFees = " and f.LedgerFK in(" + StudentFees + ")";
                }
                #endregion
            }
            else if (ddlacctype.SelectedItem.Text == "Group Header")
            {
                #region group header
                string head = "";
                string acchead = "select distinct h.HeaderName,g.HeaderFK,g.ChlGroupHeader from FS_ChlGroupHeaderSettings g,FM_HeaderMaster h where h.HeaderPK=g.HeaderFK";
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

                            FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 6;
                            if (headflag == false)
                            {
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 6].Text = chklstaccheader.Items[j].Text;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 6].HorizontalAlign = HorizontalAlign.Center;
                                headflag = true;
                                hstartc = FpSpread1.Sheets[0].ColumnCount - 6;
                            }
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6, 1, 6);
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 6].Text = dvhead[a]["HeaderName"].ToString();
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 6].HorizontalAlign = HorizontalAlign.Center;
                            if (dvhead.Count > 0)
                            {
                                feecode = dvhead[a]["HeaderFK"].ToString();
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6].Text = "Rate";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6].Note = feecode;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 5].Text = "Amount";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 5].Note = feecode;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 4].Text = "Collected";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 4].Note = feecode;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3].Text = "Arrear";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3].Note = feecode;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Text = "Total";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Note = feecode;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Concession";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Note = feecode;
                                boolColFnl = true;
                                if (demandfee == "")
                                {
                                    demandfee = feecode;
                                }
                                else
                                {
                                    demandfee = demandfee + ',' + feecode;
                                }
                            }
                            ccount += 6;
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - ccount, 1, ccount);
                        }
                    }

                }
                if (demandfee.Trim() != "")
                {
                    demandfee = " and a.HeaderFK in(" + demandfee + ")";

                }
                #endregion
            }
            else//Header..............
            {
                #region header

                for (int a = 0; a < chklstaccheader.Items.Count; a++)
                {
                    if (chklstaccheader.Items[a].Selected == true)
                    {
                        FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 6;
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6, 1, 6);
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 6].Text = chklstaccheader.Items[a].Text;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 6].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6].Text = "Rate";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6].Note = chklstaccheader.Items[a].Value;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 5].Text = "Amount";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 5].Note = chklstaccheader.Items[a].Value;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 4].Text = "Collected";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 4].Note = chklstaccheader.Items[a].Value;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3].Text = "Arrear";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3].Note = chklstaccheader.Items[a].Value;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Text = "Total";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Note = chklstaccheader.Items[a].Value;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Concession";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Note = chklstaccheader.Items[a].Value;
                        boolColFnl = true;
                        if (demandfee == "")
                        {
                            demandfee = chklstaccheader.Items[a].Value;
                        }
                        else
                        {
                            demandfee = demandfee + ',' + chklstaccheader.Items[a].Value;
                        }
                    }
                }
                if (demandfee.Trim() != "")
                {
                    StudentFees = " and f.HeaderFK in(" + demandfee + ")";
                    demandfee = " and a.HeaderFK in(" + demandfee + ")";
                }
                #endregion
            }
            int fnltot = 0;
            if (boolColFnl)
            {
                FpSpread1.Sheets[0].ColumnCount++;
                int colCnt = FpSpread1.Sheets[0].ColumnCount - 1;
                if (fnltot == 0)
                    fnltot = colCnt;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, colCnt].Text = "Total";
                if (ddlacctype.SelectedItem.Text == "Ledger" || ddlacctype.SelectedItem.Text == "Group Header")
                {
                    FpSpread1.Sheets[0].ColumnHeader.Cells[2, colCnt].Text = "Rate";
                    htcolCnt.Add("Rate", colCnt);
                    FpSpread1.Sheets[0].ColumnCount++;
                    colCnt = FpSpread1.Sheets[0].ColumnCount - 1;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[2, colCnt].Text = "Amount";
                    htcolCnt.Add("Amount", colCnt);
                    FpSpread1.Sheets[0].ColumnCount++;
                    colCnt = FpSpread1.Sheets[0].ColumnCount - 1;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[2, colCnt].Text = "Collected";
                    htcolCnt.Add("Collected", colCnt);
                    FpSpread1.Sheets[0].ColumnCount++;
                    colCnt = FpSpread1.Sheets[0].ColumnCount - 1;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[2, colCnt].Text = "Arrear";
                    htcolCnt.Add("Arrear", colCnt);
                    FpSpread1.Sheets[0].ColumnCount++;
                    colCnt = FpSpread1.Sheets[0].ColumnCount - 1;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[2, colCnt].Text = "Total";
                    htcolCnt.Add("Total", colCnt);
                    FpSpread1.Sheets[0].ColumnCount++;
                    colCnt = FpSpread1.Sheets[0].ColumnCount - 1;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[2, colCnt].Text = "Concession";
                    htcolCnt.Add("Concession", colCnt);

                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, fnltot, 2, 6);
                }
                else
                {
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, colCnt].Text = "Rate";
                    htcolCnt.Add("Rate", colCnt);
                    FpSpread1.Sheets[0].ColumnCount++;
                    colCnt = FpSpread1.Sheets[0].ColumnCount - 1;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, colCnt].Text = "Amount";
                    htcolCnt.Add("Amount", colCnt);
                    FpSpread1.Sheets[0].ColumnCount++;
                    colCnt = FpSpread1.Sheets[0].ColumnCount - 1;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, colCnt].Text = "Collected";
                    htcolCnt.Add("Collected", colCnt);
                    FpSpread1.Sheets[0].ColumnCount++;
                    colCnt = FpSpread1.Sheets[0].ColumnCount - 1;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, colCnt].Text = "Arrear";
                    htcolCnt.Add("Arrear", colCnt);
                    FpSpread1.Sheets[0].ColumnCount++;
                    colCnt = FpSpread1.Sheets[0].ColumnCount - 1;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, colCnt].Text = "Total";
                    htcolCnt.Add("Total", colCnt);
                    FpSpread1.Sheets[0].ColumnCount++;
                    colCnt = FpSpread1.Sheets[0].ColumnCount - 1;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, colCnt].Text = "Concession";
                    htcolCnt.Add("Concession", colCnt);

                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, fnltot, 1, 6);
                }
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, fnltot].HorizontalAlign = HorizontalAlign.Center;
            }

            //string seat = da.GetFunction("select TextCode from TextValTable where TextCriteria='seat' and textval like '%" + seattext + "%' and college_code='" + collegecode + "'");
            //if (seat != "0")
            //{

            #region Query

            #region new

            //demand
            demquery = "select count(distinct r.roll_no) as stucount,sum(feeamount) as demand,sum(DeductAmout) as deduction,r.degree_code,r.Batch_Year,a.HeaderFK as header_id" + strledgre + ",a.FeeCategory    from FT_FeeAllot a,registration r where a.App_No =r.App_No and (a.TotalAmount>0 or a.deductamout>0) " + fnlYr + "";
            if (commondist != "")
                demquery = demquery + commondist;
            if (chkinclude.Checked == true)
            {
                //demquery = demquery + " and r.DelFlag=0";
            }
            if (cbCurSem.Checked && !string.IsNullOrEmpty(curSem))
                demquery += " and r.current_semester in('" + curSem + "')";
            demquery = demquery + " " + degreequery + " " + batchquery + " " + demandfee + " " + feecatg + " group by r.degree_code,r.Batch_Year,a.HeaderFK" + strledger + ",a.FeeCategory  order by r.degree_code  ";


            //paid query
            paidquery = "select count(distinct r.roll_no) as stucount,sum(debit) as paid,r.degree_code,r.Batch_Year,a.HeaderFK as header_id" + strledgre + ",a.FeeCategory   from FT_FinDailyTransaction a,registration r where  a.App_No=r.App_No  and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' " + fnlYr + "";
            if (commondist != "")
                paidquery = paidquery + commondist;
            if (chkinclude.Checked == true)
            {
                // paidquery = paidquery + " and r.DelFlag=0";
            }
            if (cbCurSem.Checked && !string.IsNullOrEmpty(curSem))
                paidquery += " and r.current_semester in('" + curSem + "')";
            paidquery = paidquery + " " + degreequery + " " + batchquery + " " + demandfee + " " + feecatg + " " + date + " group by r.degree_code,r.Batch_Year,a.HeaderFK" + strledger + ",a.FeeCategory order by r.degree_code   ";


            //stud count
            string str = "select count(distinct r.roll_no) as stcount,r.Batch_Year,c.Course_Name,de.Dept_Name,r.degree_code,f.FeeCategory,r.current_semester  from Registration r,Degree d,Department de,Course c ,FT_FeeAllot f where f.App_No =r.App_No and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and c.Course_Id=d.Course_Id   " + fnlYr + "";//,r.Current_Semester
            if (commondist != "")
                str = str + commondist;
            if (chkinclude.Checked == true)
            {
                // str = str + " and r.DelFlag=0";
            }
            if (cbCurSem.Checked && !string.IsNullOrEmpty(curSem))
                str += " and r.current_semester in('" + curSem + "')";
            str = str + " " + degreequery + " " + batchquery + " and f.FeeCategory in('" + feecatval + "') " + StudentFees + " group by r.Batch_Year,c.Course_Name,de.Dept_Name,r.degree_code,f.FeeCategory,r.current_semester  order by r.degree_code,r.Batch_Year desc,f.FeeCategory ";//,r.Current_Semester

            //individual count          
            demquery += " select distinct sum(feeamount) as demand,sum(DeductAmout) as deduction,r.degree_code,r.Batch_Year,a.HeaderFK as header_id" + strledgre + ",a.FeeCategory    from FT_FeeAllot a,registration r where a.App_No =r.App_No and (a.TotalAmount>0 or a.deductamout>0) " + fnlYr + "";
            if (commondist != "")
                demquery = demquery + commondist;
            if (chkinclude.Checked == true)
            {
                // demquery = demquery + " and r.DelFlag=0";
            }
            if (cbCurSem.Checked && !string.IsNullOrEmpty(curSem))
                demquery += " and r.current_semester in('" + curSem + "')";
            demquery = demquery + "  " + degreequery + " " + batchquery + " " + demandfee + " " + feecatg + " group by r.degree_code,r.Batch_Year,a.HeaderFK" + strledger + ",a.FeeCategory,r.roll_no  order by r.degree_code  ";

            //amount count
            demquery += " select distinct r.roll_no as stucount,sum(feeamount) as demand,sum(DeductAmout) as deduction,r.degree_code,r.Batch_Year,a.HeaderFK as header_id" + strledgre + ",a.FeeCategory    from FT_FeeAllot a,registration r where a.App_No =r.App_No and (a.TotalAmount>0 or a.deductamout>0) " + fnlYr + "";
            if (commondist != "")
                demquery = demquery + commondist;
            if (chkinclude.Checked == true)
            {
                //  demquery = demquery + " and r.DelFlag=0";
            }
            if (cbCurSem.Checked && !string.IsNullOrEmpty(curSem))
                demquery += " and r.current_semester in('" + curSem + "')";
            demquery = demquery + "  " + degreequery + " " + batchquery + " " + demandfee + " " + feecatg + " group by r.degree_code,r.Batch_Year,a.HeaderFK" + strledger + ",a.FeeCategory,r.roll_no  order by r.degree_code ";

            //seme
            string semestselqry = " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code ='" + collegecode + "'";


            #endregion

            #endregion

            ds.Reset();
            ds.Dispose();
            ds = da.select_method_wo_parameter(str, "Text");

            ds1.Reset();
            ds1.Dispose();
            ds1 = da.select_method_wo_parameter(demquery, "Text");

            ds2.Reset();
            ds2.Dispose();
            ds2 = da.select_method_wo_parameter(paidquery, "Text");

            DataSet dssem = new DataSet();
            dssem.Reset();
            dssem.Dispose();
            dssem = da.select_method_wo_parameter(semestselqry, "Text");

            Dictionary<int, Double> dicgrandtotal = new Dictionary<int, double>();
            bool checksave = false;
            Hashtable httotal = new Hashtable();
            ArrayList ardegree = new ArrayList();
            if (ds.Tables[0].Rows.Count > 0)
            {
                #region feecategory settings
                Dictionary<string, string> dtFeecat = new Dictionary<string, string>();
                foreach (GridViewRow gdRow in gdSetting.Rows)
                {
                    DropDownList ddlBatch = (DropDownList)gdRow.FindControl("ddlBatch");
                    DropDownList ddlFeecat = (DropDownList)gdRow.FindControl("ddlFeecat");
                    if (ddlBatch.SelectedItem.Text.Trim() != "Select" && ddlFeecat.SelectedItem.Text.Trim() != "Select")
                    {
                        string batch = Convert.ToString(ddlBatch.SelectedItem.Text);
                        string feeCat = Convert.ToString(ddlFeecat.SelectedItem.Value);
                        if (!dtFeecat.ContainsKey(batch + "-" + feeCat))
                        {
                            dtFeecat.Add(batch + "-" + feeCat, feeCat);
                        }
                    }
                }
                if (dtFeecat.Count > 0)
                {
                    ViewState["FeeSet"] = dtFeecat;
                }
                #endregion

                FpSpread1.Visible = true;
                btnprintmaster.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                btnxl.Visible = true;
                DataView dvcnt = new DataView();
                DataView dvstudcnt = new DataView();
                DataView dvamt = new DataView();
                bool save = false;
                ArrayList ardeg = new ArrayList();
                DataTable dtFirstTbl = new DataTable();
                if (CurSemDeg.Count > 0)
                {
                    string stDegree = string.Empty;
                    string stBatch = string.Empty;
                    string stFeecat = string.Empty;
                    foreach (KeyValuePair<string, string> dtsem in CurSemDeg)
                    {
                        ds.Tables[0].DefaultView.RowFilter = "batch_year='" + dtsem.Key.Split('-')[0] + "' and degree_code='" + dtsem.Key.Split('-')[1] + "' and feecategory='" + dtsem.Value + "' and current_semester='" + dtsem.Key.Split('-')[2] + "'";
                        DataTable dtTemp = ds.Tables[0].DefaultView.ToTable();
                        if (dtTemp.Rows.Count > 0)
                        {
                            //DataRow drFirst = dtFirstTbl.NewRow();
                            dtFirstTbl.Merge(dtTemp);

                        }
                    }
                }
                else
                {
                    if (dtFeecat.Count > 0)
                    {
                        DataSet dsVal = new DataSet();
                        foreach (KeyValuePair<string, string> dtValue in dtFeecat)
                        {
                            try
                            {
                                ds.Tables[0].DefaultView.RowFilter = "batch_year='" + Convert.ToString(dtValue.Key.Split('-')[0]) + "' and feecategory='" + Convert.ToString(dtValue.Value) + "'";
                                dtFirstTbl.Merge(ds.Tables[0].DefaultView.ToTable());
                            }
                            catch { }
                        }
                        if (dtFirstTbl.Rows.Count > 0)
                        {
                            dtFirstTbl.DefaultView.Sort = "degree_code,batch_year desc,feecategory";
                            DataTable dtNew = dtFirstTbl.DefaultView.ToTable();
                            dtFirstTbl.Reset();
                            dtFirstTbl = dtNew;
                        }
                    }
                    else
                        dtFirstTbl = ds.Tables[0].DefaultView.ToTable();
                }
                Hashtable htfnlTot = new Hashtable();
                for (int i = 0; i < dtFirstTbl.Rows.Count; i++)
                {
                    ds1.Tables[0].DefaultView.RowFilter = "Batch_year='" + Convert.ToString(dtFirstTbl.Rows[i]["Batch_year"]) + "' and degree_code='" + Convert.ToString(dtFirstTbl.Rows[i]["degree_code"]) + "' and FeeCategory='" + Convert.ToString(dtFirstTbl.Rows[i]["FeeCategory"]) + "'";
                    dvcnt = ds1.Tables[0].DefaultView;
                    if (dvcnt.Count > 0)
                    {
                        string degcode = Convert.ToString(dtFirstTbl.Rows[i]["degree_code"]);
                        //total every degreewise
                        if (!ardegree.Contains(degcode))
                        {
                            #region every degreewise total
                            if (httotal.Count > 0)
                            {
                                FpSpread1.Sheets[0].RowCount++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 3);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.YellowGreen;
                                for (int d = 4; d < FpSpread1.Sheets[0].Columns.Count - 6; d++)
                                {
                                    Double dict = 0;
                                    if (httotal.ContainsKey(d))
                                        double.TryParse(Convert.ToString(httotal[d]), out dict);
                                    // dict = httotal[d];                                   
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Text = Convert.ToString(dict);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Font.Name = "Book Antiqua";
                                    if (d == 4)
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].HorizontalAlign = HorizontalAlign.Center;
                                    else
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].HorizontalAlign = HorizontalAlign.Right;
                                }
                                foreach (KeyValuePair<string, int> getcol in htcolCnt)
                                {
                                    string strKey = Convert.ToString(getcol.Key);
                                    int strValue = getcol.Value;
                                    double tempAmt = 0;
                                    double.TryParse(Convert.ToString(httotal[strKey]), out tempAmt);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, strValue].Text = Convert.ToString(tempAmt);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, strValue].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, strValue].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, strValue].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, strValue].HorizontalAlign = HorizontalAlign.Right;
                                }
                                httotal.Clear();
                            }
                            ardegree.Add(degcode);
                            #endregion
                        }
                        FpSpread1.Sheets[0].RowCount++;
                        if (!ardeg.Contains(degcode))
                        {
                            ardeg.Add(degcode);
                            sno++;
                        }
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dtFirstTbl.Rows[i]["Batch_year"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dtFirstTbl.Rows[i]["Course_Name"].ToString() + '-' + dtFirstTbl.Rows[i]["Dept_Name"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = dvcnt[0]["stucount"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        #region Semester

                        string feetext = "";
                        string semval = "";
                        // string semyear = ds.Tables[0].Rows[i]["Current_Semester"].ToString();
                        //if (semyear == "1" || semyear == "2")
                        //{
                        //    semyear = "I";
                        //    feetext = "1 Year";
                        //}
                        //else if (semyear == "3" || semyear == "4")
                        //{
                        //    semyear = "II";
                        //    feetext = "2 Year";
                        //}
                        //else if (semyear == "5" || semyear == "6")
                        //{
                        //    semyear = "III";
                        //    feetext = "3 Year";
                        //}
                        //else if (semyear == "7" || semyear == "8")
                        //{
                        //    semyear = "IV";
                        //    feetext = "4 Year";
                        //}
                        //else if (semyear == "9" || semyear == "10")
                        //{
                        //    semyear = "V";
                        //    feetext = "5 Year";
                        //}
                        //if (feetext != "")
                        //{
                        //    if (htfeecat.ContainsKey(feetext))
                        //    {
                        //        semval = Convert.ToString(htfeecat[feetext]);
                        //    }
                        //}
                        #endregion

                        string TextName = "";
                        string semcode = Convert.ToString(dtFirstTbl.Rows[i]["FeeCategory"]);
                        DataView Dview = new DataView();
                        if (dssem.Tables[0].Rows.Count > 0)
                        {
                            dssem.Tables[0].DefaultView.RowFilter = "TextCode=" + semcode + "";
                            Dview = dssem.Tables[0].DefaultView;
                            if (Dview.Count > 0)
                                TextName = Convert.ToString(Dview[0]["TextVal"]);
                        }
                        //if (sem != 0)
                        //    FpSpread1.Sheets[0].RowCount++;

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = TextName;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        Double dizasf = 0;
                        if (dicgrandtotal.ContainsKey(4))
                        {
                            dizasf = dicgrandtotal[4];
                            dicgrandtotal[4] = dizasf + Convert.ToDouble(dvcnt[0]["stucount"]); ;
                        }
                        else
                            dicgrandtotal.Add(4, Convert.ToDouble(dvcnt[0]["stucount"]));

                        if (httotal.ContainsKey(4))
                        {
                            Double amount = 0;
                            double.TryParse(Convert.ToString(httotal[4]), out amount);
                            httotal[4] = amount + Convert.ToDouble(dvcnt[0]["stucount"]); ;
                        }
                        else
                            httotal.Add(4, Convert.ToDouble(dvcnt[0]["stucount"]));
                        save = false;
                        for (int d = 5; d < FpSpread1.Sheets[0].Columns.Count - 10; d = d + 6)
                        {
                            string getfeecode = "";
                            if (ddlacctype.SelectedItem.Text != "Header")
                                getfeecode = FpSpread1.Sheets[0].ColumnHeader.Cells[2, d].Note;

                            else
                                getfeecode = FpSpread1.Sheets[0].ColumnHeader.Cells[1, d].Note;

                            Double allotamount = 0;
                            Double paidamount = 0;
                            Double concessinamount = 0;

                            string filterset = "header_id='" + getfeecode + "' and Batch_Year='" + dtFirstTbl.Rows[i]["Batch_Year"].ToString() + "' and degree_code='" + dtFirstTbl.Rows[i]["degree_code"].ToString() + "' and FeeCategory in(" + semcode + ")";
                            if (ddlacctype.SelectedItem.Text == "Ledger")
                            {
                                filterset = "Fee_code='" + getfeecode + "' and Batch_Year='" + dtFirstTbl.Rows[i]["Batch_Year"].ToString() + "' and degree_code='" + dtFirstTbl.Rows[i]["degree_code"].ToString() + "' and FeeCategory in(" + semcode + ")";
                            }
                            ds1.Tables[0].DefaultView.RowFilter = filterset;
                            ds1.Tables[1].DefaultView.RowFilter = filterset;


                            DataView dvallot = ds1.Tables[0].DefaultView;
                            dvstudcnt = ds1.Tables[1].DefaultView;
                            Dictionary<string, string> dictcount = new Dictionary<string, string>();
                            if (dvallot.Count > 0)
                            {
                                if (dvstudcnt.Count > 0)
                                {
                                    for (int k = 0; k < dvstudcnt.Count; k++)
                                    {
                                        string deamt = Convert.ToString(dvstudcnt[k]["demand"]);
                                        string filtersett = "header_id='" + getfeecode + "' and Batch_Year='" + dtFirstTbl.Rows[i]["Batch_Year"].ToString() + "' and degree_code='" + dtFirstTbl.Rows[i]["degree_code"].ToString() + "' and FeeCategory in(" + semcode + ") and demand='" + deamt + "'";
                                        ds1.Tables[2].DefaultView.RowFilter = filtersett;
                                        dvamt = ds1.Tables[2].DefaultView;
                                        if (dvamt.Count > 0)
                                        {
                                            if (!dictcount.ContainsKey(deamt))
                                                dictcount.Add(deamt, Convert.ToString(dvamt.Count));
                                        }
                                    }
                                    dictcount = dictcount.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                                }
                                save = true;
                                checksave = true;
                                //Double dizasf = 0;
                                //if (dicgrandtotal.ContainsKey(4))
                                //{
                                //    dizasf = dicgrandtotal[4];
                                //    dicgrandtotal[4] = dizasf + Convert.ToDouble(dvcnt[0]["stucount"]); ;
                                //}
                                //else
                                //    dicgrandtotal.Add(4, Convert.ToDouble(dvcnt[0]["stucount"]));



                                //allotamount = Convert.ToDouble(dvallot[0]["demand"].ToString());
                                //Double rate = allotamount / Convert.ToDouble(dvcnt[0]["stucount"]);
                                //rate = Math.Round(rate, 0, MidpointRounding.AwayFromZero);
                                //concessinamount = 0;
                                //double.TryParse(Convert.ToString(dvallot[0]["deduction"]), out concessinamount);
                                allotamount = Convert.ToDouble(dvallot[0]["demand"].ToString());
                                if (!htfnlTot.ContainsKey("Amount"))//final total
                                    htfnlTot.Add("Amount", Convert.ToString(allotamount));
                                else
                                {
                                    double amount = 0;
                                    double.TryParse(Convert.ToString(htfnlTot["Amount"]), out amount);
                                    amount += allotamount;
                                    htfnlTot.Remove("Amount");
                                    htfnlTot.Add("Amount", Convert.ToString(amount));
                                }
                                Double rate = 0;
                                foreach (KeyValuePair<string, string> semvalue in dictcount)
                                {
                                    double.TryParse(Convert.ToString(semvalue.Key), out rate);
                                    break;
                                }
                                //  double.TryParse(Convert.ToString(dvamt[0]["demand"]), out rate);
                                //Double rate = allotamount / count;
                                //rate = Math.Round(rate, 0, MidpointRounding.AwayFromZero);
                                concessinamount = 0;
                                double.TryParse(Convert.ToString(dvallot[0]["deduction"]), out concessinamount);


                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 1].Text = allotamount.ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Text = rate.ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 5].Text = Convert.ToString(concessinamount);

                                Double dic = 0;

                                if (dicgrandtotal.ContainsKey(d))
                                {
                                    dic = dicgrandtotal[d];
                                    dicgrandtotal[d] = dic + rate;
                                }
                                else
                                    dicgrandtotal.Add(d, rate);

                                if (httotal.ContainsKey(d))
                                {
                                    Double amount = 0;
                                    double.TryParse(Convert.ToString(httotal[d]), out amount);
                                    httotal[d] = amount + rate;
                                }
                                else
                                    httotal.Add(d, rate);

                                if (!htfnlTot.ContainsKey("Rate"))//final total
                                    htfnlTot.Add("Rate", Convert.ToString(rate));
                                else
                                {
                                    double amount = 0;
                                    double.TryParse(Convert.ToString(htfnlTot["Rate"]), out amount);
                                    amount += rate;
                                    htfnlTot.Remove("Rate");
                                    htfnlTot.Add("Rate", Convert.ToString(amount));
                                }

                                if (dicgrandtotal.ContainsKey(d + 1))
                                {
                                    dic = dicgrandtotal[d + 1];
                                    dicgrandtotal[d + 1] = dic + allotamount;
                                }
                                else
                                    dicgrandtotal.Add((d + 1), allotamount);

                                if (httotal.ContainsKey(d + 1))
                                {
                                    Double amount = 0;
                                    double.TryParse(Convert.ToString(httotal[d + 1]), out amount);
                                    httotal[d + 1] = amount + allotamount;
                                }
                                else
                                    httotal.Add(d + 1, allotamount);

                                if (dicgrandtotal.ContainsKey(d + 5))
                                {
                                    dic = dicgrandtotal[d + 5];
                                    dicgrandtotal[d + 5] = dic + concessinamount;
                                }
                                else
                                    dicgrandtotal.Add((d + 5), concessinamount);

                                if (httotal.ContainsKey(d + 5))
                                {
                                    Double amount = 0;
                                    double.TryParse(Convert.ToString(httotal[d + 5]), out amount);
                                    httotal[d + 5] = amount + concessinamount;
                                }
                                else
                                    httotal.Add(d + 5, concessinamount);
                                if (!htfnlTot.ContainsKey("Concession"))//final total
                                    htfnlTot.Add("Concession", Convert.ToString(concessinamount));
                                else
                                {
                                    double amount = 0;
                                    double.TryParse(Convert.ToString(htfnlTot["Concession"]), out amount);
                                    amount += concessinamount;
                                    htfnlTot.Remove("Concession");
                                    htfnlTot.Add("Concession", Convert.ToString(amount));
                                }


                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 1].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 5].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 5].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 5].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].HorizontalAlign = HorizontalAlign.Right;
                            }

                            ds2.Tables[0].DefaultView.RowFilter = filterset;
                            DataView dvpaid = ds2.Tables[0].DefaultView;
                            if (dvpaid.Count > 0)
                            {
                                DataTable dtpaid = new DataTable();
                                dtpaid = dvpaid.ToTable();
                                paidamount = Convert.ToInt32(dtpaid.Compute("sum(paid)", ""));

                                // paidamount = Convert.ToDouble(dvpaid[0]["paid"].ToString());
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 2].Text = paidamount.ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 2].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 2].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 2].HorizontalAlign = HorizontalAlign.Right;

                                Double dic = 0;

                                if (dicgrandtotal.ContainsKey(d + 2))
                                {
                                    dic = dicgrandtotal[d + 2];
                                    dicgrandtotal[d + 2] = dic + paidamount;
                                }
                                else
                                    dicgrandtotal.Add(d + 2, paidamount);

                                if (httotal.ContainsKey(d + 2))
                                {
                                    Double amount = 0;
                                    double.TryParse(Convert.ToString(httotal[d + 2]), out amount);
                                    httotal[d + 2] = amount + paidamount;
                                }
                                else
                                    httotal.Add(d + 2, paidamount);
                                if (!htfnlTot.ContainsKey("Collected"))//final total
                                    htfnlTot.Add("Collected", Convert.ToString(paidamount));
                                else
                                {
                                    double amount = 0;
                                    double.TryParse(Convert.ToString(htfnlTot["Collected"]), out amount);
                                    amount += paidamount;
                                    htfnlTot.Remove("Collected");
                                    htfnlTot.Add("Collected", Convert.ToString(amount));
                                }
                            }

                            Double balanceamount = allotamount - paidamount - concessinamount;
                            Double dict = 0;
                            if (dicgrandtotal.ContainsKey(d + 3))
                            {
                                dict = dicgrandtotal[d + 3];
                                dicgrandtotal[d + 3] = dict + balanceamount;
                            }
                            else
                                dicgrandtotal.Add(d + 3, balanceamount);

                            if (httotal.ContainsKey(d + 3))
                            {
                                Double amount = 0;
                                double.TryParse(Convert.ToString(httotal[d + 3]), out amount);
                                httotal[d + 3] = amount + balanceamount;
                            }
                            else
                                httotal.Add(d + 3, balanceamount);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 3].Text = balanceamount.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 3].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 3].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 3].HorizontalAlign = HorizontalAlign.Right;
                            if (!htfnlTot.ContainsKey("Arrear"))//final total
                                htfnlTot.Add("Arrear", Convert.ToString(balanceamount));
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(htfnlTot["Arrear"]), out amount);
                                amount += balanceamount;
                                htfnlTot.Remove("Arrear");
                                htfnlTot.Add("Arrear", Convert.ToString(amount));
                            }

                            balanceamount = balanceamount + paidamount;
                            if (dicgrandtotal.ContainsKey(d + 4))
                            {
                                dict = dicgrandtotal[d + 4];
                                dicgrandtotal[d + 4] = dict + balanceamount;
                            }
                            else
                                dicgrandtotal.Add(d + 4, balanceamount);

                            if (httotal.ContainsKey(d + 4))
                            {
                                Double amount = 0;
                                double.TryParse(Convert.ToString(httotal[d + 4]), out amount);
                                httotal[d + 4] = amount + balanceamount;
                            }
                            else
                                httotal.Add(d + 4, balanceamount);
                            if (!htfnlTot.ContainsKey("Total"))//final total
                                htfnlTot.Add("Total", Convert.ToString(balanceamount));
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(htfnlTot["Total"]), out amount);
                                amount += balanceamount;
                                htfnlTot.Remove("Total");
                                htfnlTot.Add("Total", Convert.ToString(amount));
                            }

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 4].Text = balanceamount.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 4].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 4].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 4].HorizontalAlign = HorizontalAlign.Right;
                        }

                        #region final total columns
                        foreach (KeyValuePair<string, int> getcol in htcolCnt)
                        {
                            string strKey = Convert.ToString(getcol.Key);
                            int strValue = getcol.Value;
                            double tempAmt = 0;
                            double.TryParse(Convert.ToString(htfnlTot[strKey]), out tempAmt);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, strValue].Text = Convert.ToString(tempAmt);
                            // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, strValue].Font.Bold = true;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, strValue].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, strValue].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, strValue].HorizontalAlign = HorizontalAlign.Right;
                            if (httotal.ContainsKey(strKey))
                            {
                                Double amount = 0;
                                double.TryParse(Convert.ToString(httotal[strKey]), out amount);
                                httotal[strKey] = amount + tempAmt;
                            }
                            else
                                httotal.Add(strKey, tempAmt);

                            if (dicgrandtotal.ContainsKey(strValue))
                            {
                                Double amount = 0;
                                double.TryParse(Convert.ToString(dicgrandtotal[strValue]), out amount);
                                dicgrandtotal[strValue] = amount + tempAmt;
                            }
                            else
                                dicgrandtotal.Add(strValue, tempAmt);

                        }
                        htfnlTot.Clear();
                        #endregion

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    }
                }

                #region last degree total
                if (httotal.Count > 0)
                {
                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 3);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.YellowGreen;
                    for (int d = 4; d < FpSpread1.Sheets[0].Columns.Count - 6; d++)
                    {
                        Double dict = 0;
                        if (httotal.ContainsKey(d))
                            double.TryParse(Convert.ToString(httotal[d]), out dict);
                        // dict = httotal[d];                                   
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Text = Convert.ToString(dict);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Font.Bold = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Font.Name = "Book Antiqua";
                        if (d == 4)
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].HorizontalAlign = HorizontalAlign.Center;
                        else
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].HorizontalAlign = HorizontalAlign.Right;
                    }
                    foreach (KeyValuePair<string, int> getcol in htcolCnt)
                    {
                        string strKey = Convert.ToString(getcol.Key);
                        int strValue = getcol.Value;
                        double tempAmt = 0;
                        double.TryParse(Convert.ToString(httotal[strKey]), out tempAmt);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, strValue].Text = Convert.ToString(tempAmt);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, strValue].Font.Bold = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, strValue].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, strValue].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, strValue].HorizontalAlign = HorizontalAlign.Right;
                    }
                    httotal.Clear();
                }
                #endregion
                FpSpread1.Sheets[0].RowCount++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 3);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                for (int d = 4; d < FpSpread1.Sheets[0].Columns.Count; d++)
                {
                    Double dict = 0;
                    if (dicgrandtotal.ContainsKey(d))
                    {
                        dict = dicgrandtotal[d];
                    }
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Text = dict.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Font.Bold = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].HorizontalAlign = HorizontalAlign.Right;
                }
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            }
            if (ddlacctype.SelectedItem.Text == "Header")
            {
                if (FpSpread1.Sheets[0].ColumnHeader.RowCount > 2)
                {
                    FpSpread1.Sheets[0].ColumnHeader.Rows[2].Visible = false;
                }
            }
            else
            {
                if (FpSpread1.Sheets[0].ColumnHeader.RowCount > 2)
                {
                    FpSpread1.Sheets[0].ColumnHeader.Rows[2].Visible = true;
                }
            }
            //FpSpread1.Width = 1000;
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;

            FpSpread1.Visible = true;
            if (checksave == true)
            {
                FpSpread1.Visible = true;
                btnprintmaster.Visible = true;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
            }
            else
            {
                // div1.Visible = false;
                FpSpread1.Visible = false;
                errmsg.Visible = true;
                errmsg.Text = "No Records Found";
                FpSpread1.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                return;
            }

            //}
            //else
            //{
            //    // div1.Visible = false;
            //    FpSpread1.Visible = false;
            //    errmsg.Visible = true;
            //    errmsg.Text = "Please Select Seat Type";
            //    FpSpread1.Visible = false;
            //    btnprintmaster.Visible = false;
            //    lblrptname.Visible = false;
            //    txtexcelname.Visible = false;
            //    btnxl.Visible = false;
            //    return;
            //}
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    protected void withdue()
    {
        try
        {
            clear();
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = Color.Black;
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(darkstyle);

            #region get value
            string seattext = "";
            string batchquery = "";
            string degreequery = "";
            string setbatchanddegree = "";
            string getdegreedetails = "";
            string demquery = "";
            string batch_all = string.Empty;
            string course_all = string.Empty;
            string header_all = string.Empty;
            string category = string.Empty;
            string feecode = "";
            string tot_category = string.Empty;
            string paidquery = "";
            string demandfee = "";
            int sno = 0;
            string deg_acr = string.Empty;
            string deg_dept = string.Empty;

            string getfeeval = "";

            FarPoint.Web.Spread.TextCellType txtcel = new FarPoint.Web.Spread.TextCellType();

            if (ddlseattype.Items.Count > 0)
                seattext = Convert.ToString(ddlseattype.SelectedItem.Text);

            if (ddlacctype.Text == "---Select---")
            {
                finalflag = true;
                errmsg.Visible = true;
                errmsg.Text = "Please Select Account Type";

                return;
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
            if (batch_all.Trim() != "")
            {
                batchquery = " and r.Batch_year in(" + batch_all + ")";
                setbatchanddegree = " r.Batch_year in(" + batch_all + ")";
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
                    setbatchanddegree = setbatchanddegree + " and r.degree_code in(" + course_all + ")";
                }
                else
                {
                    setbatchanddegree = " and r.degree_code in(" + course_all + ")";
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

            string fnlYr = "";
            for (int t = 0; t < chklsfyear.Items.Count; t++)
            {
                if (chklsfyear.Items[t].Selected == true)
                {
                    if (fnlYr == "")
                    {
                        fnlYr = "'" + chklsfyear.Items[t].Value.ToString() + "'";
                    }
                    else
                    {
                        fnlYr = fnlYr + ",'" + chklsfyear.Items[t].Value.ToString() + "'";
                    }
                }
            }
            if (fnlYr.Trim() != "")
            {
                fnlYr = " and finyearfk in (" + fnlYr + ")";
            }
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
            ArrayList arFeecat = new ArrayList();
            Dictionary<string, string> CurSemDeg = new Dictionary<string, string>();
            string curSem = getCurrentSemester(batch_all, course_all, ref arFeecat, commondist, ref CurSemDeg);//current semester only
            feeCatVal(curSem, arFeecat);
            Dictionary<string, string> htfeecat = new Dictionary<string, string>();
            htfeecat = (Dictionary<string, string>)ViewState["feecat"];
            string feecatval = "";
            string feecatg = "";
            if (htfeecat.Count > 0)
            {
                foreach (KeyValuePair<string, string> sem in htfeecat)
                {
                    if (feecatval == "")
                        feecatval = sem.Value.ToString();
                    else
                        feecatval += "'" + "," + "'" + sem.Value.ToString();
                }
                if (feecatval != "")
                    feecatg = " and a.FeeCategory in('" + feecatval + "')";
            }

            string fromdate = "";
            string todate = "";
            string date = "";
            if (chk_datewise.Checked == true)
            {
                fromdate = txt_fromdate.Text;
                todate = txt_todate.Text;
                if (fromdate != "" && todate != "")
                {
                    string[] frdate = fromdate.Split('/');
                    if (frdate.Length == 3)
                        fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();

                    string[] tdate = todate.Split('/');
                    if (tdate.Length == 3)
                        todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();

                    date = " AND TransDate between '" + fromdate + "' and '" + todate + "'";
                }
            }

            #endregion

            #region design

            //FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.BackColor = Color.FromArgb(214, 235, 255);
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 3;
            FpSpread1.Sheets[0].ColumnHeader.Columns.Count = 5;

            FpSpread1.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Rows[0].Font.Size = FontUnit.Large;
            FpSpread1.Sheets[0].ColumnHeader.Rows[0].Font.Name = "Book Antiqua";

            FpSpread1.Sheets[0].ColumnHeader.Rows[1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Rows[1].Font.Size = FontUnit.Large;
            FpSpread1.Sheets[0].ColumnHeader.Rows[1].Font.Name = "Book Antiqua";

            FpSpread1.Sheets[0].ColumnHeader.Rows[2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Rows[2].Font.Size = FontUnit.Large;
            FpSpread1.Sheets[0].ColumnHeader.Rows[2].Font.Name = "Book Antiqua";

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Batch Year";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = lbldept.Text;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = lblsem.Text;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Strength";


            FpSpread1.Sheets[0].Columns[1].Width = 200;

            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 3, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 3, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 3, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 3, 1);

            FpSpread1.Sheets[0].Columns[2].CellType = txtcel;

            DataView dvhead = new DataView();

            FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;

            #endregion
            string StudentFees = "";
            string strledgre = "";
            string strledger = "";
            bool boolColFnl = false;
            Dictionary<string, int> htcolCnt = new Dictionary<string, int>();
            if (ddlacctype.SelectedItem.Text == "Ledger")
            {
                #region ledger
                strledgre = ",a.LedgerFK as Fee_Code";
                strledger = ",a.LedgerFK";
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

                                FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 7;
                                if (headflag == false)
                                {
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 6].Text = treeview_spreadfields.Nodes[remv].Text.ToString();
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 6].HorizontalAlign = HorizontalAlign.Center;
                                    headflag = true;
                                    hstartc = FpSpread1.Sheets[0].ColumnCount - 6;
                                }
                                ledcount++;

                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6, 1, 6);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 6].Text = treeview_spreadfields.Nodes[remv].ChildNodes[child].Text.ToString();
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 6].HorizontalAlign = HorizontalAlign.Center;
                                feecode = treeview_spreadfields.Nodes[remv].ChildNodes[child].Value.ToString();
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 7].Text = "Rate";
                                //FpSpread1.Columns[FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 7].Visible = false;

                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 7].Note = feecode;


                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6].Text = "Amount";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6].Note = feecode;


                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 5].Text = "Collected";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 5].Note = feecode;


                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 4].Text = "Balance";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 4].Note = feecode;


                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3].Text = "Total";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3].Note = feecode;


                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Text = "Concession";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Note = feecode;

                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Arrear";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Note = feecode;
                                boolColFnl = true;
                                ccount += 7;
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - (ccount - 1), 1, ccount - 2);
                                if (demandfee == "")
                                {
                                    demandfee = feecode;
                                    StudentFees = feecode;
                                }
                                else
                                {
                                    StudentFees = StudentFees + ',' + feecode;
                                    demandfee = demandfee + ',' + feecode;
                                }

                            }
                        }
                    }
                    else
                    {
                        FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 1;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = treeview_spreadfields.Nodes[remv].Text.ToString();
                    }
                }
                if (demandfee.Trim() != "")
                {
                    demandfee = " and a.LedgerFK in(" + demandfee + ")";
                    StudentFees = " and f.LedgerFK in(" + StudentFees + ")";
                }
                #endregion
            }
            else if (ddlacctype.SelectedItem.Text == "Group Header")
            {
                #region group header
                string head = "";
                string acchead = "select distinct h.HeaderName,g.HeaderFK,g.ChlGroupHeader from FS_ChlGroupHeaderSettings g,FM_HeaderMaster h where h.HeaderPK=g.HeaderFK";
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

                            FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 7;
                            if (headflag == false)
                            {
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 6].Text = chklstaccheader.Items[j].Text;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 6].HorizontalAlign = HorizontalAlign.Center;
                                headflag = true;
                                hstartc = FpSpread1.Sheets[0].ColumnCount - 6;
                            }
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6, 1, 6);
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 6].Text = dvhead[a]["HeaderName"].ToString();
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 6].HorizontalAlign = HorizontalAlign.Center;
                            if (dvhead.Count > 0)
                            {
                                feecode = dvhead[a]["HeaderFK"].ToString();
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 7].Text = "Rate";
                               // FpSpread1.Columns[FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 7].Visible = false;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 7].Note = feecode;


                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6].Text = "Amount";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6].Note = feecode;


                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 5].Text = "Collected";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 5].Note = feecode;


                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 4].Text = "Balance";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 4].Note = feecode;


                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3].Text = "Total";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3].Note = feecode;


                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Text = "Concession";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Note = feecode;

                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Arrear";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Note = feecode;
                                boolColFnl = true;
                                if (demandfee == "")
                                {
                                    demandfee = feecode;
                                    // gpvalue = feecode;
                                }
                                else
                                {
                                    demandfee = demandfee + ',' + feecode;
                                    // gpvalue = gpvalue + ',' + feecode;
                                }
                            }
                            ccount += 7;
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - (ccount - 1), 1, (ccount - 1));
                        }
                    }

                }
                if (demandfee.Trim() != "")
                {
                    demandfee = " and a.HeaderFK in(" + demandfee + ")";
                }
                #endregion
            }
            else//Header..............
            {
                #region header

                for (int a = 0; a < chklstaccheader.Items.Count; a++)
                {
                    if (chklstaccheader.Items[a].Selected == true)
                    {
                        FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 7;
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6, 1, 6);
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 6].Text = chklstaccheader.Items[a].Text;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 7].Text = "Rate";
                      //  FpSpread1.Columns[FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 7].Visible = false;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 7].Note = chklstaccheader.Items[a].Value;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6].Text = "Amount";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6].Note = chklstaccheader.Items[a].Value;
                        //   htcol.Add(chklstaccheader.Items[a].Value, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6);
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 5].Text = "Collected";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 5].Note = chklstaccheader.Items[a].Value;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 4].Text = "Balance";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 4].Note = chklstaccheader.Items[a].Value;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3].Text = "Total";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3].Note = chklstaccheader.Items[a].Value;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Text = "Concession";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Note = chklstaccheader.Items[a].Value;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Arrear";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Note = chklstaccheader.Items[a].Value;
                        boolColFnl = true;
                        if (demandfee == "")
                        {
                            demandfee = chklstaccheader.Items[a].Value;
                            StudentFees = chklstaccheader.Items[a].Value;
                        }
                        else
                        {
                            demandfee = demandfee + ',' + chklstaccheader.Items[a].Value;
                            StudentFees = StudentFees + ',' + chklstaccheader.Items[a].Value;
                        }
                    }
                }
                if (demandfee.Trim() != "")
                {
                    StudentFees = " and f.HeaderFK in(" + demandfee + ")";
                    demandfee = " and a.HeaderFK in(" + demandfee + ")";
                }
                #endregion
            }
            int fnltot = 0;
            if (boolColFnl)
            {
                FpSpread1.Sheets[0].ColumnCount++;
                int colCnt = FpSpread1.Sheets[0].ColumnCount - 1;
                if (fnltot == 0)
                    fnltot = colCnt;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, colCnt].Text = "Total";
                if (ddlacctype.SelectedItem.Text == "Ledger" || ddlacctype.SelectedItem.Text == "Group Header")
                {
                    FpSpread1.Sheets[0].ColumnHeader.Cells[2, colCnt].Text = "Amount";
                    htcolCnt.Add("Amount", colCnt);
                    FpSpread1.Sheets[0].ColumnCount++;
                    colCnt = FpSpread1.Sheets[0].ColumnCount - 1;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[2, colCnt].Text = "Collected";
                    htcolCnt.Add("Collected", colCnt);
                    FpSpread1.Sheets[0].ColumnCount++;
                    colCnt = FpSpread1.Sheets[0].ColumnCount - 1;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[2, colCnt].Text = "Balance";
                    htcolCnt.Add("Balance", colCnt);
                    FpSpread1.Sheets[0].ColumnCount++;
                    colCnt = FpSpread1.Sheets[0].ColumnCount - 1;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[2, colCnt].Text = "Total";
                    htcolCnt.Add("Total", colCnt);
                    FpSpread1.Sheets[0].ColumnCount++;
                    colCnt = FpSpread1.Sheets[0].ColumnCount - 1;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[2, colCnt].Text = "Concession";
                    htcolCnt.Add("Concession", colCnt);
                    FpSpread1.Sheets[0].ColumnCount++;
                    colCnt = FpSpread1.Sheets[0].ColumnCount - 1;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[2, colCnt].Text = "Arrear";
                    htcolCnt.Add("Arrear", colCnt);

                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, fnltot, 2, 6);
                }
                else
                {
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, colCnt].Text = "Amount";
                    htcolCnt.Add("Amount", colCnt);
                    FpSpread1.Sheets[0].ColumnCount++;
                    colCnt = FpSpread1.Sheets[0].ColumnCount - 1;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, colCnt].Text = "Collected";
                    htcolCnt.Add("Collected", colCnt);
                    FpSpread1.Sheets[0].ColumnCount++;
                    colCnt = FpSpread1.Sheets[0].ColumnCount - 1;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, colCnt].Text = "Balance";
                    htcolCnt.Add("Balance", colCnt);
                    FpSpread1.Sheets[0].ColumnCount++;
                    colCnt = FpSpread1.Sheets[0].ColumnCount - 1;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, colCnt].Text = "Total";
                    htcolCnt.Add("Total", colCnt);
                    FpSpread1.Sheets[0].ColumnCount++;
                    colCnt = FpSpread1.Sheets[0].ColumnCount - 1;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, colCnt].Text = "Concession";
                    htcolCnt.Add("Concession", colCnt);
                    FpSpread1.Sheets[0].ColumnCount++;
                    colCnt = FpSpread1.Sheets[0].ColumnCount - 1;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, colCnt].Text = "Arrear";
                    htcolCnt.Add("Arrear", colCnt);

                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, fnltot, 1, 6);
                }
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, fnltot].HorizontalAlign = HorizontalAlign.Center;
            }
            //string seat = da.GetFunction("select TextCode from TextValTable where TextCriteria='seat' and textval like '%" + seattext + "%' and college_code='" + collegecode + "'");
            //if (seat != "0")
            //{
            #region new

            //demand
            demquery = "select count(distinct r.roll_no) as stucount,sum(feeamount) as demand,sum(DeductAmout) as deduction,r.degree_code,r.Batch_Year,a.HeaderFK as header_id" + strledgre + ",a.FeeCategory    from FT_FeeAllot a,registration r where a.App_No =r.App_No and (a.TotalAmount>0 or a.deductamout>0) " + fnlYr + "";
            if (commondist != "")
                demquery = demquery + commondist;
            if (chkinclude.Checked == true)
            {
                //demquery = demquery + " and r.DelFlag=0";
            }
            if (cbCurSem.Checked && !string.IsNullOrEmpty(curSem))
                demquery += " and r.current_semester in('" + curSem + "')";
            demquery = demquery + " " + degreequery + " " + batchquery + " " + demandfee + " " + feecatg + " group by r.degree_code,r.Batch_Year,a.HeaderFK" + strledger + ",a.FeeCategory  order by r.degree_code  ";


            //paid query
            paidquery = "select count(distinct r.roll_no) as stucount,sum(debit) as paid,r.degree_code,r.Batch_Year,a.HeaderFK as header_id" + strledgre + ",a.FeeCategory   from FT_FinDailyTransaction a,registration r where  a.App_No=r.App_No  and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' " + fnlYr + "";
            if (commondist != "")
                paidquery = paidquery + commondist;
            if (chkinclude.Checked == true)
            {
                // paidquery = paidquery + " and r.DelFlag=0";
            }
            if (cbCurSem.Checked && !string.IsNullOrEmpty(curSem))
                paidquery += " and r.current_semester in('" + curSem + "')";
            paidquery = paidquery + " " + degreequery + " " + batchquery + " " + demandfee + " " + feecatg + " " + date + " group by r.degree_code,r.Batch_Year,a.HeaderFK" + strledger + ",a.FeeCategory order by r.degree_code   ";

            //paid query
            paidquery += " select count(distinct r.roll_no) as stucount,sum(debit) as paid,r.degree_code,r.Batch_Year,a.HeaderFK as header_id" + strledgre + ",a.FeeCategory   from FT_FinDailyTransaction a,registration r where  a.App_No=r.App_No  and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' " + fnlYr + "";
            if (commondist != "")
                paidquery = paidquery + commondist;
            if (chkinclude.Checked == true)
            {
                // paidquery = paidquery + " and r.DelFlag=0";
            }
            if (cbCurSem.Checked && !string.IsNullOrEmpty(curSem))
                paidquery += " and r.current_semester in('" + curSem + "')";
            paidquery = paidquery + " " + degreequery + " " + batchquery + " " + demandfee + " " + feecatg + " " + date + " group by r.degree_code,r.Batch_Year,a.HeaderFK" + strledger + ",a.FeeCategory order by r.degree_code   ";


            //stud count
            string str = "select count(distinct r.roll_no) as stcount,r.Batch_Year,c.Course_Name,de.Dept_Name,r.degree_code,f.FeeCategory,r.current_semester  from Registration r,Degree d,Department de,Course c ,FT_FeeAllot f where f.App_No =r.App_No and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and c.Course_Id=d.Course_Id   " + fnlYr + "";//,r.Current_Semester
            if (commondist != "")
                str = str + commondist;
            if (chkinclude.Checked == true)
            {
                // str = str + " and r.DelFlag=0";
            }
            if (cbCurSem.Checked && !string.IsNullOrEmpty(curSem))
                str += " and r.current_semester in('" + curSem + "')";
            str = str + " " + degreequery + " " + batchquery + " and f.FeeCategory in('" + feecatval + "') " + StudentFees + " group by r.Batch_Year,c.Course_Name,de.Dept_Name,r.degree_code,f.FeeCategory,r.current_semester  order by r.degree_code,r.Batch_Year desc,f.FeeCategory ";//,r.Current_Semester

            //individual count          
            demquery += " select distinct sum(feeamount) as demand,sum(DeductAmout) as deduction,r.degree_code,r.Batch_Year,a.HeaderFK as header_id" + strledgre + ",a.FeeCategory    from FT_FeeAllot a,registration r where a.App_No =r.App_No and (a.TotalAmount>0 or a.deductamout>0) " + fnlYr + "";
            if (commondist != "")
                demquery = demquery + commondist;
            if (chkinclude.Checked == true)
            {
                // demquery = demquery + " and r.DelFlag=0";
            }
            if (cbCurSem.Checked && !string.IsNullOrEmpty(curSem))
                demquery += " and r.current_semester in('" + curSem + "')";
            demquery = demquery + "  " + degreequery + " " + batchquery + " " + demandfee + " " + feecatg + " group by r.degree_code,r.Batch_Year,a.HeaderFK" + strledger + ",a.FeeCategory,r.roll_no  order by r.degree_code  ";

            //amount count
            demquery += " select distinct r.roll_no as stucount,sum(feeamount) as demand,sum(DeductAmout) as deduction,r.degree_code,r.Batch_Year,a.HeaderFK as header_id" + strledgre + ",a.FeeCategory    from FT_FeeAllot a,registration r where a.App_No =r.App_No and (a.TotalAmount>0 or a.deductamout>0) " + fnlYr + "";
            if (commondist != "")
                demquery = demquery + commondist;
            if (chkinclude.Checked == true)
            {
                //  demquery = demquery + " and r.DelFlag=0";
            }
            if (cbCurSem.Checked && !string.IsNullOrEmpty(curSem))
                demquery += " and r.current_semester in('" + curSem + "')";
            demquery = demquery + "  " + degreequery + " " + batchquery + " " + demandfee + " " + feecatg + " group by r.degree_code,r.Batch_Year,a.HeaderFK" + strledger + ",a.FeeCategory,r.roll_no  order by r.degree_code ";

            //seme
            string semestselqry = " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code ='" + collegecode + "'";


            #endregion
            ds.Reset();
            ds.Dispose();
            ds = da.select_method_wo_parameter(str, "Text");

            ds1.Reset();
            ds1.Dispose();
            ds1 = da.select_method_wo_parameter(demquery, "Text");

            ds2.Reset();
            ds2.Dispose();
            ds2 = da.select_method_wo_parameter(paidquery, "Text");

            DataSet dssem = new DataSet();
            dssem.Reset();
            dssem.Dispose();
            dssem = da.select_method_wo_parameter(semestselqry, "Text");

            Dictionary<int, Double> dicgrandtotal = new Dictionary<int, double>();
            bool checksave = false;
            Hashtable httotal = new Hashtable();
            ArrayList ardegree = new ArrayList();
            if (ds.Tables[0].Rows.Count > 0)
            {
                #region feecategory settings
                Dictionary<string, string> dtFeecat = new Dictionary<string, string>();
                foreach (GridViewRow gdRow in gdSetting.Rows)
                {
                    DropDownList ddlBatch = (DropDownList)gdRow.FindControl("ddlBatch");
                    DropDownList ddlFeecat = (DropDownList)gdRow.FindControl("ddlFeecat");
                    if (ddlBatch.SelectedItem.Text.Trim() != "Select" && ddlFeecat.SelectedItem.Text.Trim() != "Select")
                    {
                        string batch = Convert.ToString(ddlBatch.SelectedItem.Text);
                        string feeCat = Convert.ToString(ddlFeecat.SelectedItem.Value);
                        if (!dtFeecat.ContainsKey(batch + "-" + feeCat))
                        {
                            dtFeecat.Add(batch + "-" + feeCat, feeCat);
                        }
                    }
                }
                if (dtFeecat.Count > 0)
                {
                    ViewState["FeeSet"] = dtFeecat;
                }
                #endregion
                FpSpread1.Visible = true;
                btnprintmaster.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                btnxl.Visible = true;
                DataView dvcnt = new DataView();
                DataView dvstudcnt = new DataView();
                DataView dvamt = new DataView();
                bool save = false;
                ArrayList ardeg = new ArrayList();
                DataTable dtFirstTbl = new DataTable();
                if (CurSemDeg.Count > 0)
                {
                    string stDegree = string.Empty;
                    string stBatch = string.Empty;
                    string stFeecat = string.Empty;
                    foreach (KeyValuePair<string, string> dtsem in CurSemDeg)
                    {
                        ds.Tables[0].DefaultView.RowFilter = "batch_year='" + dtsem.Key.Split('-')[0] + "' and degree_code='" + dtsem.Key.Split('-')[1] + "' and feecategory='" + dtsem.Value + "' and current_semester='" + dtsem.Key.Split('-')[2] + "'";
                        DataTable dtTemp = ds.Tables[0].DefaultView.ToTable();
                        if (dtTemp.Rows.Count > 0)
                        {
                            //DataRow drFirst = dtFirstTbl.NewRow();
                            dtFirstTbl.Merge(dtTemp);

                        }
                    }
                }
                else
                {
                    if (dtFeecat.Count > 0)
                    {
                        DataSet dsVal = new DataSet();
                        foreach (KeyValuePair<string, string> dtValue in dtFeecat)
                        {
                            try
                            {
                                ds.Tables[0].DefaultView.RowFilter = "batch_year='" + Convert.ToString(dtValue.Key.Split('-')[0]) + "' and feecategory='" + Convert.ToString(dtValue.Value) + "'";
                                dtFirstTbl.Merge(ds.Tables[0].DefaultView.ToTable());
                            }
                            catch { }
                        }
                        if (dtFirstTbl.Rows.Count > 0)
                        {
                            dtFirstTbl.DefaultView.Sort = "degree_code,batch_year desc,feecategory";
                            DataTable dtNew = dtFirstTbl.DefaultView.ToTable();
                            dtFirstTbl.Reset();
                            dtFirstTbl = dtNew;
                        }
                    }
                    else
                        dtFirstTbl = ds.Tables[0].DefaultView.ToTable();
                }
                Hashtable htfnlTot = new Hashtable();
                for (int i = 0; i < dtFirstTbl.Rows.Count; i++)
                {
                    ds1.Tables[0].DefaultView.RowFilter = "Batch_year='" + Convert.ToString(dtFirstTbl.Rows[i]["Batch_year"]) + "' and degree_code='" + Convert.ToString(dtFirstTbl.Rows[i]["degree_code"]) + "' and FeeCategory='" + Convert.ToString(dtFirstTbl.Rows[i]["FeeCategory"]) + "'";
                    dvcnt = ds1.Tables[0].DefaultView;
                    if (dvcnt.Count > 0)
                    {
                        string degcode = Convert.ToString(dtFirstTbl.Rows[i]["degree_code"]);
                        //total every degreewise
                        if (!ardegree.Contains(degcode))
                        {
                            #region every degreewise total
                            if (httotal.Count > 0)
                            {
                                FpSpread1.Sheets[0].RowCount++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 3);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.YellowGreen;
                                for (int d = 4; d < FpSpread1.Sheets[0].Columns.Count - 6; d++)
                                {
                                    Double dict = 0;
                                    if (httotal.ContainsKey(d))
                                        double.TryParse(Convert.ToString(httotal[d]), out dict);
                                    // dict = httotal[d];                                   
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Text = Convert.ToString(dict);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Font.Name = "Book Antiqua";
                                    if (d == 4)
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].HorizontalAlign = HorizontalAlign.Center;
                                    else
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].HorizontalAlign = HorizontalAlign.Right;
                                }
                                foreach (KeyValuePair<string, int> getcol in htcolCnt)
                                {
                                    string strKey = Convert.ToString(getcol.Key);
                                    int strValue = getcol.Value;
                                    double tempAmt = 0;
                                    double.TryParse(Convert.ToString(httotal[strKey]), out tempAmt);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, strValue].Text = Convert.ToString(tempAmt);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, strValue].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, strValue].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, strValue].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, strValue].HorizontalAlign = HorizontalAlign.Right;
                                }
                                httotal.Clear();
                            }
                            ardegree.Add(degcode);
                            #endregion
                        }
                        FpSpread1.Sheets[0].RowCount++;
                        if (!ardeg.Contains(Convert.ToString(dtFirstTbl.Rows[i]["degree_code"])))
                        {
                            ardeg.Add(Convert.ToString(dtFirstTbl.Rows[i]["degree_code"]));
                            sno++;
                        }
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dtFirstTbl.Rows[i]["Batch_year"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dtFirstTbl.Rows[i]["Course_Name"].ToString() + '-' + dtFirstTbl.Rows[i]["Dept_Name"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = dvcnt[0]["stucount"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        #region Semester

                        string feetext = "";
                        string semval = "";
                        // string semyear = ds.Tables[0].Rows[i]["Current_Semester"].ToString();
                        //if (semyear == "1" || semyear == "2")
                        //{
                        //    semyear = "I";
                        //    feetext = "1 Year";
                        //}
                        //else if (semyear == "3" || semyear == "4")
                        //{
                        //    semyear = "II";
                        //    feetext = "2 Year";
                        //}
                        //else if (semyear == "5" || semyear == "6")
                        //{
                        //    semyear = "III";
                        //    feetext = "3 Year";
                        //}
                        //else if (semyear == "7" || semyear == "8")
                        //{
                        //    semyear = "IV";
                        //    feetext = "4 Year";
                        //}
                        //else if (semyear == "9" || semyear == "10")
                        //{
                        //    semyear = "V";
                        //    feetext = "5 Year";
                        //}
                        //if (feetext != "")
                        //{
                        //    if (htfeecat.ContainsKey(feetext))
                        //    {
                        //        semval = Convert.ToString(htfeecat[feetext]);
                        //    }
                        //}
                        #endregion

                        string TextName = "";
                        string semcode = Convert.ToString(dtFirstTbl.Rows[i]["FeeCategory"]);
                        DataView Dview = new DataView();
                        if (dssem.Tables[0].Rows.Count > 0)
                        {
                            dssem.Tables[0].DefaultView.RowFilter = "TextCode=" + semcode + "";
                            Dview = dssem.Tables[0].DefaultView;
                            if (Dview.Count > 0)
                                TextName = Convert.ToString(Dview[0]["TextVal"]);
                        }
                        //if (sem != 0)
                        //    FpSpread1.Sheets[0].RowCount++;

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = TextName;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        Double dizasf = 0;
                        if (dicgrandtotal.ContainsKey(4))
                        {
                            dizasf = dicgrandtotal[4];
                            dicgrandtotal[4] = dizasf + Convert.ToDouble(dvcnt[0]["stucount"]); ;
                        }
                        else
                            dicgrandtotal.Add(4, Convert.ToDouble(dvcnt[0]["stucount"]));
                        if (httotal.ContainsKey(4))
                        {
                            Double amount = 0;
                            double.TryParse(Convert.ToString(httotal[4]), out amount);
                            httotal[4] = amount + Convert.ToDouble(dvcnt[0]["stucount"]);
                        }
                        else
                            httotal.Add(4, Convert.ToDouble(dvcnt[0]["stucount"]));
                        save = false;
                        for (int d = 5; d < FpSpread1.Sheets[0].Columns.Count - 10; d = d + 7)
                        {
                            string getfeecode = "";
                            if (ddlacctype.SelectedItem.Text != "Header")
                                getfeecode = FpSpread1.Sheets[0].ColumnHeader.Cells[2, d].Note;

                            else
                                getfeecode = FpSpread1.Sheets[0].ColumnHeader.Cells[1, d].Note;

                            Double allotamount = 0;
                            Double paidamount = 0;
                            Double concessinamount = 0;

                            string filterset = "header_id='" + getfeecode + "' and Batch_Year='" + dtFirstTbl.Rows[i]["Batch_Year"].ToString() + "' and degree_code='" + dtFirstTbl.Rows[i]["degree_code"].ToString() + "' and FeeCategory in(" + semcode + ")";
                            if (ddlacctype.SelectedItem.Text == "Ledger")
                            {
                                filterset = "Fee_code='" + getfeecode + "' and Batch_Year='" + dtFirstTbl.Rows[i]["Batch_Year"].ToString() + "' and degree_code='" + dtFirstTbl.Rows[i]["degree_code"].ToString() + "' and FeeCategory in(" + semcode + ")";
                            }
                            ds1.Tables[0].DefaultView.RowFilter = filterset;
                            ds1.Tables[1].DefaultView.RowFilter = filterset;


                            DataView dvallot = ds1.Tables[0].DefaultView;
                            dvstudcnt = ds1.Tables[1].DefaultView;
                            Dictionary<string, string> dictcount = new Dictionary<string, string>();
                            if (dvallot.Count > 0)
                            {
                                #region demand
                                if (dvstudcnt.Count > 0)
                                {
                                    for (int k = 0; k < dvstudcnt.Count; k++)
                                    {
                                        string deamt = Convert.ToString(dvstudcnt[k]["demand"]);
                                        string filtersett = "header_id='" + getfeecode + "' and Batch_Year='" + dtFirstTbl.Rows[i]["Batch_Year"].ToString() + "' and degree_code='" + dtFirstTbl.Rows[i]["degree_code"].ToString() + "' and FeeCategory in(" + semcode + ") and demand='" + deamt + "'";
                                        ds1.Tables[2].DefaultView.RowFilter = filtersett;
                                        dvamt = ds1.Tables[2].DefaultView;
                                        if (dvamt.Count > 0)
                                        {
                                            if (!dictcount.ContainsKey(deamt))
                                                dictcount.Add(deamt, Convert.ToString(dvamt.Count));
                                        }
                                    }
                                    dictcount = dictcount.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                                }
                                save = true;
                                checksave = true;
                                //Double dizasf = 0;
                                //if (dicgrandtotal.ContainsKey(4))
                                //{
                                //    dizasf = dicgrandtotal[4];
                                //    dicgrandtotal[4] = dizasf + Convert.ToDouble(dvcnt[0]["stucount"]); ;
                                //}
                                //else
                                //    dicgrandtotal.Add(4, Convert.ToDouble(dvcnt[0]["stucount"]));



                                //allotamount = Convert.ToDouble(dvallot[0]["demand"].ToString());
                                //Double rate = allotamount / Convert.ToDouble(dvcnt[0]["stucount"]);
                                //rate = Math.Round(rate, 0, MidpointRounding.AwayFromZero);
                                //concessinamount = 0;
                                //double.TryParse(Convert.ToString(dvallot[0]["deduction"]), out concessinamount);
                                allotamount = Convert.ToDouble(dvallot[0]["demand"].ToString());
                                if (!htfnlTot.ContainsKey("Amount"))//final total
                                    htfnlTot.Add("Amount", Convert.ToString(allotamount));
                                else
                                {
                                    double amount = 0;
                                    double.TryParse(Convert.ToString(htfnlTot["Amount"]), out amount);
                                    amount += allotamount;
                                    htfnlTot.Remove("Amount");
                                    htfnlTot.Add("Amount", Convert.ToString(amount));
                                }
                                Double rate = 0;
                                foreach (KeyValuePair<string, string> semvalue in dictcount)
                                {
                                    double.TryParse(Convert.ToString(semvalue.Key), out rate);
                                    break;
                                }
                                //  double.TryParse(Convert.ToString(dvamt[0]["demand"]), out rate);
                                //Double rate = allotamount / count;
                                //rate = Math.Round(rate, 0, MidpointRounding.AwayFromZero);
                                concessinamount = 0;
                                double.TryParse(Convert.ToString(dvallot[0]["deduction"]), out concessinamount);


                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 1].Text = allotamount.ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Text = rate.ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 5].Text = Convert.ToString(concessinamount);

                                Double dic = 0;

                                if (dicgrandtotal.ContainsKey(d))
                                {
                                    dic = dicgrandtotal[d];
                                    dicgrandtotal[d] = dic + rate;
                                }
                                else
                                    dicgrandtotal.Add(d, rate);

                                if (httotal.ContainsKey(d))
                                {
                                    Double amount = 0;
                                    double.TryParse(Convert.ToString(httotal[d]), out amount);
                                    httotal[d] = amount + rate;
                                }
                                else
                                    httotal.Add(d, rate);

                                //if (!htfnlTot.ContainsKey("Rate"))//final total
                                //    htfnlTot.Add("Rate", Convert.ToString(rate));
                                //else
                                //{
                                //    double amount = 0;
                                //    double.TryParse(Convert.ToString(htfnlTot["Rate"]), out amount);
                                //    amount += rate;
                                //    htfnlTot.Remove("Rate");
                                //    htfnlTot.Add("Rate", Convert.ToString(amount));
                                //}

                                if (dicgrandtotal.ContainsKey(d + 1))
                                {
                                    dic = dicgrandtotal[d + 1];
                                    dicgrandtotal[d + 1] = dic + allotamount;
                                }
                                else
                                    dicgrandtotal.Add((d + 1), allotamount);

                                if (httotal.ContainsKey(d + 1))
                                {
                                    Double amount = 0;
                                    double.TryParse(Convert.ToString(httotal[d + 1]), out amount);
                                    httotal[d + 1] = amount + allotamount;
                                }
                                else
                                    httotal.Add(d + 1, allotamount);

                                if (dicgrandtotal.ContainsKey(d + 5))
                                {
                                    dic = dicgrandtotal[d + 5];
                                    dicgrandtotal[d + 5] = dic + concessinamount;
                                }
                                else
                                    dicgrandtotal.Add((d + 5), concessinamount);
                                if (httotal.ContainsKey(d + 5))
                                {
                                    Double amount = 0;
                                    double.TryParse(Convert.ToString(httotal[d + 5]), out amount);
                                    httotal[d + 5] = amount + concessinamount;
                                }
                                else
                                    httotal.Add(d + 5, concessinamount);
                                if (!htfnlTot.ContainsKey("Concession"))//final total
                                    htfnlTot.Add("Concession", Convert.ToString(concessinamount));
                                else
                                {
                                    double amount = 0;
                                    double.TryParse(Convert.ToString(htfnlTot["Concession"]), out amount);
                                    amount += concessinamount;
                                    htfnlTot.Remove("Concession");
                                    htfnlTot.Add("Concession", Convert.ToString(amount));
                                }

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 1].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 5].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 5].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 5].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].HorizontalAlign = HorizontalAlign.Right;
                                #endregion
                            }

                            ds2.Tables[0].DefaultView.RowFilter = filterset;
                            DataView dvpaid = ds2.Tables[0].DefaultView;
                            if (dvpaid.Count > 0)
                            {
                                #region paid
                                DataTable dtpaid = new DataTable();
                                dtpaid = dvpaid.ToTable();
                                paidamount = Convert.ToInt32(dtpaid.Compute("sum(paid)", ""));

                                // paidamount = Convert.ToDouble(dvpaid[0]["paid"].ToString());
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 2].Text = paidamount.ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 2].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 2].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 2].HorizontalAlign = HorizontalAlign.Right;

                                Double dic = 0;

                                if (dicgrandtotal.ContainsKey(d + 2))
                                {
                                    dic = dicgrandtotal[d + 2];
                                    dicgrandtotal[d + 2] = dic + paidamount;
                                }
                                else
                                    dicgrandtotal.Add(d + 2, paidamount);

                                if (httotal.ContainsKey(d + 2))
                                {
                                    Double amount = 0;
                                    double.TryParse(Convert.ToString(httotal[d + 2]), out amount);
                                    httotal[d + 2] = amount + paidamount;
                                }
                                else
                                    httotal.Add(d + 2, paidamount);
                                if (!htfnlTot.ContainsKey("Collected"))//final total
                                    htfnlTot.Add("Collected", Convert.ToString(paidamount));
                                else
                                {
                                    double amount = 0;
                                    double.TryParse(Convert.ToString(htfnlTot["Collected"]), out amount);
                                    amount += paidamount;
                                    htfnlTot.Remove("Collected");
                                    htfnlTot.Add("Collected", Convert.ToString(amount));
                                }
                                #endregion
                            }

                            Double balanceamount = allotamount - paidamount - concessinamount;
                            Double dict = 0;
                            if (dicgrandtotal.ContainsKey(d + 3))
                            {
                                dict = dicgrandtotal[d + 3];
                                dicgrandtotal[d + 3] = dict + balanceamount;
                            }
                            else
                                dicgrandtotal.Add(d + 3, balanceamount);

                            if (httotal.ContainsKey(d + 3))
                            {
                                Double amount = 0;
                                double.TryParse(Convert.ToString(httotal[d + 3]), out amount);
                                httotal[d + 3] = amount + balanceamount;
                            }
                            else
                                httotal.Add(d + 3, balanceamount);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 3].Text = balanceamount.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 3].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 3].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 3].HorizontalAlign = HorizontalAlign.Right;
                            if (!htfnlTot.ContainsKey("Balance"))//final total
                                htfnlTot.Add("Balance", Convert.ToString(balanceamount));
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(htfnlTot["Balance"]), out amount);
                                amount += balanceamount;
                                htfnlTot.Remove("Balance");
                                htfnlTot.Add("Balance", Convert.ToString(amount));
                            }

                            balanceamount = balanceamount + paidamount;
                            if (dicgrandtotal.ContainsKey(d + 4))
                            {
                                dict = dicgrandtotal[d + 4];
                                dicgrandtotal[d + 4] = dict + balanceamount;
                            }
                            else
                                dicgrandtotal.Add(d + 4, balanceamount);

                            if (httotal.ContainsKey(d + 4))
                            {
                                Double amount = 0;
                                double.TryParse(Convert.ToString(httotal[d + 4]), out amount);
                                httotal[d + 4] = amount + balanceamount;
                            }
                            else
                                httotal.Add(d + 4, balanceamount);

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 4].Text = balanceamount.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 4].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 4].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 4].HorizontalAlign = HorizontalAlign.Right;
                            if (!htfnlTot.ContainsKey("Total"))//final total
                                htfnlTot.Add("Total", Convert.ToString(balanceamount));
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(htfnlTot["Total"]), out amount);
                                amount += balanceamount;
                                htfnlTot.Remove("Total");
                                htfnlTot.Add("Total", Convert.ToString(amount));
                            }
                            double fnldueamt = 0;
                            ds2.Tables[1].DefaultView.RowFilter = "header_id='" + getfeecode + "' and Batch_Year='" + dtFirstTbl.Rows[i]["Batch_Year"].ToString() + "' and degree_code='" + dtFirstTbl.Rows[i]["degree_code"].ToString() + "'";
                            DataView dvduepaid = ds2.Tables[1].DefaultView;
                            if (dvduepaid.Count > 0)
                            {
                                for (int js = 0; js < dvduepaid.Count; js++)
                                {
                                    double tempdue = 0;
                                    string sem = dvduepaid[js]["FeeCategory"].ToString();
                                    if (semcode != sem)
                                    {
                                        double.TryParse(Convert.ToString(dvduepaid[js]["paid"]), out tempdue);
                                        fnldueamt += tempdue;
                                    }
                                }
                            }
                            if (dicgrandtotal.ContainsKey(d + 6))
                            {
                                dict = dicgrandtotal[d + 6];
                                dicgrandtotal[d + 6] = dict + fnldueamt;
                            }
                            else
                                dicgrandtotal.Add(d + 6, fnldueamt);
                            if (httotal.ContainsKey(d + 6))
                            {
                                Double amount = 0;
                                double.TryParse(Convert.ToString(httotal[d + 6]), out amount);
                                httotal[d + 6] = amount + fnldueamt;
                            }
                            else
                                httotal.Add(d + 6, fnldueamt);
                            if (!htfnlTot.ContainsKey("Arrear"))//final total
                                htfnlTot.Add("Arrear", Convert.ToString(fnldueamt));
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(htfnlTot["Arrear"]), out amount);
                                amount += fnldueamt;
                                htfnlTot.Remove("Arrear");
                                htfnlTot.Add("Arrear", Convert.ToString(amount));
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 6].Text = Convert.ToString(fnldueamt);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 6].HorizontalAlign = HorizontalAlign.Right;
                        }
                        #region final total columns
                        foreach (KeyValuePair<string, int> getcol in htcolCnt)
                        {
                            string strKey = Convert.ToString(getcol.Key);
                            int strValue = getcol.Value;
                            double tempAmt = 0;
                            double.TryParse(Convert.ToString(htfnlTot[strKey]), out tempAmt);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, strValue].Text = Convert.ToString(tempAmt);
                            // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, strValue].Font.Bold = true;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, strValue].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, strValue].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, strValue].HorizontalAlign = HorizontalAlign.Right;
                            if (httotal.ContainsKey(strKey))
                            {
                                Double amount = 0;
                                double.TryParse(Convert.ToString(httotal[strKey]), out amount);
                                httotal[strKey] = amount + tempAmt;
                            }
                            else
                                httotal.Add(strKey, tempAmt);

                            if (dicgrandtotal.ContainsKey(strValue))
                            {
                                Double amount = 0;
                                double.TryParse(Convert.ToString(dicgrandtotal[strValue]), out amount);
                                dicgrandtotal[strValue] = amount + tempAmt;
                            }
                            else
                                dicgrandtotal.Add(strValue, tempAmt);

                        }
                        htfnlTot.Clear();
                        #endregion
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
                #region last degree total
                if (httotal.Count > 0)
                {
                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 3);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.YellowGreen;
                    for (int d = 4; d < FpSpread1.Sheets[0].Columns.Count - 6; d++)
                    {
                        Double dict = 0;
                        if (httotal.ContainsKey(d))
                            double.TryParse(Convert.ToString(httotal[d]), out dict);
                        // dict = httotal[d];                                   
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Text = Convert.ToString(dict);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Font.Bold = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Font.Name = "Book Antiqua";
                        if (d == 4)
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].HorizontalAlign = HorizontalAlign.Center;
                        else
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].HorizontalAlign = HorizontalAlign.Right;
                    }
                    foreach (KeyValuePair<string, int> getcol in htcolCnt)
                    {
                        string strKey = Convert.ToString(getcol.Key);
                        int strValue = getcol.Value;
                        double tempAmt = 0;
                        double.TryParse(Convert.ToString(httotal[strKey]), out tempAmt);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, strValue].Text = Convert.ToString(tempAmt);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, strValue].Font.Bold = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, strValue].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, strValue].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, strValue].HorizontalAlign = HorizontalAlign.Right;
                    }
                    httotal.Clear();
                }
                #endregion
                FpSpread1.Sheets[0].RowCount++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 3);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                for (int d = 4; d < FpSpread1.Sheets[0].Columns.Count; d++)
                {
                    Double dict = 0;
                    if (dicgrandtotal.ContainsKey(d))
                    {
                        dict = dicgrandtotal[d];
                    }
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Text = dict.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Font.Bold = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].HorizontalAlign = HorizontalAlign.Right;
                }
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            }
            if (ddlacctype.SelectedItem.Text == "Header")
            {
                if (FpSpread1.Sheets[0].ColumnHeader.RowCount > 2)
                    FpSpread1.Sheets[0].ColumnHeader.Rows[2].Visible = false;
            }
            else
            {
                if (FpSpread1.Sheets[0].ColumnHeader.RowCount > 2)
                    FpSpread1.Sheets[0].ColumnHeader.Rows[2].Visible = true;
            }
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            FpSpread1.Visible = true;
            if (checksave == true)
            {
                FpSpread1.Visible = true;
                btnprintmaster.Visible = true;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
            }
            else
            {
                // div1.Visible = false;
                FpSpread1.Visible = false;
                errmsg.Visible = true;
                errmsg.Text = "No Records Found";
                FpSpread1.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                return;
            }



            //}
            //else
            //{
            //    // div1.Visible = false;
            //    FpSpread1.Visible = false;
            //    errmsg.Visible = true;
            //    errmsg.Text = "Please Select Seat Type";
            //    FpSpread1.Visible = false;
            //    btnprintmaster.Visible = false;
            //    lblrptname.Visible = false;
            //    txtexcelname.Visible = false;
            //    btnxl.Visible = false;
            //    return;
            //}
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        Printcontrol.Visible = true;
        string degreedetails = string.Empty;


        degreedetails = " Course Wise Student's Fee Report ";
        string pagename = "Overall_student_Fee_Status.aspx";
        Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
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

    #region sem
    protected void cb_sem_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cb_sem, cbl_sem, txt_sem, "Semester", "--Select--");
        }
        catch (Exception ex)
        { }
    }
    protected void cbl_sem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_sem, cbl_sem, txt_sem, "Semester", "--Select--");
        }
        catch (Exception ex)
        { }

    }

    protected void bindsem()
    {
        try
        {
            string clgvalue = collegecode;
            cbl_sem.Items.Clear();
            cb_sem.Checked = false;
            txt_sem.Text = "--Select--";
            ds.Clear();
            string linkName = string.Empty;
            string cbltext = string.Empty;
            ds = da.loadFeecategory(clgvalue, usercode, ref linkName);
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

    //protected void bindsem()
    //{
    //    try
    //    {
    //        string sem = "";
    //        string clgvalue = collegecode;
    //        string semyear = "select * from New_InsSettings where linkname = 'SemesterandYear' and user_code ='" + usercode + "' and college_code ='" + clgvalue + "'";
    //        DataSet dsset = new DataSet();
    //        dsset.Clear();
    //        dsset = da.select_method_wo_parameter(semyear, "Text");
    //        if (dsset.Tables.Count > 0 && dsset.Tables[0].Rows.Count > 0)
    //        {
    //            string value = Convert.ToString(dsset.Tables[0].Rows[0]["LinkValue"]);
    //            if (value == "1")
    //            {
    //                string SelectQ = "select * from textvaltable where TextCriteria = 'FEECA'and (textval like '%Semester' or textval like '%Year') and textval not like '-1%' and college_code ='" + clgvalue + "' order by len(Textval) desc";
    //                ds.Clear();
    //                ds = da.select_method_wo_parameter(SelectQ, "Text");
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    //text_circode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
    //                    cbl_sem.DataSource = ds;
    //                    cbl_sem.DataTextField = "TextVal";
    //                    cbl_sem.DataValueField = "TextCode";
    //                    cbl_sem.DataBind();
    //                }
    //                if (cbl_sem.Items.Count > 0)
    //                {
    //                    for (int i = 0; i < cbl_sem.Items.Count; i++)
    //                    {
    //                        cbl_sem.Items[i].Selected = true;
    //                        sem = Convert.ToString(cbl_sem.Items[i].Text);
    //                    }
    //                    if (cbl_sem.Items.Count == 1)
    //                    {
    //                        txt_sem.Text = "SemesterandYear(" + sem + ")";
    //                    }
    //                    else
    //                    {
    //                        txt_sem.Text = "SemesterandYear(" + cbl_sem.Items.Count + ")";
    //                    }
    //                    cb_sem.Checked = true;
    //                }

    //            }
    //            else
    //            {
    //                cbl_sem.Items.Clear();
    //                string settingquery = "select * from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + clgvalue + "'";
    //                ds.Clear();
    //                ds = da.select_method_wo_parameter(settingquery, "Text");
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    string linkvalue = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
    //                    if (linkvalue == "0")
    //                    {
    //                        string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Semester' and textval not like '-1%' and college_code ='" + clgvalue + "' order by textval asc";
    //                        ds.Clear();
    //                        ds = da.select_method_wo_parameter(semesterquery, "Text");
    //                        if (ds.Tables[0].Rows.Count > 0)
    //                        {
    //                            //text_circode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
    //                            cbl_sem.DataSource = ds;
    //                            cbl_sem.DataTextField = "TextVal";
    //                            cbl_sem.DataValueField = "TextCode";
    //                            cbl_sem.DataBind();
    //                        }
    //                        if (cbl_sem.Items.Count > 0)
    //                        {
    //                            for (int i = 0; i < cbl_sem.Items.Count; i++)
    //                            {
    //                                cbl_sem.Items[i].Selected = true;
    //                                sem = Convert.ToString(cbl_sem.Items[i].Text);
    //                            }
    //                            if (cbl_sem.Items.Count == 1)
    //                            {
    //                                txt_sem.Text = "Semester(" + sem + ")";
    //                            }
    //                            else
    //                            {
    //                                txt_sem.Text = "Semester(" + cbl_sem.Items.Count + ")";
    //                            }
    //                            cb_sem.Checked = true;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Year' and textval not like '-1%' and college_code ='" + clgvalue + "'";
    //                        ds.Clear();
    //                        ds = da.select_method_wo_parameter(semesterquery, "Text");
    //                        if (ds.Tables[0].Rows.Count > 0)
    //                        {
    //                            // text_circode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
    //                            cbl_sem.DataSource = ds;
    //                            cbl_sem.DataTextField = "TextVal";
    //                            cbl_sem.DataValueField = "TextCode";
    //                            cbl_sem.DataBind();
    //                        }
    //                        if (cbl_sem.Items.Count > 0)
    //                        {
    //                            for (int i = 0; i < cbl_sem.Items.Count; i++)
    //                            {
    //                                cbl_sem.Items[i].Selected = true;
    //                                sem = Convert.ToString(cbl_sem.Items[i].Text);
    //                            }
    //                            if (cbl_sem.Items.Count == 1)
    //                            {
    //                                txt_sem.Text = "Year(" + sem + ")";
    //                            }
    //                            else
    //                            {
    //                                txt_sem.Text = "Year(" + cbl_sem.Items.Count + ")";
    //                            }
    //                            cb_sem.Checked = true;
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch { }
    //}


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

    protected void feeCatVal(string curSem, ArrayList arFeecat)
    {
        try
        {
            string type = "";
            string feecatg = "";
            Dictionary<string, string> htsem = new Dictionary<string, string>();
            string strtype = da.GetFunction("select LinkValue from New_InsSettings where college_code='" + collegecode + "' and LinkName='Fee Yearwise' And user_code = '" + usercode + "'");
            if (strtype == "0")
            {
                type = "Semester";
            }
            else if (strtype == "1")
            {
                type = "Yearly";
            }
            else if (strtype == "2")
            {
                type = "Term";
            }
            string cblvalue = "";
            string cbltext = "";
            if (!cbCurSem.Checked)
            {
                #region without current semester
                for (int sem = 0; sem < cbl_sem.Items.Count; sem++)
                {
                    if (cbl_sem.Items[sem].Selected == true)
                    {
                        cblvalue = Convert.ToString(cbl_sem.Items[sem].Value);
                        cbltext = Convert.ToString(cbl_sem.Items[sem].Text);
                        if (type == "Semester")
                        {
                            string[] feesem = cbl_sem.Items[sem].Text.Split(' ');
                            if (feesem[0] == "1" || feesem[0] == "2")
                            {
                                if (!htsem.ContainsKey("1 Year"))
                                    htsem.Add(Convert.ToString("1 Year"), Convert.ToString(cblvalue));
                                else
                                {
                                    feecatg = Convert.ToString(htsem["1 Year"]);
                                    feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                    htsem.Remove("1 Year");
                                    htsem.Add(Convert.ToString("1 Year"), feecatg);
                                }
                            }
                            else if (feesem[0] == "3" || feesem[0] == "4")
                            {
                                if (!htsem.ContainsKey("2 Year"))
                                    htsem.Add(Convert.ToString("2 Year"), Convert.ToString(cblvalue));
                                else
                                {
                                    feecatg = Convert.ToString(htsem["2 Year"]);
                                    feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                    htsem.Remove("2 Year");
                                    htsem.Add(Convert.ToString("2 Year"), feecatg);
                                }
                            }
                            else if (feesem[0] == "5" || feesem[0] == "6")
                            {
                                if (!htsem.ContainsKey("3 Year"))
                                    htsem.Add(Convert.ToString("3 Year"), Convert.ToString(cblvalue));
                                else
                                {
                                    feecatg = Convert.ToString(htsem["3 Year"]);
                                    feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                    htsem.Remove("3 Year");
                                    htsem.Add(Convert.ToString("3 Year"), feecatg);
                                }
                            }
                            else if (feesem[0] == "7" || feesem[0] == "8")
                            {
                                if (!htsem.ContainsKey("4 Year"))
                                    htsem.Add(Convert.ToString("4 Year"), Convert.ToString(cblvalue));
                                else
                                {
                                    feecatg = Convert.ToString(htsem["4 Year"]);
                                    feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                    htsem.Remove("4 Year");
                                    htsem.Add(Convert.ToString("4 Year"), feecatg);
                                };
                            }
                        }
                        else
                        {
                            if (!htsem.ContainsKey(cbltext))
                                htsem.Add(Convert.ToString(cbltext), Convert.ToString(cblvalue));
                            else
                            {
                                feecatg = Convert.ToString(htsem[cbltext]);
                                feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                htsem.Remove(cbltext);
                                htsem.Add(Convert.ToString(cbltext), feecatg);
                            };
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region with current semester
                for (int sem = 0; sem < cbl_sem.Items.Count; sem++)
                {
                    if (cbl_sem.Items[sem].Selected == true)
                    {
                        cblvalue = Convert.ToString(cbl_sem.Items[sem].Value);
                        cbltext = Convert.ToString(cbl_sem.Items[sem].Text);
                        if (arFeecat.Contains(cblvalue))//if current semester setting available only
                        {
                            if (type == "Semester")
                            {
                                string[] feesem = cbl_sem.Items[sem].Text.Split(' ');
                                if (feesem[0] == "1" || feesem[0] == "2")
                                {
                                    if (!htsem.ContainsKey("1 Year"))
                                        htsem.Add(Convert.ToString("1 Year"), Convert.ToString(cblvalue));
                                    else
                                    {
                                        feecatg = Convert.ToString(htsem["1 Year"]);
                                        feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                        htsem.Remove("1 Year");
                                        htsem.Add(Convert.ToString("1 Year"), feecatg);
                                    }
                                }
                                else if (feesem[0] == "3" || feesem[0] == "4")
                                {
                                    if (!htsem.ContainsKey("2 Year"))
                                        htsem.Add(Convert.ToString("2 Year"), Convert.ToString(cblvalue));
                                    else
                                    {
                                        feecatg = Convert.ToString(htsem["2 Year"]);
                                        feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                        htsem.Remove("2 Year");
                                        htsem.Add(Convert.ToString("2 Year"), feecatg);
                                    }
                                }
                                else if (feesem[0] == "5" || feesem[0] == "6")
                                {
                                    if (!htsem.ContainsKey("3 Year"))
                                        htsem.Add(Convert.ToString("3 Year"), Convert.ToString(cblvalue));
                                    else
                                    {
                                        feecatg = Convert.ToString(htsem["3 Year"]);
                                        feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                        htsem.Remove("3 Year");
                                        htsem.Add(Convert.ToString("3 Year"), feecatg);
                                    }
                                }
                                else if (feesem[0] == "7" || feesem[0] == "8")
                                {
                                    if (!htsem.ContainsKey("4 Year"))
                                        htsem.Add(Convert.ToString("4 Year"), Convert.ToString(cblvalue));
                                    else
                                    {
                                        feecatg = Convert.ToString(htsem["4 Year"]);
                                        feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                        htsem.Remove("4 Year");
                                        htsem.Add(Convert.ToString("4 Year"), feecatg);
                                    };
                                }
                            }
                            else
                            {
                                if (!htsem.ContainsKey(cbltext))
                                    htsem.Add(Convert.ToString(cbltext), Convert.ToString(cblvalue));
                                else
                                {
                                    feecatg = Convert.ToString(htsem[cbltext]);
                                    feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                    htsem.Remove(cbltext);
                                    htsem.Add(Convert.ToString(cbltext), feecatg);
                                }
                            }
                        }
                    }
                }
                #endregion
            }

            ViewState["feecat"] = htsem;

        }
        catch { }
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

    #region datewise

    protected void chk_datewise_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chk_datewise.Checked == true)
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

        lbl.Add(lblstr);
        lbl.Add(lbldeg);
        lbl.Add(lbldept);
        lbl.Add(lblsem);
        fields.Add(1);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }

    //added batch and feecategory setting added here 11.08.2017
    protected void lnkSetting_Click(object sender, EventArgs e)
    {
        divSetting.Visible = true;
        bindSettingGrid();
    }
    protected void imgSetting_Click(object sender, EventArgs e)
    {
        divSetting.Visible = false;
    }
    protected void bindSettingGrid()
    {
        try
        {
            ArrayList addnew = new ArrayList();
            addnew.Add("1");
            DataTable dtSetting = new DataTable();
            dtSetting.Columns.Add("Sno");
            dtSetting.Columns.Add("Batch");
            dtSetting.Columns.Add("Feecategory");
            DataRow dr;
            for (int row = 0; row < addnew.Count; row++)
            {
                dr = dtSetting.NewRow();
                dr[0] = addnew[row].ToString();
                dtSetting.Rows.Add(dr);
            }
            if (dtSetting.Rows.Count > 0)
            {
                ViewState["CurrentTable"] = dtSetting;
                gdSetting.DataSource = dtSetting;
                gdSetting.DataBind();
            }
        }
        catch { }
    }
    protected void gdSetting_OnDataBound(object sender, EventArgs e)
    {
        try
        {
            string linkName = string.Empty;

            if (gdSetting.Rows.Count > 0)
            {
                DataSet dsBatch = new DataSet();
                dsBatch = batchLoad();
                for (int a = 0; a < gdSetting.Rows.Count; a++)
                {
                    (gdSetting.Rows[a].FindControl("ddlBatch") as DropDownList).Items.Clear();
                    if (dsBatch.Tables.Count > 0 && dsBatch.Tables[0].Rows.Count > 0)
                    {
                        (gdSetting.Rows[a].FindControl("ddlBatch") as DropDownList).DataSource = dsBatch;
                        (gdSetting.Rows[a].FindControl("ddlBatch") as DropDownList).DataTextField = "Batch_year";
                        (gdSetting.Rows[a].FindControl("ddlBatch") as DropDownList).DataValueField = "Batch_year";
                        (gdSetting.Rows[a].FindControl("ddlBatch") as DropDownList).DataBind();
                    }
                    (gdSetting.Rows[a].FindControl("ddlBatch") as DropDownList).Items.Insert(0, "Select");
                    (gdSetting.Rows[a].FindControl("ddlFeecat") as DropDownList).Items.Clear();
                    DataSet dsFee = da.loadFeecategory(collegecode, usercode, ref linkName);
                    if (dsFee.Tables.Count > 0 && dsFee.Tables[0].Rows.Count > 0)
                    {
                        (gdSetting.Rows[a].FindControl("ddlFeecat") as DropDownList).DataSource = dsFee;
                        (gdSetting.Rows[a].FindControl("ddlFeecat") as DropDownList).DataTextField = "TextVal";
                        (gdSetting.Rows[a].FindControl("ddlFeecat") as DropDownList).DataValueField = "TextCode";
                        (gdSetting.Rows[a].FindControl("ddlFeecat") as DropDownList).DataBind();
                    }
                    (gdSetting.Rows[a].FindControl("ddlFeecat") as DropDownList).Items.Insert(0, "Select");
                }
            }
        }
        catch
        { }
    }

    public void btnAddRow_Click(object sender, EventArgs e)
    {
        try
        {
            if (gdSetting.Rows.Count > 0)
            {
                AddNewRowToGrid();
                gdSetting_OnDataBound(sender, e);
                SetPreviousData();
            }
        }
        catch
        {
        }
    }
    public void AddNewRowToGrid()
    {
        try
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                DropDownList box1 = new DropDownList();
                DropDownList box2 = new DropDownList();
                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                    {
                        //extract the TextBox values
                        box1 = (DropDownList)gdSetting.Rows[i].Cells[1].FindControl("ddlBatch");
                        box2 = (DropDownList)gdSetting.Rows[i].Cells[2].FindControl("ddlFeecat");
                        drCurrentRow = dtCurrentTable.NewRow();
                        dtCurrentTable.Rows[i][0] = Convert.ToString(i + 1);
                        dtCurrentTable.Rows[i][1] = box1.Text;
                        dtCurrentTable.Rows[i][2] = box2.Text;
                        rowIndex++;
                    }
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["CurrentTable"] = dtCurrentTable;
                    gdSetting.DataSource = dtCurrentTable;
                    gdSetting.DataBind();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"View State Null\");", true);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void SetPreviousData()
    {
        try
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                Hashtable hashlist = new Hashtable();
                if (dt.Rows.Count > 0)
                {
                    DropDownList box1 = new DropDownList();
                    DropDownList box2 = new DropDownList();
                    Label lbl = new Label();
                    hashlist.Add(0, "Sno");
                    hashlist.Add(1, "Batch");
                    hashlist.Add(2, "Feecategory");
                    DataRow dr;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        box1 = (DropDownList)gdSetting.Rows[i].Cells[1].FindControl("ddlBatch");
                        box2 = (DropDownList)gdSetting.Rows[i].Cells[2].FindControl("ddlFeecat");
                        lbl = (Label)gdSetting.Rows[i].Cells[0].FindControl("lblsno");
                        string val_file = Convert.ToString(hashlist[i]);
                        lbl.Text = Convert.ToString(i + 1);
                        string batch = dt.Rows[i][1].ToString();
                        string feecat = dt.Rows[i][2].ToString();
                        box1.SelectedIndex = box1.Items.IndexOf(box1.Items.FindByValue(Convert.ToString(dt.Rows[i][1])));
                        box2.SelectedIndex = box2.Items.IndexOf(box2.Items.FindByValue(Convert.ToString(dt.Rows[i][2])));
                        rowIndex++;
                    }
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected DataSet batchLoad()
    {
        DataSet dsBatch = new DataSet();
        try
        {
            string strsql = "select distinct batch_year from applyn where batch_year<>'-1' and batch_year<>''order by batch_year desc";
            dsBatch = da.select_method_wo_parameter(strsql, "Text");
        }
        catch { dsBatch.Clear(); }
        return dsBatch;
    }
    protected void btnRowOK_Click(object sender, EventArgs e)
    {
        getSettings();
    }
    protected void getSettings()
    {
        Dictionary<string, string> dtFeecat = new Dictionary<string, string>();
        try
        {
            foreach (GridViewRow gdRow in gdSetting.Rows)
            {
                DropDownList ddlBatch = (DropDownList)gdRow.FindControl("ddlBatch");
                DropDownList ddlFeecat = (DropDownList)gdRow.FindControl("ddlFeecat");
                if (ddlBatch.SelectedItem.Text.Trim() != "Select" && ddlFeecat.SelectedItem.Text.Trim() != "Select")
                {
                    string batch = Convert.ToString(ddlBatch.SelectedItem.Text);
                    string feeCat = Convert.ToString(ddlFeecat.SelectedItem.Value);
                    if (!dtFeecat.ContainsKey(batch + "-" + feeCat))
                    {
                        dtFeecat.Add(batch + "-" + feeCat, feeCat);
                    }
                }
            }
            if (dtFeecat.Count > 0)
            {
                ViewState["FeeSet"] = dtFeecat;
                divSetting.Visible = false;
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Select Values')", true);
            }
        }
        catch { }
    }
}