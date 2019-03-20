using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Configuration;

public partial class LeaveApplySettings : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string usercode = string.Empty;
    bool cellclick = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("StudentHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/StudentMod/StudentHome.aspx");
                    return;
                }
            }
            //****************************************************//

            usercode = Session["usercode"].ToString();
            // collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();

            if (!IsPostBack)
            {
                setLabelText();
                loadcollege();
                BindDepartment();
                if (cblclg.Items.Count > 0)
                {
                    collegecode = Convert.ToString(getCblSelectedValue(cblclg));
                }
            }
            if (cblclg.Items.Count > 0)
            {
                collegecode = Convert.ToString(getCblSelectedValue(cblclg));
            }
        }
        catch (Exception ex) { }
    }

    #region College
    public void loadcollege()
    {
        try
        {
            cblclg.Items.Clear();
            ds.Clear();
            ds = d2.BindCollege();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblclg.DataSource = ds;
                cblclg.DataTextField = "collname";
                cblclg.DataValueField = "college_code";
                cblclg.DataBind();
                for (int i = 0; i < cblclg.Items.Count; i++)
                {
                    cblclg.Items[i].Selected = true;
                }
                cbclg.Checked = true;
                txtclg.Text = lblclg.Text + "(" + cblclg.Items.Count + ")";
            }
        }
        catch
        { }
    }
    public void cbclg_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cbclg, cblclg, txtclg, lblclg.Text, "--Select--");
        BindDepartment();

    }
    public void cblclg_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbclg, cblclg, txtclg, lblclg.Text, "--Select--");
        BindDepartment();

    }

    #endregion

    #region add clg
    public void loadddlcollege()
    {
        try
        {
            addddlclg.Items.Clear();
            ds.Clear();
            ds = d2.BindCollege();
            if (ds.Tables[0].Rows.Count > 0)
            {
                addddlclg.DataSource = ds;
                addddlclg.DataTextField = "collname";
                addddlclg.DataValueField = "college_code";
                addddlclg.DataBind();
            }
        }
        catch
        { }
    }
    public void loadCblcollege()
    {
        try
        {
            addcblhedg.Items.Clear();
            ds.Clear();
            ds = d2.BindCollege();
            if (ds.Tables[0].Rows.Count > 0)
            {
                addcblhedg.DataSource = ds;
                addcblhedg.DataTextField = "collname";
                addcblhedg.DataValueField = "college_code";
                addcblhedg.DataBind();
                for (int i = 0; i < addcblhedg.Items.Count; i++)
                {
                    addcblhedg.Items[i].Selected = true;
                }
                addcbhedg.Checked = true;
                addtxtclg.Text = addlblclg.Text + "(" + addcblhedg.Items.Count + ")";
            }
        }
        catch
        { }
    }
    public void addcbhedg_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(addcbhedg, addcblhedg, addtxtclg, addlblclg.Text, "--Select--");
        BindDepartment2();
    }
    public void addcblhedg_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(addcbhedg, addcblhedg, addtxtclg, addlblclg.Text, "--Select--");
        BindDepartment2();
        string collegeCode = getCblSelectedValue(addcblhedg);
        if (IsFinanceInclude(collegeCode))
        {
            chkIncFinance.Checked = true;
        }
        else
        {
            chkIncFinance.Checked = false;
        }
        chkIncFinance_CheckedChange(new object(), new EventArgs());
    }
    #endregion

    #region print

    public void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            lblvalidation1.Text = "";
            string degreedetails = "";
            string pagename = "";
            degreedetails = "Leave Settings Report";
            pagename = "LeaveApplySettings.aspx";
            Printcontrolhed.loadspreaddetails(fpreport, pagename, degreedetails);
            Printcontrolhed.Visible = true;
        }
        catch { }
    }


    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.Trim() != "")
            {
                d2.printexcelreport(fpreport, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Leave Settings Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        { }

    }
    #endregion


    #region spread

    protected void fpreport_OnCellClick(object sender, EventArgs e)
    {
        cellclick = true;
    }
    protected void fpreport_Selectedindexchanged(object sender, EventArgs e)
    {
        try
        {
            if (cellclick == true)
            {
                string actrow = fpreport.ActiveSheetView.ActiveRow.ToString();
                string actcol = fpreport.ActiveSheetView.ActiveColumn.ToString();
                if (!string.IsNullOrEmpty(actrow))
                {
                    int arow = Convert.ToInt32(actrow);
                    int acol = Convert.ToInt32(actcol);

                    string clgcode = Convert.ToString(fpreport.Sheets[0].Cells[arow, 2].Tag);
                    string degcode = Convert.ToString(fpreport.Sheets[0].Cells[arow, 6].Tag);
                    string isFin = Convert.ToString(fpreport.Sheets[0].Cells[arow, 7].Tag).Trim();
                    string headerFK = Convert.ToString(fpreport.Sheets[0].Cells[arow, 3].Tag).Trim();
                    string ledgerFK = Convert.ToString(fpreport.Sheets[0].Cells[arow, 4].Tag).Trim();

                    if (!string.IsNullOrEmpty(degcode))
                    {

                        string SelQ = " SELECT SLSettingPK, DegreeCode, IsFinance, HeaderFK, LegerFK, MaxLeave, CollegeCode, FromDay ,ToDay ,Amount FROM AM_Student_Leave_Settings S,AM_Student_Leave_Settings_Det SD WHERE SLSettingPK = SLSettingFK AND CollegeCode ='" + clgcode + "' AND DegreeCode ='" + degcode + "'";
                        SelQ += " select collname,college_code from collinfo";
                        DataSet dsNew = new DataSet();
                        dsNew = d2.select_method_wo_parameter(SelQ, "Text");
                        #region Load Department
                        rbmode.SelectedIndex = 0;
                        rbmode_OnSelected(new object(), new EventArgs());
                        BindDepartment2();
                        cb_dept2.Checked = false;

                        for (int i = 0; i < cbl_dept2.Items.Count; i++)
                        {
                            if (cbl_dept2.Items[i].Value == degcode)
                            {
                                cbl_dept2.Items[i].Selected = true;
                                txtDept2.Text = lblDept2.Text + "(" + cbl_dept2.Items[i].Text + ")";
                            }
                            else
                            {
                                cbl_dept2.Items[i].Selected = false;
                            }
                        }
                        #endregion

                        clearAddScreen();
                        btnSaveLeaveSet.Visible = false;
                        btnUpdateLeaveSet.Visible = true;
                        btnDeleteLeaveSet.Visible = true;

                        if (isFin == "1")
                        {
                            chkIncFinance.Checked = true;
                            chkIncFinance_CheckedChange(new object(), new EventArgs());

                            ddlFinHeader.SelectedIndex = ddlFinHeader.Items.IndexOf(ddlFinHeader.Items.FindByValue(headerFK));
                            loadFinLedger();
                            ddlFinLedger.SelectedIndex = ddlFinLedger.Items.IndexOf(ddlFinLedger.Items.FindByValue(ledgerFK));
                        }

                        if (dsNew.Tables.Count > 0 && dsNew.Tables[0].Rows.Count > 0)
                        {
                            rbmode.SelectedIndex = 0;
                            addddlclg.Items.Clear();
                            if (dsNew.Tables[1].Rows.Count > 0)
                            {
                                DataView dv = new DataView();
                                dsNew.Tables[1].DefaultView.RowFilter = "college_code='" + clgcode + "'";
                                dv = dsNew.Tables[1].DefaultView;
                                if (dv.Count > 0)
                                    addddlclg.Items.Add(new ListItem(Convert.ToString(dv[0]["collname"]), clgcode));

                                txtMaxLeaveSet.Text = Convert.ToString(dsNew.Tables[0].Rows[0]["MaxLeave"]);

                                DataTable dtFineGrid = new DataTable();
                                dtFineGrid.Columns.Add("DaysFrom");
                                dtFineGrid.Columns.Add("DaysTo");
                                dtFineGrid.Columns.Add("Amount");
                                for (int cnt = 0; cnt < dsNew.Tables[0].Rows.Count; cnt++)
                                {
                                    dtFineGrid.Rows.Add(Convert.ToString(dsNew.Tables[0].Rows[cnt]["FromDay"]), Convert.ToString(dsNew.Tables[0].Rows[cnt]["ToDay"]), Convert.ToString(dsNew.Tables[0].Rows[cnt]["Amount"]));
                                }
                                BindGrid(dtFineGrid);
                                divadd.Visible = true;
                                tdddl.Visible = true;
                            }
                        }
                    }
                }
            }
        }
        catch { }
    }

    #endregion

    #region Button go
    protected void btngo_Click(object sender, EventArgs e)
    {

        ds.Clear();
        ds = loadDataset();
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            loadSpread();
        }
        else
        {
            fpreport.Visible = false;
            print.Visible = false;
            imgalert.Visible = true;
            lbl_alert.Text = "No Record Found";
        }
    }

    protected DataSet loadDataset()
    {
        DataSet dsload = new DataSet();
        try
        {
            string clgcode = Convert.ToString(getCblSelectedValue(cblclg));
            string degcode = Convert.ToString(getCblSelectedValue(cbl_dept));
            string SelQ = " SELECT SLSettingPK, DegreeCode, IsFinance, HeaderFK, LegerFK, MaxLeave, CollegeCode, FromDay ,ToDay ,Amount FROM AM_Student_Leave_Settings S,AM_Student_Leave_Settings_Det SD WHERE SLSettingPK = SLSettingFK AND CollegeCode in (" + clgcode + ") AND DegreeCode  in (" + degcode + ") ORDER BY DegreeCode ASC";
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(SelQ, "Text");
        }
        catch { }
        return dsload;
    }
    protected void loadSpread()
    {
        try
        {
            #region design

            fpreport.Sheets[0].RowCount = 0;
            fpreport.Sheets[0].ColumnCount = 0;
            fpreport.CommandBar.Visible = false;
            fpreport.Sheets[0].AutoPostBack = true;
            fpreport.Sheets[0].ColumnHeader.RowCount = 1;
            fpreport.Sheets[0].RowHeader.Visible = false;
            fpreport.Sheets[0].ColumnCount = 8;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            fpreport.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            fpreport.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            fpreport.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            fpreport.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            fpreport.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            fpreport.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            fpreport.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            fpreport.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            fpreport.Sheets[0].Columns[0].Locked = true;
            fpreport.Sheets[0].Columns[0].Width = 30;

            fpreport.Sheets[0].ColumnHeader.Cells[0, 1].Text = lblclg.Text;
            fpreport.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            fpreport.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            fpreport.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            fpreport.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            fpreport.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            fpreport.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
            fpreport.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
            fpreport.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);

            fpreport.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Department";
            fpreport.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            fpreport.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            fpreport.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            fpreport.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            fpreport.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            fpreport.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            fpreport.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
            fpreport.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);

            fpreport.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Maximum Leave";
            fpreport.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            fpreport.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            fpreport.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            fpreport.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            fpreport.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            fpreport.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;

            fpreport.Sheets[0].ColumnHeader.Cells[0, 4].Text = "From (Days)";
            fpreport.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
            fpreport.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            fpreport.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            fpreport.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            fpreport.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            fpreport.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;

            fpreport.Sheets[0].ColumnHeader.Cells[0, 5].Text = "To (Days)";
            fpreport.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
            fpreport.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            fpreport.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            fpreport.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            fpreport.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            fpreport.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;

            fpreport.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Fine Amount";
            fpreport.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
            fpreport.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            fpreport.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            fpreport.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            fpreport.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            fpreport.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;

            fpreport.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Finance";
            fpreport.Sheets[0].ColumnHeader.Cells[0, 7].ForeColor = ColorTranslator.FromHtml("#000000");
            fpreport.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
            fpreport.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
            fpreport.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
            fpreport.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
            fpreport.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Left;

            #endregion

            #region value
            for (int sel = 0; sel < ds.Tables[0].Rows.Count; sel++)
            {
                fpreport.Sheets[0].RowCount++;
                fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sel + 1);
                string collegeCode = Convert.ToString(ds.Tables[0].Rows[sel]["collegecode"]);
                string collegeName = cblclg.Items.Count > 0 ? cblclg.Items[cblclg.Items.IndexOf(cblclg.Items.FindByValue(collegeCode))].Text : string.Empty;

                string degCode = Convert.ToString(ds.Tables[0].Rows[sel]["DEGREECODE"]).Trim();
                string deptName = cbl_dept.Items.Count > 0 ? cbl_dept.Items[cbl_dept.Items.IndexOf(cbl_dept.Items.FindByValue(degCode))].Text : string.Empty;//d2.GetFunction("select  department.dept_name,degree.degree_code,department.dept_code from degree,department where  department.dept_code=degree.dept_code  and department.college_code = degree.college_code and degree.Degree_Code = " + degCode + "").Trim();

                string isFin = Convert.ToString(ds.Tables[0].Rows[sel]["IsFinance"]).Trim().ToUpper();
                byte isFinVal = 0;
                string headerFk = "0";
                string ledgerFK = "0";
                if (isFin == "1" || isFin == "TRUE")
                {
                    isFin = "Included";
                    isFinVal = 1;

                    headerFk = Convert.ToString(ds.Tables[0].Rows[sel]["HeaderFK"]).Trim();
                    ledgerFK = Convert.ToString(ds.Tables[0].Rows[sel]["LegerFK"]).Trim();
                }
                else
                {
                    isFin = "Not Included";
                }

                fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 1].Text = collegeName;
                fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 2].Text = deptName;
                fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[sel]["MaxLeave"]);
                fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[sel]["FromDay"]);
                fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[sel]["ToDay"]);
                fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[sel]["Amount"]);
                fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 7].Text = isFin;

                fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 2].Tag = collegeCode;
                fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 6].Tag = degCode;
                fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 3].Tag = headerFk;
                fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 4].Tag = ledgerFK;
                fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 7].Tag = isFinVal;

            }

            fpreport.Sheets[0].PageSize = fpreport.Sheets[0].RowCount;
            //fpreport.SaveChanges();
            divspread.Visible = true;
            fpreport.Visible = true;
            print.Visible = true;
            #endregion

        }
        catch { }
    }

    #endregion

    #region button add
    protected void btnadd_Click(object sender, EventArgs e)
    {
        divadd.Visible = true;
        rbmode.SelectedIndex = 0;
        tdddl.Visible = true;

        loadddlcollege();
        loadCblcollege();
        BindDepartment2();
        clearAddScreen();
        if (IsFinanceInclude(addddlclg.Items.Count > 0 ? addddlclg.SelectedValue : "13"))
        {
            chkIncFinance.Checked = true;
        }
        else
        {
            chkIncFinance.Checked = false;
        }
        chkIncFinance_CheckedChange(new object(), new EventArgs());
    }
    protected void rbmode_OnSelected(object sender, EventArgs e)
    {
        string collegeCode = string.Empty;
        if (rbmode.SelectedIndex == 0)
        {
            tdddl.Visible = true;
            tdcbl.Visible = false;
            //txtsms.Text = "";
            //txtuserid.Text = "";
            //txtsendid.Text = "";
            //txtpass.Text = "";
            collegeCode = addddlclg.Items.Count > 0 ? addddlclg.SelectedValue : "13";
        }
        else
        {
            tdddl.Visible = false;
            tdcbl.Visible = true;
            loadCblcollege();
            collegeCode = getCblSelectedValue(addcblhedg);
        }
        BindDepartment2();
        if (IsFinanceInclude(collegeCode))
        {
            chkIncFinance.Checked = true;
        }
        else
        {
            chkIncFinance.Checked = false;
        }
        chkIncFinance_CheckedChange(new object(), new EventArgs());
    }

    protected void addddlclg_IndexChange(object sender, EventArgs e)
    {
        string collegeCode = addddlclg.Items.Count > 0 ? addddlclg.SelectedValue : "13";

        BindDepartment2();
        if (IsFinanceInclude(collegeCode))
        {
            chkIncFinance.Checked = true;
        }
        else
        {
            chkIncFinance.Checked = false;
        }
        chkIncFinance_CheckedChange(new object(), new EventArgs());
    }

    #endregion

    protected void imagepopclose_click(object sender, EventArgs e)
    {
        divadd.Visible = false;
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgalert.Visible = false;
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
                        selectedvalue.Append("," + Convert.ToString(cblSelected.Items[sel].Value));
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
        fields.Add(0);

        lbl.Add(addlblclg);
        fields.Add(0);

        lbl.Add(lblDept);
        fields.Add(3);

        lbl.Add(lblDept2);
        fields.Add(3);

        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }

    protected void cb_dept_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_dept, cbl_dept, txt_dept, lblDept.Text, "--Select--");
    }
    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_dept, cbl_dept, txt_dept, lblDept.Text, "--Select--");
    }
    private void BindDepartment()
    {
        try
        {
            cbl_dept.Items.Clear();
            string collegeCode = getCblSelectedValue(cblclg);
            string query = "select distinct degree.degree_code,department.dept_name,department.dept_code from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.college_code in (" + collegeCode + ")  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + usercode + " --  and degree.course_id in(degree) ";
            DataSet dsBranch = d2.select_method_wo_parameter(query, "Text");
            if (dsBranch.Tables.Count > 0 && dsBranch.Tables[0].Rows.Count > 0)
            {
                cbl_dept.DataSource = dsBranch;
                cbl_dept.DataTextField = "dept_name";
                cbl_dept.DataValueField = "degree_code";
                cbl_dept.DataBind();
                cb_dept.Checked = true;

            }
            else
            {
                cb_dept.Checked = false;
            }

        }
        catch { cb_dept.Checked = false; }
        CallCheckboxChange(cb_dept, cbl_dept, txt_dept, lblDept.Text, "--Select--");
    }
    protected void cb_dept2_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_dept2, cbl_dept2, txtDept2, lblDept2.Text, "--Select--");
    }
    protected void cbl_dept2_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_dept2, cbl_dept2, txtDept2, lblDept2.Text, "--Select--");
    }
    private void BindDepartment2()
    {
        try
        {
            cbl_dept2.Items.Clear();
            string collegeCode = addddlclg.Items.Count > 0 ? addddlclg.SelectedValue : "13";
            if (rbmode.SelectedIndex == 1)
            {
                collegeCode = getCblSelectedValue(addcblhedg);
            }
            string query = "select distinct degree.degree_code,department.dept_name,department.dept_code from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.college_code in (" + collegeCode + ")  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + usercode + " --  and degree.course_id in(degree) ";
            DataSet dsBranch = d2.select_method_wo_parameter(query, "Text");
            if (dsBranch.Tables.Count > 0 && dsBranch.Tables[0].Rows.Count > 0)
            {
                cbl_dept2.DataSource = dsBranch;
                cbl_dept2.DataTextField = "dept_name";
                cbl_dept2.DataValueField = "degree_code";
                cbl_dept2.DataBind();
                cb_dept2.Checked = true;

            }
            else
            {
                cb_dept2.Checked = false;
            }

        }
        catch { cb_dept2.Checked = false; }
        CallCheckboxChange(cb_dept2, cbl_dept2, txtDept2, lblDept2.Text, "--Select--");
    }

    private void clearAddScreen()
    {
        txtMaxLeaveSet.Text = string.Empty;
        txtFineSet.Text = string.Empty;
        fineSetGrid.DataSource = null;
        fineSetGrid.DataBind();
        chkIncFinance.Checked = false;
        chkIncFinance_CheckedChange(new object(), new EventArgs());
        btnSaveLeaveSet.Visible = true;
        btnUpdateLeaveSet.Visible = false;
        btnDeleteLeaveSet.Visible = false;
    }
    protected void btnAddFineRow_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtFineGrid = new DataTable();
            dtFineGrid.Columns.Add("DaysFrom");
            dtFineGrid.Columns.Add("DaysTo");
            dtFineGrid.Columns.Add("Amount");
            for (int cnt = 0; cnt < Convert.ToInt16(txtFineSet.Text); cnt++)
            {
                dtFineGrid.Rows.Add("", "");
            }
            BindGrid(dtFineGrid);
            txtFineSet.Text = string.Empty;
        }
        catch { }
    }
    private void BindGrid(DataTable dtFineGrid)
    {
        fineSetGrid.DataSource = dtFineGrid;
        fineSetGrid.DataBind();
    }
    protected void chkIncFinance_CheckedChange(object sender, EventArgs e)
    {
        loadFinHeader();
        if (chkIncFinance.Checked)
        {
            lblFinHeader.Visible = true;
            lblFinLedger.Visible = true;
            ddlFinHeader.Visible = true;
            ddlFinLedger.Visible = true;
        }
        else
        {
            lblFinHeader.Visible = false;
            lblFinLedger.Visible = false;
            ddlFinHeader.Visible = false;
            ddlFinLedger.Visible = false;
        }
    }
    protected void ddlFinHeader_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            loadFinLedger();
        }
        catch (Exception ex)
        {
        }
    }
    private void loadFinHeader()
    {
        try
        {
            //string usercode = Convert.ToString(Session["usercode"]);
            string collegecodeNew = "13";
            if (rbmode.SelectedIndex == 0)
                collegecodeNew = Convert.ToString(addddlclg.SelectedItem.Value);
            else
                collegecodeNew = Convert.ToString(getCblSelectedValue(addcblhedg));

            ddlFinHeader.Items.Clear();

            string query = "SELECT distinct HeaderPK,HeaderName FROM FM_HeaderMaster H WHERE CollegeCode = " + collegecodeNew + "";
            ds = d2.select_method_wo_parameter(query, "Text");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlFinHeader.DataSource = ds;
                ddlFinHeader.DataTextField = "HeaderName";
                ddlFinHeader.DataValueField = "HeaderPK";
                ddlFinHeader.DataBind();
                loadFinLedger();
            }
        }
        catch (Exception ex)
        {
        }
    }
    private void loadFinLedger()
    {

        try
        {
            //string usercode = Convert.ToString(Session["usercode"]);
            string collegecodeNew = "13";

            if (rbmode.SelectedIndex == 0)
                collegecodeNew = Convert.ToString(addddlclg.SelectedItem.Value);
            else
                collegecodeNew = Convert.ToString(getCblSelectedValue(addcblhedg));

            ddlFinLedger.Items.Clear();

            if (ddlFinHeader.Items.Count > 0)
            {
                string query = "SELECT distinct LedgerPK,LedgerName FROM FM_LedgerMaster L WHERE  l.LedgerMode=0   AND L.CollegeCode = " + collegecodeNew + " and L.HeaderFK in (" + Convert.ToString(ddlFinHeader.SelectedItem.Value) + ")";

                ds = d2.select_method_wo_parameter(query, "Text");

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlFinLedger.DataSource = ds;
                    ddlFinLedger.DataTextField = "LedgerName";
                    ddlFinLedger.DataValueField = "LedgerPK";
                    ddlFinLedger.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnSaveLeaveSet_Click(object sender, EventArgs e)
    {
        SaveUpdateLeaveSettings();
        //btngo_Click(sender, e);
    }
    protected void btnUpdateLeaveSet_Click(object sender, EventArgs e)
    {
        SaveUpdateLeaveSettings();
        btngo_Click(sender, e);
    }
    private void SaveUpdateLeaveSettings()
    {
        int InsUpCnt = 0;
        string validation = string.Empty;
        try
        {
            if (fineSetGrid.Rows.Count > 0)
            {
                DataSet dsDegCol = d2.select_method_wo_parameter("select college_code,Degree_Code from degree ", "Text");
                Hashtable htDegCol = new Hashtable();
                if (dsDegCol.Tables.Count > 0 && dsDegCol.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsDegCol.Tables[0].Rows.Count; i++)
                    {
                        htDegCol.Add(Convert.ToString(dsDegCol.Tables[0].Rows[i][1]), Convert.ToString(dsDegCol.Tables[0].Rows[i][0]));
                    }
                }

                int maxLeaveDays = 0;
                Int32.TryParse(txtMaxLeaveSet.Text.Trim(), out maxLeaveDays);

                bool IsFinOk = true;
                byte IsFinance = 0;
                int HeaderFk = ddlFinHeader.Items.Count > 0 ? Convert.ToInt32(ddlFinHeader.SelectedValue) : 0;
                int LedgerFk = ddlFinLedger.Items.Count > 0 ? Convert.ToInt32(ddlFinLedger.SelectedValue) : 0;
                if (chkIncFinance.Checked)
                {
                    IsFinance = 1;
                    if (HeaderFk == 0 || LedgerFk == 0)
                    {
                        IsFinOk = false;
                    }
                }

                string collegeCode = "";
                if (rbmode.SelectedIndex == 0)
                    collegeCode = Convert.ToString(addddlclg.SelectedItem.Value);
                else
                    collegeCode = Convert.ToString(getCblSelectedValue(addcblhedg));

                string degCode = Convert.ToString(getCblSelectedValue(cbl_dept2));

                bool gridValidation = true;
                #region Grid Validation
                foreach (GridViewRow gRow in fineSetGrid.Rows)
                {
                    double amount = 0;
                    int FromDays = 0;
                    int ToDays = 0;

                    TextBox txtFrom = (TextBox)gRow.FindControl("txtFineDaysFrom");
                    TextBox txtTo = (TextBox)gRow.FindControl("txtFineDaysTo");
                    TextBox txtAmt = (TextBox)gRow.FindControl("txtFineAmount");

                    int.TryParse(txtFrom.Text, out FromDays);
                    int.TryParse(txtTo.Text, out ToDays);
                    double.TryParse(txtAmt.Text, out amount);

                    if (amount > 0 && FromDays > 0 && ToDays > 0)
                    {
                    }
                    else
                    {
                        gridValidation = false;
                    }
                }
                #endregion

                if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(degCode) && maxLeaveDays > 0 && IsFinOk && gridValidation)
                {
                    string[] splval = degCode.Split(',');
                    if (splval.Length > 0)
                    {
                        for (int i = 0; i < splval.Length; i++)
                        {
                            string degcode = splval[i];
                            string collCode = htDegCol[degcode].ToString();

                            string SLSettingPK = d2.GetFunction("SELECT SLSettingPK FROM AM_Student_Leave_Settings WHERE DegreeCode ='" + degcode + "' AND CollegeCode='" + collCode + "'").Trim();
                            if (!string.IsNullOrEmpty(SLSettingPK) && SLSettingPK != "0")
                            {
                                string QDelDet = " DELETE FROM AM_Student_Leave_Settings_Det WHERE SLSettingFK ='" + SLSettingPK + "' ";
                                d2.update_method_wo_parameter(QDelDet, "Text");
                            }

                            foreach (GridViewRow gRow in fineSetGrid.Rows)
                            {
                                double amount = 0;
                                int FromDays = 0;
                                int ToDays = 0;

                                TextBox txtFrom = (TextBox)gRow.FindControl("txtFineDaysFrom");
                                TextBox txtTo = (TextBox)gRow.FindControl("txtFineDaysTo");
                                TextBox txtAmt = (TextBox)gRow.FindControl("txtFineAmount");

                                int.TryParse(txtFrom.Text, out FromDays);
                                int.TryParse(txtTo.Text, out ToDays);
                                double.TryParse(txtAmt.Text, out amount);

                                if (amount > 0 && FromDays > 0 && ToDays > 0)
                                {
                                    string QInsUp = "IF EXISTS (SELECT SLSettingPK FROM AM_Student_Leave_Settings WHERE DegreeCode ='" + degcode + "' AND CollegeCode='" + collCode + "') UPDATE AM_Student_Leave_Settings SET IsFinance='" + IsFinance + "',HeaderFK ='" + HeaderFk + "',LegerFK ='" + LedgerFk + "',MaxLeave='" + maxLeaveDays + "'  WHERE DegreeCode ='" + degcode + "' AND CollegeCode='" + collCode + "' ELSE INSERT INTO AM_Student_Leave_Settings (DegreeCode, IsFinance, HeaderFK, LegerFK, MaxLeave, CollegeCode) VALUES('" + degcode + "', '" + IsFinance + "', '" + HeaderFk + "', '" + LedgerFk + "', '" + maxLeaveDays + "', '" + collCode + "')";
                                    d2.update_method_wo_parameter(QInsUp, "Text");

                                    string SLSettingFK = d2.GetFunction("SELECT SLSettingPK FROM AM_Student_Leave_Settings WHERE DegreeCode ='" + degcode + "' AND CollegeCode='" + collCode + "'").Trim();
                                    if (!string.IsNullOrEmpty(SLSettingFK) && SLSettingFK != "0")
                                    {
                                        string QInsDet = " INSERT INTO AM_Student_Leave_Settings_Det (SLSettingFK, FromDay ,ToDay ,Amount) VALUES('" + SLSettingFK + "', '" + FromDays + "', '" + ToDays + "', '" + amount + "')";
                                        InsUpCnt += d2.update_method_wo_parameter(QInsDet, "Text");
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    validation = "Please Provide All Values";
                }

            }
            else
            {
                validation = "Please Add Fine Settings";
            }
        }
        catch { }
        if (InsUpCnt > 0)
        {
            imgalert.Visible = true;
            lbl_alert.Text = "Saved Successfully";
        }
        else
        {
            imgalert.Visible = true;
            if (string.IsNullOrEmpty(validation))
            {
                lbl_alert.Text = "Not Saved";
            }
            else
            {
                lbl_alert.Text = validation;
            }
        }
    }
    protected void btnDeleteLeaveSet_Click(object sender, EventArgs e)
    {
        DeleteLeaveSettings();
        //btngo_Click(sender, e);
    }
    private void DeleteLeaveSettings()
    {
        int del = 0;
        if (cbl_dept2.Items.Count > 0)
        {
            try
            {
                string collegeCode = "";
                if (rbmode.SelectedIndex == 0)
                    collegeCode = Convert.ToString(addddlclg.SelectedItem.Value);
                else
                    collegeCode = Convert.ToString(getCblSelectedValue(addcblhedg));

                string degCode = Convert.ToString(getCblSelectedValue(cbl_dept2));

                if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(degCode))
                {
                    string selDegQ = "SELECT SLSettingPK FROM AM_Student_Leave_Settings WHERE DegreeCode in (" + degCode + ") ";
                    DataSet dsDeg = new DataSet();
                    dsDeg = d2.select_method_wo_parameter(selDegQ, "Text");
                    if (dsDeg.Tables.Count > 0 && dsDeg.Tables[0].Rows.Count > 0)
                    {
                        int delDet = 0;
                        for (int iSLSet = 0; iSLSet < dsDeg.Tables[0].Rows.Count; iSLSet++)
                        {
                            string SLSettingPK = Convert.ToString(dsDeg.Tables[0].Rows[iSLSet][0]);
                            string DelDetQ = " DELETE FROM AM_Student_Leave_Settings_Det WHERE SLSettingFK ='" + SLSettingPK + "'  ";
                            delDet += d2.update_method_wo_parameter(DelDetQ, "Text");
                        }

                        if (delDet > 0)
                        {
                            string DelQ = "DELETE FROM AM_Student_Leave_Settings WHERE DegreeCode in (" + degCode + ") ";

                            del += d2.update_method_wo_parameter(DelQ, "Text");
                        }

                    }
                }
            }
            catch { }
        }
        if (del > 0)
        {
            divadd.Visible = false;
            imgalert.Visible = true;
            lbl_alert.Text = "Deleted Successfully";
        }
    }
    //Finance Settings
    private bool IsFinanceInclude(string collegeCode)
    {
        bool include = false;
        try
        {
            //// and user_code ='" + usercode + "'
            if (Convert.ToInt16(d2.GetFunction("select LinkValue from New_InsSettings where LinkName = 'IncludeFinanceLeaveRequest' and college_code in (" + collegeCode + ")")) > 0)
            {
                include = true;
            }
        }
        catch { include = false; }
        return include;
    }
}