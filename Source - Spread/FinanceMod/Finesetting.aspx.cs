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

public partial class FinanceMod_Finesetting : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataSet dsload = new DataSet();
    DAccess2 d2 = new DAccess2();
    DAccess2 da = new DAccess2();
    int i = 0;
    int count = 0;
    string usercode = string.Empty;
    static string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string header = string.Empty;
    bool usBasedRights = false;
    bool flag_true = false;

    static int chosedmode = 0;

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
                bindBtch();
                binddeg();
                binddept();
                bindsem();
                loadheaderandledger();
                ledgerload();
                loaddesc1();
                loadsetting();
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
            //loadstrm();
            bindBtch();
            binddeg();
            binddept();
            bindsem();
            //bindsec();
            loadheaderandledger();
            ledgerload();
            //loadpaid();
            //loadfinanceyear();
            // loadconcession();
        }
        catch
        {
        }
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

            cbl_degree.Items.Clear();
            collegecode = ddl_collegename.SelectedItem.Value.ToString();
            ds.Clear();
            string selqry = "select distinct  c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code='" + collegecode + "'";

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
        //lbl.Add(lblstr);
        //lbl.Add(lbldeg);
        //lbl.Add(lbldept);
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

    #region fine setting for student

    protected void btnplusfineReason_OnClick(object sender, EventArgs e)
    {
        imgdiv3.Visible = true;
        panel_description.Visible = true;
    }
    protected void btnminusfineReason_OnClick(object sender, EventArgs e)
    {
        if (ddl_fineReason.Items.Count > 0)
        {
            surediv.Visible = true;
        }
        else
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "No Fine Type Selected";
        }
    }
    protected void btndescpopadd_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_description11.Text != "")
            {
                string sql = "if exists ( select * from CO_MasterValues where MasterValue ='" + txt_description11.Text + "' and MasterCriteria ='FineReason' and CollegeCode ='" + collegecode + "') update CO_MasterValues set MasterValue ='" + txt_description11.Text + "' where MasterValue ='" + txt_description11.Text + "' and MasterCriteria ='FineReason' and CollegeCode ='" + collegecode + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode) values ('" + txt_description11.Text + "','FineReason','" + collegecode + "')";
                int insert = d2.update_method_wo_parameter(sql, "TEXT");
                if (insert != 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Saved sucessfully";
                    txt_description11.Text = "";
                    imgdiv3.Visible = false;
                    panel_description.Visible = false;
                }
                loaddesc1();
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Enter the description";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btndescpopexit_Click(object sender, EventArgs e)
    {
        imgdiv3.Visible = false;
        panel_description.Visible = false;
        loaddesc1();
    }

    public void loaddesc1()
    {
        try
        {
            ddl_fineReason.Items.Clear();
            string query = " select Distinct MasterValue,MasterCode from CO_MasterValues where MasterCriteria ='FineReason' and CollegeCode ='" + collegecode + "' order by MasterValue asc";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_fineReason.DataSource = ds;
                    ddl_fineReason.DataTextField = "MasterValue";
                    ddl_fineReason.DataValueField = "MasterCode";
                    ddl_fineReason.DataBind();
                }
            }
        }
        catch { }
    }
    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
    }
    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        try
        {
            surediv.Visible = false;
            if (ddl_fineReason.Items.Count > 0)
            {

                string sql = "delete from CO_MasterValues where MasterCode='" + ddl_fineReason.SelectedItem.Value.ToString() + "' and MasterCriteria ='FineReason' and CollegeCode='" + collegecode + "' ";
                int delete = d2.update_method_wo_parameter(sql, "TEXT");
                if (delete != 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Deleted Sucessfully";
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Not deleted";
                }
                loaddesc1();
            }

            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "No Fine Type Selected";
            }
        }
        catch { }
    }

    #endregion

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]

    public static List<string> Getrno(string prefixText)
    {
        List<string> name = new List<string>();
        try
        {
            string query = "";
            WebService ws = new WebService();
            if (chosedmode == 0)
            {
                query = "select top 100 Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%' and college_code=" + collegecode + "  order by Roll_No asc";//and college_code=" + collegecode + "
            }
            else if (chosedmode == 1)
            {
                query = "select  top 100 Reg_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Reg_No like '" + prefixText + "%' and college_code=" + collegecode + " order by Reg_No asc";//and college_code=" + collegecode + "
            }
            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }

    public void loadsetting()
    {
        try
        {
            ListItem list1 = new ListItem("Roll No", "0");
            ListItem list2 = new ListItem("Reg No", "1");

            //Roll Number or Reg Number or Admission No or Application Number
            rbl_rollno.Items.Clear();
            string insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollNo' and user_code ='" + usercode + "' --and college_code ='" + ddl_collegename.SelectedValue + "'";

            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                //Roll No
                rbl_rollno.Items.Add(list1);
            }
            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRegNo' and user_code ='" + usercode + "' --and college_code ='" + ddl_collegename.SelectedValue + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //RegNo
                rbl_rollno.Items.Add(list2);
            }
            int smartDisp = Convert.ToInt32(d2.GetFunction("select LinkValue from New_InsSettings where LinkName='DisplayNumberForSmartCd' and user_code ='" + usercode + "' --and college_code in (" + ddl_collegename.SelectedValue + ")").Trim());

            if (rbl_rollno.Items.Count == 0)
            {
                rbl_rollno.Items.Add(list1);
            }
            switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
            {
                case 0:
                case1:
                    txt_rollno.Attributes.Add("placeholder", "Roll No");

                    chosedmode = 0;
                    break;

                case 1:
                case2:
                    txt_rollno.Attributes.Add("placeholder", "Reg No");

                    chosedmode = 1;
                    break;

                    switch (smartDisp)
                    {
                        case 0:
                            goto case1;
                        case 1:
                            goto case2;

                    }
                    break;
            }

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "Finesetting.aspx");
        }
    }

    protected void rbl_rollno_OnSelectedIndexChanged(object sender, EventArgs e)
    {

        switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
        {
            case 0:
                txt_rollno.Attributes.Add("Placeholder", "Roll No");
                chosedmode = 0;
                break;
            case 1:
                txt_rollno.Attributes.Add("Placeholder", "Reg No");
                chosedmode = 1;
                break;

        }
    }

    public void txt_rollno_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string cursem = "";
            string studMode = string.Empty;
            string rollno = Convert.ToString(txt_rollno.Text);
            if (!string.IsNullOrEmpty(rollno))
            {
                string query = "";
                query = "select a.parent_name,a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections ,r.Batch_Year,a.parent_addressP,a.parent_pincodec,Streetp,Cityp,StuPer_Id,Student_Mobile,seattype,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State,co.collname,co.college_code,C.type,(select TextVal from TextValTable where TextCode =ISNULL( a.seattype,0)) as Seat_Type,r.degree_code ,r.mode  from applyn a,Registration r ,Degree d,course c,Department dt,collinfo co where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and co.college_code =d.college_code and d.college_code='" + ddl_collegename.SelectedItem.Value + "'";

                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                    query = query + "and r.Roll_no='" + rollno + "' ";
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                    query = query + "and r.Reg_No='" + rollno + "' ";

                DataSet ds1 = d2.select_method_wo_parameter(query, "Text");
                if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {

                        txt_batch.Text = ds1.Tables[0].Rows[i]["Batch_Year"].ToString();
                        txt_degree.Text = ds1.Tables[0].Rows[i]["Course_Name"].ToString();
                        txt_dept.Text = ds1.Tables[0].Rows[i]["Dept_Name"].ToString();
                        cursem = ds1.Tables[0].Rows[i]["Current_Semester"].ToString();
                        string seatype = ds1.Tables[0].Rows[i]["seattype"].ToString();
                        Session["seatype"] = seatype;
                        studMode = Convert.ToString(ds1.Tables[0].Rows[i]["mode"]);
                    }
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                        rollno = d2.GetFunction("select roll_no from registration where reg_no='" + rollno + "' and college_code='" + ddl_collegename.SelectedItem.Value + "'");
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                        rollno = d2.GetFunction("select roll_no from registration where Roll_admit='" + rollno + "' and college_code='" + ddl_collegename.SelectedItem.Value + "'");

                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "Finesetting.aspx");
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            ds.Clear();
            dsload = dsvalue();
            txt_rollno.Text = "";
            if (dsload.Tables[0].Rows.Count > 0)
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
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "Finesetting.aspx");
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            string college_Code = Convert.ToString(ddl_collegename.SelectedValue);
            StringBuilder sbrollNo = new StringBuilder();
            string batch = getCblSelectedValue(cbl_batch);
            string degcode = getCblSelectedValue(cbl_dept);
            string sem = getCblSelectedValue(cbl_sem);
            string hedgid = ddlheader.SelectedItem.Value.ToString();
            string ledgid = ddlLedger.SelectedItem.Value.ToString();
            string courseId = getCblSelectedValue(cbl_degree);
            string rollno = txt_rollno.Text;
            string selqry = string.Empty;
            int spreadRowCnt = 0;
            string SpreadRollNo = string.Empty;
            string StuRollNo = string.Empty;
            sbrollNo.Append(rollno).Append("','");
            if (FpSpread1.Rows.Count > 0)
            {
                spreadRowCnt = FpSpread1.Rows.Count;
                for (int i = 0; i < spreadRowCnt; i++)
                {
                    SpreadRollNo = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Tag);
                    if (SpreadRollNo != "")
                    {
                        sbrollNo.Append(SpreadRollNo).Append("','");
                    }
                }
            }
            StuRollNo = Convert.ToString(sbrollNo);
            StuRollNo = StuRollNo.TrimEnd(',');

            selqry = " select distinct Roll_No ,Stud_Name,Course_Name+'-'+Dept_Name as Department,r.degree_code,A.HeaderFK,HeaderName,A.LedgerFK,(LedgerName) as LedgerName,isnull(FeeAmount,0) as FeeAmount,isnull(TotalAmount,0) as TotalAmount,isnull(BalAmount,'0') as balamount,TextVal,TextCode,f.DueDate from Registration r,FM_FineMaster f,Degree d,course c,Department dt,FT_FeeAllot A,FM_HeaderMaster H,FM_LedgerMaster L,TextValTable T,FS_HeaderPrivilage P where r.Batch_Year in('" + batch + "') and a.App_No=r.App_No and r.degree_code in ('" + degcode + "') and a.HeaderFK in('" + hedgid + "') and r.college_code in('" + college_Code + "') and A.FeeCategory in('" + sem + "') and a.LedgerFK in('" + ledgid + "') and c.Course_Id in('" + courseId + "') and d.Degree_Code=r.degree_code and d.Course_Id=c.Course_Id and dt.Dept_Code=d.Dept_Code and A.HeaderFK = H.HeaderPK AND A.LedgerFK = L.LedgerPK  and P.HeaderFK = H.HeaderPK and P.HeaderFK = L.HeaderFK and a.HeaderFK=p.HeaderFK  AND H.HeaderPK = L.HeaderFK AND A.FeeCategory = T.TextCode and BalAmount>0  and r.roll_no in('" + StuRollNo + "') order by r.degree_code,r.Roll_No";

            dsload = d2.select_method_wo_parameter(selqry, "Text");
            if (dsload.Tables[0].Rows.Count > 0)
            {
                #region design
                FpSpread1.Visible = true;
                FpSpread1.Sheets[0].RowCount = 1;
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                FpSpread1.CommandBar.Visible = false;
                FpSpread1.Sheets[0].ColumnCount = 7;
                FpSpread1.Sheets[0].RowHeader.Visible = false;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = Color.Black;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Locked = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Columns[0].Width = 50;

                FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                chkall.AutoPostBack = true;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = Color.Black;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Columns[1].Width = 50;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Locked = true;
                FpSpread1.Sheets[0].Columns[1].Locked = false;
                FpSpread1.Sheets[0].Columns[1].Visible = true;
                FpSpread1.Sheets[0].Cells[0, 1].CellType = chkall;
                FpSpread1.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = Color.Black;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Locked = true;
                FpSpread1.Columns[2].Width = 150;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Name";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = Color.Black;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Locked = true;
                FpSpread1.Columns[3].Width = 250;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Department";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = Color.Black;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Locked = true;
                FpSpread1.Columns[4].Width = 250;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Semester";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = Color.Black;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Locked = true;
                FpSpread1.Columns[5].Width = 100;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Fine Reason";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = Color.Black;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Locked = true;
                FpSpread1.Columns[6].Width = 150;

                loaddesc1();
                string[] Finearray = new string[ddl_fineReason.Items.Count + 1];
                Finearray[0] = "Select";
                if (ddl_fineReason.Items.Count > 0)
                {
                    for (int fine = 0; fine < ddl_fineReason.Items.Count; fine++)
                    {
                        Finearray[fine + 1] = Convert.ToString(ddl_fineReason.Items[fine].Text);
                    }
                }
                FarPoint.Web.Spread.ComboBoxCellType cb1 = new FarPoint.Web.Spread.ComboBoxCellType(Finearray);
                cb1.UseValue = true;
                cb1.ShowButton = true;
                cb1.AutoPostBack = true;

                FpSpread1.Sheets[0].Columns[6].Locked = false;
                FpSpread1.Sheets[0].Columns[6].Visible = true;
                FpSpread1.Sheets[0].Cells[0, 6].CellType = cb1;
                FpSpread1.Sheets[0].Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[0, 6].BackColor = Color.SkyBlue;

                #endregion

                #region value
                int sno = 0;
                for (int i = 0; i < dsload.Tables[0].Rows.Count; i++)
                {
                    FpSpread1.Sheets[0].RowCount++;
                    sno++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;

                    FarPoint.Web.Spread.CheckBoxCellType check = new FarPoint.Web.Spread.CheckBoxCellType();
                    check.AutoPostBack = false;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = check;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dsload.Tables[0].Rows[i]["Roll_No"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(dsload.Tables[0].Rows[i]["Roll_No"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = txt;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dsload.Tables[0].Rows[i]["Stud_Name"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dsload.Tables[0].Rows[i]["Department"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dsload.Tables[0].Rows[i]["TextVal"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].CellType = cb1;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;

                }
                #endregion

                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                FpSpread1.Height = 1000;
                FpSpread1.Width = 1000;
                divspread.Visible = true;

                imgdiv2.Visible = false;
                lbl_alert.Text = "";
                lblvalidation1.Text = "";

                for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
                {
                    string roll_no = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                    string semester = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 5].Text);
                    string feecat = d2.GetFunction(" select textcode from TextValTable where textval='" + semester + "' and college_code='" + ddl_collegename .SelectedItem .Value + "'");
                    if (rollno != "")
                    {
                        string confirmFine = " select distinct roll_no from FT_FineCancelSetting where roll_no='" + roll_no.Trim() + "' and feecategory in('" + feecat + "')";
                        DataSet dsFine = new DataSet();
                        dsFine = d2.select_method_wo_parameter(confirmFine, "Text");
                        if (dsFine.Tables.Count > 0 && dsFine.Tables[0].Rows.Count > 0)
                        {
                            for (int k = 0; k < FpSpread1.Columns.Count; k++)
                            {
                                FpSpread1.Sheets[0].Cells[i, k].BackColor = Color.LightGreen;
                                //FpSpread1.Sheets[0].Cells[i, k].Locked = true;
                            }
                        }
                        else
                        {
                            string DelChk = " select distinct roll_no from FT_FineCancelSetting where roll_no='" + roll_no.Trim() + "' and feecategory in('" + feecat + "')";
                            DataSet dsDel = new DataSet();
                            dsDel = d2.select_method_wo_parameter(DelChk, "Text");
                            if (dsDel.Tables.Count > 0 && dsDel.Tables[0].Rows.Count > 0)
                            {
                                for (int k = 0; k < FpSpread1.Columns.Count; k++)
                                {
                                    FpSpread1.Sheets[0].Cells[i, k].BackColor = ColorTranslator.FromHtml("#FF3333");
                                }
                            }
                        }
                    }
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
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "Finesetting.aspx");
        }
    }

    protected DataSet dsvalue()
    {

        try
        {
            string college_Code = Convert.ToString(ddl_collegename.SelectedValue);
            string batch = getCblSelectedValue(cbl_batch);
            string degcode = getCblSelectedValue(cbl_dept);
            string sem = getCblSelectedValue(cbl_sem);
            string hedgid = ddlheader.SelectedItem.Value.ToString();
            string ledgid = ddlLedger.SelectedItem.Value.ToString();
            string courseId = getCblSelectedValue(cbl_degree);

            string selqry = string.Empty;

            selqry = " select distinct Roll_No ,Stud_Name,Course_Name+'-'+Dept_Name as Department,r.degree_code,A.HeaderFK,HeaderName,A.LedgerFK,(LedgerName) as LedgerName,isnull(FeeAmount,0) as FeeAmount,isnull(TotalAmount,0) as TotalAmount,isnull(BalAmount,'0') as balamount,TextVal,TextCode,f.DueDate from Registration r,FM_FineMaster f,Degree d,course c,Department dt,FT_FeeAllot A,FM_HeaderMaster H,FM_LedgerMaster L,TextValTable T,FS_HeaderPrivilage P where r.Batch_Year in('" + batch + "') and a.App_No=r.App_No and r.degree_code in ('" + degcode + "') and a.HeaderFK in('" + hedgid + "') and r.college_code in('" + college_Code + "') and A.FeeCategory in('" + sem + "') and a.LedgerFK in('" + ledgid + "') and c.Course_Id in('" + courseId + "') and d.Degree_Code=r.degree_code and d.Course_Id=c.Course_Id and dt.Dept_Code=d.Dept_Code and A.HeaderFK = H.HeaderPK AND A.LedgerFK = L.LedgerPK  and P.HeaderFK = H.HeaderPK and P.HeaderFK = L.HeaderFK and a.HeaderFK=p.HeaderFK  AND H.HeaderPK = L.HeaderFK AND A.FeeCategory = T.TextCode and BalAmount>0  order by r.degree_code";

            dsload.Clear();
            dsload = d2.select_method_wo_parameter(selqry, "Text");

        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "Finesetting.aspx");
        }
        return dsload;
    }

    protected void loadspreadvalues()
    {
        try
        {
            #region design
            FpSpread1.Visible = true;
            FpSpread1.Sheets[0].RowCount = 1;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 7;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = Color.Black;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Locked = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Columns[0].Width = 50;

            FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
            chkall.AutoPostBack = true;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = Color.Black;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Columns[1].Width = 50;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Locked = true;
            FpSpread1.Sheets[0].Columns[1].Locked = false;
            FpSpread1.Sheets[0].Columns[1].Visible = true;
            FpSpread1.Sheets[0].Cells[0, 1].CellType = chkall;
            FpSpread1.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = Color.Black;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Locked = true;
            FpSpread1.Columns[2].Width = 150;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = Color.Black;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Locked = true;
            FpSpread1.Columns[3].Width = 250;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Department";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = Color.Black;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Locked = true;
            FpSpread1.Columns[4].Width = 250;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Semester";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = Color.Black;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Locked = true;
            FpSpread1.Columns[5].Width = 100;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Fine Reason";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = Color.Black;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Locked = true;
            FpSpread1.Columns[6].Width = 150;
            loaddesc1();
            string[] Finearray = new string[ddl_fineReason.Items.Count + 1];
            Finearray[0] = "Select";
            if (ddl_fineReason.Items.Count > 0)
            {
                for (int fine = 0; fine < ddl_fineReason.Items.Count; fine++)
                {
                    Finearray[fine + 1] = Convert.ToString(ddl_fineReason.Items[fine].Text);
                }
            }
            FarPoint.Web.Spread.ComboBoxCellType cb1 = new FarPoint.Web.Spread.ComboBoxCellType(Finearray);
            cb1.UseValue = true;
            cb1.ShowButton = true;
            cb1.AutoPostBack = true;

            FpSpread1.Sheets[0].Columns[6].Locked = false;
            FpSpread1.Sheets[0].Columns[6].Visible = true;
            FpSpread1.Sheets[0].Cells[0, 6].CellType = cb1;
            FpSpread1.Sheets[0].Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Cells[0, 6].BackColor = Color.SkyBlue;
            #endregion

            #region value
            int sno = 0;

            for (int i = 0; i < dsload.Tables[0].Rows.Count; i++)
            {
                FpSpread1.Sheets[0].RowCount++;
                sno++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;

                FarPoint.Web.Spread.CheckBoxCellType check = new FarPoint.Web.Spread.CheckBoxCellType();
                check.AutoPostBack = false;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = check;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;


                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dsload.Tables[0].Rows[i]["Roll_No"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = txt;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dsload.Tables[0].Rows[i]["Stud_Name"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dsload.Tables[0].Rows[i]["Department"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dsload.Tables[0].Rows[i]["TextVal"]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].CellType = cb1;
                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].BackColor = ColorTranslator.FromHtml("lightyellow");
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Right;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;

            }
            #endregion

            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            FpSpread1.Height = 1000;
            FpSpread1.Width = 1000;
            FpSpread1.SaveChanges();
            divspread.Visible = true;
            imgdiv2.Visible = false;
            lbl_alert.Text = "";
            lblvalidation1.Text = "";
            for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
            {
                string rollno = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                string semester = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 5].Text);
                string feecat = d2.GetFunction(" select textcode from TextValTable where textval='" + semester + "' and college_code='" + ddl_collegename .SelectedItem .Value + "'");
                if (rollno != "")
                {
                    string confirmFine = " select distinct roll_no from FT_FineCancelSetting where roll_no='" + rollno.Trim() + "' and feecategory in('" + feecat + "')";
                    DataSet dsFine = new DataSet();
                    dsFine = d2.select_method_wo_parameter(confirmFine, "Text");
                    if (dsFine.Tables.Count > 0 && dsFine.Tables[0].Rows.Count > 0)
                    {
                        for (int k = 0; k < FpSpread1.Columns.Count; k++)
                        {
                            FpSpread1.Sheets[0].Cells[i, k].BackColor = Color.LightGreen;
                        }
                    }
                    else
                    {
                        string DelChk = " select distinct roll_no from FT_FineCancelSetting where roll_no='" + rollno.Trim() + "' and feecategory in('" + feecat + "')";
                        DataSet dsDel = new DataSet();
                        dsDel = d2.select_method_wo_parameter(DelChk, "Text");
                        if (dsDel.Tables.Count > 0 && dsDel.Tables[0].Rows.Count > 0)
                        {
                            for (int k = 0; k < FpSpread1.Columns.Count; k++)
                            {
                                FpSpread1.Sheets[0].Cells[i, k].BackColor = ColorTranslator.FromHtml("#FF3333");
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "Finesetting.aspx");
        }

    }

    protected void FpSpread1_OnUpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string buttonok = String.Empty;
            string spread = string.Empty;
            string controlatt = string.Empty;
            Control control = null;
            string ctrlname = Page.Request.Params["__EVENTTARGET"];
            if (ctrlname != null && ctrlname != String.Empty)
            {
                control = Page.FindControl(ctrlname);
                spread = ctrlname.ToString();
            }
            else
            {
                string ctrlStr = String.Empty;
                Control c = null;
                foreach (string ctl in Page.Request.Form)
                {
                    if (ctl.EndsWith(".x") || ctl.EndsWith(".y"))
                    {
                        ctrlStr = ctl.Substring(0, ctl.Length - 2);
                        c = Page.FindControl(ctrlStr);
                    }
                    else
                    {
                        c = Page.FindControl(ctl);
                        buttonok = ctl;
                    }
                    if (c is System.Web.UI.WebControls.Button ||
                             c is System.Web.UI.WebControls.ImageButton)
                    {
                        control = c;
                        break;
                    }
                }
            }


            string spreadname = string.Empty;
            if (spread != "")
            {
                string[] spiltspreadname = spread.Split('$');
                spreadname = spiltspreadname[2].ToString().Trim();
                controlatt = spreadname;
            }

            string actrow = FpSpread1.Sheets[0].ActiveRow.ToString();
            string actcol = FpSpread1.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == "1")
            {
                if (FpSpread1.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[0, 1].Value);
                    if (checkval == 0)
                    {
                        for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                        {
                            FpSpread1.Sheets[0].Cells[i, 1].Value = 1;
                        }
                    }
                    if (checkval == 1)
                    {
                        for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                        {
                            FpSpread1.Sheets[0].Cells[i, 1].Value = 0;
                        }
                    }
                }
            }
            if (spreadname.ToString().Trim().ToLower() == "fpspread1")
            {
                actrow = FpSpread1.Sheets[0].ActiveRow.ToString();
                actcol = FpSpread1.Sheets[0].ActiveColumn.ToString();
                string last = e.CommandArgument.ToString();
                if (actrow == "0")
                {
                    if (last == "0")
                    {
                        flag_true = false;
                    }
                    else
                    {
                        flag_true = true;
                    }
                }
                if (actcol == "0")
                {
                    if (actrow == last)
                    {
                        flag_true = false;
                    }
                    else
                    {
                        flag_true = true;
                    }
                }

                if (flag_true == false && actrow == "0" && actcol.Trim() == "6")
                {
                    string seltext = string.Empty;
                    for (int j = 1; j < FpSpread1.Sheets[0].RowCount; j++)
                    {
                        actcol = e.SheetView.ActiveColumn.ToString();
                        string value = e.EditValues[0].ToString();
                        e.Handled = true;
                        seltext = e.EditValues[Convert.ToInt32(actcol)].ToString();
                        ddl_fineReason.SelectedItem.Text = seltext;
                        if (seltext != "System.Object")
                        {
                            if (FpSpread1.Sheets[0].Cells[j, 6].Locked == false)
                            {
                                FpSpread1.Sheets[0].Cells[j, 6].Text = seltext;
                            }
                        }
                        else
                        {
                            if (FpSpread1.Sheets[0].Cells[j, 6].Locked == false)
                            {
                                FpSpread1.Sheets[0].Cells[j, 6].Text = seltext;
                            }
                        }
                    }
                    flag_true = true;
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "Finesetting.aspx");
        }
    }

    protected void btn_FineCancelyes_Click(object sender, EventArgs e)
    {
        try
        {
            string alertmsg = "";
            SureFinediv.Visible = false;
            for (int rowStud = 1; rowStud < FpSpread1.Sheets[0].RowCount; rowStud++)
            {
                int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[rowStud, 1].Value);
                if (checkval == 1)
                {
                    string headerfk = Convert.ToString(ddlheader.SelectedValue);
                    string ledgerfk = Convert.ToString(ddlLedger.SelectedValue);

                    string roll_no = string.Empty;
                    roll_no = Convert.ToString(FpSpread1.Sheets[0].Cells[rowStud, 2].Text);
                    string name = string.Empty;
                    name = Convert.ToString(FpSpread1.Sheets[0].Cells[rowStud, 3].Text);
                    string dept = string.Empty;
                    dept = Convert.ToString(FpSpread1.Sheets[0].Cells[rowStud, 4].Text);
                    string finecan = string.Empty;
                    finecan = Convert.ToString(FpSpread1.Sheets[0].Cells[rowStud, 6].Text);
                    string semester = Convert.ToString(FpSpread1.Sheets[0].Cells[rowStud, 5].Value);
                    string feecat = d2.GetFunction(" select textcode from TextValTable where textval='" + semester + "' and college_code='" + ddl_collegename .SelectedItem .Value + "'");



                    string appNo = string.Empty;
                    string queryRollApp = "select r.app_no from Registration r where r.college_code='" + collegecode + "'  and r.Roll_No='" + roll_no + "'";
                    DataSet dsRollApp = new DataSet();
                    dsRollApp = d2.select_method_wo_parameter(queryRollApp, "Text");
                    if (dsRollApp.Tables.Count > 0)
                    {
                        if (dsRollApp.Tables[0].Rows.Count > 0)
                        {
                            appNo = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_no"]);
                        }
                    }
                    string insertqry = string.Empty;
                    insertqry = " insert into FT_FineCancelSetting (App_no,Roll_No,Name,Department,Reason,feecategory,headerfk,ledgerfk) values('" + appNo + "','" + roll_no + "','" + name + "','" + dept + "','" + finecan + "','" + feecat + "','" + headerfk + "','" + ledgerfk + "') ";
                    d2.update_method_wo_parameter(insertqry, "Text");

                    alertmsg = "Fine Cancelled Successfully";
                }

            }

            if (txt_rollno.Text.Trim() == "")
            {
                btnGo_Click(sender, e);
            }
            else
            {
                btnAdd_Click(sender, e);
            }
            lbl_alert.Text = alertmsg;
            imgdiv2.Visible = true;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "Finesetting.aspx");
        }
    }

    protected void btn_FineCancelno_Click(object sender, EventArgs e)
    {
        SureFinediv.Visible = false;
    }

    public bool checkedOK()
    {
        bool Ok = false;
        FpSpread1.SaveChanges();
        for (int i = 1; i < FpSpread1.Sheets[0].Rows.Count; i++)
        {
            byte check = Convert.ToByte(FpSpread1.Sheets[0].Cells[i, 1].Value);
            if (check == 1)
            {
                Ok = true;
            }
        }
        return Ok;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

        if (checkedOK())
        {
            SureFinediv.Visible = true;
            LblFineCancel.Visible = true;

        }
        else
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Please Select A Student";
        }

    }

    protected void btn_FineAddyes_Click(object sender, EventArgs e)
    {
        try
        {
            string alertmsg = "";
            for (int rowStud = 1; rowStud < FpSpread1.Sheets[0].RowCount; rowStud++)
            {
                int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[rowStud, 1].Value);
                if (checkval == 1)
                {
                    string roll_no = string.Empty;
                    roll_no = Convert.ToString(FpSpread1.Sheets[0].Cells[rowStud, 2].Text);
                    string name = string.Empty;
                    name = Convert.ToString(FpSpread1.Sheets[0].Cells[rowStud, 3].Text);
                    string dept = string.Empty;
                    dept = Convert.ToString(FpSpread1.Sheets[0].Cells[rowStud, 4].Text);
                    string finecan = string.Empty;
                    finecan = Convert.ToString(FpSpread1.Sheets[0].Cells[rowStud, 6].Text);
                    string semester = Convert.ToString(FpSpread1.Sheets[0].Cells[rowStud, 5].Value);
                    string feecat = d2.GetFunction(" select textcode from TextValTable where textval='" + semester + "' and college_code='" + ddl_collegename.SelectedItem .Value  + "'");


                    #region For Each Student

                    string appNo = string.Empty;
                    string queryRollApp = "select r.app_no from Registration r where r.college_code='" + collegecode + "'  and r.Roll_No='" + roll_no + "'";
                    DataSet dsRollApp = new DataSet();
                    dsRollApp = d2.select_method_wo_parameter(queryRollApp, "Text");
                    if (dsRollApp.Tables.Count > 0)
                    {
                        if (dsRollApp.Tables[0].Rows.Count > 0)
                        {
                            appNo = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_no"]);
                        }
                    }
                    string insertqry = string.Empty;
                    insertqry = " delete from FT_FineCancelSetting where app_no='" + appNo + "' and feecategory='" + feecat + "'";
                    d2.update_method_wo_parameter(insertqry, "Text");
                    #endregion
                    alertmsg = "Fine Added";
                }
            }
            if (txt_rollno.Text.Trim() == "")
            {
                btnGo_Click(sender, e);
            }
            else
            {
                btnAdd_Click(sender, e);
            }
            lbl_alert.Text = alertmsg;
            imgdiv2.Visible = true;
            SureDivAdd.Visible = false;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "Finesetting.aspx");
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {

        if (checkedOK())
        {
            SureDivAdd.Visible = true;
            //LblFineCancel.Visible = true;

        }
        else
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Please Select A Student";
        }

    }

    protected void btn_FineAddno_Click(object sender, EventArgs e)
    {
        SureDivAdd.Visible = false;
    }

}