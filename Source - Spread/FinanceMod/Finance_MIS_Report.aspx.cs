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

public partial class Finance_MIS_Report : System.Web.UI.Page
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
    static byte roll = 0;
    bool check = false; TreeNode subchildnode;
    Hashtable htfeecat = new Hashtable();
    bool usBasedRights = false;
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
                if (checkSchoolSetting() == 0)
                    loadStudenttype();

                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Attributes.Add("readonly", "readonly");
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
                txtdegree.Text = lbldeg.Text + " (1)";
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
                txtbranch.Text = lbldept.Text + " (1)";
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
                            // string strled = "select LedgerName,LedgerPK from FM_LedgerMaster where LedgerName not in ('Cash','Income & Expenditure','Misc') and HeaderFK = (" + ds.Tables[0].Rows[i]["HeaderPK"].ToString() + ")  order by isnull(priority,1000), ledgerName asc ";
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
            //loadheader();
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
                Label1.Text = "Header";
                loadheader();
                txtaccheader.Enabled = true;
            }
            else
            {
                treeview_spreadfields.Visible = true;
                loadheader();
                Label1.Text = "Header";
                txtaccheader.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
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
        if (checkSchoolSetting() == 0)
        {
            if (!cbdue.Checked)
                getwithoutdueScl();
            else
                getwithdueScl();
        }
        else
        {
            if (!cbdue.Checked)
                getwithoutdueClg();
            else
                getwithduesClg();
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
    #region college go
    protected void getwithoutdueClg()
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
            UserbasedRights();
            #region get value

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
            string StudentFees = "";
            string getfeeval = "";

            FarPoint.Web.Spread.TextCellType txtcel = new FarPoint.Web.Spread.TextCellType();

            string finYearFk = string.Empty;
            string finYearFkal = string.Empty;
            for (int fk = 0; fk < chklsfyear.Items.Count; fk++)
            {
                if (chklsfyear.Items[fk].Selected)
                {
                    if (string.IsNullOrEmpty(finYearFk))
                    {
                        finYearFk = chklsfyear.Items[fk].Value;
                    }
                    else
                    {
                        finYearFk += "','" + chklsfyear.Items[fk].Value;
                    }
                }
            }
            finYearFkal = " and f.finyearfk in ('" + finYearFk + "') ";
            finYearFk = " and a.finyearfk in ('" + finYearFk + "') ";


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
            string fromDt = string.Empty;
            string toDt = string.Empty;
            if (cbdate.Checked)
                getDate(ref  fromDt, ref  toDt);

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
            //added by abarna 29.01.2018
            string activerow = "";
            activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
            //-------------------------------
            DataView dvhead = new DataView();

            FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;

            #endregion
            Hashtable htcol = new Hashtable();
            string strledgre = "";
            string strledger = "";
            string gpvalue = "";
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
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 5].Text = treeview_spreadfields.Nodes[remv].Text.ToString();
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 5].HorizontalAlign = HorizontalAlign.Center;
                                    headflag = true;
                                    hstartc = FpSpread1.Sheets[0].ColumnCount - 5;
                                }
                                ledcount++;

                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 5, 1, 5);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 5].Text = treeview_spreadfields.Nodes[remv].ChildNodes[child].Text.ToString();
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 5].HorizontalAlign = HorizontalAlign.Center;
                                feecode = treeview_spreadfields.Nodes[remv].ChildNodes[child].Value.ToString();
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6].Text = "Rate";
                                FpSpread1.Columns[FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6].Visible = false;

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
                                ccount += 6;
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - (ccount - 1), 1, ccount - 1);
                                if (demandfee == "")
                                {
                                    demandfee = feecode;
                                    StudentFees = feecode;
                                }
                                else
                                {
                                    demandfee = demandfee + ',' + feecode;
                                    StudentFees = StudentFees + ',' + feecode;
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
                    boolColFnl = true;
                    //demandfee = " and f.LedgerFK in(" + demandfee + ")";
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
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 5].Text = chklstaccheader.Items[j].Text;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 5].HorizontalAlign = HorizontalAlign.Center;
                                headflag = true;
                                hstartc = FpSpread1.Sheets[0].ColumnCount - 5;
                            }
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 5, 1, 5);
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 5].Text = dvhead[a]["HeaderName"].ToString();
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 5].HorizontalAlign = HorizontalAlign.Center;
                            if (dvhead.Count > 0)
                            {
                                feecode = dvhead[a]["HeaderFK"].ToString();
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6].Text = "Rate";
                                FpSpread1.Columns[FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6].Visible = false;
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

                                if (demandfee == "")
                                {
                                    demandfee = feecode;
                                    gpvalue = feecode;
                                }
                                else
                                {
                                    demandfee = demandfee + ',' + feecode;
                                    gpvalue = gpvalue + ',' + feecode;
                                }
                            }
                            ccount += 6;
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - (ccount - 1), 1, (ccount - 1));
                        }
                    }

                }
                if (demandfee.Trim() != "")
                {
                    demandfee = " and a.HeaderFK in(" + demandfee + ")";
                    boolColFnl = true;
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
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 5, 1, 5);
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 5].Text = chklstaccheader.Items[a].Text;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 5].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6].Text = "Rate";
                        FpSpread1.Columns[FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6].Visible = false;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6].Note = chklstaccheader.Items[a].Value;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 5].Text = "Amount";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 5].Note = chklstaccheader.Items[a].Value;
                        htcol.Add(chklstaccheader.Items[a].Value, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 5);
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 4].Text = "Collected";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 4].Note = chklstaccheader.Items[a].Value;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3].Text = "Arrear";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3].Note = chklstaccheader.Items[a].Value;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Text = "Total";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Note = chklstaccheader.Items[a].Value;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Concession";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Note = chklstaccheader.Items[a].Value;
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
                    demandfee = " and a.HeaderFK in(" + demandfee + ")";
                    StudentFees = " and f.HeaderFK in(" + StudentFees + ")";
                    boolColFnl = true;
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

                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, fnltot, 2, 5);
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

                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, fnltot, 1, 5);
                }
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, fnltot].HorizontalAlign = HorizontalAlign.Center;
            }
            Hashtable ht = htcol;
            string seat = da.GetFunction("select TextCode from TextValTable where TextCriteria='seat' and textval like '%Govt%'");
            //if (seat != "0")
            //{
            #region Query

            #region old
            //////demand
            //demquery = "select count(distinct r.roll_no) as stucount,sum(TotalAmount) as demand,sum(DeductAmout) as deduction,r.degree_code,r.Batch_Year,f.HeaderFK as header_id" + strledgre + "  from FT_FeeAllot a,FM_LedgerMaster f,registration r where a.LedgerFK=f.LedgerPK  and a.App_No =r.App_No and a.TotalAmount>0";
            //if (chkinclude.Checked == true)
            //{
            //    demquery = demquery + " and r.DelFlag=0";
            //}
            //demquery = demquery + " and r.cc=0  and r.Exam_Flag<>'debar' " + degreequery + " " + batchquery + " " + demandfee + " group by r.degree_code,r.Batch_Year,f.HeaderFK" + strledger + " ";


            ////paid query
            //paidquery = "select count(distinct r.roll_no) as stucount,sum(debit) as paid,r.degree_code,r.Batch_Year,f.HeaderFK as header_id" + strledgre + " from FT_FinDailyTransaction a,FM_LedgerMaster f,registration r where a.LedgerFK=f.LedgerPK and a.App_No=r.App_No and a.debit>0 and r.cc=0  and r.Exam_Flag<>'debar' and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1'";
            //if (chkinclude.Checked == true)
            //{
            //    paidquery = paidquery + " and r.DelFlag=0";
            //}
            //paidquery = paidquery + " " + degreequery + " " + batchquery + " " + demandfee + " group by r.degree_code,r.Batch_Year,f.HeaderFK" + strledger + " ";


            ////stud count
            //string str = "select count(r.roll_no) as stcount,r.Batch_Year,c.Course_Name,de.Dept_Name,r.degree_code,r.Current_Semester from Registration r,Degree d,Department de,Course c where r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and c.Course_Id=d.Course_Id and r.cc=0  and r.Exam_Flag<>'debar'";
            //if (chkinclude.Checked == true)
            //{
            //    str = str + " and r.DelFlag=0";
            //}
            //str = str + " " + degreequery + " " + batchquery + " group by r.Batch_Year,c.Course_Name,de.Dept_Name,r.degree_code,r.Current_Semester order by r.degree_code,r.Batch_Year desc,r.Current_Semester";
            #endregion

            #region new

            //demand
            demquery = "select count(distinct r.roll_no) as stucount,sum(feeamount) as demand,sum(DeductAmout) as deduction,r.degree_code,r.Batch_Year,a.HeaderFK as header_id" + strledgre + ",a.FeeCategory    from FT_FeeAllot a,registration r where a.App_No =r.App_No and (a.TotalAmount>0 or a.deductamout>0) " + finYearFk;
            if (commondist != "")
                demquery = demquery + commondist;
            if (cbCurSem.Checked && !string.IsNullOrEmpty(curSem))
                demquery += " and r.current_semester in('" + curSem + "')";
            if (chkinclude.Checked == true)
            {
                //demquery = demquery + " and r.DelFlag=0";
            }
            demquery = demquery + " " + degreequery + " " + batchquery + " " + demandfee + " " + feecatg + " group by r.degree_code,r.Batch_Year,a.HeaderFK" + strledger + ",a.FeeCategory  order by r.degree_code  ";


            //paid query
            paidquery = "select count(distinct r.roll_no) as stucount,sum(debit) as paid,r.degree_code,r.Batch_Year,a.HeaderFK as header_id" + strledgre + ",a.FeeCategory   from FT_FinDailyTransaction a,registration r where  a.App_No=r.App_No and a.debit>0  and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' " + finYearFk;
            if (commondist != "")
                paidquery = paidquery + commondist;
            if (usBasedRights == true)
                paidquery += " and a.EntryUserCode in('" + usercode + "')";
            if (cbCurSem.Checked && !string.IsNullOrEmpty(curSem))
                paidquery += " and r.current_semester in('" + curSem + "')";
            if (!string.IsNullOrEmpty(fromDt) && !string.IsNullOrEmpty(toDt))
                paidquery += " and a.Transdate between '" + fromDt + "' and '" + toDt + "'";
            if (chkinclude.Checked == true)
            {
                //paidquery = paidquery + " and r.DelFlag=0";
            }
            paidquery = paidquery + " " + degreequery + " " + batchquery + " " + demandfee + " " + feecatg + " group by r.degree_code,r.Batch_Year,a.HeaderFK" + strledger + ",a.FeeCategory order by r.degree_code   ";


            //stud count
            string str = "select count(distinct r.roll_no) as stcount,r.Batch_Year,c.Course_Name,de.Dept_Name,r.degree_code,f.FeeCategory from Registration r,Degree d,Department de,Course c ,FT_FeeAllot f where f.App_No =r.App_No and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and c.Course_Id=d.Course_Id" + finYearFkal;
            if (commondist != "")
                str = str + commondist;
            if (cbCurSem.Checked && !string.IsNullOrEmpty(curSem))
                str += " and r.current_semester in('" + curSem + "')";
            if (chkinclude.Checked == true)
            {
                // str = str + " and r.DelFlag=0";
            }
            str = str + " " + degreequery + " " + batchquery + " and f.FeeCategory in('" + feecatval + "') " + StudentFees + " group by r.Batch_Year,c.Course_Name,de.Dept_Name,r.degree_code,f.FeeCategory order by r.degree_code,r.Batch_Year desc,f.FeeCategory ";

            //individual count          
            //demquery += " select distinct sum(TotalAmount) as demand,sum(DeductAmout) as deduction,r.degree_code,r.Batch_Year,a.HeaderFK as header_id" + strledgre + ",a.FeeCategory    from FT_FeeAllot a,registration r where a.App_No =r.App_No and a.TotalAmount>0";
            //if (commondist != "")
            //    demquery = demquery + commondist;

            //if (chkinclude.Checked == true)
            //{
            //    // demquery = demquery + " and r.DelFlag=0";
            //}
            //demquery = demquery + "  " + degreequery + " " + batchquery + " " + demandfee + " " + feecatg + " group by r.degree_code,r.Batch_Year,a.HeaderFK" + strledger + ",a.FeeCategory,r.roll_no  order by r.degree_code  ";

            demquery += " select distinct sum(feeamount) as demand,sum(DeductAmout) as deduction,r.degree_code,r.Batch_Year,a.HeaderFK as header_id" + strledgre + ",a.FeeCategory    from FT_FeeAllotDegree  a,registration r where a.DegreeCode=r.degree_code and a.BatchYear=r.Batch_Year and (a.TotalAmount>0 or a.deductamout>0) and seattype='" + seat + "'" + finYearFk;
            if (commondist != "")
                demquery = demquery + commondist;
            if (cbCurSem.Checked && !string.IsNullOrEmpty(curSem))
                demquery += " and r.current_semester in('" + curSem + "')";
            if (chkinclude.Checked == true)
            {
                // demquery = demquery + " and r.DelFlag=0";
            }
            demquery = demquery + "  " + degreequery + " " + batchquery + " " + demandfee + " " + feecatg + " group by r.degree_code,r.Batch_Year,a.HeaderFK" + strledger + ",a.FeeCategory,r.roll_no  order by r.degree_code  ";

            //amount count
            demquery += " select distinct r.roll_no as stucount,sum(feeamount) as demand,sum(DeductAmout) as deduction,r.degree_code,r.Batch_Year,a.HeaderFK as header_id" + strledgre + ",a.FeeCategory    from FT_FeeAllot a,registration r where a.App_No =r.App_No and (a.TotalAmount>0 or a.deductamout>0) " + finYearFk;
            if (commondist != "")
                demquery = demquery + commondist;
            if (cbCurSem.Checked && !string.IsNullOrEmpty(curSem))
                demquery += " and r.current_semester in('" + curSem + "')";
            if (chkinclude.Checked == true)
            {
                //demquery = demquery + " and r.DelFlag=0";
            }
            demquery = demquery + " " + degreequery + " " + batchquery + " " + demandfee + " " + feecatg + " group by r.degree_code,r.Batch_Year,a.HeaderFK" + strledger + ",a.FeeCategory,r.roll_no  order by r.degree_code ";

            //student count
            //demquery += " select distinct COUNT(r.App_No) as stucount,degree_code,Batch_Year from registration r where r.degree_code in(" + course_all + ")  and r.Batch_year in(" + batch_all + ")";
            //if (commondist != "")
            //    demquery = demquery + commondist;
            //demquery += " group by degree_code,Batch_Year order by degree_code";
            demquery += "select count(distinct r.roll_no) as stucount,sum(feeamount) as demand,sum(DeductAmout) as deduction,r.degree_code,r.Batch_Year" + strledgre + ",a.FeeCategory    from FT_FeeAllot a,registration r where a.App_No =r.App_No and (a.TotalAmount>0 or a.deductamout>0) " + finYearFk;
            if (commondist != "")
                demquery = demquery + commondist;
            if (cbCurSem.Checked && !string.IsNullOrEmpty(curSem))
                demquery += " and r.current_semester in('" + curSem + "')";
            demquery = demquery + " " + degreequery + " " + batchquery + " " + demandfee + " " + feecatg + " group by r.degree_code,r.Batch_Year" + strledger + ",a.FeeCategory  order by r.degree_code  ";
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
            Hashtable httotal = new Hashtable();
            ArrayList ardegree = new ArrayList();
            bool checksave = false;
            if (ds.Tables[0].Rows.Count > 0)
            {
                #region

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
                    divSetting.Visible = false;
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
                Dictionary<int, double> htamt = new Dictionary<int, double>();
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
                //added by abarna 29.01.2018
                for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                {
                    if (i == Convert.ToInt32(activerow))
                    {
                        FpSpread1.Sheets[0].Rows[i].BackColor = Color.LightBlue;
                        FpSpread1.Sheets[0].SelectionBackColor = Color.LightBlue;
                        //Fpspread1.Sheets[0].SelectionForeColor = Color.White;
                    }
                    else
                    {
                        FpSpread1.Sheets[0].Rows[i].BackColor = Color.White;
                    }
                }
                //---------------
                Hashtable htfnlTot = new Hashtable();
             
                for (int i = 0; i < dtFirstTbl.Rows.Count; i++)
                {
                    #region
                    ds1.Tables[3].DefaultView.RowFilter = "Batch_year='" + Convert.ToString(dtFirstTbl.Rows[i]["Batch_year"]) + "' and degree_code='" + Convert.ToString(dtFirstTbl.Rows[i]["degree_code"]) + "' and FeeCategory='" + dtFirstTbl.Rows[i]["FeeCategory"] + "' ";
                    dvcnt = ds1.Tables[3].DefaultView;
                    double studcnt = 0;

                    if (dvcnt.Count > 0)
                    {
                        string degcode = Convert.ToString(dtFirstTbl.Rows[i]["degree_code"]);
                       // string batch=Convert.ToString(dtFirstTbl.Rows[i]["Batch_year"]);
                        
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
                                for (int d = 4; d < FpSpread1.Sheets[0].Columns.Count - 5; d++)
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
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = degcode;
                        double.TryParse(Convert.ToString(dvcnt[0]["stucount"]), out studcnt);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = studcnt.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
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

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = TextName;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(dtFirstTbl.Rows[i]["FeeCategory"]);
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
                        double totalcnt = 0;
                        bool amtver = false;
                        //for (int d = 5; d < FpSpread1.Sheets[0].Columns.Count - 4; d = d + 6)
                        for (int d = 5; d < FpSpread1.Sheets[0].Columns.Count - 9; d = d + 6)
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
                            DataView dvallot = ds1.Tables[1].DefaultView;
                            dvstudcnt = ds1.Tables[0].DefaultView;
                            Dictionary<string, string> dictcount = new Dictionary<string, string>();
                            string deamt = "";
                            if (dvstudcnt.Count > 0)
                            {
                                if (dvstudcnt.Count > 0)
                                {
                                    deamt = Convert.ToString(dvstudcnt[0]["demand"]);
                                }
                                Double rate = 0;
                                double.TryParse(Convert.ToString(deamt), out rate);
                                //dictcount = dictcount.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

                                save = true;
                                checksave = true;
                                allotamount = Convert.ToDouble(deamt);
                                concessinamount = 0;
                                double.TryParse(Convert.ToString(dvstudcnt[0]["deduction"]), out concessinamount);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 1].Text = deamt.ToString();

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
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Text = rate.ToString();
                                //if (rate == 0)
                                //{
                                //    amtver = true;
                                //    htamt.Add(d, rate);
                                //}
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


                            balanceamount = balanceamount + paidamount;//total amount
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
                    #endregion
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
                    for (int d = 4; d < FpSpread1.Sheets[0].Columns.Count - 5; d++)
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
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                #endregion
            }
            if (checksave == true)
            {
                FpSpread1.Visible = true;
                btnprintmaster.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                btnxl.Visible = true;
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
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            FpSpread1.Visible = true;
         
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    protected void getwithduesClg()
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
            UserbasedRights();
            #region get value

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
            string StudentFees = "";
            string getfeeval = "";

            FarPoint.Web.Spread.TextCellType txtcel = new FarPoint.Web.Spread.TextCellType();

            string finYearFk = string.Empty;
            string finYearFkal = string.Empty;
            for (int fk = 0; fk < chklsfyear.Items.Count; fk++)
            {
                if (chklsfyear.Items[fk].Selected)
                {
                    if (string.IsNullOrEmpty(finYearFk))
                    {
                        finYearFk = chklsfyear.Items[fk].Value;
                    }
                    else
                    {
                        finYearFk += "','" + chklsfyear.Items[fk].Value;
                    }
                }
            }
            finYearFkal = " and f.finyearfk in ('" + finYearFk + "') ";
            finYearFk = " and a.finyearfk in ('" + finYearFk + "') ";


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
            Dictionary<string, string> CurSemDeg = new Dictionary<string, string>();
            ArrayList arFeecat = new ArrayList();
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
            string fromDt = string.Empty;
            string toDt = string.Empty;
            if (cbdate.Checked)
                getDate(ref  fromDt, ref  toDt);

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
            //added by abarna 29.01.2018
            string activerow = "";
            activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
            //-----------
            DataView dvhead = new DataView();

            FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;

            #endregion
            Hashtable htcol = new Hashtable();
            string strledgre = "";
            string strledger = "";
            string gpvalue = "";
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
                                FpSpread1.Columns[FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 7].Visible = false;

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
                                ccount += 7;
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - (ccount - 1), 1, ccount - 2);
                                if (demandfee == "")
                                {
                                    demandfee = feecode;
                                    StudentFees = feecode;
                                }
                                else
                                {
                                    demandfee = demandfee + ',' + feecode;
                                    StudentFees = StudentFees + ',' + feecode;
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
                    boolColFnl = true;
                    //demandfee = " and f.LedgerFK in(" + demandfee + ")";
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
                                FpSpread1.Columns[FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 7].Visible = false;
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

                                if (demandfee == "")
                                {
                                    demandfee = feecode;
                                    gpvalue = feecode;
                                }
                                else
                                {
                                    demandfee = demandfee + ',' + feecode;
                                    gpvalue = gpvalue + ',' + feecode;
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
                    boolColFnl = true;
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
                        FpSpread1.Columns[FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 7].Visible = false;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 7].Note = chklstaccheader.Items[a].Value;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6].Text = "Amount";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6].Note = chklstaccheader.Items[a].Value;
                        htcol.Add(chklstaccheader.Items[a].Value, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6);
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
                    demandfee = " and a.HeaderFK in(" + demandfee + ")";
                    StudentFees = " and f.HeaderFK in(" + StudentFees + ")";
                    boolColFnl = true;
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
            Hashtable ht = htcol;


            #region new

            //demand
            demquery = "select count(distinct r.roll_no) as stucount,sum(feeamount) as demand,sum(DeductAmout) as deduction,r.degree_code,r.Batch_Year,a.HeaderFK as header_id" + strledgre + ",a.FeeCategory    from FT_FeeAllot a,registration r where a.App_No =r.App_No and (a.TotalAmount>0 or a.deductamout>0) " + finYearFk;
            if (commondist != "")
                demquery = demquery + commondist;
            if (cbCurSem.Checked && !string.IsNullOrEmpty(curSem))
                demquery += " and r.current_semester in('" + curSem + "')";

            demquery = demquery + " " + degreequery + " " + batchquery + " " + demandfee + " " + feecatg + " group by r.degree_code,r.Batch_Year,a.HeaderFK" + strledger + ",a.FeeCategory  order by r.degree_code  ";

            //paid query
            paidquery = "select count(distinct r.roll_no) as stucount,sum(debit) as paid,r.degree_code,r.Batch_Year,a.HeaderFK as header_id" + strledgre + ",a.FeeCategory   from FT_FinDailyTransaction a,registration r where  a.App_No=r.App_No and a.debit>0  and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' " + finYearFk;
            if (commondist != "")
                paidquery = paidquery + commondist;
            if (usBasedRights == true)
                paidquery += " and a.EntryUserCode in('" + usercode + "')";
            if (cbCurSem.Checked && !string.IsNullOrEmpty(curSem))
                paidquery += " and r.current_semester in('" + curSem + "')";
            if (!string.IsNullOrEmpty(fromDt) && !string.IsNullOrEmpty(toDt))
                paidquery += " and a.Transdate between '" + fromDt + "' and '" + toDt + "'";
            if (chkinclude.Checked == true)
            {
                //paidquery = paidquery + " and r.DelFlag=0";
            }
            paidquery = paidquery + " " + degreequery + " " + batchquery + " " + demandfee + " " + feecatg + " group by r.degree_code,r.Batch_Year,a.HeaderFK" + strledger + ",a.FeeCategory order by r.degree_code   ";

            paidquery += " select count(distinct r.roll_no) as stucount,sum(debit) as paid,r.degree_code,r.Batch_Year,a.HeaderFK as header_id" + strledgre + ",a.FeeCategory   from FT_FinDailyTransaction a,registration r where  a.App_No=r.App_No and a.debit>0  and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' " + finYearFk;
            if (commondist != "")
                paidquery = paidquery + commondist;
            if (usBasedRights == true)
                paidquery += " and a.EntryUserCode in('" + usercode + "')";
            if (cbCurSem.Checked && !string.IsNullOrEmpty(curSem))
                paidquery += " and r.current_semester in('" + curSem + "')";
            if (!string.IsNullOrEmpty(fromDt) && !string.IsNullOrEmpty(toDt))
                paidquery += " and a.Transdate between '" + fromDt + "' and '" + toDt + "'";
            if (chkinclude.Checked == true)
            {
                //paidquery = paidquery + " and r.DelFlag=0";
            }
            paidquery = paidquery + " " + degreequery + " " + batchquery + " " + demandfee + " " + feecatg + " group by r.degree_code,r.Batch_Year,a.HeaderFK" + strledger + ",a.FeeCategory order by r.degree_code   ";


            //stud count
            string str = "select count(distinct r.roll_no) as stcount,r.Batch_Year,c.Course_Name,de.Dept_Name,r.degree_code,f.FeeCategory  from Registration r,Degree d,Department de,Course c ,FT_FeeAllot f where f.App_No =r.App_No and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and c.Course_Id=d.Course_Id" + finYearFkal;//change(abarna)
            if (commondist != "")
                str = str + commondist;
            if (cbCurSem.Checked && !string.IsNullOrEmpty(curSem))
                str += " and r.current_semester in('" + curSem + "')";
            if (chkinclude.Checked == true)
            {
                // str = str + " and r.DelFlag=0";
            }
            str = str + " " + degreequery + " " + batchquery + " and f.FeeCategory in('" + feecatval + "') " + StudentFees + " group by r.Batch_Year,c.Course_Name,de.Dept_Name,r.degree_code,f.FeeCategory  order by r.degree_code,r.Batch_Year desc,f.FeeCategory ";

            demquery += " select distinct sum(feeamount) as demand,sum(DeductAmout) as deduction,r.degree_code,r.Batch_Year,a.HeaderFK as header_id" + strledgre + ",a.FeeCategory    from FT_FeeAllotDegree  a,registration r where a.DegreeCode=r.degree_code and a.BatchYear=r.Batch_Year and (a.TotalAmount>0 or a.deductamout>0) " + finYearFk;
            if (commondist != "")
                demquery = demquery + commondist;
            if (cbCurSem.Checked && !string.IsNullOrEmpty(curSem))
                demquery += " and r.current_semester in('" + curSem + "')";

            demquery = demquery + "  " + degreequery + " " + batchquery + " " + demandfee + " " + feecatg + " group by r.degree_code,r.Batch_Year,a.HeaderFK" + strledger + ",a.FeeCategory,r.roll_no  order by r.degree_code  ";

            //amount count
            demquery += " select distinct r.roll_no as stucount,sum(feeamount) as demand,sum(DeductAmout) as deduction,r.degree_code,r.Batch_Year,a.HeaderFK as header_id" + strledgre + ",a.FeeCategory    from FT_FeeAllot a,registration r where a.App_No =r.App_No and (a.TotalAmount>0 or a.deductamout>0) " + finYearFk;
            if (commondist != "")
                demquery = demquery + commondist;
            if (cbCurSem.Checked && !string.IsNullOrEmpty(curSem))
                demquery += " and r.current_semester in('" + curSem + "')";
            demquery = demquery + " " + degreequery + " " + batchquery + " " + demandfee + " " + feecatg + " group by r.degree_code,r.Batch_Year,a.HeaderFK" + strledger + ",a.FeeCategory,r.roll_no  order by r.degree_code ";

            demquery += "select count(distinct r.roll_no) as stucount,sum(feeamount) as demand,sum(DeductAmout) as deduction,r.degree_code,r.Batch_Year" + strledgre + ",a.FeeCategory    from FT_FeeAllot a,registration r where a.App_No =r.App_No and (a.TotalAmount>0 or a.deductamout>0) " + finYearFk;
            if (commondist != "")
                demquery = demquery + commondist;
            if (cbCurSem.Checked && !string.IsNullOrEmpty(curSem))
                demquery += " and r.current_semester in('" + curSem + "')";
            demquery = demquery + " " + degreequery + " " + batchquery + " " + demandfee + " " + feecatg + " group by r.degree_code,r.Batch_Year" + strledger + ",a.FeeCategory  order by r.degree_code  ";
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
                #region

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
                    divSetting.Visible = false;
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
                Dictionary<int, double> htamt = new Dictionary<int, double>();
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
                //added by abarna 29.01.2018
                for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                {
                    if (i == Convert.ToInt32(activerow))
                    {
                        FpSpread1.Sheets[0].Rows[i].BackColor = Color.LightBlue;
                        FpSpread1.Sheets[0].SelectionBackColor = Color.LightBlue;
                        //Fpspread1.Sheets[0].SelectionForeColor = Color.White;
                    }
                    else
                    {
                        FpSpread1.Sheets[0].Rows[i].BackColor = Color.White;
                    }
                }
                //---------------
                Hashtable htfnlTot = new Hashtable();
                for (int i = 0; i < dtFirstTbl.Rows.Count; i++)
                {
                    #region
                    ds1.Tables[3].DefaultView.RowFilter = "Batch_year='" + Convert.ToString(dtFirstTbl.Rows[i]["Batch_year"]) + "' and degree_code='" + Convert.ToString(dtFirstTbl.Rows[i]["degree_code"]) + "' and FeeCategory='" + dtFirstTbl.Rows[i]["FeeCategory"] + "' ";
                    dvcnt = ds1.Tables[3].DefaultView;
                    double studcnt = 0;
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
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(dtFirstTbl.Rows[i]["degree_code"]);
                        double.TryParse(Convert.ToString(dvcnt[0]["stucount"]), out studcnt);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = studcnt.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;

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

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = TextName;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(dtFirstTbl.Rows[i]["FeeCategory"]);
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
                        double totalcnt = 0;
                        bool amtver = false;
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
                            DataView dvallot = ds1.Tables[1].DefaultView;
                            dvstudcnt = ds1.Tables[0].DefaultView;
                            Dictionary<string, string> dictcount = new Dictionary<string, string>();
                            string deamt = "";
                            if (dvstudcnt.Count > 0)
                            {
                                #region demand
                                if (dvstudcnt.Count > 0)
                                {
                                    deamt = Convert.ToString(dvstudcnt[0]["demand"]);
                                }
                                Double rate = 0;
                                double.TryParse(Convert.ToString(deamt), out rate);

                                save = true;
                                checksave = true;
                                allotamount = Convert.ToDouble(deamt);
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
                                concessinamount = 0;
                                double.TryParse(Convert.ToString(dvstudcnt[0]["deduction"]), out concessinamount);


                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 1].Text = deamt.ToString();
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
                                #region
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
                            double fnldueamt = 0;
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
                    #endregion
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
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                #endregion
            }
            if (checksave == true)
            {
                FpSpread1.Visible = true;
                btnprintmaster.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                btnxl.Visible = true;
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
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            FpSpread1.Visible = true;
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    #endregion

    #region school go
    protected void getwithoutdueScl()
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
            UserbasedRights();
            #region get value

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
            string StudentFees = "";
            string getfeeval = "";

            FarPoint.Web.Spread.TextCellType txtcel = new FarPoint.Web.Spread.TextCellType();

            string finYearFk = string.Empty;
            string finYearFkal = string.Empty;
            for (int fk = 0; fk < chklsfyear.Items.Count; fk++)
            {
                if (chklsfyear.Items[fk].Selected)
                {
                    if (string.IsNullOrEmpty(finYearFk))
                    {
                        finYearFk = chklsfyear.Items[fk].Value;
                    }
                    else
                    {
                        finYearFk += "','" + chklsfyear.Items[fk].Value;
                    }
                }
            }
            finYearFkal = " and f.finyearfk in ('" + finYearFk + "') ";
            finYearFk = " and a.finyearfk in ('" + finYearFk + "') ";


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
            string studType = Convert.ToString(getCblSelectedValue(cbl_type));
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
            Dictionary<string, string> CurSemDeg = new Dictionary<string, string>();
            ArrayList arFeecat = new ArrayList();
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
            string fromDt = string.Empty;
            string toDt = string.Empty;
            if (cbdate.Checked)
                getDate(ref  fromDt, ref  toDt);

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
            Hashtable htcol = new Hashtable();
            string strledgre = "";
            string strledger = "";
            string gpvalue = "";
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
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 5].Text = treeview_spreadfields.Nodes[remv].Text.ToString();
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 5].HorizontalAlign = HorizontalAlign.Center;
                                    headflag = true;
                                    hstartc = FpSpread1.Sheets[0].ColumnCount - 5;
                                }
                                ledcount++;

                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 5, 1, 5);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 5].Text = treeview_spreadfields.Nodes[remv].ChildNodes[child].Text.ToString();
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 5].HorizontalAlign = HorizontalAlign.Center;
                                feecode = treeview_spreadfields.Nodes[remv].ChildNodes[child].Value.ToString();
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6].Text = "Rate";
                                FpSpread1.Columns[FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6].Visible = false;

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
                                ccount += 6;
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - (ccount - 1), 1, ccount - 1);
                                if (demandfee == "")
                                {
                                    demandfee = feecode;
                                    StudentFees = feecode;
                                }
                                else
                                {
                                    demandfee = demandfee + ',' + feecode;
                                    StudentFees = StudentFees + ',' + feecode;
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
                    boolColFnl = true;
                    //demandfee = " and f.LedgerFK in(" + demandfee + ")";
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
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 5].Text = chklstaccheader.Items[j].Text;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 5].HorizontalAlign = HorizontalAlign.Center;
                                headflag = true;
                                hstartc = FpSpread1.Sheets[0].ColumnCount - 5;
                            }
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 5, 1, 5);
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 5].Text = dvhead[a]["HeaderName"].ToString();
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 5].HorizontalAlign = HorizontalAlign.Center;
                            if (dvhead.Count > 0)
                            {
                                feecode = dvhead[a]["HeaderFK"].ToString();
                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6].Text = "Rate";
                                FpSpread1.Columns[FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6].Visible = false;
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

                                if (demandfee == "")
                                {
                                    demandfee = feecode;
                                    gpvalue = feecode;
                                }
                                else
                                {
                                    demandfee = demandfee + ',' + feecode;
                                    gpvalue = gpvalue + ',' + feecode;
                                }
                            }
                            ccount += 6;
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - (ccount - 1), 1, (ccount - 1));
                        }
                    }

                }
                if (demandfee.Trim() != "")
                {
                    demandfee = " and a.HeaderFK in(" + demandfee + ")";
                    boolColFnl = true;
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
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 5, 1, 5);
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 5].Text = chklstaccheader.Items[a].Text;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 5].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6].Text = "Rate";
                        FpSpread1.Columns[FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6].Visible = false;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6].Note = chklstaccheader.Items[a].Value;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 5].Text = "Amount";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 5].Note = chklstaccheader.Items[a].Value;
                        htcol.Add(chklstaccheader.Items[a].Value, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 5);
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 4].Text = "Collected";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 4].Note = chklstaccheader.Items[a].Value;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3].Text = "Arrear";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 3].Note = chklstaccheader.Items[a].Value;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Text = "Total";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 2].Note = chklstaccheader.Items[a].Value;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Text = "Concession";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 1].Note = chklstaccheader.Items[a].Value;
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
                    demandfee = " and a.HeaderFK in(" + demandfee + ")";
                    StudentFees = " and f.HeaderFK in(" + StudentFees + ")";
                    boolColFnl = true;
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

                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, fnltot, 2, 5);
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

                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, fnltot, 1, 5);
                }
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, fnltot].HorizontalAlign = HorizontalAlign.Center;
            }
            Hashtable ht = htcol;
            string seat = da.GetFunction("select TextCode from TextValTable where TextCriteria='seat' and textval like '%Govt%'");
            //if (seat != "0")
            //{
            #region Query

            #region new

            //demand
            demquery = "select count(distinct r.roll_no) as stucount,sum(feeamount) as demand,sum(DeductAmout) as deduction,r.degree_code,r.Batch_Year,a.HeaderFK as header_id" + strledgre + ",a.FeeCategory,a.finyearfk    from FT_FeeAllot a,registration r where a.App_No =r.App_No and (a.TotalAmount>0 or a.deductamout>0) " + finYearFk;
            if (commondist != "")
                demquery = demquery + commondist;
            if (!string.IsNullOrEmpty(studType))
                demquery += " and mode in('" + studType + "')";

            if (chkinclude.Checked == true)
            {
                //demquery = demquery + " and r.DelFlag=0";
            }
            demquery = demquery + " " + degreequery + " " + batchquery + " " + demandfee + " " + feecatg + " group by r.degree_code,r.Batch_Year,a.HeaderFK" + strledger + ",a.FeeCategory,a.finyearfk  order by r.degree_code  ";


            //paid query
            paidquery = "select count(distinct r.roll_no) as stucount,sum(debit) as paid,r.degree_code,r.Batch_Year,a.HeaderFK as header_id" + strledgre + ",a.FeeCategory,a.actualfinyearfk   from FT_FinDailyTransaction a,registration r where  a.App_No=r.App_No and a.debit>0  and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' " + finYearFk;
            if (commondist != "")
                paidquery = paidquery + commondist;
            if (usBasedRights == true)
                paidquery += " and a.EntryUserCode in('" + usercode + "')";
            if (!string.IsNullOrEmpty(studType))
                paidquery += " and mode in('" + studType + "')";
            if (!string.IsNullOrEmpty(fromDt) && !string.IsNullOrEmpty(toDt))
                paidquery += " and a.Transdate between '" + fromDt + "' and '" + toDt + "'";
            if (chkinclude.Checked == true)
            {
                //paidquery = paidquery + " and r.DelFlag=0";
            }
            paidquery = paidquery + " " + degreequery + " " + batchquery + " " + demandfee + " " + feecatg + " group by r.degree_code,r.Batch_Year,a.HeaderFK" + strledger + ",a.FeeCategory,a.actualfinyearfk order by r.degree_code   ";


            //stud count
            string str = "select count(distinct r.roll_no) as stcount,r.Batch_Year,c.Course_Name,de.Dept_Name,r.degree_code,f.FeeCategory,f.finyearfk  from Registration r,Degree d,Department de,Course c ,FT_FeeAllot f where f.App_No =r.App_No and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and c.Course_Id=d.Course_Id" + finYearFkal;
            if (commondist != "")
                str = str + commondist;
            if (!string.IsNullOrEmpty(studType))
                str += " and mode in('" + studType + "')";
            if (chkinclude.Checked == true)
            {
                // str = str + " and r.DelFlag=0";
            }
            str = str + " " + degreequery + " " + batchquery + " and f.FeeCategory in('" + feecatval + "') " + StudentFees + " group by r.Batch_Year,c.Course_Name,de.Dept_Name,r.degree_code,f.FeeCategory,f.finyearfk  order by r.degree_code,r.Batch_Year desc,f.FeeCategory ";

            //individual count          
            //demquery += " select distinct sum(TotalAmount) as demand,sum(DeductAmout) as deduction,r.degree_code,r.Batch_Year,a.HeaderFK as header_id" + strledgre + ",a.FeeCategory    from FT_FeeAllot a,registration r where a.App_No =r.App_No and a.TotalAmount>0";
            //if (commondist != "")
            //    demquery = demquery + commondist;

            //if (chkinclude.Checked == true)
            //{
            //    // demquery = demquery + " and r.DelFlag=0";
            //}
            //demquery = demquery + "  " + degreequery + " " + batchquery + " " + demandfee + " " + feecatg + " group by r.degree_code,r.Batch_Year,a.HeaderFK" + strledger + ",a.FeeCategory,r.roll_no  order by r.degree_code  ";

            demquery += " select distinct sum(feeamount) as demand,sum(DeductAmout) as deduction,r.degree_code,r.Batch_Year,a.HeaderFK as header_id" + strledgre + ",a.FeeCategory,a.finyearfk    from FT_FeeAllotDegree  a,registration r where a.DegreeCode=r.degree_code and a.BatchYear=r.Batch_Year and (a.TotalAmount>0 or a.deductamout>0) and seattype='" + seat + "'" + finYearFk;
            if (commondist != "")
                demquery = demquery + commondist;
            if (!string.IsNullOrEmpty(studType))
                demquery += " and mode in('" + studType + "')";
            if (chkinclude.Checked == true)
            {
                // demquery = demquery + " and r.DelFlag=0";
            }
            demquery = demquery + "  " + degreequery + " " + batchquery + " " + demandfee + " " + feecatg + " group by r.degree_code,r.Batch_Year,a.HeaderFK" + strledger + ",a.FeeCategory,r.roll_no,a.finyearfk   order by r.degree_code  ";

            //amount count
            demquery += " select distinct r.roll_no as stucount,sum(feeamount) as demand,sum(DeductAmout) as deduction,r.degree_code,r.Batch_Year,a.HeaderFK as header_id" + strledgre + ",a.FeeCategory,a.finyearfk     from FT_FeeAllot a,registration r where a.App_No =r.App_No and (a.TotalAmount>0 or a.deductamout>0) " + finYearFk;
            if (commondist != "")
                demquery = demquery + commondist;
            if (!string.IsNullOrEmpty(studType))
                demquery += " and mode in('" + studType + "')";
            if (chkinclude.Checked == true)
            {
                //demquery = demquery + " and r.DelFlag=0";
            }
            demquery = demquery + " " + degreequery + " " + batchquery + " " + demandfee + " " + feecatg + " group by r.degree_code,r.Batch_Year,a.HeaderFK" + strledger + ",a.FeeCategory,r.roll_no,a.finyearfk   order by r.degree_code ";

            //student count
            //demquery += " select distinct COUNT(r.App_No) as stucount,degree_code,Batch_Year from registration r where r.degree_code in(" + course_all + ")  and r.Batch_year in(" + batch_all + ")";
            //if (commondist != "")
            //    demquery = demquery + commondist;
            //demquery += " group by degree_code,Batch_Year order by degree_code";
            demquery += "select count(distinct r.roll_no) as stucount,sum(feeamount) as demand,sum(DeductAmout) as deduction,r.degree_code,r.Batch_Year" + strledgre + ",a.FeeCategory,a.finyearfk     from FT_FeeAllot a,registration r where a.App_No =r.App_No and (a.TotalAmount>0 or a.deductamout>0) " + finYearFk;
            if (commondist != "")
                demquery = demquery + commondist;
            if (!string.IsNullOrEmpty(studType))
                demquery += " and mode in('" + studType + "')";
            demquery = demquery + " " + degreequery + " " + batchquery + " " + demandfee + " " + feecatg + " group by r.degree_code,r.Batch_Year" + strledger + ",a.FeeCategory,a.finyearfk   order by r.degree_code  ";
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
            Hashtable httotal = new Hashtable();

            bool checksave = false;
            Hashtable htActYr = getFinyear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                #region
                FpSpread1.Visible = true;
                btnprintmaster.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                btnxl.Visible = true;
                DataView dvcnt = new DataView();
                DataView dvstudcnt = new DataView();
                DataView dvamt = new DataView();
                bool save = false;

                Dictionary<int, double> htamt = new Dictionary<int, double>();
                string tempFinYearfk = string.Empty;
                int rowCnt = 0;
                Hashtable htfnlTot = new Hashtable();
                for (int finYr = 0; finYr < chklsfyear.Items.Count; finYr++)
                {
                    bool boolFinCheck = false;
                    string FinYearfktxt = string.Empty;
                    ArrayList ardeg = new ArrayList();
                    ArrayList ardegree = new ArrayList();
                    if (chklsfyear.Items[finYr].Selected)
                    {
                        string finYearValue = Convert.ToString(chklsfyear.Items[finYr].Value);
                        string finYearText = Convert.ToString(chklsfyear.Items[finYr].Text);
                        ds.Tables[0].DefaultView.RowFilter = " finyearfk='" + finYearValue + "'";
                        DataTable dsStudDet = ds.Tables[0].DefaultView.ToTable();
                        if (dsStudDet.Rows.Count > 0)
                        {
                            for (int i = 0; i < dsStudDet.Rows.Count; i++)
                            {
                                #region
                                ds1.Tables[3].DefaultView.RowFilter = "Batch_year='" + Convert.ToString(dsStudDet.Rows[i]["Batch_year"]) + "' and degree_code='" + Convert.ToString(dsStudDet.Rows[i]["degree_code"]) + "' and FeeCategory='" + dsStudDet.Rows[i]["FeeCategory"] + "' and finyearfk='" + finYearValue + "' ";
                                dvcnt = ds1.Tables[3].DefaultView;
                                double studcnt = 0;
                                if (dvcnt.Count > 0)
                                {
                                    string degcode = Convert.ToString(dsStudDet.Rows[i]["degree_code"]);
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
                                            for (int d = 4; d < FpSpread1.Sheets[0].Columns.Count - 5; d++)
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
                                    if (!boolFinCheck)
                                    {
                                        FpSpread1.Sheets[0].RowCount++;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = finYearText;
                                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 5);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = Color.Green;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        boolFinCheck = true;
                                    }
                                    FpSpread1.Sheets[0].RowCount++;
                                    if (!ardeg.Contains(degcode))
                                    {
                                        ardeg.Add(degcode);
                                        sno = rowCnt;
                                        sno++;
                                        rowCnt++;
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(htActYr[Convert.ToString(chklsfyear.Items[finYr].Value).Trim()]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Note = Convert.ToString(dsStudDet.Rows[i]["Batch_year"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dsStudDet.Rows[i]["Course_Name"].ToString() + '-' + dsStudDet.Rows[i]["Dept_Name"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = degcode;
                                    double.TryParse(Convert.ToString(dvcnt[0]["stucount"]), out studcnt);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = studcnt.ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;

                                    string TextName = "";
                                    string semcode = Convert.ToString(dsStudDet.Rows[i]["FeeCategory"]);
                                    DataView Dview = new DataView();
                                    if (dssem.Tables[0].Rows.Count > 0)
                                    {
                                        dssem.Tables[0].DefaultView.RowFilter = "TextCode=" + semcode + "";
                                        Dview = dssem.Tables[0].DefaultView;
                                        if (Dview.Count > 0)
                                            TextName = Convert.ToString(Dview[0]["TextVal"]);
                                    }

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = TextName;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(dsStudDet.Rows[i]["FeeCategory"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Note = Convert.ToString(dsStudDet.Rows[i]["finyearfk"]);
                                    if (!tempFinYearfk.Contains(Convert.ToString(dsStudDet.Rows[i]["finyearfk"])))
                                    {
                                        if (string.IsNullOrEmpty(tempFinYearfk))
                                            tempFinYearfk = Convert.ToString(dsStudDet.Rows[i]["finyearfk"]);
                                        else
                                            tempFinYearfk += "','" + Convert.ToString(dsStudDet.Rows[i]["finyearfk"]);
                                    }
                                    if (!FinYearfktxt.Contains(Convert.ToString(dsStudDet.Rows[i]["finyearfk"])))
                                    {
                                        FinYearfktxt = Convert.ToString(dsStudDet.Rows[i]["finyearfk"]);
                                    }
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
                                    double totalcnt = 0;
                                    bool amtver = false;
                                    for (int d = 5; d < FpSpread1.Sheets[0].Columns.Count - 9; d = d + 6)
                                    {
                                        string getfeecode = "";
                                        if (ddlacctype.SelectedItem.Text != "Header")
                                            getfeecode = FpSpread1.Sheets[0].ColumnHeader.Cells[2, d].Note;

                                        else
                                            getfeecode = FpSpread1.Sheets[0].ColumnHeader.Cells[1, d].Note;

                                        Double allotamount = 0;
                                        Double paidamount = 0;
                                        Double concessinamount = 0;

                                        string filterset = "header_id='" + getfeecode + "' and Batch_Year='" + dsStudDet.Rows[i]["Batch_Year"].ToString() + "' and degree_code='" + dsStudDet.Rows[i]["degree_code"].ToString() + "' and FeeCategory in(" + semcode + ") and finyearfk='" + finYearValue + "'";
                                        if (ddlacctype.SelectedItem.Text == "Ledger")
                                        {
                                            filterset = "Fee_code='" + getfeecode + "' and Batch_Year='" + dsStudDet.Rows[i]["Batch_Year"].ToString() + "' and degree_code='" + dsStudDet.Rows[i]["degree_code"].ToString() + "' and FeeCategory in(" + semcode + ") and finyearfk='" + finYearValue + "'";
                                        }
                                        ds1.Tables[0].DefaultView.RowFilter = filterset;
                                        ds1.Tables[1].DefaultView.RowFilter = filterset;
                                        DataView dvallot = ds1.Tables[1].DefaultView;
                                        dvstudcnt = ds1.Tables[0].DefaultView;
                                        Dictionary<string, string> dictcount = new Dictionary<string, string>();
                                        string deamt = "";
                                        if (dvstudcnt.Count > 0)
                                        {
                                            if (dvstudcnt.Count > 0)
                                            {
                                                deamt = Convert.ToString(dvstudcnt[0]["demand"]);
                                            }
                                            Double rate = 0;
                                            double.TryParse(Convert.ToString(deamt), out rate);
                                            //dictcount = dictcount.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

                                            save = true;
                                            checksave = true;
                                            allotamount = Convert.ToDouble(deamt);
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
                                            concessinamount = 0;
                                            double.TryParse(Convert.ToString(dvstudcnt[0]["deduction"]), out concessinamount);


                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 1].Text = deamt.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d].Text = rate.ToString();
                                            //if (rate == 0)
                                            //{
                                            //    amtver = true;
                                            //    htamt.Add(d, rate);
                                            //}
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
                                        filterset = "header_id='" + getfeecode + "' and Batch_Year='" + dsStudDet.Rows[i]["Batch_Year"].ToString() + "' and degree_code='" + dsStudDet.Rows[i]["degree_code"].ToString() + "' and FeeCategory in(" + semcode + ") and actualfinyearfk='" + finYearValue + "'";
                                        if (ddlacctype.SelectedItem.Text == "Ledger")
                                        {
                                            filterset = "Fee_code='" + getfeecode + "' and Batch_Year='" + dsStudDet.Rows[i]["Batch_Year"].ToString() + "' and degree_code='" + dsStudDet.Rows[i]["degree_code"].ToString() + "' and FeeCategory in(" + semcode + ") and actualfinyearfk='" + finYearValue + "'";
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
                                #endregion
                            }
                        }
                    }
                    rowCnt = sno;
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
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Note = Convert.ToString(FinYearfktxt);
                        for (int d = 4; d < FpSpread1.Sheets[0].Columns.Count - 5; d++)
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
                }

                #region grand total
                FpSpread1.Sheets[0].RowCount++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 3);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Note = Convert.ToString(tempFinYearfk);
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
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                #endregion


                #endregion
            }
            if (checksave == true)
            {
                FpSpread1.Visible = true;
                btnprintmaster.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                btnxl.Visible = true;
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
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            FpSpread1.Visible = true;
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    protected void getwithdueScl()
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
            UserbasedRights();
            #region get value

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
            string StudentFees = "";
            string getfeeval = "";

            FarPoint.Web.Spread.TextCellType txtcel = new FarPoint.Web.Spread.TextCellType();

            string finYearFk = string.Empty;
            string finYearFkal = string.Empty;
            for (int fk = 0; fk < chklsfyear.Items.Count; fk++)
            {
                if (chklsfyear.Items[fk].Selected)
                {
                    if (string.IsNullOrEmpty(finYearFk))
                    {
                        finYearFk = chklsfyear.Items[fk].Value;
                    }
                    else
                    {
                        finYearFk += "','" + chklsfyear.Items[fk].Value;
                    }
                }
            }
            finYearFkal = " and f.finyearfk in ('" + finYearFk + "') ";
            finYearFk = " and a.finyearfk in ('" + finYearFk + "') ";


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
            string studType = Convert.ToString(getCblSelectedValue(cbl_type));
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
            Dictionary<string, string> CurSemDeg = new Dictionary<string, string>();
            ArrayList arFeecat = new ArrayList();
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
            string fromDt = string.Empty;
            string toDt = string.Empty;
            if (cbdate.Checked)
                getDate(ref  fromDt, ref  toDt);
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
            Hashtable htcol = new Hashtable();
            string strledgre = "";
            string strledger = "";
            string gpvalue = "";
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
                                FpSpread1.Columns[FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 7].Visible = false;

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
                                ccount += 7;
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - (ccount - 1), 1, ccount - 2);
                                if (demandfee == "")
                                {
                                    demandfee = feecode;
                                    StudentFees = feecode;
                                }
                                else
                                {
                                    demandfee = demandfee + ',' + feecode;
                                    StudentFees = StudentFees + ',' + feecode;
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
                    boolColFnl = true;
                    //demandfee = " and f.LedgerFK in(" + demandfee + ")";
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
                                FpSpread1.Columns[FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 7].Visible = false;
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

                                if (demandfee == "")
                                {
                                    demandfee = feecode;
                                    gpvalue = feecode;
                                }
                                else
                                {
                                    demandfee = demandfee + ',' + feecode;
                                    gpvalue = gpvalue + ',' + feecode;
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
                    boolColFnl = true;
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
                        FpSpread1.Columns[FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 7].Visible = false;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 7].Note = chklstaccheader.Items[a].Value;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6].Text = "Amount";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6].Note = chklstaccheader.Items[a].Value;
                        htcol.Add(chklstaccheader.Items[a].Value, FpSpread1.Sheets[0].ColumnHeader.Columns.Count - 6);
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
                    demandfee = " and a.HeaderFK in(" + demandfee + ")";
                    StudentFees = " and f.HeaderFK in(" + StudentFees + ")";
                    boolColFnl = true;
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
            Hashtable ht = htcol;


            #region new

            //demand
            demquery = "select count(distinct r.roll_no) as stucount,sum(feeamount) as demand,sum(DeductAmout) as deduction,r.degree_code,r.Batch_Year,a.HeaderFK as header_id" + strledgre + ",a.FeeCategory,a.finyearfk from FT_FeeAllot a,registration r where a.App_No =r.App_No and (a.TotalAmount>0 or a.deductamout>0) " + finYearFk;
            if (commondist != "")
                demquery = demquery + commondist;
            if (!string.IsNullOrEmpty(studType))
                demquery += " and mode in('" + studType + "')";
            demquery = demquery + " " + degreequery + " " + batchquery + " " + demandfee + " " + feecatg + " group by r.degree_code,r.Batch_Year,a.HeaderFK" + strledger + ",a.FeeCategory,a.finyearfk  order by r.degree_code  ";

            //paid query
            paidquery = "select count(distinct r.roll_no) as stucount,sum(debit) as paid,r.degree_code,r.Batch_Year,a.HeaderFK as header_id" + strledgre + ",a.FeeCategory,a.actualfinyearfk   from FT_FinDailyTransaction a,registration r where  a.App_No=r.App_No and a.debit>0  and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' " + finYearFk;
            if (commondist != "")
                paidquery = paidquery + commondist;
            if (usBasedRights == true)
                paidquery += " and a.EntryUserCode in('" + usercode + "')";
            if (!string.IsNullOrEmpty(studType))
                paidquery += " and mode in('" + studType + "')";
            if (!string.IsNullOrEmpty(fromDt) && !string.IsNullOrEmpty(toDt))
                paidquery += " and a.Transdate between '" + fromDt + "' and '" + toDt + "'";
            if (chkinclude.Checked == true)
            {
                //paidquery = paidquery + " and r.DelFlag=0";
            }
            paidquery = paidquery + " " + degreequery + " " + batchquery + " " + demandfee + " " + feecatg + " group by r.degree_code,r.Batch_Year,a.HeaderFK" + strledger + ",a.FeeCategory,a.actualfinyearfk order by r.degree_code   ";

            paidquery += " select count(distinct r.roll_no) as stucount,sum(debit) as paid,r.degree_code,r.Batch_Year,a.HeaderFK as header_id" + strledgre + ",a.FeeCategory,a.actualfinyearfk   from FT_FinDailyTransaction a,registration r where  a.App_No=r.App_No and a.debit>0  and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' " + finYearFk;
            if (commondist != "")
                paidquery = paidquery + commondist;
            if (usBasedRights == true)
                paidquery += " and a.EntryUserCode in('" + usercode + "')";
            if (!string.IsNullOrEmpty(studType))
                paidquery += " and mode in('" + studType + "')";
            if (!string.IsNullOrEmpty(fromDt) && !string.IsNullOrEmpty(toDt))
                paidquery += " and a.Transdate between '" + fromDt + "' and '" + toDt + "'";
            if (chkinclude.Checked == true)
            {
                //paidquery = paidquery + " and r.DelFlag=0";
            }
            paidquery = paidquery + " " + degreequery + " " + batchquery + " " + demandfee + " " + feecatg + " group by r.degree_code,r.Batch_Year,a.HeaderFK" + strledger + ",a.FeeCategory,a.actualfinyearfk order by r.degree_code   ";


            //stud count
            string str = "select count(distinct r.roll_no) as stcount,r.Batch_Year,c.Course_Name,de.Dept_Name,r.degree_code,f.FeeCategory,f.finyearfk  from Registration r,Degree d,Department de,Course c ,FT_FeeAllot f where f.App_No =r.App_No and r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and c.Course_Id=d.Course_Id" + finYearFkal;
            if (commondist != "")
                str = str + commondist;
            if (!string.IsNullOrEmpty(studType))
                str += " and mode in('" + studType + "')";
            if (chkinclude.Checked == true)
            {
                // str = str + " and r.DelFlag=0";
            }
            str = str + " " + degreequery + " " + batchquery + " and f.FeeCategory in('" + feecatval + "') " + StudentFees + " group by r.Batch_Year,c.Course_Name,de.Dept_Name,r.degree_code,f.FeeCategory,f.finyearfk  order by r.degree_code,r.Batch_Year desc,f.FeeCategory ";

            demquery += " select distinct sum(feeamount) as demand,sum(DeductAmout) as deduction,r.degree_code,r.Batch_Year,a.HeaderFK as header_id" + strledgre + ",a.FeeCategory ,a.finyearfk   from FT_FeeAllotDegree  a,registration r where a.DegreeCode=r.degree_code and a.BatchYear=r.Batch_Year and (a.TotalAmount>0 or a.deductamout>0) " + finYearFk;
            if (commondist != "")
                demquery = demquery + commondist;
            if (!string.IsNullOrEmpty(studType))
                demquery += " and mode in('" + studType + "')";
            //and seattype='" + seat + "'

            demquery = demquery + "  " + degreequery + " " + batchquery + " " + demandfee + " " + feecatg + " group by r.degree_code,r.Batch_Year,a.HeaderFK" + strledger + ",a.FeeCategory,r.roll_no,a.finyearfk  order by r.degree_code  ";

            //amount count
            demquery += " select distinct r.roll_no as stucount,sum(feeamount) as demand,sum(DeductAmout) as deduction,r.degree_code,r.Batch_Year,a.HeaderFK as header_id" + strledgre + ",a.FeeCategory,a.finyearfk    from FT_FeeAllot a,registration r where a.App_No =r.App_No and (a.TotalAmount>0 or a.deductamout>0) " + finYearFk;
            if (commondist != "")
                demquery = demquery + commondist;
            if (!string.IsNullOrEmpty(studType))
                demquery += " and mode in('" + studType + "')";
            demquery = demquery + " " + degreequery + " " + batchquery + " " + demandfee + " " + feecatg + " group by r.degree_code,r.Batch_Year,a.HeaderFK" + strledger + ",a.FeeCategory,r.roll_no,a.finyearfk  order by r.degree_code ";

            demquery += "select count(distinct r.roll_no) as stucount,sum(feeamount) as demand,sum(DeductAmout) as deduction,r.degree_code,r.Batch_Year" + strledgre + ",a.FeeCategory ,a.finyearfk   from FT_FeeAllot a,registration r where a.App_No =r.App_No and (a.TotalAmount>0 or a.deductamout>0) " + finYearFk;
            if (commondist != "")
                demquery = demquery + commondist;
            if (!string.IsNullOrEmpty(studType))
                demquery += " and mode in('" + studType + "')";
            demquery = demquery + " " + degreequery + " " + batchquery + " " + demandfee + " " + feecatg + " group by r.degree_code,r.Batch_Year" + strledger + ",a.FeeCategory,a.finyearfk  order by r.degree_code  ";
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
            Hashtable htActYr = getFinyear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                #region
                FpSpread1.Visible = true;
                btnprintmaster.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                btnxl.Visible = true;
                DataView dvcnt = new DataView();
                DataView dvstudcnt = new DataView();
                DataView dvamt = new DataView();
                bool save = false;

                Dictionary<int, double> htamt = new Dictionary<int, double>();
                string tempFinYearfk = string.Empty;
                int rowCnt = 0;
                Hashtable htfnlTot = new Hashtable();
                for (int finYr = 0; finYr < chklsfyear.Items.Count; finYr++)
                {
                    bool boolFinCheck = false;
                    string FinYearfktxt = string.Empty;
                    ArrayList ardeg = new ArrayList();
                    ArrayList ardegree = new ArrayList();
                    if (chklsfyear.Items[finYr].Selected)
                    {
                        string finYearValue = Convert.ToString(chklsfyear.Items[finYr].Value);
                        string finYearText = Convert.ToString(chklsfyear.Items[finYr].Text);
                        ds.Tables[0].DefaultView.RowFilter = " finyearfk='" + finYearValue + "'";
                        DataTable dsStudDet = ds.Tables[0].DefaultView.ToTable();
                        if (dsStudDet.Rows.Count > 0)
                        {
                            for (int i = 0; i < dsStudDet.Rows.Count; i++)
                            {
                                #region
                                ds1.Tables[3].DefaultView.RowFilter = "Batch_year='" + Convert.ToString(dsStudDet.Rows[i]["Batch_year"]) + "' and degree_code='" + Convert.ToString(dsStudDet.Rows[i]["degree_code"]) + "' and FeeCategory='" + dsStudDet.Rows[i]["FeeCategory"] + "' and finyearfk='" + finYearValue + "'";
                                dvcnt = ds1.Tables[3].DefaultView;
                                double studcnt = 0;
                                if (dvcnt.Count > 0)
                                {
                                    string degcode = Convert.ToString(dsStudDet.Rows[i]["degree_code"]);
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
                                    if (!boolFinCheck)
                                    {
                                        FpSpread1.Sheets[0].RowCount++;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = finYearText;
                                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 5);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = Color.Green;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        boolFinCheck = true;
                                    }
                                    FpSpread1.Sheets[0].RowCount++;
                                    if (!ardeg.Contains(Convert.ToString(dsStudDet.Rows[i]["degree_code"])))
                                    {
                                        ardeg.Add(Convert.ToString(dsStudDet.Rows[i]["degree_code"]));
                                        sno = rowCnt;
                                        sno++;
                                        rowCnt++;
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(htActYr[Convert.ToString(chklsfyear.Items[finYr].Value).Trim()]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Note = Convert.ToString(dsStudDet.Rows[i]["Batch_year"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dsStudDet.Rows[i]["Course_Name"].ToString() + '-' + ds.Tables[0].Rows[i]["Dept_Name"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(dsStudDet.Rows[i]["degree_code"]);
                                    double.TryParse(Convert.ToString(dvcnt[0]["stucount"]), out studcnt);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = studcnt.ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;

                                    string TextName = "";
                                    string semcode = Convert.ToString(dsStudDet.Rows[i]["FeeCategory"]);
                                    DataView Dview = new DataView();
                                    if (dssem.Tables[0].Rows.Count > 0)
                                    {
                                        dssem.Tables[0].DefaultView.RowFilter = "TextCode=" + semcode + "";
                                        Dview = dssem.Tables[0].DefaultView;
                                        if (Dview.Count > 0)
                                            TextName = Convert.ToString(Dview[0]["TextVal"]);
                                    }

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = TextName;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(dsStudDet.Rows[i]["FeeCategory"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Note = Convert.ToString(dsStudDet.Rows[i]["finyearfk"]);
                                    if (!tempFinYearfk.Contains(Convert.ToString(dsStudDet.Rows[i]["finyearfk"])))
                                    {
                                        if (string.IsNullOrEmpty(tempFinYearfk))
                                            tempFinYearfk = Convert.ToString(dsStudDet.Rows[i]["finyearfk"]);
                                        else
                                            tempFinYearfk += "','" + Convert.ToString(dsStudDet.Rows[i]["finyearfk"]);
                                    }
                                    if (!FinYearfktxt.Contains(Convert.ToString(dsStudDet.Rows[i]["finyearfk"])))
                                    {
                                        FinYearfktxt = Convert.ToString(dsStudDet.Rows[i]["finyearfk"]);
                                    }
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
                                    double totalcnt = 0;
                                    bool amtver = false;
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

                                        string filterset = "header_id='" + getfeecode + "' and Batch_Year='" + dsStudDet.Rows[i]["Batch_Year"].ToString() + "' and degree_code='" + dsStudDet.Rows[i]["degree_code"].ToString() + "' and FeeCategory in(" + semcode + ") and finyearfk='" + finYearValue + "'";
                                        if (ddlacctype.SelectedItem.Text == "Ledger")
                                        {
                                            filterset = "Fee_code='" + getfeecode + "' and Batch_Year='" + dsStudDet.Rows[i]["Batch_Year"].ToString() + "' and degree_code='" + dsStudDet.Rows[i]["degree_code"].ToString() + "' and FeeCategory in(" + semcode + ") and finyearfk='" + finYearValue + "'";
                                        }
                                        ds1.Tables[0].DefaultView.RowFilter = filterset;
                                        ds1.Tables[1].DefaultView.RowFilter = filterset;
                                        DataView dvallot = ds1.Tables[1].DefaultView;
                                        dvstudcnt = ds1.Tables[0].DefaultView;
                                        Dictionary<string, string> dictcount = new Dictionary<string, string>();
                                        string deamt = "";
                                        if (dvstudcnt.Count > 0)
                                        {
                                            #region demand
                                            if (dvstudcnt.Count > 0)
                                            {
                                                deamt = Convert.ToString(dvstudcnt[0]["demand"]);
                                            }
                                            Double rate = 0;
                                            double.TryParse(Convert.ToString(deamt), out rate);

                                            save = true;
                                            checksave = true;
                                            allotamount = Convert.ToDouble(deamt);
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
                                            concessinamount = 0;
                                            double.TryParse(Convert.ToString(dvstudcnt[0]["deduction"]), out concessinamount);


                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, d + 1].Text = deamt.ToString();
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
                                        filterset = "header_id='" + getfeecode + "' and Batch_Year='" + dsStudDet.Rows[i]["Batch_Year"].ToString() + "' and degree_code='" + dsStudDet.Rows[i]["degree_code"].ToString() + "' and FeeCategory in(" + semcode + ") and actualfinyearfk='" + finYearValue + "'";
                                        if (ddlacctype.SelectedItem.Text == "Ledger")
                                        {
                                            filterset = "Fee_code='" + getfeecode + "' and Batch_Year='" + dsStudDet.Rows[i]["Batch_Year"].ToString() + "' and degree_code='" + dsStudDet.Rows[i]["degree_code"].ToString() + "' and FeeCategory in(" + semcode + ") and actualfinyearfk='" + finYearValue + "'";
                                        }
                                        ds2.Tables[0].DefaultView.RowFilter = filterset;
                                        DataView dvpaid = ds2.Tables[0].DefaultView;
                                        if (dvpaid.Count > 0)
                                        {
                                            #region
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
                                        double fnldueamt = 0;
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
                                        ds2.Tables[1].DefaultView.RowFilter = "header_id='" + getfeecode + "' and Batch_Year='" + dsStudDet.Rows[i]["Batch_Year"].ToString() + "' and degree_code='" + dsStudDet.Rows[i]["degree_code"].ToString() + "' and actualfinyearfk='" + finYearValue + "'";
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
                                #endregion
                            }

                        }
                    }
                    rowCnt = sno;
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
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Note = Convert.ToString(FinYearfktxt);
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
                }

                #region grand total
                FpSpread1.Sheets[0].RowCount++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 3);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Note = Convert.ToString(tempFinYearfk);
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
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                #endregion


                #endregion
            }
            if (checksave == true)
            {
                FpSpread1.Visible = true;
                btnprintmaster.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                btnxl.Visible = true;
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
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            FpSpread1.Visible = true;
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    #endregion

    protected void getDate(ref string fromDt, ref string toDt)
    {
        try
        {
            string fromdate = txt_fromdate.Text;
            string todate = txt_todate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromDt = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                toDt = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
        }
        catch { }
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
    //                string SelectQ = "select * from textvaltable where TextCriteria = 'FEECA'and (textval like '%Semester' or textval like '%Year') and textval not like '-1%' and college_code ='" + clgvalue + "' order by len(textval),textval asc";
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
    //                        string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Semester' and textval not like '-1%' and college_code ='" + clgvalue + "' order by len(textval),textval asc";
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
    //                        string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Year' and textval not like '-1%' and college_code ='" + clgvalue + "' order by len(textval),textval asc";
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

    #region spread cellclick for student details

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
        if (check == true)
        {
            if (checkSchoolSetting() == 0)
                studDetailsScl();
            else
                studDetailsClg();
        }
    }

    protected void studDetailsClg()
    {
        try
        {
            string finYearFk = string.Empty;
            for (int fk = 0; fk < chklsfyear.Items.Count; fk++)
            {
                if (chklsfyear.Items[fk].Selected)
                {
                    if (string.IsNullOrEmpty(finYearFk))
                    {
                        finYearFk = chklsfyear.Items[fk].Value;
                    }
                    else
                    {
                        finYearFk += "','" + chklsfyear.Items[fk].Value;
                    }
                }
            }
            finYearFk = " and f.finyearfk in ('" + finYearFk + "') ";

            poppergroup.Visible = true;
            DataSet dsnew = new DataSet();
            DataView dvallot = new DataView();
            DataView dvpaid = new DataView();
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
            RollAndRegSettings();
            UserbasedRights();
            string activrow = "";
            activrow = FpSpread1.Sheets[0].ActiveRow.ToString();
            string activecol = FpSpread1.Sheets[0].ActiveColumn.ToString();
            int actcol = Convert.ToInt16(activecol);
            string batch = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activrow), Convert.ToInt32(1)].Text);
            string Department = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activrow), Convert.ToInt32(2)].Text);
            string Semester = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activrow), Convert.ToInt32(3)].Text);

            string deptcode = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activrow), Convert.ToInt32(2)].Tag);
            string feecategory = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activrow), Convert.ToInt32(3)].Tag);

            batchSpan.InnerHtml = batch;
            DepartmentSpan.InnerHtml = Department;
            SemesterSpan.InnerHtml = Semester;

            FpSpread2.Sheets[0].AutoPostBack = true;
            FpSpread2.Sheets[0].SheetCorner.ColumnCount = 0;
            FpSpread2.Sheets[0].ColumnHeader.RowCount = 2;
            FpSpread2.Sheets[0].RowCount = 0;
            FpSpread2.Sheets[0].ColumnCount = 0;
            FpSpread2.CommandBar.Visible = false;

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = Color.Black;
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            FpSpread2.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(darkstyle);
            FpSpread2.Sheets[0].ColumnCount = 5;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Columns[0].Width = 50;


            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            // Fpspread1.Columns[2].Width = 200;
            FpSpread2.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);

            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Columns[2].Width = 50;
            FpSpread2.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);

            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Admission No";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Columns[3].Width = 50;
            FpSpread2.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);

            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Name";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Left;
            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
            spreadColumnVisible();


            #region group,header,ledger
            string selectquery = "";
            string StudentFees = "";
            Hashtable htid = new Hashtable();
            Hashtable grandtotal = new Hashtable();
            if (ddlacctype.SelectedItem.Text == "Ledger")
            {
                for (int remv = 0; remv < treeview_spreadfields.Nodes.Count; remv++)
                {
                    if (treeview_spreadfields.Nodes[remv].ChildNodes.Count > 0)
                    {
                        for (int child = 0; child < treeview_spreadfields.Nodes[remv].ChildNodes.Count; child++)
                        {
                            if (treeview_spreadfields.Nodes[remv].ChildNodes[child].Checked == true)
                            {
                                FpSpread2.Columns.Count++;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].Text = Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[child].Text);
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].Font.Bold = true;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].Font.Name = "Book Antiqua";
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;

                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Text = "Allot";
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Bold = true;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Name = "Book Antiqua";
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;

                                FpSpread2.Columns.Count++;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Text = "Paid";
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Bold = true;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Name = "Book Antiqua";
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;

                                FpSpread2.Columns.Count++;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Text = "Balance";
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Bold = true;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Name = "Book Antiqua";
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Columns.Count - 3, 1, 3);

                                if (StudentFees == "")
                                {
                                    StudentFees = treeview_spreadfields.Nodes[remv].ChildNodes[child].Value;
                                }
                                else
                                {

                                    StudentFees = StudentFees + ',' + treeview_spreadfields.Nodes[remv].ChildNodes[child].Value;
                                }
                            }
                        }
                    }
                }
                selectquery = " select SUM(TotalAmount) as total,r.App_No,f.LedgerFK from FT_FeeAllot f,Registration r where r.App_No =f.App_No and r.Batch_Year ='" + batch + "' and r.degree_code ='" + deptcode + "' and f.FeeCategory ='" + feecategory + "' and LedgerFK in (" + StudentFees + ")" + finYearFk + " " + commondist + " group by r.App_No,f.LedgerFK";
                selectquery = selectquery + " select SUM(Debit) as paid,r.App_No,f.LedgerFK from FT_FinDailyTransaction  f,Registration r where r.App_No =f.App_No and r.Batch_Year ='" + batch + "' and r.degree_code ='" + deptcode + "' and f.FeeCategory ='" + feecategory + "' and LedgerFK in (" + StudentFees + ") " + finYearFk + " " + commondist + " and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' ";
                if (usBasedRights == true)
                    selectquery += " and f.EntryUserCode in('" + usercode + "')";

                selectquery += " group by r.App_No,f.LedgerFK";
                selectquery = selectquery + " select r.App_No,r.Roll_No,r.Reg_No,r.Stud_Name,r.roll_admit from  Registration r where r.Batch_Year =" + batch + " and r.degree_code =" + deptcode + " " + commondist + "";
            }
            else if (ddlacctype.SelectedItem.Text == "Group Header")
            {
                for (int j = 0; j < chklstaccheader.Items.Count; j++)
                {
                    if (chklstaccheader.Items[j].Selected == true)
                    {
                        FpSpread2.Columns.Count++;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].Text = Convert.ToString(chklstaccheader.Items[j].Text);
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Text = "Allot";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread2.Columns.Count++;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Text = "Paid";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread2.Columns.Count++;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Text = "Balance";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Columns.Count - 3, 1, 3);
                        if (StudentFees == "")
                        {
                            StudentFees = chklstaccheader.Items[j].Text;
                        }
                        else
                        {

                            StudentFees = StudentFees + "','" + chklstaccheader.Items[j].Text;
                        }
                    }
                }
                selectquery = "select SUM(TotalAmount) as total,r.App_No,fs.ChlGroupHeader  from FT_FeeAllot f,Registration r,FS_ChlGroupHeaderSettings fs where fs.HeaderFK =f.HeaderFK and r.App_No =f.App_No and r.Batch_Year ='" + batch + "' and r.degree_code ='" + deptcode + "' and f.FeeCategory ='" + feecategory + "' and fs.ChlGroupHeader  in ('" + StudentFees + "') " + finYearFk + " " + commondist + "  group by r.App_No,fs.ChlGroupHeader";
                selectquery = selectquery + " select SUM(Debit) as paid,r.App_No,fs.ChlGroupHeader from FT_FinDailyTransaction  f,Registration r,FS_ChlGroupHeaderSettings fs where fs.HeaderFK =f.HeaderFK and r.App_No =f.App_No and r.Batch_Year ='" + batch + "' and r.degree_code ='" + deptcode + "' and f.FeeCategory ='" + feecategory + "' and fs.ChlGroupHeader  in ('" + StudentFees + "') " + finYearFk + " and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' " + commondist + "";
                if (usBasedRights == true)
                    selectquery += " and f.EntryUserCode in('" + usercode + "')";

                selectquery += " group by r.App_No,fs.ChlGroupHeader";
                selectquery = selectquery + " select r.App_No,r.Roll_No,r.Reg_No,r.Stud_Name from  Registration r where r.Batch_Year =" + batch + " and r.degree_code =" + deptcode + "  " + commondist + "";
            }
            else if (ddlacctype.SelectedItem.Text == "Header")
            {
                for (int j = 0; j < chklstaccheader.Items.Count; j++)
                {
                    int col = 1;
                    if (chklstaccheader.Items[j].Selected == true)
                    {

                        FpSpread2.Columns.Count++;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].Text = Convert.ToString(chklstaccheader.Items[j].Text);
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(chklstaccheader.Items[j].Value);
                        //   htid.Add(Convert.ToString(chklstaccheader.Items[j].Value), FpSpread2.Columns.Count - 1);
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Text = "Allot";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread2.Columns.Count++;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Text = "Paid";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread2.Columns.Count++;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Text = "Balance";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Columns.Count - 3, 1, 3);
                        if (StudentFees == "")
                        {
                            StudentFees = chklstaccheader.Items[j].Value;
                        }
                        else
                        {

                            StudentFees = StudentFees + ',' + chklstaccheader.Items[j].Value;
                        }
                    }
                }

            #endregion

                selectquery = " select SUM(TotalAmount) as total,r.App_No,f.HeaderFK from FT_FeeAllot f,Registration r where r.App_No =f.App_No and r.Batch_Year ='" + batch + "' and r.degree_code ='" + deptcode + "' and f.FeeCategory ='" + feecategory + "' and HeaderFK in (" + StudentFees + ") " + finYearFk + " " + commondist + " group by r.App_No,f.HeaderFK";
                selectquery = selectquery + " select SUM(Debit) as paid,r.App_No,f.HeaderFK from FT_FinDailyTransaction  f,Registration r where r.App_No =f.App_No and r.Batch_Year ='" + batch + "' and r.degree_code ='" + deptcode + "' and f.FeeCategory ='" + feecategory + "' and HeaderFK in (" + StudentFees + ") " + finYearFk + " and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' " + commondist + "";
                if (usBasedRights == true)
                    selectquery += " and f.EntryUserCode in('" + usercode + "')";
                selectquery += " group by r.App_No,f.HeaderFK";
                selectquery = selectquery + " select r.App_No,r.Roll_No,r.roll_admit,r.Reg_No,r.Stud_Name from  Registration r where r.Batch_Year =" + batch + " and r.degree_code =" + deptcode + " " + commondist + "";

            }
            if (selectquery.Trim() != "")
            {
                dsnew = da.select_method_wo_parameter(selectquery, "Text");
                if (dsnew.Tables.Count > 0 && dsnew.Tables[0].Rows.Count > 0)
                {
                    for (int sel = 0; sel < dsnew.Tables[2].Rows.Count; sel++)
                    {
                        FpSpread2.Sheets[0].RowCount++;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sel + 1);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dsnew.Tables[2].Rows[sel]["Roll_No"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dsnew.Tables[2].Rows[sel]["Reg_No"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dsnew.Tables[2].Rows[sel]["roll_admit"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dsnew.Tables[2].Rows[sel]["Stud_Name"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        if (dsnew.Tables[0].Rows.Count > 0 && dsnew.Tables[1].Rows.Count > 0)
                        {
                            int col = 4;
                            double allot = 0;
                            double paid = 0;
                            if (ddlacctype.SelectedItem.Text == "Header")
                            {
                                #region header

                                for (int j = 0; j < chklstaccheader.Items.Count; j++)
                                {
                                    if (chklstaccheader.Items[j].Selected == true)
                                    {
                                        col++;
                                        dsnew.Tables[0].DefaultView.RowFilter = "App_No='" + Convert.ToString(dsnew.Tables[2].Rows[sel]["App_No"]) + "' and HeaderFK='" + Convert.ToString(chklstaccheader.Items[j].Value) + "'";
                                        dvallot = dsnew.Tables[0].DefaultView;
                                        // int colnew = Convert.ToInt32(htid[Convert.ToString(chklstaccheader.Items[j].Value)]);
                                        if (dvallot.Count > 0)
                                            double.TryParse(Convert.ToString(dvallot[0]["total"]), out allot);

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].Text = Convert.ToString(allot);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;

                                        //allot
                                        if (!grandtotal.ContainsKey(col))
                                            grandtotal.Add(col, Convert.ToString(allot));
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(grandtotal[col]), out amount);
                                            amount += allot;
                                            grandtotal.Remove(col);
                                            grandtotal.Add(col, Convert.ToString(amount));
                                        }

                                        col++;
                                        dsnew.Tables[1].DefaultView.RowFilter = "App_No='" + Convert.ToString(dsnew.Tables[2].Rows[sel]["App_No"]) + "' and HeaderFK='" + Convert.ToString(chklstaccheader.Items[j].Value) + "'";
                                        dvpaid = dsnew.Tables[1].DefaultView;
                                        if (dvpaid.Count > 0)
                                            double.TryParse(Convert.ToString(dvpaid[0]["paid"]), out paid);

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].Text = Convert.ToString(paid);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;

                                        //Paid
                                        if (!grandtotal.ContainsKey(col))
                                            grandtotal.Add(col, Convert.ToString(paid));
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(grandtotal[col]), out amount);
                                            amount += paid;
                                            grandtotal.Remove(col);
                                            grandtotal.Add(col, Convert.ToString(amount));
                                        }

                                        //balance
                                        col++;
                                        double total = 0;
                                        total = allot - paid;

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].Text = Convert.ToString(total);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;

                                        //balance
                                        if (!grandtotal.ContainsKey(col))
                                            grandtotal.Add(col, Convert.ToString(total));
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(grandtotal[col]), out amount);
                                            amount += total;
                                            grandtotal.Remove(col);
                                            grandtotal.Add(col, Convert.ToString(amount));
                                        }
                                        allot = 0;
                                        paid = 0;
                                    }
                                }

                                #endregion
                            }
                            else if (ddlacctype.SelectedItem.Text == "Ledger")
                            {
                                #region Ledger

                                string[] splledg = StudentFees.Split(',');

                                for (int j = 0; j <= splledg.GetUpperBound(0); j++)
                                {
                                    if (splledg[j].Trim() != "")
                                    {
                                        col++;
                                        dsnew.Tables[0].DefaultView.RowFilter = "App_No='" + Convert.ToString(dsnew.Tables[2].Rows[sel]["App_No"]) + "' and LedgerFK='" + Convert.ToString(splledg[j]) + "'";
                                        dvallot = dsnew.Tables[0].DefaultView;
                                        // int colnew = Convert.ToInt32(htid[Convert.ToString(chklstaccheader.Items[j].Value)]);
                                        if (dvallot.Count > 0)
                                            double.TryParse(Convert.ToString(dvallot[0]["total"]), out allot);

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].Text = Convert.ToString(allot);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;

                                        //allot
                                        if (!grandtotal.ContainsKey(col))
                                            grandtotal.Add(col, Convert.ToString(allot));
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(grandtotal[col]), out amount);
                                            amount += allot;
                                            grandtotal.Remove(col);
                                            grandtotal.Add(col, Convert.ToString(amount));
                                        }

                                        col++;
                                        dsnew.Tables[1].DefaultView.RowFilter = "App_No='" + Convert.ToString(dsnew.Tables[2].Rows[sel]["App_No"]) + "' and LedgerFK='" + Convert.ToString(splledg[j]) + "'";
                                        dvpaid = dsnew.Tables[1].DefaultView;
                                        if (dvpaid.Count > 0)
                                            double.TryParse(Convert.ToString(dvpaid[0]["paid"]), out paid);

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].Text = Convert.ToString(paid);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;

                                        //Paid
                                        if (!grandtotal.ContainsKey(col))
                                            grandtotal.Add(col, Convert.ToString(paid));
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(grandtotal[col]), out amount);
                                            amount += paid;
                                            grandtotal.Remove(col);
                                            grandtotal.Add(col, Convert.ToString(amount));
                                        }

                                        //balance
                                        col++;
                                        double total = 0;
                                        total = allot - paid;

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].Text = Convert.ToString(total);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;

                                        //balance
                                        if (!grandtotal.ContainsKey(col))
                                            grandtotal.Add(col, Convert.ToString(total));
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(grandtotal[col]), out amount);
                                            amount += total;
                                            grandtotal.Remove(col);
                                            grandtotal.Add(col, Convert.ToString(amount));
                                        }
                                        allot = 0;
                                        paid = 0;
                                    }
                                }

                                #endregion
                            }
                            else if (ddlacctype.SelectedItem.Text == "Group Header")
                            {
                                #region Group header

                                for (int j = 0; j < chklstaccheader.Items.Count; j++)
                                {
                                    if (chklstaccheader.Items[j].Selected == true)
                                    {
                                        col++;
                                        dsnew.Tables[0].DefaultView.RowFilter = "App_No='" + Convert.ToString(dsnew.Tables[2].Rows[sel]["App_No"]) + "' and ChlGroupHeader='" + Convert.ToString(chklstaccheader.Items[j].Text) + "'";
                                        dvallot = dsnew.Tables[0].DefaultView;
                                        // int colnew = Convert.ToInt32(htid[Convert.ToString(chklstaccheader.Items[j].Value)]);
                                        if (dvallot.Count > 0)
                                            double.TryParse(Convert.ToString(dvallot[0]["total"]), out allot);

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].Text = Convert.ToString(allot);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;

                                        //allot
                                        if (!grandtotal.ContainsKey(col))
                                            grandtotal.Add(col, Convert.ToString(allot));
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(grandtotal[col]), out amount);
                                            amount += allot;
                                            grandtotal.Remove(col);
                                            grandtotal.Add(col, Convert.ToString(amount));
                                        }

                                        col++;
                                        dsnew.Tables[1].DefaultView.RowFilter = "App_No='" + Convert.ToString(dsnew.Tables[2].Rows[sel]["App_No"]) + "' and ChlGroupHeader='" + Convert.ToString(chklstaccheader.Items[j].Text) + "'";
                                        dvpaid = dsnew.Tables[1].DefaultView;
                                        if (dvpaid.Count > 0)
                                            double.TryParse(Convert.ToString(dvpaid[0]["paid"]), out paid);

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].Text = Convert.ToString(paid);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;

                                        //Paid
                                        if (!grandtotal.ContainsKey(col))
                                            grandtotal.Add(col, Convert.ToString(paid));
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(grandtotal[col]), out amount);
                                            amount += paid;
                                            grandtotal.Remove(col);
                                            grandtotal.Add(col, Convert.ToString(amount));
                                        }

                                        //balance
                                        col++;
                                        double total = 0;
                                        total = allot - paid;

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].Text = Convert.ToString(total);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;

                                        //balance
                                        if (!grandtotal.ContainsKey(col))
                                            grandtotal.Add(col, Convert.ToString(total));
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(grandtotal[col]), out amount);
                                            amount += total;
                                            grandtotal.Remove(col);
                                            grandtotal.Add(col, Convert.ToString(amount));
                                        }
                                        allot = 0;
                                        paid = 0;
                                    }
                                }

                                #endregion
                            }
                        }
                    }
                    #region grandtotal
                    FpSpread2.Sheets[0].PageSize = dsnew.Tables[2].Rows.Count + 1;
                    FpSpread2.Sheets[0].Rows.Count++;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                    FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].Rows.Count - 1, 0, 1, 3);
                    FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
                    double grandvalue = 0;
                    for (int j = 4; j < FpSpread2.Sheets[0].ColumnCount; j++)
                    {
                        double.TryParse(Convert.ToString(grandtotal[j]), out grandvalue);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalue);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, j].HorizontalAlign = HorizontalAlign.Right;
                    }
                    #endregion

                    FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                    FpSpread2.SaveChanges();
                }
            }
            
        }
        catch { }
    }

    protected void studDetailsScl()
    {
        try
        {
            string finYearFk = string.Empty;
            poppergroup.Visible = true;
            DataSet dsnew = new DataSet();
            DataView dvallot = new DataView();
            DataView dvpaid = new DataView();

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
            RollAndRegSettings();
            string activrow = "";
            activrow = FpSpread1.Sheets[0].ActiveRow.ToString();
            string activecol = FpSpread1.Sheets[0].ActiveColumn.ToString();
            int actcol = Convert.ToInt16(activecol);
            string batch = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activrow), 1].Note);
            string Department = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activrow), 2].Text);
            string Semester = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activrow), 3].Text);

            string deptcode = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activrow), 2].Tag);
            string feecategory = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activrow), 3].Tag);
            finYearFk = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activrow), 3].Note);
            finYearFk = " and f.finyearfk in ('" + finYearFk + "') ";
            batchSpan.InnerHtml = batch;
            DepartmentSpan.InnerHtml = Department;
            SemesterSpan.InnerHtml = Semester;

            FpSpread2.Sheets[0].AutoPostBack = true;
            FpSpread2.Sheets[0].SheetCorner.ColumnCount = 0;
            FpSpread2.Sheets[0].ColumnHeader.RowCount = 2;
            FpSpread2.Sheets[0].RowCount = 0;
            FpSpread2.Sheets[0].ColumnCount = 0;
            FpSpread2.CommandBar.Visible = false;

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = Color.Black;
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            FpSpread2.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(darkstyle);
            FpSpread2.Sheets[0].ColumnCount = 5;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Columns[0].Width = 50;


            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            // Fpspread1.Columns[2].Width = 200;
            FpSpread2.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);

            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Columns[2].Width = 50;
            FpSpread2.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);

            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Admission No";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Columns[3].Width = 50;
            FpSpread2.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);

            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Name";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Left;
            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
            spreadColumnVisible();


            #region group,header,ledger
            string selectquery = "";
            string StudentFees = "";
            Hashtable htid = new Hashtable();
            Hashtable grandtotal = new Hashtable();
            if (ddlacctype.SelectedItem.Text == "Ledger")
            {
                #region leger
                for (int remv = 0; remv < treeview_spreadfields.Nodes.Count; remv++)
                {
                    if (treeview_spreadfields.Nodes[remv].ChildNodes.Count > 0)
                    {
                        for (int child = 0; child < treeview_spreadfields.Nodes[remv].ChildNodes.Count; child++)
                        {
                            if (treeview_spreadfields.Nodes[remv].ChildNodes[child].Checked == true)
                            {
                                FpSpread2.Columns.Count++;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].Text = Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[child].Text);
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].Font.Bold = true;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].Font.Name = "Book Antiqua";
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;

                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Text = "Allot";
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Bold = true;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Name = "Book Antiqua";
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;

                                FpSpread2.Columns.Count++;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Text = "Paid";
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Bold = true;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Name = "Book Antiqua";
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;

                                FpSpread2.Columns.Count++;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Text = "Balance";
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Bold = true;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Name = "Book Antiqua";
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Columns.Count - 3, 1, 3);

                                if (StudentFees == "")
                                {
                                    StudentFees = treeview_spreadfields.Nodes[remv].ChildNodes[child].Value;
                                }
                                else
                                {

                                    StudentFees = StudentFees + ',' + treeview_spreadfields.Nodes[remv].ChildNodes[child].Value;
                                }
                            }
                        }
                    }
                }
                selectquery = " select SUM(TotalAmount) as total,r.App_No,f.LedgerFK from FT_FeeAllot f,Registration r where r.App_No =f.App_No and r.Batch_Year ='" + batch + "' and r.degree_code ='" + deptcode + "' and f.FeeCategory ='" + feecategory + "' and LedgerFK in (" + StudentFees + ")" + finYearFk + " " + commondist + " group by r.App_No,f.LedgerFK";
                selectquery = selectquery + " select SUM(Debit) as paid,r.App_No,f.LedgerFK from FT_FinDailyTransaction  f,Registration r where r.App_No =f.App_No and r.Batch_Year ='" + batch + "' and r.degree_code ='" + deptcode + "' and f.FeeCategory ='" + feecategory + "' and LedgerFK in (" + StudentFees + ") " + finYearFk + " " + commondist + " and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' ";
                if (usBasedRights == true)
                    selectquery += " and f.EntryUserCode in('" + usercode + "')";

                selectquery += " group by r.App_No,f.LedgerFK";
                selectquery = selectquery + " select r.App_No,r.Roll_No,r.Reg_No,r.Stud_Name from  Registration r where r.Batch_Year =" + batch + " and r.degree_code =" + deptcode + " " + commondist + "";
                #endregion
            }
            else if (ddlacctype.SelectedItem.Text == "Group Header")
            {
                #region group header
                for (int j = 0; j < chklstaccheader.Items.Count; j++)
                {
                    if (chklstaccheader.Items[j].Selected == true)
                    {
                        FpSpread2.Columns.Count++;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].Text = Convert.ToString(chklstaccheader.Items[j].Text);
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Text = "Allot";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread2.Columns.Count++;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Text = "Paid";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread2.Columns.Count++;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Text = "Balance";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Columns.Count - 3, 1, 3);
                        if (StudentFees == "")
                        {
                            StudentFees = chklstaccheader.Items[j].Text;
                        }
                        else
                        {

                            StudentFees = StudentFees + "','" + chklstaccheader.Items[j].Text;
                        }
                    }
                }
                selectquery = "select SUM(TotalAmount) as total,r.App_No,fs.ChlGroupHeader  from FT_FeeAllot f,Registration r,FS_ChlGroupHeaderSettings fs where fs.HeaderFK =f.HeaderFK and r.App_No =f.App_No and r.Batch_Year ='" + batch + "' and r.degree_code ='" + deptcode + "' and f.FeeCategory ='" + feecategory + "' and fs.ChlGroupHeader  in ('" + StudentFees + "') " + finYearFk + " " + commondist + "  group by r.App_No,fs.ChlGroupHeader";
                selectquery = selectquery + " select SUM(Debit) as paid,r.App_No,fs.ChlGroupHeader from FT_FinDailyTransaction  f,Registration r,FS_ChlGroupHeaderSettings fs where fs.HeaderFK =f.HeaderFK and r.App_No =f.App_No and r.Batch_Year ='" + batch + "' and r.degree_code ='" + deptcode + "' and f.FeeCategory ='" + feecategory + "' and fs.ChlGroupHeader  in ('" + StudentFees + "') " + finYearFk + " and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' " + commondist + "";
                if (usBasedRights == true)
                    selectquery += " and f.EntryUserCode in('" + usercode + "')";

                selectquery += " group by r.App_No,fs.ChlGroupHeader";
                selectquery = selectquery + " select r.App_No,r.Roll_No,r.Reg_No,r.Stud_Name from  Registration r where r.Batch_Year =" + batch + " and r.degree_code =" + deptcode + "  " + commondist + "";
                #endregion
            }
            else if (ddlacctype.SelectedItem.Text == "Header")
            {
                #region header
                for (int j = 0; j < chklstaccheader.Items.Count; j++)
                {
                    int col = 1;
                    if (chklstaccheader.Items[j].Selected == true)
                    {

                        FpSpread2.Columns.Count++;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].Text = Convert.ToString(chklstaccheader.Items[j].Text);
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(chklstaccheader.Items[j].Value);
                        //   htid.Add(Convert.ToString(chklstaccheader.Items[j].Value), FpSpread2.Columns.Count - 1);
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Text = "Allot";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread2.Columns.Count++;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Text = "Paid";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread2.Columns.Count++;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Text = "Balance";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Columns.Count - 3, 1, 3);
                        if (StudentFees == "")
                        {
                            StudentFees = chklstaccheader.Items[j].Value;
                        }
                        else
                        {

                            StudentFees = StudentFees + ',' + chklstaccheader.Items[j].Value;
                        }
                    }
                }



                selectquery = " select SUM(TotalAmount) as total,r.App_No,f.HeaderFK from FT_FeeAllot f,Registration r where r.App_No =f.App_No and r.Batch_Year ='" + batch + "' and r.degree_code ='" + deptcode + "' and f.FeeCategory ='" + feecategory + "' and HeaderFK in (" + StudentFees + ") " + finYearFk + " " + commondist + " group by r.App_No,f.HeaderFK";
                selectquery = selectquery + " select SUM(Debit) as paid,r.App_No,f.HeaderFK from FT_FinDailyTransaction  f,Registration r where r.App_No =f.App_No and r.Batch_Year ='" + batch + "' and r.degree_code ='" + deptcode + "' and f.FeeCategory ='" + feecategory + "' and HeaderFK in (" + StudentFees + ") " + finYearFk + " and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' " + commondist + "";
                if (usBasedRights == true)
                    selectquery += " and f.EntryUserCode in('" + usercode + "')";
                selectquery += " group by r.App_No,f.HeaderFK";
                selectquery = selectquery + " select r.App_No,r.Roll_No,r.roll_admit,r.Reg_No,r.Stud_Name from  Registration r where r.Batch_Year =" + batch + " and r.degree_code =" + deptcode + " " + commondist + "";
                #endregion
            }
            #endregion

            if (selectquery.Trim() != "")
            {
                dsnew = da.select_method_wo_parameter(selectquery, "Text");
                if (dsnew.Tables.Count > 0 && dsnew.Tables[0].Rows.Count > 0)
                {
                    for (int sel = 0; sel < dsnew.Tables[2].Rows.Count; sel++)
                    {
                        FpSpread2.Sheets[0].RowCount++;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sel + 1);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dsnew.Tables[2].Rows[sel]["Roll_No"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dsnew.Tables[2].Rows[sel]["Reg_No"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dsnew.Tables[2].Rows[sel]["roll_admit"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dsnew.Tables[2].Rows[sel]["Stud_Name"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        if (dsnew.Tables[0].Rows.Count > 0 && dsnew.Tables[1].Rows.Count > 0)
                        {
                            int col = 4;
                            double allot = 0;
                            double paid = 0;
                            if (ddlacctype.SelectedItem.Text == "Header")
                            {
                                #region header

                                for (int j = 0; j < chklstaccheader.Items.Count; j++)
                                {
                                    if (chklstaccheader.Items[j].Selected == true)
                                    {
                                        col++;
                                        dsnew.Tables[0].DefaultView.RowFilter = "App_No='" + Convert.ToString(dsnew.Tables[2].Rows[sel]["App_No"]) + "' and HeaderFK='" + Convert.ToString(chklstaccheader.Items[j].Value) + "'";
                                        dvallot = dsnew.Tables[0].DefaultView;
                                        // int colnew = Convert.ToInt32(htid[Convert.ToString(chklstaccheader.Items[j].Value)]);
                                        if (dvallot.Count > 0)
                                            double.TryParse(Convert.ToString(dvallot[0]["total"]), out allot);

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].Text = Convert.ToString(allot);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;

                                        //allot
                                        if (!grandtotal.ContainsKey(col))
                                            grandtotal.Add(col, Convert.ToString(allot));
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(grandtotal[col]), out amount);
                                            amount += allot;
                                            grandtotal.Remove(col);
                                            grandtotal.Add(col, Convert.ToString(amount));
                                        }

                                        col++;
                                        dsnew.Tables[1].DefaultView.RowFilter = "App_No='" + Convert.ToString(dsnew.Tables[2].Rows[sel]["App_No"]) + "' and HeaderFK='" + Convert.ToString(chklstaccheader.Items[j].Value) + "'";
                                        dvpaid = dsnew.Tables[1].DefaultView;
                                        if (dvpaid.Count > 0)
                                            double.TryParse(Convert.ToString(dvpaid[0]["paid"]), out paid);

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].Text = Convert.ToString(paid);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;

                                        //Paid
                                        if (!grandtotal.ContainsKey(col))
                                            grandtotal.Add(col, Convert.ToString(paid));
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(grandtotal[col]), out amount);
                                            amount += paid;
                                            grandtotal.Remove(col);
                                            grandtotal.Add(col, Convert.ToString(amount));
                                        }

                                        //balance
                                        col++;
                                        double total = 0;
                                        total = allot - paid;

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].Text = Convert.ToString(total);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;

                                        //balance
                                        if (!grandtotal.ContainsKey(col))
                                            grandtotal.Add(col, Convert.ToString(total));
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(grandtotal[col]), out amount);
                                            amount += total;
                                            grandtotal.Remove(col);
                                            grandtotal.Add(col, Convert.ToString(amount));
                                        }
                                        allot = 0;
                                        paid = 0;
                                    }
                                }

                                #endregion
                            }
                            else if (ddlacctype.SelectedItem.Text == "Ledger")
                            {
                                #region Ledger

                                string[] splledg = StudentFees.Split(',');

                                for (int j = 0; j <= splledg.GetUpperBound(0); j++)
                                {
                                    if (splledg[j].Trim() != "")
                                    {
                                        col++;
                                        dsnew.Tables[0].DefaultView.RowFilter = "App_No='" + Convert.ToString(dsnew.Tables[2].Rows[sel]["App_No"]) + "' and LedgerFK='" + Convert.ToString(splledg[j]) + "'";
                                        dvallot = dsnew.Tables[0].DefaultView;
                                        // int colnew = Convert.ToInt32(htid[Convert.ToString(chklstaccheader.Items[j].Value)]);
                                        if (dvallot.Count > 0)
                                            double.TryParse(Convert.ToString(dvallot[0]["total"]), out allot);

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].Text = Convert.ToString(allot);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;

                                        //allot
                                        if (!grandtotal.ContainsKey(col))
                                            grandtotal.Add(col, Convert.ToString(allot));
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(grandtotal[col]), out amount);
                                            amount += allot;
                                            grandtotal.Remove(col);
                                            grandtotal.Add(col, Convert.ToString(amount));
                                        }

                                        col++;
                                        dsnew.Tables[1].DefaultView.RowFilter = "App_No='" + Convert.ToString(dsnew.Tables[2].Rows[sel]["App_No"]) + "' and LedgerFK='" + Convert.ToString(splledg[j]) + "'";
                                        dvpaid = dsnew.Tables[1].DefaultView;
                                        if (dvpaid.Count > 0)
                                            double.TryParse(Convert.ToString(dvpaid[0]["paid"]), out paid);

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].Text = Convert.ToString(paid);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;

                                        //Paid
                                        if (!grandtotal.ContainsKey(col))
                                            grandtotal.Add(col, Convert.ToString(paid));
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(grandtotal[col]), out amount);
                                            amount += paid;
                                            grandtotal.Remove(col);
                                            grandtotal.Add(col, Convert.ToString(amount));
                                        }

                                        //balance
                                        col++;
                                        double total = 0;
                                        total = allot - paid;

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].Text = Convert.ToString(total);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;

                                        //balance
                                        if (!grandtotal.ContainsKey(col))
                                            grandtotal.Add(col, Convert.ToString(total));
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(grandtotal[col]), out amount);
                                            amount += total;
                                            grandtotal.Remove(col);
                                            grandtotal.Add(col, Convert.ToString(amount));
                                        }
                                        allot = 0;
                                        paid = 0;
                                    }
                                }

                                #endregion
                            }
                            else if (ddlacctype.SelectedItem.Text == "Group Header")
                            {
                                #region Group header

                                for (int j = 0; j < chklstaccheader.Items.Count; j++)
                                {
                                    if (chklstaccheader.Items[j].Selected == true)
                                    {
                                        col++;
                                        dsnew.Tables[0].DefaultView.RowFilter = "App_No='" + Convert.ToString(dsnew.Tables[2].Rows[sel]["App_No"]) + "' and ChlGroupHeader='" + Convert.ToString(chklstaccheader.Items[j].Text) + "'";
                                        dvallot = dsnew.Tables[0].DefaultView;
                                        // int colnew = Convert.ToInt32(htid[Convert.ToString(chklstaccheader.Items[j].Value)]);
                                        if (dvallot.Count > 0)
                                            double.TryParse(Convert.ToString(dvallot[0]["total"]), out allot);

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].Text = Convert.ToString(allot);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;

                                        //allot
                                        if (!grandtotal.ContainsKey(col))
                                            grandtotal.Add(col, Convert.ToString(allot));
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(grandtotal[col]), out amount);
                                            amount += allot;
                                            grandtotal.Remove(col);
                                            grandtotal.Add(col, Convert.ToString(amount));
                                        }

                                        col++;
                                        dsnew.Tables[1].DefaultView.RowFilter = "App_No='" + Convert.ToString(dsnew.Tables[2].Rows[sel]["App_No"]) + "' and ChlGroupHeader='" + Convert.ToString(chklstaccheader.Items[j].Text) + "'";
                                        dvpaid = dsnew.Tables[1].DefaultView;
                                        if (dvpaid.Count > 0)
                                            double.TryParse(Convert.ToString(dvpaid[0]["paid"]), out paid);

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].Text = Convert.ToString(paid);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;

                                        //Paid
                                        if (!grandtotal.ContainsKey(col))
                                            grandtotal.Add(col, Convert.ToString(paid));
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(grandtotal[col]), out amount);
                                            amount += paid;
                                            grandtotal.Remove(col);
                                            grandtotal.Add(col, Convert.ToString(amount));
                                        }

                                        //balance
                                        col++;
                                        double total = 0;
                                        total = allot - paid;

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].Text = Convert.ToString(total);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Right;

                                        //balance
                                        if (!grandtotal.ContainsKey(col))
                                            grandtotal.Add(col, Convert.ToString(total));
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(grandtotal[col]), out amount);
                                            amount += total;
                                            grandtotal.Remove(col);
                                            grandtotal.Add(col, Convert.ToString(amount));
                                        }
                                        allot = 0;
                                        paid = 0;
                                    }
                                }

                                #endregion
                            }
                        }
                    }
                    #region grandtotal
                    FpSpread2.Sheets[0].PageSize = dsnew.Tables[2].Rows.Count + 1;
                    FpSpread2.Sheets[0].Rows.Count++;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                    FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].Rows.Count - 1, 0, 1, 3);
                    FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
                    double grandvalue = 0;
                    for (int j = 4; j < FpSpread2.Sheets[0].ColumnCount; j++)
                    {
                        double.TryParse(Convert.ToString(grandtotal[j]), out grandvalue);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalue);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, j].HorizontalAlign = HorizontalAlign.Right;
                    }
                    #endregion

                    FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                    FpSpread2.SaveChanges();
                }
            }
        }
        catch { }
    }

    #endregion

    protected void imagebtnpopclose5_Click(object sender, EventArgs e)
    {
        try
        {
            poppergroup.Visible = false;
        }
        catch
        {

        }
    }



    protected void btnsncprint_Click(object sender, EventArgs e)
    {
        Printmaster1.Visible = true;
        string degreedetails = string.Empty;


        degreedetails = " Course Wise Student's Fee Report ";
        string pagename = "Overall_student_Fee_Status.aspx";
        Printmaster1.loadspreaddetails(FpSpread2, pagename, degreedetails);
    }
    protected void btnsndexcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtsndrpt.Text;
            if (reportname.ToString().Trim() != "")
            {
                da.printexcelreport(FpSpread2, reportname);
            }
            else
            {
                Label2.Text = "Please Enter Your Report Name";
                Label2.Visible = true;
            }
        }
        catch
        {
        }
    }

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

        lbl.Add(lblstr);
        lbl.Add(lbldeg);
        lbl.Add(lbldept);
        lbl.Add(lblsem);
        fields.Add(1);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);
        lbl.Add(lbldepts);
        lbl.Add(lblsems);
        fields.Add(3);
        fields.Add(4);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

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



    protected void spreadColumnVisible()
    {
        try
        {
            if (roll == 0)
            {
                FpSpread2.Columns[1].Visible = true;
                FpSpread2.Columns[2].Visible = true;
                FpSpread2.Columns[3].Visible = true;
            }
            else if (roll == 1)
            {
                FpSpread2.Columns[1].Visible = true;
                FpSpread2.Columns[2].Visible = true;
                FpSpread2.Columns[3].Visible = true;
            }
            else if (roll == 2)
            {
                FpSpread2.Columns[1].Visible = true;
                FpSpread2.Columns[2].Visible = false;
                FpSpread2.Columns[3].Visible = false;

            }
            else if (roll == 3)
            {
                FpSpread2.Columns[1].Visible = false;
                FpSpread2.Columns[2].Visible = true;
                FpSpread2.Columns[3].Visible = false;
            }
            else if (roll == 4)
            {
                FpSpread2.Columns[1].Visible = false;
                FpSpread2.Columns[2].Visible = false;
                FpSpread2.Columns[3].Visible = true;
            }
            else if (roll == 5)
            {
                FpSpread2.Columns[1].Visible = true;
                FpSpread2.Columns[2].Visible = true;
                FpSpread2.Columns[3].Visible = false;
            }
            else if (roll == 6)
            {
                FpSpread2.Columns[1].Visible = false;
                FpSpread2.Columns[2].Visible = true;
                FpSpread2.Columns[3].Visible = true;
            }
            else if (roll == 7)
            {
                FpSpread2.Columns[1].Visible = true;
                FpSpread2.Columns[2].Visible = false;
                FpSpread2.Columns[3].Visible = true;
            }
        }
        catch { }
    }

    #endregion

    // last modified 04.07.2017 sudhagar
    private double checkSchoolSetting()
    {
        double getVal = 0;
        double.TryParse(Convert.ToString(da.GetFunction("select  value from Master_Settings where settings='schoolorcollege' and usercode='" + usercode + "'")), out getVal);
        return getVal;
    }
    protected Hashtable getFinyear()
    {
        Hashtable htfin = new Hashtable();
        try
        {
            string SelQ = "  select (convert(varchar(10),datepart(year,finyearstart))+'-'+convert(varchar(10),datepart(year,finyearend)))as finyear,finyearpk,collegecode from fm_finyearmaster where collegecode='" + collegecode + "'";
            DataSet dsval = da.select_method_wo_parameter(SelQ, "Text");
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
            ds = da.select_method_wo_parameter(deptquery, "Text");
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
                cbl_type.Items.Add(new System.Web.UI.WebControls.ListItem("Old ", "1"));
                cbl_type.Items.Add(new System.Web.UI.WebControls.ListItem("New ", "3"));
                // cbl_type.Items.Add(new ListItem("Transfer", "2"));
                if (cbl_type.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_type.Items.Count; i++)
                    {
                        cbl_type.Items[i].Selected = true;
                    }
                    cb_type.Checked = true;
                    txt_type.Text = "Type(" + cbl_type.Items.Count + ")";
                    txt_type.Enabled = true;
                }
            }
            else
            {
                cbl_type.Items.Add(new System.Web.UI.WebControls.ListItem("Regular", "1"));
                cbl_type.Items.Add(new System.Web.UI.WebControls.ListItem("Lateral", "3"));
                cbl_type.Items.Add(new System.Web.UI.WebControls.ListItem("Transfer", "2"));
                cbl_type.Items.Add(new System.Web.UI.WebControls.ListItem("IrRegular", "4"));
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