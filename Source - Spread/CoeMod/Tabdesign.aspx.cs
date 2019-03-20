using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using Gios.Pdf;
using System.IO;
using System.Globalization;
using System.Configuration;

public partial class Tabdesign : System.Web.UI.Page
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
    //  DAccess2 da = new DAccess2();
    DAccess2 da = new DAccess2();
    // DataSet ds = new DataSet();
    DataSet ds = new DataSet();
    Hashtable hat = new Hashtable();
    string sql = "";
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
                //ddlYear.Enabled = true;
                //ddlMonth.Enabled = true;

                //loadmonth();
                //loadyear();
                collegecode = Session["collegecode"].ToString();
                BindBatch();
                BindDegree(singleuser, group_user, collegecode, usercode);
                BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);

                ddlMonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
                ddlMonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Jan", "1"));
                ddlMonth.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Feb", "2"));
                ddlMonth.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Mar", "3"));
                ddlMonth.Items.Insert(4, new System.Web.UI.WebControls.ListItem("Apr", "4"));
                ddlMonth.Items.Insert(5, new System.Web.UI.WebControls.ListItem("May", "5"));
                ddlMonth.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jun", "6"));
                ddlMonth.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Jul", "7"));
                ddlMonth.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Aug", "8"));
                ddlMonth.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Sep", "9"));
                ddlMonth.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Oct", "10"));
                ddlMonth.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Nov", "11"));
                ddlMonth.Items.Insert(12, new System.Web.UI.WebControls.ListItem("Dec", "12"));

                int year1;
                year1 = Convert.ToInt16(DateTime.Today.Year);
                ddlYear.Items.Clear();
                for (int l = 0; l <= 10; l++)
                {

                    ddlYear.Items.Add(Convert.ToString(year1 - l));

                }
                ddlYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
                clear();


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

            }
        }
        catch
        {
        }
    }

    public void bindtest()
    {
        string batch_year = "";
        string degree_code = "";

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
        if (degree_code.Trim() == "")
        {
            degree_code = "0";
        }
        if (batch_year.Trim() == "")
        {
            batch_year = "0";
        }

        sql = "select distinct criteria from CriteriaForInternal ci, syllabus_master sm where ci.syll_code=sm.syll_code and degree_code in ('" + degree_code + "')   and Batch_Year in ('" + batch_year + "') ";
        ds.Clear();
        ds = da.select_method_wo_parameter(sql, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            chkltest.DataSource = ds;
            chkltest.DataTextField = "criteria";
            chkltest.DataValueField = "criteria";
            chkltest.DataBind();
            for (int i = 0; i < chkltest.Items.Count; i++)
            {
                chkltest.Items[i].Selected = true;
                if (chkltest.Items[i].Selected == true)
                {
                    count += 1;
                }
                if (chkltest.Items.Count == count)
                {
                    chktest.Checked = true;
                }
            }
            if (chktest.Checked == true)
            {
                for (int i = 0; i < chkltest.Items.Count; i++)
                {
                    chkltest.Items[i].Selected = true;
                    txttest.Text = "Test(" + (chkltest.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chkltest.Items.Count; i++)
                {
                    chkltest.Items[i].Selected = false;
                    txttest.Text = "---Select---";
                }
            }
        }

    }

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }

    protected void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }

    //public void Bindcollege()
    //{
    //    try
    //    {
    //        string columnfield = "";
    //        group_user = Session["group_code"].ToString();
    //        if (group_user.Contains(';'))
    //        {
    //            string[] group_semi = group_user.Split(';');
    //            group_user = group_semi[0].ToString();
    //        }
    //        if ((group_user.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
    //        {
    //            columnfield = " and group_code='" + group_user + "'";
    //        }
    //        else
    //        {
    //            columnfield = " and user_code='" + Session["usercode"] + "'";
    //        }
    //        hat.Clear();
    //        hat.Add("column_field", columnfield.ToString());
    //        DataSet dsprint = da.select_method("bind_college", hat, "sp");
    //        ddlcollege.Items.Clear();
    //        if (dsprint.Tables[0].Rows.Count > 0)
    //        {

    //            ddlcollege.DataSource = dsprint;
    //            ddlcollege.DataTextField = "collname";
    //            ddlcollege.DataValueField = "college_code";
    //            ddlcollege.DataBind();
    //        }
    //        else
    //        {
    //            errmsg.Text = "Set college rights to the staff";
    //            errmsg.Visible = true;
    //            return;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        errmsg.Text = ex.ToString();
    //        errmsg.Visible = true;
    //    }
    //}
    public void BindBatch()
    {
        try
        {
            ds.Dispose();
            ds.Reset();
            ds = da.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
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
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    public void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {

            errmsg.Visible = false;
            count = 0;
            chklstdegree.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds.Dispose();
            ds.Reset();
            ds = da.BindDegree(singleuser, group_user, collegecode, usercode);
            if (ds.Tables[0].Rows.Count > 0)
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
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
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
                if (ds.Tables[0].Rows.Count > 0)
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
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
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
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
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
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void chklstbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            errmsg.Visible = false;
            string clg = "";
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
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            errmsg.Visible = false;
            collegecode = Session["collegecode"].ToString();
            BindDegree(singleuser, group_user, collegecode, usercode);
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);


        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void chktest_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            if (chktest.Checked == true)
            {
                for (int i = 0; i < chkltest.Items.Count; i++)
                {
                    chkltest.Items[i].Selected = true;
                }
                txttest.Text = "Test(" + (chkltest.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chkltest.Items.Count; i++)
                {
                    chkltest.Items[i].Selected = false;
                }
                chktest.Checked = false;
                txttest.Text = "---Select---";
            }

        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void chkltest_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            errmsg.Visible = false;
            string clg = "";
            int commcount = 0;
            txttest.Text = "--Select--";
            chktest.Checked = false;
            for (int i = 0; i < chkltest.Items.Count; i++)
            {
                if (chkltest.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txttest.Text = "Test(" + commcount.ToString() + ")";
                if (commcount == chkltest.Items.Count)
                {
                    chktest.Checked = true;
                }
            }


        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    public void loadyear()
    {
        try
        {
            ddlYear.Items.Clear();
            ds.Reset();
            ds.Dispose();
            // sql = "   select  distinct ed.batch_year from Registration r,Exam_Details ed,mark_entry m where r.Roll_No=m.roll_no and m.exam_code=ed.exam_code and r.Batch_Year=ed.batch_year and ed.degree_code=r.degree_code  and ed.Exam_Month='" + ddlMonth.SelectedItem.Value.ToString().Trim() + "' and ed.Exam_year='" + ddlYear.SelectedItem.Text.ToString().Trim() + "'  order by ed.batch_year";
            ds = da.Examyear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlYear.DataSource = ds;
                ddlYear.DataTextField = "Exam_year";
                ddlYear.DataBind();
            }
            else
            {
                //ddlYear.Enabled = false;
                //ddlMonth.Enabled = false;
            }
        }
        catch
        {
        }

    }

    public void loadmonth()
    {
        try
        {
            ddlMonth.Items.Clear();
            string year = ddlYear.Text.ToString();
            ds.Reset();
            ds.Dispose();
            ds = da.Exammonth(year);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlMonth.DataSource = ds;
                ddlMonth.DataTextField = "monthName";
                ddlMonth.DataValueField = "Exam_month";
                ddlMonth.DataBind();
            }
            else
            {
                //  ddlMonth.Enabled = false;
            }
        }
        catch
        {
        }

    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        string rollerr = "";

        try
        {

            #region load header
            FpSpread2.Sheets[0].RowCount = 0;
            FpSpread2.Sheets[0].AutoPostBack = true;
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.ImageCellType imgt = new FarPoint.Web.Spread.ImageCellType();
            ArrayList header = new ArrayList();

            header.Add("S.No.");
            header.Add("Reg.No.");
            header.Add("Name");
            header.Add("Photo");
            header.Add("Sex");
            header.Add("Date of Birth");
            header.Add("Subject");
            header.Add("");

            header.Add("CGPA");
            header.Add("LG");
            header.Add("Class");
            header.Add("Prov.");
            header.Add("Degree");

            FpSpread2.Visible = false;
            FpSpread2.CommandBar.Visible = false;
            FpSpread2.RowHeader.Visible = false;
            FpSpread2.Sheets[0].ColumnHeader.Rows.Count = 2;
            FpSpread2.Sheets[0].ColumnHeader.Visible = false;

            FpSpread2.Sheets[0].ColumnCount = header.Count;

            for (int ii = 0; ii < header.Count - 2; ii++)
            {
                //   FpSpread2.Sheets[0].Columns[ii].Width = 120;
                FpSpread2.Sheets[0].ColumnHeader.Columns[ii].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, ii].Text = header[ii].ToString();
                FpSpread2.Sheets[0].Columns[ii].Locked = true;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, ii].CellType = txt;
                FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, ii, 2, 1);
            }
            FpSpread2.Sheets[0].ColumnHeader.Cells[1, 11].Text = header[11].ToString();
            FpSpread2.Sheets[0].ColumnHeader.Cells[1, 12].Text = header[12].ToString();
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Applied for";
            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 11, 1, 2);
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Right;



            #endregion
            //----------
            FpSpread2.Sheets[0].Columns[0].Width = 43;
            FpSpread2.Sheets[0].Columns[1].Width = 107;
            FpSpread2.Sheets[0].Columns[2].Width = 308;
            FpSpread2.Sheets[0].Columns[3].Width = 69;
            FpSpread2.Sheets[0].Columns[4].Width = 63;
            FpSpread2.Sheets[0].Columns[5].Width = 117;
            FpSpread2.Sheets[0].Columns[6].Width = 114;
            FpSpread2.Sheets[0].Columns[7].Width = 114;
            FpSpread2.Sheets[0].Columns[8].Width = 110;
            FpSpread2.Sheets[0].Columns[9].Width = 75;
            FpSpread2.Sheets[0].Columns[10].Width = 127;
            FpSpread2.Sheets[0].Columns[11].Width = 85;
            FpSpread2.Sheets[0].Columns[12].Width = 85;

            //---------------
            DropDownList ddltitlename = new DropDownList();

            ddltitlename.Items.Clear();
            string query = "select distinct TextCode,TextVal from textvaltable where TextCriteria = 'DtGrp' and college_code = '" + Session["collegecode"].ToString() + "' order by textcode";
            ds.Clear();
            ds = da.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddltitlename.DataSource = ds;
                ddltitlename.DataTextField = "TextVal";
                ddltitlename.DataValueField = "TextCode";
                ddltitlename.DataBind();

            }

            DataView dvdeptnew = new DataView();
            string batch_year = "";
            string deptnameheader = "";
            string degree_code = "";

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
                clear();
                return;
            }


            if (degree_code.Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select Atleast One Degree and Department";
                clear();
                return;
            }

            DataSet printds = new DataSet();

            DataSet printds_new = new DataSet();
            DataSet printds_rows = new DataSet();
            string lblerror1 = "";
            Boolean printpage = false;
            string edu_level = "";
            string degree = "";
            string monthandyear = "";
            string studname = "";
            string dob = "";
            string rollnosub = "";
            string regnumber = "";

            string exam_code = "";
            string sem = "";


            string branch = "";
            int month = 0;
            string monthstr = "";
            string sql2 = "";
            string sql3 = "";
            string roman = "";
            string semroman = "";
            string grade = "";
            string gradepoints = "";
            string coe = "";
            string subjectcode_Part1 = "";
            string subjectcode_Part2 = "";
            string subjectcode_Part3 = "";
            string subjectcode_Part4 = "";
            string cal_gpa = "";
            string current_semester = "";

            string subtype = "";
            DataSet gradeds = new DataSet();

            Font f1_cos10bold = new Font("Comic Sans MS", 10, FontStyle.Bold);
            Font f2_cos9bold = new Font("Comic Sans MS", 9, FontStyle.Bold);
            Font f3_arial10bold = new Font("Arial", 10, FontStyle.Bold);
            Font Fontarial7r = new Font("Arial", 6, FontStyle.Bold);
            Font f4_arial7reg = new Font("Arial", 7, FontStyle.Regular);
            Font f5_pal10bold = new Font("Palatino Linotype", 10, FontStyle.Bold);
            lbldgreewithexmm_y.Text = "";

            sql = "select  distinct r.current_semester, m.Roll_No,r.Reg_No,r.Stud_Name,ed.batch_year,ed.degree_code from Registration r,Exam_Details ed,mark_entry m,exam_application where r.Roll_No=m.roll_no and m.exam_code=ed.exam_code and r.Batch_Year=ed.batch_year and ed.degree_code=r.degree_code and r.batch_year  in ('" + batch_year + "') and r.degree_code  in ('" + degree_code + "') and ed.Exam_Month='" + ddlMonth.SelectedItem.Value.ToString().Trim() + "' and ed.Exam_year='" + ddlYear.SelectedItem.Text.ToString().Trim() + "'   order by ed.batch_year,r.Reg_No ";
            ds.Clear();
            ds = da.select_method_wo_parameter(sql, "Text");
            int snos = 1;
            int setng_ovrtotalcreadits = 0;
            int setng_mintotalcreadits = 0;
            int totalcreitdsened = 0;
            if (ds.Tables[0].Rows.Count > 0)
            {
                string commandegreename = "";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    batch_year = ds.Tables[0].Rows[i]["batch_year"].ToString();
                    string exam_codelessthan = " ";
                    exam_codelessthan = da.GetFunctionv("SELECT STUFF((SELECT '',',' + convert(nvarchar(max),[exam_code])  FROM Exam_Details sy   where  Exam_year<='" + ddlYear.SelectedItem.Text.ToString().Trim() + "'  and batch_year in ('" + batch_year + "')	and degree_code in ('" + degree_code + "')  and exam_code not in (select distinct exam_code from Exam_Details where   Exam_year=" + ddlYear.SelectedItem.Text.ToString().Trim() + " and Exam_Month>" + ddlMonth.SelectedItem.Value.ToString().Trim() + "	and degree_code in ('" + degree_code + "')  and batch_year in ('" + batch_year + "') ) FOR XML PATH('')),1,1,'') as [exam_code]");
                    //exam_codelessthan = da.GetFunctionv("select MAX(entry_code) from mark_entry where roll_no='" + ds.Tables[0].Rows[i]["roll_no"].ToString() + "'and  exam_code in (" + exam_codelessthan + ")");
                    //  exam_codelessthan = " and  entry_code <= " + exam_codelessthan + " ";
                    exam_codelessthan = " and  exam_code in (" + exam_codelessthan + " )";
                    lbldegreeheader.Text = da.GetFunctionv("select 'Degree : '+c.Course_Name+'-'+dd.Dept_Name from Degree d, Department dd ,course c where d.Dept_Code=dd.Dept_Code and d.Course_Id=c.Course_Id and d.Degree_Code='" + ds.Tables[0].Rows[i]["degree_code"].ToString() + "'");
                    rollnosub = ds.Tables[0].Rows[i]["roll_no"].ToString();
                    regnumber = ds.Tables[0].Rows[i]["reg_no"].ToString();

                    commandegreename = da.GetFunctionv("select distinct textval+'-'+Edu_Level from   tbl_DeptGrouping t,Degree d , textvaltable tt,course c where t.Deptcode=d.Dept_Code  and tt.TextCode=t.Groupcode and d.Course_Id=c.Course_Id  and d.Degree_Code='" + ds.Tables[0].Rows[i]["degree_code"].ToString() + "' and TextCriteria = 'DtGrp'");
                    string[] splitcommandegreename = commandegreename.Split('-');
                    if (commandegreename.Length > 1)
                    {
                        string exam_y = ddlYear.SelectedItem.Text.ToString();
                        string exam_m = ddlMonth.SelectedItem.Value.ToString();

                        string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(exam_m));
                        strMonthName = strMonthName.ToUpper() + " " + exam_y + "   ";
                        string strlbldgreewithexmm_y = "";
                        lbltabl_head.Text = "TABULATED REGISTER FOR " + strMonthName + " EXAMINATIONS";
                        lbltabl_head.Visible = true;
                        if (splitcommandegreename[1].ToString().ToUpper() == "UG")
                        {
                            strlbldgreewithexmm_y = "Degree of Bachelor of " + splitcommandegreename[0].ToString() + " in " + deptnameheader + "  - " + strMonthName + "";
                        }
                        else
                        {
                            strlbldgreewithexmm_y = "Degree of Master of " + splitcommandegreename[0].ToString() + " in " + deptnameheader + "  - " + strMonthName + "";
                        }
                        lbldgreewithexmm_y.Text = strlbldgreewithexmm_y;
                    }
                    rollerr = rollnosub;
                    sql = "SELECT Reg_No,a.partI_Language,r.batch_year,r.app_no,r.degree_code,R.current_semester,R.Stud_Name,stud_nametamil,r.cc,Course_Name as degree,Dept_name,CONVERT(VARCHAR, dob, 103) as dob,case  sex when '0' Then 'M' when '1' then 'F' else 'T' end as sex,c.edu_level,a.partI_language FROM Registration R,Applyn A,Degree G,Course C,Department D WHERE R.App_No = A.App_No AND R.Degree_Code = G.Degree_Code AND G.Course_ID = C.Course_ID AND G.College_Code = C.College_Code AND G.Dept_Code = D.Dept_Code AND D.College_Code = G.College_Code AND Roll_No = '" + rollnosub + "' and r.college_code='" + Session["collegecode"].ToString() + "' ;  select coe,principal,acr from collinfo where college_code='" + Session["collegecode"].ToString() + "';select * from exam_details";
                    sql = sql + "  select count(distinct s.subject_no) as total from subjectchooser sc,subject s,registration r where sc.roll_no=r.roll_no and s.subject_no=sc.subject_no and r.roll_no='" + rollnosub + "'";
                    sql = sql + "  Select count(distinct subject.subject_no) as total from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='pass'  and roll_no='" + rollnosub + "' " + exam_codelessthan + " ";
                    sql = sql + "  Select count(distinct subject.subject_no) as total from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='fail'  and roll_no='" + rollnosub + "' " + exam_codelessthan + "  ; select  distinct c.Course_Name, Groupcode,Deptcode,textval,d.Degree_Code from tbl_DeptGrouping td, textvaltable t,degree d,course c where td.Groupcode=t.TextCode and TextCriteria='dtgrp' and d.Dept_Code=td.Deptcode and c.Course_Id=d.Course_Id and td.college_code='" + Session["collegecode"].ToString() + "' order by  textval;";
                    sql = sql + " select sc.roll_no,s.subject_name from syllabus_master sy,sub_sem ss,subject s,subjectChooser sc where sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and  s.subject_no=sc.subject_no and sc.roll_no='" + rollnosub + "' and sy.semester=1 and s.part_type='1'";
                    sql = sql + "  select distinct COUNT(teq.Equal_Subject_Code),teq.Com_Subject_Code from  subjectchooser sc,subject s,registration r , tbl_equal_paper_Matching teq   where sc.roll_no=r.roll_no and s.subject_no=sc.subject_no   and r.roll_no='" + rollnosub + "'  and teq.Equal_Subject_Code=s.subject_code group by teq.Com_Subject_Code having COUNT(teq.Equal_Subject_Code)>1";
                    sql = sql + "  select textval from applyn ,textvaltable,Registration where TextCode=partI_Language and Registration.App_No=applyn.app_no and  Registration.Reg_No='" + rollnosub + "'";
                    sql = sql + "     Select distinct  subject.subject_no, subject_name, subject_code from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='fail'  and roll_no='" + rollnosub + "'       " + exam_codelessthan + "    and SUBSTRING(subject_code,7,1)!='M'    and  subject.subject_no not in (   Select subject.subject_no from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='pass'   and roll_no='" + rollnosub + "'    " + exam_codelessthan + "   )";
                    sql = sql + "     Select distinct  subject.subject_no, subject_name, subject_code from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='fail'  and roll_no='" + rollnosub + "'       " + exam_codelessthan + "    and SUBSTRING(subject_code,7,1)='M'    and  subject.subject_no not in (   Select subject.subject_no from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='pass'   and roll_no='" + rollnosub + "'    " + exam_codelessthan + "   )";
                    sql = sql + "  SELECT STUFF((SELECT distinct ''',''' + convert(nvarchar(max),[subject_code])  FROM subject sy   where  subject_name='Computer training'   FOR XML PATH('')),1,1,'''') as [Roll_No]";
                    printds = da.select_method_wo_parameter(sql, "Text");
                    // printds = da.select_method_wo_parameter(sql, "Text");
                    int noofsubapplied = Convert.ToInt32(printds.Tables[3].Rows[0][0].ToString());
                    noofsubapplied = noofsubapplied - Convert.ToInt32(printds.Tables[8].Rows.Count);
                    int noofsubpassed = Convert.ToInt32(printds.Tables[4].Rows[0][0].ToString());
                    int noofsubfailed = Convert.ToInt32(printds.Tables[5].Rows[0][0].ToString());
                    edu_level = printds.Tables[0].Rows[0]["edu_level"].ToString();
                    setng_ovrtotalcreadits = Convert.ToInt32(da.GetFunctionv("select totalcredits from coe_ovrl_credits_Dts where degree_code='" + ds.Tables[0].Rows[i]["degree_code"].ToString() + "'"));
                    setng_mintotalcreadits = Convert.ToInt32(da.GetFunctionv("select minimcredits from coe_ovrl_credits_Dts where degree_code='" + ds.Tables[0].Rows[i]["degree_code"].ToString() + "'"));
                    if (edu_level.Trim().ToLower() == "ug" && noofsubpassed != noofsubapplied)
                    {
                        string comcode = "";

                        DataSet dspassorfail = new DataSet();
                        DataView dvcomptraing = new DataView();
                        DataView dvcomsubject = new DataView();
                        int comsubjectcount = 0;
                        DataSet dssequalpaers = new DataSet();
                        ArrayList comsubjects = new ArrayList();
                        for (int isub = 0; isub < printds.Tables[10].Rows.Count; isub++)
                        {
                            string commsubjectpaper1 = da.GetFunctionv("select  Com_Subject_Code from tbl_equal_paper_Matching where Equal_Subject_Code='" + printds.Tables[10].Rows[isub][2].ToString() + "' ");
                            sql = "  select * from tbl_equal_paper_Matching where  Com_Subject_Code  in ('" + commsubjectpaper1 + "') ";
                            dssequalpaers.Clear();
                            dssequalpaers = da.select_method_wo_parameter(sql, "Text");
                            for (int eqlpap = 0; eqlpap < dssequalpaers.Tables[0].Rows.Count; eqlpap++)
                            {
                                string syllcode = da.GetFunctionv("select syll_code from subject where subject_no='" + printds.Tables[10].Rows[isub][0].ToString() + "'");
                                string equlpapersubjectno = da.GetFunctionv("select subject_no from subject where syll_code='" + syllcode + "' and  subject_code='" + dssequalpaers.Tables[0].Rows[eqlpap]["Equal_Subject_Code"].ToString() + "'  ");
                                if (equlpapersubjectno.Trim() != "" && equlpapersubjectno.Trim() != "0")
                                {
                                    dspassorfail.Clear();
                                    dspassorfail = da.select_method_wo_parameter(" select * from mark_entry where subject_no='" + equlpapersubjectno + "' and  result='pass'  " + exam_codelessthan + "  and roll_no='" + rollnosub + "'  ", "Text");
                                    if (dspassorfail.Tables[0].Rows.Count > 0)
                                    {
                                        if (!comsubjects.Contains(commsubjectpaper1))
                                        {
                                            comsubjectcount++;
                                            comsubjects.Add(commsubjectpaper1);
                                        }
                                    }
                                }
                            }
                        }
                        string computersubjectcode = printds.Tables[12].Rows[0][0].ToString();
                        if (computersubjectcode != "")
                        {
                            computersubjectcode = computersubjectcode.Remove(0, 2);
                            computersubjectcode = computersubjectcode + "'";


                        }
                        printds.Tables[11].DefaultView.RowFilter = "subject_code in (" + computersubjectcode + ")";
                        dvcomptraing = printds.Tables[11].DefaultView;

                        int majorpaperscount = printds.Tables[11].Rows.Count;
                        int comcodecount = 0;

                        if (dvcomptraing.Count > 0)
                        {
                            majorpaperscount = printds.Tables[11].Rows.Count - dvcomptraing.Count;
                            comcodecount = comcodecount + 1;
                        }


                        comcodecount = Convert.ToInt32(printds.Tables[10].Rows.Count) - comsubjectcount;

                        int subjectmissed = noofsubapplied - noofsubpassed;
                        if (printds.Tables[11].Rows.Count <= 2 && subjectmissed <= 2 && comcodecount == 0 && setng_ovrtotalcreadits != setng_mintotalcreadits)
                        {
                            noofsubpassed = noofsubapplied;
                        }
                    }
                    if (printds.Tables[0].Rows.Count > 0 && noofsubpassed == noofsubapplied)
                    {
                        lblstudcount.Text = "No. of Students : " + Convert.ToString(snos) + "";
                        printpage = true;
                        edu_level = printds.Tables[0].Rows[0]["edu_level"].ToString();
                        degree = printds.Tables[0].Rows[0]["degree"].ToString();
                        string strMonthName = "";

                        studname = printds.Tables[0].Rows[0]["Stud_Name"].ToString(); //edu_level
                        branch = printds.Tables[0].Rows[0]["Dept_name"].ToString();
                        dob = printds.Tables[0].Rows[0]["dob"].ToString();
                        string sex = printds.Tables[0].Rows[0]["sex"].ToString();
                        string stdappno = printds.Tables[0].Rows[0]["App_No"].ToString();
                        batch_year = printds.Tables[0].Rows[0]["batch_year"].ToString();
                        degree_code = printds.Tables[0].Rows[0]["degree_code"].ToString();
                        string tabmilname = printds.Tables[0].Rows[0]["stud_nametamil"].ToString();

                        string stdphtsql = "select * from StdPhoto where app_no='" + stdappno + "'";
                        MemoryStream memoryStream = new MemoryStream();
                        DataSet dsstdpho = new DataSet();
                        dsstdpho.Clear();
                        dsstdpho.Dispose();
                        dsstdpho = da.select_method_wo_parameter(stdphtsql, "Text");
                        if (dsstdpho.Tables[0].Rows.Count > 0 && dsstdpho.Tables[0].Rows[0][1].ToString().Trim() != "")
                        {
                            byte[] file = (byte[])dsstdpho.Tables[0].Rows[0][1];
                            memoryStream.Write(file, 0, file.Length);
                            if (file.Length > 0)
                            {

                                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                System.Drawing.Image thumb = imgx.GetThumbnailImage(350, 350, null, IntPtr.Zero);
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/" + stdappno + ".jpeg")))
                                {
                                }
                                else
                                {
                                    thumb.Save(HttpContext.Current.Server.MapPath("~/coeimages/" + stdappno + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                }
                            }
                        }
                        string imgurlnew = "";
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/" + stdappno + ".jpeg")))
                        {
                            imgurlnew = "~/coeimages/" + stdappno + ".jpeg";
                        }
                        else
                        {
                            imgurlnew = "~/college/NoImage.jpg";
                        }
                        MyImg mi = new MyImg();
                        mi.ImageUrl = imgurlnew;
                        mi.ImageAlign = ImageAlign.Middle;

                        sql3 = "Select Subject.part_type, syllabus_master.semester,Subject_Type,subject_name,subject_code,subject.subject_no,result,max_int_marks,max_ext_marks,internal_mark,external_mark,((total/maxtotal)*100) as total,maxtotal,grade,cp,credit_points,mark_entry.subject_no,semester,maxtotal,exam_code from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='pass'  and roll_no='" + rollnosub + "'  order by syllabus_master.semester,subject_type,sub_sem.lab,subject.subjectpriority,subject.subject_no ; Select sum(credit_points) from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='pass'  and roll_no='" + rollnosub + "' " + exam_codelessthan + "";
                        printds_rows.Clear();
                        printds_rows.Dispose();
                        printds_rows = da.select_method_wo_parameter(sql3, "Text");

                        string batch_year1 = printds.Tables[0].Rows[0]["batch_year"].ToString() + "-";
                        totalcreitdsened = Convert.ToInt32(printds_rows.Tables[1].Rows[0][0].ToString());
                        if (edu_level.Trim().ToLower() == "ug")
                        {
                            totalcreitdsened = totalcreitdsened + 1;
                            batch_year1 = batch_year1 + Convert.ToString((Convert.ToInt32(batch_year) + 3));
                        }
                        else
                        {
                            batch_year1 = batch_year1 + Convert.ToString((Convert.ToInt32(batch_year) + 2));
                        }
                        int creditsdiff = 0;
                        if (totalcreitdsened > setng_mintotalcreadits)
                        {
                            creditsdiff = totalcreitdsened - setng_mintotalcreadits;
                            creditsdiff = creditsdiff / 5;
                        }

                        FpSpread2.Sheets[0].RowCount++;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(snos);
                        snos = snos + 1;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = regnumber;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = studname;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].CellType = mi;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = sex;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = dob;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 11].Text = "Yes";
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 12].Text = "Yes";
                        FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].Border.BorderColor = Color.White;



                        string exam_y = ddlYear.SelectedItem.Text.ToString();
                        string exam_m = ddlMonth.SelectedItem.Value.ToString();

                        strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(exam_m));
                        strMonthName = strMonthName[0].ToString() + strMonthName[1].ToString() + strMonthName[2].ToString();
                        strMonthName = strMonthName.ToUpper() + " " + exam_y + "   ";
                        FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].Height = 33;

                        if (edu_level.Trim().ToLower() == "ug")
                        {
                            if (printds.Tables[7].Rows.Count > 0)
                            {
                                string subname = printds.Tables[7].Rows[0]["subject_name"].ToString();
                                string[] spbame = subname.Split('-');
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Text = "     Language";
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Text = spbame[0].ToString();
                            }
                        }
                        else
                        {
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Text = "     Major";
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Text = printds.Tables[0].Rows[0]["Dept_name"].ToString();
                        }


                        string year = ddlYear.SelectedItem.Text;
                        string collcode = Session["collegecode"].ToString();

                        string batchsetting = "0";
                        double partsums = 0.000;
                        int partrowcount = 0;
                        Double Credit_Points = 0.0;
                        Double grade_points = 0.0;
                        double creditstotal = 0;
                        double overalltotgrade = 0;

                        string batch_yearmarkbased = da.GetFunction(" select batch_year from coe_classification where  edu_level='" + printds.Tables[0].Rows[0]["edu_level"].ToString() + "' and  markgradeflag ='1'");

                        //if (Convert.ToInt32(batch_year) > Convert.ToInt32(batch_yearmarkbased))
                        {
                            batchsetting = "1";
                            if (printds_rows.Tables[0].Rows.Count > 0)
                            {
                                string sumpart = "";

                                DataView dv_demand_data = new DataView();
                                DataView dv_demand_datadummy = new DataView();
                                overalltotgrade = 0;
                                printds_rows.Tables[0].DefaultView.RowFilter = "part_type='1'";
                                dv_demand_datadummy = printds_rows.Tables[0].DefaultView;
                                partrowcount = dv_demand_datadummy.Count;
                                printds_rows.Tables[0].DefaultView.RowFilter = "part_type='1' and result='pass'";
                                dv_demand_data = printds_rows.Tables[0].DefaultView;
                                if (dv_demand_data.Count > 0 && partrowcount == dv_demand_data.Count)
                                {

                                    for (int sum = 0; sum < dv_demand_data.Count; sum++)
                                    {
                                        grade_points = Convert.ToDouble(dv_demand_data[sum]["total"].ToString());
                                        grade_points = grade_points / 10;
                                        Credit_Points = Convert.ToDouble(dv_demand_data[sum]["credit_points"].ToString());
                                        creditstotal = creditstotal + Credit_Points;
                                        partsums = partsums + (grade_points * Credit_Points);
                                    }

                                    if (creditstotal == 0)
                                    {
                                        sumpart = "0.000";
                                    }
                                    else if (creditstotal > 0)
                                    {
                                        partsums = (partsums / creditstotal);
                                        partsums = Math.Round(partsums, 3);
                                        sumpart = String.Format("{0:0.000}", partsums);
                                    }
                                    else
                                    {
                                        sumpart = "0.000";
                                    }
                                }
                                else if (partrowcount > 0)
                                {
                                    sumpart = "0.000";
                                }
                                else
                                {
                                    sumpart = "--";
                                }
                                if (sumpart != "--")
                                {
                                    double sumpartgrade = 0;
                                    if (double.TryParse(sumpart, out sumpartgrade))
                                    {
                                        sumpartgrade = Convert.ToDouble(sumpart);
                                        overalltotgrade = sumpartgrade;

                                    }
                                    else
                                    {
                                        sumpartgrade = 0;
                                        overalltotgrade = 0;
                                    }

                                    //string gradesqlcoe = "    select * from coe_classification where edu_level='" + edu_level + "' and  '" + sumpartgrade + "'>= frompoint and '" + sumpartgrade + "'< topoint  and  markgradeflag='" + batchsetting + "' ";//added by sridhar 16/aug 2014
                                    //gradeds.Clear();
                                    //gradeds = da.select_method_wo_parameter(gradesqlcoe, "Text");
                                    //if (gradeds.Tables[0].Rows.Count > 0)
                                    //{
                                    //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].Text = sumpart;
                                    //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 11].Text = gradeds.Tables[0].Rows[0]["grade"].ToString();
                                    //}

                                    // if (noofsubfailed != 0 && overalltotgrade >= 6)
                                    if (overalltotgrade >= 6)
                                    {
                                        string cclass = "First Class";
                                        // table1forpagegpa.Cell(0, 0).SetContent(cclass);
                                        // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].Text = "1";
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Text = sumpart;
                                        string gradesqlclass = "    select * from coe_classification where edu_level='" + edu_level + "'  and  '" + overalltotgrade + "'>= frompoint and '" + overalltotgrade + "'< topoint and  markgradeflag='" + batchsetting + "'";//added by sridhar 16/aug 2014
                                        gradeds.Clear();
                                        gradeds = da.select_method_wo_parameter(gradesqlclass, "Text");
                                        if (gradeds.Tables[0].Rows.Count > 0)
                                        {
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].Text = gradeds.Tables[0].Rows[0]["grade"].ToString();
                                        }

                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].Text = cclass;
                                    }
                                    else
                                    {

                                        string gradesqlclass = "    select * from coe_classification where edu_level='" + edu_level + "'  and  '" + overalltotgrade + "'>= frompoint and '" + overalltotgrade + "'< topoint and  markgradeflag='" + batchsetting + "'";//added by sridhar 16/aug 2014
                                        gradeds.Clear();
                                        gradeds = da.select_method_wo_parameter(gradesqlclass, "Text");
                                        if (gradeds.Tables[0].Rows.Count > 0)
                                        {
                                            string cclass = gradeds.Tables[0].Rows[0]["classification"].ToString();
                                            // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].Text = gradeds.Tables[0].Rows[0]["classification"].ToString();
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Text = sumpart;
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].Text = gradeds.Tables[0].Rows[0]["grade"].ToString();
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].Text = cclass;
                                        }
                                    }
                                }
                                else
                                {
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Text = sumpart;
                                    // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 11].Text = "--";
                                }
                            }
                            else
                            {
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Text = "--";
                                // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 11].Text = "--";
                            }
                            FpSpread2.Sheets[0].RowCount++;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = printds.Tables[0].Rows[0]["stud_nametamil"].ToString();
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Size = 10;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Name = "AKILA";
                            if (edu_level.Trim().ToLower() == "ug")
                            {
                                partsums = 0;
                                partrowcount = 0;
                                creditstotal = 0;

                                if (printds_rows.Tables[0].Rows.Count > 0)
                                {
                                    string sumpart = "";
                                    DataView dv_demand_data = new DataView();
                                    DataView dv_demand_datadummy = new DataView();
                                    printds_rows.Tables[0].DefaultView.RowFilter = "part_type='2'";
                                    dv_demand_datadummy = printds_rows.Tables[0].DefaultView;
                                    partrowcount = dv_demand_datadummy.Count;
                                    printds_rows.Tables[0].DefaultView.RowFilter = "part_type='2' and result='pass'";
                                    dv_demand_data = printds_rows.Tables[0].DefaultView;
                                    if (dv_demand_data.Count > 0 && partrowcount == dv_demand_data.Count)
                                    {
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Text = "     English";
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                                        //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Text = printds.Tables[7].Rows[0]["subject_name"].ToString();

                                        for (int sum = 0; sum < dv_demand_data.Count; sum++)
                                        {
                                            grade_points = Convert.ToDouble(dv_demand_data[sum]["total"].ToString());
                                            grade_points = grade_points / 10;
                                            Credit_Points = Convert.ToDouble(dv_demand_data[sum]["credit_points"].ToString());
                                            creditstotal = creditstotal + Credit_Points;
                                            partsums = partsums + (grade_points * Credit_Points);
                                        }

                                        if (creditstotal == 0)
                                        {
                                            sumpart = "0.000";
                                        }
                                        else if (creditstotal > 0)
                                        {
                                            partsums = (partsums / creditstotal);
                                            partsums = Math.Round(partsums, 3);
                                            sumpart = String.Format("{0:0.000}", partsums);
                                        }
                                        else
                                        {
                                            sumpart = "0.000";
                                        }
                                    }
                                    else if (partrowcount > 0)
                                    {
                                        sumpart = "0.000";
                                    }
                                    else
                                    {
                                        sumpart = "--";
                                    }

                                    if (sumpart != "--")
                                    {
                                        double sumpartgrade = 0;
                                        if (double.TryParse(sumpart, out sumpartgrade))
                                        {
                                            sumpartgrade = Convert.ToDouble(sumpart);
                                            overalltotgrade = sumpartgrade;

                                        }
                                        else
                                        {
                                            sumpartgrade = 0;
                                            overalltotgrade = 0;
                                        }
                                        //string gradesqlcoe = "    select * from coe_classification where  edu_level='" + edu_level + "' and  '" + sumpartgrade + "'>= frompoint and '" + sumpartgrade + "'< topoint  and  markgradeflag='" + batchsetting + "'";//added by sridhar 16/aug 2014
                                        //gradeds.Clear();
                                        //gradeds = da.select_method_wo_parameter(gradesqlcoe, "Text");
                                        //if (gradeds.Tables[0].Rows.Count > 0)
                                        //{
                                        //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 13].Text = sumpart;
                                        //    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 14].Text = gradeds.Tables[0].Rows[0]["grade"].ToString();
                                        //}

                                        // if (noofsubfailed != 0 && overalltotgrade >= 6)
                                        if (overalltotgrade >= 6)
                                        {
                                            string cclass = "First Class";
                                            // table1forpagegpa.Cell(0, 0).SetContent(cclass);
                                            // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 12].Text ="1";
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Text = sumpart;
                                            string gradesqlclass = "    select * from coe_classification where edu_level='" + edu_level + "'  and  '" + overalltotgrade + "'>= frompoint and '" + overalltotgrade + "'< topoint and  markgradeflag='" + batchsetting + "'";//added by sridhar 16/aug 2014
                                            gradeds.Clear();
                                            gradeds = da.select_method_wo_parameter(gradesqlclass, "Text");
                                            if (gradeds.Tables[0].Rows.Count > 0)
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].Text = gradeds.Tables[0].Rows[0]["grade"].ToString();
                                            }

                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].Text = cclass;

                                        }
                                        else
                                        {

                                            string gradesqlclass = "    select * from coe_classification where edu_level='" + edu_level + "'  and  '" + overalltotgrade + "'>= frompoint and '" + overalltotgrade + "'< topoint and  markgradeflag='" + batchsetting + "'";//added by sridhar 16/aug 2014
                                            gradeds.Clear();
                                            gradeds = da.select_method_wo_parameter(gradesqlclass, "Text");
                                            if (gradeds.Tables[0].Rows.Count > 0)
                                            {
                                                string cclass = gradeds.Tables[0].Rows[0]["classification"].ToString();
                                                // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 12].Text = gradeds.Tables[0].Rows[0]["classification"].ToString();
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Text = sumpart;
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].Text = gradeds.Tables[0].Rows[0]["grade"].ToString();
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].Text = cclass;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Text = sumpart;
                                        //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].Text = gradeds.Tables[0].Rows[0]["grade"].ToString();
                                    }
                                }
                                else
                                {
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Text = "--";
                                    //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].Text = gradeds.Tables[0].Rows[0]["grade"].ToString();
                                }

                                FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].Border.BorderColor = Color.White;

                                partsums = 0;
                                partrowcount = 0;
                                creditstotal = 0;
                                FpSpread2.Sheets[0].RowCount++;
                                FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].Border.BorderColorRight = Color.White;

                                if (creditsdiff > 0)
                                {
                                }
                                if (printds_rows.Tables[0].Rows.Count > 0)
                                {
                                    string sumpart = "";
                                    string removesubjetcs = "";
                                    DataView dv_demand_data = new DataView();
                                    DataView dv_demand_datadummy = new DataView();
                                    DataSet cutsubject = new DataSet();
                                    if (creditsdiff > 0)
                                    {
                                        // sql = "Select  top " + creditsdiff + " subject_name,subject_code,subject.subject_no,total from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='pass'  and roll_no='" + rollnosub + "' and Part_Type=3 and SUBSTRING(subject_code,7,1)='M'  order by total";
                                        sql = "Select  top " + creditsdiff + " subject.subject_no,credit_points from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='pass'  and roll_no='" + rollnosub + "' and Part_Type=3 and SUBSTRING(subject_code,7,1)='M'  order by total asc,credit_points asc";

                                        cutsubject.Clear();
                                        cutsubject = da.select_method_wo_parameter(sql, "Text");
                                        //for (int ii = 0; ii < cutsubject.Tables[0].Rows.Count; ii++)
                                        //{
                                        //    if (removesubjetcs.Trim() == "")
                                        //    {
                                        //        removesubjetcs = cutsubject.Tables[0].Rows[ii][0].ToString();
                                        //    }
                                        //    else
                                        //    {
                                        //        removesubjetcs = removesubjetcs + "," + cutsubject.Tables[0].Rows[ii][0].ToString();
                                        //    }
                                        //}
                                        int removecredites = 0;
                                        if (cutsubject.Tables.Count > 0 && cutsubject.Tables[0].Rows.Count > 0)
                                        {
                                            for (int ii = 0; ii < cutsubject.Tables[0].Rows.Count; ii++)
                                            {
                                                if (removecredites == 0)
                                                {
                                                    removecredites = Convert.ToInt32(cutsubject.Tables[0].Rows[ii][1].ToString());
                                                    // removesubjetcs = cutsubject.Tables[0].Rows[ii][0].ToString();
                                                }
                                                else
                                                {
                                                    removecredites = removecredites + Convert.ToInt32(cutsubject.Tables[0].Rows[ii][1].ToString());
                                                    //  removesubjetcs = removesubjetcs + "," + cutsubject.Tables[0].Rows[ii][0].ToString();
                                                }
                                                //arr_star.Add(Convert.ToString(cutsubject.Tables[0].Rows[ii][0]));
                                            }
                                            if (removecredites <= 10)
                                            {

                                                for (int ii = 0; ii < cutsubject.Tables[0].Rows.Count; ii++)
                                                {
                                                    if (removesubjetcs.Trim() == "")
                                                    {
                                                        removesubjetcs = cutsubject.Tables[0].Rows[ii][0].ToString();
                                                        //removecredites = Convert.ToInt32(cutsubject.Tables[0].Rows[ii][1].ToString());
                                                    }
                                                    else
                                                    {
                                                        removesubjetcs = removesubjetcs + "," + cutsubject.Tables[0].Rows[ii][0].ToString();
                                                        //removecredites = removecredites + Convert.ToInt32(cutsubject.Tables[0].Rows[ii][1].ToString());
                                                    }
                                                    //arr_star.Add(Convert.ToString(cutsubject.Tables[0].Rows[ii][0]));
                                                }
                                            }
                                            else
                                            {
                                                removesubjetcs = cutsubject.Tables[0].Rows[0][0].ToString();
                                            }
                                        }
                                    }
                                    if (removesubjetcs.Trim() != "")
                                    {
                                        printds_rows.Tables[0].DefaultView.RowFilter = "part_type='3' and subject_no not in (" + removesubjetcs + ")";
                                    }
                                    else
                                    {
                                        printds_rows.Tables[0].DefaultView.RowFilter = "part_type='3'";
                                    }

                                    dv_demand_datadummy = printds_rows.Tables[0].DefaultView;
                                    partrowcount = dv_demand_datadummy.Count;
                                    if (removesubjetcs.Trim() != "")
                                    {
                                        printds_rows.Tables[0].DefaultView.RowFilter = "part_type='3' and result='pass'  and subject_no not in (" + removesubjetcs + ")";
                                    }
                                    else
                                    {
                                        printds_rows.Tables[0].DefaultView.RowFilter = "part_type='3' and result='pass'";
                                    }
                                    dv_demand_data = printds_rows.Tables[0].DefaultView;
                                    if (dv_demand_data.Count > 0 && partrowcount == dv_demand_data.Count)
                                    {
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].Text = "     Major";
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 7].Text = printds.Tables[0].Rows[0]["Dept_name"].ToString();

                                        for (int sum = 0; sum < dv_demand_data.Count; sum++)
                                        {
                                            grade_points = Convert.ToDouble(dv_demand_data[sum]["total"].ToString());
                                            grade_points = grade_points / 10;
                                            Credit_Points = Convert.ToDouble(dv_demand_data[sum]["credit_points"].ToString());
                                            creditstotal = creditstotal + Credit_Points;
                                            partsums = partsums + (grade_points * Credit_Points);
                                        }
                                        if (creditstotal == 0)
                                        {
                                            sumpart = "0.000";
                                        }
                                        else if (creditstotal > 0)
                                        {
                                            partsums = (partsums / creditstotal);
                                            partsums = Math.Round(partsums, 3);
                                            sumpart = String.Format("{0:0.000}", partsums);
                                        }
                                        else
                                        {
                                            sumpart = "0.000";
                                        }
                                    }
                                    else if (partrowcount > 0)
                                    {
                                        sumpart = "0.000";
                                    }
                                    else
                                    {
                                        sumpart = "--";
                                    }
                                    if (sumpart != "--")
                                    {
                                        double sumpartgrade = 0;
                                        if (double.TryParse(sumpart, out sumpartgrade))
                                        {
                                            sumpartgrade = Convert.ToDouble(sumpart);
                                            overalltotgrade = sumpartgrade;
                                        }
                                        else
                                        {
                                            sumpartgrade = 0;
                                            overalltotgrade = 0;
                                        }

                                        if (noofsubfailed != 0 && overalltotgrade >= 6)
                                        {
                                            string cclass = "First Class";
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Text = sumpart;
                                            string gradesqlclass = "    select * from coe_classification where edu_level='" + edu_level + "'  and  '" + overalltotgrade + "'>= frompoint and '" + overalltotgrade + "'< topoint and  markgradeflag='" + batchsetting + "'";//added by sridhar 16/aug 2014
                                            gradeds.Clear();
                                            gradeds = da.select_method_wo_parameter(gradesqlclass, "Text");
                                            if (gradeds.Tables[0].Rows.Count > 0)
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].Text = gradeds.Tables[0].Rows[0]["grade"].ToString();
                                            }

                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].Text = cclass;
                                        }
                                        else
                                        {

                                            string gradesqlclass = "    select * from coe_classification where edu_level='" + edu_level + "'  and  '" + overalltotgrade + "'>= frompoint and '" + overalltotgrade + "'< topoint and  markgradeflag='" + batchsetting + "'";//added by sridhar 16/aug 2014
                                            gradeds.Clear();
                                            gradeds = da.select_method_wo_parameter(gradesqlclass, "Text");
                                            if (gradeds.Tables[0].Rows.Count > 0)
                                            {
                                                string cclass = gradeds.Tables[0].Rows[0]["classification"].ToString();
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Text = sumpart;
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 9].Text = gradeds.Tables[0].Rows[0]["grade"].ToString();
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 10].Text = cclass;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Text = sumpart;
                                    }
                                }
                                else
                                {
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 8].Text = "--";
                                }
                                FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 3, 3, 3, 1);
                            }
                        }
                        //FpSpread2.Sheets[0].RowCount++;
                        FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].Border.BorderColor = Color.White;
                        //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = tabmilname;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Name = "AKILA";
                    }
                }
            }
            if (FpSpread2.Sheets[0].RowCount > 0)
            {
                FpSpread2.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                FpSpread2.Width = 1417;
                FpSpread2.Visible = true;
                showdata.Visible = true;
            }
            else
            {
                showdata.Visible = false;
            }

        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString() + "" + rollerr + "";
            errmsg.Visible = true;

        }

    }

    public void clear()
    {
        showdata.Visible = false;

    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            da.printexcelreport(FpSpread2, reportname);
        }
        catch
        {

        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            //string degreedetails = "Grade Master";
            //string pagename = "GradeMaster.aspx";
            //Printcontrol.loadspreaddetails(FpSpread2, pagename, degreedetails);
            //Printcontrol.Visible = true;

            printpdf();

        }
        catch
        {

        }
    }

    public void printpdf()
    {

        {
            collegecode = Session["collegecode"].ToString();
            int i = 1;
            int j = 0;
            int minus = 0;
            int addcount = 1;
            double pagecount = 0;
            int rowcount = FpSpread2.Sheets[0].RowCount;
            int columcount = FpSpread2.Sheets[0].ColumnCount;
            columcount = columcount - 1;
            Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();
            Font fontmedium = new Font("Book Antique", 10, FontStyle.Regular);
            Font fonthead = new Font("Book Antique", 12, FontStyle.Bold);
            Font fontsmall = new Font("Book Antique", 8, FontStyle.Regular);
            string query = "Select * from collinfo where college_code='" + collegecode + "'";
            DataSet dsinfo = new DataSet();
            dsinfo = da.select_method_wo_parameter(query, "Text");
            string collname = dsinfo.Tables[0].Rows[0]["collname"].ToString();
            string address = dsinfo.Tables[0].Rows[0]["address1"].ToString() + "-" + dsinfo.Tables[0].Rows[0]["address2"].ToString() + dsinfo.Tables[0].Rows[0]["district"].ToString();
            string phone = "Phone" + " : " + dsinfo.Tables[0].Rows[0]["phoneno"].ToString() + " " + "Fax" + " : " + dsinfo.Tables[0].Rows[0]["faxno"].ToString();
            string email = "E-Mail" + " : " + dsinfo.Tables[0].Rows[0]["email"].ToString() + " " + "Web Site" + " : " + dsinfo.Tables[0].Rows[0]["website"].ToString();

            PdfTextArea ptc = new PdfTextArea(fonthead, System.Drawing.Color.Black,
                                                    new PdfArea(mydocument, 50, 20, 500, 50), System.Drawing.ContentAlignment.MiddleCenter, "" + collname + "");
            PdfTextArea ptc1 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                               new PdfArea(mydocument, 50, 60, 500, 50), System.Drawing.ContentAlignment.TopCenter, "" + address + "");

            PdfTextArea ptc2 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                              new PdfArea(mydocument, 50, 80, 500, 50), System.Drawing.ContentAlignment.TopCenter, "" + phone + "");

            PdfTextArea ptc3 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                              new PdfArea(mydocument, 50, 100, 500, 50), System.Drawing.ContentAlignment.TopCenter, "" + email + "");

            PdfTextArea ptc4 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                              new PdfArea(mydocument, 50, 120, 500, 50), System.Drawing.ContentAlignment.TopCenter, "Student Rank & Topper List");
            mypdfpage.Add(ptc);
            mypdfpage.Add(ptc1);
            mypdfpage.Add(ptc2);
            mypdfpage.Add(ptc3);
            mypdfpage.Add(ptc4);
            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
            {
                PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                mypdfpage.Add(LogoImage, 25, 25, 350);
            }

            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
            {
                PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                mypdfpage.Add(LogoImage, 485, 25, 300);
            }
            Gios.Pdf.PdfTable table;
            int row = 1;
            if (rowcount > 11)
            {
                table = mydocument.NewTable(fontsmall, 11, columcount, 5);
                table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                int check = 0;

                while (rowcount > 11)
                {
                    string value = "";
                    if (check != 0)
                    {
                        Gios.Pdf.PdfTablePage pdftable = table.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, 150, 550, 950));
                        mypdfpage.Add(pdftable);
                        mypdfpage.SaveToDocument();
                        mypdfpage = mydocument.NewPage();
                        table = mydocument.NewTable(fontsmall, 11, 9, 5);
                        table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);

                        PdfTextArea pt3c = new PdfTextArea(fonthead, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, 50, 20, 500, 50), System.Drawing.ContentAlignment.MiddleCenter, "" + collname + "");
                        PdfTextArea pt3c1 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, 50, 60, 500, 50), System.Drawing.ContentAlignment.TopCenter, "" + address + "");

                        PdfTextArea pt3c2 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, 50, 80, 500, 50), System.Drawing.ContentAlignment.TopCenter, "" + phone + "");

                        PdfTextArea pt3c3 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, 50, 100, 500, 50), System.Drawing.ContentAlignment.TopCenter, "" + email + "");

                        PdfTextArea pt3c4 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, 50, 120, 500, 50), System.Drawing.ContentAlignment.TopCenter, "Student Rank & Topper List");
                        mypdfpage.Add(pt3c);
                        mypdfpage.Add(pt3c1);
                        mypdfpage.Add(pt3c2);
                        mypdfpage.Add(pt3c3);
                        mypdfpage.Add(pt3c4);
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                        {
                            PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                            mypdfpage.Add(LogoImage, 25, 25, 350);
                        }

                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                        {
                            PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                            mypdfpage.Add(LogoImage, 485, 25, 300);
                        }

                    }

                    table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table.Cell(0, 0).SetContent("S.No");
                    table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table.Cell(0, 1).SetContent("Roll No");
                    table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table.Cell(0, 2).SetContent("Reg No");
                    table.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table.Cell(0, 3).SetContent("Student Name");
                    table.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table.Cell(0, 4).SetContent("CGPA");
                    table.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table.Cell(0, 5).SetContent("Classification");
                    table.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table.Cell(0, 6).SetContent("Total Marks");
                    table.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table.Cell(0, 7).SetContent("Rank");
                    table.Cell(0, 8).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table.Cell(0, 8).SetContent("Photo");
                    table.Columns[0].SetWidth(20);
                    table.Columns[1].SetWidth(50);
                    table.Columns[2].SetWidth(60);
                    table.Columns[3].SetWidth(100);
                    table.Columns[4].SetWidth(20);
                    table.Columns[5].SetWidth(40);
                    table.Columns[6].SetWidth(50);
                    table.Columns[7].SetWidth(30);
                    table.Columns[8].SetWidth(50);
                    table.VisibleHeaders = false;
                    rowcount = rowcount - 11;
                    minus = rowcount;
                    int colval = 180;
                    for (i = 1; i < 11; i++)
                    {
                        for (j = 0; j < columcount; j++)
                        {
                            value = FpSpread2.Sheets[0].Cells[row, j].Text;
                            table.CellRange(0, 0, 0, 4).SetFont(fontsmall);
                            table.Cell(i, j).SetContent(value);
                            table.Cell(i, j).SetCellPadding(17);
                            string regn = "";
                            if (regn == "")
                            {
                                if (j == 1)
                                {
                                    regn = FpSpread2.Sheets[0].Cells[row, 1].Text;
                                    MemoryStream memoryStream = new MemoryStream();

                                    DataSet dsstuphoto = da.select_method_wo_parameter("select photo from stdphoto where app_no in(select app_no from registration where roll_no='" + regn + "')", "Text");
                                    if (dsstuphoto.Tables[0].Rows.Count > 0)
                                    {
                                        if (dsstuphoto.Tables[0].Rows[0]["photo"] != null && dsstuphoto.Tables[0].Rows[0]["photo"].ToString().Trim() != "")
                                        {
                                            byte[] file = (byte[])dsstuphoto.Tables[0].Rows[0]["photo"];
                                            memoryStream.Write(file, 0, file.Length);
                                            if (file.Length > 0)
                                            {
                                                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                System.Drawing.Image thumb = imgx.GetThumbnailImage(20, 20, null, IntPtr.Zero);
                                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + regn + ".jpeg")) == false)
                                                {
                                                    thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + regn + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                }
                                            }
                                            memoryStream.Dispose();
                                            memoryStream.Close();
                                        }
                                    }
                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + regn + ".jpeg")))
                                    {

                                        table.Cell(addcount - 1 + i, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        PdfImage leftimage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/" + regn + ".jpeg"));
                                        mypdfpage.Add(leftimage, 510, colval, 50);
                                    }
                                    colval = colval + 50;
                                }
                            }
                        }
                        row++;
                    }

                    check++;
                }
                addcount = row;
                int final = FpSpread2.Sheets[0].RowCount - addcount;
                pagecount++;
                table.VisibleHeaders = false;
                Gios.Pdf.PdfTablePage mainpdftable = table.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, 150, 550, 950));
                mypdfpage.Add(mainpdftable);
                mypdfpage.SaveToDocument();
                mypdfpage = mydocument.NewPage();

                Gios.Pdf.PdfTable table1 = mydocument.NewTable(fontsmall, final + 1, columcount, 5);
                table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                if (rowcount < 11)
                {
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                    {
                        PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                        mypdfpage.Add(LogoImage, 25, 25, 450);
                    }

                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                    {
                        PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                        mypdfpage.Add(LogoImage, 485, 25, 450);
                    }
                    PdfTextArea ptc5 = new PdfTextArea(fonthead, System.Drawing.Color.Black,
                                                   new PdfArea(mydocument, 50, 20, 500, 50), System.Drawing.ContentAlignment.MiddleCenter, "" + collname + "");
                    PdfTextArea ptc6 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 50, 60, 500, 50), System.Drawing.ContentAlignment.TopCenter, "" + address + "");

                    PdfTextArea ptc7 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, 50, 80, 500, 50), System.Drawing.ContentAlignment.TopCenter, "" + phone + "");

                    PdfTextArea ptc8 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, 50, 100, 500, 50), System.Drawing.ContentAlignment.TopCenter, "" + email + "");

                    PdfTextArea ptc9 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, 50, 120, 500, 50), System.Drawing.ContentAlignment.TopCenter, "Student Rank & Topper List");
                    mypdfpage.Add(ptc5);
                    mypdfpage.Add(ptc6);
                    mypdfpage.Add(ptc7);
                    mypdfpage.Add(ptc8);
                    mypdfpage.Add(ptc9);
                    table1.VisibleHeaders = false;
                    table1.Cell(0, 0).SetContent("S.No");
                    table1.Cell(0, 1).SetContent("Roll No");
                    table1.Cell(0, 2).SetContent("Reg No");
                    table1.Cell(0, 3).SetContent("Student Name");
                    table1.Cell(0, 4).SetContent("CGPA");
                    table1.Cell(0, 5).SetContent("Classification");
                    table1.Cell(0, 6).SetContent("Total Marks");
                    table1.Cell(0, 7).SetContent("Rank");
                    table1.Cell(0, 8).SetContent("Photo");
                    table1.Columns[0].SetWidth(20);
                    table1.Columns[1].SetWidth(50);
                    table1.Columns[2].SetWidth(60);
                    table1.Columns[3].SetWidth(80);
                    table1.Columns[4].SetWidth(20);
                    table1.Columns[5].SetWidth(40);
                    table1.Columns[6].SetWidth(50);
                    table1.Columns[7].SetWidth(30);
                    table1.Columns[8].SetWidth(50);
                    table1.VisibleHeaders = false;
                    int colval = 180;
                    int col = 1;

                    for (i = 1; i < final + 1; i++)
                    {
                        for (j = 0; j < columcount; j++)
                        {
                            string value = FpSpread2.Sheets[0].Cells[addcount - 1 + i, j].Text;
                            table1.CellRange(0, 0, 0, 4).SetFont(fontsmall);
                            table1.Cell(col, j).SetContent(value);
                            table1.Cell(i, j).SetCellPadding(17);
                            string regn = "";
                            if (regn == "")
                            {
                                if (j == 1)
                                {
                                    regn = FpSpread2.Sheets[0].Cells[addcount - 1 + i, 1].Text;
                                    MemoryStream memoryStream = new MemoryStream();
                                    DataSet dsstuphoto = da.select_method_wo_parameter("select photo from stdphoto where app_no in(select app_no from registration where roll_no='" + regn + "')", "Text");
                                    if (dsstuphoto.Tables[0].Rows.Count > 0)
                                    {
                                        if (dsstuphoto.Tables[0].Rows[0]["photo"] != null && dsstuphoto.Tables[0].Rows[0]["photo"].ToString().Trim() != "")
                                        {
                                            byte[] file = (byte[])dsstuphoto.Tables[0].Rows[0]["photo"];
                                            memoryStream.Write(file, 0, file.Length);
                                            if (file.Length > 0)
                                            {
                                                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                System.Drawing.Image thumb = imgx.GetThumbnailImage(20, 20, null, IntPtr.Zero);
                                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + regn + ".jpeg")) == false)
                                                {
                                                    thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + regn + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                }
                                            }
                                        }
                                        memoryStream.Dispose();
                                        memoryStream.Close();
                                    }
                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + regn + ".jpeg")))
                                    {
                                        table1.Cell(col, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        PdfImage leftimage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/" + regn + ".jpeg"));
                                        mypdfpage.Add(leftimage, 510, colval, 50);
                                    }
                                    colval = colval + 50;
                                }
                            }
                        }
                        col++;
                    }
                }
                Gios.Pdf.PdfTablePage mainpdftable1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, 150, 550, 950));
                mypdfpage.Add(mainpdftable1);
                mypdfpage.SaveToDocument();
                mypdfpage = mydocument.NewPage();
            }
            else
            {
                Gios.Pdf.PdfTable table2 = mydocument.NewTable(fontsmall, rowcount, columcount, 1);
                table2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                table2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                table2.Cell(0, 0).SetContent("S.No");
                table2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                table2.Cell(0, 1).SetContent("Roll No");
                table2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                table2.Cell(0, 2).SetContent("Reg No");
                table2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                table2.Cell(0, 3).SetContent("Student Name");
                table2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                table2.Cell(0, 4).SetContent("CGPA");
                table2.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                table2.Cell(0, 5).SetContent("Classification");
                table2.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                table2.Cell(0, 6).SetContent("Total Marks");
                table2.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                table2.Cell(0, 7).SetContent("Rank");
                table2.Cell(0, 8).SetContentAlignment(ContentAlignment.MiddleCenter);
                table2.Cell(0, 8).SetContent("Photo");
                table2.Columns[0].SetWidth(20);
                table2.Columns[1].SetWidth(45);
                table2.Columns[2].SetWidth(60);
                table2.Columns[3].SetWidth(80);
                table2.Columns[4].SetWidth(20);
                table2.Columns[5].SetWidth(40);
                table2.Columns[6].SetWidth(50);
                table2.Columns[7].SetWidth(30);
                table2.Columns[8].SetWidth(50);
                table2.VisibleHeaders = false;
                int colval = 170;
                for (i = 1; i < FpSpread2.Sheets[0].RowCount; i++)
                {
                    for (j = 0; j < columcount; j++)
                    {
                        string value = FpSpread2.Sheets[0].Cells[i, j].Text;
                        table2.CellRange(0, 0, 0, 4).SetFont(fontsmall);
                        table2.Cell(i, j).SetContent(value);
                        table2.Cell(i, j).SetCellPadding(17);
                        string regn = "";
                        if (regn == "")
                        {
                            if (j == 1)
                            {
                                regn = FpSpread2.Sheets[0].Cells[i, 1].Text;
                                MemoryStream memoryStream = new MemoryStream();
                                DataSet dsstuphoto = da.select_method_wo_parameter("select photo from stdphoto where app_no in(select app_no from registration where roll_no='" + regn + "')", "Text");
                                if (dsstuphoto.Tables[0].Rows.Count > 0)
                                {
                                    if (dsstuphoto.Tables[0].Rows[0]["photo"] != null && dsstuphoto.Tables[0].Rows[0]["photo"].ToString().Trim() != "")
                                    {
                                        if (dsstuphoto.Tables[0].Rows[0]["photo"] != null && dsstuphoto.Tables[0].Rows[0]["photo"] != "")
                                        {
                                            byte[] file = (byte[])dsstuphoto.Tables[0].Rows[0]["photo"];
                                            memoryStream.Write(file, 0, file.Length);
                                            if (file.Length > 0)
                                            {
                                                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                System.Drawing.Image thumb = imgx.GetThumbnailImage(20, 20, null, IntPtr.Zero);
                                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + regn + ".jpeg")) == false)
                                                {
                                                    thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + regn + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                }
                                            }
                                        }
                                    }
                                    memoryStream.Dispose();
                                    memoryStream.Close();
                                }

                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + regn + ".jpeg")))
                                {
                                    table2.Cell(i, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    PdfImage leftimage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/" + regn + ".jpeg"));
                                    mypdfpage.Add(leftimage, 510, colval, 50);
                                }
                                colval = colval + 50;
                            }
                        }
                    }
                }
                Gios.Pdf.PdfTablePage mainpdftable5 = table2.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, 150, 550, 950));
                mypdfpage.Add(mainpdftable5);
                mypdfpage.SaveToDocument();
            }
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "Studenttopperlist" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                mydocument.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
            }
        }


    }

    public class MyImg : FarPoint.Web.Spread.ImageCellType
    {

        //public override Control paintcell(string id, System.Web.UI.WebControls.TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object value, Boolean upperLevel)
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img.Width = Unit.Percentage(100);
            //  img.Height = Unit.Percentage(70);
            return img;


        }
    }

}