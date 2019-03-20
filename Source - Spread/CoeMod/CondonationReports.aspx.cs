using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Web.UI.WebControls;
using Farpoint = FarPoint.Web.Spread;
using System.Configuration;

public partial class CondonationReports : System.Web.UI.Page
{

    DAccess2 d2 = new DAccess2();
    DAccess2 da = new DAccess2();

    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();

    Hashtable hashmark = new Hashtable();
    Hashtable has = new Hashtable();
    Hashtable hat = new Hashtable();

    Boolean flag_true = false;
    ArrayList alv = new ArrayList();

    static int isHeaderwise = 0;
    int selDegree = 0;
    int selBranch = 0;
    int selSec = 0;
    int selCondo = 0;

    string group_user = string.Empty;
    string singleuser = string.Empty;
    string usercode = string.Empty;

    string collegecode = string.Empty;
    string collegecode1 = string.Empty;

    string user_code = string.Empty;
    string collegeCode = string.Empty;

    string newCollegeCode = string.Empty;
    string newBatchYear = string.Empty;
    string newDegreeCode = string.Empty;
    string newBranchCode = string.Empty;
    string newsemester = string.Empty;
    string newsections = string.Empty;
    string condonationType = string.Empty;

    string qryCollege = string.Empty;
    string qryBatch = string.Empty;
    string qryDegree = string.Empty;
    string qryBranch = string.Empty;
    string qrySem = string.Empty;
    string qrySec = string.Empty;

    string qryRollOrRegNo = string.Empty;

