using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Threading;
using System.Data;
using System.Collections;
using System.Configuration;

public partial class CollegeWiseDegreeWiseSendCircularMaster : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    Hashtable hat = new Hashtable();
    bool isSchool = false;
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string courseId = string.Empty;
    string collegeCode = string.Empty;
    string collegeName = string.Empty;
    string batchYear = string.Empty;
    string degreeCode = string.Empty;
    string semester = string.Empty;
    string section = string.Empty;
    string SenderID = string.Empty;
    string Password = string.Empty;
    string studentName = string.Empty;
    string appNo = string.Empty;
    string rollNo = string.Empty;
    string regNo = string.Empty;
    string admissionNo = string.Empty;
    string qrySearch = string.Empty;
    string text = "Text";
    string storedProcedure = "sp";
    int count = 0;
    Institution ins;
    byte schoolOrCollege = 0;
    string PageLogOut = "";
    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ////****************************************************//
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
            ////****************************************************//
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
            usercode = Convert.ToString(Session["usercode"]).Trim();
            singleuser = Convert.ToString(Session["single_user"]).Trim();
            group_user = Convert.ToString(Session["group_code"]).Trim();
            collegeCode = Convert.ToString(Session["collegecode"]).Trim();

            string grouporusercode1 = string.Empty;
            if ((Session["group_code"] != null && Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode1 = " and group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
            }
            else if (Session["usercode"] != null)
            {
                grouporusercode1 = " and usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
            }

            if (!string.IsNullOrEmpty(grouporusercode1.Trim()))
                ins = new Institution(grouporusercode1.Replace("and", ""));
            if (ins != null)
                schoolOrCollege = ins.TypeInstitute;

            DataSet schoolds = new DataSet();
            string sqlschool = "select * from Master_Settings where settings='schoolorcollege'  " + grouporusercode1 + "";
            schoolds.Clear();
            schoolds.Dispose();
            schoolds = d2.select_method_wo_parameter(sqlschool, "Text");
            if (schoolds.Tables.Count > 0 && schoolds.Tables[0].Rows.Count > 0)
            {
                string schoolvalue = Convert.ToString(schoolds.Tables[0].Rows[0]["value"]).Trim();
                if (schoolvalue.Trim() == "0")
                {
                    isSchool = true;
                }
            }
            if (!IsPostBack)
            {
                lblErrSearch.Text = string.Empty;
                lblErrSearch.Visible = false;
                lblAlertMsg.Text = string.Empty;
                divPopAlert.Visible = false;
                divMainGrid.Visible = false;
                divDegreeDetails.Visible = false;
                divCollegeWise.Visible = false;
                divPrint.Visible = false;

                BindCollege();
                BindBatch();
                collegecode = Convert.ToString(ddlCollege.SelectedValue).Trim();
                BindDegree(singleuser, group_user, collegecode, usercode);
                BindBranchMultiple(singleuser, group_user, courseId, collegecode, usercode);
                BindSectionDetail();

                ViewState["Rollflag"] = "0";
                ViewState["Regflag"] = "0";
                ViewState["Studflag"] = "0";
                ViewState["AdmissionNo"] = "0";

                setLabelText();

                string grouporusercode = string.Empty;

                if ((Session["group_code"] != null && Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                {
                    grouporusercode = " where group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
                }
                else if (Session["usercode"] != null)
                {
                    grouporusercode = " where usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
                }
                string user_code = Convert.ToString(Session["usercode"]).Trim();

                hat.Clear();
                string Master = "select * from Master_Settings  " + grouporusercode + "";
                DataSet ds = d2.select_method_wo_parameter(Master, text);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (Convert.ToString(ds.Tables[0].Rows[i]["settings"]).Trim().ToLower() == "roll no" && Convert.ToString(ds.Tables[0].Rows[i]["value"]).Trim() == "1")
                        {
                            ViewState["Rollflag"] = "1";

                        }
                        if (Convert.ToString(ds.Tables[0].Rows[i]["settings"]).Trim().ToLower() == "register no" && Convert.ToString(ds.Tables[0].Rows[i]["value"]).Trim() == "1")
                        {
                            ViewState["Regflag"] = "1";
                        }
                        if (Convert.ToString(ds.Tables[0].Rows[i]["settings"]).Trim().ToLower() == "student_type" && Convert.ToString(ds.Tables[0].Rows[i]["value"]).Trim() == "1")
                        {
                            ViewState["Studflag"] = "1";
                        }
                        if (Convert.ToString(ds.Tables[0].Rows[i]["settings"]).Trim().ToLower() == "admission no" && Convert.ToString(ds.Tables[0].Rows[i]["value"]).Trim() == "1")
                        {
                            ViewState["AdmissionNo"] = "1";
                        }
                    }
                }
            }
        }
        catch (ThreadAbortException et)
        {

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Page Load

    #region Bind Header

    public void BindCollege()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            divMainGrid.Visible = false;
            divDegreeDetails.Visible = false;
            divCollegeWise.Visible = false;
            divPrint.Visible = false;

            string columnfield = string.Empty;
            if (Session["group_code"] != null)
            {
                group_user = Convert.ToString(Session["group_code"]).Trim();
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = Convert.ToString(group_semi[0]).Trim();
                }
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["single_user"] != null && (Convert.ToString(Session["single_user"]).Trim() != "1" && Convert.ToString(Session["single_user"]).Trim().ToLower() != "true" && Convert.ToString(Session["single_user"]).Trim().ToUpper() != "TRUE" && Convert.ToString(Session["single_user"]).Trim() != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            DataSet dsprint = d2.select_method("bind_college", hat, "sp");
            ddlCollege.Items.Clear();
            if (dsprint.Tables.Count > 0 && dsprint.Tables[0].Rows.Count > 0)
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
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public void BindBatchOld()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            divMainGrid.Visible = false;
            divDegreeDetails.Visible = false;
            divCollegeWise.Visible = false;
            divPrint.Visible = false;

            ds.Dispose();
            ds.Reset();
            cblBatch.Items.Clear();
            chkBatch.Checked = false;
            txtBatch.Text = "---Select---";
            ds = d2.BindBatch();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblBatch.DataSource = ds;
                cblBatch.DataTextField = "Batch_year";
                cblBatch.DataValueField = "Batch_year";
                cblBatch.DataBind();
                cblBatch.SelectedIndex = cblBatch.Items.Count - 1;
                for (int i = 0; i < cblBatch.Items.Count; i++)
                {
                    cblBatch.Items[i].Selected = true;
                    if (cblBatch.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (cblBatch.Items.Count == count)
                    {
                        chkBatch.Checked = true;
                    }
                }
                if (chkBatch.Checked == true)
                {
                    for (int i = 0; i < cblBatch.Items.Count; i++)
                    {
                        cblBatch.Items[i].Selected = true;
                        txtBatch.Text = "Batch(" + (cblBatch.Items.Count) + ")";
                        if (isSchool == true)
                        {
                            txtBatch.Text = "Year(" + (cblBatch.Items.Count) + ")";
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < cblBatch.Items.Count; i++)
                    {
                        cblBatch.Items[i].Selected = false;
                        txtBatch.Text = "---Select---";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public void BindBatch()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            divMainGrid.Visible = false;
            divDegreeDetails.Visible = false;
            divCollegeWise.Visible = false;
            divPrint.Visible = false;

            cblBatch.Items.Clear();
            chkBatch.Checked = false;
            txtBatch.Text = "---Select---";
            string Master1 = string.Empty;
            if ((Session["group_code"] != null && Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                string group = Convert.ToString(Session["group_code"]).Trim();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    Master1 = Convert.ToString(group_semi[0]);
                }
            }
            else if (Session["usercode"] != null)
            {
                Master1 = Convert.ToString(Session["usercode"]).Trim();
            }
            string collegecode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            string strbinddegree = "select distinct batch_year from tbl_attendance_rights where user_id='" + Master1 + "' and college_code='" + collegecode + "'";
            DataSet ds = d2.select_method_wo_parameter(strbinddegree, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblBatch.DataSource = ds;
                cblBatch.DataTextField = "batch_year";
                cblBatch.DataValueField = "batch_year";
                cblBatch.DataBind();
                cblBatch.SelectedIndex = cblBatch.Items.Count - 1;
                for (int i = 0; i < cblBatch.Items.Count; i++)
                {
                    cblBatch.Items[i].Selected = true;
                    if (cblBatch.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                }
                if (cblBatch.Items.Count == count)
                {
                    chkBatch.Checked = true;
                    txtBatch.Text = "Batch(" + (cblBatch.Items.Count) + ")";
                }
                if (chkBatch.Checked == true)
                {
                    for (int i = 0; i < cblBatch.Items.Count; i++)
                    {
                        cblBatch.Items[i].Selected = true;
                        txtBatch.Text = "Batch(" + (cblBatch.Items.Count) + ")";
                        if (isSchool == true)
                        {
                            txtBatch.Text = "Year(" + (cblBatch.Items.Count) + ")";
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < cblBatch.Items.Count; i++)
                    {
                        cblBatch.Items[i].Selected = false;
                        txtBatch.Text = "---Select---";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {
            //gvDegreeDetails.Visible = false;
            //divDegreeDetails.Visible = false;
            //btnSendSMS.Visible = false;
            //btnprintmaster.Visible = false;
            //chkselectall.Visible = false;
            //chkSms.Visible = false;
            //chkvoice.Visible = false;

            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            divMainGrid.Visible = false;
            divDegreeDetails.Visible = false;
            divCollegeWise.Visible = false;
            divPrint.Visible = false;

            count = 0;
            cblDegree.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
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
                        chkDegree.Checked = true;
                    }
                }
                if (chkDegree.Checked == true)
                {
                    for (int i = 0; i < cblDegree.Items.Count; i++)
                    {
                        cblDegree.Items[i].Selected = true;
                        txtDegree.Text = "Degree(" + (cblDegree.Items.Count) + ")";
                        if (isSchool == true)
                        {
                            txtDegree.Text = "School Type(" + (cblDegree.Items.Count) + ")";
                        }
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
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }

    }

    public void BindBranchMultiple(string singleuser, string group_user, string courseId, string collegecode, string usercode)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            divMainGrid.Visible = false;
            divDegreeDetails.Visible = false;
            divCollegeWise.Visible = false;
            divPrint.Visible = false;

            count = 0;
            collegecode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            for (int i = 0; i < cblDegree.Items.Count; i++)
            {
                if (cblDegree.Items[i].Selected == true)
                {
                    if (courseId == "")
                    {
                        courseId = "" + cblDegree.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        courseId = courseId + "," + "" + cblDegree.Items[i].Value.ToString() + "";
                    }
                }
            }
            cblBranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.BindBranchMultiple(singleuser, group_user, courseId, collegecode, usercode);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
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
                        chkBranch.Checked = true;
                    }
                }
                if (chkBranch.Checked == true)
                {
                    for (int i = 0; i < cblBranch.Items.Count; i++)
                    {
                        cblBranch.Items[i].Selected = true;
                        txtBranch.Text = "Branch(" + (cblBranch.Items.Count) + ")";
                        if (isSchool == true)
                        {
                            txtBranch.Text = "Standard(" + (cblBranch.Items.Count) + ")";
                        }
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
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public void BindSectionDetail()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            divMainGrid.Visible = false;
            divDegreeDetails.Visible = false;
            divCollegeWise.Visible = false;
            divPrint.Visible = false;
            string batchyear = string.Empty;
            string branch = string.Empty;
            string batch = string.Empty;
            DataSet ds = new DataSet();
            txtSec.Enabled = false;
            cblSec.Items.Clear();
            chkSec.Checked = false;
            txtSec.Text = "--Select--";
            if (cblBatch.Items.Count > 0)
            {
                foreach (ListItem li in cblBatch.Items)
                {
                    string value = Convert.ToString(li.Value).Trim();
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(batch))
                        {
                            batch = "'" + value + "'";
                        }
                        else
                        {
                            batch += ",'" + value + "'";
                        }
                    }
                }
                //batch = Convert.ToString(ddlBatch.SelectedValue).Trim();
            }
            if (cblBranch.Items.Count > 0)
            {
                //branch = Convert.ToString(ddlBranch.SelectedValue).Trim();
                foreach (ListItem li in cblBranch.Items)
                {
                    string value = Convert.ToString(li.Value).Trim();
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(branch))
                        {
                            branch = "'" + value + "'";
                        }
                        else
                        {
                            branch += ",'" + value + "'";
                        }
                    }
                }
            }

            string Master1 = string.Empty;
            if ((Session["group_code"] != null && Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                string group = Convert.ToString(Session["group_code"]).Trim();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    Master1 = Convert.ToString(group_semi[0]);
                }
            }
            else if (Session["usercode"] != null)
            {
                Master1 = Convert.ToString(Session["usercode"]).Trim();
            }
            string collegecode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            string qrysections = string.Empty;
            ArrayList arrSect = new ArrayList();
            string sectionsList = string.Empty;
            bool hasEmptySections = false;
            if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(batch) && !string.IsNullOrEmpty(Master1))
            {
                qrysections = "select distinct sections from tbl_attendance_rights where user_id='" + Master1 + "' and college_code='" + collegecode + "' and batch_year in(" + batch + ")";
                DataSet dsSections = new DataSet();
                dsSections = d2.select_method_wo_parameter(qrysections, text);
                if (dsSections.Tables.Count > 0 && dsSections.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drSec in dsSections.Tables[0].Rows)
                    {
                        string section = Convert.ToString(drSec["sections"]).Trim();
                        if (string.IsNullOrEmpty(section))
                        {
                            hasEmptySections = true;
                        }
                        if (!arrSect.Contains(section.Trim()))
                        {
                            if (string.IsNullOrEmpty(sectionsList))
                            {
                                sectionsList = "'" + section.Trim() + "'";
                            }
                            else
                            {
                                sectionsList += ",'" + section.Trim() + "'";
                            }
                            arrSect.Add(section.Trim());
                        }
                    }
                }
                qrysections = d2.GetFunctionv("select distinct STUFF((SELECT ',' + s.sections FROM tbl_attendance_rights s where user_id='" + Master1 + "' and college_code='" + collegecode + "' and batch_year in(" + batch + ") ORDER BY s.sections  FOR XML PATH('')  ),1, 1, '') as sections ");
            }
            if (!string.IsNullOrEmpty(qrysections.Trim()))
            {
                string[] sectionsAll = qrysections.Trim().Split(new char[] { ',' });
                string sections = string.Empty;
                ArrayList arrSections = new ArrayList();
                if (sectionsAll.Length > 0)
                {
                    for (int sec = 0; sec < sectionsAll.Length; sec++)
                    {
                        if (!arrSections.Contains(sectionsAll[sec].Trim().Replace("'", "")))
                        {
                            if (!string.IsNullOrEmpty(sectionsAll[sec].Trim().Replace("'", "")))
                            {
                                if (sections.Trim() == "")
                                {
                                    sections = "'" + sectionsAll[sec].Trim().Replace("'", "") + "'";
                                }
                                else
                                {
                                    sections += ",'" + sectionsAll[sec].Trim().Replace("'", "") + "'";
                                }
                            }
                            else
                            {
                                hasEmptySections = true;
                                if (sections.Trim() == "")
                                {
                                    sections = "'" + sectionsAll[sec].Trim().Replace("'", "") + "'";
                                }
                                else
                                {
                                    sections += ",'" + sectionsAll[sec].Trim().Replace("'", "") + "'";
                                }
                            }
                            arrSections.Add(sectionsAll[sec].Trim().Replace("'", ""));
                        }
                    }
                }
                if (!string.IsNullOrEmpty(sections.Trim()))
                {
                    string sqlnew = "select distinct isnull(ltrim(rtrim(sections)),'') sections,isnull(ltrim(rtrim(sections)),'') as SecVal from registration where batch_year in(" + batch + ") and degree_code in(" + branch + ") and isnull(ltrim(rtrim(sections)),'')<>'-1' and sections<>' ' and isnull(ltrim(rtrim(sections)),'') in(" + sections + ") and delflag=0 and exam_flag<>'Debar' " + ((hasEmptySections) ? " union select '' sections,'Empty' as SecVal " : "") + " order by sections";

                    ds.Clear();
                    ds = d2.select_method_wo_parameter(sqlnew, "Text");
                }
            }

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblSec.DataSource = ds;
                cblSec.DataTextField = "SecVal";
                cblSec.DataValueField = "sections";
                cblSec.DataBind();

                for (int h = 0; h < cblSec.Items.Count; h++)
                {
                    cblSec.Items[h].Selected = true;
                }
                txtSec.Text = "Section" + "(" + cblSec.Items.Count + ")";
                chkSec.Checked = true;
                txtSec.Enabled = true;
                //ddlSec.Items.Insert(0, new ListItem("--Select--", "-1"));
            }
            else
            {
                txtSec.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public void ChangeHeaderName(bool isschool)
    {
        try
        {
            lblErrSearch.Visible = false;
            lblErrSearch.Text = string.Empty;
            lblCollege.Text = ((!isschool) ? "College" : "School");
            lblBatch.Text = ((!isschool) ? "Batch" : "Year");
            lblDegree.Text = ((!isschool) ? "Degree" : "School Type");
            lblBranch.Text = ((!isschool) ? "Branch" : "Standard");
            //lblSem.Text = ((!isschool) ? "Semester" : "Term");
            //lblSec.Text = ((!isschool) ? "Section" : "Section");
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
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
        lbl.Add(lblCollege);
        lbl.Add(lblDegree);
        lbl.Add(lblBranch);
        fields.Add(0);
        fields.Add(2);
        fields.Add(3);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }

    #endregion

    #region DropDownList Events

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            divMainGrid.Visible = false;
            divDegreeDetails.Visible = false;
            divCollegeWise.Visible = false;
            divPrint.Visible = false;
            //gvDegreeDetails.Visible = false;
            //divDegreeDetails.Visible = false;
            //btnSendSMS.Visible = false;
            //btnprintmaster.Visible = false;
            //chkselectall.Visible = false;
            //chkSms.Visible = false;
            //chkvoice.Visible = false;
            lblErrSearch.Visible = false;
            collegecode = ddlCollege.SelectedValue.ToString();
            BindDegree(singleuser, group_user, collegecode, usercode);
            BindBranchMultiple(singleuser, group_user, courseId, collegecode, usercode);
            //Bindhour();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void chkBatch_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            divMainGrid.Visible = false;
            divDegreeDetails.Visible = false;
            divCollegeWise.Visible = false;
            divPrint.Visible = false;
            //gvDegreeDetails.Visible = false;
            //divDegreeDetails.Visible = false;
            //btnSendSMS.Visible = false;
            //btnprintmaster.Visible = false;
            //chkselectall.Visible = false;
            //chkSms.Visible = false;
            //chkvoice.Visible = false;
            lblErrSearch.Visible = false;
            if (chkBatch.Checked == true)
            {
                for (int i = 0; i < cblBatch.Items.Count; i++)
                {
                    cblBatch.Items[i].Selected = true;
                }
                txtBatch.Text = "Batch(" + (cblBatch.Items.Count) + ")";
                if (isSchool == true)
                {
                    txtBatch.Text = "Year(" + (cblBatch.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < cblBatch.Items.Count; i++)
                {
                    cblBatch.Items[i].Selected = false;
                }
                txtBatch.Text = "---Select---";
            }
            //Bindhour();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void cblBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            divMainGrid.Visible = false;
            divDegreeDetails.Visible = false;
            divCollegeWise.Visible = false;
            divPrint.Visible = false;

            //gvDegreeDetails.Visible = false;
            //divDegreeDetails.Visible = false;
            //btnSendSMS.Visible = false;
            //btnprintmaster.Visible = false;
            //chkselectall.Visible = false;
            //chkSms.Visible = false;
            //chkvoice.Visible = false;
            lblErrSearch.Visible = false;
            int commcount = 0;
            txtBatch.Text = "--Select--";
            chkBatch.Checked = false;
            for (int i = 0; i < cblBatch.Items.Count; i++)
            {
                if (cblBatch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtBatch.Text = "Batch(" + commcount.ToString() + ")";
                if (isSchool == true)
                {
                    txtBatch.Text = "Year(" + commcount.ToString() + ")";
                }
                if (commcount == cblBatch.Items.Count)
                {
                    chkBatch.Checked = true;
                }
            }
            //Bindhour();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void chkDegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            divMainGrid.Visible = false;
            divDegreeDetails.Visible = false;
            divCollegeWise.Visible = false;
            divPrint.Visible = false;
            //gvDegreeDetails.Visible = false;
            //divDegreeDetails.Visible = false;
            //btnSendSMS.Visible = false;
            //btnprintmaster.Visible = false;
            //chkselectall.Visible = false;
            //chkSms.Visible = false;
            //chkvoice.Visible = false;
            lblErrSearch.Visible = false;
            collegecode = ddlCollege.SelectedValue.ToString();
            if (chkDegree.Checked == true)
            {
                for (int i = 0; i < cblDegree.Items.Count; i++)
                {
                    cblDegree.Items[i].Selected = true;
                }
                txtDegree.Text = "Degree(" + (cblDegree.Items.Count) + ")";
                if (isSchool == true)
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
            BindBranchMultiple(singleuser, group_user, courseId, collegecode, usercode);
            //Bindhour();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void cblDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            divMainGrid.Visible = false;
            divDegreeDetails.Visible = false;
            divCollegeWise.Visible = false;
            divPrint.Visible = false;
            //gvDegreeDetails.Visible = false;
            //divDegreeDetails.Visible = false;
            //btnSendSMS.Visible = false;
            //btnprintmaster.Visible = false;
            //chkselectall.Visible = false;
            //chkSms.Visible = false;
            //chkvoice.Visible = false;
            lblErrSearch.Visible = false;
            collegecode = ddlCollege.SelectedValue.ToString();
            int commcount = 0;
            chkDegree.Checked = false;
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
                txtDegree.Text = "Degree(" + commcount.ToString() + ")";
                if (isSchool == true)
                {
                    txtDegree.Text = "School Type(" + commcount.ToString() + ")";
                }
                if (commcount == cblDegree.Items.Count)
                {
                    chkDegree.Checked = true;
                }
            }
            BindBranchMultiple(singleuser, group_user, courseId, collegecode, usercode);
            //Bindhour();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void chkBranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            divMainGrid.Visible = false;
            divDegreeDetails.Visible = false;
            divCollegeWise.Visible = false;
            divPrint.Visible = false;
            //gvDegreeDetails.Visible = false;
            //divDegreeDetails.Visible = false;
            //btnSendSMS.Visible = false;
            //btnprintmaster.Visible = false;
            //chkselectall.Visible = false;
            //chkSms.Visible = false;
            //chkvoice.Visible = false;
            lblErrSearch.Visible = false;
            if (chkBranch.Checked == true)
            {
                for (int i = 0; i < cblBranch.Items.Count; i++)
                {
                    cblBranch.Items[i].Selected = true;
                }
                txtBranch.Text = "Branch(" + (cblBranch.Items.Count) + ")";

                if (isSchool == true)
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
            //Bindhour();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void cblBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            divMainGrid.Visible = false;
            divDegreeDetails.Visible = false;
            divCollegeWise.Visible = false;
            divPrint.Visible = false;
            //gvDegreeDetails.Visible = false;
            //divDegreeDetails.Visible = false;
            //btnSendSMS.Visible = false;
            //btnprintmaster.Visible = false;
            //chkselectall.Visible = false;
            //chkSms.Visible = false;
            //chkvoice.Visible = false;
            lblErrSearch.Visible = false;
            string clg = string.Empty;
            int commcount = 0;
            txtBranch.Text = "--Select--";
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
                txtBranch.Text = "Branch(" + commcount.ToString() + ")";
                if (isSchool == true)
                {
                    txtBranch.Text = "Standard(" + commcount.ToString() + ")";
                }
                if (commcount == cblBranch.Items.Count)
                {
                    chkBranch.Checked = true;
                }
            }

            //Bindhour();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void chkSec_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            divMainGrid.Visible = false;
            divDegreeDetails.Visible = false;
            divCollegeWise.Visible = false;
            divPrint.Visible = false;
            int count = 0;
            if (chkSec.Checked == true)
            {
                count++;
                for (int i = 0; i < cblSec.Items.Count; i++)
                {
                    cblSec.Items[i].Selected = true;
                }
                txtSec.Text = "Section(" + (cblSec.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cblSec.Items.Count; i++)
                {
                    cblSec.Items[i].Selected = false;
                }
                txtSec.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void cblSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            divMainGrid.Visible = false;
            divDegreeDetails.Visible = false;
            divCollegeWise.Visible = false;
            divPrint.Visible = false;
            int commcount = 0;
            chkSec.Checked = false;
            for (int i = 0; i < cblSec.Items.Count; i++)
            {
                if (cblSec.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cblSec.Items.Count)
                {
                    chkSec.Checked = true;
                }
                txtSec.Text = "Section(" + Convert.ToString(commcount) + ")";
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void rblCollegeOrDegreeWise_SelectedIndexChanged_(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            divMainGrid.Visible = false;
            divDegreeDetails.Visible = false;
            divCollegeWise.Visible = false;
            divPrint.Visible = false;

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void chkselectall_Change(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;

            if (chkselectall.Checked == true)
            {
                if (rblCollegeOrDegreeWise.SelectedIndex == 1)
                {
                    foreach (GridViewRow gvrow in gvDegreeDetails.Rows)
                    {
                        CheckBox presentall = (CheckBox)gvrow.FindControl("chkDWiseSelect");
                        presentall.Checked = true;
                    }
                }
                else
                {
                    foreach (GridViewRow gvrow in gvCollegeWise.Rows)
                    {
                        CheckBox presentall = (CheckBox)gvrow.FindControl("chkCWiseSelect");
                        presentall.Checked = true;
                    }
                }
            }
            else
            {
                if (rblCollegeOrDegreeWise.SelectedIndex == 1)
                {
                    foreach (GridViewRow gvrow in gvDegreeDetails.Rows)
                    {
                        CheckBox presentall = (CheckBox)gvrow.FindControl("chkDWiseSelect");
                        presentall.Checked = false;
                    }
                }
                else
                {
                    foreach (GridViewRow gvrow in gvCollegeWise.Rows)
                    {
                        CheckBox presentall = (CheckBox)gvrow.FindControl("chkCWiseSelect");
                        presentall.Checked = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion

    #region Popup Close

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion  Popup Close

    #region Generate Excel

    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            Printcontrol1.Visible = false;
            string reportname = txtexcelname1.Text.Trim().Replace(" ", "_").Trim();
            if (Convert.ToString(reportname).Trim() != "")
            {
                //if (FpSpreadInternalMarks.Visible == true)
                //{
                //    d2.printexcelreport(FpSpreadInternalMarks, reportname);
                //}
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
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
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
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            string rptheadname = "Send Circular Message";
            string pagename = System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString();
            //if (FpSpreadInternalMarks.Visible == true)
            //{
            //    Printcontrol1.loadspreaddetails(FpSpreadInternalMarks, pagename, rptheadname);
            //}
            Printcontrol1.Visible = true;
            lbl_norec1.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Print PDF

    #region Go Click

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            divMainGrid.Visible = false;
            divDegreeDetails.Visible = false;
            divCollegeWise.Visible = false;
            divPrint.Visible = false;
            if (ddlCollege.Items.Count == 0)
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "No " + ((ins != null) ? ins.InsName : "College") + " Were Found";
                return;
            }

            if (rblCollegeOrDegreeWise.SelectedIndex == 1)
            {
                if (isSchool == true)
                {
                    this.gvDegreeDetails.Columns[1].HeaderText = "Year";
                    this.gvDegreeDetails.Columns[2].HeaderText = "School Type";
                    this.gvDegreeDetails.Columns[3].HeaderText = "Standard";
                    this.gvDegreeDetails.Columns[4].HeaderText = "Term";
                }

                chkselectall.Checked = false;
                chkSms.Checked = false;
                //chkvoice.Checked = false;
                string sqlbatch = string.Empty;
                string sqlbatchquery = string.Empty;
                string sqlbatchquery1 = string.Empty;
                string sqlbranch = string.Empty;
                string sqlbranchquery = string.Empty;
                string sqlbranchquery1 = string.Empty;
                int itemcount = 0;

                if (cblBatch.Items.Count == 0)
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "No " + ((isSchool) ? "Year" : "Batch") + " Were Found";
                    return;
                }

                int selBatch = 0;
                int selDegree = 0;
                for (itemcount = 0; itemcount < cblBatch.Items.Count; itemcount++)
                {
                    if (cblBatch.Items[itemcount].Selected == true)
                    {
                        selBatch++;
                        if (sqlbatch == "")
                            sqlbatch = "'" + cblBatch.Items[itemcount].Value.ToString() + "'";
                        else
                            sqlbatch = sqlbatch + "," + "'" + cblBatch.Items[itemcount].Value.ToString() + "'";
                    }
                }
                if (selBatch == 0)
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "Please Select Atleast One " + ((isSchool) ? "Year" : "Batch");
                    return;
                }
                if (sqlbatch.Trim() != "")
                {
                    sqlbatch = " in(" + sqlbatch + ")";
                    sqlbatchquery = " and r.batch_year  " + sqlbatch + "";
                    sqlbatchquery1 = " batch_year  " + sqlbatch + "";
                }
                else
                {
                    sqlbatchquery = " ";
                    gvDegreeDetails.Visible = false;
                    divMainGrid.Visible = false;
                    divDegreeDetails.Visible = false;
                    btnSendSMS.Visible = false;
                    btnprintmaster.Visible = false;
                    chkselectall.Visible = false;
                    chkSms.Visible = false;
                    //chkvoice.Visible = false;
                    //lblErrSearch.Visible = true;
                    //lblErrSearch.Text = " Please Select The Batch And Then Proceed";
                    //if (isSchool == true)
                    //{
                    //    lblErrSearch.Text = " Please Select The Year And Then Proceed";
                    //}
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "Please Select The " + ((isSchool) ? "Year" : "Batch") + " And Then Proceed";
                    return;
                }
                int seleDeg = 0;
                if (cblDegree.Items.Count == 0)
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "No " + ((ins != null) ? ins.InsDegree : "Degree") + " Were Found";
                    return;
                }
                foreach (ListItem liDeg in cblDegree.Items)
                {
                    if (liDeg.Selected)
                    {
                        seleDeg++;
                    }
                }
                if (seleDeg == 0)
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "Please Select Atleast One " + ((ins != null) ? ins.InsDegree : "Degree") + " And Then Proceed";
                    return;
                }
                if (cblBranch.Items.Count == 0)
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "No " + ((ins != null) ? ins.InsBranch : "Branch") + " Were Found";
                    return;
                }
                if (txtDegree.Text == "---Select---")
                {
                    gvDegreeDetails.Visible = false;
                    divMainGrid.Visible = false;
                    divDegreeDetails.Visible = false;
                    btnSendSMS.Visible = false;
                    btnprintmaster.Visible = false;
                    chkselectall.Visible = false;
                    chkSms.Visible = false;
                    //chkvoice.Visible = false;
                    //lblErrSearch.Visible = true;
                    //lblErrSearch.Text = "Please Select The Degree  And Then Proceed";
                    //if (isSchool == true)
                    //{
                    //    lblErrSearch.Text = "Please Select The School Type  And Then Proceed";
                    //}
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "Please Select The " + ((ins != null) ? ins.InsDegree : "Degree") + " And Then Proceed";
                    return;
                }
                itemcount = 0;
                for (itemcount = 0; itemcount < cblBranch.Items.Count; itemcount++)
                {
                    if (cblBranch.Items[itemcount].Selected == true)
                    {
                        selDegree++;
                        if (sqlbranch == "")
                            sqlbranch = "'" + cblBranch.Items[itemcount].Value.ToString() + "'";
                        else
                            sqlbranch = sqlbranch + "," + "'" + cblBranch.Items[itemcount].Value.ToString() + "'";
                    }
                }
                if (selDegree == 0)
                {
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "Please Select Atleast One " + ((ins != null) ? ins.InsBranch : "Branch") + " And Then Proceed"; ;
                    return;
                }
                if (sqlbranch.Trim() != "")
                {
                    sqlbranch = " in(" + sqlbranch + ")";
                    sqlbranchquery = " and r.degree_code  " + sqlbranch + "";
                    sqlbranchquery1 = " and degree_code  " + sqlbranch + "";
                }
                else
                {
                    sqlbranchquery = " ";
                    gvDegreeDetails.Visible = false;
                    divMainGrid.Visible = false;
                    divDegreeDetails.Visible = false;
                    btnSendSMS.Visible = false;
                    btnprintmaster.Visible = false;
                    chkselectall.Visible = false;
                    chkSms.Visible = false;
                    //chkvoice.Visible = false;
                    //lblErrSearch.Visible = true;
                    //lblErrSearch.Text = "Please Select The Branch  And Then Proceed";
                    //if (isSchool == true)
                    //{
                    //    lblErrSearch.Text = "Please Select The Standard  And Then Proceed";
                    //}
                    //return;
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "Please Select The " + ((ins != null) ? ins.InsBranch : "Branch") + " And Then Proceed";
                    return;
                }

                string secrights = string.Empty;
                Boolean secrightsflag = false;
                string collegecode = ddlCollege.SelectedValue.ToString();
                string ucode = string.Empty;
                string group_code = Session["group_code"].ToString();
                if (group_code.Contains(';'))
                {
                    string[] group_semi = group_code.Split(';');
                    group_code = group_semi[0].ToString();
                }
                if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
                {
                    ucode = group_code;
                }
                else
                {
                    ucode = Session["usercode"].ToString();
                }

                string strgetsec = d2.GetFunction("select sections from tbl_attendance_rights where " + sqlbatchquery1 + " and user_id='" + ucode + "' and college_code='" + collegecode + "'");
                if (strgetsec.Trim() != null && strgetsec.Trim() != "0")
                {
                    string[] spsec = strgetsec.Split(',');
                    for (int sp = 0; sp <= spsec.GetUpperBound(0); sp++)
                    {
                        string valu = spsec[sp].ToString();
                        if (secrights.Trim().ToLower() == valu.Trim().ToLower())
                        {
                            secrightsflag = true;
                        }
                    }
                }
                if (secrightsflag == false)
                {
                    gvDegreeDetails.Visible = false;
                    divMainGrid.Visible = false;
                    divDegreeDetails.Visible = false;
                    btnSendSMS.Visible = false;
                    btnprintmaster.Visible = false;
                    chkselectall.Visible = false;
                    chkSms.Visible = false;
                    //chkvoice.Visible = false;
                    //lblErrSearch.Visible = true;
                    //lblErrSearch.Text = "Please Set The Batch Year and Sections Rights For The User";
                    //if (isSchool == true)
                    //{
                    //    lblErrSearch.Text = "Please Set The Year and Sections Rights For The User";
                    //}
                    //return;
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "Please Set The " + ((isSchool) ? "Year" : "Batch") + " and Sections Rights For The User";
                    return;
                }

                section = string.Empty;
                string qrySec = string.Empty;
                int selectedSec = 0;
                if (txtSec.Enabled)
                {
                    if (cblSec.Items.Count > 0)
                    {
                        foreach (ListItem liSec in cblSec.Items)
                        {
                            string value = Convert.ToString(liSec.Value).Trim();
                            if (liSec.Selected)
                            {
                                selectedSec++;
                                if (string.IsNullOrEmpty(section))
                                {
                                    section = "'" + value + "'";
                                }
                                else
                                {
                                    section += ",'" + value + "'";
                                }
                            }
                        }
                        if (selectedSec != 0)
                        {
                            qrySec = " and ltrim(rtrim(isnull(r.Sections,''))) in(" + section + ")";
                        }
                    }
                }

                string sqlquery = "select distinct count(distinct r.roll_no)as strength,(c.Course_Name+'-'+ dp.dept_acronym) as dept,c.Course_Name,dp.dept_acronym,r.current_semester,r.batch_year,r.degree_code,ltrim(rtrim(isnull(r.Sections,''))) as sections,dp.Dept_Name AS Dept_Name   from registration r,degree de,course c,department dp where r.degree_code=de.degree_code and c.course_id=de.course_id and dp.dept_code = de.dept_code and r.delflag=0 and r.cc=0 and r.exam_flag<>'debar'  " + sqlbatchquery + " " + sqlbranchquery + qrySec + " group by r.degree_code,r.batch_year,course_name,dept_acronym,current_semester,sections,dp.Dept_Name order by  r.batch_year desc,current_semester asc, r.degree_code,Sections asc";
                DataSet dsselect = new DataSet();
                dsselect = d2.select_method(sqlquery, hat, "Text");
                if (dsselect.Tables.Count > 0 && dsselect.Tables[0].Rows.Count > 0)
                {
                    gvDegreeDetails.DataSource = dsselect.Tables[0];
                    gvDegreeDetails.DataBind();
                    gvDegreeDetails.Visible = true;
                    divMainGrid.Visible = true;
                    divDegreeDetails.Visible = true;
                    btnSendSMS.Visible = true;
                    btnprintmaster.Visible = false;
                    chkselectall.Visible = true;
                    chkSms.Visible = true;
                    //chkvoice.Visible = true;
                }
                else
                {
                    gvDegreeDetails.Visible = false;
                    divMainGrid.Visible = false;
                    divDegreeDetails.Visible = false;
                    btnSendSMS.Visible = false;
                    btnprintmaster.Visible = false;
                    chkselectall.Visible = false;
                    chkSms.Visible = false;
                    //chkvoice.Visible = false;
                    //lblErrSearch.Visible = true;
                    //lblErrSearch.Text = "No Records Found";
                    //lblErrSearch.Visible = true;
                    //return;
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "No Records Found";
                    return;
                }
            }
            else
            {
                if (isSchool == true)
                {
                    this.gvCollegeWise.Columns[1].HeaderText = "School Name";
                }

                string sqlquery = "select collname,college_code from collinfo where college_code in('" + collegeCode + "') order by college_code";
                DataSet dsselect = new DataSet();
                dsselect = d2.select_method(sqlquery, hat, "Text");
                if (dsselect.Tables.Count > 0 && dsselect.Tables[0].Rows.Count > 0)
                {
                    gvCollegeWise.DataSource = dsselect.Tables[0];
                    gvCollegeWise.DataBind();
                    gvCollegeWise.Visible = true;
                    divMainGrid.Visible = true;
                    divDegreeDetails.Visible = false;
                    divCollegeWise.Visible = true;
                    btnSendSMS.Visible = true;
                    btnprintmaster.Visible = false;
                    chkselectall.Visible = true;
                    chkSms.Visible = true;
                    //chkvoice.Visible = true;
                }
                else
                {
                    gvDegreeDetails.Visible = false;
                    divMainGrid.Visible = false;
                    divDegreeDetails.Visible = false;
                    divCollegeWise.Visible = false;
                    btnSendSMS.Visible = false;
                    btnprintmaster.Visible = false;
                    chkselectall.Visible = false;
                    chkSms.Visible = false;
                    //chkvoice.Visible = false;
                    //lblErrSearch.Visible = true;
                    //lblErrSearch.Text = "No Records Found";
                    //lblErrSearch.Visible = true;
                    //return;
                    divPopAlert.Visible = true;
                    lblAlertMsg.Text = "No Records Found";
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion

    #region Send SMS

    protected void btnSendSMS_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
            string msgValue = string.Empty;
            bool selectedchecked = false;
            string collegeCodeValue = string.Empty;
            string qry = string.Empty;
            DataSet dsRollNo = new DataSet();
            bool isSendSuccess = false;
            if (!chkSms.Checked)
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "Please Check SMS To Send SMS";
                return;
            }
            if (rblCollegeOrDegreeWise.SelectedIndex == 1)
            {
                foreach (GridViewRow gvRow in gvDegreeDetails.Rows)
                {
                    CheckBox chkDegree = (CheckBox)gvRow.FindControl("chkDWiseSelect");
                    Label lblDWiseBatchYear = (Label)gvRow.FindControl("lblDWiseBatch");
                    Label lblDWiseDegreeCode = (Label)gvRow.FindControl("lblDWiseCourse_id");
                    Label lblDWiseSemester = (Label)gvRow.FindControl("lblDWiseCurrent_Semester");
                    Label lblDWiseSection = (Label)gvRow.FindControl("lblDWiseSections");
                    TextBox txtCircularMsg = (TextBox)gvRow.FindControl("txtDWiseCircularMessage");
                    string batchYear = string.Empty;
                    string degreeCode = string.Empty;
                    string section = string.Empty;
                    string semester = string.Empty;
                    msgValue = string.Empty;

                    batchYear = Convert.ToString(lblDWiseBatchYear.Text).Trim();
                    degreeCode = Convert.ToString(lblDWiseDegreeCode.Text).Trim();
                    semester = Convert.ToString(lblDWiseSemester.Text).Trim();
                    section = Convert.ToString(lblDWiseSection.Text).Trim();
                    msgValue = Convert.ToString(txtCircularMsg.Text).Trim();
                    if (chkDegree.Checked)
                    {
                        //'" + collegeCodeValue + "'
                        selectedchecked = true;
                        collegeCodeValue = Convert.ToString(ddlCollege.SelectedValue).Trim();
                        qry = "select distinct STUFF((SELECT ',''' + s.Roll_No+'''' FROM Registration s where s.college_code='" + collegeCodeValue + "' and s.Batch_Year='" + batchYear + "' and s.degree_code='" + degreeCode + "' and s.Current_Semester='" + semester + "' " + ((!string.IsNullOrEmpty(section)) ? " and ISNULL(ltrim(rtrim(s.Sections)),'')='" + section + "'" : "") + "  ORDER BY s.Roll_No  FOR XML PATH('') ),1, 1, '') as All_Roll_No from Registration r1 where r1.college_code='" + collegeCodeValue + "' and r1.Batch_Year='" + batchYear + "' and r1.degree_code='" + degreeCode + "' and r1.Current_Semester='" + semester + "'" + ((!string.IsNullOrEmpty(section)) ? " and ISNULL(ltrim(rtrim(r1.Sections)),'')='" + section + "'" : "") + " ";
                        dsRollNo = d2.select_method_wo_parameter(qry, text);
                        if (dsRollNo.Tables.Count > 0 && dsRollNo.Tables[0].Rows.Count > 0)
                        {
                            string allRollNo = Convert.ToString(dsRollNo.Tables[0].Rows[0]["All_Roll_No"]).Trim();
                            if (!string.IsNullOrEmpty(allRollNo))
                            {
                                if (chkSms.Checked)
                                    SendingSms(allRollNo, msgValue, out isSendSuccess);
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (GridViewRow gvRow in gvCollegeWise.Rows)
                {
                    CheckBox chkCollege = (CheckBox)gvRow.FindControl("chkCWiseSelect");
                    Label lblCollegeCodeValue = (Label)gvRow.FindControl("lblCWiseCollegeCode");
                    collegeCodeValue = Convert.ToString(lblCollegeCodeValue.Text).Trim();

                    TextBox txtCircularmsg = (TextBox)gvRow.FindControl("txtCWiseCircularMessage");
                    msgValue = Convert.ToString(txtCircularmsg.Text).Trim();
                    if (chkCollege.Checked)
                    {
                        selectedchecked = true;
                        dsRollNo = new DataSet();
                        if (!string.IsNullOrEmpty(collegeCodeValue))
                        {
                            qry = "select distinct STUFF((SELECT ',''' + s.Roll_No+'''' FROM Registration s where s.college_code='" + collegeCodeValue + "' ORDER BY s.Roll_No  FOR XML PATH('') ),1, 1, '') as All_Roll_No from Registration r1 where college_code='" + collegeCodeValue + "' ";
                            dsRollNo = d2.select_method_wo_parameter(qry, text);
                            if (dsRollNo.Tables.Count > 0 && dsRollNo.Tables[0].Rows.Count > 0)
                            {
                                string allRollNo = Convert.ToString(dsRollNo.Tables[0].Rows[0]["All_Roll_No"]).Trim();
                                if (!string.IsNullOrEmpty(allRollNo))
                                {
                                    if (chkSms.Checked)
                                        SendingSms(allRollNo, msgValue, out isSendSuccess);
                                }
                            }
                        }
                    }
                }
            }
            if (!selectedchecked)
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "Please Select Atleast One " + ((rblCollegeOrDegreeWise.SelectedIndex == 0) ? ((isSchool) ? "School" : "College") : "Degree");
                return;
            }
            if (isSendSuccess)
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "Send Successfully";
                return;
            }
            else
            {
                divPopAlert.Visible = true;
                lblAlertMsg.Text = "Not Send";
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public void SendingSms(string rollno, string msgText, out bool isSend)
    {
        isSend = false;
        try
        {
            string Gender = string.Empty;
            string collegename1 = string.Empty;
            string MsgText = msgText;
            string RecepientNo = string.Empty;
            string user_id = string.Empty;
            string section = string.Empty;

            string str1 = string.Empty;
            string group_code = Convert.ToString(Session["group_code"]).Trim();
            if (group_code.Contains(";"))
            {
                string[] group_semi = group_code.Split(';');
                group_code = Convert.ToString(group_semi[0]).Trim();
            }
            if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                str1 = "select distinct textname,taxtval from Attendance_Settings where college_code='" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "' and USER_ID='" + group_code + "'";
                str1 = str1 + "  select template from master_Settings where settings='SmsAttendanceTepmlate' and group_code='" + group_code + "'and value='1'";
            }
            else
            {
                str1 = "select distinct textname,taxtval from Attendance_Settings where college_code='" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "' and USER_ID='" + Convert.ToString(Session["usercode"]).Trim() + "'";
                str1 = str1 + "  select template from master_Settings where settings='SmsAttendanceTepmlate' and usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'and value='1'";
            }
            Boolean flage = false;
            DataSet ds1;
            ds1 = d2.select_method_wo_parameter(str1, "txt");
            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                for (int jj = 0; jj < ds1.Tables[0].Rows.Count; jj++)
                {
                    if (Convert.ToString(ds1.Tables[0].Rows[jj]["TextName"]).Trim().ToLower() == "attendance sms for absent" && Convert.ToString(ds1.Tables[0].Rows[jj]["Taxtval"]).Trim() == "1")
                    {
                        flage = true;
                    }
                }
            }

            string degcode = string.Empty;
            string ssr = "select * from Track_Value where college_code='" + Session["collegecode"].ToString() + "'";
            DataSet dstrack;
            dstrack = d2.select_method_wo_parameter(ssr, "txt");
            if (dstrack.Tables.Count > 0 && dstrack.Tables[0].Rows.Count > 0)
            {
                user_id = Convert.ToString(dstrack.Tables[0].Rows[0]["SMS_User_ID"]).Trim();
                string Phone = "select distinct isnull(a.parentF_Mobile,'0') as FatherMobile,isnull(a.parentM_Mobile,'0')as MotherMobile,isnull(a.Student_Mobile,'0') as StudentMobile,a.sex as Gender,isnull(a.stud_name,r.stud_name) as StudName,r.degree_code,r.roll_admit from applyn a,registration r where a.app_no=r.app_no and r.roll_no in(" + rollno + ") and r.college_code='" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "'";
                DataSet dsMobile;
                dsMobile = d2.select_method_wo_parameter(Phone, "txt");
                if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && dsMobile.Tables.Count > 0 && dsMobile.Tables[0].Rows.Count > 0)
                {
                    string mothersMobileNos = string.Empty;
                    string fathersMobileNos = string.Empty;
                    string studentsMobileNos = string.Empty;

                    List<string> list = dsMobile.Tables[0].AsEnumerable().Select(r => r.Field<string>("FatherMobile")).ToList();
                    List<string> dtList = dsMobile.Tables[0].AsEnumerable().Select(dr => dr.Field<string>("FatherMobile")).ToList();
                    dtList.RemoveAll(x => x == "");
                    dtList.RemoveAll(x => x == "0");
                    dtList = dtList.Distinct().ToList();
                    fathersMobileNos = string.Join(",", dtList.ToArray());

                    list = dsMobile.Tables[0].AsEnumerable().Select(r => r.Field<string>("MotherMobile")).ToList();
                    list.RemoveAll(x => x == "");
                    list.RemoveAll(x => x == "0");
                    list = list.Distinct().ToList();
                    mothersMobileNos = string.Join(",", list.ToArray());

                    list = dsMobile.Tables[0].AsEnumerable().Select(r => r.Field<string>("StudentMobile")).ToList();
                    list.RemoveAll(x => x == "");
                    list.RemoveAll(x => x == "0");
                    list = list.Distinct().ToList();
                    studentsMobileNos = string.Join(",", list.ToArray());

                    degcode = Convert.ToString(dsMobile.Tables[0].Rows[0]["degree_code"]).Trim();


                    SMSSettings smsObject = new SMSSettings();
                    smsObject.User_degreecode = Convert.ToInt32(degcode);
                    smsObject.User_collegecode = Convert.ToInt32(ddlCollege.SelectedValue);
                    smsObject.User_usercode = usercode;
                    smsObject.Text_message = MsgText;
                    smsObject.IsStaff = 0;

                    byte sms_settings = smsObject.getSMSSettings(smsObject.User_collegecode);

                    for (int jj1 = 0; jj1 < ds1.Tables[0].Rows.Count; jj1++)
                    {
                        for (int smsI = 0; smsI < dsMobile.Tables[0].Rows.Count; smsI++)
                        {
                            if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Father" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                            {
                                if (dsMobile.Tables[0].Rows[smsI]["FatherMobile"].ToString() != "0" && dsMobile.Tables[0].Rows[smsI]["FatherMobile"].ToString().Trim() != "" && dsMobile.Tables[0].Rows[smsI]["FatherMobile"].ToString() != null)
                                {
                                    RecepientNo = dsMobile.Tables[0].Rows[smsI]["FatherMobile"].ToString().Trim();
                                    string getval = d2.GetUserapi(user_id);
                                    string[] spret = getval.Split('-');
                                    if (spret.GetUpperBound(0) == 1)
                                    {
                                        SenderID = spret[0].ToString();
                                        Password = spret[0].ToString();
                                        Session["api"] = user_id;
                                        Session["senderid"] = SenderID;
                                    }
                                    //int nofosmssend = d2.send_sms(user_id, ddlCollege.SelectedValue.ToString(), usercode, RecepientNo, MsgText, "0");
                                    //int nofosmssend = d2.sendNewSMS(degcode, ddlCollege.SelectedValue, usercode, RecepientNo, MsgText, "0");
                                    //New SMS Function

                                    smsObject.MobileNos = RecepientNo;
                                    smsObject.AdmissionNos = dsMobile.Tables[0].Rows[smsI]["roll_admit"].ToString();
                                    int nofosmssend = smsObject.sendTextMessage(sms_settings);
                                    if (nofosmssend != 0)
                                        isSend = true;
                                }
                            }
                            if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Mother" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                            {
                                if (dsMobile.Tables[0].Rows[smsI]["MotherMobile"].ToString() != "0" && dsMobile.Tables[0].Rows[smsI]["MotherMobile"].ToString().Trim() != "" && dsMobile.Tables[0].Rows[smsI]["MotherMobile"].ToString() != null)
                                {
                                    RecepientNo = dsMobile.Tables[0].Rows[smsI]["MotherMobile"].ToString().Trim();
                                    string getval = d2.GetUserapi(user_id);
                                    string[] spret = getval.Split('-');
                                    if (spret.GetUpperBound(0) == 1)
                                    {
                                        SenderID = spret[0].ToString();
                                        Password = spret[0].ToString();
                                        Session["api"] = user_id;
                                        Session["senderid"] = SenderID;
                                    }
                                    //int nofosmssend = d2.send_sms(user_id, ddlCollege.SelectedValue.ToString(), usercode, RecepientNo, MsgText, "0");
                                    //int nofosmssend = d2.sendNewSMS(degcode, ddlCollege.SelectedValue, usercode, RecepientNo, MsgText, "0");
                                    //New SMS Function

                                    smsObject.MobileNos = RecepientNo;
                                    smsObject.AdmissionNos = dsMobile.Tables[0].Rows[smsI]["roll_admit"].ToString();
                                    int nofosmssend = smsObject.sendTextMessage(sms_settings);
                                    if (nofosmssend != 0)
                                        isSend = true;
                                }
                            }
                            if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Student" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                            {
                                if (dsMobile.Tables[0].Rows[smsI]["StudentMobile"].ToString() != "0" && dsMobile.Tables[0].Rows[smsI]["StudentMobile"].ToString().Trim() != "" && dsMobile.Tables[0].Rows[smsI]["StudentMobile"].ToString() != null)
                                {
                                    RecepientNo = dsMobile.Tables[0].Rows[smsI]["StudentMobile"].ToString().Trim();
                                    string getval = d2.GetUserapi(user_id);
                                    string[] spret = getval.Split('-');
                                    if (spret.GetUpperBound(0) == 1)
                                    {
                                        SenderID = spret[0].ToString();
                                        Password = spret[0].ToString();
                                        Session["api"] = user_id;
                                        Session["senderid"] = SenderID;
                                    }
                                    //int nofosmssend = d2.send_sms(user_id, ddlCollege.SelectedValue.ToString(), usercode, RecepientNo, MsgText, "0");
                                    //int nofosmssend = d2.sendNewSMS(degcode, ddlCollege.SelectedValue, usercode, RecepientNo, MsgText, "0");
                                    //New SMS Function

                                    smsObject.MobileNos = RecepientNo;
                                    smsObject.AdmissionNos = dsMobile.Tables[0].Rows[smsI]["roll_admit"].ToString();
                                    int nofosmssend = smsObject.sendTextMessage(sms_settings);
                                    if (nofosmssend != 0)
                                        isSend = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion

}