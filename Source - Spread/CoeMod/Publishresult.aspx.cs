using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Configuration;

public partial class Publishresult : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string course_id = string.Empty;
    string strbatch = string.Empty;
    string strbranch = string.Empty;
    string SenderID = string.Empty;
    string Password = string.Empty;
    int count = 0;

    DAccess2 da = new DAccess2();

    bool flag_true = false;
    DataSet ds = new DataSet();
    Hashtable hat = new Hashtable();
    string sql = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("CoeHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/CoeMod/CoeHome.aspx");
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

                collegecode = Convert.ToString(Session["collegecode"]).Trim();
                bindEduLevel();
                BindBatch();
                BindDegree(singleuser, group_user, collegecode, usercode);
                BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);

                //  ddlMonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
                ddlMonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Jan", "1"));
                ddlMonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Feb", "2"));
                ddlMonth.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Mar", "3"));
                ddlMonth.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Apr", "4"));
                ddlMonth.Items.Insert(4, new System.Web.UI.WebControls.ListItem("May", "5"));
                ddlMonth.Items.Insert(5, new System.Web.UI.WebControls.ListItem("Jun", "6"));
                ddlMonth.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jul", "7"));
                ddlMonth.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Aug", "8"));
                ddlMonth.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Sep", "9"));
                ddlMonth.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Oct", "10"));
                ddlMonth.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Nov", "11"));
                ddlMonth.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Dec", "12"));

                int year1;
                year1 = Convert.ToInt16(DateTime.Today.Year);
                ddlYear.Items.Clear();
                for (int l = 0; l <= 10; l++)
                {
                    ddlYear.Items.Add(Convert.ToString(year1 - l));
                }

                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#00aff0");
                darkstyle.Font.Name = "Book Antiqua";
                darkstyle.Font.Size = FontUnit.Medium;
                darkstyle.Border.BorderSize = 0;
                darkstyle.HorizontalAlign = HorizontalAlign.Center;
                darkstyle.VerticalAlign = VerticalAlign.Middle;
                darkstyle.Border.BorderColor = System.Drawing.Color.Black;
                FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.Font.Name = "Book Antiqua";
                darkstyle.Font.Size = FontUnit.Medium;
                darkstyle.HorizontalAlign = HorizontalAlign.Center;
                darkstyle.VerticalAlign = VerticalAlign.Middle;
                darkstyle.Border.BorderColor = System.Drawing.Color.Gray;
                FpSpread2.ActiveSheetView.DefaultStyle = darkstyle;
                // FpSpread2.Visible = true;

                FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                showdata.Visible = false;
                loadfp();

            }
        }
        catch (Exception ex)
        {
        }
    }

    public void bindEduLevel()
    {
        try
        {
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            string codevalues = string.Empty;
            if (Session["group_code"] != null && (Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                codevalues = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null)
            {
                codevalues = " and user_code='" + usercode + "'";
            }
            ds.Clear();
            chkEduLevel.Checked = false;
            txtEduLevel.Text = "--Select--";
            cblEduLevel.Items.Clear();
            string qry = "select distinct Edu_Level from degree dg,course c,deptprivilages dp,Department dt where  c.course_id=dg.course_id and c.college_code = dg.college_code and dt.Dept_Code=dg.Dept_Code and dg.Degree_Code=dp.degree_code and c.college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "' " + codevalues + " order by Edu_Level desc";
            ds = da.select_method_wo_parameter(qry, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblEduLevel.DataSource = ds;
                cblEduLevel.DataTextField = "Edu_Level";
                cblEduLevel.DataValueField = "Edu_Level";
                cblEduLevel.DataBind();

                foreach (ListItem liEdu in cblEduLevel.Items)
                {
                    liEdu.Selected = true;
                    txtEduLevel.Text = "EduLevel(" + (cblEduLevel.Items.Count) + ")";
                    chkEduLevel.Checked = true;
                }
            }
            else
            {

            }
        }
        catch (Exception ex)
        {
        }
    }

    public void BindBatch()
    {
        try
        {
            ds.Dispose();
            ds.Reset();
            ds = da.BindBatch();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                chklsbatch.DataSource = ds;
                chklsbatch.DataTextField = "Batch_year";
                chklsbatch.DataValueField = "Batch_year";
                chklsbatch.DataBind();
                chklsbatch.SelectedIndex = chklsbatch.Items.Count - 1;
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = true;
                    if (chklsbatch.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (chklsbatch.Items.Count == count)
                    {
                        chkbatch.Checked = true;
                    }
                }
                if (chkbatch.Checked == true)
                {
                    for (int i = 0; i < chklsbatch.Items.Count; i++)
                    {
                        chklsbatch.Items[i].Selected = true;
                        txtbatch.Text = "Batch(" + (chklsbatch.Items.Count) + ")";
                    }
                }
                else
                {
                    for (int i = 0; i < chklsbatch.Items.Count; i++)
                    {
                        chklsbatch.Items[i].Selected = false;
                        txtbatch.Text = "---Select---";
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            count = 0;
            chklstdegree.Items.Clear();
            //if (group_user.Contains(';'))
            //{
            //    string[] group_semi = group_user.Split(';');
            //    group_user = group_semi[0].ToString();
            //}
            ds.Dispose();
            ds.Reset();
            //ds = da.BindDegree(singleuser, group_user, collegecode, usercode);
            string codevalues = string.Empty;
            if (Session["group_code"] != null && (Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                codevalues = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null)
            {
                codevalues = " and user_code='" + usercode + "'";
            }
            string qryEduLevel = string.Empty;
            string edulevel = string.Empty;
            if (cblEduLevel.Items.Count > 0)
            {
                foreach (ListItem liEdu in cblEduLevel.Items)
                {
                    if (liEdu.Selected)
                    {
                        if (string.IsNullOrEmpty(edulevel))
                        {
                            edulevel = "'" + liEdu.Text + "'";
                        }
                        else
                        {
                            edulevel += ",'" + liEdu.Text + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(edulevel))
                {
                    qryEduLevel = " and c.Edu_Level in(" + edulevel + ")";

                }
            }
            else
            {
                qryEduLevel = string.Empty;
            }

            string qry = "select distinct dg.course_id,c.course_name from degree dg,course c,deptprivilages dp,Department dt where  c.course_id=dg.course_id and c.college_code = dg.college_code and dt.Dept_Code=dg.Dept_Code and dg.Degree_Code=dp.degree_code and dg.college_code='" + collegecode + "' " + codevalues + qryEduLevel + "";
            ds = da.select_method_wo_parameter(qry, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                chklstdegree.DataSource = ds;
                chklstdegree.DataTextField = "course_name";
                chklstdegree.DataValueField = "course_id";
                chklstdegree.DataBind();
                chklstdegree.Items[0].Selected = true;
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = true;
                    if (chklstdegree.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (chklstdegree.Items.Count == count)
                    {
                        chkdegree.Checked = true;
                    }
                }
                if (chkdegree.Checked == true)
                {
                    for (int i = 0; i < chklstdegree.Items.Count; i++)
                    {
                        chklstdegree.Items[i].Selected = true;
                        txtdegree.Text = "Degree(" + (chklstdegree.Items.Count) + ")";
                    }
                }
                else
                {
                    for (int i = 0; i < chklstdegree.Items.Count; i++)
                    {
                        chklstdegree.Items[i].Selected = false;
                        txtdegree.Text = "---Select---";
                    }
                }
                txtdegree.Enabled = true;
            }
            else
            {
                txtdegree.Enabled = false;
            }
        }
        catch (Exception ex)
        {

        }

    }

    public void BindBranchMultiple(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            count = 0;
            collegecode = Session["collegecode"].ToString();
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    if (course_id == "")
                    {
                        course_id = "" + chklstdegree.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        course_id = course_id + "," + "" + chklstdegree.Items[i].Value.ToString() + "";
                    }
                }
            }
            chklstbranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds.Dispose();
            ds.Reset();
            if (course_id.Trim() != "")
            {
                ds = da.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    chklstbranch.DataSource = ds;
                    chklstbranch.DataTextField = "dept_name";
                    chklstbranch.DataValueField = "degree_code";
                    chklstbranch.DataBind();
                    chklstbranch.Items[0].Selected = true;
                    for (int i = 0; i < chklstbranch.Items.Count; i++)
                    {
                        chklstbranch.Items[i].Selected = true;
                        if (chklstbranch.Items[i].Selected == true)
                        {
                            count += 1;
                        }
                        if (chklstbranch.Items.Count == count)
                        {
                            chkbranch.Checked = true;
                        }
                    }
                    if (chkbranch.Checked == true)
                    {
                        for (int i = 0; i < chklstbranch.Items.Count; i++)
                        {
                            chklstbranch.Items[i].Selected = true;
                            txtbranch.Text = "Branch(" + (chklstbranch.Items.Count) + ")";
                        }
                    }
                    else
                    {
                        for (int i = 0; i < chklstbranch.Items.Count; i++)
                        {
                            chkbranch.Checked = false;
                            chklstbranch.Items[i].Selected = false;
                            txtbranch.Text = "---Select---";
                        }
                    }
                }
            }
            else
            {
                txtbranch.Text = "---Select---";
                chklstbranch.Items.Clear();
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void chkbatch_ChekedChange(object sender, EventArgs e)
    {
        try
        {

            if (chkbatch.Checked == true)
            {
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = true;
                }
                txtbatch.Text = "Batch(" + (chklsbatch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = false;
                }
                txtbatch.Text = "---Select---";
            }

        }
        catch (Exception ex)
        {

        }
    }

    protected void chklstbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            int commcount = 0;
            txtbatch.Text = "--Select--";
            chkbatch.Checked = false;
            for (int i = 0; i < chklsbatch.Items.Count; i++)
            {
                if (chklsbatch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtbatch.Text = "Batch(" + commcount.ToString() + ")";
                if (commcount == chklsbatch.Items.Count)
                {
                    chkbatch.Checked = true;
                }
            }

        }
        catch (Exception ex)
        {

        }
    }

    protected void chkdegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            collegecode = Session["collegecode"].ToString();
            if (chkdegree.Checked == true)
            {
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = true;
                }
                txtdegree.Text = "Degree(" + (chklstdegree.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = false;
                }
                txtdegree.Text = "---Select---";
                txtbranch.Text = "---Select---";
            }
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);

        }
        catch (Exception ex)
        {

        }
    }

    protected void chklstdegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            collegecode = Session["collegecode"].ToString();
            int commcount = 0;
            chkdegree.Checked = false;
            txtdegree.Text = "---Select---";
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtdegree.Text = "Degree(" + commcount.ToString() + ")";
                if (commcount == chklstdegree.Items.Count)
                {
                    chkdegree.Checked = true;
                }
            }

            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);

        }
        catch (Exception ex)
        {

        }
    }

    protected void chkbranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            if (chkbranch.Checked == true)
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = true;
                }
                txtbranch.Text = "Branch(" + (chklstbranch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = false;
                }
                chkbranch.Checked = false;
                txtbranch.Text = "---Select---";
            }

        }
        catch (Exception ex)
        {

        }
    }

    protected void chklstbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {


            string clg = string.Empty;
            int commcount = 0;
            txtbranch.Text = "--Select--";
            chkbranch.Checked = false;
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtbranch.Text = "Branch(" + commcount.ToString() + ")";
                if (commcount == chklstbranch.Items.Count)
                {
                    chkbranch.Checked = true;
                }
            }


        }
        catch (Exception ex)
        {

        }
    }

    protected void chkEduLevel_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (chkEduLevel.Checked == true)
            {
                for (int i = 0; i < cblEduLevel.Items.Count; i++)
                {
                    cblEduLevel.Items[i].Selected = true;
                }
                txtEduLevel.Text = "EduLevel(" + (cblEduLevel.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cblEduLevel.Items.Count; i++)
                {
                    cblEduLevel.Items[i].Selected = false;
                }
                chkEduLevel.Checked = false;
                txtEduLevel.Text = "---Select---";
            }
            BindDegree(singleuser, group_user, collegecode, usercode);
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        }
        catch (Exception ex)
        {

        }
    }

    protected void cblEduLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string clg = string.Empty;
            int commcount = 0;
            txtEduLevel.Text = "--Select--";
            chkEduLevel.Checked = false;
            for (int i = 0; i < cblEduLevel.Items.Count; i++)
            {
                if (cblEduLevel.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtEduLevel.Text = "EduLevel(" + commcount.ToString() + ")";
                if (commcount == cblEduLevel.Items.Count)
                {
                    chkEduLevel.Checked = true;
                }
            }
            BindDegree(singleuser, group_user, collegecode, usercode);
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadfp();
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadfp();
    }

    protected void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }

    protected void btndeleteresult_Click(object sender, EventArgs e)
    {
        try
        {
            int aa = 0;
            FpSpread2.SaveChanges();
            int selectedcount = 0;
            for (int res = 1; res <= Convert.ToInt32(FpSpread2.Sheets[0].RowCount) - 1; res++)
            {
                int isval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[res, 5].Value);
                if (isval == 1)
                {
                    selectedcount++;
                }
            }

            if (selectedcount == 0)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Select Aleast One Record')", true);
                return;
            }
            if ((ddlMonth.SelectedValue.ToString() != "0") && (ddlYear.SelectedValue.ToString() != "0"))
            {
                FpSpread2.SaveChanges();
                for (int res = 1; res <= Convert.ToInt32(FpSpread2.Sheets[0].RowCount) - 1; res++)
                {
                    int isval = 0;
                    isval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[res, 5].Value);
                    if (isval == 1)
                    {
                        string publishid = FpSpread2.Sheets[0].Cells[res, 4].Tag.ToString();
                        string sql = "delete from publishresult where publishid='" + publishid + "'";
                        aa = da.update_method_wo_parameter(sql, "Text");
                    }
                }
                if (aa > 0)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted Successfully')", true);
                    loadfp();
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnSaveIsFinalYear_Click(object sender, EventArgs e)
    {
        try
        {
            int aa = 0;
            FpSpread2.SaveChanges();
            int selectedcount = 0;
            for (int res = 1; res <= Convert.ToInt32(FpSpread2.Sheets[0].RowCount) - 1; res++)
            {
                int isval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[res, 6].Value);
                if (isval == 1)
                {
                    selectedcount++;
                }
            }

            if ((ddlMonth.SelectedValue.ToString() != "0") && (ddlYear.SelectedValue.ToString() != "0"))
            {
                FpSpread2.SaveChanges();
                for (int res = 1; res <= Convert.ToInt32(FpSpread2.Sheets[0].RowCount) - 1; res++)
                {
                    int isval = 0;
                    isval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[res, 6].Value);
                    if (isval == 1)
                    {
                        string publishid = FpSpread2.Sheets[0].Cells[res, 4].Tag.ToString();
                        string sql = "update publishresult set isFinalYear='1' where publishid='" + publishid + "'";
                        aa = da.update_method_wo_parameter(sql, "Text");
                    }
                    else
                    {
                        string publishid = FpSpread2.Sheets[0].Cells[res, 4].Tag.ToString();
                        string sql = "update publishresult set isFinalYear='0' where publishid='" + publishid + "'";
                        aa = da.update_method_wo_parameter(sql, "Text");
                    }
                }
                if (aa > 0)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Update Successfully')", true);
                    loadfp();
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnsaveresult_Click(object sender, EventArgs e)
    {
        string batch_year = string.Empty;
        string deptnameheader = string.Empty;
        string degree_code = string.Empty;
        string qryEdulevel = string.Empty;
        int selected = 0;
        if (cblEduLevel.Items.Count == 0)
        {
            errmsg.Visible = true;
            errmsg.Text = "No Edu Level Found";
            return;
        }
        else
        {
            foreach (ListItem liEdu in cblEduLevel.Items)
            {
                if (liEdu.Selected)
                {
                    selected++;
                }
            }
            if (selected == 0)
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select Atleast One Edu Level";
                return;
            }
        }

        for (int i = 0; i < chklsbatch.Items.Count; i++)
        {
            if (chklsbatch.Items[i].Selected == true)
            {
                if (batch_year.Trim() == "")
                {
                    batch_year = chklsbatch.Items[i].Text.ToString();
                }
                else
                {
                    batch_year = batch_year + "','" + chklsbatch.Items[i].Text.ToString();
                }
            }
        }

        for (int i = 0; i < chklstbranch.Items.Count; i++)
        {
            if (chklstbranch.Items[i].Selected == true)
            {
                deptnameheader = chklstbranch.Items[i].Text.ToString();
                if (degree_code.Trim() == "")
                {
                    degree_code = chklstbranch.Items[i].Value.ToString();
                }
                else
                {
                    degree_code = degree_code + "','" + chklstbranch.Items[i].Value.ToString();
                }
            }
        }

        if (batch_year.Trim() == "")
        {
            errmsg.Visible = true;
            errmsg.Text = "Please Select Atleast One Batch Year";
            return;
        }

        if (batch_year.Trim() == "")
        {
            errmsg.Visible = true;
            errmsg.Text = "Please Select Atleast One Batch Year";
            return;
        }
        if (degree_code.Trim() == "")
        {
            errmsg.Visible = true;
            errmsg.Text = "Please Select Atleast One Branch";
            return;
        }
        //sql = "delete p from publishresult p,Degree d where d.Degree_Code=p.degree_code and d.college_code='" + Session["collegecode"].ToString() + "' ";
        //int a = da.update_method_wo_parameter(sql, "Text");
        int a = 0;
        string eduLevel = string.Empty;

        for (int i = 0; i < chklsbatch.Items.Count; i++)
        {
            if (chklsbatch.Items[i].Selected == true)
            {
                batch_year = chklsbatch.Items[i].Text.ToString();
                for (int ii = 0; ii < chklstbranch.Items.Count; ii++)
                {
                    if (chklstbranch.Items[ii].Selected == true)
                    {
                        degree_code = chklstbranch.Items[ii].Value.ToString();
                        sql = "if not exists (select * from publishresult where exam_month=" + ddlMonth.SelectedItem.Value.ToString() + " and exam_year=" + ddlYear.SelectedItem.Text.ToString() + " and batch_year = " + batch_year + " and degree_code=" + degree_code + ") insert into publishresult(exam_month,exam_year,batch_year,degree_code) values ('" + ddlMonth.SelectedItem.Value.ToString() + "','" + ddlYear.SelectedItem.Text.ToString() + "','" + batch_year + "','" + degree_code + "')";
                        a = da.update_method_wo_parameter(sql, "Text");
                    }
                }
            }

        }

        loadfp();

    }

    public void loadfp()
    {
        #region load header

        FpSpread2.Sheets[0].RowCount = 0;
        FpSpread2.Sheets[0].AutoPostBack = false;
        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
        FarPoint.Web.Spread.ImageCellType imgt = new FarPoint.Web.Spread.ImageCellType();
        ArrayList header = new ArrayList();

        header.Add("S.No.");
        header.Add("Edu. Level");
        header.Add("Batch Year");
        header.Add("Degree");
        header.Add("Branch");
        header.Add("Select");
        header.Add("Is Final Year Show Result Only All Pass?");

        // FpSpread2.Visible = false;
        FpSpread2.CommandBar.Visible = false;
        FpSpread2.RowHeader.Visible = false;

        FpSpread2.Sheets[0].ColumnCount = header.Count;

        for (int ii = 0; ii < header.Count; ii++)
        {
            //   FpSpread2.Sheets[0].Columns[ii].Width = 120;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, ii].Text = header[ii].ToString();
            if (ii < header.Count - 2)
            {
                FpSpread2.Sheets[0].Columns[ii].Locked = true;
            }
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, ii].CellType = txt;
        }

        string qryEduLevel = string.Empty;
        string eduLevel = string.Empty;
        foreach (ListItem li in cblEduLevel.Items)
        {
            if (li.Selected)
            {
                if (string.IsNullOrEmpty(eduLevel))
                {
                    eduLevel = "'" + li.Text + "'";
                }
                else
                {
                    eduLevel += ",'" + li.Text + "'";
                }
            }
        }
        if (!string.IsNullOrEmpty(eduLevel))
        {
            qryEduLevel = " and c.edu_level in (" + eduLevel + ")";
        }

        string qryBatchYear = string.Empty;
        string BatchYear = string.Empty;
        foreach (ListItem li in chklsbatch.Items)
        {
            if (li.Selected)
            {
                if (string.IsNullOrEmpty(BatchYear))
                {
                    BatchYear = "'" + li.Value + "'";
                }
                else
                {
                    BatchYear += ",'" + li.Value + "'";
                }
            }
        }
        if (!string.IsNullOrEmpty(BatchYear))
        {
            qryBatchYear = " and p.batch_year in (" + BatchYear + ")";
        }

        string qryDegreeCode = string.Empty;
        string DegreeCode = string.Empty;
        foreach (ListItem li in chklstbranch.Items)
        {
            if (li.Selected)
            {
                if (string.IsNullOrEmpty(DegreeCode))
                {
                    DegreeCode = "'" + li.Value + "'";
                }
                else
                {
                    DegreeCode += ",'" + li.Value + "'";
                }
            }
        }
        if (!string.IsNullOrEmpty(DegreeCode))
        {
            qryDegreeCode = " and p.degree_code in (" + DegreeCode + ")";
        }

        FpSpread2.Sheets[0].Columns[4].Width = 250;
        FpSpread2.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;

        #endregion

        #region loaddata

        FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();

        FpSpread2.Sheets[0].RowCount = 0;
        FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
        FpSpread2.Sheets[0].RowCount = FpSpread2.Sheets[0].RowCount + 1;
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].CellType = chkcell1;
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].CellType = chkcell1;
        FpSpread2.Sheets[0].Columns[6].Width = 240;
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
        FpSpread2.Sheets[0].FrozenRowCount = 1;
        chkcell1.AutoPostBack = true;

        FpSpread2.Sheets[0].AutoPostBack = false;
        sql = "select publishid,c.Edu_Level,p.batch_year,c.Course_Name,dd.Dept_Name,p.isFinalYear from publishresult p,Degree d,Department dd,Course c where p.degree_code=d.Degree_Code and d.Dept_Code=dd.Dept_Code and c.Course_Id=d.Course_Id  and d.college_code=c.college_code and p.exam_month='" + ddlMonth.SelectedItem.Value.ToString() + "'  and exam_year='" + ddlYear.SelectedItem.Text.ToString() + "'  and d.college_code='" + Session["collegecode"].ToString() + "' " + qryEduLevel + qryDegreeCode + qryBatchYear + "  order by Edu_Level desc,p.batch_year,c.Course_Name,dd.Dept_Name";

        ds.Clear();
        ds = da.select_method_wo_parameter(sql, "Text");

        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            int sno = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sno++;
                FpSpread2.Sheets[0].RowCount = FpSpread2.Sheets[0].RowCount + 1;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = sno + "";
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].CellType = txt;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["Edu_Level"].ToString();
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].CellType = txt;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[i]["batch_year"].ToString(); ;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[i]["Course_Name"].ToString();
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[i]["Dept_Name"].ToString();
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Tag = ds.Tables[0].Rows[i]["publishid"].ToString();
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].CellType = chkcell;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].CellType = chkcell;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Value = 0;
                if (Convert.ToString(ds.Tables[0].Rows[i]["isFinalYear"]).Trim().ToLower() == "1" || Convert.ToString(ds.Tables[0].Rows[i]["isFinalYear"]).Trim().ToLower() == "true")
                {
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Value = 1;
                }
            }

            FpSpread2.SaveChanges();
            FpSpread2.Width = 930;
            //  FpSpread2.Visible = true;
            string totalrows = FpSpread2.Sheets[0].RowCount.ToString();
            FpSpread2.Sheets[0].PageSize = (Convert.ToInt32(totalrows) * 20) + 40;
            FpSpread2.Height = (Convert.ToInt32(totalrows) * 24) + 60;
            showdata.Visible = true;
            FpSpread2.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            errmsg.Text = string.Empty;
            errmsg.Visible = true;
        }
        else
        {
            errmsg.Text = "No Publish result details Found on this " + ddlMonth.SelectedItem.Text.ToString() + " " + ddlYear.SelectedItem.Text.ToString() + "";
            errmsg.Visible = true;
            showdata.Visible = false;
        }

        #endregion

    }

    protected void FpSpread1_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        string actrow = e.SheetView.ActiveRow.ToString();
        if (flag_true == false && actrow == "0")
        {
            for (int j = 1; j < Convert.ToInt16(FpSpread2.Sheets[0].RowCount); j++)
            {
                string actcol = e.SheetView.ActiveColumn.ToString();
                string seltext = e.EditValues[Convert.ToInt16(actcol)].ToString();
                if (seltext != "System.Object")
                    FpSpread2.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text = seltext.ToString();
            }
            flag_true = true;
        }
    }

}