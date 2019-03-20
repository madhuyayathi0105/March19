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

public partial class SmsSettings : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();

    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string usercode = string.Empty;
    bool cellclick = false;
    string PageLogOut = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ////************************************************//
            //string sess = Convert.ToString(Session["IsLogin"]);
            //PageLogOut = Convert.ToString(Session["PageLogout"]);
            //if (sess == "")
            //{
            //}
            //else
            //{
            //    if (!Request.FilePath.Contains("smsM"))
            //    {
            //        string strPreviousPage = "";
            //        if (Request.UrlReferrer != null)
            //        {
            //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            //        }
            //        if (strPreviousPage == "")
            //        {
            //            string redrURI = ConfigurationManager.AppSettings["SMS"].Trim();
            //            Response.Redirect(redrURI, false);
            //            return;
            //        }
            //    }
            //}
            //if (Session["collegecode"] == null)
            //{
            //    string redrURI = ConfigurationManager.AppSettings["Logout"].Trim();
            //    Response.Redirect(redrURI, false);
            //    return;
            //}
            ////************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("sms"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/SMSMOD/sms.aspx");
                    return;
                }
            }
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

    protected void lb3_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("default.aspx", false);
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
            degreedetails = "Sms Settings Report";
            pagename = "SmsSettings.aspx";
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
                lblvalidation1.Text = "Please Enter Your Sms Settings Report Name";
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

                    string clgcode = Convert.ToString(fpreport.Sheets[0].Cells[arow, 1].Tag);
                    string degcode = Convert.ToString(fpreport.Sheets[0].Cells[arow, 6].Tag);

                    if (!string.IsNullOrEmpty(degcode))
                    {

                        string SelQ = "select smsapipk,smsapi_url,userid,senderid,password,collegecode,DEGREECODE,SmsreporterAPI_Url from sms_mastersettings where DEGREECODE='" + degcode + "' ";
                        SelQ += " select collname,college_code from collinfo";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(SelQ, "Text");
                        #region Load Department
                        BindDepartment2();
                        cb_dept2.Checked = false;
                        tdcbl.Visible = false;
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
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            rbmode.SelectedIndex = 0;
                            addddlclg.Items.Clear();
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                DataView dv = new DataView();
                                ds.Tables[1].DefaultView.RowFilter = "college_code='" + clgcode + "'";
                                dv = ds.Tables[1].DefaultView;
                                if (dv.Count > 0)
                                    addddlclg.Items.Add(new ListItem(Convert.ToString(dv[0]["collname"]), clgcode));

                                txtsms.Text = Convert.ToString(ds.Tables[0].Rows[0]["smsapi_url"]);
                                txtuserid.Text = Convert.ToString(ds.Tables[0].Rows[0]["userid"]);
                                txtsendid.Text = Convert.ToString(ds.Tables[0].Rows[0]["senderid"]);
                                txtpass.Text = Convert.ToString(ds.Tables[0].Rows[0]["password"]);
                                txtreceive.Text = Convert.ToString(ds.Tables[0].Rows[0]["SmsreporterAPI_Url"]);

                                addbtnsave.Text = "Update";
                                tdddl.Visible = true;
                                btndel.Visible = true;
                                divadd.Visible = true;
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

            clgcode = clgcode.Replace(",", "','");

            string SelQ = " select smsapipk,smsapi_url,userid,senderid,password,collegecode,DEGREECODE,SmsreporterAPI_Url from sms_mastersettings where collegecode in('" + clgcode + "') and DEGREECODE in (" + degcode + ")";
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
            fpreport.Sheets[0].ColumnCount = 7;
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

            fpreport.Sheets[0].ColumnHeader.Cells[0, 1].Text = "SmsApi Url";
            fpreport.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            fpreport.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            fpreport.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            fpreport.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            fpreport.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            fpreport.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;

            fpreport.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Sms ReporterApi Url";
            fpreport.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            fpreport.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            fpreport.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            fpreport.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            fpreport.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            fpreport.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;

            fpreport.Sheets[0].ColumnHeader.Cells[0, 3].Text = "User Id";
            fpreport.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            fpreport.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            fpreport.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            fpreport.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            fpreport.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            fpreport.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;

            fpreport.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Token";
            fpreport.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
            fpreport.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            fpreport.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            fpreport.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            fpreport.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            fpreport.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;

            fpreport.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Password";
            fpreport.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
            fpreport.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            fpreport.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            fpreport.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            fpreport.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            fpreport.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;

            fpreport.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Department";
            fpreport.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
            fpreport.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            fpreport.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            fpreport.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            fpreport.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            fpreport.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Left;

            #endregion

            #region value
            for (int sel = 0; sel < ds.Tables[0].Rows.Count; sel++)
            {
                fpreport.Sheets[0].RowCount++;
                fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sel + 1);
                fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[sel]["smsapi_url"]);
                fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[sel]["collegecode"]);
                fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[sel]["SmsreporterAPI_Url"]);

                fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[sel]["userid"]);
                fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[sel]["senderid"]);
                fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[sel]["password"]);
                string degCode = Convert.ToString(ds.Tables[0].Rows[sel]["DEGREECODE"]).Trim();
                string deptName = d2.GetFunction("select  department.dept_name,degree.degree_code,department.dept_code from degree,department where  department.dept_code=degree.dept_code  and department.college_code = degree.college_code and degree.Degree_Code = " + degCode + "").Trim();
                fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 6].Text = deptName;
                fpreport.Sheets[0].Cells[fpreport.Sheets[0].RowCount - 1, 6].Tag = degCode;

            }

            fpreport.Sheets[0].PageSize = fpreport.Sheets[0].RowCount;
            fpreport.SaveChanges();
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
        txtsms.Text = "";
        txtuserid.Text = "";
        txtsendid.Text = "";
        txtpass.Text = "";
        txtreceive.Text = "";
        rbmode.SelectedIndex = 0;
        tdddl.Visible = true;
        addbtnsave.Text = "Save";
        btndel.Visible = false;
        tdddl.Visible = true;
        tdcbl.Visible = false;
        loadddlcollege();
        loadCblcollege();
        BindDepartment2();
    }
    protected void rbmode_OnSelected(object sender, EventArgs e)
    {
        if (rbmode.SelectedIndex == 0)
        {
            tdddl.Visible = true;
            tdcbl.Visible = false;
            //txtsms.Text = "";
            //txtuserid.Text = "";
            //txtsendid.Text = "";
            //txtpass.Text = "";
        }
        else
        {
            tdddl.Visible = false;
            tdcbl.Visible = true;
            loadCblcollege();
        }
        BindDepartment2();
    }
    protected void addtbtnsave_Click(object sender, EventArgs e)
    {
        settingAdd();
        btngo_Click(sender, e);
    }
    protected void settingAdd()
    {
        try
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
            bool save = false;
            string smsapi = Convert.ToString(txtsms.Text);
            string userid = Convert.ToString(txtuserid.Text);
            string sendid = Convert.ToString(txtsendid.Text);
            string passwd = Convert.ToString(txtpass.Text);
            string smsreceiver = Convert.ToString(txtreceive.Text);
            string clgcoce = "";
            if (rbmode.SelectedIndex == 0)
                clgcoce = Convert.ToString(addddlclg.SelectedItem.Value);
            else
                clgcoce = Convert.ToString(getCblSelectedValue(addcblhedg));

            string degCode = Convert.ToString(getCblSelectedValue(cbl_dept2));

            if (!string.IsNullOrEmpty(clgcoce) && !string.IsNullOrEmpty(smsapi) && !string.IsNullOrEmpty(userid) && !string.IsNullOrEmpty(sendid) && !string.IsNullOrEmpty(passwd) && !string.IsNullOrEmpty(degCode))
            {
                string[] splval = degCode.Split(',');
                if (splval.Length > 0)
                {
                    for (int i = 0; i < splval.Length; i++)
                    {
                        string degcode = splval[i];
                        string collCode = htDegCol[degcode].ToString();
                        string UpdQ = "if exists(select collegecode from sms_mastersettings where collegecode='" + collCode + "' and DEGREECODE='" + degcode + "') update sms_mastersettings set smsapi_url='" + smsapi + "',userid='" + userid + "',senderid='" + sendid + "',password='" + passwd + "',SmsreporterAPI_Url='" + smsreceiver + "' where collegecode='" + collCode + "' and DEGREECODE='" + degcode + "' else insert into sms_mastersettings(smsapi_url,userid,senderid,password,collegecode,DEGREECODE,SmsreporterAPI_Url) values('" + smsapi + "','" + userid + "','" + sendid + "','" + passwd + "','" + collCode + "','" + degcode + "','" + smsreceiver + "')";
                        int val = d2.update_method_wo_parameter(UpdQ, "Text");
                        save = true;
                    }
                }
                if (save == true)
                {
                    imgalert.Visible = true;
                    if (addbtnsave.Text.Trim() == "Save")
                    {
                        divadd.Visible = false;
                        lbl_alert.Text = "Saved Successfully";
                    }
                    else
                    {
                        divadd.Visible = false;
                        lbl_alert.Text = "Updated Successfully";
                    }
                }
            }
            else
            {
                imgalert.Visible = true;
                lbl_alert.Text = "Please Fill The Values";
            }

        }
        catch { }
    }
    protected void addbtncancel_Click(object sender, EventArgs e)
    {
        divadd.Visible = false;

    }
    protected void btndel_Click(object sender, EventArgs e)
    {
        delSetting();
        btngo_Click(sender, e);
    }
    protected void delSetting()
    {
        try
        {
            string clgcoce = "";
            if (rbmode.SelectedIndex == 0)
                clgcoce = Convert.ToString(addddlclg.SelectedItem.Value);
            else
                clgcoce = Convert.ToString(getCblSelectedValue(addcblhedg));

            string degCode = Convert.ToString(getCblSelectedValue(cbl_dept2));

            if (!string.IsNullOrEmpty(degCode))
            {
                string DelQ = "delete from sms_mastersettings where DEGREECODE in (" + degCode + ") ";

                int del = d2.update_method_wo_parameter(DelQ, "Text");
                if (del > 0)
                {
                    divadd.Visible = false;
                    imgalert.Visible = true;
                    lbl_alert.Text = "Deleted Successfully";
                }
            }
        }
        catch { }
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

    // last modified 24-10-2016 sudhagar

    //Added by Idhris 27-10-2016

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
            string collegeCode = string.Empty;
            collegeCode = getCblSelectedValue(cblclg);

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

    protected void addddlclg_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDepartment2();
    }

}