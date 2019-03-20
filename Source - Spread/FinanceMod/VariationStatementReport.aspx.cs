using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;
using System.Text;

public partial class VariationStatementReport : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    DAccess2 da = new DAccess2();
    int i = 0;
    int count = 0;
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string header = string.Empty;
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
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                setLabelText();
                UserbasedRights();
                loadcollege();
                if (ddl_collegename.Items.Count > 0)
                {
                    collegecode = Convert.ToString(ddl_collegename.SelectedItem.Value);
                }
                loadstrm();
                bindBtch();
                binddeg();
                binddept();
                bindsem();
                loadheaderandledger();
                ledgerload();
                //loadpaid();
                loadfinanceyear();
                loadconcession();
                getAcademicYear();

            }
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
        }
        catch (Exception ex) { }
    }
    #region college

    public void loadcollege()
    {
        try
        {
            ddl_collegename.Items.Clear();
            ds.Clear();
            string Query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(Query, "Text");
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
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            loadstrm();
            bindBtch();
            binddeg();
            binddept();
            bindsem();
            //bindsec();
            loadheaderandledger();
            ledgerload();
            //loadpaid();
            loadfinanceyear();
            loadconcession();
        }
        catch
        {
        }
    }

    #endregion

    #region stream

    public void loadstrm()
    {
        try
        {
            ddlstream.Items.Clear();
            string selqry = "select distinct type  from Course where college_code ='" + collegecode + "' and type<>''";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlstream.DataSource = ds;
                ddlstream.DataTextField = "type";
                ddlstream.DataValueField = "type";
                ddlstream.DataBind();
                ddlstream.Enabled = true;
            }
            else
            {
                ddlstream.Enabled = false;
            }
            binddeg();
        }
        catch
        { }
    }
    protected void ddlstream_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string stream = ddlstream.SelectedItem.Text.ToString();
            string selqry = "select distinct c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code='" + collegecode + "'";
            if (stream != "")
            {
                selqry = selqry + " and type  in('" + stream + "')";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(selqry, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "Course_Name";
                cbl_degree.DataValueField = "Course_Id";
                cbl_degree.DataBind();
            }
            for (int j = 0; j < cbl_degree.Items.Count; j++)
            {
                cbl_degree.Items[j].Selected = true;
                cb_degree.Checked = true;
            }

            txt_degree.Text = lbldeg.Text + "(" + cbl_degree.Items.Count + ")";
            binddept();
        }
        catch { }
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
                    for (i = 0; i < cbl_batch.Items.Count; i++)
                    {
                        cbl_batch.Items[i].Selected = true;
                    }
                    txt_batch.Text = "Batch(" + cbl_batch.Items.Count + ")";
                    cb_batch.Checked = true;
                }
            }
        }
        catch { }
    }
    protected void cb_batch_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cb_batch, cbl_batch, txt_batch, "Batch", "--Select--");
            binddeg();
            binddept();
        }
        catch { }
    }
    protected void cbl_batch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_batch, cbl_batch, txt_batch, "Batch", "--Select--");
            binddeg();
            binddept();
        }
        catch { }
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
            if (ddlstream.Items.Count > 0)
            {
                if (ddlstream.SelectedItem.Text != "")
                {
                    stream = ddlstream.SelectedItem.Text.ToString();
                }
            }

            cbl_degree.Items.Clear();
            collegecode = ddl_collegename.SelectedItem.Value.ToString();
            ds.Clear();
            string selqry = "select distinct  c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code='" + collegecode + "'";
            if (stream != "")
            {
                selqry = selqry + " and type  in('" + stream + "')";
            }
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_id";
                cbl_degree.DataBind();
                if (cbl_degree.Items.Count > 0)
                {
                    for (i = 0; i < cbl_degree.Items.Count; i++)
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
        try
        {
            CallCheckboxChange(cb_degree, cbl_degree, txt_degree, lbldeg.Text, "--Select--");
            binddept();
        }
        catch { }
    }
    protected void cbl_degree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_degree, cbl_degree, txt_degree, lbldeg.Text, "--Select--");
            binddept();
        }
        catch { }
    }
    #endregion

    #region dept
    public void binddept()
    {
        try
        {
            string batch2 = "";
            string degree = "";
            cbl_dept.Items.Clear();
            cb_dept.Checked = false;
            txt_dept.Text = "---Select---";
            batch2 = "";
            for (i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    if (batch2 == "")
                    {
                        batch2 = Convert.ToString(cbl_batch.Items[i].Text);
                    }
                    else
                    {
                        batch2 += "','" + Convert.ToString(cbl_batch.Items[i].Text);
                    }
                }

            }
            degree = "";
            for (i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    if (degree == "")
                    {
                        degree = Convert.ToString(cbl_degree.Items[i].Value);
                    }
                    else
                    {
                        degree += "," + Convert.ToString(cbl_degree.Items[i].Value);
                    }
                }

            }
            string collegecode = ddl_collegename.SelectedItem.Value.ToString();
            if (batch2 != "" && degree != "")
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
                        for (i = 0; i < cbl_dept.Items.Count; i++)
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
        try
        {
            CallCheckboxChange(cb_dept, cbl_dept, txt_dept, lbldept.Text, "--Select--");
            //bindsec();
            //bindsem();
        }
        catch { }
    }
    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_dept, cbl_dept, txt_dept, lbldept.Text, "--Select--");
            // bindsec();
            //  bindsem();
        }
        catch { }
    }
    #endregion

    #region sem
    protected void cb_sem_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cb_sem, cbl_sem, txt_sem, "Semester", "--Select--");
            //bindsec();
        }
        catch (Exception ex)
        { }
    }
    protected void cbl_sem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_sem, cbl_sem, txt_sem, "Semester", "--Select--");
            //bindsec();
        }
        catch (Exception ex)
        { }

    }

    protected void bindsem()
    {
        try
        {
            cbl_sem.Items.Clear();
            cb_sem.Checked = false;
            txt_sem.Text = "--Select--";
            ds.Clear();
            string linkName = string.Empty;
            string cbltext = string.Empty;
            ds = d2.loadFeecategory(Convert.ToString(ddl_collegename.SelectedItem.Value), usercode, ref linkName);
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
    //        string clgvalue = ddl_collegename.SelectedItem.Value.ToString();
    //        string semyear = "select * from New_InsSettings where linkname = 'SemesterandYear' and user_code ='" + usercode + "' and college_code ='" + clgvalue + "'";
    //        DataSet dsset = new DataSet();
    //        dsset.Clear();
    //        dsset = d2.select_method_wo_parameter(semyear, "Text");
    //        if (dsset.Tables.Count > 0 && dsset.Tables[0].Rows.Count > 0)
    //        {
    //            string value = Convert.ToString(dsset.Tables[0].Rows[0]["LinkValue"]);
    //            if (value == "1")
    //            {
    //                string SelectQ = "select * from textvaltable where TextCriteria = 'FEECA'and (textval like '%Semester' or textval like '%Year') and textval not like '-1%' and college_code ='" + clgvalue + "' order by len(textval),textval asc";
    //                ds.Clear();
    //                ds = d2.select_method_wo_parameter(SelectQ, "Text");
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
    //                ds = d2.select_method_wo_parameter(settingquery, "Text");
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    string linkvalue = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
    //                    if (linkvalue == "0")
    //                    {
    //                        string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Semester' and textval not like '-1%' and college_code ='" + clgvalue + "' order by len(textval),textval asc";
    //                        ds.Clear();
    //                        ds = d2.select_method_wo_parameter(semesterquery, "Text");
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
    //                        ds = d2.select_method_wo_parameter(semesterquery, "Text");
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

    #region header and ledger

    public void loadheaderandledger()
    {
        try
        {
            #region CheckBox List Load
            //string clgvalue = ddl_collegename.SelectedItem.Value.ToString();
            //cblheader.Items.Clear();
            //string query = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + clgvalue + "  ";

            //ds = d2.select_method_wo_parameter(query, "Text");
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    cblheader.DataSource = ds;
            //    cblheader.DataTextField = "HeaderName";
            //    cblheader.DataValueField = "HeaderPK";
            //    cblheader.DataBind();
            //    for (int i = 0; i < cblheader.Items.Count; i++)
            //    {
            //        cblheader.Items[i].Selected = true;
            //    }
            //    txtheader.Text = "Header(" + cblheader.Items.Count + ")";
            //    cbheader.Checked = true;
            //}
            #endregion

            #region single selection header
            string clgvalue = ddl_collegename.SelectedItem.Value.ToString();
            ddlheader.Items.Clear();
            ds.Clear();
            string Query = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + clgvalue + "  ";
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlheader.DataSource = ds;
                ddlheader.DataTextField = "HeaderName";
                ddlheader.DataValueField = "HeaderPK";
                ddlheader.DataBind();
            }
            #endregion
        }
        catch
        {
        }
    }

    protected void ddl_header_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlheader.Items.Count > 0)
        {
            header = Convert.ToString(ddlheader.SelectedItem.Value);
        }
        //loadstrm();
        //bindBtch();
        //binddeg();
        //binddept();
        //bindsem();
        //bindsec();
        //  loadheaderandledger();
        ledgerload();
        //loadpaid();
        //loadfinanceyear();
    }

    //public void cbheader_OnCheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        CallCheckboxChange(cbheader, cblheader, txtheader, "Header", "--Select--");
    //        ledgerload();
    //    }
    //    catch (Exception ex)
    //    { }
    //}
    //public void cblheader_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        CallCheckboxListChange(cbheader, cblheader, txtheader, "Header", "--Select--");
    //        ledgerload();
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}

    #region ledger

    public void ledgerload()
    {
        try
        {
            #region checkboxlist ledger
            //string clgvalue = ddl_collegename.SelectedItem.Value.ToString();
            //cblledger.Items.Clear();
            //string hed1 = ddlheader.SelectedItem.Text.ToString();
            //string hed = ddlheader.SelectedItem.Value.ToString();
            //string query1 = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='0' and L.CollegeCode = " + clgvalue + "  and L.HeaderFK in('" + hed + "')  order by isnull(l.priority,1000), l.ledgerName asc ";
            //ds.Clear();
            //ds = d2.select_method_wo_parameter(query1, "Text");
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    cblledger.DataSource = ds;
            //    cblledger.DataTextField = "LedgerName";
            //    cblledger.DataValueField = "LedgerPK";
            //    cblledger.DataBind();
            //    for (int i = 0; i < cblledger.Items.Count; i++)
            //    {
            //        cblledger.Items[i].Selected = true;
            //    }
            //    txtledger.Text = "Ledger(" + cblledger.Items.Count + ")";
            //    cbledger.Checked = true; ;

            //}
            //else
            //{
            //    for (int i = 0; i < cblledger.Items.Count; i++)
            //    {
            //        cblledger.Items[i].Selected = false;
            //    }
            //    txtledger.Text = "--Select--";
            //    cbledger.Checked = false; ;
            //}
            #endregion

            #region single selection header
            string clgvalue = ddl_collegename.SelectedItem.Value.ToString();
            string hed = ddlheader.SelectedItem.Value.ToString();
            ddlLedger.Items.Clear();
            ds.Clear();
            string Query = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='0' and L.CollegeCode = " + clgvalue + "  and L.HeaderFK in('" + hed + "')  order by isnull(l.priority,1000), l.ledgerName asc ";
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlLedger.DataSource = ds;
                ddlLedger.DataTextField = "LedgerName";
                ddlLedger.DataValueField = "LedgerPK";
                ddlLedger.DataBind();
            }
            #endregion

        }
        catch
        {
        }
    }
    //public void cbledger_OnCheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        CallCheckboxChange(cbledger, cblledger, txtledger, "Ledger", "--Select--");

    //    }
    //    catch (Exception ex)
    //    { }
    //}
    //public void cblledger_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        CallCheckboxListChange(cbledger, cblledger, txtledger, "Ledger", "--Select--");
    //    }
    //    catch (Exception ex)
    //    { }
    //}
    #endregion

    #endregion

    #region finance year

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
        try
        {
            CallCheckboxListChange(chkfyear, chklsfyear, txtfyear, "Finance Year", "--Select--");

            //loadheader();
        }
        catch (Exception ex)
        { }
    }
    protected void chkfyear_changed(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(chkfyear, chklsfyear, txtfyear, "Finance Year", "--Select--");
        }
        catch (Exception ex)
        { }
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

    #region concession

    public void loadconcession()
    {
        try
        {
            collegecode = ddl_collegename.SelectedValue.ToString();
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

                chklsconcession.Items.Add(new System.Web.UI.WebControls.ListItem("Empty", "0"));
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
            //errmsg.Visible = true;
            //errmsg.Text = ex.ToString();
        }
    }

    protected void chkconcession_changed(object sender, EventArgs e)
    {
        //clear();
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
        // clear();
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

    #endregion

    protected void UserbasedRights()
    {
        string userrht = d2.GetFunction("select value from Master_Settings where settings='Finance Include User Based Report Settings'  and usercode='" + usercode + "'");
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

        lbl.Add(lblclg);
        lbl.Add(lblstr);
        lbl.Add(lbldeg);
        lbl.Add(lbldept);
        //lbl.Add(lblsem);
        fields.Add(0);
        fields.Add(1);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

    public void getAcademicYear()
    {
        try
        {
            string college_Code = Convert.ToString(ddl_collegename.SelectedItem.Value);
            string fnalyr = "";
            // string getfinanceyear = "select distinct convert(nvarchar(15),FinYearStart,103) sdate,convert(nvarchar(15),FinYearEnd,103) edate,FinYearPK from FM_FinYearMaster where CollegeCode in('" + collegecode + "')  order by FinYearPK desc";
            string getfinanceyear = "SELECT distinct ACD_YEAR FROM FT_ACADEMICYEAR AY,FT_ACADEMICYEAR_DETAILED AYD WHERE  AY.ACA_YEAR_PK=AYD.ACA_YEAR_FK  AND  ACD_COLLEGE_CODE IN('" + college_Code + "') order by ACD_YEAR desc";
            ds.Dispose();
            ds.Reset();
            ddlAcademic.Items.Clear();
            ds = d2.select_method_wo_parameter(getfinanceyear, "text");
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
        { //d2.sendErrorMail(ex, collegecode, "ConsolidatedDemandReport.aspx"); 
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
            string collegecode = Convert.ToString(ddl_collegename.SelectedItem.Value);
            string selQ = " SELECT distinct ACD_COLLEGE_CODE,c.collname,ACD_YEAR,ACD_BATCH_YEAR,ACD_FEECATEGORY,t.textval FROM FT_ACADEMICYEAR AY,FT_ACADEMICYEAR_DETAILED AYD,collinfo c,textvaltable t WHERE c.college_code=ay.acd_college_code and AY.ACA_YEAR_PK=AYD.ACA_YEAR_FK and textcriteria='FEECA' and t.textcode=ayd.ACD_FEECATEGORY and t.college_code=ay.ACD_COLLEGE_CODE AND  ACD_COLLEGE_CODE IN('" + collegecode + "') and ACD_YEAR in('" + acdYears + "') and ACD_SETTING_TYPE='" + settingType + "' order by ACD_COLLEGE_CODE  asc,ACD_YEAR desc,ACD_FEECATEGORY asc";
            DataSet dsPrevAMount = d2.select_method_wo_parameter(selQ, "Text");
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
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collegecode, "ConsolidatedDemandReport.aspx"); 
        }
        return htAcademic;

    }

    protected Dictionary<string, string> getCurSem()
    {
        Dictionary<string, string> dtcurSem = new Dictionary<string, string>();
        try
        {
            int year = 0;
            int.TryParse(Convert.ToString(DateTime.Now.ToString("yyyy")), out year);
            dtcurSem.Add(Convert.ToString(year), Convert.ToString("1,2"));
            dtcurSem.Add(Convert.ToString(year - 1), Convert.ToString("3,4"));
            dtcurSem.Add(Convert.ToString(year - 2), Convert.ToString("5,6"));
            dtcurSem.Add(Convert.ToString(year - 3), Convert.ToString("7,8"));
            dtcurSem.Add(Convert.ToString(year - 4), Convert.ToString("9,10"));
            dtcurSem.Add(Convert.ToString(year - 5), Convert.ToString("11,12"));
            dtcurSem.Add(Convert.ToString(year - 6), Convert.ToString("12,13"));
            dtcurSem.Add(Convert.ToString(year - 7), Convert.ToString("14,15"));
        }
        catch (Exception ex)
        { //d2.sendErrorMail(ex, collegecode, "ConsolidatedDemandReport.aspx"); 
        }
        return dtcurSem;
    }

    protected Dictionary<string, string> getFeecode(string collegecode)
    {
        Dictionary<string, string> htsem = new Dictionary<string, string>();
        try
        {
            string featDegcode = string.Empty;
            //Convert.ToString(getCblSelectedValue(cbl_dept));
            string type = string.Empty;
            string cbltext = string.Empty;
            d2.featDegreeCode = featDegcode;
            DataSet dsval = d2.loadFeecategory(Convert.ToString(collegecode), usercode, ref type);
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                string feecatg = "";
                string cblvalue = "";
                //string selQ = " select textval,textcode from textvaltable where textcriteria='FEECA' and  college_code='" + collegecode + "'";
                //DataSet dsval = d2.select_method_wo_parameter(selQ, "Text");
                if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
                {
                    for (int sem = 0; sem < dsval.Tables[0].Rows.Count; sem++)
                    {
                        cblvalue = Convert.ToString(dsval.Tables[0].Rows[sem]["textcode"]);
                        cbltext = Convert.ToString(dsval.Tables[0].Rows[sem]["textval"]);
                        #region
                        if (type == "Semester")
                        {
                            #region semester
                            string[] feesem = cbltext.Split(' ');
                            if (feesem[0] == "1" || feesem[0] == "2")
                            {
                                if (!htsem.ContainsKey("1"))
                                    htsem.Add(Convert.ToString("1"), Convert.ToString(cblvalue));
                                else
                                {
                                    feecatg = Convert.ToString(htsem["1"]);
                                    feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                    htsem.Remove("1");
                                    htsem.Add(Convert.ToString("1"), feecatg);
                                }
                            }
                            else if (feesem[0] == "3" || feesem[0] == "4")
                            {
                                if (!htsem.ContainsKey("2"))
                                    htsem.Add(Convert.ToString("2"), Convert.ToString(cblvalue));
                                else
                                {
                                    feecatg = Convert.ToString(htsem["2"]);
                                    feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                    htsem.Remove("2");
                                    htsem.Add(Convert.ToString("2"), feecatg);
                                }
                            }
                            else if (feesem[0] == "5" || feesem[0] == "6")
                            {
                                if (!htsem.ContainsKey("3"))
                                    htsem.Add(Convert.ToString("3"), Convert.ToString(cblvalue));
                                else
                                {
                                    feecatg = Convert.ToString(htsem["3"]);
                                    feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                    htsem.Remove("3");
                                    htsem.Add(Convert.ToString("3"), feecatg);
                                }
                            }
                            else if (feesem[0] == "7" || feesem[0] == "8")
                            {
                                if (!htsem.ContainsKey("4"))
                                    htsem.Add(Convert.ToString("4"), Convert.ToString(cblvalue));
                                else
                                {
                                    feecatg = Convert.ToString(htsem["4"]);
                                    feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                    htsem.Remove("4");
                                    htsem.Add(Convert.ToString("4"), feecatg);
                                }
                            }
                            #endregion
                        }
                        else if (type == "Term")
                        {
                            string[] feesem = cbltext.Split(' ');
                            if (feesem[1] == "1" || feesem[1] == "2" || feesem[1] == "3" || feesem[1] == "4")
                            {
                                if (!htsem.ContainsKey("1"))
                                    htsem.Add(Convert.ToString("1"), Convert.ToString(cblvalue));
                                else
                                {
                                    feecatg = Convert.ToString(htsem["1"]);
                                    feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                    htsem.Remove("1");
                                    htsem.Add(Convert.ToString("1"), feecatg);
                                }
                            }
                        }
                        else if (type == "Year")
                        {
                            #region year
                            string[] feesem = cbltext.Split(' ');
                            if (feesem[0] == "1")
                            {
                                if (!htsem.ContainsKey("1"))
                                    htsem.Add(Convert.ToString("1"), Convert.ToString(cblvalue));
                                else
                                {
                                    feecatg = Convert.ToString(htsem["1"]);
                                    feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                    htsem.Remove("1");
                                    htsem.Add(Convert.ToString("1"), feecatg);
                                }
                            }
                            if (feesem[0] == "2")
                            {
                                if (!htsem.ContainsKey("2"))
                                    htsem.Add(Convert.ToString("2"), Convert.ToString(cblvalue));
                                else
                                {
                                    feecatg = Convert.ToString(htsem["2"]);
                                    feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                    htsem.Remove("2");
                                    htsem.Add(Convert.ToString("2"), feecatg);
                                }
                            }
                            if (feesem[0] == "3")
                            {
                                if (!htsem.ContainsKey("3"))
                                    htsem.Add(Convert.ToString("3"), Convert.ToString(cblvalue));
                                else
                                {
                                    feecatg = Convert.ToString(htsem["3"]);
                                    feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                    htsem.Remove("3");
                                    htsem.Add(Convert.ToString("3"), feecatg);
                                }
                            }
                            if (feesem[0] == "4")
                            {
                                if (!htsem.ContainsKey("4"))
                                    htsem.Add(Convert.ToString("4"), Convert.ToString(cblvalue));
                                else
                                {
                                    feecatg = Convert.ToString(htsem["4"]);
                                    feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                    htsem.Remove("4");
                                    htsem.Add(Convert.ToString("4"), feecatg);
                                }
                            }
                            #endregion
                        }
                        #endregion

                    }
                }
            }
            ViewState["feecat"] = htsem;
        }
        catch (Exception ex)
        { //d2.sendErrorMail(ex, collegecode, "ConsolidatedDemandReport.aspx"); 
        }
        return htsem;
    }

    protected string getCurYear(string curSem)
    {
        string curYear = string.Empty;
        try
        {
            switch (curSem)
            {
                case "1":
                case "2":
                case "1,2":
                    curYear = "1";
                    break;
                case "3":
                case "4":
                case "3,4":
                    curYear = "2";
                    break;
                case "5":
                case "6":
                case "5,6":
                    curYear = "3";
                    break;
                case "7":
                case "8":
                case "7,8":
                    curYear = "4";
                    break;
                case "9":
                case "10":
                case "9,10":
                    curYear = "5";
                    break;
            }
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collegecode, "ConsolidatedDemandReport.aspx"); 
        }
        return curYear;
    }

    private string getCblSelectedTerm_Text(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedText = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true && (cblSelected.Items[sel].Text.ToUpper().Contains("TERM")))
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

    #region Print

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread1, reportname);
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
            degreedetails = "Variation Statement Report " + '@';
            pagename = "VariationStatementReport.aspx";
            Printcontrolhed.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrolhed.Visible = true;
        }
        catch { }
    }

    #endregion

    protected void btnsearch_Click(object sender, EventArgs e)
    {
        ds.Clear();
        ds = dsvalue();
        if (cbAcdYear.Checked == true)
        {
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                loadspreadvalues();
            }
            else
            {
                divspread.Visible = false;
                print.Visible = false;
                lblvalidation1.Text = "";
                imgdiv2.Visible = true;
                lbl_alert.Text = "No Record Found";
            }
        }
        if (cbAcdYear.Checked == false)
        {

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                loadspreadvalues();
            }
            else
            {
                divspread.Visible = false;
                print.Visible = false;
                lblvalidation1.Text = "";
                imgdiv2.Visible = true;
                lbl_alert.Text = "No Record Found";
            }


        }

    }

    protected DataSet dsvalue()
    {
        DataSet dsload = new DataSet();
        try
        {
            #region
            UserbasedRights();
            string batch = getCblSelectedValue(cbl_batch);
            string degcode = getCblSelectedValue(cbl_dept);
            string feecat = getCblSelectedValue(cbl_sem);
            string hedgid = ddlheader.SelectedItem.Value.ToString();
            string ledgid = ddlLedger.SelectedItem.Value.ToString();
            string sem = getCblSelectedValue(cbl_sem);
            string fnlyr = getCblSelectedValue(chklsfyear);
            string courseId = getCblSelectedValue(cbl_degree);
            //string strInclude = getStudCategory();
            if (ddl_collegename.Items.Count > 0)
                collegecode = Convert.ToString(ddl_collegename.SelectedItem.Value);

            int noofconces = 0;
            string deductioncode = "";
            for (int b = 0; b < chklsconcession.Items.Count; b++)
            {
                if (chklsconcession.Items[b].Selected == true)
                {
                    noofconces++;
                    if (deductioncode == "")
                    {
                        deductioncode = "'" + chklsconcession.Items[b].Value.ToString() + "'";
                    }
                    else
                    {
                        deductioncode = deductioncode + ",'" + chklsconcession.Items[b].Value.ToString() + "'";
                    }
                }
            }
            string selqry = string.Empty;
            if (cbAcdYear.Checked)
            {
                selqry = " SELECT r.batch_year,SUM(FeeAmount)as Allot,SUM(DeductAmout) as Concession,SUM(TotalAmount) as Total,SUM(PaidAmount) as Paid,SUM(BalAmount) as Balance,SUM(RefundAmount ) as RefundAmount,a.feecategory FROM FT_FeeAllot A,Registration r WHERE  r.App_No=a.App_No and r.college_code ='" + collegecode + "' and A.FinYearFK in ('" + fnlyr + "') and isnull(istransfer,'0')='0'";

                if (batch != "")
                    selqry = selqry + " and r.Batch_Year in('" + batch + "')";
                if (degcode != "")
                    selqry = selqry + " and r.degree_code in('" + degcode + "')";
                if (hedgid != "")
                    selqry = selqry + " and a.HeaderFK in('" + hedgid + "') ";
                if (ledgid != "")
                    selqry = selqry + " and LedgerFK in('" + ledgid + "') ";
                if (fnlyr != "")
                    selqry = selqry + " and a.FinYearFK in ('" + fnlyr + "') ";

                if (feecat != "")
                    selqry += " and a.feecategory in('" + feecat + "')";
                selqry = selqry + " group by r.batch_year,a.feecategory";

                selqry = selqry + " SELECT r.batch_year,SUM(Debit) as total,a.feecategory,A.FinYearFK FROM FT_FinDailyTransaction A,Registration r WHERE  r.App_No=a.app_no  and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and r.college_code ='" + collegecode + "' and A.FinYearFK in ('" + fnlyr + "') and isnull(paid_Istransfer,'0')='0' and Debit>0"; //AND Admission_Status = 1

                if (batch != "")
                    selqry = selqry + " and r.Batch_Year in ('" + batch + "')";
                if (degcode != "")
                    selqry = selqry + "  and  r.Degree_Code in ('" + degcode + "')";
                if (hedgid != "")
                    selqry = selqry + " and HeaderFK in ('" + hedgid + "')";
                if (ledgid != "")
                    selqry = selqry + " and LedgerFK in('" + ledgid + "')";
                if (feecat != "")
                    selqry += " and a.feecategory in('" + feecat + "')";
                //if (commondist.Trim() != "")
                //{
                //    selqry = selqry + commondist;
                //}

                selqry = selqry + " GROUP BY r.batch_year,a.feecategory,A.FinYearFK";

                selqry = selqry + " SELECT r.batch_year,SUM(DeductAmout) as Concession,DeductReason,a.feecategory FROM FT_FeeAllot A,Registration r WHERE  r.App_No=a.App_No and r.college_code ='" + collegecode + "' and A.FinYearFK in ('" + fnlyr + "')  and isnull(istransfer,'0')='0' and DeductAmout>0 ";

                if (batch != "")
                    selqry = selqry + " and r.Batch_Year in ('" + batch + "')";
                if (degcode != "")
                    selqry = selqry + "  and  r.Degree_Code in ('" + degcode + "')";
                if (hedgid != "")
                    selqry = selqry + " and HeaderFK in ('" + hedgid + "')";
                if (ledgid != "")
                    selqry = selqry + " and LedgerFK in('" + ledgid + "')";
                if (feecat != "")
                    selqry += " and a.feecategory in('" + feecat + "')";
                selqry = selqry + " GROUP BY r.batch_year,DeductReason,a.feecategory";
            }
            if (!cbAcdYear.Checked)
            {
                selqry = " SELECT r.batch_year,SUM(FeeAmount)as Allot,SUM(DeductAmout) as Concession,SUM(TotalAmount) as Total,SUM(PaidAmount) as Paid,SUM(BalAmount) as Balance,SUM(RefundAmount ) as RefundAmount FROM FT_FeeAllot A,Registration r WHERE  r.App_No=a.App_No and r.college_code ='" + collegecode + "' and A.FinYearFK in ('" + fnlyr + "') and isnull(istransfer,'0')='0'";

                if (batch != "")
                    selqry = selqry + " and r.Batch_Year in('" + batch + "')";
                if (degcode != "")
                    selqry = selqry + " and r.degree_code in('" + degcode + "')";
                if (hedgid != "")
                    selqry = selqry + " and a.HeaderFK in('" + hedgid + "') ";
                if (ledgid != "")
                    selqry = selqry + " and LedgerFK in('" + ledgid + "') ";
                if (feecat != "")
                    selqry += " and a.feecategory in('" + feecat + "')";
                selqry = selqry + " group by r.batch_year";

                selqry = selqry + " SELECT r.batch_year,SUM(Debit) as total,A.FinYearFK FROM FT_FinDailyTransaction A,Registration r WHERE  r.App_No=a.app_no  and isnull(IsCanceled,'0')='0' and  ISNULL(IsCollected,0)='1' and r.college_code ='" + collegecode + "' and A.FinYearFK in ('" + fnlyr + "') and isnull(paid_Istransfer,'0')='0' and Debit>0"; //AND Admission_Status = 1

                if (batch != "")
                    selqry = selqry + " and r.Batch_Year in ('" + batch + "')";
                if (degcode != "")
                    selqry = selqry + "  and  r.Degree_Code in ('" + degcode + "')";
                if (hedgid != "")
                    selqry = selqry + " and HeaderFK in ('" + hedgid + "')";
                if (ledgid != "")
                    selqry = selqry + " and LedgerFK in('" + ledgid + "')";
                if (feecat != "")
                    selqry += " and a.feecategory in('" + feecat + "')";
                //if (commondist.Trim() != "")
                //{
                //    selqry = selqry + commondist;
                //}

                selqry = selqry + " GROUP BY r.batch_year,A.FinYearFK";

                selqry = selqry + " SELECT r.batch_year,SUM(DeductAmout) as Concession,DeductReason FROM FT_FeeAllot A,Registration r WHERE  r.App_No=a.App_No and r.college_code ='" + collegecode + "' and A.FinYearFK in ('" + fnlyr + "')  and isnull(istransfer,'0')='0' and DeductAmout>0 ";

                if (batch != "")
                    selqry = selqry + " and r.Batch_Year in ('" + batch + "')";
                if (degcode != "")
                    selqry = selqry + "  and  r.Degree_Code in ('" + degcode + "')";
                if (hedgid != "")
                    selqry = selqry + " and HeaderFK in ('" + hedgid + "')";
                if (ledgid != "")
                    selqry = selqry + " and LedgerFK in('" + ledgid + "')";
                if (feecat != "")
                    selqry += " and a.feecategory in('" + feecat + "')";
                selqry = selqry + " GROUP BY r.batch_year,DeductReason";
            }
            selqry += " select distinct finyearfk from FT_FINYEAR_DETAILED where collegecode='" + collegecode + "' and ACD_BATCH_YEAR in('" + batch + "')";

            dsload.Clear();
            dsload = d2.select_method_wo_parameter(selqry, "Text");


            #endregion

        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collegecode, "ConsolidatedDemandReport.aspx"); 
        }
        return dsload;
    }

    protected void loadspreadvalues()
    {
        try
        {
            #region SpreadDesign


            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnHeader.Rows.Count = 3;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 7;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            string academicYear = ddlAcademic.SelectedItem.Text;
            string ledger = ddlLedger.SelectedItem.Text;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Variation Statement" + " - " + academicYear + " - " + ledger;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[0].Width = 50;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 1, 7);

            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[0].Width = 50;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, 0, 2, 1);

            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].Text = "PART I";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, 1, 2, 1);

            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 2].Text = "AMOUNT";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, 2, 1, 2);

            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 2].Text = "RS";
            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(2, 2, 1, 1);

            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 3].Text = "RS";
            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 3].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 3].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 3].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 1, 1);

            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Columns[4].Width = 50;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, 4, 2, 1);

            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 5].Text = "PART II";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 5].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 5].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 5].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 5].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 5].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, 5, 2, 1);

            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 6].Text = "AMOUNT";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 6].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 6].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 6].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 6].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, 6].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, 6, 1, 1);

            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 6].Text = "RS";
            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 6].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 6].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 6].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 6].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[2, 6].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(2, 7, 1, 1);

            #endregion

            #region Value
            int SNo = 1;
            double TotalAllot = 0;
            double TotalPaid = 0;
            double balance = 0;
            double refund = 0;
            double TotalAmount = 0;
            string finyear = string.Empty;
            double grandtotal = 0;
            double arrearAmt = 0;

            double GrandtotConcession = 0;
            DataTable dtallot = new DataTable();
            DataTable dtpaid = new DataTable();
            DataTable dtconcession = new DataTable();
            if (cbAcdYear.Checked)
            {
                #region Academic Year
                Dictionary<string, string> getAcdYear = new Dictionary<string, string>();
                Dictionary<string, string> currentSem = getCurSem();

                string collegeCode = string.Empty;
                DataSet dsNormal = ds.Copy();
                DataSet dsFinal = new DataSet();
                DataTable dtFinalBind = new DataTable();
                DataTable dtStucount = new DataTable();
                try
                {
                    collegeCode = Convert.ToString(ddl_collegename.SelectedItem.Value);
                    string acdYear = Convert.ToString(ddlAcademic.SelectedItem.Text);
                    getAcdYear = getOldSettings(acdYear);//(acdYear, clgCode);

                    if (getAcdYear.Count > 0)
                    {
                        bool boolDs = false;

                        DataTable dtfinyear = ds.Tables[3].DefaultView.ToTable();

                        foreach (KeyValuePair<string, string> getVal in getAcdYear)
                        {
                            string feeCate = getVal.Value.Replace(",", "','");
                            string degcourseid = Convert.ToString(getCblSelectedValue(cbl_degree));

                            ds.Tables[0].DefaultView.RowFilter = " batch_year='" + getVal.Key.Split('$')[1] + "' and feecategory in('" + feeCate + "')";
                            dtallot = ds.Tables[0].DefaultView.ToTable();
                            ds.Tables[1].DefaultView.RowFilter = " batch_year='" + getVal.Key.Split('$')[1] + "' and feecategory in('" + feeCate + "')";
                            dtpaid = ds.Tables[1].DefaultView.ToTable();
                            ds.Tables[2].DefaultView.RowFilter = " batch_year='" + getVal.Key.Split('$')[1] + "' and feecategory in('" + feeCate + "')";
                            dtconcession = ds.Tables[2].DefaultView.ToTable();

                            if (!boolDs)
                            {
                                dsFinal.Reset();
                                dsFinal.Tables.Add(dtallot);
                                dsFinal.Tables.Add(dtpaid);
                                dsFinal.Tables.Add(dtconcession);
                                boolDs = true;
                            }
                            else
                            {
                                dsFinal.Merge(dtallot);
                                dsFinal.Merge(dtpaid);
                                dsFinal.Merge(dtconcession);
                                dsFinal.Merge(dtfinyear);
                            }
                        }
                    }
                    if (dsFinal.Tables.Count > 0 && dsFinal.Tables[0].Rows.Count > 0)
                    {
                        ds.Reset();
                        ds = dsFinal.Copy();
                    }
                }
                catch
                {
                    ds.Reset();
                    ds = dsNormal.Copy();
                }
                #endregion
            }
            for (int i = 0; i < chklsconcession.Items.Count; i++)
            {
                FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 2;

                if (i == 0)
                {
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(SNo);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "Fees due at standard rate(as per fee return)";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;

                    if (cbAcdYear.Checked)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int all = 0; all < ds.Tables[0].Rows.Count; all++)
                            {
                                string allot = string.Empty;
                                allot = Convert.ToString(ds.Tables[0].Rows[all]["Allot"]);
                                TotalAllot += Convert.ToDouble(allot);
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(TotalAllot);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                    if (!cbAcdYear.Checked)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            for (int all = 0; all < ds.Tables[0].Rows.Count; all++)
                            {
                                string allot = string.Empty;
                                allot = Convert.ToString(ds.Tables[0].Rows[all]["Allot"]);
                                TotalAllot += Convert.ToDouble(allot);
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(TotalAllot);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                }
                else if (i == 1)
                {
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(SNo);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "Actual fee collection";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    if (!cbAcdYear.Checked)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                            {
                                string paid = string.Empty;
                                paid = Convert.ToString(ds.Tables[1].Rows[j]["total"]);
                                TotalPaid += Convert.ToDouble(paid);
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(TotalPaid);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                    if (cbAcdYear.Checked)
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                            {
                                string paid = string.Empty;
                                paid = Convert.ToString(ds.Tables[1].Rows[j]["total"]);
                                TotalPaid += Convert.ToDouble(paid);
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(TotalPaid);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                }
                else if (i == 2)
                {
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(SNo);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "a) Less amount of  arrears of previous year collected";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;

                    if (!cbAcdYear.Checked)
                    {
                        if (ds.Tables[3].Rows.Count > 0)
                        {
                            for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                            {
                                finyear = Convert.ToString(ds.Tables[3].Rows[0]["finyearfk"]);
                                string finyearfk = Convert.ToString(ds.Tables[1].Rows[j]["FinYearFK"]);
                                if (finyear == finyearfk)
                                {
                                }
                                else
                                {
                                    string arrear = string.Empty;
                                    arrearAmt = Convert.ToDouble(ds.Tables[1].Rows[j]["total"]);
                                    arrearAmt += Convert.ToDouble(arrear);
                                }
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(arrearAmt);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;

                        }
                    }
                    if (cbAcdYear.Checked)
                    {
                        if (ds.Tables[3].Rows.Count > 0)
                        {
                            for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                            {
                                finyear = Convert.ToString(ds.Tables[3].Rows[0]["finyearfk"]);
                                string finyearfk = Convert.ToString(ds.Tables[1].Rows[j]["FinYearFK"]);
                                if (finyear == finyearfk)
                                {
                                }
                                else
                                {
                                    string arrear = string.Empty;
                                    arrearAmt = Convert.ToDouble(ds.Tables[1].Rows[j]["total"]);
                                    arrearAmt += Convert.ToDouble(arrear);
                                }
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(arrearAmt);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;

                        }
                    }

                }
                else if (i == 3)
                {
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "Balance";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    balance = TotalPaid - arrearAmt;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(balance);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                }
                else if (i == 4)
                {
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "b) Less Amount refunded";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;

                    if (!cbAcdYear.Checked)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            for (int all = 0; all < ds.Tables[0].Rows.Count; all++)
                            {
                                string refunamt = string.Empty;
                                refunamt = Convert.ToString(ds.Tables[0].Rows[all]["refundamount"]);
                                refund += Convert.ToDouble(refunamt);
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(refund);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;

                            TotalAmount = balance - refund;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(TotalAmount);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                    if (cbAcdYear.Checked)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            for (int all = 0; all < ds.Tables[0].Rows.Count; all++)
                            {
                                string refunamt = string.Empty;
                                refunamt = Convert.ToString(ds.Tables[0].Rows[all]["refundamount"]);
                                refund += Convert.ToDouble(refunamt);
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(refund);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;

                            TotalAmount = balance - refund;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(TotalAmount);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                }
                string concessionReason = Convert.ToString(chklsconcession.Items[i].Text);
                string deductVal = Convert.ToString(chklsconcession.Items[i].Value);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(SNo);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(concessionReason);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                SNo++;

                if (!cbAcdYear.Checked)
                {
                    ds.Tables[2].DefaultView.RowFilter = "deductreason= '" + deductVal + "'";
                    DataTable dtConcessionBind = ds.Tables[2].DefaultView.ToTable();
                    double TotalConcession = 0;
                    if (dtConcessionBind.Rows.Count > 0)
                    {
                        for (int j = 0; j < dtConcessionBind.Rows.Count; j++)
                        {
                            string concession = string.Empty;
                            concession = Convert.ToString(dtConcessionBind.Rows[j]["concession"]);
                            TotalConcession += Convert.ToDouble(concession);
                            GrandtotConcession += Convert.ToDouble(concession);

                        }
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(TotalConcession);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
                if (cbAcdYear.Checked)
                {
                    ds.Tables[2].DefaultView.RowFilter = "deductreason= '" + deductVal + "'";
                    DataTable dtConcessionBind = ds.Tables[2].DefaultView.ToTable();
                    double TotalConcession = 0;
                    if (dtConcessionBind.Rows.Count > 0)
                    {
                        for (int j = 0; j < dtConcessionBind.Rows.Count; j++)
                        {
                            string concession = string.Empty;
                            concession = Convert.ToString(dtConcessionBind.Rows[j]["concession"]);
                            TotalConcession += Convert.ToDouble(concession);
                            GrandtotConcession += Convert.ToDouble(concession);

                        }
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(TotalConcession);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                    }
                }

            }
            FpSpread1.Sheets[0].RowCount++;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
            FpSpread1.Sheets[0].SpanModel.Add(0, 0, 1, 2);
            grandtotal = TotalAllot - TotalAmount;

            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(grandtotal);
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(GrandtotConcession);
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
            //FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
            Hashtable totAllot = new Hashtable();



            #endregion
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            FpSpread1.SaveChanges();
            divspread.Visible = true;
            print.Visible = true;
            imgdiv2.Visible = false;
            lbl_alert.Text = "";
            lblvalidation1.Text = "";
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collegecode, "ConsolidatedDemandReport.aspx"); 
        }

    }

}