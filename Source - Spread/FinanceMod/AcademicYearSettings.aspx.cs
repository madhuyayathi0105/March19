using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Text;
using System.Configuration;

public partial class AcademicYearSettings : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    ReuasableMethods reuse = new ReuasableMethods();
    DataSet ds = new DataSet();
    Hashtable hat = new Hashtable();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static bool semSettings = false;
    static string linkName = string.Empty;

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
                bindCollege();
                if (cblclg.Items.Count > 0)
                    collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            }
            if (cblclg.Items.Count > 0)
                collegecode = Convert.ToString(getCblSelectedValue(cblclg));
        }
        catch (Exception ex) { }
    }
    #region college
    //public void loadcollege()
    //{
    //    ddlcollegename.Items.Clear();
    //    reuse.bindCollegeToDropDown(usercode, ddlcollegename);
    //    if (ddlcollegename.Items.Count > 0)
    //    {
    //        // ddlcollegename.Items.Insert(0, "All");
    //    }
    //}
    //protected void ddlcollegename_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (ddlcollegename.Items.Count > 0)
    //        {
    //            collegecode = Convert.ToString(ddlcollegename.SelectedItem.Value);
    //        }

    //    }
    //    catch
    //    {
    //    }
    //}
    #endregion
    #region college
    protected void bindCollege()
    {
        cblclg.Items.Clear();
        cbclg.Checked = false;
        txtclg.Text = "--Select--";
        string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + usercode + " and cp.college_code=cf.college_code";
        ds = d2.select_method_wo_parameter(selectQuery, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            cblclg.DataSource = ds;
            cblclg.DataTextField = "collname";
            cblclg.DataValueField = "college_code";
            cblclg.DataBind();
            if (cblclg.Items.Count > 0)
            {
                for (int row = 0; row < cblclg.Items.Count; row++)
                {
                    cblclg.Items[row].Selected = true;
                }
                cbclg.Checked = true;
                txtclg.Text = lblclg.Text + "(" + cblclg.Items.Count + ")";
            }
        }
    }
    protected void cbclg_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cbclg, cblclg, txtclg, lblclg.Text, "--Select--");
    }
    protected void cblclg_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbclg, cblclg, txtclg, lblclg.Text, "--Select--");
    }
    #endregion

    //go
    protected void btnGo_Click(object sender, EventArgs e)
    {
        getSemSettings();
        // bindSettingGrid();
        getOldSettings();
        tblSave.Visible = true;
        divEdit.Visible = false;
    }
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        rblTypeNew.Enabled = true;
        if (rblType.SelectedItem.Value == "Academic Year Settings")
        {
            rblTypeNew.SelectedIndex = 0;
        }
        else if (rblType.SelectedItem.Value == "Odd Settings")
        {
            rblTypeNew.SelectedIndex = 1;
        }
        else
        {
            rblTypeNew.SelectedIndex = 2;
        }
        btnRowOK.Text = "Save";
        getSemSettings();
        bindSettingGrid();
    }

    protected void getOldSettings()
    {
        bool boolCheck = false;
        tblSave.Visible = false;
        gdReport.Visible = false;
        divEdit.Visible = false;
        Hashtable htPaidInsert = new Hashtable();
        DataTable dtReport = new DataTable();
        DataRow drReport;
        dtReport.Columns.Add("Sno");
        dtReport.Columns.Add("collegeStr");
        dtReport.Columns.Add("collegeVal");
        dtReport.Columns.Add("lblAcdemic");
        dtReport.Columns.Add("batchYear");
        dtReport.Columns.Add("semester");
        dtReport.Columns.Add("semesterVal");
        dtReport.Columns.Add("button");
        //   getClg(ref  collegecode);
        //htPaidInsert.Add("@ACD_COLLEGECODE", collegecode);
        //htPaidInsert.Add("@ACD_YEAR", "");
        //htPaidInsert.Add("@ACD_BATCH_YEAR", "");
        //htPaidInsert.Add("@ACD_FEECATEGORY", 0);
        //htPaidInsert.Add("@QUERY_TYPE", 1);
        //DataSet dsPrevAMount = d2.select_method("USP_SAVE_ACADEMICYEAR", htPaidInsert, "sp");
        string settingType = string.Empty;
        if (rblType.SelectedIndex == 0)
            settingType = "0";
        else if (rblType.SelectedIndex == 1)
            settingType = "1";
        else if (rblType.SelectedIndex == 2)
            settingType = "2";

        string collegecode = Convert.ToString(getCblSelectedValue(cblclg));
        string selQ = " SELECT distinct ACD_COLLEGE_CODE,c.collname,ACD_YEAR,ACD_BATCH_YEAR,ACD_FEECATEGORY,t.textval FROM FT_ACADEMICYEAR AY,FT_ACADEMICYEAR_DETAILED AYD,collinfo c,textvaltable t WHERE c.college_code=ay.acd_college_code and AY.ACA_YEAR_PK=AYD.ACA_YEAR_FK and textcriteria='FEECA' and t.textcode=ayd.ACD_FEECATEGORY and t.college_code=ay.ACD_COLLEGE_CODE AND  ACD_COLLEGE_CODE IN('" + collegecode + "') and ACD_SETTING_TYPE='" + settingType + "' order by ACD_COLLEGE_CODE  asc,ACD_YEAR desc,ACD_FEECATEGORY asc";
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
                                    sbSemStr.Append(feecaTStr + ",");
                                }
                                if (sbSem.Length > 0)
                                    sbSem.Remove(sbSem.Length - 1, 1);
                                if (sbSemStr.Length > 0)
                                    sbSemStr.Remove(sbSemStr.Length - 1, 1);
                                drReport = dtReport.NewRow();
                                drReport["Sno"] = Convert.ToString(Sno);
                                drReport["collegeStr"] = Convert.ToString(dtAcdYear.Rows[row]["collname"]);
                                drReport["collegeVal"] = Convert.ToString(dtAcdYear.Rows[row]["ACD_COLLEGE_CODE"]);
                                drReport["lblAcdemic"] = Convert.ToString(dtAcdYear.Rows[row]["ACD_YEAR"]);
                                drReport["batchYear"] = Convert.ToString(dtBatch.Rows[bat]["ACD_BATCH_YEAR"]);
                                drReport["semesterVal"] = sbSem;
                                drReport["semester"] = sbSemStr;
                                drReport["button"] = Convert.ToString(Sno);
                                dtReport.Rows.Add(drReport);
                                boolCheck = true;
                            }
                        }
                    }
                }
            }
        }
        if (dtReport.Rows.Count > 0)
        {
            gdReport.DataSource = dtReport;
            gdReport.DataBind();
            tblSave.Visible = true;
            divEdit.Visible = true;
            gdReport.Visible = true;
        }
        if (!boolCheck)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found')", true);
        }

    }
    protected void gdattrpt_OnDataBound(object sender, EventArgs e)
    {
        try
        {
            for (int i = gdReport.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = gdReport.Rows[i];
                GridViewRow previousRow = gdReport.Rows[i - 1];
                for (int j = 0; j <= 2; j++)
                {
                    Label lnlname = new Label();
                    Label lnlname1 = new Label();
                    switch (j)
                    {
                        case 0:
                            lnlname = (Label)row.FindControl("lblsno");
                            lnlname1 = (Label)previousRow.FindControl("lblsno");
                            break;
                        case 1:
                            lnlname = (Label)row.FindControl("lblclg");
                            lnlname1 = (Label)previousRow.FindControl("lblclg");
                            break;
                        case 2:
                            lnlname = (Label)row.FindControl("lblacd");
                            lnlname1 = (Label)previousRow.FindControl("lblacd");
                            break;
                        //case 3:
                        //    lnlname = (Label)row.FindControl("lblbatch");
                        //    lnlname1 = (Label)previousRow.FindControl("lblbatch");
                        //    break;
                        //case 4:
                        //    lnlname = (Label)row.FindControl("lblSem");
                        //    lnlname1 = (Label)previousRow.FindControl("lblSem");
                        //    break;
                        //case 5:
                        //    lnlname = (Label)row.FindControl("lblbutton");
                        //    lnlname1 = (Label)previousRow.FindControl("lblbutton");
                        //    break;
                    }
                    if (lnlname.Text == lnlname1.Text)
                    {
                        if (previousRow.Cells[j].RowSpan == 0)
                        {
                            if (row.Cells[j].RowSpan == 0)
                                previousRow.Cells[j].RowSpan += row.Cells[j].RowSpan + 2;
                            else
                                previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                            row.Cells[j].Visible = false;
                        }
                    }
                }
            }
            for (int i = gdReport.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = gdReport.Rows[i];
                GridViewRow previousRow = gdReport.Rows[i - 1];
                for (int j = 5; j <= 5; j++)
                {
                    Label lnlname = new Label();
                    Label lnlname1 = new Label();
                    switch (j)
                    {
                        //case 0:
                        //    lnlname = (Label)row.FindControl("lblsno");
                        //    lnlname1 = (Label)previousRow.FindControl("lblsno");
                        //    break;
                        //case 1:
                        //    lnlname = (Label)row.FindControl("lblclg");
                        //    lnlname1 = (Label)previousRow.FindControl("lblclg");
                        //    break;
                        //case 2:
                        //    lnlname = (Label)row.FindControl("lblacd");
                        //    lnlname1 = (Label)previousRow.FindControl("lblacd");
                        //    break;
                        //case 3:
                        //    lnlname = (Label)row.FindControl("lblbatch");
                        //    lnlname1 = (Label)previousRow.FindControl("lblbatch");
                        //    break;
                        //case 4:
                        //    lnlname = (Label)row.FindControl("lblSem");
                        //    lnlname1 = (Label)previousRow.FindControl("lblSem");
                        //    break;
                        case 5:
                            lnlname = (Label)row.FindControl("lblbutton");
                            lnlname1 = (Label)previousRow.FindControl("lblbutton");
                            break;
                    }
                    if (lnlname.Text == lnlname1.Text)
                    {
                        if (previousRow.Cells[j].RowSpan == 0)
                        {
                            if (row.Cells[j].RowSpan == 0)
                                previousRow.Cells[j].RowSpan += row.Cells[j].RowSpan + 2;
                            else
                                previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                            row.Cells[j].Visible = false;
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }
    protected void gdReport_OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string value = "Updat$" + e.Row.RowIndex;

                e.Row.Cells[5].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.gdReport, "Updat$" + e.Row.RowIndex);
            }
        }
        catch { }
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        // int rowindex = rowIndxClicked();
        int rowindex = ((GridViewRow)((Button)sender).Parent.Parent).RowIndex;
        if (gdReport.Rows.Count > 0)
        {
            int rowcnt = 0;
            foreach (GridViewRow gvpopro in gdReport.Rows)
            {
                if (rowindex == rowcnt)
                {
                    Label clgCode = (Label)gvpopro.Cells[1].FindControl("lblclgVal");
                    Label acdYear = (Label)gvpopro.Cells[2].FindControl("lblacd");
                    Label batch = (Label)gvpopro.Cells[2].FindControl("lblbatch");
                    Label Sem = (Label)gvpopro.Cells[2].FindControl("lblSemVal");
                    if (clgCode.Text.Trim() != "")
                    {
                        getUpdateSettings(clgCode.Text, acdYear.Text, batch.Text, Sem.Text);
                        btnRowOK.Text = "Update";
                        ViewState["clgCode"] = clgCode.Text;
                    }
                }
                rowcnt++;
            }
        }
    }
    protected void getUpdateSettings(string collegecode, string acdYears, string BatchYear, string feeCate)
    {
        tblSave.Visible = false;
        divEdit.Visible = false;
        Hashtable htPaidInsert = new Hashtable();
        //htPaidInsert.Add("@ACD_COLLEGECODE", collegecode);
        //htPaidInsert.Add("@ACD_YEAR", "");
        //htPaidInsert.Add("@ACD_BATCH_YEAR", "");
        //htPaidInsert.Add("@ACD_FEECATEGORY", 0);
        //htPaidInsert.Add("@QUERY_TYPE", 1);
        //DataSet dsPrevAMount = d2.select_method("USP_SAVE_ACADEMICYEAR", htPaidInsert, "sp");
        string settingType = string.Empty;
        if (rblType.SelectedIndex == 0)
            settingType = "0";
        else if (rblType.SelectedIndex == 1)
            settingType = "1";
        else if (rblType.SelectedIndex == 2)
            settingType = "2";

        rblTypeNew.SelectedIndex = Convert.ToInt32(settingType);
        rblTypeNew.Enabled = false;
        feeCate = feeCate.Replace(",", "','");
        string selQ = " SELECT ACD_COLLEGE_CODE,c.collname,ACD_YEAR,ACD_BATCH_YEAR,ACD_FEECATEGORY,t.textval FROM FT_ACADEMICYEAR AY,FT_ACADEMICYEAR_DETAILED AYD,collinfo c,textvaltable t WHERE c.college_code=ay.acd_college_code and AY.ACA_YEAR_PK=AYD.ACA_YEAR_FK and textcriteria='FEECA' and t.textcode=ayd.ACD_FEECATEGORY and t.college_code=ay.ACD_COLLEGE_CODE AND  ACD_COLLEGE_CODE IN('" + collegecode + "') and ACD_YEAR in('" + acdYears + "') and ACD_SETTING_TYPE='" + settingType + "'   order by ACD_COLLEGE_CODE";//and ACD_BATCH_YEAR in('" + BatchYear + "') and ACD_FEECATEGORY in('" + feeCate + "')
        DataSet dsPrevAMount = d2.select_method_wo_parameter(selQ, "Text");
        if (dsPrevAMount.Tables.Count > 0 && dsPrevAMount.Tables[0].Rows.Count > 0)
        {
            DataTable dtAcdYear = dsPrevAMount.Tables[0].DefaultView.ToTable(true, "ACD_YEAR", "ACD_COLLEGE_CODE", "collname");
            DataTable dtBatchYear = dsPrevAMount.Tables[0].DefaultView.ToTable(true, "ACD_YEAR", "ACD_BATCH_YEAR");
            if (dtBatchYear.Rows.Count > 0)
            {
                for (int sem = 0; sem < dtBatchYear.Rows.Count; sem++)//addnew reo bind here
                {
                    if (sem == 0)
                        bindSettingGrid();
                    else
                        AddNewRowToGrid();
                }
            }
            DataTable dtFeecat = dsPrevAMount.Tables[0].DefaultView.ToTable();
            if (dtAcdYear.Rows.Count > 0)
            {
                int Sno = 0;
                for (int row = 0; row < dtAcdYear.Rows.Count; row++)
                {
                    Sno++;
                    string acdYear = Convert.ToString(dtAcdYear.Rows[row]["ACD_YEAR"]);
                    dtBatchYear.DefaultView.RowFilter = "ACD_YEAR='" + acdYear + "'";
                    DataTable dtBatch = dtBatchYear.DefaultView.ToTable();
                    if (dtBatch.Rows.Count > 0)
                    {
                        for (int bat = 0; bat < dtBatch.Rows.Count; bat++)
                        {
                            string acdBatchYear = Convert.ToString(dtBatch.Rows[bat]["ACD_BATCH_YEAR"]);
                            dtFeecat.DefaultView.RowFilter = "ACD_YEAR='" + acdYear + "' and ACD_BATCH_YEAR='" + acdBatchYear + "'";
                            DataTable dtFee = dtFeecat.DefaultView.ToTable();
                            if (dtFee.Rows.Count > 0)
                            {
                                int val = gdReport.Rows.Count;
                                DropDownList ddlacdYear = (DropDownList)gdSetting.Rows[bat].FindControl("ddlAcademic");
                                DropDownList ddlBatch = (DropDownList)gdSetting.Rows[bat].FindControl("ddlBatch");
                                CheckBoxList cblSem = (CheckBoxList)gdSetting.Rows[bat].FindControl("cblSem");
                                if (cblSem.Items.Count > 0)
                                {
                                    for (int fee = 0; fee < dtFee.Rows.Count; fee++)
                                    {
                                        string feecaT = Convert.ToString(dtFee.Rows[fee]["ACD_FEECATEGORY"]);
                                        string feecaTStr = Convert.ToString(dtFee.Rows[fee]["textval"]);
                                        for (int sem = 0; sem < cblSem.Items.Count; sem++)
                                        {
                                            if (cblSem.Items[sem].Text != feecaTStr)
                                                continue;
                                            cblSem.Items[sem].Selected = true;
                                        }
                                    }
                                    ddlacdYear.SelectedIndex = ddlacdYear.Items.IndexOf(ddlacdYear.Items.FindByValue(acdYear));
                                    ddlBatch.SelectedIndex = ddlBatch.Items.IndexOf(ddlBatch.Items.FindByValue(acdBatchYear));
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public static Control GetPostBackControl(Page page)
    {
        Control control = null;
        string ctrlname = page.Request.Params.Get("__EVENTTARGET");
        if (ctrlname != null && ctrlname != string.Empty)
        {
            control = page.FindControl(ctrlname);
        }
        else
        {
            foreach (string ctl in page.Request.Form)
            {
                Control c = page.FindControl(ctl);
                if (c is System.Web.UI.WebControls.Button)
                {
                    control = c;
                    break;
                }
            }
        }
        return control;
    }
    public int rowIndxClicked()
    {
        int rownumber = -1;
        try
        {
            Control ctrlid = GetPostBackControl(this.Page);
            string rno = Convert.ToString(ctrlid.UniqueID).Split('$')[3].Replace("ctl", "");
            int.TryParse(rno, out rownumber);
            rownumber -= 2;
        }
        catch { rownumber = -1; }

        return rownumber;
    }

    //added batch and feecategory setting added here 11.08.2017
    protected void lnkSetting_Click(object sender, EventArgs e)
    {
        divEdit.Visible = true;
        bindSettingGrid();
    }
    protected void imgSetting_Click(object sender, EventArgs e)
    {
        divEdit.Visible = false;
    }
    protected void bindSettingGrid()
    {
        try
        {
            ArrayList addnew = new ArrayList();
            addnew.Add("1");
            DataTable dtSetting = new DataTable();
            dtSetting.Columns.Add("Sno");
            dtSetting.Columns.Add("Academic Year");
            dtSetting.Columns.Add("Batch");
            dtSetting.Columns.Add("Feecategory");
            DataRow dr;
            for (int row = 0; row < addnew.Count; row++)
            {
                dr = dtSetting.NewRow();
                dr[0] = addnew[row].ToString();
                dtSetting.Rows.Add(dr);
            }
            if (dtSetting.Rows.Count > 0)
            {
                ViewState["CurrentTable"] = dtSetting;
                gdSetting.DataSource = dtSetting;
                gdSetting.DataBind();
                btnAddRow.Visible = true;
                divEdit.Visible = true;
                tblSave.Visible = true;
            }
        }
        catch { }
    }
    protected void getSemSettings()
    {

        try
        {
            for (int clg = 0; clg < cblclg.Items.Count; clg++)
            {
                if (!cblclg.Items[clg].Selected)
                    continue;
                DataSet dsFee = d2.loadFeecategory(cblclg.Items[clg].Value, usercode, ref linkName);
                if (dsFee.Tables.Count > 0 && dsFee.Tables[0].Rows.Count > 0)
                    semSettings = true;
            }

        }
        catch { }
    }
    protected void gdSetting_OnDataBound(object sender, EventArgs e)
    {
        try
        {

            if (gdSetting.Rows.Count > 0)
            {
                DataSet dsBatch = new DataSet();
                dsBatch = batchLoad();
                for (int a = 0; a < gdSetting.Rows.Count; a++)
                {
                    //academic year
                    DataTable dtYear = loadAcadYear();
                    (gdSetting.Rows[a].FindControl("ddlAcademic") as DropDownList).Items.Clear();
                    if (dsBatch.Tables.Count > 0 && dsBatch.Tables[0].Rows.Count > 0)
                    {
                        (gdSetting.Rows[a].FindControl("ddlAcademic") as DropDownList).DataSource = dtYear;
                        (gdSetting.Rows[a].FindControl("ddlAcademic") as DropDownList).DataTextField = "Academic_Year";
                        (gdSetting.Rows[a].FindControl("ddlAcademic") as DropDownList).DataValueField = "Academic_Year";
                        (gdSetting.Rows[a].FindControl("ddlAcademic") as DropDownList).DataBind();
                    }
                    //(gdSetting.Rows[a].FindControl("ddlBatch") as DropDownList).Items.Insert(0, "Select");
                    //batch year
                    (gdSetting.Rows[a].FindControl("ddlBatch") as DropDownList).Items.Clear();
                    if (dsBatch.Tables.Count > 0 && dsBatch.Tables[0].Rows.Count > 0)
                    {
                        (gdSetting.Rows[a].FindControl("ddlBatch") as DropDownList).DataSource = dsBatch;
                        (gdSetting.Rows[a].FindControl("ddlBatch") as DropDownList).DataTextField = "Batch_year";
                        (gdSetting.Rows[a].FindControl("ddlBatch") as DropDownList).DataValueField = "Batch_year";
                        (gdSetting.Rows[a].FindControl("ddlBatch") as DropDownList).DataBind();
                    }
                    // (gdSetting.Rows[a].FindControl("ddlBatch") as DropDownList).Items.Insert(0, "Select");
                    //feecategory 
                    (gdSetting.Rows[a].FindControl("cblSem") as CheckBoxList).Items.Clear();
                    if (semSettings)//each college semester setting checked
                    {
                        DataSet dsTemp = loadFeecategory(linkName);
                        (gdSetting.Rows[a].FindControl("cblSem") as CheckBoxList).DataSource = dsTemp;
                        (gdSetting.Rows[a].FindControl("cblSem") as CheckBoxList).DataTextField = "TextVal";
                        (gdSetting.Rows[a].FindControl("cblSem") as CheckBoxList).DataValueField = "TextVal";
                        (gdSetting.Rows[a].FindControl("cblSem") as CheckBoxList).DataBind();
                    }
                    // (gdSetting.Rows[a].FindControl("ddlFeecat") as DropDownList).Items.Insert(0, "Select");
                }
            }
        }
        catch
        { }
    }
    public void btnAddRow_Click(object sender, EventArgs e)
    {
        try
        {
            if (gdSetting.Rows.Count > 0)
            {
                AddNewRowToGrid();
                gdSetting_OnDataBound(sender, e);
                SetPreviousData();
            }
        }
        catch
        {
        }
    }
    public void AddNewRowToGrid()
    {
        try
        {
            getSemSettings();
            int rowIndex = 0;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                DataRow drCurrentRow = null;
                DropDownList box1 = new DropDownList();
                CheckBoxList box2 = new CheckBoxList();
                DropDownList academic = new DropDownList();
                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dtCurrentTable.Rows.Count; i++)
                    {
                        //extract the TextBox values
                        academic = (DropDownList)gdSetting.Rows[i].Cells[1].FindControl("ddlAcademic");
                        box1 = (DropDownList)gdSetting.Rows[i].Cells[2].FindControl("ddlBatch");
                        box2 = (CheckBoxList)gdSetting.Rows[i].Cells[3].FindControl("cblSem");
                        drCurrentRow = dtCurrentTable.NewRow();
                        dtCurrentTable.Rows[i][0] = Convert.ToString(i + 1);
                        dtCurrentTable.Rows[i][1] = academic.Text;
                        dtCurrentTable.Rows[i][2] = box1.Text;
                        dtCurrentTable.Rows[i][3] = box2.Text;
                        rowIndex++;
                    }
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["CurrentTable"] = dtCurrentTable;
                    gdSetting.DataSource = dtCurrentTable;
                    gdSetting.DataBind();
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"View State Null\");", true);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void SetPreviousData()
    {
        try
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                Hashtable hashlist = new Hashtable();
                if (dt.Rows.Count > 0)
                {
                    DropDownList box1 = new DropDownList();
                    DropDownList academic = new DropDownList();
                    CheckBoxList box2 = new CheckBoxList();
                    Label lbl = new Label();
                    hashlist.Add(0, "Sno");
                    hashlist.Add(1, "Batch");
                    hashlist.Add(2, "Feecategory");
                    DataRow dr;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        box1 = (DropDownList)gdSetting.Rows[i].Cells[2].FindControl("ddlBatch");
                        academic = (DropDownList)gdSetting.Rows[i].Cells[1].FindControl("ddlAcademic");
                        box2 = (CheckBoxList)gdSetting.Rows[i].Cells[3].FindControl("cblSem");
                        lbl = (Label)gdSetting.Rows[i].Cells[0].FindControl("lblsno");
                        string val_file = Convert.ToString(hashlist[i]);
                        lbl.Text = Convert.ToString(i + 1);
                        string academicyear = dt.Rows[i][1].ToString();
                        string batch = dt.Rows[i][2].ToString();
                        string feecat = dt.Rows[i][3].ToString();
                        box1.SelectedIndex = box1.Items.IndexOf(box1.Items.FindByValue(Convert.ToString(dt.Rows[i][2])));
                        box2.SelectedIndex = box2.Items.IndexOf(box2.Items.FindByValue(Convert.ToString(dt.Rows[i][3])));
                        academic.SelectedIndex = academic.Items.IndexOf(academic.Items.FindByValue(Convert.ToString(dt.Rows[i][1])));
                        rowIndex++;
                    }
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected DataSet batchLoad()
    {
        DataSet dsBatch = new DataSet();
        try
        {
            string strsql = "select distinct batch_year from applyn where batch_year<>'-1' and batch_year<>''order by batch_year desc";
            dsBatch = d2.select_method_wo_parameter(strsql, "Text");
        }
        catch { dsBatch.Clear(); }
        return dsBatch;
    }
    protected DataTable loadAcadYear()
    {
        DataTable dtYEar = new DataTable();
        try
        {
            DataSet dsAcd = batchLoad();
            dtYEar.Columns.Add("Academic_Year");
            DataRow drYEar;
            if (dsAcd.Tables.Count > 0 && dsAcd.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsAcd.Tables[0].Rows.Count; row++)
                {
                    int yeaR = 0;
                    int.TryParse(Convert.ToString(dsAcd.Tables[0].Rows[row]["batch_year"]), out yeaR);
                    drYEar = dtYEar.NewRow();
                    drYEar["Academic_Year"] = Convert.ToString(yeaR) + "-" + Convert.ToString(++yeaR);
                    dtYEar.Rows.Add(drYEar);
                }
            }
        }
        catch { }
        return dtYEar;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        getSemSettings();//semester settings
        DataSet dsVal = loadFeecatVal(linkName);
        Hashtable htSemCode = new Hashtable();
        if (dsVal.Tables.Count > 0 && dsVal.Tables[0].Rows.Count > 0)
        {
            htSemCode = htSem(dsVal);
        }
        bool save = getSettings(htSemCode);
        if (save)
        {
            divEdit.Visible = false;
            btnGo_Click(sender, e);
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
        }
        else
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Not Saved')", true);
    }
    protected void btnDel_Click(object sender, EventArgs e)
    {
        getDelete();
    }
    protected ArrayList getClg()
    {
        ArrayList arClg = new ArrayList();
        try
        {
            string clgCode = string.Empty;
            if (btnRowOK.Text == "Save")
            {
                for (int clg = 0; clg < cblclg.Items.Count; clg++)
                {
                    if (!cblclg.Items[clg].Selected)
                        continue;
                    clgCode = Convert.ToString(cblclg.Items[clg].Value);
                    arClg.Add(clgCode);

                }
            }
            else
            {
                if (ViewState["clgCode"] != null)
                {
                    clgCode = Convert.ToString(ViewState["clgCode"]);
                    arClg.Add(clgCode);
                }
            }

        }
        catch { }
        return arClg;

    }
    protected bool getSettings(Hashtable htSemCode)
    {
        bool boolSave = false;
        Dictionary<string, string> dtFeecat = new Dictionary<string, string>();
        try
        {
            string settingType = string.Empty;
            if (rblTypeNew.SelectedIndex == 0)
                settingType = "0";
            else if (rblTypeNew.SelectedIndex == 1)
                settingType = "1";
            else if (rblTypeNew.SelectedIndex == 2)
                settingType = "2";
            string clgCodes = string.Empty;
            ArrayList arValidate = new ArrayList();
            ArrayList getClgCode = getClg();
            //  for (int clg = 0; clg < cblclg.Items.Count; clg++)
            foreach (string clgCode in getClgCode)
            {
                //{
                //if (!cblclg.Items[clg].Selected)
                //    continue;
                //string clgCode = Convert.ToString(cblclg.Items[clg].Value);

                foreach (GridViewRow gdRow in gdSetting.Rows)
                {
                    StringBuilder sbSem = new StringBuilder();
                    DropDownList acadYear = (DropDownList)gdRow.FindControl("ddlAcademic");
                    DropDownList ddlBatch = (DropDownList)gdRow.FindControl("ddlBatch");
                    CheckBoxList cblSem = (CheckBoxList)gdRow.FindControl("cblSem");
                    if (acadYear.Items.Count > 0 && ddlBatch.Items.Count > 0)
                    {
                        string academicYear = Convert.ToString(acadYear.SelectedItem.Text);
                        string batch = Convert.ToString(ddlBatch.SelectedItem.Text);
                        for (int row = 0; row < cblSem.Items.Count; row++)
                        {
                            if (!cblSem.Items[row].Selected)
                                continue;
                            string semVal = string.Empty;
                            string semStr = Convert.ToString(cblSem.Items[row].Text);
                            if (htSemCode.ContainsKey(Convert.ToString(clgCode + "-" + semStr)))
                                semVal = Convert.ToString(htSemCode[Convert.ToString(clgCode + "-" + semStr)]);
                            if (!arValidate.Contains(clgCode + "-" + academicYear + "-" + batch + "-" + semStr))
                            {
                                Hashtable htPaidInsert = new Hashtable();
                                htPaidInsert.Add("@ACD_COLLEGECODE", clgCode);
                                htPaidInsert.Add("@ACD_YEAR", academicYear);
                                htPaidInsert.Add("@ACD_BATCH_YEAR", batch);
                                htPaidInsert.Add("@ACD_FEECATEGORY", semVal);
                                htPaidInsert.Add("@QUERY_TYPE", 0);
                                htPaidInsert.Add("@ACD_SETTING_TYPE", settingType);
                                int insert = d2.insert_method("USP_SAVE_ACADEMICYEAR", htPaidInsert, "sp");
                                if (insert > 0)
                                    boolSave = true;
                                arValidate.Add(clgCode + "-" + academicYear + "-" + batch + "-" + semStr);
                            }
                        }
                    }
                }
                //ViewState["clgCode"] = null;
            }
        }
        catch { }
        return boolSave;
    }

    public DataSet loadFeecategory(string linkName)
    {
        DataSet dsset = new DataSet();
        try
        {
            string SelectQ = string.Empty;
            if (linkName == "SemesterandYear")
            {
                SelectQ = "select distinct textval from textvaltable where TextCriteria = 'FEECA'and (textval like '%Semester' or textval like '%Year') and textval not like '-1%' and college_code in('" + collegecode + "') --order by len(textval),textval asc";
                dsset.Clear();
                dsset = d2.select_method_wo_parameter(SelectQ, "Text");
            }
            else if (linkName == "Semester")
            {
                SelectQ = "select distinct textval from textvaltable where TextCriteria = 'FEECA'and textval like '%Semester' and textval not like '-1%' and college_code in('" + collegecode + "')-- order by len(textval),textval asc";
                dsset.Clear();
                dsset = d2.select_method_wo_parameter(SelectQ, "Text");
            }
            else if (linkName == "Year")
            {
                SelectQ = "select distinct textval from textvaltable where TextCriteria = 'FEECA'and textval like '%Year' and textval not like '-1%' and college_code  in('" + collegecode + "') --order by len(textval),textval asc";
                dsset.Clear();
                dsset = d2.select_method_wo_parameter(SelectQ, "Text");
            }
            else if (linkName == "Term")
            {
                SelectQ = "select distinct textval from textvaltable t,Fee_degree_match f where t.textcode=f.feecategory and t.college_code=f.college_code and  TextCriteria = 'FEECA' and textval like '%Term%' and textval not like '-1%' and t.college_code in('" + collegecode + "') ";
                SelectQ += " --order by len(textval),textval asc";
            }
            dsset.Clear();
            dsset = d2.select_method_wo_parameter(SelectQ, "Text");

        }
        catch { dsset.Clear(); }
        return dsset;
    }

    public DataSet loadFeecatVal(string linkName)
    {
        DataSet dsset = new DataSet();
        try
        {
            string SelectQ = string.Empty;
            if (linkName == "SemesterandYear")
            {
                SelectQ = "select distinct textval,textcode,college_code from textvaltable where TextCriteria = 'FEECA'and (textval like '%Semester' or textval like '%Year') and textval not like '-1%' and college_code in('" + collegecode + "') --order by len(textval),textval asc";
                dsset.Clear();
                dsset = d2.select_method_wo_parameter(SelectQ, "Text");
            }
            else if (linkName == "Semester")
            {
                SelectQ = "select distinct textval,textcode,college_code from textvaltable where TextCriteria = 'FEECA'and textval like '%Semester' and textval not like '-1%' and college_code in('" + collegecode + "')-- order by len(textval),textval asc";
                dsset.Clear();
                dsset = d2.select_method_wo_parameter(SelectQ, "Text");
            }
            else if (linkName == "Year")
            {
                SelectQ = "select distinct textval,textcode,college_code from textvaltable where TextCriteria = 'FEECA'and textval like '%Year' and textval not like '-1%' and college_code  in('" + collegecode + "') --order by len(textval),textval asc";
                dsset.Clear();
                dsset = d2.select_method_wo_parameter(SelectQ, "Text");
            }
            else if (linkName == "Term")
            {
                SelectQ = "select distinct textval,textcode,t.college_code from textvaltable t,Fee_degree_match f where t.textcode=f.feecategory and t.college_code=f.college_code and  TextCriteria = 'FEECA' and textval like '%Term%' and textval not like '-1%' and t.college_code in('" + collegecode + "') ";
                SelectQ += " --order by len(textval),textval asc";
            }
            dsset.Clear();
            dsset = d2.select_method_wo_parameter(SelectQ, "Text");
        }
        catch { dsset.Clear(); }
        return dsset;
    }
    protected Hashtable htSem(DataSet ds)
    {
        Hashtable htSemCode = new Hashtable();
        try
        {
            DataTable dtSem = new DataTable();
            for (int clg = 0; clg < cblclg.Items.Count; clg++)
            {
                if (!cblclg.Items[clg].Selected)
                    continue;
                string clgCode = Convert.ToString(cblclg.Items[clg].Value);
                ds.Tables[0].DefaultView.RowFilter = " college_code='" + clgCode + "'";
                dtSem = ds.Tables[0].DefaultView.ToTable();
                for (int row = 0; row < dtSem.Rows.Count; row++)
                {
                    string semCode = Convert.ToString(dtSem.Rows[row]["textcode"]);
                    string semStr = Convert.ToString(dtSem.Rows[row]["textval"]);
                    htSemCode.Add(clgCode + "-" + semStr, semCode);
                }
            }
        }
        catch { }
        return htSemCode;
    }
    protected void getDelete()
    {
        try
        {
            string settingType = string.Empty;
            if (rblTypeNew.SelectedIndex == 0)
                settingType = "0";
            else if (rblTypeNew.SelectedIndex == 1)
                settingType = "1";
            else if (rblTypeNew.SelectedIndex == 2)
                settingType = "2";
            bool boolSave = false;
            for (int clg = 0; clg < cblclg.Items.Count; clg++)
            {
                if (!cblclg.Items[clg].Selected)
                    continue;
                string clgCode = Convert.ToString(cblclg.Items[clg].Value);
                string DelQ = " delete from FT_ACADEMICYEAR_DETAILED where aca_year_fk in(select aca_year_pk from FT_ACADEMICYEAR where acd_college_code='" + clgCode + "') and ACD_SETTING_TYPE='" + settingType + "' ";
                int updVal = d2.update_method_wo_parameter(DelQ, "Text");
                try
                {
                    DelQ = " delete from FT_ACADEMICYEAR where acd_college_code ='" + clgCode + "'";
                    updVal = d2.update_method_wo_parameter(DelQ, "Text");
                }
                catch { }
                boolSave = true;
            }
            if (boolSave)
            {
                divEdit.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Deleted Successfully')", true);
            }
        }
        catch { }
    }

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
    private string getCblSelectedTempText(CheckBoxList cblSelected)
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

    //added by sudhagar 24.10.2017
    protected void rblType_Selected(object sender, EventArgs e)
    {
        gdReport.Visible = false;
    }

}