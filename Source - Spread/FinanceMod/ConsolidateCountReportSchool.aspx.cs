using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class ConsolidateCountReportSchool : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    int i = 0;

    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
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
                bindsec();
                loadheaderandledger();
                ledgerload();
                loadpaid();
                loadfinanceyear();
                loadScheme(collegecode);

                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_todate.Attributes.Add("readonly", "readonly");
            }
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
        }
        catch (Exception ex) { }
    }

    protected void lb3_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("default.aspx", false);

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
            bindsec();
            loadheaderandledger();
            ledgerload();
            loadpaid();
            loadfinanceyear();
            loadScheme(collegecode);
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
            bindsec();
            bindsem();
        }
        catch { }
    }
    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_dept, cbl_dept, txt_dept, lbldept.Text, "--Select--");
            bindsec();
            bindsem();
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
            bindsec();
        }
        catch (Exception ex)
        { }
    }
    protected void cbl_sem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_sem, cbl_sem, txt_sem, "Semester", "--Select--");
            bindsec();
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

    #region sec
    public void bindsec()
    {
        try
        {
            cbl_sect.Items.Clear();
            txt_sect.Text = "---Select---";
            cb_sect.Checked = false;
            string build = "";
            if (cbl_sem.Items.Count > 0)
            {
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    if (cbl_sem.Items[i].Selected == true)
                    {
                        if (build == "")
                        {
                            build = Convert.ToString(cbl_sem.Items[i].Value);
                        }
                        else
                        {
                            build = build + "'" + "," + "'" + Convert.ToString(cbl_sem.Items[i].Value);
                        }
                    }
                }
            }
            string clgvalue = ddl_collegename.SelectedItem.Value.ToString();
            if (build != "")
            {
                ds = d2.BindSectionDetailmult(clgvalue);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_sect.DataSource = ds;
                    cbl_sect.DataTextField = "sections";
                    cbl_sect.DataValueField = "sections";
                    cbl_sect.DataBind();
                    if (cbl_sect.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_sect.Items.Count; row++)
                        {
                            cbl_sect.Items[row].Selected = true;
                        }
                        txt_sect.Text = "Section(" + cbl_sect.Items.Count + ")";
                        cb_sect.Checked = true;
                    }

                }
            }
            else
            {
                cb_sect.Checked = false;
                txt_sect.Text = "--Select--";
            }
        }

        catch (Exception ex)
        {
        }
    }
    protected void cb_sect_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cb_sect, cbl_sect, txt_sect, "Section", "--Select--");
        }
        catch (Exception ex)
        { }
    }
    protected void cbl_sect_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_sect, cbl_sect, txt_sect, "Section", "--Select--");
        }
        catch (Exception ex)
        { }
    }
    #endregion

    #region paymentmode
    public void loadpaid()
    {
        try
        {
            chkl_paid.Items.Clear();
            //cbltypedep.Items.Add(new ListItem("Cash", "1"));
            //cbltypedep.Items.Add(new ListItem("Cheque", "2"));
            //cbltypedep.Items.Add(new ListItem("DD", "3"));
            //cbltypedep.Items.Add(new ListItem("Challan", "4"));
            //cbltypedep.Items.Add(new ListItem("Online", "5"));
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
        try
        {
            CallCheckboxChange(chk_paid, chkl_paid, txt_paid, "Paid", "--Select--");
        }
        catch
        { }
    }
    public void chkl_paid_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(chk_paid, chkl_paid, txt_paid, "Paid", "--Select--");
        }
        catch
        { }
    }
    #endregion

    #region header and ledger
    public void loadheaderandledger()
    {
        try
        {
            string clgvalue = ddl_collegename.SelectedItem.Value.ToString();
            cblheader.Items.Clear();
            string query = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + clgvalue + "  ";

            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblheader.DataSource = ds;
                cblheader.DataTextField = "HeaderName";
                cblheader.DataValueField = "HeaderPK";
                cblheader.DataBind();
                for (int i = 0; i < cblheader.Items.Count; i++)
                {
                    cblheader.Items[i].Selected = true;
                }
                txtheader.Text = "Header(" + cblheader.Items.Count + ")";
                cbheader.Checked = true;
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
            string clgvalue = ddl_collegename.SelectedItem.Value.ToString();
            cblledger.Items.Clear();
            string hed = "";
            for (int i = 0; i < cblheader.Items.Count; i++)
            {
                if (cblheader.Items[i].Selected == true)
                {
                    if (hed == "")
                    {
                        hed = cblheader.Items[i].Value.ToString();
                    }
                    else
                    {
                        hed = hed + "','" + "" + cblheader.Items[i].Value.ToString() + "";
                    }
                }
            }


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
    public void cbheader_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cbheader, cblheader, txtheader, "Header", "--Select--");
            ledgerload();
        }
        catch (Exception ex)
        { }
    }

    public void cblheader_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cbheader, cblheader, txtheader, "Header", "--Select--");
            ledgerload();
        }
        catch (Exception ex)
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

    protected void cbScheme_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cbScheme, chklScheme, txtScheme, "Scheme", "Scheme");
    }
    protected void chklScheme_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbScheme, chklScheme, txtScheme, "Scheme", "Scheme");
    }
    private void loadScheme(string collegeCode)
    {
        try
        {

            chklScheme.Items.Clear();
            cbScheme.Checked = false;
            txtScheme.Text = "---Select---";
            string sql = "select TextCode,TextVal from TextValTable where TextCriteria ='Schm' and college_code ='" + collegeCode + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                chklScheme.DataSource = ds;
                chklScheme.DataTextField = "TextVal";
                chklScheme.DataValueField = "TextCode";
                chklScheme.DataBind();
                if (chklScheme.Items.Count > 0)
                {
                    for (i = 0; i < chklScheme.Items.Count; i++)
                    {
                        chklScheme.Items[i].Selected = true;
                    }
                    txtScheme.Text = "Scheme(" + chklScheme.Items.Count + ")";
                    cbScheme.Checked = true;
                }
            }
        }
        catch { }
    }
    //datewise
    protected void cbdatewise_OnCheckedChanged(object sender, EventArgs e)
    {
        if (cbdatewise.Checked == true)
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

    #region button search

    protected DataSet dsvalue()
    {
        DataSet dsload = new DataSet();
        try
        {
            UserbasedRights();
            string batch = getCblSelectedValue(cbl_batch);
            string degcode = getCblSelectedValue(cbl_dept);
            string schemecodes = getCblSelectedValue(chklScheme);
            string feecat = getCblSelectedValue(cbl_sem);
            string sec = getCblSelectedValue(cbl_sect);
            string hedgid = getCblSelectedValue(cblheader);
            string ledgid = getCblSelectedValue(cblledger);
            string paymode = getCblSelectedValue(chkl_paid);
            string fnlyr = getCblSelectedValue(chklsfyear);
            string datewise = "";
            string fromdate = Convert.ToString(txt_fromdate.Text);
            string todate = Convert.ToString(txt_todate.Text);
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
            {
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            }
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
            {
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
            }
            datewise = "";

            if (ddl_collegename.Items.Count > 0)
                collegecode = Convert.ToString(ddl_collegename.SelectedItem.Value);

            #region include

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
                            debar = " r.Exam_Flag like '%debar'";
                        }
                        if (cblinclude.Items[i].Value == "3")
                        {
                            disc = "  r.DelFlag=1";
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



            string SelectQ = "";
            if (cbdatewise.Checked == false)
            {
                #region Without Date
                SelectQ = "select Count(distinct f.App_No) as totcount,r.degree_code,r.Batch_Year,f.FeeCategory,(select textval from textvaltable where textcode = feecategory)+'-'+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=f.FinYearFK) as FinYear  from FT_FeeAllot f,Registration r where f.App_No=r.App_No  and r.college_code='" + collegecode + "' " + commondist + "";
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
                if (sec != "")
                {
                    // SelectQ += " and   ISNULL( r.Sections,'') in ('','')";
                }
                if (paymode != "")
                {
                    SelectQ += " and f.PayMode in('" + paymode + "')";
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
                SelectQ += " group by r.degree_code,r.Batch_Year,f.FeeCategory,f.finyearfk order by r.degree_code";

                //fully paid
                #region old
                //SelectQ += " select Count(distinct f.App_No) as totcount,SUM(PaidAmount)as paid,degree_code,f.FeeCategory,r.Batch_Year  from FT_FeeAllot f,Registration r where f.App_No = r.app_no  and balamount = 0 and r.college_code='" + collegecode + "'";
                //if (batch != "")
                //{
                //    SelectQ += " and r.Batch_Year in('" + batch + "')";
                //}
                //if (degcode != "")
                //{
                //    SelectQ += " and r.degree_code in('" + degcode + "')";
                //}
                //if (feecat != "")
                //{
                //    SelectQ += " and f.FeeCategory in('" + feecat + "')";
                //}
                //if (sec != "")
                //{
                //    // SelectQ += " and   ISNULL( r.Sections,'') in ('','')";
                //}
                //if (paymode != "")
                //{
                //    SelectQ += " and f.PayMode in('" + paymode + "')";
                //}
                //if (hedgid != "")
                //{
                //    SelectQ += "  and f.HeaderFK in('" + hedgid + "')";
                //}
                //if (ledgid != "")
                //{
                //    SelectQ += " and f.LedgerFK in('" + ledgid + "')";
                //}
                //if (fnlyr != "")
                //{
                //    SelectQ += " and f.FinYearFK in('" + fnlyr + "')";
                //}
                //SelectQ += " group by degree_code,f.FeeCategory,r.Batch_Year having sum(TotalAmount) > 0 and sum(BalAmount) = 0";
                #endregion

                SelectQ += " select  SUM(PaidAmount) as paid,r.degree_code,r.Batch_Year,f.FeeCategory,(select textval from textvaltable where textcode = feecategory)+'-'+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=f.FinYearFK) as FinYear  from FT_FeeAllot f,Registration r where r.App_No=f.App_No and r.college_code='" + collegecode + "' " + commondist + "";
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
                if (sec != "")
                {
                    // SelectQ += " and   ISNULL( r.Sections,'') in ('','')";
                }
                if (paymode != "")
                {
                    SelectQ += " and f.PayMode in('" + paymode + "')";
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
                SelectQ += " group by r.App_No,r.degree_code,r.Batch_Year,f.FeeCategory,f.finyearfk having sum(TotalAmount) > 0 and sum(BalAmount) = 0";
                //partial paid
                #region old
                //SelectQ += " select Count(distinct f.App_No) as totcount,SUM(PaidAmount)as partamt,degree_code,f.FeeCategory,r.Batch_Year  from FT_FeeAllot f,Registration r where f.App_No = r.app_no  and balamount > 0  and r.college_code='" + collegecode + "'  ";
                //if (batch != "")
                //{
                //    SelectQ += " and r.Batch_Year in('" + batch + "')";
                //}
                //if (degcode != "")
                //{
                //    SelectQ += " and r.degree_code in('" + degcode + "')";
                //}
                //if (feecat != "")
                //{
                //    SelectQ += " and f.FeeCategory in('" + feecat + "')";
                //}
                //if (sec != "")
                //{
                //    // SelectQ += " and   ISNULL( r.Sections,'') in ('','')";
                //}
                //if (paymode != "")
                //{
                //    SelectQ += " and f.PayMode in('" + paymode + "')";
                //}
                //if (hedgid != "")
                //{
                //    SelectQ += "  and f.HeaderFK in('" + hedgid + "')";
                //}
                //if (ledgid != "")
                //{
                //    SelectQ += " and f.LedgerFK in('" + ledgid + "')";
                //}
                //if (fnlyr != "")
                //{
                //    SelectQ += " and f.FinYearFK in('" + fnlyr + "')";
                //}
                //SelectQ += " group by degree_code,f.FeeCategory,r.Batch_Year having sum(TotalAmount) <> sum(BalAmount) and sum(BalAmount) > 0";
                #endregion
                SelectQ += " select  SUM(PaidAmount) as partpaid,r.degree_code,r.Batch_Year,f.FeeCategory,(select textval from textvaltable where textcode = feecategory)+'-'+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=f.FinYearFK) as FinYear  from FT_FeeAllot f,Registration r where r.App_No=f.App_No and r.college_code='" + collegecode + "' " + commondist + "";
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
                if (sec != "")
                {
                    // SelectQ += " and   ISNULL( r.Sections,'') in ('','')";
                }
                if (paymode != "")
                {
                    SelectQ += " and f.PayMode in('" + paymode + "')";
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
                SelectQ += " group by r.App_No,r.degree_code,r.Batch_Year,f.finyearfk,f.FeeCategory having sum(TotalAmount) <> sum(BalAmount) and sum(BalAmount) > 0";

                //not paid
                #region old
                //SelectQ += " select Count(distinct f.App_No) as totcount,SUM(BalAmount)as bal,degree_code,f.FeeCategory,r.Batch_Year  from FT_FeeAllot f,Registration r where f.App_No = r.app_no  and balamount <> 0  and r.college_code='" + collegecode + "'  ";
                //if (batch != "")
                //{
                //    SelectQ += " and r.Batch_Year in('" + batch + "')";
                //}
                //if (degcode != "")
                //{
                //    SelectQ += " and r.degree_code in('" + degcode + "')";
                //}
                //if (feecat != "")
                //{
                //    SelectQ += " and f.FeeCategory in('" + feecat + "')";
                //}
                //if (sec != "")
                //{
                //    // SelectQ += " and   ISNULL( r.Sections,'') in ('','')";
                //}
                //if (paymode != "")
                //{
                //    SelectQ += " and f.PayMode in('" + paymode + "')";
                //}
                //if (hedgid != "")
                //{
                //    SelectQ += "  and f.HeaderFK in('" + hedgid + "')";
                //}
                //if (ledgid != "")
                //{
                //    SelectQ += " and f.LedgerFK in('" + ledgid + "')";
                //}
                //if (fnlyr != "")
                //{
                //    SelectQ += " and f.FinYearFK in('" + fnlyr + "')";
                //}
                //SelectQ += " group by degree_code,f.FeeCategory,r.Batch_Year having sum(TotalAmount) = sum(BalAmount)";
                #endregion
                SelectQ += " select  SUM(BalAmount) as bal,r.degree_code,r.Batch_Year,f.FeeCategory,(select textval from textvaltable where textcode = feecategory)+'-'+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=f.FinYearFK) as FinYear  from FT_FeeAllot f,Registration r where r.App_No=f.App_No and r.college_code='" + collegecode + "' " + commondist + "";
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
                if (sec != "")
                {
                    // SelectQ += " and   ISNULL( r.Sections,'') in ('','')";
                }
                if (paymode != "")
                {
                    SelectQ += " and f.PayMode in('" + paymode + "')";
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
                SelectQ += " group by r.App_No,r.degree_code,r.Batch_Year,f.finyearfk,f.FeeCategory having sum(TotalAmount) = sum(BalAmount)";
                #endregion
            }
            else
            {
                #region withdate old

                //  SelectQ = "select Count(distinct f.App_No) as totcount,r.degree_code,r.Batch_Year,f.FeeCategory from FT_FeeAllot f,Registration r where r.App_No=f.App_No  and r.college_code='" + collegecode + "'";
                //  if (batch != "")
                //  {
                //      SelectQ += " and r.Batch_Year in('" + batch + "')";
                //  }
                //  if (degcode != "")
                //  {
                //      SelectQ += " and r.degree_code in('" + degcode + "')";
                //  }
                //  if (feecat != "")
                //  {
                //      SelectQ += " and f.FeeCategory in('" + feecat + "')";
                //  }
                //  if (sec != "")
                //  {
                //      // SelectQ += " and   ISNULL( r.Sections,'') in ('','')";
                //  }
                //  if (paymode != "")
                //  {
                //      SelectQ += " and f.PayMode in('" + paymode + "')";
                //  }
                //  if (hedgid != "")
                //  {
                //      SelectQ += "  and f.HeaderFK in('" + hedgid + "')";
                //  }
                //  if (ledgid != "")
                //  {
                //      SelectQ += " and f.LedgerFK in('" + ledgid + "')";
                //  }
                //  if (fnlyr != "")
                //  {
                //      SelectQ += " and f.FinYearFK in('" + fnlyr + "')";
                //  }
                ////  SelectQ += "  and fd.TransDate between '" + fromdate + "' and '" + todate + "'";

                //  SelectQ += " group by r.degree_code,r.Batch_Year,f.FeeCategory order by r.degree_code";

                //  //fully paid             

                //  SelectQ += " select  SUM(PaidAmount) as paid,r.degree_code,r.Batch_Year,f.FeeCategory from FT_FeeAllot f,Registration r ,FT_FinDailyTransaction fd where fd.App_No =f.app_no and r.App_No =fd.app_no and fd.FeeCategory =f.FeeCategory and fd.LedgerFK=f.LedgerFK and fd.HeaderFK =f.HeaderFK and  r.App_No=f.App_No and r.college_code='" + collegecode + "' ";
                //  if (batch != "")
                //  {
                //      SelectQ += " and r.Batch_Year in('" + batch + "')";
                //  }
                //  if (degcode != "")
                //  {
                //      SelectQ += " and r.degree_code in('" + degcode + "')";
                //  }
                //  if (feecat != "")
                //  {
                //      SelectQ += " and f.FeeCategory in('" + feecat + "')";
                //  }
                //  if (sec != "")
                //  {
                //      // SelectQ += " and   ISNULL( r.Sections,'') in ('','')";
                //  }
                //  if (paymode != "")
                //  {
                //      SelectQ += " and f.PayMode in('" + paymode + "')";
                //  }
                //  if (hedgid != "")
                //  {
                //      SelectQ += "  and f.HeaderFK in('" + hedgid + "')";
                //  }
                //  if (ledgid != "")
                //  {
                //      SelectQ += " and f.LedgerFK in('" + ledgid + "')";
                //  }
                //  if (fnlyr != "")
                //  {
                //      SelectQ += " and f.FinYearFK in('" + fnlyr + "')";
                //  }
                //  SelectQ += "  and fd.TransDate between '" + fromdate + "' and '" + todate + "'";
                //  SelectQ += " group by r.App_No,r.degree_code,r.Batch_Year,f.FeeCategory having sum(TotalAmount) > 0 and sum(BalAmount) = 0";
                //  //partial paid

                //  SelectQ += " select  SUM(PaidAmount) as partpaid,r.degree_code,r.Batch_Year,f.FeeCategory from FT_FeeAllot f,Registration r ,FT_FinDailyTransaction fd where fd.App_No =f.app_no and r.App_No =fd.app_no and fd.FeeCategory =f.FeeCategory and fd.LedgerFK=f.LedgerFK and fd.HeaderFK =f.HeaderFK and  r.App_No=f.App_No and r.college_code='" + collegecode + "' ";
                //  if (batch != "")
                //  {
                //      SelectQ += " and r.Batch_Year in('" + batch + "')";
                //  }
                //  if (degcode != "")
                //  {
                //      SelectQ += " and r.degree_code in('" + degcode + "')";
                //  }
                //  if (feecat != "")
                //  {
                //      SelectQ += " and f.FeeCategory in('" + feecat + "')";
                //  }
                //  if (sec != "")
                //  {
                //      // SelectQ += " and   ISNULL( r.Sections,'') in ('','')";
                //  }
                //  if (paymode != "")
                //  {
                //      SelectQ += " and f.PayMode in('" + paymode + "')";
                //  }
                //  if (hedgid != "")
                //  {
                //      SelectQ += "  and f.HeaderFK in('" + hedgid + "')";
                //  }
                //  if (ledgid != "")
                //  {
                //      SelectQ += " and f.LedgerFK in('" + ledgid + "')";
                //  }
                //  if (fnlyr != "")
                //  {
                //      SelectQ += " and f.FinYearFK in('" + fnlyr + "')";
                //  }
                //  SelectQ += "  and fd.TransDate between '" + fromdate + "' and '" + todate + "'";
                //  SelectQ += " group by r.App_No,r.degree_code,r.Batch_Year,f.FeeCategory having sum(TotalAmount) <> sum(BalAmount) and sum(BalAmount) > 0";

                //  //not paid            
                //  SelectQ += " select  SUM(BalAmount) as bal,r.degree_code,r.Batch_Year,f.FeeCategory from FT_FeeAllot f,Registration r ,FT_FinDailyTransaction fd where fd.App_No =f.app_no and r.App_No =fd.app_no and fd.FeeCategory =f.FeeCategory and fd.LedgerFK=f.LedgerFK and fd.HeaderFK =f.HeaderFK and  r.App_No=f.App_No and r.college_code='" + collegecode + "' ";
                //  if (batch != "")
                //  {
                //      SelectQ += " and r.Batch_Year in('" + batch + "')";
                //  }
                //  if (degcode != "")
                //  {
                //      SelectQ += " and r.degree_code in('" + degcode + "')";
                //  }
                //  if (feecat != "")
                //  {
                //      SelectQ += " and f.FeeCategory in('" + feecat + "')";
                //  }
                //  if (sec != "")
                //  {
                //      // SelectQ += " and   ISNULL( r.Sections,'') in ('','')";
                //  }
                //  if (paymode != "")
                //  {
                //      SelectQ += " and f.PayMode in('" + paymode + "')";
                //  }
                //  if (hedgid != "")
                //  {
                //      SelectQ += "  and f.HeaderFK in('" + hedgid + "')";
                //  }
                //  if (ledgid != "")
                //  {
                //      SelectQ += " and f.LedgerFK in('" + ledgid + "')";
                //  }
                //  if (fnlyr != "")
                //  {
                //      SelectQ += " and f.FinYearFK in('" + fnlyr + "')";
                //  }
                //  SelectQ += "  and fd.TransDate between '" + fromdate + "' and '" + todate + "'";
                //  SelectQ += " group by r.App_No,r.degree_code,r.Batch_Year,f.FeeCategory having sum(TotalAmount) = sum(BalAmount)";
                #endregion

                if (cbbefore.Checked == false)
                {
                    //not before admission
                    #region with date

                    //total count
                    SelectQ = "select Count(distinct f.App_No) as totcount,r.degree_code,r.Batch_Year,f.FeeCategory,(select textval from textvaltable where textcode = feecategory)+'-'+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=f.FinYearFK) as FinYear  from FT_FeeAllot f,Registration r where r.App_No=f.App_No  and r.college_code='" + collegecode + "' " + commondist + "";
                    if (batch != "")
                        SelectQ += " and r.Batch_Year in('" + batch + "')";

                    if (degcode != "")
                        SelectQ += " and r.degree_code in('" + degcode + "')";

                    if (feecat != "")
                        SelectQ += " and f.FeeCategory in('" + feecat + "')";

                    if (sec != "")
                        // SelectQ += " and   ISNULL( r.Sections,'') in ('','')";

                        if (paymode != "")
                            SelectQ += " and f.PayMode in('" + paymode + "')";

                    if (hedgid != "")
                        SelectQ += "  and f.HeaderFK in('" + hedgid + "')";

                    if (ledgid != "")
                        SelectQ += " and f.LedgerFK in('" + ledgid + "')";

                    if (fnlyr != "")
                        SelectQ += " and f.FinYearFK in('" + fnlyr + "')";
                    SelectQ += " group by r.degree_code,r.Batch_Year,f.FeeCategory,f.finyearfk order by r.degree_code";

                    SelectQ += " select SUM(TotalAmount) as Demand, f.App_No,r.degree_code,r.Batch_Year,f.FeeCategory,(select textval from textvaltable where textcode = feecategory)+'-'+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=f.FinYearFK) as FinYear  from FT_FeeAllot f,Registration r where f.App_No =r.App_No " + commondist + " ";
                    if (batch != "")
                        SelectQ += " and r.Batch_Year in('" + batch + "')";

                    if (degcode != "")
                        SelectQ += " and r.degree_code in('" + degcode + "')";

                    if (feecat != "")
                        SelectQ += " and f.FeeCategory in('" + feecat + "')";

                    if (sec != "")
                        // SelectQ += " and   ISNULL( r.Sections,'') in ('','')";

                        if (paymode != "")
                            SelectQ += " and f.PayMode in('" + paymode + "')";

                    if (hedgid != "")
                        SelectQ += "  and f.HeaderFK in('" + hedgid + "')";

                    if (ledgid != "")
                        SelectQ += " and f.LedgerFK in('" + ledgid + "')";

                    if (fnlyr != "")
                        SelectQ += " and f.FinYearFK in('" + fnlyr + "')";
                    SelectQ += "  group by f.App_No,r.degree_code,r.Batch_Year,f.FeeCategory,f.finyearfk";


                    //paid
                    SelectQ += " select SUM(Debit) as Paid, f.App_No,f.FeeCategory,r.degree_code,r.Batch_Year,(select textval from textvaltable where textcode = feecategory)+'-'+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=f.FinYearFK) as FinYear  from FT_FinDailyTransaction f,Registration r where f.App_No =r.App_No " + commondist + " and ISNULL(IsCanceled ,'0')='0' and ISNULL(IsCollected,'0')='1'";
                    if (usBasedRights == true)
                        SelectQ += " and f.EntryUserCode in('" + usercode + "')";
                    if (batch != "")
                        SelectQ += " and r.Batch_Year in('" + batch + "')";

                    if (degcode != "")
                        SelectQ += " and r.degree_code in('" + degcode + "')";

                    if (feecat != "")
                        SelectQ += " and f.FeeCategory in('" + feecat + "')";

                    if (sec != "")
                        // SelectQ += " and   ISNULL( r.Sections,'') in ('','')";

                        if (paymode != "")
                            SelectQ += " and f.PayMode in('" + paymode + "')";

                    if (hedgid != "")
                        SelectQ += "  and f.HeaderFK in('" + hedgid + "')";

                    if (ledgid != "")
                        SelectQ += " and f.LedgerFK in('" + ledgid + "')";

                    if (fnlyr != "")
                        SelectQ += " and f.FinYearFK in('" + fnlyr + "')";
                    SelectQ += " and TransDate between '" + fromdate + "' and '" + todate + "'";
                    //between '" + fromdate + "' and '" + todate + "'";
                    //<='" + todate + "'";
                    SelectQ += " group by f.App_No,f.FeeCategory,r.degree_code,r.Batch_Year,f.finyearfk order by f.App_No,f.FeeCategory asc";


                    #endregion
                }
                else
                {
                    #region with date

                    //total count
                    SelectQ = " Select Cnt.degree_code,Cnt.batch_year,Cnt.feecategory,count(*) as totcount from ( select distinct f.App_No ,r.degree_code,r.Batch_Year,f.FeeCategory,(select textval from textvaltable where textcode = feecategory)+'-'+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=f.FinYearFK) as FinYear  from FT_FeeAllot f,Registration r where r.App_No=f.App_No  and r.college_code='" + collegecode + "' " + commondist + "";
                    if (batch != "")
                        SelectQ += " and r.Batch_Year in('" + batch + "')";

                    if (degcode != "")
                        SelectQ += " and r.degree_code in('" + degcode + "')";

                    if (feecat != "")
                        SelectQ += " and f.FeeCategory in('" + feecat + "')";

                    if (sec != "")
                        // SelectQ += " and   ISNULL( r.Sections,'') in ('','')";

                        if (paymode != "")
                            SelectQ += " and f.PayMode in('" + paymode + "')";

                    if (hedgid != "")
                        SelectQ += "  and f.HeaderFK in('" + hedgid + "')";

                    if (ledgid != "")
                        SelectQ += " and f.LedgerFK in('" + ledgid + "')";

                    if (fnlyr != "")
                        SelectQ += " and f.FinYearFK in('" + fnlyr + "') union all  select distinct f.App_No ,r.degree_code,r.Batch_Year,f.FeeCategory,(select textval from textvaltable where textcode = feecategory)+'-'+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=f.FinYearFK) as FinYear  from FT_FeeAllot f,applyn r where r.App_No=f.App_No and isnull(is_enroll,'0')<>'2'  and r.college_code='" + collegecode + "'";
                    if (batch != "")
                        SelectQ += " and r.Batch_Year in('" + batch + "')";

                    if (degcode != "")
                        SelectQ += " and r.degree_code in('" + degcode + "')";

                    if (feecat != "")
                        SelectQ += " and f.FeeCategory in('" + feecat + "')";

                    if (sec != "")
                        // SelectQ += " and   ISNULL( r.Sections,'') in ('','')";

                        if (paymode != "")
                            SelectQ += " and f.PayMode in('" + paymode + "')";

                    if (hedgid != "")
                        SelectQ += "  and f.HeaderFK in('" + hedgid + "')";

                    if (ledgid != "")
                        SelectQ += " and f.LedgerFK in('" + ledgid + "')";

                    if (fnlyr != "")
                        SelectQ += " and f.FinYearFK in('" + fnlyr + "')) as Cnt";
                    SelectQ += " group by Cnt.degree_code,Cnt.batch_year,Cnt.feecategory,f.finyearfk order by Cnt.degree_code ";


                    //allot

                    SelectQ += " select SUM(TotalAmount) as Demand, Cnt.App_No,Cnt.degree_code,Cnt.Batch_Year,Cnt.FeeCategory,(select textval from textvaltable where textcode = feecategory)+'-'+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=f.FinYearFK) as FinYear  from ( select TotalAmount, f.App_No,r.degree_code,r.Batch_Year,f.FeeCategory from FT_FeeAllot f,Registration r where f.App_No =r.App_No " + commondist + " ";
                    if (batch != "")
                        SelectQ += " and r.Batch_Year in('" + batch + "')";

                    if (degcode != "")
                        SelectQ += " and r.degree_code in('" + degcode + "')";

                    if (feecat != "")
                        SelectQ += " and f.FeeCategory in('" + feecat + "')";

                    if (sec != "")
                        // SelectQ += " and   ISNULL( r.Sections,'') in ('','')";

                        if (paymode != "")
                            SelectQ += " and f.PayMode in('" + paymode + "')";

                    if (hedgid != "")
                        SelectQ += "  and f.HeaderFK in('" + hedgid + "')";

                    if (ledgid != "")
                        SelectQ += " and f.LedgerFK in('" + ledgid + "')";

                    if (fnlyr != "")
                        SelectQ += " and f.FinYearFK in('" + fnlyr + "')  union all  select TotalAmount, f.App_No,r.degree_code,r.Batch_Year,f.FeeCategory,(select textval from textvaltable where textcode = feecategory)+'-'+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=f.FinYearFK) as FinYear  from FT_FeeAllot f,applyn r where f.App_No =r.App_No and isnull(is_enroll,'0')<>'2'";
                    if (batch != "")
                        SelectQ += " and r.Batch_Year in('" + batch + "')";

                    if (degcode != "")
                        SelectQ += " and r.degree_code in('" + degcode + "')";

                    if (feecat != "")
                        SelectQ += " and f.FeeCategory in('" + feecat + "')";

                    if (sec != "")
                        // SelectQ += " and   ISNULL( r.Sections,'') in ('','')";

                        if (paymode != "")
                            SelectQ += " and f.PayMode in('" + paymode + "')";

                    if (hedgid != "")
                        SelectQ += "  and f.HeaderFK in('" + hedgid + "')";

                    if (ledgid != "")
                        SelectQ += " and f.LedgerFK in('" + ledgid + "')";

                    if (fnlyr != "")
                        SelectQ += " and f.FinYearFK in('" + fnlyr + "') ";
                    SelectQ += "  ) as Cnt group by Cnt.App_No,Cnt.degree_code,Cnt.Batch_Year,Cnt.FeeCategory,f.finyearfk ";


                    //paid
                    SelectQ += " select SUM(Debit) as Paid, Cnt.App_No,Cnt.FeeCategory,Cnt.degree_code,Cnt.Batch_Year from(  select Debit, f.App_No,f.FeeCategory,r.degree_code,r.Batch_Year,(select textval from textvaltable where textcode = feecategory)+'-'+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=f.FinYearFK) as FinYear  from FT_FinDailyTransaction f,Registration r where f.App_No =r.App_No " + commondist + " and ISNULL(IsCanceled ,'0')='0' and ISNULL(IsCollected,'0')='1'";
                    if (usBasedRights == true)
                        SelectQ += " and f.EntryUserCode in('" + usercode + "')";
                    if (batch != "")
                        SelectQ += " and r.Batch_Year in('" + batch + "')";

                    if (degcode != "")
                        SelectQ += " and r.degree_code in('" + degcode + "')";

                    if (feecat != "")
                        SelectQ += " and f.FeeCategory in('" + feecat + "')";

                    if (sec != "")
                        // SelectQ += " and   ISNULL( r.Sections,'') in ('','')";

                        if (paymode != "")
                            SelectQ += " and f.PayMode in('" + paymode + "')";

                    if (hedgid != "")
                        SelectQ += "  and f.HeaderFK in('" + hedgid + "')";

                    if (ledgid != "")
                        SelectQ += " and f.LedgerFK in('" + ledgid + "')";

                    if (fnlyr != "")
                        SelectQ += " and f.FinYearFK in('" + fnlyr + "')";
                    SelectQ += " and TransDate between '" + fromdate + "' and '" + todate + "'  union all select Debit, f.App_No,f.FeeCategory,r.degree_code,r.Batch_Year,(select textval from textvaltable where textcode = feecategory)+'-'+(select Convert(varchar(4),YEAR(FinYearStart))+'-'+Convert(varchar(4),YEAR(FinYearEnd)) from FM_FinYearMaster where FinYearPK=f.FinYearFK) as FinYear  from FT_FinDailyTransaction f,applyn r where f.App_No =r.App_No  and ISNULL(IsCanceled ,'0')='0' and ISNULL(IsCollected,'0')='1' and isnull(is_enroll,'0')<>'2'";
                    if (usBasedRights == true)
                        SelectQ += " and f.EntryUserCode in('" + usercode + "')";
                    if (batch != "")
                        SelectQ += " and r.Batch_Year in('" + batch + "')";

                    if (degcode != "")
                        SelectQ += " and r.degree_code in('" + degcode + "')";

                    if (feecat != "")
                        SelectQ += " and f.FeeCategory in('" + feecat + "')";

                    if (sec != "")
                        // SelectQ += " and   ISNULL( r.Sections,'') in ('','')";

                        if (paymode != "")
                            SelectQ += " and f.PayMode in('" + paymode + "')";

                    if (hedgid != "")
                        SelectQ += "  and f.HeaderFK in('" + hedgid + "')";

                    if (ledgid != "")
                        SelectQ += " and f.LedgerFK in('" + ledgid + "')";

                    if (fnlyr != "")
                        SelectQ += " and f.FinYearFK in('" + fnlyr + "')";
                    SelectQ += " and TransDate between '" + fromdate + "' and '" + todate + "'";
                    SelectQ += ") as Cnt group by Cnt.App_No,Cnt.FeeCategory,Cnt.degree_code,Cnt.Batch_Year,f.finyearfk order by Cnt.App_No,Cnt.FeeCategory asc";



                    #endregion
                }
            }
            SelectQ = SelectQ + " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code ='" + collegecode + "'";

            SelectQ += "  select (c.Course_Name+'-'+dt.Dept_Name) as Depatname,d.Degree_Code from Degree d,Course c,Department dt where d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code ";

            SelectQ += " select IsSchemeCode,count(IsSchemeCode) as SchemeCount,r.college_code, r.Batch_Year, r.degree_code from Registration r where isnull(r.IsSchemeAdmission,'0')='1' and isnull(r.IsSchemeCode,'')<>'' and r.college_code in ('" + collegecode + "') and r.Batch_Year in ('" + batch + "') and r.degree_code in ('" + degcode + "') and IsSchemeCode in ('" + schemecodes + "') " + commondist + " group by IsSchemeCode,r.college_code,r.Batch_Year, r.degree_code";

            dsload.Clear();
            dsload = d2.select_method_wo_parameter(SelectQ, "Text");
        }
        catch { }
        return dsload;
    }

    protected void btnsearch_Click(object sender, EventArgs e)
    {
        ds.Clear();
        ds = dsvalue();
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            if (cbdatewise.Checked == false)
            {
                loadspreadvalues();
            }
            else
            {
                loadspreadvaluesDate();
            }
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

    protected void loadspreadvalues()
    {
        try
        {
            UserbasedRights();
            #region design
            DataView dv = new DataView();
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 5;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            int check = 0;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Batch Year";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = lbldept.Text;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = lblsem.Text;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Right;
            // FpSpread1.Sheets[0].ColumnCount++;


            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Total Count";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnCount++;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Fully Paid";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Count";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnCount++;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Amount";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 1, 2);

            FpSpread1.Sheets[0].ColumnCount++;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Partially Paid";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Count";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnCount++;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Amount";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 1, 2);

            FpSpread1.Sheets[0].ColumnCount++;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Not Paid";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Count";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnCount++;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Amount";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 1, 2);


            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);

            for (int schI = 0; schI < chklScheme.Items.Count; schI++)
            {
                if (chklScheme.Items[schI].Selected)
                {
                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = chklScheme.Items[schI].Text;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                }
            }


            #endregion

            #region values
            DataView dvpaid = new DataView();
            DataView dvpartial = new DataView();
            DataView dvnotpaid = new DataView();
            Hashtable grandtotal = new Hashtable();
            DataView Dview = new DataView();
            DataView dvdbset = new DataView();
            DataView dvdept = new DataView();
            double totcount = 0;
            double paidcnt = 0;
            double paidamt = 0;
            double partcnt = 0;
            double partamt = 0;
            double notcnt = 0;
            double notamt = 0;
            //for (int sel = 0; sel < ds.Tables[0].Rows.Count; sel++)
            //{
            int serialNo = 0;
            for (int batch = 0; batch < cbl_batch.Items.Count; batch++)
            {
                if (cbl_batch.Items[batch].Selected == true)
                {
                    for (int deg = 0; deg < cbl_dept.Items.Count; deg++)
                    {
                        if (cbl_dept.Items[deg].Selected == true)
                        {
                            for (int sem = 0; sem < cbl_sem.Items.Count; sem++)
                            {
                                if (cbl_sem.Items[sem].Selected == true)
                                {
                                    ds.Tables[0].DefaultView.RowFilter = "Batch_year='" + Convert.ToString(cbl_batch.Items[batch].Value) + "' and Degree_Code='" + Convert.ToString(cbl_dept.Items[deg].Value) + "' and FeeCategory='" + Convert.ToString(cbl_sem.Items[sem].Value) + "'";
                                    dvdbset = ds.Tables[0].DefaultView;
                                    if (dvdbset.Count > 0)
                                    {
                                        for (int sel = 0; sel < dvdbset.Count; sel++)
                                        {
                                            FpSpread1.Sheets[0].RowCount++;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(++serialNo);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dvdbset[sel]["Batch_Year"]);
                                            string dept = "";
                                            if (ds.Tables[5].Rows.Count > 0)
                                            {
                                                ds.Tables[5].DefaultView.RowFilter = "degree_Code='" + Convert.ToString(dvdbset[sel]["Degree_Code"]) + "'";
                                                Dview = ds.Tables[5].DefaultView;
                                                if (Dview.Count > 0)
                                                {
                                                    dept = Convert.ToString(Dview[0]["Depatname"]);
                                                }
                                            }

                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dept;
                                            string TextName = "";
                                            if (ds.Tables[4].Rows.Count > 0)
                                            {
                                                TextName = Convert.ToString(dvdbset[sel]["FinYear"]);
                                                //ds.Tables[4].DefaultView.RowFilter = "TextCode='" + Convert.ToString(dvdbset[sel]["FeeCategory"]) + "'";
                                                //Dview = ds.Tables[4].DefaultView;
                                                //if (Dview.Count > 0)
                                                //{
                                                //    TextName = Convert.ToString(Dview[0]["TextVal"]);
                                                //}
                                            }
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = TextName;
                                            double.TryParse(Convert.ToString(dvdbset[sel]["totcount"]), out totcount);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(totcount);
                                            int indxCol = 11;
                                            for (int schI = 0; schI < chklScheme.Items.Count; schI++)
                                            {
                                                if (chklScheme.Items[schI].Selected)
                                                {
                                                    string schCode = chklScheme.Items[schI].Value.Trim();
                                                    string schCnt = "0";

                                                    if (ds.Tables[6].Rows.Count > 0)
                                                    {
                                                        ds.Tables[6].DefaultView.RowFilter = "IsSchemeCode='" + schCode + "' and college_code='" + collegecode + "' and Batch_Year='" + Convert.ToString(dvdbset[sel]["Batch_Year"]) + "' and degree_code='" + Convert.ToString(dvdbset[sel]["Degree_Code"]) + "'";
                                                        DataView dvSchCnt = ds.Tables[6].DefaultView;
                                                        if (dvSchCnt.Count > 0)
                                                        {
                                                            schCnt = Convert.ToString(dvSchCnt[0]["SchemeCount"]);
                                                        }
                                                    }
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, indxCol].Text = schCnt;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, indxCol].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, indxCol].VerticalAlign = VerticalAlign.Middle;
                                                    indxCol++;
                                                }
                                            }
                                            //total count -grand total
                                            if (!grandtotal.ContainsKey(4))
                                                grandtotal.Add(4, Convert.ToString(totcount));
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(grandtotal[4]), out amount);
                                                amount += totcount;
                                                grandtotal.Remove(4);
                                                grandtotal.Add(4, Convert.ToString(amount));
                                            }
                                            if (ds.Tables[1].Rows.Count > 0)
                                            {
                                                ds.Tables[1].DefaultView.RowFilter = "Batch_year='" + Convert.ToString(cbl_batch.Items[batch].Value) + "' and degree_code='" + Convert.ToString(dvdbset[sel]["Degree_Code"]) + "' and FeeCategory='" + Convert.ToString(cbl_sem.Items[sem].Value) + "' and FinYear='" + TextName + "'";
                                                dvpaid = ds.Tables[1].DefaultView;
                                                double tottalpaidAmt = 0;
                                                if (dvpaid.Count > 0)
                                                {
                                                    DataTable dt = new DataTable();
                                                    dt = dvpaid.ToTable();
                                                    double.TryParse(Convert.ToString(dt.Compute("sum(paid)", "")), out tottalpaidAmt);
                                                    //  double tot = Convert.ToDouble(dt.Compute("sum(paid)", ""));
                                                    double tot = tottalpaidAmt;
                                                    int count = Convert.ToInt32(dvpaid.Count);
                                                    double.TryParse(Convert.ToString(count), out paidcnt);
                                                    double.TryParse(Convert.ToString(tot), out paidamt);

                                                }
                                            }
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(paidcnt); ;
                                            //paid count -grand total
                                            if (!grandtotal.ContainsKey(5))
                                                grandtotal.Add(5, Convert.ToString(paidcnt));
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(grandtotal[5]), out amount);
                                                amount += paidcnt;
                                                grandtotal.Remove(5);
                                                grandtotal.Add(5, Convert.ToString(amount));
                                            }
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(paidamt);
                                            //paid amt -grand total
                                            if (!grandtotal.ContainsKey(6))
                                                grandtotal.Add(6, Convert.ToString(paidamt));
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(grandtotal[6]), out amount);
                                                amount += paidamt;
                                                grandtotal.Remove(6);
                                                grandtotal.Add(6, Convert.ToString(amount));
                                            }
                                            paidcnt = 0;
                                            paidamt = 0;
                                            double totalParamt = 0;
                                            if (ds.Tables[2].Rows.Count > 0)
                                            {
                                                ds.Tables[2].DefaultView.RowFilter = "Batch_year='" + Convert.ToString(cbl_batch.Items[batch].Value) + "' and degree_code='" + Convert.ToString(dvdbset[sel]["Degree_Code"]) + "' and FeeCategory='" + Convert.ToString(cbl_sem.Items[sem].Value) + "'  and FinYear='" + TextName + "'";
                                                dvpartial = ds.Tables[2].DefaultView;
                                                if (dvpartial.Count > 0)
                                                {
                                                    DataTable dt = new DataTable();
                                                    dt = dvpartial.ToTable();
                                                    double.TryParse(Convert.ToString(dt.Compute("sum(partpaid)", "")), out totalParamt);
                                                    // double tot = Convert.ToDouble(dt.Compute("sum(partpaid)", ""));
                                                    double tot = totalParamt;
                                                    int count = Convert.ToInt32(dvpartial.Count);
                                                    double.TryParse(Convert.ToString(count), out partcnt);
                                                    double.TryParse(Convert.ToString(tot), out partamt);

                                                    //double.TryParse(Convert.ToString(dvpartial[0]["totcount"]), out partcnt);
                                                    //double.TryParse(Convert.ToString(dvpartial[0]["partamt"]), out partamt);
                                                }
                                            }
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(partcnt);
                                            //part count -grand total
                                            if (!grandtotal.ContainsKey(7))
                                                grandtotal.Add(7, Convert.ToString(partcnt));
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(grandtotal[7]), out amount);
                                                amount += partcnt;
                                                grandtotal.Remove(7);
                                                grandtotal.Add(7, Convert.ToString(amount));
                                            }
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(partamt);
                                            //part amt -grand total
                                            if (!grandtotal.ContainsKey(8))
                                                grandtotal.Add(8, Convert.ToString(partamt));
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(grandtotal[8]), out amount);
                                                amount += partamt;
                                                grandtotal.Remove(8);
                                                grandtotal.Add(8, Convert.ToString(amount));
                                            }
                                            partcnt = 0;
                                            partamt = 0;
                                            double totBalAmt = 0;
                                            if (ds.Tables[3].Rows.Count > 0)
                                            {
                                                ds.Tables[3].DefaultView.RowFilter = "Batch_year='" + Convert.ToString(cbl_batch.Items[batch].Value) + "' and degree_code='" + Convert.ToString(dvdbset[sel]["Degree_Code"]) + "' and FeeCategory='" + Convert.ToString(cbl_sem.Items[sem].Value) + "'  and FinYear='" + TextName + "'";
                                                dvnotpaid = ds.Tables[3].DefaultView;
                                                if (dvnotpaid.Count > 0)
                                                {
                                                    DataTable dt = new DataTable();
                                                    dt = dvnotpaid.ToTable();
                                                    double.TryParse(Convert.ToString(dt.Compute("sum(bal)", "")), out totBalAmt);
                                                    // double tot = Convert.ToDouble(dt.Compute("sum(bal)", ""));
                                                    double tot = totBalAmt;
                                                    int count = Convert.ToInt32(dvnotpaid.Count);
                                                    double.TryParse(Convert.ToString(count), out notcnt);
                                                    double.TryParse(Convert.ToString(tot), out notamt);

                                                    //double.TryParse(Convert.ToString(dvnotpaid[0]["totcount"]), out notcnt);
                                                    //double.TryParse(Convert.ToString(dvnotpaid[0]["bal"]), out notamt);
                                                }
                                            }
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(notcnt);
                                            //notpaid count -grand total
                                            if (!grandtotal.ContainsKey(9))
                                                grandtotal.Add(9, Convert.ToString(notcnt));
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(grandtotal[9]), out amount);
                                                amount += notcnt;
                                                grandtotal.Remove(9);
                                                grandtotal.Add(9, Convert.ToString(amount));
                                            }
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(notamt);
                                            if (!grandtotal.ContainsKey(10))
                                                grandtotal.Add(10, Convert.ToString(notamt));
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(grandtotal[10]), out amount);
                                                amount += notamt;
                                                grandtotal.Remove(10);
                                                grandtotal.Add(10, Convert.ToString(amount));
                                            }
                                            notcnt = 0;
                                            notamt = 0;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }



            // }
            FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            int schColCnt = 0;
            for (int i = 11; i < FpSpread1.Sheets[0].ColumnCount; i++)
            {
                schColCnt++;
                FpSpread1.Sheets[0].SetColumnMerge(i, FarPoint.Web.Spread.Model.MergePolicy.Always);
            }

            FpSpread1.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
            FpSpread1.Sheets[0].Rows.Count++;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 4);
            double grandvalue = 0;
            for (int j = 4; j < (FpSpread1.Sheets[0].ColumnCount - schColCnt); j++)
            {
                double.TryParse(Convert.ToString(grandtotal[j]), out grandvalue);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalue);
            }

            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            FpSpread1.SaveChanges();
            divspread.Visible = true;
            print.Visible = true;
            imgdiv2.Visible = false;
            lbl_alert.Text = "";
            lblvalidation1.Text = "";

            #endregion
        }
        catch { }
    }

    protected void loadspreadvaluesDate()
    {
        try
        {
            UserbasedRights();
            #region design
            DataView dv = new DataView();
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 5;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            int check = 0;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Batch Year";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = lbldept.Text;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = lblsem.Text;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Right;
            // FpSpread1.Sheets[0].ColumnCount++;


            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Total Count";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnCount++;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Fully Paid";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Count";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnCount++;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Amount";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 1, 2);

            FpSpread1.Sheets[0].ColumnCount++;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Partially Paid";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Count";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnCount++;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Amount";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 1, 2);

            FpSpread1.Sheets[0].ColumnCount++;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Not Paid";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Count";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnCount++;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Amount";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 1, 2);


            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);


            for (int schI = 0; schI < chklScheme.Items.Count; schI++)
            {
                if (chklScheme.Items[schI].Selected)
                {
                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = chklScheme.Items[schI].Text;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                }
            }
            #endregion

            #region values
            DataView dvpaid = new DataView();
            DataView dvpartial = new DataView();
            DataView dvnotpaid = new DataView();
            Hashtable grandtotal = new Hashtable();
            Hashtable total = new Hashtable();
            DataView Dview = new DataView();
            DataView dvdbset = new DataView();
            DataView dvdept = new DataView();
            DataView dvallotCnt = new DataView();
            DataView dvpaidCnt = new DataView();
            double totcount = 0;
            //double paidcnt = 0;
            //double paidamt = 0;
            //double partcnt = 0;
            //double partamt = 0;
            //double notcnt = 0;
            //double notamt = 0;
            //for (int sel = 0; sel < ds.Tables[0].Rows.Count; sel++)
            //{
            int sno = 0;
            for (int deg = 0; deg < cbl_dept.Items.Count; deg++)
            {
                if (cbl_dept.Items[deg].Selected == true)
                {
                    sno++;
                    for (int batch = 0; batch < cbl_batch.Items.Count; batch++)
                    {
                        if (cbl_batch.Items[batch].Selected == true)
                        {
                            for (int sem = 0; sem < cbl_sem.Items.Count; sem++)
                            {
                                if (cbl_sem.Items[sem].Selected == true)
                                {
                                    ds.Tables[0].DefaultView.RowFilter = "Batch_year='" + Convert.ToString(cbl_batch.Items[batch].Value) + "' and Degree_Code='" + Convert.ToString(cbl_dept.Items[deg].Value) + "' and FeeCategory='" + Convert.ToString(cbl_sem.Items[sem].Value) + "'";
                                    dvdbset = ds.Tables[0].DefaultView;
                                    if (dvdbset.Count > 0)
                                    {
                                        for (int sel = 0; sel < dvdbset.Count; sel++)
                                        {
                                            #region

                                            FpSpread1.Sheets[0].RowCount++;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dvdbset[sel]["Batch_Year"]);
                                            string dept = "";
                                            if (ds.Tables[4].Rows.Count > 0)
                                            {
                                                ds.Tables[4].DefaultView.RowFilter = "degree_Code='" + Convert.ToString(dvdbset[sel]["Degree_Code"]) + "'";
                                                Dview = ds.Tables[4].DefaultView;
                                                if (Dview.Count > 0)
                                                {
                                                    dept = Convert.ToString(Dview[0]["Depatname"]);
                                                }
                                            }

                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dept;
                                            string TextName = "";
                                            if (ds.Tables[3].Rows.Count > 0)
                                            {
                                                TextName = Convert.ToString(dvdbset[sel]["FinYear"]);
                                                //ds.Tables[3].DefaultView.RowFilter = "TextCode='" + Convert.ToString(dvdbset[sel]["FeeCategory"]) + "'";
                                                //Dview = ds.Tables[3].DefaultView;
                                                //if (Dview.Count > 0)
                                                //{
                                                //    TextName = Convert.ToString(Dview[0]["TextVal"]);
                                                //}
                                            }
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = TextName;
                                            double.TryParse(Convert.ToString(dvdbset[sel]["totcount"]), out totcount);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(totcount);
                                            int indxCol = 11;
                                            for (int schI = 0; schI < chklScheme.Items.Count; schI++)
                                            {
                                                if (chklScheme.Items[schI].Selected)
                                                {
                                                    string schCode = chklScheme.Items[schI].Value.Trim();
                                                    string schCnt = "0";

                                                    if (ds.Tables[5].Rows.Count > 0)
                                                    {
                                                        ds.Tables[5].DefaultView.RowFilter = "IsSchemeCode='" + schCode + "' and college_code='" + collegecode + "' and Batch_Year='" + Convert.ToString(dvdbset[sel]["Batch_Year"]) + "' and degree_code='" + Convert.ToString(dvdbset[sel]["Degree_Code"]) + "'";
                                                        DataView dvSchCnt = ds.Tables[5].DefaultView;
                                                        if (dvSchCnt.Count > 0)
                                                        {
                                                            schCnt = Convert.ToString(dvSchCnt[0]["SchemeCount"]);
                                                        }
                                                    }
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, indxCol].Text = schCnt;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, indxCol].HorizontalAlign = HorizontalAlign.Center;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, indxCol].VerticalAlign = VerticalAlign.Middle;
                                                    indxCol++;
                                                }
                                            }
                                            //total count -grand total
                                            if (!grandtotal.ContainsKey(4))
                                                grandtotal.Add(4, Convert.ToString(totcount));
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(grandtotal[4]), out amount);
                                                amount += totcount;
                                                grandtotal.Remove(4);
                                                grandtotal.Add(4, Convert.ToString(amount));
                                            }
                                            //////total count of the student
                                            //double totcount = 0;
                                            double paidcnt = 0;
                                            double paidamt = 0;
                                            double partcnt = 0;
                                            double partamt = 0;
                                            double notcnt = 0;
                                            double notamt = 0;
                                            ds.Tables[1].DefaultView.RowFilter = "Batch_year='" + Convert.ToString(dvdbset[sel]["Batch_year"]) + "' and Degree_Code='" + Convert.ToString(dvdbset[sel]["Degree_Code"]) + "' and FeeCategory='" + Convert.ToString(dvdbset[sel]["FeeCategory"]) + "' and FinYear='" + TextName + "'";
                                            dvallotCnt = ds.Tables[1].DefaultView;
                                            if (dvallotCnt.Count > 0)
                                            {
                                                for (int dlrow = 0; dlrow < dvallotCnt.Count; dlrow++)
                                                {
                                                    double DemandAmt = 0;
                                                    double paidAmt = 0;
                                                    double.TryParse(Convert.ToString(dvallotCnt[dlrow]["Demand"]), out DemandAmt);

                                                    ds.Tables[2].DefaultView.RowFilter = "App_no='" + Convert.ToString(dvallotCnt[dlrow]["App_no"]) + "' and Batch_year='" + Convert.ToString(dvallotCnt[dlrow]["Batch_year"]) + "' and Degree_Code='" + Convert.ToString(dvallotCnt[dlrow]["Degree_Code"]) + "' and FeeCategory='" + Convert.ToString(dvallotCnt[dlrow]["FeeCategory"]) + "'  and FinYear='" + TextName + "'";
                                                    dvpaidCnt = ds.Tables[2].DefaultView;
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
                                                        notamt += DemandAmt;
                                                        notcnt++;
                                                    }
                                                }

                                                //fully paid
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(paidcnt); ;
                                                //paid count -grand total
                                                if (!grandtotal.ContainsKey(5))
                                                    grandtotal.Add(5, Convert.ToString(paidcnt));
                                                else
                                                {
                                                    double amount = 0;
                                                    double.TryParse(Convert.ToString(grandtotal[5]), out amount);
                                                    amount += paidcnt;
                                                    grandtotal.Remove(5);
                                                    grandtotal.Add(5, Convert.ToString(amount));
                                                }
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(paidamt);
                                                if (!grandtotal.ContainsKey(6))
                                                    grandtotal.Add(6, Convert.ToString(paidamt));
                                                else
                                                {
                                                    double amount = 0;
                                                    double.TryParse(Convert.ToString(grandtotal[6]), out amount);
                                                    amount += paidamt;
                                                    grandtotal.Remove(6);
                                                    grandtotal.Add(6, Convert.ToString(amount));
                                                }

                                                //partial 
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(partcnt);
                                                //part count -grand total
                                                if (!grandtotal.ContainsKey(7))
                                                    grandtotal.Add(7, Convert.ToString(partcnt));
                                                else
                                                {
                                                    double amount = 0;
                                                    double.TryParse(Convert.ToString(grandtotal[7]), out amount);
                                                    amount += partcnt;
                                                    grandtotal.Remove(7);
                                                    grandtotal.Add(7, Convert.ToString(amount));
                                                }
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(partamt);
                                                //part amt -grand total
                                                if (!grandtotal.ContainsKey(8))
                                                    grandtotal.Add(8, Convert.ToString(partamt));
                                                else
                                                {
                                                    double amount = 0;
                                                    double.TryParse(Convert.ToString(grandtotal[8]), out amount);
                                                    amount += partamt;
                                                    grandtotal.Remove(8);
                                                    grandtotal.Add(8, Convert.ToString(amount));
                                                }

                                                //not
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(notcnt);
                                                //notpaid count -grand total
                                                if (!grandtotal.ContainsKey(9))
                                                    grandtotal.Add(9, Convert.ToString(notcnt));
                                                else
                                                {
                                                    double amount = 0;
                                                    double.TryParse(Convert.ToString(grandtotal[9]), out amount);
                                                    amount += notcnt;
                                                    grandtotal.Remove(9);
                                                    grandtotal.Add(9, Convert.ToString(amount));
                                                }
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(notamt);
                                                if (!grandtotal.ContainsKey(10))
                                                    grandtotal.Add(10, Convert.ToString(notamt));
                                                else
                                                {
                                                    double amount = 0;
                                                    double.TryParse(Convert.ToString(grandtotal[10]), out amount);
                                                    amount += notamt;
                                                    grandtotal.Remove(10);
                                                    grandtotal.Add(10, Convert.ToString(amount));
                                                }
                                            }
                                            #region old

                                            //if (ds.Tables[1].Rows.Count > 0)
                                            //{
                                            //    ds.Tables[1].DefaultView.RowFilter = "Batch_year='" + Convert.ToString(cbl_batch.Items[batch].Value) + "' and degree_code='" + Convert.ToString(dvdbset[sel]["Degree_Code"]) + "' and FeeCategory='" + Convert.ToString(cbl_sem.Items[sem].Value) + "'";
                                            //    dvpaid = ds.Tables[1].DefaultView;
                                            //    double tottalpaidAmt = 0;
                                            //    if (dvpaid.Count > 0)
                                            //    {
                                            //        DataTable dt = new DataTable();
                                            //        dt = dvpaid.ToTable();
                                            //        double.TryParse(Convert.ToString(dt.Compute("sum(paid)", "")), out tottalpaidAmt);
                                            //        //  double tot = Convert.ToDouble(dt.Compute("sum(paid)", ""));
                                            //        double tot = tottalpaidAmt;
                                            //        int count = Convert.ToInt32(dvpaid.Count);
                                            //        double.TryParse(Convert.ToString(count), out paidcnt);
                                            //        double.TryParse(Convert.ToString(tot), out paidamt);

                                            //    }
                                            //}
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(paidcnt); ;
                                            ////paid count -grand total
                                            //if (!grandtotal.ContainsKey(5))
                                            //    grandtotal.Add(5, Convert.ToString(paidcnt));
                                            //else
                                            //{
                                            //    double amount = 0;
                                            //    double.TryParse(Convert.ToString(grandtotal[5]), out amount);
                                            //    amount += paidcnt;
                                            //    grandtotal.Remove(5);
                                            //    grandtotal.Add(5, Convert.ToString(amount));
                                            //}
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(paidamt);
                                            //paid amt -grand total
                                            //if (!grandtotal.ContainsKey(6))
                                            //    grandtotal.Add(6, Convert.ToString(paidamt));
                                            //else
                                            //{
                                            //    double amount = 0;
                                            //    double.TryParse(Convert.ToString(grandtotal[6]), out amount);
                                            //    amount += paidamt;
                                            //    grandtotal.Remove(6);
                                            //    grandtotal.Add(6, Convert.ToString(amount));
                                            //}
                                            //paidcnt = 0;
                                            //paidamt = 0;
                                            //double totalParamt = 0;
                                            //if (ds.Tables[2].Rows.Count > 0)
                                            //{
                                            //    ds.Tables[2].DefaultView.RowFilter = "Batch_year='" + Convert.ToString(cbl_batch.Items[batch].Value) + "' and degree_code='" + Convert.ToString(dvdbset[sel]["Degree_Code"]) + "' and FeeCategory='" + Convert.ToString(cbl_sem.Items[sem].Value) + "'";
                                            //    dvpartial = ds.Tables[2].DefaultView;
                                            //    if (dvpartial.Count > 0)
                                            //    {
                                            //        DataTable dt = new DataTable();
                                            //        dt = dvpartial.ToTable();
                                            //        double.TryParse(Convert.ToString(dt.Compute("sum(partpaid)", "")), out totalParamt);
                                            //        // double tot = Convert.ToDouble(dt.Compute("sum(partpaid)", ""));
                                            //        double tot = totalParamt;
                                            //        int count = Convert.ToInt32(dvpartial.Count);
                                            //        double.TryParse(Convert.ToString(count), out partcnt);
                                            //        double.TryParse(Convert.ToString(tot), out partamt);

                                            //        //double.TryParse(Convert.ToString(dvpartial[0]["totcount"]), out partcnt);
                                            //        //double.TryParse(Convert.ToString(dvpartial[0]["partamt"]), out partamt);
                                            //    }
                                            //}
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(partcnt);
                                            ////part count -grand total
                                            //if (!grandtotal.ContainsKey(7))
                                            //    grandtotal.Add(7, Convert.ToString(partcnt));
                                            //else
                                            //{
                                            //    double amount = 0;
                                            //    double.TryParse(Convert.ToString(grandtotal[7]), out amount);
                                            //    amount += partcnt;
                                            //    grandtotal.Remove(7);
                                            //    grandtotal.Add(7, Convert.ToString(amount));
                                            //}
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(partamt);
                                            ////part amt -grand total
                                            //if (!grandtotal.ContainsKey(8))
                                            //    grandtotal.Add(8, Convert.ToString(partamt));
                                            //else
                                            //{
                                            //    double amount = 0;
                                            //    double.TryParse(Convert.ToString(grandtotal[8]), out amount);
                                            //    amount += partamt;
                                            //    grandtotal.Remove(8);
                                            //    grandtotal.Add(8, Convert.ToString(amount));
                                            //}
                                            //partcnt = 0;
                                            //partamt = 0;
                                            //double totBalAmt = 0;
                                            //if (ds.Tables[3].Rows.Count > 0)
                                            //{
                                            //    ds.Tables[3].DefaultView.RowFilter = "Batch_year='" + Convert.ToString(cbl_batch.Items[batch].Value) + "' and degree_code='" + Convert.ToString(dvdbset[sel]["Degree_Code"]) + "' and FeeCategory='" + Convert.ToString(cbl_sem.Items[sem].Value) + "'";
                                            //    dvnotpaid = ds.Tables[3].DefaultView;
                                            //    if (dvnotpaid.Count > 0)
                                            //    {
                                            //        DataTable dt = new DataTable();
                                            //        dt = dvnotpaid.ToTable();
                                            //        double.TryParse(Convert.ToString(dt.Compute("sum(bal)", "")), out totBalAmt);
                                            //        // double tot = Convert.ToDouble(dt.Compute("sum(bal)", ""));
                                            //        double tot = totBalAmt;
                                            //        int count = Convert.ToInt32(dvnotpaid.Count);
                                            //        double.TryParse(Convert.ToString(count), out notcnt);
                                            //        double.TryParse(Convert.ToString(tot), out notamt);

                                            //        //double.TryParse(Convert.ToString(dvnotpaid[0]["totcount"]), out notcnt);
                                            //        //double.TryParse(Convert.ToString(dvnotpaid[0]["bal"]), out notamt);
                                            //    }
                                            //}
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(notcnt);
                                            ////notpaid count -grand total
                                            //if (!grandtotal.ContainsKey(9))
                                            //    grandtotal.Add(9, Convert.ToString(notcnt));
                                            //else
                                            //{
                                            //    double amount = 0;
                                            //    double.TryParse(Convert.ToString(grandtotal[9]), out amount);
                                            //    amount += notcnt;
                                            //    grandtotal.Remove(9);
                                            //    grandtotal.Add(9, Convert.ToString(amount));
                                            //}
                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(notamt);
                                            //if (!grandtotal.ContainsKey(10))
                                            //    grandtotal.Add(10, Convert.ToString(notamt));
                                            //else
                                            //{
                                            //    double amount = 0;
                                            //    double.TryParse(Convert.ToString(grandtotal[10]), out amount);
                                            //    amount += notamt;
                                            //    grandtotal.Remove(10);
                                            //    grandtotal.Add(10, Convert.ToString(amount));
                                            //}
                                            // notcnt = 0;
                                            //notamt = 0;
                                            #endregion
                                            #endregion
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //total
                    FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);

                    int schColCnt = 0;
                    for (int i = 11; i < FpSpread1.Sheets[0].ColumnCount; i++)
                    {
                        schColCnt++;
                        FpSpread1.Sheets[0].SetColumnMerge(i, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    }

                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 4);
                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.YellowGreen;
                    double totalvalue = 0;
                    for (int j = 4; j < (FpSpread1.Sheets[0].ColumnCount - schColCnt); j++)
                    {
                        double.TryParse(Convert.ToString(grandtotal[j]), out totalvalue);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(totalvalue);
                        if (!total.ContainsKey(j))
                            total.Add(j, Convert.ToString(totalvalue));
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(total[j]), out amount);
                            amount += totalvalue;
                            total.Remove(j);
                            total.Add(j, Convert.ToString(amount));
                        }
                    }
                    grandtotal.Clear();
                }
            }
            FpSpread1.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
            FpSpread1.Sheets[0].Rows.Count++;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 4);
            double grandvalue = 0;
            for (int j = 4; j < FpSpread1.Sheets[0].ColumnCount; j++)
            {
                double.TryParse(Convert.ToString(total[j]), out grandvalue);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalue);
            }

            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            FpSpread1.SaveChanges();
            divspread.Visible = true;
            print.Visible = true;
            imgdiv2.Visible = false;
            lbl_alert.Text = "";
            lblvalidation1.Text = "";

            #endregion
        }
        catch { }
    }

    #endregion

    #region print

    public void btnprintmaster_Click(object sender, EventArgs e)
    {
        lblvalidation1.Visible = false;
        string degreedetails = "Enrollment Setting Report";
        string pagename = "Enrollmentselection.aspx";
        Printcontrolhed.loadspreaddetails(FpSpread1, pagename, degreedetails);
        Printcontrolhed.Visible = true;
    }
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
                lblvalidation1.Text = "Please Enter Your concolidate Count Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch { }
    }

    #endregion
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
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
        lbl.Add(lblsem);
        fields.Add(0);
        fields.Add(1);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }

    // last modified 04-10-2016 sudhagar
}