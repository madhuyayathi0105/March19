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

public partial class FinStudAbstractReport : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    ReuasableMethods reuse = new ReuasableMethods();
    DataSet ds = new DataSet();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
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
                //  loadpaid();
                loadheaderandledger();
                ledgerload();
                loadfinanceyear();
                ddlFinYear_SelectedIndexChanged(sender, e);
                // txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                // txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_todate.Attributes.Add("readonly", "readonly");
                //printVisibleSettings();         
                LoadIncludeSetting();
                loadseat();
            }
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
            // loadstrm();
            bindBtch();
            binddeg();
            binddept();
            bindsem();
            // loadpaid();
            loadheaderandledger();
            ledgerload();
            loadfinanceyear();
            loadseat();
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
            ddlbatch.Items.Clear();
            ds.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "batch_year";
                ddlbatch.DataValueField = "batch_year";
                ddlbatch.DataBind();
            }
        }
        catch { }
    }
    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        binddeg();
        binddept();
    }
    #endregion

    #region odl batch
    //public void bindBtch()
    //{
    //    try
    //    {
    //        cbl_batch.Items.Clear();
    //        cb_batch.Checked = false;
    //        txt_batch.Text = "---Select---";
    //        ds.Clear();
    //        ds = d2.BindBatch();
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            cbl_batch.DataSource = ds;
    //            cbl_batch.DataTextField = "batch_year";
    //            cbl_batch.DataValueField = "batch_year";
    //            cbl_batch.DataBind();
    //            if (cbl_batch.Items.Count > 0)
    //            {
    //                for (int i = 0; i < cbl_batch.Items.Count; i++)
    //                {
    //                    cbl_batch.Items[i].Selected = true;
    //                }
    //                txt_batch.Text = lblbatch.Text + "(" + cbl_batch.Items.Count + ")";
    //                cb_batch.Checked = true;
    //            }
    //        }
    //    }
    //    catch { }
    //}
    //protected void cb_batch_OnCheckedChanged(object sender, EventArgs e)
    //{
    //    CallCheckboxChange(cb_batch, cbl_batch, txt_batch, lblbatch.Text, "--Select--");
    //    binddeg();
    //    binddept();
    //}
    //protected void cbl_batch_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    CallCheckboxListChange(cb_batch, cbl_batch, txt_batch, lblbatch.Text, "--Select--");
    //    binddeg();
    //    binddept();
    //}
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
            batch = Convert.ToString(ddlbatch.SelectedValue);
            //for (int i = 0; i < cbl_batch.Items.Count; i++)
            //{
            //    if (cbl_batch.Items[i].Selected == true)
            //    {
            //        if (batch == "")
            //            batch = Convert.ToString(cbl_batch.Items[i].Text);
            //        else
            //            batch += "','" + Convert.ToString(cbl_batch.Items[i].Text);
            //    }
            //}
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
        bindsem();
    }
    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_dept, cbl_dept, txt_dept, "Department", "--Select--");
        bindsem();
    }
    #endregion

    #region sem
    protected void cb_sem_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_sem, cbl_sem, txt_sem, lbldept.Text, "--Select--");

    }
    protected void cbl_sem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_sem, cbl_sem, txt_sem, "Semester", "--Select--");
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
            ddlFinYear.Items.Clear();
            ds = d2.select_method_wo_parameter(getfinanceyear, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string fdatye = ds.Tables[0].Rows[i]["sdate"].ToString() + '-' + ds.Tables[0].Rows[i]["edate"].ToString();
                    string actid = ds.Tables[0].Rows[i]["FinYearPK"].ToString();
                    ddlFinYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem(fdatye, actid));
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void ddlFinYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        string date = Convert.ToString(ddlFinYear.SelectedItem.Text);
        if (!string.IsNullOrEmpty(date))
        {
            getFinancialDate(date);
        }
    }
    protected void getFinancialDate(string date)
    {
        txt_fromdate.Text = Convert.ToString(date.Split('-')[0]);
        txt_todate.Text = Convert.ToString(date.Split('-')[1]);
    }
    #endregion

    #region old financial year
    //public void loadfinanceyear()
    //{
    //    try
    //    {
    //        string fnalyr = "";
    //        string getfinanceyear = "select distinct convert(nvarchar(15),FinYearStart,103) sdate,convert(nvarchar(15),FinYearEnd,103) edate,FinYearPK from FM_FinYearMaster where CollegeCode='" + collegecode + "'  order by FinYearPK desc";
    //        ds.Dispose();
    //        ds.Reset();
    //        chkfyear.Checked = false;
    //        chklsfyear.Items.Clear();
    //        ds = d2.select_method_wo_parameter(getfinanceyear, "text");
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //            {
    //                string fdatye = ds.Tables[0].Rows[i]["sdate"].ToString() + '-' + ds.Tables[0].Rows[i]["edate"].ToString();
    //                string actid = ds.Tables[0].Rows[i]["FinYearPK"].ToString();
    //                chklsfyear.Items.Insert(0, new System.Web.UI.WebControls.ListItem(fdatye, actid));
    //            }

    //            for (int i = 0; i < chklsfyear.Items.Count; i++)
    //            {
    //                chklsfyear.Items[i].Selected = true;
    //                fnalyr = Convert.ToString(chklsfyear.Items[i].Text);
    //            }
    //            if (chklsfyear.Items.Count == 1)
    //            {
    //                txtfyear.Text = "" + fnalyr + "";
    //            }
    //            else
    //            {
    //                txtfyear.Text = "Finance Year(" + (chklsfyear.Items.Count) + ")";
    //            }
    //            // txtfyear.Text = "Finance Year (" + chklsfyear.Items.Count + ")";
    //            chkfyear.Checked = true;
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}
    //protected void chklsfyear_selected(object sender, EventArgs e)
    //{
    //    CallCheckboxListChange(chkfyear, chklsfyear, txtfyear, "Finance Year", "--Select--");

    //}
    //protected void chkfyear_changed(object sender, EventArgs e)
    //{
    //    CallCheckboxChange(chkfyear, chklsfyear, txtfyear, "Finance Year", "--Select--");
    //}
    #endregion

    #region seat Type
    protected void cb_seat_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_seat, cbl_seat, txt_seat, "Seat", "--Select--");

    }
    protected void cbl_seat_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_seat, cbl_seat, txt_seat, "Seat", "--Select--");
    }
    public void loadseat()
    {

        try
        {

            cbl_seat.Items.Clear();
            txt_seat.Text = "--Select--";
            cb_seat.Checked = false;
            string seat = "";
            string deptquery = "select distinct TextCode,TextVal from TextValTable  where TextCriteria='seat' and college_code='" + collegecode + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_seat.DataSource = ds;
                cbl_seat.DataTextField = "TextVal";
                cbl_seat.DataValueField = "TextCode";
                cbl_seat.DataBind();
                if (cbl_seat.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_seat.Items.Count; i++)
                    {
                        cbl_seat.Items[i].Selected = true;
                        seat = Convert.ToString(cbl_seat.Items[i].Text);
                    }
                    if (cbl_seat.Items.Count == 1)
                        txt_seat.Text = "Seat(" + seat + ")";
                    else
                        txt_seat.Text = "Seat(" + cbl_seat.Items.Count + ")";
                    cb_seat.Checked = true;
                }
            }
            else
                txt_seat.Text = "--Select--";
        }
        catch
        {
        }

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

    #region Print
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(spreadDet, reportname);
                //lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your  Report Name";
                // lblvalidation1.Visible = true;
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
            degreedetails = "Finance Student Abstract Report" + '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            pagename = "FinStudAbstractReport.aspx";
            Printcontrolhed.loadspreaddetails(spreadDet, pagename, degreedetails);
            Printcontrolhed.Visible = true;
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
        // lbl.Add(lbl_str1);
        lbl.Add(lbldeg);
        lbl.Add(lbldept);
        lbl.Add(lblsem);
        fields.Add(0);
        // fields.Add(1);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);
        //if (checkSchoolSetting() == 0)
        //{
        //    lblbatch.Text = "Year";
        //    lblheader.Text = "Fees";
        //}

        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }

    protected DataSet getDetails()
    {
        DataSet dsLoad = new DataSet();
        try
        {
            #region getvlaue
            string batch = string.Empty;
            string deptdegcode = string.Empty;
            string paid = string.Empty;
            string headervalue = string.Empty;
            string ledgervalue = string.Empty;
            string Finyearvalue = string.Empty;
            string sem = string.Empty;
            string strReg = string.Empty;
            if (ddlcollegename.Items.Count > 0)
                collegecode = Convert.ToString(ddlcollegename.SelectedItem.Value);
            batch = Convert.ToString(ddlbatch.SelectedValue);
            deptdegcode = Convert.ToString(getCblSelectedValue(cbl_dept));
            sem = Convert.ToString(getCblSelectedValue(cbl_sem));
            paid = Convert.ToString(getCblSelectedValue(chkl_paid));
            headervalue = Convert.ToString(getCblSelectedValue(chkl_studhed));
            ledgervalue = Convert.ToString(getCblSelectedValue(chkl_studled));
            Finyearvalue = Convert.ToString(ddlFinYear.SelectedValue);
            strReg = getStudCategory();
            string fromdate = txt_fromdate.Text;
            string todate = txt_todate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
            string strtype = string.Empty;

            #endregion
            string selQ = string.Empty;

            #region Average Amount headerwise

            #region from date before tbl 0,1
            //from date header wise average amount for each seattype and scholarship
            selQ = " select distinct sum(feeamount) as feeamount,isnull(Post_Matric_Scholarship,'0') as Post_Matric_Scholarship,headerfk,isnull(seattype,'0') as seattype from registration r,ft_feeallot f,applyn a where r.app_no=f.app_no and a.app_no=f.app_no and a.app_no=r.app_no and r.college_code='" + collegecode + "' and isnull(Post_Matric_Scholarship,'0')='1' and r.batch_year ='" + batch + "' and r.degree_code in('" + deptdegcode + "') and feecategory in('" + sem + "') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.finyearfk='" + Finyearvalue + "' group by Post_Matric_Scholarship,headerfk,seattype";
            //get first graduate and no first graduate student count
            selQ += " select distinct sum(feeamount) as feeamount,isnull(first_graduate,'0') as first_graduate,headerfk,isnull(seattype,'0') as seattype from registration r, applyn a,ft_feeallot f where r.app_no=a.app_no and r.app_no=f.app_no and a.app_no=f.app_no and r.college_code='" + collegecode + "'  and r.batch_year ='" + batch + "' and r.degree_code in('" + deptdegcode + "') and feecategory in('" + sem + "') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.finyearfk='" + Finyearvalue + "' group by first_graduate,headerfk,seattype";//and isnull(first_graduate,'0')='1'
            #endregion

            #region readmission tbl 2,3
            //readmission count for financial year which is selected by
            selQ += " select distinct sum(feeamount) as feeamount,isnull(Post_Matric_Scholarship,'0') as Post_Matric_Scholarship,headerfk,isnull(seattype,'0') as seattype from registration r,ft_feeallot f,readmission re,applyn a where r.app_no=f.app_no and r.app_no=re.app_no and f.app_no=re.app_no and a.app_no=f.app_no and a.app_no=r.app_no and a.app_no=re.app_no and r.college_code='" + collegecode + "' and isnull(Post_Matric_Scholarship,'0')='1' and r.batch_year ='" + batch + "' and r.degree_code in('" + deptdegcode + "') and feecategory in('" + sem + "') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.finyearfk='" + Finyearvalue + "' group by Post_Matric_Scholarship,headerfk,seattype";
            //get first graduate and no first graduate student count
            selQ += " select distinct sum(feeamount) as feeamount,isnull(first_graduate,'0') as first_graduate,headerfk,isnull(seattype,'0') as seattype from registration r, applyn a,ft_feeallot f,readmission re where r.app_no=a.app_no and r.app_no=f.app_no and a.app_no=f.app_no and r.app_no=re.app_no and a.app_no=re.app_no and f.app_no=re.app_no and r.college_code='13'  and r.batch_year ='" + batch + "' and r.degree_code in('" + deptdegcode + "') and feecategory in('" + sem + "') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.finyearfk='" + Finyearvalue + "' group by first_graduate,headerfk,seattype";//and isnull(first_graduate,'0')='1'
            #endregion

            #region discontinue tbl 4,5
            //get discontinue count before date which is selected
            selQ += " select distinct sum(feeamount) as feeamount,isnull(Post_Matric_Scholarship,'0') as Post_Matric_Scholarship,headerfk,isnull(seattype,'0') as seattype from registration r,ft_feeallot f,discontinue re,applyn a where r.app_no=f.app_no and r.app_no=re.app_no and f.app_no=re.app_no and a.app_no=f.app_no and a.app_no=r.app_no and a.app_no=re.app_no and r.college_code='13' and isnull(Post_Matric_Scholarship,'0')='1' and r.batch_year ='" + batch + "' and r.degree_code in('" + deptdegcode + "') and feecategory in('" + sem + "') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.finyearfk='" + Finyearvalue + "' and re.discontinue_date>'" + fromdate + "' group by Post_Matric_Scholarship,headerfk,seattype";
            //get first graduate and no first graduate student count
            selQ += " select distinct sum(feeamount) as feeamount,isnull(first_graduate,'0') as first_graduate,headerfk,isnull(seattype,'0') as seattype from registration r, applyn a,ft_feeallot f,discontinue re where r.app_no=a.app_no and r.app_no=f.app_no and a.app_no=f.app_no and r.app_no=re.app_no and a.app_no=re.app_no and f.app_no=re.app_no and r.college_code='13'   and r.batch_year ='" + batch + "' and r.degree_code in('" + deptdegcode + "') and feecategory in('" + sem + "') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.finyearfk='" + Finyearvalue + "' and re.discontinue_date>'" + fromdate + "' group by first_graduate,headerfk,seattype";//and isnull(first_graduate,'0')='1'
            #endregion

            #region toDate current strength tbl 6,7
            //current strength
            //from date header wise average amount for each seattype and scholarship
            selQ += " select distinct sum(feeamount) as feeamount,isnull(Post_Matric_Scholarship,'0') as Post_Matric_Scholarship,headerfk,isnull(seattype,'0') as seattype from registration r,ft_feeallot f,applyn a  where r.app_no=f.app_no and a.app_no=f.app_no and a.app_no=r.app_no  and r.college_code='" + collegecode + "' and isnull(Post_Matric_Scholarship,'0')='1' and r.batch_year ='" + batch + "' and r.degree_code in('" + deptdegcode + "') and feecategory in('" + sem + "') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.finyearfk='" + Finyearvalue + "' " + strReg + " group by Post_Matric_Scholarship,headerfk, seattype";
            //get first graduate and no first graduate student count
            selQ += " select distinct sum(feeamount) as feeamount,isnull(first_graduate,'0') as first_graduate,headerfk,isnull(seattype,'0') as seattype from registration r, applyn a,ft_feeallot f where r.app_no=a.app_no and r.app_no=f.app_no and a.app_no=f.app_no and r.college_code='" + collegecode + "'  and r.batch_year ='" + batch + "' and r.degree_code in('" + deptdegcode + "') and feecategory in('" + sem + "') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.finyearfk='" + Finyearvalue + "' " + strReg + " group by first_graduate,headerfk,seattype";//and isnull(first_graduate,'0')='1'
            #endregion

            #endregion

            #region Average coutn departmentwise

            #region from date before tbl 8,9
            //from date header wise average amount for each seattype and scholarship
            selQ += " select distinct count(roll_no) as roll_no,isnull(Post_Matric_Scholarship,'0') as Post_Matric_Scholarship,r.degree_code ,isnull(seattype,'0') as seattype from registration r,ft_feeallot f,applyn a where r.app_no=f.app_no and a.app_no=f.app_no and a.app_no=r.app_no and r.college_code='" + collegecode + "' and isnull(Post_Matric_Scholarship,'0')='1' and r.batch_year ='" + batch + "' and r.degree_code in('" + deptdegcode + "') and feecategory in('" + sem + "') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.finyearfk='" + Finyearvalue + "' group by Post_Matric_Scholarship,r.degree_code,seattype";
            //get first graduate and no first graduate student count
            selQ += " select distinct count(roll_no) as roll_no,isnull(first_graduate,'0') as first_graduate,r.degree_code,isnull(seattype,'0') as seattype from registration r, applyn a,ft_feeallot f where r.app_no=a.app_no and r.app_no=f.app_no and a.app_no=f.app_no and r.college_code='" + collegecode + "'  and r.batch_year ='" + batch + "' and r.degree_code in('" + deptdegcode + "') and feecategory in('" + sem + "') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.finyearfk='" + Finyearvalue + "' group by first_graduate,r.degree_code,seattype";//and isnull(first_graduate,'0')='1'
            #endregion

            #region readmission tbl 10,11
            //readmission count for financial year which is selected by
            selQ += " select distinct count(roll_no) as roll_no,isnull(Post_Matric_Scholarship,'0') as Post_Matric_Scholarship,r.degree_code,isnull(seattype,'0') as seattype from registration r,ft_feeallot f,readmission re,applyn a where r.app_no=f.app_no and r.app_no=re.app_no and f.app_no=re.app_no and a.app_no=f.app_no and a.app_no=r.app_no and a.app_no=re.app_no and r.college_code='" + collegecode + "' and isnull(Post_Matric_Scholarship,'0')='1' and r.batch_year ='" + batch + "' and r.degree_code in('" + deptdegcode + "') and feecategory in('" + sem + "') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.finyearfk='" + Finyearvalue + "' group by Post_Matric_Scholarship,r.degree_code,seattype";
            //get first graduate and no first graduate student count
            selQ += " select distinct count(roll_no) as roll_no,isnull(first_graduate,'0') as first_graduate,r.degree_code,isnull(seattype,'0') as seattype from registration r, applyn a,ft_feeallot f,readmission re where r.app_no=a.app_no and r.app_no=f.app_no and a.app_no=f.app_no and r.app_no=re.app_no and a.app_no=re.app_no and f.app_no=re.app_no and r.college_code='13'  and r.batch_year ='" + batch + "' and r.degree_code in('" + deptdegcode + "') and feecategory in('" + sem + "') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.finyearfk='" + Finyearvalue + "' group by first_graduate,r.degree_code,seattype";//and isnull(first_graduate,'0')='1'
            #endregion

            #region discontinue tbl 12,13
            //get discontinue count before date which is selected
            selQ += " select distinct count(roll_no) as roll_no,isnull(Post_Matric_Scholarship,'0') as Post_Matric_Scholarship,r.degree_code,isnull(seattype,'0') as seattype from registration r,ft_feeallot f,discontinue re,applyn a where r.app_no=f.app_no and r.app_no=re.app_no and f.app_no=re.app_no and a.app_no=f.app_no and a.app_no=r.app_no and a.app_no=re.app_no and r.college_code='13' and isnull(Post_Matric_Scholarship,'0')='1' and r.batch_year ='" + batch + "' and r.degree_code in('" + deptdegcode + "') and feecategory in('" + sem + "') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.finyearfk='" + Finyearvalue + "' and re.discontinue_date>'" + fromdate + "' group by Post_Matric_Scholarship,r.degree_code,seattype";
            //get first graduate and no first graduate student count
            selQ += " select distinct count(roll_no) as roll_no,isnull(first_graduate,'0') as first_graduate,r.degree_code,isnull(seattype,'0') as seattype from registration r, applyn a,ft_feeallot f,discontinue re where r.app_no=a.app_no and r.app_no=f.app_no and a.app_no=f.app_no and r.app_no=re.app_no and a.app_no=re.app_no and f.app_no=re.app_no and r.college_code='13'   and r.batch_year ='" + batch + "' and r.degree_code in('" + deptdegcode + "') and feecategory in('" + sem + "') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.finyearfk='" + Finyearvalue + "' and re.discontinue_date>'" + fromdate + "' group by first_graduate,r.degree_code,seattype";//and isnull(first_graduate,'0')='1'
            #endregion

            #region to date current strength tbl 14,15
            //current strength
            //from date header wise average amount for each seattype and scholarship
            selQ += " select distinct count(roll_no) as roll_no,isnull(Post_Matric_Scholarship,'0') as Post_Matric_Scholarship,r.degree_code,isnull(seattype,'0') as seattype from registration r,ft_feeallot f,applyn a where r.app_no=f.app_no and a.app_no=f.app_no and a.app_no=r.app_no and r.college_code='" + collegecode + "' and isnull(Post_Matric_Scholarship,'0')='1' and r.batch_year ='" + batch + "' and r.degree_code in('" + deptdegcode + "') and feecategory in('" + sem + "') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.finyearfk='" + Finyearvalue + "' " + strReg + " group by Post_Matric_Scholarship,r.degree_code, seattype";
            //get first graduate and no first graduate student count
            selQ += " select distinct count(roll_no) as roll_no,isnull(first_graduate,'0') as first_graduate,r.degree_code,isnull(seattype,'0') as seattype from registration r, applyn a,ft_feeallot f where r.app_no=a.app_no and r.app_no=f.app_no and a.app_no=f.app_no and r.college_code='" + collegecode + "'  and r.batch_year ='" + batch + "' and r.degree_code in('" + deptdegcode + "') and feecategory in('" + sem + "') and f.headerfk in('" + headervalue + "') and f.ledgerfk in('" + ledgervalue + "') and f.finyearfk='" + Finyearvalue + "' " + strReg + " group by first_graduate,r.degree_code,seattype";//and isnull(first_graduate,'0')='1'
            #endregion

            #endregion

            dsLoad.Clear();
            dsLoad = d2.select_method_wo_parameter(selQ, "Text");
        }
        catch { }
        return dsLoad;
    }
    //set spread column here
    protected ArrayList getMainColumns()
    {
        ArrayList arCol = new ArrayList();
        try
        {
            string fromDt = Convert.ToString(txt_fromdate.Text);
            string toDt = Convert.ToString(txt_todate.Text);
            string selFinYr = Convert.ToString(ddlFinYear.SelectedItem.Text);
            string Year = Convert.ToString(selFinYr.Split('-')[0].Split('/')[2]) + "-" + Convert.ToString(selFinYr.Split('-')[1].Split('/')[2]);
            arCol.Add("As on " + fromDt);
            arCol.Add("Read-Even " + Year);
            arCol.Add("Disc-After " + fromDt);
            arCol.Add("Current Strength " + fromDt);
        }
        catch { arCol.Clear(); }
        return arCol;
    }


    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            ds.Reset();
            ds = getDetails();
            if (ds.Tables.Count > 0)
            {
                getSpreadLoad(ds);
            }
            else
            {
                //lblvalidation1.Text = string.Empty;
                txtexcelname.Text = string.Empty;
                spreadDet.Visible = false;
                print.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
            }
        }
        catch { }
    }

    protected Dictionary<string, string> getHdOrLedg()
    {
        Dictionary<string, string> dtHdName = new Dictionary<string, string>();
        try
        {
            for (int hd = 0; hd < chkl_studhed.Items.Count; hd++)
            {
                if (!chkl_studhed.Items[hd].Selected)
                    continue;
                if (!dtHdName.ContainsKey(chkl_studhed.Items[hd].Value))
                    dtHdName.Add(chkl_studhed.Items[hd].Value, chkl_studhed.Items[hd].Text);
            }
        }
        catch { }
        return dtHdName;
    }

    protected void getSpreadLoad(DataSet ds)
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

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Header/" + lbldept.Text;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;

            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 3, 1);
            Hashtable htCourseName = htcourseName();
            Dictionary<string, string> dtHdName = getHdOrLedg();
            Hashtable htColCnt = new Hashtable();
            ArrayList arFstRowcol = getMainColumns();
            //    ArrayList arSndRowcol = getSecondColumns();
            ArrayList arThrdRowcol = new ArrayList();
            arThrdRowcol.Add("PMS");
            arThrdRowcol.Add("FG");
            arThrdRowcol.Add("NFG");
            foreach (string fstRowName in arFstRowcol)
            {
                int firstFnlCol = 0;
                int totcolFnlcnt = 0;
                for (int seat = 0; seat < cbl_seat.Items.Count; seat++)
                {
                    if (!cbl_seat.Items[seat].Selected)
                        continue;
                    #region
                    int firstCol = 0;
                    int totcolcnt = 0;
                    foreach (string thrdRowName in arThrdRowcol)
                    {
                        spreadDet.Sheets[0].ColumnCount++;
                        int sndCol = spreadDet.Sheets[0].ColumnCount - 1;
                        if (firstCol == 0)
                            firstCol = sndCol;
                        if (firstFnlCol == 0)
                            firstFnlCol = sndCol;
                        totcolcnt++;
                        totcolFnlcnt++;
                        htColCnt.Add(fstRowName + "~" + cbl_seat.Items[seat].Value + "~" + thrdRowName, sndCol);
                        spreadDet.Sheets[0].ColumnHeader.Cells[2, sndCol].Text = thrdRowName;
                        spreadDet.Sheets[0].ColumnHeader.Cells[2, sndCol].ForeColor = ColorTranslator.FromHtml("#000000");
                        spreadDet.Sheets[0].ColumnHeader.Cells[2, sndCol].Font.Bold = true;
                        spreadDet.Sheets[0].ColumnHeader.Cells[2, sndCol].Font.Name = "Book Antiqua";
                        spreadDet.Sheets[0].ColumnHeader.Cells[2, sndCol].Font.Size = FontUnit.Medium;
                        spreadDet.Sheets[0].ColumnHeader.Cells[2, sndCol].HorizontalAlign = HorizontalAlign.Center;
                        spreadDet.Sheets[0].Columns[sndCol].HorizontalAlign = HorizontalAlign.Center;
                    }
                    if (totcolcnt > 0)
                    {
                        spreadDet.Sheets[0].ColumnHeader.Cells[1, firstCol].Text = cbl_seat.Items[seat].Text;
                        spreadDet.Sheets[0].ColumnHeader.Cells[1, firstCol].ForeColor = ColorTranslator.FromHtml("#000000");
                        spreadDet.Sheets[0].ColumnHeader.Cells[1, firstCol].Font.Bold = true;
                        spreadDet.Sheets[0].ColumnHeader.Cells[1, firstCol].Font.Name = "Book Antiqua";
                        spreadDet.Sheets[0].ColumnHeader.Cells[1, firstCol].Font.Size = FontUnit.Medium;
                        spreadDet.Sheets[0].ColumnHeader.Cells[1, firstCol].HorizontalAlign = HorizontalAlign.Center;
                        spreadDet.Sheets[0].Columns[firstCol].HorizontalAlign = HorizontalAlign.Center;
                        spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(1, firstCol, 1, totcolcnt);
                    }
                    #endregion
                }
                if (totcolFnlcnt > 0)
                {
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, firstFnlCol].Text = fstRowName;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, firstFnlCol].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, firstFnlCol].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, firstFnlCol].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, firstFnlCol].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, firstFnlCol].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[firstFnlCol].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, firstFnlCol, 1, totcolFnlcnt);

                    spreadDet.Sheets[0].ColumnCount++;
                    int sndCol = spreadDet.Sheets[0].ColumnCount - 1;
                    htColCnt.Add(fstRowName + "~" + "Total", sndCol);
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, sndCol].Text = "Total";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, sndCol].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, sndCol].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, sndCol].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, sndCol].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, sndCol].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[sndCol].HorizontalAlign = HorizontalAlign.Center;
                    // spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, sndCol, 1, 1);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, sndCol, 3, 1);
                }
            }
            #endregion

            #region value
            int rowCnt = 0;
            int dsTblOne = 0;
            int totalMinColCnt = 0;
            int curColCnt = 0;
            string strCol = string.Empty;

            #region average count
            bool boolCheck = false;
            Hashtable htAmtTot = new Hashtable();
            foreach (KeyValuePair<string, string> dtName in dtHdName)
            {
                #region
                spreadDet.Sheets[0].RowCount++;
                int rowCount = spreadDet.Sheets[0].RowCount - 1;
                spreadDet.Sheets[0].Cells[rowCount, 0].Text = Convert.ToString(++rowCnt);
                spreadDet.Sheets[0].Cells[rowCount, 1].Text = Convert.ToString(dtName.Value);
                string strHd = " headerfk='" + dtName.Key + "'";
                totalMinColCnt = 0;
                foreach (string fstRowName in arFstRowcol)
                {
                    double indivTotAmt = 0;
                    totalMinColCnt++;
                    switch (totalMinColCnt)
                    {
                        case 1:
                            dsTblOne = 0;
                            break;
                        case 2:
                            dsTblOne = 2;
                            break;
                        case 3:
                            dsTblOne = 4;
                            break;
                        case 4:
                            dsTblOne = 6;
                            break;
                    }
                    for (int seat = 0; seat < cbl_seat.Items.Count; seat++)
                    {
                        dsTblOne = dsTblOne;
                        if (!cbl_seat.Items[seat].Selected)
                            continue;
                        string seatValue = Convert.ToString(cbl_seat.Items[seat].Value);
                        string seatText = Convert.ToString(cbl_seat.Items[seat].Text);
                        int tempTblcnt = 0;
                        foreach (string thrdRowName in arThrdRowcol)
                        {
                            bool boolFG = false;
                            switch (thrdRowName)
                            {
                                case "PMS":
                                    tempTblcnt = dsTblOne;
                                    strHd = " headerfk='" + dtName.Key + "'";
                                    break;
                                case "FG":
                                    tempTblcnt += 1;
                                    strHd = " headerfk='" + dtName.Key + "' and first_graduate='1'";
                                    break;
                                case "NFG":
                                    //tempTblcnt = tempTblcnt;                                  
                                    strHd = " headerfk='" + dtName.Key + "' and first_graduate='0'";
                                    break;
                            }
                            ds.Tables[tempTblcnt].DefaultView.RowFilter = strHd + " and seattype='" + seatValue + "'";
                            DataTable dtCount = ds.Tables[tempTblcnt].DefaultView.ToTable();
                            strCol = fstRowName + "~" + seatValue + "~" + thrdRowName;
                            curColCnt = 0;
                            int.TryParse(Convert.ToString(htColCnt[strCol]), out curColCnt);
                            double paiAmount = 0;
                            if (dtCount.Rows.Count > 0)
                                double.TryParse(Convert.ToString(dtCount.Rows[0]["feeamount"]), out paiAmount);
                            spreadDet.Sheets[0].Cells[rowCount, curColCnt].Text = Convert.ToString(paiAmount);
                            indivTotAmt += paiAmount;
                            if (!htAmtTot.ContainsKey(curColCnt))
                                htAmtTot.Add(curColCnt, Convert.ToString(paiAmount));
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(htAmtTot[curColCnt]), out amount);
                                amount += paiAmount;
                                htAmtTot.Remove(curColCnt);
                                htAmtTot.Add(curColCnt, Convert.ToString(amount));
                            }
                            boolCheck = true;
                        }
                    }
                    //total column wise total
                    strCol = fstRowName + "~" + "Total";
                    curColCnt = 0;
                    int.TryParse(Convert.ToString(htColCnt[strCol]), out curColCnt);
                    spreadDet.Sheets[0].Cells[rowCount, curColCnt].Text = Convert.ToString(indivTotAmt);
                    if (!htAmtTot.ContainsKey(curColCnt))
                        htAmtTot.Add(curColCnt, Convert.ToString(indivTotAmt));
                    else
                    {
                        double amount = 0;
                        double.TryParse(Convert.ToString(htAmtTot[curColCnt]), out amount);
                        amount += indivTotAmt;
                        htAmtTot.Remove(curColCnt);
                        htAmtTot.Add(curColCnt, Convert.ToString(amount));
                    }
                }
                #endregion
            }
            if (boolCheck)
            {
                spreadDet.Sheets[0].Rows.Count++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                double grandvalues = 0;
                for (int j = 2; j < spreadDet.Sheets[0].ColumnCount; j++)
                {
                    double.TryParse(Convert.ToString(htAmtTot[j]), out grandvalues);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                } 
            }
            #endregion

            #region departmentwise
            htAmtTot.Clear();
            rowCnt = 0;
            dsTblOne = 0;
            totalMinColCnt = 0;
            curColCnt = 0;
            ArrayList ardegree = new ArrayList();
            Hashtable htGrndTotal = new Hashtable();
            for (int dept = 0; dept < cbl_dept.Items.Count; dept++)
            {
                if (!cbl_dept.Items[dept].Selected)
                    continue;
                string deptName = Convert.ToString(cbl_dept.Items[dept].Text);
                string deptValue = Convert.ToString(cbl_dept.Items[dept].Value);
                string courseId = Convert.ToString(htCourseName[deptValue]);
                if (!ardegree.Contains(courseId))
                {
                    #region every degreewise total
                    if (htAmtTot.Count > 0)
                    {
                        spreadDet.Sheets[0].Rows.Count++;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Total";
                        spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                        spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
                        spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                        spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                        double grandCounts = 0;
                        for (int j = 2; j < spreadDet.Sheets[0].ColumnCount; j++)
                        {
                            double.TryParse(Convert.ToString(htAmtTot[j]), out grandCounts);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandCounts);
                            if (!htGrndTotal.ContainsKey(j))
                                htGrndTotal.Add(j, Convert.ToString(grandCounts));
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(htGrndTotal[j]), out amount);
                                amount += grandCounts;
                                htGrndTotal.Remove(j);
                                htGrndTotal.Add(j, Convert.ToString(amount));
                            }
                        }
                        htAmtTot.Clear();
                    }
                    ardegree.Add(courseId);
                    #endregion
                }
                #region
                spreadDet.Sheets[0].RowCount++;
                int rowCount = spreadDet.Sheets[0].RowCount - 1;
                spreadDet.Sheets[0].Cells[rowCount, 0].Text = Convert.ToString(++rowCnt);
                spreadDet.Sheets[0].Cells[rowCount, 1].Text = deptName;
                string strHd = " degree_code='" + deptValue + "'";
                totalMinColCnt = 0;
                foreach (string fstRowName in arFstRowcol)
                {
                    double indivTotCnt = 0;
                    totalMinColCnt++;
                    switch (totalMinColCnt)
                    {
                        case 1:
                            dsTblOne = 8;
                            break;
                        case 2:
                            dsTblOne = 10;
                            break;
                        case 3:
                            dsTblOne = 12;
                            break;
                        case 4:
                            dsTblOne = 14;
                            break;
                    }
                    for (int seat = 0; seat < cbl_seat.Items.Count; seat++)
                    {
                        dsTblOne = dsTblOne;
                        if (!cbl_seat.Items[seat].Selected)
                            continue;
                        string seatValue = Convert.ToString(cbl_seat.Items[seat].Value);
                        string seatText = Convert.ToString(cbl_seat.Items[seat].Text);
                        int tempTblcnt = 0;
                        foreach (string thrdRowName in arThrdRowcol)
                        {
                            bool boolFG = false;
                            switch (thrdRowName)
                            {
                                case "PMS":
                                    tempTblcnt = dsTblOne;
                                    strHd = " degree_code='" + deptValue + "'";
                                    break;
                                case "FG":
                                    tempTblcnt += 1;
                                    strHd = " degree_code='" + deptValue + "' and first_graduate='1'";
                                    break;
                                case "NFG":
                                    //tempTblcnt = tempTblcnt;                                  
                                    strHd = " degree_code='" + deptValue + "' and first_graduate='0'";
                                    break;
                            }
                            ds.Tables[tempTblcnt].DefaultView.RowFilter = strHd + " and seattype='" + seatValue + "'";
                            DataTable dtCount = ds.Tables[tempTblcnt].DefaultView.ToTable();
                            strCol = fstRowName + "~" + seatValue + "~" + thrdRowName;
                            curColCnt = 0;
                            int.TryParse(Convert.ToString(htColCnt[strCol]), out curColCnt);
                            double studCount = 0;
                            if (dtCount.Rows.Count > 0)
                                double.TryParse(Convert.ToString(dtCount.Rows[0]["roll_no"]), out studCount);
                            spreadDet.Sheets[0].Cells[rowCount, curColCnt].Text = Convert.ToString(studCount);
                            indivTotCnt += studCount;
                            if (!htAmtTot.ContainsKey(curColCnt))
                                htAmtTot.Add(curColCnt, Convert.ToString(studCount));
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(htAmtTot[curColCnt]), out amount);
                                amount += studCount;
                                htAmtTot.Remove(curColCnt);
                                htAmtTot.Add(curColCnt, Convert.ToString(amount));
                            }
                        }
                    }
                    //total column wise total
                    strCol = fstRowName + "~" + "Total";
                    curColCnt = 0;
                    int.TryParse(Convert.ToString(htColCnt[strCol]), out curColCnt);
                    spreadDet.Sheets[0].Cells[rowCount, curColCnt].Text = Convert.ToString(indivTotCnt);
                    if (!htAmtTot.ContainsKey(curColCnt))
                        htAmtTot.Add(curColCnt, Convert.ToString(indivTotCnt));
                    else
                    {
                        double amount = 0;
                        double.TryParse(Convert.ToString(htAmtTot[curColCnt]), out amount);
                        amount += indivTotCnt;
                        htAmtTot.Remove(curColCnt);
                        htAmtTot.Add(curColCnt, Convert.ToString(amount));
                    }
                }
                #endregion
            }
            #region every degreewise total
            if (htAmtTot.Count > 0)
            {
                spreadDet.Sheets[0].Rows.Count++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Total";
                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
                double grandCounts = 0;
                for (int j = 2; j < spreadDet.Sheets[0].ColumnCount; j++)
                {
                    double.TryParse(Convert.ToString(htAmtTot[j]), out grandCounts);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandCounts);
                    if (!htGrndTotal.ContainsKey(j))
                        htGrndTotal.Add(j, Convert.ToString(grandCounts));
                    else
                    {
                        double amount = 0;
                        double.TryParse(Convert.ToString(htGrndTotal[j]), out amount);
                        amount += grandCounts;
                        htGrndTotal.Remove(j);
                        htGrndTotal.Add(j, Convert.ToString(amount));
                    }
                }
                htAmtTot.Clear();
            }           
            #endregion

            spreadDet.Sheets[0].Rows.Count++;
            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
            spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].Font.Bold = true;
            double grandCount = 0;
            for (int j = 2; j < spreadDet.Sheets[0].ColumnCount; j++)
            {
                double.TryParse(Convert.ToString(htGrndTotal[j]), out grandCount);
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandCount);
            }

            #endregion

            #endregion

            spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            spreadDet.Visible = true;
            print.Visible = true;
            // spreadDet.Height = height;
            spreadDet.SaveChanges();
            printVisibleSettings();

        }
        catch { }
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

    protected Hashtable htcourseName()
    {
        Hashtable htcourse = new Hashtable();
        try
        {
            string selQ = "select c.course_id,d.degree_code from degree d,course c,department dt where d.course_id=c.course_id and d.dept_code=dt.dept_code and d.college_code='" + ddlcollegename.SelectedValue + "'";
            DataSet dscourse = d2.select_method_wo_parameter(selQ, "Text");
            if (dscourse.Tables.Count > 0 && dscourse.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dscourse.Tables[0].Rows.Count; row++)
                {
                    string courseId = Convert.ToString(dscourse.Tables[0].Rows[row]["course_id"]);
                    string degreeCode = Convert.ToString(dscourse.Tables[0].Rows[row]["degree_code"]);
                    if (!htcourse.ContainsKey(degreeCode))
                        htcourse.Add(degreeCode, courseId); 
                }
            }
        }
        catch { }
        return htcourse;
    }
}