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

public partial class Refund_Report : System.Web.UI.Page
{
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    string group_user = "", singleuser = "", usercode = "", collegecode = "", group_code = "";
    int count = 0;
    Hashtable has = new Hashtable();
    static TreeNode node;
    TreeNode subchildnode;
    static byte roll = 0;
    static ArrayList colord = new ArrayList();
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
                columnType();
                ddlMainreport_Selected(sender, e);
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
                    loadMode();
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
            //}
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
                // string straccheadquery = "select distinct a.header_id,a.header_name from chlheadersettings c,Acctheader a where c.Header_ID=a.header_id and a.header_name not in ('arrear') " + typeval + " " + strheadid + "";
                //string straccheadquery = "select distinct a.header_id,a.header_name from Acctheader a where a.header_name not in ('arrear') " + strheadid + "";
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
        loadtype();
        bindbatch();
        binddegree();
        bindbranch();
        loadheader();
        loadfeecategory();
        loadfinanceyear();
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

    protected void chkceteg_CheckedChanged(object sender, EventArgs e)
    {
        clear();
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

    #region old

    //protected void btngo_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        RollAndRegSettings();
    //        #region get value
    //        clear();
    //        string batchquery = "";
    //        for (int b = 0; b < Chklst_batch.Items.Count; b++)
    //        {
    //            if (Chklst_batch.Items[b].Selected == true)
    //            {
    //                if (batchquery == "")
    //                    batchquery = Chklst_batch.Items[b].Text;
    //                else
    //                    batchquery = batchquery + "," + Chklst_batch.Items[b].Text;
    //            }
    //        }
    //        if (batchquery.Trim() == "")
    //        {
    //            errmsg.Visible = true;
    //            errmsg.Text = "Please Select The Batch Year And Then Proceed";
    //            return;
    //        }

    //        string degreequery = "";
    //        for (int b = 0; b < chklst_branch.Items.Count; b++)
    //        {
    //            if (chklst_branch.Items[b].Selected == true)
    //            {
    //                if (degreequery == "")
    //                {
    //                    degreequery = chklst_branch.Items[b].Value.ToString();
    //                }
    //                else
    //                {
    //                    degreequery = degreequery + "," + chklst_branch.Items[b].Value.ToString();
    //                }
    //            }
    //        }
    //        if (degreequery.Trim() == "")
    //        {
    //            errmsg.Visible = true;
    //            errmsg.Text = "Please Select The Degree And Branch And Then Proceed";
    //            return;
    //        }

    //        string headercode = "";
    //        for (int b = 0; b < chklsheader.Items.Count; b++)
    //        {
    //            if (chklsheader.Items[b].Selected == true)
    //            {
    //                if (headercode == "")
    //                {
    //                    headercode = chklsheader.Items[b].Value.ToString();
    //                }
    //                else
    //                {
    //                    headercode = headercode + "," + chklsheader.Items[b].Value.ToString();
    //                }
    //            }
    //        }
    //        if (headercode.Trim() == "")
    //        {
    //            errmsg.Visible = true;
    //            errmsg.Text = "Please Select The Header And Then Proceed";
    //            return;
    //        }

    //        string feecodequery = "";
    //        for (int remv = 0; remv < treeview_spreadfields.Nodes.Count; remv++)
    //        {
    //            if (treeview_spreadfields.Nodes[remv].ChildNodes.Count > 0)
    //            {
    //                for (int child = 0; child < treeview_spreadfields.Nodes[remv].ChildNodes.Count; child++)
    //                {
    //                    if (treeview_spreadfields.Nodes[remv].ChildNodes[child].Checked == true)
    //                    {
    //                        if (feecodequery == "")
    //                        {
    //                            feecodequery = treeview_spreadfields.Nodes[remv].ChildNodes[child].Value.ToString();
    //                        }
    //                        else
    //                        {
    //                            feecodequery = feecodequery + "," + treeview_spreadfields.Nodes[remv].ChildNodes[child].Value.ToString();
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        if (feecodequery.Trim() == "")
    //        {
    //            errmsg.Visible = true;
    //            errmsg.Text = "Please Select The Ledger And Then Proceed";
    //            return;
    //        }

    //        string actidquery = "";
    //        for (int i = 0; i < chklsfyear.Items.Count; i++)
    //        {
    //            if (chklsfyear.Items[i].Selected == true)
    //            {
    //                string accid = chklsfyear.Items[i].Value.ToString();
    //                if (actidquery == "")
    //                {
    //                    actidquery = "'" + accid + "'";
    //                }
    //                else
    //                {
    //                    actidquery = actidquery + ",'" + accid + "'";
    //                }
    //            }
    //        }
    //        if (actidquery.Trim() == "")
    //        {
    //            errmsg.Visible = true;
    //            errmsg.Text = "Please Select The Finance Year And The Proceed";
    //            return;
    //        }


    //        string modeVal = "";
    //        for (int i = 0; i < cblmode.Items.Count; i++)
    //        {
    //            if (cblmode.Items[i].Selected == true)
    //            {
    //                string accid = cblmode.Items[i].Value.ToString();
    //                if (modeVal == "")
    //                    modeVal = "'" + accid + "'";
    //                else
    //                    modeVal = modeVal + ",'" + accid + "'";
    //            }
    //        }
    //        if (modeVal.Trim() == "")
    //        {
    //            errmsg.Visible = true;
    //            errmsg.Text = "Please Select The Mode And The Proceed";
    //            return;
    //        }



    //        string feecatquery = "";
    //        for (int i = 0; i < chklscategory.Items.Count; i++)
    //        {
    //            if (chklscategory.Items[i].Selected == true)
    //            {
    //                if (feecatquery == "")
    //                {
    //                    feecatquery = chklscategory.Items[i].Value.ToString();
    //                }
    //                else
    //                {
    //                    feecatquery = feecatquery + "," + chklscategory.Items[i].Value.ToString();
    //                }
    //            }
    //        }
    //        if (feecatquery.Trim() == "" && chkceteg.Checked == true)
    //        {
    //            errmsg.Visible = true;
    //            errmsg.Text = "Please Select The Category And The Proceed";
    //            return;
    //        }

    //        #endregion

    //        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
    //        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
    //        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;

    //        FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
    //        FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
    //        FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
    //        FpSpread1.Sheets[0].SheetCorner.RowCount = 1;
    //        FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
    //        style.Font.Size = 10;
    //        style.Font.Bold = true;
    //        FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
    //        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
    //        FpSpread1.Sheets[0].AllowTableCorner = true;
    //        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
    //        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
    //        FpSpread1.Sheets[0].RowHeader.Visible = false;
    //        FpSpread1.CommandBar.Visible = false;

    //        FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
    //        style2.Font.Size = 13;
    //        style2.Font.Name = "Book Antiqua";
    //        style2.Font.Bold = true;
    //        style2.HorizontalAlign = HorizontalAlign.Center;
    //        style2.ForeColor = System.Drawing.Color.White;
    //        style2.BackColor = System.Drawing.Color.Teal;
    //        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);
    //        FpSpread1.Visible = true;
    //        FpSpread1.Sheets[0].AutoPostBack = true;

    //        FpSpread1.Sheets[0].ColumnCount = 0;
    //        FpSpread1.Sheets[0].ColumnHeader.RowCount = 0;
    //        FpSpread1.Sheets[0].ColumnCount = 6;
    //        FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
    //        FpSpread1.Sheets[0].RowCount = 0;

    //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = Ibldegree.Text;
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg No";
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Admission No";
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Name";

    //        FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
    //        FpSpread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
    //        FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
    //        FpSpread1.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
    //        FpSpread1.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
    //        FpSpread1.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
    //        spreadColumnVisible();


    //        Boolean stuflag = false;
    //        DataSet dsrefunquery = new DataSet();
    //        string refundquery = " select distinct r.Batch_Year ,c.type,c.Edu_Level,c.Course_Name,dt.Dept_Name ,d.Degree_Code,r.Roll_No,r.roll_admit,r.Reg_No,r.Stud_Name from FT_FeeAllot f,Registration r,Degree d,Course c,Department dt where f.App_No=r.App_No and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and ISNULL(RefundAmount,'0')>0 and r.Batch_Year  in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and f.HeaderFK in(" + headercode + ") and f.LedgerFK in(" + feecodequery + ")  order by c.type,c.Edu_Level,d.Degree_Code,r.Batch_Year desc,r.Roll_No";
    //        DataSet dsrefunstu = da.select_method_wo_parameter(refundquery, "text");

    //        string refunval = " select r.Roll_No,SUM(ISNULL(f.RefundAmount,'0'))as refund from FT_FeeAllot f,Registration r where f.App_No=r.App_No  and ISNULL(f.RefundAmount,'0')>0 and r.Batch_Year  in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and f.HeaderFK in(" + headercode + ") and f.LedgerFK in(" + feecodequery + ") group by r.Roll_No";
    //        dsrefunquery.Clear();
    //        dsrefunquery = da.select_method_wo_parameter(refunval, "text");

    //        #region old
    //        //if (chkceteg.Checked == true)
    //        //{               
    //        //    refunval = " select r.Roll_No,f.FeeCategory,SUM(ISNULL(f.RefundAmount,'0'))as refund from FT_FeeAllot f,Registration r where f.App_No=r.App_No  and ISNULL(f.RefundAmount,'0')>0 and r.Batch_Year  in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and f.HeaderFK in(" + headercode + ") and f.LedgerFK in(" + feecodequery + ") and f.FeeCategory in(" + feecatquery + ") group by r.Roll_No,f.FeeCategory";

    //        //    dsrefunquery = da.select_method_wo_parameter(refunval, "text");

    //        //    for (int i = 0; i < chklscategory.Items.Count; i++)
    //        //    {
    //        //        if (chklscategory.Items[i].Selected == true)
    //        //        {
    //        //            dsrefunquery.Tables[0].DefaultView.RowFilter = "FeeCategory='" + chklscategory.Items[i].Value.ToString() + "' ";
    //        //            DataView dvsturefun = dsrefunquery.Tables[0].DefaultView;
    //        //            if (dvsturefun.Count > 0)
    //        //            {
    //        //                FpSpread1.Sheets[0].ColumnCount++;
    //        //                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = chklscategory.Items[i].Text.ToString();
    //        //                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = chklscategory.Items[i].Value.ToString();
    //        //                FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;
    //        //            }
    //        //        }
    //        //    }
    //        //    if (dsrefunquery.Tables[0].Rows.Count == 0)
    //        //        dsrefunstu.Clear();
    //        //}
    //        //else
    //        //{
    //        //    dsrefunquery = da.select_method_wo_parameter(refunval, "text");
    //        //}

    //        #endregion

    //        FpSpread1.Sheets[0].ColumnCount++;
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total";
    //        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].VerticalAlign = VerticalAlign.Middle;


    //        int srno = 0;
    //        Hashtable hatdegrtotal = new Hashtable();
    //        Double totamount = 0;
    //        Double grandtotal = 0;
    //        for (int i = 0; i < dsrefunstu.Tables[0].Rows.Count; i++)
    //        {
    //            stuflag = true;
    //            string batchyear = dsrefunstu.Tables[0].Rows[i]["Batch_Year"].ToString();
    //            string degreecode = dsrefunstu.Tables[0].Rows[i]["Degree_Code"].ToString();
    //            string course = dsrefunstu.Tables[0].Rows[i]["Course_name"].ToString();
    //            string department = dsrefunstu.Tables[0].Rows[i]["Dept_Name"].ToString();
    //            string rollno = dsrefunstu.Tables[0].Rows[i]["Roll_No"].ToString();
    //            string regno = dsrefunstu.Tables[0].Rows[i]["Reg_No"].ToString();
    //            string rolladmit = dsrefunstu.Tables[0].Rows[i]["roll_admit"].ToString();
    //            string name = dsrefunstu.Tables[0].Rows[i]["Stud_Name"].ToString();
    //            string amount = "0";
    //            string degreedetails = batchyear + " - " + course + " - " + department;

    //            if (!hatdegrtotal.Contains(batchyear + '-' + degreecode))
    //            {
    //                if (hatdegrtotal.Count > 0)
    //                {
    //                    FpSpread1.Sheets[0].RowCount++;
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
    //                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
    //                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.LightGray;
    //                    if (chkceteg.Checked == false)
    //                    {
    //                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = totamount.ToString();
    //                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
    //                        totamount = 0;
    //                    }
    //                    else
    //                    {
    //                        string getcode = dsrefunstu.Tables[0].Rows[i - 1]["Batch_Year"].ToString() + '-' + dsrefunstu.Tables[0].Rows[i - 1]["Degree_Code"].ToString();
    //                        int stratrow = Convert.ToInt32(hatdegrtotal[getcode].ToString());
    //                        for (int c = 6; c < FpSpread1.Sheets[0].ColumnCount; c++)
    //                        {
    //                            Double getval = 0;
    //                            for (int r = 0; r < FpSpread1.Sheets[0].RowCount - 1; r++)
    //                            {
    //                                string rowval = FpSpread1.Sheets[0].Cells[r, c].Text.ToString().Trim();
    //                                if (rowval != "")
    //                                {
    //                                    getval = getval + Convert.ToDouble(rowval);
    //                                }
    //                            }
    //                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = getval.ToString();
    //                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Right;
    //                        }
    //                    }
    //                }
    //                hatdegrtotal.Add(batchyear + '-' + degreecode, FpSpread1.Sheets[0].RowCount);
    //            }

    //            srno++;
    //            FpSpread1.Sheets[0].RowCount++;
    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = degreedetails;
    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = rollno;
    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = regno;
    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = regno;
    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = name;
    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = amount;
    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
    //            if (chkceteg.Checked == true)
    //            {
    //                Double sturefu = 0;
    //                for (int c = 6; c < FpSpread1.Sheets[0].ColumnCount - 1; c++)
    //                {
    //                    amount = "0";
    //                    string feecat = FpSpread1.Sheets[0].ColumnHeader.Cells[0, c].Tag.ToString();
    //                    dsrefunquery.Tables[0].DefaultView.RowFilter = "FeeCategory='" + feecat + "' and Roll_No='" + rollno + "'";
    //                    DataView dvsturefun = dsrefunquery.Tables[0].DefaultView;
    //                    if (dvsturefun.Count > 0)
    //                    {
    //                        amount = dvsturefun[0]["refund"].ToString();
    //                        sturefu = sturefu + Convert.ToDouble(amount);
    //                    }
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = amount;
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Right;
    //                }
    //                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = sturefu.ToString();
    //                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
    //            }
    //            else
    //            {
    //                amount = "0";
    //                dsrefunquery.Tables[0].DefaultView.RowFilter = "Roll_No='" + rollno + "'";
    //                DataView dvsturefun = dsrefunquery.Tables[0].DefaultView;
    //                if (dvsturefun.Count > 0)
    //                {
    //                    amount = dvsturefun[0]["refund"].ToString();
    //                    totamount = totamount + Convert.ToDouble(amount);
    //                    grandtotal = grandtotal + Convert.ToDouble(amount);
    //                }
    //                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = amount;
    //                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
    //            }
    //        }
    //        if (stuflag == true)
    //        {
    //            FpSpread1.Sheets[0].RowCount++;
    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
    //            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
    //            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.LightGray;

    //            if (chkceteg.Checked == false)
    //            {
    //                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = totamount.ToString();
    //                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
    //                totamount = 0;
    //            }
    //            else
    //            {
    //                string getcode = dsrefunstu.Tables[0].Rows[dsrefunstu.Tables[0].Rows.Count - 1]["Batch_Year"].ToString() + '-' + dsrefunstu.Tables[0].Rows[dsrefunstu.Tables[0].Rows.Count - 1]["Degree_Code"].ToString();
    //                int stratrow = Convert.ToInt32(hatdegrtotal[getcode].ToString());
    //                for (int c = 6; c < FpSpread1.Sheets[0].ColumnCount; c++)
    //                {
    //                    Double getval = 0;
    //                    for (int r = 0; r < FpSpread1.Sheets[0].RowCount - 1; r++)
    //                    {
    //                        string rowval = FpSpread1.Sheets[0].Cells[r, c].Text.ToString().Trim();
    //                        if (rowval != "")
    //                        {
    //                            getval = getval + Convert.ToDouble(rowval);
    //                        }
    //                    }
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = getval.ToString();
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Right;
    //                }
    //            }

    //            FpSpread1.Sheets[0].RowCount++;
    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
    //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Right;
    //            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
    //            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.LightGreen;
    //            if (chkceteg.Checked == false)
    //            {

    //                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = grandtotal.ToString();
    //                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
    //            }
    //            else
    //            {
    //                for (int c = 6; c < FpSpread1.Sheets[0].ColumnCount; c++)
    //                {
    //                    Double getval = 0;
    //                    for (int r = 0; r < FpSpread1.Sheets[0].RowCount - 1; r++)
    //                    {
    //                        string rowval = FpSpread1.Sheets[0].Cells[r, c].Text.ToString().Trim();
    //                        if (rowval != "" && rowval != "Total")
    //                        {
    //                            getval = getval + Convert.ToDouble(rowval);
    //                        }
    //                    }
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].Text = getval.ToString();
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Right;
    //                }
    //            }

