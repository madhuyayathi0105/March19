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

public partial class Finance_Reconsilation : System.Web.UI.Page
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
                    loadtype();
                    bindbatch();
                    binddegree();
                    bindbranch();
                    loadfinanceyear();
                    loadheader();
                    loadledger();
                    loadfeecategory();
                }

                string grouporusercode = "";
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                }
                else
                {
                    grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                }

                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                string Master1 = "select * from Master_Settings where " + grouporusercode + "";
                DataSet dsmastersetting = da.select_method_wo_parameter(Master1, "text");
                if (dsmastersetting.Tables[0].Rows.Count > 0)
                {
                    for (int mas = 0; mas < dsmastersetting.Tables[0].Rows.Count; mas++)
                    {
                        if (dsmastersetting.Tables[0].Rows[mas]["settings"].ToString() == "Roll No" && dsmastersetting.Tables[0].Rows[mas]["value"].ToString() == "1")
                        {
                            Session["Rollflag"] = "1";
                        }
                        if (dsmastersetting.Tables[0].Rows[mas]["settings"].ToString() == "Register No" && dsmastersetting.Tables[0].Rows[mas]["value"].ToString() == "1")
                        {
                            Session["Regflag"] = "1";
                        }
                        if (dsmastersetting.Tables[0].Rows[mas]["settings"].ToString() == "Student_Type" && dsmastersetting.Tables[0].Rows[mas]["value"].ToString() == "1")
                        {
                            Session["Studflag"] = "1";
                        }
                    }
                }
            }
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
            //Chklst_batch.Items.Clear();
            //Chk_batch.Checked = false;
            //txt_batch.Text = "---Select---";
            //ds = da.select_method_wo_parameter("bind_batch", "sp");
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    Chklst_batch.DataSource = ds;
            //    Chklst_batch.DataTextField = "batch_year";
            //    Chklst_batch.DataValueField = "batch_year";
            //    Chklst_batch.DataBind();
            //    for (int i = 0; i < Chklst_batch.Items.Count; i++)
            //    {
            //        Chklst_batch.Items[i].Selected = true;
            //        count++;
            //    }
            //    if (count > 0)
            //    {
            //        txt_batch.Text = "Batch(" + (Chklst_batch.Items.Count) + ")";
            //        if (Chklst_batch.Items.Count == count)
            //        {
            //            Chk_batch.Checked = true;
            //        }
            //    }
            Chk_batch.Checked = false;
            txt_batch.Text = "---Select---";
            Chklst_batch.Items.Clear();
            int year = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
            for (int i = year; i >= 2010; i--)
            {
                Chklst_batch.Items.Add(Convert.ToString(i));
            }
            if (Chklst_batch.Items.Count > 0)
            {
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
            if (ds.Tables[0].Rows.Count > 0)
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
                    txttype.Text = "Type (" + count + ")";
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
                txt_degree.Text = Ibldegree.Text+"" + "(" + Chklst_degree.Items.Count + ")";
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
                    txt_branch.Text = Iblbranch.Text+"(" + (chklst_branch.Items.Count) + ")";
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
                //  string straccheadquery = "select distinct a.header_id,a.header_name from chlheadersettings c,Acctheader a where c.Header_ID=a.header_id and a.header_name not in ('arrear') " + typeval + " " + strheadid + "";
                //string straccheadquery = "select distinct a.header_id,a.header_name from Acctheader a where a.header_name not in ('arrear') " + strheadid + " " + typeval + "";
                string straccheadquery = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + ddlcollege.SelectedItem.Value + "  ";
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
                    string strled = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='0' and L.CollegeCode = " + ddlcollege.SelectedItem.Value + "  and L.HeaderFK in('" + chklsheader.Items[i].Value.ToString() + "')  order by isnull(l.priority,1000), l.ledgerName asc ";
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
            //  string getfinanceyear = "select convert(nvarchar(15),af.finyear_start,103) sdate,convert(nvarchar(15),af.finyear_end,103) edate,af.acct_id from account_info af,acctinfo a where a.acct_id=af.acct_id and a.college_code='" + collegecode + "' order by af.acct_id";
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

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        loadfinanceyear();
        loadtype();
        bindbatch();
        binddegree();
        bindbranch();
        loadheader();
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
                txttype.Text = "Type (" + (chklstype.Items.Count) + ")";
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
            //loadheader();
            //loadledger();
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
                txttype.Text = "Type (" + count + ")";
                if (count == chklstype.Items.Count)
                {
                    chktype.Checked = true;
                }
            }
            loadheader();
            bindbatch();
            binddegree();
            bindbranch();
            //loadheader();
            //loadledger();
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
    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
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
            int hcount = 0;
            string actidquery = "";
            for (int i = 0; i < chklsfyear.Items.Count; i++)
            {
                if (chklsfyear.Items[i].Selected == true)
                {
                    hcount++;
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

            Dictionary<string, string> dicfeecat = new Dictionary<string, string>();

            string feecatquery = "";
            string balancefeecatqurey = "";
            for (int i = 0; i < chklscategory.Items.Count; i++)
            {
                if (chklscategory.Items[i].Selected == true)
                {
                    string feecat = chklscategory.Items[i].Value.ToString();
                    if (feecatquery == "")
                    {
                        feecatquery = feecat;
                    }
                    else
                    {
                        feecatquery = feecatquery + "," + feecat;
                    }
                }
                else
                {
                    string feecat = chklscategory.Items[i].Value.ToString();
                    if (balancefeecatqurey == "")
                    {
                        balancefeecatqurey = feecat;
                    }
                    else
                    {
                        balancefeecatqurey = balancefeecatqurey + "," + feecat;
                    }
                }
            }
            if (feecatquery.Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Category And The Proceed";
                return;
            }

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
            FpSpread1.Sheets[0].ColumnCount = 7;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
            FpSpread1.Sheets[0].RowCount = 0;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            FpSpread1.Sheets[0].Columns[0].Width = 30;
            FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Part I";
            FpSpread1.Sheets[0].Columns[1].Width = 200;
            FpSpread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "AMOUNT";
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 1, 2);
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 2].Text = "Rs.P.";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 3].Text = "Rs.P.";
            FpSpread1.Sheets[0].Columns[2].Width = 100;
            FpSpread1.Sheets[0].Columns[3].Width = 100;
            FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
            FpSpread1.Sheets[0].Columns[4].Width = 30;
            FpSpread1.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Part II";
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 2);
            FpSpread1.Sheets[0].Columns[5].Width = 250;
            FpSpread1.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "AMOUNT";
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 6].Text = "Rs.P.";
            FpSpread1.Sheets[0].Columns[6].Width = 100;
            FpSpread1.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;


            FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            Boolean stuflag = false;

            //  string strfeequery1 = "select SUM(fee_amount) from fee_allot a,acctheader h,Registration r where a.Header_ID = h.header_id and a.roll_admit=r.Roll_Admit and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and a.Header_ID in(" + headercode + ") and a.fee_code in(" + feecodequery + ") and a.fee_category in(" + feecatquery + ") and h.acct_id in(" + actidquery + ")";
            string strfeequery1 = " select SUM(isnull(TotalAmount,'0')-isnull(PaidAmount,'0')) from FT_FeeAllot f,Registration r where f.App_No=r.App_No and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and f.HeaderFK in(" + headercode + ") and f.LedgerFK in(" + feecodequery + ") and f.FeeCategory in(" + feecatquery + ") and FinYearFK in(" + actidquery + ")";
            DataSet ds1 = da.select_method_wo_parameter(strfeequery1, "text");

            Double amount1 = 0;
            if (ds1.Tables[0].Rows.Count > 0)
            {
                //  amount1 = Convert.ToDouble(ds1.Tables[0].Rows[0][0]);
                double.TryParse(Convert.ToString(ds1.Tables[0].Rows[0][0]), out amount1);
            }

            // string strfeequery2 = "select isnull(SUM(credit),0) from dailytransaction a,acctheader h,Registration r where a.Header_ID = h.header_id and a.name=r.Roll_No+'-'+r.Stud_Name and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and a.Header_ID in(" + headercode + ") and a.fee_code in(" + feecodequery + ") and a.fee_category in(" + feecatquery + ") and h.acct_id in(" + actidquery + ")";
            string strfeequery2 = " select isnull(SUM(Debit),0) from FT_FinDailyTransaction f,Registration r where f.App_No=r.App_No and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and f.HeaderFK in(" + headercode + ") and f.LedgerFK in(" + feecodequery + ") and f.FeeCategory in(" + feecatquery + ") and FinYearFK in(" + actidquery + ")";
            DataSet ds2 = da.select_method_wo_parameter(strfeequery2, "text");

            Double amount2 = 0;
            if (ds2.Tables[0].Rows.Count > 0)
            {
                //  amount2 = Convert.ToDouble(ds2.Tables[0].Rows[0][0].ToString());
                double.TryParse(Convert.ToString(ds2.Tables[0].Rows[0][0]), out amount2);
            }

            //  Double allotstart = amount1 - amount2;
            double allotstart = amount1;


            // string strrefunablequery = "select isnull(SUM(balance),0) from refund_status a,acctheader h,Registration r where a.header_id = h.header_id and a.roll_admit=r.Roll_Admit and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and a.Header_ID in(" + headercode + ") and a.fee_code in(" + feecodequery + ") and a.fee_category in(" + feecatquery + ") and h.acct_id in(" + actidquery + ")";
            // string strrefunablequery = " select SUM(RefundAmount) from FT_FeeAllot f,Registration r where f.App_No=r.App_No and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and f.HeaderFK in(" + headercode + ") and f.LedgerFK in(" + feecodequery + ") and f.FeeCategory in(" + feecatquery + ") and FinYearFK in(" + actidquery + ")";
            string strrefunablequery = " select SUM(ISNULL(d.ExcessAmt,'0')-ISNULL(d.adjamt,'0')) from FT_FeeAllot f,Registration r, FT_ExcessDet d,FT_ExcessLedgerDet dt where f.App_No=r.App_No and f.App_No=d.App_No and d.ExcessDetPK=dt.ExcessDetFK and f.HeaderFK=dt.HeaderFK and f.LedgerFK=dt.LedgerFK and f.FeeCategory=dt.FeeCategory and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and f.HeaderFK in (" + headercode + ") and f.LedgerFK in(" + feecodequery + ") and f.FeeCategory in(" + feecatquery + ") and f.FinYearFK in(" + actidquery + ")";
            DataSet dsrefunable = da.select_method_wo_parameter(strrefunablequery, "text");
            Double refunabled = 0;
            if (dsrefunable.Tables[0].Rows.Count > 0)
            {
                // refunabled = Convert.ToDouble(dsrefunable.Tables[0].Rows[0][0].ToString());
                double.TryParse(Convert.ToString(dsrefunable.Tables[0].Rows[0][0]), out refunabled);
            }


            //   string strrefund = "select isnull(SUM(amount_paid),0) from refund_status a,acctheader h,Registration r where a.header_id = h.header_id and a.roll_admit=r.Roll_Admit and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and a.Header_ID in(" + headercode + ") and a.fee_code in(" + feecodequery + ") and a.fee_category in(" + feecatquery + ") and h.acct_id in(" + actidquery + ")";
            string strrefund = " select SUM(ISNULL(d.adjamt,'0')) from FT_FeeAllot f,Registration r, FT_ExcessDet d,FT_ExcessLedgerDet dt where f.App_No=r.App_No and f.App_No=d.App_No and d.ExcessDetPK=dt.ExcessDetFK and f.HeaderFK=dt.HeaderFK and f.LedgerFK=dt.LedgerFK and f.FeeCategory=dt.FeeCategory and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and f.HeaderFK in (" + headercode + ") and f.LedgerFK in(" + feecodequery + ") and f.FeeCategory in(" + feecatquery + ") and f.FinYearFK in(" + actidquery + ")";
            DataSet dsrefund = da.select_method_wo_parameter(strrefund, "text");
            Double refund = 0;
            if (dsrefund.Tables[0].Rows.Count > 0)
            {
                // refund = Convert.ToDouble(dsrefund.Tables[0].Rows[0][0].ToString());
                double.TryParse(Convert.ToString(dsrefund.Tables[0].Rows[0][0]), out refund);
            }

            Double refundtotal = refunabled + refund;

            Double arrearfee = 0;
            if (balancefeecatqurey.Trim() != "")
            {
                //  string strarrearfees = "select isnull(SUM(credit),0) from dailytransaction a,acctheader h,Registration r where a.Header_ID = h.header_id and a.name=r.Roll_No+'-'+r.Stud_Name  and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and a.Header_ID in(" + headercode + ") and a.fee_code in(" + feecodequery + ") and a.fee_category in(" + balancefeecatqurey + ") and h.acct_id in(" + actidquery + ")";
                string strarrearfees = " select isnull(SUM(Debit),0) from FT_FinDailyTransaction f,Registration r where f.App_No=r.App_No and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and f.HeaderFK in(" + headercode + ") and f.LedgerFK in(" + feecodequery + ") and f.FeeCategory in(" + balancefeecatqurey + ") and FinYearFK in(" + actidquery + ")";
                DataSet dsarrearfee = da.select_method_wo_parameter(strarrearfees, "text");
                if (dsarrearfee.Tables[0].Rows.Count > 0)
                {
                    //  arrearfee = Convert.ToDouble(dsarrearfee.Tables[0].Rows[0][0].ToString());
                    double.TryParse(Convert.ToString(dsarrearfee.Tables[0].Rows[0][0]), out arrearfee);
                }
            }

            #region design
            FpSpread1.Sheets[0].RowCount = 7;

            FpSpread1.Sheets[0].Cells[0, 0].Text = "A";
            FpSpread1.Sheets[0].Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].Cells[0, 1].Text = "Fees Due at Standard Rate (As Per Fee Return)";
            FpSpread1.Sheets[0].Cells[0, 3].Text = allotstart.ToString();
            FpSpread1.Sheets[0].Cells[0, 3].HorizontalAlign = HorizontalAlign.Right;

            FpSpread1.Sheets[0].Cells[0, 5].Text = "Reasons for the difference explained :";

            FpSpread1.Sheets[0].Cells[1, 0].Text = "C";
            FpSpread1.Sheets[0].Cells[1, 0].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].Cells[1, 1].Text = "Actual Fee Collection";
            FpSpread1.Sheets[0].Cells[1, 2].Text = amount2.ToString();
            FpSpread1.Sheets[0].Cells[1, 2].HorizontalAlign = HorizontalAlign.Right;

            FpSpread1.Sheets[0].Cells[2, 1].Text = "a) Less Amount of arrears of previous year collected";
            FpSpread1.Sheets[0].Cells[2, 2].Text = arrearfee.ToString();
            FpSpread1.Sheets[0].Cells[2, 2].HorizontalAlign = HorizontalAlign.Right;

            Double balance = amount2 - arrearfee;
            FpSpread1.Sheets[0].Cells[3, 1].Text = "Balance";
            FpSpread1.Sheets[0].Cells[3, 2].Text = balance.ToString();
            FpSpread1.Sheets[0].Cells[3, 2].HorizontalAlign = HorizontalAlign.Right;

            FpSpread1.Sheets[0].Cells[4, 1].Text = "b) Less";

            FpSpread1.Sheets[0].Cells[5, 1].Text = "Amount Refundable : " + refunabled;
            //FpSpread1.Sheets[0].Cells[5, 2].HorizontalAlign = HorizontalAlign.Right;

            FpSpread1.Sheets[0].Cells[6, 1].Text = "Amount Refunded : " + refund;
            //FpSpread1.Sheets[0].Cells[6, 1].HorizontalAlign = HorizontalAlign.Right;

            FpSpread1.Sheets[0].Cells[6, 2].Text = refundtotal.ToString();
            //FpSpread1.Sheets[0].Cells[7, 2].Font.Bold = true;
            //FpSpread1.Sheets[0].Cells[7, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Cells[6, 2].HorizontalAlign = HorizontalAlign.Right;

            Double finalbalnce = balance - refundtotal;
            FpSpread1.Sheets[0].Cells[6, 3].Text = finalbalnce.ToString();
            FpSpread1.Sheets[0].Cells[6, 3].HorizontalAlign = HorizontalAlign.Right;

            FpSpread1.Sheets[0].Cells[1, 4].Text = "I";
            FpSpread1.Sheets[0].Cells[1, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Cells[1, 5].Text = "Amount of loss of fee income foregone by the Management on account of the award of fee concessions to the students of :";
            #endregion
            int rowval = 1;
            int sno = 0;
            //  string strconcessionquery = "select textval,isnull(SUM(deduct),0) from fee_allot a,acctheader h,textvaltable  t,Registration r where a.Header_ID = h.header_id and r.Roll_Admit=a.roll_admit and cast(a.dedect_reason as nvarchar(30)) = cast(t.TextCode as nvarchar(30))  group by TextCode,textval";
            string strconcessionquery = " select textval,isnull(SUM(DeductAmout),0) from FT_FeeAllot f,Registration r,textvaltable t where f.App_No=r.App_No and f.DeductReason=t.TextCode and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and f.HeaderFK in(" + headercode + ") and f.LedgerFK in(" + feecodequery + ") and f.FeeCategory in(" + feecatquery + ") and FinYearFK in(" + actidquery + ")  group by TextCode,textval";
            DataSet dsconcession = da.select_method_wo_parameter(strconcessionquery, "text");
            for (int d = 0; d < dsconcession.Tables[0].Rows.Count; d++)
            {
                rowval++;
                if (rowval > FpSpread1.Sheets[0].RowCount - 1)
                {
                    FpSpread1.Sheets[0].RowCount++;
                }
                sno++;
                FpSpread1.Sheets[0].Cells[rowval, 4].Text = sno.ToString();
                FpSpread1.Sheets[0].Cells[rowval, 4].HorizontalAlign = HorizontalAlign.Center;

                FpSpread1.Sheets[0].Cells[rowval, 5].Text = dsconcession.Tables[0].Rows[d]["textval"].ToString();
                FpSpread1.Sheets[0].Cells[rowval, 6].Text = dsconcession.Tables[0].Rows[d][1].ToString();
                FpSpread1.Sheets[0].Cells[rowval, 6].HorizontalAlign = HorizontalAlign.Right;
            }

            int arrow = 6;
            if (FpSpread1.Sheets[0].RowCount > 7)
            {
                FpSpread1.Sheets[0].RowCount++;
                arrow = FpSpread1.Sheets[0].RowCount - 1;
            }

            //FpSpread1.Sheets[0].Cells[arrow, 4].Text = "II";
            //FpSpread1.Sheets[0].Cells[arrow, 4].HorizontalAlign = HorizontalAlign.Center;
            // FpSpread1.Sheets[0].Cells[arrow, 5].Text = "Arrears to be collected";

            FpSpread1.Sheets[0].RowCount++;
            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.LightGray;
            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Font.Name = "Book Antiqua";

            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "TOTAL";
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 3);
            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Visible = false;

            Double part1balnce = allotstart - finalbalnce;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = part1balnce.ToString();
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Bold = true;


            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = "TOTAL";
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Bold = true;
            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 4, 1, 2);

            FpSpread1.Visible = true;
            lblrptname.Visible = true;
            txtexcelname.Visible = true;
            btnxl.Visible = true;
            btnmasterprint.Visible = true;

            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void btnmasterprint_Click(object sender, EventArgs e)
    {
        string degreedetails = "Finance Reconsilation Report";
        Printcontrol.loadspreaddetails(FpSpread1, "Refund Report.aspx", degreedetails);
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

    // last modified 04-10-2016 sudhagar
}