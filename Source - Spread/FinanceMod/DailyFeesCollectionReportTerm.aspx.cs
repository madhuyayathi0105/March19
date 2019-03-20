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

public partial class DailyFeesCollectionReportTerm : System.Web.UI.Page
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
                loadheaderandledger();
                ledgerload();
                loadfinanceyear();
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_todate.Attributes.Add("readonly", "readonly");
                memtype();
                loadStudenttype();
                //printVisibleSettings();
                sphd.InnerText = lblsem.Text + "wise Daily Fees Collection Report";
                LoadIncludeSetting();
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
    //        string cbltext = string.Empty;
    //        string linkName = "Term";
    //        cbl_sem.Items.Clear();
    //        cbl_sem.Items.Add(new ListItem("Term 1", "0"));
    //        cbl_sem.Items.Add(new ListItem("Term 2", "1"));
    //        cbl_sem.Items.Add(new ListItem("Term 3", "2"));
    //        cbl_sem.Items.Add(new ListItem("Term 4", "3"));
    //        if (cbl_sem.Items.Count > 0)
    //        {
    //            for (int i = 0; i < cbl_sem.Items.Count; i++)
    //            {
    //                cbl_sem.Items[i].Selected = true;
    //                cbltext = Convert.ToString(cbl_sem.Items[i].Text);
    //            }
    //            if (cbl_sem.Items.Count == 1)
    //                txt_sem.Text = "" + linkName + "(" + cbltext + ")";
    //            else
    //                txt_sem.Text = "" + linkName + "(" + cbl_sem.Items.Count + ")";
    //            cb_sem.Checked = true;
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
        if (cbconcession.Checked)
        {
            int selCnt = 0;
            for (int row = 0; row < chkl_paid.Items.Count; row++)
            {
                if (chkl_paid.Items[row].Selected)
                {
                    selCnt++;
                }
            }
            if (selCnt < 3)
            {
                lblvalidation1.Text = string.Empty;
                txtexcelname.Text = string.Empty;
                spreadDet.Visible = false;
                print.Visible = false;
                divlabl.Visible = false;
                lbl_alert.Text = "You have selected concession option so atleast select 3 Paymode type!";
                imgdiv2.Visible = true;
                return;
            }
        }

        bool check = false;
        string fromdate = string.Empty;
        string todate = string.Empty;
        ds.Clear();
        if (checkSchoolSetting() != 0)
            ds = dsloadDetails(ref fromdate, ref todate);
        else
            ds = dsloadDetailsScl(ref fromdate, ref todate);


        //  ds = dsFilterValues(ds, ref  fromdate, ref  todate);
        // spreadLoadDetails(ds, ref fromdate, ref todate);

        if (rbltype.SelectedIndex == 0)
        {
            if (ds.Tables.Count > 0 && (ds.Tables[0].Rows.Count > 0 || ds.Tables[5].Rows.Count > 0 || ds.Tables[7].Rows.Count > 0 || ds.Tables[9].Rows.Count > 0))
            {
                if (checkSchoolSetting() != 0)
                    SpreadLoadDetails(ds);
                else
                    SpreadLoadDetailsScl(ds);
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
            // sem = Convert.ToString(getCblSelectedValue(cbl_sem));
            // sem = Convert.ToString(getCblSelectedText(cbl_sem));
            sec = Convert.ToString(getCblSelectedValue(cbl_sect));
            paid = Convert.ToString(getCblSelectedValue(chkl_paid));
            headervalue = Convert.ToString(getCblSelectedValue(chkl_studhed));
            ledgervalue = Convert.ToString(getCblSelectedValue(chkl_studled));
            Finyearvalue = Convert.ToString(getCblSelectedValue(chklsfyear));
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

            for (int se = 0; se < cbl_sem.Items.Count; se++)
            {
                if (cbl_sem.Items[se].Selected)
                {
                    if (sem == string.Empty)
                        sem = " and( t.textval like'%" + cbl_sem.Items[se].Text + "%'";
                    else
                        sem += " or t.textval like'%" + cbl_sem.Items[se].Text + "%'";
                }
            }
            if (!string.IsNullOrEmpty(sem))
                sem += ")";
            #endregion

            #region old Query
            if (rbltype.SelectedIndex == 0)
            {
                #region student

                SelQ = " select batch_year" + strtype + ",textval,paymode,sum(debit) as debit,isnull(FineFeecategory,'0')  as FineFeecategory from(";
                SelQ += " select r.batch_year" + strtype + ",t.textval,f.paymode,debit,isnull(FineFeecategory,'0') as FineFeecategory from ft_findailytransaction f,registration r,textvaltable t where f.app_no=r.app_no and f.feecategory=t.textcode and r.college_code=t.college_code and t.textcriteria='FEECA' " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') " + sem + " and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + strReg + " and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " and isnull(debit,'0')>0 ";
                ////  if (usBasedRights == true)
                SelQ += " and f.EntryUserCode in('" + usercode + "')";
                if (cbbeforAdm.Checked)
                {
                    SelQ += " union all select r.batch_year" + strtype + ",t.textval,f.paymode,debit,isnull(FineFeecategory,'0') as FineFeecategory from ft_findailytransaction f,applyn r,textvaltable t where f.app_no=r.app_no and f.feecategory=t.textcode and r.college_code=t.college_code and t.textcriteria='FEECA' " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') " + sem + "  and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "'  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " " + applynStr + "  and isnull(debit,'0')>0";
                    //if (usBasedRights == true)
                    SelQ += " and f.EntryUserCode in('" + usercode + "')";
                }
                SelQ += ") tbl group by batch_year" + strtype + ",textval,paymode,FineFeecategory order by batch_year,textval" + strtype + ",paymode asc";

                SelQ += " select batch_year" + strtype + ",textval,paymode,sum(debit) as debit,isnull(FineFeecategory,'0')  as FineFeecategory from(";
                SelQ += " select r.batch_year" + strtype + ",t.textval,f.paymode,debit,isnull(FineFeecategory,'0') as FineFeecategory from ft_findailytransaction f,registration r,textvaltable t where f.app_no=r.app_no and f.feecategory=t.textcode and r.college_code=t.college_code and t.textcriteria='FEECA' and ( isnull(f.IsDeposited,'0')='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1' and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') " + sem + " and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + strReg + " and f.Transdate between '" + fromdate + "' and '" + todate + "'   " + strFine + "  and isnull(debit,'0')>0 ";
                //if (usBasedRights == true)
                SelQ += " and f.EntryUserCode in('" + usercode + "')";
                if (cbbeforAdm.Checked)
                {
                    SelQ += " union all select r.batch_year" + strtype + ",t.textval,f.paymode,debit,isnull(FineFeecategory,'0') as FineFeecategory from ft_findailytransaction f,applyn r,textvaltable t where f.app_no=r.app_no and f.feecategory=t.textcode and r.college_code=t.college_code and t.textcriteria='FEECA'  and ( isnull(f.IsDeposited,'0')='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1' and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') " + sem + "  and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' and f.Transdate between '" + fromdate + "' and '" + todate + "'   " + strFine + " " + applynStr + " and isnull(debit,'0')>0 ";
                    //if (usBasedRights == true)
                    SelQ += " and f.EntryUserCode in('" + usercode + "')";
                }
                SelQ += ") tbl group by batch_year" + strtype + ",textval,paymode,FineFeecategory order by batch_year,textval,paymode" + strtype + " asc";
                #region receipt no
                //receipt no setting
                if (!receiptCheck)
                {
                    #region commom
                    SelQ += " select distinct top(1) f.transcode from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') " + strRecon + " and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " ";
                    if (memType != "")
                        SelQ += " and memtype in('1','" + memType + "')";
                    else
                        SelQ += " and memtype in('1')";
                    //if (usBasedRights == true)
                    SelQ += " and f.EntryUserCode in('" + usercode + "')";
                    SelQ += "  order by f.transcode asc  ";

                    SelQ += " select distinct top(1) f.transcode from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') " + strRecon + " and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + "  ";
                    if (memType != "")
                        SelQ += " and memtype in('1','" + memType + "')";
                    else
                        SelQ += " and memtype in('1')";
                    //if (usBasedRights == true)
                    SelQ += " and f.EntryUserCode in('" + usercode + "')";
                    SelQ += " order by f.transcode desc  ";

                    SelQ += " select distinct f.transcode from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "')  and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " and ISNULL(IsCanceled,'0')='1' ";
                    if (memType != "")
                        SelQ += " and memtype in('1','" + memType + "')";
                    else
                        SelQ += " and memtype in('1')";
                    //if (usBasedRights == true)
                    SelQ += " and f.EntryUserCode in('" + usercode + "')";
                    SelQ += " order by f.transcode asc  ";
                    #endregion
                }
                else
                {
                    #region headerwise
                    SelQ += " select distinct  f.transcode,f.headerfk from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') " + strRecon + " and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " ";
                    if (memType != "")
                        SelQ += " and memtype in('1','" + memType + "')";
                    else
                        SelQ += " and memtype in('1')";
                    //if (usBasedRights == true)
                    SelQ += " and f.EntryUserCode in('" + usercode + "')";
                    SelQ += "  order by f.transcode asc  ";

                    SelQ += " select distinct f.transcode,f.headerfk from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') " + strRecon + " and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + "";
                    if (memType != "")
                        SelQ += " and memtype in('1','" + memType + "')";
                    else
                        SelQ += " and memtype in('1')";
                    ////  if (usBasedRights == true)
                    SelQ += " and f.EntryUserCode in('" + usercode + "')";
                    SelQ += " order by f.transcode desc  ";

                    SelQ += " select distinct f.transcode,f.headerfk from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "')  and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " and ISNULL(IsCanceled,'0')='1' ";
                    if (memType != "")
                        SelQ += " and memtype in('1','" + memType + "')";
                    else
                        SelQ += " and memtype in('1')";
                    //if (usBasedRights == true)
                    SelQ += " and f.EntryUserCode in('" + usercode + "')";
                    SelQ += " order by f.transcode asc  ";
                    #endregion
                }
                #endregion


                #endregion

                #region others
                //staff
                SelQ += " select  SUM(debit) as debit ,MemType,f.paymode" + strtype + " from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and t.dept_code =h.dept_code and t.desig_code =d.desig_code and T.staff_code =s.staff_code and T.latestrec ='1' " + strRecon + " and d.collegeCode='" + collegecode + "' and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                //if (usBasedRights == true)
                SelQ += " and f.EntryUserCode in('" + usercode + "')";
                SelQ += " group by MemType,f.paymode" + strtype + "";
                //reconsilation
                SelQ += " select  SUM(debit) as debit ,MemType,f.paymode" + strtype + " from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and t.dept_code =h.dept_code and t.desig_code =d.desig_code and T.staff_code =s.staff_code and T.latestrec ='1' and ( isnull(f.IsDeposited,'0')='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1' and d.collegeCode='" + collegecode + "' and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                //if (usBasedRights == true)
                SelQ += " and f.EntryUserCode in('" + usercode + "')";
                SelQ += " group by MemType,f.paymode" + strtype + "";

                //vendor
                SelQ += "  SELECT SUM(debit) as debit ,MemType,f.paymode" + strtype + "  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1' " + strRecon + "  and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                ////  if (usBasedRights == true)
                SelQ += " and f.EntryUserCode in('" + usercode + "')";
                SelQ += " group by MemType,f.paymode" + strtype + "";
                //reconsilation
                SelQ += "  SELECT SUM(debit) as debit ,MemType,f.paymode" + strtype + "  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1' and ( isnull(f.IsDeposited,'0')='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1'  and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                //if (usBasedRights == true)
                SelQ += " and f.EntryUserCode in('" + usercode + "')";
                SelQ += " group by MemType,f.paymode" + strtype + "";

                //others
                SelQ += "  SELECT SUM(debit) as debit ,MemType,f.paymode" + strtype + " FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5' " + strRecon + " and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                ////  if (usBasedRights == true)
                SelQ += " and f.EntryUserCode in('" + usercode + "')";
                SelQ += " group by MemType,f.paymode" + strtype + "";

                SelQ += "  SELECT SUM(debit) as debit ,MemType,f.paymode" + strtype + " FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5' and ( isnull(f.IsDeposited,'0')='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1' and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                //  if (usBasedRights == true)
                SelQ += " and f.EntryUserCode in('" + usercode + "')";
                SelQ += " group by MemType,f.paymode" + strtype + "";

                #endregion

                #region month query
                SelQ += "   select distinct t.college_code,fd.feecategory,t.textval,isnull(monthcode,'0')as monthcode from Fee_degree_match fd,textvaltable t  where  t.college_code=fd.college_code  and t.textcode=fd.feecategory and t.textcriteria='FEECA' and t.college_code ='" + collegecode + "' " + sem + "";
                #endregion

                #region multiple cashierwise total query

                #region student

                SelQ += " select paymode,sum(debit) as debit,EntryUserCode from(";
                SelQ += " select f.paymode,debit,f.EntryUserCode from ft_findailytransaction f,registration r,textvaltable t where f.app_no=r.app_no and f.feecategory=t.textcode and r.college_code=t.college_code and t.textcriteria='FEECA' " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') " + sem + " and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + strReg + " and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " and isnull(debit,'0')>0 ";
                // //  if (usBasedRights == true)
                SelQ += " and f.EntryUserCode in('" + usercode + "')";
                if (cbbeforAdm.Checked)
                {
                    SelQ += " union all select f.paymode,debit,f.EntryUserCode from ft_findailytransaction f,applyn r,textvaltable t where f.app_no=r.app_no and f.feecategory=t.textcode and r.college_code=t.college_code and t.textcriteria='FEECA' " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') " + sem + "  and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "'  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " " + applynStr + "  and isnull(debit,'0')>0";
                    // //  if (usBasedRights == true)
                    SelQ += " and f.EntryUserCode in('" + usercode + "')";
                }
                SelQ += ") tbl group by paymode,EntryUserCode order by paymode asc";
                #endregion

                #region others
                SelQ += " select  SUM(debit) as debit,f.paymode,f.EntryUserCode from FT_FinDailyTransaction f where  HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "' and f.finyearfk in('" + Finyearvalue + "')";
                // //  if (usBasedRights == true)
                SelQ += " and f.EntryUserCode in('" + usercode + "')";
                SelQ += " group by f.paymode,f.EntryUserCode";
                ////reconsilation
                //SelQ += " select  SUM(debit) as debit ,f.paymode,f.EntryUserCode from FT_FinDailyTransaction f where ( isnull(f.IsDeposited,'0')='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1'  and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "' and f.finyearfk in('" + Finyearvalue + "')";
                //if (usBasedRights == true)
                //    SelQ += " and f.EntryUserCode in('" + usercode + "')";
                //SelQ += " group by f.paymode,f.EntryUserCode";
                #endregion

                SelQ += " select distinct EntryUserCode  from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " ";
                if (memType != "")
                    SelQ += " and memtype in('1','" + memType + "')";
                else
                    SelQ += " and memtype in('1')";
                // //  if (usBasedRights == true)
                SelQ += " and f.EntryUserCode in('" + usercode + "')";
                SelQ += "  order by f.EntryUserCode asc  ";


                #endregion

                //added by sudhagar 10.03.2017
                if (cbconcession.Checked)
                {
                    #region concession
                    SelQ += "  select batch_year,textval,sum(totalamount) as totalamount,sum(deductamout) as deductamout,deductreason,sum(paidamount) as paidamount,stud_name,roll_admit,degree_code,sections from( select r.batch_year,t.textval,totalamount,deductamout,deductreason,paidamount,stud_name,roll_admit,degree_code,sections from ft_findailytransaction f,registration r,textvaltable t,ft_feeallot ft where f.app_no=r.app_no and r.app_no=ft.app_no and f.app_no=ft.app_no and f.feecategory=t.textcode and ft.feecategory=t.textcode and r.college_code=t.college_code and t.textcriteria='FEECA'  " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') " + sem + " and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + strReg + " and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + "  and isnull(debit,'0')>0 and isnull(deductamout,'0')>0 and isnull(deductreason,'')<>''  and isnull(paidamount,'0')>0";
                    // //  if (usBasedRights == true)
                    SelQ += " and f.EntryUserCode in('" + usercode + "')";
                    if (cbbeforAdm.Checked)
                    {
                        SelQ += " union all select r.batch_year,t.textval,totalamount,deductamout,deductreason,paidamount,stud_name,''roll_admit,degree_code,''sections from ft_findailytransaction f,applyn r,textvaltable t,ft_feeallot ft where f.app_no=r.app_no and r.app_no=ft.app_no and f.app_no=ft.app_no and f.feecategory=t.textcode and ft.feecategory=t.textcode and r.college_code=t.college_code and t.textcriteria='FEECA'  " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') " + sem + "  and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "'  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " " + applynStr + "  and isnull(debit,'0')>0 and isnull(deductamout,'0')>0 and isnull(deductreason,'')<>''  and isnull(paidamount,'0')>0 ";
                        // //  if (usBasedRights == true)
                        SelQ += " and f.EntryUserCode in('" + usercode + "')";
                    }
                    SelQ += " ) tbl group by batch_year,textval,deductreason,stud_name,roll_admit,degree_code,sections order by batch_year,textval asc";
                    SelQ += "  select TextCode,TextVal from TextValTable where TextCriteria ='DedRe' and college_code ='" + collegecode + "'";
                    SelQ += " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename,(c.Course_Name +'-'+dt.dept_acronym)as dept_acronym from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode + "'";
                    #endregion
                }
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
                            // //  if (usBasedRights == true)
                            SelQ += " and f.EntryUserCode in('" + usercode + "')";
                            SelQ += " group by MemType,f.paymode" + strtype + "";
                            //reconsilation
                            SelQ += " select  SUM(debit) as debit ,MemType,f.paymode" + strtype + " from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and t.dept_code =h.dept_code and t.desig_code =d.desig_code and T.staff_code =s.staff_code and T.latestrec ='1' and ( isnull(f.IsDeposited,'0')='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1' and d.collegeCode='" + collegecode + "' and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                            // //  if (usBasedRights == true)
                            SelQ += " and f.EntryUserCode in('" + usercode + "')";
                            SelQ += " group by MemType,f.paymode" + strtype + "";
                        }
                        if (cblmem.Items[i].Text == "Vendor")
                        {
                            //vendor
                            SelQ += "  SELECT SUM(debit) as debit ,MemType,f.paymode" + strtype + "  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1' " + strRecon + "  and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                            // //  if (usBasedRights == true)
                            SelQ += " and f.EntryUserCode in('" + usercode + "')";
                            SelQ += " group by MemType,f.paymode" + strtype + "";
                            //reconsilation
                            SelQ += "  SELECT SUM(debit) as debit ,MemType,f.paymode" + strtype + "  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1' and ( isnull(f.IsDeposited,'0')='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1'  and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                            // //  if (usBasedRights == true)
                            SelQ += " and f.EntryUserCode in('" + usercode + "')";
                            SelQ += " group by MemType,f.paymode" + strtype + "";
                        }
                        if (cblmem.Items[i].Text == "Others")
                        {
                            //others
                            SelQ += "  SELECT SUM(debit) as debit ,MemType,f.paymode" + strtype + " FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5' " + strRecon + " and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                            // //  if (usBasedRights == true)
                            SelQ += " and f.EntryUserCode in('" + usercode + "')";
                            SelQ += " group by MemType,f.paymode" + strtype + "";

                            SelQ += "  SELECT SUM(debit) as debit ,MemType,f.paymode" + strtype + " FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5' and ( isnull(f.IsDeposited,'0')='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1' and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
                            // //  if (usBasedRights == true)
                            SelQ += " and f.EntryUserCode in('" + usercode + "')";
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
                    SelQ += " select distinct top(1) f.transcode from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') " + strRecon + " and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " and memtype in('" + memType + "') order by f.transcode asc  ";

                    SelQ += " select distinct top(1) f.transcode from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') " + strRecon + " and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " and memtype in('" + memType + "') order by f.transcode desc  ";

                    SelQ += " select distinct f.transcode from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "')  and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " and memtype in('" + memType + "') and ISNULL(IsCanceled,'0')='1' order by f.transcode asc  ";
                    #endregion
                }
                else
                {
                    #region headerwise
                    SelQ += " select distinct  f.transcode,f.headerfk from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') " + strRecon + " and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " and memtype in('" + memType + "') order by f.transcode asc  ";

                    SelQ += " select distinct f.transcode,f.headerfk from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') " + strRecon + " and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " and memtype in('" + memType + "') order by f.transcode desc  ";

                    SelQ += " select distinct f.transcode,f.headerfk from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "')  and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " and memtype in('" + memType + "') and ISNULL(IsCanceled,'0')='1' order by f.transcode asc  ";

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

    protected DataSet dsloadDetailsScl(ref string fromdate, ref string todate)
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
            // sem = Convert.ToString(getCblSelectedValue(cbl_sem));
            // sem = Convert.ToString(getCblSelectedText(cbl_sem));
            sec = Convert.ToString(getCblSelectedValue(cbl_sect));
            paid = Convert.ToString(getCblSelectedValue(chkl_paid));
            headervalue = Convert.ToString(getCblSelectedValue(chkl_studhed));
            ledgervalue = Convert.ToString(getCblSelectedValue(chkl_studled));
            Finyearvalue = Convert.ToString(getCblSelectedValue(chklsfyear));
            memType = Convert.ToString(getCblSelectedValue(cblmem));
            string studMode = Convert.ToString(getCblSelectedValue(cbl_type));
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

            for (int se = 0; se < cbl_sem.Items.Count; se++)
            {
                if (cbl_sem.Items[se].Selected)
                {
                    if (sem == string.Empty)
                        sem = " and( t.textval like'%" + cbl_sem.Items[se].Text + "%'";
                    else
                        sem += " or t.textval like'%" + cbl_sem.Items[se].Text + "%'";
                }
            }
            if (!string.IsNullOrEmpty(sem))
                sem += ")";
            strReg = getStudCategory();
            #endregion

            #region new query
            SelQ = " select batch_year" + strtype + ",textval,paymode,sum(debit) as debit,isnull(FineFeecategory,'0')  as FineFeecategory,finyearfk,actualfinyearfk,mode from(";
            SelQ += " select r.batch_year" + strtype + ",t.textval,f.paymode,debit,isnull(FineFeecategory,'0') as FineFeecategory,finyearfk,actualfinyearfk,mode from ft_findailytransaction f,registration r,textvaltable t where f.app_no=r.app_no and f.feecategory=t.textcode and r.college_code=t.college_code and t.textcriteria='FEECA' " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') " + sem + " and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + strReg + "  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " and isnull(debit,'0')>0 and r.mode in('" + studMode + "') and isnull(actualfinyearfk,'0')<>'0'";
            if (usBasedRights == true)
                SelQ += " and f.EntryUserCode in('" + usercode + "')";
            if (cbbeforAdm.Checked)
            {
                SelQ += " union all select r.batch_year" + strtype + ",t.textval,f.paymode,debit,isnull(FineFeecategory,'0') as FineFeecategory,finyearfk,actualfinyearfk,mode from ft_findailytransaction f,applyn r,textvaltable t where f.app_no=r.app_no and f.feecategory=t.textcode and r.college_code=t.college_code and t.textcriteria='FEECA' " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') " + sem + "  and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "'  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " " + applynStr + "  and isnull(debit,'0')>0 and r.mode in('" + studMode + "') and isnull(actualfinyearfk,'0')<>'0'";
                if (usBasedRights == true)
                    SelQ += " and f.EntryUserCode in('" + usercode + "')";
            }
            SelQ += ") tbl group by batch_year" + strtype + ",textval,paymode,FineFeecategory,finyearfk,actualfinyearfk,mode order by finyearfk,textval" + strtype + ",paymode asc";

            //before reconsilation
            SelQ += " select batch_year" + strtype + ",textval,paymode,sum(debit) as debit,isnull(FineFeecategory,'0')  as FineFeecategory,finyearfk,actualfinyearfk,mode from(";
            SelQ += " select r.batch_year" + strtype + ",t.textval,f.paymode,debit,isnull(FineFeecategory,'0') as FineFeecategory,finyearfk,actualfinyearfk,mode from ft_findailytransaction f,registration r,textvaltable t where f.app_no=r.app_no and f.feecategory=t.textcode and r.college_code=t.college_code and t.textcriteria='FEECA' and ( isnull(f.IsDeposited,'0')='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1' and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') " + sem + " and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + strReg + " and f.Transdate between '" + fromdate + "' and '" + todate + "'   " + strFine + "  and isnull(debit,'0')>0 and r.mode in('" + studMode + "') and isnull(actualfinyearfk,'0')<>'0'";
            if (usBasedRights == true)
                SelQ += " and f.EntryUserCode in('" + usercode + "')";
            if (cbbeforAdm.Checked)
            {
                SelQ += " union all select r.batch_year" + strtype + ",t.textval,f.paymode,debit,isnull(FineFeecategory,'0') as FineFeecategory,finyearfk,actualfinyearfk,mode from ft_findailytransaction f,applyn r,textvaltable t where f.app_no=r.app_no and f.feecategory=t.textcode and r.college_code=t.college_code and t.textcriteria='FEECA'  and ( isnull(f.IsDeposited,'0')='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1' and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') " + sem + "  and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' and f.Transdate between '" + fromdate + "' and '" + todate + "'   " + strFine + " " + applynStr + " and isnull(debit,'0')>0 and r.mode in('" + studMode + "') and isnull(actualfinyearfk,'0')<>'0'";
                if (usBasedRights == true)
                    SelQ += " and f.EntryUserCode in('" + usercode + "')";
            }
            SelQ += ") tbl group by batch_year" + strtype + ",textval,paymode,FineFeecategory,finyearfk,actualfinyearfk,mode order by batch_year,textval,paymode" + strtype + " asc";


            #region receipt no
            //receipt no setting
            if (!receiptCheck)
            {
                #region commom
                SelQ += " select distinct top(1) f.transcode from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') " + strRecon + " and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " ";
                if (memType != "")
                    SelQ += " and memtype in('1','" + memType + "')";
                else
                    SelQ += " and memtype in('1')";
                if (usBasedRights == true)
                    SelQ += " and f.EntryUserCode in('" + usercode + "')";
                SelQ += "  order by f.transcode asc  ";

                SelQ += " select distinct top(1) f.transcode from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') " + strRecon + " and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + "  ";
                if (memType != "")
                    SelQ += " and memtype in('1','" + memType + "')";
                else
                    SelQ += " and memtype in('1')";
                if (usBasedRights == true)
                    SelQ += " and f.EntryUserCode in('" + usercode + "')";
                SelQ += " order by f.transcode desc  ";

                SelQ += " select distinct f.transcode from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "')  and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " and ISNULL(IsCanceled,'0')='1' ";
                if (memType != "")
                    SelQ += " and memtype in('1','" + memType + "')";
                else
                    SelQ += " and memtype in('1')";
                if (usBasedRights == true)
                    SelQ += " and f.EntryUserCode in('" + usercode + "')";
                SelQ += " order by f.transcode asc  ";
                #endregion
            }
            else
            {
                #region headerwise
                SelQ += " select distinct  f.transcode,f.headerfk from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') " + strRecon + " and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " ";
                if (memType != "")
                    SelQ += " and memtype in('1','" + memType + "')";
                else
                    SelQ += " and memtype in('1')";
                if (usBasedRights == true)
                    SelQ += " and f.EntryUserCode in('" + usercode + "')";
                SelQ += "  order by f.transcode asc  ";

                SelQ += " select distinct f.transcode,f.headerfk from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') " + strRecon + " and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + "";
                if (memType != "")
                    SelQ += " and memtype in('1','" + memType + "')";
                else
                    SelQ += " and memtype in('1')";
                if (usBasedRights == true)
                    SelQ += " and f.EntryUserCode in('" + usercode + "')";
                SelQ += " order by f.transcode desc  ";

                SelQ += " select distinct f.transcode,f.headerfk from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "')  and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " and ISNULL(IsCanceled,'0')='1' ";
                if (memType != "")
                    SelQ += " and memtype in('1','" + memType + "')";
                else
                    SelQ += " and memtype in('1')";
                if (usBasedRights == true)
                    SelQ += " and f.EntryUserCode in('" + usercode + "')";
                SelQ += " order by f.transcode asc  ";
                #endregion
            }
            #endregion

            #region others
            //staff
            SelQ += " select  SUM(debit) as debit ,MemType,f.paymode" + strtype + " from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and t.dept_code =h.dept_code and t.desig_code =d.desig_code and T.staff_code =s.staff_code and T.latestrec ='1' " + strRecon + " and d.collegeCode='" + collegecode + "' and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
            if (usBasedRights == true)
                SelQ += " and f.EntryUserCode in('" + usercode + "')";
            SelQ += " group by MemType,f.paymode" + strtype + "";
            //reconsilation
            SelQ += " select  SUM(debit) as debit ,MemType,f.paymode" + strtype + " from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and t.dept_code =h.dept_code and t.desig_code =d.desig_code and T.staff_code =s.staff_code and T.latestrec ='1' and ( isnull(f.IsDeposited,'0')='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1' and d.collegeCode='" + collegecode + "' and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
            if (usBasedRights == true)
                SelQ += " and f.EntryUserCode in('" + usercode + "')";
            SelQ += " group by MemType,f.paymode" + strtype + "";

            //vendor
            SelQ += "  SELECT SUM(debit) as debit ,MemType,f.paymode" + strtype + "  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1' " + strRecon + "  and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
            if (usBasedRights == true)
                SelQ += " and f.EntryUserCode in('" + usercode + "')";
            SelQ += " group by MemType,f.paymode" + strtype + "";
            //reconsilation
            SelQ += "  SELECT SUM(debit) as debit ,MemType,f.paymode" + strtype + "  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1' and ( isnull(f.IsDeposited,'0')='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1'  and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
            if (usBasedRights == true)
                SelQ += " and f.EntryUserCode in('" + usercode + "')";
            SelQ += " group by MemType,f.paymode" + strtype + "";

            //others
            SelQ += "  SELECT SUM(debit) as debit ,MemType,f.paymode" + strtype + " FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5' " + strRecon + " and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
            if (usBasedRights == true)
                SelQ += " and f.EntryUserCode in('" + usercode + "')";
            SelQ += " group by MemType,f.paymode" + strtype + "";

            SelQ += "  SELECT SUM(debit) as debit ,MemType,f.paymode" + strtype + " FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5' and ( isnull(f.IsDeposited,'0')='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1' and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "'";
            if (usBasedRights == true)
                SelQ += " and f.EntryUserCode in('" + usercode + "')";
            SelQ += " group by MemType,f.paymode" + strtype + "";

            #endregion

            #region month query
            SelQ += "   select distinct t.college_code,fd.feecategory,t.textval,isnull(monthcode,'0')as monthcode from Fee_degree_match fd,textvaltable t  where  t.college_code=fd.college_code  and t.textcode=fd.feecategory and t.textcriteria='FEECA' and t.college_code ='" + collegecode + "' " + sem + "";
            #endregion

            #region multiple cashierwise total query

            #region student

            SelQ += " select paymode,sum(debit) as debit,EntryUserCode from(";
            SelQ += " select f.paymode,debit,f.EntryUserCode from ft_findailytransaction f,registration r,textvaltable t where f.app_no=r.app_no and f.feecategory=t.textcode and r.college_code=t.college_code and t.textcriteria='FEECA' " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') " + sem + " and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + strReg + " and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " and isnull(debit,'0')>0 and r.mode in('" + studMode + "') " + strRecon + " and isnull(actualfinyearfk,'0')<>'0'";
            if (usBasedRights == true)
                SelQ += " and f.EntryUserCode in('" + usercode + "')";
            if (cbbeforAdm.Checked)
            {
                SelQ += " union all select f.paymode,debit,f.EntryUserCode from ft_findailytransaction f,applyn r,textvaltable t where f.app_no=r.app_no and f.feecategory=t.textcode and r.college_code=t.college_code and t.textcriteria='FEECA' " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') " + sem + "  and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "'  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " " + applynStr + "  and isnull(debit,'0')>0 and r.mode in('" + studMode + "') and isnull(actualfinyearfk,'0')<>'0'";
                if (usBasedRights == true)
                    SelQ += " and f.EntryUserCode in('" + usercode + "')";
            }
            SelQ += ") tbl group by paymode,EntryUserCode order by paymode asc";
            #endregion

            #region others
            SelQ += " select  SUM(debit) as debit,f.paymode,f.EntryUserCode from FT_FinDailyTransaction f where  HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "' and f.finyearfk in('" + Finyearvalue + "')" + strRecon + "";
            if (usBasedRights == true)
                SelQ += " and f.EntryUserCode in('" + usercode + "')";
            SelQ += " group by f.paymode,f.EntryUserCode";
            ////reconsilation
            //SelQ += " select  SUM(debit) as debit ,f.paymode,f.EntryUserCode from FT_FinDailyTransaction f where ( isnull(f.IsDeposited,'0')='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1'  and HeaderFK in ('" + headervalue + "') and LedgerFK in('" + ledgervalue + "') and f.memtype in('" + memType + "') and f.Paymode in ('" + paid + "') AND TransDate between '" + fromdate + "' and '" + todate + "' and f.finyearfk in('" + Finyearvalue + "')";
            //if (usBasedRights == true)
            //    SelQ += " and f.EntryUserCode in('" + usercode + "')";
            //SelQ += " group by f.paymode,f.EntryUserCode";
            #endregion

            SelQ += " select distinct EntryUserCode  from ft_findailytransaction f where f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and f.debit>0  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " ";
            if (memType != "")
                SelQ += " and memtype in('1','" + memType + "')";
            else
                SelQ += " and memtype in('1')";
            if (usBasedRights == true)
                SelQ += " and f.EntryUserCode in('" + usercode + "')";
            SelQ += "  order by f.EntryUserCode asc  ";


            #endregion

            //tbl 15 financial year
            SelQ += "  select (convert(varchar(10),datepart(year,finyearstart))+'-'+convert(varchar(10),datepart(year,finyearend)))as finyear,finyearpk,collegecode from fm_finyearmaster where collegecode='" + collegecode + "'";
            //tbl 16 distinct actual financial year
            SelQ += " select sum(debit) as debit,finyearfk,actualfinyearfk from(";
            SelQ += " select debit,finyearfk,actualfinyearfk from ft_findailytransaction f,registration r,textvaltable t where f.app_no=r.app_no and f.feecategory=t.textcode and r.college_code=t.college_code and t.textcriteria='FEECA' " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') " + sem + " and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + strReg + " and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " and isnull(debit,'0')>0 and isnull(actualfinyearfk,'0')<>'0' and r.mode in('" + studMode + "')";
            if (usBasedRights == true)
                SelQ += " and f.EntryUserCode in('" + usercode + "')";
            if (cbbeforAdm.Checked)
            {
                SelQ += " union all select debit,finyearfk,actualfinyearfk from ft_findailytransaction f,applyn r,textvaltable t where f.app_no=r.app_no and f.feecategory=t.textcode and r.college_code=t.college_code and t.textcriteria='FEECA' " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') " + sem + "  and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "'  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " " + applynStr + "  and isnull(debit,'0')>0 and r.mode in('" + studMode + "') and isnull(actualfinyearfk,'0')<>'0'";
                if (usBasedRights == true)
                    SelQ += " and f.EntryUserCode in('" + usercode + "')";
            }
            SelQ += ") tbl group by finyearfk,actualfinyearfk order by finyearfk asc";

            //added by sudhagar 10.03.2017
            if (cbconcession.Checked)
            {
                #region concession
                SelQ += "  select app_no,batch_year,sum(totalamount) as totalamount,sum(deductamout) as deductamout,deductreason,sum(paidamount) as paidamount,stud_name,roll_admit,degree_code,sections,finyearfk from( select ft.app_no,r.batch_year,totalamount,deductamout,deductreason,paidamount,stud_name,roll_admit,degree_code,sections,ft.finyearfk from ft_findailytransaction f,registration r,textvaltable t,ft_feeallot ft where f.app_no=r.app_no and r.app_no=ft.app_no and f.app_no=ft.app_no and f.feecategory=t.textcode and ft.feecategory=t.textcode and f.feecategory=ft.feecategory and f.headerfk=ft.headerfk and f.ledgerfk=ft.ledgerfk and r.college_code=t.college_code and t.textcriteria='FEECA'  " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') " + sem + " and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "' " + strReg + " and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + "  and isnull(debit,'0')>0 and isnull(deductamout,'0')>0 and isnull(deductreason,'')<>''  and isnull(paidamount,'0')>0 and isnull(actualfinyearfk,'0')<>'0'";
                if (usBasedRights == true)
                    SelQ += " and f.EntryUserCode in('" + usercode + "')";
                if (cbbeforAdm.Checked)
                {
                    SelQ += " union all select ft.app_no,r.batch_year,totalamount,deductamout,deductreason,paidamount,stud_name,''roll_admit,degree_code,''sections,ft.finyearfk from ft_findailytransaction f,applyn r,textvaltable t,ft_feeallot ft where f.app_no=r.app_no and r.app_no=ft.app_no and f.app_no=ft.app_no and f.feecategory=t.textcode and ft.feecategory=t.textcode and f.feecategory=ft.feecategory and f.headerfk=ft.headerfk and f.ledgerfk=ft.ledgerfk and r.college_code=t.college_code and t.textcriteria='FEECA'  " + strRecon + " and r.batch_year in('" + batch + "')  and r.degree_code in('" + deptdegcode + "') " + sem + "  and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.paymode in('" + paid + "') and f.finyearfk in('" + Finyearvalue + "') and r.college_code ='" + collegecode + "'  and f.Transdate between '" + fromdate + "' and '" + todate + "'" + strFine + " " + applynStr + "  and isnull(debit,'0')>0 and isnull(deductamout,'0')>0 and isnull(deductreason,'')<>''  and isnull(paidamount,'0')>0  and isnull(actualfinyearfk,'0')<>'0'";
                    if (usBasedRights == true)
                        SelQ += " and f.EntryUserCode in('" + usercode + "')";
                }
                SelQ += " ) tbl group by app_no,batch_year,deductreason,stud_name,roll_admit,degree_code,sections,finyearfk order by batch_year asc";
                SelQ += "  select TextCode,TextVal from TextValTable where TextCriteria ='DedRe' and college_code ='" + collegecode + "'";
                SelQ += " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename,(c.Course_Name +'-'+dt.dept_acronym)as dept_acronym from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode + "'";
                #endregion
            }
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(SelQ, "Text");

            #endregion
        }
        catch { }
        return dsload;
    }

    //old
    protected void SpreadLoadDetailss(DataSet ds)
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


            for (int batch = 0; batch < chklsfyear.Items.Count; batch++)
            {
                if (chklsfyear.Items[batch].Selected)
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
                                                string strVal = "finyearfk='" + chklsfyear.Items[batch].Value + "' and textval like'%" + cbl_sem.Items[sem].Text + "%' and headerfk='" + chkl_studhed.Items[hd].Value + "' and paymode='" + payModeVal + "'";
                                                int curColCnt = 0;
                                                double paiAmount = 0;
                                                int.TryParse(Convert.ToString(htPayCol[payModeVal]), out curColCnt);
                                                DataView dvhd = new DataView();
                                                ds.Tables[0].DefaultView.RowFilter = strVal;
                                                dvhd = ds.Tables[0].DefaultView;
                                                if (dvhd.Count > 0)
                                                {
                                                    for (int hdrow = 0; hdrow < dvhd.Count; hdrow++)
                                                    {
                                                        // DataTable dtval = dvhd.ToTable();
                                                        //double.TryParse(Convert.ToString(dtval.Compute("sum(debit)", "")), out paiAmount);
                                                        double.TryParse(Convert.ToString(dvhd[hdrow]["debit"]), out paiAmount);
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
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(chklsfyear.Items[batch].Text);
                                                            DataView dvMnth = new DataView();
                                                            string Month = string.Empty;
                                                            if (ds.Tables[11].Rows.Count > 0)
                                                            {
                                                                ds.Tables[11].DefaultView.RowFilter = "textval='" + cbl_sem.Items[sem].Text + "' and college_code='" + collegecode + "'";
                                                                dvMnth = ds.Tables[11].DefaultView;
                                                                if (dvMnth.Count > 0)
                                                                    Month = getMonth(Convert.ToString(dvMnth[0]["monthcode"]));
                                                            }
                                                            string termStr = Convert.ToString(cbl_sem.Items[sem].Text);
                                                            if (!string.IsNullOrEmpty(Month))
                                                                termStr += "(" + Month + ")";
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = termStr;

                                                            //finyear text 
                                                            DataView dvfinFK = new DataView();
                                                            string strFinText = string.Empty;
                                                            if (ds.Tables[15].Rows.Count > 0)
                                                            {
                                                                ds.Tables[15].DefaultView.RowFilter = "finyearpk='" + dvhd[0]["actualfinyearfk"] + "' and collegecode='" + collegecode + "'";
                                                                dvfinFK = ds.Tables[15].DefaultView;
                                                                if (dvfinFK.Count > 0)
                                                                    strFinText = Convert.ToString(dvfinFK[0]["finyear"]);
                                                            }
                                                            if (!string.IsNullOrEmpty(strFinText))
                                                                strFinText = Convert.ToString(chkl_studhed.Items[hd].Text) + " (" + strFinText + " )";
                                                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].Text = strFinText;
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
                                                            //boolColumn = false;
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
                                                    boolColumn = false;
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
                                        #endregion
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
                                                string strVal = "batch_year='" + cbl_batch.Items[batch].Value + "' and textval like'%" + cbl_sem.Items[sem].Text + "%' and ledgerfk='" + chkl_studled.Items[hd].Value + "' and paymode='" + payModeVal + "'";
                                                int curColCnt = 0;
                                                double paiAmount = 0;
                                                int.TryParse(Convert.ToString(htPayCol[payModeVal]), out curColCnt);
                                                DataView dvhd = new DataView();
                                                ds.Tables[0].DefaultView.RowFilter = strVal;
                                                dvhd = ds.Tables[0].DefaultView;
                                                if (dvhd.Count > 0)
                                                {
                                                    DataTable dtval = dvhd.ToTable();
                                                    double.TryParse(Convert.ToString(dtval.Compute("sum(debit)", "")), out paiAmount);
                                                }
                                                //if (dvhd.Count > 0)
                                                //    double.TryParse(Convert.ToString(dvhd[0]["debit"]), out paiAmount);
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
                                                    DataView dvMnth = new DataView();
                                                    string Month = string.Empty;
                                                    if (ds.Tables[11].Rows.Count > 0)
                                                    {
                                                        ds.Tables[11].DefaultView.RowFilter = "textval='" + cbl_sem.Items[sem].Text + "' and college_code='" + collegecode + "'";
                                                        dvMnth = ds.Tables[11].DefaultView;
                                                        if (dvMnth.Count > 0)
                                                            Month = getMonth(Convert.ToString(dvMnth[0]["monthcode"]));
                                                    }
                                                    string termStr = Convert.ToString(cbl_sem.Items[sem].Text);
                                                    if (!string.IsNullOrEmpty(Month))
                                                        termStr += "(" + Month + ")";
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = termStr;
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

                #region individual cashier total
                if (ds.Tables[14].Rows.Count > 0)
                {
                    int snocnt = 0;
                    for (int us = 0; us < ds.Tables[14].Rows.Count; us++)
                    {
                        Hashtable htcash = new Hashtable();
                        string userCode = Convert.ToString(ds.Tables[14].Rows[us]["EntryUserCode"]);
                        if (ds.Tables[12].Rows.Count > 0 && !string.IsNullOrEmpty(userCode))
                        {
                            ds.Tables[12].DefaultView.RowFilter = " EntryUserCode='" + userCode + "'";
                            DataView dvus = ds.Tables[12].DefaultView;
                            if (dvus.Count > 0)
                            {
                                for (int i = 0; i < dvus.Count; i++)
                                {
                                    double Amt = 0;
                                    double.TryParse(Convert.ToString(dvus[i]["debit"]), out Amt);
                                    if (!htcash.ContainsKey(Convert.ToString(dvus[i]["paymode"])))
                                        htcash.Add(Convert.ToString(dvus[i]["paymode"]), Convert.ToString(Amt));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(htcash[Convert.ToString(dvus[i]["paymode"])]), out amount);
                                        amount += Amt;
                                        htcash.Remove(Convert.ToString(dvus[i]["paymode"]));
                                        htcash.Add(Convert.ToString(dvus[i]["paymode"]), Convert.ToString(amount));
                                    }
                                }
                            }
                        }
                        if (ds.Tables[13].Rows.Count > 0 && !string.IsNullOrEmpty(userCode))
                        {
                            ds.Tables[13].DefaultView.RowFilter = " EntryUserCode='" + userCode + "'";
                            DataView dvus = ds.Tables[13].DefaultView;
                            if (dvus.Count > 0)
                            {
                                for (int i = 0; i < dvus.Count; i++)
                                {
                                    double Amt = 0;
                                    double.TryParse(Convert.ToString(dvus[i]["debit"]), out Amt);
                                    if (!htcash.ContainsKey(Convert.ToString(dvus[i]["paymode"])))
                                        htcash.Add(Convert.ToString(dvus[i]["paymode"]), Convert.ToString(Amt));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(htcash[Convert.ToString(dvus[i]["paymode"])]), out amount);
                                        amount += Amt;
                                        htcash.Remove(Convert.ToString(dvus[i]["paymode"]));
                                        htcash.Add(Convert.ToString(dvus[i]["paymode"]), Convert.ToString(amount));
                                    }
                                }
                            }
                        }
                        if (htcash.Count > 0)
                        {
                            snocnt++;
                            string Name = d2.GetFunction(" select user_id from usermaster where college_code='" + collegecode + "' and user_code='" + userCode + "'");
                            spreadDet.Sheets[0].Rows.Count++;
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = Convert.ToString(snocnt);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 1].Text = Name;
                            spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 1, 1, 2);
                            //  spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
                            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                            double gdvalues = 0;
                            double paiAmount = 0;
                            for (int s = 0; s < chkl_paid.Items.Count; s++)
                            {
                                if (chkl_paid.Items[s].Selected == true)
                                {
                                    string payModeVal = Convert.ToString(chkl_paid.Items[s].Value);
                                    int curColCnt = 0;
                                    int.TryParse(Convert.ToString(htPayCol[payModeVal]), out curColCnt);
                                    double.TryParse(Convert.ToString(htcash[payModeVal]), out gdvalues);
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, curColCnt].Text = Convert.ToString(gdvalues);
                                    paiAmount += gdvalues;
                                }
                            }
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(paiAmount);
                        }
                    }
                    spreadDet.Sheets[0].Rows.Count++;
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, spreadDet.Sheets[0].ColumnCount - 1);
                }
                #endregion

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
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;//0ca6ca
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
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = ColorTranslator.FromHtml("#0ca6ca");
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

                if (cbconcession.Checked && ds.Tables[15].Rows.Count > 0)//&& ds.Tables[15].Rows.Count > 0
                {
                    DataSet dsval = new DataSet();
                    dsval.Tables.Add(ds.Tables[15].Copy());
                    dsval.Tables.Add(ds.Tables[16].Copy());
                    dsval.Tables.Add(ds.Tables[17].Copy());
                    loadConsStud(spreadDet, dsval);
                }

                payModeLabels(htPayCol);
                spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
                lblvalidation1.Text = "";
                txtexcelname.Text = "";
                spreadDet.Visible = true;
                print.Visible = true;
                spreadDet.Height = height;
                spreadDet.SaveChanges();
                printVisibleSettings();
            }
        }
        catch { }
    }

    //college
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
                                                string strVal = "batch_year='" + cbl_batch.Items[batch].Value + "' and textval like'%" + cbl_sem.Items[sem].Text + "%' and headerfk='" + chkl_studhed.Items[hd].Value + "' and paymode='" + payModeVal + "'";
                                                int curColCnt = 0;
                                                double paiAmount = 0;
                                                int.TryParse(Convert.ToString(htPayCol[payModeVal]), out curColCnt);
                                                DataView dvhd = new DataView();
                                                ds.Tables[0].DefaultView.RowFilter = strVal;
                                                dvhd = ds.Tables[0].DefaultView;
                                                if (dvhd.Count > 0)
                                                {
                                                    DataTable dtval = dvhd.ToTable();
                                                    double.TryParse(Convert.ToString(dtval.Compute("sum(debit)", "")), out paiAmount);
                                                }
                                                //double.TryParse(Convert.ToString(dvhd[0]["debit"]), out paiAmount);
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
                                                    DataView dvMnth = new DataView();
                                                    string Month = string.Empty;
                                                    if (ds.Tables[11].Rows.Count > 0)
                                                    {
                                                        ds.Tables[11].DefaultView.RowFilter = "textval='" + cbl_sem.Items[sem].Text + "' and college_code='" + collegecode + "'";
                                                        dvMnth = ds.Tables[11].DefaultView;
                                                        if (dvMnth.Count > 0)
                                                            Month = getMonth(Convert.ToString(dvMnth[0]["monthcode"]));
                                                    }
                                                    string termStr = Convert.ToString(cbl_sem.Items[sem].Text);
                                                    if (!string.IsNullOrEmpty(Month))
                                                        termStr += "(" + Month + ")";
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = termStr;

                                                    ////finyear text 
                                                    //DataView dvfinFK = new DataView();
                                                    //string strFinText = string.Empty;
                                                    //if (ds.Tables[15].Rows.Count > 0)
                                                    //{
                                                    //    ds.Tables[15].DefaultView.RowFilter = "finyearpk='" + dvhd[0]["actualfinyearfk"] + "' and collegecode='" + collegecode + "'";
                                                    //    dvfinFK = ds.Tables[15].DefaultView;
                                                    //    if (dvfinFK.Count > 0)
                                                    //        strFinText = Convert.ToString(dvfinFK[0]["finyear"]);
                                                    //}
                                                    //if (!string.IsNullOrEmpty(strFinText))
                                                    //    strFinText = Convert.ToString(chkl_studhed.Items[hd].Text) + " (" + strFinText + " )";
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
                                                string strVal = "batch_year='" + cbl_batch.Items[batch].Value + "' and textval like'%" + cbl_sem.Items[sem].Text + "%' and ledgerfk='" + chkl_studled.Items[hd].Value + "' and paymode='" + payModeVal + "'";
                                                int curColCnt = 0;
                                                double paiAmount = 0;
                                                int.TryParse(Convert.ToString(htPayCol[payModeVal]), out curColCnt);
                                                DataView dvhd = new DataView();
                                                ds.Tables[0].DefaultView.RowFilter = strVal;
                                                dvhd = ds.Tables[0].DefaultView;
                                                if (dvhd.Count > 0)
                                                {
                                                    DataTable dtval = dvhd.ToTable();
                                                    double.TryParse(Convert.ToString(dtval.Compute("sum(debit)", "")), out paiAmount);
                                                }
                                                //if (dvhd.Count > 0)
                                                //    double.TryParse(Convert.ToString(dvhd[0]["debit"]), out paiAmount);
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
                                                    DataView dvMnth = new DataView();
                                                    string Month = string.Empty;
                                                    if (ds.Tables[11].Rows.Count > 0)
                                                    {
                                                        ds.Tables[11].DefaultView.RowFilter = "textval='" + cbl_sem.Items[sem].Text + "' and college_code='" + collegecode + "'";
                                                        dvMnth = ds.Tables[11].DefaultView;
                                                        if (dvMnth.Count > 0)
                                                            Month = getMonth(Convert.ToString(dvMnth[0]["monthcode"]));
                                                    }
                                                    string termStr = Convert.ToString(cbl_sem.Items[sem].Text);
                                                    if (!string.IsNullOrEmpty(Month))
                                                        termStr += "(" + Month + ")";
                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = termStr;
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

                #region individual cashier total
                if (ds.Tables[14].Rows.Count > 0)
                {
                    int snocnt = 0;
                    for (int us = 0; us < ds.Tables[14].Rows.Count; us++)
                    {
                        Hashtable htcash = new Hashtable();
                        string userCode = Convert.ToString(ds.Tables[14].Rows[us]["EntryUserCode"]);
                        if (ds.Tables[12].Rows.Count > 0 && !string.IsNullOrEmpty(userCode))
                        {
                            ds.Tables[12].DefaultView.RowFilter = " EntryUserCode='" + userCode + "'";
                            DataView dvus = ds.Tables[12].DefaultView;
                            if (dvus.Count > 0)
                            {
                                for (int i = 0; i < dvus.Count; i++)
                                {
                                    double Amt = 0;
                                    double.TryParse(Convert.ToString(dvus[i]["debit"]), out Amt);
                                    if (!htcash.ContainsKey(Convert.ToString(dvus[i]["paymode"])))
                                        htcash.Add(Convert.ToString(dvus[i]["paymode"]), Convert.ToString(Amt));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(htcash[Convert.ToString(dvus[i]["paymode"])]), out amount);
                                        amount += Amt;
                                        htcash.Remove(Convert.ToString(dvus[i]["paymode"]));
                                        htcash.Add(Convert.ToString(dvus[i]["paymode"]), Convert.ToString(amount));
                                    }
                                }
                            }
                        }
                        if (ds.Tables[13].Rows.Count > 0 && !string.IsNullOrEmpty(userCode))
                        {
                            ds.Tables[13].DefaultView.RowFilter = " EntryUserCode='" + userCode + "'";
                            DataView dvus = ds.Tables[13].DefaultView;
                            if (dvus.Count > 0)
                            {
                                for (int i = 0; i < dvus.Count; i++)
                                {
                                    double Amt = 0;
                                    double.TryParse(Convert.ToString(dvus[i]["debit"]), out Amt);
                                    if (!htcash.ContainsKey(Convert.ToString(dvus[i]["paymode"])))
                                        htcash.Add(Convert.ToString(dvus[i]["paymode"]), Convert.ToString(Amt));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(htcash[Convert.ToString(dvus[i]["paymode"])]), out amount);
                                        amount += Amt;
                                        htcash.Remove(Convert.ToString(dvus[i]["paymode"]));
                                        htcash.Add(Convert.ToString(dvus[i]["paymode"]), Convert.ToString(amount));
                                    }
                                }
                            }
                        }
                        if (htcash.Count > 0)
                        {
                            snocnt++;
                            string Name = d2.GetFunction(" select user_id from usermaster where college_code='" + collegecode + "' and user_code='" + userCode + "'");
                            spreadDet.Sheets[0].Rows.Count++;
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = Convert.ToString(snocnt);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 1].Text = Name;
                            spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 1, 1, 2);
                            //  spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
                            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                            double gdvalues = 0;
                            double paiAmount = 0;
                            for (int s = 0; s < chkl_paid.Items.Count; s++)
                            {
                                if (chkl_paid.Items[s].Selected == true)
                                {
                                    string payModeVal = Convert.ToString(chkl_paid.Items[s].Value);
                                    int curColCnt = 0;
                                    int.TryParse(Convert.ToString(htPayCol[payModeVal]), out curColCnt);
                                    double.TryParse(Convert.ToString(htcash[payModeVal]), out gdvalues);
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, curColCnt].Text = Convert.ToString(gdvalues);
                                    paiAmount += gdvalues;
                                }
                            }
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(paiAmount);
                        }
                    }
                    spreadDet.Sheets[0].Rows.Count++;
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, spreadDet.Sheets[0].ColumnCount - 1);
                }
                #endregion

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
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;//0ca6ca
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
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = ColorTranslator.FromHtml("#0ca6ca");
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

                if (cbconcession.Checked && ds.Tables[15].Rows.Count > 0)//&& ds.Tables[15].Rows.Count > 0
                {
                    DataSet dsval = new DataSet();
                    dsval.Tables.Add(ds.Tables[15].Copy());
                    dsval.Tables.Add(ds.Tables[16].Copy());
                    dsval.Tables.Add(ds.Tables[17].Copy());
                    loadConsStud(spreadDet, dsval);
                }

                payModeLabels(htPayCol);
                spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
                lblvalidation1.Text = "";
                txtexcelname.Text = "";
                spreadDet.Visible = true;
                print.Visible = true;
                spreadDet.Height = height;
                spreadDet.SaveChanges();
                printVisibleSettings();
            }
        }
        catch { }
    }

    //school
    protected void SpreadLoadDetailsScl(DataSet ds)
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
            spreadDet.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
            spreadDet.Sheets[0].Columns[0].Width = 60;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Text = lblbatch.Text;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            spreadDet.Sheets[0].Columns[1].Width = 80;
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
            spreadDet.Sheets[0].Columns[2].Width = 150;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Text = strHdName;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
            spreadDet.Sheets[0].Columns[3].Width = 320;
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
            Dictionary<string, string> studModeVal = getstudMode();
            Dictionary<string, double> cashTot = new Dictionary<string, double>();
            Dictionary<string, string> headname = new Dictionary<string, string>();
            Hashtable grandtotal = new Hashtable();
            Hashtable total = new Hashtable();
            bool boolColumn = false;
            bool boolGrand = false;
            int height = 0;
            int rowCnt = 0;
            int introwCnt = 0;
            string strFinYEarText = string.Empty;
            for (int batch = 0; batch < chklsfyear.Items.Count; batch++)
            {
                bool boolfinsno = true;
                if (chklsfyear.Items[batch].Selected)
                {
                    DataView dvfnlyear = new DataView();
                    if (ds.Tables[15].Rows.Count > 0)
                    {
                        ds.Tables[15].DefaultView.RowFilter = "finyearpk='" + chklsfyear.Items[batch].Value + "' and collegecode='" + collegecode + "'";
                        dvfnlyear = ds.Tables[15].DefaultView;
                        if (dvfnlyear.Count > 0)
                            strFinYEarText = Convert.ToString(dvfnlyear[0]["finyear"]);
                    }
                    bool boolyear = false;
                    //  rowCnt++;
                    for (int sem = 0; sem < cbl_sem.Items.Count; sem++)
                    {
                        if (cbl_sem.Items[sem].Selected)
                        {
                            if (rbtype.SelectedIndex == 0)
                            {
                                #region header
                                for (int hd = 0; hd < chkl_studhed.Items.Count; hd++)
                                {

                                    bool boolpay = false;
                                    if (chkl_studhed.Items[hd].Selected)
                                    {

                                        DataView dvactFnlYr = new DataView();
                                        ds.Tables[16].DefaultView.RowFilter = "finyearfk='" + chklsfyear.Items[batch].Value + "'";
                                        dvactFnlYr = ds.Tables[16].DefaultView;
                                        if (dvactFnlYr.Count > 0)
                                        {
                                            for (int actrow = 0; actrow < dvactFnlYr.Count; actrow++)
                                            {
                                                foreach (KeyValuePair<string, string> modeVal in studModeVal)
                                                {
                                                    double FnltotAmount = 0;
                                                    string strVal = "finyearfk='" + chklsfyear.Items[batch].Value + "' and textval like'%" + cbl_sem.Items[sem].Text + "%' and headerfk='" + chkl_studhed.Items[hd].Value + "' and actualfinyearfk='" + dvactFnlYr[actrow]["actualfinyearfk"] + "' and mode='" + modeVal.Value + "'";
                                                    DataView dvhd = new DataView();
                                                    ds.Tables[0].DefaultView.RowFilter = strVal;
                                                    dvhd = ds.Tables[0].DefaultView;
                                                    if (dvhd.Count > 0)
                                                    {
                                                        #region paid
                                                        boolColumn = true;
                                                        for (int s = 0; s < chkl_paid.Items.Count; s++)
                                                        {
                                                            string payModeVal = string.Empty;
                                                            string studMode = string.Empty;
                                                            if (chkl_paid.Items[s].Selected == true)
                                                            {
                                                                payModeVal = Convert.ToString(chkl_paid.Items[s].Value);
                                                                double paiAmount = 0;
                                                                int curColCnt = 0;
                                                                int.TryParse(Convert.ToString(htPayCol[payModeVal]), out curColCnt);
                                                                // dvhd.RowFilter = "paymode like'%" + payModeVal + "%'";
                                                                dvhd.RowFilter = strVal + " and paymode ='" + payModeVal + "'";
                                                                DataView dvMode = dvhd;
                                                                if (dvMode.Count > 0)
                                                                {
                                                                    DataTable dtval = dvMode.ToTable();
                                                                    double.TryParse(Convert.ToString(dtval.Compute("sum(debit)", "")), out paiAmount);
                                                                    studMode = Convert.ToString(dvMode[0]["Mode"]);
                                                                    studMode = studentMode(studMode);
                                                                }
                                                                FnltotAmount += paiAmount;
                                                                if (paiAmount != 0)
                                                                {
                                                                    if (boolfinsno)
                                                                        rowCnt++;
                                                                    boolfinsno = false;
                                                                    if (boolColumn)
                                                                    {
                                                                        spreadDet.Sheets[0].RowCount++;
                                                                        height += 15;
                                                                        //  rowCnt++;
                                                                        boolpay = true;
                                                                        boolyear = true;
                                                                    }
                                                                    introwCnt++;
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(rowCnt);
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = strFinYEarText;
                                                                    DataView dvMnth = new DataView();
                                                                    string Month = string.Empty;
                                                                    if (ds.Tables[11].Rows.Count > 0)
                                                                    {
                                                                        ds.Tables[11].DefaultView.RowFilter = "textval='" + cbl_sem.Items[sem].Text + "' and college_code='" + collegecode + "'";
                                                                        dvMnth = ds.Tables[11].DefaultView;
                                                                        if (dvMnth.Count > 0)
                                                                            Month = getMonth(Convert.ToString(dvMnth[0]["monthcode"]));
                                                                    }
                                                                    string termStr = Convert.ToString(cbl_sem.Items[sem].Text);
                                                                    if (!string.IsNullOrEmpty(Month))
                                                                        termStr += "(" + Month + ")";
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = termStr;
                                                                    //finyear text 
                                                                    DataView dvfinFK = new DataView();
                                                                    string strFinText = string.Empty;
                                                                    if (ds.Tables[15].Rows.Count > 0)
                                                                    {
                                                                        ds.Tables[15].DefaultView.RowFilter = "finyearpk='" + dvhd[0]["actualfinyearfk"] + "' and collegecode='" + collegecode + "'";
                                                                        dvfinFK = ds.Tables[15].DefaultView;
                                                                        if (dvfinFK.Count > 0)
                                                                            strFinText = Convert.ToString(dvfinFK[0]["finyear"]);
                                                                    }
                                                                    if (!string.IsNullOrEmpty(strFinText))
                                                                        strFinText = Convert.ToString(chkl_studhed.Items[hd].Text) + " (" + strFinText + " )" + "-" + studMode;
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].Text = strFinText;
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
                                                                        ds.Tables[1].DefaultView.RowFilter = strVal + " and paymode='" + payModeVal + "'";
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
                                                        #endregion
                                                    }
                                                }
                                            }
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

                                    bool boolpay = false;
                                    if (chkl_studled.Items[hd].Selected)
                                    {
                                        DataView dvactFnlYr = new DataView();
                                        ds.Tables[16].DefaultView.RowFilter = "finyearfk='" + chklsfyear.Items[batch].Value + "'";
                                        dvactFnlYr = ds.Tables[16].DefaultView;
                                        if (dvactFnlYr.Count > 0)
                                        {
                                            for (int actrow = 0; actrow < dvactFnlYr.Count; actrow++)
                                            {
                                                foreach (KeyValuePair<string, string> modeVal in studModeVal)
                                                {
                                                    double FnltotAmount = 0;
                                                    string strVal = "finyearfk='" + chklsfyear.Items[batch].Value + "' and textval like'%" + cbl_sem.Items[sem].Text + "%' and ledgerfk='" + chkl_studled.Items[hd].Value + "' and actualfinyearfk='" + dvactFnlYr[actrow]["actualfinyearfk"] + "' and mode='" + modeVal.Value + "'";
                                                    DataView dvhd = new DataView();
                                                    ds.Tables[0].DefaultView.RowFilter = strVal;
                                                    dvhd = ds.Tables[0].DefaultView;
                                                    if (dvhd.Count > 0)
                                                    {
                                                        #region
                                                        boolColumn = true;
                                                        for (int s = 0; s < chkl_paid.Items.Count; s++)
                                                        {
                                                            string studMode = string.Empty;
                                                            string payModeVal = string.Empty;
                                                            if (chkl_paid.Items[s].Selected == true)
                                                            {
                                                                payModeVal = Convert.ToString(chkl_paid.Items[s].Value);
                                                                double paiAmount = 0;
                                                                int curColCnt = 0;
                                                                int.TryParse(Convert.ToString(htPayCol[payModeVal]), out curColCnt);
                                                                // dvhd.RowFilter = "paymode like'%" + payModeVal + "%'";
                                                                dvhd.RowFilter = strVal + " and paymode ='" + payModeVal + "'";
                                                                DataView dvMode = dvhd;
                                                                if (dvMode.Count > 0)
                                                                {
                                                                    DataTable dtval = dvMode.ToTable();
                                                                    double.TryParse(Convert.ToString(dtval.Compute("sum(debit)", "")), out paiAmount);
                                                                    studMode = Convert.ToString(dvMode[0]["Mode"]);
                                                                    studMode = studentMode(studMode);
                                                                }
                                                                FnltotAmount += paiAmount;
                                                                if (paiAmount != 0)
                                                                {
                                                                    if (boolColumn)
                                                                    {
                                                                        spreadDet.Sheets[0].RowCount++;
                                                                        height += 15;
                                                                        //  rowCnt++;
                                                                        boolpay = true;
                                                                        boolyear = true;
                                                                    }
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(rowCnt);
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = strFinYEarText;
                                                                    DataView dvMnth = new DataView();
                                                                    string Month = string.Empty;
                                                                    if (ds.Tables[11].Rows.Count > 0)
                                                                    {
                                                                        ds.Tables[11].DefaultView.RowFilter = "textval='" + cbl_sem.Items[sem].Text + "' and college_code='" + collegecode + "'";
                                                                        dvMnth = ds.Tables[11].DefaultView;
                                                                        if (dvMnth.Count > 0)
                                                                            Month = getMonth(Convert.ToString(dvMnth[0]["monthcode"]));
                                                                    }
                                                                    string termStr = Convert.ToString(cbl_sem.Items[sem].Text);
                                                                    if (!string.IsNullOrEmpty(Month))
                                                                        termStr += "(" + Month + ")";
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 2].Text = termStr;
                                                                    //finyear text 
                                                                    DataView dvfinFK = new DataView();
                                                                    string strFinText = string.Empty;
                                                                    if (ds.Tables[15].Rows.Count > 0)
                                                                    {
                                                                        ds.Tables[15].DefaultView.RowFilter = "finyearpk='" + dvhd[0]["actualfinyearfk"] + "' and collegecode='" + collegecode + "'";
                                                                        dvfinFK = ds.Tables[15].DefaultView;
                                                                        if (dvfinFK.Count > 0)
                                                                            strFinText = Convert.ToString(dvfinFK[0]["finyear"]);
                                                                    }
                                                                    if (!string.IsNullOrEmpty(strFinText))
                                                                        strFinText = Convert.ToString(chkl_studled.Items[hd].Text) + " (" + strFinText + " )" + "-" + studMode;
                                                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 3].Text = strFinText;
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
                                                                        ds.Tables[1].DefaultView.RowFilter = strVal + " and paymode='" + payModeVal + "'";
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
                                                        #endregion
                                                    }
                                                }
                                            }
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
                spreadDet.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                spreadDet.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                spreadDet.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            }
            #endregion

            #region others
            DataView dvmem = new DataView();
            DataView dvpay = new DataView();
            //  rowCnt = introwCnt;
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

                #region individual cashier total
                if (ds.Tables[14].Rows.Count > 0)
                {
                    int snocnt = 0;
                    for (int us = 0; us < ds.Tables[14].Rows.Count; us++)
                    {
                        Hashtable htcash = new Hashtable();
                        string userCode = Convert.ToString(ds.Tables[14].Rows[us]["EntryUserCode"]);
                        if (ds.Tables[12].Rows.Count > 0 && !string.IsNullOrEmpty(userCode))
                        {
                            ds.Tables[12].DefaultView.RowFilter = " EntryUserCode='" + userCode + "'";
                            DataView dvus = ds.Tables[12].DefaultView;
                            if (dvus.Count > 0)
                            {
                                for (int i = 0; i < dvus.Count; i++)
                                {
                                    double Amt = 0;
                                    double.TryParse(Convert.ToString(dvus[i]["debit"]), out Amt);
                                    if (!htcash.ContainsKey(Convert.ToString(dvus[i]["paymode"])))
                                        htcash.Add(Convert.ToString(dvus[i]["paymode"]), Convert.ToString(Amt));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(htcash[Convert.ToString(dvus[i]["paymode"])]), out amount);
                                        amount += Amt;
                                        htcash.Remove(Convert.ToString(dvus[i]["paymode"]));
                                        htcash.Add(Convert.ToString(dvus[i]["paymode"]), Convert.ToString(amount));
                                    }
                                }
                            }
                        }
                        if (ds.Tables[13].Rows.Count > 0 && !string.IsNullOrEmpty(userCode))
                        {
                            ds.Tables[13].DefaultView.RowFilter = " EntryUserCode='" + userCode + "'";
                            DataView dvus = ds.Tables[13].DefaultView;
                            if (dvus.Count > 0)
                            {
                                for (int i = 0; i < dvus.Count; i++)
                                {
                                    double Amt = 0;
                                    double.TryParse(Convert.ToString(dvus[i]["debit"]), out Amt);
                                    if (!htcash.ContainsKey(Convert.ToString(dvus[i]["paymode"])))
                                        htcash.Add(Convert.ToString(dvus[i]["paymode"]), Convert.ToString(Amt));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(htcash[Convert.ToString(dvus[i]["paymode"])]), out amount);
                                        amount += Amt;
                                        htcash.Remove(Convert.ToString(dvus[i]["paymode"]));
                                        htcash.Add(Convert.ToString(dvus[i]["paymode"]), Convert.ToString(amount));
                                    }
                                }
                            }
                        }
                        if (htcash.Count > 0)
                        {
                            snocnt++;
                            string Name = d2.GetFunction(" select user_id from usermaster where college_code='" + collegecode + "' and user_code='" + userCode + "'");
                            spreadDet.Sheets[0].Rows.Count++;
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = Convert.ToString(snocnt);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 1].Text = Name;
                            spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 1, 1, 2);
                            //  spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
                            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                            double gdvalues = 0;
                            double paiAmount = 0;
                            for (int s = 0; s < chkl_paid.Items.Count; s++)
                            {
                                if (chkl_paid.Items[s].Selected == true)
                                {
                                    string payModeVal = Convert.ToString(chkl_paid.Items[s].Value);
                                    int curColCnt = 0;
                                    int.TryParse(Convert.ToString(htPayCol[payModeVal]), out curColCnt);
                                    double.TryParse(Convert.ToString(htcash[payModeVal]), out gdvalues);
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, curColCnt].Text = Convert.ToString(gdvalues);
                                    paiAmount += gdvalues;
                                }
                            }
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(paiAmount);
                        }
                    }
                    spreadDet.Sheets[0].Rows.Count++;
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, spreadDet.Sheets[0].ColumnCount - 1);
                }
                #endregion

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
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;//0ca6ca
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
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = ColorTranslator.FromHtml("#0ca6ca");
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

                if (cbconcession.Checked && ds.Tables[15].Rows.Count > 0)//&& ds.Tables[15].Rows.Count > 0
                {
                    DataSet dsval = new DataSet();
                    dsval.Tables.Add(ds.Tables[17].Copy());
                    dsval.Tables.Add(ds.Tables[18].Copy());
                    dsval.Tables.Add(ds.Tables[19].Copy());
                    loadConsStud(spreadDet, dsval);
                }

                payModeLabels(htPayCol);
                spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
                lblvalidation1.Text = "";
                txtexcelname.Text = "";
                spreadDet.Visible = true;
                print.Visible = true;
                spreadDet.Height = height;
                spreadDet.SaveChanges();
                printVisibleSettings();
            }
        }
        catch { }
    }
    protected string studentMode(string mode)
    {
        string strMode = string.Empty;
        switch (mode)
        {
            case "1":
                strMode = "Old";
                break;
            case "2":
                strMode = "Transfer";
                break;
            case "3":
                strMode = "New";
                break;
            default:
                strMode = "";
                break;
        }
        return strMode;
    }

    protected void loadConsStud(FarPoint.Web.Spread.FpSpread spreadDet, DataSet dsVal)
    {
        try
        {
            #region design
            spreadDet.Sheets[0].Rows.Count++;
            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Concession Details";
            spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, spreadDet.Sheets[0].ColumnCount - 1);
            // spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = ColorTranslator.FromHtml("#0ca6ca");
            spreadDet.Sheets[0].Rows.Count++;
            spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, spreadDet.Sheets[0].ColumnCount - 1);


            spreadDet.Sheets[0].Rows.Count++;
            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "SNo";
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].HorizontalAlign = HorizontalAlign.Center;

            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 1].Text = "Admission No";
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 1].HorizontalAlign = HorizontalAlign.Center;

            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 2].Text = "Name";
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 2].HorizontalAlign = HorizontalAlign.Center;

            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 3].Text = lbldept.Text;
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 3].HorizontalAlign = HorizontalAlign.Center;

            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].Text = "Allot";
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].HorizontalAlign = HorizontalAlign.Center;

            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 5].Text = "Cons(Amt)";
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 5].HorizontalAlign = HorizontalAlign.Center;

            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 6].Text = "Paid";
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 6].HorizontalAlign = HorizontalAlign.Center;

            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 7].Text = "Type of Cons";
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = ColorTranslator.FromHtml("#0ca6ca");
            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 7].HorizontalAlign = HorizontalAlign.Center;
            #endregion

            #region value
            Hashtable grandtotal = new Hashtable();
            for (int row = 0; row < dsVal.Tables[0].Rows.Count; row++)
            {
                double totAmt = 0;
                double consAmt = 0;
                double paidAmt = 0;
                spreadDet.Sheets[0].RowCount++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = Convert.ToString(row + 1);
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 1].Text = Convert.ToString(dsVal.Tables[0].Rows[row]["roll_admit"]);
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 2].Text = Convert.ToString(dsVal.Tables[0].Rows[row]["stud_name"]);
                string finyearFK = Convert.ToString(dsVal.Tables[0].Rows[row]["finyearfk"]);
                string appNo = Convert.ToString(dsVal.Tables[0].Rows[row]["app_no"]);
                string batch = Convert.ToString(dsVal.Tables[0].Rows[row]["batch_year"]);
                string Sec = Convert.ToString(dsVal.Tables[0].Rows[row]["sections"]);
                string Degreename = "";
                string Acrname = "";
                if (dsVal.Tables[2].Rows.Count > 0)
                {
                    dsVal.Tables[2].DefaultView.RowFilter = "Degree_code='" + Convert.ToString(dsVal.Tables[0].Rows[row]["Degree_code"]) + "'";
                    DataView Dview = dsVal.Tables[2].DefaultView;
                    if (Dview.Count > 0)
                    {
                        Degreename = Convert.ToString(Dview[0]["degreename"]);
                        Acrname = Convert.ToString(Dview[0]["dept_acronym"]);
                    }
                }
                if (!string.IsNullOrEmpty(Sec))
                    Degreename += "-" + Sec;

                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 3].Text = batch + "-" + Degreename;
                totAmt = getAmt(finyearFK, "totalamount", appNo);
               // double.TryParse(Convert.ToString(dsVal.Tables[0].Rows[row]["totalamount"]), out totAmt);
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 4].Text = Convert.ToString(totAmt);
                if (!grandtotal.ContainsKey(4))
                    grandtotal.Add(4, Convert.ToString(totAmt));
                else
                {
                    double amount = 0;
                    double.TryParse(Convert.ToString(grandtotal[4]), out amount);
                    amount += totAmt;
                    grandtotal.Remove(4);
                    grandtotal.Add(4, Convert.ToString(amount));
                }
                consAmt = getAmt(finyearFK, "deductamout", appNo);
                //double.TryParse(Convert.ToString(dsVal.Tables[0].Rows[row]["deductamout"]), out consAmt);
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 5].Text = Convert.ToString(consAmt);
                if (!grandtotal.ContainsKey(5))
                    grandtotal.Add(5, Convert.ToString(consAmt));
                else
                {
                    double amount = 0;
                    double.TryParse(Convert.ToString(grandtotal[5]), out amount);
                    amount += consAmt;
                    grandtotal.Remove(5);
                    grandtotal.Add(5, Convert.ToString(amount));
                }
                paidAmt = getAmt(finyearFK, "paidamount", appNo);
              // double.TryParse(Convert.ToString(dsVal.Tables[0].Rows[row]["paidamount"]), out paidAmt);
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 6].Text = Convert.ToString(paidAmt);

                if (!grandtotal.ContainsKey(6))
                    grandtotal.Add(6, Convert.ToString(paidAmt));
                else
                {
                    double amount = 0;
                    double.TryParse(Convert.ToString(grandtotal[6]), out amount);
                    amount += paidAmt;
                    grandtotal.Remove(6);
                    grandtotal.Add(6, Convert.ToString(amount));
                }
                string deuctReas = string.Empty;
                if (dsVal.Tables[1].Rows.Count > 0)
                {
                    dsVal.Tables[1].DefaultView.RowFilter = "TextCode='" + Convert.ToString(dsVal.Tables[0].Rows[row]["deductreason"]) + "'";
                    DataView Dview = dsVal.Tables[1].DefaultView;
                    if (Dview.Count > 0)
                        deuctReas = Convert.ToString(Dview[0]["TextVal"]);
                }
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 7].Text = deuctReas;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 7].HorizontalAlign = HorizontalAlign.Center;
            }
            #endregion

            #region grand total
            spreadDet.Sheets[0].Rows.Count++;
            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
            spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
            double grandvalue = 0;
            for (int j = 4; j < spreadDet.Sheets[0].ColumnCount; j++)
            {
                double.TryParse(Convert.ToString(grandtotal[j]), out grandvalue);
                if (grandvalue != 0)
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalue);
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Font.Bold = true;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].ForeColor = Color.Black;
            }
            #endregion
        }
        catch { }
    }
    protected double getAmt(string finYearFk, string colName, string appNo)
    {
        double Amt = 0;
        string selQ = "select sum(" + colName + ") from ft_feeallot where app_no='" + appNo + "' and finyearfk='" + finYearFk + "'";
        if (colName == "deductamout")
            selQ += "  and isnull(deductamout,'0')>0 and isnull(deductreason,'')<>''";
        double.TryParse(Convert.ToString(d2.GetFunction(selQ)), out Amt);
        return Amt;
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
                printVisibleSettings();
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

    protected void printVisibleSettings()
    {
        try
        {
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
                    print.Visible = true;
                }
                if (printset.Contains("P"))
                {
                    btnprintmasterhed.Visible = true;
                    print.Visible = true;
                }
                if (printset == "0")
                {
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnExcel.Visible = true;
                    btnprintmasterhed.Visible = true;
                    print.Visible = true;
                }
            }
            #endregion
        }
        catch { }
    }

    protected string getMonth(string monthcode)
    {
        string Month = string.Empty;
        try
        {
            switch (monthcode)
            {
                case "1":
                    Month = "JAN";
                    break;
                case "2":
                    Month = "FEB";
                    break;
                case "3":
                    Month = "MAR";
                    break;
                case "4":
                    Month = "APR";
                    break;
                case "5":
                    Month = "MAY";
                    break;
                case "6":
                    Month = "JUN";
                    break;
                case "7":
                    Month = "JUL";
                    break;
                case "8":
                    Month = "AUG";
                    break;
                case "9":
                    Month = "SEP";
                    break;
                case "10":
                    Month = "OCT";
                    break;
                case "11":
                    Month = "NOV";
                    break;
                case "12":
                    Month = "DEC";
                    break;
                default:
                    Month = "-";
                    break;
            }
        }
        catch { }
        return Month;
    }

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