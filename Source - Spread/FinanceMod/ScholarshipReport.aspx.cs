using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Drawing;

public partial class ScholarshipReport : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string sessstream = string.Empty;

    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    ArrayList colord = new ArrayList();
    static int chosedmode = 0;
    static int personmode = 0;
    static byte roll = 0;
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
            if (Session["collegecode"] == null)
            {
                string redrURI = ConfigurationManager.AppSettings["Logout"].Trim();
                Response.Redirect(redrURI, false);
                return;
            }
            //****************************************************//
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            sessstream = Convert.ToString(Session["streamcode"]);
            lbl_str1.Text = sessstream;
            // lbl_str2.Text = sessstream;
            if (!IsPostBack)
            {
                loadcollege();
                setLabelText();
                loadstrm();
                bindBtch();
                binddeg();
                binddept();
                bindsem();
                bindsec();
                loadheader();
                ledgerload();
                loadScholarshp();
                loadfinanceyear();
                loadsetting();
                loadColOrder();
            }
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode1 = ddl_collegename.SelectedValue;
            }
        }
        catch (Exception ex) { }
    }

    protected void lb3_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("default.aspx", false);

    }

    #region Load Filters

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

                collegecode1 = ddl_collegename.SelectedValue;
                //Session["collegecode"] = collegecode1;

            }

        }
        catch
        {
        }
    }
    protected void ddl_collegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        collegecode1 = ddl_collegename.SelectedValue;
        //Session["collegecode"] = collegecode1;
        loadstrm();
        bindBtch();
        binddeg();
        binddept();
        bindsem();
        bindsec();
        loadheader();
        ledgerload();
        loadScholarshp();
        loadfinanceyear();
        loadsetting();
        loadColOrder();
        loadScholarshp();

    }
    #endregion

    #region stream
    public void loadstrm()
    {
        try
        {
            string clgvalue = ddl_collegename.SelectedItem.Value.ToString();
            ddlstream.Items.Clear();
            string selqry = "select distinct type  from Course where college_code ='" + clgvalue + "' and type<>''";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlstream.DataSource = ds;
                ddlstream.DataTextField = "type";
                ddlstream.DataValueField = "type";
                ddlstream.DataBind();
                ddlstream.Enabled = true;
            }
            else
            {
                ddlstream.Enabled = false;
            }
            binddeg();
        }
        catch
        { }
    }
    protected void ddlstream_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string clgvalue = ddl_collegename.SelectedItem.Value.ToString();
            string stream = ddlstream.SelectedItem.Text.ToString();
            string selqry = "select distinct c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and type  in('" + stream + "') and d.college_code='" + clgvalue + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selqry, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {

                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "Course_Name";
                cbl_degree.DataValueField = "Course_Id";
                cbl_degree.DataBind();
            }
            for (int j = 0; j < cbl_degree.Items.Count; j++)
            {
                cbl_degree.Items[j].Selected = true;
                cb_degree.Checked = true;
            }

            txt_degree.Text = lbldeg.Text + "(" + cbl_degree.Items.Count + ")";
            binddept();
        }
        catch { }
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
            string batch = "";
            txt_batch.Text = "--Select--";
            if (cb_batch.Checked == true)
            {

                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    cbl_batch.Items[i].Selected = true;
                    batch = Convert.ToString(cbl_batch.Items[i].Text);
                }
                if (cbl_batch.Items.Count == 1)
                {
                    txt_batch.Text = "" + batch + "";
                }
                else
                {
                    txt_batch.Text = "Batch(" + (cbl_batch.Items.Count) + ")";
                }
                // txt_batch.Text = "Batch(" + (cbl_batch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    cbl_batch.Items[i].Selected = false;
                }
            }
            binddeg();
            binddept();
        }
        catch { }
    }
    protected void cbl_batch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string batch = "";
            cb_batch.Checked = false;
            int commcount = 0;
            txt_batch.Text = "--Select--";
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    batch = Convert.ToString(cbl_batch.Items[i].Text);
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_batch.Items.Count)
                {
                    cb_batch.Checked = true;
                }
                if (commcount == 1)
                {
                    txt_batch.Text = "" + batch + "";
                }
                else
                {
                    txt_batch.Text = "Batch(" + commcount.ToString() + ")";
                }
            }
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
            string stream = "";
            if (ddlstream.Items.Count > 0)
            {
                if (ddlstream.SelectedItem.Text != "")
                {
                    stream = ddlstream.SelectedItem.Text.ToString();
                }
            }

            cbl_degree.Items.Clear();
            string clgvalue = ddl_collegename.SelectedItem.Value.ToString();
            ds.Clear();
            string selqry = "select distinct  c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code='" + clgvalue + "'";
            if (stream != "")
            {
                selqry = selqry + " and type  in('" + stream + "')";
            }
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
        try
        {
            string degree = "";
            txt_degree.Text = "--Select--";
            if (cb_degree.Checked == true)
            {

                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = true;
                    degree = Convert.ToString(cbl_degree.Items[i].Text);
                }
                if (cbl_degree.Items.Count == 1)
                {
                    txt_degree.Text = "" + degree + "";
                }
                else
                {
                    txt_degree.Text = lbldeg.Text + "(" + (cbl_degree.Items.Count) + ")";
                }
                // txt_degree.Text = "Degree(" + (cbl_degree.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = false;
                }
            }
            binddept();
        }
        catch { }
    }
    protected void cbl_degree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string degree = "";
            cb_dept.Checked = false;
            int commcount = 0;
            cb_degree.Checked = false;
            txt_degree.Text = "--Select--";
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {

                    commcount = commcount + 1;
                    degree = Convert.ToString(cbl_degree.Items[i].Text);
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_degree.Items.Count)
                {
                    cb_degree.Checked = true;
                }
                if (commcount == 1)
                {
                    txt_degree.Text = "" + degree + "";
                }
                else
                {
                    txt_degree.Text = lbldeg.Text + "(" + commcount.ToString() + ")";
                }

            }
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
            for (int i = 0; i < cbl_batch.Items.Count; i++)
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
            for (int i = 0; i < cbl_degree.Items.Count; i++)
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
        try
        {
            string deptname = "";
            txt_dept.Text = "--Select--";
            if (cb_dept.Checked == true)
            {

                for (int i = 0; i < cbl_dept.Items.Count; i++)
                {
                    cbl_dept.Items[i].Selected = true;
                    deptname = Convert.ToString(cbl_dept.Items[i].Text);
                }
                if (cbl_dept.Items.Count == 1)
                {
                    txt_dept.Text = "" + deptname + "";
                }
                else
                {
                    txt_dept.Text = lbldept.Text + "(" + (cbl_dept.Items.Count) + ")";
                }

            }
            else
            {
                for (int i = 0; i < cbl_dept.Items.Count; i++)
                {
                    cbl_dept.Items[i].Selected = false;
                }
            }
            bindsec();
            bindsem();
        }
        catch { }
    }
    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string deptname = "";
            cb_dept.Checked = false;
            int commcount = 0;
            txt_dept.Text = "--Select--";
            for (int i = 0; i < cbl_dept.Items.Count; i++)
            {
                if (cbl_dept.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    deptname = Convert.ToString(cbl_dept.Items[i].Text);
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_dept.Items.Count)
                {
                    cb_dept.Checked = true;
                }
                if (commcount == 1)
                {
                    txt_dept.Text = "" + deptname + "";
                }
                else
                {
                    txt_dept.Text = lbldept.Text + "(" + commcount.ToString() + ")";
                }

            }
            bindsec();
            bindsem();
        }
        catch { }
    }
    #endregion

    #region sem
    protected void cb_sem_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_sem.Text = "--Select--";
            string sem = "";
            if (cb_sem.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = true;
                    sem = Convert.ToString(cbl_sem.Items[i].Text);
                }
                if (cbl_sem.Items.Count == 1)
                {
                    txt_sem.Text = "" + sem + "";
                }
                else
                {
                    txt_sem.Text = "Semester(" + (cbl_sem.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = false;
                }
            }
            bindsec();

        }
        catch (Exception ex)
        {

        }

    }
    protected void cbl_sem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cb_sem.Checked = false;
            int commcount = 0;
            txt_sem.Text = "--Select--";
            string sem = "";

            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_sem.Checked = false;
                    sem = Convert.ToString(cbl_sem.Items[i].Text);
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_sem.Items.Count)
                {
                    cb_sem.Checked = true;
                }
                if (commcount == 1)
                {
                    txt_sem.Text = "" + sem + "";
                }
                else
                {
                    txt_sem.Text = "Semester(" + commcount.ToString() + ")";
                }
            }

            bindsec();

        }
        catch (Exception ex)
        {

        }

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

    //protected void bindsem()
    //{
    //    try
    //    {
    //        string sem = "";
    //        string clgvalue = ddl_collegename.SelectedItem.Value.ToString();
    //        string semyear = "select * from New_InsSettings where linkname = 'SemesterandYear' and user_code ='" + usercode + "' and college_code ='" + clgvalue + "'";
    //        DataSet dsset = new DataSet();
    //        dsset.Clear();
    //        dsset = d2.select_method_wo_parameter(semyear, "Text");
    //        if (dsset.Tables.Count > 0 && dsset.Tables[0].Rows.Count > 0)
    //        {
    //            string value = Convert.ToString(dsset.Tables[0].Rows[0]["LinkValue"]);
    //            if (value == "1")
    //            {
    //                string SelectQ = "select * from textvaltable where TextCriteria = 'FEECA'and (textval like '%Semester' or textval like '%Year') and textval not like '-1%' and college_code ='" + clgvalue + "' order by len(textval),textval asc";
    //                ds.Clear();
    //                ds = d2.select_method_wo_parameter(SelectQ, "Text");
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    //text_circode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
    //                    cbl_sem.DataSource = ds;
    //                    cbl_sem.DataTextField = "TextVal";
    //                    cbl_sem.DataValueField = "TextCode";
    //                    cbl_sem.DataBind();
    //                }
    //                if (cbl_sem.Items.Count > 0)
    //                {
    //                    for (int i = 0; i < cbl_sem.Items.Count; i++)
    //                    {
    //                        cbl_sem.Items[i].Selected = true;
    //                        sem = Convert.ToString(cbl_sem.Items[i].Text);
    //                    }
    //                    if (cbl_sem.Items.Count == 1)
    //                    {
    //                        txt_sem.Text = "SemesterandYear(" + sem + ")";
    //                    }
    //                    else
    //                    {
    //                        txt_sem.Text = "SemesterandYear(" + cbl_sem.Items.Count + ")";
    //                    }
    //                    cb_sem.Checked = true;
    //                }

    //            }
    //            else
    //            {
    //                cbl_sem.Items.Clear();
    //                string settingquery = "select * from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + clgvalue + "'";
    //                ds.Clear();
    //                ds = d2.select_method_wo_parameter(settingquery, "Text");
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    string linkvalue = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
    //                    if (linkvalue == "0")
    //                    {
    //                        string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Semester' and textval not like '-1%' and college_code ='" + clgvalue + "' order by len(textval),textval asc";
    //                        ds.Clear();
    //                        ds = d2.select_method_wo_parameter(semesterquery, "Text");
    //                        if (ds.Tables[0].Rows.Count > 0)
    //                        {
    //                            //text_circode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
    //                            cbl_sem.DataSource = ds;
    //                            cbl_sem.DataTextField = "TextVal";
    //                            cbl_sem.DataValueField = "TextCode";
    //                            cbl_sem.DataBind();
    //                        }
    //                        if (cbl_sem.Items.Count > 0)
    //                        {
    //                            for (int i = 0; i < cbl_sem.Items.Count; i++)
    //                            {
    //                                cbl_sem.Items[i].Selected = true;
    //                                sem = Convert.ToString(cbl_sem.Items[i].Text);
    //                            }
    //                            if (cbl_sem.Items.Count == 1)
    //                            {
    //                                txt_sem.Text = "Semester(" + sem + ")";
    //                            }
    //                            else
    //                            {
    //                                txt_sem.Text = "Semester(" + cbl_sem.Items.Count + ")";
    //                            }
    //                            cb_sem.Checked = true;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Year' and textval not like '-1%' and college_code ='" + clgvalue + "' order by len(textval),textval asc";
    //                        ds.Clear();
    //                        ds = d2.select_method_wo_parameter(semesterquery, "Text");
    //                        if (ds.Tables[0].Rows.Count > 0)
    //                        {
    //                            // text_circode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
    //                            cbl_sem.DataSource = ds;
    //                            cbl_sem.DataTextField = "TextVal";
    //                            cbl_sem.DataValueField = "TextCode";
    //                            cbl_sem.DataBind();
    //                        }
    //                        if (cbl_sem.Items.Count > 0)
    //                        {
    //                            for (int i = 0; i < cbl_sem.Items.Count; i++)
    //                            {
    //                                cbl_sem.Items[i].Selected = true;
    //                                sem = Convert.ToString(cbl_sem.Items[i].Text);
    //                            }
    //                            if (cbl_sem.Items.Count == 1)
    //                            {
    //                                txt_sem.Text = "Year(" + sem + ")";
    //                            }
    //                            else
    //                            {
    //                                txt_sem.Text = "Year(" + cbl_sem.Items.Count + ")";
    //                            }
    //                            cb_sem.Checked = true;
    //                        }
    //                    }
    //                }
    //            }
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
            string clgvalue = ddl_collegename.SelectedItem.Value.ToString();
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
        try
        {
            int cout = 0;
            txt_sect.Text = "--Select--";
            string sec = "";
            if (cb_sect.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_sect.Items.Count; i++)
                {
                    cbl_sect.Items[i].Selected = true;
                    sec = Convert.ToString(cbl_sect.Items[i].Text);
                }
                if (cbl_sect.Items.Count == 1)
                {
                    txt_sect.Text = "" + sec + "";
                }
                else
                {
                    txt_sect.Text = "Semester(" + (cbl_sect.Items.Count) + ")";
                }

            }
            else
            {
                for (int i = 0; i < cbl_sect.Items.Count; i++)
                {
                    cbl_sect.Items[i].Selected = false;
                }
                txt_sect.Text = "--Select--";
            }

        }


        catch (Exception ex)
        {

        }
    }
    protected void cbl_sect_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string sec = "";
            int commcount = 0;
            txt_sect.Text = "--Select--";
            cb_sect.Checked = false;

            for (int i = 0; i < cbl_sect.Items.Count; i++)
            {
                if (cbl_sect.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    sec = Convert.ToString(cbl_sect.Items[i].Text);
                    cb_sect.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_sect.Items.Count)
                {

                    cb_sect.Checked = true;
                }
                if (commcount == 1)
                {
                    txt_sect.Text = "" + sec + "";
                }
                else
                {
                    txt_sect.Text = "Section(" + commcount.ToString() + ")";
                }

            }

        }

        catch (Exception ex)
        {

        }
    }
    #endregion

    #region headerandledger
    public void loadheader()
    {
        try
        {
            string clgvalue = ddl_collegename.SelectedItem.Value.ToString();
            chkl_studhed.Items.Clear();
            string query = " SELECT HeaderPK,HeaderName,hd_priority FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + clgvalue + "  order by len(isnull(hd_priority,10000)),hd_priority asc";

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
                txt_studhed.Text = "Header(" + chkl_studhed.Items.Count + ")";
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
            string clgvalue = ddl_collegename.SelectedItem.Value.ToString();
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
        try
        {
            string header = "";
            if (chk_studhed.Checked == true)
            {
                for (int i = 0; i < chkl_studhed.Items.Count; i++)
                {

                    chkl_studhed.Items[i].Selected = true;
                    header = Convert.ToString(chkl_studhed.Items[i].Text);
                }
                if (chkl_studhed.Items.Count == 1)
                {
                    txt_studhed.Text = "" + header + "";
                }
                else
                {
                    txt_studhed.Text = "Header(" + (chkl_studhed.Items.Count) + ")";
                }

            }
            else
            {
                for (int i = 0; i < chkl_studhed.Items.Count; i++)
                {
                    chkl_studhed.Items[i].Selected = false;
                }
                txt_studhed.Text = "---Select---";
            }

            ledgerload();
        }
        catch (Exception ex)
        {

        }
    }

    public void chkl_studhed_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string header = "";
            chkl_studled.Items.Clear();
            int commcount = 0;
            txt_studhed.Text = "--Select--";
            chk_studhed.Checked = false;
            for (int i = 0; i < chkl_studhed.Items.Count; i++)
            {
                if (chkl_studhed.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    header = Convert.ToString(chkl_studhed.Items[i].Text);
                }
            }
            if (commcount > 0)
            {
                if (commcount == chkl_studhed.Items.Count)
                {
                    chk_studhed.Checked = true;
                }
                if (commcount == 1)
                {
                    txt_studhed.Text = "" + header + "";
                }
                else
                {
                    txt_studhed.Text = "Header(" + commcount.ToString() + ")";
                }
            }
            ledgerload();
        }
        catch (Exception ex)
        {

        }
    }
    public void chk_studled_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string header = "";
            if (chk_studled.Checked == true)
            {
                for (int i = 0; i < chkl_studled.Items.Count; i++)
                {
                    chkl_studled.Items[i].Selected = true;
                    header = Convert.ToString(chkl_studled.Items[i].Text);
                }
                if (chkl_studled.Items.Count == 1)
                {
                    txt_studled.Text = "" + header + "";
                }
                else
                {
                    txt_studled.Text = "Ledger(" + (chkl_studled.Items.Count) + ")";
                }

            }
            else
            {
                for (int i = 0; i < chkl_studled.Items.Count; i++)
                {
                    chkl_studled.Items[i].Selected = false;
                }
                txt_studled.Text = "---Select---";
            }

        }
        catch (Exception ex)
        {

        }
    }
    public void chkl_studled_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            string header = "";
            txt_studled.Text = "--Select--";
            chk_studled.Checked = false;
            for (int i = 0; i < chkl_studled.Items.Count; i++)
            {
                if (chkl_studled.Items[i].Selected == true)
                {

                    commcount = commcount + 1;
                    header = Convert.ToString(chkl_studled.Items[i].Text);
                }
            }
            if (commcount > 0)
            {

                if (commcount == chkl_studled.Items.Count)
                {
                    chk_studled.Checked = true;

                }
                if (commcount == 1)
                {
                    txt_studled.Text = "" + header + "";
                }
                else
                {
                    txt_studled.Text = "Ledger(" + commcount.ToString() + ")";
                }
            }

        }
        catch (Exception ex)
        {

        }
    }
    #endregion

    #region scholarship
    private void loadScholarshp()
    {
        try
        {
            DataSet dsSchl = new DataSet();
            txtschol.Text = "";
            cbschol.Checked = false;
            cblschol.Items.Clear();
            string query = " select Distinct MasterValue,MasterCode from CO_MasterValues where MasterCriteria ='SchlolarshipReason' and CollegeCode ='" + collegecode1 + "' order by MasterValue asc";
            dsSchl = d2.select_method_wo_parameter(query, "Text");

            if (dsSchl.Tables.Count > 0 && dsSchl.Tables[0].Rows.Count > 0)
            {
                cblschol.DataSource = dsSchl;
                cblschol.DataTextField = "MasterValue";
                cblschol.DataValueField = "MasterCode";
                cblschol.DataBind();

                for (int i = 0; i < cblschol.Items.Count; i++)
                {
                    cblschol.Items[i].Selected = true;
                }
                txtschol.Text = "Reason (" + cblschol.Items.Count + ")";
                cbschol.Checked = true;
            }
        }
        catch { }
    }

    protected void cbschol_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txtschol.Text = "--Select--";
            string reason = "";
            if (cbschol.Checked == true)
            {
                cout++;
                for (int i = 0; i < cblschol.Items.Count; i++)
                {
                    cblschol.Items[i].Selected = true;
                    reason = Convert.ToString(cblschol.Items[i].Text);
                }
                if (cblschol.Items.Count == 1)
                {
                    txtschol.Text = "" + reason + "";
                }
                else
                {
                    txtschol.Text = "Reason(" + (cblschol.Items.Count) + ")";
                }

            }
            else
            {
                for (int i = 0; i < cblschol.Items.Count; i++)
                {
                    cblschol.Items[i].Selected = false;
                }
                txtschol.Text = "--Select--";
            }

        }


        catch (Exception ex)
        {

        }
    }
    protected void cblschol_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string reason = "";
            int commcount = 0;
            txtschol.Text = "--Select--";
            cbschol.Checked = false;

            for (int i = 0; i < cblschol.Items.Count; i++)
            {
                if (cblschol.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    reason = Convert.ToString(cblschol.Items[i].Text);
                    cb_sect.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cblschol.Items.Count)
                {

                    cbschol.Checked = true;
                }
                if (commcount == 1)
                {
                    txtschol.Text = "" + reason + "";
                }
                else
                {
                    txtschol.Text = "Reason(" + commcount.ToString() + ")";
                }

            }

        }

        catch (Exception ex)
        {

        }
    }
    #endregion

    #region financial year
    public void loadfinanceyear()
    {
        try
        {
            string fnalyr = "";
            string getfinanceyear = "select distinct convert(nvarchar(15),FinYearStart,103) sdate,convert(nvarchar(15),FinYearEnd,103) edate,FinYearPK from FM_FinYearMaster order by FinYearPK desc";
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
        try
        {
            string fnalyr = "";
            int count = 0;
            chkfyear.Checked = false;
            txtfyear.Text = "--Select--";
            for (int i = 0; i < chklsfyear.Items.Count; i++)
            {
                if (chklsfyear.Items[i].Selected == true)
                {
                    count++;
                    fnalyr = Convert.ToString(chklsfyear.Items[i].Text);
                }
            }
            if (count > 0)
            {
                // txtfyear.Text = "Finance Year (" + count + ")";
                if (count == chklsfyear.Items.Count)
                {
                    chkfyear.Checked = true;
                }
                if (count == 1)
                {
                    txtfyear.Text = "" + fnalyr + "";
                }
                else
                {
                    txtfyear.Text = "Finance Year (" + count.ToString() + ")";
                }
            }
            //loadheader();
        }
        catch (Exception ex)
        {

        }
    }
    protected void chkfyear_changed(object sender, EventArgs e)
    {
        try
        {
            string fnalyr = "";

            if (chkfyear.Checked == true)
            {
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

            }
            else
            {
                for (int i = 0; i < chklsfyear.Items.Count; i++)
                {
                    chklsfyear.Items[i].Selected = false;
                }
                txtfyear.Text = "--Select--";
            }
            // loadheader();
        }
        catch (Exception ex)
        {

        }
    }
    #endregion

    #region rollno and name

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getrno(string prefixText)
    {
        List<string> name = new List<string>();
        try
        {
            string query = "";
            WebService ws = new WebService();
            if (personmode == 0)
            {
                //student query
                if (chosedmode == 0)
                {
                    query = "select top 100 Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%' order by Roll_No";
                }
                else if (chosedmode == 1)
                {
                    query = "select  top 100 Reg_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Reg_No like '" + prefixText + "%' order by Reg_No";
                }
                else if (chosedmode == 2)
                {
                    query = "select  top 100 Roll_admit from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_admit like '" + prefixText + "%' order by Roll_admit";
                }
                else
                {
                    query = "  select  top 100 app_formno from applyn where admission_status =0 and isconfirm ='1' and app_formno like '" + prefixText + "%' order by app_formno";
                }
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
            ListItem list3 = new ListItem("Admission No", "2");
            ListItem list4 = new ListItem("App No", "3");

            rbl_rollno.Items.Clear();
            string insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollNo' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";

            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                rbl_rollno.Items.Add(list1);
            }


            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRegNo' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                rbl_rollno.Items.Add(list2);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollAdmit' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                rbl_rollno.Items.Add(list3);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptAppFormNo' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                rbl_rollno.Items.Add(list4);
            }
            if (rbl_rollno.Items.Count == 0)
            {
                rbl_rollno.Items.Add(list1);
            }
            switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
            {
                case 0:
                    txt_roll.Attributes.Add("placeholder", "Roll No");
                    chosedmode = 0;
                    break;
                case 1:
                    txt_roll.Attributes.Add("placeholder", "Reg No");
                    chosedmode = 1;
                    break;
                case 2:
                    txt_roll.Attributes.Add("placeholder", "Admin No");
                    chosedmode = 2;
                    break;
                case 3:
                    txt_roll.Attributes.Add("placeholder", "App No");
                    chosedmode = 3;
                    break;
            }



        }
        catch { }
    }
    protected void rbl_rollno_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_roll.Text = "";
            switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
            {
                case 0:
                    txt_roll.Attributes.Add("Placeholder", "Roll No");
                    chosedmode = 0;
                    break;
                case 1:
                    txt_roll.Attributes.Add("Placeholder", "Reg No");
                    chosedmode = 1;
                    break;
                case 2:
                    txt_roll.Attributes.Add("Placeholder", "Admin No");
                    chosedmode = 2;
                    break;
                case 3:
                    txt_roll.Attributes.Add("Placeholder", "App No");
                    chosedmode = 2;
                    break;
            }
        }
        catch { }
    }


    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();

        string query = "select a.stud_name+'-'+c.Course_Name+'-'+dt.Dept_Name+'-'+r.Roll_No,r.Roll_No from applyn a,Registration r ,Degree d,course c,Department dt  where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and a.stud_name like '" + prefixText + "%'";

        // studhash = ws.Getnamevalue(query);

        name = ws.Getname(query);
        return name;
    }

    #endregion

    #endregion

    #region Button Search

    private string GetSelectedText(CheckBoxList cblselected)
    {
        System.Text.StringBuilder selectedText = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblselected.Items.Count; sel++)
            {
                if (cblselected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append(Convert.ToString(cblselected.Items[sel].Text));
                    }
                    else
                    {
                        selectedText.Append("','" + Convert.ToString(cblselected.Items[sel].Text));
                    }
                }
            }
        }
        catch { cblselected.Items.Clear(); }
        return selectedText.ToString();
    }

    private string GetSelectedValue(CheckBoxList cblselected)
    {
        System.Text.StringBuilder selectedvalue = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblselected.Items.Count; sel++)
            {
                if (cblselected.Items[sel].Selected == true)
                {
                    if (selectedvalue.Length == 0)
                    {
                        selectedvalue.Append(Convert.ToString(cblselected.Items[sel].Value));
                    }
                    else
                    {
                        selectedvalue.Append("','" + Convert.ToString(cblselected.Items[sel].Value));
                    }
                }
            }
        }
        catch { cblselected.Items.Clear(); }
        return selectedvalue.ToString();
    }

    private bool checkColOrder()
    {
        Boolean colvalue = false;
        try
        {
            for (int col = 0; col < cblcolumnorder.Items.Count; col++)
            {
                if (cblcolumnorder.Items[col].Selected == true)
                {
                    colvalue = true;
                }
            }
        }
        catch { cblcolumnorder.Items.Clear(); }
        return colvalue;
    }

    private void loadcolumns(object sender, EventArgs e)
    {
        try
        {
            string linkname = "Scholarship column order settings";
            string columnvalue = "";
            int clsupdate = 0;
            DataSet dscol = new DataSet();
            string selcol = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "' and college_code='" + collegecode1 + "' ";
            dscol.Clear();
            dscol = d2.select_method_wo_parameter(selcol, "Text");
            if (checkColOrder())
            {
                if (cblcolumnorder.Items.Count > 0)
                {
                    colord.Clear();
                    for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                    {
                        if (cblcolumnorder.Items[i].Selected == true)
                        {
                            colord.Add(Convert.ToString(cblcolumnorder.Items[i].Value));
                            if (columnvalue == "")
                            {
                                columnvalue = Convert.ToString(cblcolumnorder.Items[i].Value);
                            }
                            else
                            {
                                columnvalue = columnvalue + ',' + Convert.ToString(cblcolumnorder.Items[i].Value);
                            }
                        }
                    }
                }
            }
            else if (dscol.Tables.Count > 0 && dscol.Tables[0].Rows.Count > 0)
            {
                colord.Clear();
                for (int col = 0; col < dscol.Tables[0].Rows.Count; col++)
                {
                    string value = Convert.ToString(dscol.Tables[0].Rows[col]["LinkValue"]);
                    string[] valuesplit = value.Split(',');
                    if (valuesplit.Length > 0)
                    {
                        for (int k = 0; k < valuesplit.Length; k++)
                        {
                            colord.Add(Convert.ToString(valuesplit[k]));
                            if (columnvalue == "")
                            {
                                columnvalue = Convert.ToString(valuesplit[k]);
                            }
                            else
                            {
                                columnvalue = columnvalue + ',' + Convert.ToString(valuesplit[k]);
                            }
                        }
                    }
                }
            }
            else
            {
                colord.Clear();
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = true;
                    colord.Add(Convert.ToString(cblcolumnorder.Items[i].Value));
                    if (columnvalue == "")
                    {
                        columnvalue = Convert.ToString(cblcolumnorder.Items[i].Value);
                    }
                    else
                    {
                        columnvalue = columnvalue + ',' + Convert.ToString(cblcolumnorder.Items[i].Value);
                    }
                }
            }
            if (columnvalue != "" && columnvalue != null)
            {
                string clsinsert = " if exists(select * from New_InsSettings where LinkName='" + linkname + "') update New_InsSettings set LinkValue='" + columnvalue + "' where LinkName='" + linkname + "' and user_code='" + usercode + "' and college_code='" + collegecode1 + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('" + linkname + "','" + columnvalue + "','" + usercode + "','" + collegecode1 + "')";
                clsupdate = d2.update_method_wo_parameter(clsinsert, "Text");
            }
            if (clsupdate == 1)
            {
                string sel = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "' and college_code='" + collegecode1 + "' ";
                DataSet dscolor = new DataSet();
                dscolor.Clear();
                dscolor = d2.select_method_wo_parameter(sel, "Text");
                if (dscolor.Tables.Count > 0)
                {
                    int count = 0;
                    if (dscolor.Tables[0].Rows.Count > 0)
                    {
                        string value = Convert.ToString(dscolor.Tables[0].Rows[0]["LinkValue"]);
                        string[] value1 = value.Split(',');
                        if (value1.Length > 0)
                        {
                            for (int i = 0; i < value1.Length; i++)
                            {
                                string val = value1[i].ToString();
                                for (int k = 0; k < cblcolumnorder.Items.Count; k++)
                                {
                                    if (val == cblcolumnorder.Items[k].Value)
                                    {
                                        cblcolumnorder.Items[k].Selected = true;
                                        count++;
                                    }
                                    if (count == cblcolumnorder.Items.Count)
                                    {
                                        cb_column.Checked = true;
                                    }
                                    else
                                    {
                                        cb_column.Checked = false;
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

    private void loadcolDetail(object sender, EventArgs e)
    {
        try
        {
            string linkname = "Scholarship column order settings";
            string columnvalue = "";
            DataSet dscol = new DataSet();
            string selcol = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "' and college_code='" + collegecode1 + "' ";
            dscol.Clear();
            dscol = d2.select_method_wo_parameter(selcol, "Text");

            if (dscol.Tables.Count > 0 && dscol.Tables[0].Rows.Count > 0)
            {
                colord.Clear();
                for (int col = 0; col < dscol.Tables[0].Rows.Count; col++)
                {
                    string value = Convert.ToString(dscol.Tables[0].Rows[col]["LinkValue"]);
                    string[] valuesplit = value.Split(',');
                    if (valuesplit.Length > 0)
                    {
                        for (int k = 0; k < valuesplit.Length; k++)
                        {
                            colord.Add(Convert.ToString(valuesplit[k]));
                            if (columnvalue == "")
                            {
                                columnvalue = Convert.ToString(valuesplit[k]);
                            }
                            else
                            {
                                columnvalue = columnvalue + ',' + Convert.ToString(valuesplit[k]);
                            }
                        }
                    }
                }

            }

            if (columnvalue != "" && columnvalue != null)
            {
                string clsinsert = " if exists(select * from New_InsSettings where LinkName='" + linkname + "') update New_InsSettings set LinkValue='" + columnvalue + "' where LinkName='" + linkname + "' and user_code='" + usercode + "' and college_code='" + collegecode1 + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('" + linkname + "','" + columnvalue + "','" + usercode + "','" + collegecode1 + "')";
                int clsupdate = d2.update_method_wo_parameter(clsinsert, "Text");
            }

        }
        catch { }
    }

    private DataSet loadDataset()
    {
        DataSet dsload = new DataSet();
        try
        {
            string batch = "";
            string deptcode = "";
            string feecat = "";
            string sec = "";
            string scholtype = "";
            string headerid = "";
            string ledgerid = "";
            string appno = "";
            string rollno = "";
            string SelectQ = "";
            string name = "";
            string finalyr = "";

            batch = GetSelectedValue(cbl_batch);
            deptcode = GetSelectedValue(cbl_dept);
            feecat = GetSelectedValue(cbl_sem);
            sec = GetSelectedValue(cbl_sect);
            scholtype = GetSelectedValue(cblschol);
            headerid = GetSelectedValue(chkl_studhed);
            ledgerid = GetSelectedValue(chkl_studled);
            finalyr = GetSelectedValue(chklsfyear);
            rollno = Convert.ToString(txt_roll.Text);
            name = Convert.ToString(txt_name.Text);
            if (rollno != "")
            {
                appno = Convert.ToString(getAppNo(rollno));
            }
            if (name != "")
            {
                string[] splitname = name.Split('-');
                if (splitname.Length > 0)
                {
                    name = splitname[3].ToString();
                    appno = Convert.ToString(getAppNo(name));
                }
            }
            if (appno == "")
            {
                SelectQ = "select SUM(TotalAmount) as allotamt,SUM(AdjusAmount) as receivedamt,SUM(TotalAmount)-SUM(AdjusAmount)as balamt,ReasonCode,HeaderFk,LedgerFK from FT_FinScholarship fs, Applyn P,Registration r WHERE fs.App_No = P.app_no and r.App_No =fs.App_No and p.app_no =r.App_No  AND P.IsConfirm = 1 AND Admission_Status = 1  and r.college_code ='" + collegecode1 + "' and fs.FinyearFK in('" + finalyr + "')";
                if (batch != "")
                    SelectQ = SelectQ + " and r.Batch_Year in ('" + batch + "')";
                if (deptcode != "")
                    SelectQ = SelectQ + "  and  r.Degree_Code in ('" + deptcode + "')";
                if (feecat != "")
                    SelectQ = SelectQ + " and fs.FeeCategory in ('" + feecat + "')";
                if (sec != "")
                    // selqry = selqry + " and   ISNULL( r.Sections,'') in ('" + sec + "','')";                
                    if (scholtype != "")
                        SelectQ = SelectQ + " and fs.ReasonCode in('" + scholtype + "')";
                if (headerid != "")
                    SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
                if (ledgerid != "")
                    SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
                SelectQ = SelectQ + " group by HeaderFk,LedgerFK,ReasonCode having sum(totalamount)>0";
                SelectQ = SelectQ + " select Distinct MasterValue,MasterCode from CO_MasterValues where MasterCriteria ='SchlolarshipReason' and CollegeCode ='" + collegecode1 + "' order by MasterValue asc";
                SelectQ = SelectQ + " select HeaderFK,LedgerPK,HeaderName,LedgerName from FM_HeaderMaster h,FM_LedgerMaster l where l.HeaderFK =h.HeaderPK and h.CollegeCode ='" + collegecode1 + "'  order by isnull(l.priority,1000), l.ledgerName asc ";
            }
            else
            {
                SelectQ = "select SUM(TotalAmount) as allotamt,SUM(AdjusAmount) as receivedamt,SUM(TotalAmount)-SUM(AdjusAmount)as balamt,fs.App_No,ReasonCode,HeaderFk,LedgerFK  from FT_FinScholarship fs, Applyn P,Registration r WHERE fs.App_No = P.app_no and r.App_No =fs.App_No and p.app_no =r.App_No  AND P.IsConfirm = 1 AND Admission_Status = 1  and r.college_code ='" + collegecode1 + "' and fs.FinyearFK in('" + finalyr + "')";
                if (appno != "")
                    SelectQ = SelectQ + "and r.App_No='" + appno + "'";
                if (feecat != "")
                    SelectQ = SelectQ + " and fs.FeeCategory in ('" + feecat + "')";
                if (scholtype != "")
                    SelectQ = SelectQ + " and fs.ReasonCode in('" + scholtype + "')";
                if (headerid != "")
                    SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
                if (ledgerid != "")
                    SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
                SelectQ = SelectQ + " group by HeaderFk,LedgerFK,ReasonCode,fs.App_No having sum(totalamount)>0";
                SelectQ = SelectQ + " select Distinct MasterValue,MasterCode from CO_MasterValues where MasterCriteria ='SchlolarshipReason' and CollegeCode ='" + collegecode1 + "' order by MasterValue asc";
                SelectQ = SelectQ + " select HeaderFK,LedgerPK,HeaderName,LedgerName from FM_HeaderMaster h,FM_LedgerMaster l where l.HeaderFK =h.HeaderPK and h.CollegeCode ='" + collegecode1 + "'  order by isnull(l.priority,1000), l.ledgerName asc ";
            }

            dsload.Clear();
            dsload = d2.select_method_wo_parameter(SelectQ, "Text");
        }
        catch { }
        return dsload;
    }

    private string getAppNo(string rollno)
    {
        string appno = "";
        try
        {
            if (txt_roll.Text.Trim() != "")
            {
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                {
                    appno = d2.GetFunction(" select App_No from Registration where Roll_No='" + rollno + "'");
                }
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                {
                    appno = d2.GetFunction(" select App_No from Registration where reg_no='" + rollno + "'");
                }
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                {
                    appno = d2.GetFunction(" select App_No from Registration where Roll_admit='" + rollno + "'");
                }
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 3)
                {
                    appno = d2.GetFunction(" select app_no from applyn where app_formno='" + rollno + "'");
                }
            }
            else
            {
                appno = d2.GetFunction(" select App_No from Registration where Roll_No='" + rollno + "'");
            }
        }
        catch { }
        return appno;
    }

    protected void btnsearch_Click(object sender, EventArgs e)
    {
        try
        {
            ds.Clear();
            ds = loadDataset();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                #region design
                loadcolumns(sender, e);
                int height = 0;
                DataView dv = new DataView();
                DataView dvhead = new DataView();
                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 0;
                FpSpread1.CommandBar.Visible = false;
                FpSpread1.Sheets[0].AutoPostBack = false;
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                FpSpread1.Sheets[0].RowHeader.Visible = false;
                FpSpread1.Sheets[0].ColumnCount = 8;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                FarPoint.Web.Spread.CheckBoxCellType selall = new FarPoint.Web.Spread.CheckBoxCellType();
                FarPoint.Web.Spread.CheckBoxCellType cblsel = new FarPoint.Web.Spread.CheckBoxCellType();
                cblsel.AutoPostBack = false;
                selall.AutoPostBack = true;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[0].Width = 60;


                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[1].Width = 90;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Scholarship Name";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Columns[2].Width = 162;


                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Header";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Columns[3].Width = 144;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Ledger";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Columns[4].Width = 144;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Allot";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Right;
                FpSpread1.Sheets[0].Columns[5].Visible = true;
                FpSpread1.Sheets[0].Columns[5].Width = 104;
                if (!colord.Contains("1"))
                {
                    FpSpread1.Sheets[0].Columns[5].Visible = false;
                }
                if (colord.Count == 0)
                {
                    FpSpread1.Sheets[0].Columns[5].Visible = true;
                }

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Received";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Right;
                FpSpread1.Sheets[0].Columns[6].Visible = true;
                FpSpread1.Sheets[0].Columns[6].Width = 104;
                if (!colord.Contains("2"))
                {
                    FpSpread1.Sheets[0].Columns[6].Visible = false;
                }
                if (colord.Count == 0)
                {
                    FpSpread1.Sheets[0].Columns[6].Visible = true;
                }

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Balance";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].ForeColor = ColorTranslator.FromHtml("#000000");
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Right;
                FpSpread1.Sheets[0].Columns[7].Visible = true;
                FpSpread1.Sheets[0].Columns[7].Width = 104;
                if (!colord.Contains("3"))
                {
                    FpSpread1.Sheets[0].Columns[7].Visible = false;
                }
                if (colord.Count == 0)
                {
                    FpSpread1.Sheets[0].Columns[7].Visible = true;
                }
                #endregion

                #region value
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    if (row == 0)
                    {
                        FpSpread1.Sheets[0].RowCount++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = selall;
                    }
                    FpSpread1.Sheets[0].RowCount++;
                    //height += 60;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = cblsel;
                    if (txt_roll.Text != "" || txt_name.Text != "")
                    {
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["App_No"]);
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        ds.Tables[1].DefaultView.RowFilter = "MasterCode='" + ds.Tables[0].Rows[row]["ReasonCode"] + "'";
                        dv = ds.Tables[1].DefaultView;
                        if (dv.Count != 0 && dv.Count != null)
                        {

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dv[0]["MasterValue"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(dv[0]["MasterCode"]);

                        }
                    }


                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        ds.Tables[2].DefaultView.RowFilter = " HeaderFK='" + Convert.ToString(ds.Tables[0].Rows[row]["HeaderFK"]) + "' and LedgerPK='" + Convert.ToString(ds.Tables[0].Rows[row]["LedgerFK"]) + "'";
                        dvhead = ds.Tables[2].DefaultView;
                        if (dvhead.Count > 0)
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dvhead[0]["HeaderName"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dvhead[0]["LedgerName"]);
                        }

                    }

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["allotamt"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[row]["receivedamt"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[row]["balamt"]);


                }
                FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);


                #endregion

                #region grandtot
                FpSpread1.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
                FpSpread1.Sheets[0].Rows.Count++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 5);
                double hedval = 0;
                for (int j = 5; j < FpSpread1.Sheets[0].Columns.Count; j++)
                {
                    for (int i = 0; i < FpSpread1.Rows.Count - 1; i++)
                    {
                        string values = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), j].Value);
                        if (values != "0" && values != "-" && values != "")
                        {
                            if (hedval == 0)
                            {
                                hedval = Convert.ToDouble(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), j].Text);
                            }
                            else
                            {
                                hedval = hedval + Convert.ToDouble(FpSpread1.Sheets[0].Cells[Convert.ToInt32(i), j].Text);
                            }
                        }
                    }
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(hedval);
                    hedval = 0;
                }
                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].BackColor = ColorTranslator.FromHtml("#4870BE");
                #endregion

                FpSpread1.Visible = true;
                divspread.Visible = true;
                btnview.Visible = true;
                pnlheader.Visible = true;
                pnlcolorder.Visible = true;
                // FpSpread1.Height = Convert.ToInt32(height);
                FpSpread1.Height = 450;
                FpSpread1.ShowHeaderSelection = false;
                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                FpSpread1.SaveChanges();
                print.Visible = true;
                divdetail.Visible = false;
                FpSpread2.Visible = false;
                error.Visible = false;
                lbl_alert.Text = "";
                // clearText();
            }
            else
            {
                FpSpread1.Visible = false;
                divspread.Visible = false;
                btnview.Visible = false;
                print.Visible = false;
                pnlheader.Visible = false;
                pnlcolorder.Visible = false;
                clearText();
                FpSpread2.Visible = false;
                divdetail.Visible = false;
                error.Visible = true;
                lbl_alert.Text = "No Record Found";
            }
        }
        catch { }
    }

    protected void btnview_Click(object sender, EventArgs e)
    {
        try
        {
            Hashtable httotal = new Hashtable();
            string colvalue = "";
            string reasoncode = "";
            string appno = "";
            string SelectQ = "";
            FpSpread1.SaveChanges();
            string actrow = FpSpread1.Sheets[0].ActiveRow.ToString();
            string actcol = FpSpread1.Sheets[0].ActiveColumn.ToString();
            int row = Convert.ToInt32(actrow);
            int col = Convert.ToInt32(actcol);
            if (actrow != "" && actcol != "")
            {
                for (int fpcol = 0; fpcol < FpSpread1.Sheets[0].Rows.Count - 1; fpcol++)
                {
                    if (fpcol == 0)
                        continue;
                    colvalue = Convert.ToString(FpSpread1.Sheets[0].Cells[fpcol, 1].Value);
                    if (colvalue == "1")
                    {
                        if (reasoncode == "")
                        {
                            reasoncode = Convert.ToString(FpSpread1.Sheets[0].Cells[fpcol, 2].Tag);
                            if (txt_roll.Text != "" || txt_name.Text != "")
                                appno = Convert.ToString(FpSpread1.Sheets[0].Cells[fpcol, 1].Tag);
                        }
                        else
                        {
                            reasoncode = reasoncode + "','" + "" + Convert.ToString(FpSpread1.Sheets[0].Cells[fpcol, 2].Tag) + "";
                            if (txt_roll.Text != "" || txt_name.Text != "")
                                appno = appno + "','" + "" + Convert.ToString(FpSpread1.Sheets[0].Cells[fpcol, 1].Tag) + "";
                        }
                    }
                }
                if (reasoncode != "")
                {
                    string batch = "";
                    string deptcode = "";
                    string feecat = "";
                    string sec = "";
                    string headerid = "";
                    string ledgerid = "";
                    string name = "";
                    string finalyr = "";

                    batch = GetSelectedValue(cbl_batch);
                    deptcode = GetSelectedValue(cbl_dept);
                    feecat = GetSelectedValue(cbl_sem);
                    sec = GetSelectedValue(cbl_sect);
                    //  scholtype = GetSelectedValue(cblschol);
                    headerid = GetSelectedValue(chkl_studhed);
                    ledgerid = GetSelectedValue(chkl_studled);
                    finalyr = GetSelectedValue(chklsfyear);

                    SelectQ = " SELECT R.app_no,Roll_No,Reg_No,r.roll_admit,R.Stud_Name,SUM(TotalAmount) as allotamt,SUM(AdjusAmount) as receivedamt,SUM(TotalAmount)-SUM(AdjusAmount)as balamt,ReasonCode,HeaderFk,LedgerFK,feecategory,r.degree_code,r.batch_year FROM FT_FinScholarship fs,Applyn P,Registration R WHERE fs.App_No = P.app_no AND P.app_no = R.App_No  AND P.IsConfirm = 1 AND Admission_Status = 1 and r.college_code ='" + collegecode1 + "' and fs.ReasonCode in('" + reasoncode + "')";
                    if (batch != "")
                        SelectQ = SelectQ + " and r.Batch_Year in ('" + batch + "')";
                    if (deptcode != "")
                        SelectQ = SelectQ + "  and  r.Degree_Code in ('" + deptcode + "')";
                    if (feecat != "")
                        SelectQ = SelectQ + " and fs.FeeCategory in ('" + feecat + "')";
                    if (headerid != "")
                        SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
                    if (ledgerid != "")
                        SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
                    if (finalyr != "")
                        SelectQ = SelectQ + "  and fs.FinyearFK in('" + finalyr + "')";
                    if (appno != "")
                    {
                        SelectQ = SelectQ + " and fs.app_no in('" + appno + "')";
                    }
                    SelectQ = SelectQ + "  group by R.app_no,Roll_No,Reg_No,R.Stud_Name,ReasonCode,HeaderFk,LedgerFK ,feecategory,r.degree_code,r.roll_admit,r.batch_year having SUM(TotalAmount)>0";
                    SelectQ = SelectQ + " select Distinct MasterValue,MasterCode from CO_MasterValues where MasterCriteria ='SchlolarshipReason' and CollegeCode ='" + collegecode1 + "' order by MasterValue asc";
                    SelectQ = SelectQ + " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename,(c.Course_Name +'-'+dt.dept_acronym)as dept_acronym from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode1 + "'";
                    SelectQ = SelectQ + " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code ='" + collegecode1 + "'";

                    ds.Clear();
                    ds = d2.select_method_wo_parameter(SelectQ, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        #region design
                        RollAndRegSettings();
                        DataView dv = new DataView();
                        loadcolDetail(sender, e);
                        FpSpread2.Sheets[0].RowCount = 0;
                        FpSpread2.Sheets[0].ColumnCount = 0;
                        FpSpread2.CommandBar.Visible = false;
                        FpSpread2.Sheets[0].AutoPostBack = true;
                        FpSpread2.Sheets[0].ColumnHeader.RowCount = 1;
                        FpSpread2.Sheets[0].RowHeader.Visible = false;
                        FpSpread2.Sheets[0].ColumnCount = 12;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;


                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Admission No";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Columns[3].VerticalAlign = VerticalAlign.Middle;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Name";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Batch";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Columns[5].VerticalAlign = VerticalAlign.Middle;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Department";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].Text = lblsem.Text;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Columns[7].VerticalAlign = VerticalAlign.Middle;


                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Scholarship";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 8].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Left;


                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Allot";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 9].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread2.Sheets[0].Columns[9].Visible = true;
                        if (!colord.Contains("1"))
                        {
                            FpSpread2.Sheets[0].Columns[9].Visible = false;
                        }
                        if (colord.Count == 0)
                        {
                            FpSpread2.Sheets[0].Columns[9].Visible = true;
                        }
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Received";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 10].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread2.Sheets[0].Columns[10].Visible = true;
                        if (!colord.Contains("2"))
                        {
                            FpSpread2.Sheets[0].Columns[10].Visible = false;
                        }
                        if (colord.Count == 0)
                        {
                            FpSpread2.Sheets[0].Columns[10].Visible = true;
                        }

                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Balance";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 11].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 11].Font.Bold = true;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 11].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 11].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 11].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Columns[11].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread2.Sheets[0].Columns[11].Visible = true;
                        if (!colord.Contains("3"))
                        {
                            FpSpread2.Sheets[0].Columns[11].Visible = false;
                        }
                        if (colord.Count == 0)
                        {
                            FpSpread2.Sheets[0].Columns[11].Visible = true;
                        }
                        spreadColumnVisible();
                        #endregion

                        #region value
                        for (int val = 0; val < ds.Tables[0].Rows.Count; val++)
                        {
                            FpSpread2.Sheets[0].RowCount++;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread2.Sheets[0].RowCount);
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[val]["Roll_No"]);
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[val]["Reg_No"]);
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[val]["roll_admit"]);
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[val]["Stud_Name"]);
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[val]["batch_year"]);
                            string deptname = string.Empty;
                            if (ds.Tables[2].Rows.Count > 0)
                            {
                                ds.Tables[2].DefaultView.RowFilter = "degree_code='" + ds.Tables[0].Rows[val]["degree_code"] + "'";
                                dv = ds.Tables[2].DefaultView;
                                if (dv.Count != 0 && dv.Count != null)
                                {
                                    if (cbdeptacr.Checked == true)
                                        deptname = Convert.ToString(dv[0]["dept_acronym"]);
                                    else
                                        deptname = Convert.ToString(dv[0]["degreename"]);
                                }
                            }
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Text = deptname;
                            string feecats = string.Empty;
                            if (ds.Tables[3].Rows.Count > 0)
                            {
                                ds.Tables[3].DefaultView.RowFilter = "textcode='" + ds.Tables[0].Rows[val]["feecategory"] + "'";
                                dv = ds.Tables[3].DefaultView;
                                if (dv.Count != 0 && dv.Count != null)
                                    feecats = Convert.ToString(dv[0]["textval"]);
                            }
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Text = feecats;
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                ds.Tables[1].DefaultView.RowFilter = "MasterCode='" + ds.Tables[0].Rows[val]["ReasonCode"] + "'";
                                dv = ds.Tables[1].DefaultView;
                                if (dv.Count != 0 && dv.Count != null)
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(dv[0]["MasterValue"]);
                            }
                            // FpSpread2.Sheets[0].Cells[4, FpSpread2.Sheets[0].RowCount - 1].Text = Convert.ToString(ds.Tables[0].Rows[val][""]);
                            double allotAmt = 0;
                            double paidAmount = 0;
                            double balAmt = 0;
                            double.TryParse(Convert.ToString(ds.Tables[0].Rows[val]["allotamt"]), out allotAmt);
                            double.TryParse(Convert.ToString(ds.Tables[0].Rows[val]["receivedamt"]), out paidAmount);
                            double.TryParse(Convert.ToString(ds.Tables[0].Rows[val]["balamt"]), out balAmt);

                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(allotAmt);
                            if (!httotal.ContainsKey(9))
                                httotal.Add(9, allotAmt);
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(httotal[9]), out amount);
                                amount += allotAmt;
                                httotal.Remove(9);
                                httotal.Add(9, Convert.ToString(amount));
                            }
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(paidAmount);
                            if (!httotal.ContainsKey(10))
                                httotal.Add(10, paidAmount);
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(httotal[10]), out amount);
                                amount += paidAmount;
                                httotal.Remove(10);
                                httotal.Add(10, Convert.ToString(amount));
                            }
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 11].Text = Convert.ToString(balAmt);
                            if (!httotal.ContainsKey(11))
                                httotal.Add(11, balAmt);
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(httotal[11]), out amount);
                                amount += balAmt;
                                httotal.Remove(11);
                                httotal.Add(11, Convert.ToString(amount));
                            }
                        }
                        FpSpread2.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSpread2.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSpread2.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSpread2.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSpread2.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSpread2.Sheets[0].SetColumnMerge(6, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSpread2.Sheets[0].SetColumnMerge(7, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        // FpSpread2.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                        //  FpSpread2.Sheets[0].Columns[4].VerticalAlign = VerticalAlign.Middle;
                        #endregion

                        #region grandtot
                        FpSpread2.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
                        FpSpread2.Sheets[0].Rows.Count++;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                        FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 1, 0, 1, 5);
                        double grandvalue = 0;
                        for (int j = 9; j < FpSpread2.Sheets[0].ColumnCount; j++)
                        {
                            double.TryParse(Convert.ToString(httotal[j]), out grandvalue);
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalue);
                        }
                        FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].Rows.Count - 1].BackColor = ColorTranslator.FromHtml("#4870BE");


                        #endregion

                        FpSpread2.Visible = true;
                        divdetail.Visible = true;
                        FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                        FpSpread2.SaveChanges();
                        error.Visible = false;
                        lbl_alert.Text = "";
                        // clearText();
                    }
                    else
                    {
                        FpSpread2.Visible = false;
                        divdetail.Visible = false;
                        clearText();
                        error.Visible = true;
                        lbl_alert.Text = "No Record Found";
                    }
                }

            }
        }
        catch { }
    }

    private void clearText()
    {
        loadsetting();
        txt_roll.Text = "";
        txt_name.Text = "";
        lblvalidation1.Text = "";
        txtexcelname.Text = "";
    }


    protected void FpSpread1_OnButtonCommand(object sender, EventArgs e)
    {
        try
        {
            string activerow = FpSpread1.Sheets[0].ActiveRow.ToString();
            string activecol = FpSpread1.Sheets[0].ActiveColumn.ToString();
            int actrow = Convert.ToInt32(activerow);
            if (activerow != "" && activecol != "")
            {
                if (actrow == 0)
                {
                    string value = Convert.ToString(FpSpread1.Sheets[0].Cells[actrow, 1].Value);
                    if (value == "1")
                    {
                        for (int row = 0; row < FpSpread1.Sheets[0].Rows.Count - 1; row++)
                        {
                            FpSpread1.Sheets[0].Cells[row, 1].Value = 1;
                        }
                    }
                    else
                    {
                        for (int row = 0; row < FpSpread1.Sheets[0].Rows.Count - 1; row++)
                        {
                            FpSpread1.Sheets[0].Cells[row, 1].Value = 0;
                        }
                    }
                }
            }
        }
        catch { }
    }

    #endregion


    #region print method

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
                lblvalidation1.Text = "Please Enter Your Scholarship Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }

        }
        catch
        {

        }

    }


    public void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails;
            string pagename;
            degreedetails = "scholarshipReport";
            pagename = "ScholarshipReport.aspx";
            Printcontrolhed.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrolhed.Visible = true;
        }
        catch { }
    }


    #endregion

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        error.Visible = false;
    }

    #region Column Order

    private void loadColOrder()
    {
        try
        {
            cblcolumnorder.Items.Add(new ListItem("Allot", "1"));
            cblcolumnorder.Items.Add(new ListItem("Received", "2"));
            cblcolumnorder.Items.Add(new ListItem("Balance", "3"));
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

        lbl.Add(lbl_collegename);
        lbl.Add(lbl_str1);
        lbl.Add(lbldeg);
        lbl.Add(lbldept);
        lbl.Add(lblsem);
        fields.Add(0);
        fields.Add(1);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }

    #region roll,reg,admission setting
    private void RollAndRegSettings()
    {
        try
        {
            DataSet dsl = new DataSet();
            string Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
            dsl = d2.select_method_wo_parameter(Master1, "text");
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Admission"] = "0";
            if (dsl.Tables[0].Rows.Count > 0)
            {
                for (int hf = 0; hf < dsl.Tables[0].Rows.Count; hf++)
                {
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Roll No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Register No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Admission No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Admission"] = "1";
                    }
                }
                settingValueRollAndReg(Convert.ToString(Session["Rollflag"]), Convert.ToString(Session["Regflag"]), Convert.ToString(Session["Admission"]));
            }
        }
        catch { }
    }
    private void settingValueRollAndReg(string rollvalue, string regvalue, string addmis)
    {
        // Tuple<byte, byte>
        string rollval = rollvalue;
        string regval = regvalue;
        string addVal = addmis;
        try
        {
            if (rollval == "0" && regval == "0" && addVal == "0")
                roll = 0;
            else if (rollval == "1" && regval == "1" && addVal == "1")
                roll = 1;
            else if (rollval == "1" && regval == "0" && addVal == "0")
                roll = 2;
            else if (rollval == "0" && regval == "1" && addVal == "0")
                roll = 3;
            else if (rollval == "0" && regval == "0" && addVal == "1")
                roll = 4;
            else if (rollval == "1" && regval == "1" && addVal == "0")
                roll = 5;
            else if (rollval == "0" && regval == "1" && addVal == "1")
                roll = 6;
            else if (rollval == "1" && regval == "0" && addVal == "1")
                roll = 7;
        }
        catch { }
        // return new Tuple<byte, byte>(roll,reg);

    }

    protected void spreadColumnVisible()
    {
        try
        {
            #region student wise
            if (roll == 0)
            {
                FpSpread2.Columns[1].Visible = true;
                FpSpread2.Columns[2].Visible = true;
                FpSpread2.Columns[3].Visible = true;
            }
            else if (roll == 1)
            {
                FpSpread2.Columns[1].Visible = true;
                FpSpread2.Columns[2].Visible = true;
                FpSpread2.Columns[3].Visible = true;
            }
            else if (roll == 2)
            {
                FpSpread2.Columns[1].Visible = true;
                FpSpread2.Columns[2].Visible = false;
                FpSpread2.Columns[3].Visible = false;

            }
            else if (roll == 3)
            {
                FpSpread2.Columns[1].Visible = false;
                FpSpread2.Columns[2].Visible = true;
                FpSpread2.Columns[3].Visible = false;
            }
            else if (roll == 4)
            {
                FpSpread2.Columns[1].Visible = false;
                FpSpread2.Columns[2].Visible = false;
                FpSpread2.Columns[3].Visible = true;
            }
            else if (roll == 5)
            {
                FpSpread2.Columns[1].Visible = true;
                FpSpread2.Columns[2].Visible = true;
                FpSpread2.Columns[3].Visible = false;
            }
            else if (roll == 6)
            {
                FpSpread2.Columns[1].Visible = false;
                FpSpread2.Columns[2].Visible = true;
                FpSpread2.Columns[3].Visible = true;
            }
            else if (roll == 7)
            {
                FpSpread2.Columns[1].Visible = true;
                FpSpread2.Columns[2].Visible = false;
                FpSpread2.Columns[3].Visible = true;
            }
            #endregion
        }
        catch { }
    }

    #endregion

    // last modified 04-10-2016 sudhagar
}