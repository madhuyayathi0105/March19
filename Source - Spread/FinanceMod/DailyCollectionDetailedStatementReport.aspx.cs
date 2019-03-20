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

public partial class DailyCollectionDetailedStatementReport : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    ReuasableMethods reuse = new ReuasableMethods();
    DataSet ds = new DataSet();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static byte roll = 0;
    static bool usBasedRights = false;
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
                loadcollege();
                if (ddlcollegename.Items.Count > 0)
                    collegecode = Convert.ToString(ddlcollegename.SelectedItem.Value);
                //  loadstrm();
                bindBtch();
                binddeg();
                binddept();
                bindsem();
                bindsec();
                loadpaid();
                loadheaderandledger();
                ledgerload();
                loadfinanceyear();
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_todate.Attributes.Add("readonly", "readonly");
                loadStudenttype();
                loadfinanceUser();
                LoadIncludeSetting();
                memtype();
                //barath 15.03.17
                #region Excel print settings
                string usertype = "";
                if (usercode.Trim() != "")
                    usertype = " and usercode='" + usercode + "'";
                else if (group_user.Trim() != "")
                    usertype = " and group_code='" + group_user + "'";
                string printset = d2.GetFunction("select value from Master_Settings where settings='Excel and Pdf Print Settings' " + usertype + " ");
                if (printset != "")
                {
                    if (printset.Contains("E"))
                    {
                        lblrptname.Visible = true;
                        txtexcelname.Visible = true;
                        btnExcel.Visible = true;
                    }
                    if (printset.Contains("P"))
                    {
                        btnprintmasterhed.Visible = true;
                    }
                    if (printset == "0")
                    {
                        lblrptname.Visible = true;
                        txtexcelname.Visible = true;
                        btnExcel.Visible = true;
                        btnprintmasterhed.Visible = true;
                    }
                }
                #endregion
            }
            if (ddlcollegename.Items.Count > 0)
                collegecode = Convert.ToString(ddlcollegename.SelectedItem.Value);
        }
        catch (Exception ex) { }
    }
    #region college
    public void loadcollege()
    {
        ddlcollegename.Items.Clear();
        reuse.bindCollegeToDropDown(usercode, ddlcollegename);
    }
    protected void ddlcollegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlcollegename.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlcollegename.SelectedItem.Value);
            }
            loadstrm();
            bindBtch();
            binddeg();
            binddept();
            bindsem();
            bindsec();
            loadpaid();
            loadheaderandledger();
            ledgerload();
            loadfinanceyear();
            loadfinanceUser();
        }
        catch
        {
        }
    }
    #endregion

    #region stream
    public void loadstrm()
    {
        //ddlstream.Items.Clear();
        //reuse.bindStreamToDropDown(ddlstream, collegecode);
        //if (ddlstream.Items.Count > 0)
        //    ddlstream.Enabled = true;
        //else
        //    ddlstream.Enabled = false;

        //binddeg();
    }
    protected void ddlstream_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //try
        //{
        //    string clgvalue = ddlcollegename.SelectedItem.Value.ToString();
        //    string stream = ddlstream.SelectedItem.Text.ToString();
        //    string selqry = "select distinct c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and type  in('" + stream + "') and d.college_code='" + clgvalue + "'";
        //    ds.Clear();
        //    ds = d2.select_method_wo_parameter(selqry, "Text");

        //    if (ds.Tables[0].Rows.Count > 0)
        //    {

        //        cbl_degree.DataSource = ds;
        //        cbl_degree.DataTextField = "Course_Name";
        //        cbl_degree.DataValueField = "Course_Id";
        //        cbl_degree.DataBind();
        //    }
        //    for (int j = 0; j < cbl_degree.Items.Count; j++)
        //    {
        //        cbl_degree.Items[j].Selected = true;
        //        cb_degree.Checked = true;
        //    }

        //    txt_degree.Text = lbldeg.Text + "(" + cbl_degree.Items.Count + ")";
        //    binddept();
        //}
        //catch { }
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
                    for (int i = 0; i < cbl_batch.Items.Count; i++)
                    {
                        cbl_batch.Items[i].Selected = true;
                    }
                    txt_batch.Text = lblbatch.Text + "(" + cbl_batch.Items.Count + ")";
                    cb_batch.Checked = true;
                }
            }
        }
        catch { }
    }
    protected void cb_batch_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_batch, cbl_batch, txt_batch, lblbatch.Text, "--Select--");
        binddeg();
        binddept();
    }
    protected void cbl_batch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_batch, cbl_batch, txt_batch, lblbatch.Text, "--Select--");
        binddeg();
        binddept();
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
            //string stream = "";
            //if (ddlstream.Items.Count > 0)
            //{
            //    if (ddlstream.SelectedItem.Text != "")
            //    {
            //        stream = ddlstream.SelectedItem.Text.ToString();
            //    }
            //}

            cbl_degree.Items.Clear();
            string clgvalue = ddlcollegename.SelectedItem.Value.ToString();
            ds.Clear();
            string selqry = "select distinct  c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code='" + clgvalue + "'";
            //if (stream != "")
            //{
            //    selqry = selqry + " and type  in('" + stream + "')";
            //}
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_id";
                cbl_degree.DataBind();
                if (cbl_degree.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_degree.Items.Count; i++)
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
        CallCheckboxChange(cb_degree, cbl_degree, txt_degree, lbldeg.Text, "--Select--");
        binddept();

    }
    protected void cbl_degree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_degree, cbl_degree, txt_degree, lbldeg.Text, "--Select--");
        binddept();

    }
    #endregion

    #region dept
    public void binddept()
    {
        try
        {
            cbl_dept.Items.Clear();
            cb_dept.Checked = false;
            txt_dept.Text = "---Select---";
            string batch = "";
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    if (batch == "")
                        batch = Convert.ToString(cbl_batch.Items[i].Text);
                    else
                        batch += "','" + Convert.ToString(cbl_batch.Items[i].Text);
                }
            }
            string degree = "";
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    if (degree == "")
                        degree = Convert.ToString(cbl_degree.Items[i].Value);

                    else
                        degree += "," + Convert.ToString(cbl_degree.Items[i].Value);
                }

            }

            string collegecode = ddlcollegename.SelectedItem.Value.ToString();
            if (batch != "" && degree != "")
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
                        for (int i = 0; i < cbl_dept.Items.Count; i++)
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
        CallCheckboxChange(cb_dept, cbl_dept, txt_dept, lbldept.Text, "--Select--");
        bindsec();
        bindsem();
    }
    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_dept, cbl_dept, txt_dept, "Department", "--Select--");
        bindsec();
        bindsem();
    }
    #endregion

    #region sem
    protected void cb_sem_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_sem, cbl_sem, txt_sem, lblsem.Text, "--Select--");
        bindsec();

    }
    protected void cbl_sem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_sem, cbl_sem, txt_sem, lblsem.Text, "--Select--");
        bindsec();

    }
    protected void bindsem()
    {
        try
        {
            string featDegcode = Convert.ToString(getCblSelectedValue(cbl_dept));
            cbl_sem.Items.Clear();
            cb_sem.Checked = false;
            txt_sem.Text = "--Select--";
            ds.Clear();
            string linkName = string.Empty;
            string cbltext = string.Empty;
            d2.featDegreeCode = featDegcode;
            ds = d2.loadFeecategory(Convert.ToString(ddlcollegename.SelectedItem.Value), usercode, ref linkName);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (linkName == "Term")
                {
                    string termStr = " and( textval like'" + linkName + " 1%' or textval like'" + linkName + " 2%' or textval like'" + linkName + " 3%' or textval like'" + linkName + " 4%' or textval like'" + linkName + " 5%' or textval like'" + linkName + " 6%') ";
                    string selQ = " select  distinct  textval,textcode,len(isnull(textval,1000)) from textvaltable t where college_code='" + ddlcollegename.SelectedItem.Value + "' and textcriteria='FEECA' " + termStr + " order by len(isnull(textval,1000)),textval asc";
                    DataSet dsval = d2.select_method_wo_parameter(selQ, "Text");
                    if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
                    {
                        cbl_sem.DataSource = dsval;
                        cbl_sem.DataTextField = "TextVal";
                        cbl_sem.DataValueField = "TextCode";
                        cbl_sem.DataBind();
                    }
                }
                else
                {
                    cbl_sem.DataSource = ds;
                    cbl_sem.DataTextField = "TextVal";
                    cbl_sem.DataValueField = "TextCode";
                    cbl_sem.DataBind();
                }
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
    //        string featDegcode = Convert.ToString(getCblSelectedValue(cbl_dept));
    //        cbl_sem.Items.Clear();
    //        cb_sem.Checked = false;
    //        txt_sem.Text = "--Select--";
    //        ds.Clear();
    //        string linkName = string.Empty;
    //        string cbltext = string.Empty;
    //        d2.featDegreeCode = featDegcode;
    //        ds = d2.loadFeecategory(Convert.ToString(ddlcollegename.SelectedItem.Value), usercode, ref linkName);
    //        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
    //        {
    //            cbl_sem.DataSource = ds;
    //            cbl_sem.DataTextField = "TextVal";
    //            cbl_sem.DataValueField = "TextCode";
    //            cbl_sem.DataBind();

    //            if (cbl_sem.Items.Count > 0)
    //            {
    //                for (int i = 0; i < cbl_sem.Items.Count; i++)
    //                {
    //                    cbl_sem.Items[i].Selected = true;
    //                    cbltext = Convert.ToString(cbl_sem.Items[i].Text);
    //                }
    //                if (cbl_sem.Items.Count == 1)
    //                    txt_sem.Text = "" + linkName + "(" + cbltext + ")";
    //                else
    //                    txt_sem.Text = "" + linkName + "(" + cbl_sem.Items.Count + ")";
    //                cb_sem.Checked = true;
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
            string clgvalue = ddlcollegename.SelectedItem.Value.ToString();
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

        CallCheckboxChange(cb_sect, cbl_sect, txt_sect, "Section", "--Select--");

    }
    protected void cbl_sect_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_sect, cbl_sect, txt_sect, "Section", "--Select--");
    }
    #endregion

    #region paymentmode
    public void loadpaid()
    {
        try
        {
            chkl_paid.Items.Clear();
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
        CallCheckboxChange(chk_paid, chkl_paid, txt_paid, "Paid", "--Select--");

    }
    public void chkl_paid_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(chk_paid, chkl_paid, txt_paid, "Paid", "--Select--");

    }
    #endregion

    #region headerandledger
    public void loadheaderandledger()
    {
        try
        {
            string clgvalue = ddlcollegename.SelectedItem.Value.ToString();
            chkl_studhed.Items.Clear();
            string query = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + clgvalue + "  ";

            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chkl_studhed.DataSource = ds;
                chkl_studhed.DataTextField = "HeaderName";
                chkl_studhed.DataValueField = "HeaderPK";
                chkl_studhed.DataBind();
                for (int i = 0; i < chkl_studhed.Items.Count; i++)
                {
                    chkl_studhed.Items[i].Selected = true;
                }
                txt_studhed.Text = lblheader.Text + "(" + chkl_studhed.Items.Count + ")";
                chk_studhed.Checked = true;
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
            string clgvalue = ddlcollegename.SelectedItem.Value.ToString();
            chkl_studled.Items.Clear();
            string hed = "";
            for (int i = 0; i < chkl_studhed.Items.Count; i++)
            {
                if (chkl_studhed.Items[i].Selected == true)
                {
                    if (hed == "")
                    {
                        hed = chkl_studhed.Items[i].Value.ToString();
                    }
                    else
                    {
                        hed = hed + "','" + "" + chkl_studhed.Items[i].Value.ToString() + "";
                    }
                }
            }


            string query1 = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='0' and L.CollegeCode = " + clgvalue + "  and L.HeaderFK in('" + hed + "')  order by isnull(l.priority,1000), l.ledgerName asc ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query1, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chkl_studled.DataSource = ds;
                chkl_studled.DataTextField = "LedgerName";
                chkl_studled.DataValueField = "LedgerPK";
                chkl_studled.DataBind();
                for (int i = 0; i < chkl_studled.Items.Count; i++)
                {
                    chkl_studled.Items[i].Selected = true;
                }
                txt_studled.Text = "Ledger(" + chkl_studled.Items.Count + ")";
                chk_studled.Checked = true; ;

            }
            else
            {
                for (int i = 0; i < chkl_studled.Items.Count; i++)
                {
                    chkl_studled.Items[i].Selected = false;
                }
                txt_studled.Text = "--Select--";
                chk_studled.Checked = false; ;
            }

        }
        catch
        {
        }
    }
    public void chk_studhed_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(chk_studhed, chkl_studhed, txt_studhed, lblheader.Text, "--Select--");
        ledgerload();
    }

    public void chkl_studhed_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(chk_studhed, chkl_studhed, txt_studhed, lblheader.Text, "--Select--");
        ledgerload();
    }
    public void chk_studled_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(chk_studled, chkl_studled, txt_studled, "Ledger", "--Select--");
    }
    public void chkl_studled_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(chk_studled, chkl_studled, txt_studled, "Ledger", "--Select--");
    }
    #endregion

    #region financial year
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
        CallCheckboxListChange(chkfyear, chklsfyear, txtfyear, "Finance Year", "--Select--");

    }
    protected void chkfyear_changed(object sender, EventArgs e)
    {
        CallCheckboxChange(chkfyear, chklsfyear, txtfyear, "Finance Year", "--Select--");
    }
    #endregion

    #region finuser year
    public void loadfinanceUser()
    {
        try
        {
            collegecode = Convert.ToString(ddlcollegename.SelectedItem.Value);
            string finUser = "   select user_id,user_code from usermaster where fin_user='1' and college_code='" + collegecode + "'";
            string getfin = d2.GetFunction("select fin_user from usermaster where fin_user='1' and user_code='" + usercode.Trim() + "' and college_code='" + collegecode + "'");
            cbuser.Checked = false;
            cbluser.Items.Clear();
            txtuser.Text = "--Select--";
            ds = d2.select_method_wo_parameter(finUser, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbluser.DataSource = ds;
                cbluser.DataTextField = "user_id";
                cbluser.DataValueField = "user_code";
                cbluser.DataBind();
                int cnt = 0;
                string getSngName = string.Empty;
                if (getfin.Trim() == "1")
                {
                    for (int i = 0; i < cbluser.Items.Count; i++)
                    {
                        //cbluser.Items[i].Selected = true;
                        if (cbluser.Items[i].Value.Trim() == usercode.Trim())
                        {
                            cbluser.Items[i].Selected = true;
                            getSngName = cbluser.Items[i].Text;
                            cnt++;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < cbluser.Items.Count; i++)
                    {
                        cbluser.Items[i].Selected = true;
                        cnt++;
                    }
                }
                if (cbluser.Items.Count == cnt)
                {
                    txtuser.Text = lbluser.Text + "(" + cbluser.Items.Count + ")";
                    cbuser.Checked = true;
                }
                else
                {
                    if (cnt == 1)
                        txtuser.Text = getSngName;
                    else
                        txtuser.Text = lbluser.Text + "(" + cnt + ")";
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void cbuser_changed(object sender, EventArgs e)
    {
        CallCheckboxChange(cbuser, cbluser, txtuser, lbluser.Text, "--Select--");

    }
    protected void cbluser_selected(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbuser, cbluser, txtuser, lbluser.Text, "--Select--");
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

    protected void rbtype_Selected(object sender, EventArgs e)
    {
        if (rbtype.SelectedIndex == 0)
        {
            spreadDet.Visible = false;
            print.Visible = false;
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            divlabl.Visible = false;
        }
        else
        {
            spreadDet.Visible = false;
            print.Visible = false;
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            divlabl.Visible = false;
        }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        bool boolcheck = false;
        string fromdate = string.Empty;
        string todate = string.Empty;
        if (!cbtermwise.Checked)//without term wise
        {
            ds.Clear();
            if (!cbBillwise.Checked)
            {
                ds = dsloadDetails(ref fromdate, ref todate);
                if (ds.Tables.Count > 0 && (ds.Tables[0].Rows.Count > 0 || (ds.Tables[5].Rows.Count > 0 || ds.Tables[9].Rows.Count > 0 || ds.Tables[13].Rows.Count > 0)))
                {
                    // ds = dsFilterValues(ds, ref  fromdate, ref  todate);
                    spreadLoadDetailed(ds, ref fromdate, ref todate);
                    boolcheck = true;
                }
            }
            else
            {
                ds = dsloadDetailsBillNo(ref fromdate, ref todate);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    // ds = dsFilterValues(ds, ref  fromdate, ref  todate);
                    spreadLoadDetailedBillNo(ds, ref fromdate, ref todate);
                    boolcheck = true;
                }
            }
        }
        else//with term wise
        {
            ds.Clear();
            ds = dsloadPaid(ref fromdate, ref todate);
            //  loadPaidDetails(ds, ref fromdate, ref todate);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                loadPaidDetails(ds, ref fromdate, ref todate);
                boolcheck = true;
            }
        }
        if (!boolcheck)
        {
            lblvalidation1.Text = string.Empty;
            txtexcelname.Text = string.Empty;
            spreadDet.Visible = false;
            divlabl.Visible = false;
            print.Visible = false;
            lbl_alert.Text = "No Record Found";
            imgdiv2.Visible = true;
        }
    }

    protected bool checkMemtype()
    {
        bool boolMemtype = false;
        try
        {
            for (int mem = 0; mem < cblmem.Items.Count; mem++)
            {
                if (cblmem.Items[mem].Selected)
                {
                    if (cblmem.Items[mem].Text.Trim() == "Staff")
                    {
                    }
                    else if (cblmem.Items[mem].Text.Trim() == "Vendor")
                    {
                    }
                    else if (cblmem.Items[mem].Text.Trim() == "Others")
                    {
                    }
                }
            }
        }
        catch { }
        return boolMemtype;
    }
    protected DataSet dsFilterValues(DataSet ds, ref string fromdate, ref string todate)
    {
        DataSet dsflt = new DataSet();
        ds.Tables[0].DefaultView.RowFilter = " (Transdate >= #" + Convert.ToDateTime(fromdate).ToString("MM/dd/yyyy") + "# And Transdate <= #" + Convert.ToDateTime(todate).ToString("MM/dd/yyyy") + "# ) ";
        DataView dvfst = ds.Tables[0].DefaultView;

        ds.Tables[1].DefaultView.RowFilter = " (Transdate >= #" + Convert.ToDateTime(fromdate).ToString("MM/dd/yyyy") + "# And Transdate <= #" + Convert.ToDateTime(todate).ToString("MM/dd/yyyy") + "# ) ";
        DataView dvsnd = ds.Tables[1].DefaultView;

        ds.Tables[2].DefaultView.RowFilter = " (Transdate >= #" + Convert.ToDateTime(fromdate).ToString("MM/dd/yyyy") + "# And Transdate <= #" + Convert.ToDateTime(todate).ToString("MM/dd/yyyy") + "# ) ";
        DataView dvthrd = ds.Tables[2].DefaultView;

        ds.Tables[3].DefaultView.RowFilter = " (Transdate >= #" + Convert.ToDateTime(fromdate).ToString("MM/dd/yyyy") + "# And Transdate <= #" + Convert.ToDateTime(todate).ToString("MM/dd/yyyy") + "# ) ";
        DataView dvfth = ds.Tables[3].DefaultView;

        ds.Tables[4].DefaultView.RowFilter = " (Transdate >= #" + Convert.ToDateTime(fromdate).ToString("MM/dd/yyyy") + "# And Transdate <= #" + Convert.ToDateTime(todate).ToString("MM/dd/yyyy") + "# ) ";
        DataView dvfith = ds.Tables[4].DefaultView;

        //   ds.Tables[6].DefaultView.RowFilter = " (Transdate >= #" + Convert.ToDateTime(fromdate).ToString("MM/dd/yyyy") + "# And Transdate <= #" + Convert.ToDateTime(todate).ToString("MM/dd/yyyy") + "# ) ";
        // DataView dvsixth = ds.Tables[6].DefaultView;
        string hdStr = string.Empty;
        if (rbtype.SelectedIndex == 0)
            hdStr = "headerfk";
        else
            hdStr = "ledgerfk";

        dsflt.Tables.Add(dvfst.ToTable());
        dsflt.Tables.Add(dvsnd.ToTable());
        dsflt.Tables.Add(dvthrd.ToTable());
        dsflt.Tables.Add(dvfth.ToTable());
        dsflt.Tables.Add(dvfith.ToTable(true, "feecategory", hdStr, "actualfinyearfk", "finyearfk"));
        //  dsflt.Tables.Add(ds.Tables[5].Copy());
        // dsflt.Tables.Add(dvsixth.ToTable());
        return dsflt;


    }

    //without bill no wise
    protected DataSet dsloadDetails(ref string fromdate, ref string todate)
    {
        DataSet dsload = new DataSet();
        try
        {
            #region get Value
            string batch = "";
            string degcourseid = "";
            string deptdegcode = "";
            string sem = "";
            string sec = "";
            string paid = "";
            string headervalue = "";
            string ledgervalue = "";
            string Finyearvalue = "";
            string SelQ = "";
            string strRecon = string.Empty;
            if (ddlcollegename.Items.Count > 0)
                collegecode = Convert.ToString(ddlcollegename.SelectedItem.Value);
            batch = Convert.ToString(getCblSelectedValue(cbl_batch));
            degcourseid = Convert.ToString(getCblSelectedValue(cbl_degree));
            deptdegcode = Convert.ToString(getCblSelectedValue(cbl_dept));
            sem = Convert.ToString(getCblSelectedValue(cbl_sem));
            sec = Convert.ToString(getCblSelectedValue(cbl_sect));
            paid = Convert.ToString(getCblSelectedValue(chkl_paid));
            headervalue = Convert.ToString(getCblSelectedValue(chkl_studhed));
            ledgervalue = Convert.ToString(getCblSelectedValue(chkl_studled));
            Finyearvalue = Convert.ToString(getCblSelectedValue(chklsfyear));
            string studMode = Convert.ToString(getCblSelectedValue(cbl_type));
            string finUser = Convert.ToString(getCblSelectedValue(cbluser));
            string memType = Convert.ToString(getCblSelectedValue(cblmem));
            fromdate = txt_fromdate.Text;
            todate = txt_todate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
            string strtype = string.Empty;
            string strReg = " ";
            if (rbtype.SelectedIndex == 0)
                strtype = ",headerfk";
            else
                strtype = ",ledgerfk";

            if (cbbfrecon.Checked)
                strRecon = " and ISNULL(IsCanceled,'0')<>'1'";
            else
                strRecon = " and ( f.IsDeposited='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1'";
            string applynStr = " AND r.IsConfirm = 1  and isnull(is_enroll,'0')<>'2'";//AND Admission_Status = 1
            //for (int se = 0; se < cbl_sem.Items.Count; se++)
            //{
            //    if (cbl_sem.Items[se].Selected)
            //    {
            //        if (sem == string.Empty)
            //            sem = " and( t.textval like'%" + cbl_sem.Items[se].Text + "%'";
            //        else
            //            sem += " or t.textval like'%" + cbl_sem.Items[se].Text + "%'";
            //    }
            //}
            //if (!string.IsNullOrEmpty(sem))
            //    sem += ")";           
            string strInclude = getStudCategory();
            #endregion

            #region Query New
            //date
            //tabl 0
            SelQ = " select Transdate, convert(varchar(10),Transdate,103) as date,sum(debit) as debit from (";
            SelQ += " select Transdate, convert(varchar(10),Transdate,103) as date,debit from ft_findailytransaction f,registration r,textvaltable t where f.app_no=r.app_no and f.feecategory=t.textcode and r.college_code=t.college_code and t.textcriteria='FEECA' " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + strReg + "  and isnull(debit,'0')>0 and r.mode in('" + studMode + "') and isnull(actualfinyearfk,'0')<>'0' and f.entryusercode in('" + finUser + "') and f.Transdate between '" + fromdate + "' and '" + todate + "'  " + strInclude + "";
            if (cbbeforeadm.Checked)
            {
                SelQ += " union all select Transdate, convert(varchar(10),Transdate,103) as date,debit from ft_findailytransaction f,applyn r,textvaltable t where f.app_no=r.app_no and f.feecategory=t.textcode and r.college_code=t.college_code and t.textcriteria='FEECA' " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "')  and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + applynStr + "  and isnull(debit,'0')>0 and r.mode in('" + studMode + "') and isnull(actualfinyearfk,'0')<>'0' and f.entryusercode in('" + finUser + "') and f.Transdate between '" + fromdate + "' and '" + todate + "'";
            }
            SelQ += ") tbl group by Transdate order by cast(Transdate as datetime) asc";
            //and f.Transdate between '" + fromdate + "' and '" + todate + "'
            //app_no details
            //tabl 1
            SelQ += " select distinct Transdate,convert(varchar(10),Transdate,103) as date,sum(debit) as debit,app_no,roll_no,reg_no,roll_admit,stud_name,degree_code,mode from(";
            SelQ += " select  Transdate,convert(varchar(10),Transdate,103) as date,debit,f.app_no,r.roll_no,r.reg_no,r.roll_admit,r.stud_name,r.degree_code,mode from ft_findailytransaction f,registration r,textvaltable t where f.app_no=r.app_no and f.feecategory=t.textcode and r.college_code=t.college_code and t.textcriteria='FEECA' " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + strReg + "  and isnull(debit,'0')>0 and r.mode in('" + studMode + "') and isnull(actualfinyearfk,'0')<>'0' and f.entryusercode in('" + finUser + "') and f.Transdate between '" + fromdate + "' and '" + todate + "'  " + strInclude + "";
            if (cbbeforeadm.Checked)
            {
                SelQ += " union all select  Transdate,convert(varchar(10),Transdate,103) as date,debit,f.app_no,r.app_formno as roll_no,'' reg_no,'' roll_admit,r.stud_name,r.degree_code,mode from ft_findailytransaction f,applyn r,textvaltable t where f.app_no=r.app_no and f.feecategory=t.textcode and r.college_code=t.college_code and t.textcriteria='FEECA' " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "')  and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + applynStr + "  and isnull(debit,'0')>0 and r.mode in('" + studMode + "') and isnull(actualfinyearfk,'0')<>'0' and f.entryusercode in('" + finUser + "') and f.Transdate between '" + fromdate + "' and '" + todate + "'";
            }
            SelQ += ") tbl group by Transdate,app_no,roll_no,reg_no,roll_admit,stud_name,degree_code,mode order by Transdate asc";
            //and f.Transdate between '" + fromdate + "' and '" + todate + "'

            //header and ledger detials
            //tabl 2
            SelQ += " select distinct Transdate,convert(varchar(10),Transdate,103) as date,app_no,sum(debit) as debit" + strtype + ",feecategory,Textval,degree_code,actualfinyearfk,finyearfk from(";
            SelQ += " select  Transdate,convert(varchar(10),Transdate,103) as date,f.app_no,debit" + strtype + ",feecategory,Textval,r.degree_code,actualfinyearfk,finyearfk from ft_findailytransaction f,registration r,textvaltable t where f.app_no=r.app_no and f.feecategory=t.textcode and r.college_code=t.college_code and t.textcriteria='FEECA' " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + strReg + " and isnull(debit,'0')>0 and r.mode in('" + studMode + "') and isnull(actualfinyearfk,'0')<>'0' and f.entryusercode in('" + finUser + "') and f.Transdate between '" + fromdate + "' and '" + todate + "'  " + strInclude + "";
            if (cbbeforeadm.Checked)
            {
                SelQ += " union all select  Transdate,convert(varchar(10),Transdate,103) as date,f.app_no,debit" + strtype + ",feecategory,Textval,r.degree_code,actualfinyearfk,finyearfk from ft_findailytransaction f,applyn r,textvaltable t where f.app_no=r.app_no and f.feecategory=t.textcode and r.college_code=t.college_code and t.textcriteria='FEECA' " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "')  and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "'" + applynStr + " and isnull(debit,'0')>0  and r.mode in('" + studMode + "') and isnull(actualfinyearfk,'0')<>'0' and f.entryusercode in('" + finUser + "') and f.Transdate between '" + fromdate + "' and '" + todate + "'";
            }
            SelQ += ") tbl group by Transdate" + strtype + ",feecategory,Textval,app_no,degree_code,actualfinyearfk,finyearfk order by Transdate,degree_code,Textval asc";

            //and f.Transdate between '" + fromdate + "' and '" + todate + "'
            // paymode
            //tabl 3
            SelQ += " select distinct Transdate,convert(varchar(10),Transdate,103) as date,sum(debit) as debit,paymode,app_no from(";
            SelQ += " select  Transdate,convert(varchar(10),Transdate,103) as date,debit,f.paymode,f.app_no from ft_findailytransaction f,registration r,textvaltable t where f.app_no=r.app_no and f.feecategory=t.textcode and r.college_code=t.college_code and t.textcriteria='FEECA' " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + strReg + "  and isnull(debit,'0')>0 and r.mode in('" + studMode + "') and isnull(actualfinyearfk,'0')<>'0' and f.entryusercode in('" + finUser + "') and f.Transdate between '" + fromdate + "' and '" + todate + "'  " + strInclude + "";
            if (cbbeforeadm.Checked)
            {
                SelQ += " union all select  Transdate,convert(varchar(10),Transdate,103) as date,debit,f.paymode,f.app_no from ft_findailytransaction f,applyn r,textvaltable t where f.app_no=r.app_no and f.feecategory=t.textcode and r.college_code=t.college_code and t.textcriteria='FEECA' " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "'" + applynStr + "  and isnull(debit,'0')>0 and r.mode in('" + studMode + "') and isnull(actualfinyearfk,'0')<>'0' and f.entryusercode in('" + finUser + "') and f.Transdate between '" + fromdate + "' and '" + todate + "'";
            }
            SelQ += ") tbl group by Transdate,paymode,app_no order by Transdate asc";
            //and f.Transdate between '" + fromdate + "' and '" + todate + "'          

            //spread header bind
            //spread bind onlyn distinct header name get
            //tbl 4
            SelQ += " select distinct Transdate,feecategory" + strtype + ",actualfinyearfk,finyearfk from (";
            SelQ += " select  Transdate,f.feecategory" + strtype + ",actualfinyearfk,finyearfk from ft_findailytransaction f,registration r where f.app_no=r.app_no " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + strReg + "  and isnull(debit,'0')>0 and isnull(actualfinyearfk,'0')<>'0' and r.mode in('" + studMode + "') and f.entryusercode in('" + finUser + "') and f.Transdate between '" + fromdate + "' and '" + todate + "'  " + strInclude + "";
            if (cbbeforeadm.Checked)
            {
                SelQ += " union all select Transdate,f.feecategory" + strtype + ",actualfinyearfk,finyearfk from ft_findailytransaction f,applyn r where f.app_no=r.app_no " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "')  and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + applynStr + "  and isnull(debit,'0')>0 and isnull(actualfinyearfk,'0')<>'0' and r.mode in('" + studMode + "') and f.entryusercode in('" + finUser + "') and f.Transdate between '" + fromdate + "' and '" + todate + "'";
            }
            SelQ += ") tbl ";

            // SelQ += " select d.Degree_Code,(dt.Dept_Name) as degreename,(c.Course_Name +'-'+dt.dept_acronym)as dept_acronym from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode + "'";

            #endregion

            #region others
            for (int mem = 0; mem < cblmem.Items.Count; mem++)
            {
                if (cblmem.Items[mem].Selected)
                {
                    if (cblmem.Items[mem].Text.Trim() == "Staff")
                    {
                        #region staff
                        //table 0
                        SelQ += " select  Transdate,convert(varchar(10),Transdate,103) as date,sum(debit) as debit,memtype from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and t.dept_code =h.dept_code and t.desig_code =d.desig_code and T.staff_code =s.staff_code and T.latestrec ='1' " + strRecon + " and d.collegeCode='" + collegecode + "' and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        // if (usBasedRights == true)
                        SelQ += " and f.EntryUserCode in('" + finUser + "')";
                        SelQ += " group by MemType,Transdate";

                        //tabl 1 with staff details
                        SelQ += " select  Transdate,convert(varchar(10),Transdate,103) as date,sum(debit) as debit,memtype,s.staff_name,s.staff_code,h.dept_name,f.app_no from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and t.dept_code =h.dept_code and t.desig_code =d.desig_code and T.staff_code =s.staff_code and T.latestrec ='1' " + strRecon + " and d.collegeCode='" + collegecode + "' and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        //if (usBasedRights == true)
                        SelQ += " and f.EntryUserCode in('" + finUser + "')";
                        SelQ += " group by MemType,Transdate,s.staff_name,s.staff_code,h.dept_name,f.app_no";

                        //tabl 2 with staff header and ledger details
                        SelQ += " select  Transdate,convert(varchar(10),Transdate,103) as date,sum(debit) as debit,memtype,s.staff_name,s.staff_code,h.dept_name,f.app_no" + strtype + ",actualfinyearfk,finyearfk from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and t.dept_code =h.dept_code and t.desig_code =d.desig_code and T.staff_code =s.staff_code and T.latestrec ='1' " + strRecon + " and d.collegeCode='" + collegecode + "' and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        // if (usBasedRights == true)
                        SelQ += " and f.EntryUserCode in('" + finUser + "')";
                        SelQ += " group by MemType,Transdate,s.staff_name,s.staff_code,h.dept_name,f.app_no " + strtype + ",actualfinyearfk,finyearfk";

                        //tabl 3 with staff paymode details
                        SelQ += " select  Transdate,convert(varchar(10),Transdate,103) as date,sum(debit) as debit,memtype,f.app_no,f.paymode from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and t.dept_code =h.dept_code and t.desig_code =d.desig_code and T.staff_code =s.staff_code and T.latestrec ='1' " + strRecon + " and d.collegeCode='" + collegecode + "' and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        //  if (usBasedRights == true)
                        SelQ += " and f.EntryUserCode in('" + finUser + "')";
                        SelQ += " group by MemType,Transdate,f.app_no,f.paymode";

                        //staff
                        // SelQ += " select  SUM(debit) as debit ,MemType,f.paymode" + strtype + " from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and t.dept_code =h.dept_code and t.desig_code =d.desig_code and T.staff_code =s.staff_code and T.latestrec ='1' " + strRecon + " and d.collegeCode='" + collegecode + "' and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        // if (usBasedRights == true)
                        //    SelQ += " and f.EntryUserCode in('" + finUser+ "')";
                        //SelQ += " group by MemType,f.paymode" + strtype + "";   
                        #endregion
                    }
                    else if (cblmem.Items[mem].Text.Trim() == "Vendor")
                    {
                        #region vendor
                        //tbl 0
                        SelQ += "  SELECT Transdate, convert(varchar(10),Transdate,103) as date,sum(debit) as debit,memtype  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1' " + strRecon + "  and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        // if (usBasedRights == true)
                        SelQ += " and f.EntryUserCode in('" + finUser + "')";
                        SelQ += " group by MemType,Transdate";

                        //tbl 1 vendor details
                        SelQ += "  SELECT Transdate, convert(varchar(10),Transdate,103) as date,sum(debit) as debit,memtype, vendorname,vendorcode,f.app_no  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1' " + strRecon + "  and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        //  if (usBasedRights == true)
                        SelQ += " and f.EntryUserCode in('" + finUser + "')";
                        SelQ += " group by MemType,Transdate, vendorname,vendorcode,f.app_no";

                        //tbl 1 vendor header and ledger details
                        SelQ += "  SELECT Transdate, convert(varchar(10),Transdate,103) as date,sum(debit) as debit,memtype,f.app_no" + strtype + ",actualfinyearfk,finyearfk  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1' " + strRecon + "  and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        //  if (usBasedRights == true)
                        SelQ += " and f.EntryUserCode in('" + finUser + "')";
                        SelQ += " group by MemType,Transdate,f.app_no" + strtype + ",actualfinyearfk,finyearfk";

                        //tbl 1 vendo rpaymode details
                        SelQ += "  SELECT Transdate, convert(varchar(10),Transdate,103) as date,sum(debit) as debit,memtype,f.app_no,paymode FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1' " + strRecon + "  and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        // if (usBasedRights == true)
                        SelQ += " and f.EntryUserCode in('" + finUser + "')";
                        SelQ += " group by MemType,Transdate,f.app_no,paymode";

                        //vendor
                        // SelQ += "  SELECT SUM(debit) as debit ,MemType,f.paymode" + strtype + "  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1' " + strRecon + "  and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        //if (usBasedRights == true)
                        //    SelQ += " and f.EntryUserCode in('" + finUser+ "')";
                        //SelQ += " group by MemType,f.paymode" + strtype + "";
                        #endregion
                    }
                    else if (cblmem.Items[mem].Text.Trim() == "Others")
                    {
                        #region Others
                        //tbl 0
                        SelQ += "  SELECT Transdate, convert(varchar(10),Transdate,103) as date,sum(debit) as debit,memtype  FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5' " + strRecon + "  and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        //  if (usBasedRights == true)
                        SelQ += " and f.EntryUserCode in('" + finUser + "')";
                        SelQ += " group by MemType,Transdate";

                        //tbl 1 vendor details
                        SelQ += "  SELECT Transdate, convert(varchar(10),Transdate,103) as date,sum(debit) as debit,memtype, vendorname,vendorcode,f.app_no  FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5' " + strRecon + "  and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        //   if (usBasedRights == true)
                        SelQ += " and f.EntryUserCode in('" + finUser + "')";
                        SelQ += " group by MemType,Transdate, vendorname,vendorcode,f.app_no";

                        //tbl 1 vendor header and ledger details
                        SelQ += "  SELECT Transdate, convert(varchar(10),Transdate,103) as date,sum(debit) as debit,memtype,f.app_no" + strtype + ",actualfinyearfk,finyearfk  FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5' " + strRecon + "  and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        // if (usBasedRights == true)
                        SelQ += " and f.EntryUserCode in('" + finUser + "')";
                        SelQ += " group by MemType,Transdate,f.app_no" + strtype + ",actualfinyearfk,finyearfk";

                        //tbl 1 vendo rpaymode details
                        SelQ += "  SELECT Transdate, convert(varchar(10),Transdate,103) as date,sum(debit) as debit,memtype,f.app_no,paymode FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5' " + strRecon + "  and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        //  if (usBasedRights == true)
                        SelQ += " and f.EntryUserCode in('" + finUser + "')";
                        SelQ += " group by MemType,Transdate,f.app_no,paymode";

                        //vendor
                        // SelQ += "  SELECT SUM(debit) as debit ,MemType,f.paymode" + strtype + "  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1' " + strRecon + "  and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        //if (usBasedRights == true)
                        //    SelQ += " and f.EntryUserCode in('" + finUser+ "')";
                        //SelQ += " group by MemType,f.paymode" + strtype + "";
                        #endregion
                    }
                }
            }

            ////others
            //SelQ += "  SELECT SUM(debit) as debit ,MemType,f.paymode" + strtype + " FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5' " + strRecon + " and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //if (usBasedRights == true)
            //    SelQ += " and f.EntryUserCode in('" + finUser+ "')";
            //SelQ += " group by MemType,f.paymode" + strtype + "";

            #endregion
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(SelQ, "Text");
        }
        catch { dsload.Clear(); }
        return dsload;
    }

    //with bill no wise
    protected DataSet dsloadDetailsBillNo(ref string fromdate, ref string todate)
    {
        DataSet dsload = new DataSet();
        try
        {
            #region get Value
            string batch = "";
            string degcourseid = "";
            string deptdegcode = "";
            string sem = "";
            string sec = "";
            string paid = "";
            string headervalue = "";
            string ledgervalue = "";
            string Finyearvalue = "";
            string SelQ = "";
            string strRecon = string.Empty;
            if (ddlcollegename.Items.Count > 0)
                collegecode = Convert.ToString(ddlcollegename.SelectedItem.Value);
            batch = Convert.ToString(getCblSelectedValue(cbl_batch));
            degcourseid = Convert.ToString(getCblSelectedValue(cbl_degree));
            deptdegcode = Convert.ToString(getCblSelectedValue(cbl_dept));
            sem = Convert.ToString(getCblSelectedValue(cbl_sem));
            sec = Convert.ToString(getCblSelectedValue(cbl_sect));
            paid = Convert.ToString(getCblSelectedValue(chkl_paid));
            headervalue = Convert.ToString(getCblSelectedValue(chkl_studhed));
            ledgervalue = Convert.ToString(getCblSelectedValue(chkl_studled));
            Finyearvalue = Convert.ToString(getCblSelectedValue(chklsfyear));
            string studMode = Convert.ToString(getCblSelectedValue(cbl_type));
            string finUser = Convert.ToString(getCblSelectedValue(cbluser));
            string memType = Convert.ToString(getCblSelectedValue(cblmem));
            fromdate = txt_fromdate.Text;
            todate = txt_todate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
            string strtype = string.Empty;
            string strReg = " ";
            if (rbtype.SelectedIndex == 0)
                strtype = ",headerfk";
            else
                strtype = ",ledgerfk";

            if (cbbfrecon.Checked)
                strRecon = " and ISNULL(IsCanceled,'0')<>'1'";
            else
                strRecon = " and ( f.IsDeposited='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1'";
            string applynStr = " AND r.IsConfirm = 1  and isnull(is_enroll,'0')<>'2'";//AND Admission_Status = 1
            //for (int se = 0; se < cbl_sem.Items.Count; se++)
            //{
            //    if (cbl_sem.Items[se].Selected)
            //    {
            //        if (sem == string.Empty)
            //            sem = " and( t.textval like'%" + cbl_sem.Items[se].Text + "%'";
            //        else
            //            sem += " or t.textval like'%" + cbl_sem.Items[se].Text + "%'";
            //    }
            //}
            //if (!string.IsNullOrEmpty(sem))
            //    sem += ")";           
            string strInclude = getStudCategory();
            #endregion

            #region Query New
            //date
            //tabl 0
            SelQ = " select Transdate, convert(varchar(10),Transdate,103) as date,sum(debit) as debit from (";
            SelQ += " select Transdate, convert(varchar(10),Transdate,103) as date,debit from ft_findailytransaction f,registration r,textvaltable t where f.app_no=r.app_no and f.feecategory=t.textcode and r.college_code=t.college_code and t.textcriteria='FEECA' " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + strReg + "  and isnull(debit,'0')>0 and r.mode in('" + studMode + "') and isnull(actualfinyearfk,'0')<>'0' and f.entryusercode in('" + finUser + "') and f.Transdate between '" + fromdate + "' and '" + todate + "'  " + strInclude + "";
            if (cbbeforeadm.Checked)
            {
                SelQ += " union all select Transdate, convert(varchar(10),Transdate,103) as date,debit from ft_findailytransaction f,applyn r,textvaltable t where f.app_no=r.app_no and f.feecategory=t.textcode and r.college_code=t.college_code and t.textcriteria='FEECA' " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "')  and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + applynStr + "  and isnull(debit,'0')>0 and r.mode in('" + studMode + "') and isnull(actualfinyearfk,'0')<>'0' and f.entryusercode in('" + finUser + "') and f.Transdate between '" + fromdate + "' and '" + todate + "'";
            }
            SelQ += ") tbl group by Transdate order by cast(Transdate as datetime) asc";
            //and f.Transdate between '" + fromdate + "' and '" + todate + "'
            //app_no details
            //tabl 1
            SelQ += " select distinct Transdate,Transcode,convert(varchar(10),Transdate,103) as date,sum(debit) as debit,app_no,roll_no,reg_no,roll_admit,stud_name,degree_code,mode from(";
            SelQ += " select  Transdate,Transcode,convert(varchar(10),Transdate,103) as date,debit,f.app_no,r.roll_no,r.reg_no,r.roll_admit,r.stud_name,r.degree_code,mode from ft_findailytransaction f,registration r,textvaltable t where f.app_no=r.app_no and f.feecategory=t.textcode and r.college_code=t.college_code and t.textcriteria='FEECA' " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + strReg + "  and isnull(debit,'0')>0 and r.mode in('" + studMode + "') and isnull(actualfinyearfk,'0')<>'0' and f.entryusercode in('" + finUser + "') and f.Transdate between '" + fromdate + "' and '" + todate + "'  " + strInclude + "";
            if (cbbeforeadm.Checked)
            {
                SelQ += " union all select  Transdate,Transcode,convert(varchar(10),Transdate,103) as date,debit,f.app_no,r.app_formno as roll_no,'' reg_no,r.app_formno as roll_admit,r.stud_name,r.degree_code,mode from ft_findailytransaction f,applyn r,textvaltable t where f.app_no=r.app_no and f.feecategory=t.textcode and r.college_code=t.college_code and t.textcriteria='FEECA' " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "')  and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + applynStr + "  and isnull(debit,'0')>0 and r.mode in('" + studMode + "') and isnull(actualfinyearfk,'0')<>'0' and f.entryusercode in('" + finUser + "') and f.Transdate between '" + fromdate + "' and '" + todate + "'";
            }
            SelQ += ") tbl group by Transdate,Transcode,app_no,roll_no,reg_no,roll_admit,stud_name,degree_code,mode order by Transdate asc";
            //and f.Transdate between '" + fromdate + "' and '" + todate + "'

            //header and ledger detials
            //tabl 2
            SelQ += " select distinct Transdate,Transcode,convert(varchar(10),Transdate,103) as date,app_no,sum(debit) as debit" + strtype + ",feecategory,Textval,degree_code,actualfinyearfk,finyearfk from(";
            SelQ += " select  Transdate,Transcode,convert(varchar(10),Transdate,103) as date,f.app_no,debit" + strtype + ",feecategory,Textval,r.degree_code,actualfinyearfk,finyearfk from ft_findailytransaction f,registration r,textvaltable t where f.app_no=r.app_no and f.feecategory=t.textcode and r.college_code=t.college_code and t.textcriteria='FEECA' " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + strReg + " and isnull(debit,'0')>0 and r.mode in('" + studMode + "') and isnull(actualfinyearfk,'0')<>'0' and f.entryusercode in('" + finUser + "') and f.Transdate between '" + fromdate + "' and '" + todate + "'  " + strInclude + "";
            if (cbbeforeadm.Checked)
            {
                SelQ += " union all select  Transdate,Transcode,convert(varchar(10),Transdate,103) as date,f.app_no,debit" + strtype + ",feecategory,Textval,r.degree_code,actualfinyearfk,finyearfk from ft_findailytransaction f,applyn r,textvaltable t where f.app_no=r.app_no and f.feecategory=t.textcode and r.college_code=t.college_code and t.textcriteria='FEECA' " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "')  and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "'" + applynStr + " and isnull(debit,'0')>0  and r.mode in('" + studMode + "') and isnull(actualfinyearfk,'0')<>'0' and f.entryusercode in('" + finUser + "') and f.Transdate between '" + fromdate + "' and '" + todate + "'";
            }
            SelQ += ") tbl group by Transdate,Transcode" + strtype + ",feecategory,Textval,app_no,degree_code,actualfinyearfk,finyearfk order by Transdate,degree_code,Textval asc";

            //and f.Transdate between '" + fromdate + "' and '" + todate + "'
            // paymode
            //tabl 3
            SelQ += " select distinct Transdate,Transcode,convert(varchar(10),Transdate,103) as date,sum(debit) as debit,paymode,app_no from(";
            SelQ += " select  Transdate,Transcode,convert(varchar(10),Transdate,103) as date,debit,f.paymode,f.app_no from ft_findailytransaction f,registration r,textvaltable t where f.app_no=r.app_no and f.feecategory=t.textcode and r.college_code=t.college_code and t.textcriteria='FEECA' " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + strReg + "  and isnull(debit,'0')>0 and r.mode in('" + studMode + "') and isnull(actualfinyearfk,'0')<>'0' and f.entryusercode in('" + finUser + "') and f.Transdate between '" + fromdate + "' and '" + todate + "'  " + strInclude + "";
            if (cbbeforeadm.Checked)
            {
                SelQ += " union all select  Transdate,Transcode,convert(varchar(10),Transdate,103) as date,debit,f.paymode,f.app_no from ft_findailytransaction f,applyn r,textvaltable t where f.app_no=r.app_no and f.feecategory=t.textcode and r.college_code=t.college_code and t.textcriteria='FEECA' " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "'" + applynStr + "  and isnull(debit,'0')>0 and r.mode in('" + studMode + "') and isnull(actualfinyearfk,'0')<>'0' and f.entryusercode in('" + finUser + "') and f.Transdate between '" + fromdate + "' and '" + todate + "'";
            }
            SelQ += ") tbl group by Transdate,Transcode,paymode,app_no order by Transdate asc";
            //and f.Transdate between '" + fromdate + "' and '" + todate + "'          

            //spread header bind
            //spread bind onlyn distinct header name get
            //tbl 4
            SelQ += " select distinct Transdate,feecategory" + strtype + ",actualfinyearfk,finyearfk from (";
            SelQ += " select  Transdate,f.feecategory" + strtype + ",actualfinyearfk,finyearfk from ft_findailytransaction f,registration r where f.app_no=r.app_no " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + strReg + "  and isnull(debit,'0')>0 and isnull(actualfinyearfk,'0')<>'0' and r.mode in('" + studMode + "') and f.entryusercode in('" + finUser + "') and f.Transdate between '" + fromdate + "' and '" + todate + "'  " + strInclude + "";
            if (cbbeforeadm.Checked)
            {
                SelQ += " union all select Transdate,f.feecategory" + strtype + ",actualfinyearfk,finyearfk from ft_findailytransaction f,applyn r where f.app_no=r.app_no " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "')  and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + applynStr + "  and isnull(debit,'0')>0 and isnull(actualfinyearfk,'0')<>'0' and r.mode in('" + studMode + "') and f.entryusercode in('" + finUser + "') and f.Transdate between '" + fromdate + "' and '" + todate + "'";
            }
            SelQ += ") tbl ";

            // SelQ += " select d.Degree_Code,(dt.Dept_Name) as degreename,(c.Course_Name +'-'+dt.dept_acronym)as dept_acronym from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode + "'";

            #endregion

            #region others
            for (int mem = 0; mem < cblmem.Items.Count; mem++)
            {
                if (cblmem.Items[mem].Selected)
                {
                    if (cblmem.Items[mem].Text.Trim() == "Staff")
                    {
                        #region staff
                        //table 0
                        SelQ += " select  Transdate,convert(varchar(10),Transdate,103) as date,sum(debit) as debit,memtype from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and t.dept_code =h.dept_code and t.desig_code =d.desig_code and T.staff_code =s.staff_code and T.latestrec ='1' " + strRecon + " and d.collegeCode='" + collegecode + "' and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        if (usBasedRights == true)
                            SelQ += " and f.EntryUserCode in('" + finUser + "')";
                        SelQ += " group by MemType,Transdate";

                        //tabl 1 with staff details
                        SelQ += " select  Transdate,Transcode,convert(varchar(10),Transdate,103) as date,sum(debit) as debit,memtype,s.staff_name,s.staff_code,h.dept_name,f.app_no from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and t.dept_code =h.dept_code and t.desig_code =d.desig_code and T.staff_code =s.staff_code and T.latestrec ='1' " + strRecon + " and d.collegeCode='" + collegecode + "' and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        if (usBasedRights == true)
                            SelQ += " and f.EntryUserCode in('" + finUser + "')";
                        SelQ += " group by MemType,Transdate,Transcode,s.staff_name,s.staff_code,h.dept_name,f.app_no";

                        //tabl 2 with staff header and ledger details
                        SelQ += " select  Transdate,Transcode,convert(varchar(10),Transdate,103) as date,sum(debit) as debit,memtype,s.staff_name,s.staff_code,h.dept_name,f.app_no" + strtype + ",actualfinyearfk,finyearfk from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and t.dept_code =h.dept_code and t.desig_code =d.desig_code and T.staff_code =s.staff_code and T.latestrec ='1' " + strRecon + " and d.collegeCode='" + collegecode + "' and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        if (usBasedRights == true)
                            SelQ += " and f.EntryUserCode in('" + finUser + "')";
                        SelQ += " group by MemType,Transdate,Transcode,s.staff_name,s.staff_code,h.dept_name,f.app_no " + strtype + ",actualfinyearfk,finyearfk";

                        //tabl 3 with staff paymode details
                        SelQ += " select  Transdate,Transcode,convert(varchar(10),Transdate,103) as date,sum(debit) as debit,memtype,f.app_no,f.paymode from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and t.dept_code =h.dept_code and t.desig_code =d.desig_code and T.staff_code =s.staff_code and T.latestrec ='1' " + strRecon + " and d.collegeCode='" + collegecode + "' and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        if (usBasedRights == true)
                            SelQ += " and f.EntryUserCode in('" + finUser + "')";
                        SelQ += " group by MemType,Transdate,Transcode,f.app_no,f.paymode";
                        #endregion
                    }
                    else if (cblmem.Items[mem].Text.Trim() == "Vendor")
                    {
                        #region vendor
                        //tbl 0
                        SelQ += "  SELECT Transdate, convert(varchar(10),Transdate,103) as date,sum(debit) as debit,memtype  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1' " + strRecon + "  and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        if (usBasedRights == true)
                            SelQ += " and f.EntryUserCode in('" + finUser + "')";
                        SelQ += " group by MemType,Transdate";

                        //tbl 1 vendor details
                        SelQ += "  SELECT Transdate,Transcode, convert(varchar(10),Transdate,103) as date,sum(debit) as debit,memtype, vendorname,vendorcode,f.app_no  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1' " + strRecon + "  and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        if (usBasedRights == true)
                            SelQ += " and f.EntryUserCode in('" + finUser + "')";
                        SelQ += " group by MemType,Transdate,Transcode, vendorname,vendorcode,f.app_no";

                        //tbl 1 vendor header and ledger details
                        SelQ += "  SELECT Transdate,Transcode, convert(varchar(10),Transdate,103) as date,sum(debit) as debit,memtype,f.app_no" + strtype + ",actualfinyearfk,finyearfk  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1' " + strRecon + "  and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        if (usBasedRights == true)
                            SelQ += " and f.EntryUserCode in('" + finUser + "')";
                        SelQ += " group by MemType,Transdate,Transcode,f.app_no" + strtype + ",actualfinyearfk,finyearfk";

                        //tbl 1 vendo rpaymode details
                        SelQ += "  SELECT Transdate,Transcode, convert(varchar(10),Transdate,103) as date,sum(debit) as debit,memtype,f.app_no,paymode FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1' " + strRecon + "  and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        if (usBasedRights == true)
                            SelQ += " and f.EntryUserCode in('" + finUser + "')";
                        SelQ += " group by MemType,Transdate,Transcode,f.app_no,paymode";
                        #endregion
                    }
                    else if (cblmem.Items[mem].Text.Trim() == "Others")
                    {
                        #region Others
                        //tbl 0
                        SelQ += "  SELECT Transdate, convert(varchar(10),Transdate,103) as date,sum(debit) as debit,memtype  FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5' " + strRecon + "  and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        if (usBasedRights == true)
                            SelQ += " and f.EntryUserCode in('" + finUser + "')";
                        SelQ += " group by MemType,Transdate";

                        //tbl 1 vendor details
                        SelQ += "  SELECT Transdate,Transcode, convert(varchar(10),Transdate,103) as date,sum(debit) as debit,memtype, vendorname,vendorcode,f.app_no  FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5' " + strRecon + "  and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        if (usBasedRights == true)
                            SelQ += " and f.EntryUserCode in('" + finUser + "')";
                        SelQ += " group by MemType,Transdate,Transcode, vendorname,vendorcode,f.app_no";

                        //tbl 1 vendor header and ledger details
                        SelQ += "  SELECT Transdate,Transcode, convert(varchar(10),Transdate,103) as date,sum(debit) as debit,memtype,f.app_no" + strtype + ",actualfinyearfk,finyearfk  FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5' " + strRecon + "  and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        if (usBasedRights == true)
                            SelQ += " and f.EntryUserCode in('" + finUser + "')";
                        SelQ += " group by MemType,Transdate,Transcode,f.app_no" + strtype + ",actualfinyearfk,finyearfk";

                        //tbl 1 vendo rpaymode details
                        SelQ += "  SELECT Transdate,Transcode, convert(varchar(10),Transdate,103) as date,sum(debit) as debit,memtype,f.app_no,paymode FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5' " + strRecon + "  and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        if (usBasedRights == true)
                            SelQ += " and f.EntryUserCode in('" + finUser + "')";
                        SelQ += " group by MemType,Transdate,Transcode,f.app_no,paymode";
                        #endregion
                    }
                }
            }
            #endregion

            dsload.Clear();
            dsload = d2.select_method_wo_parameter(SelQ, "Text");
        }
        catch { dsload.Clear(); }
        return dsload;
    }

    //without bill no wise
    protected void spreadLoadDetailed(DataSet ds, ref string fromdate, ref string todate)
    {
        try
        {
            #region design
            RollAndRegSettings();
            spreadDet.Sheets[0].RowCount = 0;
            spreadDet.Sheets[0].ColumnCount = 0;
            spreadDet.CommandBar.Visible = false;
            spreadDet.Sheets[0].AutoPostBack = true;
            spreadDet.Sheets[0].ColumnHeader.RowCount = 3;
            spreadDet.Sheets[0].RowHeader.Visible = false;
            spreadDet.Sheets[0].ColumnCount = 8;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            spreadDet.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Date";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Student Name";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            spreadDet.Sheets[0].Columns[2].Width = 350;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Roll No";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Reg No";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Admission No";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Application No";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[6].Visible = false;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 7].Text = lbldept.Text;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 7].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Left;


            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 3, 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 3, 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 3, 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 3, 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 3, 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 3, 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 3, 1);

            #region financial year

            Dictionary<string, string> dtMode = getstudMode();
            Hashtable htHDName = getHeaderFK();
            Hashtable htActYr = getFinyear();
            Hashtable htColCnt = new Hashtable();
            Hashtable htColCntOther = new Hashtable();
            for (int row = 0; row < chklsfyear.Items.Count; row++)
            {
                string tempfnlyr = string.Empty;
                if (chklsfyear.Items[row].Selected)
                {
                    bool checkbatch = false;
                    int batchCol = 0;
                    bool batchbol = false;
                    string hdrTxtValue = string.Empty;
                    int col = spreadDet.Sheets[0].ColumnCount++;
                    hdrTxtValue = Convert.ToString(chklsfyear.Items[row].Value);
                    tempfnlyr = Convert.ToString(chklsfyear.Items[row].Value);
                    //foreach (KeyValuePair<string, string> rMode in dtMode)
                    //{
                    for (int sem = 0; sem < cbl_sem.Items.Count; sem++)
                    {
                        int semcol = 0;
                        int tempsemcol = 0;
                        if (cbl_sem.Items[sem].Selected)
                        {
                            bool sembol = false;
                            hdrTxtValue += "-" + Convert.ToString(cbl_sem.Items[sem].Value);
                            //if (batchbol)
                            //    semcol = spreadDet.Sheets[0].ColumnCount++;
                            //else
                            //    semcol = col;                           
                            ds.Tables[4].DefaultView.RowFilter = " finyearfk ='" + chklsfyear.Items[row].Value + "' and feecategory='" + cbl_sem.Items[sem].Value + "'";//mode='" + rMode.Value + "' and
                            DataTable dthd = ds.Tables[4].DefaultView.ToTable();
                            if (dthd.Rows.Count > 0)
                            {
                                if (batchbol)
                                    semcol = spreadDet.Sheets[0].ColumnCount++;
                                else
                                    semcol = col;
                                #region header
                                int semcolcnt = 0;
                                for (int hd = 0; hd < dthd.Rows.Count; hd++)
                                {
                                    semcolcnt++;
                                    batchCol++;
                                    if (sembol)
                                        spreadDet.Sheets[0].ColumnCount++;
                                    string hdName = string.Empty;
                                    string actFinFk = string.Empty;
                                    string hdorldFK = string.Empty;

                                    string actualYEar = Convert.ToString(htActYr[Convert.ToString(dthd.Rows[hd]["actualfinyearfk"]).Trim()]);
                                    actFinFk = Convert.ToString(dthd.Rows[hd]["actualfinyearfk"]);

                                    if (rbtype.SelectedIndex == 0)
                                    {
                                        hdName = Convert.ToString(htHDName[Convert.ToString(dthd.Rows[hd]["headerfk"]).Trim()]);
                                        spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(dthd.Rows[hd]["headerfk"]);
                                        hdorldFK = Convert.ToString(dthd.Rows[hd]["headerfk"]);
                                    }
                                    else
                                    {
                                        hdName = Convert.ToString(htHDName[Convert.ToString(dthd.Rows[hd]["ledgerfk"]).Trim()]);
                                        spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(dthd.Rows[hd]["ledgerfk"]);
                                        hdorldFK = Convert.ToString(dthd.Rows[hd]["ledgerfk"]);
                                    }
                                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Text = hdName + "(" + actualYEar + ")";
                                    string value = hdrTxtValue + "-" + hdorldFK + "-" + actFinFk;
                                    string tempValue = tempfnlyr + "-" + hdorldFK;
                                    if (!htColCnt.ContainsKey(value))
                                        htColCnt.Add(value, spreadDet.Sheets[0].ColumnCount - 1);
                                    if (!htColCntOther.ContainsKey(tempValue))
                                        htColCntOther.Add(tempValue, spreadDet.Sheets[0].ColumnCount - 1);
                                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                    spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                    sembol = true;
                                    checkbatch = true;
                                }
                                batchbol = true;
                                if (semcolcnt > 0)
                                {
                                    spreadDet.Sheets[0].ColumnHeader.Cells[1, semcol].Text = Convert.ToString(cbl_sem.Items[sem].Text);
                                    spreadDet.Sheets[0].ColumnHeader.Cells[1, semcol].Tag = Convert.ToString(cbl_sem.Items[sem].Value);
                                    // hdrTxtValue += "-" + Convert.ToString(cbl_sem.Items[sem].Value);
                                    spreadDet.Sheets[0].ColumnHeader.Cells[1, semcol].ForeColor = ColorTranslator.FromHtml("#000000");
                                    spreadDet.Sheets[0].ColumnHeader.Cells[1, semcol].Font.Bold = true;
                                    spreadDet.Sheets[0].ColumnHeader.Cells[1, semcol].Font.Name = "Book Antiqua";
                                    spreadDet.Sheets[0].ColumnHeader.Cells[1, semcol].Font.Size = FontUnit.Medium;
                                    spreadDet.Sheets[0].ColumnHeader.Cells[1, semcol].HorizontalAlign = HorizontalAlign.Center;
                                    spreadDet.Sheets[0].Columns[semcol].HorizontalAlign = HorizontalAlign.Right;
                                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(1, semcol, 1, semcolcnt);
                                }
                                #endregion
                            }
                        }
                        hdrTxtValue = Convert.ToString(chklsfyear.Items[row].Value);
                    }
                    if (checkbatch)
                    {
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Text = Convert.ToString(htActYr[Convert.ToString(chklsfyear.Items[row].Value).Trim()]);// Convert.ToString(chklsfyear.Items[row].Text);
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Tag = Convert.ToString(chklsfyear.Items[row].Value);
                        // hdrTxtValue += "-" + Convert.ToString(cbl_sem.Items[sem].Value);
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, col].ForeColor = ColorTranslator.FromHtml("#000000");
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Bold = true;
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Name = "Book Antiqua";
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Size = FontUnit.Medium;
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, col].HorizontalAlign = HorizontalAlign.Center;
                        spreadDet.Sheets[0].Columns[col].HorizontalAlign = HorizontalAlign.Right;
                        spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, col, 1, batchCol);
                    }
                    else
                        spreadDet.Sheets[0].ColumnCount--;

                    //}
                }
            }
            #endregion

            int paycol = spreadDet.Sheets[0].ColumnCount++;
            htColCnt.Add("Total", spreadDet.Sheets[0].ColumnCount - 1);
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Text = "Total";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, paycol, 3, 1);

            Hashtable htPayCol = new Hashtable();
            int check = 0;
            paycol = spreadDet.Sheets[0].ColumnCount++;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Text = "PayMode";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

            int checkva = 0;
            for (int s = 0; s < chkl_paid.Items.Count; s++)
            {
                if (chkl_paid.Items[s].Selected == true)
                {
                    checkva++;
                    if (checkva > 1)
                        check = spreadDet.Sheets[0].ColumnCount++;
                    htPayCol.Add(Convert.ToString(chkl_paid.Items[s].Value), spreadDet.Sheets[0].ColumnCount - 1);
                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(chkl_paid.Items[s].Text);
                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(chkl_paid.Items[s].Value);
                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;

                }
            }
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(2, check, 1, spreadDet.Sheets[0].ColumnCount - 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, paycol, 2, spreadDet.Sheets[0].ColumnCount - 1);
            spreadColumnVisible();
            #endregion

            #region value
            Hashtable grandtotal = new Hashtable();
            Hashtable fnlTot = new Hashtable();
            Hashtable lstTot = new Hashtable();
            Hashtable htdept = getDeptName();
            int height = 0;
            int roll = 0;
            #region student

            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
            {
                bool rowbool = false;
                bool paybool = false;
                string date = Convert.ToString(ds.Tables[0].Rows[row]["Transdate"]);
                ds.Tables[1].DefaultView.RowFilter = "Transdate='" + date + "'";
                DataView dvdt = ds.Tables[1].DefaultView;
                if (dvdt.Count > 0)
                {
                    for (int snd = 0; snd < dvdt.Count; snd++)
                    {
                        double FnltotAmount = 0;
                        bool payBool = false;
                        bool cblbool = true;
                        for (int fin = 0; fin < chklsfyear.Items.Count; fin++)
                        {
                            if (chklsfyear.Items[fin].Selected)
                            {
                                for (int sem = 0; sem < cbl_sem.Items.Count; sem++)
                                {
                                    if (cbl_sem.Items[sem].Selected)
                                    {
                                        if (rbtype.SelectedIndex == 0)
                                        {
                                            #region
                                            for (int hd = 0; hd < chkl_studhed.Items.Count; hd++)
                                            {
                                                if (chkl_studhed.Items[hd].Selected)
                                                {

                                                    string strVal = "finyearfk='" + chklsfyear.Items[fin].Value + "' and feecategory='" + cbl_sem.Items[sem].Value + "' and headerfk='" + chkl_studhed.Items[hd].Value + "' and Transdate='" + date + "' and app_no='" + dvdt[snd]["app_no"] + "'";
                                                    DataTable dtdet = new DataTable();
                                                    ds.Tables[2].DefaultView.RowFilter = strVal;
                                                    dtdet = ds.Tables[2].DefaultView.ToTable();
                                                    if (dtdet.Rows.Count > 0)
                                                    {
                                                        for (int dtsub = 0; dtsub < dtdet.Rows.Count; dtsub++)
                                                        {
                                                            string hashValue = chklsfyear.Items[fin].Value + "-" + cbl_sem.Items[sem].Value + "-" + chkl_studhed.Items[hd].Value + "-" + Convert.ToString(dtdet.Rows[dtsub]["actualfinyearfk"]); ;
                                                            double paiAmount = 0;
                                                            int curColCnt = 0;
                                                            int.TryParse(Convert.ToString(htColCnt[hashValue]), out curColCnt);
                                                            double.TryParse(Convert.ToString(dtdet.Rows[dtsub]["debit"]), out paiAmount);
                                                            FnltotAmount += paiAmount;
                                                            payBool = true;
                                                            rowbool = true;
                                                            if (!grandtotal.ContainsKey(curColCnt))
                                                                grandtotal.Add(curColCnt, Convert.ToString(paiAmount));
                                                            else
                                                            {
                                                                double amount = 0;
                                                                double.TryParse(Convert.ToString(grandtotal[curColCnt]), out amount);
                                                                amount += paiAmount;
                                                                grandtotal.Remove(curColCnt);
                                                                grandtotal.Add(curColCnt, Convert.ToString(amount));
                                                            }
                                                            if (cblbool)
                                                                spreadDet.Sheets[0].RowCount++;
                                                            if (paiAmount == 0)
                                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = "-";
                                                            else
                                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(paiAmount);
                                                            cblbool = false;
                                                        }
                                                    }

                                                }
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region
                                            for (int hd = 0; hd < chkl_studled.Items.Count; hd++)
                                            {
                                                if (chkl_studled.Items[hd].Selected)
                                                {

                                                    string strVal = "finyearfk='" + chklsfyear.Items[fin].Value + "' and feecategory='" + cbl_sem.Items[sem].Value + "' and ledgerfk='" + chkl_studled.Items[hd].Value + "' and Transdate='" + date + "' and app_no='" + dvdt[snd]["app_no"] + "'";
                                                    DataTable dtdet = new DataTable();
                                                    ds.Tables[2].DefaultView.RowFilter = strVal;
                                                    dtdet = ds.Tables[2].DefaultView.ToTable();
                                                    if (dtdet.Rows.Count > 0)
                                                    {
                                                        for (int dtsub = 0; dtsub < dtdet.Rows.Count; dtsub++)
                                                        {
                                                            string hashValue = chklsfyear.Items[fin].Value + "-" + cbl_sem.Items[sem].Value + "-" + chkl_studled.Items[hd].Value + "-" + Convert.ToString(dtdet.Rows[dtsub]["actualfinyearfk"]); ;
                                                            double paiAmount = 0;
                                                            int curColCnt = 0;
                                                            int.TryParse(Convert.ToString(htColCnt[hashValue]), out curColCnt);
                                                            double.TryParse(Convert.ToString(dtdet.Rows[dtsub]["debit"]), out paiAmount);
                                                            FnltotAmount += paiAmount;
                                                            payBool = true;
                                                            rowbool = true;
                                                            if (!grandtotal.ContainsKey(curColCnt))
                                                                grandtotal.Add(curColCnt, Convert.ToString(paiAmount));
                                                            else
                                                            {
                                                                double amount = 0;
                                                                double.TryParse(Convert.ToString(grandtotal[curColCnt]), out amount);
                                                                amount += paiAmount;
                                                                grandtotal.Remove(curColCnt);
                                                                grandtotal.Add(curColCnt, Convert.ToString(amount));
                                                            }
                                                            if (cblbool)
                                                                spreadDet.Sheets[0].RowCount++;
                                                            if (paiAmount == 0)
                                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = "-";
                                                            else
                                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(paiAmount);
                                                            cblbool = false;
                                                        }
                                                    }

                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                }
                            }
                        }
                        if (payBool && ds.Tables[2].Rows.Count > 0)
                        {
                            #region paymode
                            for (int s = 0; s < chkl_paid.Items.Count; s++)
                            {
                                if (chkl_paid.Items[s].Selected == true)
                                {
                                    string payModeVal = Convert.ToString(chkl_paid.Items[s].Value);
                                    string strVal = " Transdate='" + date + "' and paymode='" + payModeVal + "' and app_no='" + dvdt[snd]["app_no"] + "'";
                                    int curColCnt = 0;
                                    double paiAmount = 0;
                                    int.TryParse(Convert.ToString(htPayCol[payModeVal]), out curColCnt);
                                    DataView dvhd = new DataView();
                                    ds.Tables[3].DefaultView.RowFilter = strVal;
                                    dvhd = ds.Tables[3].DefaultView;
                                    if (dvhd.Count > 0)
                                    {
                                        for (int i = 0; i < dvhd.Count; i++)
                                        {
                                            double temp = 0;
                                            double.TryParse(Convert.ToString(dvhd[i]["debit"]), out temp);
                                            paiAmount += temp;
                                        }
                                    }
                                    if (!grandtotal.ContainsKey(curColCnt))
                                        grandtotal.Add(curColCnt, Convert.ToString(paiAmount));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(grandtotal[curColCnt]), out amount);
                                        amount += paiAmount;
                                        grandtotal.Remove(curColCnt);
                                        grandtotal.Add(curColCnt, Convert.ToString(amount));
                                    }
                                    if (paiAmount != 0)
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(paiAmount);
                                    else
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = "-";
                                    if (payModeVal == "1")
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].BackColor = ColorTranslator.FromHtml("#F08080");
                                    else if (payModeVal == "2")
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].BackColor = ColorTranslator.FromHtml("#D3D3D3");
                                    else if (payModeVal == "3")
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].BackColor = ColorTranslator.FromHtml("#FFA500");
                                    else if (payModeVal == "4")
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].BackColor = ColorTranslator.FromHtml("#90EE90");
                                    else if (payModeVal == "5")
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].BackColor = ColorTranslator.FromHtml("#FAFAD2");
                                }
                            }
                            #endregion
                        }
                        if (rowbool)
                        {
                            #region student detail
                            roll++;
                            string degcode = Convert.ToString(dvdt[snd]["degree_code"]);
                            string rollno = Convert.ToString(dvdt[snd]["Roll_No"]);
                            string regno = Convert.ToString(dvdt[snd]["Reg_No"]);
                            string admisno = Convert.ToString(dvdt[snd]["Roll_admit"]);
                            string applno = Convert.ToString(dvdt[snd]["Roll_admit"]);
                            string studName = Convert.ToString(dvdt[snd]["stud_name"]);
                            string Mode = Convert.ToString(dvdt[snd]["mode"]);
                            Mode = studentMode(Mode);
                            // string strMode = Mode == "New" ? Mode : "";
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(roll);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["date"]);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(studName) + "-(" + Mode + ")";
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(rollno);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(regno);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(admisno);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(applno);
                            DataView Dview = new DataView();
                            string Degreename = Convert.ToString(htdept[degcode]);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(Degreename);
                            //total column
                            int curColCnt = 0;
                            int.TryParse(Convert.ToString(htColCnt["Total"]), out curColCnt);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(FnltotAmount);
                            if (!grandtotal.ContainsKey(curColCnt))
                                grandtotal.Add(curColCnt, Convert.ToString(FnltotAmount));
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(grandtotal[curColCnt]), out amount);
                                amount += FnltotAmount;
                                grandtotal.Remove(curColCnt);
                                grandtotal.Add(curColCnt, Convert.ToString(amount));
                            }
                            double grandvalue = 0;
                            for (int j = 8; j < spreadDet.Sheets[0].ColumnCount; j++)
                            {
                                double.TryParse(Convert.ToString(grandtotal[j]), out grandvalue);
                                //  spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalue);
                                if (!fnlTot.ContainsKey(j))
                                    fnlTot.Add(j, Convert.ToString(grandvalue));
                                else
                                {
                                    double amount = 0;
                                    double.TryParse(Convert.ToString(fnlTot[j]), out amount);
                                    amount += grandvalue;
                                    fnlTot.Remove(j);
                                    fnlTot.Add(j, Convert.ToString(amount));
                                }
                            }
                            grandtotal.Clear();
                            height += 15;
                            #endregion
                        }
                    }
                }
            }
            spreadDet.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            spreadDet.Sheets[0].SetColumnMerge(7, FarPoint.Web.Spread.Model.MergePolicy.Always);
            if (fnlTot.Count > 0)
            {
                #region student total
                spreadDet.Sheets[0].Rows.Count++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Total";
                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
                double grandvalues = 0;
                for (int j = 8; j < spreadDet.Sheets[0].ColumnCount; j++)
                {
                    double.TryParse(Convert.ToString(fnlTot[j]), out grandvalues);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                    if (!lstTot.ContainsKey(j))
                        lstTot.Add(j, Convert.ToString(grandvalues));
                    else
                    {
                        double amount = 0;
                        double.TryParse(Convert.ToString(lstTot[j]), out amount);
                        amount += grandvalues;
                        lstTot.Remove(j);
                        lstTot.Add(j, Convert.ToString(amount));
                    }
                }
                fnlTot.Clear();
                #endregion
            }

            #endregion

            try
            {
                #region Staff,vendor,other
                int memValue = 0;
                int memCnt = 0;
                for (int mem = 0; mem < cblmem.Items.Count; mem++)
                {
                    int tblZero = 0;
                    int tblFirst = 0;
                    int tblSecond = 0;
                    int tblThird = 0;
                    bool boolMem = false;
                    bool boolMemRow = false;
                    int memRowCnt = 0;
                    if (cblmem.Items[mem].Selected)
                    {
                        if (cblmem.Items[mem].Text.Trim() == "Staff")
                        {
                            tblZero = 5;
                            tblFirst = 6;
                            tblSecond = 7;
                            tblThird = 8;
                            memValue = 2;
                            memCnt++;
                        }
                        else if (cblmem.Items[mem].Text.Trim() == "Vendor")
                        {

                            if (memCnt == 1)
                            {
                                tblZero = 9;
                                tblFirst = 10;
                                tblSecond = 11;
                                tblThird = 12;
                            }
                            else
                            {
                                tblZero = 5;
                                tblFirst = 6;
                                tblSecond = 7;
                                tblThird = 8;
                            }
                            memValue = 3;
                            memCnt++;
                        }
                        else if (cblmem.Items[mem].Text.Trim() == "Others")
                        {
                            if (memCnt == 2)
                            {
                                tblZero = 13;
                                tblFirst = 14;
                                tblSecond = 15;
                                tblThird = 16;
                            }
                            else if (memCnt == 1)
                            {
                                tblZero = 9;
                                tblFirst = 10;
                                tblSecond = 11;
                                tblThird = 12;
                            }
                            else
                            {
                                tblZero = 5;
                                tblFirst = 6;
                                tblSecond = 7;
                                tblThird = 8;
                            }
                            memValue = 4;
                            memCnt++;
                        }
                        #region
                        string date = string.Empty;
                        string datetxt = string.Empty;
                        ds.Tables[tblZero].DefaultView.RowFilter = "Memtype='" + cblmem.Items[mem].Value + "'";
                        DataTable dtPaid = ds.Tables[tblZero].DefaultView.ToTable();
                        if (dtPaid.Rows.Count > 0)
                        {
                            for (int row = 0; row < dtPaid.Rows.Count; row++)
                            {
                                bool rowbool = false;
                                bool paybool = false;
                                date = Convert.ToString(dtPaid.Rows[row]["Transdate"]);
                                datetxt = Convert.ToString(dtPaid.Rows[row]["date"]);
                                ds.Tables[tblFirst].DefaultView.RowFilter = "Transdate='" + date + "' and Memtype='" + cblmem.Items[mem].Value + "'";
                                DataView dvdt = ds.Tables[tblFirst].DefaultView;
                                if (dvdt.Count > 0)
                                {
                                    for (int snd = 0; snd < dvdt.Count; snd++)
                                    {
                                        double FnltotAmount = 0;
                                        bool payBool = false;
                                        bool cblbool = true;
                                        for (int fin = 0; fin < chklsfyear.Items.Count; fin++)
                                        {
                                            if (chklsfyear.Items[fin].Selected)
                                            {
                                                if (rbtype.SelectedIndex == 0)
                                                {
                                                    #region
                                                    for (int hd = 0; hd < chkl_studhed.Items.Count; hd++)
                                                    {
                                                        if (chkl_studhed.Items[hd].Selected)
                                                        {
                                                            string strVal = "finyearfk='" + chklsfyear.Items[fin].Value + "' and Memtype='" + cblmem.Items[mem].Value + "' and headerfk='" + chkl_studhed.Items[hd].Value + "' and Transdate='" + date + "' and app_no='" + dvdt[snd]["app_no"] + "'";
                                                            DataTable dtdet = new DataTable();
                                                            ds.Tables[tblSecond].DefaultView.RowFilter = strVal;
                                                            dtdet = ds.Tables[tblSecond].DefaultView.ToTable();
                                                            if (dtdet.Rows.Count > 0)
                                                            {
                                                                for (int dtsub = 0; dtsub < dtdet.Rows.Count; dtsub++)
                                                                {
                                                                    string hashValue = chklsfyear.Items[fin].Value + "-" + chkl_studhed.Items[hd].Value;
                                                                    double paiAmount = 0;
                                                                    int curColCnt = 0;
                                                                    int.TryParse(Convert.ToString(htColCntOther[hashValue]), out curColCnt);
                                                                    double.TryParse(Convert.ToString(dtdet.Rows[dtsub]["debit"]), out paiAmount);
                                                                    FnltotAmount += paiAmount;
                                                                    payBool = true;
                                                                    rowbool = true;
                                                                    if (!grandtotal.ContainsKey(curColCnt))
                                                                        grandtotal.Add(curColCnt, Convert.ToString(paiAmount));
                                                                    else
                                                                    {
                                                                        double amount = 0;
                                                                        double.TryParse(Convert.ToString(grandtotal[curColCnt]), out amount);
                                                                        amount += paiAmount;
                                                                        grandtotal.Remove(curColCnt);
                                                                        grandtotal.Add(curColCnt, Convert.ToString(amount));
                                                                    }
                                                                    if (!boolMem)//memtype name bind in spread
                                                                        memRowCnt = ++spreadDet.Sheets[0].RowCount;
                                                                    boolMem = true;
                                                                    if (cblbool)
                                                                        spreadDet.Sheets[0].RowCount++;
                                                                    if (paiAmount == 0)
                                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = "-";
                                                                    else
                                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(paiAmount);
                                                                    cblbool = false;
                                                                }
                                                            }

                                                        }
                                                    }
                                                    #endregion
                                                }
                                                else
                                                {
                                                    #region
                                                    for (int hd = 0; hd < chkl_studled.Items.Count; hd++)
                                                    {
                                                        if (chkl_studled.Items[hd].Selected)
                                                        {

                                                            string strVal = "finyearfk='" + chklsfyear.Items[fin].Value + "' and Memtype='" + cblmem.Items[mem].Value + "' and ledgerfk='" + chkl_studled.Items[hd].Value + "' and Transdate='" + date + "' and app_no='" + dvdt[snd]["app_no"] + "'";
                                                            DataTable dtdet = new DataTable();
                                                            ds.Tables[tblSecond].DefaultView.RowFilter = strVal;
                                                            dtdet = ds.Tables[tblSecond].DefaultView.ToTable();
                                                            if (dtdet.Rows.Count > 0)
                                                            {
                                                                for (int dtsub = 0; dtsub < dtdet.Rows.Count; dtsub++)
                                                                {
                                                                    string hashValue = chklsfyear.Items[fin].Value + "-" + chkl_studled.Items[hd].Value;
                                                                    double paiAmount = 0;
                                                                    int curColCnt = 0;
                                                                    int.TryParse(Convert.ToString(htColCntOther[hashValue]), out curColCnt);
                                                                    double.TryParse(Convert.ToString(dtdet.Rows[dtsub]["debit"]), out paiAmount);
                                                                    FnltotAmount += paiAmount;
                                                                    payBool = true;
                                                                    rowbool = true;
                                                                    if (!grandtotal.ContainsKey(curColCnt))
                                                                        grandtotal.Add(curColCnt, Convert.ToString(paiAmount));
                                                                    else
                                                                    {
                                                                        double amount = 0;
                                                                        double.TryParse(Convert.ToString(grandtotal[curColCnt]), out amount);
                                                                        amount += paiAmount;
                                                                        grandtotal.Remove(curColCnt);
                                                                        grandtotal.Add(curColCnt, Convert.ToString(amount));
                                                                    }
                                                                    if (!boolMem)//memtype name bind in spread
                                                                        memRowCnt = ++spreadDet.Sheets[0].RowCount;
                                                                    boolMem = true;
                                                                    if (cblbool)
                                                                        spreadDet.Sheets[0].RowCount++;
                                                                    if (paiAmount == 0)
                                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = "-";
                                                                    else
                                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(paiAmount);
                                                                    cblbool = false;
                                                                }
                                                            }

                                                        }
                                                    }
                                                    #endregion
                                                }
                                            }
                                        }
                                        if (payBool && ds.Tables[tblThird].Rows.Count > 0)
                                        {
                                            #region paymode
                                            for (int s = 0; s < chkl_paid.Items.Count; s++)
                                            {
                                                if (chkl_paid.Items[s].Selected == true)
                                                {
                                                    string payModeVal = Convert.ToString(chkl_paid.Items[s].Value);
                                                    string strVal = " Transdate='" + date + "' and paymode='" + payModeVal + "' and app_no='" + dvdt[snd]["app_no"] + "'";
                                                    int curColCnt = 0;
                                                    double paiAmount = 0;
                                                    int.TryParse(Convert.ToString(htPayCol[payModeVal]), out curColCnt);
                                                    DataView dvhd = new DataView();
                                                    ds.Tables[tblThird].DefaultView.RowFilter = strVal;
                                                    dvhd = ds.Tables[tblThird].DefaultView;
                                                    if (dvhd.Count > 0)
                                                    {
                                                        for (int i = 0; i < dvhd.Count; i++)
                                                        {
                                                            double temp = 0;
                                                            double.TryParse(Convert.ToString(dvhd[i]["debit"]), out temp);
                                                            paiAmount += temp;
                                                        }
                                                    }
                                                    if (!grandtotal.ContainsKey(curColCnt))
                                                        grandtotal.Add(curColCnt, Convert.ToString(paiAmount));
                                                    else
                                                    {
                                                        double amount = 0;
                                                        double.TryParse(Convert.ToString(grandtotal[curColCnt]), out amount);
                                                        amount += paiAmount;
                                                        grandtotal.Remove(curColCnt);
                                                        grandtotal.Add(curColCnt, Convert.ToString(amount));
                                                    }
                                                    if (paiAmount != 0)
                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(paiAmount);
                                                    else
                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = "-";
                                                    if (payModeVal == "1")
                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].BackColor = ColorTranslator.FromHtml("#F08080");
                                                    else if (payModeVal == "2")
                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].BackColor = ColorTranslator.FromHtml("#D3D3D3");
                                                    else if (payModeVal == "3")
                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].BackColor = ColorTranslator.FromHtml("#FFA500");
                                                    else if (payModeVal == "4")
                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].BackColor = ColorTranslator.FromHtml("#90EE90");
                                                    else if (payModeVal == "5")
                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].BackColor = ColorTranslator.FromHtml("#FAFAD2");
                                                }
                                            }
                                            #endregion
                                        }
                                        if (rowbool)
                                        {
                                            #region student detail
                                            roll++;
                                            string degcode = string.Empty;
                                            string rollno = string.Empty;
                                            string regno = string.Empty;
                                            string admisno = string.Empty;
                                            string applno = string.Empty;
                                            string studName = string.Empty;
                                            string Mode = string.Empty;
                                            if (memValue == 2)
                                            {
                                                degcode = Convert.ToString(dvdt[snd]["dept_name"]);
                                                rollno = Convert.ToString(dvdt[snd]["staff_code"]);
                                                regno = Convert.ToString(dvdt[snd]["staff_code"]);
                                                admisno = Convert.ToString(dvdt[snd]["staff_code"]);
                                                applno = Convert.ToString(dvdt[snd]["staff_code"]);
                                                studName = Convert.ToString(dvdt[snd]["staff_name"]);

                                            }
                                            else if (memValue == 3)
                                            {
                                                rollno = Convert.ToString(dvdt[snd]["vendorcode"]);
                                                regno = Convert.ToString(dvdt[snd]["vendorcode"]);
                                                admisno = Convert.ToString(dvdt[snd]["vendorcode"]);
                                                applno = Convert.ToString(dvdt[snd]["vendorcode"]);
                                                studName = Convert.ToString(dvdt[snd]["vendorname"]);

                                            }
                                            else if (memValue == 4)
                                            {
                                                rollno = Convert.ToString(dvdt[snd]["vendorcode"]);
                                                regno = Convert.ToString(dvdt[snd]["vendorcode"]);
                                                admisno = Convert.ToString(dvdt[snd]["vendorcode"]);
                                                applno = Convert.ToString(dvdt[snd]["vendorcode"]);
                                                studName = Convert.ToString(dvdt[snd]["vendorname"]);
                                            }
                                            if (!boolMemRow)
                                            {
                                                spreadDet.Sheets[0].Cells[memRowCnt - 1, 0].Text = Convert.ToString(cblmem.Items[mem].Text);
                                                spreadDet.Sheets[0].SpanModel.Add(memRowCnt - 1, 0, 1, 4);
                                                spreadDet.Sheets[0].Rows[memRowCnt - 1].BackColor = Color.Gray;
                                                spreadDet.Sheets[0].Rows[memRowCnt - 1].HorizontalAlign = HorizontalAlign.Center;
                                                boolMemRow = true;
                                            }
                                            //int rowd = Convert.ToInt32(spreadDet.Sheets[0].RowCount - 1);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(roll);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = datetxt;
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(studName);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(rollno);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(regno);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(admisno);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(applno);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(degcode);
                                            //total column
                                            int curColCnt = 0;
                                            int.TryParse(Convert.ToString(htColCnt["Total"]), out curColCnt);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(FnltotAmount);
                                            if (!grandtotal.ContainsKey(curColCnt))
                                                grandtotal.Add(curColCnt, Convert.ToString(FnltotAmount));
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(grandtotal[curColCnt]), out amount);
                                                amount += FnltotAmount;
                                                grandtotal.Remove(curColCnt);
                                                grandtotal.Add(curColCnt, Convert.ToString(amount));
                                            }
                                            double grandvalue = 0;
                                            for (int j = 8; j < spreadDet.Sheets[0].ColumnCount; j++)
                                            {
                                                double.TryParse(Convert.ToString(grandtotal[j]), out grandvalue);
                                                //  spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalue);
                                                if (!fnlTot.ContainsKey(j))
                                                    fnlTot.Add(j, Convert.ToString(grandvalue));
                                                else
                                                {
                                                    double amount = 0;
                                                    double.TryParse(Convert.ToString(fnlTot[j]), out amount);
                                                    amount += grandvalue;
                                                    fnlTot.Remove(j);
                                                    fnlTot.Add(j, Convert.ToString(amount));
                                                }
                                            }
                                            grandtotal.Clear();
                                            height += 15;
                                            #endregion
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }

                spreadDet.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                spreadDet.Sheets[0].SetColumnMerge(7, FarPoint.Web.Spread.Model.MergePolicy.Always);
                if (fnlTot.Count > 0)
                {
                    #region other total
                    spreadDet.Sheets[0].Rows.Count++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Total";
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
                    double grandvalues = 0;
                    for (int j = 8; j < spreadDet.Sheets[0].ColumnCount; j++)
                    {
                        double.TryParse(Convert.ToString(fnlTot[j]), out grandvalues);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                        if (!lstTot.ContainsKey(j))
                            lstTot.Add(j, Convert.ToString(grandvalues));
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(lstTot[j]), out amount);
                            amount += grandvalues;
                            lstTot.Remove(j);
                            lstTot.Add(j, Convert.ToString(amount));
                        }
                    }
                    #endregion
                }

                #endregion
            }
            catch { }

            #region grandtot
            if (lstTot.Count > 0)
            {
                spreadDet.Sheets[0].Rows.Count++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
                double grandvalues = 0;
                for (int j = 8; j < spreadDet.Sheets[0].ColumnCount; j++)
                {
                    double.TryParse(Convert.ToString(lstTot[j]), out grandvalues);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                }
            }
            #endregion

            spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
            payModeLabels(htPayCol);
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            spreadDet.Visible = true;
            print.Visible = true;
            spreadDet.Height = 200 + height;
            spreadDet.SaveChanges();

            #endregion
        }
        catch
        { }
    }

    //with bill no wise
    protected void spreadLoadDetailedBillNo(DataSet ds, ref string fromdate, ref string todate)
    {
        try
        {
            #region design
            RollAndRegSettings();
            spreadDet.Sheets[0].RowCount = 0;
            spreadDet.Sheets[0].ColumnCount = 0;
            spreadDet.CommandBar.Visible = false;
            spreadDet.Sheets[0].AutoPostBack = true;
            spreadDet.Sheets[0].ColumnHeader.RowCount = 3;
            spreadDet.Sheets[0].RowHeader.Visible = false;
            spreadDet.Sheets[0].ColumnCount = 9;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            spreadDet.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Date";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Receipt No";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[2].Width = 350;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Roll No";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Reg No";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Admission No";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;


            spreadDet.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Application No";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 7].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[7].Visible = false;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 8].Text = lbldept.Text;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 8].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Left;


            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 3, 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 3, 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 3, 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 3, 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 3, 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 3, 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 3, 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 3, 1);

            #region financial year

            Dictionary<string, string> dtMode = getstudMode();
            Hashtable htHDName = getHeaderFK();
            Hashtable htActYr = getFinyear();
            Hashtable htColCnt = new Hashtable();
            Hashtable htColCntOther = new Hashtable();
            for (int row = 0; row < chklsfyear.Items.Count; row++)
            {
                string tempfnlyr = string.Empty;
                if (chklsfyear.Items[row].Selected)
                {
                    bool checkbatch = false;
                    int batchCol = 0;
                    bool batchbol = false;
                    string hdrTxtValue = string.Empty;
                    int col = spreadDet.Sheets[0].ColumnCount++;
                    hdrTxtValue = Convert.ToString(chklsfyear.Items[row].Value);
                    tempfnlyr = Convert.ToString(chklsfyear.Items[row].Value);
                    //foreach (KeyValuePair<string, string> rMode in dtMode)
                    //{
                    for (int sem = 0; sem < cbl_sem.Items.Count; sem++)
                    {
                        int semcol = 0;
                        int tempsemcol = 0;
                        if (cbl_sem.Items[sem].Selected)
                        {
                            bool sembol = false;
                            hdrTxtValue += "-" + Convert.ToString(cbl_sem.Items[sem].Value);
                            //if (batchbol)
                            //    semcol = spreadDet.Sheets[0].ColumnCount++;
                            //else
                            //    semcol = col;                           
                            ds.Tables[4].DefaultView.RowFilter = " finyearfk ='" + chklsfyear.Items[row].Value + "' and feecategory='" + cbl_sem.Items[sem].Value + "'";//mode='" + rMode.Value + "' and
                            DataTable dthd = ds.Tables[4].DefaultView.ToTable();
                            if (dthd.Rows.Count > 0)
                            {
                                if (batchbol)
                                    semcol = spreadDet.Sheets[0].ColumnCount++;
                                else
                                    semcol = col;
                                #region header
                                int semcolcnt = 0;
                                for (int hd = 0; hd < dthd.Rows.Count; hd++)
                                {
                                    semcolcnt++;
                                    batchCol++;
                                    if (sembol)
                                        spreadDet.Sheets[0].ColumnCount++;
                                    string hdName = string.Empty;
                                    string actFinFk = string.Empty;
                                    string hdorldFK = string.Empty;

                                    string actualYEar = Convert.ToString(htActYr[Convert.ToString(dthd.Rows[hd]["actualfinyearfk"]).Trim()]);
                                    actFinFk = Convert.ToString(dthd.Rows[hd]["actualfinyearfk"]);

                                    if (rbtype.SelectedIndex == 0)
                                    {
                                        hdName = Convert.ToString(htHDName[Convert.ToString(dthd.Rows[hd]["headerfk"]).Trim()]);
                                        spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(dthd.Rows[hd]["headerfk"]);
                                        hdorldFK = Convert.ToString(dthd.Rows[hd]["headerfk"]);
                                    }
                                    else
                                    {
                                        hdName = Convert.ToString(htHDName[Convert.ToString(dthd.Rows[hd]["ledgerfk"]).Trim()]);
                                        spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(dthd.Rows[hd]["ledgerfk"]);
                                        hdorldFK = Convert.ToString(dthd.Rows[hd]["ledgerfk"]);
                                    }
                                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Text = hdName + "(" + actualYEar + ")";
                                    string value = hdrTxtValue + "-" + hdorldFK + "-" + actFinFk;
                                    string tempValue = tempfnlyr + "-" + hdorldFK;
                                    if (!htColCnt.ContainsKey(value))
                                        htColCnt.Add(value, spreadDet.Sheets[0].ColumnCount - 1);
                                    if (!htColCntOther.ContainsKey(tempValue))
                                        htColCntOther.Add(tempValue, spreadDet.Sheets[0].ColumnCount - 1);
                                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                    spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                    sembol = true;
                                    checkbatch = true;
                                }
                                batchbol = true;
                                if (semcolcnt > 0)
                                {
                                    spreadDet.Sheets[0].ColumnHeader.Cells[1, semcol].Text = Convert.ToString(cbl_sem.Items[sem].Text);
                                    spreadDet.Sheets[0].ColumnHeader.Cells[1, semcol].Tag = Convert.ToString(cbl_sem.Items[sem].Value);
                                    // hdrTxtValue += "-" + Convert.ToString(cbl_sem.Items[sem].Value);
                                    spreadDet.Sheets[0].ColumnHeader.Cells[1, semcol].ForeColor = ColorTranslator.FromHtml("#000000");
                                    spreadDet.Sheets[0].ColumnHeader.Cells[1, semcol].Font.Bold = true;
                                    spreadDet.Sheets[0].ColumnHeader.Cells[1, semcol].Font.Name = "Book Antiqua";
                                    spreadDet.Sheets[0].ColumnHeader.Cells[1, semcol].Font.Size = FontUnit.Medium;
                                    spreadDet.Sheets[0].ColumnHeader.Cells[1, semcol].HorizontalAlign = HorizontalAlign.Center;
                                    spreadDet.Sheets[0].Columns[semcol].HorizontalAlign = HorizontalAlign.Right;
                                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(1, semcol, 1, semcolcnt);
                                }
                                #endregion
                            }
                        }
                        hdrTxtValue = Convert.ToString(chklsfyear.Items[row].Value);
                    }
                    if (checkbatch)
                    {
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Text = Convert.ToString(htActYr[Convert.ToString(chklsfyear.Items[row].Value).Trim()]);// Convert.ToString(chklsfyear.Items[row].Text);
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Tag = Convert.ToString(chklsfyear.Items[row].Value);
                        // hdrTxtValue += "-" + Convert.ToString(cbl_sem.Items[sem].Value);
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, col].ForeColor = ColorTranslator.FromHtml("#000000");
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Bold = true;
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Name = "Book Antiqua";
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Size = FontUnit.Medium;
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, col].HorizontalAlign = HorizontalAlign.Center;
                        spreadDet.Sheets[0].Columns[col].HorizontalAlign = HorizontalAlign.Right;
                        spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, col, 1, batchCol);
                    }
                    else
                        spreadDet.Sheets[0].ColumnCount--;

                    //}
                }
            }
            #endregion

            int paycol = spreadDet.Sheets[0].ColumnCount++;
            htColCnt.Add("Total", spreadDet.Sheets[0].ColumnCount - 1);
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Text = "Total";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, paycol, 3, 1);

            Hashtable htPayCol = new Hashtable();
            int check = 0;
            paycol = spreadDet.Sheets[0].ColumnCount++;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Text = "PayMode";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

            int checkva = 0;
            for (int s = 0; s < chkl_paid.Items.Count; s++)
            {
                if (chkl_paid.Items[s].Selected == true)
                {
                    checkva++;
                    if (checkva > 1)
                        check = spreadDet.Sheets[0].ColumnCount++;
                    htPayCol.Add(Convert.ToString(chkl_paid.Items[s].Value), spreadDet.Sheets[0].ColumnCount - 1);
                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(chkl_paid.Items[s].Text);
                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(chkl_paid.Items[s].Value);
                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;

                }
            }
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(2, check, 1, spreadDet.Sheets[0].ColumnCount - 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, paycol, 2, spreadDet.Sheets[0].ColumnCount - 1);
            spreadColumnVisible();
            #endregion

            #region value
            Hashtable grandtotal = new Hashtable();
            Hashtable fnlTot = new Hashtable();
            Hashtable lstTot = new Hashtable();
            Hashtable htdept = getDeptName();
            int height = 0;
            int roll = 0;
            #region student
            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
            {
                bool rowbool = false;
                bool paybool = false;
                string date = Convert.ToString(ds.Tables[0].Rows[row]["Transdate"]);
                ds.Tables[1].DefaultView.RowFilter = "Transdate='" + date + "'";
                DataView dvdt = ds.Tables[1].DefaultView;
                if (dvdt.Count > 0)
                {
                    for (int snd = 0; snd < dvdt.Count; snd++)
                    {
                        double FnltotAmount = 0;
                        bool payBool = false;
                        bool cblbool = true;
                        for (int fin = 0; fin < chklsfyear.Items.Count; fin++)
                        {
                            if (chklsfyear.Items[fin].Selected)
                            {
                                for (int sem = 0; sem < cbl_sem.Items.Count; sem++)
                                {
                                    if (cbl_sem.Items[sem].Selected)
                                    {
                                        if (rbtype.SelectedIndex == 0)
                                        {
                                            #region
                                            for (int hd = 0; hd < chkl_studhed.Items.Count; hd++)
                                            {
                                                if (chkl_studhed.Items[hd].Selected)
                                                {

                                                    string strVal = "finyearfk='" + chklsfyear.Items[fin].Value + "' and feecategory='" + cbl_sem.Items[sem].Value + "' and headerfk='" + chkl_studhed.Items[hd].Value + "' and Transdate='" + date + "' and app_no='" + dvdt[snd]["app_no"] + "' and Transcode='" + dvdt[snd]["Transcode"] + "'";
                                                    DataTable dtdet = new DataTable();
                                                    ds.Tables[2].DefaultView.RowFilter = strVal;
                                                    dtdet = ds.Tables[2].DefaultView.ToTable();
                                                    if (dtdet.Rows.Count > 0)
                                                    {
                                                        for (int dtsub = 0; dtsub < dtdet.Rows.Count; dtsub++)
                                                        {
                                                            string hashValue = chklsfyear.Items[fin].Value + "-" + cbl_sem.Items[sem].Value + "-" + chkl_studhed.Items[hd].Value + "-" + Convert.ToString(dtdet.Rows[dtsub]["actualfinyearfk"]); ;
                                                            double paiAmount = 0;
                                                            int curColCnt = 0;
                                                            int.TryParse(Convert.ToString(htColCnt[hashValue]), out curColCnt);
                                                            double.TryParse(Convert.ToString(dtdet.Rows[dtsub]["debit"]), out paiAmount);
                                                            FnltotAmount += paiAmount;
                                                            payBool = true;
                                                            rowbool = true;
                                                            if (!grandtotal.ContainsKey(curColCnt))
                                                                grandtotal.Add(curColCnt, Convert.ToString(paiAmount));
                                                            else
                                                            {
                                                                double amount = 0;
                                                                double.TryParse(Convert.ToString(grandtotal[curColCnt]), out amount);
                                                                amount += paiAmount;
                                                                grandtotal.Remove(curColCnt);
                                                                grandtotal.Add(curColCnt, Convert.ToString(amount));
                                                            }
                                                            if (cblbool)
                                                                spreadDet.Sheets[0].RowCount++;
                                                            if (paiAmount == 0)
                                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = "-";
                                                            else
                                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(paiAmount);
                                                            cblbool = false;
                                                        }
                                                    }

                                                }
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region
                                            for (int hd = 0; hd < chkl_studled.Items.Count; hd++)
                                            {
                                                if (chkl_studled.Items[hd].Selected)
                                                {

                                                    string strVal = "finyearfk='" + chklsfyear.Items[fin].Value + "' and feecategory='" + cbl_sem.Items[sem].Value + "' and ledgerfk='" + chkl_studled.Items[hd].Value + "' and Transdate='" + date + "' and app_no='" + dvdt[snd]["app_no"] + "' and Transcode='" + dvdt[snd]["Transcode"] + "'";
                                                    DataTable dtdet = new DataTable();
                                                    ds.Tables[2].DefaultView.RowFilter = strVal;
                                                    dtdet = ds.Tables[2].DefaultView.ToTable();
                                                    if (dtdet.Rows.Count > 0)
                                                    {
                                                        for (int dtsub = 0; dtsub < dtdet.Rows.Count; dtsub++)
                                                        {
                                                            string hashValue = chklsfyear.Items[fin].Value + "-" + cbl_sem.Items[sem].Value + "-" + chkl_studled.Items[hd].Value + "-" + Convert.ToString(dtdet.Rows[dtsub]["actualfinyearfk"]); ;
                                                            double paiAmount = 0;
                                                            int curColCnt = 0;
                                                            int.TryParse(Convert.ToString(htColCnt[hashValue]), out curColCnt);
                                                            double.TryParse(Convert.ToString(dtdet.Rows[dtsub]["debit"]), out paiAmount);
                                                            FnltotAmount += paiAmount;
                                                            payBool = true;
                                                            rowbool = true;
                                                            if (!grandtotal.ContainsKey(curColCnt))
                                                                grandtotal.Add(curColCnt, Convert.ToString(paiAmount));
                                                            else
                                                            {
                                                                double amount = 0;
                                                                double.TryParse(Convert.ToString(grandtotal[curColCnt]), out amount);
                                                                amount += paiAmount;
                                                                grandtotal.Remove(curColCnt);
                                                                grandtotal.Add(curColCnt, Convert.ToString(amount));
                                                            }
                                                            if (cblbool)
                                                                spreadDet.Sheets[0].RowCount++;
                                                            if (paiAmount == 0)
                                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = "-";
                                                            else
                                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(paiAmount);
                                                            cblbool = false;
                                                        }
                                                    }

                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                }
                            }
                        }
                        if (payBool && ds.Tables[2].Rows.Count > 0)
                        {
                            #region paymode
                            for (int s = 0; s < chkl_paid.Items.Count; s++)
                            {
                                if (chkl_paid.Items[s].Selected == true)
                                {
                                    string payModeVal = Convert.ToString(chkl_paid.Items[s].Value);
                                    string strVal = " Transdate='" + date + "' and paymode='" + payModeVal + "' and app_no='" + dvdt[snd]["app_no"] + "' and Transcode='" + dvdt[snd]["Transcode"] + "'";
                                    int curColCnt = 0;
                                    double paiAmount = 0;
                                    int.TryParse(Convert.ToString(htPayCol[payModeVal]), out curColCnt);
                                    DataView dvhd = new DataView();
                                    ds.Tables[3].DefaultView.RowFilter = strVal;
                                    dvhd = ds.Tables[3].DefaultView;
                                    if (dvhd.Count > 0)
                                    {
                                        for (int i = 0; i < dvhd.Count; i++)
                                        {
                                            double temp = 0;
                                            double.TryParse(Convert.ToString(dvhd[i]["debit"]), out temp);
                                            paiAmount += temp;
                                        }
                                    }
                                    if (!grandtotal.ContainsKey(curColCnt))
                                        grandtotal.Add(curColCnt, Convert.ToString(paiAmount));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(grandtotal[curColCnt]), out amount);
                                        amount += paiAmount;
                                        grandtotal.Remove(curColCnt);
                                        grandtotal.Add(curColCnt, Convert.ToString(amount));
                                    }
                                    if (paiAmount != 0)
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(paiAmount);
                                    else
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = "-";
                                    if (payModeVal == "1")
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].BackColor = ColorTranslator.FromHtml("#F08080");
                                    else if (payModeVal == "2")
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].BackColor = ColorTranslator.FromHtml("#D3D3D3");
                                    else if (payModeVal == "3")
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].BackColor = ColorTranslator.FromHtml("#FFA500");
                                    else if (payModeVal == "4")
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].BackColor = ColorTranslator.FromHtml("#90EE90");
                                    else if (payModeVal == "5")
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].BackColor = ColorTranslator.FromHtml("#FAFAD2");
                                }
                            }
                            #endregion
                        }
                        if (rowbool)
                        {
                            #region student detail
                            roll++;
                            string degcode = Convert.ToString(dvdt[snd]["degree_code"]);
                            string rollno = Convert.ToString(dvdt[snd]["Roll_No"]);
                            string regno = Convert.ToString(dvdt[snd]["Reg_No"]);
                            string admisno = Convert.ToString(dvdt[snd]["Roll_admit"]);
                            string applno = Convert.ToString(dvdt[snd]["Roll_admit"]);
                            string studName = Convert.ToString(dvdt[snd]["stud_name"]);
                            string reciptNo = Convert.ToString(dvdt[snd]["transcode"]);
                            string Mode = Convert.ToString(dvdt[snd]["mode"]);
                            Mode = studentMode(Mode);
                            // string strMode = Mode == "New" ? Mode : "";
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(roll);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["date"]);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = reciptNo;
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(studName) + "-(" + Mode + ")";
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(rollno);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(regno);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(admisno);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(applno);
                            DataView Dview = new DataView();
                            string Degreename = Convert.ToString(htdept[degcode]);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(Degreename);

                            //total column
                            int curColCnt = 0;
                            int.TryParse(Convert.ToString(htColCnt["Total"]), out curColCnt);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(FnltotAmount);
                            if (!grandtotal.ContainsKey(curColCnt))
                                grandtotal.Add(curColCnt, Convert.ToString(FnltotAmount));
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(grandtotal[curColCnt]), out amount);
                                amount += FnltotAmount;
                                grandtotal.Remove(curColCnt);
                                grandtotal.Add(curColCnt, Convert.ToString(amount));
                            }
                            double grandvalue = 0;
                            for (int j = 8; j < spreadDet.Sheets[0].ColumnCount; j++)
                            {
                                double.TryParse(Convert.ToString(grandtotal[j]), out grandvalue);
                                //  spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalue);
                                if (!fnlTot.ContainsKey(j))
                                    fnlTot.Add(j, Convert.ToString(grandvalue));
                                else
                                {
                                    double amount = 0;
                                    double.TryParse(Convert.ToString(fnlTot[j]), out amount);
                                    amount += grandvalue;
                                    fnlTot.Remove(j);
                                    fnlTot.Add(j, Convert.ToString(amount));
                                }
                            }
                            grandtotal.Clear();
                            height += 15;
                            #endregion
                        }
                    }
                }
            }
            spreadDet.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            spreadDet.Sheets[0].SetColumnMerge(8, FarPoint.Web.Spread.Model.MergePolicy.Always);
            if (fnlTot.Count > 0)
            {
                #region grandtot
                spreadDet.Sheets[0].Rows.Count++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Total";
                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
                double grandvalues = 0;
                for (int j = 9; j < spreadDet.Sheets[0].ColumnCount; j++)
                {
                    double.TryParse(Convert.ToString(fnlTot[j]), out grandvalues);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                    if (!lstTot.ContainsKey(j))
                        lstTot.Add(j, Convert.ToString(grandvalues));
                    else
                    {
                        double amount = 0;
                        double.TryParse(Convert.ToString(lstTot[j]), out amount);
                        amount += grandvalues;
                        lstTot.Remove(j);
                        lstTot.Add(j, Convert.ToString(amount));
                    }
                }
                fnlTot.Clear();
                #endregion
            }

            #endregion

            try
            {
                #region Staff,vendor,other
                int memValue = 0;
                int memCnt = 0;
                for (int mem = 0; mem < cblmem.Items.Count; mem++)
                {
                    int tblZero = 0;
                    int tblFirst = 0;
                    int tblSecond = 0;
                    int tblThird = 0;
                    bool boolMem = false;
                    bool boolMemRow = false;
                    int memRowCnt = 0;
                    if (cblmem.Items[mem].Selected)
                    {
                        if (cblmem.Items[mem].Text.Trim() == "Staff")
                        {
                            tblZero = 5;
                            tblFirst = 6;
                            tblSecond = 7;
                            tblThird = 8;
                            memValue = 2;
                            memCnt++;
                        }
                        else if (cblmem.Items[mem].Text.Trim() == "Vendor")
                        {

                            if (memCnt == 1)
                            {
                                tblZero = 9;
                                tblFirst = 10;
                                tblSecond = 11;
                                tblThird = 12;
                            }
                            else
                            {
                                tblZero = 5;
                                tblFirst = 6;
                                tblSecond = 7;
                                tblThird = 8;
                            }
                            memValue = 3;
                            memCnt++;
                        }
                        else if (cblmem.Items[mem].Text.Trim() == "Others")
                        {
                            if (memCnt == 2)
                            {
                                tblZero = 13;
                                tblFirst = 14;
                                tblSecond = 15;
                                tblThird = 16;
                            }
                            else if (memCnt == 1)
                            {
                                tblZero = 9;
                                tblFirst = 10;
                                tblSecond = 11;
                                tblThird = 12;
                            }
                            else
                            {
                                tblZero = 5;
                                tblFirst = 6;
                                tblSecond = 7;
                                tblThird = 8;
                            }
                            memValue = 4;
                            memCnt++;
                        }
                        #region
                        string date = string.Empty;
                        string datetxt = string.Empty;
                        ds.Tables[tblZero].DefaultView.RowFilter = "Memtype='" + cblmem.Items[mem].Value + "'";
                        DataTable dtPaid = ds.Tables[tblZero].DefaultView.ToTable();
                        if (dtPaid.Rows.Count > 0)
                        {
                            for (int row = 0; row < dtPaid.Rows.Count; row++)
                            {
                                bool rowbool = false;
                                bool paybool = false;
                                date = Convert.ToString(dtPaid.Rows[row]["Transdate"]);
                                datetxt = Convert.ToString(dtPaid.Rows[row]["date"]);
                                ds.Tables[tblFirst].DefaultView.RowFilter = "Transdate='" + date + "' and Memtype='" + cblmem.Items[mem].Value + "'";
                                DataView dvdt = ds.Tables[tblFirst].DefaultView;
                                if (dvdt.Count > 0)
                                {
                                    for (int snd = 0; snd < dvdt.Count; snd++)
                                    {
                                        double FnltotAmount = 0;
                                        bool payBool = false;
                                        bool cblbool = true;
                                        for (int fin = 0; fin < chklsfyear.Items.Count; fin++)
                                        {
                                            if (chklsfyear.Items[fin].Selected)
                                            {
                                                if (rbtype.SelectedIndex == 0)
                                                {
                                                    #region
                                                    for (int hd = 0; hd < chkl_studhed.Items.Count; hd++)
                                                    {
                                                        if (chkl_studhed.Items[hd].Selected)
                                                        {
                                                            string strVal = "finyearfk='" + chklsfyear.Items[fin].Value + "' and Memtype='" + cblmem.Items[mem].Value + "' and headerfk='" + chkl_studhed.Items[hd].Value + "' and Transdate='" + date + "' and app_no='" + dvdt[snd]["app_no"] + "' and Transcode='" + dvdt[snd]["Transcode"] + "'";
                                                            DataTable dtdet = new DataTable();
                                                            ds.Tables[tblSecond].DefaultView.RowFilter = strVal;
                                                            dtdet = ds.Tables[tblSecond].DefaultView.ToTable();
                                                            if (dtdet.Rows.Count > 0)
                                                            {
                                                                for (int dtsub = 0; dtsub < dtdet.Rows.Count; dtsub++)
                                                                {
                                                                    string hashValue = chklsfyear.Items[fin].Value + "-" + chkl_studhed.Items[hd].Value;
                                                                    double paiAmount = 0;
                                                                    int curColCnt = 0;
                                                                    int.TryParse(Convert.ToString(htColCntOther[hashValue]), out curColCnt);
                                                                    double.TryParse(Convert.ToString(dtdet.Rows[dtsub]["debit"]), out paiAmount);
                                                                    FnltotAmount += paiAmount;
                                                                    payBool = true;
                                                                    rowbool = true;
                                                                    if (!grandtotal.ContainsKey(curColCnt))
                                                                        grandtotal.Add(curColCnt, Convert.ToString(paiAmount));
                                                                    else
                                                                    {
                                                                        double amount = 0;
                                                                        double.TryParse(Convert.ToString(grandtotal[curColCnt]), out amount);
                                                                        amount += paiAmount;
                                                                        grandtotal.Remove(curColCnt);
                                                                        grandtotal.Add(curColCnt, Convert.ToString(amount));
                                                                    }
                                                                    if (!boolMem)//memtype name bind in spread
                                                                        memRowCnt = ++spreadDet.Sheets[0].RowCount;
                                                                    boolMem = true;
                                                                    if (cblbool)
                                                                        spreadDet.Sheets[0].RowCount++;
                                                                    if (paiAmount == 0)
                                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = "-";
                                                                    else
                                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(paiAmount);
                                                                    cblbool = false;
                                                                }
                                                            }

                                                        }
                                                    }
                                                    #endregion
                                                }
                                                else
                                                {
                                                    #region
                                                    for (int hd = 0; hd < chkl_studled.Items.Count; hd++)
                                                    {
                                                        if (chkl_studled.Items[hd].Selected)
                                                        {

                                                            string strVal = "finyearfk='" + chklsfyear.Items[fin].Value + "' and Memtype='" + cblmem.Items[mem].Value + "' and ledgerfk='" + chkl_studled.Items[hd].Value + "' and Transdate='" + date + "' and app_no='" + dvdt[snd]["app_no"] + "' and Transcode='" + dvdt[snd]["Transcode"] + "'";
                                                            DataTable dtdet = new DataTable();
                                                            ds.Tables[tblSecond].DefaultView.RowFilter = strVal;
                                                            dtdet = ds.Tables[tblSecond].DefaultView.ToTable();
                                                            if (dtdet.Rows.Count > 0)
                                                            {
                                                                for (int dtsub = 0; dtsub < dtdet.Rows.Count; dtsub++)
                                                                {
                                                                    string hashValue = chklsfyear.Items[fin].Value + "-" + chkl_studled.Items[hd].Value;
                                                                    double paiAmount = 0;
                                                                    int curColCnt = 0;
                                                                    int.TryParse(Convert.ToString(htColCntOther[hashValue]), out curColCnt);
                                                                    double.TryParse(Convert.ToString(dtdet.Rows[dtsub]["debit"]), out paiAmount);
                                                                    FnltotAmount += paiAmount;
                                                                    payBool = true;
                                                                    rowbool = true;
                                                                    if (!grandtotal.ContainsKey(curColCnt))
                                                                        grandtotal.Add(curColCnt, Convert.ToString(paiAmount));
                                                                    else
                                                                    {
                                                                        double amount = 0;
                                                                        double.TryParse(Convert.ToString(grandtotal[curColCnt]), out amount);
                                                                        amount += paiAmount;
                                                                        grandtotal.Remove(curColCnt);
                                                                        grandtotal.Add(curColCnt, Convert.ToString(amount));
                                                                    }
                                                                    if (!boolMem)//memtype name bind in spread
                                                                        memRowCnt = ++spreadDet.Sheets[0].RowCount;
                                                                    boolMem = true;
                                                                    if (cblbool)
                                                                        spreadDet.Sheets[0].RowCount++;
                                                                    if (paiAmount == 0)
                                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = "-";
                                                                    else
                                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(paiAmount);
                                                                    cblbool = false;
                                                                }
                                                            }

                                                        }
                                                    }
                                                    #endregion
                                                }
                                            }
                                        }
                                        if (payBool && ds.Tables[tblThird].Rows.Count > 0)
                                        {
                                            #region paymode
                                            for (int s = 0; s < chkl_paid.Items.Count; s++)
                                            {
                                                if (chkl_paid.Items[s].Selected == true)
                                                {
                                                    string payModeVal = Convert.ToString(chkl_paid.Items[s].Value);
                                                    string strVal = " Transdate='" + date + "' and paymode='" + payModeVal + "' and app_no='" + dvdt[snd]["app_no"] + "' and Transcode='" + dvdt[snd]["Transcode"] + "'";
                                                    int curColCnt = 0;
                                                    double paiAmount = 0;
                                                    int.TryParse(Convert.ToString(htPayCol[payModeVal]), out curColCnt);
                                                    DataView dvhd = new DataView();
                                                    ds.Tables[tblThird].DefaultView.RowFilter = strVal;
                                                    dvhd = ds.Tables[tblThird].DefaultView;
                                                    if (dvhd.Count > 0)
                                                    {
                                                        for (int i = 0; i < dvhd.Count; i++)
                                                        {
                                                            double temp = 0;
                                                            double.TryParse(Convert.ToString(dvhd[i]["debit"]), out temp);
                                                            paiAmount += temp;
                                                        }
                                                    }
                                                    if (!grandtotal.ContainsKey(curColCnt))
                                                        grandtotal.Add(curColCnt, Convert.ToString(paiAmount));
                                                    else
                                                    {
                                                        double amount = 0;
                                                        double.TryParse(Convert.ToString(grandtotal[curColCnt]), out amount);
                                                        amount += paiAmount;
                                                        grandtotal.Remove(curColCnt);
                                                        grandtotal.Add(curColCnt, Convert.ToString(amount));
                                                    }
                                                    if (paiAmount != 0)
                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(paiAmount);
                                                    else
                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = "-";
                                                    if (payModeVal == "1")
                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].BackColor = ColorTranslator.FromHtml("#F08080");
                                                    else if (payModeVal == "2")
                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].BackColor = ColorTranslator.FromHtml("#D3D3D3");
                                                    else if (payModeVal == "3")
                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].BackColor = ColorTranslator.FromHtml("#FFA500");
                                                    else if (payModeVal == "4")
                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].BackColor = ColorTranslator.FromHtml("#90EE90");
                                                    else if (payModeVal == "5")
                                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].BackColor = ColorTranslator.FromHtml("#FAFAD2");
                                                }
                                            }
                                            #endregion
                                        }
                                        if (rowbool)
                                        {
                                            #region student detail
                                            roll++;
                                            string degcode = string.Empty;
                                            string rollno = string.Empty;
                                            string regno = string.Empty;
                                            string admisno = string.Empty;
                                            string applno = string.Empty;
                                            string studName = string.Empty;
                                            string Mode = string.Empty;
                                            string reciptNo = string.Empty;
                                            if (memValue == 2)
                                            {
                                                degcode = Convert.ToString(dvdt[snd]["dept_name"]);
                                                rollno = Convert.ToString(dvdt[snd]["staff_code"]);
                                                regno = Convert.ToString(dvdt[snd]["staff_code"]);
                                                admisno = Convert.ToString(dvdt[snd]["staff_code"]);
                                                applno = Convert.ToString(dvdt[snd]["staff_code"]);
                                                studName = Convert.ToString(dvdt[snd]["staff_name"]);
                                                reciptNo = Convert.ToString(dvdt[snd]["transcode"]);
                                            }
                                            else if (memValue == 3)
                                            {
                                                rollno = Convert.ToString(dvdt[snd]["vendorcode"]);
                                                regno = Convert.ToString(dvdt[snd]["vendorcode"]);
                                                admisno = Convert.ToString(dvdt[snd]["vendorcode"]);
                                                applno = Convert.ToString(dvdt[snd]["vendorcode"]);
                                                studName = Convert.ToString(dvdt[snd]["vendorname"]);
                                                reciptNo = Convert.ToString(dvdt[snd]["transcode"]);

                                            }
                                            else if (memValue == 4)
                                            {
                                                rollno = Convert.ToString(dvdt[snd]["vendorcode"]);
                                                regno = Convert.ToString(dvdt[snd]["vendorcode"]);
                                                admisno = Convert.ToString(dvdt[snd]["vendorcode"]);
                                                applno = Convert.ToString(dvdt[snd]["vendorcode"]);
                                                studName = Convert.ToString(dvdt[snd]["vendorname"]);
                                                reciptNo = Convert.ToString(dvdt[snd]["transcode"]);
                                            }
                                            if (!boolMemRow)
                                            {
                                                spreadDet.Sheets[0].Cells[memRowCnt - 1, 0].Text = Convert.ToString(cblmem.Items[mem].Text);
                                                spreadDet.Sheets[0].SpanModel.Add(memRowCnt - 1, 0, 1, 4);
                                                spreadDet.Sheets[0].Rows[memRowCnt - 1].BackColor = Color.Gray;
                                                spreadDet.Sheets[0].Rows[memRowCnt - 1].HorizontalAlign = HorizontalAlign.Center;
                                                boolMemRow = true;
                                            }
                                            //int rowd = Convert.ToInt32(spreadDet.Sheets[0].RowCount - 1);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(roll);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = datetxt;
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(reciptNo);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(studName);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(rollno);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(regno);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(admisno);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(applno);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(degcode);
                                            //total column
                                            int curColCnt = 0;
                                            int.TryParse(Convert.ToString(htColCnt["Total"]), out curColCnt);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(FnltotAmount);
                                            if (!grandtotal.ContainsKey(curColCnt))
                                                grandtotal.Add(curColCnt, Convert.ToString(FnltotAmount));
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(grandtotal[curColCnt]), out amount);
                                                amount += FnltotAmount;
                                                grandtotal.Remove(curColCnt);
                                                grandtotal.Add(curColCnt, Convert.ToString(amount));
                                            }
                                            double grandvalue = 0;
                                            for (int j = 8; j < spreadDet.Sheets[0].ColumnCount; j++)
                                            {
                                                double.TryParse(Convert.ToString(grandtotal[j]), out grandvalue);
                                                //  spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalue);
                                                if (!fnlTot.ContainsKey(j))
                                                    fnlTot.Add(j, Convert.ToString(grandvalue));
                                                else
                                                {
                                                    double amount = 0;
                                                    double.TryParse(Convert.ToString(fnlTot[j]), out amount);
                                                    amount += grandvalue;
                                                    fnlTot.Remove(j);
                                                    fnlTot.Add(j, Convert.ToString(amount));
                                                }
                                            }
                                            grandtotal.Clear();
                                            height += 15;
                                            #endregion
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }

                spreadDet.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                spreadDet.Sheets[0].SetColumnMerge(7, FarPoint.Web.Spread.Model.MergePolicy.Always);
                if (fnlTot.Count > 0)
                {
                    #region other total
                    spreadDet.Sheets[0].Rows.Count++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Total";
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
                    double grandvalues = 0;
                    for (int j = 9; j < spreadDet.Sheets[0].ColumnCount; j++)
                    {
                        double.TryParse(Convert.ToString(fnlTot[j]), out grandvalues);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                        if (!lstTot.ContainsKey(j))
                            lstTot.Add(j, Convert.ToString(grandvalues));
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(lstTot[j]), out amount);
                            amount += grandvalues;
                            lstTot.Remove(j);
                            lstTot.Add(j, Convert.ToString(amount));
                        }
                    }
                    #endregion
                }

                #endregion
            }
            catch { }

            #region grandtot
            if (lstTot.Count > 0)
            {
                spreadDet.Sheets[0].Rows.Count++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
                double grandvalues = 0;
                for (int j = 9; j < spreadDet.Sheets[0].ColumnCount; j++)
                {
                    double.TryParse(Convert.ToString(lstTot[j]), out grandvalues);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                }
            }
            #endregion


            spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
            payModeLabels(htPayCol);
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            spreadDet.Visible = true;
            print.Visible = true;
            spreadDet.Height = 200 + height;
            spreadDet.SaveChanges();

            #endregion
        }
        catch
        { }
    }

    protected string studentMode(string mode)
    {
        string strMode = string.Empty;
        switch (mode)
        {
            case "1":
                strMode = "O";
                break;
            case "2":
                strMode = "T";
                break;
            case "3":
                strMode = "N";
                break;
            default:
                strMode = "";
                break;
        }
        return strMode;
    }
    protected void payModeLabels(Hashtable htpay)
    {
        lblcash.Visible = false;
        lblchq.Visible = false;
        lbldd.Visible = false;
        lblchal.Visible = false;
        lblonline.Visible = false;
        lblcard.Visible = false;
        foreach (DictionaryEntry row in htpay)
        {
            if (row.Key.ToString() == "1")
                lblcash.Visible = true;
            if (row.Key.ToString() == "2")
                lblchq.Visible = true;
            if (row.Key.ToString() == "3")
                lbldd.Visible = true;
            if (row.Key.ToString() == "4")
                lblchal.Visible = true;
            if (row.Key.ToString() == "5")
                lblonline.Visible = true;
            if (row.Key.ToString() == "6")
                lblcard.Visible = true;
        }
        divlabl.Visible = true;
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
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
        // lbl.Add(lbl_str1);
        lbl.Add(lbldeg);
        lbl.Add(lbldept);
        lbl.Add(lblsem);
        fields.Add(0);
        // fields.Add(1);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);
        if (checkSchoolSetting() == 0)
        {
            lblbatch.Text = "Year";
            lblheader.Text = "Fees";
        }

        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }

    #region roll,reg,admission setting
    private void RollAndRegSettings()
    {
        try
        {
            DataSet dsl = new DataSet();
            string Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
            dsl = d2.select_method_wo_parameter(Master1, "text");
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
            if (!cbtermwise.Checked)
            {
                if (!cbBillwise.Checked)
                {
                    #region
                    if (roll == 0)
                    {
                        spreadDet.Columns[3].Visible = true;
                        spreadDet.Columns[4].Visible = true;
                        spreadDet.Columns[5].Visible = true;
                    }
                    else if (roll == 1)
                    {
                        spreadDet.Columns[3].Visible = true;
                        spreadDet.Columns[4].Visible = true;
                        spreadDet.Columns[5].Visible = true;
                    }
                    else if (roll == 2)
                    {
                        spreadDet.Columns[3].Visible = true;
                        spreadDet.Columns[4].Visible = false;
                        spreadDet.Columns[5].Visible = false;

                    }
                    else if (roll == 3)
                    {
                        spreadDet.Columns[3].Visible = false;
                        spreadDet.Columns[4].Visible = true;
                        spreadDet.Columns[5].Visible = false;
                    }
                    else if (roll == 4)
                    {
                        spreadDet.Columns[3].Visible = false;
                        spreadDet.Columns[4].Visible = false;
                        spreadDet.Columns[5].Visible = true;
                    }
                    else if (roll == 5)
                    {
                        spreadDet.Columns[3].Visible = true;
                        spreadDet.Columns[4].Visible = true;
                        spreadDet.Columns[5].Visible = false;
                    }
                    else if (roll == 6)
                    {
                        spreadDet.Columns[3].Visible = false;
                        spreadDet.Columns[4].Visible = true;
                        spreadDet.Columns[5].Visible = true;
                    }
                    else if (roll == 7)
                    {
                        spreadDet.Columns[3].Visible = true;
                        spreadDet.Columns[4].Visible = false;
                        spreadDet.Columns[5].Visible = true;
                    }
                    #endregion
                }
                else
                {
                    #region
                    if (roll == 0)
                    {
                        spreadDet.Columns[4].Visible = true;
                        spreadDet.Columns[5].Visible = true;
                        spreadDet.Columns[6].Visible = true;
                    }
                    else if (roll == 1)
                    {
                        spreadDet.Columns[4].Visible = true;
                        spreadDet.Columns[5].Visible = true;
                        spreadDet.Columns[6].Visible = true;
                    }
                    else if (roll == 2)
                    {
                        spreadDet.Columns[4].Visible = true;
                        spreadDet.Columns[5].Visible = false;
                        spreadDet.Columns[6].Visible = false;

                    }
                    else if (roll == 3)
                    {
                        spreadDet.Columns[4].Visible = false;
                        spreadDet.Columns[5].Visible = true;
                        spreadDet.Columns[6].Visible = false;
                    }
                    else if (roll == 4)
                    {
                        spreadDet.Columns[4].Visible = false;
                        spreadDet.Columns[5].Visible = false;
                        spreadDet.Columns[6].Visible = true;
                    }
                    else if (roll == 5)
                    {
                        spreadDet.Columns[4].Visible = true;
                        spreadDet.Columns[5].Visible = true;
                        spreadDet.Columns[6].Visible = false;
                    }
                    else if (roll == 6)
                    {
                        spreadDet.Columns[4].Visible = false;
                        spreadDet.Columns[5].Visible = true;
                        spreadDet.Columns[6].Visible = true;
                    }
                    else if (roll == 7)
                    {
                        spreadDet.Columns[4].Visible = true;
                        spreadDet.Columns[5].Visible = false;
                        spreadDet.Columns[6].Visible = true;
                    }
                    #endregion
                }
            }
            else
            {
                #region
                if (roll == 0)
                {
                    spreadDet.Columns[2].Visible = true;
                    spreadDet.Columns[3].Visible = true;
                    spreadDet.Columns[4].Visible = true;
                }
                else if (roll == 1)
                {
                    spreadDet.Columns[2].Visible = true;
                    spreadDet.Columns[3].Visible = true;
                    spreadDet.Columns[4].Visible = true;
                }
                else if (roll == 2)
                {
                    spreadDet.Columns[2].Visible = true;
                    spreadDet.Columns[3].Visible = false;
                    spreadDet.Columns[4].Visible = false;

                }
                else if (roll == 3)
                {
                    spreadDet.Columns[2].Visible = false;
                    spreadDet.Columns[3].Visible = true;
                    spreadDet.Columns[4].Visible = false;
                }
                else if (roll == 4)
                {
                    spreadDet.Columns[2].Visible = false;
                    spreadDet.Columns[3].Visible = false;
                    spreadDet.Columns[4].Visible = true;
                }
                else if (roll == 5)
                {
                    spreadDet.Columns[2].Visible = true;
                    spreadDet.Columns[3].Visible = true;
                    spreadDet.Columns[4].Visible = false;
                }
                else if (roll == 6)
                {
                    spreadDet.Columns[2].Visible = false;
                    spreadDet.Columns[3].Visible = true;
                    spreadDet.Columns[4].Visible = true;
                }
                else if (roll == 7)
                {
                    spreadDet.Columns[2].Visible = true;
                    spreadDet.Columns[3].Visible = false;
                    spreadDet.Columns[4].Visible = true;
                }
                #endregion
            }
        }
        catch { }
    }

    #endregion

    #region Print
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(spreadDet, reportname);
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
            degreedetails = "Fees Paid Report";
            pagename = "DailyCollectionDetailedStatementReport.aspx";
            Printcontrolhed.loadspreaddetails(spreadDet, pagename, degreedetails);
            Printcontrolhed.Visible = true;
        }
        catch { }
    }
    #endregion

    //added by sudhagar 08.02.2017
    private double checkSchoolSetting()
    {
        double getVal = 0;
        try
        {
            double.TryParse(Convert.ToString(d2.GetFunction("select  value from Master_Settings where settings='schoolorcollege' and usercode='" + usercode + "'")), out getVal);

        }
        catch { }
        return getVal;
    }

    //added by sudhagar 20.04.2017
    protected void cbtermwise_Changed(object sender, EventArgs e)
    {
        if (cbtermwise.Checked)
        {
            rblpaidtype.Visible = true;
            rblpaidtype.SelectedIndex = 0;
            cbincludedate.Visible = true;

            //  txt_fromdate.Enabled = false;
            // txt_todate.Enabled = false;
        }
        else
        {
            rblpaidtype.Visible = false;
            cbincludedate.Visible = false;
            // txt_fromdate.Enabled = true;
            //txt_todate.Enabled = true;
        }
        lblvalidation1.Text = string.Empty;
        txtexcelname.Text = string.Empty;
        spreadDet.Visible = false;
        print.Visible = false;
        divlabl.Visible = false;
    }

    protected DataSet dsloadPaid(ref string fromdate, ref string todate)
    {
        DataSet dsload = new DataSet();
        try
        {
            #region get Value
            string batch = "";
            string degcourseid = "";
            string deptdegcode = "";
            string sem = "";
            string sec = "";
            string paid = "";
            string headervalue = "";
            string ledgervalue = "";
            string Finyearvalue = "";
            string SelQ = "";
            string strRecon = string.Empty;
            if (ddlcollegename.Items.Count > 0)
                collegecode = Convert.ToString(ddlcollegename.SelectedItem.Value);
            batch = Convert.ToString(getCblSelectedValue(cbl_batch));
            degcourseid = Convert.ToString(getCblSelectedValue(cbl_degree));
            deptdegcode = Convert.ToString(getCblSelectedValue(cbl_dept));
            sem = Convert.ToString(getCblSelectedValue(cbl_sem));
            sec = Convert.ToString(getCblSelectedValue(cbl_sect));
            paid = Convert.ToString(getCblSelectedValue(chkl_paid));
            headervalue = Convert.ToString(getCblSelectedValue(chkl_studhed));
            ledgervalue = Convert.ToString(getCblSelectedValue(chkl_studled));
            Finyearvalue = Convert.ToString(getCblSelectedValue(chklsfyear));
            string studMode = Convert.ToString(getCblSelectedValue(cbl_type));
            string finUser = Convert.ToString(getCblSelectedValue(cbluser));
            fromdate = txt_fromdate.Text;
            todate = txt_todate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
            // string strtype = string.Empty;
            string strReg = " ";
            //if (rbtype.SelectedIndex == 0)
            //    strtype = ",headerfk";
            //else
            //    strtype = ",ledgerfk";
            string GPpayType = string.Empty;
            string payType = string.Empty;
            if (rblpaidtype.SelectedIndex == 0)
                GPpayType = " having sum(totalamount)=sum(paidamount) and sum(balamount)='0'";
            else
                GPpayType = " having sum(totalamount)<>sum(paidamount) and sum(balamount)<>'0'";
            if (cbbfrecon.Checked)
                strRecon = " and ISNULL(IsCanceled,'0')<>'1'";
            else
                strRecon = " and ( f.IsDeposited='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1'";
            string applynStr = " AND r.IsConfirm = 1  and isnull(is_enroll,'0')<>'2'";//AND Admission_Status = 1
            //for (int se = 0; se < cbl_sem.Items.Count; se++)
            //{
            //    if (cbl_sem.Items[se].Selected)
            //    {
            //        if (sem == string.Empty)
            //            sem = " and( t.textval like'%" + cbl_sem.Items[se].Text + "%'";
            //        else
            //            sem += " or t.textval like'%" + cbl_sem.Items[se].Text + "%'";
            //    }
            //}
            //if (!string.IsNullOrEmpty(sem))
            //    sem += ")";
            #region includem

            string cc = "";
            string debar = "";
            string disc = "";
            string cancel = "";
            string strInclude = string.Empty;
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
                        if (cblinclude.Items[i].Value == "4")
                            cancel = "  r.DelFlag=2";
                    }
                }
            }
            if (!checkdicon.Checked)
            {
                if (cc != "" && debar == "" && disc == "" && cancel == "")
                    strInclude = " and (" + cc + " or r.cc=0)  and r.Exam_Flag<>'debar' and r.DelFlag=0";
                if (cc == "" && debar != "" && disc == "" && cancel == "")
                    strInclude = " and r.cc=0  and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";
                if (cc == "" && debar == "" && disc != "" && cancel == "")
                    strInclude = " and r.cc=0  and r.Exam_Flag<>'debar' and (" + disc + " or r.DelFlag=0)";
                if (cc == "" && debar == "" && disc == "" && cancel != "")
                    strInclude = " and r.cc=0  and r.Exam_Flag<>'debar' and (" + cancel + " or r.DelFlag=0)";
                //2
                if (cc != "" && debar != "" && disc == "" && cancel == "")
                    strInclude = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";
                if (cc != "" && debar == "" && disc != "" && cancel == "")
                    strInclude = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar' and (" + disc + " or r.DelFlag=0)";
                if (cc != "" && debar == "" && disc == "" && cancel != "")
                    strInclude = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar' and (" + cancel + " or r.DelFlag=0)";
                //
                if (cc == "" && debar != "" && disc != "" && cancel == "")
                    strInclude = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar') and (" + disc + " or r.DelFlag=0)";
                if (cc == "" && debar != "" && disc == "" && cancel != "")
                    strInclude = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar') and (" + cancel + " or r.DelFlag=0)";
                //
                if (cc == "" && debar == "" && disc != "" && cancel != "")
                    strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and (" + disc + " or " + cancel + " or r.DelFlag=0)";
                //3
                if (cc != "" && debar != "" && disc != "" && cancel == "")
                    strInclude = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and (" + disc + " or r.DelFlag=0)";
                if (cc != "" && debar == "" && disc != "" && cancel != "")
                    strInclude = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar' and (" + disc + " or " + cancel + " or r.DelFlag=0)";
                if (cc != "" && debar != "" && disc == "" && cancel != "")
                    strInclude = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and (" + cancel + " or r.DelFlag=0)";
                if (cc == "" && debar != "" && disc != "" && cancel != "")
                    strInclude = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar') and (" + disc + " or " + cancel + " or r.DelFlag=0)";
                if (cc == "" && debar == "" && disc == "" && cancel == "")
                    strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and  r.DelFlag=0";
                if (cc != "" && debar != "" && disc != "" && cancel != "")
                    strInclude = "";
            }
            else
            {
                if (cc != "" && debar == "" && disc == "" && cancel == "")
                    strInclude = " and " + cc + "";
                if (cc == "" && debar != "" && disc == "" && cancel == "")
                    strInclude = " and " + debar + "";
                if (cc == "" && debar == "" && disc != "" && cancel == "")
                    strInclude = " and " + disc + "";
                if (cc == "" && debar == "" && disc == "" && cancel != "")
                    strInclude = " and " + cancel + "";
                //2
                if (cc != "" && debar != "" && disc == "" && cancel == "")
                    strInclude = " and " + cc + " and " + debar + "";
                if (cc != "" && debar == "" && disc != "" && cancel == "")
                    strInclude = " and " + cc + " and " + disc + "";
                if (cc != "" && debar == "" && disc == "" && cancel != "")
                    strInclude = " and " + cc + " and " + cancel + "";
                //
                if (cc == "" && debar != "" && disc != "" && cancel == "")
                    strInclude = " and " + debar + " and " + disc + "";
                if (cc == "" && debar != "" && disc == "" && cancel != "")
                    strInclude = " and " + debar + " and " + cancel + "";
                //
                if (cc == "" && debar == "" && disc != "" && cancel != "")
                    strInclude = " and (" + disc + " or " + cancel + ")";
                //3
                if (cc != "" && debar != "" && disc != "" && cancel == "")
                    strInclude = " and " + cc + " and " + debar + " and " + disc + "";
                if (cc != "" && debar == "" && disc != "" && cancel != "")
                    strInclude = " and " + cc + " and (" + disc + " or " + cancel + ")";
                if (cc != "" && debar != "" && disc == "" && cancel != "")
                    strInclude = " and " + cc + " and " + debar + " and " + cancel + "";
                if (cc == "" && debar != "" && disc != "" && cancel != "")
                    strInclude = " and " + debar + " and (" + disc + " or " + cancel + ")";
                if (cc == "" && debar == "" && disc == "" && cancel == "")
                    strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and  r.DelFlag=0";
                if (cc != "" && debar != "" && disc != "" && cancel != "")
                    strInclude = " and " + cc + " and " + debar + " and (" + disc + " or " + cancel + ")";
            }

            #endregion
            #endregion

            #region Query New


            SelQ += " select distinct  app_no,sum(debit) as debit,Textval,batch_year,degree_code,roll_no,reg_no,roll_admit,stud_name,mode from(";
            SelQ += " select  f.app_no,debit,Textval,r.batch_year,r.degree_code,r.roll_no,r.reg_no,r.roll_admit,r.stud_name ,mode from ft_findailytransaction f,registration r,textvaltable t where f.app_no=r.app_no and f.feecategory=t.textcode and r.college_code=t.college_code and t.textcriteria='FEECA' " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + strReg + " and isnull(debit,'0')>0 and isnull(actualfinyearfk,'0')<>'0' and r.mode in('" + studMode + "') and f.entryusercode in('" + finUser + "')  " + strInclude + "";
            if (cbincludedate.Checked)
                SelQ += "   and f.Transdate between '" + fromdate + "' and '" + todate + "'";
            if (cbbeforeadm.Checked)
            {
                SelQ += " union all select  f.app_no,debit,Textval,r.batch_year,r.degree_code,''roll_no,''reg_no,''roll_admit,''stud_name,mode from ft_findailytransaction f,applyn r,textvaltable t where f.app_no=r.app_no and f.feecategory=t.textcode and r.college_code=t.college_code and t.textcriteria='FEECA' " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "')  and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "'" + applynStr + " and isnull(debit,'0')>0 and isnull(actualfinyearfk,'0')<>'0' and r.mode in('" + studMode + "') and f.entryusercode in('" + finUser + "')";
                if (cbincludedate.Checked)
                    SelQ += "   and f.Transdate between '" + fromdate + "' and '" + todate + "'";
            }
            SelQ += ") tbl group by Textval,app_no,batch_year,degree_code,roll_no,reg_no,roll_admit,stud_name,mode order by batch_year,degree_code,Textval asc";
            //and f.Transdate between '" + fromdate + "' and '" + todate + "'      
            //finyear fk
            SelQ += " select distinct  app_no,sum(debit) as debit,Textval,batch_year,degree_code,roll_no,reg_no,roll_admit,stud_name,mode,actualfinyearfk from(";
            SelQ += " select  f.app_no,debit,Textval,r.batch_year,r.degree_code,r.roll_no,r.reg_no,r.roll_admit,r.stud_name ,mode,actualfinyearfk from ft_findailytransaction f,registration r,textvaltable t where f.app_no=r.app_no and f.feecategory=t.textcode and r.college_code=t.college_code and t.textcriteria='FEECA' " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + strReg + " and isnull(debit,'0')>0 and isnull(actualfinyearfk,'0')<>'0' and r.mode in('" + studMode + "') and f.entryusercode in('" + finUser + "')  " + strInclude + "";
            if (cbincludedate.Checked)
                SelQ += "   and f.Transdate between '" + fromdate + "' and '" + todate + "'";
            if (cbbeforeadm.Checked)
            {
                SelQ += " union all select  f.app_no,debit,Textval,r.batch_year,r.degree_code,''roll_no,''reg_no,''roll_admit,''stud_name,mode,actualfinyearfk from ft_findailytransaction f,applyn r,textvaltable t where f.app_no=r.app_no and f.feecategory=t.textcode and r.college_code=t.college_code and t.textcriteria='FEECA' " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "')  and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "'" + applynStr + " and isnull(debit,'0')>0 and isnull(actualfinyearfk,'0')<>'0' and r.mode in('" + studMode + "') and f.entryusercode in('" + finUser + "')";
                if (cbincludedate.Checked)
                    SelQ += "   and f.Transdate between '" + fromdate + "' and '" + todate + "'";
            }
            SelQ += ") tbl group by Textval,app_no,batch_year,degree_code,roll_no,reg_no,roll_admit,stud_name,mode,actualfinyearfk order by batch_year,degree_code,Textval asc";

            SelQ += " select d.Degree_Code,(dt.Dept_Name) as degreename,(c.Course_Name +'-'+dt.dept_acronym)as dept_acronym from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode + "'";
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(SelQ, "Text");
            #endregion
        }
        catch { }
        return dsload;
    }

    protected void loadPaidDetails(DataSet ds, ref string fromdate, ref string todate)
    {
        try
        {
            #region design
            RollAndRegSettings();
            spreadDet.Sheets[0].RowCount = 0;
            spreadDet.Sheets[0].ColumnCount = 0;
            spreadDet.CommandBar.Visible = false;
            spreadDet.Sheets[0].AutoPostBack = true;
            spreadDet.Sheets[0].ColumnHeader.RowCount = 2;
            spreadDet.Sheets[0].RowHeader.Visible = false;
            spreadDet.Sheets[0].ColumnCount = 6;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            spreadDet.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Student Name";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            spreadDet.Sheets[0].Columns[2].Width = 250;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg No";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Admission No";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].Text = lbldept.Text;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;

            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);

            #region term
            Hashtable htColCnt = new Hashtable();
            Hashtable htActYr = getFinyear();
            for (int row = 0; row < chklsfyear.Items.Count; row++)
            {
                int fnlcol = 0;
                int totcolCnt = 0;
                if (chklsfyear.Items[row].Selected)
                {
                    bool semBool = false;
                    string finyearFk = Convert.ToString(chklsfyear.Items[row].Value);
                    fnlcol = spreadDet.Sheets[0].ColumnCount++;
                    string actualYEar = Convert.ToString(htActYr[finyearFk]);
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Text = actualYEar;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Tag = finyearFk;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                    bool sembol = false;
                    for (int sem = 0; sem < cbl_sem.Items.Count; sem++)
                    {
                        if (cbl_sem.Items[sem].Selected)
                        {
                            sembol = true;
                            if (semBool)
                                spreadDet.Sheets[0].ColumnCount++;
                            semBool = true;
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(cbl_sem.Items[sem].Text);
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(cbl_sem.Items[sem].Value);

                            htColCnt.Add(finyearFk + "-" + cbl_sem.Items[sem].Value, spreadDet.Sheets[0].ColumnCount - 1);
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                            totcolCnt++;
                        }
                    }
                    if (sembol)
                    {
                        spreadDet.Sheets[0].ColumnCount++;
                        htColCnt.Add(finyearFk + "-" + "Total", spreadDet.Sheets[0].ColumnCount - 1);
                        spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Text = "Total";
                        spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                        spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                        totcolCnt++;
                    }

                }
                //
                spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, fnlcol, 1, totcolCnt);

            }
            #endregion
            spreadColumnVisible();
            #endregion

            #region value
            Hashtable grandtotal = new Hashtable();
            bool rowVal = false;
            int height = 0;
            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
            {

                bool boolRow = false;
                // double totalAmt = 0;
                // double.TryParse(Convert.ToString(ds.Tables[0].Rows[row]["totalamount"]), out totalAmt);
                for (int fnkrow = 0; fnkrow < chklsfyear.Items.Count; fnkrow++)
                {
                    int fnlcol = 0;
                    int totcolCnt = 0;
                    if (chklsfyear.Items[fnkrow].Selected)
                    {
                        bool yearbool = false;
                        double fnlToAmt = 0;
                        string fpfnlyearFk = Convert.ToString(chklsfyear.Items[fnkrow].Value);
                        for (int semv = 0; semv < cbl_sem.Items.Count; semv++)
                        {
                            #region
                            if (cbl_sem.Items[semv].Selected)
                            {
                                double paidAmt = 0;
                                int curColCnt = 0;
                                int.TryParse(Convert.ToString(htColCnt[fpfnlyearFk + "-" + cbl_sem.Items[semv].Value]), out curColCnt);
                                string strVal = "actualfinyearfk='" + fpfnlyearFk + "' and textval like'%" + cbl_sem.Items[semv].Text + "%'  and app_no='" + ds.Tables[1].Rows[row]["app_no"] + "' and Degree_code='" + ds.Tables[1].Rows[row]["degree_code"] + "'";
                                DataView Dview = new DataView();
                                if (ds.Tables[1].Rows.Count > 0)
                                {
                                    ds.Tables[1].DefaultView.RowFilter = strVal;
                                    Dview = ds.Tables[1].DefaultView;
                                    if (Dview.Count > 0)
                                    {
                                        DataTable dt = new DataTable();
                                        dt = Dview.ToTable();
                                        double val = 0;
                                        //= Convert.ToInt32(dt.Compute("Sum(debit)", ""));
                                        double.TryParse(Convert.ToString(dt.Compute("Sum(debit)", "")), out paidAmt);
                                        // double.TryParse(Convert.ToString(Dview[0]["debit"]), out paidAmt);
                                        yearbool = true;
                                    }
                                    if (!boolRow)
                                    {
                                        spreadDet.Sheets[0].RowCount++;
                                        boolRow = true;
                                    }
                                }
                                if (boolRow)
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(paidAmt);
                                if (!grandtotal.ContainsKey(curColCnt))
                                    grandtotal.Add(curColCnt, Convert.ToString(paidAmt));
                                else
                                {
                                    double amount = 0;
                                    double.TryParse(Convert.ToString(grandtotal[curColCnt]), out amount);
                                    amount += paidAmt;
                                    grandtotal.Remove(curColCnt);
                                    grandtotal.Add(curColCnt, Convert.ToString(amount));
                                }
                                fnlToAmt += paidAmt;
                                rowVal = true;
                            }
                            #endregion
                        }
                        if (yearbool)
                        {
                            //total column
                            int curColCnt = 0;
                            int.TryParse(Convert.ToString(htColCnt[fpfnlyearFk + "-" + "Total"]), out curColCnt);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(fnlToAmt);
                            if (!grandtotal.ContainsKey(curColCnt))
                                grandtotal.Add(curColCnt, Convert.ToString(fnlToAmt));
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(grandtotal[curColCnt]), out amount);
                                amount += fnlToAmt;
                                grandtotal.Remove(curColCnt);
                                grandtotal.Add(curColCnt, Convert.ToString(amount));
                            }
                        }
                    }
                }
                if (rowVal)
                {
                    #region
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                    string Mode = Convert.ToString(ds.Tables[0].Rows[row]["mode"]);
                    Mode = studentMode(Mode);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["stud_name"]) + "-(" + Mode + ")";
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["roll_no"]);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["reg_no"]);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["roll_admit"]);
                    string batch = Convert.ToString(ds.Tables[0].Rows[row]["batch_year"]);
                    DataView dvDept = new DataView();
                    string Degreename = string.Empty;
                    string Acrname = string.Empty;
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        ds.Tables[2].DefaultView.RowFilter = "Degree_code='" + ds.Tables[0].Rows[row]["degree_code"] + "'";
                        dvDept = ds.Tables[2].DefaultView;
                        if (dvDept.Count > 0)
                        {
                            Degreename = Convert.ToString(dvDept[0]["degreename"]);
                            Acrname = Convert.ToString(dvDept[0]["dept_acronym"]);
                        }
                    }
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(batch + "-" + Acrname);
                    ////total column
                    //int curColCnt = 0;
                    //int.TryParse(Convert.ToString(htColCnt["Total"]), out curColCnt);
                    //spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(fnlToAmt);
                    //if (!grandtotal.ContainsKey(curColCnt))
                    //    grandtotal.Add(curColCnt, Convert.ToString(fnlToAmt));
                    //else
                    //{
                    //    double amount = 0;
                    //    double.TryParse(Convert.ToString(grandtotal[curColCnt]), out amount);
                    //    amount += fnlToAmt;
                    //    grandtotal.Remove(curColCnt);
                    //    grandtotal.Add(curColCnt, Convert.ToString(amount));
                    //}
                    height += 10;
                    #endregion
                }
            }
            #region grandtot
            //   spreadDet.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
            spreadDet.Sheets[0].Rows.Count++;
            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
            spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
            double grandvalues = 0;
            for (int j = 6; j < spreadDet.Sheets[0].ColumnCount; j++)
            {
                double.TryParse(Convert.ToString(grandtotal[j]), out grandvalues);
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
            }
            #endregion

            spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
            divlabl.Visible = false;
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            spreadDet.Visible = true;
            print.Visible = true;
            //   spreadDet.Height = 200 + height;
            spreadDet.SaveChanges();

            #endregion
        }
        catch { }
    }

    //changed by sudhagar 06.05.2017   

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
            ds = d2.select_method_wo_parameter(deptquery, "Text");
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
                cbl_type.Items.Add(new ListItem("Old ", "1"));
                cbl_type.Items.Add(new ListItem("New ", "3"));
                // cbl_type.Items.Add(new ListItem("Transfer", "2"));
            }
            else
            {
                cbl_type.Items.Add(new ListItem("Regular", "1"));
                cbl_type.Items.Add(new ListItem("Lateral", "3"));
                cbl_type.Items.Add(new ListItem("Transfer", "2"));
                cbl_type.Items.Add(new ListItem("IrRegular", "4"));
            }
            if (cbl_type.Items.Count > 0)
            {
                for (int i = 0; i < cbl_type.Items.Count; i++)
                {
                    cbl_type.Items[i].Selected = true;
                }
                cb_type.Checked = true;
                txt_type.Text = "Type(" + cbl_type.Items.Count + ")";
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

    protected Hashtable getFinyear()
    {
        Hashtable htfin = new Hashtable();
        try
        {
            string SelQ = "  select (convert(varchar(10),datepart(year,finyearstart))+'-'+convert(varchar(10),datepart(year,finyearend)))as finyear,finyearpk,collegecode from fm_finyearmaster where collegecode='" + collegecode + "'";
            DataSet dsval = d2.select_method_wo_parameter(SelQ, "Text");
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
    protected Hashtable getHeaderFK()
    {
        Hashtable hthdName = new Hashtable();
        try
        {
            string selQFK = string.Empty;
            if (rbtype.SelectedIndex == 0)
                selQFK = "  select distinct headerpk as pk,headername as name from fm_headermaster where collegecode in('" + collegecode + "') ";
            else
                selQFK = "   select distinct ledgername as name,ledgerpk as pk from fm_headermaster h,fm_ledgermaster l where h.headerpk=l.headerfk and h.collegecode in('" + collegecode + "') ";
            DataSet dsval = d2.select_method_wo_parameter(selQFK, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsval.Tables[0].Rows.Count; row++)
                {
                    if (!hthdName.ContainsKey(Convert.ToString(dsval.Tables[0].Rows[row]["pk"])))
                        hthdName.Add(Convert.ToString(dsval.Tables[0].Rows[row]["pk"]), Convert.ToString(dsval.Tables[0].Rows[row]["name"]));
                }
            }
        }
        catch { hthdName.Clear(); }
        return hthdName;
    }

    protected Hashtable getDeptName()
    {
        Hashtable htdtName = new Hashtable();
        try
        {
            string SelQ = " select distinct d.degree_code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename,(c.Course_Name +'-'+dt.dept_acronym)as dept_acronym from degree d,department dt,course c where c.course_id=d.course_id and d.dept_code=dt.dept_code and d.college_code in('" + collegecode + "')";
            DataSet dsdeg = d2.select_method_wo_parameter(SelQ, "Text");
            if (dsdeg.Tables.Count > 0 && dsdeg.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsdeg.Tables[0].Rows.Count; row++)
                {
                    if (!htdtName.ContainsKey(Convert.ToString(dsdeg.Tables[0].Rows[row]["degree_code"])))
                        htdtName.Add(Convert.ToString(dsdeg.Tables[0].Rows[row]["degree_code"]), Convert.ToString(dsdeg.Tables[0].Rows[row]["degreename"]));
                }
            }
        }
        catch { }
        return htdtName;
    }

    //added by sudhagar 01.06.2017
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
        CallCheckboxChange(cbinclude, cblinclude, txtinclude, "Include Setting", "--Select--");
    }
    protected void cblinclude_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbinclude, cblinclude, txtinclude, "Include Setting", "--Select--");

    }


    #endregion

    //discontinue,delflag
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
                        if (cblinclude.Items[i].Value == "4")
                            cancel = "  r.DelFlag=2";
                    }
                }
            }
            if (!checkdicon.Checked)
            {
                if (cc != "" && debar == "" && disc == "" && cancel == "")
                    strInclude = " and (" + cc + " or r.cc=0)  and r.Exam_Flag<>'debar' and r.DelFlag=0";
                if (cc == "" && debar != "" && disc == "" && cancel == "")
                    strInclude = " and r.cc=0  and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";
                if (cc == "" && debar == "" && disc != "" && cancel == "")
                    strInclude = " and r.cc=0  and r.Exam_Flag<>'debar' and (" + disc + " or r.DelFlag=0)";
                if (cc == "" && debar == "" && disc == "" && cancel != "")
                    strInclude = " and r.cc=0  and r.Exam_Flag<>'debar' and (" + cancel + " or r.DelFlag=0)";
                //2
                if (cc != "" && debar != "" && disc == "" && cancel == "")
                    strInclude = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";
                if (cc != "" && debar == "" && disc != "" && cancel == "")
                    strInclude = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar' and (" + disc + " or r.DelFlag=0)";
                if (cc != "" && debar == "" && disc == "" && cancel != "")
                    strInclude = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar' and (" + cancel + " or r.DelFlag=0)";
                //
                if (cc == "" && debar != "" && disc != "" && cancel == "")
                    strInclude = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar') and (" + disc + " or r.DelFlag=0)";
                if (cc == "" && debar != "" && disc == "" && cancel != "")
                    strInclude = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar') and (" + cancel + " or r.DelFlag=0)";
                //
                if (cc == "" && debar == "" && disc != "" && cancel != "")
                    strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and (" + disc + " or " + cancel + " or r.DelFlag=0)";
                //3
                if (cc != "" && debar != "" && disc != "" && cancel == "")
                    strInclude = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and (" + disc + " or r.DelFlag=0)";
                if (cc != "" && debar == "" && disc != "" && cancel != "")
                    strInclude = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar' and (" + disc + " or " + cancel + " or r.DelFlag=0)";
                if (cc != "" && debar != "" && disc == "" && cancel != "")
                    strInclude = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and (" + cancel + " or r.DelFlag=0)";
                if (cc == "" && debar != "" && disc != "" && cancel != "")
                    strInclude = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar') and (" + disc + " or " + cancel + " or r.DelFlag=0)";
                if (cc == "" && debar == "" && disc == "" && cancel == "")
                    strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and  r.DelFlag=0";
                if (cc != "" && debar != "" && disc != "" && cancel != "")
                    strInclude = "";
            }
            else
            {
                if (cc != "" && debar == "" && disc == "" && cancel == "")
                    strInclude = " and " + cc + "";
                if (cc == "" && debar != "" && disc == "" && cancel == "")
                    strInclude = " and " + debar + "";
                if (cc == "" && debar == "" && disc != "" && cancel == "")
                    strInclude = " and " + disc + "";
                if (cc == "" && debar == "" && disc == "" && cancel != "")
                    strInclude = " and " + cancel + "";
                //2
                if (cc != "" && debar != "" && disc == "" && cancel == "")
                    strInclude = " and( " + cc + " or " + debar + ")";
                if (cc != "" && debar == "" && disc != "" && cancel == "")
                    strInclude = " and (" + cc + " or " + disc + ")";
                if (cc != "" && debar == "" && disc == "" && cancel != "")
                    strInclude = " and (" + cc + " or " + cancel + ")";
                //
                if (cc == "" && debar != "" && disc != "" && cancel == "")
                    strInclude = " and (" + debar + " or " + disc + ")";
                if (cc == "" && debar != "" && disc == "" && cancel != "")
                    strInclude = " and (" + debar + " or " + cancel + ")";
                //
                if (cc == "" && debar == "" && disc != "" && cancel != "")
                    strInclude = " and (" + disc + " or " + cancel + ")";
                //3
                if (cc != "" && debar != "" && disc != "" && cancel == "")
                    strInclude = " and (" + cc + " or " + debar + " or " + disc + ")";
                if (cc != "" && debar == "" && disc != "" && cancel != "")
                    strInclude = " and (" + cc + " or " + disc + " or " + cancel + ")";
                if (cc != "" && debar != "" && disc == "" && cancel != "")
                    strInclude = " and (" + cc + " or " + debar + " or " + cancel + ")";
                if (cc == "" && debar != "" && disc != "" && cancel != "")
                    strInclude = " and (" + debar + " or " + disc + " or " + cancel + ")";
                if (cc == "" && debar == "" && disc == "" && cancel == "")
                    strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and  r.DelFlag=0";
                if (cc != "" && debar != "" && disc != "" && cancel != "")
                    strInclude = " and (" + cc + " or " + debar + " or " + disc + " or " + cancel + ")";
            }

            #endregion
        }
        catch { }
        return strInclude;
    }
    protected void UserbasedRights()
    {
        string userrht = d2.GetFunction("select value from Master_Settings where settings='Finance Include User Based Report Settings'  and usercode='" + usercode + "'");
        if (userrht == "1")
            usBasedRights = true;
        else
            usBasedRights = false;

    }

    #region memtype
    private void memtype()
    {
        try
        {
            cblmem.Items.Clear();
            //cblmem.Items.Add(new ListItem("Student", "1"));
            cblmem.Items.Add(new ListItem("Staff", "2"));
            cblmem.Items.Add(new ListItem("Vendor", "3"));
            cblmem.Items.Add(new ListItem("Others", "4"));
            if (cblmem.Items.Count > 0)
            {
                for (int i = 0; i < cblmem.Items.Count; i++)
                {
                    cblmem.Items[i].Selected = true;
                }
                cbmem.Checked = true;
                txtmem.Text = "MemType(" + cblmem.Items.Count + ")";
            }
        }
        catch { }
    }
    protected void cbmem_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cbmem, cblmem, txtmem, "MemType", "--Select--");
    }
    protected void cblmem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbmem, cblmem, txtmem, "MemType", "--Select--");
    }
    #endregion
}

