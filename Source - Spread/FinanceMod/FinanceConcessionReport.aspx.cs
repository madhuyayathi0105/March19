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

public partial class FinanceConcessionReport : System.Web.UI.Page
{
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    string group_user = "", singleuser = "", usercode = "", collegecode = "", group_code = "";
    int count = 0;
    Hashtable has = new Hashtable();
    static TreeNode node;
    TreeNode subchildnode;

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
            errmsg.Visible = false;
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                clear();
                setLabelText();
                //txtfromdate.Attributes.Add("readonly", "readonly");
                //txttodate.Attributes.Add("readonly", "readonly");
                treeview_spreadfields.Attributes.Add("onclick", "OnCheckBoxCheckChanged(event)");

                //txtfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                string group_code = Session["group_code"].ToString();
                string columnfield = "";
                if (group_code.Contains(';'))
                {
                    string[] group_semi = group_code.Split(';');
                    group_code = group_semi[0].ToString();
                }
                if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
                {
                    columnfield = " and group_code='" + group_code + "'";
                }
                else
                {
                    columnfield = " and user_code='" + Session["usercode"] + "'";
                }
                has.Clear();
                has.Add("column_field", columnfield.ToString());
                ds = da.select_method("bind_college", has, "sp");
                ddlcollege.Items.Clear();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlcollege.Enabled = true;
                    ddlcollege.DataSource = ds;
                    ddlcollege.DataTextField = "collname";
                    ddlcollege.DataValueField = "college_code";
                    ddlcollege.DataBind();
                    if (ddlcollege.Items.Count > 0)
                        collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
                    loadtype();
                    bindbatch();
                    binddegree();
                    bindbranch();
                    loadfinanceyear();
                    loadheader();
                    loadledger();
                    loadconcession();
                    loadfeecategory();
                }
            }
            if (ddlcollege.Items.Count > 0)
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
        }
        catch (Exception ex) { }
    }
    public void clear()
    {
        FpSpread1.Visible = false;
        txtexcelname.Text = "";
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        btnxl.Visible = false;
        btnmasterprint.Visible = false;
        Printcontrol.Visible = false;
        errmsg.Visible = false;
    }
    public void bindbatch()
    {
        try
        {
            count = 0;
            Chklst_batch.Items.Clear();
            Chk_batch.Checked = false;
            txt_batch.Text = "---Select---";
            ds = da.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                Chklst_batch.DataSource = ds;
                Chklst_batch.DataTextField = "batch_year";
                Chklst_batch.DataValueField = "batch_year";
                Chklst_batch.DataBind();
                for (int i = 0; i < Chklst_batch.Items.Count; i++)
                {
                    Chklst_batch.Items[i].Selected = true;
                    count++;
                }
                if (count > 0)
                {
                    txt_batch.Text = "Batch(" + (Chklst_batch.Items.Count) + ")";
                    if (Chklst_batch.Items.Count == count)
                    {
                        Chk_batch.Checked = true;
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
    public void loadtype()
    {
        try
        {
            count = 0;
            chktype.Checked = false;
            txttype.Text = "---Select---";
            chklstype.Items.Clear();
            collegecode = ddlcollege.SelectedValue.ToString();
            string strquery = "select distinct type from course where college_code='" + collegecode + "' and type is not null and type<>''";
            ds = da.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                txttype.Enabled = true;
                chklstype.DataSource = ds;
                chklstype.DataTextField = "type";
                chklstype.DataBind();
                txttype.Enabled = true;
                for (int i = 0; i < chklstype.Items.Count; i++)
                {
                    chklstype.Items[i].Selected = true;
                    count++;
                }
                if (count > 0)
                {
                    txttype.Text = lbltype.Text + "(" + count + ")";
                    if (count == chklstype.Items.Count)
                    {
                        chktype.Checked = true;
                    }
                }
            }
            else
            {
                txttype.Enabled = false;
            }
        }
        catch
        {
        }
    }
    public void binddegree()
    {
        try
        {
            Chklst_degree.Items.Clear();
            txt_degree.Text = "---Select---";
            chk_degree.Checked = false;
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            collegecode = ddlcollege.SelectedValue.ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            has.Clear();
            has.Add("single_user", singleuser);
            has.Add("group_code", group_user);
            has.Add("college_code", collegecode);
            has.Add("user_code", usercode);

            string typeval = "";
            for (int t = 0; t < chklstype.Items.Count; t++)
            {
                if (chklstype.Items[t].Selected == true)
                {
                    if (typeval == "")
                    {
                        typeval = "'" + chklstype.Items[t].Text.ToString() + "'";
                    }
                    else
                    {
                        typeval = typeval + ",'" + chklstype.Items[t].Text.ToString() + "'";
                    }
                }
            }
            if (typeval.Trim() != "")
            {
                typeval = " and course.type in (" + typeval + ")";
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
            {
                codevalues = "and user_code='" + usercode + "'";
            }
            string strquery = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where  course.course_id=degree.course_id and course.college_code = degree.college_code and  degree.college_code='" + collegecode + "' and deptprivilages.Degree_code=degree.Degree_code " + codevalues + " " + typeval + " ";
            ds = da.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                Chklst_degree.DataSource = ds;
                Chklst_degree.DataTextField = "course_name";
                Chklst_degree.DataValueField = "course_id";
                Chklst_degree.DataBind();

                for (int h = 0; h < Chklst_degree.Items.Count; h++)
                {
                    Chklst_degree.Items[h].Selected = true;
                }
                txt_degree.Text = Ibldegree.Text + "" + "(" + Chklst_degree.Items.Count + ")";
                chk_degree.Checked = true;
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    public void bindbranch()
    {
        try
        {
            collegecode = ddlcollege.SelectedValue.ToString();
            string degreecode = "";
            txt_branch.Text = "---Select---";
            chk_branch.Checked = false;
            chklst_branch.Items.Clear();
            for (int h = 0; h < Chklst_degree.Items.Count; h++)
            {
                if (Chklst_degree.Items[h].Selected == true)
                {
                    if (degreecode == "")
                    {
                        degreecode = Chklst_degree.Items[h].Value;
                    }
                    else
                    {
                        degreecode = degreecode + ',' + Chklst_degree.Items[h].Value;
                    }
                }
            }
            if (degreecode.Trim() != "")
            {
                ds.Clear();
                ds = da.BindBranchMultiple(Session["single_user"].ToString(), Session["group_code"].ToString(), degreecode, collegecode, Session["usercode"].ToString());

                if (ds.Tables[0].Rows.Count > 0)
                {
                    chklst_branch.DataSource = ds;
                    chklst_branch.DataTextField = "dept_name";
                    chklst_branch.DataValueField = "degree_code";
                    chklst_branch.DataBind();
                    for (int h = 0; h < chklst_branch.Items.Count; h++)
                    {
                        chklst_branch.Items[h].Selected = true;
                    }
                    txt_branch.Text = Iblbranch.Text + "(" + (chklst_branch.Items.Count) + ")";
                    chk_branch.Checked = true;
                }
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
            clear();
            txtHeader.Text = "---Select---";
            chkheader.Checked = false;
            chklsheader.Items.Clear();
            string typeval = "";
            for (int t = 0; t < chklstype.Items.Count; t++)
            {
                if (chklstype.Items[t].Selected == true)
                {
                    if (typeval == "")
                    {
                        typeval = "'" + chklstype.Items[t].Text.ToString() + "'";
                    }
                    else
                    {
                        typeval = typeval + ",'" + chklstype.Items[t].Text.ToString() + "'";
                    }
                }
            }
            if (typeval.Trim() != "")
            {
                //typeval = " and a.HeaderType in (" + typeval + ")";
                typeval = " and c.Stream in (" + typeval + ")";
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
            ds.Reset();
            ds.Dispose();
            if (strheadid.Trim() != "")
            {
                // string straccheadquery = "select distinct a.header_id,a.header_name from chlheadersettings c,Acctheader a where c.Header_ID=a.header_id and a.header_name not in ('arrear') " + typeval + " " + strheadid + "";
                // string straccheadquery = "select distinct a.header_id,a.header_name from Acctheader a where a.header_name not in ('arrear') " + strheadid + " "+typeval+"";
                string straccheadquery = " SELECT HeaderPK,HeaderName,hd_priority FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + ddlcollege.SelectedItem.Value + "  order by len(isnull(hd_priority,10000)),hd_priority asc";
                ds = da.select_method_wo_parameter(straccheadquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chklsheader.DataSource = ds;
                    chklsheader.DataTextField = "HeaderName";
                    chklsheader.DataValueField = "HeaderPK";
                    chklsheader.DataBind();

                    for (int i = 0; i < chklsheader.Items.Count; i++)
                    {
                        chklsheader.Items[i].Selected = true;
                    }
                    txtHeader.Text = " Header (" + chklsheader.Items.Count + ")";
                    chkheader.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    public void loadledger()
    {
        try
        {
            txtledger.Text = "---Select---";
            count = 0;
            treeview_spreadfields.Nodes.Clear();
            for (int i = 0; i < chklsheader.Items.Count; i++)
            {
                if (chklsheader.Items[i].Selected == true)
                {
                    node = new TreeNode(chklsheader.Items[i].Text.ToString(), chklsheader.Items[i].Value.ToString());
                    // string strled = "select fee_type,fee_code from fee_info where fee_type not in ('Cash','Income & Expenditure','Misc') and fee_type not in (select bankname from bank_master1) and header_id = '" + chklsheader.Items[i].Value.ToString() + "' order by fee_code";
                    string strled = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='0' and L.CollegeCode = " + ddlcollege.SelectedItem.Value + "  and L.HeaderFK in('" + chklsheader.Items[i].Value.ToString() + "') order by len(isnull(l.priority,1000)) , l.priority asc";
                    ds.Dispose();
                    ds.Reset();
                    ds = da.select_method_wo_parameter(strled, "Text");
                    for (int ledge = 0; ledge < ds.Tables[0].Rows.Count; ledge++)
                    {
                        count++;
                        subchildnode = new TreeNode(ds.Tables[0].Rows[ledge]["LedgerName"].ToString(), ds.Tables[0].Rows[ledge]["LedgerPK"].ToString());
                        subchildnode.ShowCheckBox = true;
                        subchildnode.Checked = true;
                        node.ChildNodes.Add(subchildnode);
                    }
                    node.Checked = true;
                    node.ShowCheckBox = true;
                    treeview_spreadfields.Nodes.Add(node);
                }
            }
            if (count > 0)
            {
                txtledger.Text = "Ledger (" + count + ")";
            }
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
            collegecode = ddlcollege.SelectedValue.ToString();
            // string getfinanceyear = "select convert(nvarchar(15),af.finyear_start,103) sdate,convert(nvarchar(15),af.finyear_end,103) edate,af.acct_id from account_info af,acctinfo a where a.acct_id=af.acct_id and a.college_code='" + collegecode + "' order by af.acct_id";
            string getfinanceyear = "select distinct convert(nvarchar(15),FinYearStart,103) sdate,convert(nvarchar(15),FinYearEnd,103) edate,FinYearPK from FM_FinYearMaster where CollegeCode='" + collegecode + "'  order by FinYearPK desc";
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

    protected void loadfeecategory()
    {
        try
        {
            chklscategory.Items.Clear();
            chkcatefory.Checked = false;
            txtcetgory.Text = "--Select--";
            ds.Clear();
            string linkName = string.Empty;
            string cbltext = string.Empty;
            ds = da.loadFeecategory(Convert.ToString(ddlcollege.SelectedItem.Value), usercode, ref linkName);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                chklscategory.DataSource = ds;
                chklscategory.DataTextField = "TextVal";
                chklscategory.DataValueField = "TextCode";
                chklscategory.DataBind();

                if (chklscategory.Items.Count > 0)
                {
                    for (int i = 0; i < chklscategory.Items.Count; i++)
                    {
                        chklscategory.Items[i].Selected = true;
                        cbltext = Convert.ToString(chklscategory.Items[i].Text);
                    }
                    if (chklscategory.Items.Count == 1)
                        txtcetgory.Text = "" + linkName + "(" + cbltext + ")";
                    else
                        txtcetgory.Text = "" + linkName + "(" + chklscategory.Items.Count + ")";
                    chkcatefory.Checked = true;
                }
            }
        }
        catch { }
    }
    //public void loadfeecategory()
    //{
    //    try
    //    {
    //        collegecode = ddlcollege.SelectedValue.ToString();
    //        chklscategory.Items.Clear();
    //        chkcatefory.Checked = false;
    //        txtcetgory.Text = "---Select---";
    //        string settingquery = "";
    //        string semesterquery = "";
    //        if (group_user.Contains(';'))
    //        {
    //            string[] group_semi = group_user.Split(';');
    //            group_user = group_semi[0].ToString();

    //            settingquery = "select * from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + group_user + "' and college_code ='" + collegecode + "'";
    //        }
    //        else
    //        {
    //            settingquery = "select * from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'";
    //        }
    //        ds.Clear();
    //        ds = da.select_method_wo_parameter(settingquery, "Text");
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            string linkvalue = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
    //            if (linkvalue == "0")
    //            {
    //                semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Semester%' and textval not like '-1%' and college_code ='" + collegecode + "' order by len(textval),textval asc";
    //                ds.Clear();
    //                ds = da.select_method_wo_parameter(semesterquery, "Text");
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    chklscategory.DataSource = ds;
    //                    chklscategory.DataTextField = "textval";
    //                    chklscategory.DataValueField = "TextCode";
    //                    chklscategory.DataBind();
    //                }
    //            }
    //            else
    //            {
    //                semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Year%' and textval not like '-1%' and college_code ='" + collegecode + "' order by len(textval),textval asc";
    //                ds.Clear();
    //                ds = da.select_method_wo_parameter(semesterquery, "Text");
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    chklscategory.DataSource = ds;
    //                    chklscategory.DataTextField = "textval";
    //                    chklscategory.DataValueField = "TextCode";
    //                    chklscategory.DataBind();
    //                }
    //            }
    //        }
    //        else
    //        {
    //            semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Semester%' and textval not like '-1%' and college_code ='" + collegecode + "' order by len(textval),textval asc";
    //            ds.Clear();
    //            ds = da.select_method_wo_parameter(semesterquery, "Text");
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                chklscategory.DataSource = ds;
    //                chklscategory.DataTextField = "textval";
    //                chklscategory.DataValueField = "TextCode";
    //                chklscategory.DataBind();
    //            }
    //        }
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            for (int i = 0; i < chklscategory.Items.Count; i++)
    //            {
    //                chklscategory.Items[i].Selected = true;
    //            }
    //            chkcatefory.Checked = true;
    //            txtcetgory.Text = "Category (" + chklscategory.Items.Count + ")";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        errmsg.Visible = true;
    //        errmsg.Text = ex.ToString();
    //    }
    //}
    public void loadconcession()
    {
        try
        {
            collegecode = ddlcollege.SelectedValue.ToString();
            string concesquery = "select * from TextValTable where TextCriteria='dedre' and college_code='" + collegecode + "'";
            ds.Dispose();
            ds.Reset();
            ds = da.select_method_wo_parameter(concesquery, "text");
            chklsconcession.Items.Clear();
            chkconcession.Checked = false;
            txtcons.Text = "---Select---";
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklsconcession.DataSource = ds;
                chklsconcession.DataTextField = "textval";
                chklsconcession.DataValueField = "Textcode";
                chklsconcession.DataBind();
                for (int i = 0; i < chklsconcession.Items.Count; i++)
                {
                    chklsconcession.Items[i].Selected = true;
                }
                txtcons.Text = "Concession (" + chklsconcession.Items.Count + ")";
                chkconcession.Checked = true;
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        loadfinanceyear();
        loadtype();
        bindbatch();
        binddegree();
        bindbranch();
        loadheader();
        loadledger();
        loadconcession();
        loadfeecategory();

      
    }
    protected void chktype_batchchanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (chktype.Checked == true)
            {
                for (int i = 0; i < chklstype.Items.Count; i++)
                {
                    chklstype.Items[i].Selected = true;
                }
                txttype.Text = lbltype.Text + "(" + (chklstype.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstype.Items.Count; i++)
                {
                    chklstype.Items[i].Selected = false;
                }
                txttype.Text = "--Select--";
            }
            bindbatch();
            binddegree();
            bindbranch();
            loadheader();
            loadledger();
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void chklstype_batchselected(object sender, EventArgs e)
    {
        try
        {
            clear();
            count = 0;
            chktype.Checked = false;
            txttype.Text = "---Select---";
            for (int i = 0; i < chklstype.Items.Count; i++)
            {
                if (chklstype.Items[i].Selected == true)
                {
                    count++;
                }
            }
            if (count > 0)
            {
                txttype.Text = lbltype.Text + "(" + count + ")";
                if (count == chklstype.Items.Count)
                {
                    chktype.Checked = true;
                }
            }
            loadheader();
            bindbatch();
            binddegree();
            bindbranch();
            loadheader();
            loadledger();
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void Chlk_batchchanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (Chk_batch.Checked == true)
            {
                for (int i = 0; i < Chklst_batch.Items.Count; i++)
                {
                    Chklst_batch.Items[i].Selected = true;
                }
                txt_batch.Text = "Batch(" + (Chklst_batch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < Chklst_batch.Items.Count; i++)
                {
                    Chklst_batch.Items[i].Selected = false;
                }
                txt_batch.Text = "--Select--";
            }

            binddegree();
            bindbranch();
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void Chlk_batchselected(object sender, EventArgs e)
    {
        try
        {
            clear();
            txt_batch.Text = "--Select--";
            count = 0;
            Chk_batch.Checked = false;
            for (int i = 0; i < Chklst_batch.Items.Count; i++)
            {
                if (Chklst_batch.Items[i].Selected == true)
                {
                    count = count + 1;
                }
            }

            if (count > 0)
            {
                txt_batch.Text = "Batch(" + count.ToString() + ")";
                if (count == Chklst_batch.Items.Count)
                {
                    Chk_batch.Checked = true;
                }
            }
            binddegree();
            bindbranch();
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void cheklist_Degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            txt_degree.Text = "--Select--";
            chk_degree.Checked = false;
            count = 0;
            for (int i = 0; i < Chklst_degree.Items.Count; i++)
            {
                if (Chklst_degree.Items[i].Selected == true)
                {
                    count = count + 1;
                }
            }
            if (count > 0)
            {
                txt_degree.Text = Ibldegree.Text + "(" + count.ToString() + ")";
                if (count == Chklst_degree.Items.Count)
                {
                    chk_degree.Checked = true;
                }
            }
            bindbranch();
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void checkDegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (chk_degree.Checked == true)
            {
                for (int i = 0; i < Chklst_degree.Items.Count; i++)
                {
                    Chklst_degree.Items[i].Selected = true;
                }
                txt_degree.Text = Ibldegree.Text + "(" + (Chklst_degree.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < Chklst_degree.Items.Count; i++)
                {
                    Chklst_degree.Items[i].Selected = false;
                }
                txt_degree.Text = "--Select--";
            }
            bindbranch();
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void chk_branchchanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (chk_branch.Checked == true)
            {
                for (int i = 0; i < chklst_branch.Items.Count; i++)
                {
                    chklst_branch.Items[i].Selected = true;
                }
                txt_branch.Text = Iblbranch.Text + "(" + (chklst_branch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklst_branch.Items.Count; i++)
                {
                    chklst_branch.Items[i].Selected = false;
                }
                txt_branch.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void chklst_branchselected(object sender, EventArgs e)
    {
        try
        {
            clear();
            count = 0;
            chk_branch.Checked = false;
            txt_branch.Text = "--Select--";
            for (int i = 0; i < chklst_branch.Items.Count; i++)
            {
                if (chklst_branch.Items[i].Selected == true)
                {
                    count = count + 1;
                }
            }
            if (count > 0)
            {
                txt_branch.Text = Iblbranch.Text + "(" + count.ToString() + ")";
                if (count == chklst_branch.Items.Count)
                {
                    chk_branch.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void chkheader_changed(object sender, EventArgs e)
    {
        try
        {
            clear();
            if (chkheader.Checked == true)
            {
                for (int i = 0; i < chklsheader.Items.Count; i++)
                {
                    chklsheader.Items[i].Selected = true;
                }
                txtHeader.Text = "Header (" + (chklsheader.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklsheader.Items.Count; i++)
                {
                    chklsheader.Items[i].Selected = false;
                }
                txtHeader.Text = "--Select--";
            }
            loadledger();
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void chklsheader_selected(object sender, EventArgs e)
    {
        try
        {
            clear();
            count = 0;
            chkheader.Checked = false;
            txtHeader.Text = "--Select--";
            for (int i = 0; i < chklsheader.Items.Count; i++)
            {
                if (chklsheader.Items[i].Selected == true)
                {
                    count++;
                }
            }
            if (count > 0)
            {
                txtHeader.Text = "Header (" + count + ")";
                if (count == chklsheader.Items.Count)
                {
                    chkheader.Checked = true;
                }
            }

            loadledger();
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
            loadheader();
            loadledger();
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
            count = 0;
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
            loadheader();
            loadledger();
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void chkcatefory_changed(object sender, EventArgs e)
    {
        clear();
        if (chkcatefory.Checked == true)
        {
            for (int i = 0; i < chklscategory.Items.Count; i++)
            {
                chklscategory.Items[i].Selected = true;
            }
            txtcetgory.Text = "Category (" + chklscategory.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < chklscategory.Items.Count; i++)
            {
                chklscategory.Items[i].Selected = false;
            }
            txtcetgory.Text = "---Select---";
        }
    }
    protected void chklscategory_selected(object sender, EventArgs e)
    {
        clear();
        txtcetgory.Text = "---Select---";
        chkcatefory.Checked = false;
        count = 0;
        for (int i = 0; i < chklscategory.Items.Count; i++)
        {
            if (chklscategory.Items[i].Selected == true)
            {
                count++;
            }
        }
        if (count > 0)
        {
            txtcetgory.Text = "Category (" + count + ")";
            if (count == chklscategory.Items.Count)
            {
                chkcatefory.Checked = true;
            }
        }
    }
    protected void chkconcession_changed(object sender, EventArgs e)
    {
        clear();
        if (chkconcession.Checked == true)
        {
            for (int i = 0; i < chklsconcession.Items.Count; i++)
            {
                chklsconcession.Items[i].Selected = true;
            }
            txtcons.Text = "Concession (" + chklsconcession.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < chklsconcession.Items.Count; i++)
            {
                chklsconcession.Items[i].Selected = false;
            }
            txtcons.Text = "---Select---";
        }
    }
    protected void chklsconcession_selected(object sender, EventArgs e)
    {
        clear();
        txtcons.Text = "---Select---";
        chkconcession.Checked = false;
        count = 0;
        for (int i = 0; i < chklsconcession.Items.Count; i++)
        {
            if (chklsconcession.Items[i].Selected == true)
            {
                count++;
            }
        }
        if (count > 0)
        {
            txtcons.Text = "Concession (" + count + ")";
            if (count == chklsconcession.Items.Count)
            {
                chkconcession.Checked = true;
            }
        }
    }
    protected void Logout_btn_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("default.aspx", false);
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void btngos_Click(object sender, EventArgs e)
    {
        try
        {
            #region get value
            clear();
            string batchquery = "";
            for (int b = 0; b < Chklst_batch.Items.Count; b++)
            {
                if (Chklst_batch.Items[b].Selected == true)
                {
                    if (batchquery == "")
                    {
                        batchquery = Chklst_batch.Items[b].Text;
                    }
                    else
                    {
                        batchquery = batchquery + "," + Chklst_batch.Items[b].Text;
                    }
                }
            }
            if (batchquery.Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Batch Year And Then Proceed";
                return;
            }

            string degreequery = "";
            for (int b = 0; b < chklst_branch.Items.Count; b++)
            {
                if (chklst_branch.Items[b].Selected == true)
                {
                    if (degreequery == "")
                    {
                        degreequery = chklst_branch.Items[b].Value.ToString();
                    }
                    else
                    {
                        degreequery = degreequery + "," + chklst_branch.Items[b].Value.ToString();
                    }
                }
            }
            if (degreequery.Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Degree And Branch And Then Proceed";
                return;
            }

            string headercode = "";
            for (int b = 0; b < chklsheader.Items.Count; b++)
            {
                if (chklsheader.Items[b].Selected == true)
                {
                    if (headercode == "")
                    {
                        headercode = chklsheader.Items[b].Value.ToString();
                    }
                    else
                    {
                        headercode = headercode + "," + chklsheader.Items[b].Value.ToString();
                    }
                }
            }
            if (headercode.Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Header And Then Proceed";
                return;
            }

            string feecodequery = "";
            for (int remv = 0; remv < treeview_spreadfields.Nodes.Count; remv++)
            {
                if (treeview_spreadfields.Nodes[remv].ChildNodes.Count > 0)
                {
                    for (int child = 0; child < treeview_spreadfields.Nodes[remv].ChildNodes.Count; child++)
                    {
                        if (treeview_spreadfields.Nodes[remv].ChildNodes[child].Checked == true)
                        {
                            if (feecodequery == "")
                            {
                                feecodequery = treeview_spreadfields.Nodes[remv].ChildNodes[child].Value.ToString();
                            }
                            else
                            {
                                feecodequery = feecodequery + "," + treeview_spreadfields.Nodes[remv].ChildNodes[child].Value.ToString();
                            }
                        }
                    }
                }
            }
            if (feecodequery.Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Ledger And Then Proceed";
                return;
            }

            string actidquery = "";
            for (int i = 0; i < chklsfyear.Items.Count; i++)
            {
                if (chklsfyear.Items[i].Selected == true)
                {
                    string accid = chklsfyear.Items[i].Value.ToString();
                    if (actidquery == "")
                    {
                        actidquery = "'" + accid + "'";
                    }
                    else
                    {
                        actidquery = actidquery + ",'" + accid + "'";
                    }
                }
            }
            if (actidquery.Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Finance Year And The Proceed";
                return;
            }

            int noofcon = 0;
            string deductionquery = "";
            for (int i = 0; i < chklsconcession.Items.Count; i++)
            {
                if (chklsconcession.Items[i].Selected == true)
                {
                    noofcon++;
                    if (deductionquery == "")
                    {
                        deductionquery = chklsconcession.Items[i].Value.ToString();
                    }
                    else
                    {
                        deductionquery = deductionquery + "," + chklsconcession.Items[i].Value.ToString();
                    }
                }
            }
            if (deductionquery.Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Concession And The Proceed";
                return;
            }
            noofcon = noofcon + 6;

            string feecatquery = "";
            for (int i = 0; i < chklscategory.Items.Count; i++)
            {
                if (chklscategory.Items[i].Selected == true)
                {
                    if (feecatquery == "")
                    {
                        feecatquery = chklscategory.Items[i].Value.ToString();
                    }
                    else
                    {
                        feecatquery = feecatquery + "," + chklscategory.Items[i].Value.ToString();
                    }
                }
            }
            if (feecatquery.Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Category And The Proceed";
                return;
            }
            //
            #endregion

            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;

            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
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
            FpSpread1.CommandBar.Visible = false;

            FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
            style2.Font.Size = 13;
            style2.Font.Name = "Book Antiqua";
            style2.Font.Bold = true;
            style2.HorizontalAlign = HorizontalAlign.Center;
            style2.ForeColor = System.Drawing.Color.White;
            style2.BackColor = System.Drawing.Color.Teal;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);
            FpSpread1.Visible = true;
            FpSpread1.Sheets[0].AutoPostBack = true;

            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 2;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
            FpSpread1.Sheets[0].RowCount = 0;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = Ibldegree.Text;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);

            Boolean stuflag = false;
            //string paidstuquery = "select r.Batch_Year,r.degree_code,s.fee_category,COUNT(distinct s.roll_admit ) as ADV from fee_status s,Registration r ,fee_info i where s.roll_admit = r.Roll_Admit and s.header_id = i.header_id and r.DelFlag=0 and r.Exam_Flag<>'debar' and r.cc=0";
            //paidstuquery = paidstuquery + " and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and s.Header_ID in(" + headercode + ") and i.fee_code in(" + feecodequery + ") and s.fee_category in(" + feecatquery + ") ";
            //paidstuquery = paidstuquery + " and r.Roll_Admit not in(select r1.Roll_Admit from fee_allot fa,Registration r1,fee_info fi where fa.roll_admit=r1.roll_admit and fa.Header_ID=fi.header_id and fa.fee_code=fi.fee_code and isnull(fa.deduct,'0')>0 and r1.Batch_Year in(" + batchquery + ") and r1.degree_code in(" + degreequery + ") and fa.Header_ID in(" + headercode + ") and fa.fee_code in(" + feecodequery + ") and fa.fee_category in(" + feecatquery + ") ) group by r.Batch_Year,r.degree_code,s.fee_category having SUM(balance) = 0 ";

            //string paidstuquery = "select r.Batch_Year,r.degree_code,s.fee_category,COUNT(distinct s.roll_admit ) as ADV  from fee_status s,Registration r where s.roll_admit = r.Roll_Admit and balance = 0 and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and s.Header_ID in(" + headercode + ") and s.fee_category in(" + feecatquery + ")";
            // paidstuquery = paidstuquery + " and s.roll_admit not in (select distinct a1.roll_admit from fee_allot a1 where a1.deduct > 0) group by r.Batch_Year,r.degree_code,s.fee_category  having SUM(balance) = 0  order by Batch_Year,degree_code ,fee_category ";

            string paidstuquery = " select r.Batch_Year ,r.degree_code,f.FeeCategory,COUNT(distinct f.App_No) as ADV from FT_FeeAllot f,Registration r where f.App_No=r.App_No and ISNULL(f.DeductAmout,'0')='0' and f.balamount=0 and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and f.HeaderFK in(" + headercode + ") and f.FeeCategory in(" + feecatquery + ") and f.DeductReason in(" + deductionquery + ") and r.college_code='" + collegecode + "' group by r.Batch_Year,r.degree_code,f.FeeCategory having SUM(BalAmount)=0 order by r.Batch_Year,r.degree_code,f.FeeCategory";
            //and f.LedgerFK in() 
            DataSet dspaid = da.select_method_wo_parameter(paidstuquery, "Text");

            //string balancequery = "select r.Batch_Year,r.degree_code,s.fee_category,COUNT(distinct s.roll_admit ) as Ins from fee_status s,Registration r ,fee_info i where s.roll_admit = r.Roll_Admit and s.header_id = i.header_id and r.DelFlag=0 and r.Exam_Flag<>'debar' and r.cc=0";
            //balancequery = balancequery + " and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and s.Header_ID in(" + headercode + ") and i.fee_code in(" + feecodequery + ") and s.fee_category in(" + feecatquery + ") ";
            //balancequery = balancequery + " and r.Roll_Admit not in(select r1.Roll_Admit from fee_allot fa,Registration r1,fee_info fi where fa.roll_admit=r1.roll_admit and fa.Header_ID=fi.header_id and fa.fee_code=fi.fee_code and isnull(fa.deduct,'0')>0 and r1.Batch_Year in(" + batchquery + ") and r1.degree_code in(" + degreequery + ") and fa.Header_ID in(" + headercode + ") and fa.fee_code in(" + feecodequery + ") and fa.fee_category in(" + feecatquery + ") )  group by r.Batch_Year,r.degree_code,s.fee_category having SUM(balance) >= 0 ";

            // string balancequery = "select r.Batch_Year,r.degree_code,s.fee_category,COUNT(distinct s.roll_admit ) as Ins  from fee_status s,Registration r where s.roll_admit = r.Roll_Admit and balance > 0 and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and s.Header_ID in(" + headercode + ") and s.fee_category in(" + feecatquery + ")";
            //balancequery = balancequery + " and s.roll_admit not in (select distinct a1.roll_admit from fee_allot a1 where a1.deduct > 0) group by r.Batch_Year,r.degree_code,s.fee_category  having SUM(balance) > 0  order by Batch_Year,degree_code ,fee_category ";

            string balancequery = " select r.Batch_Year ,r.degree_code,f.FeeCategory,COUNT(distinct f.App_No) as Ins from FT_FeeAllot f,Registration r where f.App_No=r.App_No and ISNULL(f.DeductAmout,'0')='0' and ISNULL(BalAmount,'0')>0 and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and f.HeaderFK in(" + headercode + ")  and f.FeeCategory in(" + feecatquery + ") and f.DeductReason in(" + deductionquery + ") and r.college_code='" + collegecode + "' group by r.Batch_Year,r.degree_code,f.FeeCategory having SUM(BalAmount)>0 order by r.Batch_Year,r.degree_code,f.FeeCategory";
            //and f.LedgerFK in()

            DataSet dsbalance = da.select_method_wo_parameter(balancequery, "Text");

            //  string strconquery = "select r1.Batch_Year,r1.degree_code,fa.dedect_reason,fa.fee_category,count(r1.roll_no) as concount   from fee_allot fa,Registration r1,fee_info fi where fa.roll_admit=r1.roll_admit and fa.Header_ID=fi.header_id and fa.fee_code=fi.fee_code and isnull(fa.deduct,'0')>0 and r1.Batch_Year in(" + batchquery + ") and r1.degree_code in(" + degreequery + ") and fa.Header_ID in(" + headercode + ") and fa.fee_code in(" + feecodequery + ") and fa.fee_category in(" + feecatquery + ") group by r1.Batch_Year,r1.degree_code,fa.dedect_reason,fa.fee_category";
            //old 
            //string strconquery = " select r.Batch_Year,r.degree_code ,f.DeductReason ,f.FeeCategory,COUNT(r.Roll_No) as concount from FT_FeeAllot f,Registration r where f.App_No=r.App_No and ISNULL(f.DeductAmout,'0')>0 and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and f.HeaderFK in(" + headercode + ") and f.LedgerFK in(" + feecodequery + ") and f.FeeCategory in(" + feecatquery + ") and f.DeductReason in(" + deductionquery + ") group by r.Batch_Year,r.degree_code,f.DeductReason,f.FeeCategory";
            //DataSet dsconcess = da.select_method_wo_parameter(strconquery, "text");
            //new
            string strconquery = " select r.Batch_Year,r.degree_code ,f.DeductReason ,f.FeeCategory,COUNT(r.Roll_No) as concount from FT_FeeAllotDegree f,Registration r where f.DegreeCode=r.Degree_Code and f.BatchYear=r.Batch_Year and ISNULL(f.DeductAmout,'0')>0 and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and f.HeaderFK in(" + headercode + ") and f.LedgerFK in(" + feecodequery + ") and f.FeeCategory in(" + feecatquery + ") and f.DeductReason in(" + deductionquery + ") and r.college_code='" + collegecode + "' group by r.Batch_Year,r.degree_code,f.DeductReason,f.FeeCategory";
            DataSet dsconcess = da.select_method_wo_parameter(strconquery, "text");

            string strdegree = "select distinct r.Batch_Year,r.degree_code,c.type,c.course_id,c.Edu_Level,c.Course_Name,de.Dept_Name,count(r.roll_no) stucount from Registration r,Degree d,Department de,Course c where r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and c.Course_Id=d.Course_Id and r.DelFlag=0 and r.Exam_Flag<>'debar' and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and d.college_code='" + collegecode + "' group by r.Batch_Year,r.degree_code,c.type,c.Edu_Level,c.Course_Name,de.Dept_Name,c.course_id order by c.Edu_Level desc,c.type,r.degree_code,r.Batch_Year desc";
            DataSet dsdegree = da.select_method_wo_parameter(strdegree, "Text");

            // string feeallotquery = "select Batch,d.Degree_Code,f.FeeCat,sum(f.FeeAmount) as feeamount from feedefine f,Degree d where f.DepCode=d.Dept_Code and f.DegreeCode=d.Course_Id and f.Batch in(" + batchquery + ") and d.degree_code in(" + degreequery + ") and f.HeadID in(" + headercode + ") and f.feecode in(" + feecodequery + ") and f.feecat in(" + feecatquery + ") group by Batch,d.Degree_Code,f.FeeCat";
            string feeallotquery = " select BatchYear,degree_code,f.FeeCategory ,SUM(FeeAmount) as feeamount from FT_FeeAllotDegree f,Degree d where f.DegreeCode=d.Degree_Code and BatchYear in(" + batchquery + ") and degree_code in(" + degreequery + ") and f.HeaderFK in(" + headercode + ") and f.LedgerFK in(" + feecodequery + ") and f.FeeCategory in(" + feecatquery + ") and f.DeductReason in(" + deductionquery + ") and d.college_code='" + collegecode + "' group by BatchYear,d.Degree_Code,f.FeeCategory";
            DataSet dsfeeallot = da.select_method_wo_parameter(feeallotquery, "text");

            for (int i = 0; i < chklscategory.Items.Count; i++)
            {
                if (chklscategory.Items[i].Selected == true)
                {
                    string feecat = chklscategory.Items[i].Value.ToString();
                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Adv";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Note = feecat;

                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Inst";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Note = feecat;

                    for (int co = 0; co < chklsconcession.Items.Count; co++)
                    {
                        if (chklsconcession.Items[co].Selected == true)
                        {
                            FpSpread1.Sheets[0].ColumnCount++;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = chklsconcession.Items[co].Text.ToString();
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = chklsconcession.Items[co].Value.ToString();
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Note = feecat;
                        }
                    }
                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total Advance";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Note = feecat;

                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Adv.Rate";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Note = feecat;

                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Inst.Rate";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Note = feecat;

                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Note = feecat;

                    //FpSpread1.Sheets[0].ColumnCount++;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Grand Total";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Note = feecat;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - noofcon].Text = chklscategory.Items[i].Text.ToString();
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - noofcon, 1, noofcon);

                }
            }
            int srno = 0;
            Hashtable hatdegrtotal = new Hashtable();
            for (int i = 0; i < dsdegree.Tables[0].Rows.Count; i++)
            {
                string batchyear = dsdegree.Tables[0].Rows[i]["Batch_Year"].ToString();
                string degreecode = dsdegree.Tables[0].Rows[i]["Degree_Code"].ToString();
                string course = dsdegree.Tables[0].Rows[i]["Course_name"].ToString();
                string department = dsdegree.Tables[0].Rows[i]["Dept_Name"].ToString();
                string courseid = dsdegree.Tables[0].Rows[i]["course_id"].ToString();

                //if (!hatdegrtotal.Contains(courseid))
                //{
                //    if (hatdegrtotal.Count > 0 || dsdegree.Tables[0].Rows.Count - 1 == i)
                //    {
                //        FpSpread1.Sheets[0].RowCount++;
                //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
                //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                //        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 2);
                //        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.LightGray;

                //        for (int c = 2; c < FpSpread1.Sheets[0].ColumnCount; c++)
                //        {
                //            Double total = 0;
                //            for (int r = FpSpread1.Sheets[0].RowCount - 2; r >= 0; r--)
                //            {
                //                if (FpSpread1.Sheets[0].Cells[r, 0].Text.Trim() != "Total")
                //                {
                //                    if (FpSpread1.Sheets[0].Cells[r, c].Text.Trim() != "")
                //                    {
                //                        total = total + Convert.ToDouble(FpSpread1.Sheets[0].Cells[r, c].Text.ToString());
                //                    }
                //                }
                //                else
                //                {
                //                    r = -1;
                //                }
                //            }
                //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = total.ToString();
                //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Font.Name = "Book Antiqua";
                //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Center;
                //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Font.Bold = true;
                //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Font.Size = FontUnit.Medium;
                //        }
                //    }
                //    hatdegrtotal.Add(courseid, FpSpread1.Sheets[0].RowCount);
                //}


                Double degretotoal = 0;
                Double totsu = 0;
                Double constucount = 0;
                Boolean degflag = false;
                for (int c = 2; c < FpSpread1.Sheets[0].ColumnCount; c++)
                {
                    string feesamou = "0";
                    string getheadtext = FpSpread1.Sheets[0].ColumnHeader.Cells[1, c].Text.ToString().Trim().ToLower();
                    string feecat = FpSpread1.Sheets[0].ColumnHeader.Cells[1, c].Note.ToString();
                    string deductrea = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, c].Tag);
                    if (getheadtext == "adv")
                    {

                        dspaid.Tables[0].DefaultView.RowFilter = "Batch_Year='" + batchyear + "' and degree_code='" + degreecode + "' and FeeCategory='" + feecat + "'";
                        DataView dvadv = dspaid.Tables[0].DefaultView;
                        if (dvadv.Count > 0)
                        {
                            feesamou = dvadv[0]["adv"].ToString();
                            totsu = totsu + Convert.ToDouble(feesamou);
                        }
                        degretotoal = degretotoal + Convert.ToDouble(feesamou);
                    }
                    if (getheadtext == "inst")
                    {
                        dsbalance.Tables[0].DefaultView.RowFilter = "Batch_Year='" + batchyear + "' and degree_code='" + degreecode + "' and FeeCategory='" + feecat + "'";
                        DataView dvadv = dsbalance.Tables[0].DefaultView;
                        if (dvadv.Count > 0)
                        {
                            feesamou = dvadv[0]["Ins"].ToString();
                            totsu = totsu + Convert.ToDouble(feesamou);
                        }
                        degretotoal = degretotoal + Convert.ToDouble(feesamou);
                    }
                    if (deductrea.Trim() != "")
                    {
                        dsconcess.Tables[0].DefaultView.RowFilter = "Batch_Year='" + batchyear + "' and degree_code='" + degreecode + "' and FeeCategory='" + feecat + "' and DeductReason='" + deductrea + "'";
                        DataView dvadv = dsconcess.Tables[0].DefaultView;
                        if (dvadv.Count > 0)
                        {
                            feesamou = dvadv[0]["concount"].ToString();
                            totsu = totsu + Convert.ToDouble(feesamou);
                        }
                        degretotoal = degretotoal + Convert.ToDouble(feesamou);
                    }
                    if (feesamou != "0" || degflag == true)
                    {
                        stuflag = true;
                        if (degflag == false)
                        {
                            srno++;
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = batchyear + " - " + course + " - " + department;

                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = feesamou.ToString();
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Center;                           
                            degflag = true;
                        }
                        if (feesamou != "0" && feesamou != "")
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = feesamou.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Center;
                        }

                        if (getheadtext == "total advance")
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = totsu.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Center;
                        }

                        if (getheadtext == "adv.rate")
                        {
                            dsfeeallot.Tables[0].DefaultView.RowFilter = "BatchYear='" + batchyear + "' and degree_code='" + degreecode + "' and FeeCategory='" + feecat + "'";
                            DataView dvadv = dsfeeallot.Tables[0].DefaultView;
                            if (dvadv.Count > 0)
                            {
                                feesamou = dvadv[0]["feeamount"].ToString();
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = feesamou.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Center;

                            Double allamoun = 0;
                            if (feesamou.Trim() != "0" && totsu > 0)
                            {
                                allamoun = totsu * Convert.ToDouble(feesamou);
                            }

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c + 2].Text = allamoun.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c + 2].HorizontalAlign = HorizontalAlign.Center;
                            totsu = 0;
                        }

                    }
                }
            }
            if (stuflag == true)
            {
                FpSpread1.Sheets[0].RowCount++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 2);
                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.LightGray;

                for (int c = 2; c < FpSpread1.Sheets[0].ColumnCount; c++)
                {
                    Double total = 0;
                    for (int r = FpSpread1.Sheets[0].RowCount - 2; r >= 0; r--)
                    {
                        if (FpSpread1.Sheets[0].Cells[r, 0].Text.Trim() != "Total")
                        {
                            if (FpSpread1.Sheets[0].Cells[r, c].Text.Trim() != "")
                            {
                                total = total + Convert.ToDouble(FpSpread1.Sheets[0].Cells[r, c].Text.ToString());
                            }
                        }
                        else
                        {
                            r = -1;
                        }
                    }
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = total.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Font.Bold = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Font.Size = FontUnit.Medium;
                }
                FpSpread1.Sheets[0].RowCount++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 2);
                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.LightGreen;

                for (int c = 2; c < FpSpread1.Sheets[0].ColumnCount; c++)
                {
                    Double total = 0;
                    for (int r = 0; r < FpSpread1.Sheets[0].RowCount - 1; r++)
                    {
                        if (FpSpread1.Sheets[0].Cells[r, 0].Text != "Total")
                        {
                            if (FpSpread1.Sheets[0].Cells[r, c].Text.Trim() != "")
                            {
                                total = total + Convert.ToDouble(FpSpread1.Sheets[0].Cells[r, c].Text.ToString());
                            }
                            else
                            {
                                FpSpread1.Sheets[0].Cells[r, c].Text = "0";
                                FpSpread1.Sheets[0].Cells[r, c].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                    }
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = total.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Font.Bold = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Font.Size = FontUnit.Medium;
                }

                FpSpread1.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                btnxl.Visible = true;
                btnmasterprint.Visible = true;
            }
            else
            {
                clear();
                errmsg.Visible = true;
                errmsg.Text = "No Records Found";
            }


            dsdegree.Dispose();
            dsdegree = null;
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, ddlcollege.SelectedItem.Value, "FinanceConcessionReport.aspx");
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    protected DataSet loadDetails()
    {
        DataSet dsval = new DataSet();
        try
        {
            #region get value
            clear();
            string batch = Convert.ToString(getCblSelectedValue(Chklst_batch));
            string degree = Convert.ToString(getCblSelectedValue(chklst_branch));
            string hdfk = Convert.ToString(getCblSelectedValue(chklsheader));
            string fnlyear = Convert.ToString(getCblSelectedValue(chklsfyear));
            string deductReason = Convert.ToString(getCblSelectedValue(chklsconcession));
            string feecat = Convert.ToString(getCblSelectedValue(chklscategory));


            string ldFK = "";
            for (int remv = 0; remv < treeview_spreadfields.Nodes.Count; remv++)
            {
                if (treeview_spreadfields.Nodes[remv].ChildNodes.Count > 0)
                {
                    for (int child = 0; child < treeview_spreadfields.Nodes[remv].ChildNodes.Count; child++)
                    {
                        if (treeview_spreadfields.Nodes[remv].ChildNodes[child].Checked == true)
                        {
                            if (ldFK == "")
                                ldFK = treeview_spreadfields.Nodes[remv].ChildNodes[child].Value.ToString();
                            else
                                ldFK = ldFK + "','" + treeview_spreadfields.Nodes[remv].ChildNodes[child].Value.ToString();
                        }
                    }
                }
            }
            #endregion

            string selQ = string.Empty;
            selQ = "select distinct r.Batch_Year,r.degree_code,c.type,c.course_id,c.Edu_Level,c.Course_Name,de.Dept_Name,count(r.roll_no) stucount from Registration r,Degree d,Department de,Course c where r.degree_code=d.Degree_Code and d.Dept_Code=de.Dept_Code and c.Course_Id=d.Course_Id and r.DelFlag=0 and r.Exam_Flag<>'debar' and r.Batch_Year in('" + batch + "') and r.degree_code in('" + degree + "') and d.college_code='" + collegecode + "' group by r.Batch_Year,r.degree_code,c.type,c.Edu_Level,c.Course_Name,de.Dept_Name,c.course_id order by c.Edu_Level desc,c.type,r.degree_code,r.Batch_Year desc";
            //paid
            selQ += " select count(distinct f.app_no)as total,sum(isnull(paidAmount,0)),sum(balAmount),sum(deductAmout),feecategory,r.batch_year,degree_code from Ft_feeallot f,registration r where f.app_no=r.app_no and r.Batch_Year in('" + batch + "') and r.degree_code in('" + degree + "') and f.feecategory in('" + feecat + "') and finyearfk in('" + fnlyear + "') and headerfk in('" + hdfk + "') and ledgerfk in('" + ldFK + "') and DeductReason in('" + deductReason + "','0') and r.college_code='" + collegecode + "' and isnull(f.DeductAmout,'0')>0  group by feecategory,r.batch_year,degree_code having  sum(isnull(balAmount,'0'))=0";//and  DeductReason<>0
            //balance
            selQ += " select count(distinct f.app_no)as total,sum(isnull(paidAmount,0)),sum(balAmount),sum(deductAmout),feecategory,r.batch_year,degree_code from Ft_feeallot f,registration r where f.app_no=r.app_no and r.Batch_Year in('" + batch + "') and r.degree_code in('" + degree + "') and f.feecategory in('" + feecat + "') and finyearfk in('" + fnlyear + "') and headerfk in('" + hdfk + "') and ledgerfk in('" + ldFK + "') and DeductReason in('" + deductReason + "','0') and r.college_code='" + collegecode + "' and isnull(f.DeductAmout,'0')>0  group by feecategory,r.batch_year,degree_code having  sum(isnull(feeamount,'0'))>0";
            //and  DeductReason<>0
            //deduction count
            selQ += " select count(distinct f.app_no)as total,sum(deductAmout),DeductReason,feecategory,r.batch_year,degree_code from Ft_feeallot f,registration r where f.app_no=r.app_no and r.Batch_Year in('" + batch + "') and r.degree_code in('" + degree + "') and f.feecategory in('" + feecat + "') and finyearfk in('" + fnlyear + "') and headerfk in('" + hdfk + "') and ledgerfk in('" + ldFK + "') and DeductReason in('" + deductReason + "','0') and r.college_code='" + collegecode + "' and isnull(f.DeductAmout,'0')>0  group by feecategory,DeductReason,r.batch_year,degree_code having  sum(isnull(deductAmout,'0'))>0";//and  DeductReason<>0 

            //paid amount
            selQ += " select sum(isnull(paidAmount,0)) as paidAmount,sum(balAmount),sum(deductAmout) as deductAmout,feecategory,r.batch_year,degree_code from Ft_feeallot f,registration r where f.app_no=r.app_no and r.Batch_Year in('" + batch + "') and r.degree_code in('" + degree + "') and f.feecategory in('" + feecat + "') and finyearfk in('" + fnlyear + "') and headerfk in('" + hdfk + "') and ledgerfk in('" + ldFK + "') and DeductReason in('" + deductReason + "','0') and r.college_code='" + collegecode + "' and isnull(f.DeductAmout,'0')>0   group by feecategory,r.batch_year,degree_code having  sum(isnull(balAmount,'0'))=0";//and  DeductReason<>0
            //balance amount
            selQ += " select sum(isnull(paidAmount,0)),sum(balAmount) as balAmount,sum(deductAmout) as deductAmout,feecategory,r.batch_year,degree_code from Ft_feeallot f,registration r where f.app_no=r.app_no and r.Batch_Year in('" + batch + "') and r.degree_code in('" + degree + "') and f.feecategory in('" + feecat + "') and finyearfk in('" + fnlyear + "') and headerfk in('" + hdfk + "') and ledgerfk in('" + ldFK + "') and DeductReason in('" + deductReason + "','0') and r.college_code='" + collegecode + "' and isnull(f.DeductAmout,'0')>0   group by feecategory,r.batch_year,degree_code having  sum(isnull(balAmount,'0'))>0";//and  DeductReason<>0


            selQ += " select sum(deductAmout) as deductAmout,DeductReason,feecategory,r.batch_year,degree_code from Ft_feeallot f,registration r where f.app_no=r.app_no and r.Batch_Year in('" + batch + "') and r.degree_code in('" + degree + "') and f.feecategory in('" + feecat + "') and finyearfk in('" + fnlyear + "') and headerfk in('" + hdfk + "') and ledgerfk in('" + ldFK + "') and DeductReason in('" + deductReason + "','0') and r.college_code='" + collegecode + "' and isnull(f.DeductAmout,'0')>0  group by feecategory,DeductReason ,r.batch_year,degree_code having  sum(isnull(deductAmout,'0'))>0";
            dsval.Clear();
            dsval = da.select_method_wo_parameter(selQ, "Text");
        }
        catch { }
        return dsval;
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        ds.Clear();
        ds = loadDetails();
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[2].Rows.Count > 0)
        {
            loadSpreadDetails();
        }
        else
        {
            FpSpread1.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnxl.Visible = false;
            btnmasterprint.Visible = false;
            errmsg.Visible = true;
            errmsg.Text = "No Records Found";

        }

    }

    protected void loadSpreadDetails()
    {
        try
        {
            #region design

            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;

            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
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
            FpSpread1.CommandBar.Visible = false;

            FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
            style2.Font.Size = 13;
            style2.Font.Name = "Book Antiqua";
            style2.Font.Bold = true;
            style2.HorizontalAlign = HorizontalAlign.Center;
            style2.ForeColor = System.Drawing.Color.White;
            style2.BackColor = System.Drawing.Color.Teal;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);
            FpSpread1.Visible = true;
            FpSpread1.Sheets[0].AutoPostBack = true;

            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 2;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
            FpSpread1.Sheets[0].RowCount = 0;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = Ibldegree.Text;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            int colCnt = 0;
            int fnlCnt = 0;
            bool boolcheck = false;
            Hashtable htCol = new Hashtable();
            for (int i = 0; i < chklscategory.Items.Count; i++)
            {
                if (chklscategory.Items[i].Selected == true)
                {
                    string feecat = Convert.ToString(chklscategory.Items[i].Value);
                    colCnt = FpSpread1.Sheets[0].ColumnCount++;
                    fnlCnt++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Adv";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Note = feecat;
                    htCol.Add("Adv" + feecat, FpSpread1.Sheets[0].ColumnCount - 1);

                    FpSpread1.Sheets[0].ColumnCount++;
                    fnlCnt++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Inst";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Note = feecat;
                    htCol.Add("Inst" + feecat, FpSpread1.Sheets[0].ColumnCount - 1);
                    for (int co = 0; co < chklsconcession.Items.Count; co++)
                    {
                        if (chklsconcession.Items[co].Selected == true)
                        {
                            boolcheck = true;
                            FpSpread1.Sheets[0].ColumnCount++;
                            fnlCnt++;
                            string consval = Convert.ToString(chklsconcession.Items[co].Value);
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = chklsconcession.Items[co].Text.ToString();
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = chklsconcession.Items[co].Value.ToString();
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Note = feecat;
                            htCol.Add(feecat + consval, FpSpread1.Sheets[0].ColumnCount - 1);
                        }
                    }
                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total No Concession";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Note = feecat;
                    fnlCnt++;
                    htCol.Add("Total No Deduction" + feecat, FpSpread1.Sheets[0].ColumnCount - 1);

                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Adv Concession Amt";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Note = feecat;
                    fnlCnt++;
                    htCol.Add("Adv Deduct Amt" + feecat, FpSpread1.Sheets[0].ColumnCount - 1);

                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Inst Concession Amt";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Note = feecat;
                    fnlCnt++;
                    htCol.Add("Inst Deduct Amt" + feecat, FpSpread1.Sheets[0].ColumnCount - 1);

                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total Concession Amt";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Note = feecat;
                    fnlCnt++;
                    htCol.Add("Total Deduction Amt" + feecat, FpSpread1.Sheets[0].ColumnCount - 1);
                }
                if (boolcheck)
                {
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, colCnt].Text = Convert.ToString(chklscategory.Items[i].Text);
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, colCnt, 1, fnlCnt);
                    fnlCnt = 0;
                    boolcheck = false;
                }
            }

            #endregion

            #region value
            int sno = 0;
            Hashtable fnlgrandtotal = new Hashtable();
            Hashtable grandtotal = new Hashtable();
            for (int batch = 0; batch < Chklst_batch.Items.Count; batch++)
            {
                if (Chklst_batch.Items[batch].Selected)
                {
                    for (int deg = 0; deg < chklst_branch.Items.Count; deg++)
                    {
                        if (chklst_branch.Items[deg].Selected)
                        {
                            double totalCnsCnt = 0;
                            bool boolDeg = false;
                            bool boolsno = true;
                            for (int feec = 0; feec < chklscategory.Items.Count; feec++)
                            {
                                if (chklscategory.Items[feec].Selected)
                                {
                                    #region
                                    if (boolsno)
                                    {
                                        sno++;
                                        FpSpread1.Sheets[0].RowCount++;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(Chklst_batch.Items[batch].Text + "-" + chklst_branch.Items[deg].Text);
                                        boolsno = false;

                                    }
                                    string feecat = Convert.ToString(chklscategory.Items[feec].Value);
                                    int colCnts = 0;
                                    double advCount = 0;
                                    //adv
                                    int.TryParse(Convert.ToString(htCol["Adv" + feecat]), out colCnts);
                                    if (ds.Tables[1].Rows.Count > 0)
                                    {
                                        ds.Tables[1].DefaultView.RowFilter = " batch_year='" + Chklst_batch.Items[batch].Value + "' and degree_code='" + chklst_branch.Items[deg].Value + "' and feecategory='" + feecat + "'";
                                        DataView dvadv = ds.Tables[1].DefaultView;
                                        if (dvadv.Count > 0)
                                            double.TryParse(Convert.ToString(dvadv[0]["total"]), out advCount);
                                        //totalCnsCnt += advCount;
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colCnts].Text = Convert.ToString(advCount);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colCnts].HorizontalAlign = HorizontalAlign.Right;
                                    if (!grandtotal.ContainsKey(colCnts))
                                        grandtotal.Add(colCnts, Convert.ToString(advCount));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(grandtotal[colCnts]), out amount);
                                        amount += advCount;
                                        grandtotal.Remove(colCnts);
                                        grandtotal.Add(colCnts, Convert.ToString(amount));
                                    }

                                    //install
                                    double instCount = 0;
                                    int.TryParse(Convert.ToString(htCol["Inst" + feecat]), out colCnts);
                                    if (ds.Tables[2].Rows.Count > 0)
                                    {
                                        ds.Tables[2].DefaultView.RowFilter = " batch_year='" + Chklst_batch.Items[batch].Value + "' and degree_code='" + chklst_branch.Items[deg].Value + "' and feecategory='" + feecat + "'";
                                        DataView dvadv = ds.Tables[2].DefaultView;
                                        if (dvadv.Count > 0)
                                            double.TryParse(Convert.ToString(dvadv[0]["total"]), out instCount);
                                        // totalCnsCnt += instCount;
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colCnts].Text = Convert.ToString(instCount);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colCnts].HorizontalAlign = HorizontalAlign.Right;
                                    if (!grandtotal.ContainsKey(colCnts))
                                        grandtotal.Add(colCnts, Convert.ToString(instCount));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(grandtotal[colCnts]), out amount);
                                        amount += instCount;
                                        grandtotal.Remove(colCnts);
                                        grandtotal.Add(colCnts, Convert.ToString(amount));
                                    }

                                    for (int dedt = 0; dedt < chklsconcession.Items.Count; dedt++)
                                    {
                                        if (chklsconcession.Items[dedt].Selected)
                                        {
                                            //install
                                            double consCount = 0;
                                            string consVal = Convert.ToString(chklsconcession.Items[dedt].Value);
                                            int.TryParse(Convert.ToString(htCol[feecat + consVal]), out colCnts);
                                            if (ds.Tables[3].Rows.Count > 0)
                                            {
                                                ds.Tables[3].DefaultView.RowFilter = " batch_year='" + Chklst_batch.Items[batch].Value + "' and degree_code='" + chklst_branch.Items[deg].Value + "' and feecategory='" + feecat + "' and deductReason='" + consVal + "'";
                                                DataView dvadv = ds.Tables[3].DefaultView;
                                                if (dvadv.Count > 0)
                                                    double.TryParse(Convert.ToString(dvadv[0]["total"]), out consCount);
                                                totalCnsCnt += consCount;
                                            }
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colCnts].Text = Convert.ToString(consCount);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colCnts].HorizontalAlign = HorizontalAlign.Right;
                                            if (!grandtotal.ContainsKey(colCnts))
                                                grandtotal.Add(colCnts, Convert.ToString(consCount));
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(grandtotal[colCnts]), out amount);
                                                amount += consCount;
                                                grandtotal.Remove(colCnts);
                                                grandtotal.Add(colCnts, Convert.ToString(amount));
                                            }
                                        }
                                    }
                                    //no of count
                                    int.TryParse(Convert.ToString(htCol["Total No Deduction" + feecat]), out colCnts);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colCnts].Text = Convert.ToString(totalCnsCnt);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colCnts].HorizontalAlign = HorizontalAlign.Right;
                                    if (!grandtotal.ContainsKey(colCnts))
                                        grandtotal.Add(colCnts, Convert.ToString(totalCnsCnt));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(grandtotal[colCnts]), out amount);
                                        amount += totalCnsCnt;
                                        grandtotal.Remove(colCnts);
                                        grandtotal.Add(colCnts, Convert.ToString(amount));
                                    }

                                    //paid amount
                                    double advPaidAmt = 0;
                                    int.TryParse(Convert.ToString(htCol["Adv Deduct Amt" + feecat]), out colCnts);
                                    if (ds.Tables[4].Rows.Count > 0)
                                    {
                                        ds.Tables[4].DefaultView.RowFilter = " batch_year='" + Chklst_batch.Items[batch].Value + "' and degree_code='" + chklst_branch.Items[deg].Value + "' and feecategory='" + feecat + "'";
                                        DataView dvadv = ds.Tables[4].DefaultView;
                                        if (dvadv.Count > 0)
                                            double.TryParse(Convert.ToString(dvadv[0]["deductAmout"]), out advPaidAmt);
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colCnts].Text = Convert.ToString(advPaidAmt);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colCnts].HorizontalAlign = HorizontalAlign.Right;
                                    if (!grandtotal.ContainsKey(colCnts))
                                        grandtotal.Add(colCnts, Convert.ToString(advPaidAmt));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(grandtotal[colCnts]), out amount);
                                        amount += advPaidAmt;
                                        grandtotal.Remove(colCnts);
                                        grandtotal.Add(colCnts, Convert.ToString(amount));
                                    }
                                    //install amount
                                    double insBalAmt = 0;
                                    int.TryParse(Convert.ToString(htCol["Inst Deduct Amt" + feecat]), out colCnts);
                                    if (ds.Tables[5].Rows.Count > 0)
                                    {
                                        ds.Tables[5].DefaultView.RowFilter = " batch_year='" + Chklst_batch.Items[batch].Value + "' and degree_code='" + chklst_branch.Items[deg].Value + "' and feecategory='" + feecat + "'";
                                        DataView dvadv = ds.Tables[5].DefaultView;
                                        if (dvadv.Count > 0)
                                            double.TryParse(Convert.ToString(dvadv[0]["deductAmout"]), out insBalAmt);
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colCnts].Text = Convert.ToString(insBalAmt);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colCnts].HorizontalAlign = HorizontalAlign.Right;
                                    if (!grandtotal.ContainsKey(colCnts))
                                        grandtotal.Add(colCnts, Convert.ToString(insBalAmt));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(grandtotal[colCnts]), out amount);
                                        amount += insBalAmt;
                                        grandtotal.Remove(colCnts);
                                        grandtotal.Add(colCnts, Convert.ToString(amount));
                                    }

                                    //total deduction amount
                                    double totalConsAmt = 0;
                                    int.TryParse(Convert.ToString(htCol["Total Deduction Amt" + feecat]), out colCnts);
                                    if (ds.Tables[6].Rows.Count > 0)
                                    {
                                        double temptotalConsAmt = 0;
                                        ds.Tables[6].DefaultView.RowFilter = " batch_year='" + Chklst_batch.Items[batch].Value + "' and degree_code='" + chklst_branch.Items[deg].Value + "' and feecategory='" + feecat + "'";
                                        DataView dvadv = ds.Tables[6].DefaultView;
                                        if (dvadv.Count > 0)
                                        {
                                            for (int i = 0; i < dvadv.Count; i++)
                                            {
                                                double.TryParse(Convert.ToString(dvadv[i]["deductAmout"]), out temptotalConsAmt);
                                                totalConsAmt += temptotalConsAmt;
                                            }
                                        }
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colCnts].Text = Convert.ToString(totalConsAmt);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colCnts].HorizontalAlign = HorizontalAlign.Right;
                                    if (!grandtotal.ContainsKey(colCnts))
                                        grandtotal.Add(colCnts, Convert.ToString(totalConsAmt));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(grandtotal[colCnts]), out amount);
                                        amount += totalConsAmt;
                                        grandtotal.Remove(colCnts);
                                        grandtotal.Add(colCnts, Convert.ToString(amount));
                                    }
                                    boolDeg = true;
                                    #endregion
                                }
                            }
                            if (boolDeg)
                            {
                                #region
                                FpSpread1.Sheets[0].Rows.Count++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = "Total";
                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].Rows.Count - 1, 0, 1, 2);
                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                                double grandvalue = 0;
                                for (int j = 2; j < FpSpread1.Sheets[0].ColumnCount; j++)
                                {
                                    double.TryParse(Convert.ToString(grandtotal[j]), out grandvalue);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalue);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j].HorizontalAlign = HorizontalAlign.Right;
                                    if (!fnlgrandtotal.ContainsKey(j))
                                        fnlgrandtotal.Add(j, Convert.ToString(grandvalue));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(fnlgrandtotal[j]), out amount);
                                        amount += grandvalue;
                                        fnlgrandtotal.Remove(j);
                                        fnlgrandtotal.Add(j, Convert.ToString(amount));
                                    }
                                }
                                grandtotal.Clear();
                                #endregion
                            }
                        }
                    }
                }
            }
            #endregion

            #region grandtot
            // FpSpread1.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
            FpSpread1.Sheets[0].Rows.Count++;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].Rows.Count - 1, 0, 1, 2);
            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
            double grandvalues = 0;
            for (int j = 2; j < FpSpread1.Sheets[0].ColumnCount; j++)
            {
                double.TryParse(Convert.ToString(fnlgrandtotal[j]), out grandvalues);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
               FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j].HorizontalAlign = HorizontalAlign.Right;
            }
            #endregion

            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            FpSpread1.Visible = true;
            lblrptname.Visible = true;
            txtexcelname.Visible = true;
            btnxl.Visible = true;
            btnmasterprint.Visible = true;
        }
        catch { }
    }


    protected void btnmasterprint_Click(object sender, EventArgs e)
    {
        string degreedetails = "Receipt of Standard Rates";
        Printcontrol.loadspreaddetails(FpSpread1, "FinanceConcessionReport.aspx", degreedetails);
        Printcontrol.Visible = true;
    }
    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text.ToString().Trim();
            if (reportname != "")
            {
                da.printexcelreport(FpSpread1, reportname);
                errmsg.Visible = false;
            }
            else
            {
                errmsg.Text = "Please Enter Your Report Name";
                errmsg.Visible = true;
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
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

        lbl.Add(lblcollege);
        lbl.Add(lbltype);
        lbl.Add(Ibldegree);
        lbl.Add(Iblbranch);
        lbl.Add(lblcategory);
        fields.Add(0);
        fields.Add(1);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

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

    // last modified 04-10-2016 sudhagar
}