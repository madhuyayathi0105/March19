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

public partial class Concession_Report : System.Web.UI.Page
{
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    string group_user = "", singleuser = "", usercode = "", collegecode = "", group_code = "";
    int count = 0;
    Hashtable has = new Hashtable();
    static TreeNode node;
    TreeNode subchildnode;
    static byte roll = 0;
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
                rbconsolidate.Checked = true;

                string grouporusercode = "";
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                }
                else
                {
                    grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                }

                //Session["Rollflag"] = "0";
                //Session["Regflag"] = "0";
                //Session["Studflag"] = "0";
                //string Master1 = "select * from Master_Settings where " + grouporusercode + "";
                //DataSet dsmastersetting = da.select_method_wo_parameter(Master1, "text");
                //if (dsmastersetting.Tables[0].Rows.Count > 0)
                //{
                //    for (int mas = 0; mas < dsmastersetting.Tables[0].Rows.Count; mas++)
                //    {
                //        if (dsmastersetting.Tables[0].Rows[mas]["settings"].ToString() == "Roll No" && dsmastersetting.Tables[0].Rows[mas]["value"].ToString() == "1")
                //        {
                //            Session["Rollflag"] = "1";
                //        }
                //        if (dsmastersetting.Tables[0].Rows[mas]["settings"].ToString() == "Register No" && dsmastersetting.Tables[0].Rows[mas]["value"].ToString() == "1")
                //        {
                //            Session["Regflag"] = "1";
                //        }
                //        if (dsmastersetting.Tables[0].Rows[mas]["settings"].ToString() == "Student_Type" && dsmastersetting.Tables[0].Rows[mas]["value"].ToString() == "1")
                //        {
                //            Session["Studflag"] = "1";
                //        }
                //    }
                //}

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

                    loadfinanceyear();
                    loadtype();
                    bindbatch();
                    binddegree();
                    bindbranch();
                    loadheader();
                    loadledger();
                    loadfeecategory();
                    loadconcession();
                    RollAndRegSettings();
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
        //try
        //{
        //    
        //    Chklst_batch.Items.Clear();
        //    Chk_batch.Checked = false;
        //    txt_batch.Text = "---Select---";
        //    ds = da.select_method_wo_parameter("bind_batch", "sp");
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        Chklst_batch.DataSource = ds;
        //        Chklst_batch.DataTextField = "batch_year";
        //        Chklst_batch.DataValueField = "batch_year";
        //        Chklst_batch.DataBind();
        //        for (int i = 0; i < Chklst_batch.Items.Count; i++)
        //        {
        //            Chklst_batch.Items[i].Selected = true;
        //            count++;
        //        }
        //        if (count > 0)
        //        {
        //            txt_batch.Text = "Batch(" + (Chklst_batch.Items.Count) + ")";
        //            if (Chklst_batch.Items.Count == count)
        //            {
        //                Chk_batch.Checked = true;
        //            }
        //        }
        //    }
        //}
        //catch (Exception ex)
        //{
        //    errmsg.Visible = true;
        //    errmsg.Text = ex.ToString();
        //}
        count = 0;
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
                    //chklsfyear.Items.Insert(fdatye, ds.Tables[0].Rows[i]["acct_id"].ToString());
                    chklsfyear.Items.Insert(0, new System.Web.UI.WebControls.ListItem(fdatye, actid));
                }
                //chklsfyear.DataSource = ds;
                //chklsfyear.DataTextField = "header_name";
                //chklsfyear.DataValueField = "header_id";
                //chklsfyear.DataBind();

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
                //string straccheadquery = "select distinct a.header_id,a.header_name from Acctheader a where a.header_name not in ('arrear') " + strheadid + "";
                string straccheadquery = " SELECT HeaderPK,HeaderName,hd_priority FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + ddlcollege.SelectedItem.Value + " order by len(isnull(hd_priority,10000)),hd_priority asc ";
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
                    loadledger();
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
        loadfeecategory();
        loadconcession();
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
            loadheader();
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
    //protected void txtfromdate_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        clear();
    //        string fdate = txtfromdate.Text;
    //        string[] spf = fdate.Split('/');
    //        DateTime dtf = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);

    //        string tdate = txttodate.Text.ToString();
    //        string[] spt = tdate.Split('/');
    //        DateTime dtt = Convert.ToDateTime(spt[1] + '/' + spt[0] + '/' + spt[2]);

    //        if (dtt < dtf)
    //        {
    //            txtfromdate.Text = tdate;
    //            errmsg.Visible = true;
    //            errmsg.Text = "To Date Must Be Greater Than From Date";
    //            return;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        errmsg.Visible = true;
    //        errmsg.Text = ex.ToString();
    //    }
    //}

    //protected void txttodate_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        clear();
    //        string fdate = txtfromdate.Text;
    //        string[] spf = fdate.Split('/');
    //        DateTime dtf = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);

    //        string tdate = txttodate.Text.ToString();
    //        string[] spt = tdate.Split('/');
    //        DateTime dtt = Convert.ToDateTime(spt[1] + '/' + spt[0] + '/' + spt[2]);

    //        if (dtt < dtf)
    //        {
    //            txtfromdate.Text = tdate;
    //            errmsg.Visible = true;
    //            errmsg.Text = "To Date Must Be Greater Than From Date";
    //            return;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        errmsg.Visible = true;
    //        errmsg.Text = ex.ToString();
    //    }
    //}

    protected void rbconsolidate_checkdchange(object sender, EventArgs e)
    {
        cbStaff.Enabled = false;
        lbldisp.Visible = false;
        lbldisp.Text = string.Empty;
        lblval.Text = string.Empty;
        fldPayType.Visible = false;
        clear();
    }
    protected void rbdetailed_checkdchange(object sender, EventArgs e)
    {
        cbStaff.Enabled = true;
        lbldisp.Visible = false;
        lbldisp.Text = string.Empty;
        lblval.Text = string.Empty;
        fldPayType.Visible = true;
        clear();
    }

    protected void rbledger_checkdchange(object sender, EventArgs e)
    {

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


            string feecategotyquery = "";
            for (int b = 0; b < chklscategory.Items.Count; b++)
            {
                if (chklscategory.Items[b].Selected == true)
                {
                    if (feecategotyquery == "")
                    {
                        feecategotyquery = chklscategory.Items[b].Value.ToString();
                    }
                    else
                    {
                        feecategotyquery = feecategotyquery + "," + chklscategory.Items[b].Value.ToString();
                    }
                }
            }
            if (feecategotyquery.Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Fee Category And Then Proceed";
                return;
            }

            int noofconces = 0;
            string deductionquery = "";
            for (int b = 0; b < chklsconcession.Items.Count; b++)
            {
                if (chklsconcession.Items[b].Selected == true)
                {
                    noofconces++;
                    if (deductionquery == "")
                    {
                        deductionquery = "'" + chklsconcession.Items[b].Value.ToString() + "'";
                    }
                    else
                    {
                        deductionquery = deductionquery + ",'" + chklsconcession.Items[b].Value.ToString() + "'";
                    }
                }
            }
            if (deductionquery.Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Concession And Then Proceed";
                return;
            }

            string fnlYr = Convert.ToString(getCblSelectedValue(chklsfyear));
            if (fnlYr.Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Financial Year And Then Proceed";
                return;
            }
            string collegecode = Convert.ToString(ddlcollege.SelectedValue);
            string AllotpayType = "";
            if (rblPayType.SelectedIndex==0)            
                AllotpayType = " and isnull(paidamount,'0')<>'0'";            
            else if (rblPayType.SelectedIndex == 1)
                AllotpayType = " and isnull(paidamount,'0')='0' and isnull(balamount,'0')<>'0'";
            
            //string fdate = txtfromdate.Text;
            //string[] spf = fdate.Split('/');
            //DateTime dtf = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);

            //string tdate = txttodate.Text.ToString();
            //string[] spt = tdate.Split('/');
            //DateTime dtt = Convert.ToDateTime(spt[1] + '/' + spt[0] + '/' + spt[2]);

            //if (dtt < dtf)
            //{
            //    txtfromdate.Text = tdate;
            //    errmsg.Visible = true;
            //    errmsg.Text = "To Date Must Be Greater Than From Date";
            //    return;
            //}

            #endregion

            #region design
            Boolean stuflag = false;

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

            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            #endregion
            RollAndRegSettings();
            if (rbconsolidate.Checked == true)
            {
                #region consolidate
                FpSpread1.Sheets[0].ColumnCount = 2;
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                FpSpread1.Sheets[0].RowCount = 0;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = Ibldegree.Text;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);

                // FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);

                FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;


                string strdegree = "select distinct c.type,c.Edu_Level,c.Course_Name,de.Dept_Name,r.Degree_Code,r.Batch_Year from Degree d,Department de,Course c,Registration r where d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and r.degree_code=d.Degree_Code and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") order by c.type,c.Edu_Level desc,r.Degree_Code,r.Batch_Year desc";
                DataSet dsdegree = da.select_method_wo_parameter(strdegree, "Text");

                string feeallotquery = "select r.Batch_Year,r.degree_code,f.FeeCategory,f.DeductReason,f.DeductAmout,COUNT(f.App_No) as stucount from FT_FeeAllot f,Registration r where f.App_No=r.App_No  and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' and f.DeductAmout>0 and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and f.FeeCategory in(" + feecategotyquery + ") and f.HeaderFK in(" + headercode + ") and f.LedgerFK in(" + feecodequery + ") and f.DeductReason in(" + deductionquery + ",'0') group by  r.Batch_Year,r.degree_code,f.FeeCategory,f.DeductReason,f.DeductAmout order by r.Batch_Year,r.degree_code,f.FeeCategory,f.DeductReason,f.DeductAmout";//and ISNULL(f.DeductReason,'')<>''
                DataSet dsfeeallot = da.select_method_wo_parameter(feeallotquery, "Text");

                Boolean headflag = false;
                noofconces = noofconces * 2;
                noofconces++;
                for (int fs = 0; fs < chklscategory.Items.Count; fs++)
                {
                    if (chklscategory.Items[fs].Selected == true)
                    {
                        string fcate = chklscategory.Items[fs].Text.ToString();
                        string fcatecode = chklscategory.Items[fs].Value.ToString();
                        int stratcolu = FpSpread1.Sheets[0].ColumnCount;
                        noofconces = 0;
                        for (int b = 0; b < chklsconcession.Items.Count; b++)
                        {
                            if (chklsconcession.Items[b].Selected == true)
                            {
                                string concession = chklsconcession.Items[b].Text.ToString();
                                string concode = chklsconcession.Items[b].Value.ToString();
                                dsfeeallot.Tables[0].DefaultView.RowFilter = "FeeCategory='" + fcatecode + "' and DeductReason='" + concode + "'";
                                DataView dvfeeallot = dsfeeallot.Tables[0].DefaultView;
                                if (dvfeeallot.Count > 0)
                                {
                                    headflag = true;
                                    noofconces++;
                                    FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 3;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 3].Text = concession;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 3].Tag = concode;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 3].Note = fcatecode;

                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 2].Text = "Rate";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total";

                                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 3].VerticalAlign = VerticalAlign.Middle;
                                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 2].VerticalAlign = VerticalAlign.Middle;
                                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                                }
                            }
                        }
                        if (noofconces > 0)
                        {
                            noofconces = noofconces * 3;
                            noofconces++;
                            FpSpread1.Sheets[0].ColumnCount++;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total";
                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].BackColor = Color.LightGray;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - noofconces].Text = fcate;
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - noofconces, 1, noofconces);
                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                        }
                    }
                }
                FpSpread1.Sheets[0].ColumnCount++;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Grand Total";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].BackColor = Color.LightGreen;
                FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                Dictionary<string, string> dicttot = new Dictionary<string, string>();
                if (headflag == true)
                {
                    int srno = 0;
                    for (int i = 0; i < dsdegree.Tables[0].Rows.Count; i++)
                    {
                        string batchyear = dsdegree.Tables[0].Rows[i]["Batch_Year"].ToString();
                        string degreecode = dsdegree.Tables[0].Rows[i]["Degree_Code"].ToString();
                        string course = dsdegree.Tables[0].Rows[i]["Course_name"].ToString();
                        string department = dsdegree.Tables[0].Rows[i]["Dept_Name"].ToString();
                        Double totalfee = 0, totalstu = 0;
                        Boolean degreeflag = false;
                        int degreestartrow = 0;
                        Double grantotal = 0;
                        for (int c = 2; c < FpSpread1.Sheets[0].ColumnCount - 1; c++)
                        {
                            if (FpSpread1.Sheets[0].ColumnHeader.Cells[1, c].Text != "Total")
                            {
                                string feecat = FpSpread1.Sheets[0].ColumnHeader.Cells[1, c].Note.ToString();
                                string dedectcode = FpSpread1.Sheets[0].ColumnHeader.Cells[1, c].Tag.ToString();

                                dsfeeallot.Tables[0].DefaultView.RowFilter = "Batch_Year='" + batchyear + "' and degree_code='" + degreecode + "' and FeeCategory='" + feecat + "' and DeductReason='" + dedectcode + "'";
                                DataView dvfeeallot = dsfeeallot.Tables[0].DefaultView;
                                if (dvfeeallot.Count > 0)
                                {
                                    stuflag = true;
                                    if (degreeflag == false)
                                    {
                                        srno++;
                                        FpSpread1.Sheets[0].RowCount++;
                                        FpSpread1.Sheets[0].RowCount++;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Text = srno.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 1].Text = batchyear + " - " + course + " - " + department;
                                        degreeflag = true;
                                        degreestartrow = FpSpread1.Sheets[0].RowCount - 2;

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
                                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 2);
                                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.LightGreen;
                                        //  degreeflag = true;
                                        //degreestartrow = FpSpread1.Sheets[0].RowCount - 1;
                                    }
                                    if (degreeflag == true)
                                    {
                                        int getrow = degreestartrow;
                                        for (int f = 0; f < dvfeeallot.Count; f++)
                                        {
                                            if (f > 0)
                                            {
                                                getrow++;
                                                if (degreestartrow < getrow)
                                                {
                                                    FpSpread1.Sheets[0].RowCount++;
                                                    FpSpread1.Sheets[0].RowCount++;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Text = srno.ToString();
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 1].Text = batchyear + " - " + course + " - " + department;

                                                    //total

                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "";
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "Total";
                                                }
                                            }
                                            Double feededuct = Convert.ToDouble(dvfeeallot[f]["DeductAmout"].ToString());
                                            Double noofstu = Convert.ToDouble(dvfeeallot[f]["stucount"].ToString());

                                            totalstu = totalstu + noofstu;
                                            Double amount = noofstu * feededuct;
                                            totalfee = totalfee + amount;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, c].Text = noofstu.ToString();

                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, c].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, c + 1].Text = feededuct.ToString();

                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, c + 1].HorizontalAlign = HorizontalAlign.Right;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, c + 2].Text = amount.ToString();

                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, c + 2].HorizontalAlign = HorizontalAlign.Right;


                                            //total
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = noofstu.ToString();
                                            //dicttot.Add(Convert.ToString(c), Convert.ToString(noofstu));
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c + 1].Text = feededuct.ToString();
                                            // dicttot.Add(Convert.ToString(c + 1), Convert.ToString(feededuct));
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c + 1].HorizontalAlign = HorizontalAlign.Right;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c + 2].Text = amount.ToString();
                                            //dicttot.Add(Convert.ToString(c + 2), Convert.ToString(amount));
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c + 2].HorizontalAlign = HorizontalAlign.Right;
                                            //dicttot.Add(Convert.ToString(c),Convert.ToString(noofstu));
                                        }
                                        /////
                                    }
                                }
                                c = c + 2;
                            }
                            else
                            {
                                if (degreeflag == true)
                                {
                                    if (totalfee > 0)
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, c].Text = totalfee.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, c].HorizontalAlign = HorizontalAlign.Right;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, c].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, c].Font.Size = FontUnit.Medium;
                                        grantotal = grantotal + totalfee;

                                        //toal
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = totalfee.ToString();
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Right;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Font.Name = "Book Antiqua";
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Font.Size = FontUnit.Medium;
                                        // grantotal = grantotal + totalfee;

                                    }
                                }
                                totalfee = 0;
                            }
                        }
                        if (degreeflag == true)
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 1].Text = grantotal.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;


                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = grantotal.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            grantotal = 0;
                        }

                    }
                }
                if (stuflag == true)
                {
                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = " grand Total";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 2);
                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.Gold;

                    for (int c = 2; c < FpSpread1.Sheets[0].ColumnCount; c++)
                    {
                        Double total = 0;
                        for (int r = 0; r < FpSpread1.Sheets[0].RowCount - 1; r++)
                        {
                            if (r % 2 == 0)
                            {
                                string getfee = FpSpread1.Sheets[0].Cells[r, c].Text.ToString();
                                if (getfee.Trim() != "")
                                {
                                    total = total + Convert.ToDouble(FpSpread1.Sheets[0].Cells[r, c].Text.ToString());
                                }
                            }
                        }
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = total.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Font.Name = "Book Antiqua";
                        string getnam = FpSpread1.Sheets[0].ColumnHeader.Cells[1, c].Text.ToString().Trim().ToLower();
                        if (getnam != "total" && getnam != "rate" && getnam != "")
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Center;
                        }
                        else
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Right;
                        }
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
                #endregion
                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            }
            else if (rbdetailed.Checked == true)
            {
                if (checkSchoolSetting() == 0)//school
                {
                    detailedConsScl(batchquery, degreequery, feecategotyquery, headercode, feecodequery, deductionquery, fnlYr, collegecode, AllotpayType);
                }
                else
                {
                    detailedConsClg(batchquery, degreequery, feecategotyquery, headercode, feecodequery, deductionquery, fnlYr, collegecode, AllotpayType);
                }
                if (false)//not use
                {
                    #region detailed

                    FpSpread1.Sheets[0].ColumnCount = 7;
                    FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                    FpSpread1.Sheets[0].RowCount = 0;

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = Ibldegree.Text;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Admission No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Name";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Concession";
                    //// roll and reg no rights
                    //if (roll == 0)
                    //{
                    //    FpSpread1.Sheets[0].Columns[2].Visible = true;
                    //    FpSpread1.Sheets[0].Columns[3].Visible = true;
                    //}
                    //else if (roll == 1)
                    //{
                    //    FpSpread1.Sheets[0].Columns[2].Visible = true;
                    //    FpSpread1.Sheets[0].Columns[3].Visible = true;
                    //}
                    //else if (roll == 2)
                    //{
                    //    FpSpread1.Sheets[0].Columns[2].Visible = true;
                    //    FpSpread1.Sheets[0].Columns[3].Visible = false;
                    //}
                    //else if (roll == 3)
                    //{
                    //    FpSpread1.Sheets[0].Columns[2].Visible = false;
                    //    FpSpread1.Sheets[0].Columns[3].Visible = true;
                    //}

                    FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpSpread1.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpSpread1.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpSpread1.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpSpread1.Sheets[0].SetColumnMerge(6, FarPoint.Web.Spread.Model.MergePolicy.Always);

                    FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                    FpSpread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
                    FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                    FpSpread1.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
                    FpSpread1.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
                    FpSpread1.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
                    FpSpread1.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;
                    FpSpread1.Sheets[0].Columns[4].Width = 200;
                    FpSpread1.Sheets[0].Columns[1].Width = 150;
                    spreadColumnVisible();
                    //   string strqurey = "select r.Batch_Year,r.degree_code,r.Stud_Name,r.Roll_No,r.Reg_No,f.fee_category,t.TextVal,f.dedect_reason,sum(isnull(f.deduct,'0')) as amount from Registration r,fee_allot f,fee_info l,TextValTable t where r.Roll_Admit=f.roll_admit and f.fee_code=l.fee_code and t.TextCode=f.dedect_reason and f.Header_ID=l.header_id and isnull(f.dedect_reason,'')<>'' and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' and f.deduct>0 ";
                    //  strqurey = strqurey + " and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and f.Header_ID in(" + headercode + ") and f.fee_code in(" + feecodequery + ") and f.fee_category in(" + feecategotyquery + ") and f.dedect_reason in(" + deductionquery + ") group by r.Batch_Year,r.degree_code,r.Stud_Name,r.Roll_No,r.Reg_No,f.fee_category,f.dedect_reason,t.TextVal";
                    string strqurey = " select r.Batch_Year,r.degree_code,r.Stud_Name,r.Roll_No,r.roll_admit,r.Reg_No,f.FeeCategory,t.TextVal,f.DeductReason,SUM(ISNULL(deductAmout,'0')) as amount from FT_FeeAllot f,Registration r,textvaltable t where f.App_No=r.App_No and f.FeeCategory=t.TextCode and ISNULL(f.DeductReason,'')<>'' and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' and f.DeductAmout>0 and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and f.FeeCategory in(" + feecategotyquery + ") and f.HeaderFK in(" + headercode + ") and f.LedgerFK in(" + feecodequery + ") and f.DeductReason in(" + deductionquery + ")  group by   r.Batch_Year,r.degree_code,r.Stud_Name,r.Roll_No,r.Reg_No,f.FeeCategory,f.DeductReason,t.TextVal,r.roll_admit";
                    DataSet dsfeequery = da.select_method_wo_parameter(strqurey, "Text");

                    //  string deductquery = "select distinct r.Batch_Year,r.degree_code,c.Course_Name,de.Dept_Name,r.Stud_Name,r.Roll_No,r.Reg_No from Registration r,fee_allot f,fee_info l,Degree d,Course c,Department de where r.Roll_Admit=f.roll_admit and f.fee_code=l.fee_code and d.Degree_Code=r.degree_code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and f.Header_ID=l.header_id and isnull(f.dedect_reason,'')<>'' and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' and f.deduct>0 ";
                    //   deductquery = deductquery + " and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and f.Header_ID in(" + headercode + ") and f.fee_code in(" + feecodequery + ") and f.fee_category in(" + feecategotyquery + ") and f.dedect_reason in(" + deductionquery + ") order by r.degree_code,r.Batch_Year desc,r.Roll_No";

                    string deductquery = " select r.Batch_Year,r.degree_code,c.Course_Name,dt.Dept_Name,r.Stud_Name,r.roll_admit,r.Roll_No,r.Reg_No from FT_FeeAllot f,Registration r,Degree d,Course c,Department dt where f.App_No=r.App_No and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and ISNULL(f.DeductReason,'')<>'' and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' and f.DeductAmout>0 and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and f.FeeCategory in(" + feecategotyquery + ") and f.HeaderFK in(" + headercode + ") and f.LedgerFK in(" + feecodequery + ") and f.DeductReason in(" + deductionquery + ")  group by   r.Batch_Year,r.degree_code,c.Course_Name,dt.Dept_Name,r.Stud_Name,r.Roll_No,r.Reg_No,r.roll_admit order by  r.degree_code,r.Batch_Year desc, r.Roll_No";
                    DataSet dsstu = da.select_method_wo_parameter(deductquery, "Text");
                    Dictionary<string, string> semdt = new Dictionary<string, string>();
                    for (int fs = 0; fs < chklscategory.Items.Count; fs++)
                    {
                        if (chklscategory.Items[fs].Selected == true)
                        {
                            string fcate = chklscategory.Items[fs].Text.ToString();
                            string fcatecode = chklscategory.Items[fs].Value.ToString();
                            dsfeequery.Tables[0].DefaultView.RowFilter = "FeeCategory='" + fcatecode + "'";
                            DataView dvfeca = dsfeequery.Tables[0].DefaultView;
                            if (dvfeca.Count > 0)
                            {
                                FpSpread1.Sheets[0].ColumnCount++;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = fcate;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = fcatecode;
                                FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                                semdt.Add(Convert.ToString(fcate), Convert.ToString(FpSpread1.Sheets[0].ColumnCount - 1));
                            }
                        }
                    }
                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total";
                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;

                    int srno = 0;
                    for (int i = 0; i < dsstu.Tables[0].Rows.Count; i++)
                    {
                        string rollno = dsstu.Tables[0].Rows[i]["Roll_No"].ToString();
                        string regno = dsstu.Tables[0].Rows[i]["Reg_No"].ToString();
                        string rolladmit = dsstu.Tables[0].Rows[i]["roll_admit"].ToString();
                        string name = dsstu.Tables[0].Rows[i]["Stud_Name"].ToString();
                        string batchyear = dsstu.Tables[0].Rows[i]["Batch_Year"].ToString();
                        string course = dsstu.Tables[0].Rows[i]["Course_Name"].ToString();
                        string department = dsstu.Tables[0].Rows[i]["Dept_Name"].ToString();
                        string degreedetails = batchyear + " - " + course + " - " + department;

                        srno++;
                        FpSpread1.Sheets[0].RowCount++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = txt;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = txt;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].CellType = txt;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = degreedetails.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = rollno.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = regno.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = rolladmit.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = name.ToString();

                        Double totdedcu = 0;
                        int strow = FpSpread1.Sheets[0].RowCount - 1;
                        int noofrow = 1;
                        for (int c = 7; c < FpSpread1.Sheets[0].ColumnCount; c++)
                        {
                            if (FpSpread1.Sheets[0].ColumnHeader.Cells[0, c].Text == "Total")
                            {
                                FpSpread1.Sheets[0].Cells[strow, c].Text = totdedcu.ToString();
                                FpSpread1.Sheets[0].Cells[strow, c].HorizontalAlign = HorizontalAlign.Right;
                                if (noofrow > 1)
                                {
                                    FpSpread1.Sheets[0].SpanModel.Add(strow, c, noofrow, 1);
                                }
                                totdedcu = 0;
                            }
                            else
                            {
                                string feecate = FpSpread1.Sheets[0].ColumnHeader.Cells[0, c].Tag.ToString();
                                dsfeequery.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "' and FeeCategory='" + feecate + "'";
                                DataView dvfeca = dsfeequery.Tables[0].DefaultView;
                                int nextrow = strow;
                                for (int f = 0; f < dvfeca.Count; f++)
                                {
                                    stuflag = true;
                                    if (f > 0)
                                    {
                                        nextrow++;
                                        if (nextrow > FpSpread1.Sheets[0].RowCount - 1)
                                        {
                                            noofrow++;
                                            FpSpread1.Sheets[0].RowCount++;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = degreedetails.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = txt;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = txt;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].CellType = txt;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = rollno.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = regno.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = rolladmit.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = name.ToString();
                                        }
                                    }
                                    string deducreason = dvfeca[f]["TextVal"].ToString();
                                    string deducamou = dvfeca[f]["amount"].ToString();
                                    totdedcu = totdedcu + Convert.ToDouble(deducamou);
                                    string deductname = da.GetFunction("select TextVal from TextValTable where TextCriteria ='DedRe' and college_code ='" + ddlcollege.SelectedItem.Value + "' and TextCode='" + Convert.ToString(dvfeca[f]["DeductReason"]) + "'");
                                    FpSpread1.Sheets[0].Cells[nextrow, 6].Text = deductname;
                                    // FpSpread1.Sheets[0].Cells[nextrow, 6].Text = deducreason.ToString();
                                    FpSpread1.Sheets[0].Cells[nextrow, c].Text = deducamou.ToString();
                                    FpSpread1.Sheets[0].Cells[nextrow, c].HorizontalAlign = HorizontalAlign.Right;
                                }

                            }

                        }
                        //FpSpread1.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    }

                    if (stuflag == true)
                    {
                        FpSpread1.Sheets[0].RowCount++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 7);
                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.LightGreen;

                        for (int c = 7; c < FpSpread1.Sheets[0].ColumnCount; c++)
                        {
                            Double total = 0;
                            for (int r = 0; r < FpSpread1.Sheets[0].RowCount - 1; r++)
                            {
                                string getfee = FpSpread1.Sheets[0].Cells[r, c].Text.ToString();
                                if (getfee.Trim() != "")
                                {
                                    total = total + Convert.ToDouble(getfee);
                                }
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = total.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Right;
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
                    #endregion
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                }
            }
            else
            {
                #region old

                //#region query
                //string strdegree = "select distinct c.type,c.Edu_Level,c.Course_Name,de.Dept_Name,r.Degree_Code,r.Batch_Year from Degree d,Department de,Course c,Registration r where d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and r.degree_code=d.Degree_Code and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") order by c.type,c.Edu_Level desc,r.Degree_Code,r.Batch_Year desc";
                //DataSet dsdegree = da.select_method_wo_parameter(strdegree, "Text");

                //string feeallotquery = " select r.Batch_Year,r.degree_code,f.FeeCategory,f.DeductReason,f.DeductAmout,COUNT(f.App_No) as stucount,,f.LedgerFK from FT_FeeAllot f,Registration r where f.App_No=r.App_No and ISNULL(f.DeductReason,'')<>'' and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' and f.DeductAmout>0 and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and f.FeeCategory in(" + feecategotyquery + ") and f.HeaderFK in(" + headercode + ") and f.LedgerFK in(" + feecodequery + ") and f.DeductReason in(" + deductionquery + ") group by  r.Batch_Year,r.degree_code,f.FeeCategory,f.DeductReason,f.DeductAmout,f.LedgerFK order by r.Batch_Year,r.degree_code,f.FeeCategory,f.DeductReason,f.DeductAmout";
                //DataSet dsfeeallot = da.select_method_wo_parameter(feeallotquery, "Text");
                //#endregion


                //Dictionary<int, string> newhash = new Dictionary<int, string>();
                //FpSpread1.Sheets[0].ColumnCount = 3;
                //FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                //FpSpread1.Sheets[0].RowCount = 0;

                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Degree Details";
                //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);

                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Ledgers";
                //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                //// FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                //FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);

                //FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                //FpSpread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;

                //for (int sem = 0; sem < chklscategory.Items.Count; sem++)
                //{
                //    if (chklscategory.Items[sem].Selected == true)
                //    {
                //        int colcnt = 0;
                //        string feecatname = Convert.ToString(chklscategory.Items[sem].Text);
                //        string feecatvalue = Convert.ToString(chklscategory.Items[sem].Value);
                //        for (int con = 0; con < chklsconcession.Items.Count; con++)
                //        {
                //            if (chklsconcession.Items[con].Selected == true)
                //            {
                //                colcnt++;
                //                FpSpread1.Sheets[0].ColumnCount++;
                //                newhash.Add(Convert.ToInt32(chklsconcession.Items[con].Value),Convert.ToString(FpSpread1.Sheets[0].ColumnCount - 1));
                //                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(chklsconcession.Items[con].Text);
                //                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                //                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                //                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                //                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                //                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                //                FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                //            }
                //        }


                //        if (colcnt > 0)
                //        {
                //           // colcnt = colcnt * 3;
                //           // colcnt++;
                //            FpSpread1.Sheets[0].ColumnCount++;
                //            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total";
                //            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].BackColor = Color.LightGray;
                //            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - ++colcnt].Text = feecatname;
                //            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - colcnt, 1, colcnt);
                //            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                //        }
                //    }
                //}

                //#region values
                //for (int i = 0; i < dsdegree.Tables[0].Rows.Count; i++)
                //{
                //    string batchyear = dsdegree.Tables[0].Rows[i]["Batch_Year"].ToString();
                //    string degreecode = dsdegree.Tables[0].Rows[i]["Degree_Code"].ToString();
                //    string course = dsdegree.Tables[0].Rows[i]["Course_name"].ToString();
                //    string department = dsdegree.Tables[0].Rows[i]["Dept_Name"].ToString();


                //}


                //#endregion

                #endregion
                #region consolidate
                FpSpread1.Sheets[0].ColumnCount = 3;
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                FpSpread1.Sheets[0].RowCount = 0;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = Ibldegree.Text;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Ledgers";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);

                // FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);

                FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;


                string strdegree = "select distinct c.type,c.Edu_Level,c.Course_Name,de.Dept_Name,r.Degree_Code,r.Batch_Year from Degree d,Department de,Course c,Registration r where d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and r.degree_code=d.Degree_Code and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") order by c.type,c.Edu_Level desc,r.Degree_Code,r.Batch_Year desc";
                DataSet dsdegree = da.select_method_wo_parameter(strdegree, "Text");

                string feeallotquery = " select r.Batch_Year,r.degree_code,f.FeeCategory,f.DeductReason,f.DeductAmout,COUNT(f.App_No) as stucount,f.LedgerFK from FT_FeeAllot f,Registration r where f.App_No=r.App_No and ISNULL(f.DeductReason,'')<>'' and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' and f.DeductAmout>0 and r.Batch_Year in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and f.FeeCategory in(" + feecategotyquery + ") and f.HeaderFK in(" + headercode + ") and f.LedgerFK in(" + feecodequery + ") and f.DeductReason in(" + deductionquery + ") group by  r.Batch_Year,r.degree_code,f.FeeCategory,f.DeductReason,f.DeductAmout,f.LedgerFK order by r.Batch_Year,r.degree_code,f.FeeCategory,f.DeductReason,f.DeductAmout";
                DataSet dsfeeallot = da.select_method_wo_parameter(feeallotquery, "Text");

                Boolean headflag = false;
                noofconces = noofconces * 2;
                noofconces++;
                for (int fs = 0; fs < chklscategory.Items.Count; fs++)
                {
                    if (chklscategory.Items[fs].Selected == true)
                    {
                        string fcate = chklscategory.Items[fs].Text.ToString();
                        string fcatecode = chklscategory.Items[fs].Value.ToString();
                        int stratcolu = FpSpread1.Sheets[0].ColumnCount;
                        noofconces = 0;
                        for (int b = 0; b < chklsconcession.Items.Count; b++)
                        {
                            if (chklsconcession.Items[b].Selected == true)
                            {
                                string concession = chklsconcession.Items[b].Text.ToString();
                                string concode = chklsconcession.Items[b].Value.ToString();
                                dsfeeallot.Tables[0].DefaultView.RowFilter = "FeeCategory='" + fcatecode + "' and DeductReason='" + concode + "'";
                                DataView dvfeeallot = dsfeeallot.Tables[0].DefaultView;
                                if (dvfeeallot.Count > 0)
                                {
                                    headflag = true;
                                    noofconces++;
                                    FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 3;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 3].Text = concession;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 3].Tag = concode;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 3].Note = fcatecode;

                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 2].Text = "Rate";
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total";

                                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 3].VerticalAlign = VerticalAlign.Middle;
                                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 2].VerticalAlign = VerticalAlign.Middle;
                                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                                }
                            }
                        }
                        if (noofconces > 0)
                        {
                            noofconces = noofconces * 3;
                            noofconces++;
                            FpSpread1.Sheets[0].ColumnCount++;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total";
                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].BackColor = Color.LightGray;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - noofconces].Text = fcate;
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - noofconces, 1, noofconces);
                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                        }
                    }
                }
                FpSpread1.Sheets[0].ColumnCount++;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Grand Total";
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].BackColor = Color.LightGreen;
                FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                double subtot = 0;
                Dictionary<string, string> dicttot = new Dictionary<string, string>();
                if (headflag == true)
                {
                    int srno = 0;
                    for (int i = 0; i < dsdegree.Tables[0].Rows.Count; i++)
                    {
                        string batchyear = dsdegree.Tables[0].Rows[i]["Batch_Year"].ToString();
                        string degreecode = dsdegree.Tables[0].Rows[i]["Degree_Code"].ToString();
                        string course = dsdegree.Tables[0].Rows[i]["Course_name"].ToString();
                        string department = dsdegree.Tables[0].Rows[i]["Dept_Name"].ToString();
                        Double totalfee = 0, totalstu = 0;
                        Boolean degreeflag = false;
                        int degreestartrow = 0;
                        Double grantotal = 0;
                        for (int remv = 0; remv < treeview_spreadfields.Nodes.Count; remv++)
                        {
                            if (treeview_spreadfields.Nodes[remv].ChildNodes.Count > 0)
                            {
                                for (int j = 0; j < treeview_spreadfields.Nodes[remv].ChildNodes.Count; j++)
                                {
                                    if (treeview_spreadfields.Nodes[remv].ChildNodes[j].Checked == true)
                                    {
                                        for (int c = 3; c < FpSpread1.Sheets[0].ColumnCount - 1; c++)
                                        {
                                            if (FpSpread1.Sheets[0].ColumnHeader.Cells[1, c].Text != "Total")
                                            {
                                                string feecat = FpSpread1.Sheets[0].ColumnHeader.Cells[1, c].Note.ToString();
                                                string dedectcode = FpSpread1.Sheets[0].ColumnHeader.Cells[1, c].Tag.ToString();

                                                dsfeeallot.Tables[0].DefaultView.RowFilter = "Batch_Year='" + batchyear + "' and degree_code='" + degreecode + "' and FeeCategory='" + feecat + "' and DeductReason='" + dedectcode + "' and LedgerFK='" + Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Value) + "'";
                                                DataView dvfeeallot = dsfeeallot.Tables[0].DefaultView;
                                                if (dvfeeallot.Count > 0)
                                                {
                                                    stuflag = true;
                                                    if (degreeflag == false)
                                                    {
                                                        srno++;
                                                        FpSpread1.Sheets[0].RowCount++;
                                                        FpSpread1.Sheets[0].RowCount++;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Text = srno.ToString();
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 1].Text = batchyear + " - " + course + " - " + department;

                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 2].Text = Convert.ToString(treeview_spreadfields.Nodes[remv].ChildNodes[j].Text);
                                                        degreeflag = true;
                                                        degreestartrow = FpSpread1.Sheets[0].RowCount - 2;

                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "";
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
                                                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 2);
                                                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.LightGreen;
                                                        //  degreeflag = true;
                                                        //degreestartrow = FpSpread1.Sheets[0].RowCount - 1;
                                                    }
                                                    if (degreeflag == true)
                                                    {
                                                        int getrow = degreestartrow;
                                                        for (int f = 0; f < dvfeeallot.Count; f++)
                                                        {
                                                            if (f > 0)
                                                            {
                                                                getrow++;
                                                                if (degreestartrow < getrow)
                                                                {
                                                                    FpSpread1.Sheets[0].RowCount++;
                                                                    FpSpread1.Sheets[0].RowCount++;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].Text = srno.ToString();
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, 1].Text = batchyear + " - " + course + " - " + department;

                                                                    //total

                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "";
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "Total";
                                                                }
                                                            }
                                                            Double feededuct = Convert.ToDouble(dvfeeallot[f]["DeductAmout"].ToString());
                                                            Double noofstu = Convert.ToDouble(dvfeeallot[f]["stucount"].ToString());

                                                            totalstu = totalstu + noofstu;
                                                            Double amount = noofstu * feededuct;
                                                            totalfee = totalfee + amount;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, c].Text = noofstu.ToString();

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, c].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, c + 1].Text = feededuct.ToString();

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, c + 1].HorizontalAlign = HorizontalAlign.Right;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, c + 2].Text = amount.ToString();

                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, c + 2].HorizontalAlign = HorizontalAlign.Right;


                                                            //total
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = noofstu.ToString();
                                                            //dicttot.Add(Convert.ToString(c), Convert.ToString(noofstu));
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Center;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c + 1].Text = feededuct.ToString();
                                                            // dicttot.Add(Convert.ToString(c + 1), Convert.ToString(feededuct));
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c + 1].HorizontalAlign = HorizontalAlign.Right;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c + 2].Text = amount.ToString();
                                                            //dicttot.Add(Convert.ToString(c + 2), Convert.ToString(amount));
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c + 2].HorizontalAlign = HorizontalAlign.Right;
                                                            //dicttot.Add(Convert.ToString(c),Convert.ToString(noofstu));
                                                        }
                                                        /////
                                                    }
                                                }
                                                c = c + 2;
                                            }
                                            else
                                            {
                                                if (degreeflag == true)
                                                {
                                                    if (totalfee > 0)
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, c].Text = totalfee.ToString();
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, c].HorizontalAlign = HorizontalAlign.Right;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, c].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, c].Font.Size = FontUnit.Medium;
                                                        grantotal = grantotal + totalfee;
                                                        subtot += totalfee;

                                                        //toal
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = totalfee.ToString();
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Right;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Font.Size = FontUnit.Medium;
                                                        // grantotal = grantotal + totalfee;

                                                    }
                                                }
                                                totalfee = 0;
                                            }
                                        }
                                        if (degreeflag == true)
                                        {
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 2].Text = subtot.ToString();
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Right;
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Text = subtot.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Right;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;
                                            subtot = 0;
                                        }
                                    }
                                }
                            }
                        }
                        if (degreeflag == true)
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 1].Text = grantotal.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = grantotal.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            grantotal = 0;
                        }

                    }
                }
                if (stuflag == true)
                {
                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = " grand Total";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 2);
                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.Gold;

                    for (int c = 3; c < FpSpread1.Sheets[0].ColumnCount; c++)
                    {
                        Double total = 0;
                        for (int r = 0; r < FpSpread1.Sheets[0].RowCount - 1; r++)
                        {
                            if (r % 2 == 0)
                            {
                                string getfee = FpSpread1.Sheets[0].Cells[r, c].Text.ToString();
                                if (getfee.Trim() != "")
                                {
                                    total = total + Convert.ToDouble(FpSpread1.Sheets[0].Cells[r, c].Text.ToString());
                                }
                            }
                        }
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = total.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Font.Name = "Book Antiqua";
                        string getnam = FpSpread1.Sheets[0].ColumnHeader.Cells[1, c].Text.ToString().Trim().ToLower();
                        if (getnam != "total" && getnam != "rate" && getnam != "")
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Center;
                        }
                        else
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Right;
                        }
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
                #endregion
                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            }

        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void btnmasterprint_Click(object sender, EventArgs e)
    {

        string degreedetails = "Concession Report";
        if (rbconsolidate.Checked == true)
        {
            degreedetails = "Concession Fee Consolidated Report";
        }
        else
        {
            degreedetails = "Concession Fee Detailed Report";
        }
        Printcontrol.loadspreaddetails(FpSpread1, "Concession Report.aspx", degreedetails);
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
            if (!rbdetailed.Checked)
            {
                if (roll == 0)
                {
                    FpSpread1.Columns[2].Visible = true;
                    FpSpread1.Columns[3].Visible = true;
                    FpSpread1.Columns[4].Visible = true;
                }
                else if (roll == 1)
                {
                    FpSpread1.Columns[2].Visible = true;
                    FpSpread1.Columns[3].Visible = true;
                    FpSpread1.Columns[4].Visible = true;
                }
                else if (roll == 2)
                {
                    FpSpread1.Columns[2].Visible = true;
                    FpSpread1.Columns[3].Visible = false;
                    FpSpread1.Columns[4].Visible = false;

                }
                else if (roll == 3)
                {
                    FpSpread1.Columns[2].Visible = false;
                    FpSpread1.Columns[3].Visible = true;
                    FpSpread1.Columns[4].Visible = false;
                }
                else if (roll == 4)
                {
                    FpSpread1.Columns[2].Visible = false;
                    FpSpread1.Columns[3].Visible = false;
                    FpSpread1.Columns[4].Visible = true;
                }
                else if (roll == 5)
                {
                    FpSpread1.Columns[2].Visible = true;
                    FpSpread1.Columns[3].Visible = true;
                    FpSpread1.Columns[4].Visible = false;
                }
                else if (roll == 6)
                {
                    FpSpread1.Columns[2].Visible = false;
                    FpSpread1.Columns[3].Visible = true;
                    FpSpread1.Columns[4].Visible = true;
                }
                else if (roll == 7)
                {
                    FpSpread1.Columns[2].Visible = true;
                    FpSpread1.Columns[3].Visible = false;
                    FpSpread1.Columns[4].Visible = true;
                }
            }
            else
            {
                if (roll == 0)
                {
                    FpSpread1.Columns[1].Visible = true;
                    FpSpread1.Columns[2].Visible = true;
                    FpSpread1.Columns[3].Visible = true;
                }
                else if (roll == 1)
                {
                    FpSpread1.Columns[1].Visible = true;
                    FpSpread1.Columns[2].Visible = true;
                    FpSpread1.Columns[3].Visible = true;
                }
                else if (roll == 2)
                {
                    FpSpread1.Columns[1].Visible = true;
                    FpSpread1.Columns[2].Visible = false;
                    FpSpread1.Columns[3].Visible = false;

                }
                else if (roll == 3)
                {
                    FpSpread1.Columns[1].Visible = false;
                    FpSpread1.Columns[2].Visible = true;
                    FpSpread1.Columns[3].Visible = false;
                }
                else if (roll == 4)
                {
                    FpSpread1.Columns[1].Visible = false;
                    FpSpread1.Columns[2].Visible = false;
                    FpSpread1.Columns[3].Visible = true;
                }
                else if (roll == 5)
                {
                    FpSpread1.Columns[1].Visible = true;
                    FpSpread1.Columns[2].Visible = true;
                    FpSpread1.Columns[3].Visible = false;
                }
                else if (roll == 6)
                {
                    FpSpread1.Columns[1].Visible = false;
                    FpSpread1.Columns[2].Visible = true;
                    FpSpread1.Columns[3].Visible = true;
                }
                else if (roll == 7)
                {
                    FpSpread1.Columns[1].Visible = true;
                    FpSpread1.Columns[2].Visible = false;
                    FpSpread1.Columns[3].Visible = true;
                }
            }
        }
        catch { }
    }

    #endregion

    // last modified 28.11.2016 sudhagar
    protected Hashtable htDeduction()
    {
        Hashtable htcons = new Hashtable();
        try
        {
            string SelQ = " select TextVal,textcode from TextValTable where TextCriteria ='DedRe' and college_code ='" + ddlcollege.SelectedItem.Value + "'";
            DataSet dsVal = da.select_method_wo_parameter(SelQ, "Text");
            if (dsVal.Tables.Count > 0 && dsVal.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsVal.Tables[0].Rows.Count; row++)
                {
                    htcons.Add(Convert.ToString(dsVal.Tables[0].Rows[row]["textcode"]), Convert.ToString(dsVal.Tables[0].Rows[row]["TextVal"]));
                }
            }
        }
        catch { }
        return htcons;
    }
    protected Hashtable htStaff()
    {
        Hashtable htStaff = new Hashtable();
        try
        {
            string SelQ = " select distinct (staff_code+'-'+staff_name) as staff_name,sa.appl_id from staffmaster s,staff_appl_master sa where s.appl_no=sa.appl_no and sa.college_code ='" + ddlcollege.SelectedItem.Value + "'";
            DataSet dsVal = da.select_method_wo_parameter(SelQ, "Text");
            if (dsVal.Tables.Count > 0 && dsVal.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsVal.Tables[0].Rows.Count; row++)
                {
                    htStaff.Add(Convert.ToString(dsVal.Tables[0].Rows[row]["appl_id"]), Convert.ToString(dsVal.Tables[0].Rows[row]["staff_name"]));
                }
            }
        }
        catch { }
        return htStaff;
    }
    protected void detailedConsClg(string batch, string degree, string feecat, string hdFK, string ldFK, string deduCtion, string fnlYr, string collegecode, string AllotpayType)
    {
        try
        {
            #region detailed

            #region query
            bool boolStaff = false;
            string applIdStr = string.Empty;
            if (cbStaff.Checked && lblval.Text.Trim() != "")
            {
                string applId = string.Empty;
                applId = Convert.ToString(lblval.Text);
                applIdStr = " and r.staff_appl_id in('" + applId + "')";
                boolStaff = true;
            }

            string SelQ = " select r.Batch_Year,r.degree_code,c.Course_Name,dt.Dept_Name,r.Stud_Name,r.roll_admit,r.Roll_No,r.Reg_No,f.app_no,r.staff_appl_id from FT_FeeAllot f,Registration r,Degree d,Course c,Department dt where f.App_No=r.App_No and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' and f.DeductAmout>0 and r.Batch_Year in(" + batch + ") and r.degree_code in(" + degree + ") and f.FeeCategory in(" + feecat + ") and f.HeaderFK in(" + hdFK + ") and f.LedgerFK in(" + ldFK + ") and f.DeductReason in(" + deduCtion + ",'0') and f.finyearfk in('" + fnlYr + "')  and r.college_code in('" + collegecode + "') " + applIdStr + " " + AllotpayType + " group by   r.Batch_Year,r.degree_code,c.Course_Name,dt.Dept_Name,r.Stud_Name,r.Roll_No,r.Reg_No,r.roll_admit,f.app_no,r.staff_appl_id  order by  r.degree_code,r.Batch_Year desc, r.Roll_No";//and ISNULL(f.DeductReason,'')<>''
            SelQ += " select f.app_no,f.FeeCategory,t.TextVal,f.DeductReason,sum(feeamount) as allot,sum(totalamount) as total,sum(paidamount) as paid,sum(balamount) as bal,SUM(ISNULL(deductAmout,'0')) as deduct,r.staff_appl_id  from FT_FeeAllot f,Registration r,textvaltable t where f.App_No=r.App_No and f.FeeCategory=t.TextCode  and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' and f.DeductAmout>0 and r.Batch_Year in(" + batch + ") and r.degree_code in(" + degree + ") and f.FeeCategory in(" + feecat + ") and f.HeaderFK in(" + hdFK + ") and f.LedgerFK in(" + ldFK + ") and f.DeductReason in(" + deduCtion + ",'0') and f.finyearfk in('" + fnlYr + "') and r.college_code in('" + collegecode + "') " + applIdStr + " " + AllotpayType + "  group by f.app_no,f.FeeCategory,f.DeductReason,t.TextVal,r.staff_appl_id ";
            //and ISNULL(f.DeductReason,'')<>''
            SelQ += " select distinct f.FeeCategory,t.TextVal from FT_FeeAllot f,Registration r,textvaltable t where f.App_No=r.App_No and f.FeeCategory=t.TextCode  and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' and f.DeductAmout>0 and r.Batch_Year in(" + batch + ") and r.degree_code in(" + degree + ") and f.FeeCategory in(" + feecat + ") and f.HeaderFK in(" + hdFK + ") and f.LedgerFK in(" + ldFK + ") and f.DeductReason in(" + deduCtion + ",'0') and f.finyearfk in('" + fnlYr + "') and r.college_code in('" + collegecode + "') " + applIdStr + "  order by f.FeeCategory";
            //and ISNULL(f.DeductReason,'')<>''
            DataSet dsstu = da.select_method_wo_parameter(SelQ, "Text");
            #endregion

            if (dsstu.Tables.Count > 0 && dsstu.Tables[0].Rows.Count > 0)
            {
                #region design

                FpSpread1.Sheets[0].ColumnCount = 7;
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                FpSpread1.Sheets[0].RowCount = 0;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Admission No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Name";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Staff Name";
                if (boolStaff)
                    FpSpread1.Sheets[0].Columns[5].Visible = true;
                else
                    FpSpread1.Sheets[0].Columns[5].Visible = false;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = Ibldegree.Text;
                FpSpread1.Sheets[0].Columns[6].Width = 500;
                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                Hashtable htColCnt = new Hashtable();
                for (int fs = 0; fs < chklscategory.Items.Count; fs++)
                {
                    if (chklscategory.Items[fs].Selected == true)
                    {
                        string fcate = chklscategory.Items[fs].Text.ToString();
                        string fcatecode = chklscategory.Items[fs].Value.ToString();
                        dsstu.Tables[2].DefaultView.RowFilter = "FeeCategory='" + fcatecode + "'";
                        DataView dvfeca = dsstu.Tables[2].DefaultView;
                        if (dvfeca.Count > 0)
                        {
                            int colCnt = FpSpread1.Sheets[0].ColumnCount++;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Allot";
                            FpSpread1.Sheets[0].ColumnCount++;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Cons(Amt)";
                            FpSpread1.Sheets[0].ColumnCount++;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total";
                            FpSpread1.Sheets[0].ColumnCount++;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Paid";
                            FpSpread1.Sheets[0].ColumnCount++;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Balance";
                            FpSpread1.Sheets[0].ColumnCount++;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Cons(R)";
                            //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, colCnt].Text = fcate;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, colCnt].Tag = fcatecode;
                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, colCnt, 1, 6);
                            htColCnt.Add(fcatecode, colCnt);
                        }
                    }
                }
                FpSpread1.Sheets[0].ColumnCount++;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Cons Total";
                FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;

                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                spreadColumnVisible();
                #endregion

                bool boolGrand = false;
                int rowCnt = 0;
                Hashtable httotal = new Hashtable();
                Hashtable htcons = htDeduction();
                Hashtable htstaff = htStaff();
                for (int i = 0; i < dsstu.Tables[0].Rows.Count; i++)
                {
                    #region
                    bool boolCheck = false;
                    bool rowCheck = false;
                    double totalConsAmt = 0;
                    string rollno = dsstu.Tables[0].Rows[i]["Roll_No"].ToString();
                    string regno = dsstu.Tables[0].Rows[i]["Reg_No"].ToString();
                    string rolladmit = dsstu.Tables[0].Rows[i]["roll_admit"].ToString();
                    string name = dsstu.Tables[0].Rows[i]["Stud_Name"].ToString();
                    string staffname = string.Empty;
                    string staffapplId = Convert.ToString(dsstu.Tables[0].Rows[i]["staff_appl_id"]);
                    if (!string.IsNullOrEmpty(staffapplId))
                        staffname = Convert.ToString(htstaff[staffapplId]);
                    string batchyear = dsstu.Tables[0].Rows[i]["Batch_Year"].ToString();
                    string course = dsstu.Tables[0].Rows[i]["Course_Name"].ToString();
                    string department = dsstu.Tables[0].Rows[i]["Dept_Name"].ToString();
                    string degreedetails = batchyear + " - " + course + " - " + department;
                    string appNo = Convert.ToString(dsstu.Tables[0].Rows[i]["app_no"]);
                    for (int trm = 0; trm < dsstu.Tables[2].Rows.Count; trm++)
                    {
                        string termCode = Convert.ToString(dsstu.Tables[2].Rows[trm]["feecategory"]);
                        dsstu.Tables[1].DefaultView.RowFilter = "feecategory='" + termCode + "' and app_no='" + appNo + "'";
                        DataTable dtPaid = dsstu.Tables[1].DefaultView.ToTable();
                        if (dtPaid.Rows.Count > 0)
                        {
                            int curColCnt = 0;
                            int.TryParse(Convert.ToString(htColCnt[termCode]), out curColCnt);
                            double allotAmt = 0;
                            double totalAmt = 0;
                            double consAmt = 0;
                            double paidAmt = 0;
                            double balAmt = 0;
                            string consReason = string.Empty;
                            double.TryParse(Convert.ToString(dtPaid.Rows[0]["allot"]), out allotAmt);
                            double.TryParse(Convert.ToString(dtPaid.Rows[0]["total"]), out totalAmt);
                            double.TryParse(Convert.ToString(dtPaid.Rows[0]["deduct"]), out consAmt);
                            double.TryParse(Convert.ToString(dtPaid.Rows[0]["paid"]), out paidAmt);
                            double.TryParse(Convert.ToString(dtPaid.Rows[0]["bal"]), out balAmt);
                            consReason = Convert.ToString(dtPaid.Rows[0]["DeductReason"]);
                            if (htcons.ContainsKey(consReason))
                                consReason = Convert.ToString(htcons[consReason]);
                            totalConsAmt += consAmt;
                            if (!rowCheck)
                                FpSpread1.Sheets[0].RowCount++;
                            rowCheck = true;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(allotAmt);
                            if (!httotal.ContainsKey(curColCnt))
                                httotal.Add(curColCnt, allotAmt);
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(httotal[curColCnt]), out amount);
                                amount += allotAmt;
                                httotal.Remove(curColCnt);
                                httotal.Add(curColCnt, amount);
                            }

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ++curColCnt].Text = Convert.ToString(consAmt);
                            if (!httotal.ContainsKey(curColCnt))
                                httotal.Add(curColCnt, consAmt);
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(httotal[curColCnt]), out amount);
                                amount += consAmt;
                                httotal.Remove(curColCnt);
                                httotal.Add(curColCnt, amount);
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ++curColCnt].Text = Convert.ToString(totalAmt);
                            if (!httotal.ContainsKey(curColCnt))
                                httotal.Add(curColCnt, totalAmt);
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(httotal[curColCnt]), out amount);
                                amount += totalAmt;
                                httotal.Remove(curColCnt);
                                httotal.Add(curColCnt, amount);
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ++curColCnt].Text = Convert.ToString(paidAmt);
                            if (!httotal.ContainsKey(curColCnt))
                                httotal.Add(curColCnt, paidAmt);
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(httotal[curColCnt]), out amount);
                                amount += paidAmt;
                                httotal.Remove(curColCnt);
                                httotal.Add(curColCnt, amount);
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ++curColCnt].Text = Convert.ToString(balAmt);
                            if (!httotal.ContainsKey(curColCnt))
                                httotal.Add(curColCnt, balAmt);
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(httotal[curColCnt]), out amount);
                                amount += balAmt;
                                httotal.Remove(curColCnt);
                                httotal.Add(curColCnt, amount);
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ++curColCnt].Text = Convert.ToString(consReason);

                            boolCheck = true;
                        }
                    }
                    if (boolCheck)
                    {
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(++rowCnt);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = txt;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = txt;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = txt;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = rollno;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = regno;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = rolladmit;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = name;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = staffname;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = degreedetails;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(totalConsAmt);
                        if (!httotal.ContainsKey(FpSpread1.Sheets[0].ColumnCount - 1))
                            httotal.Add(FpSpread1.Sheets[0].ColumnCount - 1, totalConsAmt);
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(httotal[FpSpread1.Sheets[0].ColumnCount - 1]), out amount);
                            amount += totalConsAmt;
                            httotal.Remove(FpSpread1.Sheets[0].ColumnCount - 1);
                            httotal.Add(FpSpread1.Sheets[0].ColumnCount - 1, amount);
                        }
                        boolGrand = true;
                    }
                    #endregion
                }
                if (boolGrand)
                {
                    #region grandtot
                    // spreadDet.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
                    FpSpread1.Sheets[0].Rows.Count++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].Rows.Count - 1, 0, 1, 4);
                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].Font.Bold = true;
                    double grandvalues = 0;
                    for (int j = 7; j < FpSpread1.Sheets[0].ColumnCount; j++)
                    {
                        double.TryParse(Convert.ToString(httotal[j]), out grandvalues);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                    }
                    #endregion
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    FpSpread1.Visible = true;
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnxl.Visible = true;
                    btnmasterprint.Visible = true;
                }
            }
            else
            {
                FpSpread1.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                btnmasterprint.Visible = false;
                clear();
                errmsg.Visible = true;
                errmsg.Text = "No Records Found";
            }
            #endregion
        }
        catch { }
    }

    protected void detailedConsScl(string batch, string degree, string feecat, string hdFK, string ldFK, string deduCtion, string fnlYr, string collegecode, string AllotpayType)
    {
        try
        {
            #region detailed

            #region query
            bool boolStaff = false;
            string applIdStr = string.Empty;
            if (cbStaff.Checked && lblval.Text.Trim() != "")
            {
                string applId = string.Empty;
                applId = Convert.ToString(lblval.Text);
                applIdStr = " and r.staff_appl_id in('" + applId + "')";
                boolStaff = true;
            }

            string SelQ = " select r.Batch_Year,r.degree_code,c.Course_Name,dt.Dept_Name,r.Stud_Name,r.roll_admit,r.Roll_No,r.Reg_No,f.app_no,f.finyearfk,r.staff_appl_id  from FT_FeeAllot f,Registration r,Degree d,Course c,Department dt where f.App_No=r.App_No and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' and f.DeductAmout>0 and r.Batch_Year in(" + batch + ") and r.degree_code in(" + degree + ") and f.FeeCategory in(" + feecat + ") and f.HeaderFK in(" + hdFK + ") and f.LedgerFK in(" + ldFK + ") and f.DeductReason in(" + deduCtion + ",'0')  and f.finyearfk in('" + fnlYr + "') and r.college_code in('" + collegecode + "') " + applIdStr + " " + AllotpayType + " group by   r.Batch_Year,r.degree_code,c.Course_Name,dt.Dept_Name,r.Stud_Name,r.Roll_No,r.Reg_No,r.roll_admit,f.app_no,f.finyearfk,r.staff_appl_id  order by  r.degree_code,r.Batch_Year desc, r.Roll_No";//and ISNULL(f.DeductReason,'')<>''
            SelQ += " select f.app_no,f.FeeCategory,t.TextVal,f.DeductReason,sum(feeamount) as allot,sum(totalamount) as total,sum(paidamount) as paid,sum(balamount) as bal,SUM(ISNULL(deductAmout,'0')) as deduct,f.finyearfk,r.staff_appl_id  from FT_FeeAllot f,Registration r,textvaltable t where f.App_No=r.App_No and f.FeeCategory=t.TextCode  and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' and f.DeductAmout>0 and r.Batch_Year in(" + batch + ") and r.degree_code in(" + degree + ") and f.FeeCategory in(" + feecat + ") and f.HeaderFK in(" + hdFK + ") and f.LedgerFK in(" + ldFK + ") and f.DeductReason in(" + deduCtion + ",'0')  and f.finyearfk in('" + fnlYr + "') and r.college_code in('" + collegecode + "') " + applIdStr + "  " + AllotpayType + " group by f.app_no,f.FeeCategory,f.DeductReason,t.TextVal,f.finyearfk,r.staff_appl_id ";
            //and ISNULL(f.DeductReason,'')<>''
            SelQ += " select distinct f.FeeCategory,t.TextVal from FT_FeeAllot f,Registration r,textvaltable t where f.App_No=r.App_No and f.FeeCategory=t.TextCode  and r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' and f.DeductAmout>0 and r.Batch_Year in(" + batch + ") and r.degree_code in(" + degree + ") and f.FeeCategory in(" + feecat + ")  and f.finyearfk in('" + fnlYr + "') and f.HeaderFK in(" + hdFK + ") and f.LedgerFK in(" + ldFK + ") and f.DeductReason in(" + deduCtion + ",'0')  and r.college_code in('" + collegecode + "') " + applIdStr + " order by f.FeeCategory";
            //and ISNULL(f.DeductReason,'')<>''
            //tbl 3 financial year
            SelQ += "  select (convert(varchar(10),datepart(year,finyearstart))+'-'+convert(varchar(10),datepart(year,finyearend)))as finyear,finyearpk,collegecode from fm_finyearmaster where collegecode='" + collegecode + "'";

            DataSet dsstu = da.select_method_wo_parameter(SelQ, "Text");
            #endregion

            if (dsstu.Tables.Count > 0 && dsstu.Tables[0].Rows.Count > 0)
            {
                #region design

                FpSpread1.Sheets[0].ColumnCount = 7;
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                FpSpread1.Sheets[0].RowCount = 0;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Admission No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Name";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Staff Name";
                if (boolStaff)
                    FpSpread1.Sheets[0].Columns[5].Visible = true;
                else
                    FpSpread1.Sheets[0].Columns[5].Visible = false;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = Ibldegree.Text;
                FpSpread1.Sheets[0].Columns[6].Width = 600;
                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                Hashtable htColCnt = new Hashtable();
                for (int fs = 0; fs < chklscategory.Items.Count; fs++)
                {
                    if (chklscategory.Items[fs].Selected == true)
                    {
                        string fcate = chklscategory.Items[fs].Text.ToString();
                        string fcatecode = chklscategory.Items[fs].Value.ToString();
                        dsstu.Tables[2].DefaultView.RowFilter = "FeeCategory='" + fcatecode + "'";
                        DataView dvfeca = dsstu.Tables[2].DefaultView;
                        if (dvfeca.Count > 0)
                        {
                            int colCnt = FpSpread1.Sheets[0].ColumnCount++;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Allot";
                            FpSpread1.Sheets[0].ColumnCount++;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Cons(Amt)";
                            FpSpread1.Sheets[0].ColumnCount++;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total";
                            FpSpread1.Sheets[0].ColumnCount++;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Paid";
                            FpSpread1.Sheets[0].ColumnCount++;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Balance";
                            FpSpread1.Sheets[0].ColumnCount++;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Cons(R)";
                            //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, colCnt].Text = fcate;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, colCnt].Tag = fcatecode;
                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, colCnt, 1, 6);
                            htColCnt.Add(fcatecode, colCnt);
                        }
                    }
                }
                FpSpread1.Sheets[0].ColumnCount++;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Cons Total";
                FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;

                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                spreadColumnVisible();
                #endregion

                bool boolGrand = false;
                int rowCnt = 0;
                Hashtable httotal = new Hashtable();
                Hashtable htFnltotal = new Hashtable();
                Hashtable htcons = htDeduction();
                Hashtable htstaff = htStaff();
                for (int fnYr = 0; fnYr < chklsfyear.Items.Count; fnYr++)
                {
                    string strFinYEarText = string.Empty;
                    bool boolFnlYR = false;
                    if (chklsfyear.Items[fnYr].Selected)
                    {
                        DataView dvfnlyear = new DataView();
                        if (dsstu.Tables[3].Rows.Count > 0)
                        {
                            dsstu.Tables[3].DefaultView.RowFilter = "finyearpk='" + chklsfyear.Items[fnYr].Value + "' and collegecode='" + collegecode + "'";
                            dvfnlyear = dsstu.Tables[3].DefaultView;
                            if (dvfnlyear.Count > 0)
                                strFinYEarText = Convert.ToString(dvfnlyear[0]["finyear"]);
                        }
                        dsstu.Tables[0].DefaultView.RowFilter = " finyearfk='" + chklsfyear.Items[fnYr].Value + "' ";
                        DataTable dtStud = dsstu.Tables[0].DefaultView.ToTable();
                        if (dtStud.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtStud.Rows.Count; i++)
                            {
                                #region
                                bool boolCheck = false;
                                bool rowCheck = false;
                                double totalConsAmt = 0;
                                string rollno = dtStud.Rows[i]["Roll_No"].ToString();
                                string regno = dtStud.Rows[i]["Reg_No"].ToString();
                                string rolladmit = dtStud.Rows[i]["roll_admit"].ToString();
                                string name = dtStud.Rows[i]["Stud_Name"].ToString();
                                string staffname = string.Empty;
                                string staffapplId = Convert.ToString(dtStud.Rows[i]["staff_appl_id"]);
                                if (!string.IsNullOrEmpty(staffapplId))
                                    staffname = Convert.ToString(htstaff[staffapplId]);
                                string batchyear = dtStud.Rows[i]["Batch_Year"].ToString();
                                string course = dtStud.Rows[i]["Course_Name"].ToString();
                                string department = dtStud.Rows[i]["Dept_Name"].ToString();
                                string degreedetails = batchyear + " - " + course + " - " + department;
                                string appNo = Convert.ToString(dtStud.Rows[i]["app_no"]);
                                for (int trm = 0; trm < dsstu.Tables[2].Rows.Count; trm++)
                                {
                                    string termCode = Convert.ToString(dsstu.Tables[2].Rows[trm]["feecategory"]);
                                    dsstu.Tables[1].DefaultView.RowFilter = "feecategory='" + termCode + "' and app_no='" + appNo + "' and finyearfk='" + chklsfyear.Items[fnYr].Value + "' ";
                                    DataTable dtPaid = dsstu.Tables[1].DefaultView.ToTable();
                                    if (dtPaid.Rows.Count > 0)
                                    {
                                        int curColCnt = 0;
                                        int.TryParse(Convert.ToString(htColCnt[termCode]), out curColCnt);
                                        double allotAmt = 0;
                                        double totalAmt = 0;
                                        double consAmt = 0;
                                        double paidAmt = 0;
                                        double balAmt = 0;
                                        string consReason = string.Empty;
                                        double.TryParse(Convert.ToString(dtPaid.Rows[0]["allot"]), out allotAmt);
                                        double.TryParse(Convert.ToString(dtPaid.Rows[0]["total"]), out totalAmt);
                                        double.TryParse(Convert.ToString(dtPaid.Rows[0]["deduct"]), out consAmt);
                                        double.TryParse(Convert.ToString(dtPaid.Rows[0]["paid"]), out paidAmt);
                                        double.TryParse(Convert.ToString(dtPaid.Rows[0]["bal"]), out balAmt);
                                        consReason = Convert.ToString(dtPaid.Rows[0]["DeductReason"]);
                                        if (htcons.ContainsKey(consReason))
                                            consReason = Convert.ToString(htcons[consReason]);
                                        totalConsAmt += consAmt;
                                        if (!boolFnlYR)
                                        {
                                            FpSpread1.Sheets[0].Rows.Count++;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = strFinYEarText;
                                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].Rows.Count - 1, 0, 1, 6);
                                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].BackColor = Color.Gold;
                                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].Font.Bold = true;
                                            boolFnlYR = true;
                                        }
                                        if (!rowCheck)
                                            FpSpread1.Sheets[0].RowCount++;
                                        rowCheck = true;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(allotAmt);
                                        if (!httotal.ContainsKey(curColCnt))
                                            httotal.Add(curColCnt, allotAmt);
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(httotal[curColCnt]), out amount);
                                            amount += allotAmt;
                                            httotal.Remove(curColCnt);
                                            httotal.Add(curColCnt, amount);
                                        }

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ++curColCnt].Text = Convert.ToString(consAmt);
                                        if (!httotal.ContainsKey(curColCnt))
                                            httotal.Add(curColCnt, consAmt);
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(httotal[curColCnt]), out amount);
                                            amount += consAmt;
                                            httotal.Remove(curColCnt);
                                            httotal.Add(curColCnt, amount);
                                        }
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ++curColCnt].Text = Convert.ToString(totalAmt);
                                        if (!httotal.ContainsKey(curColCnt))
                                            httotal.Add(curColCnt, totalAmt);
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(httotal[curColCnt]), out amount);
                                            amount += totalAmt;
                                            httotal.Remove(curColCnt);
                                            httotal.Add(curColCnt, amount);
                                        }
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ++curColCnt].Text = Convert.ToString(paidAmt);
                                        if (!httotal.ContainsKey(curColCnt))
                                            httotal.Add(curColCnt, paidAmt);
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(httotal[curColCnt]), out amount);
                                            amount += paidAmt;
                                            httotal.Remove(curColCnt);
                                            httotal.Add(curColCnt, amount);
                                        }
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ++curColCnt].Text = Convert.ToString(balAmt);
                                        if (!httotal.ContainsKey(curColCnt))
                                            httotal.Add(curColCnt, balAmt);
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(httotal[curColCnt]), out amount);
                                            amount += balAmt;
                                            httotal.Remove(curColCnt);
                                            httotal.Add(curColCnt, amount);
                                        }
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ++curColCnt].Text = Convert.ToString(consReason);
                                        boolCheck = true;
                                    }
                                }
                                if (boolCheck)
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(++rowCnt);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = txt;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = txt;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = txt;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = rollno;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = regno;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = rolladmit;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = name;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = staffname;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = degreedetails;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(totalConsAmt);
                                    if (!httotal.ContainsKey(FpSpread1.Sheets[0].ColumnCount - 1))
                                        httotal.Add(FpSpread1.Sheets[0].ColumnCount - 1, totalConsAmt);
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(httotal[FpSpread1.Sheets[0].ColumnCount - 1]), out amount);
                                        amount += totalConsAmt;
                                        httotal.Remove(FpSpread1.Sheets[0].ColumnCount - 1);
                                        httotal.Add(FpSpread1.Sheets[0].ColumnCount - 1, amount);
                                    }
                                    boolGrand = true;
                                }
                                #endregion
                            }
                        }

                    }
                    if (httotal.Count > 0)
                    {
                        #region total
                        // spreadDet.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = "Total";
                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].Rows.Count - 1, 0, 1, 4);
                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].Font.Bold = true;
                        double grandvalues = 0;
                        for (int j = 7; j < FpSpread1.Sheets[0].ColumnCount; j++)
                        {
                            double.TryParse(Convert.ToString(httotal[j]), out grandvalues);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                            if (!htFnltotal.ContainsKey(j))
                                htFnltotal.Add(j, grandvalues);
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(htFnltotal[j]), out amount);
                                amount += grandvalues;
                                htFnltotal.Remove(j);
                                htFnltotal.Add(j, amount);
                            }
                        }
                        httotal.Clear();
                        #endregion
                    }
                }
                if (htFnltotal.Count > 0)
                {
                    #region grandtot
                    // spreadDet.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
                    FpSpread1.Sheets[0].Rows.Count++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].Rows.Count - 1, 0, 1, 4);
                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].Font.Bold = true;
                    double grandvalues = 0;
                    for (int j = 7; j < FpSpread1.Sheets[0].ColumnCount; j++)
                    {
                        double.TryParse(Convert.ToString(htFnltotal[j]), out grandvalues);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                    }
                    #endregion
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    FpSpread1.Visible = true;
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnxl.Visible = true;
                    btnmasterprint.Visible = true;
                }
            }
            else
            {
                FpSpread1.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                btnmasterprint.Visible = false;
                clear();
                errmsg.Visible = true;
                errmsg.Text = "No Records Found";
            }
            #endregion
        }
        catch { }
    }

    //added by sudhagar staff name 
    protected void cbStaff_changed(object sender, EventArgs e)
    {
        btn_staffLook.Enabled = false;
        lbldisp.Text = string.Empty;
        lblval.Text = string.Empty;
        if (cbStaff.Checked)
        {
            btn_staffLook.Enabled = true;
        }
    }
    //Lookup Staff
    protected void btn_staffLook_Click(object sender, EventArgs e)
    {
        div_staffLook.Visible = true;
        ddlsearch1_OnSelectedIndexChanged(sender, e);
        btn_staffOK.Visible = false;
        btn_exitstaff.Visible = false;
        Fpspread2.Visible = false;
        lbl_errormsgstaff.Visible = false;
    }
    protected void btn_staffOK_Click(object sender, EventArgs e)
    {
        try
        {
            lbldisp.Text = string.Empty;
            lblval.Text = string.Empty;
            Fpspread2.SaveChanges();
            if (Fpspread2.Sheets[0].RowCount > 0)
            {
                string staffCode = string.Empty;
                int rowCnt = 0;
                StringBuilder sbStaff = new StringBuilder();
                for (int row = 1; row < Fpspread2.Sheets[0].RowCount; row++)
                {
                    int value = 0;
                    int.TryParse(Convert.ToString(Fpspread2.Sheets[0].Cells[row, 1].Value), out value);
                    if (value == 1)
                    {
                        rowCnt++;
                        string stfCode = Convert.ToString(Fpspread2.Sheets[0].Cells[row, 1].Tag);//staff appl id
                        sbStaff.Append(stfCode + "','");
                    }
                }
                if (sbStaff.Length > 0)
                {
                    sbStaff.Remove(sbStaff.Length - 3, 3);
                    staffCode = Convert.ToString(sbStaff);
                    lbldisp.Text = "You have selected " + rowCnt + " Staff";
                    lbldisp.Visible = true;
                    lblval.Text = staffCode;
                }
            }
            div_staffLook.Visible = false;
        }
        catch (Exception ex) { }
    }
    protected void btn_exitstaff_Click(object sender, EventArgs e)
    {
        div_staffLook.Visible = false;
    }
    protected void ddlsearch1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        txtsearch1.Text = "";
        txtsearch1c.Text = "";
        txtsearch1c.Visible = false;
        txtsearch1.Visible = false;
        if (ddlsearch1.SelectedIndex == 0)
        {
            txtsearch1.Visible = true;
        }
        else
        {
            txtsearch1c.Visible = true;
        }
    }
    protected void Fpspread2staff_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            if (Fpspread2.Sheets[0].RowCount > 0)
            {
                int value = 0;
                int.TryParse(Convert.ToString(Fpspread2.Sheets[0].Cells[0, 1].Value), out value);
                if (value == 1)
                {
                    for (int row = 0; row < Fpspread2.Sheets[0].RowCount; row++)
                        Fpspread2.Sheets[0].Cells[row, 1].Value = 1;
                }
                else
                {
                    for (int row = 0; row < Fpspread2.Sheets[0].RowCount; row++)
                        Fpspread2.Sheets[0].Cells[row, 1].Value = 0;
                }
            }
        }
        catch (Exception ex) { }
    }
    protected void btn_go2Staff_Click(object sender, EventArgs e)
    {
        try
        {
            string collegecode1 = string.Empty;
            if (ddlcollege.Items.Count > 0)
                collegecode1 = Convert.ToString(ddlcollege.SelectedValue);
            div_staffLook.Visible = true;
            if (collegecode1 != null)
            {
                string selq = "";
                if (txtsearch1.Text.Trim() != "")
                {
                    string sname = string.Empty;
                    try
                    {
                        sname = txtsearch1.Text.Trim().Split('-')[0];
                    }
                    catch { sname = txtsearch1.Text.Trim(); }
                    selq = "select appl_id ,h.dept_name,s.staff_name,s.staff_code  from staffmaster s,staff_appl_master a,hrdept_master h where s.appl_no =a.appl_no and a.dept_code =h.dept_code  and s.college_code='" + collegecode1 + "' and staff_name like '" + Convert.ToString(sname) + "%'";
                }
                else if (txtsearch1c.Text.Trim() != "")
                {
                    selq = "select appl_id ,h.dept_name,s.staff_name,s.staff_code  from staffmaster s,staff_appl_master a,hrdept_master h where s.appl_no =a.appl_no and a.dept_code =h.dept_code  and s.college_code='" + collegecode1 + "' and staff_code='" + Convert.ToString(txtsearch1c.Text) + "'";
                }
                else
                {
                    selq = "select appl_id ,h.dept_name,s.staff_name,s.staff_code  from staffmaster s,staff_appl_master a,hrdept_master h where s.appl_no =a.appl_no and a.dept_code =h.dept_code  and s.college_code='" + collegecode1 + "' order by PrintPriority";
                }
                ds.Clear();
                ds = da.select_method_wo_parameter(selq, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Fpspread2.Sheets[0].RowCount = 0;
                    Fpspread2.Sheets[0].ColumnCount = 0;
                    Fpspread2.CommandBar.Visible = false;
                    Fpspread2.Sheets[0].AutoPostBack = false;
                    Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
                    Fpspread2.Sheets[0].RowHeader.Visible = false;
                    Fpspread2.Sheets[0].ColumnCount = 4;
                    Fpspread2.Sheets[0].Columns[0].Width = 60;
                    Fpspread2.Sheets[0].Columns[1].Width = 60;
                    Fpspread2.Sheets[0].Columns[2].Width = 170;
                    Fpspread2.Sheets[0].Columns[3].Width = 360;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Code";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Staff Name";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;

                    FarPoint.Web.Spread.TextCellType txtall = new FarPoint.Web.Spread.TextCellType();
                    FarPoint.Web.Spread.CheckBoxCellType cball = new FarPoint.Web.Spread.CheckBoxCellType();
                    FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                    cball.AutoPostBack = true;
                    cb.AutoPostBack = false;

                    bool boolFirst = false;
                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        if (!boolFirst)
                        {
                            Fpspread2.Sheets[0].RowCount++;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].CellType = cball;
                            boolFirst = true;
                        }
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].CellType = cb;

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].CellType = txtall;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["staff_code"]);

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["appl_id"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["staff_name"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                    }
                    Fpspread2.Visible = true;
                    // div2.Visible = true;
                    lbl_errormsgstaff.Visible = false;
                    Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                    Fpspread2.Width = 620;
                    Fpspread2.Height = 210;
                    if (Fpspread2.Sheets[0].RowCount > 0)
                    {
                        btn_staffOK.Visible = true;
                        btn_exitstaff.Visible = true;
                    }
                    else
                    {
                        btn_staffOK.Visible = false;
                        btn_exitstaff.Visible = false;
                    }
                }
            }
        }
        catch (Exception ex) { }
    }

    private double checkSchoolSetting()
    {
        double getVal = 0;
        double.TryParse(Convert.ToString(da.GetFunction("select  value from Master_Settings where settings='schoolorcollege' and usercode='" + usercode + "'")), out getVal);
        return getVal;
    }

    protected void rblPayType_Selected(object sender, EventArgs e)
    {
        clear();
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
}