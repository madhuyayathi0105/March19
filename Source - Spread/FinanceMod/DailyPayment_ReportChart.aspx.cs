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

public partial class DailyPayment_ReportChart : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataSet dsload = new DataSet();
    DataSet dschart = new DataSet();
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
    int sel = 0;
    int row = 0;
    int col = 0;
    int check = 0;
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
                rbheader.Checked = true;
                rbledger.Checked = false;
            }
            else if (ddlacctype.SelectedItem.Value == "3")
            {
                loadheaderandledger();
                rbheader.Checked = false;
                rbledger.Checked = true;

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

    //public void bindsem()
    //{
    //    string sem = "";
    //    cbl_sem.Items.Clear();

    //    string settingquery = "select * from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
    //    ds.Clear();
    //    ds = d2.select_method_wo_parameter(settingquery, "Text");
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        string linkvalue = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
    //        if (linkvalue == "0")
    //        {
    //            string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Semester' and textval not like '-1%' and college_code ='" + collegecode1 + "' order by textval asc";
    //            ds.Clear();
    //            ds = d2.select_method_wo_parameter(semesterquery, "Text");
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                //text_circode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
    //                cbl_sem.DataSource = ds;
    //                cbl_sem.DataTextField = "TextVal";
    //                cbl_sem.DataValueField = "TextCode";
    //                cbl_sem.DataBind();
    //            }
    //            if (cbl_sem.Items.Count > 0)
    //            {
    //                for (int i = 0; i < cbl_sem.Items.Count; i++)
    //                {
    //                    cbl_sem.Items[i].Selected = true;
    //                    sem = Convert.ToString(cbl_sem.Items[i].Text);
    //                }
    //                if (cbl_sem.Items.Count == 1)
    //                {
    //                    txt_sem.Text = "Semester(" + sem + ")";
    //                }
    //                else
    //                {
    //                    txt_sem.Text = "Semester(" + cbl_sem.Items.Count + ")";
    //                }
    //                cb_sem.Checked = true;
    //            }
    //        }
    //        else
    //        {
    //            string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Year' and textval not like '-1%' and college_code ='" + collegecode1 + "'";
    //            ds.Clear();
    //            ds = d2.select_method_wo_parameter(semesterquery, "Text");
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                // text_circode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
    //                cbl_sem.DataSource = ds;
    //                cbl_sem.DataTextField = "TextVal";
    //                cbl_sem.DataValueField = "TextCode";
    //                cbl_sem.DataBind();
    //            }
    //            if (cbl_sem.Items.Count > 0)
    //            {
    //                for (int i = 0; i < cbl_sem.Items.Count; i++)
    //                {
    //                    cbl_sem.Items[i].Selected = true;
    //                    sem = Convert.ToString(cbl_sem.Items[i].Text);
    //                }
    //                if (cbl_sem.Items.Count == 1)
    //                {
    //                    txt_sem.Text = "Semester(" + sem + ")";
    //                }
    //                else
    //                {
    //                    txt_sem.Text = "Semester(" + cbl_sem.Items.Count + ")";
    //                }
    //                cb_sem.Checked = true;
    //            }
    //        }
    //    }
    //}
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
            ddlacctype.Items.Add(new ListItem("--Select--", "0"));
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
            string query1 = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + "  and L.HeaderFK in('" + headerid + "')  AND  Ledgermode='1' and L.CollegeCode = " + collegecode1 + "";
            // string query1 = " select LedgerPK,LedgerName from FM_LedgerMaster where CollegeCode='" + collegecode1 + "' and LedgerMode=1 and HeaderFK in('" + headerid + "')  order by isnull(priority,1000), ledgerName asc ";
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
            ds.Clear();
            if (rbvendor.Checked == true)
            {
                select = " select VendorPK,VendorCompName from CO_VendorMaster where VendorType='1' and VendorPK in('" + vendorpk + "')";
            }
            else if (rbother.Checked == true)
            {
                select = " select VendorPK,VendorCompName from CO_VendorMaster where VendorType='-5' and VendorPK in('" + vendorpk + "')";
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
        divspread.Visible = false;
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
        //chart

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
        divspread.Visible = false;
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
        //chart

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
        divspread.Visible = false;
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
        //chart

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
        divspread.Visible = false;
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
        //chart

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

            int value = 0;
            int txtval = 0;
            int headorledg = 0;
            ds.Clear();
            ds = loaddataset();
            if (ddlacctype.SelectedItem.Value == "2")
            {
                if (rbheader.Checked == true)
                {
                    headorledg = 2;
                }
            }
            else if (ddlacctype.SelectedItem.Value == "3")
            {
                if (rbledger.Checked == true)
                {
                    headorledg = 3;
                }
            }
            if (ddlacctype.SelectedItem.Value != "0")
            {
                if (cbldetail.Checked == false)
                    value = 1;
                else
                    value = 2;
                if (txtsearch.Text == "")
                    txtval = 1;
                else
                    txtval = 2;
                if (rbstud.Checked == true)
                {
                    check = 1;
                    if (value == 1)
                    {
                        if (txtval == 1)
                        {
                            CommonChart();
                        }
                        else
                        {
                            IndividualChart();
                        }
                    }
                    else
                    {
                        if (headorledg == 2)
                        {
                            if (txtval == 1)
                            {
                                CommonHeaderChart();
                            }
                            else
                            {
                                IndividualHeaderChart();
                            }
                        }
                        if (headorledg == 3)
                        {
                            if (txtval == 1)
                            {
                                CommonLedgerChart();
                            }
                            else
                            {
                                IndividualLedgerChart();
                            }
                        }
                    }
                }
                else if (rbstaff.Checked == true)
                {
                    check = 2;
                    if (value == 1)
                    {
                        if (txtval == 1)
                        {
                            CommonChart();
                        }
                        else
                        {
                            IndividualChart();
                        }
                    }
                    else
                    {
                        if (headorledg == 2)
                        {
                            if (txtval == 1)
                            {
                                CommonHeaderChart();
                            }
                            else
                            {
                                IndividualHeaderChart();
                            }
                        }
                        if (headorledg == 3)
                        {
                            if (txtval == 1)
                            {
                                CommonLedgerChart();
                            }
                            else
                            {
                                IndividualLedgerChart();
                            }
                        }
                    }
                }
                else if (rbvendor.Checked == true)
                {
                    check = 3;
                    if (value == 1)
                    {
                        if (txtval == 1)
                        {
                            CommonChart();
                        }
                        else
                        {
                            IndividualChart();
                        }
                    }
                    else
                    {
                        if (headorledg == 2)
                        {
                            if (txtval == 1)
                            {
                                CommonHeaderChart();
                            }
                            else
                            {
                                IndividualHeaderChart();
                            }
                        }
                        if (headorledg == 3)
                        {
                            if (txtval == 1)
                            {
                                CommonLedgerChart();
                            }
                            else
                            {
                                IndividualLedgerChart();
                            }
                        }
                    }
                }
                else if (rbother.Checked == true)
                {
                    check = 4;
                    if (value == 1)
                    {
                        if (txtval == 1)
                        {
                            CommonChart();
                        }
                        else
                        {
                            IndividualChart();
                        }
                    }
                    else
                    {
                        if (headorledg == 2)
                        {
                            if (txtval == 1)
                            {
                                CommonHeaderChart();
                            }
                            else
                            {
                                IndividualHeaderChart();
                            }
                        }
                        if (headorledg == 3)
                        {
                            if (txtval == 1)
                            {
                                CommonLedgerChart();
                            }
                            else
                            {
                                IndividualLedgerChart();
                            }
                        }
                    }
                }
            }
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
            if (rbstud.Checked == true)
            {
                batchyr = getCblSelectedValue(cbl_batch);
                courseid = getCblSelectedValue(cbl_dept);
                feecat = getCblSelectedValue(cbl_sem);
                sec = getCblSelectedValue(cbl_sect);
                finlyr = getCblSelectedValue(chklsfyear);
            }
            headerid = getCblSelectedValue(chkl_studhed);
            ledgerid = getCblSelectedValue(chkl_studled);
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

            // staff get values
            if (rbstaff.Checked == true)
            {
                staffdept = getCblSelectedValue(cbl_staffdept);
                staffdesg = getCblSelectedValue(cbl_staffdesg);
                stafftype = getCblSelectedValue(cbl_stafftype);
            }


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
            // vendor get values
            if (rbvendor.Checked == true || rbother.Checked == true)
            {

                vendorcode = getCblSelectedText(cbl_vendorcode);
                vendorname = getCblSelectedValue(cbl_vendorname);
                vendorcont = getCblSelectedValue(cbl_vendorcont);
            }


            if (ddlacctype.SelectedItem.Value == "1")
            {
                headerorledger = "";
            }
            else if (ddlacctype.SelectedItem.Value == "2")
            {
                if (cbldetail.Checked == false)
                {
                    headerorledger = " ,HeaderFK";
                }
                else
                {
                    headerorledger = " HeaderFK";
                }
            }
            else if (ddlacctype.SelectedItem.Value == "3")
            {
                if (cbldetail.Checked == false)
                {
                    headerorledger = ",LedgerFK";
                }
                else
                {
                    headerorledger = "LedgerFK";
                }

            }

            if (rbstud.Checked == true)
            {
                #region stud
                if (cbldetail.Checked == false)
                {
                    #region without cumulative
                    if (txtsearch.Text == "")
                    {
                        #region old
                        //SelectQ = " select Credit,TransCode,r.Roll_No,r.Reg_No,PayMode,r.Stud_Name,c.Course_Name +' - '+ dt.Dept_Name as Dept_Name ,CONVERT(varchar(20),TransDate,103) as  TransDate,f.App_no" + headerorledger + "  from FT_FinDailyTransaction f,Registration r,Degree d,Department dt,Course c where r.App_No=f.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and TransType ='2' ";
                        //if (batchyr != "")
                        //{
                        //    SelectQ = SelectQ + " and r.Batch_Year in ('" + batchyr + "')";
                        //}
                        //if (courseid != "")
                        //{
                        //    SelectQ = SelectQ + "  and  r.Degree_Code in ('" + courseid + "')";
                        //}
                        //if (feecat != "")
                        //{
                        //    // SelectQ = SelectQ + " and f.FeeCategory in ('" + feecat + "')";
                        //}
                        //if (sec != "")
                        //{
                        //    //SelectQ = SelectQ + " and   ISNULL( r.Sections,'') in ('" + sec + "','')";
                        //}

                        //if (headerid != "")
                        //{
                        //    SelectQ = SelectQ + " and HeaderFK in ('" + headerid + "')";
                        //}
                        //if (ledgerid != "")
                        //{
                        //    SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "')";
                        //}
                        //if (fromdate != "" && todate != "")
                        //{
                        //    SelectQ = SelectQ + "  AND TransDate between '" + fromdate + "' and '" + todate + "'";
                        //}
                        #endregion

                        #region new query
                        SelectQ = " select ISNULL( ISNULL( sum(Credit),0),0) as credit from FT_FinDailyTransaction f,Registration r,Degree d,Department dt,Course c where r.App_No=f.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and TransType ='2' ";
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
                        //SelectQ = SelectQ + " group by r.Roll_No,r.Reg_No,PayMode,r.Stud_Name,c.Course_Name +' - '+ dt.Dept_Name ,f.App_no";
                        SelectQ = SelectQ + "  select ISNULL( ISNULL( sum(Credit),0),0) as credit ,PayMode from FT_FinDailyTransaction f,Registration r,Degree d,Department dt,Course c where r.App_No=f.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and TransType ='2'";
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
                        SelectQ = SelectQ + " group by paymode";
                        #endregion
                    }
                    else if (txtsearch.Text != "")
                    {
                        #region new query
                        SelectQ = " select ISNULL( ISNULL( sum(Credit),0),0) as credit,r.Roll_No,r.Reg_No,PayMode,r.Stud_Name,c.Course_Name +' - '+ dt.Dept_Name as Dept_Name,f.App_no  from FT_FinDailyTransaction f,Registration r,Degree d,Department dt,Course c where r.App_No=f.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and TransType ='2' ";
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
                        SelectQ = SelectQ + " group by r.Roll_No,r.Reg_No,PayMode,r.Stud_Name,c.Course_Name +' - '+ dt.Dept_Name ,f.App_no";

                        SelectQ = SelectQ + " select ISNULL( ISNULL( sum(Credit),0),0) as credit,PayMode from FT_FinDailyTransaction f,Registration r,Degree d,Department dt,Course c where r.App_No=f.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and TransType ='2' ";
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
                        SelectQ = SelectQ + " group by PayMode";
                        #endregion
                    }
                    #endregion
                }
                else if (cbldetail.Checked == true)
                {
                    #region details
                    if (txtsearch.Text == "")
                    {

                        #region new query
                        SelectQ = " select ISNULL( sum(Credit),0) as credit," + headerorledger + " from FT_FinDailyTransaction f,Registration r,Degree d,Department dt,Course c where r.App_No=f.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and TransType ='2' ";
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
                        SelectQ = SelectQ + " group by " + headerorledger + "";
                        SelectQ = SelectQ + "  select ISNULL( sum(Credit),0) as credit ,PayMode," + headerorledger + " from FT_FinDailyTransaction f,Registration r,Degree d,Department dt,Course c where r.App_No=f.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and TransType ='2'";
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
                        SelectQ = SelectQ + " group by paymode," + headerorledger + "";
                        SelectQ = SelectQ + "  select HeaderFK,LedgerPK,HeaderName,LedgerName from FM_HeaderMaster h,FM_LedgerMaster l where l.HeaderFK =h.HeaderPK and h.CollegeCode ='" + collegecode1 + "'  order by isnull(l.priority,1000), l.ledgerName asc ";
                        #endregion
                    }
                    else if (txtsearch.Text != "")
                    {
                        #region new query
                        SelectQ = " select ISNULL( sum(Credit),0) as credit," + headerorledger + " from FT_FinDailyTransaction f,Registration r,Degree d,Department dt,Course c where r.App_No=f.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and TransType ='2' ";
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
                        SelectQ = SelectQ + " group by " + headerorledger + "";

                        SelectQ = SelectQ + " select ISNULL( sum(Credit),0) as credit,PayMode," + headerorledger + " from FT_FinDailyTransaction f,Registration r,Degree d,Department dt,Course c where r.App_No=f.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and TransType ='2' ";
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
                        SelectQ = SelectQ + " group by PayMode," + headerorledger + "";
                        SelectQ = SelectQ + "  select HeaderFK,LedgerPK,HeaderName,LedgerName from FM_HeaderMaster h,FM_LedgerMaster l where l.HeaderFK =h.HeaderPK and h.CollegeCode ='" + collegecode1 + "' order by isnull(l.priority,1000), l.ledgerName asc ";
                        #endregion
                    }
                    #endregion
                }
                #endregion
            }
            else if (rbstaff.Checked == true)
            {
                #region staff
                if (cbldetail.Checked == false)
                {
                    #region without cumulative
                    if (txtsearch.Text == "")
                    {
                        SelectQ = " select ISNULL( ISNULL( sum(Credit),0),0) as Credit from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and h.dept_code =sa.dept_code and d.desig_code =sa.desig_code and T.staff_code =s.staff_code and T.dept_code =h.dept_code and T.desig_code =D.desig_code and T.latestrec ='1'";
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

                        SelectQ = SelectQ + " select  ISNULL( sum(Credit),0) as credit,f.App_no,f.Paymode from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and h.dept_code =sa.dept_code and d.desig_code =sa.desig_code and T.staff_code =s.staff_code and T.dept_code =h.dept_code and T.desig_code =D.desig_code and T.latestrec ='1' ";
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
                        SelectQ = SelectQ + " group by f.App_no,sa.appl_id ,f.Paymode ";
                    }
                    else if (txtsearch.Text != "")
                    {

                        #region new qry
                        SelectQ = " select  sum (Credit) as credit,f.App_no,sa.appl_id,s.staff_code,s.staff_name,sa.Dept_Code,h.dept_name,D.desig_name,d.desig_code from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and h.dept_code =sa.dept_code and d.desig_code =sa.desig_code and T.staff_code =s.staff_code and T.dept_code =h.dept_code and T.desig_code =D.desig_code and T.latestrec ='1'";
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
                        SelectQ = SelectQ + "  group by f.App_no,sa.appl_id,s.staff_code,s.staff_name,sa.Dept_Code,h.dept_name,D.desig_name,d.desig_code";
                        SelectQ = SelectQ + "  select  ISNULL( sum(Credit),0) as credit,f.paymode,f.App_no from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and h.dept_code =sa.dept_code and d.desig_code =sa.desig_code and T.staff_code =s.staff_code and T.dept_code =h.dept_code and T.desig_code =D.desig_code and T.latestrec ='1'";
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
                        SelectQ = SelectQ + " group by f.App_no,sa.appl_id ,f.paymode";
                        #endregion
                    }
                    #endregion
                }
                else if (cbldetail.Checked == true)
                {
                    #region Details
                    if (txtsearch.Text == "")
                    {
                        SelectQ = " select  ISNULL( sum(Credit),0) as Credit," + headerorledger + " from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and h.dept_code =sa.dept_code and d.desig_code =sa.desig_code and T.staff_code =s.staff_code and T.dept_code =h.dept_code and T.desig_code =D.desig_code and T.latestrec ='1'";
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
                        SelectQ = SelectQ + "group by " + headerorledger + " ";
                        SelectQ = SelectQ + " select  ISNULL( sum(Credit),0) as credit,f.App_no,f.Paymode," + headerorledger + " from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and h.dept_code =sa.dept_code and d.desig_code =sa.desig_code and T.staff_code =s.staff_code and T.dept_code =h.dept_code and T.desig_code =D.desig_code and T.latestrec ='1' ";
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
                        SelectQ = SelectQ + " group by f.App_no,sa.appl_id ,f.Paymode," + headerorledger + " ";
                        SelectQ = SelectQ + "  select HeaderFK,LedgerPK,HeaderName,LedgerName from FM_HeaderMaster h,FM_LedgerMaster l where l.HeaderFK =h.HeaderPK and h.CollegeCode ='" + collegecode1 + "' order by isnull(l.priority,1000), l.ledgerName asc ";
                    }
                    else if (txtsearch.Text != "")
                    {

                        #region new qry
                        SelectQ = " select  sum (Credit) as credit," + headerorledger + " from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and h.dept_code =sa.dept_code and d.desig_code =sa.desig_code and T.staff_code =s.staff_code and T.dept_code =h.dept_code and T.desig_code =D.desig_code and T.latestrec ='1'";
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
                        SelectQ = SelectQ + "  group by " + headerorledger + "";
                        SelectQ = SelectQ + "  select  ISNULL( sum(Credit),0) as credit,f.paymode,f.App_no," + headerorledger + " from FT_FinDailyTransaction f,staff_appl_master sa,staffmaster s,hrdept_master h,desig_master d,stafftrans T where sa.appl_no =s.appl_no and f.App_No =sa.appl_id and h.dept_code =sa.dept_code and d.desig_code =sa.desig_code and T.staff_code =s.staff_code and T.dept_code =h.dept_code and T.desig_code =D.desig_code and T.latestrec ='1'";
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
                        SelectQ = SelectQ + " group by f.App_no,sa.appl_id ,f.paymode," + headerorledger + "";
                        SelectQ = SelectQ + "  select HeaderFK,LedgerPK,HeaderName,LedgerName from FM_HeaderMaster h,FM_LedgerMaster l where l.HeaderFK =h.HeaderPK and h.CollegeCode ='" + collegecode1 + "' order by isnull(l.priority,1000), l.ledgerName asc ";
                        #endregion
                    }
                    #endregion
                }
                #endregion
            }
            else if (rbvendor.Checked == true)
            {
                #region vendor
                if (cbldetail.Checked == false)
                {
                    #region without cumulative
                    if (txtsearch.Text == "")
                    {
                        SelectQ = "  SELECT ISNULL( sum(Credit),0) as Credit  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1'";
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
                        //SelectQ = SelectQ + "  group by p.VendorCode,f.App_no,p.VendorCompName,vc.VendorContactPK,vc.VenContactName ";
                        SelectQ = SelectQ + " SELECT ISNULL( sum(Credit),0) as Credit,f.Paymode  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1'";
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
                        SelectQ = SelectQ + " group by f.Paymode";
                    }
                    else if (txtsearch.Text != "")
                    {
                        SelectQ = "  SELECT ISNULL( sum(Credit),0) as Credit,p.VendorCode,f.App_no,p.VendorCompName,vc.VendorContactPK,vc.VenContactName,p.VendorName  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1'";
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
                        SelectQ = SelectQ + "  group by p.VendorCode,f.App_no,p.VendorCompName,vc.VendorContactPK,vc.VenContactName,p.VendorName ";
                        SelectQ = SelectQ + " SELECT ISNULL( sum(Credit),0) as Credit,f.App_no,vc.VendorContactPK,f.Paymode FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1'";
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
                        SelectQ = SelectQ + " group by f.App_no,vc.VendorContactPK,f.Paymode";
                    }
                    #endregion
                }
                else if (cbldetail.Checked == true)
                {
                    #region Details
                    if (txtsearch.Text == "")
                    {
                        SelectQ = "  SELECT ISNULL( sum(Credit),0) as Credit," + headerorledger + "  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1'";
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
                        SelectQ = SelectQ + "  group by " + headerorledger + " ";
                        SelectQ = SelectQ + " SELECT ISNULL( sum(Credit),0) as Credit,f.Paymode," + headerorledger + "  FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1'";
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
                        SelectQ = SelectQ + " group by f.Paymode," + headerorledger + "";
                        SelectQ = SelectQ + "  select HeaderFK,LedgerPK,HeaderName,LedgerName from FM_HeaderMaster h,FM_LedgerMaster l where l.HeaderFK =h.HeaderPK and h.CollegeCode ='" + collegecode1 + "' order by isnull(l.priority,1000), l.ledgerName asc ";
                    }
                    else if (txtsearch.Text != "")
                    {

                        SelectQ = "  SELECT ISNULL( sum(Credit),0) as Credit," + headerorledger + " FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1'";
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
                        SelectQ = SelectQ + "  group by " + headerorledger + " ";
                        SelectQ = SelectQ + " SELECT ISNULL( sum(Credit),0) as Credit,f.App_no,vc.VendorContactPK,f.Paymode," + headerorledger + " FROM FT_FinDailyTransaction f,CO_VendorMaster P,IM_VendorContactMaster VC WHERE p.VendorPK =VC.VendorFK and VC.VendorContactPK =F.App_No and P.VendorType ='1'";
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
                        SelectQ = SelectQ + " group by f.App_no,vc.VendorContactPK,f.Paymode," + headerorledger + "";
                        SelectQ = SelectQ + "  select HeaderFK,LedgerPK,HeaderName,LedgerName from FM_HeaderMaster h,FM_LedgerMaster l where l.HeaderFK =h.HeaderPK and h.CollegeCode ='" + collegecode1 + "' order by isnull(l.priority,1000), l.ledgerName asc ";
                    }
                    #endregion
                }
                #endregion
            }
            else if (rbother.Checked == true)
            {
                #region others
                if (cbldetail.Checked == false)
                {
                    #region without cumulative
                    if (txtsearch.Text == "")
                    {
                        SelectQ = " SELECT ISNULL( sum(Credit),0) as credit FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5'";
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
                        //  SelectQ = SelectQ + " group by p.VendorPK,p.VendorCode,f.App_no,p.VendorCompName";
                        SelectQ = SelectQ + " SELECT ISNULL( sum(Credit),0) as Credit,f.Paymode FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5' ";
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
                        SelectQ = SelectQ + " group by f.Paymode";
                    }
                    else if (txtsearch.Text != "")
                    {

                        SelectQ = " SELECT ISNULL( sum(Credit),0) as credit,p.VendorPK,p.VendorCode,f.App_no,p.VendorCompName,p.VendorName FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5'";
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
                        SelectQ = SelectQ + " group by p.VendorPK,p.VendorCode,f.App_no,p.VendorCompName,p.VendorName";
                        SelectQ = SelectQ + " SELECT ISNULL( sum(Credit),0) as Credit,p.VendorPK,f.App_no,f.Paymode FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5' ";
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
                        SelectQ = SelectQ + " group by p.VendorPK,f.App_no,f.Paymode";
                    }
                    #endregion
                }
                else if (cbldetail.Checked == true)
                {
                    #region details
                    if (txtsearch.Text == "")
                    {
                        SelectQ = " SELECT ISNULL( sum(Credit),0) as credit," + headerorledger + " FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5'";
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
                        SelectQ = SelectQ + " group by " + headerorledger + "";
                        SelectQ = SelectQ + " SELECT ISNULL( sum(Credit),0) as Credit,f.Paymode," + headerorledger + " FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5' ";
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
                        SelectQ = SelectQ + " group by f.Paymode," + headerorledger + "";
                        SelectQ = SelectQ + "  select HeaderFK,LedgerPK,HeaderName,LedgerName from FM_HeaderMaster h,FM_LedgerMaster l where l.HeaderFK =h.HeaderPK and h.CollegeCode ='" + collegecode1 + "' order by isnull(l.priority,1000), l.ledgerName asc ";
                    }
                    else if (txtsearch.Text != "")
                    {

                        SelectQ = " SELECT ISNULL( sum(Credit),0) as credit," + headerorledger + " FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5'";
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
                        SelectQ = SelectQ + " group by " + headerorledger + "";
                        SelectQ = SelectQ + " SELECT ISNULL( sum(Credit),0) as Credit,p.VendorPK,f.App_no,f.Paymode," + headerorledger + " FROM FT_FinDailyTransaction f,CO_VendorMaster P WHERE p.VendorPK=F.App_No and P.VendorType ='-5' ";
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
                        SelectQ = SelectQ + " group by p.VendorPK,f.App_no,f.Paymode," + headerorledger + "";
                        SelectQ = SelectQ + "  select HeaderFK,LedgerPK,HeaderName,LedgerName from FM_HeaderMaster h,FM_LedgerMaster l where l.HeaderFK =h.HeaderPK and h.CollegeCode ='" + collegecode1 + "' order by isnull(l.priority,1000), l.ledgerName asc ";
                    }
                    #endregion
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

    public void studvalues()
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
                        DataView dv = new DataView();
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
                            fpheight += 50;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread1.Sheets[0].RowCount);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["Roll_No"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = rollno;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["Reg_No"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = regno;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["TransCode"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["TransDate"]);
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
                        DataView dv = new DataView();
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
                            fpheight += 50;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread1.Sheets[0].RowCount);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["Roll_No"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = rollno;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["Reg_No"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = regno;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
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
                    divspread.Visible = true;
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

    public void staffvalues()
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
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["staff_code"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = rollno;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["staff_name"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = regno;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["dept_name"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["desig_name"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["TransCode"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["TransDate"]);

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
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["staff_code"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = rollno;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["staff_name"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = regno;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["dept_name"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["desig_name"]);


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
                    divspread.Visible = true;
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

    public void vendorvalues()
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
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorCode"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = rollno;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["VendorCompName"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = regno;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["VenContactName"]);

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["TransCode"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["TransDate"]);
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
                        FpSpread1.Sheets[0].ColumnCount = 4;
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


                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);

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
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 1, count);
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Paid";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
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
                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].Rows.Count - 1, 0, 1, 4);
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
                    divspread.Visible = true;
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

    public void othervalues()
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


                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
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

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["TransCode"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["TransDate"]);
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
                        FpSpread1.Sheets[0].ColumnCount = 3;
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


                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);

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
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 1, count);
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Paid";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
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
                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].Rows.Count - 1, 0, 1, 5);
                        double hedval = 0;
                        for (int j = 3; j < FpSpread1.Sheets[0].Columns.Count; j++)
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
                    divspread.Visible = true;
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


    #region chart method
    private void IndividualChart()
    {
        try
        {
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string name = "";
                DataView dv = new DataView();
                List<string> list = new List<string>();
                DataTable dtchart = new DataTable();
                DataColumn dtcol = new DataColumn();
                DataColumn dtcol1 = new DataColumn();
                chart.ChartAreas[0].AxisX.Title = "Individual";
                chart.ChartAreas[0].AxisY.Title = "Amount";
                chart.ChartAreas[0].AxisX.TitleForeColor = Color.Red;
                chart.ChartAreas[0].AxisY.TitleForeColor = Color.Red;
                chart.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Center;
                chart.ChartAreas[0].AxisY.TitleAlignment = StringAlignment.Center;
                chart.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
                chart.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
                chart.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Book Antiqua", 15, FontStyle.Bold);
                chart.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Blue;
                chart.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Book Antiqua", 15, FontStyle.Bold);
                chart.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Blue;
                chart.ChartAreas[0].AxisX.Interval = 1;
                chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                chart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                dtchart.Columns.Clear();
                dtchart.Columns.Add(dtcol1);
                for (sel = 0; sel < ds.Tables[0].Rows.Count; sel++)
                {
                    if (check == 1)
                    {
                        name = Convert.ToString(ds.Tables[0].Rows[sel]["Stud_Name"]);
                    }
                    else if (check == 2)
                    {
                        name = Convert.ToString(ds.Tables[0].Rows[sel]["staff_name"]);
                    }
                    else if (check == 3)
                    {
                        name = Convert.ToString(ds.Tables[0].Rows[sel]["VendorName"]);
                    }
                    else if (check == 4)
                    {
                        name = Convert.ToString(ds.Tables[0].Rows[sel]["VendorName"]);
                    }
                    dtcol.ColumnName = name;
                    dtchart.Columns.Add(dtcol);
                }
                double totamt = 0;
                string cash = "";
                string cheque = "";
                if (dtchart.Columns.Count > 1)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {

                            string totalamt = Convert.ToString(ds.Tables[0].Rows[i]["Credit"]);
                            totamt += Convert.ToDouble(totalamt);
                            string paymode = Convert.ToString(ds.Tables[1].Rows[i]["Paymode"]);
                            ds.Tables[1].DefaultView.RowFilter = "Paymode='" + paymode + "'";
                            dv = ds.Tables[0].DefaultView;
                            if (dv.Count > 0 && dv != null)
                            {
                                if (paymode == "1")
                                    cash = Convert.ToString(dv[0]["credit"]);
                                if (paymode == "2")
                                    cheque = Convert.ToString(dv[0]["credit"]);
                            }
                        }
                        DataRow dtrow;
                        dtrow = dtchart.NewRow();
                        dtrow[0] = "Total Paid";
                        dtrow[1] = Convert.ToString(totamt);
                        chart.Series.Add("Total Paid");

                        DataRow dtrow1;
                        dtrow1 = dtchart.NewRow();
                        dtrow1[0] = "Cash";
                        dtrow1[1] = Convert.ToString(cash);
                        chart.Series.Add("Cash");

                        DataRow dtrow2;
                        dtrow2 = dtchart.NewRow();
                        dtrow2[0] = "Cheque";
                        dtrow2[1] = Convert.ToString(cheque);
                        chart.Series.Add("Cheque");

                        dtchart.Rows.Add(dtrow);
                        dtchart.Rows.Add(dtrow1);
                        dtchart.Rows.Add(dtrow2);

                    }

                }
                if (dtchart.Rows.Count > 0)
                {
                    for (col = 1; col < dtchart.Columns.Count; col++)
                    {
                        for (row = 0; row < dtchart.Rows.Count; row++)
                        {
                            chart.Series[row].Points.AddXY(dtchart.Columns[col].ToString(), dtchart.Rows[row][col].ToString());
                            chart.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                            chart.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;

                            chart.Series[row].IsValueShownAsLabel = true;
                            chart.Series[row].IsXValueIndexed = true;

                            chart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                            chart.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                        }
                    }
                    chart.Visible = true;
                    chart.Height = 400;
                    chart.Width = 800;

                }
            }
            else
            {
                chart.Visible = false;
                lbl_alert.Visible = true;
                pupdiv.Visible = true;
                output.Text = "";
                lbl_alert.Text = "No Record Found";
            }

        }
        catch { }
    }

    private void CommonChart()
    {
        try
        {
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                string fromdate = "";
                string todates = "";
                int val = 0;
                DataView dv = new DataView();
                List<string> list = new List<string>();
                DataTable dtchart = new DataTable();
                DataColumn dtcol = new DataColumn();
                DataColumn dtcol1 = new DataColumn();
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                chart.ChartAreas[0].AxisX.Title = "Common";
                chart.ChartAreas[0].AxisY.Title = "Amount";
                chart.ChartAreas[0].AxisX.TitleForeColor = Color.Red;
                chart.ChartAreas[0].AxisY.TitleForeColor = Color.Red;
                chart.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Center;
                chart.ChartAreas[0].AxisY.TitleAlignment = StringAlignment.Center;
                chart.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
                chart.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
                chart.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Book Antiqua", 15, FontStyle.Bold);
                chart.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Blue;
                chart.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Book Antiqua", 15, FontStyle.Bold);
                chart.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Blue;
                chart.ChartAreas[0].AxisX.Interval = 1;
                chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                chart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

                fromdate = Convert.ToString(txt_fromdate.Text);
                todates = Convert.ToString(txt_todate.Text);
                //mo chart.Series.Clear();
                string[] frdate = fromdate.Split('/');
                if (frdate.Length == 3)
                {
                    fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
                    dt = Convert.ToDateTime(fromdate);
                }
                string[] tdate = todates.Split('/');
                if (tdate.Length == 3)
                {
                    todates = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
                    dt1 = Convert.ToDateTime(todates);
                }
                if (dt == dt1)
                {
                    val = 1;
                }

                dtchart.Columns.Clear();
                dtchart.Columns.Add(dtcol1);
                dtchart.Columns.Add("Total Paid");
                dtchart.Columns.Add("Cash");
                dtchart.Columns.Add("Cheque");
                if (val == 1)
                {
                    chart.Series.Add("Total Paid");
                    chart.Series.Add("Cash");
                    chart.Series.Add("Cheque");
                }
                else
                {
                    chart.Series.Add(dt.ToString("dd/MM/yyyy") + "-" + dt1.ToString("dd/MM/yyyy"));
                    //  chart.Series.Add(dt1.ToString("dd/MM/yyyy"));
                }


                double totamt = 0;
                string cash = "";
                string cheque = "";
                if (dtchart.Columns.Count > 1)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string totalamt = Convert.ToString(ds.Tables[0].Rows[i]["Credit"]);
                            totamt = Convert.ToDouble(totalamt);
                            for (row = 0; row < ds.Tables[1].Rows.Count; row++)
                            {
                                string paymode = Convert.ToString(ds.Tables[1].Rows[i]["Paymode"]);
                                ds.Tables[1].DefaultView.RowFilter = "Paymode='" + paymode + "'";
                                dv = ds.Tables[0].DefaultView;
                                if (dv.Count > 0 && dv != null)
                                {
                                    if (paymode == "1")
                                        cash = Convert.ToString(dv[0]["credit"]);
                                    if (paymode == "2")
                                        cheque = Convert.ToString(dv[0]["credit"]);
                                }
                            }
                        }
                        DataRow dtrow;
                        dtrow = dtchart.NewRow();
                        dtrow["Total Paid"] = totamt;
                        dtrow["Cash"] = cash;
                        dtrow["Cheque"] = cheque;
                        dtchart.Rows.Add(dtrow);
                    }

                }
                if (dtchart.Rows.Count > 0)
                {

                    for (row = 0; row < dtchart.Rows.Count; row++)
                    {
                        for (col = 1; col < dtchart.Columns.Count; col++)
                        {
                            chart.Series[row].Points.AddXY(dtchart.Columns[col].ToString(), dtchart.Rows[row][col].ToString());
                            chart.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                            chart.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;

                            chart.Series[row].IsValueShownAsLabel = true;
                            chart.Series[row].IsXValueIndexed = true;

                            chart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                            chart.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                        }
                    }
                    chart.Visible = true;
                    chart.Height = 400;
                    chart.Width = 1000;

                }
            }
            else
            {
                chart.Visible = false;
                lbl_alert.Visible = true;
                pupdiv.Visible = true;
                output.Text = "";
                lbl_alert.Text = "No Record Found";
            }

        }
        catch { }
    }

    private void CommonHeaderChart()
    {
        try
        {
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                string name = "";
                DataView dv = new DataView();
                List<string> list = new List<string>();
                List<string> listcol = new List<string>();
                DataTable dtchart = new DataTable();
                DataColumn dtcol1 = new DataColumn();
                Hashtable htcol = new Hashtable();

                chart.ChartAreas[0].AxisX.Title = "HeaderWise Common";
                chart.ChartAreas[0].AxisY.Title = "Amount";
                chart.ChartAreas[0].AxisX.TitleForeColor = Color.Red;
                chart.ChartAreas[0].AxisY.TitleForeColor = Color.Red;
                chart.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Center;
                chart.ChartAreas[0].AxisY.TitleAlignment = StringAlignment.Center;
                chart.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
                chart.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
                chart.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Book Antiqua", 15, FontStyle.Bold);
                chart.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Blue;
                chart.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Book Antiqua", 15, FontStyle.Bold);
                chart.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Blue;
                chart.ChartAreas[0].AxisX.Interval = 1;
                chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                chart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                dtchart.Columns.Clear();
                dtchart.Columns.Add(dtcol1);
                for (sel = 0; sel < ds.Tables[0].Rows.Count; sel++)
                {
                    ds.Tables[2].DefaultView.RowFilter = "HeaderFK='" + Convert.ToString(ds.Tables[0].Rows[sel]["HeaderFK"]) + "'";
                    dv = ds.Tables[2].DefaultView;
                    if (Convert.ToString(dv) != "" && dv != null)
                    {
                        DataColumn dtcol = new DataColumn();
                        dtcol.ColumnName = Convert.ToString(dv[0]["HeaderName"]);
                        htcol.Add(Convert.ToInt32(dv[0]["HeaderFK"]), Convert.ToString(dv[0]["HeaderName"]));
                        ListItem li = new ListItem(Convert.ToString(dv[0]["HeaderName"]), Convert.ToString(sel));
                        listcol.Add(Convert.ToString(li));
                        dtchart.Columns.Add(dtcol);
                    }
                }
                if (dtchart.Columns.Count > 0)
                {
                    DataRow dtrow;
                    dtrow = dtchart.NewRow();
                    dtrow[0] = "Total Paid";
                    ListItem li = new ListItem(Convert.ToString("Total Paid"), Convert.ToString(0));
                    list.Add(Convert.ToString(li));
                    DataRow dtrow1;
                    dtrow1 = dtchart.NewRow();
                    dtrow1[0] = "Cash";
                    ListItem li1 = new ListItem(Convert.ToString("Cash"), Convert.ToString(1));
                    list.Add(Convert.ToString(li1));
                    DataRow dtrow2;
                    dtrow2 = dtchart.NewRow();
                    dtrow2[0] = "Cheque";
                    ListItem li2 = new ListItem(Convert.ToString("Cheque"), Convert.ToString(2));
                    list.Add(Convert.ToString(li2));

                    dtchart.Rows.Add(dtrow);
                    dtchart.Rows.Add(dtrow1);
                    dtchart.Rows.Add(dtrow2);
                }
                double totamt = 0;
                string cash = "";
                string cheque = "";
                if (dtchart.Columns.Count > 1)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            bool value = true;
                            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                            {
                                ds.Tables[2].DefaultView.RowFilter = "HeaderFK='" + Convert.ToString(ds.Tables[0].Rows[row]["HeaderFK"]) + "'";
                                dv = ds.Tables[2].DefaultView;
                                if (dv.Count > 0)
                                {
                                    name = dv[0]["HeaderName"].ToString();
                                }
                                string headefk = Convert.ToString(ds.Tables[0].Rows[row]["Headerfk"]);
                                string totalamt = Convert.ToString(ds.Tables[0].Rows[row]["Credit"]);
                                if (value == true)
                                    totamt += Convert.ToDouble(totalamt);
                                value = false;
                                string paymode = Convert.ToString(ds.Tables[1].Rows[i]["Paymode"]);
                                ds.Tables[1].DefaultView.RowFilter = "Paymode='" + paymode + "' and HeaderFK='" + headefk + "'";
                                dv = ds.Tables[1].DefaultView;
                                if (dv.Count > 0 && dv != null)
                                {
                                    if (paymode == "1")
                                        cash = Convert.ToString(dv[0]["credit"]);
                                    if (paymode == "2")
                                        cheque = Convert.ToString(dv[0]["credit"]);
                                }
                            }
                            if (htcol.ContainsValue(name))
                            {
                                if (list.Contains("Total Paid"))
                                {
                                    int rowval = Convert.ToInt32(list.IndexOf("Total Paid"));
                                    int colval = Convert.ToInt32(listcol.IndexOf(name));
                                    dtchart.Rows[rowval][colval + 1] = Convert.ToString(totamt);
                                    totamt = 0;
                                }
                                if (list.Contains("Cash"))
                                {
                                    int rowval = Convert.ToInt32(list.IndexOf("Cash"));
                                    int colval = Convert.ToInt32(listcol.IndexOf(name));
                                    dtchart.Rows[rowval][colval + 1] = Convert.ToString(cash);
                                }
                                if (list.Contains("Cheque"))
                                {
                                    int rowval = Convert.ToInt32(list.IndexOf("Cheque"));
                                    int colval = Convert.ToInt32(listcol.IndexOf(name));
                                    dtchart.Rows[rowval][colval + 1] = Convert.ToString(cheque);
                                }
                            }
                        }
                        chart.Series.Add("Total Paid");
                        chart.Series.Add("Cash");
                        chart.Series.Add("Cheque");
                    }

                }
                if (dtchart.Rows.Count > 0)
                {
                    for (col = 1; col < dtchart.Columns.Count; col++)
                    {
                        for (row = 0; row < dtchart.Rows.Count; row++)
                        {
                            chart.Series[row].Points.AddXY(dtchart.Columns[col].ToString(), dtchart.Rows[row][col].ToString());
                            chart.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                            chart.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;

                            chart.Series[row].IsValueShownAsLabel = true;
                            chart.Series[row].IsXValueIndexed = true;

                            chart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                            chart.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                        }
                    }
                    chart.Visible = true;
                    chart.Height = 400;
                    chart.Width = 1000;
                }
            }
            else
            {
                chart.Visible = false;
                lbl_alert.Visible = true;
                pupdiv.Visible = true;
                output.Text = "";
                lbl_alert.Text = "No Record Found";
            }

        }
        catch { }
    }

    private void IndividualHeaderChart()
    {
        try
        {
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                string name = "";
                DataView dv = new DataView();
                List<string> list = new List<string>();
                List<string> listcol = new List<string>();
                DataTable dtchart = new DataTable();
                DataColumn dtcol1 = new DataColumn();
                Hashtable htcol = new Hashtable();


                chart.ChartAreas[0].AxisX.Title = "HeaderWise Individual";
                chart.ChartAreas[0].AxisY.Title = "Amount";
                chart.ChartAreas[0].AxisX.TitleForeColor = Color.Red;
                chart.ChartAreas[0].AxisY.TitleForeColor = Color.Red;
                chart.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Center;
                chart.ChartAreas[0].AxisY.TitleAlignment = StringAlignment.Center;
                chart.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
                chart.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
                chart.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Book Antiqua", 15, FontStyle.Bold);
                chart.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Blue;
                chart.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Book Antiqua", 15, FontStyle.Bold);
                chart.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Blue;
                chart.ChartAreas[0].AxisX.Interval = 1;
                chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                chart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                dtchart.Columns.Clear();
                dtchart.Columns.Add(dtcol1);
                for (sel = 0; sel < ds.Tables[0].Rows.Count; sel++)
                {
                    ds.Tables[2].DefaultView.RowFilter = "HeaderFK='" + Convert.ToString(ds.Tables[0].Rows[sel]["HeaderFK"]) + "'";
                    dv = ds.Tables[2].DefaultView;
                    if (Convert.ToString(dv) != "" && dv != null)
                    {
                        DataColumn dtcol = new DataColumn();
                        dtcol.ColumnName = Convert.ToString(dv[0]["HeaderName"]);
                        htcol.Add(Convert.ToInt32(dv[0]["HeaderFK"]), Convert.ToString(dv[0]["HeaderName"]));
                        ListItem li = new ListItem(Convert.ToString(dv[0]["HeaderName"]), Convert.ToString(sel));
                        listcol.Add(Convert.ToString(li));
                        dtchart.Columns.Add(dtcol);
                    }
                }
                if (dtchart.Columns.Count > 0)
                {
                    DataRow dtrow;
                    dtrow = dtchart.NewRow();
                    dtrow[0] = "Total Paid";
                    ListItem li = new ListItem(Convert.ToString("Total Paid"), Convert.ToString(0));
                    list.Add(Convert.ToString(li));
                    DataRow dtrow1;
                    dtrow1 = dtchart.NewRow();
                    dtrow1[0] = "Cash";
                    ListItem li1 = new ListItem(Convert.ToString("Cash"), Convert.ToString(1));
                    list.Add(Convert.ToString(li1));
                    DataRow dtrow2;
                    dtrow2 = dtchart.NewRow();
                    dtrow2[0] = "Cheque";
                    ListItem li2 = new ListItem(Convert.ToString("Cheque"), Convert.ToString(2));
                    list.Add(Convert.ToString(li2));

                    dtchart.Rows.Add(dtrow);
                    dtchart.Rows.Add(dtrow1);
                    dtchart.Rows.Add(dtrow2);
                }
                double totamt = 0;
                string cash = "";
                string cheque = "";
                if (dtchart.Columns.Count > 1)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            bool value = true;
                            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                            {
                                ds.Tables[2].DefaultView.RowFilter = "HeaderFK='" + Convert.ToString(ds.Tables[0].Rows[row]["HeaderFK"]) + "'";
                                dv = ds.Tables[2].DefaultView;
                                if (dv.Count > 0)
                                {
                                    name = dv[0]["HeaderName"].ToString();
                                }
                                string headefk = Convert.ToString(ds.Tables[0].Rows[row]["Headerfk"]);
                                string totalamt = Convert.ToString(ds.Tables[0].Rows[row]["Credit"]);
                                if (value == true)
                                    totamt += Convert.ToDouble(totalamt);
                                value = false;
                                string paymode = Convert.ToString(ds.Tables[1].Rows[i]["Paymode"]);
                                ds.Tables[1].DefaultView.RowFilter = "Paymode='" + paymode + "' and HeaderFK='" + headefk + "'";
                                dv = ds.Tables[1].DefaultView;
                                if (dv.Count > 0 && dv != null)
                                {
                                    if (paymode == "1")
                                        cash = Convert.ToString(dv[0]["credit"]);
                                    if (paymode == "2")
                                        cheque = Convert.ToString(dv[0]["credit"]);
                                }
                            }
                            if (htcol.ContainsValue(name))
                            {
                                if (list.Contains("Total Paid"))
                                {
                                    int rowval = Convert.ToInt32(list.IndexOf("Total Paid"));
                                    int colval = Convert.ToInt32(listcol.IndexOf(name));
                                    dtchart.Rows[rowval][colval + 1] = Convert.ToString(totamt);
                                    totamt = 0;
                                }
                                if (list.Contains("Cash"))
                                {
                                    int rowval = Convert.ToInt32(list.IndexOf("Cash"));
                                    int colval = Convert.ToInt32(listcol.IndexOf(name));
                                    dtchart.Rows[rowval][colval + 1] = Convert.ToString(cash);
                                }
                                if (list.Contains("Cheque"))
                                {
                                    int rowval = Convert.ToInt32(list.IndexOf("Cheque"));
                                    int colval = Convert.ToInt32(listcol.IndexOf(name));
                                    dtchart.Rows[rowval][colval + 1] = Convert.ToString(cheque);
                                }
                            }
                        }
                        chart.Series.Add("Total Paid");
                        chart.Series.Add("Cash");
                        chart.Series.Add("Cheque");
                    }

                }
                if (dtchart.Rows.Count > 0)
                {
                    for (col = 1; col < dtchart.Columns.Count; col++)
                    {
                        for (row = 0; row < dtchart.Rows.Count; row++)
                        {
                            chart.Series[row].Points.AddXY(dtchart.Columns[col].ToString(), dtchart.Rows[row][col].ToString());
                            chart.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                            chart.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;

                            chart.Series[row].IsValueShownAsLabel = true;
                            chart.Series[row].IsXValueIndexed = true;

                            chart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                            chart.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                        }
                    }
                    chart.Visible = true;
                    chart.Height = 400;
                    chart.Width = 1000;

                }
            }
            else
            {
                chart.Visible = false;
                lbl_alert.Visible = true;
                pupdiv.Visible = true;
                output.Text = "";
                lbl_alert.Text = "No Record Found";
            }

        }
        catch { }
    }

    private void IndividualLedgerChart()
    {
        try
        {
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                string name = "";
                DataView dv = new DataView();
                List<string> list = new List<string>();
                List<string> listcol = new List<string>();
                DataTable dtchart = new DataTable();
                DataColumn dtcol1 = new DataColumn();
                Hashtable htcol = new Hashtable();


                chart.ChartAreas[0].AxisX.Title = "LedgerWise Individual";
                chart.ChartAreas[0].AxisY.Title = "Amount";
                chart.ChartAreas[0].AxisX.TitleForeColor = Color.Red;
                chart.ChartAreas[0].AxisY.TitleForeColor = Color.Red;
                chart.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Center;
                chart.ChartAreas[0].AxisY.TitleAlignment = StringAlignment.Center;
                chart.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
                chart.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
                chart.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Book Antiqua", 15, FontStyle.Bold);
                chart.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Blue;
                chart.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Book Antiqua", 15, FontStyle.Bold);
                chart.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Blue;
                chart.ChartAreas[0].AxisX.Interval = 1;
                chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                chart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                dtchart.Columns.Clear();
                dtchart.Columns.Add(dtcol1);
                for (sel = 0; sel < ds.Tables[0].Rows.Count; sel++)
                {
                    ds.Tables[2].DefaultView.RowFilter = "LedgerPK='" + Convert.ToString(ds.Tables[0].Rows[sel]["LedgerFK"]) + "'";
                    dv = ds.Tables[2].DefaultView;
                    if (Convert.ToString(dv) != "" && dv != null)
                    {
                        DataColumn dtcol = new DataColumn();
                        dtcol.ColumnName = Convert.ToString(dv[0]["LedgerName"]);
                        htcol.Add(Convert.ToInt32(dv[0]["LedgerPK"]), Convert.ToString(dv[0]["LedgerName"]));
                        ListItem li = new ListItem(Convert.ToString(dv[0]["LedgerName"]), Convert.ToString(sel));
                        listcol.Add(Convert.ToString(li));
                        dtchart.Columns.Add(dtcol);
                    }
                }
                if (dtchart.Columns.Count > 0)
                {
                    DataRow dtrow;
                    dtrow = dtchart.NewRow();
                    dtrow[0] = "Total Paid";
                    ListItem li = new ListItem(Convert.ToString("Total Paid"), Convert.ToString(0));
                    list.Add(Convert.ToString(li));
                    DataRow dtrow1;
                    dtrow1 = dtchart.NewRow();
                    dtrow1[0] = "Cash";
                    ListItem li1 = new ListItem(Convert.ToString("Cash"), Convert.ToString(1));
                    list.Add(Convert.ToString(li1));
                    DataRow dtrow2;
                    dtrow2 = dtchart.NewRow();
                    dtrow2[0] = "Cheque";
                    ListItem li2 = new ListItem(Convert.ToString("Cheque"), Convert.ToString(2));
                    list.Add(Convert.ToString(li2));

                    dtchart.Rows.Add(dtrow);
                    dtchart.Rows.Add(dtrow1);
                    dtchart.Rows.Add(dtrow2);
                }
                double totamt = 0;
                string cash = "";
                string cheque = "";
                if (dtchart.Columns.Count > 1)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            bool value = true;
                            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                            {
                                ds.Tables[2].DefaultView.RowFilter = "LedgerPK='" + Convert.ToString(ds.Tables[0].Rows[row]["LedgerFK"]) + "'";
                                dv = ds.Tables[2].DefaultView;
                                if (dv.Count > 0)
                                {
                                    name = dv[0]["LedgerName"].ToString();
                                }
                                string headefk = Convert.ToString(ds.Tables[0].Rows[row]["LedgerFK"]);
                                string totalamt = Convert.ToString(ds.Tables[0].Rows[row]["Credit"]);
                                if (value == true)
                                    totamt += Convert.ToDouble(totalamt);
                                value = false;
                                string paymode = Convert.ToString(ds.Tables[1].Rows[i]["Paymode"]);
                                ds.Tables[1].DefaultView.RowFilter = "Paymode='" + paymode + "' and LedgerFK='" + headefk + "'";
                                dv = ds.Tables[1].DefaultView;
                                if (dv.Count > 0 && dv != null)
                                {
                                    if (paymode == "1")
                                        cash = Convert.ToString(dv[0]["credit"]);
                                    if (paymode == "2")
                                        cheque = Convert.ToString(dv[0]["credit"]);
                                }
                            }
                            if (htcol.ContainsValue(name))
                            {
                                if (list.Contains("Total Paid"))
                                {
                                    int rowval = Convert.ToInt32(list.IndexOf("Total Paid"));
                                    int colval = Convert.ToInt32(listcol.IndexOf(name));
                                    dtchart.Rows[rowval][colval + 1] = Convert.ToString(totamt);
                                    totamt = 0;
                                }
                                if (list.Contains("Cash"))
                                {
                                    int rowval = Convert.ToInt32(list.IndexOf("Cash"));
                                    int colval = Convert.ToInt32(listcol.IndexOf(name));
                                    dtchart.Rows[rowval][colval + 1] = Convert.ToString(cash);
                                }
                                if (list.Contains("Cheque"))
                                {
                                    int rowval = Convert.ToInt32(list.IndexOf("Cheque"));
                                    int colval = Convert.ToInt32(listcol.IndexOf(name));
                                    dtchart.Rows[rowval][colval + 1] = Convert.ToString(cheque);
                                }
                            }
                        }
                        chart.Series.Add("Total Paid");
                        chart.Series.Add("Cash");
                        chart.Series.Add("Cheque");
                    }

                }
                if (dtchart.Rows.Count > 0)
                {
                    for (col = 1; col < dtchart.Columns.Count; col++)
                    {
                        for (row = 0; row < dtchart.Rows.Count; row++)
                        {
                            chart.Series[row].Points.AddXY(dtchart.Columns[col].ToString(), dtchart.Rows[row][col].ToString());
                            chart.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                            chart.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;

                            chart.Series[row].IsValueShownAsLabel = true;
                            chart.Series[row].IsXValueIndexed = true;

                            chart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                            chart.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                        }
                    }
                    chart.Visible = true;
                    chart.Height = 400;
                    chart.Width = 1000;

                }
            }
            else
            {
                chart.Visible = false;
                lbl_alert.Visible = true;
                pupdiv.Visible = true;
                output.Text = "";
                lbl_alert.Text = "No Record Found";
            }

        }
        catch { }
    }


    private void CommonLedgerChart()
    {
        try
        {
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                string name = "";
                DataView dv = new DataView();
                List<string> list = new List<string>();
                List<string> listcol = new List<string>();
                DataTable dtchart = new DataTable();
                DataColumn dtcol1 = new DataColumn();
                Hashtable htcol = new Hashtable();

                chart.ChartAreas[0].AxisX.Title = "LedgerWise Common";
                chart.ChartAreas[0].AxisY.Title = "Amount";
                chart.ChartAreas[0].AxisX.TitleForeColor = Color.Red;
                chart.ChartAreas[0].AxisY.TitleForeColor = Color.Red;
                chart.ChartAreas[0].AxisX.TitleAlignment = StringAlignment.Center;
                chart.ChartAreas[0].AxisY.TitleAlignment = StringAlignment.Center;
                chart.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
                chart.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
                chart.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Book Antiqua", 15, FontStyle.Bold);
                chart.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Blue;
                chart.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Book Antiqua", 15, FontStyle.Bold);
                chart.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.Blue;
                chart.ChartAreas[0].AxisX.Interval = 1;
                chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                chart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                dtchart.Columns.Clear();
                dtchart.Columns.Add(dtcol1);
                for (sel = 0; sel < ds.Tables[0].Rows.Count; sel++)
                {
                    ds.Tables[2].DefaultView.RowFilter = "LedgerPK='" + Convert.ToString(ds.Tables[0].Rows[sel]["LedgerFK"]) + "'";
                    dv = ds.Tables[2].DefaultView;
                    if (Convert.ToString(dv) != "" && dv != null)
                    {
                        DataColumn dtcol = new DataColumn();
                        dtcol.ColumnName = Convert.ToString(dv[0]["LedgerName"]);
                        htcol.Add(Convert.ToInt32(dv[0]["LedgerPK"]), Convert.ToString(dv[0]["LedgerName"]));
                        ListItem li = new ListItem(Convert.ToString(dv[0]["LedgerName"]), Convert.ToString(sel));
                        listcol.Add(Convert.ToString(li));
                        dtchart.Columns.Add(dtcol);
                    }
                }
                if (dtchart.Columns.Count > 0)
                {
                    DataRow dtrow;
                    dtrow = dtchart.NewRow();
                    dtrow[0] = "Total Paid";
                    ListItem li = new ListItem(Convert.ToString("Total Paid"), Convert.ToString(0));
                    list.Add(Convert.ToString(li));
                    DataRow dtrow1;
                    dtrow1 = dtchart.NewRow();
                    dtrow1[0] = "Cash";
                    ListItem li1 = new ListItem(Convert.ToString("Cash"), Convert.ToString(1));
                    list.Add(Convert.ToString(li1));
                    DataRow dtrow2;
                    dtrow2 = dtchart.NewRow();
                    dtrow2[0] = "Cheque";
                    ListItem li2 = new ListItem(Convert.ToString("Cheque"), Convert.ToString(2));
                    list.Add(Convert.ToString(li2));

                    dtchart.Rows.Add(dtrow);
                    dtchart.Rows.Add(dtrow1);
                    dtchart.Rows.Add(dtrow2);
                }
                double totamt = 0;
                string cash = "";
                string cheque = "";
                if (dtchart.Columns.Count > 1)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            bool value = true;
                            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                            {
                                ds.Tables[2].DefaultView.RowFilter = "LedgerPK='" + Convert.ToString(ds.Tables[0].Rows[row]["LedgerFK"]) + "'";
                                dv = ds.Tables[2].DefaultView;
                                if (dv.Count > 0)
                                {
                                    name = dv[0]["LedgerName"].ToString();
                                }
                                string headefk = Convert.ToString(ds.Tables[0].Rows[row]["LedgerFK"]);
                                string totalamt = Convert.ToString(ds.Tables[0].Rows[row]["Credit"]);
                                if (value == true)
                                    totamt += Convert.ToDouble(totalamt);
                                value = false;
                                string paymode = Convert.ToString(ds.Tables[1].Rows[i]["Paymode"]);
                                ds.Tables[1].DefaultView.RowFilter = "Paymode='" + paymode + "' and LedgerFK='" + headefk + "'";
                                dv = ds.Tables[1].DefaultView;
                                if (dv.Count > 0 && dv != null)
                                {
                                    if (paymode == "1")
                                        cash = Convert.ToString(dv[0]["credit"]);
                                    if (paymode == "2")
                                        cheque = Convert.ToString(dv[0]["credit"]);
                                }
                            }
                            if (htcol.ContainsValue(name))
                            {
                                if (list.Contains("Total Paid"))
                                {
                                    int rowval = Convert.ToInt32(list.IndexOf("Total Paid"));
                                    int colval = Convert.ToInt32(listcol.IndexOf(name));
                                    dtchart.Rows[rowval][colval + 1] = Convert.ToString(totamt);
                                    totamt = 0;
                                }
                                if (list.Contains("Cash"))
                                {
                                    int rowval = Convert.ToInt32(list.IndexOf("Cash"));
                                    int colval = Convert.ToInt32(listcol.IndexOf(name));
                                    dtchart.Rows[rowval][colval + 1] = Convert.ToString(cash);
                                }
                                if (list.Contains("Cheque"))
                                {
                                    int rowval = Convert.ToInt32(list.IndexOf("Cheque"));
                                    int colval = Convert.ToInt32(listcol.IndexOf(name));
                                    dtchart.Rows[rowval][colval + 1] = Convert.ToString(cheque);
                                }
                            }
                        }
                        chart.Series.Add("Total Paid");
                        chart.Series.Add("Cash");
                        chart.Series.Add("Cheque");
                    }

                }
                if (dtchart.Rows.Count > 0)
                {
                    for (col = 1; col < dtchart.Columns.Count; col++)
                    {
                        for (row = 0; row < dtchart.Rows.Count; row++)
                        {
                            chart.Series[row].Points.AddXY(dtchart.Columns[col].ToString(), dtchart.Rows[row][col].ToString());
                            chart.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                            chart.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;

                            chart.Series[row].IsValueShownAsLabel = true;
                            chart.Series[row].IsXValueIndexed = true;

                            chart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                            chart.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                        }
                    }
                    chart.Visible = true;
                    chart.Height = 400;
                    chart.Width = 1000;
                }
            }
            else
            {
                chart.Visible = false;
                lbl_alert.Visible = true;
                pupdiv.Visible = true;
                output.Text = "";
                lbl_alert.Text = "No Record Found";
            }

        }
        catch { }
    }

    #endregion

    protected void cbdetail_OnCheckedChanged(object sender, EventArgs e)
    {
        if (cbldetail.Checked == true)
        {
            if (ddlacctype.SelectedItem.Value == "2")
            {
                rbheader.Checked = true;
            }
            else if (ddlacctype.SelectedItem.Value == "2")
            {
                rbledger.Checked = true;
            }
            rbheader.Visible = true;
            rbledger.Visible = true;

        }
        else
        {
            rbheader.Visible = false;
            rbledger.Visible = false;
        }
    }

    protected void rbheader_OnCheckedChanged(object sender, EventArgs e)
    {
    }
    protected void rbledger_OnCheckedChanged(object sender, EventArgs e)
    {
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

    // last modified 05-10-2016 sudhagar
}