#region Namespace Declaration

using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data;
using System.Collections;
using Farpnt = FarPoint.Web.Spread;
using System.IO;
using System.Globalization;
using System.Configuration;


#endregion Namespace Declaration

public partial class TabDesignNew : System.Web.UI.Page
{

    #region Variable Declaration

    Hashtable hat = new Hashtable();
    string usercode = "", collegecode = "", singleuser = "", group_user = "", course_id = string.Empty;
    string batch_year = "", degree_code = "", Exam_month = "", Exam_year = string.Empty;

    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();

    string qry = string.Empty;
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();

    Boolean b_school = false;

    #endregion Variable Declaration

    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Page.Header.DataBind();
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
            usercode = Convert.ToString(Session["usercode"]);
            collegecode = Convert.ToString(Session["collegecode"]);
            singleuser = Convert.ToString(Session["single_user"]);
            group_user = Convert.ToString(Session["group_code"]);
            if (!IsPostBack)
            {
                collegecode = Convert.ToString(Session["collegecode"]);
                Bindcollege();
                BindBatch();
                binddegree();
                bindbranch();
                // BindDegree(singleuser, group_user, collegecode, usercode);
                //BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                ddlExamMonth.Items.Clear();
                ddlExamYear.Items.Clear();

                ddlExamMonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
                ddlExamMonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Jan", "1"));
                ddlExamMonth.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Feb", "2"));
                ddlExamMonth.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Mar", "3"));
                ddlExamMonth.Items.Insert(4, new System.Web.UI.WebControls.ListItem("Apr", "4"));
                ddlExamMonth.Items.Insert(5, new System.Web.UI.WebControls.ListItem("May", "5"));
                ddlExamMonth.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jun", "6"));
                ddlExamMonth.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Jul", "7"));
                ddlExamMonth.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Aug", "8"));
                ddlExamMonth.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Sep", "9"));
                ddlExamMonth.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Oct", "10"));
                ddlExamMonth.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Nov", "11"));
                ddlExamMonth.Items.Insert(12, new System.Web.UI.WebControls.ListItem("Dec", "12"));

                //int year1;
                //year1 = DateTime.Today.Year;
                //ddlExamYear.Items.Clear();
                //for (int l = 0; l <= 10; l++)
                //{
                //    ddlExamYear.Items.Add(Convert.ToString(year1 - l));
                //}
                //ddlExamYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
                BindExamYear();


            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Page Load

    #region Bind Header

    public void Bindcollege()
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            string columnfield = string.Empty;
            group_user = Convert.ToString(Session["group_code"]);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            hat.Clear();
            hat.Add("column_field", Convert.ToString(columnfield));
            DataSet dsprint = d2.select_method("bind_college", hat, "sp");
            ddlCollege.Items.Clear();
            if (dsprint.Tables[0].Rows.Count > 0)
            {

                ddlCollege.DataSource = dsprint;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
            }
            else
            {
                lblErrSearch.Text = "Set college rights to the staff";
                lblErrSearch.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void BindBatch()
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;

            int count = 0;
            chklsbatch.Items.Clear();
            ds.Dispose();
            ds.Reset();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklsbatch.DataSource = ds;
                chklsbatch.DataTextField = "Batch_year";
                chklsbatch.DataValueField = "Batch_year";
                chklsbatch.DataBind();

                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "Batch_year";
                ddlbatch.DataValueField = "Batch_year";
                ddlbatch.DataBind();
                ddlbatch.SelectedIndex = ddlbatch.Items.Count - 1;
                chklsbatch.SelectedIndex = chklsbatch.Items.Count - 1;
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = true;
                    if (chklsbatch.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                }
                if (chklsbatch.Items.Count == count)
                {
                    chkbatch.Checked = true;
                }
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
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            int count = 0;
            cblDegree.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblDegree.DataSource = ds;
                cblDegree.DataTextField = "course_name";
                cblDegree.DataValueField = "course_id";
                cblDegree.DataBind();
                cblDegree.Items[0].Selected = true;
                for (int i = 0; i < cblDegree.Items.Count; i++)
                {
                    cblDegree.Items[i].Selected = true;
                    if (cblDegree.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (cblDegree.Items.Count == count)
                    {
                        cbDegree.Checked = true;
                    }
                }
                if (cbDegree.Checked == true)
                {
                    for (int i = 0; i < cblDegree.Items.Count; i++)
                    {
                        cblDegree.Items[i].Selected = true;
                        txtDegree.Text = "Degree(" + (cblDegree.Items.Count) + ")";
                    }
                }
                else
                {
                    for (int i = 0; i < cblDegree.Items.Count; i++)
                    {
                        cblDegree.Items[i].Selected = false;
                        txtDegree.Text = "---Select---";
                    }
                }
                txtDegree.Enabled = true;
            }
            else
            {
                txtDegree.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }

    }

    public void BindBranchMultiple(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;

            int count = 0;
            collegecode = Convert.ToString(ddlCollege.SelectedValue); ;
            for (int i = 0; i < cblDegree.Items.Count; i++)
            {
                if (cblDegree.Items[i].Selected == true)
                {
                    if (course_id == "")
                    {
                        course_id = "" + Convert.ToString(cblDegree.Items[i].Value) + "";
                    }
                    else
                    {
                        course_id = course_id + "," + "" + Convert.ToString(cblDegree.Items[i].Value) + "";
                    }
                }
            }
            cblBranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            ds.Dispose();
            ds.Reset();
            if (course_id.Trim() != "")
            {
                ds = d2.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cblBranch.DataSource = ds;
                    cblBranch.DataTextField = "dept_name";
                    cblBranch.DataValueField = "degree_code";
                    cblBranch.DataBind();
                    cblBranch.Items[0].Selected = true;
                    for (int i = 0; i < cblBranch.Items.Count; i++)
                    {
                        cblBranch.Items[i].Selected = true;
                        if (cblBranch.Items[i].Selected == true)
                        {
                            count += 1;
                        }
                        if (cblBranch.Items.Count == count)
                        {
                            cbBranch.Checked = true;
                        }
                    }
                    if (cbBranch.Checked == true)
                    {
                        for (int i = 0; i < cblBranch.Items.Count; i++)
                        {
                            cblBranch.Items[i].Selected = true;
                            txtBranch.Text = "Branch(" + (cblBranch.Items.Count) + ")";
                        }
                    }
                    else
                    {
                        for (int i = 0; i < cblBranch.Items.Count; i++)
                        {
                            cblBranch.Items[i].Selected = false;
                            txtBranch.Text = "---Select---";
                        }
                    }
                }
            }
            else
            {
                txtBranch.Text = "---Select---";
                cblBranch.Items.Clear();
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void binddegree()
    {
        try
        {
            ddldegree.Items.Clear();
            usercode = Convert.ToString(Session["usercode"]);
            singleuser = Convert.ToString(Session["single_user"]);
            group_user = Convert.ToString(Session["group_code"]);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            hat.Clear();
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            ds = d2.select_method("bind_degree", hat, "sp");
            int count1 = ds.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();

            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void bindbranch()
    {
        try
        {
            hat.Clear();
            usercode = Convert.ToString(Session["usercode"]);
            singleuser = Convert.ToString(Session["single_user"]);
            group_user = Convert.ToString(Session["group_code"]);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("course_id", Convert.ToString(ddldegree.SelectedValue));
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            ds = d2.select_method("bind_branch", hat, "sp");
            int count2 = ds.Tables[0].Rows.Count;
            if (count2 > 0)
            {
                ddlbranch.DataSource = ds;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    public void BindExamYear()
    {
        try
        {
            ddlExamYear.Items.Clear();
            ds.Reset();
            ds.Dispose();
            ds.Clear();
            ds = d2.select_method_wo_parameter("select distinct Exam_year from Exam_Details order by Exam_year", "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlExamYear.DataSource = ds;
                ddlExamYear.DataTextField = "Exam_year";
                ddlExamYear.DataValueField = "Exam_year";
                ddlExamYear.DataBind();
            }
            else
            {
                ddlExamYear.Enabled = false;
                ddlExamYear.Enabled = false;
            }
            ddlExamYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }

    }

    #endregion Bind Header

    #region DropDownList Events

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //divHeader.Visible = false;
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            divTabSpread.Visible = false;
            rptprint1.Visible = false;
            popupdiv.Visible = false;
            lblpoperr.Text = string.Empty;

            usercode = Convert.ToString(Session["usercode"]);
            collegecode = Convert.ToString(Session["collegecode"]);
            singleuser = Convert.ToString(Session["single_user"]);
            group_user = Convert.ToString(Session["group_code"]);
            BindBatch();
            binddegree();
            bindbranch();
            //BindDegree(singleuser, group_user, collegecode, usercode);
            //BindBranchMultiple(singleuser, group_user, "", collegecode, usercode);

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void ddlExamYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            divTabSpread.Visible = false;
            //divHeader.Visible = false;
            rptprint1.Visible = false;
            popupdiv.Visible = false;
            lblpoperr.Text = string.Empty;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void ddlExamMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            divTabSpread.Visible = false;
            //divHeader.Visible = false;
            rptprint1.Visible = false;
            popupdiv.Visible = false;
            lblpoperr.Text = string.Empty;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            divTabSpread.Visible = false;
            //divHeader.Visible = false;
            rptprint1.Visible = false;
            popupdiv.Visible = false;
            lblpoperr.Text = string.Empty;
            binddegree();
            bindbranch();
            //BindDegree(singleuser, group_user, collegecode, usercode);
            //BindBranchMultiple(singleuser, group_user, "", collegecode, usercode);
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            divTabSpread.Visible = false;
            //divHeader.Visible = false;
            rptprint1.Visible = false;
            popupdiv.Visible = false;
            lblpoperr.Text = string.Empty;
            bindbranch();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            divTabSpread.Visible = false;
            //divHeader.Visible = false;
            rptprint1.Visible = false;
            popupdiv.Visible = false;
            lblpoperr.Text = string.Empty;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion DropDownList Events

    #region CheckBox Events

    protected void chkbatch_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            divTabSpread.Visible = false;
            //divHeader.Visible = false;
            rptprint1.Visible = false;
            popupdiv.Visible = false;
            lblpoperr.Text = string.Empty;
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
            binddegree();
            bindbranch();
            BindDegree(singleuser, group_user, collegecode, usercode);
            BindBranchMultiple(singleuser, group_user, "", collegecode, usercode);
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void cbDegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            rptprint1.Visible = false;
            //divHeader.Visible = false;
            divTabSpread.Visible = false;
            popupdiv.Visible = false;
            collegecode = Convert.ToString(ddlCollege.SelectedValue);
            if (cbDegree.Checked == true)
            {
                for (int i = 0; i < cblDegree.Items.Count; i++)
                {
                    cblDegree.Items[i].Selected = true;
                }
                txtDegree.Text = "Degree(" + (cblDegree.Items.Count) + ")";
                if (b_school == true)
                {
                    txtDegree.Text = "School Type(" + (cblDegree.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < cblDegree.Items.Count; i++)
                {
                    cblDegree.Items[i].Selected = false;
                }
                txtDegree.Text = "---Select---";
                txtBranch.Text = "---Select---";
            }
            BindBranchMultiple(singleuser, group_user, "", collegecode, usercode);
            bindbranch();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void cbBranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            divTabSpread.Visible = false;
            //divHeader.Visible = false;
            rptprint1.Visible = false;
            popupdiv.Visible = false;
            if (cbBranch.Checked == true)
            {
                for (int i = 0; i < cblBranch.Items.Count; i++)
                {
                    cblBranch.Items[i].Selected = true;
                }
                txtBranch.Text = "Branch(" + (cblBranch.Items.Count) + ")";

                if (b_school == true)
                {
                    txtBranch.Text = "Standard(" + (cblBranch.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < cblBranch.Items.Count; i++)
                {
                    cblBranch.Items[i].Selected = false;
                }
                txtBranch.Text = "---Select---";
            }

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion CheckBox Events

    #region CheckBoxList Events

    protected void chklstbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            divTabSpread.Visible = false;
            ////divHeader.Visible = false;
            rptprint1.Visible = false;
            popupdiv.Visible = false;
            lblpoperr.Text = string.Empty;

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
            binddegree();
            bindbranch();
            BindDegree(singleuser, group_user, collegecode, usercode);
            BindBranchMultiple(singleuser, group_user, "", collegecode, usercode);
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void cblDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            divTabSpread.Visible = false;
            //divHeader.Visible = false;
            rptprint1.Visible = false;
            popupdiv.Visible = false;

            collegecode = Convert.ToString(ddlCollege.SelectedValue);
            int commcount = 0;
            cbDegree.Checked = false;
            txtDegree.Text = "---Select---";
            for (int i = 0; i < cblDegree.Items.Count; i++)
            {
                if (cblDegree.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtDegree.Text = "Degree(" + Convert.ToString(commcount) + ")";

                if (commcount == cblDegree.Items.Count)
                {
                    cbDegree.Checked = true;
                }
            }
            bindbranch();
            BindBranchMultiple(singleuser, group_user, "", collegecode, usercode);
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    protected void cblBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            divTabSpread.Visible = false;
            //divHeader.Visible = false;
            popupdiv.Visible = false;
            rptprint1.Visible = false;

            string clg = string.Empty;
            int commcount = 0;
            txtBranch.Text = "--Select--";
            cbBranch.Checked = false;
            for (int i = 0; i < cblBranch.Items.Count; i++)
            {
                if (cblBranch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtBranch.Text = "Branch(" + Convert.ToString(commcount) + ")";
                if (b_school == true)
                {
                    txtBranch.Text = "Standard(" + Convert.ToString(commcount) + ")";
                }
                if (commcount == cblBranch.Items.Count)
                {
                    cbBranch.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion CheckBoxList Events

    #region Initialize Spread

    public void Init_Spread(Farpnt.FpSpread FpViewSpread)
    {
        try
        {
            #region FpSpread Style

            FpViewSpread.Visible = false;
            FpViewSpread.Sheets[0].ColumnCount = 0;
            FpViewSpread.Sheets[0].RowCount = 0;
            FpViewSpread.Sheets[0].SheetCorner.ColumnCount = 0;
            FpViewSpread.CommandBar.Visible = false;

            #endregion FpSpread Style

            FpViewSpread.Visible = false;
            FpViewSpread.CommandBar.Visible = false;
            FpViewSpread.RowHeader.Visible = false;
            FpViewSpread.Sheets[0].AutoPostBack = true;
            FpViewSpread.Sheets[0].RowCount = 0;

            FpViewSpread.Sheets[0].ColumnCount = 9;

            #region SpreadStyles

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            //darkstyle.ForeColor = System.Drawing.Color.Black;
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Size = FontUnit.Point(10);
            darkstyle.Font.Bold = true;
            darkstyle.HorizontalAlign = HorizontalAlign.Center;
            darkstyle.VerticalAlign = VerticalAlign.Middle;
            darkstyle.ForeColor = System.Drawing.Color.Black;
            darkstyle.Border.BorderSize = 1;
            darkstyle.Border.BorderColor = System.Drawing.Color.Black;


            FarPoint.Web.Spread.StyleInfo sheetstyle = new FarPoint.Web.Spread.StyleInfo();
            //sheetstyle.BackColor = ColorTranslator.FromHtml("#00aff0");
            //darkstyle.ForeColor = System.Drawing.Color.Black;
            sheetstyle.Font.Name = "Book Antiqua";
            sheetstyle.Font.Size = FontUnit.Point(10);
            sheetstyle.Font.Bold = true;
            sheetstyle.HorizontalAlign = HorizontalAlign.Left;
            sheetstyle.VerticalAlign = VerticalAlign.Middle;
            sheetstyle.ForeColor = System.Drawing.Color.Black;
            sheetstyle.Border.BorderSize = 1;
            sheetstyle.Border.BorderColor = System.Drawing.Color.Black;

            #endregion SpreadStyles

            FpViewSpread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            //FpViewSpread.ActiveSheetView.ColumnHeader.DefaultStyle=
            FpViewSpread.Sheets[0].DefaultStyle = sheetstyle;
            FpViewSpread.Sheets[0].ColumnHeader.RowCount = 3;

            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 0].Locked = true;
            FpViewSpread.Sheets[0].Columns[0].Width = 25;
            FpViewSpread.Sheets[0].Columns[0].Locked = true;
            FpViewSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);

            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Reg.No";
            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 1].Locked = true;
            //FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 1].Renderer

            FpViewSpread.Sheets[0].ColumnHeader.Cells[1, 1].Text = "D.O.B";
            FpViewSpread.Sheets[0].ColumnHeader.Cells[1, 1].Locked = true;

            FpViewSpread.Sheets[0].ColumnHeader.Cells[2, 1].Text = "Community";
            FpViewSpread.Sheets[0].ColumnHeader.Cells[2, 1].Locked = true;

            FpViewSpread.Sheets[0].Columns[1].Width = 78;
            FpViewSpread.Sheets[0].Columns[1].Locked = true;

            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Name of the\nCandidate in English\nand in Tamil\nwith Initial";
            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 2].Locked = true;
            FpViewSpread.Sheets[0].Columns[2].Width = 185;
            FpViewSpread.Sheets[0].Columns[2].Locked = true;
            FpViewSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 3, 1);
            //
            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Optional\nSubject";
            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 3].Locked = true;
            FpViewSpread.Sheets[0].Columns[3].Width = 230;
            FpViewSpread.Sheets[0].Columns[3].Locked = true;
            FpViewSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 3, 1);

            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Theory\nClass";
            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 4].Locked = true;
            FpViewSpread.Sheets[0].Columns[4].Width = 75;
            FpViewSpread.Sheets[0].Columns[4].Locked = true;
            FpViewSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 3, 1);

            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Practical\nClass";
            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 5].Locked = true;
            FpViewSpread.Sheets[0].Columns[5].Width = 75;
            FpViewSpread.Sheets[0].Columns[5].Locked = true;
            FpViewSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 3, 1);

            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Month and\nYear of\nPassing";
            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 6].Locked = true;
            FpViewSpread.Sheets[0].Columns[6].Width = 100;
            FpViewSpread.Sheets[0].Columns[6].Locked = true;
            FpViewSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 3, 1);

            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Photos";
            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 7].Locked = true;
            FpViewSpread.Sheets[0].Columns[7].Width = 80;
            FpViewSpread.Sheets[0].Columns[7].Locked = true;
            FpViewSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 3, 1);

            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Medium of\nInstruction";
            FpViewSpread.Sheets[0].ColumnHeader.Cells[0, 8].Locked = true;
            FpViewSpread.Sheets[0].Columns[8].Width = 115;
            FpViewSpread.Sheets[0].Columns[8].Locked = true;
            FpViewSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 3, 1);

            FpViewSpread.Sheets[0].ColumnHeader.Visible = false;

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }

    }

    public void Init_Spread()
    {
        try
        {
            #region FpSpread Style

            FpTabSpread.Visible = false;
            FpTabSpread.Sheets[0].ColumnCount = 0;
            FpTabSpread.Sheets[0].RowCount = 0;
            FpTabSpread.Sheets[0].SheetCorner.ColumnCount = 0;
            FpTabSpread.CommandBar.Visible = false;

            #endregion FpSpread Style

            FpTabSpread.Visible = false;
            FpTabSpread.CommandBar.Visible = false;
            FpTabSpread.RowHeader.Visible = false;
            FpTabSpread.Sheets[0].AutoPostBack = true;
            FpTabSpread.Sheets[0].RowCount = 0;

            FpTabSpread.Sheets[0].ColumnCount = 9;

            #region SpreadStyles

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            //darkstyle.ForeColor = System.Drawing.Color.Black;
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Size = FontUnit.Point(10);
            darkstyle.Font.Bold = true;
            darkstyle.HorizontalAlign = HorizontalAlign.Center;
            darkstyle.VerticalAlign = VerticalAlign.Middle;
            darkstyle.ForeColor = System.Drawing.Color.Black;
            darkstyle.Border.BorderSize = 1;
            darkstyle.Border.BorderColor = System.Drawing.Color.BlueViolet;

            FarPoint.Web.Spread.StyleInfo sheetstyle = new FarPoint.Web.Spread.StyleInfo();
            //sheetstyle.BackColor = ColorTranslator.FromHtml("#00aff0");
            //darkstyle.ForeColor = System.Drawing.Color.Black;
            sheetstyle.Font.Name = "Book Antiqua";
            sheetstyle.Font.Size = FontUnit.Point(10);
            sheetstyle.Font.Bold = true;
            sheetstyle.HorizontalAlign = HorizontalAlign.Left;
            sheetstyle.VerticalAlign = VerticalAlign.Middle;
            sheetstyle.ForeColor = System.Drawing.Color.Black;
            sheetstyle.Border.BorderSize = 1;
            sheetstyle.Border.BorderColor = System.Drawing.Color.Black;

            #endregion SpreadStyles

            FpTabSpread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpTabSpread.Sheets[0].DefaultStyle = sheetstyle;
            FpTabSpread.Sheets[0].ColumnHeader.RowCount = 3;

            FpTabSpread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpTabSpread.Sheets[0].ColumnHeader.Cells[0, 0].Locked = true;
            FpTabSpread.Sheets[0].Columns[0].Width = 40;
            FpTabSpread.Sheets[0].Columns[0].Locked = true;
            FpTabSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);

            FpTabSpread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Reg.No";
            FpTabSpread.Sheets[0].ColumnHeader.Cells[0, 1].Locked = true;

            FpTabSpread.Sheets[0].ColumnHeader.Cells[1, 1].Text = "D.O.B";
            FpTabSpread.Sheets[0].ColumnHeader.Cells[1, 1].Locked = true;

            FpTabSpread.Sheets[0].ColumnHeader.Cells[2, 1].Text = "Community";
            FpTabSpread.Sheets[0].ColumnHeader.Cells[2, 1].Locked = true;

            FpTabSpread.Sheets[0].Columns[1].Width = 100;
            FpTabSpread.Sheets[0].Columns[1].Locked = true;

            FpTabSpread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Name of the Candidate in English and in Tamil";
            FpTabSpread.Sheets[0].ColumnHeader.Cells[0, 2].Locked = true;
            FpTabSpread.Sheets[0].Columns[2].Width = 300;
            FpTabSpread.Sheets[0].Columns[2].Locked = true;
            FpTabSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 3, 1);
            //
            FpTabSpread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Theory Class";
            FpTabSpread.Sheets[0].ColumnHeader.Cells[0, 3].Locked = true;
            FpTabSpread.Sheets[0].Columns[3].Width = 200;
            FpTabSpread.Sheets[0].Columns[3].Locked = true;
            FpTabSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 3, 1);

            FpTabSpread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Dissertation Class";
            FpTabSpread.Sheets[0].ColumnHeader.Cells[0, 4].Locked = true;
            FpTabSpread.Sheets[0].Columns[4].Width = 200;
            FpTabSpread.Sheets[0].Columns[4].Locked = true;
            FpTabSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 3, 1);

            FpTabSpread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Overall Classification";
            FpTabSpread.Sheets[0].ColumnHeader.Cells[0, 5].Locked = true;
            FpTabSpread.Sheets[0].Columns[5].Width = 200;
            FpTabSpread.Sheets[0].Columns[5].Locked = true;
            FpTabSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 3, 1);

            FpTabSpread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Month and Year of Passing";
            FpTabSpread.Sheets[0].ColumnHeader.Cells[0, 6].Locked = true;
            FpTabSpread.Sheets[0].Columns[6].Width = 200;
            FpTabSpread.Sheets[0].Columns[6].Locked = true;
            FpTabSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 3, 1);

            FpTabSpread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Photos";
            FpTabSpread.Sheets[0].ColumnHeader.Cells[0, 7].Locked = true;
            FpTabSpread.Sheets[0].Columns[7].Width = 200;
            FpTabSpread.Sheets[0].Columns[7].Locked = true;
            FpTabSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 3, 1);

            FpTabSpread.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Medium of Instruction";
            FpTabSpread.Sheets[0].ColumnHeader.Cells[0, 8].Locked = true;
            FpTabSpread.Sheets[0].Columns[8].Width = 200;
            FpTabSpread.Sheets[0].Columns[8].Locked = true;
            FpTabSpread.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 3, 1);

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }

    }

    #endregion Initialize Spread

    #region Button Click

    #region GO Button

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            divTabSpread.Visible = false;
            rptprint1.Visible = false;
            popupdiv.Visible = false;
            lblpoperr.Text = string.Empty;
            int degselcount = 0;
            if (ddlCollege.Items.Count == 0)
            {
                lblpoperr.Text = "College is not found";
                popupdiv.Visible = true;
                return;
            }
            else
            {
                collegecode = Convert.ToString(ddlCollege.SelectedValue);
            }
            if (ddlbatch.Items.Count == 0)
            {
                lblpoperr.Text = "Batch Year is not found";
                popupdiv.Visible = true;
                return;
            }
            else
            {
                batch_year = Convert.ToString(ddlbatch.SelectedValue);
            }
            if (ddldegree.Items.Count == 0)
            {
                lblpoperr.Text = "Degree is not found";
                popupdiv.Visible = true;
                return;
            }
            if (ddlbranch.Items.Count == 0)
            {
                lblpoperr.Text = "Branch is not found";
                popupdiv.Visible = true;
                return;
            }
            else
            {
                degree_code = Convert.ToString(ddlbranch.SelectedValue);
                //foreach (ListItem li in cblBranch.Items)
                //{
                //    if (li.Selected)
                //    {
                //        degselcount++;
                //        if (degree_code == "")
                //        {
                //            degree_code = li.Value;
                //        }
                //        else
                //        {
                //            degree_code += "," + li.Value;
                //        }
                //    }
                //}

            }
            //if (degselcount == 0)
            //{
            //    lblpoperr.Text = "Please Select Any One Branch";
            //    popupdiv.Visible = true;
            //    return;
            //}

            if (ddlExamYear.Items.Count == 0)
            {
                lblpoperr.Text = "Exam Year is not found";
                popupdiv.Visible = true;
                return;
            }
            else
            {
                if (Convert.ToString(ddlExamYear.SelectedValue) != "0")
                    Exam_year = Convert.ToString(ddlExamYear.SelectedValue);
                else
                {
                    lblpoperr.Text = "Please Select Exam Year";
                    popupdiv.Visible = true;
                    return;
                }
            }
            if (ddlExamMonth.Items.Count == 0)
            {
                lblpoperr.Text = "Exam Month is not found";
                popupdiv.Visible = true;
                return;
            }
            else
            {
                if (Convert.ToString(ddlExamMonth.SelectedValue) != "0")
                    Exam_month = Convert.ToString(ddlExamMonth.SelectedValue);
                else
                {
                    lblpoperr.Text = "Please Select Exam Month";
                    popupdiv.Visible = true;
                    return;
                }
            }

            if (batch_year != "" && collegecode != "" && degree_code != "" && Exam_year.Trim() != "" && Exam_month.Trim() != "")
            {
                lblYrofAdmission.InnerText = string.Empty;
                lblDegreeCourse.InnerText = string.Empty;
                clgname.InnerText = string.Empty;
                //lblclgname.Text =string.Empty;
                lblAddr.InnerText = string.Empty;
                lblClgCode.InnerText = string.Empty;
                DataSet dsStudDetails = new DataSet();
                DataSet dsSubject = new DataSet();
                string newqry = "select r.App_No,r.Roll_No,r.Reg_No,r.Stud_Name,a.stud_nametamil,Convert(nvarchar(15),a.dob,103) DOB,(select TextVal from TextValTable t where t.TextCode=a.community and t.college_code=r.college_code) Community,(select TextVal from TextValTable t where t.TextCode=a.medium_ins and t.college_code=r.college_code) as Medium,case sex when '0' Then 'M' when '1' then 'F' else 'T' end as sex,c.Edu_Level,c.Course_Name,dt.Dept_Name,dg.Degree_Code from applyn a,Registration r,Course c,Degree dg,Department dt where r.college_code=c.college_code and c.college_code=dg.college_code and dg.college_code=dt.college_code and dt.college_code=r.college_code and dg.college_code=a.college_code and a.college_code=dt.college_code  and r.degree_code=dg.Degree_Code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and dg.Degree_Code=a.degree_code and r.college_code=a.college_code and r.App_No=a.app_no and  r.college_code='" + collegecode + "' and r.Batch_Year='" + batch_year + "' and r.degree_code in (" + degree_code + ") and DelFlag=0 AND Exam_Flag<>'debar' order by r.Reg_No";
                dsStudDetails = d2.select_method_wo_parameter(newqry, "Text");

                newqry = "select sy.Batch_Year,sy.degree_code,sy.semester,sc.roll_no,ss.subject_type,s.subject_name,s.subject_code,s.subject_no,ss.Lab,ss.ElectivePap,ss.Disseration,ss.projThe from syllabus_master sy,subject s,sub_sem ss,subjectChooser sc where sy.syll_code=ss.syll_code and s.syll_code=sy.syll_code and s.syll_code=ss.syll_code and ss.subType_no=s.subType_no and sc.subtype_no=ss.subType_no and s.subType_no=sc.subtype_no and s.subject_no=sc.subject_no and sc.semester=sy.semester and sy.Batch_Year='" + batch_year + "' and sy.degree_code in (" + degree_code + ")  order by sc.roll_no,sc.semester,ss.subject_type,s.subject_name";
                dsSubject = d2.select_method_wo_parameter(newqry, "Text");

                qry = string.Empty;
                qry = "select  distinct r.current_semester, m.Roll_No,r.Reg_No,r.Stud_Name,ed.batch_year,ed.degree_code from Registration r,Exam_Details ed,mark_entry m where r.Roll_No=m.roll_no and m.exam_code=ed.exam_code and r.Batch_Year=ed.batch_year and ed.degree_code=r.degree_code  and r.degree_code  in (" + degree_code + ") and ed.Exam_Month='" + Exam_month.Trim() + "' and ed.Exam_year='" + Exam_year.Trim() + "' and  r.batch_year='" + batch_year + "' and r.college_code='" + collegecode + "'  order by ed.batch_year,ed.degree_code,r.Reg_No ";

                ds.Clear();
                ds = d2.select_method_wo_parameter(qry, "Text");
                int snos = 1;
                int setng_ovrtotalcreadits = 0;
                int setng_mintotalcreadits = 0;
                int totalcreitdsened = 0;
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Init_Spread(FpTabSpread);
                    //FpTabSpread.Visible = true;
                    //divTabSpread.Visible = true;
                    //FpTabSpread.RowHeader.Visible = true;
                    lblYrofAdmission.InnerText = " Year of Admission : " + batch_year;
                    lblAddr.InnerText = d2.GetFunctionv("select address1+','+address2 from collinfo where college_code='" + collegecode + "'").Trim();
                    clgname.InnerText = d2.GetFunctionv("select collname from collinfo where college_code='" + collegecode + "'").Trim();
                    //lblclgname.Text = d2.GetFunctionv("select collname from collinfo where college_code='" + collegecode + "'").Trim();
                    lblClgCode.InnerText = d2.GetFunctionv("select acr from collinfo where college_code='" + collegecode + "'").Trim();
                    string Course_Name = Convert.ToString(ddldegree.SelectedItem);
                    string exam_y1 = Convert.ToString(ddlExamYear.SelectedItem.Text);
                    string exam_m1 = Convert.ToString(ddlExamMonth.SelectedItem.Value);

                    string monthyear1 = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(exam_m1));
                    monthyear1 = monthyear1.ToUpper() + " - " + exam_y1;

                    lblDegreeCourse.InnerText = Course_Name + "  DEGREE EXAMINATION " + monthyear1;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string batch = Convert.ToString(ds.Tables[0].Rows[i]["batch_year"]);
                        string deg_code = Convert.ToString(ds.Tables[0].Rows[i]["degree_code"]);
                        string roll_no = Convert.ToString(ds.Tables[0].Rows[i]["Roll_No"]);
                        string reg_no = Convert.ToString(ds.Tables[0].Rows[i]["Reg_No"]);
                        string edu_level = string.Empty;
                        string medium = string.Empty;
                        string dob = string.Empty;
                        string community = string.Empty;
                        string studname = string.Empty;
                        string studtamilname = string.Empty;
                        string stdappno = string.Empty;
                        string optionalsubjects = string.Empty;
                        string theoryClassfy = string.Empty;
                        string pracclassify = string.Empty;
                        string monthyear = string.Empty;

                        string exam_codelessthan = " ";
                        exam_codelessthan = d2.GetFunctionv("SELECT STUFF((SELECT '',',' + convert(nvarchar(max),[exam_code])  FROM Exam_Details sy   where  Exam_year<='" + Exam_year.Trim() + "'  and batch_year in ('" + batch + "')	and degree_code in (" + deg_code + ")  and exam_code not in (select distinct exam_code from Exam_Details where   Exam_year=" + Exam_year.Trim() + " and Exam_Month>" + Exam_month.Trim() + "	and degree_code in (" + deg_code + ")  and batch_year in ('" + batch_year + "') ) FOR XML PATH('')),1,1,'') as [exam_code]");
                        exam_codelessthan = " and  exam_code in (" + exam_codelessthan + " )";

                        //qry = "select count(s.subject_no) as total from subjectchooser sc,subject s,registration r where sc.roll_no=r.roll_no and s.subject_no=sc.subject_no and r.roll_no='" + roll_no + "'; Select count(subject.subject_no) as total from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='pass'  and roll_no='" + roll_no + "' " + exam_codelessthan + "; Select count(subject.subject_no) as total from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='fail'  and roll_no='" + roll_no + "' " + exam_codelessthan + ""; //Aruna 23may2018
                        qry = "select count(s.subject_no) as total from subjectchooser sc,subject s,registration r ,sub_sem ss where sc.roll_no=r.roll_no and s.subject_no=sc.subject_no and ss.subType_no =s.subType_no and ss.promote_count =1 and r.roll_no='" + roll_no + "'; Select count(subject.subject_no) as total from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='pass'  and roll_no='" + roll_no + "' " + exam_codelessthan + "; Select count(subject.subject_no) as total from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='fail'  and roll_no='" + roll_no + "' " + exam_codelessthan + "";
                        DataSet printds = new DataSet();
                        printds = d2.select_method_wo_parameter(qry, "Text");

                        if (printds.Tables.Count > 0)
                        {
                            int noofsubapplied = 0;// Convert.ToInt32(Convert.ToString(printds.Tables[3].Rows[0][0]));
                            int.TryParse(Convert.ToString(printds.Tables[0].Rows[0][0]), out noofsubapplied);

                            int noofsubpassed = 0;// Convert.ToInt32(Convert.ToString(printds.Tables[4].Rows[0][0]));
                            int.TryParse(Convert.ToString(printds.Tables[1].Rows[0][0]), out noofsubpassed);

                            int noofsubfailed = 0;//Convert.ToInt32(Convert.ToString(printds.Tables[5].Rows[0][0]));noofsubpassed != noofsubapplied
                            int.TryParse(Convert.ToString(printds.Tables[2].Rows[0][0]), out noofsubfailed);


                            if (dsStudDetails.Tables.Count > 0 && dsStudDetails.Tables[0].Rows.Count > 0)
                            {
                                DataView dvStud = new DataView();
                                dsStudDetails.Tables[0].DefaultView.RowFilter = "Roll_No='" + roll_no + "'";
                                dvStud = dsStudDetails.Tables[0].DefaultView;
                                if (dvStud.Count > 0 && noofsubpassed == noofsubapplied)
                                {
                                    studname = Convert.ToString(dvStud[0]["Stud_Name"]);
                                    studtamilname = Convert.ToString(dvStud[0]["stud_nametamil"]);
                                    dob = Convert.ToString(dvStud[0]["DOB"]);
                                    community = Convert.ToString(dvStud[0]["Community"]);
                                    medium = Convert.ToString(dvStud[0]["Medium"]);
                                    stdappno = Convert.ToString(dvStud[0]["App_No"]);
                                    edu_level = Convert.ToString(dvStud[0]["Edu_Level"]);

                                    //if ((edu_level.Trim().ToLower() == "pg" || edu_level.Trim('.').ToLower() == "p.g") && i == 0)
                                    //{
                                    //    Init_Spread();
                                    //}

                                    string stdphtsql = "select * from StdPhoto where app_no='" + stdappno + "'";
                                    MemoryStream memoryStream = new MemoryStream();
                                    DataSet dsstdpho = new DataSet();
                                    dsstdpho.Clear();
                                    dsstdpho.Dispose();
                                    dsstdpho = d2.select_method_wo_parameter(stdphtsql, "Text");
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
                                    string imgurlnew = string.Empty;
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


                                    optionalsubjects = string.Empty;
                                    theoryClassfy = string.Empty;
                                    pracclassify = string.Empty;
                                    monthyear = string.Empty;


                                    int startrow = FpTabSpread.Sheets[0].RowCount;
                                    string sql3 = "Select Subject.part_type, syllabus_master.semester,Subject_Type,subject_name,subject_code,subject.subject_no,result,max_int_marks,max_ext_marks,internal_mark,external_mark,total,maxtotal,grade,cp,credit_points,mark_entry.subject_no,semester,maxtotal,exam_code,sub_sem.Lab,isnull(subject.Elective,'0') as ElectivePap,sub_sem.projThe from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='pass'  and roll_no='" + roll_no + "'  order by syllabus_master.semester,subject.subjectpriority,sub_sem.lab; ";//Select sum(credit_points) from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  result='pass'  and roll_no='" + roll_no + "' " + exam_codelessthan + " sub_sem.Disseration
                                    DataSet printds_rows = new DataSet();
                                    printds_rows.Clear();
                                    printds_rows.Dispose();
                                    printds_rows = d2.select_method_wo_parameter(sql3, "Text");
                                    if (printds_rows.Tables.Count > 0 && printds_rows.Tables[0].Rows.Count > 0)
                                    {
                                        int sprow = FpTabSpread.Sheets[0].RowCount += 3;
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 0].Text = Convert.ToString(snos);
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 0].Locked = true;
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 0].Font.Name = "Book Antiqua";
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 0].Font.Size = FontUnit.Point(10);
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 0].VerticalAlign = VerticalAlign.Middle;
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 0].Border.BorderSize = 1;
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 0].Border.BorderSizeTop = 1;
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 0].Border.BorderSizeBottom = 1;
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 0].Border.BorderSizeRight = 1;
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 0].Border.BorderSizeLeft = 1;
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 0].Border.BorderColor = ColorTranslator.FromHtml("#000000");
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 0].Border.BorderColorBottom = ColorTranslator.FromHtml("#000000");
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 0].Border.BorderColorLeft = ColorTranslator.FromHtml("#000000");
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 0].Border.BorderColorRight = ColorTranslator.FromHtml("#000000");
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 0].Border.BorderColorTop = ColorTranslator.FromHtml("#000000");

                                        snos = snos + 1;
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 1].Text = reg_no;
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 1].Locked = true;
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 1].Font.Name = "Book Antiqua";
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 1].Font.Size = FontUnit.Point(10);
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 1].VerticalAlign = VerticalAlign.Middle;
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 1].Border.BorderSize = 1;
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 1].Border.BorderSizeTop = 1;
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 1].Border.BorderSizeBottom = 0;
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 1].Border.BorderSizeRight = 1;
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 1].Border.BorderSizeLeft = 1;
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 1].Border.BorderColor = ColorTranslator.FromHtml("#000000");
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 1].Border.BorderColorBottom = ColorTranslator.FromHtml("#000000");
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 1].Border.BorderColorLeft = ColorTranslator.FromHtml("#000000");
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 1].Border.BorderColorRight = ColorTranslator.FromHtml("#000000");
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 1].Border.BorderColorTop = ColorTranslator.FromHtml("#000000");

                                        FpTabSpread.Sheets[0].Cells[sprow - 2, 1].Text = Convert.ToString(dob);
                                        FpTabSpread.Sheets[0].Cells[sprow - 2, 1].Locked = true;
                                        FpTabSpread.Sheets[0].Cells[sprow - 2, 1].Font.Name = "Book Antiqua";
                                        FpTabSpread.Sheets[0].Cells[sprow - 2, 1].Font.Size = FontUnit.Point(10);
                                        FpTabSpread.Sheets[0].Cells[sprow - 2, 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpTabSpread.Sheets[0].Cells[sprow - 2, 1].VerticalAlign = VerticalAlign.Middle;
                                        FpTabSpread.Sheets[0].Cells[sprow - 2, 1].Border.BorderSize = 1;
                                        FpTabSpread.Sheets[0].Cells[sprow - 2, 1].Border.BorderColor = ColorTranslator.FromHtml("#000000");
                                        FpTabSpread.Sheets[0].Cells[sprow - 2, 1].Border.BorderSizeTop = 0;
                                        FpTabSpread.Sheets[0].Cells[sprow - 2, 1].Border.BorderSizeBottom = 0;
                                        FpTabSpread.Sheets[0].Cells[sprow - 2, 1].Border.BorderSizeRight = 1;
                                        FpTabSpread.Sheets[0].Cells[sprow - 2, 1].Border.BorderSizeLeft = 1;
                                        FpTabSpread.Sheets[0].Cells[sprow - 2, 1].Border.BorderColorBottom = ColorTranslator.FromHtml("#000000");
                                        FpTabSpread.Sheets[0].Cells[sprow - 2, 1].Border.BorderColorLeft = ColorTranslator.FromHtml("#000000");
                                        FpTabSpread.Sheets[0].Cells[sprow - 2, 1].Border.BorderColorRight = ColorTranslator.FromHtml("#000000");
                                        FpTabSpread.Sheets[0].Cells[sprow - 2, 1].Border.BorderColorTop = ColorTranslator.FromHtml("#000000");

                                        FpTabSpread.Sheets[0].Cells[sprow - 1, 1].Text = Convert.ToString(community);
                                        FpTabSpread.Sheets[0].Cells[sprow - 1, 1].Locked = true;
                                        FpTabSpread.Sheets[0].Cells[sprow - 1, 1].Font.Name = "Book Antiqua";
                                        FpTabSpread.Sheets[0].Cells[sprow - 1, 1].Font.Size = FontUnit.Point(10);
                                        FpTabSpread.Sheets[0].Cells[sprow - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpTabSpread.Sheets[0].Cells[sprow - 1, 1].VerticalAlign = VerticalAlign.Middle;
                                        FpTabSpread.Sheets[0].Cells[sprow - 1, 1].Border.BorderSize = 1;
                                        FpTabSpread.Sheets[0].Cells[sprow - 1, 1].Border.BorderSizeTop = 0;
                                        FpTabSpread.Sheets[0].Cells[sprow - 1, 1].Border.BorderSizeBottom = 1;
                                        FpTabSpread.Sheets[0].Cells[sprow - 1, 1].Border.BorderSizeRight = 1;
                                        FpTabSpread.Sheets[0].Cells[sprow - 1, 1].Border.BorderSizeLeft = 1;
                                        FpTabSpread.Sheets[0].Cells[sprow - 1, 1].Border.BorderColor = ColorTranslator.FromHtml("#000000");
                                        FpTabSpread.Sheets[0].Cells[sprow - 1, 1].Border.BorderColorBottom = ColorTranslator.FromHtml("#000000");
                                        FpTabSpread.Sheets[0].Cells[sprow - 1, 1].Border.BorderColorLeft = ColorTranslator.FromHtml("#000000");
                                        FpTabSpread.Sheets[0].Cells[sprow - 1, 1].Border.BorderColorRight = ColorTranslator.FromHtml("#000000");
                                        FpTabSpread.Sheets[0].Cells[sprow - 1, 1].Border.BorderColorTop = ColorTranslator.FromHtml("#000000");

                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 2].Text = studname;
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 2].Locked = true;
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 2].Font.Name = "Book Antiqua";
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 2].Font.Size = FontUnit.Point(10);
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 2].HorizontalAlign = HorizontalAlign.Center;
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 2].VerticalAlign = VerticalAlign.Middle;
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 2].Border.BorderSize = 1;
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 2].Border.BorderSizeTop = 1;
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 2].Border.BorderSizeBottom = 0;
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 2].Border.BorderSizeRight = 1;
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 2].Border.BorderSizeLeft = 1;
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 2].Border.BorderColor = ColorTranslator.FromHtml("#000000");
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 2].Border.BorderColorBottom = ColorTranslator.FromHtml("#000000");
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 2].Border.BorderColorLeft = ColorTranslator.FromHtml("#000000");
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 2].Border.BorderColorRight = ColorTranslator.FromHtml("#000000");
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 2].Border.BorderColorTop = ColorTranslator.FromHtml("#000000");

                                        FpTabSpread.Sheets[0].Cells[sprow - 2, 2].Font.Name = "AMUDHAM";
                                        FpTabSpread.Sheets[0].Cells[sprow - 2, 2].Text = studtamilname;
                                        FpTabSpread.Sheets[0].Cells[sprow - 2, 2].Locked = true;
                                        FpTabSpread.Sheets[0].Cells[sprow - 2, 2].Font.Size = FontUnit.Point(10);
                                        FpTabSpread.Sheets[0].Cells[sprow - 2, 2].HorizontalAlign = HorizontalAlign.Center;
                                        FpTabSpread.Sheets[0].Cells[sprow - 2, 2].VerticalAlign = VerticalAlign.Middle;
                                        FpTabSpread.Sheets[0].Cells[sprow - 2, 2].Border.BorderSize = 1;
                                        FpTabSpread.Sheets[0].Cells[sprow - 2, 2].Border.BorderSizeTop = 0;
                                        FpTabSpread.Sheets[0].Cells[sprow - 2, 2].Border.BorderSizeBottom = 1;
                                        FpTabSpread.Sheets[0].Cells[sprow - 2, 2].Border.BorderSizeRight = 1;
                                        FpTabSpread.Sheets[0].Cells[sprow - 2, 2].Border.BorderSizeLeft = 1;
                                        FpTabSpread.Sheets[0].Cells[sprow - 2, 2].Border.BorderColor = ColorTranslator.FromHtml("#000000");
                                        FpTabSpread.Sheets[0].Cells[sprow - 2, 2].Border.BorderColorBottom = ColorTranslator.FromHtml("#000000");
                                        FpTabSpread.Sheets[0].Cells[sprow - 2, 2].Border.BorderColorLeft = ColorTranslator.FromHtml("#000000");
                                        FpTabSpread.Sheets[0].Cells[sprow - 2, 2].Border.BorderColorRight = ColorTranslator.FromHtml("#000000");
                                        FpTabSpread.Sheets[0].Cells[sprow - 2, 2].Border.BorderColorTop = ColorTranslator.FromHtml("#000000");
                                        //sub_sem.Lab,sub_sem.ElectivePap,sub_sem.Disseration,sub_sem.projThe 

                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 3].Border.BorderSizeTop = 1;
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 3].Border.BorderSize = 0;
                                        FpTabSpread.Sheets[0].Cells[sprow - 2, 3].Border.BorderSize = 0;
                                        FpTabSpread.Sheets[0].Cells[sprow - 1, 3].Border.BorderSize = 0;
                                        FpTabSpread.Sheets[0].Cells[sprow - 1, 3].Border.BorderSizeBottom = 1;
                                        FpTabSpread.Sheets[0].Cells[sprow - 3, 3].Border.BorderColor = ColorTranslator.FromHtml("#000000");
                                        FpTabSpread.Sheets[0].Cells[sprow - 2, 3].Border.BorderColor = ColorTranslator.FromHtml("#000000");
                                        FpTabSpread.Sheets[0].Cells[sprow - 1, 3].Border.BorderColor = ColorTranslator.FromHtml("#000000");

                                        double written = 0;
                                        findWeightedAvgMarks(printds_rows, ref written, false);

                                        double practicalwam = 0;
                                        findWeightedAvgMarks(printds_rows, ref practicalwam, true);

                                        double decertion = 0;
                                        findWeightedAvgMarks(printds_rows, ref decertion, true);

                                        double overall = 0;
                                        findWeightedAvgMarks(printds_rows, ref overall, true);

                                        DataView dvelecSub = new DataView();
                                        printds_rows.Tables[0].DefaultView.RowFilter = "ElectivePap=1";
                                        dvelecSub = printds_rows.Tables[0].DefaultView;
                                        if (dvelecSub.Count > 0)
                                        {
                                            int elec = 3;
                                            for (int elce = 0; elce < dvelecSub.Count; elce++)
                                            {
                                                optionalsubjects = string.Empty;
                                                optionalsubjects = Convert.ToString(dvelecSub[elce]["subject_name"]).Trim();
                                                if (elce < 3)
                                                {
                                                    FpTabSpread.Sheets[0].Cells[sprow - elec, 3].Text = optionalsubjects;
                                                    FpTabSpread.Sheets[0].Cells[sprow - elec, 3].Locked = true;
                                                    FpTabSpread.Sheets[0].Cells[sprow - elec, 3].Font.Name = "Book Antiqua";
                                                    FpTabSpread.Sheets[0].Cells[sprow - elec, 3].Font.Size = FontUnit.Point(10);
                                                    FpTabSpread.Sheets[0].Cells[sprow - elec, 3].HorizontalAlign = HorizontalAlign.Center;
                                                    FpTabSpread.Sheets[0].Cells[sprow - elec, 3].VerticalAlign = VerticalAlign.Middle;
                                                    //FpTabSpread.Sheets[0].Cells[sprow - elec, 3].Border.BorderColorBottom = ColorTranslator.FromHtml("#000000");
                                                    //FpTabSpread.Sheets[0].Cells[sprow - elec, 3].Border.BorderSizeBottom = 0;
                                                    //FpTabSpread.Sheets[0].Cells[sprow - elec, 3].Border.BorderColorRight = ColorTranslator.FromHtml("#000000");
                                                    FpTabSpread.Sheets[0].Cells[sprow - elec, 3].Border.BorderSize = 1;
                                                    FpTabSpread.Sheets[0].Cells[sprow - elec, 3].Border.BorderSizeRight = 1;
                                                    FpTabSpread.Sheets[0].Cells[sprow - elec, 3].Border.BorderSizeLeft = 1;
                                                    FpTabSpread.Sheets[0].Cells[sprow - elec, 3].Border.BorderSizeTop = 0;
                                                    FpTabSpread.Sheets[0].Cells[sprow - elec, 3].Border.BorderSizeBottom = 0;
                                                    if (elce == 0)
                                                    {
                                                        FpTabSpread.Sheets[0].Cells[sprow - elec, 3].Border.BorderSizeTop = 1;
                                                        FpTabSpread.Sheets[0].Cells[sprow - elec, 3].Border.BorderSizeBottom = 0;
                                                    }
                                                    else if (elce == 2)
                                                    {
                                                        FpTabSpread.Sheets[0].Cells[sprow - elec, 3].Border.BorderSizeBottom = 1;
                                                    }
                                                    FpTabSpread.Sheets[0].Cells[sprow - elec, 3].Border.BorderColor = ColorTranslator.FromHtml("#000000");
                                                    FpTabSpread.Sheets[0].Cells[sprow - elec, 3].Border.BorderColorBottom = ColorTranslator.FromHtml("#000000");
                                                    FpTabSpread.Sheets[0].Cells[sprow - elec, 3].Border.BorderColorLeft = ColorTranslator.FromHtml("#000000");
                                                    FpTabSpread.Sheets[0].Cells[sprow - elec, 3].Border.BorderColorRight = ColorTranslator.FromHtml("#000000");
                                                    FpTabSpread.Sheets[0].Cells[sprow - elec, 3].Border.BorderColorTop = ColorTranslator.FromHtml("#000000");
                                                    elec--;
                                                }
                                                else
                                                {
                                                    FpTabSpread.Sheets[0].RowCount++;
                                                    FpTabSpread.Sheets[0].Cells[FpTabSpread.Sheets[0].RowCount - 1, 3].Text = optionalsubjects;
                                                    FpTabSpread.Sheets[0].Cells[FpTabSpread.Sheets[0].RowCount - 1, 3].Locked = true;
                                                    FpTabSpread.Sheets[0].Cells[FpTabSpread.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                                    FpTabSpread.Sheets[0].Cells[FpTabSpread.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Point(10);
                                                    FpTabSpread.Sheets[0].Cells[FpTabSpread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                                    FpTabSpread.Sheets[0].Cells[FpTabSpread.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                                                    FpTabSpread.Sheets[0].Cells[FpTabSpread.Sheets[0].RowCount - 1, 3].Border.BorderSizeTop = 0;
                                                    FpTabSpread.Sheets[0].Cells[FpTabSpread.Sheets[0].RowCount - 1, 3].Border.BorderSizeBottom = 0;
                                                    FpTabSpread.Sheets[0].Cells[FpTabSpread.Sheets[0].RowCount - 1, 3].Border.BorderSize = 1;
                                                    FpTabSpread.Sheets[0].Cells[FpTabSpread.Sheets[0].RowCount - 1, 3].Border.BorderSizeRight = 1;
                                                    FpTabSpread.Sheets[0].Cells[FpTabSpread.Sheets[0].RowCount - 1, 3].Border.BorderSizeLeft = 1;
                                                    FpTabSpread.Sheets[0].Cells[FpTabSpread.Sheets[0].RowCount - 1, 3].Border.BorderColor = ColorTranslator.FromHtml("#000000");
                                                    FpTabSpread.Sheets[0].Cells[FpTabSpread.Sheets[0].RowCount - 1, 3].Border.BorderColorBottom = ColorTranslator.FromHtml("#000000");
                                                    FpTabSpread.Sheets[0].Cells[FpTabSpread.Sheets[0].RowCount - 1, 3].Border.BorderColorLeft = ColorTranslator.FromHtml("#000000");
                                                    FpTabSpread.Sheets[0].Cells[FpTabSpread.Sheets[0].RowCount - 1, 3].Border.BorderColorRight = ColorTranslator.FromHtml("#000000");
                                                    FpTabSpread.Sheets[0].Cells[FpTabSpread.Sheets[0].RowCount - 1, 3].Border.BorderColorTop = ColorTranslator.FromHtml("#000000");
                                                    if (elce == dvelecSub.Count - 1)
                                                    {
                                                        FpTabSpread.Sheets[0].Cells[FpTabSpread.Sheets[0].RowCount - 1, 3].Border.BorderSizeBottom = 1; ;
                                                    }
                                                    FpTabSpread.Sheets[0].Cells[FpTabSpread.Sheets[0].RowCount - 1, 3].Border.BorderColorRight = ColorTranslator.FromHtml("#000000");

                                                }
                                            }
                                        }
                                        //}
                                        //else
                                        //{

                                        //}                                        


                                        string wrtclassify = string.Empty;
                                        string batchsetting = "1";
                                        DataSet gradeds = new DataSet();

                                        if (noofsubfailed != 0 && written >= 60)
                                        {
                                            string gradesqlclass = "select * from coe_classification where edu_level='" + edu_level + "'  and  '" + written + "'>= frompoint and '" + written + "'<= topoint and  markgradeflag='" + batchsetting + "'";//added by sridhar 16/aug 2014  and (classification='First Class' or classification='First')
                                            gradeds.Clear();
                                            gradeds = d2.select_method_wo_parameter(gradesqlclass, "Text");
                                            string cclass = "First Class";
                                            if (gradeds.Tables[0].Rows.Count > 0)
                                            {
                                                wrtclassify = cclass;
                                            }
                                            else
                                            {
                                                wrtclassify = cclass;
                                            }
                                        }
                                        else
                                        {
                                            string gradesqlclass = "select * from coe_classification where edu_level='" + edu_level + "'  and  '" + written + "'>= frompoint and '" + written + "'<= topoint and  markgradeflag='" + batchsetting + "'";//added by sridhar 16/aug 2014
                                            gradeds.Clear();
                                            gradeds = d2.select_method_wo_parameter(gradesqlclass, "Text");
                                            if (gradeds.Tables[0].Rows.Count > 0)
                                            {
                                                wrtclassify = Convert.ToString(gradeds.Tables[0].Rows[0]["classification"]);
                                            }
                                        }

                                        pracclassify = string.Empty;
                                        batchsetting = "1";
                                        string classnameTheory = string.Empty;
                                        string classnamePractical = string.Empty;
                                        if (noofsubfailed != 0 && practicalwam >= 60)
                                        {
                                            string gradesqlclass = "select * from coe_classification where edu_level='" + edu_level + "'  and  '" + practicalwam + "'>= frompoint and '" + practicalwam + "'<= topoint and  markgradeflag='" + batchsetting + "'";//added by sridhar 16/aug 2014  and (classification='First Class' or classification='First')
                                            gradeds.Clear();
                                            gradeds = d2.select_method_wo_parameter(gradesqlclass, "Text");
                                            string cclass = "First Class";
                                            if (gradeds.Tables[0].Rows.Count > 0)
                                            {
                                                pracclassify = cclass;
                                            }
                                            else
                                            {
                                                pracclassify = cclass;
                                            }

                                        }
                                        else
                                        {
                                            string gradesqlclass = "select * from coe_classification where edu_level='" + edu_level + "'  and  '" + practicalwam + "'>= frompoint and '" + practicalwam + "'<= topoint and  markgradeflag='" + batchsetting + "'";//added by sridhar 16/aug 2014
                                            gradeds.Clear();
                                            gradeds = d2.select_method_wo_parameter(gradesqlclass, "Text");
                                            if (gradeds.Tables[0].Rows.Count > 0)
                                            {
                                                pracclassify = Convert.ToString(gradeds.Tables[0].Rows[0]["classification"]);
                                            }
                                        }
                                        ChangeClassification(wrtclassify, out classnameTheory);
                                        FpTabSpread.Sheets[0].Cells[startrow, 4].Text = classnameTheory;
                                        FpTabSpread.Sheets[0].Cells[startrow, 4].Locked = true;
                                        FpTabSpread.Sheets[0].Cells[startrow, 4].Font.Name = "Book Antiqua";
                                        FpTabSpread.Sheets[0].Cells[startrow, 4].Font.Size = FontUnit.Point(10);
                                        FpTabSpread.Sheets[0].Cells[startrow, 4].HorizontalAlign = HorizontalAlign.Center;
                                        FpTabSpread.Sheets[0].Cells[startrow, 4].VerticalAlign = VerticalAlign.Middle;
                                        FpTabSpread.Sheets[0].SpanModel.Add(startrow, 4, FpTabSpread.Sheets[0].RowCount - startrow, 1);
                                        ChangeClassification(pracclassify, out classnamePractical);
                                        FpTabSpread.Sheets[0].Cells[startrow, 5].Text = classnamePractical;
                                        FpTabSpread.Sheets[0].Cells[startrow, 5].Locked = true;
                                        FpTabSpread.Sheets[0].Cells[startrow, 5].Font.Name = "Book Antiqua";
                                        FpTabSpread.Sheets[0].Cells[startrow, 5].Font.Size = FontUnit.Point(10);
                                        FpTabSpread.Sheets[0].Cells[startrow, 5].HorizontalAlign = HorizontalAlign.Center;
                                        FpTabSpread.Sheets[0].Cells[startrow, 5].VerticalAlign = VerticalAlign.Middle;
                                        FpTabSpread.Sheets[0].SpanModel.Add(startrow, 5, FpTabSpread.Sheets[0].RowCount - startrow, 1);

                                        string exam_y = Convert.ToString(ddlExamYear.SelectedItem.Text);
                                        string exam_m = Convert.ToString(ddlExamMonth.SelectedItem.Value);

                                        monthyear = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(exam_m));
                                        monthyear = exam_y + "-" + monthyear.ToUpper();

                                        FpTabSpread.Sheets[0].Cells[startrow, 6].Text = monthyear;
                                        FpTabSpread.Sheets[0].Cells[startrow, 6].Locked = true;
                                        FpTabSpread.Sheets[0].Cells[startrow, 6].Font.Name = "Book Antiqua";
                                        FpTabSpread.Sheets[0].Cells[startrow, 6].Font.Size = FontUnit.Point(10);
                                        FpTabSpread.Sheets[0].Cells[startrow, 6].HorizontalAlign = HorizontalAlign.Center;
                                        FpTabSpread.Sheets[0].Cells[startrow, 6].VerticalAlign = VerticalAlign.Middle;
                                        FpTabSpread.Sheets[0].SpanModel.Add(startrow, 6, FpTabSpread.Sheets[0].RowCount - startrow, 1);

                                        FpTabSpread.Sheets[0].Cells[startrow, 7].CellType = mi;
                                        FpTabSpread.Sheets[0].Cells[startrow, 7].HorizontalAlign = HorizontalAlign.Center;
                                        FpTabSpread.Sheets[0].Cells[startrow, 7].VerticalAlign = VerticalAlign.Middle;
                                        FpTabSpread.Sheets[0].SpanModel.Add(startrow, 7, FpTabSpread.Sheets[0].RowCount - startrow, 1);

                                        FpTabSpread.Sheets[0].Cells[startrow, 8].Text = medium;
                                        FpTabSpread.Sheets[0].Cells[startrow, 8].Locked = true;
                                        FpTabSpread.Sheets[0].Cells[startrow, 8].Font.Name = "Book Antiqua";
                                        FpTabSpread.Sheets[0].Cells[startrow, 8].Font.Size = FontUnit.Point(10);
                                        FpTabSpread.Sheets[0].Cells[startrow, 8].HorizontalAlign = HorizontalAlign.Center;
                                        FpTabSpread.Sheets[0].Cells[startrow, 8].VerticalAlign = VerticalAlign.Middle;
                                        FpTabSpread.Sheets[0].SpanModel.Add(startrow, 8, FpTabSpread.Sheets[0].RowCount - startrow, 1);

                                        FpTabSpread.Sheets[0].SpanModel.Add(startrow, 0, FpTabSpread.Sheets[0].RowCount - startrow, 1);
                                        FpTabSpread.Sheets[0].SpanModel.Add(startrow + 2, 1, FpTabSpread.Sheets[0].RowCount - startrow - 2, 1);
                                        FpTabSpread.Sheets[0].SpanModel.Add(startrow + 1, 2, FpTabSpread.Sheets[0].RowCount - startrow - 1, 1);
                                    }
                                }
                            }

                        }
                    }
                    FpTabSpread.Sheets[0].PageSize = FpTabSpread.Sheets[0].RowCount;
                    double height = (FpTabSpread.Sheets[0].RowCount * 25) + 50;
                    FpTabSpread.Width = 980;

                    if ((FpTabSpread.Sheets[0].RowCount * 25) + 50 < 500)
                        FpTabSpread.Height = (int)height;
                    FpTabSpread.SaveChanges();
                    if (FpTabSpread.Sheets[0].RowCount > 0)
                    {
                        FpTabSpread.Visible = true;
                        divTabSpread.Visible = true;
                        //divHeader.Visible = true;
                        rptprint1.Visible = true;
                        lblpoperr.Text = string.Empty;
                        popupdiv.Visible = false;
                    }
                    else
                    {
                        FpTabSpread.Visible = false;
                        divTabSpread.Visible = false;
                        //divHeader.Visible = false;
                        rptprint1.Visible = false;
                        lblpoperr.Text = "No Record(s) Found";
                        popupdiv.Visible = true;
                        return;
                    }
                }
                else
                {
                    lblpoperr.Text = "No Record(s) Found";
                    popupdiv.Visible = true;
                    return;
                }
            }
            else
            {
                lblpoperr.Text = "Please Check Degree,Exam Month And Exam Year!!!";
                popupdiv.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion GO Button

    #region Popup Error

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            popupdiv.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Popup Error

    #region Generate Excel

    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            Printcontrol1.Visible = false;
            string reportname = txtexcelname1.Text.Trim();
            if (Convert.ToString(reportname).Trim() != "")
            {
                if (FpTabSpread.Visible == true)
                {
                    d2.printexcelreport(FpTabSpread, reportname);
                }
                lbl_norec1.Visible = false;
            }
            else
            {
                lbl_norec1.Text = "Please Enter Your Report Name";
                lbl_norec1.Visible = true;
                txtexcelname1.Focus();
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Generate Excel

    #region Print PDF

    protected void btnprintmaster1_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            popupdiv.Visible = false;
            string rptheadname = string.Empty;
            rptheadname = "Tabulated Report";
            string pagename = "TabDesignNew.aspx";

            string Course_Name = Convert.ToString(ddldegree.SelectedItem);
            string exam_y1 = Convert.ToString(ddlExamYear.SelectedItem.Text);
            string exam_m1 = Convert.ToString(ddlExamMonth.SelectedItem.Value);

            string monthyear1 = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(exam_m1));
            monthyear1 = monthyear1.ToUpper() + " - " + exam_y1;

            //lblDegreeCourse.Text = Course_Name + "  DEGREE EXAMINATION " + monthyear1;

            rptheadname += "@ " + Course_Name + "  DEGREE EXAMINATION " + monthyear1 + "@ " + " Year of Admission : " + Convert.ToString(ddlbatch.SelectedItem);
            if (FpTabSpread.Visible == true)
            {
                Printcontrol1.loadspreaddetails(FpTabSpread, pagename, rptheadname);
            }
            Printcontrol1.Visible = true;
            lbl_norec1.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
        }
    }

    #endregion Print PDF

    public class MyImg : FarPoint.Web.Spread.ImageCellType
    {
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img.Width = Unit.Percentage(100);
            return img;
        }
    }

    // Developed By Malang raja 
    /// <summary>
    /// Author Malang Raja T For Christopher
    /// </summary>
    /// 
    /// <param name="ds"></param>
    /// <param name="wam"></param>
    /// <param name="islab"></param>
    public void findWeightedAvgMarks(DataSet ds, ref double wam, bool islab)
    {
        double credit = 0;
        double totCredit = 0;
        double mark = 0;
        double wam1 = 0;
        double weightmark = 0;
        string lab = string.Empty;
        DataView dvWam = new DataView();
        if (islab)
        {
            lab = "1";
        }
        else
        {
            lab = "0";
        }
        try
        {
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ds.Tables[0].DefaultView.RowFilter = "lab=" + lab + "";
                dvWam = ds.Tables[0].DefaultView;
                if (dvWam.Count > 0)
                {
                    wam1 = 0;
                    for (int i = 0; i < dvWam.Count; i++)
                    {
                        double maxTotal = 0;
                        double.TryParse(Convert.ToString(dvWam[i]["maxtotal"]), out maxTotal);
                        double.TryParse(Convert.ToString(dvWam[i]["credit_points"]), out credit);
                        double.TryParse(Convert.ToString(dvWam[i]["total"]), out mark);
                        double mark100 = mark;
                        if (islab)
                        {
                            if (mark > 0 && maxTotal > 0)
                                mark100 = (mark / maxTotal) * 100;
                        }
                        totCredit += credit;
                        weightmark = credit * mark100;
                        wam1 += weightmark;
                    }
                    //wam = Math.Round((wam1 / totCredit),0,MidpointRounding.AwayFromZero);
                    wam = (wam1 != 0 && totCredit != 0) ? Math.Round((wam1 / totCredit), 0, MidpointRounding.AwayFromZero) : 0;
                }
            }

        }
        catch (Exception ex)
        {

        }
    }

    // Developed By Malang raja 
    /// <summary>
    /// Author Malang Raja T For Christopher
    /// </summary>
    /// 
    /// <param name="ds"></param>
    /// <param name="wam"></param>
    /// <param name="islab"></param>
    public void findWeightedAvgMarks(DataSet ds, ref double wam, int type)
    {
        double credit = 0;
        double totCredit = 0;
        double mark = 0;
        double wam1 = 0;
        double weightmark = 0;
        string lab = string.Empty;
        DataView dvWam = new DataView();
        //if (isOverallorDecersion == true)
        //{
        //    lab = "1";
        //}
        //else
        //{
        //    lab = "0";
        //}
        if (type == 0)
        {

        }
        else
        {

        }

        try
        {
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //ds.Tables[0].DefaultView.RowFilter = "lab=" + lab + "";
                //dvWam = ds.Tables[0].DefaultView;
                //if (dvWam.Count > 0)
                //{
                wam1 = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    double.TryParse(Convert.ToString(ds.Tables[0].Rows[i]["credit_points"]), out credit);
                    double.TryParse(Convert.ToString(ds.Tables[0].Rows[i]["total"]), out mark);
                    totCredit += credit;
                    weightmark = credit * mark;
                    wam1 += weightmark;
                }
                //wam = Math.Round((wam1 / totCredit),0,MidpointRounding.AwayFromZero);
                wam = (wam1 != 0 && totCredit != 0) ? Math.Round((wam1 / totCredit), 0, MidpointRounding.AwayFromZero) : 0;
                //}
            }

        }
        catch (Exception ex)
        {

        }
    }

    public void ChangeClassification(string classify, out string newclassify)
    {
        newclassify = string.Empty;
        try
        {
            string classname = classify.Trim().ToLower();
            switch (classname)
            {
                case "first class distinction":
                case "ist class distinction":
                case "i-st class distinction":
                case "i-st class with distinction":
                case "first class with distinction":
                case "ist class with distinction":
                case "1st class with distinction":
                case "1-st class with distinction":
                    newclassify = "I-D";
                    break;
                case "first class":
                case "ist class":
                case "i-st class":
                case "1st class":
                case "1-st class":
                    newclassify = "I";
                    break;
                case "second class":
                case "iind class":
                case "ii-nd class":
                case "2nd class":
                case "2-nd class":
                    newclassify = "II";
                    break;
                case "third class":
                case "iiird class":
                case "iii-rd class":
                case "3rd class":
                case "3-rd class":
                    newclassify = "III";
                    break;
            }
        }
        catch (Exception ex)
        {

        }
    }

    #endregion

}