using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Configuration;

public partial class FinanceDailyCollectionStatementReport : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    ReuasableMethods reuse = new ReuasableMethods();
    DataSet ds = new DataSet();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
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
                LoadIncludeSetting();
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
        CallCheckboxChange(cb_sem, cbl_sem, txt_sem, lbldept.Text, "--Select--");
        bindsec();

    }
    protected void cbl_sem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_sem, cbl_sem, txt_sem, "Semester", "--Select--");
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
        }
        else
        {
            spreadDet.Visible = false;
            print.Visible = false;
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
        }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        string fromdate = string.Empty;
        string todate = string.Empty;
        ds.Clear();
        ds = dsloadDetails(ref fromdate, ref todate);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ds = dsFilterValues(ds, ref  fromdate, ref  todate);
            // spreadLoadDetails(ds, ref fromdate, ref todate);
            spreadLoadDetailss(ds, ref fromdate, ref todate);
        }
        else
        {
            lblvalidation1.Text = string.Empty;
            txtexcelname.Text = string.Empty;
            spreadDet.Visible = false;
            print.Visible = false;
            lbl_alert.Text = "No Record Found";
            imgdiv2.Visible = true;
        }
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
        DataTable dvfourt = ds.Tables[3].DefaultView.ToTable();

        string hdStr = string.Empty;
        if (rbtype.SelectedIndex == 0)
            hdStr = "headerfk";
        else
            hdStr = "ledgerfk";
        dsflt.Tables.Add(dvfst.ToTable());
        dsflt.Tables.Add(dvsnd.ToTable());
        dsflt.Tables.Add(dvthrd.ToTable());
        dsflt.Tables.Add(dvfourt.DefaultView.ToTable(true, "feecategory", hdStr, "actualfinyearfk", "finyearfk"));
        // dsflt.Tables.Add(ds.Tables[4]);

        return dsflt;
    }

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
            fromdate = txt_fromdate.Text;
            todate = txt_todate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
            string strtype = string.Empty;
            string strReg = string.Empty;
                //" and r.cc=0  and r.Exam_Flag<>'debar' and r.DelFlag=0";
            if (rbtype.SelectedIndex == 0)
                strtype = ",headerfk";
            else
                strtype = ",ledgerfk";
            if (cbbfrecon.Checked)
                strRecon = " and ISNULL(IsCanceled,'0')<>'1'";
            else
                strRecon = " and ( f.IsDeposited='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1'";
            string applynStr = " AND r.IsConfirm = 1  and isnull(is_enroll,'0')<>'2'";//AND Admission_Status = 1
            strReg = getStudCategory();
            #endregion       

            #region Query New
            //tbl 0
            SelQ = " select Transdate,sum(debit) as debit,convert(varchar(10),Transdate,103) as date from(";
            SelQ += " select Transdate,debit,convert(varchar(10),Transdate,103) as date from ft_findailytransaction f,registration r where f.app_no=r.app_no " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + strReg + " and isnull(debit,'0')>0 and isnull(actualfinyearfk,'0')<>'0'";
            if (cbbeforeadm.Checked)
            {
                SelQ += " union all select Transdate,debit,convert(varchar(10),Transdate,103) as date from ft_findailytransaction f,applyn r where f.app_no=r.app_no " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "')  and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + applynStr + " and isnull(debit,'0')>0 and isnull(actualfinyearfk,'0')<>'0'";
            }
            SelQ += " ) tbl group by Transdate order by cast(Transdate as datetime) asc";
            //  SelQ += " order by cast(Transdate as datetime) asc";
            //convert(varchar(10),Transdate,103) as Transdate
            //and f.Transdate between '" + fromdate + "' and '" + todate + "'
            //tbl 1
            SelQ += " select distinct Transdate,convert(varchar(10),Transdate,103) as date,sum(debit) as debit " + strtype + ",feecategory,actualfinyearfk,finyearfk from (";
            SelQ += " select  Transdate,convert(varchar(10),Transdate,103) as date,debit" + strtype + ",f.feecategory,actualfinyearfk,finyearfk from ft_findailytransaction f,registration r where f.app_no=r.app_no " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + strReg + "  and isnull(debit,'0')>0 and isnull(actualfinyearfk,'0')<>'0'";
            if (cbbeforeadm.Checked)
            {
                SelQ += " union all select  Transdate,convert(varchar(10),Transdate,103) as date,debit" + strtype + ",f.feecategory,actualfinyearfk,finyearfk from ft_findailytransaction f,applyn r where f.app_no=r.app_no " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "')  and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + applynStr + "  and isnull(debit,'0')>0 and isnull(actualfinyearfk,'0')<>'0'";
            }
            SelQ += ") tbl group by Transdate" + strtype + ",feecategory,actualfinyearfk,finyearfk  order by Transdate asc";
            //and f.Transdate between '" + fromdate + "' and '" + todate + "'
            // paymode
            //tbl 2
            SelQ += " select distinct Transdate,convert(varchar(10),Transdate,103) as date,sum(debit) as debit,paymode from (";
            SelQ += " select  Transdate,convert(varchar(10),Transdate,103) as date, debit,f.paymode from ft_findailytransaction f,registration r where f.app_no=r.app_no " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + strReg + "  and isnull(debit,'0')>0 and isnull(actualfinyearfk,'0')<>'0' ";
            if (cbbeforeadm.Checked)
            {
                SelQ += " union all select  Transdate,convert(varchar(10),Transdate,103) as date, debit,f.paymode from ft_findailytransaction f,applyn r where f.app_no=r.app_no " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "')  and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + applynStr + "   and isnull(debit,'0')>0 and isnull(actualfinyearfk,'0')<>'0'";
            }
            SelQ += " ) tbl group by Transdate,paymode order by Transdate asc";

            //spread bind onlyn distinct header name get
            //tbl 3
            SelQ += " select distinct Transdate,feecategory" + strtype + ",actualfinyearfk,finyearfk from (";
            SelQ += " select  Transdate,f.feecategory" + strtype + ",actualfinyearfk,finyearfk from ft_findailytransaction f,registration r where f.app_no=r.app_no " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + strReg + "  and isnull(debit,'0')>0 and isnull(actualfinyearfk,'0')<>'0'";
            if (cbbeforeadm.Checked)
            {
                SelQ += " union all select Transdate,f.feecategory" + strtype + ",actualfinyearfk,finyearfk from ft_findailytransaction f,applyn r where f.app_no=r.app_no " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "')  and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + applynStr + "  and isnull(debit,'0')>0 and isnull(actualfinyearfk,'0')<>'0' ";
            }
            SelQ += ") tbl ";
            //group by Transdate" + strtype + ",feecategory,batch_year  order by Transdate asc

            //tbl 4 financial year
            // SelQ += "  select (convert(varchar(10),datepart(year,finyearstart))+'-'+convert(varchar(10),datepart(year,finyearend)))as finyear,finyearpk,collegecode from fm_finyearmaster where collegecode='" + collegecode + "'";
            //and f.Transdate between '" + fromdate + "' and '" + todate + "'
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(SelQ, "Text");
            #endregion
        }
        catch { dsload.Clear(); }
        return dsload;
    }
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
    protected void spreadLoadDetailss(DataSet ds, ref string fromdate, ref string todate)
    {
        try
        {
            #region design
            spreadDet.Sheets[0].RowCount = 0;
            spreadDet.Sheets[0].ColumnCount = 0;
            spreadDet.CommandBar.Visible = false;
            spreadDet.Sheets[0].AutoPostBack = true;
            spreadDet.Sheets[0].ColumnHeader.RowCount = 3;
            spreadDet.Sheets[0].RowHeader.Visible = false;
            spreadDet.Sheets[0].ColumnCount = 2;
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
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 3, 1);

            #region financial year

            Dictionary<string, string> dtMode = getstudMode();
            Hashtable htHDName = getHeaderFK();
            Hashtable htActYr = getFinyear();
            Hashtable htColCnt = new Hashtable();
            for (int row = 0; row < chklsfyear.Items.Count; row++)
            {
                if (chklsfyear.Items[row].Selected)
                {
                    bool checkbatch = false;
                    int batchCol = 0;
                    bool batchbol = false;
                    string hdrTxtValue = string.Empty;
                    int col = spreadDet.Sheets[0].ColumnCount++;
                    hdrTxtValue = Convert.ToString(chklsfyear.Items[row].Value);
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
                            ds.Tables[3].DefaultView.RowFilter = " finyearfk ='" + chklsfyear.Items[row].Value + "' and feecategory='" + cbl_sem.Items[sem].Value + "'";//mode='" + rMode.Value + "' and
                            DataTable dthd = ds.Tables[3].DefaultView.ToTable();
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
                                    htColCnt.Add(value, spreadDet.Sheets[0].ColumnCount - 1);
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
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Text = Convert.ToString(htActYr[Convert.ToString(chklsfyear.Items[row].Value).Trim()]); //Convert.ToString(chklsfyear.Items[row].Text);
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



            #endregion

            #region  value
            Hashtable grandtotal = new Hashtable();
            int height = 0;
            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
            {
                double FnltotAmount = 0;
                bool rowbool = false;
                bool cblbool = true;
                bool payBool = false;
                string date = Convert.ToString(ds.Tables[0].Rows[row]["Transdate"]);
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

                                            string strVal = "finyearfk='" + chklsfyear.Items[fin].Value + "' and feecategory='" + cbl_sem.Items[sem].Value + "' and headerfk='" + chkl_studhed.Items[hd].Value + "' and Transdate='" + date + "'";
                                            DataTable dtdet = new DataTable();
                                            ds.Tables[1].DefaultView.RowFilter = strVal;
                                            dtdet = ds.Tables[1].DefaultView.ToTable();
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

                                            string strVal = "finyearfk='" + chklsfyear.Items[fin].Value + "' and feecategory='" + cbl_sem.Items[sem].Value + "' and ledgerfk='" + chkl_studled.Items[hd].Value + "' and Transdate='" + date + "'";
                                            DataTable dtdet = new DataTable();
                                            ds.Tables[1].DefaultView.RowFilter = strVal;
                                            dtdet = ds.Tables[1].DefaultView.ToTable();
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
                            string strVal = " Transdate='" + date + "' and paymode='" + payModeVal + "'";
                            int curColCnt = 0;
                            double paiAmount = 0;
                            int.TryParse(Convert.ToString(htPayCol[payModeVal]), out curColCnt);
                            DataView dvhd = new DataView();
                            ds.Tables[2].DefaultView.RowFilter = strVal;
                            dvhd = ds.Tables[2].DefaultView;
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
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["date"]);
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
                    height += 15;
                }
            }
            #endregion

            #region grandtot
            spreadDet.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
            spreadDet.Sheets[0].Rows.Count++;
            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
            spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
            double grandvalue = 0;
            for (int j = 2; j < spreadDet.Sheets[0].ColumnCount; j++)
            {
                double.TryParse(Convert.ToString(grandtotal[j]), out grandvalue);
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalue);
            }
            #endregion
            payModeLabels(htPayCol);
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            spreadDet.Visible = true;
            print.Visible = true;
            spreadDet.Height = height;
            spreadDet.SaveChanges();
        }
        catch
        { }
    }
    protected void spreadLoadDetails(DataSet ds, ref string fromdate, ref string todate)
    {
        try
        {
            #region design
            spreadDet.Sheets[0].RowCount = 0;
            spreadDet.Sheets[0].ColumnCount = 0;
            spreadDet.CommandBar.Visible = false;
            spreadDet.Sheets[0].AutoPostBack = true;
            spreadDet.Sheets[0].ColumnHeader.RowCount = 3;
            spreadDet.Sheets[0].RowHeader.Visible = false;
            spreadDet.Sheets[0].ColumnCount = 2;
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
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 3, 1);

            #region batch
            Hashtable htColCnt = new Hashtable();
            for (int yr = 0; yr < cbl_batch.Items.Count; yr++)
            {
                int batchCol = 0;
                bool batchbol = true;
                string hdrTxtValue = string.Empty;
                if (cbl_batch.Items[yr].Selected)
                {
                    int col = spreadDet.Sheets[0].ColumnCount++;
                    batchbol = false;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(cbl_batch.Items[yr].Text);
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(cbl_batch.Items[yr].Value);
                    hdrTxtValue = Convert.ToString(cbl_batch.Items[yr].Value);
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                    for (int sem = 0; sem < cbl_sem.Items.Count; sem++)
                    {
                        int semcol = 0;
                        bool sembol = true;
                        if (cbl_sem.Items[sem].Selected)
                        {
                            sembol = false;
                            if (batchbol)
                                semcol = spreadDet.Sheets[0].ColumnCount++;
                            else
                                semcol = col;

                            spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(cbl_sem.Items[sem].Text);
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(cbl_sem.Items[sem].Value);
                            hdrTxtValue += "-" + Convert.ToString(cbl_sem.Items[sem].Value);
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                            #region header and ledger
                            if (rbtype.SelectedIndex == 0)
                            {
                                //header
                                int semcolcnt = 0;
                                for (int hd = 0; hd < chkl_studhed.Items.Count; hd++)
                                {
                                    if (chkl_studhed.Items[hd].Selected)
                                    {
                                        semcolcnt++;
                                        batchCol++;
                                        if (sembol)
                                            spreadDet.Sheets[0].ColumnCount++;
                                        spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(chkl_studhed.Items[hd].Text);
                                        spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(chkl_studhed.Items[hd].Value);
                                        string value = hdrTxtValue + "-" + Convert.ToString(chkl_studhed.Items[hd].Value);
                                        htColCnt.Add(value, spreadDet.Sheets[0].ColumnCount - 1);
                                        spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                        spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                        spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                        spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                        spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                        sembol = true;
                                    }
                                }
                                spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(1, semcol, 1, semcolcnt);
                            }
                            else
                            {
                                //ledger
                                int semcolcnt = 0;
                                for (int hd = 0; hd < chkl_studled.Items.Count; hd++)
                                {
                                    if (chkl_studled.Items[hd].Selected)
                                    {
                                        semcolcnt++;
                                        batchCol++;
                                        if (sembol)
                                            spreadDet.Sheets[0].ColumnCount++;
                                        spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(chkl_studled.Items[hd].Text);
                                        spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(chkl_studled.Items[hd].Value);
                                        string value = hdrTxtValue + "-" + Convert.ToString(chkl_studled.Items[hd].Value);
                                        htColCnt.Add(value, spreadDet.Sheets[0].ColumnCount - 1);
                                        spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                        spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                        spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                        spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                        spreadDet.Sheets[0].ColumnHeader.Cells[2, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                        sembol = true;
                                    }
                                }
                                spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(1, semcol, 1, semcolcnt);
                            }
                            batchbol = true;
                            #endregion
                        }
                        hdrTxtValue = Convert.ToString(cbl_batch.Items[yr].Value);
                    }
                    //  batchCol++;
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, col, 1, batchCol);
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

            #endregion

            #region value

            Hashtable grandtotal = new Hashtable();
            int height = 0;
            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
            {
                bool rowbool = false;
                bool cblbool = true;
                double FnltotAmount = 0;
                bool paybool = false;
                string strBatch = string.Empty;
                string strFeecat = string.Empty;
                string date = Convert.ToString(ds.Tables[0].Rows[row]["Transdate"]);
                for (int batch = 0; batch < cbl_batch.Items.Count; batch++)
                {
                    if (cbl_batch.Items[batch].Selected)
                    {
                        if (strBatch == string.Empty)
                            strBatch = cbl_batch.Items[batch].Value;
                        else
                            strBatch += "'" + "," + "'" + cbl_batch.Items[batch].Value;
                        for (int sem = 0; sem < cbl_sem.Items.Count; sem++)
                        {
                            if (cbl_sem.Items[sem].Selected)
                            {

                                if (strFeecat == string.Empty)
                                    strFeecat = cbl_sem.Items[sem].Value;
                                else
                                    strFeecat += "'" + "," + "'" + cbl_sem.Items[sem].Value;
                                if (rbtype.SelectedIndex == 0)
                                {
                                    #region header
                                    for (int hd = 0; hd < chkl_studhed.Items.Count; hd++)
                                    {
                                        if (chkl_studhed.Items[hd].Selected)
                                        {
                                            string strVal = "batch_year='" + cbl_batch.Items[batch].Value + "' and feecategory='" + cbl_sem.Items[sem].Value + "' and headerfk='" + chkl_studhed.Items[hd].Value + "' and Transdate='" + date + "'";
                                            string hashValue = cbl_batch.Items[batch].Value + "-" + cbl_sem.Items[sem].Value + "-" + chkl_studhed.Items[hd].Value;
                                            double paiAmount = 0;
                                            int curColCnt = 0;
                                            int.TryParse(Convert.ToString(htColCnt[hashValue]), out curColCnt);
                                            DataView dvhd = new DataView();
                                            ds.Tables[1].DefaultView.RowFilter = strVal;
                                            dvhd = ds.Tables[1].DefaultView;
                                            if (dvhd.Count > 0)
                                            {
                                                double.TryParse(Convert.ToString(dvhd[0]["debit"]), out paiAmount);
                                                FnltotAmount += paiAmount;
                                                rowbool = true;
                                                paybool = true;
                                            }
                                            if (cblbool)
                                                spreadDet.Sheets[0].RowCount++;
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
                                            cblbool = false;
                                            if (paiAmount == 0)
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = "-";
                                            else
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(paiAmount);
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region ledger
                                    for (int hd = 0; hd < chkl_studled.Items.Count; hd++)
                                    {
                                        if (chkl_studled.Items[hd].Selected)
                                        {
                                            string strVal = "batch_year='" + cbl_batch.Items[batch].Value + "' and feecategory='" + cbl_sem.Items[sem].Value + "' and ledgerfk='" + chkl_studled.Items[hd].Value + "' and Transdate='" + date + "'";
                                            string hashValue = cbl_batch.Items[batch].Value + "-" + cbl_sem.Items[sem].Value + "-" + chkl_studled.Items[hd].Value;
                                            double paiAmount = 0;
                                            int curColCnt = 0;
                                            int.TryParse(Convert.ToString(htColCnt[hashValue]), out curColCnt);
                                            DataView dvhd = new DataView();
                                            ds.Tables[1].DefaultView.RowFilter = strVal;
                                            dvhd = ds.Tables[1].DefaultView;
                                            if (dvhd.Count > 0)
                                            {
                                                double.TryParse(Convert.ToString(dvhd[0]["debit"]), out paiAmount);
                                                FnltotAmount += paiAmount;
                                                rowbool = true;
                                                paybool = true;
                                            }
                                            if (cblbool)
                                                spreadDet.Sheets[0].RowCount++;

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
                                            if (paiAmount == 0)
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = "-";
                                            else
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(paiAmount);
                                            cblbool = false;
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                    }
                }
                if (paybool && ds.Tables[2].Rows.Count > 0)
                {
                    #region paymode
                    for (int s = 0; s < chkl_paid.Items.Count; s++)
                    {
                        if (chkl_paid.Items[s].Selected == true)
                        {
                            string payModeVal = Convert.ToString(chkl_paid.Items[s].Value);
                            string strVal = "batch_year in('" + strBatch + "') and feecategory in('" + strFeecat + "') and Transdate='" + date + "' and paymode='" + payModeVal + "'";
                            int curColCnt = 0;
                            double paiAmount = 0;
                            int.TryParse(Convert.ToString(htPayCol[payModeVal]), out curColCnt);
                            DataView dvhd = new DataView();
                            ds.Tables[2].DefaultView.RowFilter = strVal;
                            dvhd = ds.Tables[2].DefaultView;
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
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["date"]);
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
                    height += 15;
                }
            }
            #region grandtot
            spreadDet.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
            spreadDet.Sheets[0].Rows.Count++;
            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
            spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
            double grandvalue = 0;
            for (int j = 2; j < spreadDet.Sheets[0].ColumnCount; j++)
            {
                double.TryParse(Convert.ToString(grandtotal[j]), out grandvalue);
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalue);
            }
            #endregion
            payModeLabels(htPayCol);
            #endregion
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            spreadDet.Visible = true;
            print.Visible = true;
            spreadDet.Height = height;
            spreadDet.SaveChanges();
        }
        catch { }
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
            degreedetails = "Fees Structure Report";
            pagename = "FeesStructureReport.aspx";
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
                cbl_type.Items.Add(new ListItem("Old Studnent", "1"));
                cbl_type.Items.Add(new ListItem("New    ", "3"));
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
}