    //            FpSpread1.Visible = true;
    //            lblrptname.Visible = true;
    //            txtexcelname.Visible = true;
    //            btnxl.Visible = true;
    //            btnmasterprint.Visible = true;
    //        }
    //        else
    //        {
    //            clear();
    //            errmsg.Visible = true;
    //            errmsg.Text = "No Records Found";
    //        }
    //        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
    //    }
    //    catch (Exception ex)
    //    {
    //        errmsg.Visible = true;
    //        errmsg.Text = ex.ToString();
    //    }
    //}

    #endregion

    //added by sudhagar

    protected void btngo_Click(object sender, EventArgs e)
    {
        ds.Clear();
        string AltColumn = string.Empty;
        string selColumn = getSelectedColumn(ref AltColumn);//get selected column name

        ds = loadDataset(selColumn, AltColumn);

        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            loadDetails(selColumn, AltColumn, ds);
        }
        else
        {
            FpSpread1.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnxl.Visible = false;
            btnmasterprint.Visible = false;

            errmsg.Visible = true;
            errmsg.Text = "No Record Found";
        }
    }

    protected DataSet loadDataset(string selColumn, string AltColumn)
    {
        DataSet dsload = new DataSet();
        string AltColumnOrder = string.Empty;
        string OrderbyColumn = getSelectedColumnOrderBy(ref AltColumnOrder);//For order by columnname
        string AltColumnGroup = string.Empty;
        string groupByColumn = getSelectedColumnGoupBy(ref AltColumnGroup);//For group by columnname
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
                        batchquery = Chklst_batch.Items[b].Text;
                    else
                        batchquery = batchquery + "," + Chklst_batch.Items[b].Text;
                }
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



            string modeVal = "";
            string ModeStr = string.Empty;
            int cnT = 0;
            for (int i = 0; i < cblmode.Items.Count; i++)
            {
                if (cblmode.Items[i].Selected == true)
                {
                    string accid = cblmode.Items[i].Value.ToString();
                    if (modeVal == "")
                        modeVal = "'" + accid + "'";
                    else
                        modeVal = modeVal + ",'" + accid + "'";
                }
            }

            //string SelVal = Convert.ToString(cblmode.Items[i].Value);
            //if (SelVal == "0")
            //    ModeStr = "and ISNULL(RefundAmount,'0')>0";


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


            #endregion

            #region Query

            string SelQ = " select distinct r.Batch_Year ,r.app_no," + selColumn.TrimEnd(',') + "  from FT_FeeAllot f,Registration r,applyn a where f.App_No=r.App_No and r.App_No=a.App_No and f.App_No=a.App_No and r.Batch_Year  in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and f.HeaderFK in(" + headercode + ") and f.LedgerFK in(" + feecodequery + ")  and r.college_code ='" + ddlcollege.SelectedItem.Value + "' order by r.Batch_Year ,r.app_no," + OrderbyColumn.TrimEnd(',') + " ";

            //refund
            SelQ += " select r.app_no,r.Roll_No,SUM(ISNULL(f.RefundAmount,'0'))-SUM(ISNULL(f.RefundAdjAmount,'0'))as Amount from FT_FeeAllot f,Registration r where f.App_No=r.App_No  and ISNULL(f.RefundAmount,'0')>0 and r.Batch_Year  in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and f.HeaderFK in(" + headercode + ") and f.LedgerFK in(" + feecodequery + ") and r.college_code ='" + ddlcollege.SelectedItem.Value + "' group by r.Roll_No,r.app_no having SUM(ISNULL(f.RefundAmount,'0'))-SUM(ISNULL(f.RefundAdjAmount,'0'))>'0'";

            //excess
            SelQ += "  select sum(ISNULL(ex.excessamt,'0'))-sum(ISNULL(ex.adjamt,'0')) as amount,r.app_no," + selColumn.TrimEnd(',') + " from registration r,ft_excessdet e,ft_excessledgerdet ex,applyn a where r.App_No=a.App_No and e.App_No=a.App_No and r.app_no=e.app_no and e.excessdetpk=ex.excessdetfk  and r.Batch_Year  in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and ex.HeaderFK in(" + headercode + ") and ex.LedgerFK in(" + feecodequery + ")  and r.college_code ='" + ddlcollege.SelectedItem.Value + "' group by r.app_no," + groupByColumn.TrimEnd(',') + " having sum(ex.excessamt)-sum(ex.adjamt)>'0'";

            //scholarship
            SelQ += " select r.app_no,r.Roll_No,SUM(ISNULL(f.FromGovtAmt,'0'))as Amount from FT_FeeAllot f,Registration r where f.App_No=r.App_No  and ISNULL(f.FromGovtAmt,'0')>0 and r.Batch_Year  in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and f.HeaderFK in(" + headercode + ") and f.LedgerFK in(" + feecodequery + ") and r.college_code ='" + ddlcollege.SelectedItem.Value + "' group by r.Roll_No,r.app_no";

            SelQ = SelQ + " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename,(c.Course_Name +'-'+dt.dept_acronym)as dept_acronym from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + ddlcollege.SelectedItem.Value + "'";

            SelQ += " select distinct r.Batch_Year ,r.Degree_Code,b.app_no,r.Roll_No,r.roll_admit,r.Reg_No,r.Stud_Name,b.accno from Registration r,studbankdet b where b.App_No=r.App_No   and r.Batch_Year  in(" + batchquery + ") and r.degree_code in(" + degreequery + ") and r.college_code ='" + ddlcollege.SelectedItem.Value + "' order by  r.Degree_Code,r.Batch_Year,b.app_no  ";
            dsload.Clear();
            dsload = da.select_method_wo_parameter(SelQ, "text");

            #endregion

        }
        catch { }
        return dsload;
    }

    protected void loadDetails(string selColumn, string AltColumn, DataSet ds)
    {
        try
        {
            #region design

            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 1;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.Sheets[0].RowHeader.Visible = false;

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            //FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            // RollAndRegSettings();  

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Tag = "0";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            //FpSpread1.Sheets[0].Columns[0].Width = 20;
            int aa = 0;
            string selQ = string.Empty;
            string Usercollegecode = string.Empty;
            if (ddlcollege.Items.Count > 0)
                Usercollegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            string linkName = string.Empty;
            if (ddlMainreport.Items.Count > 0 && ddlMainreport.SelectedItem.Text != "Select")
                linkName = Convert.ToString(ddlMainreport.SelectedItem.Text);
            if (!string.IsNullOrEmpty(linkName) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
            {
                selQ = d2.GetFunction("  select LinkValue from New_InsSettings where LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "' and user_code='" + usercode + "'");
                if (!string.IsNullOrEmpty(selQ) && selQ != "0")
                {
                    string[] splCol = selQ.Split(',');
                    for (int colVal = 0; colVal < splCol.Length; colVal++)
                    {
                        aa++;
                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = splCol[colVal];
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(aa);
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = Color.Black;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
            }
            aa++;
            FpSpread1.Sheets[0].ColumnCount++;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Account No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(aa);
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 200;
            FpSpread1.SaveChanges();

            Hashtable newhash = new Hashtable();
            for (int col = 0; col < cblmode.Items.Count; col++)
            {
                if (cblmode.Items[col].Selected == true)
                {
                    FpSpread1.Sheets[0].ColumnCount++;
                    newhash.Add(Convert.ToString(cblmode.Items[col].Value), FpSpread1.Sheets[0].ColumnCount - 1);
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(cblmode.Items[col].Text);
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(cblmode.Items[col].Value);
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                }
            }
            FpSpread1.Sheets[0].ColumnCount++;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
            //spreadColumnVisible();
            FpSpread1.SaveChanges();

            #endregion

            DataView dv = new DataView();
            DataView dvacc = new DataView();
            Hashtable grandtotal = new Hashtable();
            DataView Dview = new DataView();
            int rollCnt = 0;
            int sno = 0;
            bool check = false;
            int row1 = 0;
            FarPoint.Web.Spread.TextCellType txtacc = new FarPoint.Web.Spread.TextCellType();
           
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string appno = ds.Tables[0].Rows[i]["app_no"].ToString();
                double tempTotamt = 0;
                bool flags = false;
                int rowCnt = 0;

                if (ds.Tables[1].Rows.Count > 0 || ds.Tables[2].Rows.Count > 0)
                {

                    for (int row = 0; row < cblmode.Items.Count; row++)
                    {
                        if (cblmode.Items[row].Selected)
                        {
                            double Amount = 0;
                            int val = 0;
                            int.TryParse(Convert.ToString(cblmode.Items[row].Value), out val);
                            int colnew = Convert.ToInt32(newhash[Convert.ToString(val)]);
                            if (val == 0)
                            {
                                ds.Tables[1].DefaultView.RowFilter = "app_no='" + appno + "'";
                                dv = ds.Tables[1].DefaultView;
                                if (dv.Count > 0)
                                {
                                    double.TryParse(Convert.ToString(Convert.ToString(dv[0]["Amount"])), out Amount);
                                    flags = true;
                                    rowCnt++;

                                    tempTotamt += Amount;
                                    if (rowCnt == 1)
                                    {
                                        FpSpread1.Sheets[0].RowCount++;
                                        rollCnt++;
                                       
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colnew].Text = Convert.ToString(Amount);
                                    if (!grandtotal.ContainsKey(colnew))
                                        grandtotal.Add(colnew, Convert.ToString(Amount));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(grandtotal[colnew]), out amount);
                                        amount += Amount;
                                        grandtotal.Remove(colnew);
                                        grandtotal.Add(colnew, Convert.ToString(amount));
                                    }
                                }
                            }
                            if (val == 1)
                            {
                                ds.Tables[2].DefaultView.RowFilter = "app_no='" + appno + "'";
                                dv = ds.Tables[2].DefaultView;
                                if (dv.Count > 0)
                                {                                    
                                    double.TryParse(Convert.ToString(Convert.ToString(dv[0]["Amount"])), out Amount);
                                    flags = true;
                                    tempTotamt += Amount;
                                    rowCnt++;
                                    if (rowCnt == 1)
                                    {
                                        FpSpread1.Sheets[0].RowCount++;
                                        rollCnt++;
                                       
                                    }

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colnew].Text = Convert.ToString(Amount);
                                    if (!grandtotal.ContainsKey(colnew))
                                        grandtotal.Add(colnew, Convert.ToString(Amount));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(grandtotal[colnew]), out amount);
                                        amount += Amount;
                                        grandtotal.Remove(colnew);
                                        grandtotal.Add(colnew, Convert.ToString(amount));
                                    }
                                }
                            }
                            if (val == 2)
                            {
                                ds.Tables[3].DefaultView.RowFilter = "app_no='" + appno + "'";
                                dv = ds.Tables[3].DefaultView;
                                if (dv.Count > 0)
                                {
                                    double.TryParse(Convert.ToString(Convert.ToString(dv[0]["Amount"])), out Amount);
                                    flags = true;
                                    tempTotamt += Amount;
                                    rowCnt++;
                                    if (rowCnt == 1)
                                    {
                                        FpSpread1.Sheets[0].RowCount++;
                                        rollCnt++;
                                        
                                    }

                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colnew].Text = Convert.ToString(Amount);
                                    if (!grandtotal.ContainsKey(colnew))
                                        grandtotal.Add(colnew, Convert.ToString(Amount));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(grandtotal[colnew]), out amount);
                                        amount += Amount;
                                        grandtotal.Remove(colnew);
                                        grandtotal.Add(colnew, Convert.ToString(amount));
                                    }
                                }
                            }
                        }
                    }

                    if (flags)
                    {
                        string batchyear = ds.Tables[0].Rows[i]["Batch_Year"].ToString();

                        string Accno = string.Empty;
                        //if (ds.Tables[4].Rows.Count > 0)
                        //{
                        //    ds.Tables[4].DefaultView.RowFilter = "Degree_code='" + degreecode + "'";
                        //    Dview = ds.Tables[4].DefaultView;
                        //    if (Dview.Count > 0)
                        //    {
                        //        Degreename = Convert.ToString(Dview[0]["degreename"]);
                        //        Acrname = Convert.ToString(Dview[0]["dept_acronym"]);
                        //    }
                        //}
                        //string degreedetails = batchyear + " - " + Degreename;

                        if (ds.Tables[5].Rows.Count > 0)
                        {
                            ds.Tables[5].DefaultView.RowFilter = "app_no='" + appno + "'";
                            dvacc = ds.Tables[5].DefaultView;
                            if (dvacc.Count > 0)
                            {
                                Accno = Convert.ToString(dvacc[0]["accno"]);
                            }
                        }

                        // FpSpread1.Sheets[0].RowCount++;
                        sno++;
                        string[] splCol = selQ.Split(',');

                        for (int colVal = 0; colVal < FpSpread1.Sheets[0].ColumnHeader.Columns.Count; colVal++)
                        {
                            string columnName = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, colVal].Text).Trim();
                            int columnCount = Convert.ToInt32(FpSpread1.Sheets[0].ColumnHeader.Cells[0, colVal].Tag);
                            if (columnName == "S.No")
                            {
                                //FpSpread1.Sheets[0].Cells[row1, 0].CellType = txtacc;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(rollCnt);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].ForeColor = ColorTranslator.FromHtml("#000000");
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                            }
                            if (columnName == "Roll No")
                            {
                                string rollno = ds.Tables[0].Rows[i]["Roll No"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].CellType = txtacc;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Text = Convert.ToString(rollno);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].ForeColor = ColorTranslator.FromHtml("#000000");
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].HorizontalAlign = HorizontalAlign.Center;
                            }
                            if (columnName == "Reg No")
                            {
                                string RegNo = ds.Tables[0].Rows[i]["Reg No"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].CellType = txtacc;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Text = Convert.ToString(RegNo);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].ForeColor = ColorTranslator.FromHtml("#000000");
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].HorizontalAlign = HorizontalAlign.Center;
                            }
                            if (columnName == "Admission No")
                            {
                                string appformno = ds.Tables[0].Rows[i]["Admission No"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].CellType = txtacc;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Text = Convert.ToString(appformno);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].ForeColor = ColorTranslator.FromHtml("#000000");
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].HorizontalAlign = HorizontalAlign.Center;
                            }
                            if (columnName == "Student Name")
                            {
                                string studname = ds.Tables[0].Rows[i]["Student Name"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Text = Convert.ToString(studname);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].ForeColor = ColorTranslator.FromHtml("#000000");
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].HorizontalAlign = HorizontalAlign.Center;
                            }
                            if (columnName == "Course")
                            {
                                string Course = ds.Tables[0].Rows[i]["Course"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Text = Convert.ToString(Course);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].ForeColor = ColorTranslator.FromHtml("#000000");
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].HorizontalAlign = HorizontalAlign.Center;
                            }
                            if (columnName == "Department")
                            {
                                string Department = ds.Tables[0].Rows[i]["Department"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Text = Convert.ToString(Department);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].ForeColor = ColorTranslator.FromHtml("#000000");
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].HorizontalAlign = HorizontalAlign.Center;
                            }
                            if (columnName == "Permanent Address")
                            {
                                string address = ds.Tables[0].Rows[i]["addressp"].ToString();
                                string address1 = ds.Tables[0].Rows[i]["cityp"].ToString();
                                string address2 = ds.Tables[0].Rows[i]["parent_statep"].ToString();
                                string address3 = ds.Tables[0].Rows[i]["Countryp"].ToString();
                                string address4 = ds.Tables[0].Rows[i]["parent_pincodep"].ToString();
                                address1 = d2.GetFunction("select textval from textvaltable where textcode='" + address1 + "'");
                                if (address1 == "0")
                                {
                                    address1 = "";
                                }
                                address3 = d2.GetFunction("select textval from textvaltable where textcode='" + address3 + "'");
                                if (address3 == "0")
                                {
                                    address3 = "";
                                }
                                string permanentAdd =address + ' '+ address1 + ' ' + address2 + ' ' + address3 + ' ' + address4;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Text = Convert.ToString(permanentAdd.Trim(','));
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].ForeColor = ColorTranslator.FromHtml("#000000");
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].HorizontalAlign = HorizontalAlign.Center;
                            }
                            if (columnName == "Contact Address")
                            {
                                string address = ds.Tables[0].Rows[i]["addressc"].ToString();
                                string address1 = ds.Tables[0].Rows[i]["cityc"].ToString();
                                address1 = d2.GetFunction("select textval from textvaltable where textcode='" + address1 + "'");
                                string address2 = ds.Tables[0].Rows[i]["parent_statec"].ToString();
                                string address3 = ds.Tables[0].Rows[i]["Countryc"].ToString();
                                string address4 = ds.Tables[0].Rows[i]["parent_pincodec"].ToString();
                                if (address1 == "0")
                                {
                                    address1 = "";
                                }
                                address3 = d2.GetFunction("select textval from textvaltable where textcode='" + address3 + "'");
                                if (address3 == "0")
                                {
                                    address3 = "";
                                }
                                string ContactAdd = address + ' ' + address1 + ' ' + address2 + ' ' + address3 + ' ' + address4;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Text = Convert.ToString(ContactAdd.Trim(','));
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].ForeColor = ColorTranslator.FromHtml("#000000");
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].HorizontalAlign = HorizontalAlign.Center;
                            }
                            if (columnName == "Father Name")
                            {
                                string FatherName = ds.Tables[0].Rows[i]["Father Name"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Text = Convert.ToString(FatherName);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].ForeColor = ColorTranslator.FromHtml("#000000");
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].HorizontalAlign = HorizontalAlign.Center;
                            }
                            if (columnName == "Father Mobile No")
                            {
                                string FatherMob = ds.Tables[0].Rows[i]["Father Mobile No"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Text = Convert.ToString(FatherMob);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].ForeColor = ColorTranslator.FromHtml("#000000");
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].HorizontalAlign = HorizontalAlign.Center;
                            }
                            if (columnName == "Mother Name")
                            {
                                string MotherName = ds.Tables[0].Rows[i]["Mother Name"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Text = Convert.ToString(MotherName);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].ForeColor = ColorTranslator.FromHtml("#000000");
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].HorizontalAlign = HorizontalAlign.Center;
                            }
                            if (columnName == "Mother Mobile No")
                            {
                                string MotherMob = ds.Tables[0].Rows[i]["Mother Mobile No"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Text = Convert.ToString(MotherMob);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].ForeColor = ColorTranslator.FromHtml("#000000");
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].HorizontalAlign = HorizontalAlign.Center;
                            }
                            if (columnName == "Student Mobile No")
                            {
                                string StudentMob = ds.Tables[0].Rows[i]["Student Mobile No"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Text = Convert.ToString(StudentMob);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].ForeColor = ColorTranslator.FromHtml("#000000");
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].HorizontalAlign = HorizontalAlign.Center;
                            }
                            if (columnName == "Account No")
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].CellType = txtacc;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Text = Convert.ToString(Accno);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].ForeColor = ColorTranslator.FromHtml("#000000");
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, columnCount].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                        // FpSpread1.Sheets[0].Cells[aa, 0].Text = sno.ToString();
                        row1++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(tempTotamt);
                        if (!grandtotal.ContainsKey(FpSpread1.Sheets[0].ColumnCount - 1))
                            grandtotal.Add(FpSpread1.Sheets[0].ColumnCount - 1, Convert.ToString(tempTotamt));
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(grandtotal[FpSpread1.Sheets[0].ColumnCount - 1]), out amount);
                            amount += tempTotamt;
                            grandtotal.Remove(FpSpread1.Sheets[0].ColumnCount - 1);
                            grandtotal.Add(FpSpread1.Sheets[0].ColumnCount - 1, Convert.ToString(amount));
                        }
                        check = true;
                    }
                }
            }

            if (check)
            {
                #region grandtotal
                FpSpread1.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
                FpSpread1.Sheets[0].Rows.Count++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].Rows.Count - 1, 0, 1, 3);
                double grandvalue = 0;
                for (int j = 7; j < FpSpread1.Sheets[0].ColumnCount; j++)
                {
                    double.TryParse(Convert.ToString(grandtotal[j]), out grandvalue);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalue);
                }
                #endregion
                FpSpread1.SaveChanges();
                FpSpread1.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                btnxl.Visible = true;
                btnmasterprint.Visible = true;
                errmsg.Visible = false;
                errmsg.Text = "";
            }
            else
            {
                FpSpread1.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                btnmasterprint.Visible = false;
                errmsg.Visible = true;
                errmsg.Text = "No Record Found";
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
        string degreedetails = "Refund Report";
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
        catch { }
    }

    #endregion

    //mode
    protected void loadMode()
    {
        try
        {
            cblmode.Items.Clear();
            cblmode.Items.Add(new System.Web.UI.WebControls.ListItem("Refund", "0"));
            cblmode.Items.Add(new System.Web.UI.WebControls.ListItem("Excess", "1"));
            cblmode.Items.Add(new System.Web.UI.WebControls.ListItem("Scholarship", "2"));
            for (int i = 0; i < cblmode.Items.Count; i++)
            {
                cblmode.Items[i].Selected = true;
            }
            cbmode.Checked = true;
            txtmode.Text = "Mode" + "(" + cblmode.Items.Count + ")";

        }
        catch { }
    }

    protected void cbmode_changed(object sender, EventArgs e)
    {
        clear();
        if (cbmode.Checked == true)
        {
            for (int i = 0; i < cblmode.Items.Count; i++)
            {
                cblmode.Items[i].Selected = true;
            }
            txtmode.Text = "Mode (" + cblmode.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < cblmode.Items.Count; i++)
            {
                cblmode.Items[i].Selected = false;
            }
            txtmode.Text = "---Select---";
        }
    }

    protected void cblmode_selected(object sender, EventArgs e)
    {
        clear();
        txtmode.Text = "---Select---";
        cbmode.Checked = false;
        count = 0;
        for (int i = 0; i < cblmode.Items.Count; i++)
        {
            if (cblmode.Items[i].Selected == true)
            {
                count++;
            }
        }
        if (count > 0)
        {
            txtmode.Text = "Mode (" + count + ")";
            if (count == cblmode.Items.Count)
            {
                cbmode.Checked = true;
            }
        }
    }
    // last modified 02.12.2016 sudhagar

    #region Column order Added by saranya on 9/8/2018

    #region colorder

    protected void lnkcolorder_Click(object sender, EventArgs e)
    {
        txtcolorder.Text = string.Empty;
        loadcolumnorder();
        columnType();
        divcolorder.Attributes.Add("Style", "height: 100%; display:block; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;left: 0px;");
    }

    public void loadcolumnorder()
    {
        cblcolumnorder.Items.Clear();
        cblcolumnorder.Items.Add(new System.Web.UI.WebControls.ListItem("Roll No", "1"));
        cblcolumnorder.Items.Add(new System.Web.UI.WebControls.ListItem("Reg No", "2"));
        cblcolumnorder.Items.Add(new System.Web.UI.WebControls.ListItem("Admission No", "3"));
        cblcolumnorder.Items.Add(new System.Web.UI.WebControls.ListItem("Student Name", "4"));
        cblcolumnorder.Items.Add(new System.Web.UI.WebControls.ListItem("Course", "5"));
        cblcolumnorder.Items.Add(new System.Web.UI.WebControls.ListItem("Department", "6"));
        cblcolumnorder.Items.Add(new System.Web.UI.WebControls.ListItem("Contact Address", "7"));
        cblcolumnorder.Items.Add(new System.Web.UI.WebControls.ListItem("Permanent Address", "8"));
        cblcolumnorder.Items.Add(new System.Web.UI.WebControls.ListItem("Father Name", "9"));
        cblcolumnorder.Items.Add(new System.Web.UI.WebControls.ListItem("Father Mobile No", "10"));
        cblcolumnorder.Items.Add(new System.Web.UI.WebControls.ListItem("Mother Name", "11"));
        cblcolumnorder.Items.Add(new System.Web.UI.WebControls.ListItem("Mother Mobile No", "12"));
        cblcolumnorder.Items.Add(new System.Web.UI.WebControls.ListItem("Student Mobile No", "13"));
    }

    protected Hashtable htcolumnValue()
    {
        Hashtable htcol = new Hashtable();
        try
        {
            htcol.Add("Roll No", "r.roll_no as [Roll No]");
            htcol.Add("Reg No", "r.reg_no as [Reg No]");
            htcol.Add("Admission No", "a.app_formno as [Admission No]");
            htcol.Add("Student Name", "r.stud_name as [Student Name]");
            htcol.Add("Course", "(cast(r.batch_year as nvarchar(10))+'-'+(select c.course_name from degree d,department dt,course c where d.dept_code=dt.dept_code and c.course_id=d.course_id and d.degree_code=isnull( r.degree_code,0))) as [Course]");
            htcol.Add("Department", "(select dt.dept_acronym from degree d,department dt,course c where d.dept_code=dt.dept_code and c.course_id=d.course_id and d.degree_code=isnull( r.degree_code,0)) as [Department]");
            htcol.Add("Permanent Address", "a.parent_addressP+a.Streetp as addressp,cityp as cityp,(Select textval FROM textvaltable T WHERE parent_statep = t.TextCode) as parent_statep, Countryp,a.parent_pincodep");
            htcol.Add("Contact Address", "a.parent_addressC+a.Streetc as addressc,cityc as cityc,(Select textval FROM textvaltable T WHERE parent_statec = t.TextCode) parent_statec,Countryc,a.parent_pincodec");
            htcol.Add("Father Name", "parent_name as [Father Name]");
            htcol.Add("Father Mobile No", "parentF_Mobile as [Father Mobile No]");
            htcol.Add("Mother Name", "mother as [Mother Name]");
            htcol.Add("Mother Mobile No", "parentM_Mobile as [Mother Mobile No]");
            htcol.Add("Student Mobile No", "Student_Mobile as [Student Mobile No]");
        }
        catch { }
        return htcol;
    }

    protected Hashtable htcolumnHeaderValue()
    {

        Hashtable htcol = new Hashtable();
        try
        {
            htcol.Add("r.roll_no as [Roll No]", "Roll No");
            htcol.Add("r.reg_no as [Reg No]", "Reg No");
            htcol.Add("a.app_formno as [Admission No]", "Admission No");
            htcol.Add("r.stud_name as [Student Name]", "Student Name");
            htcol.Add("(cast(r.batch_year as nvarchar(10))+'-'+(select c.course_name from degree d,department dt,course c where d.dept_code=dt.dept_code and c.course_id=d.course_id and d.degree_code=isnull( r.degree_code,0))) as [Course]", "Course");
            htcol.Add("(select dt.dept_acronym from degree d,department dt,course c where d.dept_code=dt.dept_code and c.course_id=d.course_id and d.degree_code=isnull( r.degree_code,0)) as [Department]", "Department");
            htcol.Add("a.parent_addressP+a.Streetp as addressp,cityp as cityp,(Select textval FROM textvaltable T WHERE parent_statep = t.TextCode) as parent_statep, Countryp,a.parent_pincodep", "Permanent Address");
            htcol.Add("Contact Address", "a.parent_addressC+a.Streetc as addressc,cityc as cityc,(Select textval FROM textvaltable T WHERE parent_statec = t.TextCode) parent_statec,Countryc,a.parent_pincodec");
            htcol.Add("parent_name as [Father Name]", "Father Name");
            htcol.Add("parentF_Mobile as [Father Mobile No]", "Father Mobile No");
            htcol.Add("mother as [Mother Name]", "Mother Name");
            htcol.Add("parentM_Mobile as [Mother Mobile No]", "Mother Mobile No");
            htcol.Add("Student_Mobile as [Student Mobile No]", "Student Mobile No");
        }
        catch { }
        return htcol;
    }

    protected Hashtable htcolumnValueOrderBy()
    {
        Hashtable htcolOrderBy = new Hashtable();
        try
        {
            htcolOrderBy.Add("Roll No", "r.roll_no");
            htcolOrderBy.Add("Reg No", "r.reg_no");
            htcolOrderBy.Add("Admission No", "a.app_formno");
            htcolOrderBy.Add("Student Name", "r.stud_name");
            htcolOrderBy.Add("Course", "Course");
            htcolOrderBy.Add("Department", "Department");
            htcolOrderBy.Add("Permanent Address", "addressp,cityp,parent_statep,Countryp,a.parent_pincodep");
            htcolOrderBy.Add("Contact Address", "addressc,Cityc,parent_statec,Countryc,a.parent_pincodec");
            htcolOrderBy.Add("Father Name", "parent_name");
            htcolOrderBy.Add("Father Mobile No", "parentF_Mobile");
            htcolOrderBy.Add("Mother Name", "mother");
            htcolOrderBy.Add("Mother Mobile No", "parentM_Mobile");
            htcolOrderBy.Add("Student Mobile No", "Student_Mobile");
        }
        catch { }
        return htcolOrderBy;
    }

    protected string getSelectedColumnOrderBy(ref string AltColumnOrderby)
    {
        string val = string.Empty;
        try
        {
            StringBuilder strCol = new StringBuilder();
            StringBuilder altstrCol = new StringBuilder();
            Hashtable htcolumn = htcolumnValueOrderBy();
            string Usercollegecode = string.Empty;
            //if (Session["collegecode"] != null)
            //    Usercollegecode = Convert.ToString(Session["collegecode"]);
            if (ddlcollege.Items.Count > 0)
                Usercollegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            string linkName = string.Empty;
            if (ddlMainreport.Items.Count > 0 && ddlMainreport.SelectedItem.Text != "Select")
                linkName = Convert.ToString(ddlMainreport.SelectedItem.Text);
            if (!string.IsNullOrEmpty(linkName) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
            {
                string selQ = d2.GetFunction("  select LinkValue from New_InsSettings where LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "' and user_code='" + usercode + "'");
                if (!string.IsNullOrEmpty(selQ) && selQ != "0")
                {
                    string[] splCol = selQ.Split('$');
                    if (splCol.Length > 0)
                    {
                        bool boolcheck = false;
                        foreach (string spfirst in splCol)
                        {
                            string[] splVal = spfirst.Split(',');
                            if (splVal.Length > 0)
                            {
                                for (int row = 0; row < splVal.Length; row++)
                                {
                                    string tempSel = Convert.ToString(htcolumn[splVal[row].Trim()]);
                                    if (!boolcheck)
                                        strCol.Append(tempSel + ",");
                                    else
                                        altstrCol.Append(tempSel + ",");
                                }
                            }
                            if (strCol.Length > 0)//&& grpstrCol.Length > 0
                            {
                                if (!boolcheck)
                                {
                                    strCol.Remove(strCol.Length - 1, 1);
                                    val = Convert.ToString(strCol);
                                    boolcheck = true;
                                }
                                else
                                {
                                    altstrCol.Remove(altstrCol.Length - 1, 1);
                                    AltColumnOrderby = Convert.ToString(altstrCol);
                                }
                            }
                        }
                    }
                }
            }
        }
        catch { }
        return val;
    }

    protected Hashtable htcolumnValueGroupBy()
    {
        Hashtable htcolGroupBy = new Hashtable();
        try
        {
            htcolGroupBy.Add("Roll No", "r.roll_no");
            htcolGroupBy.Add("Reg No", "r.reg_no");
            htcolGroupBy.Add("Admission No", "a.app_formno");
            htcolGroupBy.Add("Student Name", "r.stud_name");
            htcolGroupBy.Add("Course", "r.batch_year");
            htcolGroupBy.Add("Department", "r.degree_code");
            htcolGroupBy.Add("Permanent Address", "cityp,parent_statep,Countryp,a.parent_pincodep,a.parent_addressp,a.Streetp");
            htcolGroupBy.Add("Contact Address", "Cityc,parent_statec,Countryc,a.parent_pincodec,a.parent_addressC,a.Streetc");
            htcolGroupBy.Add("Father Name", "parent_name");
            htcolGroupBy.Add("Father Mobile No", "parentF_Mobile");
            htcolGroupBy.Add("Mother Name", "mother");
            htcolGroupBy.Add("Mother Mobile No", "parentM_Mobile");
            htcolGroupBy.Add("Student Mobile No", "Student_Mobile");
        }
        catch { }
        return htcolGroupBy;
    }

    protected string getSelectedColumnGoupBy(ref string AltColumnGroupby)
    {
        string val = string.Empty;
        try
        {
            StringBuilder strCol = new StringBuilder();
            StringBuilder altstrCol = new StringBuilder();
            Hashtable htcolumn = htcolumnValueGroupBy();
            string Usercollegecode = string.Empty;
            //if (Session["collegecode"] != null)
            //    Usercollegecode = Convert.ToString(Session["collegecode"]);
            if (ddlcollege.Items.Count > 0)
                Usercollegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            string linkName = string.Empty;
            if (ddlMainreport.Items.Count > 0 && ddlMainreport.SelectedItem.Text != "Select")
                linkName = Convert.ToString(ddlMainreport.SelectedItem.Text);
            if (!string.IsNullOrEmpty(linkName) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
            {
                string selQ = d2.GetFunction("  select LinkValue from New_InsSettings where LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "' and user_code='" + usercode + "'");
                if (!string.IsNullOrEmpty(selQ) && selQ != "0")
                {
                    string[] splCol = selQ.Split('$');
                    if (splCol.Length > 0)
                    {
                        bool boolcheck = false;
                        foreach (string spfirst in splCol)
                        {
                            string[] splVal = spfirst.Split(',');
                            if (splVal.Length > 0)
                            {
                                for (int row = 0; row < splVal.Length; row++)
                                {
                                    string tempSel = Convert.ToString(htcolumn[splVal[row].Trim()]);
                                    if (!string.IsNullOrEmpty(tempSel))
                                    {
                                        if (!boolcheck)
                                            strCol.Append(tempSel + ",");
                                        else
                                            altstrCol.Append(tempSel + ",");
                                    }
                                }
                            }
                            if (strCol.Length > 0)//&& grpstrCol.Length > 0
                            {
                                if (!boolcheck)
                                {
                                    strCol.Remove(strCol.Length - 1, 1);
                                    val = Convert.ToString(strCol);
                                    boolcheck = true;
                                }
                                else
                                {
                                    altstrCol.Remove(altstrCol.Length - 1, 1);
                                    AltColumnGroupby = Convert.ToString(altstrCol);
                                }
                            }
                        }
                    }
                }
            }
        }
        catch { }
        return val;
    }

    protected void btncolorderOK_Click(object sender, EventArgs e)
    {
        // loadcolumns();
        divcolorder.Visible = true;
        if (getsaveColumnOrder())
        {
            divcolorder.Attributes.Add("Style", "display:none;");
        }
    }

    private string getCblSelectedTextwithout(CheckBoxList cblSelected)
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
                        selectedText.Append("," + Convert.ToString(cblSelected.Items[sel].Text));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedText.ToString();
    }

    protected bool getsaveColumnOrder()
    {
        bool boolSave = false;
        try
        {
            string strText = string.Empty;
            string strTextAlt = string.Empty;
            if (cblcolumnorder.Items.Count > 0)
                strText = Convert.ToString(getCblSelectedTextwithout(cblcolumnorder));

            if (!string.IsNullOrEmpty(strText))
                strText = Convert.ToString(txtcolorder.Text);

            string Usercollegecode = string.Empty;
            //if (Session["collegecode"] != null)
            //    Usercollegecode = Convert.ToString(Session["collegecode"]);
            if (ddlcollege.Items.Count > 0)
                Usercollegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            string linkName = string.Empty;
            if (ddlreport.Items.Count > 0 && ddlreport.SelectedItem.Text != "Select")
                linkName = Convert.ToString(ddlreport.SelectedItem.Text);
            if (!string.IsNullOrEmpty(linkName) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0" && !string.IsNullOrEmpty(strText))
            {
                if (!string.IsNullOrEmpty(strTextAlt))
                    strText += "$" + strTextAlt;
                string SelQ = " if exists (select * from New_InsSettings where LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "' and user_code='" + usercode + "')update New_InsSettings set linkvalue='" + strText + "' where  LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "' and user_code='" + usercode + "' else insert into New_InsSettings(LinkName,linkvalue,user_code,college_code) values('" + linkName + "','" + strText + "','" + usercode + "','" + Usercollegecode + "')";
                int insQ = d2.update_method_wo_parameter(SelQ, "Text");
                boolSave = true;
                //getOrderBySelectedColumn();                
            }
            if (!boolSave)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please select corresponding values!')", true);
            }
        }
        catch { }
        return boolSave;
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
            string linkname = "Excess column order settings";
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
                string clsinsert = " if exists(select * from New_InsSettings where LinkName='" + linkname + "') update New_InsSettings set LinkValue='" + columnvalue + "' where LinkName='" + linkname + "' and user_code='" + usercode + "' and college_code='" + collegecode + "' else insert into New_InsSettings (LinkName,LinkValue,usercode,college_code)values('" + linkname + "','" + columnvalue + "','" + usercode + "','" + collegecode + "')";
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

    #region report type added dropdown

    protected void ddlreport_SelectedIndexChanged(object sender, EventArgs e)
    {
        divcolorder.Attributes.Add("Style", "height: 100%; display:block; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;left: 0px;");
        selectReportType();
    }

    protected void btnDel_OnClick(object sender, EventArgs e)
    {
        deleteReportType();
    }
    //type save
    protected void btnaddtype_Click(object sender, EventArgs e)
    {
        try
        {
            string Usercollegecode = string.Empty;
            //if (Session["collegecode"] != null)
            //    Usercollegecode = Convert.ToString(Session["collegecode"]);
            if (ddlcollege.Items.Count > 0)
                Usercollegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            string strDesc = Convert.ToString(txtdesc.Text);
            if (!string.IsNullOrEmpty(strDesc) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
            {
                string linkName = string.Empty;

                linkName = "ExcessReport";
                string sql = "if exists ( select * from CO_MasterValues where MasterValue ='" + strDesc + "' and MasterCriteria ='" + linkName + "' and CollegeCode ='" + Usercollegecode + "') update CO_MasterValues set MasterValue ='" + strDesc + "' where MasterValue ='" + strDesc + "' and MasterCriteria ='" + linkName + "' and CollegeCode ='" + Usercollegecode + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode) values ('" + strDesc + "','" + linkName + "','" + Usercollegecode + "')";
                int insert = d2.update_method_wo_parameter(sql, "Text");
                if (insert > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true); txtdesc.Text = string.Empty;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Enter report type')", true);
            }
            columnType();
            divcolorder.Attributes.Add("Style", "height: 100%; display:block; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;left: 0px;");
        }
        catch { }
    }

    public void columnType()
    {
        string Usercollegecode = string.Empty;
        if (Session["collegecode"] != null)
            Usercollegecode = Convert.ToString(Session["collegecode"]);
        if (ddlcollege.Items.Count > 0)
            Usercollegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
        ddlreport.Items.Clear();
        ddlMainreport.Items.Clear();
        if (!string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
        {
            string linkName = string.Empty;

            linkName = "ExcessReport";
            string query = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria='" + linkName + "' and CollegeCode='" + Usercollegecode + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlreport.DataSource = ds;
                ddlreport.DataTextField = "MasterValue";
                ddlreport.DataValueField = "MasterCode";
                ddlreport.DataBind();
                ddlreport.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));
                //main search filter
                ddlMainreport.DataSource = ds;
                ddlMainreport.DataTextField = "MasterValue";
                ddlMainreport.DataValueField = "MasterCode";
                ddlMainreport.DataBind();
                // ddlMainreport.Items.Insert(0, new ListItem("Select", "0"));
                //getOrderBySelectedColumn();

            }
            else
            {
                ddlreport.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));
                ddlMainreport.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));
            }
        }
    }

    protected void selectReportType()
    {
        try
        {
            bool boolClear = false;
            bool boolcheck = false;
            string getName = string.Empty;
            txtcolorder.Text = string.Empty;
            string strText = string.Empty;
            string Usercollegecode = string.Empty;
            if (ddlcollege.Items.Count > 0)
                Usercollegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            string linkName = string.Empty;
            if (ddlreport.Items.Count > 0 && ddlreport.SelectedItem.Text != "Select")
                linkName = Convert.ToString(ddlreport.SelectedItem.Text);
            string frstName = string.Empty;
            string sndName = string.Empty;
            if (!string.IsNullOrEmpty(linkName) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
            {
                getName = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "'and user_code='" + usercode + "' ");
                if (!string.IsNullOrEmpty(getName) && getName != "0")
                {
                    bool boolcolOrd = false;
                    string[] mainrpt = getName.Split('$');//for two type of column order
                    foreach (string firstN in mainrpt)
                    {
                        string[] splName = firstN.Split(',');
                        if (splName.Length > 0)
                        {
                            if (!boolcolOrd)
                            {
                                frstName = Convert.ToString(mainrpt[0]);
                                for (int sprow = 0; sprow < splName.Length; sprow++)
                                {
                                    for (int flt = 0; flt < cblcolumnorder.Items.Count; flt++)
                                    {
                                        if (splName[sprow].Trim() == cblcolumnorder.Items[flt].Text.Trim())
                                        {
                                            cblcolumnorder.Items[flt].Selected = true;
                                            boolcheck = true;
                                            // strText += cblcolumnorder.Items[flt].Text;
                                        }
                                    }
                                    boolcolOrd = true;
                                    boolClear = true;
                                }
                            }

                        }
                    }
                }
            }
            if (!boolClear)
            {
                txtcolorder.Text = string.Empty;

                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = false;
                }

                cb_column.Checked = false;
            }
            if (boolcheck)
            {
                txtcolorder.Text = frstName;

            }
        }
        catch { }
    }

    protected void deleteReportType()
    {
        int delMQ = 0;
        string Usercollegecode = string.Empty;
        //if (Session["collegecode"] != null)
        //    Usercollegecode = Convert.ToString(Session["collegecode"]);
        if (ddlcollege.Items.Count > 0)
            Usercollegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
        string linkName = string.Empty;
        if (ddlreport.Items.Count > 0 && ddlreport.SelectedItem.Text != "Select")
            linkName = Convert.ToString(ddlreport.SelectedItem.Text);
        if (!string.IsNullOrEmpty(linkName) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
        {
            int delQ = 0;
            string linkNames = string.Empty;

            linkNames = "ExcessReport";
            int.TryParse(Convert.ToString(d2.update_method_wo_parameter("delete from New_InsSettings where LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "' and user_code='" + usercode + "'", "Text")), out delQ);
            int.TryParse(Convert.ToString(d2.update_method_wo_parameter("delete  from CO_MasterValues where MasterCriteria='" + linkNames + "' and mastervalue='" + linkName + "'  and collegecode='" + Usercollegecode + "'", "Text")), out delMQ);
        }
        if (delMQ > 0)
        {
            txtcolorder.Text = string.Empty;
            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
            {
                cblcolumnorder.Items[i].Selected = false;
            }
            cb_column.Checked = false;
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Deleted Successfully')", true);
        }
        else
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Deleted Failed')", true);
        columnType();
        divcolorder.Attributes.Add("Style", "height: 100%; display:block; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;left: 0px;");
    }

    protected void ddlMainreport_Selected(object sender, EventArgs e)
    {
        getOrderBySelectedColumn();

    }

    protected void getOrderBySelectedColumn()
    {
        string val = string.Empty;
        try
        {            //ddlordBy.Items.Clear();
            string Usercollegecode = string.Empty;
            if (ddlcollege.Items.Count > 0)
                Usercollegecode = Convert.ToString(ddlcollege.SelectedValue);
            string linkName = string.Empty;
            if (ddlMainreport.Items.Count > 0 && ddlMainreport.SelectedItem.Text != "Select")
                linkName = Convert.ToString(ddlMainreport.SelectedItem.Text);
            if (!string.IsNullOrEmpty(linkName) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
            {
                string selQ = d2.GetFunction("  select LinkValue from New_InsSettings where LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "' and user_code='" + usercode + "'");
                if (!string.IsNullOrEmpty(selQ) && selQ != "0")
                {
                    string[] splCol = selQ.Split('$');
                    if (splCol.Length > 0)
                    {
                        foreach (string spfirst in splCol)
                        {
                            string[] splVal = spfirst.Split(',');
                            if (splVal.Length > 0)
                            {
                                for (int row = 0; row < splVal.Length; row++)
                                {
                                    string tempName = splVal[row];
                                    //if (splVal[row].Trim() == "Roll No")
                                    //    ddlordBy.Items.Add(new ListItem("Roll No", "r.roll_no"));
                                    //else if (splVal[row].Trim() == "Reg No")
                                    //    ddlordBy.Items.Add(new ListItem("Reg No", "r.reg_no"));
                                    //else if (splVal[row].Trim() == "Admission No")
                                    //    ddlordBy.Items.Add(new ListItem("Admission No", "r.roll_admit"));
                                    //else if (splVal[row].Trim() == "Student Name")
                                    //    ddlordBy.Items.Add(new ListItem("Student Name", "r.stud_name"));
                                    //else if (splVal[row].Trim() == "Section")
                                    //    ddlordBy.Items.Add(new ListItem("Section", "isnull(r.sections,'')"));
                                    //else if (splVal[row].Trim() == "Department")
                                    //    ddlordBy.Items.Add(new ListItem("Department", "[Department]"));
                                    //Department
                                }
                            }
                        }
                    }
                }
            }
        }
        catch { }
        // return val;
    }

    #endregion

    protected string getSelectedColumn(ref string AltColumn)
    {
        string val = string.Empty;
        try
        {
            StringBuilder strCol = new StringBuilder();
            StringBuilder altstrCol = new StringBuilder();
            Hashtable htcolumn = htcolumnValue();
            string Usercollegecode = string.Empty;
            //if (Session["collegecode"] != null)
            //    Usercollegecode = Convert.ToString(Session["collegecode"]);
            if (ddlcollege.Items.Count > 0)
                Usercollegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            string linkName = string.Empty;
            if (ddlMainreport.Items.Count > 0 && ddlMainreport.SelectedItem.Text != "Select")
                linkName = Convert.ToString(ddlMainreport.SelectedItem.Text);
            if (!string.IsNullOrEmpty(linkName) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
            {
                string selQ = d2.GetFunction("  select LinkValue from New_InsSettings where LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "' and user_code='" + usercode + "'");
                if (!string.IsNullOrEmpty(selQ) && selQ != "0")
                {
                    string[] splCol = selQ.Split('$');
                    if (splCol.Length > 0)
                    {
                        bool boolcheck = false;
                        foreach (string spfirst in splCol)
                        {
                            string[] splVal = spfirst.Split(',');
                            if (splVal.Length > 0)
                            {
                                for (int row = 0; row < splVal.Length; row++)
                                {
                                    string tempSel = Convert.ToString(htcolumn[splVal[row].Trim()]);
                                    if (!boolcheck)
                                        strCol.Append(tempSel + ",");
                                    else
                                        altstrCol.Append(tempSel + ",");
                                }
                            }
                            if (strCol.Length > 0)//&& grpstrCol.Length > 0
                            {
                                if (!boolcheck)
                                {
                                    strCol.Remove(strCol.Length - 1, 1);
                                    val = Convert.ToString(strCol);
                                    boolcheck = true;
                                }
                                else
                                {
                                    altstrCol.Remove(altstrCol.Length - 1, 1);
                                    AltColumn = Convert.ToString(altstrCol);
                                }
                            }
                        }
                    }
                }
            }
        }
        catch { }
        return val;
    }

    #endregion
}