    #region Page Load

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
            if (!IsPostBack)
            {
                LoadCollege();
                BindBatch();
                BindDegree();
                BindBranch();
                BindSem();
                lblErrmsg.Text = string.Empty;
                lblErrmsg.Visible = false;
                rptprint1.Visible = false;
                divCondonation.Visible = false;

                if (chkReport.Checked == true)
                {
                    foreach (ListItem li in cblReport.Items)
                    {
                        li.Selected = true;
                    }
                    txtReport.Text = "Report (" + (cblReport.Items.Count) + ")";
                }
                else
                {
                    foreach (ListItem li in cblReport.Items)
                    {
                        li.Selected = false;
                    }
                    txtReport.Text = "-- Select --";
                }
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";

                string grouporusercode = "";

                if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                {
                    grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
                }
                else
                {
                    grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
                }
                user_code = Convert.ToString(Session["usercode"]).Trim();

                string Master = "select * from Master_Settings where " + grouporusercode + "";
                DataSet ds = d2.select_method(Master, hat, "Text");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (Convert.ToString(ds.Tables[0].Rows[i]["settings"]).Trim().ToLower() == "roll no" && Convert.ToString(ds.Tables[0].Rows[i]["value"]).Trim() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (Convert.ToString(ds.Tables[0].Rows[i]["settings"]).Trim().ToLower() == "register no" && Convert.ToString(ds.Tables[0].Rows[i]["value"]).Trim() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (Convert.ToString(ds.Tables[0].Rows[i]["settings"]).Trim().ToLower() == "student_type" && Convert.ToString(ds.Tables[0].Rows[i]["value"]).Trim() == "1")
                    {
                        Session["Studflag"] = "1";
                    }
                }

            }
            collegecode1 = ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13");
            if (Session["usercode"] != null)
            {
                user_code = Convert.ToString(Session["usercode"]).Trim();
                usercode = user_code;
            }
            if (Session["single_user"] != null)
            {
                singleuser = Convert.ToString(Session["single_user"]).Trim();
            }
        }
        catch (ThreadAbortException tt)
        {

        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Page Load

    #region Logout

    protected void lbtnLogout_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            Response.Redirect("default.aspx");
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Logout

    #region Bind Header

    public void LoadCollege()
    {
        try
        {
            string group_code = Convert.ToString(Session["group_code"]).Trim();
            string columnfield = "";
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = Convert.ToString(group_semi[0]).Trim();
            }
            if ((Convert.ToString(group_code).Trim() != "") && (Convert.ToString(Session["single_user"]).Trim() != "1" && Convert.ToString(Session["single_user"]).Trim().ToLower() != "true" && Convert.ToString(Session["single_user"]).Trim() != "TRUE" && Convert.ToString(Session["single_user"]).Trim() != "True"))
            {
                columnfield = " and group_code='" + group_code + "'";
            }
            else
            {
                columnfield = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            hat.Clear();
            hat.Add("column_field", Convert.ToString(columnfield));
            ds = da.select_method("bind_college", hat, "sp");
            ddlCollege.Items.Clear();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlCollege.Enabled = true;
                ddlCollege.DataSource = ds;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    //public void BindBatch()
    //{
    //    try
    //    {
    //        ddlBatch.Items.Clear();
    //        ds = da.BindBatch();
    //        if (ds.Tables.Count > 0)
    //        {
    //            int count = ds.Tables[0].Rows.Count;
    //            if (count > 0)
    //            {
    //                ddlBatch.DataSource = ds;
    //                ddlBatch.DataTextField = "batch_year";
    //                ddlBatch.DataValueField = "batch_year";
    //                ddlBatch.DataBind();
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrmsg.Text = Convert.ToString(ex);
    //        lblErrmsg.Visible = true;
    //        d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
    //    }
    //}

    public void BindBatch()
    {
        try
        {
            chklsbatch.Items.Clear();
            ds = da.BindBatch();
            if (ds.Tables.Count > 0)
            {
                int count = ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    chklsbatch.DataSource = ds;
                    chklsbatch.DataTextField = "batch_year";
                    chklsbatch.DataValueField = "batch_year";
                    chklsbatch.DataBind();
                }

                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = true;

                }
                txtbatch.Text = lblBatch.Text + "(" + chklsbatch.Items.Count + ")";
                chkbatch.Checked = true;
            }
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }



    public void BindDegree()
    {
        try
        {
            ddlDegree.Items.Clear();
            cblDegree.Items.Clear();
            chkDegree.Checked = false;
            txtDegree.Text = "-- Select --";
            usercode = Convert.ToString(Session["usercode"]).Trim();
            singleuser = Convert.ToString(Session["single_user"]).Trim();
            group_user = Convert.ToString(Session["group_code"]).Trim();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]).Trim();
            }
            has.Clear();
            has.Add("single_user", singleuser);
            has.Add("group_code", group_user);
            has.Add("college_code", Convert.ToString(ddlCollege.SelectedValue).Trim());
            has.Add("user_code", usercode);
            ds = da.select_method("bind_degree", has, "sp");
            if (ds.Tables.Count > 0)
            {
                int count1 = ds.Tables[0].Rows.Count;
                if (count1 > 0)
                {
                    ddlDegree.DataSource = ds;
                    ddlDegree.DataTextField = "course_name";
                    ddlDegree.DataValueField = "course_id";
                    ddlDegree.DataBind();

                    cblDegree.DataSource = ds;
                    cblDegree.DataTextField = "course_name";
                    cblDegree.DataValueField = "course_id";
                    cblDegree.DataBind();

                    foreach (ListItem li in cblDegree.Items)
                    {
                        li.Selected = true;
                    }
                    txtDegree.Text = "Degree" + "(" + cblDegree.Items.Count + ")";
                    chkDegree.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public void BindBranch()
    {
        try
        {
            ddlBranch.Items.Clear();
            chkBranch.Checked = false;
            cblBranch.Items.Clear();
            txtBranch.Text = "-- Select --";
            hat.Clear();
            group_user = Convert.ToString(Session["group_code"]).Trim();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]).Trim();
            }
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("course_id", ddlDegree.SelectedValue);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);

            //string typeval = "";
            //if (ddlstream.Items.Count > 0 && ddlstream.Enabled == true)
            //{
            //    typeval = " and type='" + ddlstream.SelectedItem.ToString() + "'";
            //}
            selDegree = 0;
            newDegreeCode = string.Empty;
            qryDegree = string.Empty;
            string coursecode = string.Empty;
            foreach (ListItem li in cblDegree.Items)
            {
                if (li.Selected)
                {
                    selDegree++;
                    if (string.IsNullOrEmpty(newDegreeCode.Trim()))
                    {
                        newDegreeCode = "'" + li.Value + "'";
                    }
                    else
                    {
                        newDegreeCode += ",'" + li.Value + "'";
                    }
                }
            }
            if (selDegree > 0)
            {
                coursecode = " and degree.course_id in(" + newDegreeCode + ")";

                string strquery = "select distinct degree.degree_code,de.dept_name from degree,course, deptprivilages,department de where course.course_id=degree.course_id and de.dept_code=degree.dept_code and course.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code and course.college_code ='" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "' and user_code='" + usercode + "' " + " " + coursecode + "";
                if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                {
                    strquery = "select distinct degree.degree_code,de.dept_name from degree,course, deptprivilages,department de where course.course_id=degree.course_id and de.dept_code=degree.dept_code and course.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code and group_code='" + group_user + "' and course.college_code ='" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "' " + "  " + coursecode + "";
                }
                ds = d2.select_method_wo_parameter(strquery, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int count2 = ds.Tables[0].Rows.Count;
                if (count2 > 0)
                {
                    ddlBranch.DataSource = ds;
                    ddlBranch.DataTextField = "dept_name";
                    ddlBranch.DataValueField = "degree_code";
                    ddlBranch.DataBind();

                    cblBranch.DataSource = ds;
                    cblBranch.DataTextField = "dept_name";
                    cblBranch.DataValueField = "degree_code";
                    cblBranch.DataBind();

                    foreach (ListItem li in cblBranch.Items)
                    {
                        li.Selected = true;
                    }

                    txtBranch.Text = "Branch" + "(" + cblBranch.Items.Count + ")";
                    chkBranch.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public void BindSem()
    {
        try
        {
            lblErrmsg.Text = string.Empty;
            lblErrmsg.Visible = false;
            string strbatchyear = string.Empty;
            string strbranch = string.Empty;
            ddlSem.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            if (ddlCollege.Items.Count > 0)
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }
            if (chklsbatch.Items.Count > 0)
            {
                strbatchyear =  Convert.ToString(getCblSelectedValue(chklsbatch)).Trim();
            }
            newDegreeCode = string.Empty;
            selBranch = 0;
            foreach (ListItem li in cblBranch.Items)
            {
                if (li.Selected)
                {
                    selBranch++;
                    if (string.IsNullOrEmpty(newDegreeCode))
                    {
                        newDegreeCode = "'" + li.Value + "'";
                    }
                    else
                    {
                        newDegreeCode += ",'" + li.Value + "'";
                    }
                }
            }

            ds.Dispose();
            ds.Reset();
            ddlSem.Items.Clear();

            string qry = string.Empty;

            //ddlSem.Items.Count = 0;
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(newDegreeCode) && !string.IsNullOrEmpty(strbatchyear))
            {
                qry = "select distinct max(ndurations) as ndurations,first_year_nonsemester from ndegree where degree_code in (" + newDegreeCode + ") and batch_year in ('" + strbatchyear + "') and college_code='" + collegeCode + "' group by first_year_nonsemester order by ndurations desc; select distinct max(duration) duration,first_year_nonsemester from degree where degree_code in (" + newDegreeCode + ") and college_code='" + collegeCode + "' group by first_year_nonsemester order by duration desc";
                ds = da.select_method_wo_parameter(qry, "Text");
                //ds = d2.BindSem(newDegreeCode, strbatchyear, collegeCode);
            }
            if (ds.Tables.Count > 0)
            {
                //first_year = Convert.ToBoolean(Convert.ToString(ds.Tables[0].Rows[0][1]));
                //duration = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0][0]));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    bool.TryParse(Convert.ToString(ds.Tables[0].Rows[0][1]).Trim(), out first_year);
                    int.TryParse(Convert.ToString(ds.Tables[0].Rows[0][0]).Trim(), out duration);
                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            ddlSem.Items.Add(Convert.ToString(i));
                        }
                        else if (first_year == true && i != 2)
                        {
                            ddlSem.Items.Add(Convert.ToString(i));
                        }
                    }
                }
                else if (ds.Tables[1].Rows.Count > 0)
                {
                    bool.TryParse(Convert.ToString(ds.Tables[1].Rows[0][1]).Trim(), out first_year);
                    int.TryParse(Convert.ToString(ds.Tables[1].Rows[0][0]).Trim(), out duration);
                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            ddlSem.Items.Add(Convert.ToString(i));
                        }
                        else if (first_year == true && i != 2)
                        {
                            ddlSem.Items.Add(Convert.ToString(i));
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion

    #region Header Changed Events

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrmsg.Text = string.Empty;
            lblErrmsg.Visible = false;
            rptprint1.Visible = false;
            divCondonation.Visible = false;

            BindBatch();
            BindDegree();
            BindBranch();
            BindSem();

        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void chkbatch_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(chkbatch, chklsbatch, txtbatch, lblBatch.Text, "--Select--");
        lblErrmsg.Text = string.Empty;
        lblErrmsg.Visible = false;
        rptprint1.Visible = false;
        divCondonation.Visible = false;
        BindDegree();
        BindBranch();
        BindSem();
    }
    protected void chklstbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(chkbatch, chklsbatch, txtbatch, lblBatch.Text, "--Select--");
        lblErrmsg.Text = string.Empty;
        lblErrmsg.Visible = false;
        rptprint1.Visible = false;
        divCondonation.Visible = false;
        BindDegree();
        BindBranch();
        BindSem();
    }

    //protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        lblErrmsg.Text = string.Empty;
    //        lblErrmsg.Visible = false;
    //        rptprint1.Visible = false;
    //        divCondonation.Visible = false;
    //        BindDegree();
    //        BindBranch();
    //        BindSem();
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrmsg.Text = Convert.ToString(ex);
    //        lblErrmsg.Visible = true;
    //        d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
    //    }
    //}

    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrmsg.Text = string.Empty;
            lblErrmsg.Visible = false;
            rptprint1.Visible = false;
            divCondonation.Visible = false;
            BindBranch();
            BindSem();
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrmsg.Text = string.Empty;
            lblErrmsg.Visible = false;
            rptprint1.Visible = false;
            divCondonation.Visible = false;
            BindSem();
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void chkDegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrmsg.Text = string.Empty;
            lblErrmsg.Visible = false;
            rptprint1.Visible = false;
            divCondonation.Visible = false;
            int count = 0;
            if (chkDegree.Checked == true)
            {
                count++;
                for (int i = 0; i < cblDegree.Items.Count; i++)
                {
                    cblDegree.Items[i].Selected = true;
                }
                txtDegree.Text = "Degree (" + (cblDegree.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cblDegree.Items.Count; i++)
                {
                    cblDegree.Items[i].Selected = false;
                }
                txtDegree.Text = "-- Select --";
            }
            BindBranch();
            BindSem();
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void cblDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrmsg.Text = string.Empty;
            lblErrmsg.Visible = false;
            rptprint1.Visible = false;
            divCondonation.Visible = false;
            int commcount = 0;
            txtDegree.Text = "-- Select --";
            chkDegree.Checked = false;
            for (int i = 0; i < cblDegree.Items.Count; i++)
            {
                if (cblDegree.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cblDegree.Items.Count)
                {
                    chkDegree.Checked = true;
                }
                txtDegree.Text = "Degree (" + Convert.ToString(commcount) + ")";
            }
            BindBranch();
            BindSem();
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void chkBranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrmsg.Text = string.Empty;
            lblErrmsg.Visible = false;
            rptprint1.Visible = false;
            divCondonation.Visible = false;
            int count = 0;
            if (chkBranch.Checked == true)
            {
                for (int i = 0; i < cblBranch.Items.Count; i++)
                {
                    count++;
                    cblBranch.Items[i].Selected = true;
                }
                txtBranch.Text = "Branch (" + (cblBranch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cblBranch.Items.Count; i++)
                {
                    cblBranch.Items[i].Selected = false;
                }
                txtBranch.Text = "-- Select --";
            }
            BindSem();
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void cblBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrmsg.Text = string.Empty;
            lblErrmsg.Visible = false;
            rptprint1.Visible = false;
            divCondonation.Visible = false;
            int commcount = 0;
            txtBranch.Text = "-- Select --";
            chkBranch.Checked = false;
            for (int i = 0; i < cblBranch.Items.Count; i++)
            {
                if (cblBranch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cblBranch.Items.Count)
                {
                    chkBranch.Checked = true;
                }
                txtBranch.Text = "Branch (" + Convert.ToString(commcount) + ")";
            }
            BindSem();
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlSem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrmsg.Text = string.Empty;
            lblErrmsg.Visible = false;
            rptprint1.Visible = false;
            divCondonation.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void chkReport_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrmsg.Text = string.Empty;
            lblErrmsg.Visible = false;
            rptprint1.Visible = false;
            divCondonation.Visible = false;
            int count = 0;
            txtReport.Text = "-- Select --";
            cblReport.ClearSelection();
            if (chkReport.Checked == true)
            {
                foreach (ListItem li in cblReport.Items)
                {
                    count++;
                    li.Selected = true;
                }
                txtReport.Text = "Report (" + (cblReport.Items.Count) + ")";
            }
            else
            {
                foreach (ListItem li in cblReport.Items)
                {
                    count++;
                    li.Selected = false;
                }
                txtReport.Text = "-- Select --";
            }
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void cblReport_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrmsg.Text = string.Empty;
            lblErrmsg.Visible = false;
            rptprint1.Visible = false;
            divCondonation.Visible = false;
            int commcount = 0;
            txtReport.Text = "-- Select --";
            chkReport.Checked = false;
            //for (int i = 0; i < cblBranch.Items.Count; i++)
            //{
            //    if (cblBranch.Items[i].Selected == true)
            //    {
            //        commcount = commcount + 1;
            //    }
            //}
            foreach (ListItem li in cblReport.Items)
            {
                if (li.Selected)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cblReport.Items.Count)
                {
                    chkReport.Checked = true;
                }
                txtReport.Text = "Report (" + Convert.ToString(commcount) + ")";
            }
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion

