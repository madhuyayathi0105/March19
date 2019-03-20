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

public partial class FinanceMod_ConsolidatedDemandReport : System.Web.UI.Page
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
            if (Session["collegecode"] == null)
            {
                Response.Redirect("Default.aspx");
            }
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
                //bindsec();
                loadheaderandledger();
                ledgerload();
                //loadpaid();
                loadfinanceyear();
                loadconcession();
                getAcademicYear();

                //txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //txt_fromdate.Attributes.Add("readonly", "readonly");
                //txt_todate.Attributes.Add("readonly", "readonly");
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
            string clgvalue = ddl_collegename.SelectedItem.Value.ToString();
            cblledger.Items.Clear();

            string hed1 = ddlheader.SelectedItem.Text.ToString();
            string hed = ddlheader.SelectedItem.Value.ToString();
            //for (int i = 0; i < ddlheader.Items.Count; i++)
            //{
            //    if (ddlheader.Items[i].Selected == true)
            //    {
            //        if (hed == "")
            //        {
            //            hed = ddlheader.Items[i].Value.ToString();
            //        }
            //        else
            //        {
            //            hed = hed + "','" + "" + ddlheader.Items[i].Value.ToString() + "";
            //        }
            //    }
            //}


            string query1 = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='0' and L.CollegeCode = " + clgvalue + "  and L.HeaderFK in('" + hed + "')  order by isnull(l.priority,1000), l.ledgerName asc ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query1, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblledger.DataSource = ds;
                cblledger.DataTextField = "LedgerName";
                cblledger.DataValueField = "LedgerPK";
                cblledger.DataBind();
                for (int i = 0; i < cblledger.Items.Count; i++)
                {
                    cblledger.Items[i].Selected = true;
                }
                txtledger.Text = "Ledger(" + cblledger.Items.Count + ")";
                cbledger.Checked = true; ;

            }
            else
            {
                for (int i = 0; i < cblledger.Items.Count; i++)
                {
                    cblledger.Items[i].Selected = false;
                }
                txtledger.Text = "--Select--";
                cbledger.Checked = false; ;
            }

        }
        catch
        {
        }
    }
    public void cbledger_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cbledger, cblledger, txtledger, "Ledger", "--Select--");

        }
        catch (Exception ex)
        { }
    }
    public void cblledger_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cbledger, cblledger, txtledger, "Ledger", "--Select--");
        }
        catch (Exception ex)
        { }
    }
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
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("Prolong Absent", "5"));
            //if (cblinclude.Items.Count > 0)
            //{
            //    for (int i = 0; i < cblinclude.Items.Count; i++)
            //    {
            //        cblinclude.Items[i].Selected = true;
            //    }
            //    cbinclude.Checked = true;
            //    txtinclude.Text = "Student(" + cblinclude.Items.Count + ")";
            //}
        }
        catch { }
    }


    protected void cbinclude_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cbinclude, cblinclude, txtinclude, "Student", "--Select--");
    }
    protected void cblinclude_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbinclude, cblinclude, txtinclude, "Student", "--Select--");

    }


    #endregion

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
            divspread.Visible = false;
            print.Visible = false;
            lblvalidation1.Text = "";
            imgdiv2.Visible = true;
            lbl_alert.Text = "Please Select the academic year";
        }
    }

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
            string pro = "";
            if (cblinclude.Items.Count > 0)
            {
                for (int i = 0; i < cblinclude.Items.Count; i++)
                {
                    if (cblinclude.Items[i].Selected == true)
                    {
                        if (cblinclude.Items[i].Value == "1")
                            cc = " r.cc=1  ";//and  r.ProlongAbsent=0
                        if (cblinclude.Items[i].Value == "2")
                            debar = " r.Exam_Flag like '%debar'";
                        if (cblinclude.Items[i].Value == "3")
                            disc = "r.DelFlag=1 and  isnull(r.ProlongAbsent,'0')=0 ";
                        if (cblinclude.Items[i].Value == "4")
                            cancel = "  r.DelFlag=2";
                        if (cblinclude.Items[i].Value == "5")
                            pro = " r.ProlongAbsent=1 and r.DelFlag=1";
                    }
                }
            }
            if (checkdicon.Checked)
            {
                if (cc != "")
                    strInclude = "(r.cc=1)";// and  r.ProlongAbsent=0
                if (debar != "")
                {
                    if (strInclude != "")
                    {
                        //strInclude = strInclude.TrimEnd(')');
                        strInclude += " or ";
                        // strInclude += "(";
                        strInclude += "  r.Exam_Flag like '%debar')";
                    }
                    else
                    {
                        strInclude += "(";
                        strInclude += "  r.Exam_Flag like '%debar')";
                    }
                }
                if (disc != "")
                {
                    if (strInclude != "")
                    {
                        strInclude = strInclude.TrimEnd(')');
                        strInclude += " or ";
                        strInclude += " (r.DelFlag=1 and isnull(r.ProlongAbsent,'0')=0)";
                    }
                    else
                    {
                        strInclude += "(";
                        strInclude += " r.DelFlag=1 and  isnull(r.ProlongAbsent,'0')=0)";
                    }
                }
                if (cancel != "")
                {
                    if (strInclude != "")
                    {
                        // strInclude = strInclude.TrimEnd(')');
                        strInclude += " or";
                        strInclude += "  (r.DelFlag=2)";
                    }
                    else
                    {
                        strInclude += "(";
                        strInclude += "  r.DelFlag=2)";
                    }
                }
                if (pro != "")
                {
                    if (strInclude != "")
                    {
                        // strInclude = strInclude.TrimEnd(')');
                        strInclude += " or";
                        strInclude += " (r.ProlongAbsent=1 and r.DelFlag=1)";
                    }
                    else
                    {
                        strInclude += "(";
                        strInclude += "r.ProlongAbsent=1 and r.DelFlag=1)";
                    }
                }
                if (strInclude != "")

                    strInclude = "and (" + strInclude + ")";
            }
            //if (!checkdicon.Checked)
            //{
            //    if (cc != "" && debar == "" && disc == "" && cancel == "")
            //        strInclude = " and (" + cc + " or r.cc=0)  and r.Exam_Flag<>'debar' and r.DelFlag=0";
            //    if (cc == "" && debar != "" && disc == "" && cancel == "")
            //        strInclude = " and r.cc=0  and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";
            //    if (cc == "" && debar == "" && disc != "" && cancel == "")
            //        strInclude = " and r.cc=0  and r.Exam_Flag<>'debar' and (" + disc + " or r.DelFlag=0)";
            //    if (cc == "" && debar == "" && disc == "" && cancel != "")
            //        strInclude = " and r.cc=0  and r.Exam_Flag<>'debar' and (" + cancel + " or r.DelFlag=0)";
            //    //2
            //    if (cc != "" && debar != "" && disc == "" && cancel == "")
            //        strInclude = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";
            //    if (cc != "" && debar == "" && disc != "" && cancel == "")
            //        strInclude = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar' and (" + disc + " or r.DelFlag=0)";
            //    if (cc != "" && debar == "" && disc == "" && cancel != "")
            //        strInclude = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar' and (" + cancel + " or r.DelFlag=0)";
            //    //
            //    if (cc == "" && debar != "" && disc != "" && cancel == "")
            //        strInclude = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar') and (" + disc + " or r.DelFlag=0)";
            //    if (cc == "" && debar != "" && disc == "" && cancel != "")
            //        strInclude = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar') and (" + cancel + " or r.DelFlag=0)";
            //    //
            //    if (cc == "" && debar == "" && disc != "" && cancel != "")
            //        strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and (" + disc + " or " + cancel + " or r.DelFlag=0)";
            //    //3
            //    if (cc != "" && debar != "" && disc != "" && cancel == "")
            //        strInclude = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and (" + disc + " or r.DelFlag=0)";
            //    if (cc != "" && debar == "" && disc != "" && cancel != "")
            //        strInclude = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar' and (" + disc + " or " + cancel + " or r.DelFlag=0)";
            //    if (cc != "" && debar != "" && disc == "" && cancel != "")
            //        strInclude = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and (" + cancel + " or r.DelFlag=0)";
            //    if (cc == "" && debar != "" && disc != "" && cancel != "")
            //        strInclude = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar') and (" + disc + " or " + cancel + " or r.DelFlag=0)";
            //    if (cc == "" && debar == "" && disc == "" && cancel == "")
            //        strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and  r.DelFlag=0";
            //    if (cc != "" && debar != "" && disc != "" && cancel != "")
            //        strInclude = "";
            //}
            else
            {

                strInclude = " and r.Exam_Flag<>'debar' and  r.DelFlag=0 and isnull(r.ProlongAbsent,'0')=0";//and r.cc=0

                //if (cc != "" && debar == "" && disc == "" && cancel == "")
                //    strInclude = " and " + cc + "";
                //if (cc == "" && debar != "" && disc == "" && cancel == "")
                //    strInclude = " and " + debar + "";
                //if (cc == "" && debar == "" && disc != "" && cancel == "")
                //    strInclude = " and " + disc + "";
                //if (cc == "" && debar == "" && disc == "" && cancel != "")
                //    strInclude = " and " + cancel + "";
                ////2
                //if (cc != "" && debar != "" && disc == "" && cancel == "")
                //    strInclude = " and( " + cc + " or " + debar + ")";
                //if (cc != "" && debar == "" && disc != "" && cancel == "")
                //    strInclude = " and (" + cc + " or " + disc + ")";
                //if (cc != "" && debar == "" && disc == "" && cancel != "")
                //    strInclude = " and (" + cc + " or " + cancel + ")";
                ////
                //if (cc == "" && debar != "" && disc != "" && cancel == "")
                //    strInclude = " and (" + debar + " or " + disc + ")";
                //if (cc == "" && debar != "" && disc == "" && cancel != "")
                //    strInclude = " and (" + debar + " or " + cancel + ")";
                ////
                //if (cc == "" && debar == "" && disc != "" && cancel != "")
                //    strInclude = " and (" + disc + " or " + cancel + ")";
                ////3
                //if (cc != "" && debar != "" && disc != "" && cancel == "")
                //    strInclude = " and (" + cc + " or " + debar + " or " + disc + ")";
                //if (cc != "" && debar == "" && disc != "" && cancel != "")
                //    strInclude = " and (" + cc + " or " + disc + " or " + cancel + ")";
                //if (cc != "" && debar != "" && disc == "" && cancel != "")
                //    strInclude = " and (" + cc + " or " + debar + " or " + cancel + ")";
                //if (cc == "" && debar != "" && disc != "" && cancel != "")
                //    strInclude = " and (" + debar + " or " + disc + " or " + cancel + ")";
                //if (cc == "" && debar == "" && disc == "" && cancel == "")
                //    strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and  r.DelFlag=0";
                //if (cc != "" && debar != "" && disc != "" && cancel != "")
                //    strInclude = " and (" + cc + " or " + debar + " or " + disc + " or " + cancel + ")";
            }
            #endregion
        }
        catch { }
        return strInclude;
    }

    protected DataSet dsvalue()
    {
        DataSet dsload = new DataSet();
        try
        {
            #region Detailed done by saranya
            if (rbDetailed.Checked == true)
            {
                UserbasedRights();
                string batch = getCblSelectedValue(cbl_batch);
                string degcode = getCblSelectedValue(cbl_dept);
                string feecat = getCblSelectedValue(cbl_sem);
                string hedgid = ddlheader.SelectedItem.Value.ToString();
                string ledgid = getCblSelectedValue(cblledger);
                string sem = getCblSelectedValue(cbl_sem);
                string fnlyr = getCblSelectedValue(chklsfyear);
                string strInclude = getStudCategory();
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

                string SelectQ = "";

                #region Without Date
                SelectQ = "select Count(distinct f.App_No) as totcount,max(f.FeeAmount) as amount,r.degree_code,r.Batch_Year,f.FeeCategory,r.college_code,t.textval,c.Course_Id from FT_FeeAllot f,Registration r,TextValTable t,course c,Degree d where f.App_No=r.App_No and t.TextCode=f.feecategory and isnull(f.deductamout,'0')=0 and d.Degree_Code=r.degree_code and d.Course_Id=c.Course_Id  and r.college_code='" + collegecode + "'  and isnull(BalAmount,'0')=0 and ISNULL(DeductReason,'0')=0 " + strInclude + " ";
                if (batch != "")
                {
                    SelectQ += " and r.Batch_Year in('" + batch + "')";
                }
                if (degcode != "")
                {
                    SelectQ += " and r.degree_code in('" + degcode + "')";
                }
                if (feecat != "")
                {
                    SelectQ += " and f.FeeCategory in('" + feecat + "')";
                }
                if (hedgid != "")
                {
                    SelectQ += "  and f.HeaderFK in('" + hedgid + "')";
                }
                if (ledgid != "")
                {
                    SelectQ += " and f.LedgerFK in('" + ledgid + "')";
                }
                if (fnlyr != "")
                {
                    SelectQ += " and f.FinYearFK in('" + fnlyr + "')";
                }
                SelectQ += " group by r.degree_code,r.Batch_Year,f.FeeCategory,r.college_code,t.textval,c.Course_Id order by r.Batch_Year desc";


                SelectQ += " select Count(distinct f.App_No) as totcount,r.degree_code,r.Batch_Year,f.FeeCategory,r.college_code,DeductReason,t.textval,c.Course_Id from FT_FeeAllot f,Registration r,TextValTable t,course c,Degree d where t.TextCode=f.feecategory and d.Degree_Code=r.degree_code and d.Course_Id=c.Course_Id and f.App_No=r.App_No  and r.college_code='" + collegecode + "'  and isnull(BalAmount,'0')=0  and DeductAmout>0 and f.DeductReason in(" + deductioncode + ") " + strInclude + "";
                if (batch != "")
                {
                    SelectQ += " and r.Batch_Year in('" + batch + "')";
                }
                if (degcode != "")
                {
                    SelectQ += " and r.degree_code in('" + degcode + "')";
                }
                if (feecat != "")
                {
                    SelectQ += " and f.FeeCategory in('" + feecat + "')";
                }
                if (hedgid != "")
                {
                    SelectQ += "  and f.HeaderFK in('" + hedgid + "')";
                }
                if (ledgid != "")
                {
                    SelectQ += " and f.LedgerFK in('" + ledgid + "')";
                }
                if (fnlyr != "")
                {
                    SelectQ += " and f.FinYearFK in('" + fnlyr + "')";
                }
                SelectQ += " group by r.degree_code,r.Batch_Year,f.FeeCategory,r.college_code,DeductReason,t.textval,c.Course_Id order by r.Batch_Year desc";

                #endregion
                SelectQ = SelectQ + " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code ='" + collegecode + "'";

                SelectQ += "  select (c.Course_Name+'-'+dt.Dept_Name) as Depatname,d.Degree_Code,c.Course_Id from Degree d,Course c,Department dt where d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and d.degree_code in('" + degcode + "') order by c.Course_Id ";

                SelectQ = SelectQ + " select distinct r.college_code,r.batch_year from registration r,collinfo c where r.college_code=c.college_code and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degcode + "') order by r.Batch_Year desc ";

                SelectQ = SelectQ + " select distinct c.Course_Id from Degree d,Course c,Department dt where d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and d.degree_code in('" + degcode + "') order by c.Course_Id ";
                dsload.Clear();

                dsload = d2.select_method_wo_parameter(SelectQ, "Text");
            }

            #endregion

            #region Cummulative done by abarna
            if (rbCummulative.Checked == true)
            {
                UserbasedRights();
                string batch = getCblSelectedValue(cbl_batch);
                string degcode = getCblSelectedValue(cbl_dept);
                string feecat = getCblSelectedValue(cbl_sem);
                string hedgid = ddlheader.SelectedItem.Value.ToString();
                string ledgid = getCblSelectedValue(cblledger);
                string sem = getCblSelectedValue(cbl_sem);
                string fnlyr = getCblSelectedValue(chklsfyear);
                string strInclude = getStudCategory();
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

                string SelectQ = "";

                #region Without Date
                SelectQ = "select Count(distinct f.App_No) as totcount,max(f.FeeAmount) as amount,r.Batch_Year,f.FeeCategory,r.college_code,t.textval,c.Course_Id from FT_FeeAllot f,Registration r,TextValTable t,course c,Degree d where f.App_No=r.App_No and t.TextCode=f.feecategory and isnull(f.deductamout,'0')=0 and d.Degree_Code=r.degree_code and d.Course_Id=c.Course_Id  and r.college_code='" + collegecode + "'  and isnull(BalAmount,'0')=0 and ISNULL(DeductReason,'0')=0 " + strInclude + " ";
                if (batch != "")
                {
                    SelectQ += " and r.Batch_Year in('" + batch + "')";
                }
                if (degcode != "")
                {
                    SelectQ += " and r.degree_code in('" + degcode + "')";
                }
                if (feecat != "")
                {
                    SelectQ += " and f.FeeCategory in('" + feecat + "')";
                }
                if (hedgid != "")
                {
                    SelectQ += "  and f.HeaderFK in('" + hedgid + "')";
                }
                if (ledgid != "")
                {
                    SelectQ += " and f.LedgerFK in('" + ledgid + "')";
                }
                if (fnlyr != "")
                {
                    SelectQ += " and f.FinYearFK in('" + fnlyr + "')";
                }
                SelectQ += " group by r.Batch_Year,f.FeeCategory,r.college_code,t.textval,c.Course_Id order by r.Batch_Year desc";


                SelectQ += " select Count(distinct f.App_No) as totcount,r.Batch_Year,f.FeeCategory,r.college_code,DeductReason,t.textval,c.Course_Id from FT_FeeAllot f,Registration r,TextValTable t,course c,Degree d where t.TextCode=f.feecategory and d.Degree_Code=r.degree_code and d.Course_Id=c.Course_Id and f.App_No=r.App_No  and r.college_code='" + collegecode + "'  and isnull(BalAmount,'0')=0  and DeductAmout>0 and f.DeductReason in(" + deductioncode + ") " + strInclude + "";
                if (batch != "")
                {
                    SelectQ += " and r.Batch_Year in('" + batch + "')";
                }
                if (degcode != "")
                {
                    SelectQ += " and r.degree_code in('" + degcode + "')";
                }
                if (feecat != "")
                {
                    SelectQ += " and f.FeeCategory in('" + feecat + "')";
                }
                if (hedgid != "")
                {
                    SelectQ += "  and f.HeaderFK in('" + hedgid + "')";
                }
                if (ledgid != "")
                {
                    SelectQ += " and f.LedgerFK in('" + ledgid + "')";
                }
                if (fnlyr != "")
                {
                    SelectQ += " and f.FinYearFK in('" + fnlyr + "')";
                }
                SelectQ += " group by r.Batch_Year,f.FeeCategory,r.college_code,DeductReason,t.textval,c.Course_Id order by r.Batch_Year desc";

                #endregion
                SelectQ = SelectQ + " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code ='" + collegecode + "'";

                SelectQ += "  select distinct c.Course_Name as Depatname,c.Course_Id from Degree d,Course c,Department dt where d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and d.degree_code in('" + degcode + "') order by c.Course_Id";

                SelectQ = SelectQ + " select distinct r.college_code,r.batch_year from registration r,collinfo c where r.college_code=c.college_code and r.college_code in('" + collegecode + "') and r.batch_year in('" + batch + "') and r.degree_code in('" + degcode + "') order by r.Batch_Year desc ";

                SelectQ = SelectQ + " select distinct c.Course_Id from Degree d,Course c,Department dt where d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and d.degree_code in('" + degcode + "') order by c.Course_Id ";
                dsload.Clear();

                dsload = d2.select_method_wo_parameter(SelectQ, "Text");
            }
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
            #region Detailed done by saranya
            if (rbDetailed.Checked == true)
            {
                UserbasedRights();
                DataView dvWithoutCon = new DataView();
                DataView dvWithCon = new DataView();
                DataView dvdegreeCode = new DataView();
                DataView degreename = new DataView();
                Hashtable grandtotal = new Hashtable();
                string clgCode = string.Empty;
                string semester = string.Empty;
                string headercode = string.Empty;
                Hashtable newhash = new Hashtable();
                Hashtable newhashConAmt = new Hashtable();

                #region design

                int semCount = 0;
                int ConcessionCount = 0;
                // int countval = 0;
                string concode = string.Empty;

                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    if (cbl_sem.Items[i].Selected)
                        semCount++;
                }
                for (int i = 0; i < chklsconcession.Items.Count; i++)
                {
                    if (chklsconcession.Items[i].Selected)
                        ConcessionCount++;
                }
                //int value = 0;
                Hashtable htledg = new Hashtable();
                Hashtable hthedg = new Hashtable();
                DataView dvhed = new DataView();

                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnHeader.Rows.Count = 2;
                FpSpread1.Sheets[0].ColumnCount = 0;
                FpSpread1.CommandBar.Visible = false;
                FpSpread1.Sheets[0].AutoPostBack = true;
                FpSpread1.Sheets[0].RowHeader.Visible = false;
                FpSpread1.Sheets[0].ColumnCount = 2;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = lbldept.Text;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);



                //if (ddlheader.Items.Count > 0)
                //{


                //for (int hed = 0; hed < ddlheader.Items.Count; hed++)
                //{
                //if (!ddlheader.Items[hed].Selected)
                //    continue;
                string headerName = ddlheader.SelectedItem.Text;
                FpSpread1.Sheets[0].ColumnHeader.Columns.Count++;
                string academicYear = ddlAcademic.SelectedItem.Text;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = headerName + " " + academicYear;
                //   htledg.Add(Convert.ToString(ddlheader.Items[hed].Value), Convert.ToString(FpSpread1.Sheets[0].ColumnCount - 1));
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 1, 2);

                if (chklsconcession.Items.Count > 0)
                {
                    //FpSpread1.Sheets[0].ColumnHeader.Rows.Count++;
                    //count = FpSpread1.Sheets[0].ColumnCount - 1;
                    //FpSpread1.Sheets[0].ColumnHeader.Columns.Count++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Paid Without Concession";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = "1";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;

                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Fees";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = "2";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;

                    for (int s = 0; s < chklsconcession.Items.Count; s++)
                    {
                        if (chklsconcession.Items[s].Selected == true)
                        {
                            //countval++;
                            FpSpread1.Sheets[0].ColumnCount++;
                            newhash.Add(Convert.ToString(chklsconcession.Items[s].Value), FpSpread1.Sheets[0].ColumnCount - 1);
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(chklsconcession.Items[s].Text);
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(chklsconcession.Items[s].Value);
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = true;

                            FpSpread1.Sheets[0].ColumnCount++;
                            newhashConAmt.Add(Convert.ToString(chklsconcession.Items[s].Value), FpSpread1.Sheets[0].ColumnCount - 1);
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(chklsconcession.Items[s].Text) + " " + "Amt";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(chklsconcession.Items[s].Value);
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = true;
                        }
                    }
                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Strength";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;

                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Received";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Demand";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                }
                #endregion

                #region new value

                double StudentCount = 0;
                double TotCntSemWise = 0;
                double TotCnt = 0;
                //double ConCnt = 0;
                int Count_Val = 0;
                string textval = string.Empty;
                string Course_id = string.Empty;
                string currentCourseID = string.Empty;
                string preCourseID = string.Empty;
                double DeptWiseTot = 0;
                Hashtable degreeWiseTotal = new Hashtable();
                Hashtable httotal = new Hashtable();
                int sno = 0;
                Dictionary<string, string> currentSem = getCurSem();
                DataView Dview = new DataView();
                string YearWise = string.Empty;

                #region Without Academic Year

                if (!cbAcdYear.Checked)
                {
                    for (int year = 0; year < cbl_batch.Items.Count; year++)
                    {
                        if (cbl_batch.Items[year].Selected)
                        {
                            ds.Tables[0].DefaultView.RowFilter = "batch_year='" + cbl_batch.Items[year].Value + "'";
                            DataView dvyr = ds.Tables[0].DefaultView;
                            string collegeCode = Convert.ToString(ddl_collegename.SelectedItem.Value);
                            ds.Tables[0].DefaultView.RowFilter = " college_Code='" + collegeCode + "'";
                            DataTable dtbatch = ds.Tables[0].DefaultView.ToTable();
                            string curSem = string.Empty;
                            string curSemCode = string.Empty;
                            int colnew = 0;
                            double totcount = 0;
                            string batch = Convert.ToString(dtbatch.Rows[year]["batch_year"]);

                            //if (!cbAcdYear.Checked)
                            //{
                            //    string name = string.Empty;
                            //    string feecategory = string.Empty;
                            //    string yearVal = string.Empty;
                            //    DataSet dtTermWise = new DataSet();
                            //    string YearWiseFeecat = "";
                            //    string TermWise = string.Empty;
                            //    string TermWiseFeeCat = "";
                            //    string CurSem=" select  distinct Current_Semester from Registration where Batch_Year in('"+batch+"')";
                            //    DataSet sem = d2.select_method_wo_parameter(CurSem, "Text");

                            //    for (int i = 1; i <= sem.Tables[0].Rows.Count; i++)
                            //    {
                            //        string s = i + " Semester";
                            //        name = d2.GetFunction("select textcode from textvaltable t,registration r where textval='" + s + "' and TextCriteria='feeca'");

                            //        // string feecatval += "'" + "," + "'" + name.Value.ToString();

                            //        if (feecategory == "")
                            //            feecategory = name;
                            //        else

                            //            feecategory += "'" + "," + "'" + name;
                            //        //=======Added by saranya on 18/12/2017==============//
                            //        switch (i)
                            //        {
                            //            case 1:
                            //            case 2:
                            //                yearVal = "1 Year";
                            //                break;

                            //            case 3:
                            //            case 4:
                            //                yearVal = "2 Year";
                            //                break;
                            //            case 5:
                            //            case 6:
                            //                yearVal = "3 Year";
                            //                break;
                            //            case 7:
                            //            case 8:
                            //                yearVal = "4 Year";
                            //                break;
                            //        }
                            //        YearWiseFeecat = d2.GetFunction("select distinct textcode from textvaltable where textval in('" + yearVal + "') and TextCriteria='feeca' ");

                            //        if (!YearWise.Contains(yearVal))
                            //        {
                            //            if (YearWise == "")
                            //                YearWise = YearWiseFeecat;
                            //            else

                            //                YearWise += "'" + "," + "'" + YearWiseFeecat;

                            //        }

                            //    }

                            //    string strTerm = getCblSelectedTerm_Text(cbl_sem);

                            //    dtTermWise = d2.select_method_wo_parameter("select distinct textcode from textvaltable where textval in('" + strTerm + "')  and TextCriteria='feeca'", "text");

                            //    for (int term = 0; term < dtTermWise.Tables[0].Rows.Count; term++)
                            //    {

                            //        TermWise = dtTermWise.Tables[0].Rows[term]["textcode"].ToString();
                            //        //sbTermValue = sbTermValue + "','" + abc;

                            //        if (!TermWiseFeeCat.Contains(TermWise))
                            //        {
                            //            if (TermWiseFeeCat == "")
                            //                TermWiseFeeCat = TermWise;
                            //            else

                            //                TermWiseFeeCat += "'" + "," + "'" + TermWise;
                            //        }
                            //    }

                            //    curSemCode = " '" + feecategory + "','" + YearWise + "','" + TermWiseFeeCat + "'";

                            //}

                            ds.Tables[0].DefaultView.RowFilter = "batch_year='" + cbl_batch.Items[year].Value + "' ";//and feecategory in(" + curSemCode + ")
                            DataTable dtbatchyear = ds.Tables[0].DefaultView.ToTable();
                            for (int deg = 0; deg < dtbatchyear.Rows.Count; deg++)
                            {
                                string deg_code = Convert.ToString(dtbatchyear.Rows[deg]["degree_code"].ToString());
                                ds.Tables[0].DefaultView.RowFilter = "degree_code='" + deg_code + "' and batch_year='" + cbl_batch.Items[year].Value + "' ";//and feecategory in(" + curSemCode + ")
                                DataView dvSem = ds.Tables[0].DefaultView;
                                double conAmt = 0;
                                double totConAmt = 0;
                                double demandAmt = 0;

                                //=====================For printing the degreewise total===========================//
                                currentCourseID = Convert.ToString(ds.Tables[3].Rows[deg]["Course_Id"].ToString());
                                if (currentCourseID != preCourseID && !string.IsNullOrEmpty(preCourseID))
                                {
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
                                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.YellowGreen;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 2);

                                    for (int Count = 0; Count < FpSpread1.Sheets[0].ColumnHeader.Columns.Count; Count++)
                                    {
                                        int header = Count + 2;
                                        if (degreeWiseTotal.Contains(header))
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, header].Text = Convert.ToString(degreeWiseTotal[header]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, header].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                    }
                                    degreeWiseTotal.Clear();
                                }
                                preCourseID = Convert.ToString(ds.Tables[3].Rows[deg]["Course_Id"].ToString());
                                //====================================================================================//

                                if (dvSem.Count > 0)
                                {
                                    sno++;
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                }
                                ds.Tables[3].DefaultView.RowFilter = "Degree_code='" + deg_code + "'";
                                Dview = ds.Tables[3].DefaultView;

                                if (Dview.Count > 0)
                                {
                                    if (dvSem.Count > 0)
                                    {
                                        string semCat = Convert.ToString(dvSem[0]["feecategory"]); ;
                                        string yearval = " select textval from textvaltable where TextCode in('" + semCat + "')";
                                        DataSet cursemester = d2.select_method_wo_parameter(yearval, "Text");
                                        for (int i = 0; i < cursemester.Tables[0].Rows.Count; i++)
                                        {
                                            string cur_year = Convert.ToString(cursemester.Tables[0].Rows[i]["textval"].ToString());
                                            string curYear = cur_year.Split(' ')[0];
                                            int yearVal = Convert.ToInt32(curYear);
                                            switch (yearVal)
                                            {
                                                case 1:
                                                case 2:
                                                    YearWise = "1 Year";
                                                    break;
                                                case 3:
                                                case 4:
                                                    YearWise = "2 Year";
                                                    break;
                                                case 5:
                                                case 6:
                                                    YearWise = "3 Year";
                                                    break;
                                                case 7:
                                                case 8:
                                                    YearWise = "4 Year";
                                                    break;
                                            }
                                        }
                                        string Degreename = Convert.ToString(Dview[0]["depatname"]);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = YearWise + " " + Degreename;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                    }
                                }
                                int cnt = 2;
                                if (dvSem.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        ds.Tables[0].DefaultView.RowFilter = " degree_code='" + deg_code + "'  and batch_year in ('" + cbl_batch.Items[year].Value + "')";//and FeeCategory in (" + curSemCode + ")
                                        DataTable dvStuCount = ds.Tables[0].DefaultView.ToTable();
                                        if (dvStuCount.Rows.Count > 0)
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cnt].Text = Convert.ToString(dvStuCount.Rows[0]["totcount"].ToString());
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cnt].HorizontalAlign = HorizontalAlign.Center;
                                            TotCnt = Convert.ToInt32(dvStuCount.Rows[0]["totcount"]);
                                            TotCntSemWise = TotCntSemWise + TotCnt;
                                            DeptWiseTot = DeptWiseTot + TotCntSemWise;
                                            if (!degreeWiseTotal.ContainsKey(cnt))
                                                degreeWiseTotal.Add(cnt, Convert.ToString(TotCnt));
                                            else
                                            {
                                                double Count = 0;
                                                double.TryParse(Convert.ToString(degreeWiseTotal[cnt]), out Count);
                                                Count += TotCnt;
                                                degreeWiseTotal.Remove(cnt);
                                                degreeWiseTotal.Add(cnt, Convert.ToString(Count));
                                            }
                                            if (!grandtotal.ContainsKey(cnt))
                                                grandtotal.Add(cnt, Convert.ToString(TotCnt));
                                            else
                                            {
                                                double Count = 0;
                                                double.TryParse(Convert.ToString(grandtotal[cnt]), out Count);
                                                Count += TotCnt;
                                                grandtotal.Remove(cnt);
                                                grandtotal.Add(cnt, Convert.ToString(Count));
                                            }
                                            cnt++;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cnt].Text = Convert.ToString(dvStuCount.Rows[0]["amount"].ToString());
                                            conAmt = Convert.ToDouble(dvStuCount.Rows[0]["amount"].ToString());
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cnt].HorizontalAlign = HorizontalAlign.Center;
                                            cnt++;
                                        }
                                        if (dvStuCount.Rows.Count == 0)
                                        {
                                            cnt++;
                                        }
                                    }
                                    if (ds.Tables[1].Rows.Count > 0)
                                    {
                                        double ConCnt = 0;
                                        for (int Con = 0; Con < chklsconcession.Items.Count; Con++)
                                        {
                                            if (chklsconcession.Items[Con].Selected == true)
                                            {
                                                string deduct = Convert.ToString(chklsconcession.Items[Con].Value);
                                                ds.Tables[1].DefaultView.RowFilter = "degree_code='" + deg_code + "' and deductreason='" + Convert.ToString(chklsconcession.Items[Con].Value) + "'   and batch_year in ('" + cbl_batch.Items[year].Value + "')";//and FeeCategory in(" + curSemCode + ")
                                                DataView dvTotCount = ds.Tables[1].DefaultView;
                                                colnew = Convert.ToInt32(newhash[Convert.ToString(chklsconcession.Items[Con].Value)]);
                                                for (int k = 0; k < dvTotCount.Count; k++)
                                                {
                                                    double.TryParse(Convert.ToString(dvTotCount[k]["totcount"]), out totcount);
                                                    ConCnt = totcount;
                                                    TotCntSemWise = TotCntSemWise + ConCnt;
                                                }

                                                #region Storing concessionCount
                                                if (!httotal.ContainsKey(colnew))
                                                    httotal.Add(colnew, Convert.ToString(ConCnt));
                                                else
                                                {
                                                    double Count = 0;
                                                    double.TryParse(Convert.ToString(httotal[colnew]), out Count);
                                                    Count += ConCnt;
                                                    httotal.Remove(colnew);
                                                    httotal.Add(colnew, Convert.ToString(Count));
                                                }
                                                if (!grandtotal.ContainsKey(colnew))
                                                    grandtotal.Add(colnew, Convert.ToString(ConCnt));
                                                else
                                                {
                                                    double Count = 0;
                                                    double.TryParse(Convert.ToString(grandtotal[colnew]), out Count);
                                                    Count += ConCnt;
                                                    grandtotal.Remove(colnew);
                                                    grandtotal.Add(colnew, Convert.ToString(Count));
                                                }
                                                //DegreeWiseTotal
                                                if (!degreeWiseTotal.ContainsKey(colnew))
                                                    degreeWiseTotal.Add(colnew, Convert.ToString(ConCnt));
                                                else
                                                {
                                                    double Count = 0;
                                                    double.TryParse(Convert.ToString(degreeWiseTotal[colnew]), out Count);
                                                    Count += ConCnt;
                                                    degreeWiseTotal.Remove(colnew);
                                                    degreeWiseTotal.Add(colnew, Convert.ToString(Count));
                                                }
                                                #endregion

                                                #region Concession AmountCalculation
                                                //Concession Amt Calculation//
                                                colnew = Convert.ToInt32(newhashConAmt[Convert.ToString(chklsconcession.Items[Con].Value)]);
                                                for (int con = 0; con < dvTotCount.Count; con++)
                                                {
                                                    double.TryParse(Convert.ToString(dvTotCount[con]["totcount"]), out totcount);
                                                    totConAmt = conAmt * totcount;
                                                }
                                                if (!httotal.ContainsKey(colnew))
                                                    httotal.Add(colnew, Convert.ToString(totConAmt));
                                                else
                                                {
                                                    double Count = 0;
                                                    double.TryParse(Convert.ToString(httotal[colnew]), out Count);
                                                    Count += totConAmt;
                                                    httotal.Remove(colnew);
                                                    httotal.Add(colnew, Convert.ToString(Count));
                                                }
                                                if (!grandtotal.ContainsKey(colnew))
                                                    grandtotal.Add(colnew, Convert.ToString(totConAmt));
                                                else
                                                {
                                                    double Count = 0;
                                                    double.TryParse(Convert.ToString(grandtotal[colnew]), out Count);
                                                    Count += totConAmt;
                                                    grandtotal.Remove(colnew);
                                                    grandtotal.Add(colnew, Convert.ToString(Count));
                                                }
                                                //DegreeWiseTotal
                                                if (!degreeWiseTotal.ContainsKey(colnew))
                                                    degreeWiseTotal.Add(colnew, Convert.ToString(totConAmt));
                                                else
                                                {
                                                    double Count = 0;
                                                    double.TryParse(Convert.ToString(degreeWiseTotal[colnew]), out Count);
                                                    Count += totConAmt;
                                                    degreeWiseTotal.Remove(colnew);
                                                    degreeWiseTotal.Add(colnew, Convert.ToString(Count));
                                                }
                                                #endregion

                                            }
                                        }
                                    }
                                    for (int pay = 0; pay < chklsconcession.Items.Count; pay++)
                                    {
                                        if (chklsconcession.Items[pay].Selected == true)
                                        {
                                            //printing concession count
                                            colnew = Convert.ToInt32(newhash[Convert.ToString(chklsconcession.Items[pay].Value)]);
                                            if (httotal.Contains(colnew))
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colnew].Text = Convert.ToString(httotal[colnew]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colnew].HorizontalAlign = HorizontalAlign.Center;
                                                cnt++;
                                            }
                                            //Printing Concession Amt value
                                            colnew = Convert.ToInt32(newhashConAmt[Convert.ToString(chklsconcession.Items[pay].Value)]);
                                            if (httotal.Contains(colnew))
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colnew].Text = Convert.ToString(httotal[colnew]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colnew].HorizontalAlign = HorizontalAlign.Center;
                                                cnt++;
                                            }
                                        }
                                    }
                                    httotal.Clear();

                                    #region DepartmentWise Student Count and Amount
                                    //Strength of the department//
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cnt].Text = Convert.ToString(TotCntSemWise);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cnt].HorizontalAlign = HorizontalAlign.Center;
                                    if (!grandtotal.ContainsKey(cnt))
                                        grandtotal.Add(cnt, Convert.ToString(TotCntSemWise));
                                    else
                                    {
                                        double Count = 0;
                                        double.TryParse(Convert.ToString(grandtotal[cnt]), out Count);
                                        Count += TotCntSemWise;
                                        grandtotal.Remove(cnt);
                                        grandtotal.Add(cnt, Convert.ToString(Count));
                                    }
                                    //DegreeWiseTotal for studentCount
                                    if (!degreeWiseTotal.ContainsKey(cnt))
                                        degreeWiseTotal.Add(cnt, Convert.ToString(TotCntSemWise));
                                    else
                                    {
                                        double Count = 0;
                                        double.TryParse(Convert.ToString(degreeWiseTotal[cnt]), out Count);
                                        Count += TotCntSemWise;
                                        degreeWiseTotal.Remove(cnt);
                                        degreeWiseTotal.Add(cnt, Convert.ToString(Count));
                                    }
                                    cnt++;
                                    //Received --------Paid without concession Amount 
                                    totConAmt = conAmt * TotCnt;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cnt].Text = Convert.ToString(totConAmt);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cnt].HorizontalAlign = HorizontalAlign.Center;

                                    if (!grandtotal.ContainsKey(cnt))
                                        grandtotal.Add(cnt, Convert.ToString(totConAmt));
                                    else
                                    {
                                        double Count = 0;
                                        double.TryParse(Convert.ToString(grandtotal[cnt]), out Count);
                                        Count += totConAmt;
                                        grandtotal.Remove(cnt);
                                        grandtotal.Add(cnt, Convert.ToString(Count));
                                    }
                                    //DegreeWiseTotal for Received Amount
                                    if (!degreeWiseTotal.ContainsKey(cnt))
                                        degreeWiseTotal.Add(cnt, Convert.ToString(totConAmt));
                                    else
                                    {
                                        double Count = 0;
                                        double.TryParse(Convert.ToString(degreeWiseTotal[cnt]), out Count);
                                        Count += totConAmt;
                                        degreeWiseTotal.Remove(cnt);
                                        degreeWiseTotal.Add(cnt, Convert.ToString(Count));
                                    }
                                    cnt++;
                                    //Demand amount
                                    demandAmt = conAmt * TotCntSemWise;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cnt].Text = Convert.ToString(demandAmt);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cnt].HorizontalAlign = HorizontalAlign.Center;
                                    if (!grandtotal.ContainsKey(cnt))
                                        grandtotal.Add(cnt, Convert.ToString(demandAmt));
                                    else
                                    {
                                        double Count = 0;
                                        double.TryParse(Convert.ToString(grandtotal[cnt]), out Count);
                                        Count += demandAmt;
                                        grandtotal.Remove(cnt);
                                        grandtotal.Add(cnt, Convert.ToString(Count));
                                    }
                                    //DegreeWiseTotal for Received Amount
                                    if (!degreeWiseTotal.ContainsKey(cnt))
                                        degreeWiseTotal.Add(cnt, Convert.ToString(demandAmt));
                                    else
                                    {
                                        double Count = 0;
                                        double.TryParse(Convert.ToString(degreeWiseTotal[cnt]), out Count);
                                        Count += demandAmt;
                                        degreeWiseTotal.Remove(cnt);
                                        degreeWiseTotal.Add(cnt, Convert.ToString(Count));
                                    }
                                    cnt++;
                                    TotCntSemWise = 0;
                                    #endregion
                                }

                            }
                        }

                    }
                    //}
                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.YellowGreen;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 2);

                    for (int Count = 0; Count < FpSpread1.Sheets[0].ColumnHeader.Columns.Count; Count++)
                    {
                        int header = Count + 2;
                        if (degreeWiseTotal.Contains(header))
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, header].Text = Convert.ToString(degreeWiseTotal[header]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, header].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                    degreeWiseTotal.Clear();

                    #region grandTotal
                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("green");
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 2);

                    for (int Count = 0; Count < FpSpread1.Sheets[0].ColumnHeader.Columns.Count; Count++)
                    {
                        int header = Count + 2;
                        if (grandtotal.Contains(header))
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, header].Text = Convert.ToString(grandtotal[header]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, header].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                    #endregion
                }
                #endregion

                #region with Academic Year setting

                Dictionary<string, string> getAcdYear = new Dictionary<string, string>();
                if (cbAcdYear.Checked)
                {
                    #region Academic Year
                    string collegeCode = string.Empty;
                    DataSet dsNormal = ds.Copy();
                    try
                    {
                        collegeCode = Convert.ToString(ddl_collegename.SelectedItem.Value);
                        string acdYear = Convert.ToString(ddlAcademic.SelectedItem.Text);
                        getAcdYear = getOldSettings(acdYear);//(acdYear, clgCode);
                        DataSet dsFinal = new DataSet();
                        if (getAcdYear.Count > 0)
                        {
                            bool boolDs = false;
                            DataTable dtFirst = ds.Tables[0].DefaultView.ToTable();
                            foreach (KeyValuePair<string, string> getVal in getAcdYear)
                            {
                                string feeCate = getVal.Value.Replace(",", "','");
                                string degcourseid = Convert.ToString(getCblSelectedValue(cbl_degree));
                                ds.Tables[4].DefaultView.RowFilter = "college_Code='" + getVal.Key.Split('$')[0] + "' and batch_year='" + getVal.Key.Split('$')[1] + "'";
                                DataTable dtYear = ds.Tables[4].DefaultView.ToTable();
                                ds.Tables[0].DefaultView.RowFilter = "college_Code='" + getVal.Key.Split('$')[0] + "' and batch_year='" + getVal.Key.Split('$')[1] + "' and feecategory in('" + feeCate + "') and Course_id in('" + degcourseid + "')";
                                DataTable dtAllot = ds.Tables[0].DefaultView.ToTable();
                                ds.Tables[1].DefaultView.RowFilter = "college_Code='" + getVal.Key.Split('$')[0] + "' and batch_year='" + getVal.Key.Split('$')[1] + "' and feecategory in('" + feeCate + "')  and Course_id in('" + degcourseid + "')";
                                DataTable dtPaid = ds.Tables[1].DefaultView.ToTable();

                                //ds.Tables[0].DefaultView.RowFilter = "college_Code='" + getVal.Key.Split('$')[0] + "' and batch_year='" + getVal.Key.Split('$')[1] + "' and Course_Id in('" + degcourseid + "')";// and feecategory in('" + feeCate + "')";
                                //DataTable dtstudCnt = ds.Tables[0].DefaultView.ToTable();
                                if (!boolDs)
                                {
                                    dsFinal.Reset();
                                    dsFinal.Tables.Add(dtFirst);
                                    dsFinal.Tables.Add(dtAllot);
                                    dsFinal.Tables.Add(dtPaid);
                                    dsFinal.Tables.Add(dtYear);
                                    // dsFinal.Tables.Add(dtstudCnt);
                                    boolDs = true;
                                }
                                else
                                {
                                    // ds.Merge(ds.Tables[0]);
                                    dsFinal.Merge(dtAllot);
                                    dsFinal.Merge(dtPaid);
                                    dsFinal.Merge(dtYear);
                                    //dsFinal.Merge(dtstudCnt);
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



                //Dictionary<string, string> currentSem = getCurSem();

                #region with academic year

                if (cbAcdYear.Checked == true)
                {
                    for (int year = 0; year < cbl_batch.Items.Count; year++)
                    {
                        if (cbl_batch.Items[year].Selected)
                        {
                            ds.Tables[0].DefaultView.RowFilter = "batch_year='" + cbl_batch.Items[year].Value + "'";
                            DataView dvyr = ds.Tables[0].DefaultView;
                            string collegeCode = Convert.ToString(ddl_collegename.SelectedItem.Value);
                            ds.Tables[0].DefaultView.RowFilter = " college_Code='" + collegeCode + "'";
                            DataTable dtbatch = ds.Tables[0].DefaultView.ToTable();
                            string curSem = string.Empty;
                            string curSemCode = string.Empty;
                            int colnew = 0;
                            double totcount = 0;
                            string batch = Convert.ToString(dtbatch.Rows[year]["batch_year"]);
                            if (!cbAcdYear.Checked)
                            {
                                if (currentSem.ContainsKey(batch))
                                    curSem = Convert.ToString(currentSem[batch]);
                                Dictionary<string, string> getFeeCode = getFeecode(collegeCode);//get current sem code 
                                curSem = getCurYear(curSem);
                                if (getFeeCode.ContainsKey(curSem))
                                    curSemCode = Convert.ToString(getFeeCode[curSem.Trim()]);
                            }
                            else
                            {
                                if (getAcdYear.ContainsKey(collegeCode + "$" + cbl_batch.Items[year].Value))
                                {
                                    curSemCode = Convert.ToString(getAcdYear[collegeCode + "$" + cbl_batch.Items[year].Value]);
                                    curSemCode = curSemCode.Replace(",", "','");
                                }
                            }
                            ds.Tables[0].DefaultView.RowFilter = "batch_year='" + cbl_batch.Items[year].Value + "'   and feecategory in('" + curSemCode + "')";
                            DataTable dtbatchyear = ds.Tables[0].DefaultView.ToTable();
                            for (int deg = 0; deg < dtbatchyear.Rows.Count; deg++)
                            {
                                string deg_code = Convert.ToString(dtbatchyear.Rows[deg]["degree_code"].ToString());
                                ds.Tables[0].DefaultView.RowFilter = "degree_code='" + deg_code + "' and batch_year='" + cbl_batch.Items[year].Value + "' and feecategory in('" + curSemCode + "')";
                                DataView dvSem = ds.Tables[0].DefaultView;
                                double conAmt = 0;
                                double totConAmt = 0;
                                double demandAmt = 0;

                                //=====================For printing the degreewise total===========================//
                                currentCourseID = Convert.ToString(ds.Tables[3].Rows[deg]["Course_Id"].ToString());
                                if (currentCourseID != preCourseID && !string.IsNullOrEmpty(preCourseID))
                                {
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
                                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.YellowGreen;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 2);

                                    for (int Count = 0; Count < FpSpread1.Sheets[0].ColumnHeader.Columns.Count; Count++)
                                    {
                                        int header = Count + 2;
                                        if (degreeWiseTotal.Contains(header))
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, header].Text = Convert.ToString(degreeWiseTotal[header]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, header].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                    }
                                    degreeWiseTotal.Clear();
                                }
                                preCourseID = Convert.ToString(ds.Tables[3].Rows[deg]["Course_Id"].ToString());
                                //====================================================================================//

                                if (dvSem.Count > 0)
                                {
                                    sno++;
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                }
                                ds.Tables[3].DefaultView.RowFilter = "Degree_code='" + deg_code + "'";
                                Dview = ds.Tables[3].DefaultView;
                                if (Dview.Count > 0)
                                {
                                    if (dvSem.Count > 0)
                                    {
                                        string semCat = Convert.ToString(dvSem[0]["feecategory"]); ;
                                        string yearval = " select textval from textvaltable where TextCode in('" + semCat + "')";
                                        DataSet cursemester = d2.select_method_wo_parameter(yearval, "Text");
                                        for (int i = 0; i < cursemester.Tables[0].Rows.Count; i++)
                                        {
                                            string cur_year = Convert.ToString(cursemester.Tables[0].Rows[i]["textval"].ToString());
                                            string curYear = cur_year.Split(' ')[0];
                                            int yearVal = Convert.ToInt32(curYear);
                                            switch (yearVal)
                                            {
                                                case 1:
                                                case 2:
                                                    YearWise = "1 Year";
                                                    break;
                                                case 3:
                                                case 4:
                                                    YearWise = "2 Year";
                                                    break;
                                                case 5:
                                                case 6:
                                                    YearWise = "3 Year";
                                                    break;
                                                case 7:
                                                case 8:
                                                    YearWise = "4 Year";
                                                    break;
                                            }
                                        }
                                        string Degreename = Convert.ToString(Dview[0]["depatname"]);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = YearWise + " " + Degreename;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                    }
                                }
                                int cnt = 2;
                                if (dvSem.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        ds.Tables[0].DefaultView.RowFilter = " degree_code='" + deg_code + "' and FeeCategory in ('" + curSemCode + "') and batch_year in ('" + cbl_batch.Items[year].Value + "')";
                                        DataTable dvStuCount = ds.Tables[0].DefaultView.ToTable();
                                        if (dvStuCount.Rows.Count > 0)
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cnt].Text = Convert.ToString(dvStuCount.Rows[0]["totcount"].ToString());
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cnt].HorizontalAlign = HorizontalAlign.Center;
                                            TotCnt = Convert.ToInt32(dvStuCount.Rows[0]["totcount"]);
                                            TotCntSemWise = TotCntSemWise + TotCnt;
                                            DeptWiseTot = DeptWiseTot + TotCntSemWise;
                                            if (!degreeWiseTotal.ContainsKey(cnt))
                                                degreeWiseTotal.Add(cnt, Convert.ToString(TotCnt));
                                            else
                                            {
                                                double Count = 0;
                                                double.TryParse(Convert.ToString(degreeWiseTotal[cnt]), out Count);
                                                Count += TotCnt;
                                                degreeWiseTotal.Remove(cnt);
                                                degreeWiseTotal.Add(cnt, Convert.ToString(Count));
                                            }
                                            if (!grandtotal.ContainsKey(cnt))
                                                grandtotal.Add(cnt, Convert.ToString(TotCnt));
                                            else
                                            {
                                                double Count = 0;
                                                double.TryParse(Convert.ToString(grandtotal[cnt]), out Count);
                                                Count += TotCnt;
                                                grandtotal.Remove(cnt);
                                                grandtotal.Add(cnt, Convert.ToString(Count));
                                            }
                                            cnt++;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cnt].Text = Convert.ToString(dvStuCount.Rows[0]["amount"].ToString());
                                            conAmt = Convert.ToDouble(dvStuCount.Rows[0]["amount"].ToString());
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cnt].HorizontalAlign = HorizontalAlign.Center;
                                            cnt++;
                                        }
                                        if (dvStuCount.Rows.Count == 0)
                                        {
                                            cnt++;
                                        }
                                    }
                                    if (ds.Tables[1].Rows.Count > 0)
                                    {
                                        double ConCnt = 0;
                                        for (int Con = 0; Con < chklsconcession.Items.Count; Con++)
                                        {
                                            if (chklsconcession.Items[Con].Selected == true)
                                            {
                                                string deduct = Convert.ToString(chklsconcession.Items[Con].Value);
                                                ds.Tables[1].DefaultView.RowFilter = "degree_code='" + deg_code + "' and deductreason='" + Convert.ToString(chklsconcession.Items[Con].Value) + "' and FeeCategory in('" + curSemCode + "')  and batch_year in ('" + cbl_batch.Items[year].Value + "')";
                                                DataView dvTotCount = ds.Tables[1].DefaultView;
                                                colnew = Convert.ToInt32(newhash[Convert.ToString(chklsconcession.Items[Con].Value)]);
                                                for (int k = 0; k < dvTotCount.Count; k++)
                                                {
                                                    double.TryParse(Convert.ToString(dvTotCount[k]["totcount"]), out totcount);
                                                    ConCnt = totcount;
                                                    TotCntSemWise = TotCntSemWise + ConCnt;
                                                }

                                                #region Storing concessionCount
                                                if (!httotal.ContainsKey(colnew))
                                                    httotal.Add(colnew, Convert.ToString(ConCnt));
                                                else
                                                {
                                                    double Count = 0;
                                                    double.TryParse(Convert.ToString(httotal[colnew]), out Count);
                                                    Count += ConCnt;
                                                    httotal.Remove(colnew);
                                                    httotal.Add(colnew, Convert.ToString(Count));
                                                }
                                                if (!grandtotal.ContainsKey(colnew))
                                                    grandtotal.Add(colnew, Convert.ToString(ConCnt));
                                                else
                                                {
                                                    double Count = 0;
                                                    double.TryParse(Convert.ToString(grandtotal[colnew]), out Count);
                                                    Count += ConCnt;
                                                    grandtotal.Remove(colnew);
                                                    grandtotal.Add(colnew, Convert.ToString(Count));
                                                }
                                                //DegreeWiseTotal
                                                if (!degreeWiseTotal.ContainsKey(colnew))
                                                    degreeWiseTotal.Add(colnew, Convert.ToString(ConCnt));
                                                else
                                                {
                                                    double Count = 0;
                                                    double.TryParse(Convert.ToString(degreeWiseTotal[colnew]), out Count);
                                                    Count += ConCnt;
                                                    degreeWiseTotal.Remove(colnew);
                                                    degreeWiseTotal.Add(colnew, Convert.ToString(Count));
                                                }
                                                #endregion

                                                #region Concession AmountCalculation
                                                //Concession Amt Calculation//
                                                colnew = Convert.ToInt32(newhashConAmt[Convert.ToString(chklsconcession.Items[Con].Value)]);
                                                for (int con = 0; con < dvTotCount.Count; con++)
                                                {
                                                    double.TryParse(Convert.ToString(dvTotCount[con]["totcount"]), out totcount);
                                                    totConAmt = conAmt * totcount;
                                                }
                                                if (!httotal.ContainsKey(colnew))
                                                    httotal.Add(colnew, Convert.ToString(totConAmt));
                                                else
                                                {
                                                    double Count = 0;
                                                    double.TryParse(Convert.ToString(httotal[colnew]), out Count);
                                                    Count += totConAmt;
                                                    httotal.Remove(colnew);
                                                    httotal.Add(colnew, Convert.ToString(Count));
                                                }
                                                if (!grandtotal.ContainsKey(colnew))
                                                    grandtotal.Add(colnew, Convert.ToString(totConAmt));
                                                else
                                                {
                                                    double Count = 0;
                                                    double.TryParse(Convert.ToString(grandtotal[colnew]), out Count);
                                                    Count += totConAmt;
                                                    grandtotal.Remove(colnew);
                                                    grandtotal.Add(colnew, Convert.ToString(Count));
                                                }
                                                //DegreeWiseTotal
                                                if (!degreeWiseTotal.ContainsKey(colnew))
                                                    degreeWiseTotal.Add(colnew, Convert.ToString(totConAmt));
                                                else
                                                {
                                                    double Count = 0;
                                                    double.TryParse(Convert.ToString(degreeWiseTotal[colnew]), out Count);
                                                    Count += totConAmt;
                                                    degreeWiseTotal.Remove(colnew);
                                                    degreeWiseTotal.Add(colnew, Convert.ToString(Count));
                                                }
                                                #endregion

                                            }
                                        }
                                    }
                                    for (int pay = 0; pay < chklsconcession.Items.Count; pay++)
                                    {
                                        if (chklsconcession.Items[pay].Selected == true)
                                        {
                                            //printing concession count
                                            colnew = Convert.ToInt32(newhash[Convert.ToString(chklsconcession.Items[pay].Value)]);
                                            if (httotal.Contains(colnew))
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colnew].Text = Convert.ToString(httotal[colnew]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colnew].HorizontalAlign = HorizontalAlign.Center;
                                                cnt++;
                                            }
                                            //Printing Concession Amt value
                                            colnew = Convert.ToInt32(newhashConAmt[Convert.ToString(chklsconcession.Items[pay].Value)]);
                                            if (httotal.Contains(colnew))
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colnew].Text = Convert.ToString(httotal[colnew]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colnew].HorizontalAlign = HorizontalAlign.Center;
                                                cnt++;
                                            }
                                        }
                                    }
                                    httotal.Clear();

                                    #region DepartmentWise Student Count and Amount
                                    //Strength of the department//
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cnt].Text = Convert.ToString(TotCntSemWise);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cnt].HorizontalAlign = HorizontalAlign.Center;
                                    if (!grandtotal.ContainsKey(cnt))
                                        grandtotal.Add(cnt, Convert.ToString(TotCntSemWise));
                                    else
                                    {
                                        double Count = 0;
                                        double.TryParse(Convert.ToString(grandtotal[cnt]), out Count);
                                        Count += TotCntSemWise;
                                        grandtotal.Remove(cnt);
                                        grandtotal.Add(cnt, Convert.ToString(Count));
                                    }
                                    //DegreeWiseTotal for studentCount
                                    if (!degreeWiseTotal.ContainsKey(cnt))
                                        degreeWiseTotal.Add(cnt, Convert.ToString(TotCntSemWise));
                                    else
                                    {
                                        double Count = 0;
                                        double.TryParse(Convert.ToString(degreeWiseTotal[cnt]), out Count);
                                        Count += TotCntSemWise;
                                        degreeWiseTotal.Remove(cnt);
                                        degreeWiseTotal.Add(cnt, Convert.ToString(Count));
                                    }
                                    cnt++;
                                    //Received --------Paid without concession Amount 
                                    totConAmt = conAmt * TotCnt;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cnt].Text = Convert.ToString(totConAmt);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cnt].HorizontalAlign = HorizontalAlign.Center;

                                    if (!grandtotal.ContainsKey(cnt))
                                        grandtotal.Add(cnt, Convert.ToString(totConAmt));
                                    else
                                    {
                                        double Count = 0;
                                        double.TryParse(Convert.ToString(grandtotal[cnt]), out Count);
                                        Count += totConAmt;
                                        grandtotal.Remove(cnt);
                                        grandtotal.Add(cnt, Convert.ToString(Count));
                                    }
                                    //DegreeWiseTotal for Received Amount
                                    if (!degreeWiseTotal.ContainsKey(cnt))
                                        degreeWiseTotal.Add(cnt, Convert.ToString(totConAmt));
                                    else
                                    {
                                        double Count = 0;
                                        double.TryParse(Convert.ToString(degreeWiseTotal[cnt]), out Count);
                                        Count += totConAmt;
                                        degreeWiseTotal.Remove(cnt);
                                        degreeWiseTotal.Add(cnt, Convert.ToString(Count));
                                    }
                                    cnt++;
                                    //Demand amount
                                    demandAmt = conAmt * TotCntSemWise;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cnt].Text = Convert.ToString(demandAmt);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cnt].HorizontalAlign = HorizontalAlign.Center;
                                    if (!grandtotal.ContainsKey(cnt))
                                        grandtotal.Add(cnt, Convert.ToString(demandAmt));
                                    else
                                    {
                                        double Count = 0;
                                        double.TryParse(Convert.ToString(grandtotal[cnt]), out Count);
                                        Count += demandAmt;
                                        grandtotal.Remove(cnt);
                                        grandtotal.Add(cnt, Convert.ToString(Count));
                                    }
                                    //DegreeWiseTotal for Received Amount
                                    if (!degreeWiseTotal.ContainsKey(cnt))
                                        degreeWiseTotal.Add(cnt, Convert.ToString(demandAmt));
                                    else
                                    {
                                        double Count = 0;
                                        double.TryParse(Convert.ToString(degreeWiseTotal[cnt]), out Count);
                                        Count += demandAmt;
                                        degreeWiseTotal.Remove(cnt);
                                        degreeWiseTotal.Add(cnt, Convert.ToString(Count));
                                    }
                                    cnt++;
                                    TotCntSemWise = 0;
                                    #endregion
                                }

                            }
                        }

                    }
                    //}
                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.YellowGreen;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 2);

                    for (int Count = 0; Count < FpSpread1.Sheets[0].ColumnHeader.Columns.Count; Count++)
                    {
                        int header = Count + 2;
                        if (degreeWiseTotal.Contains(header))
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, header].Text = Convert.ToString(degreeWiseTotal[header]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, header].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                    degreeWiseTotal.Clear();

                    #region grandTotal
                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("green");
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 2);

                    for (int Count = 0; Count < FpSpread1.Sheets[0].ColumnHeader.Columns.Count; Count++)
                    {
                        int header = Count + 2;
                        if (grandtotal.Contains(header))
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, header].Text = Convert.ToString(grandtotal[header]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, header].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                    #endregion
                }
                #endregion
            }
                #endregion

            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            FpSpread1.SaveChanges();
            divspread.Visible = true;
            print.Visible = true;
            imgdiv2.Visible = false;
            lbl_alert.Text = "";
            lblvalidation1.Text = "";
            //// }

                #endregion


            #endregion

            #region Cummulative done by abarna
            if (rbCummulative.Checked == true)
            {
                UserbasedRights();
                DataView dvWithoutCon = new DataView();
                DataView dvWithCon = new DataView();
                DataView dvdegreeCode = new DataView();
                DataView degreename = new DataView();
                Hashtable grandtotal = new Hashtable();
                string clgCode = string.Empty;
                string semester = string.Empty;
                string headercode = string.Empty;
                Hashtable newhash = new Hashtable();
                Hashtable newhashConAmt = new Hashtable();

                #region design

                int semCount = 0;
                int ConcessionCount = 0;
                // int countval = 0;
                string concode = string.Empty;

                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    if (cbl_sem.Items[i].Selected)
                        semCount++;
                }
                for (int i = 0; i < chklsconcession.Items.Count; i++)
                {
                    if (chklsconcession.Items[i].Selected)
                        ConcessionCount++;
                }
                //int value = 0;
                Hashtable htledg = new Hashtable();
                Hashtable hthedg = new Hashtable();
                DataView dvhed = new DataView();

                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnHeader.Rows.Count = 2;
                FpSpread1.Sheets[0].ColumnCount = 0;
                FpSpread1.CommandBar.Visible = false;
                FpSpread1.Sheets[0].AutoPostBack = true;
                FpSpread1.Sheets[0].RowHeader.Visible = false;
                FpSpread1.Sheets[0].ColumnCount = 2;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                FpSpread1.Sheets[0].ColumnCount++;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Class";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Columns[1].Width = 10;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 3, 1);
                // FpSpread1.Sheets[0].ColumnCount++;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Year";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Columns[2].Width = 10;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);



                //if (ddlheader.Items.Count > 0)
                //{


                //for (int hed = 0; hed < ddlheader.Items.Count; hed++)
                //{
                //if (!ddlheader.Items[hed].Selected)
                //    continue;
                string headerName = ddlheader.SelectedItem.Text;
                FpSpread1.Sheets[0].ColumnHeader.Columns.Count++;
                string academicYear = ddlAcademic.SelectedItem.Text;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = headerName + " " + academicYear;
                //   htledg.Add(Convert.ToString(ddlheader.Items[hed].Value), Convert.ToString(FpSpread1.Sheets[0].ColumnCount - 1));
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 1, 2);

                if (chklsconcession.Items.Count > 0)
                {
                    //FpSpread1.Sheets[0].ColumnHeader.Rows.Count++;
                    //count = FpSpread1.Sheets[0].ColumnCount - 1;
                    //FpSpread1.Sheets[0].ColumnHeader.Columns.Count++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Advance";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = "1";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;



                    for (int s = 0; s < chklsconcession.Items.Count; s++)
                    {
                        if (chklsconcession.Items[s].Selected == true)
                        {
                            //countval++;
                            FpSpread1.Sheets[0].ColumnCount++;
                            newhash.Add(Convert.ToString(chklsconcession.Items[s].Value), FpSpread1.Sheets[0].ColumnCount - 1);
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(chklsconcession.Items[s].Text);
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(chklsconcession.Items[s].Value);
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = true;


                        }
                    }
                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total Strength";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;

                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Advance Rate";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //FpSpread1.Sheets[0].ColumnCount++;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Grand Total";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                }
                #endregion

                #region value
                double StudentCount = 0;
                double TotCntSemWise = 0;
                double TotCnt = 0;
                //double ConCnt = 0;
                int Count_Val = 0;
                string textval = string.Empty;
                string Course_id = string.Empty;
                string currentCourseID = string.Empty;
                string preCourseID = string.Empty;
                double DeptWiseTot = 0;
                Hashtable degreeWiseTotal = new Hashtable();
                Hashtable httotal = new Hashtable();
                int sno = 0;
                Dictionary<string, string> currentSem = getCurSem();
                DataView Dview = new DataView();
                string YearWise = string.Empty;
                Dictionary<string, string> getAcdYear = new Dictionary<string, string>();
                if (cbAcdYear.Checked)
                {
                    #region Academic Year
                    string collegeCode = string.Empty;
                    DataSet dsNormal = ds.Copy();
                    try
                    {
                        collegeCode = Convert.ToString(ddl_collegename.SelectedItem.Value);
                        string acdYear = Convert.ToString(ddlAcademic.SelectedItem.Text);
                        getAcdYear = getOldSettings(acdYear);//(acdYear, clgCode);
                        DataSet dsFinal = new DataSet();
                        if (getAcdYear.Count > 0)
                        {
                            bool boolDs = false;
                            DataTable dtFirst = ds.Tables[0].DefaultView.ToTable();
                            foreach (KeyValuePair<string, string> getVal in getAcdYear)
                            {
                                string feeCate = getVal.Value.Replace(",", "','");
                                string degcourseid = Convert.ToString(getCblSelectedValue(cbl_degree));
                                ds.Tables[4].DefaultView.RowFilter = "college_Code='" + getVal.Key.Split('$')[0] + "' and batch_year='" + getVal.Key.Split('$')[1] + "'";
                                DataTable dtYear = ds.Tables[4].DefaultView.ToTable();
                                ds.Tables[0].DefaultView.RowFilter = "college_Code='" + getVal.Key.Split('$')[0] + "' and batch_year='" + getVal.Key.Split('$')[1] + "' and feecategory in('" + feeCate + "') and Course_id in('" + degcourseid + "')";
                                DataTable dtAllot = ds.Tables[0].DefaultView.ToTable();
                                ds.Tables[1].DefaultView.RowFilter = "college_Code='" + getVal.Key.Split('$')[0] + "' and batch_year='" + getVal.Key.Split('$')[1] + "' and feecategory in('" + feeCate + "')  and Course_id in('" + degcourseid + "')";
                                DataTable dtPaid = ds.Tables[1].DefaultView.ToTable();

                                //ds.Tables[0].DefaultView.RowFilter = "college_Code='" + getVal.Key.Split('$')[0] + "' and batch_year='" + getVal.Key.Split('$')[1] + "' and Course_Id in('" + degcourseid + "')";// and feecategory in('" + feeCate + "')";
                                //DataTable dtstudCnt = ds.Tables[0].DefaultView.ToTable();
                                if (!boolDs)
                                {
                                    dsFinal.Reset();
                                    dsFinal.Tables.Add(dtFirst);
                                    dsFinal.Tables.Add(dtAllot);
                                    dsFinal.Tables.Add(dtPaid);
                                    dsFinal.Tables.Add(dtYear);
                                    // dsFinal.Tables.Add(dtstudCnt);
                                    boolDs = true;
                                }
                                else
                                {
                                    // ds.Merge(ds.Tables[0]);
                                    dsFinal.Merge(dtAllot);
                                    dsFinal.Merge(dtPaid);
                                    dsFinal.Merge(dtYear);
                                    //dsFinal.Merge(dtstudCnt);
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
                if (cbAcdYear.Checked == true)
                {
                    for (int year = 0; year < cbl_batch.Items.Count; year++)
                    {
                        if (cbl_batch.Items[year].Selected)
                        {
                            ds.Tables[0].DefaultView.RowFilter = "batch_year='" + cbl_batch.Items[year].Value + "'";
                            DataView dvyr = ds.Tables[0].DefaultView;
                            string collegeCode = Convert.ToString(ddl_collegename.SelectedItem.Value);
                            ds.Tables[0].DefaultView.RowFilter = " college_Code='" + collegeCode + "'";
                            DataTable dtbatch = ds.Tables[0].DefaultView.ToTable();
                            string curSem = string.Empty;
                            string curSemCode = string.Empty;
                            int colnew = 0;
                            double totcount = 0;
                            string batch = Convert.ToString(dtbatch.Rows[year]["batch_year"]);
                            if (!cbAcdYear.Checked)
                            {
                                if (currentSem.ContainsKey(batch))
                                    curSem = Convert.ToString(currentSem[batch]);
                                Dictionary<string, string> getFeeCode = getFeecode(collegeCode);//get current sem code 
                                curSem = getCurYear(curSem);
                                if (getFeeCode.ContainsKey(curSem))
                                    curSemCode = Convert.ToString(getFeeCode[curSem.Trim()]);
                            }
                            else
                            {
                                if (getAcdYear.ContainsKey(collegeCode + "$" + cbl_batch.Items[year].Value))
                                {
                                    curSemCode = Convert.ToString(getAcdYear[collegeCode + "$" + cbl_batch.Items[year].Value]);
                                    curSemCode = curSemCode.Replace(",", "','");
                                }
                            }

                            ds.Tables[0].DefaultView.RowFilter = "batch_year='" + cbl_batch.Items[year].Value + "' and feecategory in('" + curSemCode + "') ";
                            DataTable dtbatchyear = ds.Tables[0].DefaultView.ToTable();
                            for (int deg = 0; deg < dtbatchyear.Rows.Count; deg++)
                            {
                                string deg_code = Convert.ToString(dtbatchyear.Rows[deg]["Course_Id"].ToString());
                                ds.Tables[0].DefaultView.RowFilter = "Course_Id='" + deg_code + "' and batch_year='" + cbl_batch.Items[year].Value + "' and feecategory in('" + curSemCode + "')";
                                DataView dvSem = ds.Tables[0].DefaultView;
                                double conAmt = 0;
                                double totConAmt = 0;
                                double demandAmt = 0;

                                //=====================For printing the degreewise total===========================//
                                currentCourseID = Convert.ToString(ds.Tables[3].Rows[deg]["Course_Id"].ToString());
                                if (currentCourseID != preCourseID && !string.IsNullOrEmpty(preCourseID))
                                {
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
                                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.YellowGreen;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 2);

                                    for (int Count = 0; Count < FpSpread1.Sheets[0].ColumnHeader.Columns.Count; Count++)
                                    {
                                        int header = Count + 2;
                                        if (degreeWiseTotal.Contains(header))
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, header].Text = Convert.ToString(degreeWiseTotal[header]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, header].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                    }
                                    degreeWiseTotal.Clear();
                                }
                                preCourseID = Convert.ToString(ds.Tables[3].Rows[deg]["Course_Id"].ToString());
                                //====================================================================================//

                                if (dvSem.Count > 0)
                                {
                                    sno++;
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                }
                                ds.Tables[3].DefaultView.RowFilter = "Course_Id='" + deg_code + "'";
                                Dview = ds.Tables[3].DefaultView;

                                if (Dview.Count > 0)
                                {

                                    if (dvSem.Count > 0)
                                    {
                                        string semCat = Convert.ToString(dvSem[0]["feecategory"]); ;
                                        string yearval = " select textval from textvaltable where TextCode in('" + semCat + "')";
                                        DataSet cursemester = d2.select_method_wo_parameter(yearval, "Text");
                                        for (int i = 0; i < cursemester.Tables[0].Rows.Count; i++)
                                        {
                                            string cur_year = Convert.ToString(cursemester.Tables[0].Rows[i]["textval"].ToString());
                                            string curYear = cur_year.Split(' ')[0];
                                            int yearVal = Convert.ToInt32(curYear);
                                            switch (yearVal)
                                            {
                                                case 1:
                                                case 2:
                                                    YearWise = "1 Year";
                                                    break;
                                                case 3:
                                                case 4:
                                                    YearWise = "2 Year";
                                                    break;
                                                case 5:
                                                case 6:
                                                    YearWise = "3 Year";
                                                    break;
                                                case 7:
                                                case 8:
                                                    YearWise = "4 Year";
                                                    break;
                                            }
                                        }
                                        string Degreename = Convert.ToString(Dview[0]["depatname"]);
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Degreename;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = YearWise;
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                    }
                                }

                                int cnt = 3;
                                double amt = 0;
                                double partamt = 0;
                                double total = 0;
                                if (dvSem.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {

                                        ds.Tables[0].DefaultView.RowFilter = " Course_Id='" + deg_code + "' and FeeCategory in ('" + curSemCode + "') and batch_year in ('" + cbl_batch.Items[year].Value + "')";
                                        DataTable dvStuCount = ds.Tables[0].DefaultView.ToTable();

                                        if (dvStuCount.Rows.Count > 0)
                                        {
                                            conAmt = Convert.ToDouble(dvStuCount.Rows[0]["amount"].ToString());
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cnt].Text = Convert.ToString(dvStuCount.Rows[0]["totcount"].ToString());
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cnt].HorizontalAlign = HorizontalAlign.Center;
                                            TotCnt = Convert.ToInt32(dvStuCount.Rows[0]["totcount"]);
                                            TotCntSemWise = TotCntSemWise + TotCnt;
                                            DeptWiseTot = DeptWiseTot + TotCntSemWise;
                                            if (!degreeWiseTotal.ContainsKey(cnt))
                                                degreeWiseTotal.Add(cnt, Convert.ToString(TotCnt));
                                            else
                                            {
                                                double Count = 0;
                                                double.TryParse(Convert.ToString(degreeWiseTotal[cnt]), out Count);
                                                Count += TotCnt;
                                                degreeWiseTotal.Remove(cnt);
                                                degreeWiseTotal.Add(cnt, Convert.ToString(Count));
                                            }
                                            if (!grandtotal.ContainsKey(cnt))
                                                grandtotal.Add(cnt, Convert.ToString(TotCnt));
                                            else
                                            {
                                                double Count = 0;
                                                double.TryParse(Convert.ToString(grandtotal[cnt]), out Count);
                                                Count += TotCnt;
                                                grandtotal.Remove(cnt);
                                                grandtotal.Add(cnt, Convert.ToString(Count));
                                            }

                                            amt = Convert.ToDouble(dvStuCount.Rows[0]["amount"].ToString());
                                            partamt = amt / 2;
                                            conAmt = Convert.ToDouble(dvStuCount.Rows[0]["amount"].ToString());

                                        }
                                        if (dvStuCount.Rows.Count == 0)
                                        {
                                            cnt++;
                                        }
                                    }
                                    if (ds.Tables[1].Rows.Count > 0)
                                    {
                                        double ConCnt = 0;
                                        for (int Con = 0; Con < chklsconcession.Items.Count; Con++)
                                        {
                                            if (chklsconcession.Items[Con].Selected == true)
                                            {
                                                string deduct = Convert.ToString(chklsconcession.Items[Con].Value);
                                                ds.Tables[1].DefaultView.RowFilter = "Course_Id='" + deg_code + "' and deductreason='" + Convert.ToString(chklsconcession.Items[Con].Value) + "' and FeeCategory in('" + curSemCode + "')  and batch_year in ('" + cbl_batch.Items[year].Value + "')";
                                                DataView dvTotCount = ds.Tables[1].DefaultView;
                                                colnew = Convert.ToInt32(newhash[Convert.ToString(chklsconcession.Items[Con].Value)]);
                                                for (int k = 0; k < dvTotCount.Count; k++)
                                                {
                                                    double.TryParse(Convert.ToString(dvTotCount[k]["totcount"]), out totcount);
                                                    ConCnt = totcount;
                                                    TotCntSemWise = TotCntSemWise + ConCnt;
                                                }

                                                #region Storing concessionCount
                                                if (!httotal.ContainsKey(colnew))
                                                    httotal.Add(colnew, Convert.ToString(ConCnt));
                                                else
                                                {
                                                    double Count = 0;
                                                    double.TryParse(Convert.ToString(httotal[colnew]), out Count);
                                                    Count += ConCnt;
                                                    httotal.Remove(colnew);
                                                    httotal.Add(colnew, Convert.ToString(Count));
                                                }
                                                if (!grandtotal.ContainsKey(colnew))
                                                    grandtotal.Add(colnew, Convert.ToString(ConCnt));
                                                else
                                                {
                                                    double Count = 0;
                                                    double.TryParse(Convert.ToString(grandtotal[colnew]), out Count);
                                                    Count += ConCnt;
                                                    grandtotal.Remove(colnew);
                                                    grandtotal.Add(colnew, Convert.ToString(Count));
                                                }

                                                #endregion



                                            }
                                        }
                                    }
                                    for (int pay = 0; pay < chklsconcession.Items.Count; pay++)
                                    {
                                        if (chklsconcession.Items[pay].Selected == true)
                                        {
                                            //printing concession count
                                            colnew = Convert.ToInt32(newhash[Convert.ToString(chklsconcession.Items[pay].Value)]);
                                            if (httotal.Contains(colnew))
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colnew].Text = Convert.ToString(httotal[colnew]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colnew].HorizontalAlign = HorizontalAlign.Center;
                                                cnt++;
                                            }

                                        }
                                    }
                                    total = TotCntSemWise * partamt;
                                    httotal.Clear();
                                    cnt++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cnt].Text = Convert.ToString(TotCntSemWise);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cnt].HorizontalAlign = HorizontalAlign.Center;
                                    if (!grandtotal.ContainsKey(cnt))
                                        grandtotal.Add(cnt, Convert.ToString(TotCntSemWise));
                                    else
                                    {
                                        double Count = 0;
                                        double.TryParse(Convert.ToString(grandtotal[cnt]), out Count);
                                        Count += TotCntSemWise;
                                        grandtotal.Remove(cnt);
                                        grandtotal.Add(cnt, Convert.ToString(Count));
                                    }
                                    TotCntSemWise = 0;
                                    cnt++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cnt].Text = Convert.ToString(partamt);
                                    ////conAmt = Convert.ToDouble(dvStuCount.Rows[0]["amount"].ToString());
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cnt].HorizontalAlign = HorizontalAlign.Center;
                                    if (!grandtotal.ContainsKey(cnt))
                                        grandtotal.Add(cnt, Convert.ToString(partamt));
                                    else
                                    {
                                        double Count = 0;
                                        double.TryParse(Convert.ToString(grandtotal[cnt]), out Count);
                                        Count += partamt;
                                        grandtotal.Remove(cnt);
                                        grandtotal.Add(cnt, Convert.ToString(Count));
                                    }
                                    cnt++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cnt].Text = Convert.ToString(total);
                                    ////conAmt = Convert.ToDouble(dvStuCount.Rows[0]["amount"].ToString());
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, cnt].HorizontalAlign = HorizontalAlign.Center;
                                    if (!grandtotal.ContainsKey(cnt))
                                        grandtotal.Add(cnt, Convert.ToString(total));
                                    else
                                    {
                                        double Count = 0;
                                        double.TryParse(Convert.ToString(grandtotal[cnt]), out Count);
                                        Count += total;
                                        grandtotal.Remove(cnt);
                                        grandtotal.Add(cnt, Convert.ToString(Count));
                                    }
                                }

                            }
                        }

                    }
                    //}
                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.YellowGreen;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 2);

                    for (int Count = 0; Count < FpSpread1.Sheets[0].ColumnHeader.Columns.Count; Count++)
                    {
                        int header = Count + 2;
                        if (degreeWiseTotal.Contains(header))
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, header].Text = Convert.ToString(degreeWiseTotal[header]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, header].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                    degreeWiseTotal.Clear();

                    #region grandTotal
                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("green");
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 2);

                    for (int Count = 0; Count < FpSpread1.Sheets[0].ColumnHeader.Columns.Count; Count++)
                    {
                        int header = Count + 2;
                        if (grandtotal.Contains(header))
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, header].Text = Convert.ToString(grandtotal[header]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, header].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                    #endregion
                }

                #endregion

            }
            #endregion
        }

        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collegecode, "ConsolidatedDemandReport.aspx"); 
        }

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
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "ConsolidatedDemandReport.aspx"); }
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
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "ConsolidatedDemandReport.aspx"); }
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
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "ConsolidatedDemandReport.aspx"); }
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
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "ConsolidatedDemandReport.aspx"); }
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
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "ConsolidatedDemandReport.aspx"); }
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
            degreedetails = "Consolidated Demand Report " + '@';
            pagename = "ConsolidatedDemandReport.aspx";
            Printcontrolhed.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrolhed.Visible = true;
        }
        catch { }
    }

    #endregion

}