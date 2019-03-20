using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Threading;
using Farpoint = FarPoint.Web.Spread;
using System.Collections;
using System.Drawing;
using System.Collections.Generic;
using InsproDataAccess;
using System.Configuration;

public partial class StudentsRedoBatchUpdation : System.Web.UI.Page
{
    static string collegecodestat = "13";
    static int choosedmode = 0;

    #region Field Declaration

    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();

    Hashtable hat = new Hashtable();

    string userCode = string.Empty;
    string collegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;

    bool isSchool = false;

    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();

    string collegeCodeNew = string.Empty;
    string batchYear = string.Empty;
    string degreeCode = string.Empty;
    string courseId = string.Empty;
    string semester = string.Empty;
    string section = string.Empty;

    string qryCollegeCode = string.Empty;
    string qryBatchYear = string.Empty;
    string qryDegreeCode = string.Empty;
    string qryCourseId = string.Empty;
    string qrySemester = string.Empty;
    string qrySection = string.Empty;

    int selected = 0;

    #endregion Field Declaration

    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //Page.DataBind();
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
            userCode = Convert.ToString(Session["usercode"]);
            collegeCode = Convert.ToString(Session["collegecode"]);
            singleUser = Convert.ToString(Session["single_user"]);
            groupUserCode = Convert.ToString(Session["group_code"]);
            if (groupUserCode.Contains(';'))
            {
                string[] group_semi = groupUserCode.Split(';');
                groupUserCode = Convert.ToString(group_semi[0]);
            }

