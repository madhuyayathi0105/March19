using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.UI;
using System.Text;
using System.Configuration;

public partial class Term_Fee_Report : System.Web.UI.Page
{
    string group_user = "", singleuser = "", usercode = "", collegecode = "", group_code = "";
    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();
    DataSet dsFinal = new DataSet();
    DataSet dsFinal1 = new DataSet();
    Hashtable hast = new Hashtable();
    DataTable dt = new DataTable();
    string acdBatchYear = string.Empty;
    string feecaT = string.Empty;
    Boolean cellclick = false;
    ArrayList colord = new ArrayList();
    static int rightscode = 0;
    static byte roll = 0;
    DataTable dtTermFeeReport = new DataTable();
    DataRow drowInst;
    ArrayList arrColHdrNames = new ArrayList();
    ArrayList arrColHdrNames2 = new ArrayList();
    Dictionary<int, string> dicRowColor = new Dictionary<int, string>();
    static Dictionary<int, string> dicColAlign = new Dictionary<int, string>();
    Dictionary<string, string> dicCellColor = new Dictionary<string, string>();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblmsg.Visible = false;
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
            string grouporusercode = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                usercode = Session["group_code"].ToString();
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                usercode = Session["usercode"].ToString();
            }
            if (!IsPostBack)
            {
                setLabelText();
                bindcollege();
                if (ddlcollege.Items.Count > 0)
                {
                    collegecode = ddlcollege.SelectedItem.Value.ToString();
                }
                loadstrm();
                loadfinanceyear();
                bindbatch();
                binddegree();
                binddept();
                bindsem();
                loadheader();
                grdTermFeeReport.Visible = false;
                loadReason();
                loadcommunity();
                loadColumnOreder();
                columnType();
                getAcademicYear();

            }
            if (ddlcollege.Items.Count > 0)
            {
                collegecode = ddlcollege.SelectedItem.Value.ToString();
            }
            if (cbAcdYear.Checked)
            {
                ddlbatch.Enabled = false;
                txtsem.Enabled = false;
            }
            else
            {
                ddlbatch.Enabled = true;
                txtsem.Enabled = true;
            }
            divcolorder.Attributes.Add("Style", "display:none;");
        }
        catch (Exception ex) { }
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

    #region stream

    public void loadstrm()
    {
        try
        {
            if (ddlcollege.Items.Count > 0)
            {
                collegecode = ddlcollege.SelectedItem.Value.ToString();
            }
            ddlstream.Items.Clear();
            string selqry = "select distinct type  from Course where college_code ='" + collegecode + "' and type<>''";
            ds.Clear();
            ds = da.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlstream.DataSource = ds;
                ddlstream.DataTextField = "type";
                ddlstream.DataValueField = "type";
                ddlstream.DataBind();
            }
            if (ddlstream.Items.Count > 0)
            {
                if (streamEnabled() == 1)
                    ddlstream.Enabled = true;
                else
                    ddlstream.Enabled = false;
            }
            else
                ddlstream.Enabled = false;
            binddegree();
        }
        catch
        { }
    }

    protected void ddlstream_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            binddegree();
            binddept();
        }
        catch { }
    }

    protected void binddegree()
    {
        try
        {
            string clgvalue = ddlcollege.SelectedItem.Value.ToString();
            string stream = string.Empty;
            if (ddlstream.Items.Count > 0)
                stream = ddlstream.SelectedItem.Text.ToString();

            string selqry = "select distinct c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id   and d.college_code='" + clgvalue + "'";
            if (!string.IsNullOrEmpty(stream))
                selqry += " and type  in('" + stream + "')";
            ds.Clear();
            ds = da.select_method_wo_parameter(selqry, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {

                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "Course_Name";
                ddldegree.DataValueField = "Course_Id";
                ddldegree.DataBind();
            }
        }
        catch { }
    }

    #endregion

    #region Bind Method

    protected void bindcollege()
    {
        try
        {
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
            hast.Clear();
            hast.Add("column_field", columnfield.ToString());
            ds = da.select_method("bind_college", hast, "sp");
            ddlcollege.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch { }
    }

    public void bindbatch()
    {
        try
        {
            ddlbatch.Items.Clear();
            ds = da.select_method_wo_parameter("bind_batch", "sp");
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "batch_year";
                ddlbatch.DataValueField = "batch_year";
                ddlbatch.DataBind();
            }
            int count1 = ds.Tables[1].Rows.Count;
            if (count > 0)
            {
                int max_bat = 0;
                max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
                ddlbatch.SelectedValue = max_bat.ToString();
            }

            ddlbatch.Text = "batch(" + 1 + ")";
        }
        catch { }
    }

    protected void binddept()
    {
        try
        {
            hast.Clear();
            usercode = Session["usercode"].ToString();
            // collegecode = Session["collegecode"].ToString();
            collegecode = ddlcollege.SelectedValue.ToString();
            //collegecode = ddldegree.SelectedItem.Value;
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            //hast.Add("single_user", singleuser);
            //hast.Add("group_code", group_user);
            //hast.Add("course_id", ddldegree.SelectedValue);
            //hast.Add("college_code", collegecode);
            //hast.Add("user_code", usercode);
            //ds = da.select_method("bind_branch", hast, "sp");
            //int count2 = ds.Tables[0].Rows.Count;
            ddldept.Items.Clear();
            string degree = string.Empty;
            if (ddldegree.Items.Count > 0)
                degree = Convert.ToString(ddldegree.SelectedValue);
            if (degree != "")
            {
                ds.Clear();
                ds = da.BindBranchMultiple(singleuser, group_user, degree, collegecode, usercode);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddldept.DataSource = ds;
                    ddldept.DataTextField = "dept_name";
                    ddldept.DataValueField = "degree_code";
                    ddldept.DataBind();
                }
            }
        }
        catch { }
    }

    protected void bindsem()
    {
        try
        {
            cblsem.Items.Clear();
            cbsem.Checked = false;
            txtsem.Text = "--Select--";
            ds.Clear();
            string linkName = string.Empty;
            string cbltext = string.Empty;
            ds = da.loadFeecategory(Convert.ToString(ddlcollege.SelectedItem.Value), usercode, ref linkName);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblsem.DataSource = ds;
                cblsem.DataTextField = "TextVal";
                cblsem.DataValueField = "TextCode";
                cblsem.DataBind();

                if (cblsem.Items.Count > 0)
                {
                    for (int i = 0; i < cblsem.Items.Count; i++)
                    {
                        cblsem.Items[i].Selected = true;
                        cbltext = Convert.ToString(cblsem.Items[i].Text);
                    }
                    if (cblsem.Items.Count == 1)
                        txtsem.Text = "" + linkName + "(" + cbltext + ")";
                    else
                        txtsem.Text = "" + linkName + "(" + cblsem.Items.Count + ")";
                    cbsem.Checked = true;
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
    //        string clgvalue = ddlcollege.SelectedItem.Value.ToString();
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
    //                    cblsem.DataSource = ds;
    //                    cblsem.DataTextField = "TextVal";
    //                    cblsem.DataValueField = "TextCode";
    //                    cblsem.DataBind();
    //                }
    //                if (cblsem.Items.Count > 0)
    //                {
    //                    for (int i = 0; i < cblsem.Items.Count; i++)
    //                    {
    //                        cblsem.Items[i].Selected = true;
    //                        sem = Convert.ToString(cblsem.Items[i].Text);
    //                    }
    //                    if (cblsem.Items.Count == 1)
    //                    {
    //                        txtsem.Text = "SemesterandYear(" + sem + ")";
    //                    }
    //                    else
    //                    {
    //                        txtsem.Text = "SemesterandYear(" + cblsem.Items.Count + ")";
    //                    }
    //                    cbsem.Checked = true;
    //                }

    //            }
    //            else
    //            {
    //                cblsem.Items.Clear();
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
    //                            cblsem.DataSource = ds;
    //                            cblsem.DataTextField = "TextVal";
    //                            cblsem.DataValueField = "TextCode";
    //                            cblsem.DataBind();
    //                        }
    //                        if (cblsem.Items.Count > 0)
    //                        {
    //                            for (int i = 0; i < cblsem.Items.Count; i++)
    //                            {
    //                                cblsem.Items[i].Selected = true;
    //                                sem = Convert.ToString(cblsem.Items[i].Text);
    //                            }
    //                            if (cblsem.Items.Count == 1)
    //                            {
    //                                txtsem.Text = "Semester(" + sem + ")";
    //                            }
    //                            else
    //                            {
    //                                txtsem.Text = "Semester(" + cblsem.Items.Count + ")";
    //                            }
    //                            cbsem.Checked = true;
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
    //                            cblsem.DataSource = ds;
    //                            cblsem.DataTextField = "TextVal";
    //                            cblsem.DataValueField = "TextCode";
    //                            cblsem.DataBind();
    //                        }
    //                        if (cblsem.Items.Count > 0)
    //                        {
    //                            for (int i = 0; i < cblsem.Items.Count; i++)
    //                            {
    //                                cblsem.Items[i].Selected = true;
    //                                sem = Convert.ToString(cblsem.Items[i].Text);
    //                            }
    //                            if (cblsem.Items.Count == 1)
    //                            {
    //                                txtsem.Text = "Year(" + sem + ")";
    //                            }
    //                            else
    //                            {
    //                                txtsem.Text = "Year(" + cblsem.Items.Count + ")";
    //                            }
    //                            cbsem.Checked = true;
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch { }
    //}

    public void loadfinanceyear()
    {
        try
        {
            string collegecode = ddlcollege.SelectedValue.ToString();
            string getfinanceyear = "select convert(nvarchar(15),FinYearStart,103) sdate,convert(nvarchar(15),FinYearEnd,103) edate,FinYearPK as acct_id from FM_FinYearMaster where CollegeCode='" + ddlcollege.SelectedValue.ToString() + "'";
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
                    chklsfyear.Items.Insert(0, new System.Web.UI.WebControls.ListItem(fdatye, actid));
                }

                for (int i = 0; i < chklsfyear.Items.Count; i++)
                {
                    chklsfyear.Items[i].Selected = true;
                }
                txtfyear.Text = "Finance Year (" + chklsfyear.Items.Count + ")";
                chkfyear.Checked = true;
            }
            else
            {
                chkfyear.Checked = false;
                txtfyear.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    public void loadheader()
    {
        try
        {
            txtheader.Text = "---Select---";
            chkheader.Checked = false;
            chklsheader.Items.Clear();
            string strheadid = "'" + GetSelectedItemsValueAsString(chklsfyear) + "'";

            if (strheadid.Trim() != "")
            {
                strheadid = " and a.acct_id in (" + strheadid + ")";
            }
            string typeval = "";
            string mode = da.GetFunction("select type from Course where Course_Id='" + ddldegree.SelectedValue.ToString() + "'");
            if (mode.Trim() != "" && mode != null && mode.Trim() != "0")
            {
                //type = " and (HeaderType='" + mode + "' or HeaderType='')";
                typeval = " and c.Stream='" + mode + "'";
            }
            ds.Reset();
            ds.Dispose();
            if (strheadid.Trim() != "")
            {
                //string straccheadquery = "select distinct a.header_id,a.header_name from chlheadersettings c,Acctheader a where c.Header_ID=a.header_id and a.header_name not in ('arrear') " + typeval + " " + strheadid + "";

                //string straccheadquery = "select distinct HeaderPK,HeaderName from FM_HeaderMaster where CollegeCode='" + ddlcollege.SelectedItem.Value + "'";

                string straccheadquery = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + Convert.ToString(Session["usercode"]) + " AND H.CollegeCode = " + ddlcollege.SelectedItem.Value + "";

                // string straccheadquery = "select distinct a.header_id,a.header_name from Acctheader a where a.header_name not in ('arrear') " + strheadid + " "+typeval+"";
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
                    txtheader.Text = "Header (" + chklsheader.Items.Count + ")";
                    chkheader.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    public void loadcommunity()
    {
        try
        {
            string comm = "";
            string selq = "SELECT Distinct community,T.TextVal  FROM applyn A,Registration R,TextValTable T WHERE A.app_no=R.App_No AND T.TextCode =A.community  AND TextVal<>''AND R.college_code ='" + ddlcollege.SelectedItem.Value + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_community.DataSource = ds;
                    cbl_community.DataTextField = "TextVal";
                    cbl_community.DataValueField = "community";
                    cbl_community.DataBind();
                    if (cbl_community.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_community.Items.Count; i++)
                        {
                            cbl_community.Items[i].Selected = true;
                            comm = Convert.ToString(cbl_community.Items[i].Text);
                        }
                        if (cbl_community.Items.Count == 1)
                        {
                            txt_community.Text = "" + comm + "";
                        }
                        else
                        {
                            txt_community.Text = "Community(" + cbl_community.Items.Count + ")";
                        }
                        cb_community.Checked = true;
                    }
                }
            }
            else
            {
                txt_community.Text = "--Select--";
                cb_community.Checked = false;
            }
        }
        catch
        {

        }
    }

    #endregion

    #region DropDownList Events

    protected void ddlcollege_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlcollege.Items.Count > 0)
        {
            collegecode = ddlcollege.SelectedItem.Value.ToString();
        }
        loadstrm();
        bindbatch();
        binddegree();
        binddept();
        bindsem();
        loadheader();
        loadfinanceyear();
        grdTermFeeReport.Visible = false;
        loadReason();
        loadcommunity();
        loadColumnOreder();
        columnType();
    }

    protected void ddlbatch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        binddegree();
        binddept();
        loadheader();
        grdTermFeeReport.Visible = false;
        Excel.Visible = false;
        Print.Visible = false;
        lblreptname.Visible = false;
        txtreptname.Visible = false;
    }

    protected void ddldegree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        loadheader();
        binddept();
        grdTermFeeReport.Visible = false;
        Excel.Visible = false;
        Print.Visible = false;
        lblreptname.Visible = false;
        txtreptname.Visible = false;
    }

    protected void ddldept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        // bindsem();
        grdTermFeeReport.Visible = false;
        Excel.Visible = false;
        Print.Visible = false;
        lblreptname.Visible = false;
        txtreptname.Visible = false;
    }

    #endregion

    #region CheckBox Events

    protected void cbsem_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbsem.Checked == true)
            {
                string hd = "";
                string hed = "";
                int cout = 0;
                for (int i = 0; i < cblsem.Items.Count; i++)
                {
                    cout++;
                    cblsem.Items[i].Selected = true;
                    hd = cblsem.Items[i].Value.ToString();
                    if (hed == "")
                    {
                        hed = hd;
                    }
                    else
                    {
                        hed = hed + "'" + "," + "'" + hd;
                    }
                }
                cbsem.Checked = true;
                txtsem.Text = "Semester(" + cout + ")";
                //binddegree();
            }
            else
            {
                int cout = 0;
                for (int i = 0; i < cblsem.Items.Count; i++)
                {
                    cout++;
                    cblsem.Items[i].Selected = false;
                    cbsem.Checked = false;
                    txtsem.Text = "---Select---";
                }
            }
        }
        catch { }
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
            loadheader();
        }
        catch (Exception ex)
        {
            //errmsg.Visible = true;
            //errmsg.Text = ex.ToString();
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
            loadheader();
        }
        catch { }
    }

    protected void cblsem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            int all = cblsem.Items.Count;
            cbsem.Checked = true;
            for (int i = 0; i < cblsem.Items.Count; i++)
            {
                if (cblsem.Items[i].Selected == true)
                {
                    cout++;
                }
            }
            cbsem.Checked = false;
            if (all == cout)
            {
                cbsem.Checked = true;
            }
            if (cout > 0)
            {
                txtsem.Text = "Semester(" + cout + ")";
            }
            else
            {
                txtsem.Text = "---Select---";
            }
            //binddegree();
            //binddept();
        }
        catch { }
    }

    protected void chkheader_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(chklsheader, chkheader, txtheader, "Header");
    }

    protected void chklsheader_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(chklsheader, chkheader, txtheader, "Header");
    }

    private string GetSelectedItemsValue(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        sbSelected.Append("," + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }

    private string GetSelectedItemsValueAsString(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }

    private string GetSelectedItemsText(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[sel].Text));
                    }
                    else
                    {
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[sel].Text));
                    }
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }

    private List<string> GetSelectedItemsValueList(CheckBoxList cblSelected)
    {
        System.Collections.Generic.List<string> lsSelected = new System.Collections.Generic.List<string>();
        try
        {
            for (int list = 0; list < cblSelected.Items.Count; list++)
            {
                if (cblSelected.Items[list].Selected)
                {
                    lsSelected.Add(cblSelected.Items[list].Value);
                }
            }
        }
        catch { lsSelected.Clear(); }
        return lsSelected;
    }

    private List<string> GetItemsValueList(CheckBoxList cblItems)
    {
        System.Collections.Generic.List<string> lsItems = new System.Collections.Generic.List<string>();
        try
        {
            for (int list = 0; list < cblItems.Items.Count; list++)
            {
                lsItems.Add(cblItems.Items[list].Value);
            }
        }
        catch { lsItems.Clear(); }
        return lsItems;
    }

    private void CallCheckBoxChangedEvent(CheckBoxList cbl, CheckBox cb, TextBox tb, string dispString)
    {
        try
        {
            tb.Text = dispString;
            if (cb.Checked)
            {
                for (int i = 0; i < cbl.Items.Count; i++)
                {
                    cbl.Items[i].Selected = true;
                }
                tb.Text = dispString + "(" + cbl.Items.Count + ")";
            }
            else
            {
                for (int i = 0; i < cbl.Items.Count; i++)
                {
                    cbl.Items[i].Selected = false;
                }
            }
        }
        catch { }
    }

    private void CallCheckBoxListChangedEvent(CheckBoxList cbl, CheckBox cb, TextBox tb, string dispString)
    {
        try
        {
            cb.Checked = false;
            tb.Text = dispString;
            int count = 0;
            for (int i = 0; i < cbl.Items.Count; i++)
            {
                if (cbl.Items[i].Selected == true)
                {
                    count++;
                }
            }
            tb.Text = dispString + "(" + count + ")";
            if (count == cbl.Items.Count)
            {
                cb.Checked = true;
            }
        }
        catch { }
    }



    protected void cb_community_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckBoxChangedEvent(cbl_community, cb_community, txt_community, "Community");
        }
        catch
        {

        }
    }

    protected void cbl_community_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckBoxListChangedEvent(cbl_community, cb_community, txt_community, "Community");
        }
        catch
        {

        }
    }

    #endregion

    protected void log_OnClick(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("default.aspx", false);
        }
        catch { }
    }

    #region Print

    protected void Excel_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblmsg.Visible = false;
            if (txtreptname.Text == "")
            {
                lblmsg.Visible = true;
                lblmsg.Text = "Please Enter the Report Name";
            }
            else
            {
                lblmsg.Visible = false;
                string reportname = txtreptname.Text;
                da.printexcelreportgrid(grdTermFeeReport, reportname);
            }
        }
        catch (Exception ex)
        {
        }
    }

    public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
    { }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string degreedetails = "TERM FEE REGISTER";
        string termdetails2 = "";
        termdetails2 = "ACADEMIC Year :" + " - ";
        string pagename = "Term Fee Report.aspx";
        string termdetails = "COURSE       :" + " " + ddldegree.SelectedItem.Text + "  " + ddldept.SelectedItem.Text + "";

        string printyear = da.GetFunction("select distinct Current_Semester from Registration where Batch_Year='" + ddlbatch.SelectedItem.ToString() + "' and degree_code='" + ddldept.SelectedValue.ToString() + "' and cc=0 and DelFlag=0 and Exam_Flag<>'debar'");
        if (printyear.Trim() == "1" || printyear.Trim() == "2")
        {
            printyear = "I";
        }
        else if (printyear.Trim() == "3" || printyear.Trim() == "4")
        {
            printyear = "II";
        }
        else if (printyear.Trim() == "5" || printyear.Trim() == "6")
        {
            printyear = "III";
        }
        else if (printyear.Trim() == "7" || printyear.Trim() == "8")
        {
            printyear = "IV";
        }

        string termdetails1 = "YEAR            :" + "  " + printyear;
        int prevyear = Convert.ToInt32(ddlbatch.SelectedItem.ToString());
        prevyear--;
        string settingType = string.Empty;
        if (rblTypeNew.SelectedIndex == 0)
            settingType = "0";
        else if (rblTypeNew.SelectedIndex == 1)
            settingType = "1";
        else if (rblTypeNew.SelectedIndex == 2)
            settingType = "2";
        string collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
        string year = Convert.ToString(ddlbatch.SelectedItem.ToString());
        string course_all = string.Empty;
        ArrayList arFeecat = new ArrayList();
        Dictionary<string, string> CurSemDeg = new Dictionary<string, string>();
        string commondist = "";
        course_all = ddldept.SelectedItem.Value;
        string curSem = getCurrentSemester(year, course_all, ref arFeecat, commondist, ref CurSemDeg);//current semester only
        feeCatVal(curSem, arFeecat);
        Dictionary<string, string> htfeecat = new Dictionary<string, string>();
        htfeecat = (Dictionary<string, string>)ViewState["feecat"];
        //     Dictionary<string, string> htfeecat = new Dictionary<string, string>();
        htfeecat = (Dictionary<string, string>)ViewState["feecat"];
        string acdYear = string.Empty;
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
        }
        string selQuery = " SELECT distinct ACD_COLLEGE_CODE,c.collname,ACD_YEAR,ACD_BATCH_YEAR,ACD_FEECATEGORY,t.textval FROM FT_ACADEMICYEAR AY,FT_ACADEMICYEAR_DETAILED AYD,collinfo c,textvaltable t WHERE c.college_code=ay.acd_college_code and AY.ACA_YEAR_PK=AYD.ACA_YEAR_FK and textcriteria='FEECA' and t.textcode=ayd.ACD_FEECATEGORY and t.college_code=ay.ACD_COLLEGE_CODE AND  ACD_COLLEGE_CODE IN('" + collegecode + "') and ACD_BATCH_YEAR in('" + year + "') and ACD_SETTING_TYPE='" + settingType + "' and ACD_FEECATEGORY in('" + feecatval + "') order by ACD_COLLEGE_CODE  asc,ACD_YEAR desc,ACD_FEECATEGORY asc";
        DataSet dsPrevAMount = da.select_method_wo_parameter(selQuery, "Text");
        if (dsPrevAMount.Tables.Count > 0 && dsPrevAMount.Tables[0].Rows.Count > 0)
        {
            acdYear = Convert.ToString(dsPrevAMount.Tables[0].Rows[0]["ACD_YEAR"]);
        }
        string sqlacedicyear = da.GetFunction(" select value from master_settings where settings='Academic year'");
        string[] splitsqlacedicyear = sqlacedicyear.Split(',');
        if (splitsqlacedicyear.GetUpperBound(0) > 0)
        {
            termdetails2 = "ACADEMIC YEAR :" + " " + splitsqlacedicyear[0] + "-" + splitsqlacedicyear[1] + "";

        }
        termdetails2 = "ACADEMIC YEAR :" + " " + acdYear;



        string coltype = da.GetFunction("select type from course where Course_Id='" + ddldegree.SelectedItem.Value + "' and college_code='" + ddlcollege.SelectedItem.Value + "'");//Session["collegecode"].ToString()
        degreedetails = "TERM FEE REGISTER ( " + coltype.ToUpper() + " )" + "@" + termdetails + "@" + termdetails1 + "@" + termdetails2;
        string ss = Session["usercode"].ToString();
        Printcontrol.loadspreaddetails(grdTermFeeReport, pagename, degreedetails, 0, ss);
        Printcontrol.Visible = true;
        lblmsg.Visible = false;
    }

    #endregion

    #region Deduct Reason

    protected void loadReason()
    {
        try
        {
            cbldeduct.Items.Clear();
            ds.Clear();
            string sql = "select TextCode,TextVal from TextValTable where TextCriteria ='DedRe' and college_code ='" + collegecode + "'";
            ds = da.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbldeduct.DataSource = ds;
                cbldeduct.DataTextField = "TextVal";
                cbldeduct.DataValueField = "TextCode";
                cbldeduct.DataBind();
                //for (int i = 0; i < cbldeduct.Items.Count; i++)
                //{
                //    cbldeduct.Items[i].Selected = true;
                //}
                //cbdeduct.Checked = true;
                //txtdeduct.Text = "Deduct Reason(" + cbldeduct.Items.Count + ")";
            }

        }
        catch
        { }
    }

    protected void cbdeduct_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cbdeduct, cbldeduct, txtdeduct, "Deduct Reason", "--Select--");

    }

    protected void cbldeduct_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbdeduct, cbldeduct, txtdeduct, "Deduct Reason", "--Select--");
    }

    #endregion

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
        lbl.Add(lblstr);
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

    #region column order

    protected Hashtable loadColumnOreders()
    {
        Hashtable htcol = new Hashtable();
        try
        {
            htcol.Add("Roll No", "1");
            htcol.Add("Reg No", "2");
            htcol.Add("Amission No", "3");
            htcol.Add("Name", "4");
            htcol.Add("Concession", "5");
            htcol.Add("Community", "6");
        }
        catch { }
        return htcol;
    }

    protected void loadColumnOreder()
    {
        try
        {
            cblcolumnorder.Items.Clear();
            cblcolumnorder.Items.Add(new ListItem("Roll No", "1"));
            cblcolumnorder.Items.Add(new ListItem("Reg No", "2"));
            cblcolumnorder.Items.Add(new ListItem("Amission No", "3"));
            cblcolumnorder.Items.Add(new ListItem("Name", "4"));
            cblcolumnorder.Items.Add(new ListItem("Concession", "5"));
            cblcolumnorder.Items.Add(new ListItem("Community", "6"));
        }
        catch { }
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

    #endregion

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
            //if (roll == 0)
            //{
            //    fpspread.Columns[1].Visible = true;
            //    fpspread.Columns[2].Visible = true;
            //    fpspread.Columns[3].Visible = true;
            //}
            //else if (roll == 1)
            //{
            //    fpspread.Columns[1].Visible = true;
            //    fpspread.Columns[2].Visible = true;
            //    fpspread.Columns[3].Visible = true;
            //}
            //else if (roll == 2)
            //{
            //    fpspread.Columns[1].Visible = true;
            //    fpspread.Columns[2].Visible = false;
            //    fpspread.Columns[3].Visible = false;

            //}
            //else if (roll == 3)
            //{
            //    fpspread.Columns[1].Visible = false;
            //    fpspread.Columns[2].Visible = true;
            //    fpspread.Columns[3].Visible = false;
            //}
            //else if (roll == 4)
            //{
            //    fpspread.Columns[1].Visible = false;
            //    fpspread.Columns[2].Visible = false;
            //    fpspread.Columns[3].Visible = true;
            //}
            //else if (roll == 5)
            //{
            //    fpspread.Columns[1].Visible = true;
            //    fpspread.Columns[2].Visible = true;
            //    fpspread.Columns[3].Visible = false;
            //}
            //else if (roll == 6)
            //{
            //    fpspread.Columns[1].Visible = false;
            //    fpspread.Columns[2].Visible = true;
            //    fpspread.Columns[3].Visible = true;
            //}
            //else if (roll == 7)
            //{
            //    fpspread.Columns[1].Visible = true;
            //    fpspread.Columns[2].Visible = false;
            //    fpspread.Columns[3].Visible = true;
            //}
            //if (!colord.Contains("1"))
            //    fpspread.Sheets[0].Columns[1].Visible = false;
            //if (!colord.Contains("2"))
            //    fpspread.Sheets[0].Columns[2].Visible = false;
            //if (!colord.Contains("3"))
            //    fpspread.Sheets[0].Columns[3].Visible = false;
        }
        catch { }
    }

    #endregion

    private double streamEnabled()
    {
        double strValue = 0;
        double.TryParse(Convert.ToString(da.GetFunction("select LinkValue from New_InsSettings where LinkName='JournalEnableStreamShift' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedValue + "'")), out strValue);
        return strValue;
    }

    protected void btngos_OnClick(object sender, EventArgs e)
    {
        try
        {
            loadcolumns();
            challanAndReceiptNoRights();
            RollAndRegSettings();
            Printcontrol.Visible = false;

            //           fpspread.Sheets[0].ColumnCount = 9;
            //           fpspread.Sheets[0].ColumnHeader.RowCount = 2;
            //           fpspread.Sheets[0].ColumnHeader.Columns[0].Width = 30;
            //           fpspread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            //           fpspread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
            //           fpspread.Sheets[0].ColumnHeader.Columns[0].HorizontalAlign = HorizontalAlign.Center;
            //           fpspread.Sheets[0].ColumnHeader.Columns[1].HorizontalAlign = HorizontalAlign.Center;
            //           fpspread.Sheets[0].ColumnHeader.Columns[2].HorizontalAlign = HorizontalAlign.Center;
            //           fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No.";

            //           fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            //           fpspread.Sheets[0].Columns[1].Visible = true;
            //           if (!colord.Contains("1"))
            //               fpspread.Sheets[0].Columns[1].Visible = false;

            //           if (colord.Count == 0)
            //               fpspread.Sheets[0].Columns[1].Visible = true;

            //           fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
            //           fpspread.Sheets[0].Columns[2].Visible = true;
            //           if (!colord.Contains("2"))
            //               fpspread.Sheets[0].Columns[2].Visible = false;

            //           if (colord.Count == 0)
            //               fpspread.Sheets[0].Columns[2].Visible = true;

            //           fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Admission No";
            //           fpspread.Sheets[0].Columns[3].Visible = true;
            //           if (!colord.Contains("3"))
            //               fpspread.Sheets[0].Columns[3].Visible = false;
            //           if (colord.Count == 0)
            //               fpspread.Sheets[0].Columns[3].Visible = true;

            //           fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Name of the Student ";
            //           fpspread.Sheets[0].Columns[4].Visible = true;
            //           if (!colord.Contains("4"))
            //               fpspread.Sheets[0].Columns[4].Visible = false;
            //           if (colord.Count == 0)
            //               fpspread.Sheets[0].Columns[4].Visible = true;
            //           fpspread.Sheets[0].Columns[4].Width = 250;

            //           fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "FFC/DFC /SFC/VHC /ADV/INST";
            //           fpspread.Sheets[0].Columns[5].Visible = true;
            //           if (!colord.Contains("5"))
            //               fpspread.Sheets[0].Columns[5].Visible = false;
            //           if (colord.Count == 0)
            //               fpspread.Sheets[0].Columns[5].Visible = true;

            //           fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Community";
            //           fpspread.Sheets[0].Columns[6].Visible = true;
            //           if (!colord.Contains("6"))
            //               fpspread.Sheets[0].Columns[6].Visible = false;
            //           if (colord.Count == 0)
            //               fpspread.Sheets[0].Columns[6].Visible = true;


            //           fpspread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Challan/Receipt No";
            //           fpspread.Sheets[0].Columns[7].Visible = true;
            //           if (!colord.Contains("7"))
            //               fpspread.Sheets[0].Columns[7].Visible = false;
            //           if (colord.Count == 0)
            //               fpspread.Sheets[0].Columns[7].Visible = true;

            //           fpspread.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Receipt Date";
            //           fpspread.Sheets[0].Columns[8].Visible = true;
            //           if (!colord.Contains("8"))
            //               fpspread.Sheets[0].Columns[8].Visible = false;
            //           if (colord.Count == 0)
            //               fpspread.Sheets[0].Columns[8].Visible = true;

            //           fpspread.Sheets[0].Columns[1].CellType = txtcelltype;
            //           fpspread.Sheets[0].Columns[2].CellType = txtcelltype;

            //           fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            //           fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            //           fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            //           fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
            //           fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
            //           fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
            //           fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
            //           fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);
            //           fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 2, 1);
            //           fpspread.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            //           fpspread.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            //           fpspread.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            //           fpspread.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;
            //           fpspread.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
            //           fpspread.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;
            //           fpspread.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;
            //           fpspread.Sheets[0].Columns[7].VerticalAlign = VerticalAlign.Middle;

            //           fpspread.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
            //           fpspread.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            //           fpspread.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            //           fpspread.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
            //           fpspread.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
            //           fpspread.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
            //           fpspread.Sheets[0].SetColumnMerge(6, FarPoint.Web.Spread.Model.MergePolicy.Always);
            //           fpspread.Sheets[0].SetColumnMerge(8, FarPoint.Web.Spread.Model.MergePolicy.Always);
            //           spreadColumnVisible();
            //           #region includem

            //           string cc = "";
            //           string debar = "";
            //           string disc = "";
            //           string commondist = "";
            //           if (cblinclude.Items.Count > 0)
            //           {
            //               for (int i = 0; i < cblinclude.Items.Count; i++)
            //               {
            //                   if (cblinclude.Items[i].Selected == true)
            //                   {
            //                       if (cblinclude.Items[i].Value == "1")
            //                       {
            //                           cc = " r.cc=1";
            //                       }
            //                       if (cblinclude.Items[i].Value == "2")
            //                       {
            //                           debar = " r.Exam_Flag like '%debar'";
            //                       }
            //                       if (cblinclude.Items[i].Value == "3")
            //                       {
            //                           disc = "  r.DelFlag=1";
            //                       }
            //                   }
            //               }
            //           }
            //           if (cc != "" && debar == "" && disc == "")
            //               commondist = " and (" + cc + " or r.cc=0)  and r.Exam_Flag<>'debar' and r.DelFlag=0";

            //           if (cc == "" && debar != "" && disc == "")
            //               commondist = " and r.cc=0  and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";

            //           if (cc == "" && debar == "" && disc != "")
            //               commondist = " and r.cc=0  and r.Exam_Flag<>'debar' and (" + disc + " or r.DelFlag=0)";

            //           if (cc != "" && debar != "" && disc == "")
            //               commondist = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";

            //           if (cc == "" && debar != "" && disc != "")
            //               commondist = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar')  and (" + disc + " or r.DelFlag=0)";

            //           if (cc != "" && debar == "" && disc != "")
            //               commondist = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar'  and (" + disc + " or r.DelFlag=0)";

            //           else if (cc == "" && debar == "" && disc == "")
            //               commondist = " and r.cc=0  and r.Exam_Flag<>'debar' and r.DelFlag=0";

            //           if (cc != "" && debar != "" && disc != "")
            //               commondist = "";

            //           #endregion

            //           fpspread.SaveChanges();
            //           Hashtable hashset = new Hashtable();

            //           Dictionary<string, Double> dicdedction = new Dictionary<string, double>();


            //           string feecaterory = GetSelectedItemsValue(cblsem);
            //           string comm = GetSelectedItemsValue(cbl_community);
            //           string commvalue = "";
            //           if (comm != "")
            //               commvalue = " and a.community in(" + comm + ")";
            //           string feecatval = feecaterory;

            //           if (feecaterory.Trim() != "")
            //           {
            //               feecaterory = " and d.FeeCategory in(" + feecaterory + ")";
            //           }


            //           string headval = GetSelectedItemsValue(chklsheader);
            //           string finyr = "";
            //           for (int a = 0; a < chklsfyear.Items.Count; a++)
            //           {
            //               if (chklsfyear.Items[a].Selected == true)
            //               {
            //                   if (finyr == "")
            //                   {
            //                       finyr = chklsfyear.Items[a].Value.ToString();
            //                   }
            //                   else
            //                   {
            //                       finyr = finyr + "','" + chklsfyear.Items[a].Value.ToString();
            //                   }
            //               }
            //           }
            //           // string fnlyear = " and finyearfk in('" + finyr + "')";
            //           string strheadrequery = "";
            //           int noofhear = GetSelectedItemsValueList(chklsheader).Count;

            //           if (headval.Trim() != "")
            //           {
            //               strheadrequery = " and d.HeaderFK in(" + headval + ")";
            //           }
            //           else
            //           {
            //               lblmsg.Visible = true;
            //               lblmsg.Text = "Please Select The Header And Then Proceed";
            //               return;
            //           }
            //           noofhear++;
            //           //paid amount with header
            //           string strqurey = "select distinct r.Roll_No,r.roll_admit,d.FeeCategory as fee_category,d.DDNo as challan_no,d.HeaderFK as  header_id,CONVERT(nvarchar(15),d.TransDate,101) as cal_date,sum(d.Debit) paid,d.PayMode,d.TransCode from registration r,FT_FinDailyTransaction d,FM_LedgerMaster f where f.LedgerPK=d.LedgerFK and f.HeaderFK=d.HeaderFK  and r.App_No =d.App_No and r.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and r.degree_code='" + ddldept.SelectedValue + "'  " + strheadrequery + " " + feecaterory + " " + commondist + " and d.finyearfk in('" + finyr + "') and d.Credit=0 and TransType=1  and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' group by r.Roll_No,d.FeeCategory,d.DDNo,d.HeaderFK,d.TransDate,d.PayMode,d.TransCode,r.roll_admit ";
            //           //and d.Debit>0
            //           //findaily transaction
            //           strqurey = strqurey + " select distinct Roll_No,roll_admit,Reg_No,d.app_no,Stud_Name,d.FeeCategory,Current_Semester,d.DDNo as challan_no,COnvert(nvarchar(15),d.TransDate,101) as challandate,COnvert(nvarchar(15),d.dddate,101) as rcptdate ,d.PayMode,d.TransCode,CC,DelFlag,Exam_Flag from Registration r,FT_FinDailyTransaction d where d.App_No =r.App_No and isnull(IsCanceled,'0')='0'  and  ISNULL(IsCollected,0)='1' and r.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' " + commondist + " and d.finyearfk in('" + finyr + "') and r.degree_code='" + ddldept.SelectedValue + "'  " + strheadrequery + " " + feecaterory + "  order by Roll_No,DDNo";

            //           //deduction
            //           strqurey = strqurey + " select distinct ISNULL(t.TextVal,'') as detection,r.Roll_No  from FT_FeeAllot a,TextValTable t,Registration r where r.App_No=a.App_No and convert(nvarchar(50),t.TextCode)=convert(nvarchar(50),a.DeductReason) and r.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and a.finyearfk in('" + finyr + "') and r.degree_code='" + ddldept.SelectedValue + "' " + commondist + " and ISNULL(DeductReason,'') <> '' ";
            //           //registration
            //           strqurey = strqurey + " select distinct Roll_No,roll_admit,Reg_No,r.app_no,r.Stud_Name,r.Current_Semester,CC,DelFlag,Exam_Flag from Registration r,applyn a where r.app_no=a.app_no and  r.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and r.degree_code='" + ddldept.SelectedValue + "' " + commondist + " " + commvalue + " order by Roll_No";

            //           //community applyn
            //           strqurey += " select community,app_no from applyn where degree_code='" + ddldept.SelectedValue + "' and batch_year='" + ddlbatch.SelectedValue.ToString() + "'";
            //           strqurey += " select textval,textcode from textvaltable where textcriteria='comm' and college_code='" + ddlcollege.SelectedItem.Value + "'";

            //           ds.Dispose();
            //           ds.Reset();
            //           ds = da.select_method_wo_parameter(strqurey, "Text");

            //           Hashtable htsem = new Hashtable();
            //           Dictionary<int, Double> dictotal = new Dictionary<int, double>();
            //           List<string> lstSemester = GetSelectedItemsValueList(cblsem);
            //           for (int i = 0; i < cblsem.Items.Count; i++)
            //           {
            //               if (cblsem.Items[i].Selected == true)
            //               {
            //                   // string feecate = lstSemester[i];
            //                   for (int h = 0; h < chklsheader.Items.Count; h++)
            //                   {
            //                       if (chklsheader.Items[h].Selected == true)
            //                       {
            //                           fpspread.Sheets[0].ColumnCount++;
            //                           fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnCount - 1].Text = chklsheader.Items[h].Text;
            //                           fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnCount - 1].Tag = chklsheader.Items[h].Value;
            //                           fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnCount - 1].Note = Convert.ToString(cblsem.Items[i].Value);
            //                           dictotal.Add(fpspread.Sheets[0].ColumnCount - 1, 0);
            //                       }
            //                   }
            //                   fpspread.Sheets[0].ColumnCount++;
            //                   fpspread.Sheets[0].ColumnHeader.Cells[0, fpspread.Sheets[0].ColumnCount - noofhear].Text = Convert.ToString(cblsem.Items[i].Text);
            //                   htsem.Add(Convert.ToString(cblsem.Items[i].Value), fpspread.Sheets[0].ColumnCount - noofhear);
            //                   fpspread.Sheets[0].ColumnHeader.Cells[1, fpspread.Sheets[0].ColumnCount - 1].Text = "Total";
            //                   fpspread.Sheets[0].ColumnHeaderSpanModel.Add(0, fpspread.Sheets[0].ColumnCount - noofhear, 1, noofhear);
            //                   dictotal.Add(fpspread.Sheets[0].ColumnCount - 1, 0);
            //               }
            //           }
            //           int srno = 0;
            //           Hashtable hatstu = new Hashtable();
            //           Boolean rowflag = false;
            //           Hashtable ht = new Hashtable();
            //           DataView dvstd = new DataView();
            //           DataView dvcom = new DataView();
            //           DataView dvtxt = new DataView();
            //           //for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
            //           //{
            //           for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
            //           {
            //               Boolean stuflag = false;
            //               ds.Tables[1].DefaultView.RowFilter = " ROll_no='" + Convert.ToString(ds.Tables[3].Rows[i]["ROll_no"]) + "'";
            //               dvstd = ds.Tables[1].DefaultView;
            //               if (dvstd.Count > 0)
            //               {
            //                   #region paid details

            //                   double delflag = 0;
            //                   for (int cnt = 0; cnt < dvstd.Count; cnt++)
            //                   {
            //                       stuflag = false;
            //                       string rollno = dvstd[cnt]["Roll_No"].ToString();
            //                       string regno = dvstd[cnt]["Reg_No"].ToString();
            //                       string rolladmit = dvstd[cnt]["roll_admit"].ToString();
            //                       string appno = dvstd[cnt]["app_no"].ToString();
            //                       string name = dvstd[cnt]["Stud_Name"].ToString();
            //                       string currentsem = dvstd[cnt]["Current_Semester"].ToString();
            //                       string challno = dvstd[cnt]["challan_no"].ToString();
            //                       string chaldate = dvstd[cnt]["challandate"].ToString();
            //                       string transdate = dvstd[cnt]["rcptdate"].ToString();
            //                       string rcptno = dvstd[cnt]["TransCode"].ToString();
            //                       double.TryParse(dvstd[cnt]["DelFlag"].ToString(), out delflag);
            //                       DateTime dtca = Convert.ToDateTime(chaldate);
            //                       DateTime dtrcpt = Convert.ToDateTime(transdate);
            //                       string dedcureason = "";
            //                       ds.Tables[2].DefaultView.RowFilter = "Roll_No='" + rollno + "'";
            //                       DataView dvdeductreason = ds.Tables[2].DefaultView;
            //                       if (dvdeductreason.Count > 0)
            //                       {
            //                           for (int k = 0; k < dvdeductreason.Count; k++)
            //                           {
            //                               if (dedcureason == "")
            //                                   dedcureason = dvdeductreason[k]["detection"].ToString();
            //                               else
            //                                   dedcureason += "," + dvdeductreason[k]["detection"].ToString();
            //                           }
            //                           // dedcureason = dvdeductreason[0]["detection"].ToString();
            //                       }
            //                       string community = "";
            //                       if (ds.Tables[4].Rows.Count > 0 && ds.Tables[4].Rows.Count > 0)
            //                       {
            //                           ds.Tables[4].DefaultView.RowFilter = "app_no='" + appno + "'";
            //                           dvcom = ds.Tables[4].DefaultView;
            //                           if (dvcom.Count > 0)
            //                           {
            //                               string code = Convert.ToString(dvcom[0]["community"]);
            //                               ds.Tables[5].DefaultView.RowFilter = "textcode='" + code + "'";
            //                               dvtxt = ds.Tables[5].DefaultView;
            //                               if (dvtxt.Count > 0)
            //                               {
            //                                   community = Convert.ToString(dvtxt[0]["textval"]);
            //                               }
            //                           }
            //                       }
            //                       Double tottaount = 0;
            //                       string feecatg = "";
            //                       for (int c = 9; c < fpspread.Sheets[0].ColumnCount; c++)
            //                       {
            //                           if (fpspread.Sheets[0].ColumnHeader.Cells[1, c].Text != "Total")
            //                           {
            //                               string headid = fpspread.Sheets[0].ColumnHeader.Cells[1, c].Tag.ToString();
            //                               string feecat = fpspread.Sheets[0].ColumnHeader.Cells[1, c].Note.ToString();
            //                               if (challno.Trim() != "")
            //                               {
            //                                   ds.Tables[0].DefaultView.RowFilter = "fee_category='" + feecat + "' and header_id='" + headid + "' and challan_no='" + challno + "' and cal_date='" + dtca.ToString("MM/dd/yyyy") + "' and Roll_no='" + rollno + "'";
            //                               }
            //                               else
            //                               {
            //                                   ds.Tables[0].DefaultView.RowFilter = "fee_category='" + feecat + "' and header_id='" + headid + "' and TransCode='" + rcptno + "' and cal_date='" + dtca.ToString("MM/dd/yyyy") + "' and Roll_no='" + rollno + "'";
            //                               }
            //                               DataView dvfeecat = ds.Tables[0].DefaultView;
            //                               //  fpspread.Sheets[0].RowCount++;
            //                               if (dvfeecat.Count > 0)
            //                               {
            //                                   if (stuflag == false)
            //                                   {
            //                                       if (!hatstu.Contains(rollno))
            //                                       {
            //                                           srno++;
            //                                           hatstu.Add(rollno, rollno);
            //                                       }
            //                                       fpspread.Sheets[0].RowCount++;
            //                                       fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
            //                                       fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Center;
            //                                       fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].CellType = txtcelltype;
            //                                       fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].CellType = txtcelltype;
            //                                       fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].CellType = txtcelltype;
            //                                       fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].CellType = txtcelltype;
            //                                       fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].CellType = txtcelltype;
            //                                       fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].CellType = txtcelltype;
            //                                       fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].CellType = txtcelltype;
            //                                       fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].CellType = txtcelltype;
            //                                       fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Text = rollno;
            //                                       fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Text = regno;
            //                                       fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].Text = rolladmit;
            //                                       fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Text = name;
            //                                       fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].Text = dedcureason;
            //                                       if (dedcureason.Trim() == "-")
            //                                           fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
            //                                       else
            //                                           fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
            //                                       //community
            //                                       fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].Text = community;

            //                                       //if (challno.Trim() != "")
            //                                       //    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].Text = challno;

            //                                       //else
            //                                       //    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].Text = rcptno;

            //                                       string chaandrpt = "";
            //                                       if (rightscode == 3 || rightscode == 1 || rightscode == 2)
            //                                       {
            //                                           if (rcptno != "" && challno == "")
            //                                               chaandrpt = rcptno;

            //                                           if (rcptno == "" && challno != "")
            //                                               chaandrpt = challno;

            //                                           if (rcptno != "" && challno != "")
            //                                               chaandrpt = challno + "/" + rcptno;


            //                                           if (rcptno == "" && challno == "")
            //                                               chaandrpt = challno;
            //                                       }
            //                                       if (rightscode == 1)
            //                                           fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Text = rcptno;

            //                                       if (rightscode == 2 || rightscode == 0)
            //                                           fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Text = challno;


            //                                       if (rightscode == 3)
            //                                           fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Text = chaandrpt;


            //                                       fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 8].Text = dtca.ToString("dd/MM/yyyy");


            //                                       stuflag = true;
            //                                   }
            //                                   rowflag = true;
            //                                   string paidamount = dvfeecat[0]["Paid"].ToString();
            //                                   fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, c].Text = paidamount;
            //                                   fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Center;


            //                                   tottaount = tottaount + Convert.ToDouble(paidamount);
            //                                   if (dictotal.ContainsKey(c))
            //                                   {
            //                                       Double getval = dictotal[c] + Convert.ToDouble(paidamount);
            //                                       dictotal[c] = getval;
            //                                   }
            //                                   else
            //                                       dictotal.Add(c, Convert.ToDouble(paidamount));

            //                                   if (dedcureason.Trim() != "-")
            //                                   {
            //                                       if (dicdedction.ContainsKey(dedcureason.Trim().ToLower() + feecat))
            //                                       {
            //                                           Double getdet = dicdedction[dedcureason.Trim().ToLower() + feecat] + Convert.ToDouble(paidamount);
            //                                           dicdedction[dedcureason.Trim().ToLower() + feecat] = getdet;
            //                                       }
            //                                       else
            //                                           dicdedction.Add(dedcureason.Trim().ToLower() + feecat, Convert.ToDouble(paidamount));
            //                                   }
            //                               }

            //                           }
            //                           else
            //                           {
            //                               if (stuflag == true)
            //                               {
            //                                   if (tottaount > 0)
            //                                   {
            //                                       fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, c].Text = tottaount.ToString();
            //                                   }
            //                                   else
            //                                   {
            //                                       fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, c].Text = "-";
            //                                   }
            //                                   fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Center;
            //                                   if (dictotal.ContainsKey(c))
            //                                   {
            //                                       Double getval = dictotal[c] + tottaount;
            //                                       dictotal[c] = getval;
            //                                   }
            //                                   else
            //                                       dictotal.Add(c, tottaount);

            //                               }
            //                               tottaount = 0;
            //                           }
            //                       }
            //                   }
            //                   #endregion
            //               }
            //               else
            //               {
            //                   double delflag = 0;
            //                   //without paid details
            //                   #region without paid details

            //                   string rollno = ds.Tables[3].Rows[i]["Roll_No"].ToString();
            //                   string regno = ds.Tables[3].Rows[i]["Reg_No"].ToString();
            //                   string rolladmit = ds.Tables[3].Rows[i]["roll_admit"].ToString();
            //                   string appno = ds.Tables[3].Rows[i]["app_no"].ToString();
            //                   string name = ds.Tables[3].Rows[i]["Stud_Name"].ToString();
            //                   string currentsem = ds.Tables[3].Rows[i]["Current_Semester"].ToString();
            //                   // string delflag = Convert.ToString(ds.Tables[3].Rows[i]["DelFlag"]);
            //                   double.TryParse(Convert.ToString(ds.Tables[3].Rows[i]["DelFlag"]), out delflag);
            //                   string dedcureason = "";
            //                   ds.Tables[2].DefaultView.RowFilter = "Roll_No='" + rollno + "'";
            //                   DataView dvdeductreason = ds.Tables[2].DefaultView;
            //                   if (dvdeductreason.Count > 0)
            //                   {
            //                       for (int k = 0; k < dvdeductreason.Count; k++)
            //                       {
            //                           if (dedcureason == "")
            //                               dedcureason = dvdeductreason[k]["detection"].ToString();
            //                           else
            //                               dedcureason += "," + dvdeductreason[k]["detection"].ToString();
            //                       }
            //                       // dedcureason = dvdeductreason[0]["detection"].ToString();
            //                   }

            //                   string community = "";
            //                   if (ds.Tables[4].Rows.Count > 0 && ds.Tables[4].Rows.Count > 0)
            //                   {
            //                       ds.Tables[4].DefaultView.RowFilter = "app_no='" + appno + "'";
            //                       dvcom = ds.Tables[4].DefaultView;
            //                       if (dvcom.Count > 0)
            //                       {
            //                           string code = Convert.ToString(dvcom[0]["community"]);
            //                           ds.Tables[5].DefaultView.RowFilter = "textcode='" + code + "'";
            //                           dvtxt = ds.Tables[5].DefaultView;
            //                           if (dvtxt.Count > 0)
            //                           {
            //                               community = Convert.ToString(dvtxt[0]["textval"]);
            //                           }
            //                       }
            //                   }
            //                   for (int c = 9; c < fpspread.Sheets[0].ColumnCount; c++)
            //                   {
            //                       if (fpspread.Sheets[0].ColumnHeader.Cells[1, c].Text != "Total")
            //                       {
            //                           if (stuflag == false)
            //                           {
            //                               if (!hatstu.Contains(rollno))
            //                               {
            //                                   srno++;
            //                                   hatstu.Add(rollno, rollno);
            //                               }
            //                               string headid = fpspread.Sheets[0].ColumnHeader.Cells[1, c].Tag.ToString();
            //                               string feecat = fpspread.Sheets[0].ColumnHeader.Cells[1, c].Note.ToString();
            //                               fpspread.Sheets[0].RowCount++;
            //                               fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
            //                               fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Center;
            //                               fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].CellType = txtcelltype;
            //                               fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].CellType = txtcelltype;
            //                               fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].CellType = txtcelltype;
            //                               fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].CellType = txtcelltype;
            //                               fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].CellType = txtcelltype;
            //                               fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].CellType = txtcelltype;
            //                               fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].CellType = txtcelltype;
            //                               fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].CellType = txtcelltype;
            //                               fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 1].Text = rollno;
            //                               fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 2].Text = regno;
            //                               fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 3].Text = rolladmit;
            //                               fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 4].Text = name;
            //                               fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].Text = dedcureason;
            //                               fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
            //                               fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].Text = community;
            //                               fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
            //                               fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].Text = "-";
            //                               fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
            //                               fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 8].Text = "-";
            //                               fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
            //                               stuflag = true;
            //                           }
            //                           fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, c].Text = "-";
            //                           fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Center;
            //                       }
            //                       else
            //                       {
            //                           fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, c].Text = "-";
            //                           fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Center;
            //                       }
            //                   }
            //                   #endregion
            //               }
            //           }
            //           if (rowflag == true)
            //           {
            //               fpspread.Sheets[0].RowCount++;
            //               fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "Total";
            //               fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Font.Bold = true;
            //               fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
            //               fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
            //               fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, 9);
            //               fpspread.Sheets[0].Rows[fpspread.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            //               for (int c = 8; c < fpspread.Sheets[0].ColumnCount; c++)
            //               {
            //                   Double getot = 0;
            //                   if (dictotal.ContainsKey(c))
            //                   {
            //                       getot = dictotal[c];
            //                   }
            //                   fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, c].Text = getot.ToString();
            //                   fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Center;
            //                   fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, c].Font.Bold = true;
            //                   fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, c].Font.Size = FontUnit.Medium;
            //               }

            //               // strqurey = "select distinct ISNULL(t.TextVal,'') as detection from fee_allot a,TextValTable t,Registration r where r.Roll_Admit=a.roll_admit and convert(nvarchar(50),t.TextCode)=convert(nvarchar(50),a.dedect_reason) and r.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and r.degree_code='" + ddldept.SelectedValue + "' and ISNULL(dedect_reason,'') <> '' and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar'";
            //               ds.Dispose();
            //               ds.Reset();
            //               ds = da.select_method_wo_parameter(strqurey, "Text");
            //               if (ds.Tables[0].Rows.Count > 0)
            //               {
            //                   fpspread.Sheets[0].RowCount++;
            //                   fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "CONCESSION";
            //                   fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Font.Bold = true;
            //                   fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
            //                   fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
            //                   fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, fpspread.Sheets[0].ColumnCount);
            //                   for (int d = 0; d < ds.Tables[0].Rows.Count; d++)
            //                   {
            //                       fpspread.Sheets[0].RowCount++;
            //                       string dection = ds.Tables[0].Rows[d]["detection"].ToString();
            //                       for (int c = 8; c < fpspread.Sheets[0].ColumnCount; c = c + noofhear)
            //                       {
            //                           string feecat = fpspread.Sheets[0].ColumnHeader.Cells[1, c].Note.ToString();
            //                           string setval = dection + feecat;

            //                           if (dicdedction.ContainsKey(setval))
            //                           {
            //                               Double getval = dicdedction[setval];
            //                               fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, c].Text = dection.ToUpper().ToString();
            //                               fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, c].Font.Bold = true;
            //                               fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, c].Font.Size = FontUnit.Medium;
            //                               fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Right;

            //                               fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, c + 1].Text = getval.ToString();
            //                               fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, c + 1].Font.Bold = true;
            //                               fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, c + 1].Font.Size = FontUnit.Medium;
            //                               fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, c + 1].HorizontalAlign = HorizontalAlign.Left;
            //                               fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, c + 1, 1, noofhear - 1);
            //                           }
            //                       }
            //                   }
            //               }

            //               #region final column add old
            //               //  final columns
            //               int cnt = 0;
            //               int totad = 0;
            //               int inst = 0;
            //               int ffc = 0;
            //               int sfc = 0;
            //               int left = 0;
            //               bool check = false;
            //               DataSet dsload = new DataSet();

            //               //Hashtable htdet = new Hashtable();
            //               Dictionary<string, int> htdet = new Dictionary<string, int>();
            //               // Hashtable htdetvalue = new Hashtable();
            //               string DeductR = "";
            //               for (int a = 0; a < cbldeduct.Items.Count; a++)
            //               {
            //                   if (cbldeduct.Items[a].Selected == true)
            //                   {
            //                       if (DeductR == "")
            //                       {
            //                           DeductR = cbldeduct.Items[a].Text.ToString();
            //                       }
            //                       else
            //                       {
            //                           DeductR = DeductR + "','" + cbldeduct.Items[a].Text.ToString();
            //                       }
            //                   }
            //               }
            //               for (int sem = 0; sem < cblsem.Items.Count; sem++)
            //               {
            //                   if (cblsem.Items[sem].Selected == true)
            //                   {
            //                       Dictionary<string, int> htdetvalue = new Dictionary<string, int>();
            //                       double totfnlval = 0;
            //                       cnt++;
            //                       int colcnt = Convert.ToInt32(htsem[Convert.ToString(cblsem.Items[sem].Value)]);
            //                       if (cnt == 1)
            //                           totad = fpspread.Sheets[0].RowCount++;

            //                       fpspread.Sheets[0].Cells[totad, colcnt].Text = "Total Advance";
            //                       fpspread.Sheets[0].Cells[totad, colcnt].Font.Bold = true;
            //                       fpspread.Sheets[0].Cells[totad, colcnt].Font.Size = FontUnit.Medium;
            //                       fpspread.Sheets[0].Cells[totad, colcnt].HorizontalAlign = HorizontalAlign.Right;
            //                       fpspread.Sheets[0].SpanModel.Add(totad, colcnt, 1, 1);

            //                       string SelectQ = " select SUM(TotalAmount) as Demand, f.App_No,r.degree_code,r.Batch_Year,f.FeeCategory from FT_FeeAllot f,Registration r where f.App_No =r.App_No " + commondist + " and r.Batch_Year in('" + ddlbatch.SelectedValue.ToString() + "') and r.degree_code in('" + ddldept.SelectedValue + "') and f.FeeCategory in(" + Convert.ToString(cblsem.Items[sem].Value) + ") and f.HeaderFK in(" + headval + ") and f.FinYearFK in('" + finyr + "') group by f.App_No,r.degree_code,r.Batch_Year,f.FeeCategory";
            //                       //FeeCategory in(" + feecaterory + ")
            //                       //paid
            //                       SelectQ += " select SUM(Debit) as Paid, f.App_No,f.FeeCategory,r.degree_code,r.Batch_Year from FT_FinDailyTransaction f,Registration r where f.App_No =r.App_No " + commondist + " and isnull(IsCanceled,'0')='0'  and  ISNULL(IsCollected,0)='1' and r.Batch_Year in('" + ddlbatch.SelectedValue.ToString() + "') and r.degree_code in('" + ddldept.SelectedValue + "') and f.FeeCategory in(" + Convert.ToString(cblsem.Items[sem].Value) + ") and f.HeaderFK in(" + headval + ") and f.FinYearFK in('" + finyr + "') group by f.App_No,f.FeeCategory,r.degree_code,r.Batch_Year order by f.App_No,f.FeeCategory asc";
            //                       SelectQ += " select r.App_No,DelFlag,r.Current_Semester,CC,Exam_Flag,r.degree_code,r.Batch_Year from Registration r,applyn a where r.app_no=a.app_no and r.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and r.degree_code='" + ddldept.SelectedValue + "' " + commondist + " " + commvalue + "";
            //                       SelectQ += " select ISNULL(t.TextVal,'') as detection,a.App_No,r.degree_code,r.Batch_Year,a.FeeCategory  from FT_FeeAllot a,TextValTable t,Registration r where r.App_No=a.App_No and convert(nvarchar(50),t.TextCode)=convert(nvarchar(50),a.DeductReason) and r.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and a.finyearfk in('" + finyr + "') and r.degree_code='" + ddldept.SelectedValue + "'  " + commondist + " and t.textval in('" + DeductR + "') and ISNULL(DeductReason,'') <> '' and isnull(deductamout,0)<>'0' ";
            //                       dsload.Clear();
            //                       dsload = da.select_method_wo_parameter(SelectQ, "Text");

            //                       int valcol = colcnt;
            //                       if (cnt == 1)
            //                           inst = fpspread.Sheets[0].RowCount++;

            //                       fpspread.Sheets[0].Cells[inst, colcnt].Text = "Total Installment";
            //                       fpspread.Sheets[0].Cells[inst, colcnt].Font.Bold = true;
            //                       fpspread.Sheets[0].Cells[inst, colcnt].Font.Size = FontUnit.Medium;
            //                       fpspread.Sheets[0].Cells[inst, colcnt].HorizontalAlign = HorizontalAlign.Right;
            //                       fpspread.Sheets[0].SpanModel.Add(inst, colcnt, 1, 1);

            //                       //dynamic column adding
            //                       if (cbldeduct.Items.Count > 0)
            //                       {
            //                           for (int det = 0; det < cbldeduct.Items.Count; det++)
            //                           {
            //                               if (cbldeduct.Items[det].Selected == true)
            //                               {
            //                                   if (check == false)
            //                                   {
            //                                       ffc = fpspread.Sheets[0].RowCount++;
            //                                       htdet.Add(Convert.ToString(cbldeduct.Items[det].Text), ffc);
            //                                   }
            //                                   else if (!htdet.ContainsKey(Convert.ToString(cblsem.Items[sem].Value)))
            //                                   {
            //                                       if (htdet.Count > 0)
            //                                           ffc = Convert.ToInt32(htdet[Convert.ToString(cbldeduct.Items[det].Text)]);
            //                                   }
            //                                   fpspread.Sheets[0].Cells[ffc, colcnt].Text = Convert.ToString(cbldeduct.Items[det].Text);
            //                                   fpspread.Sheets[0].Cells[ffc, colcnt].Font.Bold = true;
            //                                   fpspread.Sheets[0].Cells[ffc, colcnt].Font.Size = FontUnit.Medium;
            //                                   fpspread.Sheets[0].Cells[ffc, colcnt].HorizontalAlign = HorizontalAlign.Right;
            //                                   fpspread.Sheets[0].SpanModel.Add(ffc, colcnt, 1, 1);
            //                               }
            //                           }
            //                           check = true;
            //                       }

            //                       #region value bind
            //                       //value bind
            //                       double paidcnt = 0;
            //                       double paidamt = 0;
            //                       double partcnt = 0;
            //                       double partamt = 0;
            //                       double notcnt = 0;
            //                       double notamt = 0;
            //                       double leftcnt = 0;
            //                       double ffcCnt = 0;
            //                       double sfcCnt = 0;
            //                       DataView dvpaidCnt = new DataView();
            //                       DataView dvdbset = new DataView();
            //                       if (dsload.Tables.Count > 0 && dsload.Tables[0].Rows.Count > 0)
            //                       {
            //                           if (dsload.Tables[0].Rows.Count > 0)
            //                           {
            //                               for (int sel = 0; sel < dsload.Tables[2].Rows.Count; sel++)
            //                               {
            //                                   double DemandAmt = 0;
            //                                   double paidAmt = 0;
            //                                   // string app_no = Convert.ToString(dsload.Tables[2].Rows[sel]["app_no"]);
            //                                   dsload.Tables[0].DefaultView.RowFilter = "App_no='" + Convert.ToString(dsload.Tables[2].Rows[sel]["App_no"]) + "' and Batch_year='" + Convert.ToString(dsload.Tables[2].Rows[sel]["Batch_year"]) + "' and Degree_Code='" + Convert.ToString(dsload.Tables[2].Rows[sel]["Degree_Code"]) + "' and FeeCategory='" + Convert.ToString(cblsem.Items[sem].Value)
            //+ "'";
            //                                   DataView dvdemand = dsload.Tables[0].DefaultView;
            //                                   if (dvdemand.Count > 0)
            //                                       double.TryParse(Convert.ToString(dvdemand[0]["Demand"]), out DemandAmt);

            //                                   dsload.Tables[1].DefaultView.RowFilter = "App_no='" + Convert.ToString(dsload.Tables[2].Rows[sel]["App_no"]) + "' and Batch_year='" + Convert.ToString(dsload.Tables[2].Rows[sel]["Batch_year"]) + "' and Degree_Code='" + Convert.ToString(dsload.Tables[2].Rows[sel]["Degree_Code"]) + "' and FeeCategory='" + Convert.ToString(cblsem.Items[sem].Value) + "'";
            //                                   dvpaidCnt = dsload.Tables[1].DefaultView;
            //                                   if (dvpaidCnt.Count > 0)
            //                                   {
            //                                       double.TryParse(Convert.ToString(dvpaidCnt[0]["Paid"]), out paidAmt);

            //                                       if (DemandAmt == paidAmt || DemandAmt < paidAmt)
            //                                       {
            //                                           paidamt += DemandAmt;
            //                                           paidcnt++;
            //                                       }
            //                                       else if (DemandAmt > paidAmt)
            //                                       {
            //                                           double balAmt = 0;
            //                                           balAmt = paidAmt;
            //                                           partamt += balAmt;
            //                                           partcnt++;
            //                                       }
            //                                       else
            //                                       {
            //                                           notamt += DemandAmt;
            //                                           notcnt++;
            //                                       }
            //                                   }
            //                                   else
            //                                   {
            //                                       if (checkdicon.Checked == true)
            //                                       {
            //                                           if (dsload.Tables[2].Rows.Count > 0)
            //                                           {
            //                                               dsload.Tables[2].DefaultView.RowFilter = " App_no='" + Convert.ToString(dsload.Tables[2].Rows[sel]["App_no"]) + "' and (cc='True' or DelFlag='1' or Exam_Flag like '%Debar%') ";
            //                                               DataView dv = dsload.Tables[2].DefaultView;
            //                                               if (dv.Count > 0)
            //                                                   leftcnt++;
            //                                               else
            //                                                   notcnt++;
            //                                           }
            //                                           else
            //                                           {
            //                                               notamt += DemandAmt;
            //                                               notcnt++;
            //                                           }
            //                                       }
            //                                       else
            //                                       {
            //                                           notamt += DemandAmt;
            //                                           notcnt++;
            //                                       }
            //                                   }
            //                                   //deduction
            //                                   dsload.Tables[3].DefaultView.RowFilter = "App_no='" + Convert.ToString(dsload.Tables[2].Rows[sel]["App_no"]) + "' and Batch_year='" + Convert.ToString(dsload.Tables[2].Rows[sel]["Batch_year"]) + "' and Degree_Code='" + Convert.ToString(dsload.Tables[2].Rows[sel]["Degree_Code"]) + "' and FeeCategory='" + Convert.ToString(cblsem.Items[sem].Value) + "'";
            //                                   DataView dvcon = new DataView();
            //                                   dvcon = dsload.Tables[3].DefaultView;
            //                                   if (dvcon.Count > 0)
            //                                   {
            //                                       for (int con = 0; con < dvcon.Count; con++)
            //                                       {
            //                                           ffcCnt = 0;
            //                                           string detres = Convert.ToString(dvcon[con]["detection"]);
            //                                           if (!htdetvalue.ContainsKey(detres))
            //                                           {
            //                                               ffcCnt++;
            //                                               htdetvalue.Add(detres, Convert.ToInt32(ffcCnt));
            //                                           }
            //                                           else
            //                                           {
            //                                               ffcCnt++;
            //                                               double total = 0;
            //                                               double.TryParse(Convert.ToString(htdetvalue[detres]), out total);
            //                                               total += ffcCnt;
            //                                               htdetvalue.Remove(Convert.ToString(detres));
            //                                               htdetvalue.Add(detres, Convert.ToInt32(total));
            //                                           }

            //                                           //if (detres.Trim().ToUpper() == "FFC")
            //                                           //  ffcCnt++;
            //                                           //else if (detres.Trim().ToUpper() == "SFC")
            //                                           //    sfcCnt++;
            //                                       }
            //                                   }

            //                               }
            //                           }

            //                           //fully paid
            //                           fpspread.Sheets[0].Cells[totad, valcol + 1].Text = Convert.ToString(paidcnt);
            //                           fpspread.Sheets[0].Cells[totad, valcol + 1].HorizontalAlign = HorizontalAlign.Right;
            //                           totfnlval += paidcnt;
            //                           //partial and not paid
            //                           double fnlcnt = partcnt + notcnt;
            //                           fpspread.Sheets[0].Cells[inst, valcol + 1].Text = Convert.ToString(fnlcnt);
            //                           fpspread.Sheets[0].Cells[inst, valcol + 1].HorizontalAlign = HorizontalAlign.Right;
            //                           totfnlval += fnlcnt;
            //                           //dynamic value bind

            //                           foreach (KeyValuePair<string, int> htval in htdet)
            //                           {
            //                               int colval = 0;
            //                               string deductR = "";
            //                               Int32.TryParse(Convert.ToString(htval.Value), out colval);
            //                               deductR = htval.Key.ToString();
            //                               //total count
            //                               double count = 0;
            //                               if (htdetvalue.ContainsKey(deductR))
            //                                   double.TryParse(Convert.ToString(htdetvalue[deductR]), out count);

            //                               fpspread.Sheets[0].Cells[colval, valcol + 1].Text = Convert.ToString(count);
            //                               fpspread.Sheets[0].Cells[colval, valcol + 1].HorizontalAlign = HorizontalAlign.Right;
            //                               totfnlval += count;
            //                               //fpspread.Sheets[0].Cells[sfc, valcol + 1].Text = Convert.ToString(sfcCnt);
            //                               //fpspread.Sheets[0].Cells[sfc, valcol + 1].HorizontalAlign = HorizontalAlign.Right;

            //                               //fpspread.Sheets[0].Cells[left, valcol + 1].Text = Convert.ToString(leftcnt);
            //                               //fpspread.Sheets[0].Cells[left, valcol + 1].HorizontalAlign = HorizontalAlign.Right;
            //                           }
            //                       }
            //                       #endregion

            //                       if (cnt == 1)
            //                           sfc = fpspread.Sheets[0].RowCount++;

            //                       fpspread.Sheets[0].Cells[sfc, colcnt].Text = "Total";
            //                       fpspread.Sheets[0].Cells[sfc, colcnt].Font.Bold = true;
            //                       fpspread.Sheets[0].Cells[sfc, colcnt].Font.Size = FontUnit.Medium;
            //                       fpspread.Sheets[0].Cells[sfc, colcnt].HorizontalAlign = HorizontalAlign.Right;
            //                       fpspread.Sheets[0].Rows[sfc].BackColor = Color.YellowGreen;
            //                       fpspread.Sheets[0].SpanModel.Add(sfc, colcnt, 1, 1);

            //                       fpspread.Sheets[0].Cells[sfc, valcol + 1].Text = Convert.ToString(totfnlval);
            //                       fpspread.Sheets[0].Cells[sfc, valcol + 1].HorizontalAlign = HorizontalAlign.Right;

            //                   }
            //               }
            //               #endregion

            //               //div1.Visible = true;
            //               grdTermFeeReport.Visible = true;
            //               Excel.Visible = true;
            //               Print.Visible = true;
            //               lblreptname.Visible = true;
            //               txtreptname.Visible = true;
            //               lblmsg.Visible = false;
            //               txtreptname.Text = "";
            //           }
            //           else
            //           {
            //               //div1.Visible = false;
            //               grdTermFeeReport.Visible = false;
            //               Excel.Visible = false;
            //               Print.Visible = false;
            //               lblreptname.Visible = false;
            //               txtreptname.Visible = false;
            //               txtreptname.Text = "";
            //               lblmsg.Visible = true;
            //               lblmsg.Text = "No Records Found";
            //           }
            //           fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
            lblmsg.Visible = true;
            lblmsg.Text = ex.ToString();
        }
    }

    // last modified 28.07.2017 sudhagar
    protected DataSet loadDsStudDetails()
    {
        string acdBatchYear = string.Empty;
        string feecaT = string.Empty;
        string feeCategory = string.Empty;
        string BatchYear = string.Empty;
        string batch_year = string.Empty;
        if (cbAcdYear.Checked == true)
        {

            #region academic year Added by abarna on 03/01/2018

            //string acdBatchYear = string.Empty;
            //string feecaT = string.Empty;
            //string feeCategory = string.Empty;
            //string BatchYear = string.Empty;
            //string batch_year = string.Empty;
            Dictionary<string, string> htAcademic = new Dictionary<string, string>();

            string settingType = string.Empty;
            if (rblTypeNew.SelectedIndex == 0)
                settingType = "0";
            else if (rblTypeNew.SelectedIndex == 1)
                settingType = "1";
            else if (rblTypeNew.SelectedIndex == 2)
                settingType = "2";
            string collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            string acdYears = Convert.ToString(ddlAcademic.SelectedItem.Text);
            string selQuery = " SELECT distinct ACD_COLLEGE_CODE,c.collname,ACD_YEAR,ACD_BATCH_YEAR,ACD_FEECATEGORY,t.textval FROM FT_ACADEMICYEAR AY,FT_ACADEMICYEAR_DETAILED AYD,collinfo c,textvaltable t WHERE c.college_code=ay.acd_college_code and AY.ACA_YEAR_PK=AYD.ACA_YEAR_FK and textcriteria='FEECA' and t.textcode=ayd.ACD_FEECATEGORY and t.college_code=ay.ACD_COLLEGE_CODE AND  ACD_COLLEGE_CODE IN('" + collegecode + "') and ACD_YEAR in('" + acdYears + "') and ACD_SETTING_TYPE='" + settingType + "' order by ACD_COLLEGE_CODE  asc,ACD_YEAR desc,ACD_FEECATEGORY asc";
            DataSet dsPrevAMount = da.select_method_wo_parameter(selQuery, "Text");
            if (dsPrevAMount.Tables.Count > 0 && dsPrevAMount.Tables[0].Rows.Count > 0)
            {
                DataTable dtAcdYear = dsPrevAMount.Tables[0].DefaultView.ToTable(true, "ACD_YEAR", "ACD_COLLEGE_CODE", "collname");
                DataTable dtBatchYear = dsPrevAMount.Tables[0].DefaultView.ToTable(true, "ACD_YEAR", "ACD_BATCH_YEAR", "ACD_COLLEGE_CODE");
                DataTable dtFeecat = dsPrevAMount.Tables[0].DefaultView.ToTable();

                if (dtAcdYear.Rows.Count > 0)
                {
                    int Sno = 0;
                    for (int row = 0; row < dtAcdYear.Rows.Count; row++)
                    {
                        Sno++;
                        string acdYear = Convert.ToString(dtAcdYear.Rows[row]["ACD_YEAR"]);
                        string clgCode = Convert.ToString(dtAcdYear.Rows[row]["ACD_COLLEGE_CODE"]);
                        dtBatchYear.DefaultView.RowFilter = "ACD_YEAR='" + acdYear + "' and ACD_COLLEGE_CODE='" + clgCode + "'";
                        DataTable dtBatch = dtBatchYear.DefaultView.ToTable();
                        if (dtBatch.Rows.Count > 0)
                        {
                            for (int bat = 0; bat < dtBatch.Rows.Count; bat++)
                            {
                                acdBatchYear = Convert.ToString(dtBatch.Rows[bat]["ACD_BATCH_YEAR"]);
                                dtFeecat.DefaultView.RowFilter = "ACD_YEAR='" + acdYear + "' and ACD_BATCH_YEAR='" + acdBatchYear + "' and ACD_COLLEGE_CODE='" + clgCode + "'";
                                DataTable dtFee = dtFeecat.DefaultView.ToTable();
                                if (dtFee.Rows.Count > 0)
                                {
                                    StringBuilder sbSem = new StringBuilder();
                                    StringBuilder sbSemStr = new StringBuilder();
                                    for (int fee = 0; fee < dtFee.Rows.Count; fee++)
                                    {
                                        feecaT = Convert.ToString(dtFee.Rows[fee]["ACD_FEECATEGORY"]);
                                        string feecaTStr = Convert.ToString(dtFee.Rows[fee]["textval"]);
                                        sbSem.Append(feecaT + ",");
                                        // sbSemStr.Append(feecaTStr + ",");
                                        BatchYear = Convert.ToString(dtFee.Rows[fee]["ACD_BATCH_YEAR"]);

                                        if (feecaT == "")
                                        {
                                            feeCategory = feecaT;
                                        }
                                        else
                                        {
                                            feeCategory += "'" + "," + "'" + feecaT;
                                        }
                                        if (BatchYear == "")
                                        {
                                            batch_year = BatchYear;
                                        }
                                        else
                                        {
                                            batch_year += "'" + "," + "'" + BatchYear;
                                        }
                                    }
                                    if (sbSem.Length > 0)
                                        sbSem.Remove(sbSem.Length - 1, 1);
                                    if (!htAcademic.ContainsKey(clgCode + "$" + acdBatchYear))
                                        htAcademic.Add(clgCode + "$" + acdBatchYear, Convert.ToString(sbSem));
                                    //if (sbSemStr.Length > 0)
                                    //    sbSemStr.Remove(sbSemStr.Length - 1, 1);    


                                }
                            }
                        }
                    }
                }
            }


            #endregion
        }
        DataSet dsload = new DataSet();
        try
        {

            #region value
            string batchYEar = Convert.ToString(ddlbatch.SelectedValue);
            string deptCode = Convert.ToString(ddldept.SelectedValue);
            string sem = Convert.ToString(getCblSelectedValue(cblsem));
            string commuN = Convert.ToString(getCblSelectedValue(cbl_community));
            string hdFK = Convert.ToString(getCblSelectedValue(chklsheader));
            string fnlYR = Convert.ToString(getCblSelectedValue(chklsfyear));

            #region includem

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
                            cc = " r.cc=1";
                        if (cblinclude.Items[i].Value == "2")
                            debar = " r.Exam_Flag like '%debar'";
                        if (cblinclude.Items[i].Value == "3")
                            disc = "  r.DelFlag=1";
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
            #endregion

            #region Query
            string selQ = string.Empty;
            //registration total student count tbl 0
            if (cbAcdYear.Checked == true)
            {
                selQ += " select distinct Roll_No,roll_admit,Reg_No,r.app_no,r.Stud_Name,r.Current_Semester,CC,DelFlag,Exam_Flag from Registration r,applyn a where r.app_no=a.app_no and  r.Batch_Year in('" + batch_year + "') and r.degree_code='" + deptCode + "' " + commondist + "  and r.college_code='" + ddlcollege.SelectedItem.Value + "' ";
                if (!string.IsNullOrEmpty(commuN))
                    selQ += " and a.community in('" + commuN + "')";
                selQ += " order by Roll_No";

                //findaily transaction with each student receipt no count take based on feecategory bl 1
                selQ += " select distinct d.app_no,d.FeeCategory,d.DDNo as challan_no,d.TransCode from Registration r,FT_FinDailyTransaction d where d.App_No =r.App_No and isnull(IsCanceled,'0')='0'  and  ISNULL(IsCollected,0)='1' and r.Batch_Year in('" + batch_year + "') " + commondist + " and d.finyearfk in('" + fnlYR + "') and r.degree_code='" + deptCode + "'  and headerfk in('" + hdFK + "') and feecategory in('" + feeCategory + "') and r.college_code='" + ddlcollege.SelectedItem.Value + "' ";

                //findaily transaction with each student paid details tbl 2
                selQ += " select distinct Roll_No,roll_admit,Reg_No,d.app_no,Stud_Name,d.FeeCategory,Current_Semester,d.DDNo as challan_no,COnvert(nvarchar(15),d.TransDate,101) as challandate,COnvert(nvarchar(15),d.dddate,101) as rcptdate ,d.PayMode,d.TransCode,CC,DelFlag,Exam_Flag,batch_year from Registration r,FT_FinDailyTransaction d where d.App_No =r.App_No and isnull(IsCanceled,'0')='0'  and  ISNULL(IsCollected,0)='1' and r.Batch_Year in('" + batch_year + "') " + commondist + " and d.finyearfk in('" + fnlYR + "') and r.degree_code='" + deptCode + "'  and headerfk in('" + hdFK + "') and feecategory in('" + feeCategory + "')  and r.college_code='" + ddlcollege.SelectedItem.Value + "'";

                //dailytransaciton headerwise wise tbl 3
                selQ += " select distinct d.app_no,r.Roll_No,r.roll_admit,d.FeeCategory as fee_category,d.DDNo as challan_no,d.HeaderFK as  header_id,CONVERT(nvarchar(15),d.TransDate,101) as cal_date,sum(d.Debit) paid,d.PayMode,d.TransCode,batch_year from registration r,FT_FinDailyTransaction d,FM_LedgerMaster f where f.LedgerPK=d.LedgerFK and f.HeaderFK=d.HeaderFK  and r.App_No =d.App_No and r.Batch_Year in('" + batch_year + "') and r.degree_code='" + deptCode + "'  and d.headerfk in('" + hdFK + "') and feecategory in('" + feeCategory + "') " + commondist + " and d.finyearfk in('" + fnlYR + "') and d.Credit=0 and TransType=1  and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and r.college_code='" + ddlcollege.SelectedItem.Value + "' group by r.Roll_No,d.FeeCategory,d.DDNo,d.HeaderFK,d.TransDate,d.PayMode,d.TransCode,r.roll_admit,d.app_no,batch_year ";
                //and d.Debit>0


                //deduction tbl 4
                selQ += " select distinct ISNULL(t.TextVal,'') as detection,r.Roll_No,a.app_no  from FT_FeeAllot a,TextValTable t,Registration r where r.App_No=a.App_No and convert(nvarchar(50),t.TextCode)=convert(nvarchar(50),a.DeductReason) and r.Batch_Year in('" + batch_year + "') and a.finyearfk in('" + fnlYR + "') and r.degree_code='" + deptCode + "' " + commondist + " and ISNULL(DeductReason,'') <> '' and r.college_code='" + ddlcollege.SelectedItem.Value + "' ";

                //community applyn tbl 5
                selQ += " select distinct community,textval,app_no from applyn a,textvaltable t where community=t.textcode and  textcriteria='comm' and a.college_code=t.college_code  and a.college_code='" + ddlcollege.SelectedItem.Value + "' and  degree_code='" + deptCode + "' and Batch_Year in('" + batch_year + "')";
                //selQ += " select community,app_no from applyn where degree_code='" + deptCode + "' and batch_year='" + batchYEar + "'";
                //tabl 6
                //  selQ += " select textval,textcode from textvaltable where textcriteria='comm' and college_code='" + ddlcollege.SelectedItem.Value + "'";

            }
            else
            {
                selQ += " select distinct Roll_No,roll_admit,Reg_No,r.app_no,r.Stud_Name,r.Current_Semester,CC,DelFlag,Exam_Flag from Registration r,applyn a where r.app_no=a.app_no and  r.Batch_Year='" + batchYEar + "' and r.degree_code='" + deptCode + "' " + commondist + "  and r.college_code='" + ddlcollege.SelectedItem.Value + "' ";
                if (!string.IsNullOrEmpty(commuN))
                    selQ += " and a.community in('" + commuN + "')";
                selQ += " order by Roll_No";

                //findaily transaction with each student receipt no count take based on feecategory bl 1
                selQ += " select distinct d.app_no,d.FeeCategory,d.DDNo as challan_no,d.TransCode from Registration r,FT_FinDailyTransaction d where d.App_No =r.App_No and isnull(IsCanceled,'0')='0'  and  ISNULL(IsCollected,0)='1' and r.Batch_Year='" + batchYEar + "' " + commondist + " and d.finyearfk in('" + fnlYR + "') and r.degree_code='" + deptCode + "'  and headerfk in('" + hdFK + "') and feecategory in('" + sem + "') and r.college_code='" + ddlcollege.SelectedItem.Value + "' ";

                //findaily transaction with each student paid details tbl 2
                selQ += " select distinct Roll_No,roll_admit,Reg_No,d.app_no,Stud_Name,d.FeeCategory,Current_Semester,d.DDNo as challan_no,COnvert(nvarchar(15),d.TransDate,101) as challandate,COnvert(nvarchar(15),d.dddate,101) as rcptdate ,d.PayMode,d.TransCode,CC,DelFlag,Exam_Flag from Registration r,FT_FinDailyTransaction d where d.App_No =r.App_No and isnull(IsCanceled,'0')='0'  and  ISNULL(IsCollected,0)='1' and r.Batch_Year='" + batchYEar + "' " + commondist + " and d.finyearfk in('" + fnlYR + "') and r.degree_code='" + deptCode + "'  and headerfk in('" + hdFK + "') and feecategory in('" + sem + "')  and r.college_code='" + ddlcollege.SelectedItem.Value + "'";

                //dailytransaciton headerwise wise tbl 3
                selQ += " select distinct d.app_no,r.Roll_No,r.roll_admit,d.FeeCategory as fee_category,d.DDNo as challan_no,d.HeaderFK as  header_id,CONVERT(nvarchar(15),d.TransDate,101) as cal_date,sum(d.Debit) paid,d.PayMode,d.TransCode from registration r,FT_FinDailyTransaction d,FM_LedgerMaster f where f.LedgerPK=d.LedgerFK and f.HeaderFK=d.HeaderFK  and r.App_No =d.App_No and r.Batch_Year='" + batchYEar + "' and r.degree_code='" + deptCode + "'  and d.headerfk in('" + hdFK + "') and feecategory in('" + sem + "') " + commondist + " and d.finyearfk in('" + fnlYR + "') and d.Credit=0 and TransType=1  and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and r.college_code='" + ddlcollege.SelectedItem.Value + "' group by r.Roll_No,d.FeeCategory,d.DDNo,d.HeaderFK,d.TransDate,d.PayMode,d.TransCode,r.roll_admit,d.app_no ";
                //and d.Debit>0


                //deduction tbl 4
                selQ += " select distinct ISNULL(t.TextVal,'') as detection,r.Roll_No,a.app_no  from FT_FeeAllot a,TextValTable t,Registration r where r.App_No=a.App_No and convert(nvarchar(50),t.TextCode)=convert(nvarchar(50),a.DeductReason) and r.Batch_Year='" + batchYEar + "' and a.finyearfk in('" + fnlYR + "') and r.degree_code='" + deptCode + "' " + commondist + " and ISNULL(DeductReason,'') <> '' and r.college_code='" + ddlcollege.SelectedItem.Value + "' ";

                //community applyn tbl 5
                selQ += " select distinct community,textval,app_no from applyn a,textvaltable t where community=t.textcode and  textcriteria='comm' and a.college_code=t.college_code  and a.college_code='" + ddlcollege.SelectedItem.Value + "' and  degree_code='" + deptCode + "' and batch_year='" + batchYEar + "'";
            }
            dsload = da.select_method_wo_parameter(selQ, "Text");
            #endregion
        }
        catch { }
        return dsload;
    }

    protected DataSet getDeductionDetails(string sem)
    {
        DataSet dsload = new DataSet();
        string SelectQ = string.Empty;
        try
        {
            if (cbAcdYear.Checked == false)
            {
                string de = string.Empty;
                string batchYEar = Convert.ToString(ddlbatch.SelectedValue);
                string deptCode = Convert.ToString(ddldept.SelectedValue);
                //string sem = Convert.ToString(getCblSelectedValue(cblsem));
                string commuN = Convert.ToString(getCblSelectedValue(cbl_community));
                string hdFK = Convert.ToString(getCblSelectedValue(chklsheader));
                string fnlYR = Convert.ToString(getCblSelectedValue(chklsfyear));
                string deducT = Convert.ToString(getCblSelectedValue(cbldeduct));
                if (deducT != "")
                {
                    de = "t.textcode in('" + deducT + "') and";
                }




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
                                cc = " r.cc=1";
                            if (cblinclude.Items[i].Value == "2")
                                debar = " r.Exam_Flag like '%debar'";
                            if (cblinclude.Items[i].Value == "3")
                                disc = "  r.DelFlag=1";
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




                SelectQ = " select SUM(TotalAmount) as Demand, f.App_No,r.degree_code,r.Batch_Year,f.FeeCategory from FT_FeeAllot f,Registration r where f.App_No =r.App_No " + commondist + " and r.Batch_Year in('" + batchYEar + "') and r.degree_code in('" + deptCode + "') and f.FeeCategory in(" + sem + ") and f.HeaderFK in('" + hdFK + "') and f.FinYearFK in('" + fnlYR + "') group by f.App_No,r.degree_code,r.Batch_Year,f.FeeCategory";
                //FeeCategory in(" + feecaterory + ")
                //paid
                SelectQ += " select SUM(Debit) as Paid, f.App_No,f.FeeCategory,r.degree_code,r.Batch_Year from FT_FinDailyTransaction f,Registration r where f.App_No =r.App_No " + commondist + " and isnull(IsCanceled,'0')='0'  and  ISNULL(IsCollected,0)='1' and r.Batch_Year in('" + batchYEar + "') and r.degree_code in('" + deptCode + "') and f.FeeCategory in(" + sem + ") and f.HeaderFK in('" + hdFK + "') and f.FinYearFK in('" + fnlYR + "') group by f.App_No,f.FeeCategory,r.degree_code,r.Batch_Year order by f.App_No,f.FeeCategory asc";
                SelectQ += " select r.App_No,DelFlag,r.Current_Semester,CC,Exam_Flag,r.degree_code,r.Batch_Year from Registration r,applyn a where r.app_no=a.app_no and r.Batch_Year='" + batchYEar + "' and r.degree_code='" + deptCode + "' " + commondist + " ";
                if (!string.IsNullOrEmpty(commuN))
                    SelectQ += " and a.community in('" + commuN + "')";
                //" + commvalue + "";
                SelectQ += " select distinct ISNULL(t.TextVal,'') as detection,a.App_No,r.degree_code,r.Batch_Year,a.FeeCategory  from FT_FeeAllot a,TextValTable t,Registration r where r.App_No=a.App_No and convert(nvarchar(50),t.TextCode)=convert(nvarchar(50),a.DeductReason) and r.Batch_Year='" + batchYEar + "' and a.finyearfk in('" + fnlYR + "') and r.degree_code='" + deptCode + "'  " + commondist + " and " + de + "  ISNULL(DeductReason,'') <> '' and isnull(deductamout,0)<>'0' ";
            }
            else
            {

                string batchYEar = Convert.ToString(ddlbatch.SelectedValue);
                string deptCode = Convert.ToString(ddldept.SelectedValue);
                //string sem = Convert.ToString(getCblSelectedValue(cblsem));
                string commuN = Convert.ToString(getCblSelectedValue(cbl_community));
                string hdFK = Convert.ToString(getCblSelectedValue(chklsheader));
                string fnlYR = Convert.ToString(getCblSelectedValue(chklsfyear));
                string deducT = Convert.ToString(getCblSelectedValue(cbldeduct));


                string acdBatchYear = string.Empty;
                string feecaT = string.Empty;
                string feeCategory = string.Empty;
                string BatchYear = string.Empty;
                string batch_year = string.Empty;
                DataTable dtFee = new DataTable();
                StringBuilder sbSem = new StringBuilder();
                StringBuilder sbSemStr = new StringBuilder();
                Dictionary<string, string> htAcademic = new Dictionary<string, string>();

                string settingType = string.Empty;
                if (rblTypeNew.SelectedIndex == 0)
                    settingType = "0";
                else if (rblTypeNew.SelectedIndex == 1)
                    settingType = "1";
                else if (rblTypeNew.SelectedIndex == 2)
                    settingType = "2";
                string collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
                string acdYears = Convert.ToString(ddlAcademic.SelectedItem.Text);
                string selQuery = " SELECT distinct ACD_COLLEGE_CODE,c.collname,ACD_YEAR,ACD_BATCH_YEAR,ACD_FEECATEGORY,t.textval FROM FT_ACADEMICYEAR AY,FT_ACADEMICYEAR_DETAILED AYD,collinfo c,textvaltable t WHERE c.college_code=ay.acd_college_code and AY.ACA_YEAR_PK=AYD.ACA_YEAR_FK and textcriteria='FEECA' and t.textcode=ayd.ACD_FEECATEGORY and t.college_code=ay.ACD_COLLEGE_CODE AND  ACD_COLLEGE_CODE IN('" + collegecode + "') and ACD_YEAR in('" + acdYears + "') and ACD_SETTING_TYPE='" + settingType + "' order by ACD_COLLEGE_CODE  asc,ACD_YEAR desc,ACD_FEECATEGORY asc";
                DataSet dsPrevAMount = da.select_method_wo_parameter(selQuery, "Text");
                if (dsPrevAMount.Tables.Count > 0 && dsPrevAMount.Tables[0].Rows.Count > 0)
                {
                    DataTable dtAcdYear = dsPrevAMount.Tables[0].DefaultView.ToTable(true, "ACD_YEAR", "ACD_COLLEGE_CODE", "collname");
                    DataTable dtBatchYear = dsPrevAMount.Tables[0].DefaultView.ToTable(true, "ACD_YEAR", "ACD_BATCH_YEAR", "ACD_COLLEGE_CODE");
                    DataTable dtFeecat = dsPrevAMount.Tables[0].DefaultView.ToTable();

                    if (dtAcdYear.Rows.Count > 0)
                    {
                        int Sno = 0;
                        for (int row = 0; row < dtAcdYear.Rows.Count; row++)
                        {
                            Sno++;
                            string acdYear = Convert.ToString(dtAcdYear.Rows[row]["ACD_YEAR"]);
                            string clgCode = Convert.ToString(dtAcdYear.Rows[row]["ACD_COLLEGE_CODE"]);
                            dtBatchYear.DefaultView.RowFilter = "ACD_YEAR='" + acdYear + "' and ACD_COLLEGE_CODE='" + clgCode + "'";
                            DataTable dtBatch = dtBatchYear.DefaultView.ToTable();
                            if (dtBatch.Rows.Count > 0)
                            {
                                for (int bat = 0; bat < dtBatch.Rows.Count; bat++)
                                {
                                    acdBatchYear = Convert.ToString(dtBatch.Rows[bat]["ACD_BATCH_YEAR"]);
                                    dtFeecat.DefaultView.RowFilter = "ACD_YEAR='" + acdYear + "' and ACD_BATCH_YEAR='" + acdBatchYear + "' and ACD_COLLEGE_CODE='" + clgCode + "'";
                                    dtFee = dtFeecat.DefaultView.ToTable();
                                    if (dtFee.Rows.Count > 0)
                                    {
                                        //StringBuilder sbSem = new StringBuilder();
                                        //StringBuilder sbSemStr = new StringBuilder();
                                        for (int fee = 0; fee < dtFee.Rows.Count; fee++)
                                        {
                                            feecaT = Convert.ToString(dtFee.Rows[fee]["ACD_FEECATEGORY"]);
                                            string feecaTStr = Convert.ToString(dtFee.Rows[fee]["textval"]);
                                            sbSem.Append(feecaT + ",");
                                            // sbSemStr.Append(feecaTStr + ",");
                                            BatchYear = Convert.ToString(dtFee.Rows[fee]["ACD_BATCH_YEAR"]);

                                            if (feecaT == "")
                                            {
                                                feeCategory = feecaT;
                                            }
                                            else
                                            {
                                                feeCategory += "'" + "," + "'" + feecaT;
                                            }
                                            if (BatchYear == "")
                                            {
                                                batch_year = BatchYear;
                                            }
                                            else
                                            {
                                                batch_year += "'" + "," + "'" + BatchYear;
                                            }
                                        }

                                        if (sbSem.Length > 0)
                                            sbSem.Remove(sbSem.Length - 1, 1);
                                        if (!htAcademic.ContainsKey(clgCode + "$" + acdBatchYear))
                                            htAcademic.Add(clgCode + "$" + acdBatchYear, Convert.ToString(sbSem));
                                        //if (sbSemStr.Length > 0)
                                        //    sbSemStr.Remove(sbSemStr.Length - 1, 1);    


                                    }
                                }
                            }
                        }
                    }
                }




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
                                cc = " r.cc=1";
                            if (cblinclude.Items[i].Value == "2")
                                debar = " r.Exam_Flag like '%debar'";
                            if (cblinclude.Items[i].Value == "3")
                                disc = "  r.DelFlag=1";
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





                SelectQ = " select SUM(TotalAmount) as Demand, f.App_No,r.degree_code,r.Batch_Year,f.FeeCategory from FT_FeeAllot f,Registration r where f.App_No =r.App_No " + commondist + " and r.Batch_Year in('" + batch_year + "') and r.degree_code in('" + deptCode + "') and f.FeeCategory in('" + feeCategory + "') and f.HeaderFK in('" + hdFK + "') and f.FinYearFK in('" + fnlYR + "') group by f.App_No,r.degree_code,r.Batch_Year,f.FeeCategory";
                //FeeCategory in(" + feecaterory + ")
                //paid
                SelectQ += " select SUM(Debit) as Paid, f.App_No,f.FeeCategory,r.degree_code,r.Batch_Year from FT_FinDailyTransaction f,Registration r where f.App_No =r.App_No " + commondist + " and isnull(IsCanceled,'0')='0'  and  ISNULL(IsCollected,0)='1' and r.Batch_Year in('" + batch_year + "') and r.degree_code in('" + deptCode + "') and f.FeeCategory in('" + feeCategory + "') and f.HeaderFK in('" + hdFK + "') and f.FinYearFK in('" + fnlYR + "') group by f.App_No,f.FeeCategory,r.degree_code,r.Batch_Year order by f.App_No,f.FeeCategory asc";
                SelectQ += " select r.App_No,DelFlag,r.Current_Semester,CC,Exam_Flag,r.degree_code,r.Batch_Year from Registration r,applyn a where r.app_no=a.app_no and r.Batch_Year in('" + batch_year + "') and r.degree_code='" + deptCode + "' " + commondist + " ";
                if (!string.IsNullOrEmpty(commuN))
                    SelectQ += " and a.community in('" + commuN + "')";
                //" + commvalue + "";
                SelectQ += " select distinct ISNULL(t.TextVal,'') as detection,a.App_No,r.degree_code,r.Batch_Year,a.FeeCategory  from FT_FeeAllot a,TextValTable t,Registration r where r.App_No=a.App_No and convert(nvarchar(50),t.TextCode)=convert(nvarchar(50),a.DeductReason) and r.Batch_Year in('" + batch_year + "') and a.finyearfk in('" + fnlYR + "') and r.degree_code='" + deptCode + "'  " + commondist + " and t.textcode in('" + deducT + "') and ISNULL(DeductReason,'') <> '' and isnull(deductamout,0)<>'0' ";
            }

            dsload = da.select_method_wo_parameter(SelectQ, "Text");

        }
        catch { }
        return dsload;
    }

    protected void btngo_OnClick(object sender, EventArgs e)
    {
        ds.Clear();
        ds = loadDsStudDetails();
        //if (cbAcdYear.Checked && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)//academic year selected only this function execute
        //{
        //    Dictionary<string, string> getAcdYear = new Dictionary<string, string>();
        //    #region Academic Year
        //    DataSet dsNornaml = ds.Copy();
        //    try
        //    {
        //        string clgCode = Convert.ToString(ddlcollege.SelectedItem.Text);
        //        string acdYear = Convert.ToString(ddlAcademic.SelectedItem.Text);
        //        getAcdYear = getOldSettings(acdYear);//(acdYear, clgCode);

        //        if (getAcdYear.Count > 0)
        //        {
        //            bool boolDs = false;
        //            // DataTable dtFirst = ds.Tables[0].DefaultView.ToTable();
        //            foreach (KeyValuePair<string, string> getVal in getAcdYear)
        //            {
        //                string feeCate = getVal.Value.Replace(",", "','");
        //                ds.Tables[2].DefaultView.RowFilter = "batch_year='" + getVal.Key.Split('$')[1] + "' and feecategory in('" + feeCate + "')";
        //                ds.Tables[3].DefaultView.RowFilter = "batch_year='" + getVal.Key.Split('$')[1] + "' and fee_category in('" + feeCate + "')";
        //                DataTable dtPaid = ds.Tables[2].DefaultView.ToTable();
        //                DataTable dtPaid1 = ds.Tables[3].DefaultView.ToTable();
        //                //ds.Tables[1].DefaultView.RowFilter = "college_Code='" + getVal.Key.Split('$')[0] + "' and batch_year='" + getVal.Key.Split('$')[1] + "' and feecategory in('" + feeCate + "')";
        //                //DataTable dtExcess = ds.Tables[1].DefaultView.ToTable();
        //                if (!boolDs)
        //                {
        //                    dsFinal.Reset();
        //                    // dsFinal.Tables.Add(dtFirst);                           
        //                    dsFinal.Tables.Add(dtPaid);
        //                    dsFinal1.Tables.Add(dtPaid1);
        //                    //   dsFinal.Tables.Add(dtExcess);
        //                    boolDs = true;
        //                }
        //                else
        //                {
        //                    //dsFinal.Merge(dtPaid);
        //                    dsFinal.Merge(dtPaid);
        //                    dsFinal1.Merge(dtPaid1);
        //                    //dsFinal.Merge(dtExcess);
        //                }
        //            }
        //        }
        //    }

        //    catch
        //    {
        //        ds.Reset();
        //        ds = dsNornaml.Copy();
        //    }
        //    #endregion
        //}
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            getStudDetails(ds);
        }
        else
        {
            grdTermFeeReport.Visible = false;
            Excel.Visible = false;
            Print.Visible = false;
            lblreptname.Visible = false;
            txtreptname.Visible = false;
            txtreptname.Text = "";
            lblmsg.Visible = true;
            lblmsg.Text = "No Records Found";
        }
    }

    protected void getStudDetails(DataSet ds)
    {
        try
        {
            dicRowColor.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                bool boolSem = false;
                if (cbAcdYear.Checked == false)
                {
                    #region design
                    loadcolumns();
                    challanAndReceiptNoRights();
                    RollAndRegSettings();
                    Printcontrol.Visible = false;

                    arrColHdrNames.Add("S.No");
                    arrColHdrNames2.Add("S.No");
                    dtTermFeeReport.Columns.Add("col0");
                    arrColHdrNames.Add("Roll No");
                    arrColHdrNames2.Add("Roll No");
                    dtTermFeeReport.Columns.Add("col1");
                    arrColHdrNames.Add("Reg No");
                    arrColHdrNames2.Add("Reg No");
                    dtTermFeeReport.Columns.Add("col2");
                    arrColHdrNames.Add("Admission No");
                    arrColHdrNames2.Add("Admission No");
                    dtTermFeeReport.Columns.Add("col3");
                    arrColHdrNames.Add("Name of the Student");
                    arrColHdrNames2.Add("Name of the Student");
                    dtTermFeeReport.Columns.Add("col4");
                    arrColHdrNames.Add("FFC/DFC /SFC/VHC /ADV/INST");
                    arrColHdrNames2.Add("FFC/DFC /SFC/VHC /ADV/INST");
                    dtTermFeeReport.Columns.Add("col5");
                    arrColHdrNames.Add("Community");
                    arrColHdrNames2.Add("Community");
                    dtTermFeeReport.Columns.Add("col6");
                    spreadColumnVisible();
                    ArrayList arCol = new ArrayList();
                    arCol.Add("Receipt No");
                    arCol.Add("Challan No");
                    arCol.Add("Receipt Date");
                    Hashtable htCol = new Hashtable();
                    Hashtable htsem = new Hashtable();
                    int colHdrIndx = 7;
                    for (int i = 0; i < cblsem.Items.Count; i++)
                    {
                        int semColCnt = 0;
                        int incrcolCnt = 0;
                        int tempColCnt = 0;
                        string feecaT = Convert.ToString(cblsem.Items[i].Value);
                        string feecaStr = Convert.ToString(cblsem.Items[i].Text);
                        if (cblsem.Items[i].Selected == true)
                        {
                            boolSem = true;
                            bool boolArCol = true;
                            for (int h = 0; h < chklsheader.Items.Count; h++)
                            {
                                if (chklsheader.Items[h].Selected == true)
                                {
                                    tempColCnt = 0;
                                    if (boolArCol)//receipt,challan no 
                                    {
                                        foreach (string colName in arCol)
                                        {
                                            arrColHdrNames.Add(feecaStr);
                                            arrColHdrNames2.Add(colName);
                                            dtTermFeeReport.Columns.Add("col" + colHdrIndx);
                                            htCol.Add(Convert.ToString(feecaT + "-" + colName), dtTermFeeReport.Columns.Count - 1);
                                            if (boolSem == true)
                                            {
                                                htsem.Add(feecaT, dtTermFeeReport.Columns.Count - 1);//for details shows in bottom of grid
                                                boolSem = false;
                                            }
                                            incrcolCnt++;
                                            colHdrIndx++;
                                        }
                                        boolArCol = false;
                                    }
                                    arrColHdrNames.Add(feecaStr);
                                    arrColHdrNames2.Add(chklsheader.Items[h].Text);
                                    dtTermFeeReport.Columns.Add("col" + colHdrIndx);
                                    string hdFK = Convert.ToString(chklsheader.Items[h].Value);
                                    string tempStr = feecaT + "-" + hdFK;
                                    htCol.Add(Convert.ToString(feecaT + "-" + hdFK), dtTermFeeReport.Columns.Count - 1);
                                    incrcolCnt++;
                                    colHdrIndx++;
                                }
                            }
                        }
                        if (incrcolCnt > 0)
                        {
                            arrColHdrNames.Add(feecaStr);
                            arrColHdrNames2.Add("Total");
                            dtTermFeeReport.Columns.Add("col" + colHdrIndx);
                            htCol.Add(Convert.ToString(feecaT + "-" + "Total"), dtTermFeeReport.Columns.Count - 1);
                            incrcolCnt++;
                            colHdrIndx++;
                        }
                    }
                    DataRow drHdr1 = dtTermFeeReport.NewRow();
                    DataRow drHdr2 = dtTermFeeReport.NewRow();
                    for (int grCol = 0; grCol < dtTermFeeReport.Columns.Count; grCol++)
                    {
                        drHdr1[grCol] = arrColHdrNames[grCol];
                        drHdr2[grCol] = arrColHdrNames2[grCol];
                    }
                    dtTermFeeReport.Rows.Add(drHdr1);
                    dtTermFeeReport.Rows.Add(drHdr2);
                    #endregion

                    #region Value

                    Hashtable htTotal = new Hashtable();
                    Dictionary<string, Double> dicdedction = new Dictionary<string, double>();
                    bool boolCheck = false;
                    string feeCat = string.Empty;
                    int SnoCount = 0;
                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        SnoCount++;
                        string appNo = Convert.ToString(ds.Tables[0].Rows[row]["app_no"]);
                        Dictionary<string, int> dtYRowCnt = new Dictionary<string, int>();
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            for (int i = 0; i < cblsem.Items.Count; i++)//get total row count based each stud receipt no
                            {
                                if (cblsem.Items[i].Selected == true)
                                {
                                    int tempRowCnt = 0;
                                    feeCat = Convert.ToString(cblsem.Items[i].Value);
                                    ds.Tables[1].DefaultView.RowFilter = " app_no='" + appNo + "' and feecategory='" + feeCat + "'";
                                    DataTable dtRowCnt = ds.Tables[1].DefaultView.ToTable();
                                    if (dtRowCnt.Rows.Count > 0)
                                        int.TryParse(Convert.ToString(dtRowCnt.Compute("count(TransCode)", "")), out  tempRowCnt);
                                    dtYRowCnt.Add(feeCat, tempRowCnt);
                                }
                            }
                        }
                        int studRowCnt = 0;
                        if (dtYRowCnt.Count > 0)
                        {
                            // dtYRowCnt = dtYRowCnt.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                            int.TryParse(Convert.ToString(dtYRowCnt.Values.Max()), out  studRowCnt);
                            //studRowCnt = dtYRowCnt.Values.Max();
                        }
                        //else
                        //    studRowCnt = 1;
                        if (studRowCnt > 0)//with paid
                        {
                            #region with paid student count

                            for (int colInc = 1; colInc <= studRowCnt; colInc++)//row creating based on variable count
                            {
                                drowInst = dtTermFeeReport.NewRow();
                                dtTermFeeReport.Rows.Add(drowInst);
                            }
                            for (int i = 0; i < cblsem.Items.Count; i++)
                            {
                                int ColCnt = 0;
                                int rowCnt = 0;
                                int rowCnts = studRowCnt;
                                feeCat = Convert.ToString(cblsem.Items[i].Value);
                                if (cblsem.Items[i].Selected == true)//each and every semester paid details bind here
                                {
                                    ds.Tables[2].DefaultView.RowFilter = " app_no='" + appNo + "' and feecategory='" + feeCat + "'";
                                    DataTable dtSemPaid = ds.Tables[2].DefaultView.ToTable();
                                    if (dtSemPaid.Rows.Count > 0)
                                    {
                                        for (int semR = 0; semR < dtSemPaid.Rows.Count; semR++)
                                        {
                                            // --rowCnt;//decreasing row count top to bottom                                               
                                            double semTotAmount = 0;
                                            rowCnt = dtTermFeeReport.Rows.Count - rowCnts;
                                            string rollno = Convert.ToString(ds.Tables[0].Rows[row]["Roll_No"]);
                                            string regno = Convert.ToString(ds.Tables[0].Rows[row]["Reg_No"]);
                                            string rolladmit = Convert.ToString(ds.Tables[0].Rows[row]["roll_admit"]);
                                            string appno = Convert.ToString(ds.Tables[0].Rows[row]["app_no"]);
                                            string name = Convert.ToString(ds.Tables[0].Rows[row]["Stud_Name"]);
                                            string currentsem = Convert.ToString(ds.Tables[0].Rows[row]["Current_Semester"]);
                                            string delflag = Convert.ToString(ds.Tables[0].Rows[row]["delflag"]);
                                            string dedcureason = "";
                                            if (ds.Tables[4].Rows.Count > 0)
                                            {
                                                ds.Tables[4].DefaultView.RowFilter = "app_no='" + appNo + "'";
                                                DataTable dvdeductreason = ds.Tables[4].DefaultView.ToTable();
                                                if (dvdeductreason.Rows.Count > 0)
                                                {
                                                    for (int k = 0; k < dvdeductreason.Rows.Count; k++)
                                                    {
                                                        if (dedcureason == "")
                                                            dedcureason = Convert.ToString(dvdeductreason.Rows[k]["detection"]);
                                                        else
                                                            dedcureason += "," + Convert.ToString(dvdeductreason.Rows[k]["detection"]);
                                                    }
                                                }
                                            }
                                            string community = "";
                                            if (ds.Tables[5].Rows.Count > 0)
                                            {
                                                ds.Tables[5].DefaultView.RowFilter = "app_no='" + appno + "'";
                                                DataTable dtCommut = ds.Tables[5].DefaultView.ToTable(); ;
                                                if (dtCommut.Rows.Count > 0)
                                                    community = Convert.ToString(dtCommut.Rows[0]["textval"]);
                                            }
                                            dtTermFeeReport.Rows[rowCnt][0] = Convert.ToString(SnoCount);
                                            dtTermFeeReport.Rows[rowCnt][1] = rollno;
                                            dtTermFeeReport.Rows[rowCnt][2] = regno;
                                            dtTermFeeReport.Rows[rowCnt][3] = rolladmit;
                                            if (delflag == "1") //added by abarna
                                            {
                                                string date = da.GetFunction("select CONVERT(varchar(10), Discontinue_Date ,103) as TransDate from Discontinue  where app_no='" + appno + "'");
                                                if (date != "0")
                                                {
                                                    dtTermFeeReport.Rows[rowCnt][4] = name + "(" + (Convert.ToString(date)) + ")" + "(DisContinue)";
                                                    if (!dicCellColor.ContainsKey(rowCnt + "$" + 4))
                                                        dicCellColor.Add(rowCnt + "$" + 4, "DisContinue");
                                                }
                                                else
                                                {
                                                    dtTermFeeReport.Rows[rowCnt][4] = name + "(DisContinue)";
                                                    if (!dicCellColor.ContainsKey(rowCnt + "$" + 4))
                                                        dicCellColor.Add(rowCnt + "$" + 4, "DisContinue");
                                                }
                                            }
                                            else
                                            {
                                                dtTermFeeReport.Rows[rowCnt][4] = name;
                                            }
                                            dtTermFeeReport.Rows[rowCnt][5] = dedcureason;
                                            dtTermFeeReport.Rows[rowCnt][6] = community;

                                            string reciptNo = Convert.ToString(dtSemPaid.Rows[semR]["TransCode"]);
                                            string challanNo = Convert.ToString(dtSemPaid.Rows[semR]["challan_no"]);
                                            string chaldate = Convert.ToString(dtSemPaid.Rows[semR]["challandate"]);
                                            string transdate = Convert.ToString(dtSemPaid.Rows[semR]["rcptdate"]);
                                            DateTime dtChallan = Convert.ToDateTime(chaldate);
                                            // DateTime dtReceipt = Convert.ToDateTime(transdate);

                                            string hashValue = feeCat + "-" + "Receipt No";
                                            int.TryParse(Convert.ToString(htCol[hashValue]), out ColCnt);
                                            dtTermFeeReport.Rows[rowCnt][ColCnt] = reciptNo;

                                            hashValue = feeCat + "-" + "Challan No";
                                            int.TryParse(Convert.ToString(htCol[hashValue]), out ColCnt);
                                            dtTermFeeReport.Rows[rowCnt][ColCnt] = challanNo;

                                            hashValue = feeCat + "-" + "Receipt Date";
                                            int.TryParse(Convert.ToString(htCol[hashValue]), out ColCnt);
                                            dtTermFeeReport.Rows[rowCnt][ColCnt] = dtChallan.ToString("dd/MM/yyy");

                                            for (int hd = 0; hd < chklsheader.Items.Count; hd++)
                                            {
                                                if (chklsheader.Items[hd].Selected == true)
                                                {
                                                    string hdFK = Convert.ToString(chklsheader.Items[hd].Value);
                                                    string strFilter = string.Empty;
                                                    hashValue = feeCat + "-" + hdFK;
                                                    if (!string.IsNullOrEmpty(challanNo))
                                                    {
                                                        strFilter = "fee_category='" + feeCat + "' and header_id='" + hdFK + "' and challan_no='" + challanNo + "' and cal_date='" + dtChallan.ToString("MM/dd/yyyy") + "' and app_no='" + appNo + "'";
                                                    }
                                                    else
                                                    {
                                                        strFilter = "fee_category='" + feeCat + "' and header_id='" + hdFK + "' and TransCode='" + reciptNo + "' and cal_date='" + dtChallan.ToString("MM/dd/yyyy") + "' and app_no='" + appNo + "'";
                                                    }
                                                    double paidAmount = 0;
                                                    ds.Tables[3].DefaultView.RowFilter = strFilter;
                                                    DataTable dtPaidDet = ds.Tables[3].DefaultView.ToTable();
                                                    if (dtPaidDet.Rows.Count > 0)
                                                    {
                                                        int.TryParse(Convert.ToString(htCol[hashValue]), out ColCnt);
                                                        double.TryParse(Convert.ToString(dtPaidDet.Rows[0]["paid"]), out paidAmount);
                                                        dtTermFeeReport.Rows[rowCnt][ColCnt] = Convert.ToString(paidAmount);
                                                        semTotAmount += paidAmount;//every header total

                                                        if (!htTotal.ContainsKey(ColCnt))//grand total in bottom
                                                            htTotal.Add(ColCnt, Convert.ToString(paidAmount));
                                                        else
                                                        {
                                                            double amount = 0;
                                                            double.TryParse(Convert.ToString(htTotal[ColCnt]), out amount);
                                                            amount += paidAmount;
                                                            htTotal.Remove(ColCnt);
                                                            htTotal.Add(ColCnt, Convert.ToString(amount));
                                                        }
                                                        boolCheck = true;
                                                    }
                                                    string deducT = Convert.ToString(getCblSelectedValue(cbldeduct));

                                                    if (!string.IsNullOrEmpty(deducT))
                                                    {
                                                        if (dicdedction.ContainsKey(deducT.Trim().ToLower() + feeCat))
                                                        {
                                                            Double getdet = dicdedction[deducT.Trim().ToLower() + feeCat] + paidAmount;
                                                            dicdedction[deducT.Trim().ToLower() + feeCat] = getdet;
                                                        }
                                                        else
                                                            dicdedction.Add(deducT.Trim().ToLower() + feeCat, paidAmount);
                                                    }
                                                }
                                            }
                                            if (semTotAmount > 0)
                                            {
                                                hashValue = feeCat + "-" + "Total";
                                                int.TryParse(Convert.ToString(htCol[hashValue]), out ColCnt);
                                                dtTermFeeReport.Rows[rowCnt][ColCnt] = Convert.ToString(semTotAmount);
                                                if (!htTotal.ContainsKey(ColCnt))//grand total in bottom
                                                    htTotal.Add(ColCnt, Convert.ToString(semTotAmount));
                                                else
                                                {
                                                    double amount = 0;
                                                    double.TryParse(Convert.ToString(htTotal[ColCnt]), out amount);
                                                    amount += semTotAmount;
                                                    htTotal.Remove(ColCnt);
                                                    htTotal.Add(ColCnt, Convert.ToString(amount));
                                                }
                                            }
                                            --rowCnts;//decreasing row count
                                        }
                                    }
                                }
                                //if (semTotAmount > 0)
                                //{
                                //    string hashValue = feeCat + "-" + "Total";
                                //    int.TryParse(Convert.ToString(htCol[hashValue]), out ColCnt);
                                //    fpspread.Sheets[0].Cells[rowCnt, ColCnt].Text = Convert.ToString(semTotAmount);
                                //}


                            }
                            #endregion
                        }
                        else//without paid
                        {
                            #region without paid student details

                            string rollno = Convert.ToString(ds.Tables[0].Rows[row]["Roll_No"]);
                            string regno = Convert.ToString(ds.Tables[0].Rows[row]["Reg_No"]);
                            string rolladmit = Convert.ToString(ds.Tables[0].Rows[row]["roll_admit"]);
                            string appno = Convert.ToString(ds.Tables[0].Rows[row]["app_no"]);
                            string name = Convert.ToString(ds.Tables[0].Rows[row]["Stud_Name"]);
                            string currentsem = Convert.ToString(ds.Tables[0].Rows[row]["Current_Semester"]);
                            string dedcureason = "";
                            if (ds.Tables[4].Rows.Count > 0)
                            {
                                ds.Tables[4].DefaultView.RowFilter = "app_no='" + appNo + "'";
                                DataTable dvdeductreason = ds.Tables[4].DefaultView.ToTable();
                                if (dvdeductreason.Rows.Count > 0)
                                {
                                    for (int k = 0; k < dvdeductreason.Rows.Count; k++)
                                    {
                                        if (dedcureason == "")
                                            dedcureason = Convert.ToString(dvdeductreason.Rows[k]["detection"]);
                                        else
                                            dedcureason += "," + Convert.ToString(dvdeductreason.Rows[k]["detection"]);
                                    }
                                }
                            }
                            string community = "";
                            if (ds.Tables[5].Rows.Count > 0)
                            {
                                ds.Tables[5].DefaultView.RowFilter = "app_no='" + appno + "'";
                                DataTable dtCommut = ds.Tables[5].DefaultView.ToTable(); ;
                                if (dtCommut.Rows.Count > 0)
                                    community = Convert.ToString(dtCommut.Rows[0]["textval"]);
                            }
                            drowInst = dtTermFeeReport.NewRow();
                            drowInst[0] = Convert.ToString(SnoCount);
                            drowInst[1] = rollno;
                            drowInst[2] = regno;
                            drowInst[3] = rolladmit;
                            drowInst[4] = name;
                            drowInst[5] = dedcureason;
                            drowInst[6] = community;
                            for (int coL = 7; coL < dtTermFeeReport.Columns.Count; coL++)
                            {
                                drowInst[coL] = "-";
                            }
                            boolCheck = true;
                            dtTermFeeReport.Rows.Add(drowInst);

                            #endregion
                        }

                    }

                    #region grandtot

                    if (true)
                    {
                        drowInst = dtTermFeeReport.NewRow();
                        drowInst[0] = "Total";
                        dicRowColor.Add(dtTermFeeReport.Rows.Count, "Total");
                        double grandvalues = 0;
                        for (int j = 7; j < dtTermFeeReport.Columns.Count; j++)
                        {
                            double.TryParse(Convert.ToString(htTotal[j]), out grandvalues);
                            drowInst[j] = Convert.ToString(grandvalues);
                        }
                        dtTermFeeReport.Rows.Add(drowInst);
                    }

                    #endregion

                    #endregion

                    if (boolCheck)//additional detail added here
                    {
                        #region deduction

                        //int noofhear = GetSelectedItemsValueList(chklsheader).Count;
                        //noofhear += 4;//receiptno,challano,date +1
                        ////string SelQ = "select distinct ISNULL(t.TextVal,'') as detection from fee_allot a,TextValTable t,Registration r where r.Roll_Admit=a.roll_admit and convert(nvarchar(50),t.TextCode)=convert(nvarchar(50),a.dedect_reason) and r.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and r.degree_code='" + ddldept.SelectedValue + "' and ISNULL(dedect_reason,'') <> '' and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar'";
                        ////select distinct ISNULL(t.TextVal,'') as detection from FT_FeeAllot a,TextValTable t,Registration r where r.app_no=a.app_no and convert(nvarchar(50),t.TextCode)=convert(nvarchar(50),a.DeductReason) and r.Batch_Year='2016' and r.degree_code='68' and ISNULL(DeductReason,'') <> '' and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar'
                        //string SelQ = "select distinct ISNULL(t.TextVal,'') as detection from FT_FeeAllot a,TextValTable t,Registration r where r.app_no=a.app_no and convert(nvarchar(50),t.TextCode)=convert(nvarchar(50),a.DeductReason) and r.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and r.degree_code='" + ddldept.SelectedValue + "' and ISNULL(DeductReason,'') <> '' and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar'";
                        //ds.Dispose();
                        //ds.Reset();
                        //ds = da.select_method_wo_parameter(SelQ, "Text");
                        //if (ds.Tables[0].Rows.Count > 0)
                        //{
                        //    fpspread.Sheets[0].RowCount++;
                        //    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Text = "CONCESSION";
                        //    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                        //    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        //    fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                        //    fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, 0, 1, fpspread.Sheets[0].ColumnCount);
                        //    for (int d = 0; d < ds.Tables[0].Rows.Count; d++)
                        //    {
                        //        fpspread.Sheets[0].RowCount++;
                        //        string dection = ds.Tables[0].Rows[d]["detection"].ToString();
                        //        int count = ds.Tables[0].Rows.Count;
                        //        for (int c = 8; c < fpspread.Sheets[0].ColumnCount; c = c + noofhear)
                        //        {
                        //          //  string feecat = fpspread.Sheets[0].ColumnHeader.Cells[1, c].Note.ToString();
                        //            string setval = dection + feeCat;

                        //            if (dicdedction.ContainsKey(setval))
                        //            {
                        //                Double getval = dicdedction[setval];
                        //                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, c].Text = dection.ToUpper().ToString();
                        //                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, c].Font.Bold = true;
                        //                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, c].Font.Size = FontUnit.Medium;
                        //                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Right;

                        //                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, c + 1].Text = getval.ToString();
                        //                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, c + 1].Font.Bold = true;
                        //                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, c + 1].Font.Size = FontUnit.Medium;
                        //                fpspread.Sheets[0].Cells[fpspread.Sheets[0].RowCount - 1, c + 1].HorizontalAlign = HorizontalAlign.Left;
                        //                fpspread.Sheets[0].SpanModel.Add(fpspread.Sheets[0].RowCount - 1, c + 1, 1, noofhear - 1);
                        //            }
                        //        }
                        //    }
                        //}

                        #endregion

                        #region final column add old

                        int cnt = 0;
                        int totad = 0;
                        int inst = 0;
                        int ffc = 0;
                        int sfc = 0;
                        int left = 0;
                        bool check = false;
                        int valcol = 0;
                        DataSet dsload = new DataSet();

                        Dictionary<string, int> htdet = new Dictionary<string, int>();
                        string DeductR = "";
                        for (int a = 0; a < cbldeduct.Items.Count; a++)
                        {
                            if (cbldeduct.Items[a].Selected == true)
                            {
                                if (DeductR == "")
                                    DeductR = cbldeduct.Items[a].Text.ToString();
                                else
                                    DeductR = DeductR + "','" + cbldeduct.Items[a].Text.ToString();
                            }
                        }
                        for (int sem = 0; sem < cblsem.Items.Count; sem++)
                        {
                            if (cblsem.Items[sem].Selected == true)
                            {
                                string Cursem = Convert.ToString(cblsem.Items[sem].Value);
                                Dictionary<string, int> htdetvalue = new Dictionary<string, int>();
                                double totfnlval = 0;
                                cnt++;
                                int colcnt = Convert.ToInt32(htsem[Convert.ToString(cblsem.Items[sem].Value)]);

                                if (cnt == 1)
                                {
                                    drowInst = dtTermFeeReport.NewRow();
                                    totad = dtTermFeeReport.Rows.Count;
                                    dtTermFeeReport.Rows.Add(drowInst);
                                    dicRowColor.Add(totad, "Bold");
                                }
                                dtTermFeeReport.Rows[totad][colcnt] = "Total Advance";

                                dsload.Clear();
                                dsload = getDeductionDetails(Cursem);

                                valcol = colcnt;
                                if (cnt == 1)
                                {
                                    drowInst = dtTermFeeReport.NewRow();
                                    inst = dtTermFeeReport.Rows.Count;
                                    dtTermFeeReport.Rows.Add(drowInst);
                                    dicRowColor.Add(inst, "Bold");
                                }
                                dtTermFeeReport.Rows[inst][colcnt] = "Total Installment";

                                //dynamic column adding
                                if (cbldeduct.Items.Count > 0)
                                {
                                    for (int det = 0; det < cbldeduct.Items.Count; det++)
                                    {
                                        if (cbldeduct.Items[det].Selected == true)
                                        {
                                            if (check == false)
                                            {
                                                drowInst = dtTermFeeReport.NewRow();
                                                ffc = dtTermFeeReport.Rows.Count;
                                                htdet.Add(Convert.ToString(cbldeduct.Items[det].Text), ffc);
                                                dtTermFeeReport.Rows.Add(drowInst);
                                                dicRowColor.Add(ffc, "Bold");
                                            }
                                            else if (!htdet.ContainsKey(Convert.ToString(cblsem.Items[sem].Value)))
                                            {
                                                if (htdet.Count > 0)
                                                    ffc = Convert.ToInt32(htdet[Convert.ToString(cbldeduct.Items[det].Text)]);
                                            }
                                            dtTermFeeReport.Rows[ffc][colcnt] = Convert.ToString(cbldeduct.Items[det].Text);
                                        }
                                    }
                                    check = true;
                                }

                                #region value bind

                                double paidcnt = 0;
                                double paidamt = 0;
                                double partcnt = 0;
                                double partamt = 0;
                                double notcnt = 0;
                                double notamt = 0;
                                double leftcnt = 0;
                                double ffcCnt = 0;
                                double sfcCnt = 0;
                                DataView dvpaidCnt = new DataView();
                                DataView dvdbset = new DataView();
                                if (dsload.Tables.Count > 0 && dsload.Tables[0].Rows.Count > 0)
                                {
                                    if (dsload.Tables[0].Rows.Count > 0)
                                    {
                                        for (int sel = 0; sel < dsload.Tables[2].Rows.Count; sel++)
                                        {
                                            double DemandAmt = 0;
                                            double paidAmt = 0;
                                            // string app_no = Convert.ToString(dsload.Tables[2].Rows[sel]["app_no"]);
                                            dsload.Tables[0].DefaultView.RowFilter = "App_no='" + Convert.ToString(dsload.Tables[2].Rows[sel]["App_no"]) + "' and Batch_year='" + Convert.ToString(dsload.Tables[2].Rows[sel]["Batch_year"]) + "' and Degree_Code='" + Convert.ToString(dsload.Tables[2].Rows[sel]["Degree_Code"]) + "' and FeeCategory='" + Convert.ToString(cblsem.Items[sem].Value)
            + "'";
                                            DataView dvdemand = dsload.Tables[0].DefaultView;
                                            if (dvdemand.Count > 0)
                                                double.TryParse(Convert.ToString(dvdemand[0]["Demand"]), out DemandAmt);

                                            dsload.Tables[1].DefaultView.RowFilter = "App_no='" + Convert.ToString(dsload.Tables[2].Rows[sel]["App_no"]) + "' and Batch_year='" + Convert.ToString(dsload.Tables[2].Rows[sel]["Batch_year"]) + "' and Degree_Code='" + Convert.ToString(dsload.Tables[2].Rows[sel]["Degree_Code"]) + "' and FeeCategory='" + Convert.ToString(cblsem.Items[sem].Value) + "'";
                                            dvpaidCnt = dsload.Tables[1].DefaultView;
                                            if (dvpaidCnt.Count > 0)
                                            {
                                                double.TryParse(Convert.ToString(dvpaidCnt[0]["Paid"]), out paidAmt);

                                                if (DemandAmt == paidAmt || DemandAmt < paidAmt)
                                                {
                                                    paidamt += DemandAmt;
                                                    paidcnt++;
                                                }
                                                else if (DemandAmt > paidAmt)
                                                {
                                                    double balAmt = 0;
                                                    balAmt = paidAmt;
                                                    partamt += balAmt;
                                                    partcnt++;
                                                }
                                                else
                                                {
                                                    notamt += DemandAmt;
                                                    notcnt++;
                                                }
                                            }
                                            else
                                            {
                                                if (checkdicon.Checked == true)
                                                {
                                                    if (dsload.Tables[2].Rows.Count > 0)
                                                    {
                                                        dsload.Tables[2].DefaultView.RowFilter = " App_no='" + Convert.ToString(dsload.Tables[2].Rows[sel]["App_no"]) + "' and (cc='True' or DelFlag='1' or Exam_Flag like '%Debar%') ";
                                                        DataView dv = dsload.Tables[2].DefaultView;
                                                        if (dv.Count > 0)
                                                            leftcnt++;
                                                        else
                                                            notcnt++;
                                                    }
                                                    else
                                                    {
                                                        notamt += DemandAmt;
                                                        notcnt++;
                                                    }
                                                }
                                                else
                                                {
                                                    notamt += DemandAmt;
                                                    notcnt++;
                                                }
                                            }
                                            //deduction
                                            dsload.Tables[3].DefaultView.RowFilter = "App_no='" + Convert.ToString(dsload.Tables[2].Rows[sel]["App_no"]) + "' and Batch_year='" + Convert.ToString(dsload.Tables[2].Rows[sel]["Batch_year"]) + "' and Degree_Code='" + Convert.ToString(dsload.Tables[2].Rows[sel]["Degree_Code"]) + "' and FeeCategory='" + Convert.ToString(cblsem.Items[sem].Value) + "'";
                                            DataView dvcon = new DataView();
                                            dvcon = dsload.Tables[3].DefaultView;
                                            if (dvcon.Count > 0)
                                            {
                                                for (int con = 0; con < dvcon.Count; con++)
                                                {
                                                    ffcCnt = 0;
                                                    string detres = Convert.ToString(dvcon[con]["detection"]);
                                                    if (!htdetvalue.ContainsKey(detres))
                                                    {
                                                        ffcCnt++;
                                                        htdetvalue.Add(detres, Convert.ToInt32(ffcCnt));
                                                    }
                                                    else
                                                    {
                                                        ffcCnt++;
                                                        double total = 0;
                                                        double.TryParse(Convert.ToString(htdetvalue[detres]), out total);
                                                        total += ffcCnt;
                                                        htdetvalue.Remove(Convert.ToString(detres));
                                                        htdetvalue.Add(detres, Convert.ToInt32(total));
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    //fully paid
                                    dtTermFeeReport.Rows[totad][valcol + 1] = Convert.ToString(paidcnt);
                                    totfnlval += paidcnt;
                                    //partial and not paid
                                    double fnlcnt = partcnt + notcnt;
                                    dtTermFeeReport.Rows[inst][valcol + 1] = Convert.ToString(fnlcnt);
                                    totfnlval += fnlcnt;
                                    //dynamic value bind

                                    foreach (KeyValuePair<string, int> htval in htdet)
                                    {
                                        int colval = 0;
                                        string deductR = "";
                                        Int32.TryParse(Convert.ToString(htval.Value), out colval);
                                        deductR = htval.Key.ToString();
                                        //total count
                                        double count = 0;
                                        if (htdetvalue.ContainsKey(deductR))
                                            double.TryParse(Convert.ToString(htdetvalue[deductR]), out count);

                                        dtTermFeeReport.Rows[colval][valcol + 1] = Convert.ToString(count);
                                        totfnlval += count;
                                    }
                                }
                                #endregion

                                //dynamic column adding
                                if (cbldeduct.Items.Count > 0)
                                {
                                    for (int det = 0; det < cbldeduct.Items.Count; det++)
                                    {
                                        if (cbldeduct.Items[det].Selected == true)
                                        {
                                            if (check == false)
                                            {
                                                drowInst = dtTermFeeReport.NewRow();
                                                ffc = dtTermFeeReport.Rows.Count;
                                                htdet.Add(Convert.ToString(cbldeduct.Items[det].Text), ffc);
                                                dtTermFeeReport.Rows.Add(drowInst);
                                            }
                                            else if (!htdet.ContainsKey(Convert.ToString(cblsem.Items[sem].Value)))
                                            {
                                                if (htdet.Count > 0)
                                                    ffc = Convert.ToInt32(htdet[Convert.ToString(cbldeduct.Items[det].Text)]);
                                            }
                                            dtTermFeeReport.Rows[ffc][colcnt] = Convert.ToString(cbldeduct.Items[det].Text);
                                        }
                                    }
                                    check = true;
                                }
                                if (cnt == 1)
                                    drowInst = dtTermFeeReport.NewRow();
                                drowInst[colcnt] = "Total";
                                drowInst[valcol + 1] = Convert.ToString(totfnlval);
                                if (cnt == 1)
                                {
                                    dicCellColor.Add(dtTermFeeReport.Rows.Count + "$" + 0, "Total");
                                    dtTermFeeReport.Rows.Add(drowInst);
                                }
                            }
                        }

                        #endregion
                    }
                }
                else
                {
                    #region academic year Added by abarna on 03/01/2018

                    string acdBatchYear = string.Empty;
                    string feecaT = string.Empty;
                    string feeCategory = string.Empty;
                    string BatchYear = string.Empty;
                    string batch_year = string.Empty;
                    DataTable dtFee = new DataTable();
                    StringBuilder sbSem = new StringBuilder();
                    StringBuilder sbSemStr = new StringBuilder();
                    Dictionary<string, string> htAcademic = new Dictionary<string, string>();

                    string settingType = string.Empty;
                    if (rblTypeNew.SelectedIndex == 0)
                        settingType = "0";
                    else if (rblTypeNew.SelectedIndex == 1)
                        settingType = "1";
                    else if (rblTypeNew.SelectedIndex == 2)
                        settingType = "2";
                    string collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
                    string acdYears = Convert.ToString(ddlAcademic.SelectedItem.Text);
                    string selQuery = " SELECT distinct ACD_COLLEGE_CODE,c.collname,ACD_YEAR,ACD_BATCH_YEAR,ACD_FEECATEGORY,t.textval FROM FT_ACADEMICYEAR AY,FT_ACADEMICYEAR_DETAILED AYD,collinfo c,textvaltable t WHERE c.college_code=ay.acd_college_code and AY.ACA_YEAR_PK=AYD.ACA_YEAR_FK and textcriteria='FEECA' and t.textcode=ayd.ACD_FEECATEGORY and t.college_code=ay.ACD_COLLEGE_CODE AND  ACD_COLLEGE_CODE IN('" + collegecode + "') and ACD_YEAR in('" + acdYears + "') and ACD_SETTING_TYPE='" + settingType + "' order by ACD_COLLEGE_CODE  asc,ACD_YEAR desc,ACD_FEECATEGORY asc";
                    DataSet dsPrevAMount = da.select_method_wo_parameter(selQuery, "Text");
                    if (dsPrevAMount.Tables.Count > 0 && dsPrevAMount.Tables[0].Rows.Count > 0)
                    {
                        DataTable dtAcdYear = dsPrevAMount.Tables[0].DefaultView.ToTable(true, "ACD_YEAR", "ACD_COLLEGE_CODE", "collname");
                        DataTable dtBatchYear = dsPrevAMount.Tables[0].DefaultView.ToTable(true, "ACD_YEAR", "ACD_BATCH_YEAR", "ACD_COLLEGE_CODE");
                        DataTable dtFeecat = dsPrevAMount.Tables[0].DefaultView.ToTable();

                        if (dtAcdYear.Rows.Count > 0)
                        {
                            int Sno = 0;
                            for (int row = 0; row < dtAcdYear.Rows.Count; row++)
                            {
                                Sno++;
                                string acdYear = Convert.ToString(dtAcdYear.Rows[row]["ACD_YEAR"]);
                                string clgCode = Convert.ToString(dtAcdYear.Rows[row]["ACD_COLLEGE_CODE"]);
                                dtBatchYear.DefaultView.RowFilter = "ACD_YEAR='" + acdYear + "' and ACD_COLLEGE_CODE='" + clgCode + "'";
                                DataTable dtBatch = dtBatchYear.DefaultView.ToTable();
                                if (dtBatch.Rows.Count > 0)
                                {
                                    for (int bat = 0; bat < dtBatch.Rows.Count; bat++)
                                    {
                                        acdBatchYear = Convert.ToString(dtBatch.Rows[bat]["ACD_BATCH_YEAR"]);
                                        dtFeecat.DefaultView.RowFilter = "ACD_YEAR='" + acdYear + "' and ACD_BATCH_YEAR='" + acdBatchYear + "' and ACD_COLLEGE_CODE='" + clgCode + "'";
                                        dtFee = dtFeecat.DefaultView.ToTable();
                                        if (dtFee.Rows.Count > 0)
                                        {
                                            //StringBuilder sbSem = new StringBuilder();
                                            //StringBuilder sbSemStr = new StringBuilder();
                                            for (int fee = 0; fee < dtFee.Rows.Count; fee++)
                                            {
                                                feecaT = Convert.ToString(dtFee.Rows[fee]["ACD_FEECATEGORY"]);
                                                string feecaTStr = Convert.ToString(dtFee.Rows[fee]["textval"]);
                                                sbSem.Append(feecaT + ",");
                                                // sbSemStr.Append(feecaTStr + ",");
                                                BatchYear = Convert.ToString(dtFee.Rows[fee]["ACD_BATCH_YEAR"]);

                                                if (feecaT == "")
                                                {
                                                    feeCategory = feecaT;
                                                }
                                                else
                                                {
                                                    feeCategory += "'" + "," + "'" + feecaT;
                                                }
                                                if (BatchYear == "")
                                                {
                                                    batch_year = BatchYear;
                                                }
                                                else
                                                {
                                                    batch_year += "'" + "," + "'" + BatchYear;
                                                }
                                            }
                                            if (sbSem.Length > 0)
                                                sbSem.Remove(sbSem.Length - 1, 1);
                                            if (!htAcademic.ContainsKey(clgCode + "$" + acdBatchYear))
                                                htAcademic.Add(clgCode + "$" + acdBatchYear, Convert.ToString(sbSem));
                                            //if (sbSemStr.Length > 0)
                                            //    sbSemStr.Remove(sbSemStr.Length - 1, 1);    


                                        }
                                    }
                                }
                            }
                        }
                    }


                    #endregion

                    #region design

                    loadcolumns();
                    challanAndReceiptNoRights();
                    RollAndRegSettings();
                    Printcontrol.Visible = false;

                    arrColHdrNames.Add("S.No");
                    arrColHdrNames2.Add("S.No");
                    dtTermFeeReport.Columns.Add("col0");
                    arrColHdrNames.Add("Roll No");
                    arrColHdrNames2.Add("Roll No");
                    dtTermFeeReport.Columns.Add("col1");
                    arrColHdrNames.Add("Reg No");
                    arrColHdrNames2.Add("Reg No");
                    dtTermFeeReport.Columns.Add("col2");
                    arrColHdrNames.Add("Admission No");
                    arrColHdrNames2.Add("Admission No");
                    dtTermFeeReport.Columns.Add("col3");
                    arrColHdrNames.Add("Name of the Student");
                    arrColHdrNames2.Add("Name of the Student");
                    dtTermFeeReport.Columns.Add("col4");
                    arrColHdrNames.Add("FFC/DFC /SFC/VHC /ADV/INST");
                    arrColHdrNames2.Add("FFC/DFC /SFC/VHC /ADV/INST");
                    dtTermFeeReport.Columns.Add("col5");
                    arrColHdrNames.Add("Community");
                    arrColHdrNames2.Add("Community");
                    dtTermFeeReport.Columns.Add("col6");
                    ArrayList arCol = new ArrayList();
                    arCol.Add("Receipt No");
                    arCol.Add("Challan No");
                    arCol.Add("Receipt Date");
                    Hashtable htCol = new Hashtable();
                    Hashtable htsem = new Hashtable();
                    int colHdrIndx = 7;
                    //for (int fee = 0; fee < dtFee.Rows.Count; fee++)
                    //                {
                    //                    feecaT = Convert.ToString(dtFee.Rows[fee]["ACD_FEECATEGORY"]);
                    //                    string feecaTStr = Convert.ToString(dtFee.Rows[fee]["textval"]);
                    //                    sbSem.Append(feecaT + ",");
                    //                    // sbSemStr.Append(feecaTStr + ",");
                    //                    //BatchYear = Convert.ToString(dtFee.Rows[fee]["ACD_BATCH_YEAR"]);
                    //                }
                    for (int i = 0; i < dtFee.Rows.Count; i++)
                    {
                        int semColCnt = 0;
                        int incrcolCnt = 0;
                        int tempColCnt = 0;
                        feecaT = Convert.ToString(dtFee.Rows[i]["ACD_FEECATEGORY"]);
                        string feecaTStr = Convert.ToString(dtFee.Rows[i]["textval"]);
                        //if (cblsem.Items[i].Selected == true)
                        //{
                        bool boolArCol = true;
                        for (int h = 0; h < chklsheader.Items.Count; h++)
                        {
                            if (chklsheader.Items[h].Selected == true)
                            {
                                boolSem = true;
                                tempColCnt = 0;
                                if (boolArCol)//receipt,challan no 
                                {
                                    foreach (string colName in arCol)
                                    {
                                        arrColHdrNames.Add(feecaTStr);
                                        arrColHdrNames2.Add(colName);
                                        dtTermFeeReport.Columns.Add("col" + colHdrIndx);
                                        htCol.Add(Convert.ToString(feecaT + "-" + colName), dtTermFeeReport.Columns.Count - 1);
                                        if (boolSem == true)
                                        {
                                            htsem.Add(feecaT, dtTermFeeReport.Columns.Count - 1);//for details shows in bottom of grid
                                            boolSem = false;
                                        }
                                        incrcolCnt++;
                                        colHdrIndx++;
                                    }
                                    boolArCol = false;
                                }
                                arrColHdrNames.Add(feecaTStr);
                                arrColHdrNames2.Add(chklsheader.Items[h].Text);
                                dtTermFeeReport.Columns.Add("col" + colHdrIndx);
                                string hdFK = Convert.ToString(chklsheader.Items[h].Value);
                                string tempStr = feecaT + "-" + hdFK;
                                htCol.Add(Convert.ToString(feecaT + "-" + hdFK), dtTermFeeReport.Columns.Count - 1);
                                incrcolCnt++;
                                colHdrIndx++;
                            }
                        }
                        //}
                        if (incrcolCnt > 0)
                        {
                            colHdrIndx++;
                            arrColHdrNames.Add(feecaTStr);
                            arrColHdrNames2.Add("Total");
                            dtTermFeeReport.Columns.Add("col" + colHdrIndx);
                            htCol.Add(Convert.ToString(feecaT + "-" + "Total"), dtTermFeeReport.Columns.Count - 1);
                            incrcolCnt++;
                            colHdrIndx++;
                        }
                    }
                    DataRow drHdr1 = dtTermFeeReport.NewRow();
                    DataRow drHdr2 = dtTermFeeReport.NewRow();
                    for (int grCol = 0; grCol < dtTermFeeReport.Columns.Count; grCol++)
                    {
                        drHdr1[grCol] = arrColHdrNames[grCol];
                        drHdr2[grCol] = arrColHdrNames2[grCol];
                    }
                    dtTermFeeReport.Rows.Add(drHdr1);
                    dtTermFeeReport.Rows.Add(drHdr2);

                    #endregion

                    #region Value

                    Hashtable htTotal = new Hashtable();
                    Dictionary<string, Double> dicdedction = new Dictionary<string, double>();
                    bool boolCheck = false;
                    int SnoCount = 0;
                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        SnoCount++;
                        string appNo = Convert.ToString(ds.Tables[0].Rows[row]["app_no"]);
                        Dictionary<string, int> dtYRowCnt = new Dictionary<string, int>();
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            for (int i = 0; i < dtFee.Rows.Count; i++)//get total row count based each stud receipt no
                            {
                                //if (cblsem.Items[i].Selected == true)
                                //{
                                int tempRowCnt = 0;
                                // string feeCat = Convert.ToString(cblsem.Items[i].Value);
                                feecaT = Convert.ToString(dtFee.Rows[i]["ACD_FEECATEGORY"]);
                                ds.Tables[1].DefaultView.RowFilter = " app_no='" + appNo + "' and feecategory='" + feecaT + "'";
                                DataTable dtRowCnt = ds.Tables[1].DefaultView.ToTable();
                                if (dtRowCnt.Rows.Count > 0)
                                    int.TryParse(Convert.ToString(dtRowCnt.Compute("count(TransCode)", "")), out  tempRowCnt);
                                dtYRowCnt.Add(feecaT, tempRowCnt);
                                //}
                            }
                        }
                        int studRowCnt = 0;
                        if (dtYRowCnt.Count > 0)
                        {
                            // dtYRowCnt = dtYRowCnt.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                            int.TryParse(Convert.ToString(dtYRowCnt.Values.Max()), out  studRowCnt);
                            //studRowCnt = dtYRowCnt.Values.Max();
                        }
                        //else
                        //    studRowCnt = 1;
                        if (studRowCnt > 0)//with paid
                        {
                            #region with paid student count
                            for (int colInc = 1; colInc <= studRowCnt; colInc++)//row creating based on variable count
                            {
                                drowInst = dtTermFeeReport.NewRow();
                                dtTermFeeReport.Rows.Add(drowInst);
                            }
                            for (int i = 0; i < dtFee.Rows.Count; i++)
                            {
                                int ColCnt = 0;
                                int rowCnt = 0;
                                int rowCnts = studRowCnt;
                                feecaT = Convert.ToString(dtFee.Rows[i]["ACD_FEECATEGORY"]);
                                string feecaTStr = Convert.ToString(dtFee.Rows[i]["textval"]);
                                //string feeCat = Convert.ToString(cblsem.Items[i].Value);
                                //if (cblsem.Items[i].Selected == true)//each and every semester paid details bind here
                                //{
                                ds.Tables[2].DefaultView.RowFilter = " app_no='" + appNo + "' and feecategory='" + feecaT + "'";
                                DataTable dtSemPaid = ds.Tables[2].DefaultView.ToTable();
                                if (dtSemPaid.Rows.Count > 0)
                                {
                                    for (int semR = 0; semR < dtSemPaid.Rows.Count; semR++)
                                    {
                                        // --rowCnt;//decreasing row count top to bottom
                                        double semTotAmount = 0;
                                        rowCnt = dtTermFeeReport.Rows.Count - rowCnts;
                                        string rollno = Convert.ToString(ds.Tables[0].Rows[row]["Roll_No"]);
                                        string regno = Convert.ToString(ds.Tables[0].Rows[row]["Reg_No"]);
                                        string rolladmit = Convert.ToString(ds.Tables[0].Rows[row]["roll_admit"]);
                                        string appno = Convert.ToString(ds.Tables[0].Rows[row]["app_no"]);
                                        string name = Convert.ToString(ds.Tables[0].Rows[row]["Stud_Name"]);
                                        string currentsem = Convert.ToString(ds.Tables[0].Rows[row]["Current_Semester"]);
                                        string dedcureason = "";
                                        if (ds.Tables[4].Rows.Count > 0)
                                        {
                                            ds.Tables[4].DefaultView.RowFilter = "app_no='" + appNo + "'";
                                            DataTable dvdeductreason = ds.Tables[4].DefaultView.ToTable();
                                            if (dvdeductreason.Rows.Count > 0)
                                            {
                                                for (int k = 0; k < dvdeductreason.Rows.Count; k++)
                                                {
                                                    if (dedcureason == "")
                                                        dedcureason = Convert.ToString(dvdeductreason.Rows[k]["detection"]);
                                                    else
                                                        dedcureason += "," + Convert.ToString(dvdeductreason.Rows[k]["detection"]);
                                                }
                                            }
                                        }
                                        string community = "";
                                        if (ds.Tables[5].Rows.Count > 0)
                                        {
                                            ds.Tables[5].DefaultView.RowFilter = "app_no='" + appno + "'";
                                            DataTable dtCommut = ds.Tables[5].DefaultView.ToTable(); ;
                                            if (dtCommut.Rows.Count > 0)
                                                community = Convert.ToString(dtCommut.Rows[0]["textval"]);
                                        }
                                        dtTermFeeReport.Rows[rowCnt][0] = Convert.ToString(SnoCount);
                                        dtTermFeeReport.Rows[rowCnt][1] = rollno;
                                        dtTermFeeReport.Rows[rowCnt][2] = regno;
                                        dtTermFeeReport.Rows[rowCnt][3] = rolladmit;
                                        dtTermFeeReport.Rows[rowCnt][4] = name;
                                        dtTermFeeReport.Rows[rowCnt][5] = dedcureason;
                                        dtTermFeeReport.Rows[rowCnt][6] = community;

                                        string reciptNo = Convert.ToString(dtSemPaid.Rows[semR]["TransCode"]);
                                        string challanNo = Convert.ToString(dtSemPaid.Rows[semR]["challan_no"]);
                                        string chaldate = Convert.ToString(dtSemPaid.Rows[semR]["challandate"]);
                                        string transdate = Convert.ToString(dtSemPaid.Rows[semR]["rcptdate"]);
                                        DateTime dtChallan = Convert.ToDateTime(chaldate);
                                        // DateTime dtReceipt = Convert.ToDateTime(transdate);

                                        string hashValue = feecaT + "-" + "Receipt No";
                                        int.TryParse(Convert.ToString(htCol[hashValue]), out ColCnt);
                                        dtTermFeeReport.Rows[rowCnt][ColCnt] = reciptNo;

                                        hashValue = feecaT + "-" + "Challan No";
                                        int.TryParse(Convert.ToString(htCol[hashValue]), out ColCnt);
                                        dtTermFeeReport.Rows[rowCnt][ColCnt] = challanNo;

                                        hashValue = feecaT + "-" + "Receipt Date";
                                        int.TryParse(Convert.ToString(htCol[hashValue]), out ColCnt);
                                        dtTermFeeReport.Rows[rowCnt][ColCnt] = dtChallan.ToString("dd/MM/yyy");
                                        for (int hd = 0; hd < chklsheader.Items.Count; hd++)
                                        {
                                            if (chklsheader.Items[hd].Selected == true)
                                            {
                                                string hdFK = Convert.ToString(chklsheader.Items[hd].Value);
                                                string strFilter = string.Empty;
                                                hashValue = feecaT + "-" + hdFK;
                                                if (!string.IsNullOrEmpty(challanNo))
                                                {
                                                    strFilter = "fee_category='" + feecaT + "' and header_id='" + hdFK + "' and challan_no='" + challanNo + "' and cal_date='" + dtChallan.ToString("MM/dd/yyyy") + "' and app_no='" + appNo + "'";
                                                }
                                                else
                                                {
                                                    strFilter = "fee_category='" + feecaT + "' and header_id='" + hdFK + "' and TransCode='" + reciptNo + "' and cal_date='" + dtChallan.ToString("MM/dd/yyyy") + "' and app_no='" + appNo + "'";
                                                }
                                                double paidAmount = 0;
                                                ds.Tables[3].DefaultView.RowFilter = strFilter;
                                                DataTable dtPaidDet = ds.Tables[3].DefaultView.ToTable();
                                                if (dtPaidDet.Rows.Count > 0)
                                                {
                                                    int.TryParse(Convert.ToString(htCol[hashValue]), out ColCnt);
                                                    double.TryParse(Convert.ToString(dtPaidDet.Rows[0]["paid"]), out paidAmount);
                                                    dtTermFeeReport.Rows[rowCnt][ColCnt] = Convert.ToString(paidAmount);
                                                    semTotAmount += paidAmount;//every header total

                                                    if (!htTotal.ContainsKey(ColCnt))//grand total in bottom
                                                        htTotal.Add(ColCnt, Convert.ToString(paidAmount));
                                                    else
                                                    {
                                                        double amount = 0;
                                                        double.TryParse(Convert.ToString(htTotal[ColCnt]), out amount);
                                                        amount += paidAmount;
                                                        htTotal.Remove(ColCnt);
                                                        htTotal.Add(ColCnt, Convert.ToString(amount));
                                                    }
                                                    boolCheck = true;
                                                }
                                                string deducT = string.Empty;
                                                if (!string.IsNullOrEmpty(deducT))
                                                {
                                                    if (dicdedction.ContainsKey(deducT.Trim().ToLower() + feecaT))
                                                    {
                                                        Double getdet = dicdedction[deducT.Trim().ToLower() + feecaT] + paidAmount;
                                                        dicdedction[deducT.Trim().ToLower() + feecaT] = getdet;
                                                    }
                                                    else
                                                        dicdedction.Add(deducT.Trim().ToLower() + feecaT, paidAmount);
                                                }
                                            }
                                        }
                                        if (semTotAmount > 0)
                                        {
                                            hashValue = feecaT + "-" + "Total";
                                            int.TryParse(Convert.ToString(htCol[hashValue]), out ColCnt);
                                            dtTermFeeReport.Rows[rowCnt][ColCnt] = Convert.ToString(semTotAmount);
                                            if (!htTotal.ContainsKey(ColCnt))//grand total in bottom
                                                htTotal.Add(ColCnt, Convert.ToString(semTotAmount));
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(htTotal[ColCnt]), out amount);
                                                amount += semTotAmount;
                                                htTotal.Remove(ColCnt);
                                                htTotal.Add(ColCnt, Convert.ToString(amount));
                                            }
                                        }
                                        --rowCnts;//decreasing row count
                                    }
                                }
                            }
                            //}
                            //if (semTotAmount > 0)
                            //{
                            //    string hashValue = feeCat + "-" + "Total";
                            //    int.TryParse(Convert.ToString(htCol[hashValue]), out ColCnt);
                            //    fpspread.Sheets[0].Cells[rowCnt, ColCnt].Text = Convert.ToString(semTotAmount);
                            //}
                            //}
                            #endregion
                        }
                        else//without paid
                        {
                            #region without paid student details
                            string rollno = Convert.ToString(ds.Tables[0].Rows[row]["Roll_No"]);
                            string regno = Convert.ToString(ds.Tables[0].Rows[row]["Reg_No"]);
                            string rolladmit = Convert.ToString(ds.Tables[0].Rows[row]["roll_admit"]);
                            string appno = Convert.ToString(ds.Tables[0].Rows[row]["app_no"]);
                            string name = Convert.ToString(ds.Tables[0].Rows[row]["Stud_Name"]);
                            string currentsem = Convert.ToString(ds.Tables[0].Rows[row]["Current_Semester"]);
                            string dedcureason = "";
                            if (ds.Tables[4].Rows.Count > 0)
                            {
                                ds.Tables[4].DefaultView.RowFilter = "app_no='" + appNo + "'";
                                DataTable dvdeductreason = ds.Tables[4].DefaultView.ToTable();
                                if (dvdeductreason.Rows.Count > 0)
                                {
                                    for (int k = 0; k < dvdeductreason.Rows.Count; k++)
                                    {
                                        if (dedcureason == "")
                                            dedcureason = Convert.ToString(dvdeductreason.Rows[k]["detection"]);
                                        else
                                            dedcureason += "," + Convert.ToString(dvdeductreason.Rows[k]["detection"]);
                                    }
                                }
                            }
                            string community = "";
                            if (ds.Tables[5].Rows.Count > 0)
                            {
                                ds.Tables[5].DefaultView.RowFilter = "app_no='" + appno + "'";
                                DataTable dtCommut = ds.Tables[5].DefaultView.ToTable(); ;
                                if (dtCommut.Rows.Count > 0)
                                    community = Convert.ToString(dtCommut.Rows[0]["textval"]);
                            }
                            drowInst = dtTermFeeReport.NewRow();
                            //int rowCnt = fpspread.Sheets[0].RowCount - 1;
                            drowInst[0] = Convert.ToString(SnoCount);
                            drowInst[1] = rollno;
                            drowInst[2] = regno;
                            drowInst[3] = rolladmit;
                            drowInst[4] = name;
                            drowInst[5] = dedcureason;
                            drowInst[6] = community;
                            for (int coL = 7; coL < dtTermFeeReport.Columns.Count - 1; coL++)
                            {
                                drowInst[coL] = "-";
                            }
                            dtTermFeeReport.Rows.Add(drowInst);
                            boolCheck = true;
                            #endregion
                        }
                    }

                    #region grandtot

                    if (true)
                    {
                        drowInst = dtTermFeeReport.NewRow();
                        drowInst[0] = "Total";
                        dicRowColor.Add(dtTermFeeReport.Rows.Count, "Total");
                        double grandvalues = 0;
                        for (int j = 7; j < dtTermFeeReport.Columns.Count; j++)
                        {
                            double.TryParse(Convert.ToString(htTotal[j]), out grandvalues);
                            drowInst[j] = Convert.ToString(grandvalues);
                        }
                        dtTermFeeReport.Rows.Add(drowInst);
                    }
                    #endregion

                    #endregion

                    if (boolCheck)//additional detail added here
                    {
                        #region deduction

                        int noofhear = GetSelectedItemsValueList(chklsheader).Count;
                        noofhear += 4;//receiptno,challano,date +1
                        string SelQ = "select distinct ISNULL(t.TextVal,'') as detection from fee_allot a,TextValTable t,Registration r where r.Roll_Admit=a.roll_admit and convert(nvarchar(50),t.TextCode)=convert(nvarchar(50),a.dedect_reason) and r.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and r.degree_code='" + ddldept.SelectedValue + "' and ISNULL(dedect_reason,'') <> '' and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar'";
                        ds.Dispose();
                        ds.Reset();
                        ds = da.select_method_wo_parameter(SelQ, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            drowInst = dtTermFeeReport.NewRow();
                            drowInst[0] = "CONCESSION";
                            for (int d = 0; d < ds.Tables[0].Rows.Count; d++)
                            {
                                drowInst = dtTermFeeReport.NewRow();
                                string dection = ds.Tables[0].Rows[d]["detection"].ToString();
                                for (int c = 8; c < dtTermFeeReport.Rows.Count; c = c + noofhear)
                                {
                                    string feecat = Convert.ToString(dtTermFeeReport.Rows[0][c].ToString());//fpspread.Sheets[0].ColumnHeader.Cells[1, c].Note.ToString();
                                    string setval = dection + feecat;
                                    //if (dicdedction.ContainsKey(setval))
                                    //{
                                    Double getval = dicdedction[setval];
                                    drowInst[c] = dection.ToUpper().ToString();

                                    drowInst[c + 1] = getval.ToString();
                                    //}
                                }
                            }
                        }

                        #endregion

                        #region final column add old
                        //  final columns
                        int cnt = 0;
                        int totad = 0;
                        int inst = 0;
                        int ffc = 0;
                        int sfc = 0;
                        int left = 0;
                        bool check = false;
                        DataSet dsload = new DataSet();

                        Dictionary<string, int> htdet = new Dictionary<string, int>();
                        string DeductR = "";
                        for (int a = 0; a < cbldeduct.Items.Count; a++)
                        {
                            if (cbldeduct.Items[a].Selected == true)
                            {
                                if (DeductR == "")
                                    DeductR = cbldeduct.Items[a].Text.ToString();
                                else
                                    DeductR = DeductR + "','" + cbldeduct.Items[a].Text.ToString();
                            }
                        }
                        //for (int fee = 0; fee < dtFee.Rows.Count; fee++)
                        //                {
                        //                    feecaT = Convert.ToString(dtFee.Rows[fee]["ACD_FEECATEGORY"]);
                        //                    string feecaTStr = Convert.ToString(dtFee.Rows[fee]["textval"]);
                        //                    sbSem.Append(feecaT + ",");
                        //                    // sbSemStr.Append(feecaTStr + ",");
                        //                    //BatchYear = Convert.ToString(dtFee.Rows[fee]["ACD_BATCH_YEAR"]);
                        //                }
                        for (int sem = 0; sem < dtFee.Rows.Count; sem++)
                        {
                            //if (cblsem.Items[sem].Selected == true)
                            //{
                            //string Cursem = Convert.ToString(cblsem.Items[sem].Value);
                            feecaT = Convert.ToString(dtFee.Rows[sem]["ACD_FEECATEGORY"]);
                            string Cursem = Convert.ToString(feecaT);
                            Dictionary<string, int> htdetvalue = new Dictionary<string, int>();
                            double totfnlval = 0;
                            cnt++;
                            int colcnt = Convert.ToInt32(htsem[feecaT]);

                            if (cnt == 1)
                            {
                                drowInst = dtTermFeeReport.NewRow();
                                totad = dtTermFeeReport.Rows.Count;
                                dtTermFeeReport.Rows.Add(drowInst);
                                dicRowColor.Add(totad, "Bold");
                            }
                            dtTermFeeReport.Rows[totad][colcnt] = "Total Advance";

                            dsload.Clear();
                            dsload = getDeductionDetails(Cursem);

                            int valcol = colcnt;
                            if (cnt == 1)
                            {
                                drowInst = dtTermFeeReport.NewRow();
                                inst = dtTermFeeReport.Rows.Count;
                                dtTermFeeReport.Rows.Add(drowInst);
                                dicRowColor.Add(inst, "Bold");
                            }
                            dtTermFeeReport.Rows[inst][colcnt] = "Total Installment";
                            dsload.Clear();
                            dsload = getDeductionDetails(Cursem);

                            //dynamic column adding
                            if (cbldeduct.Items.Count > 0)
                            {
                                for (int det = 0; det < cbldeduct.Items.Count; det++)
                                {
                                    if (cbldeduct.Items[det].Selected == true)
                                    {
                                        if (check == false)
                                        {
                                            drowInst = dtTermFeeReport.NewRow();
                                            ffc = dtTermFeeReport.Rows.Count;
                                            htdet.Add(Convert.ToString(cbldeduct.Items[det].Text), ffc);
                                            dtTermFeeReport.Rows.Add(drowInst);
                                            dicRowColor.Add(ffc, "Bold");
                                        }
                                        else if (!htdet.ContainsKey(feecaT))
                                        {
                                            if (htdet.Count > 0)
                                                ffc = Convert.ToInt32(htdet[Convert.ToString(cbldeduct.Items[det].Text)]);
                                        }
                                        dtTermFeeReport.Rows[ffc][colcnt] = Convert.ToString(cbldeduct.Items[det].Text);
                                    }
                                }
                                check = true;
                            }

                            #region value bind
                            //value bind
                            double paidcnt = 0;
                            double paidamt = 0;
                            double partcnt = 0;
                            double partamt = 0;
                            double notcnt = 0;
                            double notamt = 0;
                            double leftcnt = 0;
                            double ffcCnt = 0;
                            double sfcCnt = 0;
                            DataView dvpaidCnt = new DataView();
                            DataView dvdbset = new DataView();
                            if (dsload.Tables.Count > 0 && dsload.Tables[0].Rows.Count > 0)
                            {
                                if (dsload.Tables[0].Rows.Count > 0)
                                {
                                    for (int sel = 0; sel < dsload.Tables[2].Rows.Count; sel++)
                                    {
                                        double DemandAmt = 0;
                                        double paidAmt = 0;
                                        // string app_no = Convert.ToString(dsload.Tables[2].Rows[sel]["app_no"]);
                                        dsload.Tables[0].DefaultView.RowFilter = "App_no='" + Convert.ToString(dsload.Tables[2].Rows[sel]["App_no"]) + "' and Batch_year='" + Convert.ToString(dsload.Tables[2].Rows[sel]["Batch_year"]) + "' and Degree_Code='" + Convert.ToString(dsload.Tables[2].Rows[sel]["Degree_Code"]) + "' and FeeCategory='" + Convert.ToString(feecaT)
        + "'";
                                        DataView dvdemand = dsload.Tables[0].DefaultView;
                                        if (dvdemand.Count > 0)
                                            double.TryParse(Convert.ToString(dvdemand[0]["Demand"]), out DemandAmt);

                                        dsload.Tables[1].DefaultView.RowFilter = "App_no='" + Convert.ToString(dsload.Tables[2].Rows[sel]["App_no"]) + "' and Batch_year='" + Convert.ToString(dsload.Tables[2].Rows[sel]["Batch_year"]) + "' and Degree_Code='" + Convert.ToString(dsload.Tables[2].Rows[sel]["Degree_Code"]) + "' and FeeCategory='" + Convert.ToString(feecaT) + "'";
                                        dvpaidCnt = dsload.Tables[1].DefaultView;
                                        if (dvpaidCnt.Count > 0)
                                        {
                                            double.TryParse(Convert.ToString(dvpaidCnt[0]["Paid"]), out paidAmt);

                                            if (DemandAmt == paidAmt || DemandAmt < paidAmt)
                                            {
                                                paidamt += DemandAmt;
                                                paidcnt++;
                                            }
                                            else if (DemandAmt > paidAmt)
                                            {
                                                double balAmt = 0;
                                                balAmt = paidAmt;
                                                partamt += balAmt;
                                                partcnt++;
                                            }
                                            else
                                            {
                                                notamt += DemandAmt;
                                                notcnt++;
                                            }
                                        }
                                        else
                                        {
                                            if (checkdicon.Checked == true)
                                            {
                                                if (dsload.Tables[2].Rows.Count > 0)
                                                {
                                                    dsload.Tables[2].DefaultView.RowFilter = " App_no='" + Convert.ToString(dsload.Tables[2].Rows[sel]["App_no"]) + "' and (cc='True' or DelFlag='1' or Exam_Flag like '%Debar%') ";
                                                    DataView dv = dsload.Tables[2].DefaultView;
                                                    if (dv.Count > 0)
                                                        leftcnt++;
                                                    else
                                                        notcnt++;
                                                }
                                                else
                                                {
                                                    notamt += DemandAmt;
                                                    notcnt++;
                                                }
                                            }
                                            else
                                            {
                                                notamt += DemandAmt;
                                                notcnt++;
                                            }
                                        }
                                        //deduction
                                        dsload.Tables[3].DefaultView.RowFilter = "App_no='" + Convert.ToString(dsload.Tables[2].Rows[sel]["App_no"]) + "' and Batch_year='" + Convert.ToString(dsload.Tables[2].Rows[sel]["Batch_year"]) + "' and Degree_Code='" + Convert.ToString(dsload.Tables[2].Rows[sel]["Degree_Code"]) + "' and FeeCategory='" + Convert.ToString(feecaT) + "'";
                                        DataView dvcon = new DataView();
                                        dvcon = dsload.Tables[3].DefaultView;
                                        if (dvcon.Count > 0)
                                        {
                                            for (int con = 0; con < dvcon.Count; con++)
                                            {
                                                ffcCnt = 0;
                                                string detres = Convert.ToString(dvcon[con]["detection"]);
                                                if (!htdetvalue.ContainsKey(detres))
                                                {
                                                    ffcCnt++;
                                                    htdetvalue.Add(detres, Convert.ToInt32(ffcCnt));
                                                }
                                                else
                                                {
                                                    ffcCnt++;
                                                    double total = 0;
                                                    double.TryParse(Convert.ToString(htdetvalue[detres]), out total);
                                                    total += ffcCnt;
                                                    htdetvalue.Remove(Convert.ToString(detres));
                                                    htdetvalue.Add(detres, Convert.ToInt32(total));
                                                }
                                            }
                                        }

                                    }
                                }

                                //fully paid
                                dtTermFeeReport.Rows[totad][valcol + 1] = Convert.ToString(paidcnt);
                                //fpspread.Sheets[0].Cells[totad, valcol + 1].HorizontalAlign = HorizontalAlign.Right;
                                totfnlval += paidcnt;
                                //partial and not paid
                                double fnlcnt = partcnt + notcnt;
                                dtTermFeeReport.Rows[inst][valcol + 1] = Convert.ToString(fnlcnt);
                                //fpspread.Sheets[0].Cells[inst, valcol + 1].HorizontalAlign = HorizontalAlign.Right;
                                totfnlval += fnlcnt;
                                //dynamic value bind

                                foreach (KeyValuePair<string, int> htval in htdet)
                                {
                                    int colval = 0;
                                    string deductR = "";
                                    Int32.TryParse(Convert.ToString(htval.Value), out colval);
                                    deductR = htval.Key.ToString();
                                    //total count
                                    double count = 0;
                                    if (htdetvalue.ContainsKey(deductR))
                                        double.TryParse(Convert.ToString(htdetvalue[deductR]), out count);
                                    dtTermFeeReport.Rows[colval][valcol + 1] = Convert.ToString(count);
                                    //fpspread.Sheets[0].Cells[colval, valcol + 1].HorizontalAlign = HorizontalAlign.Right;
                                    totfnlval += count;
                                }
                            }
                            #endregion

                            if (cnt == 1)
                                drowInst = dtTermFeeReport.NewRow();

                            drowInst[colcnt] = "Total";
                            drowInst[valcol + 1] = Convert.ToString(totfnlval);
                            if (cnt == 1)
                            {
                                dicCellColor.Add(dtTermFeeReport.Rows.Count + "$" + 0, "Total");
                                dtTermFeeReport.Rows.Add(drowInst);
                            }
                        }
                        #endregion
                    }
                }

                #region Visible

                grdTermFeeReport.DataSource = dtTermFeeReport;
                grdTermFeeReport.DataBind();
                grdTermFeeReport.Visible = true;
                GridViewRow rows = grdTermFeeReport.Rows[0];
                GridViewRow previousRow = grdTermFeeReport.Rows[1];

                for (int i = 0; i < dtTermFeeReport.Columns.Count; i++)
                {
                    if (rows.Cells[i].Text == previousRow.Cells[i].Text)
                    {
                        rows.Cells[i].RowSpan = 2;
                        previousRow.Cells[i].Visible = false;
                    }
                }
                //ColumnSpan

                for (int cell = grdTermFeeReport.Rows[0].Cells.Count - 1; cell > 0; cell--)
                {
                    TableCell colum = grdTermFeeReport.Rows[0].Cells[cell];
                    TableCell previouscol = grdTermFeeReport.Rows[0].Cells[cell - 1];
                    if (colum.Text == previouscol.Text)
                    {
                        if (previouscol.ColumnSpan == 0)
                        {
                            if (colum.ColumnSpan == 0)
                            {
                                previouscol.ColumnSpan += 2;
                            }
                            else
                            {
                                previouscol.ColumnSpan += colum.ColumnSpan + 1;
                            }
                            colum.Visible = false;
                        }
                    }
                }
                grdTermFeeReport.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                grdTermFeeReport.Rows[0].Font.Bold = true;
                grdTermFeeReport.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                grdTermFeeReport.Rows[1].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                grdTermFeeReport.Rows[1].Font.Bold = true;
                grdTermFeeReport.Rows[1].HorizontalAlign = HorizontalAlign.Center;

                foreach (KeyValuePair<int, string> dr in dicRowColor)
                {
                    int g = dr.Key;
                    string DicValue = dr.Value;
                    if (DicValue == "Total")
                    {
                        grdTermFeeReport.Rows[g].BackColor = Color.Green;
                        grdTermFeeReport.Rows[g].Cells[0].ColumnSpan = 2;
                        grdTermFeeReport.Rows[g].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        grdTermFeeReport.Rows[g].Font.Size = 12;
                        grdTermFeeReport.Rows[g].Cells[0].Font.Bold = true;
                        for (int a = 1; a < 2; a++)
                        {
                            grdTermFeeReport.Rows[g].Cells[a].Visible = false;
                        }
                    }
                    if (DicValue == "Bold")
                    {
                        grdTermFeeReport.Rows[g].HorizontalAlign = HorizontalAlign.Right;
                        grdTermFeeReport.Rows[g].Font.Bold = true;
                        grdTermFeeReport.Rows[g].Font.Size = 12;
                    }
                }
                foreach (KeyValuePair<string, string> dr in dicCellColor)
                {
                    string g = dr.Key;
                    string[] colValue = g.Split('$');
                    int rowValue = Convert.ToInt32(colValue[0]);
                    int columnValue = Convert.ToInt32(colValue[1]);
                    string DicValue = dr.Value;
                    if (DicValue == "Total")
                    {
                        grdTermFeeReport.Rows[rowValue].BackColor = Color.YellowGreen;
                        grdTermFeeReport.Rows[rowValue].Font.Bold = true;
                        grdTermFeeReport.Rows[rowValue].Font.Size = 12;
                        grdTermFeeReport.Rows[rowValue].HorizontalAlign = HorizontalAlign.Right;
                    }
                    if (DicValue == "DisContinue")
                    {
                        grdTermFeeReport.Rows[rowValue].Cells[columnValue].BackColor = Color.LightCoral;
                    }
                }
                int ConcessionCnt = 0;
                if (cbldeduct.Items.Count > 0)
                {
                    for (int det = 0; det < cbldeduct.Items.Count; det++)
                    {
                        if (cbldeduct.Items[det].Selected == true)
                        {
                            ConcessionCnt++;
                        }
                    }
                }
                MergeRows(grdTermFeeReport, ConcessionCnt);

                Excel.Visible = true;
                Print.Visible = true;
                lblreptname.Visible = true;
                txtreptname.Visible = true;
                lblmsg.Visible = false;
                txtreptname.Text = "";

                #endregion
            }
            else
            {
                grdTermFeeReport.Visible = false;
                Excel.Visible = false;
                Print.Visible = false;
                lblreptname.Visible = false;
                txtreptname.Visible = false;
                txtreptname.Text = "";
                lblmsg.Visible = true;
                lblmsg.Text = "No Records Found";
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void grdTermFeeReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowIndex == 0)
            {
                if (roll == 0)
                {
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[2].Visible = true;
                    e.Row.Cells[3].Visible = true;
                }
                else if (roll == 1)
                {
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[2].Visible = true;
                    e.Row.Cells[3].Visible = true;
                }
                else if (roll == 2)
                {
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = false;

                }
                else if (roll == 3)
                {
                    e.Row.Cells[1].Visible = false;
                    e.Row.Cells[2].Visible = true;
                    e.Row.Cells[3].Visible = false;
                }
                else if (roll == 4)
                {
                    e.Row.Cells[1].Visible = false;
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = true;
                }
                else if (roll == 5)
                {
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[2].Visible = true;
                    e.Row.Cells[3].Visible = false;
                }
                else if (roll == 6)
                {
                    e.Row.Cells[1].Visible = false;
                    e.Row.Cells[2].Visible = true;
                    e.Row.Cells[3].Visible = true;
                }
                else if (roll == 7)
                {
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = true;
                }
                if (!colord.Contains("1"))
                    e.Row.Cells[1].Visible = false;
                if (colord.Count == 0)
                {
                    e.Row.Cells[1].Visible = true;
                }
                if (!colord.Contains("2"))
                    e.Row.Cells[2].Visible = false;
                if (colord.Count == 0)
                {
                    e.Row.Cells[2].Visible = true;
                }
                if (!colord.Contains("3"))
                    e.Row.Cells[3].Visible = false;
                if (colord.Count == 0)
                {
                    e.Row.Cells[3].Visible = true;
                }
                if (!colord.Contains("4"))
                    e.Row.Cells[4].Visible = false;
                if (colord.Count == 0)
                {
                    e.Row.Cells[4].Visible = true;
                }
                if (!colord.Contains("5"))
                    e.Row.Cells[5].Visible = false;
                if (colord.Count == 0)
                {
                    e.Row.Cells[5].Visible = true;
                }
                if (!colord.Contains("6"))
                    e.Row.Cells[6].Visible = false;
                if (colord.Count == 0)
                {
                    e.Row.Cells[6].Visible = true;
                }
            }
            else
            {
                if (roll == 0)
                {
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[2].Visible = true;
                    e.Row.Cells[3].Visible = true;
                }
                else if (roll == 1)
                {
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[2].Visible = true;
                    e.Row.Cells[3].Visible = true;
                }
                else if (roll == 2)
                {
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = false;

                }
                else if (roll == 3)
                {
                    e.Row.Cells[1].Visible = false;
                    e.Row.Cells[2].Visible = true;
                    e.Row.Cells[3].Visible = false;
                }
                else if (roll == 4)
                {
                    e.Row.Cells[1].Visible = false;
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = true;
                }
                else if (roll == 5)
                {
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[2].Visible = true;
                    e.Row.Cells[3].Visible = false;
                }
                else if (roll == 6)
                {
                    e.Row.Cells[1].Visible = false;
                    e.Row.Cells[2].Visible = true;
                    e.Row.Cells[3].Visible = true;
                }
                else if (roll == 7)
                {
                    e.Row.Cells[1].Visible = true;
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = true;
                }
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                if (!colord.Contains("1"))
                    e.Row.Cells[1].Visible = false;
                if (colord.Count == 0)
                {
                    e.Row.Cells[1].Visible = true;
                }
                if (!colord.Contains("2"))
                    e.Row.Cells[2].Visible = false;
                if (colord.Count == 0)
                {
                    e.Row.Cells[2].Visible = true;
                }
                if (!colord.Contains("3"))
                    e.Row.Cells[3].Visible = false;
                if (colord.Count == 0)
                {
                    e.Row.Cells[3].Visible = true;
                }
                if (!colord.Contains("4"))
                    e.Row.Cells[4].Visible = false;
                if (colord.Count == 0)
                {
                    e.Row.Cells[4].Visible = true;
                }
                if (!colord.Contains("5"))
                    e.Row.Cells[5].Visible = false;
                if (colord.Count == 0)
                {
                    e.Row.Cells[5].Visible = true;
                }
                if (!colord.Contains("6"))
                    e.Row.Cells[6].Visible = false;
                if (colord.Count == 0)
                {
                    e.Row.Cells[6].Visible = true;
                }
            }
        }
    }

    public static void MergeRows(GridView grdTermFeeReport, int ConCount)
    {
        string sNo = grdTermFeeReport.HeaderRow.Cells[0].Text;
        string RollNo = grdTermFeeReport.HeaderRow.Cells[1].Text;
        string RegNo = grdTermFeeReport.HeaderRow.Cells[2].Text;
        string rollAdmit = grdTermFeeReport.HeaderRow.Cells[3].Text;
        string name = grdTermFeeReport.HeaderRow.Cells[4].Text;
        string scholarship = grdTermFeeReport.HeaderRow.Cells[5].Text;
        string community = grdTermFeeReport.HeaderRow.Cells[6].Text;
        int concessioncnt = 0;

        concessioncnt = ConCount + 6;
        for (int rowIndex = grdTermFeeReport.Rows.Count - concessioncnt; rowIndex >= 0; rowIndex--)
        {
            GridViewRow row = grdTermFeeReport.Rows[rowIndex];
            GridViewRow previousRow = grdTermFeeReport.Rows[rowIndex + 1];

            for (int i = 0; i < row.Cells.Count; i++)
            {
                if (grdTermFeeReport.HeaderRow.Cells[i].Text.ToLower() == sNo.ToLower() || grdTermFeeReport.HeaderRow.Cells[i].Text.ToLower() == RollNo.ToLower() || grdTermFeeReport.HeaderRow.Cells[i].Text.ToLower() == RegNo.ToLower() || grdTermFeeReport.HeaderRow.Cells[i].Text.ToLower() == rollAdmit.ToLower() || grdTermFeeReport.HeaderRow.Cells[i].Text.ToLower() == name.ToLower() || grdTermFeeReport.HeaderRow.Cells[i].Text.ToLower() == community.ToLower())
                {
                    if (row.Cells[i].Text == previousRow.Cells[i].Text)
                    {
                        row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                               previousRow.Cells[i].RowSpan + 1;
                        previousRow.Cells[i].Visible = false;
                    }
                }
                if (grdTermFeeReport.HeaderRow.Cells[i].Text.ToLower() == scholarship.ToLower() && scholarship != "")
                {
                    if (row.Cells[i].Text == previousRow.Cells[i].Text)
                    {
                        row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                               previousRow.Cells[i].RowSpan + 1;
                        previousRow.Cells[i].Visible = false;
                    }
                }
            }
        }
    }

    //COLUMN order 
    protected void lnkcolorder_Click(object sender, EventArgs e)
    {
        //txtcolorder.Text = string.Empty;
        loadColumnOreder();
        columnType();
        ddlreport_SelectedIndexChanged(sender, e);
        // loadcolumns();
        divcolorder.Attributes.Add("Style", "height: 100%; display:block; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;left: 0px;");
        //divcolorder.Visible = true;
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

    protected bool getsaveColumnOrder()
    {
        bool boolSave = false;
        try
        {
            string strText = string.Empty;
            if (cblcolumnorder.Items.Count > 0)
                strText = Convert.ToString(getCblSelectedTextwithout(cblcolumnorder));
            //if (!string.IsNullOrEmpty(strText))
            //    strText = Convert.ToString(txtcolorder.Text);
            string Usercollegecode = string.Empty;
            if (Session["collegecode"] != null)
                Usercollegecode = Convert.ToString(ddlcollege.SelectedValue);
            string linkName = string.Empty;
            if (ddlreport.Items.Count > 0 && ddlreport.SelectedItem.Text != "Select")
                linkName = Convert.ToString(ddlreport.SelectedItem.Text);
            if (!string.IsNullOrEmpty(linkName) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0" && !string.IsNullOrEmpty(strText))
            {
                string SelQ = " if exists (select * from New_InsSettings where LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "' and user_code='" + usercode + "')update New_InsSettings set linkvalue='" + strText + "' where  LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "' and user_code='" + usercode + "' else insert into New_InsSettings(LinkName,linkvalue,user_code,college_code) values('" + linkName + "','" + strText + "','" + usercode + "','" + Usercollegecode + "')";
                int insQ = da.update_method_wo_parameter(SelQ, "Text");
                boolSave = true;
            }
            if (!boolSave)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please select corresponding values!')", true);
            }
        }
        catch { }
        return boolSave;
    }

    public void loadcolumns()
    {
        try
        {
            Hashtable htCol = loadColumnOreders();
            string linkname = string.Empty;
            if (ddlMainreport.Items.Count > 0 && ddlMainreport.SelectedItem.Text != "Select")
                linkname = Convert.ToString(ddlMainreport.SelectedItem.Text);
            colord.Clear();
            string selcol = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "' and college_code='" + collegecode + "' ";
            string getVal = da.GetFunction(selcol);
            if (getVal != "0")
            {
                string[] splgetVal = getVal.Split(',');
                if (splgetVal.Length > 0)
                {
                    foreach (string val in splgetVal)
                    {
                        string values = Convert.ToString(htCol[val]);
                        if (!string.IsNullOrEmpty(values) && values != "0")
                        {
                            colord.Add(values);
                        }
                    }
                }
            }
        }
        catch { }
    }

    #region report type added dropdown

    //protected void btnAdd_OnClick(object sender, EventArgs e)
    //{
    //}

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
            if (ddlcollege.Items.Count > 0)
                Usercollegecode = Convert.ToString(ddlcollege.SelectedValue);
            string strDesc = Convert.ToString(txtdesc.Text);
            string linkCriteria = string.Empty;
            //if (rblMemType.SelectedIndex == 0)
            //    linkCriteria = "FinancePaidDeailsStud";
            //else
            linkCriteria = "FinanceTermFeeReportType";
            if (!string.IsNullOrEmpty(strDesc) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
            {
                string sql = "if exists ( select * from CO_MasterValues where MasterValue ='" + strDesc + "' and MasterCriteria ='" + linkCriteria + "' and CollegeCode ='" + Usercollegecode + "') update CO_MasterValues set MasterValue ='" + strDesc + "' where MasterValue ='" + strDesc + "' and MasterCriteria ='" + linkCriteria + "' and CollegeCode ='" + Usercollegecode + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode) values ('" + strDesc + "','" + linkCriteria + "','" + Usercollegecode + "')";
                int insert = da.update_method_wo_parameter(sql, "Text");
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
        if (ddlcollege.Items.Count > 0)
            Usercollegecode = Convert.ToString(ddlcollege.SelectedValue);
        ddlreport.Items.Clear();
        ddlMainreport.Items.Clear();
        if (!string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
        {
            string linkCriteria = string.Empty;
            //if (rblMemType.SelectedIndex == 0)
            //    linkCriteria = "FinancePaidDeailsStud";
            //else
            linkCriteria = "FinanceTermFeeReportType";
            string query = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria='" + linkCriteria + "' and CollegeCode='" + Usercollegecode + "'";
            ds.Clear();
            ds = da.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlreport.DataSource = ds;
                ddlreport.DataTextField = "MasterValue";
                ddlreport.DataValueField = "MasterCode";
                ddlreport.DataBind();
                // ddlreport.Items.Insert(0, new ListItem("Select", "0"));

                //main search filter
                ddlMainreport.DataSource = ds;
                ddlMainreport.DataTextField = "MasterValue";
                ddlMainreport.DataValueField = "MasterCode";
                ddlMainreport.DataBind();
                //ddlMainreport.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
                ddlreport.Items.Insert(0, new ListItem("Select", "0"));
                ddlMainreport.Items.Insert(0, new ListItem("Select", "0"));
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
            // txtcolorder.Text = string.Empty;
            string strText = string.Empty;
            string Usercollegecode = string.Empty;
            if (ddlcollege.Items.Count > 0)
                Usercollegecode = Convert.ToString(ddlcollege.SelectedValue);
            string linkName = string.Empty;
            if (ddlreport.Items.Count > 0 && ddlreport.SelectedItem.Text != "Select")
                linkName = Convert.ToString(ddlreport.SelectedItem.Text);
            if (!string.IsNullOrEmpty(linkName) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
            {
                getName = da.GetFunction("select LinkValue from New_InsSettings where LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "' ");
                if (!string.IsNullOrEmpty(getName) && getName != "0")
                {
                    string[] splName = getName.Split(',');
                    if (splName.Length > 0)
                    {
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
                        }
                    }
                }
                else
                    boolClear = true;
            }
            else
                boolClear = true;
            if (boolClear)
            {
                // txtcolorder.Text = string.Empty;
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = false;
                }
                cb_column.Checked = false;
            }

        }
        catch { }
    }

    protected void deleteReportType()
    {
        int delMQ = 0;
        string Usercollegecode = string.Empty;
        if (ddlcollege.Items.Count > 0)
            Usercollegecode = Convert.ToString(ddlcollege.SelectedValue);
        string linkName = string.Empty;
        if (ddlreport.Items.Count > 0 && ddlreport.SelectedItem.Text != "Select")
            linkName = Convert.ToString(ddlreport.SelectedItem.Text);
        if (!string.IsNullOrEmpty(linkName) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
        {
            string linkCriteria = string.Empty;
            //if (rblMemType.SelectedIndex == 0)
            //    linkCriteria = "FinancePaidDeailsStud";
            //else
            linkCriteria = "FinanceTermFeeReportType";
            int delQ = 0;
            int.TryParse(Convert.ToString(da.update_method_wo_parameter("delete from New_InsSettings where LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "'", "Text")), out delQ);

            int.TryParse(Convert.ToString(da.update_method_wo_parameter("delete  from CO_MasterValues where MasterCriteria='" + linkCriteria + "' and mastervalue='" + linkName + "'  and collegecode='" + Usercollegecode + "'", "Text")), out delMQ);

        }
        if (delMQ > 0)
        {
            //  txtcolorder.Text = string.Empty;
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

    #endregion

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

    public void getAcademicYear()
    {
        try
        {
            string fnalyr = "";
            // string getfinanceyear = "select distinct convert(nvarchar(15),FinYearStart,103) sdate,convert(nvarchar(15),FinYearEnd,103) edate,FinYearPK from FM_FinYearMaster where CollegeCode in('" + collegecode + "')  order by FinYearPK desc";
            string getfinanceyear = "SELECT distinct ACD_YEAR FROM FT_ACADEMICYEAR AY,FT_ACADEMICYEAR_DETAILED AYD WHERE  AY.ACA_YEAR_PK=AYD.ACA_YEAR_FK  AND  ACD_COLLEGE_CODE IN('" + collegecode + "') order by ACD_YEAR desc";
            ds.Dispose();
            ds.Reset();
            ddlAcademic.Items.Clear();
            ds = da.select_method_wo_parameter(getfinanceyear, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string fdatye = ds.Tables[0].Rows[i]["ACD_YEAR"].ToString();
                    ddlAcademic.Items.Insert(0, new System.Web.UI.WebControls.ListItem(fdatye, fdatye));
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected Dictionary<string, string> getOldSettings(string acdYears)
    {
        Dictionary<string, string> htAcademic = new Dictionary<string, string>();
        try
        {
            string settingType = string.Empty;
            if (rblTypeNew.SelectedIndex == 0)
                settingType = "0";
            else if (rblTypeNew.SelectedIndex == 1)
                settingType = "1";
            else if (rblTypeNew.SelectedIndex == 2)
                settingType = "2";
            string collegecode = Convert.ToString(ddlcollege.SelectedItem.Value.ToString());
            string selQ = " SELECT distinct ACD_COLLEGE_CODE,c.collname,ACD_YEAR,ACD_BATCH_YEAR,ACD_FEECATEGORY,t.textval FROM FT_ACADEMICYEAR AY,FT_ACADEMICYEAR_DETAILED AYD,collinfo c,textvaltable t WHERE c.college_code=ay.acd_college_code and AY.ACA_YEAR_PK=AYD.ACA_YEAR_FK and textcriteria='FEECA' and t.textcode=ayd.ACD_FEECATEGORY and t.college_code=ay.ACD_COLLEGE_CODE AND  ACD_COLLEGE_CODE IN('" + collegecode + "') and ACD_YEAR in('" + acdYears + "') and ACD_SETTING_TYPE='" + settingType + "' order by ACD_COLLEGE_CODE  asc,ACD_YEAR desc,ACD_FEECATEGORY asc";
            DataSet dsPrevAMount = da.select_method_wo_parameter(selQ, "Text");
            if (dsPrevAMount.Tables.Count > 0 && dsPrevAMount.Tables[0].Rows.Count > 0)
            {
                DataTable dtAcdYear = dsPrevAMount.Tables[0].DefaultView.ToTable(true, "ACD_YEAR", "ACD_COLLEGE_CODE", "collname");
                DataTable dtBatchYear = dsPrevAMount.Tables[0].DefaultView.ToTable(true, "ACD_YEAR", "ACD_BATCH_YEAR", "ACD_COLLEGE_CODE");
                DataTable dtFeecat = dsPrevAMount.Tables[0].DefaultView.ToTable();
                if (dtAcdYear.Rows.Count > 0)
                {
                    int Sno = 0;
                    for (int row = 0; row < dtAcdYear.Rows.Count; row++)
                    {
                        Sno++;
                        string acdYear = Convert.ToString(dtAcdYear.Rows[row]["ACD_YEAR"]);
                        string clgCode = Convert.ToString(dtAcdYear.Rows[row]["ACD_COLLEGE_CODE"]);
                        dtBatchYear.DefaultView.RowFilter = "ACD_YEAR='" + acdYear + "' and ACD_COLLEGE_CODE='" + clgCode + "'";
                        DataTable dtBatch = dtBatchYear.DefaultView.ToTable();
                        if (dtBatch.Rows.Count > 0)
                        {
                            for (int bat = 0; bat < dtBatch.Rows.Count; bat++)
                            {
                                string acdBatchYear = Convert.ToString(dtBatch.Rows[bat]["ACD_BATCH_YEAR"]);
                                dtFeecat.DefaultView.RowFilter = "ACD_YEAR='" + acdYear + "' and ACD_BATCH_YEAR='" + acdBatchYear + "' and ACD_COLLEGE_CODE='" + clgCode + "'";
                                DataTable dtFee = dtFeecat.DefaultView.ToTable();
                                if (dtFee.Rows.Count > 0)
                                {
                                    StringBuilder sbSem = new StringBuilder();
                                    StringBuilder sbSemStr = new StringBuilder();
                                    for (int fee = 0; fee < dtFee.Rows.Count; fee++)
                                    {
                                        string feecaT = Convert.ToString(dtFee.Rows[fee]["ACD_FEECATEGORY"]);
                                        string feecaTStr = Convert.ToString(dtFee.Rows[fee]["textval"]);
                                        sbSem.Append(feecaT + ",");
                                        // sbSemStr.Append(feecaTStr + ",");
                                    }
                                    if (sbSem.Length > 0)
                                        sbSem.Remove(sbSem.Length - 1, 1);
                                    if (!htAcademic.ContainsKey(clgCode + "$" + acdBatchYear))
                                        htAcademic.Add(clgCode + "$" + acdBatchYear, Convert.ToString(sbSem));
                                    //if (sbSemStr.Length > 0)
                                    //    sbSemStr.Remove(sbSemStr.Length - 1, 1);                              
                                }
                            }
                        }
                    }
                }
            }
        }
        catch { }
        return htAcademic;

    }

    protected string getCurrentSemester(string batch, string degree, ref ArrayList arFeecat, string commondist, ref Dictionary<string, string> CurSemDeg)
    {
        string curSem = string.Empty;
        try
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
        catch
        {
            curSem = string.Empty;
            arFeecat.Clear();
        }
        return curSem;

    }

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

            #region with current semester
            for (int sem = 0; sem < cblsem.Items.Count; sem++)
            {
                if (cblsem.Items[sem].Selected == true)
                {
                    cblvalue = Convert.ToString(cblsem.Items[sem].Value);
                    cbltext = Convert.ToString(cblsem.Items[sem].Text);
                    if (arFeecat.Contains(cblvalue))//if current semester setting available only
                    {
                        if (type == "Semester")
                        {
                            string[] feesem = cblsem.Items[sem].Text.Split(' ');
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

            ViewState["feecat"] = htsem;

        }
        catch { }
    }
}