    #region Generate Excel

    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol1.Visible = false;
            string reportname = txtexcelname1.Text.Trim().Replace(" ", "_");
            if (Convert.ToString(reportname).Trim() != "")
            {
                if (FpCondonation.Visible == true)
                {
                    da.printexcelreport(FpCondonation, reportname);
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
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Generate Excel

    #region Print PDF

    protected void btnprintmaster1_Click(object sender, EventArgs e)
    {
        try
        {
            string rptheadname = "Condonation Report";
            string pagename = "Nominal_Roll.aspx";
            if (FpCondonation.Visible == true)
            {
                Printcontrol1.loadspreaddetails(FpCondonation, pagename, rptheadname);
            }
            Printcontrol1.Visible = true;
            lbl_norec1.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Print PDF

    #region Go Click

    public void Init_Spread()
    {
        try
        {
            #region FpSpread Style

            FpCondonation.Visible = false;
            FpCondonation.Sheets[0].ColumnCount = 0;
            FpCondonation.Sheets[0].RowCount = 0;
            FpCondonation.Sheets[0].SheetCorner.ColumnCount = 0;
            FpCondonation.CommandBar.Visible = false;

            #endregion FpSpread Style

            FpCondonation.Visible = false;
            FpCondonation.CommandBar.Visible = false;
            FpCondonation.RowHeader.Visible = false;
            FpCondonation.Sheets[0].AutoPostBack = false;
            FpCondonation.Sheets[0].RowCount = 0;
            FpCondonation.Sheets[0].ColumnCount = 9;
            FpCondonation.Sheets[0].FrozenRowCount = 0;

            #region SpreadStyles

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#00aff0");
            //darkstyle.ForeColor = System.Drawing.Color.Black;
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Size = FontUnit.Medium;
            darkstyle.Font.Bold = true;
            darkstyle.HorizontalAlign = HorizontalAlign.Center;
            darkstyle.VerticalAlign = VerticalAlign.Middle;
            darkstyle.ForeColor = System.Drawing.Color.White;
            darkstyle.Border.BorderSize = 1;
            darkstyle.Border.BorderColor = System.Drawing.Color.Black;

            FarPoint.Web.Spread.StyleInfo sheetstyle = new FarPoint.Web.Spread.StyleInfo();
            //sheetstyle.BackColor = ColorTranslator.FromHtml("#00aff0");
            //darkstyle.ForeColor = System.Drawing.Color.Black;
            sheetstyle.Font.Name = "Book Antiqua";
            sheetstyle.Font.Size = FontUnit.Medium;
            sheetstyle.Font.Bold = true;
            sheetstyle.HorizontalAlign = HorizontalAlign.Left;
            sheetstyle.VerticalAlign = VerticalAlign.Middle;
            sheetstyle.ForeColor = System.Drawing.Color.Black;
            sheetstyle.Border.BorderSize = 1;
            sheetstyle.Border.BorderColor = System.Drawing.Color.Black;

            #endregion SpreadStyles

            Farpoint.TextCellType txtCellType = new Farpoint.TextCellType();

            FpCondonation.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpCondonation.Sheets[0].DefaultStyle = sheetstyle;
            FpCondonation.Sheets[0].ColumnHeader.RowCount = 2;

            FpCondonation.Sheets[0].Columns[0].Locked = true;
            FpCondonation.Sheets[0].Columns[1].Locked = true;
            FpCondonation.Sheets[0].Columns[2].Locked = true;
            FpCondonation.Sheets[0].Columns[3].Locked = true;
            FpCondonation.Sheets[0].Columns[4].Locked = true;
            FpCondonation.Sheets[0].Columns[5].Locked = true;
            FpCondonation.Sheets[0].Columns[6].Locked = true;

            FpCondonation.Sheets[0].Columns[0].Resizable = false;
            FpCondonation.Sheets[0].Columns[1].Resizable = false;
            FpCondonation.Sheets[0].Columns[2].Resizable = false;
            FpCondonation.Sheets[0].Columns[3].Resizable = false;
            FpCondonation.Sheets[0].Columns[4].Resizable = false;
            FpCondonation.Sheets[0].Columns[5].Resizable = false;
            FpCondonation.Sheets[0].Columns[6].Resizable = false;

            FpCondonation.Sheets[0].Columns[0].Width = 40;
            FpCondonation.Sheets[0].Columns[1].Width = 210;
            FpCondonation.Sheets[0].Columns[2].Width = 100;
            FpCondonation.Sheets[0].Columns[3].Width = 100;
            FpCondonation.Sheets[0].Columns[4].Width = 190;
            FpCondonation.Sheets[0].Columns[5].Width = 80;
            FpCondonation.Sheets[0].Columns[6].Width = 80;
            //----------- added by Deepali on 30.3.18
            FpCondonation.Sheets[0].Columns[7].Width = 130;
            FpCondonation.Sheets[0].Columns[8].Width = 130;
            //------------
            FpCondonation.Sheets[0].Columns[2].CellType = txtCellType;
            FpCondonation.Sheets[0].Columns[3].CellType = txtCellType;

            FpCondonation.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpCondonation.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            FpCondonation.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            FpCondonation.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
            FpCondonation.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            FpCondonation.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
            FpCondonation.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;

            FpCondonation.Sheets[0].AutoPostBack = false;
            FpCondonation.Sheets[0].AutoPostBack = true;
            FpCondonation.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpCondonation.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Degree Name";
            FpCondonation.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
            FpCondonation.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg No";
            FpCondonation.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Name";
            FpCondonation.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Fees";
            FpCondonation.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Status";
            FpCondonation.Sheets[0].Columns[6].Locked = true;
            //----------- added by Deepali on 30.3.18
            FpCondonation.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Challan/Receipt No.";
            FpCondonation.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Challan/Receipt Date";
            //------------
            if (Session["Rollflag"].ToString() == "1")
            {
                FpCondonation.Sheets[0].Columns[2].Visible = true;
            }
            else
            {
                FpCondonation.Sheets[0].Columns[2].Visible = false;
            }

            if (Session["Regflag"].ToString() == "1")
            {
                FpCondonation.Sheets[0].Columns[3].Visible = true;
            }
            else
            {
                FpCondonation.Sheets[0].Columns[3].Visible = false;
            }

            FpCondonation.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            FpCondonation.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            FpCondonation.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            FpCondonation.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
            FpCondonation.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
            FpCondonation.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
            FpCondonation.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
            //----------- added by Deepali on 30.3.18
            FpCondonation.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);
            FpCondonation.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 2, 1);
            //-----
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrmsg.Text = string.Empty;
            lblErrmsg.Visible = false;
            rptprint1.Visible = false;
            divCondonation.Visible = false;
            selBranch = 0;
            selDegree = 0;
            selCondo = 0;

            condonationType = string.Empty;
            newCollegeCode = string.Empty;
            newBatchYear = string.Empty;
            newBranchCode = string.Empty;
            newDegreeCode = string.Empty;
            newsemester = string.Empty;
            newsections = string.Empty;
            collegeCode = string.Empty;

            qryCollege = string.Empty;
            qryBatch = string.Empty;
            qryDegree = string.Empty;
            qryBranch = string.Empty;
            qrySem = string.Empty;
            if (ddlCollege.Items.Count > 0)
            {
                newCollegeCode = collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }
            else
            {
                lblPopupAlert.Text = "No College were Found";
                lblPopupAlert.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }
            if (chklsbatch.Items.Count > 0)
            {
                newBatchYear = Convert.ToString(getCblSelectedText(chklsbatch)).Trim();
            }
            else
            {
                lblPopupAlert.Text = "No Batch were Found";
                lblPopupAlert.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }
            if (cblDegree.Items.Count > 0)
            {
                newDegreeCode = string.Empty;
                selDegree = 0;
                qryDegree = string.Empty;
                foreach (ListItem li in cblDegree.Items)
                {
                    if (li.Selected)
                    {
                        selDegree++;
                        if (string.IsNullOrEmpty(newDegreeCode))
                        {
                            newDegreeCode = "'" + Convert.ToString(li.Value).Trim() + "'";
                        }
                        else
                        {
                            newDegreeCode += ",'" + Convert.ToString(li.Value).Trim() + "'";
                        }
                    }
                }
                if (selDegree == 0)
                {
                    lblPopupAlert.Text = "Please Select Any One Degree";
                    lblPopupAlert.Visible = true;
                    divPopupAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblPopupAlert.Text = "No Degree were Found";
                lblPopupAlert.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }
            if (cblBranch.Items.Count > 0)
            {
                newBranchCode = string.Empty;
                selBranch = 0;
                qryBranch = string.Empty;
                foreach (ListItem li in cblBranch.Items)
                {
                    if (li.Selected)
                    {
                        selBranch++;
                        if (string.IsNullOrEmpty(newBranchCode))
                        {
                            newBranchCode = "'" + Convert.ToString(li.Value).Trim() + "'";
                        }
                        else
                        {
                            newBranchCode += ",'" + Convert.ToString(li.Value).Trim() + "'";
                        }
                    }
                }
                if (selBranch == 0)
                {
                    lblPopupAlert.Text = "Please Select Any One Branch";
                    lblPopupAlert.Visible = true;
                    divPopupAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblPopupAlert.Text = "No Branch were Found";
                lblPopupAlert.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }
            if (ddlSem.Items.Count > 0)
            {
                newsemester = Convert.ToString(ddlSem.SelectedItem.Text).Trim();
                qrySem = string.Empty;
            }
            else
            {
                lblPopupAlert.Text = "No Semester were Found";
                lblPopupAlert.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }
            if (cblReport.Items.Count > 0)
            {
                selCondo = 0;
                condonationType = string.Empty;
                foreach (ListItem li in cblReport.Items)
                {
                    if (li.Selected)
                    {
                        selCondo++;
                        if (string.IsNullOrEmpty(condonationType))
                        {
                            condonationType = "'" + Convert.ToString(li.Value).Trim() + "'";
                        }
                        else
                        {
                            condonationType += ",'" + Convert.ToString(li.Value).Trim() + "'";
                        }
                    }
                }
                if (selCondo == 0)
                {
                    lblPopupAlert.Text = "Please Select Any One Report Type";
                    lblPopupAlert.Visible = true;
                    divPopupAlert.Visible = true;
                    return;
                }
            }
            else
            {
                lblPopupAlert.Text = "No Report were Found";
                lblPopupAlert.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }
            string qry = string.Empty;
            DataSet dsCondo = new DataSet();
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(newBatchYear) && !string.IsNullOrEmpty(newBranchCode) && !string.IsNullOrEmpty(newsemester) && !string.IsNullOrEmpty(condonationType))
            {
                qry = "select r.college_code,r.Batch_Year,r.degree_code,case when isnull(ltrim(rtrim(c.Edu_Level)),'')<>'' then isnull(ltrim(rtrim(c.Edu_Level)),'')+' - ' else '' end + c.Course_Name+' - '+dt.Dept_Name + case when isnull(ltrim(rtrim(r.Sections)),'')<>'' then ' - '+isnull(ltrim(rtrim(r.Sections)),'') else '' end as Degree_Name,isnull(el.is_eligible,'') is_eligible, r.serialno,r.App_No,r.Roll_No,r.Reg_No,r.Stud_Name,r.Stud_Type,isnull(el.fine_amt,'0') total_fee,CASE WHEN isnull(el.isCondonationFee,'0')='1' THEN 'Paid' ELSE 'Unpaid' END status,isnull(r.Sections,'') Sections,el.challandate,el.challanno from Eligibility_list as el,Registration r,Course c,Degree dg,Department dt where dg.Dept_Code=dt.Dept_Code and c.Course_Id=dg.Course_Id and dg.Degree_Code=r.degree_code and dg.Degree_Code=el.degree_code and r.App_No=el.app_no and r.Batch_Year=el.batch_year and r.degree_code=el.degree_code and r.Roll_No=el.Roll_no and is_eligible in(" + condonationType + ") and r.Batch_Year in('" + newBatchYear + "') and r.degree_code in(" + newBranchCode + ") and el.Semester in(" + newsemester + ") and r.college_code='" + collegeCode + "' order by is_eligible,status,r.college_code,r.Batch_Year,c.Course_Id,r.degree_code,isnull(ltrim(rtrim(r.Sections)),''),r.Reg_No";
                dsCondo = d2.select_method_wo_parameter(qry, "text");

                if (dsCondo.Tables.Count > 0 && dsCondo.Tables[0].Rows.Count > 0)
                {
                    Init_Spread();

                    DataTable dtEligible = new DataTable();
                    DataTable dtCondo = new DataTable();
                    DataTable dtNotEligible = new DataTable();

                    dsCondo.Tables[0].DefaultView.RowFilter = "is_eligible='1'";
                    dtEligible = dsCondo.Tables[0].DefaultView.ToTable();

                    dsCondo.Tables[0].DefaultView.RowFilter = "is_eligible='2'";
                    dtCondo = dsCondo.Tables[0].DefaultView.ToTable();

                    dsCondo.Tables[0].DefaultView.RowFilter = "is_eligible='3'";
                    dtNotEligible = dsCondo.Tables[0].DefaultView.ToTable();

                    if (dtEligible.Rows.Count > 0)
                    {
                        FpCondonation.Sheets[0].RowCount++;
                        FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 0].Text = "Eligible Students";
                        FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 0].BackColor = System.Drawing.Color.Green;
                        FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                        //FpCondonation.Sheets[0].AddSpanCell(FpCondonation.Sheets[0].RowCount - 1, 0, 1, 7);
                        FpCondonation.Sheets[0].AddSpanCell(FpCondonation.Sheets[0].RowCount - 1, 0, 1, 9);// modified by Deepali on 30.3.18
                        int sno = 1;

                        foreach (DataRow drCondo in dtEligible.Rows)
                        {
                            string serialNo = Convert.ToString(drCondo["serialno"]).Trim();
                            string appNo = Convert.ToString(drCondo["App_No"]).Trim();
                            string isEligible = Convert.ToString(drCondo["is_eligible"]).Trim();
                            string degreeName = Convert.ToString(drCondo["Degree_Name"]).Trim();
                            string rollNo = Convert.ToString(drCondo["Roll_No"]).Trim();
                            string regNo = Convert.ToString(drCondo["Reg_No"]).Trim();
                            string studentName = Convert.ToString(drCondo["Stud_Name"]).Trim();
                            string studentType = Convert.ToString(drCondo["Stud_Type"]).Trim();
                            string fineAmount = Convert.ToString(drCondo["total_fee"]).Trim();
                            string status = Convert.ToString(drCondo["status"]).Trim();

                            FpCondonation.Sheets[0].RowCount++;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno).Trim();
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 0].Locked = true;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(degreeName).Trim();
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 1].Locked = true;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(rollNo).Trim();
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 2].Locked = true;

                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(regNo).Trim();
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 3].Locked = true;

                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(studentName).Trim();
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 4].Locked = true;

                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 5].Text = Convert.ToString("").Trim();
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 5].Locked = true;

                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 6].Text = Convert.ToString("").Trim();
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 6].Locked = true;
                            sno++;

                        }
                    }
                    if (dtCondo.Rows.Count > 0)
                    {
                        FpCondonation.Sheets[0].RowCount++;
                        FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 0].Text = "Condonation Students";
                        FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 0].BackColor = System.Drawing.Color.Green;
                        FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                        FpCondonation.Sheets[0].AddSpanCell(FpCondonation.Sheets[0].RowCount - 1, 0, 1, 9);
                        int sno = 1;
                        foreach (DataRow drCondo in dtCondo.Rows)
                        {
                            string serialNo = Convert.ToString(drCondo["serialno"]).Trim();
                            string appNo = Convert.ToString(drCondo["App_No"]).Trim();
                            string isEligible = Convert.ToString(drCondo["is_eligible"]).Trim();
                            string degreeName = Convert.ToString(drCondo["Degree_Name"]).Trim();
                            string rollNo = Convert.ToString(drCondo["Roll_No"]).Trim();
                            string regNo = Convert.ToString(drCondo["Reg_No"]).Trim();
                            string studentName = Convert.ToString(drCondo["Stud_Name"]).Trim();
                            string studentType = Convert.ToString(drCondo["Stud_Type"]).Trim();
                            string fineAmount = Convert.ToString(drCondo["total_fee"]).Trim();
                            string status = Convert.ToString(drCondo["status"]).Trim();
                            //added by Deepali on 30.3.18
                            string challanDate = Convert.ToString(drCondo["challandate"]).Trim();
                            string challanNo = Convert.ToString(drCondo["challanno"]).Trim();
                            
                            //------
                            FpCondonation.Sheets[0].RowCount++;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno).Trim();
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 0].Locked = true;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(degreeName).Trim();
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 1].Locked = true;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(rollNo).Trim();
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 2].Locked = true;

                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(regNo).Trim();
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 3].Locked = true;

                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(studentName).Trim();
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 4].Locked = true;

                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(fineAmount).Trim();
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 5].Locked = true;

                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(status).Trim();
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 6].Locked = true;

                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(challanNo).Trim();
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 7].Locked = true;

                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(challanDate).Split(' ')[0].Trim();
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 8].Locked = true;
                            sno++;                                                              

                        }
                    }
                    if (dtNotEligible.Rows.Count > 0)
                    {
                        FpCondonation.Sheets[0].RowCount++;
                        FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 0].Text = "Not Eligible Students";
                        FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 0].BackColor = System.Drawing.Color.Green;
                        FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                        FpCondonation.Sheets[0].AddSpanCell(FpCondonation.Sheets[0].RowCount - 1, 0, 1, 9);
                        int sno = 1;
                        foreach (DataRow drCondo in dtNotEligible.Rows)
                        {
                            string serialNo = Convert.ToString(drCondo["serialno"]).Trim();
                            string appNo = Convert.ToString(drCondo["App_No"]).Trim();
                            string isEligible = Convert.ToString(drCondo["is_eligible"]).Trim();
                            string degreeName = Convert.ToString(drCondo["Degree_Name"]).Trim();
                            string rollNo = Convert.ToString(drCondo["Roll_No"]).Trim();
                            string regNo = Convert.ToString(drCondo["Reg_No"]).Trim();
                            string studentName = Convert.ToString(drCondo["Stud_Name"]).Trim();
                            string studentType = Convert.ToString(drCondo["Stud_Type"]).Trim();
                            string fineAmount = Convert.ToString(drCondo["total_fee"]).Trim();
                            string status = Convert.ToString(drCondo["status"]).Trim();

                            FpCondonation.Sheets[0].RowCount++;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno).Trim();
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 0].Locked = true;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(degreeName).Trim();
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 1].Locked = true;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(rollNo).Trim();
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 2].Locked = true;

                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(regNo).Trim();
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 3].Locked = true;

                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(studentName).Trim();
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 4].Locked = true;

                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 5].Text = Convert.ToString("").Trim();
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 5].Locked = true;

                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 6].Text = Convert.ToString("").Trim();
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                            FpCondonation.Sheets[0].Cells[FpCondonation.Sheets[0].RowCount - 1, 6].Locked = true;
                            sno++;

                        }
                    }

                    FpCondonation.SaveChanges();
                    FpCondonation.Sheets[0].PageSize = FpCondonation.Sheets[0].RowCount;
                    FpCondonation.Height = 500;
                    FpCondonation.Visible = true;
                    divCondonation.Visible = true;
                    btnCondonationReport.Visible = false;
                    rptprint1.Visible = true;

                }
                else
                {
                    lblErrmsg.Text = string.Empty;
                    lblErrmsg.Visible = false;
                    btnCondonationReport.Visible = false;
                    rptprint1.Visible = false;
                    divCondonation.Visible = false;
                    lblPopupAlert.Text = "No Record(s) were Found";
                    lblPopupAlert.Visible = true;
                    divPopupAlert.Visible = true;
                    return;
                }
            }

        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Go Click

    #region Popup Close

    protected void btnPopupClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrmsg.Text = string.Empty;
            lblErrmsg.Visible = false;
            lblPopupAlert.Text = string.Empty;
            divPopupAlert.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Popup Close

    #region Condonation Report

    protected void btnCondonationReport_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrmsg.Text = string.Empty;
            lblErrmsg.Visible = false;
            lblPopupAlert.Text = string.Empty;
            divPopupAlert.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrmsg.Text = Convert.ToString(ex);
            lblErrmsg.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Condonation Report

    #region Common Checkbox and Checkboxlist Event added by Deepali on 30.3.18

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