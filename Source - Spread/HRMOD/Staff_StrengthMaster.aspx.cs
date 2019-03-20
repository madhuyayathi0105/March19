using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using Gios.Pdf;
using System.IO;
using System.Text;
using System.Configuration;
using System.Web.Services;
using System.Drawing;
using AjaxControlToolkit;

public partial class Staff_StrengthMaster : System.Web.UI.Page
{
    int i = 0;
    string college = "";
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    DAccess2 da = new DAccess2();
    Hashtable hat = new Hashtable();
    Hashtable hat1 = new Hashtable();
    Hashtable hsave = new Hashtable();
    Hashtable temphas = new Hashtable();
    Hashtable hsbind = new Hashtable();
    bool check = false;
    bool check1 = false;
    bool check2 = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Page.MaintainScrollPositionOnPostBack = true;
            //****************************************************//
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
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                bindcollege();
                binddept();
                designation();
                stafftype();
                staffcategory();
                bindreportname();
            }
            lblalerterr.Visible = false;
            lblspread1_err.Visible = false;
            lblvalidation1.Visible = false;
        }
        catch (Exception ex) { }

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
            ddlcollege.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.Enabled = true;
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch (Exception e)
        {
        }
    }
    protected void binddept()
    {
        try
        {
            ds.Clear();
            cbl_dept.Items.Clear();
            string collcode = Convert.ToString(ddlcollege.SelectedValue);
            string item = "select distinct Dept_Code,Dept_Name from hrdept_master where college_code='" + collcode + "' ";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_dept.DataSource = ds;
                cbl_dept.DataTextField = "Dept_Name";
                cbl_dept.DataValueField = "Dept_Code";
                cbl_dept.DataBind();
                if (cbl_dept.Items.Count > 0)
                {
                    for (i = 0; i < cbl_dept.Items.Count; i++)
                    {
                        cbl_dept.Items[i].Selected = true;
                    }
                    txt_dept.Text = "Department (" + cbl_dept.Items.Count + ")";
                    cb_dept.Checked = true;
                }
                stafftype();
            }
            else
            {
                txt_dept.Text = "--Select--";
                cb_dept.Checked = false;
            }
        }
        catch
        {
        }
    }
    protected void designation()
    {
        ds.Clear();
        cbl_desig.Items.Clear();
        string collcode = Convert.ToString(ddlcollege.SelectedValue);
        string statequery = "select desig_code,desig_name from desig_master where collegeCode='" + collcode + "'";
        ds = da.select_method_wo_parameter(statequery, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_desig.DataSource = ds;
            cbl_desig.DataTextField = "desig_name";
            cbl_desig.DataValueField = "desig_code";
            cbl_desig.DataBind();
            cbl_desig.Visible = true;
            if (cbl_desig.Items.Count > 0)
            {
                for (i = 0; i < cbl_desig.Items.Count; i++)
                {
                    cbl_desig.Items[i].Selected = true;
                }
                txt_desig.Text = "Designation(" + cbl_desig.Items.Count + ")";
                cb_desig.Checked = true;
            }
            stafftype();
        }
        else
        {
            txt_desig.Text = "--Select--";
            cb_desig.Checked = false;
        }
    }
    //protected void stafftype()
    //{
    //    try
    //    {
    //        ds.Clear();
    //        cbl_stype.Items.Clear();
    //        string collcode = Convert.ToString(ddlcollege.SelectedValue);
    //        string item = "select distinct stftype from stafftrans t ,staffmaster m where m.staff_code = t.staff_code and college_code = '" + collcode + "'";
    //        ds = d2.select_method_wo_parameter(item, "Text");
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            cbl_stype.DataSource = ds;
    //            cbl_stype.DataTextField = "stftype";
    //            cbl_stype.DataBind();
    //            if (cbl_stype.Items.Count > 0)
    //            {
    //                for (i = 0; i < cbl_stype.Items.Count; i++)
    //                {
    //                    cbl_stype.Items[i].Selected = true;
    //                }
    //                txt_stype.Text = "StaffType (" + cbl_stype.Items.Count + ")";
    //                cb_stype.Checked = true;
    //            }
    //        }
    //        else
    //        {
    //            txt_stype.Text = "--Select--";
    //            cb_stype.Checked = false;
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}

    protected void stafftype()
    {
        try
        {

            cbl_stype.Items.Clear();
            txt_stype.Text = "--Select--";
            cb_stype.Checked = false;
            Dictionary<string, string> dicgetcode = new Dictionary<string, string>();
            dicgetcode.Clear();
            Dictionary<string, string> dicdescode = new Dictionary<string, string>();
            dicdescode.Clear();
            string collcode = Convert.ToString(ddlcollege.SelectedValue);
            if (cbl_desig.Items.Count > 0)
            {
                for (int ik = 0; ik < cbl_desig.Items.Count; ik++)
                {
                    if (cbl_desig.Items[ik].Selected == true)
                    {
                        if (!dicgetcode.ContainsKey(Convert.ToString(cbl_desig.Items[ik].Value)))
                        {
                            //string selq = "select desig_code,desig_name from desig_master where ((dept_code like '" + Convert.ToString(cbl_dept.Items[ik].Value) + ";%') or (dept_code like '%;" + Convert.ToString(cbl_dept.Items[ik].Value) + "%') or (dept_code like '%" + Convert.ToString(cbl_dept.Items[ik].Value) + "') or (dept_code='" + Convert.ToString(cbl_dept.Items[ik].Value) + "'))";
                            //select distinct stftype from stafftrans t ,staffmaster m where m.staff_code = t.staff_code and college_code = '13' and desig_code in ('','',';' )

                            string selq = " select distinct stftype from stafftrans t ,staffmaster m where m.staff_code = t.staff_code and college_code = '" + collcode + "' and desig_code='" + cbl_desig.Items[ik].Value + "' ";

                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selq, "Text");
                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                for (int jk = 0; jk < ds.Tables[0].Rows.Count; jk++)
                                {
                                    if (!dicdescode.ContainsKey(Convert.ToString(ds.Tables[0].Rows[jk]["stftype"])))
                                    {
                                        cbl_stype.Items.Add(new ListItem(Convert.ToString(ds.Tables[0].Rows[jk]["stftype"]), Convert.ToString(ds.Tables[0].Rows[jk]["stftype"])));
                                        dicdescode.Add(Convert.ToString(ds.Tables[0].Rows[jk]["stftype"]), Convert.ToString(ds.Tables[0].Rows[jk]["stftype"]));
                                    }
                                }
                            }
                            dicgetcode.Add(Convert.ToString(cbl_desig.Items[ik].Value), Convert.ToString(cbl_desig.Items[ik].Text));
                        }
                    }
                }
            }
            if (cbl_stype.Items.Count > 0)
            {
                for (int i = 0; i < cbl_stype.Items.Count; i++)
                {
                    cbl_stype.Items[i].Selected = true;
                }
                txt_stype.Text = "StaffType (" + cbl_stype.Items.Count + ")";
                cb_stype.Checked = true;
            }
        }
        catch (Exception ex)
        {
        }
    }
  


    //protected void staffcategory()
    //{
    //    try
    //    {
    //        ds.Clear();
    //        cbl_scat.Items.Clear();
    //        string collcode = Convert.ToString(ddlcollege.SelectedValue);
    //        string item = "select distinct category_name,category_code from staffcategorizer where college_code= '" + collcode + "'";
    //        ds = d2.select_method_wo_parameter(item, "Text");
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            cbl_scat.DataSource = ds;
    //            cbl_scat.DataTextField = "category_name";
    //            cbl_scat.DataValueField = "category_code";
    //            cbl_scat.DataBind();
    //            if (cbl_scat.Items.Count > 0)
    //            {
    //                for (i = 0; i < cbl_scat.Items.Count; i++)
    //                {
    //                    cbl_scat.Items[i].Selected = true;
    //                }
    //                txt_scat.Text = "StaffCategory (" + cbl_scat.Items.Count + ")";
    //                cb_scat.Checked = true;
    //            }
    //        }
    //        else
    //        {
    //            txt_scat.Text = "--Select--";
    //            cb_scat.Checked = false;
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}



    protected void staffcategory()
    {
        try
        {

            cbl_scat.Items.Clear();
            txt_scat.Text = "--Select--";
            cb_scat.Checked = false;
            Dictionary<string, string> dicgetcode = new Dictionary<string, string>();
            dicgetcode.Clear();
            Dictionary<string, string> dicdescode = new Dictionary<string, string>();
            dicdescode.Clear();
            string collcode = Convert.ToString(ddlcollege.SelectedValue);
            if (cbl_stype.Items.Count > 0)
            {
                for (int ik = 0; ik < cbl_stype.Items.Count; ik++)
                {
                    if (cbl_stype.Items[ik].Selected == true)
                    {
                        if (!dicgetcode.ContainsKey(Convert.ToString(cbl_stype.Items[ik].Value)))
                        {
                            //string selq = "select desig_code,desig_name from desig_master where ((dept_code like '" + Convert.ToString(cbl_dept.Items[ik].Value) + ";%') or (dept_code like '%;" + Convert.ToString(cbl_dept.Items[ik].Value) + "%') or (dept_code like '%" + Convert.ToString(cbl_dept.Items[ik].Value) + "') or (dept_code='" + Convert.ToString(cbl_dept.Items[ik].Value) + "'))";
                            //select distinct stftype from stafftrans t ,staffmaster m where m.staff_code = t.staff_code and college_code = '13' and desig_code in ('','',';' )

                            string selq = "select distinct sc.category_code,sc.category_name   from stafftrans t,staffcategorizer sc where t.category_code=sc.category_code and college_code ='" + collcode + "' and stftype ='" + cbl_stype.Items[ik].Value + "'";

                            ds.Clear();
                            ds = d2.select_method_wo_parameter(selq, "Text");
                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                for (int jk = 0; jk < ds.Tables[0].Rows.Count; jk++)
                                {
                                    if (!dicdescode.ContainsKey(Convert.ToString(ds.Tables[0].Rows[jk]["category_code"])))
                                    {
                                        cbl_scat.Items.Add(new ListItem(Convert.ToString(ds.Tables[0].Rows[jk]["category_name"]), Convert.ToString(ds.Tables[0].Rows[jk]["category_code"])));
                                        dicdescode.Add(Convert.ToString(ds.Tables[0].Rows[jk]["category_code"]), Convert.ToString(ds.Tables[0].Rows[jk]["category_name"]));
                                    }
                                }
                            }
                            dicgetcode.Add(Convert.ToString(cbl_stype.Items[ik].Value), Convert.ToString(cbl_stype.Items[ik].Text));
                        }
                    }
                }
            }
            if (cbl_stype.Items.Count > 0)
            {
                for (int i = 0; i < cbl_scat.Items.Count; i++)
                {
                    cbl_scat.Items[i].Selected = true;
                }
                txt_scat.Text = "StaffCategory (" + cbl_scat.Items.Count + ")";
                cb_scat.Checked = true;
            }
        }
        catch (Exception ex)
        {
        }
    }
  














    protected void Religieon()
    {
        try
        {
            ds.Clear();
            cbl_religieon1.Items.Clear();
            string collcode = Convert.ToString(ddlcollege.SelectedValue);
            string item = "select distinct TextCode,Textval from textvaltable t,staff_appl_master sp where TextCriteria='relig'  and sp.religion=t.TextVal and sp.college_code='" + collcode + "' and t.TextVal<>'' and t.TextVal is not null";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_religieon1.DataSource = ds;
                cbl_religieon1.DataTextField = "Textval";
                cbl_religieon1.DataValueField = "TextCode";
                cbl_religieon1.DataBind();
                if (cbl_religieon1.Items.Count > 0)
                {
                    for (i = 0; i < cbl_religieon1.Items.Count; i++)
                    {
                        cbl_religieon1.Items[i].Selected = true;
                    }
                    txt_religieon.Text = "Religion (" + cbl_religieon1.Items.Count + ")";
                    cb_religieon1.Checked = true;
                }
            }
            else
            {
                txt_religieon.Text = "--Select--";
                cb_religieon1.Checked = false;
            }
        }
        catch
        {
        }
    }
    protected void Community()
    {
        ds.Clear();
        cbl_comm1.Items.Clear();
        string collcode = Convert.ToString(ddlcollege.SelectedValue);
        string statequery = "select distinct Community,TextCode,TextVal from staff_appl_master sp,TextValTable t where sp.Community=t.TextVal and TextCriteria='comm' and Community is not null and Community<>'' and sp.college_code='" + collcode + "'";
        ds = da.select_method_wo_parameter(statequery, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_comm1.DataSource = ds;
            cbl_comm1.DataTextField = "TextVal";
            cbl_comm1.DataValueField = "TextCode";
            cbl_comm1.DataBind();
            cbl_comm1.Visible = true;
            if (cbl_comm1.Items.Count > 0)
            {
                for (i = 0; i < cbl_comm1.Items.Count; i++)
                {
                    cbl_comm1.Items[i].Selected = true;
                }
                txt_comm.Text = "Community(" + cbl_comm1.Items.Count + ")";
                cb_comm1.Checked = true;
            }
        }
        else
        {
            txt_comm.Text = "--Select--";
            cb_comm1.Checked = false;
        }
    }
    protected void Caste()
    {
        try
        {
            ds.Clear();
            cbl_caste1.Items.Clear();
            string collcode = Convert.ToString(ddlcollege.SelectedValue);
            string item = "select distinct TextCode,Textval from textvaltable t,staff_appl_master sp where TextCriteria='caste' and sp.college_code= '" + collcode + "' and t.TextVal=sp.Caste and t.TextVal<>'' and t.TextVal is not null";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_caste1.DataSource = ds;
                cbl_caste1.DataTextField = "Textval";
                cbl_caste1.DataValueField = "TextCode";
                cbl_caste1.DataBind();
                if (cbl_caste1.Items.Count > 0)
                {
                    txt_caste.Text = "--Select--";
                    cb_caste1.Checked = false;
                }
            }
            else
            {
                txt_caste.Text = "--Select--";
                cb_caste1.Checked = false;
            }
        }
        catch
        {
        }
    }
    protected void Qualification()
    {
        try
        {
            ds.Clear();
            string collcode = Convert.ToString(ddlcollege.SelectedValue);
            string getquery = "select qualification from staff_appl_master where college_code= '" + collcode + "'";
            ds = d2.select_method_wo_parameter(getquery, "Text");
            ArrayList arr1 = new ArrayList();
            string a = "";
            string b = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string qual = ds.Tables[0].Rows[i]["qualification"].ToString();
                    string[] split1 = qual.Split('\\');
                    if (split1.Length > 1)
                    {
                        for (int j = 0; j <= split1.GetUpperBound(0); j++)
                        {
                            a = split1[j];
                            if (a != "" && a != null)
                            {
                                string[] split2 = a.Split(';');
                                if (split2.GetUpperBound(0) > 0)
                                {
                                    b = split2[2];
                                    if (!arr1.Contains(b))
                                    {
                                        arr1.Add(b);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            int s = 0;
            for (int r = 0; r < arr1.Count; r++)
            {
                if (!hsave.Contains(arr1[r]))
                {
                    if (arr1[r] != "")
                    {
                        hsave.Add(s, arr1[r]);
                        s++;
                    }
                }
            }
            cbl_qual1.Items.Clear();
            if (hsave.Count > 0)
            {
                for (int k = 0; k < hsave.Count; k++)
                {
                    cbl_qual1.Items.Add(hsave[k].ToString());
                    cbl_qual1.Items[k].Selected = true;
                }
                txt_qual.Text = "Qualification (" + cbl_qual1.Items.Count + ")";
                cb_qual1.Checked = true;
            }
            else
            {
                txt_qual.Text = "--Select--";
                cb_qual1.Checked = false;
            }
        }
        catch
        {
        }
    }
    protected void FamiliarSubject()
    {
        try
        {
            ds.Clear();
            cbl_fsub1.Items.Clear();
            string collcode = Convert.ToString(ddlcollege.SelectedValue);
            string getquery = "select subjects from staff_appl_master where college_code= '" + collcode + "' and subjects is not null and subjects<>'' and subjects<>';'";
            ds = d2.select_method_wo_parameter(getquery, "Text");
            ArrayList arr2 = new ArrayList();
            string a = "";
            string b = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string qual = ds.Tables[0].Rows[i]["subjects"].ToString();
                    if (qual.Contains(';'))
                    {
                        string[] split1 = qual.Split(';');
                        if (split1.Length > 0)
                        {
                            for (int j = 0; j <= split1.GetUpperBound(0); j++)
                            {
                                a = split1[j];
                                if (a != "" && a != null)
                                {
                                    if (a.Contains(','))
                                    {
                                        string[] split2 = a.Split(',');
                                        if (split2.GetUpperBound(0) > 0)
                                        {
                                            for (int k = 0; k < split2.GetUpperBound(0); k++)
                                            {
                                                b = split2[k];
                                                if (!arr2.Contains(b))
                                                {
                                                    arr2.Add(b);
                                                    if (!hat1.ContainsKey(b))
                                                    {
                                                        hat1.Add(b, qual);
                                                        cbl_fsub1.Items.Add(b.ToString());
                                                        cbl_fsub1.Items[k].Selected = false;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (hat1.Count > 0)
            {
                ViewState["hasval"] = hat1;
                txt_fsub.Text = "--Select--";
            }
            else
            {
                txt_fsub.Text = "--Select--";
                cb_fsub1.Checked = false;
            }
        }
        catch
        {
        }
    }
    public void bindblood()
    {
        cbl_bgoup.Items.Clear();
        string collcode = Convert.ToString(ddlcollege.SelectedValue);
        string query = "select distinct textval,textcode from textvaltable t,staff_appl_master sp where TextCriteria='bgrou' and t.TextVal =sp.bldgrp and sp.college_code='" + collcode + "' and textval is not null and textval<>''";
        DataSet ds = new DataSet();
        ds.Dispose(); ds.Reset();
        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_bgoup.DataSource = ds;
            cbl_bgoup.DataTextField = "Textval";
            cbl_bgoup.DataValueField = "TextCode";
            cbl_bgoup.DataBind();
            if (cbl_bgoup.Items.Count > 0)
            {
                for (int ik = 0; ik < cbl_bgoup.Items.Count; ik++)
                {
                    cbl_bgoup.Items[ik].Selected = true;
                }
                txt_bgroup.Text = "Blood Group(" + Convert.ToString(cbl_bgoup.Items.Count) + ")";
                cb_bgoup.Checked = true;
            }
            else
            {
                txt_bgroup.Text = "--Select--";
                cb_bgoup.Checked = false;
            }
        }
    }
    public void bindmstatus()
    {
        try
        {
            cbl_marital.Items.Clear();
            ds.Clear();
            string collcode = Convert.ToString(ddlcollege.SelectedValue);
            string mstatus = "";
            mstatus = "select distinct martial_status,TextVal,TextCode from staff_appl_master sp,TextValTable t where ISNULL (martial_status,'')<>'' and sp.martial_status=t.TextVal and TextCriteria='marit' and sp.college_code='" + collcode + "'";
            ds = d2.select_method_wo_parameter(mstatus, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_marital.DataSource = ds;
                cbl_marital.DataTextField = "martial_status";
                cbl_marital.DataValueField = "martial_status";
                cbl_marital.DataBind();
                if (cbl_marital.Items.Count > 0)
                {
                    for (int ik = 0; ik < cbl_marital.Items.Count; ik++)
                    {
                        cbl_marital.Items[ik].Selected = true;
                    }
                    txt_marital.Text = "Marital Status(" + Convert.ToString(cbl_marital.Items.Count) + ")";
                    cb_marital.Checked = true;
                }
                else
                {
                    txt_marital.Text = "--Select--";
                    cb_marital.Checked = false;
                }
            }
        }
        catch (Exception e)
        {
        }
    }
    public void bindnational()
    {
        cbl_nation.Items.Clear();
        string collcode = Convert.ToString(ddlcollege.SelectedValue);
        string query = "select distinct TextCode,Textval from textvaltable t,staff_appl_master sp where TextCriteria='natio' and sp.college_code='" + collcode + "' and t.TextVal=sp.Nationality and t.TextVal<>'' and t.TextVal is not null";
        DataSet ds = new DataSet();
        ds.Dispose(); ds.Reset();
        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_nation.DataSource = ds;
            cbl_nation.DataTextField = "Textval";
            cbl_nation.DataValueField = "TextCode";
            cbl_nation.DataBind();
            if (cbl_nation.Items.Count > 0)
            {
                for (int ik = 0; ik < cbl_nation.Items.Count; ik++)
                {
                    cbl_nation.Items[ik].Selected = true;
                }
                txt_nation.Text = "Nationality(" + Convert.ToString(cbl_nation.Items.Count) + ")";
                cb_nation.Checked = true;
            }
            else
            {
                txt_nation.Text = "--Select--";
                cb_nation.Checked = false;
            }
        }
    }
    public void bindcity()
    {
        cbl_city.Items.Clear();
        string collcode = Convert.ToString(ddlcollege.SelectedValue);
        string query = "select distinct TextCode,Textval from textvaltable t,staff_appl_master sp where TextCriteria='city' and sp.college_code='" + collcode + "' and t.TextVal=sp.ccity and Textval<>'' and TextVal is not null order by  textval";
        ds.Clear();
        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_city.DataSource = ds;
            cbl_city.DataTextField = "Textval";
            cbl_city.DataValueField = "TextCode";
            cbl_city.DataBind();
            if (cbl_city.Items.Count > 0)
            {
                for (int ik = 0; ik < cbl_city.Items.Count; ik++)
                {
                    cbl_city.Items[ik].Selected = true;
                }
                txt_city.Text = "City(" + Convert.ToString(cbl_city.Items.Count) + ")";
                cb_city.Checked = true;
            }
            else
            {
                txt_city.Text = "--Select--";
                cb_city.Checked = false;
            }
        }
    }
    public void binddistrict()
    {
        cbl_dis.Items.Clear();
        string collcode = Convert.ToString(ddlcollege.SelectedValue);
        string query = "select distinct MasterCode,Mastervalue from co_mastervalues m,staff_appl_master sp where MasterCriteria='district' and sp.cdistrict=m.MasterValue and CollegeCode='" + collcode + "' and Mastervalue<>'' and MasterValue is not null order by  Mastervalue";
        ds.Clear();
        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_dis.DataSource = ds;
            cbl_dis.DataTextField = "Mastervalue";
            cbl_dis.DataValueField = "MasterCode";
            cbl_dis.DataBind();
            if (cbl_dis.Items.Count > 0)
            {
                for (int ik = 0; ik < cbl_dis.Items.Count; ik++)
                {
                    cbl_dis.Items[ik].Selected = true;
                }
                txt_dis.Text = "District(" + Convert.ToString(cbl_dis.Items.Count) + ")";
                cb_dis.Checked = true;
            }
            else
            {
                txt_dis.Text = "--Select--";
                cb_dis.Checked = false;
            }
        }
    }
    public void bindstate()
    {
        cbl_state.Items.Clear();
        string collcode = Convert.ToString(ddlcollege.SelectedValue);
        string query = "select distinct MasterCode,Mastervalue from co_mastervalues m,staff_appl_master sp where MasterCriteria='state' and CollegeCode='" + collcode + "' and sp.cstate=m.MasterValue and Mastervalue<>'' and MasterValue is not null order by  Mastervalue";
        ds.Clear();
        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_state.DataSource = ds;
            cbl_state.DataTextField = "Mastervalue";
            cbl_state.DataValueField = "MasterCode";
            cbl_state.DataBind();
            if (cbl_state.Items.Count > 0)
            {
                for (int ik = 0; ik < cbl_state.Items.Count; ik++)
                {
                    cbl_state.Items[ik].Selected = true;
                }
                txt_state.Text = "State(" + Convert.ToString(cbl_state.Items.Count) + ")";
                cb_state.Checked = true;
            }
            else
            {
                txt_state.Text = "--Select--";
                cb_state.Checked = false;
            }
        }
    }
    public void bindgender()
    {
        chklstgender.Items.Clear();
        chklstgender.Items.Add(new ListItem("Male", "Male"));
        chklstgender.Items.Add(new ListItem("Female", "Female"));
        chklstgender.Items.Add(new ListItem("TransGender", "TransGender"));
        chklstgender.DataBind();
        for (int ik = 0; ik < chklstgender.Items.Count; ik++)
        {
            chklstgender.Items[ik].Selected = true;
        }
        txtgender.Text = "Gender(" + Convert.ToString(chklstgender.Items.Count) + ")";
        chkgender.Checked = true;
    }
    public void bindstfstatus()
    {
        chklststfstatus.Items.Clear();
        chklststfstatus.Items.Add(new ListItem("Relieved", " resign='1' and settled='1'"));
        chklststfstatus.Items.Add(new ListItem("Discontinued", " Discontinue='1'"));
        chklststfstatus.DataBind();
        for (int ik = 0; ik < chklststfstatus.Items.Count; ik++)
        {
            chklststfstatus.Items[ik].Selected = true;
        }
        txtstfstatus.Text = "Staff Status(" + Convert.ToString(chklststfstatus.Items.Count) + ")";
        chkstfstatus.Checked = true;
    }
    public void bindexpin()
    {
        cbl_exp.Items.Clear();
        string collcode = Convert.ToString(ddlcollege.SelectedValue);
        string query = "select distinct yofexp from staff_appl_master where yofexp is not null and yofexp<>'' and college_code='" + collcode + "' ";
        DataSet ds = new DataSet();
        ds.Dispose();
        ds.Reset();
        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_exp.DataSource = ds;
            cbl_exp.DataTextField = "yofexp";
            cbl_exp.DataValueField = "yofexp";
            cbl_exp.DataBind();
            if (cbl_exp.Items.Count > 0)
            {
                for (int ik = 0; ik < cbl_exp.Items.Count; ik++)
                {
                    cbl_exp.Items[ik].Selected = true;
                }
                txt_exp.Text = "Experience(" + Convert.ToString(cbl_exp.Items.Count) + ")";
                cb_exp.Checked = true;
            }
            else
            {
                txt_exp.Text = "--Select--";
                cb_exp.Checked = false;
            }
        }
    }
    protected void cbblood_OnCheckedChanged(object sender, EventArgs e)
    {
        if (cbblood.Checked == true)
        {
            bindblood();
            txt_bgroup.Enabled = true;
        }
        else
        {
            cbl_bgoup.Items.Clear();
            txt_bgroup.Text = "--Select--";
            txt_bgroup.Enabled = false;
        }
    }
    protected void cb_bgoup_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_bgoup, cbl_bgoup, txt_bgroup, "Blood Group");
    }
    protected void cbl_bgoup_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_bgoup, cbl_bgoup, txt_bgroup, "Blood Group");
    }
    protected void cbmarital_OnCheckedChanged(object sender, EventArgs e)
    {
        if (cbmarital.Checked == true)
        {
            bindmstatus();
            txt_marital.Enabled = true;
        }
        else
        {
            cbl_marital.Items.Clear();
            txt_marital.Text = "--Select--";
            txt_marital.Enabled = false;
        }
    }
    protected void cb_marital_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_marital, cbl_marital, txt_marital, "Marital Status");
    }
    protected void cbl_marital_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_marital, cbl_marital, txt_marital, "Marital Status");
    }
    protected void cbnation_OnCheckedChanged(object sender, EventArgs e)
    {
        if (cbnation.Checked == true)
        {
            bindnational();
            txt_nation.Enabled = true;
        }
        else
        {
            cbl_nation.Items.Clear();
            txt_nation.Text = "--Select--";
            txt_nation.Enabled = false;
        }
    }
    protected void cb_nation_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_nation, cbl_nation, txt_nation, "Nationality");
    }
    protected void cbl_nation_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_nation, cbl_nation, txt_nation, "Nationality");
    }
    protected void cbexp_OnCheckedChanged(object sender, EventArgs e)
    {
        if (cbexp.Checked == true)
        {
            bindexpin();
            txt_exp.Enabled = true;
        }
        else
        {
            cbl_nation.Items.Clear();
            txt_exp.Text = "--Select--";
            txt_exp.Enabled = false;
        }
    }
    protected void cb_exp_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_exp, cbl_exp, txt_exp, "Experience");
    }
    protected void cbl_exp_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_exp, cbl_exp, txt_exp, "Experience");
    }
    protected void cbcity_OnCheckedChanged(object sender, EventArgs e)
    {
        if (cbcity.Checked == true)
        {
            bindcity();
            txt_city.Enabled = true;
        }
        else
        {
            cbl_city.Items.Clear();
            txt_city.Text = "--Select--";
            txt_city.Enabled = false;
        }
    }
    protected void cb_city_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_city, cbl_city, txt_city, "City");
    }
    protected void cbl_city_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_city, cbl_city, txt_city, "City");
    }
    protected void cbdis_OnCheckedChanged(object sender, EventArgs e)
    {
        if (cbdis.Checked == true)
        {
            binddistrict();
            txt_dis.Enabled = true;
        }
        else
        {
            cbl_dis.Items.Clear();
            txt_dis.Text = "--Select--";
            txt_dis.Enabled = false;
        }
    }
    protected void cb_dis_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_dis, cbl_dis, txt_dis, "District");
    }
    protected void cbl_dis_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_dis, cbl_dis, txt_dis, "District");
    }
    protected void cbstate_OnCheckedChanged(object sender, EventArgs e)
    {
        if (cbstate.Checked == true)
        {
            bindstate();
            txt_state.Enabled = true;
        }
        else
        {
            cbl_state.Items.Clear();
            txt_state.Text = "--Select--";
            txt_state.Enabled = false;
        }
    }
    protected void cbgender_OnCheckedChanged(object sender, EventArgs e)
    {
        if (cbgender.Checked == true)
        {
            bindgender();
            txtgender.Enabled = true;
        }
        else
        {
            chklstgender.Items.Clear();
            txtgender.Text = "--Select--";
            txtgender.Enabled = false;
        }
    }
    protected void cbstfstatus_OnCheckedChanged(object sender, EventArgs e)
    {
        if (cbstfstatus.Checked == true)
        {
            bindstfstatus();
            txtstfstatus.Enabled = true;
        }
        else
        {
            chklststfstatus.Items.Clear();
            txtstfstatus.Text = "--Select--";
            txtstfstatus.Enabled = false;
        }
    }
    protected void chkstfstatus_CheckedChange(object sender, EventArgs e)
    {
        chkchange(chkstfstatus, chklststfstatus, txtstfstatus, "Staff Status");
    }
    protected void chklststfstatus_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(chkstfstatus, chklststfstatus, txtstfstatus, "Staff Status");
    }
    protected void chkgender_CheckedChange(object sender, EventArgs e)
    {
        chkchange(chkgender, chklstgender, txtgender, "Gender");
    }
    protected void chklstgender_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(chkgender, chklstgender, txtgender, "Gender");
    }
    protected void cb_state_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_state, cbl_state, txt_state, "State");
    }
    protected void cbl_state_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_state, cbl_state, txt_state, "State");
    }
    protected void cb_dept_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_dept, cbl_dept, txt_dept, "Department");
    }
    protected void cbl_dept_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_dept, cbl_dept, txt_dept, "Department");
    }
    protected void cb_desig_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_desig, cbl_desig, txt_desig, "Designation");
        stafftype();
        staffcategory();
    }
    protected void cbl_desig_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_desig, cbl_desig, txt_desig, "Designation");
        stafftype();
        staffcategory();
    }
    protected void cb_stype_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_stype, cbl_stype, txt_stype, "StaffType");
        staffcategory();
    }
    protected void cbl_stype_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_stype, cbl_stype, txt_stype, "StaffType");
        staffcategory();
    }
    protected void cb_scat_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_scat, cbl_scat, txt_scat, "StaffCategory");
    }
    protected void cbl_scat_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_scat, cbl_scat, txt_scat, "StaffCategory");
    }
    protected void cb_religieon1_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_religieon1, cbl_religieon1, txt_religieon, "Religion");
    }
    protected void cbl_religieon1_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_religieon1, cbl_religieon1, txt_religieon, "Religion");
    }
    protected void cb_comm1_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_comm1, cbl_comm1, txt_comm, "Community");
    }
    protected void cbl_comm1_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_comm1, cbl_comm1, txt_comm, "Community");
    }
    protected void cb_caste1_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_caste1, cbl_caste1, txt_caste, "Caste");
    }
    protected void cbl_caste1_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_caste1, cbl_caste1, txt_caste, "Caste");
    }
    protected void cb_qual1_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_qual1, cbl_qual1, txt_qual, "Qualification");
    }
    protected void cbl_qual1_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_qual1, cbl_qual1, txt_qual, "Qualification");
    }
    protected void cb_fsub1_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_fsub1, cbl_fsub1, txt_fsub, "Subjects");
    }
    protected void cbl_fsub1_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_fsub1, cbl_fsub1, txt_fsub, "Subjects");
    }
    protected void cb_qual_OnCheckedChanged(object sender, EventArgs e)
    {
        if (cb_qual.Checked == true)
        {
            Qualification();
            txt_qual.Enabled = true;
        }
        else
        {
            cbl_qual1.Items.Clear();
            txt_qual.Text = "--Select--";
            txt_qual.Enabled = false;
        }
    }
    protected void cb_religieon_OnCheckedChanged(object sender, EventArgs e)
    {
        if (cb_religieon.Checked == true)
        {
            Religieon();
            txt_religieon.Enabled = true;
        }
        else
        {
            cbl_religieon1.Items.Clear();
            txt_religieon.Text = "--Select--";
            txt_religieon.Enabled = false;
        }
    }
    protected void cb_comm_OnCheckedChanged(object sender, EventArgs e)
    {
        if (cb_comm.Checked == true)
        {
            Community();
            txt_comm.Enabled = true;
        }
        else
        {
            cbl_comm1.Items.Clear();
            txt_comm.Text = "--Select--";
            txt_comm.Enabled = false;
        }
    }
    protected void cb_caste_OnCheckedChanged(object sender, EventArgs e)
    {
        if (cb_caste.Checked == true)
        {
            Caste();
            txt_caste.Enabled = true;
        }
        else
        {
            cbl_caste1.Items.Clear();
            txt_caste.Text = "--Select--";
            txt_caste.Enabled = false;
        }
    }
    protected void cb_fsub_OnCheckedChanged(object sender, EventArgs e)
    {
        if (cb_fsub.Checked == true)
        {
            FamiliarSubject();
            txt_fsub.Enabled = true;
        }
        else
        {
            cbl_fsub1.Items.Clear();
            txt_fsub.Text = "--Select--";
            txt_fsub.Enabled = false;
        }
    }
    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        string coll = ddlcollege.SelectedItem.Value.ToString();
        binddept();
        designation();
        stafftype();
        staffcategory();
        bindblood();
        bindmstatus();
        Religieon();
        Community();
        Caste();
        bindnational();
        FamiliarSubject();
        Qualification();
        bindexpin();
        bindcity();
        binddistrict();
        bindstate();
        bindgender();
        bindstfstatus();
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        string collcode = Convert.ToString(ddlcollege.SelectedValue);
        try
        {
            string desigcode = "";
            string deptcode = "";
            string stype = "";
            string scat = "";
            string religion = "";
            string community = "";
            string caste = "";
            string bldgrp = "";
            string marital = "";
            string nationality = "";
            string fsub = "";
            string qual = "";
            string exp = "";
            string city = "";
            string dist = "";
            string state = "";
            string gender = "";
            string stfstatus = "";
            int bcount = 0;
            int marcount = 0;
            int relcount = 0;
            int commcount = 0;
            int castecount = 0;
            int nationcount = 0;
            int citycount = 0;
            int discount = 0;
            int statecount = 0;
            int fsubcount = 0;
            int qualcount = 0;
            int expcount = 0;
            int deptcount = 0;
            int desigcount = 0;
            int stypcount = 0;
            int scatcount = 0;
            int gendercount = 0;
            int stfcount = 0;
            Fpspread2.Visible = false;
            temphas = (Hashtable)ViewState["hasval"];
            deptcode = GetSelectedItemsValueAsString(cbl_dept, out deptcount);
            desigcode = GetSelectedItemsValueAsString(cbl_desig, out desigcount);
            stype = GetSelectedItemsValueAsString(cbl_stype, out stypcount);
            scat = GetSelectedItemsValueAsString(cbl_scat, out scatcount);
            religion = GetSelectedItemsText(cbl_religieon1, out relcount);
            community = GetSelectedItemsText(cbl_comm1, out commcount);
            caste = GetSelectedItemsText(cbl_caste1, out castecount);
            bldgrp = GetSelectedItemsText(cbl_bgoup, out bcount);
            marital = GetSelectedItemsText(cbl_marital, out marcount);
            nationality = GetSelectedItemsText(cbl_nation, out nationcount);
            if (cb_fsub.Checked == true)
            {
                if (cbl_fsub1.Items.Count > 0 && txt_fsub.Text.Trim() != "--Select--")
                {
                    for (i = 0; i < cbl_fsub1.Items.Count; i++)
                    {
                        if (cbl_fsub1.Items[i].Selected == true)
                        {
                            string getval = Convert.ToString(temphas[Convert.ToString(cbl_fsub1.Items[i].Text)]);
                            if (!hat1.ContainsKey(Convert.ToString(cbl_fsub1.Items[i].Text)))
                            {
                                fsubcount++;
                                hat1.Add(Convert.ToString(cbl_fsub1.Items[i].Text), getval);
                                if (fsub.Trim() == "")
                                {
                                    fsub = "" + Convert.ToString(cbl_fsub1.Items[i].Text) + "";
                                }
                                else
                                {
                                    fsub = fsub + "," + Convert.ToString(cbl_fsub1.Items[i].Text) + "";
                                }
                            }
                        }
                    }
                }
            }
            qual = GetSelectedItemsText(cbl_qual1, out qualcount);
            exp = GetSelectedItemsText(cbl_exp, out expcount);
            city = GetSelectedItemsText(cbl_city, out citycount);
            dist = GetSelectedItemsText(cbl_dis, out discount);
            state = GetSelectedItemsText(cbl_state, out statecount);
            gender = GetSelectedItemsText(chklstgender, out gendercount);
            stfstatus = GetSelectedItemsText(chklststfstatus, out stfcount);
            string goquery = "";
            goquery = "select dept_code,dept_name from hrdept_master where college_code='" + collcode + "'";
            if (deptcode.Trim() != "")
            {
                goquery += " and dept_code in('" + deptcode + "')";
            }
            goquery += " group by dept_code ,dept_name order by cast(dept_code as bigint)";
            goquery += " select count(t.staff_code) as Staff,t.dept_code,t.desig_code,t.stftype,t.category_code,sa.bldgrp,sa.martial_status,sa.religion,sa.Community,sa.Caste,sa.Nationality,sa.ccity,sa.cdistrict,sa.cstate,sa.yofexp,sa.subjects,sa.sex,Cast(ISNULL(s.resign,'0') as varchar) as resign,cast(ISNULL(s.settled,'0') as varchar) as settled,cast(ISNULL(s.Discontinue,'0') as varchar) as Discontinue from staff_appl_master sa,staffmaster s,stafftrans t,hrdept_master h,desig_master d,staffcategorizer sc where s.staff_code =t.staff_code and s.appl_no =sa.appl_no and h.dept_code =t.dept_code and t.desig_code =d.desig_code and sc.category_code =t.category_code and s.college_code=h.college_code and s.college_code=d.collegeCode and s.college_code=sc.college_code and t.latestrec =1";
            if (cbstfstatus.Checked == false || txtstfstatus.Text == "--Select--")
            {
                goquery += " and ((resign =0 and settled =0) and (Discontinue=0 or Discontinue is null))";
            }
            if (cbstfstatus.Checked)
            {
                if (chklststfstatus.Items[0].Selected == true && chklststfstatus.Items[1].Selected == true)
                    goquery += " and ((isnull(s.resign,'0')=1 and ISNULL(s.settled,'0')=1) or (isnull(s.Discontinue,'0')=1))";
                else if (chklststfstatus.Items[0].Selected == true)
                    goquery += " and (isnull(s.resign,'0')=1 and ISNULL(s.settled,'0')=1) ";
                else if (chklststfstatus.Items[1].Selected == true)
                    goquery += " and (isnull(s.Discontinue,'0')=1)";
            }
            goquery += " group by t.desig_code ,t.dept_code ,t.stftype,t.category_code,sa.bldgrp,sa.martial_status,sa.religion,sa.Community,sa.Caste,sa.Nationality,sa.ccity,sa.cdistrict,sa.cstate,sa.yofexp,sa.subjects,sa.sex,s.resign,s.settled,s.Discontinue";
            ds.Clear();
            ds = d2.select_method_wo_parameter(goquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                Fpspread1.Sheets[0].RowCount = 0;
                Fpspread1.Sheets[0].ColumnCount = 0;
                Fpspread1.CommandBar.Visible = false;
                Fpspread1.Sheets[0].AutoPostBack = true;
                Fpspread1.Sheets[0].ColumnHeader.RowCount = 2;
                Fpspread1.Sheets[0].RowHeader.Visible = false;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.Black;
                darkstyle.Font.Bold = true;
                darkstyle.Font.Name = "Book Antiqua";
                darkstyle.Font.Size = FontUnit.Medium;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                Fpspread1.Sheets[0].ColumnCount = desigcount + 3 + stypcount + scatcount + bcount + marcount + relcount + commcount + castecount + nationcount + fsubcount + expcount + citycount + discount + statecount + gendercount + stfcount;
                Fpspread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                Fpspread1.Columns[0].Width = 75;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Department";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                Fpspread1.Columns[1].Width = 250;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Count";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                Fpspread1.Columns[2].Width = 225;
                int count = 0;
                if (desigcode.Trim() != "")
                {
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Designation";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 1, desigcount);
                    Fpspread1.Columns[3].Width = 150;
                    for (int g = 0; g < cbl_desig.Items.Count; g++)
                    {
                        if (cbl_desig.Items[g].Selected == true)
                        {
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Text = "Designation";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, 3 + count].Text = Convert.ToString(cbl_desig.Items[g].Text);
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, 3 + count].Tag = Convert.ToString(cbl_desig.Items[g].Value) + "," + "t.desig_code";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Note = Convert.ToString(cbl_desig.Items[g].Value) + "%" + "desig_code";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, 3 + count].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Columns[3 + count].Width = 250;
                            count++;
                        }
                    }
                }
                if (stype.Trim() != "")
                {
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Text = "Staff type";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3 + count, 1, stypcount);
                    Fpspread1.Columns[1 + count].Width = 150;
                    for (int t = 0; t < cbl_stype.Items.Count; t++)
                    {
                        if (cbl_stype.Items[t].Selected == true)
                        {
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Text = "Staff type";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Text = Convert.ToString(cbl_stype.Items[t].Text);
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Tag = Convert.ToString(cbl_stype.Items[t].Text) + "," + "t.stftype";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Note = Convert.ToString(cbl_stype.Items[t].Text) + "%" + "stftype";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Columns[count + 3].Width = 250;
                            count++;
                        }
                    }
                }
                if (scat.Trim() != "")
                {
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Text = "Staff Category";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3 + count, 1, scatcount);
                    Fpspread1.Columns[1 + count].Width = 150;
                    for (int c = 0; c < cbl_scat.Items.Count; c++)
                    {
                        if (cbl_scat.Items[c].Selected == true)
                        {
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Text = "Staff Category";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Text = Convert.ToString(cbl_scat.Items[c].Text);
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Tag = Convert.ToString(cbl_scat.Items[c].Value) + "," + "t.category_code";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Note = Convert.ToString(cbl_scat.Items[c].Value) + "%" + "category_code";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Columns[count + 3].Width = 250;
                            count++;
                        }
                    }
                }
                if (bldgrp.Trim() != "")
                {
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Text = "Blood Group";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3 + count, 1, bcount);
                    for (int ib = 0; ib < cbl_bgoup.Items.Count; ib++)
                    {
                        if (cbl_bgoup.Items[ib].Selected == true)
                        {
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Text = "Blood Group";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Text = Convert.ToString(cbl_bgoup.Items[ib].Text);
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Tag = Convert.ToString(cbl_bgoup.Items[ib].Text) + "," + "sa.bldgrp";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Note = Convert.ToString(cbl_bgoup.Items[ib].Text) + "%" + "bldgrp";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Columns[count + 3].Width = 250;
                            count++;
                        }
                    }
                }
                if (marital.Trim() != "")
                {
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Text = "Martial Status";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3 + count, 1, marcount);
                    for (int ib = 0; ib < cbl_marital.Items.Count; ib++)
                    {
                        if (cbl_marital.Items[ib].Selected == true)
                        {
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Text = "Martial Status";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Text = Convert.ToString(cbl_marital.Items[ib].Text);
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Tag = Convert.ToString(cbl_marital.Items[ib].Text) + "," + "sa.martial_status";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Note = Convert.ToString(cbl_marital.Items[ib].Text) + "%" + "martial_status";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Columns[count + 3].Width = 250;
                            count++;
                        }
                    }
                }
                if (religion.Trim() != "")
                {
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Text = "Religion";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3 + count, 1, relcount);
                    for (int ib = 0; ib < cbl_religieon1.Items.Count; ib++)
                    {
                        if (cbl_religieon1.Items[ib].Selected == true)
                        {
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Text = "Religion";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Text = Convert.ToString(cbl_religieon1.Items[ib].Text);
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Tag = Convert.ToString(cbl_religieon1.Items[ib].Text) + "," + "sa.religion";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Note = Convert.ToString(cbl_religieon1.Items[ib].Text) + "%" + "religion";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Columns[count + 3].Width = 250;
                            count++;
                        }
                    }
                }
                if (community.Trim() != "")
                {
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Text = "Community";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3 + count, 1, commcount);
                    for (int ib = 0; ib < cbl_comm1.Items.Count; ib++)
                    {
                        if (cbl_comm1.Items[ib].Selected == true)
                        {
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Text = "Community";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Text = Convert.ToString(cbl_comm1.Items[ib].Text);
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Tag = Convert.ToString(cbl_comm1.Items[ib].Text) + "," + "sa.Community";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Note = Convert.ToString(cbl_comm1.Items[ib].Text) + "%" + "Community";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Columns[count + 3].Width = 250;
                            count++;
                        }
                    }
                }
                if (caste.Trim() != "")
                {
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Text = "Caste";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3 + count, 1, castecount);
                    for (int ib = 0; ib < cbl_caste1.Items.Count; ib++)
                    {
                        if (cbl_caste1.Items[ib].Selected == true)
                        {
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Text = "Caste";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Text = Convert.ToString(cbl_caste1.Items[ib].Text);
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Tag = Convert.ToString(cbl_caste1.Items[ib].Text) + "," + "sa.Caste";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Note = Convert.ToString(cbl_caste1.Items[ib].Text) + "%" + "Caste";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Columns[count + 3].Width = 250;
                            count++;
                        }
                    }
                }
                if (nationality.Trim() != "")
                {
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Text = "Nationality";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3 + count, 1, nationcount);
                    for (int ib = 0; ib < cbl_nation.Items.Count; ib++)
                    {
                        if (cbl_nation.Items[ib].Selected == true)
                        {
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Text = "Nationality";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Text = Convert.ToString(cbl_nation.Items[ib].Text);
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Tag = Convert.ToString(cbl_nation.Items[ib].Text) + "," + "sa.Nationality";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Note = Convert.ToString(cbl_nation.Items[ib].Text) + "%" + "Nationality";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Columns[count + 3].Width = 250;
                            count++;
                        }
                    }
                }
                if (fsub.Trim() != "")
                {
                    string[] splsub = fsub.Split(',');
                    string getsubval = "";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Text = "Familiar Subjects";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3 + count, 1, fsubcount);
                    for (int ib = 0; ib < fsubcount; ib++)
                    {
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Text = "Familiar Subjects";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Text = Convert.ToString(splsub[ib]);
                        getsubval = Convert.ToString(hat1[Convert.ToString(splsub[ib])]);
                        Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Tag = getsubval + "," + "sa.subjects";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Note = getsubval + "%" + "subjects";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Columns[count + 3].Width = 250;
                        count++;
                    }
                }
                if (exp.Trim() != "")
                {
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Text = "Experience";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3 + count, 1, expcount);
                    for (int ib = 0; ib < cbl_exp.Items.Count; ib++)
                    {
                        if (cbl_exp.Items[ib].Selected == true)
                        {
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Text = "Experience";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Text = Convert.ToString(cbl_exp.Items[ib].Text);
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Tag = Convert.ToString(cbl_exp.Items[ib].Text) + "," + "sa.yofexp";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Note = Convert.ToString(cbl_exp.Items[ib].Text) + "%" + "yofexp";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Columns[count + 3].Width = 250;
                            count++;
                        }
                    }
                }
                if (city.Trim() != "")
                {
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Text = "City";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3 + count, 1, citycount);
                    for (int ib = 0; ib < cbl_city.Items.Count; ib++)
                    {
                        if (cbl_city.Items[ib].Selected == true)
                        {
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Text = "City";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Text = Convert.ToString(cbl_city.Items[ib].Text);
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Tag = Convert.ToString(cbl_city.Items[ib].Text) + "," + "sa.ccity";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Note = Convert.ToString(cbl_city.Items[ib].Text) + "%" + "ccity";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Columns[count + 3].Width = 250;
                            count++;
                        }
                    }
                }
                if (dist.Trim() != "")
                {
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Text = "District";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3 + count, 1, discount);
                    for (int ib = 0; ib < cbl_dis.Items.Count; ib++)
                    {
                        if (cbl_dis.Items[ib].Selected == true)
                        {
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Text = "District";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Text = Convert.ToString(cbl_dis.Items[ib].Text);
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Tag = Convert.ToString(cbl_dis.Items[ib].Text) + "," + "sa.cdistrict";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Note = Convert.ToString(cbl_dis.Items[ib].Text) + "%" + "cdistrict";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Columns[count + 3].Width = 250;
                            count++;
                        }
                    }
                }
                if (state.Trim() != "")
                {
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Text = "State";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3 + count, 1, statecount);
                    for (int ib = 0; ib < cbl_state.Items.Count; ib++)
                    {
                        if (cbl_state.Items[ib].Selected == true)
                        {
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Text = "State";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Text = Convert.ToString(cbl_state.Items[ib].Text);
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Tag = Convert.ToString(cbl_state.Items[ib].Text) + "," + "sa.cstate";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Note = Convert.ToString(cbl_state.Items[ib].Text) + "%" + "cstate";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Columns[count + 3].Width = 250;
                            count++;
                        }
                    }
                }
                if (gender.Trim() != "")
                {
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Text = "Gender";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3 + count, 1, gendercount);
                    for (int ib = 0; ib < chklstgender.Items.Count; ib++)
                    {
                        if (chklstgender.Items[ib].Selected == true)
                        {
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Text = "Gender";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Text = Convert.ToString(chklstgender.Items[ib].Text);
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Tag = Convert.ToString(chklstgender.Items[ib].Text) + "," + "sa.sex";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Note = Convert.ToString(chklstgender.Items[ib].Text) + "%" + "sex";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Columns[count + 3].Width = 100;
                            count++;
                        }
                    }
                }
                if (stfstatus.Trim() != "")
                {
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Text = "Staff Status";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3 + count, 1, stfcount);
                    for (int ib = 0; ib < chklststfstatus.Items.Count; ib++)
                    {
                        if (chklststfstatus.Items[ib].Selected == true)
                        {
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3 + count].Text = "Staff Status";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Text = Convert.ToString(chklststfstatus.Items[ib].Text);
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Tag = Convert.ToString(chklststfstatus.Items[ib].Value);
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].Note = Convert.ToString(chklststfstatus.Items[ib].Value);
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, count + 3].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Columns[count + 3].Width = 100;
                            count++;
                        }
                    }
                }
                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Fpspread1.Sheets[0].RowCount++;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["dept_code"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    string rowval = "";
                    string field = "";
                    string val = "";
                    for (int row = 2; row < Fpspread1.Sheets[0].Columns.Count; row++)
                    {
                        rowval = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[1, row].Note);
                        if (rowval.Trim() != "")
                        {
                            DataTable dnew = new DataTable();
                            int sum = 0;
                            string[] splrow = rowval.Split('%');
                            if (splrow.Length >= 2)
                            {
                                field = Convert.ToString(splrow[1]);
                                val = Convert.ToString(splrow[0]);
                                ds.Tables[1].DefaultView.RowFilter = field + "='" + Convert.ToString(val) + "' and dept_code='" + Convert.ToString(ds.Tables[0].Rows[i]["dept_code"]) + "'";
                                DataView dvcheck = new DataView();
                                dvcheck = ds.Tables[1].DefaultView;
                                dnew = dvcheck.ToTable();
                            }
                            else
                            {
                                val = Convert.ToString(splrow[0]);
                                ds.Tables[1].DefaultView.RowFilter = "" + val + " and dept_code='" + Convert.ToString(ds.Tables[0].Rows[i]["dept_code"]) + "'";
                                DataView dvcheck = new DataView();
                                dvcheck = ds.Tables[1].DefaultView;
                                dnew = dvcheck.ToTable();
                            }
                            if (dnew.Rows.Count > 0)
                            {
                                sum = Convert.ToInt32(dnew.Compute("Sum(Staff)", ""));
                            }
                            if (sum != 0)
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, row].Text = Convert.ToString(sum);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, row].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, row].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, row].Font.Name = "Book Antiqua";
                            }
                            else
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, row].Text = "-";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, row].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, row].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, row].Font.Name = "Book Antiqua";
                            }
                        }
                        else
                        {
                            ds.Tables[1].DefaultView.RowFilter = "dept_code='" + Convert.ToString(ds.Tables[0].Rows[i]["dept_code"]) + "'";
                            DataView dvcheck = new DataView();
                            dvcheck = ds.Tables[1].DefaultView;
                            DataTable dnew = new DataTable();
                            dnew = dvcheck.ToTable();
                            int sum = 0;
                            if (dnew.Rows.Count > 0)
                            {
                                sum = Convert.ToInt32(dnew.Compute("Sum(Staff)", ""));
                            }
                            if (sum != 0)
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, row].Text = Convert.ToString(sum);
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, row].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, row].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, row].Font.Name = "Book Antiqua";
                            }
                            else
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, row].Text = "-";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, row].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, row].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, row].Font.Name = "Book Antiqua";
                            }
                        }
                    }
                }
                Fpspread1.Sheets[0].RowCount++;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = "Grand Total";
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - 1, 0, 1, 2);
                string PrevHeaderTagvalue = string.Empty;
                Fpspread1.SaveChanges();
                for (int col = 2; col < Fpspread1.Sheets[0].Columns.Count; col++)
                {
                    double total = 0;
                    double grandtotal = 0;
                    for (int ro = 0; ro < Fpspread1.Sheets[0].Rows.Count; ro++)
                    {
                        double.TryParse(Convert.ToString(Fpspread1.Sheets[0].Cells[ro, col].Text), out total);
                        grandtotal = grandtotal + total;
                    }
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(grandtotal);
                    string HeaderTagvalue = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[0, col].Text);
                    if (grandtotal == 0)
                        if (PrevHeaderTagvalue == HeaderTagvalue)
                            Fpspread1.Sheets[0].Columns[col].Visible = false;
                    //else
                    //    Fpspread1.Sheets[0].Columns[col].Visible = false;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].Font.Bold = true;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, col].Font.Name = "Book Antiqua";
                    PrevHeaderTagvalue = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[0, col].Text);
                }
                Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                imgbtn_columsetting.Visible = true;
                Fpspread1.Visible = true;
                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                lblspread1_err.Visible = false;
                Fpspread1.SaveChanges();
            }
            else
            {
                lblspread1_err.Visible = true;
                lblspread1_err.Text = "No Record Found!";
                Fpspread1.Visible = false;
                imgbtn_columsetting.Visible = false;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collcode, "Staff_StrengthMaster.aspx");
        }
    }
    protected void Cell_Click(object sender, EventArgs e)
    {
        check = true;
        check1 = false;
    }
    protected void Cell1_Click(object sender, EventArgs e)
    {
        check = false;
        check1 = true;
    }
    public bool checkedOK()
    {
        bool Ok = false;
        Fpspread2.SaveChanges();
        for (i = 1; i < Fpspread2.Sheets[0].Rows.Count; i++)
        {
            string check = Convert.ToString(Fpspread2.Sheets[0].Cells[i, 1].Value);
            if (check == "1")
            {
                Ok = true;
            }
        }
        return Ok;
    }
    protected void FpSpread2_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        lblspread1_err.Visible = false;
        string value = Convert.ToString(Fpspread2.Sheets[0].Cells[0, 1].Value);
        if (value == "1")
        {
            for (int K = 1; K < Fpspread2.Sheets[0].Rows.Count; K++)
            {
                Fpspread2.Sheets[0].Cells[K, 1].Value = 1;
            }
        }
        else
        {
            for (int K = 1; K < Fpspread2.Sheets[0].Rows.Count; K++)
            {
                Fpspread2.Sheets[0].Cells[K, 1].Value = 0;
            }
        }
    }
    private string getqval(string textcode)
    {
        string txtval = "";
        string newcollcode = Convert.ToString(ddlcollege.SelectedItem.Value);
        try
        {
            txtval = d2.GetFunction("select TextVal from TextValTable where TextCode='" + textcode + "' and college_code='" + newcollcode + "'");
        }
        catch { }
        return txtval;
    }
    protected void FpSpread1_Render(object sender, EventArgs e)
    {
        string collcode = Convert.ToString(ddlcollege.SelectedValue);
        try
        {
            int depcount = 0;
            string dpt = "";
            string dct = "";
            string val = "";
            string name = "";
            string dept_Code = "";
            dept_Code = GetSelectedItemsValueAsString(cbl_dept, out depcount);
            if (check == true)
            {
                string activerow = "";
                string activecol = "";
                activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
                activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
                int a = Convert.ToInt32(activecol);
                if (a > 1)
                {
                    if (lb_column1.Items.Count > 0)
                    {
                        dpt = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                        dct = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[1, Convert.ToInt32(activecol)].Tag);
                        if (dct.Contains(';'))
                        {
                            string[] abc = dct.Split(';');
                            if (abc.Length > 1)
                            {
                                val = abc[0] + ";";
                                name = abc[1].Split(',')[1];
                            }
                        }
                        else
                        {
                            string[] abc = dct.Split(',');
                            if (abc.Length > 1)
                            {
                                val = abc[0];
                                name = abc[1];
                            }
                        }
                        string query = "";
                        if (Convert.ToInt32(activerow) == Fpspread1.Sheets[0].RowCount - 1 && a > 2)
                        {
                            if (name.Trim() != "" && val.Trim() != "")
                            {
                                query = "select staff_name,s.staff_code,h.dept_name,d.desig_name,(select TextVal from TextValTable where sa.Title=CAST(TextCode as varchar)) as Title,sa.NameAcr,sa.staff_type,category_name,per_address,comm_address,Convert(varchar(10),date_of_birth,103) as date_of_birth,Convert(varchar(10),join_date,103) as join_date,com_mobileno,email,sex,martial_status,sa.appl_no,sa.appl_id,sa.dept_name,sa.desig_name,staff_type,Convert(varchar(10),dateofapply,103) as dateofapply,Convert(varchar(10),exp_joindate,103) as exp_joindate,sex,(select TextVal from TextValTable where sa.Caste=CAST(TextCode as varchar)) as Caste,(select TextVal from TextValTable where sa.religion=CAST(TextCode as varchar)) as religion,(select TextVal from TextValTable where sa.Community=CAST(TextCode as varchar)) as Community,martial_status,email,com_mobileno,(select TextVal from TextValTable where sa.Nationality=CAST(TextCode as varchar)) as Nationality,per_address,comm_address,qualification,(select TextVal from TextValTable where sa.subjects=CAST(TextCode as varchar)) as subjects,yofexp,(select TextVal from TextValTable where sa.bldgrp=CAST(TextCode as varchar)) as bldgrp,sa.adharcardno,CASE WHEN AICTE_Grants = '1' THEN 'Yes' ELSE 'No' END AICTE_Grants,CASE WHEN AICTE_Comm = '1' THEN 'Yes' ELSE 'No' END AICTE_Comm,CASE WHEN Minority = '1' THEN 'Yes' ELSE 'No' END IsMin,'' as IsFirstYr,'' as IsFYCommon,'' as FYCommonSub,CASE WHEN IsPhy = '1' THEN 'Yes' ELSE 'No' END IsPhy,s.PANGIRNumber,CASE WHEN StfNature = 'full' THEN 'Full Time' ELSE 'Part Time' END StfNature,StfStatus,FacultyType,PayType,Programme,CASE WHEN PayMode = 0 THEN 'Cash' WHEN PayMode = 1 THEN 'Cheque' ELSE 'Credit to Bank Account' END PayMode,s.PFNumber,s.BankAccount,(select TextVal from TextValTable where s.Bank_Name=CAST(TextCode as Varchar)) as Bank_Name,(select TextVal from TextValTable where s.Branch_Name=CAST(TextCode as varchar)) as Branch_Name,s.IFSC_Code,CASE WHEN IsPhy = '1' THEN 'Yes' ELSE 'No' END IsPhy,CASE WHEN Minority = '1' THEN 'Yes' ELSE 'No' END IsMin,'' as IsFirstYr,'' as IsFYCommon,'' as FYCommonSub,CASE WHEN AICTE_Comm = 1 THEN 'Yes' ELSE 'No' END AICTE_Comm,CASE WHEN AICTE_Grants = '1' THEN 'Yes' ELSE 'No' END AICTE_Grants,Per_MobileNo from staffmaster s,stafftrans t,staff_appl_master sa,hrdept_master h,desig_master d,staffcategorizer c where s.staff_code =t.staff_code and s.appl_no =sa.appl_no and h.dept_code =t.dept_code and t.desig_code =d.desig_code and c.category_code =t.category_code and s.college_code=h.college_code and s.college_code=d.collegeCode and s.college_code=c.college_code and t.latestrec =1 and " + name + " ='" + val + "' and t.dept_Code in('" + dept_Code + "') and s.college_Code='" + collcode + "'";  //
                                //if (cbstfstatus.Checked == false || txtstfstatus.Text == "--Select--")
                                //{
                                //    query = query + " and ((resign =0 and settled =0) and (Discontinue=0 or Discontinue is null))";
                                //}
                            }
                            else
                            {
                                query = "select staff_name,s.staff_code,h.dept_name,d.desig_name,(select TextVal from TextValTable where sa.Title=CAST(TextCode as varchar)) as Title,sa.NameAcr,sa.staff_type,category_name,per_address,comm_address,Convert(varchar(10),date_of_birth,103) as date_of_birth,Convert(varchar(10),join_date,103) as join_date,com_mobileno,email,sex,martial_status,sa.appl_no,sa.appl_id,sa.dept_name,sa.desig_name,staff_type,Convert(varchar(10),dateofapply,103) as dateofapply,Convert(varchar(10),exp_joindate,103) as exp_joindate,sex,(select TextVal from TextValTable where sa.Caste=CAST(TextCode as varchar)) as Caste,(select TextVal from TextValTable where sa.religion=CAST(TextCode as varchar)) as religion,(select TextVal from TextValTable where sa.Community=CAST(TextCode as varchar)) as Community,martial_status,email,com_mobileno,(select TextVal from TextValTable where sa.Nationality=CAST(TextCode as varchar)) as Nationality,per_address,comm_address,qualification,(select TextVal from TextValTable where sa.subjects=CAST(TextCode as varchar)) as subjects,yofexp,(select TextVal from TextValTable where sa.bldgrp=CAST(TextCode as varchar)) as bldgrp,sa.adharcardno,CASE WHEN AICTE_Grants = '1' THEN 'Yes' ELSE 'No' END AICTE_Grants,CASE WHEN AICTE_Comm = '1' THEN 'Yes' ELSE 'No' END AICTE_Comm,CASE WHEN Minority = '1' THEN 'Yes' ELSE 'No' END IsMin,'' as IsFirstYr,'' as IsFYCommon,'' as FYCommonSub,CASE WHEN IsPhy = '1' THEN 'Yes' ELSE 'No' END IsPhy,s.PANGIRNumber,CASE WHEN StfNature = 'full' THEN 'Full Time' ELSE 'Part Time' END StfNature,StfStatus,FacultyType,PayType,Programme,CASE WHEN PayMode = 0 THEN 'Cash' WHEN PayMode = 1 THEN 'Cheque' ELSE 'Credit to Bank Account' END PayMode,s.PFNumber,s.BankAccount,(select TextVal from TextValTable where s.Bank_Name=CAST(TextCode as Varchar)) as Bank_Name,(select TextVal from TextValTable where s.Branch_Name=CAST(TextCode as varchar)) as Branch_Name,s.IFSC_Code,CASE WHEN IsPhy = '1' THEN 'Yes' ELSE 'No' END IsPhy,CASE WHEN Minority = '1' THEN 'Yes' ELSE 'No' END IsMin,'' as IsFirstYr,'' as IsFYCommon,'' as FYCommonSub,CASE WHEN AICTE_Comm = 1 THEN 'Yes' ELSE 'No' END AICTE_Comm,CASE WHEN AICTE_Grants = '1' THEN 'Yes' ELSE 'No' END AICTE_Grants,Per_MobileNo from staffmaster s,stafftrans t,staff_appl_master sa,hrdept_master h,desig_master d,staffcategorizer c where s.staff_code =t.staff_code and s.appl_no =sa.appl_no and h.dept_code =t.dept_code and t.desig_code =d.desig_code and c.category_code =t.category_code and s.college_code=h.college_code and s.college_code=d.collegeCode and s.college_code=c.college_code  and t.latestrec =1 and " + dct + " and t.dept_Code in('" + dept_Code + "') and s.college_Code='" + collcode + "'";  //and ((resign =0 and settled =0) and (Discontinue=0 or Discontinue is null))
                                //if (cbstfstatus.Checked == false || txtstfstatus.Text == "--Select--")
                                //{
                                //    query = query + " and ((resign =0 and settled =0) and (Discontinue=0 or Discontinue is null))";
                                //}
                            }
                        }
                        else if (a > 2)
                        {
                            if (name.Trim() != "" && val.Trim() != "")
                            {
                                query = "select staff_name,s.staff_code,h.dept_name,d.desig_name,(select TextVal from TextValTable where sa.Title=CAST(TextCode as varchar)) as Title,sa.NameAcr,sa.staff_type,category_name,per_address,comm_address,Convert(varchar(10),date_of_birth,103) as date_of_birth,Convert(varchar(10),join_date,103) as join_date,com_mobileno,email,sex,martial_status,sa.appl_no,sa.appl_id,sa.dept_name,sa.desig_name,staff_type,Convert(varchar(10),dateofapply,103) as dateofapply,Convert(varchar(10),exp_joindate,103) as exp_joindate,sex,(select TextVal from TextValTable where sa.Caste=CAST(TextCode as varchar)) as Caste,(select TextVal from TextValTable where sa.religion=CAST(TextCode as varchar)) as religion,(select TextVal from TextValTable where sa.Community=CAST(TextCode as varchar)) as Community,martial_status,email,com_mobileno,(select TextVal from TextValTable where sa.Nationality=CAST(TextCode as varchar)) as Nationality,per_address,comm_address,qualification,(select TextVal from TextValTable where sa.subjects=CAST(TextCode as varchar)) as subjects,yofexp,(select TextVal from TextValTable where sa.bldgrp=CAST(TextCode as varchar)) as bldgrp,sa.adharcardno,CASE WHEN AICTE_Grants = '1' THEN 'Yes' ELSE 'No' END AICTE_Grants,CASE WHEN AICTE_Comm = '1' THEN 'Yes' ELSE 'No' END AICTE_Comm,CASE WHEN Minority = '1' THEN 'Yes' ELSE 'No' END IsMin,'' as IsFirstYr,'' as IsFYCommon,'' as FYCommonSub,CASE WHEN IsPhy = '1' THEN 'Yes' ELSE 'No' END IsPhy,s.PANGIRNumber,CASE WHEN StfNature = 'full' THEN 'Full Time' ELSE 'Part Time' END StfNature,StfStatus,FacultyType,PayType,Programme,CASE WHEN PayMode = 0 THEN 'Cash' WHEN PayMode = 1 THEN 'Cheque' ELSE 'Credit to Bank Account' END PayMode,s.PFNumber,s.BankAccount,(select TextVal from TextValTable where s.Bank_Name=CAST(TextCode as Varchar)) as Bank_Name,(select TextVal from TextValTable where s.Branch_Name=CAST(TextCode as varchar)) as Branch_Name,s.IFSC_Code,CASE WHEN IsPhy = '1' THEN 'Yes' ELSE 'No' END IsPhy,CASE WHEN Minority = '1' THEN 'Yes' ELSE 'No' END IsMin,'' as IsFirstYr,'' as IsFYCommon,'' as FYCommonSub,CASE WHEN AICTE_Comm = 1 THEN 'Yes' ELSE 'No' END AICTE_Comm,CASE WHEN AICTE_Grants = '1' THEN 'Yes' ELSE 'No' END AICTE_Grants,Per_MobileNo from staffmaster s,stafftrans t,staff_appl_master sa,hrdept_master h,desig_master d,staffcategorizer c where s.staff_code =t.staff_code and s.appl_no =sa.appl_no and h.dept_code =t.dept_code and t.desig_code =d.desig_code and c.category_code =t.category_code and s.college_code=h.college_code and s.college_code=d.collegeCode and s.college_code=c.college_code and t.latestrec =1 and h.dept_code ='" + dpt + "' and " + name + " ='" + val + "' and s.college_Code='" + collcode + "'";  //and ((resign =0 and settled =0) and (Discontinue=0 or Discontinue is null))
                                //if (cbstfstatus.Checked == false || txtstfstatus.Text == "--Select--")
                                //{
                                //    query = query + " and ((resign =0 and settled =0) and (Discontinue=0 or Discontinue is null))";
                                //}
                            }
                            else
                            {
                                query = "select staff_name,s.staff_code,h.dept_name,d.desig_name,(select TextVal from TextValTable where sa.Title=CAST(TextCode as varchar)) as Title,sa.NameAcr,sa.staff_type,category_name,per_address,comm_address,Convert(varchar(10),date_of_birth,103) as date_of_birth,Convert(varchar(10),join_date,103) as join_date,com_mobileno,email,sex,martial_status,sa.appl_no,sa.appl_id,sa.dept_name,sa.desig_name,staff_type,Convert(varchar(10),dateofapply,103) as dateofapply,Convert(varchar(10),exp_joindate,103) as exp_joindate,sex,(select TextVal from TextValTable where sa.Caste=CAST(TextCode as varchar)) as Caste,(select TextVal from TextValTable where sa.religion=CAST(TextCode as varchar)) as religion,(select TextVal from TextValTable where sa.Community=CAST(TextCode as varchar)) as Community,martial_status,email,com_mobileno,(select TextVal from TextValTable where sa.Nationality=CAST(TextCode as varchar)) as Nationality,per_address,comm_address,qualification,(select TextVal from TextValTable where sa.subjects=CAST(TextCode as varchar)) as subjects,yofexp,(select TextVal from TextValTable where sa.bldgrp=CAST(TextCode as varchar)) as bldgrp,sa.adharcardno,CASE WHEN AICTE_Grants = '1' THEN 'Yes' ELSE 'No' END AICTE_Grants,CASE WHEN AICTE_Comm = '1' THEN 'Yes' ELSE 'No' END AICTE_Comm,CASE WHEN Minority = '1' THEN 'Yes' ELSE 'No' END IsMin,'' as IsFirstYr,'' as IsFYCommon,'' as FYCommonSub,CASE WHEN IsPhy = '1' THEN 'Yes' ELSE 'No' END IsPhy,s.PANGIRNumber,CASE WHEN StfNature = 'full' THEN 'Full Time' ELSE 'Part Time' END StfNature,StfStatus,FacultyType,PayType,Programme,CASE WHEN PayMode = 0 THEN 'Cash' WHEN PayMode = 1 THEN 'Cheque' ELSE 'Credit to Bank Account' END PayMode,s.PFNumber,s.BankAccount,(select TextVal from TextValTable where s.Bank_Name=CAST(TextCode as Varchar)) as Bank_Name,(select TextVal from TextValTable where s.Branch_Name=CAST(TextCode as varchar)) as Branch_Name,s.IFSC_Code,CASE WHEN IsPhy = '1' THEN 'Yes' ELSE 'No' END IsPhy,CASE WHEN Minority = '1' THEN 'Yes' ELSE 'No' END IsMin,'' as IsFirstYr,'' as IsFYCommon,'' as FYCommonSub,CASE WHEN AICTE_Comm = 1 THEN 'Yes' ELSE 'No' END AICTE_Comm,CASE WHEN AICTE_Grants = '1' THEN 'Yes' ELSE 'No' END AICTE_Grants,Per_MobileNo from staffmaster s,stafftrans t,staff_appl_master sa,hrdept_master h,desig_master d,staffcategorizer c where s.staff_code =t.staff_code and s.appl_no =sa.appl_no and h.dept_code =t.dept_code and t.desig_code =d.desig_code and c.category_code =t.category_code and s.college_code=h.college_code and s.college_code=d.collegeCode and s.college_code=c.college_code  and t.latestrec =1 and h.dept_code ='" + dpt + "' and " + dct + " and s.college_Code='" + collcode + "'";   //and ((resign =0 and settled =0) and (Discontinue=0 or Discontinue is null))
                                //if (cbstfstatus.Checked == false || txtstfstatus.Text == "--Select--")
                                //{
                                //    query = query + " and ((resign =0 and settled =0) and (Discontinue=0 or Discontinue is null))";
                                //}
                            }
                        }
                        else if (Convert.ToInt32(activerow) == Fpspread1.Sheets[0].RowCount - 1 && a == 2)
                        {
                            query = "select staff_name,s.staff_code,h.dept_name,d.desig_name,(select TextVal from TextValTable where sa.Title=CAST(TextCode as varchar)) as Title,sa.NameAcr,sa.staff_type,category_name,per_address,comm_address,Convert(varchar(10),date_of_birth,103) as date_of_birth,Convert(varchar(10),join_date,103) as join_date,com_mobileno,email,sex,martial_status,sa.appl_no,sa.appl_id,sa.dept_name,sa.desig_name,staff_type,Convert(varchar(10),dateofapply,103) as dateofapply,Convert(varchar(10),exp_joindate,103) as exp_joindate,sex,(select TextVal from TextValTable where sa.Caste=CAST(TextCode as varchar)) as Caste,(select TextVal from TextValTable where sa.religion=CAST(TextCode as varchar)) as religion,(select TextVal from TextValTable where sa.Community=CAST(TextCode as varchar)) as Community,martial_status,email,com_mobileno,(select TextVal from TextValTable where sa.Nationality=CAST(TextCode as varchar)) as Nationality,per_address,comm_address,qualification,(select TextVal from TextValTable where sa.subjects=CAST(TextCode as varchar)) as subjects,yofexp,(select TextVal from TextValTable where sa.bldgrp=CAST(TextCode as varchar)) as bldgrp,sa.adharcardno,CASE WHEN AICTE_Grants = '1' THEN 'Yes' ELSE 'No' END AICTE_Grants,CASE WHEN AICTE_Comm = '1' THEN 'Yes' ELSE 'No' END AICTE_Comm,CASE WHEN Minority = '1' THEN 'Yes' ELSE 'No' END IsMin,'' as IsFirstYr,'' as IsFYCommon,'' as FYCommonSub,CASE WHEN IsPhy = '1' THEN 'Yes' ELSE 'No' END IsPhy,s.PANGIRNumber,CASE WHEN StfNature = 'full' THEN 'Full Time' ELSE 'Part Time' END StfNature,StfStatus,FacultyType,PayType,Programme,CASE WHEN PayMode = 0 THEN 'Cash' WHEN PayMode = 1 THEN 'Cheque' ELSE 'Credit to Bank Account' END PayMode,s.PFNumber,s.BankAccount,(select TextVal from TextValTable where s.Bank_Name=CAST(TextCode as Varchar)) as Bank_Name,(select TextVal from TextValTable where s.Branch_Name=CAST(TextCode as varchar)) as Branch_Name,s.IFSC_Code,CASE WHEN IsPhy = '1' THEN 'Yes' ELSE 'No' END IsPhy,CASE WHEN Minority = '1' THEN 'Yes' ELSE 'No' END IsMin,'' as IsFirstYr,'' as IsFYCommon,'' as FYCommonSub,CASE WHEN AICTE_Comm = 1 THEN 'Yes' ELSE 'No' END AICTE_Comm,CASE WHEN AICTE_Grants = '1' THEN 'Yes' ELSE 'No' END AICTE_Grants,Per_MobileNo from staffmaster s,stafftrans t,staff_appl_master sa,hrdept_master h,desig_master d,staffcategorizer c where s.staff_code =t.staff_code and s.appl_no =sa.appl_no and h.dept_code =t.dept_code and t.desig_code =d.desig_code and c.category_code =t.category_code and s.college_code=h.college_code and s.college_code=d.collegeCode and s.college_code=c.college_code  and t.latestrec =1 and t.dept_Code in('" + dept_Code + "') and s.college_Code='" + collcode + "'";  //and ((resign =0 and settled =0) and (Discontinue=0 or Discontinue is null))
                            //if (cbstfstatus.Checked == false || txtstfstatus.Text == "--Select--")
                            //{
                            //    query = query + " and ((resign =0 and settled =0) and (Discontinue=0 or Discontinue is null))";
                            //}
                        }
                        else
                        {
                            query = "select staff_name,s.staff_code,h.dept_name,d.desig_name,(select TextVal from TextValTable where sa.Title=CAST(TextCode as varchar)) as Title,sa.NameAcr,sa.staff_type,category_name,per_address,comm_address,Convert(varchar(10),date_of_birth,103) as date_of_birth,Convert(varchar(10),join_date,103) as join_date,com_mobileno,email,sex,martial_status,sa.appl_no,sa.appl_id,sa.dept_name,sa.desig_name,staff_type,Convert(varchar(10),dateofapply,103) as dateofapply,Convert(varchar(10),exp_joindate,103) as exp_joindate,sex,(select TextVal from TextValTable where sa.Caste=CAST(TextCode as varchar)) as Caste,(select TextVal from TextValTable where sa.religion=CAST(TextCode as varchar)) as religion,(select TextVal from TextValTable where sa.Community=CAST(TextCode as varchar)) as Community,martial_status,email,com_mobileno,(select TextVal from TextValTable where sa.Nationality=CAST(TextCode as varchar)) as Nationality,per_address,comm_address,qualification,(select TextVal from TextValTable where sa.subjects=CAST(TextCode as varchar)) as subjects,yofexp,(select TextVal from TextValTable where sa.bldgrp=CAST(TextCode as varchar)) as bldgrp,sa.adharcardno,CASE WHEN AICTE_Grants = '1' THEN 'Yes' ELSE 'No' END AICTE_Grants,CASE WHEN AICTE_Comm = '1' THEN 'Yes' ELSE 'No' END AICTE_Comm,CASE WHEN Minority = '1' THEN 'Yes' ELSE 'No' END IsMin,'' as IsFirstYr,'' as IsFYCommon,'' as FYCommonSub,CASE WHEN IsPhy = '1' THEN 'Yes' ELSE 'No' END IsPhy,s.PANGIRNumber,CASE WHEN StfNature = 'full' THEN 'Full Time' ELSE 'Part Time' END StfNature,StfStatus,FacultyType,PayType,Programme,CASE WHEN PayMode = 0 THEN 'Cash' WHEN PayMode = 1 THEN 'Cheque' ELSE 'Credit to Bank Account' END PayMode,s.PFNumber,s.BankAccount,(select TextVal from TextValTable where s.Bank_Name=CAST(TextCode as Varchar)) as Bank_Name,(select TextVal from TextValTable where s.Branch_Name=CAST(TextCode as varchar)) as Branch_Name,s.IFSC_Code,CASE WHEN IsPhy = '1' THEN 'Yes' ELSE 'No' END IsPhy,CASE WHEN Minority = '1' THEN 'Yes' ELSE 'No' END IsMin,'' as IsFirstYr,'' as IsFYCommon,'' as FYCommonSub,CASE WHEN AICTE_Comm = 1 THEN 'Yes' ELSE 'No' END AICTE_Comm,CASE WHEN AICTE_Grants = '1' THEN 'Yes' ELSE 'No' END AICTE_Grants,Per_MobileNo from staffmaster s,stafftrans t,staff_appl_master sa,hrdept_master h,desig_master d,staffcategorizer c where s.staff_code =t.staff_code and s.appl_no =sa.appl_no and h.dept_code =t.dept_code and t.desig_code =d.desig_code and c.category_code =t.category_code and s.college_code=h.college_code and s.college_code=d.collegeCode and s.college_code=c.college_code  and t.latestrec =1 and h.dept_code ='" + dpt + "' and s.college_Code='" + collcode + "'";  //and ((resign =0 and settled =0) and (Discontinue=0 or Discontinue is null))
                            //if (cbstfstatus.Checked == false || txtstfstatus.Text == "--Select--")
                            //{
                            //    query = query + " and ((resign =0 and settled =0) and (Discontinue=0 or Discontinue is null))";
                            //}
                        }
                        if (cbstfstatus.Checked == false || txtstfstatus.Text == "--Select--")
                        {
                            query = query + " and ((resign =0 and settled =0) and (Discontinue=0 or Discontinue is null))";
                        }
                        if (cbstfstatus.Checked)
                        {
                            if (chklststfstatus.Items[0].Selected == true && chklststfstatus.Items[1].Selected == true)
                                query += " and ((isnull(s.resign,'0')=1 and ISNULL(s.settled,'0')=1) or (isnull(s.Discontinue,'0')=1))";
                            else if (chklststfstatus.Items[0].Selected == true)
                                query += " and (isnull(s.resign,'0')=1 and ISNULL(s.settled,'0')=1) ";
                            else if (chklststfstatus.Items[1].Selected == true)
                                query += " and (isnull(s.Discontinue,'0')=1)";
                        }
                        ds = d2.select_method_wo_parameter(query, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Fpspread2.SaveChanges();
                            Fpspread2.CommandBar.Visible = false;
                            Fpspread2.Sheets[0].AutoPostBack = false;
                            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            darkstyle.Font.Name = "Book Antiqua";
                            darkstyle.Font.Size = FontUnit.Medium;
                            darkstyle.Font.Bold = true;
                            darkstyle.HorizontalAlign = HorizontalAlign.Center;
                            darkstyle.VerticalAlign = VerticalAlign.Middle;
                            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                            Fpspread2.RowHeader.Visible = false;
                            FarPoint.Web.Spread.StyleInfo darkstyle1 = new FarPoint.Web.Spread.StyleInfo();
                            darkstyle1.Font.Name = "Book Antiqua";
                            darkstyle1.Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].DefaultStyle = darkstyle1;
                            Fpspread2.Sheets[0].RowCount = 0;
                            Fpspread2.Sheets[0].ColumnCount = 2;
                            Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
                            Fpspread2.Sheets[0].ColumnHeader.Columns[0].Width = 50;
                            Fpspread2.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].Columns[0].Locked = true;
                            Fpspread2.Columns[0].Width = 75;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Columns[1].Width = 75;
                            int cc = 1;
                            int j = 0;
                            for (j = 0; j < lb_column1.Items.Count; j++)
                            {
                                cc++;
                                Fpspread2.Sheets[0].ColumnCount = cc + 1;
                                Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Text = lb_column1.Items[j].Text;
                                Fpspread2.Sheets[0].Columns[cc].Locked = true;
                                if (cc == 2 || cc == 4 || cc == 6 || cc == 12 || cc == 16 || cc == 14 || cc == 15 || cc == 9 || cc == 19 || cc == 20 || cc == 27 || cc == 28 || cc == 30 || cc == 32 || cc == 33 || cc == 35 || cc == 39 || cc == 40 || cc == 41 || cc == 25 || cc == 36 || cc == 45 || cc == 46)
                                {
                                    Fpspread2.Columns[cc].Width = 150;
                                }
                                else if (cc == 3 || cc == 17 || cc == 10 || cc == 11 || cc == 26 || cc == 29 || cc == 31)
                                {
                                    Fpspread2.Columns[cc].Width = 100;
                                }
                                else
                                {
                                    Fpspread2.Columns[cc].Width = 250;
                                }
                            }
                            FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                            cb.AutoPostBack = true;
                            FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();
                            Fpspread2.Sheets[0].RowCount++;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = "";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].CellType = cb;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Value = 0;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            string applid = "";
                            string graduation = "";
                            string degree = "";
                            string specialization = "";
                            string qual = "";
                            string year = "";
                            DataSet dsgetstaff = new DataSet();
                            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                qual = "";
                                applid = Convert.ToString(ds.Tables[0].Rows[i]["appl_id"]);
                                Fpspread2.Sheets[0].RowCount++;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].CellType = cb1;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                cc = 1;
                                j = 0;
                                for (j = 0; j < lb_column1.Items.Count; j++)
                                {
                                    cc++;
                                    if (lb_column1.Items[j].Value == "qualification")
                                    {
                                        string getstfdet = "select Graduation,Degree,Specialization,PassYear from StaffDetails where Appl_ID='" + applid + "'";
                                        dsgetstaff.Clear();
                                        dsgetstaff = d2.select_method_wo_parameter(getstfdet, "Text");
                                        if (dsgetstaff.Tables.Count > 0 && dsgetstaff.Tables[0].Rows.Count > 0)
                                        {
                                            graduation = getqval(Convert.ToString(dsgetstaff.Tables[0].Rows[0]["Graduation"]));
                                            degree = getqval(Convert.ToString(dsgetstaff.Tables[0].Rows[0]["Degree"]));
                                            specialization = getqval(Convert.ToString(dsgetstaff.Tables[0].Rows[0]["Specialization"]));
                                            year = Convert.ToString(dsgetstaff.Tables[0].Rows[0]["PassYear"]);
                                        }
                                        if (graduation == "0")
                                        {
                                            graduation = "";
                                        }
                                        if (degree == "0")
                                        {
                                            degree = "";
                                        }
                                        if (specialization == "0")
                                        {
                                            specialization = "";
                                        }
                                        if (graduation.Trim() != "")
                                        {
                                            if (qual == "")
                                            {
                                                qual = graduation;
                                            }
                                            else
                                            {
                                                qual = qual + " , " + graduation;
                                            }
                                        }
                                        if (degree.Trim() != "")
                                        {
                                            if (qual == "")
                                            {
                                                qual = degree;
                                            }
                                            else
                                            {
                                                qual = qual + " , " + degree;
                                            }
                                        }
                                        if (specialization.Trim() != "")
                                        {
                                            if (qual == "")
                                            {
                                                qual = specialization;
                                            }
                                            else
                                            {
                                                qual = qual + " , " + specialization;
                                            }
                                        }
                                        if (year.Trim() != "")
                                        {
                                            if (qual == "")
                                            {
                                                qual = year;
                                            }
                                            else
                                            {
                                                qual = qual + " , " + year;
                                            }
                                        }
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Text = qual;
                                    }
                                    else
                                    {
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].Text = Convert.ToString(ds.Tables[0].Rows[i][lb_column1.Items[j].Value]);
                                    }
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, cc].HorizontalAlign = HorizontalAlign.Left;
                                }
                            }
                            imgbtn_columsetting.Visible = true;
                            Fpspread2.Visible = true;
                            Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                            Fpspread2.SaveChanges();
                            rptprint.Visible = true;
                            lblspread1_err.Visible = false;
                        }
                        else
                        {
                            Fpspread2.Visible = false;
                            Fpspread2.SaveChanges();
                            lblspread1_err.Visible = true;
                            lblspread1_err.Text = "No Staff Found!";
                        }
                    }
                    else
                    {
                        Fpspread2.Visible = false;
                        Fpspread2.SaveChanges();
                        lblspread1_err.Visible = true;
                        lblspread1_err.Text = "Please select the Columns!";
                    }
                }
                else
                {
                    Fpspread2.Visible = false;
                    Fpspread2.SaveChanges();
                    lblspread1_err.Visible = true;
                    lblspread1_err.Text = "Please select the Staff from Appropriate Designation!";
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collcode, "Staff_StrengthMaster.aspx");
        }
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread2, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        {
        }
    }
    protected void btnplusrpt_click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = true;
        txtaddrptname.Text = "";
    }
    protected void btnminusrpt_click(object sender, EventArgs e)
    {
        try
        {
            if (ddlrptname.SelectedItem.Text.Trim() == "Select")
            {
                popalert.Visible = true;
                lblalrt.Visible = true;
                lblalrt.Text = "No Value Selected!";
            }
            else
            {
                string delq = "delete from New_InsSettings where user_code='" + usercode + "' and LinkName='StrengthReportName' and LinkValue like '" + Convert.ToString(ddlrptname.SelectedItem.Text) + "%' and College_Code='" + collegecode1 + "'";
                int delcount = d2.update_method_wo_parameter(delq, "Text");
                if (delcount > 0)
                {
                    popalert.Visible = true;
                    lblalrt.Visible = true;
                    lblalrt.Text = "Deleted Successfully!";
                    alertpopwindow.Visible = false;
                    bindreportname();
                }
            }
        }
        catch { }
    }
    protected void btnsaverpt_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtaddrptname.Text.Trim() != "")
            {
                string rptname = Convert.ToString(txtaddrptname.Text);
                StringBuilder sb = new StringBuilder();
                string MyStr = String.Empty;
                if (lb_column1.Items.Count > 0)
                {
                    for (int jk = 0; jk < lb_column1.Items.Count; jk++)
                    {
                        if (sb.Length == 0)
                            sb.Append(Convert.ToString(lb_column1.Items[jk].Text) + ";" + Convert.ToString(lb_column1.Items[jk].Value));
                        else
                            sb.Append("," + Convert.ToString(lb_column1.Items[jk].Text) + ";" + Convert.ToString(lb_column1.Items[jk].Value));
                    }
                    MyStr = rptname + "-" + sb;
                    string insq = "if exists (select * from New_InsSettings where user_code='" + usercode + "' and LinkName='StrengthReportName' and LinkValue='" + MyStr + "' and college_code='" + collegecode1 + "') update New_InsSettings set LinkValue='" + MyStr + "' where user_code='" + usercode + "' and LinkName='StrengthReportName' and LinkValue='" + MyStr + "' and college_code='" + collegecode1 + "' else insert into New_InsSettings (user_code,LinkName,LinkValue,College_code) values ('" + usercode + "','StrengthReportName','" + MyStr + "','" + collegecode1 + "')";
                    int inscount = d2.update_method_wo_parameter(insq, "Text");
                    if (inscount > 0)
                    {
                        popalert.Visible = true;
                        lblalrt.Visible = true;
                        lblalrt.Text = "Saved Successfully!";
                        alertpopwindow.Visible = false;
                        bindreportname();
                    }
                }
                else
                {
                    popalert.Visible = true;
                    lblalrt.Visible = true;
                    alertpopwindow.Visible = false;
                    lblalrt.Text = "Please Select Columns From Column Order!";
                }
            }
            else
            {
                popalert.Visible = true;
                lblalrt.Visible = true;
                lblalrt.Text = "Please Enter the ReportName!";
            }
        }
        catch { }
    }
    protected void ddlrptname_Change(object sender, EventArgs e)
    {
        try
        {
            string MyColOrder = "";
            lb_column1.Items.Clear();
            if (ddlrptname.SelectedItem.Text.Trim() != "Select")
            {
                MyColOrder = d2.GetFunction("select LinkValue from New_InsSettings where user_code='" + usercode + "' and LinkName='StrengthReportName' and LinkValue like '" + Convert.ToString(ddlrptname.SelectedItem.Text) + "%' and college_code='" + collegecode1 + "'");
                if (!String.IsNullOrEmpty(MyColOrder))
                {
                    string[] splVal = MyColOrder.Split('-');
                    if (splVal.Length == 2)
                    {
                        if (!String.IsNullOrEmpty(splVal[1]))
                        {
                            string[] splColOrd = splVal[1].Split(',');
                            if (splColOrd.Length > 0)
                            {
                                for (int sp = 0; sp < splColOrd.Length; sp++)
                                {
                                    string[] splMyVal = splColOrd[sp].Split(';');
                                    if (splMyVal.Length == 2)
                                    {
                                        lb_selectcolumn.Items.Remove(new ListItem(Convert.ToString(splMyVal[0]), Convert.ToString(splMyVal[1])));
                                        lb_column1.Items.Add(new ListItem(Convert.ToString(splMyVal[0]), Convert.ToString(splMyVal[1])));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch { }
    }
    protected void btnexitrpt_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }
    protected void btnerrpopclose_Click(object sender, EventArgs e)
    {
        popalert.Visible = false;
    }
    private void bindreportname()
    {
        try
        {
            string Val = "";
            string selq = "select LinkValue from New_InsSettings where LinkName='StrengthReportName' and user_code='" + usercode + "' and college_code='" + collegecode1 + "'";
            ds.Clear();
            ddlrptname.Items.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int col = 0; col < ds.Tables[0].Rows.Count; col++)
                {
                    Val = Convert.ToString(ds.Tables[0].Rows[col]["LinkValue"]).Split('-')[0];
                    ddlrptname.Items.Add(new ListItem(Val, Convert.ToString(col++)));
                }
                ddlrptname.Items.Insert(0, "Select");
            }
            else
            {
                ddlrptname.Items.Insert(0, "Select");
            }
        }
        catch { }
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "";
            if (ddlrptname.SelectedItem.Text.Trim() == "Select")
            {
                degreedetails = "Staff Strength Report";
            }
            else
            {
                degreedetails = Convert.ToString(ddlrptname.SelectedItem.Text);
            }
            string pagename = "Staff_StrengthMaster.aspx";
            Printcontrol.loadspreaddetails(Fpspread2, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {
        }
    }
    protected void btn_pdf_OnClick(object sender, EventArgs e)
    {
        string collcode = Convert.ToString(ddlcollege.SelectedItem.Value);
        try
        {
            lblspread1_err.Visible = false;
            Font Fontco12 = new Font("Comic Sans MS", 12, FontStyle.Bold);
            Font Fontpala12 = new Font("Palatino Linotype", 10, FontStyle.Bold);
            Font Fontco10 = new Font("Comic Sans MS", 10, FontStyle.Regular);
            Font Fontco12a = new Font("Comic Sans MS", 12, FontStyle.Bold);
            Font Fontarial7 = new Font("Arial", 7, FontStyle.Regular);
            Font Fontarial7r = new Font("Arial", 6, FontStyle.Bold);
            Font Fontarial9 = new Font("Arial", 8, FontStyle.Bold);
            Font Fontarial10 = new Font("Arial", 10, FontStyle.Regular);
            Font Fontarial12 = new Font("Arial", 12, FontStyle.Regular);
            DataSet gradeds = new DataSet();
            Font fontcal11 = new Font("Calibri (Body)", 14, FontStyle.Bold);
            Font fontcal14 = new Font("Calibri (Body)", 35, FontStyle.Bold);
            Font fontcal8 = new Font("Calibri (Body)", 8, FontStyle.Bold);
            Font fontcal81 = new Font("Arial", 8, FontStyle.Underline);
            Gios.Pdf.PdfDocument Siva = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage mypdfpage;
            string selectquery = "";
            string sname = "";
            string std = "";
            string scode = "";
            string dept = "";
            string desig = "";
            string caddress = "";
            string paddress = "";
            string phone = "";
            string dob = "";
            string doj = "";
            string email = "";
            string gender = "";
            string maritalstatus = "";
            int cc = 0;
            if (checkedOK())
            {
                for (int K = 1; K < Fpspread2.Sheets[0].Rows.Count; K++)
                {
                    string check = Convert.ToString(Fpspread2.Sheets[0].Cells[K, 1].Value);
                    if (check == "1")
                    {
                        if (lst_setting2.Items.Count > 0)
                        {
                            sname = Convert.ToString(Fpspread2.Sheets[0].Cells[K, 3].Text);
                            scode = Convert.ToString(Fpspread2.Sheets[0].Cells[K, 2].Text);
                            string appl_no = d2.GetFunction("select appl_id from staff_appl_master sa,staffmaster sm where sa.appl_no=sm.appl_no and sm.college_code='" + collcode + "' and sm.staff_code='" + scode + "'");
                            //std = "History Of" + ' ' + scode;
                            std = "Staff Information Report";
                            mypdfpage = Siva.NewPage();
                            string query = "select collname,address1,address2,address3  from collinfo where college_code='" + collcode + "'";
                            ds = d2.select_method_wo_parameter(query, "Text");
                            string collage = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                            string add1 = Convert.ToString(ds.Tables[0].Rows[0]["address1"]);
                            string add2 = Convert.ToString(ds.Tables[0].Rows[0]["address2"]);
                            string add3 = Convert.ToString(ds.Tables[0].Rows[0]["address3"]);
                            string imgfilename = "left_logo";
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/left_logo.jpeg")))
                            {
                                PdfImage LogoImage = Siva.NewImage(HttpContext.Current.Server.MapPath("~/college/left_logo.jpeg"));
                                mypdfpage.Add(LogoImage, 25, 25, 400);
                            }
                            if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/left_logo.jpeg")))
                            {
                                MemoryStream memoryStream = new MemoryStream();
                                string sellogo = "select logo1,logo2 from collinfo where college_code='" + collcode + "'";
                                ds.Clear();
                                ds = d2.select_method_wo_parameter(sellogo, "Text");
                                if (ds.Tables.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        byte[] file = (byte[])ds.Tables[0].Rows[0]["logo1"];
                                        memoryStream.Write(file, 0, file.Length);
                                        if (file.Length > 0)
                                        {
                                            System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                            System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                            thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + imgfilename + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                        }
                                        memoryStream.Dispose();
                                        memoryStream.Close();
                                    }
                                }
                            }
                            string imgright = "right_logo";
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                            {
                                PdfImage LogoImage = Siva.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                mypdfpage.Add(LogoImage, 480, 25, 350);
                            }
                            if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                            {
                                string rightlogo = "right_logo";
                                MemoryStream memoryStream = new MemoryStream();
                                string sellogo = "select logo1,logo2 from collinfo where college_code='" + collcode + "'";
                                ds.Clear();
                                ds = d2.select_method_wo_parameter(sellogo, "Text");
                                if (ds.Tables.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        byte[] file = (byte[])ds.Tables[0].Rows[0]["logo2"];
                                        memoryStream.Write(file, 0, file.Length);
                                        if (file.Length > 0)
                                        {
                                            System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                            System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                            thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + rightlogo + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                        }
                                        memoryStream.Dispose();
                                        memoryStream.Close();
                                    }
                                }
                                PdfImage LogoImage = Siva.NewImage(HttpContext.Current.Server.MapPath("~/college/" + rightlogo + ".jpeg"));
                                mypdfpage.Add(LogoImage, 480, 25, 350);
                            }
                            string address = add1 + ' ' + add2 + ' ' + add3;
                            PdfTextArea pdftext = new PdfTextArea(fontcal11, System.Drawing.Color.Black, new PdfArea(Siva, 38, 50, 500, 50), System.Drawing.ContentAlignment.MiddleCenter, collage);
                            mypdfpage.Add(pdftext);
                            PdfTextArea pdftext1 = new PdfTextArea(fontcal8, System.Drawing.Color.Black, new PdfArea(Siva, 38, 70, 500, 50), System.Drawing.ContentAlignment.MiddleCenter, address);
                            mypdfpage.Add(pdftext1);
                            PdfTextArea pdftext2 = new PdfTextArea(fontcal8, System.Drawing.Color.Black, new PdfArea(Siva, 38, 90, 500, 50), System.Drawing.ContentAlignment.MiddleCenter, std);
                            mypdfpage.Add(pdftext2);
                            string pdfquery = "select staff_name,s.appl_no,s.staff_code,dept_name,desig_name,sa.staff_type,per_address,comm_address,Convert(varchar(10),date_of_birth,103) as date_of_birth,join_date,com_mobileno,email,sex,martial_status,Caste,religion,Community,Nationality,qualification,subjects,yofexp,bldgrp   from staffmaster s,staff_appl_master sa where  s.appl_no =sa.appl_no  and staff_code='" + scode + "' ";
                            pdfquery = pdfquery + " select Appl_ID,DetailType,Graduation,Degree,Specialization,PassYear,University,Institution,Percentage,Grade,Class,Convert(varchar(10),ExpFromDate,103) as ExpFromDate,Convert(varchar(10),ExpToDate,103) as ExpToDate,ExpYear,ExpMOnth,ExpOrganization,ExpDesig,ExpIn,CurrentSalary from StaffDetails where Appl_ID='" + appl_no + "'";
                            pdfquery = pdfquery + " select TextVal,TextCode from TextValTable where college_code='" + collcode + "'";
                            ds = d2.select_method_wo_parameter(pdfquery, "Text");
                            bool eduDet = false;
                            bool expdet = false;
                            string col1 = "";
                            string col2 = "";
                            for (int s = 0; s < lst_setting2.Items.Count; s++)
                            {
                                cc++;
                                string printvalue = "";
                                string header = Convert.ToString(lst_setting2.Items[s].Text);
                                string value = Convert.ToString(lst_setting2.Items[s].Value);
                                if (value.Trim().ToLower() == "qualification")
                                {
                                    eduDet = true;
                                }
                                if (value.Trim().ToLower() == "experience_info")
                                {
                                    expdet = true;
                                }
                                if (value.Trim() == "bldgrp")
                                {
                                    if ((s % 2) == 0)
                                    {
                                        string getbgroup = d2.GetFunction("select TextVal from TextValTable where TextCriteria='bgrou' and TextCode='" + Convert.ToString(ds.Tables[0].Rows[0][value]) + "'");
                                        if (getbgroup.Trim() == "" || getbgroup.Trim() == "0")
                                        {
                                            getbgroup = "";
                                        }
                                        col1 += "\n\n" + header + " : " + getbgroup;
                                    }
                                    else
                                    {
                                        string getbgroup = d2.GetFunction("select TextVal from TextValTable where TextCriteria='bgrou' and TextCode='" + Convert.ToString(ds.Tables[0].Rows[0][value]) + "'");
                                        if (getbgroup.Trim() == "" || getbgroup.Trim() == "0")
                                        {
                                            getbgroup = "";
                                        }
                                        col2 += "\n\n" + header + " : " + getbgroup;
                                    }
                                }
                                if (value.Trim() == "Caste")
                                {
                                    if ((s % 2) == 0)
                                    {
                                        string getbgroup = d2.GetFunction("select TextVal from TextValTable where TextCriteria='caste' and TextCode='" + Convert.ToString(ds.Tables[0].Rows[0][value]) + "'");
                                        if (getbgroup.Trim() == "" || getbgroup.Trim() == "0")
                                        {
                                            getbgroup = "";
                                        }
                                        col1 += "\n\n" + header + " : " + getbgroup;
                                    }
                                    else
                                    {
                                        string getbgroup = d2.GetFunction("select TextVal from TextValTable where TextCriteria='caste' and TextCode='" + Convert.ToString(ds.Tables[0].Rows[0][value]) + "'");
                                        if (getbgroup.Trim() == "" || getbgroup.Trim() == "0")
                                        {
                                            getbgroup = "";
                                        }
                                        col2 += "\n\n" + header + " : " + getbgroup;
                                    }
                                }
                                if (value.Trim() == "Nationality")
                                {
                                    if ((s % 2) == 0)
                                    {
                                        string getbgroup = d2.GetFunction("select TextVal from TextValTable where TextCriteria='natio' and TextCode='" + Convert.ToString(ds.Tables[0].Rows[0][value]) + "'");
                                        if (getbgroup.Trim() == "" || getbgroup.Trim() == "0")
                                        {
                                            getbgroup = "";
                                        }
                                        col1 += "\n\n" + header + " : " + getbgroup;
                                    }
                                    else
                                    {
                                        string getbgroup = d2.GetFunction("select TextVal from TextValTable where TextCriteria='natio' and TextCode='" + Convert.ToString(ds.Tables[0].Rows[0][value]) + "'");
                                        if (getbgroup.Trim() == "" || getbgroup.Trim() == "0")
                                        {
                                            getbgroup = "";
                                        }
                                        col2 += "\n\n" + header + " : " + getbgroup;
                                    }
                                }
                                if (value.Trim() == "religion")
                                {
                                    if ((s % 2) == 0)
                                    {
                                        string getbgroup = d2.GetFunction("select TextVal from TextValTable where TextCriteria='relig' and TextCode='" + Convert.ToString(ds.Tables[0].Rows[0][value]) + "'");
                                        if (getbgroup.Trim() == "" || getbgroup.Trim() == "0")
                                        {
                                            getbgroup = "";
                                        }
                                        col1 += "\n\n" + header + " : " + getbgroup;
                                    }
                                    else
                                    {
                                        string getbgroup = d2.GetFunction("select TextVal from TextValTable where TextCriteria='relig' and TextCode='" + Convert.ToString(ds.Tables[0].Rows[0][value]) + "'");
                                        if (getbgroup.Trim() == "" || getbgroup.Trim() == "0")
                                        {
                                            getbgroup = "";
                                        }
                                        col2 += "\n\n" + header + " : " + getbgroup;
                                    }
                                }
                                if (value.Trim() == "Community")
                                {
                                    if ((s % 2) == 0)
                                    {
                                        string getbgroup = d2.GetFunction("select TextVal from TextValTable where TextCriteria='comm' and TextCode='" + Convert.ToString(ds.Tables[0].Rows[0][value]) + "'");
                                        if (getbgroup.Trim() == "" || getbgroup.Trim() == "0")
                                        {
                                            getbgroup = "";
                                        }
                                        col1 += "\n\n" + header + " : " + getbgroup;
                                    }
                                    else
                                    {
                                        string getbgroup = d2.GetFunction("select TextVal from TextValTable where TextCriteria='comm' and TextCode='" + Convert.ToString(ds.Tables[0].Rows[0][value]) + "'");
                                        if (getbgroup.Trim() == "" || getbgroup.Trim() == "0")
                                        {
                                            getbgroup = "";
                                        }
                                        col2 += "\n\n" + header + " : " + getbgroup;
                                    }
                                }
                                if (value.Trim() == "subjects")
                                {
                                    if ((s % 2) == 0)
                                    {
                                        string getbgroup = d2.GetFunction("select TextVal from TextValTable where TextCriteria='fsub' and TextCode='" + Convert.ToString(ds.Tables[0].Rows[0][value]) + "'");
                                        if (getbgroup.Trim() == "" || getbgroup.Trim() == "0")
                                        {
                                            getbgroup = "";
                                        }
                                        col1 += "\n\n" + header + " : " + getbgroup;
                                    }
                                    else
                                    {
                                        string getbgroup = d2.GetFunction("select TextVal from TextValTable where TextCriteria='fsub' and TextCode='" + Convert.ToString(ds.Tables[0].Rows[0][value]) + "'");
                                        if (getbgroup.Trim() == "" || getbgroup.Trim() == "0")
                                        {
                                            getbgroup = "";
                                        }
                                        col2 += "\n\n" + header + " : " + getbgroup;
                                    }
                                }
                                if (value == "staff_code" || value == "appl_no" || value == "staff_type" || value == "per_address" || value == "date_of_birth" || value == "com_mobileno" || value == "sex" || value == "yofexp" || value == "staff_name" || value == "dept_name" || value == "desig_name" || value == "comm_address" || value == "join_date" || value == "email" || value == "martial_status" || value == "yofexp")
                                {
                                    if ((s % 2) == 0)
                                    {
                                        printvalue = Convert.ToString(ds.Tables[0].Rows[0][value]);
                                        col1 += "\n\n" + header + " : " + printvalue;
                                    }
                                    else
                                    {
                                        printvalue = Convert.ToString(ds.Tables[0].Rows[0][value]);
                                        col2 += "\n\n" + header + " : " + printvalue;
                                    }
                                }
                            }
                            Gios.Pdf.PdfTable table1marksa = Siva.NewTable(fontcal8, 2, 2, 5);
                            table1marksa.SetBorders(Color.Black, 1, BorderType.None);
                            table1marksa.VisibleHeaders = false;
                            table1marksa.Cell(0, 0).SetContent(col1);
                            table1marksa.Cell(0, 0).SetContentAlignment(ContentAlignment.TopLeft);
                            table1marksa.Cell(0, 1).SetContent(col2);
                            table1marksa.Cell(0, 1).SetContentAlignment(ContentAlignment.TopLeft);
                            Gios.Pdf.PdfTablePage newpdftable1table1marksa = table1marksa.CreateTablePage(new Gios.Pdf.PdfArea(Siva, 35, 160, 500, 300));
                            mypdfpage.Add(newpdftable1table1marksa);
                            DataView dvnew = new DataView();
                            DataView dvnew1 = new DataView();
                            if (eduDet)
                            {
                                ds.Tables[1].DefaultView.RowFilter = " Appl_ID='" + appl_no + "' and DetailType='1'";
                                dvnew = ds.Tables[1].DefaultView;
                                if (dvnew.Count > 0)
                                {
                                    string ed = "Educational Details";
                                    PdfTextArea pdftext40 = new PdfTextArea(Fontpala12, System.Drawing.Color.Black, new PdfArea(Siva, 4, 370, 150, 50), System.Drawing.ContentAlignment.MiddleCenter, ed);
                                    mypdfpage.Add(pdftext40);
                                    Gios.Pdf.PdfTable table1marks = Siva.NewTable(Fontarial7, dvnew.Count + 1, 9, 1);
                                    table1marks.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                    table1marks.VisibleHeaders = false;
                                    table1marks.Cell(0, 0).SetContent("S No");
                                    table1marks.Cell(0, 0).SetFont(fontcal8);
                                    table1marks.Cell(0, 0).SetContentAlignment(ContentAlignment.TopCenter);
                                    table1marks.Cell(0, 1).SetContent("Graduation");
                                    table1marks.Cell(0, 1).SetFont(fontcal8);
                                    table1marks.Cell(0, 1).SetContentAlignment(ContentAlignment.TopCenter);
                                    table1marks.Cell(0, 2).SetContent("Degree");
                                    table1marks.Cell(0, 2).SetFont(fontcal8);
                                    table1marks.Cell(0, 2).SetContentAlignment(ContentAlignment.TopCenter);
                                    table1marks.Cell(0, 3).SetContent("Year Of Passing");
                                    table1marks.Cell(0, 3).SetFont(fontcal8);
                                    table1marks.Cell(0, 3).SetContentAlignment(ContentAlignment.TopCenter);
                                    table1marks.Cell(0, 4).SetContent("Percentage");
                                    table1marks.Cell(0, 4).SetFont(fontcal8);
                                    table1marks.Cell(0, 4).SetContentAlignment(ContentAlignment.TopCenter);
                                    table1marks.Cell(0, 5).SetContent("Points");
                                    table1marks.Cell(0, 5).SetFont(fontcal8);
                                    table1marks.Cell(0, 5).SetContentAlignment(ContentAlignment.TopCenter);
                                    table1marks.Cell(0, 6).SetContent("Specialization");
                                    table1marks.Cell(0, 6).SetFont(fontcal8);
                                    table1marks.Cell(0, 6).SetContentAlignment(ContentAlignment.TopCenter);
                                    table1marks.Cell(0, 7).SetContent("Institution");
                                    table1marks.Cell(0, 7).SetFont(fontcal8);
                                    table1marks.Cell(0, 7).SetContentAlignment(ContentAlignment.TopCenter);
                                    table1marks.Cell(0, 8).SetContent("University");
                                    table1marks.Cell(0, 8).SetFont(fontcal8);
                                    table1marks.Cell(0, 8).SetContentAlignment(ContentAlignment.TopCenter);
                                    for (i = 1; i <= dvnew.Count; i++)
                                    {
                                        table1marks.Cell(i, 0).SetContent(i);
                                        table1marks.Cell(i, 0).SetContentAlignment(ContentAlignment.TopCenter);
                                        ds.Tables[2].DefaultView.RowFilter = " TextCode='" + Convert.ToString(dvnew[i - 1]["Graduation"]) + "'";
                                        dvnew1 = ds.Tables[2].DefaultView;
                                        if (dvnew1.Count > 0)
                                        {
                                            table1marks.Cell(i, 1).SetContent(Convert.ToString(dvnew1[0]["TextVal"]));
                                        }
                                        else
                                        {
                                            table1marks.Cell(i, 1).SetContent("-");
                                        }
                                        table1marks.Cell(i, 1).SetContentAlignment(ContentAlignment.TopCenter);
                                        ds.Tables[2].DefaultView.RowFilter = " TextCode='" + Convert.ToString(dvnew[i - 1]["Degree"]) + "'";
                                        dvnew1 = ds.Tables[2].DefaultView;
                                        if (dvnew1.Count > 0)
                                        {
                                            table1marks.Cell(i, 2).SetContent(Convert.ToString(dvnew1[0]["TextVal"]));
                                        }
                                        else
                                        {
                                            table1marks.Cell(i, 2).SetContent("-");
                                        }
                                        table1marks.Cell(i, 2).SetContentAlignment(ContentAlignment.TopCenter);
                                        table1marks.Cell(i, 3).SetContent(Convert.ToString(dvnew[i - 1]["PassYear"]));
                                        table1marks.Cell(i, 3).SetContentAlignment(ContentAlignment.TopCenter);
                                        table1marks.Cell(i, 4).SetContent(Convert.ToString(dvnew[i - 1]["Percentage"]));
                                        table1marks.Cell(i, 4).SetContentAlignment(ContentAlignment.TopCenter);
                                        table1marks.Cell(i, 5).SetContent(Convert.ToString(dvnew[i - 1]["Grade"]));
                                        table1marks.Cell(i, 5).SetContentAlignment(ContentAlignment.TopCenter);
                                        ds.Tables[2].DefaultView.RowFilter = " TextCode='" + Convert.ToString(dvnew[i - 1]["Specialization"]) + "'";
                                        dvnew1 = ds.Tables[2].DefaultView;
                                        if (dvnew1.Count > 0)
                                        {
                                            table1marks.Cell(i, 6).SetContent(Convert.ToString(dvnew1[0]["TextVal"]));
                                        }
                                        else
                                        {
                                            table1marks.Cell(i, 6).SetContent("-");
                                        }
                                        table1marks.Cell(i, 6).SetContentAlignment(ContentAlignment.TopCenter);
                                        ds.Tables[2].DefaultView.RowFilter = " TextCode='" + Convert.ToString(dvnew[i - 1]["Institution"]) + "'";
                                        dvnew1 = ds.Tables[2].DefaultView;
                                        if (dvnew1.Count > 0)
                                        {
                                            table1marks.Cell(i, 7).SetContent(Convert.ToString(dvnew1[0]["TextVal"]));
                                        }
                                        else
                                        {
                                            table1marks.Cell(i, 7).SetContent("-");
                                        }
                                        table1marks.Cell(i, 7).SetContentAlignment(ContentAlignment.TopCenter);
                                        ds.Tables[2].DefaultView.RowFilter = " TextCode='" + Convert.ToString(dvnew[i - 1]["University"]) + "'";
                                        dvnew1 = ds.Tables[2].DefaultView;
                                        if (dvnew1.Count > 0)
                                        {
                                            table1marks.Cell(i, 8).SetContent(Convert.ToString(dvnew1[0]["TextVal"]));
                                        }
                                        else
                                        {
                                            table1marks.Cell(i, 8).SetContent("-");
                                        }
                                        table1marks.Cell(i, 8).SetContentAlignment(ContentAlignment.TopCenter);
                                    }
                                    Gios.Pdf.PdfTablePage newpdftable1table1marks = table1marks.CreateTablePage(new Gios.Pdf.PdfArea(Siva, 35, 410, 500, 100));
                                    mypdfpage.Add(newpdftable1table1marks);
                                }
                            }
                            if (expdet)
                            {
                                ds.Tables[1].DefaultView.RowFilter = " Appl_ID='" + appl_no + "' and DetailType='2'";
                                dvnew = ds.Tables[1].DefaultView;
                                if (dvnew.Count > 0)
                                {
                                    string exp = "Experience Details";
                                    PdfTextArea pdftext41 = new PdfTextArea(Fontpala12, System.Drawing.Color.Black, new PdfArea(Siva, 4, 500, 150, 50), System.Drawing.ContentAlignment.MiddleCenter, exp);
                                    mypdfpage.Add(pdftext41);
                                    Gios.Pdf.PdfTable table1marks2 = Siva.NewTable(Fontarial7, dvnew.Count + 1, 9, 1);
                                    table1marks2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                    table1marks2.VisibleHeaders = false;
                                    table1marks2.Cell(0, 0).SetContent("S No");
                                    table1marks2.Cell(0, 0).SetFont(fontcal8);
                                    table1marks2.Cell(0, 0).SetContentAlignment(ContentAlignment.TopCenter);
                                    table1marks2.Cell(0, 1).SetContent("Experience In");
                                    table1marks2.Cell(0, 1).SetFont(fontcal8);
                                    table1marks2.Cell(0, 1).SetContentAlignment(ContentAlignment.TopCenter);
                                    table1marks2.Cell(0, 2).SetContent("From");
                                    table1marks2.Cell(0, 2).SetFont(fontcal8);
                                    table1marks2.Cell(0, 2).SetContentAlignment(ContentAlignment.TopCenter);
                                    table1marks2.Cell(0, 3).SetContent("To");
                                    table1marks2.Cell(0, 3).SetFont(fontcal8);
                                    table1marks2.Cell(0, 3).SetContentAlignment(ContentAlignment.TopCenter);
                                    table1marks2.Cell(0, 4).SetContent("Post Held");
                                    table1marks2.Cell(0, 4).SetFont(fontcal8);
                                    table1marks2.Cell(0, 4).SetContentAlignment(ContentAlignment.TopCenter);
                                    table1marks2.Cell(0, 5).SetContent("Designation Type");
                                    table1marks2.Cell(0, 5).SetFont(fontcal8);
                                    table1marks2.Cell(0, 5).SetContentAlignment(ContentAlignment.TopCenter);
                                    table1marks2.Cell(0, 6).SetContent("Year");
                                    table1marks2.Cell(0, 6).SetFont(fontcal8);
                                    table1marks2.Cell(0, 6).SetContentAlignment(ContentAlignment.TopCenter);
                                    table1marks2.Cell(0, 7).SetContent("Month");
                                    table1marks2.Cell(0, 7).SetFont(fontcal8);
                                    table1marks2.Cell(0, 7).SetContentAlignment(ContentAlignment.TopCenter);
                                    table1marks2.Cell(0, 8).SetContent("Monthly Salary");
                                    table1marks2.Cell(0, 8).SetFont(fontcal8);
                                    table1marks2.Cell(0, 8).SetContentAlignment(ContentAlignment.TopCenter);
                                    for (i = 1; i <= dvnew.Count; i++)
                                    {
                                        string collname = "";
                                        table1marks2.Cell(i, 0).SetContent(i);
                                        table1marks2.Cell(i, 0).SetContentAlignment(ContentAlignment.TopCenter);
                                        ds.Tables[2].DefaultView.RowFilter = " TextCode='" + Convert.ToString(dvnew[i - 1]["ExpIn"]) + "'";
                                        dvnew1 = ds.Tables[2].DefaultView;
                                        if (dvnew1.Count > 0)
                                        {
                                            table1marks2.Cell(i, 1).SetContent(Convert.ToString(dvnew1[0]["TextVal"]));
                                        }
                                        else
                                        {
                                            table1marks2.Cell(i, 1).SetContent("-");
                                        }
                                        table1marks2.Cell(i, 1).SetContentAlignment(ContentAlignment.TopCenter);
                                        table1marks2.Cell(i, 2).SetContent(Convert.ToString(dvnew[i - 1]["ExpFromDate"]));
                                        table1marks2.Cell(i, 2).SetContentAlignment(ContentAlignment.TopCenter);
                                        table1marks2.Cell(i, 3).SetContent(Convert.ToString(dvnew[i - 1]["ExpToDate"]));
                                        table1marks2.Cell(i, 3).SetContentAlignment(ContentAlignment.TopCenter);
                                        string getorgan = d2.GetFunction("select collname from collinfo where college_code='" + Convert.ToString(dvnew[i - 1]["ExpOrganization"]) + "'");
                                        if (getorgan.Trim() != "" && getorgan.Trim() != "0")
                                        {
                                            collname = getorgan;
                                        }
                                        else
                                        {
                                            string gettxtval = d2.GetFunction("select TextVal from TextValTable where TextCode='" + Convert.ToString(dvnew[i - 1]["ExpOrganization"]) + "'");
                                            if (gettxtval.Trim() != "" && gettxtval.Trim() != "0")
                                            {
                                                collname = gettxtval;
                                            }
                                        }
                                        table1marks2.Cell(i, 4).SetContent(collname);
                                        table1marks2.Cell(i, 4).SetContentAlignment(ContentAlignment.TopCenter);
                                        string getdesi = d2.GetFunction("select desig_name from desig_master where desig_code='" + Convert.ToString(dvnew[i - 1]["ExpDesig"]) + "' and collegeCode='" + collcode + "'");
                                        if (getdesi.Trim() != "" && getdesi.Trim() != "0")
                                        {
                                            table1marks2.Cell(i, 5).SetContent(getdesi);
                                        }
                                        else
                                        {
                                            table1marks2.Cell(i, 5).SetContent("-");
                                        }
                                        table1marks2.Cell(i, 5).SetContentAlignment(ContentAlignment.TopCenter);
                                        table1marks2.Cell(i, 6).SetContent(Convert.ToString(dvnew[i - 1]["ExpYear"]));
                                        table1marks2.Cell(i, 6).SetContentAlignment(ContentAlignment.TopCenter);
                                        table1marks2.Cell(i, 7).SetContent(Convert.ToString(dvnew[i - 1]["ExpMOnth"]));
                                        table1marks2.Cell(i, 7).SetContentAlignment(ContentAlignment.TopCenter);
                                        table1marks2.Cell(i, 8).SetContent(Convert.ToString(dvnew[i - 1]["CurrentSalary"]));
                                        table1marks2.Cell(i, 8).SetContentAlignment(ContentAlignment.TopCenter);
                                    }
                                    Gios.Pdf.PdfTablePage newpdftable1table1marks = table1marks2.CreateTablePage(new Gios.Pdf.PdfArea(Siva, 35, 540, 500, 100));
                                    mypdfpage.Add(newpdftable1table1marks);
                                }
                            }
                            mypdfpage.SaveToDocument();
                        }
                        else
                        {
                            lblspread1_err.Visible = true;
                            lblspread1_err.Text = "Please Select any one Columns From PDF ColumnOrder!";
                            return;
                        }
                    }
                }
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "marksheet" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                    Siva.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                }
            }
            else
            {
                lblspread1_err.Visible = true;
                lblspread1_err.Text = "Please Select any one staff!";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collcode, "Staff_StrengthMaster.aspx");
        }
    }
    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        poppernew.Visible = false;
        lb_column1.Items.Clear();
    }
    public void imgbtn_all_Click(object sender, EventArgs e)
    {
        poppernew.Visible = true;
        load();
    }
    public void loadright()
    {
        lb_column1.Items.Clear();
        lb_column1.Items.Add(new ListItem("Staff Code", "staff_code"));
        lb_column1.Items.Add(new ListItem("Title", "Title"));
        lb_column1.Items.Add(new ListItem("SurName", "NameAcr"));
        lb_column1.Items.Add(new ListItem("Staff Name", "staff_name"));
        lb_column1.Items.Add(new ListItem("Appl No", "appl_no"));
        lb_column1.Items.Add(new ListItem("Department", "dept_name"));
        lb_column1.Items.Add(new ListItem("Designation", "desig_name"));
        lb_column1.Items.Add(new ListItem("Staff Type", "staff_type"));
        lb_column1.Items.Add(new ListItem("DOB", "date_of_birth"));
        lb_column1.Items.Add(new ListItem("Date Of Join", "exp_joindate"));
        lb_column1.Items.Add(new ListItem("Gender", "sex"));
        lb_column1.Items.Add(new ListItem("Caste", "Caste"));
        lb_column1.Items.Add(new ListItem("Religion", "religion"));
        lb_column1.Items.Add(new ListItem("Community", "Community"));
        lb_column1.Items.Add(new ListItem("Marital Status", "martial_status"));
        lb_column1.Items.Add(new ListItem("Date Of Apply", "dateofapply"));
        lb_column1.Items.Add(new ListItem("Email", "email"));
        lb_column1.Items.Add(new ListItem("Phone No", "Per_MobileNo"));
        lb_column1.Items.Add(new ListItem("Nationality", "Nationality"));
        lb_column1.Items.Add(new ListItem("Permanent Address", "per_address"));
        lb_column1.Items.Add(new ListItem("Communication Address", "comm_address"));
        lb_column1.Items.Add(new ListItem("Qualification", "qualification"));
        lb_column1.Items.Add(new ListItem("Familiar Subjects", "subjects"));
        lb_column1.Items.Add(new ListItem("Experience", "yofexp"));
        lb_column1.Items.Add(new ListItem("Blood Group", "bldgrp"));
        lb_column1.Items.Add(new ListItem("Adhar No", "adharcardno"));
        lb_column1.Items.Add(new ListItem("PAN No", "PANGIRNumber"));
        lb_column1.Items.Add(new ListItem("Appointment FT/PT", "StfNature"));
        //lb_column1.Items.Add(new ListItem("Gross Pay Per Month", "netadd"));
        lb_column1.Items.Add(new ListItem("Appointment Type", "StfStatus"));
        lb_column1.Items.Add(new ListItem("Faculty Type", "FacultyType"));
        lb_column1.Items.Add(new ListItem("Pay Scale", "PayType"));
        lb_column1.Items.Add(new ListItem("Programme", "Programme"));
        lb_column1.Items.Add(new ListItem("Salary Mode", "PayMode"));
        lb_column1.Items.Add(new ListItem("PF Number", "PFNumber"));
        lb_column1.Items.Add(new ListItem("Bank Account Number", "BankAccount"));
        lb_column1.Items.Add(new ListItem("Bank Name", "Bank_Name"));
        lb_column1.Items.Add(new ListItem("Bank Branch Name", "Branch_Name"));
        lb_column1.Items.Add(new ListItem("IFSC Code", "IFSC_Code"));
        lb_column1.Items.Add(new ListItem("Is Physically Handicapped", "IsPhy"));
        lb_column1.Items.Add(new ListItem("Minority Indicator", "IsMin"));
        lb_column1.Items.Add(new ListItem("First Yr teacher", "IsFirstYr"));
        lb_column1.Items.Add(new ListItem("FY/Common Subject Teacher?", "IsFYCommon"));
        lb_column1.Items.Add(new ListItem("FY/Common Subject", "FYCommonSub"));
        lb_column1.Items.Add(new ListItem("Would you like to work sa Expert Member on various committees of AICTE", "AICTE_Comm"));
        lb_column1.Items.Add(new ListItem("Have you ever applied to AICTE for any grants/assistance", "AICTE_Grants"));
        //lb_column1.Items.Add(new ListItem("Basic Pay in Rs.", "BSalary"));
    }
    public void load()
    {
        lb_selectcolumn.Items.Clear();
        lb_selectcolumn.Items.Add(new ListItem("Staff Code", "staff_code"));
        lb_selectcolumn.Items.Add(new ListItem("Title", "Title"));
        lb_selectcolumn.Items.Add(new ListItem("SurName", "NameAcr"));
        lb_selectcolumn.Items.Add(new ListItem("Staff Name", "staff_name"));
        lb_selectcolumn.Items.Add(new ListItem("Appl No", "appl_no"));
        lb_selectcolumn.Items.Add(new ListItem("Department", "dept_name"));
        lb_selectcolumn.Items.Add(new ListItem("Designation", "desig_name"));
        lb_selectcolumn.Items.Add(new ListItem("Staff Type", "staff_type"));
        lb_selectcolumn.Items.Add(new ListItem("DOB", "date_of_birth"));
        lb_selectcolumn.Items.Add(new ListItem("Date Of Join", "exp_joindate"));
        lb_selectcolumn.Items.Add(new ListItem("Gender", "sex"));
        lb_selectcolumn.Items.Add(new ListItem("Caste", "Caste"));
        lb_selectcolumn.Items.Add(new ListItem("Religion", "religion"));
        lb_selectcolumn.Items.Add(new ListItem("Community", "Community"));
        lb_selectcolumn.Items.Add(new ListItem("Marital Status", "martial_status"));
        lb_selectcolumn.Items.Add(new ListItem("Date Of Apply", "dateofapply"));
        lb_selectcolumn.Items.Add(new ListItem("Email", "email"));
        lb_selectcolumn.Items.Add(new ListItem("Phone No", "Per_MobileNo"));
        lb_selectcolumn.Items.Add(new ListItem("Nationality", "Nationality"));
        lb_selectcolumn.Items.Add(new ListItem("Permanent Address", "per_address"));
        lb_selectcolumn.Items.Add(new ListItem("Communication Address", "comm_address"));
        lb_selectcolumn.Items.Add(new ListItem("Qualification", "qualification"));
        lb_selectcolumn.Items.Add(new ListItem("Familiar Subjects", "subjects"));
        lb_selectcolumn.Items.Add(new ListItem("Experience", "yofexp"));
        lb_selectcolumn.Items.Add(new ListItem("Blood Group", "bldgrp"));
        lb_selectcolumn.Items.Add(new ListItem("Adhar No", "adharcardno"));
        lb_selectcolumn.Items.Add(new ListItem("PAN No", "PANGIRNumber"));
        lb_selectcolumn.Items.Add(new ListItem("Appointment FT/PT", "StfNature"));
        //lb_selectcolumn.Items.Add(new ListItem("Gross Pay Per Month", "netadd"));
        lb_selectcolumn.Items.Add(new ListItem("Appointment Type", "StfStatus"));
        lb_selectcolumn.Items.Add(new ListItem("Faculty Type", "FacultyType"));
        lb_selectcolumn.Items.Add(new ListItem("Pay Scale", "PayType"));
        lb_selectcolumn.Items.Add(new ListItem("Programme", "Programme"));
        lb_selectcolumn.Items.Add(new ListItem("Salary Mode", "PayMode"));
        lb_selectcolumn.Items.Add(new ListItem("PF Number", "PFNumber"));
        lb_selectcolumn.Items.Add(new ListItem("Bank Account Number", "BankAccount"));
        lb_selectcolumn.Items.Add(new ListItem("Bank Name", "Bank_Name"));
        lb_selectcolumn.Items.Add(new ListItem("Bank Branch Name", "Branch_Name"));
        lb_selectcolumn.Items.Add(new ListItem("IFSC Code", "IFSC_Code"));
        lb_selectcolumn.Items.Add(new ListItem("Is Physically Handicapped", "IsPhy"));
        lb_selectcolumn.Items.Add(new ListItem("Minority Indicator", "IsMin"));
        lb_selectcolumn.Items.Add(new ListItem("First Yr teacher", "IsFirstYr"));
        lb_selectcolumn.Items.Add(new ListItem("FY/Common Subject Teacher?", "IsFYCommon"));
        lb_selectcolumn.Items.Add(new ListItem("FY/Common Subject", "FYCommonSub"));
        lb_selectcolumn.Items.Add(new ListItem("Would you like to work sa Expert Member on various committees of AICTE", "AICTE_Comm"));
        lb_selectcolumn.Items.Add(new ListItem("Have you ever applied to AICTE for any grants/assistance", "AICTE_Grants"));
        //lb_selectcolumn.Items.Add(new ListItem("Basic Pay in Rs.", "BSalary"));
    }
    public void loadright1()
    {
        lst_setting2.Items.Clear();
        lst_setting2.Items.Add(new ListItem("Appl No", "appl_no"));
        lst_setting2.Items.Add(new ListItem("Staff Code", "staff_code"));
        lst_setting2.Items.Add(new ListItem("Staff Name", "staff_name"));
        lst_setting2.Items.Add(new ListItem("Department", "dept_name"));
        lst_setting2.Items.Add(new ListItem("Designation", "desig_name"));
        lst_setting2.Items.Add(new ListItem("Staff Type", "staff_type"));
        lst_setting2.Items.Add(new ListItem("Experience Details", "experience_info"));
        lst_setting2.Items.Add(new ListItem("DOB", "date_of_birth"));
        lst_setting2.Items.Add(new ListItem("Date Of Join", "exp_joindate"));
        lst_setting2.Items.Add(new ListItem("Gender", "sex"));
        lst_setting2.Items.Add(new ListItem("Caste", "Caste"));
        lst_setting2.Items.Add(new ListItem("Religion", "religion"));
        lst_setting2.Items.Add(new ListItem("Community", "Community"));
        lst_setting2.Items.Add(new ListItem("Marital Status", "martial_status"));
        lst_setting2.Items.Add(new ListItem("Date Of Apply", "dateofapply"));
        lst_setting2.Items.Add(new ListItem("Email", "email"));
        lst_setting2.Items.Add(new ListItem("Phone No", "com_mobileno"));
        lst_setting2.Items.Add(new ListItem("Nationality", "Nationality"));
        lst_setting2.Items.Add(new ListItem("Educational Details", "qualification"));
        lst_setting2.Items.Add(new ListItem("Permanent Address", "per_address"));
        lst_setting2.Items.Add(new ListItem("Communication Address", "comm_address"));
        lst_setting2.Items.Add(new ListItem("Qualification", "qualification"));
        lst_setting2.Items.Add(new ListItem("Experience Details", "experience_info"));
        lst_setting2.Items.Add(new ListItem("Familiar Subjects", "subjects"));
        lst_setting2.Items.Add(new ListItem("Experience", "yofexp"));
        lst_setting2.Items.Add(new ListItem("Blood Group", "bldgrp"));
        lst_setting2.Items.Add(new ListItem("Adhar No", "adharcardno"));
    }
    public void load1()
    {
        lst_setting1.Items.Clear();
        lst_setting1.Items.Add(new ListItem("Appl No", "appl_no"));
        lst_setting1.Items.Add(new ListItem("Staff Code", "staff_code"));
        lst_setting1.Items.Add(new ListItem("Staff Name", "staff_name"));
        lst_setting1.Items.Add(new ListItem("Department", "dept_name"));
        lst_setting1.Items.Add(new ListItem("Designation", "desig_name"));
        lst_setting1.Items.Add(new ListItem("Staff Type", "staff_type"));
        lst_setting1.Items.Add(new ListItem("Experience Details", "experience_info"));
        lst_setting1.Items.Add(new ListItem("DOB", "date_of_birth"));
        lst_setting1.Items.Add(new ListItem("Date Of Join", "exp_joindate"));
        lst_setting1.Items.Add(new ListItem("Gender", "sex"));
        lst_setting1.Items.Add(new ListItem("Caste", "Caste"));
        lst_setting1.Items.Add(new ListItem("Religion", "religion"));
        lst_setting1.Items.Add(new ListItem("Community", "Community"));
        lst_setting1.Items.Add(new ListItem("Marital Status", "martial_status"));
        lst_setting1.Items.Add(new ListItem("Date Of Apply", "dateofapply"));
        lst_setting1.Items.Add(new ListItem("Email", "email"));
        lst_setting1.Items.Add(new ListItem("Phone No", "com_mobileno"));
        lst_setting1.Items.Add(new ListItem("Nationality", "Nationality"));
        lst_setting1.Items.Add(new ListItem("Educational Details", "qualification"));
        lst_setting1.Items.Add(new ListItem("Permanent Address", "per_address"));
        lst_setting1.Items.Add(new ListItem("Communication Address", "comm_address"));
        lst_setting1.Items.Add(new ListItem("Qualification", "qualification"));
        lst_setting1.Items.Add(new ListItem("Experience Details", "experience_info"));
        lst_setting1.Items.Add(new ListItem("Familiar Subjects", "subjects"));
        lst_setting1.Items.Add(new ListItem("Experience", "yofexp"));
        lst_setting1.Items.Add(new ListItem("Blood Group", "bldgrp"));
        lst_setting1.Items.Add(new ListItem("Adhar No", "adharcardno"));
    }
    protected void btnMvOneRt_Click(object sender, EventArgs e)
    {
        try
        {
            bool ok = true;
            if (lb_selectcolumn.Items.Count > 0 && lb_selectcolumn.SelectedItem.Value != "")
            {
                for (int j = 0; j < lb_column1.Items.Count; j++)
                {
                    if (lb_column1.Items[j].Value == lb_selectcolumn.SelectedItem.Value)
                    {
                        ok = false;
                    }
                }
                if (ok)
                {
                    ListItem lst = new ListItem(lb_selectcolumn.SelectedItem.Text, lb_selectcolumn.SelectedItem.Value);
                    lb_column1.Items.Add(lst);
                    lb_selectcolumn.Items.Remove(lst);
                }
            }
        }
        catch { }
    }
    protected void btnMvTwoRt_Click(object sender, EventArgs e)
    {
        try
        {
            lb_column1.Items.Clear();
            loadright();
            lb_selectcolumn.Items.Clear();
        }
        catch { }
    }
    protected void btnMvOneLt_Click(object sender, EventArgs e)
    {
        try
        {
            if (lb_column1.Items.Count > 0 && lb_column1.SelectedItem.Value != "")
            {
                lb_selectcolumn.Items.Add(new ListItem(lb_column1.SelectedItem.Text, lb_column1.SelectedItem.Value));
                lb_column1.Items.RemoveAt(lb_column1.SelectedIndex);
            }
        }
        catch { }
    }
    protected void btnMvTwoLt_Click(object sender, EventArgs e)
    {
        try
        {
            lb_column1.Items.Clear();
            load();
        }
        catch { }
    }
    protected void btnclose_click(object sender, EventArgs e)
    {
        poppernew.Visible = false;
        if (ddlrptname.SelectedItem.Text == "Select")
            lb_column1.Items.Clear();
    }
    protected void btnok_click(object sender, EventArgs e)
    {
        if (lb_column1.Items.Count > 0)
        {
            poppernew.Visible = false;
            lblalerterr.Visible = false;
            lblspread1_err.Visible = false;
        }
        else
        {
            lblalerterr.Visible = true;
            lblalerterr.Text = "Please select atleast one colunm then proceed!";
        }
    }
    public void imgbtn_settingpdf_Click(object sender, EventArgs e)
    {
        div_settingpdf.Visible = false;
        lst_setting2.Items.Clear();
    }
    public void img_settingpdf_Click(object sender, EventArgs e)
    {
        div_settingpdf.Visible = true;
        load1();
    }
    public void btnMvOneRt1_Click(object sender, EventArgs e)
    {
        try
        {
            bool ok = true;
            if (lst_setting1.Items.Count > 0 && lst_setting1.SelectedItem.Value != "")
            {
                for (int j = 0; j < lst_setting2.Items.Count; j++)
                {
                    if (lst_setting2.Items[j].Value == lst_setting1.SelectedItem.Value)
                    {
                        ok = false;
                    }
                }
                if (ok)
                {
                    ListItem lst = new ListItem(lst_setting1.SelectedItem.Text, lst_setting1.SelectedItem.Value);
                    lst_setting2.Items.Add(lst);
                    lst_setting1.Items.Remove(lst);
                }
            }
        }
        catch { }
    }
    public void btnMvTwoRt1_Click(object sender, EventArgs e)
    {
        try
        {
            lst_setting2.Items.Clear();
            loadright1();
            lst_setting1.Items.Clear();
        }
        catch { }
    }
    public void btnMvOneLt1_Click(object sender, EventArgs e)
    {
        try
        {
            if (lst_setting2.Items.Count > 0 && lst_setting2.SelectedItem.Value != "")
            {
                lst_setting1.Items.Add(new ListItem(lst_setting2.SelectedItem.Text, lst_setting2.SelectedItem.Value));
                lst_setting2.Items.RemoveAt(lst_setting2.SelectedIndex);
            }
        }
        catch { }
    }
    public void btnMvTwoLt1_Click(object sender, EventArgs e)
    {
        try
        {
            lst_setting2.Items.Clear();
            load1();
        }
        catch { }
    }
    public void btnok1_click(object sender, EventArgs e)
    {
        if (lst_setting2.Items.Count > 0)
        {
            div_settingpdf.Visible = false;
            lblalerterrnew.Visible = false;
        }
        else
        {
            lblalerterrnew.Visible = true;
            lblalerterrnew.Text = "Please select atleast one colunm then proceed!";
        }
    }
    public void btnclose1_click(object sender, EventArgs e)
    {
        div_settingpdf.Visible = false;
        lst_setting2.Items.Clear();
    }
    private string GetSelectedItemsValue(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        sbSelected.Append("," + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }
    private string GetSelectedItemsValueAsString(CheckBoxList cblSelected, out int count)
    {
        count = 0;
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    count++;
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }
    private string GetSelectedItemsText(CheckBoxList cblSelected, out int count)
    {
        count = 0;
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    count++;
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[sel].Text));
                    }
                    else
                    {
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[sel].Text));
                    }
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }
    protected void chkchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            if (chkchange.Checked == true)
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = true;
                }
                txtchange.Text = label + "(" + Convert.ToString(chklstchange.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = false;
                }
                txtchange.Text = "--Select--";
            }
        }
        catch { }
    }
    protected void chklstchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            txtchange.Text = "--Select--";
            chkchange.Checked = false;
            int count = 0;
            for (int i = 0; i < chklstchange.Items.Count; i++)
            {
                if (chklstchange.Items[i].Selected == true)
                {
                    count = count + 1;
                }
            }
            if (count > 0)
            {
                txtchange.Text = label + "(" + count + ")";
                if (count == chklstchange.Items.Count)
                {
                    chkchange.Checked = true;
                }
            }
        }
        catch { }
    }
}
//--------Last Modified on Oct 20th,2016-----------------------------------------//
//--------Common Print Report Name Added By Jeyaprakash on Oct 20th,2016---------//