            string grouporusercode1 = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode1 = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
            }
            else if (Session["usercode"] != null)
            {
                grouporusercode1 = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
            }

            DataSet schoolds = new DataSet();
            string sqlschool = "select * from Master_Settings where settings='schoolorcollege' and " + grouporusercode1 + "";
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
                divPopupAlert.Visible = false;
                lblAlertMsg.Text = string.Empty;
                divMainContent.Visible = false;
                divSearch.Visible = true;
                chkSearchBy.Checked = false;
                ddlSearchBy.Enabled = false;
                txtSearchBy.Enabled = false;

                #region LoadHeader

                Bindcollege();
                BindBatch();
                BindDegree();
                bindbranch();
                bindsem();
                BindSectionDetail();

                #endregion LoadHeader

                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                Session["AdmissionNo"] = "0";
                string grouporusercode = string.Empty;
                if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                {
                    grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
                }
                else if (Session["usercode"] != null)
                {
                    grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
                }
                string Master = "select * from Master_Settings where " + grouporusercode + "";
                DataSet ds = d2.select_method(Master, hat, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (Convert.ToString(ds.Tables[0].Rows[i]["settings"]).Trim().ToLower() == "roll no" && Convert.ToString(ds.Tables[0].Rows[i]["value"]).Trim().ToLower() == "1")
                        {
                            Session["Rollflag"] = "1";
                        }
                        else if (Convert.ToString(ds.Tables[0].Rows[i]["settings"]).Trim().ToLower() == "register no" && Convert.ToString(ds.Tables[0].Rows[i]["value"]).Trim().ToLower() == "1")
                        {
                            Session["Regflag"] = "1";
                        }
                        else if (Convert.ToString(ds.Tables[0].Rows[i]["settings"]).Trim().ToLower() == "student_type" && Convert.ToString(ds.Tables[0].Rows[i]["value"]).Trim().ToLower() == "1")
                        {
                            Session["Studflag"] = "1";
                        }
                        else if (Convert.ToString(ds.Tables[0].Rows[i]["settings"]).Trim().ToLower() == "admission no" && Convert.ToString(ds.Tables[0].Rows[i]["value"]).Trim().ToLower() == "1")
                        {
                            Session["AdmissionNo"] = "1";
                        }
                    }
                }
                ChangeHeaderName(isSchool);
            }
        }
        catch (ThreadAbortException tt)
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

    public void Bindcollege()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;

            string columnfield = string.Empty;
            groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
            if (groupUserCode.Contains(';'))
            {
                string[] group_semi = groupUserCode.Split(';');
                groupUserCode = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(groupUserCode).Trim() != "") && Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                columnfield = " and group_code='" + groupUserCode + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            hat.Clear();
            hat.Add("column_field", Convert.ToString(columnfield));
            ds.Dispose();
            ds.Clear();
            ds.Reset();
            ds = d2.select_method("bind_college", hat, "sp");
            ddlCollege.Items.Clear();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlCollege.DataSource = ds;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
                ddlCollege.SelectedIndex = 0;
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

    public void BindBatch()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ds.Dispose();
            ds.Reset();
            ds.Clear();
            ddlBatch.Items.Clear();
            if (ddlCollege.Items.Count > 0)
            {
                selected = 0;
                qryCollegeCode = string.Empty;
                collegeCodeNew = string.Empty;
                foreach (ListItem li in ddlCollege.Items)
                {
                    if (li.Selected)
                    {
                        selected++;
                        if (string.IsNullOrEmpty(collegeCodeNew.Trim()))
                        {
                            collegeCodeNew = "'" + li.Value.Trim() + "'";
                        }
                        else
                        {
                            collegeCodeNew += ",'" + li.Value.Trim() + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(collegeCodeNew) && selected > 0)
                {
                    qryCollegeCode = " and r.college_code in(" + collegeCodeNew + ")";
                }
                ds = d2.select_method_wo_parameter("select distinct r.Batch_Year from Registration r where r.batch_year<>'-1' and r.batch_year<>'' and ISNULL(r.isRedo,'0')='0' and CC='1' " + qryCollegeCode + " order by r.Batch_Year desc", "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlBatch.DataSource = ds;
                    ddlBatch.DataTextField = "Batch_Year";
                    ddlBatch.DataValueField = "Batch_Year";
                    ddlBatch.DataBind();
                    ddlBatch.SelectedIndex = 0;
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

    public void BindDegree()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            ddlDegree.Items.Clear();
            if (groupUserCode.Contains(';'))
            {
                string[] group_semi = groupUserCode.Split(';');
                groupUserCode = Convert.ToString(group_semi[0]);
            }
            ds.Dispose();
            ds.Reset();
            string columnfield = string.Empty;
            groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
            if (groupUserCode.Contains(';'))
            {
                string[] group_semi = groupUserCode.Split(';');
                groupUserCode = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(groupUserCode).Trim() != "") && Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                columnfield = " and dp.group_code='" + groupUserCode.Trim() + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and dp.user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            selected = 0;
            qryCollegeCode = string.Empty;
            collegeCodeNew = string.Empty;
            foreach (ListItem li in ddlCollege.Items)
            {
                if (li.Selected)
                {
                    selected++;
                    if (string.IsNullOrEmpty(collegeCodeNew.Trim()))
                    {
                        collegeCodeNew = "'" + li.Value.Trim() + "'";
                    }
                    else
                    {
                        collegeCodeNew += ",'" + li.Value.Trim() + "'";
                    }
                }
            }
            if (!string.IsNullOrEmpty(collegeCodeNew) && selected > 0)
            {
                qryCollegeCode = " and c.college_code in(" + collegeCodeNew + ") ";
            }
            qryBatchYear = string.Empty;
            batchYear = string.Empty;
            if (ddlBatch.Items.Count > 0)
            {
                batchYear = Convert.ToString(ddlBatch.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(batchYear))
                    qryBatchYear = " and r.batch_year in(" + batchYear + ") ";
            }
            ds = d2.select_method_wo_parameter("select distinct dg.course_id,c.course_name,c.Priority from Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r where  r.degree_code=dg.Degree_Code and dp.degree_code=dg.Degree_Code and dg.Course_Id=c.Course_Id and dg.Dept_Code=dt.Dept_Code and r.college_code=c.college_code and r.college_code=dg.college_code and dt.college_code=r.college_code and r.CC='1' and ISNULL(r.isRedo,'0')='0' " + qryCollegeCode + qryBatchYear + columnfield + "  order by c.Priority", "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlDegree.DataSource = ds;
                ddlDegree.DataTextField = "course_name";
                ddlDegree.DataValueField = "course_id";
                ddlDegree.DataBind();
                ddlDegree.SelectedIndex = 0;
            }

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }

    }

    public void bindbranch()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            string course_id = Convert.ToString(ddlDegree.SelectedValue);
            ddlBranch.Items.Clear();
            if (groupUserCode.Contains(';'))
            {
                string[] group_semi = groupUserCode.Split(';');
                groupUserCode = Convert.ToString(group_semi[0]);
            }
            ds.Dispose();
            ds.Reset();
            string columnfield = string.Empty;
            groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
            if (groupUserCode.Contains(';'))
            {
                string[] group_semi = groupUserCode.Split(';');
                groupUserCode = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(groupUserCode).Trim() != "") && Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                columnfield = " and dp.group_code='" + groupUserCode.Trim() + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and dp.user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }

            selected = 0;
            qryCollegeCode = string.Empty;
            collegeCodeNew = string.Empty;
            foreach (ListItem li in ddlCollege.Items)
            {
                if (li.Selected)
                {
                    selected++;
                    if (string.IsNullOrEmpty(collegeCodeNew.Trim()))
                    {
                        collegeCodeNew = "'" + li.Value.Trim() + "'";
                    }
                    else
                    {
                        collegeCodeNew += ",'" + li.Value.Trim() + "'";
                    }
                }
            }
            if (!string.IsNullOrEmpty(collegeCodeNew) && selected > 0)
            {
                qryCollegeCode = " and c.college_code in(" + collegeCodeNew + ") ";
            }
            selected = 0;
            qryCourseId = string.Empty;
            courseId = string.Empty;
            if (ddlDegree.Items.Count > 0)
            {
                foreach (ListItem li in ddlDegree.Items)
                {
                    if (li.Selected)
                    {
                        selected++;
                        if (string.IsNullOrEmpty(courseId.Trim()))
                        {
                            courseId = "'" + li.Value.Trim() + "'";
                        }
                        else
                        {
                            courseId += ",'" + li.Value.Trim() + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(courseId))
                {
                    qryCourseId = " and c.Course_Id in(" + courseId + ")";
                }
            }
            qryBatchYear = string.Empty;
            batchYear = string.Empty;
            if (ddlBatch.Items.Count > 0)
            {
                batchYear = Convert.ToString(ddlBatch.SelectedValue).Trim();
                if (!string.IsNullOrEmpty(batchYear))
                    qryBatchYear = " and r.batch_year in(" + batchYear + ") ";
            }
            ds = d2.select_method_wo_parameter("select distinct dg.Degree_Code,dt.Dept_Name from Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r where  r.degree_code=dg.Degree_Code and dp.degree_code=dg.Degree_Code and dg.Course_Id=c.Course_Id and dg.Dept_Code=dt.Dept_Code and r.college_code=c.college_code and r.college_code=dg.college_code and dt.college_code=r.college_code and r.CC='1' and ISNULL(r.isRedo,'0')='0' " + qryCollegeCode + qryBatchYear + columnfield + qryCourseId + "order by dg.Degree_Code", "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlBranch.DataSource = ds;
                ddlBranch.DataTextField = "dept_name";
                ddlBranch.DataValueField = "degree_code";
                ddlBranch.DataBind();
                ddlBranch.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public void bindsem()
    {
        try
        {
            ddlSem.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            selected = 0;
            qryCollegeCode = string.Empty;
            collegeCodeNew = string.Empty;
            foreach (ListItem li in ddlCollege.Items)
            {
                if (li.Selected)
                {
                    selected++;
                    if (string.IsNullOrEmpty(collegeCodeNew.Trim()))
                    {
                        collegeCodeNew = "'" + li.Value.Trim() + "'";
                    }
                    else
                    {
                        collegeCodeNew += ",'" + li.Value.Trim() + "'";
                    }
                }
            }
            if (!string.IsNullOrEmpty(collegeCodeNew) && selected > 0)
            {
                qryCollegeCode = " and r.college_code in(" + collegeCodeNew + ") ";
            }
            selected = 0;
            qryBatchYear = string.Empty;
            batchYear = string.Empty;
            if (ddlBatch.Items.Count > 0)
            {
                foreach (ListItem li in ddlBatch.Items)
                {
                    if (li.Selected)
                    {
                        selected++;
                        if (string.IsNullOrEmpty(batchYear.Trim()))
                        {
                            batchYear = "'" + li.Value.Trim() + "'";
                        }
                        else
                        {
                            batchYear += ",'" + li.Value.Trim() + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(batchYear) && selected > 0)
                {
                    qryBatchYear = " and el.Batch_year in(" + batchYear + ")";
                }
            }
            selected = 0;
            qryDegreeCode = string.Empty;
            degreeCode = string.Empty;
            if (ddlBranch.Items.Count > 0)
            {
                foreach (ListItem li in ddlBranch.Items)
                {
                    if (li.Selected)
                    {
                        selected++;
                        if (string.IsNullOrEmpty(degreeCode.Trim()))
                        {
                            degreeCode = "'" + li.Value.Trim() + "'";
                        }
                        else
                        {
                            degreeCode += ",'" + li.Value.Trim() + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(degreeCode))
                {
                    qryDegreeCode = " and el.degree_code in(" + degreeCode + ")";
                }
            }
            DataSet ds = new DataSet();
            string sqlnew = string.Empty;
            //sqlnew = "select distinct max(ndurations) ndurations,first_year_nonsemester from ndegree where ndurations<>'0'" + qryDegreeCode + qryCollegeCode + qryBatchYear + " group by first_year_nonsemester";
            ds.Clear();
            //ds = d2.select_method_wo_parameter(sqlnew, "Text");
            sqlnew = "select distinct el.Semester from Eligibility_list el,Registration r where r.App_No=el.app_no and el.is_eligible='3' " + qryDegreeCode + qryCollegeCode + qryBatchYear + " order by el.Semester";
            ds = d2.select_method_wo_parameter(sqlnew, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlSem.DataSource = ds;
                ddlSem.DataTextField = "Semester";
                ddlSem.DataValueField = "Semester";
                ddlSem.DataBind();
                ddlSem.SelectedIndex = 0;
            }
            //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
            //    duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
            //    for (i = 1; i <= duration; i++)
            //    {
            //        if (first_year == false)
            //        {
            //            ddlSem.Items.Add(i.ToString());
            //        }
            //        else if (first_year == true && i == 2)
            //        {
            //            ddlSem.Items.Add(i.ToString());
            //        }
            //    }
            //}
            //else
            //{
            //    sqlnew = "select distinct max(duration) duration,first_year_nonsemester from degree where duration<>'0' " + qryDegreeCode + qryCollegeCode + " group by first_year_nonsemester";
            //    ds.Clear();
            //    ds = d2.select_method_wo_parameter(sqlnew, "Text");
            //    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //    {
            //        first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
            //        duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
            //        for (i = 1; i <= duration; i++)
            //        {
            //            if (first_year == false)
            //            {
            //                ddlSem.Items.Add(i.ToString());
            //            }
            //            else if (first_year == true && i != 2)
            //            {
            //                ddlSem.Items.Add(i.ToString());
            //            }
            //        }
            //    }
            //}
        }
        catch (Exception ex)
        {
        }
    }

    public void BindSectionDetail()
    {
        string batchyear = string.Empty;
        string branch = string.Empty;
        string batch = string.Empty;
        DataSet ds = new DataSet();
        txtSection.Text = "Select";
        txtSection.Enabled = false;
        chkSection.Checked = false;
        cblSection.Items.Clear();
        selected = 0;
        qryCollegeCode = string.Empty;
        collegeCodeNew = string.Empty;
        foreach (ListItem li in ddlCollege.Items)
        {
            if (li.Selected)
            {
                selected++;
                if (string.IsNullOrEmpty(collegeCodeNew.Trim()))
                {
                    collegeCodeNew = "'" + li.Value.Trim() + "'";
                }
                else
                {
                    collegeCodeNew += ",'" + li.Value.Trim() + "'";
                }
            }
        }
        if (!string.IsNullOrEmpty(collegeCodeNew) && selected > 0)
        {
            qryCollegeCode = " and college_code in(" + collegeCodeNew + ") ";
        }
        selected = 0;
        qryBatchYear = string.Empty;
        batchYear = string.Empty;
        if (ddlBatch.Items.Count > 0)
        {
            foreach (ListItem li in ddlBatch.Items)
            {
                if (li.Selected)
                {
                    selected++;
                    if (string.IsNullOrEmpty(batchYear.Trim()))
                    {
                        batchYear = "'" + li.Value.Trim() + "'";
                    }
                    else
                    {
                        batchYear += ",'" + li.Value.Trim() + "'";
                    }
                }
            }
            if (!string.IsNullOrEmpty(batchYear) && selected > 0)
            {
                qryBatchYear = " and Batch_year in(" + batchYear + ")";
            }
        }
        selected = 0;
        qryDegreeCode = string.Empty;
        degreeCode = string.Empty;
        if (ddlBranch.Items.Count > 0)
        {
            foreach (ListItem li in ddlBranch.Items)
            {
                if (li.Selected)
                {
                    selected++;
                    if (string.IsNullOrEmpty(degreeCode.Trim()))
                    {
                        degreeCode = "'" + li.Value.Trim() + "'";
                    }
                    else
                    {
                        degreeCode += ",'" + li.Value.Trim() + "'";
                    }
                }
            }
            if (!string.IsNullOrEmpty(degreeCode))
            {
                qryDegreeCode = " and degree_code in(" + degreeCode + ")";
            }
        }
        if (!string.IsNullOrEmpty(qryBatchYear) && !string.IsNullOrEmpty(qryCollegeCode) && !string.IsNullOrEmpty(qryDegreeCode))
        {
            string sqlnew = "select distinct sections from registration where sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar' " + qryDegreeCode + qryCollegeCode + qryBatchYear + " order by sections";
            ds.Clear();
            ds = d2.select_method_wo_parameter(sqlnew, "Text");
        }
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            cblSection.DataSource = ds;
            cblSection.DataTextField = "sections";
            cblSection.DataValueField = "sections";
            cblSection.DataBind();
            for (int h = 0; h < cblSection.Items.Count; h++)
            {
                cblSection.Items[h].Selected = true;
            }
            txtSection.Text = "Section" + "(" + cblSection.Items.Count + ")";
            chkSection.Checked = true;
            txtSection.Enabled = true;
        }
        else
        {
            txtSection.Enabled = false;
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
            lblBranch.Text = ((!isschool) ? "Department" : "Standard");
            lblSem.Text = ((!isschool) ? "Semester" : "Term");
            lblSec.Text = ((!isschool) ? "Section" : "Section");
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public void Init_Spread(Farpoint.FpSpread FpSpread1, int type = 0)
    {
        try
        {
            #region FpSpread Style

            FpSpread1.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;

            #endregion FpSpread Style

            #region SpreadStyles

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Size = FontUnit.Medium;
            darkstyle.Font.Bold = true;
            darkstyle.HorizontalAlign = HorizontalAlign.Center;
            darkstyle.VerticalAlign = VerticalAlign.Middle;
            darkstyle.ForeColor = System.Drawing.Color.Black;
            darkstyle.Border.BorderSize = 1;
            darkstyle.Border.BorderColor = System.Drawing.Color.Black;

            FarPoint.Web.Spread.StyleInfo sheetstyle = new FarPoint.Web.Spread.StyleInfo();
            sheetstyle.Font.Name = "Book Antiqua";
            sheetstyle.Font.Size = FontUnit.Medium;
            sheetstyle.Font.Bold = true;
            sheetstyle.HorizontalAlign = HorizontalAlign.Center;
            sheetstyle.VerticalAlign = VerticalAlign.Middle;
            sheetstyle.ForeColor = System.Drawing.Color.Black;
            sheetstyle.Border.BorderSize = 1;
            sheetstyle.Border.BorderColor = System.Drawing.Color.Black;

            #endregion SpreadStyles

            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread1.Sheets[0].DefaultStyle = sheetstyle;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;

            FpSpread1.HorizontalScrollBarPolicy = Farpoint.ScrollBarPolicy.AsNeeded;
            FpSpread1.VerticalScrollBarPolicy = Farpoint.ScrollBarPolicy.AsNeeded;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.RowHeader.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = false;
            FpSpread1.Sheets[0].RowCount = 0;
            if (type == 0)
            {
                FpSpread1.Sheets[0].ColumnCount = 11;

                FpSpread1.Sheets[0].Columns[0].Width = 45;
                FpSpread1.Sheets[0].Columns[1].Width = 38;
                FpSpread1.Sheets[0].Columns[2].Width = 60;
                FpSpread1.Sheets[0].Columns[3].Width = 165;
                FpSpread1.Sheets[0].Columns[4].Width = 65;
                FpSpread1.Sheets[0].Columns[5].Width = 100;
                FpSpread1.Sheets[0].Columns[6].Width = 100;
                FpSpread1.Sheets[0].Columns[7].Width = 90;
                FpSpread1.Sheets[0].Columns[8].Width = 200;
                FpSpread1.Sheets[0].Columns[9].Width = 45;
                FpSpread1.Sheets[0].Columns[10].Width = 45;

                FpSpread1.Sheets[0].Columns[0].Locked = false;
                FpSpread1.Sheets[0].Columns[1].Locked = true;
                FpSpread1.Sheets[0].Columns[2].Locked = true;
                FpSpread1.Sheets[0].Columns[3].Locked = true;
                FpSpread1.Sheets[0].Columns[4].Locked = true;
                FpSpread1.Sheets[0].Columns[5].Locked = true;
                FpSpread1.Sheets[0].Columns[6].Locked = true;
                FpSpread1.Sheets[0].Columns[7].Locked = true;
                FpSpread1.Sheets[0].Columns[8].Locked = true;
                FpSpread1.Sheets[0].Columns[9].Locked = false;
                FpSpread1.Sheets[0].Columns[10].Locked = false;

                FpSpread1.Sheets[0].Columns[0].Resizable = false;
                FpSpread1.Sheets[0].Columns[1].Resizable = false;
                FpSpread1.Sheets[0].Columns[2].Resizable = false;
                FpSpread1.Sheets[0].Columns[3].Resizable = false;
                FpSpread1.Sheets[0].Columns[4].Resizable = false;
                FpSpread1.Sheets[0].Columns[5].Resizable = false;
                FpSpread1.Sheets[0].Columns[6].Resizable = false;
                FpSpread1.Sheets[0].Columns[7].Resizable = false;
                FpSpread1.Sheets[0].Columns[8].Resizable = false;
                FpSpread1.Sheets[0].Columns[9].Resizable = false;
                FpSpread1.Sheets[0].Columns[10].Resizable = false;

                FpSpread1.Sheets[0].Columns[0].Visible = true;
                FpSpread1.Sheets[0].Columns[1].Visible = true;
                FpSpread1.Sheets[0].Columns[2].Visible = true;
                FpSpread1.Sheets[0].Columns[3].Visible = true;
                FpSpread1.Sheets[0].Columns[4].Visible = true;
                FpSpread1.Sheets[0].Columns[5].Visible = true;
                FpSpread1.Sheets[0].Columns[6].Visible = true;
                FpSpread1.Sheets[0].Columns[7].Visible = true;
                FpSpread1.Sheets[0].Columns[8].Visible = true;
                FpSpread1.Sheets[0].Columns[9].Visible = true;
                FpSpread1.Sheets[0].Columns[10].Visible = true;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Select";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Batch";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Degree";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Semester";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Roll No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Reg No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Admission No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Name";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Redo Batch";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Redo Sem";

                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 10, 2, 1);

                FpSpread1.Sheets[0].SetColumnMerge(2, Farpoint.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(3, Farpoint.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(4, Farpoint.Model.MergePolicy.Always);

                FpSpread1.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[9].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Columns[10].VerticalAlign = VerticalAlign.Middle;

            }
            else
            {
                FpSpread1.Sheets[0].ColumnCount = 11;

                FpSpread1.Sheets[0].Columns[0].Width = 45;
                FpSpread1.Sheets[0].Columns[1].Width = 38;
                FpSpread1.Sheets[0].Columns[2].Width = 60;
                FpSpread1.Sheets[0].Columns[3].Width = 165;
                FpSpread1.Sheets[0].Columns[4].Width = 65;
                FpSpread1.Sheets[0].Columns[5].Width = 100;
                FpSpread1.Sheets[0].Columns[6].Width = 100;
                FpSpread1.Sheets[0].Columns[7].Width = 90;
                FpSpread1.Sheets[0].Columns[8].Width = 200;
                FpSpread1.Sheets[0].Columns[9].Width = 45;
                FpSpread1.Sheets[0].Columns[10].Width = 45;

                FpSpread1.Sheets[0].Columns[0].Locked = false;
                FpSpread1.Sheets[0].Columns[1].Locked = true;
                FpSpread1.Sheets[0].Columns[2].Locked = true;
                FpSpread1.Sheets[0].Columns[3].Locked = true;
                FpSpread1.Sheets[0].Columns[4].Locked = true;
                FpSpread1.Sheets[0].Columns[5].Locked = true;
                FpSpread1.Sheets[0].Columns[6].Locked = true;
                FpSpread1.Sheets[0].Columns[7].Locked = true;
                FpSpread1.Sheets[0].Columns[8].Locked = true;
                FpSpread1.Sheets[0].Columns[9].Locked = false;
                FpSpread1.Sheets[0].Columns[10].Locked = false;

                FpSpread1.Sheets[0].Columns[0].Resizable = false;
                FpSpread1.Sheets[0].Columns[1].Resizable = false;
                FpSpread1.Sheets[0].Columns[2].Resizable = false;
                FpSpread1.Sheets[0].Columns[3].Resizable = false;
                FpSpread1.Sheets[0].Columns[4].Resizable = false;
                FpSpread1.Sheets[0].Columns[5].Resizable = false;
                FpSpread1.Sheets[0].Columns[6].Resizable = false;
                FpSpread1.Sheets[0].Columns[7].Resizable = false;
                FpSpread1.Sheets[0].Columns[8].Resizable = false;
                FpSpread1.Sheets[0].Columns[9].Resizable = false;
                FpSpread1.Sheets[0].Columns[10].Resizable = false;

                FpSpread1.Sheets[0].Columns[0].Visible = true;
                FpSpread1.Sheets[0].Columns[1].Visible = true;
                FpSpread1.Sheets[0].Columns[2].Visible = true;
                FpSpread1.Sheets[0].Columns[3].Visible = true;
                FpSpread1.Sheets[0].Columns[4].Visible = true;
                FpSpread1.Sheets[0].Columns[5].Visible = true;
                FpSpread1.Sheets[0].Columns[6].Visible = true;
                FpSpread1.Sheets[0].Columns[7].Visible = true;
                FpSpread1.Sheets[0].Columns[8].Visible = true;
                FpSpread1.Sheets[0].Columns[9].Visible = true;
                FpSpread1.Sheets[0].Columns[10].Visible = true;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Select";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Batch";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Degree";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Semester";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Roll No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Reg No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Admission No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Name";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Redo Batch";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Redo Sem";

                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 6, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 8, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 9, 2, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 10, 2, 1);

                FpSpread1.Sheets[0].SetColumnMerge(2, Farpoint.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(3, Farpoint.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].SetColumnMerge(4, Farpoint.Model.MergePolicy.Always);

                FpSpread1.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[9].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Columns[10].VerticalAlign = VerticalAlign.Middle;
                FpSpread1.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    private void bindRedoBatchYear()
    {
        try
        {
            ddlRedoBatch.Items.Clear();
            //ddlRedoSem.Items.Clear();
            string qry = string.Empty;
            selected = 0;
            qryCollegeCode = string.Empty;
            collegeCodeNew = string.Empty;
            foreach (ListItem li in ddlCollege.Items)
            {
                if (li.Selected)
                {
                    selected++;
                    if (string.IsNullOrEmpty(collegeCodeNew.Trim()))
                    {
                        collegeCodeNew = "'" + li.Value.Trim() + "'";
                    }
                    else
                    {
                        collegeCodeNew += ",'" + li.Value.Trim() + "'";
                    }
                }
            }
            if (!string.IsNullOrEmpty(collegeCodeNew) && selected > 0)
            {
                qryCollegeCode = " and college_code in(" + collegeCodeNew + ") ";
            }

            selected = 0;
            qryCourseId = string.Empty;
            courseId = string.Empty;
            if (ddlDegree.Items.Count > 0)
            {
                foreach (ListItem li in ddlDegree.Items)
                {
                    if (li.Selected)
                    {
                        selected++;
                        if (string.IsNullOrEmpty(courseId.Trim()))
                        {
                            courseId = "'" + li.Value.Trim() + "'";
                        }
                        else
                        {
                            courseId += ",'" + li.Value.Trim() + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(courseId))
                {
                    qryCourseId = string.Empty;
                }
            }

            selected = 0;
            qryDegreeCode = string.Empty;
            degreeCode = string.Empty;
            if (ddlBranch.Items.Count > 0)
            {
                foreach (ListItem li in ddlBranch.Items)
                {
                    if (li.Selected)
                    {
                        selected++;
                        if (string.IsNullOrEmpty(degreeCode.Trim()))
                        {
                            degreeCode = "'" + li.Value.Trim() + "'";
                        }
                        else
                        {
                            degreeCode += ",'" + li.Value.Trim() + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(degreeCode))
                {
                    qryDegreeCode = " and r.degree_code in(" + degreeCode + ")";
                }
            }
            selected = 0;
            qrySemester = string.Empty;
            semester = string.Empty;
            if (ddlSem.Items.Count > 0)
            {
                foreach (ListItem li in ddlSem.Items)
                {
                    if (li.Selected)
                    {
                        lblRedoSemester.Text = li.Value.Trim();
                        selected++;
                        if (string.IsNullOrEmpty(semester.Trim()))
                        {
                            semester = "'" + li.Value.Trim() + "'";
                        }
                        else
                        {
                            semester += ",'" + li.Value.Trim() + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(semester))
                {
                    qrySemester = " and r.Current_Semester in(" + semester + ")";
                }
            }
            if (!string.IsNullOrEmpty(qrySemester) && !string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qryCollegeCode))
            {
                qry = "select distinct Batch_Year from Registration r where r.CC='0' and ISNULL(r.isRedo,'0')='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' " + qryDegreeCode + qryCollegeCode + qrySemester + "order by Batch_Year desc";
                DataSet dsRedo = new DataSet();
                dsRedo = d2.select_method_wo_parameter(qry, "text");
                if (dsRedo.Tables.Count > 0 && dsRedo.Tables[0].Rows.Count > 0)
                {
                    ddlRedoBatch.DataSource = dsRedo;
                    ddlRedoBatch.DataTextField = "Batch_Year";
                    ddlRedoBatch.DataValueField = "Batch_Year";
                    ddlRedoBatch.DataBind();
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

    private DataSet GetRedoBatchYear()
    {
        DataSet dsRedo = new DataSet();
        try
        {
            string qry = string.Empty;
            selected = 0;
            qryCollegeCode = string.Empty;
            collegeCodeNew = string.Empty;
            foreach (ListItem li in ddlCollege.Items)
            {
                if (li.Selected)
                {
                    selected++;
                    if (string.IsNullOrEmpty(collegeCodeNew.Trim()))
                    {
                        collegeCodeNew = "'" + li.Value.Trim() + "'";
                    }
                    else
                    {
                        collegeCodeNew += ",'" + li.Value.Trim() + "'";
                    }
                }
            }
            if (!string.IsNullOrEmpty(collegeCodeNew) && selected > 0)
            {
                qryCollegeCode = " and college_code in(" + collegeCodeNew + ") ";
            }

            selected = 0;
            qryCourseId = string.Empty;
            courseId = string.Empty;
            if (ddlDegree.Items.Count > 0)
            {
                foreach (ListItem li in ddlDegree.Items)
                {
                    if (li.Selected)
                    {
                        selected++;
                        if (string.IsNullOrEmpty(courseId.Trim()))
                        {
                            courseId = "'" + li.Value.Trim() + "'";
                        }
                        else
                        {
                            courseId += ",'" + li.Value.Trim() + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(courseId))
                {
                    qryCourseId = string.Empty;
                }
            }

            selected = 0;
            qryDegreeCode = string.Empty;
            degreeCode = string.Empty;
            if (ddlBranch.Items.Count > 0)
            {
                foreach (ListItem li in ddlBranch.Items)
                {
                    if (li.Selected)
                    {
                        selected++;
                        if (string.IsNullOrEmpty(degreeCode.Trim()))
                        {
                            degreeCode = "'" + li.Value.Trim() + "'";
                        }
                        else
                        {
                            degreeCode += ",'" + li.Value.Trim() + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(degreeCode))
                {
                    qryDegreeCode = " and r.degree_code in(" + degreeCode + ")";
                }
            }
            selected = 0;
            qrySemester = string.Empty;
            semester = string.Empty;
            if (ddlSem.Items.Count > 0)
            {
                foreach (ListItem li in ddlSem.Items)
                {
                    if (li.Selected)
                    {
                        lblRedoSemester.Text = li.Value.Trim();
                        selected++;
                        if (string.IsNullOrEmpty(semester.Trim()))
                        {
                            semester = "'" + li.Value.Trim() + "'";
                        }
                        else
                        {
                            semester += ",'" + li.Value.Trim() + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(semester))
                {
                    qrySemester = " and r.Current_Semester in(" + semester + ")";
                }
            }
            if (!string.IsNullOrEmpty(qrySemester) && !string.IsNullOrEmpty(qryDegreeCode) && !string.IsNullOrEmpty(qryCollegeCode))
            {
                qry = "select distinct Batch_Year from Registration r where r.CC='0' and ISNULL(r.isRedo,'0')='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' " + qryDegreeCode + qryCollegeCode + qrySemester + "order by Batch_Year desc";
                dsRedo = d2.select_method_wo_parameter(qry, "text");
            }

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
        return dsRedo;
    }

    #endregion Bind Header

    #region DropDown Events

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divPopupAlert.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divMainContent.Visible = false;

            BindDegree();
            bindbranch();
            bindsem();
            BindSectionDetail();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divPopupAlert.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divMainContent.Visible = false;

            BindDegree();
            bindbranch();
            bindsem();
            BindSectionDetail();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divPopupAlert.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divMainContent.Visible = false;
            bindbranch();
            bindsem();
            BindSectionDetail();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divPopupAlert.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divMainContent.Visible = false;
            bindsem();
            BindSectionDetail();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlSem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divPopupAlert.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divMainContent.Visible = false;
            BindSectionDetail();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void chkSection_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divPopupAlert.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divMainContent.Visible = false;
            txtSection.Text = "Select";
            txtSection.Enabled = false;
            if (chkSection.Checked == true)
            {
                foreach (ListItem li in cblSection.Items)
                {
                    li.Selected = true;
                }
                txtSection.Enabled = true;
                txtSection.Text = "Section(" + (cblSection.Items.Count) + ")";
            }
            else
            {
                foreach (ListItem li in cblSection.Items)
                {
                    li.Selected = false;
                }
                txtSection.Enabled = false;
                txtSection.Text = "Select";
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void cblSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divPopupAlert.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divMainContent.Visible = false;
            txtSection.Text = "Select";
            txtSection.Enabled = false;
            int commcount = 0;
            chkSection.Checked = false;
            foreach (ListItem li in cblSection.Items)
            {
                if (li.Selected)
                {
                    commcount++;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cblSection.Items.Count)
                {
                    chkSection.Checked = true;
                }
                txtSection.Enabled = true;
                txtSection.Text = "Section(" + Convert.ToString(commcount) + ")";
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlExamMonth_Selectchanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divPopupAlert.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divMainContent.Visible = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlSearchBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divPopupAlert.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divMainContent.Visible = false;
            if (chkSearchBy.Checked)
            {
                divSearch.Visible = true;
                ddlSearchBy.Enabled = true;
                txtSearchBy.Enabled = true;
            }
            if (ddlSearchBy.Items.Count > 0)
            {
                lblSearch.Text = Convert.ToString(ddlSearchBy.SelectedItem.Text).Trim();
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void chkSearchBy_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divPopupAlert.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divMainContent.Visible = false;
            ddlSearchBy.Enabled = false;
            txtSearchBy.Enabled = false;
            txtSearchBy.Text = string.Empty;
            if (chkSearchBy.Checked)
            {
                divSearch.Visible = true;
                ddlSearchBy.Enabled = true;
                txtSearchBy.Enabled = true;
            }
            if (ddlSearchBy.Items.Count > 0)
            {
                lblSearch.Text = Convert.ToString(ddlSearchBy.SelectedItem.Text).Trim();
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void FpRedoStudentsList_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            FpRedoStudentsList.SaveChanges();
            int r = FpRedoStudentsList.Sheets[0].ActiveRow;
            int j = FpRedoStudentsList.Sheets[0].ActiveColumn;
            if (r == 0 && j == 0)
            {
                int val = 0;
                int.TryParse(Convert.ToString(FpRedoStudentsList.Sheets[0].Cells[r, j].Value).Trim(), out val);
                for (int row = 1; row < FpRedoStudentsList.Sheets[0].RowCount; row++)
                {
                    if (val == 1)
                        FpRedoStudentsList.Sheets[0].Cells[row, j].Value = 1;
                    else
                        FpRedoStudentsList.Sheets[0].Cells[row, j].Value = 0;
                }
            }
        }
        catch
        {

        }
    }

    #endregion DropDown Events

    #region Button Events

    #region Close Popup

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divPopupAlert.Visible = false;
            lblAlertMsg.Text = string.Empty;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Close Popup

    #region Go Click

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divPopupAlert.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divMainContent.Visible = false;
            string qryRedoSem = string.Empty;
            string qrySelectedSearch = string.Empty;
            ddlRedoBatch.Items.Clear();
            //ddlRedoSem.Items.Clear();
            lblRedoSemester.Text = string.Empty;
            DataSet dsRedoAllBatch = new DataSet();
            if (chkSearchBy.Checked)
            {
                if (ddlSearchBy.Items.Count > 0)
                {
                    string selVaues = Convert.ToString(ddlSearchBy.SelectedValue).Trim();
                    string studQ = string.Empty;
                    if (!string.IsNullOrEmpty(txtSearchBy.Text.Trim().ToLower()))
                    {
                        switch (selVaues)
                        {
                            case "0":
                                qrySelectedSearch = " and r.Roll_No='" + txtSearchBy.Text.Trim() + "'";
                                break;
                            case "1":
                                qrySelectedSearch = " and r.Reg_no='" + txtSearchBy.Text.Trim() + "'";
                                break;
                            case "2":
                                qrySelectedSearch = " and r.Roll_Admit='" + txtSearchBy.Text.Trim() + "'";
                                break;
                            default:
                                qrySelectedSearch = " and r.Roll_No='" + txtSearchBy.Text.Trim() + "'";
                                break;
                        }
                    }
                    else
                    {
                        lblAlertMsg.Text = "Please Enter " + ddlSearchBy.SelectedItem.Text.Trim() + " and Then Proceed";
                        lblAlertMsg.Visible = true;
                        divPopupAlert.Visible = true;
                        return;
                    }
                    studQ = "select * from Registration r where cc='1' " + qrySelectedSearch;
                    DataSet dsStud = d2.select_method_wo_parameter(studQ, "Text");
                    if (dsStud.Tables.Count > 0 && dsStud.Tables[0].Rows.Count > 0)
                    {
                    }
                    else
                    {
                        lblAlertMsg.Text = "Please Check " + ddlSearchBy.SelectedItem.Text.Trim() + " and Student Detail";
                        lblAlertMsg.Visible = true;
                        divPopupAlert.Visible = true;
                        return;
                    }
                }
            }
            else
            {
                bindRedoBatchYear();
                dsRedoAllBatch = GetRedoBatchYear();
                selected = 0;
                qryCollegeCode = string.Empty;
                collegeCodeNew = string.Empty;
                if (ddlCollege.Items.Count == 0)
                {
                    lblAlertMsg.Text = "No " + ((isSchool) ? "School" : "College") + " were Found";
                    lblAlertMsg.Visible = true;
                    divPopupAlert.Visible = true;
                    return;
                }
                else
                {
                    foreach (ListItem li in ddlCollege.Items)
                    {
                        if (li.Selected)
                        {
                            selected++;
                            if (string.IsNullOrEmpty(collegeCodeNew.Trim()))
                            {
                                collegeCodeNew = "'" + li.Value.Trim() + "'";
                            }
                            else
                            {
                                collegeCodeNew += ",'" + li.Value.Trim() + "'";
                            }
                        }
                    }
                    if (selected == 0)
                    {
                        lblAlertMsg.Text = "Please Select Atleast One " + ((isSchool) ? "School" : "College");
                        lblAlertMsg.Visible = true;
                        divPopupAlert.Visible = true;
                        return;
                    }
                    if (!string.IsNullOrEmpty(collegeCodeNew))
                    {
                        qryCollegeCode = " and r.college_code in(" + collegeCodeNew + ")";
                    }
                }

                selected = 0;
                qryBatchYear = string.Empty;
                batchYear = string.Empty;
                if (ddlBatch.Items.Count == 0)
                {
                    lblAlertMsg.Text = "No " + ((isSchool) ? "Year" : " Batch") + " were Found";
                    lblAlertMsg.Visible = true;
                    divPopupAlert.Visible = true;
                    return;
                }
                else
                {
                    foreach (ListItem li in ddlBatch.Items)
                    {
                        if (li.Selected)
                        {
                            selected++;
                            if (string.IsNullOrEmpty(batchYear.Trim()))
                            {
                                batchYear = "'" + li.Value.Trim() + "'";
                            }
                            else
                            {
                                batchYear += ",'" + li.Value.Trim() + "'";
                            }
                        }
                    }
                    if (selected == 0)
                    {
                        lblAlertMsg.Text = "Please Select Atleast One " + ((isSchool) ? "Year" : " Batch");
                        lblAlertMsg.Visible = true;
                        divPopupAlert.Visible = true;
                        return;
                    }
                    if (!string.IsNullOrEmpty(batchYear))
                    {
                        qryBatchYear = " and r.Batch_year in(" + batchYear + ")";
                    }
                }

                selected = 0;
                qryCourseId = string.Empty;
                courseId = string.Empty;
                if (ddlDegree.Items.Count == 0)
                {
                    lblAlertMsg.Text = "No " + ((isSchool) ? "School Type" : "Degree") + " were Found";
                    lblAlertMsg.Visible = true;
                    divPopupAlert.Visible = true;
                    return;
                }
                else
                {
                    foreach (ListItem li in ddlDegree.Items)
                    {
                        if (li.Selected)
                        {
                            selected++;
                            if (string.IsNullOrEmpty(courseId.Trim()))
                            {
                                courseId = "'" + li.Value.Trim() + "'";
                            }
                            else
                            {
                                courseId += ",'" + li.Value.Trim() + "'";
                            }
                        }
                    }
                    if (selected == 0)
                    {
                        lblAlertMsg.Text = "Please Select Atleast One " + ((isSchool) ? "School Type" : "Degree");
                        lblAlertMsg.Visible = true;
                        divPopupAlert.Visible = true;
                        return;
                    }
                    if (!string.IsNullOrEmpty(courseId))
                    {
                        qryCourseId = string.Empty;
                    }
                }

                selected = 0;
                qryDegreeCode = string.Empty;
                degreeCode = string.Empty;
                if (ddlBranch.Items.Count == 0)
                {
                    lblAlertMsg.Text = "No " + ((isSchool) ? "Standard" : "Department") + " were Found";
                    lblAlertMsg.Visible = true;
                    divPopupAlert.Visible = true;
                    return;
                }
                else
                {
                    foreach (ListItem li in ddlBranch.Items)
                    {
                        if (li.Selected)
                        {
                            selected++;
                            if (string.IsNullOrEmpty(degreeCode.Trim()))
                            {
                                degreeCode = "'" + li.Value.Trim() + "'";
                            }
                            else
                            {
                                degreeCode += ",'" + li.Value.Trim() + "'";
                            }
                        }
                    }
                    if (selected == 0)
                    {
                        lblAlertMsg.Text = "Please Select Atleast One " + ((isSchool) ? "Standard" : "Department");
                        lblAlertMsg.Visible = true;
                        divPopupAlert.Visible = true;
                        return;
                    }
                    if (!string.IsNullOrEmpty(degreeCode))
                    {
                        qryDegreeCode = " and r.degree_code in(" + degreeCode + ")";
                    }
                }

                selected = 0;
                qrySemester = string.Empty;
                semester = string.Empty;
                if (ddlSem.Items.Count == 0)
                {
                    lblAlertMsg.Text = "No " + ((isSchool) ? "Term" : " Semester") + " were Found";
                    lblAlertMsg.Visible = true;
                    divPopupAlert.Visible = true;
                    return;
                }
                else
                {
                    foreach (ListItem li in ddlSem.Items)
                    {
                        if (li.Selected)
                        {
                            selected++;
                            if (string.IsNullOrEmpty(semester.Trim()))
                            {
                                semester = "'" + li.Value.Trim() + "'";
                            }
                            else
                            {
                                semester += ",'" + li.Value.Trim() + "'";
                            }
                        }
                    }
                    if (selected == 0)
                    {
                        lblAlertMsg.Text = "Please Select Atleast One " + ((isSchool) ? "Term" : " Semester");
                        lblAlertMsg.Visible = true;
                        divPopupAlert.Visible = true;
                        return;
                    }
                    if (!string.IsNullOrEmpty(semester))
                    {
                        qrySemester = " and el.Semester in(" + semester + ")";
                        qryRedoSem = " and Semester in(" + semester + ")";
                    }
                }

                selected = 0;
                qrySection = string.Empty;
                section = string.Empty;
                if (cblSection.Items.Count > 0)
                {
                    foreach (ListItem li in cblSection.Items)
                    {
                        if (li.Selected)
                        {
                            selected++;
                            if (string.IsNullOrEmpty(section.Trim()))
                            {
                                section = "'" + li.Value.Trim() + "'";
                            }
                            else
                            {
                                section += ",'" + li.Value.Trim() + "'";
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(section))
                    {
                        qrySection = " and ltrim(rtrim(isnull(r.sections,''))) in(" + section + ")";
                    }
                }
            }

            if (!string.IsNullOrEmpty(qrySelectedSearch.Trim()) || (!string.IsNullOrEmpty(qryCollegeCode.Trim()) && !string.IsNullOrEmpty(qryBatchYear.Trim()) && !string.IsNullOrEmpty(qryDegreeCode.Trim()) && !string.IsNullOrEmpty(qrySemester.Trim())))
            {
                string qry = "select r.App_no,r.Adm_Date updateedDate,r.Roll_no,r.Reg_No as RegNo,r.college_code,r.Batch_Year as BatchYear,r.degree_code as degreeCode,r.Current_Semester as semester,Ltrim(Rtrim(isnull(r.sections,''))) sections,'1' isLatest from Registration r where r.Roll_No in(select Roll_no from Eligibility_list where is_eligible=3" + qrySemester + ") " + qryCollegeCode + qryBatchYear + qryDegreeCode + qrySection + qrySelectedSearch;
                qry = "select LTRIM(RTRIM(ISNULL(c.type,''))) type,c.Edu_Level,c.Course_Name,dt.Dept_Name,dt.dept_acronym,case when(ltrim(rtrim(isnull(c.type,'')))<>'') then ltrim(rtrim(isnull(c.type,'')))+case when(LTRIM(RTRIM(ISNULL(c.Edu_Level,'')))<>'') then ' '+c.Edu_Level+' '+c.Course_Name+' '+dt.dept_acronym else c.Course_Name+' '+dt.dept_acronym end  else case when(LTRIM(RTRIM(ISNULL(c.Edu_Level,'')))<>'') then c.Edu_Level+' '+c.Course_Name+' '+dt.dept_acronym else c.Course_Name+' '+dt.dept_acronym end end as DegreeDetails,r.college_code,r.Batch_Year,r.degree_code,r.Current_Semester,r.App_No,r.Reg_No,r.Roll_No,r.Roll_Admit,r.Stud_Name,r.Stud_Type,LTRIM(RTRIM(ISNULL(r.Sections,''))) as Sections,el.Semester as RedoSem,el.batch_year as RedoBatch,el.degree_code as redoDegreeCode from Registration r,Course c,Degree dg,Department dt,Eligibility_list el where r.degree_code=dg.Degree_Code and r.college_code=c.college_code and r.college_code=dg.college_code and r.college_code=dt.college_code and c.college_code=dt.college_code and dt.college_code=dg.college_code and c.college_code=dt.college_code and c.Course_Id=dg.Course_Id and dt.Dept_Code=dg.Dept_Code and el.app_no=r.App_No and r.Roll_No=el.Roll_no and el.is_eligible='3' and r.CC='1' and r.DelFlag=0 and r.Exam_Flag<>'debar' " + qryCollegeCode + qryBatchYear + qryDegreeCode + qrySection + qrySemester + qrySelectedSearch + " order by r.college_code,r.Batch_Year desc ,c.type,c.Edu_Level desc,r.degree_code,c.Course_Id,r.Current_Semester,r.Reg_No";
                DataSet dsStudentsList = new DataSet();
                DataSet dsStudentRedoDetails = new DataSet();

                dsStudentRedoDetails = d2.select_method_wo_parameter("select StudentRedoPk,Stud_AppNo,DegreeCode,BatchYear,Semester,Sections,RedoType,updatedDate from StudentRedoDetails where Stud_AppNo<>'0' " + qryRedoSem, "text");

                dsStudentsList = d2.select_method_wo_parameter(qry, "Text");
                if (dsStudentsList.Tables.Count > 0 && dsStudentsList.Tables[0].Rows.Count > 0)
                {
                    Init_Spread(FpRedoStudentsList);
                    FpRedoStudentsList.Sheets[0].RowCount = 0;
                    Farpoint.CheckBoxCellType chkCellAll = new Farpoint.CheckBoxCellType();
                    chkCellAll.AutoPostBack = true;
                    Farpoint.TextCellType txtCell = new Farpoint.TextCellType();
                    Farpoint.CheckBoxCellType chkCell = new Farpoint.CheckBoxCellType();
                    chkCell.AutoPostBack = false;
                    Farpoint.ComboBoxCellType comboCellBatch = new Farpoint.ComboBoxCellType();
                    Farpoint.ComboBoxCellType comboCellSem = new Farpoint.ComboBoxCellType();

                    FpRedoStudentsList.Sheets[0].RowCount++;
                    FpRedoStudentsList.Sheets[0].Columns[0].CellType = chkCell;
                    FpRedoStudentsList.Sheets[0].Cells[0, 0].CellType = chkCellAll;
                    FpRedoStudentsList.Sheets[0].Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpRedoStudentsList.Sheets[0].Cells[0, 0].VerticalAlign = VerticalAlign.Middle;
                    FpRedoStudentsList.Sheets[0].AddSpanCell(0, 1, 1, FpRedoStudentsList.Sheets[0].ColumnCount - 1);
                    FpRedoStudentsList.Sheets[0].FrozenRowCount = 1;
                    int serialNo = 0;
                    ArrayList arrSem = new ArrayList();
                    foreach (DataRow drStudent in dsStudentsList.Tables[0].Rows)
                    {
                        serialNo++;
                        string courseType = Convert.ToString(drStudent["type"]).Trim();
                        string eduLevel = Convert.ToString(drStudent["Edu_Level"]).Trim();
                        string courseName = Convert.ToString(drStudent["Course_Name"]).Trim();
                        string deptName = Convert.ToString(drStudent["Dept_Name"]).Trim();
                        string deptAcronymn = Convert.ToString(drStudent["dept_acronym"]).Trim();
                        string degreeName = Convert.ToString(drStudent["DegreeDetails"]).Trim();
                        string studentCollegeCode = Convert.ToString(drStudent["college_code"]).Trim();
                        string studentBatchYear = Convert.ToString(drStudent["Batch_Year"]).Trim();
                        string studentDegreeCode = Convert.ToString(drStudent["degree_code"]).Trim();
                        string studentCurrentSem = Convert.ToString(drStudent["Current_Semester"]).Trim();

                        string redoSem = Convert.ToString(drStudent["RedoSem"]).Trim();
                        string redoBatch = Convert.ToString(drStudent["RedoBatch"]).Trim();
                        string redoDegree = Convert.ToString(drStudent["redoDegreeCode"]).Trim();

                        string rollNo = Convert.ToString(drStudent["Roll_No"]).Trim();
                        string regNo = Convert.ToString(drStudent["Reg_No"]).Trim();
                        string appNo = Convert.ToString(drStudent["App_No"]).Trim();
                        string admitNo = Convert.ToString(drStudent["Roll_Admit"]).Trim();
                        string studentName = Convert.ToString(drStudent["Stud_Name"]).Trim();
                        string studentType = Convert.ToString(drStudent["Stud_Type"]).Trim();
                        string studentSection = Convert.ToString(drStudent["Sections"]).Trim();
                        string[] redoSemValue = new string[1];
                        redoSemValue[0] = redoSem;
                        comboCellSem = new Farpoint.ComboBoxCellType(redoSemValue, redoSemValue);

                        comboCellBatch = new Farpoint.ComboBoxCellType();
                        if (dsRedoAllBatch.Tables.Count > 0 && dsRedoAllBatch.Tables[0].Rows.Count > 0)
                        {
                            comboCellBatch.DataSource = dsRedoAllBatch;
                            comboCellBatch.DataTextField = "Batch_Year";
                            comboCellBatch.DataValueField = "Batch_Year";
                        }
                        else
                        {
                            qry = "select distinct Batch_Year from Registration r where r.CC='0' and r.DelFlag='0' and ISNULL(r.isRedo,'0')='0' and r.Exam_Flag<>'debar' and r.college_code='" + studentCollegeCode + "' and r.current_semester='" + redoSem + "'  and r.degree_code='" + studentDegreeCode + "' order by Batch_Year desc";
                            //qry = "select distinct Batch_Year from Registration r where r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' order by Batch_Year desc";
                            DataSet dsRedo = new DataSet();
                            dsRedo = d2.select_method_wo_parameter(qry, "text");
                            if (dsRedo.Tables.Count > 0 && dsRedo.Tables[0].Rows.Count > 0)
                            {
                                ddlRedoBatch.Items.Clear();
                                ddlRedoBatch.DataSource = dsRedo;
                                ddlRedoBatch.DataTextField = "Batch_Year";
                                ddlRedoBatch.DataValueField = "Batch_Year";
                                ddlRedoBatch.DataBind();
                                if (!arrSem.Contains(redoSem))
                                {
                                    //ddlRedoSem.Items.Insert(0, new ListItem(redoSem, redoSem));
                                    lblRedoSemester.Text = redoSem;
                                    arrSem.Add(redoSem);
                                }
                                comboCellBatch = new Farpoint.ComboBoxCellType();
                                comboCellBatch.DataSource = dsRedo;
                                comboCellBatch.DataTextField = "Batch_Year";
                                comboCellBatch.DataValueField = "Batch_Year";
                            }
                        }

                        FpRedoStudentsList.Sheets[0].RowCount++;
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 0].CellType = chkCell;
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 0].Locked = false;
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(serialNo).Trim();
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 1].CellType = txtCell;
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 1].Locked = true;
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(studentBatchYear).Trim();
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 2].CellType = txtCell;
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 2].Locked = true;
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(degreeName).Trim();
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 3].CellType = txtCell;
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(studentDegreeCode).Trim();
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 3].Locked = true;
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;

                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(studentCurrentSem).Trim();
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 4].CellType = txtCell;
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 4].Locked = true;
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(rollNo).Trim();
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(appNo).Trim();
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 5].CellType = txtCell;
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 5].Locked = true;
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;

                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(regNo).Trim();
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 6].Tag = Convert.ToString(studentSection).Trim();
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 6].CellType = txtCell;
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 6].Locked = true;
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;

                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(admitNo).Trim();
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 7].Tag = Convert.ToString(studentCollegeCode).Trim();
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 7].CellType = txtCell;
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 7].Locked = true;
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;

                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(studentName).Trim();
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 8].Tag = Convert.ToString(studentType).Trim();
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 8].CellType = txtCell;
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 8].Locked = true;
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;

                        DataView dvRedoDetails = new DataView();
                        if (dsStudentRedoDetails.Tables.Count > 0 && dsStudentRedoDetails.Tables[0].Rows.Count > 0)
                        {
                            dsStudentRedoDetails.Tables[0].DefaultView.RowFilter = "Stud_AppNo='" + appNo + "' and Semester='" + redoSem + "'";
                            dvRedoDetails = dsStudentRedoDetails.Tables[0].DefaultView;
                        }
                        if (dvRedoDetails.Count > 0)
                        {
                            FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(dvRedoDetails[0]["BatchYear"]).Trim();
                            FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 9].Value = Convert.ToString(dvRedoDetails[0]["BatchYear"]).Trim();
                            FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(dvRedoDetails[0]["Semester"]).Trim();
                            FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 10].Value = Convert.ToString(dvRedoDetails[0]["Semester"]).Trim();
                        }

                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 9].CellType = comboCellBatch;
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 9].Locked = false;
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 9].VerticalAlign = VerticalAlign.Middle;

                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 10].Text = redoSem;
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 10].Value = redoSem;
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 10].CellType = txtCell;
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 10].Locked = true;
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Center;
                        FpRedoStudentsList.Sheets[0].Cells[FpRedoStudentsList.Sheets[0].RowCount - 1, 10].VerticalAlign = VerticalAlign.Middle;

                    }
                    divMainContent.Visible = true;
                    FpRedoStudentsList.Sheets[0].PageSize = FpRedoStudentsList.Sheets[0].RowCount;
                    FpRedoStudentsList.Width = 980;
                    FpRedoStudentsList.Height = 500;
                    FpRedoStudentsList.SaveChanges();
                    FpRedoStudentsList.Visible = true;
                }
                else
                {
                    divMainContent.Visible = false;
                    lblAlertMsg.Text = "No Record(s) Were Found";
                    lblAlertMsg.Visible = true;
                    divPopupAlert.Visible = true;
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

    #endregion Go Click

    #region Print Excel

    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            Printcontrol1.Visible = false;
            string reportname = txtexcelname1.Text.Trim().Replace(" ", "_");
            if (Convert.ToString(reportname).Trim() != "")
            {
                if (FpRedoStudentsList.Visible == true)
                {
                    d2.printexcelreport(FpRedoStudentsList, reportname);
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
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion  Print Excel

    #region Print PDF

    protected void btnprintmaster1_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            string rptheadname = "Tabulated Marks/Results - Written Examination";
            string pagename = "TabulatedMarkResults.aspx";
            if (FpRedoStudentsList.Visible == true)
            {
                Printcontrol1.loadspreaddetails(FpRedoStudentsList, pagename, rptheadname.ToUpper());
            }
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

    #region Set Click

    protected void btnSet_Click(object sender, EventArgs e)
    {
        try
        {
            FpRedoStudentsList.SaveChanges();
            string redoBatch = txtRedoBatch.Text.Trim();
            string redoSemester = lblRedoSemester.Text.Trim();
            bool isSet = false;
            txtRedoBatch.Text = string.Empty;
            txtRedoSem.Text = string.Empty;
            if (ddlRedoBatch.Items.Count == 0)
            {
                lblAlertMsg.Text = "No Redo Batch Year Were Found for Selected Semester";
                divPopupAlert.Visible = true;
                return;
            }
            else
            {
                redoBatch = Convert.ToString(ddlRedoBatch.SelectedValue).Trim();
            }
            //if (ddlRedoSem.Items.Count == 0)
            //{
            //    lblAlertMsg.Text = "No Redo Semester Were Found";
            //    divPopupAlert.Visible = true;
            //    return;
            //}
            //else
            //{
            //    redoSemester = Convert.ToString(ddlRedoSem.SelectedValue).Trim();
            //}
            if (string.IsNullOrEmpty(redoSemester.Trim()))
            {
                lblAlertMsg.Text = "Please Select Redo Semester";
                divPopupAlert.Visible = true;
                return;
            }
            if (string.IsNullOrEmpty(redoBatch.Trim()))
            {
                lblAlertMsg.Text = "Please Select Redo Batch Year";
                divPopupAlert.Visible = true;
                return;
            }
            else
            {
                if (redoBatch.Trim() == "0")
                {
                    lblAlertMsg.Text = "Please Enter Valid Redo Batch Year Other Than 0";
                    divPopupAlert.Visible = true;
                    return;
                }
                if (redoSemester.Trim() == "0")
                {
                    lblAlertMsg.Text = "Please Enter Valid Redo Semester Other Than 0";
                    divPopupAlert.Visible = true;
                    return;
                }
                if (FpRedoStudentsList.Sheets[0].RowCount == 0)
                {
                    lblAlertMsg.Text = "No Record(s) Were Found";
                    divPopupAlert.Visible = true;
                    return;
                }
                else
                {
                    for (int row = 1; row < FpRedoStudentsList.Sheets[0].RowCount; row++)
                    {
                        int val = 0;
                        int.TryParse(Convert.ToString(FpRedoStudentsList.Sheets[0].Cells[row, 0].Value).Trim(), out val);
                        if (val == 1)
                        {
                            isSet = true;
                            Farpoint.ComboBoxCellType cell = (Farpoint.ComboBoxCellType)FpRedoStudentsList.Sheets[0].Cells[row, FpRedoStudentsList.Sheets[0].ColumnCount - 2].CellType;
                            string[] arrVal = cell.Items.ToArray();
                            if (arrVal.Contains(redoBatch))
                            {
                                FpRedoStudentsList.Sheets[0].Cells[row, FpRedoStudentsList.Sheets[0].ColumnCount - 2].Text = redoBatch;
                                FpRedoStudentsList.Sheets[0].Cells[row, FpRedoStudentsList.Sheets[0].ColumnCount - 2].Value = redoBatch;
                            }
                            //Farpoint.ComboBoxCellType cell1 = (Farpoint.ComboBoxCellType)FpRedoStudentsList.Sheets[0].Cells[row, FpRedoStudentsList.Sheets[0].ColumnCount - 1].CellType;
                            //string[] arrVal1 = cell1.Items.ToArray();
                            //if (arrVal1.Contains(redoSemester))
                            //{
                            //    FpRedoStudentsList.Sheets[0].Cells[row, FpRedoStudentsList.Sheets[0].ColumnCount - 1].Value = redoSemester;
                            //    FpRedoStudentsList.Sheets[0].Cells[row, FpRedoStudentsList.Sheets[0].ColumnCount - 1].Text = redoSemester;
                            //}
                        }
                        FpRedoStudentsList.Sheets[0].Cells[row, 0].Value = 0;
                    }
                    FpRedoStudentsList.SaveChanges();
                }
            }
            if (!isSet)
            {
                lblAlertMsg.Text = "Please Select Any Student And Then Proceed";
                divPopupAlert.Visible = true;
                return;
            }
        }
        catch
        {
        }
    }

    #endregion Set Click

    #region Save Click

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            FpRedoStudentsList.SaveChanges();
            txtRedoBatch.Text = string.Empty;
            txtRedoSem.Text = string.Empty;
            bool isSet = false;
            if (FpRedoStudentsList.Sheets[0].RowCount == 0)
            {
                lblAlertMsg.Text = "No Record(s) Were Found";
                divPopupAlert.Visible = true;
                return;
            }
            else
            {
                //bool isRaja = false;
                for (int row = 1; row < FpRedoStudentsList.Sheets[0].RowCount; row++)
                {
                    string batchYear = Convert.ToString(FpRedoStudentsList.Sheets[0].Cells[row, 2].Text).Trim();
                    string degCode = Convert.ToString(FpRedoStudentsList.Sheets[0].Cells[row, 3].Tag).Trim();
                    string currentSem = Convert.ToString(FpRedoStudentsList.Sheets[0].Cells[row, 4].Text).Trim();
                    string rollNo = Convert.ToString(FpRedoStudentsList.Sheets[0].Cells[row, 5].Text).Trim();
                    string appNo = Convert.ToString(FpRedoStudentsList.Sheets[0].Cells[row, 5].Tag).Trim();
                    string regNo = Convert.ToString(FpRedoStudentsList.Sheets[0].Cells[row, 6].Text).Trim();
                    string section = Convert.ToString(FpRedoStudentsList.Sheets[0].Cells[row, 6].Tag).Trim();
                    string admitNo = Convert.ToString(FpRedoStudentsList.Sheets[0].Cells[row, 7].Text).Trim();
                    string collegeCode = Convert.ToString(FpRedoStudentsList.Sheets[0].Cells[row, 7].Tag).Trim();
                    string redoBatch = Convert.ToString(FpRedoStudentsList.Sheets[0].Cells[row, 9].Text).Trim();
                    string redoSem = Convert.ToString(FpRedoStudentsList.Sheets[0].Cells[row, 10].Text).Trim();
                    string updatedDate = DateTime.Now.ToString("MM/dd/yyyy");
                    string updateQ = string.Empty;
                    if (!string.IsNullOrEmpty(redoBatch) && !string.IsNullOrEmpty(appNo) && !string.IsNullOrEmpty(redoSem) && !string.IsNullOrEmpty(currentSem) && currentSem.Trim() != "0" && redoBatch != "0" && redoSem.Trim() != "0")
                    {
                        int res = 0;
                        string qry = "if not exists (select StudentRedoPk,Stud_AppNo,DegreeCode,BatchYear,Semester,LTRIM(RTRIM(ISNULL(Sections,''))) as Sections,RedoType,updatedDate from StudentRedoDetails where Stud_AppNo='" + appNo + "' and DegreeCode='" + degCode + "' and BatchYear='" + batchYear + "' and Semester='" + currentSem + "' and LTRIM(RTRIM(ISNULL(Sections,'')))='" + section + "' and RedoType='1') insert into StudentRedoDetails (Stud_AppNo,DegreeCode,BatchYear,Semester,Sections,RedoType,updatedDate) values('" + appNo + "','" + degCode + "','" + batchYear + "','" + currentSem + "','" + section + "','1','" + updatedDate + "') ";
                        res = d2.update_method_wo_parameter(qry, "Text");

                        //if (true)
                        //{
                        //    string qry1 = "update Registration set Current_Semester='" + redoSem + "',isRedo='1' where App_No='" + appNo + "'";
                        //    res = d2.update_method_wo_parameter(qry1, "Text");
                        //    string studHis = "if exists (select * from StudentRegisterHistory where App_no='" + appNo + "' and DegreeCode='" + degCode + "' and BatchYear='" + batchYear + "' and Semester='" + currentSem + "' and LTRIM(RTRIM(ISNULL(Sections,'')))='" + section + "' and RedoType='1') update StudentRegisterHistory set isLatest='0' where App_no='" + appNo + "' and DegreeCode='" + degCode + "' and BatchYear='" + batchYear + "' and Semester='" + currentSem + "' and LTRIM(RTRIM(ISNULL(Sections,'')))='" + section + "' and RedoType='1'  insert into StudentRegisterHistory (App_no,updatedDate,Roll_no,RegNo,collegeCode,BatchYear,degreeCode,semester,sections,isLatest) values('" + appNo + "','" + updatedDate + "','" + rollNo + "','" + regNo + "','" + collegeCode + "','" + batchYear + "','" + degCode + "','" + redoSem + "','" + section + "','1') else insert into StudentRegisterHistory (App_no,updatedDate,Roll_no,RegNo,collegeCode,BatchYear,degreeCode,semester,sections,isLatest) values('" + appNo + "','" + updatedDate + "','" + rollNo + "','" + regNo + "','" + collegeCode + "','" + batchYear + "','" + degCode + "','" + redoSem + "','" + section + "','1')";

                        //    studHis = "if exists (select * from StudentRegisterHistory where App_no='" + appNo + "') begin update StudentRegisterHistory set isLatest='0' where App_no='" + appNo + "' if not exists (select * from StudentRegisterHistory where App_no='" + appNo + "' and BatchYear='" + batchYear + "' and degreeCode='" + degCode + "' and collegeCode='" + collegeCode + "' and semester='" + redoSem + "' and sections='" + section + "' and Roll_no='" + rollNo + "' and RegNo='" + regNo + "') begin insert into StudentRegisterHistory (App_no,updatedDate,Roll_no,RegNo,collegeCode,BatchYear,degreeCode,semester,sections,isLatest) values('" + appNo + "','" + updatedDate + "','" + rollNo + "','" + regNo + "','" + collegeCode + "','" + batchYear + "','" + degCode + "','" + redoSem + "','" + section + "','1') end  else begin update StudentRegisterHistory set isLatest='1' where App_no='" + appNo + "' and BatchYear='" + batchYear + "' and degreeCode='" + degCode + "' and collegeCode='" + collegeCode + "' and semester='" + redoSem + "' and sections='" + section + "' and Roll_no='" + rollNo + "' and RegNo='" + regNo + "' end  end  else begin insert into StudentRegisterHistory (App_no,updatedDate,Roll_no,RegNo,collegeCode,BatchYear,degreeCode,semester,sections,isLatest) values('" + appNo + "','" + updatedDate + "','" + rollNo + "','" + regNo + "','" + collegeCode + "','" + batchYear + "','" + degCode + "','" + redoSem + "','" + section + "','1') end ";
                        //    res = d2.update_method_wo_parameter(studHis, "Text");
                        //}
                        //else
                        //{
                        //string qry1 = "update Registration set Batch_Year='" + redoBatch + "',Current_Semester='" + redoSem + "',isRedo='1' where App_No='" + appNo + "'";
                        //res = d2.update_method_wo_parameter(qry1, "Text");
                        //}
                        //qry = string.Empty;
                        //qry = "UPDATE Registration SET	Batch_Year = '" + redoBatch + "',college_code = @NewCollegeCode,degree_code = @NewDegreeCode,Branch_code = @NewDeptCode,Current_Semester = @NewSemester,CC=@isPassedOut,mode = '1'WHERE App_No = @studentAppNo END ELSE UPDATE Registration SET	Batch_Year = @NewBatchYear,college_code = @NewCollegeCode,degree_code = @NewDegreeCode,Branch_code = @NewDeptCode,Current_Semester = @NewSemester,CC=@isPassedOut,mode = '1'WHERE App_No = @studentAppNo END ELSE BEGIN IF NOT EXISTS (SELECT r.App_No FROM RegistrationNew r WHERE r.Batch_Year = @OldBatchYear AND r.degree_code = @OldDegreeCode AND r.Current_Semester = @OldSemester AND r.App_No = @studentAppNo AND r.college_code = @OldCollegeCode)";

                        string admitDate = dirAcc.selectScalarString("select ISNULL(Adm_Date,GETDATE()) from Registration where App_No='" + appNo + "'");
                        string InsType = (!CheckSchoolOrCollege(collegeCode) ? "0" : "1");
                        //Dictionary<string, string> dicSQLParameter = new Dictionary<string, string>();
                        //dicSQLParameter.Clear();
                        //dicSQLParameter.Add("@OldCollegeCode", collegeCode.Trim());
                        //dicSQLParameter.Add("@OldBatchYear", batchYear);
                        //dicSQLParameter.Add("@OldDegreeCode", degCode);
                        //dicSQLParameter.Add("@OldSemester", currentSem.Trim());
                        //dicSQLParameter.Add("@NewCollegeCode", collegeCode.Trim());
                        //dicSQLParameter.Add("@NewBatchYear", redoBatch);
                        //dicSQLParameter.Add("@NewDegreeCode", degCode.Trim());
                        //dicSQLParameter.Add("@NewSemester", redoSem);
                        //dicSQLParameter.Add("@studentAppNo", appNo);
                        //dicSQLParameter.Add("@rollNo", rollNo);
                        //dicSQLParameter.Add("@regNo", regNo);
                        //dicSQLParameter.Add("@OldSection", section.Trim());
                        //dicSQLParameter.Add("@NewSection", "");
                        //dicSQLParameter.Add("@admitDate", admitDate);
                        //dicSQLParameter.Add("@schoolOrCollege", InsType);
                        //dicSQLParameter.Add("@redoType", "1");
                        //dicSQLParameter.Add("@isPassedOut", "0");
                        //res = storeAcc.updateData("uspStudentPromote", dicSQLParameter);
                        qry = string.Empty;

                        qry = "UPDATE Registration SET	Batch_Year ='" + redoBatch + "',college_code = '" + collegeCode.Trim() + "',degree_code = '" + degCode.Trim() + "',Current_Semester = '" + redoSem + "',CC=0,mode = '1' WHERE App_No = '" + appNo + "'";
                        res = d2.update_method_wo_parameter(qry, "Text");
                        qry = string.Empty;
                        qry = "UPDATE RegistrationNew SET	Batch_Year ='" + redoBatch + "',college_code = '" + collegeCode.Trim() + "',degree_code = '" + degCode.Trim() + "',Current_Semester = '" + redoSem + "',CC=0,mode = '1' WHERE App_No = '" + appNo + "'";
                        res = d2.update_method_wo_parameter(qry, "Text");
                        qry = string.Empty;
                        qry = "IF NOT EXISTS (SELECT idNo FROM StudentRegisterHistory WHERE App_no = '" + appNo + "' AND BatchYear = '" + redoBatch + "' AND degreeCode = '" + degCode.Trim() + "' AND collegeCode = '" + collegeCode.Trim() + "' AND semester = '" + redoSem + "' AND sections ='" + section + "' AND RedoType ='1' AND Roll_no = '" + rollNo + "' AND RegNo ='" + regNo + "') INSERT INTO StudentRegisterHistory (App_no, admissionDate, updatedDate, Roll_no, RegNo, collegeCode, BatchYear, degreeCode, semester, sections, isLatest, RedoType) VALUES ('" + appNo + "','" + admitDate + "', GETDATE(), '" + rollNo + "', '" + regNo + "', '" + collegeCode.Trim() + "','" + redoBatch + "', '" + degCode.Trim() + "', '" + redoSem + "', '" + section + "', '1', '1') ELSE  UPDATE StudentRegisterHistory SET	isLatest = '1', admissionDate = '" + admitDate + "', updatedDate = GETDATE(),RedoType = '1' WHERE App_no = '" + appNo + "' AND BatchYear = '" + redoBatch + "' AND degreeCode = '" + degCode.Trim() + "' AND collegeCode = '" + collegeCode.Trim() + "' AND semester = '" + redoSem + "' AND sections = '" + section + "' AND RedoType = '1' AND Roll_no = '" + rollNo + "' AND RegNo = '" + regNo + "' ";
                        res = d2.update_method_wo_parameter(qry, "Text");
                     
                        if (res > 0)
                            isSet = true;
                        #region Commented

                        //int res = 0;
                        //string qry = "if not exists (select StudentRedoPk,Stud_AppNo,DegreeCode,BatchYear,Semester,Sections,RedoType,updatedDate from StudentRedoDetails where Stud_AppNo='" + appNo + "' and RedoType='1') insert into StudentRedoDetails (Stud_AppNo,DegreeCode,BatchYear,Semester,Sections,RedoType,updatedDate) values('" + appNo + "','" + degCode + "','" + batchYear + "','" + currentSem + "','" + section + "','1','" + updatedDate + "') ";
                        //res = d2.update_method_wo_parameter(qry, "Text");
                        //if (res > 0)
                        //{
                        //    isSet = true;
                        //}
                        ////if (isRaja)
                        ////{
                        ////    string qry1 = "update Registration set Current_Semester='" + redoSem + "',isRedo='1' where App_No='" + appNo + "'";
                        ////    res = d2.update_method_wo_parameter(qry1, "Text");
                        ////    string studHis = "if exists (select * from StudentRegisterHistory where App_no='" + appNo + "') update StudentRegisterHistory set isLatest='0' where App_no='" + appNo + "' insert into StudentRegisterHistory (App_no,updatedDate,Roll_no,RegNo,collegeCode,BatchYear,degreeCode,semester,sections,isLatest) values('" + appNo + "','" + updatedDate + "','" + rollNo + "','" + regNo + "','" + collegeCode + "','" + batchYear + "','" + degCode + "','" + redoSem + "','" + section + "','1') else insert into StudentRegisterHistory (App_no,updatedDate,Roll_no,RegNo,collegeCode,BatchYear,degreeCode,semester,sections,isLatest) values('" + appNo + "','" + updatedDate + "','" + rollNo + "','" + regNo + "','" + collegeCode + "','" + batchYear + "','" + degCode + "','" + redoSem + "','" + section + "','1')";

                        ////    studHis = "if exists (select * from StudentRegisterHistory where App_no='" + appNo + "') begin update StudentRegisterHistory set isLatest='0' where App_no='" + appNo + "' if not exists (select * from StudentRegisterHistory where App_no='" + appNo + "' and BatchYear='" + batchYear + "' and degreeCode='" + degCode + "' and collegeCode='" + collegeCode + "' and semester='" + redoSem + "' and sections='" + section + "' and Roll_no='" + rollNo + "' and RegNo='" + regNo + "') begin insert into StudentRegisterHistory (App_no,updatedDate,Roll_no,RegNo,collegeCode,BatchYear,degreeCode,semester,sections,isLatest) values('" + appNo + "','" + updatedDate + "','" + rollNo + "','" + regNo + "','" + collegeCode + "','" + batchYear + "','" + degCode + "','" + redoSem + "','" + section + "','1') end  else begin update StudentRegisterHistory set isLatest='1' where App_no='" + appNo + "' and BatchYear='" + batchYear + "' and degreeCode='" + degCode + "' and collegeCode='" + collegeCode + "' and semester='" + redoSem + "' and sections='" + section + "' and Roll_no='" + rollNo + "' and RegNo='" + regNo + "' end  end  else begin insert into StudentRegisterHistory (App_no,updatedDate,Roll_no,RegNo,collegeCode,BatchYear,degreeCode,semester,sections,isLatest) values('" + appNo + "','" + updatedDate + "','" + rollNo + "','" + regNo + "','" + collegeCode + "','" + batchYear + "','" + degCode + "','" + redoSem + "','" + section + "','1') end ";
                        ////    res = d2.update_method_wo_parameter(studHis, "Text");
                        ////}
                        ////else
                        ////{
                        //string qry1 = "update Registration set Batch_Year='" + redoBatch + "',Current_Semester='" + redoSem + "',isRedo='1' where App_No='" + appNo + "'";
                        //res = d2.update_method_wo_parameter(qry1, "Text");
                        ////}

                        #endregion

                    }
                }
            }
            if (isSet)
            {
                lblAlertMsg.Text = "Saved Successfully";
                divPopupAlert.Visible = true;
                return;
            }
            else
            {
                lblAlertMsg.Text = "Not Saved";
                divPopupAlert.Visible = true;
                return;
            }
            btnGo_Click(sender, e);
        }
        catch
        {
        }
    }

    #endregion Save Click

    #endregion Button Events

    //Redo Completion -- Added by Idhris  10-09-2017

    protected void lnkRedoCompletion_Click(object sender, EventArgs e)
    {
        popwindow.Visible = true;
        bindclg1();
        txt_SearchBy.Text = string.Empty;
        btnSaveRedoComplete.Visible = false;
        retrieveSearch(false);
    }

    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
    }

    public void bindclg1()
    {
        try
        {
            ddl_college1.Items.Clear();
            string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + userCode + " and cp.college_code=cf.college_code";
            DataSet ds = d2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddl_college1.DataSource = ds;
                ddl_college1.DataTextField = "collname";
                ddl_college1.DataValueField = "college_code";
                ddl_college1.DataBind();
            }
        }
        catch (Exception ex) { }
    }

    protected void ddl_college1_OnIndexChange(object sender, EventArgs e)
    {
        collegecodestat = ddl_college1.SelectedValue;
        retrieveSearch(false);
    }

    protected void ddl_searchBy_OnIndexChange(object sender, EventArgs e)
    {
        txt_SearchBy.Text = string.Empty;
        if (ddl_searchBy.SelectedIndex == 0)
        {
            txt_SearchBy.Attributes.Add("placeholder", "Adm No");
            choosedmode = 0;
        }
        else if (ddl_searchBy.SelectedIndex == 1)
        {
            txt_SearchBy.Attributes.Add("placeholder", "Student Name");
            choosedmode = 1;
        }
        else if (ddl_searchBy.SelectedIndex == 2)
        {
            txt_SearchBy.Attributes.Add("placeholder", "Roll No");
            choosedmode = 2;
        }
        retrieveSearch(false);
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetSearch(string prefixText)
    {
        List<string> name = new List<string>();
        try
        {
            string query = "";
            WebService ws = new WebService();

            if (choosedmode == 0)
            {
                query = "select top 100 Roll_admit from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_admit like '" + prefixText + "%' and college_code=" + collegecodestat + " order by Roll_No asc";
            }
            else if (choosedmode == 1)
            {
                query = "select  top 100 stud_name from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and stud_name like '" + prefixText + "%' and college_code=" + collegecodestat + "  order by Reg_No asc";
            }
            else if (choosedmode == 2)
            {
                query = "select  top 100 Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%' and college_code=" + collegecodestat + "  order by Roll_admit asc";
            }

            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }

    protected void btn_go1_Click(object sender, EventArgs e)
    {
        retrieveSearch(true);
    }

    private void retrieveSearch(bool displayAlert)
    {
        try
        {
            btnSaveRedoComplete.Visible = false;
            gridDetails.DataSource = null;
            gridDetails.DataBind();
            gridDetails.Visible = false;

            string collegeCode = ddl_college1.Items.Count > 0 ? ddl_college1.SelectedValue : "";
            DataSet dsGrid = new DataSet();
            string searchBytxt = txt_SearchBy.Text.Trim();
            if (searchBytxt != string.Empty)
            {
                string selectquery = " select r.app_No,r.Roll_no,r.Reg_No,r.Roll_Admit,r.Stud_Name,r.Batch_Year,r.degree_code,(select (C.Course_Name +' - '+ dt.Dept_Name) from Degree d,Department dt,Course c where d.Course_Id=c.Course_Id and dt.Dept_Code=d.Dept_Code and d.Degree_Code=r.degree_code) as Department ,el.Semester from Eligibility_list el, Registration r where r.App_No=el.app_no and ISNULL(isCompleteRedo,'0')='0' and r.college_code='" + collegeCode + "' ";

                if (ddl_searchBy.SelectedIndex == 0)
                {
                    selectquery += " and r.roll_admit='" + searchBytxt + "'";
                }
                else if (ddl_searchBy.SelectedIndex == 1)
                {
                    selectquery += " and r.stud_name='" + searchBytxt + "'";
                }
                else if (ddl_searchBy.SelectedIndex == 2)
                {
                    selectquery += " and r.roll_no='" + searchBytxt + "'";
                }
                dsGrid = d2.select_method_wo_parameter(selectquery, "Text");

                if (dsGrid.Tables.Count > 0 && dsGrid.Tables[0].Rows.Count > 0)
                {
                    gridDetails.DataSource = dsGrid.Tables[0];
                    gridDetails.DataBind();
                    gridDetails.Visible = true;
                    btnSaveRedoComplete.Visible = true;
                }
                else
                {
                    if (displayAlert)
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No records found')", true);
                }
            }
            else
            {
                if (displayAlert)
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please enter student detail')", true);
            }
        }
        catch { }
    }

    protected void btnSaveRedoComplete_Click(object sender, EventArgs e)
    {
        try
        {
            int update = 0;

            foreach (GridViewRow gRow in gridDetails.Rows)
            {
                CheckBox cbSel = (CheckBox)gRow.FindControl("cb_select");
                if (cbSel.Checked)
                {
                    Label lblAppNo = (Label)gRow.FindControl("lbl_AppNo");
                    Label lblBatch = (Label)gRow.FindControl("lbl_Batch");
                    Label lblSem = (Label)gRow.FindControl("lbl_Sem");
                    Label lblDeg = (Label)gRow.FindControl("lbl_DegreeCode");
                    update += d2.update_method_wo_parameter(" update Eligibility_list set isCompleteRedo='1' where app_no='" + lblAppNo.Text + "' and batch_year='" + lblBatch.Text + "' and Semester='" + lblSem.Text + "' and degree_code='" + lblDeg.Text + "'", "TEXT");
                }
            }
            if (update > 0)
            {
                retrieveSearch(false);
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
            }
            else
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Not Saved')", true);
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please try later')", true);
        }
    }

    private bool CheckSchoolOrCollege(string collegeCode)
    {
        bool isSchoolOrCollege = false;
        try
        {
            if (!string.IsNullOrEmpty(collegeCode))
            {
                //qry = "select ISNULL(InstType,'0') as InstType,case when ISNULL(InstType,'0')='0' then 'College' when ISNULL(InstType,'0')='1' then 'School' end as CollegeOrSchool from collinfo where college_code='" + collegeCode + "'";
                string qry = "select ISNULL(InstType,'0') as InstType from collinfo where college_code='" + collegeCode + "'";
                string insType = dirAcc.selectScalarString(qry);
                if (!string.IsNullOrEmpty(insType) && insType.Trim() != "0")
                {
                    isSchoolOrCollege = true;
                }
                else
                {
                    isSchoolOrCollege = false;
                }

            }
            return isSchoolOrCollege;
        }
        catch
        {
            return false;
        }
    }

}