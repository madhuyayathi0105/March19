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

public partial class DailyFeesCollectionReport : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    ReuasableMethods reuse = new ReuasableMethods();
    DataSet ds = new DataSet();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static bool usBasedRights = false;
    static Dictionary<string, string> receiptSet = new Dictionary<string, string>();
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
                loadfinanceUser();
                loadheaderandledger();
                ledgerload();
                loadfinanceyear();
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_todate.Attributes.Add("readonly", "readonly");
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
        bool check = false;
        string fromdate = string.Empty;
        string todate = string.Empty;
        ds.Clear();
        ds = dsloadDetails(ref fromdate, ref todate);

        //  ds = dsFilterValues(ds, ref  fromdate, ref  todate);
        // spreadLoadDetails(ds, ref fromdate, ref todate);

        if (rbltype.SelectedIndex == 0)
        {
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                SpreadLoadDetails(ds);
            }
            else
                check = true;
        }
        else
        {
            if (ds.Tables.Count > 0)
            {
                SpreadLoadDetailsOthers(ds);
            }
            else
                check = true;
        }

        if (check)
        {
            lblvalidation1.Text = string.Empty;
            txtexcelname.Text = string.Empty;
            spreadDet.Visible = false;
            print.Visible = false;
            divlabl.Visible = false;
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

        dsflt.Tables.Add(dvfst.ToTable());
        dsflt.Tables.Add(dvsnd.ToTable());

        return dsflt;
    }

    protected DataSet dsloadDetails(ref string fromdate, ref string todate)
    {
        DataSet dsload = new DataSet();
        try
        {
            #region get Value
            UserbasedRights();
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
            string memType = string.Empty;
            bool receiptCheck = receiptNoSetting();
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
            string finUser = Convert.ToString(getCblSelectedValue(cbluser));//added by abarna 19.02.2018
            memType = Convert.ToString(getCblSelectedValue(cblmem));
            fromdate = txt_fromdate.Text;
            todate = txt_todate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
            string strtype = string.Empty;
            string strReg = " and r.cc=0  and r.Exam_Flag<>'debar' and r.DelFlag=0";
            if (rbtype.SelectedIndex == 0)
                strtype = ",headerfk";
            else
                strtype = ",ledgerfk";
            if (cbbfrecon.Checked)
                strRecon = " and ISNULL(IsCanceled,'0')<>'1'";
            else
                strRecon = " and ( isnull(f.IsDeposited,'0')='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1'";
            string applynStr = " AND r.IsConfirm = 1  and isnull(is_enroll,'0')<>'2'";
            //AND Admission_Status = 1
            string strFine = string.Empty;
            if (cbfine.Checked)
                strFine = " ";
            else
                strFine = " and isnull(FineFeecategory,'0')<>'-1'";
            #endregion


            #region Query old

            //if (rbltype.SelectedIndex == 0)
            //{
            //    #region student

            //    SelQ = " select r.batch_year" + strtype + ",f.feecategory,f.paymode,sum(f.debit) as debit,isnull(FineFeecategory,'0') from ft_findailytransaction f,registration r where f.app_no=r.app_no " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + strReg + " and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " group by r.batch_year" + strtype + ",f.feecategory,f.paymode,isnull(FineFeecategory,'0') having sum(debit)>0 ";
            //    if (cbbeforAdm.Checked)
            //    {
            //        SelQ += " union select r.batch_year" + strtype + ",f.feecategory,f.paymode,sum(f.debit) as debit,isnull(FineFeecategory,'0') from ft_findailytransaction f,applyn r where f.app_no=r.app_no " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "')  and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "'  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " " + applynStr + " group by r.batch_year" + strtype + ",f.feecategory,f.paymode,isnull(FineFeecategory,'0') having sum(debit)>0 ";
            //    }
            //    SelQ += " order by r.batch_year,f.feecategory" + strtype + ",f.paymode asc";

            //    SelQ += " select r.batch_year" + strtype + ",f.feecategory,f.paymode,sum(f.debit) as debit,isnull(FineFeecategory,'0') from ft_findailytransaction f,registration r where f.app_no=r.app_no and ( isnull(f.IsDeposited,'0')='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1' and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + strReg + " and f.Transdate between '" + fromdate + "' and '" + todate + "'   " + strFine + " group by r.batch_year" + strtype + ",f.feecategory,f.paymode,isnull(FineFeecategory,'0') having sum(debit)>0  ";
            //    if (cbbeforAdm.Checked)
            //    {
            //        SelQ += " union select r.batch_year" + strtype + ",f.feecategory,f.paymode,sum(f.debit) as debit,isnull(FineFeecategory,'0') from ft_findailytransaction f,applyn r where f.app_no=r.app_no and ( isnull(f.IsDeposited,'0')='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1' and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "')  and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' and f.Transdate between '" + fromdate + "' and '" + todate + "'   " + strFine + " " + applynStr + " group by r.batch_year" + strtype + ",f.feecategory,f.paymode,isnull(FineFeecategory,'0') having sum(debit)>0 ";
            //    }
            //    SelQ += " order by r.batch_year,f.feecategory,f.paymode" + strtype + " asc";
            //    #region receipt no
            //    //receipt no setting
            //    if (!receiptCheck)
            //    {
            //        #region commom
            //        SelQ += " select distinct top(1) f.transcode from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') " + strRecon + " and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " ";
            //        if (memType != "")
            //            SelQ += " and memtype in('1','" + memType + "')";
            //        else
            //            SelQ += " and memtype in('1')";
            //        SelQ += "  order by f.transcode asc  ";

            //        SelQ += " select distinct top(1) f.transcode from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') " + strRecon + " and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + "  ";
            //        if (memType != "")
            //            SelQ += " and memtype in('1','" + memType + "')";
            //        else
            //            SelQ += " and memtype in('1')";
            //        SelQ += " order by f.transcode desc  ";

            //        SelQ += " select distinct f.transcode from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "')  and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " and ISNULL(IsCanceled,'0')='1' ";
            //        if (memType != "")
            //            SelQ += " and memtype in('1','" + memType + "')";
            //        else
            //            SelQ += " and memtype in('1')";
            //        SelQ += " order by f.transcode asc  ";
            //        #endregion
            //    }
            //    else
            //    {
            //        #region headerwise
            //        SelQ += " select distinct  f.transcode,f.headerfk from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') " + strRecon + " and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " ";
            //        if(memType!="")
            //            SelQ+=" and memtype in('1','" + memType + "')";
            //        else
            //            SelQ += " and memtype in('1')";
            //        SelQ += "  order by f.transcode asc  ";

            //        SelQ += " select distinct f.transcode,f.headerfk from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') " + strRecon + " and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + "";
            //       if(memType!="")
            //            SelQ+=" and memtype in('1','" + memType + "')";
            //        else
            //            SelQ += " and memtype in('1')";
            //        SelQ += " order by f.transcode desc  ";

            //        SelQ += " select distinct f.transcode,f.headerfk from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "')  and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " and ISNULL(IsCanceled,'0')='1' ";
            //        if (memType != "")
            //            SelQ += " and memtype in('1','" + memType + "')";
            //        else
            //            SelQ += " and memtype in('1')";
            //        SelQ += " order by f.transcode asc  ";
            //        #endregion
            //    }
            //    #endregion
            //    #region old query
            //    //receipt no setting
            //    //if (!receiptCheck)
            //    //{
            //    //    #region commom
            //    //    SelQ += " select distinct top(1) f.transcode from ft_findailytransaction f,registration r where f.app_no=r.app_no " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0 and r.college_code ='" + collegecode + "' " + strReg + " and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + "  ";
            //    //    if (cbbeforAdm.Checked)
            //    //    {
            //    //        SelQ += " union select distinct top(1) f.transcode from ft_findailytransaction f,applyn r where f.app_no=r.app_no " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "')  and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0 and r.college_code ='" + collegecode + "'  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + "";
            //    //    }
            //    //    SelQ += " order by f.transcode asc";

            //    //    SelQ += " select distinct top(1) f.transcode from ft_findailytransaction f,registration r where f.app_no=r.app_no " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0 and r.college_code ='" + collegecode + "'  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " ";
            //    //    if (cbbeforAdm.Checked)
            //    //    {
            //    //        SelQ += " union select distinct top(1) f.transcode from ft_findailytransaction f,applyn r where f.app_no=r.app_no " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "')  and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0 and r.college_code ='" + collegecode + "'  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + "";
            //    //    }
            //    //    SelQ += " order by f.transcode desc";

            //    //    SelQ += " select distinct f.transcode from ft_findailytransaction f,registration r where f.app_no=r.app_no and ISNULL(IsCanceled,'0')='1' and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0 and r.college_code ='" + collegecode + "' " + strReg + " and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + "   ";
            //    //    if (cbbeforAdm.Checked)
            //    //    {
            //    //        SelQ += " union select distinct f.transcode from ft_findailytransaction f,applyn r where f.app_no=r.app_no and ISNULL(IsCanceled,'0')='1' and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "')  and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0 and r.college_code ='" + collegecode + "'  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + "";
            //    //    }
            //    //    SelQ += " order by f.transcode asc";
            //    //    #endregion
            //    //}
            //    //else
            //    //{
            //    //    #region headerwise
            //    //    SelQ += " select distinct f.transcode,f.headerfk from ft_findailytransaction f,registration r where f.app_no=r.app_no " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0 and r.college_code ='" + collegecode + "' " + strReg + " and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " ";
            //    //    if (cbbeforAdm.Checked)
            //    //    {
            //    //        SelQ += " union select distinct f.transcode,f.headerfk from ft_findailytransaction f,applyn r where f.app_no=r.app_no " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "')  and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0 and r.college_code ='" + collegecode + "'  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + "";
            //    //    }
            //    //    SelQ += " order by f.transcode asc";
            //    //    SelQ += " select distinct f.transcode,f.headerfk from ft_findailytransaction f,registration r where f.app_no=r.app_no " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0 and r.college_code ='" + collegecode + "' " + strReg + " and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " ";
            //    //    if (cbbeforAdm.Checked)
            //    //    {
            //    //        SelQ += " union select distinct f.transcode,f.headerfk from ft_findailytransaction f,applyn r where f.app_no=r.app_no " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "')  and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0 and r.college_code ='" + collegecode + "'  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + "";
            //    //    }
            //    //    SelQ += " order by f.transcode desc";
            //    //    SelQ += " select distinct f.transcode,f.headerfk from ft_findailytransaction f,registration r where f.app_no=r.app_no and ISNULL(IsCanceled,'0')='1' and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0 and r.college_code ='" + collegecode + "' " + strReg + " and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " ";
            //    //    if (cbbeforAdm.Checked)
            //    //    {
            //    //        SelQ += " union select distinct f.transcode,f.headerfk from ft_findailytransaction f,applyn r where f.app_no=r.app_no and ISNULL(IsCanceled,'0')='1' and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0 and r.college_code ='" + collegecode + "' and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " ";
            //    //    }
            //    //    SelQ += " order by f.transcode asc";
            //    //    #endregion
            //    //}
            //    #endregion

            //    #endregion

            //    #region others

            //    //for (int i = 0; i < cblmem.Items.Count; i++)
            //    //{
            //    //    if (cblmem.Items[i].Selected)
            //    //    {
            //    //        if (cblmem.Items[i].Text == "Staff")
            //    //        {
            //                //staff
            //                SelQ += " select  SUM(debit) as debit ,MemType,f.paymode" + strtype + " from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and t.dept_code =h.dept_code and t.desig_code =d.desig_code and T.staff_code =s.staff_code and T.latestrec ='1' " + strRecon + " and d.collegeCode='" + collegecode + "' and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //                if (usBasedRights == true)
            //                    SelQ += " and f.EntryUserCode in('" + usercode + "')";
            //                SelQ += " group by MemType,f.paymode" + strtype + "";
            //                //reconsilation
            //                SelQ += " select  SUM(debit) as debit ,MemType,f.paymode" + strtype + " from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and t.dept_code =h.dept_code and t.desig_code =d.desig_code and T.staff_code =s.staff_code and T.latestrec ='1' and ( isnull(f.IsDeposited,'0')='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1' and d.collegeCode='" + collegecode + "' and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //                if (usBasedRights == true)
            //                    SelQ += " and f.EntryUserCode in('" + usercode + "')";
            //                SelQ += " group by MemType,f.paymode" + strtype + "";
            //            //}
            //            //if (cblmem.Items[i].Text == "Vendor")
            //            //{
            //                //vendor
            //                SelQ += "  SELECT SUM(debit) as debit ,MemType,f.paymode" + strtype + "  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1' " + strRecon + "  and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //                if (usBasedRights == true)
            //                    SelQ += " and f.EntryUserCode in('" + usercode + "')";
            //                SelQ += " group by MemType,f.paymode" + strtype + "";
            //                //reconsilation
            //                SelQ += "  SELECT SUM(debit) as debit ,MemType,f.paymode" + strtype + "  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1' and ( isnull(f.IsDeposited,'0')='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1'  and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //                if (usBasedRights == true)
            //                    SelQ += " and f.EntryUserCode in('" + usercode + "')";
            //                SelQ += " group by MemType,f.paymode" + strtype + "";
            //            //}
            //            //if (cblmem.Items[i].Text == "Others")
            //            //{
            //                //others
            //                SelQ += "  SELECT SUM(debit) as debit ,MemType,f.paymode" + strtype + " FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5' " + strRecon + " and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //                if (usBasedRights == true)
            //                    SelQ += " and f.EntryUserCode in('" + usercode + "')";
            //                SelQ += " group by MemType,f.paymode" + strtype + "";

            //                SelQ += "  SELECT SUM(debit) as debit ,MemType,f.paymode" + strtype + " FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5' and ( isnull(f.IsDeposited,'0')='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1' and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //                if (usBasedRights == true)
            //                    SelQ += " and f.EntryUserCode in('" + usercode + "')";
            //                SelQ += " group by MemType,f.paymode" + strtype + "";
            //    //        }
            //    //    }
            //    //}
            //    #endregion
            //}
            //else
            //{
            //    #region others
            //    for (int i = 0; i < cblmem.Items.Count; i++)
            //    {
            //        if (cblmem.Items[i].Selected)
            //        {
            //            if (cblmem.Items[i].Text == "Staff")
            //            {
            //                //staff
            //                SelQ = " select  SUM(debit) as debit ,MemType,f.paymode" + strtype + " from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and t.dept_code =h.dept_code and t.desig_code =d.desig_code and T.staff_code =s.staff_code and T.latestrec ='1' " + strRecon + " and d.collegeCode='" + collegecode + "' and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //                if (usBasedRights == true)
            //                    SelQ += " and f.EntryUserCode in('" + usercode + "')";
            //                SelQ += " group by MemType,f.paymode" + strtype + "";
            //                //reconsilation
            //                SelQ += " select  SUM(debit) as debit ,MemType,f.paymode" + strtype + " from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and t.dept_code =h.dept_code and t.desig_code =d.desig_code and T.staff_code =s.staff_code and T.latestrec ='1' and ( isnull(f.IsDeposited,'0')='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1' and d.collegeCode='" + collegecode + "' and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //                if (usBasedRights == true)
            //                    SelQ += " and f.EntryUserCode in('" + usercode + "')";
            //                SelQ += " group by MemType,f.paymode" + strtype + "";
            //            }
            //            if (cblmem.Items[i].Text == "Vendor")
            //            {
            //                //vendor
            //                SelQ += "  SELECT SUM(debit) as debit ,MemType,f.paymode" + strtype + "  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1' " + strRecon + "  and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //                if (usBasedRights == true)
            //                    SelQ += " and f.EntryUserCode in('" + usercode + "')";
            //                SelQ += " group by MemType,f.paymode" + strtype + "";
            //                //reconsilation
            //                SelQ += "  SELECT SUM(debit) as debit ,MemType,f.paymode" + strtype + "  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1' and ( isnull(f.IsDeposited,'0')='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1'  and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //                if (usBasedRights == true)
            //                    SelQ += " and f.EntryUserCode in('" + usercode + "')";
            //                SelQ += " group by MemType,f.paymode" + strtype + "";
            //            }
            //            if (cblmem.Items[i].Text == "Others")
            //            {
            //                //others
            //                SelQ += "  SELECT SUM(debit) as debit ,MemType,f.paymode" + strtype + " FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5' " + strRecon + " and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //                if (usBasedRights == true)
            //                    SelQ += " and f.EntryUserCode in('" + usercode + "')";
            //                SelQ += " group by MemType,f.paymode" + strtype + "";

            //                SelQ += "  SELECT SUM(debit) as debit ,MemType,f.paymode" + strtype + " FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5' and ( isnull(f.IsDeposited,'0')='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1' and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //                if (usBasedRights == true)
            //                    SelQ += " and f.EntryUserCode in('" + usercode + "')";
            //                SelQ += " group by MemType,f.paymode" + strtype + "";
            //            }
            //        }
            //    }
            //    #endregion

            //    #region receipt no
            //    //receipt no setting
            //    if (!receiptCheck)
            //    {
            //        #region commom
            //        SelQ += " select distinct top(1) f.transcode from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') " + strRecon + " and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " and memtype in('" + memType + "') order by f.transcode asc  ";

            //        SelQ += " select distinct top(1) f.transcode from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') " + strRecon + " and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " and memtype in('" + memType + "') order by f.transcode desc  ";

            //        SelQ += " select distinct f.transcode from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "')  and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " and memtype in('" + memType + "') and ISNULL(IsCanceled,'0')='1' order by f.transcode asc  ";
            //        #endregion
            //    }
            //    else
            //    {
            //        #region headerwise
            //        SelQ += " select distinct  f.transcode,f.headerfk from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') " + strRecon + " and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " and memtype in('" + memType + "') order by f.transcode asc  ";

            //        SelQ += " select distinct f.transcode,f.headerfk from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') " + strRecon + " and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " and memtype in('" + memType + "') order by f.transcode desc  ";

            //        SelQ += " select distinct f.transcode,f.headerfk from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "')  and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " and memtype in('" + memType + "') and ISNULL(IsCanceled,'0')='1' order by f.transcode asc  ";

            //        #endregion
            //    }
            //    #endregion
            //}
            //// SelQ += " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code ='" + collegecode + "'";
            ////SelQ += " select HeaderFK,LedgerPK,HeaderName,LedgerName from FM_HeaderMaster h,FM_LedgerMaster l where l.HeaderFK =h.HeaderPK and h.CollegeCode ='" + collegecode + "'";
            //dsload.Clear();
            //dsload = d2.select_method_wo_parameter(SelQ, "Text");
            #endregion

            #region new Query
            if (rbltype.SelectedIndex == 0)
            {
                #region student

                SelQ = " select batch_year" + strtype + ",feecategory,paymode,sum(debit) as debit,isnull(FineFeecategory,'0')  as FineFeecategory from(";
                SelQ += " select r.batch_year" + strtype + ",f.feecategory,f.paymode,debit,isnull(FineFeecategory,'0') as FineFeecategory from ft_findailytransaction f,registration r where f.app_no=r.app_no " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + strReg + " and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + "and f.entryusercode in('" + finUser + "') and isnull(debit,'0')>0 ";
                if (cbbeforAdm.Checked)
                {
                    SelQ += " union all select r.batch_year" + strtype + ",f.feecategory,f.paymode,debit,isnull(FineFeecategory,'0') as FineFeecategory from ft_findailytransaction f,applyn r where f.app_no=r.app_no " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "')  and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "'  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " " + applynStr + "and f.entryusercode in('" + finUser + "')  and isnull(debit,'0')>0";
                }
                SelQ += ") tbl group by batch_year" + strtype + ",feecategory,paymode,FineFeecategory order by batch_year,feecategory" + strtype + ",paymode asc";

                SelQ += " select batch_year" + strtype + ",feecategory,paymode,sum(debit) as debit,isnull(FineFeecategory,'0')  as FineFeecategory from(";
                SelQ += " select r.batch_year" + strtype + ",f.feecategory,f.paymode,debit,isnull(FineFeecategory,'0') as FineFeecategory from ft_findailytransaction f,registration r where f.app_no=r.app_no and ( isnull(f.IsDeposited,'0')='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1' and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + strReg + " and f.Transdate between '" + fromdate + "' and '" + todate + "'   " + strFine + "and f.entryusercode in('" + finUser + "')  and isnull(debit,'0')>0 ";
                if (cbbeforAdm.Checked)
                {
                    SelQ += " union all select r.batch_year" + strtype + ",f.feecategory,f.paymode,debit,isnull(FineFeecategory,'0') as FineFeecategory from ft_findailytransaction f,applyn r where f.app_no=r.app_no and ( isnull(f.IsDeposited,'0')='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1' and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') and f.feecategory in('" + sem + "')  and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' and f.Transdate between '" + fromdate + "' and '" + todate + "'   " + strFine + " " + applynStr + "and f.entryusercode in('" + finUser + "') and isnull(debit,'0')>0 ";
                }
                SelQ += ") tbl group by batch_year" + strtype + ",feecategory,paymode,FineFeecategory order by batch_year,feecategory,paymode" + strtype + " asc";
                #region receipt no
                //receipt no setting
                if (!receiptCheck)
                {
                    #region commom
                    SelQ += " select distinct top(1) f.transcode from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') " + strRecon + " and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " and f.entryusercode in('" + finUser + "') ";
                    if (memType != "")
                        SelQ += " and memtype in('1','" + memType + "')";
                    else
                        SelQ += " and memtype in('1')";
                    SelQ += "  order by f.transcode asc  ";

                    SelQ += " select distinct top(1) f.transcode from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') " + strRecon + " and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " and f.entryusercode in('" + finUser + "')  ";
                    if (memType != "")
                        SelQ += " and memtype in('1','" + memType + "')";
                    else
                        SelQ += " and memtype in('1')";
                    SelQ += " order by f.transcode desc  ";

                    SelQ += " select distinct f.transcode from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "')  and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + "and f.entryusercode in('" + finUser + "') and ISNULL(IsCanceled,'0')='1' ";
                    if (memType != "")
                        SelQ += " and memtype in('1','" + memType + "')";
                    else
                        SelQ += " and memtype in('1')";
                    SelQ += " order by f.transcode asc  ";
                    #endregion
                }
                else
                {
                    #region headerwise
                    SelQ += " select distinct  f.transcode,f.headerfk from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') " + strRecon + " and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + "and f.entryusercode in('" + finUser + "') ";
                    if (memType != "")
                        SelQ += " and memtype in('1','" + memType + "')";
                    else
                        SelQ += " and memtype in('1')";
                    SelQ += "  order by f.transcode asc  ";

                    SelQ += " select distinct f.transcode,f.headerfk from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') " + strRecon + " and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + "and f.entryusercode in('" + finUser + "')";
                    if (memType != "")
                        SelQ += " and memtype in('1','" + memType + "')";
                    else
                        SelQ += " and memtype in('1')";
                    SelQ += " order by f.transcode desc  ";

                    SelQ += " select distinct f.transcode,f.headerfk from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "')  and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + "and f.entryusercode in('" + finUser + "') and ISNULL(IsCanceled,'0')='1' ";
                    if (memType != "")
                        SelQ += " and memtype in('1','" + memType + "')";
                    else
                        SelQ += " and memtype in('1')";
                    SelQ += " order by f.transcode asc  ";
                    #endregion
                }
                #endregion


                #endregion

                #region others

                //for (int i = 0; i < cblmem.Items.Count; i++)
                //{
                //    if (cblmem.Items[i].Selected)
                //    {
                //        if (cblmem.Items[i].Text == "Staff")
                //        {
                //staff
                SelQ += " select  SUM(debit) as debit ,MemType,f.paymode" + strtype + " from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and t.dept_code =h.dept_code and t.desig_code =d.desig_code and T.staff_code =s.staff_code and T.latestrec ='1' " + strRecon + " and d.collegeCode='" + collegecode + "' and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                //  if (usBasedRights == true)
                SelQ += " and f.EntryUserCode in('" + finUser + "')";
                SelQ += " group by MemType,f.paymode" + strtype + "";
                //reconsilation
                SelQ += " select  SUM(debit) as debit ,MemType,f.paymode" + strtype + " from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and t.dept_code =h.dept_code and t.desig_code =d.desig_code and T.staff_code =s.staff_code and T.latestrec ='1' and ( isnull(f.IsDeposited,'0')='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1' and d.collegeCode='" + collegecode + "' and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                //if (usBasedRights == true)
                SelQ += " and f.EntryUserCode in('" + finUser + "')";//  SelQ += " and f.EntryUserCode in('" + usercode + "')";
                SelQ += " group by MemType,f.paymode" + strtype + "";
                //}
                //if (cblmem.Items[i].Text == "Vendor")
                //{
                //vendor
                SelQ += "  SELECT SUM(debit) as debit ,MemType,f.paymode" + strtype + "  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1' " + strRecon + "  and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                //if (usBasedRights == true)
                SelQ += " and f.EntryUserCode in('" + finUser + "')";
                SelQ += " group by MemType,f.paymode" + strtype + "";
                //reconsilation
                SelQ += "  SELECT SUM(debit) as debit ,MemType,f.paymode" + strtype + "  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1' and ( isnull(f.IsDeposited,'0')='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1'  and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                //if (usBasedRights == true)
                SelQ += " and f.EntryUserCode in('" + finUser + "')";
                SelQ += " group by MemType,f.paymode" + strtype + "";
                //}
                //if (cblmem.Items[i].Text == "Others")
                //{
                //others
                SelQ += "  SELECT SUM(debit) as debit ,MemType,f.paymode" + strtype + " FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5' " + strRecon + " and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                //if (usBasedRights == true)
                SelQ += " and f.EntryUserCode in('" + finUser + "')";
                SelQ += " group by MemType,f.paymode" + strtype + "";

                SelQ += "  SELECT SUM(debit) as debit ,MemType,f.paymode" + strtype + " FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5' and ( isnull(f.IsDeposited,'0')='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1' and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                //if (usBasedRights == true)
                SelQ += " and f.EntryUserCode in('" + finUser + "')";
                SelQ += " group by MemType,f.paymode" + strtype + "";
                //        }
                //    }
                //}
                #endregion
            }
            else
            {
                #region others
                for (int i = 0; i < cblmem.Items.Count; i++)
                {
                    if (cblmem.Items[i].Selected)
                    {
                        if (cblmem.Items[i].Text == "Staff")
                        {
                            //staff
                            SelQ = " select  SUM(debit) as debit ,MemType,f.paymode" + strtype + " from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and t.dept_code =h.dept_code and t.desig_code =d.desig_code and T.staff_code =s.staff_code and T.latestrec ='1' " + strRecon + " and d.collegeCode='" + collegecode + "' and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                            //if (usBasedRights == true)
                            SelQ += " and f.EntryUserCode in('" + finUser + "')";
                            SelQ += " group by MemType,f.paymode" + strtype + "";
                            //reconsilation
                            SelQ += " select  SUM(debit) as debit ,MemType,f.paymode" + strtype + " from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and t.dept_code =h.dept_code and t.desig_code =d.desig_code and T.staff_code =s.staff_code and T.latestrec ='1' and ( isnull(f.IsDeposited,'0')='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1' and d.collegeCode='" + collegecode + "' and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                            // if (usBasedRights == true)
                            SelQ += " and f.EntryUserCode in('" + finUser + "')";
                            SelQ += " group by MemType,f.paymode" + strtype + "";
                        }
                        if (cblmem.Items[i].Text == "Vendor")
                        {
                            //vendor
                            SelQ += "  SELECT SUM(debit) as debit ,MemType,f.paymode" + strtype + "  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1' " + strRecon + "  and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                            //if (usBasedRights == true)
                            SelQ += " and f.EntryUserCode in('" + finUser + "')";
                            SelQ += " group by MemType,f.paymode" + strtype + "";
                            //reconsilation
                            SelQ += "  SELECT SUM(debit) as debit ,MemType,f.paymode" + strtype + "  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1' and ( isnull(f.IsDeposited,'0')='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1'  and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                            // if (usBasedRights == true)
                            SelQ += " and f.EntryUserCode in('" + finUser + "')";
                            SelQ += " group by MemType,f.paymode" + strtype + "";
                        }
                        if (cblmem.Items[i].Text == "Others")
                        {
                            //others
                            SelQ += "  SELECT SUM(debit) as debit ,MemType,f.paymode" + strtype + " FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5' " + strRecon + " and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                            //if (usBasedRights == true)
                            SelQ += " and f.EntryUserCode in('" + finUser + "')";
                            SelQ += " group by MemType,f.paymode" + strtype + "";

                            SelQ += "  SELECT SUM(debit) as debit ,MemType,f.paymode" + strtype + " FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5' and ( isnull(f.IsDeposited,'0')='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1' and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                            // if (usBasedRights == true)
                            SelQ += " and f.EntryUserCode in('" + finUser + "')";
                            SelQ += " group by MemType,f.paymode" + strtype + "";
                        }
                    }
                }
                #endregion

                #region receipt no
                //receipt no setting
                if (!receiptCheck)
                {
                    #region commom
                    SelQ += " select distinct top(1) f.transcode from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') " + strRecon + " and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " and memtype in('" + memType + "') and f.EntryUserCode in('" + finUser + "') order by f.transcode asc  ";

                    SelQ += " select distinct top(1) f.transcode from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') " + strRecon + " and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " and memtype in('" + memType + "') and f.EntryUserCode in('" + finUser + "') order by f.transcode desc  ";

                    SelQ += " select distinct f.transcode from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "')  and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " and memtype in('" + memType + "') and f.EntryUserCode in('" + finUser + "') and ISNULL(IsCanceled,'0')='1' order by f.transcode asc  ";
                    #endregion
                }
                else
                {
                    #region headerwise
                    SelQ += " select distinct  f.transcode,f.headerfk from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') " + strRecon + " and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " and memtype in('" + memType + "') and f.EntryUserCode in('" + finUser + "') order by f.transcode asc  ";

                    SelQ += " select distinct f.transcode,f.headerfk from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') " + strRecon + " and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " and memtype in('" + memType + "') and f.EntryUserCode in('" + finUser + "') order by f.transcode desc  ";

                    SelQ += " select distinct f.transcode,f.headerfk from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "')  and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " and memtype in('" + memType + "') and f.EntryUserCode in('" + finUser + "') and ISNULL(IsCanceled,'0')='1' order by f.transcode asc  ";

                    #endregion
                }
                #endregion
            }
            // SelQ += " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code ='" + collegecode + "'";
            //SelQ += " select HeaderFK,LedgerPK,HeaderName,LedgerName from FM_HeaderMaster h,FM_LedgerMaster l where l.HeaderFK =h.HeaderPK and h.CollegeCode ='" + collegecode + "'";
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(SelQ, "Text");
            #endregion

        }
        catch { dsload.Clear(); }
        return dsload;
    }

    protected void SpreadLoadDetails(DataSet ds)
    {
        try
        {
            #region design
            bool receptCh = receiptNoSetting();
            spreadDet.Sheets[0].RowCount = 0;
            spreadDet.Sheets[0].ColumnCount = 0;
            spreadDet.CommandBar.Visible = false;
            spreadDet.Sheets[0].AutoPostBack = true;
            spreadDet.Sheets[0].ColumnHeader.RowCount = 2;
            spreadDet.Sheets[0].RowHeader.Visible = false;
            spreadDet.Sheets[0].ColumnCount = 4;
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

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Text = lblbatch.Text;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Text = lblsem.Text;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            string strHdName = string.Empty;
            if (rbtype.SelectedIndex == 0)
                strHdName = lblheader.Text;
            else
                strHdName = lblledger.Text;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Text = strHdName;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;

            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);

            int checkva = 0;
            Hashtable htPayCol = new Hashtable();
            int pCnt = spreadDet.Sheets[0].ColumnCount++;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Text = "Paymode";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            for (int s = 0; s < chkl_paid.Items.Count; s++)
            {
                if (chkl_paid.Items[s].Selected == true)
                {
                    checkva++;
                    if (checkva > 1)
                        spreadDet.Sheets[0].ColumnCount++;
                    htPayCol.Add(Convert.ToString(chkl_paid.Items[s].Value), spreadDet.Sheets[0].ColumnCount - 1);
                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(chkl_paid.Items[s].Text);
                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(chkl_paid.Items[s].Value);
                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                }
            }
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, pCnt, 1, checkva);

            spreadDet.Sheets[0].ColumnCount++;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Text = "Total";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, spreadDet.Sheets[0].ColumnCount - 1, 2, 1);
            #endregion

            #region value
            Dictionary<string, double> cashTot = new Dictionary<string, double>();
            Dictionary<string, string> headname = new Dictionary<string, string>();
            Hashtable grandtotal = new Hashtable();
            Hashtable total = new Hashtable();
            bool boolColumn = false;
            bool boolGrand = false;
            int height = 0;
            int rowCnt = 0;
            for (int batch = 0; batch < cbl_batch.Items.Count; batch++)
            {
                if (cbl_batch.Items[batch].Selected)
                {
                    bool boolyear = false;
                    for (int sem = 0; sem < cbl_sem.Items.Count; sem++)
                    {
                        if (cbl_sem.Items[sem].Selected)
                        {
                            if (rbtype.SelectedIndex == 0)
                            {
                                #region header
                                for (int hd = 0; hd < chkl_studhed.Items.Count; hd++)
                                {
                                    double FnltotAmount = 0;
                                    bool boolpay = false;
                                    if (chkl_studhed.Items[hd].Selected)
                                    {
                                        #region paymode
                                        boolColumn = true;
                                        for (int s = 0; s < chkl_paid.Items.Count; s++)
                                        {
                                            if (chkl_paid.Items[s].Selected == true)
                                            {
                                                string payModeVal = Convert.ToString(chkl_paid.Items[s].Value);
                                                string strVal = "batch_year='" + cbl_batch.Items[batch].Value + "' and feecategory='" + cbl_sem.Items[sem].Value + "' and headerfk='" + chkl_studhed.Items[hd].Value + "' and paymode='" + payModeVal + "'";
                                                int curColCnt = 0;
                                                double paiAmount = 0;
                                                int.TryParse(Convert.ToString(htPayCol[payModeVal]), out curColCnt);
                                                DataView dvhd = new DataView();
                                                ds.Tables[0].DefaultView.RowFilter = strVal;
                                                dvhd = ds.Tables[0].DefaultView;
                                                if (dvhd.Count > 0)
                                                    double.TryParse(Convert.ToString(dvhd[0]["debit"]), out paiAmount);
                                                FnltotAmount += paiAmount;
                                                if (paiAmount != 0)
                                                {
                                                    if (boolColumn)
                                                    {
                                                        spreadDet.Sheets[0].RowCount++;
                                                        height += 15;
                                                        rowCnt++;
                                                        boolpay = true;
                                                        boolyear = true;
                                                    }
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(rowCnt);
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(cbl_batch.Items[batch].Text);
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(cbl_sem.Items[sem].Text);
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(chkl_studhed.Items[hd].Text);
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
                                                    boolColumn = false;
                                                    //cashier total
                                                    string cashVal = string.Empty; ;
                                                    cashVal = chkl_studhed.Items[hd].Text + "$" + Convert.ToDouble(curColCnt);
                                                    if (!cashTot.ContainsKey(cashVal))
                                                    {
                                                        cashTot.Add(cashVal, paiAmount);
                                                    }
                                                    else
                                                    {
                                                        double amount = 0;
                                                        double.TryParse(Convert.ToString(cashTot[cashVal]), out amount);
                                                        amount += paiAmount;
                                                        cashTot.Remove(cashVal);
                                                        cashTot.Add(cashVal, amount);
                                                    }
                                                    if (!headname.ContainsKey(chkl_studhed.Items[hd].Text))
                                                        headname.Add(chkl_studhed.Items[hd].Text, chkl_studhed.Items[hd].Value);
                                                }
                                                if (boolpay)
                                                {
                                                    if (payModeVal == "2" || payModeVal == "3")
                                                    {
                                                        double clrAmount = 0;
                                                        ds.Tables[1].DefaultView.RowFilter = strVal;
                                                        DataView dvclr = ds.Tables[1].DefaultView;
                                                        if (dvclr.Count > 0)
                                                            double.TryParse(Convert.ToString(dvclr[0]["debit"]), out clrAmount);

                                                        if (paiAmount != 0)
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(paiAmount + "[" + clrAmount + "]");
                                                        else
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = "-";
                                                    }
                                                    else
                                                    {
                                                        if (paiAmount != 0)
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(paiAmount);
                                                        else
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = "-";
                                                    }
                                                    rowColor(payModeVal, curColCnt, spreadDet, spreadDet.Sheets[0].RowCount - 1);
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                    if (boolpay)
                                    {
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(FnltotAmount);
                                        if (!grandtotal.ContainsKey(spreadDet.Sheets[0].ColumnCount - 1))
                                            grandtotal.Add(spreadDet.Sheets[0].ColumnCount - 1, Convert.ToString(FnltotAmount));
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(grandtotal[spreadDet.Sheets[0].ColumnCount - 1]), out amount);
                                            amount += FnltotAmount;
                                            grandtotal.Remove(spreadDet.Sheets[0].ColumnCount - 1);
                                            grandtotal.Add(spreadDet.Sheets[0].ColumnCount - 1, Convert.ToString(amount));
                                        }
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region Ledger
                                for (int hd = 0; hd < chkl_studled.Items.Count; hd++)
                                {
                                    double FnltotAmount = 0;
                                    bool boolpay = false;
                                    if (chkl_studled.Items[hd].Selected)
                                    {
                                        #region paymode
                                        boolColumn = true;
                                        for (int s = 0; s < chkl_paid.Items.Count; s++)
                                        {
                                            if (chkl_paid.Items[s].Selected == true)
                                            {
                                                string payModeVal = Convert.ToString(chkl_paid.Items[s].Value);
                                                string strVal = "batch_year='" + cbl_batch.Items[batch].Value + "' and feecategory='" + cbl_sem.Items[sem].Value + "' and ledgerfk='" + chkl_studled.Items[hd].Value + "' and paymode='" + payModeVal + "'";
                                                int curColCnt = 0;
                                                double paiAmount = 0;
                                                int.TryParse(Convert.ToString(htPayCol[payModeVal]), out curColCnt);
                                                DataView dvhd = new DataView();
                                                ds.Tables[0].DefaultView.RowFilter = strVal;
                                                dvhd = ds.Tables[0].DefaultView;
                                                if (dvhd.Count > 0)
                                                    double.TryParse(Convert.ToString(dvhd[0]["debit"]), out paiAmount);
                                                FnltotAmount += paiAmount;
                                                if (paiAmount != 0)
                                                {
                                                    if (boolColumn)
                                                    {
                                                        spreadDet.Sheets[0].RowCount++;
                                                        height += 15;
                                                        rowCnt++;
                                                        boolpay = true;
                                                        boolyear = true;
                                                    }
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(rowCnt);
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(cbl_batch.Items[batch].Text);
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(cbl_sem.Items[sem].Text);
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(chkl_studled.Items[hd].Text);
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
                                                    boolColumn = false;
                                                    //cashier total
                                                    string cashVal = string.Empty; ;
                                                    cashVal = chkl_studled.Items[hd].Text + "$" + Convert.ToDouble(curColCnt);
                                                    if (!cashTot.ContainsKey(cashVal))
                                                    {
                                                        cashTot.Add(cashVal, paiAmount);
                                                    }
                                                    else
                                                    {
                                                        double amount = 0;
                                                        double.TryParse(Convert.ToString(cashTot[cashVal]), out amount);
                                                        amount += paiAmount;
                                                        cashTot.Remove(cashVal);
                                                        cashTot.Add(cashVal, amount);
                                                    }
                                                    if (!headname.ContainsKey(chkl_studled.Items[hd].Text))
                                                        headname.Add(chkl_studled.Items[hd].Text, chkl_studled.Items[hd].Value);
                                                }
                                                if (boolpay)
                                                {
                                                    if (payModeVal == "2" || payModeVal == "3")
                                                    {
                                                        double clrAmount = 0;
                                                        ds.Tables[1].DefaultView.RowFilter = strVal;
                                                        DataView dvclr = ds.Tables[1].DefaultView;
                                                        if (dvclr.Count > 0)
                                                            double.TryParse(Convert.ToString(dvclr[0]["debit"]), out clrAmount);

                                                        if (paiAmount != 0)
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(paiAmount + "[" + clrAmount + "]");
                                                        else
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = "-";
                                                    }
                                                    else
                                                    {
                                                        if (paiAmount != 0)
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(paiAmount);
                                                        else
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = "-";
                                                    }
                                                    rowColor(payModeVal, curColCnt, spreadDet, spreadDet.Sheets[0].RowCount - 1);
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                    if (boolpay)
                                    {
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(FnltotAmount);
                                        if (!grandtotal.ContainsKey(spreadDet.Sheets[0].ColumnCount - 1))
                                            grandtotal.Add(spreadDet.Sheets[0].ColumnCount - 1, Convert.ToString(FnltotAmount));
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(grandtotal[spreadDet.Sheets[0].ColumnCount - 1]), out amount);
                                            amount += FnltotAmount;
                                            grandtotal.Remove(spreadDet.Sheets[0].ColumnCount - 1);
                                            grandtotal.Add(spreadDet.Sheets[0].ColumnCount - 1, Convert.ToString(amount));
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                    if (boolyear)
                    {
                        #region Total
                        // spreadDet.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
                        spreadDet.Sheets[0].Rows.Count++;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Total";
                        spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                        spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
                        spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.White;
                        double grandvalue = 0;
                        for (int j = 4; j < spreadDet.Sheets[0].ColumnCount; j++)
                        {
                            double.TryParse(Convert.ToString(grandtotal[j]), out grandvalue);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalue);
                            if (!total.ContainsKey(spreadDet.Sheets[0].ColumnCount - 1))
                                total.Add(j, Convert.ToString(grandvalue));
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(total[j]), out amount);
                                amount += grandvalue;
                                total.Remove(j);
                                total.Add(j, Convert.ToString(amount));
                            }
                        }
                        grandtotal.Clear();
                        boolGrand = true;
                        #endregion
                    }
                }
                spreadDet.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                spreadDet.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            }
            #endregion

            #region others
            DataView dvmem = new DataView();
            DataView dvpay = new DataView();

            for (int mem = 0; mem < cblmem.Items.Count; mem++)
            {
                bool membool = false;
                bool boolyear = false;
                if (cblmem.Items[mem].Selected)
                {
                    if (rbtype.SelectedIndex == 0)
                    {
                        #region header
                        for (int hd = 0; hd < chkl_studhed.Items.Count; hd++)
                        {
                            double FnltotAmount = 0;
                            bool boolpay = false;
                            boolColumn = true;
                            if (chkl_studhed.Items[hd].Selected)
                            {
                                for (int s = 0; s < chkl_paid.Items.Count; s++)
                                {
                                    bool boolpaymode = false;
                                    if (chkl_paid.Items[s].Selected == true)
                                    {
                                        string payModeVal = Convert.ToString(chkl_paid.Items[s].Value);
                                        int curColCnt = 0;
                                        double paiAmount = 0;
                                        int.TryParse(Convert.ToString(htPayCol[payModeVal]), out curColCnt);
                                        #region dataview
                                        if (cblmem.Items[mem].Text.Trim() == "Staff")
                                        {
                                            if (ds.Tables[5].Rows.Count > 0)
                                            {
                                                ds.Tables[5].DefaultView.RowFilter = "memtype='" + cblmem.Items[mem].Value + "' and headerfk='" + chkl_studhed.Items[hd].Value + "' and paymode='" + chkl_paid.Items[s].Value + "'";
                                                dvmem = ds.Tables[5].DefaultView;
                                            }
                                            if (ds.Tables[6].Rows.Count > 0 && payModeVal == "2" || payModeVal == "3")
                                            {
                                                ds.Tables[6].DefaultView.RowFilter = "memtype='" + cblmem.Items[mem].Value + "' and headerfk='" + chkl_studhed.Items[hd].Value + "' and paymode='" + chkl_paid.Items[s].Value + "'";
                                                dvpay = ds.Tables[6].DefaultView;
                                                boolpaymode = true;
                                            }
                                        }
                                        else if (cblmem.Items[mem].Text.Trim() == "Vendor")
                                        {
                                            if (ds.Tables[7].Rows.Count > 0)
                                            {
                                                ds.Tables[7].DefaultView.RowFilter = "memtype='" + cblmem.Items[mem].Value + "' and headerfk='" + chkl_studhed.Items[hd].Value + "' and paymode='" + chkl_paid.Items[s].Value + "'";
                                                dvmem = ds.Tables[7].DefaultView;
                                            }
                                            if (ds.Tables[8].Rows.Count > 0 && payModeVal == "2" || payModeVal == "3")
                                            {
                                                ds.Tables[8].DefaultView.RowFilter = "memtype='" + cblmem.Items[mem].Value + "' and headerfk='" + chkl_studhed.Items[hd].Value + "' and paymode='" + chkl_paid.Items[s].Value + "'";
                                                dvpay = ds.Tables[8].DefaultView;
                                                boolpaymode = true;
                                            }
                                        }
                                        else if (cblmem.Items[mem].Text.Trim() == "Others")
                                        {
                                            if (ds.Tables[9].Rows.Count > 0)
                                            {
                                                ds.Tables[9].DefaultView.RowFilter = "memtype='" + cblmem.Items[mem].Value + "' and headerfk='" + chkl_studhed.Items[hd].Value + "' and paymode='" + chkl_paid.Items[s].Value + "'";
                                                dvmem = ds.Tables[9].DefaultView;
                                            }
                                            if (ds.Tables[10].Rows.Count > 0 && payModeVal == "2" || payModeVal == "3")
                                            {
                                                ds.Tables[10].DefaultView.RowFilter = "memtype='" + cblmem.Items[mem].Value + "' and headerfk='" + chkl_studhed.Items[hd].Value + "' and paymode='" + chkl_paid.Items[s].Value + "'";
                                                dvpay = ds.Tables[10].DefaultView;
                                                boolpaymode = true;
                                            }
                                        }
                                        #endregion
                                        if (dvmem.Count > 0)
                                        {
                                            // rowCnt++;

                                            if (!membool)
                                            {
                                                spreadDet.Sheets[0].Rows.Count++;
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = cblmem.Items[mem].Text;
                                                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, spreadDet.Sheets[0].ColumnCount - 1);
                                                // spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Gray;
                                                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                                                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                                                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                                                membool = true;
                                            }
                                            if (boolColumn)
                                            {
                                                spreadDet.Sheets[0].Rows.Count++;
                                                height += 15;
                                                rowCnt++;
                                                boolpay = true;
                                                boolyear = true;

                                            }
                                            boolColumn = false;
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(rowCnt);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(cblmem.Items[mem].Text);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(chkl_studhed.Items[hd].Text);
                                            double.TryParse(Convert.ToString(dvmem[0]["debit"]), out paiAmount);
                                            FnltotAmount += paiAmount;
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
                                            //cashier total
                                            string cashVal = string.Empty; ;
                                            cashVal = chkl_studhed.Items[hd].Text + "$" + Convert.ToDouble(curColCnt);
                                            if (!cashTot.ContainsKey(cashVal))
                                            {
                                                cashTot.Add(cashVal, paiAmount);
                                            }
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(cashTot[cashVal]), out amount);
                                                amount += paiAmount;
                                                cashTot.Remove(cashVal);
                                                cashTot.Add(cashVal, amount);
                                            }
                                            if (!headname.ContainsKey(chkl_studhed.Items[hd].Text))
                                                headname.Add(chkl_studhed.Items[hd].Text, chkl_studhed.Items[hd].Value);
                                            if (boolpaymode)
                                            {
                                                double clrAmount = 0;
                                                if (dvpay.Count > 0)
                                                    double.TryParse(Convert.ToString(dvpay[0]["debit"]), out clrAmount);

                                                if (paiAmount != 0)
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(paiAmount + "[" + clrAmount + "]");
                                                else
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = "-";
                                            }
                                            else
                                            {
                                                if (paiAmount != 0)
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(paiAmount);
                                                else
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = "-";
                                            }
                                            rowColor(payModeVal, curColCnt, spreadDet, spreadDet.Sheets[0].RowCount - 1);
                                        }
                                    }

                                }
                                if (boolpay)
                                {
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(FnltotAmount);
                                    if (!grandtotal.ContainsKey(spreadDet.Sheets[0].ColumnCount - 1))
                                        grandtotal.Add(spreadDet.Sheets[0].ColumnCount - 1, Convert.ToString(FnltotAmount));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(grandtotal[spreadDet.Sheets[0].ColumnCount - 1]), out amount);
                                        amount += FnltotAmount;
                                        grandtotal.Remove(spreadDet.Sheets[0].ColumnCount - 1);
                                        grandtotal.Add(spreadDet.Sheets[0].ColumnCount - 1, Convert.ToString(amount));
                                    }
                                    // FnltotAmount = 0;
                                }
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region ledger
                        for (int hd = 0; hd < chkl_studled.Items.Count; hd++)
                        {
                            double FnltotAmount = 0;
                            bool boolpay = false;
                            boolColumn = true;
                            if (chkl_studled.Items[hd].Selected)
                            {
                                for (int s = 0; s < chkl_paid.Items.Count; s++)
                                {
                                    bool boolpaymode = false;
                                    if (chkl_paid.Items[s].Selected == true)
                                    {
                                        string payModeVal = Convert.ToString(chkl_paid.Items[s].Value);
                                        int curColCnt = 0;
                                        double paiAmount = 0;
                                        int.TryParse(Convert.ToString(htPayCol[payModeVal]), out curColCnt);
                                        #region dataview
                                        if (cblmem.Items[mem].Text.Trim() == "Staff")
                                        {
                                            if (ds.Tables[5].Rows.Count > 0)
                                            {
                                                ds.Tables[5].DefaultView.RowFilter = "memtype='" + cblmem.Items[mem].Value + "' and ledgerfk='" + chkl_studled.Items[hd].Value + "' and paymode='" + chkl_paid.Items[s].Value + "'";
                                                dvmem = ds.Tables[5].DefaultView;
                                            }
                                            if (ds.Tables[6].Rows.Count > 0 && payModeVal == "2" || payModeVal == "3")
                                            {
                                                ds.Tables[6].DefaultView.RowFilter = "memtype='" + cblmem.Items[mem].Value + "' and ledgerfk='" + chkl_studled.Items[hd].Value + "' and paymode='" + chkl_paid.Items[s].Value + "'";
                                                dvpay = ds.Tables[6].DefaultView;
                                                boolpaymode = true;
                                            }
                                        }
                                        else if (cblmem.Items[mem].Text.Trim() == "Vendor")
                                        {
                                            if (ds.Tables[7].Rows.Count > 0)
                                            {
                                                ds.Tables[7].DefaultView.RowFilter = "memtype='" + cblmem.Items[mem].Value + "' and ledgerfk='" + chkl_studled.Items[hd].Value + "' and paymode='" + chkl_paid.Items[s].Value + "'";
                                                dvmem = ds.Tables[7].DefaultView;
                                            }
                                            if (ds.Tables[8].Rows.Count > 0 && payModeVal == "2" || payModeVal == "3")
                                            {
                                                ds.Tables[8].DefaultView.RowFilter = "memtype='" + cblmem.Items[mem].Value + "' and ledgerfk='" + chkl_studled.Items[hd].Value + "' and paymode='" + chkl_paid.Items[s].Value + "'";
                                                dvpay = ds.Tables[8].DefaultView;
                                                boolpaymode = true;
                                            }
                                        }
                                        else if (cblmem.Items[mem].Text.Trim() == "Others")
                                        {
                                            if (ds.Tables[9].Rows.Count > 0)
                                            {
                                                ds.Tables[9].DefaultView.RowFilter = "memtype='" + cblmem.Items[mem].Value + "' and ledgerfk='" + chkl_studled.Items[hd].Value + "' and paymode='" + chkl_paid.Items[s].Value + "'";
                                                dvmem = ds.Tables[9].DefaultView;
                                            }
                                            if (ds.Tables[10].Rows.Count > 0 && payModeVal == "2" || payModeVal == "3")
                                            {
                                                ds.Tables[10].DefaultView.RowFilter = "memtype='" + cblmem.Items[mem].Value + "' and ledgerfk='" + chkl_studled.Items[hd].Value + "' and paymode='" + chkl_paid.Items[s].Value + "'";
                                                dvpay = ds.Tables[10].DefaultView;
                                                boolpaymode = true;
                                            }
                                        }
                                        #endregion
                                        if (dvmem.Count > 0)
                                        {
                                            // rowCnt++;
                                            boolpay = true;
                                            boolyear = true;

                                            if (!membool)
                                            {
                                                spreadDet.Sheets[0].Rows.Count++;
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = cblmem.Items[mem].Text;
                                                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, spreadDet.Sheets[0].ColumnCount - 1);
                                                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.LightBlue;
                                                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                                                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                                                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                                                membool = true;
                                            }
                                            if (boolColumn)
                                            {
                                                spreadDet.Sheets[0].Rows.Count++;
                                                height += 15;
                                                rowCnt++;
                                            }
                                            boolColumn = false;
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(rowCnt);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(cblmem.Items[mem].Text);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(chkl_studled.Items[hd].Text);
                                            double.TryParse(Convert.ToString(dvmem[0]["debit"]), out paiAmount);
                                            FnltotAmount += paiAmount;
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
                                            //cashier total
                                            string cashVal = string.Empty; ;
                                            cashVal = chkl_studled.Items[hd].Text + "$" + Convert.ToDouble(curColCnt);
                                            if (!cashTot.ContainsKey(cashVal))
                                            {
                                                cashTot.Add(cashVal, paiAmount);
                                            }
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(cashTot[cashVal]), out amount);
                                                amount += paiAmount;
                                                cashTot.Remove(cashVal);
                                                cashTot.Add(cashVal, amount);
                                            }
                                            if (!headname.ContainsKey(chkl_studled.Items[hd].Text))
                                                headname.Add(chkl_studled.Items[hd].Text, chkl_studled.Items[hd].Value);
                                            if (boolpaymode)
                                            {
                                                double clrAmount = 0;
                                                if (dvpay.Count > 0)
                                                    double.TryParse(Convert.ToString(dvpay[0]["debit"]), out clrAmount);

                                                if (paiAmount != 0)
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(paiAmount + "[" + clrAmount + "]");
                                                else
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = "-";
                                            }
                                            else
                                            {
                                                if (paiAmount != 0)
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(paiAmount);
                                                else
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = "-";
                                            }
                                            rowColor(payModeVal, curColCnt, spreadDet, spreadDet.Sheets[0].RowCount - 1);
                                        }
                                    }
                                }
                                if (boolpay)
                                {
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(FnltotAmount);
                                    if (!grandtotal.ContainsKey(spreadDet.Sheets[0].ColumnCount - 1))
                                        grandtotal.Add(spreadDet.Sheets[0].ColumnCount - 1, Convert.ToString(FnltotAmount));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(grandtotal[spreadDet.Sheets[0].ColumnCount - 1]), out amount);
                                        amount += FnltotAmount;
                                        grandtotal.Remove(spreadDet.Sheets[0].ColumnCount - 1);
                                        grandtotal.Add(spreadDet.Sheets[0].ColumnCount - 1, Convert.ToString(amount));
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
                if (boolyear)
                {
                    #region Total
                    // spreadDet.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
                    spreadDet.Sheets[0].Rows.Count++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Total";
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.White;
                    double grandvalue = 0;
                    for (int j = 4; j < spreadDet.Sheets[0].ColumnCount; j++)
                    {
                        double.TryParse(Convert.ToString(grandtotal[j]), out grandvalue);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalue);
                        if (!total.ContainsKey(spreadDet.Sheets[0].ColumnCount - 1))
                            total.Add(j, Convert.ToString(grandvalue));
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(total[j]), out amount);
                            amount += grandvalue;
                            total.Remove(j);
                            total.Add(j, Convert.ToString(amount));
                        }
                    }
                    grandtotal.Clear();
                    boolGrand = true;
                    #endregion
                }
                spreadDet.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            }
            #endregion

            if (boolGrand)
            {
                #region grandtot
                // spreadDet.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
                spreadDet.Sheets[0].Rows.Count++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                double grandvalues = 0;
                for (int j = 4; j < spreadDet.Sheets[0].ColumnCount; j++)
                {
                    double.TryParse(Convert.ToString(total[j]), out grandvalues);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                }
                #endregion

                #region cashier details
                //cashier details                   
                spreadDet.Sheets[0].Rows.Count++;
                string name = "";
                if (rbtype.SelectedIndex == 0)
                    name = lblheader.Text;
                else
                    name = lblledger.Text;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 1].Text = name + " Name";
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 1, 1, 3);
                int rowcnt = 0;
                Hashtable cashfnltot = new Hashtable();
                foreach (KeyValuePair<string, string> hdname in headname)
                {
                    double temptot = 0;
                    spreadDet.Sheets[0].Rows.Count++;
                    bool tempbool = false;
                    for (int j = 3; j < spreadDet.Sheets[0].ColumnCount - 1; j++)
                    {
                        string strval = Convert.ToString(hdname.Key + "$" + j);
                        if (cashTot.ContainsKey(strval))
                        {
                            double amt = 0;
                            double.TryParse(Convert.ToString(cashTot[strval]), out amt);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(amt);
                            temptot += amt;
                            tempbool = true;
                            if (!cashfnltot.ContainsKey(j))
                                cashfnltot.Add(j, Convert.ToString(amt));
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(cashfnltot[j]), out amount);
                                amount += amt;
                                cashfnltot.Remove(j);
                                cashfnltot.Add(j, Convert.ToString(amount));
                            }
                        }
                        else if (j == 3)
                        {
                            rowcnt++;
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 1].Text = Convert.ToString(hdname.Key);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = Convert.ToString(rowcnt);
                            spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 1, 1, 3);
                        }
                        string payModeVal = Convert.ToString(spreadDet.Sheets[0].ColumnHeader.Cells[1, j].Tag);
                        rowColor(payModeVal, j, spreadDet, spreadDet.Sheets[0].RowCount - 1);
                    }
                    if (tempbool)
                    {
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, spreadDet.Sheets[0].Columns.Count - 1].Text = Convert.ToString(temptot);
                        if (!cashfnltot.ContainsKey(spreadDet.Sheets[0].Columns.Count - 1))
                            cashfnltot.Add(spreadDet.Sheets[0].Columns.Count - 1, Convert.ToString(temptot));
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(cashfnltot[spreadDet.Sheets[0].Columns.Count - 1]), out amount);
                            amount += temptot;
                            cashfnltot.Remove(spreadDet.Sheets[0].Columns.Count - 1);
                            cashfnltot.Add(spreadDet.Sheets[0].Columns.Count - 1, Convert.ToString(amount));
                        }
                    }
                }
                //cashier total
                if (cashfnltot.Count > 0)
                {
                    spreadDet.Sheets[0].Rows.Count++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Cashier wise Total";
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                    double gdvalues = 0;
                    for (int j = 4; j < spreadDet.Sheets[0].ColumnCount; j++)
                    {
                        double.TryParse(Convert.ToString(cashfnltot[j]), out gdvalues);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(gdvalues);
                    }
                }
                if (!receptCh)
                {
                    #region receipt no details

                    spreadDet.Sheets[0].Rows.Count++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Bill Particulars";
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 4);

                    spreadDet.Sheets[0].Rows.Count++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "1";
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 1].Text = "Starting Receipt No";
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                    if (ds.Tables[2].Rows.Count > 0)
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].Text = Convert.ToString(ds.Tables[2].Rows[0]["transcode"]);
                    // spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].BackColor = Color.Olive;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 1, 1, 2);

                    spreadDet.Sheets[0].Rows.Count++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "2";
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 1].Text = "Ending Receipt No";
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                    if (ds.Tables[3].Rows.Count > 0)
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].Text = Convert.ToString(ds.Tables[3].Rows[0]["transcode"]);
                    // spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].BackColor = Color.Cyan;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 1, 1, 2);

                    spreadDet.Sheets[0].Rows.Count++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "3";
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 1].Text = "Cancellation  Receipt No";
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 1, 1, 2);
                    if (ds.Tables[4].Rows.Count > 0)
                    {
                        string rcptno = string.Empty;
                        int cll = 0;
                        bool boolrow = false;
                        bool ck = false;
                        for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                        {
                            cll++;
                            if (rcptno == string.Empty)
                                rcptno = Convert.ToString(ds.Tables[4].Rows[i]["transcode"]);
                            else
                                rcptno += "," + Convert.ToString(ds.Tables[4].Rows[i]["transcode"]);
                            if (cll == 6)
                            {
                                cll = 0;
                                if (boolrow)
                                    spreadDet.Sheets[0].Rows.Count++;
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].Text = Convert.ToString(rcptno);
                                int colcnt = chkl_paid.Items.Count + 1;
                                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 4, 1, colcnt);
                                rcptno = string.Empty;
                                boolrow = true;
                                ck = true;
                            }
                        }
                        if (!ck)
                        {
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].Text = Convert.ToString(rcptno);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                            //   spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].BackColor = Color.RoyalBlue;
                            int colcnt = chkl_paid.Items.Count + 1;
                            spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 4, 1, colcnt);
                        }
                    }
                    #endregion
                }
                else
                {
                    #region receipt no details

                    spreadDet.Sheets[0].Rows.Count++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Bill Particulars";
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);

                    //  spreadDet.Sheets[0].Rows.Count++;
                    // spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "1";
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 2].Text = "Starting No";
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 3].Text = "Ending No";
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].Text = "Cancel No";
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                    int rowCnts = 0;
                    Dictionary<string, string> headerName = getHeaderName();
                    foreach (KeyValuePair<string, string> hdname in receiptSet)
                    {
                        rowCnts++;
                        spreadDet.Sheets[0].Rows.Count++;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = Convert.ToString(rowCnts);
                        string hdName = string.Empty;
                        string[] splhd = hdname.Value.Split(',');
                        if (splhd.Length > 0)
                        {
                            foreach (string hdFK in splhd)
                            {
                                string fk = Convert.ToString(hdFK.TrimStart('\'').TrimEnd('\''));
                                if (headerName.ContainsKey(fk))
                                {
                                    if (hdName == string.Empty)
                                        hdName = Convert.ToString(headerName[fk]);
                                    else
                                        hdName += "," + Convert.ToString(headerName[fk]);
                                }
                            }
                        }
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 1].Text = hdName;
                        spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                        spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                        bool rcptbind = false;
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            ds.Tables[2].DefaultView.RowFilter = "headerfk in('" + hdname.Value + "')";
                            DataView dvhd = ds.Tables[2].DefaultView;
                            string hdstrname = string.Empty;
                            DataTable dtval = new DataTable();
                            if (dvhd.Count > 0)
                            {
                                dtval = dvhd.ToTable();
                                DataTable dtck = dtval.AsEnumerable().Take(1).CopyToDataTable();
                                hdstrname = Convert.ToString(dtck.Rows[0]["transcode"]);
                                rcptbind = true;
                            }
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 2].Text = hdstrname;
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        }

                        if (ds.Tables[3].Rows.Count > 0)
                        {
                            ds.Tables[3].DefaultView.RowFilter = "headerfk in('" + hdname.Value + "')";
                            DataView dvhd = ds.Tables[3].DefaultView;
                            string hdstrname = string.Empty;
                            DataTable dtval = new DataTable();
                            if (dvhd.Count > 0)
                            {
                                dtval = dvhd.ToTable();
                                DataTable dtck = dtval.AsEnumerable().Take(1).CopyToDataTable();
                                hdstrname = Convert.ToString(dtck.Rows[0]["transcode"]);
                                rcptbind = true;
                            }
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 3].Text = hdstrname;
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        }
                        if (ds.Tables[4].Rows.Count > 0)
                        {
                            ds.Tables[4].DefaultView.RowFilter = "headerfk in('" + hdname.Value + "')";
                            DataView dvhd = ds.Tables[4].DefaultView;
                            if (dvhd.Count > 0)
                            {
                                string rcptno = string.Empty;
                                int cll = 0;
                                bool boolrow = false;
                                bool ck = false;
                                for (int i = 0; i < dvhd.Count; i++)
                                {
                                    cll++;
                                    if (rcptno == string.Empty)
                                        rcptno = Convert.ToString(dvhd[i]["transcode"]);
                                    else
                                        rcptno += "," + Convert.ToString(dvhd[i]["transcode"]);
                                    if (cll == 6)
                                    {
                                        cll = 0;
                                        if (boolrow)
                                            spreadDet.Sheets[0].Rows.Count++;
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].Text = Convert.ToString(rcptno);
                                        int colcnt = chkl_paid.Items.Count + 1;
                                        spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 4, 1, colcnt);
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                        rcptno = string.Empty;
                                        boolrow = true;
                                        ck = true;
                                        rcptbind = true;
                                    }
                                }
                                if (!ck)
                                {
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].Text = Convert.ToString(rcptno);
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                    //  spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].BackColor = Color.RoyalBlue;
                                    int colcnt = chkl_paid.Items.Count + 1;
                                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 4, 1, colcnt);
                                }
                            }
                        }
                    }
                    #endregion
                }

                #endregion

                payModeLabels(htPayCol);
                spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
                lblvalidation1.Text = "";
                txtexcelname.Text = "";
                spreadDet.Visible = true;
                print.Visible = true;
                spreadDet.Height = height;
                spreadDet.SaveChanges();
            }
        }
        catch { }
    }
    protected void SpreadLoadDetailsOthers(DataSet ds)
    {
        try
        {
            #region design
            bool receptCh = receiptNoSetting();
            spreadDet.Sheets[0].RowCount = 0;
            spreadDet.Sheets[0].ColumnCount = 0;
            spreadDet.CommandBar.Visible = false;
            spreadDet.Sheets[0].AutoPostBack = true;
            spreadDet.Sheets[0].ColumnHeader.RowCount = 2;
            spreadDet.Sheets[0].RowHeader.Visible = false;
            spreadDet.Sheets[0].ColumnCount = 3;
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

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Memtype";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            string strHdName = string.Empty;
            if (rbtype.SelectedIndex == 0)
                strHdName = lblheader.Text;
            else
                strHdName = lblledger.Text;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Text = strHdName;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;

            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);

            int checkva = 0;
            Hashtable htPayCol = new Hashtable();
            int pCnt = spreadDet.Sheets[0].ColumnCount++;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Text = "Paymode";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            for (int s = 0; s < chkl_paid.Items.Count; s++)
            {
                if (chkl_paid.Items[s].Selected == true)
                {
                    checkva++;
                    if (checkva > 1)
                        spreadDet.Sheets[0].ColumnCount++;
                    htPayCol.Add(Convert.ToString(chkl_paid.Items[s].Value), spreadDet.Sheets[0].ColumnCount - 1);
                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(chkl_paid.Items[s].Text);
                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(chkl_paid.Items[s].Value);
                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                }
            }
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, pCnt, 1, checkva);

            spreadDet.Sheets[0].ColumnCount++;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Text = "Total";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, spreadDet.Sheets[0].ColumnCount - 1, 2, 1);
            #endregion

            DataView dvmem = new DataView();
            DataView dvpay = new DataView();
            int rowCnt = 0;
            int height = 0;
            bool boolGrand = false;
            Hashtable grandtotal = new Hashtable();
            Hashtable total = new Hashtable();
            Dictionary<string, double> cashTot = new Dictionary<string, double>();
            Dictionary<string, string> headname = new Dictionary<string, string>();
            for (int mem = 0; mem < cblmem.Items.Count; mem++)
            {
                bool membool = false;
                bool boolyear = false;
                if (cblmem.Items[mem].Selected)
                {
                    if (rbtype.SelectedIndex == 0)
                    {
                        #region header
                        for (int hd = 0; hd < chkl_studhed.Items.Count; hd++)
                        {
                            double FnltotAmount = 0;
                            bool boolpay = false;
                            bool boolColumn = true;
                            if (chkl_studhed.Items[hd].Selected)
                            {
                                for (int s = 0; s < chkl_paid.Items.Count; s++)
                                {
                                    bool boolpaymode = false;
                                    if (chkl_paid.Items[s].Selected == true)
                                    {
                                        string payModeVal = Convert.ToString(chkl_paid.Items[s].Value);
                                        int curColCnt = 0;
                                        double paiAmount = 0;
                                        int.TryParse(Convert.ToString(htPayCol[payModeVal]), out curColCnt);
                                        #region dataview
                                        if (cblmem.Items[mem].Text.Trim() == "Staff")
                                        {
                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                ds.Tables[0].DefaultView.RowFilter = "memtype='" + cblmem.Items[mem].Value + "' and headerfk='" + chkl_studhed.Items[hd].Value + "' and paymode='" + chkl_paid.Items[s].Value + "'";
                                                dvmem = ds.Tables[0].DefaultView;
                                            }
                                            if (ds.Tables[1].Rows.Count > 0 && payModeVal == "2" || payModeVal == "3")
                                            {
                                                ds.Tables[1].DefaultView.RowFilter = "memtype='" + cblmem.Items[mem].Value + "' and headerfk='" + chkl_studhed.Items[hd].Value + "' and paymode='" + chkl_paid.Items[s].Value + "'";
                                                dvpay = ds.Tables[1].DefaultView;
                                                boolpaymode = true;
                                            }
                                        }
                                        else if (cblmem.Items[mem].Text.Trim() == "Vendor")
                                        {
                                            if (ds.Tables[2].Rows.Count > 0)
                                            {
                                                ds.Tables[2].DefaultView.RowFilter = "memtype='" + cblmem.Items[mem].Value + "' and headerfk='" + chkl_studhed.Items[hd].Value + "' and paymode='" + chkl_paid.Items[s].Value + "'";
                                                dvmem = ds.Tables[2].DefaultView;
                                            }
                                            if (ds.Tables[3].Rows.Count > 0 && payModeVal == "2" || payModeVal == "3")
                                            {
                                                ds.Tables[3].DefaultView.RowFilter = "memtype='" + cblmem.Items[mem].Value + "' and headerfk='" + chkl_studhed.Items[hd].Value + "' and paymode='" + chkl_paid.Items[s].Value + "'";
                                                dvpay = ds.Tables[3].DefaultView;
                                                boolpaymode = true;
                                            }
                                        }
                                        else if (cblmem.Items[mem].Text.Trim() == "Others")
                                        {
                                            if (ds.Tables[4].Rows.Count > 0)
                                            {
                                                ds.Tables[4].DefaultView.RowFilter = "memtype='" + cblmem.Items[mem].Value + "' and headerfk='" + chkl_studhed.Items[hd].Value + "' and paymode='" + chkl_paid.Items[s].Value + "'";
                                                dvmem = ds.Tables[4].DefaultView;
                                            }
                                            if (ds.Tables[5].Rows.Count > 0 && payModeVal == "2" || payModeVal == "3")
                                            {
                                                ds.Tables[5].DefaultView.RowFilter = "memtype='" + cblmem.Items[mem].Value + "' and headerfk='" + chkl_studhed.Items[hd].Value + "' and paymode='" + chkl_paid.Items[s].Value + "'";
                                                dvpay = ds.Tables[5].DefaultView;
                                                boolpaymode = true;
                                            }
                                        }
                                        #endregion
                                        if (dvmem.Count > 0)
                                        {
                                            // rowCnt++;

                                            if (!membool)
                                            {
                                                spreadDet.Sheets[0].Rows.Count++;
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = cblmem.Items[mem].Text;
                                                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, spreadDet.Sheets[0].ColumnCount - 1);
                                                // spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Gray;
                                                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                                                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                                                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                                                membool = true;
                                            }
                                            if (boolColumn)
                                            {
                                                spreadDet.Sheets[0].Rows.Count++;
                                                height += 15;
                                                rowCnt++;
                                                boolpay = true;
                                                boolyear = true;

                                            }
                                            boolColumn = false;
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(rowCnt);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(cblmem.Items[mem].Text);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(chkl_studhed.Items[hd].Text);
                                            double.TryParse(Convert.ToString(dvmem[0]["debit"]), out paiAmount);
                                            FnltotAmount += paiAmount;
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
                                            //cashier total
                                            string cashVal = string.Empty; ;
                                            cashVal = chkl_studhed.Items[hd].Text + "$" + Convert.ToDouble(curColCnt);
                                            if (!cashTot.ContainsKey(cashVal))
                                            {
                                                cashTot.Add(cashVal, paiAmount);
                                            }
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(cashTot[cashVal]), out amount);
                                                amount += paiAmount;
                                                cashTot.Remove(cashVal);
                                                cashTot.Add(cashVal, amount);
                                            }
                                            if (!headname.ContainsKey(chkl_studhed.Items[hd].Text))
                                                headname.Add(chkl_studhed.Items[hd].Text, chkl_studhed.Items[hd].Value);
                                            if (boolpaymode)
                                            {
                                                double clrAmount = 0;
                                                if (dvpay.Count > 0)
                                                    double.TryParse(Convert.ToString(dvpay[0]["debit"]), out clrAmount);

                                                if (paiAmount != 0)
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(paiAmount + "[" + clrAmount + "]");
                                                else
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = "-";
                                            }
                                            else
                                            {
                                                if (paiAmount != 0)
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(paiAmount);
                                                else
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = "-";
                                            }
                                            rowColor(payModeVal, curColCnt, spreadDet, spreadDet.Sheets[0].RowCount - 1);
                                        }
                                    }

                                }
                                if (boolpay)
                                {
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(FnltotAmount);
                                    if (!grandtotal.ContainsKey(spreadDet.Sheets[0].ColumnCount - 1))
                                        grandtotal.Add(spreadDet.Sheets[0].ColumnCount - 1, Convert.ToString(FnltotAmount));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(grandtotal[spreadDet.Sheets[0].ColumnCount - 1]), out amount);
                                        amount += FnltotAmount;
                                        grandtotal.Remove(spreadDet.Sheets[0].ColumnCount - 1);
                                        grandtotal.Add(spreadDet.Sheets[0].ColumnCount - 1, Convert.ToString(amount));
                                    }
                                    // FnltotAmount = 0;
                                }
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region ledger
                        for (int hd = 0; hd < chkl_studled.Items.Count; hd++)
                        {
                            double FnltotAmount = 0;
                            bool boolpay = false;
                            bool boolColumn = true;
                            if (chkl_studled.Items[hd].Selected)
                            {
                                for (int s = 0; s < chkl_paid.Items.Count; s++)
                                {
                                    bool boolpaymode = false;
                                    if (chkl_paid.Items[s].Selected == true)
                                    {
                                        string payModeVal = Convert.ToString(chkl_paid.Items[s].Value);
                                        int curColCnt = 0;
                                        double paiAmount = 0;
                                        int.TryParse(Convert.ToString(htPayCol[payModeVal]), out curColCnt);
                                        #region dataview
                                        if (cblmem.Items[mem].Text.Trim() == "Staff")
                                        {
                                            if (ds.Tables[0].Rows.Count > 0)
                                            {
                                                ds.Tables[0].DefaultView.RowFilter = "memtype='" + cblmem.Items[mem].Value + "' and ledgerfk='" + chkl_studled.Items[hd].Value + "' and paymode='" + chkl_paid.Items[s].Value + "'";
                                                dvmem = ds.Tables[0].DefaultView;
                                            }
                                            if (ds.Tables[1].Rows.Count > 0 && payModeVal == "2" || payModeVal == "3")
                                            {
                                                ds.Tables[1].DefaultView.RowFilter = "memtype='" + cblmem.Items[mem].Value + "' and ledgerfk='" + chkl_studled.Items[hd].Value + "' and paymode='" + chkl_paid.Items[s].Value + "'";
                                                dvpay = ds.Tables[1].DefaultView;
                                                boolpaymode = true;
                                            }
                                        }
                                        else if (cblmem.Items[mem].Text.Trim() == "Vendor")
                                        {
                                            if (ds.Tables[2].Rows.Count > 0)
                                            {
                                                ds.Tables[2].DefaultView.RowFilter = "memtype='" + cblmem.Items[mem].Value + "' and ledgerfk='" + chkl_studled.Items[hd].Value + "' and paymode='" + chkl_paid.Items[s].Value + "'";
                                                dvmem = ds.Tables[2].DefaultView;
                                            }
                                            if (ds.Tables[3].Rows.Count > 0 && payModeVal == "2" || payModeVal == "3")
                                            {
                                                ds.Tables[3].DefaultView.RowFilter = "memtype='" + cblmem.Items[mem].Value + "' and ledgerfk='" + chkl_studled.Items[hd].Value + "' and paymode='" + chkl_paid.Items[s].Value + "'";
                                                dvpay = ds.Tables[3].DefaultView;
                                                boolpaymode = true;
                                            }
                                        }
                                        else if (cblmem.Items[mem].Text.Trim() == "Others")
                                        {
                                            if (ds.Tables[4].Rows.Count > 0)
                                            {
                                                ds.Tables[4].DefaultView.RowFilter = "memtype='" + cblmem.Items[mem].Value + "' and ledgerfk='" + chkl_studled.Items[hd].Value + "' and paymode='" + chkl_paid.Items[s].Value + "'";
                                                dvmem = ds.Tables[4].DefaultView;
                                            }
                                            if (ds.Tables[5].Rows.Count > 0 && payModeVal == "2" || payModeVal == "3")
                                            {
                                                ds.Tables[5].DefaultView.RowFilter = "memtype='" + cblmem.Items[mem].Value + "' and ledgerfk='" + chkl_studled.Items[hd].Value + "' and paymode='" + chkl_paid.Items[s].Value + "'";
                                                dvpay = ds.Tables[5].DefaultView;
                                                boolpaymode = true;
                                            }
                                        }
                                        #endregion
                                        if (dvmem.Count > 0)
                                        {
                                            // rowCnt++;
                                            boolpay = true;
                                            boolyear = true;

                                            if (!membool)
                                            {
                                                spreadDet.Sheets[0].Rows.Count++;
                                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = cblmem.Items[mem].Text;
                                                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, spreadDet.Sheets[0].ColumnCount - 1);
                                                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.LightBlue;
                                                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                                                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                                                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                                                membool = true;
                                            }
                                            if (boolColumn)
                                            {
                                                spreadDet.Sheets[0].Rows.Count++;
                                                height += 15;
                                                rowCnt++;
                                            }
                                            boolColumn = false;
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(rowCnt);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(cblmem.Items[mem].Text);
                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(chkl_studled.Items[hd].Text);
                                            double.TryParse(Convert.ToString(dvmem[0]["debit"]), out paiAmount);
                                            FnltotAmount += paiAmount;
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
                                            //cashier total
                                            string cashVal = string.Empty; ;
                                            cashVal = chkl_studled.Items[hd].Text + "$" + Convert.ToDouble(curColCnt);
                                            if (!cashTot.ContainsKey(cashVal))
                                            {
                                                cashTot.Add(cashVal, paiAmount);
                                            }
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(cashTot[cashVal]), out amount);
                                                amount += paiAmount;
                                                cashTot.Remove(cashVal);
                                                cashTot.Add(cashVal, amount);
                                            }
                                            if (!headname.ContainsKey(chkl_studled.Items[hd].Text))
                                                headname.Add(chkl_studled.Items[hd].Text, chkl_studled.Items[hd].Value);
                                            if (boolpaymode)
                                            {
                                                double clrAmount = 0;
                                                if (dvpay.Count > 0)
                                                    double.TryParse(Convert.ToString(dvpay[0]["debit"]), out clrAmount);

                                                if (paiAmount != 0)
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(paiAmount + "[" + clrAmount + "]");
                                                else
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = "-";
                                            }
                                            else
                                            {
                                                if (paiAmount != 0)
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(paiAmount);
                                                else
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = "-";
                                            }
                                            rowColor(payModeVal, curColCnt, spreadDet, spreadDet.Sheets[0].RowCount - 1);
                                        }
                                    }
                                }
                                if (boolpay)
                                {
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(FnltotAmount);
                                    if (!grandtotal.ContainsKey(spreadDet.Sheets[0].ColumnCount - 1))
                                        grandtotal.Add(spreadDet.Sheets[0].ColumnCount - 1, Convert.ToString(FnltotAmount));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(grandtotal[spreadDet.Sheets[0].ColumnCount - 1]), out amount);
                                        amount += FnltotAmount;
                                        grandtotal.Remove(spreadDet.Sheets[0].ColumnCount - 1);
                                        grandtotal.Add(spreadDet.Sheets[0].ColumnCount - 1, Convert.ToString(amount));
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
                if (boolyear)
                {
                    #region Total
                    // spreadDet.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
                    spreadDet.Sheets[0].Rows.Count++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Total";
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.White;
                    double grandvalue = 0;
                    for (int j = 3; j < spreadDet.Sheets[0].ColumnCount; j++)
                    {
                        double.TryParse(Convert.ToString(grandtotal[j]), out grandvalue);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalue);
                        if (!total.ContainsKey(spreadDet.Sheets[0].ColumnCount - 1))
                            total.Add(j, Convert.ToString(grandvalue));
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(total[j]), out amount);
                            amount += grandvalue;
                            total.Remove(j);
                            total.Add(j, Convert.ToString(amount));
                        }
                    }
                    grandtotal.Clear();
                    boolGrand = true;
                    #endregion
                }
                spreadDet.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            }
            if (boolGrand)
            {
                #region grandtot
                // spreadDet.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
                spreadDet.Sheets[0].Rows.Count++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                double grandvalues = 0;
                for (int j = 3; j < spreadDet.Sheets[0].ColumnCount; j++)
                {
                    double.TryParse(Convert.ToString(total[j]), out grandvalues);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                }
                #endregion

                #region cashier details
                //cashier details                   
                spreadDet.Sheets[0].Rows.Count++;
                string name = "";
                if (rbtype.SelectedIndex == 0)
                    name = lblheader.Text;
                else
                    name = lblledger.Text;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 1].Text = name + " Name";
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 1, 1, 3);
                int rowcnt = 0;
                Hashtable cashfnltot = new Hashtable();
                foreach (KeyValuePair<string, string> hdname in headname)
                {
                    double temptot = 0;
                    spreadDet.Sheets[0].Rows.Count++;
                    bool tempbool = false;
                    for (int j = 3; j < spreadDet.Sheets[0].ColumnCount - 1; j++)
                    {
                        string strval = Convert.ToString(hdname.Key + "$" + j);
                        if (cashTot.ContainsKey(strval))
                        {
                            double amt = 0;
                            double.TryParse(Convert.ToString(cashTot[strval]), out amt);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(amt);
                            temptot += amt;
                            tempbool = true;
                            if (!cashfnltot.ContainsKey(j))
                                cashfnltot.Add(j, Convert.ToString(amt));
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(cashfnltot[j]), out amount);
                                amount += amt;
                                cashfnltot.Remove(j);
                                cashfnltot.Add(j, Convert.ToString(amount));
                            }
                        }
                        if (j == 3)
                        {
                            rowcnt++;
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 1].Text = Convert.ToString(hdname.Key);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = Convert.ToString(rowcnt);
                            spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 1, 1, 2);
                        }
                        string payModeVal = Convert.ToString(spreadDet.Sheets[0].ColumnHeader.Cells[1, j].Tag);
                        rowColor(payModeVal, j, spreadDet, spreadDet.Sheets[0].RowCount - 1);
                    }
                    if (tempbool)
                    {
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, spreadDet.Sheets[0].Columns.Count - 1].Text = Convert.ToString(temptot);
                        if (!cashfnltot.ContainsKey(spreadDet.Sheets[0].Columns.Count - 1))
                            cashfnltot.Add(spreadDet.Sheets[0].Columns.Count - 1, Convert.ToString(temptot));
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(cashfnltot[spreadDet.Sheets[0].Columns.Count - 1]), out amount);
                            amount += temptot;
                            cashfnltot.Remove(spreadDet.Sheets[0].Columns.Count - 1);
                            cashfnltot.Add(spreadDet.Sheets[0].Columns.Count - 1, Convert.ToString(amount));
                        }
                    }
                }
                //cashier total
                if (cashfnltot.Count > 0)
                {
                    spreadDet.Sheets[0].Rows.Count++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Cashier wise Total";
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                    double gdvalues = 0;
                    for (int j = 3; j < spreadDet.Sheets[0].ColumnCount; j++)
                    {
                        double.TryParse(Convert.ToString(cashfnltot[j]), out gdvalues);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(gdvalues);
                    }
                }
                if (!receptCh)
                {
                    #region receipt no details

                    spreadDet.Sheets[0].Rows.Count++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Bill Particulars";
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 4);

                    spreadDet.Sheets[0].Rows.Count++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "1";
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 1].Text = "Starting Receipt No";
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                    if (ds.Tables[6].Rows.Count > 0)
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].Text = Convert.ToString(ds.Tables[6].Rows[0]["transcode"]);
                    // spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].BackColor = Color.Olive;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 1, 1, 2);

                    spreadDet.Sheets[0].Rows.Count++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "2";
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 1].Text = "Ending Receipt No";
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                    if (ds.Tables[7].Rows.Count > 0)
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].Text = Convert.ToString(ds.Tables[7].Rows[0]["transcode"]);
                    // spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].BackColor = Color.Cyan;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 1, 1, 2);

                    spreadDet.Sheets[0].Rows.Count++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "3";
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 1].Text = "Cancellation  Receipt No";
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 1, 1, 2);
                    if (ds.Tables[8].Rows.Count > 0)
                    {
                        string rcptno = string.Empty;
                        int cll = 0;
                        bool boolrow = false;
                        bool ck = false;
                        for (int i = 0; i < ds.Tables[8].Rows.Count; i++)
                        {
                            cll++;
                            if (rcptno == string.Empty)
                                rcptno = Convert.ToString(ds.Tables[8].Rows[i]["transcode"]);
                            else
                                rcptno += "," + Convert.ToString(ds.Tables[8].Rows[i]["transcode"]);
                            if (cll == 6)
                            {
                                cll = 0;
                                if (boolrow)
                                    spreadDet.Sheets[0].Rows.Count++;
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].Text = Convert.ToString(rcptno);
                                int colcnt = chkl_paid.Items.Count + 1;
                                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 4, 1, colcnt);
                                rcptno = string.Empty;
                                boolrow = true;
                                ck = true;
                            }
                        }
                        if (!ck)
                        {
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].Text = Convert.ToString(rcptno);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                            //   spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].BackColor = Color.RoyalBlue;
                            int colcnt = chkl_paid.Items.Count + 1;
                            spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 4, 1, colcnt);
                        }
                    }
                    #endregion
                }
                else
                {
                    #region receipt no details

                    spreadDet.Sheets[0].Rows.Count++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Bill Particulars";
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);

                    //  spreadDet.Sheets[0].Rows.Count++;
                    // spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "1";
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 2].Text = "Starting No";
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 3].Text = "Ending No";
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].Text = "Cancel No";
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                    int rowCnts = 0;
                    Dictionary<string, string> headerName = getHeaderName();
                    foreach (KeyValuePair<string, string> hdname in receiptSet)
                    {
                        rowCnts++;
                        spreadDet.Sheets[0].Rows.Count++;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = Convert.ToString(rowCnts);
                        string hdName = string.Empty;
                        string[] splhd = hdname.Value.Split(',');
                        if (splhd.Length > 0)
                        {
                            foreach (string hdFK in splhd)
                            {
                                string fk = Convert.ToString(hdFK.TrimStart('\'').TrimEnd('\''));
                                if (headerName.ContainsKey(fk))
                                {
                                    if (hdName == string.Empty)
                                        hdName = Convert.ToString(headerName[fk]);
                                    else
                                        hdName += "," + Convert.ToString(headerName[fk]);
                                }
                            }
                        }
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 1].Text = hdName;
                        spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                        spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                        bool rcptbind = false;
                        if (ds.Tables[6].Rows.Count > 0)
                        {
                            ds.Tables[6].DefaultView.RowFilter = "headerfk in('" + hdname.Value + "')";
                            DataView dvhd = ds.Tables[6].DefaultView;
                            string hdstrname = string.Empty;
                            DataTable dtval = new DataTable();
                            if (dvhd.Count > 0)
                            {
                                dtval = dvhd.ToTable();
                                DataTable dtck = dtval.AsEnumerable().Take(1).CopyToDataTable();
                                hdstrname = Convert.ToString(dtck.Rows[0]["transcode"]);
                                rcptbind = true;
                            }
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 2].Text = hdstrname;
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        }

                        if (ds.Tables[7].Rows.Count > 0)
                        {
                            ds.Tables[7].DefaultView.RowFilter = "headerfk in('" + hdname.Value + "')";
                            DataView dvhd = ds.Tables[7].DefaultView;
                            string hdstrname = string.Empty;
                            DataTable dtval = new DataTable();
                            if (dvhd.Count > 0)
                            {
                                dtval = dvhd.ToTable();
                                DataTable dtck = dtval.AsEnumerable().Take(1).CopyToDataTable();
                                hdstrname = Convert.ToString(dtck.Rows[0]["transcode"]);
                                rcptbind = true;
                            }
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 3].Text = hdstrname;
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        }
                        if (ds.Tables[8].Rows.Count > 0)
                        {
                            ds.Tables[8].DefaultView.RowFilter = "headerfk in('" + hdname.Value + "')";
                            DataView dvhd = ds.Tables[8].DefaultView;
                            if (dvhd.Count > 0)
                            {
                                string rcptno = string.Empty;
                                int cll = 0;
                                bool boolrow = false;
                                bool ck = false;
                                for (int i = 0; i < dvhd.Count; i++)
                                {
                                    cll++;
                                    if (rcptno == string.Empty)
                                        rcptno = Convert.ToString(dvhd[i]["transcode"]);
                                    else
                                        rcptno += "," + Convert.ToString(dvhd[i]["transcode"]);
                                    if (cll == 6)
                                    {
                                        cll = 0;
                                        if (boolrow)
                                            spreadDet.Sheets[0].Rows.Count++;
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].Text = Convert.ToString(rcptno);
                                        int colcnt = chkl_paid.Items.Count + 1;
                                        spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 4, 1, colcnt);
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                        rcptno = string.Empty;
                                        boolrow = true;
                                        ck = true;
                                        rcptbind = true;
                                    }
                                }
                                if (!ck)
                                {
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].Text = Convert.ToString(rcptno);
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                    //  spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].BackColor = Color.RoyalBlue;
                                    int colcnt = chkl_paid.Items.Count + 1;
                                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 4, 1, colcnt);
                                }
                            }
                        }
                    }
                    #endregion
                }

                #endregion
                payModeLabels(htPayCol);
                spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
                lblvalidation1.Text = "";
                txtexcelname.Text = "";
                spreadDet.Visible = true;
                print.Visible = true;
                spreadDet.Height = height;
                spreadDet.SaveChanges();
            }
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

    protected void rowColor(string payModeVal, int curColCnt, FarPoint.Web.Spread.FpSpread spreadDet, int rowcnt)
    {
        if (payModeVal == "1")
            spreadDet.Sheets[0].Cells[rowcnt, curColCnt].BackColor = ColorTranslator.FromHtml("#F08080");
        else if (payModeVal == "2")
            spreadDet.Sheets[0].Cells[rowcnt, curColCnt].BackColor = ColorTranslator.FromHtml("#D3D3D3");
        else if (payModeVal == "3")
            spreadDet.Sheets[0].Cells[rowcnt, curColCnt].BackColor = ColorTranslator.FromHtml("#FFA500");
        else if (payModeVal == "4")
            spreadDet.Sheets[0].Cells[rowcnt, curColCnt].BackColor = ColorTranslator.FromHtml("#90EE90");
        else if (payModeVal == "5")
            spreadDet.Sheets[0].Cells[rowcnt, curColCnt].BackColor = ColorTranslator.FromHtml("#FAFAD2");
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
            degreedetails = "Daily Fees Structure Report" + '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            pagename = "DailyFeesCollectionReport.aspx";
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
        double.TryParse(Convert.ToString(d2.GetFunction("select  value from Master_Settings where settings='schoolorcollege' and usercode='" + usercode + "'")), out getVal);
        return getVal;
    }
    //receipt no setting checked
    protected bool receiptNoSetting()
    {
        receiptSet.Clear();
        bool check = false;
        int isHeaderwise = 0;
        try
        {
            string HeaderwiseQ = "select LinkValue from New_InsSettings where LinkName='HeaderWiseChallanorReceipt' and user_code ='" + usercode + "' and college_code ='" + collegecode + "' ";
            isHeaderwise = Convert.ToInt32(d2.GetFunction(HeaderwiseQ).Trim());
            if (isHeaderwise == 1)
                check = true;
        }
        catch { isHeaderwise = 0; }
        if (isHeaderwise > 0)
        {
            try
            {
                string finYearid = d2.getCurrentFinanceYear(usercode, collegecode);
                string headerid = Convert.ToString(getCblSelectedValue(chkl_studhed));
                if (!string.IsNullOrEmpty(headerid) && !string.IsNullOrEmpty(finYearid))
                {
                    // string Selq = " select distinct headersettingpk from fm_headerfincodesettings f,fm_headerfincodesettingsdet ft where ft.headersettingfk=f.headersettingpk  and finyearfk='1'";
                    string Selq = "  select distinct headersettingpk,headerfk from fm_headerfincodesettings f,fm_headerfincodesettingsdet ft where ft.headersettingfk=f.headersettingpk and headerfk in('" + headerid + "') and finyearfk='" + finYearid + "' and isnull(onlineRec,'')<>'1'";
                    DataSet dshd = d2.select_method_wo_parameter(Selq, "Text");
                    if (dshd.Tables.Count > 0 && dshd.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dshd.Tables[0].Rows.Count; i++)
                        {
                            string hdsettingPk = Convert.ToString(dshd.Tables[0].Rows[i]["headersettingpk"]);
                            if (!receiptSet.ContainsKey(hdsettingPk))
                                receiptSet.Add(hdsettingPk, Convert.ToString(dshd.Tables[0].Rows[i]["headerfk"]));
                            else
                            {
                                string temp = Convert.ToString(receiptSet[hdsettingPk]);
                                temp += "','" + Convert.ToString(dshd.Tables[0].Rows[i]["headerfk"]);
                                receiptSet.Remove(hdsettingPk);
                                receiptSet.Add(hdsettingPk, temp);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }
        return check;
    }

    protected Dictionary<string, string> getHeaderName()
    {
        Dictionary<string, string> headerName = new Dictionary<string, string>();
        for (int i = 0; i < chkl_studhed.Items.Count; i++)
        {
            if (chkl_studhed.Items[i].Selected)
            {
                headerName.Add(chkl_studhed.Items[i].Value, chkl_studhed.Items[i].Text);
            }
        }
        return headerName;
    }

    //added by sudhagar 17.02.2017
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
            tdmemtype.Visible = true;
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
    protected void rbltype_Selected(object sender, EventArgs e)
    {
        if (rbltype.SelectedIndex == 0)
        {
            tdmemtype.Visible = false;
            cbbeforAdm.Visible = true;
            txt_batch.Enabled = true;
            txt_degree.Enabled = true;
            txt_dept.Enabled = true;
            txt_sect.Enabled = true;
            txt_sem.Enabled = true;
            memtype();
        }
        else
        {
            memtype();
            cbbeforAdm.Visible = false;
            txt_batch.Enabled = false;
            txt_degree.Enabled = false;
            txt_dept.Enabled = false;
            txt_sect.Enabled = false;
            txt_sem.Enabled = false;
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

    //added by abarna 19.02.2018
    #region finuser year
    public void loadfinanceUser()
    {
        try
        {
            // collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            string finUser = "   select user_id,user_code from usermaster where fin_user='1' ";//and college_code in('" + collegecode + "')
            string getfin = d2.GetFunction("select fin_user from usermaster where fin_user='1' and user_code='" + usercode.Trim() + "' ");//and college_code in('" + collegecode + "')
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
}