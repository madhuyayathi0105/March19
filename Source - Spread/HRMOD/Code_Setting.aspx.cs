using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Security.Cryptography;
using System.Drawing;
using System.Text.RegularExpressions;
using AjaxControlToolkit;

public partial class Code_Setting : System.Web.UI.Page
{
    string college = "";
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

    DataSet ds = new DataSet();
    DataSet dv = new DataSet();
    DAccess2 d2 = new DAccess2();
    DAccess2 da = new DAccess2();
    Hashtable hat = new Hashtable();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //Page.MaintainScrollPositionOnPostBack = true;
            ////****************************************************//
            //string PageLogOut = "";
            //string sess = Convert.ToString(Session["IsLogin"]);
            //PageLogOut = Convert.ToString(Session["PageLogout"]);
            //if (sess == "")
            //{
            //}
            //else
            //{
            //    if (!Request.FilePath.Contains("HRM"))
            //    {
            //        string strPreviousPage = "";
            //        if (Request.UrlReferrer != null)
            //        {
            //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            //        }
            //        if (strPreviousPage == "")
            //        {
            //            string redrURI = ConfigurationManager.AppSettings["HR"].Trim();
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
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("HRMenuIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/HRMOD/HRMenuIndex.aspx");
                    return;
                }
            }
            //****************************************************//
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();

            //application.Visible = true;
            if (!IsPostBack)
            {
                bindcollege();
                if (ddlcoll.Items.Count > 0)
                {
                    collegecode1 = Convert.ToString(ddlcoll.SelectedItem.Value);
                }
                Acronym();
                rdb_scode_Change(sender, e);
            }
            if (ddlcoll.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddlcoll.SelectedItem.Value);
            }
            lbl_err.Visible = false;
        }
        catch (Exception ex) { }
    }

    protected void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }

    protected void ddlcoll_Change(object sender, EventArgs e)
    {
        if (ddlcoll.Items.Count > 0)
        {
            collegecode1 = Convert.ToString(ddlcoll.SelectedItem.Value);
        }
        Acronym();
        rdb_scode.Checked = true;
        rdb_appno.Checked = false;
        rdb_desigcode.Checked = false;
        rdb_catcode.Checked = false;
        rdb_scode_Change(sender, e);
    }

    public void bindcollege()
    {
        try
        {
            string group_code = Session["group_code"].ToString();
            string columnfield = "";
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                columnfield = " and group_code='" + group_code + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            ds = d2.select_method("bind_college", hat, "sp");
            ddlcoll.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcoll.Enabled = true;
                ddlcoll.DataSource = ds;
                ddlcoll.DataTextField = "collname";
                ddlcoll.DataValueField = "college_code";
                ddlcoll.DataBind();
            }
        }
        catch (Exception e) { }
    }

    protected void Acronym()
    {
        ds.Clear();
        ddl_acr.Items.Clear();
        string item = "select distinct dept_acronym from hrdept_master where college_code='" + collegecode1 + "'";
        ds = d2.select_method_wo_parameter(item, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_acr.DataSource = ds;
            ddl_acr.DataTextField = "dept_acronym";
            //ddl_acr.DataValueField = "TextCode";
            ddl_acr.DataBind();
        }
    }
    protected void cb_general_OnCheckedChanged(object sender, EventArgs e)
    {
        ViewState["prevclick"] = "";
        txt_general.Text = "";
        preiview.Visible = false;
        lbl_err.Visible = false;

        if (cb_general.Checked == true)
        {
            txt_general.Enabled = true;
        }
        else
        {
            txt_general.Enabled = false;
        }
    }

    protected void rdb_scode_Change(object sender, EventArgs e)
    {
        ViewState["prevclick"] = "";
        commclear();
        lbl_err.Visible = false;
        cb_dept.Checked = false;
        cb_dept.Enabled = true;
        string selq = "";
        selq = "select * from HRS_CodeSettings where SettingField='1' and CollegeCode='" + collegecode1 + "'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(selq, "Text");
        getgo(ds);
        btn_go_OnClick(sender, e);
    }

    protected void rdb_appno_Change(object sender, EventArgs e)
    {
        ViewState["prevclick"] = "";
        commclear();
        lbl_err.Visible = false;
        cb_dept.Checked = false;
        cb_dept.Enabled = true;
        string selq = "";
        selq = "select * from HRS_CodeSettings where SettingField='2' and CollegeCode='" + collegecode1 + "'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(selq, "Text");
        getgo(ds);
        btn_go_OnClick(sender, e);
    }

    protected void rdb_desigcode_Change(object sender, EventArgs e)
    {
        ViewState["prevclick"] = "";
        commclear();
        lbl_err.Visible = false;
        cb_dept.Checked = false;
        cb_dept.Enabled = false;
        string selq = "";
        selq = "select * from HRS_CodeSettings where SettingField='3' and CollegeCode='" + collegecode1 + "'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(selq, "Text");
        getgo(ds);
        btn_go_OnClick(sender, e);
    }

    protected void rdb_catcode_Change(object sender, EventArgs e)
    {
        ViewState["prevclick"] = "";
        commclear();
        lbl_err.Visible = false;
        cb_dept.Checked = false;
        cb_dept.Enabled = false;
        string selq = "";
        selq = "select * from HRS_CodeSettings where SettingField='4' and CollegeCode='" + collegecode1 + "'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(selq, "Text");
        getgo(ds);
        btn_go_OnClick(sender, e);
    }

    public void commclear()
    {
        lbl_select.Items.Clear();
        lbl_disp.Items.Clear();
        cb_clg.Checked = false;
        cb_dept.Checked = false;
        cb_general.Checked = false;
        txt_general.Text = "";
        txt_startingno.Text = "";
        txt_serial.Text = "";
        txt_preview.Text = "";
        ddl_acr.SelectedIndex = 0;
        preiview.Visible = false;
    }

    protected void btn_go_OnClick(object sender, EventArgs e)
    {
        ViewState["prevclick"] = "";
        lbl_select.Items.Clear();
        lbl_disp.Items.Clear();
        string selq = "";
        alert.Visible = false;
        lbl_err.Visible = false;
        preiview.Visible = false;

        if (cb_clg.Checked == true)
        {
            lbl_select.Items.Add(cb_clg.Text);
            lbl_disp.Items.Add(cb_clg.Text);
        }
        if (cb_dept.Checked == true)
        {
            lbl_select.Items.Add(cb_dept.Text);
            lbl_disp.Items.Add(cb_dept.Text);
        }
        if (cb_general.Checked == true)
        {
            lbl_select.Items.Add(cb_general.Text);
            lbl_disp.Items.Add(cb_general.Text);
            txt_general.Enabled = true;
        }
    }

    private void getgo(DataSet ds)
    {
        try
        {
            ViewState["prevclick"] = "";
            string[] splval = new string[4];
            string settval = "";
            string genacr = "";
            string startno = "";
            string size = "";
            cb_clg.Checked = false;
            cb_dept.Checked = false;
            cb_general.Checked = false;

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                settval = Convert.ToString(ds.Tables[0].Rows[0]["SettingValues"]);
                if (settval.Trim() != "" && settval.Trim() != null)
                {
                    splval = settval.Split(';');
                    if (splval.Length > 0)
                    {
                        for (int ik = 0; ik < splval.Length; ik++)
                        {
                            if (splval[ik] == "1")
                            {
                                cb_clg.Checked = true;
                            }
                            if (splval[ik] == "2")
                            {
                                cb_dept.Checked = true;
                            }
                            if (splval[ik] == "3")
                            {
                                cb_general.Checked = true;
                            }
                        }
                    }
                    genacr = Convert.ToString(ds.Tables[0].Rows[0]["GeneralAcr"]);
                    if (genacr.Trim() != "" && genacr.Trim() != null)
                    {
                        if (cb_general.Checked == true)
                        {
                            txt_general.Enabled = true;
                            txt_general.Text = genacr;
                        }
                        else
                        {
                            txt_general.Enabled = false;
                            txt_general.Text = "";
                        }
                    }
                    else
                    {
                        txt_general.Enabled = false;
                        txt_general.Text = "";
                    }
                    startno = Convert.ToString(ds.Tables[0].Rows[0]["StartNo"]);
                    if (startno.Trim() != "" && startno.Trim() != null)
                    {
                        txt_startingno.Text = startno;
                    }
                    size = Convert.ToString(ds.Tables[0].Rows[0]["SerialSize"]);
                    if (size.Trim() != "" && size.Trim() != null)
                    {
                        txt_serial.Text = size;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Code_Setting.aspx");
        }
    }

    protected void btn_right_OnClick(object sender, EventArgs e)
    {
        try
        {
            //lbl_disp.Items.Clear();
            bool ok = true;
            if (lbl_select.Items.Count > 0 && lbl_select.SelectedItem.Value != "")
            {
                for (int j = 0; j < lbl_disp.Items.Count; j++)
                {
                    if (lbl_disp.Items[j].Value == lbl_select.SelectedItem.Value)
                    {
                        ok = false;
                    }
                }
                if (ok)
                {
                    ListItem lst = new ListItem(lbl_select.SelectedItem.Text, lbl_select.SelectedItem.Value);
                    lbl_disp.Items.Add(lst);
                }
                alert.Visible = false;
            }
        }
        catch
        {
            alert.Visible = true;
            alert.Text = "Please select any one item and then procceed!";
        }
    }
    protected void btn_rightfwd_OnClick(object sender, EventArgs e)
    {
        try
        {
            lbl_disp.Items.Clear();
            if (lbl_select.Items.Count > 0)
            {
                for (int j = 0; j < lbl_select.Items.Count; j++)
                {
                    lbl_disp.Items.Add(new ListItem(lbl_select.Items[j].Text.ToString(), lbl_select.Items[j].Value.ToString()));
                }
            }
        }
        catch { }
    }
    protected void btn_left_OnClick(object sender, EventArgs e)
    {
        if (lbl_disp.Items.Count > 0)
        {
            if (lbl_disp.SelectedIndex != -1)
            {
                lbl_disp.Items.RemoveAt(lbl_disp.SelectedIndex);
            }
        }
    }
    protected void btn_leftfwd_OnClick(object sender, EventArgs e)
    {
        lbl_disp.Items.Clear();
    }

    protected void btn_preview_OnClick(object sender, EventArgs e)
    {
        preiview.Visible = true;
        string previewval = "";
        string startno = Convert.ToString(txt_startingno.Text);
        int serialsize = 0;
        Int32.TryParse(Convert.ToString(txt_serial.Text), out serialsize);
        for (int i = 0; i < lbl_disp.Items.Count; i++)
        {
            if (lbl_disp.Items[i].Text == cb_clg.Text)
            {
                string acr = "";
                if (previewval.Trim() == "")
                {
                    acr = d2.GetFunction("select Coll_acronymn from collinfo where college_code ='" + collegecode1 + "'");
                    previewval = acr;
                }
                else
                {
                    acr = d2.GetFunction("select Coll_acronymn from collinfo where college_code ='" + collegecode1 + "'");
                    previewval = previewval + acr;
                }
            }
            else if (lbl_disp.Items[i].Text == cb_dept.Text)
            {
                if (previewval.Trim() == "")
                {
                    previewval = ddl_acr.SelectedItem.Text;
                }
                else
                {
                    previewval = previewval + ddl_acr.SelectedItem.Text;
                }
            }
            else if (lbl_disp.Items[i].Text == cb_general.Text)
            {
                if (previewval.Trim() == "")
                {
                    previewval = (txt_general.Text).Trim().ToUpper();
                }
                else
                {
                    previewval = previewval + (txt_general.Text).Trim().ToUpper();
                }
            }
        }
        string value = "";
        value = startno.ToString().PadLeft(serialsize, '0');
        if ((cb_general.Checked == true && txt_general.Text.Trim() != "") || (cb_general.Checked == false && txt_general.Text.Trim() == ""))
        {
            txt_preview.Text = previewval + value;
            lbl_err.Visible = false;
            ViewState["prevclick"] = "1";
        }
        else
        {
            txt_preview.Text = "";
            lbl_err.Visible = true;
            lbl_err.Text = "Please Enter the General Acronym!";
        }
    }
    protected void ddl_acr_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        txt_preview.Text = ddl_acr.SelectedItem.Text;
    }
    //protected void txt_startingno_OnTextChanged(object sender, EventArgs e)
    //{
    //    int val = Convert.ToInt32(txt_startingno.Text);
    //    txt_serial.Text = 1.ToString().PadLeft(val, '0');
    //}
    protected void btn_save_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToString(ViewState["prevclick"]) == "1")
            {
                string field = "";
                if (rdb_scode.Checked == true)
                {
                    field = "1";
                }
                if (rdb_appno.Checked == true)
                {
                    field = "2";
                }
                if (rdb_desigcode.Checked == true)
                {
                    field = "3";
                }
                if (rdb_catcode.Checked == true)
                {
                    field = "4";
                }
                string value = "";
                string clg = "";
                string dept = "";
                string general = "";
                for (int i = 0; i < lbl_disp.Items.Count; i++)
                {
                    if (lbl_disp.Items[i].Text == cb_clg.Text)
                    {
                        clg = "1";
                        if (value == "")
                        {
                            value = clg;
                        }
                        else
                        {
                            value = value + ";" + clg;
                        }
                    }
                    else if (lbl_disp.Items[i].Text == cb_dept.Text)
                    {
                        dept = "2";
                        if (value == "")
                        {
                            value = dept;
                        }
                        else
                        {
                            value = value + ";" + dept;
                        }
                    }
                    else if (lbl_disp.Items[i].Text == cb_general.Text)
                    {
                        general = "3";
                        if (value == "")
                        {
                            value = general;
                        }
                        else
                        {
                            value = value + ";" + general;
                        }
                    }
                }
                // value = clg + ";" + dept + ";" + general;

                string startno = txt_startingno.Text;
                string serialsize = txt_serial.Text;
                string generalacr = txt_general.Text;
                if (startno.Trim() != "" && serialsize.Trim() != "" && ((cb_general.Checked == true && generalacr.Trim() != "") || (cb_general.Checked == false && generalacr.Trim() == "")))
                {
                    string savequery = "if exists(select * from HRS_CodeSettings where SettingField='" + field + "' and CollegeCode='" + collegecode1 + "') update HRS_CodeSettings set SettingValues='" + value + "',GeneralAcr='" + generalacr + "',StartNo='" + startno + "',SerialSize='" + serialsize + "' where SettingField='" + field + "' and CollegeCode='" + collegecode1 + "' else insert into HRS_CodeSettings(SettingField,SettingValues,GeneralAcr,StartNo,SerialSize,CollegeCode) values ('" + field + "','" + value + "','" + generalacr + "','" + startno + "','" + serialsize + "','" + collegecode1 + "')";
                    int a = d2.update_method_wo_parameter(savequery, "Text");
                    if (a != 0)
                    {
                        ViewState["prevclick"] = "";
                        lbl_err.Visible = false;
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Saved Successfully";
                    }
                }
                else
                {
                    lbl_err.Visible = true;
                    lbl_err.Text = "Please Fill All The Fields";
                }
            }
            else
            {
                lbl_err.Visible = true;
                lbl_err.Text = "Please Preview the Code!";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Code_Setting.aspx");
        }
    }
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }
    protected void btn_close_OnClick(object sender, EventArgs e)
    {
        preiview.Visible = false;
        txt_preview.Text = "";
    }
}