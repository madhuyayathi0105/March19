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

public partial class DailyPayment_Report : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataSet dsload = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hashval = new Hashtable();
    DataView dv = new DataView();
    string collegecode = string.Empty;
    string collegecode1 = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string usercode = string.Empty;
    static int personmode = 0;
    static int chosedmode = 0;
    int fpheight = 0;
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
            //****************************************************//
            collegecode = Convert.ToString(Session["collegecode"]);
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            usercode = Session["usercode"].ToString();
            lbl_str1.Text = Convert.ToString(Session["streamcode"]);

            if (!IsPostBack)
            {
                setLabelText();
                loadcollege();
                if (ddl_collegename.Items.Count > 0)
                {
                    collegecode = Convert.ToString(ddl_collegename.SelectedItem.Value);
                    collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
                }
                loadstrm();
                binddeg();
                bindBtch();
                binddept();
                bindsem();
                bindsec();
                loadfinanceyear();
                loadacctype();
                loadheaderandledger();
                ledgerload();
                rbstud.Checked = true;
                rbstud_OnCheckedChanged(sender, e);
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddl_collegename.SelectedItem.Value);
                collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
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
        catch
        {

        }
    }

    protected void ddl_collegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        //chkl_studhed.Items.Clear();
        //txt_studhed.Text = "--Select--";
        //lblheadorled.Text = "Header";
        txtfyear.Text = "--Select--";
        loadstrm();
        bindBtch();
        binddeg();
        binddept();
        bindsem();
        bindsec();
        loadheaderandledger();
        loadfinanceyear();
    }

    protected void ddlstream_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string stream = ddlstream.SelectedItem.Text.ToString();
            string selqry = "select distinct c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and type  in('" + stream + "') and d.college_code='" + collegecode1 + "'";
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
            int i = 0;
            cb_batch.Checked = false;
            int commcount = 0;
            string batch = "";
            txt_batch.Text = "--Select--";
            for (i = 0; i < cbl_batch.Items.Count; i++)
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
            int i = 0;
            cb_dept.Checked = false;
            int commcount = 0;
            cb_degree.Checked = false;
            txt_degree.Text = "--Select--";
            for (i = 0; i < cbl_degree.Items.Count; i++)
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

    protected void cb_dept_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string dept = "";
            int i = 0;
            txt_dept.Text = "--Select--";
            if (cb_dept.Checked == true)
            {

                for (i = 0; i < cbl_dept.Items.Count; i++)
                {
                    cbl_dept.Items[i].Selected = true;
                    dept = Convert.ToString(cbl_dept.Items[i].Text);
                }
                if (cbl_dept.Items.Count == 1)
                {
                    txt_dept.Text = "" + dept + "";

                }
                else
                {
                    txt_dept.Text = lbldept.Text + "(" + (cbl_dept.Items.Count) + ")";
                }

            }
            else
            {
                for (i = 0; i < cbl_dept.Items.Count; i++)
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
            string dept = "";
            int i = 0;
            cb_dept.Checked = false;
            int commcount = 0;
            txt_dept.Text = "--Select--";
            for (i = 0; i < cbl_dept.Items.Count; i++)
            {
                if (cbl_dept.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    dept = Convert.ToString(cbl_dept.Items[i].Text);
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
                    txt_dept.Text = "" + dept + "";
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

    protected void cb_sect_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string sec = "";
            int cout = 0;
            txt_sect.Text = "--Select--";
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
                    txt_sect.Text = "Section(" + (cbl_sect.Items.Count) + ")";
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

    protected void chk_studhed_OnCheckedChanged(object sender, EventArgs e)
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

    protected void chkl_studhed_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string header = "";
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
                    txt_studhed.Text = "Header(" + (chkl_studhed.Items.Count) + ")";

                }
            }
            ledgerload();
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlacctype_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlacctype.Items.Count > 0)
        {
            if (ddlacctype.SelectedItem.Value == "1")
            {
                // loadheaderandledger();
            }
            else if (ddlacctype.SelectedItem.Value == "2")
            {
                loadheaderandledger();
            }
            else if (ddlacctype.SelectedItem.Value == "3")
            {
                loadheaderandledger();
                // ledgerload();
            }
            else
            {
                loadheaderandledger();
            }
        }
    }

    public void chk_studled_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string ledger = "";
            if (chk_studled.Checked == true)
            {
                for (int i = 0; i < chkl_studled.Items.Count; i++)
                {
                    chkl_studled.Items[i].Selected = true;
                    ledger = Convert.ToString(chkl_studled.Items[i].Text);
                }
                if (chkl_studled.Items.Count == 1)
                {
                    txt_studled.Text = "" + ledger + "";

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
            string ledger = "";
            int commcount = 0;
            txt_studled.Text = "--Select--";
            chk_studled.Checked = false;
            for (int i = 0; i < chkl_studled.Items.Count; i++)
            {
                if (chkl_studled.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    ledger = Convert.ToString(chkl_studled.Items[i].Text);
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
                    txt_studled.Text = "" + ledger + "";
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

    protected void txt_fromdate_Textchanged(object sender, EventArgs e)
    {

    }

    protected void txt_todate_Textchanged(object sender, EventArgs e)
    {

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
                    txtfyear.Text = "Finance Year (" + (chklsfyear.Items.Count) + ")";
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
                    txtfyear.Text = "Finance Year (" + count + ")";
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

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
        {
        }
    }

    public void loadstrm()
    {
        try
        {
            ddlstream.Items.Clear();
            string selqry = "select distinct type  from Course where college_code ='" + collegecode1 + "' and type<>''";
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
            ds.Clear();
            string selqry = "select distinct  c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code='" + collegecode1 + "'";
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

    public void binddept()
    {
        try
        {
            cbl_dept.Items.Clear();
            cb_dept.Checked = false;
            txt_dept.Text = "---Select---";
            string batch2 = "";
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

            string degree = "";
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

            if (batch2 != "" && degree != "")
            {
                ds.Clear();
                ds = d2.BindBranchMultiple(singleuser, group_user, degree, collegecode1, usercode);
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
            if (build != "")
            {
                ds = d2.BindSectionDetailmult(collegecode1);
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

    public void loadacctype()
    {
        try
        {
            ddlacctype.Items.Clear();
            // ddlacctype.Items.Add(new ListItem("--Select--", "0"));
            // ddlacctype.Items.Add(new ListItem("Group Header", "1"));
            ddlacctype.Items.Add(new ListItem("Header", "2"));
            ddlacctype.Items.Add(new ListItem("Ledger", "3"));
        }
        catch { }
    }

    public void loadheaderandledger()
    {
        try
        {
            // lblledg.Text = "Group Header";
            txt_studhed.Text = "--Select--";
            chk_studhed.Checked = false;
            chkl_studhed.Items.Clear();
            string query = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + collegecode1 + "  ";
            // string query = " select HeaderPK,HeaderName from FM_HeaderMaster where CollegeCode ='" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chkl_studhed.DataSource = ds;
                chkl_studhed.DataTextField = "HeaderName";
                chkl_studhed.DataValueField = "HeaderPK";
                chkl_studhed.DataBind();
                if (ddlacctype.SelectedItem.Value == "2" || ddlacctype.SelectedItem.Value == "3")
                {
                    for (int i = 0; i < chkl_studhed.Items.Count; i++)
                    {
                        chkl_studhed.Items[i].Selected = true;
                    }
                    txt_studhed.Text = "Header(" + chkl_studhed.Items.Count + ")";
                    chk_studhed.Checked = true;
                }
                else
                {
                    for (int i = 0; i < chkl_studhed.Items.Count; i++)
                    {
                        chkl_studhed.Items[i].Selected = false;
                    }
                    txt_studhed.Text = "--Select--";
                    chk_studhed.Checked = false;
                }

            }
            ledgerload();
        }
        catch
        {
        }
    }

    public void ledgerload()
    {
        try
        {
            txt_studled.Text = "--Select--";
            chk_studled.Checked = false;
            chkl_studled.Items.Clear();
            string headerid = "";
            for (int i = 0; i < chkl_studhed.Items.Count; i++)
            {
                if (chkl_studhed.Items[i].Selected == true)
                {
                    if (headerid == "")
                    {
                        headerid = Convert.ToString(chkl_studhed.Items[i].Value);
                    }
                    else
                    {
                        headerid = headerid + "'" + "," + "'" + Convert.ToString(chkl_studhed.Items[i].Value);
                    }
                }
            }
            string query1 = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + "  and L.HeaderFK in('" + headerid + "')   and L.CollegeCode = " + collegecode1 + "";//AND  Ledgermode='1'
            //string query1 = " select LedgerPK,LedgerName from FM_LedgerMaster where CollegeCode='" + collegecode1 + "' and LedgerMode=1 and HeaderFK in('" + headerid + "')  order by isnull(priority,1000), ledgerName asc ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query1, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chkl_studled.DataSource = ds;
                    chkl_studled.DataTextField = "LedgerName";
                    chkl_studled.DataValueField = "LedgerPK";
                    chkl_studled.DataBind();
                    if (ddlacctype.SelectedItem.Value == "2" || ddlacctype.SelectedItem.Value == "3")
                    {
                        for (int i = 0; i < chkl_studled.Items.Count; i++)
                        {
                            chkl_studled.Items[i].Selected = true;
                        }
                        txt_studled.Text = "Ledger(" + chkl_studled.Items.Count + ")";
                        chk_studled.Checked = true;
                    }
                    else
                    {
                        for (int i = 0; i < chkl_studled.Items.Count; i++)
                        {
                            chkl_studled.Items[i].Selected = false;
                        }
                        txt_studled.Text = "--Select--";
                        chk_studled.Checked = false;
                    }
                }
                else
                {
                    for (int i = 0; i < chkl_studled.Items.Count; i++)
                    {
                        chkl_studled.Items[i].Selected = false;
                    }
                    txt_studled.Text = "--Select--";
                    chk_studled.Checked = false;
                }
            }

        }
        catch
        {
        }
    }

    public void loadfinanceyear()
    {
        try
        {
            string getfinanceyear = "select distinct convert(nvarchar(15),FinYearStart,103)+' - '+convert(nvarchar(15),FinYearEnd,103) as FinYear,FinYearPK from FM_FinYearMaster where CollegeCode='" + collegecode1 + "' order by FinYearPK desc";
            ds.Dispose();
            ds.Reset();
            chkfyear.Checked = false;
            chklsfyear.Items.Clear();
            ds = d2.select_method_wo_parameter(getfinanceyear, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklsfyear.DataSource = ds;
                chklsfyear.DataTextField = "FinYear";
                chklsfyear.DataValueField = "FinYearPK";
                chklsfyear.DataBind();

                for (int i = 0; i < chklsfyear.Items.Count; i++)
                {
                    chklsfyear.Items[i].Selected = true;
                }
                txtfyear.Text = "Finance Year (" + chklsfyear.Items.Count + ")";
                chkfyear.Checked = true;
            }
        }
        catch (Exception ex)
        {

        }
    }

    #region staff menu rbevent
    public void bindstaffdept(string scollege)
    {
        try
        {
            ds = d2.loaddepartment(scollege);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_staffdept.DataSource = ds;
                cbl_staffdept.DataTextField = "dept_name";
                cbl_staffdept.DataValueField = "Dept_Code";
                cbl_staffdept.DataBind();
            }

            for (int i = 0; i < cbl_staffdept.Items.Count; i++)
            {
                cbl_staffdept.Items[i].Selected = true;
            }
            txt_staffdept.Text = "Department(" + cbl_staffdept.Items.Count + ")";
            cb_staffdept.Checked = true;
        }
        catch (Exception e)
        { }
    }
    protected void cb_staffdept_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_staffdept.Checked == true)
            {
                for (int i = 0; i < cbl_staffdept.Items.Count; i++)
                {
                    cbl_staffdept.Items[i].Selected = true;
                }
                txt_staffdept.Text = "Department(" + (cbl_staffdept.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_staffdept.Items.Count; i++)
                {
                    cbl_staffdept.Items[i].Selected = false;
                }
                txt_staffdept.Text = "---Select---";
            }

        }
        catch (Exception ex)
        {

        }
    }
    protected void cbl_staffdept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_staffdept.Text = "--Select--";
            cb_staffdept.Checked = false;
            for (int i = 0; i < cbl_staffdept.Items.Count; i++)
            {
                if (cbl_staffdept.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_staffdept.Text = "Ledger(" + commcount.ToString() + ")";
                if (commcount == cbl_staffdept.Items.Count)
                {
                    cb_staffdept.Checked = true;
                }
            }

        }
        catch (Exception ex)
        {

        }
    }
    public void bindstaffdesig(string coll)
    {
        try
        {
            cbl_staffdesg.Items.Clear();
            ds.Clear();
            ds = d2.loaddesignation(coll);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_staffdesg.DataSource = ds;
                cbl_staffdesg.DataTextField = "desig_name";
                cbl_staffdesg.DataValueField = "Desig_Code";
                cbl_staffdesg.DataBind();

                for (int i = 0; i < cbl_staffdesg.Items.Count; i++)
                {
                    cbl_staffdesg.Items[i].Selected = true;
                }
                txt_staffdesg.Text = "Designation(" + cbl_staffdesg.Items.Count + ")";
                cb_staffdesg.Checked = true;
            }
        }
        catch (Exception e)
        {
        }
    }
    protected void cb_staffdesg_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_staffdesg.Checked == true)
            {
                for (int i = 0; i < cbl_staffdesg.Items.Count; i++)
                {
                    cbl_staffdesg.Items[i].Selected = true;
                }
                txt_staffdesg.Text = "Designation(" + (cbl_staffdesg.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_staffdesg.Items.Count; i++)
                {
                    cbl_staffdesg.Items[i].Selected = false;
                }
                txt_staffdesg.Text = "---Select---";
            }

        }
        catch (Exception ex)
        {

        }
    }
    protected void cbl_staffdesg_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_staffdesg.Text = "--Select--";
            cb_staffdesg.Checked = false;
            for (int i = 0; i < cbl_staffdesg.Items.Count; i++)
            {
                if (cbl_staffdesg.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_staffdesg.Text = "Designation(" + commcount.ToString() + ")";
                if (commcount == cbl_staffdesg.Items.Count)
                {
                    cb_staffdesg.Checked = true;
                }
            }

        }
        catch (Exception ex)
        {

        }
    }
    public void bindstafftype(string college)
    {
        try
        {
            cbl_stafftype.Items.Clear();
            ds.Clear();

            ds = d2.loadstafftype(college);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_stafftype.DataSource = ds;
                cbl_stafftype.DataTextField = "StfType";
                cbl_stafftype.DataValueField = "StfType";
                cbl_stafftype.DataBind();

                for (int i = 0; i < cbl_stafftype.Items.Count; i++)
                {
                    cbl_stafftype.Items[i].Selected = true;
                }
                txt_stafftype.Text = "Staff Type(" + cbl_stafftype.Items.Count + ")";
                cb_stafftype.Checked = true;
            }
        }
        catch (Exception)
        {

        }
    }
    protected void cb_stafftype_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_stafftype.Checked == true)
            {
                for (int i = 0; i < cbl_stafftype.Items.Count; i++)
                {
                    cbl_stafftype.Items[i].Selected = true;
                }
                txt_stafftype.Text = "Type(" + (cbl_stafftype.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_stafftype.Items.Count; i++)
                {
                    cbl_stafftype.Items[i].Selected = false;
                }
                txt_stafftype.Text = "---Select---";
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void cbl_stafftype_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_stafftype.Text = "--Select--";
            cb_stafftype.Checked = false;
            for (int i = 0; i < cbl_stafftype.Items.Count; i++)
            {
                if (cbl_stafftype.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_stafftype.Text = "Type(" + commcount.ToString() + ")";
                if (commcount == cbl_stafftype.Items.Count)
                {
                    cb_stafftype.Checked = true;
                }
            }

        }
        catch (Exception ex)
        { }
    }
    #endregion

    #region vendor menu rbevents
    public void bindvendorcode()
    {
        try
        {
            cbl_vendorcode.Items.Clear();
            txt_vendorcode.Text = "--Select--";
            cb_vendorcode.Checked = false;
            string select = "";
            ds.Clear();
            if (rbvendor.Checked == true)
            {
                select = " select VendorPK,VendorCode from CO_VendorMaster where VendorType=1";
            }
            else if (rbother.Checked == true)
            {
                select = " select VendorPK,VendorCode from CO_VendorMaster where VendorType='-5'";
            }
            ds = d2.select_method_wo_parameter(select, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_vendorcode.DataSource = ds;
                    cbl_vendorcode.DataTextField = "VendorCode";
                    cbl_vendorcode.DataValueField = "VendorPK";
                    cbl_vendorcode.DataBind();
                    for (int i = 0; i < cbl_vendorcode.Items.Count; i++)
                    {
                        cbl_vendorcode.Items[i].Selected = true;
                    }
                    txt_vendorcode.Text = "Code(" + cbl_vendorcode.Items.Count + ")";
                    cb_vendorcode.Checked = true;
                }
            }
        }
        catch { }
    }
    protected void cb_vendorcode_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_vendorcode.Checked == true)
            {
                for (int i = 0; i < cbl_vendorcode.Items.Count; i++)
                {
                    cbl_vendorcode.Items[i].Selected = true;
                }
                txt_vendorcode.Text = "Code(" + (cbl_vendorcode.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_vendorcode.Items.Count; i++)
                {
                    cbl_vendorcode.Items[i].Selected = false;
                }
                txt_vendorcode.Text = "---Select---";
            }
            bindvendername();
            bindvendercont();

        }
        catch (Exception ex)
        {

        }
    }
    protected void cbl_vendorcode_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_vendorcode.Text = "--Select--";
            cb_vendorcode.Checked = false;
            for (int i = 0; i < cbl_vendorcode.Items.Count; i++)
            {
                if (cbl_vendorcode.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_vendorcode.Text = "Code(" + commcount.ToString() + ")";
                if (commcount == cbl_vendorcode.Items.Count)
                {
                    cb_vendorcode.Checked = true;
                }
            }
            bindvendername();
            bindvendercont();

        }
        catch (Exception ex)
        {

        }
    }
    public void bindvendername()
    {
        try
        {
            string vendorpk = "";
            string select = "";
            for (int i = 0; i < cbl_vendorcode.Items.Count; i++)
            {
                if (cbl_vendorcode.Items[i].Selected == true)
                {
                    if (vendorpk == "")
                    {
                        vendorpk = Convert.ToString(cbl_vendorcode.Items[i].Value);
                    }
                    else
                    {
                        vendorpk = vendorpk + "'" + "," + "'" + Convert.ToString(cbl_vendorcode.Items[i].Value);
                    }
                }
            }
            cbl_vendorname.Items.Clear();
            txt_vendorname.Text = "--Select--";
            cb_vendorname.Checked = false;
            ds.Clear();
            if (rbvendor.Checked == true)
            {
                select = " select VendorPK,VendorCompName from CO_VendorMaster where VendorType='1' and VendorPK in('" + vendorpk + "')";
            }
            else if (rbother.Checked == true)
            {
                select = " select VendorPK,vendorname as VendorCompName from CO_VendorMaster where VendorType='-5' and VendorPK in('" + vendorpk + "')";
            }
            ds = d2.select_method_wo_parameter(select, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_vendorname.DataSource = ds;
                    cbl_vendorname.DataTextField = "VendorCompName";
                    cbl_vendorname.DataValueField = "VendorPK";
                    cbl_vendorname.DataBind();
                    for (int i = 0; i < cbl_vendorname.Items.Count; i++)
                    {
                        cbl_vendorname.Items[i].Selected = true;
                    }
                    txt_vendorname.Text = "Name(" + cbl_vendorname.Items.Count + ")";
                    cb_vendorname.Checked = true;
                }
            }
        }
        catch { }
    }
    protected void cb_vendorname_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_vendorname.Checked == true)
            {
                for (int i = 0; i < cbl_vendorname.Items.Count; i++)
                {
                    cbl_vendorname.Items[i].Selected = true;
                }
                txt_vendorname.Text = "Name(" + (cbl_vendorname.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_vendorname.Items.Count; i++)
                {
                    cbl_vendorname.Items[i].Selected = false;
                }
                txt_vendorname.Text = "---Select---";
            }
            bindvendercont();

        }
        catch (Exception ex)
        {

        }
    }
    protected void cbl_vendorname_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_vendorname.Text = "--Select--";
            cb_vendorname.Checked = false;
            for (int i = 0; i < cbl_vendorname.Items.Count; i++)
            {
                if (cbl_vendorname.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_vendorname.Text = "Name(" + commcount.ToString() + ")";
                if (commcount == cbl_vendorname.Items.Count)
                {
                    cb_vendorname.Checked = true;
                }
            }
            bindvendercont();
        }
        catch (Exception ex)
        {

        }
    }
    public void bindvendercont()
    {
        try
        {
            string vendorpk = "";
            string select = "";
            for (int i = 0; i < cbl_vendorcode.Items.Count; i++)
            {
                if (cbl_vendorcode.Items[i].Selected == true)
                {
                    if (vendorpk == "")
                    {
                        vendorpk = Convert.ToString(cbl_vendorcode.Items[i].Value);
                    }
                    else
                    {
                        vendorpk = vendorpk + "'" + "," + "'" + Convert.ToString(cbl_vendorcode.Items[i].Value);
                    }
                }
            }
            cbl_vendorcont.Items.Clear();
            txt_vendorcont.Text = "--Select--";
            cb_vendorcont.Checked = false;
            ds.Clear();
            if (rbvendor.Checked == true)
            {
                select = " select VenContactName,VendorContactPk from IM_VendorContactMaster where VendorFK in('" + vendorpk + "')";
            }
            else if (rbother.Checked == true)
            {
                select = " select VenContactName,VendorContactPk from IM_VendorContactMaster where VendorFK in('" + vendorpk + "')";
            }
            ds = d2.select_method_wo_parameter(select, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_vendorcont.DataSource = ds;
                    cbl_vendorcont.DataTextField = "VenContactName";
                    cbl_vendorcont.DataValueField = "VendorContactPk";
                    cbl_vendorcont.DataBind();
                    for (int i = 0; i < cbl_vendorcont.Items.Count; i++)
                    {
                        cbl_vendorcont.Items[i].Selected = true;
                    }
                    txt_vendorcont.Text = "Contact Name(" + cbl_vendorcont.Items.Count + ")";
                    cb_vendorcont.Checked = true;
                }
            }
        }
        catch { }
    }
    protected void cb_vendorcont_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_vendorcont.Checked == true)
            {
                for (int i = 0; i < cbl_vendorcont.Items.Count; i++)
                {
                    cbl_vendorcont.Items[i].Selected = true;
                }
                txt_vendorcont.Text = "Contact Name(" + (cbl_vendorcont.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_vendorcont.Items.Count; i++)
                {
                    cbl_vendorcont.Items[i].Selected = false;
                }
                txt_vendorcont.Text = "---Select---";
            }

        }
        catch (Exception ex)
        {

        }
    }
    protected void cbl_vendorcont_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_vendorcont.Text = "--Select--";
            cb_vendorcont.Checked = false;
            for (int i = 0; i < cbl_vendorcont.Items.Count; i++)
            {
                if (cbl_vendorcont.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_vendorcont.Text = "Contact Name(" + commcount.ToString() + ")";
                if (commcount == cbl_vendorcont.Items.Count)
                {
                    cb_vendorcont.Checked = true;
                }
            }

        }
        catch (Exception ex)
        {

        }
    }
    #endregion

    #region rb events
    protected void rbstud_OnCheckedChanged(object sender, EventArgs e)
    {
        //  maindiv.Attributes.Add("Style","width:1000px;");
        //stud menu
        tdstr.Visible = true;
        tdddlstr.Visible = true;
        tdbatch.Visible = true;
        tdcblbatch.Visible = true;
        tddegree.Visible = true;
        tdcbldegree.Visible = true;
        tddept.Visible = true;
        tdcbldept.Visible = true;
        tdsem.Visible = true;
        tdcblsem.Visible = true;
        tdsec.Visible = false;
        tdcblsec.Visible = false;
        //staff menu
        tdstaffdept.Visible = false;
        tdcblstaffdept.Visible = false;
        tdstaffdesg.Visible = false;
        tdcblstaffdesg.Visible = false;
        tdstafftype.Visible = false;
        tdcblstafftype.Visible = false;
        //vendor menu
        tdvendorcode.Visible = false;
        tdcblvendorcode.Visible = false;
        tdvendorname.Visible = false;
        tdcblvendorname.Visible = false;
        tdvendorcont.Visible = false;
        tdcblvendorcont.Visible = false;

        //fpspread
        //divspread.Visible = false;

        output.Text = "";
        print.Visible = false;
        lblvalidation1.Text = "";
        txtexcelname.Text = "";
        //setting
        loadsetting();
        personmode = 0;
        rbl_rollno.Visible = true;
        lbltext.Visible = false;
        txtsearch.Text = "";
        txtsearch.Attributes.Add("placeholder", "Roll No");
        tddivsearch.Visible = false;
        FpSpread1.Visible = false;

    }
    protected void rbstaff_OnCheckedChanged(object sender, EventArgs e)
    {
        // maindiv.Attributes.Add("Style", "width:1000px;");
        //stud menu
        tdstr.Visible = false;
        tdddlstr.Visible = false;
        tdbatch.Visible = false;
        tdcblbatch.Visible = false;
        tddegree.Visible = false;
        tdcbldegree.Visible = false;
        tddept.Visible = false;
        tdcbldept.Visible = false;
        tdsem.Visible = false;
        tdcblsem.Visible = false;
        tdsec.Visible = false;
        tdcblsec.Visible = false;
        //staff menu
        tdstaffdept.Visible = true;
        tdcblstaffdept.Visible = true;
        tdstaffdesg.Visible = true;
        tdcblstaffdesg.Visible = true;
        tdstafftype.Visible = true;
        tdcblstafftype.Visible = true;
        //vendor menu
        tdvendorcode.Visible = false;
        tdcblvendorcode.Visible = false;
        tdvendorname.Visible = false;
        tdcblvendorname.Visible = false;
        tdvendorcont.Visible = false;
        tdcblvendorcont.Visible = false;
        //load
        bindstaffdept(collegecode1);
        bindstaffdesig(collegecode1);
        bindstafftype(collegecode1);
        //fpspread
        //divspread.Visible = false;
        output.Text = "";
        print.Visible = false;
        lblvalidation1.Text = "";
        txtexcelname.Text = "";
        //setting
        personmode = 1;
        rbl_rollno.Visible = false;
        lbltext.Visible = true;
        lbltext.Text = "Search";
        txtsearch.Text = "";
        txtsearch.Attributes.Add("placeholder", "Staff Code");
        //  tddivsearch.Visible = true;
        FpSpread1.Visible = false;
        tddivsearch.Visible = false;
    }
    protected void rbvendor_OnCheckedChanged(object sender, EventArgs e)
    {
        //  maindiv.Attributes.Add("Style", "width:1000px;");
        //stud menu
        tdstr.Visible = false;
        tdddlstr.Visible = false;
        tdbatch.Visible = false;
        tdcblbatch.Visible = false;
        tddegree.Visible = false;
        tdcbldegree.Visible = false;
        tddept.Visible = false;
        tdcbldept.Visible = false;
        tdsem.Visible = false;
        tdcblsem.Visible = false;
        tdsec.Visible = false;
        tdcblsec.Visible = false;
        //staff menu
        tdstaffdept.Visible = false;
        tdcblstaffdept.Visible = false;
        tdstaffdesg.Visible = false;
        tdcblstaffdesg.Visible = false;
        tdstafftype.Visible = false;
        tdcblstafftype.Visible = false;
        //vendor menu
        tdvendorcode.Visible = true;
        tdcblvendorcode.Visible = true;
        tdvendorname.Visible = true;
        tdcblvendorname.Visible = true;
        tdvendorcont.Visible = true;
        tdcblvendorcont.Visible = true;

        //load 
        bindvendorcode();
        bindvendername();
        bindvendercont();
        //fpspread
        //  divspread.Visible = false;
        output.Text = "";
        print.Visible = false;
        lblvalidation1.Text = "";
        txtexcelname.Text = "";
        //setting
        personmode = 2;
        rbl_rollno.Visible = false;
        lbltext.Visible = true;
        lbltext.Text = "Search";
        txtsearch.Text = "";
        txtsearch.Attributes.Add("placeholder", "Vendor Code");
        tddivsearch.Visible = true;
        FpSpread1.Visible = false;
        tddivsearch.Visible = false;
    }
    protected void rbother_OnCheckedChanged(object sender, EventArgs e)
    {
        // maindiv.Attributes.Add("Style", "width:1000px;");
        //stud menu
        tdstr.Visible = false;
        tdddlstr.Visible = false;
        tdbatch.Visible = false;
        tdcblbatch.Visible = false;
        tddegree.Visible = false;
        tdcbldegree.Visible = false;
        tddept.Visible = false;
        tdcbldept.Visible = false;
        tdsem.Visible = false;
        tdcblsem.Visible = false;
        tdsec.Visible = false;
        tdcblsec.Visible = false;
        //staff menu
        tdstaffdept.Visible = false;
        tdcblstaffdept.Visible = false;
        tdstaffdesg.Visible = false;
        tdcblstaffdesg.Visible = false;
        tdstafftype.Visible = false;
        tdcblstafftype.Visible = false;
        //other menu
        tdvendorcode.Visible = true;
        tdcblvendorcode.Visible = true;
        tdvendorname.Visible = true;
        tdcblvendorname.Visible = true;
        tdvendorcont.Visible = false;
        tdcblvendorcont.Visible = false;
        //load 
        bindvendorcode();
        bindvendername();
        bindvendercont();
        //fpspread
        //   divspread.Visible = false;
        output.Text = "";
        print.Visible = false;
        lblvalidation1.Text = "";
        txtexcelname.Text = "";
        //setting
        personmode = 3;
        rbl_rollno.Visible = false;
        lbltext.Visible = true;
        lbltext.Text = "Search";
        txtsearch.Text = "";
        txtsearch.Attributes.Add("placeholder", "Others Code");
        tddivsearch.Visible = true;
        FpSpread1.Visible = false;
        tddivsearch.Visible = false;
    }
    protected void chkcumul_OnCheckedChanged(object sender, EventArgs e)
    {
    }
    #endregion

    #region button search
    protected void btnsearch_Click(object sender, EventArgs e)
    {
        try
        {
            bool boolCheck = false;
            bool boolCheckDs = false;
            if (cbReceipt.Checked || cbPayment.Checked)
            {
                ds.Reset();
                ds = loadDataset();
                if (ds.Tables.Count > 0 && (ds.Tables[0].Rows.Count > 0 || ds.Tables[1].Rows.Count > 0))
                {
                    boolCheck = true;
                    loadSpread(ds);
                }
                else
                    boolCheckDs = true;
            }
            if (!boolCheckDs && !boolCheck)
            {
                print.Visible = false;
                FpSpread1.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Select Any One Receipt or Payment')", true);
            }
            else if (boolCheckDs)
            {
                print.Visible = false;
                FpSpread1.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found')", true);
            }
            //ds.Clear();
            //ds = loaddataset();
            //if (ddlacctype.SelectedItem.Value != "0")
            //{
            //    if (rbstud.Checked == true)
            //    {
            //        studvalues(sender, e);
            //    }
            //    else if (rbstaff.Checked == true)
            //    {
            //        staffvalues(sender, e);
            //    }
            //    else if (rbvendor.Checked == true)
            //    {
            //        vendorvalues(sender, e);
            //    }
            //    else if (rbother.Checked == true)
            //    {
            //        othervalues(sender, e);
            //    }
            //}
        }
        catch
        { }
    }
    #endregion

    #region load Dataset

    public DataSet loaddataset()
    {
        try
        {

            string batchyr = "";
            string courseid = "";
            string feecat = "";
            string SelectQ = "";
            string sec = "";
            string headerid = "";
            string ledgerid = "";
            string finlyr = "";
            string fromdate = "";
            string todate = "";
            string headerorledger = "";
            string staffdept = "";
            string staffdesg = "";
            string stafftype = "";
            string vendorcode = "";
            string vendorname = "";
            string vendorcont = "";
            string txtcode = "";
            string Appno = "";

            #region stud get values
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    if (batchyr == "")
                    {
                        batchyr = Convert.ToString(cbl_batch.Items[i].Value);
                    }
                    else
                    {
                        batchyr = batchyr + "'" + "," + "'" + Convert.ToString(cbl_batch.Items[i].Value);
                    }
                }
            }
            for (int i = 0; i < cbl_dept.Items.Count; i++)
            {
                if (cbl_dept.Items[i].Selected == true)
                {
                    if (courseid == "")
                    {
                        courseid = Convert.ToString(cbl_dept.Items[i].Value);
                    }
                    else
                    {
                        courseid = courseid + "'" + "," + "'" + Convert.ToString(cbl_dept.Items[i].Value);
                    }
                }
            }
            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    if (feecat == "")
                    {
                        feecat = Convert.ToString(cbl_sem.Items[i].Value);
                    }
                    else
                    {
                        feecat = feecat + "'" + "," + "'" + Convert.ToString(cbl_sem.Items[i].Value);
                    }
                }
            }
            for (int i = 0; i < cbl_sect.Items.Count; i++)
            {
                if (cbl_sect.Items[i].Selected == true)
                {
                    if (sec == "")
                    {
                        sec = Convert.ToString(cbl_sect.Items[i].Value);
                    }
                    else
                    {
                        sec = sec + "'" + "," + "'" + Convert.ToString(cbl_sect.Items[i].Value);
                    }
                }
            }
            for (int i = 0; i < chkl_studhed.Items.Count; i++)
            {
                if (chkl_studhed.Items[i].Selected == true)
                {
                    if (headerid == "")
                    {
                        headerid = Convert.ToString(chkl_studhed.Items[i].Value);
                    }
                    else
                    {
                        headerid = headerid + "'" + "," + "'" + Convert.ToString(chkl_studhed.Items[i].Value);
                    }
                }
            }
            for (int i = 0; i < chkl_studled.Items.Count; i++)
            {
                if (chkl_studled.Items[i].Selected == true)
                {
                    if (ledgerid == "")
                    {
                        ledgerid = Convert.ToString(chkl_studled.Items[i].Value);
                    }
                    else
                    {
                        ledgerid = ledgerid + "'" + "," + "'" + Convert.ToString(chkl_studled.Items[i].Value);
                    }
                }
            }
            for (int i = 0; i < chklsfyear.Items.Count; i++)
            {
                if (chklsfyear.Items[i].Selected == true)
                {
                    if (finlyr == "")
                    {
                        finlyr = Convert.ToString(chklsfyear.Items[i].Value);
                    }
                    else
                    {
                        finlyr = finlyr + "'" + "," + "'" + Convert.ToString(chklsfyear.Items[i].Value);
                    }
                }
            }
            fromdate = txt_fromdate.Text;
            todate = txt_todate.Text;
            if (fromdate != "" && todate != "")
            {
                string[] frdate = fromdate.Split('/');
                if (frdate.Length == 3)
                {
                    fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
                }
                string[] tdate = todate.Split('/');
                if (tdate.Length == 3)
                {
                    todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
                }
            }
            #endregion

            #region staff get values
            for (int i = 0; i < cbl_staffdept.Items.Count; i++)
            {
                if (cbl_staffdept.Items[i].Selected == true)
                {
                    if (staffdept == "")
                    {
                        staffdept = Convert.ToString(cbl_staffdept.Items[i].Value);
                    }
                    else
                    {
                        staffdept = staffdept + "'" + "," + "'" + Convert.ToString(cbl_staffdept.Items[i].Value);
                    }
                }
            }
            for (int i = 0; i < cbl_staffdesg.Items.Count; i++)
            {
                if (cbl_staffdesg.Items[i].Selected == true)
                {
                    if (staffdesg == "")
                    {
                        staffdesg = Convert.ToString(cbl_staffdesg.Items[i].Value);
                    }
                    else
                    {
                        staffdesg = staffdesg + "'" + "," + "'" + Convert.ToString(cbl_staffdesg.Items[i].Value);
                    }
                }
            }
            for (int i = 0; i < cbl_stafftype.Items.Count; i++)
            {
                if (cbl_stafftype.Items[i].Selected == true)
                {
                    if (stafftype == "")
                    {
                        stafftype = Convert.ToString(cbl_stafftype.Items[i].Value);
                    }
                    else
                    {
                        stafftype = stafftype + "'" + "," + "'" + Convert.ToString(cbl_stafftype.Items[i].Value);
                    }
                }
            }
            #endregion

            #region textbox
            if (rbstud.Checked == true)
            {
                txtcode = Convert.ToString(txtsearch.Text);
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                {
                    Appno = d2.GetFunction(" select App_No from Registration where Roll_No='" + txtcode + "'");
                }
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                {
                    Appno = d2.GetFunction(" select App_No from Registration where reg_no='" + txtcode + "'");
                }
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                {
                    Appno = d2.GetFunction(" select App_No from Registration where Roll_admit='" + txtcode + "'");
                }
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 3)
                {
                    Appno = d2.GetFunction(" select app_no from applyn where app_formno='" + txtcode + "'");
                }
            }
            else if (rbstaff.Checked == true)
            {
                txtcode = Convert.ToString(txtsearch.Text);
                Appno = d2.GetFunction("select sa.appl_id  from staffmaster s,staff_appl_master sa where s.appl_no =sa.appl_no and s.staff_code ='" + txtcode + "'");
            }
            else if (rbvendor.Checked == true)
            {
                txtcode = Convert.ToString(txtsearch.Text);
                if (txtcode != "")
                {
                    string[] splitcode = txtcode.Split('-');
                    if (splitcode.Length > 0)
                    {
                        txtcode = Convert.ToString(splitcode[1]);
                        Appno = d2.GetFunction("select VendorContactPK from CO_VendorMaster v,IM_VendorContactMaster vc where v.VendorPK=vc.VendorFK and v.VendorCode='" + txtcode + "' and vendorType='1'");

                    }
                }
            }
            else if (rbother.Checked == true)
            {
                txtcode = Convert.ToString(txtsearch.Text);
                if (txtcode != "")
                {
                    string[] splitcode = txtcode.Split('-');
                    if (splitcode.Length > 0)
                    {
                        txtcode = Convert.ToString(splitcode[1]);
                        Appno = Convert.ToString(txtcode);
                    }
                }
            }
            #endregion

            #region vendor get values
            for (int i = 0; i < cbl_vendorcode.Items.Count; i++)
            {
                if (cbl_vendorcode.Items[i].Selected == true)
                {
                    if (vendorcode == "")
                    {
                        vendorcode = Convert.ToString(cbl_vendorcode.Items[i].Text);
                    }
                    else
                    {
                        vendorcode = vendorcode + "'" + "," + "'" + Convert.ToString(cbl_vendorcode.Items[i].Text);
                    }
                }
            }
            for (int i = 0; i < cbl_vendorname.Items.Count; i++)
            {
                if (cbl_vendorname.Items[i].Selected == true)
                {
                    if (vendorname == "")
                    {
                        vendorname = Convert.ToString(cbl_vendorname.Items[i].Value);
                    }
                    else
                    {
                        vendorname = vendorname + "'" + "," + "'" + Convert.ToString(cbl_vendorname.Items[i].Value);
                    }
                }
            }
            for (int i = 0; i < cbl_vendorcont.Items.Count; i++)
            {
                if (cbl_vendorcont.Items[i].Selected == true)
                {
                    if (vendorcont == "")
                    {
                        vendorcont = Convert.ToString(cbl_vendorcont.Items[i].Value);
                    }
                    else
                    {
                        vendorcont = vendorcont + "'" + "," + "'" + Convert.ToString(cbl_vendorcont.Items[i].Value);
                    }
                }
            }
            #endregion

            if (ddlacctype.SelectedItem.Value == "1")
            {
                headerorledger = "";
            }
            else if (ddlacctype.SelectedItem.Value == "2")
            {
                headerorledger = " ,HeaderFK";
            }
            else if (ddlacctype.SelectedItem.Value == "3")
            {
                headerorledger = ",LedgerFK";
            }

            if (rbstud.Checked == true)
            {
                #region stud
                if (chkcumul.Checked == false)
                {
                    if (txtsearch.Text == "")
                    {
                        SelectQ = " select Credit,TransCode,r.Roll_No,r.roll_admit,r.Reg_No,PayMode,r.Stud_Name,c.Course_Name +' - '+ dt.Dept_Name as Dept_Name ,CONVERT(varchar(20),TransDate,103) as  TransDate,f.App_no" + headerorledger + ",Narration  from FT_FinDailyTransaction f,Registration r,Degree d,Department dt,Course c where r.App_No=f.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and TransType ='2' ";
                        if (batchyr != "")
                        {
                            SelectQ = SelectQ + " and r.Batch_Year in ('" + batchyr + "')";
                        }
                        if (courseid != "")
                        {
                            SelectQ = SelectQ + "  and  r.Degree_Code in ('" + courseid + "')";
                        }
                        if (feecat != "")
                        {
                            // SelectQ = SelectQ + " and f.FeeCategory in ('" + feecat + "')";
                        }
                        if (sec != "")
                        {
                            //SelectQ = SelectQ + " and   ISNULL( r.Sections,'') in ('" + sec + "','')";
                        }

                        if (headerid != "")
                        {
                            SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
                        }
                        if (ledgerid != "")
                        {
                            SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
                        }
                        if (fromdate != "" && todate != "")
                        {
                            SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        }
                    }
                    else if (txtsearch.Text != "")
                    {
                        SelectQ = "   select Credit,TransCode,r.Roll_No,r.roll_admit,r.Reg_No,PayMode,r.Stud_Name,c.Course_Name +' - '+ dt.Dept_Name as Dept_Name ,CONVERT(varchar(20),TransDate,103) as  TransDate,f.App_no" + headerorledger + ",Narration  from FT_FinDailyTransaction f,Registration r,Degree d,Department dt,Course c where r.App_No=f.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and TransType ='2'";
                        if (Appno != "")
                        {
                            SelectQ = SelectQ + " and r.App_No='" + Appno + "'";
                        }
                        if (headerid != "")
                        {
                            SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
                        }
                        if (ledgerid != "")
                        {
                            SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
                        }
                        if (fromdate != "" && todate != "")
                        {
                            SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        }
                    }
                }
                else
                {
                    if (txtsearch.Text == "")
                    {
                        SelectQ = " select ISNULL( sum(Credit),0) as Credit,r.Roll_No,r.roll_admit,r.Reg_No,PayMode,r.Stud_Name,c.Course_Name +' - '+ dt.Dept_Name as Dept_Name ,f.App_no,Narration    from FT_FinDailyTransaction f,Registration r,Degree d,Department dt,Course c where r.App_No=f.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and TransType ='2' ";
                        if (batchyr != "")
                        {
                            SelectQ = SelectQ + " and r.Batch_Year in ('" + batchyr + "')";
                        }
                        if (courseid != "")
                        {
                            SelectQ = SelectQ + "  and  r.Degree_Code in ('" + courseid + "')";
                        }

                        if (headerid != "")
                        {
                            SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
                        }
                        if (ledgerid != "")
                        {
                            SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
                        }
                        if (fromdate != "" && todate != "")
                        {
                            SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        }
                        SelectQ = SelectQ + " group by r.Roll_No,r.Reg_No,PayMode,r.Stud_Name,c.Course_Name +' - '+ dt.Dept_Name  ,f.App_no,Narration,r.roll_admit  ";

                        SelectQ = SelectQ + "  select ISNULL( sum(Credit),0) as Credit,PayMode ,f.App_no " + headerorledger + "  from FT_FinDailyTransaction f,Registration r,Degree d,Department dt,Course c where r.App_No=f.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and TransType ='2'";
                        if (batchyr != "")
                        {
                            SelectQ = SelectQ + " and r.Batch_Year in ('" + batchyr + "')";
                        }
                        if (courseid != "")
                        {
                            SelectQ = SelectQ + "  and  r.Degree_Code in ('" + courseid + "')";
                        }

                        if (headerid != "")
                        {
                            SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
                        }
                        if (ledgerid != "")
                        {
                            SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
                        }
                        if (fromdate != "" && todate != "")
                        {
                            SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        }
                        SelectQ = SelectQ + "group by PayMode ,f.App_no " + headerorledger + " ";
                    }
                    else if (txtsearch.Text != "")
                    {
                        SelectQ = " select ISNULL( sum(Credit),0) as Credit,r.Roll_No,r.roll_admit,r.Reg_No,PayMode,r.Stud_Name,c.Course_Name +' - '+ dt.Dept_Name as Dept_Name ,f.App_no,Narration  from FT_FinDailyTransaction f,Registration r,Degree d,Department dt,Course c where r.App_No=f.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and TransType ='2' ";
                        if (Appno != "")
                        {
                            SelectQ = SelectQ + " and r.App_No='" + Appno + "'";
                        }
                        if (headerid != "")
                        {
                            SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
                        }
                        if (ledgerid != "")
                        {
                            SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
                        }
                        if (fromdate != "" && todate != "")
                        {
                            SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        }
                        SelectQ = SelectQ + " group by r.Roll_No,r.Reg_No,PayMode,r.Stud_Name,c.Course_Name +' - '+ dt.Dept_Name  ,f.App_no,Narration ,r.roll_admit  ";

                        SelectQ = SelectQ + "  select ISNULL( sum(Credit),0) as Credit,PayMode ,f.App_no " + headerorledger + "  from FT_FinDailyTransaction f,Registration r,Degree d,Department dt,Course c where r.App_No=f.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and TransType ='2'";
                        if (Appno != "")
                        {
                            SelectQ = SelectQ + " and r.App_No='" + Appno + "'";
                        }
                        if (headerid != "")
                        {
                            SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
                        }
                        if (ledgerid != "")
                        {
                            SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
                        }
                        if (fromdate != "" && todate != "")
                        {
                            SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        }
                        SelectQ = SelectQ + "group by PayMode ,f.App_no " + headerorledger + " ";
                    }
                }

                #endregion
            }
            else if (rbstaff.Checked == true)
            {
                #region staff
                if (chkcumul.Checked == false)
                {
                    if (txtsearch.Text == "")
                    {
                        SelectQ = " select  Credit,f.App_no,TransCode,sa.appl_id,s.staff_code,s.staff_name,sa.Dept_Code,h.dept_name,D.desig_name" + headerorledger + ",TransCode,CONVERT(varchar(20),TransDate,103) as TransDate,d.desig_code,f.Narration from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and h.dept_code =sa.dept_code and d.desig_code =sa.desig_code and T.staff_code =s.staff_code and T.dept_code =h.dept_code and T.desig_code =D.desig_code and T.latestrec ='1' ";
                        if (staffdept != "")
                        {
                            SelectQ = SelectQ + " and sa.Dept_Code in ('" + staffdept + "')";
                        }
                        if (staffdesg != "")
                        {
                            SelectQ = SelectQ + "  and d.desig_code in ('" + staffdesg + "')";
                        }
                        if (stafftype != "")
                        {
                            SelectQ = SelectQ + " and t.StfType in ('" + stafftype + "')";
                        }
                        if (headerid != "")
                        {
                            SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
                        }
                        if (ledgerid != "")
                        {
                            SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
                        }
                        if (fromdate != "" && todate != "")
                        {
                            SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        }
                    }
                    else if (txtsearch.Text != "")
                    {
                        SelectQ = "select  Credit,f.App_no,TransCode,sa.appl_id,s.staff_code,s.staff_name,sa.Dept_Code,h.dept_name,D.desig_name" + headerorledger + ",TransCode,CONVERT(varchar(20),TransDate,103) as  TransDate,d.desig_code,f.Narration from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and h.dept_code =sa.dept_code and d.desig_code =sa.desig_code and T.staff_code =s.staff_code and T.dept_code =h.dept_code and T.desig_code =D.desig_code and T.latestrec ='1'";
                        //and appl_id='" + Appno + "'";
                        if (Appno != "")
                        {
                            SelectQ = SelectQ + " and appl_id in ('" + Appno + "')";
                        }

                        if (headerid != "")
                        {
                            SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
                        }
                        if (ledgerid != "")
                        {
                            SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
                        }
                        if (fromdate != "" && todate != "")
                        {
                            SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        }
                    }
                }
                else
                {
                    if (txtsearch.Text == "")
                    {
                        SelectQ = " select  ISNULL( sum(Credit),0) as Credit,f.App_no,sa.appl_id,s.staff_code,s.staff_name,sa.Dept_Code,h.dept_name,D.desig_name ,d.desig_code,f.Narration from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and h.dept_code =sa.dept_code and d.desig_code =sa.desig_code and T.staff_code =s.staff_code and T.dept_code =h.dept_code and T.desig_code =D.desig_code and T.latestrec ='1'";
                        if (staffdept != "")
                        {
                            SelectQ = SelectQ + " and sa.Dept_Code in ('" + staffdept + "')";
                        }
                        if (staffdesg != "")
                        {
                            SelectQ = SelectQ + "  and d.desig_code in ('" + staffdesg + "')";
                        }
                        if (stafftype != "")
                        {
                            SelectQ = SelectQ + " and t.StfType in ('" + stafftype + "')";
                        }
                        if (headerid != "")
                        {
                            SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
                        }
                        if (ledgerid != "")
                        {
                            SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
                        }
                        if (fromdate != "" && todate != "")
                        {
                            SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        }
                        SelectQ = SelectQ + "group by f.App_no,sa.appl_id,s.staff_code,s.staff_name,sa.Dept_Code,h.dept_name,D.desig_name ,d.desig_code,Narration";

                        SelectQ = SelectQ + " select  ISNULL( sum(Credit),0) as credit,f.App_no" + headerorledger + " from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and h.dept_code =sa.dept_code and d.desig_code =sa.desig_code and T.staff_code =s.staff_code and T.dept_code =h.dept_code and T.desig_code =D.desig_code and T.latestrec ='1' ";
                        if (staffdept != "")
                        {
                            SelectQ = SelectQ + " and sa.Dept_Code in ('" + staffdept + "')";
                        }
                        if (staffdesg != "")
                        {
                            SelectQ = SelectQ + "  and d.desig_code in ('" + staffdesg + "')";
                        }
                        if (stafftype != "")
                        {
                            SelectQ = SelectQ + " and t.StfType in ('" + stafftype + "')";
                        }
                        if (headerid != "")
                        {
                            SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
                        }
                        if (ledgerid != "")
                        {
                            SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
                        }
                        if (fromdate != "" && todate != "")
                        {
                            SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        }
                        SelectQ = SelectQ + " group by f.App_no,sa.appl_id " + headerorledger + " ";
                    }
                    else if (txtsearch.Text != "")
                    {
                        SelectQ = " select  ISNULL( sum(Credit),0) as Credit,f.App_no,sa.appl_id,s.staff_code,s.staff_name,sa.Dept_Code,h.dept_name,D.desig_name ,d.desig_code,f.Narration from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and h.dept_code =sa.dept_code and d.desig_code =sa.desig_code and T.staff_code =s.staff_code and T.dept_code =h.dept_code and T.desig_code =D.desig_code and T.latestrec ='1'";
                        if (Appno != "")
                        {
                            SelectQ = SelectQ + " and appl_id in ('" + Appno + "')";
                        }

                        if (headerid != "")
                        {
                            SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
                        }
                        if (ledgerid != "")
                        {
                            SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
                        }
                        if (fromdate != "" && todate != "")
                        {
                            SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        }
                        SelectQ = SelectQ + "group by f.App_no,sa.appl_id,s.staff_code,s.staff_name,sa.Dept_Code,h.dept_name,D.desig_name ,d.desig_code,Narration";

                        SelectQ = SelectQ + " select  ISNULL( sum(Credit),0) as credit,f.App_no" + headerorledger + " from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and h.dept_code =sa.dept_code and d.desig_code =sa.desig_code and T.staff_code =s.staff_code and T.dept_code =h.dept_code and T.desig_code =D.desig_code and T.latestrec ='1' ";
                        if (Appno != "")
                        {
                            SelectQ = SelectQ + " and appl_id in ('" + Appno + "')";
                        }

                        if (headerid != "")
                        {
                            SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
                        }
                        if (ledgerid != "")
                        {
                            SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
                        }
                        if (fromdate != "" && todate != "")
                        {
                            SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        }
                        SelectQ = SelectQ + " group by f.App_no,sa.appl_id " + headerorledger + " ";
                    }
                }
                #endregion
            }
            else if (rbvendor.Checked == true)
            {
                #region vendor
                if (chkcumul.Checked == false)
                {
                    if (txtsearch.Text == "")
                    {
                        SelectQ = "  SELECT Credit,p.VendorCode,f.App_no,p.VendorCompName,vc.VendorContactPK,vc.VenContactName,TransCode" + headerorledger + ",CONVERT(varchar(20),TransDate,103) as  TransDate ,f.Narration  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1'";

                        if (vendorcode != "")
                        {
                            SelectQ = SelectQ + " and p.VendorCode in ('" + vendorcode + "')";
                        }
                        if (vendorname != "")
                        {
                            // SelectQ = SelectQ + "  and p.VendorName in ('" + vendorname + "')";
                        }
                        if (vendorcont != "")
                        {
                            SelectQ = SelectQ + " and vc.VendorContactPK in ('" + vendorcont + "')";
                        }
                        if (headerid != "")
                        {
                            SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
                        }
                        if (ledgerid != "")
                        {
                            SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
                        }
                        if (fromdate != "" && todate != "")
                        {
                            SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        }
                    }
                    else if (txtsearch.Text != "")
                    {
                        SelectQ = "  SELECT Credit,p.VendorCode,f.App_no,p.VendorCompName,vc.VendorContactPK,vc.VenContactName,TransCode" + headerorledger + ",CONVERT(varchar(20),TransDate,103) as  TransDate,f.Narration   FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1' ";
                        if (Appno != "")
                        {
                            SelectQ = SelectQ + " and App_No in ('" + Appno + "')";
                        }
                        if (headerid != "")
                        {
                            SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
                        }
                        if (ledgerid != "")
                        {
                            SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
                        }
                        if (fromdate != "" && todate != "")
                        {
                            SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        }
                    }
                }
                else
                {
                    if (txtsearch.Text == "")
                    {
                        SelectQ = "  SELECT ISNULL( sum(Credit),0) as Credit,p.VendorCode,f.App_no,p.VendorCompName,vc.VendorContactPK,vc.VenContactName ,f.Narration  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1'";
                        if (vendorcode != "")
                        {
                            SelectQ = SelectQ + " and p.VendorCode in ('" + vendorcode + "')";
                        }
                        if (vendorname != "")
                        {
                            // SelectQ = SelectQ + "  and p.VendorName in ('" + vendorname + "')";
                        }
                        if (vendorcont != "")
                        {
                            SelectQ = SelectQ + " and vc.VendorContactPK in ('" + vendorcont + "')";
                        }
                        if (headerid != "")
                        {
                            SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
                        }
                        if (ledgerid != "")
                        {
                            SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
                        }
                        if (fromdate != "" && todate != "")
                        {
                            SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        }
                        SelectQ = SelectQ + "  group by p.VendorCode,f.App_no,p.VendorCompName,vc.VendorContactPK,vc.VenContactName,Narration ";
                        SelectQ = SelectQ + " SELECT ISNULL( sum(Credit),0) as Credit,f.App_no,vc.VendorContactPK" + headerorledger + ",f.Narration  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1'";
                        if (vendorcode != "")
                        {
                            SelectQ = SelectQ + " and p.VendorCode in ('" + vendorcode + "')";
                        }
                        if (vendorname != "")
                        {
                            // SelectQ = SelectQ + "  and p.VendorName in ('" + vendorname + "')";
                        }
                        if (vendorcont != "")
                        {
                            SelectQ = SelectQ + " and vc.VendorContactPK in ('" + vendorcont + "')";
                        }
                        if (headerid != "")
                        {
                            SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
                        }
                        if (ledgerid != "")
                        {
                            SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
                        }
                        if (fromdate != "" && todate != "")
                        {
                            SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        }
                        SelectQ = SelectQ + " group by f.App_no,vc.VendorContactPK" + headerorledger + "";
                    }
                    else if (txtsearch.Text != "")
                    {
                        SelectQ = "  SELECT ISNULL( sum(Credit),0) as Credit,p.VendorCode,f.App_no,p.VendorCompName,vc.VendorContactPK,vc.VenContactName,f.Narration  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1'";
                        if (Appno != "")
                        {
                            SelectQ = SelectQ + " and App_No in ('" + Appno + "')";
                        }
                        if (headerid != "")
                        {
                            SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
                        }
                        if (ledgerid != "")
                        {
                            SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
                        }
                        if (fromdate != "" && todate != "")
                        {
                            SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        }
                        SelectQ = SelectQ + "  group by p.VendorCode,f.App_no,p.VendorCompName,vc.VendorContactPK,vc.VenContactName,Narration ";
                        SelectQ = SelectQ + " SELECT ISNULL( sum(Credit),0) as Credit,f.App_no,vc.VendorContactPK" + headerorledger + "  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1'";
                        if (Appno != "")
                        {
                            SelectQ = SelectQ + " and App_No in ('" + Appno + "')";
                        }
                        if (headerid != "")
                        {
                            SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
                        }
                        if (ledgerid != "")
                        {
                            SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
                        }
                        if (fromdate != "" && todate != "")
                        {
                            SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        }
                        SelectQ = SelectQ + " group by f.App_no,vc.VendorContactPK" + headerorledger + "";
                    }

                }
                #endregion
            }
            else if (rbother.Checked == true)
            {
                #region others
                if (chkcumul.Checked == false)
                {
                    if (txtsearch.Text == "")
                    {
                        SelectQ = "  SELECT Credit,p.VendorPK,p.VendorCode,f.App_no,p.VendorCompName,TransCode" + headerorledger + ",CONVERT(varchar(20),TransDate,103) as  TransDate,f.Narration   FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5'";
                        if (vendorcode != "")
                        {
                            SelectQ = SelectQ + " and p.VendorCode in ('" + vendorcode + "')";
                        }
                        if (vendorname != "")
                        {
                            // SelectQ = SelectQ + "  and p.VendorName in ('" + vendorname + "')";
                        }
                        if (vendorcont != "")
                        {
                            // SelectQ = SelectQ + " and vc.VendorContactPK in ('" + vendorcont + "')";
                        }
                        if (headerid != "")
                        {
                            SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
                        }
                        if (ledgerid != "")
                        {
                            SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
                        }
                        if (fromdate != "" && todate != "")
                        {
                            SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        }
                    }
                    else if (txtsearch.Text != "")
                    {
                        SelectQ = "  SELECT Credit,p.VendorPK,p.VendorCode,f.App_no,p.VendorCompName,TransCode" + headerorledger + ",CONVERT(varchar(20),TransDate,103) as  TransDate,f.Narration  FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5'";
                        if (Appno != "")
                        {
                            SelectQ = SelectQ + " and App_No in ('" + Appno + "')";
                        }
                        if (headerid != "")
                        {
                            SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
                        }
                        if (ledgerid != "")
                        {
                            SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
                        }
                        if (fromdate != "" && todate != "")
                        {
                            SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        }
                    }
                }
                else
                {
                    if (txtsearch.Text == "")
                    {
                        SelectQ = " SELECT ISNULL( sum(Credit),0) as credit,p.VendorPK,p.VendorCode,f.App_no,p.VendorCompName,f.Narration FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5'";
                        if (vendorcode != "")
                        {
                            SelectQ = SelectQ + " and p.VendorCode in ('" + vendorcode + "')";
                        }
                        if (headerid != "")
                        {
                            SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
                        }
                        if (ledgerid != "")
                        {
                            SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
                        }
                        if (fromdate != "" && todate != "")
                        {
                            SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        }
                        SelectQ = SelectQ + " group by p.VendorPK,p.VendorCode,f.App_no,p.VendorCompName,Narration";
                        SelectQ = SelectQ + " SELECT ISNULL( sum(Credit),0) as Credit,p.VendorPK,f.App_no" + headerorledger + " FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5' ";
                        if (vendorcode != "")
                        {
                            SelectQ = SelectQ + " and p.VendorCode in ('" + vendorcode + "')";
                        }
                        if (headerid != "")
                        {
                            SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
                        }
                        if (ledgerid != "")
                        {
                            SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
                        }
                        if (fromdate != "" && todate != "")
                        {
                            SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        }
                        SelectQ = SelectQ + " group by p.VendorPK,f.App_no" + headerorledger + "";
                    }
                    else if (txtsearch.Text != "")
                    {
                        SelectQ = " SELECT ISNULL( sum(Credit),0) as credit,p.VendorPK,p.VendorCode,f.App_no,p.VendorCompName,f.Narration FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5'";
                        if (Appno != "")
                        {
                            SelectQ = SelectQ + " and App_No in ('" + Appno + "')";
                        }
                        if (headerid != "")
                        {
                            SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
                        }
                        if (ledgerid != "")
                        {
                            SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
                        }
                        if (fromdate != "" && todate != "")
                        {
                            SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        }
                        SelectQ = SelectQ + " group by p.VendorPK,p.VendorCode,f.App_no,p.VendorCompName,Narration";
                        SelectQ = SelectQ + " SELECT ISNULL( sum(Credit),0) as Credit,p.VendorPK,f.App_no" + headerorledger + " FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5' ";
                        if (Appno != "")
                        {
                            SelectQ = SelectQ + " and App_No in ('" + Appno + "')";
                        }
                        if (headerid != "")
                        {
                            SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
                        }
                        if (ledgerid != "")
                        {
                            SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
                        }
                        if (fromdate != "" && todate != "")
                        {
                            SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        }
                        SelectQ = SelectQ + " group by p.VendorPK,f.App_no" + headerorledger + "";
                    }
                }
                #endregion
            }
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(SelectQ, "Text");
        }
        catch { }
        return dsload;
    }
    #endregion

    #region Load values method

    #region stud method

    public void studvalues(object sender, EventArgs e)
    {
        try
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    RollAndRegSettings();
                    if (chkcumul.Checked == false)
                    {
                        #region design
                        int count = 0;
                        DataView dv = new DataView();
                        FpSpread1.Sheets[0].RowCount = 0;
                        FpSpread1.Sheets[0].ColumnCount = 0;
                        FpSpread1.CommandBar.Visible = false;
                        FpSpread1.Sheets[0].AutoPostBack = true;
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                        FpSpread1.Sheets[0].RowHeader.Visible = false;
                        FpSpread1.Sheets[0].ColumnCount = 9;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                        FarPoint.Web.Spread.TextCellType regno = new FarPoint.Web.Spread.TextCellType();
                        FarPoint.Web.Spread.TextCellType rollno = new FarPoint.Web.Spread.TextCellType();

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Columns[1].Visible = true;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Admission No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        // FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Right;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Name";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = lbldeg.Text;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Voucher No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Voucher Date";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;


                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Narration";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Left;

                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 2, 1);
                        if (ddlacctype.Items.Count > 0)
                        {
                            if (ddlacctype.SelectedItem.Value == "2")
                            {
                                if (chkl_studhed.Items.Count > 0)
                                {
                                    for (int i = 0; i < chkl_studhed.Items.Count; i++)
                                    {
                                        if (chkl_studhed.Items[i].Selected == true)
                                        {
                                            count++;
                                            FpSpread1.Sheets[0].ColumnCount++;
                                            hashval.Add(Convert.ToString(chkl_studhed.Items[i].Value), FpSpread1.Sheets[0].ColumnCount - 1);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(chkl_studhed.Items[i].Text);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(chkl_studhed.Items[i].Value);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                        }
                                    }
                                }
                            }
                            else if (ddlacctype.SelectedItem.Value == "3")
                            {
                                if (chkl_studled.Items.Count > 0)
                                {
                                    for (int i = 0; i < chkl_studled.Items.Count; i++)
                                    {
                                        if (chkl_studled.Items[i].Selected == true)
                                        {
                                            count++;
                                            FpSpread1.Sheets[0].ColumnCount++;
                                            hashval.Add(Convert.ToString(chkl_studled.Items[i].Value), FpSpread1.Sheets[0].ColumnCount - 1);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(chkl_studled.Items[i].Text);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(chkl_studled.Items[i].Value);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                        }
                                    }
                                }
                            }
                        }
                        if (count != 0)
                        {
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 1, count);
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Paid";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].ForeColor = ColorTranslator.FromHtml("#000000");
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                        }

                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total Paid";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                        spreadColumnVisible();
                        #endregion

                        #region value
                        double totamount = 0;
                        double fnlamount = 0;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            fpheight += 50;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread1.Sheets[0].RowCount);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["Roll_No"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = rollno;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["Reg_No"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["roll_admit"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = regno;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["TransCode"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[i]["TransDate"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[i]["Narration"]);
                            if (ddlacctype.SelectedItem.Value == "1")
                            {
                            }
                            else if (ddlacctype.SelectedItem.Value == "2")
                            {
                                if (chkl_studhed.Items.Count > 0)
                                {
                                    for (int k = 0; k < chkl_studhed.Items.Count; k++)
                                    {
                                        if (chkl_studhed.Items[k].Selected == true)
                                        {
                                            output.Text = "Student-->Header";
                                            ds.Tables[0].DefaultView.RowFilter = "HeaderFK='" + Convert.ToString(chkl_studhed.Items[k].Value) + "' and App_no='" + Convert.ToString(ds.Tables[0].Rows[i]["App_no"]) + "' and TransCode='" + Convert.ToString(ds.Tables[0].Rows[i]["TransCode"]) + "' ";
                                            dv = ds.Tables[0].DefaultView;
                                            int countval = Convert.ToInt32(hashval[(Convert.ToString(chkl_studhed.Items[k].Value))]);
                                            if (dv.Count == 0 || dv.Count == null)
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, countval].Text = "-";
                                            }
                                            else
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, countval].Text = Convert.ToString(dv[0]["Credit"]);
                                                totamount = Convert.ToDouble(dv[0]["Credit"]);
                                                if (totamount != 0)
                                                {
                                                    fnlamount = fnlamount + totamount;
                                                }
                                            }
                                        }
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(fnlamount);
                                    fnlamount = 0;
                                }
                            }
                            else if (ddlacctype.SelectedItem.Value == "3")
                            {
                                if (chkl_studled.Items.Count > 0)
                                {
                                    for (int k = 0; k < chkl_studled.Items.Count; k++)
                                    {
                                        if (chkl_studled.Items[k].Selected == true)
                                        {
                                            output.Text = "Student-->Ledger";
                                            ds.Tables[0].DefaultView.RowFilter = "LedgerFK='" + Convert.ToString(chkl_studled.Items[k].Value) + "' and App_no='" + Convert.ToString(ds.Tables[0].Rows[i]["App_no"]) + "' and TransCode='" + Convert.ToString(ds.Tables[0].Rows[i]["TransCode"]) + "'";
                                            dv = ds.Tables[0].DefaultView;
                                            int countval = Convert.ToInt32(hashval[(Convert.ToString(chkl_studled.Items[k].Value))]);
                                            if (dv.Count == 0 || dv.Count == null)
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, countval].Text = "-";
                                            }
                                            else
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, countval].Text = Convert.ToString(dv[0]["Credit"]);
                                                totamount = Convert.ToDouble(dv[0]["Credit"]);
                                                if (totamount != 0)
                                                {
                                                    fnlamount = fnlamount + totamount;
                                                }
                                            }
                                        }
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(fnlamount);
                                    fnlamount = 0;
                                }
                            }

                        }

                        #endregion

                        #region grandtot
                        FpSpread1.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].Rows.Count - 1, 0, 1, 8);
                        double hedval = 0;
                        for (int j = 9; j < FpSpread1.Sheets[0].Columns.Count; j++)
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
                    }
                    else
                    {

                        #region design
                        int count = 0;
                        DataView dv = new DataView();
                        FpSpread1.Sheets[0].RowCount = 0;
                        FpSpread1.Sheets[0].ColumnCount = 0;
                        FpSpread1.CommandBar.Visible = false;
                        FpSpread1.Sheets[0].AutoPostBack = true;
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                        FpSpread1.Sheets[0].RowHeader.Visible = false;
                        FpSpread1.Sheets[0].ColumnCount = 6;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                        FarPoint.Web.Spread.TextCellType regno = new FarPoint.Web.Spread.TextCellType();
                        FarPoint.Web.Spread.TextCellType rollno = new FarPoint.Web.Spread.TextCellType();

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Columns[1].Visible = true;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        // FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Right;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Name";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = lbldeg.Text;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Narration";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;


                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                        if (ddlacctype.Items.Count > 0)
                        {
                            if (ddlacctype.SelectedItem.Value == "2")
                            {
                                if (chkl_studhed.Items.Count > 0)
                                {
                                    for (int i = 0; i < chkl_studhed.Items.Count; i++)
                                    {
                                        if (chkl_studhed.Items[i].Selected == true)
                                        {
                                            count++;
                                            FpSpread1.Sheets[0].ColumnCount++;
                                            hashval.Add(Convert.ToString(chkl_studhed.Items[i].Value), FpSpread1.Sheets[0].ColumnCount - 1);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(chkl_studhed.Items[i].Text);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(chkl_studhed.Items[i].Value);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                        }
                                    }
                                }
                            }
                            else if (ddlacctype.SelectedItem.Value == "3")
                            {
                                if (chkl_studled.Items.Count > 0)
                                {
                                    for (int i = 0; i < chkl_studled.Items.Count; i++)
                                    {
                                        if (chkl_studled.Items[i].Selected == true)
                                        {
                                            count++;
                                            FpSpread1.Sheets[0].ColumnCount++;
                                            hashval.Add(Convert.ToString(chkl_studled.Items[i].Value), FpSpread1.Sheets[0].ColumnCount - 1);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(chkl_studled.Items[i].Text);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(chkl_studled.Items[i].Value);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                        }
                                    }
                                }
                            }
                        }
                        if (count != 0)
                        {
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 1, count);
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Paid";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                        }

                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total Paid";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);

                        #endregion

                        #region value
                        double totamount = 0;
                        double fnlamount = 0;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            fpheight += 50;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread1.Sheets[0].RowCount);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["Roll_No"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = rollno;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["Reg_No"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = regno;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["Narration"]);
                            if (ddlacctype.SelectedItem.Value == "1")
                            {
                            }
                            else if (ddlacctype.SelectedItem.Value == "2")
                            {
                                if (chkl_studhed.Items.Count > 0)
                                {
                                    for (int k = 0; k < chkl_studhed.Items.Count; k++)
                                    {
                                        if (chkl_studhed.Items[k].Selected == true)
                                        {
                                            output.Text = "Student-->Header";
                                            ds.Tables[1].DefaultView.RowFilter = "HeaderFK='" + Convert.ToString(chkl_studhed.Items[k].Value) + "' and App_no='" + Convert.ToString(ds.Tables[0].Rows[i]["App_no"]) + "'";
                                            dv = ds.Tables[1].DefaultView;
                                            int countval = Convert.ToInt32(hashval[(Convert.ToString(chkl_studhed.Items[k].Value))]);
                                            if (dv.Count == 0 || dv.Count == null)
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, countval].Text = "-";
                                            }
                                            else
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, countval].Text = Convert.ToString(dv[0]["Credit"]);
                                                totamount = Convert.ToDouble(dv[0]["Credit"]);
                                                if (totamount != 0)
                                                {
                                                    fnlamount = fnlamount + totamount;
                                                }
                                            }
                                        }
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(fnlamount);
                                    fnlamount = 0;
                                }
                            }
                            else if (ddlacctype.SelectedItem.Value == "3")
                            {
                                if (chkl_studled.Items.Count > 0)
                                {
                                    for (int k = 0; k < chkl_studled.Items.Count; k++)
                                    {
                                        if (chkl_studled.Items[k].Selected == true)
                                        {
                                            output.Text = "Student-->Ledger";
                                            ds.Tables[1].DefaultView.RowFilter = "LedgerFK='" + Convert.ToString(chkl_studled.Items[k].Value) + "' and App_no='" + Convert.ToString(ds.Tables[0].Rows[i]["App_no"]) + "'";
                                            dv = ds.Tables[1].DefaultView;
                                            int countval = Convert.ToInt32(hashval[(Convert.ToString(chkl_studled.Items[k].Value))]);
                                            if (dv.Count == 0 || dv.Count == null)
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, countval].Text = "-";
                                            }
                                            else
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, countval].Text = Convert.ToString(dv[0]["Credit"]);
                                                totamount = Convert.ToDouble(dv[0]["Credit"]);
                                                if (totamount != 0)
                                                {
                                                    fnlamount = fnlamount + totamount;
                                                }
                                            }
                                        }
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(fnlamount);
                                    fnlamount = 0;
                                }
                            }

                        }

                        #endregion

                        #region grandtot
                        FpSpread1.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].Rows.Count - 1, 0, 1, 6);
                        double hedval = 0;
                        for (int j = 6; j < FpSpread1.Sheets[0].Columns.Count; j++)
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

                    }

                    #region visible
                    FpSpread1.Width = 1300;
                    FpSpread1.Height = Convert.ToInt32(fpheight);
                    FpSpread1.ShowHeaderSelection = false;
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    FpSpread1.SaveChanges();
                    output.Visible = true;
                    print.Visible = true;
                    //  divspread.Visible = true;
                    FpSpread1.Visible = true;
                    #endregion
                }
                else
                {
                    FpSpread1.Visible = false;
                    print.Visible = false;
                    pupdiv.Visible = true;
                    pupdiv1.Visible = true;
                    lbl_alert.Visible = true;
                    output.Text = "";
                    lbl_alert.Text = "No Record Found";
                }
            }
        }
        catch { }
    }

    #endregion

    #region staff method

    public void staffvalues(object sender, EventArgs e)
    {
        try
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (chkcumul.Checked == false)
                    {
                        #region design
                        int count = 0;
                        FpSpread1.Sheets[0].RowCount = 0;
                        FpSpread1.Sheets[0].ColumnCount = 0;
                        FpSpread1.CommandBar.Visible = false;
                        FpSpread1.Sheets[0].AutoPostBack = true;
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                        FpSpread1.Sheets[0].RowHeader.Visible = false;
                        FpSpread1.Sheets[0].ColumnCount = 8;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                        FarPoint.Web.Spread.TextCellType regno = new FarPoint.Web.Spread.TextCellType();
                        FarPoint.Web.Spread.TextCellType rollno = new FarPoint.Web.Spread.TextCellType();

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Code";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[1].Visible = true;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Name";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        // FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Right;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Department ";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Designation";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Voucher No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Voucher Date";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Narration";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Left;


                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);
                        if (ddlacctype.Items.Count > 0)
                        {
                            if (ddlacctype.SelectedItem.Value == "2")
                            {
                                if (chkl_studhed.Items.Count > 0)
                                {
                                    for (int i = 0; i < chkl_studhed.Items.Count; i++)
                                    {
                                        if (chkl_studhed.Items[i].Selected == true)
                                        {
                                            count++;
                                            FpSpread1.Sheets[0].ColumnCount++;
                                            hashval.Add(Convert.ToString(chkl_studhed.Items[i].Value), FpSpread1.Sheets[0].ColumnCount - 1);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(chkl_studhed.Items[i].Text);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(chkl_studhed.Items[i].Value);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                        }
                                    }
                                }
                            }
                            else if (ddlacctype.SelectedItem.Value == "3")
                            {
                                if (chkl_studled.Items.Count > 0)
                                {
                                    for (int i = 0; i < chkl_studled.Items.Count; i++)
                                    {
                                        if (chkl_studled.Items[i].Selected == true)
                                        {
                                            count++;
                                            FpSpread1.Sheets[0].ColumnCount++;
                                            hashval.Add(Convert.ToString(chkl_studled.Items[i].Value), FpSpread1.Sheets[0].ColumnCount - 1);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(chkl_studled.Items[i].Text);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(chkl_studled.Items[i].Value);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                        }
                                    }
                                }
                            }
                        }
                        if (count != 0)
                        {
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 1, count);
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Paid";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].ForeColor = ColorTranslator.FromHtml("#000000");
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                        }

                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total Paid";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);

                        #endregion

                        #region value
                        double totamount = 0;
                        double fnlamount = 0;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            fpheight += 25;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread1.Sheets[0].RowCount);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["staff_code"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = rollno;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["staff_name"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = regno;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["dept_name"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["desig_name"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["TransCode"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["TransDate"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[i]["Narration"]);

                            if (ddlacctype.SelectedItem.Value == "1")
                            {
                            }
                            else if (ddlacctype.SelectedItem.Value == "2")
                            {
                                if (chkl_studhed.Items.Count > 0)
                                {
                                    for (int k = 0; k < chkl_studhed.Items.Count; k++)
                                    {
                                        if (chkl_studhed.Items[k].Selected == true)
                                        {
                                            output.Text = "Staff-->Header";
                                            ds.Tables[0].DefaultView.RowFilter = "HeaderFK='" + Convert.ToString(chkl_studhed.Items[k].Value) + "' and App_no='" + Convert.ToString(ds.Tables[0].Rows[i]["App_no"]) + "' and TransCode='" + Convert.ToString(ds.Tables[0].Rows[i]["TransCode"]) + "'";
                                            dv = ds.Tables[0].DefaultView;
                                            int countval = Convert.ToInt32(hashval[(Convert.ToString(chkl_studhed.Items[k].Value))]);
                                            if (dv.Count == 0 || dv.Count == null)
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, countval].Text = "-";
                                            }
                                            else
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, countval].Text = Convert.ToString(dv[0]["Credit"]);
                                                totamount = Convert.ToDouble(dv[0]["Credit"]);
                                                if (totamount != 0)
                                                {
                                                    fnlamount = fnlamount + totamount;
                                                }
                                            }
                                        }
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(fnlamount);
                                    fnlamount = 0;
                                }
                            }
                            else if (ddlacctype.SelectedItem.Value == "3")
                            {
                                if (chkl_studled.Items.Count > 0)
                                {
                                    for (int k = 0; k < chkl_studled.Items.Count; k++)
                                    {
                                        if (chkl_studled.Items[k].Selected == true)
                                        {
                                            output.Text = "Staff-->Ledger";
                                            ds.Tables[0].DefaultView.RowFilter = "LedgerFK='" + Convert.ToString(chkl_studled.Items[k].Value) + "' and App_no='" + Convert.ToString(ds.Tables[0].Rows[i]["App_no"]) + "' and TransCode='" + Convert.ToString(ds.Tables[0].Rows[i]["TransCode"]) + "'";
                                            dv = ds.Tables[0].DefaultView;
                                            int countval = Convert.ToInt32(hashval[(Convert.ToString(chkl_studled.Items[k].Value))]);
                                            if (dv.Count == 0 || dv.Count == null)
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, countval].Text = "-";
                                            }
                                            else
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, countval].Text = Convert.ToString(dv[0]["Credit"]);
                                                totamount = Convert.ToDouble(dv[0]["Credit"]);
                                                if (totamount != 0)
                                                {
                                                    fnlamount = fnlamount + totamount;
                                                }
                                            }
                                        }
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(fnlamount);
                                    fnlamount = 0;
                                }
                            }

                        }

                        #endregion

                        #region grandtot
                        FpSpread1.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].Rows.Count - 1, 0, 1, 8);
                        double hedval = 0;
                        for (int j = 8; j < FpSpread1.Sheets[0].Columns.Count; j++)
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
                    }
                    else
                    {
                        #region design
                        int count = 0;
                        FpSpread1.Sheets[0].RowCount = 0;
                        FpSpread1.Sheets[0].ColumnCount = 0;
                        FpSpread1.CommandBar.Visible = false;
                        FpSpread1.Sheets[0].AutoPostBack = true;
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                        FpSpread1.Sheets[0].RowHeader.Visible = false;
                        FpSpread1.Sheets[0].ColumnCount = 6;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                        FarPoint.Web.Spread.TextCellType regno = new FarPoint.Web.Spread.TextCellType();
                        FarPoint.Web.Spread.TextCellType rollno = new FarPoint.Web.Spread.TextCellType();

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Code";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[1].Visible = true;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Name";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        // FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Right;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Department ";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Designation";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Narration";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;


                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                        if (ddlacctype.Items.Count > 0)
                        {
                            if (ddlacctype.SelectedItem.Value == "2")
                            {
                                if (chkl_studhed.Items.Count > 0)
                                {
                                    for (int i = 0; i < chkl_studhed.Items.Count; i++)
                                    {
                                        if (chkl_studhed.Items[i].Selected == true)
                                        {
                                            count++;
                                            FpSpread1.Sheets[0].ColumnCount++;
                                            hashval.Add(Convert.ToString(chkl_studhed.Items[i].Value), FpSpread1.Sheets[0].ColumnCount - 1);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(chkl_studhed.Items[i].Text);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(chkl_studhed.Items[i].Value);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                        }
                                    }
                                }
                            }
                            else if (ddlacctype.SelectedItem.Value == "3")
                            {
                                if (chkl_studled.Items.Count > 0)
                                {
                                    for (int i = 0; i < chkl_studled.Items.Count; i++)
                                    {
                                        if (chkl_studled.Items[i].Selected == true)
                                        {
                                            count++;
                                            FpSpread1.Sheets[0].ColumnCount++;
                                            hashval.Add(Convert.ToString(chkl_studled.Items[i].Value), FpSpread1.Sheets[0].ColumnCount - 1);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(chkl_studled.Items[i].Text);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(chkl_studled.Items[i].Value);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                        }
                                    }
                                }
                            }
                        }
                        if (count != 0)
                        {
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 1, count);
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Paid";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                        }

                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total Paid";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);

                        #endregion

                        #region value
                        double totamount = 0;
                        double fnlamount = 0;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            fpheight += 25;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread1.Sheets[0].RowCount);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["staff_code"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = rollno;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["staff_name"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = regno;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["dept_name"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["desig_name"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["Narration"]);


                            if (ddlacctype.SelectedItem.Value == "1")
                            {
                            }
                            else if (ddlacctype.SelectedItem.Value == "2")
                            {
                                if (chkl_studhed.Items.Count > 0)
                                {
                                    for (int k = 0; k < chkl_studhed.Items.Count; k++)
                                    {
                                        if (chkl_studhed.Items[k].Selected == true)
                                        {
                                            output.Text = "Staff-->Header";
                                            ds.Tables[1].DefaultView.RowFilter = "HeaderFK='" + Convert.ToString(chkl_studhed.Items[k].Value) + "' and App_no='" + Convert.ToString(ds.Tables[0].Rows[i]["App_no"]) + "'";
                                            dv = ds.Tables[1].DefaultView;
                                            int countval = Convert.ToInt32(hashval[(Convert.ToString(chkl_studhed.Items[k].Value))]);
                                            if (dv.Count == 0 || dv.Count == null)
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, countval].Text = "-";
                                            }
                                            else
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, countval].Text = Convert.ToString(dv[0]["Credit"]);
                                                totamount = Convert.ToDouble(dv[0]["Credit"]);
                                                if (totamount != 0)
                                                {
                                                    fnlamount = fnlamount + totamount;
                                                }
                                            }
                                        }
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(fnlamount);
                                    fnlamount = 0;
                                }
                            }
                            else if (ddlacctype.SelectedItem.Value == "3")
                            {
                                if (chkl_studled.Items.Count > 0)
                                {
                                    for (int k = 0; k < chkl_studled.Items.Count; k++)
                                    {
                                        if (chkl_studled.Items[k].Selected == true)
                                        {
                                            output.Text = "Staff-->Ledger";
                                            ds.Tables[1].DefaultView.RowFilter = "LedgerFK='" + Convert.ToString(chkl_studled.Items[k].Value) + "' and App_no='" + Convert.ToString(ds.Tables[0].Rows[i]["App_no"]) + "'";
                                            dv = ds.Tables[1].DefaultView;
                                            int countval = Convert.ToInt32(hashval[(Convert.ToString(chkl_studled.Items[k].Value))]);
                                            if (dv.Count == 0 || dv.Count == null)
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, countval].Text = "-";
                                            }
                                            else
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, countval].Text = Convert.ToString(dv[0]["Credit"]);
                                                totamount = Convert.ToDouble(dv[0]["Credit"]);
                                                if (totamount != 0)
                                                {
                                                    fnlamount = fnlamount + totamount;
                                                }
                                            }
                                        }
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(fnlamount);
                                    fnlamount = 0;
                                }
                            }

                        }

                        #endregion

                        #region grandtot
                        FpSpread1.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].Rows.Count - 1, 0, 1, 6);
                        double hedval = 0;
                        for (int j = 6; j < FpSpread1.Sheets[0].Columns.Count; j++)
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
                    }

                    #region visible
                    FpSpread1.Width = 1300;
                    FpSpread1.Height = Convert.ToInt32(fpheight);
                    FpSpread1.ShowHeaderSelection = false;
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    FpSpread1.SaveChanges();
                    output.Visible = true;
                    print.Visible = true;
                    //  divspread.Visible = true;
                    FpSpread1.Visible = true;
                    #endregion
                }
                else
                {
                    FpSpread1.Visible = false;
                    print.Visible = false;
                    pupdiv.Visible = true;
                    pupdiv1.Visible = true;
                    lbl_alert.Visible = true;
                    output.Text = "";
                    lbl_alert.Text = "No Record Found";
                }
            }
        }
        catch { }
    }

    #endregion

    #region vendor method

    public void vendorvalues(object sender, EventArgs e)
    {
        try
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (chkcumul.Checked == false)
                    {
                        #region design
                        int count = 0;
                        FpSpread1.Sheets[0].RowCount = 0;
                        FpSpread1.Sheets[0].ColumnCount = 0;
                        FpSpread1.CommandBar.Visible = false;
                        FpSpread1.Sheets[0].AutoPostBack = true;
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                        FpSpread1.Sheets[0].RowHeader.Visible = false;
                        FpSpread1.Sheets[0].ColumnCount = 7;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                        FarPoint.Web.Spread.TextCellType regno = new FarPoint.Web.Spread.TextCellType();
                        FarPoint.Web.Spread.TextCellType rollno = new FarPoint.Web.Spread.TextCellType();

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Vendor Code";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Columns[1].Visible = true;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Vendor Company Name";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        // FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Right;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Vendor Contact Name";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Voucher No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Voucher No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Narration";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Left;


                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
                        if (ddlacctype.Items.Count > 0)
                        {
                            if (ddlacctype.SelectedItem.Value == "2")
                            {
                                if (chkl_studhed.Items.Count > 0)
                                {
                                    for (int i = 0; i < chkl_studhed.Items.Count; i++)
                                    {
                                        if (chkl_studhed.Items[i].Selected == true)
                                        {
                                            count++;
                                            FpSpread1.Sheets[0].ColumnCount++;
                                            hashval.Add(Convert.ToString(chkl_studhed.Items[i].Value), FpSpread1.Sheets[0].ColumnCount - 1);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(chkl_studhed.Items[i].Text);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(chkl_studhed.Items[i].Value);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                        }
                                    }
                                }
                            }
                            else if (ddlacctype.SelectedItem.Value == "3")
                            {
                                if (chkl_studled.Items.Count > 0)
                                {
                                    for (int i = 0; i < chkl_studled.Items.Count; i++)
                                    {
                                        if (chkl_studled.Items[i].Selected == true)
                                        {
                                            count++;
                                            FpSpread1.Sheets[0].ColumnCount++;
                                            hashval.Add(Convert.ToString(chkl_studled.Items[i].Value), FpSpread1.Sheets[0].ColumnCount - 1);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(chkl_studled.Items[i].Text);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(chkl_studled.Items[i].Value);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                        }
                                    }
                                }
                            }
                        }
                        if (count != 0)
                        {
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 1, count);
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Paid";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].ForeColor = ColorTranslator.FromHtml("#000000");
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                        }

                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total Paid";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);

                        #endregion

                        #region value
                        double totamount = 0;
                        double fnlamount = 0;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            fpheight += 25;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread1.Sheets[0].RowCount);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorCode"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = rollno;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorCompName"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = regno;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["VenContactName"]);

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["TransCode"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["TransDate"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["Narration"]);
                            if (ddlacctype.SelectedItem.Value == "1")
                            {
                            }
                            else if (ddlacctype.SelectedItem.Value == "2")
                            {
                                if (chkl_studhed.Items.Count > 0)
                                {
                                    for (int k = 0; k < chkl_studhed.Items.Count; k++)
                                    {
                                        if (chkl_studhed.Items[k].Selected == true)
                                        {
                                            output.Text = "Vendor-->Header";
                                            ds.Tables[0].DefaultView.RowFilter = "HeaderFK='" + Convert.ToString(chkl_studhed.Items[k].Value) + "' and App_no='" + Convert.ToString(ds.Tables[0].Rows[i]["App_no"]) + "' and VendorContactPK='" + Convert.ToString(ds.Tables[0].Rows[i]["VendorContactPK"]) + "' and TransCode='" + Convert.ToString(ds.Tables[0].Rows[i]["TransCode"]) + "'";
                                            dv = ds.Tables[0].DefaultView;
                                            int countval = Convert.ToInt32(hashval[(Convert.ToString(chkl_studhed.Items[k].Value))]);
                                            if (dv.Count == 0 || dv.Count == null)
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, countval].Text = "-";
                                            }
                                            else
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, countval].Text = Convert.ToString(dv[0]["Credit"]);
                                                totamount = Convert.ToDouble(dv[0]["Credit"]);
                                                if (totamount != 0)
                                                {
                                                    fnlamount = fnlamount + totamount;
                                                }
                                            }
                                        }
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(fnlamount);
                                    fnlamount = 0;
                                }
                            }
                            else if (ddlacctype.SelectedItem.Value == "3")
                            {
                                if (chkl_studled.Items.Count > 0)
                                {
                                    for (int k = 0; k < chkl_studled.Items.Count; k++)
                                    {
                                        if (chkl_studled.Items[k].Selected == true)
                                        {
                                            output.Text = "Vendor-->Ledger";
                                            ds.Tables[0].DefaultView.RowFilter = "LedgerFK='" + Convert.ToString(chkl_studled.Items[k].Value) + "' and App_no='" + Convert.ToString(ds.Tables[0].Rows[i]["App_no"]) + "'  and VendorContactPK='" + Convert.ToString(ds.Tables[0].Rows[i]["VendorContactPK"]) + "' and TransCode='" + Convert.ToString(ds.Tables[0].Rows[i]["TransCode"]) + "'";
                                            dv = ds.Tables[0].DefaultView;
                                            int countval = Convert.ToInt32(hashval[(Convert.ToString(chkl_studled.Items[k].Value))]);
                                            if (dv.Count == 0 || dv.Count == null)
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, countval].Text = "-";
                                            }
                                            else
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, countval].Text = Convert.ToString(dv[0]["Credit"]);
                                                totamount = Convert.ToDouble(dv[0]["Credit"]);
                                                if (totamount != 0)
                                                {
                                                    fnlamount = fnlamount + totamount;
                                                }
                                            }
                                        }
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(fnlamount);
                                    fnlamount = 0;
                                }
                            }

                        }

                        #endregion

                        #region grandtot
                        FpSpread1.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].Rows.Count - 1, 0, 1, 7);
                        double hedval = 0;
                        for (int j = 7; j < FpSpread1.Sheets[0].Columns.Count; j++)
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
                    }
                    else
                    {
                        #region design
                        int count = 0;
                        FpSpread1.Sheets[0].RowCount = 0;
                        FpSpread1.Sheets[0].ColumnCount = 0;
                        FpSpread1.CommandBar.Visible = false;
                        FpSpread1.Sheets[0].AutoPostBack = true;
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                        FpSpread1.Sheets[0].RowHeader.Visible = false;
                        FpSpread1.Sheets[0].ColumnCount = 5;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                        FarPoint.Web.Spread.TextCellType regno = new FarPoint.Web.Spread.TextCellType();
                        FarPoint.Web.Spread.TextCellType rollno = new FarPoint.Web.Spread.TextCellType();

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Vendor Code";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Columns[1].Visible = true;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Vendor Company Name";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        // FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Right;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Vendor Contact Name";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Narration";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;


                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);

                        if (ddlacctype.Items.Count > 0)
                        {
                            if (ddlacctype.SelectedItem.Value == "2")
                            {
                                if (chkl_studhed.Items.Count > 0)
                                {
                                    for (int i = 0; i < chkl_studhed.Items.Count; i++)
                                    {
                                        if (chkl_studhed.Items[i].Selected == true)
                                        {
                                            count++;
                                            FpSpread1.Sheets[0].ColumnCount++;
                                            hashval.Add(Convert.ToString(chkl_studhed.Items[i].Value), FpSpread1.Sheets[0].ColumnCount - 1);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(chkl_studhed.Items[i].Text);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(chkl_studhed.Items[i].Value);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                        }
                                    }
                                }
                            }
                            else if (ddlacctype.SelectedItem.Value == "3")
                            {
                                if (chkl_studled.Items.Count > 0)
                                {
                                    for (int i = 0; i < chkl_studled.Items.Count; i++)
                                    {
                                        if (chkl_studled.Items[i].Selected == true)
                                        {
                                            count++;
                                            FpSpread1.Sheets[0].ColumnCount++;
                                            hashval.Add(Convert.ToString(chkl_studled.Items[i].Value), FpSpread1.Sheets[0].ColumnCount - 1);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(chkl_studled.Items[i].Text);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(chkl_studled.Items[i].Value);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                        }
                                    }
                                }
                            }
                        }
                        if (count != 0)
                        {
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 1, count);
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Paid";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                        }

                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total Paid";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);

                        #endregion

                        #region value
                        double totamount = 0;
                        double fnlamount = 0;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            fpheight += 25;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread1.Sheets[0].RowCount);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorCode"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = rollno;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorCompName"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = regno;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["VenContactName"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["Narration"]);

                            if (ddlacctype.SelectedItem.Value == "1")
                            {
                            }
                            else if (ddlacctype.SelectedItem.Value == "2")
                            {
                                if (chkl_studhed.Items.Count > 0)
                                {
                                    for (int k = 0; k < chkl_studhed.Items.Count; k++)
                                    {
                                        if (chkl_studhed.Items[k].Selected == true)
                                        {
                                            output.Text = "Vendor-->Header";
                                            ds.Tables[1].DefaultView.RowFilter = "HeaderFK='" + Convert.ToString(chkl_studhed.Items[k].Value) + "' and App_no='" + Convert.ToString(ds.Tables[0].Rows[i]["App_no"]) + "' and VendorContactPK='" + Convert.ToString(ds.Tables[0].Rows[i]["VendorContactPK"]) + "'";
                                            dv = ds.Tables[1].DefaultView;
                                            int countval = Convert.ToInt32(hashval[(Convert.ToString(chkl_studhed.Items[k].Value))]);
                                            if (dv.Count == 0 || dv.Count == null)
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, countval].Text = "-";
                                            }
                                            else
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, countval].Text = Convert.ToString(dv[0]["Credit"]);
                                                totamount = Convert.ToDouble(dv[0]["Credit"]);
                                                if (totamount != 0)
                                                {
                                                    fnlamount = fnlamount + totamount;
                                                }
                                            }
                                        }
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(fnlamount);
                                    fnlamount = 0;
                                }
                            }
                            else if (ddlacctype.SelectedItem.Value == "3")
                            {
                                if (chkl_studled.Items.Count > 0)
                                {
                                    for (int k = 0; k < chkl_studled.Items.Count; k++)
                                    {
                                        if (chkl_studled.Items[k].Selected == true)
                                        {
                                            output.Text = "Vendor-->Ledger";
                                            ds.Tables[1].DefaultView.RowFilter = "LedgerFK='" + Convert.ToString(chkl_studled.Items[k].Value) + "' and App_no='" + Convert.ToString(ds.Tables[0].Rows[i]["App_no"]) + "'  and VendorContactPK='" + Convert.ToString(ds.Tables[0].Rows[i]["VendorContactPK"]) + "'";
                                            dv = ds.Tables[1].DefaultView;
                                            int countval = Convert.ToInt32(hashval[(Convert.ToString(chkl_studled.Items[k].Value))]);
                                            if (dv.Count == 0 || dv.Count == null)
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, countval].Text = "-";
                                            }
                                            else
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, countval].Text = Convert.ToString(dv[0]["Credit"]);
                                                totamount = Convert.ToDouble(dv[0]["Credit"]);
                                                if (totamount != 0)
                                                {
                                                    fnlamount = fnlamount + totamount;
                                                }
                                            }
                                        }
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(fnlamount);
                                    fnlamount = 0;
                                }
                            }

                        }

                        #endregion

                        #region grandtot
                        FpSpread1.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].Rows.Count - 1, 0, 1, 5);
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
                    }

                    #region visible
                    FpSpread1.Width = 1300;
                    FpSpread1.Height = Convert.ToInt32(fpheight);
                    FpSpread1.ShowHeaderSelection = false;
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    FpSpread1.SaveChanges();
                    output.Visible = true;
                    print.Visible = true;
                    //  divspread.Visible = true;
                    FpSpread1.Visible = true;
                    #endregion
                }
                else
                {
                    FpSpread1.Visible = false;
                    print.Visible = false;
                    pupdiv.Visible = true;
                    pupdiv1.Visible = true;
                    lbl_alert.Visible = true;
                    output.Text = "";
                    lbl_alert.Text = "No Record Found";
                }
            }
        }
        catch { }
    }

    #endregion

    #region other method

    public void othervalues(object sender, EventArgs e)
    {
        try
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (chkcumul.Checked == false)
                    {
                        #region design
                        int count = 0;
                        FpSpread1.Sheets[0].RowCount = 0;
                        FpSpread1.Sheets[0].ColumnCount = 0;
                        FpSpread1.CommandBar.Visible = false;
                        FpSpread1.Sheets[0].AutoPostBack = true;
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                        FpSpread1.Sheets[0].RowHeader.Visible = false;
                        FpSpread1.Sheets[0].ColumnCount = 6;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                        FarPoint.Web.Spread.TextCellType regno = new FarPoint.Web.Spread.TextCellType();
                        FarPoint.Web.Spread.TextCellType rollno = new FarPoint.Web.Spread.TextCellType();

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Vendor Code";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Columns[1].Visible = true;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Vendor Company Name";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        // FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Right;

                        //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Vendor Contact Name";
                        //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
                        //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        //FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Voucher No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Voucher No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Narration";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;


                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                        // FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                        if (ddlacctype.Items.Count > 0)
                        {
                            if (ddlacctype.SelectedItem.Value == "2")
                            {
                                if (chkl_studhed.Items.Count > 0)
                                {
                                    for (int i = 0; i < chkl_studhed.Items.Count; i++)
                                    {
                                        if (chkl_studhed.Items[i].Selected == true)
                                        {
                                            count++;
                                            FpSpread1.Sheets[0].ColumnCount++;
                                            hashval.Add(Convert.ToString(chkl_studhed.Items[i].Value), FpSpread1.Sheets[0].ColumnCount - 1);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(chkl_studhed.Items[i].Text);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(chkl_studhed.Items[i].Value);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                        }
                                    }
                                }
                            }
                            else if (ddlacctype.SelectedItem.Value == "3")
                            {
                                if (chkl_studled.Items.Count > 0)
                                {
                                    for (int i = 0; i < chkl_studled.Items.Count; i++)
                                    {
                                        if (chkl_studled.Items[i].Selected == true)
                                        {
                                            count++;
                                            FpSpread1.Sheets[0].ColumnCount++;
                                            hashval.Add(Convert.ToString(chkl_studled.Items[i].Value), FpSpread1.Sheets[0].ColumnCount - 1);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(chkl_studled.Items[i].Text);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(chkl_studled.Items[i].Value);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                        }
                                    }
                                }
                            }
                        }
                        if (count != 0)
                        {
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 1, count);
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Paid";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                        }

                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total Paid";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);

                        #endregion

                        #region value
                        double totamount = 0;
                        double fnlamount = 0;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            fpheight += 25;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread1.Sheets[0].RowCount);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorCode"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = rollno;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorCompName"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = regno;
                            //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["VenContactName"]);

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["TransCode"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["TransDate"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["Narration"]);
                            if (ddlacctype.SelectedItem.Value == "1")
                            {
                            }
                            else if (ddlacctype.SelectedItem.Value == "2")
                            {
                                if (chkl_studhed.Items.Count > 0)
                                {
                                    for (int k = 0; k < chkl_studhed.Items.Count; k++)
                                    {
                                        if (chkl_studhed.Items[k].Selected == true)
                                        {
                                            output.Text = "Other-->Header";
                                            ds.Tables[0].DefaultView.RowFilter = "HeaderFK='" + Convert.ToString(chkl_studhed.Items[k].Value) + "' and App_no='" + Convert.ToString(ds.Tables[0].Rows[i]["App_no"]) + "' and TransCode='" + Convert.ToString(ds.Tables[0].Rows[i]["TransCode"]) + "' ";
                                            dv = ds.Tables[0].DefaultView;
                                            int countval = Convert.ToInt32(hashval[(Convert.ToString(chkl_studhed.Items[k].Value))]);
                                            if (dv.Count == 0 || dv.Count == null)
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, countval].Text = "-";
                                            }
                                            else
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, countval].Text = Convert.ToString(dv[0]["Credit"]);
                                                totamount = Convert.ToDouble(dv[0]["Credit"]);
                                                if (totamount != 0)
                                                {
                                                    fnlamount = fnlamount + totamount;
                                                }
                                            }
                                        }
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(fnlamount);
                                    fnlamount = 0;
                                }
                            }
                            else if (ddlacctype.SelectedItem.Value == "3")
                            {
                                if (chkl_studled.Items.Count > 0)
                                {
                                    for (int k = 0; k < chkl_studled.Items.Count; k++)
                                    {
                                        if (chkl_studled.Items[k].Selected == true)
                                        {
                                            output.Text = "Other-->Ledger";
                                            ds.Tables[0].DefaultView.RowFilter = "LedgerFK='" + Convert.ToString(chkl_studled.Items[k].Value) + "' and App_no='" + Convert.ToString(ds.Tables[0].Rows[i]["App_no"]) + "' and TransCode='" + Convert.ToString(ds.Tables[0].Rows[i]["TransCode"]) + "'";
                                            dv = ds.Tables[0].DefaultView;
                                            int countval = Convert.ToInt32(hashval[(Convert.ToString(chkl_studled.Items[k].Value))]);
                                            if (dv.Count == 0 || dv.Count == null)
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, countval].Text = "-";
                                            }
                                            else
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, countval].Text = Convert.ToString(dv[0]["Credit"]);
                                                totamount = Convert.ToDouble(dv[0]["Credit"]);
                                                if (totamount != 0)
                                                {
                                                    fnlamount = fnlamount + totamount;
                                                }
                                            }
                                        }
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(fnlamount);
                                    fnlamount = 0;
                                }
                            }

                        }

                        #endregion

                        #region grandtot
                        FpSpread1.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].Rows.Count - 1, 0, 1, 6);
                        double hedval = 0;
                        for (int j = 6; j < FpSpread1.Sheets[0].Columns.Count; j++)
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
                    }
                    else
                    {

                        #region design
                        int count = 0;
                        FpSpread1.Sheets[0].RowCount = 0;
                        FpSpread1.Sheets[0].ColumnCount = 0;
                        FpSpread1.CommandBar.Visible = false;
                        FpSpread1.Sheets[0].AutoPostBack = true;
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                        FpSpread1.Sheets[0].RowHeader.Visible = false;
                        FpSpread1.Sheets[0].ColumnCount = 5;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.White;
                        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                        FarPoint.Web.Spread.TextCellType regno = new FarPoint.Web.Spread.TextCellType();
                        FarPoint.Web.Spread.TextCellType rollno = new FarPoint.Web.Spread.TextCellType();

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Vendor Code";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Columns[1].Visible = true;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Vendor Company Name";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        // FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Right;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Vendor Company Name";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Vendor Company Name";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                        if (ddlacctype.Items.Count > 0)
                        {
                            if (ddlacctype.SelectedItem.Value == "2")
                            {
                                if (chkl_studhed.Items.Count > 0)
                                {
                                    for (int i = 0; i < chkl_studhed.Items.Count; i++)
                                    {
                                        if (chkl_studhed.Items[i].Selected == true)
                                        {
                                            count++;
                                            FpSpread1.Sheets[0].ColumnCount++;
                                            hashval.Add(Convert.ToString(chkl_studhed.Items[i].Value), FpSpread1.Sheets[0].ColumnCount - 1);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(chkl_studhed.Items[i].Text);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(chkl_studhed.Items[i].Value);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                        }
                                    }
                                }
                            }
                            else if (ddlacctype.SelectedItem.Value == "3")
                            {
                                if (chkl_studled.Items.Count > 0)
                                {
                                    for (int i = 0; i < chkl_studled.Items.Count; i++)
                                    {
                                        if (chkl_studled.Items[i].Selected == true)
                                        {
                                            count++;
                                            FpSpread1.Sheets[0].ColumnCount++;
                                            hashval.Add(Convert.ToString(chkl_studled.Items[i].Value), FpSpread1.Sheets[0].ColumnCount - 1);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(chkl_studled.Items[i].Text);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(chkl_studled.Items[i].Value);
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                                        }
                                    }
                                }
                            }
                        }
                        if (count != 0)
                        {
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 1, count);
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Paid";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                        }

                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total Paid";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);

                        #endregion

                        #region value
                        double totamount = 0;
                        double fnlamount = 0;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            fpheight += 25;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread1.Sheets[0].RowCount);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorCode"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = rollno;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorCompName"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = regno;
                            //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["VenContactName"]);   
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["Narration"]);
                            if (ddlacctype.SelectedItem.Value == "1")
                            {
                            }
                            else if (ddlacctype.SelectedItem.Value == "2")
                            {
                                if (chkl_studhed.Items.Count > 0)
                                {
                                    for (int k = 0; k < chkl_studhed.Items.Count; k++)
                                    {
                                        if (chkl_studhed.Items[k].Selected == true)
                                        {
                                            output.Text = "Other-->Header";
                                            ds.Tables[1].DefaultView.RowFilter = "HeaderFK='" + Convert.ToString(chkl_studhed.Items[k].Value) + "' and App_no='" + Convert.ToString(ds.Tables[0].Rows[i]["App_no"]) + "'";
                                            dv = ds.Tables[1].DefaultView;
                                            int countval = Convert.ToInt32(hashval[(Convert.ToString(chkl_studhed.Items[k].Value))]);
                                            if (dv.Count == 0 || dv.Count == null)
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, countval].Text = "-";
                                            }
                                            else
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, countval].Text = Convert.ToString(dv[0]["Credit"]);
                                                totamount = Convert.ToDouble(dv[0]["Credit"]);
                                                if (totamount != 0)
                                                {
                                                    fnlamount = fnlamount + totamount;
                                                }
                                            }
                                        }
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(fnlamount);
                                    fnlamount = 0;
                                }
                            }
                            else if (ddlacctype.SelectedItem.Value == "3")
                            {
                                if (chkl_studled.Items.Count > 0)
                                {
                                    for (int k = 0; k < chkl_studled.Items.Count; k++)
                                    {
                                        if (chkl_studled.Items[k].Selected == true)
                                        {
                                            output.Text = "Other-->Ledger";
                                            ds.Tables[1].DefaultView.RowFilter = "LedgerFK='" + Convert.ToString(chkl_studled.Items[k].Value) + "' and App_no='" + Convert.ToString(ds.Tables[0].Rows[i]["App_no"]) + "'";
                                            dv = ds.Tables[1].DefaultView;
                                            int countval = Convert.ToInt32(hashval[(Convert.ToString(chkl_studled.Items[k].Value))]);
                                            if (dv.Count == 0 || dv.Count == null)
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, countval].Text = "-";
                                            }
                                            else
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, countval].Text = Convert.ToString(dv[0]["Credit"]);
                                                totamount = Convert.ToDouble(dv[0]["Credit"]);
                                                if (totamount != 0)
                                                {
                                                    fnlamount = fnlamount + totamount;
                                                }
                                            }
                                        }
                                    }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(fnlamount);
                                    fnlamount = 0;
                                }
                            }

                        }

                        #endregion

                        #region grandtot
                        FpSpread1.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].Rows.Count - 1, 0, 1, 6);
                        double hedval = 0;
                        for (int j = 4; j < FpSpread1.Sheets[0].Columns.Count; j++)
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
                    }

                    #region visible
                    FpSpread1.Width = 1300;
                    FpSpread1.Height = Convert.ToInt32(fpheight);
                    FpSpread1.ShowHeaderSelection = false;
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    FpSpread1.SaveChanges();
                    output.Visible = true;
                    print.Visible = true;
                    //  divspread.Visible = true;
                    FpSpread1.Visible = true;
                    #endregion
                }
                else
                {
                    FpSpread1.Visible = false;
                    print.Visible = false;
                    pupdiv.Visible = true;
                    pupdiv1.Visible = true;
                    lbl_alert.Visible = true;
                    output.Visible = false;
                    lbl_alert.Text = "No Record Found";
                }
            }
        }
        catch { }
    }

    #endregion

    #endregion


    #region print control

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
                if (rbstud.Checked == true)
                {
                    lblvalidation1.Text = "Please Enter Your Student Report Name";
                    lblvalidation1.Visible = true;
                    txtexcelname.Focus();
                }
                if (rbstaff.Checked == true)
                {
                    lblvalidation1.Text = "Please Enter Your Staff Report Name";
                    lblvalidation1.Visible = true;
                    txtexcelname.Focus();
                }
                if (rbvendor.Checked == true)
                {
                    lblvalidation1.Text = "Please Enter Your Vendor Report Name";
                    lblvalidation1.Visible = true;
                    txtexcelname.Focus();
                }
                if (rbother.Checked == true)
                {
                    lblvalidation1.Text = "Please Enter Your Other Report Name";
                    lblvalidation1.Visible = true;
                    txtexcelname.Focus();
                }
            }


        }
        catch
        { }

    }
    public void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        { printmethod(); }
        catch { }
    }
    public void printmethod()
    {
        try
        {
            string degreedetails = "";
            string pagename = "";
            if (rbstud.Checked == true)
            {
                degreedetails = "Student Report";
                pagename = "DailyPayment_Report.aspx";
                Printcontrolhed.loadspreaddetails(FpSpread1, pagename, degreedetails);
                Printcontrolhed.Visible = true;
            }
            if (rbstaff.Checked == true)
            {
                degreedetails = "Staff Report";
                pagename = "DailyPayment_Report.aspx";
                Printcontrolhed.loadspreaddetails(FpSpread1, pagename, degreedetails);
                Printcontrolhed.Visible = true;
            }
            if (rbvendor.Checked == true)
            {
                degreedetails = "Vendor Report";
                pagename = "DailyPayment_Report.aspx";
                Printcontrolhed.loadspreaddetails(FpSpread1, pagename, degreedetails);
                Printcontrolhed.Visible = true;
            }
            if (rbother.Checked == true)
            {
                degreedetails = "Other Report";
                pagename = "DailyPayment_Report.aspx";
                Printcontrolhed.loadspreaddetails(FpSpread1, pagename, degreedetails);
                Printcontrolhed.Visible = true;
            }
        }
        catch { }
    }
    #endregion

    public void btn_errorclose_Click(object sender, EventArgs e)
    {
        pupdiv.Visible = false;
        pupdiv.Visible = false;
    }

    #region auto search

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetName(string prefixText)
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
                    query = "select top 100 Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%' order by Roll_No asc";
                }
                else if (chosedmode == 1)
                {
                    query = "select  top 100 Reg_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Reg_No like '" + prefixText + "%' order by Reg_No asc";
                }
                else if (chosedmode == 2)
                {
                    query = "select  top 100 Roll_admit from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_admit like '" + prefixText + "%' order by Roll_admit asc";
                }
                else
                {
                    query = "  select  top 100 app_formno from applyn where admission_status =0 and isconfirm ='1' and app_formno like '" + prefixText + "%' order by app_formno asc";
                }
            }
            else if (personmode == 1)
            {
                query = "select distinct top (50) s.staff_code from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_code like '" + prefixText + "%'";
            }
            else if (personmode == 2)
            {
                query = "select VendorCompName+'-'+VendorCode as vendorcodename ,VendorPK  from CO_VendorMaster where VendorType =1 and VendorCompName like '" + prefixText + "%' ";
            }
            else if (personmode == 3)
            {
                query = "select (VendorName +'-'+ convert (varchar(20),VendorPK)) as VendorName from CO_VendorMaster  where VendorType='-5' and VendorName like '%' ";
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
            if (personmode == 0)
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
                        txtsearch.Attributes.Add("placeholder", "Roll No");
                        chosedmode = 0;
                        break;
                    case 1:
                        txtsearch.Attributes.Add("placeholder", "Reg No");
                        chosedmode = 1;
                        break;
                    case 2:
                        txtsearch.Attributes.Add("placeholder", "Admin No");
                        chosedmode = 2;
                        break;
                    case 3:
                        txtsearch.Attributes.Add("placeholder", "App No");
                        chosedmode = 3;
                        break;
                }

            }


        }
        catch { }
    }
    protected void rbl_rollno_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtsearch.Text = "";
            switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
            {
                case 0:
                    txtsearch.Attributes.Add("Placeholder", "Roll No");
                    chosedmode = 0;
                    break;
                case 1:
                    txtsearch.Attributes.Add("Placeholder", "Reg No");
                    chosedmode = 1;
                    break;
                case 2:
                    txtsearch.Attributes.Add("Placeholder", "Admin No");
                    chosedmode = 2;
                    break;
                case 3:
                    txtsearch.Attributes.Add("Placeholder", "App No");
                    chosedmode = 2;
                    break;
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
            if (rollval != "" && regval != "")
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
        }
        catch { }
        // return new Tuple<byte, byte>(roll,reg);

    }

    protected void spreadColumnVisible()
    {
        try
        {
            if (roll == 0)
            {
                FpSpread1.Columns[1].Visible = true;
                FpSpread1.Columns[2].Visible = true;
                FpSpread1.Columns[3].Visible = true;
            }
            else if (roll == 1)
            {
                FpSpread1.Columns[1].Visible = true;
                FpSpread1.Columns[2].Visible = true;
                FpSpread1.Columns[3].Visible = true;
            }
            else if (roll == 2)
            {
                FpSpread1.Columns[1].Visible = true;
                FpSpread1.Columns[2].Visible = false;
                FpSpread1.Columns[3].Visible = false;

            }
            else if (roll == 3)
            {
                FpSpread1.Columns[1].Visible = false;
                FpSpread1.Columns[2].Visible = true;
                FpSpread1.Columns[3].Visible = false;
            }
            else if (roll == 4)
            {
                FpSpread1.Columns[1].Visible = false;
                FpSpread1.Columns[2].Visible = false;
                FpSpread1.Columns[3].Visible = true;
            }
            else if (roll == 5)
            {
                FpSpread1.Columns[1].Visible = true;
                FpSpread1.Columns[2].Visible = true;
                FpSpread1.Columns[3].Visible = false;
            }
            else if (roll == 6)
            {
                FpSpread1.Columns[1].Visible = false;
                FpSpread1.Columns[2].Visible = true;
                FpSpread1.Columns[3].Visible = true;
            }
            else if (roll == 7)
            {
                FpSpread1.Columns[1].Visible = true;
                FpSpread1.Columns[2].Visible = false;
                FpSpread1.Columns[3].Visible = true;
            }
        }
        catch { }
    }

    #endregion

    // last modified 27.10.2017 sudhagar
    protected void cbAll_OnCheckedChanged(object sender, EventArgs e)
    {
        //stud menu
        tdstr.Visible = false;
        tdddlstr.Visible = false;
        tdbatch.Visible = false;
        tdcblbatch.Visible = false;
        tddegree.Visible = false;
        tdcbldegree.Visible = false;
        tddept.Visible = false;
        tdcbldept.Visible = false;
        tdsem.Visible = false;
        tdcblsem.Visible = false;
        tdsec.Visible = false;
        tdcblsec.Visible = false;
        //staff menu
        tdstaffdept.Visible = false;
        tdcblstaffdept.Visible = false;
        tdstaffdesg.Visible = false;
        tdcblstaffdesg.Visible = false;
        tdstafftype.Visible = false;
        tdcblstafftype.Visible = false;
        //vendor menu
        tdvendorcode.Visible = false;
        tdcblvendorcode.Visible = false;
        tdvendorname.Visible = false;
        tdcblvendorname.Visible = false;
        tdvendorcont.Visible = false;
        tdcblvendorcont.Visible = false;

        rbl_rollno.Visible = false;
        //lbltext.Visible = false;
        //txtsearch.Visible = false;
        tddivsearch.Visible = false;
        FpSpread1.Visible = false;
    }
    protected void cbReceipt_Changed(object sender, EventArgs e)
    {
    }
    protected void cbPayment_Changed(object sender, EventArgs e)
    {
    }

    public DataSet loadDataset()
    {
        try
        {
            string batchyr = "";
            string courseid = "";
            string feecat = "";
            string SelectQ = "";
            string sec = "";
            string headerid = "";
            string ledgerid = "";
            string finlyr = "";
            string fromdate = "";
            string todate = "";
            string headerorledger = "";
            string staffdept = "";
            string staffdesg = "";
            string stafftype = "";
            string vendorcode = "";
            string vendorname = "";
            string vendorcont = "";
            string txtcode = "";
            string Appno = "";
            string checkedVal = string.Empty;
            if (rbstud.Checked)
                checkedVal = "1";
            else if (rbstaff.Checked)
                checkedVal = "2";
            else if (rbvendor.Checked)
                checkedVal = "3";
            else if (rbother.Checked)
                checkedVal = "4";
            else if (cbAll.Checked)
                checkedVal = "5";

            if (string.IsNullOrEmpty(Convert.ToString(txtsearch.Text).Trim()))
            {
                switch (checkedVal)
                {
                    case "1":
                        batchyr = Convert.ToString(getCblSelectedValue(cbl_batch));
                        courseid = Convert.ToString(getCblSelectedValue(cbl_dept));
                        feecat = Convert.ToString(getCblSelectedValue(cbl_sem));
                        sec = Convert.ToString(getCblSelectedValue(cbl_sect));
                        break;
                    case "2":
                        staffdept = Convert.ToString(getCblSelectedValue(cbl_staffdept));
                        staffdesg = Convert.ToString(getCblSelectedValue(cbl_staffdesg));
                        stafftype = Convert.ToString(getCblSelectedValue(cbl_stafftype));
                        break;
                    case "3":
                    case "4":
                        vendorcode = Convert.ToString(getCblSelectedValue(cbl_vendorcode));
                        vendorname = Convert.ToString(getCblSelectedValue(cbl_vendorname));
                        vendorcont = Convert.ToString(getCblSelectedValue(cbl_vendorcont));
                        break;
                }
                headerid = Convert.ToString(getCblSelectedValue(chkl_studhed));
                ledgerid = Convert.ToString(getCblSelectedValue(chkl_studled));
                //  ledgerid = "4','11','8','344','3','291";
                finlyr = Convert.ToString(getCblSelectedValue(chklsfyear));

                fromdate = txt_fromdate.Text;
                todate = txt_todate.Text;
                if (fromdate != "" && todate != "")
                {
                    string[] frdate = fromdate.Split('/');
                    if (frdate.Length == 3)
                        fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
                    string[] tdate = todate.Split('/');
                    if (tdate.Length == 3)
                        todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
                }
            }
            else
            {
                string textcode = Convert.ToString(txtsearch.Text);
                Appno = getAppNo(txtcode);
            }



            string selQ = string.Empty;
            if (cbPayment.Checked)
            {
                #region payment
                switch (checkedVal)
                {
                    case "1":
                        selQ = " select transcode[Transaction Code],convert(varchar(10),transdate,103)[Transaction Date],Headerfk,ledgerfk,(case when paymode='1' then 'Cash' when paymode='2' then 'Cheque' when paymode='3' then 'DD' when paymode='4' then 'Challan' when paymode='5' then 'Online' when paymode='6' then 'Card' end)paymode,f.app_no,sum(credit)[Debit],ddno[Cheque No],convert(varchar(10),dddate,103)[cheque Date],Narration from ft_findailytransaction f,registration r where f.app_no=r.app_no  and r.college_code in('" + collegecode + "') and isnull(iscanceled,'0')='0' and isnull(transcode,'')<>'' and transdate between '" + fromdate + "' and '" + todate + "' and headerfk in('" + headerid + "') and ledgerfk in('" + ledgerid + "') and finyearfk in('" + finlyr + "')  and f.memtype='1'";
                        if (batchyr != "")
                            selQ += " and r.Batch_Year in ('" + batchyr + "')";
                        if (courseid != "")
                            selQ += "  and  r.Degree_Code in ('" + courseid + "')";
                        selQ += " group by transcode,transdate,Headerfk,ledgerfk,paymode,f.app_no,ddno,dddate,Narration having sum(credit)>0";
                        break;
                    case "2":
                        selQ = " select transcode[Transaction Code],convert(varchar(10),transdate,103)[Transaction Date],Headerfk,ledgerfk,(case when f.paymode='1' then 'Cash' when f.paymode='2' then 'Cheque' when f.paymode='3' then 'DD' when f.paymode='4' then 'Challan' when f.paymode='5' then 'Online' when f.paymode='6' then 'Card' end)paymode,app_no,sum(credit)[Debit],ddno[Cheque No],convert(varchar(10),dddate,103)[cheque Date],Narration from ft_findailytransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and h.dept_code =sa.dept_code and d.desig_code =sa.desig_code and T.staff_code =s.staff_code and T.dept_code =h.dept_code and T.desig_code =D.desig_code and T.latestrec ='1' and  isnull(iscanceled,'0')='0' and isnull(transcode,'')<>'' and transdate between '" + fromdate + "' and '" + todate + "' and headerfk in('" + headerid + "') and ledgerfk in('" + ledgerid + "') and finyearfk in('" + finlyr + "') and f.memtype='2' ";
                        if (staffdept != "")
                            selQ += " and sa.Dept_Code in ('" + staffdept + "')";
                        if (staffdesg != "")
                            selQ += "  and d.desig_code in ('" + staffdesg + "')";
                        if (stafftype != "")
                            selQ += " and t.StfType in ('" + stafftype + "')";
                        selQ += " group by transcode,transdate,Headerfk,ledgerfk,f.paymode,app_no,ddno,dddate,Narration having sum(credit)>0";
                        break;
                    case "3":
                        selQ = " select transcode[Transaction Code],convert(varchar(10),transdate,103)[Transaction Date],Headerfk,ledgerfk,(case when paymode='1' then 'Cash' when paymode='2' then 'Cheque' when paymode='3' then 'DD' when paymode='4' then 'Challan' when paymode='5' then 'Online' when paymode='6' then 'Card' end)paymode,app_no,sum(credit)[Debit],ddno[Cheque No],convert(varchar(10),dddate,103)[cheque Date],Narration from ft_findailytransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1' and isnull(iscanceled,'0')='0' and isnull(transcode,'')<>'' and transdate between '" + fromdate + "' and '" + todate + "' and headerfk in('" + headerid + "') and ledgerfk in('" + ledgerid + "') and finyearfk in('" + finlyr + "') and f.memtype='3'";
                        if (vendorcode != "")
                            selQ += " and p.VendorCode in ('" + vendorcode + "')";
                        if (vendorcont != "")
                            selQ += " and vc.VendorContactPK in ('" + vendorcont + "')";

                        selQ += " group by transcode,transdate,Headerfk,ledgerfk,paymode,app_no,ddno,dddate,Narration having sum(credit)>0";
                        break;
                    case "4":
                        selQ = " select transcode[Transaction Code],convert(varchar(10),transdate,103)[Transaction Date],Headerfk,ledgerfk,(case when paymode='1' then 'Cash' when paymode='2' then 'Cheque' when paymode='3' then 'DD' when paymode='4' then 'Challan' when paymode='5' then 'Online' when paymode='6' then 'Card' end)paymode,app_no,sum(credit)[Debit],ddno[Cheque No],convert(varchar(10),dddate,103)[cheque Date],Narration from ft_findailytransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5' and isnull(iscanceled,'0')='0' and isnull(transcode,'')<>'' and transdate between '" + fromdate + "' and '" + todate + "' and headerfk in('" + headerid + "') and ledgerfk in('" + ledgerid + "') and finyearfk in('" + finlyr + "') and f.memtype='4'";
                        if (vendorcode != "")
                            selQ += " and p.VendorPK in ('" + vendorcode + "')";
                        selQ += " group by transcode,transdate,Headerfk,ledgerfk,paymode,app_no,ddno,dddate,Narration having sum(credit)>0";
                        break;
                    case "5":
                        selQ = " select transcode[Transaction Code],convert(varchar(10),transdate,103)[Transaction Date],Headerfk,ledgerfk,(case when paymode='1' then 'Cash' when paymode='2' then 'Cheque' when paymode='3' then 'DD' when paymode='4' then 'Challan' when paymode='5' then 'Online' when paymode='6' then 'Card' end)paymode,app_no,sum(credit)[Debit],ddno[Cheque No],convert(varchar(10),dddate,103)[cheque Date],Narration from ft_findailytransaction where isnull(iscanceled,'0')='0' and isnull(transcode,'')<>'' and transdate between '" + fromdate + "' and '" + todate + "' and headerfk in('" + headerid + "') and ledgerfk in('" + ledgerid + "') and finyearfk in('" + finlyr + "') group by transcode,transdate,Headerfk,ledgerfk,paymode,app_no,ddno,dddate,Narration having sum(credit)>0";
                        break;
                }
                #endregion

            }
            if (cbReceipt.Checked)
            {
                #region Receipt
                switch (checkedVal)
                {
                    case "1":
                        selQ += " select transcode[Transaction Code],convert(varchar(10),transdate,103)[Transaction Date],Headerfk,ledgerfk,(case when paymode='1' then 'Cash' when paymode='2' then 'Cheque' when paymode='3' then 'DD' when paymode='4' then 'Challan' when paymode='5' then 'Online' when paymode='6' then 'Card' end)paymode,f.app_no,sum(debit)[Debit],ddno[Cheque No],convert(varchar(10),dddate,103)[cheque Date],Narration from ft_findailytransaction f,registration r where f.app_no=r.app_no and r.college_code in('" + collegecode + "') and isnull(iscanceled,'0')='0' and isnull(transcode,'')<>'' and transdate between '" + fromdate + "' and '" + todate + "' and headerfk in('" + headerid + "') and ledgerfk in('" + ledgerid + "') and finyearfk in('" + finlyr + "') and f.memtype='1'";
                        if (batchyr != "")
                            selQ += " and r.Batch_Year in ('" + batchyr + "')";
                        if (courseid != "")
                            selQ += "  and  r.Degree_Code in ('" + courseid + "')";
                        selQ += " group by transcode,transdate,Headerfk,ledgerfk,paymode,f.app_no,ddno,dddate,Narration having sum(debit)>0";
                        break;
                    case "2":
                        selQ += " select transcode[Transaction Code],convert(varchar(10),transdate,103)[Transaction Date],Headerfk,ledgerfk,(case when f.paymode='1' then 'Cash' when f.paymode='2' then 'Cheque' when f.paymode='3' then 'DD' when f.paymode='4' then 'Challan' when f.paymode='5' then 'Online' when f.paymode='6' then 'Card' end)paymode,app_no,sum(debit)[Debit],ddno[Cheque No],convert(varchar(10),dddate,103)[cheque Date],Narration from ft_findailytransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and h.dept_code =sa.dept_code and d.desig_code =sa.desig_code and T.staff_code =s.staff_code and T.dept_code =h.dept_code and T.desig_code =D.desig_code and T.latestrec ='1' and  isnull(iscanceled,'0')='0' and isnull(transcode,'')<>'' and transdate between '" + fromdate + "' and '" + todate + "' and headerfk in('" + headerid + "') and ledgerfk in('" + ledgerid + "') and finyearfk in('" + finlyr + "') and f.memtype='2'";
                        if (staffdept != "")
                            selQ += " and sa.Dept_Code in ('" + staffdept + "')";
                        if (staffdesg != "")
                            selQ += "  and d.desig_code in ('" + staffdesg + "')";
                        if (stafftype != "")
                            selQ += " and t.StfType in ('" + stafftype + "')";
                        selQ += " group by transcode,transdate,Headerfk,ledgerfk,f.paymode,app_no,ddno,dddate,Narration having sum(debit)>0";
                        break;
                    case "3":
                        selQ += " select transcode[Transaction Code],convert(varchar(10),transdate,103)[Transaction Date],Headerfk,ledgerfk,(case when paymode='1' then 'Cash' when paymode='2' then 'Cheque' when paymode='3' then 'DD' when paymode='4' then 'Challan' when paymode='5' then 'Online' when paymode='6' then 'Card' end)paymode,app_no,sum(debit)[Debit],ddno[Cheque No],convert(varchar(10),dddate,103)[cheque Date],Narration from ft_findailytransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1' and isnull(iscanceled,'0')='0' and isnull(transcode,'')<>'' and transdate between '" + fromdate + "' and '" + todate + "' and headerfk in('" + headerid + "') and ledgerfk in('" + ledgerid + "') and finyearfk in('" + finlyr + "') and f.memtype='3'";
                        if (vendorcode != "")
                            selQ += " and p.VendorCode in ('" + vendorcode + "')";
                        if (vendorcont != "")
                            selQ += " and vc.VendorContactPK in ('" + vendorcont + "')";

                        selQ += " group by transcode,transdate,Headerfk,ledgerfk,paymode,app_no,ddno,dddate,Narration having sum(debit)>0";
                        break;
                    case "4":
                        selQ += " select transcode[Transaction Code],convert(varchar(10),transdate,103)[Transaction Date],Headerfk,ledgerfk,(case when paymode='1' then 'Cash' when paymode='2' then 'Cheque' when paymode='3' then 'DD' when paymode='4' then 'Challan' when paymode='5' then 'Online' when paymode='6' then 'Card' end)paymode,app_no,sum(debit)[Debit],ddno[Cheque No],convert(varchar(10),dddate,103)[cheque Date],Narration from ft_findailytransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5' and isnull(iscanceled,'0')='0' and isnull(transcode,'')<>'' and transdate between '" + fromdate + "' and '" + todate + "' and headerfk in('" + headerid + "') and ledgerfk in('" + ledgerid + "') and finyearfk in('" + finlyr + "') and f.memtype='4'";
                        if (vendorcode != "")
                            selQ += " and p.VendorPK in ('" + vendorcode + "')";
                        selQ += " group by transcode,transdate,Headerfk,ledgerfk,paymode,app_no,ddno,dddate,Narration having sum(debit)>0";
                        break;
                    case "5":
                        selQ += " select transcode[Transaction Code],convert(varchar(10),transdate,103)[Transaction Date],Headerfk,ledgerfk,(case when paymode='1' then 'Cash' when paymode='2' then 'Cheque' when paymode='3' then 'DD' when paymode='4' then 'Challan' when paymode='5' then 'Online' when paymode='6' then 'Card' end)paymode,app_no,sum(debit)[Debit],ddno[Cheque No],convert(varchar(10),dddate,103)[cheque Date],Narration from ft_findailytransaction where isnull(iscanceled,'0')='0' and isnull(transcode,'')<>'' and transdate between '" + fromdate + "' and '" + todate + "' and headerfk in('" + headerid + "') and ledgerfk in('" + ledgerid + "') and finyearfk in('" + finlyr + "') group by transcode,transdate,Headerfk,ledgerfk,paymode,app_no,ddno,dddate,Narration having sum(debit)>0";
                        break;
                }
                #endregion
            }


            //select transcode[Transaction Code],convert(varchar(10),transdate,103)[Transaction Date],Headerfk,ledgerfk,(case when paymode='1' then 'Cash' when paymode='2' then 'Cheque' when paymode='3' then 'DD' when paymode='4' then 'Challan' when paymode='5' then 'Online' when paymode='6' then 'Card' end)paymode,app_no,sum(credit)[Debit],ddno[Cheque No],convert(varchar(10),dddate,103)[cheque Date],Narration from ft_findailytransaction where isnull(iscanceled,'0')='0' and isnull(transcode,'')<>'' and transdate='10/26/2017' group by transcode,transdate,Headerfk,ledgerfk,paymode,app_no,ddno,dddate,Narration having sum(credit)>0

            //select transcode[Transaction Code],convert(varchar(10),transdate,103)[Transaction Date],Headerfk,ledgerfk,(case when paymode='1' then 'Cash' when paymode='2' then 'Cheque' when paymode='3' then 'DD' when paymode='4' then 'Challan' when paymode='5' then 'Online' when paymode='6' then 'Card' end)paymode,app_no,sum(debit)[Debit],ddno[Cheque No],convert(varchar(10),dddate,103)[cheque Date],Narration from ft_findailytransaction where isnull(iscanceled,'0')='0' and isnull(transcode,'')<>'' and transdate='10/26/2017' group by transcode,transdate,Headerfk,ledgerfk,paymode,app_no,ddno,dddate,Narration having sum(debit)>0
            #region old

            //if (rbstud.Checked == true)
            //{
            //    #region stud
            //    if (chkcumul.Checked == false)
            //    {
            //        if (txtsearch.Text == "")
            //        {
            //            SelectQ = " select Credit,TransCode,r.Roll_No,r.roll_admit,r.Reg_No,PayMode,r.Stud_Name,c.Course_Name +' - '+ dt.Dept_Name as Dept_Name ,CONVERT(varchar(20),TransDate,103) as  TransDate,f.App_no" + headerorledger + ",Narration  from FT_FinDailyTransaction f,Registration r,Degree d,Department dt,Course c where r.App_No=f.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and TransType ='2' ";
            //            if (batchyr != "")
            //            {
            //                SelectQ = SelectQ + " and r.Batch_Year in ('" + batchyr + "')";
            //            }
            //            if (courseid != "")
            //            {
            //                SelectQ = SelectQ + "  and  r.Degree_Code in ('" + courseid + "')";
            //            }
            //            if (feecat != "")
            //            {
            //                // SelectQ = SelectQ + " and f.FeeCategory in ('" + feecat + "')";
            //            }
            //            if (sec != "")
            //            {
            //                //SelectQ = SelectQ + " and   ISNULL( r.Sections,'') in ('" + sec + "','')";
            //            }

            //            if (headerid != "")
            //            {
            //                SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
            //            }
            //            if (ledgerid != "")
            //            {
            //                SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
            //            }
            //            if (fromdate != "" && todate != "")
            //            {
            //                SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //            }
            //        }
            //        else if (txtsearch.Text != "")
            //        {
            //            SelectQ = "   select Credit,TransCode,r.Roll_No,r.roll_admit,r.Reg_No,PayMode,r.Stud_Name,c.Course_Name +' - '+ dt.Dept_Name as Dept_Name ,CONVERT(varchar(20),TransDate,103) as  TransDate,f.App_no" + headerorledger + ",Narration  from FT_FinDailyTransaction f,Registration r,Degree d,Department dt,Course c where r.App_No=f.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and TransType ='2'";
            //            if (Appno != "")
            //            {
            //                SelectQ = SelectQ + " and r.App_No='" + Appno + "'";
            //            }
            //            if (headerid != "")
            //            {
            //                SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
            //            }
            //            if (ledgerid != "")
            //            {
            //                SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
            //            }
            //            if (fromdate != "" && todate != "")
            //            {
            //                SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (txtsearch.Text == "")
            //        {
            //            SelectQ = " select ISNULL( sum(Credit),0) as Credit,r.Roll_No,r.roll_admit,r.Reg_No,PayMode,r.Stud_Name,c.Course_Name +' - '+ dt.Dept_Name as Dept_Name ,f.App_no,Narration    from FT_FinDailyTransaction f,Registration r,Degree d,Department dt,Course c where r.App_No=f.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and TransType ='2' ";
            //            if (batchyr != "")
            //            {
            //                SelectQ = SelectQ + " and r.Batch_Year in ('" + batchyr + "')";
            //            }
            //            if (courseid != "")
            //            {
            //                SelectQ = SelectQ + "  and  r.Degree_Code in ('" + courseid + "')";
            //            }

            //            if (headerid != "")
            //            {
            //                SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
            //            }
            //            if (ledgerid != "")
            //            {
            //                SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
            //            }
            //            if (fromdate != "" && todate != "")
            //            {
            //                SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //            }
            //            SelectQ = SelectQ + " group by r.Roll_No,r.Reg_No,PayMode,r.Stud_Name,c.Course_Name +' - '+ dt.Dept_Name  ,f.App_no,Narration,r.roll_admit  ";

            //            SelectQ = SelectQ + "  select ISNULL( sum(Credit),0) as Credit,PayMode ,f.App_no " + headerorledger + "  from FT_FinDailyTransaction f,Registration r,Degree d,Department dt,Course c where r.App_No=f.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and TransType ='2'";
            //            if (batchyr != "")
            //            {
            //                SelectQ = SelectQ + " and r.Batch_Year in ('" + batchyr + "')";
            //            }
            //            if (courseid != "")
            //            {
            //                SelectQ = SelectQ + "  and  r.Degree_Code in ('" + courseid + "')";
            //            }

            //            if (headerid != "")
            //            {
            //                SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
            //            }
            //            if (ledgerid != "")
            //            {
            //                SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
            //            }
            //            if (fromdate != "" && todate != "")
            //            {
            //                SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //            }
            //            SelectQ = SelectQ + "group by PayMode ,f.App_no " + headerorledger + " ";
            //        }
            //        else if (txtsearch.Text != "")
            //        {
            //            SelectQ = " select ISNULL( sum(Credit),0) as Credit,r.Roll_No,r.roll_admit,r.Reg_No,PayMode,r.Stud_Name,c.Course_Name +' - '+ dt.Dept_Name as Dept_Name ,f.App_no,Narration  from FT_FinDailyTransaction f,Registration r,Degree d,Department dt,Course c where r.App_No=f.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and TransType ='2' ";
            //            if (Appno != "")
            //            {
            //                SelectQ = SelectQ + " and r.App_No='" + Appno + "'";
            //            }
            //            if (headerid != "")
            //            {
            //                SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
            //            }
            //            if (ledgerid != "")
            //            {
            //                SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
            //            }
            //            if (fromdate != "" && todate != "")
            //            {
            //                SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //            }
            //            SelectQ = SelectQ + " group by r.Roll_No,r.Reg_No,PayMode,r.Stud_Name,c.Course_Name +' - '+ dt.Dept_Name  ,f.App_no,Narration ,r.roll_admit  ";

            //            SelectQ = SelectQ + "  select ISNULL( sum(Credit),0) as Credit,PayMode ,f.App_no " + headerorledger + "  from FT_FinDailyTransaction f,Registration r,Degree d,Department dt,Course c where r.App_No=f.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and TransType ='2'";
            //            if (Appno != "")
            //            {
            //                SelectQ = SelectQ + " and r.App_No='" + Appno + "'";
            //            }
            //            if (headerid != "")
            //            {
            //                SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
            //            }
            //            if (ledgerid != "")
            //            {
            //                SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
            //            }
            //            if (fromdate != "" && todate != "")
            //            {
            //                SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //            }
            //            SelectQ = SelectQ + "group by PayMode ,f.App_no " + headerorledger + " ";
            //        }
            //    }

            //    #endregion
            //}
            //else if (rbstaff.Checked == true)
            //{
            //    #region staff
            //    if (chkcumul.Checked == false)
            //    {
            //        if (txtsearch.Text == "")
            //        {
            //            SelectQ = " select  Credit,f.App_no,TransCode,sa.appl_id,s.staff_code,s.staff_name,sa.Dept_Code,h.dept_name,D.desig_name" + headerorledger + ",TransCode,CONVERT(varchar(20),TransDate,103) as TransDate,d.desig_code,f.Narration from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and h.dept_code =sa.dept_code and d.desig_code =sa.desig_code and T.staff_code =s.staff_code and T.dept_code =h.dept_code and T.desig_code =D.desig_code and T.latestrec ='1' ";
            //            if (staffdept != "")
            //            {
            //                SelectQ = SelectQ + " and sa.Dept_Code in ('" + staffdept + "')";
            //            }
            //            if (staffdesg != "")
            //            {
            //                SelectQ = SelectQ + "  and d.desig_code in ('" + staffdesg + "')";
            //            }
            //            if (stafftype != "")
            //            {
            //                SelectQ = SelectQ + " and t.StfType in ('" + stafftype + "')";
            //            }
            //            if (headerid != "")
            //            {
            //                SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
            //            }
            //            if (ledgerid != "")
            //            {
            //                SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
            //            }
            //            if (fromdate != "" && todate != "")
            //            {
            //                SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //            }
            //        }
            //        else if (txtsearch.Text != "")
            //        {
            //            SelectQ = "select  Credit,f.App_no,TransCode,sa.appl_id,s.staff_code,s.staff_name,sa.Dept_Code,h.dept_name,D.desig_name" + headerorledger + ",TransCode,CONVERT(varchar(20),TransDate,103) as  TransDate,d.desig_code,f.Narration from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and h.dept_code =sa.dept_code and d.desig_code =sa.desig_code and T.staff_code =s.staff_code and T.dept_code =h.dept_code and T.desig_code =D.desig_code and T.latestrec ='1'";
            //            //and appl_id='" + Appno + "'";
            //            if (Appno != "")
            //            {
            //                SelectQ = SelectQ + " and appl_id in ('" + Appno + "')";
            //            }

            //            if (headerid != "")
            //            {
            //                SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
            //            }
            //            if (ledgerid != "")
            //            {
            //                SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
            //            }
            //            if (fromdate != "" && todate != "")
            //            {
            //                SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (txtsearch.Text == "")
            //        {
            //            SelectQ = " select  ISNULL( sum(Credit),0) as Credit,f.App_no,sa.appl_id,s.staff_code,s.staff_name,sa.Dept_Code,h.dept_name,D.desig_name ,d.desig_code,f.Narration from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and h.dept_code =sa.dept_code and d.desig_code =sa.desig_code and T.staff_code =s.staff_code and T.dept_code =h.dept_code and T.desig_code =D.desig_code and T.latestrec ='1'";
            //            if (staffdept != "")
            //            {
            //                SelectQ = SelectQ + " and sa.Dept_Code in ('" + staffdept + "')";
            //            }
            //            if (staffdesg != "")
            //            {
            //                SelectQ = SelectQ + "  and d.desig_code in ('" + staffdesg + "')";
            //            }
            //            if (stafftype != "")
            //            {
            //                SelectQ = SelectQ + " and t.StfType in ('" + stafftype + "')";
            //            }
            //            if (headerid != "")
            //            {
            //                SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
            //            }
            //            if (ledgerid != "")
            //            {
            //                SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
            //            }
            //            if (fromdate != "" && todate != "")
            //            {
            //                SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //            }
            //            SelectQ = SelectQ + "group by f.App_no,sa.appl_id,s.staff_code,s.staff_name,sa.Dept_Code,h.dept_name,D.desig_name ,d.desig_code,Narration";

            //            SelectQ = SelectQ + " select  ISNULL( sum(Credit),0) as credit,f.App_no" + headerorledger + " from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and h.dept_code =sa.dept_code and d.desig_code =sa.desig_code and T.staff_code =s.staff_code and T.dept_code =h.dept_code and T.desig_code =D.desig_code and T.latestrec ='1' ";
            //            if (staffdept != "")
            //            {
            //                SelectQ = SelectQ + " and sa.Dept_Code in ('" + staffdept + "')";
            //            }
            //            if (staffdesg != "")
            //            {
            //                SelectQ = SelectQ + "  and d.desig_code in ('" + staffdesg + "')";
            //            }
            //            if (stafftype != "")
            //            {
            //                SelectQ = SelectQ + " and t.StfType in ('" + stafftype + "')";
            //            }
            //            if (headerid != "")
            //            {
            //                SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
            //            }
            //            if (ledgerid != "")
            //            {
            //                SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
            //            }
            //            if (fromdate != "" && todate != "")
            //            {
            //                SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //            }
            //            SelectQ = SelectQ + " group by f.App_no,sa.appl_id " + headerorledger + " ";
            //        }
            //        else if (txtsearch.Text != "")
            //        {
            //            SelectQ = " select  ISNULL( sum(Credit),0) as Credit,f.App_no,sa.appl_id,s.staff_code,s.staff_name,sa.Dept_Code,h.dept_name,D.desig_name ,d.desig_code,f.Narration from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and h.dept_code =sa.dept_code and d.desig_code =sa.desig_code and T.staff_code =s.staff_code and T.dept_code =h.dept_code and T.desig_code =D.desig_code and T.latestrec ='1'";
            //            if (Appno != "")
            //            {
            //                SelectQ = SelectQ + " and appl_id in ('" + Appno + "')";
            //            }

            //            if (headerid != "")
            //            {
            //                SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
            //            }
            //            if (ledgerid != "")
            //            {
            //                SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
            //            }
            //            if (fromdate != "" && todate != "")
            //            {
            //                SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //            }
            //            SelectQ = SelectQ + "group by f.App_no,sa.appl_id,s.staff_code,s.staff_name,sa.Dept_Code,h.dept_name,D.desig_name ,d.desig_code,Narration";

            //            SelectQ = SelectQ + " select  ISNULL( sum(Credit),0) as credit,f.App_no" + headerorledger + " from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and h.dept_code =sa.dept_code and d.desig_code =sa.desig_code and T.staff_code =s.staff_code and T.dept_code =h.dept_code and T.desig_code =D.desig_code and T.latestrec ='1' ";
            //            if (Appno != "")
            //            {
            //                SelectQ = SelectQ + " and appl_id in ('" + Appno + "')";
            //            }

            //            if (headerid != "")
            //            {
            //                SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
            //            }
            //            if (ledgerid != "")
            //            {
            //                SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
            //            }
            //            if (fromdate != "" && todate != "")
            //            {
            //                SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //            }
            //            SelectQ = SelectQ + " group by f.App_no,sa.appl_id " + headerorledger + " ";
            //        }
            //    }
            //    #endregion
            //}
            //else if (rbvendor.Checked == true)
            //{
            //    #region vendor
            //    if (chkcumul.Checked == false)
            //    {
            //        if (txtsearch.Text == "")
            //        {
            //            SelectQ = "  SELECT Credit,p.VendorCode,f.App_no,p.VendorCompName,vc.VendorContactPK,vc.VenContactName,TransCode" + headerorledger + ",CONVERT(varchar(20),TransDate,103) as  TransDate ,f.Narration  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1'";

            //            if (vendorcode != "")
            //            {
            //                SelectQ = SelectQ + " and p.VendorCode in ('" + vendorcode + "')";
            //            }
            //            if (vendorname != "")
            //            {
            //                // SelectQ = SelectQ + "  and p.VendorName in ('" + vendorname + "')";
            //            }
            //            if (vendorcont != "")
            //            {
            //                SelectQ = SelectQ + " and vc.VendorContactPK in ('" + vendorcont + "')";
            //            }
            //            if (headerid != "")
            //            {
            //                SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
            //            }
            //            if (ledgerid != "")
            //            {
            //                SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
            //            }
            //            if (fromdate != "" && todate != "")
            //            {
            //                SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //            }
            //        }
            //        else if (txtsearch.Text != "")
            //        {
            //            SelectQ = "  SELECT Credit,p.VendorCode,f.App_no,p.VendorCompName,vc.VendorContactPK,vc.VenContactName,TransCode" + headerorledger + ",CONVERT(varchar(20),TransDate,103) as  TransDate,f.Narration   FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1' ";
            //            if (Appno != "")
            //            {
            //                SelectQ = SelectQ + " and App_No in ('" + Appno + "')";
            //            }
            //            if (headerid != "")
            //            {
            //                SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
            //            }
            //            if (ledgerid != "")
            //            {
            //                SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
            //            }
            //            if (fromdate != "" && todate != "")
            //            {
            //                SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (txtsearch.Text == "")
            //        {
            //            SelectQ = "  SELECT ISNULL( sum(Credit),0) as Credit,p.VendorCode,f.App_no,p.VendorCompName,vc.VendorContactPK,vc.VenContactName ,f.Narration  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1'";
            //            if (vendorcode != "")
            //            {
            //                SelectQ = SelectQ + " and p.VendorCode in ('" + vendorcode + "')";
            //            }
            //            if (vendorname != "")
            //            {
            //                // SelectQ = SelectQ + "  and p.VendorName in ('" + vendorname + "')";
            //            }
            //            if (vendorcont != "")
            //            {
            //                SelectQ = SelectQ + " and vc.VendorContactPK in ('" + vendorcont + "')";
            //            }
            //            if (headerid != "")
            //            {
            //                SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
            //            }
            //            if (ledgerid != "")
            //            {
            //                SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
            //            }
            //            if (fromdate != "" && todate != "")
            //            {
            //                SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //            }
            //            SelectQ = SelectQ + "  group by p.VendorCode,f.App_no,p.VendorCompName,vc.VendorContactPK,vc.VenContactName,Narration ";
            //            SelectQ = SelectQ + " SELECT ISNULL( sum(Credit),0) as Credit,f.App_no,vc.VendorContactPK" + headerorledger + ",f.Narration  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1'";
            //            if (vendorcode != "")
            //            {
            //                SelectQ = SelectQ + " and p.VendorCode in ('" + vendorcode + "')";
            //            }
            //            if (vendorname != "")
            //            {
            //                // SelectQ = SelectQ + "  and p.VendorName in ('" + vendorname + "')";
            //            }
            //            if (vendorcont != "")
            //            {
            //                SelectQ = SelectQ + " and vc.VendorContactPK in ('" + vendorcont + "')";
            //            }
            //            if (headerid != "")
            //            {
            //                SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
            //            }
            //            if (ledgerid != "")
            //            {
            //                SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
            //            }
            //            if (fromdate != "" && todate != "")
            //            {
            //                SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //            }
            //            SelectQ = SelectQ + " group by f.App_no,vc.VendorContactPK" + headerorledger + "";
            //        }
            //        else if (txtsearch.Text != "")
            //        {
            //            SelectQ = "  SELECT ISNULL( sum(Credit),0) as Credit,p.VendorCode,f.App_no,p.VendorCompName,vc.VendorContactPK,vc.VenContactName,f.Narration  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1'";
            //            if (Appno != "")
            //            {
            //                SelectQ = SelectQ + " and App_No in ('" + Appno + "')";
            //            }
            //            if (headerid != "")
            //            {
            //                SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
            //            }
            //            if (ledgerid != "")
            //            {
            //                SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
            //            }
            //            if (fromdate != "" && todate != "")
            //            {
            //                SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //            }
            //            SelectQ = SelectQ + "  group by p.VendorCode,f.App_no,p.VendorCompName,vc.VendorContactPK,vc.VenContactName,Narration ";
            //            SelectQ = SelectQ + " SELECT ISNULL( sum(Credit),0) as Credit,f.App_no,vc.VendorContactPK" + headerorledger + "  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1'";
            //            if (Appno != "")
            //            {
            //                SelectQ = SelectQ + " and App_No in ('" + Appno + "')";
            //            }
            //            if (headerid != "")
            //            {
            //                SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
            //            }
            //            if (ledgerid != "")
            //            {
            //                SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
            //            }
            //            if (fromdate != "" && todate != "")
            //            {
            //                SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //            }
            //            SelectQ = SelectQ + " group by f.App_no,vc.VendorContactPK" + headerorledger + "";
            //        }

            //    }
            //    #endregion
            //}
            //else if (rbother.Checked == true)
            //{
            //    #region others
            //    if (chkcumul.Checked == false)
            //    {
            //        if (txtsearch.Text == "")
            //        {
            //            SelectQ = "  SELECT Credit,p.VendorPK,p.VendorCode,f.App_no,p.VendorCompName,TransCode" + headerorledger + ",CONVERT(varchar(20),TransDate,103) as  TransDate,f.Narration   FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5'";
            //            if (vendorcode != "")
            //            {
            //                SelectQ = SelectQ + " and p.VendorCode in ('" + vendorcode + "')";
            //            }
            //            if (vendorname != "")
            //            {
            //                // SelectQ = SelectQ + "  and p.VendorName in ('" + vendorname + "')";
            //            }
            //            if (vendorcont != "")
            //            {
            //                // SelectQ = SelectQ + " and vc.VendorContactPK in ('" + vendorcont + "')";
            //            }
            //            if (headerid != "")
            //            {
            //                SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
            //            }
            //            if (ledgerid != "")
            //            {
            //                SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
            //            }
            //            if (fromdate != "" && todate != "")
            //            {
            //                SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //            }
            //        }
            //        else if (txtsearch.Text != "")
            //        {
            //            SelectQ = "  SELECT Credit,p.VendorPK,p.VendorCode,f.App_no,p.VendorCompName,TransCode" + headerorledger + ",CONVERT(varchar(20),TransDate,103) as  TransDate,f.Narration  FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5'";
            //            if (Appno != "")
            //            {
            //                SelectQ = SelectQ + " and App_No in ('" + Appno + "')";
            //            }
            //            if (headerid != "")
            //            {
            //                SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
            //            }
            //            if (ledgerid != "")
            //            {
            //                SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
            //            }
            //            if (fromdate != "" && todate != "")
            //            {
            //                SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (txtsearch.Text == "")
            //        {
            //            SelectQ = " SELECT ISNULL( sum(Credit),0) as credit,p.VendorPK,p.VendorCode,f.App_no,p.VendorCompName,f.Narration FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5'";
            //            if (vendorcode != "")
            //            {
            //                SelectQ = SelectQ + " and p.VendorCode in ('" + vendorcode + "')";
            //            }
            //            if (headerid != "")
            //            {
            //                SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
            //            }
            //            if (ledgerid != "")
            //            {
            //                SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
            //            }
            //            if (fromdate != "" && todate != "")
            //            {
            //                SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //            }
            //            SelectQ = SelectQ + " group by p.VendorPK,p.VendorCode,f.App_no,p.VendorCompName,Narration";
            //            SelectQ = SelectQ + " SELECT ISNULL( sum(Credit),0) as Credit,p.VendorPK,f.App_no" + headerorledger + " FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5' ";
            //            if (vendorcode != "")
            //            {
            //                SelectQ = SelectQ + " and p.VendorCode in ('" + vendorcode + "')";
            //            }
            //            if (headerid != "")
            //            {
            //                SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
            //            }
            //            if (ledgerid != "")
            //            {
            //                SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
            //            }
            //            if (fromdate != "" && todate != "")
            //            {
            //                SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //            }
            //            SelectQ = SelectQ + " group by p.VendorPK,f.App_no" + headerorledger + "";
            //        }
            //        else if (txtsearch.Text != "")
            //        {
            //            SelectQ = " SELECT ISNULL( sum(Credit),0) as credit,p.VendorPK,p.VendorCode,f.App_no,p.VendorCompName,f.Narration FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5'";
            //            if (Appno != "")
            //            {
            //                SelectQ = SelectQ + " and App_No in ('" + Appno + "')";
            //            }
            //            if (headerid != "")
            //            {
            //                SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
            //            }
            //            if (ledgerid != "")
            //            {
            //                SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
            //            }
            //            if (fromdate != "" && todate != "")
            //            {
            //                SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //            }
            //            SelectQ = SelectQ + " group by p.VendorPK,p.VendorCode,f.App_no,p.VendorCompName,Narration";
            //            SelectQ = SelectQ + " SELECT ISNULL( sum(Credit),0) as Credit,p.VendorPK,f.App_no" + headerorledger + " FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5' ";
            //            if (Appno != "")
            //            {
            //                SelectQ = SelectQ + " and App_No in ('" + Appno + "')";
            //            }
            //            if (headerid != "")
            //            {
            //                SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
            //            }
            //            if (ledgerid != "")
            //            {
            //                SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
            //            }
            //            if (fromdate != "" && todate != "")
            //            {
            //                SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
            //            }
            //            SelectQ = SelectQ + " group by p.VendorPK,f.App_no" + headerorledger + "";
            //        }
            //    }
            //    #endregion
            //}
            #endregion
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(selQ, "Text");
        }
        catch { }
        return dsload;
    }

    protected string getAppNo(string txtcode)
    {
        string Appno = string.Empty;
        try
        {
            if (rbstud.Checked == true)
            {
                txtcode = Convert.ToString(txtsearch.Text);
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                    Appno = d2.GetFunction(" select App_No from Registration where Roll_No='" + txtcode + "'");
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                    Appno = d2.GetFunction(" select App_No from Registration where reg_no='" + txtcode + "'");
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                    Appno = d2.GetFunction(" select App_No from Registration where Roll_admit='" + txtcode + "'");
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 3)
                    Appno = d2.GetFunction(" select app_no from applyn where app_formno='" + txtcode + "'");
            }
            else if (rbstaff.Checked == true)
            {
                Appno = d2.GetFunction("select sa.appl_id  from staffmaster s,staff_appl_master sa where s.appl_no =sa.appl_no and s.staff_code ='" + txtcode + "'");
            }
            else if (rbvendor.Checked == true)
            {
                string[] splitcode = txtcode.Split('-');
                if (splitcode.Length > 0)
                {
                    txtcode = Convert.ToString(splitcode[1]);
                    Appno = d2.GetFunction("select VendorContactPK from CO_VendorMaster v,IM_VendorContactMaster vc where v.VendorPK=vc.VendorFK and v.VendorCode='" + txtcode + "' and vendorType='1'");

                }
            }
            else if (rbother.Checked == true)
            {
                string[] splitcode = txtcode.Split('-');
                if (splitcode.Length > 0)
                {
                    txtcode = Convert.ToString(splitcode[1]);
                    Appno = Convert.ToString(txtcode);
                }
            }
        }
        catch { }
        return Appno;
    }
    protected Hashtable htSupplierNames()
    {
        Hashtable htName = new Hashtable();
        try
        {
            ArrayList arCount = new ArrayList();
            arCount.Add("0");
            arCount.Add("1");
            arCount.Add("2");
            arCount.Add("3");
            string collegecode = Convert.ToString(ddl_collegename.SelectedItem.Value);
            string selQ = " select app_no as pk,stud_name as name from registration where college_code='" + collegecode + "'";
            selQ += " select VendorContactPK as pk,vendorname as name from CO_VendorMaster v,IM_VendorContactMaster vc where v.VendorPK=vc.VendorFK  and vendorType='1'";
            selQ += "select vendorpk as pk,vendorname as name from CO_VendorMaster v where vendorType='-5'";
            selQ += "select sa.appl_id as pk,s.staff_name as name  from staffmaster s,staff_appl_master sa where s.appl_no =sa.appl_no and s.college_code='" + collegecode + "'";
            DataSet dsval = d2.select_method_wo_parameter(selQ, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                foreach (string rowCnt in arCount)
                {
                    int rowCount = Convert.ToInt32(rowCnt);
                    try
                    {
                        for (int row = 0; row < dsval.Tables[rowCount].Rows.Count; row++)
                        {
                            if (!htName.ContainsKey(Convert.ToString(dsval.Tables[rowCount].Rows[row]["pk"])))
                                htName.Add(Convert.ToString(dsval.Tables[rowCount].Rows[row]["pk"]), Convert.ToString(dsval.Tables[rowCount].Rows[row]["name"]));
                        }
                    }
                    catch { }
                }
            }
        }
        catch { }
        return htName;
    }
    protected Dictionary<string, string> getHeader()
    {
        Dictionary<string, string> dtHeader = new Dictionary<string, string>();
        dtHeader.Add("Sno", "0");
        dtHeader.Add("Transaction Code", "1");
        dtHeader.Add("Transaction Date", "2");
        dtHeader.Add("Header Name", "3");
        dtHeader.Add("Ledger Name", "4");
        dtHeader.Add("PayMode", "5");
        if (rbstud.Checked == true)
        {
            dtHeader.Add("Student Name", "6");
        }
        else
        {
            dtHeader.Add("Supplier Name", "6");
        }
        dtHeader.Add("Amount", "7");
        dtHeader.Add("Cheque No", "8");
        dtHeader.Add("Cheque Date", "9");
        dtHeader.Add("Narration", "10");
        return dtHeader;
    }

    protected void loadSpread(DataSet dsVal)
    {
        try
        {
            #region design
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            //  FpSpread1.Sheets[0].ColumnCount = 0;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FarPoint.Web.Spread.TextCellType regno = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.TextCellType rollno = new FarPoint.Web.Spread.TextCellType();

            Dictionary<string, string> dtHeader = getHeader();
            Dictionary<string, string> dtCol = new Dictionary<string, string>();
            Hashtable htHeaderName = getHDName();
            Hashtable htLedgerName = getLDName();
            Hashtable htSupplierName = htSupplierNames();
            Hashtable httotal = new Hashtable();
            foreach (KeyValuePair<string, string> header in dtHeader)
            {
                #region design
                FpSpread1.Sheets[0].ColumnCount++;
                int colCnt = FpSpread1.Sheets[0].ColumnCount - 1;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, colCnt].Text = Convert.ToString(header.Key);
                dtCol.Add(Convert.ToString(header.Key), Convert.ToString(colCnt));
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, colCnt].ForeColor = ColorTranslator.FromHtml("#000000");
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, colCnt].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, colCnt].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, colCnt].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, colCnt].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[colCnt].HorizontalAlign = HorizontalAlign.Center;
                switch (Convert.ToString(header.Key))
                {
                    case "Sno":
                        FpSpread1.Sheets[0].Columns[colCnt].Width = 50;
                        break;
                    case "PayMode":
                        FpSpread1.Sheets[0].Columns[colCnt].Width = 50;
                        break;
                    case "Header Name":
                        FpSpread1.Sheets[0].Columns[colCnt].HorizontalAlign = HorizontalAlign.Left;
                        break;
                    case "Ledger Name":
                        FpSpread1.Sheets[0].Columns[colCnt].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Columns[colCnt].Width = 200;
                        break;
                    case "Supplier Name":
                        FpSpread1.Sheets[0].Columns[colCnt].HorizontalAlign = HorizontalAlign.Left;
                        break;
                    case "Student Name"://added by abarna 07.05.2018
                        FpSpread1.Sheets[0].Columns[colCnt].HorizontalAlign = HorizontalAlign.Left;
                        break;
                    case "Amount":
                        FpSpread1.Sheets[0].Columns[colCnt].HorizontalAlign = HorizontalAlign.Right;
                        break;
                    case "Narration":
                        FpSpread1.Sheets[0].Columns[colCnt].HorizontalAlign = HorizontalAlign.Left;
                        break;
                }
                #endregion
            }
            #endregion
            int rowNo = 0;
            ArrayList arLoop = new ArrayList();
            if (cbPayment.Checked)
                arLoop.Add("0");
            if (cbReceipt.Checked)
                arLoop.Add("1");
            int totArCnt = 0;
            int.TryParse(Convert.ToString(arLoop.Count), out totArCnt);
            foreach (string strCount in arLoop)
            {
                int tablCnt = 0;
                int rowCnt = 0;
                int.TryParse(strCount, out tablCnt);
                try
                {
                    int colCnt = 0;
                    if (totArCnt == 1 && cbReceipt.Checked)
                    {
                        tablCnt = 0;
                        colCnt = 1;
                    }
                    else
                        colCnt = tablCnt;
                    if (dsVal.Tables[tablCnt].Rows.Count == 0)
                        continue;
                    switch (colCnt)
                    {
                        case 0:
                            FpSpread1.Sheets[0].RowCount++;
                            rowCnt = FpSpread1.Sheets[0].RowCount - 1;
                            FpSpread1.Sheets[0].Cells[rowCnt, 0].Text = "Voucher-" + Convert.ToString(txt_fromdate.Text) + "-" + Convert.ToString(txt_todate.Text);
                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].Rows.Count - 1, 0, 1, FpSpread1.Sheets[0].ColumnCount - 1);
                            break;
                        case 1:
                            FpSpread1.Sheets[0].RowCount++;
                            rowCnt = FpSpread1.Sheets[0].RowCount - 1;
                            FpSpread1.Sheets[0].Cells[rowCnt, 0].Text = "Receipt-" + Convert.ToString(txt_fromdate.Text) + "-" + Convert.ToString(txt_todate.Text);
                            FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].Rows.Count - 1, 0, 1, FpSpread1.Sheets[0].ColumnCount - 1);
                            break;
                    }
                }
                catch
                {
                    continue;

                }
                for (int row = 0; row < dsVal.Tables[tablCnt].Rows.Count; row++)
                {
                    #region
                    FpSpread1.Sheets[0].RowCount++;
                    rowCnt = FpSpread1.Sheets[0].RowCount - 1;
                    bool boolCheque = false;
                    foreach (KeyValuePair<string, string> header in dtHeader)
                    {
                        fpheight += 50;
                        int colCnt = 0;
                        string colName = Convert.ToString(header.Key);
                        int.TryParse(Convert.ToString(dtCol[colName]), out colCnt);
                        if (Convert.ToString(header.Key) == "Sno")
                            FpSpread1.Sheets[0].Cells[rowCnt, colCnt].Text = Convert.ToString(++rowNo);
                        else
                        {
                            string hdName = string.Empty;
                            string strValue = string.Empty;
                            switch (colName)
                            {
                                case "Header Name":
                                    hdName = Convert.ToString(dsVal.Tables[tablCnt].Rows[row]["headerfk"]);
                                    strValue = Convert.ToString(htHeaderName[hdName]);
                                    break;
                                case "Ledger Name":
                                    hdName = Convert.ToString(dsVal.Tables[tablCnt].Rows[row]["ledgerfk"]);
                                    strValue = Convert.ToString(htLedgerName[hdName]);
                                    break;

                                case "Supplier Name":
                                    hdName = Convert.ToString(dsVal.Tables[tablCnt].Rows[row]["app_no"]);
                                    strValue = Convert.ToString(htSupplierName[hdName]);
                                    break;
                                case "Student Name"://modified by abarna 7.05.2018
                                    hdName = Convert.ToString(dsVal.Tables[tablCnt].Rows[row]["app_no"]);
                                    strValue = Convert.ToString(htSupplierName[hdName]);
                                    break;
                                case "Amount":
                                    hdName = Convert.ToString(dsVal.Tables[tablCnt].Rows[row]["Debit"]);
                                    strValue = hdName;
                                    double tempPaid = 0;
                                    double.TryParse(strValue, out tempPaid);
                                    if (!httotal.ContainsKey("Amount"))
                                        httotal.Add("Amount", tempPaid);
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(httotal["Amount"]), out amount);
                                        amount += tempPaid;
                                        httotal.Remove("Amount");
                                        httotal.Add("Amount", Convert.ToString(amount));
                                    }
                                    break;
                            }
                            //if (hdName == string.Empty)
                            //    hdName = colName;
                            if (rbstud.Checked == true)
                            {
                                if (colName != "Student Name" && strValue == string.Empty)
                                    strValue = Convert.ToString(dsVal.Tables[tablCnt].Rows[row][colName]);
                            }
                            else
                            {
                                if (colName != "Supplier Name" && strValue == string.Empty)
                                    strValue = Convert.ToString(dsVal.Tables[tablCnt].Rows[row][colName]);
                            }
                            //else
                            //    strValue = hdName;
                            if (colName == "Cheque No" && strValue != string.Empty)
                                boolCheque = true;
                            FpSpread1.Sheets[0].Cells[rowCnt, colCnt].Text = strValue;
                            if (colName == "Cheque Date")
                            {
                                if (boolCheque)
                                    FpSpread1.Sheets[0].Cells[rowCnt, colCnt].Text = strValue;
                                else
                                    FpSpread1.Sheets[0].Cells[rowCnt, colCnt].Text = string.Empty;
                            }

                        }
                    }
                    #endregion
                }
                if (httotal.Count > 0)
                {
                    #region grandtotal
                    //FpSpread1.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
                    FpSpread1.Sheets[0].Rows.Count++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].Rows.Count - 1, 0, 1, 3);
                    double grandvalue = 0;
                    //for (int j = 2; j < FpSpread1.Sheets[0].ColumnCount; j++)
                    //{
                    int colCnt = 0;
                    string colName = "Amount";
                    int.TryParse(Convert.ToString(dtCol[colName]), out colCnt);
                    double.TryParse(Convert.ToString(httotal[colName]), out grandvalue);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, colCnt].Text = Convert.ToString(grandvalue);
                    httotal.Clear();
                    // }
                    #endregion
                }
            }
            FpSpread1.Width = 1300;
            //FpSpread1.Height = Convert.ToInt32(fpheight);
            FpSpread1.ShowHeaderSelection = false;
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            FpSpread1.SaveChanges();
            output.Visible = true;
            print.Visible = true;
            // divspread.Visible = true;
            FpSpread1.Visible = true;
        }
        catch
        {
            FpSpread1.Visible = false;
            //  divspread.Visible = false;
            print.Visible = false;
        }
    }
    protected Hashtable getHDName()
    {
        Hashtable hthdName = new Hashtable();
        try
        {
            string selQFK = string.Empty;
            selQFK = "  select distinct headerpk as pk,headername as name from fm_headermaster where collegecode in('" + ddl_collegename.SelectedValue + "') ";
            DataSet dsval = d2.select_method_wo_parameter(selQFK, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsval.Tables[0].Rows.Count; row++)
                {
                    if (!hthdName.ContainsKey(Convert.ToString(dsval.Tables[0].Rows[row]["pk"])))
                        hthdName.Add(Convert.ToString(dsval.Tables[0].Rows[row]["pk"]), Convert.ToString(dsval.Tables[0].Rows[row]["name"]));
                }
            }
        }
        catch { hthdName.Clear(); }
        return hthdName;
    }
    protected Hashtable getLDName()
    {
        Hashtable hthdName = new Hashtable();
        try
        {
            string selQFK = string.Empty;
            selQFK = "  select ledgerpk as pk,ledgername as name from fm_ledgermaster where collegecode in('" + ddl_collegename.SelectedValue + "') ";
            DataSet dsval = d2.select_method_wo_parameter(selQFK, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsval.Tables[0].Rows.Count; row++)
                {
                    if (!hthdName.ContainsKey(Convert.ToString(dsval.Tables[0].Rows[row]["pk"])))
                        hthdName.Add(Convert.ToString(dsval.Tables[0].Rows[row]["pk"]), Convert.ToString(dsval.Tables[0].Rows[row]["name"]));
                }
            }
        }
        catch { hthdName.Clear(); }
        return hthdName;
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
    private string getCblSelectedTextwithout(CheckBoxList cblSelected)
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
}